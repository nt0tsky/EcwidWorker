using System.Collections.Generic;
using System.IO;
using System.Linq;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using EcwidIntegration.GoogleSheets.Interfaces;
using Google.Apis.Services;
using EcwidIntegration.GoogleSheets.Models;

namespace EcwidIntegration.GoogleSheets
{
    public class SheetService : ISheetService
    {
        private SheetsService sheetsService;
        private SheetManager sheetManager;
        public SheetsParams SheetParams { get; private set; }

        /// <summary>
        /// Инициализация сервиса
        /// </summary>
        private SheetsService InitService()
        {
            using (var stream = new FileStream(SheetParams.CredentialsName, FileMode.Open, FileAccess.Read))
            {
                return new SheetsService(new BaseClientService.Initializer()
                {
                    HttpClientInitializer = GoogleCredential.FromStream(stream).CreateScoped(new string[] { SheetsService.Scope.Spreadsheets }),
                    ApplicationName = SheetParams.ApplicationName
                });
            }
        }

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="sheetparams">Объект инициализации</param>
        public SheetService(SheetsParams sheetparams)
        {
            this.SheetParams = sheetparams;
            this.sheetManager = new SheetManager(this);
        }

        /// <summary>
        /// Получить инстанс сервиса вкладок
        /// </summary>
        /// <returns>Сервис вкладок</returns>
        public SheetsService GetInstance()
        {
            if (this.sheetsService == null)
            {
                this.sheetsService = InitService();
            }

            return this.sheetsService;
        }

        /// <summary>
        /// Сделать запись
        /// </summary>
        /// <param name="data">Данные для записи</param>
        /// <param name="startColumn">Начальная колонка</param>
        /// <returns>Результат записи</returns>
        public object Write(IList<object> data)
        {
            return this.sheetManager.Post(data);
        }

        /// <summary>
        /// Сделать запись
        /// </summary>
        /// <param name="data">Данные для записи</param>
        /// <param name="startColumn">Начальная колонка</param>
        /// <returns>Результат записи</returns>
        public object Write(IList<object> data, string sheetName, string beginColumn)
        {
            return this.sheetManager.Post(data, sheetName, beginColumn);
        }

        /// <summary>
        /// Создать вкладку
        /// </summary>
        /// <param name="sheetName">Имя вкладки</param>
        /// <returns>Созданная вкладка</returns>
        public Sheet CreateSheet(string sheetName)
        {
            return this.sheetManager.Create(sheetName);
        }

        /// <summary>
        /// Удалить вкладку
        /// </summary>
        /// <param name="sheetName">Имя вкладки</param>
        /// <returns>Успех</returns>
        public bool RemoveSheet(string sheetName)
        {
            return this.sheetManager.Remove(sheetName);
        }

        /// <summary>
        /// Список номеров заказов
        /// </summary>
        /// <returns></returns>
        public IList<int> GetOrdersNumbers(string tabName)
        {
            var response = this.Get(tabName, "B1", 1);
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

        IList<IList<object>> Get(string tabName, string beginColumn, int length)
        {
            return this.sheetManager.Get(tabName, beginColumn, length);
        }

        public IList<IList<object>> Get(string tabName, int length)
        {
            return this.Get(tabName, SheetsConstants.BEGIN, length);
        }
    }
}
