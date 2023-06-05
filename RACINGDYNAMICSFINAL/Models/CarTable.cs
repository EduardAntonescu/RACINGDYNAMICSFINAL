namespace RACINGDYNAMICSFINAL.Models
{
	using System;
	using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Drawing;

    public partial class CarsTable
	{
		public int car_id {get; set; }

        [Required]
        public string car_name {get; set; }
        [Required]
        public int car_year {get; set; }
        [Required]
        public string car_description { get; set; }
        [Required]
        public string car_engine {get; set; }
        public string car_image { get; set; }
        [Required]
        public string car_type {get; set; }
		
	}
}