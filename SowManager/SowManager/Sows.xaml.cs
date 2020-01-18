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
    public partial class Sows : ContentPage
    {

        public Sows()
        {
            InitializeComponent();
        }

        async void AddButtonClicked(object sender, EventArgs args)
        {
            // navigate to the Add Sow page
            await Navigation.PushAsync(new AddSow());
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            using (SQLiteConnection conn = new SQLiteConnection(App.FilePath))
            {
                // select all the sows from the database and add them to the list view
                conn.CreateTable<Sow>();
                var sows = conn.Table<Sow>().ToList();
                sowsListView.ItemsSource = sows;
            }
        }

        async void SowItemTapped(object sender, ItemTappedEventArgs args)
        {
            // when a sow list item is tapped, open the Sow Details page passing
            // in the id of the sow that was tapped
            var sow = args.Item as Sow;
            await Navigation.PushAsync(new SowDetails(sow.ID));
        }

    }
}