using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using TradingBell.Common;
using TradingBell.WebServices;
using System.ComponentModel;

/// <summary>
///This web control is used to list out all the 
/// Families based on the Supplier Name.
/// </summary>


[assembly: TagPrefix("TradingBell.WebServices.UI", "WebCat")]
[assembly: System.Reflection.AssemblyVersion("5.0")]
namespace TradingBell.WebServices.UI
{
	public class SupplierList : WebControl
	{ 
		
        Helper oHelper;
        ErrorHandler oErr;
        Product oProd;
        
        public enum SupplierListVisibleText
        {
            Yes = 1,
            No = 2

        }
        
        string _HeadCssClass;
        string _HeadLinkCssClass;
        string _CssClass;
        string _LinkCssClass;
        string _UserImage;
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
        public override string CssClass
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

        [
        Browsable(true),
        Category("TradingBell"),
        DefaultValue("")
        ]
        public string UserImage   
    {
            get
            {
                return _UserImage;
            }
            set
            {
                _UserImage = value;
            }
        }
        #endregion


        protected override void RenderContents(HtmlTextWriter output)
        {
            if (!DesignMode)
            {
                string ImgPath = _UserImage;
                oHelper = new Helper();
                oErr = new ErrorHandler();
                oProd = new Product();
                Image imgBlank = new Image();
                Label lblTop = new Label();
                lblTop.Text = "<a name = \"top\" id =\"top\">Top</a>";
                if (_CurrentSupplierVisible == SupplierListVisibleText.Yes)
                {
                    imgBlank.ImageUrl = ImgPath + "/spacer.gif";
                    imgBlank.Height = 20;
                    LoadSupplierNameIndex(output);
                    imgBlank.RenderControl(output);
                    LoadSupplierList(output);
                }

            }
            else
            {
                Image oImg = new Image();
                oImg.ImageUrl = "~/Images/SupplierList.gif";
                oImg.ID = "imgDefault";
                oImg.Width = 125;
                this.Controls.Add(oImg);
                oImg.RenderControl(output);
            }
        }
        #region "Functions"
        public void LoadSupplierNameIndex(HtmlTextWriter oWriter)
        {
            DataSet dsSuppIndex = new DataSet();
            Table tblSuppHeadIndex = new Table();
            TableRow RowHeadIndex = new TableRow();
            dsSuppIndex = oProd.GetSupplierNameIndex();
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
                tblSuppHeadIndex.RenderControl(oWriter);
                dsSuppIndex.Dispose();
            }
        }
        public void LoadSupplierList(HtmlTextWriter oWriter)
        {
            DataSet dsIndex = new DataSet();
            DataSet dsSupplierLst = new DataSet();

            dsIndex = oProd.GetSupplierNameIndex();
            if (dsIndex != null)
            {
                foreach (DataRow rIndex in dsIndex.Tables[0].Rows)
                {
                    Table tblSupLstHead = new Table();
                    tblSupLstHead.CssClass = _HeadCssClass;
                    Table tblSupplst = new Table();
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
                    tblSupplst.CssClass = _HeadCssClass;
                    dsSupplierLst = oProd.GetSupplierrList(rIndex["SUPPLIERNAMEINDEX"].ToString());
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
                            oHyLink.NavigateUrl = "~/Family.aspx?SName=" + dsSupplierLst.Tables[0].Rows[i].ItemArray[0].ToString().Replace("&", "?"); ;
                            oHyLink.Text = dsSupplierLst.Tables[0].Rows[i].ItemArray[0].ToString();
                            CellFirst.Controls.Add(oHyLink);
                            if (i + j < dsSupplierLst.Tables[0].Rows.Count)
                            {
                                oHyLink = new HyperLink();
                                oHyLink.CssClass = _LinkCssClass;
                                oHyLink.NavigateUrl = "~/Family.aspx?SName=" + dsSupplierLst.Tables[0].Rows[i + j].ItemArray[0].ToString().Replace("&", "?");
                                oHyLink.Text = dsSupplierLst.Tables[0].Rows[i + j].ItemArray[0].ToString();
                                CellSecond.Controls.Add(oHyLink);
                                   }
                            CellFirst.Width = 650;
                            RowSuppLst.Cells.Add(CellFirst);
                            RowSuppLst.Cells.Add(CellSecond);
                            tblSupplst.Rows.Add(RowSuppLst);
                            tblSupplst.BackColor = System.Drawing.Color.White;
                        }
                    }
                   
                    tblSupLstHead.RenderControl(oWriter);
                    tblSupplst.RenderControl(oWriter);

                }
            }
        }
        #endregion

	}
}
