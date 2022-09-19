using Bussiness;
using Data;
using Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AdminPlatform.Controllers
{
    [Authorize(Roles ="Administrador")]
    public class Usuarios : Controller
    {
        private BussinessUsuarios _bUsuarios = new BussinessUsuarios();
        public IActionResult usuarios()
        {
            return View();
        }

        [HttpGet]
        public JsonResult ListaUsuarios()
        {
            List<Usuario> listaUsuarios = new List<Usuario>();
            listaUsuarios = _bUsuarios.ListarUsuarios();
            return Json(new { data = listaUsuarios });
        }

        [HttpPost]
        public JsonResult GuardarUsuario([FromBody] Usuario obj)
        {
            string Mensaje = string.Empty;
            string operacion = Request.Headers["operacion"];
            object respuesta;

            if (operacion == "crear")
            {
                respuesta = _bUsuarios.Registrar(obj, out Mensaje);
            }
            else
            {
                respuesta = _bUsuarios.Editar(obj, out Mensaje);
            }


            return Json(new { respuesta = respuesta, mensaje= Mensaje });
        }

        [HttpDelete]
        public JsonResult EliminarUsuario(int idUsuario)
        {
            string Mensaje = string.Empty;
            bool respuesta;
            respuesta = _bUsuarios.Eliminar(idUsuario, out Mensaje);
            return Json(new { respuesta = respuesta });
        }

    }
}
