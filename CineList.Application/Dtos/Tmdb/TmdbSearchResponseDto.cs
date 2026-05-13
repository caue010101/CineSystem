
namespace CineList.Application.Dtos
{

    public record TmdbSearchResponseDto(
        IEnumerable<TmdbResponseDto> Results
    );
}
