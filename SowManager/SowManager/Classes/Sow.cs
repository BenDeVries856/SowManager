using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace SowManager
{
    public class Sow
    {

        // the ID of the sow, not to be confused with the SowNo
        // the ID is hidden from the user and is only used in the code
        [PrimaryKey, AutoIncrement]
        public int ID
        {
            get;
            set;
        }

        // the Sow Number, not to be confused with the ID
        // the Sow Number is determined by the user
        public string SowNo
        {
            get;
            set;
        }

        // the Nickname of the sow
        // might remove in the future as barns can have thousands of sows
        // I didn't know how to spell Nickname when I started this app
        public string KnickName
        {
            get;
            set;
        }
        
        // the date a sow gives birth
        // on the farrow date, the user will have to enter information
        // about the litter the sow gave birth to in the Litter.cs class
        public DateTime? FarrowedDate
        {
            get;
            set;
        }

        // the date the user breeds the sow
        // once the sow is bred and this date is entered, 
        // the due date is determined by adding 113 days to the Bred Date
        // and the ultrasound is determined by adding 27 days
        public DateTime? BredDate
        {
            get;
            set;
        }

        // the date the sow is expected to give birth
        // aka the next farrow date
        // 113 days after the bred date
        public DateTime? DueDate
        {
            get;
            set;
        }

        // the date the sow should be checked to see if it is pregnant like it should be
        // 27 days after the bred date
        public DateTime? UltrasoundDate
        {
            get;
            set;
        }

        // the status is used for determining tasks for the todo list
        // current statuses are:
        // - READY_TO_BREED
        // - BRED
        // - PENDING_ULTRASOUND
        // - ULTRASOUND_COMPLETE
        // - DUE
        // - FARROWED
        // - NO_STATUS
        // TODO: add a document to explain each of the statuses
        // this is the variable that is stored in the database
        // the Status variable below is just the getter and setter
        private string status;

        // [Ignore] means it won't be stored in the sqlite database
        // makes sure the status entered is valid or sets it to NO_STATUS
        [Ignore]
        public string Status
        {
            get { return status; }
            set
            {
                switch (value)
                {
                    // check to see if a valid status was entered
                    case "READY_TO_BREED":
                    case "BRED":
                    case "PENDING_ULTRASOUND":
                    case "ULTRASOUND_COMPLETE":
                    case "DUE":
                    case "FARROWED":
                        status = value;
                        break;
                    // if not, set to NO_STATUS
                    default:
                        status = "NO_STATUS";
                        break;
                }
            }
        }

        public Sow()
        {
            Status = "NO_STATUS";
            FarrowedDate = null;
            BredDate = null;
            UltrasoundDate = null;
            DueDate = null;
        }


        // checks if enough time has elapsed to move the sow to the next status
        // if currently in a status that waits for time to elapse to move to the next status
        public void Update()
        {
            if(Status == "BRED")
            {
                // move to PENDING_ULTRA_SOUND
            }
            else if(Status == "ULTRASOUND_COMPLETE")
            {
                // move to DUE
            }
            else if(Status == "FARROWED")
            {
                // move to READY_TO_BREED
            }
        }

        // breeds the sow
        // sets the bred date and determines the due date and ultrasound date based off of it
        // sets the status to BRED
        // TODO:
        //  - make sure 29 days has passed since farrowed date
        //  - make sure the status is READY_TO_BREED
        public void Breed(DateTime dateBred)
        {
            if (Status == "READY_TO_BREED" && DaysBetween(dateBred, FarrowedDate) >= 29)
            {
                BredDate = dateBred;
                DueDate = dateBred.AddDays(113);
                UltrasoundDate = dateBred.AddDays(27);
                Status = "BRED";
            }
        }

        // determine what status the sow falls under if none was provided
        // TODO this method should account for ultrasounds when those are added
        public void DetermineStatus()
        {
            // only determine the status if there is not currently a status set
            if (string.IsNullOrEmpty(Status) || Status == "NO_STATUS")
            {
                // if the last bred date and farrowed date where provided
                if (BredDate.HasValue && FarrowedDate.HasValue)
                {
                    if (BredDate > FarrowedDate)
                    {
                        if (DaysBetween(BredDate, DateTime.Now) >= 113)
                            Status = "DUE";
                        else
                            Status = "ULTRASOUND_COMPLETE";
                    }
                    else if (FarrowedDate > BredDate)
                    {
                        if (DaysBetween(FarrowedDate, DateTime.Now) >= 29)
                            Status = "READY_TO_BREED";
                        else
                            Status = "FARROWED";
                    }
                    else
                    {
                        // TODO throw exception
                    }
                }
                // if only the last bred date was provided
                else if (BredDate.HasValue)
                {
                    if (DaysBetween(BredDate, DateTime.Now) >= 113)
                        Status = "DUE";
                    else
                        Status = "ULTRASOUND_COMPLETE";
                }
                // if only the farrowed date was provided
                else if (FarrowedDate.HasValue)
                {
                    if (DaysBetween(FarrowedDate, DateTime.Now) >= 29)
                        Status = "READY_TO_BREED";
                    else
                        Status = "FARROWED";
                }
                // if neither dates where provided
                else
                {
                    Status = "READY_TO_BREED";
                }
            }
        }

        // PRIVATE METHODS

        // gets the difference in days between two given dates
        // make sure the first date is larger to return a positive double
        // only use this method if both dates have already been checked to not be null
        private double DaysBetween(DateTime? date1, DateTime? date2)
        {
            // convert DateTime? to DateTime
            DateTime d1 = (DateTime) date1;
            DateTime d2 = (DateTime) date2;
            double days = (d1 - d2).TotalDays;
            if (d1 > d2) return days;
            else return days * -1;
        }

    }
}
