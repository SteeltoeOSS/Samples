
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MusicStoreUI.Models
{
    public class Album
    {
        [ScaffoldColumn(false)]
        public int AlbumId { get; set; }

        public int GenreId { get; set; }
        public int ArtistId { get; set; }

        [Required]
        [StringLength(160, MinimumLength = 2)]
        public string Title { get; set; }

        [Range(0.01, 100.00)]
        [Required]
        [DataType(DataType.Currency)]
        public decimal Price { get; set; }

        [Display(Name = "Album Art URL")]
        [StringLength(1024)]
        public string AlbumArtUrl { get; set; }

        public int OrderCount { get; set; }
    
        public virtual Genre Genre { get; set; }
        public virtual Artist Artist { get; set; }

        [ScaffoldColumn(false)]
        [Required]
        public DateTime Created { get; set; }

        public Album()
        {
            Created = DateTime.UtcNow;
        }

        public static Album From(AlbumJson json)
        {
            if (json != null)
            {
                return new Album()
                {
                    AlbumId = json.AlbumId,
                    GenreId = json.GenreId,
                    ArtistId = json.ArtistId,
                    Title = json.Title,
                    Price = json.Price,
                    AlbumArtUrl = json.AlbumArtUrl,
                    Created = json.Created,
                    OrderCount = json.OrderCount,
                    Artist = Artist.From(json.Artist),
                    Genre = Genre.From(json.Genre)
                };
            } else
            {
                return new Album()
                {
                    AlbumId = 0,
                    Title = "Unknown",
                };
            }
  

        }
        public static List<Album> From(List<AlbumJson> albums)
        {
            List<Album> results = new List<Album>();
            if (albums == null)
                return results;

            foreach (AlbumJson a in albums)
            {
                results.Add(Album.From(a));
            }
            return results;
        }
    }
}
