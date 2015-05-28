using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using FUT15_JOB;
using FUT15_Center;
using FUT15_Center.Models;
using log4net;

namespace FUT15_JOBConsole
{
    class Program
    {
        static FUT15_Center.FUT15_Center FC;
        static bool stopSign = false;
        static void Main(string[] args)
        {
            JOBStatus JS = new JOBStatus();
            Console.WriteLine("Awaiting all jobs started ...");
            List<Thread> threadList = new List<Thread>();
            int grpNumber = 0;
            //Connect to Center DB
            //Read Group number to get all job accounts.
            //    if args[0] is not  null, then read the group number and start it
            //    if args0] is null, then failed
            if (args.Length == 0)
            {
                Console.WriteLine("Please assgin a group number for this job. Press any key to exit ...");
                Console.ReadKey();
                return;
            }
            else
            {
                grpNumber = Convert.ToInt32(args[0]);
            }

            Console.Title = "FUT15_JOB_" + grpNumber.ToString();

            FC = new FUT15_Center.FUT15_Center(grpNumber);
            var JobUserList = FC.Database.TradeUserAccounts.Where(a => a.JobGroupNumber == grpNumber);
            //loop to start account job threads
            //Job.Start(userid, password, security question)
            //User command "exit" to trigger Job.End()
            int _i = 0;
            foreach (var jobUser in JobUserList)
            {
                _i++;
                Thread t = new Thread(u => Test(jobUser, FC.CurrentJob));
                threadList.Add(t);
                t.Name = jobUser.UserID;
                t.IsBackground = true;
                t.Start();
            }

            Thread.Sleep(1000);
            bool allThreadAlive = false;
            while (!allThreadAlive)
            {
                allThreadAlive = true;
                foreach (var t in threadList)
                {
                    if (!t.IsAlive)
                    {
                        allThreadAlive = false;
                        Thread.Sleep(1000);
                        break;
                    }

                }
            }
            Console.WriteLine("All JOB Threads Started.");
            //Update JOB Status

            string keyin = Console.ReadLine();
            while (keyin.ToLower() != "exit")
            {
                switch(keyin.ToLower())
                {
                    case "exit":
                        break;
                    case "loginstatus":
                        Console.WriteLine("To check every threads' log in status. under construction now.");
                        break;
                    case "job":
                        foreach (var t in threadList)
                        {
                            if (t.IsAlive)
                            {
                                Console.WriteLine("Job thread " + t.Name + " is active.");
                            }
                            else
                            {
                                Console.WriteLine("Job thread " + t.Name + " is down.");
                            }

                        }
                        break;
                    default:
                        Console.WriteLine("An invalid command. Please try it again.");
                        break;
                }
                keyin = Console.ReadLine();
            }
            stopSign = true;
            Console.WriteLine("Awaiting all jobs stopped ...");
            Thread.Sleep(1000);
            //Awaiting all sub threads stopped.
            allThreadAlive = true;
            while (allThreadAlive)
            {
                allThreadAlive = false;
                foreach (var t in threadList)
                {
                    if (t.IsAlive)
                    {
                        allThreadAlive = true;
                        Thread.Sleep(1000);
                        break;
                    }

                }
            }
            Console.WriteLine("All JOB Threads Stopped. Press any key to exit ...");
            Console.ReadKey();

        }

        static async void Test(TradeUserAccount tu, JOBStatus js)
        {
            Console.WriteLine("Thread " + Thread.CurrentThread.Name + " Started.");

            Trade_JOB TJ = new Trade_JOB(tu, js, "gold");
            int i_Result = await TJ.Start();
            Console.WriteLine("Thread " + Thread.CurrentThread.Name + " Stopped. Result(" + i_Result.ToString() + ")");
        }

    }
}
