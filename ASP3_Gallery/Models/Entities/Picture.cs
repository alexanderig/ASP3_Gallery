using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ASP3_Gallery.Models.Entities
{
    public class Picture
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string URLpath { get; set; }
        public string Date { get; set; }
        public string Size { get; set; }
        public string Color { get; set; }
        public virtual Album Album { get; set; }

    }
}