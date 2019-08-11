using GetStartAspNet.Models;
using MyMap.Library.Ajax;
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

        //Login
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(UserLogin login, string ReturnUrl = "")
        {
            MongoAjax _mongoAjax = new MongoAjax();
            var respond = _mongoAjax.Login(login.UserName, login.Password);
            if (respond.isSusscess)
            {
                int timeout = login.RememberMe ? 525600 : 1;
                var ticket = new FormsAuthenticationTicket(login.UserName, login.RememberMe, timeout);
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
                ViewBag.Message = respond.Message;
            }
            return View();
        }


        //Logout
        [Authorize]
        [HttpPost]
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login", "User");
        }

       
    }
}