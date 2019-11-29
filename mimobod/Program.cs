using System;
using System.Collections.Generic;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision.Models;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace face_quickstart
{
    class Program
    {
        // From your Face subscription in the Azure portal, get your subscription key and endpoint.
        // Set your environment variables using the names below. Close and reopen your project for changes to take effect.
        static string SubscriptionKey = "";
        static string Endpoint = "";

        static void Main(string[] args)
        {

            ComputerVisionClient client = Authenticate(Endpoint, SubscriptionKey);
            // Analyze an image to get features and other properties.
            AnalyzeImageUrl(client).Wait();
        }

        /*
        * AUTHENTICATE
        * Creates a Computer Vision client used by each example.
        */
        public static ComputerVisionClient Authenticate(string endpoint, string key)
        {
            ComputerVisionClient client =
                new ComputerVisionClient(new ApiKeyServiceClientCredentials(key))
                { Endpoint = endpoint };
            return client;
        }

        /*
        * ANALYZE IMAGE - URL IMAGE
        * Analyze URL image. Extracts captions, categories, tags, objects, faces, racy/adult content,
        * brands, celebrities, landmarks, color scheme, and image types.
        */
        public static async Task AnalyzeImageUrl(ComputerVisionClient client)
        {
            Console.WriteLine("----------------------------------------------------------");
            Console.WriteLine();

            // Creating a list that defines the features to be extracted from the image.
            List<VisualFeatureTypes> features = new List<VisualFeatureTypes>()
           {
               VisualFeatureTypes.Categories, VisualFeatureTypes.Description,
               VisualFeatureTypes.Faces, VisualFeatureTypes.ImageType,
               VisualFeatureTypes.Tags, VisualFeatureTypes.Adult,
               VisualFeatureTypes.Color, VisualFeatureTypes.Brands,
               VisualFeatureTypes.Objects
           };

            Console.WriteLine();

            string[] filePaths = Directory.GetFiles(@"D:\Work\Mimobod\dir_001", "*", SearchOption.AllDirectories);

            foreach (var filePath in filePaths)
            {
                try
                {
                    byte[] byteData = GetImageAsByteArray(filePath);
                    using (var stream = new MemoryStream(byteData))
                    {
                        ImageAnalysis results = await client.AnalyzeImageInStreamAsync(stream, features);

                        // Celebrities in image, if any.
                        Console.WriteLine(filePath);
                        Console.WriteLine("Celebrities:");
                        foreach (var category in results.Categories)
                        {
                            if (category.Detail?.Celebrities != null)
                            {
                                foreach (var celeb in category.Detail.Celebrities)
                                {
                                    Console.WriteLine($"{celeb.Name} with confidence {celeb.Confidence}");
                                }
                            }
                        }

                        Console.WriteLine();
                    }

                    Console.WriteLine("----------------------------------------------------------");
                }
                catch (Exception)
                {
                    Console.WriteLine("----------------------------------------------------------");
                }
            }
        }

        static byte[] GetImageAsByteArray(string imageFilePath)
        {
            FileStream fileStream = new FileStream(imageFilePath, FileMode.Open, FileAccess.Read);
            BinaryReader binaryReader = new BinaryReader(fileStream);
            return binaryReader.ReadBytes((int)fileStream.Length);
        }
    }
}
