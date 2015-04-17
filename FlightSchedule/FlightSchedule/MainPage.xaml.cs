﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Xml.Linq;
using Windows.Foundation;
using Windows.Foundation.Collections;
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
        }
        public void Main()
        {
            ListView.Items.Clear();
            string søkefelt = AirportBox.SelectedValue as string;
            int ArrivalDeparture = ArrivalBox.SelectedIndex;
            string ad;


            XDocument doccheck = XDocument.Load("airports.xml");

            var XmlSearch = from node in doccheck.Descendants("airportName")
                            where node.Attribute("name").Value == søkefelt
                            select node.Attribute("code").Value;
            string XmlSearchResult = XmlSearch.ElementAt(0);
            string i = XmlSearchResult;


            if (ArrivalDeparture == 1)
            {
                ad = "D";

            }
            else
            {
                ad = "A";

            }

            string x = AviNorRequest.Request.DoRequest(i, ad);

            XDocument doc = XDocument.Parse(x);
            foreach (XElement flight in doc.Descendants("flight"))
            {
               
                string uniqueid = flight.Attribute("uniqueID").Value;
                string airline = flight.Element("airline").Value;
                string flightId = flight.Element("flight_id").Value;
                string dom = flight.Element("dom_int").Value;
                string schedule_time = flight.Element("schedule_time").Value;
                string airport = flight.Element("airport").Value;
                string schedule_time_sub = schedule_time.Substring(11);
                string schedule_time_sub2 = schedule_time_sub.TrimEnd('Z');

                XDocument doccheckAirline = XDocument.Load("airlines.xml");
                foreach (XElement Alcheck in doccheckAirline.Descendants("airlineName"))
                {
                    string test = Alcheck.Attribute("code").Value;
                    if (test == airline)
                    {
                        test = Alcheck.Attribute("name").Value;
                        ListView.Items.Add(schedule_time_sub2 + "          " + flightId + "          " + airport + "          " + test);
                    }
                    else
                    {
                    }
                        /* var XmlSearchAirline = from node in doccheck.Descendants("airlineName")
                                     where node.Attribute("code").Value == airline
                                     select node.Attribute("name").Value;
                                     string Result = XmlSearchAirline.ElementAtOrDefault(0);
                         */
                    }
                }
                FlightData.Text = x;
                StatusText.Text = "Viser " + ArrivalBox.SelectedValue + " For " + søkefelt;
            
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            Main();
            stopWatch.Stop();
            TimeSpan ts = stopWatch.Elapsed;
           
            

            // Format and display the TimeSpan value. 
            string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
                ts.Hours, ts.Minutes, ts.Seconds,
                ts.Milliseconds / 10);
            TimeSpent.Text = elapsedTime;

        }
        private void ListView_Tapped(object sender, TappedRoutedEventArgs e)
        {
            string x;
            string x1;
            int ArrivalDeparture = ArrivalBox.SelectedIndex;
            string ListText = ListView.SelectedItems[0].ToString();
            HubSection.Header = ListText.Substring(15, 18);
            hubhead.Header = ListText.Substring(0, 8);
            hubhead1.Header = AirportBox.SelectedValue;
            x = ListText.Substring(30);
            x1 = x.Substring(3, 4);
            hubhead2.Header = x1;
            hubhead3.Header = ListText.Substring(40);
            if (ArrivalDeparture == 1){
                FromBox.Text = "Fra Flyplass";
                ToBox.Text = "Til Flyplass";
            }
            else{
                FromBox.Text = "Til Flyplass";
                ToBox.Text = "Fra Flyplass";
            }
            HubSection.Visibility = Visibility.Visible;
            ToBox.Visibility = Visibility.Visible;
            TimeBox.Visibility = Visibility.Visible;
            FromBox.Visibility = Visibility.Visible;
            AirlineBox.Visibility = Visibility.Visible;
              
        }

       
    }
}