using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace FlightSchedule
{
    class AirportNC
    {
        public static string airportSearchCode(string søkefelt){
        XDocument doccheck = XDocument.Load("airports.xml");

        var XmlSearch = from node in doccheck.Descendants("airportName")
                        where node.Attribute("name").Value == søkefelt
                        select node.Attribute("code").Value;
        string XmlSearchResult = XmlSearch.ElementAt(0);
        string i = XmlSearchResult;
        return i;
        }
    }
}
