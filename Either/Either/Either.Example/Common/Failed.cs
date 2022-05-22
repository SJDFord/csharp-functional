namespace Either.Example.Common
{
    public class Failed {}

    class NotFound : Failed { }

    class Moved : Failed
    {
        public Uri MovedTo { get; }
        public Moved(Uri movedTo)
        {
            MovedTo = movedTo;
        }
    }

    class Timeout : Failed { }

    class NetworkError : Failed { }

    class Unknown : Failed { }
}
