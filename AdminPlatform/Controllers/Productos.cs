using Bussiness;
using Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Scripting;
using Microsoft.Extensions.FileProviders;
using Newtonsoft.Json;
using System.Globalization;
using System.Web;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Authorization;

namespace AdminPlatform.Controllers
{
    [Authorize]
    public class Productos : Controller
    {
        private readonly IWebHostEnvironment _appEnviroment;
        public Productos (IWebHostEnvironment appEnviroment)
        {
            _appEnviroment = appEnviroment;
        }

        private BussinessProducto _bProductos = new BussinessProducto();
        public IActionResult productos()
        {
            return View();
        }

        [HttpGet]
        public JsonResult ListaProductos()
        {
            List<Producto> listaProductos = new List<Producto>();
            listaProductos = _bProductos.ListarProductos();
            return Json(new { data = listaProductos });
        }

        [HttpPost]
        public JsonResult GuardarProducto(string obj, IFormFile archivoImage)
        {
            string Mensaje = string.Empty;
            string operacion = Request.Headers["operacion"];
            object respuesta;

            bool operacion_exitosa = false;
            bool guardar_imagen_exitosa = true;

            Producto objProducto = new Producto();
            objProducto = JsonConvert.DeserializeObject<Producto>(obj);
            decimal precio;

            if (decimal.TryParse(objProducto.PrecioTexto,System.Globalization.NumberStyles.AllowDecimalPoint,new CultureInfo("es-ES"),out precio))
            {
                objProducto.Precio = precio;
            }
            else
            {
                return Json(new {operacion_exitosa= false, mensaje = "El formato del precio debe ser ##,##"});
            }

            if (operacion == "crear")
            {
                respuesta = _bProductos.Registrar(objProducto, out Mensaje);
                operacion_exitosa = true;
            }
            else
            {
                respuesta = _bProductos.Editar(objProducto, out Mensaje);
                operacion_exitosa = true;
            }
            
            if (operacion_exitosa)
            {
                if(archivoImage != null)
                {
                    string ruta_guardar = _appEnviroment.WebRootPath + "\\FOTOS_CARRITO\\";
                    string extension = Path.GetExtension(archivoImage.FileName);
                    string nombre_imagen = String.Concat(objProducto.Nombre.ToString(), extension);
                    string path_to_images = ruta_guardar + nombre_imagen;
                    Stream fileStream = new FileStream(path_to_images, FileMode.Create, FileAccess.ReadWrite);
                    
                    try
                    {
                        archivoImage.CopyTo(fileStream);
                    }
                    catch(Exception ex)
                    {
                        string msg = ex.Message;
                        guardar_imagen_exitosa = false;
                    }

                    if (guardar_imagen_exitosa)
                    {
                        objProducto.RutaImagen = ruta_guardar;
                        objProducto.NombreImagen = nombre_imagen;
                        bool respta = _bProductos.GuardarDatosImagen(objProducto, out Mensaje);
                    }
                    else
                    {
                        Mensaje = "Se ha guardado el producto, pero hubo problemas con la imagen";
                    }
                }
            }
            


            return Json(new { respuesta = respuesta, mensaje = Mensaje });
        }


        [HttpPost]
        public JsonResult ImagenProducto([FromBody] int id) 
        {
            string Mensaje = string.Empty;
            string textoBase64 = string.Empty;
            bool conversion = false;
            Producto objProducto = _bProductos.ListarProductos().Where(p => p.IdProducto == id).FirstOrDefault();
            if (objProducto != null)
            {
                textoBase64 = BussinessRecursos.ConvertirBase64(Path.Combine(objProducto.RutaImagen, objProducto.NombreImagen), conversion = true);
            }
            else
            {
                Mensaje = "No se ha podido encontrar la imagen";
            }

            return Json(new {
                conversion = conversion,
                textoBase64 = textoBase64,
                extension = Path.GetExtension(objProducto.NombreImagen
                ),
                Mensaje = Mensaje
            });
        }

        [HttpDelete]
        public JsonResult EliminarProducto(int idProducto)
        {
            string Mensaje = string.Empty;
            object respuesta;

            respuesta = _bProductos.Eliminar(idProducto, out Mensaje);
            return Json(new { respuesta = respuesta, mensaje = Mensaje });
        }

    }

}
