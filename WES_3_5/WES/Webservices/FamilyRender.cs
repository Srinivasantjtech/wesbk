using System;
using System.Web;
using System.Data;
using System.Collections;
using System.Web.Services;
using System.Web.Services.Protocols;
using TradingBell.Common;
using TradingBell5.CatalogX;
using System.IO;
using TradingBell.WebCat.CatalogDB;
using TradingBell.WebCat.Helpers;
namespace TradingBell.WebServices
{
    /// <summary>
    /// This is used to Get and Return all the FamilyTemplateDriven Methods 
    /// </summary>
    /// <remarks>
    /// Used to get Product and Family Attributes,Family and Product Details etc..
    /// </remarks>
    /// <example>
    /// FamilyRender oFamilyRender=new FamilyRender();
    /// </example>
    
    [WebService(Namespace = "http://WebCat.com/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class FamilyRender : System.Web.Services.WebService
    {
        #region Declarations
        Helper oHelper = new Helper();
        ErrorHandler oErr = new ErrorHandler();
        CSRender oCSRender = new CSRender();
        ProductRender oProdRender = new ProductRender();
        ProductFamily oTBFamily = new ProductFamily();
        TemplateLayout oTemplate = new TemplateLayout();
        DataSet CXAttributes = new DataSet();
        int CatID;
        string FamilyTabContent = "";
        string FamilyTblContent = "";
        string WOHeaderContent = "";
        string HeaderContent = "";        
        string HtmlTop = "";
        string HtmlEnd = "";
        string ProdRegHtmlS = "";
        string ProdRegHtmlE = "";
        int columnDisplay;
        int Pagesize;
        int UserID;
        int FamilyStart;
        int FamilyEnd;
        int ProdHStart;
        int ProdHEnd=-1;
        int ProdRStart;        
        int ProdREnd=-1;
        int ProdRegStart;
        int ProdRegEnd;
        int FamilyFirst;
        Boolean _NoFamilyImage;
        Boolean _NoProductImage;
        double _FamilyImageHeight;
        double _FamilyImageWidth;
        double _ProductImageHeight;
        double _ProductImageWidth;
        private int _CatalogID;
        string _NoImageAvailableCaption = "No Image to Display";

        public Boolean NoFamilyImage
        {
            get
            {
                return _NoFamilyImage;
            }
            set
            {
                _NoFamilyImage = value;
            }
            
        }

        public Boolean NoProductImage
        {
            get
            {
                return _NoProductImage;
            }
            set
            {
                _NoProductImage = value;
            }

        }


        public double FamilyImageHeight
        {
            get
            {
                return _FamilyImageHeight;
            }
            set
            {
                _FamilyImageHeight = value;
            }
        }


        public double FamilyImageWidth
        {
            get
            {
                return _FamilyImageWidth;
            }
            set
            {
                _FamilyImageWidth = value;
            }
        }


        public double ProductImageHeight
        {
            get
            {
                return _ProductImageHeight;
            }
            set
            {
                _ProductImageHeight = value;
            }
        }

        public double ProductImageWidth
        {
            get
            {
                return _ProductImageWidth;
            }
            set
            {
                _ProductImageWidth = value;
            }
        }
        public int CatalogID
        {
            get
            {
                return _CatalogID;
            }
            set
            {
                _CatalogID = value;

            }
        }
        public string NoImageAvailableCaption
        {
            get
            {
                return _NoImageAvailableCaption;
            }
            set
            {
                _NoImageAvailableCaption = value;
            }
        }
      #endregion
        /// <summary>
        /// Default Constructor
        /// </summary>
        public FamilyRender()
        {
            
        }

     

        #region TemplateBuilding Functions
        //This is used to Divide the Html Content to design Family Display and Product Display
        /// <summary>
        /// This is used to Divide the Html Content to design Family Display and Product Display
        /// </summary>
        /// <param name="HtmlContent">string</param>
        /// <example>
        /// <code>
        /// using System;
        /// using System.IO;
        /// using System.Web;
        /// using System.Data;
        /// using TradingBell.Common;
        /// using TradingBell.WebServices;
        /// 
        /// protected void Page_Load(object sender, EventArgs e)
        /// {
        ///  string HtmlContent; 
        ///  ...
        ///  DivideHTMLContent(HtmlContent); 
        /// }
        /// </code>
        /// </example>
        [WebMethod]
        protected void DivideHTMLContent(string HtmlContent)
        {
            try
            {
                FamilyTabContent = "";
                if (HtmlContent.IndexOf("{START", 0) > -1)
                {
                    HtmlTop = HtmlContent.Substring(0, HtmlContent.IndexOf("{START", 0));
                }
                FamilyStart = HtmlContent.IndexOf("{START_FAMILY_REGION}", 0);
                FamilyEnd = HtmlContent.IndexOf("{END_FAMILY_REGION}", FamilyStart);
                ProdRegStart = HtmlContent.IndexOf("{START_PRODUCT_REGION}",0);
                if (ProdRegStart > -1)
                {
                    ProdRegEnd = HtmlContent.IndexOf("{END_PRODUCT_REGION}", 0);
                }
                ProdRStart = HtmlContent.IndexOf("{START_PRODUCT_ROW}", 0);
                if (ProdRStart > -1)
                {
                    ProdREnd = HtmlContent.IndexOf("{END_PRODUCT_ROW}", ProdRStart);
                }
                ProdHStart = HtmlContent.IndexOf("{START_PRODUCT_HEADER}", 0);
                if (ProdHStart > -1)
                {
                    ProdHEnd = HtmlContent.IndexOf("{END_PRODUCT_HEADER}", ProdHStart);
                    HeaderContent = HtmlContent.Substring((ProdHStart + 22), (ProdHEnd - (ProdHStart + 22)));
                    ProdRegHtmlS = HtmlContent.Substring(ProdRegStart + 22, ProdHStart - (ProdRegStart + 22));
                }
                else
                {
                    HeaderContent = "";
                    if (ProdRStart > -1)
                    {
                        ProdRegHtmlS = HtmlContent.Substring(ProdRegStart + 22, ProdRStart - (ProdRegStart + 22));
                    }
                }
               
                if (ProdRegHtmlS.ToUpper().IndexOf("<TABLE", 0) > -1)
                {
                    ProdRegHtmlS = ProdRegHtmlS.ToUpper().Replace("<TABLE", "<TABLE ID=\"tblTemplate\" cellspacing=\"0\" cellpadding=\"0\"");
                }
                if (ProdREnd > -1)
                {
                    ProdRegHtmlE = HtmlContent.Substring(ProdREnd + 17, (ProdRegEnd - (ProdREnd + 17)));
                    WOHeaderContent = HtmlContent.Substring(ProdRStart + 19, (ProdREnd - (ProdRStart + 19)));
                }
                if (FamilyStart < ProdRStart)
                {
                    FamilyFirst = 1;
                    HtmlEnd = HtmlContent.Substring(ProdRegEnd + 20);
                }
                else
                {
                    FamilyFirst=0;
                    HtmlEnd = HtmlContent.Substring(FamilyEnd + 20);
                }
                FamilyTblContent = HtmlContent.Substring(FamilyStart + 21, FamilyEnd - (FamilyStart + 21));                
            }
            catch(Exception Ex)
            {
                oErr.ErrorMsg = Ex;
                oErr.CreateLog();                
            }
        }

        //This is used to Build Image Content
        /// <summary>
        /// This is used to Build Image Content
        /// </summary>
        /// <param name="ImageUrl">string</param>
        /// <returns>string</returns>
        /// <example>
        /// <code>
        /// using System;
        /// using System.IO;
        /// using System.Web;
        /// using System.Data;
        /// using TradingBell.Common;
        /// using TradingBell.WebServices;
        /// 
        /// protected void Page_Load(object sender, EventArgs e)
        /// {
        ///   string Img;
        ///   ...
        ///   Img=BuildFamilyImageContent(ImgSource);
        /// }
        /// </code>
        /// </example>
        [WebMethod]
        private string BuildFamilyImageContent(string ImageUrl,string ZoomImageUrl,bool IsFamilyImgZoom)
        {
            System.Drawing.Size newVal;
            string ImageContent = "";
            try
            {
                ImageUrl = ImageUrl.Replace("\\", "/");
                if ((File.Exists(Server.MapPath(ImageUrl)) == true) && ImageUrl != null)
                {
                    System.Drawing.Image oImg = System.Drawing.Image.FromFile(Server.MapPath(ImageUrl));
                    newVal = ScaleImage(oImg.Height, oImg.Width, this.FamilyImageWidth, this.FamilyImageHeight);
                    if (IsFamilyImgZoom == true)
                    {
                        ImageUrl = "<a href=\"javascript:Zoom('" + ZoomImageUrl + "')\"><IMG SRC=\"" + ImageUrl + "\" WIDTH=\"" + newVal.Width + "\" HEIGHT=\"" + newVal.Height + "\" style=\"border-width:0px;\" ALT=\"" + GetImageNameFromURL(ImageUrl) + "\"/></a>";
                    }
                    else if(IsFamilyImgZoom==false)
                    {
                        ImageUrl = "<IMG SRC=\"" + ImageUrl + "\" WIDTH=\"" + newVal.Width + "\" HEIGHT=\"" + newVal.Height + "\" style=\"border-width:0px;\" ALT=\"" + GetImageNameFromURL(ImageUrl) + "\"/>";
                    }
                    
                }
                else if (ImageUrl != null)
                {
                    if (IsFamilyImgZoom == true)
                    {
                        ImageUrl = "<a href=\"javascript:Zoom('" + ZoomImageUrl + "')\"><IMG SRC=\"" + ImageUrl + "\" Width =\"" + this.FamilyImageWidth + "\" Height=\"" + this.FamilyImageHeight + "\" style=\"border-width:0px;\" ALT=\"" + GetImageNameFromURL(ImageUrl) + "\"/></a>";
                    }
                    else if (IsFamilyImgZoom == false)
                    {
                        ImageUrl = "<IMG SRC=\"" + ImageUrl + "\" Width =\"" + this.FamilyImageWidth + "\" Height=\"" + this.FamilyImageHeight + "\" style=\"border-width:0px;\" ALT=\"" + GetImageNameFromURL(ImageUrl) + "\"/>";
                    }
                }
                ImageContent = ImageUrl;
            }
            catch (Exception ex)
            {
                oErr.ErrorMsg = ex;
                if (this.NoFamilyImage == true)
                {
                    ImageUrl = "images/NoImage.gif";
                    ImageUrl = "<IMG SRC=\"" + ImageUrl + "\" Width =\"" + this.FamilyImageWidth + "\" Height=\"" + this.FamilyImageHeight + "\" style=\"border-width:0px;\" ALT=\"" + NoImageAvailableCaption + "\"/>";
                }
                else
                {
                    ImageUrl = "<IMG SRC=\"" + ImageUrl + "\"  Width =\"" + this.FamilyImageWidth + "\" Height=\"" + this.FamilyImageHeight + "\" style=\"border-width:0px;\" ALT=\"" + GetImageNameFromURL(ImageUrl) + "\"/>";
                }
                ImageContent = ImageUrl;
                return ImageContent;
            }
            return ImageContent;
        }

        public string ZoomImage(string ImageUrl)
        {
            string ZoomImage = "Images/zoom1.gif";
            ZoomImage = "<a href=\"javascript:Zoom('" + ImageUrl + "')\"><IMG SRC=\"" + ZoomImage + "\" style=\"text-decoration:none;border-width:0px\" ALT=\"" + GetImageNameFromURL(ImageUrl) + "\"/></a>";
            return ZoomImage;
        }



        //This is used to resize the Image
        /// <summary>
        /// This is used to resize the Image
        /// </summary>
        /// <param name="origHeight">double</param>
        /// <param name="origWidth">double</param>
        /// <param name="Width">double</param>
        /// <param name="Height">double</param>
        /// <returns>System.Drawing.Size</returns>
        /// <example>
        /// <code>
        /// using System;
        /// using System.IO;
        /// using System.Web;
        /// using System.Data;
        /// using TradingBell.Common;
        /// using TradingBell.WebServices;
        /// 
        /// protected void Page_Load(object sender, EventArgs e)
        /// {
        ///   ...
        ///   System.Drawing.Image oImg = System.Drawing.Image.FromFile(Server.MapPath(ImageUrl));
        ///   newVal = ScaleImage(oImg.Height, oImg.Width, 180, 180);
        /// }
        /// </code>
        /// </example>
        [WebMethod]
        protected System.Drawing.Size ScaleImage(double origHeight, double origWidth, double Width, double Height)
        {
            System.Drawing.Size newSize = new System.Drawing.Size();
            double nWidth = Width;
            double nHeight = Height;
            double oWidth = origWidth;
            double oHeight = origHeight;
            //if (origHeight > 200 || origWidth > 200)
            if (origHeight > nHeight || origWidth > nWidth)
            {
                if (oWidth > oHeight)
                {
                    double Ratio = (double)((double)(oHeight) / (double)(oWidth));
                    double Final = (nWidth) * Ratio;
                    nHeight = (int)Final;
                }
                else
                {
                    double Ratio = (double)((double)(oWidth) / (double)(oHeight));
                    double Final = (nHeight) * Ratio;
                    nWidth = (int)Final;
                }
            }
            newSize.Height = (int)nHeight;
            newSize.Width = (int)nWidth;
            return newSize;
        }


        //This is used to Build the Html for family Template
        /// <summary>
        /// This is used to Build the Html for family Template
        /// </summary>
        /// <param name="FamilyID">int</param>
        /// <returns>string</returns>
        /// <example>
        /// <code>
        /// using System;
        /// using System.IO;
        /// using System.Web;
        /// using System.Data;
        /// using TradingBell.Common;
        /// using TradingBell.WebServices;
        /// 
        /// protected void Page_Load(object sender, EventArgs e)
        /// {
        ///  string Temp; 
        ///  int FamilyID;
        ///  ...
        ///  Temp = BuildFamilyTemplate(FamilyID)
        /// }
        /// </code>
        /// </example>
        [WebMethod]
        protected string BuildFamilyTemplate(int CatID,int FamilyID,int UserID,string FamilyTblContent)
        {
            String FamilyHtmlText = "";

            string TempFamilyTabContent = FamilyTblContent;


            int FamImgStartLoc = TempFamilyTabContent.IndexOf("{START_FAMILY_IMAGE_ZOOM}");
            if (FamImgStartLoc != -1)
            {
                FamImgStartLoc = FamImgStartLoc + 25;
            }
            int FamImgEndLoc = TempFamilyTabContent.IndexOf("{END_FAMILY_IMAGE_ZOOM}");


            int StartLoc = TempFamilyTabContent.IndexOf("{START_ZOOM_FAMILY_ATTRIBUTE}");
            if (StartLoc != -1)
            {
                StartLoc = StartLoc + 29;
            }
            int EndLoc = TempFamilyTabContent.IndexOf("{END_ZOOM_FAMILY_ATTRIBUTE}");
            string ZoomImgAttr = "";
            if (StartLoc != -1 && EndLoc != -1)
            {
                ZoomImgAttr = TempFamilyTabContent.Substring(StartLoc, (EndLoc - StartLoc));
                if (ZoomImgAttr.IndexOf("{") > -1)
                {
                    ZoomImgAttr = ZoomImgAttr.Substring(ZoomImgAttr.IndexOf("{") + 1, (ZoomImgAttr.IndexOf("}") - 1) - (ZoomImgAttr.IndexOf("{")));
                }
                else
                {
                    ZoomImgAttr = "";
                }
            }

            string FamImgAttr = "";

            if (FamImgStartLoc != -1 && FamImgEndLoc != -1)
            {
                FamImgAttr = TempFamilyTabContent.Substring(FamImgStartLoc, (FamImgEndLoc - FamImgStartLoc));
                if (FamImgAttr.IndexOf("{") != -1)
                {
                    FamImgAttr = FamImgAttr.Substring(FamImgAttr.IndexOf("{") + 1, (FamImgAttr.IndexOf("}") - 1) - (FamImgAttr.IndexOf("{")));
                }
            }
            #region Commented for Old Method of ZOOM
            //////////CXAttributes = oCSRender.GetAttributes(CatID, UserID);
            //////////DataSet oDSTempProdFamily =new DataSet();
            //////////oDSTempProdFamily = oCSRender.GetCXFamilies(CatID,FamilyID,UserID);
            //////////DataRow[] DRTemp = CXAttributes.Tables[0].Select("ATTRIBUTE_TYPE=9");
            //////////foreach (DataColumn oDCs in oDSTempProdFamily.Tables[0].Columns)
            //////////{
            //////////    foreach (DataRow oDRTemp in DRTemp)
            //////////    {
            //////////        if ((oDCs.ColumnName != ZoomImgAttr) && oDRTemp[1].ToString().ToUpper() == oDCs.ColumnName.ToString().ToUpper())//&& (ZoomImgAttr.Trim() != string.Empty)
            //////////        {
            //////////            int FamImgStartLoc = TempFamilyTabContent.IndexOf("{" + oDCs.ColumnName + "}");
            //////////            if (FamImgStartLoc != -1)
            //////////            {
            //////////                FamImgAttr = oDCs.ColumnName.ToString();
            //////////            }
            //////////        }
            //////////    }
            //////////}
            #endregion

            DataSet oDSProdFamily = new DataSet();
            try
            {
                FamilyTabContent = FamilyTblContent;
                bool IsImageAttribute = false;
                CXAttributes = oCSRender.GetAttributes(CatID, UserID);
                oDSProdFamily = oCSRender.GetCXFamilies(CatID,FamilyID,UserID);
                if (oDSProdFamily != null)
                {
                foreach (DataRow Dr in oDSProdFamily.Tables[0].Rows)
                {
                    FamilyTabContent = FamilyTblContent;
                    foreach (DataColumn DC in oDSProdFamily.Tables[0].Columns)
                    {
                        DataRow[] DRAtr = CXAttributes.Tables[0].Select("ATTRIBUTE_TYPE=9");
                        foreach (DataRow DrAtr in DRAtr)
                        {
                            if (DC.ColumnName.ToLower() == DrAtr[1].ToString().ToLower())
                            {
                                if (FamilyTabContent.Contains("{" + DC.ColumnName.ToString() + "}") == true)
                                {
                                    IsImageAttribute = true;
                                    string ImgSource = Dr[DC.ColumnName].ToString();
                                    if (ImgSource.ToString() != null && ImgSource.ToString() != "")
                                    {
                                        ImgSource = oHelper.GetOptionValues("PRODUCT IMAGE PATH").ToString() + ImgSource;
                                        if (DC.ColumnName.ToString() != ZoomImgAttr)
                                        {
                                            if (FamImgStartLoc==-1 && FamImgEndLoc==-1)
                                            {
                                                string Img = BuildFamilyImageContent(ImgSource,"",false);
                                                FamilyTabContent = FamilyTabContent.Replace("{" + DC.ColumnName.ToString() + "}", Img);
                                            }
                                        }
                                        if (ZoomImgAttr != string.Empty && ZoomImgAttr != FamImgAttr && DC.ColumnName!=ZoomImgAttr)
                                        {
                                            BuildFamilyZoomImage(oDSProdFamily, DRAtr, ZoomImgAttr,FamImgAttr,ImgSource);
                                            StartLoc = FamilyTabContent.IndexOf("{START_ZOOM_FAMILY_ATTRIBUTE}");
                                            EndLoc = FamilyTabContent.IndexOf("{END_ZOOM_FAMILY_ATTRIBUTE}") + 27 ;
                                            if (StartLoc != -1 && EndLoc != -1)
                                            {
                                                FamilyTabContent = FamilyTabContent.Remove(StartLoc, EndLoc - StartLoc);
                                            }
                                            FamilyTabContent = FamilyTabContent.Replace("START_FAMILY_IMAGE_ZOOM", "");
                                            FamilyTabContent = FamilyTabContent.Replace("END_FAMILY_IMAGE_ZOOM", "");
                                        }
                                        else if (FamImgStartLoc!=-1 && ZoomImgAttr == string.Empty)
                                        {
                                            string ImageZoom = BuildFamilyImageContent(ImgSource, ImgSource, true); // "<a href=\"javascript:Zoom('" + ImgSource + "')\"><IMG SRC=\"" + ImgSource + "\" style=\"text-decoration:none;border-width:0px\" ALT=\"" + GetImageNameFromURL(ImgSource) + "\"/></a>";
                                            ImageZoom = ImageZoom.Replace("\\", "/");
                                            FamilyTabContent=FamilyTabContent.Replace("{" + FamImgAttr + "}", ImageZoom);
                                            FamilyTabContent = FamilyTabContent.Replace("START_FAMILY_IMAGE_ZOOM", "");
                                            FamilyTabContent = FamilyTabContent.Replace("END_FAMILY_IMAGE_ZOOM", "");
                                        }
                                        if (FamImgAttr==ZoomImgAttr && DC.ColumnName == FamImgAttr)
                                        {
                                            string ImageZoom = BuildFamilyImageContent(ImgSource, ImgSource, true);
                                            ImageZoom = ImageZoom.Replace("\\", "/");
                                            FamilyTabContent = FamilyTabContent.Replace("{" + FamImgAttr + "}", ImageZoom);
                                            FamilyTabContent = FamilyTabContent.Replace("START_FAMILY_IMAGE_ZOOM", "");
                                            FamilyTabContent = FamilyTabContent.Replace("END_FAMILY_IMAGE_ZOOM", "");
                                            StartLoc = FamilyTabContent.IndexOf("{START_ZOOM_FAMILY_ATTRIBUTE}");
                                            EndLoc = FamilyTabContent.IndexOf("{END_ZOOM_FAMILY_ATTRIBUTE}") + 27;
                                            if (StartLoc != -1 && EndLoc != -1)
                                            {
                                                FamilyTabContent = FamilyTabContent.Remove(StartLoc, EndLoc - StartLoc);
                                            }
                                        }

                                    }
                                    else
                                    {
                                        if (this.NoFamilyImage == true && DC.ColumnName!=ZoomImgAttr)
                                        {
                                            string ImageUrl = "images/NoImage.gif";
                                            ImageUrl = "<IMG SRC=\"" + ImageUrl + "\" Width =\"" + this.FamilyImageWidth + "\" Height=\"" + this.FamilyImageHeight + "\" style=\"border-width:0px;\" ALT=\"" + NoImageAvailableCaption + "\"/>";
                                            FamilyTabContent = FamilyTabContent.Replace("{" + DC.ColumnName.ToString() + "}", ImageUrl);
                                        }
                                        else
                                        {
                                            FamilyTabContent = FamilyTabContent.Replace("{" + DC.ColumnName.ToString() + "}", "&nbsp");
                                        }
                                    }
                                }
                            }
                        }
                        if (IsImageAttribute != true)
                        {
                            FamilyTabContent = FamilyTabContent.Replace("{" + DC.ColumnName.ToString() + "}", Dr[DC.ColumnName].ToString());
                        }
                        else
                        {
                            IsImageAttribute = false;
                        }
                    }
                }
                FamilyHtmlText = FamilyHtmlText + FamilyTabContent;
                int CheckIndex = 0;
                while (CheckIndex > -1)
                {
                    string ImageUrl;
                    CheckIndex = FamilyHtmlText.IndexOf("{");
                    if (CheckIndex > -1)
                    {
                        string oImgAttrName = FamilyHtmlText.Substring(CheckIndex + 1, FamilyHtmlText.IndexOf("}") - 1 - CheckIndex);
                        DataRow[] oDr = CXAttributes.Tables[0].Select("ATTRIBUTE_TYPE =9 AND ATTRIBUTE_NAME='" + oImgAttrName + "'");
                        if (oDr.Length > 0)
                        {
                            string TempFamilyAttrName = "";
                            foreach (DataColumn oDCTemp in oDSProdFamily.Tables[0].Columns)
                            {
                                if (oDCTemp.ColumnName.ToString() == oImgAttrName)
                                {
                                    TempFamilyAttrName = oImgAttrName;
                                }
                            }
                            if (TempFamilyAttrName.Trim() == string.Empty)
                            {
                                FamilyHtmlText = FamilyHtmlText.Replace("{" + oImgAttrName + "}", "");
                            }
                            if (this.NoFamilyImage == true && ZoomImgAttr != oImgAttrName)
                            {
                                ImageUrl = "images/NoImage.gif";
                                ImageUrl = "<IMG SRC=\"" + ImageUrl + "\" Width =\"" + this.FamilyImageWidth + "\" Height=\"" + this.FamilyImageHeight + "\" style=\"border-width:0px;\" ALT=\"" + NoImageAvailableCaption + "\"/>";
                                FamilyHtmlText = FamilyHtmlText.Replace("{" + oImgAttrName + "}", ImageUrl);
                            }
                            else if (oImgAttrName == ZoomImgAttr)
                            {
                                FamilyHtmlText = FamilyHtmlText.Replace("{" + ZoomImgAttr + "}", "");
                            }
                        }
                        else
                        {
                            FamilyHtmlText = FamilyHtmlText.Remove(CheckIndex, FamilyHtmlText.IndexOf("}") + 1 - CheckIndex);
                            FamilyHtmlText = FamilyHtmlText.Insert(CheckIndex, " &nbsp;");
                        }
                    }
                }
                }
            }
            catch (Exception Ex)
            {
                oErr.ErrorMsg = Ex;
                oErr.CreateLog();
                return "";
            }
            return FamilyHtmlText;
        }

        public void BuildFamilyZoomImage(DataSet oDSProdFamily,DataRow[] oDRAttr,string ZoomImageAttribute,string FamilyImgAttr,string MainImageSrc)
        {
            foreach (DataRow oDRProdFam in oDSProdFamily.Tables[0].Rows)
            {
                foreach (DataColumn oDC in oDSProdFamily.Tables[0].Columns)
                {
                    foreach (DataRow oDR in oDRAttr)
                    {
                        if (oDC.ColumnName == oDR[1].ToString())
                        {
                            //if ((FamilyTabContent.Contains("{ZOOM}")) && (FamilyTabContent.Contains("{START_ZOOM_FAMILY_ATTRIBUTE}")) && (FamilyTabContent.Contains("{END_ZOOM_FAMILY_ATTRIBUTE}")))
                            //{
                            if (oDC.ColumnName.ToString() == ZoomImageAttribute)
                            {
                                string ImgSource = oDRProdFam[oDC.ColumnName].ToString();
                                if (ImgSource.ToString() != null && ImgSource.ToString() != "")
                                {
                                    ImgSource = oHelper.GetOptionValues("PRODUCT IMAGE PATH").ToString() + ImgSource;
                                    //string ImageZoom = ZoomImage(ImgSource);
                                    string ImageZoom = BuildFamilyImageContent(MainImageSrc, ImgSource, true); 
                                    ImageZoom = ImageZoom.Replace("\\", "/");
                                    FamilyTabContent = FamilyTabContent.Replace("{" + FamilyImgAttr + "}", ImageZoom);
                                }
                            }
                                    
                            //}
                        }
                    }
                }
            }
            if(FamilyTabContent.Contains("{" + FamilyImgAttr + "}"))
            {
                string ImageZoom = BuildFamilyImageContent(MainImageSrc, MainImageSrc, true); 
                ImageZoom = ImageZoom.Replace("\\", "/");
                FamilyTabContent = FamilyTabContent.Replace("{" + FamilyImgAttr + "}", ImageZoom);
            }
        }

        //This is used to Build the Final Html
        /// <summary>
        /// This is used to Build the Final Html
        /// </summary>
        /// <param name="FamilyID">int</param>
        /// <param name="csTemplate">Boolean</param>
        /// <returns>string</returns>
        /// <example>
        /// <code>
        /// using System;
        /// using System.IO;
        /// using System.Web;
        /// using System.Data;
        /// using TradingBell.Common;
        /// using TradingBell.WebServices;
        /// 
        /// protected void Page_Load(object sender, EventArgs e)
        /// {
        ///  string  Template;
        ///  Boolean CSTemp;
        ///  int FamID;
        ///  ...
        ///  Template = BuildTemplate(int FamilyID, csTemplate)
        /// }
        /// </code>
        /// </example>
        [WebMethod]
        protected string BuildTemplate(int FamilyID,Boolean csTemplate)
        {
            DataSet oPRODData = new DataSet();
            string ProductTemplateHtml = "";
            string FamilyTemplateHtml = "";
            string TemplateSource = "";
            string ProductView = "";
            double RowSize;
            try
            {
                //HtmlTop = HtmlTop.Replace("{CATEGORY_HIERARCHY}", oTBFamily.CategoryName(FamilyID));
                HtmlTop = BuildHierarchy(HtmlTop,FamilyID);
                HtmlTop = BuildCatalogName(HtmlTop);
                CatID = oHelper.CI(_CatalogID);
                if (csTemplate == true)
                {
                    CSProductTable oCSProdTab = new CSProductTable(FamilyID.ToString(),oHelper.CS(CatID));
                    oCSProdTab.UserID = UserID;

                    oCSProdTab.DesciptionAttributeWidth = 300;
                    oCSProdTab.DesciptionHighAttributeWidth = 180;
                    oCSProdTab.DesciptionMidumAttributeWidth = 80;
                    oCSProdTab.DesciptionNormalAttributeWidth = 440;
                    oCSProdTab.ProductImgHeight = this.ProductImageHeight;
                    oCSProdTab.ProductImgWidth = this.ProductImageWidth;
                    oCSProdTab.IsVisibleShipping = true;
                    ProductTemplateHtml = oCSProdTab.GenerateFamilyPreview(); 
                    if (ProductTemplateHtml == string.Empty)
                    {
                        csTemplate = false;
                    }
                    else
                    {
                        FamilyTemplateHtml = BuildFamilyTemplate(CatID, FamilyID, UserID, FamilyTblContent);
                        if (FamilyFirst == 1)
                        {
                            ProductTemplateHtml = HtmlTop + FamilyTemplateHtml + ProductTemplateHtml + HtmlEnd;
                        }
                        else
                        {
                            ProductTemplateHtml = HtmlTop + ProductTemplateHtml + FamilyTemplateHtml + HtmlEnd;
                        }
                    }
                }
                if (csTemplate == false)
                {
                    FamilyTemplateHtml = BuildFamilyTemplate(CatID, FamilyID, UserID, FamilyTblContent);
                    oPRODData = oCSRender.GetCXProducts(CatID,FamilyID,UserID);
                    if (oPRODData != null)
                    {
                        if (oPRODData.Tables[0].Rows.Count > 0)
                        {
                            if (HeaderContent.ToString().Trim() == string.Empty)
                            {
                                ProductView = "FLAT";
                            }
                            else
                            {
                                ProductView = "GRID";
                            }
                            oProdRender.NoProductImage = this.NoProductImage;
                            oProdRender.ProductImageHeight = this.ProductImageHeight;
                            oProdRender.ProductImageWidth = this.ProductImageWidth;
                            if (HeaderContent == string.Empty && WOHeaderContent == string.Empty)
                            {
                                TemplateSource = oTemplate.GetTemplateHtmlSource((int)TemplateLayout.TemplateType.FamilyLayout);
                                DivideHTMLContent(TemplateSource);
                                if (ProductView == "FLAT")
                                {
                                    ProdRegHtmlS = ProdRegHtmlS.Replace("ID=\"tblTemplate\"", "");
                                }
                                ProductTemplateHtml = ProdRegHtmlS + oProdRender.BuildProductTemplate(CatID, FamilyID, UserID, oPRODData, HeaderContent, WOHeaderContent,columnDisplay,ProductView) + ProdRegHtmlE;
                            }
                            else
                            {
                                if (ProductView == "FLAT")
                                {
                                    //newly modified
                                    if (ProdRegHtmlS.Contains("<TABLE"))
                                    {
                                        ProdRegHtmlS = ProdRegHtmlS.Replace("ID=\"tblTemplate\"", "");
                                        ProdRegHtmlS = ProdRegHtmlS + "<TR><TD>";
                                        ProdRegHtmlE = "</TD></TR>" + ProdRegHtmlE;
                                    }
                                    //newly modified
                                }
                                ProductTemplateHtml = ProdRegHtmlS + oProdRender.BuildProductTemplate(CatID, FamilyID, UserID, oPRODData, HeaderContent, WOHeaderContent,columnDisplay,ProductView) + ProdRegHtmlE;
                            }
                            if (ProductView =="FLAT")
                            {
                                RowSize = Convert.ToDouble(oPRODData.Tables[0].Rows.Count) / Convert.ToDouble(columnDisplay);
                            }
                            else
                            {
                                RowSize = oPRODData.Tables[0].Rows.Count;
                            }
                            if (RowSize > Pagesize)
                            {
                                if (FamilyFirst == 1)
                                {
                                    ProductTemplateHtml = HtmlTop + oProdRender.BuildProductTemplateHead(ProductView) + FamilyTemplateHtml + ProductTemplateHtml + oProdRender.BuildProductTemplatePaging(Pagesize) + HtmlEnd;
                                }
                                else
                                {
                                    ProductTemplateHtml = HtmlTop + oProdRender.BuildProductTemplateHead(ProductView) + ProductTemplateHtml + oProdRender.BuildProductTemplatePaging(Pagesize) + FamilyTemplateHtml + HtmlEnd;
                                }
                            }
                            else
                            {
                                if (FamilyFirst == 1)
                                {
                                    ProductTemplateHtml = HtmlTop + oProdRender.BuildProductTemplateHead(ProductView) + FamilyTemplateHtml + ProductTemplateHtml + HtmlEnd;
                                }
                                else
                                {
                                    ProductTemplateHtml = HtmlTop + oProdRender.BuildProductTemplateHead(ProductView) + ProductTemplateHtml + FamilyTemplateHtml + HtmlEnd;
                                }
                            }
                        }
                        else
                        {
                            if (FamilyFirst == 1)
                            {
                                ProductTemplateHtml = HtmlTop + FamilyTemplateHtml + ProductTemplateHtml + HtmlEnd;
                            }
                            else
                            {
                                ProductTemplateHtml = HtmlTop + ProductTemplateHtml + FamilyTemplateHtml + HtmlEnd;
                            }
                        }
                    }
                    else
                    {
                        ProductTemplateHtml = HtmlTop + ProductTemplateHtml + FamilyTemplateHtml + HtmlEnd;
                    }
                }
            }
            catch (Exception Ex)
            {
                oErr.ErrorMsg = Ex;
                oErr.CreateLog();
                
                return "";
            }
            return ProductTemplateHtml;
        }

        [WebMethod]
        public string BuildHierarchy(string TemplateContent,int FamilyID)
        {
            ProductFamily oFam = new ProductFamily();
            string HierarchyContent = "";
            try
            {
                string CatID = "";
                string sSQL = "SELECT CATEGORY_ID FROM TB_FAMILY WHERE FAMILY_ID = " + FamilyID;
                oHelper.SQLString = sSQL;
                CatID = oHelper.GetValue("CATEGORY_ID");

                int loc = 0;
                string StartTag = "";
                string EndTag = "";
                if (TemplateContent.IndexOf("{CATEGORY_HIERARCHY}") > -1)
                {
                    if (TemplateContent.ToUpper().LastIndexOf("<FONT", TemplateContent.LastIndexOf("{CATEGORY_HIERARCHY}")) > -1)
                    {
                        loc = TemplateContent.ToUpper().LastIndexOf("<FONT", TemplateContent.LastIndexOf("{CATEGORY_HIERARCHY}"));
                        StartTag = TemplateContent.ToUpper().Substring(loc, (TemplateContent.IndexOf(">", loc) + 1) - loc);
                        EndTag = "</FONT>";
                        //Remove the Old Font Tag in Template
                        TemplateContent = TemplateContent.Remove(loc, (TemplateContent.IndexOf(">", loc) + 1) - loc);
                        loc = TemplateContent.ToUpper().IndexOf("</FONT>", TemplateContent.LastIndexOf("{CATEGORY_HIERARCHY}"));
                        TemplateContent = TemplateContent.Remove(loc, 7);
                    }
                    else if (TemplateContent.ToUpper().LastIndexOf("<P", TemplateContent.LastIndexOf("{CATEGORY_HIERARCHY}")) > -1)
                    {
                        loc = TemplateContent.ToUpper().LastIndexOf("<P", TemplateContent.LastIndexOf("{CATEGORY_HIERARCHY}"));
                        StartTag = TemplateContent.ToUpper().Substring(loc, (TemplateContent.IndexOf(">", loc) + 1) - loc);
                        EndTag = "</P>";
                        //Remove the Old Font Tag in Template
                        TemplateContent = TemplateContent.Remove(loc, (TemplateContent.IndexOf(">", loc) + 1) - loc);
                        loc = TemplateContent.ToUpper().IndexOf("</P>", TemplateContent.LastIndexOf("{CATEGORY_HIERARCHY}"));
                        TemplateContent = TemplateContent.Remove(loc, 4);
                    }
                    else if (TemplateContent.ToUpper().LastIndexOf("<SPAN", TemplateContent.LastIndexOf("{CATEGORY_HIERARCHY}")) > -1)
                    {
                        loc = TemplateContent.ToUpper().LastIndexOf("<SPAN", TemplateContent.LastIndexOf("{CATEGORY_HIERARCHY}"));
                        StartTag = TemplateContent.ToUpper().Substring(loc, (TemplateContent.IndexOf(">", loc) + 1) - loc);
                        EndTag = "</SPAN>";
                        //Remove the Old Font Tag in Template
                        TemplateContent = TemplateContent.Remove(loc, (TemplateContent.IndexOf(">", loc) + 1) - loc);
                        loc = TemplateContent.ToUpper().IndexOf("</SPAN>", TemplateContent.LastIndexOf("{CATEGORY_HIERARCHY}"));
                        TemplateContent = TemplateContent.Remove(loc, 7);
                    }
                }
                oFam.HirearchyStyleStartTag = StartTag;
                oFam.HirearchyStyleEndTag = EndTag;
                oFam.HierarchyCharacter = " > ";
                TemplateContent = TemplateContent.Replace("{CATEGORY_HIERARCHY}", oFam.GetParentCategory(CatID).ToString());
                HierarchyContent = TemplateContent;
            }
            catch (Exception ex)
            {
                oErr.ErrorMsg = ex;
                TemplateContent = TemplateContent.Replace("{CATEGORY_HIERARCHY}", "");
                HierarchyContent = TemplateContent;
            }
            return HierarchyContent;
            #region "Comments for another one type to replace the font tag"
            //string CatHierarchy = oFam.GetParentCategory(CatID).ToString();
            //string StrStartStyle = "<FONT COLOR=\"YELLOW\">";
            //string StrEndStyle="</FONT>";
            //loc = CatHierarchy.IndexOf("<A", loc);
            //while (loc != -1)
            //{
            //    loc = CatHierarchy.IndexOf(">", loc) + 1;
            //    CatHierarchy = CatHierarchy.Insert(loc, StrStartStyle);
            //    loc = CatHierarchy.IndexOf("</A>", loc);
            //    CatHierarchy = CatHierarchy.Insert(loc, StrEndStyle);
            //    loc = CatHierarchy.IndexOf("<A", loc);
            //}
            //return CatHierarchy;
            #endregion
        }

        [WebMethod]
        public string BuildCatalogName(string TemplateContent)
        {
            string NameContent = "";
            try
            {
                ProductFamily oFam = new ProductFamily();
                int loc = 0;
                string StartTag = "";
                string EndTag = "";
                if (TemplateContent.IndexOf("{CATALOG_NAME}") > -1)
                {
                    if (TemplateContent.ToUpper().LastIndexOf("<FONT", TemplateContent.LastIndexOf("{CATALOG_NAME}")) > -1)
                    {
                        loc = TemplateContent.ToUpper().LastIndexOf("<FONT", TemplateContent.LastIndexOf("{CATALOG_NAME}"));
                        StartTag = TemplateContent.ToUpper().Substring(loc, (TemplateContent.IndexOf(">", loc) + 1) - loc);
                        EndTag = "</FONT>";
                        //Remove the Old Font Tag in Template
                        TemplateContent = TemplateContent.Remove(loc, (TemplateContent.IndexOf(">", loc) + 1) - loc);
                        loc = TemplateContent.ToUpper().IndexOf("</FONT>", TemplateContent.LastIndexOf("{CATALOG_NAME}"));
                        TemplateContent = TemplateContent.Remove(loc, 7);
                    }
                    else if (TemplateContent.ToUpper().LastIndexOf("<P", TemplateContent.LastIndexOf("{CATALOG_NAME}")) > -1)
                    {
                        loc = TemplateContent.ToUpper().LastIndexOf("<P", TemplateContent.LastIndexOf("{CATALOG_NAME}"));
                        StartTag = TemplateContent.ToUpper().Substring(loc, (TemplateContent.IndexOf(">", loc) + 1) - loc);
                        EndTag = "</P>";
                        //Remove the Old Para Tag in Template
                        TemplateContent = TemplateContent.Remove(loc, (TemplateContent.IndexOf(">", loc) + 1) - loc);
                        loc = TemplateContent.ToUpper().IndexOf("</P>", TemplateContent.LastIndexOf("{CATALOG_NAME}"));
                        TemplateContent = TemplateContent.Remove(loc, 4);
                    }
                    else if (TemplateContent.ToUpper().LastIndexOf("<SPAN", TemplateContent.LastIndexOf("{CATALOG_NAME}")) > -1)
                    {
                        loc = TemplateContent.ToUpper().LastIndexOf("<SPAN", TemplateContent.LastIndexOf("{CATALOG_NAME}"));
                        StartTag = TemplateContent.ToUpper().Substring(loc, (TemplateContent.IndexOf(">", loc) + 1) - loc);
                        EndTag = "</SPAN>";
                        //Remove the Old Span Tag in Template
                        TemplateContent = TemplateContent.Remove(loc, (TemplateContent.IndexOf(">", loc) + 1) - loc);
                        loc = TemplateContent.ToUpper().IndexOf("</SPAN>", TemplateContent.LastIndexOf("{CATALOG_NAME}"));
                        TemplateContent = TemplateContent.Remove(loc, 7);
                    }
                }
                oFam.CatalogStyleStartTag = StartTag;
                oFam.CatalogStyleEndTag = EndTag;
                //TemplateContent = TemplateContent.Replace("{CATALOG_NAME}", oFam.GetCatalogName(oHelper.GetOptionValues("DEFAULT CATALOG")));
                TemplateContent = TemplateContent.Replace("{CATALOG_NAME}", oFam.GetCatalogName(oHelper.CS(Session["CATALOGID"])));
                NameContent = TemplateContent;
            }
            catch (Exception ex)
            {
                oErr.ErrorMsg = ex;
                TemplateContent = TemplateContent.Replace("{CATALOG_NAME}", "");
                NameContent = TemplateContent;
            }
            return NameContent;
        }
       
        //This is used to call the Template Design Functions
        /// <summary>
        /// This is used to call the Template Design Functions
        /// </summary>
        /// <param name="FamilyID">int</param>
        /// <param name="userId">int</param>
        /// <param name="HtmlContent">string</param>
        /// <param name="Rows">int</param>
        /// <param name="Columns">int</param>
        /// <param name="csTemplate">Boolean</param>
        /// <returns>string</returns>
        /// <example>
        /// <code>
        /// using System;
        /// using System.IO;
        /// using System.Web;
        /// using System.Data;
        /// using TradingBell.Common;
        /// using TradingBell.WebServices;
        /// 
        /// protected void Page_Load(object sender, EventArgs e)
        /// {
        ///   string TempHTML;
        ///   int FamilyID;
        ///   int userId;
        ///   string HtmlContent;
        ///   int Rows;
        ///   int Columns;
        ///   Boolean csTemplate;
        ///   ...
        ///   TempHTML = GenerateTemplateHtml(FamilyID,userId,HtmlContent,Rows,Columns,csTemplate)
        /// }
        /// </code>
        /// </example>
        [WebMethod]
        public string GenerateTemplateHtml(int FamilyID,int userId, string HtmlContent,int Rows,int Columns,Boolean csTemplate)
        {
            string GeneratedHtml = "";
            UserID = userId;
            Pagesize = Rows;
            columnDisplay = Columns;
            DivideHTMLContent(HtmlContent);
            GeneratedHtml = BuildTemplate(FamilyID,csTemplate);
            return GeneratedHtml;

        }
        public string GetImageNameFromURL(string ImageURL)
        {
            string TempUrl = "";
            string ImgFileNameWOExt = "";
            string ImgFileNameWExt = "";
            try
            {
                TempUrl = ImageURL.Replace("\\", "/");
                if (TempUrl.IndexOf("/") > -1)
                {
                    ImgFileNameWExt = TempUrl.Substring(TempUrl.LastIndexOf("/") + 1);
                }
                if (ImgFileNameWExt.IndexOf(".") > -1)
                {
                    ImgFileNameWOExt = ImgFileNameWExt.Remove(ImgFileNameWExt.IndexOf("."));
                }
            }
            catch (Exception ex)
            {
                oErr.ErrorMsg = ex;
                oErr.CreateLog();
            }
            return ImgFileNameWOExt;
        }

#endregion
    }

}