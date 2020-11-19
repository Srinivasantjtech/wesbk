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
using TradingBell.WebCat;

using System.Security.Cryptography;
using System.Text;
using TradingBell.WebCat.CatalogDB;
using TradingBell.WebCat.Helpers;
using TradingBell.WebCat.TemplateRender;
using TradingBell.WebCat.CommonServices;
using StringTemplate = Antlr3.ST.StringTemplate;  
using StringTemplateGroup = Antlr3.ST.StringTemplateGroup;
public partial class ForgotUserName : System.Web.UI.Page
{
    #region "Delecarations..."
    private static int _UserID;
    UserServices objUserServices = new UserServices();
    DataTable objdt = new DataTable();
    CompanyGroupServices objCompanyGroupServices = new CompanyGroupServices();
    NotificationServices objNotificationServices = new NotificationServices();
    //ConnectionDB objConnectionDB = new ConnectionDB();
    HelperServices objHelperServices = new HelperServices();
    ErrorHandler objErrorHandler = new ErrorHandler();
    private const int DefaultMinimum = 6;
    private const int DefaultMaximum = 10;
    private const int UBoundDigit = 61;

    private RNGCryptoServiceProvider rng;
    private int minSize;
    private int maxSize;
    private bool hasRepeating;
    private bool hasConsecutive;
    private bool hasSymbols;
    private string exclusionSet;
    private char[] pwdCharArray = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789$".ToCharArray();

    #endregion
    #region property
    public string Exclusions
    {
        get
        {
            return this.exclusionSet;
        }
        set
        {
            this.exclusionSet = value;
        }
    }
    public bool ExcludeSymbols
    {
        get { return this.hasSymbols; }
        set { this.hasSymbols = value; }
    }

    public bool RepeatCharacters
    {
        get { return this.hasRepeating; }
        set { this.hasRepeating = value; }
    }

    public bool ConsecutiveCharacters
    {
        get { return this.hasConsecutive; }
        set { this.hasConsecutive = value; }
    }
    public int Minimum
    {
        get { return this.minSize; }
        set
        {
            this.minSize = value;
            if (DefaultMinimum > this.minSize)
            {
                this.minSize = DefaultMinimum;
            }
        }
    }

    public int Maximum
    {
        get { return this.maxSize; }
        set
        {
            this.maxSize = value;
            if (this.minSize >= this.maxSize)
            {
                this.maxSize = DefaultMaximum;
            }
        }
    }
    #endregion
    # region "Events..."

    protected void Page_Load(object sender, EventArgs e)
    {
        //if (Request.QueryString["loginName"] != null && Request.QueryString["loginName"].ToString().Trim() != "")
        //{
        //    bool validUser;
        //    string username;
        //    int UserID;

        //    txtLoginName.Text = Request.QueryString["loginName"];
        //    username = txtLoginName.Text;
        //    validUser = oUser.CheckUserName(username);
        //    UserID = oUser.GetUserID(username);
        //    if (UserID == -1 || username == string.Empty || validUser == false)
        //    {
        //        Session["ForgotAction"] = "Invalid User Name!";
        //        Response.Redirect("Login.aspx", true);
        //    }
        //}
        //else
        //{
        //    Session["ForgotAction"] = "Invalid User Name!";
        //    Response.Redirect("Login.aspx", true);
        //}

        //Session["ForgotAction"] = "";
        //txtLoginName.Attributes.Add("onkeypress", "javascript:return Email(event);");
        txtUserID.Attributes.Add("onkeypress", "javascript:return Email(event);");

        txtUserID.Attributes.Add("onkeydown", "if(event.which || event.keyCode){if ((event.which == 13) || (event.keyCode == 13)) {document.getElementById('" + btnUser.UniqueID + "').click();return false;}} else {return true}; ");
        txtYourAnswer.Attributes.Add("onkeydown", "if(event.which || event.keyCode){if ((event.which == 13) || (event.keyCode == 13)) {document.getElementById('" + btnSubmit.UniqueID + "').click();return false;}} else {return true}; ");
        try
        {



            Page.Title = objHelperServices.GetOptionValues("BROWSER TITLE").ToString();
            HtmlMeta meta = new HtmlMeta();
            meta.Name = "keywords";
            meta.Content = objHelperServices.GetOptionValues("Meta keyword").ToString();
            this.Header.Controls.Add(meta);
            // Render: <meta name="Description" content="noindex" />
            meta = new HtmlMeta();
            meta.Name = "Description";
            meta.Content = objHelperServices.GetOptionValues("Meta Description").ToString();
            this.Header.Controls.Add(meta);
            // Render: <meta name="Abstraction" content="Some words listed here" />
            meta.Name = "Abstraction";
            meta.Content = objHelperServices.GetOptionValues("Meta Abstraction").ToString();
            this.Header.Controls.Add(meta);
            // Render: <meta name="Distribution" content="noindex" />
            meta = new HtmlMeta();
            meta.Name = "Distribution";
            meta.Content = objHelperServices.GetOptionValues("Meta Distribution").ToString();
            this.Header.Controls.Add(meta);


            this.Minimum = DefaultMinimum;
            this.Maximum = DefaultMaximum;
            this.ConsecutiveCharacters = false;
            this.RepeatCharacters = true;
            this.ExcludeSymbols = false;
            this.Exclusions = null;

            rng = new RNGCryptoServiceProvider();
            Session["COUNT"] = "0";
        }
        catch (Exception ex)
        {
            objErrorHandler.ErrorMsg = ex;
            objErrorHandler.CreateLog();
        }

        txtUserID.Focus();
    }

    protected void btnUser_Click(object sender, EventArgs e)
    {
        tblError.Visible = false;
        try
        {
            if (tblSecurityQuestion.Visible)
            {
                if (txtYourAnswer.Text.Trim().Length > 0)
                {
                    string sAnswer = objUserServices.GetSecurityAnswer(_UserID);
                    if (sAnswer.Equals(txtYourAnswer.Text.Trim()))
                    {
                        SendNotification();
                       // lblsucess.Text = "Your User Id(s) sent to your email address. Check your email address and continue to login.";
                        Response.Redirect("ConfirmMessage.aspx?Result=FORGOTPASSWORD");
                    }
                    else
                    {
                        tblError.Visible = true;
                        lblError.Text = GetLocalResourceObject("msgAnswerError").ToString();
                    }
                }
                else
                {
                    tblError.Visible = true;
                    lblError.Text = GetLocalResourceObject("msgAnswerEmptyError").ToString();
                }
            }
            //if (tblUserID.Visible)
            //{
                if (txtUserID.Text.Trim().Length > 0)
                {

                    objdt = objCompanyGroupServices.GetForgotUserId(txtUserID.Text, "Dealer");

                    if (objdt != null && objdt.Rows.Count > 0)
                    {
                        if (objUserServices.CheckUserEmail(txtUserID.Text,"Dealer"))
                        {
                            //_UserID = objUserServices.GetUserID(txtLoginName.Text);
                            //SendNotification();
                            SendNotificationMail();
                            divmain.Visible = false;
                            lblsucess.Visible = true; 
                            lblsucess.Text = "Your User Id(s) sent to your email address. Check your email address and continue to login.";
                            //Response.Redirect("ConfirmMessage.aspx?Result=FORGOTUSERID");
                            //RetriveSecurityQuestion(_UserID);
                        }
                        else
                        {
                            tblError.Visible = true;
                            lblError.Text = GetLocalResourceObject("msgUserIDError").ToString();
                        }
                    }
                    else
                    {
                        tblError.Visible = true;
                        lblError.Text = GetLocalResourceObject("msgCompanyError").ToString();
                    }

                }
                else
                {
                    tblError.Visible = true;
                    lblError.Text = GetLocalResourceObject("msgUserIDEmptyError").ToString();
                }
            //}

        }
        catch (Exception ex)
        {
            objErrorHandler.ErrorMsg = ex;
            objErrorHandler.CreateLog();
        }
    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            /* tblError.Visible = false;
             if (tblSecurityQuestion.Visible)
             {
                 if (txtYourAnswer.Text.Trim().Length > 0)
                 {
                     string sAnswer = objUserServices.GetSecurityAnswer(_UserID);
                     string userEmail = txtUserID.Text;
                     if (sAnswer.Equals(txtYourAnswer.Text.Trim()) && objUserServices.CheckForgotUserEmail(txtLoginName.Text, txtUserID.Text))
                     {
                         SendNotification();
                         Response.Redirect("ConfirmMessage.aspx?Result=FORGOTPASSWORD");
                     }
                     else
                     {
                         tblError.Visible = true;
                         lblError.Text = GetLocalResourceObject("msgAnswerError").ToString();
                     }
                 }
                 else
                 {
                     tblError.Visible = true;
                     lblError.Text = GetLocalResourceObject("msgAnswerEmptyError").ToString();
                 }
             }
             if (tblUserID.Visible)
             {
                 if (txtUserID.Text.Trim().Length > 0)
                 {
                     if (objUserServices.CheckUserName(txtUserID.Text))
                     {
                         _UserID = objUserServices.GetUserID(txtUserID.Text);
                         RetriveSecurityQuestion(_UserID);
                     }
                     else
                     {
                         tblError.Visible = true;
                         lblError.Text = GetLocalResourceObject("msgUserIDError").ToString();
                     }
                 }
                 else
                 {
                     tblError.Visible = true;
                     lblError.Text = GetLocalResourceObject("msgUserIDEmptyError").ToString();
                 }
             }*/
        }
        catch (Exception ex)
        {
            objErrorHandler.ErrorMsg = ex;
            objErrorHandler.CreateLog();
        }
    }

    #endregion

    #region "Functions..."
    public string Generate()
    {
        // Pick random length between minimum and maximum   
        int pwdLength = GetCryptographicRandomNumber(this.Minimum,
            this.Maximum);

        StringBuilder pwdBuffer = new StringBuilder();
        pwdBuffer.Capacity = this.Maximum;

        // Generate random characters
        char lastCharacter, nextCharacter;

        // Initial dummy character flag
        lastCharacter = nextCharacter = '\n';

        for (int i = 0; i < pwdLength; i++)
        {
            nextCharacter = GetRandomCharacter();

            if (false == this.ConsecutiveCharacters)
            {
                while (lastCharacter == nextCharacter)
                {
                    nextCharacter = GetRandomCharacter();
                }
            }

            if (false == this.RepeatCharacters)
            {
                string temp = pwdBuffer.ToString();
                int duplicateIndex = temp.IndexOf(nextCharacter);
                while (-1 != duplicateIndex)
                {
                    nextCharacter = GetRandomCharacter();
                    duplicateIndex = temp.IndexOf(nextCharacter);
                }
            }

            if ((null != this.Exclusions))
            {
                while (-1 != this.Exclusions.IndexOf(nextCharacter))
                {
                    nextCharacter = GetRandomCharacter();
                }
            }

            pwdBuffer.Append(nextCharacter);
            lastCharacter = nextCharacter;
        }

        if (null != pwdBuffer)
        {
            return pwdBuffer.ToString();
        }
        else
        {
            return String.Empty;
        }
    }
    protected char GetRandomCharacter()
    {
        int upperBound = pwdCharArray.GetUpperBound(0);

        if (true == this.ExcludeSymbols)
        {
            upperBound = UBoundDigit;
        }

        int randomCharPosition = GetCryptographicRandomNumber(
            pwdCharArray.GetLowerBound(0), upperBound);

        char randomChar = pwdCharArray[randomCharPosition];

        return randomChar;
    }

    protected int GetCryptographicRandomNumber(int lBound, int uBound)
    {
        // Assumes lBound >= 0 && lBound < uBound
        // returns an int >= lBound and < uBound
        uint urndnum;
        byte[] rndnum = new Byte[4];
        if (lBound == uBound - 1)
        {
            // test for degenerate case where only lBound can be returned
            return lBound;
        }

        uint xcludeRndBase = (uint.MaxValue -
            (uint.MaxValue % (uint)(uBound - lBound)));

        do
        {
            rng.GetBytes(rndnum);
            urndnum = System.BitConverter.ToUInt32(rndnum, 0);
        } while (urndnum >= xcludeRndBase);

        return (int)(urndnum % (uBound - lBound)) + lBound;
    }
    //public int PasswordGenerator() 
    //    {
    //        this.Minimum               = DefaultMinimum;
    //        this.Maximum               = DefaultMaximum;
    //        this.ConsecutiveCharacters = false;
    //        this.RepeatCharacters      = true;
    //        this.ExcludeSymbols        = false;
    //        this.Exclusions            = null;

    //        rng = new RNGCryptoServiceProvider();
    //    }	
    //public void RetriveSecurityQuestion(int UserID)
    //{
    //    tblUserID.Visible = false;
    //    tblusedidbtn.Visible = false;
    //    tblSecurityQuestion.Visible = true;
    //    tblSecurityQuestionbtn.Visible = true;
    //    lblSecurityQuestion.Text = objUserServices.GetSecurityQuestion(UserID);
    //}

    public void SendNotification()
    {
        try
        {
            string UserName = "";
            string sEmailContent = "";
            string UserFullName = "";
            string ResetPasswordLink = "";
            //objNotificationServices.NotifyConnection = objConnectionDB.GetConnection();
            DataSet dsFP = objNotificationServices.BuildNotifyInfo();
            DataRow row = dsFP.Tables[0].NewRow();
            row["ColumnKey"] = objNotificationServices.UniqueStartSymbol + NotificationVariablesServices.ResetPassword.USERID.ToString() + objNotificationServices.UniqueEndSymbol;
            row["ColumnValue"] = objUserServices.GetUserEmailAdd(_UserID);
            UserName = objUserServices.GetUserEmailAdd(_UserID);
            //row["ColumnValue"] = oUser.GetUserEmail(txtUserID.Text);
            //UserName = oUser.GetUserEmail(txtUserID.Text);
            dsFP.Tables[0].Rows.Add(row);
            row = dsFP.Tables[0].NewRow();
            row["ColumnKey"] = objNotificationServices.UniqueStartSymbol + NotificationVariablesServices.ResetPassword.FULLNAME.ToString() + objNotificationServices.UniqueEndSymbol;
            row["ColumnValue"] = objUserServices.GetUserFullName(_UserID);
            UserFullName = objUserServices.GetUserFullName(_UserID);
            dsFP.Tables[0].Rows.Add(row);
            row = dsFP.Tables[0].NewRow();
            row["ColumnKey"] = objNotificationServices.UniqueStartSymbol + NotificationVariablesServices.ResetPassword.NEWPASSWORD.ToString() + objNotificationServices.UniqueEndSymbol;
            row["ColumnValue"] = objUserServices.GetPassword(_UserID);
            dsFP.Tables[0].Rows.Add(row);
            string sTemplateContent = objNotificationServices.GetTemplateContent(NotificationVariablesServices.NotificationList.RESETPASSWORD.ToString());
            string newPassword = "";
            newPassword = Generate();
            //  ResetPasswordLink = string.Format("http://staging.wesonline.com.au/ResetPassword/{0}/{1}/",txtLoginName.Text, newPassword);
            sEmailContent = sTemplateContent;
            //sEmailContent = sEmailContent.Replace("{FULLNAME}", UserFullName);
            //sEmailContent = sEmailContent.Replace("{NEWPASSWORD}", newPassword);
            //sEmailContent = sEmailContent.Replace("{USER_ID}", _UserID.ToString());
            sEmailContent = sEmailContent.Replace("{RESETPWDLINK}", ResetPasswordLink.ToString());
            objNotificationServices.SMTPServer = objHelperServices.GetOptionValues("MAIL SERVER").ToString();
            objNotificationServices.NotifyTo.Add(UserName);
            objNotificationServices.NotifyFrom = objHelperServices.GetOptionValues("ADMIN EMAIL").ToString();
            objNotificationServices.NotifySubject = objNotificationServices.GetEmailSubject(NotificationVariablesServices.NotificationList.RESETPASSWORD.ToString());
            objNotificationServices.NotifyMessage = sEmailContent;
            objNotificationServices.NotifyIsHTML = objNotificationServices.IsHTMLNotification(NotificationVariablesServices.NotificationList.RESETPASSWORD.ToString());
            int chkmail = objNotificationServices.SendMessage();
            if (chkmail > 0)
            {
                // int Userchk = objUserServices.UpdateUserName(newPassword, _UserID, txtLoginName.Text, txtUserID.Text);
            }
        }
        catch (Exception ex)
        {
            objErrorHandler.ErrorMsg = ex;
            objErrorHandler.CreateLog();
        }
    }


    private int SendNotificationMail()
    {
        try
        {


            string BillAdd;
            string ShippAdd;
            string stemplatepath;
            //DataSet dsOItem = new DataSet();
            //OrderServices.OrderInfo oOrderInfo = new OrderServices.OrderInfo();
            //UserServices objUserServices = new UserServices();
            //UserServices.UserInfo oUserInfo = new UserServices.UserInfo();


            string sHTML = "";
            try
            {
                StringTemplateGroup _stg_container = null;
                StringTemplateGroup _stg_records = null;
                StringTemplate _stmpl_container = null;
                StringTemplate _stmpl_records = null;
                StringTemplate _stmpl_records1 = null;
                StringTemplate _stmpl_recordsrows = null;
                TBWDataList[] lstrecords = new TBWDataList[0];
                TBWDataList[] lstrows = new TBWDataList[0];

                StringTemplateGroup _stg_container1 = null;
                StringTemplateGroup _stg_records1 = null;
                TBWDataList1[] lstrecords1 = new TBWDataList1[0];
                TBWDataList1[] lstrows1 = new TBWDataList1[0];

                stemplatepath = Server.MapPath(objHelperServices.GetOptionValues("SHOPPTING TEMPLATE PATH").ToString());
                int ictrows = 0;

                DataSet dscat = new DataSet();
                DataTable dt = null;
                _stg_records = new StringTemplateGroup("row", stemplatepath);
                _stg_container = new StringTemplateGroup("main", stemplatepath);


                lstrecords = new TBWDataList[objdt.Rows.Count + 1];



                int ictrecords = 0;

                foreach (DataRow dr in objdt.Rows)//For Records
                {

                    _stmpl_records = _stg_records.GetInstanceOf("mail" + "\\" + "ForgotuserPwdRow");
                    _stmpl_records.SetAttribute("UserName", dr["LOGIN_NAME"].ToString());

                    lstrecords[ictrecords] = new TBWDataList(_stmpl_records.ToString());
                    ictrecords++;
                }

                _stmpl_container = _stg_container.GetInstanceOf("mail" + "\\" + "Forgotuseridmain");

                _stmpl_container.SetAttribute("TBWDataList", lstrecords);
                sHTML = _stmpl_container.ToString();
            }
            catch (Exception ex)
            {
                objErrorHandler.ErrorMsg = ex;
                objErrorHandler.CreateLog();
                sHTML = "";
            }
            if (sHTML != "")
            {
                // objNotificationServices.NotifyConnection = objConnectionDB.GetConnection();
                //System.Net.Mail.MailMessage MessageObj = new System.Net.Mail.MailMessage();

                //MessageObj.From = new System.Net.Mail.MailAddress(objHelperServices.GetOptionValues("ADMIN EMAIL").ToString());
                //MessageObj.To.Add(txtUserID.Text);
                //MessageObj.Subject = "Forgot User Name-Cellink";
                //MessageObj.IsBodyHtml = true;
                //MessageObj.Body = sHTML;
                //System.Net.Mail.SmtpClient smtpclient = new System.Net.Mail.SmtpClient(objHelperServices.GetOptionValues("MAIL SERVER").ToString());                
                //smtpclient.UseDefaultCredentials = false;
                //smtpclient.Credentials = new System.Net.NetworkCredential(objHelperServices.GetOptionValues("MAIL SERVER USERNAME").ToString(), objHelperServices.GetOptionValues("MAIL SERVER PASSWORD").ToString());                
                //smtpclient.Send(MessageObj);

                objNotificationServices.SMTPServer = objHelperServices.GetOptionValues("MAIL SERVER").ToString();
                objNotificationServices.NotifyTo.Add(txtUserID.Text);
                objNotificationServices.NotifyFrom = objHelperServices.GetOptionValues("ADMIN EMAIL").ToString();
                //objNotificationServices.NotifySubject = "Forgot User Name-Cellink";
                objNotificationServices.NotifySubject = "WES Login ID Info";
                objNotificationServices.NotifyMessage = sHTML;
                objNotificationServices.NotifyIsHTML = true;
                int chkmail = objNotificationServices.SendMessage();
                return chkmail;
            }
            return -1;
        }
        catch (Exception ex)
        {
            objErrorHandler.ErrorMsg = ex;
            objErrorHandler.CreateLog();
            return -1;
        }
    }
    #endregion

  

  
}
