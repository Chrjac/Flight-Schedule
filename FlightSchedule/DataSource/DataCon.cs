using DataModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAcc
{
    public class DataAccess : DbContext
    {
        public DbSet<Reise> Reiser { get; set; }
        public DbSet<Users> Users { get; set; }

        public DataAccess()
            : base(@"Data Source=donau.hiof.no;Initial Catalog=chrisjac;Persist Security Info=True;User ID=chrisjac;Password=Sommer15")
        {
            this.Configuration.ProxyCreationEnabled = false;
        }
    }
}

