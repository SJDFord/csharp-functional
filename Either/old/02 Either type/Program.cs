
using Demo.Either;
using System.Net;

namespace Demo
{

    public class Resource
    {
        public string Data { get; }

        public Resource(string data)
        {
            this.Data = data;
        }
    }

    public class Program
    {
        public static Either<Failed, Resource> Fetch(Uri address)
        {
            var request = WebRequest.Create(address);

            try
            {
                HttpWebResponse response = (HttpWebResponse) request.GetResponse();
                if (response.StatusCode == HttpStatusCode.NotFound)
                    return new Left<Failed, Resource>(new NotFound());

                if (response.StatusCode == HttpStatusCode.Redirect ||
                            response.StatusCode == HttpStatusCode.TemporaryRedirect)
                {
                    Uri redirectUri = new Uri(response.Headers[HttpResponseHeader.Location]);
                    return new Left<Failed, Resource>(new Moved(redirectUri));
                }

                if (response.StatusCode != HttpStatusCode.OK)
                    return new Left<Failed, Resource>(new Failed());

                Stream dataStream = response.GetResponseStream();
                string data = new StreamReader(dataStream).ReadToEnd();
                return new Right<Failed, Resource>(new Resource(data));

            }
            catch (WebException ex) when (ex.Status == WebExceptionStatus.Timeout)
            {
                return new Left<Failed, Resource>(new Timeout());
            }
            catch (WebException)
            {
                return new Left<Failed, Resource>(new NetworkError());
            }
        }

        public static void Main(string[] args)
        {
            Uri address = new Uri("https://something.out.there");
            Either<Failed, Resource> result = Fetch(address);

            string report = result
                .MapLeft(failure => $"Error fetching resource - {failure}")
                .ReduceRight(resource => resource.Data);

            Console.WriteLine(report);
        }

    }
}
