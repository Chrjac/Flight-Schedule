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
            string searchField = AirportBox.SelectedValue as string;
            int ArrivalDeparture = ArrivalBox.SelectedIndex;
           
            string Arrival = "A";
            string Departure = "D";
            string airportCode = AirportNC.airportSearchCode(searchField);

            string ad = (ArrivalDeparture == 1) ? Departure : Arrival;
            
            string aviNorResponse = AviNorRequest.Request.DoRequest(airportCode, ad);
            XDocument document = XDocument.Parse(aviNorResponse);
            XDocument doccheckAirline = XDocument.Load("airlines.xml");

            foreach (XElement flight in document.Descendants("flight"))
            {
                string airline = flight.Element("airline").Value;
                string flightId = flight.Element("flight_id").Value;
                string schedule_time = flight.Element("schedule_time").Value.Substring(11).TrimEnd('Z');
                string airport = flight.Element("airport").Value;
                string airportName = AirportCN.airportSearchName(airport);

                foreach (XElement airLineCheck in doccheckAirline.Descendants("airlineName"))
                {
                    string AirLineName;
                    string airLineCode = airLineCheck.Attribute("code").Value;
                    if (airLineCode == airline)
                    {
                        AirLineName = airLineCheck.Attribute("name").Value;
                        string from;
                        string to;
                        if (ad == Arrival) {
                            from = airportName;
                            to = searchField;
                        }
                        else
                        {
                            from = searchField;
                            to = airportName;
                        }
                        var a = new ListViewItem();                  
                        a.Tag = new Reise
                        {
                            FlightId = flightId,
                            Til = to,
                            Fra = from,
                            Flyselskap = AirLineName,
                            Tid = schedule_time
                        };
                        a.Content = schedule_time + "          " + flightId + "          " + airportName + "          " + AirLineName;
                        ListView.Items.Add(a);
                    }
                    }
                }    
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string airportSelector = AirportBox.SelectedValue as string;
            int arrivalDepature = ArrivalBox.SelectedIndex;
            

            if (airportSelector == null) 
            {
                AirportErrorText.Visibility = Visibility.Visible; 
                AirportErrorText.Text = "Vennligst velg flyplass";
            }
            else 
            {
                AirportErrorText.Visibility = Visibility.Collapsed;
            }

            if (arrivalDepature == -1) 
            {
                DepartureErrorText.Visibility = Visibility.Visible;
                DepartureErrorText.Text = "Vennligst velg akn/avg";
                
            }
            else 
            {
                DepartureErrorText.Visibility = Visibility.Collapsed;
            }

            if (airportSelector != "null" && arrivalDepature != -1) {
                Main();
                ListView.IsEnabled = true;
            }

        }
        private void ListView_Tapped(object sender, TappedRoutedEventArgs e)
        {

            var a = (ListViewItem)ListView.SelectedItem;
            var travel = (Reise)a.Tag;
            var date = DateTime.Now.ToString("dd.MM.yy"); 

            FlightText.Text = travel.FlightId;
            DateText.Text = date;
            TimeText.Text = travel.Tid;
            FromText.Text = travel.Fra;
            ToText.Text = travel.Til;
            AirlineText.Text = travel.Flyselskap;
            
            StoreButton.Visibility = Visibility.Visible;
            reisenavntext.Visibility = Visibility.Visible;
            block.Visibility = Visibility.Collapsed;
   
        }

       
      
        private async void Button_Tapped(object sender, TappedRoutedEventArgs e)
        {
           
           var a = (ListViewItem)ListView.SelectedItems[0];
           var travel = (Reise)a.Tag;
           travel.Navn = reisenavntext.Text;
           travel.Dato = DateTime.Now.ToString("dd.MM.yy");
           if (reisenavntext.Text != "") 
           {
               await DataSource.AddReiserAsync(travel);
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
