using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using ASP3_Gallery.Models;
using ASP3_Gallery.Models.Entities;
using ASP3_Gallery.Models.ViewModels;
using ExifLib;

namespace ASP3_Gallery.Controllers
{
    public class HomeController : Controller
    {
        
        GalleryDBC ctx = new GalleryDBC();

        public async Task<ActionResult> Index()
        {
            ViewBag.Albums = ctx.Albums.Count();
            ViewBag.Pictures = ctx.Pictures.Count();
          
            return View(GetAlbums(null));
        }

        public async Task<ActionResult> About()
        {
            ViewBag.Message = "About Picture Gallery Content";

            return PartialView();
        }

        public async Task<ActionResult> Contact()
        {
            ViewBag.Message = "Picture Gallery site contacts info";

            return PartialView();
        }


        public async Task<ActionResult> Logged()
        {
            return PartialView();
        }


        public async Task<ActionResult> Logout()
        {
            Session.Abandon();
            return RedirectToAction("Index");
        }


        public async Task<ActionResult> Album()
        {
            if (Session["Login"] == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            string UserLogin = Session["Login"].ToString();
            var user = ctx.Users.Where(u => u.Login == UserLogin).FirstOrDefault();

            return PartialView(GetAlbums(user?.ID));
        }


        public async Task<ActionResult> UserAlbums(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User usr = ctx.Users.Find(id);
            ViewBag.Username = usr.Name;
            ViewBag.Login = usr.Login;
            return PartialView(GetAlbums(id));
        }


        public async Task<ActionResult> Allbums(int? page, int? tiles)
        {
            ViewBag.Albums = ctx.Albums.Count();
            ViewBag.Pictures = ctx.Pictures.Count();
            if (Session["tiles"] == null)
                Session["tiles"] = 6;
            if (tiles != null)
                Session["tiles"] = tiles;
            ViewBag.page = page;
            return PartialView(GetAlbums(null));
            
        }


        public async Task<ActionResult> Gallery(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Album album = ctx.Albums.Find(id);
            if (album == null)
            {
                return HttpNotFound();
            }

            ViewAlbum viewAlbum = new ViewAlbum
            {
                Name = album.Name,
                Date = album.Date,
                IDA = album.ID,
                User = album.User.Name,
                IDU = album.User.ID,
                Comment = album.Comment,
                URLpath = album.ThumbnailURL,
                Count = album.Pictures.Count
            };
            ViewBag.Album = viewAlbum;
            List<Picture> pictures = album.Pictures.ToList();
            return PartialView(pictures);
        }


        private List<ViewAlbum> GetAlbums(int? userid)
        {

            IQueryable<Album> albums;
            if (userid == null)
                albums = ctx.Albums;
            else
                albums = ctx.Albums.Where(al => al.User.ID == userid);
            List<ViewAlbum> albumdata = new List<ViewAlbum>();

            foreach (Album a in albums)
            {
                albumdata.Add(new ViewAlbum
                {
                    Name = a.Name,
                    Date = a.Date,
                    IDA = a.ID,
                    User = a.User.Name,
                    IDU = a.User.ID,
                    Comment = a.Comment,
                    URLpath = a.ThumbnailURL,
                    Count = a.Pictures.Count
                });
            }

            return albumdata;
        }


        public async Task<ActionResult> EditThumbnail(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Picture pic = ctx.Pictures.Find(id);
            if (pic == null)
            {
                return HttpNotFound();
            }

            return PartialView(pic);
        }

        [HttpPost, ActionName("EditThumbnail")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditThumbnailConfirm(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Picture pic = ctx.Pictures.Find(id);
            if (pic == null)
            {
                return HttpNotFound();
            }
            pic.Album.ThumbnailURL = pic.URLpath;
            ctx.Entry(pic).State = System.Data.Entity.EntityState.Modified;
            await ctx.SaveChangesAsync();

            return Json("ok");
        }

        public async Task<ActionResult> UploadPics()
        {
            
            int ida = Int32.Parse(Request.Form["IDA"]);
            Album nextAlbum = ctx.Albums.Find(ida);
            List<Picture> pictures_collection = LoadPictures(nextAlbum);
            ctx.Pictures.AddRange(pictures_collection);
            ctx.Entry(nextAlbum).State = System.Data.Entity.EntityState.Modified;
            await ctx.SaveChangesAsync();
            return Json("Загрузка успешно завершена!");

        }



        private List<Picture> LoadPictures(Album nextAlbum)
        {
            string GalleryDir = "~/Gallery";
            List<Picture> pictures_collection = new List<Picture>();
            int i = 0;
            foreach (string file in Request.Files)
            {
                var upload = Request.Files[file];
                if (upload != null)
                {
                    // получаем имя файла
                    string PictureDate = Request.Form["filedate" + i];
                    PictureDate = PictureDate.Substring(0, PictureDate.LastIndexOf("GMT"));
                    i++;
                    string picture_fileName = System.IO.Path.GetFileName(upload.FileName);
                    string serverSavefolder = Server.MapPath(GalleryDir);
                    string fullpathURL = GalleryDir.Substring(2) + "/" + picture_fileName;
                    string filesystempath = System.IO.Path.Combine(serverSavefolder, picture_fileName);
                    // сохраняем файл в папку Files в проекте
                    if (picture_fileName != null)
                    {
                        upload.SaveAs(filesystempath);
                        System.IO.FileStream image_stream = System.IO.File.OpenRead(filesystempath);
                        string PictureSize = System.Drawing.Image.FromStream(image_stream, false, false).Width.ToString();
                        PictureSize += "x" + System.Drawing.Image.FromStream(image_stream, false, false).Height.ToString();
                        string PictureBPS = System.Drawing.Image.FromStream(image_stream, false, false).PixelFormat.ToString().Substring(6);
                        image_stream.Close();

                        Picture picture = new Picture
                        {
                            Name = picture_fileName,
                            Album = nextAlbum,
                            URLpath = fullpathURL,
                            Date = PictureDate,
                            Size = PictureSize,
                            Color = PictureBPS
                        };
                        pictures_collection.Add(picture);
                    }
                }
            }

            return pictures_collection;
        }




        public async Task<ActionResult> DeletePic(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Picture pic = ctx.Pictures.Find(id);
            if (pic == null)
            {
                return HttpNotFound();
            }            
            return PartialView(pic);
        }

        [HttpPost, ActionName("DeletePic")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeletePicConfirm(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Picture pic = ctx.Pictures.Find(id);
            if (pic == null)
            {
                return HttpNotFound();
            }
            
            string serverFolder = Server.MapPath("~/Gallery");
                var ispics = ctx.Pictures.Where(p => p.ID != pic.ID && p.URLpath == pic.URLpath);
                if (ispics.Count() == 0)
                {
                    string filename = pic.URLpath.Substring(pic.URLpath.LastIndexOf('/') + 1);
                    string filePath = System.IO.Path.Combine(serverFolder, filename);
                    if (System.IO.File.Exists(filePath))
                        System.IO.File.Delete(filePath);
                }
            if (pic.URLpath == pic.Album.ThumbnailURL)
            {
                Album album = ctx.Albums.Find(pic.Album.ID);
                foreach (var p in album.Pictures)
                {
                    if (p.URLpath != album.ThumbnailURL)
                    {
                        album.ThumbnailURL = p.URLpath;
                        ctx.Entry(album).State = System.Data.Entity.EntityState.Modified;
                        break;
                    }
                }
            }
            ctx.Pictures.Remove(pic);
            await ctx.SaveChangesAsync();
            return Json("ok");
        }



        public async Task<ActionResult> EditAlbum(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Album album = ctx.Albums.Find(id);
            if (album == null)
            {
                return HttpNotFound();
            }

            return PartialView(album);

        }




        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditAlbum(Album album)
        {
            if (ModelState.IsValid)
            {
                string AlbumNameLower = album.Name.ToLower();
                var isalbum = ctx.Albums.Where(a => a.ID != album.ID && a.Name.ToLower() == AlbumNameLower);
                if (isalbum.Count() > 0)
                {
                    ModelState.AddModelError("", "Same Album name exist. Choose other Name");
                    return PartialView("EditAlbum", album);
                }

                var isEdited = ctx.Albums.Find(album.ID);
                isEdited.Name = album.Name;
                isEdited.Comment = album.Comment;
                ctx.Entry(isEdited).State = System.Data.Entity.EntityState.Modified;
                await ctx.SaveChangesAsync();
                return Json("ok");
            }


            ModelState.AddModelError("", "Something wrong");

            return PartialView("EditAlbum", album);
        }




        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Album album = ctx.Albums.Find(id);
            if (album == null)
            {
                return HttpNotFound();
            }

            return PartialView("Delete", album);

        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(int id)
        {
            string serverFolder = Server.MapPath("~/Gallery");
            Album album = ctx.Albums.Find(id);
            List<Picture> piclist = album.Pictures.ToList(); 
            foreach(var pic in piclist)
            {
                var ispics = ctx.Pictures.Where(p => p.ID != pic.ID && p.URLpath == pic.URLpath);
                if(ispics.Count() == 0)
                {
                    string filename = pic.URLpath.Substring(pic.URLpath.LastIndexOf('/') + 1);
                    string filePath = System.IO.Path.Combine(serverFolder, filename);
                    if (System.IO.File.Exists(filePath))
                        System.IO.File.Delete(filePath);
                }
            }
            ctx.Pictures.RemoveRange(piclist);
            ctx.Albums.Remove(album);
            await ctx.SaveChangesAsync();
            return Json("ok");

        }



        private bool CompareText(string one, string two)
        {
            StringComparer comparer = StringComparer.OrdinalIgnoreCase;
            return comparer.Compare(one, two) == 0;
        }

        [HttpPost]
        public  JsonResult PreUploadCheck(int? id, string AlbumName)
        {
            string albumNamelower = AlbumName.ToLower();
            IQueryable<Album> albums;
            if(id == null)
            albums = ctx.Albums.Where(a => a.Name.ToLower() == albumNamelower);
            else
            albums = ctx.Albums.Where(a => a.ID != id && a.Name.ToLower() == albumNamelower);
            return Json(albums.Count() > 0 ? true:false);
        }



        [HttpPost]
        public async Task<JsonResult> Upload()
        {
            Album nextAlbum = new Album();
            List<Picture> pictures_collection = LoadPictures(nextAlbum);
            string AlbumName = Request.Form["albumname"];
            string Comments = Request.Form["comments"];
            string SessionLogin = Session["Login"].ToString();
            nextAlbum.Name = AlbumName;
            nextAlbum.User = ctx.Users.Where(u => u.Login == SessionLogin).FirstOrDefault();
            nextAlbum.Pictures = pictures_collection;
            nextAlbum.Date = DateTime.Now.ToString();
            nextAlbum.ThumbnailURL = pictures_collection.First().URLpath;
            nextAlbum.Comment = Comments;
            ctx.Albums.Add(nextAlbum);
            await ctx.SaveChangesAsync();
            return Json("Загрузка успешно завершена!");
        }


      



    }
}