using DAO.DBConnection;
using DAO.Interfaces;
using DTO.General.Base.Output;
using DTO.General.DAO.Output;
using DTO.Logic.Genre.Database;
using DTO.Logic.Genre.Input;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using System.Linq.Expressions;
using System.Text.RegularExpressions;

namespace DAO.Logic.GenreDAO
{
    public class GenreDAO : IGenreDAO
    {
        internal RepositoryMongo<Genre> Repository;
        public GenreDAO(IXDataDatabaseSettings settings) => Repository = new(settings?.MongoDBSettings);
        public DAOActionResultOutput Insert(Genre obj)
        {
            var result = Repository.Insert(obj);
            if (string.IsNullOrEmpty(result?.Id))
                return new("Não foi possível salvar o registro");

            return new(result);
        }

        public DAOActionResultOutput Update(Genre obj)
        {
            var result = Repository.Update(obj);
            if (string.IsNullOrEmpty(result?.Id))
                return new("Não foi possível salvar o registro");

            return new(result);
        }

        public DAOActionResultOutput Upsert(Genre obj) => string.IsNullOrEmpty(obj.Id) ? Insert(obj) : Update(obj);

        public DAOActionResultOutput Remove(Genre obj)
        {
            Repository.RemoveById(obj.Id);
            return new(true);
        }

        public DAOActionResultOutput RemoveById(string id)
        {
            Repository.RemoveById(id);
            return new(true);
        }

        public Genre FindOne() => Repository.FindOne();

        public Genre FindOne(Expression<Func<Genre, bool>> predicate) => Repository.FindOne(predicate);

        public Genre FindById(string id) => Repository.FindById(id);

        public IEnumerable<Genre> Find(Expression<Func<Genre, bool>> predicate) => Repository.Collection.Find(Query<Genre>.Where(predicate));

        public IEnumerable<Genre> FindAll() => Repository.FindAll();

        public Genre FindByName(string name) => Repository.Collection.FindOne(Query<Genre>.Matches(x => x.Name, $"(?i).*{string.Join(".*", Regex.Split(name, @"\s+").Select(Regex.Escape))}.*"));

        public BaseListOutput<Genre> List(GenreListInput input = null) => input == null ?
           new(FindAll().OrderBy(x => x.Id), Repository.Collection.Count()) : new(input.Paginator == null ?
               Repository.Collection.Find(GenerateFilters(input.Filters)).OrderBy(x => x.Id) :
               Repository.Collection.Find(GenerateFilters(input.Filters)).SetSkip((input.Paginator.Page > 0 ? input.Paginator.Page - 1 : 0) * input.Paginator.ResultsPerPage).SetLimit(input.Paginator.ResultsPerPage).OrderBy(x => x.Id), Repository.Collection.Count(GenerateFilters(input.Filters)));

        private static IMongoQuery GenerateFilters(GenreFiltersInput input)
        {
            var emptyResult = Query.And(Query.Empty);
            if (input == null)
                return emptyResult;

            var queryList = new List<IMongoQuery>();
            if (!string.IsNullOrEmpty(input.Name))
                queryList.Add(Query<Genre>.Matches(x => x.Name, $"(?i).*{string.Join(".*", Regex.Split(input.Name, @"\s+").Select(Regex.Escape))}.*"));

            return queryList.Any() ? Query.And(queryList) : emptyResult;
        }

    }
}
