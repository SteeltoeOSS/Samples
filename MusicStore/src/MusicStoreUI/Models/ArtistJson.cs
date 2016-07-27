using System.Collections.Generic;


namespace MusicStoreUI.Models
{
    public class ArtistJson
    {
        public int ArtistId { get; set; }
        public string Name { get; set; }

        public static ArtistJson From(Artist artist)
        {
            var result = new ArtistJson()
            {
                ArtistId = artist.ArtistId,
                Name = artist.Name
            };
            return result;
        }

        public static List<ArtistJson> From(List<Artist> artists)
        {
            List<ArtistJson> results = new List<ArtistJson>();
            if (artists == null)
                return results;

            foreach (Artist a in artists)
            {
                results.Add(ArtistJson.From(a));
            }
            return results;
        }
    }
}

