namespace TradingBell.Common
{
    using System;

    public interface IDataDescription
    {
        string getColName();
        string getColType();
        bool getDecoded();
        bool getDisplayable();
        string getFormat();
        string getHTMLType();
        string getTagName();
    }
}

