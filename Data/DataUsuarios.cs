using Entities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data
{
    public class DataUsuarios
    {
        public List<Usuario> Listar()
        {
            List<Usuario> usuarios = new List<Usuario>();

            try
            {

                SqlConnection conexion = new SqlConnection(Conexion.cn);
                conexion.Open();
                string query = "select IdUsuario,Nombre,Apellidos,Correo,Contrasena,Reestablacer,Rol,Activo from Usuario";
                SqlCommand cmd = new SqlCommand(query, conexion);
                cmd.CommandType = CommandType.Text;


                SqlDataReader dataReader = cmd.ExecuteReader();

                while (dataReader.Read())
                {
                    usuarios.Add(
                        new Usuario()
                        {
                            IdUsuario = Convert.ToInt32(dataReader["IdUsuario"]),
                            Nombre = dataReader["Nombre"].ToString(),
                            Apellidos = dataReader["Apellidos"].ToString(),
                            Correo = dataReader["Correo"].ToString(),
                            Contrasena = dataReader["Contrasena"].ToString(),
                            Rol = dataReader["Rol"].ToString(),
                            Reestablecer = Convert.ToBoolean(dataReader["Reestablacer"]),
                            Activo = Convert.ToBoolean(dataReader["Activo"]),
                            
                        }
                        );
                }


            }
            catch (SqlException ex)
            {
                Console.WriteLine(ex.Message.ToString());
                usuarios = new List<Usuario>();
            }
            return usuarios;
        }

        public string Registrar(Usuario obj, out string Mensaje)
        {
            
            Mensaje = string.Empty;
            try
            {
                SqlConnection conexion = new SqlConnection(Conexion.cn);

               
                SqlCommand cmd = new SqlCommand("sp_GuardarUsuario", conexion);
                cmd.Parameters.AddWithValue("Nombre", obj.Nombre);
                cmd.Parameters.AddWithValue("Apellidos", obj.Apellidos);
                cmd.Parameters.AddWithValue("Correo", obj.Correo);
                cmd.Parameters.AddWithValue("Activo", obj.Activo);
                cmd.Parameters.AddWithValue("Contrasena", obj.Contrasena);
                cmd.Parameters.AddWithValue("Rol", obj.Rol);
                cmd.Parameters.Add("Mensaje", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;
                cmd.CommandType = CommandType.StoredProcedure;
                
                conexion.Open();

                cmd.ExecuteNonQuery();
                Mensaje = cmd.Parameters["Mensaje"].Value.ToString();
            }
            catch (Exception ex)
            {
                
                Mensaje = ex.Message;
            }
            return Mensaje;
        }

        public string Editar(Usuario obj, out string Mensaje)
        {
            
            Mensaje = string.Empty;

            try
            {
                SqlConnection conexion = new SqlConnection(Conexion.cn);


                SqlCommand cmd = new SqlCommand("sp_EditarUsuario", conexion);

                cmd.Parameters.AddWithValue("IdUsuario", obj.IdUsuario);
                cmd.Parameters.AddWithValue("Nombre", obj.Nombre);
                cmd.Parameters.AddWithValue("Apellidos", obj.Apellidos);
                cmd.Parameters.AddWithValue("Correo", obj.Correo);
                cmd.Parameters.AddWithValue("Rol", obj.Rol);
                cmd.Parameters.AddWithValue("Activo", obj.Activo);
                
                cmd.CommandType = CommandType.StoredProcedure;

                conexion.Open();

                cmd.ExecuteNonQuery();
                
               
            }
            catch (Exception ex)
            {
                
                Mensaje = ex.Message;
            }
            return Mensaje;
        }

        public bool Eliminar(int id, out string Mensaje)
        {
            bool Resultado = false;
            Mensaje = string.Empty;

            try
            {
                SqlConnection conexion = new SqlConnection(Conexion.cn);
                conexion.Open();

                SqlCommand cmd = new SqlCommand("delete top (1) from USUARIO where IdUsuario = @id", conexion);
                cmd.Parameters.AddWithValue("@id", id);
                cmd.CommandType = CommandType.Text;


                Resultado = cmd.ExecuteNonQuery() > 0 ? true : false;
            }
            catch (Exception ex)
            {
                Resultado = false;
                Mensaje = ex.Message;
            }
            return Resultado;
        }

        public bool CambiarContrasena(int idUsuario, string nuevaContrasena, out string Mensaje)
        {
            bool Resultado = false;
            Mensaje = string.Empty;

            try
            {
                SqlConnection conexion = new SqlConnection(Conexion.cn);
                conexion.Open();

                SqlCommand cmd = new SqlCommand("update Usuario set Contrasena = @nuevaContrasena, Reestablacer = 0 where idUsuario = @id", conexion);
                cmd.Parameters.AddWithValue("@id", idUsuario);
                cmd.Parameters.AddWithValue("@nuevaContrasena", nuevaContrasena);
                cmd.CommandType = CommandType.Text;


                Resultado = cmd.ExecuteNonQuery() > 0 ? true : false;
            }
            catch (Exception ex)
            {
                Resultado = false;
                Mensaje = ex.Message;
            }
            return Resultado;
        }

        public bool ReestablecerContrasena(int idUsuario, string contrasena, out string Mensaje)
        {
            bool Resultado = false;
            Mensaje = string.Empty;

            try
            {
                SqlConnection conexion = new SqlConnection(Conexion.cn);
                conexion.Open();

                SqlCommand cmd = new SqlCommand("update Usuario set Contrasena = @contrasena, Reestablacer = 1 where idUsuario = @id", conexion);
                cmd.Parameters.AddWithValue("@id", idUsuario);
                cmd.Parameters.AddWithValue("@contrasena", contrasena);
                cmd.CommandType = CommandType.Text;


                Resultado = cmd.ExecuteNonQuery() > 0 ? true : false;
            }
            catch (Exception ex)
            {
                Resultado = false;
                Mensaje = ex.Message;
            }
            return Resultado;
        }
    }
}