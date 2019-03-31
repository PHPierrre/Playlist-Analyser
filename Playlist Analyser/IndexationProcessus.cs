using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using QuickGraph;

namespace Playlist_Analyser
{
    public class IndexationProcessus
    {
        public List<Music> Musics { get; private set; } = new List<Music>();
        public List<Artist> Artists { get; private set; } = new List<Artist>();
        public List<Genre> Genres { get; private set; } = new List<Genre>();
        public List<Year> Years { get; private set; } = new List<Year>();
        
        public IndexationProcessus(String path)
        {
            this.IndexAllMusics(path);
        }

        private void IndexAllMusics(String folder)
        {
            FileInfo[] files = GetAllMusics(folder);
            foreach (FileInfo file in files)
            {
                if (file.Extension != ".mp3") continue;

                Music music = AddMusic(file);
                Genre genre = AddGenre(music);
                Artist artist = AddArtist(music);
                Year year = AddYear(music);
            }


            DirectoryInfo[] subfolders = GetSubFolders(folder);

            foreach (DirectoryInfo subfolder in subfolders)
            {
                this.IndexAllMusics(subfolder.FullName);
            }

        }

        private Music AddMusic(FileInfo file)
        {
            Music music = new Music();
            music.FilePath = file.DirectoryName + @"\" + file;
            music = ExtractTags(music);

            this.Musics.Add(music);
            return music;
        }

        private Artist AddArtist(Music music)
        {
            Artist artist = new Artist(music.Artist);

            if (this.Artists.Exists(a => a.Name == artist.Name))
            {
                this.Artists.First(a => a.Name == artist.Name).Count++;
            }
            else
            {
                this.Artists.Add(artist);
            }

            return artist;
        }

        private Genre AddGenre(Music music)
        {
            Genre genre = new Genre(music.Genre);

            if (this.Genres.Exists(g => g.Name == genre.Name))
            {
                this.Genres.First(g => g.Name == genre.Name).Count++;
            }
            else
            {
                this.Genres.Add(genre);
            }

            return genre;
        }
    
        private Year AddYear(Music music)
        {
            Year year = new Year(music.Year);

            if (this.Years.Exists(y => y.Name == year.Name))
            {
                this.Years.First(y => y.Name == year.Name).Count++;
            }
            else
            {
                this.Years.Add(year);
            }

            return year;
        }

        private Music ExtractTags(Music music)
        {
            TagLib.File file = TagLib.File.Create(music.FilePath);

            music.Title = String.IsNullOrEmpty(file.Tag.Title) ? "Titre Inconnu" : file.Tag.Title;

            if(String.IsNullOrEmpty(file.Tag.FirstPerformer)){
                music.Artist = "Artiste Inconnu";
            } else {
                music.Artist = file.Tag.FirstPerformer;
            }
            
            music.Album = file.Tag.Album;
            music.Year = file.Tag.Year;
            music.Comment = file.Tag.Comment;
            music.Pictures = file.Tag.Pictures;

            if(file.Tag.Genres.Length > 0)
            {
                music.Genre = file.Tag.Genres[0];
            } else
            {
                music.Genre = "Genre Inconnu";
            }
           

            return music;
        }

        private DirectoryInfo[] GetSubFolders(String folder)
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(folder);
            DirectoryInfo[] folders = directoryInfo.GetDirectories();
            return folders;
        }

        private FileInfo[] GetAllMusics(String folder)
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(folder);
            FileInfo[] musics = directoryInfo.GetFiles();
            return musics;
        }

    }
}
