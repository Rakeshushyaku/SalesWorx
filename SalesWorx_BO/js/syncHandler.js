//PlanHandler JavaScript Control......

function SyncHandler()
{
	if (window.SyncHandlerInstance != null)
	{
		window.alert("Only one SyncHandler control is allowed on a page.");
		return;
	}
	window.SyncHandlerInstance = this;
	
	this.syncO2B=SyncHandler_syncO2B;
	this.syncB2H=SyncHandler_syncB2H;
	this.syncH2B=SyncHandler_syncH2B;
	this.syncB2O=SyncHandler_syncB2O;		
	this.setProcessList=SyncHandler_ProcessList;
	this.ParentRef=null;
	this.CurrentProcess=0;
	this.ProcessDesc=null;
	this.ProcessList=null;
	this.ResultArea=null;
	this.Delay=100;
	this.PathToLoader=null;
	this.popUp=SyncHandler_popUp;
	this.winHnd=null;
}

//Generic Functions START.....

function SyncHandler_syncO2B(SyncData)
{
	var retBool=false;
	try
	{
		var url="";
		
		url=this.PathToLoader+"/_BO_SyncHandler_O2B.aspx?SD="+this.ProcessList[this.CurrentProcess]+"&ref="+(new Date()).toString();

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
				addInnerHTML(this.ResultArea,page_response);
				retBool=true;
			}
			else
			{
				alert("Error while loading sync: "+xmlhttp.status+" - "+xmlhttp.statusText);
				alert(xmlhttp.responseText);
			}
		}
		else
		{
			alert("Error while loading sync: invalid ready state ("+xmlhttp.readyState+")");
		}

		xmlhttp=null;
	}
	catch(ex)
	{
		alert("Error while loading sync: "+ex.description);
	}

	this.CurrentProcess++;
	if(this.CurrentProcess<this.ProcessList.length)
	{
		this.ParentRef.setTimeout('sh.syncO2B()',this.Delay);
		addInnerHTML(this.ResultArea,"<B>Please wait, sync process underway....</B><BR><BR>");
	}
	else
	{
		this.ProcessList=null;
		this.CurrentProcess=0;
		
		addInnerHTML(this.ResultArea,("<B>Completed "+this.ProcessDesc+" Process @ "+(new Date()).toString())+"</B><BR><BR>");
		
		if(confirmSwitchOver())
			enableInputs(true);
	}

	return retBool;
}

function SyncHandler_syncB2H(SyncData)
{
	var retBool=false;
	try
	{
		var url="";
		
		url=this.PathToLoader+"/_BO_SyncHandler_B2H.aspx?SD="+this.ProcessList[this.CurrentProcess]+"&ref="+(new Date()).toString();

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
				addInnerHTML(this.ResultArea,page_response);
				retBool=true;
			}
			else
			{
				alert("Error while loading sync: "+xmlhttp.status+" - "+xmlhttp.statusText);
				alert(xmlhttp.responseText);
			}
		}
		else
		{
			alert("Error while loading sync: invalid ready state ("+xmlhttp.readyState+")");
		}

		xmlhttp=null;
	}
	catch(ex)
	{
		alert("Error while loading sync: "+ex.description);
	}

	this.CurrentProcess++;
	if(this.CurrentProcess<this.ProcessList.length)
	{
		this.ParentRef.setTimeout('sh.syncB2H()',this.Delay);
		addInnerHTML(this.ResultArea,"<B>Please wait, sync process underway....</B><BR><BR>");
	}
	else
	{
		this.ProcessList=null;
		this.CurrentProcess=0;

		addInnerHTML(this.ResultArea,("<B>Completed "+this.ProcessDesc+" Process @ "+(new Date()).toString())+"</B><BR><BR>");

		if(confirmSwitchOver())
			enableInputs(true);

	}

	return retBool;
}


function SyncHandler_syncH2B(SyncData)
{
	var retBool=false;
	try
	{
		var url="";
		
		url=this.PathToLoader+"/_BO_SyncHandler_H2B.aspx?SD="+this.ProcessList[this.CurrentProcess]+"&ref="+(new Date()).toString();

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
				addInnerHTML(this.ResultArea,page_response);
				retBool=true;
			}
			else
			{
				alert("Error while loading sync: "+xmlhttp.status+" - "+xmlhttp.statusText);
				alert(xmlhttp.responseText);
			}
		}
		else
		{
			alert("Error while loading sync: invalid ready state ("+xmlhttp.readyState+")");
		}

		xmlhttp=null;
	}
	catch(ex)
	{
		alert("Error while loading sync: "+ex.description);
	}

	this.CurrentProcess++;
	if(this.CurrentProcess<this.ProcessList.length)
	{
		this.ParentRef.setTimeout('sh.syncH2B()',this.Delay);
		addInnerHTML(this.ResultArea,"<B>Please wait, sync process underway....</B><BR><BR>");
	}
	else
	{
		this.ProcessList=null;
		this.CurrentProcess=0;

		addInnerHTML(this.ResultArea,("<B>Completed "+this.ProcessDesc+" Process @ "+(new Date()).toString())+"</B><BR><BR>");

		if(confirmSwitchOver())
			enableInputs(true);

	}

	return retBool;
}


function SyncHandler_syncB2O(SyncData)
{
	var retBool=false;
	try
	{
		var url="";
		
		url=this.PathToLoader+"/_BO_SyncHandler_B2O.aspx?SD="+this.ProcessList[this.CurrentProcess]+"&ref="+(new Date()).toString();

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
				addInnerHTML(this.ResultArea,page_response);
				retBool=true;
			}
			else
			{
				alert("Error while loading sync: "+xmlhttp.status+" - "+xmlhttp.statusText);
				alert(xmlhttp.responseText);
			}
		}
		else
		{
			alert("Error while loading sync: invalid ready state ("+xmlhttp.readyState+")");
		}

		xmlhttp=null;
	}
	catch(ex)
	{
		alert("Error while loading sync: "+ex.description);
	}

	this.CurrentProcess++;
	if(this.CurrentProcess<this.ProcessList.length)
	{
		this.ParentRef.setTimeout('sh.syncB2O()',this.Delay);
		addInnerHTML(this.ResultArea,"<B>Please wait, sync process underway....</B><BR><BR>");
	}
	else
	{
		this.ProcessList=null;
		this.CurrentProcess=0;
		
		addInnerHTML(this.ResultArea,("<B>Completed "+this.ProcessDesc+" Process @ "+(new Date()).toString())+"</B><BR><BR>");
		
		if(confirmSwitchOver())
			enableInputs(true);
	}

	return retBool;
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


function SyncHandler_popUp(contentPage, wndName, w, h, scroll) 
{
	var winl = (screen.width - w) / 2;
	var wint = (screen.height - h) / 2;
	var winprops = 'height='+h+',width='+w+',top='+wint+',left='+winl+',scrollbars='+scroll+',resizable=no'
	wndHandle = window.open(contentPage, wndName, winprops)
	if (parseInt(navigator.appVersion) >= 4) { wndHandle.window.focus(); }
	return wndHandle;
}


//Generic Functions END.....

function confirmSwitchOver()
{
	return (confirm("Synchronization process completed.\n\nClick 'OK' to return back to the synchronization controls, or hit 'Cancel' to continue viewing the sync log."));
}


function SyncHandler_ProcessList(ProcessArr)
{
	this.ProcessList=ProcessArr;
	changeInnerHTML(this.ResultArea,("<B>Started "+this.ProcessDesc+" Process @ "+(new Date()).toString()+"</B>"));
}