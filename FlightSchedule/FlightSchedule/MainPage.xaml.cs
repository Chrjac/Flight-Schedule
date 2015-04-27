using DataLink;
using DataModel;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using System.Xml.Linq;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace FlightSchedule
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
            Initzialize();
        }
        public void Initzialize(){
            AirportBox.Items.Add("Oslo");
            AirportBox.Items.Add("Bodø");
            AirportBox.Items.Add("Bergen");
            AirportBox.Items.Add("Kristiansand");
            AirportBox.Items.Add("Stavanger");
            AirportBox.Items.Add("Tromsø");
            AirportBox.Items.Add("Trondheim");
            AirportBox.Items.Add("Ålesund");
            ArrivalBox.Items.Add("Ankomster");
            ArrivalBox.Items.Add("Avganger");
            ListView.IsEnabled = false;
        }
        public void Main()
        {
            ListView.Items.Clear();
            string søkefelt = AirportBox.SelectedValue as string;
            int ArrivalDeparture = ArrivalBox.SelectedIndex;
            string Arrival = "A";
            string Departure = "D";
            string i = AirportNC.airportSearchCode(søkefelt);

            string ad = (ArrivalDeparture == 1) ? Departure : Arrival;

            string x = AviNorRequest.Request.DoRequest(i, ad);
            XDocument doc = XDocument.Parse(x);
            foreach (XElement flight in doc.Descendants("flight"))
            {
                string airline = flight.Element("airline").Value;
                string flightId = flight.Element("flight_id").Value;
                string schedule_time = flight.Element("schedule_time").Value;
                string airport = flight.Element("airport").Value;
                string schedule_time_sub = schedule_time.Substring(11);
                string schedule_time_sub2 = schedule_time_sub.TrimEnd('Z');
                string airportName = AirportCN.airportSearchName(airport);

                XDocument doccheckAirline = XDocument.Load("airlines.xml");
                foreach (XElement Alcheck in doccheckAirline.Descendants("airlineName"))
                {
                    string test = Alcheck.Attribute("code").Value;
                    if (test == airline)
                    {
                        test = Alcheck.Attribute("name").Value;
                        string from;
                        string to;
                        if (ad == Arrival) {
                            from = airportName;
                            to = AirportBox.SelectedValue as string;
                        }
                        else
                        {
                            from = AirportBox.SelectedValue as string;
                            to = airportName;
                        }
                        var a = new ListViewItem();                  
                        a.Tag = new Reise
                        {
                            FlightId = flightId,
                            Til = to,
                            Fra = from,
                            Flyselskap = test,
                            Tid = schedule_time_sub2
                        };
                        a.Content = schedule_time_sub2 + "          " + flightId + "          " + airportName + "          " + test;
                        ListView.Items.Add(a);
                    }
                    }
                }    
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var a = AirportBox.SelectedValue as string;
            int b = ArrivalBox.SelectedIndex;

            if (a == null) 
            {
                AirportErrorText.Visibility = Visibility.Visible; 
                AirportErrorText.Text = "Vennligst velg flyplass";
            }
            else 
            {
                AirportErrorText.Visibility = Visibility.Collapsed;
            }

            if (b == -1) 
            {
                DepartureErrorText.Visibility = Visibility.Visible;
                DepartureErrorText.Text = "Vennligst velg akn/avg"; 
                
            }
            else 
            {
                DepartureErrorText.Visibility = Visibility.Collapsed;
            }

            if (a != "null" && b != -1) {
                Main();
                ListView.IsEnabled = true;
            }

        }
        private void ListView_Tapped(object sender, TappedRoutedEventArgs e)
        {

            int ArrivalDeparture = ArrivalBox.SelectedIndex;
            var a = (ListViewItem)ListView.SelectedItem;
            var travel = (Reise)a.Tag;
            var date = DateTime.Now.ToString("dd.MM.yy"); 

            HubSection.Header = travel.FlightId;
            hubhead.Header = date;
            hubhead1.Header = travel.Tid;
            hubhead2.Header = travel.Fra;
            hubhead3.Header = travel.Til;
            hubhead4.Header = travel.Flyselskap;
       
            HubSection.Visibility = Visibility.Visible;
            StoreButton.Visibility = Visibility.Visible;
            reisenavntext.Visibility = Visibility.Visible;
   
        }

       
      
        private async void Button_Tapped(object sender, TappedRoutedEventArgs e)
        {
           
           var a = (ListViewItem)ListView.SelectedItems[0];
           var travel = (Reise)a.Tag;
           travel.Navn = reisenavntext.Text;
           travel.Dato = DateTime.Now.ToString("dd.MM.yy");
           if (reisenavntext.Text != "") 
           {
               await DataSource.AddReisesAsync(travel);
               NameErrorText.Visibility = Visibility.Collapsed;
           }
           else 
           {
               NameErrorText.Visibility = Visibility.Visible;
               NameErrorText.Text = "Angi reisenavn";
           }
           
        
        }

        private void Button_Tapped_1(object sender, TappedRoutedEventArgs e)
        {   
            this.Frame.Navigate(typeof(LagredeReiser));
        }
    }
}
