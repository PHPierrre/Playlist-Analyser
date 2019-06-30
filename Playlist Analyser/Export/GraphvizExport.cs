using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Playlist_Analyser.Export
{
    public class GraphvizExport
    {
        public static void Export(String cmd, String fileName, String extension)
        {
            String command = cmd + " " + fileName + ".dot -o " + fileName + "." + extension + " -T" + extension;
            Console.WriteLine(command);
            Process p = Process.Start("cmd.exe", command);
            p.WaitForExit();
            Console.WriteLine("Generated file : " + Environment.CurrentDirectory + "\\" + fileName + "." + extension);

            //Process.Start(@"C:\Program Files (x86)\Naver\Naver Whale\Application\whale.exe", "DetailledMusicGraph.dot");
        }
    }
}
