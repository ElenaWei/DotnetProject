namespace MyDotnetProject.Extensions
{
    public interface IQueryObject
    {
        string SortBy { get; set; }
        bool IsSortAsceding { get; set; }
    }
}