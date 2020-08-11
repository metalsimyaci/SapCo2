using System.Collections.Generic;
using SapCo2.Core.Abstract;

namespace SapCo2.Abstract
{
    public interface IReadTable<T>:IRfcFunction where T:class
    {
        List<T> GetTable(IRfcConnection connection, List<string> whereClause = null, bool getUnsafeFields = false, int rowCount = 0, int rowSkips = 0, string delimiter = "|", string noData = "");

        T GetStruct(IRfcConnection connection, List<string> whereClause = null, bool getUnsafeFields = false, string delimiter = "|", string noData = "");
    }
}
