using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace ASP3_Gallery.Models.ViewModels
{
    public class ViewAlbum
    {
        public int IDA { get; set; }
        public int IDU { get; set; }
        public int AlbmCount { get; set; }
        public string Name { get; set; }
        public string Date { get; set; }
        public string User { get; set; }
        public int Count { get; set; }
        public string Comment { get; set; }
        public string URLpath { get; set; }


    }
}