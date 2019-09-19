using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lumin.AwsLogger;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace WebSample.Pages
{
    public class IndexModel : PageModel
    {
        ILogger<IndexModel> Logger { get; set; }

        public IndexModel(ILogger<IndexModel> logger)
        {
            Logger = logger;
        }

        public void OnGet()
        {
            Logger.LogInformation("Welcome to the AWS Logger. You are viewing the home page");
            LogLibraryErrors = AWSLoggerConfig.GetLogLibraryErrors();
        }

        public IEnumerable<LibraryLogItem> LogLibraryErrors { get; set; }
    }
}
