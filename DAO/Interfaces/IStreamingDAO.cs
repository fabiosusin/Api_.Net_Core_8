using DAO.Base;
using DTO.General.Base.Output;
using DTO.Logic.Streaming.Database;
using DTO.Logic.Streaming.Input;

namespace DAO.Interfaces
{
    public interface IStreamingDAO : IBaseDAO<Streaming>
    {
        public Streaming FindByName(string name);
        public BaseListOutput<Streaming> List(StreamingListInput input);
    }
}
