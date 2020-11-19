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
public partial class ChangeUserName : System.Web.UI.Page
{
    UserServices objUserServices = new UserServices();
    HelperServices objHelperServices = new HelperServices();
    ErrorHandler objErrorHandler = new ErrorHandler();
    string OldUserName = "";
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
            if (!IsPostBack )
            {
                
                
                    txtOldUserName.Text = objUserServices.GetUserLoginName(objHelperServices.CI(Session["USER_ID"].ToString()));
                    //txtOldUserName.BackColor = System.Drawing.Color.DarkGray;
                    txtOldUserName.ReadOnly = true;
                    OldUserName = objUserServices.GetUserLoginName(objHelperServices.CI(Session["USER_ID"].ToString()));                
            }
        }
        catch (Exception ex)
        {
            objErrorHandler.ErrorMsg = ex;
            objErrorHandler.CreateLog();
        }
        txtOldUserName.Attributes.Add("onkeypress", "javascript:return blockspecialcharacters(event);");
        txtNewUserName.Attributes.Add("onkeypress", "javascript:return blockspecialcharacters(event);");
        txtConfirmUserName.Attributes.Add("onkeypress", "javascript:return blockspecialcharacters(event);");
    }
    #region "Control events"
    protected void btnChange_Click(object sender, EventArgs e)
    {
        int UsrID = objHelperServices.CI(Session["USER_ID"].ToString());
        int cmpid =objUserServices.GetCompanyID(UsrID);
        try
        {
            if (Request["ChangePass"] != null)
            {
                //txtOldUserName.Text = objUserServices.GetPassword(objHelperServices.CI(Session["USER_ID"].ToString()));
                //rfvOldPwd.Visible = false;
            }
            if (txtOldUserName.Text != "" && txtNewUserName.Text != "" && txtConfirmUserName.Text != "") //txtOldUserName.Text 
            {
                OldUserName = objUserServices.GetUserLoginName(UsrID);

                if (OldUserName.ToUpper() != txtOldUserName.Text.ToUpper().Trim().ToString())
                {
                    lblError.Text = (string)GetLocalResourceObject("ErrMsgInValid");
                }
                else if (txtNewUserName.Text.Trim().ToString() != txtConfirmUserName.Text.Trim().ToString())
                {
                    lblError.Text = (string)GetLocalResourceObject("ErrMsgPwdSame");
                    //"New UserName and confirm UserName are not same"
                }
                else if (txtNewUserName.Text.ToString().ToUpper().StartsWith(cmpid.ToString().ToUpper()) == true) 
                {
                    lblError.Text = "User name should not contain " + cmpid.ToString() + ",try with different user name" ; 
                }
                else if (txtNewUserName.Text.ToString().ToUpper().StartsWith("WES") == true || txtNewUserName.Text.ToString().ToUpper().StartsWith("CELL") == true || txtNewUserName.Text.ToString().ToUpper().StartsWith("WAG") == true)
                {
                    lblError.Text = "User name should not contain `WES` or `CELL` or `WAG`,try with different user name";
                }
                else if (txtNewUserName.Text.Trim().ToString() != txtOldUserName.Text.Trim().ToString())
                {
                    if (OldUserName.ToUpper() == txtOldUserName.Text.ToUpper().Trim().ToString())
                    {

                        DataSet tmpdb = objUserServices.CheckMultipleLoginName(txtNewUserName.Text,"");
                        if (txtNewUserName.Text.Trim().Length < 6)
                        {
                            lblError.Text = (string)GetLocalResourceObject("ErrMsgPwdLimit");
                            //"UserName must be 6 or more characters.";
                        }
                        else if (tmpdb!=null && tmpdb.Tables.Count>0 && tmpdb.Tables[0].Rows.Count>0)
                        {
                            lblError.Text = (string)GetLocalResourceObject("ErrMsgExists");
                        }
                        else if (objUserServices.ChangeLoginName(UsrID, txtNewUserName.Text) > 0)
                        {
                            // Session.RemoveAll();
                            Session["USER_ID"] = "";
                            Session["USER_NAME"] = null;
                            if (Request.Cookies["LoginInfoCL"] != null && Request.Cookies["LoginInfoCL"].Value.ToString().Trim() != "")
                            {
                                HttpCookie LoginInfoCookie = new HttpCookie("LoginInfoCL");
                                LoginInfoCookie["UserName"] = txtNewUserName.Text;
                                LoginInfoCookie["Password"] = objUserServices.GetPassword(UsrID);
                                LoginInfoCookie.Expires = DateTime.Now.AddDays(1);
                                HttpContext.Current.Response.AppendCookie(LoginInfoCookie);
                            }
                            Response.Redirect("ConfirmMessage.aspx?Result=LOGINNAMECHANGED");
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
