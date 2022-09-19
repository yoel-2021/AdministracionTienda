using AdminPlatform.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Bussiness;
using Entities;
using System.Data;
using ClosedXML.Excel;
using Microsoft.AspNetCore.Authorization;

namespace AdminPlatform.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
       

        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public JsonResult ListaReporte(string fechainicio, string fechafin, string idtransaccion)
        {
            List<ReporteVenta> reportes = new List<ReporteVenta>();

            reportes = new BussinessReporteDashboard().Ventas(fechainicio,fechafin,idtransaccion);
            return Json(new { resultado = reportes });
        }

        [HttpGet]
        public JsonResult VistaDashboard()
        {
            Dashboard objeto = new BussinessReporteDashboard().VerDashboard();
            return Json(new { resultado= objeto });
        }

        [HttpPost]
        public FileResult ExportarVenta(string fechainicio, string fechafin,string idtransaccion)
        {
            List<ReporteVenta> oLista = new List<ReporteVenta>();
            oLista = new BussinessReporteDashboard().Ventas(fechainicio, fechafin, idtransaccion);

            DataTable dt = new DataTable();

            dt.Locale = new System.Globalization.CultureInfo("es-ES");
            dt.Columns.Add("Fecha Venta", typeof(string));
            dt.Columns.Add("Cliente", typeof(string));
            dt.Columns.Add("Producto", typeof(string));
            dt.Columns.Add("Precio", typeof(decimal));
            dt.Columns.Add("Cantidad", typeof(int));
            dt.Columns.Add("Total", typeof(decimal));
            dt.Columns.Add("IdTransaccion", typeof(string));

            foreach (ReporteVenta rp in oLista)
            {
                dt.Rows.Add(new object[]
                {
                    rp.FechaVenta,
                    rp.Cliente,
                    rp.Producto,
                    rp.Precio,
                    rp.Cantidad,
                    rp.Total,
                    rp.IdTransaccion
                });
            }

            dt.TableName = "Datos";
            XLWorkbook wb = new XLWorkbook();
            wb.Worksheets.Add(dt);
            MemoryStream stream = new MemoryStream();
            wb.SaveAs(stream);
            return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "ReporteVentas" + DateTime.Now.ToString() + ".xlsx");
         }
    }
}