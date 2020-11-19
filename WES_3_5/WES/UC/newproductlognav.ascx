<%@ Control Language="C#" AutoEventWireup="true" Inherits="UC_newproductlognav" Codebehind="newproductlognav.ascx.cs" %>
<%
//Response.Write(ST_Newproduct());

   if (Request.Url.ToString().ToLower().Contains("orderdetails.aspx") || Request.Url.ToString().Contains("Shipping.aspx"))
    {
        string path = System.Configuration.ConfigurationManager.AppSettings["CDNBANNER"].ToString() + "CartContents" + "/";
        System.Data.DataSet dataset = new System.Data.DataSet();
        dataset = (System.Data.DataSet)HttpContext.Current.Cache["Cache_CartContentsBanner"];
        if (dataset != null & dataset.Tables.Count > 0 && dataset.Tables[0].Rows.Count > 0)
        {
            string image =path + dataset.Tables[0].Rows[0]["IMG_NAME"].ToString();
     %>
            <a href="<%=dataset.Tables[0].Rows[0]["URL"].ToString() %>" target="_blank" class="">
			    <img  width="180px" src="<%=image%>"/>
		    </a>
     <%
         }
     
    }
    else
    {
        Response.Write(ST_Newproduct());
    }

%>
