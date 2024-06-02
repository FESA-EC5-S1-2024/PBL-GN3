using Microsoft.AspNetCore.Mvc;
using PBLprojectMVC.DAO;
using PBLprojectMVC.Models;

namespace PBLprojectMVC.Controllers
{
    public class DeviceController : StandardController<DeviceViewModel>
    {
        private readonly DeviceService deviceService;
        public DeviceController()
        {
            DAO = new DeviceDAO();
            deviceService = new DeviceService();
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

        public override IActionResult Save(DeviceViewModel model, string operation)
        {
            IActionResult action = base.Save(model, operation);
            if (ModelState.IsValid && operation == "I")
            {
                var response = RegisterDevice(model.Name);
            }
            return action;
        }

        public override IActionResult Delete(int id)
        {
            var device = DAO.Get(id);
            IActionResult action = base.Delete(id);
            if (ModelState.IsValid)
            {
                var response = DeleteDevice(device.Name);
            }
            return action;
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

        public async Task<string> RegisterDevice(string deviceName)
        {
            string host = "your_host";
            int port = 4041;                // IoT-Agent MQTT Port
            string deviceId = deviceName;
            string entityName = "urn:ngsi-ld:" + GetEntityName(deviceName); //URN format

            var response = await deviceService.RegisterDeviceAsync(host, port, deviceId, entityName);
            
            if (response.IsSuccessStatusCode)
            {
                // Handle success response
                ViewBag.Message = "Device registered successfully.";
            }
            else
            {
                // Handle error response
                var errorMessage = await response.Content.ReadAsStringAsync();
                ViewBag.Message = $"Error: {errorMessage}";
            }
            return ViewBag.Message;
        }

        public static string GetEntityName(string input)
        {
            var prefix = string.Concat(input.TakeWhile(char.IsLetter));
            var numericPart = string.Concat(input.SkipWhile(char.IsLetter));
            if (string.IsNullOrEmpty(numericPart))
            {
                numericPart = "001";
            }
            prefix = char.ToUpper(prefix[0]) + prefix.Substring(1);
            return $"{prefix}:{numericPart}";
        }

        public async Task<string> DeleteDevice(string deviceId)
        {
            string host = "your_host";
            int port = 4041;    // IoT-Agent MQTT Port

            var response = await deviceService.DeleteDeviceAsync(host, port, deviceId);

            if (response.IsSuccessStatusCode)
            {
                // Handle success response
                ViewBag.Message = "Device deleted successfully.";
            }
            else
            {
                // Handle error response
                var errorMessage = await response.Content.ReadAsStringAsync();
                ViewBag.Message = $"Error: {errorMessage}";
            }

            return ViewBag.Message;
        }
    }
}
