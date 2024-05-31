using Microsoft.AspNetCore.Mvc;
using PBLprojectMVC.DAO;
using PBLprojectMVC.Models;

namespace PBLprojectMVC.Controllers
{
    public class UserController : StandardController<UserViewModel>
    {
        public UserController()
        {
            DAO = new UserDAO();
            NovoUsuario = true;
        }

        protected override void ValidaDados(UserViewModel model, string operacao)
        {
            if (string.IsNullOrEmpty(model.Nome))
                ModelState.AddModelError("Name", "Preencha o nome.");

            if (string.IsNullOrEmpty(model.Email))
                ModelState.AddModelError("Email", "Preencha o Email.");

            if (string.IsNullOrEmpty(model.Senha))
                ModelState.AddModelError("Password", "Adicione uma senha.");
        }
    }
}
