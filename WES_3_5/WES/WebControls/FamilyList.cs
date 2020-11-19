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
///This web control is used to list out all the Family for Each Category.
/// </summary>

[assembly: TagPrefix("TradingBell.WebServices.UI", "WebCat")]
[assembly: System.Reflection.AssemblyVersion("5.0")]

namespace TradingBell.WebServices.UI
{
    public class FamilyList : WebControl
    {
        public enum DisplayText
        {
            Yes = 1,
            No = 2
        }
        public enum DisplayMode
        {
            FamilyList = 1,
            RelatedFamilyList = 2,
            SupplierFamilyList = 3
        }

        string _CategoryID;
        int _CatalogID;
        int _FamilyID;
        string _SName;
        string _DisplayProductCount;
        string _ListCssStyle;
        DisplayMode _CurrentDisplayMode;
      
        #region "Property"
        [
        Browsable(true),
        Category("TradingBell"),
        DefaultValue("")
        ]
        public string CategoryID
        {
            get
            {
                return _CategoryID;
            }
            set
            {
                _CategoryID = value;
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
            Browsable(false),
            Category("TradingBell")
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
        public string SupplierName
        {
            get
            {
                return _SName;
            }
            set
            {
                _SName = value;
            }
        }
    [
        Browsable(true),
        Category("TradingBell"),
        DefaultValue(""),
        Description("Display the family list.")
        ]
        public DisplayMode Display
        {
            get
            {
                return _CurrentDisplayMode;
            }
            set
            {
                _CurrentDisplayMode = value;
            }
        }
        
        [
        Browsable(true),
        Category("TradingBell"),
        Description("Apply BulletList control Style")
        ]

        public string ListCssStyle
        {
            get
            {
                return _ListCssStyle;
            }
            set
            {
                _ListCssStyle = value;
            }
        }
        [
        Browsable(true),
        Category("TradingBell"),
        DefaultValue(""),
        Description("Display Product Count")
        ]
        public string DisplayProductCount
        {
            get
            {
                return _DisplayProductCount;
            }
            set
            {
                _DisplayProductCount = value;
            }
        }


        #endregion
        protected override void RenderContents(HtmlTextWriter output)
        {
            if (DesignMode)
            {
                Image oImg = new Image();
                oImg.ImageUrl = "~/Images/FamilyList.gif";
                oImg.ID = "imgDefault";
                oImg.Width = 125;
                this.Controls.Add(oImg);
                oImg.RenderControl(output);
            }
            else
            {
                

                Helper oHelper = new Helper();
                ErrorHandler oErr = new ErrorHandler();
                ProductFamily oPF = new ProductFamily();
                Product Prod = new Product();
                Category oCategory = new Category();
                DataSet dsFam = new DataSet();
                BulletedList lstFamily = new BulletedList();



                lstFamily.CssClass = _ListCssStyle;


                if (_CurrentDisplayMode == DisplayMode.FamilyList)
                {

                    if (_CategoryID != null && _CategoryID != "")
                    {
                        dsFam = oPF.GetFamilylistWithProdCount(_CategoryID,_CatalogID);
                    }
                    else
                    {
                        dsFam = null;
                    }
                }
                else if (_CurrentDisplayMode == DisplayMode.SupplierFamilyList)
                {
                    if (_SName != "")
                    {
                       // string DefCatID = Helper.WebCatGlb["DEFAULT CATALOG"].ToString();
                        string DefCatID = oHelper.GetOptionValues("DEFAULT CATALOG").ToString();

                        dsFam = oPF.GetFamilylistBySupplierName(_SName.Replace("?", "&"), DefCatID);
                    }
                    else
                    {
                        dsFam = null;
                    }
                }
                else
                {

                    if (_FamilyID != 0)
                    {
                        DataTable dtFam = new DataTable();
                        dtFam = oPF.GetSubFamilyList(_FamilyID);
                        dsFam.Tables.Add(dtFam);
                    }
                    else
                    {
                        dsFam = null;
                    }
                }
                if (dsFam != null)
                {
                    int i = 0;
                    int PCount = 0;
                    if (lstFamily.Items.Count > 0)
                    {
                        lstFamily.Items.Clear();
                    }
                    foreach (DataRow rFam in dsFam.Tables[0].Rows)
                    {

                        PCount = oHelper.CI(rFam["PRODUCT_COUNT"].ToString());
                        _DisplayProductCount = oHelper.GetOptionValues("DISPLAY PRODUCT COUNT").ToString();
                        if (PCount >= 0 && _DisplayProductCount.ToUpper() == "YES")//Helper.WebCatGlb["DISPLAY PRODUCT COUNT"].ToString() == "YES")
                        {
                            lstFamily.Items.Add(oHelper.CS(rFam["FAMILY_NAME"] + " (" + rFam["PRODUCT_COUNT"] + ")"));
                            if (_SName != null)
                            {
                                lstFamily.Items[i].Value = "~/Familydisplay.aspx?Fid=" + oHelper.CS(rFam["FAMILY_ID"]) + "&SName=" + _SName.Replace("&", "?~");

                            }
                            else
                            {
                                lstFamily.Items[i].Value = "~/Familydisplay.aspx?Fid=" + oHelper.CS(rFam["FAMILY_ID"]);
                            }
                            i = i + 1;
                        }
                        else
                        {
                            lstFamily.Items.Add(oHelper.CS(rFam["FAMILY_NAME"]));
                            if (_SName != null)
                            {
                                lstFamily.Items[i].Value = "~/Familydisplay.aspx?Fid=" + oHelper.CS(rFam["FAMILY_ID"]) + "&SName=" + _SName.Replace("&", "?~");
                            }
                            else
                            {
                                lstFamily.Items[i].Value = "~/Familydisplay.aspx?Fid=" + oHelper.CS(rFam["FAMILY_ID"]);
                            }
                            i = i + 1;
                        }
                    }
                }
                lstFamily.DisplayMode = BulletedListDisplayMode.HyperLink;
                lstFamily.RenderControl(output);

            }

        }


    }
}