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

        protected override void ValidaDados(TemperatureViewModel model, string operacao)
        {
            if (model.Instante == default(DateTime))
                ModelState.AddModelError("Instante", "Preencha o instante.");

            if (model.Valor == default(float))
                ModelState.AddModelError("Valor", "Preencha o valor da temperatura.");

            if (model.DispositivoId == 0)
                ModelState.AddModelError("DispositivoId", "Selecione um dispositivo.");
        }

    }
}
