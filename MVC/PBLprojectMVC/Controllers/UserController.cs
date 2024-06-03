using Microsoft.AspNetCore.Mvc;
using PBLprojectMVC.DAO;
using PBLprojectMVC.Models;
using Microsoft.AspNetCore.Mvc.Filters;

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
            LoginDAO DAO = new LoginDAO();

            if (string.IsNullOrEmpty(model.Name))
                ModelState.AddModelError("Name", "Preencha o Name.");

            if (string.IsNullOrEmpty(model.Email))
                ModelState.AddModelError("Email", "Preencha o Email.");

            if (string.IsNullOrEmpty(model.Password) || model.Password.Length < 8)
                ModelState.AddModelError("Password", "Adicione uma Password.");
        }

        public IActionResult UpdateAdminStatus(int id, bool isAdmin) {

            UserDAO DAO = new UserDAO();
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

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            ViewBag.UserLogged = HelperController.LoginSessionVerification(HttpContext.Session);
            ViewBag.IsAdmin = HelperController.AdminSessitionVerification(HttpContext.Session);

            if (NeedsAuthentication && HelperController.AdminSessitionVerification(HttpContext.Session))
            {
                base.OnActionExecuting(context);

            }
            else
            {
                context.Result = RedirectToAction("Index", "Home");
            }
        }
    }
}
