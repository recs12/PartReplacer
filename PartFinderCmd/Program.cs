using LoadPartsFromTeamcenter;
using System;
using SolidEdgeFramework;
using System.IO;
using static System.Console;

// TODO:
// 3. implement if the bolt is not in the database.
// 4. reduce the list of selected item and remove duplicate... (all same part are supposed to be replace anyway.)
// 5. display values of dictionary as information for user...
// 6. Check if no multi session of TC opened in task manager...
// 7. Check if part subassembly
// 8. Check if part ReferenceOnly

namespace PartReplacer
{
    internal class Program
    {
        [STAThread]
        private static void Main()
        {
            const string version = "0.0.0";
            const string author = "recs";
            const string update = "2020-10-09";

            // Json files where is the information for conversion.
            const string table = @"J:\PTCR\Users\RECS\Macros\Replacer\dataFasteners\table.json";
            const string fasteners = @"J:\PTCR\Users\RECS\Macros\Replacer\dataFasteners\fasteners.json";

            WriteLine(
                $@"PartReplacer  --author: {author} --version: {version} --last-update :{update} ");
            WriteLine(@"Replace the fasteners in the assembly, press y/[Y] to proceed:");

            var resp = ReadLine()?.ToLower();
            const string answerYes = "y";
            if (resp != answerYes)
            {
                WriteLine(@"You have exit the application.");
                ReadKey();
            }
            else
            {
                // Connection to Solid edge session.
                var application = SolidEdgeCommunity.SolidEdgeUtils.Connect(true, true);
                var assemblyDocument = (SolidEdgeAssembly.AssemblyDocument)application.ActiveDocument;

                // selection set
                var selection = assemblyDocument.SelectSet;

                // check if any selection in solidedge.
                WriteLine($@"Quantity item selected: {selection.Count}");
                if (selection.Count != 0)
                {

                    // Command line.
                    WriteLine(@"");
                    WriteLine(@"1) imperial zinc");
                    WriteLine(@"2) metric zinc");
                    WriteLine(@"3) imperial ss-304");
                    WriteLine(@"4) metric ss-304");
                    WriteLine(@"5) imperial ss-316");
                    WriteLine(@"6) metric ss-316");
                    WriteLine(@"");
                    WriteLine(@"Select material by pressing keys [1,2,3,4,5,6]");

                    var materialChoice = ReadLine();

                    string material = null;
                    switch (materialChoice)
                    {
                        case "1":
                            material = "imperial zinc";
                            break;

                        case "2":
                            material = "metric zinc";
                            break;

                        case "3":
                            material = "imperial ss-304";
                            break;

                        case "4":
                            material = "metric ss-304";
                            break;

                        case "5":
                            material = "imperial ss-316";
                            break;

                        case "6":
                            material = "metric ss-316";
                            break;

                        default:
                            WriteLine(@"Choose between 1 et 6..."); // TODO: how to stop exit app if no selection made?
                            break;
                    }


                    try
                    {
                        for (var i = 1; i <= selection.Count; i++)
                        {
                            // Loop through items selected in the active assembly.
                            SolidEdgeAssembly.Occurrence occ = (SolidEdgeAssembly.Occurrence)selection.Item(i);

                            if (occ.Subassembly == false)
                            {
                                var partFullName = occ.OccurrenceFileName;

                                var cacheDirectory = Path.GetDirectoryName(partFullName) + Path.DirectorySeparatorChar;

                                //Find the part equivalent with the required material in <table.json>.
                                var jdeOccurrence = Cache.GetJde(partFullName);

                                var jdeReplacement = Convertor.Convertor.GetConversionFromTable(jdeOccurrence, material, table);

                                // Get details from jde number.
                                var part = Convertor.Convertor.GetDatasetDetailsForPart(jdeReplacement, fasteners);

                                if (part.Jde != null && part.Jde != jdeOccurrence) // review this condition and assure that the part is not null.
                                {
                                    // Load new part in Solid edge cache.
                                    AccessTc.LoadPartToCache(part, cacheDirectory);
                                    Console.WriteLine($@"{part.Jde} loaded in your cache");

                                    // Replace selected part with new part.
                                    var newPart = Path.Combine(cacheDirectory, part.Filename);
                                    occ.Replace(newPart, true);

                                    // to-do: find a way to display the other options of material with a table.
                                    WriteLine(@"Replaced: {0} -> {1}", jdeOccurrence, jdeReplacement);
                                    Console.WriteLine("---");
                                }
                                else
                                {
                                    WriteLine($@"Replacement not performed ({part.Jde})->(=)");
                                }
                            }
                            else
                            {
                                WriteLine(@"Replacement not performed (.)->() (SubAssembly)");
                            }
                        }
                    }
                    finally
                    {
                        WriteLine(@"exit");
                        ReadKey();
                    }
                }
                else
                {
                    WriteLine(@"No active selection: Select some items before running the macro again.");
                    ReadKey();
                }
            }

        }
    }
}