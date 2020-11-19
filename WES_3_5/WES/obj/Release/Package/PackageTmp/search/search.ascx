<%@ Control Language="C#" AutoEventWireup="true" Inherits="search_search" Codebehind="search.ascx.cs" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<input id="HidItemPage" type="hidden" runat="server" />
<script language="javascript">
    function fnSetEvent() {
        document.getElementById("<%=hdnForClear.ClientID%>").value = "CLEAR";
    }

    function fnValidateSearchKeyword() {
        var SearchKeyword = document.getElementById("<%=txtSearch.ClientID%>").value;
        SearchKeyword = trim(SearchKeyword);
        if (SearchKeyword.length < 3) {
            document.getElementById("<%=lblSearchError.ClientID%>").innerHTML = "You must enter atleast 3 characters to search";
            return false;
        }
        else {
            window.document.location = 'powersearch.aspx?srctext='+SearchKeyword;
        }
    }
    function trim(str, chars) {
        return ltrim(rtrim(str, chars), chars);
    }

    function ltrim(str, chars) {
        chars = chars || "\\s";
        return str.replace(new RegExp("^[" + chars + "]+", "g"), "");
    }

    function rtrim(str, chars) {
        chars = chars || "\\s";
        return str.replace(new RegExp("[" + chars + "]+$", "g"), "");
    }

    function searchurl() {
        var ddlattrvalue = document.getElementById('<%=txtSearch.ClientID%>').value;
        if (ddlattrvalue != "") {
            if (ttrim(ddlattrvalue) != "") {
                document.getElementById('<%=txtsearchhidden.ClientID%>').value = ddlattrvalue;
            }
        }
        else {
           // alert("Keyword cannot be empty !");
            return false;
        }

    }

    function GetSelectedIts1() {
      
        var mySplitResult = "pppopt";
        var SelAttrStr = '';
        for (var j = 0; j < document.getElementById(mySplitResult).options.length; j++) {
            if (document.getElementById(mySplitResult).options[j].selected) {
                temp = document.getElementById(mySplitResult).options[j].value;
                
                document.getElementById("<%=HidItemPage.ClientID%>").value = temp;
            }
        }
        document.forms[0].submit();
    }
</script>
<script language="javascript" type="text/javascript">
    function blockspecialcharacters(e) {
        var keynum
        var keychar
        var numcheck
        if (window.event) {
            keynum = e.keyCode
        }
        else if (e.which) {
            keynum = e.which
        }
        keychar = String.fromCharCode(keynum)
        }
</script>

<script language="javascript" type="text/javascript">
    function geturl() {
        //alert("inside");
        var rootPath = window.location.protocol + "//" + window.location.host + "/";
        var e = document.getElementById("ctl00_maincontent_searchctrl1_search1_ddlcategory");
        var strURL = e.options[e.selectedIndex].text;
        //alert(strURL);
        document.getElementById('ctl00_maincontent_searchctrl1_search1_cv').value = strURL;
        var strURL1 = e.options[e.selectedIndex].value;
        var fullPath = rootPath + strURL1;
        if (e.options[e.selectedIndex].text != 'Select Category') {

            window.location.href = fullPath;
        }

    }

    function geturlsubcat() {
        var rootPath = window.location.protocol + "//" + window.location.host + "/";
        var f = document.getElementById("ctl00_maincontent_searchctrl1_search1_ddlsubcategory");
        var strURL = f.options[f.selectedIndex].text;
        var strURL1 = f.options[f.selectedIndex].value;

        var fullPath = rootPath + strURL1;
        if (f.options[f.selectedIndex].text != 'Select Category') {
            window.location.href = fullPath;
        }

    }
</script>

<input type="hidden" id="hdnForClear" runat="server" />

          <%
                Response.Write(Spell_Correction());
            %>
<div class="box2"  style="margin-top:4px;">
        

        <div style="padding: 0px; text-align: center; color: red; display: block; font-size: 11px;">
          <asp:HiddenField ID="txtsearchhidden" runat="server" />
           
          <asp:Label ID="lblSearchError" runat="server" ForeColor="#FF0000"></asp:Label>
        </div>
        <div class="searchpagesearch"  >
           <asp:TextBox ID="txtSearch" runat="server" CssClass="topsearch1" placeholder="Search WES! Enter Keywords or Part No's" ></asp:TextBox>
           <%-- <asp:ImageButton ID="btnSearch" runat="server" CssClass="hoverbtn searchbtn sb_search"  OnClientClick="return searchurl();" OnClick="btnsearch_Click" />    --%>   
           <asp:Button ID="btnSearch" runat="server" CssClass="hoverbtn searchbtn" style="border:0px;"  OnClientClick="return searchurl();" OnClick="btnsearch_Click" />     
       </div> 
       <div class="CategoryFilter">
       <div id="Rcatdiv">
       <span class="CategoryFiltertxt">Filter Products By Category: </span>
      <%-- <select id="ddlcategory" class="CategoryFilterdrop" onchange="geturl();" name="lst1" OnSelectedIndexChanged="itemSelected">
       </select>
       </div>--%>
       <asp:DropDownList  runat="server" class="CategoryFilterdrop"  id="ddlcategory"    AutoPostBack="True" onchange="geturl();"></asp:DropDownList>
       <div class="cl" style="height:3px;"></div>
       <div id="Scatdiv">
       <span id="span1" class="CategoryFiltertxt">Filter Products By SubCategory:</span>
      <%-- <select id="ddlsubcategory" class="CategoryFilterdrop1" onchange="geturlsubcat();" name="lst2">
       </select>--%>
        <asp:DropDownList  runat="server" class="CategoryFilterdrop1"  AutoPostBack="True"  id="ddlsubcategory" onchange="geturlsubcat();"></asp:DropDownList>
       </div>
       <p style="text-align:left;">Note. See Addtional Filtering Options in left side Navigation.</p>
       </div>

     
</div> 

  <div style="height:4px;"></div>
</div>
<asp:HiddenField ID="cv" runat="server" />