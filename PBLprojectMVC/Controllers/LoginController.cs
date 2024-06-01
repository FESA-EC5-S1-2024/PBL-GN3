using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using PBLprojectMVC.DAO;
using PBLprojectMVC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.WebEncoders.Testing;

namespace PBLprojectMVC.Controllers
{
    public class LoginController : StandardController<UserViewModel>
    {

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }

        public IActionResult NewLogin(){
            return View("Register");
        }

        public IActionResult SaveLogin(UserViewModel model)
        {

            LoginDAO DAO = new LoginDAO();

            model.Password = HashHelper.ComputeSha256Hash(model.Password);
            model.Id = 0;
            model.IsAdmin = false;

            ValidateRegistry(model);

            if(ModelState.IsValid == true){

                DAO.Insert(model);
                return RedirectToAction("Index", "Home");

            }

            return View("Register", model);

        }

        public void ValidateRegistry(UserViewModel model)
        {

            ModelState.Clear();

            if(string.IsNullOrEmpty(model.Password) || model.Password.Length < 8)
                ModelState.AddModelError("Password", "The Password must be longer than 8 caracthers!!!");

        }

        public IActionResult Login(UserViewModel model)
        {
            
            LoginDAO DAO = new LoginDAO();

            if(DAO.LoginExists(model.Email, HashHelper.ComputeSha256Hash(model.Password)))
            {
                HttpContext.Session.SetString("UserLogged", "true");

                if(DAO.IsAdmin(model.Email, HashHelper.ComputeSha256Hash(model.Password)))
                    HttpContext.Session.SetString("IsAdmin", "true");

                HttpContext.Session.SetInt32("ID", DAO.LoginExists(model.Email, HashHelper.ComputeSha256Hash(model.Password), true));

                return RedirectToAction("index", "Home");
            }
            else
            {
                ViewBag.TempData = "Email ou Password Invalida!!!";
                return View("Index");
            } 
        }
    
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index");
        }

    }
}
