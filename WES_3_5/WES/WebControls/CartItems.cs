using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.ComponentModel;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Globalization;
using TradingBell.Common;
using TradingBell.WebServices;


/// <summary>
/// This web control is used to show the number of Items
/// Currently in the View Cart and its Subtotal.
/// </summary>
/// 
[assembly: TagPrefix("TradingBell.WebServices.UI", "WebCat")]
[assembly: System.Reflection.AssemblyVersion("5.0")]

namespace TradingBell.WebServices.UI
{
    public class CartItems : WebControl
    {
        #region "Declarations"
        int _UserId;
        string _CartText;
        string _ItemText;
        string _CartTextCssClass;
        string _ValueText;
        string _ValueTextCssClass;
        string _Skin;
        string _Currency;

        #endregion

        #region "Property"
        [Browsable(true),
        Category("TradingBell"),
        DefaultValue("")
        ]
        public int UserId
        {
            get
            {
                return _UserId;
            }
            set
            {
                _UserId = value;
            }

        }
        [Browsable(true),
        Category("TradingBell"),
        DefaultValue("")]
        public string CartText
        {

            get
            {
                return _CartText;
            }
            set
            {
                _CartText = value;
            }

        }
        [Browsable(true),
        Category("TradingBell"),
        DefaultValue("")]
        public string ItemText
        {

            get
            {
                return _ItemText;
            }
            set
            {
                _ItemText = value;
            }

        }
        [Browsable(true),
        Category("TradingBell"),
        DefaultValue("")]
        public string CartTextCssClass
        {

            get
            {
                return _CartTextCssClass;
            }
            set
            {
                _CartTextCssClass = value;
            }

        }

        [Browsable(true),
        Category("TradingBell"),
        DefaultValue("")]
        public string ValueText
        {

            get
            {
                return _ValueText;
            }
            set
            {
                _ValueText = value;
            }

        }

        [Browsable(true),
        Category("TradingBell"),
        DefaultValue("")]
        public string ValueTextCssClass
        {

            get
            {
                return _ValueTextCssClass;
            }
            set
            {
                _ValueTextCssClass = value;
            }

        }
        [Browsable(true),
        Category("TradingBell"),
        DefaultValue("")]
        public override string SkinID
        {

            get
            {
                return _Skin;
            }
            set
            {
                _Skin = value;
            }

        }
        [Browsable(true),
       Category("TradingBell"),
       DefaultValue("")]
        public string Currency
        {
            get
            {
                return _Currency;
            }
            set
            {
                _Currency = value;
            }

        }

        #endregion

        protected override void RenderContents(HtmlTextWriter output)
        {
            if (!DesignMode)
            {
                Helper oHelper = new Helper();
                ErrorHandler oErr = new ErrorHandler();
                Category oCat = new Category();
                ProductFamily oPF = new ProductFamily();
                Order oOrder = new Order();
                Table oTable = new Table();
                TableRow oRow = new TableRow();
                TableCell oCell = new TableCell();
                int OpenOrdStatusID = (int)Order.OrderStatus.OPEN;

                if (_UserId != 0)
                {
                    int OrderID = 0;

                    if ((HttpContext.Current.Session["ORDER_ID"] != null && Convert.ToInt32(HttpContext.Current.Session["ORDER_ID"]) > 0) || (HttpContext.Current.Request.QueryString["ViewOrder"] != null && HttpContext.Current.Request.QueryString["ViewOrder"].Equals("View")))
                    {
                        OrderID = Convert.ToInt32(HttpContext.Current.Session["ORDER_ID"]);
                    }
                    else
                    {
                        OrderID = oOrder.GetOrderID(_UserId, OpenOrdStatusID);
                    }
                    string OrderStatus = oOrder.GetOrderStatus(OrderID);

                    if (OrderID > 0 && (OrderStatus == Order.OrderStatus.OPEN.ToString() || OrderStatus == "CAU_PENDING"))
                    {
                        if (oOrder.GetOrderItemCount(OrderID) == 0)
                            oCell.Text = _ItemText + "No " + _CartText;
                        else
                            oCell.Text = _ItemText + oOrder.GetOrderItemCount(OrderID) + " " + _CartText;

                        //oCell.SkinID = _Skin;
                        oCell.CssClass = CartTextCssClass;
                        oRow.Controls.Add(oCell);
                        oTable.Controls.Add(oRow);
                        if (oOrder.GetOrderItemCount(OrderID) > 0)
                        {
                            oCell = new TableCell();
                            oRow = new TableRow();
                            oCell.Text = _ValueText + " " + _Currency + oOrder.GetCurrentProductTotalCost(OrderID);
                            oCell.CssClass = ValueTextCssClass;
                            oRow.Controls.Add(oCell);
                            oTable.Controls.Add(oRow);
                        }
                        //oRow.SkinID = _Skin;
                        oTable.SkinID = _Skin;



                    }
                    else
                    {
                        oCell.Text = _ItemText + "No " + _CartText;
                        oCell.CssClass = CartTextCssClass;
                        oRow.Controls.Add(oCell);
                        oTable.Controls.Add(oRow);
                        //oCell = new TableCell();
                        // oRow = new TableRow();
                        // oCell.Text = _ValueText + " " + _Currency + " 0.00";
                        // oCell.CssClass = ValueTextCssClass;
                        //oRow.Controls.Add(oCell);
                        oTable.SkinID = _Skin;
                        // oTable.Controls.Add(oRow);


                    }
                }
                else
                {
                    oCell.Text = _ItemText + "No " + _CartText;
                    oCell.CssClass = CartTextCssClass;
                    oRow.Controls.Add(oCell);
                    oTable.Controls.Add(oRow);
                    //oCell = new TableCell();
                    //oRow = new TableRow();
                    //oCell.Text = _ValueText + " " + _Currency + " 0.00";
                    //oCell.CssClass = ValueTextCssClass;
                    //oRow.Controls.Add(oCell); 
                    oTable.SkinID = _Skin;
                    //oTable.Controls.Add(oRow);


                }

                oTable.RenderControl(output);
            }
            else
            {
                // DesignMode
                Image oImg = new Image();
                oImg.ImageUrl = "~/Images/CartItems.gif";
                oImg.ID = "imgDefault";
                oImg.Width = 125;
                this.Controls.Add(oImg);
                oImg.RenderControl(output);

            }
        }
    }
}