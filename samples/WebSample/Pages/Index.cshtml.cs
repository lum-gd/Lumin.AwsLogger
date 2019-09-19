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
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
            _logger.LogCritical("Critical");
            _logger.LogError("Error");
            _logger.LogWarning("Warning");
            _logger.LogInformation("Information");
            _logger.LogDebug("Debug");
            _logger.LogTrace("Trace");

            _logger.LogInformation("Welcome to the AWS Logger. You are viewing the home page");
            LatestErrorLogs = AWSLoggerConfig.GetLatestErrorLogs();
            LogLibraryErrors = AWSLoggerConfig.GetLogLibraryErrors();
        }

        public IEnumerable<LogItem> LatestErrorLogs { get; set; }
        public IEnumerable<LogItem> LogLibraryErrors { get; set; }
    }
}
