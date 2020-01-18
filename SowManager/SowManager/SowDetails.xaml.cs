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
    public partial class SowDetails : ContentPage
    {

        // this page displays all the information of a selected sow
        // this information is stored in the variable below
        private Sow sow;

        // toggles whether the inputs can be editted or not
        // default set to false, to activate, tap the Edit Info button
        // when activated all the inputs are enabled and the Edit Info button 
        // turns into Save Data button
        private Boolean editMode;

        public SowDetails(int id)
        {
            InitializeComponent();
            editMode = false;

            // using the id passed to this page, select the corresponding sow from the databases
            using (SQLiteConnection conn = new SQLiteConnection(App.FilePath))
            {
                this.sow = conn.Table<Sow>().Where(x => x.ID == id).SingleOrDefault();
            }
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            // plug the selected sows information into the page form
            SowNo.Text = sow.SowNo;
            KnickName.Text = sow.KnickName;
            SowFarrowedDate.Date = sow.FarrowedDate;
            SowBredDate.Date = sow.BredDate;
            SowDueDate.Date = sow.DueDate;

            // only enable to breed button if the sow status is READY_TO_BREED
            if(sow.Status != "READY_TO_BREED")
            {
                BreedButton.IsEnabled = false;
            }
        }

        async void EditButtonClicked(object sender, EventArgs args)
        {
            if (editMode)
            {
                // show the hidden buttons
                EditButton.Text = "Edit Info";
                if (sow.Status == "READY_TO_BREED")
                {
                    BreedButton.IsVisible = true;
                }
                RemoveButton.IsVisible = true;
                // disable the inputs
                KnickName.IsReadOnly = true;
                SowFarrowedDate.IsEnabled = false;
                SowBredDate.IsEnabled = false;
                SowDueDate.IsEnabled = false;
                // save changed data
                sow.KnickName = KnickName.Text;
                sow.FarrowedDate = SowFarrowedDate.Date;
                sow.BredDate = SowBredDate.Date;
                sow.DueDate = SowDueDate.Date;
                using (SQLiteConnection conn = new SQLiteConnection(App.FilePath))
                {
                    conn.Update(sow);
                }
            }
            else
            {
                // hide the buttons
                EditButton.Text = "Save";
                BreedButton.IsVisible = false;
                RemoveButton.IsVisible = false;
                // enable the inputs
                KnickName.IsReadOnly = false;
                SowFarrowedDate.IsEnabled = true;
                SowBredDate.IsEnabled = true;
                SowDueDate.IsEnabled = true;
            }
            // toggle Edit Mode
            editMode = !editMode;
        }

        async void BreedButtonClicked(object sender, EventArgs args)
        {
            // Navigate to the Breed Sow Page
            await Navigation.PushAsync(new BreedSow(sow.ID));
        }

        async void RemoveButtonClicked(object sender, EventArgs args)
        {
            // delete the currently displayed sow from the database
            using (SQLiteConnection conn = new SQLiteConnection(App.FilePath))
            {
                conn.Delete<Sow>(sow.ID);
            }

            // return to the sows page
            await Navigation.PopAsync();
        }

    }

}