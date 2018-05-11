using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace assignment11
{
    interface IDAL
    {
        IEnumerable<T> GetData<T>(string code, ICollection<KeyValuePair<string, object>> parameters);
    }
}
