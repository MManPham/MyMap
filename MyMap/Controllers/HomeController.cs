using GetStartAspNet.Models;
using GetStartAspNet.Services;
using GetStartAspNet.vbd_services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AjaxPro;
using MyMap.Library.Ajax;

namespace GetStartAspNet.Controllers
{
    public class HomeController : Controller
    {

        [Authorize]
        public ActionResult Index()
        {
         
            return View();
        }

        [HttpPost]
        public ActionResult Index(SearchDirectModel search)
        {
            if (search.keySearch != null)
            {
                MyMapSerVices map_services = new MyMapSerVices();
                try
                {
                    search.results = map_services.SearchAll(search.keySearch, 1);
                }
                catch (Exception e)
                {
                    ViewBag.hasResult = false;
                    ViewBag.message = "Error:" + e.ToString();
                }
                if (search.results == null || search.results.Length == 0)
                {
                    ViewBag.hasResult = false;
                    ViewBag.message = "No result!";
                    return View(search);
                }
                else
                {
                    ViewBag.hasResult = true;
                    return View(search);
                }

            }
            else
            {
                ViewBag.hasResult = false;
                ViewBag.message = "search key not found!";
                return View();
            }
        }

        [Authorize]
        public ActionResult FindDirects()
        {  
            ViewBag.hasResult = true;
            return View();
        }

        [HttpPost]
        public JsonResult FindDirects(FindDirectsModel findDirects, Point[] points)
        {
            string errMessage = null;

            ViewBag.hasResult = true;
            findDirects.startPoint = points[0];
            findDirects.endPoint = points[1];

            MyMapSerVices map_services = new MyMapSerVices();

            try
            {
                TransportType transport = TransportType.Car;
                findDirects.directionResult = map_services.FindShortPath(points, transport);
            }
            catch (Exception e)
            {
                errMessage = e.ToString();
            }
            return Json(findDirects.directionResult, errMessage, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult SearchDirect(string keySearch)
        {
            string errMessage = null;

            SearchDirectModel search = new SearchDirectModel();
            search.keySearch = keySearch;
            MyMapSerVices map_services = new MyMapSerVices();
            try
            {
                search.results = map_services.SearchAll(search.keySearch, 1);

            }
            catch (Exception e)
            {
                errMessage = e.ToString();
            }
            return Json(search, errMessage, JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        public ActionResult MyDirection()
        {
            return View();
        }
    }
}