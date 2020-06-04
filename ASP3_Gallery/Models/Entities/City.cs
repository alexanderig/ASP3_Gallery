using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ASP3_Gallery.Models.Entities
{
    public class City
    {
        public City()
        {
            Users = new HashSet<User>();
        }
        public int ID { get; set; }
        public string CityName { get; set; }
        public virtual Country Country { get; set; }
        public virtual ICollection<User> Users { get; set; }
    }
}