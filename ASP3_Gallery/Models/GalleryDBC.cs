using System;
using System.Data.Entity;
using System.Linq;
using ASP3_Gallery.Models.Entities;



namespace ASP3_Gallery.Models
{
   

    public class GalleryDBC : DbContext
    {
        // Your context has been configured to use a 'GalleryDBC' connection string from your application's 
        // configuration file (App.config or Web.config). By default, this connection string targets the 
        // 'ASP3_Gallery.Models.GalleryDBC' database on your LocalDb instance. 
        // 
        // If you wish to target a different database and/or database provider, modify the 'GalleryDBC' 
        // connection string in the application configuration file.
        public GalleryDBC()
            : base("name=GalleryDBC")
        {
        }

        // Add a DbSet for each entity type that you want to include in your model. For more information 
        // on configuring and using a Code First model, see http://go.microsoft.com/fwlink/?LinkId=390109.

        // public virtual DbSet<MyEntity> MyEntities { get; set; }
        public virtual DbSet<Album> Albums { get; set; }
        public virtual DbSet<City> Cities { get; set; }
        public virtual DbSet<Country> Countries { get; set; }        
        public virtual DbSet<Picture> Pictures { get; set; }
        public virtual DbSet<User> Users { get; set; }
    }

    //public class MyEntity
    //{
    //    public int Id { get; set; }
    //    public string Name { get; set; }
    //}
}