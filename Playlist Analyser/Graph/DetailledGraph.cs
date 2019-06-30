using Playlist_Analyser.Export;
using QuickGraph;
using QuickGraph.Graphviz;
using QuickGraph.Graphviz.Dot;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace Playlist_Analyser.Graph
{
    public class DetailledGraph : IGraph
    {
        private readonly static String DETAILLED = "Detailled";

        private readonly static String GRAPH = "Graph";

        private readonly static String ARTIST = "Artist";

        private readonly static String GENRE = "Genre";

        private readonly static String MUSIC = "Music";

        private readonly static String DAG = DETAILLED + ARTIST + GRAPH;

        private readonly static String DGG = DETAILLED + GENRE + GRAPH;

        private readonly static String DMG = DETAILLED + MUSIC + GRAPH;


        private readonly List<Music> MusicList = new List<Music>();

        private readonly List<Artist> ArtistList = new List<Artist>();

        private readonly List<Genre> GenreList = new List<Genre>();

        private GraphvizAlgorithm<String, Edge<String>> Graphviz;

        private readonly String ExportFormat;

        private readonly Graph GDot;


        public DetailledGraph(List<Music> musicList, List<Artist> artistList, List<Genre> genreList, String exportFormat)
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

            DotExport.Export(this.Graphviz, DGG);
            GraphvizExport.Export("/C circo", DGG, this.ExportFormat);
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

            DotExport.Export(this.Graphviz, DAG);
            GraphvizExport.Export("/C sfdp -Goverlap=prism", DAG, this.ExportFormat);
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

            DotExport.Export(this.Graphviz, DMG);
            GraphvizExport.Export("/C sfdp -Goverlap=prism", DMG, this.ExportFormat);
        }

        private void InitializeGraph()
        {
            IVertexAndEdgeListGraph<String, Edge<String>> g = this.GDot.VertexNodeList;
            this.Graphviz = new GraphvizAlgorithm<String, Edge<String>>(g);
            //this.Graphviz.FormatCluster += Graphviz_FormatCluster;
        }

        /*private static void Graphviz_FormatCluster(object sender, FormatClusterEventArgs<string, Edge<string>> e)
        {
            e.GraphFormat.Name = "Detailled graph of";
            e.GraphFormat.Font = new Font("Gulim", 12);
            e.GraphFormat.IsCentered = true;
            e.GraphFormat.IsNormalized = true;
            e.GraphFormat.BackgroundColor = Color.Black;
        }*/

        public void HighlightGenre()
        {

            this.Graphviz.FormatVertex += (sender, args) => {
                foreach (Genre genre in this.GenreList)
                {
                    if (genre.Name == args.Vertex.ToString())
                    {
                        args.VertexFormatter.ToolTip = "Genre : " + genre.Name;
                        args.VertexFormatter.Shape = GraphvizVertexShape.Circle;
                        args.VertexFormatter.Style = GraphvizVertexStyle.Filled;
                        args.VertexFormatter.FillColor = Color.Red;
                        args.VertexFormatter.FontColor = Color.White;
                        args.VertexFormatter.StrokeColor = Color.Red;
                        args.VertexFormatter.FixedSize = true;
                        args.VertexFormatter.Size = new SizeF(2, 2);
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
                        args.VertexFormatter.ToolTip = "Artiste : " + artist.Name;
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
                            args.VertexFormatter.ToolTip = "Artiste mis en valeur : " + artist.Name;
                            args.VertexFormatter.Style = GraphvizVertexStyle.Filled;
                            args.VertexFormatter.FillColor = Color.Yellow;
                            args.VertexFormatter.FontColor = Color.Black;
                            args.VertexFormatter.StrokeColor = Color.Yellow;
                            args.VertexFormatter.FixedSize = true;
                            args.VertexFormatter.Size = new SizeF(2.5f, 2.5f);
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
            e.VertexFormatter.Label = e.Vertex.ToString().Replace("\"", "");
            e.VertexFormatter.Font = new Font("Gulim", 12);
            e.VertexFormatter.Comment = e.Vertex.ToString().Replace("\"", "");
        }

    }
}
