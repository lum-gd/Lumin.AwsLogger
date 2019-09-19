﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Amazon.Runtime;

namespace Lumin.AwsLogger
{
    /// <summary>
    /// This class contains all the configuration options for logging messages to AWS. As messages from the application are 
    /// sent to the logger they are queued up in a batch. The batch will be sent when either BatchPushInterval or BatchSizeInBytes
    /// are exceeded.
    /// 
    /// <para>
    /// AWS Credentials are determined using the following steps.
    /// 1) If the Credentials property is set
    /// 2) If the Profile property is set and the can be found
    /// 3) Use the AWS SDK for .NET fall back mechanism to find enviroment credentials.
    /// </para>
    /// </summary>
    public class AWSLoggerConfig : IAWSLoggerConfig
    {
        private int batchSizeInBytes = 102400;
        #region Public Properties

        /// <summary>
        /// Gets and sets the LogGroup property. This is the name of the CloudWatch Logs group where 
        /// streams will be created and log messages written to.
        /// </summary>
        public string LogGroup { get; set; }

        /// <summary>
        /// Gets and sets the Profile property. The profile is used to look up AWS credentials in the profile store.
        /// <para>
        /// For understanding how credentials are determine view the top level documentation for AWSLoggerConfig class.
        /// </para>
        /// </summary>
        public string Profile { get; set; }

        /// <summary>
        /// Gets and sets the ProfilesLocation property. If this is not set the default profile store is used by the AWS SDK for .NET 
        /// to look up credentials. This is most commonly used when you are running an application of on-priemse under a service account.
        /// <para>
        /// For understanding how credentials are determine view the top level documentation for AWSLoggerConfig class.
        /// </para>
        /// </summary>
        public string ProfilesLocation { get; set; }

        /// <summary>
        /// Gets and sets the Credentials property. These are the AWS credentials used by the AWS SDK for .NET to make service calls.
        /// <para>
        /// For understanding how credentials are determine view the top level documentation for AWSLoggerConfig class.
        /// </para>
        /// </summary>
        public AWSCredentials Credentials { get; set; }


        /// <summary>
        /// Gets and sets the Region property. This is the AWS Region that will be used for CloudWatch Logs. If this is not
        /// the AWS SDK for .NET will use its fall back logic to try and determine the region through environment variables and EC2 instance metadata.
        /// If the Region is not set and no region is found by the SDK's fall back logic then an exception will be thrown.
        /// </summary>
        public string Region { get; set; }

        /// <summary>
        /// Gets and sets the BatchPushInterval property. For performance the log messages are sent to AWS in batch sizes. BatchPushInterval 
        /// dictates the frequency of when batches are sent. If either BatchPushInterval or BatchSizeInBytes are exceeded the batch will be sent.
        /// <para>
        /// The default is 3 seconds.
        /// </para>
        /// </summary>
        public TimeSpan BatchPushInterval { get; set; } = TimeSpan.FromMilliseconds(3000);

        /// <summary>
        /// Gets and sets the BatchSizeInBytes property. For performance the log messages are sent to AWS in batch sizes. BatchSizeInBytes 
        /// dictates the total size of the batch in bytes when batches are sent. If either BatchPushInterval or BatchSizeInBytes are exceeded the batch will be sent.
        /// <para>
        /// The default is 100 Kilobytes.
        /// </para>
        /// </summary>
        public int BatchSizeInBytes
        {
            get
            {
                return batchSizeInBytes;
            }
            set
            {
                if (value > Math.Pow(1024, 2))
                {
                    throw new ArgumentException("The events batch size cannot exeed 1MB. https://docs.aws.amazon.com/AmazonCloudWatch/latest/logs/cloudwatch_limits_cwl.html");
                }
                batchSizeInBytes = value;
            }
        }

        /// <summary>
        /// Gets and sets the MaxQueuedMessages property. This specifies the maximum number of log messages that could be stored in-memory. MaxQueuedMessages 
        /// dictates the total number of log messages that can be stored in-memory. If this exceeded, incoming log messages will be dropped.
        /// <para>
        /// The default is 10000.
        /// </para>
        /// </summary>
        public int MaxQueuedMessages { get; set; } = 10000;

        /// <summary>
        /// Internal MonitorSleepTime property. This specifies the timespan after which the Monitor wakes up. MonitorSleepTime 
        /// dictates the timespan after which the Monitor checks the size and time constarint on the batch log event and the existing in-memory buffer for new messages. 
        /// <para>
        /// The value is 500 Milliseconds.
        /// </para>
        /// </summary>
        internal TimeSpan MonitorSleepTime = TimeSpan.FromMilliseconds(500);
        #endregion

        /// <summary>
        /// Default Constructor
        /// </summary>
        public AWSLoggerConfig()
        {
        }

        /// <summary>
        /// Construct instance and sets the LogGroup
        /// </summary>
        /// <param name="logGroup">The CloudWatch Logs log group.</param>
        public AWSLoggerConfig(string logGroup)
        {
            LogGroup = logGroup;
        }

        /// <summary>
        /// Gets and sets the LogStreamNameSuffix property. The LogStreamName consists of an optional user-defined LogStreamNamePrefix (that can be set here)
        /// followed by a DateTimeStamp as the prefix, and a user defined suffix value
        /// The LogstreamName then follows the pattern '[LogStreamNamePrefix]-[DateTime.Now.ToString("yyyy/MM/ddTHH.mm.ss")]-[LogStreamNameSuffix]'
        /// <para>
        /// The default is new a Guid.
        /// </para>
        /// </summary>
        public string LogStreamNameSuffix { get; set; } = Guid.NewGuid().ToString();

        /// <summary>
        /// Gets and sets the LogStreamNamePrefix property. The LogStreamName consists of an optional user-defined LogStreamNamePrefix (that can be set here)
        /// followed by a DateTimeStamp as the prefix, and a user defined suffix value
        /// The LogstreamName then follows the pattern '[LogStreamNamePrefix]-[DateTime.Now.ToString("yyyy/MM/ddTHH.mm.ss")]-[LogStreamNameSuffix]'
        /// <para>
        /// The default is an empty string.
        /// </para>
        /// </summary>
        public string LogStreamNamePrefix { get; set; } = string.Empty;

        #region LibraryError
        static long index = 0;
        static readonly LiminitedConcurrentQueue<LogItem> bag = new LiminitedConcurrentQueue<LogItem>();
        public static void LogLibraryError(string msg)
        {
            bag.Enqueue(new LogItem
            {
                Id = Interlocked.Increment(ref index),
                Time = DateTime.Now,
                Msg = msg
            });
        }
        public static IEnumerable<LogItem> GetLogLibraryErrors()
        {
            return bag.ToArray().Reverse();
        }
        #endregion

        #region LibraryError
        static long errorIndex = 0;
        static readonly LiminitedConcurrentQueue<LogItem> errorBag = new LiminitedConcurrentQueue<LogItem>();
        public static void LogError(string msg)
        {
            errorBag.Enqueue(new LogItem
            {
                Id = Interlocked.Increment(ref errorIndex),
                Time = DateTime.Now,
                Msg = msg
            });
        }
        public static IEnumerable<LogItem> GetLatestErrorLogs()
        {
            return errorBag.ToArray().Reverse();
        }
        #endregion
    }

    [Serializable]
    public class LiminitedConcurrentQueue<T> : ConcurrentQueue<T>
    {
        private int _limit;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="limit"></param>
        public LiminitedConcurrentQueue(int limit = 100)
        {
            this._limit = limit;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        public new void Enqueue(T item)
        {
            if (this.Count >= this._limit)
            {
                T t;
                this.TryDequeue(out t);
            }
            base.Enqueue(item);
        }
    }

    [Serializable]
    public class LogItem
    {
        public long Id { get; set; }
        public DateTime Time { get; set; }
        public string Msg { get; set; }
    }
}
