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
using System.ComponentModel;
using TradingBell.Common;
using TradingBell.WebServices;
using TradingBell.WebCat.CatalogDB;
using TradingBell.WebCat.Helpers;
using TradingBell.WebCat.TemplateRender;
using TradingBell.WebCat.CommonServices;
public partial class FamilyImages : System.Web.UI.UserControl
{
        #region "Declarations"
        public enum DisplayText
        {
            FirstImageOnly = 1,
            AllImages = 2,
            ExecptFirstImage = 3
        }
    HelperDB oHelper = new HelperDB();        
        DisplayText _CurrentDisplayText;
        int _FamilyID;
        int _ImageCount;
        int _CatalogID;
        DataSet dsFamImg;
        //Helper oHelper;
        ProductFamily oPF;

        Table tblImages;
        TableRow RowImages;
        TableCell cellImages;
        Image imgFamily;
    string UserImgPath;   
        #endregion

        # region "Property"
        [
        Browsable(true),
        Category("TradingBell"),
        DefaultValue("")   
        ]
        public DisplayText Display
        {
            get
            {
                return _CurrentDisplayText;
            }
            set
            {
                _CurrentDisplayText = value;
            }

        }
        [
        Browsable(true),
        Category("TradingBell"),
        DefaultValue("")
        ]
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
        [
        Browsable(true),
        Category("TradingBell"),
        DefaultValue("")
        ]
        public int FamilyID
        {
            get
            {
                return _FamilyID;
            }
            set
            {
                _FamilyID = value;
            }

        }
        [
        Browsable(true),
        Category("TradingBell"),
        DefaultValue(""),
        Description("Enter no of images will be display.")
        ]
        public int DisplayImageCount
        {
            get
            {
                return _ImageCount;
            }
            set
            {
                _ImageCount = value;
            }
        }
        #endregion
        protected void Page_Load(object sender, EventArgs e)
        {

            UserImgPath = oHelper.GetOptionValues("PRODUCT IMAGE PATH").ToString();
            
        }
    protected override void OnInit(EventArgs e)
    {
        dsFamImg = new DataSet();
        //oHelper = new Helper();
        oPF = new ProductFamily();
        string ImgFilePath = "";
        _CatalogID = oHelper.CI(oHelper.GetOptionValues("DEFAULT CATALOG").ToString());

        if (Request["Fid"] != null)
        {
            _FamilyID = oHelper.CI(Request["Fid"].ToString());
        }
        dsFamImg = oPF.GetFamilyImageList(_FamilyID,_CatalogID);

        if (_CurrentDisplayText == DisplayText.FirstImageOnly)
        {
            if (dsFamImg != null)
            {
                tblImages = new Table();
                RowImages = new TableRow();
                cellImages = new TableCell();
                imgFamily = new Image();
                ImageButton btnImg = new ImageButton();

                tblImages.ID = "FamilyFirstImage";
                tblImages.CellPadding = 0;
                tblImages.CellSpacing = 1;
                tblImages.BorderWidth = 0;
                btnImg.ID = "ibtnZoom";
                btnImg.ImageUrl = "~/Images/zoom1.gif";
                btnImg.ToolTip = "Zoom";

                foreach (DataRow rImg in dsFamImg.Tables[0].Rows)
                {
                    if (rImg["IMAGEFILE"].ToString() != null)
                    {
                        imgFamily.ID = "FamilyImages1";
                        imgFamily.Width = 120;
                        imgFamily.Height = 120;
                        //imgFamily.ImageUrl = @"\webcat\" + ImgPath + rImg["IMAGEFILE"].ToString();
                        UserImgPath = oHelper.GetOptionValues("PRODUCT IMAGE PATH").ToString();
                        imgFamily.ImageUrl = UserImgPath + rImg["IMAGEFILE"].ToString();
                        imgFamily.ImageUrl = imgFamily.ImageUrl.Replace("\\", "/");
                        if (rImg["IMAGENAME"].ToString().Trim() != "")
                        {
                            imgFamily.ToolTip = rImg["IMAGENAME"].ToString();
                        }
                        else
                        {
                            string strImgName = rImg["IMAGEFILE"].ToString();
                            if (strImgName.Contains("\\") && strImgName.Contains("."))
                                imgFamily.ToolTip = strImgName.Substring(strImgName.LastIndexOf("\\") + 1, strImgName.Remove(0, strImgName.LastIndexOf("\\") + 1).LastIndexOf("."));
                        }
                        imgFamily.AlternateText = "No Image Available";
                        cellImages.Controls.Add(imgFamily);
                        RowImages.Controls.Add(cellImages);
                        RowImages.Cells.Add(cellImages);
                        tblImages.Controls.Add(RowImages);
                        //Session["ImgFilePath"] = imgFamily.ImageUrl;
                        ImgFilePath = imgFamily.ImageUrl;
                        break;
                    }
                }
                RowImages = new TableRow();
                cellImages = new TableCell();
                //cellImages.Text = "Zoom";
                //cellImages.CssClass = "b1";
                //cellImages.ColumnSpan = 2;
                //RowImages.Controls.Add(cellImages);
                //cellImages = new TableCell();
                btnImg.OnClientClick = "javascript:Zoom()";
                cellImages.Controls.Add(btnImg);
                cellImages.HorizontalAlign = HorizontalAlign.Right;
                RowImages.Controls.Add(cellImages);
                tblImages.Controls.Add(RowImages);
                Controls.Add(tblImages);
            }
        }
        else if (_CurrentDisplayText == DisplayText.ExecptFirstImage)
        {
            LoadImages();
        }
        else if (_CurrentDisplayText == DisplayText.AllImages)
        {
            LoadImages();
        }

        //OrderId = 21;
        string Scr = @"<script>";
        Scr = Scr + " function Zoom()";
        Scr = Scr + "{";
        Scr = Scr + "var is_URL ='Zoom.aspx?ImgUrl=" + ImgFilePath + "';\n";
        Scr = Scr + "var is_Features = 'left=300,Top=200,scrollbars=yes,width=500,height=500';\n";
        Scr = Scr + "window.open(is_URL,'WEBCAT',is_Features);\n";
        Scr = Scr + "}\n";           
        Scr = Scr + "</script>";
        Page.RegisterClientScriptBlock("OpenZoom", Scr);
        base.OnInit(e);
    }
    #region "Functions"
    public void LoadImages()
    {
        if (dsFamImg != null)
        {
            tblImages = new Table();
            RowImages = new TableRow();
            TableRow RowImgName = new TableRow();
            int imgCnt = 1;
            int chkImgCnt = _ImageCount;

            tblImages.ID = "FamilyExceptFirst";
            tblImages.CellPadding = 0;
            tblImages.CellSpacing = 1;
            tblImages.BorderWidth = 0;

            foreach (DataRow rImg in dsFamImg.Tables[0].Rows)
            {
                cellImages = new TableCell();
                TableCell CellImgName = new TableCell();
                //Load Except first images
                if (imgCnt > 1 && _CurrentDisplayText == DisplayText.ExecptFirstImage)
                {
                    imgFamily = new Image();
                    imgFamily.ID = "FamilyImage" + imgCnt;
                    imgFamily.Width = 120;
                    imgFamily.Height = 120;
                    //imgFamily.ImageUrl = @"/WebCat/" + UserImgPath + rImg["IMAGEFILE"].ToString();
                    imgFamily.ImageUrl = UserImgPath + rImg["IMAGEFILE"].ToString();
                    imgFamily.ImageUrl = imgFamily.ImageUrl.Replace("\\", "/");
                    if (rImg["IMAGENAME"].ToString().Trim() != "")
                    {
                        imgFamily.ToolTip = rImg["IMAGENAME"].ToString();
                    }
                    else
                    {
                        string strImgName = rImg["IMAGEFILE"].ToString();
                        if (strImgName.Contains("\\") && strImgName.Contains("."))
                            imgFamily.ToolTip = strImgName.Substring(strImgName.LastIndexOf("\\") + 1, strImgName.Remove(0, strImgName.LastIndexOf("\\") + 1).LastIndexOf("."));
                    }
                    imgFamily.AlternateText = "No Image Available";                    
                    cellImages.Controls.Add(imgFamily);
                    RowImages.Cells.Add(cellImages);

                    CellImgName.CssClass = "b1";
                    CellImgName.Text = rImg["IMAGENAME"].ToString();
                    CellImgName.HorizontalAlign = HorizontalAlign.Center;
                    RowImgName.Cells.Add(CellImgName);
                }
                //Load All  images
                else if(_CurrentDisplayText == DisplayText.AllImages)
                {
                    imgFamily = new Image();
                    imgFamily.ID = "FamilyImagge" + imgCnt;
                    imgFamily.Width = 120;
                    imgFamily.Height = 120;
                    //imgFamily.ImageUrl = @"/WebCat/" + UserImgPath + rImg["IMAGEFILE"].ToString();
                    imgFamily.ImageUrl = UserImgPath + rImg["IMAGEFILE"].ToString();
                    imgFamily.ImageUrl = imgFamily.ImageUrl.Replace("\\", "/");
                    if (rImg["IMAGENAME"].ToString().Trim() != "")
                    {
                        imgFamily.ToolTip = rImg["IMAGENAME"].ToString();
                    }
                    else
                    {
                        string strImgName = rImg["IMAGEFILE"].ToString();
                        if (strImgName.Contains("\\") && strImgName.Contains("."))
                            imgFamily.ToolTip = strImgName.Substring(strImgName.LastIndexOf("\\") + 1, strImgName.Remove(0, strImgName.LastIndexOf("\\") + 1).LastIndexOf("."));
                    }
                    imgFamily.AlternateText = "No Image Available";
                    cellImages.Controls.Add(imgFamily);
                    RowImages.Cells.Add(cellImages);

                    CellImgName.CssClass = "b1";
                    CellImgName.Text = rImg["IMAGENAME"].ToString();
                    CellImgName.HorizontalAlign = HorizontalAlign.Center;
                    RowImgName.Cells.Add(CellImgName);
                }
                tblImages.Controls.Add(RowImages);
                tblImages.Controls.Add(RowImgName);
                if (imgCnt > chkImgCnt)
                {
                    cellImages = new TableCell();
                    RowImages = new TableRow();
                    CellImgName = new TableCell();
                    RowImgName = new TableRow();
                    chkImgCnt = chkImgCnt + _ImageCount;
                }
                imgCnt = imgCnt + 1;
            }
            Controls.Add(tblImages);
        }
    }
    #endregion
}



