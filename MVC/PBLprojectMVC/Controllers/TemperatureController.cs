using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using PBLprojectMVC.DAO;
using PBLprojectMVC.Models;
using PBLprojectMVC.Services;
using PBLprojectMVC.Utils;
using System;
using System.Text.Json;

namespace PBLprojectMVC.Controllers
{
    public class TemperatureController : StandardController<TemperatureViewModel>
    {
        private readonly TemperatureService _temperatureService;
        public TemperatureController()
        {
            DAO = new TemperatureDAO();
            _temperatureService = new TemperatureService();
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

        public async Task<ActionResult> GetTemperaturePartial(string deviceName, DateTime dateFrom, DateTime dateTo, int hLimit, int hOffset)
        {
            try
            {
                if (hLimit <= 0)
                    hLimit = 10;
                string entityName = GetEntityName(deviceName);
                List<TemperatureViewModel> temperatures = await _temperatureService.GetTemperatureValues(entityName, dateFrom, dateTo, hLimit, hOffset);
                return PartialView("pvGridTemperatures", temperatures);
            }
            catch (Exception error)
            {
                return Json(new { error = true, msg = error.Message });
            }
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
            return $"urn:ngsi-ld:{prefix}:{numericPart}";
        }

        public IActionResult AdvancedQuery()
        {
            try
            {
                ViewBag.UserLogged = HelperController.LoginSessionVerification(HttpContext.Session);
                ViewBag.IsAdmin = HelperController.AdminSessitionVerification(HttpContext.Session);
                LoadDeviceListCombo();
                return View("AdvancedQuery");
            }
            catch (Exception error)
            {
                return View("Error", new ErrorViewModel(error.Message));
            }
        }

        private void LoadDeviceListCombo()
        {
            DeviceDAO deviceDAO = new DeviceDAO();
            var devices = deviceDAO.GetAll();
            List<SelectListItem> listDevices = new List<SelectListItem>
            {
                new SelectListItem("Selecione um dispositivo...", "0")
            };
            foreach (var dev in devices)
            {
                SelectListItem item = new SelectListItem(dev.Name, dev.Name);
                listDevices.Add(item);
            }
            ViewBag.Devices = listDevices;
        }

        public IActionResult Dashboard()
        {
            try
            {
                ViewBag.UserLogged = HelperController.LoginSessionVerification(HttpContext.Session);
                ViewBag.IsAdmin = HelperController.AdminSessitionVerification(HttpContext.Session);

                return View("Dashboard");
            }
            catch (Exception error)
            {
                return View("Error", new ErrorViewModel(error.Message));
            }
        }
        public async Task<JsonResult> Request(string lastN, DateTime dateFrom, DateTime dateTo)
        {
            var data = await Request_D(lastN, dateFrom, dateTo);
            return Json(data);
        }

        public async Task<JsonElement> Request_D(string lastN, DateTime dateFrom, DateTime dateTo)
        {
            string port = "8666";
            string host = UtilsParams.Host();

            var client = new HttpClient();
            string url = $"http://{host}:{port}/STH/v2/entities/urn:ngsi-ld:Temp:003/attrs/temperature?type=Temp&&lastN={lastN}";
            if (dateFrom == dateTo)
            {
                dateTo = dateTo.Date.AddHours(23).AddMinutes(59).AddSeconds(59);
            }
            if (dateFrom != default && dateTo != default)
            {
                url += $"&dateFrom={dateFrom:yyyy-MM-ddTHH:mm:ss.fff}&dateTo={dateTo:yyyy-MM-ddTHH:mm:ss.fff}";
            }
            var request = new HttpRequestMessage(HttpMethod.Get, url);
            request.Headers.Add("fiware-service", "smart");
            request.Headers.Add("fiware-servicepath", "/");

            var response = await client.SendAsync(request);
            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();

            using var jsonDoc = JsonDocument.Parse(responseBody);
            return jsonDoc.RootElement.Clone();
        }
    }
}
