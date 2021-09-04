using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FreDX.Models;
using FreDX.Functions;
using System.Web.Security;
using System.IO;
using System.Text;

namespace FreDX.Providers
{
    public class AppbProvider
    {
        public string CreateAppl(Inventionbiblio model)
        {
            string Idb;

            if (model.Numerator.Length != 0 && model.Department.Length != 0 && model.Year.Length != 0  && model.Department != null && model.Year != null)
            {
                if (model.Numerator.Length < 6)  // подгоняем номер заявки к исходной
                {
                    var iw = model.Numerator.Length;
                    iw = 6 - iw;
                    for (int i = 0; i < iw; i++)
                    {
                        model.Numerator = "0" + model.Numerator;
                    }
                    Idb = "PCT" + model.Department + model.Year + model.Numerator;
                    model.Numerator = Idb;
                }
                else
                {
                    Idb = "PCT" + model.Department + model.Year + model.Numerator;
                    model.Numerator = Idb;
                }
              if (GetContent(Idb, model.Department, false)) // вызываем метод проверки Id
              {
                    try
                    {
                        using (BiblioDbContext _db = new BiblioDbContext())
                        {
                            string folder = GetFilePath(Idb);
                            if (!Directory.Exists(folder))
                            {
                                Directory.CreateDirectory(folder);
                            }
                            _db.inventionbiblio.Add(model);
                            _db.SaveChanges();
                        }

                        using (LogdbContent _db2 = new LogdbContent())
                        {
                            ActionLog actionlog = new ActionLog();
                            actionlog.Name = "null"; // Membership.GetUser().UserName;
                            GetIP gp = new GetIP();
                            string userip = gp.GetIPAddress();
                            actionlog.Ip = userip;
                            actionlog.ActionDate = DateTime.Now;
                            actionlog.Procedure = "Create";

                            _db2.alog.Add(actionlog);
                            _db2.SaveChanges();
                        }
                        return model.Numerator;
                    }
                    catch
                    {
                     return null;
                    }
              }
                return null;
            }
            return null;
        }

        public string Enter(Enterbiblio model)
        {

            if (model.Numerator != null)
            {
                try
                {
                    using (BiblioDbContext _db = new BiblioDbContext())
                    {

                        _db.Entry(model).State = System.Data.Entity.EntityState.Modified;
                        _db.SaveChanges();
                    }
                    return model.Numerator;
                }
                catch
                {
                    return null;
                }

            }
            return null;
        }

        public string Invention(Inventionbiblio model)
        {

            if (model.Numerator != null)
            {
                try
                {
                    using (BiblioDbContext _db = new BiblioDbContext())
                    {
                        _db.inventionbiblio.Add(model);
                        _db.SaveChanges();
                    }
                    return model.Numerator;
                }
                catch
                {
                    return null;
                }

            }
            return null;
        }

        public bool Inventors(Invent model)
        {

            if (model != null)
            {
                try
                {
                    using (BiblioDbContext _db = new BiblioDbContext())
                    {
                            _db.inventors.Add(model);
                            _db.SaveChanges();
                    }
                    return true;
                }
                catch
                {
                    return false;
                }

            }
            return false;
        }

        public bool Appl(Appl model)
        {

            if (model != null)
            {
                try
                {
                    using (BiblioDbContext _db = new BiblioDbContext())
                    {
                        _db.aplicants.Add(model);
                        _db.SaveChanges();
                    }
                    return true;
                }
                catch
                {
                    return false;
                }

            }
            return false;
        }

        public bool Agent(Agent model)
        {

            if (model != null)
            {
                try
                {
                    using (BiblioDbContext _db = new BiblioDbContext())
                    {
                        _db.agents.Add(model);
                        _db.SaveChanges();
                    }
                    return true;
                }
                catch
                {
                    return false;
                }

            }
            return false;
        }
        public bool Priority(Priority model)
        {

            if (model != null)
            {
                try
                {
                    using (BiblioDbContext _db = new BiblioDbContext())
                    {
                        _db.priority.Add(model);
                        _db.SaveChanges();
                    }
                    return true;
                }
                catch
                {
                    return false;
                }

            }
            return false;
        }

        public string GetFilePath(string num)
        {
            return HttpContext.Current.Server.MapPath("~/appb/"+num);
        }

        public string[] GetNews(string path)
        {
            return File.ReadAllLines(path, Encoding.UTF8);
        }
        //-----------------------Проверяем в таблице значения параметра Id если есть то возвращаем лож иначе истина--------------------
        public bool GetContent (string num, string Department, bool create)
            {
            try
            {
                using (BiblioDbContext _db = new BiblioDbContext())
                {
                    var invention = from inv in _db.inventionbiblio
                                    where inv.Numerator == num && inv.Department == Department
                                    select inv;
                    if (invention.Count() > 0)
                    {
                        create = false;
                        return create;
                    }
                    else
                    {
                        create = true;
                        return create;
                    }
                }
            }
            catch
            {
                create = false;
                return create;
            }

            
        }
     
    }
}