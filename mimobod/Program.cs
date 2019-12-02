using Microsoft.Azure.CognitiveServices.Search.ImageSearch;
using Microsoft.Azure.CognitiveServices.Vision.Face;
using Microsoft.Azure.CognitiveServices.Vision.Face.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace mimobod
{
    class Program
    {
        // From your Face subscription in the Azure portal, get your subscription key and endpoint.
        // Set your environment variables using the names below. Close and reopen your project for changes to take effect.
        static string SubscriptionKeyFace = "";
        static string SubscriptionKeySearch = "";
        static string Endpoint = "";

        // the image search term to be used in the query
        static string searchTerm = "human faces with emotions";

        static void Main(string[] args)
        {
            //GetImages();

            var client = Authenticate(Endpoint, SubscriptionKeyFace);
            // Analyze an image to get features and other properties.
            DetectFaceExtract(client).Wait();
        }

        public static void GetImages()
        {
            var client = new ImageSearchClient(new Microsoft.Azure.CognitiveServices.Search.ImageSearch.ApiKeyServiceClientCredentials(SubscriptionKeySearch));
            // make the search request to the Bing Image API, and get the results
            var imageResults = client.Images.SearchAsync(query: searchTerm, count: 10).Result; //search query
            if (imageResults != null)
            {
                int i = 0;
                foreach (var image in imageResults.Value)
                {
                    using (WebClient webClient = new WebClient())
                    {
                        webClient.DownloadFileAsync(new Uri($"{image.ContentUrl}"), $@"C:\Users\Mikita_Dubovik\source\repos\ForTest\image\image{i}.{image.EncodingFormat}");
                        i++;
                    }
                }
            }
        }

        /*
         *	AUTHENTICATE
         *	Uses subscription key and region to create a client.
         */
        public static IFaceClient Authenticate(string endpoint, string key)
        {
            return new FaceClient(new Microsoft.Azure.CognitiveServices.Vision.Face.ApiKeyServiceClientCredentials(key)) { Endpoint = endpoint };
        }

        /* 
         * DETECT FACES
         * Detects features from faces and IDs them.
         */
        public static async Task DetectFaceExtract(IFaceClient client)
        {
            Console.WriteLine("========DETECT FACES========");
            Console.WriteLine();

            // Create a list of images
            string[] filePaths = Directory.GetFiles(@"C:\Users\Mikita_Dubovik\source\repos\ForTest\image\", "*", SearchOption.AllDirectories);

            foreach (var filePath in filePaths)
            {
                try
                {
                    byte[] byteData = GetImageAsByteArray(filePath);
                    using (var stream = new MemoryStream(byteData))
                    {
                        IList<DetectedFace> detectedFaces;

                        // Detect faces with all attributes from image url.
                        detectedFaces = await client.Face.DetectWithStreamAsync(stream,
                            returnFaceAttributes: new List<FaceAttributeType>
                            {
                                FaceAttributeType.Emotion
                            });

                        Console.WriteLine($"{detectedFaces.Count} face(s) detected from image `{filePath}`.");

                        foreach (var face in detectedFaces)
                        {
                            string emotionType = string.Empty;
                            double emotionValue = 0.0;
                            Emotion emotion = face.FaceAttributes.Emotion;
                            if (emotion.Anger > emotionValue) { emotionValue = emotion.Anger; emotionType = "Anger"; }
                            if (emotion.Contempt > emotionValue) { emotionValue = emotion.Contempt; emotionType = "Contempt"; }
                            if (emotion.Disgust > emotionValue) { emotionValue = emotion.Disgust; emotionType = "Disgust"; }
                            if (emotion.Fear > emotionValue) { emotionValue = emotion.Fear; emotionType = "Fear"; }
                            if (emotion.Happiness > emotionValue) { emotionValue = emotion.Happiness; emotionType = "Happiness"; }
                            if (emotion.Neutral > emotionValue) { emotionValue = emotion.Neutral; emotionType = "Neutral"; }
                            if (emotion.Sadness > emotionValue) { emotionValue = emotion.Sadness; emotionType = "Sadness"; }
                            if (emotion.Surprise > emotionValue) { emotionType = "Surprise"; }
                            Console.WriteLine($"Emotion : {emotionType}");
                        }
                    }
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