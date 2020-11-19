using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Data.SqlClient;
using TradingBell.WebCat.CatalogDB;
using TradingBell.WebCat.Helpers;
using TradingBell.WebCat.EasyAsk;
using TradingBell.WebCat.CommonServices;
public partial class PendingOrder : System.Web.UI.Page
{
    public DataTable oDt;
    ConnectionDB objConnectionDB = new ConnectionDB();
    UserServices objUserServices = new UserServices();
    HelperServices objHelperServices = new HelperServices();
    ErrorHandler objErrorHandler = new ErrorHandler();
    protected void Page_Load(object sender, EventArgs e)
    {

        try
        {
        Page.Title = objHelperServices.GetOptionValues("BROWSER TITLE").ToString();

        if (Request["Act"] != null)
        {
            String _Action = Request["Act"].ToString();
            if (_Action == "D")
            {
                objErrorHandler.CreateLog(Request.RawUrl.ToString()); 
                if (Request["OrderId"] != null)
                {
                    String OrderId = Request["OrderId"].ToString();
                    //SqlConnection oCon = new SqlConnection(oConStr.ConnectionString.Replace("provider=SQLOLEDB;", ""));
                    SqlCommand CmdTest = new SqlCommand("STP_TBWC_CANCEL_DeleteOrder", objConnectionDB.GetConnection());
                    CmdTest.CommandType = CommandType.StoredProcedure;
                    CmdTest.Parameters.AddWithValue("@OrderId", OrderId);
                    CmdTest.Parameters.AddWithValue("@Modified_user", Session["USER_ID"]);
                    //oCon.Open();
                    CmdTest.ExecuteNonQuery();
                    objConnectionDB.CloseConnection();
                }
                //SqlCommand CmdTest = new SqlCommand();
                //SqlDataAdapter da = new SqlDataAdapter("DELETEORDER", oCon);
                //da.SelectCommand.CommandType = CommandType.StoredProcedure;
                //da.SelectCommand.Parameters.Clear();
                //da.SelectCommand.Parameters.AddWithValue("@OrderId", OrderId);                                
                //DataSet Ds = new DataSet();
                //DataTable Dt = new DataTable();
                //da.Fill(Dt);
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

