using Bussiness;
using Entities;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;


namespace AdminPlatform.Controllers
{
    public class AccesoController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult CambiarContrasena()
        {
            return View();
        }

        public IActionResult ReestablecerContrasena()
        {
            return View();
        }

        public IActionResult AccesoNoAutorizado()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(string correo, string contrasena)
        {
            Usuario usuarios = new Usuario();
            usuarios = new BussinessUsuarios().ListarUsuarios().Where(u => u.Correo == correo && u.Contrasena == BussinessRecursos.ConvertirASha256(contrasena)).FirstOrDefault();

            if (usuarios == null)
            {
                ViewBag.Error = "Correo o Contraseña invalido";
            }
            else
            {
                if (usuarios.Reestablecer)
                {
                    TempData["idUsuario"] = usuarios.IdUsuario;
                    return RedirectToAction("CambiarContrasena");

                }

                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, usuarios.Nombre),
                    new Claim("Correo", usuarios.Correo),
                    new Claim(ClaimTypes.Role, usuarios.Rol)
                };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));
                
                ViewBag.Error = null;
                return RedirectToAction("Index", "Home");

            }
            
            return View();
        }

        [HttpPost]
        public IActionResult CambiarContrasena(string idusuario,string contrasenaActual, string nuevacontrasena, string confirmarcontrasena)
        {
            Usuario usuarios = new Usuario();
            usuarios = new BussinessUsuarios().ListarUsuarios().Where(u => u.IdUsuario == int.Parse(idusuario)).FirstOrDefault();

            if(usuarios.Contrasena != BussinessRecursos.ConvertirASha256(contrasenaActual))
            {
                TempData["idUsuario"] = idusuario;
                ViewData["vcontrasena"] = "";
                ViewBag.Error = "La contraseña actual no es correcta";
                return View();
            }
            else if(nuevacontrasena != confirmarcontrasena)
            {
                TempData["idUsuario"] = idusuario;
                ViewData["vcontrasena"] = contrasenaActual;
                ViewBag.Error = "Las contraseñas no coinciden";
                return View();
            }

            ViewData["vcontrasena"] = "";

            nuevacontrasena = BussinessRecursos.ConvertirASha256(nuevacontrasena);

            string mensaje = string.Empty;
            bool respuesta = new BussinessUsuarios().CambiarContrasena(int.Parse(idusuario), nuevacontrasena, out mensaje);

            if (respuesta)
            {
                return RedirectToAction("Index");

            }
            else
            {
                TempData["idUsuario"] = idusuario;
                ViewBag.Error = mensaje;
                return View();
            }

            
        }
        [HttpPost]
        public IActionResult ReestablecerContrasena( string correo)
        {
            Usuario usuarios = new Usuario();
            usuarios = new BussinessUsuarios().ListarUsuarios().Where(item => item.Correo == correo).FirstOrDefault();
            if(usuarios == null)
            {
                ViewBag.Error = "No se encontró un usuario relacionado con ese correo";
                return View();
            }
            else
            {
                string mensaje = string.Empty;
                bool respuesta = new BussinessUsuarios().ReestablecerContrasena(usuarios.IdUsuario, correo, out mensaje);
                if (respuesta)
                {
                    ViewBag.Error = null;
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.Error = mensaje;
                    return View();
                }
            }

        }

        public async Task<IActionResult> CerrarSesion()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Acceso");
        }

    }
}
