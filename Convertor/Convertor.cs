using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace Convertor
{

    public class Convertor
    {
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

                    foreach (KeyValuePair<string, string> entry in conversionCollection)
                    {
                        //do something with entry.Value or entry.Key
                        Console.WriteLine(entry.Key);
                        Console.WriteLine(entry.Value);
                    }

                    return conversionCollection.ContainsKey(material) ? conversionCollection[material] : null;

                }
                else
                {
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

            return new CadPart();

        }
    }
}


