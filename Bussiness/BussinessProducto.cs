using Data;
using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bussiness
{
    public class BussinessProducto
    {
        private DataProducto cd_Productos = new DataProducto();

        public List<Producto> ListarProductos()
        {
            return cd_Productos.Listar();
        }

        public string Registrar(Producto obj, out string Mensaje)
        {
            Mensaje = String.Empty;

            if (string.IsNullOrEmpty(obj.Nombre) || string.IsNullOrWhiteSpace(obj.Nombre))
            {
                Mensaje = "El nombre no puede estar vacía";
            }

            else if (string.IsNullOrEmpty(obj.DescripcionProducto) || string.IsNullOrWhiteSpace(obj.DescripcionProducto))
            {
                Mensaje = "La descripción no puede estar vacía";
            }

            else if (obj.objetoMarca.IdMarca ==0)
            {
                Mensaje = "Debe seleccionar una marca";
            }

            else if (obj.objetoCategoria.IdCategoria == 0)
            {
                Mensaje = "Debe seleccionar una Categoría";
            }

            else if (obj.Precio == 0)
            {
                Mensaje = "Debe ingresar el precio del producto ";
            }

            else if (obj.Stock == 0)
            {
                Mensaje = "Debe ingresar el stock del producto ";
            }

            if (string.IsNullOrEmpty(Mensaje))
            {
                return cd_Productos.Registrar(obj, out Mensaje);

            }

            return Mensaje;
        }

        public string Editar(Producto obj, out string Mensaje)
        {
            Mensaje = String.Empty;

            if (string.IsNullOrEmpty(obj.Nombre) || string.IsNullOrWhiteSpace(obj.Nombre))
            {
                Mensaje = "El nombre no puede estar vacía";
            }

            else if (string.IsNullOrEmpty(obj.DescripcionProducto) || string.IsNullOrWhiteSpace(obj.DescripcionProducto))
            {
                Mensaje = "La descripción no puede estar vacía";
            }

            else if (obj.objetoMarca.IdMarca == 0)
            {
                Mensaje = "Debe seleccionar una marca";
            }

            else if (obj.objetoCategoria.IdCategoria == 0)
            {
                Mensaje = "Debe seleccionar una Categoría";
            }

            else if (obj.Precio == 0)
            {
                Mensaje = "Debe ingresar el precio del producto ";
            }

            else if (obj.Stock == 0)
            {
                Mensaje = "Debe ingresar el stock del producto ";
            }

            if (string.IsNullOrEmpty(Mensaje))
            {
                return cd_Productos.Editar(obj, out Mensaje);

            }
            return Mensaje;
        }

        public bool GuardarDatosImagen(Producto obj, out string Mensaje)
        {
            
            return cd_Productos.GuardarDatosImagen(obj, out Mensaje);
        }


        public string Eliminar(int id, out string Mensaje)
        {
            Mensaje = String.Empty;
            if (string.IsNullOrEmpty(Mensaje))
            {

                return cd_Productos.Eliminar(id, out Mensaje);
            }
            return Mensaje;
        }
    }
}

