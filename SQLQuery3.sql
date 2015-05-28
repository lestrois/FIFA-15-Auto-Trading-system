select * from [TradeUserAccounts] 
update [TradeUserAccounts] set JobGroupNumber=3
where AccountID = 47QL_

delete from [TradeUserAccounts] where AccountID> 3
select * from [dbo].[TradeConfigurations]

select * from [dbo].[JobPlatforms]
select * from [dbo].[JOBStatus]
select * from [dbo].[AccountStatus] order by LastModified desc

delete from [TradeConfigurations] where AccountID = 47

insert into [TradeConfigurations]
select 47, name, value, getdate() from TradeConfigurations
where AccountID = 3

