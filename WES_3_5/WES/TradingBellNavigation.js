//*****************************************************************************
//Created By : J.suthahar/D.Thiyagarajan
//Date       : Nov 30 2007
// Time       : 11.38 pm
// File Name  : TradingBellNavigation.js
// Feature    : Paging / Sorting Using Html Table 
//*****************************************************************************


var table;			
var ttable;				
var tableId;			
var ttableId;				
var tableIdArray = new Array();		
var sortDirArray = new Array();		  
var tableCount = 0;			
var rowArray = new Array();		
var rowIdArray = new Array();		
var rowPropArray = new Array();		
var titleRowArray = new Array();	
var titleInnerHTMLArray = new Array();	
var titleSpanCountArray = new Array();	
var titleRowCellArray = new Array();	
var titleSpanCellArray = new Array();	
var colSpanArray = new Array();		
var colTitleFilled = new Array();	
var sortIndex;				
var nRow, actualNRow, maxNCol;		
var origColor;				
var isIE;			
var separateTitle = false;		
var linkEventString =			
	'onMouseOver=\'setCursor(this);' +
	'setColor(this,"selected");\' ' +
	'onMouseOut=\'setColor(this,"default");\' ' +
	'onClick="setCursorWait(this);setTimeout(\'sortTable(';
var cellPropArray = new Array(		
	"align", "vAlign", "bgColor",
	"noWrap", "width", "height", 
	"borderColor",
	"borderColorLight",
	"borderColorDark");
var colsToIgnoreArray;			
var colsToIgnore;			
var useImg = true;		
var ascChrFile = "Images/sort_up.gif";	
var desChrFile = "Images/sort_down.gif";	
var ascChr = "&uarr;";		
var desChr = "&darr;";		
var useCustomTitleFont = false;	
var titleFont = "Trebuchet MS";	
var selectedColor = "blue";	
var defaultColor = "black";	
var recDelimiter = '|';		
var cellPropDelimiter = ",";	
var updownColor = 'gray';	
var doSortUponLoad = true;	
var defaultSortColumn = 0;	
var useEuroDate = false;	
var highlightSelCell = false;	
var cellHighlightColor = "red";	
function Pager(tableName, itemsPerPage) {
    this.tableName = tableName;
    this.itemsPerPage = itemsPerPage;
    this.currentPage = 1;
    this.pages = 0;
    this.inited = false;
    
    this.showRecords = function(from, to) {        
        var rows = document.getElementById(tableName).rows;
        
        for (var i = 1; i < rows.length; i++) {
            if (i < from || i > to)  
                rows[i].style.display = 'none';
            else
                rows[i].style.display = '';
        }        
    }
    
    this.showPage = function(pageNumber) {
    	if (! this.inited) {
    		alert("not inited");
    		return;
    	}

        var oldPageAnchor = document.getElementById('pg'+this.currentPage);
        oldPageAnchor.className = 'pg-normal';
        
        this.currentPage = pageNumber;
        var newPageAnchor = document.getElementById('pg'+this.currentPage);
        newPageAnchor.className = 'pg-selected';
        
        var from = (pageNumber - 1) * itemsPerPage + 1;
        var to = from + itemsPerPage - 1;        
        this.showRecords(from, to);
    }   
    
    this.prev = function() {
        if (this.currentPage > 1)
            this.showPage(this.currentPage - 1);
    }
    
    this.next = function() {
        if (this.currentPage < this.pages) {
            this.showPage(this.currentPage + 1);
        }
    }                        
    
    this.init = function() {
        var rows = document.getElementById(tableName).rows;
        var records = (rows.length - 1); 
        this.pages = Math.ceil(records / itemsPerPage);
        this.inited = true;
    }

    this.showPageNav = function(pagerName, positionId) {
    	if (! this.inited) {
    		alert("not inited");
    		return;
    	}
    	var element = document.getElementById(positionId);  
        var pagerHtml = '<span onclick="' + pagerName + '.prev();" class="pg-normal">Prev</span> |';
        for (var page = 1; page <= this.pages; page++) 
//        pagerHtml += '<span id="pg' + page + '"></span>';
        pagerHtml += ' <span id="pg' + page + '" class="pg-normal" onclick="' + pagerName + '.showPage(' + page + ');">' + page + '</span> | ';
        pagerHtml += '<span onclick="' + pagerName + '.next();" class="pg-normal">Next</span>';     
        element.innerHTML = pagerHtml;
    }
}

function initTable(obj,ignore)
{
	var tIndex;
if (! checkBrowser()) return;
	colsToIgnore = ignore;
	if (colsToIgnore != null) 
		colsToIgnoreArray = ignore.split(",");
	else
		colsToIgnore = null;
    var countCol;
	var currentCell;
	var nColSpan, nRowSpannedTitleCol, colPos;
	var titleFound = false;
	var skipRow = false;
	var rNRowSpan, rNColSpan, parentNodeName;
	var cCellContent, cCellSetting;
	var sObj;
	var ctable;
	var doneftable = false;
	var cmd;
	var tableLoaded = false;
    if (obj.tagName == "TABLE")
	{
		
		table = obj;
	}
	else
	{
		table = document.getElementById(obj);
		sObj = obj.id + "title";
		ttable = document.getElementById(sObj);
	}

	
	if (table == null) return;


	if (table.tagName != "TABLE") return;

	if (ttable != null && ttable.tagName == "TABLE")
		separateTitle = true;

	
	if (tableId == table.id && table.tainted == false) return;


	tableId = table.id;

	if (separateTitle) ttableId = ttable.id;

	
	maxNCol = table.rows[table.rows.length-1].cells.length;

	
	rowArray = new Array();
	rowIdArray = new Array();
	rowPropArray = new Array();
	colSpanArray = new Array();
	colTitleFilled = new Array();
	titleRowArray = new Array();
	titleInnerHTMLArray = new Array();
	titleSpanCountArray = new Array();
	titleRowCellArray = new Array();
	
	for (var i=0; i<maxNCol; i++)
	{
		if (i == 0) continue;
		colTitleFilled[i] = false;
	}

	// Setting the number of rows
	nRow = table.rows.length;	

	
	if (nRow < 1) return;

	
	actualNRow = 0;			
	rNRowSpan = 0;			
	rNColSpan = 0;			
	nRowSpannedTitleCol = 0;	

	
	for (var i=0; i<nRow; i++)
	{
		if (! separateTitle)
		{
			ctable = table;
		}
		else if (i == 0 && separateTitle && titleFound == false) 
		{
			ctable = ttable;
			nRow = ttable.rows.length + 1;
		}

		if (! doneftable && separateTitle && titleFound)
		{
			ctable = table;
			nRow = table.rows.length;
			i = 0;
			doneftable = true;
		}

		skipRow = false;
		
		if (ctable.rows[i].parentNode != null)
		{
			parentNodeName = ctable.rows[i].parentNode.nodeName;
			parentNodeName.toUpperCase();
			if (parentNodeName == 'THEAD' ||
				parentNodeName == 'TFOOT')
			{
				skipRow = true;
			}
		}	
		nColSpan = 1, colPos = 0;
	
		for (var j=0; j<ctable.rows[i].cells.length; j++)
		{
			
			if (titleFound == false)
			{
				if (ctable.rows[i].cells[j].rowSpan > 1)
				{
					if (ctable.rows[i].cells[j].colSpan < 2)
					{
						titleSpanCellArray[colPos] =
							ctable.rows[i].cells[j];
						titleRowArray[colPos] =
							ctable.rows[i];
						colTitleFilled[colPos] = true;
						nRowSpannedTitleCol++;
					}
					if (ctable.rows[i].cells[j].rowSpan - 1 
						> rNRowSpan)
					{
						rNRowSpan = 
							ctable.
							rows[i].cells[j].
							rowSpan - 1;

						if (ctable.rows[i].
							cells[j].colSpan > 1)
							rNColSpan = 
								rNRowSpan + 1;
					}
				}
			}
			if (ctable.rows[i].cells[j].colSpan > 1 &&
				rNColSpan == 0)
			{ 
				nColSpan = ctable.rows[i].cells[j].colSpan;
				colPos += nColSpan;
			}
			else
			{
				colPos++;
			}		
		}
					
	
		if (titleFound == false && nColSpan == 1 && 
			rNRowSpan == 0 && rNColSpan == 0)
		{
			colSpanArray[i] = true;
			titleFound = true;

			
			countCol = 0;
			for (var j=0;
				j<ctable.rows[i].cells.length
					+ nRowSpannedTitleCol; j++)
			{
				if (colTitleFilled[j] != true)
				{
					titleRowCellArray[j] =
						ctable.rows[i].cells[countCol];
					titleRowArray[j] =
						ctable.rows[i];
					countCol++;
				}
				else
				{
					titleRowCellArray[j] = 
						titleSpanCellArray[j];
					
				}
				titleInnerHTMLArray[j] =
					String(titleRowCellArray[j].innerHTML);
				titleSpanCountArray[j] = 
					titleRowCellArray[j].rowSpan;
			}
		}
	
		else if (titleFound == true && nColSpan == 1 && 
			rNRowSpan == 0 && !skipRow)
		{
			for (var j=0; j<ctable.rows[i].cells.length; j++)
			{
				
				if (ctable.rows[i].cells[j].rowSpan > 1) return;

				currentCell = ctable.rows[i].cells[j];
				cCellContent = String(currentCell.innerHTML);
				for (var k=0; k<cellPropArray.length; k++)
				{
					if (k == 0)
						cmd = "cCellSetting=" +
							"String(currentCell." +
							cellPropArray[k] + ");"
					else
						cmd = "cCellSetting+=" +
							"cellPropDelimiter+" +
							"String(currentCell." +
							cellPropArray[k] + ");"
					eval(cmd);
				}

				if (j == 0)
				{
					rowArray[actualNRow] = cCellContent;
					rowPropArray[actualNRow] = cCellSetting;
				}
				else
				{
					rowArray[actualNRow] += recDelimiter +
						cCellContent;
					rowPropArray[actualNRow] += 
						recDelimiter + cCellSetting;
				}
				if (j == ctable.rows[i].cells.length-1)
					rowArray[actualNRow] += recDelimiter +
						String(actualNRow);
			}
			
			if (ctable.rows[i].cells.length > maxNCol)
				return;
			actualNRow++;
			colSpanArray[i] = false;
		}
		else if (nColSpan == 1 && rNRowSpan == 0 && 
			rNColSpan == 0 && titleFound == false && !skipRow)
		{
			colSpanArray[i] = false;
		}
		else
		{
			colSpanArray[i] = true;
		}
		
		
		if (rNRowSpan > 0) rNRowSpan--;
		if (rNColSpan > 0) rNColSpan--;
	}

	
	if (actualNRow < 1) return;

	
	tIndex = findTableIndex(table.id);
	if (tIndex >= 0) tableLoaded = true;
	if (!tableLoaded)
	{
		tableIdArray[tableCount] = table.id;
		sortDirArray[tableCount] = false;
		tIndex = tableCount;
		tableCount++;
	}


	redrawTitle(false);


	if (doSortUponLoad) 
	{
		sortTable(defaultSortColumn, table.id, 0);
		if (!tableLoaded) sortDirArray[tIndex] = true;
	}
}


function redrawTitle(isSort)
{
	var currentRow, innerHTML, newInnerHTML, cellIndex;
	var reAnchor, reUpDown, reLabel, cellAlign, makeBold;
	var cellOnmouseover, cellOnmouseout;
 	var cellClass;
	var cmd;
	var cCellPropArray = new Array();
	var tIndex;

	cellAlign = "";
	makeBold = false;
	reAnchor = / *\<a[^\>]*\>(.*) *\<\/a\>/i;
	reUpDown = /\<font[^\>]* *id=.*updown.*[^\>]*\>.*\<\/font\>/i;
	reLabel = /\>([^\<]*)\</g;

	tIndex = findTableIndex(table.id);

	
	for (var j=0; j<maxNCol; j++)
	{
		currentRow = titleRowArray[j];
		innerHTML = String(titleInnerHTMLArray[j]);
		cellIndex = titleRowCellArray[j].cellIndex;

		
		if (colsToIgnore != null)
			if (indexExist(colsToIgnoreArray,cellIndex)) continue;

		
		for (var k=0; k<cellPropArray.length; k++)
		{
			cmd = "cCellPropArray[" + k + 
				"]=titleRowCellArray[j]." + 
				cellPropArray[k] + ";";
			eval(cmd);
		}
		cellClass = titleRowCellArray[j].getAttribute('className');

		currentRow.deleteCell(cellIndex);
		currentRow.insertCell(cellIndex);

		
		if (cellAlign != "")
			currentRow.cells[cellIndex].align =
				cellAlign;
		if (titleRowCellArray[j].tagName == "TH")
		{
			makeBold = true;
		}

		
		for (var k=0; k<cellPropArray.length; k++)
		{
			cmd = "currentRow.cells[cellIndex]." +
				cellPropArray[k] +
				"=cCellPropArray[" + k + "];";
			eval(cmd);
		}

		currentRow.cells[cellIndex].setAttribute(
			'className', cellClass, 0);

		if (titleSpanCountArray[j] > 1)
			currentRow.cells[cellIndex].rowSpan = 
				titleSpanCountArray[j];
		newTitle = '';
		if (j == sortIndex && isSort)
		{
			newTitle = '<font id=updown color=' + 
				updownColor + '>&nbsp;';
			if (sortDirArray[tIndex])
				if (useImg)
					newTitle += '<img src="' +
						desChrFile + '" alt="' +
						desChr + '">';
				else
					newTitle += desChr;
			else
				if (useImg)
					newTitle += '<img src="' +
						ascChrFile + '" alt="' +
						ascChr + '">';
				else
					newTitle += ascChr;
			newTitle += '</font>';
		} 
		
		innerHTML = innerHTML.replace(/\r|\n|\t/g, "");
		if (makeBold)
		{
			if (innerHTML.match(reLabel))
				innerHTML = 
					innerHTML.replace(reLabel, "<b>$1</b>");
			else
				innerHTML =
					innerHTML.replace(
						/(^.*$)/, "<b>$1</b>");
		}
		innerHTML = innerHTML.replace(reUpDown, "");
		innerHTML = innerHTML.replace(reAnchor, "$1");
		newInnerHTML =
			'<a ' + linkEventString + j + ',' +
			tIndex + ',' + 1 + ',' + 1 + ')\', 1);">';
		if (useCustomTitleFont)
			newInnerHTML += '<font face="' + titleFont + '">' +
				+ innerHTML + '</font>';
		else
			newInnerHTML += innerHTML;
		newInnerHTML += '</a>' + newTitle;

		currentRow.cells[cellIndex].innerHTML = newInnerHTML;
		titleRowCellArray[j] = currentRow.cells[cellIndex];
	}
}


function sortTable(index,tobj,doInit,isIndex)
{
	var obj, tIndex;
	if (isIndex != null)
		if (isIndex)
		{
			tIndex = tobj;
			
			obj = tableIdArray[tobj];
		}	
		else
		{
			tIndex = findTableIndex(tobj.id);
			obj = tobj;
		}

	
	if (doInit) 
	{
		if (colsToIgnore != null) 
			initTable(obj,colsToIgnore);
		else
			initTable(obj);
	}


	var rowContent, rowProp, cellProp;
	var rowCount;
	var rowIndex;
 	var cellClass, cellOnClick, cellMouseOver, cellMouseOut;
	var cmd;
	

	if (index < 0 || index >= maxNCol) return;
	
	
	sortIndex = index;
	
	rowArray.sort(compare);

	
	if (doInit || doSortUponLoad) redrawTitle(true);

	
	rowCount = 0;
	for (var i=0; i<nRow; i++)
	{
		if (! colSpanArray[i])
		{
			for (var j=0; j<maxNCol; j++)
			{
				
				if (colsToIgnore != null)
					if (indexExist(colsToIgnoreArray,j)) 
						continue;
				rowContent = rowArray[rowCount].
					split(recDelimiter);
				rowIndex = rowContent[maxNCol];
				rowProp = rowPropArray[rowIndex].
					split(recDelimiter);
				cellProp = rowProp[j].split(cellPropDelimiter);
				cellClass = 
					table.rows[i].cells[j].getAttribute(
						'className');
 				cellOnClick = table.rows[i].cells[j].onclick;
 				cellMouseOver = 
 					table.rows[i].cells[j].onmouseover;
 				cellMouseOut = 
 					table.rows[i].cells[j].onmouseout;

				table.rows[i].deleteCell(j);
				table.rows[i].insertCell(j);

				for (var k=0; k<cellPropArray.length; k++)
				{
					cmd = "table.rows[i].cells[j]." +
						cellPropArray[k] +
						"=cellProp[" + k + "];";
					eval(cmd);
				}

				table.rows[i].cells[j].innerHTML =
					rowContent[j];
				table.rows[i].cells[j].setAttribute(
					'className', cellClass, 0);
 				table.rows[i].cells[j].onclick = cellOnClick;
 				table.rows[i].cells[j].onmouseover = 
 					cellMouseOver;
 				table.rows[i].cells[j].onmouseout =
 					cellMouseOut;
			}
			rowCount++;
		}
	}


	if (doInit)
	{
		if (sortDirArray[tIndex])
			sortDirArray[tIndex] = false;
		else
			sortDirArray[tIndex] = true;
	}
	setCursorDefault();
}


function compare(a, b)
{
	var tIndex;
	tIndex = findTableIndex(table.id);
 
	
	var aRowContent = a.split(recDelimiter);
	var bRowContent = b.split(recDelimiter);
	
	
	var aToBeCompared, bToBeCompared;

	
	reRowText = /(\< *[^\>]*\>|\&nbsp\;)/g;
	aRowContent[sortIndex] = aRowContent[sortIndex].replace(reRowText, "");
	bRowContent[sortIndex] = bRowContent[sortIndex].replace(reRowText, "");

	
	if (useEuroDate)
	{
		aRowContent[sortIndex] = 
			convertEuroDate(aRowContent[sortIndex]);
		bRowContent[sortIndex] = 
			convertEuroDate(bRowContent[sortIndex]);
	}

	if (isDate(aRowContent[sortIndex]) && isDate(bRowContent[sortIndex]))
	{
		aToBeCompared = new Date(aRowContent[sortIndex]);
		bToBeCompared = new Date(bRowContent[sortIndex]);
	}
	else if (! isNaN(aRowContent[sortIndex]) &&
		! isNaN(bRowContent[sortIndex]))
	{
		aToBeCompared = parseFloat(aRowContent[sortIndex], 10);
		bToBeCompared = parseFloat(bRowContent[sortIndex], 10);
	}
	else
	{
		aToBeCompared = aRowContent[sortIndex];
		bToBeCompared = bRowContent[sortIndex];
	}

	if (aToBeCompared < bToBeCompared)
		if (!sortDirArray[tIndex])
		{
			return -1;
		}
		else
		{
			return 1;
		}
	if (aToBeCompared > bToBeCompared)
		if (!sortDirArray[tIndex])
		{
			return 1;
		}
		else
		{
			return -1;
		}
	return 0;
}


function isDate(x)
{
	var xDate;
	xDate = new Date(x);
	if (xDate.toString() == 'NaN' || 
		xDate.toString() == 'Invalid Date')
		return false;
	else
		return true;
}


function convertEuroDate(x)
{
	var reExp = /^ *(\d{2})\-(\d{2})\-(\d{4}) *$/g;
	var dArray;
	if (x.match(reExp))
		x = x.replace(reExp, "$2\/$1\/$3");	
	return(x);
}


function setCursor(obj)
{
	var rowText, reRowText;

	reRowText = /(\< *[^\>]*\>|\&nbsp\;)/g;
	
	rowText = String(obj.innerHTML);


	rowText = rowText.replace(/\<font id\=updown.*\<\/font\>/ig, "");

	rowText = rowText.replace(reRowText, "");
	
	rowText = rowText.replace(/\r|\n|\t/g, "");
	
	
	window.status = "Click to sort by " + String(rowText);

	
	obj.title = "Click to sort by " + String(rowText);

	if (isIE)
		obj.style.cursor = "hand";
	else
		obj.style.cursor = "pointer";
}

function setCursorWait(obj)
{
	if (isIE && document.all)
		for (var i=0;i<document.all.length;i++) 
			document.all(i).style.cursor = 'wait';
	else
	{
		obj.style.cursor = 'wait';
		document.body.style.cursor = 'wait';
	}
}

function setCursorDefault()
{
	if (isIE && document.all)
		for (var i=0;i<document.all.length;i++) 
			document.all(i).style.cursor = '';
	else
		document.body.style.cursor = '';
}


function setColor(obj,mode)
{
	if (mode == "selected")
	{
		
		if (obj.style.color != selectedColor) 
			defaultColor = obj.style.color;
		obj.style.color = selectedColor;

		if (highlightSelCell)
			obj.bgColor = cellHighlightColor;
	}
	else
	{	
	
		obj.style.color = defaultColor;
		if (highlightSelCell) obj.bgColor = defaultColor;
		window.status = '';
	}
}


function checkBrowser()
{
	if (navigator.appName == "Microsoft Internet Explorer"
		&& parseInt(navigator.appVersion) >= 4)
	{
		isIE = true;
		return true;
	}

	else if (navigator.appName == "Netscape"
		&& navigator.appVersion.indexOf("5.") >= 0)
	{
		isIE = false;
		return true;
	}
	else
		return false;
}


function indexExist(x, index)
{
	var found = false;
	for (var i=0; i<x.length; i++)
	{
		if (index == x[i])
		{
			found = true;
			break;
		}
	}
	return found;
}

function findTableIndex(id)
{
	for (var i=0; i<tableCount; i++)
		if (tableIdArray[i] == id)
			return(i);
	return(-1);
}

//*****************************************************************************
// Paging User  Manuval 
//*****************************************************************************
// Script Structure   <script type="text/javascript" src="TradingBellNavigation.js"></script>
// Table Structure    <table id ="TableName> <tr><td> Title Of Content</td></tr><tr><td>value</td></td></table>
// Bottom Of the  Page write This code   
// <div id="pageNavPosition"></div>
 //    <script type="text/javascript">
  //      var pager = new Pager('results', 3); 
  //      pager.init(); 
  //      pager.showPageNav('pager', 'pageNavPosition'); 
  //      pager.showPage(1);
  // </script>
//*****************************************************************************
// Sorting  User  Manuval
//***************************************************************************** 
// Script Structure  <BODY onLoad='initTable("Table Id"); <script type="text/javascript" src="TradingBellNavigation.js"></script>
// Table Structure    <table id ="TableName> <tr><td> Title Of Content</td></tr><tr><td>value</td></td></table>
//*****************************************************************************
//Any Bugs & Command 
//suthaharj@tradingbell.com
//thiyagarajand@tradingbell.com