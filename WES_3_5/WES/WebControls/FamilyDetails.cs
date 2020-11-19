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

/// <summary>
///This web control is used to display the category name or
/// Family name  as header in the Family Display Page..
/// </summary>

[assembly: TagPrefix("TradingBell.WebServices.UI", "WebCat")]
[assembly: System.Reflection.AssemblyVersion("5.0")]

namespace TradingBell.WebServices.UI
{
    
    public class FamilyDetails : WebControl
      {
        public enum DisplayText
        {
            CategoryHierarchy = 1,
            FamilyID = 2,
            FamilyName = 3,
            FootNotes = 4
        }

        DisplayText _currentDisplay;
        int _FamilyID;
        string _ControlCssClass;
        string _CategoryID;
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

        public string CategortID
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

        public string ControlCssClass
        {
            get
            {
                return _ControlCssClass;
            }
            set
            {
                _ControlCssClass = value;
            }
        }
        #endregion
             protected override void RenderContents(HtmlTextWriter output)
            {
            if (DesignMode)
            {
                Image oImg = new Image();
                oImg.ImageUrl = "~/Images/FamilyDetails.gif";
                oImg.ID = "imgDefault";
                oImg.Width = 125;
                this.Controls.Add(oImg);
                oImg.RenderControl(output);
            }
            else
            {
                ProductFamily oTBFamiy = new ProductFamily();
                Helper oHelper = new Helper();
                Label lblControl = new Label();
                lblControl.ID = "lblFamilyDetails";
                lblControl.CssClass = _ControlCssClass;

                if (_FamilyID > 0)
                {
                    _FamilyID = oHelper.CI(_FamilyID.ToString());
                }
                if (_CategoryID != null)
                {
                    _CategoryID = _CategoryID.ToString();
                }

                if (_currentDisplay == DisplayText.FamilyName)
                { lblControl.Text = oTBFamiy.GetFamilyName(_FamilyID); }
                else if (_currentDisplay == DisplayText.CategoryHierarchy)
                {
                    if (_FamilyID != 0)
                    {
                    lblControl.Text = oTBFamiy.CategoryName(_FamilyID);
                    }
                    if (_CategoryID != null)
                    {
                     
                       
                        lblControl.Text = oTBFamiy.GetParentCategory(_CategoryID); 
                    }

                }
                else if (_currentDisplay == DisplayText.FamilyID)
                {
                    lblControl.Text = _FamilyID.ToString(); 
                }
                else if (_currentDisplay == DisplayText.FootNotes)
                {
                    lblControl.Text = oTBFamiy.FootNotes(_FamilyID);
                }
                lblControl.RenderControl(output);
            }
        }
    }
}


        