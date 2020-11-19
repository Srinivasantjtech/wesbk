using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.ComponentModel;
using TradingBell.Common;
using TradingBell.WebServices;
using System;

/// <summary>
///This web control is used to navigate the different categories and Subcategories.
/// </summary>

[assembly: TagPrefix("TradingBell.WebServices.UI", "WebCat")]
[assembly: System.Reflection.AssemblyVersion("5.0")]

namespace TradingBell.WebServices.UI
{
    public class CategoryNavigator : WebControl
    {
        #region "Declarations"
        Helper oHelper;
        ErrorHandler oErr;
        Category oCat;
        ProductFamily oPF;
        ProductRender oPR;
        TreeView TVCategory = new TreeView();

        string _TvrSkin;
        //String _CategoryText;
        int _CatalogId;
        string _ImagePath;
        string _CategoryLogo;
        string _FamilyLogo;
        string _DisplayCategoryMode;
        string _DisplayFamilyCount;
        string _DisplayProductCount;
        string _DisplayFamilyLogo;
        string _DisplayCategoryLogo;
        string _NaviWidth;
        string _NaviHeight;
        string _NodeExpanded;
        string _CategoryHeaderText;
        string _HeaderCssClass;
        #endregion

        public enum DisplayMode
        {
            No = 0,
            YES = 1
        }
        DisplayMode _CategoryHeaderVisible;
        #region "Property"

        [Browsable(true),
        Category("TradingBell"),
        DefaultValue("")]
        public int DefaultCatalogId
        {
            get
            {
                return _CatalogId;
            }
            set
            {
                _CatalogId = value;
            }


        }
        [Browsable(true),
        Category("TradingBell"),
        DefaultValue("")]
        public string ImagePath
        {
            get
            {
                return _ImagePath;
            }
            set
            {
                _ImagePath = value;
            }
        }
        [Browsable(true),
        Category("TradingBell"),
        DefaultValue("")]
        public string NodeExpanded
        {
            get
            {
                return _NodeExpanded;
            }
            set
            {
                _NodeExpanded = value;
            }
        }
        [Browsable(true),
       Category("TradingBell"),
       DefaultValue("")]
        public string CategoryLogo
        {
            get
            {
                return _CategoryLogo;
            }
            set
            {
                _CategoryLogo = value;
            }
        }
        [Browsable(true),
       Category("TradingBell"),
       DefaultValue("")]
        public string FamilyLogo
        {
            get
            {
                return _FamilyLogo;
            }
            set
            {
                _FamilyLogo = value;
            }
        }
        [Browsable(true),
        Category("TradingBell"),
        DefaultValue("")]
        public string DisplayFamilyCount
        {
            get
            {
                return _DisplayFamilyCount;
            }
            set
            {
                _DisplayFamilyCount = value;
            }
        }
        [Browsable(true),
        Category("TradingBell"),
        DefaultValue("")]
        public string DisplayProductCount
        {
            get
            {
                return _DisplayProductCount;
            }
            set
            {
                _DisplayProductCount = value;
            }
        }
        [Browsable(true),
        Category("TradingBell"),
        DefaultValue("")]
        public string DisplayCategoryLogo
        {
            get
            {
                return _DisplayCategoryLogo;
            }
            set
            {
                _DisplayCategoryLogo = value;
            }
        }
        [Browsable(true),
       Category("TradingBell"),
       DefaultValue("")]
        public string DisplayFamilyLogo
        {
            get
            {
                return _DisplayFamilyLogo;
            }
            set
            {
                _DisplayFamilyLogo = value;
            }
        }
        [Browsable(true),
        Category("TradingBell"),
        DefaultValue("")]
        public string DisplayCategoryMode
        {
            get
            {
                return _DisplayCategoryMode;
            }
            set
            {
                _DisplayCategoryMode = value;
            }

        }
        [Browsable(true),
        Category("TradingBell"),
        DefaultValue("")]
        public string NavigatorWidth
        {
            get
            {
                return _NaviWidth;
            }
            set
            {
                _NaviWidth = value;
            }
        }
        [Browsable(true),
        Category("TradingBell"),
        DefaultValue("")]
        public string NavigatorHeight
        {
            get
            {
                return _NaviHeight;
            }
            set
            {
                _NaviHeight = value;
            }
        }
        [Browsable(true),
        Category("TradingBell"),
        DefaultValue("")]
        public string CategoryHeaderText
        {

            get
            {
                return _CategoryHeaderText;
            }
            set
            {
                _CategoryHeaderText = value;
            }

        }
        [Browsable(true),
        Category("TradingBell"),
        DefaultValue("")]
        public DisplayMode CategoryHeaderVisible
        {

            get
            {
                return _CategoryHeaderVisible;
            }
            set
            {
                _CategoryHeaderVisible = value;
            }

        }
        [Browsable(true),
        Category("TradingBell"),
        DefaultValue("")]
        public string HeaderCssClass
        {

            get
            {
                return _HeaderCssClass;
            }
            set
            {
                _HeaderCssClass = value;
            }

        }
        [Browsable(true),
        Category("TradingBell"),
        DefaultValue("")]
        public override string SkinID
        {

            get
            {
                return _TvrSkin;
            }
            set
            {
                _TvrSkin = value;
            }

        }
        #endregion

        //protected override void OnInit(EventArgs e)
        //{
        //    TVCategory.PreRender += new TreeNodeEventHandler(TVCategory_TreeNodeExpanded);
        //    //  this.TVCategory.Attributes.Add("TreeNodeExpanded", "return TVCategory_TreeNodeExpanded");
        //    TreeView trv = new TreeView();
        //    //TVCategory = new TreeView();
        //    TVCategory.EnableViewState = true; 
        //    base.OnInit(e);
        //}
        //protected override void Render(HtmlTextWriter writer)
        //{
          
        //    base.Render(writer);
        //} 
        protected override void RenderContents(HtmlTextWriter output)
        {
            Label oLbei = new Label();
            Panel oP = new Panel();
            if (DesignMode)
            {
                Image oImg = new Image();
                oImg.ImageUrl = "~/Images/CategoryNavigator.gif";
                oImg.ID = "imgDefault";
                oImg.Width = 125;
                this.Controls.Add(oImg);
                oImg.RenderControl(output);
            }
            else
            {
                if (_CategoryHeaderVisible == TradingBell.WebServices.UI.CategoryNavigator.DisplayMode.YES)
                {
                    Table oTable = new Table();
                    TableRow oRow = new TableRow();
                    TableCell oCell = new TableCell();
                    oCell.Text = _CategoryHeaderText;
                    oCell.HorizontalAlign = HorizontalAlign.Center;
                    oCell.CssClass = _HeaderCssClass;
                    oRow.Controls.Add(oCell);
                    oTable.Controls.Add(oRow);
                    this.Controls.Add(oTable);
                    oTable.RenderControl(output);
                    oTable.Dispose();
                }
                oHelper = new Helper();
                oErr = new ErrorHandler();
                oCat = new Category();
                oPF = new ProductFamily();
                oPR = new ProductRender();
                //   TVCategory = new TreeView();
                TVCategory.ID = "TVCategory";
                TVCategory.SkinID = _TvrSkin;
                //TVCategory.NodeStyle.BorderColor = System.Drawing.Color.White;
                //TVCategory.NodeStyle.BorderWidth =2;
                //TVCategory.TreeNodeExpanded= _NodeExpanded;
                TVCategory.CollapseAll();
                LoadCategoryTree(_CatalogId);

                oP.Style["margin-left"] = "2px";
                oP.Style["margin-top"] = "2px";
                oP.Style["overflow"] = "auto";
                oP.Style["width"] = _NaviWidth;
                oP.Style["height"] = _NaviHeight;
                oP.Controls.Add(TVCategory);
                this.Controls.Add(oP);
                oP.RenderControl(output);
            }
        }
        #region "Functions"
        public void LoadCategoryTree(int CatID)
        {
            DataSet dsCat = new DataSet();
            int FCnt;

            dsCat = oCat.GetRootCategories(CatID);
            if (TVCategory.Nodes.Count > 0)
            {
                TVCategory.Nodes.Clear();
            }
            if (dsCat != null)
            {
                foreach (DataRow rCat in dsCat.Tables[0].Rows)
                {
                    System.Web.UI.WebControls.TreeNode parentNode = new TreeNode();
                    // TreeNode parentNode = new TreeNode();
                    FCnt = oPF.GetFamilyCountWithInventory(rCat["CATEGORY_ID"].ToString(),_CatalogId);
                    if (FCnt > 0 && _DisplayFamilyCount.ToUpper() == "YES")
                    {
                        parentNode.Text = rCat["CATEGORY_NAME"].ToString() + " (" + FCnt + ")";
                    }
                    else
                    {
                        parentNode.Text = rCat["CATEGORY_NAME"].ToString();
                    }
                    parentNode.Value = rCat["CATEGORY_ID"].ToString();
                    int ProdCnt = oPR.GetProductCountByCategory(parentNode.Value, _CatalogId);
                    if (ProdCnt > 0)
                    {
                        DataSet oDS = new DataSet();
                        oDS = oCat.GetSubCategories(rCat["CATEGORY_ID"].ToString(), CatID);
                        if ((oDS != null))
                        {
                            parentNode.NavigateUrl = "~/CategoryDisplay.aspx?CatID=" + parentNode.Value;
                        }
                        else
                        {
                            //parentNode.NavigateUrl = "~/Family.aspx?Cat=" + parentNode.Value;
                            //For IPD Requirement
                            parentNode.NavigateUrl = "~/CategoryDisplay.aspx?CatID=" + parentNode.Value;
                        }
                        if (_DisplayCategoryLogo.ToUpper() == "YES")
                        {
                            parentNode.ImageUrl = _ImagePath + _CategoryLogo;
                        }
                        if ((parentNode.Text.ToUpper() != "DEFAULT CATEGORY"))
                            if (parentNode.Text != "General Category")
                                TVCategory.Nodes.Add(parentNode);
                    }
                    if (_DisplayCategoryMode.ToUpper() == "CATALOG HIERARCHY")
                    {
                        LoadFamily(parentNode.Value, parentNode);
                    }
                    LoadSubCategories(parentNode.Value, parentNode);
                }
                dsCat.Dispose();
            }
        }
        public void LoadSubCategories(string CateID, TreeNode ParentNode)
        {
            DataSet dsCat = new DataSet();
            int FCnt;

            dsCat = oCat.GetSubCategories(CateID,_CatalogId);
            if (dsCat != null)
            {
                foreach (DataRow rSCat in dsCat.Tables[0].Rows)
                {
                    System.Web.UI.WebControls.TreeNode subCatNode = new TreeNode();
                    //TreeNode subCatNode = new TreeNode();

                    FCnt = oPF.GetFamilyCountWithInventory(rSCat["CATEGORY_ID"].ToString(),_CatalogId);
                    if (FCnt > 0 && _DisplayFamilyCount.ToUpper() == "YES")
                    {
                        subCatNode.Text = rSCat["CATEGORY_NAME"].ToString() + " (" + FCnt + ")";
                    }
                    else
                    {
                        subCatNode.Text = rSCat["CATEGORY_NAME"].ToString();
                    }
                    subCatNode.Value = rSCat["CATEGORY_ID"].ToString();
                    int ProdCnt = oPR.GetProductCountByCategory(subCatNode.Value, _CatalogId);
                    if (ProdCnt > 0)
                    {
                        DataSet oDS = new DataSet();
                        oDS = oCat.GetSubCategories(rSCat["CATEGORY_ID"].ToString(), _CatalogId);
                        if ((oDS != null))
                        {
                            subCatNode.NavigateUrl = "~/CategoryDisplay.aspx?CatID=" + subCatNode.Value;
                        }
                        else
                        {
                            subCatNode.NavigateUrl = "~/Family.aspx?Cat=" + subCatNode.Value;
                        }
                        if (_DisplayCategoryLogo.ToUpper() == "YES")
                        {
                            subCatNode.ImageUrl = _ImagePath + _CategoryLogo;
                        }

                        ParentNode.ChildNodes.Add(subCatNode);
                    }
                    if (_DisplayCategoryMode.ToUpper() == "CATALOG HIERARCHY")
                    {
                        LoadFamily(subCatNode.Value, subCatNode);
                    }

                    LoadSubCategories(subCatNode.Value, subCatNode);
                }
                dsCat.Dispose();
            }



        }
        public void LoadFamily(string CatID, TreeNode SubCatNode)
        {
            DataTable dsFam = new DataTable();
            Product oProd = new Product();
            int FCnt;
            int PCnt;

            FCnt = oPF.GetFamilyCount(CatID,_CatalogId);

            if (FCnt > 0)
            {
                //dsFam = oPF.GetCategoryFamilyList(CatID);
                dsFam = oPF.GetRootFamilyList(CatID,_CatalogId);
                if (dsFam != null)
                {
                    foreach (DataRow rFam in dsFam.Rows)
                    {
                        System.Web.UI.WebControls.TreeNode FamilyNode = new TreeNode();
                        //TreeNode FamilyNode = new TreeNode();
                        PCnt = oProd.GetProductCount(oHelper.CI(rFam["FAMILY_ID"].ToString()),_CatalogId);
                        if (PCnt > 0 && _DisplayProductCount.ToUpper() == "YES")
                        {
                            FamilyNode.Text = rFam["FAMILY_NAME"].ToString() + " (" + PCnt + ")";
                        }
                        else
                        {
                            FamilyNode.Text = rFam["FAMILY_NAME"].ToString();
                        }
                        FamilyNode.Value = rFam["FAMILY_ID"].ToString();
                        if (_DisplayFamilyLogo.ToUpper() == "YES")
                        {
                            FamilyNode.ImageUrl = _ImagePath + _FamilyLogo;
                        }
                        FamilyNode.NavigateUrl = "~/Familydisplay.aspx?Fid=" + FamilyNode.Value;
                        SubCatNode.ChildNodes.Add(FamilyNode);
                        if (_DisplayCategoryMode.ToUpper() == "CATALOG HIERARCHY")
                        {
                            LoadSubFamily(oHelper.CI(FamilyNode.Value), FamilyNode);
                        }
                    }
                }
            }

        }
        public void LoadSubFamily(int FamilyID, TreeNode ParentFamilyNode)
        {
            int SubFid;
            int pCnt;
            Product oProd = new Product();
            DataTable dsSubFam = new DataTable();

            SubFid = oPF.GetSubFamilyCount(FamilyID,_CatalogId);
            //if (SubFid > 0)
            //{
            dsSubFam = oPF.GetSubFamilyList(FamilyID);
            if (dsSubFam != null)
            {
                foreach (DataRow rSubFam in dsSubFam.Rows)
                {
                    System.Web.UI.WebControls.TreeNode SubFamilyNode = new TreeNode();
                    //TreeNode SubFamilyNode = new TreeNode();
                    //pCnt = oProd.GetProductCount(FamilyID);
                    pCnt = oProd.GetProductCount(oHelper.CI(rSubFam["FAMILY_ID"].ToString()),_CatalogId);
                    if (pCnt > 0 && _DisplayProductCount.ToUpper() == "YES")
                    {
                        SubFamilyNode.Text = rSubFam["FAMILY_NAME"].ToString() + "(" + pCnt + ")";
                    }
                    else
                    {
                        SubFamilyNode.Text = rSubFam["FAMILY_NAME"].ToString();
                    }
                    SubFamilyNode.Value = rSubFam["FAMILY_ID"].ToString();
                    SubFamilyNode.NavigateUrl = "~/Familydisplay.aspx?Fid=" + SubFamilyNode.Value;
                    // if (Helper.WebCatGlb["DISPLAY SUBFAMILY LOGO"].ToString().ToUpper() == "YES")
                    //{
                    //  SubFamilyNode.ImageUrl = imgPath + SubFamLogo;
                    //}
                    if (_DisplayFamilyLogo.ToUpper() == "YES")
                    {
                        SubFamilyNode.ImageUrl = _ImagePath + _FamilyLogo;
                    }

                    ParentFamilyNode.ChildNodes.Add(SubFamilyNode);
                }
            }
            //}

        }
        protected void TVCategory_TreeNodeExpanded(object sender, TreeNodeEventArgs e)
        {
            //if (!Page.IsPostBack)
            //{
                //if (Helper.WebCatGlb["CATEGORY DISPLAY MODE"].ToString() == "FLAT VIEW")
                //{
                //  LoadSubCategories(e.Node.Value, e.Node);
                //}
            //}
        }

        #endregion


    }
}

