using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

//https://github.com/Arch/UnitOfWork/blob/master/src/Microsoft.EntityFrameworkCore.UnitOfWork/IUnitOfWork.cs
namespace CustomFramework.BaseWebApi.Data
{
    public interface IUnitOfWork<out TContext> : IUnitOfWork where TContext : DbContext
    {
        TContext DbContext { get; }

        Task<int> SaveChangesAsync(params IUnitOfWork[] unitOfWorks);
    }
}
