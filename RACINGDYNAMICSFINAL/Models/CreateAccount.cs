namespace RACINGDYNAMICSFINAL.Models
{
	using System;
	using System.Collections.Generic;
	using System.ComponentModel.DataAnnotations;
	
	public partial class CreateAccount
	{
		public int Id {get; set; }

		[Required(ErrorMessage ="This field is required!")]
		[Display(Name = "Username")]
		public string UserName {get; set; }

		[DataType(DataType.Password)]
        [Required(ErrorMessage = "This field is required!")]
        public string Password {get; set; }

        [Compare("Password", ErrorMessage = "Passwords don't match, try again!")]
        [Required(ErrorMessage = "This field is required!")]
        [Display(Name ="Confirm Password")]
		[DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }


        [Required(ErrorMessage = "This field is required!")]
        public string Email {get; set; }
	}
}