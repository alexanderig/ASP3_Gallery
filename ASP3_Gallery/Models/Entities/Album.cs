using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ASP3_Gallery.Models.Entities
{
    public class Album
    {
        public Album()
        {
            Pictures = new HashSet<Picture>();
        }
        public int ID { get; set; }
        [Required(ErrorMessage = "Must be not empty")]
        [StringLength(maximumLength: 30, MinimumLength = 4, ErrorMessage = "Too short or too long name")]
        public string Name { get; set; }
        public string Date { get; set; }
        [Required(ErrorMessage = "Write something")]
        public string Comment { get; set; }
        public string ThumbnailURL { get; set; }
        public virtual User User { get; set; }
        public virtual ICollection<Picture> Pictures { get; set; }
    }
}