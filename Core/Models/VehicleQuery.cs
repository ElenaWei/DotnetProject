using MyDotnetProject.Extensions;

namespace MyDotnetProject.Core.Models
{
    public class VehicleQuery : IQueryObject
    {
       public int? MakeId { get; set; } 
       public int? ModelId { get; set; }
       public string SortBy { get; set; }
       public bool IsSortAsceding { get; set; }
    }
}