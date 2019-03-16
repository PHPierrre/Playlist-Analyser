using QuickGraph;
using System;
using System.Collections.Generic;

namespace Playlist_Analyser.Graph
{
    class Graph
    {
        private readonly List<Music> MusicList = new List<Music>();

        private readonly List<Artist> ArtistList = new List<Artist>();

        private readonly List<Genre> GenreList = new List<Genre>();

        public QuickGraph.AdjacencyGraph<String, Edge<String>> VertexNodeList { get; private set; } = new AdjacencyGraph<String, Edge<String>>(false);


        public Graph(List<Music> musicList, List<Artist> artistList, List<Genre> genreList)
        {
            this.MusicList = musicList;
            this.ArtistList = artistList;
            this.GenreList = genreList;
        }

        // Generate methods 

        public void GenerateOrigin()
        {
            this.VertexNodeList.AddVertex("o");
        }

        public void GenerateGenre()
        {
            foreach (Genre genre in this.GenreList)
            {
                this.VertexNodeList.AddVertex(genre.Name);
            }
        }

        public void GenerateArtist()
        {
            foreach (Artist artist in this.ArtistList)
            {
                this.VertexNodeList.AddVertex(artist.Name);
            }
        }

        public void GenerateMusic()
        {
            foreach (Music music in this.MusicList)
            {
                this.VertexNodeList.AddVertex(music.Title);
            }
        }

        //Connector methods

        public void ConnectOriginToGenre()
        {
            foreach (Genre genre in this.GenreList)
            {
                AddEdge(genre.Name, "o", 1);
            }

        }

        public void ConnectArtistToGenre()
        {
            foreach (Music music in this.MusicList)
            {
                AddEdge(music.Artist, music.Genre, 1);
            }

        }

        public void ConnectMusicToArtist()
        {
            foreach (Music music in this.MusicList)
            {
                AddEdge(CountTitle(music.Title), music.Artist, 1);
            }

        }

        //Tools

        private String CountTitle(String s)
        {
            /*if (s.Length > 25)
            {
                return s.Substring(0, 10);
            }*/
            return s;
        }

        private void AddEdge(String from, String to, int weight)
        {
            Edge<String> edge = new Edge<String>(from, to);
            this.VertexNodeList.AddEdge(edge);
        }
    }
}
