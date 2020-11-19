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
using TradingBell.WebCat;
using TradingBell.WebCat.CatalogDB;
using TradingBell.WebCat.Helpers;
using TradingBell.WebCat.CommonServices;
//Modified by:Indu
//Modified on:11-Apr-2013
//Mofified reason:To Build encrption logic
public partial class ResetPassword : System.Web.UI.Page
{
    private static int _UserID;
    Security objcrpengine = new Security(); 
    UserServices objUserServices = new UserServices();
    CompanyGroupServices objCompanyGroupServices = new CompanyGroupServices();
    NotificationServices objNotificationServices = new NotificationServices();
    ConnectionDB objConnectionDB = new ConnectionDB();
    HelperServices objHelperServices = new HelperServices();
    ErrorHandler objErrorHandler = new ErrorHandler();
    UserServices.UserInfo oUserInfo = new UserServices.UserInfo();
    AjaxControlToolkit.ModalPopupExtender modalPop = new AjaxControlToolkit.ModalPopupExtender();
    int usercheck;
    int UserID;
    protected void Page_Load(object sender, EventArgs e)
    {

        try
        {
            if (Request.QueryString["pwdKey"] != null && Request.QueryString["pwdKey"].ToString().Trim() != "" && Request.QueryString["loginName"] != null && Request.QueryString["loginName"].ToString().Trim() != "")
            {
                bool validUser;
                string username;
              
                // int usercheck;
             txtLoginName.Text = Request.QueryString["loginName"];
                username = Request.QueryString["loginName"];
                validUser = objUserServices.CheckUserName(username);
                if (Request.QueryString["UserId"] == null)
                {
                    UserID = objUserServices.GetUserID(username);
                }
                else 
                
                {
                    UserID = Convert.ToInt32( Request.QueryString["UserId"].ToString());
                }
              txtCompanyID.Text = objUserServices.GetCompanyID(UserID).ToString();
           //   txtEmailAddress.Text = objUserServices.GetUserEmailAdd(usercheck);
                if (Request.QueryString["UserId"] != null)
                {
                    usercheck = Convert.ToInt32(Request.QueryString["UserId"].ToString());
                    oUserInfo = objUserServices.GetUserInfo(usercheck);
                    //if (oUserInfo.CUSTOMER_TYPE == "Retailer")
                    //{
                    txtCompanyID.Visible = false;
                    lblCompanyAccountNo.Visible = false;
                        string retailermailladdress = objUserServices.GetUserEmailAdd(usercheck);
                        txtEmailAddress.Text = retailermailladdress;
                        //Modified by:Indu
                        //Modified on:11-Apr-2013
                        //Mofified reason:To Build encrption logic
                        string temppwd = objcrpengine.StringEnCrypt(Request.QueryString["pwdKey"]);
                        if (objUserServices.CheckValidUserForForgetPassword(username, retailermailladdress, temppwd))
                        {
                            lblErrorMessage.Text = "";
                            lblErrorMessage.Visible = false;
                            Session["RESETSTATUS"] = "RESETVALID";
                            if (Session["RESETSTATUS"] != null && Session["RESETSTATUS"].ToString().Trim().Equals("RESETVALID"))
                            {
                                if (!IsPostBack)
                                {
                                    ShowPopUpReset();
                                }
                            }
                        }
                        else
                        {
                            lblErrorMessage.Text = "Invalid Account Information! Contact Wes Admin and then Try again";
                            lblErrorMessage.Visible = true;
                        }
                    }
                //}

                if (UserID == -1 || username == string.Empty || validUser == false)
                {
                    Session["ForgotAction"] = "Invalid User Name!";
                    Response.Redirect("Login.aspx", true);
                }

                Session["RESETSTATUS"] = "RESETVALID";
            }
        }
        catch (Exception ex)
        {
            objErrorHandler.ErrorMsg = ex;
            objErrorHandler.CreateLog();

        }
    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
     try
     {


        //Modified by:Indu
        //Modified on:11-Apr-2013
        //Mofified reason:To Build encrption logic

        string cipherText = objcrpengine.StringEnCrypt(Request.QueryString["pwdKey"]);
        int UserID;
        UserID = objUserServices.GetUserID(txtLoginName.Text);
        
       // if (objUserServices.CheckValidUserForForgetPassword(txtLoginName.Text, txtEmailAddress.Text, Request.QueryString["pwdKey"]))
      if (objUserServices.CheckValidUserForForgetPassword(txtLoginName.Text, txtEmailAddress.Text, cipherText))
        {
            lblErrorMessage.Text = "";
            lblErrorMessage.Visible = false;

            if (Session["RESETSTATUS"] != null && Session["RESETSTATUS"].ToString().Trim().Equals("RESETVALID"))
            {
                //Code Modified by:palani

                if (oUserInfo.CUSTOMER_TYPE != "Retailer")
                {
                    if (System.Convert.ToInt32(txtCompanyID.Text) == objUserServices.GetCompanyID(UserID) && objUserServices.CheckForgotUserEmail(txtLoginName.Text, txtEmailAddress.Text))
                        ShowPopUpReset();
                    else
                        lblErrorMessage.Visible = true;
                        lblErrorMessage.Text = "Invalid Company Account Number.Please Enter Valid Account Number.";
                        return;
                }
                else
                {
                    ShowPopUpReset();
                }
                
            }
        }
        else
        {
            lblErrorMessage.Text = "Invalid Account Information! Contact Wes Admin and then Try again";
            lblErrorMessage.Visible = true;
        }
     }
     catch (Exception ex)
     {
         objErrorHandler.ErrorMsg = ex;
         objErrorHandler.CreateLog();

     }
    }

    private void ShowPopUpReset()
    {
        modalPop.ID = "popUp";
        modalPop.PopupControlID = "ResetPWDPopupPanel";
        modalPop.BackgroundCssClass = "modalBackground";
        modalPop.TargetControlID = "btnHidden";
        modalPop.DropShadow = false;
        //modalPop.CancelControlID = "btnCancel";
        this.ResetPWDPopupPanel.Controls.Add(modalPop);
        this.modalPop.Show();
        txtNewPassword.Focus();
    }

    private void HidePopUpReset()
    {
        this.modalPop.Hide();
    }


  



    protected void btnUpdate_Click(object sender, EventArgs e)
    {
        try
        {
            string cipherText = objcrpengine.StringEnCrypt(txtConfirmPassword.Text);
            string TempPwd = objcrpengine.StringEnCrypt(Request.QueryString["pwdKey"].ToString());
            int chkvalue = objUserServices.UpdateNewPassword(txtLoginName.Text, UserID, 0, txtEmailAddress.Text, TempPwd, cipherText);
            // int chkvalue = objUserServices.UpdateNewPassword(txtLoginName.Text, UserID, System.Convert.ToInt32(txtCompanyID.Text), txtEmailAddress.Text, TempPwd, txtConfirmPassword.Text);

            if (chkvalue <= 0)
            {
                lblPwdErrorMessage.Text = "Unable to Proceed! Check Company AccountNo and Email";
                lblPwdErrorMessage.Visible = true;
                return;
            }
            else
            {
                tblusedidbtn.Visible = true;
                divmain.Visible = false; 
               // Response.Redirect("ConfirmMessage.aspx?Result=RESETPASSWORD", false);
            }

        }
        catch (Exception ex)
        { 
        
          objErrorHandler.ErrorMsg = ex;
         objErrorHandler.CreateLog();
        
        }

    }

 //   protected void btnUpdate_Click(object sender, EventArgs e)
 //   {
 //       try
 //       {
 //       bool validUser;
 //       string username;
 //       int UserID;

 //       lblPwdErrorMessage.Visible = false;
 //       username = txtLoginName.Text;
 //       validUser = objUserServices.CheckUserName(username);
 //       UserID = objUserServices.GetUserID(username);
 //       string TempPwd = Request.QueryString["pwdKey"].ToString();

 //       if (oUserInfo.CUSTOMER_TYPE != "Retailer")
 //       {

 //           if (System.Convert.ToInt32(txtCompanyID.Text) == objUserServices.GetCompanyID(UserID) && objUserServices.CheckForgotUserEmail(username, txtEmailAddress.Text))
 //           {
 // string cipherText = objcrpengine.StringEnCrypt(txtConfirmPassword.Text);
 //               TempPwd = objcrpengine.StringEnCrypt(Request.QueryString["pwdKey"].ToString());
 //    int chkvalue = objUserServices.UpdateNewPassword(txtLoginName.Text, UserID, System.Convert.ToInt32(txtCompanyID.Text), txtEmailAddress.Text, TempPwd, cipherText);          
 //// int chkvalue = objUserServices.UpdateNewPassword(txtLoginName.Text, UserID, System.Convert.ToInt32(txtCompanyID.Text), txtEmailAddress.Text, TempPwd, txtConfirmPassword.Text);

 //               if (chkvalue <= 0)
 //               {
 //                   lblPwdErrorMessage.Text = "Unable to Proceed! Check Company AccountNo and Email";
 //                   lblPwdErrorMessage.Visible = true;
 //                   return;
 //               }

 //               Response.Redirect("ConfirmMessage.aspx?Result=RESETPASSWORD", false);
 //           }
 //       }
 //       else
 //       {
 //           txtCompanyID.Text = oUserInfo.CompanyID;
 //           string retailermailladdress = objUserServices.GetUserEmailAdd(usercheck);
 //           if (System.Convert.ToInt32(txtCompanyID.Text) == objUserServices.GetCompanyID(UserID) && objUserServices.CheckForgotUserEmail(username, retailermailladdress))
 //           {

 //               string cipherText = objcrpengine.StringEnCrypt(txtConfirmPassword.Text);
 //               TempPwd = objcrpengine.StringEnCrypt(Request.QueryString["pwdKey"].ToString());
 //               int chkvalue = objUserServices.UpdateNewPassword(txtLoginName.Text, UserID, System.Convert.ToInt32(txtCompanyID.Text), retailermailladdress, TempPwd, cipherText);

 //               if (chkvalue <= 0)
 //               {
 //                   lblPwdErrorMessage.Text = "Unable to Proceed! Check Company AccountNo and Email";
 //                   lblPwdErrorMessage.Visible = true;
 //                   return;
 //               }

 //               Response.Redirect("ConfirmMessage.aspx?Result=RESETPASSWORD", false);
 //           }

 //       }

 //       }
 //       catch (Exception ex)
 //       {
 //           objErrorHandler.ErrorMsg = ex;
 //           objErrorHandler.CreateLog();

 //       }
 //   }
}