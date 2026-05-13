
namespace CineList.Application.Dtos
{

    public record MovieDto(
      int TmdbId,
      string Title,
      string Overview,
      string PosterPath,
      decimal Popularity
    );
}
