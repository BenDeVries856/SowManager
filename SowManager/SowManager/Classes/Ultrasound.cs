using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace SowManager
{
    class Ultrasound
    {

        [PrimaryKey, AutoIncrement]
        public int ID
        {
            get;
            set;
        }

        // a link to the sow that the ultrasound is for
        public int SowID
        {
            get;
            set;
        }

        public DateTime UltrasoundDate
        {
            get;
            set;
        }

        public Ultrasound()
        {

        }

    }
}
