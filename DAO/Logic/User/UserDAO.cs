using DAO.DBConnection;
using DAO.Interfaces;
using DTO.General.DAO.Output;
using DTO.Logic.User.Database;
using MongoDB.Driver.Builders;
using System.Linq.Expressions;

namespace DAO.Logic.UserDAO
{
    public class UserDAO : IUserDAO
    {
        internal RepositoryMongo<User> Repository;
        public UserDAO(IXDataDatabaseSettings settings) => Repository = new(settings?.MongoDBSettings);
        public DAOActionResultOutput Insert(User obj)
        {
            var result = Repository.Insert(obj);
            if (string.IsNullOrEmpty(result?.Id))
                return new("Não foi possível salvar o registro");

            return new(result);
        }

        public DAOActionResultOutput Update(User obj)
        {
            var result = Repository.Update(obj);
            if (string.IsNullOrEmpty(result?.Id))
                return new("Não foi possível salvar o registro");

            return new(result);
        }

        public DAOActionResultOutput Upsert(User obj) => string.IsNullOrEmpty(obj.Id) ? Insert(obj) : Update(obj);

        public DAOActionResultOutput Remove(User obj)
        {
            Repository.RemoveById(obj.Id);
            return new(true);
        }

        public DAOActionResultOutput RemoveById(string id)
        {
            Repository.RemoveById(id);
            return new(true);
        }

        public User FindOne() => Repository.FindOne();

        public User FindOne(Expression<Func<User, bool>> predicate) => Repository.FindOne(predicate);

        public User FindById(string id) => Repository.FindById(id);

        public IEnumerable<User> Find(Expression<Func<User, bool>> predicate) => Repository.Collection.Find(Query<User>.Where(predicate));

        public IEnumerable<User> FindAll() => Repository.FindAll();

    }
}
