using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Collections;
using System.Web.Security;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Security.Cryptography;
using System.Security;
using TradingBell.WebCat.CatalogDB;
using TradingBell.WebCat.Helpers;
using TradingBell.WebCat.TemplateRender;
using TradingBell.WebCat.CommonServices;
using System.Web.Services;



public partial class PaymentHistory : System.Web.UI.Page
{
    HelperDB objHelperDB = new HelperDB();
    ErrorHandler objErrorHandler = new ErrorHandler();
    HelperServices objHelperServices = new HelperServices();
    OrderServices objOrderServices = new OrderServices();

    PaymentServices objPaymentServices = new PaymentServices();
    PaymentServices.PayInfo oPayInfo = new PaymentServices.PayInfo();

    OrderServices.OrderInfo oOrderInfo = new OrderServices.OrderInfo();

    UserServices objUserServices = new UserServices();
    UserServices.UserInfo oUserInfo = new UserServices.UserInfo();

    PayOnlineService objPayOnlineService = new PayOnlineService();
    Security objSecurity = new Security();
    const string EnDekey = "WES@SecuryPAY@dm1n@123";

    public int OrderID = 0;
    public int PaymentID = 0;
    DataTable dsOItem = new DataTable();
    DataSet dsOItem1 = new DataSet();
    string strBackLink = "<br/><a href=\"OrderHistory.aspx\" class=\"toplinkatest\" >Back</a>";
    string invNo = null;
    string FromDate = null;
    string ToDate = null;
    int Companyid = 0;
    int Userid = 0;

    protected override void OnPreRender(EventArgs e)
    {
        base.OnPreRender(e);
        string sb;
        sb = "<script language=javascript>\n";
        sb += "window.history.forward(1);\n";
        sb += "\n</script>";
        ClientScript.RegisterClientScriptBlock(Page.GetType(), "clientScript", sb);


        sb = "";
        sb = "<script type=javascript>\n";
        sb += "window.onload = function () { Clear(); }\n";
        sb += "function Clear() { \n";
        sb += " var Backlen=history.length; \n";
        sb += " if (Backlen > 0) history.go(-Backlen); \n";
        sb += "\n}</script>";
        ClientScript.RegisterClientScriptBlock(Page.GetType(), "clientScript", sb);



    }

    protected void Page_Load(object sender, EventArgs e)
    {
        //Response.Buffer = true;
        ////Response.ExpiresAbsolute = DateTime.Now;
        //Response.ExpiresAbsolute = DateTime.Now.AddHours(-1);


        //Response.Expires = 0;
        //Response.AddHeader("Expires", "-1");
        //Response.AddHeader("pragma", "no-cache");
        //Response.AddHeader("cache-control", "private");
        //Response.CacheControl = "no-cache";
        //Response.Cache.SetCacheability(HttpCacheability.NoCache);
        //Response.Cache.SetExpires(DateTime.UtcNow.AddHours(-1));
        //Response.Cache.SetNoStore();



        //Response.Cache.SetCacheability(HttpCacheability.NoCache);
        //Response.Cache.SetExpires(DateTime.UtcNow.AddMinutes(-1));
        //Response.Cache.SetCacheability(HttpCacheability.NoCache);
        //Response.Cache.SetNoStore();
        //Response.Cache.SetCacheability(HttpCacheability.NoCache);
        //Response.Cache.SetNoStore();

        //Response.ClearHeaders();
        //Response.AppendHeader("Cache-Control", "no-cache"); //HTTP 1.1
        //Response.AppendHeader("Cache-Control", "private"); // HTTP 1.1
        //Response.AppendHeader("Cache-Control", "no-store"); // HTTP 1.1
        //Response.AppendHeader("Cache-Control", "must-revalidate"); // HTTP 1.1
        //Response.AppendHeader("Cache-Control", "max-stale=0"); // HTTP 1.1
        //Response.AppendHeader("Cache-Control", "post-check=0"); // HTTP 1.1
        //Response.AppendHeader("Cache-Control", "pre-check=0"); // HTTP 1.1
        //Response.AppendHeader("Pragma", "no-cache"); // HTTP 1.1
        //Response.AppendHeader("Keep-Alive", "timeout=3, max=993"); // HTTP 1.1
       // Response.AppendHeader("Expires", "Mon, 26 Jul 1997 05:00:00 GMT"); // HTTP 1.1

        if (!IsPostBack)
        {
            Companyid = Convert.ToInt32(Session["COMPANY_ID"]);
            Userid = Convert.ToInt32(Session["User_id"]);
            UserList(Userid);

            CreatedUserDropDownlist.SelectedIndex = 0;

            LoadData();

        }
       

    }
    private void UserList(int Userid)
    {
        //All Users Display in Drop down 
        //Connection oConStr = new Connection();
        //SqlConnection oCon = new SqlConnection(oConStr.ConnectionString.Replace("provider=SQLOLEDB;", ""));      
        //SqlDataAdapter Sqlda = new SqlDataAdapter("SELECT	distinct substring(TBCB.CONTACT,0,15)  [User], 'View Order' as [Submitted Order] FROM TBWC_ORDER AS TBO INNER JOIN TBWC_COMPANY_BUYERS AS TBCB ON TBO.USER_ID = TBCB.USER_ID and TBCB.CONTACT != '' ", oCon);        
        //SqlConnection oCon = new SqlConnection(oConStr.ConnectionString.Replace("provider=SQLOLEDB;", ""));
        ConnectionDB objConnectionDB = new ConnectionDB();
        SqlDataAdapter da = new SqlDataAdapter("STP_TBWC_PICK_GetUserList", objConnectionDB.GetConnection());
        da.SelectCommand.Parameters.AddWithValue("@Userid", Userid);
        da.SelectCommand.CommandType = CommandType.StoredProcedure;
        DataTable Sqldt = new DataTable();
        da.Fill(Sqldt);
        objConnectionDB.CloseConnection();


        // if (!Page.IsPostBack)
        // {
        CreatedUserDropDownlist.DataSource = Sqldt;
        CreatedUserDropDownlist.DataTextField = Sqldt.Columns["User"].ColumnName.ToString();
        CreatedUserDropDownlist.DataValueField = Sqldt.Columns["User"].ColumnName.ToString();
        CreatedUserDropDownlist.DataBind();
        if ((Convert.ToInt32(Session["USER_ROLE"]) == 1 && Session["CUSTOMER_TYPE"].ToString() == "Retailer") || (Convert.ToInt32(Session["USER_ROLE"]) == 1 && Session["CUSTOMER_TYPE"].ToString() != "Retailer"))
            CreatedUserDropDownlist.Items.Insert(0, "All Users");

        //}
    }
    protected void SearchButton_Click(object sender, EventArgs e)
    {
        System.Globalization.CultureInfo culture = new System.Globalization.CultureInfo("en-GB", true);

        if (FromdateTextBox.Text.Trim() != "" && TodateTextBox.Text.Trim() != "")
        {
            DateTime _mFromDate = DateTime.Parse(FromdateTextBox.Text, culture, System.Globalization.DateTimeStyles.NoCurrentDateDefault);
            DateTime _mToDate = DateTime.Parse(TodateTextBox.Text, culture, System.Globalization.DateTimeStyles.NoCurrentDateDefault);

            if (DateTime.Compare(_mFromDate, _mToDate) > 0)
            {
                MsgLabel.Text = "To Date Should be Greater than From Date";
                MsgLabel.Visible = true;
                
            }
            else
            {
                MsgLabel.Text = "  .";
                MsgLabel.Visible = false;
            }
        }
        LoadData();
    }
    protected void ResetButton_Click(object sender, EventArgs e)
    {
      
        CreatedUserDropDownlist.SelectedIndex = 0;      
        MsgLabel.Text = "";
        MsgLabel.Visible = false;
        OrderNo.Text = "";
        FromdateTextBox.Text = "";
        TodateTextBox.Text = "";
        LoadData();
    }
    protected string EncryptSP(string ordid, string Txn_id)
    {
        string enc = ordid + "#####" + Txn_id;
        enc = objSecurity.StringEnCrypt(enc, EnDekey);
        enc = objSecurity.StringEnCrypt(enc, EnDekey);
        enc = objSecurity.StringEnCrypt(enc, EnDekey);
        enc = objSecurity.StringEnCrypt(enc, EnDekey);
        enc = objSecurity.StringEnCrypt(enc, EnDekey);
        return HttpUtility.UrlEncode(enc);
    }

    private void LoadData()
    {
        Companyid = Convert.ToInt32(Session["COMPANY_ID"]);
        if (!string.IsNullOrEmpty(OrderNo.Text))
            invNo = null;

        if (!string.IsNullOrEmpty(FromdateTextBox.Text))
            FromDate = FromdateTextBox.Text;
             

        if (!string.IsNullOrEmpty(TodateTextBox.Text))
            ToDate = TodateTextBox.Text;

        if (CboTranType.SelectedItem.Text.Contains ("Failed"   ) )
        {

        dsOItem = objOrderServices.GetFailedHistory(invNo, FromDate, ToDate, CreatedUserDropDownlist.SelectedItem.Text , Companyid);
        }
        else
        {
            dsOItem = objOrderServices.GetPaymentHistory(invNo, FromDate, ToDate, CreatedUserDropDownlist.SelectedItem.Text , Companyid);
        }
        PaymentRepeater.DataSource = dsOItem;
        PaymentRepeater.DataBind();
    }
   
}
