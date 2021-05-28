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
    class WindowTests
    {
        [Test]
        public void test()
        {
            PositionAdd win;

            var t = new Thread(() =>
            {
                win = new PositionAdd();

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
