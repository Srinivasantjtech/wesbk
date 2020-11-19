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
///This web control is used to get all the Family Attributes and its Values.
/// </summary>


[assembly: TagPrefix("TradingBell.WebServices.UI", "WebCat")]
[assembly: System.Reflection.AssemblyVersion("5.0")]
namespace TradingBell.WebServices.UI
{
    public class FamilyAttributes : WebControl
    {
        #region "Declarations.."
        Helper oHelper;
        ErrorHandler oErr;
        ProductFamily oPF;
        string ImagePath;
        int _FamilyID;
        int _CatalogID;

        public enum DispayType
        {
            SpecificationType = 1,
           // DescriptionType = 2,
            Attachments = 3
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

        protected override void RenderContents(HtmlTextWriter output)
        {
            if (DesignMode)
            {
                Image oImg = new Image();
                oImg.ImageUrl = "~/Images/FamilyAttributes.gif";
                oImg.ID = "imgDefault";
                oImg.Width = 125;
                this.Controls.Add(oImg);
                oImg.RenderControl(output);
            }
            else
            {
                oHelper = new Helper();
                oErr = new ErrorHandler();
                oPF = new ProductFamily();
                //ImagePath = ConfigurationManager.AppSettings["ImagePath"];
                ImagePath = Helper.WebCatGlb["IMAGE PATH"].ToString();

                if (_FamilyID >0 )
                {
                    _FamilyID = oHelper.CI(_FamilyID.ToString());
                }


                if (_DisplayType == DispayType.SpecificationType)
                    LoadAttributes(oPF.GetFamilySpecList(_FamilyID,_CatalogID), output);
                //else if (_DisplayType == DispayType.DescriptionType)
                //    LoadAttributes(oPF.GetFamilyDescList(_FamilyID));
                else if (_DisplayType == DispayType.Attachments)
                    LoadPDFAttributes(oPF.GetFamilyPdf(_FamilyID), output);

                //output.Write(Text);
            }
        }

        #region "Functions.."
        public void LoadAttributes(DataSet dsFamAttr,HtmlTextWriter output)
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
            tblFAttr.RenderControl(output);
           // Controls.Add(tblFAttr);
            dsFamAttr = null;
            
        }
        public void LoadPDFAttributes(DataSet dsPDF,HtmlTextWriter output)
        {
            Table tblFaImgIcon = new Table();
            if (dsPDF != null)
            {

                TableRow RowIcon = new TableRow();
                TableRow RowIconValue = new TableRow();
                TableCell cellIcon = new TableCell();
                TableCell cellIconValue = new TableCell();
                TableCell CellAttach = new TableCell();
                //CellAttach.Text = "PDF Attachment : ";
                int i = 1;
                foreach (DataRow rPDF in dsPDF.Tables[0].Rows)
                {
                    CellAttach.Text = rPDF["ATTRIBUTENAME"].ToString();
                    ImageButton ibtnPDF = new ImageButton();
                    ibtnPDF.ID = "ImgPdf" + i;
                    //ibtnPDF.ImageUrl = @"\WebCat\" + ImagePath + "/PdfImage.jpg";
                    ibtnPDF.ImageUrl = ImagePath + "/PdfImage.jpg";
                    ibtnPDF.Width = 30;
                    ibtnPDF.Height = 30;
                    string ImgFilePath = ImagePath + rPDF["IMAGEFILE"].ToString();
                    ImgFilePath = ImgFilePath.Replace("\\", "/");
                    ibtnPDF.OnClientClick = "javascript:window.open('" + ImgFilePath + "');window.document.title='PDF';target:main,scrollbars=yes,width=990,height=750";
                    ibtnPDF.ToolTip = rPDF["IMAGENAME"].ToString();
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
            tblFaImgIcon.RenderControl(output);
            dsPDF = null;
            //Controls.Add(tblFaImgIcon);
        }
        #endregion

    }
}
