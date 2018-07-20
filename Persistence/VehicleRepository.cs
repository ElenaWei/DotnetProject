using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MyDotnetProject.Models;
using MyDotnetProject.Core;
using System.Collections.Generic;
using MyDotnetProject.Core.Models;
using System.Linq;
using System.Linq.Expressions;
using System;
using MyDotnetProject.Extensions;
using MyDotnetProject.Controllers.Resources;

namespace MyDotnetProject.Persistence
{
    public class VehicleRepository : IVehicleRepository
    {
        private readonly MyDbContext context;
        public VehicleRepository(MyDbContext context)
        {
            this.context = context;

        }
        public async Task<Vehicle> GetVehicle(int id, bool includeRelated = true)
        {
            if (!includeRelated)
            {
                return await context.Vehicles.FindAsync(id);
            }
            return await context.Vehicles
            .Include(v => v.Features).ThenInclude(vf => vf.Feature)
             .Include(v => v.Model).ThenInclude(m => m.Make)
              .SingleOrDefaultAsync(v => v.Id == id);
        }

        public void Add(Vehicle vehicle)
        {
            context.Vehicles.Add(vehicle);
        }

        public void Remove(Vehicle vehicle)
        {
            context.Remove(vehicle);
        }

        public async Task<QueryResult<Vehicle>> GetVehicles(VehicleQuery queryObj)
        {
            var result = new QueryResult<Vehicle>();
            // get all the vehicles in the database
            var query = context.Vehicles
            .Include(v => v.Model).ThenInclude(m => m.Make).AsQueryable();

            // filter by the given makeId
           query = query.ApplyFiltering(queryObj);

            // sort by the given info
            /*
            if (queryObj.SortBy == "make")
                query = (queryObj.IsSortAsceding) ? query.OrderBy(v => v.Model.Make.Name) :
                query.OrderByDescending(v => v.Model.Make.Name);
            if (queryObj.SortBy == "model")
                query = (queryObj.IsSortAsceding) ? query.OrderBy(v => v.Model.Name) :
                query.OrderByDescending(v => v.Model.Name);
            if (queryObj.SortBy == "contactName")
                query = (queryObj.IsSortAsceding) ? query.OrderBy(v => v.ContactName) :
                query.OrderByDescending(v => v.ContactName);
            if (queryObj.SortBy == "id")
                query = (queryObj.IsSortAsceding) ? query.OrderBy(v => v.Id) :
                query.OrderByDescending(v => v.Id);
            */

            // Optimize duplicate simailar code above using Expression<Func<Vehicle, object>> exp          
            var columnsMap = new Dictionary<string, Expression<Func<Vehicle, object>>>()
            {
                ["make"] = v => v.Model.Make.Name,
                ["model"] = v => v.Model.Name,
                ["contactName"] = v => v.ContactName,
                //["id"] = v => v.Id,
            };
            query = query.ApplyOrdering(queryObj, columnsMap);

            //implement pagination
            result.TotalItems = await query.CountAsync(); // count total items get from database
        
            query = query.ApplyPaging(queryObj); // apply pagination

            result.Items = await query.ToListAsync();
            
            return result;
        }

    }
}