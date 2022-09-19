using Bussiness;
using Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AdminPlatform.Controllers
{
    [Authorize]
    public class Marcas : Controller
    {
        private BussinessMarca _bMarcas = new BussinessMarca();
        public IActionResult marcas()
        {
            return View();
        }

        [HttpGet]
        public JsonResult ListaMarcas()
        {
            List<Marca> listaMarcas = new List<Marca>();
            listaMarcas = _bMarcas.ListarMarcas();
            return Json(new { data = listaMarcas });
        }

        [HttpPost]
        public JsonResult GuardarMarca([FromBody] Marca obj)
        {
            string Mensaje = string.Empty;
            string operacion = Request.Headers["operacion"];
            object respuesta;

            if (operacion == "crear")
            {
                respuesta = _bMarcas.Registrar(obj, out Mensaje);
            }
            else
            {
                respuesta = _bMarcas.Editar(obj, out Mensaje);
            }


            return Json(new { respuesta = respuesta, mensaje = Mensaje });
        }

        [HttpDelete]
        public JsonResult EliminarMarca(int idMarca)
        {
            string Mensaje = string.Empty;
            object respuesta;

            respuesta = _bMarcas.Eliminar(idMarca, out Mensaje);
            return Json(new { respuesta = respuesta, mensaje = Mensaje });
        }

    }

}

