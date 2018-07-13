using System.Threading.Tasks;

namespace MyDotnetProject.Core
{
    public interface IUnitOfWork
    {
        Task Complete();
    }
}