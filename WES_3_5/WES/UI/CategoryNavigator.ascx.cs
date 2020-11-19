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
using TradingBell.Common;
using TradingBell.WebServices;
using TradingBell.WebCat.CatalogDB;
using TradingBell.WebCat.Helpers;
using TradingBell.WebCat.TemplateRender;
using TradingBell.WebCat.CommonServices;
public partial class UI_CategoryNavigator : System.Web.UI.UserControl
{
    HelperDB oHelper;
    ErrorHandler oErr;
    Category oCat;
    ProductFamily oPF;
    ProductRender oPR;
    Order oOrder;
    string imgPath;
    string CatLogo;
    string FamLogo;
    int CatalogID;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!DesignMode)
        {
            oHelper = new HelperDB();
            oErr = new ErrorHandler();
            oCat = new Category();
            oPF = new ProductFamily();
            oOrder = new Order();
            oPR = new ProductRender();

            CatalogID = oHelper.CI(oHelper.GetOptionValues("DEFAULT CATALOG").ToString());
            imgPath = oHelper.GetOptionValues("IMAGE PATH").ToString();
            CatLogo = oHelper.GetOptionValues("CATEGORY LOGO").ToString();
            FamLogo = oHelper.GetOptionValues("FAMILY LOGO").ToString();
            //SubFamLogo = oHelper.GetOptionValues("SUBFAMILY LOGO"].ToString();
            int OpenOrdStatusID = (int)Order.OrderStatus.OPEN;

            if (!Page.IsPostBack)
            {
                LoadCategoryTree(CatalogID);
            }
            /* To display the No of items in cart*/
            if (Session["USER_ID"] != null && Session["USER_ID"].ToString() != "")
            {
                int OrderID = 0;

                if ((Session["ORDER_ID"] != null && Convert.ToInt32(Session["ORDER_ID"]) > 0) || (Request.QueryString["ViewOrder"] != null && Request.QueryString["ViewOrder"].Equals("View")))
                {
                    OrderID = Convert.ToInt32(Session["ORDER_ID"]);
                }
                else
                {
                    OrderID = oOrder.GetOrderID(oHelper.CI(Session["USER_ID"]), OpenOrdStatusID);
                }

                string OrderStatus = oOrder.GetOrderStatus(OrderID);
                if (OrderID > 0 && (OrderStatus == Order.OrderStatus.OPEN.ToString() || OrderStatus == "CAU_PENDING"))
                {
                    lblItemCount.Text = oOrder.GetOrderItemCount(OrderID) + " " + GetGlobalResourceObject("CategoryNavigator", "Items");
                    lblCostValue.Text = oHelper.GetOptionValues("CURRENCYFORMAT") + oOrder.GetCurrentProductTotalCost(OrderID);
                }
                else
                {
                    lblItemCount.Text = "No Items";
                    lblCostValue.Text = oHelper.GetOptionValues("CURRENCYFORMAT") + " 0.00";
                }
            }
            else
            {
                lblItemCount.Text = "No Items";
                lblCostValue.Text = oHelper.GetOptionValues("CURRENCYFORMAT") + " 0.00";
            }
        }

    }
    #region "Functions"
    public void LoadCategoryTree(int CatID)
    {
        DataSet dsCat = new DataSet();
        int FCnt;
        //bool ChekInvetory = false;
        try
        {
            dsCat = oCat.GetRootCategories(CatID);
            if (TVCategory.Nodes.Count > 0)
            {
                TVCategory.Nodes.Clear();
            }
            if (dsCat != null)
            {
                foreach (DataRow rCat in dsCat.Tables[0].Rows)
                {
                    //ChekInvetory = oCat.CheckInventory(rCat["CATEGORY_ID"].ToString());
                    //if (ChekInvetory)
                    //{
                    TreeNode parentNode = new TreeNode();

                    //FCnt = oPF.GetFamilyCount(rCat["CATEGORY_ID"].ToString());
                    FCnt = oPF.GetFamilyCountWithInventory(rCat["CATEGORY_ID"].ToString(), CatalogID);
                    if (FCnt > 0 && oHelper.GetOptionValues("DISPLAY FAMILY COUNT").ToString() == "YES")
                    {
                        parentNode.Text = rCat["CATEGORY_NAME"].ToString() + " (" + FCnt + ")";

                    }
                    else
                    {
                        parentNode.Text = rCat["CATEGORY_NAME"].ToString();
                    }
                    //parentNode.Text = rCat["CATEGORY_NAME"].ToString();
                    parentNode.Value = rCat["CATEGORY_ID"].ToString();

                    int ProdCnt = oPR.GetProductCountByCategory(parentNode.Value, CatalogID);
                    if (ProdCnt > 0)
                    {
                        DataSet oDS = new DataSet();
                        oDS = oCat.GetSubCategories(rCat["CATEGORY_ID"].ToString(), CatID);
                        if ((oDS != null))
                        {
                            parentNode.NavigateUrl = "~/Default.aspx";// "~/CategoryDisplay.aspx?CatID=" + parentNode.Value;
                        }
                        else
                        {
                            parentNode.NavigateUrl = "~/Family.aspx?Cat=" + parentNode.Value;
                        }
                        if (oHelper.GetOptionValues("DISPLAY CATEGORY LOGO").ToString().ToUpper() == "YES")
                        {
                            parentNode.ImageUrl = imgPath + CatLogo;

                        }
                        //if ((parentNode.Text.ToUpper() != "DEFAULT CATEGORY"))
                        //    if(parentNode.Text != "General Category")                       

                        TVCategory.Nodes.Add(parentNode);
                    }

                    if (oHelper.GetOptionValues("CATEGORY DISPLAY MODE").ToString().ToUpper() == "CATALOG HIERARCHY")
                    {
                        LoadFamily(parentNode.Value, parentNode);
                    }
                    LoadSubCategories(parentNode.Value, parentNode);
                }
                //}//Check Inventory.
            }
        }
        catch (Exception e)
        {
            oErr.ErrorMsg = e;
        }
    }
    public void LoadSubCategories(string CateID, TreeNode ParentNode)
    {
        DataSet dsCat = new DataSet();
        int FCnt;
        try
        {
            dsCat = oCat.GetSubCategories(CateID, CatalogID);
            if (dsCat != null)
            {
                foreach (DataRow rSCat in dsCat.Tables[0].Rows)
                {
                    //bool ChkInventory;
                    TreeNode subCatNode = new TreeNode();

                    //ChkInventory = oCat.CheckInventory(rSCat["CATEGORY_ID"].ToString());
                    //if (ChkInventory == true)
                    //{

                    //FCnt = oPF.GetFamilyCount(rSCat["CATEGORY_ID"].ToString());
                    FCnt = oPF.GetFamilyCountWithInventory(rSCat["CATEGORY_ID"].ToString(), CatalogID);
                    if (FCnt > 0 && oHelper.GetOptionValues("DISPLAY FAMILY COUNT").ToString() == "YES")
                    {
                        subCatNode.Text = " " + rSCat["CATEGORY_NAME"].ToString() + " (" + FCnt + ")";
                    }
                    else
                    {
                        subCatNode.Text = " " + rSCat["CATEGORY_NAME"].ToString();
                    }
                    subCatNode.Value = rSCat["CATEGORY_ID"].ToString();
                    int ProdCnt = oPR.GetProductCountByCategory(subCatNode.Value, CatalogID);
                    if (ProdCnt > 0)
                    {
                        DataSet oDS = new DataSet();
                        oDS = oCat.GetSubCategories(rSCat["CATEGORY_ID"].ToString(), CatalogID);
                        if ((oDS != null))
                        {
                            subCatNode.NavigateUrl = "~/CategoryDisplay.aspx?CatID=" + subCatNode.Value;
                        }
                        else
                        {
                            subCatNode.NavigateUrl = "~/Family.aspx?Cat=" + subCatNode.Value;
                        }
                        if (oHelper.GetOptionValues("DISPLAY CATEGORY LOGO").ToString() == "YES")
                        {
                            subCatNode.ImageUrl = imgPath + CatLogo;
                        }

                        ParentNode.ChildNodes.Add(subCatNode);
                    }

                    if (oHelper.GetOptionValues("CATEGORY DISPLAY MODE").ToString().ToUpper() == "CATALOG HIERARCHY")
                    {
                        LoadFamily(subCatNode.Value, subCatNode);
                    }
                    LoadSubCategories(subCatNode.Value, subCatNode);
                    //}
                }
            }
            //else
            //{
            //    if (oHelper.GetOptionValues("CATEGORY DISPLAY MODE"].ToString().ToUpper() == "CATALOG HIERARCHY")
            //    {
            //        LoadFamily(ParentNode.Value, ParentNode);
            //    }

            //}
        }

        catch (Exception e)
        {
            oErr.ErrorMsg = e;
        }

    }
    public void LoadFamily(string CatID, TreeNode SubCatNode)
    {
        DataTable dsFam = new DataTable();
        Product oProd = new Product();
        int FCnt;
        int PCnt;
        try
        {
            FCnt = oPF.GetFamilyCount(CatID, CatalogID);

            if (FCnt > 0)
            {
                //dsFam = oPF.GetCategoryFamilyList(CatID);
                dsFam = oPF.GetRootFamilyList(CatID, CatalogID);
                if (dsFam != null)
                {
                    foreach (DataRow rFam in dsFam.Rows)
                    {
                        TreeNode FamilyNode = new TreeNode();
                        PCnt = oProd.GetProductCount(oHelper.CI(rFam["FAMILY_ID"].ToString()), CatalogID);
                        if (PCnt > 0 && oHelper.GetOptionValues("DISPLAY PRODUCT COUNT").ToString() == "YES")
                        {
                            FamilyNode.Text = rFam["FAMILY_NAME"].ToString() + " (" + PCnt + ")";
                        }
                        else
                        {
                            FamilyNode.Text = rFam["FAMILY_NAME"].ToString();
                        }
                        FamilyNode.Value = rFam["FAMILY_ID"].ToString();
                        if (oHelper.GetOptionValues("DISPLAY FAMILY LOGO").ToString() == "YES")
                        {
                            FamilyNode.ImageUrl = imgPath + FamLogo;
                        }
                        FamilyNode.NavigateUrl = "~/Familydisplay.aspx?Fid=" + FamilyNode.Value;
                        SubCatNode.ChildNodes.Add(FamilyNode);
                        if (oHelper.GetOptionValues("CATEGORY DISPLAY MODE").ToString().ToUpper() == "CATALOG HIERARCHY")
                        {
                            LoadSubFamily(oHelper.CI(FamilyNode.Value), FamilyNode);
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            oErr.ErrorMsg = ex; oErr.CreateLog();

        }
    }
    public void LoadSubFamily(int FamilyID, TreeNode ParentFamilyNode)
    {
        int SubFid;
        int pCnt;
        Product oProd = new Product();
        DataTable dsSubFam = new DataTable();
        try
        {
            SubFid = oPF.GetSubFamilyCount(FamilyID, CatalogID);
            //if (SubFid > 0)
            //{
            dsSubFam = oPF.GetSubFamilyList(FamilyID);
            if (dsSubFam != null)
            {
                foreach (DataRow rSubFam in dsSubFam.Rows)
                {
                    TreeNode SubFamilyNode = new TreeNode();
                    //pCnt = oProd.GetProductCount(FamilyID);
                    pCnt = oProd.GetProductCount(oHelper.CI(rSubFam["FAMILY_ID"].ToString()), CatalogID);
                    if (pCnt > 0 && oHelper.GetOptionValues("DISPLAY PRODUCT COUNT").ToString().ToUpper() == "YES")
                    {
                        SubFamilyNode.Text = rSubFam["FAMILY_NAME"].ToString() + "(" + pCnt + ")";
                    }
                    else
                    {
                        SubFamilyNode.Text = rSubFam["FAMILY_NAME"].ToString();
                    }
                    SubFamilyNode.Value = rSubFam["FAMILY_ID"].ToString();
                    SubFamilyNode.NavigateUrl = "~/Familydisplay.aspx?Fid=" + SubFamilyNode.Value;
                    // if (oHelper.GetOptionValues("DISPLAY SUBFAMILY LOGO"].ToString().ToUpper() == "YES")
                    //{
                    //  SubFamilyNode.ImageUrl = imgPath + SubFamLogo;
                    //}
                    if (oHelper.GetOptionValues("DISPLAY FAMILY LOGO").ToString() == "YES")
                    {
                        SubFamilyNode.ImageUrl = imgPath + FamLogo;
                    }

                    ParentFamilyNode.ChildNodes.Add(SubFamilyNode);
                }
            }
            //}
        }
        catch (Exception ex)
        {
            oErr.ErrorMsg = ex; oErr.CreateLog();
        }
    }

    #endregion
    // protected void TVCategory_TreeNodeExpanded(object sender, TreeNodeEventArgs e)
    //{
    //  if (!Page.IsPostBack)
    //   {
    //if (oHelper.GetOptionValues("CATEGORY DISPLAY MODE"].ToString() == "FLAT VIEW")
    //{
    //LoadSubCategories(e.Node.Value, e.Node);
    //}
    //  }
    // }

}
