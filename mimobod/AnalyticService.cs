using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.CognitiveServices.Vision.Face;
using Microsoft.Azure.CognitiveServices.Vision.Face.Models;

namespace mimobod
{
    public class AnalyticService
    {
        public static async Task DetectFaceExtract()
        {
            Console.WriteLine("========DETECT FACES========");
            Console.WriteLine();
            var client = AuthenticateService.AuthenticateFaceClient();
            // Create a list of images
            string[] filePaths = Directory.GetFiles(@"C:\Users\User\Downloads\MIMOBOD-master\MIMOBOD-master\images",
                "*", SearchOption.AllDirectories);

            foreach (var filePath in filePaths)
            {
                try
                {
                    byte[] byteData = ParserService.GetImageAsByteArray(filePath);
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
                        List<string> emotions = new List<string>();
                        foreach (var face in detectedFaces)
                        {
                            string emotionType = string.Empty;
                            double emotionValue = 0.0;
                            Emotion emotion = face.FaceAttributes.Emotion;
                            if (emotion.Anger > emotionValue)
                            {
                                emotionValue = emotion.Anger;
                                emotionType = "Anger";
                            }

                            if (emotion.Contempt > emotionValue)
                            {
                                emotionValue = emotion.Contempt;
                                emotionType = "Contempt";
                            }

                            if (emotion.Disgust > emotionValue)
                            {
                                emotionValue = emotion.Disgust;
                                emotionType = "Disgust";
                            }

                            if (emotion.Fear > emotionValue)
                            {
                                emotionValue = emotion.Fear;
                                emotionType = "Fear";
                            }

                            if (emotion.Happiness > emotionValue)
                            {
                                emotionValue = emotion.Happiness;
                                emotionType = "Happiness";
                            }

                            if (emotion.Neutral > emotionValue)
                            {
                                emotionValue = emotion.Neutral;
                                emotionType = "Neutral";
                            }

                            if (emotion.Sadness > emotionValue)
                            {
                                emotionValue = emotion.Sadness;
                                emotionType = "Sadness";
                            }

                            if (emotion.Surprise > emotionValue)
                            {
                                emotionType = "Surprise";
                            }

                            Console.WriteLine($"Emotion : {emotionType}");
                            emotions.Add(emotionType);
                        }

                        DbService.SaveToDatabase(byteData, emotions);
                    }
                }
                catch (Exception)
                {
                    Console.WriteLine("----------------------------------------------------------");
                }
            }
        }
    }
}
