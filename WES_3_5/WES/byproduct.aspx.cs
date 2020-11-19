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
using System.Data.SqlClient;
                                                                     
using TradingBell.WebCat.CatalogDB;
using TradingBell.WebCat.Helpers;
using TradingBell.WebCat.TemplateRender;
using TradingBell.WebCat.CommonServices;
using System.Web.Services;
//using WES.Webservices;
using System.Web.Script.Serialization;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Reflection;
public partial class byproduct : System.Web.UI.Page
{
    HelperServices objHelperServices = new HelperServices();
    ErrorHandler objErrorHandler = new ErrorHandler();
    protected void Page_Load(object sender, EventArgs e)
    {
        //***********************later want to be update with default page************//
        try
        {
            Session["PageUrl"] = Request.Url.PathAndQuery.ToString().Substring(Request.Url.PathAndQuery.IndexOf("/", 1) + 1);
            Page.Title = objHelperServices.GetOptionValues("BROWSER TITLE").ToString();
            Session["CATALOG_ID"] = objHelperServices.GetOptionValues("DEFAULT CATALOG").ToString();
            Session["DO_PAGING"] = objHelperServices.GetOptionValues("SEARCH_DO_PAGING").ToString();
            if (Session["RECORDS_PER_PAGE"] == null || Session["RECORDS_PER_PAGE"] == "")
                Session["RECORDS_PER_PAGE"] = objHelperServices.GetOptionValues("SEARCH_RECS_PER_PAGE").ToString();
            Session["INVENTORY_LEVEL_CHECK"] = objHelperServices.GetOptionValues("INVENTORY_LEVEL_CHECK").ToString();
            Session["SEARCH_CATEGORY_COLS"] = objHelperServices.GetOptionValues("SEARCH_CATEGORY_COLS").ToString();
            //**********************End**************
            if (IsPostBack)
            {
                if (Request.Form["__EVENTTARGET"].ToString() == "compare,")
                {
                    string s = Request.Form["__EVENTARGUMENT"].ToString().Substring(0, Request.Form["__EVENTARGUMENT"].ToString().Length - 1);//.LastIndexOf("Fid".ToCharArray());
                    string[] str = Request.Form["__EVENTARGUMENT"].Split('$');
                    int FamilyID = objHelperServices.CI(str[0]);
                    Session["CloseWin"] = str[1];
                    Session["FAMILY_ID"] = str[0];
                    Response.Redirect("Compare.aspx", false);
                }
            }

        }


        catch (Exception ex)
        {
            objErrorHandler.ErrorMsg = ex;
            objErrorHandler.CreateLog();

        }
    }

  public static DataTable dtGetDataFromDb = new DataTable();

    [WebMethod]
    public static string ProcessAddDeleteOrder(string OrderCode, string Qty)
    {
        ErrorHandler objErrorHandler = new ErrorHandler();
        

        OrderCode = OrderCode.Replace("\n", String.Empty);
        Qty = Qty.Replace("\n", String.Empty);
        string Errmess = "Success";
        string ID = null;
        if (HttpContext.Current.Session["bindid"].ToString() != null)
        {
            ID = HttpContext.Current.Session["bindid"].ToString();
        }
        else
        {
            return "reload";
        }
        try
        {
          


            DataTable dtCode = GetJSONToDataTableUsingNewtonSoftDll( OrderCode.ToUpper());
            DataTable dtQty = GetJSONToDataTableUsingNewtonSoftDll(Qty);
            if (dtCode.Rows.Count == 0 && dtQty.Rows.Count == 0)
            {
                return  "Empty";;
               
            }

            if (dtCode != null)
            {
                int i = 0;
                string errCode = "";

                foreach (DataRow row in dtCode.Rows)
                {
                    errCode = dtCode.Rows[i]["Code"].ToString().Replace("\n", String.Empty);
                   
                    if (errCode == "" || errCode == null)
                    {
                        return  "Err1";
                       
                    }
                }
            }
           if (dtQty != null)
            {
                int i = 0;
                string errCode = "";
                foreach (DataRow row in dtQty.Rows)
                {
                    errCode = dtQty.Rows[i]["Qty"].ToString().Replace("\n", string.Empty); ;
                    if (errCode == "" || errCode == null)
                    {
                        return "Err2";
                        
                    }
                }
            }
            if(dtCode != null && dtQty != null)
            {

                DataTable dtord_qty = new DataTable();
                dtord_qty.Columns.Add("Code");
                dtord_qty.Columns.Add("Qty");
                int i = 0;
                foreach (DataRow row in dtCode.Rows)
                {
                    dtord_qty.Rows.Add(dtCode.Rows[i]["Code"], dtQty.Rows[i]["Qty"]);
                    i++;
                }


                ConnectionDB objconn = new ConnectionDB();
                SqlConnection conn = new SqlConnection(objconn.ConnectionString);

                conn.Open();

                DateTime dtdatetime = DateTime.Now;
                DataSet ds1 = new DataSet();
                string sqlCmd1 = "SELECT User_ID from TBWC_MYPRODUCT where ID = '" + ID + "' ";
                SqlCommand cmd1 = new SqlCommand(sqlCmd1, conn);
                SqlDataAdapter da1 = new SqlDataAdapter(cmd1);
                da1.Fill(ds1);
                string user_id = ds1.Tables[0].Rows[0][0].ToString();


                dtord_qty.Columns.Add("Id", typeof(Int32));
                dtord_qty.Columns.Add("Product_Id", typeof(Int32));
                dtord_qty.Columns.Add("Stock_Status", typeof(String));
                dtord_qty.Columns.Add("Comments", typeof(String));
                dtord_qty.Columns.Add("Price", typeof(decimal));
                dtord_qty.Columns.Add("CreatedOn", typeof(DateTime));
                dtord_qty.Columns.Add("ModifiedOn", typeof(DateTime));
                //dtCode = ds1.Tables[0];
                DataTable dtcheckOrderCode = new DataTable();
                int itmchk = 0;
                string ordCodeNotFnd = "";
                foreach (DataRow row in dtord_qty.Rows)
                {
                    string Checkcode = row["Code"].ToString().Replace("\n", String.Empty);
                    string str = "select STRING_VALUE from TB_PROD_SPECS_Code where string_value = '" + Checkcode + "'";
                    SqlDataAdapter dadp = new SqlDataAdapter(str, conn);
                    dadp.Fill(dtcheckOrderCode);

                    if (dtcheckOrderCode != null)
                    {
                        if (dtcheckOrderCode.Rows.Count > 0)
                        {
                            string chkcode  = dtcheckOrderCode.Rows[itmchk]["STRING_VALUE"].ToString().ToUpper();
                            if (chkcode != Checkcode.ToUpper())
                            {
                                ordCodeNotFnd += Checkcode;
                            }
                        }
                        else {
                            ordCodeNotFnd += Checkcode;
                        
                        }
                    }
                    else {
                        ordCodeNotFnd += Checkcode;
                    }
                    itmchk++;
                }
                if (ordCodeNotFnd != "")
                {
                    return ordCodeNotFnd;
                }

                foreach (DataRow row in dtord_qty.Rows)
                {
                    string code = row["Code"].ToString().Replace("\n",String.Empty);
                    objErrorHandler.CreateLog(code);
                    string str = "select b.PRODUCT_ID,c.PROD_STOCK_STATUS,DBO.GetETA(c.PROD_STOCK_STATUS,c.PROD_NEXT_SHIP) AS eta,c.PROD_STOCK_FLAG,c.PROD_NEXT_SHIP,c.Product_id,c.SOH,Supplier_ID,c.SUPPLIER_SHIP_DAYS,c.PROD_SUBSTITUTE,c.PROD_LEGISTATED_STATE,c.PROD_STK_STATUS_DSC,dbo.fnGetWESProductPriceNew(b.PRODUCT_ID,1," + user_id + ",'') as Cost  from  TB_PROD_SPECS_Code b    join WESTB_PRODUCT_ITEM c on b.PRODUCT_ID = c.PRODUCT_ID where string_value='" + code + "'";
                           
                    objErrorHandler.CreateLog(str);
                    SqlDataAdapter da2 = new SqlDataAdapter(str, conn);
                    DataTable dt1 = new DataTable();
                    da2.Fill(dt1);

                    if (dtord_qty != null)
                    {

                        string StockStatus = GetStockStatus(dt1.Rows[0]);
                       // getqty(code.ToString().ToUpper(),Convert.ToString(row["QTY"]));


                        row["Stock_Status"] = StockStatus.Replace("_", " ");
                        row["Id"] = ID;
                        row["Product_Id"] = dt1.Rows[0]["PRODUCT_ID"].ToString();
                   
                        row["Comments"] = "";
                        row["Price"] = dt1.Rows[0]["Cost"].ToString();
                        row["CreatedOn"] = dtdatetime;
                        row["ModifiedOn"] = dtdatetime;

                    }
                }

               
               
                string sqlCmd = "SELECT Id,Product_Id,Code,Stock_Status,Comments,Price,Qty from TBWC_MYPRODUCT_DETAILS where ID='" + ID + "'";
                SqlCommand cmd = new SqlCommand(sqlCmd, conn);        
               
                cmd = new SqlCommand(sqlCmd, conn);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataSet dsnew = new DataSet();
                da.Fill(dsnew);

                DataTable dtnew = (DataTable)dsnew.Tables[0];

                foreach (DataRow drn in dtord_qty.Rows)
                {
                    string strCode = drn["Code"].ToString().Replace("\n", String.Empty).ToUpper();

                    foreach (DataRow dr2 in dtnew.Rows)
                    {
                        string strdr2 = dr2["Code"].ToString().Replace("\n", String.Empty);
                        if (strCode.ToUpper() == strdr2.ToUpper())
                        {
                            dr2.Delete();
                            SqlCommand sqlcm = new SqlCommand("Delete from TBWC_MYPRODUCT_DETAILS  where ID=" + ID + " and ltrim(Code) = '" + strCode.ToUpper() + "'", conn);
                            sqlcm.ExecuteNonQuery();
                        }

                    }
                    dtnew.AcceptChanges();
                }




                bulkcopyProd(dtord_qty);
                HttpContext.Current.Session["dtProdList"] = null;
            }

        }
        catch (Exception ex)
        {
            objErrorHandler.CreateLog(ex.ToString());
            return "Err3";

            
        }
       
        //UC_byproduct ord = new UC_byproduct();
        //ord.GetDataFromDb(ID.ToString());
        return "Success";
    }

    public static DataTable GetJSONToDataTableUsingNewtonSoftDll(string JSONData)
    {
        DataTable dt = (DataTable)JsonConvert.DeserializeObject(JSONData, (typeof(DataTable)));
        return dt;
    }

    public static void getqty(string product_code,string QTY)
    {
        UC_byproduct ord = new UC_byproduct();

        ord.AddToCart(QTY.Replace("\n", String.Empty), product_code);
     
       
    }

    private static void bulkcopyProd(DataTable dt)
    {
        ErrorHandler objErrorHandler = new ErrorHandler();
        try
        {
            ConnectionDB objconn = new ConnectionDB();
            SqlConnection conn = new SqlConnection(objconn.ConnectionString);
            conn.Open();

            using (SqlBulkCopy sqlBulkCopy = new SqlBulkCopy(objconn.ConnectionString))
            {
                //Set the database table name
                sqlBulkCopy.DestinationTableName = "dbo.TBWC_MYPRODUCT_DETAILS";




                //[OPTIONAL]: Map the DataTable columns with that of the database table
                sqlBulkCopy.ColumnMappings.Add("Id", "Id");


                sqlBulkCopy.ColumnMappings.Add("Product_Id", "Product_Id");
                sqlBulkCopy.ColumnMappings.Add("Code", "Code");
                sqlBulkCopy.ColumnMappings.Add("Stock_Status", "Stock_Status");
                sqlBulkCopy.ColumnMappings.Add("Comments", "Comments");
                sqlBulkCopy.ColumnMappings.Add("Price", "Price");
                sqlBulkCopy.ColumnMappings.Add("QTY", "Qty");
                sqlBulkCopy.ColumnMappings.Add("CreatedOn", "CreatedOn");
                sqlBulkCopy.ColumnMappings.Add("ModifiedOn", "ModifiedOn");


                sqlBulkCopy.WriteToServer(dt);
                conn.Close();

            }
        }
        catch (Exception ex)
        {
            objErrorHandler.CreateLog(ex.ToString());
        }
    }
    public static string GetStockStatus(DataRow row)
    {

        string PROD_STOCK_STATUS = row["PROD_STOCK_STATUS"].ToString();
        // dr["Stock"] = dtitems.Rows[i]["prod_stk_status_dsc"].ToString();
        int stockflag = Convert.ToInt16(row["PROD_STOCK_FLAG"].ToString());
        string ETA = row["ETA"].ToString();

        bool validETA = false;

        if (ETA != string.Empty && ETA != "PLEASE CALL")
        {

            return "Pre-Order. ETA :  " + ETA + "";

        }
        string strpid = row["Product_id"].ToString();
        int SOH = 0;
        if (row["SOH"] != null && row["SOH"].ToString() != "")
        {
            SOH = Convert.ToInt16(row["SOH"].ToString());
        }

        int Supplier_ID = 0;
        if (row["Supplier_ID"] != null && row["Supplier_ID"].ToString() != "")
        {
            Supplier_ID = Convert.ToInt16(row["Supplier_ID"].ToString());
        }
        int SUPPLIER_SHIP_DAYS = 0;
        if (row["SUPPLIER_SHIP_DAYS"] != null && row["SUPPLIER_SHIP_DAYS"].ToString() != "")
        {
            SUPPLIER_SHIP_DAYS = Convert.ToInt16(row["SUPPLIER_SHIP_DAYS"].ToString());
        }
        //int Supplier_ID =Convert.ToInt16( dtitems.Rows[i]["Supplier_ID"].ToString());
        string PROD_SUBSTITUTE = row["PROD_SUBSTITUTE"].ToString().Trim();
        string PROD_LEGISTATED_STATE = row["PROD_LEGISTATED_STATE"].ToString();

        bool insert = true;


        if (stockflag == -2 && validETA == true)
        {
            return "Pre-Order";

        }
        else if (stockflag <= -2 && (PROD_LEGISTATED_STATE == "DDDDDDD" || Supplier_ID == 999) && PROD_LEGISTATED_STATE != "UUUUUUU")
        {

            return "Discontinued No Longer Available";
        }
        else if ((row["PROD_STOCK_FLAG"].ToString() == "-2" && row["PROD_STOCK_STATUS"].ToString() == "False" && row["PROD_STK_STATUS_DSC"].ToString().Trim() == "OUT_OF_STOCK ITEM WILL BE BACK ORDERED") || (row["PROD_STOCK_FLAG"].ToString() == "0" && row["PROD_STOCK_STATUS"].ToString() == "True" && row["PROD_STK_STATUS_DSC"].ToString().Trim() == "Please_Call") || (row["PROD_STOCK_FLAG"].ToString() == "-2" && row["PROD_STOCK_STATUS"].ToString() == "False" && row["PROD_STK_STATUS_DSC"].ToString().Trim() == "SPECIAL_ORDER PRICE & AVAILABILITY TO BE CONFIRMED"))
        {
            string isSameLogic = GetStockDetails(row["PRODUCT_ID"].ToString(), ETA);
            if (isSameLogic == "true")
            {
                return row["PROD_STK_STATUS_DSC"].ToString().Trim();
            }
            else
            {
                return isSameLogic;
            }
        }
        else
        {



            return row["PROD_STK_STATUS_DSC"].ToString().Trim();
            //dr["soh"] = SOH;
            //dr["prod_stock_flag"] =stockflag;

            //dr["prod_legistated_state"] = PROD_LEGISTATED_STATE.Trim();

        }
    }
    public static string GetStockDetails(string Pid,string ETA)
    {
        HelperDB objhelperDb = new HelperDB();
        try
        {
            string user_id = string.Empty;
            string order_id = string.Empty;
            int no = 0;
            int availabilty = 0;
            string availabilty1 = string.Empty;
            string sqlexec = "exec SP_CHECK_STOCK_STATUS '" + Pid.ToString() + "' ";

            DataSet Dsall = objhelperDb.GetDataSetDB(sqlexec);
            bool validETA = false;

            if (ETA != string.Empty && ETA != "PLEASE CALL")
            {

                return "Pre-Order. ETA :  " + ETA + "";

            }
            if (Dsall.Tables[0].Rows[0]["AVAILABILTY_TOTAL"] != null && Dsall.Tables[0].Rows[0]["AVAILABILTY_TOTAL"].ToString() != "" && Dsall.Tables[0].Rows[0]["AVAILABILTY_TOTAL"].ToString().ToUpper().Trim() != "CALL")
            {
                availabilty1 = Dsall.Tables[0].Rows[0]["AVAILABILTY_TOTAL"].ToString();
                availabilty1 = availabilty1.Replace("+", "");
                availabilty = Convert.ToInt32(availabilty1.ToString());
            }
            if ((Dsall.Tables[0].Rows[0]["SUPPLIER_ITEM_CODE"] == null || Dsall.Tables[0].Rows[0]["SUPPLIER_ITEM_CODE"].ToString().Trim() == "") || (Dsall.Tables[0].Rows[0]["SUPPLIER_ID"] == null || Dsall.Tables[0].Rows[0]["SUPPLIER_ID"].ToString().Trim() == ""))
            {
                return "true";
            }
            if (Dsall.Tables[0].Rows[0]["product_id"] != null && Dsall.Tables[0].Rows[0]["product_id"].ToString() == string.Empty && Dsall.Tables[0].Rows[0]["PRODUCT_CODE"] != null && Dsall.Tables[0].Rows[0]["PRODUCT_CODE"].ToString() == string.Empty)
            {
                if (Dsall.Tables[0].Rows[0]["SUPPLIER_SHIP_DAYS"] != null && Dsall.Tables[0].Rows[0]["SUPPLIER_SHIP_DAYS"].ToString() != "")
                {
                    int shipping_time = Convert.ToInt32(Dsall.Tables[0].Rows[0]["SUPPLIER_SHIP_DAYS"].ToString());

                    string supplier_shipping_time = string.Empty;

                    if (shipping_time > 1)
                    {
                        supplier_shipping_time = "1 - " + shipping_time + " Days";
                    }
                    else if (shipping_time == 1)
                    {
                        supplier_shipping_time = " 1 Day";
                    }

                    if (shipping_time > 0 && shipping_time <= 14)
                    {
                        return "Please Call" + "supplier_shipping_time";
                        //st.SetAttribute("TBT_STOCK_STATUS_2", true);
                        //st.SetAttribute("TBT_ISINSTOCK", true);
                        //st.SetAttribute("TBT_ISINSTOCK_STAUS", "Please Call");
                        //st.SetAttribute("TBT_SHIPPINGDAYS", supplier_shipping_time);
                        //st.SetAttribute("TBT_SHOW_SHIPPINGDAYS", true);
                        //objErrorHandler.CreateLog("supplier_shipping_time " + supplier_shipping_time);
                        //return false;
                    }
                    else
                    {
                        return "true";
                    }
                }
                return "true";
            }
            else if (availabilty > 0)
            {

                int avail_total = Convert.ToInt32(availabilty);
                int stock_cutoff = Convert.ToInt32(Dsall.Tables[0].Rows[0]["WEB_STOCK_CUTOFF"]);
                int shipping_time = Convert.ToInt32(Dsall.Tables[0].Rows[0]["SUPPLIER_SHIPPING_TIME"].ToString());

                string supplier_shipping_time = string.Empty;
                if (shipping_time > 1)
                {
                    supplier_shipping_time = "1 - " + shipping_time + " Days";
                }
                else if (shipping_time == 1)
                {
                    supplier_shipping_time = " 1 Day";
                }
                if (avail_total >= stock_cutoff)
                {

                    return "In Stock" + supplier_shipping_time;
                    //st.SetAttribute("TBT_SHOW_SHIPPINGDAYS", true);
                    //st.SetAttribute("TBT_ISINSTOCK", true);
                    //st.SetAttribute("TBT_ISINSTOCK_STAUS", "In Stock");
                    //st.SetAttribute("TBT_SHIPPINGDAYS", supplier_shipping_time);
                }
                else if (avail_total < stock_cutoff)
                {
                    return "Please Call" + supplier_shipping_time;
                    //st.SetAttribute("TBT_SHOW_SHIPPINGDAYS", true);
                    //st.SetAttribute("TBT_PLEASE_CALL", true);
                    //st.SetAttribute("TBT_SHIPPINGDAYS", supplier_shipping_time);
                }

            }
        }
        catch (Exception ex)
        {

        }
        return "true";
    }

}
