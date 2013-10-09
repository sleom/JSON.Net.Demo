using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace JSON.Net.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            var json = "{\"S1\":null,\"S2\":\"s2\",\"I1\":1}";

            var o3 = JsonConvert.DeserializeObject<MyClass>(json, new EmptyStringToNullConverter());

            System.Console.ReadLine();
        }

        class MyClass
        {
            public string S1 { get; set; }
            public string S2 { get; set; }
            public int I1 { get; set; }
        }

        public class EmptyStringToNullConverter : JsonConverter
        {
            #region Overrides of JsonConverter

            /// <summary>
            /// Writes the JSON representation of the object.
            /// </summary>
            /// <param name="writer">The <see cref="T:Newtonsoft.Json.JsonWriter"/> to write to.</param><param name="value">The value.</param><param name="serializer">The calling serializer.</param>
            public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
            {
                writer.WriteValue(value);
            }

            /// <summary>
            /// Reads the JSON representation of the object.
            /// </summary>
            /// <param name="reader">The <see cref="T:Newtonsoft.Json.JsonReader"/> to read from.</param><param name="objectType">Type of the object.</param><param name="existingValue">The existing value of object being read.</param><param name="serializer">The calling serializer.</param>
            /// <returns>
            /// The object value.
            /// </returns>
            public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
            {
                switch (reader.TokenType)
                {
                    case JsonToken.Null:
                        return null;
                    case JsonToken.String:
                    {
                        var val = (string)reader.Value;
                        if(string.IsNullOrEmpty(val)) val = null;
                        return val;
                    }
                }
                throw new JsonReaderException(string.Format("Unexcepted token {0}", reader.TokenType));
            }

            /// <summary>
            /// Determines whether this instance can convert the specified object type.
            /// </summary>
            /// <param name="objectType">Type of the object.</param>
            /// <returns>
            /// <c>true</c> if this instance can convert the specified object type; otherwise, <c>false</c>.
            /// </returns>
            public override bool CanConvert(Type objectType)
            {
                return objectType == typeof(string);
            }

            #endregion
        }
    }
}
