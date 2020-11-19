
<%@ Application Language="C#" %>
<%@ Import Namespace ="TradingBell.WebCat.CatalogDB" %>
<%@ Import Namespace ="TradingBell.WebCat.CommonServices" %>
<%@ Import Namespace ="TradingBell.WebCat.Helpers" %>
<%@ Import Namespace ="System.Windows.Forms" %>
 

<script runat="server">
       
    public System.Data.SqlClient.SqlConnection Gcon = new System.Data.SqlClient.SqlConnection();
    //public ErrorHandler objErrorHandler = new ErrorHandler();
   
    void Application_Start(object sender, EventArgs e) 
    {
        // Code that runs on application startup
        Application["WebsiteID"] = 1;
        WES.App_Code.CL.StaticCache.LoadStaticCache();

    }
    
    void Application_End(object sender, EventArgs e) 
    {
        //  Code that runs on application shutdown

    }
        
    void Application_Error(object sender, EventArgs e) 
    { 
        // Code that runs when an unhandled error occurs

    }

    void Session_Start(object sender, EventArgs e) 
    {
        Session["USER_ID"] = "";        
    }

    void Session_End(object sender, EventArgs e) 
    {
        if (Session["pdffile"] != null && Session["pdffile"].ToString() != string.Empty)
        {
            //string filename = Session["PdfFileName"].ToString();
            System.IO.FileInfo filin = new System.IO.FileInfo(Session["pdffile"].ToString());
            filin.Delete();
        }
        HelperServices objHelperServices = new HelperServices();
        if (objHelperServices.CI(Session["USER_ID"]) > 0)
        {
            // Set the User Online Flag to false
            UserServices objUserServices = new UserServices();
            objUserServices.OnLineFlag(false, objHelperServices.CI(Session["USER_ID"]));
           
        }
        //System.Collections.Generic.List<string> keys = new System.Collections.Generic.List<string>();
        //IDictionaryEnumerator enumerator = System.Web.Caching.Cache.GetEnumerator();
        //while (enumerator.MoveNext())
        //{
        //    keys.Add(enumerator.Key.ToString());
        //}
        //for (int i = 0; i < keys.Count; i++)
        //{

        //    Cache.Remove(keys[i]);
        //}


        
    }
  
        //getting root     
    string GetAppRoot(string sRequestedPath)
    {
        return sRequestedPath.Replace(System.IO.Path.GetFileName(sRequestedPath), "");
    }


    private bool CheckCredential()
    {
        bool returnvalue = false;
        bool validUser;
        string username;
        string password;
        string login;
        string requrl = HttpContext.Current.Request.Url.ToString().ToLower();
        int UserID;
        TradingBell.WebCat.Helpers.Security objSecurity = new TradingBell.WebCat.Helpers.Security();
        UserServices objUserServices = new UserServices();
        //TradingBell.WebCat.Helpers.ErrorHandler objErrorHandler = new TradingBell.WebCat.Helpers.ErrorHandler();
        CompanyGroupServices objCompanyGroupServices=new CompanyGroupServices();
        if (requrl.Contains("onlinecatalogue.aspx"))
        {
            return true;
        }
        if (requrl.Contains("resetchangepassword.aspx"))
        {
            return true;
        }
        if (Request.Cookies["LoginInfo"] != null && Request.Cookies["LoginInfo"].Value.ToString().Trim() != "")
        {
            try
            {
                login = "False";              
                HttpCookie LoginInfoCookie = Request.Cookies["LoginInfo"];
                username = objSecurity.StringDeCrypt(LoginInfoCookie["UserName"].ToString());
                password = objSecurity.StringDeCrypt(LoginInfoCookie["Password"].ToString());               
                if (LoginInfoCookie["Login"] != null)
                    login = objSecurity.StringDeCrypt(LoginInfoCookie["Login"].ToString()); 
                validUser = objUserServices.CheckUserName(username);
                UserID = objUserServices.GetUserID(username);
                if (UserID != -1 && username != string.Empty && login == "True")
                {
                    if (objCompanyGroupServices.CheckCompanyStatus(UserID) == CompanyGroupServices.CompanyStatus.ACTIVE.ToString())
                    {
                        if ((validUser))
                        {
                            bool HasAdminUser = objUserServices.HasAdmin(UserID);
                            if (objUserServices.IsUserActive(UserID.ToString() ))
                            {
                                password = objSecurity.StringEnCrypt(password); 
                                if (objUserServices.CheckUser(username, password))
                                {
                                    string Role;
                                    Role = objUserServices.GetRole(UserID);

                                    if (Role != null)
                                    {
                                        returnvalue = true;
                                        objUserServices.OnLineFlag(true, UserID);
                                        Session["USER_NAME"] = username;
                                        Session["USER_ID"] = UserID;
                                        Session["USER_ROLE"] = Role;
                                        Session["COMPANY_ID"] = objUserServices.GetCompanyID(UserID);
                                        Session["CUSTOMER_TYPE"] = objUserServices.GetCustomerType(UserID);
                                    }
                                }
                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {
               // objErrorHandler.ErrorMsg = ex;
               // objErrorHandler.CreateLog();
            }
            
        }
        else
            returnvalue= false;
       

        if (!(returnvalue) && (requrl.Contains("orderdetail")) && (requrl.Contains("popup=true")))
            returnvalue = true;	

        return returnvalue;
    
    }

    protected void Application_BeginRequest(object sender, EventArgs e)
    {

     string lurl = HttpContext.Current.Request.RawUrl.ToString().Replace("\\","") ;
                if ((lurl.Contains(".pdf") && !lurl.ToLower().Contains("attachments/attachments") && lurl.ToLower().Contains("/attachments/")))
               {
                   lurl = lurl.ToLower().Replace("/Attachments/", "/Attachments/attachments/").Replace("/attachments/", "/Attachments/attachments/");
                   Context.RewritePath(lurl);
              }
        //HelperServices objHelperServices = new HelperServices();
        //objHelperServices.writelog(HttpContext.Current.Request.Url.ToString().ToLower());
        //if ((HttpContext.Current.Request.Url.ToString().ToLower().Contains(".pdf") == true
        //      || HttpContext.Current.Request.Url.ToString().ToLower().Contains(".htm") == true
        //      || HttpContext.Current.Request.Url.ToString().ToLower().Contains(".html") == true) &&
        //      HttpContext.Current.Request.Url.ToString().ToLower().Contains("404new.htm") == false)
        //{
        //    string[] localhost = HttpContext.Current.Request.Url.ToString().Split(new string[] { "/" }, StringSplitOptions.None);

        //    string querypath = HttpContext.Current.Request.Url.PathAndQuery.ToString().Replace("/", "\\");
        //    string newurl = HttpUtility.UrlDecode(HttpContext.Current.Request.Url.PathAndQuery.ToString());
        //    if (HttpContext.Current.Request.Url.PathAndQuery.ToString().ToLower().Contains("pdfdownload.aspx") == false)
        //    {
        //       // string newurl = string.Empty;
        //        if (HttpContext.Current.Request.Url.PathAndQuery.ToString().ToLower().Contains("attachments"))
        //        {
        //            newurl = HttpUtility.UrlDecode(HttpContext.Current.Request.Url.PathAndQuery.ToString());
        //        }
        //        else
        //        {
        //            newurl = "/attachments" + HttpUtility.UrlDecode(HttpContext.Current.Request.Url.PathAndQuery.ToString());
        //        }

        //        //HelperServices objHelperServices = new HelperServices();
        //        objHelperServices.writelog(HttpContext.Current.Request.Url.ToString());
        //        objHelperServices.writelog(newurl);
        //        if (HttpContext.Current.Request.Url.PathAndQuery.ToString().ToLower().Contains("http:") == false)
        //        {
        //            Context.RewritePath(newurl);
        //        }
        //    }
        //}
    }

    protected void Application_PreRequestHandlerExecute(object sender, EventArgs e)
      {

        HelperServices objHelperServices = new HelperServices();

        string requrl = HttpContext.Current.Request.Url.ToString().ToLower();
        
          
        //objHelperServices.writelog(HttpContext.Current.Request.Url.ToString().ToLower()); 
        
        TradingBell.WebCat.Helpers.Security objSecurity = new TradingBell.WebCat.Helpers.Security();
        
        if (ConfigurationManager.AppSettings["SSL_ACTIVE"].ToString() == "1")
        {
            if (!Request.IsLocal)
            {

                if ((requrl.Contains("login.aspx")) || (requrl.Contains("newcustomerregistration.aspx")) || (requrl.Contains("dealerregistration.aspx")) || (requrl.Contains("retailerregistration.aspx")) || (requrl.Contains("createanaccount.aspx")) || (requrl.Contains("payonlinecc.aspx")))
                    objSecurity.RedirectSSL(HttpContext.Current, true, true);
                else
                {
                    
                    if (System.Web.HttpContext.Current.Session != null) //if (HttpContext.Current.Session.Count > 0)
                    {
                        if (System.Web.HttpContext.Current.Session["USER_ID"] != null && System.Web.HttpContext.Current.Session["USER_ID"].ToString() != "" && Convert.ToInt32(System.Web.HttpContext.Current.Session["USER_ID"].ToString()) > 0)
                        {
                            
                            objSecurity.RedirectSSL(HttpContext.Current, true, true);
                        }
                        else
                        {
                            objSecurity.RedirectSSL(HttpContext.Current, true, false);
                        }
                    }
                    else
                        objSecurity.RedirectSSL(HttpContext.Current, true, false);

                }

            }
        }
        if (!(requrl.Contains("userbasicinfo.aspx")) && !(requrl.Contains("agencies.aspx")) &&
            !(requrl.Contains("companyprofile.aspx")) && !(requrl.Contains("userprofile.aspx")) &&
            !(requrl.Contains("forgotpassword.aspx")) && !(requrl.Contains("confirmmessage.aspx")) &&
            !(requrl.Contains("aboutus.aspx")) && !(requrl.Contains("contactus.aspx")) &&
            !(requrl.Contains("newcustomerregistration.aspx")) && !(requrl.Contains("existcustomerregistration.aspx")) &&
            !(requrl.Contains("dealerregistration.aspx")) && !(requrl.Contains("retailerregistration.aspx")) &&
            !(requrl.Contains("activation.aspx"))
            && !(requrl.Contains("makeopenordermail.aspx"))
            
            && !(requrl.Contains("termsandconditions.aspx")) && !(requrl.Contains("resetpassword.aspx")) && !(requrl.Contains("forgotusername.aspx")) && !(requrl.Contains("createanaccount.aspx"))
            && !(requrl.Contains("gblwebmethods.aspx"))
            && !(requrl.Contains("resetchangepassword.aspx"))
             && !(requrl.Contains("autosuggestions.aspx"))
            && !(requrl.Contains("popup=true"))
            && !(requrl.Contains("eahomeindex")))  
            
        {
            if (ConfigurationManager.AppSettings["REQUIRE_LOGIN"].ToString() == "YES")
            {
                if ((requrl.Contains(".aspx")) && HttpContext.Current.Session != null && HttpContext.Current.Session["USER_ID"] != null)
                {
                    if (HttpContext.Current.Session["USER_ID"].ToString() != string.Empty && Convert.ToInt32(HttpContext.Current.Session["USER_ID"].ToString()) > 0)
                    {
                        string sRequestedPath, sPageExtention = "";
                        //HelperServices objHelperServices = new HelperServices();
                        sRequestedPath = HttpContext.Current.Request.Path.ToLower();
                        string _sAppRoot = GetAppRoot(sRequestedPath);
                        //if (sRequestedPath.Contains(".aspx"))
                        //    Session["PageUrl"] = sRequestedPath.Substring(sRequestedPath.IndexOf("/", 1) + 1).ToString();
                        // generating relevant url
                        if (sRequestedPath.IndexOf("c-", StringComparison.InvariantCultureIgnoreCase) > -1)
                        {
                            sPageExtention = Convert.ToString(objHelperServices.GetOptionValues("PAGE EXTENTION"));
                            if (sRequestedPath.EndsWith(sPageExtention))
                            {
                                string[] sArrCustomUrlParts = sRequestedPath.Substring(sRequestedPath.LastIndexOf("/")).Split('-');
                                string sCatId = sArrCustomUrlParts[1].ToString();
                                sRequestedPath = sRequestedPath.Substring(0, sRequestedPath.LastIndexOf("/"));
                                sRequestedPath = sRequestedPath + "/home.aspx?CatID=" + sCatId;
                                Context.RewritePath(sRequestedPath, false);
                            }
                        }
                        else if (sRequestedPath.IndexOf("f-", StringComparison.InvariantCultureIgnoreCase) > -1)
                        {
                            sPageExtention = Convert.ToString(objHelperServices.GetOptionValues("PAGE EXTENTION"));
                            if (sRequestedPath.EndsWith(sPageExtention))
                            {
                                string[] sArrCustomUrlParts = sRequestedPath.Substring(sRequestedPath.LastIndexOf("/")).Split('-');
                                string sCatId = sArrCustomUrlParts[1].ToString();
                                sRequestedPath = sRequestedPath.Substring(0, sRequestedPath.LastIndexOf("/"));
                                sRequestedPath = sRequestedPath + "/family.aspx?Cat=" + sCatId;
                                Context.RewritePath(sRequestedPath, false);
                            }
                        }
                    }
                    else if ((requrl.Contains(".aspx")) && !(requrl.Contains("login.aspx")) && !(requrl.Contains("ResetChangePassword.aspx")) && !(requrl.Contains("OnlineCatalogue.aspx")))
                    {
                        
                        if (HttpContext.Current.Session == null || HttpContext.Current.Session["USER_ID"] == null || HttpContext.Current.Session["USER_ID"].ToString() == string.Empty)
                            if (HttpContext.Current.Session["USER_ID"].ToString() == string.Empty || Convert.ToInt32(HttpContext.Current.Session["USER_ID"].ToString()) < 1)
                            {
                                HttpContext.Current.Session["PageUrl"] = HttpContext.Current.Request.Url.AbsoluteUri;
                                if (!(CheckCredential()) )
                                {
                                 
                                   string strActionType = objSecurity.StringEnCrypt("RequireLogin");
                                   HttpContext.Current.Response.Redirect("login.aspx?ActionType=" + strActionType);
                                   //if (CheckCredential() == false && HttpContext.Current.Request.Url.ToString().ToLower().Contains("home.aspx") == true)
                                   //    HttpContext.Current.Response.Redirect("login.aspx");
                                   //else
                                   //{
                                   //    string strActionType = objSecurity.StringEnCrypt("RequireLogin");
                                   //    HttpContext.Current.Response.Redirect("login.aspx?ActionType=" + strActionType);
                                   //    //HttpContext.Current.Response.Redirect("login.aspx");
                                   //}
                                }
                               
                            }
                    }

                }
                else if ((requrl.Contains(".aspx")) && (requrl.Contains("login.aspx")))
                {
                   
                    if (HttpContext.Current.Session == null || HttpContext.Current.Session["USER_ID"] == null || HttpContext.Current.Session["USER_ID"].ToString() == string.Empty)
                        if (HttpContext.Current.Session["USER_ID"] == string.Empty || Convert.ToInt32(HttpContext.Current.Session["USER_ID"].ToString()) < 1)
                        {                            
                            if ((CheckCredential()))
                                HttpContext.Current.Response.Redirect("home.aspx");

                        }
                }
               

            /*                else if (HttpContext.Current.Request.Url.ToString().ToLower().Contains("cataloguedownload.aspx") == true && !HttpContext.Current.Request.Url.ToString().ToLower().Contains("cataloguedownload.aspx"))
                            {
                                HttpContext.Current.Response.Redirect("login.aspx?URL=cataloguedownload.aspx");
                            }*/
                //Modified by:indu
                //else if (HttpContext.Current.Request.Url.ToString().ToLower().Contains("attachments") == true && !HttpContext.Current.Request.Url.ToString().ToLower().Contains("cataloguedownload.aspx"))
                //// else if (HttpContext.Current.Request.Url.ToString().ToLower().Contains("/media/") == true && !HttpContext.Current.Request.Url.ToString().ToLower().Contains("cataloguedownload.aspx"))
                //{
                //    if (HttpContext.Current.Request.UrlReferrer == null)
                //        HttpContext.Current.Response.Redirect("../login.aspx?URL=cataloguedownload.aspx");
                //}
                else if ((requrl.Contains(".aspx")) && !(requrl.Contains("login.aspx")))
                {
                    {
                        try
                        {
                            HttpContext.Current.Session["PageUrl"] = HttpContext.Current.Request.Url.AbsoluteUri;

                        }
                        catch
                        {
                            HttpContext.Current.Response.Redirect("404New.htm");
                        }
                        if (!(CheckCredential()))
                        {
                            string strActionType = objSecurity.StringEnCrypt("RequireLogin");
                            HttpContext.Current.Response.Redirect("login.aspx?ActionType=" + strActionType);
                        }
                    }
                }
                //else 
                //{

                //    if (HttpContext.Current.Session == null || HttpContext.Current.Session["USER_ID"] == null || HttpContext.Current.Session["USER_ID"].ToString() == string.Empty)
                //    {
                //        HttpContext.Current.Session["USER_ID"] = "0";
                //        if (HttpContext.Current.Session["USER_ID"] == string.Empty || Convert.ToInt32(HttpContext.Current.Session["USER_ID"].ToString()) < 1)
                //        {
                //            if (CheckCredential() == true)
                //                HttpContext.Current.Response.Redirect("home.aspx");

                //        }
                //    }
                //}    
               
            }
        }
          
    }
</script>
