using AppLogin.Data;
using AppLogin.ViewModels;
using Microsoft.AspNetCore.Mvc;
using AppLogin.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace AppLogin.Controllers
{
    public class AccessController : Controller
    {
        //Permite interactuar con la base de datos
        private readonly DBContext _dbContext;
        //Contructor que recive la conexion de la base de datos
        public AccessController(DBContext dBContext)
        {
            _dbContext = dBContext;
        }
        [HttpGet]
        //Se encargara de registrar a los usuarios
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(UserVM userVM)
        {
            //Procesa la validacion del modelo
            if (ModelState.IsValid)
            {
                //Pasar el UserModel y verifica si las claves son correctas
                if (userVM.Password != userVM.ConfirmPassword)
                {
                    ViewData["Mensaje"] = "Las contraseñas no coinciden";
                    return View();
                }

                User user = new User()
                {
                    Name = userVM.Name,
                    Password = userVM.Password,
                    Email = userVM.Email
                };

                await _dbContext.Users.AddAsync(user);
                await _dbContext.SaveChangesAsync();

                if (user.Id != 0) return RedirectToAction("Index", "Access");

                ViewData["Mensaje"] = "No se pudo crear el usuario, error fatal";

                return View();
            } 
            else
            {
                return View(userVM);
            }
            
        }

        [HttpGet]
        //Se encargara de registrar a los usuarios
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginVM loginVM)
        {
            User? user_encontrado = await _dbContext.Users
                .Where(u => 
                u.Email == loginVM.Email &&
                u.Password == loginVM.Password
                ).FirstOrDefaultAsync();
            if (user_encontrado == null)
            {
                ViewData["Mensaje"] = "No se encontraron coincidencias";

                return View();
            }
            List <Claim> claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, user_encontrado.Name)
            };

            ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims,CookieAuthenticationDefaults.AuthenticationScheme);
            AuthenticationProperties properties = new AuthenticationProperties()
            {
                AllowRefresh = true,
            };

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                properties
                );



           return RedirectToAction("Index", "Home");
        }
    }
}