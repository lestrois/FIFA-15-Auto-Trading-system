using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FUT15_Center.Models;

namespace FUT15_Center
{
    public class FUT15_Center
    {
        public JOBStatus CurrentJob { get; set; }
        public FUT15Entities_CenterDB Database = new FUT15Entities_CenterDB();
        public int Log_Level { get; set; }

        public FUT15_Center()
        {
            CurrentJob = null;
        }

        public FUT15_Center(int grpNumber)
        {
            int MaxJobID = Database.JOBStatuses.Max(a => a.JOBID);
            JOBStatus JS = new JOBStatus();
            JS.JOBID = MaxJobID + 1; 
            JS.JobStartAt = DateTime.Now;
            JS.JobEndAt = null;
            JS.JOBGroupNumber = grpNumber;
            JS.Status = "started";
            Database.JOBStatuses.Add(JS);
            Database.SaveChanges();
            CurrentJob = JS;
        }

        public FUT15_Center(int jobid, int grpNumber)
        {
            var JS = Database.JOBStatuses.FirstOrDefault(a => a.JOBID == jobid && a.JOBGroupNumber == grpNumber);
            CurrentJob = JS;
        }

        public int SendEmail(string emailFrom, string emailTo, string subject, string body)
        {
            try
            {
                System.Net.Mail.MailMessage message = new System.Net.Mail.MailMessage();
                message.To.Add(emailTo);
                message.Subject = subject;
                message.From = new System.Net.Mail.MailAddress(emailFrom);
                message.Body = body;
                System.Net.Mail.SmtpClient smtp = new System.Net.Mail.SmtpClient("smtp.gmail.com", 587);
                smtp.EnableSsl = true;
                smtp.Send(message);
                return 0;
            }
            catch (Exception e)
            {
                return -1;
            }
        }

        public void AddLogToListItem(string Message, int OptionalLevel = 5)
        {
            if (OptionalLevel > Log_Level)
            {
                //lstLog.Items.Add(DateTime.Now.ToString() +": " + Message);
                //lstLog.EnsureVisible(lstLog.Items.Count - 1);
            }
        }

        public void AddLogToFile(string Message, int OptionalLevel = 5)
        {
            if (OptionalLevel > Log_Level)
            {
                //lstLog.Items.Add(DateTime.Now.ToString() + ": " + Message);
                //lstLog.EnsureVisible(lstLog.Items.Count - 1);
            }
        }
    }
}
