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
using TradingBell.WebCat.EasyAsk;
using TradingBell.WebCat.TemplateRender;
using TradingBell.WebCat.CommonServices;
public partial class UI_FamilyAttributes : System.Web.UI.UserControl
{
    #region "Declarations.."
    HelperDB oHelper;
    ErrorHandler oErr;
    ProductFamily oPF;
    string ImagePath;
    int _FamilyID;
    int _CatalogID;

    public enum DispayType
    {
        SpecificationType = 1,
        DescriptionType=2,
        DocumentType =3
    }
    string _HeaderCss;
    string _CSSClass;

    Table tblFAttr = new Table();
    DataSet dsFam = new DataSet();
    DispayType _DisplayType;
    #endregion
    #region "Property"
        [Browsable(true),
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
        [Browsable(true),
        Category("TradingBell"),
        DefaultValue("")
        ]
        public DispayType Display
        {
            get
            {
                return _DisplayType;
            }
            set 
            {
                _DisplayType = value;
            }
        }
        [
        Browsable(true),
        Category("TradingBell"),
        DefaultValue(""),
        Description("Attribute Header CSSClass")
        ]
        public string HeaderCSSClass
        {
            get
            {
                return _HeaderCss;
            }
            set
            {
                _HeaderCss = value;
            }
        }
        [
        Browsable(true),
        Category("TradingBell"),
        DefaultValue(""),
        Description("Attribute value CSSClass")
        ]
        public string CSSClass
        {
            get
            {
                return _CSSClass;
            }
            set
            {
                _CSSClass = value;
            }

        }
    #endregion
    protected void Page_Load(object sender, EventArgs e)
    {
       oHelper = new HelperDB();
       oErr = new ErrorHandler();
       oPF = new ProductFamily();
       //ImagePath = ConfigurationManager.AppSettings["ImagePath"];
       _CatalogID = oHelper.CI(oHelper.GetOptionValues("DEFAULT CATALOG").ToString());
       ImagePath = oHelper.GetOptionValues("IMAGE PATH").ToString();

       if (Request["Fid"] != null)
       {
           _FamilyID = oHelper.CI(Request["Fid"].ToString());
       }
        if (_DisplayType == DispayType.SpecificationType)
            LoadAttributes(oPF.GetFamilySpecList(_FamilyID,_CatalogID));
        //else if (_DisplayType == DispayType.DescriptionType)
        //    LoadAttributes(oPF.GetFamilyDescList(_FamilyID));
        else if (_DisplayType == DispayType.DocumentType)
        {
            
            LoadPDFAttributes(oPF.GetFamilyPdf(_FamilyID));
        }
    }
    
    #region "Functions.."
    public void LoadAttributes(DataSet dsFamAttr)
    {
        if (dsFamAttr != null)
        {   
            foreach (DataRow rAttr in dsFamAttr.Tables[0].Rows)
            {
                TableRow RowAttrName = new TableRow();
                TableRow RowAttrValue = new TableRow();
                TableCell cellAttrName = new TableCell();
                TableCell cellAttrValue = new TableCell();

                cellAttrName.Text = rAttr["ATTRIBUTENAME"].ToString();
                cellAttrValue.Text = rAttr["ATTRIBUTEVALUE"].ToString().Replace("\r\n", "<br/>");
                RowAttrName.Cells.Add(cellAttrName);
                RowAttrName.CssClass = _HeaderCss;
                RowAttrValue.Cells.Add(cellAttrValue);
                RowAttrValue.CssClass = _CSSClass;
                tblFAttr.Rows.Add(RowAttrName);
                tblFAttr.Rows.Add(RowAttrValue);
            }
        }
        //tblFAttr.Width = 300;
        Controls.Add(tblFAttr);
        dsFamAttr = null;
    }
    public void LoadPDFAttributes(DataSet dsPDF)
    {
        Table tblFaImgIcon = new Table();
        if (dsPDF != null)
        {

            TableRow RowIcon = new TableRow();
            TableRow RowIconValue = new TableRow();
            TableCell cellIcon = new TableCell();
            TableCell cellIconValue = new TableCell();
            //Table tblAttachment = new Table();
            //TableRow RowTitle = new TableRow();
            ////TableRow RowAttrValue = new TableRow();
            TableCell CellAttach = new TableCell();
            //CellAttach.Text = "PDF Attachment : ";
            //CellAttach.CssClass = _HeaderCss;
            ////CellAttach.CssClass = _CssClass;
            //RowTitle.Cells.Add(CellAttach);
            //tblAttachment.Rows.Add(RowTitle);
            int i = 1;
            foreach (DataRow rPDF in dsPDF.Tables[0].Rows)
            {
                ImageButton ibtnPDF = new ImageButton();
                ibtnPDF.ID = "ImgPdf" + i;
                //ibtnPDF.ImageUrl = @"\WebCat\" + ImagePath + "/PdfImage.jpg";
                ibtnPDF.ImageUrl = ImagePath + "/PdfImage.jpg";
                ibtnPDF.Width = 30;
                ibtnPDF.Height = 30;
                string ImgFilePath = ImagePath + rPDF["IMAGEFILE"].ToString();
                ImgFilePath = ImgFilePath.Replace("\\", "/");
                
//               // ibtnPDF.OnClientClick = "javascript:window.open('" + ImgFilePath + "');target:main,scrollbars=yes,width=990,height=750";
                CellAttach.Text = rPDF["ATTRIBUTENAME"].ToString();

              ibtnPDF.OnClientClick = "javascript:window.open('" + ImgFilePath + "');window.document.title='PDF';target:main,scrollbars=yes,width=990,height=750";


                
               
                //string Scr = @"<script>";
                //Scr = Scr + " function openindex()";
                //Scr = Scr + "{";
                //Scr = Scr + "var is_URL ='" + ImgFilePath + "'";
                //Scr = Scr + "var is_Features = 'left=300,Top=200,scrollbars=yes,width=500,height=500';\n";
                //Scr = Scr + "window.open(is_URL,'WEBCAT',is_Features);\n";
                //Scr = Scr + "}\n";
                //Scr = Scr + "</script>";
                //Page.RegisterClientScriptBlock("OpenZoom", Scr);
                //ibtnPDF.OnClientClick = "javascript:openindex()";
                //ibtnPDF.ToolTip = rPDF["IMAGENAME"].ToString(); 
                cellIcon.Controls.Add(ibtnPDF);
                cellIconValue.Text = rPDF["IMAGENAME"].ToString();
                cellIcon.Width = 32;
                CellAttach.CssClass = _CSSClass;
                CellAttach.Font.Bold = true;
                RowIcon.Cells.Add(CellAttach);
                RowIcon.Cells.Add(cellIcon);
                RowIcon.Cells.Add(cellIconValue);
             
                tblFaImgIcon.Rows.Add(RowIcon);
            }
        }
        dsPDF = null;
        Controls.Add(tblFaImgIcon);
    }
    #endregion
}
