using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace FlightSchedule
{
    class AirportCN
    {
        public static string airportSearchName(string søkefeltNavn)
        {
            XDocument doccheck = XDocument.Load("airports.xml");

            var XmlSearch = from node in doccheck.Descendants("airportName")
                            where node.Attribute("code").Value == søkefeltNavn
                            select node.Attribute("name").Value;
            string XmlSearchResult = XmlSearch.ElementAt(0);
            string i = XmlSearchResult;
            return i;
        }
    }
}
