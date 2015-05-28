select a.resourceid, a.avgPrice, b.avgPrice, c.avgPrice
from (
select c.ResourceId, avg(a.CurrentBid) avgPrice
from [dbo].[AuctionInfoes] a, [dbo].[TradeTimes] b, [dbo].[ItemDatas] c, Players d
where a.TradeId = b.Trade_Id
  and a.ItemData_Id = c.id
  and c.ResourceId = d.resourceID
  and datediff(hh, b.Transaction_Time, getdate()) <= 3
group by c.ResourceId
) a,
(
select c.ResourceId, avg(a.CurrentBid) avgPrice
from [dbo].[AuctionInfoes] a, [dbo].[TradeTimes] b, [dbo].[ItemDatas] c, Players d
where a.TradeId = b.Trade_Id
  and a.ItemData_Id = c.id
  and c.ResourceId = d.resourceID
  and datediff(hh, b.Transaction_Time, getdate()) > 3 and datediff(hh, b.Transaction_Time, getdate()) <= 6
group by c.ResourceId
) b,
(
select c.ResourceId, avg(a.CurrentBid) avgPrice
from [dbo].[AuctionInfoes] a, [dbo].[TradeTimes] b, [dbo].[ItemDatas] c, Players d
where a.TradeId = b.Trade_Id
  and a.ItemData_Id = c.id
  and c.ResourceId = d.resourceID
  and datediff(hh, b.Transaction_Time, getdate()) > 6 and datediff(hh, b.Transaction_Time, getdate()) <= 9
group by c.ResourceId
) c
where a.resourceid = b.resourceid and a.resourceid=c.resourceid