using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Planetbase;

namespace StorageGuru {

    public class SerializeHelper {

        private const string moduleSeperator = ":";
        private const string resourceWrapperL = "{";
        private const string resourceWrapperR = "}";
        private const char resourceDelim = ',';
        private const string targetNamespace = "Planetbase.";
        private const string targetAssembly = ", Assembly-CSharp";

        public string SerializeManifest(Dictionary<Module, HashSet<Type>> manifest) {
            StringBuilder stringBuilder = new StringBuilder();
            foreach (KeyValuePair<Module, HashSet<Type>> item in manifest) {
                stringBuilder.Append(item.Key.getId());
                stringBuilder.Append(":{");
                int num = 0;
                foreach (Type item2 in item.Value) {
                    stringBuilder.Append(item2.ToString().Replace("Planetbase.", ""));
                    num++;
                    stringBuilder.Append((num == item.Value.Count) ? "" : ','.ToString());
                }
                stringBuilder.Append("}");
                stringBuilder.Append(Environment.NewLine);
            }
            return stringBuilder.ToString();
        }

        public Dictionary<int, List<string>> DeserializeManifest(string[] contents) {
            Dictionary<int, List<string>> dictionary = new Dictionary<int, List<string>>();
            foreach (string text in contents) {
                if (!string.IsNullOrEmpty(text)) {
                    int key = int.Parse(text.Remove(text.IndexOf(":")));
                    List<string> source = text.Substring(text.IndexOf(":") + 1).Replace("{", "").Replace("}", "")
                        .Split(',')
                        .ToList();
                    source = source.Where((string x) => !string.IsNullOrEmpty(x)).ToList();
                    source = source.Select((string x) => "Planetbase." + x.Trim() + ", Assembly-CSharp").ToList();
                    dictionary.Add(key, source);
                }
            }
            return dictionary;
        }

    }

}
