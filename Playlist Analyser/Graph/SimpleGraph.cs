using Playlist_Analyser.Export;
using QuickGraph;
using QuickGraph.Graphviz;
using QuickGraph.Graphviz.Dot;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;

namespace Playlist_Analyser.Graph
{
    public class SimpleGraph : IGraph
    {
        private readonly static String SIMPLE = "Simple";

        private readonly static String GRAPH = "Graph";

        private readonly static String ARTIST = "Artist";

        private readonly static String GENRE = "Genre";

        private readonly static String MUSIC = "Music";

        private readonly static String SAG = SIMPLE + ARTIST + GRAPH;

        private readonly static String SGG = SIMPLE + GENRE + GRAPH;

        private readonly static String SMG = SIMPLE + MUSIC + GRAPH;


        private readonly List<Music> MusicList = new List<Music>();

        private readonly List<Artist> ArtistList = new List<Artist>();

        private readonly List<Genre> GenreList = new List<Genre>();

        private GraphvizAlgorithm<String, Edge<String>> Graphviz;

        private readonly String ExportFormat;

        private readonly Graph GDot;


        public SimpleGraph(List<Music> musicList, List<Artist> artistList, List<Genre> genreList, String exportFormat)
        {
            this.GDot = new Graph(musicList, artistList, genreList);

            this.MusicList = musicList;
            this.ArtistList = artistList;
            this.GenreList = genreList;
            this.ExportFormat = exportFormat;
        }

        public void GenerateGenreGraph()
        {
            this.GDot.GenerateOrigin();

            this.GDot.GenerateGenre();
            this.GDot.ConnectOriginToGenre();

            InitializeGraph();

            FormatVertex();
            HighlightGenre();

            DotExport.Export(this.Graphviz, SGG);
            GraphvizExport.Export("/C circo", SGG, this.ExportFormat);
        }

        public void GenerateArtistGraph()
        {
            this.GDot.GenerateGenre();

            this.GDot.GenerateArtist();
            this.GDot.ConnectArtistToGenre();

            InitializeGraph();

            FormatVertex();
            HighlightGenre();
            HighlightArtist();
            HighlightTargetedArtist();

            DotExport.Export(this.Graphviz, SAG);
            GraphvizExport.Export("/C sfdp -Goverlap=prism", SAG, this.ExportFormat);
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
            HighlightTargetedArtist();

            DotExport.Export(this.Graphviz, SMG);
            GraphvizExport.Export("/C sfdp -Goverlap=prism", SMG, this.ExportFormat);
        }

        private void InitializeGraph()
        {
            IVertexAndEdgeListGraph<String, Edge<String>> g = this.GDot.VertexNodeList;
            this.Graphviz = new GraphvizAlgorithm<String, Edge<String>>(g);
        }

        public void HighlightGenre()
        {
            this.Graphviz.FormatVertex += (sender, args) => {
                foreach (Genre genre in this.GenreList)
                {
                    if (genre.Name == args.Vertex.ToString())
                    {
                        args.VertexFormatter.Shape = GraphvizVertexShape.Circle;
                        args.VertexFormatter.Style = GraphvizVertexStyle.Filled;
                        args.VertexFormatter.FillColor = Color.Red;
                        args.VertexFormatter.StrokeColor = Color.Orange;
                    }
                }
            };
        }

        public void HighlightArtist()
        {
            this.Graphviz.FormatVertex += (sender, args) =>
            {
                foreach (Artist artist in this.ArtistList)
                {
                    if (artist.Name == args.Vertex.ToString())
                    {
                        args.VertexFormatter.Shape = GraphvizVertexShape.Circle;
                        args.VertexFormatter.Style = GraphvizVertexStyle.Filled;
                        args.VertexFormatter.FillColor = Color.Green;
                        args.VertexFormatter.FontColor = Color.White;
                        args.VertexFormatter.StrokeColor = Color.Green;
                    }
                }
            };
        }

        public void HighlightTargetedArtist()
        {
            this.Graphviz.FormatVertex += (sender, args) =>
            {
                foreach (Artist artist in this.ArtistList)
                {
                    if (artist.Name == args.Vertex.ToString())
                    {
                        String target = args.Vertex.ToString().ToLower();
                        if (target.Contains("lala") && target.Contains("hsu"))
                        {
                            args.VertexFormatter.Shape = GraphvizVertexShape.Circle;
                            args.VertexFormatter.Style = GraphvizVertexStyle.Filled;
                            args.VertexFormatter.FillColor = Color.Yellow;
                            args.VertexFormatter.FontColor = Color.Black;
                            args.VertexFormatter.StrokeColor = Color.Yellow;
                        }
                    }
                }
            };
        }

        private void FormatVertex()
        {
            this.Graphviz.FormatVertex += FormatVertex;
        }

        private static void FormatVertex(object sender, FormatVertexEventArgs<String> e)
        {
            e.VertexFormatter.Label = "";
            e.VertexFormatter.Comment = "";
        }
    }
}
