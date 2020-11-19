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


  
<script src="../Scripts/jquery1.11.2.js"></script>

    <style>
        #cover-spin {
            position: fixed;
            width: 100%;
            left: 0;
            right: 0;
            top: 0;
            bottom: 0;
            background-color: rgba(255,255,255,0.7);
            z-index: 9999;
            display: none;
        }
         #cover-spin1 {
            position: fixed;
            width: 100%;
            left: 0;
            right: 0;
            top: 0;
            bottom: 0;
            background-color: rgba(255,255,255,0.7);
            z-index: 9999;
            display: none;
        }

        @-webkit-keyframes spin {
            from {
                -webkit-transform: rotate(0deg);
            }

            to {
                -webkit-transform: rotate(360deg);
            }
        }

        @keyframes spin {
            from {
                transform: rotate(0deg);
            }

            to {
                transform: rotate(360deg);
            }
        }

        #cover-spin::after {
            content: '';
            display: block;
            position: absolute;
            left: 48%;
            top: 40%;
            width: 40px;
            height: 40px;
            border-style: solid;
            border-color: black;
            border-top-color: transparent;
            border-width: 4px;
            border-radius: 50%;
            -webkit-animation: spin .8s linear infinite;
            animation: spin .8s linear infinite;
        }
        #cover-spin1::after {
            content: '';
            display: block;
            position: absolute;
            left: 48%;
            top: 40%;
            width: 40px;
            height: 40px;
            border-style: solid;
            border-color: black;
            border-top-color: transparent;
            border-width: 4px;
            border-radius: 50%;
            -webkit-animation: spin .8s linear infinite;
            animation: spin .8s linear infinite;
        }



        body {
            font-family: Arial, sanserif;
        }

        .myProduct_upl_wrap {
            padding: 10px 10px 20px;
            background: #f2f2f2;
            border-radius: 4px;
            min-height: 150px;
            margin-bottom: 20px;
            width: 970px;
        }

        .upload_title1 {
            background: #0071CF;
            border-radius: 4px 4px 4px 4px;
            color: #FFFFFF;
            font-size: 12px;
            height: 34px;
            line-height: 34px;
            text-indent: 15px;
            width: 100%;
            margin-bottom: 15px;
        }

        .upload_titleDark {
            background: #4e4e4e;
            border-radius: 4px 4px 4px 4px;
            color: #FFFFFF;
            font-size: 14px;
            height: 50px;
            line-height: 50px;
            text-indent: 15px;
            width: 98%;
            margin: 15px 0 15px 12px;
            display: block;
            font-weight:700;
        }
        .upload_titleDark1 {
            background: #4e4e4e;
            border-radius: 4px 4px 4px 4px;
            color: #FFFFFF;
            font-size: 14px;
            height: 40px;
            line-height: 40px;
            text-indent: 15px;
            width: 100%;
            margin: 5px 0 15px 0px;
            display: block;
            font-weight:700;
        }
        .btngreen1 {
background-color: #61b11c;
border: none;
}
        .title_pink {
            background: #FF1F3A;
            border-radius: 4px 4px 4px 4px;
            color: #FFFFFF;
            font-size: 12px;
            height: 34px;
            line-height: 34px;
            text-indent: 15px;
            width: 100%;
        }

        table {
            background-color: transparent;
            border-collapse: collapse;
            border-spacing: 0;
            padding: 15px 10px;
        }

        .myProduct_addtocart {
            border: none;
        }

        input {
            font-family: Arial,Tahoma,Helvetica;
            font-size: 12px;
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
        .inputImg1 {
        border:none;
        vertical-align:middle !important;
        background:none;
        outline:none;
        }
        .myproductPageTable {
            border: 1px solid #ccc;
            font-size: 14px;
        }

            .myproductPageTable td {
                border: 1px solid #ccc;
                padding: 4px 6px;
            }

        .noTableborder td {
            border: none !important;
        }

        .myproductPageTable a {
            color: #0071CF;
            text-decoration: none;
            font-size: 14px;
        }

        .buttons_wrap {
            width: 96%;
            display: block;
            margin-bottom: 10px;
            text-align: left;
        }

        .upload_wrap {
            width: 96%;
            display: block;
            margin-bottom: 10px;
            text-align: left;
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
            background-color: #009900;
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
            border: none;
            border-top-left-radius: 6px;
            border-bottom-left-radius: 6px;
            font-weight: 300;
            border: 1px solid #ccc;
        }

        input[type=file].file-upload-field {
            -webkit-appearance: none;
            -moz-appearance: none;
        }


        .upload_btn {
            width: 80px;
            font-size: 14px;
            height: 32px;
            color: #fff;
            background: #0071CF;
            border-top-right-radius: 6px;
            border-bottom-right-radius: 6px;
            border: none;
        }

        .buttons_wrap a {
            width: 100px;
            text-align: center;
            text-decoration: none;
            margin: 0 5px;
            border-radius: 6px;
            font-size: 14px;
        }

            .buttons_wrap a.blue {
                background: #0071CF;
                color: #fff;
                line-height: 42px;
                width: 180px;
            }

            .buttons_wrap a.green {
                background: #009900;
                color: #fff;
                line-height: 42px;
            }

            .buttons_wrap a.blue:hover {
                opacity: 0.8;
            }

            .buttons_wrap a.green:hover {
                opacity: 0.8;
            }

        .dflex {
            display: flex;
        }

        .title_blue {
            color: #0071CF;
            font-weight: normal;
        }

        .clear {
            clear: both;
        }
 .pagination
{
  height:40px;
     padding-left: 5px;
   padding-right: 5px;
       background-color: #ececec;
       /*background-color: #000;*/
   color: #2461BF;
   border: none;
   font-weight: bold;
   border-top: 1px solid #808080;
    border-bottom: 1px solid #808080;
      
}
 .pagination span {
    padding-left: 0;
    padding-right: 0;
    color: #fff;
    background-color: #0071CF;
    width: 25px;
    height: 25px;
    display: inline-block;
    text-align: center;
    vertical-align: middle;
    line-height: 25px;
    border-radius: 50%;
}

.Grid .pagination tr > td {
    border:none;
}

.pagination a, 
.pagination a:visited
{
  text-decoration: none;
  padding: 6px;
  white-space: nowrap;
  color: #2461BF;
  font-weight: bold
}
.pagination a:hover, 
.pagination a:active
{
  /*padding: 5px;
  border: solid 1px #9ECDE7;
  text-decoration: none;
  white-space: nowrap;
  background: #486694;*/
    /*padding-left: 5px;
   padding-right: 5px;
   background-color: #fff;
   color: #2461BF;*/
}
/*.Grid > tr > td:first-child, .Grid > tr > th:first-child {
 border-left:0px;
}
.Grid > tr > td:last-child, .Grid > tr > th:last-child {
 border-left:0px;
}*/
        .m0 {
        margin:0 !important;

        }
        .ml30 {
        margin-left:30px !important;

        }
        
        .p0 {
        padding:0 !important;

        }
        .f12 {
            font-size:12px !important;
        }
        .f13 {
            font-size:13px !important;
        }
        .f14 {
            font-size:14px !important;
        }
        .blue1 {
            color:#0071CF !important;
        }
    </style>

<%--popup1--%>
<style>
    body {
        font-family: Arial, sanserif;
    }

    table {
        background-color: transparent;
        border-collapse: collapse;
        border-spacing: 0;
        padding: 15px 10px;
    }

    .title_blue {
        color: #0071CF;
        font-weight: normal;
    }

    .clear {
        clear: both;
    }

    .dblock {
        display: block;
    }

    .inblock {
        display: inline-block;
    }

    .text-center {
        text-align: center;
    }

    .text-left {
        text-align: left;
    }

    .addItems_popup {
        width: 418px;
        height: 342px;
        border: 5px solid #61b11c;
        background: #fff;
        text-align: center;
        padding: 20px;
        position: fixed;
        top: 0;
        right: 0;
        left: 0;
        bottom: 0;
        margin: auto;
    }

        .addItems_popup h2 {
            margin-top: 8px;
            margin-bottom: 15px;
            color: #4e4e4e;
            font-size: 18px;
            font-weight: bold;
        }

    .orderCode_box {
        display: block;
        width: 252px;
        position: relative;
        margin: 0 auto;
        margin-bottom: 15px;
            padding-top: 20px;
    }

        .orderCode_box h5 {
            margin-bottom: 12px;
        }

    .orderCode_box_title {
        font-size: 13px;
        color: #444;
        text-align: left;
         width: 196px;
    display: inline-block;
    font-weight: 400;
    }

    .ml12 {
        margin-left: 12px;
    }

    .ml20 {
        margin-left: 20px;
    }

    .orderCode_box input.oc_inp {
        border: 2px solid #dadada;
        width: 176px;
        height: 37px;
        vertical-align: middle;
        color: #444;
        float: left;
        margin-right: 8px;
        margin-bottom: 8px;
        padding-left: 10px;
    }

    .orderCode_box input.qty_inp {
        border: 1px solid #b5b5b5;
        width: 42px;
        height: 38px;
        vertical-align: middle;
        text-align: center;
        color: #444;
        float: right;
        margin-bottom: 8px;
    }

    a.addRows {
        color: #0071CF;
        font-size: 13px;
        text-align: center;
        display: block;
        text-decoration: none;
        vertical-align: middle;
        line-height: 22px;
    }

        a.addRows span {
            margin-top: 3px;
            display: inline-block;
            vertical-align: middle;
            margin-right: 7px;
        }

    .btm_btnsWrap {
        width: 252px;
        margin: 15px auto 0;
        text-align: center;
        display: flex;
        align-items: center;
        justify-content: center;
    }

    .addList_btn {
        background: #0071CF;
        color: #fff;
        font-size: 13px;
        text-align: center;
        display: block;
        width: 106px;
        padding: 10px 0;
        text-decoration: none;
        border-radius: 4px;
        margin-right: 12px;
        border: none; /*//srini addded*/
    cursor: pointer;
    outline:none
    }

    .cancel_btn {
        background: #ececec;
        color: #444;
        font-size: 13px;
        text-align: center;
        display: block;
        width: 106px;
        padding: 12px 0;
        text-decoration: none;
        border-radius: 4px;
    }
</style>
<%--popup2--%>
<style>
    body {
        font-family: Arial, sanserif;
    }

    table {
        background-color: transparent;
        border-collapse: collapse;
        border-spacing: 0;
        padding: 15px 10px;
    }

    .title_blue {
        color: #0071CF;
        font-weight: normal;
    }

    .clear {
        clear: both;
    }

    .dblock {
        display: block;
    }

    .inblock {
        display: inline-block;
    }

    .text-center {
        text-align: center;
    }

    .text-left {
        text-align: left;
    }

    .text-right {
        text-align: right;
    }

    .pull-right {
        float: right;
    }

    .pull-left {
        float: left;
    }

    .excelUpload_popup {
        width: 350px;
        height: 212px;
        border: 5px solid #61b11c;
        background: #fff;
        text-align: center;
        padding: 20px;
        position: fixed;
        top: 0;
        right: 0;
        left: 0;
        bottom: 0;
        margin: auto;
    }

        .excelUpload_popup h2 {
            margin-top: 8px;
            margin-bottom: 25px;
            color: #4e4e4e;
            font-size: 18px;
            font-weight: bold;
        }

    .excelUpload_box {
        display: block;
        width: 320px;
        position: relative;
        margin: 0 auto;
        margin-bottom: 10px;
    }

        .excelUpload_box h5 {
            margin-bottom: 12px;
            font-weight: normal;
        }

    .excelUpload_box_title {
        font-size: 13px;
        color: #444;
        text-align: left;
        width: 216px;
        display: inline-block;
    }

    .ml12 {
        margin-left: 12px;
    }

    .ml20 {
        margin-left: 20px;
    }

    .mr12 {
        margin-right: 12px;
    }

    .mr20 {
        margin-right: 20px;
    }

    .mb12 {
        margin-bottom: 12px;
    }

    .mb20 {
        margin-bottom: 20px;
    }

    .excelUpload_box input.upl_inp {
        border: 2px solid #dadada;
        width: 290px;
        padding: 8px;
        vertical-align: middle;
        color: #444;
        float: left;
        margin-right: 8px;
        margin-bottom: 8px;
        padding-left: 10px;
      
    }
    .upl_inp {
      font-size: 11px;
    }
    a.addRows {
        color: #0071CF;
        font-size: 13px;
        text-align: center;
        display: block;
        text-decoration: none;
        vertical-align: middle;
        line-height: 22px;
    }

    .btm_btnsWrap {
        width: 252px;
        margin: 15px auto 0;
        text-align: center;
        display: flex;
        align-items: center;
        justify-content: center;
    }

    .addList_btn {
        background: #0071CF;
        color: #fff;
        font-size: 13px;
        text-align: center;
        display: block;
        width: 106px;
        padding: 10px 0;
        text-decoration: none;
        border-radius: 4px;
        margin-right: 12px;
    }

    .cancel_btn {
        background: #ececec;
        color: #444;
        font-size: 13px;
        text-align: center;
        display: block;
        width: 106px;
        padding: 12px 0;
        text-decoration: none;
        border-radius: 4px;
    }

    .excel_btn {
        color: #0071CF;
        font-size: 14px;
        text-align: left;
        display: block;
        text-decoration: none;
        vertical-align: middle;
        line-height: 22px;

    }

    .bottombtns_wrapper {
        width: 98%;
        margin: 15px 0;
    }

    .excel_btn img {
        margin-right: 10px;
        vertical-align: middle;
    }

    .lgbtn_blue {
        width: 195px;
        padding: 10px 0;
        border-radius: 4px;
        color: #fff !important;
        background: #0071CF;
        text-align: center;
        font-size: 13px;
        vertical-align: middle;
        text-decoration: none;
    }

    .iconBtn_blue {
        width: 195px;
        padding: 4px 0;
        border-radius: 4px;
        color: #fff !important;
        background: #0071CF;
        text-align: center;
        font-size: 13px;
        vertical-align: middle;
        text-decoration: none;
    }

        .iconBtn_blue img {
            margin-right: 10px;
            vertical-align: middle;
        }
    input.ol_delete {
        background-color:none !important;
        background:none;
        border:none;
        outline:none
    }
    .mdel {
    vertical-align:middle;
    }
</style>

<style>
      .buttonProd{
                background: url(../images/Plus_icon.png) no-repeat;
                cursor:pointer;
                border: none;
            }
.Grid {
    border-collapse: collapse;
}
.Grid td, .Grid th {
    border: 1px solid gray;
}
.Grid tr:first-child th {
    border-top: 0px;
}
.Grid tr:last-child td {
    border-bottom: 0px;
}
.Grid tr td:first-child,
.Grid tr th:first-child {
    border-left: 0px;
}
.Grid tr td:last-child,
.Grid tr th:last-child {
    border-right: 0px;
}
.Grid.templatePge td{
    border:1px solid gray;
    border-top: none;
    border-bottom: none;
    font-size:13px;
    padding: 3px 6px;
}
.Grid.savedTemplate td{
    padding: 8px 6px;
     border-top: 0px;
     border-bottom: 0px;
}
    
</style>


<script type="text/javascript">
    function Callprogress() {
        alert("x");

    }

</script>

  <%--add rows dropdown list showing--%>



		<div class="myProduct_upl_wrap" id="divMain" runat="server" >
              
                <div class="upload_wrap">
            <h3 class="upload_titleDark1">Please Upload Your  Products</h3> 
          
                                          
                    <div style="width: 330px;float: left;">
                   <p style="display: inline;" class="f13"> Create New Template </p>:<span class="redx">*</span>
                    <div class="clear"></div>
                                         
                                             <asp:TextBox  style="Width:288px;height:24px;margin-top: 5px;" ID="txtName" runat="server"  MaxLength="50" class="input_dr" ></asp:TextBox>
                     <asp:RequiredFieldValidator runat="server" ForeColor="Red" id="reqName" controltovalidate="txtName" errormessage="Please enter your Template name!" validationgroup="templ" />
                                       
                      
                <div style="text-align:center!important"  >
             <asp:Label runat="server" id="StatusLabel" text="Upload status: "  Visible ="false" />
                    <asp:Label ID="lbldiverror" runat="server" ForeColor="Red"></asp:Label>
               </div>                     
             </div>       
                                                                     
                   <div style="width: 360px;float: left;" >                            
            <P class="p2 f13" style="display: inline;" >Upload your excel file for My Products. </P>
           
			<div class="clear"></div>

            
			<div class="dflex">



				<div class="file-upload-wrapper" data-text="Select your file!"  >
				    

                      <asp:FileUpload ID="FileUploadControl" runat="server" CssClass="file-upload-wrapper"   validationgroup="templ" BorderColor="#0071CF" BorderStyle="Solid" /> 
                     <asp:Button ID="btnUpload"  class="upload_btn" Text="Upload" runat="server"  OnClick="UploadButton_Click"  OnClientClick="$('#cover-spin').show(0)" />
                        
               

				</div>
				
				  
			</div>
           </div>

            <div style="width:220px; float:left; display:block;">
                    <asp:LinkButton id="LinkButton2" Text="Click Here to Download example Excel upload order sheet" OnClick="LinkButton_Click"  runat="server" class="toplinkatest" > 
                    <div class=""><p style="width:285px;margin-top:24px;margin-bottom:0px" class="f14"><img src="../images/Excel_icon.png" /> Download Sample Sheet</p></div>     </asp:LinkButton>
            </div>


			<div class="clear"></div>
		</div>
            <div class="clear"></div>
            <span id="split3" style="padding-top:5px;color:red">
                 <asp:Literal ID="Literal3" runat="server"></asp:Literal> 
             </span> 
            <div class="buttons_wrap" id="divsavetemp" runat="server" visible="false">
               
                     <%--<h3 class="upload_titleDark1">Select From Saved Template</h3> --%>
            
<asp:GridView ID="dgtemplate" runat="server" AutoGenerateColumns="False" CssClass="Grid savedTemplate"
    DataKeyNames="ID" OnRowCommand="dgtemplate_RowCommand" CellPadding="4"  GridLines="None"  ForeColor="#333333" PageSize="10"  BorderWidth="1px" BorderColor="#808080"
      Width="100%">
   
    <%--<AlternatingRowStyle BackColor="#D8EBF5" />--%>
<%--    <EditRowStyle ForeColor="#0066FF" />
    <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
    <HeaderStyle BackColor="#266AAA" Font-Bold="True" ForeColor="White" HorizontalAlign="Left" Font-Size="15px" BorderStyle="None" />
    <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="left" />
    <RowStyle BorderStyle="None" Font-Bold="False" Font-Size="Large" Font-Underline="False" ForeColor="#0066FF" />
    <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
    <SortedAscendingCellStyle BackColor="#F5F7FB" />
    <SortedAscendingHeaderStyle BackColor="#6D95E1" />
    <SortedDescendingCellStyle BackColor="#E9EBEF" />
    <SortedDescendingHeaderStyle BackColor="#4870BE" />--%>
             
            
     <RowStyle Height="22px"  BackColor="#ECECEC" />
                      
    <AlternatingRowStyle BackColor="White" />
   
    <Columns>
        <asp:BoundField DataField="ID" HeaderText="ID" InsertVisible="False"  Visible="false"
            ReadOnly="True" SortExpression="ID" />
        
        <asp:TemplateField HeaderText="Template Name"  SortExpression="Filepath">
               
            <ItemTemplate >
                <%--<asp:LinkButton ID="LbPath1" runat="server" ForeColor="#007EE0" 
                    Text='<%# Eval("Filename") %>'
                    CommandName="PathUpdate" 
                    CommandArgument='<%#Bind("id") %>'>
                </asp:LinkButton>--%>
                <asp:Label ID="LbPath" runat="server" ForeColor="#333333" Text='<%# Eval("Filename") %>'></asp:Label>
                <%--  --%>
            </ItemTemplate>
            <ControlStyle Font-Size="13px" />
              <HeaderStyle HorizontalAlign="Center" />
             <ItemStyle ForeColor="#333333" HorizontalAlign="Center" />
             </asp:TemplateField>
           <asp:TemplateField HeaderText="Created On" SortExpression="Filepath">
            <ItemTemplate>
                <asp:Label ID="lblcreatedon" runat="server" ForeColor="#333333" Text='<%# Eval("CreatedOn") %>'></asp:Label>
                 
              
            </ItemTemplate>
               <ControlStyle Font-Size="13px" />
               <HeaderStyle HorizontalAlign="Center" />
            <ItemStyle ForeColor="#333333" HorizontalAlign="Center" />
             </asp:TemplateField>
       
         <asp:TemplateField HeaderText="Actions" SortExpression="Filepath">
            <ItemTemplate>
                    <asp:ImageButton ID="Imagedelete_mainProduct" CssClass="inputImg1" runat="server" src="images/View_Icon_1.PNG"  CommandName="PathUpdate" 
                        CommandArgument ='<%#Bind("id") %>'
                    />
               <%-- <asp:LinkButton ID="LbPath1" runat="server" ForeColor="#333333" 
                    Text='View'
                    CommandName="PathUpdate" 
                    CommandArgument='<%#Bind("id") %>'>
                </asp:LinkButton>--%>
        
             <asp:ImageButton ID="Image3" 
                            runat="server" class="ol_delete ml30 mdel" AlternateText="" ImageUrl="~/images/Delete_icon1.png" Width="16px" Height="16px" CommandName="Delete_dgtemplate"
                    OnClick="Imagedelete_mainProduct_Click" OnClientClick="return confirm('Are you sure you want to delete this product?');"
                           />
            </ItemTemplate>
             <HeaderStyle HorizontalAlign="Center" />
             <HeaderStyle HorizontalAlign="Center" />
            <ItemStyle ForeColor="#0066FF" HorizontalAlign="Center" />
             </asp:TemplateField>
    
     <%--    <asp:TemplateField HeaderText="Delete" SortExpression="Filepath">
            <ItemTemplate>
      
                     <asp:ImageButton ID="Image3" 
                            runat="server" class="ol_delete" AlternateText="" ImageUrl="~/images/Delete_icon1.png" Width="16px" Height="16px" CommandName="Delete_dgtemplate"
                    OnClick="Imagedelete_mainProduct_Click" OnClientClick="return confirm('Are you sure you want to delete this product?');"
                           />
            
            </ItemTemplate>
             <HeaderStyle HorizontalAlign="Center" />
            <ItemStyle ForeColor="#0066FF" HorizontalAlign="Center" />
             </asp:TemplateField>--%>

         
    </Columns>
                            <EditRowStyle BackColor="#2461BF" /> 
    <EmptyDataRowStyle BorderStyle="None"></EmptyDataRowStyle>


                        <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="#0071CF" />
            <HeaderStyle CssClass="grdheadermster" BackColor="#0071CF" Font-Bold="false" ForeColor="White" Font-Size="13px" Height="40px" BorderColor="#000444"></HeaderStyle>
             <PagerStyle CssClass="pagination" HorizontalAlign="Center" 
 VerticalAlign="Middle"/>
             
     <RowStyle Height="22px"  />
                       <%-- <SelectedRowStyle BackColor="#333333" Font-Bold="True" ForeColor="#D1DDF1" />
                        <SortedAscendingCellStyle BackColor="#F5F7FB" />
                        <SortedAscendingHeaderStyle BackColor="#6D95E1" />
                        <SortedDescendingCellStyle BackColor="#E9EBEF" />
                        <SortedDescendingHeaderStyle BackColor="#4870BE" />--%>

</asp:GridView>
				</div>
			
         
             <div id="cover-spin"></div>

         
            </div>
<div Style="text-align:left;">
<asp:Label ID="lbltempname" ControlStyle-CssClass="upload_titleDark"  runat="server"  Visible="false"></asp:Label>
    </div>
<div class="clear"></div>

<div class="clear"></div>
		<div class="testscroll" id="divmyproduct" runat="server" visible="false" > 
            <div id="cover-spin1"></div>
            <div class="excelbtn_wrapper pull-right mb20 mr20" >
	<%--<a href="" class="excel_btn"><img src="../images/Excel_icon.png"> Download excel product list </a>--%>
  <asp:LinkButton id="LinkButton3" class="excel_btn" Style="color: #0071CF;" OnClick="LinkButton3_Click"  runat="server"><img src="../images/Excel_icon.png" />Download Excel Product List</asp:LinkButton> 
</div>
             <span id="split2" style="padding-top:5px;color:red" >
                 <asp:Literal ID="Literal2" runat="server"></asp:Literal> 
             </span> 
                  <asp:Label runat="server" id="lblnoprod" text="No Products To Display" Visible ="false" />
               <%--<h3 class="title_blue">My Product List</h3>--%>
		 <asp:GridView ID="Gridprodlst" runat="server" AutoGenerateColumns="False"  CssClass="Grid templatePge"
            AllowPaging="true" OnRowDataBound="Gridprodlst_RowDataBound" Width="98%" 
            DataKeyNames="CODE" OnRowCommand="Gridprodlst_RowCommand" EmptyDataRowStyle-BorderStyle="None" GridLines="None"
                         OnPageIndexChanging="Gridprodlst_PageIndexChanging" CellPadding="4" ForeColor="#333333" PageSize="10"    >
                     
            <RowStyle Height="15px" BorderColor="Black" BackColor="#ECECEC" />
                       <AlternatingRowStyle BackColor="White" />
            <Columns>
               <%-- <asp:TemplateField HeaderStyle-CssClass="grdheadermster" Visible="false">
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
                </asp:TemplateField>--%>

                <asp:TemplateField HeaderStyle-CssClass="grdheadermster" >
                     <HeaderTemplate>
                       
                       <%--pid--%>
                       
                    </HeaderTemplate>

                    <ItemTemplate>
                        <div style="padding: 0 3px; word-wrap: break-word; overflow: hidden; width: 100px">
                           <%-- <asp:Label ID="lblordercode" runat="server" Text='<%# bind("CODE") %>' CommandName="Entry"></asp:Label>--%>
                            <%--<asp:Label ID="lblprdid" runat="server" CssClass="labelstyle" Text='<%# bind("Product_Id") %>'></asp:Label>--%>

                <%-- <asp:ImageButton ID="ImgOrd" Visible='<%# Bind("FlagVisible_Invisible") %>' runat="server"  AlternateText="" ImageUrl='<%# Bind("STRING_VALUE") %>' />--%>
                            <img src='<%# Bind("STRING_VALUE") %>' ID="ImgOrd" runat="server" />
                        </div>
                    </ItemTemplate>
                     <ItemStyle CssClass="itemstyle_border" HorizontalAlign="Center" VerticalAlign="Middle" />
                    <HeaderStyle CssClass="grdheadermster"></HeaderStyle>
                </asp:TemplateField>
                <asp:TemplateField HeaderStyle-CssClass="grdheadermster">
                    <HeaderTemplate>
                        Order Code
                    
                    </HeaderTemplate>
                    <ItemTemplate>
                        <div style="padding: 0 3px; word-wrap: break-word; overflow: hidden; width: 100px">
                            <asp:Label ID="lblordercode" runat="server" Text='<%# Bind("CODE") %>' CommandName="Entry"></asp:Label>
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
                            <asp:Label ID="lblCost" runat="server" CssClass="labelstyle" Text='<%# Bind("Price") %>'></asp:Label>
                        </div>
                    </ItemTemplate>
                    <ItemStyle CssClass="itemstyle_border" HorizontalAlign="Center" VerticalAlign="Middle" />
                    <HeaderStyle CssClass="grdheadermster"></HeaderStyle>
                </asp:TemplateField>
                  <asp:TemplateField HeaderStyle-CssClass="grdheadermster">
                    <HeaderTemplate>
                       Stock Status
                    </HeaderTemplate>
                    <ItemTemplate>
                        <div style="padding: 0 3px; word-wrap: break-word; overflow: hidden; width: 200px">
                            <asp:Label ID="lblStockStatus" runat="server" CssClass="labelstyle" Text='<%# Bind("Stock_Status") %>' ></asp:Label>
                        </div>
                  
                    </ItemTemplate>
                    <ItemStyle CssClass="itemstyle_border" HorizontalAlign="Center" VerticalAlign="Middle" />
                    <HeaderStyle CssClass="grdheadermster"></HeaderStyle>
                </asp:TemplateField>      
                     <asp:TemplateField HeaderStyle-CssClass="grdheadermster">
                    <HeaderTemplate>
                      Actions
                    </HeaderTemplate>
                    <ItemTemplate>
                        <div style="padding: 0 3px;word-wrap: break-word;overflow: hidden;width: 130px; display:inline-block;vertical-align:middle;">
                            <asp:TextBox ID="txtqty"  runat="server"  Visible='<%# Bind("FlagVisible_Invisible") %>'  Style="width: 30px;height: 30px;display:inline-block;cursor:pointer;border-radius:4px; text-align: center;vertical-align: middle;border: 1px solid #444;" Text='<%# Bind("QTY") %>'></asp:TextBox>
                            <asp:Button ID="btnBuytoadd"  runat="server" Visible='<%# Bind("FlagVisible_Invisible") %>'  style="width:75px;display:inline-block;color:#fff;background:#61b11c;cursor:pointer;text-align:center;border-radius:6px;vertical-align: middle;font-size: 14px;padding: 10px 0;" Text="BUY" OnClick="btnBuytoadd_Click" />

                        </div>
                              <asp:ImageButton ID="ImageButtonDelete" 
                            runat="server" class="ol_delete mdel ml20 " AlternateText="" ImageUrl="~/images/Delete_icon1.png" Width="16px" Height="16px" 
                     
                            CommandName="Delete_grdProd"  OnClick="ImageButtonDelete_Click"
                        OnClientClick="return confirm('Are you sure you want to delete this product?');"
                           />   
                    </ItemTemplate>
                    <ItemStyle CssClass="itemstyle_border" HorizontalAlign="Center" VerticalAlign="Middle" />
                    <HeaderStyle CssClass="grdheadermster"></HeaderStyle>
                </asp:TemplateField>  
           
              <%--   <asp:TemplateField HeaderStyle-CssClass="grdheadermster" ItemStyle-HorizontalAlign="Center">
                    <HeaderTemplate>
                        Delete
                    </HeaderTemplate>
                    <ItemTemplate>
                     
                        <asp:ImageButton ID="ImageButtonDelete" 
                            runat="server" class="ol_delete green" AlternateText="" ImageUrl="~/images/Delete_icon1.png" Width="16px" Height="16px" 
                     
                            CommandName="Delete_grdProd"  OnClick="ImageButtonDelete_Click"
                        OnClientClick="return confirm('Are you sure you want to delete this product?');"
                           />   
                             
                    </ItemTemplate>
                    <ItemStyle CssClass="itemstyle_border" VerticalAlign="Middle" />
                    <HeaderStyle CssClass="grdheadermster"></HeaderStyle>
                </asp:TemplateField>--%>
            </Columns>
                        <EditRowStyle BackColor="#2461BF" />

<EmptyDataRowStyle BorderStyle="None"></EmptyDataRowStyle>

                        <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
            <HeaderStyle CssClass="grdheadermster" BackColor="#0071CF" Font-Bold="false" ForeColor="White" Font-Size="13px" Height="40px" BorderColor="#000444"></HeaderStyle>
             <PagerStyle CssClass="pagination" ForeColor="#0071CF" HorizontalAlign="Center" 
 VerticalAlign="Middle"/>
             
            <RowStyle Height="22px" />
                        <SelectedRowStyle BackColor="#333333" Font-Bold="True" ForeColor="#D1DDF1" />
                        <SortedAscendingCellStyle BackColor="#F5F7FB" />
                        <SortedAscendingHeaderStyle BackColor="#6D95E1" />
                        <SortedDescendingCellStyle BackColor="#E9EBEF" />
                        <SortedDescendingHeaderStyle BackColor="#4870BE" />
             
        </asp:GridView>
            <div class="bottombtns_wrapper">
	<a href="#" class="lgbtn_blue pull-left mr12" onclick="additembyordercode();">Add Item By Order Code </a>
	<a href="#" class="lgbtn_blue pull-left" onclick="addexcelcode();">Add Item By Excel Upload  </a>
	<%--<a href="#" class="iconBtn_blue pull-right"><img src="../images/addcart_icon.jpg">  Add all Items to Cart </a>--%>
                 <button type="submit" id="cmdAction" runat="server" style ="color: #fff !important; background: #0071CF;width: 187px; padding: 2px 0;height: 36px; border-radius: 4px; border: none; cursor: pointer;float: right;" onclick="errRemove();">
            
                     <img src="../images/addcart_icono.jpg">  Add All Items to Cart
        </button>

           
	<div class="clear"></div>

   
</div>
             <span id="split1" style="padding-top:36px;color:red">
                 <asp:Literal ID="Literal1" runat="server"></asp:Literal> 
             </span>   
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
                    bool UserCheckout = false;
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
                                            if (1 == 1)
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
                            <table id="SiteMapTable0"  class="orderdettable" > 
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
                                                            if (tbErrorItem != null && tbErrorItem.Rows.Count > 0)
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
                                                                    <a href="OrderDetails.aspx?bulkorder=1&amp;rma=NF&amp;item=<%=_NotFoundItem%>&amp;cla_id=<%=_ClaItem_ID%>" style="font-weight: bold;
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
                                                                   _ordQty = (RItem["QTY"].ToString() != "" && Convert.ToInt32(RItem["QTY"].ToString()) > 0) ? Convert.ToInt32(RItem["QTY"].ToString()) : 1;

                                                                   if (_replaceItem.Trim() != "")
                                                                   {
                                                                       if (_replaceItem.Trim() != TempreplaceItem.Trim())
                                                                       {

                                                                           DataTable substituteproduct = new DataTable();
                                                                           DataTable rtntbl = new DataTable();
                                                                           DataTable wag_product_code_substituteproduct = new DataTable();
                                                                           bool samecodesubproduct = false;
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
                                                                               pfid = rtntbl.Rows[0]["Pfid"].ToString();
                                                                               Ea_Path = rtntbl.Rows[0]["Ea_Path"].ToString();
                                                                               samecodesubproduct = (bool)rtntbl.Rows[0]["samecodesubproduct"];
                                                                               samecodenotFound = (bool)rtntbl.Rows[0]["samecodenotFound"];
                                                                               wag_product_code = rtntbl.Rows[0]["wag_product_code"].ToString();
                                                                               SubstuyutePid = rtntbl.Rows[0]["SubstuyutePid"].ToString();
                                                                           }
                                                                           else
                                                                           {
                                                                               samecodesubproduct = true;
                                                                               samecodenotFound = false;
                                                                           }

                                                                           if (samecodenotFound == false && samecodesubproduct == true)
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
                            <div class="upload_titleDark" style="margin: 2px 0 7px 0px;width:100%;text-indent:12px;">Shopping Cart Contents</div>
                              
                            <asp:Label ID="Label1" runat="server" Text="Order No : " Visible="false"></asp:Label>
                             <asp:Label ID="lblOrdNo" runat="server" Visible="false"></asp:Label>
                            <table border="0px" cellpadding="0" cellspacing="0" class="orderdettable" style="font-size:12px !important;">
                             
                             
                             <%--   <tr>
                                    <td align="left" colspan="7" bgColor="#cccccc">
                                        <b>Order Contents</b>
                                    </td>
                                </tr>--%>
                                <tr>
                                    <td align="left" bgcolor="#f2f2f2">
                                        <b>Order Code</b>
                                    </td>
                                    <td align="left"  bgcolor="#f2f2f2" colspan="2" >
                                        <b>Quantity</b>
                                    </td>
                                    <td align="left"  bgcolor="#f2f2f2" style="width:250px;">
                                        <b>Description</b>
                                    </td>
    
                                    <td align="left" bgcolor="#f2f2f2" style="width:170px;">
                                        <b>Cost (Ex. GST)</b>
                                    </td>
                                    <td align="left" bgcolor="#f2f2f2" style="width:170px;">
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
                                                decimal ProdTotal = Math.Round(Qty * ProductUnitPrice, 2, MidpointRounding.AwayFromZero);
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
                                        &nbsp;&nbsp;<a href="byproduct.aspx?&bulkorder=1&SelPid=<%=pid %>&amp;SelProdPrice=<%=ProdTotal %>&amp;ProdPrice=<%=ProductUnitPrice %>&amp;ORDER_ID=<%=OrderID %>&amp;ORDER_ITEM_ID=<%=OrderItemId1 %>" class="toplinkatest">Remove</a>&nbsp;&nbsp;
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
                                            Total = Math.Round(subtot, 2, MidpointRounding.AwayFromZero) + Math.Round(taxamt, 2, MidpointRounding.AwayFromZero) + TotalShipCost;
                                            Total = objHelperServices.CDEC(objHelperServices.FixDecPlace(Math.Round(Total, 2, MidpointRounding.AwayFromZero)));
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
                            <asp:Button ID="lblOrderProceed" runat="server"   OnClick="btnNext_Click" Text="Check Out" style="float: right;margin-right: 15px;padding-left: 28px;padding-right: 28px;margin-bottom: 10px;"  class="button normalsiz btngreen1 fleft"   />                 
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
                 <asp:AsyncPostBackTrigger ControlID="Button1" EventName="Click" />
            </Triggers>
        </asp:UpdatePanel>
                      <script>
                          var modal = document.getElementById('divPop');

                          // When the user clicks anywhere outside of the modal, close it  
                          window.onclick = function (event) {
                              if (event.target == modal) {
                                  modal.style.display = "none";
                              }
                          }
</script> 

	</div>

            <div id="popupordercode_1" class="element" style="display:none; overflow: scroll;">
                
                      
	<div class="addItems_popup" style="overflow: auto;overflow-x:hidden;">
		<h2>Add Items By Order Codes</h2>
        <div id="errormg" style="display:none"></div>
		<div class="orderCode_box" id="myContainer">

            <p id="ErrDisp" style="display:none;color:red;font:bold;text-decoration:solid"></p>
			<div class="dblock text-left">
				<h5 class="orderCode_box_title">Order Code</h5>
				<h5 class="inblock ml12" style="font-size:13px;font-weight:400">Qty</h5>
              
			</div>
             
       <div id="TextBoxContainer">
			<input class="oc_inp" type="text" id="txtOrderCode" name="searchText">
			<input class="qty_inp" type="text" id="txtOrderCodeQty">
               
			
		     </div>
			
			<%--<div class="clear"></div>--%>
		</div>
           <button id="btnAdd" class="buttonProd" style="margin-left: 143px;padding-bottom: 10px;float: left; padding-top: 1px;outline:none" type="button" title="Add More Rows"><span style="padding-left:13px;color: #0071CF;">Add More Rows</span></button> 
        <%--<span><img src="../images/Plus_icon.png"></span>--%>
        <%--<input id="btnGet" type="button" value="Get Values" />--%>
		<div class="btm_btnsWrap">
			<button ID="btnAddList"  class="addList_btn" value="Add List" >Add to List</button>
			<a href="#" class="cancel_btn" onclick="cancelitembyordercode();"> Cancel</a>
 		</div>
	</div></div>
 
<div id="popupexcelcode" style="display:none">
    <div class="excelbtn_wrapper pull-right mb20">
	<a href="" class="excel_btn"><img src="Excel_icon.png" style="color: #0071CF;"> Download excel product list </a>
</div>
<div class="clear"></div>

              <div class="aipopup_wrapper">
	<div class="excelUpload_popup">
		<h2>Add Items by Excel Upload</h2>
        <asp:Label runat="server" id="StatusLabel1" text="Upload status: " Visible ="false" />
		<div class="excelUpload_box">
			<div class="dblock text-left">
				<h5 class="excelUpload_box" style="font-size:11px;">Select File</h5>
			</div>
			
            <asp:FileUpload ID="FileUpload1" runat="server" CssClass="upl_inp" /> 
		
		</div>
		
        <asp:LinkButton id="LinkButton1" class="addRows" OnClick="LinkButton_Click"  runat="server">Download Sample File</asp:LinkButton>
		<div class="btm_btnsWrap">
		
             <asp:Button ID="Button1"  class="addList_btn" Text="Add to List" runat="server"  OnClick="UploadButtonProdlist_Click"   OnClientClick="$('#cover-spin1').show(0)"/>
			<a href="#" class="cancel_btn" onclick="cancelexcelcode();"> Cancel</a>
 		</div>
	</div>
</div>
</div>
<%--add more rows--%>

<script>
    
    function errRemove()
    {
        errRemove1();
    }
    function errRemove1()
    {
        var er1 = document.getElementById("rupspan");
          if (er1 != null) er1.innerText = "";                          
       
       // var er2 = document.getElementById("split2");
       //if (er2 != null) er2.innerText = "";
       //  var er3 = document.getElementById("split3");
       // if (er3 != null)  er3.innerText = "";
    }
    function additembyordercode()
    {    $('#btnAddList').prop('disabled', false);
        var error = document.getElementById("errormg");
        error.style.display = "none";
       // document.getElementById("TextBoxContainer").innerHTML = "";
        $('#TextBoxContainer').find('input:text').val('');
        $("input[name=DynamicTextBoxOrd]").find('input:text').val('');
        $("input[name=DynamicTextBoxOrdQty]").find('input:text').val('');
        var testPopUp = document.getElementById('#popupordercode_1');
        if (testPopUp == null)          
            $("#popupordercode_1").css("display", "block");
        errRemove1();
   
    }
    function cancelitembyordercode() {
        var testPopUp = document.getElementById('#popupordercode_1');
       
        if (testPopUp == null)
            
            $("#popupordercode_1").css("display", "none");
        
    }
    function addexcelcode() {
        //var a =
         
        //alert(a);
        //console.log(a);
        var testPopUp = document.getElementById('#popupexcelcode');
        if (testPopUp == null)
            $("#popupexcelcode").css("display", "block");

       
       errRemove1();
       
    }
    function cancelexcelcode() {

        $("#ctl00_maincontent_byproduct1_StatusLabel1").css("display", "none");
        //document.getElementById("#StatusLabel1").style.display = "none";
        var testPopUp = document.getElementById('#popupexcelcode');
        if (testPopUp == null)
            $("#popupexcelcode").css("display", "none");
        var event = event || window.event;
       
        return false;
       
    }
    //function getExcelpop(e) {
        
    //    var a = document.getElementById('#FileUpload1');
    //    if (a == null || a == '') {
    //        alert("excel");
    //        document.getElementById('StatusLabel1').innerHTML = "Please Choose File";
    //        e.preventdefault();
    //        e.stoppropagation();
    //    }
    //}
  
</script>
<script type="text/javascript">
    var OrdCode = [];
    var OrdQty = [];
    var vCode = "";
    var vQty = "";
    var valuesOrder = "";
    var valuesQty = "";
    var incr = parseInt(0);
    $(function () {
        
      

        $("#btnAdd").bind("click", function () {

            errRemove1();
            var div = $("<div />");
            div.html(GetDynamicTextBox(""));
            $("#TextBoxContainer").append(div);


            //list by order item code

         
        });
        $("#btnAddList").bind("click", function btnAddListclick(e) {
            debugger
            e.preventDefault();
            e.stopPropagation();
            $(this).prop('disabled', true);
           
            vCode = $("#txtOrderCode").val();
           
            vQty = $("#txtOrderCodeQty").val();

            if (vCode == "" || vQty == "")
            {
              var error = document.getElementById("errormg");
                                        error.style.display = "block";
                                        
                                        error.innerHTML = "<span style='color: red;'>" +
                                        "Please enter order code & qty</span>"
                return;
            }

            if (vCode != "" || vQty != "") {
                var error = document.getElementById("errormg");
                error.style.display = "none";
                error.innerHTML = "";
                valuesOrder = vCode + "\n";
                addJSONOrder(valuesOrder);
                valuesQty = vQty + "\n";
                addJSONQty(valuesQty);
            }
            $("input[name=DynamicTextBoxOrd]").each(function () {
               
                    valuesOrder = $(this).val() + "\n";              
                addJSONOrder(valuesOrder);
            });
            $("input[name=DynamicTextBoxOrdQty]").each(function () {
               
                    valuesQty = $(this).val() + "\n";
             
            
                addJSONQty(valuesQty);
            });
          console.log(JSON.stringify(OrdCode));
          console.log(JSON.stringify(OrdQty));

          var jqxhr = $.post("byproduct.aspx/ProcessAddDeleteOrder", function () {
                            $.ajax({
                                type: "POST",
                                url: "byProduct.aspx/ProcessAddDeleteOrder",
                                data: "{'OrderCode':'" + JSON.stringify(OrdCode) + "','Qty':'" + JSON.stringify(OrdQty) + "'}",
                                contentType: "application/json; charset=utf-8",
                                dataType: "json",
                                success: function (data) {
                                    debugger
                                    
       
                                   
                                    if(data.d == "Empty")
                                    {
                                        debugger
                                        var error = document.getElementById("errormg");
                                        error.style.display = "block";
                                        
                                        error.innerHTML = "<span style='color: red;'>" +
                                        "Please enter order code & qty</span>"
                                        OrdCode = [];
                                         OrdQty = [];
                                      
                                    }
                                    else if (data.d == "Err1") {
                                          $('#btnAddList').prop('disabled', false);
                                        var error = document.getElementById("errormg");
                                        error.style.display = "block";
                                      
                                        error.innerHTML = "<span style='color: red;'>" +
                                         "Please enter order code </span>"
                                        OrdCode = [];
                                        OrdQty = [];
                                    }
                                    else if (data.d == "Err2") {
                                          $('#btnAddList').prop('disabled', false);
                                        var error = document.getElementById("errormg");
                                        error.style.display = "block";
                                        error.innerHTML = "<span style='color: red;'>" +
                                           "Please enter qty</span>"
                                        OrdCode = [];
                                        OrdQty = [];

                                    }
                                    else if (data.d == "Err3") {
                                          $('#btnAddList').prop('disabled', false);
                                        var error = document.getElementById("errormg");
                                        error.style.display = "block";
                                        error.innerHTML = "<span style='color: red;'>" +
                                       "Please enter valid code & qty</span>"
                                        OrdCode = [];
                                        OrdQty = [];

                                    }
                                    else if (data.d == "Success") {
                                        OrdCode = [];
                                        OrdQty = [];
                                        var error = document.getElementById("errormg");
                                        error.style.display = "block";
                                        error.innerHTML = "<span style='color: green;'>" +
                                           "Added success</span>"
                                      
                                       var er1 = document.getElementById("split1");
                                    if (er1 != null)
                                      er1.innerText = "";
       
                                    var er2 = document.getElementById("split2");
                                     if (er2 != null)
                                    er2.innerText = "";
                                    var er3 = document.getElementById("split3");
                                     if (er3 != null)
                                            er3.innerText = "";
                                        window.location.href="byproduct.aspx?GetData=true";
                                       
                                    <%--        
                                      <%
    string idcop = HttpContext.Current.Session["bindid"].ToString();

    GetDataFromDb(idcop);
    %>--%>
                                        //window.location.assign(document.url);
                                       //window.location.href = window.location.protocol +'//'+ window.location.host + window.location.pathname+'#';
                                       
                                    }
                                    else
                                    {
                                          $('#btnAddList').prop('disabled', false);
                                        var error = document.getElementById("errormg");
                                        error.style.display = "block";
                                        error.innerHTML = "<span style='color: red;'>" +
                                       " Please enter valid code '" + data.d + "'</span>"
                                        OrdCode = [];
                                        OrdQty = [];

                                    }
                                },
                                error: function (xhr, status, error) {
                                    var err = eval("(" + xhr.responseText + ")");

                                }
                            });   
                            
                        })
              .done(function () {
                 // alert("second success");
              })
              .fail(function () {
                 // alert("error");
              })
              .always(function () {
                
              });


        });
        $("body").on("click", ".remove1", function () {
            $(this).closest("div").remove();
        });
    });
   
    function addJSONOrder(valuesOrder) {
        
      
        var newObject = {
            "Code": valuesOrder
           
        };
        OrdCode.push(newObject);
    }
    function addJSONQty(valuesQty) {
       
        var newObject = {
            "Qty": valuesQty
          
        };
        OrdQty.push(newObject);
    }
    function GetDynamicTextBox(value) {

        return '<input name = "DynamicTextBoxOrd" class="oc_inp" type="text" id="searchtxt'+1+'" value = "' + value + '" />&nbsp;' + '<input name = "DynamicTextBoxOrdQty" class="qty_inp" type="text" value = "' + value + '" />&nbsp;' +
                '<input type="button" value="Remove" style="width:50px; height:20px;outline:none"  class="remove1 ol_delete" />'

        //ImageUrl="~/images/Delete_icon.png" Width="16px" Height="16px" 
    }
    
  
</script>
 