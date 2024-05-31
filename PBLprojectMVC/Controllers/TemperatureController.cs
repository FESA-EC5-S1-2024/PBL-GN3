using Microsoft.AspNetCore.Mvc;
using PBLprojectMVC.DAO;
using PBLprojectMVC.Models;

namespace PBLprojectMVC.Controllers
{
    public class TemperatureController : StandardController<TemperatureViewModel>
    {
        public TemperatureController()
        {
            DAO = new TemperatureDAO();
        }

        protected override void ValidateData(TemperatureViewModel model, string operation)
        {
            if (model.Time == default)
                ModelState.AddModelError("Time", "Preencha o instante.");

            if (model.Value == default)
                ModelState.AddModelError("Value", "Preencha o valor da temperatura.");

            if (model.DeviceId <= 0)
                ModelState.AddModelError("DeviceId", "Selecione um dispositivo.");
        }

    }
}
