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
    public partial class BreedSow : ContentPage
    {

        // the sow being bred
        private Sow sow;

        public BreedSow(int id)
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

            // set the date picker to the current date when the page loads
            // and set the sow number label
            SowNo.Text = sow.SowNo;
            SowBredDate.Date = DateTime.Now;
        }

        async void BreedButtonClicked(object sender, EventArgs args)
        {
            // breed the sow, pass in the date entered by the user
            sow.Breed(SowBredDate.Date);

            // update the sow in the database
            using (SQLiteConnection conn = new SQLiteConnection(App.FilePath))
            {
                conn.Update(sow);
            }

            // go back to the Sow Details Page
            await Navigation.PopAsync();
        }

    }
}