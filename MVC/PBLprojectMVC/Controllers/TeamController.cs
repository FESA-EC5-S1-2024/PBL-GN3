using Microsoft.AspNetCore.Mvc;
using PBLprojectMVC.DAO;
using PBLprojectMVC.Models;

namespace PBLprojectMVC.Controllers
{
    public class TeamController : StandardController<TeamViewModel>
    {
        public TeamController()
        {
            DAO = new TeamDAO();
        }

        public override IActionResult Index()
        {

            ViewBag.UserLogged = HelperController.LoginSessionVerification(HttpContext.Session);
            ViewBag.IsAdmin = HelperController.AdminSessitionVerification(HttpContext.Session);

            try
            {
                var list = DAO.GetAll();
                return View(IndexViewName, list);
            }
            catch (Exception error)
            {
                return View("Error", new ErrorViewModel(error.ToString()));
            }
        }

        protected override void ValidateData(TeamViewModel model, string operation)
        {
            if (model.Name == default)
                ModelState.AddModelError("Name", "Preencha o nome da equipe.");

            if (model.Description == default)
                ModelState.AddModelError("Description", "Preencha a descrição da equipe.");
        }
    }
}
