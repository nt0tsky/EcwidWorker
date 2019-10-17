﻿using System.Collections.Generic;
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
        private readonly SheetsParams sheetparams;

        private readonly SheetsService sheetsService;

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="sheetName">Имя вкладки</param>
        public SheetManager(SheetsParams sheetparams, SheetsService sheetsService)
        {
            this.sheetparams = sheetparams;
            this.sheetsService = sheetsService;
        }

        public bool Remove(string sheetName)
        {
            var info = sheetsService.Spreadsheets.Get(this.sheetparams.SheetId).Execute();
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

                sheetsService.Spreadsheets.BatchUpdate(batchRequest, this.sheetparams.SheetId).Execute();

                return true;
            }

            return false;
        }

        public Sheet Create(string sheetName)
        {
            var info = sheetsService.Spreadsheets.Get(this.sheetparams.SheetId).Execute();
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

                sheetsService.Spreadsheets.BatchUpdate(batchRequest, this.sheetparams.SheetId).Execute();
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
            var request = sheetsService.Spreadsheets.Values.Get(this.sheetparams.SheetId, range);
            var response = request.Execute();
            return response.Values;
        }

        public AppendValuesResponse Post(IList<object> data)
        {
            return this.Post(data, string.Empty, SheetsConstants.BEGIN);
        }

        public AppendValuesResponse Post(IList<object> data, string tabName, string beginColumn)
        {
            string lastLetter = char.ConvertFromUtf32(data.Count() + 65);
            var range = string.IsNullOrEmpty(tabName) ? SheetsConstants.END : $"{tabName}!{beginColumn}:{lastLetter}";
            var valueRange = new ValueRange()
            {
                Values = new List<IList<object>> { data }
            };
            var request = sheetsService.Spreadsheets.Values.Append(valueRange, this.sheetparams.SheetId, range);
            request.ValueInputOption = SpreadsheetsResource.ValuesResource.AppendRequest.ValueInputOptionEnum.USERENTERED;
            return request.Execute();
        }
    }
}
