using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OAuth;
using SampleWebApp.Models;
using SampleWebApp.Providers;

namespace SampleWebApp.Controllers
{
    public class TestAccountController : Controller
    {
        private IAuthenticationManager Authentication => Request.GetOwinContext().Authentication;

        private ApplicationUserManager UserManager => Request.GetOwinContext().GetUserManager<ApplicationUserManager>();

        [HttpGet]
        public async Task<ActionResult> LogIn()
        {
            var testEmail = "test@example.com";

            ApplicationUser user = await UserManager.FindByEmailAsync(testEmail);
            if (user == null)
            {
                user = new ApplicationUser {UserName = "test_user", Email = testEmail};
                await UserManager.CreateAsync(user, "P@ssw0rd");
            }

            ClaimsIdentity oAuthIdentity = await user.GenerateUserIdentityAsync(UserManager,
                OAuthDefaults.AuthenticationType);
            ClaimsIdentity cookieIdentity = await user.GenerateUserIdentityAsync(UserManager,
                CookieAuthenticationDefaults.AuthenticationType);

            AuthenticationProperties properties = ApplicationOAuthProvider.CreateProperties(user.UserName);
            Authentication.SignIn(properties, oAuthIdentity, cookieIdentity);

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public ActionResult Logout()
        {
            Authentication.SignOut(CookieAuthenticationDefaults.AuthenticationType);
            return RedirectToAction("Index", "Home");
        }
    }
}