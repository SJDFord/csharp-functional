using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo
{
    public class Failed
    {
    }

    class NotFound : Failed { }

    class Moved : Failed
    {
        public Uri MovedTo { get; }
        public Moved(Uri movedTo)
        {
            this.MovedTo = movedTo;
        }
    }

    class Timeout : Failed { }

    class NetworkError : Failed { }
}
