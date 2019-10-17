using System.Collections.Generic;
using System.Linq;
using EcwidIntegration.GoogleSheets.Interfaces;
using EcwidIntegration.GoogleSheets.Models;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;

namespace EcwidIntegration.GoogleSheets
{
    /// <summary>
    /// Менеджер работы с вкладками
    /// </summary>
    public class SheetManager : ISheetManager
    {
        private readonly SheetsService googleSheetService;
        private readonly SheetService sheetService;

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="sheetService">Сервис</param>
        public SheetManager(SheetService sheetService)
        {
            this.sheetService = sheetService;
            this.googleSheetService = sheetService.GetInstance();
        }

        /// <summary>
        /// Получить таб по имени вкладки
        /// </summary>
        /// <param name="sheetName">Имя вкладки</param>
        /// <returns>Таб</returns>
        private Sheet GetSheet(string sheetName)
        {
            var info = googleSheetService.Spreadsheets.Get(this.sheetService.SheetParams.SheetId).Execute();
            return info.Sheets.FirstOrDefault(s => s.Properties.Title == sheetName);
        }

        public bool Remove(string sheetName)
        {
            var sheet = GetSheet(sheetName);
            if (sheet != null)
            {
                var deleteSheetRequest = new DeleteSheetRequest
                {
                    SheetId = sheet.Properties.SheetId
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

                googleSheetService.Spreadsheets.BatchUpdate(batchRequest, this.sheetService.SheetParams.SheetId).Execute();

                return true;
            }

            return false;
        }

        public Sheet Create(string sheetName)
        {
            var sheet = GetSheet(sheetName);
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

                googleSheetService.Spreadsheets.BatchUpdate(batchRequest, this.sheetService.SheetParams.SheetId).Execute();
                return Create(sheetName);
            }
            return sheet;
        }

        /// <summary>
        /// Получить записи по имени вкладки
        /// </summary>
        /// <param name="tabName">Имя вкладки</param>
        /// <param name="length">Количество колонок для чтения</param>
        /// <returns>Список записей</returns>
        public IList<IList<object>> Get(string tabName, int length)
        {
            return this.Get(tabName, SheetsConstants.BEGIN, length);
        }

        /// <summary>
        /// Получить записи по имени вкладки
        /// </summary>
        /// <param name="tabName">Имя вкладки</param>
        /// <param name="length">Количество колонок для чтения</param>
        /// <returns>Список записей</returns>
        public IList<IList<object>> Get(string tabName, string beginColumn, int length)
        {
            string lastLetter = char.ConvertFromUtf32(length + 65);
            var range = $"{tabName}!{beginColumn}:{lastLetter}";
            var request = googleSheetService.Spreadsheets.Values.Get(this.sheetService.SheetParams.SheetId, range);
            var response = request.Execute();
            return response.Values;
        }

        /// <summary>
        /// Опубликовать запись
        /// </summary>
        /// <param name="data">Данные записи</param>
        /// <returns>Результат</returns>
        public AppendValuesResponse Post(IList<object> data)
        {
            return this.Post(data, string.Empty, SheetsConstants.BEGIN);
        }

        /// <summary>
        /// Опубликовать запись
        /// </summary>
        /// <param name="data">Данные записи</param>
        /// <param name="beginColumn">Начальная колонка</param>
        /// <param name="tabName">Имя вкладки</param>
        /// <returns>Результат</returns>
        public AppendValuesResponse Post(IList<object> data, string tabName, string beginColumn)
        {
            string lastLetter = char.ConvertFromUtf32(data.Count() + 65);
            var range = string.IsNullOrEmpty(tabName) ? SheetsConstants.END : $"{tabName}!{beginColumn}:{lastLetter}";
            var valueRange = new ValueRange()
            {
                Values = new List<IList<object>> { data }
            };
            var request = googleSheetService.Spreadsheets.Values.Append(valueRange, this.sheetService.SheetParams.SheetId, range);
            request.ValueInputOption = SpreadsheetsResource.ValuesResource.AppendRequest.ValueInputOptionEnum.USERENTERED;
            return request.Execute();
        }
    }
}
