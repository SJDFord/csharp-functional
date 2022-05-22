using Either.Example.Common;
using Either.Lib;
using System.Net;

namespace Either.Example
{
    /// <summary>
    /// Web invocation example
    /// </summary>
    public class WebExample
    {
        public void Demonstrate()
        {
            Uri address = new Uri("https://something.out.there");
            var resourceFetcher = new ResourceFetcher();
            Either<Failed, Resource> result = resourceFetcher.Fetch(address);

            string report = result
                .MapLeft(failure => $"Error fetching resource - {failure}")
                .ReduceRight(resource => resource.Data);

            Console.WriteLine(report);
        }
    }

    public class ResourceFetcher {
        public Either<Failed, Resource> Fetch(Uri address)
        {
            var httpClient = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Get, address);

            try
            {
                var response = httpClient.Send(request);
                if (response.StatusCode == HttpStatusCode.NotFound)
                    return new Left<Failed, Resource>(new NotFound());

                if (
                    response.StatusCode == HttpStatusCode.Redirect ||
                    response.StatusCode == HttpStatusCode.TemporaryRedirect)
                {
                    var redirectUri = response.Headers.Location;
                    return new Left<Failed, Resource>(new Moved(redirectUri!));
                }

                if (response.StatusCode != HttpStatusCode.OK)
                    return new Left<Failed, Resource>(new Failed());

                var data = response.Content.ReadAsStringAsync().Result;
                return new Right<Failed, Resource>(new Resource(data));

            }
            catch (WebException ex) when (ex.Status == WebExceptionStatus.Timeout)
            {
                return new Left<Failed, Resource>(new Common.Timeout());
            }
            catch (WebException)
            {
                return new Left<Failed, Resource>(new NetworkError());
            }
        }
    }

    public class Resource
    {
        public string Data { get; }

        public Resource(string data)
        {
            Data = data;
        }
    }
}
