using Data;
using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bussiness
{
    public class BussinessCategorias
    {
        private DataCategorias cd_Categorias = new DataCategorias();

        public List<Categoria> ListarCategorias()
        {
            return cd_Categorias.Listar();
        }

        public string Registrar(Categoria obj, out string Mensaje)
        {
            Mensaje = String.Empty;

            if (string.IsNullOrEmpty(obj.NombreCategoria) || string.IsNullOrWhiteSpace(obj.NombreCategoria))
            {
                Mensaje = "El nombre no puede estar vacío";
            }

            if (string.IsNullOrEmpty(Mensaje))
            {
                return cd_Categorias.Registrar(obj, out Mensaje);

            } 

            return Mensaje;
        }

        public string Editar(Categoria obj, out string Mensaje)
        {
            Mensaje = String.Empty;

            if (string.IsNullOrEmpty(obj.NombreCategoria) || string.IsNullOrWhiteSpace(obj.NombreCategoria))
            {
                Mensaje = "El nombre no puede estar vacío";
            }

            if (string.IsNullOrEmpty(Mensaje))
            {
                return cd_Categorias.Editar(obj, out Mensaje);

            }
            return Mensaje;
        }

        public string Eliminar(int id, out string Mensaje)
        {
            Mensaje = String.Empty;
            if (string.IsNullOrEmpty(Mensaje))
            {

                return cd_Categorias.Eliminar(id, out Mensaje);
            }
            return Mensaje;
        }
    }

}

