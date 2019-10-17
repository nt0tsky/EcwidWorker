using System.Collections.Generic;
using Google.Apis.Sheets.v4.Data;

namespace EcwidIntegration.GoogleSheets.Interfaces
{
    public interface ISheetManager
    {
        AppendValuesResponse Post(IList<object> data);

        IList<IList<object>> Get(string tabNAme, int length);

        Sheet Create(string sheetName);
    }
}
