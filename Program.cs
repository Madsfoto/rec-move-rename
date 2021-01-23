using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace rec_move_rename
{
    class Program
    {

        public static void ProcessDirectory(string targetDirectory)
        {
            // Recurse into subdirectories of this directory.

            string[] subdirectoryEntries = Directory.GetDirectories(targetDirectory);
            foreach (string subdirectory in subdirectoryEntries)
            {
                
                ProcessDirectory(subdirectory);
            }
                

            // Process the list of files found in the directory.
            string[] fileEntries = Directory.GetFiles(targetDirectory);
            foreach (string fileName in fileEntries)
            {
                Console.WriteLine(fileName);
                ProcessFile(fileName);

            }
                

            }

        
        public static void ProcessFile(string path)
        {
            int lastIndex = path.LastIndexOf("\\")+1; // substring is 0 based, LastIndexOf is 1 based.
            string pureFileNameWithExt = path.Substring(lastIndex); // File name WITH extention
            string pureFileName = pureFileNameWithExt.Substring(0, pureFileNameWithExt.LastIndexOf(".")); // File name WITHOUT extention

            int filenameLengthTotal = pureFileNameWithExt.Length;
            int filenameLengthToPeriod = pureFileNameWithExt.LastIndexOf(".") + 1;
            string currentFilenameExt = pureFileNameWithExt.Substring(filenameLengthToPeriod,(filenameLengthTotal-filenameLengthToPeriod)); // the extention itself
            
            if (currentFilenameExt == fileType) //Only do the moving for the supplied extention
            {
                
                for (int i = 0; i<100 ; i++) // limit the number of duplicates
                {
                    
                    if (Rename(path, pureFileName,currentFilenameExt, i) == 0) // success
                    {
                        break;
                    }
                    
                }


            }


        }
        public static int Rename(string path, string pureFile, string extention, int tryNr)
        {
            int returnvalue = 1;
            
            if (tryNr == 0) // first run
            {
                try // test if the moving will work
                {
                    File.Move(path, curDir + "\\" + pureFile + "." + extention);
                    returnvalue = 0;
                }
                catch (Exception)
                {


                }
            }
            else // second and more run. We will succeed eventually do do 100 runs, whichever is less. 
            {
                try
                {
                    File.Move(path, curDir + "\\" + pureFile +"_"+tryNr+ "." + extention);
                    returnvalue = 0;
                }
                catch (Exception)
                {


                }
            }
            
            


            return returnvalue; // 0 for success, 1 for failure
        }

        static void SetFileType(string file)
        {
            fileType = file;
        }


        static string curDir = Directory.GetCurrentDirectory();
        static string fileType = "";

        static void Main(string[] args)
        {
            if (args.Length==0)
            {
                Console.WriteLine("rec_move_rename will, at the top directory, go through all directories, find all files with FileType and move them to the top directory");
                Console.WriteLine("The key is duplicate filenames are resolved by appending \"_n \" to the name, where n is an interger");
                Console.WriteLine("Syntax is rec_move_rename FileType");


            }
            else if (args.Length==1)
            {
                string fileExt = args[0];
                SetFileType(fileExt);


                //Console.WriteLine(curDir);
                // list of directories in current directory
                string[] dirArr = Directory.GetDirectories(Directory.GetCurrentDirectory());
                

                foreach (string path in dirArr)
                {   
                    if (Directory.Exists(path))
                    {
                        // This path is a directory
                        
                        ProcessDirectory(path);
                    }
                    else
                    {
                        Console.WriteLine("{0} is not a valid file or directory.", path);
                    }
                }

            }
        }
    }
}
