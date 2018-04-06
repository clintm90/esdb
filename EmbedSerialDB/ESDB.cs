using System;
using System.IO;

namespace EmbedSerialDB
{
    public class ESDB : IDisposable
    {
        public string WorkingDirectory { get; set; }
        public bool DebugMode { get; set; }

        /// <summary>
        /// Initialize the Context DB
        /// </summary>
        /// <param name="isDebug">Specify if ESDB is loaded with debug mode</param>
        public ESDB(bool isDebug = false)
        {
            WorkingDirectory = Environment.CurrentDirectory;
            DebugMode = isDebug;

            var esdbDirectory = Path.Combine(WorkingDirectory, ".esdb");
            Console.WriteLine($"ESDB Folder: {esdbDirectory}");

            if(!Directory.Exists(esdbDirectory))
            {
                Directory.CreateDirectory(esdbDirectory);
                File.SetAttributes(esdbDirectory, FileAttributes.Hidden);
            }
        }

        /// <summary>
        /// Export all the databases in a zip file
        /// </summary>
        /// <param name="filename"></param>
        public void Export(string filename)
        {
        }

        /// <summary>
        /// Initialize the Context DB in specified directory
        /// </summary>
        /// <param name="workingDirectory">Specified working directory</param>
        public ESDB(string workingDirectory)
        {
            WorkingDirectory = workingDirectory;

            Console.WriteLine($"ESDB Folder: {workingDirectory}");
        }

        /// <summary>
        /// Load a database
        /// </summary>
        /// <param name="databaseName">The name of the database (with or without .json extension)</param>
        /// <param name="createIfNotExists">Create the database if does not exists</param>
        /// <returns>Return true or false if the database is correctly loaded</returns>
        /// <summary>
        public DatabaseContext LoadDatabase(string databaseName, bool createIfNotExists = false)
        {
            var databaseBasename = string.Concat(databaseName, databaseName.EndsWith("json") ? "" : ".json");
            var databaseFilename = Path.Combine(WorkingDirectory, ".esdb", databaseBasename);

            if (createIfNotExists && !File.Exists(databaseFilename))
            {
                File.Create(databaseFilename);
            }

            return new DatabaseContext()
            {
                Filename = databaseFilename
            }.Exec();
        }

        /// <summary>
        /// Load a database with a specified type
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="databaseName">the name of the database (with or without .json extension)</param>
        /// <param name="createIfNotExists">create the database if does not exists</param>
        /// <returns>return true or false if the database is correctly loaded</returns>
        public DatabaseContext LoadDatabase<T>(string databaseName, bool createIfNotExists = false)
        {
            return new DatabaseContext()
            {
                Filename = string.Concat(databaseName, databaseName.EndsWith("json") ? "" : ".json")
            }.Exec();
        }

        public void Dispose()
        {
        }
    }
}
