using System;
using System.Data;
using System.Collections;
using System.Data.SqlClient;
using System.Configuration;
using System.Web;
using System.Web.Security;

using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Security.Cryptography;
using System.Security;
using System.Text;
using TradingBell.WebCat.Helpers;

namespace TradingBell.WebCat.CatalogDB
{

    #region OLD CODE TRADING BELL
    //ConnectionDB objConnection=new ConnectionDB();

    //    //SqlConnection GCon = new SqlConnection();
    //    //string StrConnectionString = ConfigurationManager.ConnectionStrings["DBConnectionString"].ToString();
    //    /// <summary>
    //    /// Hash Table
    //    /// </summary>

    //    string WebSiteID = ConfigurationManager.AppSettings["WEBSITEID"].ToString();

    //    public static Hashtable WebCatGlb = new Hashtable();
    //    private string sSqlStr;
    //    ErrorHandler objErrorHandler = new ErrorHandler();
    //    SqlCommand objSqlCommand;
    //    DataSet objDataSet = new DataSet();
    //    SqlDataReader objDataR = null;
    //    DataTable objDatatbl = new DataTable();
    //    SqlDataAdapter objDataAdapter;
    //    string ReturnValue = string.Empty;
    //    public enum ReturnType
    //    {
    //        RTString = 1,
    //        RTTable = 2,
    //        RTDataSet = 3            
    //    }


    //    public HelperDB()
    //    {
    //        //GetInitialDetails();
    //    }
    //    #region "Properties"
    //    /// <summary>
    //    /// Set the Property for SQLString
    //    /// </summary>
    //    public string SQLString
    //    {
    //        get
    //        {
    //            return sSqlStr;
    //        }
    //        set
    //        {
    //            sSqlStr = value;
    //        }
    //    }
    //    #endregion
    //    #region "Functions"
     
    //    //public DataSet GetDataSetDB(string sTableName)
    //    //{
    //    //    DataSet oDs = new DataSet();
    //    //    try
    //    //    {
    //    //        SqlDataAdapter oDA = new SqlDataAdapter(SQLString, objConnection.GetConnection());
    //    //        oDA.Fill(oDs, sTableName);
    //    //        oDA.Dispose();
    //    //        if (oDs.Tables[sTableName].Rows.Count == 0)
    //    //        {
    //    //            oDs = null;
    //    //        }
    //    //    }
    //    //    catch (Exception ex)
    //    //    {
    //    //        objErrorHandler.ErrorMsg = ex;
    //    //        objErrorHandler.CreateLog();
    //    //        oDs = null;
    //    //    }          
    //    //    return oDs;

    //    //}

       
    //    //public DataSet GetDataSetDB()
    //    //{
    //    //    DataSet oDs = new DataSet();
    //    //    try
    //    //    {
    //    //        SqlDataAdapter oDA = new SqlDataAdapter(SQLString, objConnection.GetConnection());
    //    //        oDA.Fill(oDs);
    //    //        oDA.Dispose();
    //    //        if (oDs.Tables[0].Rows.Count == 0)
    //    //        {
    //    //            oDs = null;
    //    //        }
    //    //    }
    //    //    catch (Exception ex)
    //    //    {
    //    //        objErrorHandler.ErrorMsg = ex;
    //    //        objErrorHandler.CreateLog();
    //    //        //oDs = null;
    //    //    }          
    //    //    return oDs;
    //    //}

    //    //public DataTable GetDataTableDB(string SQLString)
    //    //{
    //    //    DataTable objtbl = new DataTable();
    //    //    try
    //    //    {
    //    //        SqlDataAdapter objDataAdapter = new SqlDataAdapter(SQLString, objConnection.GetConnection());
    //    //        objDataAdapter.Fill(objtbl);
    //    //        objDataAdapter.Dispose();
    //    //    }
    //    //    catch (Exception ex)
    //    //    {
    //    //        objErrorHandler.ErrorMsg = ex;
    //    //        objErrorHandler.CreateLog();
    //    //        objtbl = null;
    //    //    }
    //    //    return objtbl;
    //    //}

    //    public int ExecuteSQLQueryDB( string SqlQuery )
    //    {
    //        int rValue;

    //        try
    //        {
    //            SqlCommand oCmd = new SqlCommand(SqlQuery, objConnection.GetConnection());
    //            rValue = oCmd.ExecuteNonQuery();
    //            oCmd.Dispose();
               
    //        }
    //        catch (Exception ex)
    //        {
    //            objErrorHandler.ErrorMsg = ex;
    //            objErrorHandler.CreateLog();
    //            rValue = -1;
    //        }
    //        finally
    //        {
    //            // new code
    //            objConnection.CloseConnection();
    //        }

    //        return rValue;
    //    }

        
    //    //public string GetValueDB(string ColumnName)
    //    //{
    //    //    string rValue = "";
    //    //    DataSet oDs = new DataSet();
    //    //    try
    //    //    {
    //    //        SqlDataAdapter oDA = new SqlDataAdapter(SQLString, objConnection.GetConnection());
    //    //        oDA.Fill(oDs);
    //    //        oDA.Dispose();
    //    //        if (oDs != null)
    //    //            foreach (DataRow oRow in oDs.Tables[0].Rows)
    //    //            {
    //    //                if (oRow[ColumnName] != DBNull.Value)
    //    //                {
    //    //                    rValue = oRow[ColumnName].ToString();
    //    //                }
    //    //                else
    //    //                {
    //    //                    rValue = "-1";
    //    //                }
    //    //            }

    //    //    }
    //    //    catch (Exception ex)
    //    //    {
    //    //        objErrorHandler.ErrorMsg = ex;
    //    //        objErrorHandler.CreateLog();
    //    //        rValue = "-1";
    //    //    }
    //    //    return rValue;
    //    //}
      
    //    //public void ReadConfigXMLDB()
    //    //{
    //    //    DataSet oDs = new DataSet();
    //    //    try
    //    //    {
    //    //        WebCatGlb = new Hashtable();
    //    //        AppDomain sPath;
    //    //        sPath = AppDomain.CurrentDomain;
    //    //        oDs.ReadXml(sPath.BaseDirectory + @"\WebCat.xml");
    //    //        oDs.Dispose();
    //    //    }
    //    //    catch (Exception ex)
    //    //    {
    //    //        objErrorHandler.ErrorMsg = ex;
    //    //        objErrorHandler.CreateLog();
    //    //    }
    //    //    finally
    //    //    {
    //    //        oDs.Dispose();
    //    //    }
    //    //}


       
    //    public DataSet GetProductPriceTable(int ProductID, int userid)
    //    {
    //        DataSet dsPriceTable = new DataSet();
    //        try
    //        {
    //            objSqlCommand = new SqlCommand("GetPriceTable", objConnection.GetConnection());
    //            objSqlCommand.CommandType = CommandType.StoredProcedure;
    //            objSqlCommand.Parameters.Add(new SqlParameter("@ProductID", ProductID));
    //            objSqlCommand.Parameters.Add(new SqlParameter("@UserID", userid));
    //            objDataAdapter = new SqlDataAdapter(objSqlCommand);
    //            objDataAdapter.Fill(dsPriceTable, "Price");
    //            objConnection.CloseConnection();
    //        }
    //        catch (Exception objException)
    //        {
    //            objErrorHandler.ErrorMsg = objException;
    //            objErrorHandler.CreateLog();
    //            return null;
    //        }
    //        finally
    //        {
    //            objSqlCommand.Dispose();
    //            objSqlCommand=null;
    //            objDataAdapter.Dispose();
    //            objDataAdapter = null;
    //            objConnection.CloseConnection();
    //        }


    //        return dsPriceTable;
    //    }
    //    public int GetPriceCode(string userid)
    //    {
    //        int _return = -1;
    //        string _tempstring="";
    //        try
    //        {
    //            _tempstring = (string)GetGenericDataDB(userid, "PROCE_CODE", HelperDB.ReturnType.RTString);
    //            if (_tempstring != null && _tempstring != "")
    //                _return = Convert.ToInt32(_tempstring);
    //        }
    //        catch (Exception objException)
    //        {
    //            objErrorHandler.ErrorMsg = objException;
    //            objErrorHandler.CreateLog();
    //            return -1;
    //        }
    //        finally
    //        {

    //        }
    //        return _return;
    //    }
    //    public decimal GetProductPrice(int productids, int Qty, string UserID)
    //    {
    //        decimal _return = 0.00M;
    //        DataSet ds = new DataSet();
    //        ds = GetProductPriceOnly(productids.ToString(), Qty, UserID);
    //        if (ds != null && ds.Tables.Count >= 1 && ds.Tables[0].Rows.Count > 0)
    //            _return = Math.Round(Convert.ToDecimal(ds.Tables[0].Rows[0][1].ToString()), 2);

    //        return _return;
    //    }
    //    public DataSet GetProductPrice(string familyids, string productids, string UserID)
    //    {
    //        DataSet ds = new DataSet();
    //        try
    //        {
    //            objSqlCommand = new SqlCommand("STP_TBWC_PICKFPRODUCTPRICE", objConnection.GetConnection());
    //            objSqlCommand.CommandType = CommandType.StoredProcedure;
    //            objSqlCommand.Parameters.Add(new SqlParameter("@FamilyIDs", familyids));
    //            objSqlCommand.Parameters.Add(new SqlParameter("@ProductIDs", productids));
    //            objSqlCommand.Parameters.Add(new SqlParameter("@UserID", UserID));
    //            objDataAdapter = new SqlDataAdapter(objSqlCommand);
    //            objDataAdapter.Fill(ds);
    //        }
    //        catch (Exception objException)
    //        {
    //            objErrorHandler.ErrorMsg = objException;
    //            objErrorHandler.CreateLog();
    //            return null;
    //        }
    //        finally
    //        {
    //            //new code 
    //            objConnection.CloseConnection();
                
    //        }
    //        return ds;

    //    }
    //    public DataSet GetProductPriceOnly( string productids,int Qty, string UserID)
    //    {
    //        DataSet ds = new DataSet();
    //        try
    //        {
    //            objSqlCommand = new SqlCommand("STP_TBWC_PICK_PRODUCT_PRICE_ONLY", objConnection.GetConnection());
    //            objSqlCommand.CommandType = CommandType.StoredProcedure;
               
    //            objSqlCommand.Parameters.Add(new SqlParameter("@ProductIDs", productids));
    //            objSqlCommand.Parameters.Add(new SqlParameter("@Qty", Qty));
    //            objSqlCommand.Parameters.Add(new SqlParameter("@UserID", UserID));
    //            objDataAdapter = new SqlDataAdapter(objSqlCommand);
    //            objDataAdapter.Fill(ds);               
    //        }
    //        catch (Exception objException)
    //        {
    //            objErrorHandler.ErrorMsg = objException;
    //            objErrorHandler.CreateLog();
    //            return null;
    //        }
    //        finally
    //        {
    //            //new code 
    //            objConnection.CloseConnection();
               
                
    //        }
    //        return ds;

    //    }
    //    public object GetGenericDataDB(string Param1, string ReturnOption, ReturnType ReturnType)
    //    {
    //        return GetGenericDataDB("", Param1, "", "", "", ReturnOption, ReturnType);
    //    }
    //    public object GetGenericDataDB(string Catalog_ID, string Param1, string ReturnOption, ReturnType ReturnType)
    //    {
    //        return GetGenericDataDB(Catalog_ID, Param1, "", "", "", ReturnOption, ReturnType);
    //    }        
    //    public object GetGenericDataDB(string Catalog_ID, string Param1, string Param2, string ReturnOption, ReturnType ReturnType)
    //    {
    //        return GetGenericDataDB(Catalog_ID, Param1, Param2, "", "", ReturnOption, ReturnType);
    //    }
    //    public object GetGenericDataDB(string Catalog_ID, string Param1, string Param2, string Param3, string ReturnOption, ReturnType ReturnType)
    //    {
    //        return GetGenericDataDB(Catalog_ID, Param1, Param2, Param3, "", ReturnOption, ReturnType);
    //    }
        
    //    public object GetGenericDataDB(string Catalog_ID, string Param1, string Param2, string Param3, string Param4, string ReturnOption, ReturnType ReturnType)
    //    {
    //        object TempReturn = null;
    //        try
    //        {
               
            
    //            objDataSet = new DataSet ();
    //            objDatatbl = new DataTable();
    //            objSqlCommand = new SqlCommand("STP_TBWC_PICKGENERICDATA", objConnection.GetConnection());
    //            objSqlCommand.CommandType = CommandType.StoredProcedure;
    //            objSqlCommand.Parameters.Add(new SqlParameter("@Catalog_ID", Catalog_ID));
    //            objSqlCommand.Parameters.Add(new SqlParameter("@Param1", Param1));
    //            objSqlCommand.Parameters.Add(new SqlParameter("@Param2", Param2));
    //            objSqlCommand.Parameters.Add(new SqlParameter("@Param3", Param3));
    //            objSqlCommand.Parameters.Add(new SqlParameter("@Param4", Param4));
    //            objSqlCommand.Parameters.Add(new SqlParameter("@ReturnOption", ReturnOption));
    //            objSqlCommand.Parameters.Add(new SqlParameter("@WebSiteID", WebSiteID));

    //            objDataR = objSqlCommand.ExecuteReader();
    //            objDatatbl.Load(objDataR);

    //            if (objDatatbl != null && objDatatbl.Rows.Count > 0)
    //            {
    //                if (ReturnType == HelperDB.ReturnType.RTString)
    //                {
    //                    TempReturn = objDatatbl.Rows[0][0].ToString();
    //                }
    //                else if (ReturnType == HelperDB.ReturnType.RTTable)
    //                {
    //                    TempReturn = objDatatbl;
    //                }
    //                else if (ReturnType == HelperDB.ReturnType.RTDataSet)
    //                {
    //                    objDataSet.Tables.Add(objDatatbl.Copy());
    //                    TempReturn = objDataSet;
    //                }
    //            }
    //            else
    //            {
    //                if (ReturnType == HelperDB.ReturnType.RTString)
    //                {
    //                    TempReturn = "";
    //                }
    //                else if (ReturnType == HelperDB.ReturnType.RTTable)
    //                {
    //                    TempReturn = null;
    //                }
    //                else if (ReturnType == HelperDB.ReturnType.RTDataSet)
    //                {
    //                    TempReturn = null;
    //                }
    //            }

    //            //objDataAdapter = new SqlDataAdapter(objSqlCommand);
    //            //objDataAdapter.Fill(objDataSet);

    //            //if (objDataSet != null && objDataSet.Tables.Count > 0 && objDataSet.Tables[0].Rows.Count > 0)
    //            //{
    //            //    if (ReturnType == HelperDB.ReturnType.RTString)
    //            //    {
    //            //        TempReturn = objDataSet.Tables[0].Rows[0][0].ToString();
    //            //    }
    //            //    else if (ReturnType == HelperDB.ReturnType.RTTable)
    //            //    {
    //            //        TempReturn = objDataSet.Tables[0];
    //            //    }
    //            //    else if (ReturnType == HelperDB.ReturnType.RTDataSet)
    //            //    {
    //            //        TempReturn = objDataSet;
    //            //    }
    //            //}
    //            //else
    //            //{
    //            //    if (ReturnType == HelperDB.ReturnType.RTString)
    //            //    {
    //            //        TempReturn = "";
    //            //    }
    //            //    else if (ReturnType == HelperDB.ReturnType.RTTable)
    //            //    {
    //            //        TempReturn = null;
    //            //    }
    //            //    else if (ReturnType == HelperDB.ReturnType.RTDataSet)
    //            //    {
    //            //        TempReturn = null;
    //            //    }
    //            //}
               
    //        }
    //        catch (Exception objException)
    //        {
    //             objErrorHandler.ErrorMsg = objException;
    //            objErrorHandler.CreateLog();
    //            return null;

    //        }
    //        finally
    //        {
    //            objConnection.CloseConnection();
    //            objSqlCommand.Dispose();
    //            objSqlCommand = null;
    //            objDataSet.Dispose();
    //            objDataSet = null;
    //            objDatatbl.Dispose();
    //            objDatatbl = null;
    //            objDataR.Close();
               
    //        }
    //        return TempReturn;
    //    }

    //    // Get Generic Page Data functions
    //    public object GetGenericPageDataDB(string Param1, string ReturnOption, ReturnType ReturnType)
    //    {
    //        return GetGenericPageDataDB("", Param1, "", "", "", ReturnOption, ReturnType);
    //    }
    //    public object GetGenericPageDataDB(string Catalog_ID, string Param1, string ReturnOption, ReturnType ReturnType)
    //    {
    //        return GetGenericPageDataDB(Catalog_ID, Param1, "", "", "", ReturnOption, ReturnType);
    //    }
    //    public object GetGenericPageDataDB(string Catalog_ID, string Param1, string Param2, string ReturnOption, ReturnType ReturnType)
    //    {
    //        return GetGenericPageDataDB(Catalog_ID, Param1, Param2, "", "", ReturnOption, ReturnType);
    //    }
    //    public object GetGenericPageDataDB(string Catalog_ID, string Param1, string Param2, string Param3, string ReturnOption, ReturnType ReturnType)
    //    {
    //        return GetGenericPageDataDB(Catalog_ID, Param1, Param2, Param3, "", ReturnOption, ReturnType);
    //    }

    //    public object GetGenericPageDataDB(string Catalog_ID, string Param1, string Param2, string Param3, string Param4, string ReturnOption, ReturnType ReturnType)
    //    {
    //        try
    //        {
    //            object TempReturn = null;

    //            objDataSet = new DataSet();
    //            objDatatbl = new DataTable();
    //            objSqlCommand = new SqlCommand("STP_TBWC_PICK_GENERIC_PAGE_DATA", objConnection.GetConnection());
    //            objSqlCommand.CommandType = CommandType.StoredProcedure;
    //            objSqlCommand.Parameters.Add(new SqlParameter("@Catalog_ID", Catalog_ID));
    //            objSqlCommand.Parameters.Add(new SqlParameter("@Param1", Param1));
    //            objSqlCommand.Parameters.Add(new SqlParameter("@Param2", Param2));
    //            objSqlCommand.Parameters.Add(new SqlParameter("@Param3", Param3));
    //            objSqlCommand.Parameters.Add(new SqlParameter("@Param4", Param4));
    //            objSqlCommand.Parameters.Add(new SqlParameter("@ReturnOption", ReturnOption));
    //            objSqlCommand.Parameters.Add(new SqlParameter("@WebSiteID", WebSiteID));

    //            objDataR = objSqlCommand.ExecuteReader();
    //            objDatatbl.Load(objDataR);                    

    //            if (objDatatbl != null && objDatatbl.Rows.Count > 0)
    //            {
    //                if (ReturnType == HelperDB.ReturnType.RTString)
    //                {
    //                    TempReturn = objDatatbl.Rows[0][0].ToString();
    //                }
    //                else if (ReturnType == HelperDB.ReturnType.RTTable)
    //                {
    //                    TempReturn = objDatatbl;
    //                }
    //                else if (ReturnType == HelperDB.ReturnType.RTDataSet)
    //                {
    //                    objDataSet.Tables.Add(objDatatbl.Copy());
    //                    TempReturn = objDataSet;
    //                }
    //            }
    //            else
    //            {
    //                if (ReturnType == HelperDB.ReturnType.RTString)
    //                {
    //                    TempReturn = "";
    //                }
    //                else if (ReturnType == HelperDB.ReturnType.RTTable)
    //                {
    //                    TempReturn = null;
    //                }
    //                else if (ReturnType == HelperDB.ReturnType.RTDataSet)
    //                {
    //                    TempReturn = null;
    //                }
    //            }

    //            //objDataAdapter = new SqlDataAdapter(objSqlCommand);
    //            //objDataAdapter.Fill(objDataSet);

    //            //if (objDataSet != null && objDataSet.Tables.Count > 0 && objDataSet.Tables[0].Rows.Count > 0)
    //            //{
    //            //    if (ReturnType == HelperDB.ReturnType.RTString)
    //            //    {
    //            //        TempReturn = objDataSet.Tables[0].Rows[0][0].ToString();
    //            //    }
    //            //    else if (ReturnType == HelperDB.ReturnType.RTTable)
    //            //    {
    //            //        TempReturn = objDataSet.Tables[0];
    //            //    }
    //            //    else if (ReturnType == HelperDB.ReturnType.RTDataSet)
    //            //    {
    //            //        TempReturn = objDataSet;
    //            //    }
    //            //}
    //            //else
    //            //{
    //            //    if (ReturnType == HelperDB.ReturnType.RTString)
    //            //    {
    //            //        TempReturn = "";
    //            //    }
    //            //    else if (ReturnType == HelperDB.ReturnType.RTTable)
    //            //    {
    //            //        TempReturn = null;
    //            //    }
    //            //    else if (ReturnType == HelperDB.ReturnType.RTDataSet)
    //            //    {
    //            //        TempReturn = null;
    //            //    }
    //            //}
    //            return TempReturn;
    //        }
    //        catch (Exception objException)
    //        {
    //            objErrorHandler.ErrorMsg = objException;
    //            objErrorHandler.CreateLog();
    //            return null;

    //        }
    //        finally
    //        {
    //            objConnection.CloseConnection();
    //            objSqlCommand.Dispose();
    //            objSqlCommand = null;
    //            objDataSet.Dispose();
    //            objDataSet = null;
    //            objDatatbl.Dispose();
    //            objDatatbl = null;
    //            objDataR.Close();
                
    //        }
    //    }
    //    #endregion
      
    //    # region "SQL Connection"
    //    //public SqlDataReader ExecuteReader(string strSql)
    //    //{
    //    //    SqlDataReader SqlDr=null;
    //    //    try
    //    //    {

    //    //        objSqlCommand = new SqlCommand(strSql, objConnection.GetConnection());
    //    //        SqlDr = objSqlCommand.ExecuteReader();

    //    //    }
    //    //    catch (Exception ex)
    //    //    {
    //    //        objErrorHandler.ErrorMsg = ex;
    //    //        objErrorHandler.CreateLog();
    //    //        return null;
    //    //    }
    //    //    finally
    //    //    {
    //    //        objSqlCommand = null;
    //    //    }
    //    //    return SqlDr;
    //    //}
    //    public DataTable  GetDataTableDB(string strSql)
    //    {
    //        DataSet Dst = new DataSet();
    //        try
    //        {
    //            objDataAdapter = new SqlDataAdapter(strSql, objConnection.GetConnection());
    //            objDataAdapter.Fill(Dst);             

    //        }
    //        catch (Exception ex)
    //        {
    //            objErrorHandler.ErrorMsg = ex;
    //            objErrorHandler.CreateLog();
    //            return null;
    //        }
    //        finally
    //        {
    //            // new code
    //            objConnection.CloseConnection();
    //            objDataAdapter.Dispose(); 
    //            objDataAdapter = null;
    //        }
    //        return Dst.Tables[0] ;
    //    }
    //    public DataSet GetDataSetDB(string strSql)
    //    {
    //        DataSet Dst = new DataSet();
    //        try
    //        {
    //            objDataAdapter = new SqlDataAdapter(strSql, objConnection.GetConnection());
    //            objDataAdapter.Fill(Dst);

    //        }
    //        catch (Exception ex)
    //        {
    //            objErrorHandler.ErrorMsg = ex;
    //            objErrorHandler.CreateLog();
    //            return null;
    //        }
    //        finally
    //        {
    //            // new code
    //            objConnection.CloseConnection();
    //            objDataAdapter.Dispose();
    //            objDataAdapter = null;
    //        }
    //        return Dst;
    //    }        

    //    #endregion

       


    //}

    #endregion

    /*********************************** J TECH CODE ***********************************/
    /// <summary>
    /// Helping objects for entire class
    /// </summary>
    /// <remarks>
    /// This is used to get the DataSet values and get the DataTable using Table Names
    /// </remarks>
    /// <example>
    /// Helper oHelper=new Helper();
    /// </example>
    public class HelperDB
    {
        /*********************************** DECLARATION ***********************************/
        ConnectionDB objConnection=new ConnectionDB();

        //SqlConnection GCon = new SqlConnection();
        //string StrConnectionString = ConfigurationManager.ConnectionStrings["DBConnectionString"].ToString();
        /// <summary>
        /// Hash Table
        /// </summary>

        string WebSiteID = ConfigurationManager.AppSettings["WEBSITEID"].ToString();

        public static Hashtable WebCatGlb = new Hashtable();
        private string sSqlStr;
        ErrorHandler objErrorHandler = new ErrorHandler();
        SqlCommand objSqlCommand;
        DataSet objDataSet = new DataSet();
        SqlDataReader objDataR = null;
        DataTable objDatatbl = new DataTable();
        SqlDataAdapter objDataAdapter;
        string ReturnValue = string.Empty;
        public enum ReturnType
        {
            RTString = 1,
            RTTable = 2,
            RTDataSet = 3            
        }
        public string SQLString
        {
            get
            {
                return sSqlStr;
            }
            set
            {
                sSqlStr = value;
            }
        }
        /*********************************** DECLARATION ***********************************/
        /*********************************** CONSTRUCTOR ***********************************/
        public HelperDB()
        {
            //GetInitialDetails();
        }
        /*********************************** CONSTRUCTOR ***********************************/

        #region "Properties"
        /// <summary>
        /// Set the Property for SQLString
        /// </summary>
        
        #endregion
        #region "Functions"
     
        //public DataSet GetDataSetDB(string sTableName)
        //{
        //    DataSet oDs = new DataSet();
        //    try
        //    {
        //        SqlDataAdapter oDA = new SqlDataAdapter(SQLString, objConnection.GetConnection());
        //        oDA.Fill(oDs, sTableName);
        //        oDA.Dispose();
        //        if (oDs.Tables[sTableName].Rows.Count == 0)
        //        {
        //            oDs = null;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        objErrorHandler.ErrorMsg = ex;
        //        objErrorHandler.CreateLog();
        //        oDs = null;
        //    }          
        //    return oDs;

        //}

       
        //public DataSet GetDataSetDB()
        //{
        //    DataSet oDs = new DataSet();
        //    try
        //    {
        //        SqlDataAdapter oDA = new SqlDataAdapter(SQLString, objConnection.GetConnection());
        //        oDA.Fill(oDs);
        //        oDA.Dispose();
        //        if (oDs.Tables[0].Rows.Count == 0)
        //        {
        //            oDs = null;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        objErrorHandler.ErrorMsg = ex;
        //        objErrorHandler.CreateLog();
        //        //oDs = null;
        //    }          
        //    return oDs;
        //}

        //public DataTable GetDataTableDB(string SQLString)
        //{
        //    DataTable objtbl = new DataTable();
        //    try
        //    {
        //        SqlDataAdapter objDataAdapter = new SqlDataAdapter(SQLString, objConnection.GetConnection());
        //        objDataAdapter.Fill(objtbl);
        //        objDataAdapter.Dispose();
        //    }
        //    catch (Exception ex)
        //    {
        //        objErrorHandler.ErrorMsg = ex;
        //        objErrorHandler.CreateLog();
        //        objtbl = null;
        //    }
        //    return objtbl;
        //}

        /*********************************************************************************/
        /*** ORGANIZATION : J TECH ***/
        /*** PURPOSE      : TO RETRIVE NUMBER OF ROWS AFFECTED IN A TABLE ***/
        /********************************************************************************/

        public int ExecuteSQLQueryDB( string SqlQuery )
        {
            int rValue;

            try
            {
                SqlCommand oCmd = new SqlCommand(SqlQuery, objConnection.GetConnection());
                rValue = oCmd.ExecuteNonQuery();
                oCmd.Dispose();
               
            }
            catch (Exception ex)
            {
                //objErrorHandler.ErrorMsg = ex;
                //objErrorHandler.CreateLog();
                objErrorHandler.CreateLog( ex.ToString()+SqlQuery);
                rValue = -1;
            }
            finally
            {
                // new code
               // objConnection.CloseConnection();
            }

            return rValue;
        }

        /*********************************************************************************/
        /*** ORGANIZATION : J TECH ***/
        /*** PURPOSE      : TO RETRIVE THE IDENTITY VALUE ***/
        /********************************************************************************/

        public int  ExecuteSQLQueryDBRtnIdentity(string SqlQuery)
        {
            int rValue=0;
            
            DataSet dsId = new DataSet();
            try
            {
                objSqlCommand = new SqlCommand(SqlQuery, objConnection.GetConnection());
                objSqlCommand.CommandType = CommandType.Text; 
                objDataAdapter = new SqlDataAdapter(objSqlCommand);
                objDataAdapter.Fill(dsId, "id");
                objConnection.CloseConnection();
                if (dsId != null && dsId.Tables.Count > 0 && dsId.Tables[0].Rows.Count > 0)
                {
                    rValue = Convert.ToInt32(dsId.Tables[0].Rows[0][0]);
                }
            }
            catch (Exception objException)
            {
                objErrorHandler.ErrorMsg = objException;
                objErrorHandler.CreateLog();
                return 0;
            }
            finally
            {
                objSqlCommand.Dispose();
                objSqlCommand = null;
                objDataAdapter.Dispose();
                objDataAdapter = null;
                objConnection.CloseConnection();
            }


            return rValue;
        }
        //public string GetValueDB(string ColumnName)
        //{
        //    string rValue = "";
        //    DataSet oDs = new DataSet();
        //    try
        //    {
        //        SqlDataAdapter oDA = new SqlDataAdapter(SQLString, objConnection.GetConnection());
        //        oDA.Fill(oDs);
        //        oDA.Dispose();
        //        if (oDs != null)
        //            foreach (DataRow oRow in oDs.Tables[0].Rows)
        //            {
        //                if (oRow[ColumnName] != DBNull.Value)
        //                {
        //                    rValue = oRow[ColumnName].ToString();
        //                }
        //                else
        //                {
        //                    rValue = "-1";
        //                }
        //            }

        //    }
        //    catch (Exception ex)
        //    {
        //        objErrorHandler.ErrorMsg = ex;
        //        objErrorHandler.CreateLog();
        //        rValue = "-1";
        //    }
        //    return rValue;
        //}
      
        //public void ReadConfigXMLDB()
        //{
        //    DataSet oDs = new DataSet();
        //    try
        //    {
        //        WebCatGlb = new Hashtable();
        //        AppDomain sPath;
        //        sPath = AppDomain.CurrentDomain;
        //        oDs.ReadXml(sPath.BaseDirectory + @"\WebCat.xml");
        //        oDs.Dispose();
        //    }
        //    catch (Exception ex)
        //    {
        //        objErrorHandler.ErrorMsg = ex;
        //        objErrorHandler.CreateLog();
        //    }
        //    finally
        //    {
        //        oDs.Dispose();
        //    }
        //}

        /*********************************************************************************/
        /*** ORGANIZATION : J TECH ***/
        /*** PURPOSE      : TO RETRIVE PRODUCT PRICE DETAILS FROM DATABASE BASED ON PRODUCT ID AND USER ID ***/
        /********************************************************************************/

        
        public DataSet GetProductPriceTable(int ProductID, int userid)
        {
            DataSet dsPriceTable = new DataSet();
            try
            {
                objSqlCommand = new SqlCommand("GetPriceTable", objConnection.GetConnection());
                objSqlCommand.CommandType = CommandType.StoredProcedure;
                objSqlCommand.Parameters.Add(new SqlParameter("@ProductID", ProductID));
                objSqlCommand.Parameters.Add(new SqlParameter("@UserID", userid));
                objDataAdapter = new SqlDataAdapter(objSqlCommand);
                objDataAdapter.Fill(dsPriceTable, "Price");
                objConnection.CloseConnection();
            }
            catch (Exception objException)
            {
                objErrorHandler.ErrorMsg = objException;
                objErrorHandler.CreateLog();
                return null;
            }
            finally
            {
                objSqlCommand.Dispose();
                objSqlCommand=null;
                objDataAdapter.Dispose();
                objDataAdapter = null;
                objConnection.CloseConnection();
            }


            return dsPriceTable;
        }

        /*********************************************************************************/
        /*** ORGANIZATION : J TECH ***/
        /*** PURPOSE      : TO RETRIVE ALL PRODUCT PRICE DETAILS FROM DATABASE  ***/
        /********************************************************************************/

        public DataSet GetProductPriceTableAll(string ProductIDs, int userid)
        {
            DataSet dsPriceTable = new DataSet();
            try
            {
                objSqlCommand = new SqlCommand("GetPriceTableAll", objConnection.GetConnection());
                objSqlCommand.CommandType = CommandType.StoredProcedure;
                objSqlCommand.Parameters.Add(new SqlParameter("@ProductIds", ProductIDs));
                objSqlCommand.Parameters.Add(new SqlParameter("@userid", userid));
                objDataAdapter = new SqlDataAdapter(objSqlCommand);
                objDataAdapter.Fill(dsPriceTable, "Price");
                objConnection.CloseConnection();
            }
            catch (Exception objException)
            {
                objErrorHandler.ErrorMsg = objException;
                objErrorHandler.CreateLog();
                return null;
            }
            finally
            {
                objSqlCommand.Dispose();
                objSqlCommand = null;
                objDataAdapter.Dispose();
                objDataAdapter = null;
                objConnection.CloseConnection();
            }


            return dsPriceTable;
        }

        /*********************************************************************************/
        /*** ORGANIZATION : J TECH ***/
        /*** PURPOSE      : TO RETRIVE PRODUCT PRICE CODE FROM DATABASE BASED ON USERID ***/
        /********************************************************************************/

        public int GetPriceCode(string userid)
        {
            int _return = -1;
            string _tempstring="";
            try
            {
                _tempstring = (string)GetGenericDataDB(userid, "PROCE_CODE", HelperDB.ReturnType.RTString);
                if (_tempstring != null && _tempstring != "")
                    _return = Convert.ToInt32(_tempstring);
            }
            catch (Exception objException)
            {
                objErrorHandler.ErrorMsg = objException;
                objErrorHandler.CreateLog();
                return -1;
            }
            finally
            {

            }
            return _return;
        }
        /*********************************************************************************/
        /*** ORGANIZATION : J TECH ***/
        /*** PURPOSE      : TO ROUND THE PRODUCT PRICE  ***/
        /********************************************************************************/

        public decimal GetProductPrice(int productids, int Qty, string UserID)
        {
            decimal _return = 0.00M;
            DataSet ds = new DataSet();
            ds = GetProductPriceOnly(productids.ToString(), Qty, UserID);
            if (ds != null && ds.Tables.Count >= 1 && ds.Tables[0].Rows.Count > 0)
                _return = Math.Round(Convert.ToDecimal(ds.Tables[0].Rows[0][1].ToString()), 2);

            return _return;
        }

        /*********************************************************************************/
        /*** ORGANIZATION : J TECH ***/
        /*** PURPOSE      : TO RETRIVE PRODUCT PRICE INCLUDING GST PRICE DETAILS ***/
        /********************************************************************************/

        public decimal GetProductPrice_inc(int productids, int Qty, string UserID)
        {
            decimal _return = 0.00M;
            DataSet ds = new DataSet();
            ds = GetProductPrice_Inc_Exc(productids.ToString(), Qty, UserID);
            if (ds != null && ds.Tables.Count >= 1 && ds.Tables[0].Rows.Count > 0)
                _return = Math.Round(Convert.ToDecimal(ds.Tables[0].Rows[0]["Inc_GST_Price"].ToString()), 2);

            return _return;
        }
        /*********************************************************************************/
        /*** ORGANIZATION : J TECH ***/
        /*** PURPOSE      : TO RETRIVE PRODUCT PRICE EXCLUDING GST PRICE DETAILS  ***/
        /********************************************************************************/

        public decimal GetProductPrice_Exc(int productids, int Qty, string UserID)
        {
            decimal _return = 0.00M;
            DataSet ds = new DataSet();
            ds = GetProductPrice_Inc_Exc(productids.ToString(), Qty, UserID);
            if (ds != null && ds.Tables.Count >= 1 && ds.Tables[0].Rows.Count > 0)
                _return = Math.Round(Convert.ToDecimal(ds.Tables[0].Rows[0]["Exc_GST_Price"].ToString()), 4);
                //_return = Math.Round(Convert.ToDecimal(ds.Tables[0].Rows[0]["Exc_GST_Price"].ToString()), 2);

            return _return;
        }

        /*********************************************************************************/
        /*** ORGANIZATION : J TECH ***/
        /*** PURPOSE      : TO RETRIVE PRODUCT PRICE DETAILS FROM DATABASE BASED ON PARAMETERS ***/
        /********************************************************************************/
        public DataSet GetProductPriceEA(string familyids, string productids, string UserID)
        {
            DataSet ds = new DataSet();
            try
            {
                objSqlCommand = new SqlCommand("STP_TBWC_PICKFPRODUCTPRICE_EA", objConnection.GetConnection());
                objSqlCommand.CommandType = CommandType.StoredProcedure;
                objSqlCommand.Parameters.Add(new SqlParameter("@FamilyIDs", familyids));
                objSqlCommand.Parameters.Add(new SqlParameter("@ProductIDs", productids));
                objSqlCommand.Parameters.Add(new SqlParameter("@UserID", UserID));
                objDataAdapter = new SqlDataAdapter(objSqlCommand);
                objDataAdapter.Fill(ds);
            }
            catch (Exception objException)
            {
                objErrorHandler.ErrorMsg = objException;
                objErrorHandler.CreateLog();
                return null;
            }
            finally
            {
                //new code 
                // objConnection.CloseConnection();

            }
            return ds;

        }
        public DataSet GetProductPrice(string familyids, string productids, string UserID)
        {
            DataSet ds = new DataSet();
            try
            {
                objSqlCommand = new SqlCommand("STP_TBWC_PICKFPRODUCTPRICE", objConnection.GetConnection());
                objSqlCommand.CommandType = CommandType.StoredProcedure;
                objSqlCommand.Parameters.Add(new SqlParameter("@FamilyIDs", familyids));
                objSqlCommand.Parameters.Add(new SqlParameter("@ProductIDs", productids));
                objSqlCommand.Parameters.Add(new SqlParameter("@UserID", UserID));
                objDataAdapter = new SqlDataAdapter(objSqlCommand);
                objDataAdapter.Fill(ds);
            }
            catch (Exception objException)
            {
                objErrorHandler.ErrorMsg = objException;
                objErrorHandler.CreateLog();
                return null;
            }
            finally
            {
                //new code 
               // objConnection.CloseConnection();
                
            }
            return ds;

        }

        /*********************************************************************************/
        /*** ORGANIZATION : J TECH ***/
        /*** PURPOSE      : TO RETRIVE PRODUCT PRICE FROM DATABASE BASED ON PRODUCTID,QUANTITY,USERID ***/
        /********************************************************************************/

        public DataSet GetProductPriceOnly( string productids,int Qty, string UserID)
        {
            DataSet ds = new DataSet();
            try
            {
                objSqlCommand = new SqlCommand("STP_TBWC_PICK_PRODUCT_PRICE_ONLY", objConnection.GetConnection());
                objSqlCommand.CommandType = CommandType.StoredProcedure;
               
                objSqlCommand.Parameters.Add(new SqlParameter("@ProductIDs", productids));
                objSqlCommand.Parameters.Add(new SqlParameter("@Qty", Qty));
                objSqlCommand.Parameters.Add(new SqlParameter("@UserID", UserID));
                objDataAdapter = new SqlDataAdapter(objSqlCommand);
                objDataAdapter.Fill(ds);               
            }
            catch (Exception objException)
            {
                objErrorHandler.ErrorMsg = objException;
                objErrorHandler.CreateLog();
                return null;
            }
            finally
            {
                //new code 
               // objConnection.CloseConnection();
               
                
            }
            return ds;

        }

        /*********************************************************************************/
        /*** ORGANIZATION : J TECH ***/
        /*** PURPOSE      : TO RETRIVE POWER SEARCH PRODUCT DETAILS BASED ON POWER SEARCH TEXT ***/
        /********************************************************************************/

        public DataSet GetPowerSearchProducts(string Searchtxt,int price_code,int user_id)
        {
            DataSet ds = new DataSet();
            try
            {

                objSqlCommand = new SqlCommand("STP_PICK_POWER_SEARCH_PRODUCTS", objConnection.GetConnection());
                objSqlCommand.CommandType = CommandType.StoredProcedure;

                
                objSqlCommand.Parameters.Add(new SqlParameter("@WebSite_id", Convert.ToInt32(WebSiteID)));
                objSqlCommand.Parameters.Add(new SqlParameter("@searchValue", Searchtxt));
                objSqlCommand.Parameters.Add(new SqlParameter("@Price_Code", price_code));
                objSqlCommand.Parameters.Add(new SqlParameter("@userId", user_id));
                objDataAdapter = new SqlDataAdapter(objSqlCommand);
                objDataAdapter.Fill(ds);
            }
            catch (Exception objException)
            {
                objErrorHandler.ErrorMsg = objException;
                objErrorHandler.CreateLog();
                return null;
            }
            finally
            {
                //new code 
               // objConnection.CloseConnection();


            }
            return ds;

        }
        /*********************************************************************************/
        /*** ORGANIZATION : J TECH ***/
        /*** PURPOSE      : TO RETRIVE PRODUCT PRICE DETAILS INCLUDING AND EXCLUDING GST_PRICE ***/
        /********************************************************************************/

        public DataSet GetProductPrice_Inc_Exc(string productids, int Qty, string UserID)
        {
            DataSet ds = new DataSet();
            try
            {
                ConnectionDB objConnection = new ConnectionDB();
                objSqlCommand = new SqlCommand("STP_TBWC_PICK_PRODUCT_PRICE_INC_EXC", objConnection.GetConnection());
                objSqlCommand.CommandType = CommandType.StoredProcedure;

                objSqlCommand.Parameters.Add(new SqlParameter("@ProductIDs", productids));
                objSqlCommand.Parameters.Add(new SqlParameter("@Qty", Qty));
                objSqlCommand.Parameters.Add(new SqlParameter("@UserID", UserID));
                objDataAdapter = new SqlDataAdapter(objSqlCommand);
                objDataAdapter.Fill(ds);
                objConnection.CloseConnection();
            }
            catch (Exception objException)
            {
                objErrorHandler.ErrorMsg = objException;
                objErrorHandler.CreateLog();
                return null;
            }
            finally
            {

            }
            return ds;

        }

        /*********************************************************************************/
        /*** ORGANIZATION : J TECH ***/
        /*** PURPOSE      : TO RETRIVE GENERICDATA DETAILS FROM DATABASE BASED ON PARAMETERS ***/
        /********************************************************************************/

        public object GetGenericDataDB(string Param1, string ReturnOption, ReturnType ReturnType)
        {
            return GetGenericDataDB("", Param1, "", "", "", ReturnOption, ReturnType);
        }
        public object GetGenericDataDB(string Catalog_ID, string Param1, string ReturnOption, ReturnType ReturnType)
        {
            return GetGenericDataDB(Catalog_ID, Param1, "", "", "", ReturnOption, ReturnType);
        }        
        public object GetGenericDataDB(string Catalog_ID, string Param1, string Param2, string ReturnOption, ReturnType ReturnType)
        {
            return GetGenericDataDB(Catalog_ID, Param1, Param2, "", "", ReturnOption, ReturnType);
        }
        public object GetGenericDataDB(string Catalog_ID, string Param1, string Param2, string Param3, string ReturnOption, ReturnType ReturnType)
        {
            return GetGenericDataDB(Catalog_ID, Param1, Param2, Param3, "", ReturnOption, ReturnType);
        }
        
        public object GetGenericDataDB(string Catalog_ID, string Param1, string Param2, string Param3, string Param4, string ReturnOption, ReturnType ReturnType)
        {
            object TempReturn = null;
            try
            {
               
            
                objDataSet = new DataSet ();
                objDatatbl = new DataTable();
                objSqlCommand = new SqlCommand("STP_TBWC_PICKGENERICDATA", objConnection.GetConnection());
                objSqlCommand.CommandType = CommandType.StoredProcedure;
                objSqlCommand.Parameters.Add(new SqlParameter("@Catalog_ID", Catalog_ID));
                objSqlCommand.Parameters.Add(new SqlParameter("@Param1", Param1));
                objSqlCommand.Parameters.Add(new SqlParameter("@Param2", Param2));
                objSqlCommand.Parameters.Add(new SqlParameter("@Param3", Param3));
                objSqlCommand.Parameters.Add(new SqlParameter("@Param4", Param4));
                objSqlCommand.Parameters.Add(new SqlParameter("@ReturnOption", ReturnOption));
                objSqlCommand.Parameters.Add(new SqlParameter("@WebSiteID", WebSiteID));

                objDataR = objSqlCommand.ExecuteReader();
                objDatatbl.Load(objDataR);

                if (objDatatbl != null && objDatatbl.Rows.Count > 0)
                {
                    if (ReturnType == HelperDB.ReturnType.RTString)
                    {
                        TempReturn = objDatatbl.Rows[0][0].ToString();
                    }
                    else if (ReturnType == HelperDB.ReturnType.RTTable)
                    {
                        TempReturn = objDatatbl;
                    }
                    else if (ReturnType == HelperDB.ReturnType.RTDataSet)
                    {
                        objDataSet.Tables.Add(objDatatbl.Copy());
                        TempReturn = objDataSet;
                    }
                }
                else
                {
                    if (ReturnType == HelperDB.ReturnType.RTString)
                    {
                        TempReturn = "";
                    }
                    else if (ReturnType == HelperDB.ReturnType.RTTable)
                    {
                        TempReturn = null;
                    }
                    else if (ReturnType == HelperDB.ReturnType.RTDataSet)
                    {
                        TempReturn = null;
                    }
                }

                //objDataAdapter = new SqlDataAdapter(objSqlCommand);
                //objDataAdapter.Fill(objDataSet);

                //if (objDataSet != null && objDataSet.Tables.Count > 0 && objDataSet.Tables[0].Rows.Count > 0)
                //{
                //    if (ReturnType == HelperDB.ReturnType.RTString)
                //    {
                //        TempReturn = objDataSet.Tables[0].Rows[0][0].ToString();
                //    }
                //    else if (ReturnType == HelperDB.ReturnType.RTTable)
                //    {
                //        TempReturn = objDataSet.Tables[0];
                //    }
                //    else if (ReturnType == HelperDB.ReturnType.RTDataSet)
                //    {
                //        TempReturn = objDataSet;
                //    }
                //}
                //else
                //{
                //    if (ReturnType == HelperDB.ReturnType.RTString)
                //    {
                //        TempReturn = "";
                //    }
                //    else if (ReturnType == HelperDB.ReturnType.RTTable)
                //    {
                //        TempReturn = null;
                //    }
                //    else if (ReturnType == HelperDB.ReturnType.RTDataSet)
                //    {
                //        TempReturn = null;
                //    }
                //}
               
            }
            catch (Exception objException)
            {
                 //objErrorHandler.ErrorMsg = objException;
                 objErrorHandler.CreateLog(objException.ToString() + "STP_TBWC_PICKGENERICDATA " + "," + Catalog_ID + "," + Param1 + "," + Param2 + "," + Param3 + "," + Param4 + "," + ReturnOption);
                return null;

            }
            finally
            {
                objConnection.CloseConnection();
                objSqlCommand.Dispose();
                objSqlCommand = null;
                objDataSet.Dispose();
                objDataSet = null;
                objDatatbl.Dispose();
                objDatatbl = null;
                objDataR.Close();
               
            }
            return TempReturn;
        }

        /*********************************************************************************/
        /*** ORGANIZATION : J TECH ***/
        /*** PURPOSE      : TO RETRIVE GENERIC PAGE DATA FUNCTIONS FROM DATABASE BASED ON PARAMETERS ***/
        /********************************************************************************/

        // Get Generic Page Data functions
        public object GetGenericPageDataDB(string Param1, string ReturnOption, ReturnType ReturnType)
        {
            return GetGenericPageDataDB("", Param1, "", "", "", ReturnOption, ReturnType);
        }
        public object GetGenericPageDataDB(string Catalog_ID, string Param1, string ReturnOption, ReturnType ReturnType)
        {
            return GetGenericPageDataDB(Catalog_ID, Param1, "", "", "", ReturnOption, ReturnType);
        }
        public object GetGenericPageDataDB(string Catalog_ID, string Param1, string Param2, string ReturnOption, ReturnType ReturnType)
        {
            return GetGenericPageDataDB(Catalog_ID, Param1, Param2, "", "", ReturnOption, ReturnType);
        }
        public object GetGenericPageDataDB(string Catalog_ID, string Param1, string Param2, string Param3, string ReturnOption, ReturnType ReturnType)
        {
            return GetGenericPageDataDB(Catalog_ID, Param1, Param2, Param3, "", ReturnOption, ReturnType);
        }

        public object GetGenericPageDataDB(string Catalog_ID, string Param1, string Param2, string Param3, string Param4, string ReturnOption, ReturnType ReturnType)
        {
            try
            {
                object TempReturn = null;

                objDataSet = new DataSet();
                objDatatbl = new DataTable();
                objSqlCommand = new SqlCommand("STP_TBWC_PICK_GENERIC_PAGE_DATA", objConnection.GetConnection());
                objSqlCommand.CommandType = CommandType.StoredProcedure;
                objSqlCommand.Parameters.Add(new SqlParameter("@Catalog_ID", Catalog_ID));
                objSqlCommand.Parameters.Add(new SqlParameter("@Param1", Param1));
                objSqlCommand.Parameters.Add(new SqlParameter("@Param2", Param2));
                objSqlCommand.Parameters.Add(new SqlParameter("@Param3", Param3));
                objSqlCommand.Parameters.Add(new SqlParameter("@Param4", Param4));
                objSqlCommand.Parameters.Add(new SqlParameter("@ReturnOption", ReturnOption));
                objSqlCommand.Parameters.Add(new SqlParameter("@WebSiteID", WebSiteID));

                objDataR = objSqlCommand.ExecuteReader();
                objDatatbl.Load(objDataR);                    

                if (objDatatbl != null && objDatatbl.Rows.Count > 0)
                {
                    if (ReturnType == HelperDB.ReturnType.RTString)
                    {
                        TempReturn = objDatatbl.Rows[0][0].ToString();
                    }
                    else if (ReturnType == HelperDB.ReturnType.RTTable)
                    {
                        TempReturn = objDatatbl;
                    }
                    else if (ReturnType == HelperDB.ReturnType.RTDataSet)
                    {
                        objDataSet.Tables.Add(objDatatbl.Copy());
                        TempReturn = objDataSet;
                    }
                }
                else
                {
                    if (ReturnType == HelperDB.ReturnType.RTString)
                    {
                        TempReturn = "";
                    }
                    else if (ReturnType == HelperDB.ReturnType.RTTable)
                    {
                        TempReturn = null;
                    }
                    else if (ReturnType == HelperDB.ReturnType.RTDataSet)
                    {
                        TempReturn = null;
                    }
                }

                //objDataAdapter = new SqlDataAdapter(objSqlCommand);
                //objDataAdapter.Fill(objDataSet);

                //if (objDataSet != null && objDataSet.Tables.Count > 0 && objDataSet.Tables[0].Rows.Count > 0)
                //{
                //    if (ReturnType == HelperDB.ReturnType.RTString)
                //    {
                //        TempReturn = objDataSet.Tables[0].Rows[0][0].ToString();
                //    }
                //    else if (ReturnType == HelperDB.ReturnType.RTTable)
                //    {
                //        TempReturn = objDataSet.Tables[0];
                //    }
                //    else if (ReturnType == HelperDB.ReturnType.RTDataSet)
                //    {
                //        TempReturn = objDataSet;
                //    }
                //}
                //else
                //{
                //    if (ReturnType == HelperDB.ReturnType.RTString)
                //    {
                //        TempReturn = "";
                //    }
                //    else if (ReturnType == HelperDB.ReturnType.RTTable)
                //    {
                //        TempReturn = null;
                //    }
                //    else if (ReturnType == HelperDB.ReturnType.RTDataSet)
                //    {
                //        TempReturn = null;
                //    }
                //}
                return TempReturn;
            }
            catch (Exception objException)
            {
                //objErrorHandler.ErrorMsg = objException;
                //objErrorHandler.CreateLog();
                objErrorHandler.CreateLog(objException.ToString() + "STP_TBWC_PICK_GENERIC_PAGE_DATA " + "," + Catalog_ID + "," + Param1 + "," + Param2 + "," + Param3 + "," + Param4 + "," + ReturnOption);
                return null;

            }
            finally
            {
                objConnection.CloseConnection();
                objSqlCommand.Dispose();
                objSqlCommand = null;
                objDataSet.Dispose();
                objDataSet = null;
                objDatatbl.Dispose();
                objDatatbl = null;
                objDataR.Close();
                
            }
        }
        #endregion
      
        # region "SQL Connection"
        //public SqlDataReader ExecuteReader(string strSql)
        //{
        //    SqlDataReader SqlDr=null;
        //    try
        //    {

        //        objSqlCommand = new SqlCommand(strSql, objConnection.GetConnection());
        //        SqlDr = objSqlCommand.ExecuteReader();

        //    }
        //    catch (Exception ex)
        //    {
        //        objErrorHandler.ErrorMsg = ex;
        //        objErrorHandler.CreateLog();
        //        return null;
        //    }
        //    finally
        //    {
        //        objSqlCommand = null;
        //    }
        //    return SqlDr;
        //}


        /*********************************************************************************/
        /*** ORGANIZATION : J TECH ***/
        /*** PURPOSE      : TO RETRIVE PARTICULAR DATASET TABLE DETAILS  ***/
        /********************************************************************************/

        public DataTable  GetDataTableDB(string strSql)
        {
            DataSet Dst = new DataSet();
            try
            {
                objDataAdapter = new SqlDataAdapter(strSql, objConnection.GetConnection());
                objDataAdapter.Fill(Dst);             

            }
            catch (Exception ex)
            {
                objErrorHandler.ErrorMsg = ex;
                objErrorHandler.CreateLog(ex.ToString() + strSql);
                return null;
            }
            finally
            {
                // new code
                objConnection.CloseConnection();
                objDataAdapter.Dispose(); 
                objDataAdapter = null;
            }
            return Dst.Tables[0] ;
        }

        /*********************************************************************************/
        /*** ORGANIZATION : J TECH ***/
        /*** PURPOSE      : TO RETRIVE DATASET TABLE VALUES  ***/
        /********************************************************************************/

        public DataSet GetDataSetDB(string strSql)
        {
            DataSet Dst = new DataSet();
            try
            {
                objDataAdapter = new SqlDataAdapter(strSql, objConnection.GetConnection());
                objDataAdapter.Fill(Dst);

            }
            catch (Exception ex)
            {
                //objErrorHandler.ErrorMsg = strSql + ex;
                objErrorHandler.CreateLog(strSql + ex.Message.ToString());
                return null;
            }
            finally
            {
                // new code
                objConnection.CloseConnection();
                objDataAdapter.Dispose();
                objDataAdapter = null;
            }
            return Dst;
        }        

        #endregion

       


    }

    /*********************************** J TECH CODE ***********************************/
}