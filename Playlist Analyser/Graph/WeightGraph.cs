using Playlist_Analyser.Export;
using QuickGraph;
using QuickGraph.Graphviz;
using QuickGraph.Graphviz.Dot;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Playlist_Analyser.Graph
{
    public class WeightGraph : IGraph
    {
        private readonly static String WEIGHTED = "Weighted";

        private readonly static String GRAPH = "Graph";

        private readonly static String ARTIST = "Artist";

        private readonly static String GENRE = "Genre";

        private readonly static String MUSIC = "Music";

        private readonly static String WAG = WEIGHTED + ARTIST + GRAPH;

        private readonly static String WGG = WEIGHTED + GENRE + GRAPH;

        private readonly static String WMG = WEIGHTED + MUSIC + GRAPH;


        private readonly List<Music> MusicList = new List<Music>();

        private readonly List<Artist> ArtistList = new List<Artist>();

        private List<Genre> GenreList = new List<Genre>();

        private GraphvizAlgorithm<String, Edge<String>> Graphviz;

        private readonly String ExportFormat;

        private readonly Graph GDot;

        private readonly Random random;

        public WeightGraph(List<Music> musicList, List<Artist> artistList, List<Genre> genreList, String exportFormat)
        {
            this.GDot = new Graph(musicList, artistList, genreList);

            this.MusicList = musicList;
            this.ArtistList = artistList;
            this.GenreList = genreList;
            this.ExportFormat = exportFormat;

           this.random = new Random();
        }

        public void GenerateGenreGraph()
        {
            this.GDot.GenerateGenre();

            InitializeGraph();

            FormatVertex();
            HighlightGenre();

            DotExport.Export(this.Graphviz, WGG);
            GraphvizExport.Export("/C sfdp -Goverlap=prism", WGG, this.ExportFormat);
        }

        public void GenerateArtistGraph()
        {
            this.GDot.GenerateGenre();
            this.GDot.GenerateArtist();
            this.GDot.ConnectArtistToGenre();

            InitializeGraph();

            FormatVertex();
            HighlightGenre();
            IndexGenreMusic();
            HighlightArtist();

            DotExport.Export(this.Graphviz, WAG);
            GraphvizExport.Export("/C neato -Goverlap=prism200", WAG, this.ExportFormat);
        }

        public void GenerateMusicGraph()
        {
            this.GDot.GenerateGenre();
            this.GDot.GenerateArtist();
            this.GDot.GenerateMusic();

            this.GDot.ConnectArtistToGenre();
            this.GDot.ConnectMusicToArtist();

            InitializeGraph();

            FormatVertex();
            HighlightGenre();
            HighlightArtist();
            //HighlightMusic();

            DotExport.Export(this.Graphviz, WMG);
            GraphvizExport.Export("/C sfdp -Goverlap=prism", WMG, this.ExportFormat);
        }

        public void HighlightArtist()
        {
            this.Graphviz.FormatVertex += (sender, args) =>
            {
                foreach (Artist artist in this.ArtistList)
                {
                    if (artist.Name == args.Vertex.ToString())
                    {
                        args.VertexFormatter.ToolTip = "Artiste : " + artist.Name;
                        args.VertexFormatter.Style = GraphvizVertexStyle.Filled;
                        args.VertexFormatter.Shape = GraphvizVertexShape.Circle;

                        Genre genreArtist = null;
                        String gs = "";
                        int max = 0;

                        foreach(Genre g in artist.genres.Values)
                        {
                            if(g.Count > max)
                            {
                                max = g.Count;
                                genreArtist = g;
                                gs = g.Name;
                            }
                        }
                        Genre gn = this.GenreList.Find(x => x.Name == gs);
                        args.VertexFormatter.FillColor = Color.FromArgb(125, gn.Color.R, gn.Color.G, gn.Color.B);
                        args.VertexFormatter.FontColor = Color.White;
                        args.VertexFormatter.StrokeColor = Color.FromArgb(125, gn.Color.R, gn.Color.G, gn.Color.B);

                        args.VertexFormatter.FixedSize = true;
                        float calc = (float) Math.Pow(Math.Log(artist.Count), 2) +1;
                            
                        args.VertexFormatter.Size = new SizeF(calc, calc);
                    }
                }
            };
        }

        public void HighlightGenre()
        {
            this.Graphviz.FormatVertex += (sender, args) =>
            {
                Color c;
                for(int i = 0; i < this.GenreList.Count; i++)
                {
                    if (this.GenreList[i].Name == args.Vertex.ToString())
                    {
                        c = createColor();
                        this.GenreList[i].Color = c;

                        args.VertexFormatter.ToolTip = "Genre : " + this.GenreList[i].Name;
                        args.VertexFormatter.Shape = GraphvizVertexShape.Circle;
                        args.VertexFormatter.Style = GraphvizVertexStyle.Filled;
                        args.VertexFormatter.FillColor = Color.FromArgb(c.R, c.G, c.B);
                        args.VertexFormatter.FontColor = Color.White;
                        args.VertexFormatter.StrokeColor = Color.FromArgb(0, 0, 0);
                        args.VertexFormatter.FixedSize = true;
                        args.VertexFormatter.Size = new SizeF((float)this.GenreList[i].Count / 40, (float)this.GenreList[i].Count / 40);
                    }
                }
            };
        }

        public Color createColor()
        {
            Color color = Color.FromArgb(this.random.Next(256), this.random.Next(256), this.random.Next(256));
            return color;
        }

        public void IndexGenreMusic()
        {
            foreach(Music music in this.MusicList)
            {
                Artist a = null;
                foreach(Artist artist in this.ArtistList)
                {
                    if(artist.Name == music.Artist)
                    {
                        a = artist;
                    }
                }

                if (a.genres.ContainsKey(music.Genre))
                {
                    Genre g = a.genres[music.Genre];
                    g.Count++;
                }
                else
                {
                    Genre old = this.GenreList.Find(x => x.Name == music.Genre);
                    Genre newG = new Genre(music.Genre);

                    newG.Color = Color.FromArgb(old.Color.R, old.Color.G, old.Color.B);
                    newG.Count = old.Count;
                    a.genres.Add(music.Genre, newG);
                }

            }
        }

        private void InitializeGraph()
        {
            IVertexAndEdgeListGraph<String, Edge<String>> g = this.GDot.VertexNodeList;
            this.Graphviz = new GraphvizAlgorithm<String, Edge<String>>(g);
            this.Graphviz.FormatEdge += FormatEdge;
        }

        private void FormatEdge(object sender, FormatEdgeEventArgs<string, Edge<string>> e)
        {
            e.EdgeFormatter.StrokeColor = Color.Transparent;
        }

        private void FormatVertex()
        {
            this.Graphviz.FormatVertex += FormatVertex;
        }

        private static void FormatVertex(object sender, FormatVertexEventArgs<String> e)
        {
            e.VertexFormatter.Label = e.Vertex.ToString().Replace("\"", "");
            e.VertexFormatter.Font = new Font("Gulim", 12, FontStyle.Bold);
            e.VertexFormatter.Comment = e.Vertex.ToString().Replace("\"", "");
        }

        public void HighlightTargetedArtist()
        {
            throw new NotImplementedException();
        }
    }
}
