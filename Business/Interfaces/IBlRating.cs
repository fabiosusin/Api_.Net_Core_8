using DTO.General.Base.Output;
using DTO.Logic.Rating.Database;
using DTO.Logic.Rating.Input;

namespace Business.Interfaces
{
    public interface IBlRating
    {
        public Rating GetRating(string id);
        public BaseListOutput<Rating> List(RatingListInput input);
        public BaseApiOutput CreateRating(RatingInput Rating);
        public BaseApiOutput UpdateRating(string id, RatingInput Rating);
        public BaseApiOutput DeleteRating(string id);
    }
}
