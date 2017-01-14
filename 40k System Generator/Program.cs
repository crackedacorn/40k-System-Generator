using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WH40K_System_Generator
{
    internal enum KeyFeatureType { Bountiful = 0, [Description("Gravity Tides")] GravityTides, Haven,
        [Description("Ill-Omened")] IllOmened, [Description("Pirate Den")] PirateDen, [Description("Ruined Empire")] RuinedEmpire,
        Starfarers, [Description("Stellar Anomaly")] StellarAnomaly, [Description("Warp Stasis")] WarpStasis,
        [Description("Warp Turbulence")] WarpTurbulence };
    public enum StarType { Mighty = 0, Vigorous, Luminous, Dull, Anomalous, Binary };

    class Program
    {
        static void Main(string[] args)
        {
            for (; ; )
            {
                StarSystem ss = new StarSystem();

                Console.WriteLine(ss.ToString());

                Console.WriteLine("=================================================");
                Console.WriteLine("Ctrl-c to exit, or ENTER to generate a new system");
                Console.WriteLine("=================================================\n");
                Console.Read();
            }
        }
    }

    class RNG
    {
        public static int RandNumber(int Low, int High)
        {
            Random rndNum = new Random(int.Parse(Guid.NewGuid().ToString().Substring(0, 8), System.Globalization.NumberStyles.HexNumber));

            int rnd = rndNum.Next(Low, High);

            return rnd;
        }
    }

}
