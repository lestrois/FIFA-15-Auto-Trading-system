﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UltimateTeam.Toolkit.Extensions;
using UltimateTeam.Toolkit.Factories;
using UltimateTeam.Toolkit.Models;
using UltimateTeam.Toolkit.Parameters;
using UltimateTeam.Toolkit.Constants;
using UltimateTeam.Toolkit.Requests;
using System.Net.Http;
using System.Net;

namespace UltimateTeam.Toolkit
{
    public class FutClient : IFutClient
    {
        private readonly FutRequestFactories _requestFactories = new FutRequestFactories();

        public FutRequestFactories RequestFactories { get { return _requestFactories; } }

        public async Task<LoginResponse> LoginAsync(LoginDetails loginDetails)
        {
            loginDetails.ThrowIfNullArgument();

            var loginRequest = _requestFactories.LoginRequestFactory(loginDetails);
            var loginResponse = await loginRequest.PerformRequestAsync();
            RequestFactories.PhishingToken = loginResponse.PhishingToken;
            RequestFactories.SessionId = loginResponse.SessionId;

            return loginResponse;
        }

        public void UpdateToken(string sessionID, string token)
        {
            RequestFactories.PhishingToken = token;
            RequestFactories.SessionId = sessionID;
        }

        public void UpdateCookie(CookieContainer _cookie)
        {
            RequestFactories._cookieContainer = _cookie;
        }

        public Task<AuctionResponse> SearchAsync(SearchParameters searchParameters)
        {
            searchParameters.ThrowIfNullArgument();

            return _requestFactories.SearchRequestFactory(searchParameters).PerformRequestAsync();
        }

        public Task<AuctionResponse> PlaceBidAsync(AuctionInfo auctionInfo, long bidAmount = 0)
        {
            auctionInfo.ThrowIfNullArgument();

            if (bidAmount == 0)
            {
                bidAmount = auctionInfo.CalculateBid();
            }

            return _requestFactories.PlaceBidRequestFactory(auctionInfo, bidAmount).PerformRequestAsync();
        }

        public Task<Item> GetItemAsync(AuctionInfo auctionInfo)
        {
            auctionInfo.ThrowIfNullArgument();

            return _requestFactories.ItemRequestFactory(auctionInfo.CalculateBaseId()).PerformRequestAsync();
        }

        public Task<Item> GetItemAsync(long resourceId)
        {
            if (resourceId < 1) throw new ArgumentException("Definitely not valid", "resourceId");

            return _requestFactories.ItemRequestFactory(resourceId.CalculateBaseId()).PerformRequestAsync();
        }

        public Task<byte[]> GetPlayerImageAsync(AuctionInfo auctionInfo)
        {
            auctionInfo.ThrowIfNullArgument();

            return _requestFactories.PlayerImageRequestFactory(auctionInfo).PerformRequestAsync();
        }

        //public async Task<byte[]> GetPlayerImageAsync(long resourceId)
        //{
        //    resourceId.ThrowIfNullArgument();
        //    IHttpClient _httpClient;
        //    HttpClientHandler MessageHandler = new HttpClientHandler { AutomaticDecompression = DecompressionMethods.GZip };
        //    _httpClient = new HttpClient(MessageHandler);
        //    _httpClient.DefaultRequestHeaders.ExpectContinue = false;
        //    _httpClient.AddRequestHeader(HttpHeaders.UserAgent, "Mozilla/5.0 (Windows NT 6.2; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/29.0.1547.62 Safari/537.36");
        //    _httpClient.AddRequestHeader(HttpHeaders.Accept, "*/*");
        //    _httpClient.SetReferrerUri(Resources.BaseShowoff);
        //    _httpClient.AddRequestHeader(HttpHeaders.AcceptEncoding, "gzip,deflate,sdch");
        //    _httpClient.AddRequestHeader(HttpHeaders.AcceptLanguage, "en-US,en;q=0.8");

        //    return await _httpClient
        //        .GetByteArrayAsync(string.Format(Resources.PlayerImage, resourceId.CalculateBaseId()))
        //        .ConfigureAwait(false);
        //}

        public Task<AuctionResponse> GetTradeStatusAsync(IEnumerable<long> tradeIds)
        {
            tradeIds.ThrowIfNullArgument();

            return _requestFactories.TradeStatusRequestFactory(tradeIds).PerformRequestAsync();
        }

        public Task<CreditsResponse> GetCreditsAsync()
        {
            return _requestFactories.CreditsRequestFactory().PerformRequestAsync();
        }

        public Task<PileSizeResponse> GetPileSizeAsync()
        {
            return _requestFactories.PileSizeRequestFactory().PerformRequestAsync();
        }

        public Task<ConsumablesResponse> GetConsumablesAsync()
        {
            return _requestFactories.ConsumablesRequestFactory().PerformRequestAsync();
        }
        
        public Task<AuctionResponse> GetTradePileAsync()
        {
            return _requestFactories.TradePileRequestFactory().PerformRequestAsync();
        }

        public Task<WatchlistResponse> GetWatchlistAsync()
        {
            return _requestFactories.WatchlistRequestFactory().PerformRequestAsync();
        }

        public Task<ClubItemResponse> GetClubItemsAsync()
        {
            return _requestFactories.ClubItemRequestFactory().PerformRequestAsync();
        }

        public Task<SquadListResponse> GetSquadListAsync()
        {
            return _requestFactories.SquadListRequestFactory().PerformRequestAsync();
        }
        
        public Task<PurchasedItemsResponse> GetPurchasedItemsAsync()
        {
            return _requestFactories.PurchasedItemsRequestFactory().PerformRequestAsync();
        }

        public Task<ListAuctionResponse> ListAuctionAsync(AuctionDetails auctionDetails)
        {
            auctionDetails.ThrowIfNullArgument();
            return _requestFactories.ListAuctionFactory(auctionDetails).PerformRequestAsync();
        }

        public Task AddToWatchlistRequestAsync(IEnumerable<AuctionInfo> auctionInfo)
        {
            auctionInfo.ThrowIfNullArgument();
            return _requestFactories.AddToWatchlistRequestFactory(auctionInfo).PerformRequestAsync();
        }

        public Task AddToWatchlistRequestAsync(AuctionInfo auctionInfo)
        {
            return AddToWatchlistRequestAsync(new [] { auctionInfo });
        }

        public Task RemoveFromWatchlistAsync(IEnumerable<AuctionInfo> auctionInfo)
        {
            auctionInfo.ThrowIfNullArgument();
            return _requestFactories.RemoveFromWatchlistRequestFactory(auctionInfo).PerformRequestAsync();
        }

        public Task RemoveFromWatchlistAsync(AuctionInfo auctionInfo)
        {
            return RemoveFromWatchlistAsync(new[] { auctionInfo });
        }

        public Task RemoveFromTradePileAsync(AuctionInfo auctionInfo)
        {
            auctionInfo.ThrowIfNullArgument();
            return _requestFactories.RemoveFromTradePileRequestFactory(auctionInfo).PerformRequestAsync();
        }

        public Task<SquadDetailsResponse> GetSquadDetailsAsync(ushort squadId)
        {
            return _requestFactories.SquadDetailsRequestFactory(squadId).PerformRequestAsync();
        }

        public Task<SendItemToClubResponse> SendItemToClubAsync(ItemData itemData)
        {
            itemData.ThrowIfNullArgument();
            return _requestFactories.SendItemToClubRequestFactory(itemData).PerformRequestAsync();
        }

        public Task<SendItemToTradePileResponse> SendItemToTradePileAsync(ItemData itemData)
        {
            itemData.ThrowIfNullArgument();
            return _requestFactories.SendItemToTradePileRequestFactory(itemData).PerformRequestAsync();
        }

        public Task<QuickSellResponse> QuickSellItemAsync(long itemId)
        {
            if (itemId < 1) throw new ArgumentException("Definitely not valid", "itemId");
            return QuickSellItemAsync(new[] { itemId });
        }

        public Task<QuickSellResponse> QuickSellItemAsync(IEnumerable<long> itemIds)
        {
            if (itemIds == null) throw new ArgumentNullException("itemIds");

            foreach (var itemId in itemIds)
                if (itemId < 1) 
                    throw new ArgumentException(string.Format("ItemId {0} is definitely not valid", itemId), "itemId");
            return _requestFactories.QuickSellRequestFactory(itemIds).PerformRequestAsync();
        }

        public Task<byte[]> GetClubImageAsync(AuctionInfo auctionInfo)
        {
            auctionInfo.ThrowIfNullArgument();
            return _requestFactories.ClubImageRequestFactory(auctionInfo).PerformRequestAsync();
        }

        public Task<byte[]> GetNationImageAsync(Item item)
        {
            item.ThrowIfNullArgument();
            return _requestFactories.NationImageRequestFactory(item).PerformRequestAsync();
        }
        
        public Task<byte> ReListAsync()
        {
            return _requestFactories.ReListRequestFactory().PerformRequestAsync();
        }
    }
}
