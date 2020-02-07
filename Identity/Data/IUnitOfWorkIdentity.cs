using CustomFramework.BaseWebApi.Identity.Data.Repositories;
using CustomFramework.BaseWebApi.Data;

namespace CustomFramework.BaseWebApi.Identity.Data
{
    public interface IUnitOfWorkIdentity : IUnitOfWork
    {
        IClientApplicationRepository ClientApplications { get; }
    }
}