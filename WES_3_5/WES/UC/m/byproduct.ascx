 <%@ Control Language="C#" AutoEventWireup="true" Inherits="UC_byproduct" Codebehind="byproduct.ascx.cs" %>
<%@ Import Namespace ="TradingBell.WebCat.Helpers" %>
<%@ Import Namespace ="TradingBell.WebCat.CatalogDB" %>
<%@ Import Namespace ="TradingBell.WebCat.CommonServices" %> 
<%@ Import Namespace ="System.Data.OleDb" %> 
<%@ Import Namespace ="System.IO" %>  
<%@ Import Namespace ="System.Data" %>
	<meta http-equiv="content-type" content="text/html; charset=UTF-8">
	<meta charset="utf-8">
	<title>FFB Payment</title>
	<meta name="viewport" content="width=device-width, initial-scale=1.0, maximum-scale=1, user-scalable=no">
	<meta name="description" content="">




    <style>
        #cover-spin {
    position:fixed;
    width:100%;
    left:0;right:0;top:0;bottom:0;
    background-color: rgba(255,255,255,0.7);
    z-index:9999;
    display:none;
}

@-webkit-keyframes spin {
	from {-webkit-transform:rotate(0deg);}
	to {-webkit-transform:rotate(360deg);}
}

@keyframes spin {
	from {transform:rotate(0deg);}
	to {transform:rotate(360deg);}
}

#cover-spin::after {
    content:'';
    display:block;
    position:absolute;
    left:48%;top:40%;
    width:40px;height:40px;
    border-style:solid;
    border-color:black;
    border-top-color:transparent;
    border-width: 4px;
    border-radius:50%;
    -webkit-animation: spin .8s linear infinite;
    animation: spin .8s linear infinite;
}



	body { font-family:Arial, sanserif;}
		.myProduct_upl_wrap {
			padding: 10px 10px 20px;
			background: #f2f2f2;
			border-radius: 4px;
			min-height: 150px;
			margin-bottom:20px;
            width:970px;
		}
		.upload_title1 {
			background: #007ee0;
			border-radius: 4px 4px 4px 4px;
			color: #FFFFFF;
			font-size: 12px;
			height: 34px;
			line-height: 34px;
			text-indent: 15px;
            width:100%;
            margin-bottom:15px;
		}
        .title_pink {
			background: #FF1F3A;
			border-radius: 4px 4px 4px 4px;
			color: #FFFFFF;
			font-size: 12px;
			height: 34px;
			line-height: 34px;
			text-indent: 15px;
            width:100%;
		}
        
		table {
    background-color: transparent;
    border-collapse: collapse;
    border-spacing: 0;

	padding: 15px 10px;
}
        .myProduct_addtocart {
            border:none; 
        }
input {
    font-family: Arial,Tahoma,Helvetica;
    font-size: 11px;
    color: #666666;
    text-decoration: none;
    background-color: #FFF;
    border: 1px;
    border-style: solid;
    border-top-color: #B9C9CE;
    border-right-color: #EBEFF2;
    border-bottom-color: #EBEFF2;
    border-left-color: #B9C9CE;
}
.myproductPageTable {
    border:1px solid #ccc;
	font-size:14px;
}
.myproductPageTable td {
    border:1px solid #ccc;
	padding: 4px 6px;
}
.noTableborder td {
border:none !important;
}
.myproductPageTable a {
   color:#007ee0;
   text-decoration:none;
   font-size:14px;
}
        .buttons_wrap {
            width:96%;
            display:block;        
              
            margin-bottom:10px;
            text-align:left;
        }
         .upload_wrap {
            width:96%;
            display:block;
          
            margin-bottom:10px;
              text-align:left;
           
        }
.FamilyPageTableHead {
    background-color: #007BDB;
    /* vertical-align: bottom; */
    /* background-color: #15538A; */
    color: #FFFFFF;
    font-weight: bold;
    text-align: center;
    /* vertical-align: middle; */
    border-bottom: 1px solid #E8E8E8;
    border-right: 1px solid #E8E8E8;
    border-top: none;
    border-left: none;
    height: 23px;
    font-size: 11px;
}
.btnbuy2 {
    display: block;
    float: left;
    height: 26px;
    line-height: 27px;
    margin: 6px 0 0 5px;
    text-align: center;
    width: 56px;
}
.smallsiz {
    display: block;
    font-size: 11px;
    height: 24px;
    line-height: 21px;
    margin: auto;
    width: 60px;
}
.button {
    border-radius: 4px 4px 4px 4px;
    color: #FFFFFF !important;
    cursor: pointer;
}
.btngreen {
	background-color:#009900;
    border: 1px solid #009900 !important;
}
		
		
.file-upload-wrapper {
position: relative;
width: 322px;
height: 18px;
display: flex;
padding: 6px 3px;
}
input.file-upload-field {
  font-size: 12px;
  background: #fff;
  padding: 5px 15px;
  z-index: 20;
      width: 280px;
    height: 20px;
  line-height: 34px;
  color: #999;
  border:none;
  border-top-left-radius:6px;
  border-bottom-left-radius:6px;
  font-weight: 300;
  border:1px solid #ccc;
}
input[type=file].file-upload-field {
    -webkit-appearance:none;
    -moz-appearance:none;
}
       

.upload_btn {
  width:80px;
  font-size: 14px;
  height:32px;
  color:#fff;
  background: #009900;
  border-top-right-radius:6px;
  border-bottom-right-radius:6px;
  border:none;
}

.buttons_wrap a {
	width:100px;
	text-align:center;
	text-decoration:none;
	margin:0 5px;
	border-radius:6px;
	font-size:14px;	
}
.buttons_wrap a.blue {
  background: #007ee0;
  color:#fff;
  line-height:42px;
  width:180px;
}
.buttons_wrap a.green {
  background: #009900;
  color:#fff;
  line-height:42px;
}
.buttons_wrap a.blue:hover {
	opacity:0.8;
}
.buttons_wrap a.green:hover {
	opacity:0.8;
}
.dflex {
display:flex;
}
.title_blue {
  color:#007ee0;
  font-weight:normal;
}
.clear { clear:both; }

	</style>


  
<script type="text/javascript">
    function Callprogress() {
        alert("x");
        
    }

</script>

		<div class="myProduct_upl_wrap" id="divMain" runat="server" >
              
                <div class="upload_wrap">
            <h3 class="upload_title1">Please Upload Your  Products</h3> 
           <asp:LinkButton id="LinkButton2" 
                                                Text="Click Here to Download example Excel upload order sheet" 
                                                 OnClick="LinkButton_Click"  runat="server" class="toplinkatest" > 
                                                 <div class=""><p style="width:285px;">Click Here to Download Sample Sheet</p></div>     </asp:LinkButton>
                                          
                    <div style="width: 250px;float: left;">
                   <p style="display: inline;"> Create New Template </p>:<span class="redx">*</span>
                    <div class="clear"></div>
                                         
                                             <asp:TextBox  style="Width:220px;height:24px;margin-top: 5px;" ID="txtName" runat="server"  MaxLength="50" class="input_dr" ></asp:TextBox>
                     <asp:RequiredFieldValidator runat="server" ForeColor="Red" id="reqName" controltovalidate="txtName" errormessage="Please enter your Template name!" validationgroup="templ" />
                                       
                      
                <div style="text-align:center!important"  >
             <asp:Label runat="server" id="StatusLabel" text="Upload status: " Visible ="false" />
               </div>                     
             </div>       
                                                                     
                   <div style="width: 600px;float: left;" >                            
            <P class="p2" style="display: inline;">Upload your excel file for My Products. </P>
           
			<div class="clear"></div>

            
			<div class="dflex">



				<div class="file-upload-wrapper" data-text="Select your file!"  >
				    

                      <asp:FileUpload ID="FileUploadControl" runat="server" CssClass="file-upload-wrapper"   validationgroup="templ" BorderColor="#33CC33" BorderStyle="Solid" /> 
                     <asp:Button ID="btnUpload"  class="upload_btn" Text="Submit" runat="server"  OnClick="UploadButton_Click"  OnClientClick="$('#cover-spin').show(0)" />
                        
               

				</div>
				
				  
			</div>
                       </div>
			<div class="clear"></div>
		</div>
            <div class="clear"></div>
            <div class="buttons_wrap" id="divsavetemp" runat="server" visible="false">
               
                     <h3 class="upload_title1">Select From Saved Template</h3> 
            
<asp:GridView ID="dgtemplate" runat="server" AutoGenerateColumns="False" 
    DataKeyNames="ID" OnRowCommand="dgtemplate_RowCommand" CellPadding="4"  GridLines="None" PageSize="5"  BorderStyle="Solid" BorderWidth="1px" BorderColor="#6588AB" Width="100%"
   >
   
    <AlternatingRowStyle BackColor="#D8EBF5" />
   
    <Columns>
        <asp:BoundField DataField="ID" HeaderText="ID" InsertVisible="False"  Visible="false"
            ReadOnly="True" SortExpression="ID" />
        
        <asp:TemplateField HeaderText="Template Name" SortExpression="Filepath">
               
            <ItemTemplate >
                <asp:LinkButton ID="LbPath" runat="server" ForeColor="#007EE0" 
                    Text='<%# Eval("Filename") %>'
                    CommandName="PathUpdate" 
                    CommandArgument='<%#Bind("id") %>'>
                </asp:LinkButton>
            </ItemTemplate>
             <ItemStyle HorizontalAlign="Left" />
             </asp:TemplateField>
           <asp:TemplateField HeaderText="CreatedOn" SortExpression="Filepath">
            <ItemTemplate>
                <asp:Label ID="lblcreatedon" runat="server" ForeColor="#007EE0" Text='<%# Eval("CreatedOn") %>'></asp:Label>
                 
              
            </ItemTemplate>
               <ControlStyle Font-Size="13px" />
               <HeaderStyle HorizontalAlign="Center" />
            <ItemStyle ForeColor="#0066FF" HorizontalAlign="Center" />
             </asp:TemplateField>
       
         <asp:TemplateField HeaderText="View" SortExpression="Filepath">
            <ItemTemplate>
                    <asp:Image ID="Image2" runat="server" src="images/View_16x16.png"  CommandName="PathUpdate" 
                    />
                <asp:HyperLink ID="HyperLink2" runat="server"  CommandName="PathUpdate" 
                   >View Template</asp:HyperLink>
            
            </ItemTemplate>
             <HeaderStyle HorizontalAlign="Center" />
            <ItemStyle ForeColor="#0066FF" HorizontalAlign="Center" />
             </asp:TemplateField>
                <asp:TemplateField HeaderText="Edit" SortExpression="Filepath">
            <ItemTemplate>
                   <asp:Image ID="Image1" runat="server" src="images/Edit_16x16.png"   CommandName="PathUpdate" 
                   />
             <asp:HyperLink ID="HyperLink1" runat="server"  CommandName="PathUpdate"  
                    >Edit Template</asp:HyperLink>
            </ItemTemplate>
                    <HeaderStyle HorizontalAlign="Center" />
                      
                 
            <ItemStyle ForeColor="#0066FF" HorizontalAlign="Center" />
             </asp:TemplateField>
         <asp:TemplateField HeaderText="Delete" SortExpression="Filepath">
            <ItemTemplate>
                 <asp:Image ID="Image3" runat="server" src="images/Delete_order.jpg"  Width="16px" Height="16px" CommandName="PathUpdate" 
                    />
            
                 <asp:HyperLink ID="HyperLink3" runat="server"  CommandName="PathUpdate" 
                    >Delete Template</asp:HyperLink>
            </ItemTemplate>
             <HeaderStyle HorizontalAlign="Center" />
            <ItemStyle ForeColor="#0066FF" HorizontalAlign="Center" />
             </asp:TemplateField>

           <%-- <asp:TemplateField >
        <ItemTemplate>
                <asp:LinkButton ID="LbPath1" runat="server" 
                    Text='View'
                    CommandName="PathUpdate" 
                    CommandArgument='<%#Bind("id") %>'>
                </asp:LinkButton>
            </ItemTemplate>
  </asp:TemplateField>--%>
    </Columns>
    <EditRowStyle ForeColor="#0066FF" />
    <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
    <HeaderStyle BackColor="#266AAA" Font-Bold="True" ForeColor="White" HorizontalAlign="Left" Font-Size="15px" BorderStyle="None" />
    <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="left" />
    <RowStyle BorderStyle="None" Font-Bold="False" Font-Size="Large" Font-Underline="False" ForeColor="#0066FF" />
    <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
    <SortedAscendingCellStyle BackColor="#F5F7FB" />
    <SortedAscendingHeaderStyle BackColor="#6D95E1" />
    <SortedDescendingCellStyle BackColor="#E9EBEF" />
    <SortedDescendingHeaderStyle BackColor="#4870BE" />
</asp:GridView>
				</div>
			
         
             <div id="cover-spin"></div>

         
            </div>
<div Style="text-align:left;">
<asp:Label ID="lbltempname" Style="display:inline-block; color:#007ee0; font-size:16px; margin-top:12px; margin-bottom:6px;margin-left:12px" runat="server" Text="Template Name:Product" Visible="false"></asp:Label>
    </div>
		<div class="testscroll" id="divmyproduct" runat="server" visible="false"> 
                  <asp:Label runat="server" id="lblnoprod" text="No Products To Display" Visible ="false" />
               <%--<h3 class="title_blue">My Product List</h3>--%>
		 <asp:GridView ID="Gridprodlst" runat="server" CssClass="grdbord" AutoGenerateColumns="False"
            AllowPaging="True" OnRowDataBound="Gridprodlst_RowDataBound" Width="98%"
            DataKeyNames="CODE" OnRowCommand="Gridprodlst_RowCommand" EmptyDataRowStyle-BorderStyle="None"
            OnPageIndexChanging="Gridprodlst_PageIndexChanging" CellPadding="4" ForeColor="#333333" GridLines="None" BorderColor="#CCCCCC" BorderStyle="Solid" BorderWidth="1px" >
            <RowStyle Height="22px" />
                       <AlternatingRowStyle BackColor="#D8EBF5" />
            <Columns>
                <asp:TemplateField HeaderStyle-CssClass="grdheadermster" Visible="false">
                    <HeaderTemplate >
                       
                       pid
                    </HeaderTemplate>
                    <ItemTemplate>
                        <div style="padding: 0 3px; word-wrap: break-word; overflow: hidden;">
                            <asp:Label ID="lblprdid" runat="server" CssClass="labelstyle" Text='<%# bind("Product_Id") %>'></asp:Label>
                        </div>
                    </ItemTemplate>
                    <ItemStyle CssClass="itemstyle_border" Width="100px" />
                    <HeaderStyle CssClass="grdheadermster" BackColor="#0066FF" Width="100px" HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                </asp:TemplateField>
                <asp:TemplateField HeaderStyle-CssClass="grdheadermster">
                    <HeaderTemplate>
                        Order Code
                    
                    </HeaderTemplate>
                    <ItemTemplate>
                        <div style="padding: 0 3px; word-wrap: break-word; overflow: hidden; width: 100px">
                            <asp:Label ID="lblordercode" runat="server" Text='<%# bind("CODE") %>' CommandName="Entry"></asp:Label>
                        </div>
                    </ItemTemplate>
                    <ItemStyle CssClass="itemstyle_border" HorizontalAlign="Center" VerticalAlign="Middle" />
                    <HeaderStyle CssClass="grdheadermster"></HeaderStyle>
                </asp:TemplateField>
                <asp:TemplateField HeaderStyle-CssClass="grdheadermster" >
                    <HeaderTemplate>
                       Cost
                    </HeaderTemplate>
                    <ItemTemplate>
                        <div style="padding: 0 3px; word-wrap: break-word; overflow: hidden; width: 100px">
                            <asp:Label ID="lblCost" runat="server" CssClass="labelstyle" Text='<%# bind("Price") %>'></asp:Label>
                        </div>
                    </ItemTemplate>
                    <ItemStyle CssClass="itemstyle_border" HorizontalAlign="Center" VerticalAlign="Middle" />
                    <HeaderStyle CssClass="grdheadermster"></HeaderStyle>
                </asp:TemplateField>
                  <asp:TemplateField HeaderStyle-CssClass="grdheadermster">
                    <HeaderTemplate>
                       StockStatus
                    </HeaderTemplate>
                    <ItemTemplate>
                        <div style="padding: 0 3px; word-wrap: break-word; overflow: hidden; width: 200px">
                            <asp:Label ID="lblStockStatus" runat="server" CssClass="labelstyle" Text='<%# bind("Stock_Status") %>'></asp:Label>
                        </div>
                    </ItemTemplate>
                    <ItemStyle CssClass="itemstyle_border" HorizontalAlign="Center" VerticalAlign="Middle" />
                    <HeaderStyle CssClass="grdheadermster"></HeaderStyle>
                </asp:TemplateField>      
                     <asp:TemplateField HeaderStyle-CssClass="grdheadermster">
                    <HeaderTemplate>
                      Qty
                    </HeaderTemplate>
                    <ItemTemplate>
                        <div style="padding: 0 3px; word-wrap: break-word; overflow: hidden; width: 50px">
                            <asp:TextBox ID="txtqty"  runat="server" CssClass="labelstyle" Text="1"></asp:TextBox>
                        </div>
                    </ItemTemplate>
                    <ItemStyle CssClass="itemstyle_border" HorizontalAlign="Center" VerticalAlign="Middle" />
                    <HeaderStyle CssClass="grdheadermster"></HeaderStyle>
                </asp:TemplateField>  
                <asp:TemplateField HeaderStyle-CssClass="grdheadermster" ItemStyle-HorizontalAlign="Center">
                    <HeaderTemplate>
                        Add To Cart
                    </HeaderTemplate>
                    <ItemTemplate>
                       
                        <asp:ImageButton ID="ImageButton1" Visible='<%# bind("FlagVisible_Invisible") %>' runat="server" class="myProduct_addtocart green" AlternateText="" ImageUrl="~/images/AddToCart_Small.png" OnClick="BtnAddtoCart_Click"/>
                             
                    </ItemTemplate>
                    <ItemStyle CssClass="itemstyle_border" VerticalAlign="Middle" />
                    <HeaderStyle CssClass="grdheadermster"></HeaderStyle>
                </asp:TemplateField>
                 <asp:TemplateField HeaderStyle-CssClass="grdheadermster" ItemStyle-HorizontalAlign="Center">
                    <HeaderTemplate>
                        Delete
                    </HeaderTemplate>
                    <ItemTemplate>
                       
                        <asp:ImageButton ID="ImageButton1" Visible='<%# bind("FlagVisible_Invisible") %>' runat="server" class="myProduct_addtocart green" AlternateText="" ImageUrl="~/images/Delete_order.jpg" Width="16px" Height="16px" OnClick="BtnAddtoCart_Click"/>
                             
                    </ItemTemplate>
                    <ItemStyle CssClass="itemstyle_border" VerticalAlign="Middle" />
                    <HeaderStyle CssClass="grdheadermster"></HeaderStyle>
                </asp:TemplateField>
            </Columns>
                        <EditRowStyle BackColor="#2461BF" />

<EmptyDataRowStyle BorderStyle="None"></EmptyDataRowStyle>

                        <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
            <HeaderStyle CssClass="grdheadermster" BackColor="#266AAA" Font-Bold="True" ForeColor="White"></HeaderStyle>
                        <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
            <RowStyle Height="15px" BorderColor="Black" BackColor="White" />
                        <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                        <SortedAscendingCellStyle BackColor="#F5F7FB" />
                        <SortedAscendingHeaderStyle BackColor="#6D95E1" />
                        <SortedDescendingCellStyle BackColor="#E9EBEF" />
                        <SortedDescendingHeaderStyle BackColor="#4870BE" />
        </asp:GridView>

          
     

              
        
            













 <asp:UpdatePanel ID="updorddet" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <% 
                    HelperServices objHelperServices = new HelperServices();
                    HelperDB objHelperDB = new HelperDB();
                   
                    OrderServices objOrderServices = new OrderServices();
                    ProductServices objProductServices = new ProductServices();
                    Security objSecurity = new Security();
                    //ProductFamily oProdFam = new ProductFamily();
                    DataSet dsOItem = new DataSet();
                    int OrderID = 0;
                    int Userid;
                    bool UserCheckout=false;
                    int ProductId;
                    string OrdStatus = "";
                    int OpenOrdStatusID = (int)OrderServices.OrderStatus.OPEN;

                    Userid = objHelperServices.CI(Session["USER_ID"]);

                    if ((Session["ORDER_ID"] != null && Convert.ToInt32(Session["ORDER_ID"]) > 0) || (Request.QueryString["ViewOrder"] != null && Request.QueryString["ViewOrder"].ToString().Equals("View")))
                    {
                        OrderID = Convert.ToInt32(Session["ORDER_ID"]);
                    }
                    else
                    {
                        OrderID = objOrderServices.GetOrderID(Userid, OpenOrdStatusID);
                    }

                    OrdStatus = objOrderServices.GetOrderStatus(OrderID);
                    ProductId = objHelperServices.CI(Request.QueryString["Pid"]);

                    decimal subtot = 0.00M;
                    decimal taxamt = 0.00M;
                    decimal Total = 0.00M;

                    string SelProductId = "";
                %>
               <table width="970" id="BaseTable0" border="0" cellpadding="0" cellspacing="0" style="padding-left:0px;" >
                     
                                                    

                                        <%
                                            int DupProdCount = 0;
                                            string LeaveDuplicateProds = "";
                                            if (1== 1)
                                            {
                                                if (Request["rma"] != null)
                                                {
                                                    string _rma = Request["rma"].ToString();
                                                    string _rmitem = Request["Item"].ToString();
                                                    string _rmqty = "";
                                                    double CalItem_ID = 0;
                                                    if (Request.QueryString["DelQty"] != null)
                                                    {
                                                        _rmqty = Request["DelQty"].ToString();
                                                    }
                                                    if (Request.QueryString["cla_id"] != null)
                                                    {
                                                        CalItem_ID = Convert.ToDouble(Request["cla_id"].ToString());
                                                    }
                                                    if (_rma == "NF")
                                                    {
                                                        //Session["ITEM_ERROR"] = Session["ITEM_ERROR"].ToString().Replace(_rmitem + ",", "");
                                                        objOrderServices.Remove_Clarification_item(CalItem_ID);  
                                                    }
                                                    if (_rma == "CI")
                                                    {
                                                        //Session["ITEM_CHK"] = Session["ITEM_CHK"].ToString().Replace(_rmitem + ",", "");
                                                        //Session["QTY_CHK"] = Session["QTY_CHK"].ToString().Replace(_rmqty + ",", "");
                                                        objOrderServices.Remove_Clarification_item(CalItem_ID); 
                                                    }
                                                }
                                                
                                               //if (Session["LeaveDuplicateProds"] != null && Session["LeaveDuplicateProds"].ToString() != "")
                                               //{
                                               //    LeaveDuplicateProds = Session["LeaveDuplicateProds"].ToString();
                                               //    LeaveDuplicateProds = LeaveDuplicateProds.Replace("-", "");
                                               //    if (LeaveDuplicateProds.StartsWith(",") == true)
                                               //        LeaveDuplicateProds = LeaveDuplicateProds.Substring(1);

                                               //    if (LeaveDuplicateProds.EndsWith(",") == true)
                                               //        LeaveDuplicateProds = LeaveDuplicateProds.Substring(0, LeaveDuplicateProds.Length- 1); 
                                                    
                                               //}
                                               LeaveDuplicateProds = GetLeaveDuplicateProducts();

                                               DataSet dsDuplicateItem = objOrderServices.GetOrderItemsWithDuplicate(OrderID, LeaveDuplicateProds);
                                               DataTable tbErrorItem = objOrderServices.GetOrder_Clarification_Items(OrderID, "ITEM_ERROR");
                                               DataTable tbErrorChk = objOrderServices.GetOrder_Clarification_Items(OrderID, "ITEM_CHK");
                                               DataTable tbErrorReplace = objOrderServices.GetOrder_Clarification_Items(OrderID, "ITEM_REPLACE");
                                               DataTable tbErrorPromotion = objOrderServices.GetOrder_Clarification_Items(OrderID, "ITEM_PROMOTION");
                                                


                                               DataSet dsDuplicateItem_Prod_id = objOrderServices.GetOrderItemsWithDuplicate_Prod_id(OrderID, LeaveDuplicateProds);
                                                if (dsDuplicateItem_Prod_id != null && dsDuplicateItem_Prod_id.Tables.Count > 0 && dsDuplicateItem_Prod_id.Tables[0].Rows.Count > 0)
                                                {                                                   
                                                    DupProdCount = dsDuplicateItem_Prod_id.Tables[0].Rows.Count;
                                                }
                                                
                                                //if (Session["ITEM_ERROR"] != null && (Session["ITEM_ERROR"].ToString().Replace(",", "") != "" || Session["ITEM_CHK"].ToString() != "") ||( DupProdCount>0))
                                                if ((tbErrorItem != null && tbErrorItem.Rows.Count > 0) || (tbErrorChk != null && tbErrorChk.Rows.Count > 0) || (DupProdCount > 0) || (tbErrorReplace != null && tbErrorReplace.Rows.Count > 0) || (tbErrorPromotion != null && tbErrorPromotion.Rows.Count > 0))                                                    
                                                {%>
                                                <tr>
                        <td align="left" colspan="2" width="100%">
                          <div class="quickorder3">        
                            <H3 class="title_pink">Order Clarification/Errors</H3>
                            <table id="SiteMapTable0"  class="orderdettable">
                                  <tr>
                                    <td align="left" colspan="4" bgColor="#cccccc">
                                        <b>Please Check Below</b>
                                    </td>
                                </tr>
                                 <tr>
                                        <td  align="left" bgcolor="#f2f2f2"   >
                                            ITEMCODE
                                        </td>
                                        <td align="left" bgcolor="#f2f2f2" >
                                            CLARIFICATION REQUIRED
                                        </td>
                                        <td colspan="2" bgcolor="#f2f2f2" >
                                            &nbsp;
                                        </td>
                                    </tr>  
                                   
                                   
                                                         <% string TempNotFoundItem = "";
                                                           string _NotFoundItem = "";
                                                           string _ClaItem_ID = "0";
                                                           if (tbErrorItem != null && tbErrorItem.Rows.Count>0)
                                                           {

                                                               foreach (DataRow RItem in tbErrorItem.Rows)
                                                               {
                                                                   _NotFoundItem = RItem["PRODUCT_DESC"].ToString();
                                                                   _ClaItem_ID = RItem["CLARIFICATION_ID"].ToString();
                                                                   if (_NotFoundItem.Trim() != "")
                                                                   {
                                                                       if (_NotFoundItem.Trim() != TempNotFoundItem.Trim())
                                                                       {
                                                                %>
                                                                <tr>
                                                                 <td align="left" >
                                                                    <%=_NotFoundItem%>
                                                                </td>
                                                                <td align="left">
                                                                    <font color="red" style="font-weight: bold;">Not Found / Incorrect Code</font>
                                                                
                                                                </td>
                                                                <td align="left" >
                                                                    <a href="#bulkorder" style="font-weight: bold; text-decoration: none; color: #1589FF;">
                                                                        Please Re-Enter Below</a>
                                                                </td>
                                                                <td align="left">
                                                                    <a href="Orderdetails.aspx?bulkorder=1&amp;rma=NF&amp;item=<%=_NotFoundItem%>&amp;cla_id=<%=_ClaItem_ID%>" style="font-weight: bold;
                                                                        text-decoration: none; color: #1589FF;">Delete Item</a>
                                                                </td>
                                                                </tr>
                                                                       <%
                                                                        }
                                                                       
                                                                    }
                                                                       TempNotFoundItem = _NotFoundItem;
                                                                       }
                                                                   }
                                                           
                                                                
                                                           string TempreplaceItem = "";
                                                           string _replaceItem = "";
                                                           int _ordQty = 1;
                                                           string EA_root_Cat_path1 = System.Configuration.ConfigurationManager.AppSettings["EA_ROOT_CATEGORY_PATH"].ToString();
                                                           string EA_New_Product_init_cat_path1 = System.Configuration.ConfigurationManager.AppSettings["EA_NEW_PRODUCT_INIT_CATEGORY_PATH"].ToString();
                                                           string WesCatalogId = System.Configuration.ConfigurationManager.AppSettings["WES_CATALOG_ID"].ToString();

                                                           string Ea_Path = "";

                                                           string pfid = "";
                                                           string _catid = "";
                                                    
                                                           if (tbErrorReplace != null && tbErrorReplace.Rows.Count > 0)
                                                           {

                                                               foreach (DataRow RItem in tbErrorReplace.Rows)
                                                               {
                                                                   _replaceItem = RItem["PRODUCT_DESC"].ToString();
                                                                   _ClaItem_ID = RItem["CLARIFICATION_ID"].ToString();
                                                                   _ordQty= (RItem["QTY"].ToString()!="" && Convert.ToInt32(RItem["QTY"].ToString())>0) ? Convert.ToInt32(RItem["QTY"].ToString()):1;
                                                                   
                                                                   if (_replaceItem.Trim() != "")
                                                                   {
                                                                       if (_replaceItem.Trim() != TempreplaceItem.Trim())
                                                                       {
                                                                           
                                                                           DataTable substituteproduct = new DataTable();
                                                                           DataTable rtntbl = new DataTable();
                                                                           DataTable wag_product_code_substituteproduct = new DataTable();
                                                                           bool samecodesubproduct= false;
                                                                           bool samecodenotFound = false;
                                                                           string wag_product_code = "";
                                                                           string SubstuyutePid = "";
                                                                           int checksetsub = -1;
                                                                           string product_status = "";
                                                                           int StrProductStatusSub = -1;
                                                                           string StrStockStatus = "";
                                                                           int ProdCodeId = 0;
                                                                            
                                                                           rtntbl = objProductServices.GetSubstituteProductDetails(_replaceItem, Userid);
                                                                           if (rtntbl != null && rtntbl.Rows.Count > 0)
                                                                           {

                                                                               _catid = rtntbl.Rows[0]["CatId"].ToString();
                                                                               pfid=rtntbl.Rows[0]["Pfid"].ToString()  ;
                                                                               Ea_Path=rtntbl.Rows[0]["Ea_Path"].ToString()  ;
                                                                               samecodesubproduct= (bool)rtntbl.Rows[0]["samecodesubproduct"] ;
                                                                               samecodenotFound = (bool)rtntbl.Rows[0]["samecodenotFound"];
                                                                               wag_product_code = rtntbl.Rows[0]["wag_product_code"].ToString();
                                                                               SubstuyutePid = rtntbl.Rows[0]["SubstuyutePid"].ToString();
                                                                           }
                                                                           else
                                                                           {
                                                                               samecodesubproduct = true;
                                                                               samecodenotFound = false;
                                                                           }
                                                                           
                                                                           if (samecodenotFound==false && samecodesubproduct == true)
                                                                           {
                                                                               
                                                                               
                                                              %>
                                                        <tr>
                                                            <td style="left" >
                                                                <%=_replaceItem%>
                                                            </td>
                                                            <td align="left">
                                                                <%--<font color="red" style="font-weight: bold;">Not Found / Incorrect Code</font>--%>
                                                                <font color="orange" style="font-weight: bold;">Product Temporarily Unavailable.&nbsp;Please Contact Us for more details</font>
                                                            </td>
                                                          
                                                            <td>
                                                                <a href="Orderdetails.aspx?bulkorder=1&amp;rma=NF&amp;item=<%=_replaceItem%>&amp;cla_id=<%=_ClaItem_ID%>" style="font-weight: bold;
                                                                    text-decoration: none; color: #1589FF;">Delete Item</a>
                                                            </td>
                                                        </tr>
                                                        <%
                                                                    }
                                                                           else if (samecodenotFound == false && samecodesubproduct == false)
                                                                           {
                                                                               %>
                                                         <tr>
                                                            <td style="left" >
                                                                <%=_replaceItem%>
                                                            </td>
                                                            <td>
                                                                <font color="orange" style="font-weight: bold;">Product Not Available.</font>
                                                               <font color="#009900" style="font-weight: bold;">Replaced With:&nbsp;<%=wag_product_code%></font></td>
                                                            <td align="center">
                                                                <table>
                                                                 <tbody>
                                                                  <tr>
                                                                    <td>
                                                                   
                                                                     <a href ="ProductDetails.aspx?Pid=<%=SubstuyutePid.Trim()%>&amp;fid=<%=pfid%>&amp;Cid=<%=_catid%>&amp;path=<%=Ea_Path%> " style="font-weight: bold; text-decoration: none; color: #1589FF;" >
                                                                     View Item
                                                                     </a>
                                                                    </td>
                                                                           <td align="left">
                                                                <a href="Orderdetails.aspx?bulkorder=1&amp;rma=NF&amp;item=<%=_replaceItem%>&amp;cla_id=<%=_ClaItem_ID%>" style="font-weight: bold;
                                                                    text-decoration: none; color: #1589FF;">Delete Item</a>
                                                            </td>
                                                                     
                                                                  </tr>
                                                                 </tbody>
                                                               </table>

                                                            </td>
                                                            <td>
                                                                 <a href="OrderDetails.aspx?bulkorder=1&amp;flgAddItem=chkAddItem&amp;rma=NF&amp;ORDER_ID=<%=OrderID%>&amp;Pid=<%=SubstuyutePid.Trim() %>&amp;Qty=<%=_ordQty %>&amp;item=<%=_replaceItem%>&amp;cla_id=<%=_ClaItem_ID%>" style=" text-align: center;height: 29px;width: 68px;text-decoration: none;line-height:28px" class="button normalsiz btnblue fleft">
                                                                      Add Item
                                                                      </a>
                                                            </td>
                                                        </tr>
                                                                         <%
                                                                    }
                                                                           else
                                                                           {%>
                                                                            <tr>
                                                                                <td style="left" >
                                                                                    <%=_replaceItem%>
                                                                                </td>
                                                                                <td style="left" >
                                                                                    <font color="red" style="font-weight: bold;">Not Found / Incorrect Code</font>
                                                                                </td>
                                                                                <td>
                                                                                    <a href="#bulkorder" style="font-weight: bold; text-decoration: none; color: #1589FF;">
                                                                                        Please Re-Enter Below</a>
                                                                                </td>
                                                                                <td>
                                                                                    <a href="Orderdetails.aspx?bulkorder=1&amp;rma=NF&amp;item=<%=_replaceItem%>&amp;cla_id=<%=_ClaItem_ID%>" style="font-weight: bold;
                                                                                        text-decoration: none; color: #1589FF;">Delete Item</a>
                                                                                </td>
                                                                            </tr>

                                                                               <%
                                                                    }



                                                                       }
                                                                       TempreplaceItem = _replaceItem;
                                                                   }
                                                               }
                                                           }


                                                           string TempClarifyItem = "";
                                                           string _ClarifyItem = "";
                                                           Int32 _orderQty = 0;

                                                           if (tbErrorChk != null && tbErrorChk.Rows.Count > 0)
                                                           {
                                                               foreach (DataRow RItem in tbErrorChk.Rows)
                                                               {
                                                                   _ClarifyItem = RItem["PRODUCT_DESC"].ToString();
                                                                   _orderQty = Convert.ToInt32(RItem["QTY"].ToString());
                                                                   _ClaItem_ID = RItem["CLARIFICATION_ID"].ToString();
                                                                   if (_ClarifyItem.Trim() != "")
                                                                   {
                                                                       if (_ClarifyItem.Trim() != TempClarifyItem.Trim())
                                                                       {      
                                                        %>
                                                        <tr>
                                                            <td align="left">
                                                                <% =_ClarifyItem%>
                                                            </td>
                                                            <td align="left">
                                                                <font color="#ff9900" style="font-weight: bold">Not unique Code</font>
                                                            </td>
                                                            <td align="left">
                                                                <a class="thickbox" href="SubProducts.aspx?Item=<%=_ClarifyItem %>&amp;height=400&amp;width=600&amp;modal=true&amp;OrderID=<%=OrderID%>&amp;ClrQty=<%=_orderQty%>&amp;cla_id=<%=_ClaItem_ID%>"
                                                                     style="font-weight: bold; text-decoration: none; color: #1589FF;">Clarify Now</a>
                                                            </td>
                                                            <td align="left">
                                                                <a href="OrderDetails.aspx?bulkorder=1&amp;rma=CI&amp;item=<%= _ClarifyItem %>&amp;DelQty=<%=_orderQty%>&amp;cla_id=<%=_ClaItem_ID%> "
                                                                    style="font-weight: bold; text-decoration: none; color: #1589FF;">Delete Item</a>
                                                            </td>
                                                        </tr>
                                                        <%  
                                                                    }

                                                                       TempClarifyItem = _ClarifyItem;

                                                                   }
                                                               }
                                                           }
                                                           
                                                           if (tbErrorPromotion != null && tbErrorPromotion.Rows.Count > 0)
                                                           {
                                                               foreach (DataRow RItem in tbErrorPromotion.Rows)
                                                               {
                                                                   _ClarifyItem = RItem["PRODUCT_DESC"].ToString();
                                                                   _orderQty = Convert.ToInt32(RItem["QTY"].ToString());
                                                                   _ClaItem_ID = RItem["CLARIFICATION_ID"].ToString();
                                                                   if (_ClarifyItem.Trim() != "")
                                                                   {
                                                                       if (_ClarifyItem.Trim() != TempClarifyItem.Trim())
                                                                       {      
                                                        %>
                                                        <tr>
                                                            <td align="left">
                                                                <% =_ClarifyItem%>
                                                            </td>
                                                            <td align="left">
                                                                <font color="#ff9900" style="font-weight: bold">Not a Promotion Product&nbsp;, Please Contact Us for more details</font>
                                                            </td>
                                                          
                                                            <td align="left">
                                                                <a href="OrderDetails.aspx?bulkorder=1&amp;rma=CI&amp;item=<%= _ClarifyItem %>&amp;DelQty=<%=_orderQty%>&amp;cla_id=<%=_ClaItem_ID%> "
                                                                    style="font-weight: bold; text-decoration: none; color: #1589FF;">Delete Item</a>
                                                            </td>
                                                        </tr>
                                                        <%  
                                                                    }

                                                                       TempClarifyItem = _ClarifyItem;

                                                                   }
                                                               }
                                                           }
                                                           string dupItem = "";
                                                           int pid1;
                                                           int maxqty1;
                                                           int minQty1;
                                                           int FId1 = 0;
                                                           double orderItemId = 0;
                                                           decimal ProductUnitPrice1;
                                                           decimal ProdTotal1;
                                                           int Qty1;
                                                           int rowcnt1 = 0;

                                                           DataTable temptbl = null;
                                                           if (dsDuplicateItem_Prod_id != null && dsDuplicateItem_Prod_id.Tables.Count > 0 && dsDuplicateItem_Prod_id.Tables[0].Rows.Count > 0)
                                                           {
                                                               foreach (DataRow drItem in dsDuplicateItem_Prod_id.Tables[0].Rows)
                                                               {
                                                                   DataRow[] Dr = dsDuplicateItem.Tables[0].Select("PRODUCT_ID='" + drItem["PRODUCT_ID"].ToString() + "'");
                                                                   if (Dr.Length > 0)
                                                                   {
                                                                       temptbl = Dr.CopyToDataTable();
                                                                       if (temptbl != null && temptbl.Rows.Count > 0)
                                                                       { %>
                                                                          <tr>
                                                                            <td >
                                                                                <% =temptbl.Rows[0]["CATALOG_ITEM_NO"].ToString().ToUpper()%>
                                                                            </td>
                                                                            <td>
                                                                                <font color="#ff9900" style="font-weight: bold">Duplicate/Multiple Product Entries Found</font>
                                                                           
                                                                            <table border="0px" cellpadding="0" cellspacing="0" style="padding-top:0px;padding-bottom:0px;"   >
                                                                             <tr>                                                                           
                                                                                    <td width="100">
                                                                                        
                                                                                          <strong> Order Code </strong>
                                                                                        
                                                                                    </td>
                                                                                     <td width="100" align ="right" >
                                                                                     
                                                                                     <strong>  QTY</strong>
                                                                                     
                                                                                    </td>  
                                                                                     <td width="100" align="center"  >
                                                                                     <font  style="font-weight: bold" >
                                                                                    
                                                                                       </font>
                                                                                    </td>                                                                             
                                                                                  </tr>
                                                                            
                                                                            
                                                                           
                                                                        <%
                                                                    pid1 = objHelperServices.CI(drItem["PRODUCT_ID"].ToString());
                                                                    foreach (DataRow tmpItem in temptbl.Rows)
                                                                    {

                                                                        ProductUnitPrice1 = objHelperServices.CDEC(tmpItem["PRICE_EXT_APPLIED"].ToString());
                                                                        ProductUnitPrice1 = objHelperServices.CDEC(ProductUnitPrice1.ToString("N2"));
                                                                        Qty1 = objHelperServices.CI(tmpItem["QTY"].ToString());
                                                                        ProdTotal1 = Qty1 * ProductUnitPrice1;
                                                                        orderItemId = Convert.ToDouble(tmpItem["ORDER_ITEM_ID"].ToString());
                                                                                %>
                                                                                 <tr>                                                                           
                                                                                    <td width="100" align ="left" >
                                                                                        <font  style="font-weight: bold">
                                                                                           <% =tmpItem["CATALOG_ITEM_NO"].ToString().ToUpper()%>
                                                                                        </font>
                                                                                    </td>
                                                                                    <td width="100" align ="right" >
                                                                                        <font  style="font-weight: bold;font-size:12px;">
                                                                                           <% =tmpItem["QTY"].ToString().ToUpper()%>
                                                                                        </font>
                                                                                    </td>
                                                                                      <td width="100" align="center">
                                                                                        <a href="byproduct.aspx?&bulkorder=1&SelPid=<%=pid1 %>&amp;SelProdPrice=<%=ProdTotal1 %>&amp;ProdPrice=<%=ProductUnitPrice1 %>&amp;ORDER_ID=<%=OrderID %>&amp;ORDER_ITEM_ID=<%=orderItemId %> "style="color: #0099ff">Remove</a>
                                                                                    </td>                                                                            
                                                                                  </tr>
                                                                                <%
                                                                    }
                                                                            %>

                                                                            <tr>
                                                                            
                                                                            <td colspan="3" align="left"  >
                                                                                <a href="byproduct.aspx?&bulkorder=1&amp;ORDER_ID=<%=OrderID %>&amp;CombainProd_id=<%=pid1 %>"style="color: #0099ff">Combine all into one (QTY discount may apply)</a>                                                                                
                                                                                <br />
                                                                                 <a href="byproduct.aspx?&bulkorder=1&amp;ORDER_ID=<%=OrderID %>&amp;LeaveProd_id=<%=pid1 %>"style="color: #0099ff">Leave as multiple lines</a>
                                                                            </td>
                                                                            
                                                                            </tr>
                                                                             </table>
                                                                               
                                                                            </td>
                                                                            <td>
                                                                                
                                                                            </td>
                                                                            <td>
                                                                                
                                                                            </td>
                                                                            </tr>
                                                                            


                                                                            <%
                                                                                
                                                                    }
                                                                   }

                                                               }

                                                           }                                                                                                                                         
                                                        %>
                                             
                                                   </table>    
                            </div>
                            <br />
                        </td>
                    </tr>
                                        <%   }
                                            }
                                            else
                                            {
                                                Session["ITEM_ERROR"] = "";
                                                Session["ITEM_CHK"] = "";
                                                Session["QTY_CHK"] = "";
                                            }
          
                                        %>
                              
                    <tr>
                        <td align="left" colspan="2"  valign="top" style="background-color: #F2F2F2; border: thin solid #E7E7E7">
                           <div class="quickorder3">        
                            <H3 class="upload_title1">Shopping cart contents</H3>
                            <asp:Label ID="Label1" runat="server" Text="Order No : " Visible="false"></asp:Label>
                             <asp:Label ID="lblOrdNo" runat="server" Visible="false"></asp:Label>
                            <table border="0px" cellpadding="0" cellspacing="0" class="orderdettable">
                             
                             
                                <tr>
                                    <td align="left" colspan="7" bgColor="#cccccc">
                                        <b>Order Contents</b>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left" bgcolor="#f2f2f2">
                                        <b>Order Code</b>
                                    </td>
                                    <td align="left"  bgcolor="#f2f2f2" colspan="2" >
                                        <b>Quantity</b>
                                    </td>
                                    <td align="left"  bgcolor="#f2f2f2">
                                        <b>Description</b>
                                    </td>
    
                                    <td align="left" bgcolor="#f2f2f2">
                                        <b>Cost (Ex. GST)</b>
                                    </td>
                                    <td align="left" bgcolor="#f2f2f2">
                                        <b>Extension Amount (Ex. GST)</b>
                                    </td>
                                </tr>
                                <%   	   
                                                                    string EA_root_Cat_path = System.Configuration.ConfigurationManager.AppSettings["EA_ROOT_CATEGORY_PATH"].ToString();
                                                                    string EA_New_Product_init_cat_path = System.Configuration.ConfigurationManager.AppSettings["EA_NEW_PRODUCT_INIT_CATEGORY_PATH"].ToString();



                                                                    //dsOItem = objOrderServices.GetOrderItems(OrderID);
                                                                    dsOItem = objOrderServices.GetOrderItemsWithoutDuplicate(OrderID, LeaveDuplicateProds);
                                                                    string cSymbol = objHelperServices.GetOptionValues("CURRENCYFORMAT").ToString();
                                                                    decimal ProdShippCost = 0.00M;
                                                                    decimal TotalShipCost = 0.00M;
                                                                    decimal TotalTaxAmount = 0.00M;
                                                                    SelProductId = "";
                                                                    if (OrdStatus == OrderServices.OrderStatus.OPEN.ToString() || OrdStatus == "CAU_PENDING")
                                                                    {
                                                                        if (dsOItem != null)
                                                                        {
                                                                            int i = 0;
                                                                            foreach (DataRow rItem in dsOItem.Tables[0].Rows)
                                                                            {
                                                                                decimal ProductUnitPrice;
                                                                                int pid;
                                                                                int maxqty;
                                                                                int minQty;
                                                                                int FId = 0;
                                                                                double OrderItemId1 = 0;
                                                                                string sty = "style=\"border-style: none solid none none; border-width: thin; border-color: #E7E7E7\" ";
                                                                                string styl = "style=\"border-style: none none none none; border-width: thin; border-color: #E7E7E7\" ";
                                                                                if (rItem["PRODUCT_ID"].ToString() == dsOItem.Tables[0].Rows[dsOItem.Tables[0].Rows.Count - 1]["PRODUCT_ID"].ToString())
                                                                                {
                                                                                    sty = "style=\"border-style: none solid none none; border-width: thin; border-color: #E7E7E7\" ";
                                                                                    styl = "style=\"border-style: none none none none; border-width: thin; border-color: #E7E7E7\" ";
                                                                                }
                                                                                pid = objHelperServices.CI(rItem["PRODUCT_ID"].ToString());
                                                                                OrderItemId1 = objHelperServices.CD(rItem["ORDER_ITEM_ID"].ToString());
                                                                                //FId = oHelper.CI(rItem["FAMILY_ID"].ToString()); 
                                                                                FId = objProductServices.GetFamilyID(pid);
                                                                                int pQty = objOrderServices.GetOrderItemQty(pid, OrderID, OrderItemId1);
                                                                                maxqty = objHelperServices.CI(rItem["QTY_AVAIL"].ToString());
                                                                                maxqty = maxqty + objHelperServices.CI(Request.Form["txtQty"] + pQty);
                                                                                minQty = objHelperServices.CI(rItem["MIN_ORD_QTY"].ToString());
                                                                                ProductUnitPrice = objHelperServices.CDEC(rItem["PRICE_EXT_APPLIED"].ToString());
                                                                                ProductUnitPrice = objHelperServices.CDEC(ProductUnitPrice.ToString("N4"));

                                                                                //ProdShippCost = CalculateShippingCost(OrderID, pid, ProductUnitPrice, pQty);
                                                                                //TotalShipCost = oHelper.CDEC(TotalShipCost + ProdShippCost); 
                                                                                int Qty = objHelperServices.CI(rItem["QTY"].ToString());
                                                                                decimal ProdTotal =Math.Round(Qty * ProductUnitPrice, 2,MidpointRounding.AwayFromZero) ;
                                                                                //decimal ProdTotal = Qty * ProductUnitPrice;
                                                                                subtot = subtot + ProdTotal;
                                                                                TotalTaxAmount = TotalTaxAmount + objHelperServices.CDEC(rItem["TAX_AMOUNT"].ToString());
                                                                                string Desc = rItem["DESCRIPTION"].ToString().Replace("<ars>g</ars>", "&rarr;").Replace("<sps>l</sps>", "<font face=\"Symbol\">l</font>");
                                                                                string Available = rItem["PRODUCT_STATUS"].ToString();

                                                                                string WesCatalogId = System.Configuration.ConfigurationManager.AppSettings["WES_CATALOG_ID"].ToString();

                                                                                string Ea_Path = "";
                                                                                string pfid = "";
                                                                                string _catid = "";
                                                                                DataSet tmpds = (DataSet)objHelperDB.GetGenericDataDB(WesCatalogId, pid.ToString(), "GET_PARENT_CATEGORY_ID_PATH_INPUT_PID", HelperDB.ReturnType.RTDataSet);
                                                                                if (tmpds != null && tmpds.Tables.Count > 0)
                                                                                {
                                                                                    _catid = tmpds.Tables[0].Rows[0]["CATEGORY_ID"].ToString();
                                                                                    pfid = tmpds.Tables[0].Rows[0]["PARENT_FAMILY_ID"].ToString();                                                                                   
                                                                                    Ea_Path = EA_New_Product_init_cat_path + "////" + tmpds.Tables[0].Rows[0]["CATEGORY_PATH"].ToString() + "////UserSearch1=Family Id=" + tmpds.Tables[0].Rows[0]["PARENT_FAMILY_ID"].ToString();
                                                                                    Ea_Path = HttpUtility.UrlEncode(objSecurity.StringEnCrypt(Ea_Path));
                                                                                }

                                                                                // int FId=oProd.getpr
                                                                                if (Request["SelAll"] != "1")
                                                                                {
                                                                                    SelProductId = "";
                                                                                    Session["SelProduct"] = null;
                                                                                    CheckBox chk = new CheckBox();
                                %>
                                <tr>
                                    <td  align="left" >
                                        <%="<a href =ProductDetails.aspx?&Pid=" + pid + "&fid=" + pfid + "&Cid=" + _catid + "&path=" + Ea_Path + "  class=\"toplinkatest\">" + rItem["CATALOG_ITEM_NO"].ToString() + " </a>"%>
                                    </td>
                                    <td align="left" colspan="2" >
                                        <!--onBlur=\"javascript:document.getElementById('updCart').click();\" -->
                                        <%="<input type =\"Text\" Id=\"txtQtyId" + OrderItemId1 + "_" + maxqty + "\" Name =\"txtQty" + OrderItemId1 + "\" size=\"4\"  onkeydown=\"return keyct(event)\"  maxlength=\"6\" style=\"background-color:White;height:14px;width:20px;padding:1px;border:#E7E7E7 1px solid;font-size:12px;\" value =\"" + Qty + "\">"%>
                                        &nbsp;&nbsp;<a href="OrderDetails.aspx?&bulkorder=1&SelPid=<%=pid %>&amp;SelProdPrice=<%=ProdTotal %>&amp;ProdPrice=<%=ProductUnitPrice %>&amp;ORDER_ID=<%=OrderID %>&amp;ORDER_ITEM_ID=<%=OrderItemId1 %>" class="toplinkatest">Remove</a>&nbsp;&nbsp;
                                    </td>
                                    <td < align="left" >
                                        <%=Desc%>
                                    </td>
                                   
                                    <td align="left" >
                                        <%=cSymbol + " " + objHelperServices.CheckPriceValueDecimal(ProductUnitPrice.ToString("#,#0.0000"))%>
                                    </td>
                                    <td align="left" >
                                        <%=cSymbol + " " + ProdTotal.ToString("#,#0.00")%>
                                    </td>
                                    <%="<input type=\"hidden\" Name=\"txtPid" + OrderItemId1 + "\" runat=\"server\" value=\"" + pid + "\">"%>
                                    <%="<input type=\"hidden\" Name=\"txtCatItem" + OrderItemId1 + "\" runat=\"server\" value=\"" + rItem["CATALOG_ITEM_NO"].ToString() + "\">"%>
                                    <%="<input type=\"hidden\" Name=\"txtMaxQty" + OrderItemId1 + "\" runat=\"server\" value=\"" + maxqty + "\">"%>
                                    <%="<input type=\"hidden\" Name=\"txtMinQty" + OrderItemId1 + "\" runat=\"server\" value=\"" + minQty + "\">"%>
                                    <%="<input type=\"hidden\" Name=\"txtUntPrice" + OrderItemId1 + "\" runat=\"server\" value=\"" + ProductUnitPrice.ToString("#,#0.00") + "\">"%>
                                    <%="<input type=\"hidden\" Name=\"txtPrdTprice" + OrderItemId1 + "\" runat=\"server\" value=\"" + ProdTotal.ToString("#,#0.00") + "\">"%>
                                </tr>
                                <%  
                                                                    i = i + 1;
                                                                                }
                                                                                else if (Request["SelAll"] == "0")
                                                                                {
                                                                                    SelProductId = "";
                                                                                    Session["SelProduct"] = null;
                                %>
                                <tr>
                                    <td align="center" bgcolor="White" class="style24" style="border-style: none none none solid;
                                        border-width: thin; border-color: #E7E7E7">
                                        <%="<input type =\"CheckBox\" style=\"border-style:none;\" Name =\"Chk" + OrderItemId1 + "\" value =" + pid + "\"  onclick =\"javascript:Click(" + pid + "," + Qty + "," + OrderItemId1 + ");\">"%>
                                    </td>
                                    <td align="left" bgcolor="White" class="style19" style="border-style: none none none solid;
                                        border-width: thin; border-color: #E7E7E7">
                                        <%="<a href =ProductFeatures.aspx?Fid=" + FId + "&Pid" + pid + "&Min=" + minQty + "&Max" + maxqty + ");>" + rItem["CATALOG_ITEM_NO"].ToString() + "</a>"%>
                                    </td>
                                    <td align="left" bgcolor="White" class="Numeric" style="border-style: none none none solid;
                                        border-width: thin; border-color: #E7E7E7">
                                        <%="<input type =\"Text\" Id=\"txtQtyId" + OrderItemId1 + "_" + maxqty + "\" Name =\"txtQty" + OrderItemId1 + "\" size=\"7\"  runat =\"server\" onBlur=\"javascript:Check(" + OrderItemId1 + "," + maxqty + ");\" value =\"" + Qty + "\">"%>
                                    </td>
                                    <td bgcolor="White" class="style21" style="border-style: none none none solid; border-width: thin;
                                        border-color: #E7E7E7">
                                        <%=Desc%>
                                    </td>
                                    <td bgcolor="White" class="style20" style="border-style: none none none solid; border-width: thin;
                                        border-color: #E7E7E7">
                                        <%=Available%>
                                    </td>
                                    <td align="center" bgcolor="White" class="style23" style="width: 130px" style="border-style: none none none solid;
                                        border-width: thin; border-color: #E7E7E7">
                                        <%=cSymbol + " " + objHelperServices.CheckPriceValueDecimal(ProductUnitPrice.ToString("#,#0.00"))%>
                                    </td>
                                    <%--								                                <td class="NumericField" align="center"><%Response.Write(cSymbol + ProdShippCost.ToString("#,#0.00")); %></td>
                                    --%>
                                    <td align="center" bgcolor="White" class="NumericField" style="border-style: none solid none solid;
                                        border-width: thin; border-color: #E7E7E7" width="20%">
                                        <%=cSymbol + " " + ProdTotal.ToString("#,#0.00")%>
                                    </td>
                                    <%="<input type=\"hidden\" Name=\"txtPid" + OrderItemId1 + "\" runat=\"server\" value=\"" + pid + "\">"%>
                                    <%="<input type=\"hidden\" Name=\"txtCatItem" + OrderItemId1 + "\" runat=\"server\" value=\"" + rItem["CATALOG_ITEM_NO"].ToString() + "\">"%>
                                    <%="<input type=\"hidden\" Name=\"txtMaxQty" + OrderItemId1 + "\" runat=\"server\" value=\"" + maxqty + "\">"%>
                                    <%="<input type=\"hidden\" Name=\"txtsPrdId" + OrderItemId1 + "\" runat=\"server\" value=\"" + pid + "\">"%>
                                    <%="<input type=\"hidden\" Name=\"txtUntPrice" + OrderItemId1 + "\" runat=\"server\" value=\"" + ProductUnitPrice.ToString("#,#0.00") + "\">"%>
                                    <%="<input type=\"hidden\" Name=\"txtPrdTprice" + OrderItemId1 + "\" runat=\"server\" value=\"" + ProdTotal.ToString("#,#0.00") + "\">"%>
                                </tr>
                                <%
                                                                    i = i + 1;
                                                                                }
                                                                                else
                                                                                { 
                                %>
                                <tr>
                                    <td align="center" bgcolor="White" class="style24" style="border-style: none none none solid;
                                        border-width: thin; border-color: #E7E7E7">
                                        <%="<input type =\"CheckBox\" style=\"border-style:none;\" Name =\"Chk" + OrderItemId1 + "\" value =" + pid + "\" checked=\"checked\" onclick =\"javascript:Click(" + pid + "," + Qty + "," + OrderItemId1 + ");\">"%>
                                    </td>
                                    <td align="left" bgcolor="White" class="style19" style="border-style: none none none solid;
                                        border-width: thin; border-color: #E7E7E7">
                                        <%="<a href =ProductFeatures.aspx?Fid=" + FId + "&Pid=" + pid + "&Min=" + minQty + "&Max=" + maxqty + ");>" + rItem["CATALOG_ITEM_NO"].ToString() + "</a>"%>
                                    </td>
                                    <td align="left" bgcolor="White" class="Numeric" style="border-style: none none none solid;
                                        border-width: thin; border-color: #E7E7E7">
                                        <%="<input type =\"Text\" Id=\"txtQtyId" + OrderItemId1 + "_" + maxqty + "\" Name =\"txtQty" + OrderItemId1 + "\" size=\"7\"   runat =\"server\" onBlur=\"javascript:Check(" + OrderItemId1 + "," + maxqty + ");\" value =\"" + Qty + "\">"%>
                                    </td>
                                    <td bgcolor="White" class="style21" style="border-style: none none none solid; border-width: thin;
                                        border-color: #E7E7E7">
                                        <%=Desc%>
                                    </td>
                                    <td bgcolor="White" class="style20" style="border-style: none none none solid; border-width: thin;
                                        border-color: #E7E7E7">
                                        <%=Available%>
                                    </td>
                                    <td align="center" bgcolor="White" class="style23" style="width: 130px" style="border-style: none none none solid;
                                        border-width: thin; border-color: #E7E7E7">
                                        <%=cSymbol + " " + objHelperServices.CheckPriceValueDecimal(ProductUnitPrice.ToString("#,#0.00"))%>
                                    </td>
                                    <%--								                                <td class="NumericField" align="center"><%Response.Write(cSymbol + ProdShippCost.ToString("#,#0.00")); %></td>
                                    --%>
                                    <td align="center" bgcolor="White" class="NumericField" style="border-style: none solid none solid;
                                        border-width: thin; border-color: #E7E7E7">
                                        <%=cSymbol + " " + ProdTotal.ToString("#,#0.00")%>
                                    </td>
                                    <%="<input type=\"hidden\" Name=\"txtPid" + OrderItemId1 + "\" runat=\"server\" value=\"" + pid + "\">"%>
                                    <%="<input type=\"hidden\" Name=\"txtCatItem" + OrderItemId1 + "\" runat=\"server\" value=\"" + rItem["CATALOG_ITEM_NO"].ToString() + "\">"%>
                                    <%="<input type=\"hidden\" Name=\"txtMaxQty" + OrderItemId1 + "\" runat=\"server\" value=\"" + maxqty + "\">"%>
                                    <%="<input type=\"hidden\" Name=\"txtsPrdId" + OrderItemId1 + "\" runat=\"server\" value=\"" + pid + "\">"%>
                                    <%="<input type=\"hidden\" Name=\"txtUntPrice" + OrderItemId1 + "\" runat=\"server\" value=\"" + ProductUnitPrice.ToString("#,#0.00") + "\">"%>
                                    <%="<input type=\"hidden\" Name=\"txtPrdTprice" + OrderItemId1 + "\" runat=\"server\" value=\"" + ProdTotal.ToString("#,#0.00") + "\">"%>
                                </tr>
                                <%   
                                                                    SelProductId = SelProductId + "," + pid;
                                                                    i = i + 1;
                                                                                } //End of SelAll
                                                                            } //End of for each.
                                                                            dsOItem.Dispose();
                                                                        }//End of dataset empty. 
                                                                    } // End Of Order Status Check
                                                                    if (SelProductId != "")
                                                                    {
                                                                        SelProductId = SelProductId.Substring(1, SelProductId.Length - 1);
                                                                        Session["SelProduct"] = SelProductId;
                                                                    }
                                %>
                                <!-- End Up Here-->
                               <%-- <tr>
                                    <td height="2px">
                                    </td>
                                </tr>--%>
                                <tr>
                                    <td colspan="4" rowspan="3">
                                        <asp:ImageButton ID="ImageButton2" runat="server" ClientIDMode="Static" ImageUrl="~/images/spacer.gif"
                                            OnClientClick="javascript:return  setTimeout('SendRemoveProducts()',100);" />
                                    </td>
                                   
                                    <td  colspan="1">
                                        Sub Total
                                    </td>
                                    <td align="left">
                                        <%--<% =objHelperServices.GetOptionValues("CURRENCYFORMAT").ToString() + " " + objHelperServices.CDEC(objHelperServices.FixDecPlace(Math.Round(subtot,2)))  %>--%>
                                        <%=objHelperServices.GetOptionValues("CURRENCYFORMAT").ToString() + " " + objHelperServices.CDEC(objHelperServices.FixDecPlace(subtot))%>
                                    </td>
                                </tr>
                               
                                <tr>
                                    <td  colspan="1" >
                                        Tax Amount(GST)<br />
                                        <span style="font-size: 4"></span>
                                    </td>
                                    <td  align="left">
                                        <%       
                                            
                                                                    //string sSQL = string.Format("SELECT isnull(TAX_AMOUNT,0) [TAX_AMOUNT] FROM TBWC_ORDER WHERE ORDER_ID = {0}", OrderID);
                                                                    //objHelperServices.SQLString = sSQL;
                                                                    //taxamt = System.Convert.ToDecimal(oHelper.GetValue("TAX_AMOUNT"));
                                                                    if (subtot > 0)
                                                                    {
                                                                        //taxamt =objOrderServices.CalculateTaxAmount(subtot, OrderID.ToString()); //Math.Round((subtot * 10 / 100), 2, MidpointRounding.AwayFromZero);
                                                                        taxamt = TotalTaxAmount;
                                                                    }
                                                                    else
                                                                    {
                                                                        taxamt = 0;
                                                                    }
                                                                  Total = Math.Round( subtot,2,MidpointRounding.AwayFromZero) +Math.Round( taxamt,2,MidpointRounding.AwayFromZero) + TotalShipCost;
                                                                    Total = objHelperServices.CDEC(objHelperServices.FixDecPlace(Math.Round(Total,2,MidpointRounding.AwayFromZero)));
                                                                   // Total = subtot + taxamt + TotalShipCost;
                                                                  //  Total = objHelperServices.CDEC(objHelperServices.FixDecPlace(Total));
                                            %>
                                        <%--<%=objHelperServices.GetOptionValues("CURRENCYFORMAT").ToString() + " " + objHelperServices.CDEC(objHelperServices.FixDecPlace(Math.Round( taxamt,2)))%>--%>
                                        <%=objHelperServices.GetOptionValues("CURRENCYFORMAT").ToString() + " " + objHelperServices.CDEC(objHelperServices.FixDecPlace(taxamt))%>
                                    </td>
                                </tr>

                                <tr>
                                    <td colspan="1"  bgColor="#dff0d8">
                                    <%
                                        if (objOrderServices.IsNativeCountry(OrderID) == 0)
                                        {
                                             %>
                                        <strong>Total </strong><br />
                                        <%}
                                        else
                                        {
                                            %>
                                            <strong>Total Inc GST</strong><br />
                                            <%} %>
                                        
                                    </td>
                                    <td  bgColor="#dff0d8" align="left">
                                        <strong>
                                            <%=objHelperServices.GetOptionValues("CURRENCYFORMAT").ToString() + " " + objHelperServices.CDEC(objHelperServices.FixDecPlace(Math.Round(Total, 2, MidpointRounding.AwayFromZero)))%>
                                           <%-- <%=objHelperServices.GetOptionValues("CURRENCYFORMAT").ToString() + " " + objHelperServices.CDEC(objHelperServices.FixDecPlace(Total))%>--%>
                                        </strong>
                                    </td>
                                </tr>
                                
                    </table>                            
                            </div>                     
                        </td>
                    </tr>
                </table>
                <div id="PopupMsg">
                    <asp:Panel ID="ModalPanel" runat="server" Style="display: none" BackColor="White"
                        Height="50px" Width="650px" BorderStyle="Solid" BorderWidth="2px" BorderColor="#b81212">
                        <table width="100%" cellpadding="0" cellspacing="0" border="0" style="border-collapse: collapse;
                            font-family: Arial; font-size: 12px; font-weight: bold; color: #FF0000;" align="center">
                            <tr style="height: 15px">
                                <td colspan="4">
                                    &nbsp;
                                </td>
                            </tr>
                            <tr style="height: 25px">
                                <td width="5%">
                                    &nbsp;
                                </td>
                                <td width="80%" align="left">
                                    Please review and correct Order Clarifications / Errors before proceeding to Check
                                    Out!
                                </td>
                                <td width="10%" align="right">
                                    <asp:Button ID="btnCancel" runat="server" Text="Close" Width="55px" Font-Bold="true"
                                        ForeColor="#1589FF" />
                                </td>
                                <td width="5%">
                                    &nbsp;
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                </div>
                <div id="DivAlert">
                    <asp:Panel ID="pnlAlert" runat="server" Style="display: none" BackColor="White"
                        Height="100px" Width="350px" BorderStyle="Solid" BorderWidth="2px" BorderColor="#0077cc">
                        <table width="100%" cellpadding="0" cellspacing="0" border="0" style="border-collapse: collapse;
                            font-family: Arial; font-size: 12px; font-weight: bold; color: #FF0000;" align="center">
                            <tr style="height: 15px">
                                <td colspan="3">
                                    &nbsp;
                                </td>
                            </tr>
                            <tr style="height: 50px">
                                <td width="5%">
                                    &nbsp;
                                </td>
                                <td width="80%" align="center">
                                    
                                    <asp:Label ID="lblAlert" runat="server" Text=""></asp:Label>
                                </td>                               
                                <td width="5%">
                                    &nbsp;
                                </td>
                            </tr>
                            <tr>
                             <td colspan="3" align="center">
                                  <asp:Button ID="btnok" runat="server" Text="Ok" Width="55px" Font-Bold="true"
                                        ForeColor="#1589FF" />
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                </div>
                <%
                                                                    if (objOrderServices.GetOrderItemCount(OrderID) == 0)
                                                                    {
                                                                        if (Session["NOITEMADDED"] != null && Session["NOITEMADDED"].ToString() != "")
                                                                        {
                                                                          //  Response.Redirect("ConfirmMessage.aspx?Result=NOPRICEAMT");
                                                                        }
                                                                        else
                                                                        {
                                                                            if (Request.QueryString["bulkorder"] != null && Request.QueryString["bulkorder"].ToString() == "1")
                                                                            {

                                                                            }
                                                                            else
                                                                            {
                                                                              //  Response.Redirect("ConfirmMessage.aspx?Result=QTEEMPTY");
                                                                            }
                                                                        }
                                                                    }

                                                                    if ((HttpContext.Current.Session["ORDER_ID"] != null && Convert.ToInt32(HttpContext.Current.Session["ORDER_ID"]) > 0) || (HttpContext.Current.Request.QueryString["ViewOrder"] != null && HttpContext.Current.Request.QueryString["ViewOrder"].Equals("View")))
                                                                    {
                                                                        OrderID = Convert.ToInt32(Session["ORDER_ID"]);
                                                                    }
                                                                    else
                                                                    {
                                                                        OrderID = objOrderServices.GetOrderID(objHelperServices.CI(Session["USER_ID"].ToString()), OpenOrdStatusID);
                                                                    }

                                                                    decimal totalcost = objOrderServices.GetOrderTotalCost(OrderID);

                                                                    
                %>
                    <asp:Button ID="btnHiddenTestPopupExtender" runat="server" Style="display: block;
        visibility: hidden"></asp:Button>
            

                <div id="Div2">
                    <asp:Panel ID="MessageboxPanel" runat="server" Style="display: none" BackColor="White"
                        Height="65px" Width="450px" BorderStyle="Solid" BorderWidth="2px" BorderColor="#b81212">
                        <table width="100%" cellpadding="0" cellspacing="0" border="0" style="border-collapse: collapse;
                            font-family: Arial; font-size: 12px; font-weight: bold; color: #FF0000;" align="center">
                            <tr style="height: 5px;">
                                <td>
                                    &nbsp;
                                </td>
                            </tr>
                            <tr style="height: 10px;">
                                <td>
                                    Invalid Quantity! Minimum Ordered Quantity Should be Equal/Greater than 1
                                </td>
                            </tr>
                            <tr style="height: 5px;">
                                <td>
                                    &nbsp;
                                </td>
                            </tr>
                            <tr style="height: 15px;">
                                <td>
                                    <asp:Button ID="CloseButton" runat="server" Text="Close" Width="55px" Font-Bold="true"
                                        ForeColor="#1589FF" />
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                </div>

                <div id="divOrderTemplate1" runat="server" visible="false"  >
                  
                    <asp:Panel ID="plnOrderTemplate" runat="server" Style="display: block" BackColor="White" CssClass="PopUpDisplayStylehpop"
                        Height="130px" Width="400px" BorderStyle="Solid" BorderWidth="2px">                        
                      

                        <table width="100%" cellpadding="0" cellspacing="0" border="0" style="border-collapse: collapse;"
                align="center">
                <tr style="height: 5px">
                    <td colspan="3">
                        &nbsp;
                    </td>
                </tr>
                <tr style="height: 10px">
                    <td width="100%" align="center" colspan="3">
                        &nbsp;
                    </td>
                </tr>
                <tr style="height: 10px">
                    <td width="100%" align="center" colspan="3" class="TextContentStyle">
                      Template Name Already Exist,
                        <br />
                        Do You want to Replace It.
                    </td>
                </tr>
                <tr style="height: 10px">
                    <td width="100%" align="center" colspan="3">
                        &nbsp;
                    </td>
                </tr>
                <tr style="height: 5px">
                    <td colspan="3">
                        &nbsp;
                    </td>
                </tr>
                <tr style="height: 10px">
                    <td width="45%" align="right">
                        <asp:Button ID="ContinueOrder" runat="server" Text="Yes"
                            Width="205px" CssClass="ButtonStylehpop" OnClick="btnSaveOrdTemplate_excel"  />
                    </td>
                    <td width="10%">
                        &nbsp;
                    </td>
                    <td width="45%" align="left">
                        <asp:Button ID="ClearOrder" runat="server" Text="No" Width="165px"
                            CssClass="ButtonStylehpop" OnClick="btnOTClose_click" />
                    </td>
                </tr>
            </table>
                
                        
                        
                       
                    </asp:Panel>
                </div>
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="updCart" EventName="Click" />
                <asp:AsyncPostBackTrigger ControlID="ImageButton2" EventName="Click" />
            </Triggers>
        </asp:UpdatePanel>


	</div>
 
            