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
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using StringTemplate = Antlr3.ST.StringTemplate;
using StringTemplateGroup = Antlr3.ST.StringTemplateGroup;
using TradingBell.WebCat.CatalogDB;
using TradingBell.WebCat.Helpers;
using TradingBell.WebCat.TemplateRender;
using TradingBell.WebCat.CommonServices;

public partial class UC_mfgbyalpha : System.Web.UI.UserControl
{
    ErrorHandler objErrorHandler = new ErrorHandler();
    ConnectionDB objConnectionDB = new ConnectionDB();
    HelperDB objHelperDB = new HelperDB();
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    public string ST_MFGbyalpha()
    {
        HelperServices objHelperServices = new HelperServices();
        return (Mfgbyalpha_RenderHTML("MANUFACTURERBYALPHA", Server.MapPath(objHelperServices.GetOptionValues("SHOPPTING TEMPLATE PATH").ToString())));
    }

    public string Mfgbyalpha_RenderHTML(string Package, string SkinRootPath)
    {
        try
        {
        string skin_container = null;
        int grid_cols = 0;
        int grid_rows = 0;
        string skin_sql_container = null;
        string skin_sql_param_container = null;
        string skin_records = null;
        StringTemplateGroup stg_records = null;
        StringTemplate bodyST = null;
        StringTemplate bodyST_alphalist = null;
        StringTemplate bodyST_head = null;
        StringTemplate bodyST_list1 = null;
        TBWDataList[] lstrecords = new TBWDataList[0];
        List<string> name = new List<string>();
        System.Text.StringBuilder alphalist = new StringBuilder();
        string firstval = null;
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
        lstrecords = new TBWDataList[dsrecords.Tables[0].Rows.Count * 2];
        int indV = 0;
        if (dsrecords.Tables[0].Rows[0][0].ToString() != null && dsrecords.Tables[0].Rows[0][0].ToString() != string.Empty)
        {
            //Build the alpha sub heading
            firstval = dsrecords.Tables[0].Rows[0][0].ToString().Substring(0, 1).ToUpper();
            bodyST_head = stg_records.GetInstanceOf("cell1_head");
            bodyST_head.SetAttribute("TBT_HEADING", firstval);
            name.Add(bodyST_head.ToString());
            indV++;
            //Build the alpha listing
            bodyST_alphalist = stg_records.GetInstanceOf("cell");
            bodyST_alphalist.SetAttribute("TBT_ALPHA", firstval);
            alphalist.Append(bodyST_alphalist.ToString());
        }
        foreach (DataRow dr in dsrecords.Tables[0].Rows)
        {
            if (dr[0].ToString() != null && dr[0].ToString() != string.Empty)
            {
                if (firstval != dr[0].ToString().Substring(0, 1).ToUpper())
                {
                    //Build the alpha sub heading
                    bodyST_head = stg_records.GetInstanceOf("cell1_head");
                    bodyST_head.SetAttribute("TBT_HEADING", dr[0].ToString().Substring(0, 1).ToUpper());
                    firstval = dr[0].ToString().Substring(0, 1).ToUpper();
                    name.Add(bodyST_head.ToString());
                    indV++;
                    //Build the alpha listing
                    bodyST_alphalist = stg_records.GetInstanceOf("cell");
                    bodyST_alphalist.SetAttribute("TBT_ALPHA", dr[0].ToString().Substring(0, 1).ToUpper());
                    alphalist.Append(bodyST_alphalist.ToString());
                }
                //Build the Content
                bodyST_list1 = stg_records.GetInstanceOf("cell1");
                bodyST_list1.SetAttribute(dr.Table.Columns[0].ColumnName.ToString(), dr[0].ToString());
                name.Add(bodyST_list1.ToString());
                indV++;
            }
        }

        string nameVal = "";
        int indValue = 0; int bodyValue = 0;
        int cols = name.Count / grid_cols;
        int colsRemainder = name.Count % grid_cols; int rowval = 0;
        int extra = 0;
        if (colsRemainder > 0)
            extra = 1;
        for (int icol = 0; icol < grid_cols; icol++)
        {
            if (indV - 1 >= indValue)
            {
                if (name.Count < grid_cols && colsRemainder > icol)
                {
                    nameVal = nameVal + name[indValue + cols * icol];
                }
                else if (cols * grid_cols <= name.Count && (name.Count > (rowval + icol)))
                {
                    if (icol > 0 && indV > (extra * 2 + indValue + cols * icol))
                        nameVal = nameVal + name[extra * 2 + indValue + cols * icol];
                    else
                        nameVal = nameVal + name[indValue + cols * icol];
                }
                if (icol + 1 == grid_cols)
                {
                    bodyST = stg_records.GetInstanceOf("row");
                    bodyST.SetAttribute("TBWDataList", nameVal);
                    lstrecords[bodyValue] = new TBWDataList(bodyST.ToString());
                    bodyValue++; indValue++;
                    nameVal = ""; rowval = rowval + grid_cols;
                    if (rowval < indV)
                        icol = -1;
                }
            }
        }
        if (name.Count < grid_cols)
        {
            bodyST = stg_records.GetInstanceOf("row");
            bodyST.SetAttribute("TBWDataList", nameVal);
            lstrecords[bodyValue] = new TBWDataList(bodyST.ToString());
            bodyValue++;
            nameVal = "";
        }

        StringTemplate bodyST_main = stg_records.GetInstanceOf("main");
        bodyST_main.SetAttribute("COLSPAN", grid_cols.ToString());
        bodyST_main.SetAttribute("TBT_ALPHA_LIST", alphalist.ToString());
        bodyST_main.SetAttribute("TBWDataList", lstrecords);
        return (bodyST_main.ToString());
       
    }
 
        
        catch (Exception e)
        {
            objErrorHandler.ErrorMsg = e;
            objErrorHandler.CreateLog();
            return string.Empty;
        }
    //private DataSet GetDataSet(string SQLQuery)
    //{
    //    DataSet ds = new DataSet();
    //    SqlDataAdapter da = new SqlDataAdapter(SQLQuery, conStr.ConnectionString.Substring(conStr.ConnectionString.IndexOf(";") + 1));
    //    da.Fill(ds, "generictable");
    //    return ds;
    //}
}
}
