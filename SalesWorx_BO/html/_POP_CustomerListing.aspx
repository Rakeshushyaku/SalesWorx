<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="_POP_CustomerListing.aspx.vb" Inherits="SalesWorx_BO._POP_CustomerListing" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">

    <title>Route Planner - Customer List</title>
   
<script type="text/javascript" src="../js/jquery-1.3.2.min.js"></script>
<link href="../facebox/facebox.css" media="screen" rel="stylesheet" type="text/css"/>
<script src="../facebox/facebox.js" type="text/javascript"></script> 
<link href="../styles/UpdateProgress.css" rel="stylesheet" type="text/css">
<link href="../styles/swxstyle.css" rel="stylesheet" type="text/css">
<link href="../styles/salesworx.css" rel="stylesheet" type="text/css">
<link rel="stylesheet" type="text/css" href="../styles/superfish.css" media="screen">
    

        <style type="text/css">
        .FixedHeader {
            position: absolute;
            font-weight: bold;
        }     
 
    </style>
    <script type="text/javascript">


        var TargetBaseControl = null;

        window.onload = function () {
            try {

            }
            catch (err) {
                TargetBaseControl = null;
            }
        }


        function GetRadWindow() {
            var oWindow = null;
            if (window.radWindow) oWindow = window.radWindow;
            else if (window.frameElement.radWindow) oWindow = window.frameElement.radWindow;
            return oWindow;
        }




</script>
       
</head>
<body>
    <form id="frmPopupCustList" runat="server" class="outerform" >
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
     <input id="DayText" name="DayText" type="hidden"  runat="server"/>
      <input id="ComVal" name="ComVal" type="hidden"  runat="server"/>
      <input id="DayRef" name="DayRef" type="hidden"  runat="server"/>
        <input id="ComString" name="ComString" type="hidden"  runat="server"/>

   <script language="javascript" type="text/javascript">

       var prm = Sys.WebForms.PageRequestManager.getInstance();

       prm.add_initializeRequest(InitializeRequest);
       prm.add_endRequest(EndRequest);
       var postBackElement;
       function InitializeRequest(sender, args) {

           if (prm.get_isInAsyncPostBack())
               args.set_cancel(true);
           postBackElement = args.get_postBackElement();
           if (postBackElement.id == 'Btn_Filter') {
               $get('UpdateProgress1').style.display = 'block';
           }



           if (postBackElement.id == 'Panel2Trigger')
               $get('UpdateProgress2').style.display = 'block';
           if (postBackElement.id == 'MoveBtn')
               $get('UpdateProgress2').style.display = 'block';
           if (postBackElement.id == 'CopyBtn')
               $get('UpdateProgress2').style.display = 'block';

           postBackElement.disabled = true;


       }

       function EndRequest(sender, args) {
           if (postBackElement.id == 'Btn_Filter') {
               $get('UpdateProgress1').style.display = 'none';
           }

           if (postBackElement.id == 'Panel2Trigger')
               $get('UpdateProgress2').style.display = 'none';
           if (postBackElement.id == 'MoveBtn')
               $get('UpdateProgress2').style.display = 'none';
           if (postBackElement.id == 'CopyBtn')
               $get('UpdateProgress2').style.display = 'none';



           postBackElement.disabled = false;
       }



</script>
 
     <table width="100%" height="100%" border="0" cellspacing="0" cellpadding="0" bgcolor="#f6f6f6">

	<tr>
	
	<td id="contentofpage" width="100%" height="100%" class="topshadow">
	
	<span class="pgtitile3">Customer List</span>
	

          <asp:GridView ID="CustomerGrid"  runat="server" AutoGenerateColumns="False" 
                                           EnableViewState="true"   CellPadding="0" CellSpacing="0" CssClass="tablecellalign"  EmptyDataText="No visits planned">
                                    <Columns>
                                   
                                        <asp:TemplateField>
                                           <ItemTemplate>
                                  <asp:HiddenField ID="CustomerNo" runat="server" Value='<%# Bind("Customer_No") %>'/>
                                                <asp:Label  ID="Customer_No" runat="server" Text='<%# Bind("Customer_No") %>'></asp:Label>
                                            </ItemTemplate>
                                            <HeaderTemplate>
                                             Cust.No
                                            </HeaderTemplate>
                                        </asp:TemplateField>
                                       
                                       <asp:TemplateField>
                                           <ItemTemplate>
                                  <asp:HiddenField ID="CustomerName" runat="server" Value='<%# Bind("Customer_Name") %>'/>
                                   <asp:HiddenField ID="Sequence" runat="server" Value='<%# Bind("Sequence") %>'/>
                                                <asp:Label  ID="Customer_Name" runat="server" Text='<%# Bind("Customer_Name") %>'></asp:Label>
                                            </ItemTemplate>
                                             <HeaderTemplate>
                                             Cust. Name
                                            </HeaderTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="Customer_Class" Visible="False" />
                                        <asp:BoundField DataField="Address" Visible="False" />
                                        <asp:BoundField DataField="City" Visible="False" />

                                    </Columns>
                                     <PagerStyle CssClass="pagerstyle" />
                                                    <HeaderStyle   />
                                                    <RowStyle CssClass="tdstyle"   />
                                                    <AlternatingRowStyle CssClass="alttdstyle"  />
                                </asp:GridView>

	<br/>
	<br/>
	</td> <!-- "contentofpage" ends in this td -->
	</tr>
	

</table>
       

  
        </form>
</body>
</html>
