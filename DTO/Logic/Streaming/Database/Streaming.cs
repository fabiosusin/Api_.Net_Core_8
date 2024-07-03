using DTO.General.Base.Database;

namespace DTO.Logic.Streaming.Database
{
    public class Streaming : BaseData
    {
        public Streaming() { }
        public Streaming(string name) => Name = name;
        public string Name { get; set; }
    }
}
