Определение эмоций человека по фотографии. Данный проект сделан с помощью Azure Cognitive Service, CNTK

Ссылка на датасет, если нет возможности использовать скробблер - https://drive.google.com/drive/folders/0B5G8pYUQMNZnLTBVaENWUWdzR0E

Зависимости:
1. .NET Core 2.0 SDK 2.1.202;
2. Microsoft.Azure.CognitiveServices.Search.ImageSearch; Version="2.0.0"
3. Microsoft.Azure.CognitiveServices.Vision.ComputerVision; Version="5.0.0"
4. Microsoft.Azure.CognitiveServices.Vision.Face; Version="2.5.0-preview.1"
5. Azure подписка, на ней созданный Azure Cognitive Service c типами Bing.Search.v7 и Face. Из них необходимо взять Key1 и Endpoint

Запуск:
В терминале зайти в папку mimobod, прописать dotnet run 
