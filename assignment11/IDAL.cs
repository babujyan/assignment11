using System.Collections.Generic;

namespace assignment11
{
    interface IDAL
    {
        IEnumerable<T> GetData<T>(string code, params KeyValuePair<string, object>[] parameters);
    }
}
