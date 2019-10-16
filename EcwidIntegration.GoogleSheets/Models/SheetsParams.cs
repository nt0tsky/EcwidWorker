using System;
using System.Collections.Generic;
using System.Text;

namespace EcwidIntegration.GoogleSheets.Models
{
    public class SheetsParams
    {
        public string SheetId { get; set; }

        public string TabName { get; set; }

        public string ApplicationName { get; set; }

        public string CredentialsName { get; set; }
    }
}
