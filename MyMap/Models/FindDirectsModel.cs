using GetStartAspNet.vbd_services;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace GetStartAspNet.Models
{
    public class FindDirectsModel
    {
        [Display(Name = "Name of Start Point")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Name of start point is required")]
        public string nameStartPoint { get; set; }

        [Display(Name = "Name of End Point")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Name of end point is required")]
        public string nameEndPoint { get; set; }

        public Point startPoint { get; set; }
        public Point endPoint { get; set; }

        public DirectionResult directionResult { get; set; }
    }
}