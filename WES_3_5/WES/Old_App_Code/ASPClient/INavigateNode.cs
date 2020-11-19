﻿namespace TradingBell.Common
{
    using System;

    public interface INavigateNode
    {
        string getEnglishName();
        string getLabel();
        string getPath();
        string getPurePath();
        string getSEOPath();
        int getType();
        string getValue();
    }
}

