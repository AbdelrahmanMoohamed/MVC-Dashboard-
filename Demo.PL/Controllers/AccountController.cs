using Demo.DAL.Models;
using Demo.PL.Helper;
using Demo.PL.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Twilio.TwiML.Voice;

namespace Demo.PL.Controllers
{
	public class AccountController : Controller
	{
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly SignInManager<ApplicationUser> _signInManager;
		private readonly IEmailSettings _emailSettings;
		private readonly ISmsService _smsService;

		public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager
			,IEmailSettings emailSettings, ISmsService smsService)
		{
			_userManager = userManager;
			_signInManager = signInManager;
			_emailSettings = emailSettings;
			_smsService = smsService;
		}


		#region Sign Up

		[HttpGet]
		public IActionResult SignUp()
		{
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> SignUp(SignUpViewModel model)
		{
			if (ModelState.IsValid) // Server Side Validation
			{
				var user = await _userManager.FindByNameAsync(model.UserName);
				if (user is null)
				{
					user = await _userManager.FindByEmailAsync(model.Email);
					if (user is null)
					{
						// Manual Mapping
						user = new ApplicationUser()
						{
							UserName = model.UserName,
							Email = model.Email,
							FirstName = model.FirstName,
							LastName = model.LastName,
							IsAgree = model.IsAgree,
						};
						var result = await _userManager.CreateAsync(user, model.Password);
						if (result.Succeeded)
							return RedirectToAction(nameof(SignIn));
						foreach (var error in result.Errors)
						{
							ModelState.AddModelError(string.Empty, error.Description);
						}
					}
				}

				ModelState.AddModelError(string.Empty, "User is already Exits (:");
			}

			return View(model);
		}


		#endregion

		#region Sign In
		public IActionResult SignIn()
		{
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> SignIn(SignInViewModel model)
		{

			if (ModelState.IsValid)
			{
				var user = await _userManager.FindByEmailAsync(model.Email);
				if (user is not null)
				{
					var Flag = await _userManager.CheckPasswordAsync(user, model.Password);
					if (Flag)
					{
						var result = await _signInManager.PasswordSignInAsync(user, model.Password, model.RememberMe, false);
						if (result.Succeeded)
						{
							return RedirectToAction("Index", "Home");
						}
					}
				}

				ModelState.AddModelError(string.Empty, "Invalid Login!");
			}

			return View(model);
		}


		// GoogleLogin
		 public IActionResult GoogleLogin()
		{
			var prop = new AuthenticationProperties
			{
				RedirectUri = Url.Action("GoogleResponse")
			};
			return Challenge(prop,GoogleDefaults.AuthenticationScheme);
		}
		

		// GoogleResponse

		public async Task<IActionResult> GoogleResponse()
		{
			var result = await HttpContext.AuthenticateAsync(GoogleDefaults.AuthenticationScheme);

			var calims = result.Principal.Identities.FirstOrDefault().Claims.Select(
				claim => new
				{
					claim.Issuer,
					claim.OriginalIssuer,
					claim.Type, claim.Value
				});

			return RedirectToAction("Index", "Home");
		}

		#endregion

		#region Sign Out


		public new async Task<IActionResult> SignOut()
		{
			await _signInManager.SignOutAsync();
			return RedirectToAction(nameof(SignIn));
		}

		#endregion

		#region ForgetPassword
		[HttpGet]
		public IActionResult ForgetPassword()
		{
			return View();
		}


		[HttpPost]
		public async Task<IActionResult> SendResetPasswordUrl(ForgetPasswordViewModel model)
		{
			if (ModelState.IsValid)
			{
				var user = await _userManager.FindByEmailAsync(model.Email);
				if (user is not null)
				{
					// Generate Token

					var token = await _userManager.GeneratePasswordResetTokenAsync(user);

					// create URL Which Send in Body of The Email

					var url = Url.Action("ResetPassword", "Account", new { email = model.Email, token = token }, Request.Scheme);

					// https://localhost:44384/Account/ResetPassword?email=ahmed@gmail.com&token=

					// Create Email

					var email = new Email()
					{
						To = model.Email,
						Subject = "Reset Password",
						Body = url,
					};

					// Send Email

					//EmailSettings.SendEmail(email);
					_emailSettings.SendEmail(email);

					return RedirectToAction(nameof(CheckYourInbox));
				}

				ModelState.AddModelError(string.Empty, "Invalid Email!");

			}

			return View(nameof(ForgetPassword), model);
		}

		// Sned SMS
		[HttpPost]
		public async Task<IActionResult> SendSMS(ForgetPasswordViewModel model)
		{
			if (ModelState.IsValid)
			{
				var user = await _userManager.FindByEmailAsync(model.Email);
				if (user is not null)
				{
					// Generate Token

					var token = await _userManager.GeneratePasswordResetTokenAsync(user);

					// create URL Which Send in Body of The Email

					var url = Url.Action("ResetPassword", "Account", new { email = model.Email, token = token }, Request.Scheme);

					// https://localhost:44384/Account/ResetPassword?email=ahmed@gmail.com&token=

					// Create Email

					var sms = new SmsMessage()
					{
						PhoneNumber = user.PhoneNumber,
						Body = url
					};

					// Send Email

					//EmailSettings.SendEmail(email);
					//_emailSettings.SendEmail(email);
					_smsService.send(sms);

					return RedirectToAction(nameof(CheckYourInbox));
				}

				ModelState.AddModelError(string.Empty, "Invalid Email!");

			}

			return View(nameof(ForgetPassword), model);
		}

		public IActionResult CheckYourInbox()
		{
			return View();
		}

		#endregion

		#region Reset Password
		[HttpGet]
		public IActionResult ResetPassword(string email, string token)
		{
			TempData["email"] = email;
			TempData["token"] = token;

			return View();
		}

		[HttpPost]
		public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
		{
			if (ModelState.IsValid)
			{
				var email = TempData["email"] as string;
				var token = TempData["token"] as string;

				var user = await _userManager.FindByEmailAsync(email);

				if (user is not null)
				{

					var result = await _userManager.ResetPasswordAsync(user, token, model.NewPassword);
					if (result.Succeeded)
					{
						return RedirectToAction(nameof(SignIn));
					}
					foreach (var error in result.Errors)
					{
						ModelState.AddModelError(string.Empty, error.Description);

					}

				}
				ModelState.AddModelError(string.Empty, "Invalid Reset Password Please Try Again!");

			}

			return View(model);
		}

		#endregion



		public IActionResult AccessDenied()
		{
			return View();
		}


    }
}
