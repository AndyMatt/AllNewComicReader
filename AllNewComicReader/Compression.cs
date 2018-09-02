using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using SevenZip;
using System.Windows.Forms;
using AllNewComicReader.Helper;

namespace AllNewComicReader
{
    class Compression
    {
        String ArchiveFilePath;
        EnhancedDictionary<int,Byte[]> ArchiveFiles;
        public string[] Filenames;

        SevenZipExtractor extractor;
        SevenZipExtractor Streamedextractor;

        Dictionary<int, string> ArchiveSort;

        MemoryStream Extracted;
        readonly FileStream fs;
        //MemoryStream ms;
        public int TotalPages;
        public int CurrentPosition;
        public int iCurrentPage;
        public int iCurrentDoublePage;

        private delegate void ProcessExtractedStream();
        private ProcessExtractedStream ProcessStream;


        public Compression(string filename)
        {
            ArchiveFilePath = filename;
            ProcessStream = new ProcessExtractedStream(this.StreamtoImage);

            ArchiveSort = new Dictionary<int, string>();
            Initalise7zip();
            CurrentPosition = 0;
            iCurrentPage = -1;
            fs = new FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);

            extractor = new SevenZipExtractor(fs);

            Streamedextractor = new SevenZipExtractor(fs);

            Filenames = extractor.ArchiveFileNames.ToArray<string>();

            Streamedextractor.ExtractionFinished += new EventHandler<EventArgs>(Streamedextractor_FileExtractionFinished);

            ArchiveFiles = new EnhancedDictionary<int,Byte[]>();
            SortArchive();
        }

        public void SortArchive()
        {
            LinkedList<string> names = new LinkedList<string>();
            for(int i = 0; i < extractor.FilesCount; i++)
            {
                if(CheckifSupported(extractor.ArchiveFileNames[i]))
                names.AddLast(extractor.ArchiveFileNames[i]);
            }

            TotalPages = names.Count;
            int currentpos = 0;

            while(names.Count > 0)
            {
                
                int position = 0;

                for(int i = 0; i < names.Count; i++)
                {
                    if(string.Compare(names.ElementAt<string>(position),names.ElementAt<string>(i)) == 1)
                    {
                        position = i;
                    }
                }

                ArchiveSort.Add(currentpos, names.ElementAt<string>(position));
                names.Remove(names.ElementAt<string>(position));
                currentpos++;
            }

          
        }
        public void Initalise7zip()
        {
            string ProgramPath = Path.GetDirectoryName(Application.ExecutablePath);

            if (IntPtr.Size == 8) //x64
                SevenZip.SevenZipExtractor.SetLibraryPath(ProgramPath + @"/7z64.dll");
            else //x86
                SevenZip.SevenZipExtractor.SetLibraryPath(ProgramPath + @"/7z.dll");
        }

        public MemoryStream ExtractFile()
        {
            MemoryStream newfs = new MemoryStream();
            extractor.ExtractFile(ArchiveSort[0], newfs);

            return newfs;
        }

        public string GetFileName()
        {
            return ArchiveSort[iCurrentPage];
        }

        public string GetDoublePageFileName()
        {
            return ArchiveSort[iCurrentDoublePage];
        }

        public void ExtractArchive()
        {
            Extracted = new MemoryStream();
            Streamedextractor = new SevenZipExtractor(fs);
            Streamedextractor.ExtractionFinished += new EventHandler<EventArgs>(Streamedextractor_FileExtractionFinished);
            Streamedextractor.BeginExtractFile(ArchiveSort[CurrentPosition], Extracted);
            CurrentPosition++;

        }

        public void Streamedextractor_FileExtractionFinished(object sender, EventArgs e)
        {
            StreamtoImage();
        }

        public void Destroy()
        {
            extractor.Dispose();
        }


        public void StreamtoImage()
        {
            ArchiveFiles.Add(CurrentPosition, Extracted.ToArray());

            if (CurrentPosition <= TotalPages - 1)
            ExtractArchive();
        }

        public Byte[] GetFirstFile()
        {
            if (Properties.Settings.Default.DoublePage)
                iCurrentDoublePage = 0;

            iCurrentPage = 0;

            return RetrieveFileFromBufferOrArchive(iCurrentPage);
        }

        public Byte[] GetLastFile()
        {
            if (Properties.Settings.Default.DoublePage)
                iCurrentDoublePage = TotalPages - 1;

            iCurrentPage = TotalPages - 1;

            return RetrieveFileFromBufferOrArchive(iCurrentPage);          
        }

        public Byte[] GetPage(uint Position)
        {
            iCurrentPage = (int)Position;

            return RetrieveFileFromBufferOrArchive(iCurrentPage);
        }

        public Byte[] ExtractCurrentFile()
        {
            return RetrieveFileFromBufferOrArchive(iCurrentPage);
        }


        public Byte[] ExtractNextFile()
        {
            if(Properties.Settings.Default.DoublePage)
            {
                if (iCurrentDoublePage > iCurrentPage)
                    iCurrentPage = iCurrentDoublePage;
            }

            iCurrentPage++;

            return RetrieveFileFromBufferOrArchive(iCurrentPage);           
        }


        public Byte[] ExtractPrevFile()
        {
            if (Properties.Settings.Default.DoublePage)
            {
                if (iCurrentDoublePage < iCurrentPage)
                    iCurrentPage = iCurrentDoublePage;
            }

            if (iCurrentPage > 0)
            iCurrentPage--;

            return RetrieveFileFromBufferOrArchive(iCurrentPage);
        }

        public Byte[] ExtractNextDoubleFile()
        {
            iCurrentDoublePage = iCurrentPage + 1;

            return RetrieveFileFromBufferOrArchive(iCurrentDoublePage);
        }


        public Byte[] ExtractPrevDoubleFile()
        {
            iCurrentDoublePage = iCurrentPage - 1;

            return RetrieveFileFromBufferOrArchive(iCurrentDoublePage);
        }

        public Byte[] RetrieveFileFromBufferOrArchive(int PageNum)
        {
            if(ArchiveFiles.ContainsKey(PageNum))
                return ArchiveFiles[PageNum];

            MemoryStream newfs = new MemoryStream();
            SevenZipExtractor Extr = new SevenZipExtractor(fs);
            Extr.ExtractFile(ArchiveSort[PageNum], newfs);
            AddtoBuffer(PageNum, newfs.ToArray());

            return newfs.ToArray();
        }

        public void AddtoBuffer(int PageNum, Byte[] bytes)
        {
            //I dont want to store More than 100 cached Pages at a time
            if(ArchiveFiles.Count > 100)
            {
                ArchiveFiles.RemoveFromFront(ArchiveFiles.Count - 100);
            }

            ArchiveFiles.Add(PageNum, bytes);
        }


        public bool CheckifSupported(string filename)
        {


            bool bSupported = false;

            string fileformat = Path.GetExtension(filename).ToLower();

            if (fileformat == ".txt")
                return false;

             foreach (ImageMagick.MagickFormat format in Enum.GetValues(typeof(ImageMagick.MagickFormat)))
             {
                 if ("." + format.ToString().ToLower() == fileformat)
                bSupported = true;
             }

             return bSupported;
    
        }
    }
}
