using DataModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAcc
{
    public class DataAcces : DbContext
    {
        public DbSet<Reise> Reises { get; set; }

        public DataAcces()
            : base(@"Data Source=donau.hiof.no;Initial Catalog=chrisjac;Persist Security Info=True;User ID=chrisjac;Password=Sommer15")
        {
            this.Configuration.ProxyCreationEnabled = false;
        }
    }
}

