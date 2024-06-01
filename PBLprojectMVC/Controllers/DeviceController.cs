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

            // Image is required on Insert
            if (model.Image == null && operation == "I")
                ModelState.AddModelError("Image", "Escolha uma imagem.");
            if (model.Image != null && model.Image.Length / 1024 / 1024 >= 2)
                ModelState.AddModelError("Image", "Imagem limitada a 2 mb.");
            if (ModelState.IsValid)
            {
                // Get saved Image on Update if not informed
                if (operation == "A" && model.Image == null)
                {
                    DeviceViewModel device = DAO.Get(model.Id);
                    model.ImageByte = device.ImageByte;
                }
                else
                {
                    model.ImageByte = ConvertImageToByte(model.Image);
                }
            }
        }

        public byte[] ConvertImageToByte(IFormFile file)
        {
            if (file != null)
                using (var ms = new MemoryStream())
                {
                    file.CopyTo(ms);
                    return ms.ToArray();
                }
            else
                return null;
        }

    }
}
