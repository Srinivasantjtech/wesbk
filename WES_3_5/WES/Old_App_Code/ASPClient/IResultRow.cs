namespace TradingBell.Common
{
    using System;

    public interface IResultRow
    {
        string getCellData(int col);
        int size();
    }
}

