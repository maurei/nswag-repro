using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using NswagRepro.ClientV1;
using NswagRepro.ClientV2;

namespace NswagRepro
{
    internal static class Program
    {
        private static async Task Main()
        {
            using var httpClient = new HttpClient
            {
                BaseAddress = new Uri("http://localhost:5000")
            };

            INswagReproClientV1 v1Client = new NswagReproClientV1(httpClient);
            INswagReproClientV2 v2Client = new NswagReproClientV2(httpClient);

            ClientV1.PersonCollectionResponseDocument v1PersonDocument = await v1Client.GetPersonCollectionAsync();
            ClientV1.Attributes v1PersonAttributes = v1PersonDocument.Data.First().Attributes;

            ClientV2.PersonCollectionResponseDocument v2PersonDocument = await v2Client.GetPersonCollectionAsync();
            ClientV2.Attributes2 v2PersonAttributes = v2PersonDocument.Data.First().Attributes;

            var v1PropertyType = typeof(ClientV1.PersonDataInResponse).GetProperty(nameof(ClientV1.PersonDataInResponse.Attributes))!.PropertyType;
            var v2PropertyType = typeof(ClientV1.PersonDataInResponse).GetProperty(nameof(ClientV1.PersonDataInResponse.Attributes))!.PropertyType;

            if (v1PropertyType.Name != v2PropertyType.Name)
            {
                // Adding a new path to the OpenAPI document shouldn't affect names of types generated in already existing paths
                // that are unchanged in the new OpenAPI document.
                throw new Exception();
            }
        }
    }
}
