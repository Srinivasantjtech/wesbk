using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TradingBell.WebCat.CatalogDB;
using TradingBell.WebCat.Helpers;
using TradingBell.WebCat.TemplateRender;
using TradingBell.WebCat.CommonServices;

    public partial class WebSiteSettings : System.Web.UI.Page
    {
        UserServices objUserServices = new UserServices();
        HelperServices objHelperServices = new HelperServices();
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
               
                if (!IsPostBack)
                {
                    if (Session["USER_ID"] != null)
                    {

                        int USERID = Convert.ToInt32(  Session["USER_ID"]);
                        if ((Session["USER_ROLE"].ToString()) != "1")
                        {
                            Response.Redirect("ConfirmMessage.aspx?Result=NOTADMIN");
                        }
                        int chkcheckout_option = 0;
                        int chkdup_orderno=0;
                        int UsrID = objHelperServices.CI(Session["USER_ID"].ToString());
                        chkcheckout_option = objUserServices.GetCheckOutOption(UsrID);
                        if (chkcheckout_option == 1)
                        {
                            chkcheckoutoptions.Checked = true;
                        }
                        else
                        {
                            chkcheckoutoptions.Checked = false;
                        }

                        chkdup_orderno = objUserServices.GET_DUP_ORDERNO_OPTION(UsrID);
                        if (chkdup_orderno == 1)
                        {
                            ChkDuporder.Checked = true;
                        }
                        else
                        {
                            ChkDuporder.Checked = false;
                        }

                    }
                    else
                    {
                        Response.Redirect("login.aspx");
                    }
                }
                
            }
            catch
            { }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            int rst = 0;
            int UsrID = objHelperServices.CI(Session["USER_ID"].ToString());
            int ChkRst = 0;
            if (chkcheckoutoptions.Checked == true)
            {
                ChkRst = 1;
            }
            else
            {
                ChkRst = 0;
            }
            rst = objUserServices.ChangeCheckOutOption(UsrID, ChkRst);
            if (rst == 1)
            {
                Response.Redirect("ConfirmMessage.aspx?Result=CHECKOUTOPTIONCHANGED");
            }
            else
            {
                Response.Redirect("home.aspx");
            }
        }


        protected void btnorderman_Click(object sender, EventArgs e)
        {
            int rst = 0;
            int UsrID = objHelperServices.CI(Session["USER_ID"].ToString());
            int ChkRst = 0;
            if (ChkDuporder.Checked == true)
            {
                ChkRst = 1;
            }
            else
            {
                ChkRst = 0;
            }
            
            rst = objUserServices.UPDATE_DUPLICATEOption(UsrID, ChkRst);
            if (rst == 1)
            {
                Response.Redirect("ConfirmMessage.aspx?Result=UPDATE_DUPLICATE");
            }
            else
            {
                Response.Redirect("home.aspx");
            }
        }
    }
