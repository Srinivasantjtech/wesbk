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
using System.Text;
using StringTemplate = Antlr3.ST.StringTemplate;
using StringTemplateGroup = Antlr3.ST.StringTemplateGroup;
using TradingBell.WebCat.CatalogDB;
using TradingBell.WebCat.Helpers;
using TradingBell.WebCat.TemplateRender;
using TradingBell.WebCat.CommonServices;

public partial class UC_sitemap : System.Web.UI.UserControl
{
    HelperServices objHelperServices = new HelperServices();
    ConnectionDB objConnectionDB = new ConnectionDB();
    HelperDB objHelperDB = new HelperDB();
    ErrorHandler objErrorHandler = new ErrorHandler();
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    public string ST_Sitemap()
    {

        return (objHelperServices.StripWhitespace( Category_RenderHTML("SITEMAP", Server.MapPath(objHelperServices.GetOptionValues("SHOPPTING TEMPLATE PATH").ToString()))));
    }


    public string Category_RenderHTML(string Package, string SkinRootPath)
    {

        try
        {
        string skin_container = null;
        int grid_cols = 0;
        int grid_rows = 0;
        string skin_sql_container = null;
        string skin_sql_param_container = null;
        string skin_records = null;
        TBWDataList[] lstrecords = new TBWDataList[0];
        StringTemplateGroup stg_records = null;
        StringTemplate bodyST = null;
        StringTemplate bodyST_categorylist = null;
        StringTemplate bodyST_head = null;
        StringTemplate bodyST_list1 = null;
        string firstval = null;
        //List<string> name = new List<string>();
        StringBuilder name = new StringBuilder();
        System.Text.StringBuilder categorylist = new StringBuilder();
        System.Text.StringBuilder categoryrowlist = new StringBuilder();
        int indV = 0;
        int bodyValue = 0;
        DataSet dspkg = new DataSet();
        //string sqlpkginfo = " SELECT * FROM TBW_PACKAGE ";
        //sqlpkginfo = sqlpkginfo + " WHERE PACKAGE_NAME = '" + Package + "'";
        //dspkg = GetDataSet(sqlpkginfo);
        dspkg = (DataSet)objHelperDB.GetGenericDataDB(Package, "GET_PACKAGE_WITHOUT_ISROOT", HelperDB.ReturnType.RTDataSet);  
        if (dspkg != null)
        {
            if (dspkg.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in dspkg.Tables[0].Rows)
                {
                    skin_container = dr["SKIN_NAME"].ToString();
                    grid_cols = Convert.ToInt32(dr["GRID_COLS"]);
                    grid_rows = Convert.ToInt32(dr["GRID_ROWS"]);
                    skin_sql_container = dr["SKIN_SQL"].ToString();
                    skin_sql_param_container = dr["SKIN_SQL_PARAM"].ToString();
                    skin_records = dr["SKIN_NAME"].ToString();
                }
            }
        }

        //Build the inner body of the HTML
        stg_records = new StringTemplateGroup(skin_records, SkinRootPath + "\\" + skin_records);
        DataSet dsrecords = objHelperDB.GetDataSetDB(skin_sql_container);
        if (dsrecords != null && dsrecords.Tables[0].Rows.Count != 0)
        {
            lstrecords = new TBWDataList[dsrecords.Tables[0].Rows.Count * 2];
            //int catno = 0;
            if (dsrecords.Tables[0].Rows[0][1].ToString() != null && dsrecords.Tables[0].Rows[0][1].ToString() != string.Empty)
            {
                //Build the Sub heading 
                firstval = dsrecords.Tables[0].Rows[0][1].ToString().ToUpper();
                bodyST_categorylist = stg_records.GetInstanceOf("cell");
                DataRow ddr = dsrecords.Tables[0].Rows[0];
                foreach (DataColumn dc in dsrecords.Tables[0].Columns)
                {
                    if (dc.ColumnName.ToString().Contains("CUSTOM_NUM"))
                        if (ddr[dc.ColumnName].ToString().Length > 0)
                            bodyST_categorylist.SetAttribute(dc.ColumnName, Convert.ToDouble(ddr[dc.ColumnName].ToString()));
                        else
                            bodyST_categorylist.SetAttribute(dc.ColumnName, "0");
                    else
                        bodyST_categorylist.SetAttribute(dc.ColumnName, ddr[dc.ColumnName].ToString());
                }
            }
            foreach (DataRow dr in dsrecords.Tables[0].Rows)
            {
                if (dr[1].ToString() != null && dr[1].ToString() != string.Empty)
                {
                    if (firstval != dr[1].ToString().ToUpper())
                    {
                        //Build the category 
                        bodyST_categorylist.SetAttribute("TBT_SUB_CATEGORY_LIST", categorylist.ToString());
                        name.Append(bodyST_categorylist.ToString());
                        indV++; 
                        //catno = 0;
                        if (indV == grid_cols)
                        {
                            bodyST = stg_records.GetInstanceOf("row");
                            bodyST.SetAttribute("TBWDataList", name);
                            lstrecords[bodyValue] = new TBWDataList(bodyST.ToString());
                            bodyValue++;
                            indV = 0;
                            name = new StringBuilder();
                        }

                        //Build the sub heading
                        categorylist = new StringBuilder();
                        bodyST_categorylist = stg_records.GetInstanceOf("cell");
                        foreach (DataColumn dc in dsrecords.Tables[0].Columns)
                        {
                            if (dc.ColumnName.ToString().Contains("CUSTOM_NUM"))
                                if (dr[dc.ColumnName].ToString().Length > 0)
                                    bodyST_categorylist.SetAttribute(dc.ColumnName, Convert.ToDouble(dr[dc.ColumnName].ToString()));
                                else
                                    bodyST_categorylist.SetAttribute(dc.ColumnName, "0");
                            else
                                bodyST_categorylist.SetAttribute(dc.ColumnName, dr[dc.ColumnName].ToString());
                        }
                        firstval = dr[1].ToString().ToUpper();

                    }
                    //Build the Content
                    //if (catno < 6)
                    //{
                        if (dr["TBT_SHORT_DESC"].ToString().ToLower() != "not for web")
                        {
                            bodyST_list1 = stg_records.GetInstanceOf("cell1");
                            bodyST_list1.SetAttribute(dr.Table.Columns[3].ColumnName.ToString(), dr[3].ToString());
                            bodyST_list1.SetAttribute("TBT_CATEGORY_ID", dr["TBT_CATEGORY_ID"].ToString());
                            bodyST_list1.SetAttribute("TBT_CUSTOM_NUM_FIELD3", Convert.ToInt32(dr["TBT_CUSTOM_NUM_FIELD3"]).ToString());
                            bodyST_list1.SetAttribute("TBT_PARENT_CATEGORY_ID", dr["TBT_PARENT_CATEGORY_ID"].ToString());
                            categorylist.Append(bodyST_list1.ToString());
                        }
                        //catno++;
                    //}

                }
            }
        }
        if (dsrecords == null || dsrecords.Tables[0].Rows.Count == 0)
        {
            bodyST_categorylist = stg_records.GetInstanceOf("cell");
        }
        if (indV < grid_cols)
        {
            bodyST_categorylist.SetAttribute("TBT_SUB_CATEGORY_LIST", categorylist.ToString());
            name.Append(bodyST_categorylist.ToString());
            bodyST = stg_records.GetInstanceOf("row");
            bodyST.SetAttribute("TBWDataList", name);
            if (lstrecords.Length != 0)
            {
                lstrecords[bodyValue] = new TBWDataList(bodyST.ToString());
            }
            bodyValue++;
        }
        StringTemplate bodyST_main = stg_records.GetInstanceOf("main");
        bodyST_main.SetAttribute("TBWDataList", lstrecords);
        string sHtmls = bodyST_main.ToString();
        if (sHtmls.Contains("src=\"prodimages\""))
            sHtmls = sHtmls.Replace("src=\"prodimages\"", "src=\"images/noimage.gif\"");
        if (sHtmls.Contains("src=\"\""))
            sHtmls = sHtmls.Replace("src=\"\"", "src=\"images/noimage.gif\"");

        return sHtmls;
        }
        catch (Exception ex)
        {
            objErrorHandler.ErrorMsg = ex;
            objErrorHandler.CreateLog();
            return string.Empty;
        }
        //if (dsrecords.Tables[0].Rows[0][1].ToString() != null && dsrecords.Tables[0].Rows[0][1].ToString() != string.Empty)
        //{
        //    //Build the Sub heading 
        //    firstval = dsrecords.Tables[0].Rows[0][1].ToString().ToUpper();
        //    bodyST_head = stg_records.GetInstanceOf("cell_head");
        //    bodyST_head.SetAttribute("TBT_PARENT_CATEGORY_NAME", firstval);
        //    categorylist.Append(bodyST_head.ToString());

        //}
        //foreach (DataRow dr in dsrecords.Tables[0].Rows)
        //{
        //    if (dr[1].ToString() != null && dr[1].ToString() != string.Empty)
        //    {
        //        if (firstval != dr[1].ToString().ToUpper())
        //        {

        //            //Build the category 
        //            bodyST_categorylist = stg_records.GetInstanceOf("cell");
        //            bodyST_categorylist.SetAttribute("TBT_SUB_CATEGORY_LIST", categorylist.ToString());
        //            name.Append(bodyST_categorylist.ToString());
        //            indV++;
        //            if (indV == grid_cols)
        //            {
        //                bodyST = stg_records.GetInstanceOf("row");
        //                bodyST.SetAttribute("TBWDataList", name);
        //                lstrecords[bodyValue] = new TBWDataList(bodyST.ToString());
        //                bodyValue++;
        //                indV = 0;
        //                name = new StringBuilder();
        //            }

        //            //Build the sub heading
        //            categorylist = new StringBuilder();
        //            bodyST_head = stg_records.GetInstanceOf("cell_head");
        //            bodyST_head.SetAttribute("TBT_PARENT_CATEGORY_NAME", dr[1].ToString().ToUpper());
        //            categorylist.Append(bodyST_head.ToString());
        //            firstval = dr[1].ToString().ToUpper();

        //        }
        //        //Build the Content
        //        bodyST_list1 = stg_records.GetInstanceOf("cell1");
        //        bodyST_list1.SetAttribute(dr.Table.Columns[3].ColumnName.ToString(), dr[3].ToString());
        //        categorylist.Append(bodyST_list1.ToString());

        //    }
        //}      

        //if (indV < grid_cols)
        //{
        //    bodyST = stg_records.GetInstanceOf("row");
        //    bodyST.SetAttribute("TBWDataList", name);
        //    lstrecords[bodyValue] = new TBWDataList(bodyST.ToString());
        //    bodyValue++;            
        //}
        //StringTemplate bodyST_main = stg_records.GetInstanceOf("main");
        //bodyST_main.SetAttribute("TBWDataList", lstrecords);
        //return bodyST_main.ToString();
    }



    //private DataSet GetDataSet(string SQLQuery)
    //{
    //    DataSet ds = new DataSet();
    //    SqlDataAdapter da = new SqlDataAdapter(SQLQuery, conStr.ConnectionString.Substring(conStr.ConnectionString.IndexOf(";") + 1));
    //    da.Fill(ds, "generictable");
    //    return ds;
    //}
}
