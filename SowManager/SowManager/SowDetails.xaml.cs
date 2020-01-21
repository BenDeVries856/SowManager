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
            SowStatus.Text = sow.Status;
            SowNo.Text = sow.SowNo;
            KnickName.Text = sow.KnickName;
            if(sow.FarrowedDate.HasValue)
                SowFarrowedDate.Date = (DateTime) sow.FarrowedDate;
            if(sow.BredDate.HasValue)
                SowBredDate.Date = (DateTime) sow.BredDate;
            if(sow.DueDate.HasValue)
                SowDueDate.Date = (DateTime) sow.DueDate;

            // only enable the breed button if the sow status is READY_TO_BREED
            if(sow.Status != "READY_TO_BREED")
            {
                BreedButton.IsEnabled = false;
            }

            // only enable the farrow button if the sow status is DUE
            if(sow.Status != "DUE")
            {
                FarrowButton.IsEnabled = false;
            }

            // only enable the ultrasound button if the sow status id PENDING_ULTRASOUND
            if(sow.Status != "PENDING_ULTRASOUND")
            {
                UltrasoundButton.IsEnabled = false;
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
                if(sow.Status == "DUE")
                {
                    FarrowButton.IsVisible = true;
                }
                if(sow.Status == "PENDING_ULTRASOUND")
                {
                    UltrasoundButton.IsVisible = true;
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
                sow.Status = "";
                // determine what the status is after the dates have been modified
                sow.DetermineStatus();
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
                FarrowButton.IsVisible = false;
                UltrasoundButton.IsVisible = false;
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

        async void FarrowButtonClicked(object sender, EventArgs args)
        {
            // Navigate to the Farrow Sow Page
            await Navigation.PushAsync(new FarrowSow(sow.ID));
        }

        async void UltrasoundButtonClicked(object sender, EventArgs args)
        {
            // Navigate to the Complete Ultrasound Page
            await Navigation.PushAsync(new UltrasoundSow(sow.ID));
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