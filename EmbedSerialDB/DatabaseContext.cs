using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;

namespace EmbedSerialDB
{
    public class DatabaseContext : IDisposable
    {
        public string Filename { get; set; }
        public bool IsLoaded { get; set; } = false;

        private string Content { get; set; }

        public dynamic Search(object terms)
        {
            var Results = new List<object>();

            var TermsList = new Dictionary<string, JToken>();
            var AllTerms = JsonConvert.DeserializeObject<JToken>(JsonConvert.SerializeObject(terms));
            foreach(var x in AllTerms)
            {
                TermsList.Add((x as JProperty).Name, (x as JProperty).Value);
            }

            var Context = JsonConvert.DeserializeObject<List<object>>(Content);
            foreach(dynamic Insert in Context)
            {
                foreach(var Term in TermsList)
                {
                    if(Insert[Term.Key] == Term.Value)
                    {
                        Results.Add(Insert);
                        break;
                    }
                }
            }

            return (dynamic)Results;
        }

        public dynamic SearchOne(object terms)
        {
            return Search(terms)[0];
        }

        public dynamic SearchById(string id)
        {
            return false;
        }

        public string Insert(dynamic document)
        {
            var CurrentDatabase = (dynamic)null;
            var NewInsertId = Guid.NewGuid().ToString();

            /// Loading previous database
            if (string.IsNullOrWhiteSpace(Content))
            {
                var toInsertDocument = JsonConvert.DeserializeObject<JToken>(JsonConvert.SerializeObject(document));
                toInsertDocument.id = NewInsertId;

                CurrentDatabase = new List<object>
                {
                    toInsertDocument
                };
            }
            else
            {
                var toInsertDocument = JsonConvert.DeserializeObject<JToken>(JsonConvert.SerializeObject(document));
                toInsertDocument.id = NewInsertId;

                var PreviousDatabase = JsonConvert.DeserializeObject<List<object>>(Content);
                PreviousDatabase.Add(toInsertDocument);
                CurrentDatabase = PreviousDatabase;
            }

            /// We save the new database
            SaveDatabase(Filename, JsonConvert.SerializeObject(CurrentDatabase, Formatting.Indented));

            return NewInsertId;
        }

        public DatabaseContext Exec()
        {
            LoadDatabase(Filename);

            IsLoaded = true;

            return this;
        }

        private void LoadDatabase(string database)
        {
            Content = File.ReadAllText(Filename);
        }

        private void SaveDatabase(string filename, string content)
        {
            Content = content;
            File.WriteAllText(filename, content);
        }

        public void Dispose()
        {
        }
    }
}