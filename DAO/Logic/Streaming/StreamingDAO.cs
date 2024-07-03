using DAO.DBConnection;
using DAO.Interfaces;
using DTO.General.Base.Output;
using DTO.General.DAO.Output;
using DTO.Logic.Streaming.Input;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text.RegularExpressions;
using StreamingClass = DTO.Logic.Streaming.Database.Streaming;

namespace DAO.Logic.Streaming
{
    public class StreamingDAO : IStreamingDAO
    {
        internal RepositoryMongo<StreamingClass> Repository;
        public StreamingDAO(IXDataDatabaseSettings settings) => Repository = new(settings?.MongoDBSettings);
        public DAOActionResultOutput Insert(StreamingClass obj)
        {
            var result = Repository.Insert(obj);
            if (string.IsNullOrEmpty(result?.Id))
                return new("Não foi possível salvar o registro");

            return new(result);
        }

        public DAOActionResultOutput Update(StreamingClass obj)
        {
            var result = Repository.Update(obj);
            if (string.IsNullOrEmpty(result?.Id))
                return new("Não foi possível salvar o registro");

            return new(result);
        }

        public DAOActionResultOutput Upsert(StreamingClass obj) => string.IsNullOrEmpty(obj.Id) ? Insert(obj) : Update(obj);

        public DAOActionResultOutput Remove(StreamingClass obj)
        {
            Repository.RemoveById(obj.Id);
            return new(true);
        }

        public DAOActionResultOutput RemoveById(string id)
        {
            Repository.RemoveById(id);
            return new(true);
        }

        public StreamingClass FindOne() => Repository.FindOne();

        public StreamingClass FindOne(Expression<Func<StreamingClass, bool>> predicate) => Repository.FindOne(predicate);

        public StreamingClass FindById(string id) => Repository.FindById(id);

        public IEnumerable<StreamingClass> Find(Expression<Func<StreamingClass, bool>> predicate) => Repository.Collection.Find(Query<StreamingClass>.Where(predicate));

        public IEnumerable<StreamingClass> FindAll() => Repository.FindAll();

        public StreamingClass FindByName(string name) => Repository.Collection.FindOne(Query<StreamingClass>.Matches(x => x.Name, $"(?i).*{string.Join(".*", Regex.Split(name, @"\s+").Select(Regex.Escape))}.*"));

        public BaseListOutput<StreamingClass> List(StreamingListInput input = null) => input == null ?
           new(FindAll().OrderBy(x => x.Id), Repository.Collection.Count()) : new(input.Paginator == null ?
               Repository.Collection.Find(GenerateFilters(input.Filters)).OrderBy(x => x.Id) :
               Repository.Collection.Find(GenerateFilters(input.Filters)).SetSkip((input.Paginator.Page > 0 ? input.Paginator.Page - 1 : 0) * input.Paginator.ResultsPerPage).SetLimit(input.Paginator.ResultsPerPage).OrderBy(x => x.Id), Repository.Collection.Count(GenerateFilters(input.Filters)));

        private static IMongoQuery GenerateFilters(StreamingFiltersInput input)
        {
            var emptyResult = Query.And(Query.Empty);
            if (input == null)
                return emptyResult;

            var queryList = new List<IMongoQuery>();
            if (!string.IsNullOrEmpty(input.Name))
                queryList.Add(Query<StreamingClass>.Matches(x => x.Name, $"(?i).*{string.Join(".*", Regex.Split(input.Name, @"\s+").Select(Regex.Escape))}.*"));

            return queryList.Any() ? Query.And(queryList) : emptyResult;
        }

    }
}
