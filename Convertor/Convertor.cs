using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Convertor
{
    public class MyConverter : CustomCreationConverter<IDictionary<string, Dictionary<string, string>>>
    {
        public override IDictionary<string, Dictionary<string, string>> Create(Type objectType)
        {
            return new Dictionary<string, Dictionary<string, string>>();
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(Dictionary<string, string>) || base.CanConvert(objectType);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue,
            JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.StartObject || reader.TokenType == JsonToken.Null)
                return base.ReadJson(reader, objectType, existingValue, serializer);
            return serializer.Deserialize(reader);
        }
    }


    public class Convertor
    {
        // return jde number equivalent to the input jde for the material provided to the function.
        public static string GetConversionFor(
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

        public struct CadPart
        {
            public string Jde;
            public string Revision;
            public string Filename;
        }


        public static CadPart GetDetails(
            string jdeNumber,
            string fastenerFilePath
        )
        {
            var readAllText = File.ReadAllText(fastenerFilePath);
            var listing = JsonConvert.DeserializeObject<IDictionary<string, object>>(
                readAllText, new JsonConverter[] { new MyConverter() }
            );

            // ReSharper disable once InvertIf
            if (listing != null && listing.ContainsKey(jdeNumber) )
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


