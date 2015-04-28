using DataModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.UI.Popups;

namespace DataLink
{
    public class DataSource
    {
        private const string RestServiceUrl = "http://localhost:1912/api/";
        private static DataSource _DataSource = new DataSource();
        private ObservableCollection<Reise> _reiser;

        public ObservableCollection<Reise> Reiser
        {
            get { return this._reiser; }
        }
        
        public static async Task<IEnumerable<Reise>> GetReiserAsync()
        {
           // if (_DataSource._reises == null) //Datacaching
              _DataSource._reiser = new ObservableCollection<Reise>(await _DataSource.GetAllReiserAsync());
              return _DataSource.Reiser;
        }
        
        public static async Task DeleteReiserAsync(int a)
        {
            MessageDialog dialog = new MessageDialog("");
            dialog.Commands.Add(new UICommand("Lukk"));
            Reise rs = new Reise();
            rs.Id = a;
            const string dateTimeFormat = "yyyy-MM-ddTHH:mm:ss.fffffffZ";
            var jsonSerializerSettings = new DataContractJsonSerializerSettings { DateTimeFormat = new DateTimeFormat(dateTimeFormat) };
            var jsonSerializer = new DataContractJsonSerializer(typeof(Reise), jsonSerializerSettings);

            var stream = new MemoryStream();
            stream.Position = 0;
            var content = new StringContent(new StreamReader(stream).ReadToEnd(), System.Text.Encoding.UTF8, "application/json");

            var client = new HttpClient { BaseAddress = new Uri(RestServiceUrl) };
            var response = await client.DeleteAsync("Reise/" + rs.Id);
                
            if (response.IsSuccessStatusCode)
            {
                dialog.Title = "Success";
                dialog.Content = "Flygning slettet fra databasen";
            }
            else
            {
                dialog.Title = "Failed";
                dialog.Content = "Kunne ikke slette flygningen fra databasen";
            }
            await dialog.ShowAsync();
            
        }
        public static async Task EditReiserAsync(Reise Reise, int Id)
        {
            MessageDialog dialog = new MessageDialog("");
            dialog.Commands.Add(new UICommand("Lukk"));
            Reise rs = new Reise();
            Reise.Id = Id;
            const string dateTimeFormat = "yyyy-MM-ddTHH:mm:ss.fffffffZ";
            var jsonSerializerSettings = new DataContractJsonSerializerSettings { DateTimeFormat = new DateTimeFormat(dateTimeFormat) };
            var jsonSerializer = new DataContractJsonSerializer(typeof(Reise), jsonSerializerSettings);

            var stream = new MemoryStream();
            jsonSerializer.WriteObject(stream, Reise);
            stream.Position = 0;
            var content = new StringContent(new StreamReader(stream).ReadToEnd(), System.Text.Encoding.UTF8, "application/json");
            var client = new HttpClient { BaseAddress = new Uri(RestServiceUrl) };  
            var response = await client.PutAsync(new Uri(RestServiceUrl + "Reise/" + Reise.Id), content);

            if(response.IsSuccessStatusCode)
            {
                dialog.Title = "Success";
                dialog.Content = "Flygning endret";
            }
            else
            {
                dialog.Title = "Failed";
                dialog.Content = "Kunne ikke endre flygning";
            }
            await dialog.ShowAsync();

        }
        public static async Task<Reise>AddReiserAsync(Reise aReise)
        {
            CancellationTokenSource ctc = new CancellationTokenSource();
            MessageDialog dialog = new MessageDialog("");
            dialog.Commands.Add(new UICommand("Lukk"));
            string lineChange = "\n";
            const string dateTimeFormat = "yyyy-MM-ddTHH:mm:ss.fffffffZ";
            var jsonSerializerSettings = new DataContractJsonSerializerSettings { DateTimeFormat = new DateTimeFormat(dateTimeFormat) };
            var jsonSerializer = new DataContractJsonSerializer(typeof(Reise), jsonSerializerSettings);

            var stream = new MemoryStream();
            jsonSerializer.WriteObject(stream, aReise);
            stream.Position = 0;
            var content = new StringContent(new StreamReader(stream).ReadToEnd(), System.Text.Encoding.UTF8, "application/json");
            var client = new HttpClient { BaseAddress = new Uri(RestServiceUrl) };
            var response = await client.PostAsync("Reise/", content);
            if (response.IsSuccessStatusCode)
                {
                    dialog.Title = "Success";
                    dialog.Content = "Flygningen er lagret i databasen: \n\n"
                        + "Dato: " + aReise.Dato + lineChange
                        + "Tid: " + aReise.Tid + lineChange
                        + "Navn: " + aReise.Navn + lineChange
                        + "Fra: " + aReise.Fra + lineChange
                        + "Til: " + aReise.Til + lineChange
                        + "Flyselskap: " + aReise.Flyselskap + lineChange;
                }
                else
                {
                    dialog.Title = "Failed";
                    dialog.Content = "Kunne ikke lagre flygning i databasen";
                }
            await dialog.ShowAsync();

            if (_DataSource._reiser != null)
            {
                _DataSource._reiser.Clear();
            }
            return aReise;
        }
        public async Task<IEnumerable<Reise>> GetAllReiserAsync()
        {
            MessageDialog dialog = new MessageDialog("");
            dialog.Commands.Add(new UICommand("Lukk"));
            var client = new HttpClient { BaseAddress = new Uri(RestServiceUrl) };
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var response = await client.GetAsync("Reise");

            if (response.IsSuccessStatusCode) 
            {
                dialog.Title = "Failed";
                dialog.Content = "Kunne ikke hente flygninger fra databasen";
            }

            const string dateTimeFormat = "yyyy-MM-ddTHH:mm:ss.fffffffZ";
            var jsonSerializerSettings = new DataContractJsonSerializerSettings { DateTimeFormat = new DateTimeFormat(dateTimeFormat) };

            var jsonSerializer = new DataContractJsonSerializer(typeof(Reise[]), jsonSerializerSettings);

            var stream = await response.Content.ReadAsStreamAsync();
            return (Reise[])jsonSerializer.ReadObject(stream);
        }
       
    }
}
