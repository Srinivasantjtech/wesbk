//AJAXroutine - ajax common routine for the tree view implementation
//v1.01- Author: AJFK (http://www.alhamdgroup.com)
//Last updated: November 22th 06
<!--
 
var xmlHttp;
/***********************************************************
//Builds the query string and invoke a request to the server 
//using javascript async call
/***********************************************************/
function showContentsEx(div, str, lmsg){	
	if (div == null) return;
	divObj = div;
		
		
	var objDivContainer = document.getElementById(eval("'"+div+"'"));
	 
	if (objDivContainer == null) return;
	
	if (str == "") {
		objDivContainer.innerHTML= "";		
		return;
	}
	//else
	//show the processing image to the user
	objDivContainer.innerHTML = "<img src='images/loading.gif' border='0'/>";
	
	//get the xml http object
	xmlHttp=GetXmlHttpObject()
	if (xmlHttp==null){
		alert ("Browser does not support HTTP Request")
		return
	} 
	
	//build the processing page and it's query string
	//invoke a request to the server using javascript async call
	var url="GetTreeContents.aspx?q="+str+"&d="+lmsg;
	url=url+"&sid="+Math.random();					
	xmlHttp.onreadystatechange=stateChangedEx	
	xmlHttp.open("GET",url,true)
	xmlHttp.send(null)
	/**/
}

//The extented method to check the http request state has changed
//Then the dynamic div tag will be filled with the response text
function stateChangedEx() { 
	if (xmlHttp.readyState==4 || xmlHttp.readyState=="complete")
	{ 	  
		document.getElementById(eval("'"+divObj+"'")).innerHTML=xmlHttp.responseText;		
	} 
} 

//The core function to get the xml http request object
function GetXmlHttpObject(){ 
	var objXMLHttp=null
	if (window.XMLHttpRequest){
		objXMLHttp=new XMLHttpRequest()
	}
	else if (window.ActiveXObject) {
		objXMLHttp=new ActiveXObject("Microsoft.XMLHTTP")
	}
	return objXMLHttp
}

//To toggle the node image and expand collapse
function Toggle(node){

  var nodparent=node.parentNode;
            if (node.nodeName == "IMG")
			{
			
				if(node.src.indexOf("rightarrow.gif")> -1)
				{
						
					nodparent.nextSibling.style.display = 'block';
					node.src = "images/leftarrow.gif";
					return true;
				}
				else 
				{
//				 alert('toggle check');
					nodparent.nextSibling.style.display = 'none';
					node.src = "images/rightarrow.gif";
					return false;
				}				 
			}
			function Togglenew1(node)
			{
                alert('test');
 	            if (node.nextSibling.style)	
 	            {		
		            if (node.childNodes.length > 0)	
		            {
			            if (node.childNodes.item(0).nodeName == "IMG")
			            {
			                    if(node.childNodes.item(0).src.indexOf("images/rightarrow.gif")> -1)
			                    {
            				
				                    node.nextSibling.style.display = 'block';
				                    node.childNodes.item(0).src = "images/leftarrow.gif";
				                    return true;
			                    }
			                    else 
			                    {
            			
				                    node.nextSibling.style.display = 'none';
				                    node.childNodes.item(0).src = "images/rightarrow.gif";
				                    return false;
			                    }				 
			            }
    	            }		
	            }	 
			}
 
	// Unfold the branch if it isn't visible
//	if (node.nextSibling.style)	{		
//		if (node.childNodes.length > 0)	{
//			if (node.childNodes.item(0).nodeName == "IMG"){
//				if(node.childNodes.item(0).src.indexOf("images/rightarrow.gif")> -1){
//					
//					node.nextSibling.style.display = 'block';
//					node.childNodes.item(0).src = "images/leftarrow.gif";
//					return true;
//				}
//				else {
//				
//					node.nextSibling.style.display = 'none';
//					node.childNodes.item(0).src = "images/rightarrow.gif";
//					return false;
//				}				 
//			}
//		}		
//	}
}

    
    function GetTreecategoryid(categoryID)
    {    
     //alert(categoryID);
     document.getElementsByName("ctl00$rightnav$ctl00$hdncategoryId").value=categoryID;
     //alert( document.getElementsByName("ctl00$rightnav$ctl00$hdncategoryId").value);
    }
    
//-->
