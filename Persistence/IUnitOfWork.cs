using System.Threading.Tasks;

namespace MyDotnetProject.Persistence
{
    public interface IUnitOfWork
    {
        Task Complete();
    }
}