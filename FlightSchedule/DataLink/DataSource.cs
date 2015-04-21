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
using System.Threading.Tasks;

namespace DataLink
{
    public class DataSource
    {
        private const string RestServiceUrl = "http://localhost:1912/api/";
        private static DataSource _DataSource = new DataSource();
        private ObservableCollection<Reise> _reises;

        public ObservableCollection<Reise> Reises
        {
            get { return this._reises; }
        }

        public static async Task<IEnumerable<Reise>> GetReisesAsync()
        {
           // if (_DataSource._reises == null) //Datacaching!!!!!!!! styr unna.
                _DataSource._reises = new ObservableCollection<Reise>(await _DataSource.GetAllReisesAsync());

            return _DataSource.Reises;
        }
        public static async Task UpdateReisesAsync(Reise aReise)
        {
            // to update the database, if OK, then proceed to update the local data
            const string dateTimeFormat = "yyyy-MM-ddTHH:mm:ss.fffffffZ";
            var jsonSerializerSettings = new DataContractJsonSerializerSettings { DateTimeFormat = new DateTimeFormat(dateTimeFormat) };
            var jsonSerializer = new DataContractJsonSerializer(typeof(Reise), jsonSerializerSettings);

            var stream = new MemoryStream();
            jsonSerializer.WriteObject(stream, aReise);
            stream.Position = 0;   // Make sure to rewind the cursor before you try to read the stream
            var content = new StringContent(new StreamReader(stream).ReadToEnd(), System.Text.Encoding.UTF8, "application/json");

            var client = new HttpClient { BaseAddress = new Uri(RestServiceUrl) };
            var response = await client.PutAsync("Reise/" + aReise.Id, content);

            response.EnsureSuccessStatusCode(); // Throw an exception if something went wrong

            // a smarter approach would be to update the element (remove/add is brute force)
            _DataSource._reises.Remove(_DataSource._reises.First(c => c.Id == aReise.Id));
            _DataSource._reises.Add(aReise);
        }
        public static async Task<Reise>AddReisesAsync(Reise aReise)
        {
            // to update the database, if OK, then proceed to update the local data
            const string dateTimeFormat = "yyyy-MM-ddTHH:mm:ss.fffffffZ";
            var jsonSerializerSettings = new DataContractJsonSerializerSettings { DateTimeFormat = new DateTimeFormat(dateTimeFormat) };
            var jsonSerializer = new DataContractJsonSerializer(typeof(Reise), jsonSerializerSettings);

            var stream = new MemoryStream();
            jsonSerializer.WriteObject(stream, aReise);
            stream.Position = 0;   // Make sure to rewind the cursor before you try to read the stream
            var content = new StringContent(new StreamReader(stream).ReadToEnd(), System.Text.Encoding.UTF8, "application/json");

            var client = new HttpClient { BaseAddress = new Uri(RestServiceUrl) };
            var response = await client.PostAsync("Reise/", content);

            response.EnsureSuccessStatusCode(); // Throw an exception if something went wrong
            
            // a smarter approach would be to update the element (remove/add is brute force)
            if (_DataSource._reises != null) {
                _DataSource._reises.Clear();
            }
           
            return aReise;
        }
        public async Task<IEnumerable<Reise>> GetAllReisesAsync()
        {
            var client = new HttpClient { BaseAddress = new Uri(RestServiceUrl) };
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var response = await client.GetAsync("Reise");
            response.EnsureSuccessStatusCode(); // Throw an exception if something went wrong

            const string dateTimeFormat = "yyyy-MM-ddTHH:mm:ss.fffffffZ";
            var jsonSerializerSettings = new DataContractJsonSerializerSettings { DateTimeFormat = new DateTimeFormat(dateTimeFormat) };

            var jsonSerializer = new DataContractJsonSerializer(typeof(Reise[]), jsonSerializerSettings);

            var stream = await response.Content.ReadAsStreamAsync();
           // string x = await response.Content.ReadAsStringAsync();
            return (Reise[])jsonSerializer.ReadObject(stream);
            
            
            
            
        }
    }
}
