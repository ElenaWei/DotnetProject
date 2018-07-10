using System.Threading.Tasks;

namespace MyDotnetProject.Persistence
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly MyDbContext context;
        public UnitOfWork(MyDbContext context)
        {
            this.context = context;

        }

        public async Task Complete()
        {
            await context.SaveChangesAsync();
        }
    }
}