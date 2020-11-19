using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TradingBell.Common;
using TradingBell.WebServices;
using TradingBell5.CatalogX;
using System.Data.SqlClient;
using StringTemplate = Antlr.StringTemplate.StringTemplate;
using StringTemplateGroup = Antlr.StringTemplate.StringTemplateGroup;
using TradingBell.WebCat.CatalogDB;
using TradingBell.WebCat.Helpers;
using TradingBell.WebCat.EasyAsk;
using TradingBell.WebCat.TemplateRender;
using TradingBell.WebCat.CommonServices;
public partial class UC_browsebybrandWES : System.Web.UI.UserControl
{
    #region "Declarations"
    ErrorHandler oErr;
    Category oCat;
    ProductFamily oPF;
    ProductRender oPR;
    HelperDB oHelper;
    
    ConnectionDB conStr = new ConnectionDB();
    string _TvrSkin;
    //String _CategoryText;
    int _CatalogId=1;
    string _ImagePath;
    string _CategoryLogo;
    string _FamilyLogo;
    string _DisplayCategoryMode = "FLAT";
    string _DisplayFamilyCount="NO";
    string _DisplayProductCount="YES";
    string _DisplayFamilyLogo="NO";
    string _DisplayCategoryLogo="NO";
    string _NaviWidth;
    string _NaviHeight;
    string _NodeExpanded;
    string _CategoryHeaderText;
    string _HeaderCssClass;
    string MCID = "";
    string CID = "";
    string valuepath = "";
    string ParentCatID = "";
    string stemplatepath = "";
    string tempCID = "";
    string tempCName = "";
    int ictrecords = 0;
    EasyAsk_WES EasyAsk = new EasyAsk_WES();    
 
    #endregion

    protected void Page_Load(object sender, EventArgs e)
    {
        oHelper = new HelperDB();
        oErr = new ErrorHandler();
        //oCat = new Category();
        oPF = new ProductFamily();
        oPR = new ProductRender();
        //TVBrand.ID = "TVBrand";
        System.Web.UI.WebControls.TreeNode parentNode;

        stemplatepath = Server.MapPath(ConfigurationManager.AppSettings["StringTemplatePath"].ToString());

             

        if (Request.QueryString["cid"] != null && Request.QueryString["cid"].ToString() != "" && Request.QueryString["tsb"] != null && Request.QueryString["tsb"].ToString() != "" && Request.QueryString["tsm"] != null && Request.QueryString["tsm"].ToString() != "" )
        {           
            valuepath = Request.QueryString["cid"].ToString() + ">" + Server.UrlDecode(Request.QueryString["tsb"].ToString()) + ">" + Server.UrlDecode(Request.QueryString["tsm"].ToString());
           
        }
        else if (Request.QueryString["cid"] != null && Request.QueryString["cid"].ToString() != "" && Request.QueryString["tsb"] != null && Request.QueryString["tsb"].ToString() != "" )
        {          
            
            valuepath = Request.QueryString["cid"].ToString() + ">" + Server.UrlDecode(Request.QueryString["tsb"].ToString());
            
        }
        if (Request.QueryString["cid"] != null)
        {
            ParentCatID = Request.QueryString["cid"];
        }
       
    }


    //protected void TVBrand_SelectedNodeChanged(object sender, EventArgs e)
    //{
    //    //PopulateNode(sender,e);

    //    if (TVBrand.SelectedNode.Depth == 0)
    //    {
    //        //GetCategories(TVBrand.SelectedNode);

    //    }
    //    //else
    //    //{
    //    //    GetSubcategory(TVBrand.SelectedNode);
    //    //}

    //}



    #region "Functions"
   
    //protected void PopulateNode(object sender, TreeNodeEventArgs e)
    //{
    //    if (e.Node.Depth == 0)
    //    {
    //        GetCategories(e.Node);
    //    }
    //    //else
    //    //{
    //    //    GetSubcategory(e.Node);
    //    //}
    //}
    
    //protected void ConstructRootTree(object sender, EventArgs e)
    //{
    //    if (TVBrand.Nodes.Count == 0)
    //    {
    //        string[] IDS = MCID.Split('>');
    //        string valuepath = "";
         
    //        if ((Request.QueryString["cid"] != null && Request.QueryString["cid"].ToString() !="List all models")   || CID.ToString() != "")
    //        {
    //            if (Request.QueryString["cid"] != null )                
    //            {
                    
    //                string CateID = "";
    //                if (Request.QueryString["pcr"] != null && Request.QueryString["pcr"].ToString() != "")
    //                {
    //                    CateID = Request.QueryString["pcr"].ToString();
    //                }
    //                else
    //                {
    //                    CateID = Request.QueryString["cid"].ToString();
    //                }

    //                //if (CateID == "WES210582")//hard  coded
    //                //{
    //                    DataSet dsCat = new DataSet();
    //                    //dsCat = oCat.GetWESBrand(CateID, Convert.ToInt32(oHelper.GetOptionValues("DEFAULT CATALOG").ToString()));
    //                    //DataTable dt=EasyAsk.GetMainMenuClickDetail(CateID, "Brand");
    //                    DataTable dt = (DataTable)((DataSet)HttpContext.Current.Session["MainMenuClick"]).Tables["Brand"];
                    
    //                    if (dt == null)
    //                        dsCat = null;
    //                    else
    //                        dsCat.Tables.Add(dt.Copy());   
         
    //                    if (dsCat != null)
    //                    {
    //                        foreach (DataRow rSCat in dsCat.Tables[0].Rows)
    //                        {
    //                            #region brandOld
    //                            //if ((rSCat["CATEGORY_NAME"].ToString().ToUpper() != "DEFAULT CATEGORY") && (rSCat["CATEGORY_NAME"].ToString() != "General Category"))
    //                            //{
    //                            //    TreeNode newNode = new TreeNode(rSCat["CATEGORY_NAME"].ToString(), CateID + ">" + rSCat["CATEGORY_ID"].ToString());
    //                            //    if (!(oCat.GetSubCategoriesL2CountBrandWES(CateID, Convert.ToInt32(oHelper.GetOptionValues("DEFAULT CATALOG").ToString()), rSCat["CATEGORY_ID"].ToString()) > 0))
    //                            //    {
    //                            //        newNode.NavigateUrl = "~/bybrand.aspx?&ld=0&cid=" + CateID + "&sl2=" + rSCat["CATEGORY_ID"].ToString() + "&pcid=" + CateID + "&byp=2";
    //                            //    }
    //                            //    else
    //                            //    {
    //                            //        newNode.NavigateUrl = "~/categorylist.aspx?&ld=0&cid=" + CateID + "&sl2=" + rSCat["CATEGORY_ID"].ToString() + "&byp=2&bypcat=1";
    //                            //        newNode.PopulateOnDemand = true;
    //                            //    }
    //                            //    TreeNode parentNode;

    //                            //    if (Request.QueryString["cid"] != null && Request.QueryString["cid"].ToString() != "" && Request.QueryString["sl2"] != null && Request.QueryString["sl2"].ToString() != "")
    //                            //    {
    //                            //        parentNode = new TreeNode();
    //                            //        valuepath = Request.QueryString["cid"].ToString() + ">" + Request.QueryString["sl2"].ToString();
    //                            //        parentNode = TVBrand.FindNode(valuepath);
    //                            //        if (parentNode != null)
    //                            //        {
    //                            //            parentNode.Expand();
    //                            //            parentNode.Text = "<div style='color:Black'>" + parentNode.Text + "</div>";
    //                            //            parentNode.Selected = true;

    //                            //        }
    //                            //    }
    //                            //    if (Request.QueryString["cid"] != null && Request.QueryString["cid"].ToString() != "" && Request.QueryString["sl2"] != null && Request.QueryString["sl2"].ToString() != "" && Request.QueryString["sl3"] != null && Request.QueryString["sl3"].ToString() != "")
    //                            //    {
    //                            //        parentNode = new TreeNode();
    //                            //        valuepath = Request.QueryString["cid"].ToString() + ">" + Request.QueryString["sl2"].ToString() + ">" + Request.QueryString["sl3"].ToString();
    //                            //        parentNode = TVBrand.FindNode(valuepath);
    //                            //        if (parentNode != null)
    //                            //        {
    //                            //            parentNode.Expand();
    //                            //            parentNode.Text = "<div style='color:Black'>" + parentNode.Text + "</div>";
    //                            //            parentNode.Selected = true;

    //                            //        }
    //                            //    }
    //                            //    if (Request.QueryString["cid"] != null && Request.QueryString["cid"].ToString() != "" && Request.QueryString["sl2"] != null && Request.QueryString["sl2"].ToString() != "" && Request.QueryString["sl3"] != null && Request.QueryString["sl3"].ToString() != "" && Request.QueryString["sl4"] != null && Request.QueryString["sl4"].ToString() != "")
    //                            //    {
    //                            //        parentNode = new TreeNode();
    //                            //        valuepath = Request.QueryString["cid"].ToString() + ">" + Request.QueryString["sl2"].ToString() + ">" + Request.QueryString["sl3"].ToString() + ">" + Request.QueryString["sl4"].ToString();
    //                            //        parentNode = TVBrand.FindNode(valuepath);
    //                            //        if (parentNode != null)
    //                            //        {
    //                            //            parentNode.Expand();
    //                            //            parentNode.Text = "<div style='color:Black'>" + parentNode.Text + "</div>";
    //                            //            parentNode.Selected = true;
    //                            //        }

    //                            //    }

    //                            //    newNode.SelectAction = TreeNodeSelectAction.Expand;
    //                            //    newNode.Text = "<div style='border-bottom-width: 1px; border-bottom-style: solid; border-bottom-color:#d1d1d1; width:100%;cursor: hand;'>" + newNode.Text + "</div>";
    //                            //    TVBrand.Nodes.Add(newNode);
    //                            //}
    //                            #endregion

    //                                TreeNode newNode = new TreeNode(rSCat["TOSUITE_BRAND"].ToString(),CateID + ">" + rSCat["TOSUITE_BRAND"].ToString());
    //                                //if (!(oCat.GetWESModelcount(CateID, Convert.ToInt32(oHelper.GetOptionValues("DEFAULT CATALOG").ToString()), rSCat["TOSUITE_BRAND"].ToString()) > 0))
    //                                //{
    //                                //    newNode.NavigateUrl = "~/bybrand.aspx?&ld=0&cid=" + CateID + "&tsb=" + Server.UrlEncode(rSCat["TOSUITE_BRAND"].ToString()) + "&pcid=" + CateID + "&byp=2";
    //                                //}
    //                                //else
    //                                //{
    //                                    newNode.NavigateUrl = "~/categorylist.aspx?&ld=0&cid=" + CateID + "&tsb=" + Server.UrlEncode(rSCat["TOSUITE_BRAND"].ToString()) + "&byp=2&bypcat=1";
    //                                    newNode.PopulateOnDemand = true;
    //                                //}
    //                                newNode.SelectAction = TreeNodeSelectAction.Expand;
    //                                newNode.Text = "<div style='border-bottom-width: 1px; border-bottom-style: solid; border-bottom-color:#d1d1d1; width:100%;cursor: hand;'>" + newNode.Text + "</div>";
    //                                TVBrand.Nodes.Add(newNode);
    //                                // Jtech Mohan
    //                                //if (Request.QueryString["cid"] != null && Request.QueryString["cid"].ToString() != "" && Request.QueryString["tsb"] != null && Request.QueryString["tsb"].ToString() != "")
    //                                //{
    //                                //    parentNode = new TreeNode();
    //                                //    valuepath = Request.QueryString["cid"].ToString() + ">" + Server.UrlDecode(Request.QueryString["tsb"].ToString());
    //                                //    parentNode = TVBrand.FindNode(valuepath);
    //                                //    if (parentNode != null)
    //                                //    {
    //                                //        parentNode.Expand();
    //                                //        parentNode.Text = "<div style='color:Black'>" + parentNode.Text + "</div>";
    //                                //        parentNode.Selected = true;

    //                                //    }
    //                                //}
    //                                // Jtech Mohan
    //                                //if (Request.QueryString["cid"] != null && Request.QueryString["cid"].ToString() != "" && Request.QueryString["sl2"] != null && Request.QueryString["sl2"].ToString() != "" && Request.QueryString["sl3"] != null && Request.QueryString["sl3"].ToString() != "")
    //                                //{
    //                                //    parentNode = new TreeNode();
    //                                //    valuepath = Request.QueryString["cid"].ToString() + ">" + Request.QueryString["sl2"].ToString() + ">" + Request.QueryString["sl3"].ToString();
    //                                //    parentNode = TVBrand.FindNode(valuepath);
    //                                //    if (parentNode != null)
    //                                //    {
    //                                //        parentNode.Expand();
    //                                //        parentNode.Text = "<div style='color:Black'>" + parentNode.Text + "</div>";
    //                                //        parentNode.Selected = true;

    //                                //    }
    //                                //}
    //                                //if (Request.QueryString["cid"] != null && Request.QueryString["cid"].ToString() != "" && Request.QueryString["sl2"] != null && Request.QueryString["sl2"].ToString() != "" && Request.QueryString["sl3"] != null && Request.QueryString["sl3"].ToString() != "" && Request.QueryString["sl4"] != null && Request.QueryString["sl4"].ToString() != "")
    //                                //{
    //                                //    parentNode = new TreeNode();
    //                                //    valuepath = Request.QueryString["cid"].ToString() + ">" + Request.QueryString["sl2"].ToString() + ">" + Request.QueryString["sl3"].ToString() + ">" + Request.QueryString["sl4"].ToString();
    //                                //    parentNode = TVBrand.FindNode(valuepath);
    //                                //    if (parentNode != null)
    //                                //    {
    //                                //        parentNode.Expand();
    //                                //        parentNode.Text = "<div style='color:Black'>" + parentNode.Text + "</div>";
    //                                //        parentNode.Selected = true;
    //                                //    }

    //                                //}

                                   
                                
    //                        }
    //                        dsCat.Dispose();
    //                    }
    //                //}
    //            }
    //        }
    //    }
        
    //}
    
    //void GetCategories(TreeNode node)
    //{
    //    DataSet dsCat = new DataSet();
    //    string CateID = node.Value;
    //    dsCat = oCat.GetWESModel(CateID.Split(new char[] { '>' })[0].ToString(), Convert.ToInt32(oHelper.GetOptionValues("DEFAULT CATALOG").ToString()), CateID.Split(new char[] { '>' })[1].ToString());
    //    if (dsCat != null)
    //    {
    //        foreach (DataRow rSCat in dsCat.Tables[0].Rows)
    //        {
    //            TreeNode newNode = new TreeNode(rSCat["TOSUITE_MODEL"].ToString(), CateID.Split(new char[] { '>' })[0].ToString() + ">" + CateID.Split(new char[] { '>' })[1].ToString() + ">" + rSCat["TOSUITE_MODEL"].ToString());
    //            //if (!(oCat.GetWESFamilycount(CateID.Split(new char[] { '>' })[0].ToString(), Convert.ToInt32(oHelper.GetOptionValues("DEFAULT CATALOG").ToString()), CateID.Split(new char[] { '>' })[1].ToString(), rSCat["TOSUITE_MODEL"].ToString()) > 0))
    //            //{
    //                newNode.NavigateUrl = "~/bybrand.aspx?&ld=0&cid=" + CateID.Split(new char[] { '>' })[0].ToString() + "&tsb=" + Server.UrlEncode(CateID.Split(new char[] { '>' })[1].ToString()) + "&tsm=" + Server.UrlEncode(rSCat["TOSUITE_MODEL"].ToString()) + "&byp=2";
    //            //}
    //            //else
    //            //{
    //            //    newNode.PopulateOnDemand = true;
    //            //}
              
    //            newNode.SelectAction = TreeNodeSelectAction.Expand;
    //            if (newNode.Value == valuepath)
    //            {                    
    //                newNode.Text = "<div style='color:Black'>" + newNode.Text + "</div>";
    //                newNode.Selected = true;                    
    //            }                
    //            node.ChildNodes.Add(newNode);
    //        }
    //        dsCat.Dispose();
    //    }       
    //}

    public string ST_Browsebycategory()
    {
        if (Request.QueryString["pcid"] != null)
        {
            HttpContext.Current.Session["BROWSEBYPRODUCT"] = Request.QueryString["pcid"];
            HttpContext.Current.Session["BROWSEBYBRAND"] = Request.QueryString["cid"];
        }

        if (Request.QueryString["cid"] != null && Request.QueryString["cid"].ToString() != "")//&& Request.QueryString["cid"].ToString() == "WES210582")
        {
            HelperDB oHelper = new HelperDB();
            ConnectionDB conStr = new ConnectionDB();
            TBWTemplateEngine tbwtEngine = new TBWTemplateEngine("BROWSEBYBRAND", Server.MapPath(oHelper.GetOptionValues("SHOPPTING TEMPLATE PATH").ToString()), conStr.ConnectionString);
            tbwtEngine.paraValue = Request.QueryString["cid"].ToString();
            tbwtEngine.RenderHTML("Row");
            return (tbwtEngine.RenderedHTML);
        }
        else if (Request.QueryString["pcr"] != null && Request.QueryString["pcr"].ToString() != "")//&& Request.QueryString["pcr"].ToString() == "WES210582")
        {
            HelperDB oHelper = new HelperDB();
            ConnectionDB conStr = new ConnectionDB();
            TBWTemplateEngine tbwtEngine = new TBWTemplateEngine("BROWSEBYBRAND", Server.MapPath(oHelper.GetOptionValues("SHOPPTING TEMPLATE PATH").ToString()), conStr.ConnectionString);
            tbwtEngine.paraValue = Request.QueryString["cid"].ToString();
            tbwtEngine.RenderHTML("Row");
            return (tbwtEngine.RenderedHTML);
        }
        return "<tr><td><div class=\"arrowlistmenu\"><table width=\"0\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\"><tr><td align=\"LEFT\" valign=\"bottom\" width=\"180\" ><ul>";
    }

    
    private DataSet GetDataSet(string SQLQuery)
    {
        DataSet ds = new DataSet();
        SqlDataAdapter da = new SqlDataAdapter(SQLQuery, conStr.ConnectionString.ToString().Substring(conStr.ConnectionString.ToString().IndexOf(';') + 1));
        da.Fill(ds, "generictable");
        return ds;
    }

    //void GetSubcategory(TreeNode node)
    //{
    //    DataSet dsCat = new DataSet();
    //    string CateID = node.Value;
    //    dsCat = oCat.GetSubCategoriesL4BrandWES(CateID.Split(new char[] { '>' })[0].ToString(), Convert.ToInt32(oHelper.GetOptionValues("DEFAULT CATALOG").ToString()), CateID.Split(new char[] { '>' })[1].ToString(), CateID.Split(new char[] { '>' })[2].ToString());
    //    if (dsCat != null)
    //    {
    //        foreach (DataRow rSCat in dsCat.Tables[0].Rows)
    //        {
    //            TreeNode newNode = new TreeNode(rSCat["CATEGORY_NAME"].ToString(),CateID.Split(new char[] { '>' })[0].ToString() + ">" + CateID.Split(new char[] { '>' })[1].ToString() + ">" + CateID.Split(new char[] { '>' })[2].ToString() + ">" + rSCat["CATEGORY_ID"].ToString());
    //            //if (oCat.GetSubCategoriesCount(rSCat["CATEGORY_ID"].ToString(), Convert.ToInt32(oHelper.GetOptionValues("DEFAULT CATALOG").ToString())) > 0)
    //            //{
    //            //    newNode.PopulateOnDemand = true;
    //            //}
    //            newNode.NavigateUrl = "~/bybrand.aspx?&ld=0&cid=" + CateID.Split(new char[] { '>' })[0].ToString() + "&sl2=" + CateID.Split(new char[] { '>' })[1].ToString() + "&sl3=" + CateID.Split(new char[] { '>' })[2].ToString() + "&sl4=" + rSCat["CATEGORY_ID"].ToString() + "&byp=2";
    //            newNode.SelectAction = TreeNodeSelectAction.Expand;
    //            if (newNode.Value == valuepath)
    //            {
    //                // parentNode.Expand();
    //                //if (idValue == IDS.Length - 2)
    //                {
    //                    newNode.Text = "<div style='color:Black'>" + newNode.Text + "</div>";
    //                    newNode.Selected = true;
    //                }
    //            }
    //            node.ChildNodes.Add(newNode);
    //            //node.ChildNodes.Add(newNode);               
    //        }
    //        dsCat.Dispose();
    //    }       
    //}
   
    #endregion


    //protected void ConstructRootTreeModel(object sender, EventArgs e)
    //{
    //    if (TVModel.Nodes.Count == 0 && Request.QueryString["tsb"]!=null)
    //    {
    //        string[] IDS = MCID.Split('>');
    //        string valuepath = "";
            

    //        if ((Request.QueryString["cid"] != null && Request.QueryString["cid"].ToString() != "List all models") || CID.ToString() != "")
    //        {
    //            if (Request.QueryString["cid"] != null)
    //            {

    //                string CateID = "";
    //                if (Request.QueryString["pcr"] != null && Request.QueryString["pcr"].ToString() != "")
    //                {
    //                    CateID = Request.QueryString["pcr"].ToString();
    //                }
    //                else
    //                {
    //                    CateID = Request.QueryString["cid"].ToString();
    //                }
                    
            
    //                DataSet dsCat = new DataSet();                                        

    //                string tempCName = GetCName(CateID);
    //                //dsCat = EasyAsk.GetWESModel(tempCName, 2, Server.UrlDecode(Request.QueryString["tsb"].ToString()));                               
    //                dsCat = (DataSet)HttpContext.Current.Session["WESBrand_Model"];
    //                if (dsCat != null)
    //                {
    //                    foreach (DataRow rSCat in dsCat.Tables[0].Rows)
    //                    {


    //                        TreeNode newNode = new TreeNode(rSCat["TOSUITE_MODEL"].ToString(), CateID + ">" + rSCat["TOSUITE_MODEL"].ToString());
                            
    //                        newNode.NavigateUrl = "~/bybrand.aspx?&ld=0&cid=" + CateID + "&tsb=" + Server.UrlEncode(Request.QueryString["tsb"]) + "&tsm=" + Server.UrlEncode(rSCat["TOSUITE_MODEL"].ToString()) + "&byp=2";
    //                        newNode.PopulateOnDemand = true;                          
    //                        newNode.SelectAction = TreeNodeSelectAction.Expand;
    //                        newNode.Text = "<div style='border-bottom-width: 1px; border-bottom-style: solid; border-bottom-color:#d1d1d1; width:100%;cursor: hand;'>" + newNode.Text + "</div>";
    //                        TVModel.Nodes.Add(newNode);                          
    //                    }
    //                    dsCat.Dispose();
    //                }                    
    //            }
    //        }
    //    }

    //}
    //private string GetCName(string catID)
    //{
    //    DataSet DSBC = null;
    //    string catIDtemp = catID;
    //    do
    //    {
    //        DSBC = GetDataSet("SELECT CATEGORY_NAME,PARENT_CATEGORY,CATEGORY_ID FROM TB_CATEGORY WHERE CATEGORY_ID = '" + catIDtemp + "'");
    //        foreach (DataRow DR in DSBC.Tables[0].Rows)
    //        {
    //            catIDtemp = DR["PARENT_CATEGORY"].ToString();
    //            if (catIDtemp == "0")
    //            {
    //                // DSUBC = GetDataSet("SELECT CATEGORY_NAME,PARENT_CATEGORY,CATEGORY_ID FROM TB_CATEGORY WHERE PARENT_CATEGORY = '" + DR["CATEGORY_ID"].ToString() + "' AND CATEGORY_NAME Like 'Product'");
    //                return DR["CATEGORY_NAME"].ToString();
    //            }
    //        }
    //    } while (DSBC.Tables[0].Rows[0].ItemArray[1].ToString() != "0");
    //    return DSBC.Tables[0].Rows[0]["CATEGORY_NAME"].ToString();
    //}


    protected string ST_BrandAndModel()
    {

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

            stemplatepath = Server.MapPath(ConfigurationManager.AppSettings["StringTemplatePath"].ToString());
            int ictrows = 0;
            string _bypcat = null;
            _bypcat = Request.QueryString["bypcat"];
            DataSet dscat = new DataSet();
            DataTable dt = null;
            if ((HttpContext.Current.Request.Url.ToString().ToLower().Contains("categorylist.aspx") == true && _bypcat == null) || HttpContext.Current.Request.Url.ToString().ToLower().Contains("product_list.aspx") == true)
            {
                dt = (DataTable)((DataSet)HttpContext.Current.Session["MainMenuClick"]).Tables["Brand"];
                if (dt == null)
                    dscat = null;
                else
                    dscat.Tables.Add(dt.Copy());   
            }
            else
            {
                dscat = (DataSet)HttpContext.Current.Session["WESBrand_Model"];
            }
           
                                

            
          

            if (dscat == null)
                return "";
            _stg_records = new StringTemplateGroup("searchrsltcategoryleftrecords", stemplatepath);
            _stg_container = new StringTemplateGroup("searchrsltcategoryleftmain", stemplatepath);
            if (dscat.Tables.Count > 0)
                lstrows = new TBWDataList[dscat.Tables.Count + 1];

            for (int i = 0; i < dscat.Tables.Count; i++)
            {
                Boolean tmpallow = true;
                //if (HttpContext.Current.Request.Url.ToString().ToLower().Contains("categorylist.aspx") == true)
                //{
                    if (dscat.Tables[i].TableName.Contains("Brand")==true && Request.QueryString["byp"] == "2")
                        tmpallow = true;                    
                    else
                        tmpallow = false;
                //}
                if (tmpallow == true)
                {
                    if (dscat.Tables[i].Rows.Count > 0)
                    {
                        lstrecords = new TBWDataList[dscat.Tables[i].Rows.Count + 1];
                       
                        int ictrecords = 0;

                        int j = 0;
                        foreach (DataRow dr in dscat.Tables[i].Rows)//For Records
                        {


                            if ((HttpContext.Current.Request.Url.ToString().ToLower().Contains("categorylist.aspx") == true && _bypcat == null) || HttpContext.Current.Request.Url.ToString().ToLower().Contains("product_list.aspx") == true)
                            {
                                _stmpl_records = _stg_records.GetInstanceOf("searchrsltcategoryright" + "\\" + "cell");
                                _stmpl_records.SetAttribute("TBW_CATEGORY_ID", HttpUtility.UrlEncode(ParentCatID.ToString()));
                                _stmpl_records.SetAttribute("TBW_BRAND", dr["TOSUITE_BRAND"].ToString());                                

                            }
                            else
                            {
                                _stmpl_records = _stg_records.GetInstanceOf("searchrsltcategoryright" + "\\" + "cell2");
                                _stmpl_records.SetAttribute("TBW_CATEGORY_ID", HttpUtility.UrlEncode(ParentCatID.ToString()));
                                _stmpl_records.SetAttribute("TBW_BRAND", Server.UrlEncode(Request.QueryString["tsb"]));
                                _stmpl_records.SetAttribute("TBW_MODEL", HttpUtility.UrlEncode(dr["TOSUITE_MODEL"].ToString()));
                                
                            }
                            if (HttpContext.Current.Session["EA"] != null)
                            {
                                _stmpl_records.SetAttribute("EA_PATH", HttpUtility.UrlEncode(oHelper.StringEnCrypt(HttpContext.Current.Session["EA"].ToString())));
                            }
                         
                            lstrecords[ictrecords] = new TBWDataList(_stmpl_records.ToString());                            
                            ictrecords++;
                        }

                        j++;
                         
                        _stmpl_recordsrows = _stg_container.GetInstanceOf("searchrsltcategoryright" + "\\" + "row1");
                        if ((HttpContext.Current.Request.Url.ToString().ToLower().Contains("categorylist.aspx") == true && _bypcat == null)||   HttpContext.Current.Request.Url.ToString().ToLower().Contains("product_list.aspx") == true)                            
                                _stmpl_recordsrows.SetAttribute("TBT_ATTRIBUTE_TITLE", dscat.Tables[i].TableName.ToString());
                        else
                            _stmpl_recordsrows.SetAttribute("TBT_ATTRIBUTE_TITLE", "MODEL");

                        _stmpl_recordsrows.SetAttribute("TBWDataList", lstrecords);                        
                        lstrows[ictrows] = new TBWDataList(_stmpl_recordsrows.ToString());
                        ictrows++;
                    }
                }
            }
           
            _stmpl_container = _stg_container.GetInstanceOf("searchrsltcategoryright" + "\\" + "main");
            //_stmpl_container.SetAttribute("Selection", updateNavigation());
            _stmpl_container.SetAttribute("TBWDataList", lstrows);          
            sHTML += _stmpl_container.ToString();
        }

        catch (Exception ex)
        {
            sHTML = ex.Message;
        }
        finally
        {

        }


        return sHTML;
    }


    protected string ST_bybrand()
    {
        StringTemplateGroup _stg_main_container = null;
        StringTemplateGroup _stg_records_container = null;
        StringTemplateGroup _stg_records = null;
        StringTemplate _stmpl_main_container_tmpl = null;
        StringTemplate _stmpl_records_container_tmpl = null;
        StringTemplate _stmpl_records_tmpl = null;
        //StringTemplate _stmpl_records_tmpl2 = null;
        //StringTemplate _stmpl_records_tmpl3 = null;
        TBWDataList[] lstrecords = new TBWDataList[0];
        TBWDataList[] lstrows = new TBWDataList[0];
        TBWDataList[] lstcontainers = new TBWDataList[3];
        DataSet dsCat;
        string[] filterval = null;
        string[] filterval1 = null;
        string[] filterval2 = null;
        oHelper = new HelperDB();
        oErr = new ErrorHandler();
        oCat = new Category();
        oPF = new ProductFamily();
        oPR = new ProductRender();
        string sHTML = "";
        string dropdowncatid = "";
        string _catid = "";
        string _fid = "";


        try
        {            
            stemplatepath = stemplatepath.Substring(0, stemplatepath.Substring(0, stemplatepath.Length - 1).LastIndexOf('\\')) + "\\bybrand\\";

            if (Request.QueryString["cid"] != null && Request.QueryString["sldummy"] != null)//sldummy is to skip
            {
                #region old
                if (Request.QueryString["cid"].ToString().Length > 0)
                {
                    dropdowncatid = Getdropdwoncatid(Request.QueryString["cid"].ToString());
                    _catid = Request.QueryString["cid"].ToString();
                    tempCName = GetCName(Request.QueryString["cid"].ToString());
                    if (Request.QueryString["pcid"] != null)
                    {
                        tempCID = Request.QueryString["pcid"].ToString();
                        if (tempCID != GetCID(Request.QueryString["cid"].ToString()))
                        {
                            tempCID = GetCID(Request.QueryString["cid"].ToString());
                        }
                    }
                    else
                    {
                        tempCID = GetCID(Request.QueryString["cid"].ToString());
                    }
                    //if (Request.QueryString["cid"].ToString() == "WES598" || Request.QueryString["cid"].ToString() == "WES503")
                    //{
                    //    tempCID = GetCID(Request.QueryString["cid"].ToString());
                    //}
                    //else
                    //{
                    //    tempCID = Request.QueryString["cid"].ToString();
                    //}
                }
                if (Request.QueryString["fid"] != null && Request.QueryString["fid"].ToString() != "")
                {
                    _fid = Request.QueryString["fid"].ToString();
                }

                if (hidcatIds.Value != string.Empty && hidcatIds.Value != null)
                {
                    filterval = hidcatIds.Value.Split('^');
                }
                if (HidsubcatIds.Value != string.Empty && HidsubcatIds.Value != null)
                {
                    filterval1 = HidsubcatIds.Value.Split('^');
                }

                dsCat = new DataSet();
                dsCat = oCat.GetSubCategoriesBrand(tempCID, Convert.ToInt32(oHelper.GetOptionValues("DEFAULT CATALOG").ToString()));
                if (dsCat != null && dsCat.Tables.Count > 0 && (dsCat.Tables[0].Rows.Count > 0))
                {
                    ictrecords = 0;
                    lstrecords = new TBWDataList[dsCat.Tables[0].Rows.Count + 1];
                    _stg_records = new StringTemplateGroup("searchrsltmultilistitem", stemplatepath);
                    _stg_records_container = new StringTemplateGroup("searchrsltmultilistcontainer", stemplatepath);
                    _stmpl_records_tmpl = _stg_records.GetInstanceOf("searchbybrandcategory" + "\\" + "multilistitem");
                    _stmpl_records_tmpl.SetAttribute("TBW_LIST_VAL1", "Select Brand");
                    _stmpl_records_tmpl.SetAttribute("TBW_LIST_VAL", "Select Brand");
                    lstrecords[ictrecords] = new TBWDataList(_stmpl_records_tmpl.ToString());
                    ictrecords++;
                    bool selstate = false;
                    foreach (DataRow _drow in dsCat.Tables[0].Rows)
                    {
                        _stmpl_records_tmpl = _stg_records.GetInstanceOf("searchbybrandcategory" + "\\" + "multilistitem");
                        _stmpl_records_tmpl.SetAttribute("TBW_LIST_VAL1", _drow["CATEGORY_ID"].ToString());
                        if (filterval != null && _drow["CATEGORY_ID"].ToString() == filterval[0].ToString())
                        {
                            _stmpl_records_tmpl.SetAttribute("TBW_CATEGORY_ID_SELECTED", "SELECTED=\"SELECTED\" ");
                            selstate = true;
                            Response.Redirect("categorylist.aspx?&ld=0&cid=" + _drow["CATEGORY_ID"].ToString() + "&byp=2&bypcat=1", false);
                        }
                        else if (filterval == null && _drow["CATEGORY_ID"].ToString() == dropdowncatid && selstate == false)
                        {
                            filterval = new string[2];
                            filterval[0] = _drow["CATEGORY_ID"].ToString();
                            filterval[1] = _drow["CATEGORY_NAME"].ToString();
                            _stmpl_records_tmpl.SetAttribute("TBW_CATEGORY_ID_SELECTED", "SELECTED=\"SELECTED\" ");
                            //Response.Redirect("categorylist.aspx?ld=0&cid=" + _drow["CATEGORY_ID"].ToString() + "&byp=2&bypcat=1", false);
                        }
                        _stmpl_records_tmpl.SetAttribute("TBW_LIST_VAL", _drow["CATEGORY_NAME"].ToString());
                        lstrecords[ictrecords] = new TBWDataList(_stmpl_records_tmpl.ToString());
                        ictrecords++;

                    }
                    _stmpl_records_container_tmpl = _stg_records_container.GetInstanceOf("searchbybrandcategory" + "\\" + "multilistcontainer");
                    _stmpl_records_container_tmpl.SetAttribute("TBWDataList", lstrecords);
                    _stmpl_records_container_tmpl.SetAttribute("TBW_SELECTED_ID", "1");
                    lstcontainers[0] = new TBWDataList(_stmpl_records_container_tmpl.ToString());
                }
                if (filterval != null && filterval[0].ToString().Length > 0)
                {
                    tempCID = filterval[0].ToString();
                    //if (tempCID != Request.QueryString["pcid"].ToString())
                    bool selstate1 = false;
                    DataSet dsCat1 = new DataSet();
                    //dsCat1 = GetDataSet("SELECT category_id,category_name,HIERARCHY_LEVEL FROM Category_Function (" + oHelper.GetOptionValues("DEFAULT CATALOG").ToString() + ",'" + tempCID + "')");
                    dsCat1 = GetDataSet(string.Format("SELECT category_id,category_name,HIERARCHY_LEVEL FROM Category_Function ({0},'{1}') where ISNULL(CUSTOM_NUM_FIELD3,0)<> 3", oHelper.GetOptionValues("DEFAULT CATALOG").ToString(), tempCID));
                    if (dsCat1 != null && dsCat1.Tables.Count > 0 && (dsCat1.Tables[0].Rows.Count > 0))
                    {
                        ictrecords = 0;
                        DataRow[] _DCRow = null;
                        _DCRow = dsCat1.Tables[0].Select("HIERARCHY_LEVEL>0");
                        //if (filterval == null)
                        //    _DCRow = dsCat1.Tables[0].Select("HIERARCHY_LEVEL>1");
                        //else
                        //    _DCRow = dsCat1.Tables[0].Select("HIERARCHY_LEVEL>0");
                        if (_DCRow != null && _DCRow.Length > 0)
                        {
                            lstrecords = new TBWDataList[_DCRow.Count() + 1];
                            _stg_records = new StringTemplateGroup("searchrsltmultilistitem", stemplatepath);
                            _stg_records_container = new StringTemplateGroup("searchrsltmultilistcontainer", stemplatepath);
                            _stmpl_records_tmpl = _stg_records.GetInstanceOf("searchbybrandcategory" + "\\" + "multilistitem");
                            _stmpl_records_tmpl.SetAttribute("TBW_LIST_VAL1", "List all models");
                            _stmpl_records_tmpl.SetAttribute("TBW_LIST_VAL", "List all models");
                            lstrecords[ictrecords] = new TBWDataList(_stmpl_records_tmpl.ToString());
                            ictrecords++;
                            foreach (DataRow _drow in _DCRow)
                            {
                                _stmpl_records_tmpl = _stg_records.GetInstanceOf("searchbybrandcategory" + "\\" + "multilistitem");
                                _stmpl_records_tmpl.SetAttribute("TBW_LIST_VAL1", _drow["CATEGORY_ID"].ToString());
                                //if (_catid == _drow["CATEGORY_ID"].ToString())
                                //{
                                //  _stmpl_records_tmpl.SetAttribute("TBW_CATEGORY_ID_SELECTED", "SELECTED=\"SELECTED\" ");
                                //}
                                //else 
                                if (filterval1 != null && _drow["CATEGORY_ID"].ToString() == filterval1[0].ToString())
                                {
                                    _stmpl_records_tmpl.SetAttribute("TBW_CATEGORY_ID_SELECTED", "SELECTED=\"SELECTED\" ");
                                    selstate1 = true;
                                    //Response.Redirect("product_list.aspx?&cid=" + _drow["CATEGORY_ID"].ToString() + "&byp=2", false);
                                    Response.Redirect("bybrand.aspx?&id=0&cid=" + _drow["CATEGORY_ID"].ToString() + "&byp=2", false);
                                }
                                else if (filterval1 == null && _drow["CATEGORY_ID"].ToString() == _catid.ToString() && selstate1 == false)
                                {
                                    filterval1 = new string[2];
                                    filterval1[0] = _drow["CATEGORY_ID"].ToString();
                                    filterval1[1] = _drow["CATEGORY_NAME"].ToString();
                                    _stmpl_records_tmpl.SetAttribute("TBW_CATEGORY_ID_SELECTED", "SELECTED=\"SELECTED\" ");

                                }
                                _stmpl_records_tmpl.SetAttribute("TBW_LIST_VAL", _drow["CATEGORY_NAME"].ToString());
                                lstrecords[ictrecords] = new TBWDataList(_stmpl_records_tmpl.ToString());
                                ictrecords++;

                            }
                            _stmpl_records_container_tmpl = _stg_records_container.GetInstanceOf("searchbybrandcategory" + "\\" + "multilistcontainer");
                            _stmpl_records_container_tmpl.SetAttribute("TBWDataList", lstrecords);
                            _stmpl_records_container_tmpl.SetAttribute("TBW_SELECTED_ID", "2");
                            lstcontainers[1] = new TBWDataList(_stmpl_records_container_tmpl.ToString());
                        }
                    }
                    dsCat1.Dispose();
                }
                else
                {
                    lstrecords = new TBWDataList[1];
                    _stg_records = new StringTemplateGroup("searchrsltmultilistitem", stemplatepath);
                    _stg_records_container = new StringTemplateGroup("searchrsltmultilistcontainer", stemplatepath);
                    _stmpl_records_tmpl = _stg_records.GetInstanceOf("searchbybrandcategory" + "\\" + "multilistitem");
                    _stmpl_records_tmpl.SetAttribute("TBW_LIST_VAL1", "List all models");
                    _stmpl_records_tmpl.SetAttribute("TBW_LIST_VAL", "List all models");
                    lstrecords[0] = new TBWDataList(_stmpl_records_tmpl.ToString());
                    _stmpl_records_container_tmpl = _stg_records_container.GetInstanceOf("searchbybrandcategory" + "\\" + "multilistcontainer");
                    _stmpl_records_container_tmpl.SetAttribute("TBWDataList", lstrecords);
                    _stmpl_records_container_tmpl.SetAttribute("TBW_SELECTED_ID", "2");
                    lstcontainers[1] = new TBWDataList(_stmpl_records_container_tmpl.ToString());
                }
                string catIDLiSt = "'0'";
                if (filterval1 != null && filterval1[0].ToString().Length > 0)
                {
                    DataSet dsCattemp = new DataSet();
                    //dsCattemp = GetDataSet("SELECT CATEGORY_ID FROM Category_Function (" + oHelper.GetOptionValues("DEFAULT CATALOG").ToString() + ",'" + filterval1[0].ToString() + "')");
                    dsCattemp = GetDataSet(string.Format("SELECT CATEGORY_ID FROM Category_Function ({0},'{1}') where ISNULL(CUSTOM_NUM_FIELD3,0)<> 3'", oHelper.GetOptionValues("DEFAULT CATALOG").ToString(), filterval1[0].ToString()));
                    foreach (DataRow _drow in dsCattemp.Tables[0].Rows)
                    {
                        catIDLiSt = catIDLiSt + ",'" + _drow["category_Id"].ToString() + "'";
                    }

                    //else
                    //{

                    //    foreach (DataRow _drow in dsCat1.Tables[0].Rows)
                    //    {
                    //        catIDLiSt = catIDLiSt + ",'" + _drow["category_Id"].ToString() + "'";
                    //    }
                    //}
                    //catIDLiSt = catIDLiSt + ",'" + _catid + "'";
                    DataSet dsCat2 = new DataSet();
                    bool selstate2 = false;
                    string sqlQuery = "(SELECT DISTINCT F.FAMILY_ID,F.FAMILY_NAME,F.CATEGORY_ID,(SELECT COUNT(PRODUCT_ID) FROM TB_PROD_FAMILY WHERE FAMILY_ID=F.FAMILY_ID) AS COUNT FROM TB_FAMILY F,TB_CATALOG_FAMILY CF where CF.FAMILY_ID=F.FAMILY_ID AND F.CATEGORY_ID IN(" + catIDLiSt + ") AND CF.CATALOG_ID=" + oHelper.GetOptionValues("DEFAULT CATALOG").ToString() + " AND F.PARENT_FAMILY_ID=0 AND (SELECT COUNT(PRODUCT_ID) FROM TB_PROD_FAMILY WHERE FAMILY_ID=F.FAMILY_ID) > 0) ";
                    sqlQuery += "UNION ";
                    sqlQuery += "(SELECT DISTINCT F.FAMILY_ID,(F1.FAMILY_NAME + ' - ' + F.FAMILY_NAME) AS FAMILY_NAME,F.CATEGORY_ID,(SELECT COUNT(PRODUCT_ID) FROM TB_PROD_FAMILY WHERE FAMILY_ID=F.FAMILY_ID) AS COUNT FROM TB_FAMILY F,TB_FAMILY F1,TB_CATALOG_FAMILY CF where CF.FAMILY_ID=F.FAMILY_ID AND F.CATEGORY_ID IN(" + catIDLiSt + ") AND CF.CATALOG_ID=" + oHelper.GetOptionValues("DEFAULT CATALOG").ToString() + " AND F.PARENT_FAMILY_ID IN(SELECT DISTINCT F.FAMILY_ID FROM TB_FAMILY F,TB_CATALOG_FAMILY CF where CF.FAMILY_ID=F.FAMILY_ID AND F.CATEGORY_ID IN(" + catIDLiSt + ") AND CF.CATALOG_ID=" + oHelper.GetOptionValues("DEFAULT CATALOG").ToString() + " AND F.PARENT_FAMILY_ID=0) AND F1.FAMILY_ID=F.PARENT_FAMILY_ID AND (SELECT COUNT(PRODUCT_ID) FROM TB_PROD_FAMILY WHERE FAMILY_ID=F.FAMILY_ID) > 0 ) ORDER BY FAMILY_NAME";
                    dsCat2 = GetDataSet(sqlQuery);
                    if (dsCat2 != null && dsCat2.Tables.Count > 0 && (dsCat2.Tables[0].Rows.Count > 0))
                    {
                        ictrecords = 0;

                        lstrecords = new TBWDataList[dsCat2.Tables[0].Rows.Count + 1];
                        _stg_records = new StringTemplateGroup("searchrsltmultilistitem", stemplatepath);
                        _stg_records_container = new StringTemplateGroup("searchrsltmultilistcontainer", stemplatepath);
                        _stmpl_records_tmpl = _stg_records.GetInstanceOf("searchbybrandcategory" + "\\" + "multilistitem");
                        _stmpl_records_tmpl.SetAttribute("TBW_LIST_VAL1", "List all products");
                        _stmpl_records_tmpl.SetAttribute("TBW_LIST_VAL", "List all products");
                        lstrecords[ictrecords] = new TBWDataList(_stmpl_records_tmpl.ToString());
                        ictrecords++;
                        foreach (DataRow _drow in dsCat2.Tables[0].Rows)
                        {
                            _stmpl_records_tmpl = _stg_records.GetInstanceOf("searchbybrandcategory" + "\\" + "multilistitem");
                            _stmpl_records_tmpl.SetAttribute("TBW_LIST_VAL1", _drow["FAMILY_ID"].ToString());//_drow["CATEGORY_ID"].ToString() + "|" + _drow["FAMILY_ID"].ToString());
                            if (filterval2 != null && _drow["CATEGORY_ID"].ToString() + "|" + _drow["FAMILY_ID"].ToString() == filterval2[0].ToString())
                            {
                                _stmpl_records_tmpl.SetAttribute("TBW_CATEGORY_ID_SELECTED", "SELECTED=\"SELECTED\" ");
                                selstate2 = true;
                            }
                            else if (filterval2 == null && _drow["CATEGORY_ID"].ToString() == _catid.ToString() && _drow["FAMILY_ID"].ToString() == _fid.ToString() && selstate2 == false)
                            {
                                filterval2 = new string[2];
                                filterval2[0] = _drow["FAMILY_ID"].ToString();
                                filterval2[1] = _drow["FAMILY_NAME"].ToString();
                                _stmpl_records_tmpl.SetAttribute("TBW_CATEGORY_ID_SELECTED", "SELECTED=\"SELECTED\" ");
                                //Response.Redirect("categorylist.aspx?ld=0&cid=" + _drow["CATEGORY_ID"].ToString() + "&byp=2&bypcat=1", false);
                            }
                            _stmpl_records_tmpl.SetAttribute("TBW_LIST_VAL", _drow["FAMILY_NAME"].ToString());
                            lstrecords[ictrecords] = new TBWDataList(_stmpl_records_tmpl.ToString());
                            ictrecords++;

                        }
                        _stmpl_records_container_tmpl = _stg_records_container.GetInstanceOf("searchbybrandcategory" + "\\" + "multilistcontainer");
                        _stmpl_records_container_tmpl.SetAttribute("TBWDataList", lstrecords);
                        _stmpl_records_container_tmpl.SetAttribute("TBW_SELECTED_ID", "3");
                        lstcontainers[2] = new TBWDataList(_stmpl_records_container_tmpl.ToString());
                    }
                }
                else
                {

                    lstrecords = new TBWDataList[2];
                    _stg_records = new StringTemplateGroup("searchrsltmultilistitem", stemplatepath);
                    _stg_records_container = new StringTemplateGroup("searchrsltmultilistcontainer", stemplatepath);
                    _stmpl_records_tmpl = _stg_records.GetInstanceOf("searchbybrandcategory" + "\\" + "multilistitem");
                    _stmpl_records_tmpl.SetAttribute("TBW_LIST_VAL1", "List all products");
                    _stmpl_records_tmpl.SetAttribute("TBW_LIST_VAL", "List all products");
                    lstrecords[0] = new TBWDataList(_stmpl_records_tmpl.ToString());
                    _stmpl_records_container_tmpl = _stg_records_container.GetInstanceOf("searchbybrandcategory" + "\\" + "multilistcontainer");
                    _stmpl_records_container_tmpl.SetAttribute("TBWDataList", lstrecords);
                    _stmpl_records_container_tmpl.SetAttribute("TBW_SELECTED_ID", "3");
                    lstcontainers[2] = new TBWDataList(_stmpl_records_container_tmpl.ToString());

                }

                /* _stg_records = new StringTemplateGroup("searchrsltmultilistitem", stemplatepath);
                 _stg_records_container = new StringTemplateGroup("searchrsltmultilistcontainer", stemplatepath);
                 _stmpl_records_tmpl2 = _stg_records.GetInstanceOf("searchbyproduct" + "\\" + "multilistitem");
                 _stmpl_records_container_tmpl = _stg_records_container.GetInstanceOf("searchbyproduct" + "\\" + "multilistcontainer");

                // _stmpl_records_container_tmpl = LoadModel(dsCat, _stmpl_records_tmpl2, _stmpl_records_container_tmpl);
                 lstcontainers[1] = new TBWDataList(_stmpl_records_container_tmpl.ToString());
                 */



                _stg_main_container = new StringTemplateGroup("searchrsltmultilistmain", stemplatepath);
                _stmpl_main_container_tmpl = _stg_main_container.GetInstanceOf("searchbybrandcategory" + "\\" + "multilistmain");
                _stmpl_main_container_tmpl.SetAttribute("TBW_CATEGORY_NAME", tempCName);
                _stmpl_main_container_tmpl.SetAttribute("TBWDataList", lstcontainers);

                sHTML = _stmpl_main_container_tmpl.ToString();
                //if (dspfilters.Tables[0].Rows.Count == 0)
                //sHTML = "";
                #endregion
            }
            else
            {
                if (hidcatIds.Value != string.Empty && hidcatIds.Value != null)
                {
                    filterval = hidcatIds.Value.Split('^');
                }
                if (HidsubcatIds.Value != string.Empty && HidsubcatIds.Value != null)
                {
                    filterval1 = HidsubcatIds.Value.Split('^');
                }
                if (HidsubcatIds1.Value != string.Empty && HidsubcatIds1.Value != null)
                {
                    filterval2 = HidsubcatIds1.Value.Split('^');
                }

                if (Request.QueryString["cid"] != null && Request.QueryString["cid"].ToString() != "")//&& Request.QueryString["cid"].ToString() == "WES210582")
                {
                    string cid = Request.QueryString["cid"].ToString();

                    tempCID = Request.QueryString["cid"].ToString();
                    tempCName = GetCName(tempCID);
                    dsCat = new DataSet();
                    //if (cid == "WES0389")
                    //{
                    //    ictrecords = 0;
                    //    lstrecords = new TBWDataList[1];
                    //    _stg_records = new StringTemplateGroup("searchrsltmultilistitem", stemplatepath);
                    //    _stg_records_container = new StringTemplateGroup("searchrsltmultilistcontainer", stemplatepath);
                    //    _stmpl_records_tmpl = _stg_records.GetInstanceOf("searchbybrandcategory" + "\\" + "multilistitem");
                    //    _stmpl_records_tmpl.SetAttribute("TBW_LIST_VAL1", "Select Category");
                    //    string sSQL = "select distinct subcatname_l1 from VIEW_WESTB_TOSUITE_BY_BRAND_PRODUCT where CATEGORY_ID = 'WES0389'";
                    //    oHelper.SQLString = sSQL;
                    //    DataSet oDs = oHelper.GetDataSet();
                    //    _stmpl_records_tmpl = _stg_records.GetInstanceOf("searchbybrandcategory" + "\\" + "multilistitem");
                    //    _stmpl_records_tmpl.SetAttribute("TBW_LIST_VAL", "Select Category");
                    //    lstrecords[ictrecords] = new TBWDataList(_stmpl_records_tmpl.ToString());
                    //    ictrecords++;
                    //    if (oDs != null)
                    //    {
                    //        lstrecords = new TBWDataList[oDs.Tables[0].Rows.Count + 1];
                    //        foreach (DataRow _drow in oDs.Tables[0].Rows)
                    //        {
                    //            _stmpl_records_tmpl = _stg_records.GetInstanceOf("searchbybrandcategory" + "\\" + "multilistitem");
                    //            _stmpl_records_tmpl.SetAttribute("TBW_LIST_VAL1", _drow["subcatname_l1"].ToString());
                    //            _stmpl_records_tmpl.SetAttribute("TBW_LIST_VAL", _drow["subcatname_l1"].ToString());
                    //            lstrecords[ictrecords] = new TBWDataList(_stmpl_records_tmpl.ToString());
                    //            ictrecords++;
                    //        }
                    //    }
                    //    _stmpl_records_container_tmpl = _stg_records_container.GetInstanceOf("searchbybrandcategory" + "\\" + "multilistcontainer");
                    //    _stmpl_records_container_tmpl.SetAttribute("TBWDataList", lstrecords);
                    //    _stmpl_records_container_tmpl.SetAttribute("TBW_SELECTED_ID", "2");
                    //    lstcontainers[0] = new TBWDataList(_stmpl_records_container_tmpl.ToString());

                    //    ictrecords = 0;
                    //    lstrecords = new TBWDataList[1];
                    //    _stg_records = new StringTemplateGroup("searchrsltmultilistitem", stemplatepath);
                    //    _stg_records_container = new StringTemplateGroup("searchrsltmultilistcontainer", stemplatepath);
                    //    _stmpl_records_tmpl = _stg_records.GetInstanceOf("searchbybrandcategory" + "\\" + "multilistitem");
                    //    _stmpl_records_tmpl.SetAttribute("TBW_LIST_VAL1", "Select Brand");
                    //    _stmpl_records_tmpl.SetAttribute("TBW_LIST_VAL", "Select Brand");
                    //    lstrecords[0] = new TBWDataList(_stmpl_records_tmpl.ToString());

                    //    _stmpl_records_container_tmpl = _stg_records_container.GetInstanceOf("searchbybrandcategory" + "\\" + "multilistcontainer");
                    //    _stmpl_records_container_tmpl.SetAttribute("TBWDataList", lstrecords);
                    //    _stmpl_records_container_tmpl.SetAttribute("TBW_SELECTED_ID", "2");
                    //    lstcontainers[1] = new TBWDataList(_stmpl_records_container_tmpl.ToString());

                    //    ictrecords = 0;
                    //    lstrecords = new TBWDataList[1];
                    //    _stg_records = new StringTemplateGroup("searchrsltmultilistitem", stemplatepath);
                    //    _stg_records_container = new StringTemplateGroup("searchrsltmultilistcontainer", stemplatepath);
                    //    _stmpl_records_tmpl = _stg_records.GetInstanceOf("searchbybrandcategory" + "\\" + "multilistitem");
                    //    _stmpl_records_tmpl.SetAttribute("TBW_LIST_VAL1", "Select Model");
                    //    _stmpl_records_tmpl.SetAttribute("TBW_LIST_VAL", "Select Model");
                    //    lstrecords[0] = new TBWDataList(_stmpl_records_tmpl.ToString());
                    //    _stmpl_records_container_tmpl = _stg_records_container.GetInstanceOf("searchbybrandcategory" + "\\" + "multilistcontainer");
                    //    _stmpl_records_container_tmpl.SetAttribute("TBWDataList", lstrecords);
                    //    _stmpl_records_container_tmpl.SetAttribute("TBW_SELECTED_ID", "2");
                    //    lstcontainers[2] = new TBWDataList(_stmpl_records_container_tmpl.ToString());
                    //}
                    //else
                    //{
                        //dsCat = oCat.GetWESBrand(tempCID, Convert.ToInt32(oHelper.GetOptionValues("DEFAULT CATALOG").ToString()));
                        //dsCat.Tables.Add (EasyAsk.GetMainMenuClickDetail(tempCID, "Brand").Copy()) ;
                        dsCat.Tables.Add((DataTable)((DataSet)HttpContext.Current.Session["MainMenuClick"]).Tables["Brand"].Copy());


                        if (dsCat != null && dsCat.Tables.Count > 0 && (dsCat.Tables[0].Rows.Count > 0))
                        {
                            ictrecords = 0;
                            lstrecords = new TBWDataList[dsCat.Tables[0].Rows.Count + 1];
                            _stg_records = new StringTemplateGroup("searchrsltmultilistitem", stemplatepath);
                            _stg_records_container = new StringTemplateGroup("searchrsltmultilistcontainer", stemplatepath);
                            _stmpl_records_tmpl = _stg_records.GetInstanceOf("searchbybrandcategory" + "\\" + "multilistitem");

                            _stmpl_records_tmpl.SetAttribute("TBW_LIST_VAL1", "Select Brand");
                            _stmpl_records_tmpl.SetAttribute("TBW_LIST_VAL", "Select Brand");
                            lstrecords[ictrecords] = new TBWDataList(_stmpl_records_tmpl.ToString());
                            ictrecords++;
                            bool selstate = false;
                            foreach (DataRow _drow in dsCat.Tables[0].Rows)
                            {
                                _stmpl_records_tmpl = _stg_records.GetInstanceOf("searchbybrandcategory" + "\\" + "multilistitem");
                                _stmpl_records_tmpl.SetAttribute("TBW_LIST_VAL1", _drow["TOSUITE_BRAND"].ToString());
                                if (filterval != null && _drow["TOSUITE_BRAND"].ToString() == filterval[0].ToString())
                                {
                                    _stmpl_records_tmpl.SetAttribute("TBW_CATEGORY_ID_SELECTED", "SELECTED=\"SELECTED\" ");
                                    selstate = true;
                                    Response.Redirect("categorylist.aspx?&ld=0&cid=" + tempCID + "&tsb=" + Server.UrlEncode(_drow["TOSUITE_BRAND"].ToString()) + "&byp=2&bypcat=1", false);
                                }
                                else if (filterval == null && Request.QueryString["tsb"] != null && Request.QueryString["tsb"].ToString() != "" && _drow["TOSUITE_BRAND"].ToString() == Server.UrlDecode(Request.QueryString["tsb"].ToString()) && selstate == false)
                                {
                                    filterval = new string[2];
                                    filterval[0] = _drow["TOSUITE_BRAND"].ToString();
                                    filterval[1] = _drow["TOSUITE_BRAND"].ToString();
                                    _stmpl_records_tmpl.SetAttribute("TBW_CATEGORY_ID_SELECTED", "SELECTED=\"SELECTED\" ");
                                    //Response.Redirect("categorylist.aspx?ld=0&cid=" + _drow["CATEGORY_ID"].ToString() + "&byp=2&bypcat=1", false);
                                }
                                _stmpl_records_tmpl.SetAttribute("TBW_LIST_VAL", _drow["TOSUITE_BRAND"].ToString());
                                lstrecords[ictrecords] = new TBWDataList(_stmpl_records_tmpl.ToString());
                                ictrecords++;

                            }
                            _stmpl_records_container_tmpl = _stg_records_container.GetInstanceOf("searchbybrandcategory" + "\\" + "multilistcontainer");
                            _stmpl_records_container_tmpl.SetAttribute("TBWDataList", lstrecords);
                            _stmpl_records_container_tmpl.SetAttribute("TBW_SELECTED_ID", "1");
                            lstcontainers[0] = new TBWDataList(_stmpl_records_container_tmpl.ToString());

                        }
                        if (Request.QueryString["tsb"] != null && Request.QueryString["tsb"].ToString() != "")
                        {
                            tempCID = Request.QueryString["cid"].ToString();
                            bool selstate1 = false;
                            DataSet dsCat1 = new DataSet();
                            //dsCat1 = oCat.GetWESModel(tempCID, Convert.ToInt32(oHelper.GetOptionValues("DEFAULT CATALOG").ToString()), Server.UrlDecode(Request.QueryString["tsb"].ToString()));
                            //dsCat1 = EasyAsk.GetWESModel(tempCName, Convert.ToInt32(oHelper.GetOptionValues("DEFAULT CATALOG").ToString()), Server.UrlDecode(Request.QueryString["tsb"].ToString())); 
                            dsCat1 = (DataSet)HttpContext.Current.Session["WESBrand_Model"];
                            if (dsCat1 != null && dsCat1.Tables.Count > 0 && (dsCat1.Tables[0].Rows.Count > 0))
                            {
                                ictrecords = 0;
                                DataRow[] _DCRow = null;
                                _DCRow = dsCat1.Tables[0].Select();
                                if (_DCRow != null && _DCRow.Length > 0)
                                {
                                    lstrecords = new TBWDataList[_DCRow.Count() + 1];
                                    _stg_records = new StringTemplateGroup("searchrsltmultilistitem", stemplatepath);
                                    _stg_records_container = new StringTemplateGroup("searchrsltmultilistcontainer", stemplatepath);
                                    _stmpl_records_tmpl = _stg_records.GetInstanceOf("searchbybrandcategory" + "\\" + "multilistitem");
                                    _stmpl_records_tmpl.SetAttribute("TBW_LIST_VAL1", "List all models");
                                    _stmpl_records_tmpl.SetAttribute("TBW_LIST_VAL", "List all models");
                                    lstrecords[ictrecords] = new TBWDataList(_stmpl_records_tmpl.ToString());
                                    ictrecords++;
                                    foreach (DataRow _drow in _DCRow)
                                    {
                                        _stmpl_records_tmpl = _stg_records.GetInstanceOf("searchbybrandcategory" + "\\" + "multilistitem");
                                        _stmpl_records_tmpl.SetAttribute("TBW_LIST_VAL1", _drow["TOSUITE_MODEL"].ToString());
                                        if (filterval1 != null && _drow["TOSUITE_MODEL"].ToString() == filterval1[0].ToString())
                                        {
                                            _stmpl_records_tmpl.SetAttribute("TBW_CATEGORY_ID_SELECTED", "SELECTED=\"SELECTED\" ");
                                            selstate1 = true;
                                            //Response.Redirect("product_list.aspx?&cid=" + _drow["CATEGORY_ID"].ToString() + "&byp=2", false);
                                            Response.Redirect("bybrand.aspx?&id=0&cid=" + tempCID + "&tsb=" + Server.UrlEncode(Request.QueryString["tsb"].ToString()) + "&tsm=" + Server.UrlEncode(_drow["TOSUITE_MODEL"].ToString()) + "&byp=2&bypcat=1", false);
                                        }
                                        else if (filterval1 == null && Request.QueryString["tsm"] != null && _drow["TOSUITE_MODEL"].ToString() == Server.UrlDecode(Request.QueryString["tsm"].ToString()) && selstate1 == false)
                                        {
                                            filterval1 = new string[2];
                                            filterval1[0] = _drow["TOSUITE_MODEL"].ToString();
                                            filterval1[1] = _drow["TOSUITE_MODEL"].ToString();
                                            _stmpl_records_tmpl.SetAttribute("TBW_CATEGORY_ID_SELECTED", "SELECTED=\"SELECTED\" ");

                                        }
                                        _stmpl_records_tmpl.SetAttribute("TBW_LIST_VAL", _drow["TOSUITE_MODEL"].ToString());
                                        lstrecords[ictrecords] = new TBWDataList(_stmpl_records_tmpl.ToString());
                                        ictrecords++;

                                    }
                                    _stmpl_records_container_tmpl = _stg_records_container.GetInstanceOf("searchbybrandcategory" + "\\" + "multilistcontainer");
                                    _stmpl_records_container_tmpl.SetAttribute("TBWDataList", lstrecords);
                                    _stmpl_records_container_tmpl.SetAttribute("TBW_SELECTED_ID", "2");
                                    lstcontainers[1] = new TBWDataList(_stmpl_records_container_tmpl.ToString());
                                }
                            }
                            dsCat1.Dispose();
                        }
                        else
                        {
                            lstrecords = new TBWDataList[1];
                            _stg_records = new StringTemplateGroup("searchrsltmultilistitem", stemplatepath);
                            _stg_records_container = new StringTemplateGroup("searchrsltmultilistcontainer", stemplatepath);
                            _stmpl_records_tmpl = _stg_records.GetInstanceOf("searchbybrandcategory" + "\\" + "multilistitem");
                            _stmpl_records_tmpl.SetAttribute("TBW_LIST_VAL1", "List all models");
                            _stmpl_records_tmpl.SetAttribute("TBW_LIST_VAL", "List all models");
                            lstrecords[0] = new TBWDataList(_stmpl_records_tmpl.ToString());
                            _stmpl_records_container_tmpl = _stg_records_container.GetInstanceOf("searchbybrandcategory" + "\\" + "multilistcontainer");
                            _stmpl_records_container_tmpl.SetAttribute("TBWDataList", lstrecords);
                            _stmpl_records_container_tmpl.SetAttribute("TBW_SELECTED_ID", "2");
                            lstcontainers[1] = new TBWDataList(_stmpl_records_container_tmpl.ToString());
                        }

                        //if (Request.QueryString["tsm"] != null && Request.QueryString["tsm"].ToString() != "")
                        //{
                        //    /* DataSet dsCat2 = new DataSet();
                        //     bool selstate2 = false;
                        //     dsCat2 = oCat.GetWESFamily(tempCID, Convert.ToInt32(oHelper.GetOptionValues("DEFAULT CATALOG").ToString()), Request.QueryString["tsb"].ToString(), Server.UrlDecode(Request.QueryString["tsm"].ToString()));
                        //     if (dsCat2 != null && dsCat2.Tables.Count > 0 && (dsCat2.Tables[0].Rows.Count > 0))
                        //     {
                        //         ictrecords = 0;

                        //         lstrecords = new TBWDataList[dsCat2.Tables[0].Rows.Count + 1];
                        //         _stg_records = new StringTemplateGroup("searchrsltmultilistitem", stemplatepath);
                        //         _stg_records_container = new StringTemplateGroup("searchrsltmultilistcontainer", stemplatepath);
                        //         _stmpl_records_tmpl = _stg_records.GetInstanceOf("searchbybrand" + "\\" + "multilistitem");
                        //         _stmpl_records_tmpl.SetAttribute("TBW_LIST_VAL1", "List all products");
                        //         _stmpl_records_tmpl.SetAttribute("TBW_LIST_VAL", "List all products");
                        //         lstrecords[ictrecords] = new TBWDataList(_stmpl_records_tmpl.ToString());
                        //         ictrecords++;
                        //         foreach (DataRow _drow in dsCat2.Tables[0].Rows)
                        //         {
                        //             _stmpl_records_tmpl = _stg_records.GetInstanceOf("searchbybrand" + "\\" + "multilistitem");
                        //             _stmpl_records_tmpl.SetAttribute("TBW_LIST_VAL1", _drow["SUBCATID_L1"].ToString() + "|" + _drow["SUBCATID_L2"].ToString());
                        //             string byproduct = "";
                        //             if (_drow["BYPRODUCT"].ToString().EndsWith("- ") == true)
                        //             {
                        //                 byproduct = _drow["BYPRODUCT"].ToString().Replace("- ", "");
                        //             }
                        //             else
                        //             {
                        //                 byproduct = _drow["BYPRODUCT"].ToString();
                        //             }
                        //             if (filterval2 != null && _drow["SUBCATID_L1"].ToString() + "|" + _drow["SUBCATID_L2"].ToString() == filterval2[0].ToString() && filterval2[0].EndsWith("|") == false)
                        //             {
                        //                 _stmpl_records_tmpl.SetAttribute("TBW_CATEGORY_ID_SELECTED", "SELECTED=\"SELECTED\" ");
                        //                 selstate2 = true;
                        //                 Response.Redirect("bybrand.aspx?&id=0&cid=" + tempCID + "&tsb=" + Server.UrlEncode(Request.QueryString["tsb"].ToString()) + "&tsm=" + Server.UrlEncode(Request.QueryString["tsm"].ToString()) + "&sl1=" + _drow["SUBCATID_L1"].ToString() + "&sl2=" + _drow["SUBCATID_L2"].ToString() + "&byp=2&bypcat=1", false);
                        //             }
                        //             else if (filterval2 == null && Request.QueryString["sl1"] != null && Request.QueryString["sl1"].ToString() != "" && Request.QueryString["sl2"] != null && Request.QueryString["sl2"].ToString() != "" && _drow["SUBCATID_L1"].ToString() == Request.QueryString["sl1"].ToString() && _drow["SUBCATID_L2"].ToString() == Request.QueryString["sl2"].ToString() && selstate2 == false)
                        //             {
                        //                 filterval2 = new string[2];
                        //                 filterval2[0] = _drow["SUBCATID_L1"].ToString() + "|" + _drow["SUBCATID_L2"].ToString();
                        //                 filterval2[1] = _drow["BYPRODUCT"].ToString();
                        //                 _stmpl_records_tmpl.SetAttribute("TBW_CATEGORY_ID_SELECTED", "SELECTED=\"SELECTED\" ");
                        //             }
                        //             else if (filterval2 != null && _drow["SUBCATID_L2"].ToString() == "" && _drow["SUBCATID_L1"].ToString() + "|" == filterval2[0].ToString())
                        //             {
                        //                 _stmpl_records_tmpl.SetAttribute("TBW_CATEGORY_ID_SELECTED", "SELECTED=\"SELECTED\" ");
                        //                 selstate2 = true;
                        //                 Response.Redirect("bybrand.aspx?&id=0&cid=" + tempCID + "&tsb=" + Server.UrlEncode(Request.QueryString["tsb"].ToString()) + "&tsm=" + Server.UrlEncode(Request.QueryString["tsm"].ToString()) + "&sl1=" + _drow["SUBCATID_L1"].ToString() + "&sl2=0&byp=2&bypcat=1", false);
                        //             }
                        //             else if (filterval2 == null && Request.QueryString["sl1"] != null && Request.QueryString["sl1"].ToString() != "" && Request.QueryString["sl2"] != null && Request.QueryString["sl2"].ToString() != "" && Request.QueryString["sl2"].ToString() == "0" && _drow["SUBCATID_L1"].ToString() == Request.QueryString["sl1"].ToString() && _drow["SUBCATID_L2"].ToString() == "" && selstate2 == false)
                        //             {
                        //                 filterval2 = new string[2];
                        //                 filterval2[0] = _drow["SUBCATID_L1"].ToString() + "|" + _drow["SUBCATID_L2"].ToString();
                        //                 filterval2[1] = byproduct;
                        //                 _stmpl_records_tmpl.SetAttribute("TBW_CATEGORY_ID_SELECTED", "SELECTED=\"SELECTED\" ");
                        //             }
                        //             _stmpl_records_tmpl.SetAttribute("TBW_LIST_VAL", byproduct);
                        //             lstrecords[ictrecords] = new TBWDataList(_stmpl_records_tmpl.ToString());
                        //             ictrecords++;

                        //         }
                        //         _stmpl_records_container_tmpl = _stg_records_container.GetInstanceOf("searchbybrand" + "\\" + "multilistcontainer");
                        //         _stmpl_records_container_tmpl.SetAttribute("TBWDataList", lstrecords);
                        //         _stmpl_records_container_tmpl.SetAttribute("TBW_SELECTED_ID", "3");
                        //         lstcontainers[2] = new TBWDataList(_stmpl_records_container_tmpl.ToString());
                        //     }*/
                        //}
                        //else
                        //{
                        //    lstrecords = new TBWDataList[2];
                        //    _stg_records = new StringTemplateGroup("searchrsltmultilistitem", stemplatepath);
                        //    _stg_records_container = new StringTemplateGroup("searchrsltmultilistcontainer", stemplatepath);
                        //    _stmpl_records_tmpl = _stg_records.GetInstanceOf("searchbybrandcategory" + "\\" + "multilistitem");
                        //    _stmpl_records_tmpl.SetAttribute("TBW_LIST_VAL1", "Select Category");
                        //    _stmpl_records_tmpl.SetAttribute("TBW_LIST_VAL", "Select Category");
                        //    lstrecords[0] = new TBWDataList(_stmpl_records_tmpl.ToString());
                        //    _stmpl_records_container_tmpl = _stg_records_container.GetInstanceOf("searchbybrandcategory" + "\\" + "multilistcontainer");
                        //    _stmpl_records_container_tmpl.SetAttribute("TBWDataList", lstrecords);
                        //    _stmpl_records_container_tmpl.SetAttribute("TBW_SELECTED_ID", "3");
                        //    lstcontainers[2] = new TBWDataList(_stmpl_records_container_tmpl.ToString());
                        //}

                    }
                //}
                _stg_main_container = new StringTemplateGroup("searchrsltmultilistmain", stemplatepath);
                _stmpl_main_container_tmpl = _stg_main_container.GetInstanceOf("searchbybrandcategory" + "\\" + "multilistmain");
                _stmpl_main_container_tmpl.SetAttribute("TBW_CATEGORY_NAME", tempCName);
                _stmpl_main_container_tmpl.SetAttribute("TBWDataList", lstcontainers);
                sHTML = _stmpl_main_container_tmpl.ToString();
            }
        }
        catch (Exception ex)
        {
            sHTML = ex.Message;
        }
        return sHTML;
    }

    private string Getdropdwoncatid(string catID)
    {
        DataSet DSBC = null;
        //DataSet DSUBC = null;
        string catIDtemp = catID;
        do
        {
            DSBC = GetDataSet("SELECT CATEGORY_NAME,CATEGORY_ID FROM TB_CATEGORY WHERE CATEGORY_ID IN(SELECT PARENT_CATEGORY FROM TB_CATEGORY WHERE CATEGORY_ID = '" + catIDtemp + "')");

            if (DSBC != null && DSBC.Tables[0] != null && DSBC.Tables[0].Rows != null && DSBC.Tables[0].Rows.Count > 0)
                foreach (DataRow DR in DSBC.Tables[0].Rows)
                {
                    if (DR["CATEGORY_NAME"].ToString().ToUpper() == "PRODUCT" || DR["CATEGORY_NAME"].ToString().ToUpper() == "BRAND")
                    {
                        return catIDtemp;

                    }
                    else
                    {
                        catIDtemp = DR["CATEGORY_ID"].ToString();
                    }
                }
            else
                return "0";
        } while (DSBC.Tables[0].Rows[0].ItemArray[1].ToString() != "0");
        return DSBC.Tables[0].Rows[0]["CATEGORY_ID"].ToString();
    }
    private string GetCID(string catID)
    {
        DataSet DSBC = null;
        DataSet DSUBC = null;
        string catIDtemp = catID;
        do
        {
            DSBC = GetDataSet("SELECT CATEGORY_NAME,PARENT_CATEGORY,CATEGORY_ID FROM TB_CATEGORY WHERE CATEGORY_ID = '" + catIDtemp + "'");
            foreach (DataRow DR in DSBC.Tables[0].Rows)
            {
                catIDtemp = DR["PARENT_CATEGORY"].ToString();
                if (catIDtemp == "0")
                {
                    DSUBC = GetDataSet("SELECT CATEGORY_NAME,PARENT_CATEGORY,CATEGORY_ID FROM TB_CATEGORY WHERE PARENT_CATEGORY = '" + DR["CATEGORY_ID"].ToString() + "' AND CATEGORY_NAME Like 'brand'");
                    return DSUBC.Tables[0].Rows[0]["CATEGORY_ID"].ToString();
                }
            }
        } while (DSBC.Tables[0].Rows[0].ItemArray[1].ToString() != "0");
        return DSBC.Tables[0].Rows[0]["CATEGORY_ID"].ToString();
    }
    private string GetCName(string catID)
    {
        DataSet DSBC = null;
        string catIDtemp = catID;
        do
        {
            DSBC = GetDataSet("SELECT CATEGORY_NAME,PARENT_CATEGORY,CATEGORY_ID FROM TB_CATEGORY WHERE CATEGORY_ID = '" + catIDtemp + "'");
            foreach (DataRow DR in DSBC.Tables[0].Rows)
            {
                catIDtemp = DR["PARENT_CATEGORY"].ToString();
                if (catIDtemp == "0")
                {
                    // DSUBC = GetDataSet("SELECT CATEGORY_NAME,PARENT_CATEGORY,CATEGORY_ID FROM TB_CATEGORY WHERE PARENT_CATEGORY = '" + DR["CATEGORY_ID"].ToString() + "' AND CATEGORY_NAME Like 'Product'");
                    return DR["CATEGORY_NAME"].ToString();
                }
            }
        } while (DSBC.Tables[0].Rows[0].ItemArray[1].ToString() != "0");
        return DSBC.Tables[0].Rows[0]["CATEGORY_NAME"].ToString();
    }

}
