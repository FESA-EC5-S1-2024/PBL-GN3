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

        protected override void ValidaDados(DeviceViewModel model, string operacao)
        {
            if (string.IsNullOrEmpty(model.Nome))
                ModelState.AddModelError("Nome", "Preencha o nome.");

            if (string.IsNullOrEmpty(model.Tipo))
                ModelState.AddModelError("Tipo", "Preencha o tipo.");
        }

    }
}
