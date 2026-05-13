using System.Text.Json.Serialization;

namespace CineList.Application.Dtos
{

    public record TmdbResponseDto(
        int Id,
        string Title,
        string Overview,
        [property: JsonPropertyName("poster_path")] string PosterPath,
        decimal Popularity
    );
}
