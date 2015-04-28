using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModel
{
    public partial class Users
    {
          public Users()
        {
           
        }
            public int Id { get; set; }
            public string Brukernavn { get; set; }
            public string Passord { get; set; }


            public virtual ICollection<Reise> Reise { get; set; }
    }
}
