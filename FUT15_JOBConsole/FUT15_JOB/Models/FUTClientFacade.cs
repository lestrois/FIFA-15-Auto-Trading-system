using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using UltimateTeam.Toolkit.Extensions;
using UltimateTeam.Toolkit.Factories;
using UltimateTeam.Toolkit.Models;
using UltimateTeam.Toolkit.Parameters;
using UltimateTeam.Toolkit.Constants;
using UltimateTeam.Toolkit.Requests;
using UltimateTeam.Toolkit;
using System.Net;


namespace FUT15_JOB.Models
{
    public class FUTClientFacade
    {
        public FutClient fc;
        private object locker = new object();
        private FUT15Configs fut15_config;
        public FUTClientFacade(FUT15Configs config)
        {
            fc = new FutClient();
            fut15_config = config;
        }
        public Task<LoginResponse> LoginAsync(LoginDetails loginDetails)
        {
            lock (locker)
            {
                //Thread.Sleep(fut15_config.Bid_Interval);
                //Task.Delay(fut15_config.Bid_Interval);
                return fc.LoginAsync(loginDetails);
            }
        }

        public void UpdateToken(string sessionID, string token, Platform platform)
        {
            lock (locker)
            {
                fc.RequestFactories.PhishingToken = token;
                fc.RequestFactories.SessionId = sessionID;
                if(platform == Platform.Xbox360)
                    fc.RequestFactories.FUTHome ="https://utas.fut.ea.com/ut/game/fifa15/";
                else
                    fc.RequestFactories.FUTHome = "https://utas.s2.fut.ea.com/ut/game/fifa15/";
            }
        }


        public Task<AuctionResponse> SearchAsync(SearchParameters searchParameters)
        {
            lock (locker)
            {
                //Thread.Sleep(fut15_config.Bid_Interval);
                //Task.Delay(fut15_config.Query_Interval);
                return fc.SearchAsync(searchParameters);
            }
        }

        public Task<AuctionResponse> PlaceBidAsync(AuctionInfo auctionInfo, long bidAmount = 0)
        {
            lock (locker)
            {
                //Thread.Sleep(fut15_config.Bid_Interval);
                //Task.Delay(fut15_config.Bid_Interval);
                return fc.PlaceBidAsync(auctionInfo, bidAmount);
            }
        }

        public Task<Item> GetItemAsync(AuctionInfo auctionInfo)
        {
            lock (locker)
            {
                //Thread.Sleep(fut15_config.Bid_Interval);
                //Task.Delay(fut15_config.Bid_Interval);
                return fc.GetItemAsync(auctionInfo);
            }
        }

        public Task<Item> GetItemAsync(long resourceId)
        {
            lock (locker)
            {
                //Thread.Sleep(fut15_config.Bid_Interval);
                //Task.Delay(fut15_config.Bid_Interval);
                return fc.GetItemAsync(resourceId);
            }
        }

        public Task<byte[]> GetPlayerImageAsync(AuctionInfo auctionInfo)
        {
            lock (locker)
            {
                //Task.Delay(Common.Bid_Interval);
                return fc.GetPlayerImageAsync(auctionInfo);
            }

        }

        public Task<AuctionResponse> GetTradeStatusAsync(IEnumerable<long> tradeIds)
        {
            lock (locker)
            {
                //Thread.Sleep(fut15_config.Bid_Interval);
                //Task.Delay(fut15_config.Bid_Interval);

                return fc.GetTradeStatusAsync(tradeIds);
            }
        }

        public Task<CreditsResponse> GetCreditsAsync()
        {            lock (locker)
            {
                //Thread.Sleep(fut15_config.Bid_Interval);
                //Task.Delay(fut15_config.Bid_Interval);
                return fc.GetCreditsAsync();
            }

        }

        public Task<PileSizeResponse> GetPileSizeAsync()
        {            lock (locker)
            {
                //Thread.Sleep(fut15_config.Bid_Interval);
                //Task.Delay(fut15_config.Bid_Interval);

            return fc.GetPileSizeAsync();
            }
        }

        public Task<ConsumablesResponse> GetConsumablesAsync()
        {            lock (locker)
            {
                //Thread.Sleep(fut15_config.Bid_Interval);
                //Task.Delay(fut15_config.Bid_Interval);

                return fc.GetConsumablesAsync();
            }
        }

        public Task<AuctionResponse> GetTradePileAsync()
        {            lock (locker)
            {
                //Thread.Sleep(fut15_config.Bid_Interval);
                //Task.Delay(fut15_config.Bid_Interval);
                return fc.GetTradePileAsync();
            }

        }

        public Task<WatchlistResponse> GetWatchlistAsync()
        {            lock (locker)
            {
                //Thread.Sleep(fut15_config.Bid_Interval);
                //Task.Delay(fut15_config.Bid_Interval);
            return fc.GetWatchlistAsync();
            }

        }

        public Task<ClubItemResponse> GetClubItemsAsync()
        {            lock (locker)
            {
                //Thread.Sleep(fut15_config.Bid_Interval);
                //Task.Delay(fut15_config.Bid_Interval);
            return fc.GetClubItemsAsync();
            }

        }

        public Task<SquadListResponse> GetSquadListAsync()
        {            lock (locker)
            {
                //Thread.Sleep(fut15_config.Bid_Interval);
                //Task.Delay(fut15_config.Bid_Interval);

            return fc.GetSquadListAsync();
            }
        }

        public Task<PurchasedItemsResponse> GetPurchasedItemsAsync()
        {
                    lock (locker)
            {
                //Thread.Sleep(fut15_config.Bid_Interval);
                //Task.Delay(fut15_config.Bid_Interval);
                return GetPurchasedItemsAsync();
            }

        }

        public Task<ListAuctionResponse> ListAuctionAsync(AuctionDetails auctionDetails)
        {
            lock (locker)
            {
                //Thread.Sleep(fut15_config.Bid_Interval);
                //Task.Delay(fut15_config.Bid_Interval);
                return fc.ListAuctionAsync(auctionDetails);
            }

        }

        public Task AddToWatchlistRequestAsync(IEnumerable<AuctionInfo> auctionInfo)
        {
            lock (locker)
            {
                //Thread.Sleep(fut15_config.Bid_Interval);
                //Task.Delay(fut15_config.Bid_Interval);
                return fc.AddToWatchlistRequestAsync(auctionInfo);
            }

        }

        public Task AddToWatchlistRequestAsync(AuctionInfo auctionInfo)
        {
            lock (locker)
            {
                //Thread.Sleep(fut15_config.Bid_Interval);
                //Task.Delay(fut15_config.Bid_Interval);
                return fc.AddToWatchlistRequestAsync(auctionInfo);
            }

        }

        public Task RemoveFromWatchlistAsync(IEnumerable<AuctionInfo> auctionInfo)
        {
            lock (locker)
            {
                //Thread.Sleep(fut15_config.Bid_Interval);
                //Task.Delay(fut15_config.Bid_Interval);
                return fc.RemoveFromWatchlistAsync(auctionInfo);
            }

        }

        public Task RemoveFromWatchlistAsync(AuctionInfo auctionInfo)
        {
            lock (locker)
            {
                //Thread.Sleep(fut15_config.Bid_Interval);
                //Task.Delay(fut15_config.Bid_Interval);
                return RemoveFromWatchlistAsync(new[] { auctionInfo });
            }

        }

        public Task RemoveFromTradePileAsync(AuctionInfo auctionInfo)
        {
            lock (locker)
            {
                //Thread.Sleep(fut15_config.Bid_Interval);
                //Task.Delay(fut15_config.Bid_Interval);
                return fc.RemoveFromTradePileAsync(auctionInfo);
            }

        }

        public Task<SquadDetailsResponse> GetSquadDetailsAsync(ushort squadId)
        {
            lock (locker)
            {
                //Thread.Sleep(fut15_config.Bid_Interval);
                //Task.Delay(fut15_config.Bid_Interval);
                return fc.GetSquadDetailsAsync(squadId);
            }

        }

        public Task<SendItemToClubResponse> SendItemToClubAsync(ItemData itemData)
        {
            lock (locker)
            {
                //Thread.Sleep(fut15_config.Bid_Interval);
                //Task.Delay(fut15_config.Bid_Interval);
                return fc.SendItemToClubAsync(itemData);
            }

        }

        public Task<SendItemToTradePileResponse> SendItemToTradePileAsync(ItemData itemData)
        {
            lock (locker)
            {
                //Thread.Sleep(fut15_config.Bid_Interval);
                //Task.Delay(fut15_config.Bid_Interval);
            return fc.SendItemToTradePileAsync(itemData);
            }

        }

        public Task<QuickSellResponse> QuickSellItemAsync(long itemId)
        {
            lock (locker)
            {
                //Thread.Sleep(fut15_config.Bid_Interval);
                //Task.Delay(fut15_config.Bid_Interval);
            return fc.QuickSellItemAsync(itemId);
            }

        }

        public Task<QuickSellResponse> QuickSellItemAsync(IEnumerable<long> itemIds)
        {
            lock (locker)
            {
                //Thread.Sleep(fut15_config.Bid_Interval);
                //Task.Delay(fut15_config.Bid_Interval);

            return fc.QuickSellItemAsync(itemIds);
            }
        }

        public Task<byte[]> GetClubImageAsync(AuctionInfo auctionInfo)
        {
            lock (locker)
            {
                //Thread.Sleep(fut15_config.Bid_Interval);
                //Task.Delay(fut15_config.Bid_Interval);
            return fc.GetClubImageAsync(auctionInfo);
            }

        }

        public Task<byte[]> GetNationImageAsync(Item item)
        {
            lock (locker)
            {
                //Thread.Sleep(fut15_config.Bid_Interval);
                //Task.Delay(fut15_config.Bid_Interval);
            return fc.GetNationImageAsync(item);
            }
        }

        public Task<byte> ReListAsync()
        {
            lock (locker)
            {
                //Thread.Sleep(fut15_config.Bid_Interval);
                //Task.Delay(fut15_config.Bid_Interval);
            return fc.ReListAsync();
            }
        }
    }
}
