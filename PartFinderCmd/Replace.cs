using System;
using LoadPartsFromTeamcenter;
using static System.Console;
using Path = System.IO.Path;

namespace PartReplacer
{
    class Replace
    {
        public static void Part(SolidEdgeAssembly.Occurrence occ, string material, string table, string fasteners)
        {

            if (occ.Subassembly == false)
            {

                var partFullName = occ.OccurrenceFileName;

                var cacheDirectory = Path.GetDirectoryName(partFullName) + Path.DirectorySeparatorChar;

                //Find the part equivalent with the required material in <table.json>.
                var jdeOccurrence = Cache.GetJde(partFullName);

                var jdeReplacement = Convertor.Convertor.GetConversionFromTable(jdeOccurrence, material, table);

                if (material == "?") return;  // No conversion, the user just check the values in table.

                // Get details from jde number.
                var part = Convertor.Convertor.GetDatasetDetailsForPart(jdeReplacement, fasteners);

                var jde = part.Item1;
                var revision = part.Item2;
                var filename = part.Item3;


                if (jde != null && jde != jdeOccurrence) // review this condition and assure that the part is not null.
                {
                    // Load new part in Solid edge cache.

                    if ((bool)AccessTc.GetUserTcMode())
                    {
                        AccessTc.LoadPartToCache(jde, revision, filename, cacheDirectory);

                        // Replace selected part with new part.
                        var newPart = Path.Combine(cacheDirectory, filename);
                        occ.Replace(newPart, true);
                        WriteLine(@"Replaced: {0} -> {1}", jdeOccurrence, jdeReplacement);
                    }
                    else
                    {
                        Console.WriteLine(@"[!] Unable to load the part because you are not connected to Teamcenter.");
                        Console.WriteLine(@"Connect to teamcenter before running this macro.");
                    }
                    Console.WriteLine(@"---");
                }
                else
                {
                    WriteLine($@"Replacement not performed ({jde})->(=)");
                }
            }
            else
            {
                WriteLine(@"Replacement not performed (.)->() (SubAssembly)");
            }
        }
    }
}
