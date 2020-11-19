using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Security.Permissions;
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
    public partial class UI_ProductCategory : System.Web.UI.UserControl
    {

        public enum DisplayText
        {
            CategoryName = 1,
            CategoryHierarchy = 2
        }
        DisplayText _CurrentDisplayText;
        int _ProductID;
        string _SkinID;
        string _CssClass;
        #region "Property"

        [
        Browsable(true),
        Category("TradingBell"),
        DefaultValue(""),
        Description("Disply Product Category")
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
        Description("Aplly Control style")
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
        Description("Aplly Control style")
        ]
        public string CssClass
        {
            get
            {
                return _CssClass;
            }
            set
            {
                _CssClass = value;
            }
        }
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!DesignMode)
            {
                Label lblControl = new Label();
                HelperDB oHelper = new HelperDB();
                Product oProduct = new Product();
                lblControl.ID = "lblProdCategory";
                lblControl.SkinID = _SkinID;
                if (Request["Pid"] != null)
                {
                    _ProductID = oHelper.CI(Request["Pid"].ToString());
                }

                if (_CurrentDisplayText == DisplayText.CategoryName)
                    lblControl.Text = oProduct.GetCategoryName(_ProductID, false);
                else
                    lblControl.Text = oProduct.GetCategoryName(_ProductID, true);

                Controls.Add(lblControl);
            }
        }

    }
