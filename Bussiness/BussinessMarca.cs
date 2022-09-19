using Data;
using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bussiness
{
    public class BussinessMarca
    {
        private DataMarcas cd_Marcas = new DataMarcas();

        public List<Marca> ListarMarcas()
        {
            return cd_Marcas.Listar();
        }

        public string Registrar(Marca obj, out string Mensaje)
        {
            Mensaje = String.Empty;

            if (string.IsNullOrEmpty(obj.NombreMarca) || string.IsNullOrWhiteSpace(obj.NombreMarca))
            {
                Mensaje = "El nombre no puede estar vacío";
            }

            if (string.IsNullOrEmpty(Mensaje))
            {
                return cd_Marcas.Registrar(obj, out Mensaje);

            }

            return Mensaje;
        }

        public string Editar(Marca obj, out string Mensaje)
        {
            Mensaje = String.Empty;

            if (string.IsNullOrEmpty(obj.NombreMarca) || string.IsNullOrWhiteSpace(obj.NombreMarca))
            {
                Mensaje = "El nombre no puede estar vacío";
            }
            else
            {
                Mensaje = String.Empty;
            }

            if (string.IsNullOrEmpty(Mensaje))
            {
                return cd_Marcas.Editar(obj, out Mensaje);

            }
            return Mensaje;
        }

        public string Eliminar(int id, out string Mensaje)
        {
            Mensaje = String.Empty;
            if (string.IsNullOrEmpty(Mensaje))
            {

                return cd_Marcas.Eliminar(id, out Mensaje);
            }
            return Mensaje;
        }
    }
}

