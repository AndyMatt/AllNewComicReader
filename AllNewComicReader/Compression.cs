using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using SevenZip;
using System.Windows.Forms;

namespace AllNewComicReader
{
    class Compression
    {
        String ArchiveFilePath;
        LinkedList<Byte[]> ArchiveFiles;
        public string[] Filenames;
        LinkedListNode<Byte[]> pCurrentPage;

        SevenZipExtractor extractor;
        SevenZipExtractor Streamedextractor;

        Dictionary<int, string> ArchiveSort;

        MemoryStream Extracted;
        FileStream fs;
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
            //CheckifSupported("blabla.jpg");
            ArchiveSort = new Dictionary<int, string>();
            Initalise7zip();
            CurrentPosition = 0;
            iCurrentPage = -1;
            fs = new FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            //ms = new MemoryStream();
            //fs.CopyTo(ms);
            extractor = new SevenZipExtractor(fs);

            Streamedextractor = new SevenZipExtractor(fs);

            Filenames = extractor.ArchiveFileNames.ToArray<string>();

            Streamedextractor.ExtractionFinished += new EventHandler<EventArgs>(Streamedextractor_FileExtractionFinished);

            ArchiveFiles = new LinkedList<Byte[]>();
            SortArchive();

            //ExtractArchive();
        }

        /*
        public List<Byte[]> GetAllPages()
        {
            List<Byte[]> filesex = new List<Byte[]>();

             for(int i = 0; i < extractor.FilesCount; i++)
            {
                MemoryStream newfs = new MemoryStream();
                extractor.ExtractFile(i, newfs);
                filesex.Add(newfs.ToArray());
            }

            return filesex;
        }
         */

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
            //return Filenames[iCurrentPage];
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
            //ProcessStream.BeginInvoke(null, null);
            StreamtoImage();
        }

        public void Destroy()
        {
            extractor.Dispose();
        }


        public void StreamtoImage()
        {

            ArchiveFiles.AddLast(Extracted.ToArray());

            if(pCurrentPage == null)
                pCurrentPage = ArchiveFiles.First;

            if (CurrentPosition <= TotalPages - 1)
            ExtractArchive();
        }

        public Byte[] GetFirstFile()
        {
            if (Properties.Settings.Default.DoublePage)
                iCurrentDoublePage = 0;

            if (CurrentPosition <= 1)
            {
                MemoryStream newfs = new MemoryStream();
                extractor.ExtractFile(ArchiveSort[0], newfs);

                iCurrentPage = 0;
                return newfs.ToArray();
            }
            else
            {
                pCurrentPage = ArchiveFiles.First;
                iCurrentPage = 0;
                return pCurrentPage.Value;
            }
        }

        public Byte[] GetLastFile()
        {
            if (Properties.Settings.Default.DoublePage)
                iCurrentDoublePage = TotalPages - 1;

            if (CurrentPosition < TotalPages)
            {
                MemoryStream newfs = new MemoryStream();
                extractor.ExtractFile(ArchiveSort[TotalPages - 1], newfs);

                iCurrentPage = TotalPages - 1;
                return newfs.ToArray();
            }
            else
            {
                pCurrentPage = ArchiveFiles.Last;
                iCurrentPage = TotalPages - 1;
                return pCurrentPage.Value;
            }
        }

        public Byte[] GetPage(uint Position)
        {
            iCurrentPage = (int)Position;
            MemoryStream newfs = new MemoryStream();
            extractor.ExtractFile((int)Position, newfs);
            return newfs.ToArray();
        }

        public Byte[] ExtractCurrentFile()
        {
            if (CurrentPosition < iCurrentPage || iCurrentPage == 0)
            {
                MemoryStream newfs = new MemoryStream();
                extractor.ExtractFile(ArchiveSort[iCurrentPage], newfs);

                return newfs.ToArray();
            }
            else
            {
                return ArchiveFiles.ElementAt<byte[]>(iCurrentPage);

            }
        }


        public Byte[] ExtractNextFile()
        {
            if(Properties.Settings.Default.DoublePage)
            {
                if (iCurrentDoublePage > iCurrentPage)
                    iCurrentPage = iCurrentDoublePage;
            }

            if (iCurrentPage < TotalPages - 1)
            iCurrentPage++;

            if (ArchiveFiles.Count -1  < iCurrentPage)
            {
                MemoryStream newfs = new MemoryStream();
                SevenZipExtractor Extr = new SevenZipExtractor(ArchiveFilePath);
                Extr.ExtractFile(ArchiveSort[iCurrentPage], newfs);
                //extractor.BeginExtractFile(ArchiveSort[iCurrentPage], newfs);

                return newfs.ToArray();
            }
            else
            {
                return ArchiveFiles.ElementAt<byte[]>(iCurrentPage);

            }
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

            if (CurrentPosition <= iCurrentPage)
            {
                MemoryStream newfs = new MemoryStream();
                extractor.ExtractFile(ArchiveSort[iCurrentPage], newfs);

                return newfs.ToArray();
            }
            else
            {
                return ArchiveFiles.ElementAt<byte[]>(iCurrentPage);

            }
        }

        public Byte[] ExtractNextDoubleFile()
        {
            iCurrentDoublePage = iCurrentPage + 1;

            if (CurrentPosition < iCurrentDoublePage)
            {
                MemoryStream newfs = new MemoryStream();
                extractor.ExtractFile(ArchiveSort[iCurrentDoublePage], newfs);

                return newfs.ToArray();
            }
            else
            {
                return ArchiveFiles.ElementAt<byte[]>(iCurrentDoublePage);

            }
        }


        public Byte[] ExtractPrevDoubleFile()
        {
            iCurrentDoublePage = iCurrentPage - 1;

            if (CurrentPosition <= iCurrentDoublePage)
            {
                MemoryStream newfs = new MemoryStream();
                extractor.ExtractFile(ArchiveSort[iCurrentDoublePage], newfs);

                return newfs.ToArray();
            }
            else
            {
                return ArchiveFiles.ElementAt<byte[]>(iCurrentDoublePage);

            }
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
