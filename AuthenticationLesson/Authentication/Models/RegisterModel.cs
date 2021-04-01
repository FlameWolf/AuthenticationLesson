using Swashbuckle.AspNetCore.Filters;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AuthenticationLesson.Authentication.Models
{
	public class RegisterModel
	{
		[EmailAddress]
		public string Email { set; get; }

		[Required(ErrorMessage = "Username is required")]
		public string Username { set; get; }

		[Required(ErrorMessage = "Password is required")]
		public string Password { set; get; }
	}

	public class RegisterModelExample : IExamplesProvider<RegisterModel>
	{
		public RegisterModel GetExamples()
		{
			return new RegisterModel
			{
				Email = "user@server.net",
				Username = "test",
				Password = "Test@123"
			};
		}
	}
}