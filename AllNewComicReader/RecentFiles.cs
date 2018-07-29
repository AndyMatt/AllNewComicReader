using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections;
using System.Threading;

namespace AllNewComicReader
{
    [Serializable()]
    public struct RecentInfo
    {
        public string Filepath;
        public DateTime OpenTime;
    }

    static class RecentFiles
    {
        static Dictionary<string, RecentInfo> Files;
        public static List<string> Recent;

        static RecentFiles()
        {
            Recent = new List<string>();
            Files = new Dictionary<string, RecentInfo>();
            Deserialize();
        }

        public static void Serialize(string Filename)
        {
            if (Files.ContainsKey(Path.GetFileNameWithoutExtension(Filename)))
            {

                Files.Remove(Path.GetFileNameWithoutExtension(Filename));
                RecentInfo newrecent; newrecent.Filepath = Filename; newrecent.OpenTime = DateTime.Now;
                Files.Add(Path.GetFileNameWithoutExtension(Filename), newrecent);
            }
            else
            {
                RecentInfo newrecent; newrecent.Filepath = Filename; newrecent.OpenTime = DateTime.Now;
                Files.Add(Path.GetFileNameWithoutExtension(Filename), newrecent);

                while (Files.Count > 10)
                    RemoveOldestFile();
            }

            // To serialize the hashtable and its key/value pairs,   
            // you must first open a stream for writing.  
            // In this case, use a file stream.

            FileStream fs = GetRecentBin();

            if (fs == null)
                return;

            // Construct a BinaryFormatter and use it to serialize the data to the stream.
            BinaryFormatter formatter = new BinaryFormatter();
            try
            {
                formatter.Serialize(fs, Files);
            }
            catch (SerializationException e)
            {
                Console.WriteLine("Failed to serialize. Reason: " + e.Message);
                throw;
            }
            finally
            {
                fs.Close();
                Deserialize();
                PopulateList();
            }
        }

        static void RemoveOldestFile()
        {
            KeyValuePair<string, RecentInfo> RemoveFile = Files.First();

            foreach (KeyValuePair<string, RecentInfo> entry in Files)
            {
                if (DateTime.Compare(RemoveFile.Value.OpenTime, entry.Value.OpenTime) > 0)
                    RemoveFile = entry;
            }

            Files.Remove(RemoveFile.Key);

        }

        static void PopulateList()
        {
            Recent = new List<string>();

            Dictionary<string, RecentInfo> copy = new Dictionary<string, RecentInfo>(Files);

            while (copy.Count > 0)
            {
                KeyValuePair<string, RecentInfo> AddFile = copy.First();

                foreach (KeyValuePair<string, RecentInfo> entry in copy)
                {
                    if (DateTime.Compare(AddFile.Value.OpenTime, entry.Value.OpenTime) > 0)
                        AddFile = entry;
                }

                Recent.Add(AddFile.Key);
                copy.Remove(AddFile.Key);
            }
        }

        public static string GetFilepath(string Filename)
        { 
            if (Files.ContainsKey(Filename))
            {
                return Files[Filename].Filepath;
            }
            return "";
        }

        public static void Deserialize()
        {

            // Open the file containing the data that you want to deserialize.
            try
            {
                string Dir = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\AllNewComicReader\\";
                if (Directory.Exists(Dir) == false)
                    Directory.CreateDirectory(Dir);

                FileStream fs = GetRecentBin(); //new FileStream(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\AllNewComicReader\\Recent.dat", FileMode.OpenOrCreate);

                if (fs == null)
                    return;

                BinaryFormatter formatter = new BinaryFormatter();

                // Deserialize the hashtable from the file and  
                // assign the reference to the local variable.
                if (fs.Length != 0)
                {
                    Files = (Dictionary<string, RecentInfo>)formatter.Deserialize(fs);
                    PopulateList();
                }
                fs.Close();
            }
            catch (SerializationException e)
            {
                Console.WriteLine("Failed to deserialize. Reason: " + e.Message);
                //throw;
            }

        }

        private static FileStream GetRecentBin()
        {
            int Attempts = 0;
            FileStream stream = null;

            while(Attempts < 20)
            {
                try
                {
                    stream = new FileStream(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\AllNewComicReader\\Recent.dat", FileMode.OpenOrCreate);
                    return stream;
                }
                catch
                {
                    Attempts++;
                    Thread.Sleep(1000);
                }
            }

            return null;
        }

    }
}
