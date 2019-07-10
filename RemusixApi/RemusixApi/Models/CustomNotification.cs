using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RemusixApi.Models
{
    public class CustomNotification
    {
        public int UserID { get; set; }
        public int nUserID { get; set; }
        public Nullable<int> PostID { get; set; }
        public string Action { get; set; }
        public System.DateTime ActionTime { get; set; }
        public string Status { get; set; }
    }
}