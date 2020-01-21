using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using SQLite;

namespace SowManager
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {

        public MainPage()
        {
            InitializeComponent();
        }

        async void SowsButtonClicked(object sender, EventArgs args)
        {
            // navigate to the sows page
            await Navigation.PushAsync(new Sows());
        }

        async void LittersButtonClicked(object sender, EventArgs args)
        {
            // navigate to the litters page
            await Navigation.PushAsync(new Litters());
        }

        // gets a list of all the TODO items
        // and populates the listview with them
        protected override void OnAppearing()
        {
            base.OnAppearing();
            TODOListView.ItemsSource = GetTODOList();
        }

        // creates a list of TODO items and returns it
        // first updates all active sows in the database
        // TODO add a loading icon while the sows are updating and TODO list is being created
        public List<TODOItem> GetTODOList()
        {
            // create TODO list
            List<TODOItem> todos = new List<TODOItem>();

            // update all sows
            using (SQLiteConnection conn = new SQLiteConnection(App.FilePath))
            {
                // select all the sows from the database and add them to the list view
                conn.CreateTable<Sow>();
                var sows = conn.Query<Sow>("select * from Sow where Alive = ?", true).ToList();
                foreach(Sow sow in sows)
                {
                    // move sows to new status if time elapsed
                    sow.Update();
                    TODOItem todo;

                    // if sow status = READY_TO_BREED, add a Breed button to the listview item
                    // if sow status = PENDING_ULTRA_SOUND, add a Ultrasound button to the listview item
                    // if sow status = DUE, add a Farrow button to the list view
                    // if any other status, don't add a button to the listview item
                    if(sow.Status == "READY_TO_BREED")
                    {
                        todo = new TODOItem(sow.ID, sow.SowNo, sow.Status, "Breed", true, "#3483EB");
                        todos.Add(todo);
                    }
                    else if(sow.Status == "PENDING_ULTRASOUND")
                    {
                        todo = new TODOItem(sow.ID, sow.SowNo, sow.Status, "Ultrasound", true, "#F65C78");
                        todos.Add(todo);
                    }
                    else if(sow.Status == "DUE")
                    {
                        todo = new TODOItem(sow.ID, sow.SowNo, sow.Status, "Farrow", true, "#A0C334");
                        todos.Add(todo);
                    }
                    else
                    {
                        todo = new TODOItem(sow.ID, sow.SowNo, sow.Status, "Text", false, "#000000");
                        todos.Add(todo);
                    }
                }
            }

            return todos;
        }

        // internal class to represent a TODO Item
        // used for creating a list for the listview
        public class TODOItem
        {
            // the ID of the sow the TODOItem refers to (could also be a litter id)
            public int SowID { get; set; }
            // the text to display on the TODOItem
            public string Name { get; set; }
            // the current status of the sow the TODOItem refers to
            public string Status { get; set; }
            // the text to go on the button on the listview item (if there is a button)
            public string ButtonText { get; set; }
            // whether to add a button to the listview item or not
            public Boolean IsVisible { get; set; }
            // the color of the button
            public string ButtonColor { get; set; }
            public TODOItem(int id, string name, string status, string buttonText, Boolean isVisible, string buttonColor)
            {
                this.SowID = id;
                this.Name = name;
                this.Status = status;
                this.ButtonText = buttonText;
                this.IsVisible = isVisible;
                this.ButtonColor = buttonColor;
            }
        }

        // when a TODOItem listview option is clicked
        // opens either the Breed Page, the Ultrasound Page, or the Farrow Page
        // depending on what status the sow is that the TODOItem refers to
        //
        // TODO currently, this method is run if the user taps anywhere on the list item
        //      it should only happen when they click on the button
        async void ActionItemTapped(object sender, ItemTappedEventArgs args)
        {
            // get the TODOItem from the list view
            var item = args.Item as TODOItem;

            // if the status of the TODOItem is:
            // - READY_TO_BREED, navigate to the Breed Page
            // - PENDING_ULTRASOUND, navigate to the Ultrasound Page
            // - DUE, navigate to the Farrow Page
            if(item.Status == "READY_TO_BREED")
            {
                await Navigation.PushAsync(new BreedSow(item.SowID));
            }
            else if(item.Status == "PENDING_ULTRASOUND")
            {
                await Navigation.PushAsync(new UltrasoundSow(item.SowID));
            }
            else if(item.Status == "DUE")
            {
                await Navigation.PushAsync(new FarrowSow(item.SowID));
            }
        }

        // temporary method for adding test data
        async void TestDataButtonClicked(object sender, EventArgs args)
        {
            // delete all sows
            using (SQLiteConnection conn = new SQLiteConnection(App.FilePath))
            {
                conn.CreateTable<Sow>();
                conn.CreateTable<Litter>();
                conn.CreateTable<Ultrasound>();

                conn.DeleteAll<Sow>();
                conn.DeleteAll<Litter>();
                conn.DeleteAll<Ultrasound>();

                // sows with statuses that need an action
                Sow sow1 = new Sow()
                {
                    SowNo = "4352",
                    KnickName = "Jim"
                };
                sow1.DetermineStatus();
                Sow sow2 = new Sow()
                {
                    SowNo = "4356",
                    KnickName = "Jimbo",
                    BredDate = DateTime.Now.AddDays(-27),
                    Status = "PENDING_ULTRASOUND"
                };
                Sow sow3 = new Sow()
                {
                    SowNo = "4354",
                    KnickName = "Jimmy",
                    BredDate = DateTime.Now.AddDays(-113)
                };
                sow3.DetermineStatus();

                // sows with status's that do not need to be actions
                Sow sow4 = new Sow()
                {
                    SowNo = "4111",
                    KnickName = "Jarge",
                    BredDate = DateTime.Now.AddDays(-10)
                };
                sow4.DetermineStatus();
                Sow sow5 = new Sow()
                {
                    SowNo = "4222",
                    KnickName = "Yorg",
                    BredDate = DateTime.Now.AddDays(-30)
                };
                sow5.DetermineStatus();
                Sow sow6 = new Sow()
                {
                    SowNo = "4333",
                    KnickName = "Gark",
                    BredDate = DateTime.Now.AddDays(-150),
                    FarrowedDate = DateTime.Now.AddDays(-5)
                };
                sow6.DetermineStatus();

                int rowsAdded;
                rowsAdded = conn.Insert(sow1);
                rowsAdded = conn.Insert(sow2);
                rowsAdded = conn.Insert(sow3);
                rowsAdded = conn.Insert(sow4);
                rowsAdded = conn.Insert(sow5);
                rowsAdded = conn.Insert(sow6);
            }
        }

    }
}
