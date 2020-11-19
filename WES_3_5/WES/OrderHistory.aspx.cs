
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Data.SqlClient;
using System.Data;

using System.Configuration;
using System.Timers;
using TradingBell.WebCat.CatalogDB;
using TradingBell.WebCat.Helpers;
using TradingBell.WebCat.CommonServices;

using System.Web.Services;
using System.Web.Configuration;
public partial class OrderHistory1 : System.Web.UI.Page
{
    HelperServices objHelperServices = new HelperServices();
    Security objSecurity = new Security();
    DataTable dtOrder = new DataTable();
    ErrorHandler objErrorHandler = new ErrorHandler();
    // ConnectionDB objConnectionDB = new ConnectionDB();
    OrderServices objOrderServices = new OrderServices();
    const string EnDekey = "WAGNER@PayPalPAY@dm1n@123";
    public static int tick_count = 0;
    int Userid;
    AjaxControlToolkit.ModalPopupExtender modalPop = new AjaxControlToolkit.ModalPopupExtender();


    //System.Timers.Timer Timer1;

    public static System.Web.UI.Control GetPostBackControl(System.Web.UI.Page page)
    {
        Control control = null;
        string ctrlname = page.Request.Params["__EVENTTARGET"];
        if (ctrlname != null && ctrlname != String.Empty)
        {
            control = page.FindControl(ctrlname);
        }
        // if __EVENTTARGET is null, the control is a button type and we need to 
        // iterate over the form collection to find it
        else
        {
            string ctrlStr = String.Empty;
            Control c = null;
            foreach (string ctl in page.Request.Form)
            {
                // handle ImageButton controls ...
                if (ctl.EndsWith(".x") || ctl.EndsWith(".y"))
                {
                    ctrlStr = ctl.Substring(0, ctl.Length - 2);
                    c = page.FindControl(ctrlStr);
                }
                else
                {
                    c = page.FindControl(ctl);
                }
                if (c is System.Web.UI.WebControls.Button ||
                            c is System.Web.UI.WebControls.ImageButton)
                {
                    control = c;
                    break;
                }
            }
        }
        return control;
    }
    protected void Page_Load(object sender, EventArgs e)
    {

        //Control controlThatCausedPostBack = GetPostBackControl(this);
        //if (controlThatCausedPostBack.ID == "ClickMe" && IsPostBack == true)
        //    return;
        try
        {
        if (Session["USER_NAME"] == null || Session["USER_NAME"] == "")
        {
            Response.Redirect("Login.aspx");
        }
        if (Convert.ToInt16(Session["USER_ROLE"]) == 4)
        {
            Response.Redirect("home.aspx");
        }
        if (Request.QueryString["PDFSession"] != null)
        {
            if (Request.QueryString["PDFSession"].ToString() == "true")
            {

                //string tmppath = HttpContext.Current.Request.Url.OriginalString.Replace(HttpContext.Current.Request.Url.Query, "");
                //HtmlMeta hm1 = new HtmlMeta();

                //hm1.HttpEquiv = "refresh";
                //hm1.Content = "5;" + tmppath;
                //HtmlHead head = (HtmlHead)Page.Header;
                //head.Controls.Add(hm1);


                ShowProcessSignalMessage1();
            }
            else
            {
                ShowProcessSignalMessage();
            }
        }

        if (IsPostBack)
        {

            if ((Session["tickcheck"] != null && Session["tickcheck"].ToString() != "") && (Convert.ToInt32(Session["tickcheck"]) >= 30))
            {

                Timer1.Enabled = false;
                if (Convert.ToInt32(Session["tickcheck"]) > 30)
                {
                    Session["tickcheck"] = null;
                    this.modalPop.Hide();
                    Response.Redirect("Orderhistory.aspx?PDFSession=false");
                    ShowProcessSignalMessage();
                }
                else
                {

                    Session["tickcheck"] = null;


                    Response.Redirect("Orderhistory.aspx?PDFSession=true");
                    this.modalPop.Hide();



                    //string pdffile = Session["pdffile"].ToString();
                    //string PdfFileName = Session["PdfFileName"].ToString();
                    //if (System.IO.File.Exists(pdffile))
                    //{

                    //    Session["pdffile"] = null;
                    //    Session["PdfFileName"] = null;
                    //    Response.AppendHeader("refresh", "1");
                    //    Response.AppendHeader("content-disposition", string.Format("attachment; filename={0}", "In" + PdfFileName[1] + ".pdf"));
                    //    Response.ContentType = "application/pdf";
                    //    Response.WriteFile(pdffile);
                    //    Response.End();
                    //}                   
                }
            }
        }
        Page.Title = objHelperServices.GetOptionValues("BROWSER TITLE").ToString();
        if (Session["USER_NAME"] == null)
        {
            Session["USER"] = "";
            Session["COUNT"] = "0";
            Response.Redirect("Login.aspx");
        }

        if (!Page.IsPostBack)
        {

            if (Request["Key"] != null)
            {
                tick_count = 0;
                string _Key = Server.HtmlDecode(Request["Key"].ToString());
                string DecryptedValueString = objSecurity.Decrypt(_Key, Session.SessionID);

                if (!string.IsNullOrEmpty(DecryptedValueString))
                {
                    string[] PdfFileName = DecryptedValueString.Split('|');
                    string pdffile = Server.MapPath(string.Format("~/Invoices/In{0}", PdfFileName[1] + ".pdf"));
                    Session["PdfFileName"] = PdfFileName[1].ToString();
                    Session["pdffile"] = pdffile;
                    //System.Threading.Thread.Sleep(100000);
                    if (System.IO.File.Exists(pdffile))
                    {
                        //Timer1.Enabled = false;
                        Session["tickcheck"] = null;
                        //System.Threading.Thread.Sleep(20000);
                        //ShowProcessSignalMessage();
                        Response.AppendHeader("content-disposition", string.Format("attachment; filename={0}", "In" + PdfFileName[1] + ".pdf"));
                        Response.ContentType = "application/pdf";
                        Response.WriteFile(pdffile);
                        Response.End();

                    }
                    else
                    {
                        if (PdfFileName[0] != null && PdfFileName[0].ToString().Trim() != string.Empty)
                        {
                            int cStatus = 0;
                            string cOrderNo = PdfFileName[0].ToString();
                            cStatus = objOrderServices.SentSignalInvoiceNotification(cOrderNo);
                            if (cStatus > 0)
                            {
                                if (tick_count == 0)
                                {
                                    //Response.Write("<script>alert('timer started');</script>");
                                    ShowProcessMessage();
                                    //ShowProcessSignalMessage();
                                    //Timer1.Interval = 3000;
                                    //Timer1.Elapsed += new ElapsedEventHandler(timer1_Tick);
                                    //// Only raise the event the first time Interval elapses.
                                    //Timer1.AutoReset = false;
                                    Timer1.Enabled = true;
                                    Timer1.Interval = 500;
                                    tick_count = 1;
                                }
                            }
                            else
                            {
                                ShowProcessSignalMessage();
                            }

                        }
                    }
                }
                else
                {
                    ShowProcessSignalMessage();
                }

            }

            OrderNoHiddenField.Value = "";
            FromDateHiddenField.Value = "";
            ToDateHiddenField.Value = "";
            UserHiddenField.Value = "";
            Userid = Convert.ToInt32(Session["User_id"]);
            UserList(Userid);

            CreatedUserDropDownlist.SelectedIndex = 0;
            UserHiddenField.Value = CreatedUserDropDownlist.SelectedItem.Text;
            OrderHistory();

            //if (Session["pdffile"] != null && Request["Key"] == null)
            //{

            //    string pdffile = Session["pdffile"].ToString();
            //    string PdfFileName = Session["PdfFileName"].ToString();
            //    if (System.IO.File.Exists(pdffile))
            //    {

            //        Session["pdffile"] = null;
            //        Session["PdfFileName"] = null;
            //        //Response.AppendHeader("content-disposition", string.Format("attachment; filename={0}", "In" + PdfFileName + ".pdf"));
            //        //Response.ContentType = "application/pdf";
            //        //Response.WriteFile(pdffile);
            //        //Response.Write("<script type='text/javascript'> window.open('"+pdffile+"','_blank'); </script>");
            //        //Response.End();
            //        //Response.Redirect("Orderhistory.aspx");

            //        Response.Clear();
            //        Response.AppendHeader("content-disposition", string.Format("attachment; filename={0}", "In" + PdfFileName + ".pdf"));
            //        Response.ContentType = "application/pdf";

            //        //Response.AppendHeader("Content-Disposition", "attachment; filename=" + fileInfo.Name);
            //        //Response.AppendHeader("Content-Length", fileInfo.Length.ToString());
            //        //Response.ContentType = "application/pdf";
            //        Response.Flush();
            //        Response.WriteFile(pdffile);
            //        //Response.Write("<script type='text/javaScript'> window.location.href ='Orderhistory.aspx'; </script>");



            //    }
            //}
        }
        else
        {
            OrderNoHiddenField.Value = OrderNo.Text;
            FromDateHiddenField.Value = FromdateTextBox.Text;
            ToDateHiddenField.Value = TodateTextBox.Text;
            UserHiddenField.Value = CreatedUserDropDownlist.SelectedItem.Text;
        }
        }
        catch (Exception ex)
        {
            objErrorHandler.ErrorMsg = ex;
            objErrorHandler.CreateLog();
            
        }
    }
    //protected override void OnLoad(EventArgs e)
    //{
    //    Response.Write("<script>alert('onload');</script>");
    //    //System.Threading.Thread.Sleep(6000);
    //    base.OnLoad(e);

    //    for (int i = 0; i < 6; i++)
    //    {
    //        if (System.IO.File.Exists(Session["pdffile"].ToString()))
    //        {

    //            Timer1.Enabled = false;
    //            //HidePopUpSignalMessage();
    //            i = 6;
    //            tick_count = 5;
    //            Session["tickcheck"] = tick_count;
    //            //Response.Redirect("Orderhistory.aspx");
    //            Response.AppendHeader("content-disposition", string.Format("attachment; filename={0}", "In" + Session["PdfFileName"].ToString() + " " + i + ".pdf"));
    //            Response.ContentType = "application/pdf";
    //            Response.WriteFile(Session["pdffile"].ToString());
    //            Response.End();


    //        }
    //        tick_count = tick_count + 1;
    //        Session["tickcheck"] = tick_count;
    //        System.Threading.Thread.Sleep(1000);
    //    }
    //}

    public void timer1_Tick(object sender, EventArgs e)
    {
        try
        {
        for (int i = 0; i < 2; i++)
        {
            if (System.IO.File.Exists(Session["pdffile"].ToString()))
            {
                Timer1.Enabled = false;

                i = 2;
                tick_count = 29;
                //Session["tickcheck"] = tick_count;
                //Response.AppendHeader("content-disposition", string.Format("attachment; filename={0}", "In" + Session["PdfFileName"].ToString() + " .pdf"));
                //Response.ContentType = "application/pdf";
                //Response.WriteFile(Session["pdffile"].ToString());
                //Response.End();                
            }
            tick_count = tick_count + 1;
            Session["tickcheck"] = tick_count;
            System.Threading.Thread.Sleep(1000);
        }
        }
        catch (Exception ex)
        {
            objErrorHandler.ErrorMsg = ex;
            objErrorHandler.CreateLog();
           
        }
    }


    //public void timer1_Tick(object sender, EventArgs e)
    //{

    //    // message.Attributes["style"] = "display:none;";
    //    //  messagediv.Visible = true;
    //    if (tick_count == 0)
    //    {
    //        // messagediv.Visible = false;
    //        // Response.Write("time out " + tick_count + " ...........");
    //        tick_count = tick_count + 1;
    //        //timer1.Stop();
    //        //ShowProcessSignalMessage();

    //        if (Request["Key"] != null)
    //        {

    //            string _Key = Server.HtmlDecode(Request["Key"].ToString());
    //            string DecryptedValueString = oHelper.Decrypt(_Key, Session.SessionID);

    //            if (!string.IsNullOrEmpty(DecryptedValueString))
    //            {
    //                string[] PdfFileName = DecryptedValueString.Split('|');
    //                string pdffile = Server.MapPath(string.Format("Invoices/In{0}", PdfFileName[1] + ".pdf"));
    //                //System.Threading.Thread.Sleep(100000);
    //                if (System.IO.File.Exists(pdffile))
    //                {
    //                    ProcessDiv.Visible = false;
    //                    // messagediv.Visible = false;
    //                    Timer1.Enabled = false;

    //                    Session["tickcheck"] = tick_count.ToString();
    //                    // tick_count = 0;
    //                    //timer1.Stop();
    //                    //System.Threading.Thread.Sleep(20000);
    //                    //ShowProcessSignalMessage();
    //                    Response.AppendHeader("content-disposition", string.Format("attachment; filename={0}", "In" + PdfFileName[1] + ".pdf"));
    //                    Response.ContentType = "application/pdf";
    //                    Response.WriteFile(pdffile);
    //                    Response.End();

    //                }
    //                //else
    //                //{
    //                //   // messagediv.Visible = false;
    //                //    ProcessDiv.Visible = false;
    //                //    Timer1.Enabled = false;

    //                //    if (PdfFileName[0] != null && PdfFileName[0].ToString().Trim() != string.Empty)
    //                //    {
    //                //        int cStatus = 0;
    //                //        string cOrderNo = PdfFileName[0].ToString();
    //                //        cStatus = oOrder.SentSignalInvoiceNotification(cOrderNo);
    //                //        if (cStatus >= 0)
    //                //        {

    //                //            ShowPopUpSignalMessage();
    //                //        }
    //                //        else
    //                //        {

    //                //            ShowProcessSignalMessage();
    //                //        }

    //                //    }
    //                //}
    //            }

    //        }


    //    }
    //    else if (tick_count == 3)
    //    {
    //        tick_count = tick_count + 1;
    //        //messagediv.Visible = false;
    //        ProcessDiv.Visible = false;
    //        Timer1.Enabled = false;

    //        if (Request["Key"] != null)
    //        {

    //            string _Key = Server.HtmlDecode(Request["Key"].ToString());
    //            string DecryptedValueString = oHelper.Decrypt(_Key, Session.SessionID);

    //            if (!string.IsNullOrEmpty(DecryptedValueString))
    //            {
    //                string[] PdfFileName = DecryptedValueString.Split('|');
    //                string pdffile = Server.MapPath(string.Format("Invoices/In{0}", PdfFileName[1] + ".pdf"));
    //                //System.Threading.Thread.Sleep(100000);
    //                if (System.IO.File.Exists(pdffile))
    //                {
    //                    //messagediv.Visible = false;
    //                    ProcessDiv.Visible = false;
    //                    Timer1.Enabled = false;

    //                    Session["tickcheck"] = tick_count.ToString();
    //                    // tick_count = 0;
    //                    //timer1.Stop();
    //                    //System.Threading.Thread.Sleep(20000);
    //                    //ShowProcessSignalMessage();
    //                    Response.AppendHeader("content-disposition", string.Format("attachment; filename={0}", "In" + PdfFileName[1] + ".pdf"));
    //                    Response.ContentType = "application/pdf";
    //                    Response.WriteFile(pdffile);
    //                    Response.End();

    //                }
    //                //else
    //                //{
    //                //    //messagediv.Visible = false;
    //                //    ProcessDiv.Visible = false;
    //                //    Timer1.Enabled = false;

    //                //    if (PdfFileName[0] != null && PdfFileName[0].ToString().Trim() != string.Empty)
    //                //    {
    //                //        ShowProcessSignalMessage();
    //                //        //int cStatus = 0;
    //                //        //string cOrderNo = PdfFileName[0].ToString();
    //                //        //cStatus = oOrder.SentSignalInvoiceNotification(cOrderNo);
    //                //        //if (cStatus >= 0)
    //                //        //{

    //                //        //    ShowPopUpSignalMessage();
    //                //        //}
    //                //        //else
    //                //        //{

    //                //        //    ShowProcessSignalMessage();
    //                //        //}

    //                //    }
    //                //}
    //            }
    //            //else
    //            //{
    //            //   // messagediv.Visible = false;
    //            //    ProcessDiv.Visible = false;
    //            //    Timer1.Enabled = false;
    //            //    ShowProcessSignalMessage();
    //            //}
    //        }

    //    }

    //    else
    //    {
    //        // messagediv.Visible = false;
    //        ProcessDiv.Visible = false;
    //        Timer1.Enabled = false;

    //        tick_count = tick_count + 1;
    //        if (Request["Key"] != null)
    //        {

    //            string _Key = Server.HtmlDecode(Request["Key"].ToString());
    //            string DecryptedValueString = oHelper.Decrypt(_Key, Session.SessionID);

    //            if (!string.IsNullOrEmpty(DecryptedValueString))
    //            {
    //                string[] PdfFileName = DecryptedValueString.Split('|');
    //                string pdffile = Server.MapPath(string.Format("Invoices/In{0}", PdfFileName[1] + ".pdf"));
    //                //System.Threading.Thread.Sleep(100000);
    //                if (System.IO.File.Exists(pdffile))
    //                {
    //                    ProcessDiv.Visible = false;
    //                    //messagediv.Visible = false;
    //                    Timer1.Enabled = false;

    //                    Session["tickcheck"] = tick_count.ToString();
    //                    //tick_count = 0;
    //                    //timer1.Stop();
    //                    //System.Threading.Thread.Sleep(20000);
    //                    //ShowProcessSignalMessage();
    //                    Response.AppendHeader("content-disposition", string.Format("attachment; filename={0}", "In" + PdfFileName[1] + ".pdf"));
    //                    Response.ContentType = "application/pdf";
    //                    Response.WriteFile(pdffile);
    //                    Response.End();

    //                }

    //            }

    //        }

    //    }

    //}

    //public void invoiceprocessstart()
    //{
    //    if (Request["Key"] != null)
    //    {
    //        string _Key = Server.HtmlDecode(Request["Key"].ToString());
    //        string DecryptedValueString = oHelper.Decrypt(_Key, Session.SessionID);

    //        if (!string.IsNullOrEmpty(DecryptedValueString))
    //        {
    //            string[] PdfFileName = DecryptedValueString.Split('|');
    //            string pdffile = Server.MapPath(string.Format("Invoices/In{0}", PdfFileName[1] + ".pdf"));
    //            if (System.IO.File.Exists(pdffile))
    //            {
    //                System.Windows.Forms.DialogResult result1 = System.Windows.Forms.MessageBox.Show("Please wait for a moment. Invoice will be available shortly", "View Invoice", System.Windows.Forms.MessageBoxButtons.OKCancel);
    //                if (result1 == System.Windows.Forms.DialogResult.OK)
    //                {
    //                    Response.AppendHeader("content-disposition", string.Format("attachment; filename={0}", "In" + PdfFileName[1] + ".pdf"));
    //                    Response.ContentType = "application/pdf";
    //                    Response.WriteFile(pdffile);
    //                    Response.End();
    //                }
    //            }
    //            else
    //            {
    //                if (PdfFileName[0] != null && PdfFileName[0].ToString().Trim() != string.Empty)
    //                {
    //                    int cStatus = 0;
    //                    string cOrderNo = PdfFileName[0].ToString();
    //                    cStatus = oOrder.SentSignalInvoiceNotification(cOrderNo);
    //                    if (cStatus >= 0)
    //                    {
    //                        ShowPopUpSignalMessage();
    //                    }
    //                    else
    //                    {
    //                        ShowProcessSignalMessage();
    //                    }

    //                }
    //            }
    //        }
    //        else
    //        {
    //            ShowProcessSignalMessage();
    //        }
    //    }
    //}
    public void SendInvoiceNotification(string InvoiceNo)
    {
        //oHelper.SQLString = "exec stp_reqinvoice";
        //oHelper.ExecuteSQLQuery();
    }

    private void OrderHistory()
    {
        //Order History Display    
        //Connection oConStr = new Connection();
        //SqlConnection oCon = new SqlConnection(oConStr.ConnectionString.Replace("provider=SQLOLEDB;", ""));
        string websiteid = ConfigurationManager.AppSettings["WEBSITEID"].ToString();
        ConnectionDB objConnectionDB = new ConnectionDB();
        SqlDataAdapter da = new SqlDataAdapter("STP_TBWC_PICK_GetOrderHistory", objConnectionDB.GetConnection());
        da.SelectCommand.Parameters.AddWithValue("@WEBSITE_ID", websiteid);
        if (HttpContext.Current.Session["CUSTOMER_TYPE"] != null && HttpContext.Current.Session["CUSTOMER_TYPE"].ToString() == "Retailer")
        {
            da.SelectCommand.Parameters.AddWithValue("@User_id", Userid);
        }
        
        da.SelectCommand.CommandType = CommandType.StoredProcedure;

        DataTable dt = new DataTable();
        da.Fill(dt);
        objConnectionDB.CloseConnection();
        //OrderHistoryGridView.DataSource = dt;
        //OrderHistoryGridView.DataBind();
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
        if ((Convert.ToInt32(Session["USER_ROLE"]) == 1 && Session["CUSTOMER_TYPE"].ToString() == "Retailer") || Session["CUSTOMER_TYPE"].ToString() != "Retailer")
            CreatedUserDropDownlist.Items.Insert(0, "All Users");
        
        //}
    }
 
    protected void SearchButton_Click(object sender, EventArgs e)
    {
        try
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

            //String InvoiceNo = (OrderNo.Text.Trim() == string.Empty ? null : OrderNo.Text);
            //String FromDate = (FromdateTextBox.Text.Trim() == string.Empty ? null : FromdateTextBox.Text);
            //String Todate = (TodateTextBox.Text.Trim() == string.Empty ? null : TodateTextBox.Text);
            //String Users = (CreatedUserDropDownlist.SelectedItem.Text.Trim() == string.Empty ? null : CreatedUserDropDownlist.SelectedItem.Text);

            //Order oOrder1 = new Order();
            //DataTable Dt = new DataTable();
            //Dt = oOrder1.GetFilteredOrderHistory(InvoiceNo, FromDate, Todate, Users);

            //if (Dt.Rows.Count > 0)
            //{
            //    MsgLabel.ForeColor = System.Drawing.Color.White;
            //    MsgLabel.BackColor = System.Drawing.Color.White;
            //    MsgLabel.Text = "   .";
            //    //MsgLabel.Text = "No of Records Available : " + Dt.Rows.Count;
            //}
            //else
            //{
            //    MsgLabel.ForeColor = System.Drawing.Color.Red;
            //    MsgLabel.Text = "No Records Found!";
            //}

            //SqlConnection oCon = new SqlConnection(oConStr.ConnectionString.Replace("provider=SQLOLEDB;", ""));
            //SqlDataAdapter da = new SqlDataAdapter("GetOrderHistory_Search", oCon);
            //da.SelectCommand.CommandType = CommandType.StoredProcedure;
            ////da.SelectCommand.Parameters.Clear();
            //da.SelectCommand.Parameters.AddWithValue("@InvoiceNo", InvoiceNo);
            //da.SelectCommand.Parameters.AddWithValue("@Users", Users);
            //da.SelectCommand.Parameters.AddWithValue("@Fromdate1", FromDate);
            //da.SelectCommand.Parameters.AddWithValue("@Todate1", Todate);

            //DataSet ds = new DataSet();
            //DataTable Dt = new DataTable();
            //da.Fill(ds);
            ////OrderHistoryGridView.DataSource = Dt;
            ////OrderHistoryGridView.DataBind();

            //if (Dt.Rows.Count == 0)
            //{
            //    MsgLabel.Text = "Record Not Found!";
            //}

        }
        catch (Exception ex)
        {
            MsgLabel.Text = ex.Message;
        }

    }



    protected void ResetButton_Click(object sender, EventArgs e)
    {
        OrderNoHiddenField.Value = "";
        FromDateHiddenField.Value = "";
        ToDateHiddenField.Value = "";
        CreatedUserDropDownlist.SelectedIndex = 0;
        UserHiddenField.Value = CreatedUserDropDownlist.SelectedItem.Text;
        MsgLabel.Text = "  .";
        MsgLabel.Visible = false;

        OrderNo.Text = "";
        FromdateTextBox.Text = "";
        TodateTextBox.Text = "";
        OrderHistory();
        //CreatedUserDropDownlist.Items.Insert(0, "All Users");

    }

    protected void btnContinue_Click(object sender, EventArgs e)
    {
        btnClose_Click(sender, e);

    }

    protected void btnClose_Click(object sender, EventArgs e)
    {
        HidePopUpSignalMessage();
        Response.AppendHeader("content-disposition", string.Format("attachment; filename={0}", "In" + Session["PdfFileName"].ToString() + " .pdf"));
        Response.ContentType = "application/pdf";
        Response.WriteFile(Session["pdffile"].ToString());
        Response.End();
    }
    private void ShowPopUpSignalMessage()
    {

        modalPop.ID = "popUp";
        modalPop.PopupControlID = "SignalPopupPanel";
        modalPop.BackgroundCssClass = "modalBackground";
        modalPop.TargetControlID = "btnHidden";
        modalPop.DropShadow = false;
        modalPop.CancelControlID = "btnCancel";
        this.SignalPopupPanel.Controls.Add(modalPop);

        this.modalPop.Show();
    }

    protected void btnExit_Click(object sender, EventArgs e)
    {
        ShowProcessSignalMessage();
    }

    protected void btncancel_Click(object sender, EventArgs e)
    {
        //Response.Redirect("Orderhistory.aspx");
        //this.modalPop.Hide();
    }
    private void ShowProcessSignalMessage()
    {
        modalPop.ID = "PopDiv1";
        modalPop.PopupControlID = "SignalProcessPanel";
        modalPop.BackgroundCssClass = "modalBackground";
        modalPop.TargetControlID = "btnHidden";
        modalPop.DropShadow = false;
        modalPop.CancelControlID = "ExitButton";

        this.SignalPopupPanel.Controls.Add(modalPop);
        this.modalPop.Show();
    }
    private void ShowProcessSignalMessage1()
    {
        modalPop.ID = "PopDiv";
        modalPop.PopupControlID = "SignalPopupPanel";
        modalPop.BackgroundCssClass = "modalBackground";
        modalPop.TargetControlID = "btnHidden";
        modalPop.DropShadow = false;
        modalPop.CancelControlID = "ExitButton";
        this.SignalPopupPanel.Controls.Add(modalPop);
        this.modalPop.Show();
    }
    private void ShowProcessMessage()
    {
        //modalPop.TargetControlID = "timer";
        // modalPop.PopupControlID = "timer";
        this.modalPop.ID = "popUp";
        modalPop.ID = "popUp";
        modalPop.PopupControlID = "ShowProcessPanel";
        modalPop.BackgroundCssClass = "modalBackground";
        modalPop.TargetControlID = "btnHidden";
        modalPop.DropShadow = false;
        modalPop.CancelControlID = "ExitButton";

        this.ShowProcessPanel.Controls.Add(modalPop);
        this.modalPop.Show();
        //this.message.Visible=true;
    }


    private void HidePopUpSignalMessage()
    {
        this.modalPop.Hide();
    }



    [WebMethod]
    public static string SendInvoiceSignal(string strvalue)
    {
        
        string DecryptedValueString = strvalue;
        string invno = "-1";
        OrderServices objOrderServices = new OrderServices();
        HelperServices objHelperServices = new HelperServices();
        ErrorHandler objerrorhandler=new ErrorHandler(); 
        if (!string.IsNullOrEmpty(DecryptedValueString))
        {
           // objerrorhandler.CreateLog("orderhistory"+DecryptedValueString);

            string[] PdfFileName = DecryptedValueString.Split('|');
            string pdffile = HttpContext.Current.Server.MapPath(string.Format("~/Invoices/In{0}", PdfFileName[1] + ".pdf"));
           // objerrorhandler.CreateLog("orderhistory" + pdffile);
            invno = PdfFileName[1].ToString();
            HttpContext.Current.Session["PdfFileName"] = PdfFileName[1].ToString();
            HttpContext.Current.Session["pdffile"] = pdffile;
            HttpContext.Current.Session["invno"] = PdfFileName[1].ToString();


            string INVOICE_NO_OF_TIME_TRY = System.Configuration.ConfigurationManager.AppSettings["INVOICE_NO_OF_TIME_TRY"].ToString();
            string INVOICE_WAIT_TIME = System.Configuration.ConfigurationManager.AppSettings["INVOICE_WAIT_TIME"].ToString();

            //System.Threading.Thread.Sleep(100000);
            if (System.IO.File.Exists(pdffile))
            {
                return "inv" + invno;
            }
            else
            {
                if (PdfFileName[0] != null && PdfFileName[0].ToString().Trim() != string.Empty)
                {
                    int cStatus = 0;
                    int invTry = objHelperServices.CI(INVOICE_NO_OF_TIME_TRY);
                    int invWaitTime = objHelperServices.CI(INVOICE_WAIT_TIME);
                    string cOrderNo = PdfFileName[0].ToString();
                    //cStatus = objOrderServices.SentSignalInvoiceNotification(cOrderNo);
                    if (objOrderServices.GetOrderStatus(objHelperServices.CI(cOrderNo)).ToLower() == (Enum.GetName(typeof(OrderServices.OrderStatus), OrderServices.OrderStatus.Proforma_Payment_Required)).ToLower())
                        cStatus = objOrderServices.SentSignal("0", cOrderNo, "201");
                    else
                        cStatus = objOrderServices.SentSignal("0", cOrderNo, "200");


                    if (cStatus >= 0)
                    {

                        for (int i = 0; i < invTry; i++)
                        {
                            if (System.IO.File.Exists(HttpContext.Current.Session["pdffile"].ToString()))
                            {



                                return "inv" + invno;
                            }
                            System.Threading.Thread.Sleep(invWaitTime);
                        }
                    }
                    else
                    {
                        HttpContext.Current.Session["pdffile"] = "";
                        HttpContext.Current.Session["PdfFileName"] = "";

                        return invno;
                    }

                }
            }
        }
        else
        {
            HttpContext.Current.Session["pdffile"] = "";
            HttpContext.Current.Session["PdfFileName"] = "";

            return invno;
        }
        HttpContext.Current.Session["pdffile"] = "";
        HttpContext.Current.Session["PdfFileName"] = "";
        return invno;


    }


    protected void cmdUpdateField_Click(object sender, EventArgs e)
    {

        try
        {
        if (HttpContext.Current.Session["PdfFileName"] != null && HttpContext.Current.Session["pdffile"] != "" && HttpContext.Current.Session["PdfFileName"].ToString() != "")
        {
            HttpContext.Current.Response.AppendHeader("content-disposition", string.Format("attachment; filename={0}", "In" + HttpContext.Current.Session["PdfFileName"].ToString() + " .pdf"));
            HttpContext.Current.Response.ContentType = "application/pdf";
            HttpContext.Current.Response.WriteFile(HttpContext.Current.Session["pdffile"].ToString());
            HttpContext.Current.Response.End();
        }
        else
        {
            ShowProcessSignalMessage();
        }
        }
        catch (Exception ex)
        {
            objErrorHandler.ErrorMsg = ex;
            objErrorHandler.CreateLog();
        }   
    }
    protected string  EncryptSP(string ordid)
    {
        string enc="";
        enc = objSecurity.StringEnCrypt(ordid, EnDekey);
        enc = objSecurity.StringEnCrypt(enc, EnDekey);
        enc = objSecurity.StringEnCrypt(enc, EnDekey);
        enc = objSecurity.StringEnCrypt(enc, EnDekey);
        enc = objSecurity.StringEnCrypt(enc, EnDekey);
        return  HttpUtility.UrlEncode(enc);
    }
}

