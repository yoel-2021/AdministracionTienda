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
    public class DataMarcas
    {
        public List<Marca> Listar()
        {
            List<Marca> marcas = new List<Marca>();

            try
            {

                SqlConnection conexion = new SqlConnection(Conexion.cn);
                conexion.Open();
                string query = "select IdMarca,NombreMarca,Activo from Marca";
                SqlCommand cmd = new SqlCommand(query, conexion);
                cmd.CommandType = CommandType.Text;


                SqlDataReader dataReader = cmd.ExecuteReader();

                while (dataReader.Read())
                {
                    marcas.Add(
                        new Marca()
                        {
                            IdMarca = Convert.ToInt32(dataReader["IdMarca"]),
                            NombreMarca = dataReader["NombreMarca"].ToString(),
                            Activo = Convert.ToBoolean(dataReader["Activo"])
                        }
                        );
                }


            }
            catch (SqlException ex)
            {
                Console.WriteLine(ex.Message.ToString());
                marcas = new List<Marca>();
            }
            return marcas;
        }

        public string Registrar(Marca obj, out string Mensaje)
        {

            Mensaje = string.Empty;
            try
            {
                SqlConnection conexion = new SqlConnection(Conexion.cn);


                SqlCommand cmd = new SqlCommand("sp_GuardarMarca", conexion);
                cmd.Parameters.AddWithValue("NombreMarca", obj.NombreMarca);
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

        public string Editar(Marca obj, out string Mensaje)
        {

            Mensaje = string.Empty;

            try
            {
                SqlConnection conexion = new SqlConnection(Conexion.cn);


                SqlCommand cmd = new SqlCommand("sp_EditarMarca", conexion);

                cmd.Parameters.AddWithValue("IdMarca", obj.IdMarca);
                cmd.Parameters.AddWithValue("NombreMarca", obj.NombreMarca);
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

        public string Eliminar(int idMarca, out string Mensaje)
        {

            Mensaje = string.Empty;

            try
            {
                SqlConnection conexion = new SqlConnection(Conexion.cn);

                SqlCommand cmd = new SqlCommand("sp_EliminarMarca", conexion);

                cmd.Parameters.AddWithValue("IdMarca", idMarca);
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

