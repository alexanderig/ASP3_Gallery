using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using ASP3_Gallery.Models.Entities;
namespace ASP3_Gallery.Models
{
    public class GalleryDBC_Initializer:DropCreateDatabaseIfModelChanges<GalleryDBC>
    {
        protected override void Seed(GalleryDBC context)
        {

            City city1 = new City { CityName = "Одесса" };
            City city2 = new City { CityName = "Киев" };
            City city3 = new City { CityName = "Измаил" };
            City city4 = new City { CityName = "Николаев" };
            City city5 = new City { CityName = "Днепр" };
            City city6 = new City { CityName = "Львов" };
            City city7 = new City { CityName = "Минск" };
            City city8 = new City { CityName = "Гомель" };
            City city9 = new City { CityName = "Лондон" };
            City city10 = new City { CityName = "Манчестер" };
            City city11 = new City { CityName = "Тбилиси" };
            City city12 = new City { CityName = "Батуми" };
            City city13 = new City { CityName = "Рига" };
            City city14 = new City { CityName = "Лиепая" };
            City city15 = new City { CityName = "Вильнюс" };
            City city16 = new City { CityName = "Клайпеда" };
            City city17 = new City { CityName = "Ереван" };
            City city18 = new City { CityName = "Степанаван" };
            City city19 = new City { CityName = "Варшава" };
            City city20 = new City { CityName = "Гдыня" };
            City city21 = new City { CityName = "Будапешт" };
            City city22 = new City { CityName = "Дебрецен" };
            City city23 = new City { CityName = "Прага" };
            City city24 = new City { CityName = "Брно" };
            City city25 = new City { CityName = "Берлин" };
            City city26 = new City { CityName = "Лейпцих" };
            City city27 = new City { CityName = "Дюссельдорф" };
            City city28 = new City { CityName = "Рим" };
            City city29 = new City { CityName = "Неаполь" };
            Country ctr1 = new Country { Name = "Україна", Cities = new List<City> { city1, city2, city3, city4, city5, city6}  };
            Country ctr2 = new Country { Name = "Беларусі", Cities = new List<City> { city7, city8} };
            Country ctr3 = new Country { Name = "Great Britain", Cities = new List<City> { city9, city10} };
            Country ctr4 = new Country { Name = "საქართველო", Cities = new List<City> { city11, city12 } };
            Country ctr5 = new Country { Name = "Latvija", Cities = new List<City> { city13, city14 } };
            Country ctr6 = new Country { Name = "Lietuva", Cities = new List<City> { city15, city16 } };
            Country ctr7 = new Country { Name = "Հայաստան", Cities = new List<City> { city17, city18 } };
            Country ctr8 = new Country { Name = "Polska", Cities = new List<City> { city19, city20 } };
            Country ctr9 = new Country { Name = "Magyarország", Cities = new List<City> { city21, city22 } };
            Country ctr10 = new Country { Name = "Česká republika", Cities = new List<City> { city23, city24 } };
            Country ctr11 = new Country { Name = "Deutschland", Cities = new List<City> { city25, city26, city27 } };
            Country ctr12 = new Country { Name = "Italia", Cities = new List<City> { city28, city29 } };
            
            context.Cities.AddRange(new List<City> { city1, city2, city3, city4, city5, city6, city7, city8, city9, city10, city11, city12, city13, city14, city15, city16, city17, city18, city19, city20, city21, city22, city23, city24, city25, city26, city27, city28, city29 });
            context.Countries.AddRange(new List<Country> { ctr1, ctr2, ctr3, ctr4, ctr5, ctr6, ctr7, ctr8, ctr9, ctr10, ctr11, ctr12 });
            User user = new User { Name = "Alexa Man", Email = "alexa@gmail.com", Login = "admin", Password = "DbIAsRt9hMw1sYY+d5z1KCbM8WW5LdZdiq09mk8R3bsxmuGH0Qz8j/OAssl/L7+R", Salt = "DeSkJ4CUxt3EXRROFyT122iTmpB3pMk0fbtsT6uLIfc=", City = city1, Country = ctr1, AboutMe = "Just passing by" };
            context.Users.Add(user);


            base.Seed(context);
        }
    }
}