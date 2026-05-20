using System.Data;

namespace CineList.Domain.Interfaces
{

    public interface IUnitOfWork : IAsyncDisposable
    {

        IUserRepository Users { get; }
        IMovieRepository Movies { get; }
        IUserFavoritesRepository UserFavorites { get; }
        IDbTransaction? Transaction { get; }



        void BeginTransaction();
        void Commit();
        void Rollback();
    }
}
