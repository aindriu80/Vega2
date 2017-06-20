using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Vega.Core;
using Vega.Core.Models;
using Vega.Extentions;

namespace Vega.Persistence
{
    public class VehicleRepository : IVehicleRepository
    {
        private readonly VegaDbContext _context;
        public VehicleRepository(VegaDbContext context)
        {
            _context = context;
        }
        public async Task<Vehicle> GetVehicle(int id, bool includeRelated = true)
        {
            if (!includeRelated)
                return await _context.Vehicles.FindAsync(id);

            return await _context.Vehicles
                .Include(v => v.Features)
                .ThenInclude(vf => vf.Feature)
                .Include(v => v.Model)
                .ThenInclude(m => m.Make)
                .SingleOrDefaultAsync(v => v.Id == id);
        }

        public void Add(Vehicle vehicle)
        {
            _context.Vehicles.Add(vehicle);
        }
        public void Remove(Vehicle vehicle)
        {
            _context.Remove(vehicle);
        }

        public async Task<QueryResult<Vehicle>> GetVehicles(VehicleQuery vehicleQuery)
        {
            var result = new QueryResult<Vehicle>();

            var query = _context.Vehicles
                .Include(v => v.Model)
                .ThenInclude(m => m.Make)
                .Include(v => v.Features)
                .ThenInclude(vf => vf.Feature)
                .AsQueryable();

            if (vehicleQuery.MakeId.HasValue)
                query = query.Where(v => v.Model.MakeId == vehicleQuery.MakeId.Value);

            if (vehicleQuery.ModelId.HasValue)
                query = query.Where(v => v.ModelId == vehicleQuery.ModelId.Value);

            var columnsMap = new Dictionary<string, Expression<Func<Vehicle, object>>>()
            {
                ["make"] = v => v.Model.Make.Name,
                ["model"] = v => v.Model.Name,
                ["contactName"] = v => v.ContactName

            };


            query = query.ApplyOrdering(vehicleQuery, columnsMap);

            result.TotalItems = await query.CountAsync();

            query = query.ApplyPaging(vehicleQuery);

            result.Items = await query.ToListAsync();

            return result;
        }



    }
}
