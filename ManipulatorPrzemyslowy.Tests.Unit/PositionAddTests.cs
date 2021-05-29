using System;
using NUnit.Framework;
using Moq;
using FluentAssertions;
using System.Threading;
using System.Windows.Forms;

namespace ManipulatorPrzemyslowy.Tests.Unit
{
    [TestFixture]
    [SetCulture("en-US")]
    public class PositionAddTests
    {
        [Test]
        [TestCase(new string[] {"1", "0.0", "0.0", "0.0", "0.0", "0.0", "0.0", "L", "B", "F", "O"}, true)]
        [TestCase(new string[] { "1", "a", "0.0", "0.0", "0.0", "0.0", "0.0", "L", "B", "F", "O" }, false)]
        [TestCase(new string[] { "1", "0.0", "0.0", "0.0", "0.0", "0.0", "0.0", "B", "R", "F", "O" }, false)]
        [TestCase(new string[] { "", "0.0", "0.0", "0.0", "0.0", "0.0", "0.0", "B", "R", "F", "O" }, false)]
        [TestCase(new string[] { "1", "1.0", "0.0", "0.0", "0.0", "0.0", "0.0", "B", "F", "O", "O" }, false)]
        public void CheckValueCorrectness_WhenValuesCorrect_ReturnTrue(string[] arr, bool expected)
        {
            CheckValues p = new CheckValues();

            bool validate = p.CheckValuesCorrectness(arr);

            validate.Should().Be(expected);

        }

        [Test]
        [TestCase(new string[] { "1", "0.0", "0.0", "0.0", "0.0", "0.0", "0.0", "L", "B", "F", "O", "A"}, true)]
        [TestCase(new string[] { "1", "0.0", "0.0", "0.0", "0.0", "0.0", "0.0", "L", "B"}, true)]
        public void CheckValueCorrectness_InvalidArrayLength_Exception(string[] arr, bool expected)
        {
            CheckValues p = new CheckValues();

            Action act = () => p.CheckValuesCorrectness(arr);

            act.Should().Throw<ArgumentException>();
        }

        [Test]
        [TestCase("1", true)]
        [TestCase("1.0", true)]
        [TestCase("1,0", true)]
        [TestCase("", false)]
        [TestCase("a", false)]
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
        [TestCase("1", 7, false)]
        [TestCase("", 8, false)]
        [TestCase("R", 9, false)]
        [TestCase("R", 10, false)]
        public void CheckCharCorrectness_WhenCharCorrect_ReturnTrue(string ch, int pos, bool expected)
        {
            CheckValues p = new CheckValues();

            bool validate = p.CheckCharCorrectness(ch, pos);

            validate.Should().Be(expected);
        }

        [Test]
        [TestCase(new string[] { "1", "1.0", "0.0", "0.0", "0.0", "0.0", "0.0", "L" }, false)]
        [TestCase(new string[] { "1", "2.0", "0.0", "0.0", "0.0", "0.0", "0.0", "L", "B", "F", "P" }, false)]
        [TestCase(new string[] {}, false)]
        public void SendPosition_IncorrectValues_Exception(string[] arr, bool expected)
        {

            ICheck checkMoq = Mock.Of<ICheck>(check => check.CheckValuesCorrectness(arr) == expected);

            PositionAddCommunication p = new PositionAddCommunication(checkMoq);

            Action act = () => p.SendPosition(arr);

            act.Should().Throw<ArgumentException>();
        }

        [Test]
        [TestCase(new string[] { "1", "2.0", "0.0", "0.0", "0.0", "0.0", "0.0", "L", "B", "F", "O" }, true, true)]
        [TestCase(new string[] { "1", "10.0", "-30.0", "+20.1", "0.0", "0.0", "0.0", "R", "A", "F", "O" }, true, true)]
        [TestCase(new string[] { "1", "0.0", "0.0", "0.0", "0.0", "0.0", "0.0", "L", "B", "F", "O", "O"}, false, false)]
        [TestCase(new string[] { "1", "1.0", "0.0", "0.0", "0.0", "0.0", "0.0", "B", "F", "O", "O" }, false, false)]
        [TestCase(new string[] { "", "0.0", "0.0", "0.0", "0.0", "0.0", "0.0", "L", "B", "F", "O" }, false, false)]
        public void OnSendPositiondIsCalled(string[] arr, bool expectedOut, bool expectedEvent)
        {
            ICheck checkMoq = Mock.Of<ICheck>(check => check.CheckValuesCorrectness(arr) == expectedOut);

            PositionAddCommunication p = new PositionAddCommunication(checkMoq);

            bool wasEventCalled = false;
            p.DataSend += (sender, args) => wasEventCalled = true;

            try
            {
                p.SendPosition(arr);
            }
            catch(ArgumentException)
            {
            }

            wasEventCalled.Should().Be(expectedEvent);

        }

    }
}
