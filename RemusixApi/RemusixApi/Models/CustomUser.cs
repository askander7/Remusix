using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RemusixApi.Models
{
    public class CustomUser
    {
        public int UserID { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string UserPassword { get; set; }
        public string Photo { get; set; }
        public string Gender { get; set; }
        public string FacebookUsername { get; set; }
        public string GoogleUsername { get; set; }
        public string SoundcloudUsername { get; set; }
        public Nullable<System.DateTime> Birthdate { get; set; }
        public Nullable<double> Longitude { get; set; }
        public Nullable<double> Latitude { get; set; }
        public string Status { get; set; }
        public string Location { get; set; }
    }
}