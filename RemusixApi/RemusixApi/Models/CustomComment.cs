using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RemusixApi.Models
{
    public class CustomComment
    {
        public string UserName { get; set; }
        public string Photo { get; set; }
        public string Comment { get; set; }
        public System.DateTime CommentTime { get; set; }
    }
}