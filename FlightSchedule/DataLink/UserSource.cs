using DataModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Popups;

namespace DataLink
{
    public class UserSource
    {
        private const string RestServiceUrl = "http://localhost:1912/api/";
        private static UserSource _UserSource = new UserSource();
        private ObservableCollection<Users> _users;

        public ObservableCollection<Users> Users
        {
            get { return this._users; }
        }

        
        public static async Task<IEnumerable<Users>> GetUsersAsync()
        {
            // if (_DataSource._reises == null) //Datacaching
            _UserSource._users = new ObservableCollection<Users>(await _UserSource.GetAllUsersAsync());
            return _UserSource.Users;
        }
        public async Task<IEnumerable<Users>> GetAllUsersAsync()
        {
            MessageDialog dialog = new MessageDialog("");
            dialog.Commands.Add(new UICommand("Lukk"));
            var client = new HttpClient { BaseAddress = new Uri(RestServiceUrl) };
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var response = await client.GetAsync("Users");

            if (response.IsSuccessStatusCode)
            {
                dialog.Title = "Failed";
                dialog.Content = "Kunne ikke hente flygninger fra databasen";
            }
            const string dateTimeFormat = "yyyy-MM-ddTHH:mm:ss.fffffffZ";
            var jsonSerializerSettings = new DataContractJsonSerializerSettings { DateTimeFormat = new DateTimeFormat(dateTimeFormat) };

            var jsonSerializer = new DataContractJsonSerializer(typeof(Users[]), jsonSerializerSettings);

            var stream = await response.Content.ReadAsStreamAsync();
            return (Users[])jsonSerializer.ReadObject(stream);
        }
    }
}
