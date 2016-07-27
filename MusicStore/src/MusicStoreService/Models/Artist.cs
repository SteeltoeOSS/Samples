
using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;

namespace MusicStore.Models
{

    public class Artist
    {
        public int ArtistId { get; set; }

        [Required]
        public string Name { get; set; }

        public static Artist From(ArtistJson artist)
        {
            if (artist != null)
            {
                return new Artist()
                {
                    ArtistId = artist.ArtistId,
                    Name = artist.Name
                };
            }
            else
            {
                return new Artist()
                {
                    ArtistId = 0,
                    Name = "Unknown"
                };
            }
        }
        public static List<Artist> From(List<ArtistJson> artist)
        {
            List<Artist> results = new List<Artist>();
            if (artist == null)
                return results;

            foreach (ArtistJson a in artist)
            {
                results.Add(Artist.From(a));
            }
            return results;
        }
    }
}