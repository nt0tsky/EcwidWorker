using System.Collections.Generic;
using System.IO;
using System.Linq;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using EcwidIntegration.GoogleSheets.Interfaces;

namespace EcwidIntegration.GoogleSheets
{
    public class GoogleSheetsService : IGoogleSheetsService
    {
        private const string credentialsName = "credentials.json";

        private readonly string applicationName = "GoogleSheetsWriter_FCAA338E-426A-44CB-8474-200048847DBD";
        private readonly string[] Scopes = { SheetsService.Scope.Spreadsheets };
        private readonly SheetsService sheetsService;
        private readonly string sheetId;

        /// <summary>
        /// Инициализация сервиса
        /// </summary>
        private SheetsService InitService()
        {
            using (var stream = new FileStream(credentialsName, FileMode.Open, FileAccess.Read))
            {
                return new SheetsService(new Google.Apis.Services.BaseClientService.Initializer()
                {
                    HttpClientInitializer = GoogleCredential.FromStream(stream).CreateScoped(Scopes),
                    ApplicationName = applicationName
                });
            }
        }

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="sheetId">Уникальный идентификатор листа</param>
        /// <param name="sheetName">Имя вкладки</param>
        public GoogleSheetsService(string sheetId)
        {
            this.sheetId = sheetId;
            this.sheetsService = InitService();
        }

        /// <summary>
        /// Сделать запись
        /// </summary>
        /// <param name="data">Данные для записи</param>
        /// <param name="startColumn">Начальная колонка</param>
        /// <returns>Результат записи</returns>
        public object Write(string sheetName, IList<object> data, string startColumn)
        {
            return Write(sheetName, data, startColumn, startColumn);
        }

        /// <summary>
        /// Создать вкладку
        /// </summary>
        /// <param name="sheetName">Имя вкладки</param>
        public Sheet CreateSheet(string sheetName)
        {
            var info = sheetsService.Spreadsheets.Get(sheetId).Execute();
            var sheet = info.Sheets.FirstOrDefault(s => s.Properties.Title == sheetName);
            if (sheet == null)
            {
                var addSheetRequest = new AddSheetRequest
                {
                    Properties = new SheetProperties()
                    {
                        Title = sheetName
                    }
                };
                var batchRequest = new BatchUpdateSpreadsheetRequest
                {
                    Requests = new List<Request>
                {
                    new Request
                    {
                        AddSheet = addSheetRequest
                    }
                }
                };

                sheetsService.Spreadsheets.BatchUpdate(batchRequest, sheetId).Execute();
                return CreateSheet(sheetName);
            }
            return sheet;
        }

        public bool RemoveSheet(string sheetName)
        {
            var info = sheetsService.Spreadsheets.Get(sheetId).Execute();
            var sheet4Delete = info.Sheets.FirstOrDefault(s => s.Properties.Title == sheetName);
            if (sheet4Delete != null)
            {
                var deleteSheetRequest = new DeleteSheetRequest
                {
                    SheetId = sheet4Delete.Properties.SheetId
                };

                var batchRequest = new BatchUpdateSpreadsheetRequest
                {
                    Requests = new List<Request>
                {
                    new Request
                    {
                        DeleteSheet = deleteSheetRequest
                    }
                }
                };

                sheetsService.Spreadsheets.BatchUpdate(batchRequest, sheetId).Execute();

                return true;
            }

            return false;
        }

        /// <summary>
        /// Сделать запись
        /// </summary>
        /// <param name="data">Данные для записи</param>
        /// <param name="startColumn">Начальная колонка</param>
        /// <param name="endColumn">Последняя колонка записи</param>
        /// <returns>Результат записи</returns>
        public object Write(string sheetName, IList<object> data, string startColumn, string endColumn)
        {
            var range = $"{sheetName}!{startColumn}:{endColumn}";
            var valueRange = new ValueRange()
            {
                Values = new List<IList<object>> { data }
            };
            var request = sheetsService.Spreadsheets.Values.Append(valueRange, sheetId, range);
            request.ValueInputOption = SpreadsheetsResource.ValuesResource.AppendRequest.ValueInputOptionEnum.USERENTERED;
            return request.Execute();
        }

        /// <summary>
        /// Список номеров заказов
        /// </summary>
        /// <returns></returns>
        public IList<int> GetOrdersNumbers(string sheetName)
        {
            var response = this.Get(sheetName, "B1", $"B");
            if (response != null && response.Any())
            {
                return response.Select(r =>
                {
                    int order;
                    var orderString = r.FirstOrDefault();
                    if (orderString != null && int.TryParse(orderString.ToString(), out order))
                    {
                        return order;
                    }

                    return -1;
                }).Where(r => r != -1).ToList();
            }

            return new List<int>();
        }

        public IList<IList<object>> Get(string sheetName, string startColumn, string endColumn)
        {
            var range = $"{sheetName}!{startColumn}:{endColumn}";
            var request = sheetsService.Spreadsheets.Values.Get(sheetId, range);
            var response = request.Execute();
            return response.Values;
        }
    }
}
