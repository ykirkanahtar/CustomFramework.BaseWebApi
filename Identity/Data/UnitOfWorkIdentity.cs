using CustomFramework.BaseWebApi.Identity.Data.Repositories;
using CustomFramework.BaseWebApi.Identity.Models;
using CustomFramework.BaseWebApi.Data;

namespace CustomFramework.BaseWebApi.Identity.Data
{
    public class UnitOfWorkIdentity<TUser, TRole> : UnitOfWork<IdentityContext<TUser, TRole>>, IUnitOfWorkIdentity 
        where TUser : CustomUser
        where TRole : CustomRole
    {
        public UnitOfWorkIdentity(IdentityContext<TUser, TRole> context) : base(context)
        {
            ClientApplications = new ClientApplicationRepository(context);
        }

        public IClientApplicationRepository ClientApplications { get; }
    }
}