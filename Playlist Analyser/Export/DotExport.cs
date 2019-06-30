using QuickGraph;
using QuickGraph.Graphviz;
using System;

namespace Playlist_Analyser.Export
{
    public class DotExport
    {
        public static void Export(GraphvizAlgorithm<String, Edge<String>> graphviz, String fileName)
        {
            String output = graphviz.Generate(new FileDotEngine(), fileName);
            Console.WriteLine("Generated file " + Environment.CurrentDirectory + "\\" + output);
        }
    }
}
