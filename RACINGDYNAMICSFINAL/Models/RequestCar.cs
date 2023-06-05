using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Permissions;
using System.Web;

namespace RACINGDYNAMICSFINAL.Models
{
    public partial class UserRequestCars
    {
        public int RequestId { get; set; }

        [Required]
        public string CarName { get; set; }

        [Required]
        public int CarYear { get; set; }

        [Required]
        public string CarDescription { get; set; }

        [Required]        
        public string CarEngine { get; set; }

        public string CarImage { get; set; }

        public string RequestStatus { get; set; }

        public string RequestUsername { get; set; }

        [Required]
        public string CarType { get; set; }

        

    }
}