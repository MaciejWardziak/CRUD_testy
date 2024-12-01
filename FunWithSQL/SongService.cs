using FunWithSQL.Model;
using FunWithSQL.Repo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FunWithSQL
{
    public class SongService
    {
        private readonly ISQLSongRepository _repository;

        public SongService(ISQLSongRepository repository)
        {
            _repository = repository;
        }

        public void AddSong(Song song)
        {
            _repository.AddSong(song);
        }

        public void DeleteSong(Song song)
        {
            _repository.DeleteSong(song);
        }

        public void UpdateSong(Song oldSong, Song newSong)
        {
            _repository.UpdateSong(oldSong, newSong);
        }

        public Song[] GetAllSongs()
        {
            return _repository.ReadAll();
        }

        public Song[] GetSongsByFilter(string filter, string value)
        {
            return _repository.ReadAllByFilter(filter, value);
        }
    }

}
