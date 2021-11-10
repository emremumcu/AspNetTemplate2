using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetTemplate2.AppLib.Concrete
{
    public static class Tools
    {
        public static string JsonSerialize<T>(this T obj)
        {
            return JsonConvert.SerializeObject(obj, Formatting.Indented);
        }

        public static void JsonSerializeToFile<T>(this T obj, string path)
        {
            JsonSerializer serializer = new JsonSerializer();
            serializer.Converters.Add(new JavaScriptDateTimeConverter());
            serializer.NullValueHandling = NullValueHandling.Include;
            serializer.Formatting = Formatting.Indented;

            using (StreamWriter sw = new StreamWriter(path))
            using (JsonWriter writer = new JsonTextWriter(sw))
            {
                serializer.Serialize(writer, obj);
            }
        }

        public static T JsonDeserialize<T>(this string json)
        {
            return JsonConvert.DeserializeObject<T>(json);
        }

        public static T JsonDeserializeFromFile<T>(this string path)
        {
            // read file into a string and deserialize JSON to a type
            // T obj = JsonConvert.DeserializeObject<T>(File.ReadAllText(path));

            // deserialize JSON directly from a file
            using (StreamReader file = File.OpenText(path))
            {
                JsonSerializer serializer = new JsonSerializer();
                T obj = (T)serializer.Deserialize(file, typeof(T));
                return obj;
            }
        }


    }
}
