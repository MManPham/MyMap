using GetStartAspNet.Models;
using MyMap.Library.Ajax;
using MyMap.Library.ModelsAjax;
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
        //Registration POST Action
        [HttpPost]
        public ActionResult Registration(User user)
        {
            bool Status = false;
            string Message = "";
            if (ModelState.IsValid)
            {
                MongoAjax _mongoAjax = new MongoAjax();
                UserAjax userAjax = new UserAjax { FistName = user.FistName, LastName = user.LastName, UserName = user.UserName, Password = user.Password };
                var respond = _mongoAjax.AddUser(userAjax);

                if (respond.isSuccess)
                {
                    Message = "you have registration successfully ";
                }
                else
                {
                    Message = respond.Message;
                }
                Status = respond.isSuccess;
            }
            else
            {
                ViewBag.Message = "Invalid requrest";
            }
            ViewBag.Message = Message;
            ViewBag.Status = Status;
            return View(user);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(UserLogin login, string ReturnUrl = "")
        {
            MongoAjax _mongoAjax = new MongoAjax();
            var respond = _mongoAjax.Login(login.UserName, login.Password);
            if (respond.isSuccess)
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