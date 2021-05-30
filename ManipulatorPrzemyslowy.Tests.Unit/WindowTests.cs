using System;
using System.Collections.Generic;
using System.Text;
using Moq;
using FluentAssertions;
using System.Threading;
using NUnit.Framework;
using System.Windows;


namespace ManipulatorPrzemyslowy.Tests.Unit
{
    [TestFixture(typeof(PositionAdd))]
    //[TestFixture(typeof(CommandTool))]
    [SetCulture("en-US")]
    class WindowTests<T> where T : Window, new()
    {
        [Test]
        public void test()
        {
            T win;
            
            var t = new Thread(() =>
            {
                win = new T();

                win.Closed += (s, e) => win.Dispatcher.InvokeShutdown();

                win.Show();

                System.Windows.Threading.Dispatcher.Run();

            });
            t.SetApartmentState(ApartmentState.STA);
            t.Start();
            t.Join();
        }

    }
}
