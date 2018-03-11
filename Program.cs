using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1
{
    public class Program
    {
        static string newLine = System.Environment.NewLine;
        static string userName = String.Empty;
        static string fileWritePath = String.Empty;

        public static void Main(string[] args)
        {
            try
            {
                Console.WriteLine(args.Length);
                string[] arr = new string[] { null, null };
                if (args != null && args.Length >= 2)  // run th' cmd with proper arguments
                {
                    arr[0] = args[0].Trim();
                    arr[1] = args[1].Trim();
                }
                else if (args != null && args.Length == 1) // th' send to
                {
                    //arr[0] = args[0].Substring(0, args[0].LastIndexOf('\\') + 1); // directory path
                    //arr[1] = args[0].Substring(args[0].LastIndexOf('\\') + 1); // fileName

                    arr[0] = Path.GetDirectoryName(args[0])+"\\";
                    arr[1] = Path.GetFileName(args[0]);
                    Console.WriteLine("dirpath:  " + arr[0]);
                    Console.WriteLine("filename:  " + arr[1]);
                }
                else // missing args
                {
                    Console.WriteLine("Please provide appropriate arguments");
                    Console.WriteLine("Correct Usage:");
                    Console.WriteLine("Argument 1:  FilePath");
                    Console.WriteLine("Argument 2:  FileName");

                    while (arr[0] == null || arr[0].Length == 0)
                    {
                        Console.WriteLine("Please enter the FilePath");
                        arr[0] = Console.ReadLine().ToString().Trim();
                    }
                    while (arr[1] == null || arr[1].Length == 0)
                    {
                        Console.WriteLine("Please enter the file name to be parsed.");
                        arr[1] = Console.ReadLine().ToString().Trim();
                    }
                }

                while (!Directory.Exists(arr[0]))
                {
                    Console.WriteLine("The FilePath you entered does not exist. Please enter a valid FilePath.");
                    arr[0] = Console.ReadLine().ToString().Trim();
                }
                while (!File.Exists(arr[0] + arr[1]))
                {
                    Console.WriteLine("Please enter a valid FileName.");
                    arr[1] = Console.ReadLine().ToString().Trim();
                }
                parseFile(arr);
                Console.ReadKey();
            }
            catch (Exception e) {
                LogError(e);
            }
        }

        public static void parseFile(string[] arguments) //string filePath
        {
            try {
             
                string pattern = arguments[1].Substring(0, arguments[1].IndexOf('.'));
                Console.WriteLine("pattern =   " + pattern);
                string[] fileArray = Directory.GetFiles(arguments[0], pattern + "*.txt");
                userName = System.Security.Principal.WindowsIdentity.GetCurrent().Name.Split('\\').Last();
                Console.WriteLine("user = " + userName);
                fileWritePath = "C:\\Users\\" + userName + "\\Documents\\";
                Console.WriteLine("write file location = " + fileWritePath);

                foreach (string file in fileArray)
                {
                    Console.WriteLine("===========  Parsing file " + file + "  =============");
                    File.AppendAllText(fileWritePath + "ErrorLines.txt", "===========  Parsing file " + file + "  =============" + newLine);
                    using (StreamReader sr = File.OpenText(file))
                    {
                        string line = String.Empty;
                        while ((line = sr.ReadLine()) != null)
                        {
                            if (line.Contains("(Error"))
                            {
                                Console.WriteLine(line);
                                File.AppendAllText(fileWritePath + "ErrorLines.txt", line + newLine);
                            }
                        }
                    }
                    Console.WriteLine("================================================================================");
                }
                Console.WriteLine(" ******* Finished processing ******* ");
            }
            catch (Exception e)
            {
                LogError(e);
            }
        }

        public static void LogError(Exception ex) {
            string filePath = fileWritePath + "Exception.txt";
            using (StreamWriter writer = new StreamWriter(filePath, true))
            {
                writer.WriteLine("Message :" + ex.Message + "<br/>" + Environment.NewLine + "StackTrace :" + ex.StackTrace +
                   "" + Environment.NewLine + "Date :" + DateTime.Now.ToString());
                writer.WriteLine(Environment.NewLine + "-----------------------------------------------------------------------------" + Environment.NewLine);
            }
        }
    }
}
