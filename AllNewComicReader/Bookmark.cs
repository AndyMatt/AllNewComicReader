using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;

namespace AllNewComicReader
{
    class Bookmark
    {
        Dictionary<string, uint> FilePositions;
        public Bookmark()
        {
            FilePositions = new Dictionary<string, uint>();
        }

        public void LoadFile()
        {
        }

        public void Serialize(string Filename, uint Pagenum) 
        {
            if (FilePositions.ContainsKey(Filename))
                FilePositions[Filename] = Pagenum;
            else
            FilePositions.Add(Filename, Pagenum);

        // To serialize the hashtable and its key/value pairs,   
        // you must first open a stream for writing.  
        // In this case, use a file stream.

            FileStream fs = new FileStream(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\AllNewComicReader\\DataFile.dat", FileMode.OpenOrCreate);

        // Construct a BinaryFormatter and use it to serialize the data to the stream.
        BinaryFormatter formatter = new BinaryFormatter();
        try 
        {
            formatter.Serialize(fs, FilePositions);
        }
        catch (SerializationException e) 
        {
            Console.WriteLine("Failed to serialize. Reason: " + e.Message);
            throw;
        }
        finally 
        {
            fs.Close();
        }
    }

        public bool CheckifBookMarked(string Filename)
        {
            if (FilePositions.ContainsKey(Filename))
            {
                if(FilePositions[Filename] != 0)
                return true;
            }
            return false;
        }

        public uint GetPage(string Filename)
        {
            if (FilePositions.ContainsKey(Filename))
                return FilePositions[Filename];

            return 0;
        }

        public void Deserialize() 
     {
         string Dir = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\AllNewComicReader\\";
         if (Directory.Exists(Dir) == false)
             Directory.CreateDirectory(Dir);

        // Open the file containing the data that you want to deserialize.
         FileStream fs = new FileStream(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\AllNewComicReader\\DataFile.dat", FileMode.OpenOrCreate);
        try 
        {
            BinaryFormatter formatter = new BinaryFormatter();

            // Deserialize the hashtable from the file and  
            // assign the reference to the local variable.
            if (fs.Length != 0)
            {
                FilePositions = (Dictionary<string, uint>)formatter.Deserialize(fs);
            }
        }
        catch (SerializationException e) 
        {
            Console.WriteLine("Failed to deserialize. Reason: " + e.Message);
            //throw;
        }
        finally 
        {
            fs.Close();
        }

    }
    }
}
