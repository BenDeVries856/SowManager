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
        public DateTime FarrowedDate
        {
            get;
            set;
        }

        // the date the user breeds the sow
        // once the sow is bred and this date is entered, 
        // the due date is determined by adding 113 days to the Bred Date
        // and the ultrasound is determined by adding 27 days
        public DateTime BredDate
        {
            get;
            set;
        }

        // the date the sow is expected to give birth
        // aka the next farrow date
        // 113 days after the bred date
        public DateTime DueDate
        {
            get;
            set;
        }

        // the date the sow should be checked to see if it is pregnant like it should be
        // 27 days after the bred date
        public DateTime UltrasoundDate
        {
            get;
            set;
        }

        // the status is used for determining tasks for the todo list
        // current statuses are:
        // - READY_TO_BREED
        // - BRED
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
            //
        }

        // breeds the sow
        // sets the bred date and determines the due date and ultrasound date based off of it
        // sets the status to BRED
        public void Breed(DateTime dateBred)
        {
            BredDate = dateBred;
            DueDate = dateBred.AddDays(113);
            UltrasoundDate = dateBred.AddDays(27);
            Status = "BRED";
        }

    }
}
