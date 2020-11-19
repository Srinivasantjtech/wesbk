using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using TradingBell.WebCat.CatalogDB;
using TradingBell.WebCat.Helpers;
using TradingBell.WebCat.CommonServices;
public partial class QuoteList : System.Web.UI.Page
{
    # region "Declarations..."

    HelperServices objHelperServices = new HelperServices();
    DataTable dtResQuote = new DataTable();
    DataTable dtQuote = new DataTable();
    ErrorHandler objErrorHandler = new ErrorHandler();
    QuoteServices objQuoteServices = new QuoteServices();
    DataTable dtInit = new DataTable();
    DataTable dtRequested = new DataTable();
    DataTable dtHistory = new DataTable();
    DataTable dtOpenedInit = new DataTable();

    int sViewType;
    int sReqViewType;
    int sViewHistoryType;
    int sOpenedQuote;
    int Pid;
    decimal QuoteValue;
    public enum ViewType
    {
        STATUS = 1,
        QUOTELIST = 2,
        RESPONSEQUOTE = 3,

        CLOSED = 4,
        CANCELED = 5,
        REQUESTEDQUOTELIST = 6
    }
    #endregion

    protected override void OnInit(EventArgs e)
    {

        try
        {
        sViewType = objHelperServices.CI(QuoteServices.QuoteStatus.RESPONSEQUOTE);
        sReqViewType = objHelperServices.CI(QuoteServices.QuoteStatus.REQUESTQUOTE);
        sOpenedQuote = objHelperServices.CI(QuoteServices.QuoteStatus.OPEN);

        dtHistory = objQuoteServices.GetQuotesHistory(objHelperServices.CI(Session["USER_ID"].ToString()));
        dtInit = objQuoteServices.GetQuoteList(objHelperServices.CI(Session["USER_ID"].ToString()), sViewType);
        dtRequested = objQuoteServices.GetQuoteList(objHelperServices.CI(Session["USER_ID"].ToString()), sReqViewType);
        dtOpenedInit = objQuoteServices.GetQuoteList(objHelperServices.CI(Session["USER_ID"].ToString()), sOpenedQuote);

        if (dtInit.Rows.Count < 1 && dtRequested.Rows.Count < 1)
        {
            Response.Redirect("ConfirmMessage.aspx?Result=NOQUOTELIST");
        }
        if (Request["ViewType"].ToString() == "QUOTEHISTORY")
        {
            if (dtHistory.Rows.Count != 0)
                InitHistoryQuote();
        }
        else
        {
            //    sViewType = objHelperServices.CI(QuoteServices.QuoteStatus.RESPONSEQUOTE);
            //  sReqViewType = objHelperServices.CI(QuoteServices.QuoteStatus.REQUESTQUOTE);

            if (dtInit.Rows.Count != 0)
                InitResponsedQuote();
            else
                if (dtOpenedInit.Rows.Count != 0) InitResponsedQuote();

            if (dtRequested.Rows.Count != 0)
                InitReQuestedQuote();
        }
        base.OnInit(e);

        }
        catch (Exception ex)
        {
            objErrorHandler.ErrorMsg = ex;
            objErrorHandler.CreateLog();
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {

        Session["PageUrl"] = Request.Url.PathAndQuery.ToString().Substring(Request.Url.PathAndQuery.IndexOf("/", 1) + 1);
        Page.Title = objHelperServices.GetOptionValues("BROWSER TITLE").ToString();
    }

    #region "ResPonsed Quote List Methods"

    public DataTable BuildResQuoteTable()
    {
        try
        {
            dtResQuote.Columns.Clear();
            DataColumn dtCol = new DataColumn("QuoteID", typeof(int));
            dtResQuote.Columns.Add(dtCol);
            dtCol = new DataColumn("NoItems", typeof(int));
            dtResQuote.Columns.Add(dtCol);
            dtCol = new DataColumn("QuoteDate", typeof(DateTime));
            dtResQuote.Columns.Add(dtCol);

            dtCol = new DataColumn("QuoteValue", typeof(double));
            dtResQuote.Columns.Add(dtCol);
            return dtResQuote;
        }
        catch (Exception ex)
        {
            objErrorHandler.ErrorMsg = ex;
            objErrorHandler.CreateLog();
            return null;
        }

    }

    public void LoadResQuoteTable(int sViewType)
    {
        try
        {
            BuildResQuoteTable();
            if (dtInit != null)
            {
                foreach (DataRow row in dtInit.Rows)
                {
                    DataRow rowOT = dtResQuote.NewRow();
                    rowOT["QuoteID"] = row["QUOTE_ID"];
                    rowOT["NoItems"] = objQuoteServices.GetQuoteItemCount(objHelperServices.CI(row["QUOTE_ID"]));
                    rowOT["QuoteDate"] = row["QUOTEDATE"];
                    rowOT["QuoteValue"] = row["TOTAL_AMOUNT"];
                    dtResQuote.Rows.Add(rowOT);
                }
            }

        }
        catch (Exception ex)
        {
            objErrorHandler.ErrorMsg = ex;
            objErrorHandler.CreateLog();
        }
    }

    public void InitResponsedQuote()
    {
        try
        {
            HtmlTable oHT = new HtmlTable();

            PageDisplay();

            string QuoteStatus = "";

            string QuoteID = "";
            int rowCnt = 0;
            int PageSize = 20;

            LoadResQuoteTable(sViewType);

            oHT.ID = "QuoteTable";
            oHT.Width = "600";
            oHT.CellSpacing = 2;

            int currentPage = 0;
            int TotalPages = 0;
            float totpagedec;
            int currentStart = 0;
            int currentEnd = 0;
            if (dtResQuote.Rows.Count != 0)
                if (dtResQuote != null)
                {

                    if (PageSize > 0)
                    {
                        currentPage = objHelperServices.CI(Page.Request["Pg"]);
                        totpagedec = (float)dtResQuote.Rows.Count / (float)PageSize;
                        TotalPages = dtResQuote.Rows.Count / PageSize;
                        if (((float)totpagedec % 2 != 0))
                            TotalPages = TotalPages;
                        else
                            TotalPages = TotalPages - 1;

                        currentStart = currentPage * PageSize;
                        currentEnd = currentStart + PageSize;
                        if (currentEnd >= dtResQuote.Rows.Count)
                            currentEnd = dtResQuote.Rows.Count;
                    }
                    string LabelText = "";
                    HtmlTableRow oPageRow = new HtmlTableRow();
                    HtmlTableCell oPageCell = new HtmlTableCell();
                    oPageCell.ColSpan = 7;
                    oPageCell.Align = HorizontalAlign.Right.ToString();
                    Label oLbl = new Label();
                    string toURL = Page.Request.FilePath.ToString();
                    if (dtResQuote.Rows.Count > 0)
                    {
                        if (currentPage > 0)
                            LabelText = "<a href=\"" + toURL + "?ViewType=" + sViewType + "&Pg=0\" style=\"text-decoration:none\"><img src=\"Images/start.gif\" align=\"absbottom\" border=\"0\" ></img></a><a href=\"" + toURL + "?ViewType=" + sViewType + "&Pg=" + objHelperServices.CS(currentPage - 1) + "\"><img onmouseover=\"red\" align=\"absbottom\" src=\"Images/previous.gif\" border=\"0\" ></img></a>";
                        LabelText = " " + LabelText + " " + objHelperServices.CS(currentPage + 1) + " of " + objHelperServices.CS(TotalPages + 1) + " ";
                        if (currentPage < TotalPages)
                            LabelText = LabelText + "<a href=\"" + toURL + "?ViewType=" + sViewType + "&Pg=" + objHelperServices.CS(currentPage + 1) + "\" style=\"text-decoration:none\"><img align=\"absbottom\" src=\"Images/next.gif\" border=\"0\"></img></a><a href=\"" + toURL + "?ViewType=" + sViewType + "&Pg=" + TotalPages + "\"><img align=\"absbottom\" src=\"Images/end.gif\" border=\"0\" ></img></a></font>";
                    }
                }

            if (dtResQuote.Rows.Count == 0)
            {
                HtmlTableRow oHead = new HtmlTableRow();
                HtmlTableCell EmptyTitle = new HtmlTableCell();
                EmptyTitle.Height = "22";
                EmptyTitle.Align = HorizontalAlign.Center.ToString();
                EmptyTitle.Attributes["colspan"] = "7";
                EmptyTitle.InnerText = (string)GetLocalResourceObject("ResponsedEmpty");
            }
            HtmlTableRow oHeadRow = new HtmlTableRow();
            HtmlTableCell oIDTitle = new HtmlTableCell();
            HtmlTableCell oNPTitle = new HtmlTableCell();
            HtmlTableCell oODTitle = new HtmlTableCell();
            HtmlTableCell oAmtTitle = new HtmlTableCell();
            HtmlTableRow oCaptionRow = new HtmlTableRow();
            HtmlTableCell oCaptionCell = new HtmlTableCell();

            oCaptionRow.Height = "22";
            oCaptionCell.Align = HorizontalAlign.Center.ToString();
            oCaptionCell.Attributes["colspan"] = "7";
            oCaptionCell.Attributes["class"] = "TableRowHead";

            oCaptionCell.InnerText = (string)GetLocalResourceObject("ResponsedQuote");


            oCaptionRow.Controls.Add(oCaptionCell);
            oHT.Controls.Add(oCaptionRow);
            oHeadRow.Attributes["class"] = "TableRowHead";

            Label lblID = new Label();
            lblID.ID = "lblID";
            lblID.Text = (string)GetLocalResourceObject("lblID.Text");
            oIDTitle.Controls.Add(lblID);

            Label lblItems = new Label();
            lblItems.ID = "lblItems";
            lblItems.Text = (string)GetLocalResourceObject("lblItems.Text");
            oNPTitle.Controls.Add(lblItems);

            Label lblQD = new Label();
            lblQD.ID = "lblQuoteDate";
            lblQD.Text = (string)GetLocalResourceObject("lblQuoteDate.Text");
            oODTitle.Controls.Add(lblQD);

            Label lblAmt = new Label();
            lblAmt.ID = "lblQuoteValue";
            lblAmt.Text = (string)GetLocalResourceObject("lblQuoteValue.Text");
            oAmtTitle.Controls.Add(lblAmt);

            oIDTitle.Align = HorizontalAlign.Center.ToString();
            oNPTitle.Align = HorizontalAlign.Center.ToString();
            oODTitle.Align = HorizontalAlign.Center.ToString();
            oAmtTitle.Align = HorizontalAlign.Center.ToString();

            oHeadRow.Height = "10";
            oHeadRow.Controls.Add(oIDTitle);
            oHeadRow.Controls.Add(oNPTitle);
            oHeadRow.Controls.Add(oODTitle);

            oHeadRow.Controls.Add(oAmtTitle);
            //  oHeadRow.Controls.Add(oShipTitle);
            oHT.Controls.Add(oHeadRow);

            if (dtResQuote != null)
            {
                foreach (DataRow row in dtResQuote.Rows)
                {
                    rowCnt = rowCnt + 1;
                    if (rowCnt - 1 >= currentStart && rowCnt <= currentEnd)
                    {

                        HtmlTableRow oHTR = new HtmlTableRow();
                        HtmlTableCell cellID = new HtmlTableCell();
                        HtmlTableCell cellNP = new HtmlTableCell();
                        HtmlTableCell cellDate = new HtmlTableCell();
                        HtmlTableCell cellAmount = new HtmlTableCell();
                        if (rowCnt % 2 == 0)
                            oHTR.Attributes["class"] = "TableAltRow";
                        else
                            oHTR.Attributes["class"] = "TableFirstRow";

                        oHTR.ID = "rowQuote" + rowCnt;
                        oHTR.Height = "10";
                        cellID.ID = "cellQuoteID" + rowCnt;
                        QuoteStatus = objQuoteServices.GetQuoteStatus(objHelperServices.CI(row["QuoteID"].ToString()));//Get status from Quote_history 
                        QuoteID = row["QuoteID"].ToString();
                        HyperLink oHyp = new HyperLink();
                        oHyp.ID = "oIDLink" + rowCnt;
                        if (QuoteStatus.Equals(QuoteServices.QuoteStatus.OPEN.ToString()))
                        {
                            oHyp.NavigateUrl = "QuoteCart.aspx";
                        }
                        else
                        {
                            int QteFlag = 1;
                            oHyp.NavigateUrl = "QuoteCart.aspx?Quote_Id=" + QuoteID + "&pid=" + Pid + "&QuoteStatus=" + sViewType + "&QteFlag=" + QteFlag;
                        }
                        oHyp.Text = QuoteID;
                        QuoteValue = objHelperServices.CDEC(row["QuoteValue"].ToString());
                        cellID.Align = HorizontalAlign.Center.ToString();
                        cellNP.Align = HorizontalAlign.Center.ToString();
                        cellDate.Align = HorizontalAlign.Center.ToString();

                        cellAmount.Align = HorizontalAlign.Right.ToString();

                        cellID.Controls.Add(oHyp);
                        cellNP.InnerText = row["NoItems"].ToString();
                        cellDate.InnerText = row["QUOTEDATE"].ToString();
                        cellAmount.InnerText = objHelperServices.GetOptionValues("CURRENCYFORMAT").ToString() + " " + QuoteValue.ToString("#,#0.00");
                        oHTR.Controls.Add(cellID);
                        oHTR.Controls.Add(cellNP);
                        oHTR.Controls.Add(cellDate);
                        oHTR.Controls.Add(cellAmount);
                        oHT.Controls.Add(oHTR);
                    }
                    //end rof each
                }

                //New Code for Opened Status
                HtmlTableRow oHTROpen = new HtmlTableRow();
                HtmlTableCell cellIDOpen = new HtmlTableCell();
                HtmlTableCell cellNPOpen = new HtmlTableCell();
                HtmlTableCell cellDateOpen = new HtmlTableCell();
                HtmlTableCell cellAmountOpen = new HtmlTableCell();
                oHTROpen.Attributes["class"] = "TableOpenedQuote";

                oHTROpen.ID = "rowQuote" + rowCnt;
                oHTROpen.Height = "10";
                cellIDOpen.ID = "cellQuoteID" + rowCnt;

                int QID = objQuoteServices.GetQuoteID(objHelperServices.CI(Session["USER_ID"].ToString()), objHelperServices.CI(QuoteServices.QuoteStatus.OPEN));
                if (objQuoteServices.GetQuoteStatus(QID) == QuoteServices.QuoteStatus.OPEN.ToString())
                {
                    QuoteStatus = objQuoteServices.GetQuoteStatus(QID);
                    HyperLink oHypOpen = new HyperLink();
                    oHypOpen.ID = "oIDLink" + rowCnt;
                    if (QuoteStatus.Equals(QuoteServices.QuoteStatus.OPEN.ToString()))
                    {
                        oHypOpen.NavigateUrl = "QuoteCart.aspx?Quote_Id=" + QID;
                    }
                    oHypOpen.Text = QID.ToString();

                    cellIDOpen.Align = HorizontalAlign.Center.ToString();
                    cellNPOpen.Align = HorizontalAlign.Center.ToString();
                    cellDateOpen.Align = HorizontalAlign.Center.ToString();
                    cellAmountOpen.Align = HorizontalAlign.Right.ToString();
                    cellIDOpen.Controls.Add(oHypOpen);
                    cellNPOpen.InnerText = objHelperServices.CS(objQuoteServices.GetQuoteItemCount(QID));
                    cellDateOpen.InnerText = objHelperServices.CS(objQuoteServices.GetQuoteDate(QID));
                    cellAmountOpen.InnerText = objHelperServices.GetOptionValues("CURRENCYFORMAT").ToString() + " " + objHelperServices.CDEC(objQuoteServices.GetQuoteTotalCost(QID).ToString("#,#0.00"));
                    oHTROpen.Controls.Add(cellIDOpen);
                    oHTROpen.Controls.Add(cellNPOpen);
                    oHTROpen.Controls.Add(cellDateOpen);
                    oHTROpen.Controls.Add(cellAmountOpen);
                    oHT.Controls.Add(oHTROpen);
                }

                //End Code for Opened Status**/

                //Add the paging in the bottom.

                if (dtResQuote != null)
                {
                    if (PageSize > 0)
                    {
                        currentPage = objHelperServices.CI(Page.Request["Pg"]);
                        totpagedec = (float)dtResQuote.Rows.Count / (float)PageSize;
                        TotalPages = dtResQuote.Rows.Count / PageSize;
                        if (((float)totpagedec % 2 != 0))
                            TotalPages = TotalPages;
                        else
                            TotalPages = TotalPages - 1;
                        currentStart = currentPage * PageSize;
                        currentEnd = currentStart + PageSize;
                        if (currentEnd > dtResQuote.Rows.Count)
                            currentEnd = dtResQuote.Rows.Count;
                    }

                    string LabelText = "";
                    HtmlTableRow oPageRow = new HtmlTableRow();
                    HtmlTableCell oPageCell = new HtmlTableCell();
                    oPageRow.Attributes["class"] = "TableRowHead";
                    oPageCell.ColSpan = 7;
                    oPageCell.Align = HorizontalAlign.Right.ToString();
                    Label oLbl = new Label();


                    string toURL = Page.Request.FilePath.ToString();
                    if (dtResQuote.Rows.Count > 0)
                    {
                        if (currentPage > 0)
                            LabelText = "<a href=\"" + toURL + "?ViewType=" + sViewType + "&Pg=0\" style=\"text-decoration:none\"><img src=\"Images/start.gif\" align=\"absbottom\" border=\"0\" ></img></a><a href=\"" + toURL + "?ViewType=" + sViewType + "&Pg=" + objHelperServices.CS(currentPage - 1) + "\"><img onmouseover=\"red\" align=\"absbottom\" src=\"Images/previous.gif\" border=\"0\" ></img></a>";
                        LabelText = " " + LabelText + " " + objHelperServices.CS(currentPage + 1) + " of " + objHelperServices.CS(TotalPages + 1) + " ";
                        if (currentPage < TotalPages)
                            LabelText = LabelText + "<a href=\"" + toURL + "?ViewType=" + sViewType + "&Pg=" + objHelperServices.CS(currentPage + 1) + "\" style=\"text-decoration:none\"><img align=\"absbottom\" src=\"Images/next.gif\" border=\"0\"></img></a><a href=\"" + toURL + "?ViewType=" + sViewType + "&Pg=" + TotalPages + "\"><img align=\"absbottom\" src=\"Images/end.gif\" border=\"0\" ></img></a></font>";
                    }
                }
            }
            pnlQuote.Controls.Add(oHT);
        }
        catch (Exception ex)
        {
            objErrorHandler.ErrorMsg = ex;
            objErrorHandler.CreateLog();
        }
    }

    #endregion

    #region "Requested Quote List Methods"

    public DataTable BuildReqQuoteTable()
    {
        try
        {
            dtQuote.Columns.Clear();
            DataColumn dtCol = new DataColumn("QuoteID", typeof(int));
            dtQuote.Columns.Add(dtCol);
            dtCol = new DataColumn("NoItems", typeof(int));
            dtQuote.Columns.Add(dtCol);
            dtCol = new DataColumn("QuoteDate", typeof(DateTime));
            dtQuote.Columns.Add(dtCol);
            dtCol = new DataColumn("QuoteValue", typeof(double));
            dtQuote.Columns.Add(dtCol);
            return dtQuote;
        }
        catch (Exception ex)
        {
            objErrorHandler.ErrorMsg = ex;
            objErrorHandler.CreateLog();
            return null;
        }

    }

    public void LoadReqQuoteTable(int sViewType)
    {
        try
        {
            BuildReqQuoteTable();
            DataTable dtInit = new DataTable();
            dtInit = objQuoteServices.GetQuoteList(objHelperServices.CI(Session["USER_ID"].ToString()), sReqViewType);
            if (dtInit != null)
            {
                foreach (DataRow row in dtInit.Rows)
                {
                    DataRow rowOT = dtQuote.NewRow();
                    rowOT["QuoteID"] = row["QUOTE_ID"];
                    rowOT["NoItems"] = objQuoteServices.GetQuoteItemCount(objHelperServices.CI(row["QUOTE_ID"]));
                    rowOT["QuoteDate"] = row["QUOTEDATE"];
                    rowOT["QuoteValue"] = row["TOTAL_AMOUNT"];
                    dtQuote.Rows.Add(rowOT);
                }
            }
        }
        catch (Exception ex)
        {
            objErrorHandler.ErrorMsg = ex;
            objErrorHandler.CreateLog();
        }
    }

    public void InitReQuestedQuote()
    {
        try
        {
            HtmlTable oHT = new HtmlTable();
            PageDisplay();
            string QuoteStatus = "";

            string QuoteID = "";
            int rowCnt = 0;
            int PageSize = 20;

            LoadReqQuoteTable(sReqViewType);

            oHT.ID = "QuoteTable";
            oHT.Width = "600";
            oHT.CellSpacing = 2;

            int currentPage = 0;
            int TotalPages = 0;
            float totpagedec;
            int currentStart = 0;
            int currentEnd = 0;

            if (dtQuote != null)
            {
                if (PageSize > 0)
                {
                    currentPage = objHelperServices.CI(Page.Request["Pg"]);
                    totpagedec = (float)dtQuote.Rows.Count / (float)PageSize;
                    TotalPages = dtQuote.Rows.Count / PageSize;
                    if (((float)totpagedec % 2 != 0))
                        TotalPages = TotalPages;
                    else
                        TotalPages = TotalPages - 1;

                    currentStart = currentPage * PageSize;
                    currentEnd = currentStart + PageSize;
                    if (currentEnd >= dtQuote.Rows.Count)
                        currentEnd = dtQuote.Rows.Count;
                }
                string LabelText = "";
                HtmlTableRow oPageRow = new HtmlTableRow();
                HtmlTableRow oPageRowtwo = new HtmlTableRow();
                HtmlTableCell oPageCell = new HtmlTableCell();
                // oPageRow.Attributes["class"] = "TableRowHead";
                oPageCell.ColSpan = 7;
                oPageCell.Align = HorizontalAlign.Right.ToString();
                oPageCell.Attributes["class"] = "txt_6";
                oPageCell.Attributes["background"] = "images/17.gif";

                Label oLbl = new Label();
                string toURL = Page.Request.FilePath.ToString();
                if (dtQuote.Rows.Count > 0)
                {
                    if (currentPage > 0)
                        LabelText = "<a href=\"" + toURL + "?ViewType=" + sViewType + "&Pg=0\" style=\"text-decoration:none\"><img src=\"Images/start.gif\" align=\"absbottom\" border=\"0\" ></img></a><a href=\"" + toURL + "?ViewType=" + sViewType + "&Pg=" + objHelperServices.CS(currentPage - 1) + "\"><img onmouseover=\"red\" align=\"absbottom\" src=\"Images/previous.gif\" border=\"0\" ></img></a>";
                    LabelText = " " + LabelText + " " + objHelperServices.CS(currentPage + 1) + " of " + objHelperServices.CS(TotalPages + 1) + " ";
                    if (currentPage < TotalPages)
                        LabelText = LabelText + "<a href=\"" + toURL + "?ViewType=" + sViewType + "&Pg=" + objHelperServices.CS(currentPage + 1) + "\" style=\"text-decoration:none\"><img align=\"absbottom\" src=\"Images/next.gif\" border=\"0\"></img></a><a href=\"" + toURL + "?ViewType=" + sViewType + "&Pg=" + TotalPages + "\"><img align=\"absbottom\" src=\"Images/end.gif\" border=\"0\" ></img></a></font>";
                }
                oHT.Rows.Add(oPageRow);
                oHT.Rows.Add(oPageRowtwo);
            }

            HtmlTableRow oHeadRow = new HtmlTableRow();
            HtmlTableRow oHeadReqRow = new HtmlTableRow();
            HtmlTableCell oIDTitle = new HtmlTableCell();
            HtmlTableCell oNPTitle = new HtmlTableCell();
            HtmlTableCell oODTitle = new HtmlTableCell();

            HtmlTableCell oAmtTitle = new HtmlTableCell();


            HtmlTableRow oCaptionRow = new HtmlTableRow();
            HtmlTableCell oCaptionCell = new HtmlTableCell();

            oCaptionRow.Height = "22";
            oCaptionCell.Align = HorizontalAlign.Center.ToString();
            oCaptionCell.Attributes["colspan"] = "7";
            oCaptionCell.Attributes["class"] = "TableRowHead";
            //oCaptionCell.Attributes["class"] = "txt_6";
            oCaptionCell.Attributes["background"] = "images/17.gif";
            oCaptionCell.InnerText = (string)GetLocalResourceObject("RequestedQuote");
            oCaptionRow.Controls.Add(oCaptionCell);
            oHT.Controls.Add(oCaptionRow);
            oHeadRow.Attributes["class"] = "TableRowHead";

            Label lblID = new Label();
            lblID.ID = "lblID";
            lblID.Text = (string)GetLocalResourceObject("lblID.Text");
            oIDTitle.Controls.Add(lblID);
            oIDTitle.Attributes["background"] = "images/17.gif";

            Label lblItems = new Label();
            lblItems.ID = "lblItems";
            lblItems.Text = (string)GetLocalResourceObject("lblItems.Text");
            oNPTitle.Controls.Add(lblItems);
            oNPTitle.Attributes["background"] = "images/17.gif";

            Label lblQD = new Label();
            lblQD.ID = "lblQuoteDate";
            lblQD.Text = (string)GetLocalResourceObject("lblQuoteDate.Text");
            oODTitle.Controls.Add(lblQD);
            oODTitle.Attributes["background"] = "images/17.gif";

            Label lblAmt = new Label();
            lblAmt.ID = "lblQuoteValue";
            lblAmt.Text = (string)GetLocalResourceObject("lblQuoteValue.Text");
            oAmtTitle.Controls.Add(lblAmt);
            oAmtTitle.Attributes["background"] = "images/17.gif";

            oIDTitle.Align = HorizontalAlign.Center.ToString();
            oNPTitle.Align = HorizontalAlign.Center.ToString();
            oODTitle.Align = HorizontalAlign.Center.ToString();

            oAmtTitle.Align = HorizontalAlign.Center.ToString();

            oHeadRow.Height = "10";

            oHeadRow.Controls.Add(oIDTitle);
            oHeadRow.Controls.Add(oNPTitle);
            oHeadRow.Controls.Add(oODTitle);
            oHeadRow.Controls.Add(oAmtTitle);
            oHT.Controls.Add(oHeadRow);

            if (dtQuote != null)
            {
                foreach (DataRow row in dtQuote.Rows)
                {
                    rowCnt = rowCnt + 1;
                    if (rowCnt - 1 >= currentStart && rowCnt <= currentEnd)
                    {

                        HtmlTableRow oHTR = new HtmlTableRow();
                        HtmlTableCell cellID = new HtmlTableCell();
                        HtmlTableCell cellNP = new HtmlTableCell();
                        HtmlTableCell cellDate = new HtmlTableCell();
                        HtmlTableCell cellAmount = new HtmlTableCell();
                        if (rowCnt % 2 == 0)
                            oHTR.Attributes["class"] = "TableFirstRow";
                        else
                            oHTR.Attributes["class"] = "TableAltRow"; oHTR.ID = "rowQuote" + rowCnt;
                        oHTR.Height = "10";
                        cellID.ID = "cellQuoteID" + rowCnt;
                        QuoteStatus = objQuoteServices.GetQuoteStatus(objHelperServices.CI(row["QuoteID"].ToString()));//Get status from Quote_history 
                        QuoteID = row["QuoteID"].ToString();


                        HyperLink oHyp = new HyperLink();
                        oHyp.ID = "oIDLink" + rowCnt;
                        if (QuoteStatus.Equals(QuoteServices.QuoteStatus.OPEN.ToString()))
                        {
                            oHyp.NavigateUrl = "QuoteCart.aspx";
                        }
                        else
                        {
                            string Scr = @"<script  type='text/javascript'>
                            function PopUp(QuoteID)
                                {
                            var strReturn = window.open('QuoteView.aspx?QteId='+ QuoteID +'&','','width=700,height=300,left=150,top=200,toolbar=1,status=1,scrollbars=1,resizable=yes');                                        
                                }          
                            window.clipboardData.clearData();
                                </script>";
                            Page.RegisterClientScriptBlock("PopUp", Scr);
                            oHyp.NavigateUrl = "javascript:PopUp(" + QuoteID + ")";
                        }

                        oHyp.Text = QuoteID;
                        QuoteValue = objHelperServices.CDEC(row["QuoteValue"].ToString());
                        cellID.Align = HorizontalAlign.Center.ToString();
                        cellNP.Align = HorizontalAlign.Center.ToString();
                        cellDate.Align = HorizontalAlign.Center.ToString();
                        cellAmount.Align = HorizontalAlign.Right.ToString();

                        cellID.Controls.Add(oHyp);
                        cellNP.InnerText = row["NoItems"].ToString();
                        cellDate.InnerText = row["QUOTEDATE"].ToString();
                        cellAmount.InnerText = objHelperServices.GetOptionValues("CURRENCYFORMAT").ToString() + " " + QuoteValue.ToString("#,#0.00");


                        oHTR.Controls.Add(cellID);
                        oHTR.Controls.Add(cellNP);
                        oHTR.Controls.Add(cellDate);
                        oHTR.Controls.Add(cellAmount);
                        oHT.Controls.Add(oHTR);

                    }
                    //end rof each
                }
                //Add the paging in the bottom.

                if (dtQuote != null)
                {
                    if (PageSize > 0)
                    {
                        currentPage = objHelperServices.CI(Page.Request["Pg"]);
                        totpagedec = (float)dtQuote.Rows.Count / (float)PageSize;
                        TotalPages = dtQuote.Rows.Count / PageSize;
                        if (((float)totpagedec % 2 != 0))
                            TotalPages = TotalPages;
                        else
                            TotalPages = TotalPages - 1;
                        currentStart = currentPage * PageSize;
                        currentEnd = currentStart + PageSize;
                        if (currentEnd > dtQuote.Rows.Count)
                            currentEnd = dtQuote.Rows.Count;
                    }

                    string LabelText = "";
                    HtmlTableRow oPageRow = new HtmlTableRow();
                    HtmlTableCell oPageCell = new HtmlTableCell();
                    oPageRow.Attributes["class"] = "TableRowHead";
                    oPageCell.ColSpan = 7;                   
                    oPageCell.Align = HorizontalAlign.Right.ToString();
                    Label oLbl = new Label();


                    string toURL = Page.Request.FilePath.ToString();
                    if (dtQuote.Rows.Count > 0)
                    {
                        if (currentPage > 0)

                            LabelText = "<a href=\"" + toURL + "?ViewType=" + sViewType + "&Pg=0\" style=\"text-decoration:none\"><img src=\"Images/start.gif\" align=\"absbottom\" border=\"0\" ></img></a><a href=\"" + toURL + "?ViewType=" + sViewType + "&Pg=" + objHelperServices.CS(currentPage - 1) + "\"><img onmouseover=\"red\" align=\"absbottom\" src=\"Images/previous.gif\" border=\"0\" ></img></a>";
                        LabelText = " " + LabelText + " " + objHelperServices.CS(currentPage + 1) + " of " + objHelperServices.CS(TotalPages + 1) + " ";

                        if (currentPage < TotalPages)
                            LabelText = LabelText + "<a href=\"" + toURL + "?ViewType=" + sViewType + "&Pg=" + objHelperServices.CS(currentPage + 1) + "\" style=\"text-decoration:none\"><img align=\"absbottom\" src=\"Images/next.gif\" border=\"0\"></img></a><a href=\"" + toURL + "?ViewType=" + sViewType + "&Pg=" + TotalPages + "\"><img align=\"absbottom\" src=\"Images/end.gif\" border=\"0\" ></img></a></font>";

                    }
                }
            }
            pnlQuote.Controls.Add(oHT);
        }
        catch (Exception ex)
        {
            objErrorHandler.ErrorMsg = ex;
            objErrorHandler.CreateLog();

        }

    }

    #endregion

    #region "Quote History List"
    public DataTable BuildQuoteHistoryTable()
    {
        try
        {
            dtHistory.Columns.Clear();
            dtHistory.Rows.Clear();
            DataColumn dtCol = new DataColumn("QuoteID", typeof(int));
            dtHistory.Columns.Add(dtCol);
            dtCol = new DataColumn("NoItems", typeof(int));
            dtHistory.Columns.Add(dtCol);
            dtCol = new DataColumn("QuoteDate", typeof(DateTime));
            dtHistory.Columns.Add(dtCol);
            dtCol = new DataColumn("QuoteValue", typeof(double));
            dtHistory.Columns.Add(dtCol);
            return dtHistory;
        }
        catch (Exception ex)
        {
            objErrorHandler.ErrorMsg = ex;
            objErrorHandler.CreateLog();
            return null;
        }

    }
    public void LoadQuoteHistoryTable()
    {
        try
        {
            BuildQuoteHistoryTable();
            DataTable dtInit = new DataTable();

            dtInit = objQuoteServices.GetQuotesHistory(objHelperServices.CI(Session["USER_ID"].ToString()));
            if (dtInit != null)
            {
                foreach (DataRow row in dtInit.Rows)
                {
                    DataRow rowOT = dtHistory.NewRow();
                    rowOT["QuoteID"] = row["QUOTE_ID"];
                    rowOT["NoItems"] = objQuoteServices.GetQuoteItemCount(objHelperServices.CI(row["QUOTE_ID"]));
                    rowOT["QuoteDate"] = row["QUOTEDATE"];
                    rowOT["QuoteValue"] = row["TOTAL_AMOUNT"];
                    dtHistory.Rows.Add(rowOT); ;
                }
            }
        }
        catch (Exception ex)
        {
            objErrorHandler.ErrorMsg = ex;
            objErrorHandler.CreateLog();
        }
    }
    public void InitHistoryQuote()
    {
        try
        {
            HtmlTable oHT = new HtmlTable();
            PageDisplay();
            string QuoteStatus = "";

            string QuoteID = "";
            int rowCnt = 0;
            int PageSize = 20;

            LoadQuoteHistoryTable();

            oHT.ID = "QuoteHistoryTable";
            oHT.Width = "600";
            oHT.CellSpacing = 2;

            int currentPage = 0;
            int TotalPages = 0;
            float totpagedec;
            int currentStart = 0;
            int currentEnd = 0;

            if (dtHistory != null)
            {
                if (PageSize > 0)
                {
                    currentPage = objHelperServices.CI(Page.Request["Pg"]);
                    totpagedec = (float)dtHistory.Rows.Count / (float)PageSize;
                    TotalPages = dtHistory.Rows.Count / PageSize;
                    if (((float)totpagedec % 2 != 0))
                        TotalPages = TotalPages;
                    else
                        TotalPages = TotalPages - 1;

                    currentStart = currentPage * PageSize;
                    currentEnd = currentStart + PageSize;
                    if (currentEnd >= dtHistory.Rows.Count)
                        currentEnd = dtHistory.Rows.Count;
                }
                string LabelText = "";
                HtmlTableRow oPageRow = new HtmlTableRow();
                HtmlTableRow oPageRowtwo = new HtmlTableRow();
                HtmlTableCell oPageCell = new HtmlTableCell();
                oPageCell.ColSpan = 7;
                oPageCell.Align = HorizontalAlign.Right.ToString();
                oPageCell.Attributes["class"] = "txt_6";
                oPageCell.Attributes["background"] = "images/17.gif";
                Label oLbl = new Label();
                string toURL = Page.Request.FilePath.ToString();
                if (dtHistory.Rows.Count > 0)
                {
                    if (currentPage > 0)
                        LabelText = "<a href=\"" + toURL + "?ViewType=" + sViewType + "&Pg=0\" style=\"text-decoration:none\"><img src=\"Images/start.gif\" align=\"absbottom\" border=\"0\" ></img></a><a href=\"" + toURL + "?ViewType=" + sViewType + "&Pg=" + objHelperServices.CS(currentPage - 1) + "\"><img onmouseover=\"red\" align=\"absbottom\" src=\"Images/previous.gif\" border=\"0\" ></img></a>";
                    LabelText = " " + LabelText + " " + objHelperServices.CS(currentPage + 1) + " of " + objHelperServices.CS(TotalPages + 1) + " ";
                    if (currentPage < TotalPages)
                        LabelText = LabelText + "<a href=\"" + toURL + "?ViewType=" + sViewType + "&Pg=" + objHelperServices.CS(currentPage + 1) + "\" style=\"text-decoration:none\"><img align=\"absbottom\" src=\"Images/next.gif\" border=\"0\"></img></a><a href=\"" + toURL + "?ViewType=" + sViewType + "&Pg=" + TotalPages + "\"><img align=\"absbottom\" src=\"Images/end.gif\" border=\"0\" ></img></a></font>";
                }
                oHT.Rows.Add(oPageRow);
                oHT.Rows.Add(oPageRowtwo);
            }

            HtmlTableRow oHeadRow = new HtmlTableRow();
            HtmlTableRow oHeadReqRow = new HtmlTableRow();
            HtmlTableCell oIDTitle = new HtmlTableCell();
            HtmlTableCell oNPTitle = new HtmlTableCell();
            HtmlTableCell oODTitle = new HtmlTableCell();

            HtmlTableCell oAmtTitle = new HtmlTableCell();


            HtmlTableRow oCaptionRow = new HtmlTableRow();
            HtmlTableCell oCaptionCell = new HtmlTableCell();

            oCaptionRow.Height = "22";
            oCaptionCell.Align = HorizontalAlign.Center.ToString();
            oCaptionCell.Attributes["colspan"] = "7";
            oCaptionCell.Attributes["class"] = "TableRowHead";
            oCaptionCell.InnerText = (string)GetLocalResourceObject("HistoryQuote");
            oCaptionRow.Controls.Add(oCaptionCell);
            oHT.Controls.Add(oCaptionRow);
            oHeadRow.Attributes["class"] = "TableRowHead";

            Label lblID = new Label();
            lblID.ID = "lblID";
            lblID.Text = (string)GetLocalResourceObject("lblID.Text");
            oIDTitle.Controls.Add(lblID);

            Label lblItems = new Label();
            lblItems.ID = "lblItems";
            lblItems.Text = (string)GetLocalResourceObject("lblItems.Text");
            oNPTitle.Controls.Add(lblItems);

            Label lblQD = new Label();
            lblQD.ID = "lblQuoteDate";
            lblQD.Text = (string)GetLocalResourceObject("lblQuoteDate.Text");
            oODTitle.Controls.Add(lblQD);

            Label lblAmt = new Label();
            lblAmt.ID = "lblQuoteValue";
            lblAmt.Text = (string)GetLocalResourceObject("lblQuoteValue.Text");
            oAmtTitle.Controls.Add(lblAmt);

            oIDTitle.Align = HorizontalAlign.Center.ToString();
            oNPTitle.Align = HorizontalAlign.Center.ToString();
            oODTitle.Align = HorizontalAlign.Center.ToString();

            oAmtTitle.Align = HorizontalAlign.Center.ToString();

            oHeadRow.Height = "10";

            oHeadRow.Controls.Add(oIDTitle);
            oHeadRow.Controls.Add(oNPTitle);
            oHeadRow.Controls.Add(oODTitle);
            oHeadRow.Controls.Add(oAmtTitle);
            oHT.Controls.Add(oHeadRow);

            if (dtHistory != null)
            {
                foreach (DataRow row in dtHistory.Rows)
                {
                    rowCnt = rowCnt + 1;
                    if (rowCnt - 1 >= currentStart && rowCnt <= currentEnd)
                    {

                        HtmlTableRow oHTR = new HtmlTableRow();
                        HtmlTableCell cellID = new HtmlTableCell();
                        HtmlTableCell cellNP = new HtmlTableCell();
                        HtmlTableCell cellDate = new HtmlTableCell();
                        HtmlTableCell cellAmount = new HtmlTableCell();
                        if (rowCnt % 2 == 0)
                            oHTR.Attributes["class"] = "TableFirstRow";
                        else
                            oHTR.Attributes["class"] = "TableAltRow"; oHTR.ID = "rowQuote" + rowCnt;
                        oHTR.Height = "10";
                        cellID.ID = "cellQuoteID" + rowCnt;
                        QuoteStatus = objQuoteServices.GetQuoteStatus(objHelperServices.CI(row["QuoteID"].ToString()));//Get status from Quote_history 
                        QuoteID = row["QuoteID"].ToString();


                        HyperLink oHyp = new HyperLink();
                        oHyp.ID = "oIDLink" + rowCnt;
                        if (QuoteStatus.Equals(QuoteServices.QuoteStatus.OPEN.ToString()))
                        {
                            oHyp.NavigateUrl = "QuoteCart.aspx";
                        }
                        else
                        {
                            string Scr = @"<script  type='text/javascript'>
                            function PopUp(QuoteID)
                                {
                            var strReturn = window.open('QuoteView.aspx?QteId='+ QuoteID +'&','','width=550,height=500,left=150,top=200,toolbar=1,status=1,scrollbars=1,resizable=yes');                                        
                                }          
                            window.clipboardData.clearData();
                                </script>";
                            Page.RegisterClientScriptBlock("PopUp", Scr);
                            oHyp.NavigateUrl = "javascript:PopUp(" + QuoteID + ")";
                        }
                        oHyp.Text = QuoteID;

                        QuoteValue = objHelperServices.CDEC(row["QuoteValue"].ToString());
                        cellID.Align = HorizontalAlign.Center.ToString();
                        cellNP.Align = HorizontalAlign.Center.ToString();
                        cellDate.Align = HorizontalAlign.Center.ToString();
                        cellAmount.Align = HorizontalAlign.Right.ToString();

                        cellID.InnerText = row["Quoteid"].ToString();
                        cellNP.InnerText = row["NoItems"].ToString();
                        cellDate.InnerText = row["QUOTEDATE"].ToString();
                        cellAmount.InnerText = objHelperServices.GetOptionValues("CURRENCYFORMAT").ToString() +  " " + QuoteValue.ToString("#,#0.00");


                        oHTR.Controls.Add(cellID);
                        oHTR.Controls.Add(cellNP);
                        oHTR.Controls.Add(cellDate);
                        oHTR.Controls.Add(cellAmount);
                        oHT.Controls.Add(oHTR);

                    }
                    //end rof each
                }
                //Add the paging in the bottom.

                if (dtHistory != null)
                {
                    if (PageSize > 0)
                    {
                        currentPage = objHelperServices.CI(Page.Request["Pg"]);
                        totpagedec = (float)dtHistory.Rows.Count / (float)PageSize;
                        TotalPages = dtHistory.Rows.Count / PageSize;
                        if (((float)totpagedec % 2 != 0))
                            TotalPages = TotalPages;
                        else
                            TotalPages = TotalPages - 1;
                        currentStart = currentPage * PageSize;
                        currentEnd = currentStart + PageSize;
                        if (currentEnd > dtHistory.Rows.Count)
                            currentEnd = dtHistory.Rows.Count;
                    }

                    string LabelText = "";
                    HtmlTableRow oPageRow = new HtmlTableRow();
                    HtmlTableCell oPageCell = new HtmlTableCell();
                    oPageRow.Attributes["class"] = "TableRowHead";
                    oPageCell.ColSpan = 7;
                    oPageCell.Align = HorizontalAlign.Right.ToString();
                    Label oLbl = new Label();


                    string toURL = Page.Request.FilePath.ToString();
                    if (dtHistory.Rows.Count > 0)
                    {
                        if (currentPage > 0)
                            LabelText = "<a href=\"" + toURL + "?ViewType=" + sViewType + "&Pg=0\" style=\"text-decoration:none\"><img src=\"Images/start.gif\" align=\"absbottom\" border=\"0\" ></img></a><a href=\"" + toURL + "?ViewType=" + sViewType + "&Pg=" + objHelperServices.CS(currentPage - 1) + "\"><img onmouseover=\"red\" align=\"absbottom\" src=\"Images/previous.gif\" border=\"0\" ></img></a>";
                        LabelText = " " + LabelText + " " + objHelperServices.CS(currentPage + 1) + " of " + objHelperServices.CS(TotalPages + 1) + " ";
                        if (currentPage < TotalPages)
                            LabelText = LabelText + "<a href=\"" + toURL + "?ViewType=" + sViewType + "&Pg=" + objHelperServices.CS(currentPage + 1) + "\" style=\"text-decoration:none\"><img align=\"absbottom\" src=\"Images/next.gif\" border=\"0\"></img></a><a href=\"" + toURL + "?ViewType=" + sViewType + "&Pg=" + TotalPages + "\"><img align=\"absbottom\" src=\"Images/end.gif\" border=\"0\" ></img></a></font>";
                    }
                }
            }
            pnlQuote.Controls.Add(oHT);
        }
        catch (Exception ex)
        {
            objErrorHandler.ErrorMsg = ex;
            objErrorHandler.CreateLog();
        }

    }
    #endregion

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
            if (dtQuote != null)
            {
                PageSize = 6;
                if (PageSize > 0)
                {
                    currentPage = objHelperServices.CI(Page.Request["Pg"]);
                    TotalPages = dtQuote.Rows.Count / PageSize;
                    currentStart = currentPage * PageSize;
                    currentEnd = currentStart + PageSize;
                    if (currentEnd > dtQuote.Rows.Count)
                        currentEnd = dtQuote.Rows.Count;
                }
            }
        }
        catch (Exception ex)
        {
            objErrorHandler.ErrorMsg = ex;
            objErrorHandler.CreateLog();

        }
    }
}
