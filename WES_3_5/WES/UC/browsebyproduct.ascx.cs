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
public partial class UC_browsebyproduct : System.Web.UI.UserControl
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
            //if (TVProduct.Nodes.Count > 0)
            //  TVProduct.Nodes.Clear();
            _catalogid = oHelper.GetOptionValues("DEFAULT CATALOG").ToString();
            if (Request.QueryString["fid"] != null && Request.QueryString["fid"] != "")
            {
                CID = GetCID(Request.QueryString["fid"].ToString());
            }
            if (Request.QueryString["cid"] != null || CID.ToString() != "")
            {
                if (Request.QueryString["cid"] != null)
                {
                    CID = GetMCatID(Request.QueryString["cid"].ToString());
                }
            }
            //LoadCategoryTree(CID);
            System.Web.UI.WebControls.TreeNode parentNode;
            string[] IDS = MCID.Split('>');
            string valuepath = "";
            for (int idValue = 2; idValue < IDS.Length - 1; idValue++)
            {
                if (valuepath == "")
                    valuepath = IDS[idValue].ToString();
                else
                    valuepath = valuepath + "/" + IDS[idValue].ToString();
                parentNode = new TreeNode();
                parentNode = TVProduct.FindNode(valuepath);
                if (parentNode != null)
                {
                    parentNode.Expand();
                    if (idValue == IDS.Length - 2)
                    {
                        parentNode.Selected = true;
                    }
                }
            }
           
    }

    public string ST_Browsebycategory()
    {
        if (Request.QueryString["pcid"] != null && CID!="0")
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
        else if (Request.QueryString["cid"] != null && CID != "0")
        {
            HelperDB oHelper = new HelperDB();
            ConnectionDB conStr = new ConnectionDB();

            TBWTemplateEngine tbwtEngine = new TBWTemplateEngine("BROWSEBYPRODUCT", Server.MapPath(oHelper.GetOptionValues("SHOPPTING TEMPLATE PATH").ToString()), conStr.ConnectionString);
            tbwtEngine.paraValue = Request.QueryString["cid"].ToString();

            if (Request.Url.ToString().ToLower().Contains("bybrand.aspx"))
                tbwtEngine.paraValue = CID;
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
        else
        {
            GetSubcategory(e.Node);
        }
    }
    protected void ConstructRootTree(object sender, EventArgs e)
    {
        if (TVProduct.Nodes.Count == 0)
        {
            IDS = MCID.Split('>');
            //string valuepath = "";
            //for (int idValue = 2; idValue < IDS.Length - 1; idValue++)
            //{
            //    if (valuepath == "")
            //        valuepath = IDS[idValue].ToString();
            //    else
            //        valuepath = valuepath + "/" + IDS[idValue].ToString();
            //    parentNode = new TreeNode();
            //    parentNode = TVBrand.FindNode(valuepath);
            //    if (parentNode != null)
            //    {
            //        parentNode.Expand();
            //        if (idValue == IDS.Length - 2)
            //        {
            //            parentNode.Selected = true;
            //        }
            //    }
            //}
            if (Request.QueryString["cid"] != null || CID.ToString() != "")
            {
                if (Request.QueryString["cid"] != null)
                {
                    //"WES598";
                    string CateID = "";
                    if (Request.QueryString["pcid"] != null)
                    {
                        CateID = Request.QueryString["pcid"].ToString();
                    }
                    else
                    {
                        CateID = GetMCatID(Request.QueryString["cid"].ToString());
                    }
                    if (CateID != "0")
                    {
                        DataSet dsCat = new DataSet();

                        dsCat = oCat.GetSubCategories(CateID, Convert.ToInt32(oHelper.GetOptionValues("DEFAULT CATALOG").ToString()));
                        if (dsCat != null)
                        {
                            foreach (DataRow rSCat in dsCat.Tables[0].Rows)
                            {
                                if ((rSCat["CATEGORY_NAME"].ToString().ToUpper() != "DEFAULT CATEGORY") && (rSCat["CATEGORY_NAME"].ToString() != "General Category"))
                                {
                                    TreeNode newNode = new TreeNode(rSCat["CATEGORY_NAME"].ToString(), rSCat["CATEGORY_ID"].ToString());
                                    if (!(oCat.GetSubCategoriesCount(rSCat["CATEGORY_ID"].ToString(), Convert.ToInt32(oHelper.GetOptionValues("DEFAULT CATALOG").ToString())) > 0))
                                    {
                                        newNode.NavigateUrl = "~/byproduct.aspx?&ld=0&&cid=" + rSCat["CATEGORY_ID"].ToString() + "&byp=1";
                                    }
                                    else
                                    {
                                        newNode.PopulateOnDemand = true;
                                    }
                                   
                                    for (int idValue = 2; idValue < IDS.Length-1 ; idValue++)
                                    {
                                        if (rSCat["CATEGORY_ID"].ToString() == IDS[idValue].ToString())
                                        {
                                            naviinc = idValue + 1;
                                            if ((IDS.Length - 1) == (idValue + 1))
                                            {
                                                valuepath = IDS[idValue].ToString();
                                            }
                                            else if (valuepath == "")
                                                valuepath = IDS[idValue + 1].ToString();
                                            else
                                                valuepath = valuepath + "/" + IDS[idValue + 1].ToString();
                                            newNode.Expand();
                                            TreeNode parentNode = new TreeNode();
                                            parentNode = TVProduct.FindNode(valuepath);
                                            if (parentNode != null)
                                            {
                                                // parentNode.Expand();
                                                //if (idValue == IDS.Length - 2)
                                                {
                                                    parentNode.Text = "<div style='color:Black'>" + parentNode.Text + "</div>";
                                                    parentNode.Selected = true;
                                                }
                                            }
                                        }
                                    }
                                    newNode.Text = "<div style='border-bottom-width: 1px; border-bottom-style: solid; border-bottom-color:#d1d1d1; width:100%;cursor: hand;'>" + newNode.Text + "</div>";
                                    newNode.SelectAction = TreeNodeSelectAction.Expand;
                                    TVProduct.Nodes.Add(newNode);
                                }
                            }
                            if (valuepath == "")
                            {
                                DataSet _dscatname = GetDataSet("select c.category_id,c.category_name from tb_category c,tb_catalog_sections cs where c.category_name in(SELECT CATEGORY_NAME FROM TB_CATEGORY WHERE CATEGORY_ID ='" + Request.QueryString["cid"].ToString() + "') and c.category_id <>'" + Request.QueryString["cid"].ToString() + "' and c.category_id=cs.category_id and cs.catalog_id=" + _catalogid);
                                if (_dscatname != null && _dscatname.Tables[0].Rows.Count > 0)
                                {
                                    TreeNode parentNode = new TreeNode();
                                    foreach (DataRow _row in _dscatname.Tables[0].Rows)
                                    {
                                        parentNode = TVProduct.FindNode(_dscatname.Tables[0].Rows[0][0].ToString());
                                        if (parentNode != null)
                                            parentNode.Expand();
                                    }
                                }
                            }
                            dsCat.Dispose();
                        }
                    }
                }
            }
        }
        //String categoryId = "WES598";
        //SubcategoryList products = CategoryTreeDB.GetSubcategory(categoryId);
        //foreach (Subcategory p in products)
        //{
        //    TreeNode newNode = new TreeNode(p.Name, p.Id);
        //    newNode.PopulateOnDemand = true;
        //    TVBrand.Nodes.Add(newNode);
        //}
    }
    void GetCategories(TreeNode node)
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
                if (!(oCat.GetSubCategoriesCount(rSCat["CATEGORY_ID"].ToString(), Convert.ToInt32(oHelper.GetOptionValues("DEFAULT CATALOG").ToString())) > 0))
                {
                    newNode.NavigateUrl = "~/byproduct.aspx?&ld=0&&cid=" + rSCat["CATEGORY_ID"].ToString() + "&byp=1";
                }
                else
                {
                    newNode.Text = "&nbsp;&nbsp;&nbsp;&nbsp;" + newNode.Text;
                    newNode.PopulateOnDemand = true;
                } 
                newNode.SelectAction = TreeNodeSelectAction.Expand;
                if (IDS != null &&IDS.Length >= naviinc)
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
        else
        {
            GetSubcategory(TVProduct.SelectedNode);
        }

    }

#endregion
}
