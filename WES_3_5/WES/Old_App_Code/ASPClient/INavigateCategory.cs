namespace TradingBell.Common
{
    using System;
    using System.Collections.Generic;

    public interface INavigateCategory
    {
        IList<string> getIDs();
        string getName();
        string getNodeString();
        int getProductCount();
        string getSEOPath();
        IList<INavigateCategory> getSubCategories();
    }
}

