using FunWithSQL.Model;
using System.Data.SqlClient;
namespace FunWithSQL.Repo
{

public interface ISQLSongRepository
{
    void AddSong(Song song);
    void DeleteSong(Song song);
    void UpdateSong(Song song, Song newsong);
    Song[] ReadAll();
    Song[] ReadAllByFilter(string param, string filtr);
    int GetNewId();
}
public class SQLSongRepository : ISQLSongRepository
{
    private string _connectionString = "Server=S122-507;Initial Catalog=MW; Integrated Security=true;";

    public void AddSong(Song song)
    {
        var query = $"insert into Song values({song.Id}, '{song.Title}', '{song.Album}', {song.Year}, '{song.Artist}')";
        Execute(query);
    }

    public void DeleteSong(Song song)
    {
        var query = $"delete from Song where Title='{song.Title}' and Album='{song.Album}' and Year={song.Year} and Artist='{song.Artist}'";
        Execute(query);
    }

    public Song[] ReadAll()
    {
        var query = "Select * from Song";
        return Read(query);
    }

    public int GetNewId()
    {
        var query = $"select top 1 * from Song order by ID desc";
        var id = 0;
        using (var sqlConnection = new SqlConnection(_connectionString))
        {
            sqlConnection.Open();
            using (var sqlCommand = new SqlCommand(query, sqlConnection))
            {
                var reader = sqlCommand.ExecuteReader();
                while (reader.Read())
                {
                    id = int.Parse(reader["ID"].ToString());
                }
            }
            return id;
        }
    }

    public void UpdateSong(Song song, Song newsong)
    {
        var query = $"update Song set Title='{newsong.Title}', Album='{newsong.Album}', Year={newsong.Year}, Artist='{newsong.Artist}' where Title='{song.Title}' and Album='{song.Album}' and Year={song.Year} and Artist='{song.Artist}'";
        Execute(query);
    }

    public Song[] ReadAllByFilter(string param, string filtr)
    {
        var query = $"Select * from Song where {param} like @Filter";
        var list = new List<Song>();
        using (var sqlConnection = new SqlConnection(_connectionString))
        {
            sqlConnection.Open();
            using (var sqlCommand = new SqlCommand(query, sqlConnection))
            {
                sqlCommand.Parameters.AddWithValue("@Filter", filtr + "%");
                var reader = sqlCommand.ExecuteReader();
                while (reader.Read())
                {
                    var id = int.Parse(reader["ID"].ToString());
                    var title = reader["Title"].ToString();
                    var album = reader["Album"].ToString();
                    var year = int.Parse(reader["Year"].ToString());
                    var artist = reader["Artist"].ToString();

                    var song = new Song(id, title, album, year, artist);
                    list.Add(song);
                }
            }
            return list.ToArray();
        }
    }

    private void Execute(string query)
    {
        using (var sqlConnection = new SqlConnection(_connectionString))
        {
            sqlConnection.Open();
            using (var sqlCommand = new SqlCommand(query, sqlConnection))
            {
                sqlCommand.ExecuteNonQuery();
            }
        }
    }

    private Song[] Read(string query)
    {
        var list = new List<Song>();
        using (var sqlConnection = new SqlConnection(_connectionString))
        {
            sqlConnection.Open();
            using (var sqlCommand = new SqlCommand(query, sqlConnection))
            {
                var reader = sqlCommand.ExecuteReader();
                while (reader.Read())
                {
                    var id = int.Parse(reader["ID"].ToString());
                    var title = reader["Title"].ToString();
                    var album = reader["Album"].ToString();
                    var year = int.Parse(reader["Year"].ToString());
                    var artist = reader["Artist"].ToString();

                    var song = new Song(id, title, album, year, artist);
                    list.Add(song);
                }
            }
            return list.ToArray();
        }
    }
}

}