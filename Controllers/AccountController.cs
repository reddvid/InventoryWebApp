using ASPNETWebApp48.Models;
using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using CaptchaMvc.HtmlHelpers;
using System.Data.Entity;

namespace ASPNETWebApp48.Controllers
{
    public class AccountController : Controller
    {
        InventoryDbContext _db = new InventoryDbContext();
        [AllowAnonymous]
        public ActionResult Login(string returnUrl = "/")
        {
            if (Session["user"] != null)
            {
                TempData["alert"] = "You are login already";
                return RedirectToAction("Homepage", "Home");
            }
            // Logout account
            FormsAuthentication.SignOut();
            // Then return login form
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(string username, string password, bool rememberme = false, string returnUrl = "/")
        {
            if (this.IsCaptchaValid(""))
            {
                var valid = UserAccount.Authenticate(username, password);
                if (valid)
                {
                    Session.Clear();
                    var user = UserAccount.GetCurrentUser();
                    
                    var authTicket = new FormsAuthenticationTicket(
                        1,                              // version
                        user.UserName,                  // user name
                        DateTime.Now,                   // created
                        DateTime.Now.AddMinutes(20),    // expires
                        rememberme,                     // persistent?
                        user.Roles                      // can be used to store roles
                    );

                    string encryptedTicket = FormsAuthentication.Encrypt(authTicket);

                    var authCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);
                    System.Web.HttpContext.Current.Response.Cookies.Add(authCookie);
                    
                    Session["user"] = user.UserName;

                    if (username.ToLower() == UserAccount.DEFAULT_ADMIN_LOGIN.ToLower() && password == UserAccount.DEFAULT_ADMIN_LOGIN.ToLower())
                        return RedirectToAction("ChangePassword");
                    else
                        return RedirectToAction("Homepage", "Home"); // auth succeed			
                }
                // invalid username or password
                int loginFailureCount = 1;
                if (Session["LoginFailureCount"] != null)
                {
                    loginFailureCount = (int)Session["LoginFailureCount"] + 1;
                }

                if (loginFailureCount > 3)
                {
                    loginFailureCount = 1;
                }
                if (loginFailureCount == 3)
                {
                    Session["LoginFailureTime"] = DateTime.Now;
                    Session["example"] = DateTime.Now.AddMinutes(5);
                    TimeSpan remainingTime = ((DateTime)Session["example"]) - DateTime.Now;
                    Session["TimeRemaining"] = $"Too many failed attempts. Please try again in {remainingTime.Minutes} minutes and {remainingTime.Seconds} seconds.";
                }
                Session["LoginFailureCount"] = loginFailureCount;
                return RedirectToAction("Login", new { ReturnUrl = returnUrl });
            }
            // invalid captcha
            TempData["alert"] = "Invalid Captcha";
            return RedirectToAction("Login", new { ReturnUrl = returnUrl });
        }

        [AllowAnonymous]
        public ActionResult Forgot()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Forgot(string username)
        {
            if (this.IsCaptchaValid(""))
            {
                var valid = UserAccount.AuthenticateUser(username);
                if (valid)
                {
                    Random random = new Random();
                    int otp = random.Next(100000, 999999);
                    var user = _db.Users.FirstOrDefault(x => x.UserName.Equals(username, StringComparison.OrdinalIgnoreCase));
                    Session["OTP"] = otp.ToString();
                    string emailBody = $"Your OTP for password reset is: {otp}";
                    SendMail(user.Email, "Password Reset OTP", emailBody);
                    Session["user"] = username;
                    return RedirectToAction("Verify");
                }
                TempData["alert"] = "Invalid username";
                return RedirectToAction("Forgot");
            }
            // invalid captcha
            TempData["alert"] = "Invalid Captcha";
            return RedirectToAction("Forgot");
        }

        [HttpPost]
        public ActionResult SendMail(string EmailTo, string Subject, string Message)
        {
            var success = EmailService.SendEmail(EmailTo, Subject, Message);
            if (success)
                TempData["alert"] = "Successfully sent.";
            else
                TempData["alert"] = "Sending failed.";

            return RedirectToAction("Forgot");
        }

        [AllowAnonymous]
        public ActionResult Verify()
        {
            if (Session["OTP"] == null || Session["user"] == null)
            {
                TempData["alert"] = "Unauthorized Access, Please go back";
                return RedirectToAction("Forgot");
            }

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Verify(string otp)
        {
            string savedOTP = Session["OTP"] as string;
            if (savedOTP != null && otp == savedOTP)
            {
                return RedirectToAction("ResetPassword");
            }
            TempData["alert"] = "Invalid OTP, Please try again.";

            return View();
        }

        public ActionResult ResetPassword()
        {
            if (Session["user"] == null)
            {
                TempData["alert"] = "Unauthorized Access, Please go back";
                return RedirectToAction("Forgot");
            }

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ResetPassword(string newPassword)
        {
            // Assuming you retrieve the username from session as in your view
            var userName = Session["user"] as string;

            bool success = UserAccount.ChangePasswordForgot(userName, newPassword);
            if (success)
            {
                TempData["alert"] = $"{userName} - Password changed successfully.";
                Session.Abandon();
                return RedirectToAction("Forgot");
            }
            else
            {
                TempData["alert"] = "Failed to change password.";
                return View();
            }
        }

        public ActionResult Logoff()
        {
            FormsAuthentication.SignOut();
            Session.Abandon();
            return RedirectToAction("Login", "Account");
        }

        [Authorize]
        public ActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult ChangePassword(string currentPassword, string newPassword)
        {
            bool success = UserAccount.ChangePassword(User.Identity.Name, currentPassword, newPassword);
            if (success)
                TempData["alertbox"] = "Password changed successfully.";
            else
                TempData["alertbox"] = "Failed to change password.";

            return RedirectToAction("Logoff");
        }

        [Authorize]
        public ActionResult Register()
        {
            if (User.IsInRole("admin"))
            {
                return View();
            }
            else if (User.IsInRole("wrhmgr") || User.IsInRole("purchmgr") || User.IsInRole("salesmgr"))
                ViewBag.Message = "admin can access create user";

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(string username, string password, string email, string role = "")
        {
            if (role.ToLower().Contains(UserAccount.DEFAULT_ADMIN_ROLENAME)) role = "user"; // Prevent unauthorized creation of admin account			
            var result = UserAccount.Create(username, password, email, role);
            if (result != null)
                TempData["alert"] = String.Format("Account successfully created. Welcome {0}!", username);
            else
                TempData["alertbox"] = "There was an issue creating your account.";

            return RedirectToAction("ManageUsers");
        }

        [Authorize]
        public ActionResult ManageUsers()
        {
            if (User.IsInRole("admin"))
            {
                var user = _db.Users.ToList();

                return View(user);
            }
            else if (User.IsInRole("wrhmgr") || User.IsInRole("purchmgr") || User.IsInRole("salesmgr"))
                ViewBag.Message = "admin can access manage user";

            return View();
        }

        [HttpGet]
        [Authorize(Roles = "admin")]
        public ActionResult DisplayActivityLog()
        {
            if (User.IsInRole("admin"))
            {
                var res = _db.ActivityLogs
                    .OrderByDescending(log => log.DateTime)
                    .ToList();

                return View(res);
            }
            else if (User.IsInRole("wrhmgr") || User.IsInRole("purchmgr") || User.IsInRole("salesmgr"))
                ViewBag.Message = "admin can access activity log";

            return View();
        }

        [HttpGet]
        [Authorize(Roles = "admin")]
        public ActionResult TransactionLodge()
        {
            if (User.IsInRole("admin"))
            {
                var transact = _db.TransactionLodges
                    .OrderByDescending(log => log.Date)
                    .ToList();

                return View(transact);
            }
            else if (User.IsInRole("wrhmgr") || User.IsInRole("purchmgr") || User.IsInRole("salesmgr"))
                ViewBag.Message = "admin can access activity log";

            return View();
        }

        [Authorize(Roles = "admin")]
        public ActionResult Deactivate(string username)
        {
            if (username == "admin")
                TempData["alert"] = "Admin account cannot be deactivated.";
            else
            {
                UserAccount.SetUserActivation(username, false);
                TempData["alertbox"] = username + " is now deactivated.";
            }

            return RedirectToAction("ManageUsers", "Account");
        }

        [Authorize(Roles = "admin")]
        public ActionResult Activate(string username)
        {
            if (username == "admin")
                TempData["alert"] = "Admin account cannot be deactivated.";
            else
            {
                UserAccount.SetUserActivation(username, true);
                TempData["alertbox"] = username + " is now activated.";
            }

            return RedirectToAction("ManageUsers", "Account");
        }

        public ActionResult EditDetail(Guid Id)
        {
            UserAccount user = _db.Users.Find(Id);

            if (user == null)
            {
                TempData["alertbox"] = "User does not exist.";
                return RedirectToAction("Manage");
            }

            return View(user);
        }
    }
}