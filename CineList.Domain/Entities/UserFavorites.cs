
namespace CineList.Domain.Entities
{

    public sealed class UserFavorite
    {

        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid MovieId { get; set; }
        public DateTime CreatedAt { get; set; }

    }
}
