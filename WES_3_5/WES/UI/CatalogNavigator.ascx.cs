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
using TradingBell5.CatalogX;
using System.IO;
using TradingBell.WebCat.CatalogDB;
using TradingBell.WebCat.Helpers;
using TradingBell.WebCat.TemplateRender;
using TradingBell.WebCat.CommonServices;
public partial class UI_CatalogNavigator : System.Web.UI.UserControl
{
    #region Declarations
    ConnectionDB oCon = new ConnectionDB();
    HelperDB oHelper = new HelperDB();
    ProductFamily oFam = new ProductFamily();
    TemplateLayout oTmpLayout = new TemplateLayout();
    DataSet oRootCatDS = new DataSet();
    string CategoryTmpContent = "";
    int _CategoryImageHeight = 30;
    int _CategoryImageWidth = 30;
    int CatalogID;
    #endregion
    #region Properties
    public int CategoryImageHeight
    {
        get
        {
            return _CategoryImageHeight;
        }
        set
        {
            _CategoryImageHeight = value;
        }
    }

    public int CategoryImageWidth
    {
        get
        {
            return _CategoryImageWidth;
        }
        set
        {
            _CategoryImageWidth = value;
        }
    }
    #endregion
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (Session["CATALOGID"] != null && Session["CATALOGID"].ToString() != "")
            {
                CatalogID = oHelper.CI(Session["CATALOGID"]);
            }
            else
            {
                Session["CATALOGID"] = oHelper.CI(oHelper.GetOptionValues("DEFAULT CATALOG").ToString());
                CatalogID = oHelper.CI(Session["CATALOGID"]);

            }
            oRootCatDS.Tables.Add(FetchCXDS());
            int TemplateType = (int)TemplateLayout.TemplateType.ParentCategoryLayout;
            #region FetchTemplateSource
            //DataSet oDSTemplate = oTmpLayout.GetTemplateSource(Type);
            //foreach (DataRow oDR in oDSTemplate.Tables["TBWC_TEMPLATES"].Select("IS_DEFAULT_TEMPLATE=" + true))
            //{
            //    CategoryTmpContent = oDR["TEMPLATE_SOURCE"].ToString();
            //}
            #endregion
            CategoryTmpContent = oTmpLayout.GetTemplateHtmlSource(TemplateType);
        }
        catch (Exception ex)
        {

        }
    }

    #region Commented Methods

    //protected string GetTableContent()
    //{
    //    string TableTag = "";
    //    try
    //    {
    //        TableTag = CategoryTmpContent.Substring(CategoryTmpContent.IndexOf("<TABLE"));
    //        TableTag = TableTag.Substring(TableTag.IndexOf("<TABLE"), TableTag.IndexOf(">") + 1 - TableTag.IndexOf("<TABLE"));
    //    }
    //    catch(Exception ex)
    //    {
    //    }
    //    return TableTag;
    //}
    //protected string[] GetRowCollection()
    //{
    //    string[] SeparateLine = new string[] { "" };
    //    try
    //    {
    //        string BodyContent = "";
    //        BodyContent = CategoryTmpContent.Remove(CategoryTmpContent.IndexOf("<TABLE"), GetTableContent().Length);
    //        int start = BodyContent.IndexOf("<TR");
    //        int end = BodyContent.LastIndexOf("</TR>");
    //        string temp = BodyContent.Substring(start, ((end) - start));
    //        string[] StrSeparator = new string[] { "</TR>" };
    //        SeparateLine = temp.Split(StrSeparator, StringSplitOptions.None);
    //    }
    //    catch (Exception ex)
    //    {
    //    }
    //    return SeparateLine;
    //}
    //protected string GetSeperateRow(string RowName)
    //{
    //    string RowContent = "";
    //    try
    //    {
    //        string CategoryContent = "";
    //        string HeaderContent = "";
    //        string[] SeparateLine = new string[] { "" };
    //        SeparateLine = GetRowCollection();
    //        for (int i = 0; i <= SeparateLine.Length - 1; i++)
    //        {
    //            bool strFlag = SeparateLine[i].Contains("{CATEGORY NAME}");
    //            if (strFlag)
    //            {
    //                CategoryContent = SeparateLine[i].ToString();
    //            }
    //            else
    //            {
    //                HeaderContent = SeparateLine[i].ToString();
    //            }
    //        }
    //        if (RowName == "CategoryName")
    //        {
    //            RowContent = CategoryContent;
    //        }
    //        else if (RowName == "Header")
    //        {
    //            RowContent = HeaderContent;
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //    }
    //    return RowContent;
    //}
    
    //public string BuildCategoryListHTML()
    //{
    //    string CategoryLists = "";
    //    try
    //    {
    //        string oCatCode = "";
    //        string TempLinkContent = "";
    //        string LinkContent = BuildLinkContent();
    //        string CategoryContent = GetSeperateRow("CategoryName");
    //        string HeaderContent = GetSeperateRow("Header");
    //        if (HeaderContent == "" && CategoryContent == "")
    //        {
    //            oCatCode = "<TABLE WIDTH=\"100%\" HEIGHT=\"100%\"><TR><TD WIDTH=\"100%\" HEIGHT=\"100%\" ALIGN=\"CENTER\" VALIGN=\"MIDDLE\">";
    //            oCatCode = oCatCode + "<Font Color=\"Red\" SIZE=\"2\"><BR/><BR/><B>CATEGORY NAVIGATOR CAN'T DISPLAYED <BR/>INVALID LAYOUT SPECIFICATION</B></Font></TD></TR>";
    //        }
    //        else
    //        {
    //            foreach (DataRow oDR in oRootCatDS.Tables[0].Select("", "CATEGORY_NAME"))
    //            {
    //                //TempLinkContent = LinkContent + oDR[0] + " \">" + oDR[1].ToString().Trim() + "</A>";
    //                //oCatCode = oCatCode + CategoryContent.Replace("{Category Name}", TempLinkContent) + "</TR>" + System.Environment.NewLine;
    //                //oCatCode = oCatCode + LinkContent + oDR[0].ToString() + "\">" + CategoryContent.Replace("{CATEGORY NAME}", oDR[1].ToString().Trim()) + "</TR>" + "</A>" + System.Environment.NewLine;
    //                TempLinkContent = LinkContent + oDR["CATEGORY_ID"].ToString() + "\">";
    //                string TmpContent = CategoryContent.Insert(CategoryContent.IndexOf(">", CategoryContent.IndexOf("<TD")) + 1, TempLinkContent);
    //                TmpContent = TmpContent.Replace("{CATEGORY NAME}", oDR["CATEGORY_NAME"].ToString().Trim());
    //                oCatCode = oCatCode + TmpContent.Insert(TmpContent.IndexOf("</TD>"), "</A>");
    //            }
    //        }
    //        oCatCode = HeaderContent + oCatCode;
    //        oCatCode = oCatCode.Replace("{CATEGORY NAME}", "");
    //        oCatCode = oCatCode.Replace("{CATEGORY IMAGE}", "");
    //        CategoryLists = GetTableContent() + oCatCode + "</TABLE>";
    //    }
    //    catch (Exception ex)
    //    {
    //    }
    //    return CategoryLists;
    //}

    #endregion

    //Build the Link or Redirect to FamilyListPage
    protected string BuildLinkContent()
    {
        string LinkContent = "";
        try
        {
            if (oRootCatDS != null)
            {
                LinkContent = "<A HREF=";
            }
            else
            {
                //Response.Redirect("Family.aspx?Cat=" + Request.QueryString["CatID"].ToString());
                Response.Redirect(UrlRewriteServices.ConstructUrl("f" + Request.QueryString["CatID"].ToString()));

            }
            //if (oRootCatDS != null)
            //{
            //    LinkContent = "<A HREF=\"CategoryDisplay.aspx?CatID=";
            //}
            //else
            //{
            //    Response.Redirect("Family.aspx?Cat=" + Request.QueryString["CatID"].ToString());
            //}
        }
        catch (Exception ex)
        {
        }
        return LinkContent;
    }

    //Fetch the Child Catgory List Dataset
    private DataTable FetchCXDS()
    {
        DataTable oDT = new DataTable();
        try
        {
            TradingBell5.CatalogX.CSDBProvider.Connection oCXCon = new TradingBell5.CatalogX.CSDBProvider.Connection();
            TradingBell5.CatalogX.CSDBProvider.CSDSTableAdapters.Web_TB_CATEGORYTableAdapter oCatTA = new TradingBell5.CatalogX.CSDBProvider.CSDSTableAdapters.Web_TB_CATEGORYTableAdapter();
            TradingBell5.CatalogX.CSDBProvider.CSDS.WebCat_CategoryDataTable oCatDT = new TradingBell5.CatalogX.CSDBProvider.CSDS.WebCat_CategoryDataTable();
            string ConString = oCon.ConnectionString.Substring(oCon.ConnectionString.IndexOf(";") + 1);
            oCXCon.ConnSettings(ConString);
            oCatTA.Fill(oCatDT, CatalogID);
            oCatTA.Dispose();
            if (oCatDT != null)
            {
                oDT = oCatDT.Clone();
                DataRow[] oDR = oCatDT.Select("PARENT_CATEGORY='0'", "CATEGORY_NAME");
                foreach (DataRow oDrs in oDR)
                {
                    oDT.ImportRow(oDrs);
                }
            }
            oCatDT.Dispose();
        }
        catch (Exception ex)
        {
        }
        return oDT;
    }

    //Build the Category List Html Source and Clear the Unfilled Macros
    public string BuildCategoryListHTML()
    {
        string TableContent = "";
        try
        {
            string oCatCode = "";
            string TempLinkContent = "";
            string TemplImgstr = "";
            string TempImgcont = "";
            string TmpContent = "";
            string ProdImgpath = "";
            string ImgFilepath = ""; string Imgsrc = "";
            ProdImgpath = oHelper.CS(oHelper.GetOptionValues("PRODUCT IMAGE PATH").ToString());
            string LinkContent = BuildLinkContent();
            string CategoryContent = CategoryTmpContent.Substring(CategoryTmpContent.IndexOf("{START_CATEGORY_REGION}") + 23, CategoryTmpContent.IndexOf("{END_CATEGORY_REGION}") - (CategoryTmpContent.IndexOf("{START_CATEGORY_REGION}") + 23));
            TableContent = CategoryTmpContent.Remove(CategoryTmpContent.IndexOf("{START_CATEGORY_REGION}") + 23, CategoryTmpContent.IndexOf("{END_CATEGORY_REGION}") - (CategoryTmpContent.IndexOf("{START_CATEGORY_REGION}") + 23));
            foreach (DataRow oDR in oRootCatDS.Tables[0].Select("", "CATEGORY_NAME"))
            {
                int Imgfileloc = CategoryContent.IndexOf("{IMAGE_FILE}");
                if (Imgfileloc > -1)
                {
                    if (oDR["IMAGE_FILE"].ToString() != "" && oDR["IMAGE_FILE"] != null)
                    {
                        ImgFilepath = ProdImgpath + oDR["IMAGE_FILE"].ToString();
                        Imgsrc=BuildImageContent(ImgFilepath);
                        //TemplImgstr = "<img ID=\"CatImg\" runat=\"server\" height=" +_CategoryImageHeight + " width=" + _CategoryImageWidth + " src=" + oDR["IMAGE_FILE"] + " style=\"Border-width:0\"/>";//</img>";
                        TemplImgstr = Imgsrc;
                    }
                    else
                    {
                        //TemplImgstr = Imgsrc;
                        TemplImgstr = "<img ID=\"CatImg\" runat=\"server\" height=" + _CategoryImageHeight + " width=" + _CategoryImageWidth + " src=\"images\\NoImage.gif\" style=\"Border-width:0\"/>";//</img>";
                    }
                    TempImgcont = CategoryContent.Replace("{IMAGE_FILE}", TemplImgstr.ToString().Trim());
                }
                int loc = CategoryContent.LastIndexOf("<TD", CategoryContent.LastIndexOf("{CATEGORY_NAME}"));
                if (loc > -1 && Imgfileloc !=-1)
                {
                    TempLinkContent = LinkContent + oDR["CATEGORY_ID"].ToString() + "-" + oDR["CATEGORY_NAME"].ToString() + ".aspx\">";
                    TmpContent = TempImgcont.Replace("{CATEGORY_NAME}", TempLinkContent + oDR["CATEGORY_NAME"].ToString().Trim());
                }
                else if (loc > -1 && Imgfileloc ==-1)
                {
                    string _sDescription = oDR["CATEGORY_NAME"].ToString().Replace(",", "");
                    _sDescription = _sDescription.Replace(" & ", "-").Replace(" ", "-").Replace("&", "-").Replace("/", "-");
                    TempLinkContent = LinkContent + UrlRewriteServices.ConstructUrl("c-" + oDR["CATEGORY_ID"].ToString() + "-" + _sDescription) + ">";
                    TmpContent = CategoryContent.Replace("{CATEGORY_NAME}", (TempLinkContent + oDR["CATEGORY_NAME"].ToString().Trim()));
                }
                else
                {
                    string _sDescription = oDR["CATEGORY_NAME"].ToString().Replace(",", "");
                    _sDescription = _sDescription.Replace(" & ", "-").Replace(" ", "-").Replace("&", "-").Replace("/", "-");
                    TempLinkContent = LinkContent + UrlRewriteServices.ConstructUrl("c-" + oDR["CATEGORY_ID"].ToString() + "-" + _sDescription) + ">";
//                    TempLinkContent = LinkContent + TradingBell.Common.UrlRewrite.GetRewrittenUrl("categories", oDR["CATEGORY_ID"].ToString() + "-" + _sDescription) + ">";
                    TmpContent = CategoryContent.Replace("{CATEGORY_NAME}", (TempLinkContent + oDR["CATEGORY_NAME"].ToString().Trim()));
                }
                TmpContent.Trim();
                TmpContent = TmpContent + "</A>";
                oCatCode = oCatCode + TmpContent;
            }
            TableContent = TableContent.Insert(CategoryTmpContent.IndexOf("{START_CATEGORY_REGION}") + 23, oCatCode);
            TableContent = TableContent.Replace("{START_CATEGORY_REGION}", "");
            TableContent = TableContent.Replace("{END_CATEGORY_REGION}", "");
            TableContent = TableContent.Replace("{CATEGORY_NAME}", "");
            TableContent = TableContent.Replace("{IMAGE_FILE}", "");
        }
        catch (Exception ex)
        {

        }
        return TableContent;
    }
    private string BuildImageContent(string ImageUrl)
    {
        //System.Drawing.Size newVal;
        string ImageContent = "";
        try
        {
            ImageUrl = ImageUrl.Replace("\\", "/");
            if ((File.Exists(Server.MapPath(ImageUrl)) == true) && ImageUrl != null)
            {
                //System.Drawing.Image oImg = System.Drawing.Image.FromFile(Server.MapPath(ImageUrl));
                //newVal = ScaleImage(oImg.Height, oImg.Width, 120, 120);
                ImageUrl = "<IMG SRC=\"" + ImageUrl + "\" WIDTH=\"" + _CategoryImageWidth + "\" HEIGHT=\"" + _CategoryImageHeight + "\" style=\"border-width:0px;background-color:#FFFFFF\"/>";
            }
            else if (ImageUrl != null)
            {
                ImageUrl = "<IMG SRC=\"" + ImageUrl + "\" Width =" + _CategoryImageWidth + " Height=" +_CategoryImageHeight + " style=\"border-width:0px;background-color:#FFFFFF\"/>";
            }
            ImageContent = ImageUrl;
        }
        catch (Exception ex)
        {

            ImageUrl = "<IMG SRC=\"" + ImageUrl + "\" Width =" + _CategoryImageWidth + " Height=" + _CategoryImageHeight + " style=\"border-width:0px;background-color:#FFFFFF\"/>";
            ImageContent = ImageUrl;
            return ImageContent;
        }
        return ImageContent;
    }
    //public System.Drawing.Size ScaleImage(double origHeight, double origWidth, double Width, double Height)
    //{
    //    System.Drawing.Size newSize = new System.Drawing.Size();
    //    double nWidth = Width;
    //    double nHeight = Height;
    //    double oWidth = origWidth;
    //    double oHeight = origHeight;
    //    if (origHeight > 200 || origWidth > 200)
    //    {
    //        if (oWidth > oHeight)
    //        {
    //            double Ratio = (double)((double)(oHeight) / (double)(oWidth));
    //            double Final = (nWidth) * Ratio;
    //            nHeight = (int)Final;
    //        }
    //        else
    //        {
    //            double Ratio = (double)((double)(oWidth) / (double)(oHeight));
    //            double Final = (nHeight) * Ratio;
    //            nWidth = (int)Final;
    //        }
    //    }
    //    newSize.Height = (int)nHeight;
    //    newSize.Width = (int)nWidth;
    //    return newSize;
    //}
}
