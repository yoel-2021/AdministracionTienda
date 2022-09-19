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
    public class DataReporteDashboard
    {
        public List<ReporteVenta> Ventas(string fechainicio, string fechafin, string idtransaccion)
        {
            List<ReporteVenta> reporteVentas = new List<ReporteVenta>();

            try
            {

                SqlConnection conexion = new SqlConnection(Conexion.cn);
                conexion.Open();
                SqlCommand cmd = new SqlCommand("sp_ReporteVentas", conexion);
                cmd.Parameters.AddWithValue("fechainicio", fechainicio);
                cmd.Parameters.AddWithValue("fechafin", fechafin);
                cmd.Parameters.AddWithValue("idtransaccion", idtransaccion);

                cmd.CommandType = CommandType.StoredProcedure;



                SqlDataReader dataReader = cmd.ExecuteReader();

                while (dataReader.Read())
                {
                    reporteVentas.Add(
                        new ReporteVenta()
                        {
                            FechaVenta = dataReader["FechaVenta"].ToString(),
                            Cliente = dataReader["Cliente"].ToString(),
                            Producto = dataReader["Producto"].ToString(),
                            Precio = Convert.ToDecimal(dataReader["Precio"], new CultureInfo("es-ES")),
                            Cantidad = Convert.ToInt32(dataReader["Cantidad"]),
                            Total = Convert.ToDecimal(dataReader["Total"], new CultureInfo("es-ES")),
                            IdTransaccion = dataReader["IdTransaccion"].ToString()

                        }
                        );
                }


            }
            catch (SqlException ex)
            {
                Console.WriteLine(ex.Message.ToString());
                reporteVentas = new List<ReporteVenta>();
            }
            return reporteVentas;
        }


        public Dashboard VerDashboard()
        {
            Dashboard objeto = new Dashboard();

            try
            {

                SqlConnection conexion = new SqlConnection(Conexion.cn);
                conexion.Open();
                
                SqlCommand cmd = new SqlCommand("sp_ReporteDashboard", conexion);
                cmd.CommandType = CommandType.StoredProcedure;


                SqlDataReader dataReader = cmd.ExecuteReader();

                while (dataReader.Read())
                {
                    objeto = new Dashboard()
                    {
                        TotalCliente = Convert.ToInt32(dataReader["TotalCliente"]),
                        TotalVenta = Convert.ToInt32(dataReader["TotalVenta"]),
                        TotalProducto = Convert.ToInt32(dataReader["TotalProducto"]),
                    };

                }


            }
            catch (SqlException ex)
            {
                Console.WriteLine(ex.Message.ToString());
                objeto = new Dashboard();
            }
            return objeto;
        }
    }
}
