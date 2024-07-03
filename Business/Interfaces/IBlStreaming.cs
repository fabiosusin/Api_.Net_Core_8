using DTO.General.Base.Output;
using DTO.Logic.Streaming.Database;
using DTO.Logic.Streaming.Input;

namespace Business.Interfaces
{
    public interface IBlStreaming
    {
        public Streaming GetStreaming(string id);
        public BaseListOutput<Streaming> List(StreamingListInput input);
        public BaseApiOutput CreateStreaming(string name);
        public BaseApiOutput UpdateStreaming(string id, string name);
        public BaseApiOutput DeleteStreaming(string id);
    }
}
