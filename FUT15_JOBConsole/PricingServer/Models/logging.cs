using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net;
using log4net.Config;

namespace FUT15_PricingServer.Models
{
    public enum LogLevelL4N
    {
        DEBUG = 1,
        ERROR,
        FATAL,
        INFO,
        WARN
    }

    public class Logger
    {
        #region Members
        private readonly ILog logger = LogManager.GetLogger(typeof(Logger));
        private int lineCounter;
        #endregion

        #region Constructors
        private string UserID;
        private string logFile;

        public Logger()
        {
            lineCounter = 0;
            logFile = "PricingServer-" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".log";
            log4net.ThreadContext.Properties["LogName"] = logFile;
            XmlConfigurator.Configure();
        }
        
        public Logger(string userid)
        {
            lineCounter = 0;
            UserID = userid;
            logFile = UserID + "-" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".log";
            log4net.ThreadContext.Properties["LogName"] = logFile;
            XmlConfigurator.Configure();
        }
        #endregion

        #region Methods

        public void WriteLog(LogLevelL4N logLevel, String log)
        {
            lineCounter++;
            if (lineCounter > 60000)
            {
                logFile = UserID + "-" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".log";
                log4net.ThreadContext.Properties["LogName"] = logFile;
                XmlConfigurator.Configure();
                lineCounter = 0;
            }
            else
            {
                log4net.ThreadContext.Properties["LogName"] = logFile;
                XmlConfigurator.Configure();
            }

            if (logLevel.Equals(LogLevelL4N.DEBUG))
            {
                logger.Debug(log);
            }

            else if (logLevel.Equals(LogLevelL4N.ERROR))
            {
                logger.Error(log);
            }

            else if (logLevel.Equals(LogLevelL4N.FATAL))
            {
                logger.Fatal(log);
            }

            else if (logLevel.Equals(LogLevelL4N.INFO))
            {
                logger.Info(log);
            }

            else if (logLevel.Equals(LogLevelL4N.WARN))
            {

                logger.Warn(log);

            }

        }

        #endregion

    }

}