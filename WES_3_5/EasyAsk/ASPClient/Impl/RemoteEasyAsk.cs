//namespace TradingBell.WebCat.EasyAsk.Impl OLD
//{
//    using TradingBell.WebCat.EasyAsk;
//    using System;
//    using System.Web;

//    public class RemoteEasyAsk : IRemoteEasyAsk
//    {
//        private static readonly string ADVISOR_URI = "EasyAsk/apps/Advisor.jsp";
//        private static readonly string HTTP_PROTOCOL = "http";
//        private static readonly string HTTPS_PROTOCOL = "https";
//        private int m_nPort = -1;
//        private IOptions m_options = null;
//        private string m_sHostName = "";
//        private string m_sProtocol = HTTP_PROTOCOL;
//        private static readonly string ROOT_URI = ADVISOR_URI;

//        public RemoteEasyAsk(string sHostName, int nPort, string dictionary)
//        {
//            this.m_sHostName = sHostName;
//            this.m_nPort = nPort;
//            this.m_options = new Options(dictionary);
//        }

//        private string addNonNullVal(string val)
//        {
//            return ((val != null) ? val : "");
//        }

//        private string addParam(string name, string val)
//        {
//            return (((val != null) && (0 < val.Length)) ? ("&" + name + "=" + val) : "");
//        }

//        private string addTrueParam(string name, bool val)
//        {
//            return (val ? string.Concat(new object[] { "&", name, "=", val }) : "");
//        }

//        private string formBaseURL()
//        {
//            return string.Concat(new object[] { this.m_sProtocol, "://", this.m_sHostName, ":", this.m_nPort, "/", ROOT_URI, "?disp=xml&oneshot=1" });
//        }

//        private string formURL()
//        {
//            return string.Concat(new object[] { this.formBaseURL(), "&dct=", this.m_options.getDictionary(), "&indexed=1&ResultsPerPage=", this.m_options.getResultsPerPage(), this.addParam("defsortcols", this.m_options.getSortOrder()), this.addTrueParam("subcategories", this.m_options.getSubCategories()), this.addTrueParam("rootprods", this.m_options.getToplevelProducts()), this.addTrueParam("navigatehierarchy", this.m_options.getNavigateHierarchy()), this.addTrueParam("returnskus", this.m_options.getReturnSKUs()), this.addParam("defarrangeby", this.m_options.getGrouping()), this.addNonNullVal(this.m_options.getCallOutParam()) });
//        }

//        public IOptions getOptions()
//        {
//            return this.m_options;
//        }

//        public void setOptions(IOptions options)
//        {
//            this.m_options = options;
//        }

//        public INavigateResults userAttributeClick(string path, string attr)
//        {
//            string url = this.formURL() + "&RequestAction=advisor&CatPath=" + HttpUtility.UrlEncode(path) + "&RequestData=CA_AttributeSelected&AttribSel=" + attr;
//            RemoteResults results = new RemoteResults();
//            results.load(url);
//            return results;
//        }
//        public INavigateResults userAttributeClick_Brand(string path, string attr)
//        {
//            string url = this.formURL() + "&RequestAction=advisor&CatPath=" + HttpUtility.UrlEncode(path) + "&RequestData=CA_AttributeSelected&AttribSel=" + HttpUtility.UrlEncode(attr);
//            RemoteResults results = new RemoteResults();
//            results.load(url);
//            return results;
//        }

//        public INavigateResults userBreadCrumbClick(string path)
//        {
//            string url = this.formURL() + "&RequestAction=advisor&CatPath=" + HttpUtility.UrlEncode(path) + "&RequestData=CA_BreadcrumbSelect";
//            RemoteResults results = new RemoteResults();
//            results.load(url);
//            return results;
//        }

//        public INavigateResults userBreadCrumbClick1(String path)
//        {
//            String url = formURL() + "&RequestAction=advisor&CatPath=" + path + "&RequestData=CA_BreadcrumbSelect";
//            RemoteResults res = new RemoteResults();
//            res.load(url);
//            return res;
//        }

//        public INavigateResults userCategoryClick(string path, string cat)
//        {
//            string str = (((path != null) && (0 < path.Length)) ? (path + "/") : "") + cat;
//            string url = this.formURL() + "&RequestAction=advisor&CatPath=" + HttpUtility.UrlEncode(str) + "&RequestData=CA_CategoryExpand";
//            RemoteResults results = new RemoteResults();
//            results.load(url);
//            return results;
//        }
//        public INavigateResults userCategoryClick(string path)
//        {
//            // string str = (((path != null) && (0 < path.Length)) ? (path + "") : "") + cat;
//            string url = this.formURL() + "&RequestAction=advisor&CatPath=" + HttpUtility.UrlEncode(path) + "&RequestData=CA_CategoryExpand";
//            RemoteResults results = new RemoteResults();
//            results.load(url);
//            return results;
//        }
//        public INavigateResults userGoToPage(string path, string pageNumber)
//        {
//            string url = this.formURL() + "&RequestAction=navbar&CatPath=" + HttpUtility.UrlEncode(path) + "&RequestData=page" + pageNumber;
//            RemoteResults results = new RemoteResults();
//            results.load(url);
//            return results;
//        }

//        public INavigateResults userPageOp(string path, string curPage, string pageOp)
//        {
//            string url = this.formURL() + "&RequestAction=navbar&CatPath=" + HttpUtility.UrlEncode(path) + "&RequestData=" + pageOp;
//            if ((curPage != null) && (0 < curPage.Length))
//            {
//                url = url + "&currentpage=" + curPage;
//            }
//            RemoteResults results = new RemoteResults();
//            results.load(url);
//            return results;
//        }

//        public INavigateResults userSearch(string path, string question)
//        {
//            string url = this.formURL() + "&RequestAction=advisor&CatPath=" + HttpUtility.UrlEncode(path) + "&RequestData=CA_Search&q=" + HttpUtility.UrlEncode(question);
//            RemoteResults results = new RemoteResults();
//            results.load(url);
//            return results;
//        }
//    }
//} OLD



namespace TradingBell.WebCat.EasyAsk.Impl
{
    using TradingBell.WebCat.EasyAsk;
    using System;
    using System.Web;
    using System.Net;
    using System.Xml;
    using System.Data;
    using Newtonsoft.Json;
    using System.Runtime.Serialization;
    using TradingBell.WebCat.Helpers;

    public class RemoteEasyAsk : IRemoteEasyAsk
    {
        private static readonly string ADVISOR_URI = "EasyAsk/apps/Advisor.jsp";
        private static readonly string HTTP_PROTOCOL = "http";
        private static readonly string HTTPS_PROTOCOL = "https";
        private int m_nPort = -1;
        private IOptions m_options = null;
        private string m_sHostName = "";
        private string m_sProtocol = HTTP_PROTOCOL;
        private static readonly string ROOT_URI = ADVISOR_URI;

        public RemoteEasyAsk(string sHostName, int nPort, string dictionary)
        {
            this.m_sHostName = sHostName;
            this.m_nPort = nPort;
            this.m_options = new Options(dictionary);
        }

        private string addNonNullVal(string val)
        {
            return ((val != null) ? val : "");
        }

        private string addParam(string name, string val)
        {
            return (((val != null) && (0 < val.Length)) ? ("&" + name + "=" + val) : "");
        }

        private string addTrueParam(string name, bool val)
        {
            return (val ? string.Concat(new object[] { "&", name, "=", val }) : "");
        }

        private string formBaseURL()
        {
            return string.Concat(new object[] { this.m_sProtocol, "://", this.m_sHostName, ":", this.m_nPort, "/", ROOT_URI, "?disp=json&oneshot=1" });
        }

        private string formURL()
        {
            return string.Concat(new object[] { this.formBaseURL(), "&dct=", this.m_options.getDictionary(), "&indexed=1&ResultsPerPage=", this.m_options.getResultsPerPage(), this.addParam("defsortcols", this.m_options.getSortOrder()), this.addTrueParam("subcategories", this.m_options.getSubCategories()), this.addTrueParam("rootprods", this.m_options.getToplevelProducts()), this.addTrueParam("navigatehierarchy", this.m_options.getNavigateHierarchy()), this.addTrueParam("returnskus", this.m_options.getReturnSKUs()), this.addParam("defarrangeby", this.m_options.getGrouping()), this.addNonNullVal(this.m_options.getCallOutParam()) });
        }

        public IOptions getOptions()
        {
            return this.m_options;
        }

        public void setOptions(IOptions options)
        {
            this.m_options = options;
        }

        public INavigateResults userAttributeClick(string path, string attr)
        {
            string url = this.formURL() + "&RequestAction=advisor&CatPath=" + HttpUtility.UrlEncode(path) + "&RequestData=CA_AttributeSelected&AttribSel=" + HttpUtility.UrlEncode(attr);
            //RemoteResults results = new RemoteResults();
            //results.load(url);
            //return results;
            return (urlPostjson(url));
        }
        public INavigateResults userAttributeClick_Brand(string path, string attr)
        {
            string url = this.formURL() + "&RequestAction=advisor&CatPath=" + HttpUtility.UrlEncode(path) + "&RequestData=CA_AttributeSelected&AttribSel=" + HttpUtility.UrlEncode(attr);
            //RemoteResults results = new RemoteResults();
            //results.load(url);
            //return results;
            return (urlPostjson(url));
        }

        public INavigateResults userBreadCrumbClick(string path)
        {
            string url = this.formURL() + "&RequestAction=advisor&CatPath=" + HttpUtility.UrlEncode(path) + "&RequestData=CA_BreadcrumbSelect";
          //  ErrorHandler objErrorhandler = new ErrorHandler();
          //  objErrorhandler.CreateLog(url);
            //RemoteResults results = new RemoteResults();
            //results.load(url);
            //return results;
            return (urlPostjson(url));
        }

        //public INavigateResults userBreadCrumbClick1(String path)
        //{
        //    String url = formURL() + "&RequestAction=advisor&CatPath=" + HttpUtility.UrlEncode(path) + "&RequestData=CA_BreadcrumbSelect";
        //    //RemoteResults res = new RemoteResults();
        //    //res.load(url);
        //    //return res;
        //    return (urlPost(url));
        //}

        public INavigateResults userCategoryClick(string path, string cat)
        {
            string str = (((path != null) && (0 < path.Length)) ? (path + "/") : "") + cat;
            string url = this.formURL() + "&RequestAction=advisor&CatPath=" + HttpUtility.UrlEncode(str) + "&RequestData=CA_CategoryExpand";
            //RemoteResults results = new RemoteResults();
            //results.load(url);
            //return results;
            return (urlPostjson(url));
        }
        public INavigateResults userCategoryClick(string path)
        {
            // string str = (((path != null) && (0 < path.Length)) ? (path + "") : "") + cat;
            string url = this.formURL() + "&RequestAction=advisor&CatPath=" + HttpUtility.UrlEncode(path) + "&RequestData=CA_CategoryExpand";
            //RemoteResults results = new RemoteResults();
            //results.load(url);
            //return results;
            return (urlPostjson(url));
        }
        public INavigateResults userGoToPage(string path, string pageNumber)
        {
            string url = this.formURL() + "&RequestAction=navbar&CatPath=" + HttpUtility.UrlEncode(path) + "&RequestData=page" + pageNumber;
            //RemoteResults results = new RemoteResults();
            //results.load(url);
            //return results;
            return (urlPostjson(url));
        }

        public INavigateResults userPageOp(string path, string curPage, string pageOp)
        {
            string url = this.formURL() + "&RequestAction=navbar&CatPath=" + HttpUtility.UrlEncode(path) + "&RequestData=" + pageOp;
            if ((curPage != null) && (0 < curPage.Length))
            {
                url = url + "&currentpage=" + curPage;
            }
            //RemoteResults results = new RemoteResults();
            //results.load(url);
            //return results;
            return (urlPostjson(url));
        }

        public INavigateResults userSearch(string path, string question)
        {
            //ErrorHandler objErrorhandler = new ErrorHandler();
            //objErrorhandler.CreateLog(path.Length.ToString());
            //path = path.Trim();
            //objErrorhandler.CreateLog(path.Length.ToString());
            string url = this.formURL() + "&RequestAction=advisor&CatPath=" + HttpUtility.UrlEncode(path)+"&RequestData=CA_Search&q=" + HttpUtility.UrlEncode(question);
           
          //  objErrorhandler.CreateLog(url);
            //RemoteResults results = new RemoteResults();
            //results.load(url);
            //return results;
            return (urlPostjson(url));
        }

        public INavigateResults urlPost(String url)
        {
            RemoteResults res = new RemoteResults();

            url = url.Replace("disp=json", "disp=xml");

            WebRequest req = WebRequest.Create(url);
            req.ContentType = "application/x-www-form-urlencoded";
            req.Method = "POST";
            WebResponse resp = req.GetResponse();
            res.load(resp.GetResponseStream());
            return res;
        }

        public INavigateResults urlPostjson(String url)
        {

             
            RemoteResults res = new RemoteResults();
            DataSet dsAdvisor = new DataSet();


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
            if (dsAdvisor != null && dsAdvisor.Tables.Count > 0 && dsAdvisor.Tables["items"] != null && dsAdvisor.Tables["items"].Rows.Count > 0 && dsAdvisor.Tables["datadescription"] != null && dsAdvisor.Tables["datadescription"].Rows.Count > 0)
            {
                foreach (DataRow dr in dsAdvisor.Tables["datadescription"].Rows)
                {
                    if (dsAdvisor.Tables["items"].Columns[dr["tagname"].ToString()] == null)
                    {
                        DataColumn dc = new DataColumn(dr["tagname"].ToString(), typeof(string));
                        dc.DefaultValue = "";
                        dsAdvisor.Tables["items"].Columns.Add(dc);
                    }
                }

            }

            res.SetDBAdvisor(dsAdvisor);


            return res;
        }


    }
}

