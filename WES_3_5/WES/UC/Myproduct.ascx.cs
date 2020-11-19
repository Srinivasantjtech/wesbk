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
using System.Text.RegularExpressions;
using System.Data.SqlClient;
using TradingBell.WebCat.CatalogDB;
using TradingBell.WebCat.Helpers;
using TradingBell.WebCat.TemplateRender;
using TradingBell.WebCat.CommonServices;
using System.Collections.Generic;
using System.Web.Services;
using System.Web.Configuration;
using System.Data.OleDb;
using System.IO;


namespace WES.UC
{
    public partial class Myproduct : System.Web.UI.UserControl
    {
        int i = 0;

        string user_id = "999";
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void LinkButton_Click(object sender, EventArgs e)
        {

            ////Response.ContentType = "xls";
            ////Response.AppendHeader("Content-Disposition", "attachment; filename=Sample.xls");
            ////Response.TransmitFile(Server.MapPath("~/Export/Sample.xls"));
            ////Response.End();


            try
            {

                string fileName = ConfigurationManager.AppSettings["MyFileName"].ToString();
                Response.Clear();
                Response.ClearHeaders();
                Response.ClearContent();
                //Response.AddHeader("Content-Type", "application/Excel");
                //Response.ContentType = "application/vnd.xls";
                Response.AddHeader("content-disposition", "attachment; filename=" + fileName);
                Response.AddHeader("Content-Type", "application/Excel");
                Response.ContentType = "application/vnd.xls";
                //  Response.TransmitFile(Server.MapPath("~/Export/Sample.xls"));
                Response.TransmitFile(Server.MapPath("~/Export/" + fileName));
                Response.End();
            }
            catch (Exception ex)
            {
                // objErrorHandler.ErrorMsg = ex;
            }



        }


        public void UploadButton_Click(object sender, EventArgs e)
        {
            // RegularExpressionValidator1.ErrorMessage = null;
            string fileName = Path.GetFileName(FileUploadControl.PostedFile.FileName);
            HttpContext.Current.Session["fileName"] = fileName;
            string fileExtension = Path.GetExtension(FileUploadControl.PostedFile.FileName);
            double dblMaxFileSize = Convert.ToDouble(ConfigurationManager.AppSettings["MaxFileSize"]);
            int intFileSize = FileUploadControl.PostedFile.ContentLength;  // Here the file size is obtained in bytes
            double dblFileSizeinKB = intFileSize / 1024.0; // We convert the file size into kilobytes
            int QuickOrderBoxCount = Convert.ToInt16(ConfigurationManager.AppSettings["QuickOrderBoxCount"]);


            if (fileExtension == ".csv")
            {
                ConnectCSV();
                return;
            }
            else
            {
                if ((FileUploadControl.HasFile))
                {
                    if (dblFileSizeinKB < 1024)
                    {
                        try
                        {

                            OleDbConnection conn = new OleDbConnection();
                            OleDbCommand cmd = new OleDbCommand();
                            OleDbDataAdapter da = new OleDbDataAdapter();
                            DataSet ds = new DataSet();
                            string query = null;
                            string connString = "";
                            string strFileName = Path.GetFileName(FileUploadControl.PostedFile.FileName);
                            string strFileType = System.IO.Path.GetExtension(FileUploadControl.FileName).ToString().ToLower();

                            if (strFileType == ".xls" || strFileType == ".xlsx")
                            {


                                FileUploadControl.SaveAs(Server.MapPath("~/Import/ExcelImport_" + user_id + strFileType));

                            }
                            //else if (strFileType == ".csv")
                            //{
                            //    //FileUploadControl.SaveAs(Server.MapPath("~/ExcelImport_" + user_id + strFileType ));
                            //    ConnectCSV();
                            //    return;
                            //}

                            else
                            {
                                Page.ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('Only excel files allowed');", true);
                                return;
                            }



                            string strNewPath = Server.MapPath("~/Import/ExcelImport_" + user_id + strFileType);
                            // string strNewPath1 = Server.MapPath("~/ExcelImport1_" + user_id + " + strFileType +");

                            //  if (strFileType.Trim() == ".csv")
                            //  {
                            // connString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + strNewPath + ";Extended Properties=\"Excel 8.0;HDR=Yes;IMEX=2\"";
                            //     connString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + strNewPath + ";Extended Properties=\"Excel 8.0;HDR=Yes;IMEX=2\"";
                            //   }
                            //  if (strFileType.Trim() == ".xlsx" || strFileType.Trim() == ".xls")
                            //  {
                            connString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + strNewPath + ";Extended Properties=\"Excel 12.0;HDR=Yes;IMEX=2\"";
                            //  }

                            //  if (strFileType == ".xls" || strFileType == ".xlsx")
                            //  {
                            query = "SELECT * FROM [Sheet1$]";
                            //  }
                            //if (strFileType == ".csv")
                            //{
                            //    query = "SELECT * FROM [Sheet1$]";
                            //}
                            conn = new OleDbConnection(connString);
                            if (conn.State == ConnectionState.Closed) conn.Open();
                            cmd = new OleDbCommand(query, conn);
                            da = new OleDbDataAdapter(cmd);

                            System.Data.DataTable dt = null;
                            dt = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                            string sheetname = dt.Rows[0][2].ToString();
                            if (sheetname == "Sheet1$")
                            {
                                ds = new DataSet();
                                da.Fill(ds);
                                da.Dispose();
                                conn.Close();
                                conn.Dispose();

                                int rowcount = ds.Tables[0].Rows.Count;
                                int rowcountnew = ds.Tables.Count;
                                if (rowcount <= QuickOrderBoxCount)
                                {
                                    for (i = 0; i <= ds.Tables[0].Rows.Count - 1; i++)
                                    {
                                        string sqlwordcol1 = ds.Tables[0].Rows[i].ItemArray[0].ToString().Trim();
                                        string sqlwordcol2 = ds.Tables[0].Rows[i].ItemArray[1].ToString().Trim();
                                        string containword = sqlwordcol1;
                                        if (sqlwordcol1.Contains("select") == true || sqlwordcol1.Contains("delete") == true || sqlwordcol1.Contains("insert") == true || sqlwordcol1.Contains("update") == true || sqlwordcol1 == "select *" || sqlwordcol1 == "select*" || sqlwordcol1 == "select * from" || sqlwordcol1 == "delete  from" || sqlwordcol1 == "insert into")
                                        {
                                            Page.ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert(' Excel File Not Valid Format.');", true);
                                            ds = null;
                                            HttpContext.Current.Session["fileData"] = null;
                                            return;
                                        }
                                        else if (sqlwordcol2.Contains("select") == true || sqlwordcol2.Contains("delete") == true || sqlwordcol2.Contains("insert") == true || sqlwordcol2.Contains("update") == true || sqlwordcol2 == "select *" || sqlwordcol2 == "select*" || sqlwordcol2 == "select * from" || sqlwordcol2 == "delete  from" || sqlwordcol2 == "insert into")
                                        {
                                            Page.ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert(' Excel File Not Valid Format.');", true);
                                            ds = null;
                                            HttpContext.Current.Session["fileData"] = null;
                                            return;
                                        }
                                        else
                                        {
                                            if (ds != null && ds.Tables.Count > 0)
                                                HttpContext.Current.Session["fileData"] = ds;
                                            else
                                                HttpContext.Current.Session["fileData"] = null;
                                        }
                                    }
                                }
                                else
                                {
                                    //StatusLabel.Text = "Maximun Excel File Allowed 30 Rows.";
                                    //StatusLabel.ForeColor = System.Drawing.Color.Red;
                                    //StatusLabel.Visible = true;
                                    Page.ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('Maximun Excel File Allowed 99 Rows.');", true);
                                    return;
                                }
                            }
                            else
                            {
                                //StatusLabel.Text = "First Sheet Name must contain Sheet1";
                                //StatusLabel.ForeColor = System.Drawing.Color.Red;
                                //StatusLabel.Visible = true;
                                Page.ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('First Sheet Name must contain Sheet1.');", true);
                                ds = null;
                                HttpContext.Current.Session["fileData"] = null;
                                return;
                            }




                            da.Dispose();
                            conn.Close();
                            conn.Dispose();
                        }
                        catch (System.NullReferenceException ex)
                        {
                            StatusLabel.Text = "Error: " + ex.Message;
                            StatusLabel.ForeColor = System.Drawing.Color.Red;
                            StatusLabel.Visible = true;
                        }
                    }

                    else
                    {
                        StatusLabel.Text = "Maximun file size allowed 1024KB";
                        StatusLabel.ForeColor = System.Drawing.Color.Red;
                        StatusLabel.Visible = true;
                    }


                }
                else
                {
                    StatusLabel.Text = "Please select an excel file first";
                    StatusLabel.ForeColor = System.Drawing.Color.Red;
                    StatusLabel.Visible = true;
                }

            }

        }

        private void ConnectCSV()
        {
            string fileName = Path.GetFileName(FileUploadControl.PostedFile.FileName);
            HttpContext.Current.Session["fileName"] = fileName;
            string fileExtension = Path.GetExtension(FileUploadControl.PostedFile.FileName);
            double dblMaxFileSize = Convert.ToDouble(ConfigurationManager.AppSettings["MaxFileSize"]);
            int intFileSize = FileUploadControl.PostedFile.ContentLength;  // Here the file size is obtained in bytes
            double dblFileSizeinKB = intFileSize / 1024.0; // We convert the file size into kilobytes
            int QuickOrderBoxCount = Convert.ToInt16(ConfigurationManager.AppSettings["QuickOrderBoxCount"]);

            try
            {
                string strFileType = System.IO.Path.GetExtension(FileUploadControl.FileName).ToString().ToLower();
                string target = Server.MapPath("~/Import/ExcelImport_" + user_id + strFileType);







                if (FileUploadControl.HasFile)
                {

                    FileUploadControl.SaveAs(target);

                    ////string connString = string.Format("Provider=Microsoft.Jet.OLEDB.4.0;Data Source={0};Extended Properties=Text;", System.IO.Path.GetDirectoryName(target + "\\" + FileUploadControl.FileName));
                    // string connString = string.Format("Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties=Text;", System.IO.Path.GetDirectoryName(target + "\\" + FileUploadControl.FileName));
                    // string cmdString = string.Format("SELECT * FROM {0}", System.IO.Path.GetFileName(target + "\\" + FileUploadControl.FileName));
                    string connString = string.Format("Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties=Text;", System.IO.Path.GetDirectoryName(target));
                    string cmdString = string.Format("SELECT * FROM {0}", System.IO.Path.GetFileName(target));
                    OleDbDataAdapter dataAdapter = new OleDbDataAdapter(cmdString, connString);

                    DataSet ds = new DataSet();

                    dataAdapter.Fill(ds);
                    dataAdapter.Dispose();
                    int rowcount = ds.Tables[0].Rows.Count;
                    int rowcountnew = ds.Tables.Count;
                    if (rowcount <= QuickOrderBoxCount)
                    {
                        for (i = 0; i <= ds.Tables[0].Rows.Count - 1; i++)
                        {

                            string sqlwordcol1 = ds.Tables[0].Rows[i].ItemArray[0].ToString().Trim();
                            string sqlwordcol2 = ds.Tables[0].Rows[i].ItemArray[1].ToString().Trim();
                            if (sqlwordcol1.Contains("select") == true || sqlwordcol1.Contains("delete") == true || sqlwordcol1.Contains("insert") == true || sqlwordcol1.Contains("update") == true || sqlwordcol1 == "select *" || sqlwordcol1 == "select*" || sqlwordcol1 == "select * from" || sqlwordcol1 == "delete  from" || sqlwordcol1 == "insert into")
                            {
                                Page.ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert(' Excel File Not Valid Format.');", true);
                                ds = null;
                                HttpContext.Current.Session["fileData"] = null;
                                return;
                            }
                            else if (sqlwordcol2.Contains("select") == true || sqlwordcol2.Contains("delete") == true || sqlwordcol2.Contains("insert") == true || sqlwordcol2.Contains("update") == true || sqlwordcol2 == "select *" || sqlwordcol2 == "select*" || sqlwordcol2 == "select * from" || sqlwordcol2 == "delete  from" || sqlwordcol2 == "insert into")
                            {
                                Page.ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert(' Excel File Not Valid Format.');", true);
                                ds = null;
                                HttpContext.Current.Session["fileData"] = null;
                                return;
                            }
                            else
                            {
                                if (ds != null && ds.Tables.Count > 0)
                                    HttpContext.Current.Session["fileData"] = ds;
                                else
                                    HttpContext.Current.Session["fileData"] = null;
                            }
                        }

                    }
                    else
                    {
                        Page.ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('Maximun Excel File Allowed 99 Rows.');", true);
                        return;
                    }
                }
            }
            catch (Exception e)
            {

            }


        }

    }
}