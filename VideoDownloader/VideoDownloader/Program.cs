using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Net;

namespace VideoMerge
{
    class Program
    {


        static void Main(string[] args)
        {
            string stringSplitter = "_PK598_";
            string[] parsedArgs = parseArgs(args, stringSplitter);
            if (validateArgs(parsedArgs, stringSplitter))
            {
                Downloader(parsedArgs);
            }
            Console.WriteLine("Finished!");
            Console.ReadKey();

        }

        private static string[] parseArgs(string[] args, string stringSplitter)
        {
            
            var temp = args[0].Split(':')[1];
            var parsedArgs = temp.Split(new string[] {stringSplitter}, StringSplitOptions.None);
            foreach (var arg in parsedArgs)
            {
                arg.Trim();
                Console.WriteLine(arg);
            }
            return parsedArgs;
        }

        private static bool validateArgs(string[] args, string stringSplitter)
        {
            
            if (args.Length != 2)
            {
                Console.WriteLine("Incorrect number of arguments supplied, 2 were expected " + args.Length + " were supplied");
                Console.WriteLine("Possible mismatch between C# and JS. C# String Splitter = " + stringSplitter);
                Console.ReadKey();
                return false;
            }
            Console.WriteLine("videoURLID : " + args[0]);
            Console.WriteLine("videoID : " + "video_" + args[1]);
            //Console.WriteLine("Title: " + args[2]); Could add the title in the future
            return true;
        }



        private static void Downloader(string[] args)
        {

            var videoURLID = args[0];
            var videoID = "video_" + args[1];

            var baseUrl = "https://cdn02.brighttalk.com/core/asset/video/" + videoURLID + "/ios/iphone/";
            var m3u8Url = baseUrl + videoID + "-4.m3u8";

            var m3u8Path = "C:\\Users\\trones\\Downloads\\" + videoID + "-4.m3u8";
            Downloadm3u8(m3u8Url, m3u8Path);

        
            var videoOutputPath = "C:\\Users\\trones\\test\\";
            List<string> TSFilesNames = Parsem3u8(m3u8Path);

            Console.WriteLine("Downloading and Saving to " + videoOutputPath);

            //Just concatenate the streams
            using (var filestream = new FileStream(videoOutputPath + videoID + ".ts", FileMode.Append, FileAccess.Write))
            {
                foreach (var file in TSFilesNames)
                {
                    var videoPart = GetandAppendTsfile(baseUrl + file);
                    videoPart.CopyTo(filestream);
                    Console.WriteLine(file + " downloaded");
                }
            }           
        }

        private static void Downloadm3u8(string m3u8Url, string path)
        {
            try
            {
                var request = WebRequest.Create(m3u8Url);
                var response = request.GetResponse();
                var responsestream = response.GetResponseStream();
                using (var file = File.Create(path))
                {
                    responsestream.CopyTo(file);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.ReadKey();
                Environment.Exit(0);
            }
        }

        static List<string> Parsem3u8(string path)
        {
            var sr = new StreamReader(path);
            string line;
            var tsfilesnames = new List<string>();
            while ((line = sr.ReadLine()) != null)
            {
                if (!line.StartsWith("#"))
                {
                    tsfilesnames.Add(line);
                    //Console.WriteLine(line);
                }
            }
            return tsfilesnames;
        }

        //Send the get request and write to folder
        static void GetandSaveTsfile(string url, string outputfilepath)
        {
            WebRequest request = WebRequest.Create(url);
            WebResponse response = request.GetResponse();
            Stream responsestream = response.GetResponseStream();
            using (var file = File.Create(outputfilepath))
            {
                responsestream.CopyTo(file);
            }
        }

        static Stream GetandAppendTsfile(string url)
        {
            WebRequest request = WebRequest.Create(url);
            WebResponse response = request.GetResponse();
            Stream responsestream = response.GetResponseStream();
            return responsestream;
        }


    }
}
