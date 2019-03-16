using System;
using System.IO;
using Playlist_Analyser.Graph;

namespace Playlist_Analyser
{
    class Program
    {

        static void Main(string[] args)
        {
            string[] argTwoAccepted = { "png", "svg", "ps" };
            string[] argThreeAccepted = {"genre", "artist", "music"};

            if (args.Length == 3)
            {
                if(!Directory.Exists(args[0])) {
                    Console.WriteLine("Argument 1 : Your directory does not exist");
                    Console.ReadKey();
                    return;
                }

                if (Array.IndexOf(argTwoAccepted, args[1]) == -1)
                {
                    Console.WriteLine("Argument 2 : ps|png|svg");
                    Console.ReadKey();
                    return;
                }
                
                if (Array.IndexOf(argThreeAccepted, args[2]) == -1){
                    Console.WriteLine("Argument 3 : Invalide : genre|artist|music");
                    Console.WriteLine("music is applied ...");
                }

                IndexationProcessus indexation = new IndexationProcessus(args[0]);
                DetailledGraph deteilledGraph = new DetailledGraph(
                    indexation.Musics,
                    indexation.Artists,
                    indexation.Genres,
                    args[1]
                );

                if (args[2].Equals("genre"))
                    deteilledGraph.GenerateGenreGraph();
                else if (args[2].Equals("artist"))
                    deteilledGraph.GenerateArtistGraph();
                else
                    deteilledGraph.GenerateMusicGraph();

                /*SimpleGraph simpleGraph = new SimpleGraph(
                    indexation.Musics,
                    indexation.Artists,
                    indexation.Genres,
                    args[1]
                );

                simpleGraph.GenerateMusicGraph(); */

                Console.ReadKey();
            }
            else
            {
                Console.WriteLine("Info : playlist.exe [path to music folder] [svg|png|ps] [genre|artist|music:default]");
            }
        }
    }
}
