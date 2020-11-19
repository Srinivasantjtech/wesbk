using System;
using System.ComponentModel;
using System.Data;
using System.Configuration;
using System.Web;
using System.Security;
using System.Security.Permissions;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Drawing.Design;
using System.IO;
using System.Text;
using TradingBell.Common;
using TradingBell.WebServices;

/// <summary>
/// TBGridView Control is used to show the Dynamic Table for list out all the Products 
/// in the Family.(including Shipping Image  and Cart Image)
/// </summary>

[assembly: TagPrefix("TradingBell.Web.UI", "Webcat")]
[assembly: System.Reflection.AssemblyVersion("5.0")]
namespace TradingBell.Web.UI
{
    public enum DisplayModes
    {
        Flat = 1,
        GridView = 2,
    }

    public enum GroupModes
    {
        DropDown = 1,
        Merged = 2,
    }

    [
    AspNetHostingPermission(SecurityAction.Demand,
    Level = AspNetHostingPermissionLevel.Minimal),
    AspNetHostingPermission(SecurityAction.InheritanceDemand,
    Level = AspNetHostingPermissionLevel.Minimal),
    ToolboxData("<{0}:TBGridView ID='TBGridView' runat=\"server\"> </{0}:TBGridView>"),
    ParseChildren(true, "GroupColumns")
    ]

    public class TBGridView : WebControl
    {
        DataSet _DataSet;
        Style _hdr = new Style();
        ArrayList _Group; /// = new ArrayList();
        ArrayList _GroupLower;
        ArrayList _HyperLinks = new ArrayList();
        //ArrayList  _HyperLower;
        DataSet _Auto = null;
        int _FamilyID;

        GroupModes _GrpMode;
        DisplayModes _DispMode;
        ShoppingCart _Cart = new ShoppingCart();
        Shipping _Ship = new Shipping();
        bool _Paging;
        bool _AutoConfig = true;
        int _PageSize;
        int _currentPage = 0;
        int _TotalPages = 0;
        int _currentStart = 0;
        int _currentEnd = 0;
        string _Currency;
        Style _Alt = new Style();
        Style _Row = new Style();
        Style _GrpStyle = new Style();
        bool _GrpDisp = false;
        bool _ECom = true;
        int _TBGridWidth;
        string _NaviURL;
        string _NaviColumn;
        int _ImgHeight = 100;
        int _ImgWidth = 100;
        string _EnableRestProduct;
        int _NoofColumns;
        int _NoofRows;
        string _SearchAttrColor = "#000000";
        public string FlatViewHtml = "";
        Hashtable oGrp = new Hashtable();
        Helper oHelper = new Helper();

        public TBGridView()
        {
        }
        [
        Browsable(true),
        Category("TradingBell"),
        Description("Set the Number of Columns"),
        ]
        public virtual int NoofColumns
        {
            get
            {
                return _NoofColumns;
            }
            set
            {
                _NoofColumns = value;
            }
        }
        [
        Browsable(true),
        Category("TradingBell"),
        Description("Set the Number of Rows"),
        ]
        public virtual int NoofRows
        {
            get
            {
                return _NoofRows;
            }
            set
            {
                _NoofRows = value;
            }
        }

        [
        Browsable(true),
        Category("Hyperlink"),
        Description("Columns to be Hyperlinked"),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Content),
        PersistenceMode(PersistenceMode.InnerProperty),
        Editor(typeof(HyperlinkColumnCollectionEditor), typeof(System.Drawing.Design.UITypeEditor))
        ]

        public ArrayList HyperlinkColumns
        {
            get
            {
                if (_HyperLinks == null)
                {
                    _HyperLinks = new ArrayList();
                }
                return _HyperLinks;
            }
        }

        [
        Browsable(true),
        Category("Grouping"),
        Description("Columns needed to be Grouped"),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Content),
        PersistenceMode(PersistenceMode.InnerProperty),
        Editor(typeof(GroupingColumnCollectionEditor), typeof(System.Drawing.Design.UITypeEditor))
        ]

        public ArrayList GroupColumns
        {
            get
            {
                if (_Group == null)
                {
                    _Group = new ArrayList();
                }
                return _Group;
            }
        }

        [
        Browsable(true),
        Category("TradingBell"),
        Description("Get Automatic configuaration"),
        ]
        public virtual bool AutoConfig
        {
            get
            {
                return _AutoConfig;
            }
            set
            {
                _AutoConfig = value;
            }
        }


        [
        Browsable(true),
        Category("TradingBell"),
        Description("FAMILY ID to get automatic Configuaration"),
        ]

        public virtual int FamilyID
        {
            get
            {
                return _FamilyID;
            }
            set
            {
                _FamilyID = value;
            }
        }
        [
        Browsable(true),
        Category("TradingBell"),
        Description("TBGridWidth to get automatic Configuaration"),
        ]

        public int TBGridWidth
        {
            get
            {
                return _TBGridWidth;
            }
            set
            {
                _TBGridWidth = value;
            }
        }
        [
        Browsable(true),
        Category("TradingBell"),
        Description("Paging Enable / Disable")
        ]

        public virtual bool Paging
        {
            get
            {
                return _Paging;
            }
            set
            {
                _Paging = value;
            }
        }

        [
        Browsable(true),
        Category("TradingBell"),
        ]

        public int PageSize
        {
            get
            {
                return _PageSize;
            }
            set
            {
                _PageSize = value;
            }
        }

        [
        Browsable(true),
        Category("Grouping"),
        Description("Display Grouped columns in Table?"),
        ]
        public bool GroupedColumnDisplay
        {
            get
            {
                return _GrpDisp;
            }
            set
            {
                _GrpDisp = value;
            }
        }

        [
       Browsable(true),
       Category("TradingBell"),
       Description("Set the navigation column name"),
       ]
        public string NavigationColumn
        {
            get
            {
                return _NaviColumn;
            }
            set
            {
                _NaviColumn = value;
            }
        }
        [
        Browsable(true),
        Category("TradingBell"),
        Description("Set Navigation URL for column navigation"),
        ]
        public string NavigationURL
        {
            get
            {
                return _NaviURL;
            }
            set
            {
                _NaviURL = value;
            }
        }

        [
    Browsable(true),
    Category("TradingBell"),
    Description("Enable Restricted Product"),
    ]
        public string EnableRestrictedProduct
        {
            get
            {
                return _EnableRestProduct;
            }
            set
            {
                _EnableRestProduct = value;
            }
        }
        [
        Browsable(true),
        Category("Grouping"),
        Description("Grouped columns Display Mode"),
        ]

        public GroupModes GroupColumnMode
        {
            get
            {
                return _GrpMode;
            }
            set
            {
                _GrpMode = value;
            }
        }

        [
        Browsable(true),
        Category("Appearance"),
        Description("Grid Display style")
        ]

        public virtual DisplayModes GridDisplayStyle
        {
            get
            {
                return _DispMode;
            }
            set
            {
                _DispMode = value;
            }
        }

        [
        Browsable(true),
        Category("Appearance"),
        Description("Grouped Column display style"),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Content),
        TypeConverter(typeof(ExpandableObjectConverter))
        ]

        public virtual Style GroupStyle
        {
            get
            {
                return _GrpStyle;
            }
            set
            {
                _GrpStyle = value;
            }
        }


        [
        Browsable(true),
        Category("Appearance"),
        Description("Alternate rowstyle"),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Content),
        TypeConverter(typeof(System.ComponentModel.ExpandableObjectConverter))
        ]

        public virtual Style AlternateRowStyle
        {
            get
            {
                return _Alt;
            }
            set
            {
                _Alt = value;
            }
        }

        [
        Browsable(true),
        Category("Appearance"),
        Description("Assign Row Style"),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Content),
        TypeConverter(typeof(ExpandableObjectConverter))
        ]

        public virtual Style RowStyle
        {
            get
            {
                return _Row;
            }
            set
            {
                _Row = value;
            }
        }

        [
        Browsable(true),
        Category("Appearance"),
        Description("Assign Header object"),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Content),
        TypeConverter(typeof(System.ComponentModel.ExpandableObjectConverter))
        ]

        public virtual Style HeaderStyle
        {
            get
            {
                return _hdr;
            }
            set
            {
                _hdr = value;
            }
        }


        [
        Browsable(true),
        Category("TradingBell"),
        Description("Enable E-Commerce"),
        ]
        public virtual bool ECommerceEnabled
        {
            get
            {
                return _ECom;
            }
            set
            {
                _ECom = value;
            }
        }


        [
        Browsable(true),
        Category("Appearance"),
        Description("Shopping Cart Image display details"),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Content),
        PersistenceMode(PersistenceMode.InnerProperty),
        ]

        public virtual ShoppingCart Shopping
        {
            get
            {
                if (_Cart == null)
                {
                    _Cart = new ShoppingCart();
                }
                return _Cart;
            }
            set
            {
                _Cart = value;
            }

        }
        [
       Browsable(true),
       Category("TradingBell"),
       Description("Currency Symbol"),
       ]
        public virtual string Currency
        {
            get
            {
                return _Currency;
            }
            set
            {
                _Currency = value;
            }
        }
        [
        Browsable(true),
        Category("Appearance"),
        Description("Shipping  Image display details"),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Content),
        PersistenceMode(PersistenceMode.InnerProperty),
        ]

        public virtual Shipping Shipping
        {
            get
            {
                if (_Ship == null)
                {
                    _Ship = new Shipping();
                }
                return _Ship;
            }
            set
            {
                _Ship = value;
            }

        }

        [
        Browsable(true),
        Category("Data"),
        Description("Assign a DataSource to Build a Grid View")
        ]

        public virtual DataSet DataSource
        {

            get
            {
                return _DataSet;
            }
            set
            {
                HyperlinkColumn hypc = new HyperlinkColumn();
                _DataSet = value;
                if (_AutoConfig)
                {
                    //if (Page.Session["NAVIGATIONCOLUMN"].ToString() == "")
                    //if (Helper.WebCatGlb["NAVIGATIONCOLUMN"].ToString() == "")
                    if (_NaviColumn == "")
                        hypc.FieldName = _DataSet.Tables[0].Columns[1].ColumnName.ToString();
                    else
                    {
                        //hypc.FieldName = _DataSet.Tables[0].Columns[Page.Session["NAVIGATIONCOLUMN"].ToString()].ColumnName.ToString();
                        hypc.FieldName = _DataSet.Tables[0].Columns[_NaviColumn].ColumnName.ToString();
                    }
                    //hypc.URL = Page.Session["NAVIGATIONURL"].ToString(); //"ProductDisplay.aspx?pid={PRODUCT_ID}";
                    //hypc.URL = Helper.WebCatGlb["NAVIGATIONURL"].ToString();
                    hypc.URL = _NaviURL.ToString();
                    _HyperLinks.Add(hypc);
                }
            }
        }

        public virtual string SearchAttrColor
        {
            get
            {
                return _SearchAttrColor;
            }
            set
            {
                _SearchAttrColor = value;
            }
        }
        public virtual int ImgHeight
        {
            get
            {
                return _ImgHeight;
            }
            set
            {
                _ImgHeight = value;
            }
        }

        public virtual int ImgWidth
        {
            get
            {
                return _ImgWidth;
            }
            set
            {
                _ImgWidth = value;
            }
        }
        public string Text()
        {
            return "";
        }

        private DropDownList TBDropDown(DataSet DataSource, string ColumnName)
        {
            DropDownList retDropDown = new DropDownList();
            ListItem lITM = new ListItem();

            lITM.Value = "";
            lITM.Text = "(All)";
            retDropDown.Items.Add(lITM);
            string _tempValue = "  ";
            foreach (DataRow oDr in DataSource.Tables[0].Select("", ColumnName))
            {
                if (oDr[ColumnName].ToString().ToLower() != _tempValue.ToLower())
                {
                    retDropDown.Items.Add(oDr[ColumnName].ToString());
                    _tempValue = oDr[ColumnName].ToString();
                }
            }
            return retDropDown;
        }

        private DropDownList TBDropDown(DataSet DataSource, string ColumnName, string SelectedItem)
        {
            DropDownList retDropDown = new DropDownList();
            ListItem lITM = new ListItem();

            lITM.Value = "";
            lITM.Text = "(All)";
            retDropDown.Items.Add(lITM);
            string _tempValue = "  ";
            foreach (DataRow oDr in DataSource.Tables[0].Select("", ColumnName))
            {
                if (oDr[ColumnName].ToString().ToLower() != _tempValue.ToLower())
                {
                    lITM = new ListItem();
                    lITM.Text = oDr[ColumnName].ToString();
                    lITM.Value = oDr[ColumnName].ToString();
                    if (SelectedItem.ToLower() == oDr[ColumnName].ToString().ToLower())
                    {
                        lITM.Selected = true;
                    }
                    else lITM.Selected = false;
                    retDropDown.Items.Add(lITM);

                    _tempValue = oDr[ColumnName].ToString();
                }
            }
            return retDropDown;
        }

        private ArrayList ConvertCase(ArrayList oArray)
        {
            ArrayList retArray = new ArrayList();
            if (oArray != null)
            {
                foreach (string sStr in oArray)
                {
                    retArray.Add(sStr.ToLower());
                }
            }
            else
                retArray.Add("  ");
            return retArray;
        }

        private string TBParse(string URLString, DataRow DataSource)
        {
            string ParseStr = URLString;
            string tempStr = ParseStr;
            string finalString = "";

            while (tempStr != "")
            {
                finalString = finalString + (tempStr + "{").Substring(0, (tempStr + "{").IndexOf("{"));
                tempStr = tempStr.Substring((tempStr + "{").IndexOf("{"));
                if (tempStr != "")
                {
                    if (tempStr.IndexOf("{") >= 0)
                    {
                        finalString = finalString + DataSource[(tempStr + "{").Substring(1, (tempStr + "}").IndexOf("}") - 1)];
                    }
                    tempStr = tempStr.Substring((tempStr + "}").IndexOf("}") + 1);
                }
            }

            return finalString;
        }

        private HyperLink GetHyperLink(string ColumnName, string Value, DataRow DataSource)
        {
            HyperLink retHyp = null;
            if (_HyperLinks != null)
            {
                for (int iCtr = 0; iCtr <= _HyperLinks.Count - 1; iCtr++)
                {
                    if (ColumnName.ToLower() == ((TradingBell.Web.UI.HyperlinkColumn)_HyperLinks[iCtr]).FieldName.ToLower())
                    {
                        retHyp = new HyperLink();
                        retHyp.NavigateUrl = TBParse(((TradingBell.Web.UI.HyperlinkColumn)_HyperLinks[iCtr]).URL.ToString(), DataSource);
                        retHyp.Text = Value;
                    }
                }
            }
            return retHyp;
        }

        //public virtual bool GroupedColumnsDiplay
        //{
        //    get
        //    {
        //        return _GrpDisp;
        //    }
        //    set
        //    {
        //        _GrpDisp = value;
        //    }
        //}

        /*public virtual string DateFormat()
        {

        }*/

        private Hashtable GetHash(ArrayList oArray)
        {
            Hashtable retHash = new Hashtable();

            if (oArray != null)
            {
                foreach (string sStr in oArray)
                {
                    retHash.Add(sStr, "  ");
                }
            }
            return retHash;
        }

        private void GetAutoConfig()
        {
            if (_AutoConfig)
            {

                // ProductFamily oPF = new ProductFamily();
                //_Auto = oPF.GetFamilyLayout();
                Layout oLay = new Layout();
                _Auto = oLay.GetFamilyLayout();
                if (_FamilyID == 0)
                {
                    //_FamilyID = (int)Page.Session["FAMILY_ID"];
                    _FamilyID = oHelper.CI(Page.Session["FAMILY_ID"].ToString());
                }

                DataRow[] oDRs = _Auto.Tables[0].Select("FAMILY_ID = " + _FamilyID);
                foreach (DataRow oDR in oDRs)
                {
                    if (oHelper.CI(oDR["LAYOUT"]) == 1)
                    {
                        _DispMode = DisplayModes.GridView;
                    }
                    else if (oHelper.CI(oDR["LAYOUT"]) == 2)
                    {
                        _DispMode = DisplayModes.Flat;
                    }
                    string _tGrpCols = oDR["GROUPED_COLUMNS"].ToString();
                    if (_tGrpCols != "")
                    {
                        string[] _aGrpCols = _tGrpCols.Split(',');
                        _Group = new ArrayList();
                        for (int iCtr = 0; iCtr < _aGrpCols.Length; iCtr++)
                        {
                            _Group.Add(_aGrpCols[iCtr].ToString().Trim());
                            _GrpMode = GroupModes.DropDown;
                        }
                    }
                }

                //_hdr.CssClass = Page.Session["HEADERROWSTYLECLASS"].ToString();
                //_Row.CssClass = Page.Session["ROWSTYLECLASS"].ToString();
                //_Alt.CssClass = Page.Session["ALTROWSTYLECLASS"].ToString();
                //_Cart.CartURL = Page.Session["CARTURL"].ToString();
                //_Cart.CartImagePath = Page.Session["CARTIMGPATH"].ToString();
                //_Cart.Caption = Page.Session["CARTCAPTION"].ToStrion();]

                //_hdr.CssClass = Helper.WebCatGlb["HEADERROWSTYLE"].ToString();
                //_Row.CssClass = Helper.WebCatGlb["ROWSTYLE"].ToString();
                //_Alt.CssClass = Helper.WebCatGlb["ALTROWSTYLE"].ToString();

                // _Cart.CartURL = Helper.WebCatGlb["CARTURL"].ToString();
                // _Cart.CartImagePath = Helper.WebCatGlb["IMAGE PATH"].ToString() + Helper.WebCatGlb["CARTIMGPATH"].ToString();
                // _Cart.Caption = Helper.WebCatGlb["CARTCAPTION"].ToString();
                //  if (Helper.WebCatGlb["GROUPCOLUMNDISPLAY"].ToString() == "YES")
                if (GroupedColumnDisplay)
                    _GrpDisp = true;
                else _GrpDisp = false;
                // if (Helper.WebCatGlb["ECOMMERCEENABLED"].ToString() == "YES") _ECom = true;
                if (ECommerceEnabled) _ECom = true;
                else _ECom = false;

            }
        }

        protected override void RenderContents(HtmlTextWriter writer)
        {
            Table oTable = new Table();
            //oTable.BorderWidth = Unit.Pixel(10);
            TableRow oTblRow = new TableRow();
            string groupcol = "";
            int nCols = 0;
            Hashtable GrpVariables = new Hashtable();

            if (DesignMode)
            {
                oTblRow = new TableRow();
                oTblRow.ApplyStyle(_hdr);
                TableCell oTblCell = new TableCell();
                oTblCell.Text = "Catalog#";
                oTblRow.Cells.Add(oTblCell);
                oTblCell = new TableCell();
                oTblCell.Text = "Mfg Part#";
                oTblRow.Cells.Add(oTblCell);
                oTblCell = new TableCell();
                oTblCell.Text = "Attribute";
                oTblRow.Cells.Add(oTblCell);
                oTblCell = new TableCell();
                oTblCell.Text = "Attribute";
                oTblRow.Cells.Add(oTblCell);
                oTblCell = new TableCell();
                oTblCell.Text = "Price";
                oTblRow.Cells.Add(oTblCell);
                oTable.Rows.Add(oTblRow);

                oTblRow = new TableRow();
                oTblRow.ApplyStyle(_Row);
                oTblCell = new TableCell();
                oTblCell.Text = "CAT ITM 001";
                oTblRow.Cells.Add(oTblCell);
                oTblCell = new TableCell();
                oTblCell.Text = "MFG";
                oTblRow.Cells.Add(oTblCell);
                oTblCell = new TableCell();
                oTblCell.Text = "Value";
                oTblRow.Cells.Add(oTblCell);
                oTblCell = new TableCell();
                oTblCell.Text = "Value";
                oTblRow.Cells.Add(oTblCell);
                oTblCell = new TableCell();
                oTblCell.Text = "0.00";
                oTblRow.Cells.Add(oTblCell);
                oTable.Rows.Add(oTblRow);

                oTblRow = new TableRow();
                oTblRow.ApplyStyle(_Alt);
                oTblCell = new TableCell();
                oTblCell.Text = "CAT ITM 001";
                oTblRow.Cells.Add(oTblCell);
                oTblCell = new TableCell();
                oTblCell.Text = "MFG";
                oTblRow.Cells.Add(oTblCell);
                oTblCell = new TableCell();
                oTblCell.Text = "Value";
                oTblRow.Cells.Add(oTblCell);
                oTblCell = new TableCell();
                oTblCell.Text = "Value";
                oTblRow.Cells.Add(oTblCell);
                oTblCell = new TableCell();
                oTblCell.Text = "0.00";
                oTblRow.Cells.Add(oTblCell);
                oTable.Rows.Add(oTblRow);

                oTblRow = new TableRow();
                oTblRow.ApplyStyle(_Row);
                oTblCell = new TableCell();
                oTblCell.Text = "CAT ITM 001";
                oTblRow.Cells.Add(oTblCell);
                oTblCell = new TableCell();
                oTblCell.Text = "MFG";
                oTblRow.Cells.Add(oTblCell);
                oTblCell = new TableCell();
                oTblCell.Text = "Value";
                oTblRow.Cells.Add(oTblCell);
                oTblCell = new TableCell();
                oTblCell.Text = "Value";
                oTblRow.Cells.Add(oTblCell);
                oTblCell = new TableCell();
                oTblCell.Text = "0.00";
                oTblRow.Cells.Add(oTblCell);
                oTable.Rows.Add(oTblRow);

                oTblRow = new TableRow();
                oTblRow.ApplyStyle(_Alt);
                oTblCell = new TableCell();
                oTblCell.Text = "CAT ITM 001";
                oTblRow.Cells.Add(oTblCell);
                oTblCell = new TableCell();
                oTblCell.Text = "MFG";
                oTblRow.Cells.Add(oTblCell);
                oTblCell = new TableCell();
                oTblCell.Text = "Value";
                oTblRow.Cells.Add(oTblCell);
                oTblCell = new TableCell();
                oTblCell.Text = "Value";
                oTblRow.Cells.Add(oTblCell);
                oTblCell = new TableCell();
                oTblCell.Text = "0.00";
                oTblRow.Cells.Add(oTblCell);
                oTable.Rows.Add(oTblRow);

                oTblRow = new TableRow();
                oTblRow.ApplyStyle(_Row);
                oTblCell = new TableCell();
                oTblCell.Text = "CAT ITM 001";
                oTblRow.Cells.Add(oTblCell);
                oTblCell = new TableCell();
                oTblCell.Text = "MFG";
                oTblRow.Cells.Add(oTblCell);
                oTblCell = new TableCell();
                oTblCell.Text = "Value";
                oTblRow.Cells.Add(oTblCell);
                oTblCell = new TableCell();
                oTblCell.Text = "Value";
                oTblRow.Cells.Add(oTblCell);
                oTblCell = new TableCell();
                oTblCell.Text = "0.00";
                oTblRow.Cells.Add(oTblCell);
                oTable.Rows.Add(oTblRow);

                oTblRow = new TableRow();
                oTblRow.ApplyStyle(_Alt);
                oTblCell = new TableCell();
                oTblCell.Text = "CAT ITM 001";
                oTblRow.Cells.Add(oTblCell);
                oTblCell = new TableCell();
                oTblCell.Text = "MFG";
                oTblRow.Cells.Add(oTblCell);
                oTblCell = new TableCell();
                oTblCell.Text = "Value";
                oTblRow.Cells.Add(oTblCell);
                oTblCell = new TableCell();
                oTblCell.Text = "Value";
                oTblRow.Cells.Add(oTblCell);
                oTblCell = new TableCell();
                oTblCell.Text = "0.00";
                oTblRow.Cells.Add(oTblCell);
                oTable.Rows.Add(oTblRow);

                oTblRow = new TableRow();
                oTblRow.ApplyStyle(_Row);
                oTblCell = new TableCell();
                oTblCell.Text = "CAT ITM 001";
                oTblRow.Cells.Add(oTblCell);
                oTblCell = new TableCell();
                oTblCell.Text = "MFG";
                oTblRow.Cells.Add(oTblCell);
                oTblCell = new TableCell();
                oTblCell.Text = "Value";
                oTblRow.Cells.Add(oTblCell);
                oTblCell = new TableCell();
                oTblCell.Text = "Value";
                oTblRow.Cells.Add(oTblCell);
                oTblCell = new TableCell();
                oTblCell.Text = "0.00";
                oTblRow.Cells.Add(oTblCell);
                oTable.Rows.Add(oTblRow);

                oTblRow = new TableRow();
                oTblRow.ApplyStyle(_Alt);
                oTblCell = new TableCell();
                oTblCell.Text = "CAT ITM 001";
                oTblRow.Cells.Add(oTblCell);
                oTblCell = new TableCell();
                oTblCell.Text = "MFG";
                oTblRow.Cells.Add(oTblCell);
                oTblCell = new TableCell();
                oTblCell.Text = "Value";
                oTblRow.Cells.Add(oTblCell);
                oTblCell = new TableCell();
                oTblCell.Text = "Value";
                oTblRow.Cells.Add(oTblCell);
                oTblCell = new TableCell();
                oTblCell.Text = "0.00";
                oTblRow.Cells.Add(oTblCell);
                oTable.Rows.Add(oTblRow);

                oTblRow = new TableRow();
                oTblRow.ApplyStyle(_Row);
                oTblCell = new TableCell();
                oTblCell.Text = "CAT ITM 001";
                oTblRow.Cells.Add(oTblCell);
                oTblCell = new TableCell();
                oTblCell.Text = "MFG";
                oTblRow.Cells.Add(oTblCell);
                oTblCell = new TableCell();
                oTblCell.Text = "Value";
                oTblRow.Cells.Add(oTblCell);
                oTblCell = new TableCell();
                oTblCell.Text = "Value";
                oTblRow.Cells.Add(oTblCell);
                oTblCell = new TableCell();
                oTblCell.Text = "0.00";
                oTblRow.Cells.Add(oTblCell);
                oTable.Rows.Add(oTblRow);

                oTblRow = new TableRow();
                oTblRow.ApplyStyle(_Alt);
                oTblCell = new TableCell();
                oTblCell.Text = "CAT ITM 001";
                oTblRow.Cells.Add(oTblCell);
                oTblCell = new TableCell();
                oTblCell.Text = "MFG";
                oTblRow.Cells.Add(oTblCell);
                oTblCell = new TableCell();
                oTblCell.Text = "Value";
                oTblRow.Cells.Add(oTblCell);
                oTblCell = new TableCell();
                oTblCell.Text = "Value";
                oTblRow.Cells.Add(oTblCell);
                oTblCell = new TableCell();
                oTblCell.Text = "0.00";
                oTblRow.Cells.Add(oTblCell);
                oTable.Rows.Add(oTblRow);

                oTable.RenderControl(writer);
            }
            else
            {
                GetAutoConfig();
                //Label oLbl;

                oTable.CellSpacing = 0;
                _GroupLower = ConvertCase(_Group);
                if (_Group != null)
                {
                    GrpVariables = GetHash(_GroupLower);
                    foreach (string grpstr in _GroupLower)
                    {
                        groupcol = groupcol + "," + grpstr;
                    }
                    groupcol = groupcol.Substring(1);
                }

                string sFilter = null;
                if (_Group != null)
                {
                    if (_GrpMode == GroupModes.DropDown)
                    {
                        //////////////////////for (int iCtr = 0; iCtr <= _Group.Count - 1; iCtr++)
                        //////////////////////{
                        //////////////////////    oTblRow = new TableRow();
                        //////////////////////    TableCell otblCell = new TableCell();
                        //////////////////////    Label oLbl = new Label();
                        //////////////////////    DropDownList oDrp;
                        //////////////////////    oLbl.ApplyStyle(_hdr);
                        //////////////////////    if (_DataSet.Tables[0].Columns[_Group[iCtr].ToString()].Caption != "")
                        //////////////////////        oLbl.Text = _DataSet.Tables[0].Columns[_Group[iCtr].ToString()].Caption.ToString();
                        //////////////////////    else
                        //////////////////////        oLbl.Text = _DataSet.Tables[0].Columns[_Group[iCtr].ToString()].ColumnName.ToString();
                        //////////////////////    string sSelItm = Page.Request[_Group[iCtr].ToString()];
                        //////////////////////    if (sSelItm != null && sSelItm != "")
                        //////////////////////    {
                        //////////////////////        oDrp = TBDropDown(_DataSet, _Group[iCtr].ToString(), sSelItm);
                        //////////////////////        if (_DataSet.Tables[0].Columns[_Group[iCtr].ToString()].DataType == typeof(System.Decimal))
                        //////////////////////            sFilter = sFilter + " AND [" + _Group[iCtr].ToString().ToLower() + "] = " + sSelItm.Replace("'", "''");
                        //////////////////////        else
                        //////////////////////            sFilter = sFilter + " AND [" + _Group[iCtr].ToString().ToLower() + "] = '" + sSelItm.Replace("'", "''") + "'";
                        //////////////////////    }
                        //////////////////////    else
                        //////////////////////    {
                        //////////////////////        oDrp = TBDropDown(_DataSet, _Group[iCtr].ToString());
                        //////////////////////    }
                        //////////////////////    oDrp.ID = _Group[iCtr].ToString();
                        //////////////////////    otblCell.Controls.Add(oLbl);
                        //////////////////////    otblCell.Controls.Add(oLbl);
                        //////////////////////    oTblRow.Cells.Add(otblCell);
                        //////////////////////    otblCell = new TableCell();
                        //////////////////////    otblCell.Controls.Add(oDrp);
                        //////////////////////    oTblRow.Cells.Add(otblCell);
                        //////////////////////    ////////////////oTable.Rows.Add(oTblRow);
                        //////////////////////}
                        //////////////////////if (sFilter != null)
                        //////////////////////{
                        //////////////////////    sFilter = sFilter.Substring(5);
                        //////////////////////}

                        //////////////////////oTblRow = new TableRow();
                        //////////////////////TableCell oTblCell = new TableCell();
                        //////////////////////oTblCell.ColumnSpan = 2;
                        //////////////////////oTblCell.HorizontalAlign = HorizontalAlign.Center;
                        //////////////////////// Submit Button
                        //////////////////////Button oBtn = new Button();
                        //////////////////////oBtn.Text = "Go";
                        //////////////////////oTblCell.Controls.Add(oBtn);
                        //////////////////////oTblRow.Cells.Add(oTblCell);
                        //////////////////////////////////oTable.Rows.Add(oTblRow);
                        //////////////////////////////////oTable.RenderControl(writer);
                        //////////////////////oTable = new Table();
                        //////////////////////oTblRow = new TableRow();
                    }
                }
                //oTable.CellPadding = 5;
                int pCtr = 0;
                int ct = 0;
                bool IsCartImageAdded = false;
                bool IsShipImageAdded = false;
                DataRow[] oDRs;
                //Check the data set
                if (_DataSet != null)
                {
                    oDRs = _DataSet.Tables[0].Select(sFilter, groupcol);

                    if (!_Paging)
                        _PageSize = oDRs.Length;
                    if (_PageSize > 0)
                    {
                        _currentPage = oHelper.CI(Page.Request["Pg"]);
                        _TotalPages = oDRs.Length / _PageSize;
                        _currentStart = _currentPage * _PageSize;
                        _currentEnd = _currentStart + _PageSize;
                        if (_currentEnd > oDRs.Length)
                            _currentEnd = oDRs.Length;
                    }
                    #region "Grid View"
                    if (_DispMode == DisplayModes.GridView)
                    {
                        oTblRow = new TableRow();
                        oTblRow.ApplyStyle(_hdr);
                        if (_hdr.CssClass != null) oTblRow.CssClass = _hdr.CssClass;
                        for (int iCtr = 0; iCtr <= _DataSet.Tables[0].Columns.Count - 1; iCtr++)
                        {
                            if (_DataSet.Tables[0].Columns[iCtr].ColumnMapping != MappingType.Hidden)
                            {
                                if ((_GroupLower.Contains(_DataSet.Tables[0].Columns[iCtr].ColumnName.ToLower()) && _GrpDisp) || !_GroupLower.Contains(_DataSet.Tables[0].Columns[iCtr].ColumnName.ToLower()))
                                {
                                    TableCell oTblCell = new TableCell();
                                    oTblCell.ApplyStyle(_hdr);
                                    if (pCtr == _Cart.CartImagePosition && _ECom)
                                    {
                                        IsCartImageAdded = true;
                                        oTblCell.ApplyStyle(_hdr);
                                        oTblCell.Text = _Cart.Caption;
                                        oTblRow.Cells.Add(oTblCell);
                                        oTblCell = new TableCell();
                                    }
                                    if (pCtr == _Ship.ShippingImagePosition && _ECom)
                                    {
                                        IsShipImageAdded = true;
                                        oTblCell.ApplyStyle(_hdr);
                                        oTblCell.Text = _Ship.ShippingCaption;
                                        oTblRow.Cells.Add(oTblCell);
                                        oTblCell = new TableCell();
                                    }
                                    string _caption;
                                    _caption = _DataSet.Tables[0].Columns[iCtr].Caption.ToString();
                                    if (_caption == null || _caption == string.Empty)
                                    {
                                        _caption = _DataSet.Tables[0].Columns[iCtr].ColumnName.ToString();
                                    }
                                    oTblCell.Text = _caption;
                                    oTblRow.Cells.Add(oTblCell);
                                    nCols++;
                                    pCtr = pCtr + 1;
                                } ct = ct + 1;
                            }
                        }
                        //Set the Shipping image in last position.
                        if (IsShipImageAdded == false)
                        {
                            if (pCtr == _Ship.ShippingImagePosition)
                            {
                                IsShipImageAdded = true;
                                TableCell oTblCell = new TableCell();
                                oTblCell.ApplyStyle(_hdr);
                                oTblCell.Text = _Ship.ShippingCaption;
                                oTblRow.Cells.Add(oTblCell);
                                oTblRow.Cells.Add(oTblCell);
                                pCtr = pCtr + 1;
                            }
                        }
                        //Set the cart image in last position.
                        if (IsCartImageAdded == false)
                        {
                            //if (ct == _Cart.CartImagePosition && _ECom)
                            if (pCtr == _Cart.CartImagePosition && _ECom)
                            {
                                IsCartImageAdded = true;
                                TableCell oTblCell = new TableCell();
                                oTblCell.ApplyStyle(_hdr);
                                oTblCell.Text = _Cart.Caption;
                                oTblRow.Cells.Add(oTblCell);
                                oTblRow.Cells.Add(oTblCell);
                            }
                        }
                        oTable.Rows.Add(oTblRow);
                        //TableBody Construction
                        if (_Paging)
                        {
                            string LabelText = "";
                            oTblRow = new TableRow();
                            TableCell oTblCell = new TableCell();
                            oTblCell.ColumnSpan = nCols + 2;
                            oTblCell.HorizontalAlign = HorizontalAlign.Right;
                            oTblCell.ApplyStyle(_Row);
                            Label oLbl = new Label();
                            string _toURL = Page.Request.FilePath.ToString();
                            string _txtSearch = Page.Request["txtSearch"].ToString();
                            if (_txtSearch.Length > 0)
                                _txtSearch = "txtSearch=" + _txtSearch + "&";
                            if (_currentPage > 0)
                                LabelText = "<font face=\"webdings\" size=\"2\"><a href=\"" + _toURL + "?" + _txtSearch + "Pg=0\" style=\"text-decoration:none\">9</a><a href=\"" + _toURL + "?" + _txtSearch + "Pg=" + oHelper.CS(_currentPage - 1) + "\" style=\"text-decoration:none\">7</a></font> &nbsp;&nbsp;";

                            LabelText = LabelText + oHelper.CS(_currentPage + 1) + " of " + oHelper.CS(_TotalPages + 1);

                            if (_currentPage < _TotalPages)
                                LabelText = LabelText + "<font face=\"webdings\" size=\"2\"><a href=\"" + _toURL + "?" + _txtSearch + "Pg=" + oHelper.CS(_currentPage + 1) + "\" style=\"text-decoration:none\">8</a><a href=\"" + _toURL + "?" + _txtSearch + "Pg=" + _TotalPages + "\"  style=\"text-decoration:none\">:</a></font>";

                            oLbl.Text = LabelText;
                            oTblCell.Controls.Add(oLbl);
                            oTblRow.Cells.Add(oTblCell);
                            oTable.Rows.Add(oTblRow);
                        }
                        oDRs = _DataSet.Tables[0].Select(sFilter, groupcol);
                        bool alternateRow = false;
                        for (int rCtr = _currentStart; rCtr < _currentEnd; rCtr++)
                        {
                            DataRow oDR = oDRs[rCtr];
                            oTblRow = new TableRow();

                            pCtr = 0;
                            ct = 0;

                            IsCartImageAdded = false;
                            IsShipImageAdded = false;
                            for (int iCtr = 0; iCtr <= _DataSet.Tables[0].Columns.Count - 1; iCtr++)
                            {

                                if (_DataSet.Tables[0].Columns[iCtr].ColumnMapping != MappingType.Hidden)
                                {
                                    if ((_GroupLower.Contains(_DataSet.Tables[0].Columns[iCtr].ColumnName.ToLower()) && _GrpDisp) || !_GroupLower.Contains(_DataSet.Tables[0].Columns[iCtr].ColumnName.ToLower()))
                                    {
                                        TableCell oTblCell = new TableCell();
                                        oTblCell.ApplyStyle(_Row);
                                        if (alternateRow)
                                            oTblCell.ApplyStyle(_Alt);
                                        if (pCtr == _Cart.CartImagePosition && _ECom)
                                        {
                                            IsCartImageAdded = true;
                                            HyperLink oHyp = new HyperLink();
                                            if (_EnableRestProduct.ToString() == "YES")
                                            {
                                                if (oDR["RESTRICTED"].ToString() == "YES")
                                                {
                                                    Label oLbl = new Label();
                                                    oLbl.Text = _Cart.RestrictedProdText;
                                                    oHyp.Controls.Add(oLbl);
                                                    oHyp.NavigateUrl = TBParse(_Cart.RestrictedProdURL, oDR);
                                                }
                                                else
                                                {
                                                    Image CartImg = new Image();
                                                    CartImg.ImageUrl = _Cart.CartImagePath;
                                                    oHyp.Controls.Add(CartImg);
                                                    oHyp.NavigateUrl = TBParse(_Cart.CartURL, oDR);
                                                }
                                            }
                                            else
                                            {
                                                Image CartImg = new Image();
                                                CartImg.ImageUrl = _Cart.CartImagePath;
                                                oHyp.Controls.Add(CartImg);
                                                oHyp.NavigateUrl = TBParse(_Cart.CartURL, oDR);
                                            }

                                            oTblCell.HorizontalAlign = HorizontalAlign.Center;
                                            oTblCell.Controls.Add(oHyp);
                                            oTblRow.Cells.Add(oTblCell);
                                            oTblCell = new TableCell();
                                            oTblCell.ApplyStyle(_Row);
                                            if (alternateRow) oTblCell.ApplyStyle(_Alt);
                                        }
                                        //Modified on 06_April_2007
                                        //Implement the Product Ship column...
                                        if (pCtr == _Ship.ShippingImagePosition && _ECom)
                                        {
                                            IsShipImageAdded = true;
                                            HyperLink oHyp = new HyperLink();
                                            Image ShipImg = new Image();

                                            if (oDR["IS_SHIPPING"].ToString() == "False")
                                            {
                                                ShipImg.ImageUrl = _Ship.NoShippingImagePath;
                                            }
                                            else
                                            {
                                                oHyp.NavigateUrl = TBParse(_Ship.ShippingURL, oDR);
                                                ShipImg.ImageUrl = _Ship.ShippingImagePath;
                                            }
                                            oHyp.Controls.Add(ShipImg);
                                            oTblCell.Controls.Add(oHyp);
                                            oTblCell.HorizontalAlign = HorizontalAlign.Center;
                                            oTblRow.Cells.Add(oTblCell);
                                            oTblCell = new TableCell();
                                            oTblCell.ApplyStyle(_Row);
                                            if (alternateRow) oTblCell.ApplyStyle(_Alt);
                                        }

                                        if (_DataSet.Tables[0].Columns[iCtr].DataType == typeof(System.Decimal))
                                        {
                                            oTblCell.HorizontalAlign = HorizontalAlign.Right;
                                        }
                                        HyperLink oHypValue;
                                        oHypValue = GetHyperLink(_DataSet.Tables[0].Columns[iCtr].ColumnName, oDR[iCtr].ToString(), oDR);
                                        if (oHypValue != null)
                                        {
                                            oTblCell.Controls.Add(oHypValue);
                                        }
                                        else
                                        {
                                            //Adding currency symbol
                                            if (_DataSet.Tables[0].Columns[iCtr].DataType == typeof(System.Decimal))
                                            {
                                                oTblCell.ForeColor = System.Drawing.Color.Red;
                                                oTblCell.Text = _Currency + oHelper.FixDecPlace(oHelper.CDEC(oDR[iCtr].ToString()));
                                            }
                                            else
                                            {
                                                oTblCell.Text = "&nbsp;" + oDR[iCtr].ToString();
                                            }
                                        }
                                        oTblRow.Cells.Add(oTblCell);
                                        pCtr = pCtr + 1;
                                    } 
                                }
                            }
                            //Set the last position for shipping and cart item.
                            if (IsShipImageAdded == false)
                            {
                                //if (ct == _Ship.ShippingImagePosition && _ECom)
                                if (pCtr == _Ship.ShippingImagePosition && _ECom)
                                {
                                    TableCell oTblCell = new TableCell();
                                    HyperLink oHyp = new HyperLink();
                                    Image ShipImg = new Image();
                                    if (oDR["IS_SHIPPING"].ToString() == "False")
                                    {
                                        ShipImg.ImageUrl = _Ship.NoShippingImagePath;
                                    }
                                    else
                                    {
                                        ShipImg.ImageUrl = _Ship.ShippingImagePath;
                                        oHyp.NavigateUrl = TBParse(_Ship.ShippingURL, oDR);
                                    }
                                    oHyp.Controls.Add(ShipImg);
                                    oTblCell.Controls.Add(oHyp);
                                    oTblCell.HorizontalAlign = HorizontalAlign.Center;
                                    oTblRow.Cells.Add(oTblCell);
                                    oTblCell.ApplyStyle(_Row);
                                    if (alternateRow) oTblCell.ApplyStyle(_Alt);
                                    pCtr = pCtr + 1;
                                }

                            }
                            if (IsCartImageAdded == false)
                            {
                                //if (ct == _Cart.CartImagePosition && _ECom)
                                if (pCtr == _Cart.CartImagePosition && _ECom)
                                {
                                    TableCell oTblCell = new TableCell();
                                    HyperLink oHyp = new HyperLink();
                                    if (_EnableRestProduct.ToString() == "YES")
                                    {
                                        if (oDR["RESTRICTED"].ToString() == "YES")
                                        {
                                            Label oLbl = new Label();
                                            oLbl.Text = _Cart.RestrictedProdText;
                                            oHyp.Controls.Add(oLbl);
                                            oHyp.NavigateUrl = TBParse(_Cart.RestrictedProdURL, oDR);
                                        }
                                        else
                                        {
                                            Image CartImg = new Image();
                                            CartImg.ImageUrl = _Cart.CartImagePath;
                                            oHyp.Controls.Add(CartImg);
                                            oHyp.NavigateUrl = TBParse(_Cart.CartURL, oDR);
                                        }
                                    }
                                    else
                                    {
                                        Image CartImg = new Image();
                                        CartImg.ImageUrl = _Cart.CartImagePath;
                                        oHyp.Controls.Add(CartImg);
                                        oHyp.NavigateUrl = TBParse(_Cart.CartURL, oDR);
                                    }

                                    oTblCell.HorizontalAlign = HorizontalAlign.Center;
                                    oTblCell.Controls.Add(oHyp);
                                    oTblRow.Cells.Add(oTblCell);
                                    oTblCell.ApplyStyle(_Row);
                                    if (alternateRow) oTblCell.ApplyStyle(_Alt);
                                }

                            }

                            oTable.Rows.Add(oTblRow);
                            alternateRow = !alternateRow;
                        }
                        if (_Paging)
                        {
                            string LabelText = "";
                            oTblRow = new TableRow();
                            TableCell oTblCell = new TableCell();
                            oTblCell.ColumnSpan = nCols + 2;
                            oTblCell.HorizontalAlign = HorizontalAlign.Right;
                            oTblCell.ApplyStyle(_Row);
                            Label oLbl = new Label();
                            string _toURL = Page.Request.FilePath.ToString();
                            string _txtSearch = Page.Request["txtSearch"].ToString();
                            if (_txtSearch.Length > 0)
                                _txtSearch = "txtSearch=" + _txtSearch + "&";
                            if (_currentPage > 0)
                                LabelText = "<font face=\"webdings\" size=\"2\"><a href=\"" + _toURL + "?" + _txtSearch + "Pg=0\" style=\"text-decoration:none\">9</a><a href=\"" + _toURL + "?" + _txtSearch + "Pg=" + oHelper.CS(_currentPage - 1) + "\" style=\"text-decoration:none\">7</a></font> &nbsp;&nbsp;";

                            LabelText = LabelText + oHelper.CS(_currentPage + 1) + " of " + oHelper.CS(_TotalPages + 1);

                            if (_currentPage < _TotalPages)
                                LabelText = LabelText + "<font face=\"webdings\" size=\"2\"><a href=\"" + _toURL + "?" + _txtSearch + "Pg=" + oHelper.CS(_currentPage + 1) + "\" style=\"text-decoration:none\">8</a><a href=\"" + _toURL + "?" + _txtSearch + "Pg=" + _TotalPages + "\"  style=\"text-decoration:none\">:</a></font>";

                            oLbl.Text = LabelText;
                            oTblCell.Controls.Add(oLbl);
                            oTblRow.Cells.Add(oTblCell);
                            oTable.Rows.Add(oTblRow);
                        }
                        oTable.Width = _TBGridWidth;
                        oTable.RenderControl(writer);
                    }
                    #endregion
                    else
                        #region "Flat View"
                        if (_DispMode == DisplayModes.Flat)
                        {
                            if (HttpContext.Current.Request["SortCol"] != null) { }
                            else
                            {
                                oDRs = _DataSet.Tables[0].Select(sFilter, groupcol);
                            }
                            //////////////////foreach (DataRow oDR in oDRs)
                            //////////////////{
                            //////////////////    oTable = new Table();
                            //////////////////    oTable.CellPadding = 5;
                            //////////////////    oTblRow = new TableRow();
                            //////////////////    pCtr = 0;
                            //////////////////    ct = 0;

                            //////////////////    IsCartImageAdded = false;
                            //////////////////    IsShipImageAdded = false;
                            //////////////////    for (int iCtr = 0; iCtr < _DataSet.Tables[0].Columns.Count; iCtr++)
                            //////////////////    {

                            //////////////////        if (_DataSet.Tables[0].Columns[iCtr].ColumnMapping != MappingType.Hidden)
                            //////////////////        {
                            //////////////////            oTblRow = new TableRow();
                            //////////////////            if ((_GroupLower.Contains(_DataSet.Tables[0].Columns[iCtr].ColumnName.ToLower()) && _GrpDisp) || !_GroupLower.Contains(_DataSet.Tables[0].Columns[iCtr].ColumnName.ToLower()))
                            //////////////////            {
                            //////////////////                TableCell oTblCell = new TableCell();
                            //////////////////                oTblCell.ApplyStyle(_hdr);
                            //////////////////                if (pCtr == _Ship.ShippingImagePosition && _ECom)
                            //////////////////                {
                            //////////////////                    IsShipImageAdded = true;
                            //////////////////                    oTblCell.ApplyStyle(_hdr);
                            //////////////////                    oTblCell.Text = _Ship.ShippingCaption;
                            //////////////////                    oTblRow.Cells.Add(oTblCell);
                            //////////////////                    oTblCell = new TableCell();
                            //////////////////                    HyperLink oHyp = new HyperLink();
                            //////////////////                    Image ShipImg = new Image();
                            //////////////////                    if (oDR["IS_SHIPPING"].ToString() == "False")
                            //////////////////                    {
                            //////////////////                        ShipImg.ImageUrl = _Ship.NoShippingImagePath;
                            //////////////////                        //ShipImg = Helper.WebCatGlb["NO SHIPPING IMAGE"];
                            //////////////////                    }
                            //////////////////                    else
                            //////////////////                    {
                            //////////////////                        ShipImg.ImageUrl = _Ship.ShippingImagePath;
                            //////////////////                        oHyp.NavigateUrl = TBParse(_Ship.ShippingURL, oDR);
                            //////////////////                    }
                            //////////////////                    oHyp.Controls.Add(ShipImg);

                            //////////////////                    oTblCell.Controls.Add(oHyp);
                            //////////////////                    oTblCell.HorizontalAlign = HorizontalAlign.Center;
                            //////////////////                    oTblRow.Cells.Add(oTblCell);
                            //////////////////                    oTable.Rows.Add(oTblRow);
                            //////////////////                    oTblRow = new TableRow();
                            //////////////////                    oTblCell = new TableCell();
                            //////////////////                    oTblCell.ApplyStyle(_hdr);
                            //////////////////                }

                            //////////////////                if (pCtr == _Cart.CartImagePosition && _ECom)
                            //////////////////                {
                            //////////////////                    IsCartImageAdded = true;
                            //////////////////                    oTblCell.ApplyStyle(_hdr);
                            //////////////////                    oTblCell.Text = _Cart.Caption;
                            //////////////////                    oTblRow.Cells.Add(oTblCell);
                            //////////////////                    oTblCell = new TableCell();
                            //////////////////                    HyperLink oHyp = new HyperLink();
                            //////////////////                    if (_EnableRestProduct.ToString() == "YES")
                            //////////////////                    {
                            //////////////////                        if (oDR["RESTRICTED"].ToString() == "YES")
                            //////////////////                        {
                            //////////////////                            Label oLbl = new Label();
                            //////////////////                            oLbl.Text = _Cart.RestrictedProdText;
                            //////////////////                            oHyp.Controls.Add(oLbl);
                            //////////////////                            oHyp.NavigateUrl = TBParse(_Cart.RestrictedProdURL, oDR);
                            //////////////////                        }
                            //////////////////                        else
                            //////////////////                        {
                            //////////////////                            Image CartImg = new Image();
                            //////////////////                            CartImg.ImageUrl = _Cart.CartImagePath;
                            //////////////////                            oHyp.Controls.Add(CartImg);
                            //////////////////                            oHyp.NavigateUrl = TBParse(_Cart.CartURL, oDR);
                            //////////////////                        }
                            //////////////////                    }
                            //////////////////                    else
                            //////////////////                    {
                            //////////////////                        Image CartImg = new Image();
                            //////////////////                        CartImg.ImageUrl = _Cart.CartImagePath;
                            //////////////////                        oHyp.Controls.Add(CartImg);
                            //////////////////                        oHyp.NavigateUrl = TBParse(_Cart.CartURL, oDR);
                            //////////////////                    }

                            //////////////////                    oTblCell.HorizontalAlign = HorizontalAlign.Center;
                            //////////////////                    oTblCell.Controls.Add(oHyp);
                            //////////////////                    oTblRow.Cells.Add(oTblCell);
                            //////////////////                    oTable.Rows.Add(oTblRow);
                            //////////////////                    oTblRow = new TableRow();
                            //////////////////                    oTblCell = new TableCell();
                            //////////////////                    oTblCell.ApplyStyle(_hdr);
                            //////////////////                }



                            //////////////////                string _caption;
                            //////////////////                _caption = _DataSet.Tables[0].Columns[iCtr].Caption.ToString();
                            //////////////////                if (_caption == null) _caption = _DataSet.Tables[0].Columns[iCtr].ColumnName.ToString();
                            //////////////////                //oTblCell.CssClass = base.CssClass;
                            //////////////////                oTblCell.Text = _caption;
                            //////////////////                oTblRow.Cells.Add(oTblCell);
                            //////////////////                nCols++;

                            //////////////////                oTblCell = new TableCell();
                            //////////////////                //oTblCell.Width = "70%";
                            //////////////////                oTblCell.ApplyStyle(_Row);
                            //////////////////                HyperLink oHypValue;
                            //////////////////                oHypValue = GetHyperLink(_DataSet.Tables[0].Columns[iCtr].ColumnName, oDR[iCtr].ToString(), oDR);
                            //////////////////                if (oHypValue != null)
                            //////////////////                {
                            //////////////////                    oTblCell.Controls.Add(oHypValue);
                            //////////////////                }
                            //////////////////                else if (_DataSet.Tables[0].Columns[iCtr].DataType == typeof(System.Decimal))
                            //////////////////                {
                            //////////////////                    oTblCell.HorizontalAlign = HorizontalAlign.Right;
                            //////////////////                    oTblCell.Text = _Currency + oDR[iCtr].ToString();
                            //////////////////                }
                            //////////////////                else if (_DataSet.Tables[0].Columns[iCtr].ColumnName.ToString() == "PRODUCT_IMAGE")
                            //////////////////                {
                            //////////////////                    oTblCell.Text = "<IMG SRC=" + oHelper.GetOptionValues("PRODUCT IMAGE PATH").ToString() + oDR[iCtr].ToString() + "\"/>";
                            //////////////////                }
                            //////////////////                else
                            //////////////////                {
                            //////////////////                    oTblCell.Text = "&nbsp;" + oDR[iCtr].ToString();
                            //////////////////                }
                            //////////////////                oTblRow.Cells.Add(oTblCell);
                            //////////////////                pCtr = pCtr + 1;
                            //////////////////            }
                            //////////////////            ct = ct + 1;

                            //////////////////        }
                            //////////////////        oTable.Rows.Add(oTblRow);
                            //////////////////    }
                            //////////////////    //ct = ct - 3;
                            //////////////////    //Set the Shipping image in last position for Flat layout.

                            //////////////////    if (IsShipImageAdded == false)
                            //////////////////    {
                            //////////////////        if (ct == _Ship.ShippingImagePosition && _ECom)
                            //////////////////        {
                            //////////////////            IsShipImageAdded = true;
                            //////////////////            oTblRow = new TableRow();
                            //////////////////            TableCell oTblCell = new TableCell();
                            //////////////////            oTblCell.ApplyStyle(_hdr);
                            //////////////////            oTblCell.Text = _Ship.ShippingCaption;
                            //////////////////            oTblRow.Cells.Add(oTblCell);
                            //////////////////            oTblRow.Cells.Add(oTblCell);
                            //////////////////            oTable.Rows.Add(oTblRow);
                            //////////////////            oTblCell = new TableCell();
                            //////////////////            HyperLink oHyp = new HyperLink();
                            //////////////////            Image ShipImg = new Image();
                            //////////////////            if (oDR["IS_SHIPPING"].ToString() == "False")
                            //////////////////            {
                            //////////////////                ShipImg.ImageUrl = _Ship.NoShippingImagePath;
                            //////////////////                //ShipImg = Helper.WebCatGlb["NO SHIPPING IMAGE"];
                            //////////////////            }
                            //////////////////            else
                            //////////////////            {
                            //////////////////                ShipImg.ImageUrl = _Ship.ShippingImagePath;
                            //////////////////                oHyp.NavigateUrl = TBParse(_Ship.ShippingURL, oDR);
                            //////////////////            }
                            //////////////////            oHyp.Controls.Add(ShipImg);

                            //////////////////            oTblCell.Controls.Add(oHyp);
                            //////////////////            oTblCell.HorizontalAlign = HorizontalAlign.Center;
                            //////////////////            oTblRow.Cells.Add(oTblCell);
                            //////////////////            oTable.Rows.Add(oTblRow);
                            //////////////////            oTblCell.ApplyStyle(_Row);
                            //////////////////        }
                            //////////////////    }
                            //////////////////    //Set the cart image in last position for Flat layout.
                            //////////////////    if (IsCartImageAdded == false)
                            //////////////////    {
                            //////////////////        if (ct == _Cart.CartImagePosition && _ECom)
                            //////////////////        {
                            //////////////////            IsCartImageAdded = true;
                            //////////////////            oTblRow = new TableRow();
                            //////////////////            TableCell oTblCell = new TableCell();
                            //////////////////            oTblCell.ApplyStyle(_hdr);
                            //////////////////            oTblCell.Text = _Cart.Caption;
                            //////////////////            oTblRow.Cells.Add(oTblCell);
                            //////////////////            oTblRow.Cells.Add(oTblCell);
                            //////////////////            oTable.Rows.Add(oTblRow);
                            //////////////////            oTblCell = new TableCell();
                            //////////////////            HyperLink oHyp = new HyperLink();
                            //////////////////            if (_EnableRestProduct.ToString() == "YES")
                            //////////////////            {
                            //////////////////                if (oDR["RESTRICTED"].ToString() == "YES")
                            //////////////////                {
                            //////////////////                    Label oLbl = new Label();
                            //////////////////                    oLbl.Text = _Cart.RestrictedProdText;
                            //////////////////                    oHyp.Controls.Add(oLbl);
                            //////////////////                    oHyp.NavigateUrl = TBParse(_Cart.RestrictedProdURL, oDR);
                            //////////////////                }
                            //////////////////                else
                            //////////////////                {
                            //////////////////                    Image CartImg = new Image();
                            //////////////////                    CartImg.ImageUrl = _Cart.CartImagePath;
                            //////////////////                    oHyp.Controls.Add(CartImg);
                            //////////////////                    oHyp.NavigateUrl = TBParse(_Cart.CartURL, oDR);
                            //////////////////                }
                            //////////////////            }
                            //////////////////            else
                            //////////////////            {
                            //////////////////                Image CartImg = new Image();
                            //////////////////                CartImg.ImageUrl = _Cart.CartImagePath;
                            //////////////////                oHyp.Controls.Add(CartImg);
                            //////////////////                oHyp.NavigateUrl = TBParse(_Cart.CartURL, oDR);
                            //////////////////            }

                            //////////////////            oTblCell.HorizontalAlign = HorizontalAlign.Center;
                            //////////////////            oTblCell.Controls.Add(oHyp);
                            //////////////////            oTblRow.Cells.Add(oTblCell);
                            //////////////////            oTable.Rows.Add(oTblRow);
                            //////////////////            oTblCell.ApplyStyle(_Row);
                            //////////////////        }
                            //////////////////    }

                            //////////////////    oTable.RenderControl(writer);
                            //////////////////    writer.Write("</BR>");
                            //////////////////}
                            //////////////////oTable.Width = _TBGridWidth;
                            //  oTable.RenderControl(writer);
                            // writer.Write("</BR>");
                            StringBuilder oStrBldr = new StringBuilder();
                            string TempProdImage = "";
                            string TempDesc = "";
                            string TempSupFamName = "";
                            int NoofColumns = 3, i = 1, NoofRows = 30;
                            int NoofItems = 1;
                            int rowCnt = 1; int count = 0;
                            DataRow[] newrods;
                            //NoofItems = _DataSet.Tables[0].Rows.Count;
                            string _txtSearch;
                            string LabelText = "";
                            if (_Paging)
                            {
                                oTable = new Table();
                                oTable.BorderWidth = Unit.Pixel(0);
                                oTblRow = new TableRow();
                                TableCell oTblCell = new TableCell();
                                oTblCell.ColumnSpan = nCols + 2;
                                oTblCell.HorizontalAlign = HorizontalAlign.Right;
                                oTblCell.ApplyStyle(_Row);
                                Label oLbl = new Label();
                                string _toURL = Page.Request.FilePath.ToString();
                                _txtSearch = Page.Request["txtSearch"].ToString();
                                if (_txtSearch.Length > 0)
                                    _txtSearch = "txtSearch=" + _txtSearch + "&";

                                if (_currentPage > 0)
                                {
                                    if (HttpContext.Current.Request["SortCol"] != null)
                                    {
                                        //LabelText = "<font face=\"webdings\" size=\"2\"><a href=\"" + _toURL + "?" + _txtSearch + "Pg=0" + "&SortCol=" + HttpContext.Current.Request["SortCol"] + "\"style=\"text-decoration:none\">9</a><a href=\"" + _toURL + "?" + _txtSearch + "Pg=" + oHelper.CS(_currentPage - 1) + "&SortCol=" + HttpContext.Current.Request["SortCol"] + "\" style=\"text-decoration:none\">7</a></font>";
                                        LabelText = "<a href=\"" + _toURL + "?" + _txtSearch + "Pg=0" + "&SortCol=" + HttpContext.Current.Request["SortCol"] + "\"style=\"text-decoration:none\"><img src=\"images/btnfirst.jpg\" border=0/></a><a href=\"" + _toURL + "?" + _txtSearch + "Pg=" + oHelper.CS(_currentPage - 1) + "&SortCol=" + HttpContext.Current.Request["SortCol"] + "\" style=\"text-decoration:none\"><img src=\"images/btnprevious.jpg\" border=0 valign=bottom/></a>";
                                    }
                                    else
                                    {
                                        //LabelText = "<font face=\"webdings\" size=\"2\"><a href=\"" + _toURL + "?" + _txtSearch + "Pg=0" + "&SortCol=" + HttpContext.Current.Request["SortCol"] + "\"style=\"text-decoration:none\">9</a><a href=\"" + _toURL + "?" + _txtSearch + "Pg=" + oHelper.CS(_currentPage - 1) + "&SortCol=" + HttpContext.Current.Request["SortCol"] + "\" style=\"text-decoration:none\">7</a></font>";
                                        LabelText = "<a href=\"" + _toURL + "?" + _txtSearch + "Pg=0" + "&SortCol=" + HttpContext.Current.Request["SortCol"] + "\"style=\"text-decoration:none\"><img src=\"images/btnfirst.jpg\" border=0/></a><a href=\"" + _toURL + "?" + _txtSearch + "Pg=" + oHelper.CS(_currentPage - 1) + "&SortCol=" + HttpContext.Current.Request["SortCol"] + "\" style=\"text-decoration:none\"><img src=\"images/btnprevious.jpg\" border=0/></a>";
                                    }
                                }
                                //LabelText = LabelText + "Page ";
                                //LabelText = "<font style=\"font-family:Arial,@Arial Unicode MS;font-size:12px;vertical-align:top\">" + LabelText + oHelper.CS(_currentPage + 1) + " of " + oHelper.CS(_TotalPages + 1) + "</font>";
                                LabelText = LabelText + "<font style=\"font-family:Arial,@Arial Unicode MS;font-size:12px;vertical-align:top\">Page ";
                                LabelText = LabelText + oHelper.CS(_currentPage + 1) + " of " + oHelper.CS(_TotalPages + 1) + "</font>";

                                if (_currentPage < _TotalPages)
                                {
                                    if (HttpContext.Current.Request["SortCol"] != null)
                                    {
                                        //LabelText = LabelText + "<font face=\"webdings\" size=\"2\"><a href=\"" + _toURL + "?" + _txtSearch + "Pg=" + oHelper.CS(_currentPage + 1) + "&SortCol=" + HttpContext.Current.Request["SortCol"] + "\" style=\"text-decoration:none\">8</a><a href=\"" + _toURL + "?" + _txtSearch + "Pg=" + _TotalPages + "&SortCol=" + HttpContext.Current.Request["SortCol"] + "\"  style=\"text-decoration:none\">:</a></font>";
                                        LabelText = LabelText + "<a href=\"" + _toURL + "?" + _txtSearch + "Pg=" + oHelper.CS(_currentPage + 1) + "&SortCol=" + HttpContext.Current.Request["SortCol"] + "\" style=\"text-decoration:none\"><img src=\"images/btnnext.jpg\" border=0/></a><a href=\"" + _toURL + "?" + _txtSearch + "Pg=" + _TotalPages + "&SortCol=" + HttpContext.Current.Request["SortCol"] + "\"  style=\"text-decoration:none\"><img src=\"images/btnlast.jpg\" border=0/></a>";
                                    }
                                    else
                                    {
                                        //LabelText = LabelText + "<font face=\"webdings\" size=\"2\"><a href=\"" + _toURL + "?" + _txtSearch + "Pg=" + oHelper.CS(_currentPage + 1) + "\" style=\"text-decoration:none\">8</a><a href=\"" + _toURL + "?" + _txtSearch + "Pg=" + _TotalPages + "\"  style=\"text-decoration:none\">:</a></font>";
                                        LabelText = LabelText + "<a href=\"" + _toURL + "?" + _txtSearch + "Pg=" + oHelper.CS(_currentPage + 1) + "\" style=\"text-decoration:none\"><img src=\"images/btnnext.jpg\" border=0/></a><a href=\"" + _toURL + "?" + _txtSearch + "Pg=" + _TotalPages + "\"  style=\"text-decoration:none\"><img src=\"images/btnlast.jpg\" border=0/></a>";
                                    }
                                }
                            }


                            //SORT IMPLEMENTATION STARTED
                            DataSet _DatasetDS = new DataSet();
                            string CatAttrName = "CATALOG_ITEM_NO";
                            string PriceAttrName = "PRICE";
                            string AttrName = "";
                            if (HttpContext.Current.Request["SortCol"] != null && HttpContext.Current.Request["SortCol"].ToString() != "")
                            {
                                DataSet oDsSort = new DataSet();
                                string SortCol = HttpContext.Current.Request["SortCol"].ToString();
                                int Sortcontent = SortCol.IndexOf("]");
                                string SortcontStr = SortCol.Substring(Sortcontent + 2);
                                DataSet StringDataSet = new DataSet();
                                //AttrName = PriceAttrName;
                                AttrName = "[" + PriceAttrName + "]" + " " + SortcontStr;
                                if (SortCol.ToString().Trim() == AttrName || SortCol.ToString().Trim() == AttrName)
                                {
                                    //PriceAttrName = "[" + PriceAttrName + "]" + " " + SortcontStr;
                                    //oPRODDataDS = oPRODData.Copy();
                                    DataTable oResultTable = new DataTable();
                                    foreach (DataColumn oDC in _DataSet.Tables[0].Columns)
                                    {

                                        if (oDC.ColumnName.Equals(AttrName))
                                        {
                                            DataColumn oNewColumn = new DataColumn(oDC.ColumnName, System.Type.GetType("System.Decimal"));
                                            oResultTable.Columns.Add(oNewColumn);
                                        }
                                        else
                                        {
                                            oResultTable.Columns.Add(oDC.ColumnName.ToString(), System.Type.GetType(oDC.DataType.FullName.ToString()));
                                        }
                                    }
                                    DataRow[] oDR = _DataSet.Tables[0].Select();
                                    foreach (DataRow oDataRow in oDR)
                                    {
                                        oResultTable.ImportRow(oDataRow);
                                    }
                                    _DatasetDS.Tables.Add(oResultTable);

                                    //Sorting Based on arguments columns..
                                    DataRow[] oDrSort = _DatasetDS.Tables[0].Select("", SortCol);
                                    oDsSort = _DatasetDS.Clone();
                                    _DatasetDS.Tables.Clear();

                                    foreach (DataRow oDr in oDrSort)
                                    {
                                        oDsSort.Tables[0].ImportRow(oDr);
                                    }
                                    _DatasetDS = oDsSort;
                                    //oDRs = _DatasetDS.Tables[0].Select(sFilter, groupcol);
                                    oDRs = _DatasetDS.Tables[0].Select("", SortCol);
                                }
                                else
                                {
                                    //copy data from original DataSet to Temp DS
                                    _DatasetDS = _DataSet.Copy();
                                    DataRow[] oDrSort = _DatasetDS.Tables[0].Select("", SortCol);
                                    oDsSort = _DatasetDS.Clone();
                                    _DatasetDS.Tables.Clear();

                                    foreach (DataRow oDr in oDrSort)
                                    {
                                        oDsSort.Tables[0].ImportRow(oDr);
                                    }
                                    _DatasetDS = oDsSort;
                                    oDRs = _DatasetDS.Tables[0].Select("", SortCol);

                                }
                            }
                            else
                            {
                                //copy data from original DataSet to Temp DS
                                _DatasetDS = _DataSet.Copy();
                            }

                            //if (_DispMode == DisplayModes.Flat)
                            //{
                            //    oStrBldr.Append("<TABLE BORDER=\"0\" CELLPADDING =\"0\" CELLSPACING =\"0\"><TR><TD><FONT FACE=Arial Unicode MS SIZE=2>Sort products:</FONT><SELECT id=\"SelSort\" style=\"font-family:Arial Unicode MS:Arial;font-size:12px;\">");
                            //    oStrBldr.Append("<OPTION SELECTED=\"SELECTED\" VALUE= \"" + "[" + CatAttrName + "] ASC" + "\" >Item Ascending</OPTION>");
                            //    oStrBldr.Append("<OPTION VALUE= \"" + "[" + CatAttrName + "] DESC" + "\" >Item Descending</OPTION>");
                            //    oStrBldr.Append("<OPTION VALUE= \" " + "[" + PriceAttrName + "] ASC" + "\" >Price Low to High</OPTION>");
                            //    oStrBldr.Append("<OPTION VALUE= \"" + "[" + PriceAttrName + "] DESC" + " \" >Price High to Low</OPTION>");
                            //    oStrBldr.Append("</SELECT></TD>");
                            //    //oStrBldr.Append("<TD>&nbsp<INPUT TYPE=\"button\" id =\"btnsub\"  value=\"go\" onclick=\"javascript:GetName(SelSort.value," + Page.Request["txtSearch"].ToString() + ");\" style=\"font-family:Arial Unicode MS:Arial;font-size:12px; font-weight:bold\"/></TD>");
                            //    oStrBldr.Append("<TD>&nbsp<INPUT TYPE=\"button\" id =\"btnsub\"  value=\"go\" onclick=\"javascript:GetName('" + Page.Request["txtSearch"].ToString() + "',SelSort.value);\" style=\"font-family:Arial Unicode MS:Arial;font-size:12px; font-weight:bold\"/></TD>");
                            //    oStrBldr.Append("</TR></TABLE>");

                            //}

                            //oStrBldr.Append(System.Environment.NewLine + "<TABLE ID=\"topband\" WIDTH=\"100%\" CELLPADDING=\"0\" CELLSPACING=\"0\" BORDER=\"0\" HEIGHT=\"5px\"><TR><TD bgColor=#309000 COLSPAN=" + NoofColumns + " HEIGHT=\"5px\"><STRONG><FONT face=Verdana color=#ffffff>Search Result for \"" + Page.Request["txtSearch"].ToString() + "\"</FONT></STRONG> </TD></TR><TR></TR><TR></TR></TABLE>" + System.Environment.NewLine);

                            if (_DispMode == DisplayModes.Flat)
                            {
                                oStrBldr.Append("<TABLE BORDER=\"0\" CELLPADDING =\"0\" CELLSPACING =\"0\"><TR><TD><FONT FACE=Arial Unicode MS SIZE=2>Sort products:</FONT><SELECT id=\"SelSort\" style=\"font-family:Arial Unicode MS:Arial;font-size:12px;\">");
                                oStrBldr.Append("<OPTION SELECTED=\"SELECTED\" VALUE= \"" + "[" + CatAttrName + "] ASC" + "\" >Item Ascending</OPTION>");
                                oStrBldr.Append("<OPTION VALUE= \"" + "[" + CatAttrName + "] DESC" + "\" >Item Descending</OPTION>");
                                oStrBldr.Append("<OPTION VALUE= \" " + "[" + PriceAttrName + "] ASC" + "\" >Price Low to High</OPTION>");
                                oStrBldr.Append("<OPTION VALUE= \"" + "[" + PriceAttrName + "] DESC" + " \" >Price High to Low</OPTION>");
                                oStrBldr.Append("</SELECT></TD>");
                                //oStrBldr.Append("<TD>&nbsp<INPUT TYPE=\"button\" id =\"btnsub\"  value=\"go\" onclick=\"javascript:GetName(SelSort.value," + Page.Request["txtSearch"].ToString() + ");\" style=\"font-family:Arial Unicode MS:Arial;font-size:12px; font-weight:bold\"/></TD>");
                                oStrBldr.Append("<TD>&nbsp<INPUT TYPE=\"button\" id =\"btnsub\"  value=\"go\" onclick=\"javascript:GetName('" + Page.Request["txtSearch"].ToString() + "',SelSort.value);\" style=\"font-family:Arial Unicode MS:Arial;font-size:12px; font-weight:bold\"/></TD>");
                                oStrBldr.Append("</TR></TABLE>");

                            }
                            oStrBldr.Append("<TABLE ID=\"TBLSEARCH\" CELLPADDING=\"5\" CELLSPACING=\"1\" BORDER=\"0\" WIDTH=\"100%\">" + System.Environment.NewLine + "<TR><TD COLSPAN=" + NoofColumns + " ALIGN=\"RIGHT\">");
                            oStrBldr.Append(LabelText.ToString() + "</TD></TR><TR>");
                            System.Drawing.Size newVal;
                            string ProdImgPath = oHelper.GetOptionValues("PRODUCT IMAGE PATH").ToString();
                            foreach (DataRow oDR in oDRs)
                            {
                                if (rowCnt > _currentStart && rowCnt <= _currentEnd)
                                {

                                    string TempNaviUrl = TBParse(_NaviURL, oDR);
                                    if (i <= NoofColumns)
                                    {
                                        oStrBldr.Append(System.Environment.NewLine + "<TD>");
                                        oStrBldr.Append(System.Environment.NewLine + "<TABLE CELLPADDING=\"1\" CELLSPACING=\"1\" BORDER=\"0\" WIDTH=\"150px\">");

                                        for (int iCtr = 0; iCtr < _DataSet.Tables[0].Columns.Count; iCtr++)
                                        {
                                            if (_DataSet.Tables[0].Columns[iCtr].ColumnMapping != MappingType.Hidden)
                                            {
                                                if ((_GroupLower.Contains(_DataSet.Tables[0].Columns[iCtr].ColumnName.ToLower()) && _GrpDisp) || !_GroupLower.Contains(_DataSet.Tables[0].Columns[iCtr].ColumnName.ToLower()))
                                                {
                                                    if (_DataSet.Tables[0].Columns[iCtr].ColumnName.ToString() == "PRODUCT_IMAGE")
                                                    {
                                                        string ImgVal = oDR[iCtr].ToString();
                                                        ImgVal = ImgVal.Replace("\\", "/");
                                                        if ((File.Exists(HttpContext.Current.Server.MapPath(ProdImgPath + ImgVal)) == true) && ImgVal != null)
                                                        {
                                                            System.Drawing.Image oImg = System.Drawing.Image.FromFile(HttpContext.Current.Server.MapPath(ProdImgPath + ImgVal));
                                                            newVal = ScaleImage(oImg.Height, oImg.Width, _ImgWidth, _ImgHeight);
                                                            TempProdImage = "<A HREF=\"" + TempNaviUrl + "\" style=\"text-decoration:none;border:0px\"><IMG width=" + newVal.Width + " height=" + newVal.Height + " SRC=\"" + ProdImgPath + ImgVal + "\" BORDER=\"0\" ALT=\"" + GetImageNameFromURL(ImgVal) + "\"/></A>";
                                                        }
                                                        else if (ImgVal != string.Empty)
                                                        {
                                                            TempProdImage = "<A HREF=\"" + TempNaviUrl + "\" style=\"text-decoration:none;border:0px\"><IMG width=" + _ImgWidth + " height=" + _ImgHeight + " SRC=\"" + ProdImgPath + ImgVal + "\" BORDER=\"0\" ALT=\"" + GetImageNameFromURL(ImgVal) + "\"/></A>";
                                                        }
                                                        else
                                                        {
                                                            string NoImg = "images" + "\\" + "NoImage.gif";
                                                            TempProdImage = "<A HREF=\"" + TempNaviUrl + "\" style=\"text-decoration:none;border:0px\"><IMG width=" + _ImgWidth + " height=" + _ImgHeight + " SRC=" + NoImg + "  BORDER=\"0\" ALT=\"No Image to Display\"/></A>";
                                                        }
                                                    }
                                                    if ((_DataSet.Tables[0].Columns[iCtr].ColumnName.ToString() == "SUPPLIER_NAME"))
                                                    {
                                                        TempSupFamName = "<A HREF=\"" + TempNaviUrl + "\" style=\"text-decoration:none;\"><STRONG><FONT SIZE=2 color= " + SearchAttrColor + " FACE=ARIAL>" + oDR["SUPPLIER_NAME"].ToString() + " " + oDR["FAMILY_NAME"].ToString() + "</FONT></STRONG></A>";
                                                    }
                                                    if (_DataSet.Tables[0].Columns[iCtr].ColumnName.ToString() == "FEATURE")
                                                    {
                                                        TempDesc = "<A HREF=\"" + TempNaviUrl + "\">" + "<STRONG><FONT FACE=Arial SIZE=3 color= \"RED\">" + oDR["SUPPLIER_NAME"].ToString() + "</FONT>" + " " + "<FONT SIZE=2 color= " + SearchAttrColor + " FACE=ARIAL>" + oDR["FAMILY_NAME"].ToString() + "</FONT><BR/><FONT SIZE=\"1\" FACE=\"ARIAL\">" + oDR[iCtr].ToString() + "</FONT></STRONG></A>";
                                                    }
                                                    if (_DataSet.Tables[0].Columns[iCtr].DataType == typeof(System.Decimal) && oHelper.CDEC(oDR[iCtr].ToString()) != 0)
                                                    {
                                                        oStrBldr.Append(System.Environment.NewLine + "<TR>" + System.Environment.NewLine + "<TD ALIGN=\"CENTER\">" + System.Environment.NewLine);
                                                        oStrBldr.Append("<A HREF=\"" + TempNaviUrl + "\"><FONT SIZE=1 face=Arial><STRONG>Click for more details</STRONG></FONT></A>");
                                                        oStrBldr.Append(System.Environment.NewLine + "</TD>" + System.Environment.NewLine + "</TR>" + System.Environment.NewLine);

                                                        oStrBldr.Append(System.Environment.NewLine + "<TR>" + System.Environment.NewLine + "<TD ALIGN=\"CENTER\">" + System.Environment.NewLine);
                                                        oStrBldr.Append("<FONT color=#ff0000 size=1 face=Arial>Price:" + _Currency + oDR[iCtr].ToString() + "</FONT>");
                                                        oStrBldr.Append(System.Environment.NewLine + "</TD>" + System.Environment.NewLine + "</TR>" + System.Environment.NewLine);
                                                    }
                                                    else if (_DataSet.Tables[0].Columns[iCtr].ColumnName.ToString() != "PRODUCT_IMAGE" && _DataSet.Tables[0].Columns[iCtr].ColumnName.ToString() != "FEATURE")
                                                    {
                                                        if (_DataSet.Tables[0].Columns[iCtr].ColumnName.ToString() == "CATALOG_ITEM_NO")
                                                        {
                                                            oStrBldr.Append(System.Environment.NewLine + "<TR>" + System.Environment.NewLine + "<TD ALIGN=\"CENTER\">" + System.Environment.NewLine);
                                                            oStrBldr.Append("<FONT SIZE=1 face=Arial><STRONG>Item #: </STRONG>" + oDR[iCtr].ToString() + "</FONT>");
                                                            oStrBldr.Append(System.Environment.NewLine + "</TD>" + System.Environment.NewLine + "</TR>" + System.Environment.NewLine);
                                                        }
                                                    }
                                                    //oStrBldr.Append(System.Environment.NewLine + "</TD>" + System.Environment.NewLine + "</TR>" + System.Environment.NewLine);
                                                }
                                            }
                                        }
                                        TempProdImage = System.Environment.NewLine + "<TR>" + System.Environment.NewLine + "<TD ALIGN=\"CENTER\">" + TempProdImage + "</TD>" + System.Environment.NewLine + "</TR>" + System.Environment.NewLine;
                                        TempDesc = System.Environment.NewLine + "<TR>" + System.Environment.NewLine + "<TD ALIGN=\"CENTER\" width=\"40%\">" + TempDesc + "</TD>" + System.Environment.NewLine + "</TR>" + System.Environment.NewLine;
                                        TempSupFamName = System.Environment.NewLine + "<TR>" + System.Environment.NewLine + "<TD ALIGN=\"CENTER\" width=\"40%\">" + TempSupFamName + "</TD>" + System.Environment.NewLine + "</TR>" + System.Environment.NewLine;
                                        int TempLoc = oStrBldr.ToString().LastIndexOf("<TABLE");
                                        TempLoc = oStrBldr.ToString().IndexOf(">", TempLoc) + 1;
                                        //oStrBldr.Insert(TempLoc, TempProdImage + TempDesc);
                                        oStrBldr.Insert(TempLoc, TempProdImage + TempSupFamName);
                                        string TempCartUrl = TBParse(_Cart.CartURL, oDR);
                                        if (_ECom)
                                        {
                                            string TempCartString = "<A HREF=\"" + TempCartUrl + "\"> <IMG SRC=\"" + _Cart.CartImagePath + "\" BORDER=\"0\"/></A>";
                                            oStrBldr.Append(System.Environment.NewLine + "<TR>" + System.Environment.NewLine + "<TD ALIGN=\"CENTER\">" + TempCartString + "</TD>" + System.Environment.NewLine + "</TR>" + System.Environment.NewLine);
                                        }
                                        oStrBldr.Append("</TABLE>");
                                        oStrBldr.Append(System.Environment.NewLine + "</TD>");
                                        i++;
                                    }
                                    if (i > NoofColumns)
                                    {
                                        if (NoofItems == _DataSet.Tables[0].Rows.Count)
                                        {
                                            oStrBldr.Append("</TR>");
                                            i = 1;
                                        }
                                        else
                                        {
                                            oStrBldr.Append("</TR><TR>");
                                            i = 1;
                                        }
                                    }
                                    NoofItems++;
                                }

                                if (rowCnt == _currentEnd + 1)
                                {
                                    break;
                                }
                                else
                                {
                                    rowCnt = rowCnt + 1;
                                }
                            }
                            oStrBldr.Append("<TR><TD COLSPAN=" + NoofColumns + " ALIGN=\"RIGHT\">" + LabelText.ToString() + "</TD></TR></TABLE>");
                            //oStrBldr.Append("</TABLE>");

                            //Server side paging

                            FlatViewHtml = oStrBldr.ToString();

                            //Java Script paging..
                            //if ((NoofItems > (NoofRows * NoofColumns)))
                            //{
                            //    FlatViewHtml = BuildCategoryTemplateHead() + "<BODY>" + oStrBldr.ToString() + BuildCategoryTemplatePaging(NoofRows);
                            //}
                            //else
                            //{
                            //    FlatViewHtml = oStrBldr.ToString();
                            //}
                            HttpContext.Current.Response.Write(FlatViewHtml);
                            oTable.RenderControl(writer);

                        }
                        #endregion
                }//Dataset Check conditions
            }
        }
        //Build the Paging Content
        private string BuildCategoryTemplatePaging(int Pagesize)
        {
            string Paging = "";
            string TblbodyEnd = "";
            try
            {
                Paging = "<div id=\"pageNavPosition\"></div><script type=\"text/javascript\">"
                      + "var pager = new Pager('TBLSEARCH'," + Pagesize + ");"
                      + "pager.init();pager.showPageNav('pager', 'pageNavPosition');"
                      + "pager.showPage(1);</script>";
                TblbodyEnd = Paging + "</body></Html>";
            }
            catch (Exception Ex)
            {
                //oErr.ErrorMsg = Ex;
                //oErr.CreateLog();
                return "";
            }
            return TblbodyEnd;
        }

        public System.Drawing.Size ScaleImage(double origHeight, double origWidth, double Width, double Height)
        {
            System.Drawing.Size newSize = new System.Drawing.Size();
            double nWidth = Width;
            double nHeight = Height;
            double oWidth = origWidth;
            double oHeight = origHeight;
            if (origHeight > nHeight || origWidth > nWidth)
            {
                if (oWidth > oHeight)
                {
                    double Ratio = (double)((double)(oHeight) / (double)(oWidth));
                    double Final = (nWidth) * Ratio;
                    nHeight = (int)Final;
                }
                else
                {
                    double Ratio = (double)((double)(oWidth) / (double)(oHeight));
                    double Final = (nHeight) * Ratio;
                    nWidth = (int)Final;
                }
            }
            newSize.Height = (int)nHeight;
            newSize.Width = (int)nWidth;
            return newSize;
        }


        //paging
        private string BuildCategoryTemplateHead()
        {
            string TblHead = "";
            try
            {
                TblHead = "<head><script type=\"text/javascript\" src=\"TradingBellNavigation.js\"></script>"
                    //+ "<style type=\"text/css\">.pg-normal {color: black;font-weight: normal;text-decoration: none;cursor: pointer;}"
                    //+ ".pg-selected {color: black;font-weight: bold;text-decoration: underline;cursor: pointer;}</style>"
                        + "</head>";
            }
            catch (Exception Ex)
            {
                //oErr.ErrorMsg = Ex;
                //oErr.CreateLog();
                return "";
            }
            return TblHead;
        }

        public string GetImageNameFromURL(string ImageURL)
        {
            string TempUrl = "";
            string ImgFileNameWOExt = "";
            string ImgFileNameWExt = "";
            try
            {
                TempUrl = ImageURL.Replace("\\", "/");
                if (TempUrl.IndexOf("/") > -1)
                {
                    ImgFileNameWExt = TempUrl.Substring(TempUrl.LastIndexOf("/") + 1);
                }
                if (ImgFileNameWExt.IndexOf(".") > -1)
                {
                    ImgFileNameWOExt = ImgFileNameWExt.Remove(ImgFileNameWExt.IndexOf("."));
                }
            }
            catch (Exception ex)
            {

            }
            return ImgFileNameWOExt;
        }
    }

    [
    TypeConverter(typeof(ExpandableObjectConverter))
    ]

    public class HyperlinkColumn
    {
        string _FieldName;
        string _URL;

        public HyperlinkColumn()
            : this(String.Empty, string.Empty)
        {
        }

        public HyperlinkColumn(string FieldName, string URL)
        {
            _FieldName = FieldName;
            _URL = URL;
        }

        public string FieldName
        {
            get
            {
                return _FieldName;
            }
            set
            {
                _FieldName = value;
            }
        }

        public string URL
        {
            get
            {
                return _URL;
            }
            set
            {
                _URL = value;
            }
        }

    }

    [
    TypeConverter(typeof(ExpandableObjectConverter))
    ]

    public class GroupingColumn
    {
        string _Fieldname;
        public GroupingColumn()
            : this(String.Empty)
        {
        }

        public GroupingColumn(string FieldName)
        {
            _Fieldname = FieldName;
        }

        public string FieldName
        {
            get
            {
                return _Fieldname;
            }
            set
            {
                _Fieldname = value;
            }
        }
    }

    public class HyperlinkColumnCollectionEditor : CollectionEditor
    {
        public HyperlinkColumnCollectionEditor(Type type)
            : base(type)
        {
        }

        protected override bool CanSelectMultipleInstances()
        {
            return false;
        }

        protected override Type CreateCollectionItemType()
        {
            return typeof(HyperlinkColumn);
        }
    }


    public class GroupingColumnCollectionEditor : CollectionEditor
    {
        public GroupingColumnCollectionEditor(Type type)
            : base(type)
        {
        }

        protected override bool CanSelectMultipleInstances()
        {
            return false;
        }

        protected override Type CreateCollectionItemType()
        {
            return typeof(GroupingColumn);
        }
    }

    [
    TypeConverter(typeof(ExpandableObjectConverter))
    ]
    public class ShoppingCart
    {
        string _imgPath;
        string _ResProdURL;
        string _ResText;
        int _imgPos;
        string _URL;
        string _Caption;
        bool _Qty;

        [
        Browsable(true),
        Description("Assign Shopping Cart Column Caption"),
        ]
        public virtual string Caption
        {
            get
            {
                return _Caption;
            }

            set
            {
                _Caption = value;
            }
        }

        [
        Browsable(true),
        Description("Assign Cart Image path"),
        ]
        public string CartImagePath
        {
            get
            {
                return _imgPath;
            }
            set
            {
                _imgPath = value;
            }
        }

        [
        Browsable(true),
        Description("Assign Cart Image Position in Grid"),
        ]
        public int CartImagePosition
        {
            get
            {
                return _imgPos;
            }
            set
            {
                _imgPos = value;
            }
        }

        [
        Browsable(true),
        Description("Assign Cart URL"),
        ]
        public virtual string CartURL
        {
            get
            {
                return _URL;
            }
            set
            {
                _URL = value;
            }

        }

        [
    Browsable(true),
    Description("Restricted Product Text"),
    ]
        public virtual string RestrictedProdText
        {
            get
            {
                return _ResText;
            }
            set
            {
                _ResText = value;
            }

        }
        [
        Browsable(true),
        Description("Restricted Product URL"),
        ]
        public virtual string RestrictedProdURL
        {
            get
            {
                return _ResProdURL;
            }
            set
            {
                _ResProdURL = value;
            }

        }
        [
        Browsable(true),
        Description("Quantity Field Display")
        ]
        public virtual bool Quantity
        {
            get
            {
                return _Qty;
            }
            set
            {
                _Qty = value;
            }
        }
    }
    [
    TypeConverter(typeof(ExpandableObjectConverter))
    ]
    public class Shipping
    {
        string _simgPath;
        int _simgPos;
        string _sURL;
        string _sCaption;
        string _nsimgPath;

        [
        Browsable(true),
        Description("Assign Shipping Column Caption"),
        ]
        public virtual string ShippingCaption
        {
            get
            {
                return _sCaption;
            }

            set
            {
                _sCaption = value;
            }
        }

        [
        Browsable(true),
        Description("Assign Shipping Image Path"),
        ]
        public string ShippingImagePath
        {
            get
            {
                return _simgPath;
            }
            set
            {
                _simgPath = value;
            }
        }
        [
        Browsable(true),
        Description("Assign No Shipping Image Path"),
        ]
        public string NoShippingImagePath
        {
            get
            {
                return _nsimgPath;
            }
            set
            {
                _nsimgPath = value;
            }
        }


        [
        Browsable(true),
        Description("Assign Cart Image Position in Grid"),
        ]
        public int ShippingImagePosition
        {
            get
            {
                return _simgPos;
            }
            set
            {
                _simgPos = value;
            }
        }

        [
        Browsable(true),
        Description("Assign Cart URL"),
        ]
        public virtual string ShippingURL
        {
            get
            {
                return _sURL;
            }
            set
            {
                _sURL = value;
            }

        }
    }

}


