using System;
using System.Collections.Generic;
using System.Web;
using System.IO;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using StringTemplate = Antlr.StringTemplate.StringTemplate;
using StringTemplateGroup = Antlr.StringTemplate.StringTemplateGroup;

namespace TradingBell.Common
{
    public class TBWDataList
    {
        string _TBWDataListItem;
        public TBWDataList(string TBWDataListItem)
        {
            this._TBWDataListItem = TBWDataListItem;
        }
        public string TBWDataListItem { get { return _TBWDataListItem; } }
    }
    public class TBWDataList1
    {
        string _TBWDataListItem1;
        public TBWDataList1(string TBWDataListItem1)
        {
            this._TBWDataListItem1 = TBWDataListItem1;
        }
        public string TBWDataListItem1 { get { return _TBWDataListItem1; } }
    }
}
