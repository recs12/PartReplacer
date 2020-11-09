using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace Convertor
{
    public class Convertor
    {
        private const string Format = "{0,9}|{1,-2}{2,-20} -> {3,-10}";

        // return jde number equivalent to the input jde for the material provided to the function.
        public static string GetConversionFromTable(
            string jdeNumber,
            string material,
            string tableFilePath
        )
        {
            try
            {
                var json = File.ReadAllText(tableFilePath); // json table query

                var table = JsonConvert.DeserializeObject<IDictionary<string, object>>(
                    json, new JsonConverter[] { new MyConverter() }
                );

                if (table != null && table.ContainsKey(jdeNumber))
                {
                    var subTable = table[jdeNumber];
                    var conversionCollection =
                        JsonConvert.DeserializeObject<Dictionary<string, string>>(subTable.ToString());

                    Console.WriteLine("...");
                    Console.WriteLine("match {0} with", jdeNumber);
                    var i = 0;
                    foreach (KeyValuePair<string, string> entry in conversionCollection)
                    {
                        i++;
                        Console.WriteLine(string.Format(Format,
                                                        i,
                                                        entry.Key == material ? ">" : " ",
                                                        entry.Key,
                                                        entry.Value != jdeNumber ? entry.Value : "(=)"));
                    }

                    return conversionCollection.ContainsKey(material) ? conversionCollection[material] : null;
                }
                else
                {
                    Console.WriteLine($@"MISSING: the conversion dataset file: {table} \n" +
                        $@"does not contain {jdeNumber}\n" +
                        "To fix it, you can edit and add the missing data in the file table.json"); // add a sample of what to add
                    return null;
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static CadPart GetDatasetDetailsForPart(
            string jdeNumber,
            string fastenerFilePath
        )
        {
            var readAllText = File.ReadAllText(fastenerFilePath);
            var listing = JsonConvert.DeserializeObject<IDictionary<string, object>>(
                readAllText, new JsonConverter[] { new MyConverter() }
            );

            // ReSharper disable once InvertIf
            if (listing != null && listing.ContainsKey(jdeNumber))
            {
                var n = listing[jdeNumber];
                var chair = n.ToString();

                var item = JsonConvert.DeserializeObject<Dictionary<string, string>>(chair);

                // Create a part to return with the details attached to it.
                return new CadPart
                {
                    // attributes of the part
                    Jde = item["JdeNumber"],
                    Revision = item["Revision"],
                    Filename = item["Filename"]
                };
            }
            else
            {
                Console.WriteLine($@"MISSING: the fasteners dataset file: {fastenerFilePath} \n" +
                    $@"does not contain {jdeNumber}\n" +
                    "To fix it, you can edit and add the missing data in the file fasteners.json \n" // add a sample of what to add
                 );
                return new CadPart();
            }
        }
    }
}