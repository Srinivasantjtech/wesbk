using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TradingBell.WebCat.CatalogDB;
using TradingBell.WebCat.Helpers;
using TradingBell.WebCat.CommonServices;
public partial class MultiUserSetup : System.Web.UI.Page
{
    public bool EditMode = false;
    public String Test;
    ErrorHandler objErrorHandler= new ErrorHandler();
    HelperDB objHelperDB  =new HelperDB();
    Security objSecurity = new Security();
    ConnectionDB objConnectionDB = new ConnectionDB();

    HelperServices objHelperServices = new HelperServices();
    AjaxControlToolkit.ModalPopupExtender modalPop = new AjaxControlToolkit.ModalPopupExtender();

    UserServices objUserServices = new UserServices();
    UserServices.UserInfo oUserinfo = new UserServices.UserInfo();
    String EditUser_id;
    string Newpassword = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
        Page.Title = objHelperServices.GetOptionValues("BROWSER TITLE").ToString();
        //EditUser_id = Request.QueryString["ACT"];


        if (Request["USERID"] != null)
        {
            if (string.IsNullOrEmpty(Request["ACT"]) || Request["ACT"].ToString().Equals("EDIT"))
            {
                EditMode = true;
            }
        }
        else
        {
            if (!IsPostBack)
                GetUserLoginID();
        }
        if (!IsPostBack)
        {
           

            if (!IsAdminUser())
                GetFillUser(System.Convert.ToInt32(Session["USER_ID"]));

            if (Request["USERID"] != null)
            {
                if (string.IsNullOrEmpty(Request["ACT"]) || Request["ACT"].ToString().Equals("EDIT"))
                {
                    int userid = System.Convert.ToInt32(Request["USERID"]);
                    GetFillUser(userid);
                    EditMode = true;
                    radSA_CheckedChanged(sender, e);
                }
                else if (Request["ACT"].ToString().Equals("REMOVE"))
                {
                    string UserRoleDescription = "";
                    int userid = System.Convert.ToInt32(Request["USERID"]);

                    oUserinfo = objUserServices.GetUserInfo(userid);
                    int userrole = System.Convert.ToInt16(oUserinfo.USERROLE);  //Session["USER_ROLE"]
                    string username = oUserinfo.Contact;  // Request.QueryString["Contact"].ToString();

                    switch (userrole)
                    {
                        case 1:
                            UserRoleDescription = "Admin";
                            break;
                        case 2:
                            UserRoleDescription = "Submit/Approve";
                            break;
                        case 3:
                            UserRoleDescription = "Create Only";
                            break;
                        case 4:
                            UserRoleDescription = "Browse Only";
                            break;
                    }

                    Page.ClientScript.RegisterStartupScript(this.GetType(), "alert", "DeleteConfirm(" + userid + ",'" + username + "'," + userrole + ",'" + UserRoleDescription + "','" + oUserinfo.LoginName + "');", true);
                }
                else if (Request["ACT"].ToString().Equals("RST"))
                {
                    int userid = System.Convert.ToInt32(Request["USERID"]);
                    ResetPassword(userid);
                }

                if (Request.QueryString["DELETEACTION"] != null && Request.QueryString["DELETEACTION"].ToString().Equals("SUCCESS"))
                {
                    DeleteUser(System.Convert.ToInt32(Request.QueryString["USERID"]));
                }
            }
            // GetUserLoginID();
        }
        else
        {
            ViewState["txtPW"] = txtPass.Text;
            txtPass.Attributes.Add("value", ViewState["txtPW"].ToString());

            ViewState["txtPWConfirm"] = txtCPass.Text;
            txtCPass.Attributes.Add("value", ViewState["txtPW"].ToString());
        }

        if (radBO.Checked)
        {
            chkDrop.Checked = false;
            chkDrop.Enabled = false;
        }

        if (radAO.Checked==true || radCO.Checked==true)
        {
            chkDrop.Enabled = true;
            
        }

        IsDropShipment();

        MessageLabel.Text = "&nbsp;";
        MessageLabel.Visible = false;

        HidePopUpMessage();
        }
        catch (Exception ex)
        {
            objErrorHandler.ErrorMsg = ex;
            objErrorHandler.CreateLog();
        }
    }

    public void GetUserLoginID()
    {
        try
        {
            //Auto Generate UserID 
            DataSet oDs = new DataSet();
            HelperServices objHelperServices = new HelperServices();
            String Uid = "";
            string websiteid = System.Configuration.ConfigurationManager.AppSettings["WEBSITEID"].ToString();
            //Jtech
            //ConnectionDB oConStr = new ConnectionDB();
            //SqlConnection oCon = new SqlConnection(oConStr.ConnectionString.Replace("provider=SQLOLEDB;", ""));
            //SqlDataAdapter oDa = new SqlDataAdapter("SELECT top 1 * FROM TBWC_COMPANY_BUYERS WHERE COMPANY_ID = (SELECT COMPANY_ID FROM TBWC_COMPANY_BUYERS WHERE WEBSITE_ID = @websiteid AND USER_ID = @userid) AND USER_STATUS <> 5 ORDER BY Created_date DESC", oCon);
            //oDa.SelectCommand.Parameters.Clear();
            //oDa.SelectCommand.Parameters.AddWithValue("@websiteid", websiteid);
            //oDa.SelectCommand.Parameters.AddWithValue("@userid", Session["USER_ID"]);
            //DataSet oDs = new DataSet();
            //oDa.Fill(oDs, "Users");
            //String Uid = oDs.Tables[0].Rows[0][3].ToString();
            //Jtech

            oDs = (DataSet)objHelperDB.GetGenericPageDataDB("", websiteid, Session["USER_ID"].ToString(), "GET_MULTIUSER_USER_LOGIN", HelperDB.ReturnType.RTDataSet);
           // objErrorHandler.CreateLog(websiteid + "user_id" + Session["USER_ID"].ToString() + "GET_MULTIUSER_USER_LOGIN");   
            if (oDs!=null)
            {
                 Uid = oDs.Tables[0].Rows[0]["LOGIN_NAME"].ToString();
            }
            //int uid = Test.Length;
            //String UidSpec = Test.Substring(Test.IndexOf('-') + 1, Test.IndexOf('-'));
            //int Uid = Convert.ToInt32(UidSpec) + 1;
            //String Nid = Test.Substring(0, Test.IndexOf('-') + 1) + Uid;

            //AutoGenerate ID Increment
            string[] UidValue = Uid.Split('-');
            string UidValue1 = UidValue[0];
            string UidValue2 = UidValue[1];
            int IncValue = Convert.ToInt32(UidValue2) + 1;
            txtLogin.Text = UidValue1 + '-' + IncValue;
        }
        catch (Exception ex)
        {
            objErrorHandler.ErrorMsg = ex;
            objErrorHandler.CreateLog(); 
        }
    }

    public string GetUsers()
    {

        try
        {
        HelperServices objHelperServices = new HelperServices();
        string websiteid = System.Configuration.ConfigurationManager.AppSettings["WEBSITEID"].ToString();
        //ConnectionDB oConStr = new ConnectionDB();
        //SqlConnection oCon = new SqlConnection(oConStr.ConnectionString.Replace("provider=SQLOLEDB;", ""));
        //SqlDataAdapter oDa = new SqlDataAdapter("SELECT * FROM TBWC_COMPANY_BUYERS WHERE COMPANY_ID = (SELECT COMPANY_ID FROM TBWC_COMPANY_BUYERS WHERE WEBSITE_ID = @websiteid AND USER_ID = @userid) AND USER_STATUS <> 5", oCon);
        //SqlDataAdapter oDa = new SqlDataAdapter("SELECT COMPANY_ID,USER_ID,WEBSITE_ID,LOGIN_NAME,PASSWORD,CONTACT,PHONE,EMAILADDR,CASE USER_ROLE WHEN 1 THEN 'Admin' WHEN 2 THEN 'Submit/Approve' WHEN 3 THEN 'Create Only' WHEN 4 THEN 'Browse Only'  END USER_ROLE ,NOTIFYORD,NOTIFYACT,NOTIFYNEWS,USER_STATUS,USER_ONLINE,CREATED_DATE,CREATED_USER,MODIFIED_DATE,MODIFIED_USER,ORDDRPSHP FROM TBWC_COMPANY_BUYERS WHERE COMPANY_ID = (SELECT COMPANY_ID FROM TBWC_COMPANY_BUYERS WHERE WEBSITE_ID = @websiteid AND USER_ID = @userid) AND USER_STATUS <> 5", oCon);
        //oDa.SelectCommand.Parameters.Clear();
        //oDa.SelectCommand.Parameters.AddWithValue("@websiteid", websiteid);
        //oDa.SelectCommand.Parameters.AddWithValue("@userid", Session["USER_ID"]);
        DataSet oDs = new DataSet();
        //oDa.Fill(oDs, "Users");
        oDs = (DataSet)objHelperDB.GetGenericPageDataDB("", websiteid, Session["USER_ID"].ToString(), "GET_MULTIUSER_GET_USER", HelperDB.ReturnType.RTDataSet);
      
        string oHtmlStr = "";
        bool oddRow = true;
        string UserType;
        if (oDs != null)
        {
            oDs.Tables[0].TableName = "Users";  
            int iCtr = 1;
            foreach (DataRow oDr in oDs.Tables["Users"].Rows)
            {
                //string UserType = System.Convert.ToInt32(oDr["USER_ROLE"]) > 1 ? "User" : "Admin";                
                string color = oddRow ? "#ffffff" : "#ccddff";
                oddRow = !oddRow;
                oHtmlStr += string.Format("<tr><td style=\"background-color:{6}\" height=\"21\" align=\"center\">{0}</td><td style=\"background-color:{6}\" align=\"center\">{1}</td><td style=\"background-color:{6}\" align=\"center\">{2}</td><td style=\"background-color:{6}\" align=\"center\">{3}</td><td style=\"background-color:{6}\" align=\"center\">{4}</td><td valign=\"middle\" style=\"background-color:{6};\" align=\"center\"><a href=\"multiusersetup.aspx?ACT=EDIT&USERID={5}\" style=\"color:#0000ff;\">Edit</a></td><td style=\"background-color:{6};text-color:#0000ff;\" align=\"center\"><a style=\"color:#0000ff;\" href=\"multiusersetup.aspx?ACT=REMOVE&USERID={5}&Contact={7}\">Delete</a></td></tr>", iCtr++, oDr["LOGIN_NAME"], oDr["CONTACT"], oDr["EMAILADDR"], oDr["USER_ROLE"], oDr["USER_ID"], color, oDr["CONTACT"]);
            }
        }
        return oHtmlStr;

        }
        catch (Exception ex)
        {
            objErrorHandler.ErrorMsg = ex;
            objErrorHandler.CreateLog();
            return string.Empty;  
        }
    }

    private int AddUser()
    {


        HelperServices objHelperServices = new HelperServices();
        
        int CompanyID=0;

        try
        {

            string websiteid = System.Configuration.ConfigurationManager.AppSettings["WEBSITEID"].ToString();

            string UserID = HttpContext.Current.Session["USER_ID"].ToString();
            int UserLogWebsiteId = objUserServices.GetUserWebSite_id(UserID);
            string webTitle = objUserServices.GetWebTitle(UserLogWebsiteId.ToString());
            if (websiteid == UserLogWebsiteId.ToString() && UserLogWebsiteId > 0 && UserLogWebsiteId != null)
            {
                websiteid = System.Configuration.ConfigurationManager.AppSettings["WEBSITEID"].ToString();
            }
            else
            {
                websiteid = objUserServices.GetUserWebSite_id(UserID).ToString();
            }
        

            //ConnectionDB oConStr = new ConnectionDB();
            //SqlConnection oCon = new SqlConnection(oConStr.ConnectionString.Replace("provider=SQLOLEDB;", ""));
            //oCon.Open();        
            //SqlCommand oCmd = new SqlCommand("SELECT COMPANY_ID FROM TBWC_COMPANY_BUYERS WHERE WEBSITE_ID = @websiteid AND USER_ID = @userid", oCon);
            //oCmd.Parameters.Clear();
            //oCmd.Parameters.AddWithValue("@websiteid", websiteid);
            //oCmd.Parameters.AddWithValue("@userid", Session["USER_ID"]);
            //int CompanyID = System.Convert.ToInt32(oCmd.ExecuteScalar());
            string tmpstr = (string)objHelperDB.GetGenericPageDataDB("", websiteid, Session["USER_ID"].ToString(), "GET_MULTIUSER_COMPANU_ID", HelperDB.ReturnType.RTString);
            if (tmpstr != null && tmpstr != "")
                CompanyID = Convert.ToInt32(tmpstr);


            int userRole = 0;
            if (radSA.Checked) userRole = 1;
            if (radAO.Checked) userRole = 2;
            if (radCO.Checked) userRole = 3;
            if (radBO.Checked) userRole = 4;

            /*string sSQL = "INSERT INTO TBWC_COMPANY_BUYERS([COMPANY_ID],[WEBSITE_ID],[LOGIN_NAME],[PASSWORD],[CONTACT],[PHONE],[EMAILADDR],[USER_ROLE],[NOTIFYORD],[NOTIFYACT],[NOTIFYNEWS],[USER_STATUS],[ORDDRPSHP],[CREATED_DATE],[CREATED_USER],[MODIFIED_DATE],[MODIFIED_USER]) ";
            sSQL += "VALUES(@COMPANY_ID,@WEBSITE_ID,@LOGIN_NAME,@PASSWORD,@CONTACT,
             * @PHONE,@EMAILADDR,@USER_ROLE,@NOTIFYORD,@NOTIFYACT,@NOTIFYNEWS,1,
             * @ORDDRPSHP,@CURDATE,@CUR_USER,@CURDATE,@CUR_USER)";
            oCmd = new SqlCommand(sSQL, oCon);
            oCmd.Parameters.Clear();
            oCmd.Parameters.AddWithValue("@COMPANY_ID", CompanyID);
            oCmd.Parameters.AddWithValue("@WEBSITE_ID", websiteid);
            oCmd.Parameters.AddWithValue("@LOGIN_NAME", txtLogin.Text);
            //oCmd.Parameters.AddWithValue("@LOGIN_NAME", Test);        
            oCmd.Parameters.AddWithValue("@PASSWORD", txtPass.Text);
            oCmd.Parameters.AddWithValue("@CONTACT", txtContact.Text);
            oCmd.Parameters.AddWithValue("@PHONE", txtPhone.Text);
            oCmd.Parameters.AddWithValue("@EMAILADDR", txtAEmail.Text);
            oCmd.Parameters.AddWithValue("@USER_ROLE", userRole);
            oCmd.Parameters.AddWithValue("@NOTIFYORD", chkDespatch.Checked);
            oCmd.Parameters.AddWithValue("@NOTIFYACT", chkAcc.Checked);
            oCmd.Parameters.AddWithValue("@NOTIFYNEWS", chkNewsUpd.Checked);
            oCmd.Parameters.AddWithValue("@ORDDRPSHP", chkDrop.Enabled == true ? chkDrop.Checked : false);
            oCmd.Parameters.AddWithValue("@CURDATE", System.DateTime.Now);
            oCmd.Parameters.AddWithValue("@CUR_USER", Session["USER_ID"]);
            oCmd.ExecuteNonQuery();
            if (oCon.State == ConnectionState.Open) oCon.Close();*/

            //string sSQL = "INSERT INTO TBWC_COMPANY_BUYERS([COMPANY_ID],[WEBSITE_ID],[LOGIN_NAME],[PASSWORD],[CONTACT],[PHONE],[EMAILADDR],[USER_ROLE],[NOTIFYORD],[NOTIFYACT],[NOTIFYNEWS],[USER_STATUS],[ORDDRPSHP],[CREATED_DATE],[CREATED_USER],[MODIFIED_DATE],[MODIFIED_USER]) ";
            //sSQL += "VALUES(@COMPANY_ID,@WEBSITE_ID,@LOGIN_NAME,@PASSWORD,@CONTACT,
            //@PHONE,@EMAILADDR,@USER_ROLE,@NOTIFYORD,@NOTIFYACT,@NOTIFYNEWS,1,
            //@ORDDRPSHP,@CURDATE,@CUR_USER,@CURDATE,@CUR_USER)";

            SqlCommand oCmd;

            oCmd = new SqlCommand("STP_TBWC_POP_MULTIUSER_COMPANY_BUYERS", objConnectionDB.GetConnection());
            oCmd.CommandType = CommandType.StoredProcedure;
            oCmd.Parameters.Clear();
            oCmd.Parameters.AddWithValue("@COMPANY_ID", CompanyID); 
            oCmd.Parameters.AddWithValue("@WEBSITE_ID", websiteid);
            oCmd.Parameters.AddWithValue("@LOGIN_NAME", txtLogin.Text);
            Newpassword = objSecurity.StringEnCrypt(txtPass.Text);
            oCmd.Parameters.AddWithValue("@PASSWORD", Newpassword);

            oCmd.Parameters.AddWithValue("@CONTACT", txtContact.Text);
            oCmd.Parameters.AddWithValue("@PHONE", txtPhone.Text);
            oCmd.Parameters.AddWithValue("@EMAILADDR", txtAEmail.Text);
            oCmd.Parameters.AddWithValue("@USER_ROLE", userRole);
            oCmd.Parameters.AddWithValue("@NOTIFYORD", chkDespatch.Checked);
            oCmd.Parameters.AddWithValue("@NOTIFYACT", chkAcc.Checked);
            oCmd.Parameters.AddWithValue("@NOTIFYNEWS", chkNewsUpd.Checked);
            if (radSA.Checked == true)
                oCmd.Parameters.AddWithValue("@NOTIFYMAIL", chkMailToAdmin.Checked);
            else
                oCmd.Parameters.AddWithValue("@NOTIFYMAIL", 0);
            oCmd.Parameters.AddWithValue("@USER_STATUS", 1);

            if (chkDrop.Checked == true)
            {
                oCmd.Parameters.AddWithValue("@ORDDRPSHP", 1);
            }
            else 
            {
                oCmd.Parameters.AddWithValue("@ORDDRPSHP", 0);
            
            }
            //oCmd.Parameters.AddWithValue("@CURDATE", System.DateTime.Now);
            oCmd.Parameters.AddWithValue("@CREATED_USER", Session["USER_ID"]);
            oCmd.Parameters.AddWithValue("@MOBILE_PHONE", txtmobilephone.Text);
           
            oCmd.ExecuteNonQuery();
            objConnectionDB.CloseConnection();

          //  decimal Updpr = objOrderServices.Update_MOBILE_NUMBER(mobilenumber, _UserrID, OrderID, false);
            return 1;
        }
        catch (Exception ex)
        {
            objErrorHandler.ErrorMsg = ex;
            objErrorHandler.CreateLog();
            return -1;
        }


    }

    private void DeleteUser(int UserID)
    {
        try
        {
            HelperServices objHelperServices = new HelperServices();
            string websiteid = System.Configuration.ConfigurationManager.AppSettings["WEBSITEID"].ToString();
            //ConnectionDB oConStr = new ConnectionDB();
            //SqlConnection oCon = new SqlConnection(oConStr.ConnectionString.Replace("provider=SQLOLEDB;", ""));
            //oCon.Open();
            //SqlCommand oCmd = new SqlCommand(string.Format("UPDATE TBWC_COMPANY_BUYERS SET USER_STATUS = 5 WHERE USER_ID={0}", UserID), oCon);
            //oCmd.ExecuteNonQuery();
            string sSql = "Exec STP_TBWC_RENEW_MULTIUSER_COMPANY_BUYERS_USER_STATUS " + UserID;
            objHelperDB.ExecuteSQLQueryDB(sSql);

            HttpContext.Current.Response.Redirect("Multiusersetup.aspx");
        }
        catch (Exception ex)
        {
            objErrorHandler.ErrorMsg = ex;
            objErrorHandler.CreateLog();
           
        }


    }

    private void ResetPassword(int UserID)
    {
        try
        {
        HelperServices objHelperServices = new HelperServices();
        string websiteid = System.Configuration.ConfigurationManager.AppSettings["WEBSITEID"].ToString();
        
        //ConnectionDB oConStr = new ConnectionDB();
        //SqlConnection oCon = new SqlConnection(oConStr.ConnectionString.Replace("provider=SQLOLEDB;", ""));
        //oCon.Open();

        //SqlCommand mCmd = new SqlCommand("SELECT EMAILADDR FROM TBWC_COMPANY_BUYERS WHERE USER_ID=@userid", oCon);
        //mCmd.Parameters.AddWithValue("@userid", UserID);
        //String EmailID = Convert.ToString(mCmd.ExecuteScalar());
        String EmailID = (string)objHelperDB.GetGenericPageDataDB(UserID.ToString(), "GET_MULTIUSER_EMAILADDR", HelperDB.ReturnType.RTString);  

        if (EmailID == "")
        {
            MessageLabel.Visible = true;
            MessageLabel.Text = "Enter User E-Mail Address";
        }
        else
        {
            string Password = GenPassword(10);
            NotificationServices objNotificationServices = new NotificationServices();
            objNotificationServices.SMTPServer = objHelperServices.GetOptionValues("MAIL SERVER").ToString();
            //oNot.NotifyTo.Add(oHelper.GetOptionValues("ADMIN EMAIL").ToString());
            objNotificationServices.NotifyTo.Add(EmailID);
            objNotificationServices.NotifyFrom = objHelperServices.GetOptionValues("ADMIN EMAIL").ToString();
            objNotificationServices.NotifySubject = "Mail from ";
            string message = "";
            message = message + "Your New Password : " + Password.ToString() + Environment.NewLine;
            objNotificationServices.NotifySubject = "Password Reset";
            objNotificationServices.NotifyMessage = message;
            int chkmail = objNotificationServices.SendMessage();   //Uncomment this line 
            
            //SqlCommand oCmd = new SqlCommand(string.Format("UPDATE TBWC_COMPANY_BUYERS SET PASSWORD='{0}', USER_STATUS = 4 WHERE USER_ID={1}", Password, UserID), oCon);
            //oCmd.ExecuteNonQuery();
            Newpassword = objSecurity.StringEnCrypt(Password);
            string sSql = "Exec STP_TBWC_RENEW_MULTIUSER_COMPANY_BUYERS_PASSWORD '" + Newpassword + "'," + UserID;
            objHelperDB.ExecuteSQLQueryDB(sSql);
            HttpContext.Current.Response.Redirect("Multiusersetup.aspx",false);
        }
        }
        catch (Exception ex)
        {
            objErrorHandler.ErrorMsg = ex;
            objErrorHandler.CreateLog();

        }
    }

    private int UpdUser()
    {
        int CompanyID = 0;
        HelperServices objHelperServices = new HelperServices();
        string websiteid = System.Configuration.ConfigurationManager.AppSettings["WEBSITEID"].ToString();
        try
        {
            string UserID = HttpContext.Current.Session["USER_ID"].ToString();
            int UserLogWebsiteId = objUserServices.GetUserWebSite_id(UserID);
            string webTitle = objUserServices.GetWebTitle(UserLogWebsiteId.ToString());
            if (websiteid == UserLogWebsiteId.ToString() && UserLogWebsiteId > 0 && UserLogWebsiteId != null)
            {
                websiteid = System.Configuration.ConfigurationManager.AppSettings["WEBSITEID"].ToString();
            }
            else
            {
                websiteid = objUserServices.GetUserWebSite_id(UserID).ToString();
            }
        //ConnectionDB oConStr = new ConnectionDB();
        //SqlConnection oCon = new SqlConnection(oConStr.ConnectionString.Replace("provider=SQLOLEDB;", ""));
        //oCon.Open();
        //SqlCommand oCmd = new SqlCommand("SELECT COMPANY_ID FROM TBWC_COMPANY_BUYERS WHERE WEBSITE_ID = @websiteid AND USER_ID = @userid", oCon);
        //oCmd.Parameters.Clear();
        //oCmd.Parameters.AddWithValue("@websiteid", websiteid);
        //oCmd.Parameters.AddWithValue("@userid", Session["USER_ID"]);
        //int CompanyID = System.Convert.ToInt32(oCmd.ExecuteScalar());

        string tmpstr = (string)objHelperDB.GetGenericPageDataDB("", websiteid, Session["USER_ID"].ToString(), "GET_MULTIUSER_COMPANU_ID", HelperDB.ReturnType.RTString);
        if (tmpstr != null && tmpstr != "")
            CompanyID = Convert.ToInt32(tmpstr);

        int userRole = 0;
        if (radSA.Checked) userRole = 1;
        if (radAO.Checked) userRole = 2;
        if (radCO.Checked) userRole = 3;
        if (radBO.Checked) userRole = 4;

        //string sSQL = "UPDATE TBWC_COMPANY_BUYERS SET [CONTACT] = @CONTACT, [PHONE] = @PHONE,[EMAILADDR] = @EMAILADDR, [USER_ROLE] = @USER_ROLE,[NOTIFYORD] = @NOTIFYORD,[NOTIFYACT] = @NOTIFYACT, [NOTIFYNEWS] = @NOTIFYNEWS,[ORDDRPSHP] = @ORDDRPSHP,[CREATED_DATE] = @CURDATE,[CREATED_USER] = @CURUSER,[MODIFIED_DATE] = @CURDATE,[MODIFIED_USER] = @CURUSER WHERE USER_ID = @USER_ID";
        //string sSQL = "UPDATE TBWC_COMPANY_BUYERS SET [CONTACT] = @CONTACT, [PASSWORD]=@PASSWORD, [PHONE] = @PHONE,[EMAILADDR] = @EMAILADDR, [USER_ROLE] = @USER_ROLE,[NOTIFYORD] = @NOTIFYORD,[NOTIFYACT] = @NOTIFYACT, [NOTIFYNEWS] = @NOTIFYNEWS,[ORDDRPSHP] = @ORDDRPSHP,[CREATED_USER] = @CURUSER,[MODIFIED_DATE] = @CURDATE,[MODIFIED_USER] = @CURUSER WHERE USER_ID = @USER_ID";
        SqlCommand oCmd;
        oCmd = new SqlCommand("STP_TBWC_RENEW_MULTIUSER_COMPANY_BUYERS", objConnectionDB.GetConnection());
        oCmd.CommandType = CommandType.StoredProcedure;
        oCmd.Parameters.Clear();
        //oCmd.Parameters.AddWithValue("@COMPANY_ID", CompanyID);
        //oCmd.Parameters.AddWithValue("@WEBSITE_ID", websiteid);
        //oCmd.Parameters.AddWithValue("@LOGIN_NAME", txtLogin.Text);
        Newpassword = objSecurity.StringEnCrypt(txtPass.Text);
        oCmd.Parameters.AddWithValue("@PASSWORD", Newpassword);
        oCmd.Parameters.AddWithValue("@CONTACT", txtContact.Text);
        oCmd.Parameters.AddWithValue("@PHONE", txtPhone.Text);
        oCmd.Parameters.AddWithValue("@EMAILADDR", txtAEmail.Text);
        oCmd.Parameters.AddWithValue("@USER_ROLE", userRole);
        oCmd.Parameters.AddWithValue("@NOTIFYORD", chkDespatch.Checked);
        oCmd.Parameters.AddWithValue("@NOTIFYACT", chkAcc.Checked);
        oCmd.Parameters.AddWithValue("@NOTIFYNEWS", chkNewsUpd.Checked);
        if (radSA.Checked == true)
            oCmd.Parameters.AddWithValue("@NOTIFYMAIL", chkMailToAdmin.Checked);
        else
            oCmd.Parameters.AddWithValue("@NOTIFYMAIL", 0);

        oCmd.Parameters.AddWithValue("@ORDDRPSHP", chkDrop.Enabled == true ? chkDrop.Checked : false);
        oCmd.Parameters.AddWithValue("@CURDATE", System.DateTime.Now);
        oCmd.Parameters.AddWithValue("@CURUSER", Session["USER_ID"]);
        oCmd.Parameters.AddWithValue("@USER_ID", txtUserID.Text);
        oCmd.Parameters.AddWithValue("@LOGIN_NAME",txtLogin.Text);
      //  oCmd.Parameters.AddWithValue("@MOBILE_PHONE", txtmobilephone.Text);
        int ret = oCmd.ExecuteNonQuery();
            objErrorHandler.CreateLog("upduser");
        objErrorHandler.CreateLog("dropshipment" + chkDrop.Enabled.ToString() + "ORDDRPSHP" + chkDrop.Checked.ToString());
        objConnectionDB.CloseConnection();
        string sSQL = "Exec STP_TBWC_UPDATE_MOBILE_NUMBER_USER ";
        sSQL = sSQL + "'" + txtmobilephone.Text + "'," + txtUserID.Text + "";
        objHelperDB.ExecuteSQLQueryDB(sSQL);
        //if (oCon.State == ConnectionState.Open) oCon.Close();
        //Response.Redirect("MultiuserSetup.aspx");
        return 1;

        }
        catch (Exception ex)
        {
            objErrorHandler.ErrorMsg = ex;
            objErrorHandler.CreateLog();
            return -1;
        }
    }


    public void GetFillUser(int UserID)
    {

        try
        {
        HelperServices objHelperServices = new HelperServices();
        string websiteid = System.Configuration.ConfigurationManager.AppSettings["WEBSITEID"].ToString();
        DataSet oDs = new DataSet();
        //ConnectionDB oConStr = new ConnectionDB();
        //SqlConnection oCon = new SqlConnection(oConStr.ConnectionString.Replace("provider=SQLOLEDB;", ""));
        //SqlDataAdapter oDa = new SqlDataAdapter("SELECT * FROM TBWC_COMPANY_BUYERS WHERE WEBSITE_ID = @websiteid AND USER_ID = @userid", oCon);
        //oDa.SelectCommand.Parameters.Clear();
        //oDa.SelectCommand.Parameters.AddWithValue("@websiteid", websiteid);
        //oDa.SelectCommand.Parameters.AddWithValue("@userid", UserID);
        //DataSet oDs = new DataSet();
        //oDa.Fill(oDs, "UserInfo");
        oDs=(DataSet)objHelperDB.GetGenericPageDataDB("",websiteid,UserID.ToString(), "GET_MULTIUSER_COMPANY_BUYERS_DETAILS", HelperDB.ReturnType.RTDataSet);
        if (oDs != null)
        {
            oDs.Tables [0].TableName ="UserInfo";
            foreach (DataRow oDr in oDs.Tables["UserInfo"].Rows)
            {
                txtUserID.Text = UserID.ToString();
                txtLogin.Text = oDr["LOGIN_NAME"].ToString();
                txtLogin.ReadOnly = false;
                txtContact.Text = oDr["CONTACT"].ToString();
                txtAEmail.Text = oDr["EMAILADDR"].ToString();
                txtAEmail.ReadOnly = !IsAdminUser();
                txtPhone.Text = oDr["USER_PHONE"].ToString();
                txtPass.Text = objSecurity.StringDeCrypt(oDr["Password"].ToString());
                txtPass.Attributes.Add("value", txtPass.Text);
                txtCPass.Text = objSecurity.StringDeCrypt(oDr["Password"].ToString());
                txtCPass.Attributes.Add("value", txtCPass.Text);
                //txtPass.ReadOnly = true;
                //txtCPass.ReadOnly = true;
                chkDrop.Checked = (System.Convert.ToInt16(oDr["ORDDRPSHP"]) == 0 ? false : true);
                //chkDrop.Enabled = IsAdminUser();
                chkAcc.Checked = System.Convert.ToBoolean(oDr["NotifyACT"]);
                chkDespatch.Checked = System.Convert.ToBoolean(oDr["NotifyORD"]);
                chkNewsUpd.Checked = System.Convert.ToBoolean(oDr["NotifyNEWS"]);
                int userrole = System.Convert.ToInt32(oDr["USER_ROLE"]);
                txtmobilephone.Text = oDr["MOBILE_PHONE"].ToString();
                switch (userrole)
                {
                    case 1:
                        radSA.Checked = true;
                        radAO.Checked = false;
                        radCO.Checked = false;
                        radBO.Checked = false;
                        chkDrop.Checked = true;
                        break;
                    case 2:
                        radAO.Checked = true;
                        radSA.Checked = false;
                        radCO.Checked = false;
                        radBO.Checked = false;
                        break;
                    case 3:
                        radCO.Checked = true;
                        radAO.Checked = false;
                        radSA.Checked = false;
                        radBO.Checked = false;
                        break;
                    case 4:
                        radBO.Checked = true;
                        radCO.Checked = false;
                        radAO.Checked = false;
                        radSA.Checked = false;
                        break;
                    default:
                        radBO.Checked = true;
                        radCO.Checked = false;
                        radAO.Checked = false;
                        radSA.Checked = false;
                        break;
                }

                if (radSA.Checked == true)
                    MailToAdmin.Visible = true;
                else
                    MailToAdmin.Visible = false;

                chkMailToAdmin.Checked = System.Convert.ToBoolean(oDr["NotifyMAIL"]);
            }
        }
        //if (oCon.State != ConnectionState.Open) oCon.Open();
        //SqlCommand oCmd = new SqlCommand("SELECT DROPSHIP FROM WES_CUSTOMER WHERE WEBSITE_ID = @websiteid  AND WES_CUSTOMER_ID=@COMPID", oCon);
        //oCmd.Parameters.Clear();
        //oCmd.Parameters.AddWithValue("@websiteid", websiteid);
        //oCmd.Parameters.AddWithValue("@COMPID", Session["COMPANY_ID"]);
        //int iDrop = Convert.ToInt16(oCmd.ExecuteScalar());
        //oCon.Close();
         int iDrop=0;
         string tmpstr=(string)objHelperDB.GetGenericPageDataDB("",websiteid,Session["COMPANY_ID"].ToString(), "GET_MULTIUSER_WES_CUSTOMER", HelperDB.ReturnType.RTString);
         if (tmpstr!=null && tmpstr==null)
            iDrop = Convert.ToInt16(tmpstr);

        }
        catch (Exception ex)
        {
            objErrorHandler.ErrorMsg = ex;
            objErrorHandler.CreateLog();

        }

    }

    public void IsDropShipment()
    {

        try
        {
        int iDrop = 0;
        string websiteid = System.Configuration.ConfigurationManager.AppSettings["WEBSITEID"].ToString();
        //ConnectionDB oConStr = new ConnectionDB();
        //SqlConnection oCon = new SqlConnection(oConStr.ConnectionString.Replace("provider=SQLOLEDB;", ""));
        //if (oCon.State != ConnectionState.Open) oCon.Open();
        //SqlCommand oCmd = new SqlCommand("SELECT DROPSHIP FROM WES_CUSTOMER WHERE WEBSITE_ID = @websiteid  AND WES_CUSTOMER_ID=@COMPID", oCon);
        //oCmd.Parameters.Clear();
        //oCmd.Parameters.AddWithValue("@websiteid", websiteid);
        //oCmd.Parameters.AddWithValue("@COMPID", Session["COMPANY_ID"]);
        //iDrop = Convert.ToInt16(oCmd.ExecuteScalar());
        //oCon.Close();

        string tmpstr = (string)objHelperDB.GetGenericPageDataDB("", websiteid, Session["COMPANY_ID"].ToString(), "GET_MULTIUSER_WES_CUSTOMER", HelperDB.ReturnType.RTString);
        if (tmpstr != null && tmpstr == null)
            iDrop = Convert.ToInt32(tmpstr);

       // chkDrop.Enabled = false;

        if (!radBO.Checked && iDrop == 1)
        {
            chkDrop.Enabled = true;
        }
        }
        catch (Exception ex)
        {
            objErrorHandler.ErrorMsg = ex;
            objErrorHandler.CreateLog();

        }
    }

    public bool IsAdminUser()
    {

        try
        {
        bool retvalue = false;
        HelperServices objHelperServices = new HelperServices();
        //ConnectionDB oConStr = new ConnectionDB();
        //SqlConnection oCon = new SqlConnection(oConStr.ConnectionString.Replace("provider=SQLOLEDB;", ""));
        //oCon.Open();
        string websiteid = System.Configuration.ConfigurationManager.AppSettings["WEBSITEID"].ToString();
        //SqlCommand oCmd = new SqlCommand("SELECT USER_ROLE FROM TBWC_COMPANY_BUYERS WHERE WEBSITE_ID = @websiteid  AND USER_ID = @userid", oCon);
        //oCmd.Parameters.Clear();
        //oCmd.Parameters.AddWithValue("@websiteid", websiteid);
        //oCmd.Parameters.AddWithValue("@userid", Session["USER_ID"]);
        //int UserRole = System.Convert.ToInt32(oCmd.ExecuteScalar());
        int UserRole = 0;
        string tmpstr = (string)objHelperDB.GetGenericPageDataDB("", websiteid, Session["USER_ID"].ToString(), "GET_MULTIUSER_COMPANY_BUYERS_USER_ROLE", HelperDB.ReturnType.RTString);
        if (tmpstr != null && tmpstr != "")
            UserRole = Convert.ToInt32(tmpstr);

        if (UserRole == 1)
            retvalue = true;
        //oCon.Close();
        return retvalue;

        }
        catch (Exception ex)
        {
            objErrorHandler.ErrorMsg = ex;
            objErrorHandler.CreateLog();
           return  false;
        }
    }

    protected void btnSave_Click(object sender, ImageClickEventArgs e)
    {

        try
        {
        int r = 0;
        if (!string.IsNullOrEmpty(txtAEmail.Text.Trim()))
        {
            if (isEmail(txtAEmail.Text.Trim()))
            {
                HidePopUpMessage();
                DataSet tmpdb = objUserServices.CheckMultipleLoginName(txtLogin.Text, txtUserID.Text);
                if (txtLogin.Text.ToString().ToUpper().StartsWith("WES") == true || txtLogin.Text.ToString().ToUpper().StartsWith("CELL") == true || txtLogin.Text.ToString().ToUpper().StartsWith("WAG") == true)
                {
                    MessageLabel.Text = "User name should not contain `WES` or `CELL` or `WAG`,try with different user name";
                    MessageLabel.Visible = true;
                }
                
                else if (txtLogin.Text.Trim().Length < 6)
                {
                   // MessageLabel.Text = (string)GetLocalResourceObject("ErrMsgusernameLimit");
                    MessageLabel.Text = "Login name length must be 6 to 10";
                    MessageLabel.Visible = true;
                    //"UserName must be 6 or more characters.";
                }
                else if (tmpdb != null && tmpdb.Tables.Count > 0 && tmpdb.Tables[0].Rows.Count > 0 )
                {
                    //MessageLabel.Text = (string)GetLocalResourceObject("ErrMsgExists");
                    MessageLabel.Text = "Login Name Already Exists";
                    MessageLabel.Visible = true;
                }
                else
                {
                     r = UpdUser();
                }
               // int r=UpdUser();
                if (r > 0)
                    HttpContext.Current.Response.Redirect("MultiuserSetup.aspx",false);
                else
                {
                   // MessageLabel.Text = "Update failed,Try again";
                    MessageLabel.Visible = true;
                }
            }
            else
            {
                HidePopUpMessage();
                MessageLabel.Text = "Invalid Email! Please Enter Valid Email Address!";
                MessageLabel.Visible = true;
            }
        }
        else
        {
            //ShowPopUpMessage("SaveButton");
        }

        }
        catch (Exception ex)
        {
            objErrorHandler.ErrorMsg = ex;
            objErrorHandler.CreateLog();
         
        }
    }

    protected void btnAdd_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            DataSet tmpdb = objUserServices.CheckMultipleLoginName(txtLogin.Text, "");
            if (tmpdb != null && tmpdb.Tables.Count > 0 && tmpdb.Tables[0].Rows.Count > 0)
            {
                MessageLabel.Text = "Login name already exists!";
                MessageLabel.Visible = true;
                return;
            }
            else if (txtLogin.Text.ToString().ToUpper().StartsWith("WES") == true || txtLogin.Text.ToString().ToUpper().StartsWith("CELL") == true || txtLogin.Text.ToString().ToUpper().StartsWith("WAG") == true)
            {
                MessageLabel.Text = "User name should not contain `WES` or `CELL` or `WAG`,try with different user name";
                MessageLabel.Visible = true;
                return;
            }

            else if (txtLogin.Text.Trim().Length < 6)
            {
                //MessageLabel.Text = (string)GetLocalResourceObject("ErrMsgusernameLimit");
                MessageLabel.Text = "Login name length must be 6 to 10";
                MessageLabel.Visible = true;
                return;
                //"UserName must be 6 or more characters.";
            }
            if (!string.IsNullOrEmpty(txtAEmail.Text.Trim()))
            {
                if (isEmail(txtAEmail.Text.Trim()))
                {
                    HidePopUpMessage();
                    int r = AddUser();
                    if (r > 0)
                        HttpContext.Current.Response.Redirect("MultiuserSetup.aspx",false);
                    else
                    {
                        MessageLabel.Text = "failed,Try again";
                        MessageLabel.Visible = true;
                    }

                }
                else
                {
                    HidePopUpMessage();
                    MessageLabel.Text = "Invalid Email! Please Enter Valid Email Address!";
                    MessageLabel.Visible = true;
                }
            }
            else
            {
                ShowPopUpMessage("AddButton");
            }
        }
        catch (Exception ex)
        {
            objErrorHandler.ErrorMsg = ex;
            objErrorHandler.CreateLog();

        }
    }

    private void ShowPopUpMessage(string buttonName)
    {

        try
        {
        modalPop.ID = "popUp1";
        modalPop.PopupControlID = "ModalPanel";
        modalPop.BackgroundCssClass = "modalBackground";

        switch (buttonName)
        {
            case "AddButton":
                modalPop.TargetControlID = "btnAdd";
                break;
            case "SaveButton":
                modalPop.TargetControlID = "btnSave";
                break;
        }
        }
        catch (Exception ex)
        {
            objErrorHandler.ErrorMsg = ex;
            objErrorHandler.CreateLog();

        }
        //modalPop.DropShadow = true;
        //this.ModalPanel.Controls.Add(modalPop);
        //this.ModalPanel.Visible = true;
        //modalPop.Show();
    }

    private void HidePopUpMessage()
    {
        //this.ModalPanel.Visible = false;
        //modalPop.Hide();
    }

    protected void btnOk_Click(object sender, EventArgs e)
    {

        try
        {
        HidePopUpMessage();

        if (!string.IsNullOrEmpty(Request["ACT"]) && Request.QueryString["ACT"].ToString().Equals("EDIT"))
        {
            UpdUser();
        }
        else
        {
            AddUser();
        }

        HttpContext.Current.Response.Redirect("MultiuserSetup.aspx");

        }
        catch (Exception ex)
        {
            objErrorHandler.ErrorMsg = ex;
            objErrorHandler.CreateLog();

        }
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        try
        {
        
        if (!string.IsNullOrEmpty(txtAEmail.Text.Trim()))
        {
            if (isEmail(txtAEmail.Text.Trim()))
            {
                HidePopUpMessage();

                if (!string.IsNullOrEmpty(Request["ACT"]) && Request.QueryString["ACT"].ToString().Equals("EDIT"))
                {
                    UpdUser();
                }
                else
                {
                    AddUser();
                }
                HttpContext.Current.Response.Redirect("MultiuserSetup.aspx");
            }
            else
            {
                HidePopUpMessage();
                MessageLabel.Text = "Invalid Email! Please Enter Valid Email Address!";
                MessageLabel.Visible = true;
            }
        }
        else
        {
            HidePopUpMessage();
            MessageLabel.Text = "Enter Your Email Address";
            MessageLabel.Visible = true;
            txtAEmail.Focus();
        }

        }
        catch (Exception ex)
        {
            objErrorHandler.ErrorMsg = ex;
            objErrorHandler.CreateLog();

        }
    }

    protected void txtLogin_TextChanged(object sender, EventArgs e)
    {
        try
        {
        
        lblMess.Text = "";
        HelperServices objHelperServices = new HelperServices();
        //ConnectionDB oConStr = new ConnectionDB();
        //SqlConnection oCon = new SqlConnection(oConStr.ConnectionString.Replace("provider=SQLOLEDB;", ""));
        //oCon.Open();
        string websiteid = System.Configuration.ConfigurationManager.AppSettings["WEBSITEID"].ToString();
        //SqlCommand oCmd = new SqlCommand("SELECT count(*) FROM TBWC_COMPANY_BUYERS WHERE WEBSITE_ID = @websiteid  AND login_name=@login", oCon);
        //oCmd.Parameters.Clear();
        //oCmd.Parameters.AddWithValue("@login", txtLogin.Text);
        //oCmd.Parameters.AddWithValue("@websiteid", websiteid);
        //int iRet = Convert.ToInt16(oCmd.ExecuteScalar());
        int iRet = 0;
        string tmpstr = (string)objHelperDB.GetGenericPageDataDB("", websiteid, txtLogin.Text, "GET_MULTIUSER_COMPANY_BUYERS_COUNT", HelperDB.ReturnType.RTString);
        if (tmpstr != null && tmpstr == null)
            iRet = Convert.ToInt32(tmpstr);

        if (iRet > 0)
        {
            lblMess.Text = "User id unavailable";
            btnAdd.Enabled = false;
        }
        else btnAdd.Enabled = true;
        //if (oCon.State == ConnectionState.Open) oCon.Close();
        }
        catch (Exception ex)
        {
            objErrorHandler.ErrorMsg = ex;
            objErrorHandler.CreateLog();

        }
        
    }

    protected void radSA_CheckedChanged(object sender, EventArgs e)
    {
      try
      {
        
        HelperServices objHelperServices = new HelperServices();
        //ConnectionDB oConStr = new ConnectionDB();
        //SqlConnection oCon = new SqlConnection(oConStr.ConnectionString.Replace("provider=SQLOLEDB;", ""));
        //oCon.Open();
        string websiteid = System.Configuration.ConfigurationManager.AppSettings["WEBSITEID"].ToString();
        //SqlCommand oCmd = new SqlCommand("SELECT DROPSHIP FROM WES_CUSTOMER WHERE WEBSITE_ID = @websiteid  AND WES_CUSTOMER_ID=@COMPID", oCon);
        //oCmd.Parameters.Clear();
        //oCmd.Parameters.AddWithValue("@websiteid", websiteid);
        //oCmd.Parameters.AddWithValue("@COMPID", Session["COMPANY_ID"]);
        //int iDrop = Convert.ToInt16(oCmd.ExecuteScalar());
        //oCon.Close();
        int iDrop = 0;
        string tmpstr = (string)objHelperDB.GetGenericPageDataDB("", websiteid, Session["COMPANY_ID"].ToString(), "GET_MULTIUSER_WES_CUSTOMER", HelperDB.ReturnType.RTString);
        if (tmpstr != null && tmpstr == null)
            iDrop = Convert.ToInt32(tmpstr);

       // chkDrop.Enabled = false;
        //  chkDrop.Checked = false;
        if (!radBO.Checked && iDrop == 1)
            chkDrop.Enabled = true;

      }
      catch (Exception ex)
      {
          objErrorHandler.ErrorMsg = ex;
          objErrorHandler.CreateLog();

      }
    }

    private string GenPassword(int PasswordLength)
    {
        string retval = "";
        string pass = System.Guid.NewGuid().ToString();
        retval = pass.Substring(0, PasswordLength);
        return retval;
    }
    protected void btnRst_Click(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(txtUserID.Text))
        {
            ResetPassword(Convert.ToInt32(txtUserID.Text));
        }
    }

    public static bool isEmail(string inputEmail)
    {
        bool isValid = false;

        string strRegex = @"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}" +
              @"\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\" +
              @".)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$";

        System.Text.RegularExpressions.Regex re = new System.Text.RegularExpressions.Regex(strRegex);

        if (string.IsNullOrEmpty(inputEmail))
        {
            isValid = false;
        }
        else
        {
            isValid = re.IsMatch(inputEmail);
        }

        return isValid;
    }

}