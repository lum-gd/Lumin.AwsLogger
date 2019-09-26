using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lumin.AwsLogger.AspNetCore.Castle
{
    public class MsFactory : global::Castle.Core.Logging.AbstractLoggerFactory
    {
        public override global::Castle.Core.Logging.ILogger Create(string name)
        {
            return null;
            //return new MsLogger(
            //    logger.ForContext(Serilog.Core.Constants.SourceContextPropertyName, name),
            //    this);
        }

        public override global::Castle.Core.Logging.ILogger Create(string name, global::Castle.Core.Logging.LoggerLevel level)
        {
            throw new NotSupportedException("Logger levels cannot be set at runtime. Please see Serilog's LoggerConfiguration.MinimumLevel.");
        }
    }
}
