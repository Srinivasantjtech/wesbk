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
using System.IO;
using TradingBell.WebCat.CatalogDB;
using TradingBell.WebCat.Helpers;
using TradingBell.WebCat.TemplateRender;
using TradingBell.WebCat.CommonServices;
public partial class UI_ProductImages : System.Web.UI.UserControl
{
    HelperDB oHelper = new HelperDB();
    ErrorHandler oErr;
    int _ProductID;
    bool _ImageEnlarge;
    Double _ImageHeight;
    Double _ImageWidth;
    string _UserImagePath;
    int _CatalogID;
       
    #region "Property"
    [Browsable(true),
    Category("TradingBell"),
    DefaultValue("")
    ]
    public int ProductID
    {
        get
        {
            return _ProductID;
        }
        set
        {
            _ProductID = value;
        }
    }
    [
        Browsable(true),
        Category("TradingBell"),
        DefaultValue(""),
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
         DefaultValue(""),
          ]

    public bool ImageEnlarge
    {
        get
        {
            return _ImageEnlarge;
        }
        set
        {
            _ImageEnlarge = value;
        }
    }

    [
    Browsable(true),
    Category("TradingBell"),
    DefaultValue(""),
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
    DefaultValue("ImageWidth"),
    ]
    public Double ImageWidth
    {
        get
        {
            return _ImageWidth;
        }
        set
        {
            _ImageWidth = value;
        }
    }
    [
   Browsable(true),
   Category("TradingBell"),
   DefaultValue("ImageHeight"),
   ]
    public Double ImageHeight
    {
        get
        {
            return _ImageHeight;
        }
        set
        {
            _ImageHeight = value;
        }
    }
    //public DisplayText Display
    //{
    //    get
    //    {
    //        return _DisplayText; 
    //    }
    //    set
    //    {
    //        _DisplayText = value;
    //    }
    //}


    #endregion
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!DesignMode)
        {           
            Product oProudct = new Product();
            DataSet dsImg = new DataSet();
            oErr = new ErrorHandler();
            string ImgFilePath = "";
            ImageButton btnImg = new ImageButton();
            System.Drawing.Size newVal;
            Image imgProduct = new Image();         
          
            Double iHeight;
            Double iWidth;
            try
            {
                if (Request["Pid"] != null)
                {   
                    _ProductID = oHelper.CI(Request["Pid"].ToString());
                    _CatalogID  = oHelper.CI(oHelper.GetOptionValues("DEFAULT CATALOG").ToString());
                }
                Table tblProdImage = new Table();
                dsImg = oProudct.GetProductImages(_ProductID,_CatalogID);
               

                if (dsImg != null)
                {
                    foreach (DataRow rImg in dsImg.Tables[0].Rows)
                    {
                        TableRow RowImages = new TableRow();
                        TableCell cellImages = new TableCell();
                                                
                        imgProduct.ID = "ProductImage";
                        if (_ImageEnlarge ==  true)
                        {
                            btnImg.ID = "ibtnZoom";
                            btnImg.ImageUrl = "~/Images/zoom1.gif";
                            btnImg.ToolTip = "Zoom";
                            btnImg.OnClientClick = "javascript:Zoom()";
                        }
                        imgProduct.ImageUrl = _UserImagePath + rImg["STRING_VALUE"].ToString();
                        imgProduct.ImageUrl = imgProduct.ImageUrl.Replace("\\", "/");
                        if (File.Exists(Server.MapPath(_UserImagePath + rImg["STRING_VALUE"].ToString())) == true)
                        {
                            System.Drawing.Image oImg = System.Drawing.Image.FromFile(Server.MapPath(_UserImagePath + rImg["STRING_VALUE"].ToString()));
                            iHeight = (Double)imgProduct.Height.Value;
                            iWidth = (Double)imgProduct.Width.Value;
                            newVal = ScaleImage(oImg.Height, oImg.Width, _ImageWidth, _ImageHeight);
                                                       
                            imgProduct.Height = newVal.Height;
                            imgProduct.Width = newVal.Width;
                        }
                        
                        if (rImg["OBJECT_NAME"].ToString().Trim() != "")
                        {
                            imgProduct.ToolTip = rImg["OBJECT_NAME"].ToString();
                        }
                        else
                        {
                            string strImgName = rImg["STRING_VALUE"].ToString();
                            if (strImgName.Contains("\\") && strImgName.Contains("."))
                                imgProduct.ToolTip = strImgName.Substring(strImgName.LastIndexOf("\\") + 1, strImgName.Remove(0, strImgName.LastIndexOf("\\") + 1).LastIndexOf("."));
                        }
                        cellImages.Controls.Add(btnImg);
                        imgProduct.AlternateText = "No Image Available";
                        cellImages.Controls.Add(imgProduct);
                        RowImages.Cells.Add(cellImages);

                        //CellImgName.CssClass = "b1";
                        //CellImgName.Text = rImg["IMAGENAME"].ToString();
                        //CellImgName.HorizontalAlign = HorizontalAlign.Center;
                        //RowImgName.Cells.Add(CellImgName);
                        tblProdImage.Rows.Add(RowImages);
                        Controls.Add(tblProdImage);
                        ImgFilePath = imgProduct.ImageUrl;
                        string Scr = @"<script>";
                        Scr = Scr + " function Zoom()";
                        Scr = Scr + "{";
                        Scr = Scr + "var is_URL ='Zoom.aspx?ImgUrl=" + ImgFilePath + "';\n";
                        Scr = Scr + "var is_Features = 'left=300,Top=200,scrollbars=yes,width=500,height=500';\n";
                        Scr = Scr + "window.open(is_URL,'WEBCAT',is_Features);\n";
                        Scr = Scr + "}\n";
                        Scr = Scr + "</script>";
                        Page.RegisterClientScriptBlock("OpenZoom", Scr);
                        break;
                    }

                }
            }
            catch (Exception ex)
            {
                oErr.ErrorMsg = ex; 
                oErr.CreateLog();
            }

        }

    }

    public System.Drawing.Size ScaleImage(double origHeight, double origWidth, double Width, double Height)
    {
        System.Drawing.Size newSize = new System.Drawing.Size();
        double nWidth = Width;
        double nHeight = Height;
        double oWidth = origWidth;
        double oHeight = origHeight;
        //if (origHeight > 200 || origWidth > 200)
        //{
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
        //}
        newSize.Height = (int)nHeight;
        newSize.Width = (int)nWidth;
        return newSize;
    }


}
