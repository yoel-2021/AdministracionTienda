using Bussiness;
using Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AdminPlatform.Controllers
{
    [Authorize]
    public class Categorias : Controller
    {
        private BussinessCategorias _bCategorias = new BussinessCategorias();
        public IActionResult categorias()
        {
            return View();
        }

        [HttpGet]
        public JsonResult ListaCategorias()
        {
            List<Categoria> listaCategorias = new List<Categoria>();
            listaCategorias = _bCategorias.ListarCategorias();
            return Json(new { data = listaCategorias });
        }

        [HttpPost]
        public JsonResult GuardarCategoria([FromBody] Categoria obj)
        {
            string Mensaje = string.Empty;
            string operacion = Request.Headers["operacion"];
            object respuesta;

            if (operacion == "crear")
            {
                respuesta = _bCategorias.Registrar(obj, out Mensaje);
            }
            else
            {
                respuesta = _bCategorias.Editar(obj, out Mensaje);
            }


            return Json(new { respuesta = respuesta, mensaje = Mensaje });
        }

        [HttpDelete]
        public JsonResult EliminarCategoria(int idCategoria)
        {
            string Mensaje = string.Empty;
            object respuesta;

            respuesta = _bCategorias.Eliminar(idCategoria, out Mensaje);
            return Json(new { respuesta = respuesta, mensaje= Mensaje });
        }

    }
}
