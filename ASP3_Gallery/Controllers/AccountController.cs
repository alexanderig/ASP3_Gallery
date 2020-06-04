using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ASP3_Gallery.Models.ViewModels;
using System.Threading.Tasks;
using ASP3_Gallery.Models.Entities;
using ASP3_Gallery.Models;
using System.Net;

namespace ASP3_Gallery.Controllers
{
    public class AccountController : Controller
    {
        GalleryDBC ctx;

        public AccountController() { ctx = new GalleryDBC(); }

        // GET: Account
        public async Task<ActionResult> Login()
        {
            return PartialView(new ViewLogin { Login = "", Password = "" });
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(ViewLogin userdata)
        {
            User user = ctx.Users.Where(u => u.Login == userdata.Login).FirstOrDefault();
            if (ModelState.IsValid && user != null && SecurityHandler.Compare(userdata.Password, user.Password, user.Salt))
            {
                Session["Name"] = user.Name;
                Session["Email"] = user.Email;
                Session["Login"] = user.Login;
                Session["Country"] = user.Country.Name;
                Session["City"] = user.City.CityName;
                Session["About"] = user.AboutMe;
                return RedirectToAction("Logged", "Home");
            }
            else
                ModelState.AddModelError("", "You wrong somewhere");
            return PartialView(userdata);
        }

        [HttpGet]
        public async Task<ActionResult> Register()
        {
                ViewBag.ListCountries = new SelectList(ctx.Countries, "ID", "Name");
                ViewBag.ListCities = new SelectList(ctx.Cities.Where(c => c.Country.ID == 1), "ID", "CityName");
                return PartialView(new ViewRegister { Name = "", Email = "", CityID = 1, Country = null, Login = "", Password = "", PasswordDouble = "", About = "" });
            
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(ViewRegister data)
        {
            User nuser = ctx.Users.Where(u => u.Login == data.Login).FirstOrDefault();
            if (ModelState.IsValid && nuser == null)
            {
                nuser = new User();
                nuser.Name = data.Name;
                nuser.Email = data.Email;
                nuser.Login = data.Login;
                nuser.Country = ctx.Countries.Find(data.CountryID);
                nuser.City = ctx.Cities.Where(c => c.ID == data.CityID).FirstOrDefault();
                nuser.Salt = SecurityHandler.GetSalt();
                nuser.Password = SecurityHandler.GetHash(data.Password, nuser.Salt);
                nuser.AboutMe = data.About;
                ctx.Users.Add(nuser);
                ctx.SaveChangesAsync();
                Session["Name"] = nuser.Name;
                Session["Email"] = nuser.Email;
                Session["Login"] = nuser.Login;
                Session["Country"] = nuser.Country.Name;
                Session["City"] = nuser.City.CityName;
                Session["About"] = nuser.AboutMe;
                return RedirectToAction("Logged", "Home");

            }
            else if (nuser != null)
                ModelState.AddModelError("", "User already exist");
            else
                ModelState.AddModelError("", "You wrong somewhere");
            ViewBag.ListCountries = new SelectList(ctx.Countries, "ID", "Name");
            ViewBag.ListCities = new SelectList(ctx.Cities.Where(c => c.Country.ID == data.CountryID), "ID", "CityName");
            return PartialView(data);
        }


        public JsonResult GetCities(string CountryID)
        {
            List<City> jsondata;
            List<City> jsonresult = new List<City>();
            if (Int32.TryParse(CountryID, out int result))
                jsondata = ctx.Countries.Where(ct => ct.ID == result).SelectMany(c => c.Cities).ToList();
            else
                jsondata = ctx.Countries.Where(c => c.ID == 1).SelectMany(c => c.Cities).ToList();
            foreach (var js in jsondata)
                jsonresult.Add(new City { ID = js.ID, CityName = js.CityName });
                return Json(jsonresult, JsonRequestBehavior.AllowGet);
            
        }

    }


    
}