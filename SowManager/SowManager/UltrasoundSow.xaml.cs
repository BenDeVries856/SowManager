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
    public partial class UltrasoundSow : ContentPage
    {

        // keeps track of the sow the ultrasound is for
        private Sow sow;

        public UltrasoundSow(int id)
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

            // set the sowno label and set the ultrasound date input to the current date
            SowNo.Text = sow.SowNo;
            UltrasoundDate.Date = DateTime.Now;
        }

        async void SubmitButtonClicked(object sender, EventArgs args)
        {
            // update the sow
            sow.Ultrasound(DateTime.Now);

            // get user input from the form
            DateTime ultrasoundDate = UltrasoundDate.Date;

            // create ultrasound object with data from form
            Ultrasound ultrasound = new Ultrasound()
            {
                SowID = sow.ID,
                UltrasoundDate = ultrasoundDate
            };

            // insert new ultrasound object and update sow
            using (SQLiteConnection conn = new SQLiteConnection(App.FilePath))
            {
                conn.Update(sow);
                conn.CreateTable<Ultrasound>();
                int rowsAdded = conn.Insert(ultrasound);
            }

            // return to the sows page
            await Navigation.PopAsync();
        }

    }
}