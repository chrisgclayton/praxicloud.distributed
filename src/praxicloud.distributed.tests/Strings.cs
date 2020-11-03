// Copyright (c) Chris Clayton. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace praxicloud.distributed.tests
{
    #region Using Clauses
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using praxicloud.distributed.indexes.strings;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    #endregion

    /// <summary>
    /// Validates functionality of the string index managers
    /// </summary>
    [TestClass]
    public class Strings
    {
        #region Methods
        /// <summary>
        /// Validates the basic functionality of the string index manager
        /// </summary>
        /// <param name="managerCount">The number of instances sharing in the index management</param>
        /// <param name="minimum">The minimum number of indexes</param>
        /// <param name="maximum">The maximum number of indexes</param>
        [DataTestMethod]
        [DataRow(1, 1, 1000)]
        [DataRow(1, 50, 100)]
        [DataRow(2, 1, 1000)]
        [DataRow(2, 50, 100)]
        [DataRow(3, 1, 1000)]
        [DataRow(3, 50, 100)]
        [DataRow(30, 1, 1000)]
        [DataRow(30, 50, 100)]
        [DataRow(500, 1, 1000)]
        [DataRow(500, 50, 100)]
        public void StringTestBasic(int managerCount, int minimum, int maximum)
        {
            var indexKeys = Enumerable.Range(minimum, maximum - minimum + 1).Select(item => item.ToString()).ToArray();


            var indexes = new List<string>();
            var indexOwnershipCount = new List<int>();
            var idTracker = new List<int>();
            var updateCount = 0;

            var manager = new StringIndexManager(managerCount, 0, indexKeys);

            manager.NotificationHandler = (manager, cancellationToken) =>
            {
                Interlocked.Increment(ref updateCount);
                return Task.CompletedTask;
            };

            manager.InitializeAsync(CancellationToken.None).GetAwaiter().GetResult();
            idTracker.Add(manager.Id);

            var ownedIndexes = manager.OwnedIndexes;
            indexes.AddRange(ownedIndexes);
            indexOwnershipCount.Add(ownedIndexes.Length);

            for (var index = 1; index < managerCount; index++)
            {
                manager.UpdateIdAsync(index, CancellationToken.None).GetAwaiter().GetResult();
                idTracker.Add(manager.Id);

                ownedIndexes = manager.OwnedIndexes;
                indexes.AddRange(ownedIndexes);

                indexOwnershipCount.Add(ownedIndexes.Length);
            }

            Assert.IsTrue(managerCount == manager.ManagerQuantity);
            Assert.IsTrue(manager.Indexes.Count() == maximum - minimum + 1);
            Assert.IsTrue(manager.IsIdUpdatable);
            Assert.IsTrue(manager.IsManagerQuantityUpdatable);
            Assert.IsTrue(manager.AreIndexesUpdatable);

            Assert.IsTrue(idTracker.Count == managerCount);
            Assert.IsTrue(idTracker.Distinct().Count() == managerCount);
            Assert.IsTrue(updateCount == managerCount - 1);
            Assert.IsTrue(indexes.Count() == maximum - minimum + 1);
            Assert.IsTrue(indexes.Distinct().Count() == maximum - minimum + 1);
            Assert.IsTrue(indexes.Select(item => int.TryParse(item, out var value) ? value : int.MaxValue).Min() == minimum);
            Assert.IsTrue(indexes.Select(item => int.TryParse(item, out var value) ? value : int.MinValue).Max() == maximum);

            Assert.IsTrue((indexOwnershipCount.Max() - indexOwnershipCount.Min()) <= 1);
        }

        /// <summary>
        /// Tests the string index manager and the behavior when changes occur
        /// </summary>
        [TestMethod]
        public void StringTestManagerQuantityChange()
        {
            var updateCount = 0;
            var updateSuccess = new List<bool>();
            var managerCount = new List<int>();
            var ownedLists = new Dictionary<int, List<string[]>>();
            var indexKeys = Enumerable.Range(1, 20).Select(item => item.ToString()).ToArray();

            var manager = new StringIndexManager(1, 0, indexKeys);
            managerCount.Add(manager.ManagerQuantity);

            manager.NotificationHandler = (manager, cancellationToken) =>
            {
                Interlocked.Increment(ref updateCount);
                return Task.CompletedTask;
            };

            manager.InitializeAsync(CancellationToken.None).GetAwaiter().GetResult();

            for(var index = 1; index <= 10; index++)
            {
                updateSuccess.Add(manager.UpdateManagerQuantityAsync(index, CancellationToken.None).GetAwaiter().GetResult());
                managerCount.Add(manager.ManagerQuantity);

                ownedLists.Add(index, new List<string[]>());

                for(var id = 0; id < 10; id++)
                {
                    manager.UpdateIdAsync(id, CancellationToken.None).GetAwaiter().GetResult();

                    ownedLists[index].Add(manager.OwnedIndexes);
                }
            }

            Assert.IsTrue(ownedLists.Count == 10);
            Assert.IsTrue(ownedLists.Sum(item => item.Value.Count()) == 100);
            Assert.IsTrue(updateCount == 110);                       

            var results = ownedLists.Select(pair =>
            {
                var balanceCheck = new List<int>();
                var errorFound = false;

                for(var entry = 0; entry < pair.Value.Count(); entry++)
                {
                    balanceCheck.Add(pair.Value.Count());

                    if(entry < pair.Key)
                    {
                        errorFound = pair.Value[entry].Count() == 0;
                    }
                    else
                    {
                        errorFound = pair.Value[entry].Count() > 0;
                    }
                }

                return errorFound;
            }).ToList();

            Assert.IsFalse(results.Any(item => item));

        }
        #endregion
    }
}
