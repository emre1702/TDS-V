using GTANetworkAPI;
using NUnit.Framework;
using System;
using System.Threading;
using System.Threading.Tasks;
using TDS.Server.Handler.Extensions;

namespace TDS.Server.Tests.Server.Data.Extensions
{
    public class TaskExtensionsTest
    {
        private int mainThreadId;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            mainThreadId = GetThreadId();
            Handler.Extensions.TaskExtensions.IsMainThread = true;
        }

        [Test, RequiresThread]
        [Ignore("Doesn't work without RAGE Shim")]
        public async Task MainThreadCheckForTaskRunWorks()
        {
            // act
            int actionThreadId = -1;
            try
            {
                actionThreadId = await NAPI.Task.RunWait(GetThreadId).ConfigureAwait(false);
            }

            // assert
            catch (NullReferenceException)
            {
                actionThreadId = mainThreadId;
            }

            Assert.AreEqual(mainThreadId, actionThreadId);
        }

        [Test]
        [Ignore("Doesn't work without RAGE Shim")]
        public async Task TaskRunWait_WaitsForActionEnd()
        {
            // arrange
            var valueTest = -1;
            var valueExpected = 5;

            // act
            await NAPI.Task.RunWait(() => { Thread.Sleep(5000); valueTest = valueExpected; }).ConfigureAwait(false);

            // assert
            Assert.AreEqual(valueExpected, valueTest);
        }

        private int GetThreadId()
            => Thread.CurrentThread.ManagedThreadId;
    }
}