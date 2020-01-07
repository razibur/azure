using Microsoft.Azure.Storage;
using Microsoft.Azure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Azure_LearningActivity_Blob
{
    class _EndState
    {
        static string _connectionString = "ejzNO1fCHjarT4kpx5EymeYeU7WYFyBzYBiVPQkIdL105YF9+pao5E+jxTVy/Yx4ToFzO7eXPf2qqJ4OvRJJ5w==";
        static string _savedBlobId;

        static void _Main(string[] args)
        {
            Console.WriteLine("Saving a blob...");
            SavedBlob();

            Console.WriteLine("Getting the saved blob...");
            Console.WriteLine(GetBlob());

            Console.WriteLine("Press any key to exit");
            Console.ReadLine();
        }

        static void SavedBlob()
        {
            CloudStorageAccount accout = CloudStorageAccount.Parse(_connectionString);
            CloudBlobClient client = accout.CreateCloudBlobClient();

            CloudBlobContainer container = client.GetContainerReference("test");
            container.CreateIfNotExistsAsync().Wait();

            _savedBlobId = DateTime.Now.Ticks.ToString();

            CloudBlockBlob blockBlob = container.GetBlockBlobReference(_savedBlobId);
            blockBlob.Properties.ContentType = "application/json";

            var json = @"
            {
            'By' : 'AZ-200 Learning Activity - Blob',
            'Message' : 'This is a test Message'
            }";

            blockBlob.UploadTextAsync(json).Wait();
        }

        static string GetBlob()
        {
            CloudStorageAccount accout = CloudStorageAccount.Parse(_connectionString);
            CloudBlobClient client = accout.CreateCloudBlobClient();

            CloudBlobContainer container = client.GetContainerReference("test");
            container.CreateIfNotExistsAsync().Wait();

            CloudBlob blob = container.GetBlockBlobReference(_savedBlobId);

            string json;
            using (var memoryStream = new MemoryStream())
            {
                blob.DownloadToStreamAsync(memoryStream).Wait();
                json = Encoding.UTF8.GetString(memoryStream.ToArray());
            }
            return json;
        }
    }
}
