using System;
using TagLib;

namespace Playlist_Analyser
{
    public class Music
    {
        public String FilePath { get; set; }
        public String FileName { get; set; }
        public String Title { get; set; }
        public String Artist { get; set; }
        public String Album { get; set; }
        public String AlbumArtist { get; set; }
        public String Comment { get; set; }

        public uint Year { get; set; }
        public int Piste { get; set; }
        public String Genre { get; set; }

        public IPicture[] Pictures { get; set; }
    }
}
