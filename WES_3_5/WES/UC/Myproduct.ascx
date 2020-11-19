<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Myproduct.ascx.cs" Inherits="WES.UC.Myproduct" %>
     <link rel="Stylesheet" href="css/thickboxNew.css" type="text/css" />
       <link rel="Stylesheet" href="../css/jquery-ui.css" type="text/css" />               
       <script language="javascript" src="Scripts/thickboxNew.js" type="text/javascript" />
       <script language="javascript" src="Scripts/jquery-Thickbox-New.js" type="text/javascript" />  
<script src="../Scripts/jquery-1.4.2.min.js" type="text/javascript"></script>
<script src="../Scripts/jquery-1.8.1.min.js" type="text/javascript"></script>

<div>
                <div class="quickorder3">
                                    <H3 class="title1">Please Upload Your  Products</H3>                                    
                                    <P class="p2">Upload your excel file for quick order. Enter the Order Code in column "A" </P>
                                    <p class="pad10" style="padding:0px 0px;">  
                                      <asp:LinkButton id="LinkButton2" 
                                                Text="Click Here to Download example Excel upload order sheet" 
                                                 OnClick="LinkButton_Click"  runat="server" class="toplinkatest" > 
                                                 <div class="toplinkatestbulk"><p style="margin: 0 0 0 21px;width:285px;">Click Here to Download example Excel upload order sheet</p></div>     
                                                       <%-- <div class="toplinkatestbulk"><p style="margin: 0 0 0 21px;width:282px;">Click Here to Download example Excel upload order sheet</p></div>                     --%>
                                                </asp:LinkButton>
                                    </p>
                                    <div>
                                    <asp:FileUpload ID="FileUploadControl" runat="server" CssClass="inputtxtfile"   /> 
                                    <asp:Button runat="server" id="UploadButton" text="Upload" onclick="UploadButton_Click" class="inputupload" style="height:23px;margin-left:0px;border:'1'px solid #465C71;font-family:Arial;"/>
                                    <asp:Label runat="server" id="StatusLabel" text="Upload status: " />
                                    </div>
                                    </div>
        </div>
