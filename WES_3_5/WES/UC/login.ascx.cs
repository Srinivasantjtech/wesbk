using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Services ; 
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Web.UI.HtmlControls;
using TradingBell.WebCat.CatalogDB;
using TradingBell.WebCat.Helpers;
using TradingBell.WebCat.TemplateRender;
using TradingBell.WebCat.CommonServices;

public partial class UC_login : System.Web.UI.UserControl
{
  
    #region "Declarations..."

    UserServices objUserServices = new UserServices();
    HelperDB objHelperDB = new HelperDB();
    HelperServices objHelperServices = new HelperServices();
    ErrorHandler objErrorHandler = new ErrorHandler();
    Security objSecurity = new Security();
    CompanyGroupServices objCompanyGroupServices = new CompanyGroupServices();
    private static int _HitCount;
    private static string sReferralURL;
    int newuserid;
    int cOrderID = 0;
    int cUserID = 0;
    bool redirectflag = false;
    #endregion

    #region "Events..."

    public static bool IsDate(Object obj)
    {
        if (obj != null)
        {
            string strDate = obj.ToString();
            try
            {
                DateTime dt = DateTime.Parse(strDate);
                if (dt >= DateTime.Now)
                    return true;

                return false;
            }
            catch
            {
                return false;
            }
        }
        return false;
    }
 
    protected void Page_Load(object sender, EventArgs e)
    {
        //if (Session["FORGOTPWD"] != null && Session["FORGOTPWD"].ToString() != "")
        //{
        //    RegSucess.Text = Session["FORGOTPWD"].ToString();
        //    RegSucess.Visible = true;
        //    Session["FORGOTPWD"] = "";
        //}
      
        if (!Page.IsPostBack)
        {
            chkShopCart.Checked = true;
          

            if (Request.Cookies["LoginInfo"] != null && Request.Cookies["LoginInfo"].Value.ToString().Trim() != "")
            {
                HttpCookie LoginInfoCookie = Request.Cookies["LoginInfo"];

                txtUserName.Text = objSecurity.StringDeCrypt(LoginInfoCookie["UserName"].ToString());
                chkKeepme.Checked = true;
                 if (IsDate(LoginInfoCookie["Expires"]))
                 {
                      hidpwd.Value = objSecurity.StringDeCrypt(LoginInfoCookie["Password"].ToString());
                 }
                 else
                 {
                     hidpwd.Value = string.Empty;
                 }               
                   
            }
            else
            {
                HttpCookie LoginInfoCookie = Request.Cookies["LoginInfo"];
                if (LoginInfoCookie != null && LoginInfoCookie["Password"] != null)
                    LoginInfoCookie["Password"] ="";

                chkKeepme.Checked = false;
            }

            chkShopCart.Disabled = true;
        }

        InitLoad();
        txtUserName.Focus();
        if (Request["Result"] == "SUCCESS")
        {
            RegSucess.Visible = true;
        }
        if (Session["FORGOTPWD"] != null && Session["FORGOTPWD"].ToString() != "")
        {
            RegSucess.Text = Session["FORGOTPWD"].ToString();
            RegSucess.Visible = true;
            Session["FORGOTPWD"] = "";
        }
        //if (Session["FORGOTPWD_FORGOT"] != null && Session["FORGOTPWD_FORGOT"].ToString() != "")
        //{
        //    RegSucess.Text = Session["FORGOTPWD_FORGOT"].ToString();
        //    RegSucess.Visible = true;
        //    Session["FORGOTPWD_FORGOT"] = "";
        //}
        if (Session["RESETPWD"] != null && Session["RESETPWD"].ToString() != "")
        {
            RegSucess.Text = Session["RESETPWD"].ToString();
            RegSucess.Visible = true;
            Session["RESETPWD"] = "";
        }

       // int i = 1;
        txtUserName.Attributes.Add("onkeydown", "if(event.which || event.keyCode){if ((event.which == 13) || (event.keyCode == 13)) {document.getElementById('" + cmdLogin.UniqueID + "').click();return false;}} else {return true}; ");
        txtPassword.Attributes.Add("onkeydown", "if(event.which || event.keyCode){if ((event.which == 13) || (event.keyCode == 13)) {document.getElementById('" + cmdLogin.UniqueID + "').click();return false;}} else {return true}; ");

        if (!IsPostBack)
        {
            try
            {
                 sReferralURL = Request.ServerVariables["HTTP_REFERER"].ToString();
            }
            catch (Exception ex)
            {
            }
        }

        if (Session["ForgotAction"] != null && Session["ForgotAction"].ToString().Trim() != string.Empty)
        {
            lblErrMsg.Text = Session["ForgotAction"].ToString();
        }
        else
        {
            lblErrMsg.Text = string.Empty;
        }

        lnkResetPassword.Attributes.Add("onclick", "javascript:ForgotLinkPage();");
        lnkForgotPWDPage.Attributes.Add("onclick", "javascript:ForgotLinkPage();");
        lnkForgotNamePage.Attributes.Add("onclick", "javascript:ForgotLinkPageUserID();");
       // CmdSignuphp.Attributes.Add("onclick", "javascript:CreateanAccount();");

        txtUserName.Attributes.Add("onkeypress", "javascript:return checkUserName(event);");
       //txtPassword.Attributes.Add("onkeypress", "javascript:return check(event);");
      // txtPassword.Attributes.Add("onkeyDown", "javascript:CheckTextPassMaxLength(this,event,'15');");
       if (Request.QueryString["Result"] != null)
       {
           if (Request.QueryString["Result"] == "FORGOTUSERID")
           {

               RegSucess.Text = GetGlobalResourceObject("login", "msgForgotuserid").ToString();
               //RegSucess.Text = "Your User Id(s) sent to your email address. Check your email address and continue to login";
               RegSucess.Visible = true;
           }
           else if (Request.QueryString["Result"] == "FORGOTPWD")
           {
               RegSucess.Text = GetGlobalResourceObject("login", "msgForgotPassWord").ToString();
               //RegSucess.Text = "Your password has been reset and sent to your email address. Check your email address and continue to login";
               RegSucess.Visible = true;
           }
           else if (Request.QueryString["Result"] == "RESETPASSWORD")
           {
               RegSucess.Text = GetGlobalResourceObject("login", "RESETPASSWORD").ToString();
              
               RegSucess.Visible = true;
           }
       }
      
           
       
        
        // txtchgPassnew.Text = "";
      //  txtchgCPass.Text = "";
      // CmdSignup.OnClientClick = "fn();return false;";
    }

  

    protected void cmdLogin_Click(object sender, EventArgs e)
    {
        string strMsg = string.Empty;
        bool isredirection = false;
        OrderServices.OrderInfo oOrdInfo = new OrderServices.OrderInfo();
        try
        {
            bool validUser;
            string username;
            string password;
            DataSet tmpds = null;
            int UserID;


            username = txtUserName.Text;
            password = txtPassword.Text;

            //Modified by:Indu
            //Modified on:11-Apr-2013
            //Mofified reason:To Build encrption logic
            //string key = System.Configuration.ConfigurationManager.AppSettings["SecurityKey"].ToString();
            //password = objcrpengine.Encrypt(txtPassword.Text, true, key);
           password = objSecurity.StringEnCrypt(txtPassword.Text);  
           // string pwd1 = objSecurity.Encrypt(txtPassword.Text, key);  
           // string pwd = objcrpengine.Decrypt("G3TKStGU7wx5g4hg3aQjWQ==", true, key); 
            if ((objHelperServices.ValidateEmail(username)))
            {
                tmpds = objUserServices.CheckMultipleUserMail(username);
                if (tmpds != null && tmpds.Tables.Count > 0 && tmpds.Tables[0].Rows.Count > 0)
                {
                    if( tmpds.Tables[0].Rows.Count > 1)
                    {
                        strMsg = GetGlobalResourceObject("login", "ErrorMsg10").ToString();
                        lblErrMsg.Text = strMsg;
                        RegSucess.Visible = false;                        
                        return;
                    }
                    else
                    {
                        username = tmpds.Tables[0].Rows[0]["LOGIN_NAME"].ToString();
                    }
                }               
            }
            validUser = objUserServices.CheckUserName(username);
            UserID = objUserServices.GetUserID(username);
            if (UserID != -1 && username != string.Empty)
            {

                if (objUserServices.GetPassword(UserID).EndsWith("W@9$"))
                {
                    //password = password + "W@9$";
                }
                if (Session["USER_NAME"] != null && Session["USER_NAME"].ToString() == username)
                {
                    strMsg = GetGlobalResourceObject("login", "ErrorMsg07").ToString();
                    lblErrMsg.Text = strMsg;
                    txtUserName.Focus();
                    RegSucess.Visible = false;
                }
                else
                {
                    if (objCompanyGroupServices.CheckCompanyStatus(UserID) == CompanyGroupServices.CompanyStatus.ACTIVE.ToString())
                    {
                        if ((validUser))
                        {
                            bool HasAdminUser = objUserServices.HasAdmin(UserID);
                            if (objUserServices.IsUserActive(UserID.ToString()))
                            {
                                if (objUserServices.CheckUser(username, password))
                                {
                                    //Setting cookie

                                    DataSet ds = objUserServices.GetsessionUserDetails(UserID);

                                    if (objUserServices.GetUserStatus(UserID) != 4)
                                        {
                                    SetCookie(txtUserName.Text, txtPassword.Text);
                                    }
                                    string Role;
                                    //Role = objUserServices.GetRole(UserID);
                                    Role = ds.Tables[0].Rows[0]["USER_ROLE"].ToString();

                                    if (Role != null)
                                    {
                                        objUserServices.OnLineFlag(true, UserID);
                                        //if (objUserServices.GetUserStatus(UserID) != 4)
                                        //{

                                        if (ds.Tables[0].Rows[0]["USER_STATUS"].ToString() != "4")
                                        {
                                            Session["USER_NAME"] = username;
                                            Session["USER_ID"] = UserID;
                                            Session["USER_ROLE"] = Role;
                                            //Session["COMPANY_ID"] = objUserServices.GetCompanyID(UserID);
                                            //Session["CUSTOMER_TYPE"] = objUserServices.GetCustomerType(UserID);
                                            Session["COMPANY_ID"] = ds.Tables[0].Rows[0]["COMPANY_ID"].ToString();
                                            Session["CUSTOMER_TYPE"] = ds.Tables[0].Rows[0]["CUSTOMER_TYPE"].ToString();
                                            Session["PRICE_CODE"] = ds.Tables[0].Rows[0]["PRICE_CODE"].ToString();
                                            Session["EBAY_BLOCK"] = ds.Tables[0].Rows[0]["EBAY_BLOCK"].ToString();

                                           // objErrorHandler.CreateLog("USER_ID" + UserID+ "username"+username +"EBAY_BLOCK"+ ds.Tables[0].Rows[0]["EBAY_BLOCK"].ToString());
                                            //|| ds.Tables[0].Rows[0]["EBAY_BLOCK"].ToString()=="True"
                                            if (ds.Tables[0].Rows[0]["EBAY_BLOCK"].ToString() == "1" || ds.Tables[0].Rows[0]["EBAY_BLOCK"].ToString() == "True")
                                            {

                                                Session["EBAY_BLOCK"] = "True";
                                            }
                                            else
                                            {

                                                Session["EBAY_BLOCK"] = "false";
                                            }

                                          objErrorHandler.CreateLog(ds.Tables[0].Rows[0]["EBAY_BLOCK"].ToString());
                                          if (ds.Tables[0].Rows[0]["EBAY_BLOCK"].ToString() == "1")
                                          {

                                              Session["EBAY_BLOCK"] = "True";
                                          }
                                          else
                                          {

                                              Session["EBAY_BLOCK"] = "false";
                                          }
                                            LogSession();
                                        }

                                        if (objUserServices.GetUserStatus(UserID) == 4)
                                        {
                                            newuserid = UserID;
                                           // ChgPassPop.Show();
                                            Session["USER_ID_Ch"] = UserID;
                                           // string uname = objSecurity.StringEnCrypt(UserID.ToString());
                                            Response.Redirect("ResetChangePassword.aspx",false);  
                                            //txtchgPassnew.Text = "";
                                           // txtchgCPass.Text = "";
                                            //Response.Redirect("ChangePassword.aspx?ChangePass=True");
                                        }
                                        else if (Session["PageUrl"] != null)
                                        {
                                            if (!(chkShopCart.Checked))
                                            {
                                                {

                                                }
                                                OrderServices objOrderServices = new OrderServices();
                                                int OpenOrdStatusID = (int)OrderServices.OrderStatus.OPEN;
                                                int OrderID = 0;

                                                if ((Session["ORDER_ID"] != null && Convert.ToInt32(Session["ORDER_ID"]) > 0) || (Request.QueryString["ViewOrder"] != null && Request.QueryString["ViewOrder"].Equals("View")))
                                                {
                                                    OrderID = Convert.ToInt32(Session["ORDER_ID"]);
                                                }
                                                else
                                                {
                                                    OrderID = objOrderServices.GetOrderID(objHelperServices.CI(Session["USER_ID"]), OpenOrdStatusID);
                                                }

                                                string OrderStatus = objOrderServices.GetOrderStatus(OrderID);
                                                if (OrderID > 0 && (OrderStatus == OrderServices.OrderStatus.OPEN.ToString() || OrderStatus == "CAU_PENDING"))
                                                {
                                                    DataSet oDSOrderItems = objOrderServices.GetOrderItems(OrderID);
                                                    if (oDSOrderItems != null)
                                                    {
                                                        foreach (DataRow dr in oDSOrderItems.Tables[0].Rows)
                                                        {
                                                            objOrderServices.UpdateQuantity(Convert.ToInt32(dr["PRODUCT_ID"].ToString()), Convert.ToInt32(dr["QTY"].ToString()) + Convert.ToInt32(dr["QTY_AVAIL"].ToString()));
                                                        }
                                                    }
                                                    int chk = cOrderID == 0 ? objOrderServices.RemoveItem("AllProd", OrderID, objHelperServices.CI(Session["USER_ID"]),"") : 0;
                                                    oOrdInfo.OrderID = OrderID;
                                                    oOrdInfo.ProdTotalPrice = 0.00M;
                                                    oOrdInfo.TotalAmount = 0.00M;
                                                    oOrdInfo.TaxAmount = 0.00M;
                                                    oOrdInfo.ShipCost = 0.00M;
                                                    objOrderServices.UpdateOrderPrice(oOrdInfo, true);
                                                }
                                            }
                                            else
                                            {
                                                OrderServices objOrderServices = new OrderServices();
                                                int OpenOrdStatusID = (int)OrderServices.OrderStatus.OPEN;

                                                cOrderID = objOrderServices.GetOrderID(objHelperServices.CI(Session["USER_ID"]), OpenOrdStatusID);
                                            }

                                            if (Session["PageUrl"].ToString().Contains("ConfirmMessage.aspx?Result=PASSWORDCHANGED"))
                                            {
                                                hlink.NavigateUrl = "MyAccount.aspx";
                                                if (!HasAdminUser)
                                                {
                                                    ShowAdminAlert.Show();
                                                }
                                                else
                                                    Response.Redirect("MyAccount.aspx");
                                            }
                                            else
                                            {
                                                hlink.NavigateUrl = Session["PageUrl"].ToString();
                                                if (!HasAdminUser)
                                                {
                                                    ShowAdminAlert.Show();
                                                }
                                                else
                                                //Response.Redirect(Session["PageUrl"].ToString());
                                                {

                                                    //modified by indu
                                                    isredirection = true;
                                                    Response.Redirect(Session["PageUrl"].ToString(), false);
                                                }
                                                    //   Response.Redirect("home.aspx", false);
                                            }
                                        }
                                        else
                                        {

                                            if (!(chkShopCart.Checked))
                                            {
                                                OrderServices objOrderServices = new OrderServices();
                                                int OpenOrdStatusID = (int)OrderServices.OrderStatus.OPEN;
                                                int OrderID = 0;

                                                if ((Session["ORDER_ID"] != null && Convert.ToInt32(Session["ORDER_ID"]) > 0) || (Request.QueryString["ViewOrder"] != null && Request.QueryString["ViewOrder"].Equals("View")))
                                                {
                                                    OrderID = Convert.ToInt32(Session["ORDER_ID"]);
                                                }
                                                else
                                                {
                                                    OrderID = objOrderServices.GetOrderID(objHelperServices.CI(Session["USER_ID"]), OpenOrdStatusID);
                                                }

                                                string OrderStatus = objOrderServices.GetOrderStatus(OrderID);
                                                if (OrderID > 0 && (OrderStatus == OrderServices.OrderStatus.OPEN.ToString() || OrderStatus == "CAU_PENDING"))
                                                {
                                                    DataSet oDSOrderItems = objOrderServices.GetOrderItems(OrderID);
                                                    if (oDSOrderItems != null)
                                                    {
                                                        foreach (DataRow dr in oDSOrderItems.Tables[0].Rows)
                                                        {
                                                            objOrderServices.UpdateQuantity(Convert.ToInt32(dr["PRODUCT_ID"].ToString()), Convert.ToInt32(dr["QTY"].ToString()) + Convert.ToInt32(dr["QTY_AVAIL"].ToString()));
                                                        }
                                                    }
                                                    int chk = cOrderID == 0 ? objOrderServices.RemoveItem("AllProd", OrderID, objHelperServices.CI(Session["USER_ID"]),"") : 0;
                                                    objOrderServices.RemoveOrder(OrderID, objHelperServices.CI(Session["USER_ID"]));

                                                    oOrdInfo.OrderID = OrderID;
                                                    oOrdInfo.ProdTotalPrice = 0.00M;
                                                    oOrdInfo.TotalAmount = 0.00M;
                                                    oOrdInfo.TaxAmount = 0.00M;
                                                    oOrdInfo.ShipCost = 0.00M;
                                                    objOrderServices.UpdateOrderPrice(oOrdInfo, true);
                                                }
                                            }
                                            else
                                            {
                                                OrderServices objOrderServices = new OrderServices();
                                                int OpenOrdStatusID = (int)OrderServices.OrderStatus.OPEN;

                                                cOrderID = objOrderServices.GetOrderID(objHelperServices.CI(Session["USER_ID"]), OpenOrdStatusID);
                                            }
                                            if (Request.QueryString["URL"] != null)
                                            {
                                                hlink.NavigateUrl = Request["URL"].ToString();
                                                if (!HasAdminUser)
                                                {
                                                    ShowAdminAlert.Show();
                                                }
                                                else
                                                    Response.Redirect(Request["URL"].ToString());
                                            }
                                            else
                                            {
                                                hlink.NavigateUrl = Request["URL"].ToString();
                                                if (!HasAdminUser)
                                                {
                                                    ShowAdminAlert.Show();
                                                }
                                                else
                                                    Response.Redirect("home.aspx", false);
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    //For Checking that different users log in to same time..
                                    //Initially it will locked even different users log in with wrong password (3 user)
                                    //3rd userd will be locked in the first time
                                    //To solve this following code is used.....
                                    if (Session["USER"] == null || Session["USER"].ToString() == "")
                                    {
                                        Session["USER"] = username;
                                        Session["COUNT"] = "1";
                                    }
                                    else
                                    {
                                        if (Session["USER"].ToString() != username)
                                        {
                                            Session["USER"] = username;
                                            Session["COUNT"] = "1";
                                        }
                                        else if (Session["USER"].ToString() == username)
                                        {
                                            Session["COUNT"] = (objHelperServices.CI(Session["COUNT"].ToString()) + 1).ToString();
                                        }
                                    }
                                    //Modified by:indu
                                    //Ontime defect:661
                                    if (objHelperServices.CI(Session["COUNT"].ToString()) < 6)
                                    {

                                        if (txtPassword.Text == string.Empty)
                                        {
                                            strMsg = GetGlobalResourceObject("login", "ErrorMsg05").ToString();
                                            lblErrMsg.Text = strMsg;
                                            RegSucess.Visible = false;
                                            txtPassword.Text = string.Empty;
                                            txtPassword.Focus();

                                        }
                                        else
                                        {
                                            strMsg = GetGlobalResourceObject("login", "ErrorMsg01").ToString();
                                            lblErrMsg.Text = strMsg;
                                            RegSucess.Visible = false;
                                            txtPassword.Text = string.Empty;
                                            txtPassword.Focus();
                                            HttpContext.Current.Response.Cookies.Remove("LoginInfo");
                                        }
                                    }
                                    else
                                    {
                                        objUserServices.LockUser(username);
                                        //Modified by:indu
                                        //Ontime defect:661
                                        //strMsg = GetGlobalResourceObject("login", "ErrorMsg021").ToString();
                                        strMsg = GetGlobalResourceObject("login", "ErrorMsg02").ToString();
                                        lblErrMsg.Text = strMsg;
                                        lnkResetPassword.Visible = true;  
                                        RegSucess.Visible = false;
                                        HttpContext.Current.Response.Cookies.Remove("LoginInfo");
                                    }
                                }

                            }
                            else if (objUserServices.GetUserStatus(UserID) == 3) 
                            {
                                strMsg = GetGlobalResourceObject("login", "ErrorMsg02").ToString();
                                lblErrMsg.Text = strMsg;
                                lnkResetPassword.Visible = true;  
                                RegSucess.Visible = false;
                                txtUserName.Text = string.Empty;
                                txtPassword.Text = string.Empty;
                                HttpContext.Current.Response.Cookies.Remove("LoginInfo");
                            }
                            else
                            {
                                strMsg = GetGlobalResourceObject("login", "ErrorMsg11").ToString();
                                lblErrMsg.Text = strMsg;
                                lnkResetPassword.Visible = false;
                                RegSucess.Visible = false;
                                txtUserName.Text = string.Empty;
                                txtPassword.Text = string.Empty;
                                HttpContext.Current.Response.Cookies.Remove("LoginInfo");
                            }


                        }
                        else
                        {
                            strMsg = GetGlobalResourceObject("login", "ErrorMsg03").ToString();
                            lblErrMsg.Text = strMsg;
                            RegSucess.Visible = false;
                            txtUserName.Text = string.Empty;
                            txtUserName.Focus();
                            HttpContext.Current.Response.Cookies.Remove("LoginInfo");
                        }
                    }
                    else
                    {
                        if (!(validUser))
                        {
                            strMsg = GetGlobalResourceObject("login", "ErrorMsg03").ToString();
                            lblErrMsg.Text = strMsg;
                            RegSucess.Visible = false;
                            txtUserName.Focus();
                            HttpContext.Current.Response.Cookies.Remove("LoginInfo");
                        }
                        else
                        {
                            strMsg = GetGlobalResourceObject("login", "ErrorMsg04").ToString();
                            lblErrMsg.Text = strMsg;
                            txtUserName.Focus();
                            RegSucess.Visible = false;
                            HttpContext.Current.Response.Cookies.Remove("LoginInfo");
                        }
                    }
                }
            }

            if (UserID == -1 && username == string.Empty)
            {
              //  strMsg = GetGlobalResourceObject("login", "ErrorMsg06").ToString();
                strMsg = GetGlobalResourceObject("login", "rfvUserName").ToString();               
               // lblErrMsg.Text = strMsg;
                lblErrMsg.Text = "Please Enter UserID";
                RegSucess.Visible = false;
                txtUserName.Focus();

            }
            if (txtPassword.Text == string.Empty && password == string.Empty && username != string.Empty)
            {
                strMsg = GetGlobalResourceObject("login", "rfvPassWord").ToString();
                lblErrMsg.Text = strMsg;
                RegSucess.Visible = false;
                txtPassword.Focus();

            }
            if (UserID == -1 && username != string.Empty && password != string.Empty)
            {
                strMsg = GetGlobalResourceObject("login", "ErrorMsg03").ToString();
                lblErrMsg.Text = strMsg;
                RegSucess.Visible = false;
                txtUserName.Focus();
            }


            if (cOrderID > 0)
            {
                OrderServices objOrderServices= new OrderServices();
                if (objOrderServices.GetOrderItemCount(cOrderID) > 0)
                {
                    Session["PrevOrderID"] = cOrderID;
                }
                else
                {
                    Session["PrevOrderID"] = "0";
                }
                if (!(isredirection))
                {
                    Response.Redirect("home.aspx", false);
                }
            }
            else
            {
                Session["PrevOrderID"] = "0";
            }
        }
        catch (Exception ex)
        {
            objErrorHandler.ErrorMsg = ex;
            objErrorHandler.CreateLog();
            strMsg = string.Empty;
        }
        //ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "<script> alert('Confirm password does not match')</script>", true);
    }

    private void SetCookie(string UserName, string Password)
    {
        if ((chkKeepme.Checked))
        {
            HttpCookie LoginInfoCookie = new HttpCookie("LoginInfo");
            LoginInfoCookie["UserName"] = objSecurity.StringEnCrypt(UserName);
            LoginInfoCookie["Password"] = objSecurity.StringEnCrypt(Password);
            LoginInfoCookie["Expires"] = DateTime.Now.AddDays(1).ToString(); 
             LoginInfoCookie["Login"] =  objSecurity.StringEnCrypt("True");
             LoginInfoCookie.Expires = DateTime.Now.AddDays(1);
            HttpContext.Current.Response.AppendCookie(LoginInfoCookie);
        }
        else
        {
            HttpCookie LoginInfoCookie = Request.Cookies["LoginInfo"];
            if (LoginInfoCookie != null && LoginInfoCookie["Password"] != null)
                LoginInfoCookie["Password"] = "";

            Response.Cookies["LoginInfo"].Expires = DateTime.Now.AddDays(-665);
            hidpwd.Value = string.Empty;
        }

    }

    #endregion

    #region "Functions..."

    public void InitLoad()
    {
       // int UserID;
        if (!IsPostBack)
        {
            _HitCount = 0;
        }
        try
        {
            if (Session.Count > 0)
            {
                if (Session["USER_ID"].ToString() != "" && Session["USER_ID"] != null && objUserServices.GetUserStatus(Convert.ToInt32(Session["USER_ID"].ToString())) == 1)
                {
                    if (Request.QueryString["URL"] != null)
                        Response.Redirect(Request["URL"].ToString(),false);
                    else
                        Response.Redirect("MyAccount.aspx",false);
                }



            }

          
        }
        catch (Exception ex)
        {
            objErrorHandler.ErrorMsg = ex;
            objErrorHandler.CreateLog();
        }

        
    }

    private void LogSession()
    {
        try
        {
            UserSessionServices objUserSession = new UserSessionServices();
            UserSessionServices.UserSessionInfo oUSI = new UserSessionServices.UserSessionInfo();

            oUSI.Session_ID = Session.SessionID;
            oUSI.Referal_URL = sReferralURL;
            oUSI.User_ID = objHelperServices.CI(Session["USER_ID"]);
            oUSI.Last_IP = Request.ServerVariables["REMOTE_ADDR"].ToString();
            if (oUSI.User_ID > 0)
            {
                objUserSession.TrackSession(oUSI);
            }
        }
        catch (Exception ex)
        {
            objErrorHandler.ErrorMsg = ex;
            objErrorHandler.CreateLog();
        }
    }

    #endregion
   
    protected void btnOk_Click(object sender, EventArgs e)
    {
       // string pass = txtchgPassnew1.Text.Trim().Substring(1);
        //string Cpass = txtchgCPass.Text.Trim().Substring(1);
        string pass = txtchgPassnew1.Text.Trim();
        string Cpass = txtchgCPass.Text.Trim();
        pass = objSecurity.StringEnCrypt(txtchgPassnew1.Text);
        Cpass = objSecurity.StringEnCrypt(txtchgCPass.Text);
        if (pass != string.Empty)
        {
            if (Cpass == pass)
            {
                objUserServices.ChangePassword(System.Convert.ToInt32(Session["USER_ID"]), pass);
              
                Session["USER_NAME"] = txtUserName.Text  ;
                Session["USER_ID"] = newuserid;
                string Role;
                Role = objUserServices.GetRole(newuserid);
                Session["USER_ROLE"] = Role;
                Session["COMPANY_ID"] = objUserServices.GetCompanyID(newuserid);
                Session["CUSTOMER_TYPE"] = objUserServices.GetCustomerType(newuserid);
                HttpContext.Current.Session["USER_STATUS"] = 1;
                LogSession();
                HttpContext.Current.Response.Redirect("Home.aspx");
                HttpContext.Current.Response.Close();
            }
            else
            {
                txtchgPassnew1.Text = string.Empty;
                txtchgCPass.Text = string.Empty;
                lblCMsg.Text = "Confirm password does not match";
                lblCMsg1.Text = "Confirm password does not match";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "<script> alert('Confirm password does not match')</script>", true);
            }
        }
        else
            lblCMsg.Text = "Invalid Password";

        //if (txtchgPassnew1.Text.Trim() != "")
        //{
        //    if (txtchgCPass.Text.Trim() == txtchgPassnew1.Text.Trim())
        //    {
        //        objUserServices.ChangePassword(System.Convert.ToInt32(Session["USER_ID"]), txtchgPassnew1.Text.Trim());
        //        HttpContext.Current.Response.Redirect("Home.aspx");
        //        HttpContext.Current.Response.Close();
        //    }
        //    else
        //    {
        //        lblCMsg.Text = "Confirm password does not match";
        //        lblCMsg1.Text = "Confirm password does not match";
        //        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "<script> alert('Confirm password does not match')</script>", true);
        //    }
        //}
        //else
        //    lblCMsg.Text = "Invalid Password";
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        HttpContext.Current.Response.Redirect("Logout.aspx");
        HttpContext.Current.Response.Close();
    }

    protected void CmdSignup_Click(object sender, EventArgs e)
    {
        redirectflag = true;
        HttpContext.Current.Response.Redirect("CreateanAccount.aspx");
        
    }
    protected void CmdSignupNew_Click(object sender, EventArgs e)
    {

    }
}
