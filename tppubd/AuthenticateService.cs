using Microsoft.Azure.CognitiveServices.Search.ImageSearch;
using Microsoft.Azure.CognitiveServices.Vision.Face;

namespace tppubd
{
    public class AuthenticateService
    {
        // From your Face subscription in the Azure portal, get your subscription key and endpoint.
        // Set your environment variables using the names below. Close and reopen your project for changes to take effect.
        static string SubscriptionKeyFace = "52bd7b2e2b704f98ba875709424f6d6e";
        static string SubscriptionKeySearch = "670fd9da678c45bc863d686d402b32cb";
        static string Endpoint = "https://mimobodface.cognitiveservices.azure.com/";

        /*
         *	AUTHENTICATE
         *	Uses subscription key and region to create a client.
         */
        public static IFaceClient AuthenticateFaceClient()
        {
            return new FaceClient(new Microsoft.Azure.CognitiveServices.Vision.Face.ApiKeyServiceClientCredentials(SubscriptionKeyFace)) { Endpoint = Endpoint };
        }

        public static ImageSearchClient AuthenticateImageClient()
        {
            return new ImageSearchClient(new Microsoft.Azure.CognitiveServices.Search.ImageSearch.ApiKeyServiceClientCredentials(SubscriptionKeySearch));
        }
    }
}
