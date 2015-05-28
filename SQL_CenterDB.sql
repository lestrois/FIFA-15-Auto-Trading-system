select tua.*, jp.PlatformName, acs.CoinsInHand, acs.AuctionValue, acs.LastModified, j.jobid, acs.SessionID, acs.PhishingToken
from [TradeUserAccounts] tua,
[JOBStatus] j, 
(
 select a.* from [AccountStatus] a,
 (select AccountID, max(LastModified) md from [AccountStatus] group by AccountID) b
 where a.AccountID = b.AccountID and a.LastModified = b.md
) acs, JobPlatforms jp
where acs.JobID = j.JobID and tua.AccountID = acs.AccountID and jp.PlatformID = tua.PlatformID
order by LastModified desc
--
select *
from [dbo].[TradeConfigurations]
--where AccountID = 1 and name = 'AutoListLatestTargetPrice'
order by AccountID, id


select distinct accountid from [TradeConfigurations]
 select a.* from [AccountStatus] a
 where a.AccountID = 27
 order by a.LastModified desc
 update [AccountStatus] set PhishingToken='1119776865170629209', SessionID='3b7a8289-f732-41d6-aedd-d308234c4f26' where id = 1944

--select * from [dbo].[JobPlatforms]
--insert into [TradeConfigurations]
--select 47, name, value, getdate() from TradeConfigurations
--where AccountID = 3
--update [TradeConfigurations] set value='y' where AccountID = 1 and name = 'AutoListLatestTargetPrice'
--where AccountID = 0
--update [TradeUserAccounts] set password = '' where accountid = 48
--select * from TradeUserAccounts where JobGroupNumber <> 0
--insert into [TradeConfigurations]
--select 35, name, value, getdate() from TradeConfigurations
--where AccountID = 1
select * from TradeUserAccounts order by PlatformID, AccountID
update TradeUserAccounts set MailPassword='XXXXXX' where AccountID=26
update TradeUserAccounts set PlatformID=5, JobGroupNumber=11 where AccountID=32
--update TradeUserAccounts set userid = REPLACE(userid, 'GMAL', 'gmail')
select * from TradeUserAccounts order by PlatformID, AccountID

select * from JobPlatforms
--update JobPlatforms set marketadjuster = 1
select * from [dbo].[JOBStatus]
order by id desc

select tua.accountid, tua.userid, tua.loginstatus, tua.jobgroupnumber, tua.credit totalvalue,
isnull(acs.CoinsInHand,0) CoinsInHand, isnull(acs.AuctionValue,0) AuctionValue, acs.LastModified, isnull(j.jobid,-1) jobid, acs.SessionID, acs.PhishingToken, jp.PlatformName Platform
from [TradeUserAccounts] tua
left join (
 select a.* from [AccountStatus] a,
 (select AccountID, max(LastModified) md from [AccountStatus] group by AccountID) b
 where a.AccountID = b.AccountID and a.LastModified = b.md
) acs on tua.AccountID = acs.AccountID
left join JobPlatforms jp on jp.PlatformID = tua.PlatformID
left join [JOBStatus] j on acs.JobID = j.JobID
where tua.jobgroupnumber <> 0
order by LastModified desc

