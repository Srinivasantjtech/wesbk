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
    public partial class FamilyDetails : System.Web.UI.UserControl
    {
        public enum DisplayText
        {
            CategoryName = 1,
            FamilyID = 2,
            FamilyName = 3,
            FootNotes = 4
        }

        DisplayText _currentDisplay;
        int _FamilyID;
        string _SkinID;
        string _CategoryID;
        int _CatalogID;
        #region "Property"
        
        [
        Browsable(true),
        Category("TradingBell"),
        DefaultValue(""),
        Description("Enter the FamilyID")
        ]

        public int FamilyID
        {
            get { return _FamilyID; }
            set { _FamilyID = value; }
        }
        [
         Browsable(true),
         Category("TradingBell"),
         DefaultValue(""),
         Description("Enter the CategoryID")
                ]

        public string CategoryID
        {
            get { return _CategoryID; }
            set { _CategoryID = value; }
        }


        [
        Browsable(true),
        Category("TradingBell"),
        DefaultValue("Category Name")
        ]

        public DisplayText Display
        {
            get
            {
                return _currentDisplay;
            }
            set
            {
                _currentDisplay = value;
            }
        }
        [
       Browsable(true),
       Category("TradingBell"),
       DefaultValue("")
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
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!DesignMode)
            {

                ProductFamily oTBFamiy = new ProductFamily();
                HelperDB oHelper = new HelperDB();
                Label lblControl = new Label();
                lblControl.ID = "lblFamilyDatails";
                lblControl.SkinID = _SkinID;
                _CatalogID= oHelper.CI(oHelper.GetOptionValues("DEFAULT CATALOG").ToString());

                if (Request["Cat"] != null)
                { _CategoryID = Request["Cat"].ToString();}
                if (Request["Fid"] != null)
                { _FamilyID = oHelper.CI(Request["Fid"].ToString()); }


                if (_currentDisplay == DisplayText.FamilyName)
                { lblControl.Text = oTBFamiy.GetFamilyName(_FamilyID); }
                else if (_currentDisplay == DisplayText.CategoryName)
                {
                    if (_FamilyID != 0)
                    {
                        lblControl.Text = oTBFamiy.CategoryName(_FamilyID);
                    }
                    if (_CategoryID != null)
                    {
                        { lblControl.Text = oTBFamiy.GetParentCategory(_CategoryID); }
                    }
                
                }
                
                
                else if (_currentDisplay == DisplayText.FamilyID)
                { lblControl.Text = _FamilyID.ToString(); }
                else if (_currentDisplay == DisplayText.FootNotes)
                { lblControl.Text = oTBFamiy.FootNotes(_FamilyID); }

                Controls.Add(lblControl);
            }
        }

    }
