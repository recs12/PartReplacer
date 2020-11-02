using System;
using System.IO;
using LoadPartsFromTeamcenter;
using static System.Console;

// TODO:
// 2. anticipate  that method GetDetails return a part empty and skip.
// 3. implement if the bolt is not in the database.
// 4. reduce the list of selected item and remove duplicate... (all same part are supposed to be replace anyway.)
// 5. display values of dictionary as information for user...
// 6. Check if no multi session of TC opened in task manager...

namespace PartReplacer
{
    class Program
    {
        [STAThread]
        static void Main()
        {
            string __version__ = "0.0.0";
            string __author__ = "recs";
            string __update__ = "2020-10-09";

            // Json files where is the information for conversion.
            string table = @"J:\PTCR\Users\RECS\Macros\Replacer\dataFasteners\table.json";
            string fasteners = @"J:\PTCR\Users\RECS\Macros\Replacer\dataFasteners\fasteners.json";


            WriteLine(
                $@"PartReplacer  --author: {__author__} --version: {__version__} --last-update :{__update__} ");
            WriteLine(@"Replace the fasteners in the assembly, press y/[Y] to proceed:");

            string resp = ReadLine()?.ToLower();
            string answerYes = "y";
            if (resp != answerYes)
            {
                WriteLine(@"You have exit the application.");
                ReadKey();
            }
            else
            {

                // Connection to Solid edge session.
                SolidEdgeFramework.Application application = SolidEdgeCommunity.SolidEdgeUtils.Connect(true, true);
                SolidEdgeAssembly.AssemblyDocument assemblyDocument = (SolidEdgeAssembly.AssemblyDocument)application.ActiveDocument;

                // selection set
                SolidEdgeFramework.SelectSet selection = assemblyDocument.SelectSet;

                // check if any selection in solidedge.
                if (selection.Count != 0)
                {

                    Console.WriteLine(selection.Item(1));
                    Console.WriteLine(selection.Item(2));

                    // Command line.
                    WriteLine(@"");
                    WriteLine(@"	1) imperial zinc");
                    WriteLine(@"	2) metric zinc");
                    WriteLine(@"	3) imperial ss-304");
                    WriteLine(@"	4) metric ss-304");
                    WriteLine(@"	5) imperial ss-316");
                    WriteLine(@"	6) metric ss-316");
                    WriteLine("");
                    WriteLine(@" Select material by pressing those keys [1,2,3,4,5,6]... ");

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


                    WriteLine($@"--Selected items: {selection.Count}");



                    for (int i = 1; i <= selection.Count; i++)
                    {
                        WriteLine(i);
                        // Loop through items selected in the active assembly.
                        var occ = (SolidEdgeAssembly.Occurrence)selection.Item(i);
                        string partFullName = occ.OccurrenceFileName;

                        string cacheDirectory = Path.GetDirectoryName(partFullName) + Path.DirectorySeparatorChar;

                        //Find the part equivalent with the required material in <table.json>.
                        var jdeOccurrence = Cache.GetJde(partFullName);

                        var jdeReplacement = Convertor.Convertor.GetConversionFor(jdeOccurrence, material, table);

                        // Get details from jde number.
                        Convertor.Convertor.CadPart part = Convertor.Convertor.GetDetails(jdeReplacement, fasteners);


                        if (jdeOccurrence != jdeReplacement && jdeReplacement != null) // review this condition and assure that the part is not null.
                        {
                            // Load new part in Solid edge cache.
                            AccessTc.LoadPartToCache(part, cacheDirectory);

                            // Replace selected part with new part.
                            string newPart = Path.Combine(cacheDirectory, part.Filename);
                            occ.Replace(newPart, true);

                            // to-do: find a way to display the other options of material with a table.
                            WriteLine(@"Replace {0} by {1}", jdeOccurrence, jdeReplacement);
                        }
                        else
                        {
                            WriteLine(@"Replacement was not performed");
                        }

                    }


                }
                else
                {
                    WriteLine(@"No active selection, select items you want to replace then run the macro again.");
                    ReadKey();
                }
            }
        }
    }
}


