using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.ComponentModel;
using TradingBell.WebCat.CatalogDB;
using TradingBell.WebCat.Helpers;
using TradingBell.WebCat.TemplateRender;
using TradingBell.WebCat.CommonServices;
public partial class UI_SupplierList : System.Web.UI.UserControl
{
    HelperServices objHelperServices=new HelperServices();
    ErrorHandler objErrorHandler=new ErrorHandler();
    ProductServices objProductServices =new ProductServices();
    //public enum IndexHeadVisible
    //{
    //    Yes = 1,
    //    No = 2

    //}
    public enum SupplierListVisibleText
    {
        Yes =1,
        No =2

   }
    //int _DisplayCnt;
    // IndexHeadVisible _CurrentHeadVisible;
    string _HeadCssClass;
    string _HeadLinkCssClass;
    string _CssClass;
    string _LinkCssClass;
    SupplierListVisibleText _CurrentSupplierVisible;


    #region "Property"
    [
    Browsable(true),
    Category("TradingBell"),
    DefaultValue(""),
    Description("Display the supplier list")
    ]
    public SupplierListVisibleText SupplierListVisible
    {
        get
        {
            return _CurrentSupplierVisible;
        }
        set
        {
            _CurrentSupplierVisible = value;
        }

    }

    [
    Browsable(true),
    Category("TradingBell"),
    DefaultValue("")
    ]
    public string HeadCssClass
    {
        get
        {
            return _HeadCssClass;
        }
        set
        {
            _HeadCssClass = value;
        }
    }

    [
    Browsable(true),
    Category("TradingBell"),
    DefaultValue("")
    ]
    public string HeadLinkCssClass
    {
        get
        {
            return _HeadLinkCssClass;
        }
        set
        {
            _HeadLinkCssClass = value;
        }
    }




    [
    Browsable(true),
    Category("TradingBell"),
    DefaultValue("")
    ]
    public string LinkCssClass
    {
        get
        {
            return _LinkCssClass;
        }
        set
        {
            _LinkCssClass = value;
        }
    }
   
   [
    Browsable(true),
    Category("TradingBell"),
    DefaultValue("")
    ]
    public string CssClass
    {
        get
        {
            return _CssClass;
        }
        set
        {
            _CssClass = value;
        }
    }
    #endregion


    protected void Page_Load(object sender, EventArgs e)
    {
        if (!DesignMode)
        {
            
            Image imgBlank = new Image();
            Label lblTop = new Label();
            lblTop.Text = "<a name = \"top\" id =\"top\">Top</a>";
            string ImgPath = objHelperServices.GetOptionValues("IMAGE PATH").ToString();

            if (_CurrentSupplierVisible == SupplierListVisibleText.Yes)
            {
                imgBlank.ImageUrl = ImgPath + "/spacer.gif";
                imgBlank.Height = 20;
                LoadSupplierNameIndex();
                Controls.Add(imgBlank);
                LoadSupplierList();
            }
           
        }

    }
    #region "Functions"
    public void LoadSupplierNameIndex()
    {
        try
        {
            DataSet dsSuppIndex = new DataSet();
            Table tblSuppHeadIndex = new Table();
            TableRow RowHeadIndex = new TableRow();
            dsSuppIndex = objProductServices.GetSupplierNameIndex();
            int i = 1;

            if (dsSuppIndex != null)
            {
                foreach (DataRow rSupp in dsSuppIndex.Tables[0].Rows)
                {
                    TableCell CellHeadIndex = new TableCell();
                    CellHeadIndex.Text = "<a href =#" + rSupp["SUPPLIERNAMEINDEX"].ToString() + " class =\"" + _HeadLinkCssClass + "\" >" + rSupp["SUPPLIERNAMEINDEX"].ToString() + "</a> ";
                    CellHeadIndex.HorizontalAlign = HorizontalAlign.Center;
                    RowHeadIndex.Cells.Add(CellHeadIndex);
                    i = i + 1;
                }
                tblSuppHeadIndex.Rows.Add(RowHeadIndex);
                tblSuppHeadIndex.CssClass = _HeadCssClass;
                Controls.Add(tblSuppHeadIndex);
                dsSuppIndex.Dispose();
            }
        }
        catch (Exception ex)
        {
            objErrorHandler.ErrorMsg = ex;
            objErrorHandler.CreateLog();
        }


    }
    public void LoadSupplierList()
    {
        try
        {
            DataSet dsIndex = new DataSet();
            DataSet dsSupplierLst = new DataSet();

            dsIndex = objProductServices.GetSupplierNameIndex();
            if (dsIndex != null)
            {
                foreach (DataRow rIndex in dsIndex.Tables[0].Rows)
                {
                    Table tblSupLstHead = new Table();
                    tblSupLstHead.CssClass = _HeadCssClass;
                    Table tblSupplst = new Table();

                    //Build the supplier list Head Index.
                    TableRow RowSupLstHead = new TableRow();
                    TableCell CellSuppLstHead = new TableCell();
                    CellSuppLstHead.Text = "<a name = \"" + rIndex["SUPPLIERNAMEINDEX"].ToString() + "\"" + " ID = \"" + rIndex["SUPPLIERNAMEINDEX"].ToString() + "\"" + ">" + rIndex["SUPPLIERNAMEINDEX"].ToString();
                    CellSuppLstHead.HorizontalAlign = HorizontalAlign.Left;
                    RowSupLstHead.Cells.Add(CellSuppLstHead);
                    CellSuppLstHead = new TableCell();
                    CellSuppLstHead.Text = "<a href = # class = \"" + _HeadLinkCssClass + "\">Top</a>";
                    CellSuppLstHead.HorizontalAlign = HorizontalAlign.Right;
                    RowSupLstHead.Cells.Add(CellSuppLstHead);
                    tblSupLstHead.Rows.Add(RowSupLstHead);

                    //Build the supplier list depending on the name index.
                    tblSupplst.CssClass = _HeadCssClass;
                    dsSupplierLst = objProductServices.GetSupplierrList(rIndex["SUPPLIERNAMEINDEX"].ToString());
                    if (dsSupplierLst != null)
                    {

                        int Rcnt = dsSupplierLst.Tables[0].Rows.Count % 2;
                        if (Rcnt > 0)
                        {
                            Rcnt = dsSupplierLst.Tables[0].Rows.Count / 2 + Rcnt;
                        }
                        else
                        {
                            Rcnt = dsSupplierLst.Tables[0].Rows.Count / 2;
                        }
                        int j = Rcnt;
                        for (int i = 0; i < Rcnt; i++)
                        {
                            TableRow RowSuppLst = new TableRow();
                            TableCell CellFirst = new TableCell();
                            TableCell CellSecond = new TableCell();
                            HyperLink oHyLink;
                            CellFirst.CssClass = _CssClass;
                            CellSecond.CssClass = _CssClass;

                            oHyLink = new HyperLink();
                            oHyLink.CssClass = _LinkCssClass;
                            oHyLink.NavigateUrl = "../Family.aspx?SName=" + dsSupplierLst.Tables[0].Rows[i].ItemArray[0].ToString().Replace("&", "?"); ;
                            oHyLink.Text = dsSupplierLst.Tables[0].Rows[i].ItemArray[0].ToString();
                            CellFirst.Controls.Add(oHyLink);
                            //CellFirst.Text = dsSupplierLst.Tables[0].Rows[i].ItemArray[0].ToString();
                            if (i + j < dsSupplierLst.Tables[0].Rows.Count)
                            {
                                oHyLink = new HyperLink();
                                oHyLink.CssClass = _LinkCssClass;
                                oHyLink.NavigateUrl = "../Family.aspx?SName=" + dsSupplierLst.Tables[0].Rows[i + j].ItemArray[0].ToString().Replace("&", "?");
                                oHyLink.Text = dsSupplierLst.Tables[0].Rows[i + j].ItemArray[0].ToString();
                                CellSecond.Controls.Add(oHyLink);
                                // CellSecond.Text = dsSupplierLst.Tables[0].Rows[i + j].ItemArray[0].ToString();
                            }
                            CellFirst.Width = 650;
                            RowSuppLst.Cells.Add(CellFirst);
                            RowSuppLst.Cells.Add(CellSecond);
                            tblSupplst.Rows.Add(RowSuppLst);
                            tblSupplst.BackColor = System.Drawing.Color.White;
                        }
                    }
                    Controls.Add(tblSupLstHead);
                    Controls.Add(tblSupplst);
                }
            }
        }
        catch (Exception ex)
        {
            objErrorHandler.ErrorMsg = ex;
            objErrorHandler.CreateLog();
        }

        }
    #endregion
}
