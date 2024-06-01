using Microsoft.AspNetCore.Mvc;
using PBLprojectMVC.DAO;
using PBLprojectMVC.Models;

namespace PBLprojectMVC.Controllers
{
    public class DeviceController : StandardController<DeviceViewModel>
    {
        public DeviceController()
        {
            DAO = new DeviceDAO();
        }

        public override IActionResult Index()
        {
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

        protected override void ValidateData(DeviceViewModel model, string operation)
        {
            ModelState.Clear();
            if (string.IsNullOrEmpty(model.Name))
                ModelState.AddModelError("Name", "Preencha o nome do dispositivo.");

            if (string.IsNullOrEmpty(model.Type))
                ModelState.AddModelError("Type", "Preencha o tipo.");
        }

    }
}
