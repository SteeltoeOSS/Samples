using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MusicStore.Models
{
    public class AlbumJson
    {
    
        public int AlbumId { get; set; }
     
        public int GenreId { get; set; }
    
        public int ArtistId { get; set; }
        
        public string Title { get; set; }

        public decimal Price { get; set; }

        public string AlbumArtUrl { get; set; }

        public GenreJson Genre { get; set; }
        public ArtistJson Artist { get; set; }

        public DateTime Created { get; set; }

        public int OrderCount { get; set; }

        public AlbumJson()
        {
    
        }
        public static AlbumJson From(Album album)
        {
            var result = new AlbumJson()
            {
                AlbumId = album.AlbumId,
                GenreId = album.GenreId,
                ArtistId = album.ArtistId,
                Title = album.Title,
                Price = album.Price,
                AlbumArtUrl = album.AlbumArtUrl,
                Created = album.Created,
                OrderCount = album.OrderCount,
                Artist = ArtistJson.From(album.Artist),
                Genre = GenreJson.From(album.Genre)
            };
            return result;
        }
  
        public static List<AlbumJson> From(List<Album> albums)
        {
            List<AlbumJson> results = new List<AlbumJson>();
            if (albums == null)
                return results;
            foreach (Album a in albums)
            {
                results.Add(AlbumJson.From(a));
            }
            return results;
        }
    }
}
