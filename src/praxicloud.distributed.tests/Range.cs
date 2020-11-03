// Copyright (c) Chris Clayton. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace praxicloud.distributed.tests
{
    #region Using Clauses
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using praxicloud.distributed.indexes;
    using praxicloud.distributed.indexes.range;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    #endregion
    
    /// <summary>
    /// Validates functionality of the range index managers
    /// </summary>
    [TestClass]
    public class Range
    {
        #region Methods
        /// <summary>
        /// Performs the basic functionality of the range index manager
        /// </summary>
        /// <param name="managerCount">The number of managers sharing management of the indexes</param>
        /// <param name="minimum">The minimum of the range</param>
        /// <param name="maximum">The maximum of the range</param>
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
        public void RangeTestBasic(int managerCount, int minimum, int maximum)
        {
            var indexes = new List<IndexRange<int>>();
            var indexOwnershipCount = new List<int>();
            var idTracker = new List<int>();
            var updateCount = 0;

            var manager = new Int32RangeIndexManager(managerCount, 0, minimum, maximum);

            manager.NotificationHandler = (manager, cancellationToken) =>
            {
                Interlocked.Increment(ref updateCount);
                return Task.CompletedTask;
            };

            manager.InitializeAsync(CancellationToken.None).GetAwaiter().GetResult();
            idTracker.Add(manager.Id);

            var ownedRange = manager.OwnedIndexRange;
            
            indexes.Add(ownedRange);
            indexOwnershipCount.Add(ownedRange == null ? 0 : ownedRange.Maximum - ownedRange.Minimum + 1);

            for (var index = 1; index < managerCount; index++)
            {
                manager.UpdateIdAsync(index, CancellationToken.None).GetAwaiter().GetResult();
                idTracker.Add(manager.Id);

                ownedRange = manager.OwnedIndexRange;
                indexes.Add(ownedRange);

                indexOwnershipCount.Add(ownedRange == null ? 0 : ownedRange.Maximum - ownedRange.Minimum + 1);
            }

            Assert.IsTrue(managerCount == manager.ManagerQuantity);
            Assert.IsTrue(minimum == manager.Minimum);
            Assert.IsTrue(maximum == manager.Maximum);
            Assert.IsTrue(manager.IsIdUpdatable);
            Assert.IsTrue(manager.IsManagerQuantityUpdatable);
            Assert.IsTrue(manager.AreIndexesUpdatable);

            Assert.IsTrue(idTracker.Count == managerCount);
            Assert.IsTrue(idTracker.Distinct().Count() == managerCount);
            Assert.IsTrue(updateCount == managerCount - 1);
            Assert.IsTrue(indexes.Count() == managerCount);
            Assert.IsTrue(indexes.Select(item => item != null ? item.Maximum - item.Minimum + 1 : 0).Sum() == maximum - minimum + 1);
            Assert.IsTrue(indexes.Min(item => item?.Minimum ?? int.MaxValue) == minimum);
            Assert.IsTrue(indexes.Max(item => item?.Maximum ?? int.MinValue) == maximum);

            Assert.IsTrue((indexOwnershipCount.Max() - indexOwnershipCount.Min()) <= 1);
        }
        #endregion
    }
}
