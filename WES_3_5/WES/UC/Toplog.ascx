<%@ Control Language="C#" AutoEventWireup="true" Inherits="UC_Toplog" Codebehind="Toplog.ascx.cs" %>
<%@ Register Src="CartItems.ascx" TagName="CartItems" TagPrefix="uc1" %>

 <% 
     if (HttpContext.Current.Session["USER_ID"] == null || HttpContext.Current.Session["USER_ID"].ToString() == "")
     Response.Write(ST_top());
      %>