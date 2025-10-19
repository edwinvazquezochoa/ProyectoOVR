using Ovr.DAO;
using Ovr.Domain.Models;
using Ovr.DaoServices.Interfaces;
using System.Globalization;

namespace Ovr.DaoServices.Implementations
{
    public class DashBoardService : IDashBoardService
    {
        public async Task<int> TotalVentasUltimaSemana()
        {
            return await DashBoardDAO.TotalVentasUltimaSemana();
        }

        public async Task<string> TotalIngresosUltimaSemana()
        {
            return await DashBoardDAO.TotalIngresosUltimaSemana();
        }

        public async Task<int> TotalProductos()
        {
            return await DashBoardDAO.TotalProductos();
        }

        public async Task<Dictionary<string, int>> VentasUltimaSemana()
        {
            return await DashBoardDAO.VentasUltimaSemana();
        }
    }
}
