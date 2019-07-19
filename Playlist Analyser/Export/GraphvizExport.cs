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
        private static Boolean detectIfGraphvizIsInstalled()
        {
            String command = "/C neato -V";

            Process p = new Process();
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.RedirectStandardError = true;
            p.StartInfo.FileName = "cmd.exe";
            p.StartInfo.Arguments = command;
            p.Start();
            p.WaitForExit();

            if (p.ExitCode != 0)
            {
                Console.WriteLine("Graphviz n'est soit non installe, soit non ajoute au path de windows.");
                return false;
            }

            return true;
        }

        public static void Export(String cmd, String fileName, String extension)
        {
            String path, command;

            if (!detectIfGraphvizIsInstalled())
            {
                return;
            }

            command = cmd + " " + fileName + ".dot -o " + fileName + "." + extension + " -T" + extension;
            Console.WriteLine(command);
            Process p = Process.Start("cmd.exe", command);
            p.WaitForExit();

            path = Environment.CurrentDirectory + "\\" + fileName + "." + extension;
            Console.WriteLine("Generated file : " + path);

            Process p1 = new Process();
            p1.StartInfo.FileName = @"C:\Program Files (x86)\Naver\Naver Whale\Application\whale.exe";
            p1.StartInfo.Arguments = "\"" + path + "\"";
            p1.Start();
        }
    }
}
