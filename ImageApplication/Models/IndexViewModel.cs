using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ClassLibrary1;

namespace ImageApplication.Models
{
    public class IndexViewModel
    {
        public string Message { get; set; }
    }

    public class ImagesViewModel
    {
        public Image Image { get; set; }
        public bool HasPermission { get; set; }
        public string Message { get; set; }
    }

   
}