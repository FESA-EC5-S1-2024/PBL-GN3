using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using PBLprojectMVC.Models;
using PBLprojectMVC.DAO;

namespace PBLprojectMVC.Controllers
{
    public class StandardController<T> : Controller where T : StandardViewModel
    {
        // Standard Controller for CRUD operations
        protected StandardDAO<T> DAO { get; set; }
        protected bool NeedsAuthentication { get; set; } = true;

        // Default view names
        protected string IndexViewName { get; set; } = "Index";
        protected string FormViewName { get; set; } = "Form";
        
        protected bool RequiresAdmin { get; set; } = true;
        protected bool NewUser { get; set; } = false;
        protected virtual void ValidateData(T model, string operation) { }
        protected virtual void FillDataForView(string operation, T model) { }

        public virtual IActionResult Index()
        {
            try
            {
                ViewBag.UserLogged = HelperController.LoginSessionVerification(HttpContext.Session);
                ViewBag.IsAdmin = HelperController.AdminSessitionVerification(HttpContext.Session);

                return View();
            }
            catch (Exception error)
            {
                return View("Error", new ErrorViewModel(error.ToString()));
            }
        }
        
        public virtual IActionResult Create()
        {
            if (HelperController.AdminSessitionVerification(HttpContext.Session) || NewUser)
            {
                try
                {
                    ViewBag.Operation = "I";
                    T model = Activator.CreateInstance<T>();
                    FillDataForView("I", model);
                    return View(FormViewName, model);
                }
                catch (Exception error)
                {
                    return View("Error", new ErrorViewModel(error.ToString()));
                }
            }
            else
            {
                return View("Error", new ErrorViewModel("Access denied."));
            }
        }
        
        public virtual IActionResult Save(T model, string operation)
        {
            bool admin = HelperController.AdminSessitionVerification(HttpContext.Session);
            if (admin || NewUser)
            {
                try
                {
                    ValidateData(model, operation);
                    if (ModelState.IsValid == false)
                    {
                        ViewBag.Operation = operation;
                        FillDataForView(operation, model);
                        return View(FormViewName, model);
                    }
                    else
                    {
                        if (operation == "I")
                            DAO.Insert(model);
                        else
                            DAO.Update(model);

                        if (NewUser && !admin)
                        {
                            return RedirectToAction("Index", "Login");
                        }
                        else
                        {
                            return RedirectToAction(IndexViewName);
                        }
                    }
                }
                catch (Exception erro)
                {
                    return View("Error", new ErrorViewModel(erro.ToString()));
                }
            }
            else
            {
                return View("Error", new ErrorViewModel("Access denied."));
            }
        }

        public IActionResult Edit(int id)
        {
            if (HelperController.AdminSessitionVerification(HttpContext.Session))
            {
                try
                {
                    ViewBag.Operation = "A";
                    var model = DAO.Get(id);
                    if (model == null)
                        return RedirectToAction(IndexViewName);
                    else
                    {
                        FillDataForView("A", model);
                        return View(FormViewName, model);
                    }
                }
                catch (Exception error)
                {
                    Console.WriteLine(error.ToString());
                    return View("Error", new ErrorViewModel(error.ToString()));
                }
            }
            else
            {
                return View("Error", new ErrorViewModel("Access denied."));
            }
        }

        public virtual IActionResult Delete(int id)
        {
            if (HelperController.AdminSessitionVerification(HttpContext.Session))
            {
                try
                {
                    DAO.Delete(id);
                    return RedirectToAction(IndexViewName);
                }
                catch (Exception error)
                {
                    return View("Error", new ErrorViewModel(error.ToString()));
                }
            }
            else
            {
                return View("Error", new ErrorViewModel("Access denied."));
            }
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            ViewBag.UserLogged = HelperController.LoginSessionVerification(HttpContext.Session);
            ViewBag.IsAdmin = HelperController.AdminSessitionVerification(HttpContext.Session);

            if (NeedsAuthentication && !HelperController.LoginSessionVerification(HttpContext.Session))
            {
                if (NewUser)
                {
                    base.OnActionExecuting(context);
                }
                else
                {
                    context.Result = RedirectToAction("Index", "Login");
                }

            }
            else
            {
                base.OnActionExecuting(context);
            }
        }
    }
}
