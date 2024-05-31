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
                try
                {
                    var list = DAO.GetAll();
                    return View(NameViewIndex, list);
                }
                catch (Exception error)
                {
                    return View("Error", new ErrorViewModel(error.ToString()));
                }
            }
            else
            {
                return new RedirectToActionResult("AcessoNegado", "Error", null);
            }
        }

        public virtual IActionResult Create()
        {
            if (HelperController.VerificaAdmin(HttpContext.Session) || NovoUsuario)
            {
                try
                {
                    ViewBag.Operacao = "I";
                    T model = Activator.CreateInstance<T>();
                    PreencheDadosParaView("I", model);
                    return View(NomeViewForm, model);
                }
                catch (Exception erro)
                {
                    return View("Error", new ErrorViewModel(erro.ToString()));
                }
            }
            else
            {
                return new RedirectToActionResult("AcessoNegado", "Error", null);
            }
        }

        public virtual IActionResult Save(T model, string Operacao)
        {
            bool Admin = HelperController.VerificaAdmin(HttpContext.Session);
            if (Admin || NovoUsuario)
            {
                try
                {
                    ValidaDados(model, Operacao);
                    if (ModelState.IsValid == false)
                    {
                        ViewBag.Operacao = Operacao;
                        PreencheDadosParaView(Operacao, model);
                        return View(NomeViewForm, model);
                    }
                    else
                    {
                        if (Operacao == "I")
                            DAO.Insert(model);
                        else
                            DAO.Update(model);

                        if (NovoUsuario && !Admin)
                        {
                            return RedirectToAction("Index", "Login");
                        }
                        else
                        {
                            return RedirectToAction(NomeViewIndex);
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
                return new RedirectToActionResult("AcessoNegado", "Error", null);
            }
        }

        public IActionResult Edit(int id)
        {
            if (HelperController.VerificaAdmin(HttpContext.Session))
            {
                try
                {
                    ViewBag.Operacao = "A";
                    var model = DAO.Consulta(id);
                    if (model == null)
                        return RedirectToAction(NomeViewIndex);
                    else
                    {
                        PreencheDadosParaView("A", model);
                        return View(NomeViewForm, model);
                    }
                }
                catch (Exception erro)
                {
                    return View("Error", new ErrorViewModel(erro.ToString()));
                }
            }
            else
            {
                return new RedirectToActionResult("AcessoNegado", "Error", null);
            }
        }

        public IActionResult Delete(int id)
        {
            if (HelperController.VerificaAdmin(HttpContext.Session))
            {
                try
                {
                    DAO.Delete(id);
                    return RedirectToAction(NomeViewIndex);
                }
                catch (Exception erro)
                {
                    return View("Error", new ErrorViewModel(erro.ToString()));
                }
            }
            else
            {
                return new RedirectToActionResult("AcessoNegado", "Error", null);
            }
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (ExigeAutenticacao && !HelperController.VerificaUserLogado(HttpContext.Session))
            {
                if (NovoUsuario)
                {
                    base.OnActionExecuting(context);
                }
                else
                {
                    context.Result = RedirectToAction("Index", "Login");
                }

            }
            else if (ExigeAdmin && !HelperController.VerificaAdmin(HttpContext.Session))
            {
                context.Result = new RedirectToActionResult("AcessoNegado", "Error", null);
            }
            else
            {
                base.OnActionExecuting(context);
            }
        }
    }
}
