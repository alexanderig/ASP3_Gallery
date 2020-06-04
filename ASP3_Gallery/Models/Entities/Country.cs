using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ASP3_Gallery.Models.Entities
{
    public class Country
    {
        public Country()
        {
            Users = new HashSet<User>();
            Cities = new HashSet<City>();
        }
        public int ID { get; set; }
        public string Name { get; set; }
        public virtual ICollection<User> Users { get; set; }
        public virtual ICollection<City> Cities { get; set; }
    }
}