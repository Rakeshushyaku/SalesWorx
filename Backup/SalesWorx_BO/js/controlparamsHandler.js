var arrControlParams;
var arrCounter;

function sectionClicked(objRef)
{
	var objFormInputs=document.forms[0].all;
	var i;
	for(i=0;i<objFormInputs.length;i++)
	{
		if(objFormInputs[i].Section_Id!=null && objFormInputs[i].Section_Id==objRef.Section_Id)
		{
			if(objFormInputs[i].SubPoint_Id==null)
			{
				objFormInputs[i].checked=objRef.checked;
			}
			else
			{
				if(isSubLinked(objFormInputs[i].Point_Id))
				{
					objFormInputs[i].disabled=!objRef.checked;
				}
			}
		}
	}
}

function isSubLinked(PointId)
{
	var objFormInputs=document.getElementsByName("POINT");
	var i;
	
	for(i=0;i<objFormInputs.length;i++)
	{
		if(objFormInputs[i].Point_Id==PointId)
		{
			if(objFormInputs[i].sublink.toUpperCase()=="TRUE")
				return true;
			else
				return false;
		}
	}
	
	return false;
}

function pointClicked(objRef)
{
	var objFormInputs=document.forms[0].all;
	var i;
	if(objRef.checked)
	{
		for(i=0;i<objFormInputs.length;i++)
		{
			if(objFormInputs[i].Section_Id!=null && objFormInputs[i].Section_Id==objRef.Section_Id && objFormInputs[i].name=="SECTION")
			{
				objFormInputs[i].checked=true;
			}
		}
		
		if(objRef.sublink.toUpperCase()=="TRUE")
		{
			for(i=0;i<objFormInputs.length;i++)
			{
				if(objFormInputs[i].Point_Id!=null && objFormInputs[i].Point_Id==objRef.Point_Id && objFormInputs[i].name.indexOf("SUB_POINT")==0)
				{
					objFormInputs[i].disabled=false;
				}
			}
		}
	}
	else
	{
		if(objRef.sublink.toUpperCase()=="TRUE")
		{
			for(i=0;i<objFormInputs.length;i++)
			{
				if(objFormInputs[i].Point_Id!=null && objFormInputs[i].Point_Id==objRef.Point_Id && objFormInputs[i].name.indexOf("SUB_POINT")==0)
				{
					objFormInputs[i].disabled=true;
				}
			}
		}
		
		verifyAndSelectSection(objFormInputs,objRef.Section_Id);
	}
}

function verifyAndSelectSection(objInputs, MenuId)
{
	var x;
	var inputRef;
	var isPageSelected=false;
	
	for(x=0;x<objInputs.length;x++)		
	{
		inputRef=objInputs[x];
		if(inputRef.Section_Id!=null && inputRef.Section_Id==MenuId && inputRef.checked && inputRef.name=="POINT")
		{
			isPageSelected=true;
			break;
		}
	}
	
	if(!isPageSelected)
	{
		for(x=0;x<objInputs.length;x++)		
		{
			inputRef=objInputs[x];
			if(inputRef.name=="SECTION" && inputRef.Section_Id==MenuId)
			{
				inputRef.checked=false;
				return;
			}
		}
	}
}

function subPointClicked(objRef)
{
	var objFormInputs=null;
	var i;
	
	if(objRef.checked)    
	{
		objFormInputs=document.getElementsByName("POINT");
		for(i=0;i<objFormInputs.length;i++)
		{
			if(objFormInputs[i].Point_Id!=null && objFormInputs[i].Point_Id==objRef.Point_Id && objFormInputs[i].name=="POINT")
			{
				if(objFormInputs[i].sublink.toUpperCase()=="FALSE")
					return;
				else
					objFormInputs[i].checked=true;
			}
		}
		
		objFormInputs=document.getElementById("SECTION");
		for(i=0;i<objFormInputs.length;i++)
		{
			if(objFormInputs[i].Section_Id!=null && objFormInputs[i].Section_Id==objRef.Section_Id && objFormInputs[i].name=="SECTION")
			{
				objFormInputs[i].checked=true;
			}
		}
	}
}

function setSubPointData()
{
	var objFormInputs=document.getElementById("frmAdminAppControl").all;
	var i;
	var sSubPointData='';
	
	for(i=0;i<objFormInputs.length;i++)
	{
		if(objFormInputs[i].name!=null && objFormInputs[i].name.indexOf("SUB_POINT_")==0)
		{
			if(objFormInputs[i].state=='1' && objFormInputs[i].checked)
			{
				if(sSubPointData=='')
					sSubPointData=objFormInputs[i].SubPoint_Id;
				else
					sSubPointData=sSubPointData+","+objFormInputs[i].SubPoint_Id;
			}
		}
	}

	document.getElementById("SUB_POINT").value=sSubPointData;
}
