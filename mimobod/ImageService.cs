using Microsoft.Azure.CognitiveServices.Search.ImageSearch;
using System;
using System.Net;

namespace mimobod
{
    public class ImageService
    {
        // the image search term to be used in the query
        static string searchTerm = "human faces with emotions";

        public static void GetImages()
        {
            var client = AuthenticateService.AuthenticateImageClient();
            // DbService.SaveToDatabase(null,null);
            // make the search request to the Bing Image API, and get the results
            var imageResults = client.Images.SearchAsync(query: searchTerm, count: 10000).Result; //search query
            if (imageResults != null)
            {
                int i = 0;
                foreach (var image in imageResults.Value)
                {
                    using (WebClient webClient = new WebClient())
                    {
                        webClient.DownloadFileAsync(new Uri($"{image.ContentUrl}"), $@"C:\Users\User\Downloads\MIMOBOD-master\MIMOBOD-master\images\image{i}.{image.EncodingFormat}");
                        i++;
                    }
                }
            }
        }
    }
}
