using Moq;
using NUnit.Framework;
using FunWithSQL;
using FunWithSQL.Model;
using FunWithSQL.Repo;

namespace FunWithSQL.Tests
{
    [TestFixture]
    public class ProgramTests
    {
        private Mock<ISQLSongRepository> mockRepository;
        private Mock<IUserInterface> mockUI;

        [SetUp]
        public void SetUp()
        {
            mockRepository = new Mock<ISQLSongRepository>();
            mockUI = new Mock<IUserInterface>();
        }

        [Test]
        public void AddSong_ShouldAddSong_WhenValidData()
        {

            mockUI.SetupSequence(ui => ui.ReadLine())
                  .Returns("Test Title") 
                  .Returns("Test Album") 
                  .Returns("2023")      
                  .Returns("Test Artist");

            mockRepository.Setup(r => r.GetNewId()).Returns(3);  

            Program.AddSong(mockRepository.Object, mockUI.Object);

            mockRepository.Verify(r => r.AddSong(It.Is<Song>(s =>
                s.Title == "Test Title" &&
                s.Album == "Test Album" &&
                s.Year == 2023 &&
                s.Artist == "Test Artist" &&
                s.Id == 3)), Times.Once); 

            mockUI.Verify(ui => ui.Write(It.Is<string>(s => s == "Piosenka zosta³a dodana!")), Times.Once);
        }

        [Test]
        public void AddSong_ShouldNotAddSong_WhenTitleIsEmpty()
        {


            mockUI.SetupSequence(ui => ui.ReadLine())
                  .Returns("") 
                  .Returns("Test Album")
                  .Returns("2023")
                  .Returns("Test Artist");

            Program.AddSong(mockRepository.Object, mockUI.Object);

            mockRepository.Verify(r => r.AddSong(It.IsAny<Song>()), Times.Never); 

            mockUI.Verify(ui => ui.Write(It.Is<string>(s => s == "Tytu³ nie mo¿e byæ pusty.")), Times.Once);  
        }

        [Test]
        public void AddSong_ShouldNotAddSong_WhenYearIsNotANumber()
        {
            mockUI.SetupSequence(ui => ui.ReadLine())
                  .Returns("Test Title")
                  .Returns("Test Album")
                  .Returns("abc") 
                  .Returns("Test Artist");

            Program.AddSong(mockRepository.Object, mockUI.Object);

            mockRepository.Verify(r => r.AddSong(It.IsAny<Song>()), Times.Never); 

            mockUI.Verify(ui => ui.Write(It.Is<string>(s => s == "Niepoprawny rok.")), Times.Once);  
        }

        [Test]
        public void AddSong_ShouldNotAddSong_WhenAlbumIsEmpty()
        {
            mockUI.SetupSequence(ui => ui.ReadLine())
                  .Returns("Test Title")  
                  .Returns("")     
                  .Returns("2023") 
                  .Returns("Test Artist"); 

            Program.AddSong(mockRepository.Object, mockUI.Object);

            mockRepository.Verify(r => r.AddSong(It.IsAny<Song>()), Times.Never);  

            mockUI.Verify(ui => ui.Write(It.Is<string>(s => s == "Album nie mo¿e byæ pusty.")), Times.Once);  
        }

        [Test]
        public void AddSong_ShouldNotAddSong_WhenArtistIsEmpty()
        {
            mockUI.SetupSequence(ui => ui.ReadLine())
                  .Returns("Test Title") 
                  .Returns("Test Album") 
                  .Returns("2023") 
                  .Returns(""); 

            Program.AddSong(mockRepository.Object, mockUI.Object);

            mockRepository.Verify(r => r.AddSong(It.IsAny<Song>()), Times.Never); 

            mockUI.Verify(ui => ui.Write(It.Is<string>(s => s == "Artysta nie mo¿e byæ pusty.")), Times.Once);  
        }

        [Test]
        public void AddSong_ShouldNotAddSong_WhenAllFieldsAreEmpty()
        {
            mockUI.SetupSequence(ui => ui.ReadLine())
                  .Returns("")  
                  .Returns("") 
                  .Returns("")  
                  .Returns(""); 

            Program.AddSong(mockRepository.Object, mockUI.Object);

            mockRepository.Verify(r => r.AddSong(It.IsAny<Song>()), Times.Never);  

            mockUI.Verify(ui => ui.Write(It.Is<string>(s => s == "Tytu³ nie mo¿e byæ pusty.")), Times.Once);  
        }



        [Test]
        public void UpdateSong_ShouldUpdateSongInRepository()
        {
            mockUI.SetupSequence(ui => ui.ReadLine())
                  .Returns("Old Title") 
                  .Returns("Old Album") 
                  .Returns("2020") 
                  .Returns("Old Artist") 
                  .Returns("New Title")  
                  .Returns("New Album") 
                  .Returns("2023") 
                  .Returns("New Artist"); 

            Program.UpdateSong(mockRepository.Object, mockUI.Object);

            mockRepository.Verify(r => r.UpdateSong(It.Is<Song>(s =>
                s.Title == "Old Title" &&
                s.Album == "Old Album" &&
                s.Year == 2020 &&
                s.Artist == "Old Artist"), It.Is<Song>(s =>
                s.Title == "New Title" &&
                s.Album == "New Album" &&
                s.Year == 2023 &&
                s.Artist == "New Artist")), Times.Once); 

            mockUI.Verify(ui => ui.Write("Piosenka zosta³a zaktualizowana!"), Times.Once); 
        }
        [Test]
        public void UpdateSong_ShouldNotUpdate_WhenOldTitleIsEmpty()
        {
            mockUI.SetupSequence(x => x.ReadLine())
                  .Returns("")  
                  .Returns("Some Album") 
                  .Returns("2023") 
                  .Returns("Some Artist") 
                  .Returns("New Title")  
                  .Returns("New Album") 
                  .Returns("2024")  
                  .Returns("New Artist"); 

            Program.UpdateSong(mockRepository.Object, mockUI.Object);

            mockRepository.Verify(r => r.UpdateSong(It.IsAny<Song>(), It.IsAny<Song>()), Times.Never);  
            mockUI.Verify(ui => ui.Write("Tytu³ nie mo¿e byæ pusty."), Times.Once);  
        }

        [Test]
        public void UpdateSong_ShouldNotUpdate_WhenOldAlbumIsEmpty()
        {
            mockUI.SetupSequence(x => x.ReadLine())
                  .Returns("Some Title") 
                  .Returns("")  
                  .Returns("2023")
                  .Returns("Some Artist") 
                  .Returns("New Title") 
                  .Returns("New Album") 
                  .Returns("2024")  
                  .Returns("New Artist"); 

            Program.UpdateSong(mockRepository.Object, mockUI.Object);

            mockRepository.Verify(r => r.UpdateSong(It.IsAny<Song>(), It.IsAny<Song>()), Times.Never);  
            mockUI.Verify(ui => ui.Write("Album nie mo¿e byæ pusty."), Times.Once);  
        }

        [Test]
        public void UpdateSong_ShouldNotUpdate_WhenOldYearIsInvalid()
        {
            mockUI.SetupSequence(x => x.ReadLine())
                  .Returns("Some Title")  
                  .Returns("Some Album") 
                  .Returns("invalid")  
                  .Returns("Some Artist")  
                  .Returns("New Title")  
                  .Returns("New Album")  
                  .Returns("2024") 
                  .Returns("New Artist");  

            Program.UpdateSong(mockRepository.Object, mockUI.Object);

            mockRepository.Verify(r => r.UpdateSong(It.IsAny<Song>(), It.IsAny<Song>()), Times.Never); 
            mockUI.Verify(ui => ui.Write("Niepoprawny rok."), Times.Once); 
        }

        [Test]
        public void UpdateSong_ShouldNotUpdate_WhenOldArtistIsEmpty()
        {
            mockUI.SetupSequence(x => x.ReadLine())
                  .Returns("Some Title")  
                  .Returns("Some Album") 
                  .Returns("2023") 
                  .Returns("") 
                  .Returns("New Title") 
                  .Returns("New Album") 
                  .Returns("2024")  
                  .Returns("New Artist");  

            Program.UpdateSong(mockRepository.Object, mockUI.Object);

            mockRepository.Verify(r => r.UpdateSong(It.IsAny<Song>(), It.IsAny<Song>()), Times.Never); 
            mockUI.Verify(ui => ui.Write("Artysta nie mo¿e byæ pusty."), Times.Once); 
        }

        [Test]
        public void UpdateSong_ShouldNotUpdate_WhenNewTitleIsEmpty()
        {
            mockUI.SetupSequence(x => x.ReadLine())
                  .Returns("Some Title") 
                  .Returns("Some Album")  
                  .Returns("2023")  
                  .Returns("Some Artist")  
                  .Returns("") 
                  .Returns("New Album")
                  .Returns("2024") 
                  .Returns("New Artist");  

            Program.UpdateSong(mockRepository.Object, mockUI.Object);

            mockRepository.Verify(r => r.UpdateSong(It.IsAny<Song>(), It.IsAny<Song>()), Times.Never); 
            mockUI.Verify(ui => ui.Write("Nowy tytu³ nie mo¿e byæ pusty."), Times.Once);  
        }

        [Test]
        public void UpdateSong_ShouldNotUpdate_WhenNewAlbumIsEmpty()
        {
            mockUI.SetupSequence(x => x.ReadLine())
                  .Returns("Some Title")  
                  .Returns("Some Album") 
                  .Returns("2023")  
                  .Returns("Some Artist")  
                  .Returns("New Title")  
                  .Returns("")  
                  .Returns("2024") 
                  .Returns("New Artist"); 

            Program.UpdateSong(mockRepository.Object, mockUI.Object);

            mockRepository.Verify(r => r.UpdateSong(It.IsAny<Song>(), It.IsAny<Song>()), Times.Never); 
            mockUI.Verify(ui => ui.Write("Nowy album nie mo¿e byæ pusty."), Times.Once); 
        }

        [Test]
        public void UpdateSong_ShouldNotUpdate_WhenNewYearIsInvalid()
        {
            mockUI.SetupSequence(x => x.ReadLine())
                  .Returns("Some Title")  
                  .Returns("Some Album") 
                  .Returns("2023")  
                  .Returns("Some Artist") 
                  .Returns("New Title") 
                  .Returns("New Album")
                  .Returns("invalid") 
                  .Returns("New Artist"); 

            Program.UpdateSong(mockRepository.Object, mockUI.Object);

            mockRepository.Verify(r => r.UpdateSong(It.IsAny<Song>(), It.IsAny<Song>()), Times.Never); 
            mockUI.Verify(ui => ui.Write("Niepoprawny rok."), Times.Once);
        }

        [Test]
        public void UpdateSong_ShouldNotUpdate_WhenNewArtistIsEmpty()
        {
            mockUI.SetupSequence(x => x.ReadLine())
                  .Returns("Some Title")  
                  .Returns("Some Album")  
                  .Returns("2023")  
                  .Returns("Some Artist")  
                  .Returns("New Title") 
                  .Returns("New Album") 
                  .Returns("2024")  
                  .Returns("")  
                  .Returns("New Artist"); 

            Program.UpdateSong(mockRepository.Object, mockUI.Object);

            mockRepository.Verify(r => r.UpdateSong(It.IsAny<Song>(), It.IsAny<Song>()), Times.Never);  
            mockUI.Verify(ui => ui.Write("Nowy artysta nie mo¿e byæ pusty."), Times.Once);  
        }

        [Test]
        public void UpdateSong_ShouldUpdate_WhenAllDataIsValid()
        {
            mockUI.SetupSequence(x => x.ReadLine())
                  .Returns("Some Title")  
                  .Returns("Some Album")  
                  .Returns("2023")  
                  .Returns("Some Artist")  
                  .Returns("New Title")  
                  .Returns("New Album")  
                  .Returns("2024")  
                  .Returns("New Artist");  

            Program.UpdateSong(mockRepository.Object, mockUI.Object);

            mockRepository.Verify(r => r.UpdateSong(It.IsAny<Song>(), It.IsAny<Song>()), Times.Once);  
            mockUI.Verify(ui => ui.Write("Piosenka zosta³a zaktualizowana!"), Times.Once);  
        }

        [Test]
        public void DeleteSong_ShouldDeleteSongFromRepository()
        {
            mockUI.SetupSequence(ui => ui.ReadLine())
                  .Returns("Test Title")  
                  .Returns("Test Album")  
                  .Returns("2023") 
                  .Returns("Test Artist"); 

            Program.DeleteSong(mockRepository.Object, mockUI.Object);

            mockRepository.Verify(r => r.DeleteSong(It.Is<Song>(s =>
                s.Title == "Test Title" &&
                s.Album == "Test Album" &&
                s.Year == 2023 &&
                s.Artist == "Test Artist")), Times.Once); 

            mockUI.Verify(ui => ui.Write("Piosenka zosta³a usuniêta!"), Times.Once);
        }

        [Test]
        public void DeleteSong_ShouldNotDelete_WhenInvalidTitle()
        {
            mockUI.SetupSequence(x => x.ReadLine())
                  .Returns("") 
                  .Returns("Some Album")
                  .Returns("2023")
                  .Returns("Some Artist");

            Program.DeleteSong(mockRepository.Object, mockUI.Object);

            mockRepository.Verify(r => r.DeleteSong(It.IsAny<Song>()), Times.Never);
            mockUI.Verify(ui => ui.Write("Tytu³ nie mo¿e byæ pusty."), Times.Once);
        }

        [Test]
        public void DeleteSong_ShouldNotDelete_WhenInvalidAlbum()
        {
            mockUI.SetupSequence(x => x.ReadLine())
                  .Returns("Some Title")
                  .Returns("") 
                  .Returns("2023")
                  .Returns("Some Artist");

            Program.DeleteSong(mockRepository.Object, mockUI.Object);

            mockRepository.Verify(r => r.DeleteSong(It.IsAny<Song>()), Times.Never);
            mockUI.Verify(ui => ui.Write("Album nie mo¿e byæ pusty."), Times.Once);
        }

        [Test]
        public void DeleteSong_ShouldNotDelete_WhenInvalidYear()
        {
            mockUI.SetupSequence(x => x.ReadLine())
                  .Returns("Some Title")
                  .Returns("Some Album")
                  .Returns("Invalid Year") 
                  .Returns("Some Artist");

            Program.DeleteSong(mockRepository.Object, mockUI.Object);

            mockRepository.Verify(r => r.DeleteSong(It.IsAny<Song>()), Times.Never);
            mockUI.Verify(ui => ui.Write("Niepoprawny rok."), Times.Once);
        }

        [Test]
        public void DeleteSong_ShouldNotDelete_WhenInvalidArtist()
        {
            mockUI.SetupSequence(x => x.ReadLine())
                  .Returns("Some Title")
                  .Returns("Some Album")
                  .Returns("2023")
                  .Returns(""); 

            Program.DeleteSong(mockRepository.Object, mockUI.Object);

            mockRepository.Verify(r => r.DeleteSong(It.IsAny<Song>()), Times.Never);
            mockUI.Verify(ui => ui.Write("Artysta nie mo¿e byæ pusty."), Times.Once);
        }


    }
}
