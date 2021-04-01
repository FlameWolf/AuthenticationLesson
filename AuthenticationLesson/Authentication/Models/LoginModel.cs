using Swashbuckle.AspNetCore.Filters;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AuthenticationLesson.Authentication.Models
{
	public class LoginModel
	{
		[Required(ErrorMessage = "Username is required")]
		public string Username { set; get; }

		[Required(ErrorMessage = "Password is required")]
		public string Password { set; get; }
	}

	public class LoginModelExample : IExamplesProvider<LoginModel>
	{
		public LoginModel GetExamples()
		{
			return new LoginModel
			{
				Username = "test",
				Password = "Test@123"
			};
		}
	}
}