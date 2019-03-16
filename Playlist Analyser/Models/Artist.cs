using Playlist_Analyser.Models;
using System;
using TagLib;

namespace Playlist_Analyser
{
    public class Artist :ICount
    {
        public String Name { get; set;}
        public int Count { get; set; }
        public IPicture[] Pictures { get; set; }

        public Artist(String name)
        {
            this.Name = name;
            this.Count = 1;
        }
    }
}
