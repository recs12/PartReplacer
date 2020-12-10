using System;
using static System.Console;
using Tools;

namespace PartReplacer
{
    internal class Program

    {
        [STAThread]
        private static void Main()
        {

            User.displayDetails(Details.author, Details.version, Details.update);

            var resp = ReadLine()?.ToLower();
            if (resp != "y")
            {
                WriteLine(@"You have exit the application.");
                ReadKey();
            }
            else
            {
                // Connection to Solid edge session.
                var application = SolidEdgeCommunity.SolidEdgeUtils.Connect(true, true);
                var assemblyDocument = (SolidEdgeAssembly.AssemblyDocument)application.ActiveDocument;

                // Get all elements selected by user.
                var selection = assemblyDocument.SelectSet;

                // check if any selection in solidedge.
                Quantity.displaySelectionCount(selection.Count);
                if (selection.Count != 0)
                {

                    _ = Utilities.displayOptions;

                    var material = Switcher.getUserChoice;

                    if (material == "")
                    {
                        _ = Pick.WrongPick;
                        ReadKey();
                    }
                    else
                    {
                        try
                        {
                            for (var i = 1; i <= selection.Count; i++)
                            {
                                try
                                {
                                    // Loop through items selected in the active assembly.
                                    var occ = (SolidEdgeAssembly.Occurrence)selection.Item(i);
                                    Replace.Part(occ, material);
                                }
                                catch (InvalidCastException)
                                {
                                    _ = Level.WrongLevel;
                                }
                            }
                        }
                        finally
                        {
                            WriteLine(@"Exit 0");
                            ReadKey();
                        }

                    }
                }
                else
                {
                    _ = Selection.WrongSelection;
                    ReadKey();
                }
            }

        }
    }
}