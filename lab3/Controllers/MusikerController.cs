using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using lab3.Models;
using Microsoft.AspNetCore.Mvc;


namespace lab3.Controllers
{
    public class MusikerController : Controller
    {


        /*public IActionResult Index()
        {
            return View();
        }*/

        [HttpGet]
        public IActionResult InsertMusiker()
        {
            return View();
        }

        [HttpPost]
        public IActionResult InsertMusiker(Musikerdetalj md)
        {

            DBMetoder dm = new DBMetoder();
            int i = 0;
            string error = "";
            i = dm.InsertMusiker(md, out error);

            if (i == 1)
            {
                TempData["successMessage"] = "Musikern har lagts till.";
                return RedirectToAction("SelectWithDataSet");
            }
            else
            {
                TempData["errorMessage"] = error;
                return RedirectToAction("SelectWithDataSet");
            }
        }

        [HttpGet]
        public IActionResult Delete(int Skivor_ID)
        {
            DBMetoder dm = new DBMetoder();
            string error = "";
            var skivor = dm.GetMusiker(Skivor_ID, out error);
            ViewBag.error = error;
            if (skivor != null)
            {
                return View(skivor);
            }

            return RedirectToAction("SelectWithDataSet");

        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult Delete2(int Skivor_ID)
        {
            DBMetoder dm = new DBMetoder();
            string error = "";
            int i = dm.DeleteMusiker(Skivor_ID, out error);
            HttpContext.Session.SetString("antal", i.ToString());
            return RedirectToAction("SelectWithDataSet");
        }

        [HttpGet]
        public IActionResult Edit(int Skivor_ID)
        {
            DBMetoder db = new DBMetoder();
            string error = "";
            var skivor = db.GetMusiker(Skivor_ID, out error);

            if (skivor != null)
            {
                return View(skivor);
            }

            ViewBag.error = error;
            return RedirectToAction("SelectWithDataSet");
        }


        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int Skivor_ID, Musikerdetalj updatedSkivor)
        {
            if (ModelState.IsValid)
            {
                DBMetoder db = new DBMetoder();
                string error = "";

                int i = db.UpdateSkivor(Skivor_ID, updatedSkivor, out error);

                if (i > 0)
                {
                    return RedirectToAction("Details", new {Skivor_ID });
                }
                else
                {
                    ViewBag.error = error;
                }
            }
            return View(updatedSkivor);
        }


        public IActionResult SelectWithDataSet()
        {
            List<Musikerdetalj> Musikerlist = new List<Musikerdetalj>();
            DBMetoder dm = new DBMetoder();

            ViewBag.SuccessMessage = TempData["successMessage"] as string;
            ViewBag.ErrorMessage = TempData["errorMessage"] as string;

            string error = " ";
            Musikerlist = dm.GetMusikerWithDataSet(out error);

            if (Musikerlist == null)
            {
                ViewBag.error = error;
                Musikerlist = new List<Musikerdetalj>();
            }

            return View(Musikerlist);
        }


        public ActionResult Details(int Skivor_ID)
        {
            Musikerdetalj Skivor = new Musikerdetalj();
            DBMetoder dm = new DBMetoder();
            Skivor = dm.GetMusiker(Skivor_ID, out string error);
            ViewBag.error = error;
            return View(Skivor);
        }

        [HttpGet]
        public IActionResult Sortering (string sortera)
        {
            SkivorMusikerMetoder sm = new SkivorMusikerMetoder();
            MusikerMetoder mm = new MusikerMetoder();

            List<SkivorMusikerModel> SkivorMusikerModelLista = sm.GetSkivorMusikerModel(out string errormsg);
            string currentDirection = HttpContext.Session.GetString("Direction");

            bool ascending = true;

            if (currentDirection !=null)
            {
                ascending = currentDirection == "asc";
            }

            ViewBag.Direction = ascending ? "asc" : "desc";

            if (sortera == "skivor")
            {
                if (ascending)
                {
                    SkivorMusikerModelLista = SkivorMusikerModelLista.OrderBy(s => s.Skivor).ToList();
                    HttpContext.Session.SetString("Direction", "desc");
;               }
                else
                {
                    SkivorMusikerModelLista = SkivorMusikerModelLista.OrderByDescending(s => s.Skivor).ToList();
                    HttpContext.Session.SetString("Direction", "asc");
                }

            }
            else { }

            ViewModelSM modelSM = new ViewModelSM
            {
                SkivorMusikerModelLista = SkivorMusikerModelLista,
                MusikerModelLista = mm.GetMusikerLista(out string errormsg2)
            };

            ViewBag.sortera = sortera;

            return View(modelSM);
        }

        [HttpGet]
        public IActionResult Search()
        {
            List<Musikerdetalj> musikerLista = new List<Musikerdetalj>();
            DBMetoder dm = new DBMetoder();
            string error = "";
            musikerLista = dm.GetMusikerWithDataSet(out error);
            ViewBag.error = error;
            return View(musikerLista);
        }


        [HttpPost]
        public IActionResult Search(string input)
        {
            DBMetoder dm = new DBMetoder();
            string error = "";

            List<Musikerdetalj> musiker = dm.SearchMusiker(input, out string errormsg);

            ViewBag.error = errormsg;

            if (musiker != null)
            {
                return View(musiker);
            }

            return RedirectToAction("SelectWithDataSet");
        }

        [HttpGet]
        public IActionResult Filtrering3(int Musiker)
        {
            SkivorMusikerMetoder sm = new SkivorMusikerMetoder();
            MusikerMetoder mm = new MusikerMetoder();

            ViewModelSM myModel = new ViewModelSM
            {
                SkivorMusikerModelLista = sm.GetSkivorMusikerModel(Musiker, out string errormsg),
                MusikerModelLista = mm.GetMusikerLista(out string errormsg2)
            };

            List<MusikerModel> MusikantLista = new List<MusikerModel>();
            MusikantLista = mm.GetMusikerLista(out string errormsg3);
            ViewBag.error = "1: " + errormsg + "2: " + errormsg2 + "3: " + errormsg3;
            ViewData["musikantlista"] = MusikantLista;

            ViewBag.musikantlista = MusikantLista;

            return View(myModel);
        }

        [HttpPost]
        public IActionResult Filtrering3(string Musiker)
        {
            int i = Convert.ToInt32(Musiker);
            SkivorMusikerMetoder sm = new SkivorMusikerMetoder();
            MusikerMetoder mm = new MusikerMetoder();

            ViewModelSM myModel = new ViewModelSM
            {
                SkivorMusikerModelLista = sm.GetSkivorMusikerModel(i, out string errormsg),
                MusikerModelLista = mm.GetMusikerLista(out string errormsg2)
            };
            List<MusikerModel> MusikantLista = new List<MusikerModel>();
            MusikantLista = mm.GetMusikerLista(out string errormsg3);

            ViewBag.error = "1: " + errormsg + "2: " + errormsg2 + "3: " + errormsg3;
            ViewData["musikantlista"] = MusikantLista;

            ViewBag.musikantlista = MusikantLista;
            ViewBag.message = Musiker;
            ViewData["Musiker"] = i;


            return View(myModel);
        }

    }

}
