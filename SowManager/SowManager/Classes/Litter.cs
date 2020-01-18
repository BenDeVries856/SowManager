using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace SowManager
{

    // A litter of pigs created when the due date is reached for a sow

    class Litter
    {

        // id is hidden from the user, only used in code
        [PrimaryKey, AutoIncrement]
        public int ID
        {
            get;
            set;
        }

        // the number of piglets that survived burth
        public int Alive
        {
            get;
            set;
        }

        // the number of piglets that did not make it (excluding mummified piglets)
        public int Dead
        {
            get;
            set;
        }

        // the number of piglets still inside the sac (dead)
        public int Mummified
        {
            get;
            set;
        }

        // piglets added to the litter from a different litter
        public int FosteredIn
        {
            get;
            set;
        }

        // piglets moved from this litter to another litter
        public int FosteredOut
        {
            get;
            set;
        }

        public Litter()
        {

        }

    }
}
