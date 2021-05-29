using System;
using NUnit.Framework;
using Moq;
using FluentAssertions;
using System.Threading;

namespace ManipulatorPrzemyslowy.Tests.Unit
{
    [TestFixture]
    public class PositionAddTests
    {
        [Test]
        [TestCase(new string[] {"1", "0.0", "0.0", "0.0", "0.0", "0.0", "0.0", "L", "B", "F", "O"}, true)]
        public void CheckValueCorrectness_WhenValuesCorrect_ReturnTrue(string[] arr, bool expected)
        {
            CheckValues p = new CheckValues();

            bool validate = p.CheckValuesCorrectness(arr);

            validate.Should().Be(expected);

        }

        [Test]
        [TestCase("1", true)]
        public void CheckNumberCorrectness_WhenNumberCorrect_ReturnTrue(string num, bool expected)
        {
            CheckValues p = new CheckValues();

            bool validate = p.CheckNumberCorrectness(num);

            validate.Should().Be(expected);
        }

        [Test]
        [TestCase("R", 7, true)]
        [TestCase("A", 8, true)]
        [TestCase("N", 9, true)]
        [TestCase("O", 10, true)]
        public void CheckCharCorrectness_WhenCharCorrect_ReturnTrue(string ch, int pos, bool expected)
        {
            CheckValues p = new CheckValues();

            bool validate = p.CheckCharCorrectness(ch, pos);

            validate.Should().Be(expected);
        }

        [Test]
        public void SendPosition_IncorrectValues_Exception(string[] arr)
        {

            ICheck checkMoq = Mock.Of<ICheck>(check => check.CheckValuesCorrectness(arr) == false);

            PositionAddCommunication p = new PositionAddCommunication(checkMoq);

            Action act = () => p.SendPosition(arr);

            act.Should().Throw<InvalidValueException>();
        }

        [Test]
        public void OnSendPositiondIsCalled(string[] arr, bool expectedEvent)
        {
            ICheck checkMoq = Mock.Of<ICheck>(check => check.CheckValuesCorrectness(arr) == true);

            PositionAddCommunication p = new PositionAddCommunication(checkMoq);

            bool wasEventCalled = false;
            p.DataSend += (sender, args) => wasEventCalled = true;

            p.SendPosition(arr);

            wasEventCalled.Should().Be(expectedEvent);

        }

    }
}
