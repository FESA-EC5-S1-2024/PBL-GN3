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
            NeedsAuthentication = true;
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

        public IActionResult UpdateAdminStatus(int id, bool isAdmin, int PersonId) {

            UserViewModel user_base = DAO.Get(id);

            try {

                DAO.Update(new UserViewModel{

                Id = id, 
                Name = user_base.Name,
                Email = user_base.Email,
                Password = user_base.Password,
                IsAdmin = isAdmin,

                });

                return Json(new { success = true });

            } catch (Exception ex) {
                return Json(new { success = false, message = ex.Message });
            }
            
        }  

        public IActionResult SearchUsers(string query) {

            UserDAO DAO = new UserDAO();
            return Json(DAO.GetSearchUsers(query));
            
        }
    }
}
