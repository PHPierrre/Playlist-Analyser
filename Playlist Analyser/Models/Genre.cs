using Playlist_Analyser.Models;
using System;

namespace Playlist_Analyser
{
    public class Genre : ICount
    {
        public String Name { get; set; }
        public int Count { get; set; }

        public Genre(String name)
        {
            this.Name = name;
            this.Count = 1;
        }
    }
}
