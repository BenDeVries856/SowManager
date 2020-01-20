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
    public partial class AddSow : ContentPage
    {
        public AddSow()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            // set the date pickers to the current date when the page loads
            SowFarrowedDate.Date = DateTime.Now;
            SowBredDate.Date = DateTime.Now;
        }

        // Add Sow Button, adds a new sow to the database
        async void AddButtonClicked(object sender, EventArgs args)
        {
            // get user input from the form
            string sowno = SowNo.Text;
            string knickname = KnickName.Text;
            DateTime farrowed = SowFarrowedDate.Date;
            DateTime bred = SowBredDate.Date;
            DateTime due = bred.AddDays(113);
            DateTime usound = bred.AddDays(27);

            // TODO: determine status
            // if farrowed date > bred date:
            //  status = "READY_TO_BREED"
            // if bred date > farrowed date:
            //  status = "BRED"
            // use sow.update() instead after the sow object has been created

            // create a sow object with the input
            // leav status empty as it will be determined by sow.update()
            Sow sow = new Sow()
            {
                SowNo = sowno,
                KnickName = knickname,
                FarrowedDate = farrowed,
                BredDate = bred,
                DueDate = due,
                UltrasoundDate = usound,
            };
            sow.DetermineStatus();

            // insert sow into the database
            using(SQLiteConnection conn = new SQLiteConnection(App.FilePath))
            {
                conn.CreateTable<Sow>();
                int rowsAdded = conn.Insert(sow);
            }

            // return to the sows page
            await Navigation.PopAsync();
        }

    }
}