using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using ASP3_Gallery.Models.Entities;

namespace ASP3_Gallery.Models.ViewModels
{
    public class ViewRegister
    {
        [Required]
        [RegularExpression("[A-ZА-ЯЁ]{1}[a-zа-яё]+\\s[A-ZА-ЯЁ]{1}[a-zа-яё]+$", ErrorMessage = "Name must be type Elon Musk")]
        public string Name { get; set; }

        [Required]
        [RegularExpression("^\\w+([-+.']\\w+)*@\\w+([-.]\\w+)*\\.\\w+([-.]\\w+)*$", ErrorMessage = "Email not correct")]
        public string Email { get; set; }
        
        [Required]
        public int CountryID { get; set; }
        public virtual Country Country { get; set; }

        [Required]
        public int CityID { get; set; }
        public virtual City City { get; set; }

        [Required]
        [RegularExpression("\\w{4,10}", ErrorMessage = "login must be not less 4 alphanumeric symbols")]
        public string Login { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [RegularExpression("^[a-zA-Z][a-zA-Z0-9-_\\.]{3,10}$", ErrorMessage = "password must be not less 4 symbols and begin with alphanumeric")]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Пароли не совпадают")]
        [Display(Name = "Repeat")]
        public string PasswordDouble { get; set; }

        public string About { get; set; }
    }
}