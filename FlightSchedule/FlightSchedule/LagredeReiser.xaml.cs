using FlightSchedule.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using DataLink;
using DataModel;

// The Basic Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234237

namespace FlightSchedule
{
    /// <summary>
    /// A basic page that provides characteristics common to most applications.
    /// </summary>
    public sealed partial class LagredeReiser : Page
    {

        private NavigationHelper navigationHelper;
        private ObservableDictionary defaultViewModel = new ObservableDictionary();

        /// <summary>
        /// This can be changed to a strongly typed view model.
        /// </summary>
        public ObservableDictionary DefaultViewModel
        {
            get { return this.defaultViewModel; }
        }

        /// <summary>
        /// NavigationHelper is used on each page to aid in navigation and 
        /// process lifetime management
        /// </summary>
        public NavigationHelper NavigationHelper
        {
            get { return this.navigationHelper; }
        }


        public LagredeReiser()
        {
            this.InitializeComponent();
            this.navigationHelper = new NavigationHelper(this);
            this.navigationHelper.LoadState += navigationHelper_LoadState;
            this.navigationHelper.SaveState += navigationHelper_SaveState;
            SqlList2.IsEnabled = false;
        }

        /// <summary>
        /// Populates the page with content passed during navigation. Any saved state is also
        /// provided when recreating a page from a prior session.
        /// </summary>
        /// <param name="sender">
        /// The source of the event; typically <see cref="NavigationHelper"/>
        /// </param>
        /// <param name="e">Event data that provides both the navigation parameter passed to
        /// <see cref="Frame.Navigate(Type, Object)"/> when this page was initially requested and
        /// a dictionary of state preserved by this page during an earlier
        /// session. The state will be null the first time a page is visited.</param>
        private void navigationHelper_LoadState(object sender, LoadStateEventArgs e)
        {
        }

        /// <summary>
        /// Preserves state associated with this page in case the application is suspended or the
        /// page is discarded from the navigation cache.  Values must conform to the serialization
        /// requirements of <see cref="SuspensionManager.SessionState"/>.
        /// </summary>
        /// <param name="sender">The source of the event; typically <see cref="NavigationHelper"/></param>
        /// <param name="e">Event data that provides an empty dictionary to be populated with
        /// serializable state.</param>
        private void navigationHelper_SaveState(object sender, SaveStateEventArgs e)
        {
        }

        #region NavigationHelper registration

        /// The methods provided in this section are simply used to allow
        /// NavigationHelper to respond to the page's navigation methods.
        /// 
        /// Page specific logic should be placed in event handlers for the  
        /// <see cref="GridCS.Common.NavigationHelper.LoadState"/>
        /// and <see cref="GridCS.Common.NavigationHelper.SaveState"/>.
        /// The navigation parameter is available in the LoadState method 
        /// in addition to page state preserved during an earlier session.

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            navigationHelper.OnNavigatedTo(e);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            navigationHelper.OnNavigatedFrom(e);
        }

        #endregion

        private void backButton_Tapped(object sender, TappedRoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(MainPage));
        }

        private void Button_Tapped(object sender, TappedRoutedEventArgs e)
        {
            
        }
        public async void UpdateSqlList()
        {
            SqlList2.Items.Clear();

            var b = await DataSource.GetReisesAsync();

            foreach (var item in b)
            {
                var a = new ListViewItem();
                a.Tag = item;
                a.Content = item.Dato + "        " + item.Tid + "        " + item.Navn + "        " + item.FlightId + "        " + item.Fra + "        " + item.Til + "        " + item.Flyselskap;
                SqlList2.Items.Add(a);
            }
            sqllistcount.Text = "Antall reiser i listen: " + SqlList2.Items.Count.ToString();
            sqllistcount.Visibility = Visibility.Visible;
            

        }
        private void Button_Tapped_1(object sender, TappedRoutedEventArgs e)
        {
            UpdateSqlList();
            SqlList2.IsEnabled = true;
            
        }

        private void SqlList2_Tapped(object sender, TappedRoutedEventArgs e)
        {
            var a = (ListViewItem)SqlList2.SelectedItem;        
            var item = (Reise)a.Tag;
            HubNavn.Header = item.Navn;
            HubDato.Header = "Dato: "+item.Dato;
            HubTid.Header = "Avgang: "+item.Tid;
            Hubflightid.Header = "Flight: " +item.FlightId;
            Hubflyselskap.Header = "Flyselskap: "+item.Flyselskap;
            FraHub.Header = "Fra: " + item.Fra;
            TilHub.Header = "Til: " + item.Til;
            UpdateReiseButton.Visibility = Visibility.Visible;
            
            
            


        }

        private async void Button_Tapped_2(object sender, TappedRoutedEventArgs e)
        {
            var a = (ListViewItem)SqlList2.SelectedItems[0];
            var travel = (Reise)a.Tag;
            await DataSource.DeleteReisesAsync(travel.Id);
            UpdateSqlList();
        }

        private async void Button_Tapped_3(object sender, TappedRoutedEventArgs e)
        {
            var a = (ListViewItem)SqlList2.SelectedItem;
            var travel = (Reise)a.Tag;

            int Id = travel.Id;
            var UpdateReise = new Reise()
            {
                Navn = editreisenavn.Text,
                Dato = travel.Dato,
                Tid = travel.Tid,
                FlightId = travel.FlightId,
                Fra = travel.Fra,
                Til = travel.Til,
                Flyselskap = travel.Flyselskap
            };
            await DataSource.EditReisesAsync(UpdateReise,Id);
            UpdateSqlList();
        }

        private void UpdateReiseButton_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (editreisenavn.Visibility == Visibility.Collapsed)
            { 
                editreisenavn.Visibility = Visibility.Visible;
                NameText.Visibility = Visibility.Visible;
                EditButton.Visibility = Visibility.Visible;
            }
            else 
            {
                editreisenavn.Visibility = Visibility.Collapsed;
                NameText.Visibility = Visibility.Collapsed;
                EditButton.Visibility = Visibility.Collapsed;
            }
     
        }

    }
}
