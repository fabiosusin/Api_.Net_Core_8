using DTO.General.Base.Database;

namespace DTO.Logic.Genre.Database
{
    public class Genre : BaseData
    {
        public Genre() { }
        public Genre(string name) => Name = name;
        public string Name { get; set; }
    }
}
