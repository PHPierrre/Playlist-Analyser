using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Playlist_Analyser
{
    public class Top
    {
        public List<Artist> ArtistList { private get;  set; } = new List<Artist>();
        public List<Genre> GenreList { private get; set; } = new List<Genre>();
        public List<Year> YearList { private get; set; } = new List<Year>();

        public Top(List<Artist> artistList, List<Genre> genreList, List<Year> yearList)
        {
            this.ArtistList = artistList;
            this.GenreList = genreList;
            this.YearList = YearList;
        }

        public void ShowGlobalStats(String path, List<Music> musicList)
        {
            Console.WriteLine("Répertoire : " + path);
            Console.WriteLine("Nombre de musiques : " + musicList.Count);
        }

        public void ShowTopArtists()
        {
            Console.WriteLine("\nTop 20 artistes " + this.ArtistList.Count + " (difs) \n");

            this.ArtistList.Sort(delegate (Artist a1, Artist a2) { return a2.Count.CompareTo(a1.Count); });
            int max = (this.ArtistList.Count > 20) ? 20 : this.ArtistList.Count;
            for (int i = 0; i < max; i++)
            {
                Console.WriteLine(this.ArtistList[i].Name + " : " + this.ArtistList[i].Count);
            }
        }

        public void ShowTopTags()
        {
            Console.WriteLine("\nTop genres " + this.GenreList.Count + " (difs) \n");

            this.GenreList.Sort(delegate (Genre g1, Genre g2) { return g2.Count.CompareTo(g1.Count); });
            for (int i = 0; i < this.GenreList.Count; i++)
            {
                Console.WriteLine(this.GenreList[i].Name + " : " + this.GenreList[i].Count);
            }
        }

        public void ShowTopYear()
        {
            Console.WriteLine("\nTop Années\n");
            this.YearList.Sort(delegate (Year y1, Year y2) { return y2.Count.CompareTo(y1.Count); });
            for (int i = 0; i < this.YearList.Count; i++)
            {
                Console.WriteLine(this.YearList[i].Name + " : " + this.YearList[i].Count);
            }
        }

    }
}
