using DTO.General.Base.Database;
using DTO.Logic.Rating.Input;

namespace DTO.Logic.Rating.Database
{
    public class Rating : BaseData
    {
        public Rating() { }
        public Rating(RatingInput rating)
        {
            if (rating == null)
                return;

            UserId = rating.UserId;
            MovieId = rating.MovieId;
            Comment = rating.Comment;
            RatingValue = rating.RatingValue;
        }

        public string UserId { get; set; }
        public string MovieId { get; set; }
        public string Comment { get; set; }
        public int RatingValue { get; set; }
    }
}
