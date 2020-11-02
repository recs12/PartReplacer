using System;
using System.Collections.Generic;
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
    public struct CadPart
    {
        public string Jde;
        public string Revision;
        public string Filename;
    }
}
