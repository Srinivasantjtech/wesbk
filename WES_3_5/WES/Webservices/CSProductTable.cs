using System;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Collections;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using System.Drawing;
using TradingBell5.CatalogX;
using TradingBell.Common;
using TradingBell.WebServices;
using TradingBell.WebCat.CatalogDB;
using TradingBell.WebCat.Helpers;
using TradingBell.WebCat.EasyAsk;
namespace TradingBell.WebServices
{
    /// <summary>
    /// Summary description for CSProductTable
    /// </summary>
    /// 
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class CSProductTable : System.Web.Services.WebService
    {
        #region "Declarations For Pivot Table"
        TradingBell5.CatalogX.CSDBProvider.Connection oConn = new TradingBell5.CatalogX.CSDBProvider.Connection();
        TradingBell5.CatalogX.CSDBProvider.CSDSTableAdapters.TB_FAMILYTableAdapter oTaFam = new TradingBell5.CatalogX.CSDBProvider.CSDSTableAdapters.TB_FAMILYTableAdapter();
        TradingBell5.CatalogX.CSDBProvider.CSDS.TB_FAMILYDataTable oDTFam = new TradingBell5.CatalogX.CSDBProvider.CSDS.TB_FAMILYDataTable();
        TradingBell5.CatalogX.CSDBProvider.CSDSTableAdapters.TB_ATTRIBUTETableAdapter oTaAttr = new TradingBell5.CatalogX.CSDBProvider.CSDSTableAdapters.TB_ATTRIBUTETableAdapter();
        TradingBell5.CatalogX.CSDBProvider.CSDS.TB_ATTRIBUTEDataTable oDTAttr = new TradingBell5.CatalogX.CSDBProvider.CSDS.TB_ATTRIBUTEDataTable();

        private List<string> m_rowFields;
        private List<string> m_columnFields;
        private List<string> m_summaryFields;
        private string m_summaryGroupField;
        private string m_familyID, m_catalogID;
        private bool m_showRowHeaders;
        private bool m_showColumnHeaders;
        private bool m_showSummaryHeaders;
        private bool m_mergeRowHeaders;
        private bool m_mergeSummaryFields;
        private string m_placeHolderText;
        private DataTable pivotTable, rowTable, columnTable, sourceTable, attributeTable;
        private string currencyFormat;
        private bool applyCurrencySymbolForFirstRow;
        private string mCheckLayout;
        Connection oCon = new Connection();
        Helper oHelper = new Helper();
        ErrorHandler oErr = new ErrorHandler();
        BuyerGroup oBG = new BuyerGroup();
        Product oPro = new Product();
        string Prefix = string.Empty; string Suffix = string.Empty; string EmptyCondition = string.Empty; string ReplaceText = string.Empty; string Headeroptions = string.Empty;
        System.Random RandomClass = new Random();
        //string AttrPriceIDs;
        //int UserID = 0;
        int[] iAttrList = new int[0];
        #endregion

        #region "Declarations For ProductPreview

        private int _familyID;
        private int _UserID;
        private int _catalogID;
        private bool _displayHeader;
        private int _DesciptionAttributeWidth;
        private int _DesciptionMidumAttributeWidth;
        private int _DesciptionNormalAttributeWidth;
        private int _DesciptionHighAttributeWidth;
        private double _ProductImgHeight;
        private double _ProductImgWidth;
        private static int ContinueCalclautedCols;
        private static int CheckCalculatedCtr;
        Order oOrder = new Order();
        DataSet DsPreview = new DataSet();
        DataTable oModfdColDT;
        string Restricted = "NO";
        string[] SeparateLine = new string[] { "" };
        //ProductValidationServices Oservices = new ProductValidationServices();
        //ServiceProvider.ProductValidationServices Oservices = new TradingBell5.CatalogStudio.ServiceProvider.ProductValidationServices();
        private int _valInc;
        int ProdID;
        string cellvalues; string XMLcell; string XMLdes, GrpColItem; int levelpros;//, gropsize;
        int Mergecellcount; int totalrows, totalcols, crow, AllinGroup = 0; int HeaderID;
        StringBuilder strBuildXMLOutput = new StringBuilder();
        StringBuilder strBuildXMLOutput1 = new StringBuilder();
        int[] chkAttrType;
        int rowVal = -1;
        int[] rowlevel; int[] cellposval; int[] cellposition; int[] GrpColno; int grpno;
        bool _IsVisibleShipping = false;
        //Infragistics.Win.UltraWinGrid.UltraGrid obj = new Infragistics.Win.UltraWinGrid.UltraGrid();
        //Infragistics.Win.UltraWinGrid.UltraGrid obj = new UltraGrid();
        //DBConnection Ocon = new DBConnection();
        //Connection oCon = new Connection();
        //private string Prefix, Suffix, EmptyCondition, ReplaceText, Headeroptions = string.Empty;
        string[] ModfdColNames;
        private Image chkSize;

        #endregion

        #region "Declaration For ProductValidation Services"
        public enum FieldValidationTypes
        {
            /// <summary>
            /// Name
            /// </summary>
            NAMEFIELD = 1,
            /// <summary>
            /// E_Mail ID
            /// </summary>
            EMAIL = 2,
            /// <summary>
            /// URL
            /// </summary>
            URL = 3,
            /// <summary>
            /// Phone No.
            /// </summary>
            PHONE = 4,
            /// <summary>
            /// Zip Code
            /// </summary>
            ZIP = 5,

        }
        private int _AttributeType;
        private int _AttributeID;
        private static int TextLength;
        private string _ObjValue;
        private static int AfterDecimalVal;
        private static int BeforeDecimalVal;
        private static bool dstosuitesubfamopt = false;

        //AppLoader.DBConnection Ocon = new TradingBell5.CatalogStudio.AppLoader.DBConnection();
        #endregion

        #region "Properties For Product Preview"
        public int UserID
        {
            get
            {
                return _UserID;
            }
            set
            {
                _UserID = value;
            }
        }
        public int valInc
        {
            get
            {
                return _valInc;
            }
            set
            {
                _valInc = value;
            }
        }

        public int FamilyID
        {
            get
            {
                return _familyID;
            }
            set
            {
                _familyID = value;
            }
        }
        public int CatalogID
        {
            get
            {
                return _catalogID;
            }
            set
            {
                _catalogID = value;

            }
        }

        public bool DisplayHeaders
        {
            get
            {
                return _displayHeader;
            }
            set
            {
                _displayHeader = value;
            }
        }

        public int DesciptionAttributeWidth
        {
            get
            {
                return _DesciptionAttributeWidth;
            }
            set
            {
                _DesciptionAttributeWidth = value;
            }
        }

        public int DesciptionMidumAttributeWidth
        {
            get
            {
                return _DesciptionMidumAttributeWidth;
            }
            set
            {
                _DesciptionMidumAttributeWidth = value;
            }
        }

        public int DesciptionNormalAttributeWidth
        {
            get
            {
                return _DesciptionNormalAttributeWidth;
            }
            set
            {
                _DesciptionNormalAttributeWidth = value;
            }
        }

        public int DesciptionHighAttributeWidth
        {
            get
            {
                return _DesciptionHighAttributeWidth;
            }
            set
            {
                _DesciptionHighAttributeWidth = value;
            }
        }
        public bool IsVisibleShipping
        {
            get
            {
                return _IsVisibleShipping;
            }
            set
            {
                _IsVisibleShipping = value;
            }
        }
        public double ProductImgHeight
        {
            get
            {
                return _ProductImgHeight;
            }
            set
            {
                _ProductImgHeight = value;
            }
        }
        public double ProductImgWidth
        {
            get
            {
                return _ProductImgWidth;
            }
            set
            {
                _ProductImgWidth = value;
            }
        }
        #endregion

        #region "Properties For Product Validation Services
        public int AttributeType
        {
            get
            {
                return _AttributeType;
            }
            set
            {
                _AttributeType = value;
            }
        }
        public int AttributeID
        {
            get
            {
                return _AttributeID;
            }
            set
            {
                _AttributeID = value;
            }
        }
        public string ObjValue
        {
            get
            {
                return _ObjValue;
            }
            set
            {
                _ObjValue = value;
            }
        }

        #endregion

        #region "Pivot Table Methods"

        public CSProductTable()
        {
            string Con = oCon.ConnectionString.ToString();
            Con = Con.Remove(0, Con.IndexOf(';') + 1);
            oConn.ConnSettings(Con);
            this.FamilyID = oHelper.CI(HttpContext.Current.Request["Fid"].ToString());
            //this.CatalogID = oHelper.CI(oHelper.GetOptionValues("DEFAULT CATALOG").ToString());
        }

        public CSProductTable(string familyID, string catalogID)
        {

            m_familyID = familyID;
            this.FamilyID = oHelper.CI(m_familyID);
            m_catalogID = catalogID;
            this.CatalogID = oHelper.CI(m_catalogID);
            string Con = oCon.ConnectionString.ToString();
            Con = Con.Remove(0, Con.IndexOf(';') + 1);
            oConn.ConnSettings(Con);
            //oTaFam.Fill(oDTFam);
            //oTaAttr.Fill(oDTAttr);
        }

        public string CheckLayout
        {
            get { return mCheckLayout; }
            set { mCheckLayout = value; }
        }

        /// <summary>
        /// This is used to generate Pivot Table HTML Content
        /// </summary>
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
        /// using System;
        /// using System.IO;
        /// using System.Web;
        /// using System.Data;
        /// using TradingBell.Common;
        /// using TradingBell.WebServices;
        /// 
        /// protected void Page_Load(object sender, EventArgs e)
        /// {
        ///   string xmlStr;
        ///   ...
        ///   if (m_rowFields.Count > 0 && m_columnFields.Count > 0)
        ///   {
        ///      this.ConstructPivotTable();
        ///      xmlStr = this.GeneratePivotXML();
        ///   }   
        /// }
        /// </code>
        /// </example>
        [WebMethod]
        public string GeneratePivotXML()
        {
            string xmlStr = string.Empty;
            //Retrieve the Pivot Layout from Database
            this.DecodeLayoutXML(this.RetrievePivotLayout());
            if (m_rowFields.Count > 0 && m_columnFields.Count > 0)
            {
                this.FetchSourceData();
                this.SetAttributePropertiesForSourceTable();
                this.ConstructPivotTable();
                xmlStr = this.GenerateXML();
            }
            return xmlStr;
        }
        /// <summary>
        /// This is used to generate the XML 
        /// </summary>
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
        ///   this.ConstructPivotTable();
        ///   xmlStr = this.GenerateXML();
        /// }
        /// </code>
        /// </example>
        [WebMethod]
        public string GenerateXML()
        {
            string pivotXML = string.Empty;
            FileInfo chkFile = null;
            StringBuilder strBldr = new StringBuilder();
            int hmCount = 0;

            if (pivotTable.Rows.Count > 0 && rowTable.Rows.Count > 0 && columnTable.Rows.Count > 0)
            {

                //Initialize the Extended properties for use in vertical merging
                for (int j = 0; j < pivotTable.Columns.Count; j++)
                {
                    pivotTable.Columns[j].ExtendedProperties.Remove("MergeCounter");
                    pivotTable.Columns[j].ExtendedProperties.Add("MergeCounter", "0");
                }

                strBldr.Append("<products TBGUID=\"P" + RandomClass.Next() + "\">");
                strBldr.Append("<table xmlns:aid=\"http://ns.adobe.com/AdobeInDesign/4.0/\" aid:table=\"table\" ");
                strBldr.Append("aid:trows=\"" + pivotTable.Rows.Count + "\" aid:tcols=\"" + pivotTable.Columns.Count + "\" nrows=\"" + pivotTable.Rows.Count + "\"  ncols=\"" + pivotTable.Columns.Count + "\"  TBGUID=\"" + RandomClass.Next() + "\"  Format=\"Horizontal\">");

                for (int i = 0; i < pivotTable.Rows.Count; i++)
                {
                    for (int j = 0; j < pivotTable.Columns.Count; j++)
                    {
                        if (Convert.ToInt32(pivotTable.Columns[j].ExtendedProperties["MergeCounter"].ToString()) > 0)
                        {
                            int temp = Convert.ToInt32(pivotTable.Columns[j].ExtendedProperties["MergeCounter"].ToString());
                            temp = temp - 1;
                            pivotTable.Columns[j].ExtendedProperties["MergeCounter"] = temp.ToString();
                        }
                        bool ImageType = false;
                        if (hmCount > 0) { hmCount = hmCount - 1; }
                        //For Column Header and ColumnFields -- Horizondal Merging
                        if (i < columnTable.Columns.Count)
                        {
                            string cellName = "Cell";
                            if (columnTable.Columns[i].ExtendedProperties.ContainsKey("ATTRIBUTE_STYLE"))
                            {
                                cellName = columnTable.Columns[i].ExtendedProperties["ATTRIBUTE_STYLE"].ToString();
                            }
                            if (cellName == string.Empty)
                            { cellName = "Cell"; }

                            if (j < rowTable.Columns.Count)
                            { cellName = cellName + "_Header"; }

                            if (hmCount == 0)
                            {
                                int mColCnt = GetHorizontalMergeCells(j, i);
                                if (mColCnt > 1)
                                {
                                    if (pivotTable.Rows[i][j].ToString() != string.Empty)
                                    {
                                        chkFile = new FileInfo(pivotTable.Rows[i][j].ToString());
                                    }
                                    if (chkFile != null)
                                    {
                                        if (chkFile.Extension.ToUpper() == ".BMP" || chkFile.Extension.ToUpper() == ".JPG" || chkFile.Extension.ToUpper() == ".GIF" || chkFile.Extension.ToUpper() == ".EPS")
                                        {
                                            ImageType = true;
                                        }
                                        if (ImageType == true)
                                        {
                                            strBldr.Append("<" + cellName + " aid:table=\"cell\" aid:theader=\"\"  aid:crows=\"1\"   aid:ccols=\"" + mColCnt.ToString() + "\"" + "TBGUID=\"TP" + RandomClass.Next() + "\">");
                                            strBldr.Append("<image_name COTYPE=\"IMAGE\"  TBGUID=\"CI" + RandomClass.Next() + "\"  IMAGE_FILE=\"" + pivotTable.Rows[i][j].ToString() + "\"></image_name>");
                                            strBldr.Append("</" + cellName + ">");
                                        }
                                        else
                                        {
                                            strBldr.Append("<" + cellName + " aid:table=\"cell\" aid:theader=\"\" aid:crows=\"1\" aid:ccols=\"" + mColCnt.ToString() + "\">");
                                            strBldr.Append("<![CDATA[" + pivotTable.Rows[i][j].ToString() + "]]>");
                                            strBldr.Append("</" + cellName + ">");
                                            hmCount = mColCnt;
                                        }
                                    }
                                }
                                else
                                {
                                    if (pivotTable.Rows[i][j].ToString() != string.Empty)
                                    {
                                        chkFile = new FileInfo(pivotTable.Rows[i][j].ToString());
                                    }

                                    if (chkFile != null)
                                    {
                                        if (chkFile.Extension.ToUpper() == ".BMP" || chkFile.Extension.ToUpper() == ".JPG" || chkFile.Extension.ToUpper() == ".GIF" || chkFile.Extension.ToUpper() == ".EPS")
                                        {
                                            ImageType = true;
                                        }
                                    }
                                    if (ImageType == true)
                                    {
                                        strBldr.Append("<" + cellName + " aid:table=\"cell\" aid:theader=\"\" aid:crows=\"1\"   aid:ccols=\"1\"  TBGUID=\"TP" + RandomClass.Next() + "\">");
                                        strBldr.Append("<image_name COTYPE=\"IMAGE\"  TBGUID=\"CI" + RandomClass.Next() + "\"  IMAGE_FILE=\"" + pivotTable.Rows[i][j].ToString() + "\"></image_name>");
                                        strBldr.Append("</" + cellName + ">");
                                    }
                                    else
                                    {

                                        strBldr.Append("<" + cellName + " aid:table=\"cell\" aid:theader=\"\" aid:crows=\"1\" aid:ccols=\"1\">");
                                        strBldr.Append("<![CDATA[" + pivotTable.Rows[i][j].ToString() + "]]>");
                                        strBldr.Append("</" + cellName + ">");
                                    }
                                }
                            }
                        }

                        //For Row and Summary Fields -- Vertical Merging
                        else if ((i > columnTable.Columns.Count && (m_showSummaryHeaders == true || m_showRowHeaders == true)) || (i >= columnTable.Columns.Count && m_showSummaryHeaders == false && m_showRowHeaders == false))
                        {
                            string cellName = "";
                            if (pivotTable.Columns[j].ExtendedProperties.ContainsKey("ATTRIBUTE_STYLE"))
                            {
                                cellName = pivotTable.Columns[j].ExtendedProperties["ATTRIBUTE_STYLE"].ToString();
                            }
                            if (cellName == string.Empty)
                            {
                                cellName = "Cell";
                            }

                            if (Convert.ToInt32(pivotTable.Columns[j].ExtendedProperties["MergeCounter"].ToString()) == 0)
                            {
                                int mRowCnt = GetVerticalMergeCells(j, i);
                                if (mRowCnt > 1 && m_mergeSummaryFields == true)
                                {
                                    if (pivotTable.Rows[i][j].ToString() != string.Empty)
                                    {
                                        chkFile = new FileInfo(pivotTable.Rows[i][j].ToString());
                                    }
                                    if (chkFile != null)
                                    {
                                        if (chkFile.Extension.ToUpper() == ".BMP" || chkFile.Extension.ToUpper() == ".JPG" || chkFile.Extension.ToUpper() == ".GIF" || chkFile.Extension.ToUpper() == ".EPS")
                                        {
                                            ImageType = true;
                                        }
                                        if (ImageType == true)
                                        {
                                            strBldr.Append("<" + cellName + " aid:table=\"cell\" aid:crows=\"" + mRowCnt.ToString() + "\" aid:ccols=\"" + "1" + "\"" + "TBGUID=\"TP" + RandomClass.Next() + "\">");
                                            strBldr.Append("<image_name COTYPE=\"IMAGE\"  TBGUID=\"CI" + RandomClass.Next() + "\"  IMAGE_FILE=\"" + pivotTable.Rows[i][j].ToString() + "\"></image_name>");
                                            strBldr.Append("</" + cellName + ">");
                                            pivotTable.Columns[j].ExtendedProperties["MergeCounter"] = mRowCnt.ToString();
                                        }
                                        else
                                        {
                                            strBldr.Append("<" + cellName + " aid:table=\"cell\" aid:crows=\" " + mRowCnt.ToString() + " \" aid:ccols=\"1\">");
                                            strBldr.Append("<![CDATA[" + pivotTable.Rows[i][j].ToString() + "]]>");
                                            strBldr.Append("</" + cellName + ">");
                                            pivotTable.Columns[j].ExtendedProperties["MergeCounter"] = mRowCnt.ToString();
                                        }
                                    }
                                }
                                else
                                {
                                    if (pivotTable.Rows[i][j].ToString() != string.Empty)
                                    {
                                        chkFile = new FileInfo(pivotTable.Rows[i][j].ToString());
                                    }
                                    if (chkFile != null)
                                    {
                                        if (chkFile.Extension.ToUpper() == ".BMP" || chkFile.Extension.ToUpper() == ".JPG" || chkFile.Extension.ToUpper() == ".GIF" || chkFile.Extension.ToUpper() == ".EPS")
                                        {
                                            ImageType = true;
                                        }
                                        if (ImageType == true)
                                        {
                                            strBldr.Append("<" + cellName + " aid:table=\"cell\" aid:crows=\"" + mRowCnt.ToString() + "\" aid:ccols=\"" + "1" + "\"" + " TBGUID=\"TP" + RandomClass.Next() + "\">");
                                            strBldr.Append("<image_name COTYPE=\"IMAGE\"  TBGUID=\"CI" + RandomClass.Next() + "\"  IMAGE_FILE=\"" + pivotTable.Rows[i][j].ToString() + "\"></image_name>");
                                            strBldr.Append("</" + cellName + ">");

                                        }
                                        else
                                        {
                                            strBldr.Append("<" + cellName + " aid:table=\"cell\" aid:crows=\" 1 \" aid:ccols=\"1\">");
                                            strBldr.Append("<![CDATA[" + pivotTable.Rows[i][j].ToString() + "]]>");
                                            strBldr.Append("</" + cellName + ">");
                                        }
                                    }
                                    else
                                    {
                                        strBldr.Append("<" + cellName + " aid:table=\"cell\" aid:crows=\" 1 \" aid:ccols=\"1\">");
                                        strBldr.Append("<![CDATA[" + pivotTable.Rows[i][j].ToString() + "]]>");
                                        strBldr.Append("</" + cellName + ">");
                                    }
                                }
                            }
                        }
                        else if (i == columnTable.Columns.Count)//Row Headers and Summary Headers
                        {
                            if (m_showSummaryHeaders == true || m_showRowHeaders == true)
                            {
                                string cellName = "Cell_Header";
                                if (pivotTable.Columns[j].ExtendedProperties.ContainsKey("ATTRIBUTE_STYLE"))
                                {
                                    cellName = pivotTable.Columns[j].ExtendedProperties["ATTRIBUTE_STYLE"].ToString();
                                }
                                if (cellName == string.Empty)
                                {
                                    cellName = "Cell_Header";
                                }
                                else
                                {
                                    cellName = cellName + "_Header";
                                }
                                if (pivotTable.Rows[i][j].ToString() != string.Empty)
                                {
                                    chkFile = new FileInfo(pivotTable.Rows[i][j].ToString());
                                }
                                if (chkFile != null)
                                {
                                    if (chkFile.Extension.ToUpper() == ".BMP" || chkFile.Extension.ToUpper() == ".JPG" || chkFile.Extension.ToUpper() == ".GIF" || chkFile.Extension.ToUpper() == ".EPS")
                                    {
                                        ImageType = true;
                                    }
                                    if (ImageType == true)
                                    {
                                        strBldr.Append("<" + cellName + " aid:table=\"cell\"  aid:theader=\"\" aid:crows=\"1\"  aid:ccols=\"" + "1" + "\"" + " TBGUID=\"TP" + RandomClass.Next() + "\">");
                                        strBldr.Append("<image_name COTYPE=\"IMAGE\"  TBGUID=\"CI" + RandomClass.Next() + "\"  IMAGE_FILE=\"" + pivotTable.Rows[i][j].ToString() + "\"></image_name>");
                                        strBldr.Append("</" + cellName + ">");
                                    }
                                    else
                                    {

                                        strBldr.Append("<" + cellName + " aid:table=\"cell\" aid:theader=\"\" aid:crows=\" 1 \" aid:ccols=\"1\">");
                                        strBldr.Append("<![CDATA[" + pivotTable.Rows[i][j].ToString() + "]]>");
                                        strBldr.Append("</" + cellName + ">");
                                    }
                                }
                            }
                        }
                    }
                }

                strBldr.Append("</table>");
                strBldr.Append("</products>");
            }
            return strBldr.ToString();
        }

        /// <summary>
        /// This is used to retrieve the Pivot Table Layout  
        /// </summary>
        /// <returns>string</returns>
        /// <example>
        /// <code>
        /// using System;
        /// using System.IO;
        /// using System.Web;
        /// using System.Data;
        /// using TradingBell.Common;
        /// using TradingBell.WebServices;
        /// protected void Page_Load(object sender, EventArgs e)
        /// {
        ///   ...
        ///    string htmlStr = string.Empty;
        //Retrieve the Pivot Layout from Database
        ///    this.RetrievePivotLayout();
        /// }
        /// </code>
        /// </example>
        [WebMethod]
        public string RetrievePivotLayout()
        {
            //newly added
            DataRow[] oDRFamily = oDTFam.Select("FAMILY_ID= " + m_familyID);
            DataTable oFamProdTabStruct = oDTFam.Clone();

            //newly added
            string layoutXML = string.Empty;
            string sqlStr = "SELECT PRODUCT_TABLE_STRUCTURE FROM TB_FAMILY WHERE FAMILY_ID = " + m_familyID + "";
            oHelper.SQLString = sqlStr;
            DataSet ds = oHelper.GetDataSet();
            if (ds.Tables[0].Rows.Count > 0)
            {
                layoutXML = ds.Tables[0].Rows[0]["PRODUCT_TABLE_STRUCTURE"].ToString();
            }
            CheckLayout = layoutXML;
            this.DecodeLayoutXML(layoutXML);
            this.ConvertAttributeIDToAttributeName();
            layoutXML = this.ConstructLayoutXML();

            return layoutXML;
        }

        /// <summary>
        /// This is used to Decode the XML Layout
        /// </summary>
        /// <param name="xmlStr">string</param>
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
        /// using System;
        /// using System.IO;
        /// using System.Web;
        /// using System.Data;
        /// using TradingBell.Common;
        /// using TradingBell.WebServices;
        /// 
        /// protected void Page_Load(object sender, EventArgs e)
        /// {
        ///   string xmlTxt;
        ///   ...
        ///   this.DecodeLayoutXML(xmlTxt);
        /// }
        /// </code>
        /// </example>
        [WebMethod]
        private void DecodeLayoutXML(string xmlStr)
        {
            m_rowFields = new List<string>();
            m_summaryFields = new List<string>();
            m_columnFields = new List<string>();
            m_placeHolderText = "";
            m_summaryGroupField = "";
            m_showColumnHeaders = false;
            m_showRowHeaders = false;
            m_showSummaryHeaders = false;
            m_mergeRowHeaders = false;
            m_mergeSummaryFields = false;

            if (xmlStr.Trim() != string.Empty)
            {
                XmlDocument xmlDOc = new XmlDocument();
                xmlDOc.LoadXml(xmlStr);

                XmlNode rootNode = xmlDOc.DocumentElement;
                XmlNodeList childNodes = rootNode.ChildNodes;
                for (int i = 0; i < childNodes.Count; i++)
                {

                    if (childNodes[i].Name == "RowField")
                        m_rowFields.Add(childNodes[i].InnerText);

                    if (childNodes[i].Name == "ColumnField")
                        m_columnFields.Add(childNodes[i].InnerText);

                    if (childNodes[i].Name == "SummaryField")
                        m_summaryFields.Add(childNodes[i].InnerText);

                    if (childNodes[i].Name == "SummaryGroupField")
                        m_summaryGroupField = childNodes[i].InnerText;

                    if (childNodes[i].Name == "PlaceHolderText")
                        m_placeHolderText = childNodes[i].InnerText;

                    if (childNodes[i].Name == "DisplayRowHeader")
                        m_showRowHeaders = Convert.ToBoolean(childNodes[i].InnerText);

                    if (childNodes[i].Name == "DisplayColumnHeader")
                        m_showColumnHeaders = Convert.ToBoolean(childNodes[i].InnerText);

                    if (childNodes[i].Name == "DisplaySummaryHeader")
                        m_showSummaryHeaders = Convert.ToBoolean(childNodes[i].InnerText);

                    if (childNodes[i].Name == "MergeRowHeader")
                        m_mergeRowHeaders = Convert.ToBoolean(childNodes[i].InnerText);

                    if (childNodes[i].Name == "MergeSummaryFields")
                        m_mergeSummaryFields = Convert.ToBoolean(childNodes[i].InnerText);

                }
            }
        }

        /// <summary>
        /// This is used to Fetch the Source Data 
        /// </summary>
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
        ///    ...
        ///    if (m_rowFields.Count > 0 && m_columnFields.Count > 0)
        ///    {
        ///      this.FetchSourceData();
        ///      this.SetAttributePropertiesForSourceTable();
        ///    }
        /// }
        /// </code>
        /// </example>
        [WebMethod]
        private void FetchSourceData()
        {
            try
            {
                string sqlStr = "EXEC STP_CATALOGSTUDIO5_ProductTable '" + Convert.ToInt32(m_catalogID) + "','" + Convert.ToInt32(m_familyID) + "','FORPIVOT' ,'" + ConstructRowFields() + "', '" + ConstructColumnFields() + "', 0 ";
                oHelper.SQLString = sqlStr;
                DataSet ds = oHelper.GetDataSet();
                sourceTable = ds.Tables[0];
                attributeTable = ds.Tables[1];
                rowTable = ds.Tables[6];
                columnTable = ds.Tables[7];
                rowTable.Columns.Remove("SORT_ORDER");
                columnTable.Columns.Remove("SORT_ORDER");
                //DataTable oDTProdData = new DataTable();
                //oDTProdData = ProductTableData();
                //foreach (DataColumn oDC in oDTProdData.Columns)
                //{
                //    if (oDC.ColumnName.ToUpper().ToString() != "PRODUCT_ID")
                //    {
                //        oDTProdData.Columns.Remove(oDC.ColumnName.ToString());
                //    }
                //}
            }
            catch //(Exception ex)
            {
                sourceTable = new DataTable();
                rowTable = new DataTable();
                columnTable = new DataTable();
                attributeTable = new DataTable();
            }
        }

        /// <summary>
        /// This is used to set the Attribute Properties
        /// </summary>
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
        ///    ...
        ///    if (m_rowFields.Count > 0 && m_columnFields.Count > 0)
        ///    {
        ///      this.FetchSourceData();
        ///      this.SetAttributePropertiesForSourceTable();
        ///    }
        /// }
        /// </code>
        /// </example>
        [WebMethod]
        private void SetAttributePropertiesForSourceTable()
        {
            for (int i = 0; i < attributeTable.Columns.Count; i++)
            {
                for (int j = 0; j < sourceTable.Columns.Count; j++)
                {
                    if (attributeTable.Columns[i].ColumnName == sourceTable.Columns[j].ColumnName)
                    {
                        sourceTable.Columns[j].ExtendedProperties.Add("ATTRIBUTE_ID", attributeTable.Rows[0][i].ToString());
                        sourceTable.Columns[j].ExtendedProperties.Add("ATTRIBUTE_TYPE", attributeTable.Rows[1][i].ToString());
                        sourceTable.Columns[j].ExtendedProperties.Add("ATTRIBUTE_STYLE", attributeTable.Rows[2][i].ToString());
                    }
                }
            }

            for (int i = 0; i < attributeTable.Columns.Count; i++)
            {
                for (int j = 0; j < columnTable.Columns.Count; j++)
                {
                    if (attributeTable.Columns[i].ColumnName == columnTable.Columns[j].ColumnName)
                    {
                        columnTable.Columns[j].ExtendedProperties.Add("ATTRIBUTE_ID", attributeTable.Rows[0][i].ToString());
                        columnTable.Columns[j].ExtendedProperties.Add("ATTRIBUTE_TYPE", attributeTable.Rows[1][i].ToString());
                        columnTable.Columns[j].ExtendedProperties.Add("ATTRIBUTE_STYLE", attributeTable.Rows[2][i].ToString());
                    }
                }
            }

            for (int i = 0; i < attributeTable.Columns.Count; i++)
            {
                for (int j = 0; j < rowTable.Columns.Count; j++)
                {
                    if (attributeTable.Columns[i].ColumnName == rowTable.Columns[j].ColumnName)
                    {
                        rowTable.Columns[j].ExtendedProperties.Add("ATTRIBUTE_ID", attributeTable.Rows[0][i].ToString());
                        rowTable.Columns[j].ExtendedProperties.Add("ATTRIBUTE_TYPE", attributeTable.Rows[1][i].ToString());
                        rowTable.Columns[j].ExtendedProperties.Add("ATTRIBUTE_STYLE", attributeTable.Rows[2][i].ToString());
                    }
                }
            }
        }

        /// <summary>
        /// This is used to Get the Currency Symbol
        /// </summary>
        /// <param name="AttrID">string</param>
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
        ///    ...
        ///    if(q = 0)
        ///    {
        ///      this.GetCurrencySymbol(columnTable.Columns[j].ExtendedProperties["ATTRIBUTE_ID"].ToString());
        ///    }
        /// }
        /// </code>
        /// </example>
        [WebMethod]
        private void GetCurrencySymbol(string AttrID)
        {
            //newly added
            DataSet oDS = new DataSet();
            DataRow[] oDRAttr = oDTAttr.Select("ATTRIBUTE_ID = " + AttrID);
            DataTable oDTAttrDataRule = oDTAttr.Clone();
            foreach (DataRow oDR in oDRAttr)
            {
                oDTAttrDataRule.ImportRow(oDR);
            }
            //newly added
            DataSet dscURRENCY = new DataSet();
            string XMLstr = string.Empty;
            //oHelper.SQLString = "SELECT ATTRIBUTE_DATARULE FROM TB_ATTRIBUTE WHERE ATTRIBUTE_ID = " + AttrID + "";
            //dscURRENCY = oHelper.GetDataSet();
            dscURRENCY.Tables.Add(oDTAttrDataRule);
            Prefix = string.Empty; Suffix = string.Empty; EmptyCondition = string.Empty; ReplaceText = string.Empty; Headeroptions = string.Empty;
            if (dscURRENCY.Tables[0].Rows.Count > 0)
            {
                if (dscURRENCY.Tables[0].Rows[0]["ATTRIBUTE_DATARULE"].ToString() != string.Empty)
                {
                    XMLstr = dscURRENCY.Tables[0].Rows[0]["ATTRIBUTE_DATARULE"].ToString();
                    XmlDocument xmlDOc = new XmlDocument();
                    xmlDOc.LoadXml(XMLstr);
                    XmlNode rootNode = xmlDOc.DocumentElement;
                    {
                        XmlNodeList xmlNodeList;
                        xmlNodeList = rootNode.ChildNodes;

                        for (int xmlNode = 0; xmlNode < xmlNodeList.Count; xmlNode++)
                        {
                            if (xmlNodeList[xmlNode].ChildNodes.Count > 0)
                            {
                                if (xmlNodeList[xmlNode].ChildNodes[0].LastChild != null)
                                {
                                    Prefix = xmlNodeList[xmlNode].ChildNodes[0].LastChild.Value;
                                }
                                if (xmlNodeList[xmlNode].ChildNodes[1].LastChild != null)
                                {
                                    Suffix = xmlNodeList[xmlNode].ChildNodes[1].LastChild.Value;
                                }
                                if (xmlNodeList[xmlNode].ChildNodes[2].LastChild != null)
                                {
                                    EmptyCondition = xmlNodeList[xmlNode].ChildNodes[2].LastChild.Value;
                                }
                                if (xmlNodeList[xmlNode].ChildNodes[3].LastChild != null)
                                {
                                    ReplaceText = xmlNodeList[xmlNode].ChildNodes[3].LastChild.Value;
                                }
                                if (xmlNodeList[xmlNode].ChildNodes[4].LastChild != null)
                                {
                                    Headeroptions = xmlNodeList[xmlNode].ChildNodes[4].LastChild.Value;
                                }

                            }
                        }
                    }



                }
            }
        }

        /// <summary>
        /// This is used to construct the XML Layout
        /// </summary>
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
        ///    ...
        ///    this.ConvertAttributeNameToAttributeID();
        ///    string xmlString = this.ConstructLayoutXML();
        /// }
        /// </code>
        /// </example>
        [WebMethod]
        private string ConstructLayoutXML()
        {
            StringBuilder xmlTxtBuilder = new StringBuilder();
            XmlDocument xmlDoc = new XmlDocument();
            MemoryStream mStream = new MemoryStream();
            XmlTextWriter writer = new XmlTextWriter(mStream, Encoding.UTF8);

            //Causes child elements to be indented
            writer.Formatting = Formatting.Indented;

            // Report element
            xmlTxtBuilder.Append("<?xml version=\"1.0\" encoding=\"utf-8\"?>");
            xmlTxtBuilder.Append("<TradingBell TableType=\"Pivot\">");

            for (int i = 0; i < m_rowFields.Count; i++)
            {
                xmlTxtBuilder.Append("<RowField>");
                xmlTxtBuilder.Append(m_rowFields[i]);
                xmlTxtBuilder.Append("</RowField>");
            }

            for (int i = 0; i < m_columnFields.Count; i++)
            {
                xmlTxtBuilder.Append("<ColumnField>");
                xmlTxtBuilder.Append(m_columnFields[i]);
                xmlTxtBuilder.Append("</ColumnField>");
            }

            for (int i = 0; i < m_summaryFields.Count; i++)
            {
                xmlTxtBuilder.Append("<SummaryField>");
                xmlTxtBuilder.Append(m_summaryFields[i]);
                xmlTxtBuilder.Append("</SummaryField>");
            }

            xmlTxtBuilder.Append("<SummaryGroupField>");

            xmlTxtBuilder.Append(m_summaryGroupField);

            xmlTxtBuilder.Append("</SummaryGroupField>");

            xmlTxtBuilder.Append("<PlaceHolderText>");

            xmlTxtBuilder.Append(m_placeHolderText.ToString());

            xmlTxtBuilder.Append("</PlaceHolderText>");

            xmlTxtBuilder.Append("<DisplayRowHeader>");

            xmlTxtBuilder.Append(m_showRowHeaders.ToString());

            xmlTxtBuilder.Append("</DisplayRowHeader>");

            xmlTxtBuilder.Append("<DisplayColumnHeader>");

            xmlTxtBuilder.Append(m_showColumnHeaders.ToString());

            xmlTxtBuilder.Append("</DisplayColumnHeader>");

            xmlTxtBuilder.Append("<DisplaySummaryHeader>");

            xmlTxtBuilder.Append(m_showSummaryHeaders.ToString());

            xmlTxtBuilder.Append("</DisplaySummaryHeader>");

            xmlTxtBuilder.Append("<MergeRowHeader>");

            xmlTxtBuilder.Append(m_mergeRowHeaders.ToString());

            xmlTxtBuilder.Append("</MergeRowHeader>");

            xmlTxtBuilder.Append("<MergeSummaryFields>");

            xmlTxtBuilder.Append(m_mergeSummaryFields.ToString());

            xmlTxtBuilder.Append("</MergeSummaryFields>");

            xmlTxtBuilder.Append("</TradingBell>");

            //Flush the writer and close the stream
            return xmlTxtBuilder.ToString();
        }

        /// <summary>
        /// This is used to retrieve the Source Table
        /// </summary>
        /// <returns>DataTable</returns>
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
        ///    ...
        ///    int savePivot;
        ///    DataTablt sourceTbl;
        ///    ...
        ///    sourceTbl = FetchSourceTable();
        /// }
        /// </code>
        /// </example>
        [WebMethod]
        public DataTable FetchSourceTable()
        {
            DataTable srcTable = new DataTable();
            try
            {
                string sqlStr = "EXEC STP_CATALOGSTUDIO5_ProductTable '" + Convert.ToInt32(m_catalogID) + "','" + Convert.ToInt32(m_familyID) + "','FORPIVOT',' ',' ' , 0 ";
                oHelper.SQLString = sqlStr;
                DataSet ds = oHelper.GetDataSet();
                if (ds.Tables.Count > 0 && ds.Tables.Count == 6)
                {
                    srcTable = ds.Tables[0];
                }
            }
            catch { } //(Exception ex) { }
            return srcTable;
        }

        /// <summary>
        /// This is used to Save Pivot Layout
        /// </summary>
        /// <param name="xmlTxt">string</param>
        /// <returns>int</returns>
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
        ///    int savePivot;
        ///    string xmlTxt;
        ///    ...
        ///    savePivot = SavePivotLayout(xmlTxt);
        /// }
        /// </code>
        /// </example>
        [WebMethod]
        public int SavePivotLayout(string xmlTxt)
        {
            this.DecodeLayoutXML(xmlTxt);
            this.ConvertAttributeNameToAttributeID();
            string xmlString = this.ConstructLayoutXML();
            string sqlStr = "UPDATE TB_FAMILY SET PRODUCT_TABLE_STRUCTURE = N'" + xmlString.Replace("'", "''") + "' WHERE FAMILY_ID = " + m_familyID + "";
            oHelper.SQLString = sqlStr;
            return oHelper.ExecuteSQLQuery();
        }

        /// <summary>
        /// This is used to Generate the HTML Layout for Pivot Table
        /// </summary>
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
        ///    ...
        ///    string xmlStr;
        ///   ...
        ///   if (m_rowFields.Count > 0 && m_columnFields.Count > 0)
        ///   {
        ///      this.ConstructPivotTable();
        ///      xmlStr = this.GeneratePivotXML();
        ///   }   
        /// }
        /// </code>
        /// </example>
        [WebMethod]
        public string GeneratePivotHTML()
        {
            string htmlStr = string.Empty;
            //Retrieve the Pivot Layout from Database
            this.DecodeLayoutXML(this.RetrievePivotLayout());
            if (m_rowFields.Count > 0 && m_columnFields.Count > 0)
            {
                this.FetchSourceData();
                this.SetAttributePropertiesForSourceTable();
                this.ConstructPivotTable();
                htmlStr = this.GenerateHTML();
            }

            if (htmlStr.Trim() == string.Empty)
            {
                htmlStr = "<P style=\"font-size: x-medium; color: red; font-family: Verdana;\"> INVALID LAYOUT SPECIFICATION ! </P>";
            }
            return htmlStr;
        }

        /// <summary>
        /// This is used to Build the Row Fields
        /// </summary>
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
        ///    
        /// }
        /// </code>
        /// </example>
        [WebMethod]
        private string ConstructRowFields()
        {
            string temp = string.Empty;
            //Populate ROwList with AttributeNames
            for (int i = 0; i < m_rowFields.Count; i++)
            {
                if (temp.Length > 0)
                {
                    temp = temp + "," + m_rowFields[i];
                }
                else
                {
                    temp = m_rowFields[i];
                }
            }

            return temp;
        }

        /// <summary>
        /// This is used to Convert Attribute Name to Attribute ID
        /// </summary>
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
        ///    
        /// }
        /// </code>
        /// </example>
        [WebMethod]
        private void ConvertAttributeNameToAttributeID()
        {
            try
            {
                DataSet ds = new DataSet();
                for (int i = 0; i < m_rowFields.Count; i++)
                {
                    //sqlStr = "SELECT ATTRIBUTE_ID FROM TB_ATTRIBUTE WHERE ATTRIBUTE_NAME = N'" + m_rowFields[i].ToString() + "' ";
                    //oHelper.SQLString = sqlStr;
                    //ds = oHelper.GetDataSet();
                    DataRow[] oDRAttrID = oDTAttr.Select("ATTRIBUTE_NAME= '" + m_rowFields[i].ToString() + "'");
                    DataTable oDTAttrID = oDTAttr.Clone();
                    foreach (DataRow oDR in oDRAttrID)
                    {
                        oDTAttrID.ImportRow(oDR);
                    }
                    ds.Tables.Clear();
                    ds.Tables.Add(oDTAttrID);
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        m_rowFields[i] = ds.Tables[0].Rows[0]["ATTRIBUTE_ID"].ToString();
                    }

                }

                for (int i = 0; i < m_columnFields.Count; i++)
                {
                    //sqlStr = "SELECT ATTRIBUTE_ID FROM TB_ATTRIBUTE WHERE ATTRIBUTE_NAME = N'" + m_columnFields[i].ToString() + "' ";
                    //oHelper.SQLString = sqlStr;
                    //ds = oHelper.GetDataSet();
                    DataRow[] oDRAttrID = oDTAttr.Select("ATTRIBUTE_NAME= '" + m_columnFields[i].ToString() + "'");
                    DataTable oDTAttrID = oDTAttr.Clone();
                    foreach (DataRow oDR in oDRAttrID)
                    {
                        oDTAttrID.ImportRow(oDR);
                    }
                    ds.Tables.Clear();
                    ds.Tables.Add(oDTAttrID);
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        m_columnFields[i] = ds.Tables[0].Rows[0]["ATTRIBUTE_ID"].ToString();
                    }
                }

                for (int i = 0; i < m_summaryFields.Count; i++)
                {
                    //sqlStr = "SELECT ATTRIBUTE_ID FROM TB_ATTRIBUTE WHERE ATTRIBUTE_NAME =N'" + m_summaryFields[i].ToString() + "' ";
                    //oHelper.SQLString = sqlStr;
                    //ds = oHelper.GetDataSet();
                    DataRow[] oDRAttrID = oDTAttr.Select("ATTRIBUTE_NAME= '" + m_summaryFields[i].ToString() + "'");
                    DataTable oDTAttrID = oDTAttr.Clone();
                    foreach (DataRow oDR in oDRAttrID)
                    {
                        oDTAttrID.ImportRow(oDR);
                    }
                    ds.Tables.Clear();
                    ds.Tables.Add(oDTAttrID);

                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        m_summaryFields[i] = ds.Tables[0].Rows[0]["ATTRIBUTE_ID"].ToString();
                    }
                }
            }

            catch //(Exception ex)
            {
                m_rowFields.Clear();
                m_columnFields.Clear();
                m_summaryFields.Clear();
            }
        }

        /// <summary>
        /// This is used to convert AttributeID to Attribute Name
        /// </summary>
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
        ///    
        /// }
        /// </code>
        /// </example>
        [WebMethod]
        private void ConvertAttributeIDToAttributeName()
        {
            try
            {
                DataSet ds = new DataSet();
                string sqlStr = string.Empty;
                for (int i = 0; i < m_rowFields.Count; i++)
                {
                    DataRow[] oDRAttrName = oDTAttr.Select("ATTRIBUTE_ID= " + m_rowFields[i].ToString());
                    DataTable oDTAttrName = oDTAttr.Clone();
                    foreach (DataRow oDR in oDRAttrName)
                    {
                        oDTAttrName.ImportRow(oDR);
                    }
                    ds.Tables.Clear();
                    ds.Tables.Add(oDTAttrName);
                    sqlStr = "SELECT ATTRIBUTE_NAME FROM TB_ATTRIBUTE WHERE ATTRIBUTE_ID  = " + m_rowFields[i].ToString() + " ";
                    oHelper.SQLString = sqlStr;
                    ds = oHelper.GetDataSet();
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        m_rowFields[i] = ds.Tables[0].Rows[0]["ATTRIBUTE_NAME"].ToString();
                    }
                }

                for (int i = 0; i < m_columnFields.Count; i++)
                {
                    DataRow[] oDRAttrName = oDTAttr.Select("ATTRIBUTE_ID= " + m_columnFields[i].ToString());
                    DataTable oDTAttrName = oDTAttr.Clone();
                    foreach (DataRow oDR in oDRAttrName)
                    {
                        oDTAttrName.ImportRow(oDR);
                    }
                    ds.Tables.Clear();
                    ds.Tables.Add(oDTAttrName);
                    sqlStr = "SELECT ATTRIBUTE_NAME FROM TB_ATTRIBUTE WHERE ATTRIBUTE_ID  = " + m_columnFields[i].ToString() + " ";
                    oHelper.SQLString = sqlStr;
                    ds = oHelper.GetDataSet();
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        m_columnFields[i] = ds.Tables[0].Rows[0]["ATTRIBUTE_NAME"].ToString();
                    }
                }

                for (int i = 0; i < m_summaryFields.Count; i++)
                {
                    DataRow[] oDRAttrName = oDTAttr.Select("ATTRIBUTE_ID= " + m_summaryFields[i].ToString());
                    DataTable oDTAttrName = oDTAttr.Clone();
                    foreach (DataRow oDR in oDRAttrName)
                    {
                        oDTAttrName.ImportRow(oDR);
                    }
                    ds.Tables.Clear();
                    ds.Tables.Add(oDTAttrName);
                    sqlStr = "SELECT ATTRIBUTE_NAME FROM TB_ATTRIBUTE WHERE ATTRIBUTE_ID = " + m_summaryFields[i].ToString() + " ";
                    oHelper.SQLString = sqlStr;
                    ds = oHelper.GetDataSet();
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        m_summaryFields[i] = ds.Tables[0].Rows[0]["ATTRIBUTE_NAME"].ToString();
                    }
                }
            }

            catch //(Exception ex)
            {
                m_rowFields.Clear();
                m_summaryFields.Clear();
                m_columnFields.Clear();
            }
        }

        /// <summary>
        /// This is used to Build the Column Fields
        /// </summary>
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
        ///    
        /// }
        /// </code>
        /// </example>
        [WebMethod]
        private string ConstructColumnFields()
        {
            string temp = string.Empty;

            //Populate ColumnList with AttributeNames
            for (int i = 0; i < m_columnFields.Count; i++)
            {
                if (temp.Length > 0)
                {
                    temp = temp + "," + m_columnFields[i];
                }
                else
                {
                    temp = m_columnFields[i];
                }
            }

            return temp;
        }
        //<IMG SRC = "C:\spisepl2.jpg" HEIGHT=200 WIDTH = 146>

        /// <summary>
        /// This is used to Generate HTML code
        /// </summary>
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
        ///    
        /// }
        /// </code>
        /// </example>
        [WebMethod]
        private string GenerateHTML()
        {
            StringBuilder strBldr = new StringBuilder();
            int hmCount = 0;
            int Min_ord_qty = 0;
            int Qty_avail = 0;
            string NavColumn = oHelper.GetOptionValues("NAVIGATIONCOLUMN").ToString();
            string HypCURL = oHelper.GetOptionValues("NAVIGATIONURL").ToString();
            string EComState = oHelper.GetOptionValues("ECOMMERCEENABLED").ToString();
            if (pivotTable.Rows.Count > 0 && rowTable.Rows.Count > 0 && columnTable.Rows.Count > 0)
            {
                //Initialize the Extended properties for use in vertical merging
                for (int j = 0; j < pivotTable.Columns.Count; j++)
                {
                    pivotTable.Columns[j].ExtendedProperties.Clear();
                    pivotTable.Columns[j].ExtendedProperties.Add("MergeCounter", "0");
                }
                strBldr.Append("<table width=\"100%\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\"><tr> <td align=\"right\" ><TABLE width=\"99%\" style=\"font-size:12px; border-left-color: black; border-bottom-color: black;");
                strBldr.Append("margin: .1pt; color: black; border-top-style: outset; border-top-color: black;");
                strBldr.Append("font-family: Arial Unicode ms; border-right-style: outset; border-left-style: outset;");
                strBldr.Append("background-color: white; font-variant: normal; border-right-color: black;");
                strBldr.Append("border-bottom-style: outset\">");
                bool IsCartHeaderAdder = false;
                for (int i = 0; i < pivotTable.Rows.Count; i++)
                {
                    strBldr.Append(System.Environment.NewLine + "<TR>" + System.Environment.NewLine);
                    for (int j = 0; j < pivotTable.Columns.Count; j++)
                    {
                        if (i > columnTable.Columns.Count)
                        {
                            ProdID = oHelper.CI(sourceTable.Rows[i - (columnTable.Columns.Count + 1)]["PRODUCT_ID"].ToString());
                        }
                        if (Convert.ToInt32(pivotTable.Columns[j].ExtendedProperties["MergeCounter"].ToString()) > 0)
                        {
                            int temp = Convert.ToInt32(pivotTable.Columns[j].ExtendedProperties["MergeCounter"].ToString());
                            temp = temp - 1;
                            pivotTable.Columns[j].ExtendedProperties["MergeCounter"] = temp.ToString();
                        }

                        if (hmCount > 0) { hmCount = hmCount - 1; }

                        //For Column Header and Field -- Horizondal Merging
                        if (i < columnTable.Columns.Count)
                        {
                            if (hmCount == 0)
                            {
                                int mColCnt = GetHorizontalMergeCells(j, i);
                                if (mColCnt > 1)
                                {
                                    if (i < columnTable.Columns.Count && j < rowTable.Columns.Count) //Column Headers
                                        strBldr.Append(System.Environment.NewLine + "<TD ALIGN=\"Center\" VALIGN=\"Middle\"  style=\"width: 200px; color: white; BACKGROUND-COLOR: #DF7637 \" colspan=\"");
                                    else if (i < columnTable.Columns.Count && j >= rowTable.Columns.Count) //Column Fields
                                        strBldr.Append(System.Environment.NewLine + "<TD ALIGN=\"Center\" VALIGN=\"Middle\"  style=\"width: 200px; color: Black; BACKGROUND-COLOR: #FAD27D   \" colspan=\"");

                                    strBldr.Append(mColCnt.ToString());
                                    strBldr.Append("\">" + System.Environment.NewLine);

                                    if ((ConstructImagePath(pivotTable.Rows[i][j].ToString().Trim()) != string.Empty))
                                    {
                                        strBldr.Append(ConstructImagePath(pivotTable.Rows[i][j].ToString()));
                                    }
                                    else
                                    {
                                        strBldr.Append(pivotTable.Rows[i][j].ToString().Replace("\r", "<br/>").Replace("\n", "&nbsp;"));
                                    }

                                    strBldr.Append(System.Environment.NewLine + "</TD>" + System.Environment.NewLine);
                                    hmCount = mColCnt;
                                }
                                else
                                {
                                    if (i < columnTable.Columns.Count && j < rowTable.Columns.Count) //Column Headers
                                        strBldr.Append(System.Environment.NewLine + "<TD ALIGN=\"Center\" VALIGN=\"Middle\"  style=\"width: 200px; color: white; BACKGROUND-COLOR: #DF7637\" >" + System.Environment.NewLine);
                                    else if (i < columnTable.Columns.Count && j >= rowTable.Columns.Count) //Column Fields
                                        strBldr.Append(System.Environment.NewLine + "<TD ALIGN=\"Center\" VALIGN=\"Middle\"  style=\"width: 200px; color: Black; BACKGROUND-COLOR: #FAD27D \" >" + System.Environment.NewLine);

                                    if ((ConstructImagePath(pivotTable.Rows[i][j].ToString().Trim()) != string.Empty))
                                    {
                                        strBldr.Append(ConstructImagePath(pivotTable.Rows[i][j].ToString()));
                                    }
                                    else
                                    {
                                        strBldr.Append(pivotTable.Rows[i][j].ToString().Replace("\r", "<br/>").Replace("\n", "&nbsp;"));
                                    }
                                    strBldr.Append(System.Environment.NewLine + "</TD>" + System.Environment.NewLine);
                                }
                                if (j >= pivotTable.Columns.Count - 1 && EComState.ToUpper() == "YES")
                                {
                                    if (m_showRowHeaders == false && IsCartHeaderAdder == false)
                                    {
                                        if (IsVisibleShipping == true)
                                        {
                                            strBldr.Append("<TD ALIGN=\"Center\" VALIGN=\"Middle\"  style=\"width: 200px; color: Black; BACKGROUND-COLOR: #FAD27D\">More Info</TD>");
                                        }
                                        strBldr.Append("<TD ALIGN=\"Center\" VALIGN=\"Middle\"  style=\"width: 200px; color: Black; BACKGROUND-COLOR: #FAD27D\">Cart</TD>");
                                        IsCartHeaderAdder = true;
                                    }
                                    else
                                    {
                                        strBldr.Append("<TD ALIGN=\"Center\" VALIGN=\"Middle\"  style=\"width: 200px; color: Black; BACKGROUND-COLOR: #FAD27D\" colspan=\"2\">");
                                        strBldr.Append("</TD>");
                                    }
                                }
                            }
                        }
                        //For Row and Summary Fields -- Vertical Merging
                        else if ((i > columnTable.Columns.Count && (m_showSummaryHeaders == true || m_showRowHeaders == true)) || (i >= columnTable.Columns.Count && m_showSummaryHeaders == false && m_showRowHeaders == false))
                        {
                            if (Convert.ToInt32(pivotTable.Columns[j].ExtendedProperties["MergeCounter"].ToString()) == 0)
                            {
                                int mRowCnt = GetVerticalMergeCells(j, i);

                                if ((mRowCnt > 1 && m_mergeSummaryFields == true && j >= rowTable.Columns.Count) || (mRowCnt > 1 && m_mergeRowHeaders == true && j < rowTable.Columns.Count) || (mRowCnt > 1 && m_mergeRowHeaders == true && m_mergeSummaryFields == true))
                                {
                                    if (i >= columnTable.Columns.Count && j < rowTable.Columns.Count) //Row Fields
                                        strBldr.Append(System.Environment.NewLine + "<TD ALIGN=\"Center\" VALIGN=\"Middle\"  style=\"width: 200px; color: #032F61; BACKGROUND-COLOR: #A9CDF2 \" rowspan=\"");
                                    else
                                        strBldr.Append(System.Environment.NewLine + "<TD ALIGN=\"Center\" VALIGN=\"Middle\"  style=\"width: 200px; BACKGROUND-COLOR: #F2C2CB   \" rowspan=\"");

                                    strBldr.Append(mRowCnt.ToString());
                                    strBldr.Append("\">" + System.Environment.NewLine);

                                    if ((ConstructImagePath(pivotTable.Rows[i][j].ToString().Trim()) != string.Empty))
                                    {
                                        strBldr.Append(ConstructImagePath(pivotTable.Rows[i][j].ToString()));
                                    }
                                    else
                                    {
                                        strBldr.Append(pivotTable.Rows[i][j].ToString().Replace("\r", "<br/>").Replace("\n", "&nbsp;"));
                                    }

                                    strBldr.Append(System.Environment.NewLine + "</TD>" + System.Environment.NewLine);
                                    pivotTable.Columns[j].ExtendedProperties["MergeCounter"] = mRowCnt.ToString();
                                }
                                else
                                {
                                    if (i >= columnTable.Columns.Count && j < rowTable.Columns.Count) //Row Fields
                                        strBldr.Append(System.Environment.NewLine + "<TD ALIGN=\"Center\" VALIGN=\"Middle\"  style=\"width: 200px; color: #032F61; BACKGROUND-COLOR: #A9CDF2 \" >" + System.Environment.NewLine);
                                    else
                                        strBldr.Append(System.Environment.NewLine + "<TD ALIGN=\"Center\" VALIGN=\"Middle\"  style=\"width: 200px; BACKGROUND-COLOR: #F2C2CB  \" >" + System.Environment.NewLine);

                                    if ((ConstructImagePath(pivotTable.Rows[i][j].ToString().Trim()) != string.Empty))
                                    {
                                        strBldr.Append(ConstructImagePath(pivotTable.Rows[i][j].ToString()));
                                    }
                                    else
                                    {
                                        DataRow oDR = pivotTable.Rows[columnTable.Columns.Count];
                                        int FindNavColumn = 0;
                                        for (int no = 0; no <= (oDR.ItemArray.Length - rowTable.Columns.Count); no++)
                                        {
                                            if (oDR[no].ToString() == NavColumn)
                                            {
                                                FindNavColumn = no;
                                            }
                                        }
                                        if ((NavColumn == pivotTable.Rows[columnTable.Columns.Count][FindNavColumn].ToString()) && (j == FindNavColumn))
                                        {
                                            string NavColValue = "";
                                            string ValueFortag = "";
                                            string HypColumn = "";
                                            HypColumn = HypCURL.Replace("{PRODUCT_ID}", ProdID.ToString());
                                            Min_ord_qty = oHelper.CI(oOrder.GetProductMinimumOrderQty(ProdID));
                                            HypColumn = HypColumn.Replace("{MIN_ORD_QTY}", Min_ord_qty.ToString());
                                            Qty_avail = oHelper.CI(oOrder.GetProductAvilableQty(ProdID));
                                            HypColumn = HypColumn.Replace("{QTY_AVAIL}", Qty_avail.ToString());
                                            HypColumn = HypColumn.Replace("{FAMILY_ID}", this.FamilyID.ToString());
                                            NavColValue = pivotTable.Rows[i][j].ToString().Replace("\r", "<br/>").Replace("\n", "&nbsp;");
                                            ValueFortag = "<A HREF=\"" + HypColumn + "\" > " + NavColValue + "</A>";
                                            //strBldr.Append(pivotTable.Rows[i][j].ToString().Replace("\r", "<br/>").Replace("\n", "&nbsp;"));
                                            strBldr.Append(ValueFortag);
                                        }
                                        else
                                        {
                                            strBldr.Append(pivotTable.Rows[i][j].ToString().Replace("\r", "<br/>").Replace("\n", "&nbsp;"));
                                        }
                                    }

                                    strBldr.Append(System.Environment.NewLine + "</TD>" + System.Environment.NewLine);
                                }
                            }
                        }
                        else if (i == columnTable.Columns.Count)//Row Headers
                        {
                            if (m_showSummaryHeaders == true || m_showRowHeaders == true)
                            {
                                if (i == columnTable.Columns.Count && j < rowTable.Columns.Count) //Row Header
                                {
                                    strBldr.Append(System.Environment.NewLine + "<TD ALIGN=\"Center\" VALIGN=\"Middle\" style=\"width: 200px; color: white; BACKGROUND-COLOR: #3D84CC  \" >" + System.Environment.NewLine);
                                }
                                else if (i == columnTable.Columns.Count && j >= rowTable.Columns.Count) //Summary Header
                                {
                                    strBldr.Append(System.Environment.NewLine + "<TD ALIGN=\"Center\" VALIGN=\"Middle\" style=\"width: 200px; color: white; BACKGROUND-COLOR: #cc6678  \" >" + System.Environment.NewLine);
                                }

                                if ((ConstructImagePath(pivotTable.Rows[i][j].ToString().Trim()) != string.Empty))
                                {
                                    strBldr.Append(ConstructImagePath(pivotTable.Rows[i][j].ToString()));
                                }
                                else
                                {
                                    strBldr.Append(pivotTable.Rows[i][j].ToString().Replace("\r", "<br/>").Replace("\n", "&nbsp;"));
                                }
                                strBldr.Append(System.Environment.NewLine + "</TD>" + System.Environment.NewLine);
                                if (i == columnTable.Columns.Count && j >= rowTable.Columns.Count) //Summary Header
                                {
                                    //if (j >= pivotTable.Columns.Count - rowTable.Columns.Count)
                                    if (j >= pivotTable.Columns.Count - 1 && EComState.ToUpper() == "YES")
                                    {
                                        if (IsVisibleShipping == true)
                                        {
                                            strBldr.Append(System.Environment.NewLine + "<TD ALIGN=\"Center\" VALIGN=\"Middle\" style=\"width: 200px; color: white; BACKGROUND-COLOR: #cc6678  \" >More Info</TD>" + System.Environment.NewLine);
                                        }
                                        strBldr.Append(System.Environment.NewLine + "<TD ALIGN=\"Center\" VALIGN=\"Middle\" style=\"width: 200px; color: white; BACKGROUND-COLOR: #cc6678  \" >Cart</TD>" + System.Environment.NewLine);
                                    }
                                }
                            }
                        }
                        //Add the Shipping and Cart Image
                        //if (j >= pivotTable.Columns.Count - rowTable.Columns.Count)
                        if (j >= pivotTable.Columns.Count - 1 && EComState.ToUpper() == "YES")
                        {
                            int rowCount = columnTable.Columns.Count;
                            int ColCount = rowTable.Columns.Count;
                            //if (m_showRowHeaders == false)
                            //{
                            //    i = i + 1;
                            //}
                            if (m_showRowHeaders == false)
                            {
                                rowCount = rowCount - 1;
                            }
                            if (i > rowCount)
                            {
                                //Add the Shipping Image
                                Boolean IsShipping = oOrder.GetProductIsShipping(ProdID);
                                string ShipImgPath = "";
                                if (IsShipping == true)
                                {
                                    ShipImgPath = oHelper.GetOptionValues("IMAGE PATH").ToString() + oHelper.GetOptionValues("SHIPPING IMAGE").ToString();
                                    string ShipUrl = oHelper.GetOptionValues("SHIP URL").ToString();
                                    ShipImgPath = "<A HREF=\"" + ShipUrl + "\" style=\"text-decoration:none\"><IMG SRC=\"" + ShipImgPath + "\" style=\"border-width:0\"></A>";
                                }
                                else if (IsShipping == false)
                                {
                                    ShipImgPath = oHelper.GetOptionValues("IMAGE PATH").ToString() + oHelper.GetOptionValues("NO SHIPPING IMAGE").ToString();
                                    ShipImgPath = "<IMG SRC=\"" + ShipImgPath + "\" style=\"border-width:0\">";
                                }
                                string CartImgPath = "";
                                if (m_showRowHeaders == true)
                                {
                                    ProdID = oHelper.CI(sourceTable.Rows[i - (columnTable.Columns.Count + 1)]["PRODUCT_ID"].ToString());
                                }
                                else
                                {
                                    ProdID = oHelper.CI(sourceTable.Rows[i - (columnTable.Columns.Count)]["PRODUCT_ID"].ToString());
                                    //ProdID = oHelper.CI(sourceTable.Rows[ColCount - (columnTable.Columns.Count + 1)]["PRODUCT_ID"].ToString());
                                }
                                if (IsVisibleShipping == true)
                                {
                                    if (HttpContext.Current.Request.Url.ToString().ToLower().Contains("family.aspx") == true && HttpContext.Current.Request["sl1"] != null && HttpContext.Current.Request["sl1"].ToString() != "" && HttpContext.Current.Request["sl2"] != null && HttpContext.Current.Request["sl2"].ToString() != "")
                                    {
                                        ShipImgPath = "<a href=\"productdetails.aspx?&pid=" + ProdID.ToString() + "&fid=" + FamilyID.ToString() + "&byp=2&qf=1&cid=" + HttpContext.Current.Request["cid"].ToString() + "&sl1=" + HttpContext.Current.Request["sl1"].ToString() + "&sl2=" + HttpContext.Current.Request["sl2"].ToString() + "&tf=1\"  class=\"tx_3\">" +
                                                  "<img src=\"images/ico_details.gif\" width=\"17\" height=\"14\" border=\"0\" align=\"absmiddle\" /> <br/>Details </a>";

                                    }
                                    else if (HttpContext.Current.Request.Url.ToString().ToLower().Contains("family.aspx") == true && HttpContext.Current.Request["sl1"] != null && HttpContext.Current.Request["sl1"].ToString() != "")
                                    {
                                        ShipImgPath = "<a href=\"productdetails.aspx?&pid=" + ProdID.ToString() + "&fid=" + FamilyID.ToString() + "&byp=2&qf=1&cid=" + HttpContext.Current.Request["cid"].ToString() + "&sl1=" + HttpContext.Current.Request["sl1"].ToString() + "&tf=1\"  class=\"tx_3\">" +
                                                 "<img src=\"images/ico_details.gif\" width=\"17\" height=\"14\" border=\"0\" align=\"absmiddle\" /> <br/>Details </a>";
                                    }
                                    else
                                    {
                                        if (HttpContext.Current.Request["pcr"] != null && HttpContext.Current.Request["pcr"].ToString() != "")
                                        {
                                            ShipImgPath = "<a href=\"productdetails.aspx?&pid=" + ProdID.ToString() + "&fid=" + FamilyID.ToString() + "&pcr=" + HttpContext.Current.Request["pcr"].ToString() + "\"  class=\"tx_3\">" +
                                      "<img src=\"images/ico_details.gif\" width=\"17\" height=\"14\" border=\"0\" align=\"absmiddle\" /> Details </a>";
                                        }
                                        else
                                        {
                                            ShipImgPath = "<a href=\"productdetails.aspx?&pid=" + ProdID.ToString() + "&fid=" + FamilyID.ToString() + "\"  class=\"tx_3\">" +
                                         "<img src=\"images/ico_details.gif\" width=\"17\" height=\"14\" border=\"0\" align=\"absmiddle\" /> Details </a>";
                                        }
                                    }
                                    strBldr.Append(System.Environment.NewLine + "<TD ALIGN=\"Center\" VALIGN=\"Middle\"  style=\"width: 200px; BACKGROUND-COLOR: #F2C2CB  \">" + System.Environment.NewLine + ShipImgPath + System.Environment.NewLine + "</TD>" + System.Environment.NewLine);
                                }
                                //Add the Cart Image

                                if (Restricted.ToUpper() == "YES")
                                {
                                    CartImgPath = oHelper.GetOptionValues("RESTRICTED PRODUCT TEXT");
                                    string CartUrl = oHelper.GetOptionValues("RESTRICTED PRODUCT URL").ToString();
                                    CartImgPath = "<A HREF=\"" + CartUrl + "\" style=\"text-decoration:none\">" + CartImgPath + " </A>";
                                }
                                else
                                {
                                    CartImgPath = oHelper.GetOptionValues("IMAGE PATH").ToString() + oHelper.GetOptionValues("CARTIMGPATH").ToString();
                                    Min_ord_qty = oOrder.GetProductMinimumOrderQty(ProdID);
                                    string CartUrl = oHelper.GetOptionValues("CARTURL").ToString();
                                    CartUrl = CartUrl.Replace("{PRODUCT_ID}", ProdID.ToString());
                                    CartUrl = CartUrl.Replace("{MIN_ORD_QTY}", Min_ord_qty.ToString());
                                    CartImgPath = "<A HREF=\"" + CartUrl + "\" style=\"text-decoration:none\" onMouseOut=\"MM_swapImgRestore()\" onMouseOver=\"MM_swapImage('Image" + ProdID.ToString() + "_mp','','images/but_buy2.gif',1)\"><img src=\"images/but_buy1.gif\" name=\"Image" + ProdID.ToString() + "_mp\" height=\"25\" border=\"0\"></A>";
                                    //CartImgPath = "<A HREF=\"" + CartUrl + "\" style=\"text-decoration:none\"><IMG SRC=\"" + CartImgPath + "\" style=\"border-width:0\"></A>";
                                    CartImgPath = "<table><tr><td>" +
                                       "<input valign=\"middle\" name=\"txt" + ProdID.ToString() + "_" + oOrder.GetProductAvilableQty(ProdID).ToString() + "_" + Min_ord_qty.ToString() + "_" + FamilyID.ToString() + "\" type=\"text\" size=\"5\" id=\"txt" + ProdID.ToString() + "_" + oOrder.GetProductAvilableQty(ProdID).ToString() + "_" + Min_ord_qty.ToString() + "_" + FamilyID.ToString() + "\" style=\"background-color:#FFFFFF;color: #000000;height=23;\"   /> " +
                                     "</td><td>" +
                                     "  <a style=\"cursor:pointer;\" valign=\"middle\"  onMouseOut=\"MM_swapImgRestore()\" onMouseOver=\"MM_swapImage('Image" + ProdID.ToString() + "_fp','','images/but_buy2.gif',1)\">" +
                                     "   <img src=\"images/but_buy1.gif\" name=\"Image" + ProdID.ToString() + "_fp\" width=\"76\" height=\"25\" border=\"0\" onClick=\"productbuy('txt" + ProdID.ToString() + "_" + oOrder.GetProductAvilableQty(ProdID).ToString() + "_" + Min_ord_qty.ToString() + "_" + FamilyID.ToString() + "','" + ProdID.ToString() + "');\"/>" +
                                     "</a></td></tr></table>";

                                }
                                strBldr.Append(System.Environment.NewLine + "<TD ALIGN=\"Center\" VALIGN=\"Middle\"  style=\"width: 200px; BACKGROUND-COLOR: #F2C2CB  \">" + System.Environment.NewLine + CartImgPath + System.Environment.NewLine + "</TD>" + System.Environment.NewLine);
                            }
                        }
                    }
                    strBldr.Append(System.Environment.NewLine + "</TR>" + System.Environment.NewLine);
                }

                strBldr.Append("</TABLE></TD></TR></TABLE>");
                //////strBldr.Append("</HTML>");
            }
            return strBldr.ToString();
        }

        /// <summary>
        /// This is used to Build the ImagePath based on the Relative Path
        /// </summary>
        /// <param name="relativePath">string</param>
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
        ///    
        /// }
        /// </code>
        /// </example>
        [WebMethod]
        private string ConstructImagePath(string relativePath)
        {
            string ProductImagePath = oHelper.GetOptionValues("PRODUCT IMAGE PATH").ToString(); //"/ProdImages"; 
            string imageUrl = "";
            string returnstr = "";
            bool contin = true;
            string Regx = " | * ?";
            string TestPath = Server.MapPath("").ToString() + "/" + ProductImagePath + relativePath;
            for (int i = 0; i < Regx.Length; i++)
            {
                if (TestPath.Contains(Regx[i].ToString()))
                {
                    contin = false;

                }
            }

            if (contin == true)
            {
                if (File.Exists(TestPath) == true)
                {
                    FileInfo chkFile = new FileInfo(TestPath);
                    if (chkFile.Extension.ToUpper() == ".BMP" || chkFile.Extension.ToUpper() == ".JPG" ||
                         chkFile.Extension.ToUpper() == ".GIF" || chkFile.Extension.ToUpper() == ".EPS")
                    {
                        imageUrl = TestPath;
                        returnstr = "<IMG SRC = \"" + imageUrl + "\" HEIGHT=" + "100" + " WIDTH = " + "100" + ">";
                    }

                    if (chkFile.Extension.ToUpper() == ".AVI" || chkFile.Extension.ToUpper() == ".WMV" ||
                         chkFile.Extension.ToUpper() == ".MPG" || chkFile.Extension.ToUpper() == ".SWF")
                    {
                        imageUrl = TestPath;
                        returnstr = "<EMBED SRC= \"" + imageUrl.ToString() + "\" height = 150pts width = 150pts AUTOPLAY=\"false\" CONTROLLER=\"true\"/>";
                    }
                }
            }

            return returnstr;
        }

        /// <summary>
        /// This is used to Merge Cells vertically based on the Row Index and Column Index
        /// </summary>
        /// <param name="colIndex">int</param>
        /// <param name="rowIndex">int</param>
        /// <returns>int</returns>
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
        ///    
        /// }
        /// </code>
        /// </example>
        [WebMethod]
        private int GetVerticalMergeCells(int colIndex, int rowIndex)
        {
            bool exit = false;
            int mergeCount = 0;
            for (int i = rowIndex; i < pivotTable.Rows.Count; i++)
            {
                if (pivotTable.Rows[i][colIndex].ToString() == pivotTable.Rows[rowIndex][colIndex].ToString())
                { mergeCount++; }
                else
                { exit = true; break; }

                if (exit == true) { break; }
            }
            return mergeCount;
        }

        /// <summary>
        /// This is used to Merge Cells horizontally based on the Row Index and Column Index
        /// </summary>
        /// <param name="colIndex">int</param>
        /// <param name="rowIndex">int</param>
        /// <returns>int</returns>
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
        ///    
        /// }
        /// </code>
        /// </example>
        [WebMethod]
        private int GetHorizontalMergeCells(int colIndex, int rowIndex)
        {
            bool exit = false;
            int mergeCount = 0;
            for (int j = colIndex; j < pivotTable.Columns.Count; j++)
            {
                if (pivotTable.Rows[rowIndex][j].ToString() == pivotTable.Rows[rowIndex][colIndex].ToString())
                { mergeCount++; }
                else
                { exit = true; break; }

                if (exit == true) { break; }
            }
            return mergeCount;
        }

        /// <summary>
        /// This is used to Construct the Pivot Table
        /// </summary>
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
        ///    
        /// }
        /// </code>
        /// </example>
        [WebMethod]
        private void ConstructPivotTable()
        {
            //Prepare Summary Fileds for Grouping
            if (m_summaryGroupField.Trim() != string.Empty)
            {
                for (int i = 0; i < m_summaryFields.Count; i++)
                {
                    if (m_summaryFields[i].Trim() == m_summaryGroupField.Trim())
                    {
                        m_summaryFields.RemoveAt(i);
                        i = 0;
                    }
                }
                m_summaryFields.Add(m_summaryGroupField.Trim());
            }
            pivotTable = new DataTable();
            Int32 bodyFieldCount = m_summaryFields.Count;
            Int32 columnCount = (columnTable.Rows.Count * bodyFieldCount) + rowTable.Columns.Count;
            Int32 RowFieldsColumnCount = rowTable.Columns.Count;

            //Add columns to Pivot Table            
            for (int i = 0; i < columnCount; i++)
            {
                string colName = "Column" + i;
                pivotTable.Columns.Add(colName);
            }

            //Construct Header Columns
            //========================
            for (int j = 0; j < columnTable.Columns.Count; j++)
            {
                DataRow dr = pivotTable.NewRow();
                Int32 colPosition = 0;

                //Set value for Corner 
                for (int k = 0; k < rowTable.Columns.Count; k++)
                {
                    if (m_showColumnHeaders == true)
                    {
                        dr[k] = columnTable.Columns[j].ColumnName;
                    }
                    colPosition = colPosition + 1;
                }

                for (int P = 0; P < columnTable.Rows.Count; P++)
                {
                    for (int q = 0; q < m_summaryFields.Count; q++)
                    {
                        this.GetCurrencySymbol(columnTable.Columns[j].ExtendedProperties["ATTRIBUTE_ID"].ToString());
                        if (columnTable.Columns[j].ExtendedProperties["ATTRIBUTE_TYPE"].ToString() == "4")//|| columnTable.Columns[j].ExtendedProperties["ATTRIBUTE_ID"].ToString() == "3")
                        {

                            if (columnTable.Rows[P][j].ToString().Trim() != string.Empty)
                            {
                                string currencyValue = string.Empty;
                                Decimal PriceValue = Convert.ToDecimal(columnTable.Rows[P][j].ToString().Trim());
                                currencyValue = PriceValue.ToString(currencyFormat);
                                if (currencyFormat == null || currencyFormat == "")
                                {
                                    currencyValue = PriceValue.ToString("N2");
                                    currencyValue = oHelper.GetOptionValues("CURRENCYFORMAT").ToString() + " " + currencyValue.ToString();
                                }

                                if ((EmptyCondition == "Null" || EmptyCondition == "Empty" || EmptyCondition == null) && (currencyValue == string.Empty))
                                {
                                    dr[colPosition] = ReplaceText;
                                }
                                else if (currencyValue == EmptyCondition)
                                {
                                    dr[colPosition] = ReplaceText;
                                }
                                else
                                {
                                    if (Headeroptions == "All")
                                    {
                                        dr[colPosition] = Prefix + " " + currencyValue.ToString().Replace("\r", "<br/>").Replace("\n", "&nbsp;") + " " + Suffix;
                                    }
                                    else
                                    {
                                        dr[colPosition] = currencyValue.ToString().Replace("\r", "<br/>").Replace("\n", "&nbsp;");
                                    }

                                }
                            }
                            else
                            { dr[colPosition] = ReplaceText; }
                        }
                        else if (columnTable.Columns[j].ExtendedProperties["ATTRIBUTE_TYPE"].ToString() == "3")
                        {
                            if (columnTable.Columns[j].ExtendedProperties["ATTRIBUTE_TYPE"].ToString() == "3")
                            {
                                if (!pivotTable.Columns[colPosition].ExtendedProperties.ContainsKey("ImageColFields"))
                                    pivotTable.Columns[colPosition].ExtendedProperties.Add("ImageColFields", "True");
                            }
                            dr[colPosition] = columnTable.Rows[P][j].ToString();
                        }
                        else
                        {
                            if (columnTable.Rows[P][j].ToString().Trim() != string.Empty)
                            {
                                if ((EmptyCondition == "Null" || EmptyCondition == "Empty" || EmptyCondition == null) && (columnTable.Rows[P][j].ToString() == string.Empty))
                                {
                                    dr[colPosition] = ReplaceText;
                                }
                                else if (columnTable.Rows[P][j].ToString() == EmptyCondition)
                                {
                                    dr[colPosition] = ReplaceText;
                                }
                                else
                                {
                                    dr[colPosition] = Prefix + " " + columnTable.Rows[P][j].ToString().Replace("\r", "<br/>").Replace("\n", "&nbsp;") + " " + Suffix;
                                }
                            }
                            else
                            {
                                dr[colPosition] = columnTable.Rows[P][j].ToString();
                            }
                        }

                        if (P < columnTable.Rows.Count - 1 && j == columnTable.Columns.Count - 1)
                        {
                            if (m_summaryFields[q].Trim() == m_summaryGroupField.Trim())
                            {
                            }
                        }
                        colPosition = colPosition + 1;
                    }
                }
                pivotTable.Rows.Add(dr);
            }


            //Create Header for Rows and Summary Fields
            //=========================================
            {
                DataRow drow = pivotTable.NewRow();
                Int32 colPos = 0;

                //Set value for Rows 
                for (int k = 0; k < rowTable.Columns.Count; k++)
                {
                    if (rowTable.Columns[k].ExtendedProperties["ATTRIBUTE_TYPE"].ToString() == "3")
                    {
                        if (!pivotTable.Columns[k].ExtendedProperties.ContainsKey("ImageRowFields"))
                            pivotTable.Columns[k].ExtendedProperties.Add("ImageRowFields", "True");
                    }

                    //Set Attribute Info
                    if (!pivotTable.Columns[k].ExtendedProperties.ContainsKey("ATTRIBUTE_STYLE"))
                    {
                        pivotTable.Columns[k].ExtendedProperties.Add("ATTRIBUTE_STYLE", rowTable.Columns[k].ExtendedProperties["ATTRIBUTE_STYLE"].ToString());
                    }
                    {
                        drow[k] = rowTable.Columns[k].ColumnName;
                    }
                    colPos = colPos + 1;
                }

                //Set value for Summary Fields
                for (int r = 0; r < pivotTable.Columns.Count; r++)
                {
                    for (int t = 0; t < m_summaryFields.Count; t++)
                    {
                        if (colPos < pivotTable.Columns.Count)
                        {
                            {
                                drow[colPos] = m_summaryFields[t];
                            }
                            colPos = colPos + 1;
                        }
                    }
                }
                pivotTable.Rows.Add(drow);
            }

            //Populate Body Fields
            for (int k = 0; k < rowTable.Rows.Count; k++)
            {
                DataRow dr = pivotTable.NewRow();
                Int32 colPosition = 0;
                //Set value for Row Fields 
                for (int l = 0; l < rowTable.Columns.Count; l++)
                {
                    this.GetCurrencySymbol(rowTable.Columns[l].ExtendedProperties["ATTRIBUTE_ID"].ToString());

                    if (rowTable.Columns[l].ExtendedProperties["ATTRIBUTE_TYPE"].ToString() == "4")//|| rowTable.Columns[l].ExtendedProperties["ATTRIBUTE_ID"].ToString() == "3")
                    {
                        if (applyCurrencySymbolForFirstRow)
                        {
                            if (k == 0)
                            {
                                if (rowTable.Rows[k][colPosition].ToString() != string.Empty)
                                {
                                    string currencyValue = string.Empty;
                                    Double PriceValue = Convert.ToDouble(rowTable.Rows[k][colPosition].ToString());
                                    currencyValue = PriceValue.ToString(currencyFormat);
                                    if (currencyFormat == null || currencyFormat.ToString() == "")
                                    {
                                        currencyValue = PriceValue.ToString("N2");
                                        currencyValue = oHelper.GetOptionValues("CURRENCYFORMAT").ToString() + " " + currencyValue.ToString();
                                    }
                                    if (EmptyCondition == "Null" || EmptyCondition == "Empty")
                                    {
                                        dr[l] = currencyValue;
                                    }
                                    else if (Convert.ToDecimal(currencyValue) == Convert.ToDecimal(EmptyCondition))
                                    {
                                        dr[l] = ReplaceText;
                                    }
                                    else
                                    {
                                        if (Headeroptions == "All")
                                        {
                                            dr[l] = Prefix + " " + currencyValue + " " + Suffix;
                                        }
                                        else
                                        {
                                            dr[l] = currencyValue;
                                        }

                                    }
                                }
                                else
                                { dr[l] = ReplaceText; }
                            }
                            else
                            {
                                dr[l] = rowTable.Rows[k][colPosition].ToString();
                            }
                        }
                        else
                        {
                            if (rowTable.Rows[k][colPosition].ToString() != string.Empty)
                            {
                                string currencyValue = string.Empty;
                                Double PriceValue = Convert.ToDouble(rowTable.Rows[k][colPosition].ToString());
                                currencyValue = PriceValue.ToString(currencyFormat);
                                if (currencyFormat == null || currencyFormat == "")
                                {
                                    currencyValue = PriceValue.ToString("N2");
                                    currencyValue = oHelper.GetOptionValues("CURRENCYFORMAT").ToString() + " " + currencyValue.ToString();
                                }
                                if ((EmptyCondition == "Null" || EmptyCondition == "Empty" || EmptyCondition == null) && (currencyValue == string.Empty))
                                {
                                    dr[l] = ReplaceText;
                                }
                                else if (currencyValue == EmptyCondition)
                                {
                                    dr[l] = ReplaceText;
                                }
                                else
                                {
                                    dr[l] = Prefix + " " + currencyValue.ToString().Replace("\r", "<br/>").Replace("\n", "&nbsp;") + " " + Suffix;
                                }
                            }
                            else
                            { dr[l] = ReplaceText; }
                        }
                    }
                    else if (rowTable.Columns[l].ExtendedProperties["ATTRIBUTE_TYPE"].ToString() == "3") //modified to support Images 
                    {
                        dr[l] = rowTable.Rows[k][colPosition].ToString();
                    }
                    else
                    {
                        if (rowTable.Rows[k][colPosition].ToString().Trim() != string.Empty)
                        {
                            if ((EmptyCondition == "Null" || EmptyCondition == "Empty" || EmptyCondition == null) && (rowTable.Rows[k][colPosition].ToString() == string.Empty))
                            {
                                dr[l] = ReplaceText;
                            }
                            else if (rowTable.Rows[k][colPosition].ToString() == EmptyCondition)
                            {
                                dr[l] = ReplaceText;
                            }
                            else
                            {
                                dr[l] = Prefix + " " + rowTable.Rows[k][colPosition].ToString().Replace("\r", "<br/>").Replace("\n", "&nbsp;") + " " + Suffix;
                            }
                        }
                        else
                        {
                            dr[l] = rowTable.Rows[k][colPosition].ToString();
                        }
                    }
                    colPosition = colPosition + 1;
                }
                for (int m = 0; m < columnTable.Rows.Count; m++)
                {
                    for (int n = 0; n < m_summaryFields.Count; n++)
                    {

                        if (sourceTable.Columns[m_summaryFields[n].ToString()].ExtendedProperties["ATTRIBUTE_TYPE"].ToString() == "3")
                        {
                            if (!pivotTable.Columns[colPosition].ExtendedProperties.ContainsKey("ImageBodyFields"))
                                pivotTable.Columns[colPosition].ExtendedProperties.Add("ImageBodyFields", "True");
                        }

                        //Set Attribute Info
                        if (!pivotTable.Columns[colPosition].ExtendedProperties.ContainsKey("ATTRIBUTE_STYLE"))
                        {
                            pivotTable.Columns[colPosition].ExtendedProperties.Add("ATTRIBUTE_STYLE", sourceTable.Columns[m_summaryFields[n]].ExtendedProperties["ATTRIBUTE_STYLE"].ToString());
                        }
                        if (applyCurrencySymbolForFirstRow) //IF Apply CurrencySymbol For First Row
                        {
                            if (k == 0) //if First Row
                            {
                                dr[colPosition] = FetchBodyFieldValue(m_summaryFields[n], k, m, true);
                            }
                            else //if not first Row
                            {
                                dr[colPosition] = FetchBodyFieldValue(m_summaryFields[n], k, m, false);
                            }
                        }
                        else //If not apply CurrencySymbol for first row
                        {
                            dr[colPosition] = FetchBodyFieldValue(m_summaryFields[n], k, m, true);
                        }
                        colPosition = colPosition + 1;
                    }
                }
                pivotTable.Rows.Add(dr);
                //Group Summary Fileds
                if (m_summaryGroupField.Trim() != string.Empty)
                {
                    this.SetGroupSummaryField();
                }
                if (m_showSummaryHeaders == false && m_showRowHeaders == true)
                {
                    for (int i = rowTable.Columns.Count; i < pivotTable.Columns.Count; i++)
                    {
                        pivotTable.Rows[columnTable.Columns.Count][i] = "";
                    }
                }
                else if (m_showRowHeaders == false && m_showSummaryHeaders == true)
                {
                    for (int i = 0; i < rowTable.Columns.Count; i++)
                    {
                        pivotTable.Rows[columnTable.Columns.Count][i] = "";
                    }
                }
                else if (m_showRowHeaders == false && m_showSummaryHeaders == false)
                {
                    pivotTable.Rows.RemoveAt(columnTable.Columns.Count);
                }
            }
        }

        /// <summary>
        /// This is used to group the Summary Fields
        /// </summary>
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
        ///    ...
        ///    if (m_summaryGroupField.Trim() != string.Empty)
        ///    {
        ///       this.SetGroupSummaryField();
        ///    }
        /// }
        /// </code>
        /// </example>
        [WebMethod]
        private void SetGroupSummaryField()
        {
            int index;
            if (columnTable.Columns.Count > 1)
            {
                index = columnTable.Columns.Count - 2;
            }
            else { index = 0; }

            string lastValue = pivotTable.Rows[index][0].ToString().Trim();

            for (int c = 0; c < pivotTable.Columns.Count; c++)
            {
                if ((pivotTable.Rows[index][c].ToString().Trim() != lastValue) || (c == pivotTable.Columns.Count - 1))
                {//Owner Group Changes Here
                    if (m_summaryGroupField.Trim() == pivotTable.Rows[columnTable.Columns.Count][c - 1].ToString().Trim())
                    {
                        if (!pivotTable.Columns[c - 1].ExtendedProperties.ContainsKey("GroupField"))
                            pivotTable.Columns[c - 1].ExtendedProperties.Add("GroupField", "True");
                    }
                }
                else
                {

                }
                lastValue = pivotTable.Rows[index][c].ToString().Trim();
            }

            if (m_summaryGroupField.Trim() == pivotTable.Rows[columnTable.Columns.Count][pivotTable.Columns.Count - 1].ToString().Trim())
            {
                if (!pivotTable.Columns[pivotTable.Columns.Count - 1].ExtendedProperties.ContainsKey("GroupField"))
                    pivotTable.Columns[pivotTable.Columns.Count - 1].ExtendedProperties.Add("GroupField", "True");
            }

            for (int c = 0; c < pivotTable.Columns.Count; c++)
            {
                if (m_summaryGroupField.Trim() == pivotTable.Rows[columnTable.Columns.Count][c].ToString().Trim())
                {
                    if (!pivotTable.Columns[c].ExtendedProperties.Contains("GroupField"))
                    {
                        pivotTable.Columns.RemoveAt(c);
                        c = 0;
                    }
                    else
                    {
                        for (int j = columnTable.Columns.Count - 1; j >= 0; j--)
                        {
                            pivotTable.Rows[j][c] = "";
                        }
                    }
                }
            }
        }

        /// <summary>
        /// This ism used to Retrieve the Field Values
        /// </summary>
        /// <param name="fieldName">string</param>
        /// <param name="rowFieldIndex">int</param>
        /// <param name="colFieldIndex">int</param>
        /// <param name="applyCurrencySymbol">bool</param>
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
        ///    ...
        ///    if (k == 0) //if First Row
        ///    {
        ///        dr[colPosition] = FetchBodyFieldValue(m_summaryFields[n], k, m, true);
        ///    }
        ///        else //if not first Row
        ///    {
        ///        dr[colPosition] = FetchBodyFieldValue(m_summaryFields[n], k, m, false);
        ///    }
        /// }
        /// </code>
        /// </example>
        [WebMethod]
        private string FetchBodyFieldValue(string fieldName, int rowFieldIndex, int colFieldIndex, bool applyCurrencySymbol)
        {
            string result = string.Empty;
            string condition = string.Empty;

            if (ConstructRowFieldCondition(rowFieldIndex).Trim() != string.Empty && ConstructColumnFieldCondition(colFieldIndex).Trim() != string.Empty)
            {
                condition = ConstructRowFieldCondition(rowFieldIndex) + " and " + ConstructColumnFieldCondition(colFieldIndex);
            }
            else if (ConstructRowFieldCondition(rowFieldIndex).Trim() == string.Empty)
            {
                condition = ConstructColumnFieldCondition(colFieldIndex);
            }
            else if (ConstructColumnFieldCondition(colFieldIndex).Trim() == string.Empty)
            {
                condition = ConstructRowFieldCondition(rowFieldIndex);
            }
            try
            {
                if (condition != string.Empty)
                {
                    foreach (DataRow oDr in sourceTable.Select(condition))
                    {
                        this.GetCurrencySymbol(sourceTable.Columns[fieldName].ExtendedProperties["ATTRIBUTE_ID"].ToString());
                        if (sourceTable.Columns[fieldName].ExtendedProperties["ATTRIBUTE_TYPE"].ToString() == "4" || sourceTable.Columns[fieldName].ExtendedProperties["ATTRIBUTE_ID"].ToString() == "3")
                        {
                            if (oDr[fieldName].ToString().Trim() != string.Empty)
                            {
                                Double PriceValue = Convert.ToDouble(oDr[fieldName]);
                                result = PriceValue.ToString(currencyFormat);
                                if (currencyFormat == null || currencyFormat.ToString() == "")
                                {
                                    result = PriceValue.ToString("N2");
                                    result = oHelper.GetOptionValues("CURRENCYFORMAT").ToString() + " " + result.ToString();
                                }

                                if ((EmptyCondition == "Null" || EmptyCondition == "Empty" || EmptyCondition == null) && (result == string.Empty))
                                {
                                    result = ReplaceText;
                                }
                                else if ((result) == (EmptyCondition))
                                {
                                    result = ReplaceText;
                                }
                                else
                                {
                                    result = Prefix + " " + result.ToString().Replace("\r", "<br/>").Replace("\n", "&nbsp;") + " " + Suffix;
                                }
                            }
                            else
                            {
                                result = ReplaceText;
                            }
                        }
                        else
                        {
                            result = oDr[fieldName].ToString();
                        }
                    }
                }
            }
            catch //(Exception ex)
            {
                result = m_placeHolderText;
            }

            if (result == string.Empty || result == null)
            {
                result = m_placeHolderText;
            }

            return result;
        }

        /// <summary>
        /// This is used to construct Row Field Condition based on Row Index
        /// </summary>
        /// <param name="rowIndex">int</param>
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
        ///    ...
        ///    if (ConstructRowFieldCondition(rowFieldIndex).Trim() != string.Empty && ConstructColumnFieldCondition(colFieldIndex).Trim() != string.Empty)
        ///    {
        ///        condition = ConstructRowFieldCondition(rowFieldIndex) + " and " + ConstructColumnFieldCondition(colFieldIndex);
        ///    }
        /// }
        /// </code>
        /// </example>
        [WebMethod]
        private string ConstructRowFieldCondition(int rowIndex)
        {
            string rowCond = string.Empty;
            if (rowTable.Columns.Count > 0)
            {
                for (int l = 0; l < rowTable.Columns.Count; l++)
                {
                    if (rowTable.Rows[rowIndex][l].ToString().Trim() != string.Empty)
                    {
                        if (rowCond.Trim() != string.Empty)
                            rowCond = rowCond + " and [" + Prepare(rowTable.Columns[l].ColumnName) + "] = " + "'" + Prepare(rowTable.Rows[rowIndex][l].ToString()) + "'";
                        else
                            rowCond = " [" + Prepare(rowTable.Columns[l].ColumnName) + "] = " + "'" + Prepare(rowTable.Rows[rowIndex][l].ToString()) + "'";
                    }
                }
            }
            return rowCond;
        }

        /// <summary>
        /// This is used to construct Cloumn Field Condition based on Row Index
        /// </summary>
        /// <param name="rowIndex">int</param>
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
        ///    ...
        ///    if (ConstructRowFieldCondition(rowFieldIndex).Trim() != string.Empty && ConstructColumnFieldCondition(colFieldIndex).Trim() != string.Empty)
        ///    {
        ///        condition = ConstructColumnFieldCondition(rowFieldIndex) + " and " + ConstructColumnFieldCondition(colFieldIndex);
        ///    }
        /// }
        /// </code>
        /// </example>
        [WebMethod]
        private string ConstructColumnFieldCondition(int rowIndex)
        {
            string colCond = string.Empty;
            if (columnTable.Columns.Count > 0)
            {
                for (int l = 0; l < columnTable.Columns.Count; l++)
                {
                    if (columnTable.Rows[rowIndex][l].ToString().Trim() != string.Empty)
                    {
                        if (colCond.Trim() != string.Empty)
                            colCond = colCond + " and [" + Prepare(columnTable.Columns[l].ColumnName) + "] = " + "'" + Prepare(columnTable.Rows[rowIndex][l].ToString()) + "'";
                        else
                            colCond = " [" + Prepare(columnTable.Columns[l].ColumnName) + "] = " + "'" + Prepare(columnTable.Rows[rowIndex][l].ToString()) + "'";
                    }
                }
            }
            return colCond;
        }

        /// <summary>
        /// This method is used to Prepare the Layout
        /// </summary>
        /// <param name="StrVal">string</param>
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
        ///    string xmlStr;
        ///    string HTMLText;
        ///    ...
        ///    xmlStr = prepare(HTMLText);
        /// }
        /// </code>
        /// </example>
        [WebMethod]
        public string Prepare(string StrVal)
        {
            string strRetVal;
            strRetVal = StrVal.Replace("'", "''");
            strRetVal = strRetVal.Trim();
            return strRetVal;
        }

        #region "Comments for Buyer Groups
        /*Comments for BuyerGroups
        public DataTable ProductTableData()
        {
            int FamilyID = oHelper.CI(m_familyID);
            DataSet dsProd = new DataSet();
            DataSet dsCore = new DataSet();
            DataSet dsAttrVal = null;
            DataSet dsBgDisc = null;
            DataTable dtProducts = new DataTable("Products");
            try
            {
                dtProducts = BuildProductTableStructure();

                if (oHelper.GetOptionValues("ENABLED RESTRICTED PRODUCT").ToString().ToUpper() == "YES")
                    dsProd = oPro.GetRestrictedFamilyProductList(FamilyID, true);

                else
                    dsProd = oPro.GetRestrictedFamilyProductList(FamilyID, false);

                if (dsProd != null)
                {
                    DataRow[] dtRow;
                    ///////////////////////////////////////////////////////////////////////if (Request["SName"] != null)
                    {
                        //dtRow = dsProd.Tables[0].Select(CoreAttribute_4 + "='" + Request["SName"].ToString() + "'");
                        dtRow = dsProd.Tables[0].Select();
                    }
                    ///////////////////////////////////////////////////////////////////////else
                    {
                        dtRow = dsProd.Tables[0].Select();
                    }

                    //Add attributes values for each product under that family.
                    foreach (DataRow rProd in dtRow)
                    {
                        DataRow ProdRow = dtProducts.NewRow();
                        ProdRow["PRODUCT_ID"] = oHelper.CI(rProd["PRODUCT_ID"].ToString());
                        dsAttrVal = new DataSet();
                        dsAttrVal = oPro.GetProductsAttributesValues(oHelper.CI(rProd["PRODUCT_ID"]), FamilyID);
                        int ColIndex = 0;
                        if (dsAttrVal != null)
                        {
                            foreach (DataRow rAttr in dsAttrVal.Tables[0].Rows)
                            {



                                ColIndex = GetColPosition(oHelper.CI(rAttr["ATTRIBUTE_ID"].ToString()));

                                ProdRow[ColIndex] = rAttr["ATTRIBUTE_VALUE"].ToString();
                            }
                        }
                        dsAttrVal = null;

                        //Add product Price values...
                        //Getting the particular product price attributes value.
                        dsAttrVal = new DataSet();
                        //Calculate the base price and also Calculate the discount amount based on the default buyergroup.
                        dsAttrVal = oPro.GetProductPriceValues(oHelper.CI(rProd["PRODUCT_ID"]), AttrPriceIDs);
                        if (dsAttrVal != null)
                        {
                            decimal CurPrice = 0;
                            foreach (DataRow rPrice in dsAttrVal.Tables[0].Rows)
                            {
                                ColIndex = GetColPosition(oHelper.CI(rPrice[0].ToString()));
                                if (rPrice["ATTRIBUTE_VALUE"].ToString() != "")
                                {
                                    //CustomPrice PRICE


                                    CurPrice = oHelper.CDEC(rPrice["ATTRIBUTE_VALUE"].ToString());
                                }
                                //Calculate the buyer group based discount price.
                                string BGName = oBG.GetBuyerGroup(UserID);
                                dsBgDisc = oBG.GetBuyerGroupBasedDiscountDetails(BGName);

                                if (dsBgDisc != null)
                                {
                                    if (dsBgDisc.Tables[0].Rows.Count > 0)
                                    {
                                        decimal DiscVal = oHelper.CDEC(dsBgDisc.Tables[0].Rows[0].ItemArray[0].ToString());
                                        DateTime ValidDt = DateTime.Now.Subtract(System.TimeSpan.FromDays(7));//By default set the  previous date.
                                        if (dsBgDisc.Tables[0].Rows[0].ItemArray[1].ToString() != "")
                                        {
                                            ValidDt = Convert.ToDateTime(dsBgDisc.Tables[0].Rows[0].ItemArray[1].ToString());
                                        }
                                        string CalMth = dsBgDisc.Tables[0].Rows[0].ItemArray[2].ToString();
                                        if (ValidDt.CompareTo(DateTime.Today) >= 0 && DiscVal > 0)
                                        {
                                            ProdRow[ColIndex] = oHelper.FixDecPlace(oBG.CalculateBGDiscountPrice(CurPrice, DiscVal, CalMth));
                                        }
                                        else
                                        {
                                            ProdRow[ColIndex] = oHelper.FixDecPlace(CurPrice);
                                        }
                                    }
                                }
                                else
                                {
                                    ProdRow[ColIndex] = CurPrice;
                                }
                            }
                        }
                        dsAttrVal = null;
                        ProdRow["MIN_ORD_QTY"] = oHelper.CI(rProd["MIN_ORD_QTY"].ToString());
                        ProdRow["QTY_AVAIL"] = oHelper.CI(rProd["QTY_AVAIL"].ToString());

                        ProdRow["IS_SHIPPING"] = rProd["IS_SHIPPING"].ToString();

                        if (oHelper.GetOptionValues("ENABLED RESTRICTED PRODUCT").ToString().ToUpper() == "YES")
                            ProdRow["RESTRICTED"] = rProd["RESTRICTED"].ToString();
                        dtProducts.Rows.Add(ProdRow);
                    }
                    dsProd = null;
                }
            }
            catch (Exception ex)
            {
                oErr.ErrorMsg = ex; oErr.CreateLog();
            }
            
            return dtProducts;
        }
        public DataTable BuildProductTableStructure()
        {
            int FamilyID = oHelper.CI(m_familyID);
            DataTable dtProd = new DataTable();
            DataTable dtBuild = new DataTable();
            DataColumn dtCol;
            int BGPriceID = 0;
            int i = 0;
            
            try
            {
                //Add the new column for family id and product id.
                //dtCol = new DataColumn("FamilyID", typeof(int));
                //dtBuild.Columns.Add(dtCol);

                dtCol = new DataColumn("PRODUCT_ID", typeof(int));
                dtBuild.Columns.Add(dtCol);

                //Get the buyer group based price attribute id.
                if (Session["USER_ID"] != null && Session["USER_ID"].ToString() != "")
                {
                    BGPriceID = oBG.GetBuyerGroupPriceID(oHelper.CI(Session["USER_ID"].ToString()));
                }
                else
                {
                    BGPriceID = oBG.GetBuyerGroupPriceID(0);
                }

                //Get the selected web attribute names from database.
                dtProd = oPro.GetAttributesName(FamilyID);

                for (i = 0; i < dtProd.Rows.Count; i++)
                {
                    //if (dtProd.Rows[i].ItemArray[2].ToString() == "1" && oHelper.CI(dtProd.Rows[i].ItemArray[0].ToString()) > 4)
                    //{
                    //    AttrSpecIDs = AttrSpecIDs + dtProd.Rows[i].ItemArray[0].ToString() + ",";

                    //}
                    //else if (dtProd.Rows[i].ItemArray[2].ToString() == "2" && oHelper.CI(dtProd.Rows[i].ItemArray[0].ToString()) > 4)
                    //{
                    //    AttrDescIDs = AttrDescIDs + dtProd.Rows[i].ItemArray[0].ToString() + ",";
                    //}

                    dtCol = new DataColumn(dtProd.Rows[i].ItemArray[1].ToString(), typeof(string));

                    dtCol.ExtendedProperties.Add("ATTRIBUTEID", dtProd.Rows[i].ItemArray[0].ToString());
                    dtBuild.Columns.Add(dtCol);
                }
                dtProd = null;
                //if (AttrSpecIDs != null && AttrSpecIDs != "") AttrSpecIDs = AttrSpecIDs.Substring(0, AttrSpecIDs.Length - 1);
                //if (AttrDescIDs != null && AttrDescIDs != "") AttrDescIDs = AttrDescIDs.Substring(0, AttrDescIDs.Length - 1);

                //Add the buyer group based price attribute.
                dtProd = new DataTable();
                dtProd = oPro.GetBGAttributesName(BGPriceID);
                for (i = 0; i < dtProd.Rows.Count; i++)
                {
                    //Custom Price based on buyer group.
                    if (dtProd.Rows[i].ItemArray[2].ToString() == "4" && oHelper.CI(dtProd.Rows[i].ItemArray[0].ToString()) > 4)
                    {
                        AttrPriceIDs = AttrPriceIDs + dtProd.Rows[i].ItemArray[0].ToString() + ",";
                    }
                    //Base Price.
                    else if (dtProd.Rows[i].ItemArray[0].ToString() == "3")
                    {
                        AttrPriceIDs = AttrPriceIDs + dtProd.Rows[i].ItemArray[0].ToString() + ",";
                    }
                    dtCol = new DataColumn(dtProd.Rows[i].ItemArray[1].ToString(), typeof(decimal));
                    dtCol.ExtendedProperties.Add("ATTRIBUTEID", dtProd.Rows[i].ItemArray[0].ToString());
                    dtBuild.Columns.Add(dtCol);
                }

                if (AttrPriceIDs != null && AttrPriceIDs != "") AttrPriceIDs = AttrPriceIDs.Substring(0, AttrPriceIDs.Length - 1);
                //Add the product minimum order qty.
                dtCol = new DataColumn("MIN_ORD_QTY", typeof(int));
                dtBuild.Columns.Add(dtCol);
                dtCol = new DataColumn("QTY_AVAIL", typeof(int));
                dtBuild.Columns.Add(dtCol);
                dtCol = new DataColumn("IS_SHIPPING", typeof(bool));
                dtBuild.Columns.Add(dtCol);
                if (oHelper.GetOptionValues("ENABLED RESTRICTED PRODUCT").ToString().ToUpper() == "YES")
                {
                    dtCol = new DataColumn("RESTRICTED", typeof(string));
                    dtBuild.Columns.Add(dtCol);
                }

            }
            catch (Exception ex)
            {
                oErr.ErrorMsg = ex; oErr.CreateLog();
                return null;
            }
            return dtBuild;
        }
        public int GetColPosition(int AttributeID)
        {
            int FamilyID = oHelper.CI(m_familyID);
            DataTable dtAttrNames = new DataTable();
            int c;
            int Colindex = 0;
            int BGPriceID = 0;
            if (Session["USER_ID"] != null && Session["USER_ID"].ToString() != "")
            {
                BGPriceID = oBG.GetBuyerGroupPriceID(oHelper.CI(Session["USER_ID"].ToString()));
            }
            else
            {
                BGPriceID = oBG.GetBuyerGroupPriceID(0);
            }
            dtAttrNames = oPro.GetAttributesName(FamilyID);

            if (BGPriceID == AttributeID)
            {
                Colindex = dtAttrNames.Rows.Count + 1;
            }
            else
            {
                for (c = 0; c < dtAttrNames.Rows.Count; c++)
                {
                    if (oHelper.CI(dtAttrNames.Rows[c].ItemArray[0].ToString()) == AttributeID)
                    {
                        Colindex = c + 1;
                        break;

                    }
                }
            }
            return Colindex;
        }
         */
        #endregion

        #endregion

        #region "ProductPreview Methods"

        /// <summary>
        /// This is used to fix the Scaling size for the Image 
        /// </summary>
        /// <param name="origHeight">double</param>
        /// <param name="origWidth">double</param>
        /// <param name="Width">double</param>
        /// <param name="Height">double</param>
        /// <returns>Size</returns>
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
        ///    ...
        ///    Double iHeight = chkSize.Height;
        ///    Double iWidth = chkSize.Width;
        ///    newVal = ScaleImage(iHeight, iWidth, 200, 200);
        /// }
        /// </code>
        /// </example>
        [WebMethod]
        public Size ScaleImage(double origHeight, double origWidth, double Width, double Height)
        {
            Size newSize = new Size();
            double nWidth = Width;
            double nHeight = Height;
            double oWidth = origWidth;
            double oHeight = origHeight;

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

        /// <summary>
        /// This is used to Generate Family Preview Table
        /// </summary>
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
        ///    ...
        ///    CSProductTable oCSProdTab = new CSProductTable(FamilyID.ToString(), CatalogID);
        ///    oCSProdTab.UserID = UserID;
        ///    ProductTemplateHtml = oCSProdTab.GenerateFamilyPreview(); 
        /// }
        /// </code>
        /// </example>
        [WebMethod]
        public string GenerateFamilyPreview()
        {
            _valInc = 0;
            string tableLayout = this.FetchTableLayout();
            string HTMLString = string.Empty;
            //SystemSettingsCollection SettingMemebers = SystemSettingsConfiguration.GetConfig.Members;
            string TempFilepath = ""; //SettingMemebers.GetValue(SystemSettingsCollection.SettingsList.TEMPORARYPATH.ToString());
            oHelper.SQLString = "SELECT DISPLAY_TABLE_HEADER FROM TB_FAMILY WHERE FAMILY_ID = " + _familyID;
            DataSet DsDisTab = oHelper.GetDataSet();
            if (DsDisTab.Tables[0].Rows[0].ItemArray[0].ToString() != null && DsDisTab.Tables[0].Rows[0].ItemArray[0].ToString() != "")
            {
                DisplayHeaders = Convert.ToBoolean(DsDisTab.Tables[0].Rows[0].ItemArray[0].ToString());
            }
            else
            {
                DisplayHeaders = false;
            }

            ///product filter
            StringBuilder DynamicSQl = new StringBuilder();
            DataSet oDsProdFilter = new DataSet();
            string sProdFilter = string.Empty;
            oHelper.SQLString = " SELECT PRODUCT_FILTERS FROM TB_CATALOG WHERE  CATALOG_ID = " + _catalogID + " ";
            oDsProdFilter = oHelper.GetDataSet();
            if (oDsProdFilter.Tables[0].Rows[0].ItemArray[0].ToString() != string.Empty && oDsProdFilter.Tables[0].Rows.Count > 0)
            {
                sProdFilter = oDsProdFilter.Tables[0].Rows[0].ItemArray[0].ToString();
                XmlDocument xmlDOc = new XmlDocument();
                xmlDOc.LoadXml(sProdFilter);
                XmlNode rNode = xmlDOc.DocumentElement;
                // StringBuilder SQLstring = new StringBuilder();
                if (rNode.ChildNodes.Count > 0)
                {
                    for (int i = 0; i < rNode.ChildNodes.Count; i++)
                    {
                        StringBuilder SQLstring = new StringBuilder();
                        XmlNode TableDataSetNode = rNode.ChildNodes[i];
                        if (TableDataSetNode.HasChildNodes)
                        {
                            if (TableDataSetNode.ChildNodes[2].InnerText == " ")
                            {
                                TableDataSetNode.ChildNodes[2].InnerText = "=";
                            }
                            SQLstring.Append("SELECT PRODUCT_ID FROM TB_PROD_SPECS TPS, TBWC_INVENTORY TI WHERE TI.PRODUCT_ID = TPS.PRODUCT_ID AND TI.PRODUCT_STATUS <> 'DISABLEE' AND STRING_VALUE " + TableDataSetNode.ChildNodes[2].InnerText + " '" + TableDataSetNode.ChildNodes[3].InnerText.Trim() + "'  AND ATTRIBUTE_ID = " + TableDataSetNode.ChildNodes[0].InnerText + "");
                        }
                        if (TableDataSetNode.ChildNodes[4].InnerText == "NONE")
                        {
                            DynamicSQl.Append(SQLstring);
                            break;
                        }
                        if (i > 0)
                        {
                            DynamicSQl.Append(" union ");
                        }
                        DynamicSQl.Append(SQLstring);
                    }
                }
            }
            DataSet DSfilter = new DataSet();
            oHelper.SQLString = DynamicSQl.ToString();
            if (DynamicSQl.ToString() != string.Empty)
            {
                DSfilter = oHelper.GetDataSet();
            }
            // prod_price_3 attribute list

            ChkAttributeExistinFamily();
            int loop = 0;
            CSRender oCSRender = new CSRender();

            //selecting all attributes

            DataSet AttrListDS = oCSRender.GetAttributes(CatalogID, this.UserID);
            int[] attrList = new int[AttrListDS.Tables[0].Rows.Count];
            foreach (DataRow oDR in AttrListDS.Tables[0].Rows)
            {
                attrList[loop] = oHelper.CI(oDR["ATTRIBUTE_ID"]);
                loop = loop + 1;
            }
            string con = oCon.ConnectionString.Substring(oCon.ConnectionString.IndexOf(";") + 1);
            TradingBell5.CatalogX.CatalogXfunction ofrm = new TradingBell5.CatalogX.CatalogXfunction();

            // here selecting catalog studio value for selected family

            DataSet tempdsPreview = ofrm.CatalogProductDetailsX(Convert.ToInt32(_catalogID), Convert.ToInt32(_familyID), attrList, true, con);
            //DsPreview = ofrm.CatalogProductPreviewX(Convert.ToInt32(_catalogID), Convert.ToInt32(_familyID), attrList, con);
            /* this code is for sorting the product */
            DataTable DSSS = new DataTable("ProductTable");
            foreach (DataColumn DC in tempdsPreview.Tables["ProductTable"].Columns)
            {
                //filling header 
                if (DC.Caption == "Sort")
                {
                    DSSS.Columns.Add(DC.Caption, typeof(System.Int32));
                }
                else
                {
                    DSSS.Columns.Add(DC.Caption);
                }
            }

            // checking product status is disable or not

            oHelper.SQLString = "SELECT TPF.PRODUCT_ID,SORT_ORDER FROM TB_PROD_FAMILY TPF, TBWC_INVENTORY TI WHERE TI.PRODUCT_ID = TPF.PRODUCT_ID AND TI.PRODUCT_STATUS <> 'DISABLE' AND FAMILY_ID=" + _familyID + " order by SORT_ORDER";
            DataSet DSsort = oHelper.GetDataSet();
            if (DSsort != null && DSsort.Tables[0] != null && DSsort.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow _rows in DSsort.Tables[0].Rows)
                {
                    foreach (DataRow row in tempdsPreview.Tables["ProductTable"].Select("PRODUCT_ID=" + Convert.ToInt32(_rows["PRODUCT_ID"])))
                        DSSS.ImportRow(row);
                }
            }
            else
            {
                foreach (DataRow row in tempdsPreview.Tables["ProductTable"].Rows)
                    DSSS.ImportRow(row);
            }
            DataRow[] tempDsRows = DSSS.Select("FAMILY_ID=" + _familyID, "FAMILY_ID");
            DsPreview = tempdsPreview.Clone();
            foreach (DataRow row in tempDsRows)
                DsPreview.Tables["ProductTable"].ImportRow(row);
            DataRow[] tempsDsRows = tempdsPreview.Tables["ExtendedProperties"].Select("Family_ID=" + _familyID, "Sort");
            foreach (DataRow row in tempsDsRows)
                DsPreview.Tables["ExtendedProperties"].ImportRow(row);
            /* this code is for sorting the product */

            //for filters 
            if (DSfilter != null && DSfilter.Tables.Count > 0)
            {
                if (DSfilter.Tables[0].Rows.Count > 0)
                {
                    DataSet temp = DsPreview;
                    DataSet Dspreviewaltered = new DataSet();
                    DataTable t1 = new DataTable();
                    Dspreviewaltered.Tables.Add(DsPreview.Tables[0].Clone());
                    foreach (DataRow DR in DsPreview.Tables[0].Rows)
                    {
                        DataRow[] DrfilteredRows = DSfilter.Tables[0].Select("PRODUCT_ID = " + DR["PRODUCT_ID"]);
                        if (DrfilteredRows != null && DrfilteredRows.Length != 0)
                        {
                            //DataRow datarw = t1.NewRow();
                            Dspreviewaltered.Tables[0].ImportRow(DR);
                        }
                    }
                    DsPreview = new DataSet();
                    DsPreview.Tables.Add(Dspreviewaltered.Tables[0].Clone());
                    foreach (DataRow DR in Dspreviewaltered.Tables[0].Rows)
                    {
                        DsPreview.Tables[0].ImportRow(DR);
                    }
                    DataTable dsf = temp.Tables[1].Clone();
                    foreach (DataRow Dr in temp.Tables[1].Rows)
                    {
                        DataRow DRTEMP = dsf.NewRow();
                        dsf.ImportRow(Dr);
                    }
                    DsPreview.Tables.Add(dsf);
                    //for Filters
                }
            }

            //for Publish

            //comparing publish attributes vs catalog studio attributes

            oHelper.SQLString = " SELECT ATTRIBUTE_NAME FROM TB_ATTRIBUTE WHERE ATTRIBUTE_ID IN ( SELECT DISTINCT ATTRIBUTE_ID FROM TB_PROD_FAMILY_ATTR_LIST WHERE FAMILY_ID = " + FamilyID + "  ) ";
            DataSet DSpublish = new DataSet();
            DSpublish = oHelper.GetDataSet();
            if (DSpublish != null)
            {
                if (DSpublish.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow Dr in DSpublish.Tables[0].Rows)
                    {
                        if (DsPreview.Tables[0].Columns.Contains(Dr[0].ToString()))
                        {
                            DsPreview.Tables[0].Columns[Dr[0].ToString()].Caption = "`" + DsPreview.Tables[0].Columns[Dr[0].ToString()].Caption;
                        }
                    }
                    for (int i = 0; i < DsPreview.Tables[0].Columns.Count; i++)
                    {
                        if (DsPreview.Tables[0].Columns[i].Caption.Contains("`") == false)
                        {
                            if (DsPreview.Tables[0].Columns[i].Caption != "CATALOG_ID" && DsPreview.Tables[0].Columns[i].Caption != "FAMILY_ID" && DsPreview.Tables[0].Columns[i].Caption != "PRODUCT_ID" && DsPreview.Tables[0].Columns[i].Caption != "Publish" && DsPreview.Tables[0].Columns[i].Caption != "Sort")
                            {
                                DsPreview.Tables[0].Columns.RemoveAt(i);
                                i = 0;
                            }
                        }
                    }
                    for (int i = 0; i < DsPreview.Tables[0].Columns.Count; i++)
                    {
                        if (DsPreview.Tables[0].Columns[i].Caption.Contains("`") == true)
                        {
                            if (DsPreview.Tables[0].Columns[i].Caption != "CATALOG_ID" && DsPreview.Tables[0].Columns[i].Caption != "FAMILY_ID" && DsPreview.Tables[0].Columns[i].Caption != "PRODUCT_ID" && DsPreview.Tables[0].Columns[i].Caption != "Publish" && DsPreview.Tables[0].Columns[i].Caption != "Sort")
                            {
                                
                                DsPreview.Tables[0].Columns[i].Caption = DsPreview.Tables[0].Columns[i].Caption.Remove(0, 1);
                            }
                        }
                    }
                }
            }

            //for publish
            if (DsPreview.Tables.Count == 2)
            {
                DataSet Dsp = DsPreview;
                DataTable et = new DataTable("ProductTable");
                foreach (DataColumn DC in Dsp.Tables[0].Columns)
                {
                    //if (DC.Caption != "CATALOG_ID" && DC.Caption != "Publish" && DC.Caption != "FAMILY_ID" && DC.Caption != "PRODUCT_ID" && DC.Caption != "Sort")
                    if (DC.Caption != "CATALOG_ID" && DC.Caption != "Publish" && DC.Caption != "FAMILY_ID" && DC.Caption != "Sort")
                        et.Columns.Add(DC.Caption, DC.DataType);
                }
                DataSet dstosuite = null;
                bool dstosuiteOpt = false;
                if (HttpContext.Current.Request["fid"] != null && HttpContext.Current.Request["fid"].ToString() != "" && HttpContext.Current.Request["fid"].ToString() == FamilyID.ToString() && HttpContext.Current.Request["sb"] != null && HttpContext.Current.Request["sb"].ToString() != "" && HttpContext.Current.Request["sm"] != null && HttpContext.Current.Request["sm"].ToString() != "" && HttpContext.Current.Request["pcr"] != null && HttpContext.Current.Request["pcr"].ToString() != "")
                {
                    oHelper.SQLString = "SELECT DISTINCT PRODUCT_ID FROM VIEW_WESTB_TOSUITE_BY_BRAND_PRODUCT WHERE FAMILY_ID=" + FamilyID.ToString() + " AND (SUBFAMILY_ID IS NULL OR SUBFAMILY_ID='') AND TOSUITE_BRAND='" + HttpContext.Current.Request["sb"].ToString() + "' AND TOSUITE_MODEL='" + HttpUtility.UrlDecode(HttpContext.Current.Request["sm"].ToString()) + "' AND CATALOG_ID=" + CatalogID.ToString() + " AND CATEGORY_ID='" + HttpContext.Current.Request["pcr"].ToString() + "'";
                    dstosuite = oHelper.GetDataSet();
                    if (dstosuite != null && dstosuite.Tables.Count > 0 && dstosuite.Tables[0].Rows.Count > 0)
                    {
                        dstosuitesubfamopt = true;
                    }
                    else
                    {
                        dstosuitesubfamopt = false;
                    }
                    dstosuiteOpt = true;

                }
                else if (HttpContext.Current.Request["fid"] != null && HttpContext.Current.Request["fid"].ToString() != "" && HttpContext.Current.Request["fid"].ToString() == FamilyID.ToString() && HttpContext.Current.Request["sb"] != null && HttpContext.Current.Request["sb"].ToString() != "" && HttpContext.Current.Request["pcr"] != null && HttpContext.Current.Request["pcr"].ToString() != "")
                {
                    oHelper.SQLString = "SELECT DISTINCT PRODUCT_ID FROM VIEW_WESTB_TOSUITE_BY_BRAND_PRODUCT WHERE FAMILY_ID=" + FamilyID.ToString() + " AND (SUBFAMILY_ID IS NULL OR SUBFAMILY_ID='') AND TOSUITE_BRAND='" + HttpContext.Current.Request["sb"].ToString() + "' AND CATALOG_ID=" + CatalogID.ToString() + " AND CATEGORY_ID='" + HttpContext.Current.Request["pcr"].ToString() + "'";
                    dstosuite = oHelper.GetDataSet();
                    if (dstosuite != null && dstosuite.Tables.Count > 0 && dstosuite.Tables[0].Rows.Count > 0)
                    {
                        dstosuitesubfamopt = true;
                    }
                    else
                    {
                        dstosuitesubfamopt = false;
                    }
                    dstosuiteOpt = true;
                }
                else if (HttpContext.Current.Request["fid"] != null && HttpContext.Current.Request["fid"].ToString() != "" && HttpContext.Current.Request["fid"].ToString() != FamilyID.ToString() && HttpContext.Current.Request["sb"] != null && HttpContext.Current.Request["sb"].ToString() != "" && HttpContext.Current.Request["sm"] != null && HttpContext.Current.Request["sm"].ToString() != "" && HttpContext.Current.Request["pcr"] != null && HttpContext.Current.Request["pcr"].ToString() != "")
                {
                    oHelper.SQLString = "SELECT DISTINCT PRODUCT_ID FROM VIEW_WESTB_TOSUITE_BY_BRAND_PRODUCT WHERE FAMILY_ID=" + HttpContext.Current.Request["fid"].ToString() + " AND SUBFAMILY_ID=" + FamilyID.ToString() + " AND TOSUITE_BRAND='" + HttpContext.Current.Request["sb"].ToString() + "' AND TOSUITE_MODEL='" + HttpUtility.UrlDecode(HttpContext.Current.Request["sm"].ToString()) + "' AND CATALOG_ID=" + CatalogID.ToString() + " AND CATEGORY_ID='" + HttpContext.Current.Request["pcr"].ToString() + "'";
                    dstosuite = oHelper.GetDataSet();
                    dstosuiteOpt = true;
                }
                else if (HttpContext.Current.Request["fid"] != null && HttpContext.Current.Request["fid"].ToString() != "" && HttpContext.Current.Request["fid"].ToString() != FamilyID.ToString() && HttpContext.Current.Request["sb"] != null && HttpContext.Current.Request["sb"].ToString() != "" && HttpContext.Current.Request["pcr"] != null && HttpContext.Current.Request["pcr"].ToString() != "")
                {
                    oHelper.SQLString = "SELECT DISTINCT PRODUCT_ID FROM VIEW_WESTB_TOSUITE_BY_BRAND_PRODUCT WHERE FAMILY_ID=" + HttpContext.Current.Request["fid"].ToString() + " AND SUBFAMILY_ID=" + FamilyID.ToString() + " AND TOSUITE_BRAND='" + HttpContext.Current.Request["sb"].ToString() + "' AND CATALOG_ID=" + CatalogID.ToString() + " AND CATEGORY_ID='" + HttpContext.Current.Request["pcr"].ToString() + "'";
                    dstosuite = oHelper.GetDataSet();
                    dstosuiteOpt = true;
                }
                string tosuiteproduct = "";
                if (dstosuite != null && dstosuite.Tables.Count > 0 && dstosuite.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow drtosuite in dstosuite.Tables[0].Rows)
                    {
                        if (tosuiteproduct == "")
                        {
                            tosuiteproduct = drtosuite[0].ToString();
                        }
                        else
                        {
                            tosuiteproduct = tosuiteproduct + "," + drtosuite[0].ToString();
                        }
                    }

                }
                if (tosuiteproduct != "")
                {
                    DataRow[] drRtosuite = Dsp.Tables[0].Select("PRODUCT_ID IN(" + tosuiteproduct + ")");
                    foreach (DataRow DR in drRtosuite)
                        et.ImportRow(DR);
                }
                else
                {
                    if (dstosuiteOpt == false)
                    {
                        foreach (DataRow DR in Dsp.Tables[0].Rows)
                            et.ImportRow(DR);
                    }
                    else if (dstosuiteOpt == true)
                    {
                        if (dstosuitesubfamopt == false)
                        {
                            return HTMLString;
                        }
                        else
                        {
                            foreach (DataRow DR in Dsp.Tables[0].Rows)
                                et.ImportRow(DR);
                        }
                    }
                }
                DsPreview = new DataSet();
                DsPreview.Tables.Add(et);
                DsPreview.Tables.Add(Dsp.Tables[1].Clone());
                foreach (DataRow DR in Dsp.Tables[1].Rows)
                    DsPreview.Tables[1].ImportRow(DR);
                // if (DsPreview.Tables[1].Rows.Count >= 2 && DsPreview.Tables[1].Columns.Count > 6)
                chkAttrType = new int[DsPreview.Tables[1].Columns.Count - 5];
                for (int attrtypeCount = 0; attrtypeCount < chkAttrType.Length; attrtypeCount++)
                {
                    if (DsPreview.Tables[1].Rows.Count > 0)
                    {
                        if (DsPreview.Tables[1].Rows[1].ItemArray[5 + attrtypeCount].ToString() != "")
                        {
                            chkAttrType[attrtypeCount] = Convert.ToInt32(DsPreview.Tables[1].Rows[1].ItemArray[5 + attrtypeCount].ToString());
                        }
                    }
                }



                if (tableLayout == "Pivot")
                {
                    HTMLString = GeneratePivotHTML();
                }
                else if (tableLayout == "Grouped")
                {
                    DataSet oGroupDS = new DataSet();
                    string[] GrpColNames;
                    GrpColNames = getcoluumns();
                    if (GrpColNames.Length != 0)
                    {
                        oGroupDS = BuildGroupedDSTest(DsPreview);
                        HTMLString = BuildGroupedHtml(oGroupDS);

                    }
                    else
                    {
                        HTMLString = GenerateHorizontalHTML();
                    }
                }
                else if (tableLayout == "Horizontal" || tableLayout == "")
                {
                    HTMLString = this.GenerateHorizontalHTML();
                }
                else if (tableLayout == "Vertical")
                {
                    HTMLString = this.GenerateHorizontalHTML();  // This has been overrided to Horizontal Table DO NOT MAKE ANY CHANGES. IF ANY ONE CHANGE THIS CODE PLEASE CHECK WITH ANAND / NATHAN for the Price Table & Buy Button logics
                    //HTMLString = this.GenerateVerticalHTML();
                }
                else if (tableLayout.Trim() == string.Empty)
                {
                    HTMLString = "<P style=\"font-size: x-medium; color: red; font-family: Verdana;\"> INVALID LAYOUT SPECIFICATION ! </P>";
                }
            }
            return HTMLString;
        }

        /// <summary>
        /// This is used to check Attribute Exist in a Family
        /// </summary>
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
        ///    ...
        ///    DataSet DSfilter = new DataSet();
        ///    oHelper.SQLString = DynamicSQl.ToString();
        ///    if (DynamicSQl.ToString() != string.Empty)
        ///    {
        ///        DSfilter = oHelper.GetDataSet();
        ///    }
        ///    ChkAttributeExistinFamily(); 
        /// }
        /// </code>
        /// </example>
        [WebMethod]
        private void ChkAttributeExistinFamily()
        {
            DataSet OdsCalc = new DataSet();
            DataSet Dschk = new DataSet();
            string AttrCalcFormaula = string.Empty;
            ContinueCalclautedCols = 0;
            string TempStr = string.Empty;
            CheckCalculatedCtr = 0;
            oHelper.SQLString = " SELECT ATTRIBUTE_CALC_FORMULA FROM TB_ATTRIBUTE WHERE IS_CALCULATED =1  AND ATTRIBUTE_CALC_FORMULA <> '' " +
                            "  AND ATTRIBUTE_ID IN (SELECT DISTINCT ATTRIBUTE_ID FROM TB_PROD_SPECS WHERE PRODUCT_ID IN " +
                            "  (SELECT PRODUCT_ID FROM TB_PROD_FAMILY WHERE FAMILY_ID = " + FamilyID + "))";
            OdsCalc = oHelper.GetDataSet();
            List<string> attrList = new List<string>();
            ///////////My Code
            if (OdsCalc != null)
            {
                foreach (DataRow Dr in OdsCalc.Tables[0].Rows)
                {
                    AttrCalcFormaula = Dr[0].ToString();
                    string tempAttrEq = AttrCalcFormaula;

                    while (tempAttrEq.Contains("["))
                    {
                        int startIndex = tempAttrEq.IndexOf('[');
                        int endIndex = tempAttrEq.IndexOf(']');
                        attrList.Add(tempAttrEq.Substring(startIndex + 1, endIndex - startIndex - 1));
                        tempAttrEq = tempAttrEq.Substring(endIndex + 1, tempAttrEq.Length - endIndex - 1);
                    }
                }
            }
            for (int j = 0; j < attrList.Count; j++)
            {
                oHelper.SQLString = " SELECT ATTRIBUTE_NAME FROM TB_ATTRIBUTE TA WHERE ATTRIBUTE_ID IN ( SELECT  DISTINCT ATTRIBUTE_ID FROM TB_PROD_SPECS WHERE PRODUCT_ID IN  " +
                                " (SELECT PRODUCT_ID FROM TB_PROD_fAMILY WHERE FAMILY_ID = " + FamilyID + ")) AND TA.ATTRIBUTE_NAME = '" + attrList[j] + "' ";
                Dschk = oHelper.GetDataSet();
                if (Dschk.Tables[0].Rows.Count > 0)
                {
                    CheckCalculatedCtr++;
                }
            }

            if (CheckCalculatedCtr == attrList.Count)
            {
                ContinueCalclautedCols = 1;
            }
        }

        /// <summary>
        /// This is used to Fetch Table Layout
        /// </summary>
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
        ///    ...
        ///    _valInc = 0;
        ///    string tableLayout = this.FetchTableLayout();
        ///    string HTMLString = string.Empty;
        /// }
        /// </code>
        /// </example>
        [WebMethod]
        private string FetchTableLayout()
        {
            string LayoutXML = "HORIZONTAL TABLE";
            //SystemSettingsCollection SettingMemebers = SystemSettingsConfiguration.GetConfig.Members;
            string TempFilepath = ""; //SettingMemebers.GetValue(SystemSettingsCollection.SettingsList.TEMPORARYPATH.ToString());
            string layoutType = string.Empty;
            DataSet DSlayout = new DataSet();
            oHelper.SQLString = "  SELECT PRODUCT_TABLE_STRUCTURE FROM TB_FAMILY WHERE FAMILY_ID = " + _familyID + "";
            DSlayout = oHelper.GetDataSet();

            if (DSlayout.Tables[0].Rows[0][0].ToString() != "")
            {
                LayoutXML = DSlayout.Tables[0].Rows[0].ItemArray[0].ToString();
                if (LayoutXML != null || LayoutXML != string.Empty)
                {
                    XmlDocument xmlDOc = new XmlDocument();
                    xmlDOc.LoadXml(LayoutXML);
                    XmlNode rootNode = xmlDOc.DocumentElement;
                    layoutType = rootNode.Attributes["TableType"].Value;

                    if (rootNode.Attributes["TableType"].Value == "Grouped")
                    {
                        XmlNodeList xmlNodeList;
                        xmlNodeList = rootNode.ChildNodes;

                        for (int i = 0; i < xmlNodeList.Count; i++)
                        {
                            if (xmlNodeList[i].Name == "LayoutXML")
                            {
                                if (xmlNodeList[i].ChildNodes.Count > 0)
                                {
                                    LayoutXML = xmlNodeList[i].ChildNodes[0].Value;
                                }
                            }
                        }
                    }
                    if (LayoutXML != null || LayoutXML != string.Empty)
                    {
                        //FileStream FileHtml = new FileStream(Application.StartupPath + "\\Layout.xml", FileMode.Create);
                        FileStream FileHtml = new FileStream(Server.MapPath("").ToString() + "\\Layout.xml", FileMode.Create);
                        StreamWriter Strwriter = new StreamWriter(FileHtml);
                        Strwriter.Write(LayoutXML);
                        Strwriter.Close();
                        FileHtml.Close();
                        //obj.DisplayLayout.LoadFromXml(TempFilepath + "\\Layout.xml");
                    }
                }
            }
            if (layoutType.Length == 0)
                layoutType = "Horizontal";
            return layoutType;
        }


        private bool IsEcomenabled()
        {
            bool retvalue = false;
            string userid = HttpContext.Current.Session["USER_ID"].ToString();
            string websiteid = System.Configuration.ConfigurationManager.AppSettings["WEBSITEID"].ToString();
            string sSQL = "SELECT USER_ROLE FROM TBWC_COMPANY_BUYERS WHERE WEBSITE_ID = " + websiteid + " and USER_ID = " + userid;
            oHelper.SQLString = sSQL;
            int iROLE = oHelper.CI(oHelper.GetValue("USER_ROLE"));
            if (iROLE <= 3)
                retvalue = true;
            return retvalue;
        }


        /// <summary>
        /// This is used to Generate Horizontal HTML Layout
        /// </summary>
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
        ///    ...
        ///    else if (tableLayout == "Horizontal" || tableLayout == "")
        ///    {
        ///        HTMLString = this.GenerateHorizontalHTML();
        ///    }
        /// }
        /// </code>
        /// </example>
        [WebMethod]
        #region comments
        //private string GenerateHorizontalHTML()
        //{
        //    //ServiceProvider.ProductValidationServices Oservices = new TradingBell5.CatalogStudio.ServiceProvider.ProductValidationServices();
        //    string EcomState = oHelper.GetOptionValues("ECOMMERCEENABLED").ToString();
        //    DataSet dsBgDisc = new DataSet();
        //    decimal untPrice = 0;
        //    string AttrID = string.Empty;
        //    string HypColumn = "";
        //    int Min_ord_qty;
        //    int Qty_avail;
        //    //int ProdID;
        //    int AttrType;
        //    string NavColumn = oHelper.GetOptionValues("NAVIGATIONCOLUMN").ToString();
        //    string HypCURL = oHelper.GetOptionValues("NAVIGATIONURL").ToString();
        //    StringBuilder strBldr = new StringBuilder();

        //    strBldr.Append("<HTML>");
        //    strBldr.Append("<CENTER>");
        //    //strBldr.Append("<HEAD>");
        //    //strBldr.Append("</HEAD>");
        //    strBldr.Append("<TABLE border=0 cellspacing=1 style=\"background-color:black\" cellpadding=3>");
        //    strBldr.Append("<style>td{font-family:arial Unicode ms;font-size:12px;}th{font-family:arial unicode ms;font-size:12px;font-weight:Bold}</style>");
        //    if (DisplayHeaders == true)
        //    {
        //        //bool AddColWidth = false;
        //        strBldr.Append("<TR>");
        //        for (int j = 1; j < DsPreview.Tables[0].Columns.Count; j++)
        //        {
        //            strBldr.Append("<TD ALIGN=\"Center\" VALIGN=\"Middle\" style=\"width: 200px; color: Black; BACKGROUND-COLOR: #99CCFF\" >");
        //            strBldr.Append(DsPreview.Tables[0].Columns[j].Caption);
        //            strBldr.Append("</TD>");

        //        }
        //        if (EcomState == "YES")
        //        {
        //            strBldr.Append("<TD ALIGN=\"Center\" VALIGN=\"Middle\" style=\"width: 200px;\" >Shipping</TD>");
        //            strBldr.Append("<TD ALIGN=\"Center\" VALIGN=\"Middle\" style=\"width: 200px;\" >Cart</TD>");
        //        }
        //        strBldr.Append("</TR>");
        //        //strBldr.Append("<TR>");
        //        //strBldr.Append("<TR>");
        //    }
        //    string ValueFortag = string.Empty;
        //    for (int i = 0; i < DsPreview.Tables[0].Rows.Count; i++)
        //    {
        //        strBldr.Append("<TR>");
        //        for (int j = 1; j < DsPreview.Tables[0].Columns.Count; j++)
        //        {

        //            string alignVal = "LEFT";
        //            AttrID = DsPreview.Tables[1].Rows[0].ItemArray[5 + j].ToString();
        //            ExtractCurrenyFormat(Convert.ToInt32(AttrID));
        //            oHelper.SQLString = "SELECT ATTRIBUTE_DATATYPE FROM TB_ATTRIBUTE WHERE ATTRIBUTE_ID = " + AttrID;
        //            DataSet DSS = oHelper.GetDataSet();
        //            oHelper.SQLString = "SELECT ATTRIBUTE_TYPE FROM TB_ATTRIBUTE WHERE ATTRIBUTE_NAME= '" + DsPreview.Tables[0].Columns[j].ToString() + "'";
        //            AttrType = oHelper.CI(oHelper.GetValue("ATTRIBUTE_TYPE").ToString());
        //            //if (chkAttrType[j] == 4 || DSS.Tables[0].Rows[0].ItemArray[0].ToString().Substring(0, 3).ToUpper() == "NUM")
        //            //{
        //            //    alignVal = "RIGHT";

        //            //}
        //            if (AttrType == 4 || DSS.Tables[0].Rows[0].ItemArray[0].ToString().Substring(0, 3).ToUpper() == "NUM")
        //            {
        //                alignVal = "RIGHT";
        //            }
        //            if (AttrType == 3)
        //                strBldr.Append("<TD ALIGN=\"" + alignVal + getCellString(DsPreview.Tables[0].Rows[i][j].ToString()));
        //            else  //if (chkAttrType[j] == 4)
        //            {
        //                if ((Headeroptions == "All") || (Headeroptions != "All" && i == 0))
        //                {
        //                    if ((EmptyCondition == "Null" || EmptyCondition == "Empty" || EmptyCondition == null) && (DsPreview.Tables[0].Rows[i][j].ToString() == string.Empty))
        //                    {
        //                        ValueFortag = ReplaceText;
        //                    }
        //                    else if ((DsPreview.Tables[0].Rows[i][j].ToString()) == (EmptyCondition))
        //                    {
        //                        ValueFortag = ReplaceText;
        //                    }
        //                    else
        //                    {
        //                        if (Isnumber(DsPreview.Tables[0].Rows[i][j].ToString().Replace("\r", "<br/>").Replace("\n", "&nbsp;")) == true)
        //                        {
        //                            ValueFortag = Prefix + " " + Convert.ToDouble(DsPreview.Tables[0].Rows[i][j].ToString().Replace("\r", "<br/>").Replace("\n", "&nbsp;")).ToString() + " " + Suffix;
        //                        }
        //                        else
        //                        {
        //                            if (DsPreview.Tables[0].Rows[i][j].ToString().Length > 0)
        //                            {
        //                                ValueFortag = Prefix + " " + DsPreview.Tables[0].Rows[i][j].ToString().Replace("\r", "<br/>").Replace("\n", "&nbsp;") + " " + Suffix;
        //                            }
        //                            else
        //                            {
        //                                ValueFortag = string.Empty;
        //                            }
        //                        }
        //                    }
        //                }
        //                else
        //                {
        //                    if (Isnumber(DsPreview.Tables[0].Rows[i][j].ToString().Replace("\r", "<br/>").Replace("\n", "&nbsp;")) == true)
        //                    {
        //                        ValueFortag = Convert.ToDouble(DsPreview.Tables[0].Rows[i][j].ToString().Replace("\r", "<br/>").Replace("\n", "&nbsp;")).ToString();
        //                    }
        //                    else
        //                    {
        //                        ValueFortag = DsPreview.Tables[0].Rows[i][j].ToString().Replace("\r", "<br/>").Replace("\n", "&nbsp;");
        //                    }
        //                }
        //                if (DsPreview.Tables[0].Columns[j].Caption.ToLower() == NavColumn.ToLower().ToString())
        //                {
        //                    ProdID = oHelper.CI(DsPreview.Tables[0].Rows[i][0].ToString());
        //                    HypColumn = HypCURL.Replace("{PRODUCT_ID}", ProdID.ToString());
        //                    Min_ord_qty = oHelper.CI(oOrder.GetProductMinimumOrderQty(ProdID));
        //                    HypColumn = HypColumn.Replace("{MIN_ORD_QTY}", Min_ord_qty.ToString());
        //                    Qty_avail = oHelper.CI(oOrder.GetProductAvilableQty(ProdID));
        //                    HypColumn = HypColumn.Replace("{QTY_AVAIL}", Qty_avail.ToString());
        //                    HypColumn = HypColumn.Replace("{FAMILY_ID}", this.FamilyID.ToString());

        //                    ValueFortag = "<A HREF=\"" + HypColumn + "\" > " + ValueFortag + "</A>";
        //                }
        //                if (AttrType == 4)
        //                {
        //                    if (UserID > 0)
        //                    {

        //                        dsBgDisc = oBG.GetBuyerGroupBasedDiscountDetails(oBG.GetBuyerGroup(UserID));
        //                    }
        //                    else
        //                    {
        //                        dsBgDisc = oBG.GetBuyerGroupBasedDiscountDetails("DEFAULTBG");
        //                    }

        //                    if (dsBgDisc != null)
        //                    {
        //                        if (dsBgDisc.Tables[0].Rows.Count > 0)
        //                        {
        //                            decimal DiscVal = oHelper.CDEC(dsBgDisc.Tables[0].Rows[0].ItemArray[0].ToString());
        //                            DateTime ValidDt = Convert.ToDateTime(dsBgDisc.Tables[0].Rows[0].ItemArray[1].ToString());
        //                            string CalMth = dsBgDisc.Tables[0].Rows[0].ItemArray[2].ToString();
        //                            untPrice = oHelper.CDEC(DsPreview.Tables[0].Rows[i][j].ToString());
        //                            if (ValidDt.CompareTo(DateTime.Today) >= 0 && DiscVal > 0)
        //                            {
        //                                ValueFortag = oBG.CalculateBGDiscountPrice(untPrice, DiscVal, CalMth).ToString();
        //                            }
        //                        }
        //                    }

        //                    ValueFortag = oHelper.GetOptionValues("CURRENCYFORMAT").ToString() + ValueFortag;
        //                }

        //                if (DsPreview.Tables[0].Columns[j].ToString().Contains("Price"))
        //                {
        //                    //<td width="20%" class="price" style="height: 19px">&nbsp;</td>
        //                    strBldr.Append("<TD ALIGN=\"" + alignVal + "\" VALIGN=\"Middle\" style=\"width: 200px; color: Black; BACKGROUND-COLOR: white  \" >" + ValueFortag + "</TD>");
        //                    //strBldr.Append("<TD ALIGN=\"left\" VALIGN=\"TOP\">" + ValueFortag + "</TD>");
        //                }
        //                else
        //                {
        //                    //Fixed the column witdh for table
        //                    if (ValueFortag.ToString().Length >= 9 && ValueFortag.ToString().Length <= 25)
        //                    {
        //                        //DesciptionMidumAttributeWidth = 80;
        //                        ValueFortag = ValueFortag.Replace("<br/>", "");
        //                        ValueFortag = ValueFortag.Replace("&nbsp;", "");
        //                        strBldr.Append("<TD ALIGN=\"left\"VALIGN=\"TOP\" BACKGROUND-COLOR: white width=\"" + DesciptionMidumAttributeWidth + "px\" NOWRAP>" + ValueFortag + "</TD>");
        //                    }
        //                    else if (ValueFortag.ToString().Length >= 26 && ValueFortag.ToString().Length <= 50)
        //                    {
        //                        //DesciptionNormalAttributeWidth = 140;
        //                        ValueFortag = ValueFortag.Replace("<br/>", "");
        //                        ValueFortag = ValueFortag.Replace("&nbsp;", "");
        //                        strBldr.Append("<TD ALIGN=\"left\"VALIGN=\"TOP\" BACKGROUND-COLOR: white width=\"" + DesciptionNormalAttributeWidth + "px\" NOWRAP>" + ValueFortag + "</TD>");
        //                    }
        //                    else if (ValueFortag.ToString().Length >= 51 && ValueFortag.ToString().Length <= 100)
        //                    {
        //                        //DesciptionHighAttributeWidth = 180;
        //                        ValueFortag = ValueFortag.Replace("<br/>", "");
        //                        ValueFortag = ValueFortag.Replace("&nbsp;", "");
        //                        strBldr.Append("<TD ALIGN=\"left\"VALIGN=\"TOP\" BACKGROUND-COLOR: white width=\"" + DesciptionHighAttributeWidth + "px\" NOWRAP>" + ValueFortag + "</TD>");
        //                    }
        //                    else if (ValueFortag.ToString().Length > 100)
        //                    {
        //                        ValueFortag = ValueFortag.Replace("<br/>", "");
        //                        ValueFortag = ValueFortag.Replace("&nbsp;", "");
        //                        strBldr.Append("<TD ALIGN=\"left\"VALIGN=\"TOP\" BACKGROUND-COLOR: white width=\"" + DesciptionAttributeWidth + "px\" NOWRAP>" + ValueFortag + "</TD>");
        //                    }
        //                    else
        //                    {

        //                        strBldr.Append("<TD ALIGN=\"" + alignVal + "\" VALIGN=\"Middle\" style=\"width: 200px; color: Black; BACKGROUND-COLOR: white  \" >" + ValueFortag + "</TD>");
        //                        //strBldr.Append("<TD ALIGN=\"left\"VALIGN=\"TOP\" BACKGROUND-COLOR: white>" + ValueFortag + "</TD>");
        //                    }

        //                }
        //            }

        //            //else
        //            //{
        //            //    strBldr.Append("<TD ALIGN=\"" + alignVal + "\" VALIGN=\"Middle\" style=\"width: 200px; color: Black; BACKGROUND-COLOR: white  \" >" + DsPreview.Tables[0].Rows[i][j].ToString().Replace("\r", "<br/>").Replace("\n", "&nbsp;") + "</TD>");
        //            //}

        //            //Add the Shipping and Cart Images
        //            if (EcomState == "YES")
        //            {
        //                //strBldr.Append("<TR>");
        //                //strBldr.Append("<TD ALIGN=\"" + alignvalue + "\" VALIGN=\"Middle\" style=\"width: 200px; color: Black; BACKGROUND-COLOR: #99CCFF  \" > Shipping </TD>");
        //                //strBldr.Append("<TR>");
        //                //strBldr.Append("<TD ALIGN=\"" + alignvalue + "\" VALIGN=\"Middle\" style=\"width: 200px; color: Black; BACKGROUND-COLOR: #99CCFF  \"> Cart </TD>");
        //                //strBldr.Append("<TD ALIGN=\"" + alignVal + "\" VALIGN=\"Middle\" style=\"width: 200px; color: Black; BACKGROUND-COLOR: #99CCFF  \" > Shipping </TD>");
        //                //strBldr.Append("<TD ALIGN=\"" + alignVal + "\" VALIGN=\"Middle\" style=\"width: 200px; color: Black; BACKGROUND-COLOR: #99CCFF  \"> Cart </TD>");
        //                DsPreview.Tables[0].Columns.Add("Cart");
        //                DsPreview.Tables[0].Columns.Add("Shipping");   

        //                if (j == DsPreview.Tables[0].Columns.Count - 1)
        //                {

        //                    ProdID = oHelper.CI(DsPreview.Tables[0].Rows[i][0].ToString());
        //                    Boolean IsShipping = oOrder.GetProductIsShipping(ProdID);
        //                    string ShipImgPath = "";
        //                    if (IsShipping == true)
        //                    {
        //                        ShipImgPath = oHelper.GetOptionValues("IMAGE PATH").ToString() + oHelper.GetOptionValues("SHIPPING IMAGE").ToString();
        //                        string ShipUrl = oHelper.GetOptionValues("SHIP URL").ToString();
        //                        ShipImgPath = "<A HREF=\"" + ShipUrl + "\" style=\"text-decoration:none\"><IMG SRC=\"" + ShipImgPath + "\" style=\"border-width:0\"></A>";
        //                    }
        //                    else if (IsShipping == false)
        //                    {
        //                        ShipImgPath = oHelper.GetOptionValues("IMAGE PATH").ToString() + oHelper.GetOptionValues("NO SHIPPING IMAGE").ToString();
        //                        ShipImgPath = "<IMG SRC=\"" + ShipImgPath + "\" style=\"border-width:0\">";
        //                    }

        //                    //strBldr.Append("<TD ALIGN=\"Center\" VALIGN=\"Middle\" style=\"width: 200px; color: Black; BACKGROUND-COLOR: #99CCFF\" >");
        //                    strBldr.Append("<TD ALIGN=\"Center\" VALIGN=\"Middle\"  style=\"width: 200px; BACKGROUND-COLOR: white  \">" + ShipImgPath + "</TD>");
        //                    //strBldr.Append("<TD ALIGN=\"Center\" VALIGN=\"Middle\"  style=\"width: 200px; color: Black; BACKGROUND-COLOR: white; \">" + ShipImgPath + "</TD>");



        //                    //Add the Cart Image
        //                    string CartImgPath = "";
        //                    //ProdID = oHelper.CI(sourceTable.Rows[i - (columnTable.Columns.Count + 1)]["PRODUCT_ID"].ToString());
        //                    if (Restricted.ToUpper() == "YES")
        //                    {
        //                        CartImgPath = oHelper.GetOptionValues("RESTRICTED PRODUCT TEXT");
        //                        string CartUrl = oHelper.GetOptionValues("RESTRICTED PRODUCT URL").ToString();
        //                        CartImgPath = "<A HREF=\"" + CartUrl + "\" style=\"text-decoration:none\">" + CartImgPath + " </A>";
        //                    }
        //                    else
        //                    {
        //                        CartImgPath = oHelper.GetOptionValues("IMAGE PATH").ToString() + oHelper.GetOptionValues("CARTIMGPATH").ToString();
        //                        Min_ord_qty = oOrder.GetProductMinimumOrderQty(ProdID);
        //                        string CartUrl = oHelper.GetOptionValues("CARTURL").ToString();
        //                        CartUrl = CartUrl.Replace("{PRODUCT_ID}", ProdID.ToString());
        //                        CartUrl = CartUrl.Replace("{MIN_ORD_QTY}", Min_ord_qty.ToString());
        //                        CartImgPath = "<A HREF=\"" + CartUrl + "\" style=\"text-decoration:none\"><IMG SRC=\"" + CartImgPath + "\" style=\"border-width:0\"></A>";
        //                    }

        //                    strBldr.Append("<TD ALIGN=\"Center\" VALIGN=\"Middle\"  style=\"width: 200px; BACKGROUND-COLOR: white  \">" + CartImgPath + "</TD>");
        //                    //strBldr.Append("<TD ALIGN=\"Center\" VALIGN=\"TOP\"  style=\"width: 200px; color: Black; BACKGROUND-COLOR: white  \">" + CartImgPath + "</TD>");
        //                }
        //            }

        //        }
        //        strBldr.Append("</TR>");
        //    }
        //    strBldr.Append("</TABLE>");
        //    strBldr.Append("<CENTER>");
        //    strBldr.Append("</HTML>");
        //    //strBldr.Append("</TR>");
        //    //strBldr.Append("</TABLE>");
        //    //strBldr.Append("</TD>");
        //    //strBldr.Append("<td background=\"Images/bg/right.jpg\" class=\"norepeatbg3\" style=\"width: 17px\">&nbsp;</td>");
        //    //strBldr.Append("</TR>");
        //    //strBldr.Append("<tr valign=\"top\">");
        //    //strBldr.Append("<td style=\"width: 17px; height: 19px;\"><img src=\"images/bg/bot_left.jpg\" width=\"16\" height=\"15\"></td>");
        //   // strBldr.Append("<td background=\"images/bg/bot_bg.jpg\" class=\"norepeatbg\" style=\"height: 19px\"></td>");
        //    //strBldr.Append("<td style=\"width: 16px; height: 19px;\"><img src=\"images/bg/bot_right.jpg\" width=\"16\" height=\"15\"></td>");
        //   // strBldr.Append("</tr>");
        //   // strBldr.Append("</TABLE>");
        //    //strBldr.Append("</TD>"); strBldr.Append("</TR>"); strBldr.Append("</TABLE>");
        //    //strBldr.Append("</BODY>");
        //    //strBldr.Append("</HTML>");
        //    if (DsPreview.Tables[0].Rows.Count == 0)
        //    {
        //        strBldr.Remove(0, strBldr.ToString().Length);
        //    }
        //    return strBldr.ToString();
        //}
        #endregion
        private string GenerateHorizontalHTML()
        {
            //ServiceProvider.ProductValidationServices Oservices = new TradingBell5.CatalogStudio.ServiceProvider.ProductValidationServices();
            DataSet dsBgDisc = new DataSet();
            decimal untPrice = 0;
            string AttrID = string.Empty;
            string HypColumn = "";
            int Min_ord_qty = 0;
            int Qty_avail;
            int flagtemp = 0;
            //int ProdID;
            int AttrType;
            string NavColumn = oHelper.GetOptionValues("NAVIGATIONCOLUMN").ToString();
            string HypCURL = oHelper.GetOptionValues("NAVIGATIONURL").ToString();
            string EComState = oHelper.GetOptionValues("ECOMMERCEENABLED").ToString();
            if (EComState == "YES")
                if (!IsEcomenabled())
                    EComState = "NO";
            StringBuilder strBldr = new StringBuilder();

            strBldr.Append("<table width=\"100%\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\"><tr> <td align=\"right\" ><TABLE width=\"99%\" border=0 cellspacing=1 style=\"background-color:black\" cellpadding=3>");
            strBldr.Append("<style>td{font-family:arial Unicode ms;font-size:12px;}th{font-family:arial unicode ms;font-size:12px;font-weight:Bold}</style>");

            string userid = HttpContext.Current.Session["USER_ID"].ToString();
            string sSQL = string.Format("select price_code from wes_customer where wes_customer_id in (select company_id from tbwc_company_buyers where user_id = {0})", userid);
            oHelper.SQLString = sSQL;
            int pricecode = oHelper.CI(oHelper.GetValue("price_code"));

            if (DisplayHeaders == true)
            {
                strBldr.Append("<TR>");
                for (int j = 1; j < DsPreview.Tables[0].Columns.Count; j++)
                {
                    oHelper.SQLString = "SELECT ATTRIBUTE_TYPE FROM TB_ATTRIBUTE WHERE ATTRIBUTE_NAME= '" + DsPreview.Tables[0].Columns[j].ToString() + "'";
                    AttrType = oHelper.CI(oHelper.GetValue("ATTRIBUTE_TYPE").ToString());
                    if (AttrType != 3)
                    {
                        strBldr.Append("<TD ALIGN=\"Center\" VALIGN=\"Middle\" style=\"width: 200px; color:white; BACKGROUND-COLOR: #649bd4   \" >");

                        if (pricecode == 1)
                        {
                            strBldr.Append(DsPreview.Tables[0].Columns[j].Caption + (AttrType == 4 ? " Inc GST" : ""));
                        }
                        else
                        {
                            strBldr.Append(DsPreview.Tables[0].Columns[j].Caption + (AttrType == 4 ? " Ex GST" : ""));
                        }

                        strBldr.Append("</TD>");
                    }
                }
                if (DsPreview.Tables[0].Rows.Count > 0)
                    strBldr.Append("<TD ALIGN=\"Center\" VALIGN=\"Middle\" style=\"width: 200px; color: white; BACKGROUND-COLOR: #649bd4   \" >More Info</TD>");
                if (EComState.ToUpper() == "YES" && DsPreview.Tables[0].Rows.Count > 0)
                {
                    strBldr.Append("<TD ALIGN=\"Center\" VALIGN=\"Middle\" style=\"width: 200px; color: white; BACKGROUND-COLOR: #649bd4   \" >Cart</TD>");
                }
                strBldr.Append("</TR>");
            }
            string ValueFortag = string.Empty;
            bool rowcolor = false;
            for (int i = 0; i < DsPreview.Tables[0].Rows.Count; i++)
            {
                strBldr.Append("<TR>");
                if (rowcolor == false && i != 0)
                {
                    rowcolor = true;
                }
                else if (rowcolor == true)
                {
                    rowcolor = false;
                }
                for (int j = 1; j < DsPreview.Tables[0].Columns.Count; j++)
                {
                    string alignVal = "LEFT";
                    AttrID = DsPreview.Tables[1].Rows[0][DsPreview.Tables[0].Columns[j].ToString()].ToString();
                    ExtractCurrenyFormat(Convert.ToInt32(AttrID));
                    oHelper.SQLString = "SELECT ATTRIBUTE_DATATYPE FROM TB_ATTRIBUTE WHERE ATTRIBUTE_ID = " + AttrID;
                    DataSet DSS = oHelper.GetDataSet();
                    oHelper.SQLString = "SELECT ATTRIBUTE_TYPE FROM TB_ATTRIBUTE WHERE ATTRIBUTE_NAME= '" + DsPreview.Tables[0].Columns[j].ToString() + "'";
                    AttrType = oHelper.CI(oHelper.GetValue("ATTRIBUTE_TYPE").ToString());
                    //if (chkAttrType[j] == 4 || DSS.Tables[0].Rows[0].ItemArray[0].ToString().Substring(0, 3).ToUpper() == "NUM")
                    //{
                    //    alignVal = "RIGHT";

                    //}
                    if (AttrType == 4 || DSS.Tables[0].Rows[0].ItemArray[0].ToString().Substring(0, 3).ToUpper() == "NUM")
                    {
                        alignVal = "RIGHT";
                    }
                    if (AttrType == 3) { }
                    // strBldr.Append("<TD ALIGN=\"" + alignVal + getCellString(DsPreview.Tables[0].Rows[i][j].ToString()));
                    else  //if (chkAttrType[j] == 4)
                    {
                        if ((Headeroptions == "All") || (Headeroptions != "All" && i == 0))
                        {
                            if ((EmptyCondition == "Null" || EmptyCondition == "Empty" || EmptyCondition == null) && (DsPreview.Tables[0].Rows[i][j].ToString() == string.Empty))
                            {
                                ValueFortag = ReplaceText;
                            }
                            else if ((DsPreview.Tables[0].Rows[i][j].ToString()) == (EmptyCondition))
                            {
                                ValueFortag = ReplaceText;
                            }
                            else
                            {
                                if (Isnumber(DsPreview.Tables[0].Rows[i][j].ToString().Replace("\r", "<br/>").Replace("\n", "&nbsp;")) == true)
                                {
                                    if (AttrType == 4)
                                    {
                                        int _prodid = System.Convert.ToInt32(DsPreview.Tables[0].Rows[i][0].ToString());

                                        ValueFortag = Prefix + " " + oHelper.FixDecPlace(Convert.ToDecimal(GetMyPrice(_prodid))).ToString() + " " + Suffix;

                                    }
                                    else
                                    {
                                        ValueFortag = Prefix + " " + DsPreview.Tables[0].Rows[i][j].ToString().Replace("\r", "<br/>").Replace("\n", "&nbsp;") + " " + Suffix;
                                    }
                                }
                                else
                                {
                                    if (DsPreview.Tables[0].Rows[i][j].ToString().Length > 0)
                                    {
                                        ValueFortag = Prefix + " " + DsPreview.Tables[0].Rows[i][j].ToString().Replace("\r", "<br/>").Replace("\n", "&nbsp;") + " " + Suffix;
                                    }
                                    else
                                    {
                                        ValueFortag = string.Empty;
                                    }
                                }
                            }
                        }
                        else
                        {
                            if (Isnumber(DsPreview.Tables[0].Rows[i][j].ToString().Replace("\r", "<br/>").Replace("\n", "&nbsp;")) == true)
                            {
                                ValueFortag = Convert.ToDouble(DsPreview.Tables[0].Rows[i][j].ToString().Replace("\r", "<br/>").Replace("\n", "&nbsp;")).ToString();
                            }
                            else
                            {
                                ValueFortag = DsPreview.Tables[0].Rows[i][j].ToString().Replace("\r", "<br/>").Replace("\n", "&nbsp;");
                            }
                        }
                        if (DsPreview.Tables[0].Columns[j].Caption.ToLower() == NavColumn.ToLower().ToString())
                        {
                            ProdID = oHelper.CI(DsPreview.Tables[0].Rows[i][0].ToString());
                            HypColumn = HypCURL.Replace("{PRODUCT_ID}", ProdID.ToString());
                            Min_ord_qty = oHelper.CI(oOrder.GetProductMinimumOrderQty(ProdID));
                            HypColumn = HypColumn.Replace("{MIN_ORD_QTY}", Min_ord_qty.ToString());
                            Qty_avail = oHelper.CI(oOrder.GetProductAvilableQty(ProdID));
                            HypColumn = HypColumn.Replace("{QTY_AVAIL}", Qty_avail.ToString());
                            HypColumn = HypColumn.Replace("{FAMILY_ID}", this.FamilyID.ToString());

                            ValueFortag = "<A HREF=\"" + HypColumn + "\" > " + ValueFortag + "</A>";
                        }
                        if (AttrType == 4)
                        {
                            if (UserID > 0)
                            {

                                dsBgDisc = oBG.GetBuyerGroupBasedDiscountDetails(oBG.GetBuyerGroup(UserID));
                            }
                            else
                            {
                                dsBgDisc = oBG.GetBuyerGroupBasedDiscountDetails("DEFAULTBG");
                            }

                            if (dsBgDisc != null)
                            {
                                if (dsBgDisc.Tables[0].Rows.Count > 0)
                                {
                                    decimal DiscVal = oHelper.CDEC(dsBgDisc.Tables[0].Rows[0].ItemArray[0].ToString());
                                    DateTime ValidDt = Convert.ToDateTime(dsBgDisc.Tables[0].Rows[0].ItemArray[1].ToString());
                                    string CalMth = dsBgDisc.Tables[0].Rows[0].ItemArray[2].ToString();
                                    untPrice = oHelper.CDEC(DsPreview.Tables[0].Rows[i][j].ToString());
                                    bool IsBGCatProd = oBG.IsBGCatalogProduct(CatalogID, oBG.GetBuyerGroup(UserID).ToString());
                                    if (ValidDt.CompareTo(DateTime.Today) >= 0 && DiscVal > 0 && IsBGCatProd == true)
                                    {
                                        ValueFortag = oBG.CalculateBGDiscountPrice(untPrice, DiscVal, CalMth).ToString();

                                    }
                                }
                            }
                            ValueFortag = "<div id=\"pid" + DsPreview.Tables[0].Rows[i]["product_id"].ToString() + "\" style=\"background-color:#ffffff;visibility:hidden;position:absolute\">" + AssemblePriceTable((int)DsPreview.Tables[0].Rows[i]["product_id"]) + "</div><div onMouseOver=\"javascript:ShowPriceTable('pid" + DsPreview.Tables[0].Rows[i]["product_id"].ToString() + "')\" onMouseOut=\"javascript:ClosePriceTable('pid" + DsPreview.Tables[0].Rows[i]["product_id"].ToString() + "')\" style=\"position:relative\">" + ValueFortag + "</div>";
                            //ValueFortag = oHelper.GetOptionValues("CURRENCYFORMAT").ToString() + ValueFortag;
                        }
                        if (rowcolor == false)
                        {
                            strBldr.Append("<TD ALIGN=\"center\" VALIGN=\"Middle\" style=\"width: 200px; color: Black; BACKGROUND-COLOR: #e0e0e0  \" >" + ValueFortag + "</TD>");
                        }
                        else if (rowcolor == true)
                        {
                            strBldr.Append("<TD ALIGN=\"center\" VALIGN=\"Middle\" style=\"width: 200px; color: Black; BACKGROUND-COLOR: #f3f3f3  \" >" + ValueFortag + "</TD>");
                        }
                    }
                    //else
                    //{
                    //    strBldr.Append("<TD ALIGN=\"" + alignVal + "\" VALIGN=\"Middle\" style=\"width: 200px; color: Black; BACKGROUND-COLOR: white  \" >" + DsPreview.Tables[0].Rows[i][j].ToString().Replace("\r", "<br/>").Replace("\n", "&nbsp;") + "</TD>");
                    //}

                    //Add the Shipping and Cart Images
                    if (j == DsPreview.Tables[0].Columns.Count - 1)
                    {

                        ProdID = oHelper.CI(DsPreview.Tables[0].Rows[i][0].ToString());
                        Boolean IsShipping = oOrder.GetProductIsShipping(ProdID);
                        string ShipImgPath = "";
                        int IsAvailable = oPro.GetProductAvailability(ProdID);
                        if (IsShipping == true)
                        {
                            ShipImgPath = oHelper.GetOptionValues("IMAGE PATH").ToString() + oHelper.GetOptionValues("SHIPPING IMAGE").ToString();
                            string ShipUrl = oHelper.GetOptionValues("SHIP URL").ToString();
                            ShipImgPath = "<A HREF=\"" + ShipUrl + "\" style=\"text-decoration:none\"><IMG SRC=\"" + ShipImgPath + "\" style=\"border-width:0\"></A>";
                        }
                        else if (IsShipping == false)
                        {
                            ShipImgPath = oHelper.GetOptionValues("IMAGE PATH").ToString() + oHelper.GetOptionValues("NO SHIPPING IMAGE").ToString();
                            ShipImgPath = "<IMG SRC=\"" + ShipImgPath + "\" style=\"border-width:0\">";
                        }

                        if (HttpContext.Current.Request.Url.ToString().ToLower().Contains("family.aspx") == true && HttpContext.Current.Request["sl1"] != null && HttpContext.Current.Request["sl1"].ToString() != "" && HttpContext.Current.Request["sl2"] != null && HttpContext.Current.Request["sl2"].ToString() != "")
                        {
                            ShipImgPath = "<a href=\"productdetails.aspx?&pid=" + DsPreview.Tables[0].Rows[i]["PRODUCT_ID"].ToString() + "&fid=" + DsPreview.Tables[1].Rows[0]["FAMILY_ID"].ToString() + "&byp=2&qf=1&cid=" + HttpContext.Current.Request["cid"].ToString() + "&sl1=" + HttpContext.Current.Request["sl1"].ToString() + "&sl2=" + HttpContext.Current.Request["sl2"].ToString() + "&tf=1\"  class=\"tx_3\">" +
                                      "<img src=\"images/ico_details.gif\" width=\"17\" height=\"14\" border=\"0\" align=\"absmiddle\" /> <br/>Details </a>";

                        }
                        else if (HttpContext.Current.Request.Url.ToString().ToLower().Contains("family.aspx") == true && HttpContext.Current.Request["sl1"] != null && HttpContext.Current.Request["sl1"].ToString() != "")
                        {
                            ShipImgPath = "<a href=\"productdetails.aspx?&pid=" + DsPreview.Tables[0].Rows[i]["PRODUCT_ID"].ToString() + "&fid=" + DsPreview.Tables[1].Rows[0]["FAMILY_ID"].ToString() + "&byp=2&qf=1&cid=" + HttpContext.Current.Request["cid"].ToString() + "&sl1=" + HttpContext.Current.Request["sl1"].ToString() + "&tf=1\"  class=\"tx_3\">" +
                                     "<img src=\"images/ico_details.gif\" width=\"17\" height=\"14\" border=\"0\" align=\"absmiddle\" /> <br/>Details </a>";
                        }
                        else
                        {
                            if (HttpContext.Current.Request["pcr"] != null && HttpContext.Current.Request["pcr"].ToString() != "")
                            {
                                if (HttpContext.Current.Request["byp"] != null && HttpContext.Current.Request["byp"].ToString() != "")
                                {
                                    if (HttpContext.Current.Request["cid"] != null && HttpContext.Current.Request["cid"].ToString() != null)
                                    {
                                        ShipImgPath = "<a href=\"productdetails.aspx?&pid=" + DsPreview.Tables[0].Rows[i]["PRODUCT_ID"].ToString() + "&fid=" + DsPreview.Tables[1].Rows[0]["FAMILY_ID"].ToString() + "&pcr=" + HttpContext.Current.Request["pcr"].ToString() + "&cid=" + HttpContext.Current.Request["cid"].ToString() + "&byp=" + HttpContext.Current.Request["byp"].ToString() + "&qf=1\"  class=\"tx_3\">" +
                                  "<img src=\"images/ico_details.gif\" width=\"17\" height=\"14\" border=\"0\" align=\"absmiddle\" /> Details </a>";
                                    }
                                    else
                                    {
                                        ShipImgPath = "<a href=\"productdetails.aspx?&pid=" + DsPreview.Tables[0].Rows[i]["PRODUCT_ID"].ToString() + "&fid=" + DsPreview.Tables[1].Rows[0]["FAMILY_ID"].ToString() + "&pcr=" + HttpContext.Current.Request["pcr"].ToString() + "&byp=" + HttpContext.Current.Request["byp"].ToString() + "&qf=1\"  class=\"tx_3\">" +
                                  "<img src=\"images/ico_details.gif\" width=\"17\" height=\"14\" border=\"0\" align=\"absmiddle\" /> Details </a>";
                                    }
                                }
                                else
                                {
                                    ShipImgPath = "<a href=\"productdetails.aspx?&pid=" + DsPreview.Tables[0].Rows[i]["PRODUCT_ID"].ToString() + "&fid=" + DsPreview.Tables[1].Rows[0]["FAMILY_ID"].ToString() + "&pcr=" + HttpContext.Current.Request["pcr"].ToString() + "\"  class=\"tx_3\">" +
                              "<img src=\"images/ico_details.gif\" width=\"17\" height=\"14\" border=\"0\" align=\"absmiddle\" /> Details </a>";
                                }
                            }
                            else
                                ShipImgPath = "<a href=\"productdetails.aspx?&pid=" + DsPreview.Tables[0].Rows[i]["PRODUCT_ID"].ToString() + "&fid=" + DsPreview.Tables[1].Rows[0]["FAMILY_ID"].ToString() + "\"  class=\"tx_3\">" +
                                          "<img src=\"images/ico_details.gif\" width=\"17\" height=\"14\" border=\"0\" align=\"absmiddle\" /> <br/>Details </a>";
                        }

                        if (rowcolor == false)
                        {
                            strBldr.Append("<TD  ALIGN=\"center\" VALIGN=\"Middle\" style=\"width: 200px; color: Black;  BACKGROUND-COLOR: #e0e0e0  \">" + ShipImgPath + "</TD>");
                        }
                        else if (rowcolor == true)
                        {
                            strBldr.Append("<TD  ALIGN=\"center\" VALIGN=\"Middle\" style=\"width: 200px; color: Black;  BACKGROUND-COLOR: #f3f3f3  \">" + ShipImgPath + "</TD>");
                        }

                        if (EComState.ToUpper() == "YES")
                        {
                            //Add the Cart Image
                            string CartImgPath = "";
                            //ProdID = oHelper.CI(sourceTable.Rows[i - (columnTable.Columns.Count + 1)]["PRODUCT_ID"].ToString());
                            if (Restricted.ToUpper() == "YES")
                            {
                                CartImgPath = oHelper.GetOptionValues("RESTRICTED PRODUCT TEXT");
                                string CartUrl = oHelper.GetOptionValues("RESTRICTED PRODUCT URL").ToString();
                                CartImgPath = "<A HREF=\"" + CartUrl + "\" style=\"text-decoration:none\">" + CartImgPath + " </A>";
                            }
                            else
                            {
                                if (IsAvailable == 1)
                                {
                                    CartImgPath = oHelper.GetOptionValues("IMAGE PATH").ToString() + oHelper.GetOptionValues("CARTIMGPATH").ToString();
                                    Min_ord_qty = oOrder.GetProductMinimumOrderQty(ProdID);
                                    string CartUrl = oHelper.GetOptionValues("CARTURL").ToString();
                                    CartUrl = CartUrl.Replace("{PRODUCT_ID}", ProdID.ToString());
                                    CartUrl = CartUrl.Replace("{MIN_ORD_QTY}", Min_ord_qty.ToString());
                                    CartImgPath = "<A HREF=\"" + CartUrl + "\" style=\"text-decoration:none\"><IMG SRC=\"" + CartImgPath + "\" style=\"border-width:0\"></A>";

                                    string _StockStatus = GetStockStatus(Convert.ToInt32(ProdID.ToString()));
                                    string _StockStatusTrim = _StockStatus.Trim();
                                    bool _Tbt_Stock_Status_2 = false;

                                    switch (_StockStatusTrim)
                                    {
                                        case "IN STOCK":
                                            _Tbt_Stock_Status_2 = true;
                                            break;
                                        case "SPECIAL ORDER":
                                            _Tbt_Stock_Status_2 = true;
                                            break;
                                        case "SPECIAL ORDER PRICE & AVAILABILITY TO BE CONFIRMED":
                                            _Tbt_Stock_Status_2 = true;
                                            break;
                                        case "SPECIAL ORDER PRICE &":
                                            _Tbt_Stock_Status_2 = true;
                                            break;
                                        case "DISCONTINUED":
                                            _Tbt_Stock_Status_2 = false;
                                            break;
                                        case "DISCONTINUED NO LONGER AVAILABLE":
                                            _Tbt_Stock_Status_2 = false;
                                            break;
                                        case "DISCONTINUED NO LONGER":
                                            _Tbt_Stock_Status_2 = false;
                                            break;
                                        case "TEMPORARY UNAVAILABLE":
                                            _Tbt_Stock_Status_2 = true;
                                            break;
                                        case "TEMPORARY UNAVAILABLE NO ETA":
                                            _Tbt_Stock_Status_2 = true;
                                            break;
                                        case "OUT OF STOCK":
                                            _Tbt_Stock_Status_2 = true;
                                            break;
                                        case "OUT OF STOCK ITEM WILL BE BACK ORDERED":
                                            _Tbt_Stock_Status_2 = true;
                                            break;
                                        case "OUT OF STOCK ITEM WILL":
                                            _Tbt_Stock_Status_2 = true;
                                            break;
                                        default:
                                            _Tbt_Stock_Status_2 = false;
                                            break;
                                    }

                                    if (_Tbt_Stock_Status_2 == true)
                                    {

                                        CartImgPath = "<table><tr><td>" +
                                                   "<input valign=\"middle\" name=\"txt" + ProdID.ToString() + "_" + oOrder.GetProductAvilableQty(ProdID).ToString() + "_" + Min_ord_qty.ToString() + "_" + FamilyID.ToString() + "\" type=\"text\" size=\"5\" id=\"txt" + ProdID.ToString() + "_" + oOrder.GetProductAvilableQty(ProdID).ToString() + "_" + Min_ord_qty.ToString() + "_" + FamilyID.ToString() + "\" style=\"background-color:#FFFFFF;color: #000000;height=23;\"   /> " +
                                                 "</td><td>" +
                                                 "  <a style=\"cursor:pointer;\" valign=\"middle\"  onMouseOut=\"javascript:MM_swapImgRestore();ClosePriceTable('pid" + DsPreview.Tables[0].Rows[i]["product_id"].ToString() + "')\" onMouseOver=\"javascript:MM_swapImage('Image" + ProdID.ToString() + "_fp','','images/but_buy2.gif',1);ShowPriceTable('pid" + DsPreview.Tables[0].Rows[i]["product_id"].ToString() + "')\">" +
                                            //"<div onmouseout=\"MM_swapImgRestore()\" onmouseover=\"MM_swapImage('Image"+ ProdID.ToString() + "_fp','','images/but_buy2.gif',1)\" style=\"width:76px; height:25px; cursor:pointer; \">" +
                                                 "   <img src=\"images/but_buy1.gif\" name=\"Image" + ProdID.ToString() + "_fp\" width=\"76\" height=\"25\" border=\"0\" onClick=\"productbuy('txt" + ProdID.ToString() + "_" + oOrder.GetProductAvilableQty(ProdID).ToString() + "_" + Min_ord_qty.ToString() + "_" + FamilyID.ToString() + "','" + ProdID.ToString() + "');\"/>" +
                                                 "</a></td></tr></table>";
                                    }
                                    else
                                    {
                                        CartImgPath = "";
                                    }

                                    if (rowcolor == false)
                                    {
                                        strBldr.Append("<TD ALIGN=\"Center\" VALIGN=\"Middle\"  style=\"width: 200px; color: Black; BACKGROUND-COLOR: #e0e0e0  \">" + CartImgPath + "</TD>");
                                    }
                                    if (rowcolor == true)
                                    {
                                        strBldr.Append("<TD ALIGN=\"Center\" VALIGN=\"Middle\"  style=\"width: 200px; color: Black; BACKGROUND-COLOR: #f3f3f3  \">" + CartImgPath + "</TD>");
                                    }
                                }
                                else
                                {
                                    if (rowcolor == false)
                                    {
                                        strBldr.Append("<TD ALIGN=\"Center\" VALIGN=\"Middle\"  style=\"width: 200px; color: Black; BACKGROUND-COLOR: #e0e0e0  \">N/A</TD>");
                                    }
                                    if (rowcolor == true)
                                    {
                                        strBldr.Append("<TD ALIGN=\"Center\" VALIGN=\"Middle\"  style=\"width: 200px; color: Black; BACKGROUND-COLOR: #f3f3f3  \">N/A</TD>");
                                    }
                                }
                            }
                        }
                    }

                }
                strBldr.Append("</TR>");
            }

            strBldr.Append("</TABLE></td></tr></table>");
            if (strBldr.ToString().Contains("<TABLE border=0 cellspacing=1 style=\"background-color:black\" cellpadding=3><style>td{font-family:arial Unicode ms;font-size:12px;}th{font-family:arial unicode ms;font-size:12px;font-weight:Bold}</style><TR></TR></TABLE>"))
            {
                strBldr = strBldr.Remove(0, strBldr.Length);
            }
            return strBldr.ToString();
        }

        private string GetStockStatus(int ProductID)
        {
            string Retval = "NO STATUS AVAILABLE";
            try
            {
                string sSQL = string.Format("select PROD_STK_STATUS_DSC from WESTB_PRODUCT_ITEM WHERE PRODUCT_ID = {0}", ProductID);
                oHelper.SQLString = sSQL;
                Retval = oHelper.GetValue("PROD_STK_STATUS_DSC").ToString().Replace("_", " ");
            }
            catch
            {
            }
            return Retval;
        }

        private decimal GetMyPrice(int ProductID)
        {
            decimal retval = 0.00M;
            string userid = HttpContext.Current.Session["USER_ID"].ToString();
            if (!string.IsNullOrEmpty(userid))
            {
                string sSQL = string.Format("select price_code from wes_customer where wes_customer_id in (select company_id from tbwc_company_buyers where user_id = {0})", userid);
                oHelper.SQLString = sSQL;
                int pricecode = oHelper.CI(oHelper.GetValue("price_code"));

                string strquery = "";
                if (pricecode == 1)
                {
                    strquery = string.Format("exec GetWESIncProductPrice {0},{1},{2}", ProductID, 1, HttpContext.Current.Session["USER_ID"]);
                }
                else
                {
                    strquery = string.Format("exec GetWESProductPrice {0},{1},{2}", ProductID, 1, HttpContext.Current.Session["USER_ID"]);
                }

                DataSet DSprice = new DataSet();
                oHelper.SQLString = strquery;
                retval = Math.Round(Convert.ToDecimal(oHelper.GetValue("Numeric_Value")), 2);
            }
            return retval;
        }

        private string AssemblePriceTable(int ProductID)
        {
            string _sPriceTable = "";
            SqlConnection oSQLCon = null;
            try
            {
                string userid = HttpContext.Current.Session["USER_ID"].ToString();
                string sSQL = string.Format("select price_code from wes_customer where wes_customer_id in (select company_id from tbwc_company_buyers where user_id = {0})", userid);
                oHelper.SQLString = sSQL;
                int pricecode = oHelper.CI(oHelper.GetValue("price_code"));
                DataSet dsPriceTable = new DataSet();
                oSQLCon = new SqlConnection(oCon.ConnectionString.Replace("provider=SQLOLEDB;", ""));
                oSQLCon.Open();
                SqlCommand oCmd = new SqlCommand("SELECT STRING_VALUE FROM TB_PROD_SPECS WHERE PRODUCT_ID = @PRODUCT_ID and ATTRIBUTE_ID = 1", oSQLCon);
                oCmd.Parameters.Clear();
                oCmd.Parameters.AddWithValue("@PRODUCT_ID", ProductID);
                string _sCODE = oCmd.ExecuteScalar().ToString();
                oCmd = new SqlCommand("select PROD_STK_STATUS_DSC from WESTB_PRODUCT_ITEM WHERE PRODUCT_ID = @PRODUCT_ID", oSQLCon);
                oCmd.Parameters.Clear();
                oCmd.Parameters.AddWithValue("@PRODUCT_ID", ProductID);
                string stkstatus = oCmd.ExecuteScalar().ToString();

                string _Tbt_Stock_Status = "";
                string _Tbt_Stock_Status_1 = "";
                bool _Tbt_Stock_Status_2 = false;
                string _Tbt_Stock_Status_3 = "";
                string _Colorcode1 = "";
                string _Colorcode;
                string StockStatus = stkstatus.Replace("_", " ");
                string _StockStatusTrim = StockStatus.Trim();

                switch (_StockStatusTrim)
                {
                    case "IN STOCK":
                        _Tbt_Stock_Status = "<span style=color:#43A246><b>INSTOCK</b></span><br>";
                        _Tbt_Stock_Status_2 = true;
                        break;
                    case "SPECIAL ORDER":
                        _Colorcode = "#43A246";
                        _Tbt_Stock_Status_2 = true;
                        _Tbt_Stock_Status = "<span style=color:#43A246><b>SPECIAL ORDER PRICE & AVAILABILTY TO BE CONFIRMED</b></span><br>";
                        break;
                    case "SPECIAL ORDER PRICE & AVAILABILITY TO BE CONFIRMED":
                        _Tbt_Stock_Status_2 = true;
                        _Tbt_Stock_Status = "<span style=color:#43A246><b>SPECIAL ORDER PRICE & AVAILABILTY TO BE CONFIRMED</b></span><br>";
                        break;
                    case "SPECIAL ORDER PRICE &":
                        _Tbt_Stock_Status_2 = true;
                        _Tbt_Stock_Status = "<span style=color:#43A246><b>SPECIAL ORDER PRICE & AVAILABILTY TO BE CONFIRMED</b></span><br>";
                        break;
                    case "DISCONTINUED":
                        _Tbt_Stock_Status_2 = false;
                        _Tbt_Stock_Status = "<span style=color:#ED1C24>DISCONTINUED NO LONGER AVAILABLE</span><br>";
                        break;
                    case "DISCONTINUED NO LONGER AVAILABLE":
                        _Tbt_Stock_Status_2 = false;
                        _Tbt_Stock_Status = "<span style=color:#ED1C24>DISCONTINUED NO LONGER AVAILABLE</span><br>";
                        break;
                    case "DISCONTINUED NO LONGER":
                        _Tbt_Stock_Status_2 = false;
                        _Tbt_Stock_Status_3 = "<span style=color:#ED1C24>DISCONTINUED NO LONGER AVAILABLE</span><br>";
                        break;
                    case "TEMPORARY UNAVAILABLE":
                        _Tbt_Stock_Status_2 = false;
                        _Tbt_Stock_Status = "<span style=color:#F9A023>TEMPORARY UNAVAILABLE NO ETA</span>";
                        break;
                    case "TEMPORARY UNAVAILABLE NO ETA":
                        _Tbt_Stock_Status_2 = false;
                        _Tbt_Stock_Status = "<span style=color:#F9A023>TEMPORARY UNAVAILABLE NO ETA</span>";
                        break;
                    case "OUT OF STOCK":
                        _Tbt_Stock_Status_2 = false;
                        _Tbt_Stock_Status_3 = "<span style=color:#F9A023>OUT OF STOCK</span><br>";
                        _Tbt_Stock_Status_1 = _Tbt_Stock_Status_3 + "<span style=color:#43A246> <b>ITEM WILL BE BACK ORDERED</b> </span>";
                        break;
                    case "OUT OF STOCK ITEM WILL BE BACK ORDERED":
                        _Tbt_Stock_Status_2 = false;
                        _Tbt_Stock_Status_3 = "<span style=color:#F9A023>OUT OF STOCK</span><br/>";
                        _Tbt_Stock_Status_1 = _Tbt_Stock_Status_3 + "<span style=color:#43A246><b>ITEM WILL BE BACK ORDERED</b></span>";
                        break;
                    case "OUT OF STOCK ITEM WILL":
                        _Tbt_Stock_Status_3 = "<span style=color:#F9A023>OUT OF STOCK</span><br/>";
                        _Tbt_Stock_Status_2 = false;
                        _Tbt_Stock_Status_1 = _Tbt_Stock_Status_3 + "<span style=color:#43A246><b>ITEM WILL BE BACK ORDERED</b></span>";
                        break;
                    default:
                        _Colorcode = "Black";
                        _Tbt_Stock_Status = _StockStatusTrim;
                        break;
                }

                SqlDataAdapter oDa = new SqlDataAdapter();
                oDa.SelectCommand = new SqlCommand();
                oDa.SelectCommand.Connection = oSQLCon;
                oDa.SelectCommand.CommandText = "GetPriceTable";
                oDa.SelectCommand.CommandType = CommandType.StoredProcedure;
                oDa.SelectCommand.Parameters.Clear();
                oDa.SelectCommand.Parameters.AddWithValue("@ProductID", ProductID);
                oDa.SelectCommand.Parameters.AddWithValue("@UserID", userid);
                oDa.Fill(dsPriceTable, "PriceTable");
                _sPriceTable = "<table border=\"0\" cellspacing=\"0\" cellpadding=\"1\" bgcolor=\"black\"><tr><td><table width=\"200\" border=\"0\" cellpadding=\"4\" cellspacing=\"0\" class=\"bg_grey3\"><tr>";
                _sPriceTable += "<td width=\"100\" height=\"39\" valign=\"top\" class=\"pad2\"><b>ORDER CODE:</b><br />";
                _sPriceTable += string.Format("<span class=\"#00CC00\"><b>{0}</b></span></td>", _sCODE);
                _sPriceTable += "<td width=\"100\" valign=\"top\" class=\"pad1\"><b>STOCK STATUS</b><br />";
                if (_Tbt_Stock_Status != "")
                {
                    _sPriceTable += string.Format("{0}</td></tr><tr><td colspan=\"2\" valign=\"top\">", _Tbt_Stock_Status);
                }
                else
                {
                    _sPriceTable += string.Format("{0}</td></tr><tr><td colspan=\"2\" valign=\"top\">", _Tbt_Stock_Status_1);
                }
                _sPriceTable += "<table cellpadding=\"4\" cellspacing=\"0\" border=\"0\" class=\"table_bdr\"><tr class=\"bg_grey3\"><td><b>Qty</b></td><td><b>Cost Inc GST</b></td><td><b>Cost Ex GST</b></td></tr>";

                int TotalCount = 0;
                int RowCount = 0;

                if (pricecode == 3)
                    foreach (DataRow oDr in dsPriceTable.Tables["PriceTable"].Rows)
                    {
                        _sPriceTable += string.Format("<tr><td class=\"bg_grey3\"><b>{0}</b></td><td align=\"center\">${1:0.00}</td><td align=\"center\">${2:0.00}</td></tr>", oDr["QTY"], oDr["Price1"], oDr["Price2"]);
                    }
                else
                {
                    bool bLastRow = false;
                    TotalCount = dsPriceTable.Tables["PriceTable"].Rows.Count;
                    RowCount = 0;

                    foreach (DataRow oDr in dsPriceTable.Tables["PriceTable"].Rows)
                    {
                        RowCount = RowCount + 1;
                        if (RowCount == TotalCount && (pricecode >= 4) && oDr["QTY"].Equals("Your Price"))  // check whether it is Last Row
                        {
                            bLastRow = true;
                        }

                        string _color = bLastRow ? "bg_grey31" : "bg_grey3";
                        _sPriceTable += string.Format("<tr><td class=\"{3}\"><b>{0}</b></td><td  class=\"{3}\" align=\"center\">${1:0.00}</td><td class=\"{3}\" align=\"center\">${2:0.00}</td></tr>", oDr["QTY"], oDr["Price1"], oDr["Price2"], _color);
                    }
                }

                _sPriceTable += "</table></td></tr><tr><td colspan=\"2\" height=\"4\"></td></tr></table></td></tr></table>";
                if (oSQLCon.State == ConnectionState.Open) oSQLCon.Close();
            }
            catch (Exception)
            {
                _sPriceTable = "";//<table width=\"200\" border=\"0\" cellpadding=\"4\" cellspacing=\"0\" class=\"bg_grey3\"><tr>";
                if (oSQLCon.State == ConnectionState.Open) oSQLCon.Close();
            }
            return _sPriceTable;
        }

        /// <summary>
        /// This is used to Get Cell String 
        /// </summary>
        /// <param name="CellData">string</param>
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
        ///    ...
        ///     else
        ///     {
        ///       if (chkAttrType[i] == 3)
        ///         strBldr.Append(alignVal + getCellString(DsPreview.Tables[0].Rows[j][i].ToString()));
        ///      else if (chkAttrType[i] == 4)
        ///      {
        ///       ...
        /// }
        /// </code>
        /// </example>
        [WebMethod]
        private string getCellString(string CellData)
        {
            int imgSizeCount = 0;
            //SystemSettingsCollection SettingMembers = SystemSettingsConfiguration.GetConfig.Members;
            string imageUrl = "";
            string imageType = "";
            string imageSizeW = "";
            string imageSizeH = "";
            string returnstr = "";
            imageType = "";
            try
            {
                //FileInfo chkFile = new FileInfo((SettingMembers.GetValue(SystemSettingsCollection.SettingsList.USERIMAGEPATH.ToString())) + CellData);
                FileInfo chkFile = new FileInfo(Server.MapPath(oHelper.GetOptionValues("PRODUCT IMAGE PATH").ToString() + CellData));
                if (chkFile.Exists == true)
                {
                    imageType = "";
                    if (chkFile.Extension.ToUpper() == ".EPS" ||
                        chkFile.Extension.ToUpper() == ".TIF" ||
                        chkFile.Extension.ToUpper() == ".TIFF" ||
                        chkFile.Extension.ToUpper() == ".PSD" ||
                        chkFile.Extension.ToUpper() == ".TGA" ||
                        chkFile.Extension.ToUpper() == ".PCX")
                    {
                        object[] imgargs = { CellData, Server.MapPath("").ToString() + oHelper.GetOptionValues("PRODUCT IMAGE PATH").ToString() + "\\temp\\ImageandAttFile" + chkFile.Name.ToString().Substring(0, chkFile.Name.ToString().LastIndexOf('.')) + valInc.ToString() + ".jpg" };
                        //MagickImageClass MagickImageClassRef = new MagickImageClass();
                        try
                        {
                            //MagickImageClassRef.Convert(ref imgargs);
                        }
                        catch (Exception e)
                        {

                        }
                        FileInfo chkFileVal = new FileInfo(Server.MapPath("").ToString() + oHelper.GetOptionValues("PRODUCT IMAGE PATH").ToString() + "\\temp\\ImageandAttFile" + chkFile.Name.ToString().Substring(0, chkFile.Name.ToString().LastIndexOf('.')) + valInc.ToString() + ".jpg");
                        if (chkFileVal.Exists == true)
                        {
                            //htmlData.Append("<br><br><IMG src = \"" + chkFileVal.FullName.ToString() + "\" height = 200pts width = 200pts />");
                            if (chkSize != null) { chkSize.Dispose(); }
                            chkSize = Image.FromFile(Server.MapPath("").ToString() + oHelper.GetOptionValues("PRODUCT IMAGE PATH").ToString() + "\\temp\\ImageandAttFile" + chkFile.Name.ToString().Substring(0, chkFile.Name.ToString().LastIndexOf('.')) + valInc.ToString() + ".jpg");
                            Size newVal;
                            Double iHeight = chkSize.Height;
                            Double iWidth = chkSize.Width;
                            newVal = ScaleImage(iHeight, iWidth, this._ProductImgWidth, this._ProductImgHeight);
                            imageSizeH = Convert.ToString(newVal.Height);
                            imageSizeW = Convert.ToString(newVal.Width);
                            if (chkSize != null)
                            {
                                chkSize.Dispose();
                            }
                            imageUrl = chkFileVal.FullName.ToString();
                            imageType = "IMAGE";
                            imgSizeCount++;
                            valInc++;
                        }
                    }
                    else if (chkFile.Extension.ToUpper() == ".JPG" ||
                             chkFile.Extension.ToUpper() == ".JPEG" ||
                             chkFile.Extension.ToUpper() == ".GIF" ||
                             chkFile.Extension.ToUpper() == ".BMP" ||
                             chkFile.Extension.ToUpper() == ".ICO" ||
                             chkFile.Extension.ToUpper() == ".PNG")
                    {
                        // htmlData.Append("<br><br><IMG src = \"" + (SettingMembers.GetValue(SystemSettingsCollection.SettingsList.USERIMAGEPATH.ToString())) + htmlPreview1.FAMLIY_SPECS.Rows[rowCount].ItemArray[8].ToString().Trim() + "\" height = 200pts width = 200pts />");
                        if (chkSize != null) { chkSize.Dispose(); }
                        chkSize = Image.FromFile(CellData);
                        Size newVal;
                        Double iHeight = chkSize.Height;
                        Double iWidth = chkSize.Width;
                        newVal = ScaleImage(iHeight, iWidth, this._ProductImgWidth, this._ProductImgHeight);
                        imageSizeH = Convert.ToString(newVal.Height);
                        imageSizeW = Convert.ToString(newVal.Width);
                        if (chkSize != null)
                        {
                            chkSize.Dispose();
                        }
                        imageUrl = CellData.Trim();
                        imageType = "IMAGE";
                        imgSizeCount++;
                    }
                    else if (chkFile.Extension.ToUpper() == ".AVI" ||
                             chkFile.Extension.ToUpper() == ".WMV" ||
                             chkFile.Extension.ToUpper() == ".MPG" ||
                             chkFile.Extension.ToUpper() == ".SWF")
                    {
                        imageUrl = CellData.Trim();
                        imageType = "MEDIA";
                        imgSizeCount++;
                    }
                }
                else if (CellData == "")
                {
                    returnstr = ("\" VALIGN=\"Middle\" style=\" color: Black; BACKGROUND-COLOR: white  \" >");
                    returnstr = returnstr + "";
                    //returnstr = returnstr + ("<IMG SRC = \"" + Application.StartupPath + "\\images\\unsupportedImageformat.jpg " + "\" height = 82px>");
                    returnstr = returnstr + ("</TD>");
                }
                if (imageType == "IMAGE")
                {
                    returnstr = ("\" VALIGN=\"Middle\" style=\"width: 150px; height:150px; color: Black; BACKGROUND-COLOR: white  \" >");
                    returnstr = returnstr + ("<IMG SRC = \"" + imageUrl + "\" HEIGHT=" + imageSizeH.ToString() + " WIDTH = " + imageSizeW.ToString() + ">");
                    returnstr = returnstr + ("</TD>");
                }
                else if (imageType == "MEDIA")
                {
                    returnstr = ("\" VALIGN=\"Middle\" style=\"width: 150px; height:150px; color: Black; BACKGROUND-COLOR: white  \" >");
                    returnstr = returnstr + ("<EMBED SRC= \"" + imageUrl.ToString() + "\" height = 150pts width = 150pts AUTOPLAY=\"false\" CONTROLLER=\"true\"/>");
                    returnstr = returnstr + ("</TD>");
                }
                else if (imageType == "" && CellData != "")
                {
                    returnstr = ("\" VALIGN=\"Middle\" style=\"width: 150px; height:82px; color: Black; BACKGROUND-COLOR: white  \" >");
                    returnstr = returnstr + ("<IMG SRC = \"" + oHelper.GetOptionValues("PRODUCT IMAGE PATH").ToString() + "\\images\\unsupportedImageformat.jpg " + "\" height = 82px>");
                    returnstr = returnstr + ("</TD>");
                }
            }
            catch (Exception) { }
            return (returnstr);
        }

        /// <summary>
        /// This is used to Get Cell String based on Cell Data
        /// </summary>
        /// <param name="CellData">string</param>
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
        ///    ...
        /// if (chkAttrType[j] == 3)
        ///   strBuildXMLOutput1.Append(getCellStrin(obj.DisplayLayout.Rows[i].Cells[GrpColno[GrpColno[j]]].Value.ToString()));
        /// else
        ///   strBuildXMLOutput1.Append((ValueFortag.ToString().Replace("\r", "<br/>").Replace("\n", "&nbsp;")));
        /// }
        /// </code>
        /// </example>
        [WebMethod]
        private string getCellStrin(string CellData)
        {
            int imgSizeCount = 0;
            //SystemSettingsCollection SettingMembers = SystemSettingsConfiguration.GetConfig.Members;
            string imageUrl = "";
            string imageType = "";
            string imageSizeW = "";
            string imageSizeH = "";
            string returnstr = "";
            imageType = "";
            FileInfo chkFile = new FileInfo(Server.MapPath(oHelper.GetOptionValues("PRODUCT IMAGE PATH").ToString() + CellData));
            if (chkFile.Exists == true)
            {
                imageType = "";
                if (chkFile.Extension.ToUpper() == ".EPS" ||
                    chkFile.Extension.ToUpper() == ".TIF" ||
                    chkFile.Extension.ToUpper() == ".TIFF" ||
                    chkFile.Extension.ToUpper() == ".PSD" ||
                    chkFile.Extension.ToUpper() == ".TGA" ||
                    chkFile.Extension.ToUpper() == ".PCX")
                {
                    object[] imgargs = { CellData, Server.MapPath("").ToString() + oHelper.GetOptionValues("PRODUCT IMAGE PATH").ToString() + "\\temp\\ImageandAttFile" + chkFile.Name.ToString().Substring(0, chkFile.Name.ToString().LastIndexOf('.')) + valInc.ToString() + ".jpg" };
                    //MagickImageClass MagickImageClassRef = new MagickImageClass();
                    try
                    {
                        //MagickImageClassRef.Convert(ref imgargs);
                    }
                    catch (Exception)
                    {

                    }
                    FileInfo chkFileVal = new FileInfo(Server.MapPath("").ToString() + oHelper.GetOptionValues("PRODUCT IMAGE PATH").ToString() + "\\temp\\ImageandAttFile" + chkFile.Name.ToString().Substring(0, chkFile.Name.ToString().LastIndexOf('.')) + valInc.ToString() + ".jpg");
                    if (chkFileVal.Exists == true)
                    {
                        //htmlData.Append("<br><br><IMG src = \"" + chkFileVal.FullName.ToString() + "\" height = 200pts width = 200pts />");
                        if (chkSize != null) { chkSize.Dispose(); }
                        chkSize = Image.FromFile(Server.MapPath("").ToString() + oHelper.GetOptionValues("PRODUCT IMAGE PATH").ToString() + "\\temp\\ImageandAttFile" + chkFile.Name.ToString().Substring(0, chkFile.Name.ToString().LastIndexOf('.')) + valInc.ToString() + ".jpg");
                        Size newVal;
                        Double iHeight = chkSize.Height;
                        Double iWidth = chkSize.Width;
                        newVal = ScaleImage(iHeight, iWidth, this._ProductImgWidth, this._ProductImgHeight);
                        imageSizeH = Convert.ToString(newVal.Height);
                        imageSizeW = Convert.ToString(newVal.Width);
                        if (chkSize != null)
                        {
                            chkSize.Dispose();
                        }
                        imageUrl = chkFileVal.FullName.ToString();
                        imageType = "IMAGE";
                        imgSizeCount++;
                        valInc++;
                    }
                }
                else if (chkFile.Extension.ToUpper() == ".JPG" ||
                         chkFile.Extension.ToUpper() == ".JPEG" ||
                         chkFile.Extension.ToUpper() == ".GIF" ||
                         chkFile.Extension.ToUpper() == ".BMP" ||
                         chkFile.Extension.ToUpper() == ".PNG" ||
                         chkFile.Extension.ToUpper() == ".ICO")
                {
                    // htmlData.Append("<br><br><IMG src = \"" + (SettingMembers.GetValue(SystemSettingsCollection.SettingsList.USERIMAGEPATH.ToString())) + htmlPreview1.FAMLIY_SPECS.Rows[rowCount].ItemArray[8].ToString().Trim() + "\" height = 200pts width = 200pts />");
                    if (chkSize != null) { chkSize.Dispose(); }
                    chkSize = Image.FromFile(CellData);
                    Size newVal;
                    Double iHeight = chkSize.Height;
                    Double iWidth = chkSize.Width;
                    newVal = ScaleImage(iHeight, iWidth, this._ProductImgWidth, this._ProductImgHeight);
                    imageSizeH = Convert.ToString(newVal.Height);
                    imageSizeW = Convert.ToString(newVal.Width);
                    if (chkSize != null)
                    {
                        chkSize.Dispose();
                    }
                    imageUrl = CellData.Trim();
                    imageType = "IMAGE";
                    imgSizeCount++;
                }
                else if (chkFile.Extension.ToUpper() == ".AVI" ||
                         chkFile.Extension.ToUpper() == ".WMV" ||
                         chkFile.Extension.ToUpper() == ".MPG" ||
                         chkFile.Extension.ToUpper() == ".SWF")
                {
                    imageUrl = CellData.Trim();
                    imageType = "MEDIA";
                    imgSizeCount++;
                }
            }

            if (imageType == "IMAGE")
            {
                returnstr = ("<IMG SRC = \"" + imageUrl + "\" HEIGHT=" + imageSizeH.ToString() + " WIDTH = " + imageSizeW.ToString() + ">");
            }
            else if (imageType == "MEDIA")
            {
                returnstr = ("<EMBED SRC= \"" + imageUrl.ToString() + "\" height =  200pts width =  200pts AUTOPLAY=\"false\" CONTROLLER=\"true\"/>");
            }
            else if (CellData != "")
            {
                returnstr = ("<IMG SRC = \"" + oHelper.GetOptionValues("PRODUCT IMAGE PATH").ToString() + "\\images\\unsupportedImageformat.jpg " + "\" HEIGHT=82px >");
            }
            else if (imageType == "")
            {
                returnstr = ("<IMG SRC = \"" + oHelper.GetOptionValues("PRODUCT IMAGE PATH").ToString() + "\\images\\unsupportedImageformat.jpg " + "\" HEIGHT=82px >");
                //returnstr = returnstr + (CellData);
            }
            return (returnstr);
        }

        /// <summary>
        /// This is used to Generate Vertical HTML Layout
        /// </summary>
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
        ///    ...
        ///    else if (tableLayout == "Vertical")
        ///    {
        ///      HTMLString = this.GenerateVerticalHTML();
        ///    }
        /// }
        /// </code>
        /// </example>
        [WebMethod]
        private string GenerateVerticalHTML()
        {
            string AttrID = string.Empty;
            DataSet dsBgDisc = new DataSet();
            decimal untPrice = 0;
            StringBuilder strBldr = new StringBuilder();
            Order oOrder = new Order();
            string HypCols = "";
            int ProdID = 0;
            int Qty_avail;
            int Min_odr_qty;
            int IsAvailable;
            string NavColumn = oHelper.GetOptionValues("NAVIGATIONCOLUMN").ToString();
            string HypCURL = oHelper.GetOptionValues("NAVIGATIONURL").ToString();
            string EcommerceState = oHelper.GetOptionValues("ECOMMERCEENABLED").ToString();
            strBldr.Append("<table width=\"100%\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\"><tr> <td align=\"right\" ><TABLE width=\"99%\" border=0 cellspacing=1 style=\"background-color:black\" cellpadding=3>");
            strBldr.Append("<style>td{font-family:arial Unicode ms;font-size:12px;}th{font-family:arial Unicode ms;font-size:12px;font-weight:Bold}</style>");

            int AttributeType;
            string ValueFortag = string.Empty;
            bool newRow = true;
            for (int i = 1; i < DsPreview.Tables[0].Columns.Count; i++)
            {
                oHelper.SQLString = "SELECT ATTRIBUTE_TYPE FROM TB_ATTRIBUTE WHERE ATTRIBUTE_NAME= '" + DsPreview.Tables[0].Columns[i].ToString() + "'";
                AttributeType = oHelper.CI(oHelper.GetValue("ATTRIBUTE_TYPE").ToString());
                strBldr.Append("<TR>");
                newRow = true;
                for (int j = 0; j < DsPreview.Tables[0].Rows.Count; j++)
                {
                    string alignVal = "LEFT";
                    AttrID = DsPreview.Tables[1].Rows[0].ItemArray[4 + i].ToString();
                    ExtractCurrenyFormat(Convert.ToInt32(AttrID));
                    oHelper.SQLString = "SELECT ATTRIBUTE_DATATYPE FROM TB_ATTRIBUTE WHERE ATTRIBUTE_ID = " + AttrID;
                    DataSet DSS = oHelper.GetDataSet();
                    if (AttributeType == 1)
                    {
                    }
                    if (AttributeType == 4 || DSS.Tables[0].Rows[0].ItemArray[0].ToString().Substring(0, 3).ToUpper() == "NUM")
                    {
                        alignVal = "LEFT";
                    }
                    if (DisplayHeaders == true && newRow == true)
                    {

                        strBldr.Append("<TD ALIGN=\"" + alignVal + "\" VALIGN=\"Middle\" style=\"width: 300px; color: Black; BACKGROUND-COLOR: #99CCFF   \" >");
                        //strBldr.Append("<TD ALIGN=\"" + alignVal + "\" VALIGN=\"TOP\">");
                        if (DsPreview.Tables[0].Columns[i].Caption.ToString() != "PRODUCT_ID")
                        {
                            strBldr.Append(DsPreview.Tables[0].Columns[i].Caption + "</TD>");
                        }
                        j = -1;
                        newRow = false;

                    }
                    else
                    {
                        if (AttributeType == 3)
                            strBldr.Append("<TD ALIGN=\"" + alignVal + getCellString(DsPreview.Tables[0].Rows[j][i].ToString()));
                        else
                        {
                            if ((Headeroptions == "All") || (Headeroptions != "All" && j == 0))
                            {
                                if ((EmptyCondition == "Null" || EmptyCondition == "Empty" || EmptyCondition == null) && (DsPreview.Tables[0].Rows[j][i].ToString() == string.Empty))
                                {
                                    ValueFortag = ReplaceText;
                                }
                                else if ((DsPreview.Tables[0].Rows[j][i].ToString()) == (EmptyCondition))
                                {
                                    ValueFortag = ReplaceText;
                                }
                                else
                                {

                                    if (Isnumber(DsPreview.Tables[0].Rows[j][i].ToString().Replace("\r", "<br/>").Replace("\n", "&nbsp;")) == true)
                                    {
                                        if (AttributeType == 4)
                                        {
                                            ValueFortag = Prefix + " " + oHelper.FixDecPlace(Convert.ToDecimal(DsPreview.Tables[0].Rows[j][i].ToString().Replace("\r", "<br/>").Replace("\n", "&nbsp;"))).ToString() + " " + Suffix;
                                        }
                                        else
                                        {
                                            ValueFortag = Prefix + " " + DsPreview.Tables[0].Rows[j][i].ToString().Replace("\r", "<br/>").Replace("\n", "&nbsp;").ToString() + " " + Suffix;
                                        }
                                    }
                                    else
                                    {
                                        if (DsPreview.Tables[0].Rows[j][i].ToString().Length > 0)
                                        {
                                            ValueFortag = Prefix + " " + DsPreview.Tables[0].Rows[j][i].ToString().Replace("\r", "<br/>").Replace("\n", "&nbsp;") + " " + Suffix;
                                        }
                                        else
                                        {
                                            ValueFortag = string.Empty;
                                        }
                                    }
                                }
                            }
                            else
                            {
                                if (Isnumber(DsPreview.Tables[0].Rows[j][i].ToString().Replace("\r", "<br/>").Replace("\n", "&nbsp;")) == true)
                                {
                                    if (AttributeType == 4)
                                    {
                                        ValueFortag = Convert.ToDouble(DsPreview.Tables[0].Rows[j][i].ToString().Replace("\r", "<br/>").Replace("\n", "&nbsp;")).ToString();
                                    }
                                    else
                                    {
                                        ValueFortag = DsPreview.Tables[0].Rows[j][i].ToString().Replace("\r", "<br/>").Replace("\n", "&nbsp;").ToString();
                                    }
                                }
                                else
                                {
                                    ValueFortag = DsPreview.Tables[0].Rows[j][i].ToString().Replace("\r", "<br/>").Replace("\n", "&nbsp;");
                                }
                            }

                            if (DsPreview.Tables[0].Columns[i].Caption.ToLower() == NavColumn.ToLower().ToString())
                            {
                                ProdID = oHelper.CI(DsPreview.Tables[0].Rows[j][0].ToString());
                                HypCols = HypCURL.Replace("{PRODUCT_ID}", ProdID.ToString());
                                Min_odr_qty = oHelper.CI(oOrder.GetProductMinimumOrderQty(ProdID));
                                HypCols = HypCols.Replace("{MIN_ORD_QTY}", Min_odr_qty.ToString());
                                Qty_avail = oHelper.CI(oOrder.GetProductAvilableQty(ProdID));
                                HypCols = HypCols.Replace("{QTY_AVAIL}", Qty_avail.ToString());
                                HypCols = HypCols.Replace("{FAMILY_ID}", this.FamilyID.ToString());
                                ValueFortag = "<A HREF=\"" + HypCols + "\" > " + ValueFortag + "</A>";
                            }

                            if (AttributeType == 4)
                            {
                                if (UserID > 0)
                                {
                                    dsBgDisc = oBG.GetBuyerGroupBasedDiscountDetails(oBG.GetBuyerGroup(UserID));
                                }
                                else
                                {
                                    dsBgDisc = oBG.GetBuyerGroupBasedDiscountDetails("DEFAULTBG");
                                }

                                if (dsBgDisc != null)
                                {
                                    if (dsBgDisc.Tables[0].Rows.Count > 0)
                                    {
                                        decimal DiscVal = oHelper.CDEC(dsBgDisc.Tables[0].Rows[0].ItemArray[0].ToString());
                                        DateTime ValidDt = Convert.ToDateTime(dsBgDisc.Tables[0].Rows[0].ItemArray[1].ToString());
                                        string CalMth = dsBgDisc.Tables[0].Rows[0].ItemArray[2].ToString();
                                        untPrice = oHelper.CDEC(DsPreview.Tables[0].Rows[j][i].ToString());
                                        bool IsBGCatProd = oBG.IsBGCatalogProduct(CatalogID, oBG.GetBuyerGroup(UserID).ToString());
                                        if (ValidDt.CompareTo(DateTime.Today) >= 0 && DiscVal > 0 && IsBGCatProd == true)
                                        {
                                            ValueFortag = oBG.CalculateBGDiscountPrice(untPrice, DiscVal, CalMth).ToString();
                                        }
                                    }
                                }
                                ValueFortag += "PRC1";
                                //ValueFortag = oHelper.GetOptionValues("CURRENCYFORMAT").ToString() + ValueFortag;
                            }
                            if (Isnumber(ValueFortag.Trim()) == true)
                            {
                                ValueFortag = Convert.ToDouble(ValueFortag).ToString();
                            }

                            if (DsPreview.Tables[0].Columns[i].ToString().Contains("Price"))
                            {
                                strBldr.Append("<TD ALIGN=\"LEFT\" VALIGN=\"TOP\" style=\"color: Black; BACKGROUND-COLOR: white  \">" + ValueFortag + "</TD>");
                            }
                            else
                            {
                                //strBldr.Append("<TD ALIGN=\"" + alignVal + "\" VALIGN=\"TOP\" class=\"VertnnerColNormal\" > " + ValueFortag + "</TD>");
                                strBldr.Append("<TD ALIGN=\"" + alignVal + "\" VALIGN=\"Middle\" style=\"width: 200px; color: Black; BACKGROUND-COLOR: white  \" > " + ValueFortag + "</TD>");
                            }
                        }
                    }
                }
                strBldr.Append("</TR>");
            }

            string alignvalue = "LEFT";
            DsPreview.Tables[0].Columns.Add("Cart");
            DsPreview.Tables[0].Columns.Add("Shipping");
            strBldr.Append("<TR>");
            strBldr.Append("<TD ALIGN=\"" + alignvalue + "\" VALIGN=\"Middle\" style=\"width: 200px; color: Black; BACKGROUND-COLOR: #99CCFF  \" > More Info </TD>");
            for (int i = 0; i < DsPreview.Tables[0].Rows.Count; i++)
            {
                ProdID = oHelper.CI(DsPreview.Tables[0].Rows[i][0].ToString());
                Boolean IsShipping = oOrder.GetProductIsShipping(ProdID);
                string ShipImgPath = "";
                if (IsShipping == true)
                {
                    ShipImgPath = oHelper.GetOptionValues("IMAGE PATH").ToString() + oHelper.GetOptionValues("SHIPPING IMAGE").ToString();
                    string ShipUrl = oHelper.GetOptionValues("SHIP URL").ToString();
                    ShipImgPath = "<A HREF=\"" + ShipUrl + "\" style=\"text-decoration:none\"><IMG SRC=\"" + ShipImgPath + "\" style=\"border-width:0\"></A>";
                }
                else if (IsShipping == false)
                {
                    ShipImgPath = oHelper.GetOptionValues("IMAGE PATH").ToString() + oHelper.GetOptionValues("NO SHIPPING IMAGE").ToString();
                    ShipImgPath = "<IMG SRC=\"" + ShipImgPath + "\" style=\"border-width:0\">";
                }
                if (HttpContext.Current.Request.Url.ToString().ToLower().Contains("family.aspx") == true && HttpContext.Current.Request["sl1"] != null && HttpContext.Current.Request["sl1"].ToString() != "" && HttpContext.Current.Request["sl2"] != null && HttpContext.Current.Request["sl2"].ToString() != "")
                {
                    ShipImgPath = "<a href=\"productdetails.aspx?&pid=" + DsPreview.Tables[0].Rows[i]["PRODUCT_ID"].ToString() + "&fid=" + DsPreview.Tables[1].Rows[0]["FAMILY_ID"].ToString() + "&byp=2&qf=1&cid=" + HttpContext.Current.Request["cid"].ToString() + "&sl1=" + HttpContext.Current.Request["sl1"].ToString() + "&sl2=" + HttpContext.Current.Request["sl2"].ToString() + "&tf=1\"  class=\"tx_3\">" +
                              "<img src=\"images/ico_details.gif\" width=\"17\" height=\"14\" border=\"0\" align=\"absmiddle\" /> <br/>Details </a>";

                }
                else if (HttpContext.Current.Request.Url.ToString().ToLower().Contains("family.aspx") == true && HttpContext.Current.Request["sl1"] != null && HttpContext.Current.Request["sl1"].ToString() != "")
                {
                    ShipImgPath = "<a href=\"productdetails.aspx?&pid=" + DsPreview.Tables[0].Rows[i]["PRODUCT_ID"].ToString() + "&fid=" + DsPreview.Tables[1].Rows[0]["FAMILY_ID"].ToString() + "&byp=2&qf=1&cid=" + HttpContext.Current.Request["cid"].ToString() + "&sl1=" + HttpContext.Current.Request["sl1"].ToString() + "&tf=1\"  class=\"tx_3\">" +
                             "<img src=\"images/ico_details.gif\" width=\"17\" height=\"14\" border=\"0\" align=\"absmiddle\" /> <br/>Details </a>";
                }
                else
                {
                    if (HttpContext.Current.Request["pcr"] != null && HttpContext.Current.Request["pcr"].ToString() != "")
                    {
                        ShipImgPath = "<a href=\"productdetails.aspx?&pid=" + DsPreview.Tables[0].Rows[i]["PRODUCT_ID"].ToString() + "&fid=" + DsPreview.Tables[1].Rows[0]["FAMILY_ID"].ToString() + "&pcr=" + HttpContext.Current.Request["pcr"].ToString() + "\"  class=\"tx_3\">" +
                  "<img src=\"images/ico_details.gif\" width=\"17\" height=\"14\" border=\"0\" align=\"absmiddle\" /> Details </a>";
                    }
                    else
                        ShipImgPath = "<a href=\"productdetails.aspx?&pid=" + DsPreview.Tables[0].Rows[i]["PRODUCT_ID"].ToString() + "&fid=" + DsPreview.Tables[1].Rows[0]["FAMILY_ID"].ToString() + "\"  class=\"tx_3\">" +
                                          "<img src=\"images/ico_details.gif\" width=\"17\" height=\"14\" border=\"0\" align=\"absmiddle\" /> Details </a>";
                }

                strBldr.Append("<TD ALIGN=\"Center\" VALIGN=\"Middle\"  style=\"width: 200px; BACKGROUND-COLOR:white \">" + ShipImgPath + "</TD>");
            }
            strBldr.Append("<TR>");
            strBldr.Append("<TD ALIGN=\"" + alignvalue + "\" VALIGN=\"Middle\" style=\"width: 200px; color: Black; BACKGROUND-COLOR: #99CCFF  \"> Cart </TD>");
            for (int i = 0; i < DsPreview.Tables[0].Rows.Count; i++)
            {
                ProdID = oHelper.CI(DsPreview.Tables[0].Rows[i][0].ToString());
                string CartImgPath = "";
                //ProdID = oHelper.CI(sourceTable.Rows[i - (columnTable.Columns.Count + 1)]["PRODUCT_ID"].ToString());
                if (Restricted.ToUpper() == "YES")
                {
                    CartImgPath = oHelper.GetOptionValues("RESTRICTED PRODUCT TEXT");
                    string CartUrl = oHelper.GetOptionValues("RESTRICTED PRODUCT URL").ToString();
                    CartImgPath = "<A HREF=\"" + CartUrl + "\" style=\"text-decoration:none\">" + CartImgPath + " </A>";
                }
                else
                {
                    IsAvailable = oPro.GetProductAvailability(ProdID);
                    if (IsAvailable == 1)
                    {
                        CartImgPath = oHelper.GetOptionValues("IMAGE PATH").ToString() + oHelper.GetOptionValues("CARTIMGPATH").ToString();
                        Min_odr_qty = oOrder.GetProductMinimumOrderQty(ProdID);
                        string CartUrl = oHelper.GetOptionValues("CARTURL").ToString();
                        CartUrl = CartUrl.Replace("{PRODUCT_ID}", ProdID.ToString());
                        CartUrl = CartUrl.Replace("{MIN_ORD_QTY}", Min_odr_qty.ToString());
                        CartImgPath = "<A HREF=\"" + CartUrl + "\" style=\"text-decoration:none\" onMouseOut=\"MM_swapImgRestore()\" onMouseOver=\"MM_swapImage('Image" + ProdID.ToString() + "_mp','','images/but_buy2.gif',1)\"><img src=\"images/but_buy1.gif\" name=\"Image" + ProdID.ToString() + "_mp\" height=\"25\" border=\"0\"></A>";

                        CartImgPath = "<table><tr><td>" +
                                           "<input valign=\"middle\" name=\"txt" + ProdID.ToString() + "_" + oOrder.GetProductAvilableQty(ProdID).ToString() + "_" + Min_odr_qty.ToString() + "_" + FamilyID.ToString() + "\" type=\"text\" size=\"5\" id=\"txt" + ProdID.ToString() + "_" + oOrder.GetProductAvilableQty(ProdID).ToString() + "_" + Min_odr_qty.ToString() + "_" + FamilyID.ToString() + "\" style=\"background-color:#FFFFFF;color: #000000;height=23;\"   /> " +
                                         "</td><td>" +
                                         "  <a style=\"cursor:pointer;\" valign=\"middle\"  onMouseOut=\"MM_swapImgRestore()\" onMouseOver=\"MM_swapImage('Image" + ProdID.ToString() + "_fp','','images/but_buy2.gif',1)\">" +
                                         "   <img src=\"images/but_buy1.gif\" name=\"Image" + ProdID.ToString() + "_fp\" width=\"76\" height=\"25\" border=\"0\" onClick=\"productbuy('txt" + ProdID.ToString() + "_" + oOrder.GetProductAvilableQty(ProdID).ToString() + "_" + Min_odr_qty.ToString() + "_" + FamilyID.ToString() + "','" + ProdID.ToString() + "');\"/>" +
                                         "</a></td></tr></table>";
                        strBldr.Append("<TD ALIGN=\"Center\" VALIGN=\"Middle\"  style=\"width: 200px;  BACKGROUND-COLOR: white\">" + CartImgPath + "</TD>");
                    }
                    else
                    {
                        strBldr.Append("<TD ALIGN=\"Center\" VALIGN=\"Middle\"  style=\"width: 200px;  BACKGROUND-COLOR: white\">N/A</TD>");
                    }
                }


            }
            strBldr.Append("</TR>");
            strBldr.Append("</TABLE></td></tr></table>");
            if (DsPreview.Tables[0].Rows.Count == 0)
            {
                strBldr.Remove(0, strBldr.ToString().Length);
            }
            return strBldr.ToString();

        }

        #region "Grouped Methods"

        public DataSet BuildGroupedDS(DataSet oDS)
        {
            DataSet oResDS = new DataSet();
            try
            {
                string[] GrpColNames;
                GrpColNames = getcoluumns();
                bool ColumnlSetUp = false;
                //DataTable oModfdColDT = new DataTable();
                foreach (DataColumn oDC in oDS.Tables[0].Columns)
                {
                    DataColumn oNewColumn = new DataColumn();

                    //if (oDC.ColumnName.ToString().Contains(" "))
                    //{
                    ColumnlSetUp = false;
                    for (int GrpCol = 0; GrpCol < GrpColNames.Length; GrpCol++)
                    {
                        if (ColumnlSetUp != true)
                        {
                            if (GrpColNames[GrpCol] != null && GrpColNames[GrpCol].ToString() == oDC.ColumnName.ToString() && oDC.ColumnName.Contains(" "))
                            {
                                oNewColumn = new DataColumn(oDC.ColumnName.Replace(" ", ""), System.Type.GetType("System.String"));
                                ColumnlSetUp = true;
                            }
                            else if (GrpColNames[GrpCol] != null && GrpColNames[GrpCol].ToString() == oDC.ColumnName.ToString() && oDC.ColumnName.Contains("."))
                            {
                                oNewColumn = new DataColumn(oDC.ColumnName.Replace(".", ""), System.Type.GetType("System.String"));
                                ColumnlSetUp = true;
                            }

                            else if (GrpColNames[GrpCol] != null)
                            {
                                oNewColumn = new DataColumn(oDC.ColumnName, System.Type.GetType("System.String"));
                                ColumnlSetUp = true;
                            }
                        }
                    }
                    oModfdColDT.Columns.Add(oNewColumn);
                }
                #region "Commented"
                //foreach (DataRow oDataRow in oDR)
                //{
                //    //oResultTable.ImportRow(oDataRow);
                //    //oResultTable.ImportRow(oDataRow);
                //    DataRow oDRSSS = oResultTable.NewRow();
                //    oDRSSS[0] = "sample";
                //    oDRSSS[1] = "sample";
                //    oResultTable.Rows.Add(oDRSSS);
                //    oDRSSS = oResultTable.NewRow();
                //    oDRSSS.ItemArray = oDataRow.ItemArray;
                //    oResultTable.Rows.Add(oDRSSS);
                //    oResultTable.ImportRow(oDataRow);
                //}
                #endregion
                DataRow[] oDR = oDS.Tables[0].Select();
                foreach (DataRow oDataRow in oDR)
                {
                    DataRow oDRSSS;
                    oDRSSS = oModfdColDT.NewRow();
                    oDRSSS.ItemArray = oDataRow.ItemArray;
                    oModfdColDT.Rows.Add(oDRSSS);
                }
                oModfdColDT.Columns.Remove("PRODUCT_ID");

                //Build the Unique Value Datas
                DataSet UniqValsDS = UniqueValueDS(GrpColNames[0].ToString());

                //Build the Dataset for FirstLevel Grouping
                string GroupFilterExpr;
                DataTable oDTGrpResult = new DataTable();
                oDTGrpResult = oModfdColDT.Clone();
                string GrpColName = GrpColNames[0].ToString().Replace(" ", ""); ; //oModfdColDT.Columns[0].ColumnName.ToString();
                foreach (DataRow oDRUniq in UniqValsDS.Tables[0].Rows)
                {
                    GroupFilterExpr = GrpColName + "='" + oDRUniq[0].ToString() + "'";
                    DataRow oDRGrpHeader = oDTGrpResult.NewRow();
                    for (int Level = 0; Level <= oDRGrpHeader.ItemArray.Length - 1; Level++)
                    {
                        oDRGrpHeader[Level] = "Level-1";
                    }
                    oDTGrpResult.Rows.Add(oDRGrpHeader);
                    foreach (DataRow oDRMain in oModfdColDT.Select(GroupFilterExpr))
                    {
                        oDTGrpResult.ImportRow(oDRMain);
                    }
                }
                oDTGrpResult.Columns.Remove(GrpColName);
                oResDS.Tables.Add(oDTGrpResult);
            }
            catch (Exception ex)
            {
                oErr.ErrorMsg = ex;
                oErr.CreateLog();
            }
            return oResDS;
        }

        public string BuildGroupedHtml(DataSet oDSHtml)
        {
            string[] ColNames = getcoluumns();

            string ColumnValues = "";
            DataSet oUniqDS = new DataSet();
            oUniqDS = UniqueValueDS(ColNames[0].ToString());
            DataRow[] oDRUniqVals = oUniqDS.Tables[0].Select();
            bool FAddHeader = false;
            int i = 0;
            StringBuilder oStrBuilder = new StringBuilder();
            string Cart = "";
            //oStrBuilder.Append("<style>td{font-family:arial Unicode ms;font-size:12px;}th{font-family:arial Unicode ms;font-size:12px;font-weight:Bold}.GrpInnerHeadStyle{border-bottom: 1px solid silver;font-family: Arial, Helvetica, sans-serif;font-size: 11px;font-weight:bold;color: #042373;width=\"25%\"}</style>");
            //oStrBuilder.Append("<style>.GrpInnerColPrice {font-family: Arial, Helvetica, sans-serif;font-size:12px;background-color: #E7FB88;width=\"20%\"}</style>");
            //oStrBuilder.Append("<style>.GrpInnerColNormal {font-family: Arial, Helvetica, sans-serif;font-size:12px; width=\"40%\"}</style>");
            //oStrBuilder.Append("<style>.GrpInnerHeadStyle{border-bottom: 1px solid silver;font-family: Arial, Helvetica, sans-serif;font-size: 11px;font-weight:bold;color: #042373;width=\"25%\"}</style>");
            //oStrBuilder.Append("<style>.norepeatbg {background-repeat: repeat-x;}</style>");
            //oStrBuilder.Append("<style>.norepeatbg2 {background-repeat: repeat-y;}</style>");
            //oStrBuilder.Append("<style>.norepeatbg3 {background-repeat: repeat-y; background-position: right;}</style>");

            oStrBuilder.Append("<table width=\"100%\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\"><tr> <td align=\"right\" ><TABLE width=\"99%\" border=\"0\">");
            oStrBuilder.Append("<tr valign=\"top\">");
            oStrBuilder.Append("<td><p></p>");

            //oStrBuilder.Append("<table width=\"93%\"  border=\"0\" align=\"center\" cellpadding=\"0\" cellspacing=\"0\">");
            //oStrBuilder.Append("<tr valign=\"top\">");
            //oStrBuilder.Append("<td valign=\"top\" background=\"images/bg/left.jpg\" style=\"width:17px;\">");
            //oStrBuilder.Append("<img src=\"images/bg/top_left.jpg\" width=\"16\" height=\"15\"></td>");
            //oStrBuilder.Append("<td width=\"95%\" valign=\"top\" background=\"images/bg/top_bg.jpg\" class=\"norepeatbg\">");
            //oStrBuilder.Append("</td>");
            //oStrBuilder.Append("<td valign=\"top\" background=\"images/bg/right.jpg\" style=\"width:17px;\">");
            //oStrBuilder.Append("<img src=\"images/bg/top_right.jpg\" width=\"16\" height=\"15\"></td>");
            //oStrBuilder.Append("</tr>");
            //oStrBuilder.Append("<tr>");
            //oStrBuilder.Append("<td background=\"images/bg/left.jpg\" class=\"norepeatbg2\" style=\"width:17px\"></td>");
            //oStrBuilder.Append("<td bgcolor=\"#FFFFFF\">");
            //oStrBuilder.Append("<TABLE BORDER=\"1px\" width=\"700\" CELLSPACING=\"0\" CELLPADDING=\"3\" bordercolor=\"#CCCCCC\"><TR><TD WIDTH=\"550PX\">");
            oStrBuilder.Append("<TABLE BORDER=\"1px\" width=\"100%\" CELLSPACING=\"0\" CELLPADDING=\"0\" bordercolor=black >");
            oStrBuilder.Append("<style>td{font-family:arial Unicode ms;font-size:12px;}th{font-family:arial unicode ms;font-size:12px;font-weight:Bold}</style>" + System.Environment.NewLine + "<TR>");
            try
            {
                foreach (DataColumn oDC in oDSHtml.Tables[0].Columns)
                {
                    //    oHelper.SQLString = "SELECT ATTRIBUTE_NAME  FROM TB_ATTRIBUTE WHERE ATTRIBUTE_ID IN(SELECT DISTINCT(ATTRIBUTE_ID) FROM TB_PROD_SPECS  WHERE LEN(STRING_VALUE)>50) AND PUBLISH2WEB=1";
                    //    DataSet oAttrDs = oHelper.GetDataSet();
                    //    bool AddColWidth = false;
                    //    foreach (DataRow oDr in oAttrDs.Tables[0].Rows)
                    //    {
                    //        if (oDC.ColumnName.ToString() == oDr[0].ToString())
                    //        {
                    //            AddColWidth = true;
                    //        }
                    //    }
                    //    if (AddColWidth == true && DesciptionAttributeWidth > 0)
                    //    {
                    //            oStrBuilder.Append(System.Environment.NewLine + "<TD ALIGN=\"CENTER\" VALIGN=\"TOP\" class=GrpInnerHeadStyle align=left style=\"width:" + DesciptionAttributeWidth + "%\">" + oDC.ColumnName.ToString() + "</TD>");
                    //            AddColWidth = false;
                    //    }
                    //    else
                    //    {
                    if (oDC.ColumnName.ToString().ToUpper() == "PRODUCT_ID")//cart added
                    {
                        Cart = System.Environment.NewLine + "<TD ALIGN=\"Center\" VALIGN=\"Middle\" style=\"width: 150px; border:1px; color: Black; BACKGROUND-COLOR: #99CCFF;bordercolor=black\">Cart</TD>";
                    }
                    else
                    {
                        oStrBuilder.Append(System.Environment.NewLine + "<TD ALIGN=\"Center\" VALIGN=\"Middle\" style=\"width: 150px; border:1px; color: Black; BACKGROUND-COLOR: #99CCFF;bordercolor=black\">" + oDC.ColumnName.ToString() + "</TD>");
                    }
                    // }
                }
                if (Cart != "")
                {
                    oStrBuilder.Append(Cart).ToString();
                    Cart = "";
                }
                oStrBuilder.Append("</TR>");

                foreach (DataRow oDR in oDSHtml.Tables[0].Rows)
                {
                    FAddHeader = false;
                    oStrBuilder.Append(System.Environment.NewLine + "<TR>");
                    foreach (DataColumn oDC in oDSHtml.Tables[0].Columns)
                    {
                        if (oDR[oDC.ColumnName.ToString()].ToString().Contains("<@LEVEL>") && FAddHeader == false)
                        {
                            oStrBuilder.Append(System.Environment.NewLine + "<TD ALIGN=\"LEFT\" class=td COLSPAN=" + (oDR.ItemArray.Length) + "><B>" + oDR[oDC.ColumnName.ToString()].ToString().Replace("<@LEVEL>", "") + "</B></TD>");
                            FAddHeader = true;
                            i++;
                            Cart = "";
                        }
                        else if (!(oDR[oDC.ColumnName.ToString()].ToString().Contains("<@LEVEL>")))
                        {
                            if (oDC.ColumnName.ToString().ToUpper().Contains("COST"))
                            {
                                oStrBuilder.Append(System.Environment.NewLine + "<TD ALIGN=\"LEFT\" VALIGN=\"TOP\" class=\"GrpInnerColPrice\">" + " " + oHelper.GetOptionValues("CURRENCYFORMAT").ToString() + " " + oHelper.FixDecPlace(Convert.ToDecimal(oDR[oDC.ColumnName.ToString()])).ToString() + "</TD>");
                            }
                            else if (oDC.ColumnName.ToString().ToUpper() == "PRODUCT_ID")//cart added
                            {
                                ProdID = Convert.ToInt32(oDR[oDC.ColumnName.ToString()]);
                                if (oPro.GetProductAvailability(ProdID) == 1)
                                {
                                    int Min_ord_qty = oOrder.GetProductMinimumOrderQty(ProdID);
                                    string CartUrl = oHelper.GetOptionValues("CARTURL").ToString();
                                    CartUrl = CartUrl.Replace("{PRODUCT_ID}", ProdID.ToString());
                                    CartUrl = CartUrl.Replace("{MIN_ORD_QTY}", Min_ord_qty.ToString());
                                    string CartImgPath = "<A HREF=\"" + CartUrl + "\" style=\"text-decoration:none\" onMouseOut=\"MM_swapImgRestore()\" onMouseOver=\"MM_swapImage('Image" + ProdID.ToString() + "_mp','','images/but_buy2.gif',1)\"><img src=\"images/but_buy1.gif\" name=\"Image" + ProdID.ToString() + "_mp\" height=\"25\" border=\"0\"></A>";
                                    CartImgPath = "<table><tr><td>" +
                                            "<input valign=\"middle\" name=\"txt" + ProdID.ToString() + "_" + oOrder.GetProductAvilableQty(ProdID).ToString() + "_" + Min_ord_qty.ToString() + "_" + FamilyID.ToString() + "\" type=\"text\" size=\"5\" id=\"txt" + ProdID.ToString() + "_" + oOrder.GetProductAvilableQty(ProdID).ToString() + "_" + Min_ord_qty.ToString() + "_" + FamilyID.ToString() + "\" style=\"background-color:#FFFFFF;color: #000000;height=23;\"   /> " +
                                          "</td><td>" +
                                          "  <a style=\"cursor:pointer;\" valign=\"middle\"  onMouseOut=\"MM_swapImgRestore()\" onMouseOver=\"MM_swapImage('Image" + ProdID.ToString() + "_fp','','images/but_buy2.gif',1)\">" +
                                          "   <img src=\"images/but_buy1.gif\" name=\"Image" + ProdID.ToString() + "_fp\" width=\"76\" height=\"25\" border=\"0\" onClick=\"productbuy('txt" + ProdID.ToString() + "_" + oOrder.GetProductAvilableQty(ProdID).ToString() + "_" + Min_ord_qty.ToString() + "_" + FamilyID.ToString() + "','" + ProdID.ToString() + "');\"/>" +
                                          "</a></td></tr></table>";
                                    Cart = System.Environment.NewLine + "<TD ALIGN=\"CENTER\" VALIGN=\"TOP\" class=GrpInnerHeadStyle align=left>" + CartImgPath.ToString() + "</TD>";
                                }
                                else
                                {
                                    Cart = System.Environment.NewLine + "<TD ALIGN=\"CENTER\" VALIGN=\"TOP\" class=GrpInnerHeadStyle align=left>N/A</TD>";
                                }
                            }
                            else if ((oDR[oDC.ColumnName.ToString()].ToString()) == "")
                            {
                                oStrBuilder.Append(System.Environment.NewLine + "<TD ALIGN=\"LEFT\" VALIGN=\"TOP\" class=td>&nbsp</TD>");
                            }
                            else
                            {

                                if (oDR[oDC.ColumnName.ToString()].ToString().Length >= 9 && oDR[oDC.ColumnName.ToString()].ToString().Length <= 25)
                                {
                                    //DesciptionMidumAttributeWidth = 80;
                                    ColumnValues = oDR[oDC.ColumnName.ToString()].ToString().Replace("<br/>", "");
                                    ColumnValues = ColumnValues.Replace("&nbsp", "");
                                    oStrBuilder.Append(System.Environment.NewLine + "<TD ALIGN=\"LEFT\" VALIGN=\"TOP\" class=\"GrpInnerColNormal width=\"" + DesciptionMidumAttributeWidth + "px\" NOWRAP>" + ColumnValues.ToString() + "</TD>");
                                }
                                else if (oDR[oDC.ColumnName.ToString()].ToString().Length >= 26 && oDR[oDC.ColumnName.ToString()].ToString().Length <= 50)
                                {
                                    // DesciptionNormalAttributeWidth = 140;
                                    ColumnValues = oDR[oDC.ColumnName.ToString()].ToString().Replace("<br/>", "");
                                    ColumnValues = ColumnValues.Replace("&nbsp", "");
                                    oStrBuilder.Append(System.Environment.NewLine + "<TD ALIGN=\"LEFT\" VALIGN=\"TOP\" class=\"GrpInnerColNormal width=\"" + DesciptionNormalAttributeWidth + "px\" NOWRAP>" + ColumnValues.ToString() + "</TD>");
                                }
                                else if (oDR[oDC.ColumnName.ToString()].ToString().Length >= 51 && oDR[oDC.ColumnName.ToString()].ToString().Length <= 100)
                                {
                                    //DesciptionHighAttributeWidth = 180;
                                    ColumnValues = oDR[oDC.ColumnName.ToString()].ToString().Replace("<br/>", "");
                                    ColumnValues = ColumnValues.Replace("&nbsp", "");
                                    oStrBuilder.Append(System.Environment.NewLine + "<TD ALIGN=\"LEFT\" VALIGN=\"TOP\" class=\"GrpInnerColNormal width=\"" + DesciptionHighAttributeWidth + "px\" NOWRAP>" + ColumnValues.ToString() + "</TD>");
                                }
                                else if (oDR[oDC.ColumnName.ToString()].ToString().Length > 100)
                                {
                                    ColumnValues = oDR[oDC.ColumnName.ToString()].ToString().Replace("<br/>", "");
                                    ColumnValues = ColumnValues.Replace("&nbsp", "");
                                    oStrBuilder.Append(System.Environment.NewLine + "<TD ALIGN=\"LEFT\" VALIGN=\"TOP\" class=\"GrpInnerColNormal width=\"" + DesciptionAttributeWidth + "px\" NOWRAP>" + ColumnValues.ToString() + "</TD>");
                                }
                                else
                                {
                                    oStrBuilder.Append(System.Environment.NewLine + "<TD ALIGN=\"LEFT\" VALIGN=\"TOP\" class=\"GrpInnerColNormal\">" + oDR[oDC.ColumnName.ToString()] + "</TD>");
                                }
                            }

                        }
                    }
                    if (Cart != "")
                        oStrBuilder.Append(Cart.ToString());
                    oStrBuilder.Append(System.Environment.NewLine + "</TR>");
                }


                oStrBuilder.Append("</TABLE>");

                //oStrBuilder.Append("</TD>");

                //oStrBuilder.Append("<td background=\"Images/bg/right.jpg\" class=\"norepeatbg3\" style=\"width: 17px\">&nbsp;</td>");
                //oStrBuilder.Append("</TR>");

                //oStrBuilder.Append("<tr valign=\"top\">");
                //oStrBuilder.Append("<td style=\"width: 17px; height: 19px;\"><img src=\"images/bg/bot_left.jpg\" width=\"16\" height=\"15\"></td>");
                //oStrBuilder.Append("<td background=\"images/bg/bot_bg.jpg\" class=\"norepeatbg\" style=\"height: 19px\"></td>");
                //oStrBuilder.Append("<td style=\"width: 17px; height: 19px;\"><img src=\"images/bg/bot_right.jpg\" width=\"16\" height=\"15\"></td>");
                //oStrBuilder.Append("</tr>");

                //oStrBuilder.Append("</TABLE>");
                oStrBuilder.Append("</TD>"); oStrBuilder.Append("</TR>"); oStrBuilder.Append("</TABLE></TD></TR></TABLE>");
                if (oDSHtml.Tables[0].Rows.Count == 0 || oDSHtml == null)
                {
                    oStrBuilder.Remove(0, oStrBuilder.ToString().Length);
                }
            }
            catch (Exception ex)
            {
                oErr.ErrorMsg = ex;
                oErr.CreateLog();
                oStrBuilder = null;
            }
            return oStrBuilder.ToString();

        }

        #region comment1
        //Commented General Method for Build Html
        //public string BuildGroupedHtml(DataSet oDSHtml)
        //{
        //    string[] ColNames = getcoluumns();
        //    DataSet oUniqDS = new DataSet();
        //    oUniqDS = UniqueValueDS(ColNames[0].ToString());
        //    DataRow[] oDRUniqVals = oUniqDS.Tables[0].Select();
        //    bool FAddHeader = false;
        //    int i = 0;
        //    StringBuilder oStrBuilder = new StringBuilder();
        //    oStrBuilder.Append("<style>td{font-family:arial Unicode ms;font-size:12px;}th{font-family:arial Unicode ms;font-size:12px;font-weight:Bold}</style> "
        //        + "<TABLE BORDER=\"1px\" CELLSPACING=\"0\" CELLPADDING=\"3\" bordercolor=black>" + System.Environment.NewLine + "<TR>");

        //    try
        //    {
        //        foreach (DataColumn oDC in oDSHtml.Tables[0].Columns)
        //        {
        //            oStrBuilder.Append(System.Environment.NewLine + "<TD class=th BGCOLOR=#99CCFF align=center>" + oDC.ColumnName.ToString() + "</TD>");
        //        }
        //        oStrBuilder.Append("</TR>");

        //        foreach (DataRow oDR in oDSHtml.Tables[0].Rows)
        //        {
        //            FAddHeader = false;
        //            oStrBuilder.Append(System.Environment.NewLine + "<TR>");
        //            foreach (DataColumn oDC in oDSHtml.Tables[0].Columns)
        //            {
        //                if (oDR[oDC.ColumnName.ToString()].ToString().Contains("<@LEVEL>") && FAddHeader == false)
        //                {
        //                    oStrBuilder.Append(System.Environment.NewLine + "<TD class=td BGCOLOR=#99DDFF COLSPAN=" + (oDR.ItemArray.Length + 1) + ">" + oDR[oDC.ColumnName.ToString()].ToString().Replace("<@LEVEL>", "") + "</TD>");
        //                    FAddHeader = true;
        //                    i++;
        //                }
        //                else if (!(oDR[oDC.ColumnName.ToString()].ToString().Contains("<@LEVEL>")))
        //                {
        //                    if (oDC.ColumnName.ToString().ToUpper().Contains("PRICE"))
        //                    {
        //                        oStrBuilder.Append(System.Environment.NewLine + "<TD class=td BGCOLOR=\"#E7FB88\">" + oDR[oDC.ColumnName.ToString()] + "</TD>");
        //                    }
        //                    else if ((oDR[oDC.ColumnName.ToString()].ToString()) == "")
        //                    {
        //                        oStrBuilder.Append(System.Environment.NewLine + "<TD class=td>&nbsp</TD>");
        //                    }
        //                    else
        //                    {
        //                        oStrBuilder.Append(System.Environment.NewLine + "<TD class=td>" + oDR[oDC.ColumnName.ToString()] + "</TD>");
        //                    }
        //                }
        //            }
        //            oStrBuilder.Append(System.Environment.NewLine + "</TR>");
        //        }
        //        oStrBuilder.Append(System.Environment.NewLine + "</TABLE>");
        //    }
        //    catch (Exception ex)
        //    {
        //        oErr.ErrorMsg = ex;
        //        oErr.CreateLog();
        //        oStrBuilder = null;
        //    }
        //    return oStrBuilder.ToString();

        //}
        #endregion

        public DataSet BuildGroupedDSTest(DataSet oDS)
        {
            string[] GrpColNames;
            GrpColNames = getcoluumns();
            bool ColumnlSetUp = false;
            oModfdColDT = new DataTable();
            foreach (DataColumn oDC in oDS.Tables[0].Columns)
            {
                DataColumn oNewColumn = new DataColumn();
                if (GrpColNames.Length == 1)
                {
                    if (GrpColNames[0].Contains(oDC.ColumnName))
                    {
                        oNewColumn = new DataColumn(ReplaceSpecialChar(oDC.ColumnName), System.Type.GetType("System.String"));
                    }
                    else
                    {
                        oNewColumn = new DataColumn(oDC.ColumnName, System.Type.GetType("System.String"));
                    }
                }
                else if (GrpColNames.Length == 2)
                {
                    if (GrpColNames[0].Contains(oDC.ColumnName) || GrpColNames[1].Contains(oDC.ColumnName))
                    {
                        oNewColumn = new DataColumn(ReplaceSpecialChar(oDC.ColumnName), System.Type.GetType("System.String"));
                    }
                    else
                    {
                        oNewColumn = new DataColumn(oDC.ColumnName, System.Type.GetType("System.String"));
                    }
                }
                else if (GrpColNames.Length == 3)
                {
                    if (GrpColNames[0].Contains(oDC.ColumnName) || GrpColNames[1].Contains(oDC.ColumnName) || GrpColNames[2].Contains(oDC.ColumnName))
                    {
                        oNewColumn = new DataColumn(ReplaceSpecialChar(oDC.ColumnName), System.Type.GetType("System.String"));
                    }
                    else
                    {
                        oNewColumn = new DataColumn(oDC.ColumnName, System.Type.GetType("System.String"));
                    }
                }
                else if (GrpColNames.Length == 4)
                {
                    if (GrpColNames[0].Contains(oDC.ColumnName) || GrpColNames[1].Contains(oDC.ColumnName) || GrpColNames[2].Contains(oDC.ColumnName) || GrpColNames[3].Contains(oDC.ColumnName))
                    {
                        oNewColumn = new DataColumn(ReplaceSpecialChar(oDC.ColumnName), System.Type.GetType("System.String"));
                    }
                    else
                    {
                        oNewColumn = new DataColumn(oDC.ColumnName, System.Type.GetType("System.String"));
                    }
                }
                else
                {
                    oNewColumn = new DataColumn(oDC.ColumnName, System.Type.GetType("System.String"));
                }

                oModfdColDT.Columns.Add(oNewColumn);
            }
            DataRow[] oDR = oDS.Tables[0].Select();
            foreach (DataRow oDataRow in oDR)
            {
                DataRow oDRTemp;
                oDRTemp = oModfdColDT.NewRow();
                oDRTemp.ItemArray = oDataRow.ItemArray;
                oModfdColDT.Rows.Add(oDRTemp);
            }
            //oModfdColDT.Columns.Remove("PRODUCT_ID");
            DataSet oDSLevUniqVals = new DataSet();
            DataSet oDSTemp = new DataSet();
            int i, j, k, l, m = 0;
            int[] RowsCount = new int[10];
            for (int GrpLevel = 0; GrpLevel < SeparateLine.Length - 1; GrpLevel++)
            {
                oDSTemp = UniqueValueDS(GrpColNames[GrpLevel].ToString());
                DataTable oDT = new DataTable();
                oDT = oDSTemp.Tables[0].Copy();
                oDT.TableName = "Table" + GrpLevel;
                oDSLevUniqVals.Tables.Add(oDT);
            }

            //Set the Count Value for Condition Expressions
            int Count = 1;
            foreach (DataTable oDTTemp in oDSLevUniqVals.Tables)
            {
                Count = Count * oDTTemp.Rows.Count;
            }

            //Build the Condition Expression
            string[] CondExpr = new string[Count];
            if (oDSLevUniqVals != null)
            {
                for (i = 0; i < oDSLevUniqVals.Tables[0].Rows.Count; i++)
                {
                    if (oDSLevUniqVals.Tables.Count > 1)
                    {
                        for (j = 0; j < oDSLevUniqVals.Tables[1].Rows.Count; j++)
                        {
                            if (oDSLevUniqVals.Tables.Count > 2)
                            {
                                for (k = 0; k < oDSLevUniqVals.Tables[2].Rows.Count; k++)
                                {
                                    if (oDSLevUniqVals.Tables.Count > 3)
                                    {
                                        for (l = 0; l < oDSLevUniqVals.Tables[3].Rows.Count; l++)
                                        {
                                            CondExpr[m] = ReplaceSpecialChar(GrpColNames[0].ToString()) + " = '" + oDSLevUniqVals.Tables[0].Rows[i][0].ToString() + "' AND " + ReplaceSpecialChar(GrpColNames[1].ToString()) + " = '" + oDSLevUniqVals.Tables[1].Rows[j][0].ToString() + "' AND " + ReplaceSpecialChar(GrpColNames[2].ToString()) + " = '" + oDSLevUniqVals.Tables[2].Rows[k][0].ToString() + "' AND " + ReplaceSpecialChar(GrpColNames[3].ToString()) + " = '" + oDSLevUniqVals.Tables[3].Rows[l][0].ToString() + "'";
                                            m++;
                                        }
                                    }
                                    if (oDSLevUniqVals.Tables.Count == 3)
                                    {
                                        CondExpr[m] = ReplaceSpecialChar(GrpColNames[0].ToString()) + " = '" + oDSLevUniqVals.Tables[0].Rows[i][0].ToString() + "' AND " + ReplaceSpecialChar(GrpColNames[1].ToString()) + " = '" + oDSLevUniqVals.Tables[1].Rows[j][0].ToString() + "' AND " + ReplaceSpecialChar(GrpColNames[2].ToString()) + " = '" + oDSLevUniqVals.Tables[2].Rows[k][0].ToString() + "'";
                                        m++;
                                    }
                                }
                            }
                            if (oDSLevUniqVals.Tables.Count == 2)
                            {
                                CondExpr[m] = ReplaceSpecialChar(GrpColNames[0].ToString()) + " = '" + oDSLevUniqVals.Tables[0].Rows[i][0].ToString() + "' AND " + ReplaceSpecialChar(GrpColNames[1].ToString()) + " = '" + oDSLevUniqVals.Tables[1].Rows[j][0].ToString() + "'";
                                m++;
                            }
                        }
                    }
                    if (oDSLevUniqVals.Tables.Count == 1)
                    {
                        CondExpr[m] = ReplaceSpecialChar(GrpColNames[0].ToString()) + " = '" + oDSLevUniqVals.Tables[0].Rows[i][0].ToString() + "'";
                        m++;
                    }
                }
            }
            //Reset the table modified(without Special Character) column
            oDS.Tables.Clear();
            oDS.Tables.Add(oModfdColDT);
            DataSet oDSRst = new DataSet();
            oDSRst = oDS.Clone();

            //Build the Dataset for Grouped Table
            string[] TmpStr = new string[4];
            string TmpStrHeader = "";
            for (int TmpStrCount = 0; TmpStrCount < TmpStr.Length; TmpStrCount++)
            {
                TmpStr[TmpStrCount] = "";
            }
            for (int CondCount = 0; CondCount < Count; CondCount++)
            {
                foreach (DataRow oDRTemp in oDS.Tables[0].Select(CondExpr[CondCount]))
                {
                    #region "Add the row for Group Level Header"
                    string[] FlagHeadersCheck = new string[] { "" };
                    string[] StrSeparator = new string[] { " AND " };
                    FlagHeadersCheck = CondExpr[CondCount].Split(StrSeparator, StringSplitOptions.None);
                    DataRow oDRGrpHeader;
                    if (FlagHeadersCheck.Length > 0)
                    {
                        if (FlagHeadersCheck[0] != TmpStr[0] || TmpStr[0] == string.Empty)
                        {
                            oDRGrpHeader = oDSRst.Tables[0].NewRow();
                            TmpStrHeader = FlagHeadersCheck[0].Substring(FlagHeadersCheck[0].IndexOf("= \'") + 3);
                            TmpStrHeader = TmpStrHeader.Remove(TmpStrHeader.Length - 1);
                            for (int Level = 0; Level <= oDRGrpHeader.ItemArray.Length - 1; Level++)
                            {
                                if (TmpStrHeader.ToString().Contains("."))
                                {
                                    try
                                    {
                                        TmpStrHeader = oHelper.FixDecPlace(Convert.ToDecimal(TmpStrHeader)).ToString();
                                        TmpStrHeader = oHelper.GetOptionValues("CURRENCYFORMAT").ToString() + " " + TmpStrHeader;
                                    }
                                    catch (Exception ex)
                                    {
                                    }
                                }
                                oDRGrpHeader[Level] = "<@LEVEL>" + TmpStrHeader;// CondExpr[CondCount];
                            }
                            oDSRst.Tables[0].Rows.Add(oDRGrpHeader);
                            TmpStr[0] = FlagHeadersCheck[0];
                        }
                        if (FlagHeadersCheck.Length > 1)
                        {
                            if ((FlagHeadersCheck[0] + FlagHeadersCheck[1]) != TmpStr[1] || TmpStr[1] == string.Empty)
                            {
                                oDRGrpHeader = oDSRst.Tables[0].NewRow();
                                TmpStrHeader = FlagHeadersCheck[1].Substring(FlagHeadersCheck[1].IndexOf("= \'") + 3);
                                TmpStrHeader = TmpStrHeader.Remove(TmpStrHeader.Length - 1);
                                try
                                {
                                    if (TmpStrHeader.ToString().Contains("."))
                                    {
                                        try
                                        {
                                            TmpStrHeader = oHelper.FixDecPlace(Convert.ToDecimal(TmpStrHeader)).ToString();
                                            TmpStrHeader = oHelper.GetOptionValues("CURRENCYFORMAT").ToString() + " " + TmpStrHeader;
                                        }
                                        catch (Exception ex)
                                        {

                                        }
                                    }
                                }
                                catch (Exception Ex)
                                {

                                }
                                for (int Level = 0; Level <= oDRGrpHeader.ItemArray.Length - 1; Level++)
                                {
                                    if (TmpStrHeader.ToString().Contains("."))
                                    {
                                        try
                                        {
                                            TmpStrHeader = oHelper.FixDecPlace(Convert.ToDecimal(TmpStrHeader)).ToString();
                                            TmpStrHeader = oHelper.GetOptionValues("CURRENCYFORMAT").ToString() + " " + TmpStrHeader;
                                        }
                                        catch (Exception ex)
                                        {
                                        }
                                    }
                                    oDRGrpHeader[Level] = "<@LEVEL>" + TmpStrHeader;// CondExpr[CondCount];
                                }
                                oDSRst.Tables[0].Rows.Add(oDRGrpHeader);
                                TmpStr[1] = (FlagHeadersCheck[0] + FlagHeadersCheck[1]);
                            }
                            if (FlagHeadersCheck.Length > 2)
                            {
                                if ((FlagHeadersCheck[0] + FlagHeadersCheck[1] + FlagHeadersCheck[2]) != TmpStr[2] || TmpStr[2] == string.Empty)
                                {
                                    oDRGrpHeader = oDSRst.Tables[0].NewRow();
                                    TmpStrHeader = FlagHeadersCheck[2].Substring(FlagHeadersCheck[2].IndexOf("= \'") + 3);
                                    TmpStrHeader = TmpStrHeader.Remove(TmpStrHeader.Length - 1);
                                    for (int Level = 0; Level <= oDRGrpHeader.ItemArray.Length - 1; Level++)
                                    {
                                        if (TmpStrHeader.ToString().Contains("."))
                                        {
                                            try
                                            {
                                                TmpStrHeader = oHelper.FixDecPlace(Convert.ToDecimal(TmpStrHeader)).ToString();
                                                TmpStrHeader = oHelper.GetOptionValues("CURRENCYFORMAT").ToString() + " " + TmpStrHeader;
                                            }
                                            catch (Exception ex)
                                            {
                                            }
                                        }
                                        oDRGrpHeader[Level] = "<@LEVEL>" + TmpStrHeader;// CondExpr[CondCount];
                                    }
                                    oDSRst.Tables[0].Rows.Add(oDRGrpHeader);
                                    TmpStr[2] = (FlagHeadersCheck[0] + FlagHeadersCheck[1] + FlagHeadersCheck[2]);
                                }
                                if (FlagHeadersCheck.Length > 3)
                                {
                                    if ((FlagHeadersCheck[0] + FlagHeadersCheck[1] + FlagHeadersCheck[2] + FlagHeadersCheck[3]) != TmpStr[3] || TmpStr[3] == string.Empty)
                                    {
                                        oDRGrpHeader = oDSRst.Tables[0].NewRow();
                                        TmpStrHeader = FlagHeadersCheck[3].Substring(FlagHeadersCheck[3].IndexOf("= \'") + 3);
                                        TmpStrHeader = TmpStrHeader.Remove(TmpStrHeader.Length - 1);
                                        for (int Level = 0; Level <= oDRGrpHeader.ItemArray.Length - 1; Level++)
                                        {
                                            if (TmpStrHeader.ToString().Contains("."))
                                            {
                                                try
                                                {
                                                    TmpStrHeader = oHelper.FixDecPlace(Convert.ToDecimal(TmpStrHeader)).ToString();
                                                    TmpStrHeader = oHelper.GetOptionValues("CURRENCYFORMAT").ToString() + " " + TmpStrHeader;
                                                }
                                                catch (Exception ex)
                                                {
                                                }
                                            }
                                            oDRGrpHeader[Level] = "<@LEVEL>" + TmpStrHeader;// CondExpr[CondCount];
                                        }
                                        oDSRst.Tables[0].Rows.Add(oDRGrpHeader);
                                        TmpStr[3] = (FlagHeadersCheck[0] + FlagHeadersCheck[1] + FlagHeadersCheck[2] + FlagHeadersCheck[3]);
                                    }
                                }
                            }
                        }
                    }
                    #endregion "Add the row for Group Level Header"
                    //oDR[CondCount] = oDRTemp;
                    //Add the Rows for Grouped Table
                    oDSRst.Tables[0].ImportRow(oDRTemp);
                }
            }

            //Removing Grouped Column in Dataset
            for (int DC = 0; DC < GrpColNames.Length; DC++)
            {
                string TempColNames = ReplaceSpecialChar(GrpColNames[DC]);
                oDSRst.Tables[0].Columns.Remove(TempColNames);
            }
            return oDSRst;
        }

        public DataSet UniqueValueDS(string AttrName)
        {
            DataSet oDSUniqVals = new DataSet();
            try
            {
                oHelper.SQLString = "SELECT ATTRIBUTE_ID,ATTRIBUTE_DATATYPE FROM TB_ATTRIBUTE WHERE ATTRIBUTE_NAME='" + AttrName + "'";
                int AttrID = oHelper.CI(oHelper.GetValue("ATTRIBUTE_ID").ToString());
                if ("NUM" == oHelper.CS(oHelper.GetValue("ATTRIBUTE_DATATYPE").ToString()).ToString().ToUpper().Substring(0, 3))
                    oHelper.SQLString = "SELECT DISTINCT NUMERIC_VALUE  FROM TB_PROD_SPECS WHERE ATTRIBUTE_ID = " + AttrID + " AND PRODUCT_ID IN "
                         + "(SELECT PRODUCT_ID FROM TB_PROD_FAMILY WHERE FAMILY_ID =" + FamilyID + ")";
                else
                    oHelper.SQLString = "SELECT DISTINCT STRING_VALUE  FROM TB_PROD_SPECS WHERE ATTRIBUTE_ID = " + AttrID + " AND PRODUCT_ID IN "
                             + "(SELECT PRODUCT_ID FROM TB_PROD_FAMILY WHERE FAMILY_ID =" + FamilyID + ")";
                oDSUniqVals = oHelper.GetDataSet();
            }
            catch (Exception ex)
            {
                oErr.ErrorMsg = ex;
                oErr.CreateLog();
                oDSUniqVals = null;
            }
            return oDSUniqVals;
        }

        [WebMethod]
        public string[] getcoluumns()
        {
            string FilePath = "";
            string RawData;
            int Keyindex;
            string Con = oCon.ConnectionString.ToString();
            Con = Con.Remove(0, Con.IndexOf(';') + 1);
            oConn.ConnSettings(Con);
            string layoutXML = string.Empty;
            string sqlStr = "SELECT PRODUCT_TABLE_STRUCTURE FROM TB_FAMILY WHERE FAMILY_ID = " + m_familyID + "";
            oHelper.SQLString = sqlStr;
            DataSet ds = oHelper.GetDataSet();

            RawData = ds.Tables[0].Rows[0]["PRODUCT_TABLE_STRUCTURE"].ToString();
            //newly added
            //string[] SeparateLine = new string[] { "" };
            string[] StrSeparator = new string[] { "<IsGroupByColumn>true</IsGroupByColumn>" };
            SeparateLine = RawData.Split(StrSeparator, StringSplitOptions.None);
            //newly added

            string[] Columns = new string[SeparateLine.Length - 1];
            int i = 0;
            int st = -1;
            try
            {
                while (RawData.IndexOf(RawData.Substring(RawData.IndexOf("<IsGroupByColumn>true</IsGroupByColumn>", st + 1))) >= 0)
                {
                    int ind = RawData.IndexOf(RawData.Substring(RawData.IndexOf("<IsGroupByColumn>true</IsGroupByColumn>", st + 1)));
                    st = ind;
                    int start = ind - 300;
                    string Temp = RawData.Substring(start, 500);
                    string KeyContent = Temp.Substring(Temp.IndexOf("Key"), 20);
                    string KeyValue = KeyContent.Substring(KeyContent.IndexOf("="), KeyContent.LastIndexOf("\"") - KeyContent.IndexOf("="));
                    KeyValue = KeyValue.Replace("=\"", "").Replace("#", "");
                    string KeySt = "<Key id=" + "\"" + KeyValue + "\">";
                    int ColStart = RawData.IndexOf(KeySt);
                    int KeyEnd = RawData.IndexOf("</Key>", ColStart);
                    string ColText = RawData.Substring(ColStart, KeyEnd - ColStart);
                    string col = ColText.Substring(ColText.IndexOf(">") + 1);
                    Columns[i] = col;
                    i++;
                    int chk = RawData.IndexOf("<IsGroupByColumn>true</IsGroupByColumn>", st + 1);
                    if (chk >= 0)
                        continue;
                    else
                        return Columns;
                    //throw new Exception("");
                }
            }
            catch (Exception ex)
            {
                oErr.ErrorMsg = ex;
                oErr.CreateLog();
            }

            return Columns;

        }

        private string ReplaceSpecialChar(string StrName)
        {
            char c;
            string StrConverted = string.Empty;
            int len = StrName.Length;
            for (int i = 0; i < len; i++)
            {
                c = Convert.ToChar(StrName.Substring(i, 1));
                if (Convert.ToInt32(c) < 48)
                {
                }
                else if (Convert.ToInt32(c) > 57 && Convert.ToInt32(c) < 65)
                {
                }
                else if (Convert.ToInt32(c) > 90 && Convert.ToInt32(c) < 97)
                {
                }
                else if (Convert.ToInt32(c) > 122)
                {
                }
                else
                {
                    StrConverted = StrConverted + c;
                }

            }
            return StrConverted;
        }


        #region "Commented Old Method"
        //[WebMethod]
        //public String GenHTMLGroupedTable()
        //{
        //    // DBConnection Ocon = new DBConnection();

        //    HeaderID = 1;
        //    cellvalues = ""; strBuildXMLOutput.Remove(0, strBuildXMLOutput.Length); strBuildXMLOutput1.Remove(0, strBuildXMLOutput1.Length);
        //    int GrpExtcheck; GrpExtcheck = 0; grpno = 0;
        //    int[] a = new int[obj.DisplayLayout.Bands[0].Columns.Count];
        //    int[] b = new int[obj.DisplayLayout.Bands[0].Columns.Count];
        //    int[] GrpCol = new int[obj.DisplayLayout.Bands[0].Columns.Count];
        //    cellposition = new int[obj.DisplayLayout.Bands[0].Columns.Count];
        //    cellposval = new int[obj.DisplayLayout.Bands[0].Columns.Count];
        //    GrpColno = new int[obj.DisplayLayout.Bands[0].Columns.Count];
        //    rowlevel = new int[obj.DisplayLayout.Bands[0].Columns.Count + 1];


        //    for (int k = 0; k < obj.DisplayLayout.Bands[0].Columns.Count; k++)
        //    { b[k] = 0; }


        //    //groupby column finding 
        //    strBuildXMLOutput.Append("<HTML><style>td{font-family:arial Unicode ms;font-size:12px;}th{font-family:arial Unicode ms;font-size:12px;font-weight:Bold}</style><BODY><br><center><TABLE border=0 cellspacing=1 style=\"background-color:black\" cellpadding=3>");
        //    for (int GrpColcnt = 0, grpcol = 0; GrpColcnt < obj.DisplayLayout.Bands[0].Columns.Count; GrpColcnt++)
        //    {
        //        if (obj.DisplayLayout.Bands[0].Columns[GrpColcnt].IsGroupByColumn)
        //        {
        //            GrpCol[grpcol] = 0; grpcol++;
        //            GrpExtcheck = GrpCol.Length;
        //            cellposval[GrpColcnt] = -1; GrpColno[GrpColcnt] = GrpColcnt;
        //        }
        //        else
        //        {
        //            GrpColno[GrpColcnt] = GrpColcnt; grpno++;
        //            cellposval[GrpColcnt] = 0;
        //        }
        //        cellposition[GrpColcnt] = obj.DisplayLayout.Bands[0].Columns[GrpColcnt].Header.VisiblePosition;
        //    }

        //    for (int CellPos = 0; CellPos < obj.DisplayLayout.Bands[0].Columns.Count; CellPos++)
        //    {
        //        for (int CellValue = 0; CellValue < obj.DisplayLayout.Bands[0].Columns.Count; CellValue++)
        //        {
        //            if (GrpColno[CellPos] == cellposition[CellValue])
        //            { GrpColno[CellPos] = CellValue; break; }
        //        }
        //    }
        //    for (int CellPos = 0; CellPos < obj.DisplayLayout.Bands[0].Columns.Count; CellPos++)
        //    {
        //        if (cellposval[CellPos] == -1)
        //        {
        //            for (int CellValue = 0; CellValue < obj.DisplayLayout.Bands[0].Columns.Count; CellValue++)
        //            {
        //                if (GrpColno[CellValue] == CellPos)
        //                { GrpColno[CellValue] = -1; break; }
        //            }
        //        }
        //    }
        //    int tottsize = obj.DisplayLayout.Bands[0].Columns.Count;
        //    while (tottsize > 0)
        //    {
        //        for (int CellPos = 0; CellPos < obj.DisplayLayout.Bands[0].Columns.Count; CellPos++)
        //        {
        //            if (GrpColno[CellPos] == -1)
        //            {
        //                for (int CellValue = CellPos; CellValue < tottsize - 1; CellValue++)
        //                {
        //                    GrpColno[CellValue] = GrpColno[CellValue + 1];
        //                }
        //            }
        //        } tottsize--;
        //    }
        //    if ((obj.DisplayLayout.Bands[0].Columns.Count - grpno) <= 4)
        //    {

        //        //header
        //        if (HeaderID == 1 && DisplayHeaders == true)
        //        {
        //            strBuildXMLOutput1.Append("<tr style=\"background-color:white\">");
        //            for (int cellheader = 0; cellheader < grpno; cellheader++)
        //            {
        //                {
        //                    strBuildXMLOutput1.Append("<th BGCOLOR=#99CCFF align=center  valign=middle style=\"width: 200px; color: Black; BACKGROUND-COLOR: #99CCFF \">");
        //                    strBuildXMLOutput1.Append(obj.DisplayLayout.Bands[0].Columns[GrpColno[cellheader]].ToString());
        //                    strBuildXMLOutput1.Append("</th>");
        //                    //HtmlBuilder.Append(""
        //                }
        //            }
        //            strBuildXMLOutput1.Append("</tr>");
        //        }
        //        if (grpno != 0) { totalcols = grpno; totalrows = HeaderID;/* + grdAdvanceProductTable.DisplayLayout.Rows.Count;*/ } else { totalcols = 1; grpno = 1; totalrows = 0; AllinGroup = 1; }
        //        //endheader

        //        //Without grouping// Merging && Non // 
        //        if (GrpExtcheck == 0)
        //        {
        //            for (int i = 0; i < obj.DisplayLayout.Rows.Count; i++)
        //            {
        //                totalrows++; strBuildXMLOutput1.Append("<tr style=\"background-color:white\">");
        //                for (int j = 0; j < obj.DisplayLayout.Bands[0].Columns.Count; j++)
        //                {
        //                    if (obj.DisplayLayout.Rows[i].Cells[GrpColno[j]].GetMergedCells() != null)
        //                    {
        //                        if (b[j] == 0)
        //                        {

        //                            a[j] = obj.DisplayLayout.Rows[i].Cells[GrpColno[j]].GetMergedCells().Length;
        //                            b[j] = obj.DisplayLayout.Rows[i].Cells[GrpColno[j]].GetMergedCells().Length - 1;
        //                            cellvalues = cellvalues + "//" + obj.DisplayLayout.Rows[i].Cells[GrpColno[j]].Value.ToString();
        //                            if (a[j] == 0) { crow = 1; } else { crow = a[j]; }
        //                            if (crow > 1)
        //                            {
        //                                string ALIGN = "LEFT";
        //                                oHelper.SQLString = "SELECT ATTRIBUTE_DATATYPE FROM TB_ATTRIBUTE WHERE ATTRIBUTE_NAME = N'" + obj.DisplayLayout.Bands[0].Columns[GrpColno[j]].ToString() + "'";
        //                                DataSet DSSS = oHelper.GetDataSet();
        //                                if (chkAttrType[j] == 4 || DSSS.Tables[0].Rows[0].ItemArray[0].ToString().Substring(0, 3).ToUpper() == "NUM")
        //                                    ALIGN = "right";
        //                                strBuildXMLOutput1.Append("<td rowspan=" + crow + " align=" + ALIGN + "  valign=middle>");
        //                            }
        //                            else
        //                            {
        //                                string ALIGN = "LEFT";
        //                                if (chkAttrType[j] == 4) ALIGN = "right"; strBuildXMLOutput1.Append("<td align=" + ALIGN + " valign=middle>");
        //                            }
        //                            //DBConnection connn = new DBConnection();
        //                            Helper connn = new Helper();

        //                            string ValueFortag = String.Empty;
        //                            string GrpColVal = obj.DisplayLayout.Rows[i].Cells[GrpColno[j]].Value.ToString();
        //                            connn.SQLString = "SELECT ATTRIBUTE_ID FROM TB_ATTRIBUTE WHERE ATTRIBUTE_NAME = N'" + obj.DisplayLayout.Bands[0].Columns[GrpColno[j]].ToString() + "'";
        //                            //DataSet DSS = connn.CreateDataSet();
        //                            DataSet DSS = connn.GetDataSet();
        //                            ExtractCurrenyFormat(Convert.ToInt32(DSS.Tables[0].Rows[0].ItemArray[0].ToString()));
        //                            if ((Headeroptions == "All") || (Headeroptions != "All" && i == 0))
        //                            {
        //                                if ((EmptyCondition == "Null" || EmptyCondition == "Empty" || EmptyCondition == null) && (GrpColVal == string.Empty))
        //                                {
        //                                    ValueFortag = ReplaceText;
        //                                }
        //                                else if ((GrpColVal.ToString()) == (EmptyCondition))
        //                                {
        //                                    ValueFortag = ReplaceText;
        //                                }
        //                                else
        //                                {
        //                                    if (Isnumber(GrpColVal.ToString().Replace("\r", "<br/>").Replace("\n", "&nbsp;")) == true)
        //                                    {
        //                                        ValueFortag = Prefix + " " + Convert.ToDouble(GrpColVal.ToString().Replace("\r", "<br/>").Replace("\n", "&nbsp;")).ToString() + " " + Suffix;
        //                                    }
        //                                    else
        //                                    {
        //                                        ValueFortag = Prefix + " " + GrpColVal.ToString().Replace("\r", "<br/>").Replace("\n", "&nbsp;") + " " + Suffix;
        //                                    }
        //                                }
        //                            }
        //                            else
        //                            {
        //                                if (Isnumber(GrpColVal.ToString().Replace("\r", "<br/>").Replace("\n", "&nbsp;")) == true)
        //                                {
        //                                    ValueFortag = Convert.ToDouble(GrpColVal.ToString().Replace("\r", "<br/>").Replace("\n", "&nbsp;")).ToString();
        //                                }
        //                                else
        //                                {
        //                                    ValueFortag = GrpColVal.ToString().Replace("\r", "<br/>").Replace("\n", "&nbsp;");
        //                                }
        //                            }
        //                            if (obj.DisplayLayout.Rows[i].Cells[GrpColno[GrpColno[j]]].Value.ToString() == "")
        //                            {
        //                                strBuildXMLOutput1.Append(ValueFortag);
        //                            }
        //                            else
        //                            {
        //                                if (chkAttrType[j] == 3)
        //                                    strBuildXMLOutput1.Append(getCellStrin(obj.DisplayLayout.Rows[i].Cells[GrpColno[GrpColno[j]]].Value.ToString()));
        //                                else
        //                                    strBuildXMLOutput1.Append((ValueFortag.ToString().Replace("\r", "<br/>").Replace("\n", "&nbsp;")));
        //                            }

        //                            strBuildXMLOutput1.Append("</td>");
        //                        }
        //                        else
        //                        {
        //                            cellvalues = cellvalues + "/////";
        //                            a[j]--; b[j]--;
        //                        }
        //                    }
        //                    else
        //                    {
        //                        cellvalues = cellvalues + "//" + obj.DisplayLayout.Rows[i].Cells[GrpColno[j]].Value.ToString();
        //                        string ALIGN = "LEFT";
        //                        oHelper.SQLString = "SELECT ATTRIBUTE_DATATYPE FROM TB_ATTRIBUTE WHERE ATTRIBUTE_NAME = N'" + obj.DisplayLayout.Bands[0].Columns[GrpColno[j]].ToString() + "'";
        //                        DataSet DSS = oHelper.GetDataSet();
        //                        if (chkAttrType[j] == 4 || DSS.Tables[0].Rows[0].ItemArray[0].ToString().Substring(0, 3).ToUpper() == "NUM")
        //                            ALIGN = "right";
        //                        if (a[j] == 0) { crow = 1; } else { crow = a[j]; }
        //                        if (crow > 1)
        //                        {
        //                            strBuildXMLOutput1.Append("<td rowspan=" + crow + "  align=" + ALIGN + " valign=middle>");
        //                        }
        //                        else
        //                        { strBuildXMLOutput1.Append("<td align=" + ALIGN + "  valign=middle>"); }
        //                        //DBConnection connn = new DBConnection();
        //                        Helper connn = new Helper();
        //                        string ValueFortag = String.Empty;
        //                        string GrpColVal = obj.DisplayLayout.Rows[i].Cells[GrpColno[j]].Value.ToString();
        //                        connn.SQLString = "SELECT ATTRIBUTE_ID FROM TB_ATTRIBUTE WHERE ATTRIBUTE_NAME = N'" + obj.DisplayLayout.Bands[0].Columns[GrpColno[j]].ToString() + "'";
        //                        //DSS = connn.CreateDataSet();
        //                        DSS = connn.GetDataSet();
        //                        ExtractCurrenyFormat(Convert.ToInt32(DSS.Tables[0].Rows[0].ItemArray[0].ToString()));
        //                        if ((Headeroptions == "All") || (Headeroptions != "All" && i == 0))
        //                        {
        //                            if ((EmptyCondition == "Null" || EmptyCondition == "Empty" || EmptyCondition == null) && (GrpColVal == string.Empty))
        //                            {
        //                                ValueFortag = ReplaceText;
        //                            }
        //                            else if ((GrpColVal.ToString()) == (EmptyCondition))
        //                            {
        //                                ValueFortag = ReplaceText;
        //                            }
        //                            else
        //                            {
        //                                if (Isnumber(GrpColVal.ToString()) == true)
        //                                {
        //                                    ValueFortag = Prefix + " " + Convert.ToDouble(GrpColVal.ToString().Replace("\r", "<br/>").Replace("\n", "&nbsp;")).ToString() + " " + Suffix;
        //                                }
        //                                else
        //                                {
        //                                    ValueFortag = Prefix + " " + GrpColVal.ToString().Replace("\r", "<br/>").Replace("\n", "&nbsp;") + " " + Suffix;
        //                                }
        //                            }
        //                        }
        //                        else
        //                        {
        //                            if (Isnumber(GrpColVal.ToString().Replace("\r", "<br/>").Replace("\n", "&nbsp;")) == true)
        //                            {
        //                                ValueFortag = Convert.ToDouble(GrpColVal.ToString().Replace("\r", "<br/>").Replace("\n", "&nbsp;")).ToString();
        //                            }
        //                            else
        //                            {
        //                                ValueFortag = GrpColVal.ToString().Replace("\r", "<br/>").Replace("\n", "&nbsp;");
        //                            }
        //                        }
        //                        if (obj.DisplayLayout.Rows[i].Cells[GrpColno[j]].Value.ToString() == "")
        //                        {
        //                            strBuildXMLOutput1.Append(ValueFortag);
        //                            //                            grdAdvanceProductTable   
        //                        }
        //                        else
        //                        {
        //                            if (chkAttrType[j] == 3)
        //                                strBuildXMLOutput1.Append(getCellStrin(obj.DisplayLayout.Rows[i].Cells[GrpColno[j]].Value.ToString()));
        //                            else
        //                                strBuildXMLOutput1.Append((ValueFortag.ToString()));
        //                        }
        //                        strBuildXMLOutput1.Append("</td>");
        //                    }
        //                }
        //                cellvalues = cellvalues + "/nn/";
        //                strBuildXMLOutput1.Append("</tr>");
        //            }
        //            //Without grouping// Merging && Non // End /////////
        //        }
        //        else
        //        {
        //            //////////////Grouping//////////////Start//////////////////////
        //            for (int i1 = 0; i1 < obj.Rows.Count; i1++)
        //            {
        //                rowVal = i1;
        //                strBuildXMLOutput1.Append("<tr style=\"background-color:white\">");
        //                string GrpColName, GrpColVal;
        //                int diff = GrpColno.Length - grpno;

        //                {
        //                    GrpColName = obj.Rows[i1].Description.ToString();
        //                    GrpColVal = CharConverter(GrpColName);
        //                    GrpColItem = ItemConverter(GrpColName);
        //                    totalrows++;
        //                    //DBConnection connn = new DBConnection();
        //                    Helper connn = new Helper();
        //                    string ValueFortag = String.Empty;
        //                    connn.SQLString = "SELECT ATTRIBUTE_ID FROM TB_ATTRIBUTE WHERE ATTRIBUTE_NAME = N'" + obj.Rows[i1].Description.ToString().Substring(0, obj.Rows[i1].Description.ToString().LastIndexOf(" :")) + "'";
        //                    //DataSet DSS = connn.CreateDataSet();
        //                    DataSet DSS = connn.GetDataSet();
        //                    ExtractCurrenyFormat(Convert.ToInt32(DSS.Tables[0].Rows[0].ItemArray[0].ToString()));

        //                    if ((EmptyCondition == "Null" || EmptyCondition == "Empty" || EmptyCondition == null) && (GrpColVal == string.Empty))
        //                    {
        //                        ValueFortag = ReplaceText;
        //                    }
        //                    else if ((GrpColVal.ToString()) == (EmptyCondition))
        //                    {
        //                        ValueFortag = ReplaceText;
        //                    }
        //                    else
        //                    {
        //                        if (Isnumber(GrpColVal.ToString().Replace("\r", "<br/>").Replace("\n", "&nbsp;")) == true)
        //                        {
        //                            ValueFortag = Prefix + " " + Convert.ToDouble(GrpColVal.ToString().Replace("\r", "<br/>").Replace("\n", "&nbsp;")).ToString() + " " + Suffix;
        //                        }
        //                        else
        //                        {
        //                            ValueFortag = Prefix + " " + GrpColVal.ToString().Replace("\r", "<br/>").Replace("\n", "&nbsp;") + " " + Suffix;
        //                        }
        //                    }
        //                    strBuildXMLOutput1.Append("<td colspan= " + grpno.ToString() + "  BGCOLOR=#99DDFF >");
        //                    if (GrpColVal == "")
        //                    {
        //                        strBuildXMLOutput1.Append(ValueFortag);
        //                    }
        //                    else
        //                    {
        //                        strBuildXMLOutput1.Append(ValueFortag.Replace("\r", "<br/>").Replace("\n", "&nbsp;"));
        //                    }
        //                    strBuildXMLOutput1.Append("</td></tr>");
        //                    rowlevel[0] = i1; levelpros = 1;
        //                    {

        //                        constructXML(Convert.ToInt16(GrpColItem), i1, levelpros);

        //                    }

        //                }
        //                strBuildXMLOutput1.Append("</tr>");
        //            }
        //            ///////////////Grouping//////////////End//////////////////////
        //        }

        //        strBuildXMLOutput.Append(strBuildXMLOutput1);
        //        strBuildXMLOutput.Append("</TABLE></center></BODY></HTML>");
        //        AllinGroup = 0;
        //    }

        //    return strBuildXMLOutput.ToString();

        //}

        //#region "HTML Construction"

        //[WebMethod]
        //public void constructXML(int ItraItem, int ProsVal, int levelofpros)
        //{
        //    //DBConnection Ocon = new DBConnection();
        //    string GrpColName, GrpColVal;
        //    for (int grpcount = 0; grpcount < ItraItem; grpcount++)
        //    {
        //        rowVal = grpcount;
        //        strBuildXMLOutput1.Append("<tr style=\"background-color:white\">");
        //        rowlevel[levelpros] = grpcount;
        //        GrpColName = XMLforGroup(levelofpros);
        //        if (GrpColName != "")
        //        {
        //            GrpColVal = CharConverter(GrpColName);
        //            GrpColItem = ItemConverter(GrpColName);
        //            totalrows++;
        //            //DBConnection connn = new DBConnection();
        //            string ValueFortag = String.Empty;
        //            oHelper.SQLString = "SELECT ATTRIBUTE_ID FROM TB_ATTRIBUTE WHERE ATTRIBUTE_NAME = N'" + GrpColName.ToString().Substring(0, GrpColName.ToString().LastIndexOf(" :")) + "'";
        //            DataSet DSS = oHelper.GetDataSet();
        //            ExtractCurrenyFormat(Convert.ToInt32(DSS.Tables[0].Rows[0].ItemArray[0].ToString()));

        //            if ((EmptyCondition == "Null" || EmptyCondition == "Empty" || EmptyCondition == null) && (GrpColVal == string.Empty))
        //            {
        //                ValueFortag = ReplaceText;
        //            }
        //            else if ((GrpColVal.ToString()) == (EmptyCondition))
        //            {
        //                ValueFortag = ReplaceText;
        //            }
        //            else
        //            {
        //                ValueFortag = Prefix + " " + GrpColVal.ToString().Replace("\r", "<br/>").Replace("\n", "&nbsp;") + " " + Suffix;
        //            }

        //            {
        //                string colorCode = "";
        //                if (levelofpros == 1)
        //                    colorCode = "LightBlue";
        //                if (levelofpros == 2)
        //                    colorCode = "LightYellow";
        //                if (levelofpros == 3)
        //                    colorCode = "Pink";
        //                if (levelofpros == 4)
        //                    colorCode = "LightGreen";
        //                strBuildXMLOutput1.Append("<td BGCOLOR=" + colorCode + " colspan=" + grpno.ToString() + " >");
        //            }

        //            if (GrpColVal == "")
        //            {
        //                strBuildXMLOutput1.Append(ValueFortag);
        //            }
        //            else
        //            {
        //                strBuildXMLOutput1.Append(ValueFortag);
        //            }
        //            strBuildXMLOutput1.Append("</td></tr><tr style=\"background-color:pink\">"); levelpros++;
        //        }
        //        {
        //            if (GrpColName != "")
        //            {
        //                constructXML(Convert.ToInt16(GrpColItem), grpcount, levelpros);
        //            }
        //            else
        //            {
        //                int conrep = XMLforGroupChildcount(levelofpros);
        //                if (AllinGroup == 0)
        //                {
        //                    totalrows++;
        //                    {
        //                        rowlevel[levelpros] = grpcount;
        //                        constructXMLforCell(grpno, ProsVal, levelpros);
        //                    }
        //                }
        //                rowlevel[levelpros] = 0;
        //            }
        //        }
        //        strBuildXMLOutput1.Append("</tr>");
        //    }

        //    levelpros--;

        //    //MessageBox.Show(ex.ToString());

        //}

        //[WebMethod]
        //public void constructXMLforCell(int ItraItem, int ProsVal, int levelofpros)
        //{
        //    //  DBConnection Ocon = new DBConnection();
        //    string GrpColName, Megval;

        //    for (int celldata = 0; celldata < ItraItem; celldata++)
        //    {
        //        bool check;
        //        string alignVal = "left";
        //        GrpColName = XMLforCell(levelofpros, GrpColno[celldata]);
        //        //DBConnection connn = new DBConnection();
        //        string ValueFortag = string.Empty;
        //        oHelper.SQLString = "SELECT ATTRIBUTE_ID FROM TB_ATTRIBUTE WHERE ATTRIBUTE_NAME = N'" + obj.DisplayLayout.Bands[0].Columns[GrpColno[celldata]].ToString() + "'";
        //        DataSet DSS = oHelper.GetDataSet();
        //        ExtractCurrenyFormat(Convert.ToInt32(DSS.Tables[0].Rows[0].ItemArray[0].ToString()));

        //        if ((Headeroptions == "All") || (Headeroptions != "All" && rowVal == 0))
        //        {
        //            if ((EmptyCondition == "Null" || EmptyCondition == "Empty" || EmptyCondition == null) && (GrpColName == string.Empty))
        //            {
        //                ValueFortag = ReplaceText;
        //            }
        //            else if ((GrpColName.ToString()) == (EmptyCondition))
        //            {
        //                ValueFortag = ReplaceText;
        //            }
        //            else
        //            {
        //                if (Isnumber(GrpColName.ToString().Replace("\r", "<br/>").Replace("\n", "&nbsp;")) == true)
        //                {
        //                    ValueFortag = Prefix + " " + Convert.ToDouble(GrpColName.ToString().Replace("\r", "<br/>").Replace("\n", "&nbsp;")).ToString() + " " + Suffix;
        //                }
        //                else
        //                {
        //                    ValueFortag = Prefix + " " + GrpColName.ToString().Replace("\r", "<br/>").Replace("\n", "&nbsp;") + " " + Suffix;
        //                }
        //            }
        //        }
        //        else
        //        {
        //            if (Isnumber(GrpColName.ToString().Replace("\r", "<br/>").Replace("\n", "&nbsp;")) == true)
        //            {
        //                ValueFortag = Convert.ToDouble(GrpColName.ToString().Replace("\r", "<br/>").Replace("\n", "&nbsp;")).ToString();
        //            }
        //            else
        //            {
        //                ValueFortag = GrpColName.ToString().Replace("\r", "<br/>").Replace("\n", "&nbsp;");
        //            }
        //        }

        //        GrpColName = ValueFortag;
        //        oHelper.SQLString = "SELECT ATTRIBUTE_DATATYPE FROM TB_ATTRIBUTE WHERE ATTRIBUTE_NAME = N'" + obj.DisplayLayout.Bands[0].Columns[GrpColno[celldata]].ToString() + "'";
        //        DataSet DSSS = oHelper.GetDataSet();
        //        if (chkAttrType[GrpColno[celldata]] == 4 || DSSS.Tables[0].Rows[0].ItemArray[0].ToString().Substring(0, 3).ToUpper() == "NUM")
        //            alignVal = "right";
        //        Mergecellcount = XMLforMergedCell(levelofpros, GrpColno[celldata]);
        //        if (Mergecellcount > 1)
        //        {
        //            Megval = XMLforMergeCheck(levelofpros, GrpColno[celldata]);
        //            if (Megval == "True")
        //            { check = false; }
        //            else { check = true; }
        //        }
        //        else
        //        {
        //            check = true;
        //        }
        //        if (check == true)
        //        {
        //            if (Mergecellcount > 1)
        //            {
        //                strBuildXMLOutput1.Append("<td align=" + alignVal + " rowspan=" + Mergecellcount.ToString() + ">");
        //            }
        //            else { strBuildXMLOutput1.Append("<td align=" + alignVal + ">"); }
        //            if (GrpColName == "")
        //            {
        //                strBuildXMLOutput1.Append("&nbsp");
        //            }
        //            else
        //            {
        //                if (chkAttrType[GrpColno[celldata]] == 3)
        //                    strBuildXMLOutput1.Append(getCellStrin(GrpColName));
        //                else
        //                {
        //                    strBuildXMLOutput1.Append(GrpColName);
        //                }
        //                //                        strBuildXMLOutput1.Append(GrpColName);
        //            }
        //            strBuildXMLOutput1.Append("</td>");
        //        }
        //    }


        //}

        //[WebMethod]
        //public string XMLforGroup(int levelofpros)
        //{
        //    // DBConnection Ocon = new DBConnection();
        //    XMLdes = "";
        //    if (levelofpros == 0)
        //    {
        //        XMLdes = obj.Rows[rowlevel[0]].Description.ToString();
        //    }
        //    else if (levelofpros == 1)
        //    {
        //        if (obj.Rows[rowlevel[0]].ChildBands[0].Rows[rowlevel[1]].HasChild())
        //        {
        //            XMLdes = obj.Rows[rowlevel[0]].ChildBands[0].Rows[rowlevel[1]].Description.ToString();
        //        }
        //        else { XMLdes = ""; }
        //    }
        //    else if (levelofpros == 2)
        //    {
        //        if (obj.Rows[rowlevel[0]].ChildBands[0].Rows[rowlevel[1]].ChildBands[0].Rows[rowlevel[2]].HasChild())
        //        {
        //            XMLdes = obj.Rows[rowlevel[0]].ChildBands[0].Rows[rowlevel[1]].ChildBands[0].Rows[rowlevel[2]].Description.ToString();
        //        }
        //        else { XMLdes = ""; }
        //    }
        //    else if (levelofpros == 3)
        //    {
        //        if (obj.Rows[rowlevel[0]].ChildBands[0].Rows[rowlevel[1]].ChildBands[0].Rows[rowlevel[2]].ChildBands[0].Rows[rowlevel[3]].HasChild())
        //        {
        //            XMLdes = obj.Rows[rowlevel[0]].ChildBands[0].Rows[rowlevel[1]].ChildBands[0].Rows[rowlevel[2]].ChildBands[0].Rows[rowlevel[3]].Description.ToString();
        //        }
        //        else { XMLdes = ""; }
        //    }

        //    return XMLdes;
        //}

        //[WebMethod]
        //public int XMLforGroupChildcount(int levelofpros)
        //{
        //    //DBConnection Ocon = new DBConnection();
        //    if (levelofpros == 0)
        //    {
        //        XMLdes = obj.Rows.Count.ToString();
        //    }
        //    else if (levelofpros == 1)
        //    {
        //        XMLdes = obj.Rows[rowlevel[0]].ChildBands[0].Rows.Count.ToString();
        //    }
        //    else if (levelofpros == 2)
        //    {

        //        XMLdes = obj.Rows[rowlevel[0]].ChildBands[0].Rows[rowlevel[1]].ChildBands[0].Rows.Count.ToString();

        //    }
        //    else if (levelofpros == 3)
        //    {
        //        XMLdes = obj.Rows[rowlevel[0]].ChildBands[0].Rows[rowlevel[1]].ChildBands[0].Rows[rowlevel[2]].ChildBands[0].Rows.Count.ToString();
        //    }
        //    else if (levelofpros == 4)
        //    {
        //        XMLdes = obj.Rows[rowlevel[0]].ChildBands[0].Rows[rowlevel[1]].ChildBands[0].Rows[rowlevel[2]].ChildBands[0].Rows[rowlevel[3]].ChildBands[0].Rows.Count.ToString();
        //    }

        //    return Convert.ToInt16(XMLdes);
        //}

        //[WebMethod]
        //public string XMLforCell(int levelofpros, int cellval)
        //{
        //    //DBConnection Ocon = new DBConnection();
        //    XMLcell = "";
        //    if (levelofpros == 1)
        //    {
        //        XMLcell = obj.Rows[rowlevel[0]].ChildBands[0].Rows[rowlevel[1]].Cells[cellval].Value.ToString();
        //    }
        //    else if (levelofpros == 2)
        //    {
        //        XMLcell = obj.Rows[rowlevel[0]].ChildBands[0].Rows[rowlevel[1]].ChildBands[0].Rows[rowlevel[2]].Cells[cellval].Value.ToString();
        //    }
        //    else if (levelofpros == 3)
        //    {
        //        XMLcell = obj.Rows[rowlevel[0]].ChildBands[0].Rows[rowlevel[1]].ChildBands[0].Rows[rowlevel[2]].ChildBands[0].Rows[rowlevel[3]].Cells[cellval].Value.ToString();
        //    }
        //    else if (levelofpros == 4)
        //    {
        //        XMLcell = obj.Rows[rowlevel[0]].ChildBands[0].Rows[rowlevel[1]].ChildBands[0].Rows[rowlevel[2]].ChildBands[0].Rows[rowlevel[3]].ChildBands[0].Rows[rowlevel[4]].Cells[cellval].Value.ToString();
        //    }

        //    return XMLcell.Replace("\r", "<br/>").Replace("\n", "&nbsp;");
        //}

        //[WebMethod]
        //public int XMLforMergedCell(int levelofpros, int cellval)
        //{
        //    //DBConnection Ocon = new DBConnection();
        //    XMLcell = "";
        //    if (levelofpros == 1)
        //    {
        //        if (obj.Rows[rowlevel[0]].ChildBands[0].Rows[rowlevel[1]].Cells[cellval].GetMergedCells() != null)
        //        {
        //            XMLcell = obj.Rows[rowlevel[0]].ChildBands[0].Rows[rowlevel[1]].Cells[cellval].GetMergedCells().Length.ToString();
        //        }
        //    }
        //    else if (levelofpros == 2)
        //    {
        //        if (obj.Rows[rowlevel[0]].ChildBands[0].Rows[rowlevel[1]].ChildBands[0].Rows[rowlevel[2]].Cells[cellval].GetMergedCells() != null)
        //        {
        //            XMLcell = obj.Rows[rowlevel[0]].ChildBands[0].Rows[rowlevel[1]].ChildBands[0].Rows[rowlevel[2]].Cells[cellval].GetMergedCells().Length.ToString();
        //        }
        //    }
        //    else if (levelofpros == 3)
        //    {
        //        if (obj.Rows[rowlevel[0]].ChildBands[0].Rows[rowlevel[1]].ChildBands[0].Rows[rowlevel[2]].ChildBands[0].Rows[rowlevel[3]].Cells[cellval].GetMergedCells() != null)
        //        {
        //            XMLcell = obj.Rows[rowlevel[0]].ChildBands[0].Rows[rowlevel[1]].ChildBands[0].Rows[rowlevel[2]].ChildBands[0].Rows[rowlevel[3]].Cells[cellval].GetMergedCells().Length.ToString();
        //        }
        //    }
        //    else if (levelofpros == 4)
        //    {
        //        if (obj.Rows[rowlevel[0]].ChildBands[0].Rows[rowlevel[1]].ChildBands[0].Rows[rowlevel[2]].ChildBands[0].Rows[rowlevel[3]].ChildBands[0].Rows[rowlevel[4]].Cells[cellval].GetMergedCells() != null)
        //        {
        //            XMLcell = obj.Rows[rowlevel[0]].ChildBands[0].Rows[rowlevel[1]].ChildBands[0].Rows[rowlevel[2]].ChildBands[0].Rows[rowlevel[3]].ChildBands[0].Rows[rowlevel[4]].Cells[cellval].GetMergedCells().Length.ToString();
        //        }
        //    }
        //    if (XMLcell == "")
        //    {
        //        XMLcell = "1";
        //    }

        //    return Convert.ToInt16(XMLcell);
        //}

        //[WebMethod]
        //public string XMLforMergeCheck(int levelofpros, int cellval)
        //{
        //    string Megcell = "First";
        //    //  DBConnection Ocon = new DBConnection();

        //    if (levelofpros == 1)
        //    {
        //        if (rowlevel[1] > 0)
        //        {
        //            Megcell = obj.Rows[rowlevel[0]].ChildBands[0].Rows[rowlevel[1]].Cells[cellval].IsMergedWith(obj.Rows[rowlevel[0]].ChildBands[0].Rows[rowlevel[1] - 1].Cells[cellval]).ToString();
        //        }
        //    }
        //    else if (levelofpros == 2)
        //    {
        //        if (rowlevel[2] > 0)
        //        {
        //            Megcell = obj.Rows[rowlevel[0]].ChildBands[0].Rows[rowlevel[1]].ChildBands[0].Rows[rowlevel[2]].Cells[cellval].IsMergedWith(obj.Rows[rowlevel[0]].ChildBands[0].Rows[rowlevel[1]].ChildBands[0].Rows[rowlevel[2] - 1].Cells[cellval]).ToString();
        //        }
        //    }
        //    else if (levelofpros == 3)
        //    {
        //        if (rowlevel[3] > 0)
        //        {
        //            Megcell = obj.Rows[rowlevel[0]].ChildBands[0].Rows[rowlevel[1]].ChildBands[0].Rows[rowlevel[2]].ChildBands[0].Rows[rowlevel[3]].Cells[cellval].IsMergedWith(obj.Rows[rowlevel[0]].ChildBands[0].Rows[rowlevel[1]].ChildBands[0].Rows[rowlevel[2]].ChildBands[0].Rows[rowlevel[3] - 1].Cells[cellval]).ToString();
        //        }
        //    }
        //    else if (levelofpros == 4)
        //    {
        //        if (rowlevel[4] > 0)
        //        {
        //            Megcell = obj.Rows[rowlevel[0]].ChildBands[0].Rows[rowlevel[1]].ChildBands[0].Rows[rowlevel[2]].ChildBands[0].Rows[rowlevel[3]].ChildBands[0].Rows[rowlevel[4]].Cells[cellval].IsMergedWith(obj.Rows[rowlevel[0]].ChildBands[0].Rows[rowlevel[1]].ChildBands[0].Rows[rowlevel[2]].ChildBands[0].Rows[rowlevel[3]].ChildBands[0].Rows[rowlevel[4] - 1].Cells[cellval]).ToString();
        //        }
        //    }


        //    return Megcell;
        //}
        //#endregion
        #endregion
        #endregion

        #region " HTML Utilities"
        /// <summary>
        /// This is used to convert Character 
        /// </summary>
        /// <param name="GrpColName">string</param>
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
        ///      ...
        ///      GrpColName = obj.Rows[i1].Description.ToString();
        ///      GrpColVal = CharConverter(GrpColName);
        /// }    
        [WebMethod]
        public string CharConverter(string GrpColName)
        {
            string TempColname; TempColname = GrpColName;
            string Temp = "";

            for (int i = 0; i < TempColname.Length - 2; i++)
            {
                if (TempColname.Substring(i, 2) == ": ")
                {
                    i = i + 2;
                    for (int j = TempColname.Length - 3; j > i; j--)
                    {
                        if (TempColname.Substring(j, 2) == " (")
                        {
                            Temp = TempColname.Substring(i, j - i); j = i;
                        }
                    }
                }
            }

            return Temp;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="GrpColName">0</param>
        /// <returns></returns>
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
        ///      ...
        ///       ...
        ///      GrpColName = obj.Rows[i1].Description.ToString();
        ///      GrpColVal = ItemConverter(GrpColName);
        /// }    
        [WebMethod]
        public string ItemConverter(string GrpColName)
        {
            string TempColname; TempColname = GrpColName;
            string Temp = "";

            for (int i = TempColname.Length - 2; i > 0; i--)
            {
                if (TempColname.Substring(i, 2) == " (")
                {
                    i = i + 2;
                    for (int j = i; j < TempColname.Length - 5; j++)
                    {
                        if (TempColname.Substring(j, 5) == " item")
                        {
                            Temp = TempColname.Substring(i, j - i); j = i + j; i = 0;
                        }
                    }
                }
            }

            return Temp;
        }
        #endregion
        /// <summary>
        /// This is used to Retrieve the Currency Format
        /// </summary>
        /// <param name="AttributeID">int</param>
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
        ///      ...
        ///      string alignVal = "LEFT";
        ///      AttrID = DsPreview.Tables[1].Rows[0].ItemArray[5 + j].ToString();
        ///      ExtractCurrenyFormat(Convert.ToInt32(AttrID));
        ///      ...
        /// }
        /// </code>
        /// </example>
        [WebMethod]
        private void ExtractCurrenyFormat(int AttributeID)
        {
            //AppLoader.DBConnection Oocn = new DBConnection();
            string XMLstr = string.Empty;
            DataSet dscURRENCY = new DataSet();
            oHelper.SQLString = " SELECT ATTRIBUTE_DATARULE FROM TB_ATTRIBUTE WHERE ATTRIBUTE_ID  =" + AttributeID + " ";
            dscURRENCY = oHelper.GetDataSet();
            Prefix = string.Empty; Suffix = string.Empty; EmptyCondition = string.Empty; ReplaceText = string.Empty; Headeroptions = string.Empty;
            if (dscURRENCY.Tables[0].Rows.Count > 0)
            {
                if (dscURRENCY.Tables[0].Rows[0].ItemArray[0].ToString() != string.Empty)
                {
                    XMLstr = dscURRENCY.Tables[0].Rows[0].ItemArray[0].ToString();
                    XmlDocument xmlDOc = new XmlDocument();
                    xmlDOc.LoadXml(XMLstr);
                    XmlNode rootNode = xmlDOc.DocumentElement;
                    {
                        XmlNodeList xmlNodeList;
                        xmlNodeList = rootNode.ChildNodes;

                        for (int xmlNode = 0; xmlNode < xmlNodeList.Count; xmlNode++)
                        {
                            if (xmlNodeList[xmlNode].ChildNodes.Count > 0)
                            {
                                if (xmlNodeList[xmlNode].ChildNodes[0].LastChild != null)
                                {
                                    Prefix = xmlNodeList[xmlNode].ChildNodes[0].LastChild.Value;
                                }
                                if (xmlNodeList[xmlNode].ChildNodes[1].LastChild != null)
                                {
                                    Suffix = xmlNodeList[xmlNode].ChildNodes[1].LastChild.Value;
                                }
                                if (xmlNodeList[xmlNode].ChildNodes[2].LastChild != null)
                                {
                                    EmptyCondition = xmlNodeList[xmlNode].ChildNodes[2].LastChild.Value;
                                }
                                if (xmlNodeList[xmlNode].ChildNodes[3].LastChild != null)
                                {
                                    ReplaceText = xmlNodeList[xmlNode].ChildNodes[3].LastChild.Value;
                                }
                                if (xmlNodeList[xmlNode].ChildNodes[4].LastChild != null)
                                {
                                    Headeroptions = xmlNodeList[xmlNode].ChildNodes[4].LastChild.Value;
                                }

                            }
                        }
                    }



                }
            }
        }

        [WebMethod]
        private void InitializeComponent()
        {
            //this.SuspendLayout();
            //// 
            //// ProductPreview
            //// 
            //this.ClientSize = new System.Drawing.Size(292, 266);
            //this.Name = "ProductPreview";
            //this.ResumeLayout(false);

        }
        #endregion

        #region "Product Validation Services Methods"

        #region "Textvalidator"
        /// <summary>
        /// This is used to Resolve the Text Size
        /// </summary>
        /// <returns>int</returns>
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
        ///    int TextLength;
        ///    ...
        ///    TextLength = ResolveTextSize();
        /// }
        /// </code>
        /// </example>
        [WebMethod]
        public int ResolveTextSize()
        {
            int Irc = 0; TextLength = 0; //bool Retval = false;
            DataSet DsAttribute = new DataSet();
            DataSet Dspicklist = new DataSet();
            string DefaultDataType = string.Empty;
            //ErrorLog.ExceptionHandle Oex = new ErrorLog.ExceptionHandle();

            oHelper.SQLString = "  SELECT ATTRIBUTE_DATATYPE FROM TB_ATTRIBUTE WHERE ATTRIBUTE_ID =  " + AttributeID + " ";
            DsAttribute = oHelper.GetDataSet();
            if (DsAttribute.Tables[0].Rows.Count > 0)
            {
                if (DsAttribute.Tables[0].Rows[0].ItemArray[0].ToString() != string.Empty)
                {
                    DefaultDataType = DsAttribute.Tables[0].Rows[0].ItemArray[0].ToString();

                    bool NumberType = DefaultDataType.StartsWith("Number");
                    bool HyperlinkType = DefaultDataType.StartsWith("Hyperlink");
                    bool MultipleLineType = DefaultDataType.StartsWith("Text");
                    bool DateTimeType = DefaultDataType.StartsWith("Date");

                    if (NumberType == true)
                    {
                        GetBeforeDecimallength(DefaultDataType);
                        GetAfterDecimalLength(DefaultDataType);
                        bool BoolFormat = IsValidDataFormat();
                        int valuelength = 0;

                        oHelper.SQLString = "  SELECT USE_PICKLIST FROM TB_ATTRIBUTE WHERE ATTRIBUTE_ID =  " + AttributeID + " ";
                        DsAttribute = oHelper.GetDataSet();
                        if (Convert.ToBoolean(DsAttribute.Tables[0].Rows[0].ItemArray[0].ToString()))
                        {
                            if (_ObjValue.Contains("."))
                            {
                                valuelength = (_ObjValue.Length - 1);
                            }
                            else
                            {
                                valuelength = _ObjValue.Length;
                            }
                            if (valuelength > (BeforeDecimalVal + AfterDecimalVal))
                                BoolFormat = false;
                        }
                        //  bool Validate = ValidateNumber();
                        if (BoolFormat == true) //&& Validate == true)
                        {
                            Irc = 1;
                        }
                        else
                        {
                            Irc = 0;

                        }

                    }
                    else if (MultipleLineType == true)
                    {
                        ExtractTextlength(DefaultDataType);
                        int iMultipleText = ValidateTextEditor();
                        bool bDataForamt = IsValidDataFormat();
                        if (iMultipleText == 1 && bDataForamt == true)
                        {
                            Irc = 1;
                        }
                        else
                        {
                            Irc = 0;

                        }


                    }
                    else if (HyperlinkType == true)
                    {
                        bool bHyperLink = IsValidDataFormat();
                        if (bHyperLink == true)
                        {
                            Irc = 1;
                        }
                        else
                        {
                            Irc = 0;
                        }

                    }
                    else if (DateTimeType == true)
                    {
                        bool bDateFormat = IsValidDataFormat();
                        if (bDateFormat == true)
                        {
                            Irc = 1;
                        }
                        else
                        {
                            //MessageBox.Show("Invalid Date Format");
                            Irc = 0;
                        }
                    }
                    else if (DefaultDataType == "STRING")
                    {

                        ExtractTextlength("");
                        int iLength = ValidateTextEditor();
                        if (iLength > 0)
                        {
                            Irc = 1;
                        }
                        else
                        {
                            //  MessageBox.Show("Text Length Exceeds the Maximum limit");
                            Irc = 0;
                        }
                    }
                }
            }
            //Ocon._DBCon.Dispose();
            //Ocon = null;
            return Irc;
        }

        /// <summary>
        /// This is used to Validate the PickList
        /// </summary>
        /// <param name="Regx">string</param>
        /// <param name="StrValue">string</param>
        /// <returns>bool</returns>
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
        ///    
        /// }
        /// </code>
        /// </example>
        [WebMethod]
        public bool ValidatePickList(string[] Regx, string StrValue)
        {
            bool Retval = false;

            for (int RegxCount = 0; RegxCount < Regx.Length; RegxCount++)
            {
                Regex re = new Regex(Regx[RegxCount]);
                if (Regx[RegxCount] != string.Empty)
                {
                    if (Regx.Length == 4)
                    {
                        if (StrValue.Length == 10 && (RegxCount == 0 || RegxCount == 1))
                        {
                            if (re.IsMatch(StrValue))
                                Retval = true;
                        }
                        else if (StrValue.Length == 19 && RegxCount == 2)
                        {
                            if (re.IsMatch(StrValue))
                                Retval = true;
                        }
                        else if (StrValue.Length == 22 && RegxCount == 3)
                        {
                            if (re.IsMatch(StrValue))
                                Retval = true;
                        }
                    }
                    else
                    {
                        if (re.IsMatch(StrValue))
                            Retval = true;
                    }
                }
            }

            return Retval;
        }

        /// <summary>
        /// This is used to Validate Text Editor
        /// </summary>
        /// <returns>int</returns>
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
        ///    ...
        ///    ExtractTextlength(DefaultDataType);
        ///    int iMultipleText = ValidateTextEditor();
        ///    bool bDataForamt = IsValidDataFormat();
        /// }
        /// </code>
        /// </example>
        [WebMethod]
        public int ValidateTextEditor()
        {
            int Retval;
            if (TextLength != -1)
            {
                if (ObjValue.Length > TextLength)
                {
                    Retval = 0;
                }
                else
                {
                    Retval = 1;
                }
            }
            else
            {
                Retval = 1;
            }
            return Retval;
        }

        #endregion

        #region "Methods"

        /// <summary>
        /// This is used to check Validate Data Format
        /// </summary>
        /// <returns>bool</returns>
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
        ///    ...
        ///    bool BoolFormat = IsValidDataFormat();
        ///    int valuelength = 0;
        /// }
        /// </code>
        /// </example>
        [WebMethod]
        public bool IsValidDataFormat()
        {
            string StrRegx = string.Empty;
            //DBConnection Ocon = new DBConnection();
            DataSet DSDataFormat = new DataSet();
            oHelper.SQLString = " SELECT ATTRIBUTE_DATAFORMAT FROM TB_ATTRIBUTE WHERE ATTRIBUTE_ID = " + _AttributeID + "";
            DSDataFormat = oHelper.GetDataSet();
            if (DSDataFormat.Tables[0].Rows[0].ItemArray[0].ToString() != string.Empty)
            {
                StrRegx = DSDataFormat.Tables[0].Rows[0].ItemArray[0].ToString();
            }
            bool Retval = false;
            Regex re = new Regex(StrRegx);
            if (StrRegx != string.Empty)
            {
                if (re.IsMatch(_ObjValue) || _ObjValue == "" || StrRegx == "System default settings")
                    return (Retval = true);
                else
                    return (Retval = false);

            }
            else
            {
                Retval = true;
            }
            return Retval;
        }

        /// <summary>
        /// This is used to change the Numeric Format
        /// </summary>
        /// <param name="NumberPrecision">string</param>
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
        ///  if (NumberType == true)
        ///  {
        ///             GetBeforeDecimallength(DefaultDataType);
        ///             GetAfterDecimalLength(DefaultDataType);
        ///   ...
        ///  }  
        /// }
        /// </code>
        /// </example>
        [WebMethod]
        private void GetBeforeDecimallength(string NumberPrecision)
        {
            string BeforeDecimal = string.Empty;

            for (int i = 0; i < NumberPrecision.Length; i++)
            {
                if (NumberPrecision[i] == '(')
                {
                    int j = i + 1;
                    while (NumberPrecision[j] != ',')
                    {
                        BeforeDecimal = BeforeDecimal + NumberPrecision[j].ToString();
                        j++;
                    }
                }
            }
            BeforeDecimalVal = Convert.ToInt32(BeforeDecimal);

        }

        /// <summary>
        /// This is used to change the Numeric Format
        /// </summary>
        /// <param name="DecimalPrecision">string</param>
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
        ///  ...
        ///  if (NumberType == true)
        ///  {
        ///             GetBeforeDecimallength(DefaultDataType);
        ///             GetAfterDecimalLength(DefaultDataType);
        ///   ...
        ///  }
        /// }
        /// </code>
        /// </example>
        [WebMethod]
        private void GetAfterDecimalLength(string DecimalPrecision)
        {
            string AfterDecimal = string.Empty;

            for (int i = 0; i < DecimalPrecision.Length; i++)
            {
                if (DecimalPrecision[i] == ',')
                {
                    int j = i + 1;
                    while (DecimalPrecision[j] != ')')
                    {
                        AfterDecimal = AfterDecimal + DecimalPrecision[j].ToString();
                        j++;
                    }

                }
            }
            AfterDecimalVal = Convert.ToInt32(AfterDecimal);

        }

        /// <summary>
        /// This is used to validate Number
        /// </summary>
        /// <returns>bool</returns>
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
        ///  ...
        ///  if (NumberType == true)
        ///  {
        ///   ValidateNumber();          
        ///   ...
        ///  }
        /// }
        /// </code>
        /// </example>
        [WebMethod]
        private bool ValidateNumber()
        {
            string Temp = string.Empty;
            bool Retval = false;
            for (int i = 0; i < _ObjValue.Length; i++)
            {
                if (_ObjValue[i] != '.')
                {
                    Temp = Temp + _ObjValue[i].ToString();
                    if (_ObjValue.Contains("."))
                    {
                        if (_ObjValue[i + 1] == '.')
                        {
                            break;
                        }
                    }

                }


            }
            string TempVal = string.Empty;
            for (int j = 0; j < _ObjValue.Length; j++)
            {
                if (_ObjValue[j] == '.')
                {
                    int Ctr = j + 1;
                    for (int i = Ctr; i < _ObjValue.Length; i++)
                    {
                        TempVal = TempVal + _ObjValue[i].ToString();
                    }
                }
            }

            if (TempVal.Length <= 6 && Temp.Length <= BeforeDecimalVal)
            {
                Retval = true;
            }

            if (AfterDecimalVal == 0)
            {
                Retval = true;
            }
            //int iZero = 0;
            //for (int i = 0; i < _ObjValue.Length; i++)
            //{
            //    if (_ObjValue[i] != '.')
            //    {   
            //        if (_ObjValue[i] == '0')
            //        {
            //            iZero = iZero + 1;
            //        }

            //    }
            //}
            if (_ObjValue.Length > 0)
            {
                if (_ObjValue[0] == '0')
                {
                    Retval = true;
                }
            }
            //if (iZero == BeforeDecimalVal)
            //{
            //    Retval = true;
            //}
            return Retval;
        }

        /// <summary>
        /// This is used to Extract the MultiLine Text Length
        /// </summary>
        /// <param name="MultipleLineText">string</param>
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
        ///     string DefaultDataType = string.Empty;
        ///     ...
        ///     else if (MultipleLineType == true)
        ///     {
        ///        ExtractTextlength(DefaultDataType);
        ///        ...
        ///     }
        /// }
        /// </code>
        /// </example>
        [WebMethod]
        private void ExtractTextlength(string MultipleLineText)
        {
            if (MultipleLineText != string.Empty)
            {
                string TempTextlength = string.Empty;
                if (MultipleLineText.Contains("("))
                {

                    for (int itr = 0; itr < MultipleLineText.Length; itr++)
                    {
                        if (MultipleLineText[itr] == '(')
                        {
                            int Ctr = itr + 1;
                            for (int i = Ctr; i < MultipleLineText.Length; i++)
                            {
                                if (MultipleLineText[i] != ')')
                                {
                                    TempTextlength = TempTextlength + MultipleLineText[i].ToString();
                                }

                            }



                        }

                    }
                    TextLength = Convert.ToInt32(TempTextlength);
                }
                else
                {
                    TextLength = -1;
                }
            }
            else
            {
                if (_AttributeType == 1)
                {
                    if (_AttributeID == 1)
                    {
                        TextLength = 255;
                    }
                    else if (_AttributeID == 2)
                    {
                        TextLength = 255;
                    }
                    else if (_AttributeID == 4)
                    {
                        TextLength = 100;
                    }
                    else if (_AttributeID > 4)
                    {
                        TextLength = 4000;
                    }

                }
                else if (_AttributeType == 2)
                {
                    TextLength = 1073741823;//Ntext "
                }

            }


        }
        /// <summary>
        /// This is used to check Field Value
        /// </summary>
        /// <param name="FieldValue">string</param>
        /// <param name="FieldType">int</param>
        /// <returns>bool</returns>
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
        ///    ...
        ///    else
        ///    {
        ///       if (IsValidFieldvalue(Value,Type)
        ///       {
        ///          ValueFortag = Convert.ToDouble(DsPreview.Tables[0].Rows[i][j].ToString().Replace("\r", "<br/>").Replace("\n", "&nbsp;")).ToString();
        ///       }
        ///       ...
        ///    }
        /// }
        /// </code>
        /// </example>
        [WebMethod]
        public bool IsValidFieldvalue(string FieldValue, int FieldType)
        {
            string strRegex = "";
            bool Retval = false;
            FieldValue = FieldValue.ToLower();
            if (FieldValue != string.Empty)
            {
                if (FieldType == 1)
                {

                }
                else if (FieldType == 2)
                {
                    strRegex = @"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}" +
                                  @"\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\" +
                                  @".)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$";

                }
                else if (FieldType == 3)
                {
                    strRegex = "^(https?://)|^(HTTPS?://)|^(Https?://)"
                       + "?(([0-9a-zA-Z_!~*'().&=+$%-]+: )?[0-9a-zA-Z_!~*'().&=+$%-]+@)?" //user@ 
                       + @"(([0-9]{1,3}\.){3}[0-9]{1,3}" // IP- 199.194.52.184 
                       + "|" // allows either IP or domain 
                       + @"([0-9a-zA-Z_!~*'()-]+\.)*" // tertiary domain(s)- www. 
                       + @"([0-9a-zA-Z][0-9a-zA-Z-]{0,61})?[0-9a-zA-Z]\." // second level domain 
                       + "[a-zA-Z]{2,6})" // first level domain- .com or .museum 
                       + "(:[0-9]{1,4})?" // port number- :80 
                       + "((/?)|" // a slash isn't required if there is no file name 
                       + "(/[0-9a-zA-Z_!~*'().;?:@&=+$,%#-]+)+/?)$";
                }
                else if (FieldType == 4)
                {
                    strRegex = @"(^\+[A-Za-z0-9.-]+$)|(^[A-Za-z0-9.-]+$)|(^\(\d{3}\)\s?\d{3}-\d{4}$)";
                }
                else if (FieldType == 5)
                {
                    strRegex = "^[0-9]|[0-9-0-9]+$";
                }

                Regex re = new Regex(strRegex);
                if (strRegex != string.Empty)
                {
                    if (re.IsMatch(FieldValue))
                        return (Retval = true);
                    else if (FieldType == 3)
                    {
                        strRegex = "^(https?://)|^(HTTPS?://)|^(Https?://)"
                       + "?(([0-9a-zA-Z_!~*'().&=+$%-]+: )?[0-9a-zA-Z_!~*'().&=+$%-]+@)?" //user@ 
                       + @"(([0-9]{1,3}\.){3}[0-9]{1,3}" // IP- 199.194.52.184 
                       + "|" // allows either IP or domain 
                       + @"([0-9a-zA-Z_!~*'()-]+\.)*" // tertiary domain(s)- www. 
                       + @"([0-9a-zA-Z][0-9a-zA-Z-]{0,61})?[0-9a-zA-Z])" // second level domain 
                       + "(:[0-9]{1,4})?" // port number- :80 
                       + "((/?)|" // a slash isn't required if there is no file name 
                       + "(/[0-9a-zA-Z_!~*'().;?:@&=+$,%#-]+)+/?)$";
                        Regex re1 = new Regex(strRegex);
                        if (strRegex != string.Empty)
                        {
                            if (re1.IsMatch(FieldValue))
                                return (Retval = true);
                            else
                                return (Retval = false);
                        }
                    }
                    else
                    {
                        return (Retval = false);
                    }
                }
            }
            else
            {
                Retval = true;
            }
            return Retval;
        }

        /// <summary>
        /// This is used to check a number is a Regular Expression
        /// </summary>
        /// <param name="RefValue">string</param>
        /// <returns>bool</returns>
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
        ///    ...
        ///    else
        ///    {
        ///       if (Isnumber(DsPreview.Tables[0].Rows[i][j].ToString().Replace("\r", "<br/>").Replace("\n", "&nbsp;")) == true)
        ///       {
        ///          ValueFortag = Convert.ToDouble(DsPreview.Tables[0].Rows[i][j].ToString().Replace("\r", "<br/>").Replace("\n", "&nbsp;")).ToString();
        ///       }
        ///       ...
        ///    }
        /// }
        /// </code>
        /// </example>
        [WebMethod]
        public bool Isnumber(string RefValue)
        {
            //string StrRegx = "^[0-9.]";
            //string StrRegx =@"(^-?\d\d*$)"; jai
            //string StrRegx = @"^d[0-9.]+$";
            string StrRegx = @"^[0-9]*(\.)?[0-9]+$";
            bool Retval = false;
            Regex re = new Regex(StrRegx);
            if (re.IsMatch(RefValue))
            {
                Retval = true;
            }
            else
            {
                Retval = false;
            }
            return Retval;
        }

        #endregion "Methods"

        #endregion


    }

}