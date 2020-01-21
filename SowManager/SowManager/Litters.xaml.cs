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
    public partial class Litters : ContentPage
    {
        public Litters()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            using (SQLiteConnection conn = new SQLiteConnection(App.FilePath))
            {
                // select all the sows from the database and add them to the list view
                conn.CreateTable<Litter>();
                var litters = conn.Table<Litter>().ToList();
                LittersListView.ItemsSource = litters;
            }
        }

    }
}