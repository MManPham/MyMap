using GetStartAspNet.vbd_services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GetStartAspNet.Models
{
    public class SearchDirectModel
    {
        public string  keySearch { get; set; }
        public VietBandoPOI[] results { get; set; }

    }
}