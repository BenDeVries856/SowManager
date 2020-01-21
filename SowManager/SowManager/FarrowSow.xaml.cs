using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using SQLite;

namespace SowManager
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FarrowSow : ContentPage
    {

        /**
         * TODO
         * - make the inputs only accept numbers
         * - validation
         */

        // keeps track of the sow being farrowed
        private Sow sow;

        public FarrowSow(int id)
        {
            InitializeComponent();

            // select the sow from the database using the id passed in the constructor
            using (SQLiteConnection conn = new SQLiteConnection(App.FilePath))
            {
                this.sow = conn.Table<Sow>().Where(x => x.ID == id).SingleOrDefault();
            }
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            // set the sowno label
            SowNo.Text = sow.SowNo;
        }

        async void SubmitButtonClicked(object sender, EventArgs args)
        {
            // update the sow
            sow.Farrow(DateTime.Now);

            // get input from the form
            // TODO: validate this to make sure it is integers
            int alive = Int32.Parse(LitterAlive.Text);
            int dead = Int32.Parse(LitterDead.Text);
            int mummified = Int32.Parse(LitterMummified.Text);
            int fosteredin = Int32.Parse(LitterFosteredIn.Text);
            int fosteredout = Int32.Parse(LitterFosteredOut.Text);

            // create litter object with data from form
            Litter litter = new Litter()
            {
                SowID = sow.ID,
                Alive = alive,
                Dead = dead,
                Mummified = mummified,
                FosteredIn = fosteredin,
                FosteredOut = fosteredout
            };
            
            // insert new litter and update sow
            using (SQLiteConnection conn = new SQLiteConnection(App.FilePath))
            {
                conn.Update(sow);
                conn.CreateTable<Litter>();
                int rowsAdded = conn.Insert(litter);
            }

            // return to the sows page
            await Navigation.PopAsync();
        }

    }
}