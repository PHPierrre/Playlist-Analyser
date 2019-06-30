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
            string[] argFourAccepted = { "simple", "detailled", "weighted" };

            if (args.Length == 4)
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

                if (Array.IndexOf(argFourAccepted, args[3]) == -1)
                {
                    Console.WriteLine("Argument 3 : Invalide : simple|detailled|weighted");
                    Console.ReadKey();
                    return;
                }

                IndexationProcessus indexation = new IndexationProcessus(args[0]);
                IGraph graph = null;

                if(args[3] == "simple")
                {
                    graph = new SimpleGraph(indexation.Musics, indexation.Artists, indexation.Genres, args[1]);
                }
                else if (args[3] == "detailled")
                {
                    graph = new DetailledGraph(indexation.Musics, indexation.Artists, indexation.Genres, args[1]);
                }
                else if (args[3] == "weighted")
                {
                    graph = new WeightGraph(indexation.Musics, indexation.Artists, indexation.Genres, args[1]);
                }

                if (args[2].Equals("genre"))
                    graph.GenerateGenreGraph();
                else if (args[2].Equals("artist"))
                    graph.GenerateArtistGraph();
                else
                    graph.GenerateMusicGraph();

                Top top = new Top(indexation.Artists, indexation.Genres, indexation.Years);
                top.ShowGlobalStats(args[0], indexation.Musics);
                top.ShowTopArtists();
                top.ShowTopTags();
                top.ShowTopYear();

                Console.ReadKey();
            }
            else
            {
                Console.WriteLine("Info : playlist.exe [path to music folder] [svg|png|ps] [genre|artist|music:default]");
            }

        }
    }
}
