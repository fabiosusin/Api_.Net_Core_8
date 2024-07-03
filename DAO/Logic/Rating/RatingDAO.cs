using DAO.DBConnection;
using DAO.Interfaces;
using DTO.General.Base.Output;
using DTO.General.DAO.Output;
using DTO.Logic.Rating.Database;
using DTO.Logic.Rating.Input;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text.RegularExpressions;

namespace DAO.Logic.RatingDAO
{
    public class RatingDAO : IRatingDAO
    {
        internal RepositoryMongo<Rating> Repository;
        public RatingDAO(IXDataDatabaseSettings settings) => Repository = new(settings?.MongoDBSettings);
        public DAOActionResultOutput Insert(Rating obj)
        {
            var result = Repository.Insert(obj);
            if (string.IsNullOrEmpty(result?.Id))
                return new("Não foi possível salvar o registro");

            return new(result);
        }

        public DAOActionResultOutput Update(Rating obj)
        {
            var result = Repository.Update(obj);
            if (string.IsNullOrEmpty(result?.Id))
                return new("Não foi possível salvar o registro");

            return new(result);
        }

        public DAOActionResultOutput Upsert(Rating obj) => string.IsNullOrEmpty(obj.Id) ? Insert(obj) : Update(obj);

        public DAOActionResultOutput Remove(Rating obj)
        {
            Repository.RemoveById(obj.Id);
            return new(true);
        }

        public DAOActionResultOutput RemoveById(string id)
        {
            Repository.RemoveById(id);
            return new(true);
        }

        public Rating FindOne() => Repository.FindOne();

        public Rating FindOne(Expression<Func<Rating, bool>> predicate) => Repository.FindOne(predicate);

        public Rating FindById(string id) => Repository.FindById(id);

        public IEnumerable<Rating> Find(Expression<Func<Rating, bool>> predicate) => Repository.Collection.Find(Query<Rating>.Where(predicate));

        public IEnumerable<Rating> FindAll() => Repository.FindAll();

        public BaseListOutput<Rating> List(RatingListInput input = null) => input == null ?
           new(FindAll().OrderBy(x => x.Id), Repository.Collection.Count()) : new(input.Paginator == null ?
               Repository.Collection.Find(GenerateFilters(input.Filters)).OrderBy(x => x.Id) :
               Repository.Collection.Find(GenerateFilters(input.Filters)).SetSkip((input.Paginator.Page > 0 ? input.Paginator.Page - 1 : 0) * input.Paginator.ResultsPerPage).SetLimit(input.Paginator.ResultsPerPage).OrderBy(x => x.Id), Repository.Collection.Count(GenerateFilters(input.Filters)));

        private static IMongoQuery GenerateFilters(RatingFiltersInput input)
        {
            var emptyResult = Query.And(Query.Empty);
            if (input == null)
                return emptyResult;

            var queryList = new List<IMongoQuery>();
            if (!string.IsNullOrEmpty(input.MovieId))
                queryList.Add(Query<Rating>.EQ(x => x.MovieId, input.MovieId));

            return queryList.Any() ? Query.And(queryList) : emptyResult;
        }

    }
}
