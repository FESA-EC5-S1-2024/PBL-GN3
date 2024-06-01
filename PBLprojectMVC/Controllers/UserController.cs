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
            NewUser = true;
        }

        protected override void ValidateData(UserViewModel model, string Operation)
        {
            if (string.IsNullOrEmpty(model.Name))
                ModelState.AddModelError("Name", "Preencha o Name.");

            if (string.IsNullOrEmpty(model.Email))
                ModelState.AddModelError("Email", "Preencha o Email.");

            if (string.IsNullOrEmpty(model.Password))
                ModelState.AddModelError("Password", "Adicione uma Password.");
        }
    }
}
