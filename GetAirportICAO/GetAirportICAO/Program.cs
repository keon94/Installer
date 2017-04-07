using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;


namespace GetAirportICAO
{
    class Program
    {
        static void Main(string[] args)
        {

            try
            {
                string baseDir = "I:\\Games\\Flight Simulation\\FSX\\Mesh\\FS Global Ultimate\\FS Global Aerodrome Flattening Meshes\\AFM_FSX";
                String[] allfiles = Directory.GetFiles(baseDir, "*.*", System.IO.SearchOption.AllDirectories);
                for (int i = 0; i < allfiles.Length; ++i)
                {
                    allfiles[i] = Path.GetFileName(allfiles[i]).Replace("DX_", "").Replace(".bgl", "");
                }
                Array.Sort(allfiles);
                File.WriteAllLines(Directory.GetCurrentDirectory() + "\\Airports.txt", allfiles, System.Text.Encoding.ASCII);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

        }
    }
}
