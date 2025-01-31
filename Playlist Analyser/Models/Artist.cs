﻿using Playlist_Analyser.Models;
using System;
using System.Collections.Generic;
using TagLib;

namespace Playlist_Analyser
{
    public class Artist : ICount
    {
        public String Name { get; set;}
        public int Count { get; set; }
        public IPicture[] Pictures { get; set; }
        public readonly IDictionary<String, Genre> genres;

        public Artist(String name)
        {
            this.Name = name;
            this.Count = 1;
            this.genres = new Dictionary<String, Genre>();
        }
    }
}
