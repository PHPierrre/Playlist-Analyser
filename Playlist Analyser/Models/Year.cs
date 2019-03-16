using Playlist_Analyser.Models;
using System;

namespace Playlist_Analyser
{
    public class Year : ICount
    {
        public String Name { get; set; }
        public int Count { get; set; }

        public Year(uint year)
        {
            this.Name = year + "";
            this.Count = 1;
        }
    }
}
