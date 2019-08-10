using GetStartAspNet.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace GetStartAspNet.Controllers
{
    public class UserController : Controller
    {
        //Registration Action
        [HttpGet]
        public ActionResult Registration()
        {
            return View();
        }

        //Registration POST Action
        [HttpPost]
        public ActionResult Registration([Bind(Exclude = "IsEmailVerifed, ActivationCode")]User user)
        {
            bool Status = false;
            string Message = "";


            if (ModelState.IsValid)
            {
                #region Email is already
                var isExist = IsEmailExist(user.EmailID);
                if (isExist)
                {
                    ModelState.AddModelError("EmailExist", "Email already exist");
                    return View(user);
                }
                #endregion

                #region Generate Activation Code
                user.ActivationCode = Guid.NewGuid();
                #endregion

                #region Password Hashing
                user.Password = Crypto.Hash(user.Password);
                user.ConfirmPassword = Crypto.Hash(user.ConfirmPassword);
                #endregion
                user.IsEmailVerifed = false;

                #region Save too Database
                using (MyDatabaseEntities1 dc = new MyDatabaseEntities1())
                {
                    dc.Users.Add(user);
                    dc.SaveChanges();
                    ////Send Emailto User
                    //SendVerificationLinkEmail(user.EmailID, user.ActivationCode.ToString());
                    //Message = "Registration successfully done . Account activation link" +
                    //          " has been sent to your email id:" + user.EmailID;
                    user.IsEmailVerifed = true; // have not strill use email verify
                    Message = "you have registration successfully ";
                    Status = true;
                }
                #endregion
            }
            else
            {
                Message = "Invalid requrest";
            }

            ViewBag.Message = Message;
            ViewBag.Status = Status;
            return View(user);
        }

        //Verify Account
        public ActionResult VerifyAccount(string id)
        {
            bool Status = false;
            using (MyDatabaseEntities1 dc = new MyDatabaseEntities1())
            {
                dc.Configuration.ValidateOnSaveEnabled = false;// this line is added here to avoid 
                                                               // confirm passowrd does not match issue on save chages

                var v = dc.Users.Where(user => user.ActivationCode == new Guid(id)).FirstOrDefault();
                if (v != null)
                {
                    v.IsEmailVerifed = true;
                    dc.SaveChanges();
                    Status = true;
                }
                else
                {
                    ViewBag.Message = "Invalid Request";
                }
            }
            ViewBag.Status = Status;
            return View();
        }

        //Login
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        //Login POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(UserLogin login, string ReturnUrl = "")
        {
            string message = "";
            using (MyDatabaseEntities1 dc = new MyDatabaseEntities1())
            {
                var accountLogin = dc.Users.Where(user => user.EmailID == login.EmailID).FirstOrDefault();
                if (accountLogin != null)
                {
                    if (string.Compare(Crypto.Hash(login.Password), accountLogin.Password) == 0)
                    {
                        int timeout = login.RememberMe ? 525600 : 1;
                        var ticket = new FormsAuthenticationTicket(login.EmailID, login.RememberMe, timeout);
                        string encrypted = FormsAuthentication.Encrypt(ticket);

                        var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, encrypted);
                        cookie.Expires = DateTime.Now.AddMinutes(timeout);
                        cookie.HttpOnly = true;
                        Response.Cookies.Add(cookie);

                        if (Url.IsLocalUrl(ReturnUrl))
                        {
                            return Redirect(ReturnUrl);
                        }
                        else
                        {
                            return RedirectToAction("Index", "Home");
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("WrongPassword", "Password is not correct!");

                    }
                }
                else
                {
                    ModelState.AddModelError("EmailNotExist", "Email dont exist");
                }

                ViewBag.Message = message;
                return View();

            }

        }

        //Logout
        [Authorize]
        [HttpPost]
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login", "User");
        }

        // Check email exist
        [NonAction]
        public bool IsEmailExist(string emailID)
        {
            using (MyDatabaseEntities1 dc = new MyDatabaseEntities1())
            {
                var v = dc.Users.Where(user => user.EmailID == emailID).FirstOrDefault();
                return v != null;
            }
        }

        [NonAction]
        public void SendVerificationLinkEmail(string emailID, string activationCode)
        {
            var verifyUrl = "/User/VerifyAccount/" + activationCode;
            var link = Request.Url.AbsoluteUri.Replace(Request.Url.PathAndQuery, verifyUrl);

            var fromEmail = new MailAddress("dotnetawesome@gmail.com", "Dotnet Awesome");
            var toEmail = new MailAddress(emailID);
            var fromEmailPasswrod = "*********";//Replace wich actual password

            string subject = "Your account is successfully created";
            string body = "<br/><br> We are excited  to tell you that your Dotnet Awesome account is" +
                          "successfully created. Please click on the below link to verify your account" +
                          "<br/><br><a href='" + link + "'>" + link + "</a>";

            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromEmail.Address, fromEmailPasswrod)
            };

            using (var message = new MailMessage(fromEmail, toEmail)
            {
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            })
                smtp.Send(message);


        }

    }
}