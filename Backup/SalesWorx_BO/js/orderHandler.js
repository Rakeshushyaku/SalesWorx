//PlanHandler JavaScript Control......

function OrderHandler()
{
	if (window.OrderHandlerInstance != null)
	{
		window.alert("Only one OrderHandler control is allowed on a page.");
		return;
	}
	window.OrderHandlerInstance = this;
	
	this.getSalesData=OrderHandler_getSalesData;
	this.getBonusData=OrderHandler_getBonusData;
	this.getStockData=OrderHandler_getStockData;
	this.getDefaultBonus=OrderHandler_getDefaultBonus;
	this.getDefaultBonusForReview=OrderHandler_getDefaultBonusForReview;
	this.ParentRef=null;
	this.ResultArea=null;
	this.PathToLoader=null;
	this.popUp=OrderHandler_popUp;
	this.winHnd=null;
}

//Generic Functions START.....

function OrderHandler_getSalesData(CustomerID, ItemID)
{
	var retBool=false;
	try
	{
		var url="";
		
		url=this.PathToLoader+"/_BO_GetSalesData.aspx?CustID="+CustomerID+"&ItemID="+ItemID+"&ref="+(new Date()).toString();

		//alert(url);

		var xmlhttp = null;

		// Mozilla/Safari
		if (window.XMLHttpRequest) 
		{
			xmlhttp = new XMLHttpRequest();
			xmlhttp.open("GET", url, false);
			xmlhttp.send(null);
		}
		// IE
		else if (window.ActiveXObject) 
		{
			xmlhttp = new ActiveXObject("Microsoft.XMLHTTP");
			xmlhttp.open("GET", url, false);
			xmlhttp.send();
		}

		if(xmlhttp.readyState==4)
		{
			if(xmlhttp.status==200)
			{
				var page_response=xmlhttp.responseText;
				//alert(page_response);
				changeInnerHTML(this.ResultArea,page_response);
				retBool=true;
			}
			else
			{
				alert("Error while loading sales data: "+xmlhttp.status+" - "+xmlhttp.statusText);
				alert(xmlhttp.responseText);
			}
		}
		else
		{
			alert("Error while loading sales data: invalid ready state ("+xmlhttp.readyState+")");
		}

		xmlhttp=null;
	}
	catch(ex)
	{
		alert("Error while loading sales data: "+ex.description);
	}
	
	this.ResultArea=null;

	return retBool;
}


function OrderHandler_getBonusData(ItemCode, SalesRepID)
{
	var retBool=false;
	try
	{
		var url="";
		
		url=this.PathToLoader+"/_BO_GetBonusData.aspx?ItemCode="+ItemCode+"&SalesRepID="+SalesRepID+"&ref="+(new Date()).toString();

		//alert(url);

		var xmlhttp = null;

		// Mozilla/Safari
		if (window.XMLHttpRequest) 
		{
			xmlhttp = new XMLHttpRequest();
			xmlhttp.open("GET", url, false);
			xmlhttp.send(null);
		}
		// IE
		else if (window.ActiveXObject) 
		{
			xmlhttp = new ActiveXObject("Microsoft.XMLHTTP");
			xmlhttp.open("GET", url, false);
			xmlhttp.send();
		}

		if(xmlhttp.readyState==4)
		{
			if(xmlhttp.status==200)
			{
				var page_response=xmlhttp.responseText;
				//alert(page_response);
				changeInnerHTML(this.ResultArea,page_response);
				retBool=true;
			}
			else
			{
				alert("Error while loading bonus data: "+xmlhttp.status+" - "+xmlhttp.statusText);
				alert(xmlhttp.responseText);
			}
		}
		else
		{
			alert("Error while loading bonus data: invalid ready state ("+xmlhttp.readyState+")");
		}

		xmlhttp=null;
	}
	catch(ex)
	{
		alert("Error while loading bonus info: "+ex.description);
	}
	
	this.ResultArea=null;

	return retBool;
}


function OrderHandler_getStockData(ItemID, SalesRepID)
{
	var retBool=false;
	try
	{
		var url="";
		
		url=this.PathToLoader+"/_BO_GetStockData.aspx?ItemID="+ItemID+"&SalesRepID="+SalesRepID+"&ref="+(new Date()).toString();

		//alert(url);

		var xmlhttp = null;

		// Mozilla/Safari
		if (window.XMLHttpRequest) 
		{
			xmlhttp = new XMLHttpRequest();
			xmlhttp.open("GET", url, false);
			xmlhttp.send(null);
		}
		// IE
		else if (window.ActiveXObject) 
		{
			xmlhttp = new ActiveXObject("Microsoft.XMLHTTP");
			xmlhttp.open("GET", url, false);
			xmlhttp.send();
		}

		if(xmlhttp.readyState==4)
		{
			if(xmlhttp.status==200)
			{
				var page_response=xmlhttp.responseText;
				//alert(page_response);
				changeInnerHTML(this.ResultArea,page_response);
				retBool=true;
			}
			else
			{
				alert("Error while loading stock data: "+xmlhttp.status+" - "+xmlhttp.statusText);
				alert(xmlhttp.responseText);
			}
		}
		else
		{
			alert("Error while loading stock data: invalid ready state ("+xmlhttp.readyState+")");
		}

		xmlhttp=null;
	}
	catch(ex)
	{
		alert("Error while loading stock data: "+ex.description);
	}
	
	this.ResultArea=null;

	return retBool;
}


function OrderHandler_getDefaultBonus(ItemID, ItemCode, ItemQty)
{
	var retBool=false;
	try
	{
		var url="";
		
		url=this.PathToLoader+"/_BO_GetDefaultBonus.aspx?ItemID="+ItemID+"&ItemCode="+ItemCode+"&ItemQty="+ItemQty+"&ref="+(new Date()).toString();

		//alert(url);

		var xmlhttp = null;

		// Mozilla/Safari
		if (window.XMLHttpRequest) 
		{
			xmlhttp = new XMLHttpRequest();
			xmlhttp.open("GET", url, false);
			xmlhttp.send(null);
		}
		// IE
		else if (window.ActiveXObject) 
		{
			xmlhttp = new ActiveXObject("Microsoft.XMLHTTP");
			xmlhttp.open("GET", url, false);
			xmlhttp.send();
		}

		if(xmlhttp.readyState==4)
		{
			if(xmlhttp.status==200)
			{
				var page_response=xmlhttp.responseText;
				//alert(page_response);
				
				if(page_response.indexOf("[$]")!=-1)
				{
					this.ParentRef.setDefBonusItem(ItemID, page_response);
				}
				else
					this.ParentRef.resetPreviousBonus(ItemID);
					
				retBool=true;
			}
			else
			{
				alert("Error while loading default bonus: "+xmlhttp.status+" - "+xmlhttp.statusText);
				alert(xmlhttp.responseText);
			}
		}
		else
		{
			alert("Error while loading default bonus: invalid ready state ("+xmlhttp.readyState+")");
		}

		xmlhttp=null;
	}
	catch(ex)
	{
		alert("Error while loading default bonus: "+ex.description);
	}
	
	this.ResultArea=null;

	return retBool;
}


function OrderHandler_getDefaultBonusForReview(ItemID, ItemCode, ItemQty)
{
	var retVal=null;
	try
	{
		var url="";
		
		url=this.PathToLoader+"/_BO_GetDefaultBonus.aspx?ItemID="+ItemID+"&ItemCode="+ItemCode+"&ItemQty="+ItemQty+"&ref="+(new Date()).toString();

		//alert(url);

		var xmlhttp = null;

		// Mozilla/Safari
		if (window.XMLHttpRequest) 
		{
			xmlhttp = new XMLHttpRequest();
			xmlhttp.open("GET", url, false);
			xmlhttp.send(null);
		}
		// IE
		else if (window.ActiveXObject) 
		{
			xmlhttp = new ActiveXObject("Microsoft.XMLHTTP");
			xmlhttp.open("GET", url, false);
			xmlhttp.send();
		}

		if(xmlhttp.readyState==4)
		{
			if(xmlhttp.status==200)
			{
				var page_response=xmlhttp.responseText;
				//alert(page_response);
				
				retVal=page_response;
			}
			else
			{
				alert("Error while loading default bonus: "+xmlhttp.status+" - "+xmlhttp.statusText);
				alert(xmlhttp.responseText);
			}
		}
		else
		{
			alert("Error while loading default bonus: invalid ready state ("+xmlhttp.readyState+")");
		}

		xmlhttp=null;
	}
	catch(ex)
	{
		alert("Error while loading default bonus: "+ex.description);
	}
	
	this.ResultArea=null;

	return retVal;
}

//Cross-Browser Compatibility Functions.......
function changeInnerHTML(divId, strText)
{
	var x=null;
	try
	{
		if(document.getElementById) 
		{
			x=document.getElementById(divId);
			x.innerHTML='';
			x.innerHTML=strText;
		}
		else if(document.all) 
		{
			x=document.all[divId];
			x.innerHTML=strText;
		}
		else if(document.layers)
		{
			x=document.layers[divId];
			x.document.open();
			x.document.write(strText);
			x.document.close();
		}
	}
	catch(ex)
	{
		alert("Error in changeInnerHTML(): "+ex.description);
	}
	x=null;
}

function getElementById_New(divId)
{
	if(document.getElementById) 
	{
		return document.getElementById(divId);
	}
	else if(document.all) 
	{
		return document.all[divId];
	}
	else
	{
		return null;
	}
}


function addInnerHTML(divId, strText)
{
	var x=null;
	try
	{
		if(document.getElementById) 
		{
			x=document.getElementById(divId);
			x.insertAdjacentHTML("afterBegin",strText);
		}
		else if(document.all) 
		{
			x=document.all[divId];
			insertAdjacentHTML(x,strText);
		}
		else if(document.layers)
		{
			x=document.layers[divId];
			x.document.open();
			x.document.write(strText);
			x.document.close();
		}
	}
	catch(ex)
	{
		alert("Error in addInnerHTML(): "+ex.description);
	}
	x=null;
}


function OrderHandler_popUp(contentPage, wndName, w, h, scroll) 
{
	var winl = (screen.width - w) / 2;
	var wint = (screen.height - h) / 2;
	var winprops = 'height='+h+',width='+w+',top='+wint+',left='+winl+',scrollbars='+scroll+',resizable=no'
	wndHandle = window.open(contentPage, wndName, winprops)
	if (parseInt(navigator.appVersion) >= 4) { wndHandle.window.focus(); }
	return wndHandle;
}


//Generic Functions END.....

