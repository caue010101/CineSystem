using CineList.Domain.Interfaces;
using System.Data;
using CineList.Infrastructure.Repositories;

namespace CineList.Infrastructure.UnitOfWork
{

    public class UnitOfWork : IUnitOfWork
    {

        private readonly IDbConnection _connection;
        private IDbTransaction? _transaction;
        public IUserRepository Users { get; }

        public IDbTransaction? Transaction => _transaction;

        public UnitOfWork(IDbConnection connection)
        {
            this._connection = connection;
            Users = new UserRepository(_connection, this);
        }

        public void BeginTransaction()
        {

            if (_connection.State != ConnectionState.Open)
            {

                _connection.Open();
            }

            _transaction = _connection.BeginTransaction();
        }

        public void Commit()
        {
            try
            {
                _transaction?.Commit();
            }
            catch
            {
                _transaction?.Rollback();
                _transaction?.Dispose();
                _transaction = null;
            }
        }

        public void Rollback()
        {

            try
            {

                _transaction?.Rollback();
                _transaction?.Dispose();
                _transaction = null;

            }
            catch
            {
                _transaction?.Dispose();
                _transaction = null;
            }
        }

        public async ValueTask DisposeAsync()
        {
            _transaction?.Dispose();

            if (_connection.State != ConnectionState.Closed)
            {
                _connection.Close();
            }

            _connection.Dispose();
            await ValueTask.CompletedTask;
        }
    }
}
