
<%@ Application Language="C#" %>
<%@ Import Namespace ="TradingBell.WebCat.CatalogDB" %>
<%@ Import Namespace ="TradingBell.WebCat.CommonServices" %>
<%@ Import Namespace ="TradingBell.WebCat.Helpers" %>
<%@ Import Namespace ="System.Windows.Forms" %>
 

<script runat="server">
       
    public System.Data.SqlClient.SqlConnection Gcon = new System.Data.SqlClient.SqlConnection();
    
   
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
        try
        {
            HelperServices objHelperServices = new HelperServices();
            bool validUser;
            string username;
            string password;
            string login;
            string requrl = HttpContext.Current.Request.Url.ToString().ToLower();
            int UserID;
            TradingBell.WebCat.Helpers.Security objSecurity = new TradingBell.WebCat.Helpers.Security();
            UserServices objUserServices = new UserServices();
            //TradingBell.WebCat.Helpers.ErrorHandler objErrorHandler = new TradingBell.WebCat.Helpers.ErrorHandler();
            CompanyGroupServices objCompanyGroupServices = new CompanyGroupServices();
            if (requrl.Contains("onlinecatalogue.aspx") || requrl.Contains("onlinecatalogue_price.aspx") || (requrl.Contains("onlinecatalogue_price.aspx") || (requrl.Contains("/wesnews/") )&& !string.IsNullOrEmpty(Request.QueryString["UserId"])))
            {
                return true;
            }
            if (requrl.Contains("resetchangepassword.aspx"))
            {
               // objHelperServices.writelog("inside resetpa");
                return true;
            }
            //Added by smith to autologin from wagner
            if (!string.IsNullOrEmpty(Request.QueryString["username"]) && !string.IsNullOrEmpty(Request.QueryString["password"]) && !string.IsNullOrEmpty(Request.QueryString["login"]))
            {
                try
                {
                    login = "False";
                    username = objSecurity.StringDeCrypt(Request.QueryString["username"].ToString().Replace("%2b", "+"));
                    password = objSecurity.StringDeCrypt(Request.QueryString["password"].ToString());
                    if (Request.QueryString["login"].ToString() != null)
                        login = objSecurity.StringDeCrypt(Request.QueryString["login"].ToString());
                    validUser = objUserServices.CheckUserName(username);
                    UserID = objUserServices.GetUserID(username);


                    if (UserID != -1 && username != string.Empty && login == "True")
                    {
                        if (objCompanyGroupServices.CheckCompanyStatus(UserID) == CompanyGroupServices.CompanyStatus.ACTIVE.ToString())
                        {
                            if ((validUser))
                            {
                                bool HasAdminUser = objUserServices.HasAdmin(UserID);
                                if (objUserServices.IsUserActive(UserID.ToString()))
                                {
                                    //password = objSecurity.StringEnCrypt(password);
                                    if (objUserServices.CheckUser(username, password))
                                    {
                                        string Role;
                                        Role = objUserServices.GetRole(UserID);

                                        if (Role != null)
                                        {
                                           
                                           
                                         
                                            //Session["USER_ID"] = "";
                                            returnvalue = true;
                                            objUserServices.OnLineFlag(true, UserID);
                                            Session["USER_NAME"] = username;
                                            Session["USER_ID"] = UserID;
                                            Session["USER_ROLE"] = Role;
                                            Session["COMPANY_ID"] = objUserServices.GetCompanyID(UserID);
                                            Session["CUSTOMER_TYPE"] = objUserServices.GetCustomerType(UserID);
                                            Session["AUTOLOGIN"] = "1";
                                            HttpCookie LoginInfoCookie = Request.Cookies["LoginInfo"];
                                            if (LoginInfoCookie != null && LoginInfoCookie["Password"] != null)
                                                LoginInfoCookie["Password"] = "";

                                            Response.Cookies["LoginInfo"].Expires = DateTime.Now.AddDays(-665);
                                            Response.Redirect("home.aspx", false);
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
            else if (Request.Cookies["LoginInfo"] != null && Request.Cookies["LoginInfo"].Value.ToString().Trim() != "")
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
                                if (objUserServices.IsUserActive(UserID.ToString()))
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
                returnvalue = false;


            if (!(returnvalue) && (requrl.Contains("orderdetail")) && (requrl.Contains("popup=true")))
                returnvalue = true;
        }
        catch (Exception ex)
        { 
        
        }
        return returnvalue;
    
    }

    protected void Application_BeginRequest(object sender, EventArgs e)
    {

     string lurl = HttpContext.Current.Request.RawUrl.ToString().Replace("\\","") ;
     HelperServices objHelperServices = new HelperServices();
   //objHelperServices.writelog("lurl----"+lurl);
     if (((lurl.Contains(".pdf") || lurl.Contains(".zip"))  && !lurl.ToLower().Contains("attachments/attachments") && lurl.ToLower().Contains("/attachments/")))
               {
                   lurl = lurl.ToLower().Replace("/Attachments/", "/Attachments/attachments/").Replace("/attachments/", "/Attachments/attachments/");
                   Context.RewritePath(lurl);
              }
             

     if (lurl.ToLower() == "/ubiquiti" || lurl.ToLower() == "/ubiquiti/")
     {
         //HelperServices objHelperServices = new HelperServices();
       // objHelperServices.writelog(lurl);
         Context.RewritePath("/product_list.aspx?&id=0&pcr=WES-05E&cid=UBIQUITI&tsb=&tsm=&searchstr=&type=Category&value=Ubiquiti&bname=&byp=0&Path=K6WOvrM7yDGJsbKYFM1l%2b53cP8HUT0Dxg%2bPOAn2YvT0YlbMD8sZ%2bAYpvXfTb5WhveYOIYN2kI1k%3d");
     }
     else if (lurl.Contains("/dlink"))
    {
        //string dlinkurl = ConfigurationManager.AppSettings["DLINKURL"].ToString();
        Context.RewritePath("/product_list.aspx?&id=0&pcr=WES-05E&cid=WES2007095940001N&tsb=&tsm=&searchstr=&type=Category&value=D-Link&bname=&byp=0&Path=K6WOvrM7yDGJsbKYFM1l%2b53cP8HUT0Dxg%2bPOAn2YvT0MIR57wXzlJw7XrDMQVFfSeYOIYN2kI1k%3d");
    }
     else if (lurl.ToLower() == "/wesnews" || lurl.ToLower() == "/wesnews/")
     {
         Context.RewritePath("/OnlineCatalogue_price.aspx?tab=wes_news");
     }
     else if (lurl.ToLower() == "/sba" || lurl.ToLower() == "/sba/")
     {
         // HelperServices objHelperServices = new HelperServices();
         //objHelperServices.writelog(lurl);

         Context.RewritePath("/product_list.aspx?id=0&cid=WES-04D&byp=0&type=Category&value=SB+Acoustics&bname=&searchstr=&path=K6WOvrM7yDGJsbKYFM1l%2b53cP8HUT0Dxg%2bPOAn2YvT1VRAenDWMFTJG3Hh1cs5QM3RsHvH%2fahuCi%2bRJ6eB%2fqm2Zz7UGldABM1XGrjv0rN5pSh0g3xibgzg%3d%3d");

     }
     else if (lurl.ToLower() == "/scanspeak" || lurl.ToLower() == "/scanspeak/")
     {
         Context.RewritePath("/product_list.aspx?&id=0&pcr=WES-04D&cid=WES10935N&tsb=&tsm=&searchstr=&type=Category&value=Scan-Speak&bname=&byp=&Path=K6WOvrM7yDGJsbKYFM1l%2b53cP8HUT0Dxg%2bPOAn2YvT1VRAenDWMFTJG3Hh1cs5QM3RsHvH%2fahuCi%2bRJ6eB%2fqm0x71HMQs1cG");

     }
     else if (lurl.ToLower() == "/koss" || lurl.ToLower() == "/koss/")
     {
         Context.RewritePath("/product_list.aspx?&id=0&pcr=WES-04D&cid=WN721HPH&tsb=&tsm=&searchstr=&type=Category&value=Koss+Headphones&bname=&byp=&Path=K6WOvrM7yDGJsbKYFM1l%2b53cP8HUT0Dxg%2bPOAn2YvT1VRAenDWMFTJG3Hh1cs5QM3RsHvH%2fahuD21qL%2fpnaGzzsotUSxEiS7");

     }
     else if (lurl.ToLower() == "/info/casio" || lurl.ToLower() == "/info/casio/")
     {
      Response.Redirect("/powersearch.aspx?&srctext=Casio%20LED%20Projectors");

     }
     else if (lurl.ToLower() == "/accento" || lurl.ToLower() == "/accento/" || lurl == "/product_list.aspx?&id=0&pcr=WES-04D&cid=WES995J&tsb=&tsm=&searchstr=&type=Category&value=Accento+Dynamica&bname=&byp=&Path=K6WOvrM7yDGJsbKYFM1l+53cP8HUT0Dxg+POAn2YvT1VRAenDWMFTJG3Hh1cs5QM3RsHvH/ahuCi+RJ6eB/qm8EG29a+PAivVKNetVQzQgB5g4hg3aQjWQ==")
     {
         //objHelperServices.writelog("url=" + lurl);

         Context.RewritePath("/product_list.aspx?&id=0&pcr=WES-04D&cid=WES995J&tsb=&tsm=&searchstr=&type=Category&value=Accento+Dynamica&bname=&byp=2&Path=K6WOvrM7yDGJsbKYFM1l%2b53cP8HUT0Dxg%2bPOAn2YvT1VRAenDWMFTJG3Hh1cs5QM3RsHvH%2fahuCi%2bRJ6eB%2fqm8EG29a%2bPAivVKNetVQzQgB5g4hg3aQjWQ%3d%3d");
     }
     else if (lurl.ToLower() == "/elsafe" || lurl.ToLower() == "/elsafe/" || lurl == "/product_list.aspx?&id=0&pcr=WES-01A&cid=WES5023J&tsb=&tsm=&searchstr=&type=Category&value=Elsafe+Install&bname=&byp=0&Path=K6WOvrM7yDGJsbKYFM1l+53cP8HUT0Dxg+POAn2YvT3/axCH8UYK12zWnxguXmSRoHicdHZeLVo=")
     {
         objHelperServices.writelog("url=" + lurl);

         Context.RewritePath("/product_list.aspx?&id=0&pcr=WES-01A&cid=WES5023J&tsb=&tsm=&searchstr=&type=Category&value=Elsafe+Install&bname=&byp=0&Path=K6WOvrM7yDGJsbKYFM1l%2b53cP8HUT0Dxg%2bPOAn2YvT3%2faxCH8UYK12zWnxguXmSRoHicdHZeLVo%3d");

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
        
 	 if (requrl.Contains(".aspx"))
        {
            string ip;
            ip = Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            if (ip == "" || ip == null)
                ip = Request.ServerVariables["REMOTE_ADDR"];
            objHelperServices.writelog("URL : " + requrl + "   ip :  "+ip);
        }
          
   //   objHelperServices.writelog(HttpContext.Current.Request.Url.ToString().ToLower()); 
        
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
                    //else
                    //    objSecurity.RedirectSSL(HttpContext.Current, true, false);

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
            && !(requrl.Contains("termsandconditions.aspx")) && !(requrl.Contains("resetpassword.aspx")) && !(requrl.Contains("forgotusername.aspx")) && !(requrl.Contains("createanaccount.aspx"))
            && !(requrl.Contains("gblwebmethods.aspx"))
            && !(requrl.Contains("resetchangepassword.aspx"))
             && !(requrl.Contains("autosuggestions.aspx"))
            && !(requrl.Contains("popup=true"))
            && !(requrl.Contains("eahomeindex"))
            && !(requrl.Contains("makepaymentmail_wes.aspx"))
               && !(requrl.Contains("ipnhandler.aspx"))
                && !(requrl.Contains("ipnhandler.aspx"))
 && !(requrl.Contains("onlinecatalogue_price.aspx"))
  && !(requrl.Contains("orderpickready_mail.aspx"))
)  
            
        {
            //objHelperServices.writelog("inside 1"); 
            
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
                        //objHelperServices.writelog("inside 2"); 
                        if (HttpContext.Current.Session == null || HttpContext.Current.Session["USER_ID"] == null || HttpContext.Current.Session["USER_ID"].ToString() == string.Empty)
                            if (HttpContext.Current.Session["USER_ID"].ToString() == string.Empty || Convert.ToInt32(HttpContext.Current.Session["USER_ID"].ToString()) < 1)
                            {
                                HttpContext.Current.Session["PageUrl"] = HttpContext.Current.Request.Url.AbsoluteUri;
                             
                                
                                if (!(CheckCredential()) )
                                {
                                   // objHelperServices.writelog("inside 3"); 
                                   string strActionType = objSecurity.StringEnCrypt("RequireLogin");
                                     string lurl = HttpContext.Current.Request.RawUrl.ToString().Replace("\\","") ;
   

     if (lurl.ToLower() == "/ubiquiti")
     {
        // objHelperServices.writelog(HttpContext.Current.Request.Url.AbsoluteUri);
         HttpContext.Current.Session["PageUrl"] = "/ubiquiti";
                             
                                
        
     }
     else if (lurl.ToLower() == "/dlink")
     {
        // objHelperServices.writelog(HttpContext.Current.Request.Url.AbsoluteUri);
         HttpContext.Current.Session["PageUrl"] = "/dlink";
     }
     else if (lurl.ToLower() == "/sba")
     {
         // HelperServices objHelperServices = new HelperServices();
         // objHelperServices.writelog(lurl);

         HttpContext.Current.Session["PageUrl"] = "/sba";
     }
     else if (lurl.ToLower() == "/scanspeak")
     {
         HttpContext.Current.Session["PageUrl"] = "/scanspeak";
     }
     else if (lurl.ToLower() == "/koss")
     {
         HttpContext.Current.Session["PageUrl"] = "/koss";
     }
     else if (lurl.ToLower() == "/accento" || lurl == "/product_list.aspx?&id=0&pcr=WES-04D&cid=WES995J&tsb=&tsm=&searchstr=&type=Category&value=Accento+Dynamica&bname=&byp=&Path=K6WOvrM7yDGJsbKYFM1l+53cP8HUT0Dxg+POAn2YvT1VRAenDWMFTJG3Hh1cs5QM3RsHvH/ahuCi+RJ6eB/qm8EG29a+PAivVKNetVQzQgB5g4hg3aQjWQ==")
     {
         HttpContext.Current.Session["PageUrl"] = "/accento";
     }
     else if (lurl.ToLower() == "/elsafe" || lurl == "/product_list.aspx?&id=0&pcr=WES-01A&cid=WES5023J&tsb=&tsm=&searchstr=&type=Category&value=Elsafe+Install&bname=&byp=0&Path=K6WOvrM7yDGJsbKYFM1l+53cP8HUT0Dxg+POAn2YvT3/axCH8UYK12zWnxguXmSRoHicdHZeLVo=")
     {
         HttpContext.Current.Session["PageUrl"] = "/elsafe";
     }                                              
                                       
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
