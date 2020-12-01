using System;
using System.IO;
using LoadPartsFromTeamcenter;
using static System.Console;
using Path = System.IO.Path;

namespace PartReplacer
{
    internal class Replace
    {
        public static void Part(SolidEdgeAssembly.Occurrence occ, string material)
        {


            var partFullName = occ.OccurrenceFileName;

            var cacheDirectory = Path.GetDirectoryName(partFullName) + Path.DirectorySeparatorChar;

            //Find the part equivalent with the required material in <table.json>.
            var jdeOccurrence = Cache.GetJde(partFullName);

            var replacement = Helpers.TableConversion.getEquivalentByTypeMaterial(jdeOccurrence, material);

            //var replacement = tuple.Item1;
            //var table = tuple.Item2;

            //Helpers.Chart.displayChart(replacement, table);

            if (material == "?") return;  // No conversion, the user just wants check the values in table.

            // Get details from jde number.
            var part = Helpers.Fasteners.getReplacementPartDetails(replacement);

            var jde = part.Item1;
            var revision = part.Item2;
            var filename = part.Item3;


            if (jde != null && jde != jdeOccurrence) // review this condition and assure that the part is not null.
            {
                // Load new part in Solid edge cache.
                AccessTc.LoadPartToCache(jde, revision, filename, cacheDirectory);

                // Replace selected part with new part.
                var newPart = Path.Combine(cacheDirectory, filename);
                if (File.Exists(newPart))
                {
                    occ.Replace(newPart, true);
                    WriteLine(@"[+] Replaced: {0} -> {1}", jdeOccurrence, replacement); //not a good place
                }
                else
                {
                    Console.WriteLine($@"[!] Replacement part was not loaded in your cache, check if you are connected to Teamcenter.");
                }
            }
            else
            {
                WriteLine($@"[-] Replacement not performed ({jde})->(=)");
            }

            Console.WriteLine(@"---");
        }
    }
}
