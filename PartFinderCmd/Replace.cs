using System.IO;
using LoadPartsFromTeamcenter;
using static System.Console;
using Path = System.IO.Path;
using Tools;

namespace PartReplacer
{
    internal class Replace
    {
        public static void Part(SolidEdgeAssembly.Occurrence occ, string material)
        {

            WriteLine("...");

            var partFullName = occ.OccurrenceFileName;

            var cacheDirectory = Path.GetDirectoryName(partFullName) + Path.DirectorySeparatorChar;

            var jdeOccurrence = Cache.GetJde(partFullName);

            var replacement = TableConversion.getEquivalentByTypeMaterial(jdeOccurrence, material);

            if (material == "?") return;  // No conversion, the user just wants check the values in table.


            // Get details from jde number.
            var part = Tools.Fasteners.getReplacementPartDetails(replacement);

            var jde = part.Item1;
            var revision = part.Item2;
            var filename = part.Item3;


            if (jde != "" && jde != jdeOccurrence)
            {
                // Load new part in Solid edge cache.
                Tc.LoadPartToCache(jde, revision, filename, cacheDirectory);

                // Replace selected part with new part.
                var newPart = Path.Combine(cacheDirectory, filename);
                if (File.Exists(newPart))
                {
                    occ.Replace(newPart, true);
                    WriteLine(@"[+] Replaced: {0} -> {1}", jdeOccurrence, replacement);
                }
                else
                {
                    WriteLine($@"[!] Replacement part was not loaded in your cache, check if you are connected to Teamcenter.");
                }
            }
            else
            {
                WriteLine($@"[-] Replacement not performed ({jde})->(=)");
            }

            WriteLine("---");
        }
    }
}
