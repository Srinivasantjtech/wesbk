namespace TradingBell.Common
{
    using System;
    using System.Collections.Generic;

    public interface IFeaturedProducts
    {
        IList<IResultRow> getItems();
        int getProductCount();
    }
}

