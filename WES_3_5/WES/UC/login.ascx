﻿<%@ Control Language="C#" AutoEventWireup="true" Inherits="UC_login" EnableTheming="true" Codebehind="login.ascx.cs"  %> <%@ Register Src="~/UC/newproductsnav.ascx" TagName="newproductsnav" TagPrefix="uc1" %> <%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %> <input id="hidpwd" name="hidpwd" type="hidden" runat="server" /> <script type="text/javascript">
//        var text = document.forms[0].elements["<%=txtPassword.ClientID%>"].value;
//        if (text.length >= 15) {
//            alert('Password Length should not be greater than 15.');
//        }       
        System.Data.DataSet dataset = new System.Data.DataSet();
        //var webRequest = System.Net.WebRequest.Create(@path + "Links.txt");

        //using (var response = webRequest.GetResponse())
        //using (var content = response.GetResponseStream())
        //using (var reader = new System.IO.StreamReader(content))
        //{
        //    string restr = "";
        //    System.Xml.XmlDocument xd = new System.Xml.XmlDocument();
        //    restr = reader.ReadToEnd();
        //    restr = "{ \"rootNode\": {" + restr.Trim().TrimStart('{').TrimEnd('}') + "} }";
        //    xd = (System.Xml.XmlDocument)Newtonsoft.Json.JsonConvert.DeserializeXmlNode(restr);
        //    dataset.ReadXml(new System.Xml.XmlNodeReader(xd));
        //}
         dataset = (System.Data.DataSet)HttpContext.Current.Cache["Cache_PreLogin"];
        if (dataset != null & dataset.Tables.Count > 0 && dataset.Tables[0].Rows.Count > 0)
        {
            for (int i = 0; i < dataset.Tables[0].Rows.Count; i++)
            {
                string image =path + dataset.Tables[0].Rows[i]["IMG_NAME"].ToString();