using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Peter_Registry.Models
{
    public class ShareRegistry : IEnumerable<ShareInMemory>
    {
        private const int SLOW_SMA_DAY_COUNT = 21;
        private const int FAST_SMA_DAY_COUNT = 7;

        private IList<ShareInMemory> registry = new List<ShareInMemory>();

        /// <summary>
        /// The number of shares in the registry.
        /// </summary>
        public int Count => registry.Count;

        public void Add(ShareInMemory shareInMemory) => registry.Add(shareInMemory);

        /// <summary>
        /// The number of shares short-listed for buy. If NULL, no evaluation has run yet.
        /// </summary>
        public int BuyablesCount => Buyables.Count();

        /// <summary>
        /// The number of shares short-listed for update. If NULL, no evaluation has run yet.
        /// </summary>
        public int UpdateablesCount => Updateables.Count();

        private IEnumerable<ShareInMemory> Buyables => registry.Where(share => share.IsBuyable);

        private IEnumerable<ShareInMemory> Updateables => registry.Where(share => share.IsUpdateable);

        /// <summary>
        /// Evaluates all the shares in the registry and fills up the IsBuyable properties
        /// </summary>
        //public void EvaluateBuyingSignals(MarketDataAllShares marketData)
        //{
        //    registry.ToList().ForEach(share =>
        //    {
        //        share.IsBuyable = IsBuyable(marketData, share, out decimal closingPrice);
        //        share.ClosingPrice = closingPrice;
        //        share.PE = (share.EPS != null && share.EPS != 0) ? closingPrice / share.EPS : 0;
        //    });
        //}        

        //private static bool IsBuyable(MarketDataAllShares marketData, ShareInMemory share, out decimal closingPrice)
        //{
        //    var marketDataOneShare = marketData[share.ISIN];
        //    closingPrice = marketData[share.ISIN][marketDataOneShare.Keys.Max()].ClosingPrice;

        //    if (share.ReportIsOutDatedOrMissing || share.EPS <= 0)
        //    {
        //        return false;
        //    }

        //    share.FastSMA = marketDataOneShare
        //        .Keys
        //        .OrderByDescending(date => date)
        //        .Take(FAST_SMA_DAY_COUNT)
        //        .Average(date => marketData[share.ISIN][date].ClosingPrice);

        //    share.SlowSMA = marketDataOneShare
        //        .Keys
        //        .OrderByDescending(date => date)
        //        .Take(SLOW_SMA_DAY_COUNT)
        //        .Average(date => marketData[share.ISIN][date].ClosingPrice);

        //    return share.SlowSMA < closingPrice && closingPrice < share.FastSMA;
        //}

        public IEnumerator<ShareInMemory> GetEnumerator() => registry.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => registry.GetEnumerator();
    }
}
