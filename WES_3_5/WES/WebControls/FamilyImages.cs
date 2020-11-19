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

/// <summary>
///This web control is used to show the Family Images.
///(First Image or Except First Image or All Images)
/// </summary>



[assembly: TagPrefix("TradingBell.WebServices.UI", "WebCat")]
[assembly: System.Reflection.AssemblyVersion("5.0")]

namespace TradingBell.WebServices.UI
{
    public partial class FamilyImages : WebControl
    {
        #region "Declarations"
        public enum DisplayText
        {
            FirstImageOnly = 1,
            AllImages = 2,
            ExecptFirstImage = 3
        }

        DisplayText _CurrentDisplayText;
        int _FamilyID;
        string _UserImagePath;
        int _ImageCount;
        int _PopUpImageHeight;
        int _PopUpImageWidth;
        int _CatalogID;
        DataSet dsFamImg;
        Helper oHelper;
        string _ImgFilePath;
        ProductFamily oPF;
        Table tblImages;
        TableRow RowImages;
        TableCell cellImages;
        Image imgFamily;
        ImageButton btnImg = new ImageButton();

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

        public string UserImagePath
        {
            get
            {
                return _UserImagePath;
            }
            set
            {
                _UserImagePath = value;
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

        [
       Browsable(true),
       Category("TradingBell"),
       DefaultValue(""),
       Description("Enter no of images will be display.")
       ]
        public string ImgFilePath
        {
            get
            {
                return _ImgFilePath;
            }
            set
            {
                _ImgFilePath = value;
            }
        }
        [
        Browsable(true),
        Category("TradingBell"),
        DefaultValue(""),
        Description("PopUpImageWidth.")
        ]
        public int PopUpImageWidth
        {
            get
            {
                return _PopUpImageWidth;
            }
            set
            {
                _PopUpImageWidth = value;
            }
        }

        [
        Browsable(true),
        Category("TradingBell"),
        DefaultValue(""),
        Description("PopUpImageHeight.")
        ]
        public int PopUpImageHeight
        {
            get
            {
                return _PopUpImageHeight;
            }
            set
            {
                _PopUpImageHeight = value;
            }
        }

        #endregion
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
        }
        protected override void OnPreRender(EventArgs e)
        {
            try
            {

                dsFamImg = new DataSet();
                oHelper = new Helper();
                oPF = new ProductFamily();
                if (_FamilyID>0)
                {
                    _FamilyID = oHelper.CI(_FamilyID.ToString());
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
                        tblImages.ID = "FamilyFirstImage";
                        tblImages.CellPadding = 0;
                        tblImages.CellSpacing = 1;
                        foreach (DataRow rImg in dsFamImg.Tables[0].Rows)
                        {
                            if (rImg["IMAGEFILE"].ToString() != null)
                            {
                                imgFamily.ID = "FamilyImages1";
                                imgFamily.Width = 120;
                                imgFamily.Height = 120;
                                btnImg.ImageUrl = "~/Images/Zoom1.gif";

                                imgFamily.ImageUrl = _UserImagePath + rImg["IMAGEFILE"].ToString();
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
                                
                                _ImgFilePath = imgFamily.ImageUrl;
                                break;
                            }
                        }
                        RowImages = new TableRow();
                        cellImages = new TableCell();
                        // btnImg.OnClientClick = "javascript:Zoom()";
                        cellImages.Controls.Add(btnImg);
                        cellImages.HorizontalAlign = HorizontalAlign.Right;
                        RowImages.Controls.Add(cellImages);
                        tblImages.Controls.Add(RowImages);
                        this.Controls.Add(tblImages);


                        OpenPopUp(btnImg, "Zoom.aspx?ImgUrl=" + _ImgFilePath, _PopUpImageWidth, _PopUpImageHeight, "yes");
                        base.OnPreRender(e);
                    }
                }
              
            }
            catch (Exception nn)
            {
            }
        }

        protected override void RenderContents(HtmlTextWriter output)
        {
            if (DesignMode)
            {
                Image oImg = new Image();
                oImg.ImageUrl = "~/Images/FamilyImages.gif";
                oImg.ID = "imgDefault";
                oImg.Width = 125;
                this.Controls.Add(oImg);
                oImg.RenderControl(output);
            }
            if (_CurrentDisplayText == DisplayText.ExecptFirstImage)
            {
                LoadImages(output);
            }
            else if (_CurrentDisplayText == DisplayText.AllImages)
            {
                LoadImages(output);
            }

            if (dsFamImg != null)
            {
                tblImages.RenderControl(output);
            }
        }

        protected void btnImg_Click(object sender, EventArgs e)
        {
            OpenPopUp(btnImg, "Zoom.aspx?ImgUrl=" + _ImgFilePath, _PopUpImageWidth, _PopUpImageHeight, "yes");
        }
        #region "Functions"

        public void LoadImages(HtmlTextWriter oWriter)
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
                    if (imgCnt > 1 && _CurrentDisplayText == DisplayText.ExecptFirstImage)
                    {
                        imgFamily = new Image();
                        imgFamily.ID = "FamilyImage" + imgCnt;
                        imgFamily.Width = 120;
                        imgFamily.Height = 120;
                        imgFamily.ImageUrl = _UserImagePath + rImg["IMAGEFILE"].ToString();
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
                    else if (_CurrentDisplayText == DisplayText.AllImages)
                    {
                        imgFamily = new Image();
                        imgFamily.ID = "FamilyImagge" + imgCnt;
                        imgFamily.Width = 120;
                        imgFamily.Height = 120;
                        imgFamily.ImageUrl = _UserImagePath + rImg["IMAGEFILE"].ToString();
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
                //tblImages.RenderControl(oWriter);

            }
        }
        public void OpenPopUp(System.Web.UI.WebControls.WebControl opener, string PagePath,int width,int height,string scroll)
        {
            string clientScript;
            string windowAttribs;
            windowAttribs = "width=" + width + "px,height=" + height + "px,left=500,top=100,resizable=1,scrollbars=" + scroll;
          //  string val = "<div>";
            clientScript = "window.open('" + PagePath + "','Test','" + windowAttribs + "')";
           // val = val + clientScript+"</div>";

            opener.Attributes.Add("OnClick", clientScript);
        }
        #endregion
    }
}


