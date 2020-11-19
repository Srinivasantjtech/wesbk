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

using TradingBell.WebCat.CatalogDB;
using TradingBell.WebCat.Helpers;
using TradingBell.WebCat.TemplateRender;
using TradingBell.WebCat.CommonServices;
public partial class ChangePassword : System.Web.UI.Page
{
    Security objcrpengine = new Security(); 
    UserServices objUserServices = new UserServices();
    HelperServices objHelperServices = new HelperServices();
    ErrorHandler objErrorHandler = new ErrorHandler();
    string OldPwd = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        Session["PageUrl"] = Request.Url.PathAndQuery.ToString().Substring(Request.Url.PathAndQuery.IndexOf("/", 1) + 1);
        Page.Title = objHelperServices.GetOptionValues("BROWSER TITLE").ToString();
        HtmlMeta meta = new HtmlMeta();
        meta.Name = "keywords";
        meta.Content = objHelperServices.GetOptionValues("Meta keyword").ToString();
        this.Header.Controls.Add(meta);

        // Render: <meta name="Description" content="noindex" />
        meta = new HtmlMeta();
        meta.Name = "Description";
        meta.Content = objHelperServices.GetOptionValues("Meta Description").ToString();
        this.Header.Controls.Add(meta);

        // Render: <meta name="Abstraction" content="Some words listed here" />

        meta.Name = "Abstraction";
        meta.Content = objHelperServices.GetOptionValues("Meta Abstraction").ToString();
        this.Header.Controls.Add(meta);

        // Render: <meta name="Distribution" content="noindex" />
        meta = new HtmlMeta();
        meta.Name = "Distribution";
        meta.Content = objHelperServices.GetOptionValues("Meta Distribution").ToString();
        this.Header.Controls.Add(meta);
        try
        {
            // Page.Title = Helper.oHelper.GetOptionValues("BROWSER TITLE"].ToString();
            if (!IsPostBack && Request["ChangePass"] != null)
            {
                if (Request["ChangePass"] == "True")
                {
                    txtOldPassword.Text = objUserServices.GetPassword(objHelperServices.CI(Session["USER_ID"].ToString()));
                    txtOldPassword.BackColor = System.Drawing.Color.DarkGray;
                   // txtOldPassword.ReadOnly = true;
                    OldPwd = objUserServices.GetPassword(objHelperServices.CI(Session["USER_ID"].ToString()));
                }
            }
        }
        catch (Exception ex)
        {
            objErrorHandler.ErrorMsg = ex;
            objErrorHandler.CreateLog();
        }
       // txtOldPassword.Attributes.Add("onkeypress", "javascript:return blockspecialcharacters(event);");
       // txtNewPassword.Attributes.Add("onkeypress", "javascript:return blockspecialcharacters(event);");
       // txtConfirmPassword.Attributes.Add("onkeypress", "javascript:return blockspecialcharacters(event);");
    }
    #region "Control events"
    protected void btnChange_Click(object sender, EventArgs e)
    {
        int UsrID = objHelperServices.CI(Session["USER_ID"].ToString());
        try
        {
            if (Request["ChangePass"] != null)
            {
                txtOldPassword.Text = objUserServices.GetPassword(objHelperServices.CI(Session["USER_ID"].ToString()));
                //rfvOldPwd.Visible = false;
            }
            if (txtOldPassword.Text != "" && txtNewPassword.Text != "" && txtConfirmPassword.Text != "") //txtOldPassword.Text 
            {
                OldPwd = objUserServices.GetPassword(UsrID);
                //Modified by:Indu
                //Modified on:11-Apr-2013
                //Mofified reason:To Build encrption logic
                string cibertext = objcrpengine.StringEnCrypt(txtOldPassword.Text);
                if (OldPwd != cibertext)
               // if (OldPwd != txtOldPassword.Text.Trim().ToString())
                {
                    lblError.Text = (string)GetLocalResourceObject("ErrMsgInValid");
                }
                else if (txtNewPassword.Text.Trim().ToString() != txtConfirmPassword.Text.Trim().ToString())
                {
                    lblError.Text = (string)GetLocalResourceObject("ErrMsgPwdSame");
                    //"New password and confirm password are not same"
                }
                else if (txtNewPassword.Text.Trim().ToString() != txtOldPassword.Text.Trim().ToString())
                {
                    if (OldPwd == cibertext)
                    //if (OldPwd == txtOldPassword.Text.Trim().ToString())
                    {
                        //Modified by:Indu
                        //Modified on:11-Apr-2013
                        //Mofified reason:To Build encrption logic
                       
                        string cipherText = objcrpengine.StringEnCrypt(txtNewPassword.Text);
                        if (txtNewPassword.Text.Trim().Length < 6)
                        {
                            lblError.Text = (string)GetLocalResourceObject("ErrMsgPwdLimit");
                            //"Password must be 6 or more characters.";
                        }

                        else if (objUserServices.ChangePassword(UsrID, cipherText) > 0)
                      //  else if (objUserServices.ChangePassword(UsrID, txtNewPassword.Text) > 0)
                        {
                            // Session.RemoveAll();
                            Session["USER_ID"] = "";
                            Session["USER_NAME"] = null;
                            if (Request.Cookies["LoginInfoCL"] != null && Request.Cookies["LoginInfoCL"].Value.ToString().Trim() != "")
                            {
                                HttpCookie LoginInfoCookie = new HttpCookie("LoginInfoCL");
                                LoginInfoCookie["UserName"] = objUserServices.GetUserEmailAdd(UsrID);
                                LoginInfoCookie["Password"] = txtNewPassword.Text;
                                LoginInfoCookie.Expires = DateTime.Now.AddDays(1);
                                HttpContext.Current.Response.AppendCookie(LoginInfoCookie);
                            }
                            Response.Redirect("ConfirmMessage.aspx?Result=PASSWORDCHANGED");
                        }
                    }
                    else
                    {
                        lblError.Text = (string)GetLocalResourceObject("ErrMsgInValid");
                    }
                }
                else
                {
                    lblError.Text = (string)GetLocalResourceObject("ErrMsgNoPwdSame");
                    if (Request["ChangePass"] != null && Request["ChangePass"] == "True")
                    {
                        Session.RemoveAll();
                    }
                }
            }
        }
        catch (Exception ex)
        {
            objErrorHandler.ErrorMsg = ex;
            objErrorHandler.CreateLog();
        }
    }
    #endregion
}
