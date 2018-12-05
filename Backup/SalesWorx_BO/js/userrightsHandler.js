var arrUserRights;
var arrCounter;

function setUserRightsData()
{
	var objForm=document.getElementById("frmAdminUserTypes");
	arrUserRights=new Array(0);
	arrCounter=0;
	if(objForm!=null)
	{
		var input_items=objForm.all;
		var item_count=input_items.length;
		var field_items;
		var i;
		for(i=0;i<item_count;i++)
		{
			if(input_items[i].type=="checkbox")
			{
				if(input_items[i].checked)
				{
					if(input_items[i].name=="PAGE_ITEM")
					{
						setFieldItems(input_items, item_count, input_items[i].Menu_Id, input_items[i].Page_Id);
					}						
				}
			}
		}
	}
	
	var tempStr="";
	var x;
	
	for(x=0;x<arrUserRights.length;x++)
	{
		tempStr=tempStr+arrUserRights[x]+"^";			
	}
	
	objForm.UserRightsData.value=tempStr;
	
	arrUserRights=null;
	arrCounter=0;
}

function setFieldItems(input_items, item_count, menu_ref, page_ref)
{
	if(!pageExists(menu_ref, page_ref))
	{
		var field_items="";
		var itemRef;
		var j;
		for(j=0;j<item_count;j++)
		{
			itemRef=input_items[j];
			if(itemRef.name!=null && itemRef.name=="FIELD_ITEM" && itemRef.Page_Id==page_ref)
			{
				if(itemRef.checked)
					field_items=field_items + itemRef.Field_Id + ":1|";
				else
					field_items=field_items + itemRef.Field_Id + ":0|";
			}
		}
		arrUserRights[arrUserRights.length]=menu_ref + "#" + page_ref + "#" + field_items + "#";
		arrCounter++;
	}
}

function pageExists(menu_ref, page_ref)
{
	var k;
	for(k=0;k<arrUserRights.length;k++)
	{
		if(arrUserRights[k].toString().indexOf(menu_ref+"#"+page_ref+"#")==0)
		{
			return true;
		}
	}
	return false;			
}

function menuItemClicked(objRef)
{
	var objFormInputs=document.getElementById("frmAdminUserTypes").all;
	var i;
	for(i=0;i<objFormInputs.length;i++)
	{
		if(objFormInputs[i].Menu_Id!=null && objFormInputs[i].Menu_Id==objRef.Menu_Id)
		{
			objFormInputs[i].checked=objRef.checked;
		}
	}
}

function pageItemClicked(objRef)
{
	var objFormInputs=document.getElementById("frmAdminUserTypes").all;
	var i;
	if(objRef.checked)
	{
		for(i=0;i<objFormInputs.length;i++)
		{
			if((objFormInputs[i].Page_Id!=null && objFormInputs[i].Page_Id==objRef.Page_Id) || (objFormInputs[i].Menu_Id!=null && objFormInputs[i].Menu_Id==objRef.Menu_Id && objFormInputs[i].name=="MENU_ITEM"))
			{
				objFormInputs[i].checked=true;
			}
		}
	}
	else
	{
		for(i=0;i<objFormInputs.length;i++)
		{
			if(objFormInputs[i].Page_Id!=null && objFormInputs[i].Page_Id==objRef.Page_Id)
			{
				objFormInputs[i].checked=false;
			}
		}
		
		verifyAndSelectMenu(objFormInputs,objRef.Menu_Id);
	}
}

function verifyAndSelectMenu(objInputs, MenuId)
{
	var x;
	var inputRef;
	var isPageSelected=false;
	
	for(x=0;x<objInputs.length;x++)		
	{
		inputRef=objInputs[x];
		if(inputRef.Menu_Id!=null && inputRef.Menu_Id==MenuId && inputRef.name=="PAGE_ITEM" && inputRef.checked)
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
			if(inputRef.name=="MENU_ITEM" && inputRef.Menu_Id==MenuId)
			{
				inputRef.checked=false;
				return;
			}
		}
	}
}

function fieldItemClicked(objRef)
{
	var objFormInputs=document.getElementById("frmAdminUserTypes").all;
	var i;
	
	if(objRef.checked)    
	{
		for(i=0;i<objFormInputs.length;i++)
		{
			if((objFormInputs[i].Page_Id!=null && objFormInputs[i].Page_Id==objRef.Page_Id && objFormInputs[i].name=="PAGE_ITEM") || (objFormInputs[i].Menu_Id!=null && objFormInputs[i].Menu_Id==objRef.Menu_Id && objFormInputs[i].name=="MENU_ITEM"))
			{
				objFormInputs[i].checked=true;
			}
		}
	}
}
