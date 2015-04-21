using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataModel
{
    public partial class Reise
    {
        public Reise()
        {

        }
            public int Id { get; set; }
            public string Navn { get; set; }
            public string Dato { get; set; }
            public string Tid { get; set; }
            public string FlightId { get; set; }
            public string Fra { get; set; }
            public string Til { get; set; }
            public string Flyselskap { get; set; }

        }
    }

