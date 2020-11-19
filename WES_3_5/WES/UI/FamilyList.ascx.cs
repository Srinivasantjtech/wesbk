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
using System.ComponentModel.Design;
using TradingBell.Common;
using TradingBell.WebServices;
using TradingBell.WebCat.CatalogDB;
using TradingBell.WebCat.Helpers;
using TradingBell.WebCat.TemplateRender;
using TradingBell.WebCat.CommonServices;
public partial class UI_FamilyList : System.Web.UI.UserControl
{
   
    public enum DisplayText
    {
        Yes = 1,
        No = 2
    }
    public enum DisplayMode
    {
        FamilyList = 1,
        RelatedFamilyList =2
    }

    string _CategoryID;
    int _CatalogID;
    //string _BulletImgURL;
    string _SkinID;
    int _FamilyID;
    DisplayText _CurrentDisplayText;
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
        //[
        //Browsable(true),
        //Category("TradingBell"),
        //DefaultValue("")
        //]
        //public string BulletImageUrl
        //{
        //    get
        //    {
        //        return _BulletImgURL;
        //    }
        //    set
        //    {
        //        _BulletImgURL = value;
        //    }
        //}
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
        Description("Apply BulletList control skinid")
        ]
        public string SkinID
        {
            get
            {
                return _SkinID;
            }
            set
            {
                _SkinID = value;
            }
        }
    [
    Browsable(true),
    Category("TradingBell"),
    DefaultValue(""),
    Description("Display Product Count")
    ]
    public DisplayText DisplayProductCount
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


    #endregion 
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!DesignMode)
        {
            HelperDB oHelper = new HelperDB();
            ErrorHandler oErr = new ErrorHandler();
            ProductFamily oPF = new ProductFamily();
            Product Prod = new Product();
            Category oCategory = new Category();
            Page.Title = oHelper.GetOptionValues("BROWSER TITLE").ToString();
            DataSet dsFam = new DataSet();
            BulletedList lstFamily = new BulletedList();
            lstFamily.SkinID = _SkinID;
            

            if (_CurrentDisplayMode == DisplayMode.FamilyList)
            {
                if (Request["Cat"] != null)
                {
                    { _CategoryID = Request["Cat"].ToString(); }

                    if (_CategoryID != null)
                    {
                        //dsFam = oPF.GetCategoryFamilyList(_CategoryID);
                        dsFam = oPF.GetFamilylistWithProdCount(_CategoryID,_CatalogID);
                    }
                    else
                    {
                        dsFam = null;
                    }
                }
                else if (Request["SName"]!= null)
                {
                    string SName = Request["SName"].ToString();
                    if (SName != "")
                    {
                        string DefCatID = oHelper.GetOptionValues("DEFAULT CATALOG").ToString();
                        dsFam = oPF.GetFamilylistBySupplierName(SName.Replace("?","&"),DefCatID);
                    }
                    else
                    {
                        dsFam = null;
                    }
                }
                Session["PageUrl"] = "Family.aspx?Cat=" + Request["Cat"];
            }
            else
            {
                //_FamilyID = Request["Fid"];
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
              
               Session["PageUrl"] = "Family.aspx?Cat=" + Request["Cat"];
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

                   // PCount = Prod.GetProductCount(oHelper.CI(rFam["family_id"]));
                    PCount = oHelper.CI(rFam["PRODUCT_COUNT"].ToString());
                    //if (PCount >= 0 && _CurrentDisplayText == DisplayText.Yes)
                    if (PCount >= 0 && oHelper.GetOptionValues("DISPLAY PRODUCT COUNT").ToString() == "YES")
                    {
                        lstFamily.Items.Add(oHelper.CS(rFam["FAMILY_NAME"] + " (" + rFam["PRODUCT_COUNT"] + ")"));
                        if (Request["SName"] != null)
                        {
                            lstFamily.Items[i].Value = "~/Familydisplay.aspx?Fid=" + oHelper.CS(rFam["FAMILY_ID"]) + "&SName=" + Request["SName"].Replace("&", "?~");
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
                        if (Request["SName"] != null)
                        {
                            lstFamily.Items[i].Value = "~/Familydisplay.aspx?Fid=" + oHelper.CS(rFam["FAMILY_ID"]) + "&SName=" + Request["SName"].Replace("&", "?~");
                        }
                        else
                        {
                            lstFamily.Items[i].Value = "~/Familydisplay.aspx?Fid=" + oHelper.CS(rFam["FAMILY_ID"]);
                        }
                        i = i + 1;
                    }
                }
                
                //if (_BulletImgURL == "")
                //{
                //    lstFamily.BulletImageUrl = _BulletImgURL;
                //}
            }
           lstFamily.DisplayMode = BulletedListDisplayMode.HyperLink;
           Controls.Add(lstFamily);
        }
    }
    
}
