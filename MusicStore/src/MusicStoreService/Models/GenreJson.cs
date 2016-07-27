using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MusicStore.Models
{
    public class GenreJson
    {
        public int GenreId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public virtual List<int> AlbumIds { get; set; }

        public static GenreJson From(Genre genre)
        {
            var result = new GenreJson()
            {
                GenreId = genre.GenreId,
                Name = genre.Name,
                Description = genre.Description,
                AlbumIds = new List<int>()

            };
            foreach (Album a in genre.Albums)
            {
                result.AlbumIds.Add(a.AlbumId);
            }
            return result;

        }
        public static List<GenreJson> From(List<Genre> genres)
        {
            List<GenreJson> results = new List<GenreJson>();
            if (genres == null)
                return results;

            foreach (Genre g in genres)
            {
                results.Add(GenreJson.From(g));
            }
            return results;
        }
    }
}
