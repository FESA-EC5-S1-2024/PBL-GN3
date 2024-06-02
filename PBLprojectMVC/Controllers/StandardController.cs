using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using PBLprojectMVC.DAO;
using PBLprojectMVC.Models;

namespace PBLprojectMVC.Controllers
{
    public class StandardController<T> : Controller where T : StandardViewModel
    {
        protected bool RequiresAuthentication { get; set; } = true;
        protected bool RequiresAdmin { get; set; } = true;
        protected bool NewUser { get; set; } = false;
        protected StandardDAO<T> DAO { get; set; }
        protected string NameViewIndex { get; set; } = "index";
        protected string NameViewForm { get; set; } = "form";
        protected virtual void ValidateData(T model, string operation) { }
        protected virtual void FillDataForView(string operation, T model) { }

        public virtual IActionResult Index()
        {
            if (HelperController.VerificaUserLogado(HttpContext.Session))
            {
                ViewBag.UserLogged = HelperController.LoginSessionVerification(HttpContext.Session);
                ViewBag.IsAdmin = HelperController.AdminSessitionVerification(HttpContext.Session);

                return View();
            }
            else
            {
                return View("Error", new ErrorViewModel("Access denied."));
            }
        }

        public virtual IActionResult Create()
        {
            if (HelperController.VerificaAdmin(HttpContext.Session) || NewUser)
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
            bool admin = HelperController.VerificaAdmin(HttpContext.Session);
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
            if (HelperController.VerificaAdmin(HttpContext.Session))
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
            if (HelperController.VerificaAdmin(HttpContext.Session))
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
            if (NeedsAuthentication && HelperController.LoginSessionVerification(HttpContext.Session))
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
            else if (RequiresAdmin && !HelperController.VerificaAdmin(HttpContext.Session))
            {
                context.Result = View("Error", new ErrorViewModel("Access denied."));
            }
            else
            {
                base.OnActionExecuting(context);
            }
        }
    }
}
