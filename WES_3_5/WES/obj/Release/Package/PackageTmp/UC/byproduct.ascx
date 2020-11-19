<%@ Control Language="C#" AutoEventWireup="true" Inherits="UC_byproduct" Codebehind="byproduct.ascx.cs" %>
<%--<%@ Register Src="~/search/search.ascx" TagName="search" TagPrefix="uc1" %>
<%@ Register Src="~/search/searchrsltcategory.ascx" TagName="searchrsltcategory" TagPrefix="uc2" %>
<%@ Register Src="~/search/searchrsltfamily.ascx" TagName="searchrsltfamily" TagPrefix="uc3" %>
<%@ Register Src="~/search/searchrsltproducts.ascx" TagName="searchrsltproducts" TagPrefix="uc4" %>
<%@ Register Src="~/search/searchparametricfilter.ascx" TagName="searchparametricfilter" TagPrefix="uc5" %>
<%@ Register Src ="~/search/searchbyproduct.ascx" TagName ="searchbyproduct" TagPrefix ="uc6" %>--%>
<link href="../search/templates/byproduct/searchrsltproducts/searchrsltproducts_files/base.css" rel="stylesheet" type="text/css" />
<script type="text/javascript">
        function __doPostBack(eventTarget, eventArgument) {
            document.getElementById("__EVENTTARGET").value = eventTarget;
            document.getElementById("__EVENTARGUMENT").value = eventArgument;
            document.forms[0].submit();
        }


        function GetSelItems(field) {

            var count=0;
            var sCategoryIds='';
            for (var j = 0; j < document.getElementsByName(field).length; j++)
            {            
                if(document.getElementsByName(field).item(j).checked==true)
                {
                    sCategoryIds = sCategoryIds + document.getElementsByName(field).item(j).value + ",";
                }
            }

            sCategoryIds = sCategoryIds.substring(0, sCategoryIds.length - 1)
            if (sCategoryIds.length > 0) 
                __doPostBack('CATEGORYFILTER', sCategoryIds);
            else
                alert('Please select atleast one category for filtering');
        }
        
        function GetCompareItems(field,fid)
        {
            var count=0;
            var st="";
            for (var j = 0; j < field.length; j++)
            {            
                if(field[j].checked)
                {
                //alert(field[j].value)
                st+=field[j].value+",";
                    count=count+1;
                }
            }
            //return false;
            if(count > 1)
            {
            //alert(document.getElementsByName("CompareItem").length);
                <%Session["CloseWin"]=Request.Form["CompareItem"];%> 
                __doPostBack('compare',fid+"$"+st);
            }
            else
            {
                alert('Please select atleast two items to compare');
            }
        }
        
        function CheckCompareCount(field,ctlid)
        {
            var count=1;
            for (var j = 0; j < field.length; j++)
            {
                if(field[j].checked)
                {
                    if(count > 5)
                    {
                        document.forms[0].elements[ctlid].checked=false;
                        alert('A maximum of 5 products can be compared at one time');
                    }
                    count=count+1;
                }
            }
        }
        
        function Geturlqstring(st)
        {
        var urls=new String();
        urls=window.location.href;
        alert(urls.lastIndexOf("#"))
        if(urls.lastIndexOf("#")>0)
        {
            alert(urls.substring(0,urls.lastIndexOf("#")))
        }
        else
        {
//            window.location.href=urls+'&'+st;
//document.getElementById("hdnFamilyId").value=st;
                __doPostBack('compare', st);
            return false;
        }
        return false;
        //alert(urls);
        }      
       
    </script>
<link href="search/templates/type1/searchrsltproducts/searchrsltproducts_files/base.css" type="text/css" rel="Stylesheet" />

    <input type="hidden" id="hdnFamilyId" runat="server">
    <input type="hidden" name="__EVENTTARGET" value="">
    <input type="hidden" name="__EVENTARGUMENT" value="">
    <table align="center" style="width: 558">
    <tr>
    <td align="center">
    <table width="558" border="0" cellspacing="0" cellpadding="0" onload="Getfidfromurl();">
        <tr>
          <td align="left" class="tx_1">
            <a href="home.aspx" style="color:#0099FF" class="tx_3">Home</a> / <%
               // Response.Write(Bread_Crumbs());
                %>
          </td>
        </tr>
        <tr>
          <td width=100%>
            <hr>
          </td>

        </tr>
      </table>
      </td>
      </tr>
        <tr>
            <td>
                <table style=" width: 100%;">                        
                    <tr>
                        <td>
                            <div style="width: 100%;  solid #D1D1D1">
                                <table style="width: 100%">
                                    <tr>
                                        <td>
                                           <%-- <uc1:search ID="search1"  Visible=false runat="server" />--%>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                           
                                            
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            
                                            <div style="width: 100%">
                                            <%--<uc6:searchbyproduct ID="searchrbyproduct1" runat="server" />--%>
                                            </div>
                                            <div style="width: 100%">
                                               <%-- <uc2:searchrsltcategory ID="searchrsltcategory1" runat="server" />--%>
                                            </div>
                                            <%--<div style="width: 100%">
                                             <uc5:searchparametricfilter ID="searchparametricfilter1" runat="server" />
                                             </div>--%>
                                            <div style="width: 100%">
                                                <%--<uc4:searchrsltproducts ID="searchrsltproducts1" runat="server" />--%>
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>