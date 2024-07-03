using Business.Interfaces;
using DAO.Interfaces;
using DTO.General.Base.Output;
using DTO.Logic.Streaming.Database;
using DTO.Logic.Streaming.Input;
using System.Linq;

namespace Business.Logic.StreamingData
{
    public class BlStreaming : IBlStreaming
    {
        private readonly IStreamingDAO _streamingDAO;
        private readonly IMovieDAO _movieDAO;
        public BlStreaming(
            IMovieDAO movieDAO,
            IStreamingDAO streamingDAO)
        {
            _streamingDAO = streamingDAO;
            _movieDAO = movieDAO;
        }

        public BaseApiOutput CreateStreaming(string name)
        {
            var existingName = _streamingDAO.FindByName(name);
            if (existingName != null)
                return new("Já existe um Streaming com este nome!");

            return _streamingDAO.Insert(new(name));
        }

        public BaseApiOutput DeleteStreaming(string id)
        {
            var streaming = _streamingDAO.FindById(id);
            if (streaming == null)
                return new("Streaming não encontrado!");

            var movie = _movieDAO.FindOne(x => x.StreamingsId.Contains(id));
            if (movie != null)
                return new("Existem filmes vinculados a este Streaming!");

            return _streamingDAO.RemoveById(id);
        }

        public Streaming GetStreaming(string id) => _streamingDAO.FindById(id);

        public BaseListOutput<Streaming> List(StreamingListInput input)
        {
            var result = _streamingDAO.List(input);
            if (!(result?.Items?.Any() ?? false))
                return new("Nenhum Streaming encontrado!");

            return result;
        }

        public BaseApiOutput UpdateStreaming(string id, string name)
        {
            var streaming = _streamingDAO.FindById(id);
            if (streaming == null)
                return new("Streaming não encontrado!");

            streaming.Name = name;
            return _streamingDAO.Update(streaming);
        }
    }
}
