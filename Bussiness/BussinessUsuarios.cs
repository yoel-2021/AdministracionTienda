using Data;
using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bussiness
{
    public class BussinessUsuarios
    {
        private DataUsuarios cd_Usuarios = new DataUsuarios();

        public List<Usuario> ListarUsuarios()
        {
            return cd_Usuarios.Listar();
        }

        public string Registrar(Usuario obj, out string Mensaje)
        {
            Mensaje = String.Empty;

            if (string.IsNullOrEmpty(obj.Nombre) || string.IsNullOrWhiteSpace(obj.Nombre))
            {
                Mensaje = "El nombre del usuario no puede estar vacío";
            }
            else if (string.IsNullOrEmpty(obj.Apellidos) || string.IsNullOrWhiteSpace(obj.Apellidos))
            {
                Mensaje = "Los apellidos no puede estar vacío";
            }
            else if (string.IsNullOrEmpty(obj.Correo) || string.IsNullOrWhiteSpace(obj.Correo))
            {
                Mensaje = "El correo del usuario no puede estar vacío";
            }
            else if (string.IsNullOrEmpty(obj.Rol) || string.IsNullOrWhiteSpace(obj.Rol))
            {
                Mensaje = "Se debe asignar un rol al usuario";
            }

            if (string.IsNullOrEmpty(Mensaje))
            {
                string contrasena = BussinessRecursos.GenerarContrasena();
                string asunto = "Creación de Cuenta";
                string mensaje_correo = "<h3>Su cuenta fue creada correctamente</h3></br><p>Su contraseña para acceder es: !contrasena!</p>";
                mensaje_correo = mensaje_correo.Replace("!contrasena!", contrasena);

                bool respuesta = BussinessRecursos.EnviarCorreo(obj.Correo, asunto, mensaje_correo);

                Console.Write(respuesta);
                if (respuesta)
                {
                    obj.Contrasena = BussinessRecursos.ConvertirASha256(contrasena);
                    return cd_Usuarios.Registrar(obj, out Mensaje);
                }
                else
                {
                    Mensaje = "No se puede enviar el correo ";
                    return "0";
                }

            }

            else
            {
                return Mensaje;
            }

        }

        public string Editar(Usuario obj, out string Mensaje)
        {
            Mensaje = String.Empty;

            if (string.IsNullOrEmpty(obj.Nombre) || string.IsNullOrWhiteSpace(obj.Nombre))
            {
                Mensaje = "El nombre del usuario no puede estar vacío";
            }
            else if (string.IsNullOrEmpty(obj.Apellidos) || string.IsNullOrWhiteSpace(obj.Apellidos))
            {
                Mensaje = "Los apellidos no puede estar vacío";
            }
            else if (string.IsNullOrEmpty(obj.Correo) || string.IsNullOrWhiteSpace(obj.Correo))
            {
                Mensaje = "El correo del usuario no puede estar vacío";
            }
            else if (string.IsNullOrEmpty(obj.Rol) || string.IsNullOrWhiteSpace(obj.Rol))
            {
                Mensaje = "Se debe asignar un rol al usuario";
            }

            if (string.IsNullOrEmpty(Mensaje))
            {
                return cd_Usuarios.Editar(obj, out Mensaje);
            }
            else
            {
                return Mensaje;
            }

        }

        public bool Eliminar(int id, out string Mensaje)
        {
            return cd_Usuarios.Eliminar(id, out Mensaje);
        }

        public bool CambiarContrasena(int idUsuario, string nuevaContrasena, out string Mensaje)
        {
            return cd_Usuarios.CambiarContrasena(idUsuario,nuevaContrasena, out Mensaje);
        }

        public bool ReestablecerContrasena(int idUsuario, string correo, out string Mensaje)
        {
            Mensaje = String.Empty;
            string nuevaContrasena = BussinessRecursos.GenerarContrasena();
            bool resultado = cd_Usuarios.ReestablecerContrasena(idUsuario, BussinessRecursos.ConvertirASha256(nuevaContrasena), out Mensaje);


            if (resultado)
            {
                string asunto = "Contraseña Reestablecida";
                string mensaje_correo = "<h3>Su cuenta fue reestablecida correctamente</h3></br><p>Su contraseña para acceder ahora es: !contrasena!</p>";
                mensaje_correo = mensaje_correo.Replace("!contrasena!", nuevaContrasena);

                bool respuesta = BussinessRecursos.EnviarCorreo(correo, asunto, mensaje_correo);
                if (respuesta)
                {
                    return true;
                }
                else
                {
                    Mensaje = "No se pudo enviar el correo ";
                    return false;
                }
            }
            else
            {
                Mensaje = "No se pudo reestablecer la contraseña ";
                return false;
            }

            

        }
    }
}

