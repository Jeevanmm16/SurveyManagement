using SurveyManagement.Infrastructure.Data;

namespace SurveyManagement.Infrastructure.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly SurveyDbContext _context;
        public IResponseRepository Responses { get; }

        public UnitOfWork(SurveyDbContext context)
        {
            _context = context;
            Responses = new ResponseRepository(context);
        }

        public async Task<int> CompleteAsync() => await _context.SaveChangesAsync();
    }
}




