<%@ Control Language="C#" AutoEventWireup="true" Inherits="UC_topnext" Codebehind="topnext.ascx.cs" %> <%@ Import Namespace="TradingBell.WebCat.Helpers" %> <%@ Register Src="Quickbuy.ascx" TagName="QuickOrder" TagPrefix="uc2" %> <%--<link href="../css/stilos_.css" rel="stylesheet" type="text/css"/>--%> <%@ Import Namespace="System.Data" %> <%@ Import Namespace ="TradingBell.WebCat.Helpers" %> <%@ Import Namespace ="TradingBell.WebCat.CatalogDB" %> <%@ Import Namespace ="TradingBell.WebCat.CommonServices" %> <%--<script src="../Scripts/AC_RunActiveContent.js" type="text/javascript"></script>--%> <script type="text/javascript">function mouseOverImg(A,B){switch(parseInt(B)){case 1:document.images[A].src="images/btn_images/btn_lnk_qo_mo.png";break;case 2:document.images[A].src="images/btn_images/btn_lnk_wn_mo.png";break;case 3:document.images[A].src="images/btn_images/btn_lnk_cat_mo.png";break}}function mouseOutImg(A,B){switch(parseInt(B)){case 1:document.images[A].src="images/btn_images/btn_lnk_qo.png";break;case 2:document.images[A].src="images/btn_images/btn_lnk_wn.png";break;case 3:document.images[A].src="images/btn_images/btn_lnk_cat.png";break}};</script> <table width="967px" cellpadding="0" cellspacing="0" border="0" style="border-collapse: collapse;"> <tr> <td width="967px"> <table width="967px" cellpadding="0" cellspacing="0" border="0" style="border-collapse: collapse;"> <tr> <td width="795px" valign="top"> <table width="795px" cellpadding="0" cellspacing="0" border="0" style="border-collapse: collapse"> <tr> <td valign="top" style="padding-top: 3px; padding-left: 0px; padding-right: 0px;
padding-bottom: 0px;"> <div id="featured" style="border: 0px; padding-left: 0px; padding-top: 0px; padding-bottom: 0px;"> <ul class="ui-tabs-nav" style="display: none;"> <li class="ui-tabs-nav-item ui-tabs-selected" id="nav-fragment-1"><a href="#fragment-1"> <img src="images/img_flash/11_th.jpg" alt="" /><span></span></a></li> <li class="ui-tabs-nav-item" id="nav-fragment-2"><a href="#fragment-2"> <img src="images/img_flash/12_th.jpg" alt="" /><span></span></a></li> <li class="ui-tabs-nav-item" id="nav-fragment-3"><a href="#fragment-3"> <img src="images/img_flash/13_th.jpg" alt="" /><span></span></a></li> <li class="ui-tabs-nav-item" id="nav-fragment-4"><a href="#fragment-4"> <img src="images/img_flash/14_th.jpg" alt="" /><span></span></a></li> </ul> <%-- <%
DataSet ds = new DataSet();
string banner1aurl = "";
string banner2aurl = "";
string banner3aurl = "";
string banner4aurl = "";
if (HttpContext.Current.Session["dsblink"] != null)
{
ds = (DataSet)HttpContext.Current.Session["dsblink"];
if (ds != null && ds.Tables[0].Rows.Count > 0)
{
if (ds.Tables[0].Rows[0]["BANNER1_LINK_NEW"].ToString() != null)
{
banner1aurl = ds.Tables[0].Rows[0]["BANNER1_LINK_NEW"].ToString();
}
if (ds.Tables[0].Rows[0]["BANNER1_LINK_NEW"].ToString() == null)
{
banner1aurl = "#";
}
if (ds.Tables[0].Rows[0]["BANNER2_LINK_NEW"].ToString() != null)
{
banner2aurl = ds.Tables[0].Rows[0]["BANNER2_LINK_NEW"].ToString();
}
if (ds.Tables[0].Rows[0]["BANNER2_LINK_NEW"].ToString() == null)
{
banner2aurl = "#";
}
if (ds.Tables[0].Rows[0]["BANNER3_LINK_NEW"].ToString() != null)
{
banner3aurl = ds.Tables[0].Rows[0]["BANNER3_LINK_NEW"].ToString();
}
if (ds.Tables[0].Rows[0]["BANNER3_LINK_NEW"].ToString() == null)
{
banner3aurl = "#";
}
if (ds.Tables[0].Rows[0]["BANNER4_LINK_NEW"].ToString() != null)
{
banner4aurl = ds.Tables[0].Rows[0]["BANNER4_LINK_NEW"].ToString();
}
if (ds.Tables[0].Rows[0]["BANNER4_LINK_NEW"].ToString() == null)
{
banner4aurl = "#";
}
}
}
%>--%>  <div id="fragment-1" class="ui-tabs-panel ui-tabs-hide" style="border: 0px;"> <a  id="banner1a" runat="server"> <img src="images/img_flash/11.jpg" width="795px" height="220" alt=""/> </a> </div>  <div id="fragment-2" class="ui-tabs-panel ui-tabs-hide" style=""> <a  id="banner2a" runat="server"> <img src="images/img_flash/12.jpg" width="795px" height="220" alt=""/> </a> </div>  <div id="fragment-3" class="ui-tabs-panel ui-tabs-hide" style=""> <a  id="banner3a" runat="server"> <img src="images/img_flash/13.jpg" width="795px" height="220" alt=""/> </a> </div>  <div id="fragment-4" class="ui-tabs-panel ui-tabs-hide" style=""> <a  id="banner4a" runat="server"> <img src="images/img_flash/14.jpg" width="795px" height="220" alt=""/> </a> </div> <div style="" class="ui-tabs-panel" id="fragment-5"> 
 <%-- <a href="https://www.wes.com.au/media/ebook/casio/index.html" target="_blank">
<img width="795px" height="220" alt="" src="images/img_flash/wes-xmas-homepage-afterlogin.gif"/> 
</a> --%>
 <%-- <a href="https://www.wes.com.au/product_list.aspx?&id=0&pcr=WES-05E&cid=UBIQUITI&tsb=&tsm=&searchstr=&type=Category&value=Ubiquiti&bname=&byp=0&Path=K6WOvrM7yDGJsbKYFM1l%2b53cP8HUT0Dxg%2bPOAn2YvT0MIR57wXzlJw7XrDMQVFfSeYOIYN2kI1k%3d" target="_blank">
<img width="795px" height="220" alt="" src="/images/img_flash/ubiquiti-banner-wes-external_afterlogin.jpg"/> 
</a> --%>

<div class="container-fluid zerospace">
<div class="container">
    <div class="main-banner slides">
		<%--<a href="https://www.wes.com.au/media/ebook/wescat2020/" target="_blank" class="">
			<img  width="795px" height="220" src="/images/banner-home2.jpg" />
		</a>
		<a  href="https://www.wes.com.au/media/ebook/wescat2020/" target="_blank" class="">
		<img  width="795px" height="220" src="/images/banner-home2.jpg"  />
		</a>--%>

            <% 
             try
             {
                //PostLogin
              string path=path = System.Configuration.ConfigurationManager.AppSettings["CDNBANNER"].ToString() + "PostLogin" + "/";
       ErrorHandler objErrorHandler = new ErrorHandler();
       DataSet dataset = (DataSet)HttpContext.Current.Cache["Cache_PostLogin"];
   //  objErrorHandler.CreateLog("banner created from session");
    
      if (dataset != null & dataset.Tables.Count > 0 && dataset.Tables[0].Rows.Count > 0)
      {
          for (int i = 0; i < dataset.Tables[0].Rows.Count; i++)
          {
              string image =path + dataset.Tables[0].Rows[i]["IMG_NAME"].ToString();
     %>
        <a href="<%=dataset.Tables[0].Rows[i]["URL"].ToString() %>" target="_blank" class="">
			<img  width="795px" height="220" src="<%=image%>"/>
		</a>
     <%
          }
      }
      }
      catch (Exception ex)
    {
        ErrorHandler objErrorHandler = new ErrorHandler();
       // objErrorHandler.ErrorMsg = ex;
       // objErrorHandler.CreateLog();
    }
     %>
		
	</div>
</div>
</div>
		   
</div> </div> </td> </tr> </table> </td> <td width="2px"> &nbsp; </td> <td width="170px" valign="top"> <table width="170px" cellpadding="0" cellspacing="0" border="0" style="border-collapse: collapse;"> <tr> <td style="padding-top: 3px; padding-left: 0px; padding-right: 0px; padding-bottom: 0px;"> <a href="categorylist.aspx?&ld=0&cid=WESNEWS&byp=2&path=K6WOvrM7yDGJsbKYFM1l%2b53cP8HUT0Dxg%2bPOAn2YvT1jeqpQhfYbVHmDiGDdpCNZ" onmouseover="mouseOverImg('imgQO',1)" onmouseout="mouseOutImg('imgQO',1)"> <img src="images/btn_images/btn_lnk_qo.png" alt="" name="imgQO" width="170" height="71" /> </a> <%-- <a href="categorylist.aspx?&ld=0&cid=WESNEWS&byp=2&path=K6WOvrM7yDGJsbKYFM1l%2b53cP8HUT0Dxg%2bPOAn2YvT1jeqpQhfYbVHmDiGDdpCNZ" class="WNPU"></a>--%> </td> </tr> <tr> <td style="padding-top: 1px; padding-left: 0px; padding-right: 0px; padding-bottom: 0px;"> <a href="BulkOrder.aspx?txtcnt=27" onmouseover="mouseOverImg('imgWN',2)" onmouseout="mouseOutImg('imgWN',2)"> <img src="images/btn_images/btn_lnk_wn.png" alt="" name="imgWN" width="170" height="71" /> </a> <%--<a href="BulkOrder.aspx?txtcnt=27" class="QO"></a>--%> </td> </tr> <tr> <td style="padding-top: 1px; padding-left: 0px; padding-right: 0px; padding-bottom: 0px;"> <a href="OnlineCatalogue_price.aspx" onmouseover="mouseOverImg('imgCAT',3)" onmouseout="mouseOutImg('imgCAT',3)"> <img src="images/btn_images/btn_lnk_cat.png" alt="" name="imgCAT" width="170" height="71" /> </a> <%-- <a href="CatalogueDownload.aspx?ActionResult=CATALOGUE" class="CPDF"></a>--%> </td> </tr> </table> </td> </tr> </table> </td> <%--<td valign="top"> <uc2:QuickOrder ID="QuickOrder1" runat="server" /> </td>--%> </tr> </table> 