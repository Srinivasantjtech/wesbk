namespace TradingBell.Common
{
    using System;
    using System.Collections.Generic;

    public interface IBreadCrumbTrail
    {
        string getFullPath();
        string getPureCategoryPath();
        IList<INavigateNode> getSearchPath();
    }
}

