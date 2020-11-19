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
using Microsoft.SqlServer.Server;


/// <summary>
///This web control is used to display the the Product Image.
/// </summary>


[assembly: TagPrefix("TradingBell.WebServices.UI", "WebCat")]
[assembly: System.Reflection.AssemblyVersion("5.0")]

namespace TradingBell.WebServices.UI
 {

    public class ProductImages : WebControl
    {
        ErrorHandler oErr; 
        int _ProductID;
        Double _ImageWidth;
        Double _ImageHeight;
        string _UserImagePath;
        int _CatalogID;
        
        #region "Property"
        [
        Browsable(true),
        Category("TradingBell"),
        DefaultValue(""),
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
       // [
       //    Browsable(true),
       //Category("TradingBell"),
       //DefaultValue(""),
       // ]

       // public string ImageEnlarge
       // {
       //     get
       //     {
       //         return _ImageEnlarge;
       //     }
       //     set
       //     {
       //         _ImageEnlarge = value;
       //     }
       // }
        [
       Browsable(true),
       Category("TradingBell"),
       DefaultValue(""),
       Description("ImageWidth.")
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
       DefaultValue(""),
       Description("ImageHeight.")
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

        #endregion
        protected override void RenderContents(HtmlTextWriter output)
        {
            if (DesignMode)
            {

                Image oImg = new Image();
                oImg.ImageUrl = "~/Images/ProductImages.gif";
                oImg.ID = "imgDefault";
                oImg.Width = 125;
                this.Controls.Add(oImg);
                oImg.RenderControl(output);

            }
            else
            {
                Helper oHelper = new Helper();
                Product oProudct = new Product();
                DataSet dsImg = new DataSet();
                oErr = new ErrorHandler();
                Image imgProduct = new Image();
                TableRow RowImages = new TableRow();
                TableCell cellImages = new TableCell();

                System.Drawing.Size newVal;
                Double iHeight;
                Double iWidth;              

                try
                {
                    Table tblProdImage = new Table();
                    dsImg = oProudct.GetProductImages(_ProductID,_CatalogID);
                    string txt = dsImg.Tables[0].Rows.Count.ToString();
               
                    if (dsImg != null)
                    {
                        foreach (DataRow rImg in dsImg.Tables[0].Rows)
                        {                      
                            //if (_ImageEnlarge == "YES")
                            //{
                            //    btnImg.ID = "ibtnZoom";
                            //    btnImg.ImageUrl = "~/Images/zoom1.gif";
                            //    btnImg.ToolTip = "Zoom";
                            //    btnImg.OnClientClick = "javascript:Zoom()";
                            //}
                            imgProduct.ID = "ProductImage";
                            imgProduct.ImageUrl = _UserImagePath + rImg["STRING_VALUE"].ToString();
                            imgProduct.ImageUrl = imgProduct.ImageUrl.Replace("\\", "/");

                            if (File.Exists(Server.MapPath(_UserImagePath + rImg["STRING_VALUE"].ToString())) == true)
                            {
                                System.Drawing.Image oImg = System.Drawing.Image.FromFile(HttpContext.Current.Server.MapPath(_UserImagePath + rImg["STRING_VALUE"].ToString()));
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
                           
                            imgProduct.AlternateText = "No Image Available";
                            cellImages.Controls.Add(imgProduct);
                            RowImages.Cells.Add(cellImages);
                          
                            tblProdImage.Rows.Add(RowImages);
                            this.Controls.Add(tblProdImage);
                            tblProdImage.RenderControl(output);

                            //Add
                            //ImgFilePath = imgProduct.ImageUrl;
                            //string Scr = @"<script>";
                            //Scr = Scr + " function Zoom()";
                            //Scr = Scr + "{";
                            //Scr = Scr + "var is_URL ='Zoom.aspx?ImgUrl=" + ImgFilePath + "';\n";
                            //Scr = Scr + "var is_Features = 'left=300,Top=200,scrollbars=yes,width=500,height=500';\n";
                            //Scr = Scr + "window.open(is_URL,'WEBCAT',is_Features);\n";
                            //Scr = Scr + "}\n";
                            //Scr = Scr + "</script>";
                            //Page.RegisterClientScriptBlock("OpenZoom", Scr);
                            break;
                        }
                    }
                }
                catch (Exception ex)
                {
                    oErr.ErrorMsg = ex; // oErr.CreateLog();
                }
            }
        }

        private string ServerFromFile(object p)
        {
            throw new Exception("The method or operation is not implemented.");
        }
        public System.Drawing.Size ScaleImage(double origHeight, double origWidth, double Width, double Height)
        {
            System.Drawing.Size newSize = new System.Drawing.Size();
            double nWidth = Width;
            double nHeight = Height;
            double oWidth = origWidth;
            double oHeight = origHeight;
            if (origHeight > 200 || origWidth > 200)
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
    }
}