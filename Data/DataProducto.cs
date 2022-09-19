using Entities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;

namespace Data
{
    public class DataProducto
    {
        public List<Producto> Listar()
        {
            List<Producto> productos = new List<Producto>();

            try
            {

                SqlConnection conexion = new SqlConnection(Conexion.cn);
                conexion.Open();

                StringBuilder sb = new StringBuilder();

                sb.AppendLine("select p.IdProducto, p.Nombre, p.DescripcionProducto,");
                sb.AppendLine("m.IdMarca, m.NombreMarca,");
                sb.AppendLine("c.IdCategoria, c.NombreCategoria,");
                sb.AppendLine("p.Precio, p.Stock, p.RutaImagen, p.NombreImagen, p.Activo");
                sb.AppendLine(" from Producto p");
                sb.AppendLine("inner join Marca m on m.IdMarca = p.IdMarca");
                sb.AppendLine("inner join Categoria c on c.IdCategoria = p.IdCategoria");

                Console.WriteLine(sb);
                SqlCommand cmd = new SqlCommand(sb.ToString(), conexion);
                cmd.CommandType = CommandType.Text;


                SqlDataReader dataReader = cmd.ExecuteReader();

                while (dataReader.Read())
                {
                    productos.Add(
                        new Producto()
                        {
                            IdProducto = Convert.ToInt32(dataReader["IdProducto"]),
                            Nombre = dataReader["Nombre"].ToString(),
                            DescripcionProducto = dataReader["DescripcionProducto"].ToString(),
                            objetoMarca = new Marca()
                            {
                                IdMarca = Convert.ToInt32(dataReader["IdMarca"]),
                                NombreMarca = dataReader["NombreMarca"].ToString()
                            },
                            objetoCategoria = new Categoria()
                            {
                                IdCategoria = Convert.ToInt32(dataReader["IdCategoria"]),
                                NombreCategoria = dataReader["NombreCategoria"].ToString()
                            },
                            Precio = Convert.ToDecimal(dataReader["Precio"], new CultureInfo("es-ES")),
                            Stock = Convert.ToInt32(dataReader["Stock"]),
                            RutaImagen = dataReader["RutaImagen"].ToString(),
                            NombreImagen = dataReader["NombreImagen"].ToString(),
                            Activo = Convert.ToBoolean(dataReader["Activo"]),
                        }
                        ); ;
                }


            }
            catch (SqlException ex)
            {
                Console.WriteLine(ex.Message.ToString());
                productos = new List<Producto>();
            }
            return productos;
        }

        public string Registrar(Producto obj, out string Mensaje)
        {

            Mensaje = string.Empty;
            try
            {
                SqlConnection conexion = new SqlConnection(Conexion.cn);


                SqlCommand cmd = new SqlCommand("sp_GuardarProducto", conexion);
                cmd.Parameters.AddWithValue("Nombre", obj.Nombre);
                cmd.Parameters.AddWithValue("DescripcionProducto", obj.DescripcionProducto);
                cmd.Parameters.AddWithValue("IdMarca", obj.objetoMarca.IdMarca);
                cmd.Parameters.AddWithValue("IdCategoria", obj.objetoCategoria.IdCategoria);
                cmd.Parameters.AddWithValue("Precio", obj.Precio);
                cmd.Parameters.AddWithValue("Stock", obj.Stock);
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

        public string Editar(Producto obj, out string Mensaje)
        {

            Mensaje = string.Empty;

            try
            {
                SqlConnection conexion = new SqlConnection(Conexion.cn);


                SqlCommand cmd = new SqlCommand("sp_EditarProducto", conexion);

                cmd.Parameters.AddWithValue("IdProducto", obj.IdProducto);
                cmd.Parameters.AddWithValue("Nombre", obj.Nombre);
                cmd.Parameters.AddWithValue("DescripcionProducto", obj.DescripcionProducto);
                cmd.Parameters.AddWithValue("IdMarca", obj.objetoMarca.IdMarca);
                cmd.Parameters.AddWithValue("IdCategoria", obj.objetoCategoria.IdCategoria);
                cmd.Parameters.AddWithValue("Precio", obj.Precio);
                cmd.Parameters.AddWithValue("Stock", obj.Stock);
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

        public bool GuardarDatosImagen(Producto obj, out string Mensaje) 
        {
            bool resultado = false;
            Mensaje = string.Empty;
            
            try
            {
                SqlConnection conexion = new SqlConnection(Conexion.cn);

                string query = "update Producto set RutaImagen = @RutaImagen, NombreImagen = @NombreImagen where DescripcionProducto = @DescripcionProducto";
                
                SqlCommand cmd = new SqlCommand(query, conexion);
                cmd.Parameters.AddWithValue("RutaImagen", obj.RutaImagen);
                cmd.Parameters.AddWithValue("NombreImagen", obj.NombreImagen);
                cmd.Parameters.AddWithValue("DescripcionProducto", obj.DescripcionProducto);
                cmd.CommandType = CommandType.Text;

                conexion.Open();

                resultado = cmd.ExecuteNonQuery() > 0 ? true : false;
            } 
            catch (Exception ex)
            {
                resultado=false;
                Mensaje = ex.Message;
            }
            return resultado;
        }



        public string Eliminar(int idProducto, out string Mensaje)
        {

            Mensaje = string.Empty;

            try
            {
                SqlConnection conexion = new SqlConnection(Conexion.cn);

                SqlCommand cmd = new SqlCommand("sp_EliminarProducto", conexion);

                cmd.Parameters.AddWithValue("IdProducto", idProducto);
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

