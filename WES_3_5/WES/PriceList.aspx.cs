using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using TradingBell.WebCat.CatalogDB;
using TradingBell.WebCat.CommonServices;
using TradingBell.WebCat.Helpers;
namespace WES
{
    public partial class PriceList : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            ErrorHandler objErrorHandler = new ErrorHandler();
            objErrorHandler.CreateLog("start pricelist");  
            string esip = System.Configuration.ConfigurationManager.AppSettings["ESIP"].ToString();  
             //   http://14.202.163.38:9100
            string url = esip+"/EasyAsk/apps/Advisor.jsp?disp=json&oneshot=1&dct=webcat&indexed=1&ResultsPerPage=25000&defsortcols=prod id,true&subcategories=True&returnskus=True&eap_PriceCode=4&RequestAction=advisor&CatPath=AllProducts%252f%252f%252f%252fWESAUSTRALASIA%252f%252f%252f%252fCellular%2bAccessories&RequestData=CA_BreadcrumbSelect";
          //  RemoteResults res = new RemoteResults();
            DataSet dsAdvisor = new DataSet();
              HelperDB objhelperDb = new HelperDB();

            var httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
            httpWebRequest.ContentType = "application/json; charset=utf-8";
            httpWebRequest.Method = "POST";



            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            string restr = "";
            using (var streamReader = new System.IO.StreamReader(httpResponse.GetResponseStream()))
            {
                XmlDocument xd = new XmlDocument();
                restr = streamReader.ReadToEnd();
                //ErrorHandler objErrorhandler = new ErrorHandler();
                //objErrorhandler.CreateLog(restr);
                restr = "{ \"rootNode\": {" + restr.Trim().TrimStart('{').TrimEnd('}') + "} }";


                xd = (XmlDocument)JsonConvert.DeserializeXmlNode(restr);
                dsAdvisor.ReadXml(new XmlNodeReader(xd));
            }
            if (dsAdvisor != null)
            {
                DataTable dtitems = dsAdvisor.Tables["items"];
                DataTable exceldt=new DataTable() ;
               string  userid=string.Empty;
         if (HttpContext.Current.Session["USER_ID"] != null)
             userid = HttpContext.Current.Session["USER_ID"].ToString();
         if (userid == string.Empty || userid == "999")
             userid = "0";

        

                exceldt.Columns.Add("Code");
                exceldt.Columns.Add("Description");
                exceldt.Columns.Add("Buy_Qty");
                exceldt.Columns.Add("Cost_Ex_GST");
                exceldt.Columns.Add("Buy_Qty_2");
                exceldt.Columns.Add("Cost_Ex_GST_2");
                exceldt.Columns.Add("Buy_Qty_3");
                exceldt.Columns.Add("Cost_Ex_GST_3");
                exceldt.Columns.Add("Stock");
                for (int i = 0; i < dtitems.Rows.Count; i++)
                {

                    DataRow dr =exceldt.NewRow();
                    dr["Code"]=dtitems.Rows[i]["prod_code"].ToString() ;
                    dr["Description"]=dtitems.Rows[i]["family_name"].ToString() ;
                 //   dr["Buy_Qty"]="1";
string strpid=dtitems.Rows[i]["product_id"].ToString() ;
                  //DataSet    tmpDs = objhelperDb.GetProductPriceEA("", strpid,userid);
                  //if (tmpDs != null && tmpDs.Tables[0].Rows.Count>0    )
                  //{
                  //    dr["Cost_Ex_GST"] = tmpDs.Tables[0].Rows[0][1].ToString();
                  //}
                  //else
                  //{
                  //    dr["Cost_Ex_GST"] = 0;
                  
                  //}
                   DataSet   dsPriceTableAll = objhelperDb.GetProductPriceTableAll(strpid, Convert.ToInt32(userid));
                   if (dsPriceTableAll != null && dsPriceTableAll.Tables[0].Rows.Count>0)
                   {
                       if (dsPriceTableAll.Tables[0].Rows.Count > 2)
                       {
                           dr["Buy_Qty"] = dsPriceTableAll.Tables[0].Rows[0][1].ToString();
                           dr["Cost_Ex_GST"] = dsPriceTableAll.Tables[0].Rows[0][3].ToString();
                           dr["Buy_Qty_2"] = dsPriceTableAll.Tables[0].Rows[1][1].ToString();
                           dr["Cost_Ex_GST_2"] = dsPriceTableAll.Tables[0].Rows[1][3].ToString();
                           dr["Buy_Qty_3"] = dsPriceTableAll.Tables[0].Rows[2][1].ToString();
                           dr["Cost_Ex_GST_3"] = dsPriceTableAll.Tables[0].Rows[2][3].ToString();
                       }
                       else if (dsPriceTableAll.Tables[0].Rows.Count == 2)
                       {
                           dr["Buy_Qty"] = dsPriceTableAll.Tables[0].Rows[0][1].ToString();
                           dr["Cost_Ex_GST"] = dsPriceTableAll.Tables[0].Rows[0][3].ToString();
                           dr["Buy_Qty_2"] = dsPriceTableAll.Tables[0].Rows[1][0].ToString();
                           dr["Cost_Ex_GST_2"] = dsPriceTableAll.Tables[0].Rows[1][2].ToString();
                       }
                       else
                       {
                           dr["Buy_Qty"] = dsPriceTableAll.Tables[0].Rows[0][1].ToString();
                           dr["Cost_Ex_GST"] = dsPriceTableAll.Tables[0].Rows[0][3].ToString();
                       }
                   }
                    string stockstatus= dtitems.Rows[i]["prod_stk_status_dsc"].ToString();
                    // dr["Stock"] = dtitems.Rows[i]["prod_stk_status_dsc"].ToString();
                    string stockflag= dtitems.Rows[i]["PROD_STOCK_FLAG"].ToString();
                // string   PROD_LEGISTATED_STATE= dtitems.Rows[i]["PROD_LEGISTATED_STATE"].ToString();
                    if(stockstatus=="IN_STOCK")
                    {
                        DataSet tmpDs = objhelperDb.GetProductPriceEA_SOH(strpid);
                        if (tmpDs != null && tmpDs.Tables[0].Rows.Count > 0)
                        {
                            dr["Stock"] = tmpDs.Tables[0].Rows[0]["soh"].ToString();
                        }
                       
                    }
                   
                    else
                    {
                        dr["Stock"] = dtitems.Rows[i]["prod_stk_status_dsc"].ToString();
                    }
                    if (stockflag != "-2" && stockstatus != "DISCONTINUED NO LONGER AVAILABLE")
                    {
                        exceldt.Rows.Add(dr);
                    }
                }
                DataSet ds = new DataSet();
                ds.Tables.Add(exceldt);  
           
                string UploadFolder = System.Configuration.ConfigurationManager.AppSettings["ReourceDocumentUploadDir"].ToString();
                ExportToExcel(ds, UploadFolder + "\\" + "WES0830.xls");
                ExportToExcel(ds, UploadFolder + "\\" + "WES0830.csv");
                objErrorHandler.CreateLog("end pricelist");  
            //    ExporttoExcel(exceldt);
            }
        }
        private void ExportToExcel(DataSet table, string filePath)
        {

            int tablecount = table.Tables.Count;
            StreamWriter sw = new StreamWriter(filePath, false);
            sw.Write(@"<!DOCTYPE HTML PUBLIC ""-//W3C//DTD HTML 4.0 Transitional//EN"">");
            sw.Write("<font style='font-size:10.0pt; font-family:Calibri;'>");

            for (int k = 0; k < tablecount; k++)
            {


                sw.Write("<BR><BR><BR>");
                sw.Write("<Table border='1' bgColor='#ffffff' borderColor='#000000' cellSpacing='0' cellPadding='0' style='font-size:10.0pt; font-family:Calibri; background:'#1E90FF'> <TR>");


                int columnscount = table.Tables[k].Columns.Count;

                for (int j = 0; j < columnscount; j++)
                {
                    sw.Write("<Td bgColor='#87CEFA'>");
                    sw.Write("");
                    //sw.Write(table.Columns[j].ToString());
                    sw.Write(table.Tables[k].Columns[j].ToString());

                    sw.Write("");
                    sw.Write("</Td>");
                }
                sw.Write("</TR>");
                foreach (DataRow row in table.Tables[k].Rows)
                {
                    sw.Write("<TR>");
                    for (int i = 0; i < table.Tables[k].Columns.Count; i++)
                    {
                        sw.Write("<Td>");
                        sw.Write(row[i].ToString());
                        sw.Write("</Td>");
                    }
                    sw.Write("</TR>");
                }
                sw.Write("</Table>");
                //sw.Write("<BR><BR><BR><BR>");
                //sw.Write("\n");
                //sw.Write(string.Format("Line1{0}Line2{0}", Environment.NewLine));


                sw.Write("</font>");

            }
            sw.Close();
        }
        private void ExporttoExcel(DataTable table,string filepath)
        {
            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.ClearContent();
            HttpContext.Current.Response.ClearHeaders();
            HttpContext.Current.Response.Buffer = true;
            HttpContext.Current.Response.ContentType = "application/ms-excel";
            HttpContext.Current.Response.Write(@"<!DOCTYPE HTML PUBLIC ""-//W3C//DTD HTML 4.0 Transitional//EN"">");
            HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment;filename=Reports.xls");
            HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment;filename=Reports.xls");

            HttpContext.Current.Response.Charset = "utf-8";
            HttpContext.Current.Response.ContentEncoding = System.Text.Encoding.GetEncoding("windows-1250");
            //sets font
            HttpContext.Current.Response.Write("<font style='font-size:10.0pt; font-family:Calibri;'>");
            HttpContext.Current.Response.Write("<BR><BR><BR>");
            //sets the table border, cell spacing, border color, font of the text, background, foreground, font height
            HttpContext.Current.Response.Write("<Table border='1' bgColor='#ffffff' " +
              "borderColor='#000000' cellSpacing='0' cellPadding='0' " +
              "style='font-size:10.0pt; font-family:Calibri; background:white;'> <TR>");
            //am getting my grid's column headers
            int columnscount = table.Columns.Count;

            for (int j = 0; j < columnscount; j++)
            {      //write in new column
                HttpContext.Current.Response.Write("<Td>");
                //Get column headers  and make it as bold in excel columns
                HttpContext.Current.Response.Write("<B>");
                HttpContext.Current.Response.Write(table.Columns[j].ToString());
                HttpContext.Current.Response.Write("</B>");
                HttpContext.Current.Response.Write("</Td>");
            }
            HttpContext.Current.Response.Write("</TR>");
            foreach (DataRow row in table.Rows)
            {//write in new row
                HttpContext.Current.Response.Write("<TR>");
                for (int i = 0; i < table.Columns.Count; i++)
                {
                    HttpContext.Current.Response.Write("<Td>");
                    HttpContext.Current.Response.Write(row[i].ToString());
                    HttpContext.Current.Response.Write("</Td>");
                }

                HttpContext.Current.Response.Write("</TR>");
            }
            HttpContext.Current.Response.Write("</Table>");
            HttpContext.Current.Response.Write("</font>");
            HttpContext.Current.Response.Flush();
            HttpContext.Current.Response.End();
        }
    }
}