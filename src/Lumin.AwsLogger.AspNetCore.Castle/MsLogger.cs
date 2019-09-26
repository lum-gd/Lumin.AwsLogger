using System;
using Microsoft.Extensions.Logging;

namespace Lumin.AwsLogger.AspNetCore.Castle
{
    public class MsLogger : global::Castle.Core.Logging.ILogger
    {
        public MsLogger(ILogger logger, MsFactory factory)
        {
            Logger = logger;
            Factory = factory;
        }

        internal MsLogger() { }

        protected internal ILogger Logger { get; set; }

        protected internal MsFactory Factory { get; set; }

        public bool IsTraceEnabled
        {
            get { return Logger.IsEnabled(LogLevel.Trace); }
        }

        public bool IsDebugEnabled
        {
            get { return Logger.IsEnabled(LogLevel.Debug); }
        }

        public bool IsErrorEnabled
        {
            get { return Logger.IsEnabled(LogLevel.Error); }
        }

        public bool IsFatalEnabled
        {
            get { return Logger.IsEnabled(LogLevel.Critical); }
        }

        public bool IsInfoEnabled
        {
            get { return Logger.IsEnabled(LogLevel.Information); }
        }

        public bool IsWarnEnabled
        {
            get { return Logger.IsEnabled(LogLevel.Warning); }
        }

        public override string ToString()
        {
            return Logger.ToString();
        }

        public global::Castle.Core.Logging.ILogger CreateChildLogger(string loggerName)
        {
            // Serilog calls these sub loggers. We might be able to do something here but for now I'm going leave it like this.
            throw new NotImplementedException("Creating child loggers for Serilog is not supported");
        }

        public void Trace(string message, Exception exception)
        {
            Logger.LogTrace(exception, message);
        }

        public void Trace(Func<string> messageFactory)
        {
            if (IsTraceEnabled)
            {
                Logger.LogTrace(messageFactory.Invoke());
            }
        }

        public void Trace(string message)
        {
            if (IsTraceEnabled)
            {
                Logger.LogTrace(message);
            }
        }

        public void TraceFormat(Exception exception, IFormatProvider formatProvider, string format, params object[] args)
        {
            if (IsTraceEnabled)
            {
                //TODO: This honours the formatProvider rather than passing through args for structured logging
                Logger.LogTrace(exception, string.Format(formatProvider, format, args));
            }
        }

        public void TraceFormat(IFormatProvider formatProvider, string format, params object[] args)
        {
            if (IsTraceEnabled)
            {
                //TODO: This honours the formatProvider rather than passing through args for structured logging
                Logger.LogTrace(string.Format(formatProvider, format, args));
            }
        }

        public void TraceFormat(Exception exception, string format, params object[] args)
        {
            if (IsTraceEnabled)
            {
                Logger.LogTrace(exception, format, args);
            }
        }

        public void TraceFormat(string format, params object[] args)
        {
            if (IsTraceEnabled)
            {
                Logger.LogTrace(format, args);
            }
        }

        public void Debug(string message, Exception exception)
        {
            if (IsDebugEnabled)
            {
                Logger.LogDebug(exception, message);
            }
        }

        public void Debug(Func<string> messageFactory)
        {
            if (IsDebugEnabled)
            {
                Logger.LogDebug(messageFactory.Invoke());
            }
        }

        public void Debug(string message)
        {
            if (IsDebugEnabled)
            {
                Logger.LogDebug(message);
            }
        }

        public void DebugFormat(Exception exception, IFormatProvider formatProvider, string format, params object[] args)
        {
            if (IsDebugEnabled)
            {
                //TODO: This honours the formatProvider rather than passing through args for structured logging
                Logger.LogDebug(exception, string.Format(formatProvider, format, args));
            }
        }

        public void DebugFormat(IFormatProvider formatProvider, string format, params object[] args)
        {
            if (IsDebugEnabled)
            {
                //TODO: This honours the formatProvider rather than passing through args for structured logging
                Logger.LogDebug(string.Format(formatProvider, format, args));
            }
        }

        public void DebugFormat(Exception exception, string format, params object[] args)
        {
            if (IsDebugEnabled)
            {
                Logger.LogDebug(exception, format, args);
            }
        }

        public void DebugFormat(string format, params object[] args)
        {
            if (IsDebugEnabled)
            {
                Logger.LogDebug(format, args);
            }
        }

        public void Error(string message, Exception exception)
        {
            if (IsErrorEnabled)
            {
                Logger.LogError(exception, message);
            }
        }

        public void Error(Func<string> messageFactory)
        {
            if (IsErrorEnabled)
            {
                Logger.LogError(messageFactory.Invoke());
            }
        }

        public void Error(string message)
        {
            if (IsErrorEnabled)
            {
                Logger.LogError(message);
            }
        }

        public void ErrorFormat(Exception exception, IFormatProvider formatProvider, string format, params object[] args)
        {
            if (IsErrorEnabled)
            {
                //TODO: This honours the formatProvider rather than passing through args for structured logging
                Logger.LogError(exception, string.Format(formatProvider, format, args));
            }
        }

        public void ErrorFormat(IFormatProvider formatProvider, string format, params object[] args)
        {
            if (IsErrorEnabled)
            {
                //TODO: This honours the formatProvider rather than passing through args for structured logging
                Logger.LogError(string.Format(formatProvider, format, args));
            }
        }

        public void ErrorFormat(Exception exception, string format, params object[] args)
        {
            if (IsErrorEnabled)
            {
                Logger.LogError(exception, format, args);
            }
        }

        public void ErrorFormat(string format, params object[] args)
        {
            if (IsErrorEnabled)
            {
                Logger.LogError(format, args);
            }
        }

        public void Fatal(string message, Exception exception)
        {
            if (IsFatalEnabled)
            {
                Logger.LogCritical(exception, message);
            }
        }

        public void Fatal(Func<string> messageFactory)
        {
            if (IsFatalEnabled)
            {
                Logger.LogCritical(messageFactory.Invoke());
            }
        }

        public void Fatal(string message)
        {
            if (IsFatalEnabled)
            {
                Logger.LogCritical(message);
            }
        }

        public void FatalFormat(Exception exception, IFormatProvider formatProvider, string format, params object[] args)
        {
            if (IsFatalEnabled)
            {
                //TODO: This honours the formatProvider rather than passing through args for structured logging
                Logger.LogCritical(exception, string.Format(formatProvider, format, args));
            }
        }

        public void FatalFormat(IFormatProvider formatProvider, string format, params object[] args)
        {
            if (IsFatalEnabled)
            {
                //TODO: This honours the formatProvider rather than passing through args for structured logging
                Logger.LogCritical(string.Format(formatProvider, format, args));
            }
        }

        public void FatalFormat(Exception exception, string format, params object[] args)
        {
            if (IsFatalEnabled)
            {
                Logger.LogCritical(exception, format, args);
            }
        }

        public void FatalFormat(string format, params object[] args)
        {
            if (IsFatalEnabled)
            {
                Logger.LogCritical(format, args);
            }
        }

        public void Info(string message, Exception exception)
        {
            if (IsInfoEnabled)
            {
                Logger.LogInformation(exception, message);
            }
        }

        public void Info(Func<string> messageFactory)
        {
            if (IsInfoEnabled)
            {
                Logger.LogInformation(messageFactory.Invoke());
            }
        }

        public void Info(string message)
        {
            if (IsInfoEnabled)
            {
                Logger.LogInformation(message);
            }
        }

        public void InfoFormat(Exception exception, IFormatProvider formatProvider, string format, params object[] args)
        {
            if (IsInfoEnabled)
            {
                //TODO: This honours the formatProvider rather than passing through args for structured logging
                Logger.LogInformation(exception, string.Format(formatProvider, format, args));
            }
        }

        public void InfoFormat(IFormatProvider formatProvider, string format, params object[] args)
        {
            if (IsInfoEnabled)
            {
                //TODO: This honours the formatProvider rather than passing through args for structured logging
                Logger.LogInformation(string.Format(formatProvider, format, args));
            }
        }

        public void InfoFormat(Exception exception, string format, params object[] args)
        {
            if (IsInfoEnabled)
            {
                Logger.LogInformation(exception, format, args);
            }
        }

        public void InfoFormat(string format, params object[] args)
        {
            if (IsInfoEnabled)
            {
                Logger.LogInformation(format, args);
            }
        }

        public void Warn(string message, Exception exception)
        {
            if (IsWarnEnabled)
            {
                Logger.LogWarning(exception, message);
            }
        }

        public void Warn(Func<string> messageFactory)
        {
            if (IsWarnEnabled)
            {
                Logger.LogWarning(messageFactory.Invoke());
            }
        }

        public void Warn(string message)
        {
            if (IsWarnEnabled)
            {
                Logger.LogWarning(message);
            }
        }

        public void WarnFormat(Exception exception, IFormatProvider formatProvider, string format, params object[] args)
        {
            if (IsWarnEnabled)
            {
                //TODO: This honours the formatProvider rather than passing through args for structured logging
                Logger.LogWarning(exception, string.Format(formatProvider, format, args));
            }
        }

        public void WarnFormat(IFormatProvider formatProvider, string format, params object[] args)
        {
            if (IsWarnEnabled)
            {
                //TODO: This honours the formatProvider rather than passing through args for structured logging
                Logger.LogWarning(string.Format(formatProvider, format, args));
            }
        }

        public void WarnFormat(Exception exception, string format, params object[] args)
        {
            if (IsWarnEnabled)
            {
                Logger.LogWarning(exception, format, args);
            }
        }

        public void WarnFormat(string format, params object[] args)
        {
            if (IsWarnEnabled)
            {
                Logger.LogWarning(format, args);
            }
        }
    }
}
