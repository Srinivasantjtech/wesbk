namespace TradingBell.Common
{
    using System;
    using System.Collections.Generic;

    public interface ICarveOut
    {
        string getDisplayFormat();
        IList<IResultRow> getItems();
        int getMaximum();
        int getProductCount();
    }
}

