using FunWithSQL;
using FunWithSQL.Model;
using FunWithSQL.Repo;
using System;

public class Program
{
    private readonly IUserInterface _ui;
    private readonly SongService _songService;

    public Program(IUserInterface ui, SongService songService)
    {
        _ui = ui;
        _songService = songService;
    }

    static void Main(string[] args)
    {
        var repository = new SQLSongRepository();
        var ui = new ConsoleUserInterface();

        while (true)
        {
            Console.Clear();
            Console.WriteLine("Co chciałbyś zrobić? Wciśnij: ");
            Console.WriteLine("(1) Dodaj piosenkę");
            Console.WriteLine("(2) Usuń piosenkę");
            Console.WriteLine("(3) Uaktualnij piosenkę");
            Console.WriteLine("(4) Wyświetl wszystkie piosenki");
            Console.WriteLine("(5) Wyświetl piosenki z filtrem");
            Console.WriteLine("(0) Wyjście");

            var choice = Console.ReadKey().KeyChar;
            Console.Clear();

            switch (choice)
            {
                case '1':
                    AddSong(repository, ui);
                    break;
                case '2':
                    DeleteSong(repository, ui);
                    break;
                case '3':
                    UpdateSong(repository, ui);
                    break;
                case '4':
                    ReadAll(repository, ui);
                    break;
                case '5':
                    ReadAllByFilter(repository, ui);
                    break;
                case '0':
                    Console.WriteLine("Do widzenia!");
                    return;
                default:
                    Console.WriteLine("Niepoprawny wybór. Spróbuj ponownie.");
                    break;
            }

            Console.WriteLine("\nNaciśnij dowolny klawisz, aby kontynuować...");
            Console.ReadKey();
        }
    }

    public static void AddSong(ISQLSongRepository repository, IUserInterface ui)
    {
        Console.WriteLine("Dodawanie piosenki...");

        Console.Write("Podaj tytuł: ");
        string title = ui.ReadLine();
        if (string.IsNullOrEmpty(title))
        {
            ui.Write("Tytuł nie może być pusty.");
            return;
        }

        Console.Write("Podaj album: ");
        string album = ui.ReadLine();
        if (string.IsNullOrEmpty(album))
        {
            ui.Write("Album nie może być pusty.");
            return;
        }

        Console.Write("Podaj rok wydania: ");
        if (!int.TryParse(ui.ReadLine(), out int year))
        {
            ui.Write("Niepoprawny rok.");
            return;
        }

        Console.Write("Podaj artystę: ");
        string artist = ui.ReadLine();
        if (string.IsNullOrEmpty(artist))
        {
            ui.Write("Artysta nie może być pusty.");
            return;
        }
        var id = repository.GetNewId();

        var song = new Song(id, title, album, year, artist);
        repository.AddSong(song);

        ui.Write("Piosenka została dodana!");
    }

    public static void DeleteSong(ISQLSongRepository repository, IUserInterface ui)
    {
        ui.Clear();
        ui.Write("Usuwanie piosenki...");

        ui.Write("Podaj tytuł: ");
        string title = ui.ReadLine();
        if (string.IsNullOrEmpty(title))
        {
            ui.Write("Tytuł nie może być pusty.");
            return;
        }


        ui.Write("Podaj album: ");
        string album = ui.ReadLine();
        if (string.IsNullOrEmpty(album))
        {
            ui.Write("Album nie może być pusty.");
            return;
        }

        ui.Write("Podaj rok wydania: ");
        if (!int.TryParse(ui.ReadLine(), out int year))
        {
            ui.Write("Niepoprawny rok.");
            return;
        }

        ui.Write("Podaj artystę: ");
        string artist = ui.ReadLine();
        if (string.IsNullOrEmpty(artist))
        {
            ui.Write("Artysta nie może być pusty.");
            return;
        }

        var song = new Song(0, title, album, year, artist);
        repository.DeleteSong(song);

        ui.Write("Piosenka została usunięta!");
    }

    public static void UpdateSong(ISQLSongRepository repository, IUserInterface ui)
    {
        ui.Clear();
        ui.Write("Aktualizowanie piosenki...");

        ui.Write("Podaj dane istniejącej piosenki:");
        ui.Write("Tytuł: ");
        string oldTitle = ui.ReadLine();
        if (string.IsNullOrEmpty(oldTitle))
        {
            ui.Write("Tytuł nie może być pusty.");
            return;
        }

        ui.Write("Album: ");
        string oldAlbum = ui.ReadLine();
        if (string.IsNullOrEmpty(oldAlbum))
        {
            ui.Write("Album nie może być pusty.");
            return;
        }

        ui.Write("Rok wydania: ");
        if (!int.TryParse(ui.ReadLine(), out int oldYear))
        {
            ui.Write("Niepoprawny rok.");
            return;
        }

        ui.Write("Artysta: ");
        string oldArtist = ui.ReadLine();
        if (string.IsNullOrEmpty(oldArtist))
        {
            ui.Write("Artysta nie może być pusty.");
            return;
        }

        ui.Write("\nPodaj nowe dane piosenki:");
        ui.Write("Nowy tytuł: ");
        string newTitle = ui.ReadLine();
        if (string.IsNullOrEmpty(newTitle))
        {
            ui.Write("Nowy tytuł nie może być pusty.");
            return;
        }

        ui.Write("Nowy album: ");
        string newAlbum = ui.ReadLine();
        if (string.IsNullOrEmpty(newAlbum))
        {
            ui.Write("Nowy album nie może być pusty.");
            return;
        }

        ui.Write("Nowy rok wydania: ");
        if (!int.TryParse(ui.ReadLine(), out int newYear))
        {
            ui.Write("Niepoprawny rok.");
            return;
        }

        ui.Write("Nowy artysta: ");
        string newArtist = ui.ReadLine();
        if (string.IsNullOrEmpty(newArtist))
        {
            ui.Write("Nowy artysta nie może być pusty.");
            return;
        }

        var oldSong = new Song(0, oldTitle, oldAlbum, oldYear, oldArtist);
        var newSong = new Song(0, newTitle, newAlbum, newYear, newArtist);
        repository.UpdateSong(oldSong, newSong);

        ui.Write("Piosenka została zaktualizowana!");
    }

    public static void ReadAll(ISQLSongRepository repository, IUserInterface ui)
    {
        ui.Clear();
        ui.Write("Lista wszystkich piosenek:");

        var songs = repository.ReadAll();
        if (songs.Length == 0)
        {
            ui.Write("Brak piosenek w bazie.");
            return;
        }

        foreach (var song in songs)
        {
            ui.Write(song.ToString());
        }
    }

    public static void ReadAllByFilter(ISQLSongRepository repository, IUserInterface ui)
    {
        Console.Clear();
        Console.WriteLine("Wczytywanie piosenek z filtrem...");

        Console.Write("Podaj parametrami: ");
        string param = Console.ReadLine();

        Console.Write("Podaj filtr: ");
        string filtr = Console.ReadLine();

        var songs = repository.ReadAllByFilter(param, filtr);

        Console.WriteLine($"Znaleziono {songs.Length} piosenek:");
        foreach (var song in songs)
        {
            ui.Write($"{song.Title} - {song.Artist} ({song.Year})");
        }
    }


}
