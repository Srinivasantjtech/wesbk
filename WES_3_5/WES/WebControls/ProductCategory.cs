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

/// <summary>
///This web control is used to display the Product Name and 
/// Category Name as a title in Product Details Page.
/// </summary>

[assembly: TagPrefix("TradingBell.WebServices.UI", "WebCat")]
[assembly: System.Reflection.AssemblyVersion("5.0")]
namespace TradingBell.WebServices.UI
{
    public class ProductCategory : WebControl
    {
        
        public enum DisplayText
        {
            CategoryName = 1,
            CategoryHierarchy = 2
        }
        DisplayText _CurrentDisplayText;
        int _ProductID;
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
        public override string CssClass
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
        protected override void RenderContents(HtmlTextWriter output)

         {

             if (DesignMode)
             {
                 Image oImg = new Image();
                 oImg.ImageUrl = "~/Images/ProductCategory.gif";
                 oImg.ID = "imgDefault";
                 oImg.Width = 125;
                 this.Controls.Add(oImg);
                 oImg.RenderControl(output);
                 
             }
             else
             {                
                 Table oTable = new Table();
                 TableCell ocell = new TableCell();
                 TableRow orow = new TableRow();

                 Label lblControl = new Label();
                 Helper oHelper = new Helper();
                 Product oProduct = new Product();

                 if (_CurrentDisplayText == DisplayText.CategoryName)
                 {


                     oTable.Controls.Add(orow);
                     this.Controls.Add(oTable);
                     oTable.RenderControl(output);
                     ocell.Text = oProduct.GetCategoryName(_ProductID, false);
                     ocell.CssClass = _CssClass;

                 }

                 else
                 {

                     orow.Controls.Add(ocell);
                     oTable.Controls.Add(orow);
                     this.Controls.Add(oTable);
                     ocell.Text = oProduct.GetCategoryName(_ProductID, true);
                     ocell.CssClass = _CssClass;
                     oTable.RenderControl(output);
                 }
             }

        }
    }
}

