//PlanHandler JavaScript Control......

function PlanHandler()
{
	if (window.PlanHandlerInstance != null)
	{
		window.alert("Only one PlanHandler control is allowed on a page.");
		return;
	}
	window.PlanHandlerInstance = this;
	
	this.setCalColors=PlanHandler_setCalColors;
	this.loadDefaultPlan=PlanHandler_loadDefaultPlan;
	this.setPlanValues=PlanHandler_setPlanValues;
	this.setRoutePlanValues=PlanHandler_setRoutePlanValues;
	this.setCustVisitDays=PlanHandler_setCustVisitDays;	
	this.setVisitDays=PlanHandler_setVisitDays;	
	this.getArrDates=PlanHandler_getArrDates;
	this.getArrComments=PlanHandler_getArrComments;	
	this.checkAndUpdatePlan=PlanHandler_checkAndUpdatePlan;
	this.editDay=PlanHandler_editDay;
	this.editRouteDay=PlanHandler_editRouteDay;	//for route setup
	this.setDay=PlanHandler_setDay;
	this.setRouteDay=PlanHandler_setRouteDay
	this.existsInArray=PlanHandler_existsInArray;
	this.getDateLimits=PlanHandler_getDateLimits;	
	this.popUp=PlanHandler_popUp;
	this.defBGColor="#FFFFFF";
	this.offBGColor="#CCCCCC";
	this.onBGColor="#EEEEEE";
	this.weekendBGColor="#E1E1E1";
	this.isRouteSetup=false;
	this.winHnd=null;
	this.startDate=null;
	this.endDate=null;
	this.enablePlanEdit=true;
	this.arrCustVisits=null;
	this.routeID=null;
	this.defplanID=null;
}

//Generic Functions START.....

function PlanHandler_loadDefaultPlan(RP_ID, Start_Date, End_Date, PathToLoader,IsRouteSetup, Route_ID, IsRefresh, ActionMode)
{
	var retBool=false;
	try
	{
		var url="";
		
		if(IsRouteSetup=="Y")
		{
			url=PathToLoader+"/_BO_FetchPlan.aspx?RP_ID="+RP_ID+"&Route_ID="+Route_ID+"&IRS=Y&Action="+ActionMode+"&ref="+(new Date()).toString();
		}
		else
		{
			url=PathToLoader+"/_BO_FetchPlan.aspx?RP_ID="+RP_ID+"&Start_Date="+Start_Date+"&End_Date="+End_Date+"&IsRefresh="+IsRefresh+"&ref="+(new Date()).toString();
		}

		//alert(url);

		var xmlhttp = null;

		// Mozilla/Safari
		if (window.XMLHttpRequest) 
		{
			xmlhttp = new XMLHttpRequest();
			//xmlhttp.overrideMimeType('text/xml');
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
				if(page_response=="DAY_COUNT_INVALID")
				{
					alert("Specified date range exceeds max limit of 31 days.");
					retBool=false;					
				}
				else if(page_response=="DATE_RANGE_INVALID")
				{
					alert("A plan with the specified date range already exists.");
					retBool=false;
				}
				else if(page_response=="SDATE_LT_EQ_EDATE")
				{
					alert("End date should be greater than start date.");
					retBool=false;
				}
				else
				{
					//alert(page_response);
					changeInnerHTML("TD_PlanArea",page_response);
					retBool=true;
				}
			}
			else
			{
				alert("Error while loading plan: "+xmlhttp.status+" - "+xmlhttp.statusText);
				alert(xmlhttp.responseText);
			}
		}
		else
		{
			alert("Error while loading plan: invalid ready state ("+xmlhttp.readyState+")");
		}

		xmlhttp=null;
	}
	catch(ex)
	{
		alert("Error while loading plan: "+ex.description);
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


function changeTitle(divId, strText)
{
	var x=null;
	try
	{
		if(document.getElementById) 
		{
			x=document.getElementById(divId);
			x.title=strText;
		}
		else if(document.all) 
		{
			x=document.all[divId];
			x.title=strText;
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
		alert("Error in changeTitle(): "+ex.description);
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


function PlanHandler_popUp(contentPage, wndName, w, h, scroll) 
{
	var winl = (screen.width - w) / 2;
	var wint = (screen.height - h) / 2;
	var winprops = 'height='+h+',width='+w+',top='+wint+',left='+winl+',scrollbars='+scroll+',resizable=no'
	wndHandle = window.open(contentPage, wndName, winprops)
	if (parseInt(navigator.appVersion) >= 4) { wndHandle.window.focus(); }
	return wndHandle;
}


//Generic Functions END.....


function PlanHandler_setCalColors(defColor, offColor, onColor, weekendColor)
{
	this.defBGColor=defColor;
	this.offBGColor=offColor;
	this.onBGColor=onColor;
	this.weekendBGColor=weekendColor;
}


function PlanHandler_setPlanValues()
{
	var objRef="Day";
	var planValues="";
	var objDay;
	var objDayMsg;
	var i=1;
	var WorkingDays=0;
	
	for(i=1;i<32;i++)
	{
		objDay=getElementById_New(objRef+i);
		objDayMsg=getElementById_New("DIV_"+objRef+i);
		if(objDay!=null && objDayMsg!=null)
		{
			if(objDay.value=="W")
			{
				getElementById_New(("HDay"+i)).value="W";
			}
			else if(objDay.value=="O")
			{
				getElementById_New(("HDay"+i)).value="O|"+objDayMsg.innerHTML;
			}
			else if(objDay.value=="U")
			{
				getElementById_New(("HDay"+i)).value="U|"+objDayMsg.innerHTML;
				WorkingDays++;
			}
			else
			{
				getElementById_New(("HDay"+i)).value="NULL";
				WorkingDays++;
			}
		}
		else
			getElementById_New(("HDay"+i)).value="NULL";
		
		objDay=null;
		objDayMsg=null;
	}
	
	getElementById_New("HWorkingDays").value=WorkingDays;	
}


function PlanHandler_getDateLimits()
{
	this.startDate=null;
	this.endDate=null;
	
	var objDay=getElementById_New("Day1");
	
	if(objDay!=null)
	{
		this.startDate=objDay.date;
		this.endDate=this.startDate;
	}
	
	for(i=2;i<32;i++)
	{
		objDay=getElementById_New("Day"+i);

		if(objDay!=null)
			this.endDate=objDay.date;		
		else
			break;
		
		objDay=null;
	}
}


function PlanHandler_getArrDates()
{
	objRef="Day";
	var objDay;
	var objDayMsg;
	var arrDates=new Array();
	var arrCounter=0;
	var i=1;
	for(i=1;i<32;i++)
	{
		objDay=getElementById_New(objRef+i);
		objDayMsg=getElementById_New("DIV_"+objRef+i);
		if(objDay!=null && objDayMsg!=null)
		{
			if(objDay.value=="O")
			{
				arrDates[arrCounter]=objDay.date;
				arrCounter++;
			}
		}
		objDay=null;
		objDayMsg=null;
	}
	return arrDates;
}

function PlanHandler_getArrComments()
{
	objRef="Day";
	var objDay;
	var objDayMsg;
	var arrComments=new Array();
	var arrCounter=0;
	var i=1;
	for(i=1;i<32;i++)
	{
		objDay=getElementById_New(objRef+i);
		objDayMsg=getElementById_New("DIV_"+objRef+i);
		if(objDay!=null && objDayMsg!=null)
		{
			if(objDay.value=="O")
			{
				arrComments[arrCounter]=objDayMsg.innerHTML;
				arrCounter++;
			}
		}
		objDay=null;
		objDayMsg=null;
	}
	return arrComments;
}

function PlanHandler_checkAndUpdatePlan(arrDates,arrComments)
{
	objRef="Day";
	var objDay;
	var objDayMsg;
	var arrCounter=-1;
	var x=1;
	for(x=1;x<32;x++)
	{
		objDay=getElementById_New(objRef+x);
		objDayMsg=getElementById_New("DIV_"+objRef+x);
		if(objDay!=null && objDayMsg!=null)
		{
			if(objDay.value!="W")
			{
				arrCounter=this.existsInArray(arrDates,objDay.date);
				
				if(arrCounter>=0)
					this.setDay((objRef+x),"O",arrComments[arrCounter]);
			}
		}
	}
}

function PlanHandler_existsInArray(ElementArray,Element)
{
	var j=0;
	for(j=0;j<ElementArray.length;j++)
	{
		if(ElementArray[j]==Element)
			return j;
	}
	return -1;
}


function PlanHandler_editDay(objRef)
{
	if(this.isRouteSetup)//if route plan is being setup then use other func.....
	{
		if(this.enablePlanEdit)
		{
			this.editRouteDay(objRef)
			return;
		}
		else
		{
			if(document.getElementById(objRef).isupdatable=="Y")
			{
				this.editRouteDay(objRef)
				return;
			}
			else
				alert("This plan is already approved and cannot be updated.");
		}
	}
	else//if def plan is being setup....
	{
		if(this.enablePlanEdit)
		{
			var user_comments, day_box, comment_area, pre_text;

			day_box=getElementById_New(objRef);
			comment_area=getElementById_New("DIV_"+objRef);
			
			if(day_box!=null && comment_area!=null)
			{
				day_value=day_box.value;
				user_comments=comment_area.innerHTML;
				if(day_value=="O")	//if editing a day-off box...
				{
					qryStr="_POP_CustomerList.aspx?irs=N&dr="+objRef+"&dv="+day_value+"&uc="+user_comments;
				}
				else	//if anything else....
				{
					qryStr="_POP_CustomerList.aspx?irs=N&dr="+objRef+"&dv="+day_value;
				}
				this.winHnd=this.popUp(qryStr,'CustWindow',600,200,'yes');
			}
		}
		else
			alert("This plan is in use and cannot be updated.");
	}
}

function PlanHandler_editRouteDay(objRef)
{
	var day_box, day_value, user_comments, comment_area, custinfo_area, qryStr;
	day_box=getElementById_New(objRef);
	comment_area=getElementById_New("DIV_"+objRef);
	custinfo_area=getElementById_New("DIV_C_"+objRef);
	
	if(day_box!=null && comment_area!=null && custinfo_area!=null)
	{
		day_value=day_box.value;
		user_comments=comment_area.innerHTML;
		if(day_value=="L")	//if editing a leave box...
		{
			qryStr="_POP_CustomerList.aspx?irs=Y&dr="+objRef+"&dv="+day_value+"&uc="+user_comments+"&rid="+this.routeID+"&defid="+this.defplanID;
		}
		else if(day_value=="V")	//if editing a visits box....
		{
			qryStr="_POP_CustomerList.aspx?irs=Y&dr="+objRef+"&dv="+day_value+"&sc="+custinfo_area.innerHTML+"&rid="+this.routeID+"&defid="+this.defplanID;
		}
		else	//if anything else....
		{
			qryStr="_POP_CustomerList.aspx?irs=Y&dr="+objRef+"&dv="+day_value+"&rid="+this.routeID+"&defid="+this.defplanID;
		}
		this.winHnd=this.popUp(qryStr,'CustWindow',550,600,'yes');
	}
	this.setCustVisitDays();
}


function PlanHandler_setDay(objRef,day_type,user_comments)
{
	var day_box;

	day_box=getElementById_New(objRef);
	if(day_box!=null)
	{
		if(day_type=="O")	//if marking as off
		{
			day_box.bgColor=this.offBGColor;
			day_box.value=day_type;
			changeInnerHTML(("DIV_"+objRef),user_comments);
			changeTitle(("DIV_"+objRef),user_comments);			
		}
		else if(day_type=="")	//if resetting day.....
		{
			day_box.bgColor=this.defBGColor;
			day_box.value="";
			changeInnerHTML(("DIV_"+objRef),"");
			changeTitle(("DIV_"+objRef),"");						
		}
		else if(day_type=="X")	//if mark as working.....
		{
			day_box.bgColor=this.defBGColor;
			day_box.value=day_type;
			changeInnerHTML(("DIV_"+objRef),"");
			changeTitle(("DIV_"+objRef),"");						
		}		
		else if(day_type=="W")	//if resetting weekend.....
		{
			day_box.bgColor=this.weekendBGColor;
			day_box.value=day_type;
			changeInnerHTML(("DIV_"+objRef),"");
			changeTitle(("DIV_"+objRef),"");						
		}		
		else if(day_type=="U")	//if marking as out of office...
		{
			day_box.bgColor=this.onBGColor;
			day_box.value=day_type;
			changeInnerHTML(("DIV_"+objRef),user_comments);
			changeTitle(("DIV_"+objRef),user_comments);			
		}		
	}
}

function PlanHandler_setRouteDay(objRef,day_type,user_comments,cust_info)
{
	var day_box;

	day_box=getElementById_New(objRef);
	
	if(day_box!=null)
	{
		if(day_type=="L")	//if setting leave(or day off)...
		{
			day_box.bgColor=this.offBGColor;
			day_box.value=day_type;
			changeInnerHTML(("DIV_"+objRef),user_comments);
			changeTitle(("DIV_"+objRef),user_comments.replace(/<BR>/g,"\n"));
			changeInnerHTML(("DIV_C_"+objRef),"");
			changeInnerHTML(("SPAN_V_C_"+objRef),"");						
		}
		else if(day_type=="V")	//if setting up visits.....
		{
			day_box.bgColor=this.onBGColor;
			day_box.value=day_type;
			changeInnerHTML(("DIV_"+objRef),user_comments);
			changeTitle(("DIV_"+objRef),user_comments.replace(/<BR>/g,"\n"));
			changeInnerHTML(("DIV_C_"+objRef),cust_info);
			changeInnerHTML(("SPAN_V_C_"+objRef),"("+cust_info.match(new RegExp("[$]","g")).length+")");
		}
		else if(day_type=="Z")	//if Out Of Office.....
		{
			day_box.bgColor=this.onBGColor;
			day_box.value=day_type;
			changeInnerHTML(("DIV_"+objRef),user_comments);
			changeTitle(("DIV_"+objRef),user_comments.replace(/<BR>/g,"\n"));
			changeInnerHTML(("DIV_C_"+objRef),"");
			changeInnerHTML(("SPAN_V_C_"+objRef),"");						
		}
		else if(day_type=="")	//if resetting day.....
		{
			day_box.bgColor=this.defBGColor;
			day_box.value="";
			changeInnerHTML(("DIV_"+objRef),"");
			changeTitle(("DIV_"+objRef),"");						
			changeInnerHTML(("DIV_C_"+objRef),"");
			changeInnerHTML(("SPAN_V_C_"+objRef),"");
		}
	}
}

function PlanHandler_setRoutePlanValues()
{
	var objRef="Day";
	var planValues="";
	var objDay;
	var objDayMsg;
	var objDayVisits;
	var i=1;
	for(i=1;i<32;i++)
	{
		objDay=getElementById_New(objRef+i);
		objDayMsg=getElementById_New("DIV_"+objRef+i);
		objDayVisits=getElementById_New("DIV_C_"+objRef+i);
		if(objDay!=null && objDayMsg!=null && objDayVisits!=null)
		{
			if(objDay.value=="W")	//weekend
			{
				getElementById_New(("HDay"+i)).value="W";
			}
			else if(objDay.value=="O")	//marked as off in default plan
			{
				getElementById_New(("HDay"+i)).value="O|"+objDayMsg.innerHTML;
			}
			else if(objDay.value=="L")	//marked as leave/off by FSR in route plan
			{
				getElementById_New(("HDay"+i)).value="L|"+objDayMsg.innerHTML;
			}
			else if(objDay.value=="V")	//marked for visits by FSR in route plan
			{
				getElementById_New(("HDay"+i)).value="V|"+objDayVisits.innerHTML;
			}
			else if(objDay.value=="Z")	//marked as out-of-office by FSR in route plan..
			{
				getElementById_New(("HDay"+i)).value="Z|"+objDayMsg.innerHTML;
			}
			else if(objDay.value=="U")	//marked as out-of-office in default plan....
			{
				getElementById_New(("HDay"+i)).value="U|"+objDayMsg.innerHTML;
			}
			else
				getElementById_New(("HDay"+i)).value="NULL";
		}
		else
		{
			getElementById_New(("HDay"+i)).value="NULL";
		}
		objDay=null;
		objDayMsg=null;
	}
}

function PlanHandler_setCustVisitDays()
{
	var objRef="Day";
	var planValues="";
	var objDay;
	var objDayVisits;
	var i=1;
	var j=0;
	
	this.arrCustVisits=new Array();
	var objCustVisits;
	var arrTempCustSiteID;
	
	for(i=1;i<32;i++)
	{
		objDay=getElementById_New(objRef+i);
		objDayVisits=getElementById_New("DIV_C_"+objRef+i);
		if(objDay!=null && objDayVisits!=null)
		{
			if(objDay.value=="V")	//marked for visits by FSR in route plan
			{
				arrTempCustSiteID=objDayVisits.innerHTML.split("|");
				if(arrTempCustSiteID!=null && arrTempCustSiteID.length>0)
				{
					for(j=0;j<arrTempCustSiteID.length;j++)
					{
						objCustVisits=new CustVisits();
						objCustVisits.CustSiteID=arrTempCustSiteID[j];
						objCustVisits.VisitDays=objDayVisits.day;
						this.setVisitDays(objCustVisits);
						objCustVisits=null;
					}
				}
			}
		}
		objDay=null;
		objDayVisits=null;
	}
}

function PlanHandler_setVisitDays(CustVisitInfo)
{
	var i=0;
	var IsSet=false;
	for(i=0;i<this.arrCustVisits.length;i++)
	{
		if(this.arrCustVisits[i].CustSiteID==CustVisitInfo.CustSiteID)
		{
			this.arrCustVisits[i].VisitDays=this.arrCustVisits[i].VisitDays+","+CustVisitInfo.VisitDays;
			IsSet=true;
			break;
		}
	}
	if(!IsSet)
	{
		var objCustVisits=new CustVisits();
		objCustVisits.CustSiteID=CustVisitInfo.CustSiteID;
		objCustVisits.VisitDays=CustVisitInfo.VisitDays;
		this.arrCustVisits[this.arrCustVisits.length]=objCustVisits;
		objCustVisits=null;				
	}
}
