using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthenticationLesson.Authentication.Models
{
	public enum AuthenticationStatus
	{
		UserCreated,
		UserExists,
		UserLoginSuccess,
		UserLoginFailure,
		UserLocked,
		UserUnlocked,
		UserPasswordChanged,
		UserEmailChanged,
		UserDeleted
	}

	public class AuthenticationResponse
	{
		public AuthenticationStatus Status { set; get; }
		public string Message { set; get; }
	}
}