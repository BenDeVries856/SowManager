using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using SowManager;

namespace UnitTesting
{
    [TestClass]
    public class SowTest
    {

        [TestMethod]
        public void TestBreed()
        {
            DateTime referenceDate = DateTime.Now;
            Sow sow = new Sow()
            {
                SowNo = "1324",
                KnickName = "Jim",
                FarrowedDate = DateTime.Now.AddDays(-130),
                Status = "READY_TO_BREED"
            };
            sow.Breed(referenceDate);
            Assert.AreEqual("BRED", sow.Status);
            Assert.AreEqual(referenceDate, sow.BredDate);
            Assert.AreEqual(referenceDate.AddDays(113), sow.DueDate);
            Assert.AreEqual(referenceDate.AddDays(27), sow.UltrasoundDate);
            Assert.AreNotEqual(sow.BredDate, sow.FarrowedDate);
            Assert.IsTrue(sow.BredDate > sow.FarrowedDate);
        }

        // TODO add breed tests with bad data
        // - passing in a bad status, it shouls be "NO_STATUS"

    }
}
