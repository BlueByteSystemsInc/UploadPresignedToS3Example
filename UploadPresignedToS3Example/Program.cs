using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace UploadPresignedToS3Example
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var presignedUrl = "PUT YOUR SIGNED URL HERE";

            Console.WriteLine($"presigned url: {presignedUrl}");


            Console.WriteLine("Reading file to upload:");
            var filePath = (new FileInfo("Test.bmp")).FullName;
            
            try
            {
                UploadObject(presignedUrl, filePath);
            }
            catch (Exception e) 
            {
             Console.Write($"{e.Message} {e.StackTrace}");   
            }
            finally
            {
                Console.ReadLine();
            }
        }

        // code from this page: https://docs.aws.amazon.com/AmazonS3/latest/userguide/PresignedUrlUploadObject.html
        private static void UploadObject(string url, string filePath)
        {
            HttpWebRequest httpRequest = WebRequest.Create(url) as HttpWebRequest;
            httpRequest.Method = "PUT";
            using (Stream dataStream = httpRequest.GetRequestStream())
            {
                var buffer = new byte[8000];
                using (FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                {
                    int bytesRead = 0;
                    while ((bytesRead = fileStream.Read(buffer, 0, buffer.Length)) > 0)
                    {
                        dataStream.Write(buffer, 0, bytesRead);
                    }
                }
            }
            HttpWebResponse response = httpRequest.GetResponse() as HttpWebResponse;
        }
    }
}
