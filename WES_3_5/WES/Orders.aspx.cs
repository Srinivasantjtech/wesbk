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
using TradingBell.WebCat.CatalogDB;
using TradingBell.WebCat.Helpers;
using TradingBell.WebCat.CommonServices;
public partial class Orders : System.Web.UI.Page
{
    # region "Declarations..."

    HelperServices objHelperServices = new HelperServices();
    DataTable dtOrder = new DataTable();
    ErrorHandler objErrorHandler = new ErrorHandler();
    OrderServices objOrderServices = new OrderServices();

    public enum ViewType
    {
        STATUS = 1,
        HISTORY = 2
    }

    #endregion

    #region "Events..."
    protected override void OnInit(EventArgs e)
    {
        DataTable dtInit = new DataTable();
        dtInit = objOrderServices.GetOrders(objHelperServices.CI(Session["USER_ID"].ToString()), Request["ViewType"].ToString());
        if (dtInit==null || dtInit.Rows.Count < 1)
        {
            Response.Redirect("ConfirmMessage.aspx?Result=NOORDERS");
        }
        InitOrderTable();
        base.OnInit(e);
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            Session["PageUrl"] = Request.Url.PathAndQuery.ToString().Substring(Request.Url.PathAndQuery.IndexOf("/", 1) + 1);
            Page.Title = objHelperServices.GetOptionValues("BROWSER TITLE").ToString();
            if (System.Convert.ToInt32(Session["USER_ROLE"]) > 2)
            {
                Response.Redirect("MyAccount.aspx");
            }
            if ((Session["USER_ID"] == null) || (Session["USER_ID"].ToString() == ""))
            {
                Session["USER"] = "";
                Session["COUNT"] = "0";
                Response.Redirect("Login.aspx");
            }
        }
        catch (Exception ex)
        {
            objErrorHandler.ErrorMsg = ex;
            objErrorHandler.CreateLog();

        }
    }

    private void lnkShippingStatus_Clicked(Object sender, EventArgs e)
    {
        try
        {
            int rowCnt = 0;


            ImageButton imgButton = sender as System.Web.UI.WebControls.ImageButton;

            if (dtOrder.Rows.Count > 0)
            {
                foreach (DataRow row in dtOrder.Rows)
                {
                    rowCnt = rowCnt + 1;
                    if (imgButton.ID == "imgButton" + rowCnt)
                    {
                        string sTrackingNo = row["TrackingNo"].ToString();
                        string sShippingCompany = row["ShippingCompany"].ToString();
                        string sURL = "";
                        if (OrderServices.ShippingCompany.FEDEX.ToString().Equals(sShippingCompany))
                        {
                            sURL = (string)GetLocalResourceObject("FEDEXTRACKINGURL") + sTrackingNo;
                        }
                        if (OrderServices.ShippingCompany.UPS.ToString().Equals(sShippingCompany))
                        {
                            sURL = (string)GetLocalResourceObject("UPSTRACKINGURL");
                        }

                        if (OrderServices.ShippingCompany.DHL.ToString().Equals(sShippingCompany))
                        {
                            sURL = (string)GetLocalResourceObject("DHLTRACKINGURL");
                        }

                        if (OrderServices.ShippingCompany.USPS.ToString().Equals(sShippingCompany))
                        {
                            sURL = (string)GetLocalResourceObject("USPSTRACKINGURL");
                        }
                        string sScript = "<script defer>\n" +

                         "  var strReturn = window.open('" + sURL + "','desc','scrollbars=yes,width=550,height=500');\n" +

                         " __doPostBack('btnSetup',strReturn);" +

                         "</script>";

                        if (!Page.IsClientScriptBlockRegistered("OpenDialog"))
                        {
                            Page.RegisterStartupScript("OpenDialog", sScript);
                        }
                    }
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

    #region "Functions..."
    public DataTable BuildOrderTable()
    {
        try
        {
            dtOrder.Columns.Clear();

            DataColumn dtCol = new DataColumn("OrderID", typeof(string));
            dtOrder.Columns.Add(dtCol);
            
            dtCol = new DataColumn("refID", typeof(int));
            dtOrder.Columns.Add(dtCol);
            
            dtCol = new DataColumn("NoItems", typeof(int));
            dtOrder.Columns.Add(dtCol);

            dtCol = new DataColumn("OrderDate", typeof(DateTime));
            dtOrder.Columns.Add(dtCol);

            dtCol = new DataColumn("Status", typeof(string));
            dtOrder.Columns.Add(dtCol);

            dtCol = new DataColumn("OrderValue", typeof(double));
            dtOrder.Columns.Add(dtCol);

            dtCol = new DataColumn("ShippingCompany", typeof(string));
            dtOrder.Columns.Add(dtCol);

            dtCol = new DataColumn("TrackingNo", typeof(string));
            dtOrder.Columns.Add(dtCol);

            return dtOrder;
        }
        catch (Exception ex)
        {
            objErrorHandler.ErrorMsg = ex;
            objErrorHandler.CreateLog();
            return null;
        }

    }

    public void LoadOrderTable(string sViewType)
    {
        try
        {
            BuildOrderTable();
            DataTable dtInit = objOrderServices.GetOrders(objHelperServices.CI(Session["USER_ID"].ToString()), sViewType);
            if (dtInit != null)
            {
                foreach (DataRow row in dtInit.Rows)
                {
                    DataRow rowOT = dtOrder.NewRow();
                    rowOT["OrderID"] = row["ORDER_ID"];
                    rowOT["refid"] = row["refid"];
                    rowOT["NoItems"] = objOrderServices.GetOrderItemCount(objHelperServices.CI(row["refid"]));
                    rowOT["OrderDate"] = row["OrderDate"];
                    rowOT["Status"] = row["ORDER_STATUS"];
                    rowOT["OrderValue"] = row["TOTAL_AMOUNT"];
                    rowOT["ShippingCompany"] = row["SHIP_COMPANY"];
                    rowOT["TrackingNo"] = row["TRACKING_NO"];

                    dtOrder.Rows.Add(rowOT);
                }
            }
        }
        catch (Exception ex)
        {
            objErrorHandler.ErrorMsg = ex;
            objErrorHandler.CreateLog();
        }
    }

    public void InitOrderTable()
    {
        try
        {
            HtmlTable oHT = new HtmlTable();

            PageDisplay();

            string OrderStatus = "";
            string sViewType = Request["ViewType"].ToString();
            string OrderID = "";
            int rowCnt = 0;
            int PageSize = 20;

            LoadOrderTable(sViewType);
            oHT.ID = "OrderTable";
            oHT.Width = "570";
            oHT.CellSpacing = 2;
            
            //Paging
            //int PageSize;
            int currentPage = 0;
            int TotalPages = 0;
            float totpagedec;
            int currentStart = 0;
            int currentEnd = 0;
            
            if (dtOrder != null)
            {
                //  oDRs = _DataSet.Tables[0].Select(sFilter, groupcol);
                if (PageSize > 0)
                {
                    currentPage = objHelperServices.CI(Page.Request["Pg"]);
                    totpagedec = (float)dtOrder.Rows.Count / (float)PageSize;                   
                    //TotalPages = dtOrder.Rows.Count / PageSize;
                    if (totpagedec > Convert.ToInt32(Math.Round(totpagedec, MidpointRounding.AwayFromZero)))
                        TotalPages = Convert.ToInt32(Math.Round(totpagedec, MidpointRounding.AwayFromZero)) + 1;
                    else
                        TotalPages = Convert.ToInt32(Math.Round(totpagedec, MidpointRounding.AwayFromZero));
                    /*if (((float)totpagedec % 2 != 0))
                        TotalPages = TotalPages;
                    else
                        TotalPages = TotalPages - 1;*/

                    currentStart = currentPage * PageSize;
                    currentEnd = currentStart + PageSize;
                    if (currentEnd >= dtOrder.Rows.Count)
                        currentEnd = dtOrder.Rows.Count;
                }

                string LabelText = "";
                HtmlTableRow oPageRow = new HtmlTableRow();
                HtmlTableCell oPageCell = new HtmlTableCell();
                oPageRow.Attributes["class"] = "TableRowHead";
                oPageCell.ColSpan = 6;
                oPageCell.Align = HorizontalAlign.Right.ToString();
                oPageCell.Attributes["class"] = "tx_6";
                oPageCell.Attributes["background"] = "images/17.gif";
                Label oLbl = new Label();


                string toURL = Page.Request.FilePath.ToString();
                if (dtOrder.Rows.Count > 0)
                {
                    if (currentPage > 0)

                        // LabelText = "<font face=\"webdings\" size=\"2\"><a href=\"" + toURL + "?ViewType=" + sViewType + "&Pg=0\" style=\"text-decoration:none\">9</a><a href=\"" + toURL + "?ViewType=" + sViewType + "&Pg=" + oHelper.CS(currentPage - 1) + "\" style=\"text-decoration:none\">7</a></font> &nbsp;&nbsp;";
                        LabelText = "<a href=\"" + toURL + "?ViewType=" + sViewType + "&Pg=0\" style=\"text-decoration:none\"><img src=\"Images/start.gif\" align=\"absbottom\" border=\"0\" ></img></a><a href=\"" + toURL + "?ViewType=" + sViewType + "&Pg=" + objHelperServices.CS(currentPage - 1) + "\"><img onmouseover=\"red\" align=\"absbottom\" src=\"Images/previous.gif\" border=\"0\" ></img></a>";


                    //next11
                    //if (currentPage == 0)
                    //    LabelText = " " + LabelText + " " + oHelper.CS(currentPage + 1) + " of " + oHelper.CS(TotalPages + 2) + " ";
                    //else
                    LabelText = " " + LabelText + " " + objHelperServices.CS(currentPage + 1) + " of " + objHelperServices.CS(TotalPages) + " ";


                    if ((currentPage+1) < TotalPages)
                        //   LabelText = LabelText + "<font face=\"webdings\" size=\"2\"><a href=\"" + toURL + "?ViewType=" + sViewType + "&Pg=" + oHelper.CS(currentPage + 1) + "\" style=\"text-decoration:none\">8</a><a href=\"" + toURL + "?ViewType=" + sViewType + "&Pg=" + TotalPages + "\"  style=\"text-decoration:none\">:</a></font>";
                        LabelText = LabelText + "<a href=\"" + toURL + "?ViewType=" + sViewType + "&Pg=" + objHelperServices.CS(currentPage + 1) + "\" style=\"text-decoration:none\"><img align=\"absbottom\" src=\"Images/next.gif\" border=\"0\"></img></a><a href=\"" + toURL + "?ViewType=" + sViewType + "&Pg=" + (TotalPages-1) + "\"><img align=\"absbottom\" src=\"Images/end.gif\" border=\"0\" ></img></a></font>";

                }
                oLbl.Text = LabelText;
                oPageCell.Controls.Add(oLbl);

                oPageRow.Cells.Add(oPageCell);
                //oPageRow.Height = "5px";
                oHT.Rows.Add(oPageRow);
            }

            HtmlTableRow oHeadRow = new HtmlTableRow();
            HtmlTableCell oIDTitle = new HtmlTableCell();
            HtmlTableCell oNPTitle = new HtmlTableCell();
            HtmlTableCell oODTitle = new HtmlTableCell();
            HtmlTableCell oOSTitle = new HtmlTableCell();
            HtmlTableCell oAmtTitle = new HtmlTableCell();
            HtmlTableCell oShipTitle = new HtmlTableCell();


            HtmlTableRow oCaptionRow = new HtmlTableRow();
            HtmlTableCell oCaptionCell = new HtmlTableCell();

            oCaptionRow.Height = "22";
            oCaptionCell.Align = HorizontalAlign.Center.ToString();
            oCaptionCell.Attributes["colspan"] = "7";
            oCaptionCell.Attributes["class"] = "tx_6";
            oCaptionCell.Attributes["background"] = "images/17.gif";
            if (sViewType.Equals(ViewType.STATUS.ToString()))
            {
                oCaptionCell.InnerText = (string)GetLocalResourceObject("StatusTitle");
            }
            if (sViewType.Equals(ViewType.HISTORY.ToString()))
            {
                oCaptionCell.InnerText = (string)GetLocalResourceObject("HistoryTitle");
            }

            oCaptionRow.Controls.Add(oCaptionCell);
            oHT.Controls.Add(oCaptionRow);
            oHeadRow.Attributes["class"] = "TableRowHead";

            Label lblID = new Label();
            lblID.ID = "lblID";
            lblID.Text = (string)GetLocalResourceObject("lblID.Text");
            oIDTitle.Controls.Add(lblID);
            oIDTitle.Attributes["class"] = "txt_6";
            oIDTitle.Attributes["background"] = "images/17.gif";

            Label lblItems = new Label();
            lblItems.ID = "lblItems";
            lblItems.Text = (string)GetLocalResourceObject("lblItems.Text");
            oNPTitle.Controls.Add(lblItems);
            oNPTitle.Attributes["class"] = "txt_6";
            oNPTitle.Attributes["background"] = "images/17.gif";

            Label lblOD = new Label();
            lblOD.ID = "lblOrderDate";
            lblOD.Text = (string)GetLocalResourceObject("lblOrderDate.Text");
            oODTitle.Controls.Add(lblOD);
            oODTitle.Attributes["class"] = "txt_6";
            oODTitle.Attributes["background"] = "images/17.gif";

            Label lblOS = new Label();
            lblOS.ID = "lblOrderStatus";
            lblOS.Text = (string)GetLocalResourceObject("lblOrderStatus.Text");
            oOSTitle.Controls.Add(lblOS);
            oOSTitle.Attributes["class"] = "txt_6";
            oOSTitle.Attributes["background"] = "images/17.gif";

            Label lblAmt = new Label();
            lblAmt.ID = "lblOrderValue";
            lblAmt.Text = (string)GetLocalResourceObject("lblOrderValue.Text");
            oAmtTitle.Controls.Add(lblAmt);
            oAmtTitle.Attributes["class"] = "txt_6";
            oAmtTitle.Attributes["background"] = "images/17.gif";

            Label lblTrack = new Label();
            lblTrack.ID = "lblShippingStatus";
            lblTrack.Text = (string)GetLocalResourceObject("lblShippingStatus.Text");
            oShipTitle.Controls.Add(lblTrack);
            oShipTitle.Attributes["class"] = "txt_6";
            oShipTitle.Attributes["background"] = "images/17.gif";

            oIDTitle.Align = HorizontalAlign.Center.ToString();
            oNPTitle.Align = HorizontalAlign.Center.ToString();
            oODTitle.Align = HorizontalAlign.Center.ToString();
            oOSTitle.Align = HorizontalAlign.Center.ToString();
            oAmtTitle.Align = HorizontalAlign.Center.ToString();
            oShipTitle.Align = HorizontalAlign.Center.ToString();


            oHeadRow.Height = "20";
            oHeadRow.Controls.Add(oIDTitle);
            oHeadRow.Controls.Add(oNPTitle);
            oHeadRow.Controls.Add(oODTitle);
            oHeadRow.Controls.Add(oOSTitle);
            oHeadRow.Controls.Add(oAmtTitle);
            oHeadRow.Controls.Add(oShipTitle);

            oHT.Controls.Add(oHeadRow);


            if (dtOrder != null)
            {

                foreach (DataRow row in dtOrder.Rows)
                {
                    rowCnt = rowCnt + 1;
                    if (rowCnt - 1 >= currentStart && rowCnt <= currentEnd)
                    {

                        HtmlTableRow oHTR = new HtmlTableRow();
                        HtmlTableCell cellID = new HtmlTableCell();
                        HtmlTableCell cellNP = new HtmlTableCell();
                        HtmlTableCell cellDate = new HtmlTableCell();
                        HtmlTableCell cellStatus = new HtmlTableCell();
                        HtmlTableCell cellAmount = new HtmlTableCell();
                        HtmlTableCell cellTrack = new HtmlTableCell();
                        HtmlTableCell cellAction = new HtmlTableCell();

                        if (rowCnt % 2 == 0)
                        {
                            oHTR.Attributes["class"] = "TableFirstRow";
                        }
                        else
                        {
                            oHTR.Attributes["class"] = "TableAltRow";
                        }
                        oHTR.ID = "rowOrder" + rowCnt;
                        oHTR.Height = "20";
                        cellID.ID = "cellOrderID" + rowCnt;

                        OrderStatus = objOrderServices.GetOrderStatus(objHelperServices.CI(row["refid"].ToString()));
                        OrderID = row["refid"].ToString();
                        HyperLink oHyp = new HyperLink();
                        oHyp.ID = "oIDLink" + rowCnt;
                        if (OrderStatus.Equals(OrderServices.OrderStatus.OPEN.ToString()))
                        {
                            oHyp.NavigateUrl = "OrderDetails.aspx";
                        }
                        else
                        {
                            // oHyp.NavigateUrl = "Confirm.aspx?OrdId=" + OrderID;

                            //"javascript:popup("+OrderID+")";
                            string Scr = @"<script  type='text/javascript'>
                            function PopUp(OrdID)
                                {
                            var strReturn = window.open('OrderReport.aspx?OrdId='+ OrdID +'&','','width=600,height=500,left=150,top=200,toolbar=1,status=1,scrollbars=1,resizable=yes');                                        
                                }          

                                </script>";
                            Page.RegisterClientScriptBlock("PopUp", Scr);//                            window.clipboardData.clearData();
                            oHyp.NavigateUrl = "javascript:PopUp(" + OrderID + ")";
                            //   oHyp.NavigateUrl = "<script language=javascript> window.showModalDialog("Confirm.aspx?OrdId=" + OrderID, "", "resizable=yes;dialogWidth:14cm; dialogHeight:10cm")</script>";

                            //    string sScript = "<script defer>\n" +

                            //"  var strReturn = window.open('" + oHyp.NavigateUrl  + "','desc','scrollbars=yes,width=600,height=500');\n" +

                            //" __doPostBack('btnSetup',strReturn);" +

                            //"</script>";
                        }
                        oHyp.Text = row["OrderID"].ToString();

                        cellID.Align = HorizontalAlign.Center.ToString();
                        cellNP.Align = HorizontalAlign.Center.ToString();
                        cellDate.Align = HorizontalAlign.Center.ToString();
                        cellStatus.Align = HorizontalAlign.Center.ToString();
                        cellAmount.Align = HorizontalAlign.Right.ToString();
                        cellTrack.Align = HorizontalAlign.Center.ToString();
                        cellTrack.VAlign = VerticalAlign.Middle.ToString();


                        cellID.Controls.Add(oHyp);
                        cellNP.InnerText = row["NoItems"].ToString();
                        cellDate.InnerText = row["ORDERDATE"].ToString();
                        cellStatus.InnerText = OrderStatus;
                        cellAmount.InnerText = objHelperServices.GetOptionValues("CURRENCYFORMAT").ToString() + " " + objHelperServices.FixDecPlace(Convert.ToDecimal(row["OrderValue"].ToString()));

                        //if ((int) Order.OrderStatus.OPEN  == oHelper.CI(row["STATUS"]))
                        //{
                        //    Button btnAction = new Button();
                        //    btnAction.ID = "btnCancel" + rowCnt;
                        //    btnAction.Text = (string)GetLocalResourceObject("ClearCart");
                        //    btnAction.Class  = "btnNormalSkin";
                        //    btnAction.Click += new EventHandler(btnAction_Clicked);
                        //    cellAction.Controls.Add(btnAction);
                        //}

                        if ((int)OrderServices.OrderStatus.SHIPPED == objHelperServices.CI(row["STATUS"]))
                        {
                            ImageButton imgButton = new ImageButton();
                            imgButton.ID = "imgButton" + rowCnt;
                            imgButton.Height = 20;
                            imgButton.Width = 40;
                            string sShippingCompany = "";
                            string sTrackingNo = "";
                            sShippingCompany = row["ShippingCompany"].ToString();
                            sTrackingNo = row["TrackingNo"].ToString();
                            if (sShippingCompany.Length > 0)
                            {
                                imgButton.ImageUrl = objHelperServices.GetOptionValues("IMAGE PATH").ToString() + @"/" + sShippingCompany + "_logo.gif";
                            }

                            imgButton.Click += new ImageClickEventHandler(lnkShippingStatus_Clicked);
                            cellTrack.Controls.Add(imgButton);

                        }

                        oHTR.Controls.Add(cellID);
                        oHTR.Controls.Add(cellNP);
                        oHTR.Controls.Add(cellDate);
                        oHTR.Controls.Add(cellStatus);
                        oHTR.Controls.Add(cellAmount);
                        oHTR.Controls.Add(cellTrack);
                        //oTblCell.ApplyStyle(_Row);


                        oHT.Controls.Add(oHTR);

                    }
                    //end rof each
                }
                //Add the paging in the bottom.

                if (dtOrder != null)
                {
                    if (PageSize > 0)
                    {
                        currentPage = objHelperServices.CI(Page.Request["Pg"]);
                        totpagedec = (float)dtOrder.Rows.Count / (float)PageSize;
                        //TotalPages = dtOrder.Rows.Count / PageSize;
                        if (totpagedec > Convert.ToInt32(Math.Round(totpagedec, MidpointRounding.AwayFromZero)))
                            TotalPages = Convert.ToInt32(Math.Round(totpagedec, MidpointRounding.AwayFromZero)) + 1;
                        else
                            TotalPages = Convert.ToInt32(Math.Round(totpagedec, MidpointRounding.AwayFromZero));
                        /*if (((float)totpagedec % 2 != 0))
                            TotalPages = TotalPages;
                        else
                            TotalPages = TotalPages - 1;*/
                        currentStart = currentPage * PageSize;
                        currentEnd = currentStart + PageSize;
                        if (currentEnd > dtOrder.Rows.Count)
                            currentEnd = dtOrder.Rows.Count;
                    }

                    string LabelText = "";
                    HtmlTableRow oPageRow = new HtmlTableRow();
                    HtmlTableCell oPageCell = new HtmlTableCell();
                    oPageRow.Attributes["class"] = "TableRowHead";
                    oPageCell.ColSpan = 6;
                    oPageCell.Align = HorizontalAlign.Right.ToString();
                    oPageCell.Attributes["class"] = "txt_6";
                    oPageCell.Attributes["background"] = "images/17.gif";
                    Label oLbl = new Label();


                    string toURL = Page.Request.FilePath.ToString();
                    if (dtOrder.Rows.Count > 0)
                    {
                        if (currentPage > 0)

                            // LabelText = "<font face=\"webdings\" size=\"2\"><a href=\"" + toURL + "?ViewType=" + sViewType + "&Pg=0\" style=\"text-decoration:none\">9</a><a href=\"" + toURL + "?ViewType=" + sViewType + "&Pg=" + oHelper.CS(currentPage - 1) + "\" style=\"text-decoration:none\">7</a></font> &nbsp;&nbsp;";
                            LabelText = "<a href=\"" + toURL + "?ViewType=" + sViewType + "&Pg=0\" style=\"text-decoration:none\"><img src=\"Images/start.gif\" align=\"absbottom\" border=\"0\" ></img></a><a href=\"" + toURL + "?ViewType=" + sViewType + "&Pg=" + objHelperServices.CS(currentPage - 1) + "\"><img onmouseover=\"red\" align=\"absbottom\" src=\"Images/previous.gif\" border=\"0\" ></img></a>";


                        //next11
                        //if (currentPage == 0)
                        //    LabelText = " " + LabelText + " " + oHelper.CS(currentPage + 1) + " of " + oHelper.CS(TotalPages + 2) + " ";
                        //else
                        LabelText = " " + LabelText + " " + objHelperServices.CS(currentPage + 1) + " of " + objHelperServices.CS(TotalPages) + " ";


                        if ((currentPage+1) < TotalPages)
                            //   LabelText = LabelText + "<font face=\"webdings\" size=\"2\"><a href=\"" + toURL + "?ViewType=" + sViewType + "&Pg=" + oHelper.CS(currentPage + 1) + "\" style=\"text-decoration:none\">8</a><a href=\"" + toURL + "?ViewType=" + sViewType + "&Pg=" + TotalPages + "\"  style=\"text-decoration:none\">:</a></font>";
                            LabelText = LabelText + "<a href=\"" + toURL + "?ViewType=" + sViewType + "&Pg=" + objHelperServices.CS(currentPage + 1) + "\" style=\"text-decoration:none\"><img align=\"absbottom\" src=\"Images/next.gif\" border=\"0\"></img></a><a href=\"" + toURL + "?ViewType=" + sViewType + "&Pg=" + (TotalPages - 1) + "\"><img align=\"absbottom\" src=\"Images/end.gif\" border=\"0\" ></img></a></font>";

                    }
                    oLbl.Text = LabelText;
                    oPageCell.Controls.Add(oLbl);
                    //oPageCell.
                    oPageRow.Cells.Add(oPageCell);

                    oHT.Rows.Add(oPageRow);
                }
            }
            pnlOrder.Controls.Add(oHT);

        }
        catch (Exception ex)
        {
            objErrorHandler.ErrorMsg = ex;
            objErrorHandler.CreateLog();

        }

    }
    public void PageDisplay()
    {
        try
        {
            DataSet ds = new DataSet();

            DataRow[] oDRs;
            bool Paging;
            int PageSize;
            int currentPage = 0;
            int TotalPages = 0;
            int currentStart = 0;
            int currentEnd = 0;
            // string sFilter = null;
            //string groupcol = "";
            if (dtOrder != null)
            {
                //  oDRs = _DataSet.Tables[0].Select(sFilter, groupcol);
                PageSize = 5;
                if (PageSize > 0)
                {
                    currentPage = objHelperServices.CI(Page.Request["Pg"]);
                    TotalPages = dtOrder.Rows.Count / PageSize;
                    currentStart = currentPage * PageSize;
                    currentEnd = currentStart + PageSize;
                    if (currentEnd > dtOrder.Rows.Count)
                        currentEnd = dtOrder.Rows.Count;
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
