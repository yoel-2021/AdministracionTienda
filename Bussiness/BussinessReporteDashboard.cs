using Data;
using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bussiness
{
    public class BussinessReporteDashboard
    {
        private DataReporteDashboard cd_Reporte = new DataReporteDashboard();

        public Dashboard VerDashboard()
        {
            return cd_Reporte.VerDashboard();
        }

        public List<ReporteVenta> Ventas(string fechainicio, string fechafin, string idtransaccion)
        {
            return cd_Reporte.Ventas(fechainicio, fechafin, idtransaccion);
        }
    }
}
