using System;
using static System.Console;
using Helpers;

// 6. Check wrong password


namespace PartReplacer
{
    internal class Program

    {
        [STAThread]
        private static void Main()
        {

            User.displayDetails(User.author, User.version, User.update);

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

                    var material = Switcher.getUserChoice;

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
                                WriteLine(@"[!] Item not in the current assembly level.");
                            }
                        }
                    }
                    finally
                    {
                        WriteLine(@"Exit 0");
                        ReadKey();
                    }
                }
                else
                {
                    WriteLine(@"[!] No active selection: Select some items before running the macro again.");
                    ReadKey();
                }
            }

        }
    }
}