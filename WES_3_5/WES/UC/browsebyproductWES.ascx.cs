using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TradingBell.Common;
using TradingBell.WebServices;
using TradingBell5.CatalogX;
using System.Data.SqlClient;
using TradingBell.WebCat.CatalogDB;
using TradingBell.WebCat.Helpers;
using TradingBell.WebCat.TemplateRender;
using TradingBell.WebCat.CommonServices;
public partial class UC_browsebyproductWES : System.Web.UI.UserControl
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
    //int _CatalogId=1;
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
    string _catalogid = "";
    string[] IDS = null;
    int naviinc = 0;
    #endregion
    
    protected void Page_Load(object sender, EventArgs e)
    {       
            oHelper = new HelperDB();
            oErr = new ErrorHandler();
            oCat = new Category();
            oPF = new ProductFamily();
            oPR = new ProductRender();
            TVProduct.ID = "TVProduct";            
            _catalogid = oHelper.GetOptionValues("DEFAULT CATALOG").ToString();

            if (Request.QueryString["cid"] != null && Request.QueryString["cid"].ToString() != "" && Request.QueryString["sl1"] != null && Request.QueryString["sl1"].ToString() != "" && Request.QueryString["sl2"] != null && Request.QueryString["sl2"].ToString() != "")
            {
                valuepath = Request.QueryString["cid"].ToString() + ">" + Server.HtmlDecode(Request.QueryString["sl1"].ToString()) + ">" + Server.HtmlDecode(Request.QueryString["sl2"].ToString());

            }
            else if (Request.QueryString["cid"] != null && Request.QueryString["cid"].ToString() != "" && Request.QueryString["sl1"] != null && Request.QueryString["sl1"].ToString() != "")
            {

                valuepath = Request.QueryString["cid"].ToString() + ">" + Request.QueryString["sl1"].ToString();

            }
    }

    public string ST_Browsebycategory()
    {
        if (Request.QueryString["pcid"] != null )//&& CID!="0")
        {
            HttpContext.Current.Session["BROWSEBYPRODUCT"] = Request.QueryString["pcid"];
            HttpContext.Current.Session["BROWSEBYBRAND"] = Request.QueryString["cid"];
            HelperDB oHelper = new HelperDB();
            ConnectionDB conStr = new ConnectionDB();
            TBWTemplateEngine tbwtEngine = new TBWTemplateEngine("BROWSEBYPRODUCT", Server.MapPath(oHelper.GetOptionValues("SHOPPTING TEMPLATE PATH").ToString()), conStr.ConnectionString);
            tbwtEngine.paraValue = CID;
            tbwtEngine.RenderHTML("Row");
            return (tbwtEngine.RenderedHTML);
        }
        else if (Request.QueryString["cid"] != null)// && CID != "0")
        {
            HelperDB oHelper = new HelperDB();
            ConnectionDB conStr = new ConnectionDB();
            TBWTemplateEngine tbwtEngine = new TBWTemplateEngine("BROWSEBYPRODUCT", Server.MapPath(oHelper.GetOptionValues("SHOPPTING TEMPLATE PATH").ToString()), conStr.ConnectionString);
            tbwtEngine.paraValue = Request.QueryString["cid"].ToString();
            //if (Request.Url.ToString().ToLower().Contains("bybrand.aspx"))
            //    tbwtEngine.paraValue = CID;
            tbwtEngine.RenderHTML("Row");
            return (tbwtEngine.RenderedHTML);
        }
        return "<table width=\"0\" style=\"display :none;\" height=\"0px\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\"><tr><td><div height=\"0px\" style =\"display:none;\"><table width=\"0\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" style=\"display :none;\"><tr><td align=\"LEFT\" valign=\"bottom\" width=\"0\" ><ul>";
    }

    private DataSet GetDataSet(string SQLQuery)
    {
        DataSet ds = new DataSet();
        SqlDataAdapter da = new SqlDataAdapter(SQLQuery, conStr.ConnectionString.ToString().Substring(conStr.ConnectionString.ToString().IndexOf(';') + 1));
        da.Fill(ds, "generictable");
        return ds;
    }

    private string GetCID(string familyid)
    {
        DataSet DSBC = null;
        DataSet DSUBC = null;
        string catIDtemp = "";
        DSBC = GetDataSet("SELECT family_NAME,category_id FROM TB_family WHERE family_ID = " + familyid);
        foreach (DataRow DR in DSBC.Tables[0].Rows)
        {
            catIDtemp = DR[1].ToString();
        }
        do
        {
            DSBC = GetDataSet("SELECT CATEGORY_NAME,PARENT_CATEGORY,CATEGORY_ID FROM TB_CATEGORY WHERE CATEGORY_ID = '" + catIDtemp + "'");
            foreach (DataRow DR in DSBC.Tables[0].Rows)
            {
                catIDtemp = DR["PARENT_CATEGORY"].ToString();
                if (catIDtemp == "0")
                {
                    DSUBC = GetDataSet("SELECT CATEGORY_NAME,PARENT_CATEGORY,CATEGORY_ID FROM TB_CATEGORY WHERE PARENT_CATEGORY = '" + DR["CATEGORY_ID"].ToString() + "' AND CATEGORY_NAME Like 'Product'");
                    if (DSUBC.Tables[0].Rows.Count <= 0)
                        return "0";
                    else
                        return DSUBC.Tables[0].Rows[0]["CATEGORY_ID"].ToString();
                }
                MCID = DR["CATEGORY_ID"].ToString() + ">" + MCID;
            }
        } while (DSBC.Tables[0].Rows[0].ItemArray[1].ToString() != "0");
        MCID = DSBC.Tables[0].Rows[0]["CATEGORY_ID"].ToString() + ">" + MCID;
        return DSBC.Tables[0].Rows[0]["CATEGORY_ID"].ToString();
    }
    private string GetMCatID(string MainCategoryid)
    {
        DataSet DSBC = null;
        DataSet DSUBC = null;
        string catIDtemp = "";
        catIDtemp = MainCategoryid;
        MCID = MainCategoryid + ">" + MCID;
        
        do
         {
            DSBC = GetDataSet("SELECT CATEGORY_NAME,PARENT_CATEGORY,CATEGORY_ID FROM TB_CATEGORY WHERE CATEGORY_ID = '" + catIDtemp + "'");
            foreach (DataRow DR in DSBC.Tables[0].Rows)
            {
                catIDtemp = DR["PARENT_CATEGORY"].ToString();
                if (catIDtemp == "0")
                {
                    DSUBC = GetDataSet("SELECT CATEGORY_NAME,PARENT_CATEGORY,CATEGORY_ID FROM TB_CATEGORY WHERE PARENT_CATEGORY = '" + DR["CATEGORY_ID"].ToString() + "' AND CATEGORY_NAME Like 'Product'");
                    if (DSUBC.Tables[0].Rows.Count > 0)
                        return DSUBC.Tables[0].Rows[0]["CATEGORY_ID"].ToString();
                    else
                        return "0";
                }
                //CID = DR["PARENT_CATEGORY"].ToString() + ">" + DR["CATEGORY_ID"].ToString();
                MCID = DR["PARENT_CATEGORY"].ToString() + ">" + MCID;
            }
         } while (DSBC.Tables[0].Rows[0].ItemArray[1].ToString() != "0");
        return DSBC.Tables[0].Rows[0]["CATEGORY_ID"].ToString();
    }
    #region "Functions"
    public void LoadCategoryTree(string CatID)
    {
        DataSet dsCat = new DataSet();
        int FCnt;

        dsCat = oCat.GetSubCategories(CatID.ToString(), Convert.ToInt32(oHelper.GetOptionValues("DEFAULT CATALOG").ToString()));
        if (TVProduct.Nodes.Count > 0)
        {
            TVProduct.Nodes.Clear();
        }
        if (dsCat != null)
        {
            foreach (DataRow rCat in dsCat.Tables[0].Rows)
            {
                System.Web.UI.WebControls.TreeNode parentNode = new TreeNode();

                parentNode.Text = rCat["CATEGORY_NAME"].ToString();
                parentNode.Value = rCat["CATEGORY_ID"].ToString();
                parentNode.NavigateUrl = "~/byproduct.aspx?&ld=0&&cid=" + parentNode.Value;             

                if ((parentNode.Text.ToUpper() != "DEFAULT CATEGORY"))
                    if (parentNode.Text != "General Category")
                        TVProduct.Nodes.Add(parentNode); 
                    LoadSubCategories(parentNode.Value, parentNode);
            }
            dsCat.Dispose();
        }
    }       

    public void LoadSubCategories(string CateID, TreeNode ParentNode)
    {
        DataSet dsCat = new DataSet();
        dsCat = oCat.GetSubCategories(CateID, Convert.ToInt32(oHelper.GetOptionValues("DEFAULT CATALOG").ToString()));
        if (dsCat != null)
        {
            foreach (DataRow rSCat in dsCat.Tables[0].Rows)
            {
                System.Web.UI.WebControls.TreeNode subCatNode = new TreeNode();                               
                subCatNode.Text = rSCat["CATEGORY_NAME"].ToString();                
                subCatNode.Value = rSCat["CATEGORY_ID"].ToString();
                subCatNode.NavigateUrl = "~/byproduct.aspx?&ld=0&&cid=" + subCatNode.Value;
                ParentNode.ChildNodes.Add(subCatNode);
                LoadSubCategories(subCatNode.Value, subCatNode);
            }
            dsCat.Dispose();
        }



    }
    protected void PopulateNode(object sender, TreeNodeEventArgs e)
    {
        if (e.Node.Depth == 0)
        {
            GetCategories(e.Node);
        }
        //else
        //{
        //    GetSubcategory(e.Node);
        //}
    }
    protected void ConstructRootTree(object sender, EventArgs e)
    {
        if (TVProduct.Nodes.Count == 0)
        {
            IDS = MCID.Split('>');
            string valuepath = "";
            
            if (Request.QueryString["cid"] != null || CID.ToString() != "")
            {
                if (Request.QueryString["cid"] != null)
                {
                   
                    string CateID = "";
                    CateID = Request.QueryString["cid"].ToString();                    
                    if (CateID != "0")
                    {
                        DataSet dsCat = new DataSet();
                        dsCat = null;
                        dsCat = oCat.GetWESproductL1(CateID, Convert.ToInt32(oHelper.GetOptionValues("DEFAULT CATALOG").ToString()));
                        if (dsCat != null)
                        {
                            foreach (DataRow rSCat in dsCat.Tables[0].Rows)
                            {
                                
                                    TreeNode newNode = new TreeNode(rSCat["SUBCATNAME_L1"].ToString(),CateID + ">" + rSCat["SUBCATID_L1"].ToString());
                                    //newNode.NavigateUrl = "~/byproduct.aspx?&ld=0&cid=" + CateID + "&sl2=" + rSCat["CATEGORY_ID"].ToString() + "&byp=2";
                                    if (oCat.GetWESProductL2count(CateID, Convert.ToInt32(oHelper.GetOptionValues("DEFAULT CATALOG").ToString()), rSCat["SUBCATID_L1"].ToString()) > 0)
                                    {
                                        newNode.PopulateOnDemand = true;
                                    }
                                    else
                                    {
                                        newNode.NavigateUrl = "~/product_list.aspx?&ld=0&&cid=" + CateID.Split(new char[] { '>' })[0].ToString() + "&sl1=" + rSCat["SUBCATID_L1"].ToString() + "&byp=2&qf=1";
                                    }
                                    TreeNode parentNode;
                                    if (Request.QueryString["cid"] != null && Request.QueryString["cid"].ToString() != "" && Request.QueryString["sl1"] != null && Request.QueryString["sl1"].ToString() != "")
                                    {
                                        parentNode = new TreeNode();
                                        valuepath = Request.QueryString["cid"].ToString() + ">" + Request.QueryString["sl1"].ToString();
                                        parentNode = TVProduct.FindNode(valuepath);
                                        if (parentNode != null)
                                        {
                                            parentNode.Expand();
                                            parentNode.Text = "<div style='color:Black'>" + parentNode.Text + "</div>";
                                            parentNode.Selected = true;

                                        }
                                    }                                  
                                  
                                    newNode.Text = "<div style='border-bottom-width: 1px; border-bottom-style: solid; border-bottom-color:#d1d1d1; width:100%;cursor: hand;'>" + newNode.Text + "</div>";
                                    newNode.SelectAction = TreeNodeSelectAction.Expand;
                                    TVProduct.Nodes.Add(newNode);
                                
                            }                            
                            dsCat.Dispose();
                        }
                    }
                }
            }
        }        
    }

    void GetCategories(TreeNode node)
    {
        DataSet dsCat = new DataSet();
        DataSet dscatid = null;
        string CateID = node.Value;
        dsCat = oCat.GetWESproductL2(CateID.Split(new char[] { '>' })[0].ToString(), Convert.ToInt32(oHelper.GetOptionValues("DEFAULT CATALOG").ToString()), CateID.Split(new char[] { '>' })[1].ToString());
        if (dsCat != null)
        {
            foreach (DataRow rSCat in dsCat.Tables[0].Rows)
            {
                TreeNode newNode = new TreeNode(rSCat["SUBCATNAME_L2"].ToString(),CateID.Split(new char[] { '>' })[0].ToString() + ">" + CateID.Split(new char[] { '>' })[1].ToString() + ">" + rSCat["SUBCATID_L2"].ToString());
                newNode.NavigateUrl = "~/product_list.aspx?&ld=0&&cid=" + CateID.Split(new char[] { '>' })[0].ToString() + "&sl1=" + CateID.Split(new char[] { '>' })[1].ToString() + "&sl2=" + rSCat["SUBCATID_L2"].ToString() + "&byp=2&qf=1";
                newNode.SelectAction = TreeNodeSelectAction.Expand;
                if (newNode.Value == valuepath)
                {
                    newNode.Text = "<div style='color:Black'>" + newNode.Text + "</div>";
                    newNode.Selected = true;
                }                
                node.ChildNodes.Add(newNode);
            }
            dsCat.Dispose();
        }
    }

    void GetSubcategory(TreeNode node)
    {
        DataSet dsCat = new DataSet();
        DataSet dscatid = null;
        string CateID = node.Value;
        dsCat = oCat.GetSubCategories(CateID, Convert.ToInt32(oHelper.GetOptionValues("DEFAULT CATALOG").ToString()));
        if (dsCat != null)
        {
            foreach (DataRow rSCat in dsCat.Tables[0].Rows)
            {
                TreeNode newNode = new TreeNode(rSCat["CATEGORY_NAME"].ToString(), rSCat["CATEGORY_ID"].ToString());
                if (oCat.GetSubCategoriesCount(rSCat["CATEGORY_ID"].ToString(), Convert.ToInt32(oHelper.GetOptionValues("DEFAULT CATALOG").ToString())) > 0)
                {
                    newNode.Text = "<p>&nbsp;&nbsp;" +newNode.Text+"</p>";
                    newNode.PopulateOnDemand = true;
                }
                newNode.NavigateUrl = "~/byproduct.aspx?&ld=0&&cid=" + rSCat["CATEGORY_ID"].ToString() + "&byp=1"; ;
                newNode.SelectAction = TreeNodeSelectAction.Expand;
                if (IDS != null && IDS.Length >= naviinc)
                    if (newNode.Value == IDS[naviinc])
                    {
                        dscatid = oCat.GetSubCategories(IDS[naviinc], Convert.ToInt32(oHelper.GetOptionValues("DEFAULT CATALOG").ToString()));
                        if (dscatid != null && dscatid.Tables[0].Rows.Count > 0)
                        {
                            naviinc += 1;                            
                            newNode.Expand();
                        }
                        else
                        {
                            newNode.Text = "<div style='color:Black'>" + newNode.Text + "</div>";
                            newNode.Selected = true;
                        }
                    }
                node.ChildNodes.Add(newNode);
                
            }
            dsCat.Dispose();
        }
    }
    protected void TVProduct_SelectedNodeChanged(object sender, EventArgs e)
    {
        //PopulateNode(sender,e);

        if (TVProduct.SelectedNode.Depth == 0)
        {
            GetCategories(TVProduct.SelectedNode);

        }
        //else
        //{
        //    GetSubcategory(TVProduct.SelectedNode);
        //}

    }

#endregion
}
