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

        protected override void ValidateData(DeviceViewModel model, string operation)
        {
            if (string.IsNullOrEmpty(model.Name))
                ModelState.AddModelError("Name", "Preencha o nome.");

            if (string.IsNullOrEmpty(model.Type))
                ModelState.AddModelError("Type", "Preencha o tipo.");
        }

    }
}
