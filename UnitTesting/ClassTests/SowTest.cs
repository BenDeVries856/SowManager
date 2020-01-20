using Microsoft.VisualStudio.TestTools.UnitTesting;
using SowManager;
using System;

namespace UnitTesting
{
    [TestClass]
    public class SowTest
    {

        /**
         * TODO
         * - test whole lifecycle, 
         *      - create a sow that is ready to breed
         *      - instead of using the current date, use a variable that can be moved around
         *      - breed the sow then shift the time variable forward
         *      - do this for the rest of the cycle and test stuff along the way
         **/

        // test if a bad status is passed to a sow object
        [TestMethod]
        public void TestBadStatus()
        {
            Sow sow = new Sow()
            {
                SowNo = "1324",
                KnickName = "Jim",
                FarrowedDate = DateTime.Now,
                Status = "SDFFDS"
            };
            Assert.AreEqual("NO_STATUS", sow.Status);
        }

        // test a sow object that wasn't given any parameters
        [TestMethod]
        public void TestEmptySow()
        {
            Sow sow = new Sow();
            Assert.AreEqual("NO_STATUS", sow.Status);
        }

        // test the basic functionality of the breed function
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

        // test that the breed does not happen if not in the right status
        [TestMethod]
        public void TestBreedBadStatus()
        {
            DateTime referenceDate = DateTime.Now.AddDays(-130);
            Sow sow = new Sow()
            {
                SowNo = "1324",
                KnickName = "Jim",
                FarrowedDate = referenceDate,
                Status = "BRED"
            };
            string[] badStatuses = new string[]{"BRED", "PENDING_ULTRASOUND", "ULTRASOUND_COMPLETE", "DUE", "FARROWED", "NO_STATUS" };
            foreach(string status in badStatuses)
            {
                sow.Status = status;
                sow.Breed(DateTime.Now);
                Assert.AreEqual(status, sow.Status);
                Assert.AreEqual(referenceDate, sow.FarrowedDate);
            }
            
        }

        // test that the breed does not happen if the alloted time has not passed since farrow
        [TestMethod]
        public void TestBreedTimeElapsed()
        {
            // time elapsed should be 29 days
            DateTime referenceDate = DateTime.Now.AddDays(-27);
            Sow sow = new Sow()
            {
                SowNo = "1324",
                KnickName = "Jim",
                FarrowedDate = referenceDate,
                Status = "READY_TO_BREED"
            };
            sow.Breed(DateTime.Now);
            Assert.AreEqual("READY_TO_BREED", sow.Status);
            Assert.AreEqual(referenceDate, sow.FarrowedDate);
        }

        // TEST DetermineDate()
        // TODO modify tests to account for ultrasounds
        // TODO add test for farrowed date == bred date (which should test for an exception)

        // test no bred date and farrowed date
        // farrowed date >= 29 days from date = READY_TO_BREED
        [TestMethod]
        public void TestDetDate1()
        {
            Sow sow = new Sow()
            {
                SowNo = "1324",
                KnickName = "Jim",
                FarrowedDate = DateTime.Now.AddDays(-29)
            };
            sow.DetermineStatus();
            Assert.AreEqual("READY_TO_BREED", sow.Status);
        }

        // test no bred date and farrowed date
        // farrowed date < 29 days from date = FARROWED
        [TestMethod]
        public void TestDetDate2()
        {
            Sow sow = new Sow()
            {
                SowNo = "1324",
                KnickName = "Jim",
                FarrowedDate = DateTime.Now.AddDays(-28)
            };
            sow.DetermineStatus();
            Assert.AreEqual("FARROWED", sow.Status);
        }

        // test bred date and no farrowed date
        // if current date < 113 days after bred date = ULTRASOUND_COMPLETE
        [TestMethod]
        public void TestDetDate3()
        {
            Sow sow = new Sow()
            {
                SowNo = "1324",
                KnickName = "Jim",
                BredDate = DateTime.Now.AddDays(-112)
            };
            sow.DetermineStatus();
            Assert.AreEqual("ULTRASOUND_COMPLETE", sow.Status);
        }

        // test bred date and no farrowed date
        // if current date >= 113 days after bred date = DUE
        [TestMethod]
        public void TestDetDate4()
        {
            Sow sow = new Sow()
            {
                SowNo = "1324",
                KnickName = "Jim",
                BredDate = DateTime.Now.AddDays(-113)
            };
            sow.DetermineStatus();
            Assert.AreEqual("DUE", sow.Status);
        }

        // test both bred date and farrowed date
        // if bred date > farrowed date and current date >= 113 days after bred date = DUE
        [TestMethod]
        public void TestDetDate5()
        {
            Sow sow = new Sow()
            {
                SowNo = "1324",
                KnickName = "Jim",
                FarrowedDate = DateTime.Now.AddDays(-200),
                BredDate = DateTime.Now.AddDays(-113)
            };
            sow.DetermineStatus();
            Assert.AreEqual("DUE", sow.Status);
        }

        // test both bred date and farrowed date
        // if bred date > farrowed date and current date < 113 days after bred date = ULTRASOUND_COMPLETE
        [TestMethod]
        public void TestDetDate6()
        {
            Sow sow = new Sow()
            {
                SowNo = "1324",
                KnickName = "Jim",
                FarrowedDate = DateTime.Now.AddDays(-200),
                BredDate = DateTime.Now.AddDays(-112)
            };
            sow.DetermineStatus();
            Assert.AreEqual("ULTRASOUND_COMPLETE", sow.Status);
        }

        // test both bred date and farrowed date
        // if farrowed date > bred date and farrowed date >= 29 days from date = READY_TO_BREED
        [TestMethod]
        public void TestDetDate7()
        {
            Sow sow = new Sow()
            {
                SowNo = "1324",
                KnickName = "Jim",
                FarrowedDate = DateTime.Now.AddDays(-29),
                BredDate = DateTime.Now.AddDays(-150)
            };
            sow.DetermineStatus();
            Assert.AreEqual("READY_TO_BREED", sow.Status);
        }

        // test both bred date and farrowed date
        // if farrowed date > bred date and farrowed date < 29 days from date = FARROWED
        [TestMethod]
        public void TestDetDate8()
        {
            Sow sow = new Sow()
            {
                SowNo = "1324",
                KnickName = "Jim",
                FarrowedDate = DateTime.Now.AddDays(-28),
                BredDate = DateTime.Now.AddDays(-150)
            };
            sow.DetermineStatus();
            Assert.AreEqual("FARROWED", sow.Status);
        }

        // test neither
        [TestMethod]
        public void TestDetDate10()
        {
            Sow sow = new Sow()
            {
                SowNo = "1324",
                KnickName = "Jim",
            };
            sow.DetermineStatus();
            Assert.AreEqual("READY_TO_BREED", sow.Status);
        }

    }
}
