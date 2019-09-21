using Google.Apis.Sheets.v4.Data;
using System.Collections.Generic;

namespace EcwidIntegration.GoogleSheets.Interfaces
{
    public interface IGoogleSheetsService
    {
        bool RemoveSheet(string sheetName);

        Sheet CreateSheet(string sheetName);

        object Write(string sheetName, IList<object> data, string startColumn);

        object Write(string sheetName, IList<object> data, string startColumn, string endColumn);

        IList<IList<object>> Get(string sheetName, string startColumn, string endColumn);

        IList<int> GetOrdersNumbers(string sheetName);
    }
}
