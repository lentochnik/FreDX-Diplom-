using System;
using System.Collections.Generic;
using System.Linq;
using FreDX.Models;
using System.Web.Mvc;
using FreDX.Providers;
using System.Net;
using System.IO;

namespace FreDX.Controllers
{
    [Authorize(Roles = "User")]
    public class WorkController : Controller
    {

        AppbProvider appb = new AppbProvider();

//----------------------Создание заявки ---------------------------------------

        // GET
        public ActionResult CreateAppb()
        {
            return View();
        }

        //POST
        [HttpPost]
        public ActionResult CreateAppb(Inventionbiblio model)
        {
            if (ModelState.IsValid)
            {

                string ib = appb.CreateAppl(model);
                if (ib != null)
                {
                    Response.Redirect("Enter?num=" + model.Numerator);

                }
            }

            return View("Enter");
        }

//--------------Ввод информации о поступлении--------------------------------
         
         // GET 
         public ActionResult Enter(string num)
         {
            num = Request.QueryString["num"];

            if (num != null)
            {


                BiblioDbContext db = new BiblioDbContext();
                HelpersDbContext db1 = new HelpersDbContext();
                var CountList = db1.Count.ToList();
                var Lang = db1.lang.ToList();
                SelectList list = new SelectList(CountList, "Id", "Country");
                SelectList listd = new SelectList(CountList, "Id", "Department");
                SelectList listL = new SelectList(Lang, "Id", "Lang");
                ViewBag.LiastLang = listL;
                ViewBag.ListCountry = list;
                ViewBag.ListCountry2 = listd;
                ViewBag.Id = num;

                var enterbibl = (from ent in db.enterbiblio
                                where ent.Numerator == num
                                select ent).FirstOrDefault();
                Enterbiblio entb = enterbibl;
                db.Dispose();
                return View(entb);
            }

            return HttpNotFound();
        }

        //POST
        [HttpPost, ActionName("Enter")]
        [ValidateAntiForgeryToken]
        public ActionResult EnterConf(Enterbiblio model)
        {
          string num = Request.QueryString["num"];
           model.Numerator = num;
            if (ModelState.IsValid)
            {
                    try
                    {
                        using (BiblioDbContext db = new BiblioDbContext())
                        {
                        if (model.Id != null)
                        {
                            db.Entry(model).State = System.Data.Entity.EntityState.Modified;
                            db.SaveChanges();
                        }
                        else
                        {
                            db.enterbiblio.Add(model);
                            db.SaveChanges();

                        }
                    }
                        return RedirectToAction("Enter", "Work", new { num = model.Numerator });
                    }
                    catch
                    {
                   
                    
                    }
                   
            }
            return HttpNotFound();
        }

        //----------------------ВВод информации об изобретении -------------------------------

        //GET
        public ActionResult Invention(string num)
        {
            num = Request.QueryString["num"];
            if (num != null)
            {
                ViewBag.Id = num;
                BiblioDbContext db = new BiblioDbContext();
                Inventionbiblio ibibl = new Inventionbiblio();
                ibibl = db.inventionbiblio.Where(p => p.Numerator == num).FirstOrDefault();
                return View(ibibl);
            }
            return HttpNotFound();
        }
        //POST
        [HttpPost, ActionName("Invention")]
        [ValidateAntiForgeryToken]
        public ActionResult InventionConfirm(Inventionbiblio model)
        {
            string num = Request.QueryString["num"];
            model.Numerator = num;
            if (ModelState.IsValid)
            {
                try
                {
                    using (BiblioDbContext db = new BiblioDbContext())
                    {
                        Inventionbiblio ibibl = new Inventionbiblio();
                        ibibl = db.inventionbiblio.Where(p => p.Numerator == num).FirstOrDefault();
                        ibibl.Invname = model.Invname;
                        db.Entry(ibibl).State = System.Data.Entity.EntityState.Modified;
                        db.SaveChanges();
                    }
                    return RedirectToAction("Invention", "Work", new { num = model.Numerator });
                }
                catch
                {
                }

            }
            return HttpNotFound();
        }

//------------------------------------Изобретатели---------------------------------

        //GET
        [HttpGet]
        public ActionResult Inventors(Invent model) 
        {
            string num = Request.QueryString["num"];
            if (num != null)
            {
                BiblioDbContext db = new BiblioDbContext();
                HelpersDbContext db1 = new HelpersDbContext();
                var CountList = db1.Count.ToList();
                SelectList list = new SelectList(CountList, "Id", "Country");
                ViewBag.ListCountry = list;
                SelectList listd = new SelectList(CountList, "Id", "Department");
                ViewBag.ListCountry2 = listd;
                ViewBag.Id = num;

                    List<Invent> inventior = (from inv in db.inventors
                                        where inv.Numerator == num
                                        select inv).ToList();
                    return View("Inventors", inventior);
            }
            return HttpNotFound();
        }

        //------------------Редактировать изобретателя------------------------------------

        //GET
        public ActionResult EditIn(string num, int Id)
        {
            num = Request.QueryString["num"];
            if (num != null)
            {
                BiblioDbContext db = new BiblioDbContext();
                HelpersDbContext db1 = new HelpersDbContext();
                var CountList = db1.Count.ToList();
                SelectList list = new SelectList(CountList, "Id", "Country");
                SelectList listd = new SelectList(CountList, "Id", "Department");
                ViewBag.ListCountry = list;
                ViewBag.ListCountry2 = listd;
                ViewBag.Id = num;

             
                    var inventr = (from inv in db.inventors
                                   where inv.Numerator == num && inv.Id == Id
                                   select inv).First();
                    return View(inventr);
            }
            return HttpNotFound();
        }

        //POST
        [HttpPost, ActionName("EditIn")]
        [ValidateAntiForgeryToken]
        public ActionResult EditInConf(Invent model)
        {
            string num = Request.QueryString["num"];
            BiblioDbContext db = new BiblioDbContext();
            if (ModelState.IsValid)
            {
                try
                {
                    model.Numerator = num;
                    db.Entry(model).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Inventors", "Work", new { num = model.Numerator });
                }
                catch
                {

                }
            }
            return HttpNotFound();
        }

        //-----------------Удалить изобретателя---------------------------------------------

        //GET
        [HttpGet]
        public ActionResult DelIn(int? Id)
        {
            string num = Request.QueryString["num"];
            ViewBag.Id = num;
            if (Id == null)
            {
                if (num != null)
                {
                    return View();
                }
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BiblioDbContext db = new BiblioDbContext();
            Invent invent = db.inventors.Find(Id);
            if (invent == null)
            {
                return HttpNotFound();
            }
            return View(invent);
        }


        //POST
        [HttpPost, ActionName("DelIn")]
        [ValidateAntiForgeryToken]
        public ActionResult DelIn(string num, int Id)
        {
            num = Request.QueryString["num"];
            if (Id != 0)
            {
                BiblioDbContext db = new BiblioDbContext();
                    try
                    {
                        Invent inventor = db.inventors.Find(Id);
                        db.inventors.Remove(inventor);
                        db.SaveChanges();
                        return RedirectToAction("Inventors", "Work", new { num = inventor.Numerator });
                    }
                    catch
                    {

                    }
            }
            return HttpNotFound();
        }

//------------------Добавить изообретателя--------------------------------

        // GET
        public ActionResult AddInv(string num)
        {
            num = Request.QueryString["num"];
            if (num != null)
            {
                ViewBag.Id = num;
                BiblioDbContext db = new BiblioDbContext();
                HelpersDbContext db1 = new HelpersDbContext();
                var CountList = db1.Count.ToList();
                SelectList list = new SelectList(CountList, "Id", "Country");
                SelectList listd = new SelectList(CountList, "Id", "Department");
                ViewBag.ListCountry = list;
                ViewBag.ListCountry2 = listd;
                ViewBag.Id = num;
                return View();
           }
            return HttpNotFound();
        }

        //POST
        [HttpPost, ActionName("AddInv")]
        [ValidateAntiForgeryToken]
        public ActionResult AddInvConf(Invent model)
        {
            string num = Request.QueryString["num"];
            if (ModelState.IsValid)
            {
                ViewBag.Id = num;
                model.Numerator = num;
                if (appb.Inventors(model))
                {
                    return RedirectToAction("Inventors", "Work", new { num = model.Numerator });

                }
            }
            return View();
        }



//--------------------------Заявители-------------------------------------------------------

        //GET
        [HttpGet]
        public ActionResult Applicants(Appl model)
        {
            string num = Request.QueryString["num"];
            if (num != null)
            {
                BiblioDbContext db = new BiblioDbContext();
                HelpersDbContext db1 = new HelpersDbContext();
                var CountList = db1.Count.ToList();
                SelectList list = new SelectList(CountList, "Id", "Country");
                ViewBag.ListCountry = list;
                SelectList listd = new SelectList(CountList, "Id", "Department");
                ViewBag.ListCountry2 = listd;
                ViewBag.Id = num;

                List<Appl> applic = (from apl in db.aplicants
                                              where apl.Numerator == num
                                              select apl).ToList();
                return View("Applicants", applic);
            }
            return HttpNotFound();
        }

        //------------------Добавить заявителя--------------------------------

        //GET
        public ActionResult AddAppl(string num)
        {
            num = Request.QueryString["num"];
            BiblioDbContext db = new BiblioDbContext();
            HelpersDbContext db1 = new HelpersDbContext();
            var CountList = db1.Count.ToList();
            SelectList list = new SelectList(CountList, "Id", "Country");
            ViewBag.ListCountry = list;
            SelectList listd = new SelectList(CountList, "Id", "Department");
            ViewBag.ListCountry2 = listd;
            ViewBag.Id = num;

            return View();

        }

        //POST
        [HttpPost, ActionName("AddAppl")]
        [ValidateAntiForgeryToken]
        public ActionResult AddApplConf(Appl model)
        {
            string num = Request.QueryString["num"];
            if (ModelState.IsValid)
            {
                ViewBag.Id = num;
                model.Numerator = num;
                if (appb.Appl(model))
                {
                    return RedirectToAction("Applicants", "Work", new { num = model.Numerator });

                }
            }
            return View();
        }

//---------------------Удалить заявителя------------------------------------

        //GET
        [HttpGet]
        public ActionResult DelApp(int? Id)
        {
            string num = Request.QueryString["num"];
            ViewBag.Id = num;
            if (Id == null)
            {
                if (num != null)
                {

                    return View();

                }
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BiblioDbContext db = new BiblioDbContext();
            Appl appl = db.aplicants.Find(Id);
            if (appl == null)
            {
                return HttpNotFound();
            }
            return View(appl);
        }

        //POST
        [HttpPost, ActionName("DelApp")]
        [ValidateAntiForgeryToken]
        public ActionResult DelAppConf(string num, int Id)
        {
            num = Request.QueryString["num"];

            if (Id != 0)
            {
                BiblioDbContext db = new BiblioDbContext();
                    try
                    {
                        Appl app = db.aplicants.Find(Id);
                        db.aplicants.Remove(app);
                        db.SaveChanges();
                        return RedirectToAction("Applicants", "Work", new { num = app.Numerator });
                    }
                    catch
                    {

                    }
            }
            return HttpNotFound(); ;
        }

//-------------------Редактировать заявителя-----------------------------------

        //GET
        public ActionResult EditApl(string num, int Id)
        {
            num = Request.QueryString["num"];
            if (num != null)
            {
                BiblioDbContext db = new BiblioDbContext();
                HelpersDbContext db1 = new HelpersDbContext();
                var CountList = db1.Count.ToList();
                SelectList list = new SelectList(CountList, "Id", "Country");
                SelectList listd = new SelectList(CountList, "Id", "Department");
                ViewBag.ListCountry = list;
                ViewBag.ListCountry2 = listd;
                ViewBag.Id = num;
                    var Applicant = (from apl in db.aplicants
                                     where apl.Numerator == num && apl.Id == Id
                                   select apl).First();
                    return View(Applicant);
            }
            return HttpNotFound();
        }

        //POST
        [HttpPost, ActionName("EditApl")]
        [ValidateAntiForgeryToken]
        public ActionResult EditAplConf(Appl model)
        {
            string num = Request.QueryString["num"];
            BiblioDbContext db = new BiblioDbContext();
            if (ModelState.IsValid)
            {
                try
                {
                    model.Numerator = num;
                    db.Entry(model).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Applicants", "Work", new { num = model.Numerator });
                }
                catch
                {

                }
            }
            return HttpNotFound();
        }

        //---------------------------Агенты----------------------------------------

        //GET
        [HttpGet]
        public ActionResult Agents(Agent model)
        {
            string num = Request.QueryString["num"];
            if (num != null)
            {
                BiblioDbContext db = new BiblioDbContext();
                HelpersDbContext db1 = new HelpersDbContext();
                var CountList = db1.Count.ToList();
                SelectList list = new SelectList(CountList, "Id", "Country");
                ViewBag.ListCountry = list;
                ViewBag.Id = num;
                var StatusList = db1.Status.ToList();
                SelectList list2 = new SelectList(StatusList, "Id", "Statuses");
                ViewBag.ListStatus = list2;
                    List<Agent> ag = (from a in db.agents
                                         where a.Numerator == num
                                         select a).ToList();
                    return View("Agents", ag);
            }
            return HttpNotFound();
        }

        //------------------Добавить агента--------------------------------

        //GET
        public ActionResult AddAg()
        {
            string num = Request.QueryString["num"];
            BiblioDbContext db = new BiblioDbContext();
            HelpersDbContext db1 = new HelpersDbContext();
            var CountList = db1.Count.ToList();
            SelectList list = new SelectList(CountList, "Id", "Country");
            ViewBag.ListCountry = list;
            ViewBag.Id = num;
            var StatusList = db1.Status.ToList();
            SelectList list2 = new SelectList(StatusList, "Id", "Statuses");
            ViewBag.ListStatus = list2;
            
            return View();

        }

        //POST
        [HttpPost, ActionName("AddAg")]
        [ValidateAntiForgeryToken]
        public ActionResult AddAgConf(Agent model)
        {
            string num = Request.QueryString["num"];
            if (ModelState.IsValid)
            {
                ViewBag.Id = num;
                model.Numerator = num;
                if (appb.Agent(model))
                {
                    return RedirectToAction("Agents", "Work", new { num = model.Numerator });

                }
            }
            return View();
        }

        //---------------------Удалить агента------------------------------------

        //GET
        [HttpGet]
        public ActionResult DelAg(int? Id)
        {
            string num = Request.QueryString["num"];
            ViewBag.Id = num;
            if (Id == null)
            {
                if (num != null)
                {

                    return View();

                }
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            BiblioDbContext db = new BiblioDbContext();

            Agent agent = db.agents.Find(Id);
            if (agent == null)
            {
                return HttpNotFound();
            }
            return View(agent);
        }

        //POST
        [HttpPost, ActionName("DelAg")]
        [ValidateAntiForgeryToken]
        public ActionResult DelAgConf(string num, int Id)
        {
            num = Request.QueryString["num"];

            if (Id != 0)
            {
                BiblioDbContext db = new BiblioDbContext();

                var agn = from ag in db.agents
                           where ag.Id == Id
                           select ag;

                if (agn.Count() > 0)
                {
                    try
                    {
                        Agent agent = db.agents.Find(Id);
                        db.agents.Remove(agent);
                        db.SaveChanges();
                        return RedirectToAction("Agents", "Work", new { num = agent.Numerator });
                    }
                    catch
                    {

                    }
                }
            }
            return HttpNotFound(); ;
        }

        //-------------------Редактировать агента-----------------------------------

        //GET
        public ActionResult EditAg(string num, int Id)
        {
            num = Request.QueryString["num"];
            if (num != null)
            {
                BiblioDbContext db = new BiblioDbContext();
                HelpersDbContext db1 = new HelpersDbContext();
                var CountList = db1.Count.ToList();
                SelectList list = new SelectList(CountList, "Id", "Country");
                ViewBag.ListCountry = list;
                ViewBag.Id = num;
                var StatusList = db1.Status.ToList();
                SelectList list2 = new SelectList(StatusList, "Id", "Statuses");
                ViewBag.ListStatus = list2;

                    var agent = (from age in db.agents
                                     where age.Numerator == num && age.Id == Id
                                     select age).First();
                    return View(agent);
            }
            return HttpNotFound();
        }

        //POST
        [HttpPost, ActionName("EditAg")]
        [ValidateAntiForgeryToken]
        public ActionResult EditAgConf(Agent model)
        {
            string num = Request.QueryString["num"];
            BiblioDbContext db = new BiblioDbContext();
            if (ModelState.IsValid)
            {
                try
                {
                    model.Numerator = num;
                    db.Entry(model).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Agents", "Work", new { num = model.Numerator });
                }
                catch
                {

                }
            }
            return HttpNotFound();
        }

//---------------------------Приоритетные документы----------------------------------------

        //GET
        [HttpGet]
        public ActionResult Priority(Priority model)
        {
            string num = Request.QueryString["num"];
            if (num != null)
            {
                BiblioDbContext db = new BiblioDbContext();
                HelpersDbContext db1 = new HelpersDbContext();
                var CountList = db1.Count.ToList();
                SelectList list = new SelectList(CountList, "Id", "Country");
                SelectList list2 = new SelectList(CountList, "Id", "Department");
                ViewBag.ListCountry = list;
                ViewBag.ListCountry2 = list2;
                ViewBag.Id = num;

                List<Priority> pr = (from p in db.priority
                                      where p.Numerator == num
                                      select p).ToList();
                    return View("Priority", pr);
            }
            return HttpNotFound();
        }

        //------------------Добавить приоритет--------------------------------

        //GET
        public ActionResult AddPr(string num)
        {
            num = Request.QueryString["num"];
            BiblioDbContext db = new BiblioDbContext();
            HelpersDbContext db1 = new HelpersDbContext();
            var CountList = db1.Count.ToList();
            SelectList list = new SelectList(CountList, "Id", "Country");
            SelectList list2 = new SelectList(CountList, "Id", "Department");
            ViewBag.ListCountry = list;
            ViewBag.ListCountry2 = list2;
            ViewBag.Id = num;


            return View();

        }

        //POST
        [HttpPost, ActionName("AddPr")]
        [ValidateAntiForgeryToken]
        public ActionResult AddPrConf(Priority model)
        {
            string num = Request.QueryString["num"];
            if (ModelState.IsValid)
            {
                ViewBag.Id = num;
                model.Numerator = num;
                if (appb.Priority(model))
                {
                    return RedirectToAction("Priority", "Work", new { num = model.Numerator });

                }
            }
            return View();
        }

        //---------------------Удалить приоритет------------------------------------

        //GET
        [HttpGet]
        public ActionResult DelPr(int? Id)
        {
            string num = Request.QueryString["num"];
            ViewBag.Id = num;
            if (Id == null)
            {
                if (num != null)
                {

                    return View();

                }
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            BiblioDbContext db = new BiblioDbContext();

            Priority priority = db.priority.Find(Id);
            if (priority == null)
            {
                return HttpNotFound();
            }
            return View(priority);
        }

        //POST
        [HttpPost, ActionName("DelPr")]
        [ValidateAntiForgeryToken]
        public ActionResult DelPrConf(string num, int Id)
        {
            num = Request.QueryString["num"];

            if (Id != 0)
            {
                BiblioDbContext db = new BiblioDbContext();

                var priority = from pr in db.agents
                          where pr.Id == Id
                          select pr;

                if (priority.Count() > 0)
                {
                    try
                    {
                        Priority prior = db.priority.Find(Id);
                        db.priority.Remove(prior);
                        db.SaveChanges();
                        return RedirectToAction("Priority", "Work", new { num = prior.Numerator });
                    }
                    catch
                    {

                    }
                }
            }
            return HttpNotFound(); ;
        }

        //-------------------Редактировать приоритет-----------------------------------

        //GET
        public ActionResult EditPr(string num, int Id)
        {
            num = Request.QueryString["num"];
            if (num != null)
            {
                BiblioDbContext db = new BiblioDbContext();
                HelpersDbContext db1 = new HelpersDbContext();
                var CountList = db1.Count.ToList();
                SelectList list = new SelectList(CountList, "Id", "Country");
                SelectList list2 = new SelectList(CountList, "Id", "Department");
                ViewBag.ListCountry = list;
                ViewBag.ListCountry2 = list2;
                ViewBag.Id = num;
                var priority = (from pr in db.agents
                                 where pr.Numerator == num && pr.Id == Id
                                 select pr).First();
                    return View(priority);
            }
            return HttpNotFound();
        }

        //POST
        [HttpPost, ActionName("EditPr")]
        [ValidateAntiForgeryToken]
        public ActionResult EditPrConf(Agent model)
        {
            string num = Request.QueryString["num"];
            BiblioDbContext db = new BiblioDbContext();
            if (ModelState.IsValid)
            {
                try
                {
                    model.Numerator = num;
                    db.Entry(model).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Priority", "Work", new { num = model.Numerator });
                }
                catch
                {

                }
            }
            return HttpNotFound();
        }

        public ActionResult Application(string num)
        {
            var folder = appb.GetFilePath(num);
            if (Directory.Exists(folder))
            {
                string path = Path.Combine(folder, "1.pdf");
                return File(path, "application/pdf");
            }
            return HttpNotFound();
        }
    }

}