using MusicStoreUI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MusicStoreUI.Services
{
    public interface IMusicStore
    {
        Task<List<Genre>> GetGenresAsync();

        Task<List<Album>> GetTopSellingAlbumsAsync(int count);

        Task<Genre> GetGenreAsync(string genre);

        Task<Genre> GetGenreAsync(int id);

        Task<Album> GetAlbumAsync(int id);

        Task<List<Album>> GetAllAlbumsAsync();

        Task<List<Artist>> GetAllArtistsAsync();

        Task<Artist> GetArtistAsync(int id);

        Task<bool> AddAlbumAsync(Album album);

        Task<bool> UpdateAlbumAsync(Album album);

        Task<bool> RemoveAlbumAsync(Album album);

        Task<Album> GetAlbumAsync(string title);


    }
}
