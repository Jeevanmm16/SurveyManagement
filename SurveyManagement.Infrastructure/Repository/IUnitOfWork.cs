namespace SurveyManagement.Infrastructure.Repository
{
    public interface IUnitOfWork
    {
        IResponseRepository Responses { get; }
        Task<int> CompleteAsync();
    }
}




