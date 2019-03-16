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

            String output = GenerateDotFile("simpleGenreGraph");
            String filename = "simpleGenreGraph." + this.ExportFormat;

            String command = "/C circo " + output + " -o " + filename + " -T" + this.ExportFormat;
            Process p = Process.Start("cmd.exe", command);
            p.WaitForExit();
            Console.WriteLine("Generated file : " + filename);
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

            String output = GenerateDotFile("simpleArtistGraph");
            String filename = "simpleArtistGraph." + this.ExportFormat;

            String command = "/C sfdp -Goverlap=prism " + output + " -o " + filename + " -T" + this.ExportFormat;
            Process p = Process.Start("cmd.exe", command);
            p.WaitForExit();
            Console.WriteLine("Generated file : " + filename);
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

            String output = GenerateDotFile("simpleMusicGraph");
            String filename = "simpleMusicGraph." + this.ExportFormat;

            String command = "/C sfdp -Goverlap=prism " + output + " -o " + filename + " -T" + this.ExportFormat;
            Process p = Process.Start("cmd.exe", command);
            p.WaitForExit();
            Console.WriteLine("Generated file : " + filename);
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


        private String GenerateDotFile(String fileName)
        {
            String output = this.Graphviz.Generate(new FileDotEngine(), fileName);
            Console.WriteLine("Generated file " + Environment.CurrentDirectory + "\\" + output);

            return output;
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
