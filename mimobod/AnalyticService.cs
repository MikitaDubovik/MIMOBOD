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
            string[] filePaths = Directory.GetFiles(@"D:\Work\Mimobod\dir_001",
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
                                FaceAttributeType.Accessories, FaceAttributeType.Age,
                                FaceAttributeType.Emotion, FaceAttributeType.FacialHair,
                                FaceAttributeType.Gender, FaceAttributeType.Glasses, FaceAttributeType.Hair,
                                FaceAttributeType.Makeup,  FaceAttributeType.Smile
                            });

                        Console.WriteLine($"{detectedFaces.Count} face(s) detected from image `{filePath}`.");
                        if (detectedFaces.Count > 0)
                        {
                            var imageIndsex = DbService.SaveImage(byteData);

                            // Parse and print all attributes of each detected face.
                            var faceNumber = 1;
                            foreach (var face in detectedFaces)
                            {
                                // Get accessories of the faces
                                List<Accessory> accessoriesList = (List<Accessory>)face.FaceAttributes.Accessories;
                                int count = face.FaceAttributes.Accessories.Count;
                                string accessory; string[] accessoryArray = new string[count];
                                if (count == 0) { accessory = "NoAccessories"; }
                                else
                                {
                                    for (int i = 0; i < count; ++i) { accessoryArray[i] = accessoriesList[i].Type.ToString(); }
                                    accessory = string.Join(",", accessoryArray);
                                }


                                // Get emotion on the face
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

                                // Get more face attributes
                                var facialHair = string.Format("{0}", face.FaceAttributes.FacialHair.Moustache + face.FaceAttributes.FacialHair.Beard + face.FaceAttributes.FacialHair.Sideburns > 0 ? "Yes" : "No");

                                // Get hair color
                                Hair hair = face.FaceAttributes.Hair;
                                string color = null;
                                if (hair.HairColor.Count == 0) { if (hair.Invisible) { color = "Invisible"; } else { color = "Bald"; } }
                                HairColorType returnColor = HairColorType.Unknown;
                                double maxConfidence = 0.0f;
                                foreach (HairColor hairColor in hair.HairColor)
                                {
                                    if (hairColor.Confidence <= maxConfidence) { continue; }
                                    maxConfidence = hairColor.Confidence; returnColor = hairColor.Color; color = returnColor.ToString();
                                }

                                DbService.SaveToDatabase(
                                    imageIndsex,
                                     accessory,
                                      face.FaceAttributes.Age,
                                       emotionType,
                                        facialHair,
                                         (int)face.FaceAttributes.Gender,
                                           (int)face.FaceAttributes.Glasses,
                                             color,
                                              face.FaceAttributes.Smile,
                                               faceNumber);
                                faceNumber++;
                            }
                        }
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
