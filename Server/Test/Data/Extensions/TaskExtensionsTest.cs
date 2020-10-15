using GTANetworkAPI;
using NUnit.Framework;
using System;
using System.Threading;
using System.Threading.Tasks;
using TDS_Server.Handler.Extensions;

namespace TDS_Server.Tests.Data.Extensions
{
    internal class TaskExtensionsTest
    {
        private int mainThreadId;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            mainThreadId = GetThreadId();
            Handler.Extensions.TaskExtensions.IsMainThread = true;
        }

        [Test, RequiresThread]
        public async Task MainThreadCheckForTaskRunWorks()
        {
            // act
            int actionThreadId = -1;
            try
            {
                actionThreadId = await NAPI.Task.RunWait(GetThreadId);
            }

            // assert
            catch (NullReferenceException)
            {
                actionThreadId = mainThreadId;
            }

            Assert.AreEqual(mainThreadId, actionThreadId);
        }

        [Test]
        public async Task TaskRunWait_WaitsForActionEnd()
        {
            // arrange
            var valueTest = -1;
            var valueExpected = 5;

            // act
            await NAPI.Task.RunWait(() => { Thread.Sleep(5000); valueTest = valueExpected; });

            // assert
            Assert.AreEqual(valueExpected, valueTest);
        }

        private int GetThreadId()
            => Thread.CurrentThread.ManagedThreadId;
    }
}
