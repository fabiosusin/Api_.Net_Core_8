using DAO.DBConnection;
using DAO.Interfaces;
using DTO.General.Base.Output;
using DTO.General.DAO.Output;
using DTO.Logic.Movie.Database;
using DTO.Logic.Movie.Input;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text.RegularExpressions;

namespace DAO.Logic.MovieDAO
{
    public class MovieDAO : IMovieDAO
    {
        internal RepositoryMongo<Movie> Repository;
        public MovieDAO(IXDataDatabaseSettings settings) => Repository = new(settings?.MongoDBSettings);
        public DAOActionResultOutput Insert(Movie obj)
        {
            var result = Repository.Insert(obj);
            if (string.IsNullOrEmpty(result?.Id))
                return new("Não foi possível salvar o registro");

            return new(result);
        }

        public DAOActionResultOutput Update(Movie obj)
        {
            var result = Repository.Update(obj);
            if (string.IsNullOrEmpty(result?.Id))
                return new("Não foi possível salvar o registro");

            return new(result);
        }

        public DAOActionResultOutput Upsert(Movie obj) => string.IsNullOrEmpty(obj.Id) ? Insert(obj) : Update(obj);

        public DAOActionResultOutput Remove(Movie obj)
        {
            Repository.RemoveById(obj.Id);
            return new(true);
        }

        public DAOActionResultOutput RemoveById(string id)
        {
            Repository.RemoveById(id);
            return new(true);
        }

        public Movie FindOne() => Repository.FindOne();

        public Movie FindOne(Expression<Func<Movie, bool>> predicate) => Repository.FindOne(predicate);

        public Movie FindById(string id) => Repository.FindById(id);

        public IEnumerable<Movie> Find(Expression<Func<Movie, bool>> predicate) => Repository.Collection.Find(Query<Movie>.Where(predicate));

        public IEnumerable<Movie> FindAll() => Repository.FindAll();

        public BaseListOutput<Movie> List(MovieListInput input = null) => input == null ?
           new(FindAll().OrderBy(x => x.Id), Repository.Collection.Count()) : new(input.Paginator == null ?
               Repository.Collection.Find(GenerateFilters(input.Filters)).OrderBy(x => x.Id) :
               Repository.Collection.Find(GenerateFilters(input.Filters)).SetSkip((input.Paginator.Page > 0 ? input.Paginator.Page - 1 : 0) * input.Paginator.ResultsPerPage).SetLimit(input.Paginator.ResultsPerPage).OrderBy(x => x.Id), Repository.Collection.Count(GenerateFilters(input.Filters)));

        private static IMongoQuery GenerateFilters(MovieFiltersInput input)
        {
            var emptyResult = Query.And(Query.Empty);
            if (input == null)
                return emptyResult;

            var queryList = new List<IMongoQuery>();
            if (!string.IsNullOrEmpty(input.Title))
                queryList.Add(Query<Movie>.Matches(x => x.Title, $"(?i).*{string.Join(".*", Regex.Split(input.Title, @"\s+").Select(Regex.Escape))}.*"));

            if (!string.IsNullOrEmpty(input.GenreId))
                queryList.Add(Query<Movie>.EQ(x => x.GenreId, input.GenreId));

            return queryList.Any() ? Query.And(queryList) : emptyResult;
        }

    }
}
