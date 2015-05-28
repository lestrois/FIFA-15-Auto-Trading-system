--1. select * from [dbo].[JobPlatforms]
insert into JobPlatforms values(1, 'PC', null, 15, 7);
insert into JobPlatforms values(2, 'PS3', null, 15, 7);
insert into JobPlatforms values(3, 'PS4', null, 15, 7);
insert into JobPlatforms values(4, 'XBox360', null, 15, 7);
insert into JobPlatforms values(5, 'XBoxOne', null, 15, 7);
--2. select * from [TradeUserAccounts]
INSERT INTO [dbo].[TradeUserAccounts]([AccountID], [PlatformID], [PlatformUserID], [UserID], [Password], [SecurityAnswer], [LoginStatus], [JobGroupNumber], [Credit])
VALUES(1,1,1,'lestrois@gmail.com','XXXXXX','tianjin',null, 1, 0);
INSERT INTO [dbo].[TradeUserAccounts]([AccountID], [PlatformID], [PlatformUserID], [UserID], [Password], [SecurityAnswer], [LoginStatus], [JobGroupNumber], [Credit])
VALUES(2,5,1,'lestrois@hotmail.com','OOOOOO','shanghai',null, 1, 0);

--3. select * from [dbo].[TradeConfigurations]
INSERT INTO [dbo].[TradeConfigurations]([AccountID], [Name], [Value], [LastModified])values(1, 'query_interval', '2500', Getdate())
INSERT INTO [dbo].[TradeConfigurations]([AccountID], [Name], [Value], [LastModified])values(1, 'bid_interval', '2500', Getdate())
INSERT INTO [dbo].[TradeConfigurations]([AccountID], [Name], [Value], [LastModified])values(1, 'relist_interval_mins', '5', Getdate())
INSERT INTO [dbo].[TradeConfigurations]([AccountID], [Name], [Value], [LastModified])values(1, 'max_singleplayers_tobid', '4', Getdate())
INSERT INTO [dbo].[TradeConfigurations]([AccountID], [Name], [Value], [LastModified])values(1, 'coins_limit', '10000', Getdate())
INSERT INTO [dbo].[TradeConfigurations]([AccountID], [Name], [Value], [LastModified])values(1, 'expired_hour', '1', Getdate())
INSERT INTO [dbo].[TradeConfigurations]([AccountID], [Name], [Value], [LastModified])values(1, 'bid_in_seconds', '180', Getdate())
INSERT INTO [dbo].[TradeConfigurations]([AccountID], [Name], [Value], [LastModified])values(1, 'urgent_bid_in_seconds', '45', Getdate())
INSERT INTO [dbo].[TradeConfigurations]([AccountID], [Name], [Value], [LastModified])values(1, 'enable_bin', 'N', Getdate())
INSERT INTO [dbo].[TradeConfigurations]([AccountID], [Name], [Value], [LastModified])values(1, 'enable_login_app_start', 'N', Getdate())
INSERT INTO [dbo].[TradeConfigurations]([AccountID], [Name], [Value], [LastModified])values(1, 'low_price_to_avoid_collect', '5000', Getdate())
INSERT INTO [dbo].[TradeConfigurations]([AccountID], [Name], [Value], [LastModified])values(1, 'Log_Level', '0', Getdate())
INSERT INTO [dbo].[TradeConfigurations]([AccountID], [Name], [Value], [LastModified])values(1, 'PriceLimit', '1000000', Getdate())
INSERT INTO [dbo].[TradeConfigurations]([AccountID], [Name], [Value], [LastModified])values(1, 'StopAutoRelist', 'N', Getdate())
INSERT INTO [dbo].[TradeConfigurations]([AccountID], [Name], [Value], [LastModified])values(1, 'AutoListLatestTargetPrice', 'N', Getdate())
INSERT INTO [dbo].[TradeConfigurations]([AccountID], [Name], [Value], [LastModified])values(1, 'DataCollectionWhileBidding', 'Y', Getdate())
INSERT INTO [dbo].[TradeConfigurations]([AccountID], [Name], [Value], [LastModified])values(2, 'query_interval', '2500', Getdate())
INSERT INTO [dbo].[TradeConfigurations]([AccountID], [Name], [Value], [LastModified])values(2, 'bid_interval', '2500', Getdate())
INSERT INTO [dbo].[TradeConfigurations]([AccountID], [Name], [Value], [LastModified])values(2, 'relist_interval_mins', '5', Getdate())
INSERT INTO [dbo].[TradeConfigurations]([AccountID], [Name], [Value], [LastModified])values(2, 'max_singleplayers_tobid', '4', Getdate())
INSERT INTO [dbo].[TradeConfigurations]([AccountID], [Name], [Value], [LastModified])values(2, 'coins_limit', '500', Getdate())
INSERT INTO [dbo].[TradeConfigurations]([AccountID], [Name], [Value], [LastModified])values(2, 'expired_hour', '1', Getdate())
INSERT INTO [dbo].[TradeConfigurations]([AccountID], [Name], [Value], [LastModified])values(2, 'bid_in_seconds', '180', Getdate())
INSERT INTO [dbo].[TradeConfigurations]([AccountID], [Name], [Value], [LastModified])values(2, 'urgent_bid_in_seconds', '45', Getdate())
INSERT INTO [dbo].[TradeConfigurations]([AccountID], [Name], [Value], [LastModified])values(2, 'enable_bin', 'N', Getdate())
INSERT INTO [dbo].[TradeConfigurations]([AccountID], [Name], [Value], [LastModified])values(2, 'enable_login_app_start', 'N', Getdate())
INSERT INTO [dbo].[TradeConfigurations]([AccountID], [Name], [Value], [LastModified])values(2, 'low_price_to_avoid_collect', '150', Getdate())
INSERT INTO [dbo].[TradeConfigurations]([AccountID], [Name], [Value], [LastModified])values(2, 'Log_Level', '0', Getdate())
INSERT INTO [dbo].[TradeConfigurations]([AccountID], [Name], [Value], [LastModified])values(2, 'PriceLimit', '1000000', Getdate())
INSERT INTO [dbo].[TradeConfigurations]([AccountID], [Name], [Value], [LastModified])values(2, 'StopAutoRelist', 'N', Getdate())
INSERT INTO [dbo].[TradeConfigurations]([AccountID], [Name], [Value], [LastModified])values(2, 'AutoListLatestTargetPrice', 'N', Getdate())
INSERT INTO [dbo].[TradeConfigurations]([AccountID], [Name], [Value], [LastModified])values(2, 'DataCollectionWhileBidding', 'Y', Getdate())


