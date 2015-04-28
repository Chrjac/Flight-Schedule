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
using System.Collections.ObjectModel;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace FlightSchedule
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class FrontPage : Page
    {
        public FrontPage()
        {
            this.InitializeComponent();
            LoginErrorText.Text = "";
            
        }
        private async void userAuthentication()
        {

            // var b = await DataSource.GetReiserAsync();

            //foreach (var item in b)
            //{
            //    var a = new ListViewItem();
            //    a.Tag = item;
            //    a.Content = item.Dato + "        " + item.Navn +"        " + item.Fra + "        " + item.Til;
            //    SqlList2.Items.Add(a);
            //}

            string uN = UserName.Text;
            var pW = Password.Password;

            var userCheck = await UserSource.GetUsersAsync();
            
            foreach (var item in userCheck)
            {
                if (item.Brukernavn == uN)
                {
                    if (item.Passord == pW)
                    {
                        this.Frame.Navigate(typeof(MainPage));
                    }
                    else
                    {
                        
                        LoginErrorText.Text = "Feil Brukernavn / Passord";
                    }
                }
                else
                {
                   
                    LoginErrorText.Text = "Feil Brukernavn / Passord";
                }            
            }
        }
        
        private void Button_Tapped(object sender, TappedRoutedEventArgs e)
        {
            userAuthentication();
            
        }
    }
}            