using System;
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

            Utilities.displayDetails(Utilities.author, Utilities.version, Utilities.update);
            _ = Utilities.question;

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


                    _ = Utilities.displayOptions;

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

                        case "?":
                            material = "?";
                            break;

                        default:
                            WriteLine(@"Choose between 1 et 6...");
                            break;
                    }


                    try
                    {
                        for (var i = 1; i <= selection.Count; i++)
                        {
                            // Loop through items selected in the active assembly.
                            var occ = (SolidEdgeAssembly.Occurrence)selection.Item(i);
                            Replace.Part(occ, material, Utilities.tablePath, Utilities.fastenersPath);
                        }
                    }
                    finally
                    {
                        WriteLine(@"Exit");
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