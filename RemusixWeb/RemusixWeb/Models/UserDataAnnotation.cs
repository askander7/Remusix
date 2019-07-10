using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
namespace RemusixWeb.Models
{
    [MetadataType(typeof(UserMetaData))]
    public partial class User
    {
    }
    public class UserMetaData
    {

        public int UserID { get; set; }

        [Required(ErrorMessage = "must enter{0}")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "must enter{0}")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "must enter{0}")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "must enter{0}")]
        [DataType(DataType.EmailAddress, ErrorMessage = "not valid email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "must enter{0}")]
        [DataType(DataType.Password)]
        //[MaxLength(10)]
        //[MinLength(6)]
        public string UserPassword { get; set; }

        [Required(ErrorMessage = "must enter{0}")]
        [Compare("UserPassword", ErrorMessage = "not matched")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }

        public string Photo { get; set; }
        public string Gender { get; set; }
        public string FacebookUsername { get; set; }
        public string GoogleUsername { get; set; }
        public string SoundcloudUsername { get; set; }
        public Nullable<System.DateTime> Birthdate { get; set; }
        public Nullable<double> Longitude { get; set; }
        public Nullable<double> Latitude { get; set; }
    }
}