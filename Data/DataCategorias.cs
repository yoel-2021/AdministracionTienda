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
    public class DataCategorias
    {
        public List<Categoria> Listar()
        {
            List<Categoria> categorias = new List<Categoria>();

            try
            {

                SqlConnection conexion = new SqlConnection(Conexion.cn);
                conexion.Open();
                string query = "select IdCategoria,NombreCategoria,Activo from Categoria";
                SqlCommand cmd = new SqlCommand(query, conexion);
                cmd.CommandType = CommandType.Text;


                SqlDataReader dataReader = cmd.ExecuteReader();

                while (dataReader.Read())
                {
                    categorias.Add(
                        new Categoria()
                        {
                            IdCategoria = Convert.ToInt32(dataReader["IdCategoria"]),
                            NombreCategoria = dataReader["NombreCategoria"].ToString(),
                            Activo = Convert.ToBoolean(dataReader["Activo"])
                        }
                        );
                }


            }
            catch (SqlException ex)
            {
                Console.WriteLine(ex.Message.ToString());
                categorias = new List<Categoria>();
            }
            return categorias;
        }

        public string Registrar(Categoria obj, out string Mensaje)
        {

            Mensaje = string.Empty;
            try
            {
                SqlConnection conexion = new SqlConnection(Conexion.cn);


                SqlCommand cmd = new SqlCommand("sp_GuardarCategoria", conexion);
                cmd.Parameters.AddWithValue("NombreCategoria", obj.NombreCategoria);
                cmd.Parameters.AddWithValue("Activo", obj.Activo);
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

        public string Editar(Categoria obj, out string Mensaje)
        {

            Mensaje = string.Empty;

            try
            {
                SqlConnection conexion = new SqlConnection(Conexion.cn);


                SqlCommand cmd = new SqlCommand("sp_EditarCategoria", conexion);

                cmd.Parameters.AddWithValue("IdCategoria", obj.IdCategoria);
                cmd.Parameters.AddWithValue("NombreCategoria", obj.NombreCategoria);
                cmd.Parameters.AddWithValue("Activo", obj.Activo);
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

        public string Eliminar(int idCategoria, out string Mensaje)
        {
           
            Mensaje = string.Empty;

            try
            {
                SqlConnection conexion = new SqlConnection(Conexion.cn);

                SqlCommand cmd = new SqlCommand("sp_EliminarCategoria", conexion);

                cmd.Parameters.AddWithValue("IdCategoria", idCategoria);
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
    }
}

