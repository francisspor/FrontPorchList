using System.Linq;
using System.Threading;
using FrontPorchList;
using NUnit.Framework;

namespace Tests
{
    [TestFixture]
    public class StandardTests
    {
        [Test]
        public void Constructor_should_create_new_list()
        {
            var list = new ThreadSafeList<string>();
            Assert.IsNotNull(list);
            Assert.AreEqual(list.Count, 0);
        }

        [Test]
        public void List_allows_add_like_a_list_should()
        {
            var list = new ThreadSafeList<string> {"test string"};

            Assert.IsNotNull(list);
            Assert.AreEqual(list.Count, 1);
            Assert.AreEqual(list.First(), "test string");
        }

        [Test]
        public void List_allows_adds_an_int_and_it_stays_an_int()
        {
            var list = new ThreadSafeList<int> {1};

            Assert.IsNotNull(list);
            Assert.AreEqual(list.Count, 1);
            Assert.AreEqual(list.First(), 1);
            Assert.IsTrue(list.First() is int);
        }


        [Test]
        public void List_adds_and_then_removes()
        {
            var list = new ThreadSafeList<string> {"test string"};

            Assert.IsNotNull(list);
            Assert.AreEqual(list.Count, 1);
            Assert.AreEqual(list.First(), "test string");

            Assert.IsTrue(list.Contains("test string"));

            list.Remove("test string");
            Assert.AreEqual(list.Count, 0);
            Assert.AreEqual(list.FirstOrDefault(), null);

            Assert.IsFalse(list.Contains("test string"));
        }
    }

    [TestFixture]
    public class ThreadedTests
    {
        private ThreadSafeList<string> list;

        [SetUp]
        public void Setup()
        {
            list = new ThreadSafeList<string>();
        }
        /// <summary>
        /// This should insert MaxWorkers worth of ints into a list in a threaded way.
        /// </summary>
        [Test]
        public void List_Inserts_Well_Threadsafe()
        {
            const int MaxWorkers = 10;

            var list = new ThreadSafeList<int>();
            int remainingWorkers = MaxWorkers;
            var workCompletedEvent = new ManualResetEvent(false);
            for (int i = 0; i < MaxWorkers; i++)
            {
                int workerNum = i;  // Make a copy of local variable for next thread
                ThreadPool.QueueUserWorkItem(s =>
                {
                    list.Add(workerNum);
                    if (Interlocked.Decrement(ref remainingWorkers) == 0)
                    {
                        workCompletedEvent.Set();
                    }
                });
            }
            workCompletedEvent.WaitOne();
            workCompletedEvent.Close();
            for (int i = 0; i < MaxWorkers; i++)
            {
                Assert.IsTrue(list.Contains(i), "Element was not added");
            }
            Assert.AreEqual(MaxWorkers, list.Count,
                "List count does not match worker count.");
        }


        /// <summary>
        /// This is a test that should remove all of the items from the list, using a max of 10 workers.
        /// </summary>
        [Test]
        public void List_Removes_Well_Threadsafe()
        {
            const int MaxWorkers = 10;
            var list = new ThreadSafeList<int>();

            for (var i = 0; i < MaxWorkers; i++)
            {
                list.Add(i);
            }


            int remainingWorkers = MaxWorkers;
            var workCompletedEvent = new ManualResetEvent(false);
            for (int i = 0; i < MaxWorkers; i++)
            {
                int workerNum = i;  // Make a copy of local variable for next thread
                ThreadPool.QueueUserWorkItem(s =>
                {
                    list.Remove(workerNum);
                    if (Interlocked.Decrement(ref remainingWorkers) == 0)
                    {
                        workCompletedEvent.Set();
                    }
                });
            }
            workCompletedEvent.WaitOne();
            workCompletedEvent.Close();
            for (int i = 0; i < MaxWorkers; i++)
            {
                Assert.IsFalse(list.Contains(i), "Element wasn't removed");
            }
            Assert.AreEqual( list.Count, 0,
                "LIst should be empty.");
        }
    }
}