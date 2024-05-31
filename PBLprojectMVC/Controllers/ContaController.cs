using Microsoft.AspNetCore.Mvc;
using PBLprojectMVC.Models;

namespace PBLprojectMVC.Controllers
{
    public class ContaController : Controller
    {
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(UserViewModel model)
        {
            //TODO ADICIONAR VALIDAÇÃO DE USUARIO
            //ModelState.AddModelError("", "Login inválido.");
            if (ModelState.IsValid)
            {
                if (model.IsAdmin)
                {
                    // Redirecionar para uma área de admin
                    return RedirectToAction("Index", "Admin");
                }
                else
                {
                    return RedirectToAction("Index", "Home");
                }
            }
            return View(model);
        }

        // GET: Conta/Registrar
        public ActionResult Registrar()
        {
            return View();
        }

        // POST: Conta/Registrar
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Registrar(UserViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Adicione a lógica de registro aqui
                // Por exemplo, salvar o usuário no banco de dados

                // Redirecionar para a página de login após o registro bem-sucedido
                return RedirectToAction("Login");
            }

            return View(model);
        }
    }
}
