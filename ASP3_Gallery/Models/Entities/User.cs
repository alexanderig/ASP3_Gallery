using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ASP3_Gallery.Models.Entities
{
    public class User
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Login { get; set; }        
        public string Password { get; set; }        
        public string Salt { get; set; }
        public string AboutMe { get; set; }
        public virtual Country Country { get; set; }
        public virtual City City { get; set; }
    }
}