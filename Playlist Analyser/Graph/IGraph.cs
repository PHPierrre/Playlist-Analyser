using QuickGraph.Graphviz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Playlist_Analyser.Graph
{
    public interface IGraph
    {
        void GenerateGenreGraph();
        void GenerateArtistGraph();
        void GenerateMusicGraph();

        void HighlightGenre();
        void HighlightArtist();
        void HighlightTargetedArtist();

    }
}
