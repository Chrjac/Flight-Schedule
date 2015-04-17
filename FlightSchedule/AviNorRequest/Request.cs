using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace AviNorRequest
{
    public class Request
    {

        public static void Main()
        {
            
            
        }
        public static string DoRequest(string AirPort, string ArrDep)
        {
            string body;
            string url = "http://flydata.avinor.no/XmlFeed.asp?airport="+AirPort+"&TimeFrom=1&TimeTo=7&direction="+ArrDep+"&lastUpdate=2015-03-18T15:03:00Z";

            HttpWebRequest req = WebRequest.CreateHttp(url);
            req.Method = "GET"; //get is default

            var tsk = Task.Factory.FromAsync<WebResponse>(req.BeginGetResponse, req.EndGetResponse, req);

            if (!tsk.Wait(10000))
            {
                throw new Exception("Timeout");
            }

            HttpWebResponse resp = tsk.Result as HttpWebResponse;
            
            using (StreamReader reader = new StreamReader(resp.GetResponseStream()))
            {
                body = reader.ReadToEnd();
                

            }
            string final = body;
            return final;
        }

       
        
    }
}
