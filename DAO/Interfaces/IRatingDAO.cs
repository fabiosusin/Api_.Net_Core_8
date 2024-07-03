using DAO.Base;
using DTO.General.Base.Output;
using DTO.Logic.Rating.Database;
using DTO.Logic.Rating.Input;

namespace DAO.Interfaces
{
    public interface IRatingDAO : IBaseDAO<Rating>
    {
        public BaseListOutput<Rating> List(RatingListInput input);
    }
}
