using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace ASP3_Gallery.Models.ViewModels
{
    public class ViewLogin
    {
        [Required]
        [RegularExpression("\\w{4,10}", ErrorMessage = "login must be not less 4 alphanumeric symbols")]
        public string Login { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [RegularExpression("^[a-zA-Z][a-zA-Z0-9-_\\.]{3,10}$", ErrorMessage = "password must be not less 4 symbols and begin with alpha")]
        public string Password { get; set; }
    }
}