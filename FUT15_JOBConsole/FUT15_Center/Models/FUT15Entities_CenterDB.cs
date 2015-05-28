using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using FUT15_Center;

namespace FUT15_Center.Models
{
    public class FUT15Entities_CenterDB : DbContext
    {
        public DbSet<AccountStatus> AccountStatuses { get; set; }
        public DbSet<JobPlatform> JobPlatforms { get; set; }
        public DbSet<JOBStatus> JOBStatuses { get; set; }
        public DbSet<TradeConfiguration> TradeConfigurations { get; set; }
        public DbSet<TradeUserAccount> TradeUserAccounts { get; set; }
        public DbSet<ErrorLog> ErrorLogs { get; set; }

        public FUT15Entities_CenterDB()
        {
            //System.Data.Entity.Database.SetInitializer(new DBInitializer());
        }
        public void RunSP(string spName)
        {
            //this.Database.SqlQuery<TargetBidInfo>(spName);
            this.Database.ExecuteSqlCommand(spName);
        }

        public void RunSQL(string SQL)
        {
            this.Database.ExecuteSqlCommand(SQL);
            //this.Database.SqlQuery<TargetBidInfo>(SQL);
        }

        public void InitiateTradeConfigData(int accountID)
        {
            this.Database.ExecuteSqlCommand("insert into [TradeConfigurations] " +
                                            "select " + accountID.ToString() + ", name, value, getdate() from TradeConfigurations " +
                                            "where AccountID = 1");
        }

    }

    public class DBInitializer : DropCreateDatabaseAlways<FUT15Entities_CenterDB>
    {
        protected override void Seed(FUT15Entities_CenterDB context)
        {
            IList<TradeUserAccount> tus = new List<TradeUserAccount>();

            TradeUserAccount ta = new TradeUserAccount();
            ta.UserID = "lestrois@gmail.com";
            ta.JobGroupNumber = 1;
            ta.Password = "";
            ta.PlatformID = 1;
            ta.PlatformUserID = 1;
            ta.AccountID = 1;

            tus.Add(ta);
            foreach (TradeUserAccount std in tus)
                context.TradeUserAccounts.Add(std);

            //All standards will
            base.Seed(context);
        }
    }

}