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
            string material
        )
        {
            try
            {
                var tableFilePath = @"J:\PTCR\Users\RECS\Macros\ReplacerFasteners\dataFastenersJson\table.json";
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
                        Console.WriteLine(Format, i, entry.Key == material ? ">" : " ", entry.Key, entry.Value != jdeNumber ? entry.Value : "(=)");
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


        }
    }
