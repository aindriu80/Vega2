using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Vega.Core;
using Vega.Core.Models;

namespace Vega.Persistence
{
    public class PhotoRepository : IPhotoRepository
    {
        private readonly VegaDbContext _context;
        public PhotoRepository(VegaDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Photo>> GetPhotos(int vehicleId)
        {

            return await _context.Photos
                .Where(p => p.VehicleId == vehicleId)
                .ToListAsync();
        }
    }
}
