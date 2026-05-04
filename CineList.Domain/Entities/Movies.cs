

using System;

namespace CineList.Domain.Entities
{

    public class Movie
    {

        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public int TmdbId { get; set; }
        public string Overview { get; set; } = string.Empty;
        public string PosterPath { get; set; } = string.Empty;
        public decimal Popularity { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
