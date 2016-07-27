
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System;

namespace MusicStore.Models
{

    public class Genre
    {
        public int GenreId { get; set; }

        [Required]
        public string Name { get; set; }

        public string Description { get; set; }

        public virtual List<Album> Albums { get; set; }

        public static Genre From(GenreJson genre)
        {
            if (genre != null)
            {
                return new Genre()
                {
                    GenreId = genre.GenreId,
                    Name = genre.Name,
                    Description = genre.Description,
                    Albums = new List<Album>()

                };
            }
            else
            {
                return new Genre()
                {
                    GenreId = 0,
                    Name = "Not Found",
                    Description = "Not Found",
                    Albums = new List<Album>()

                };
            }
        }
        public static List<Genre> From(List<GenreJson> genres)
        {
            List<Genre> results = new List<Genre>();
            if (genres == null)
                return results;

            foreach (GenreJson g in genres)
            {
                results.Add(Genre.From(g));
            }
            return results;
        }
    }
}
