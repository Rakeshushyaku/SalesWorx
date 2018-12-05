<%@ Page Title="Manage User Types" Language="vb" AutoEventWireup="false" MasterPageFile="~/html/DefaultLayout.Master"
    CodeBehind="ManageUserTypes.aspx.vb" Inherits="SalesWorx_BO.ManageUserTypes" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script language="javascript" type="text/javascript">
        function client_OnTreeNodeChecked(evt) {
            var obj;
            if (window.event) obj = window.event.srcElement;
            else obj = (evt ? evt : (window.event ? window.event : null)).target;

            var treeNodeFound = false;
            var checkedState;
            if (obj.tagName == "INPUT" && obj.type == "checkbox") {
                checkedState = obj.checked;
                do {
                    obj = obj.parentNode;
                }
                while (obj.tagName != "TABLE")




                var parentTreeLevel = obj.rows[0].cells.length;

                //get the current node's parent node.
                var tables = obj.parentNode.getElementsByTagName("TABLE");
                var numTables = tables.length;




                if (numTables >= 1) {
                    for (i = 0; i < numTables; i++) {
                        if (tables[i] == obj) {
                            treeNodeFound = true;
                            i++;
                            if (i == numTables) return;



                        }

                        if (treeNodeFound == true) {
                            var childTreeLevel = tables[i].rows[0].cells.length;
                            if (childTreeLevel > parentTreeLevel) {
                                var cell = tables[i].rows[0].cells[childTreeLevel - 1];
                                var inputs = cell.getElementsByTagName("INPUT");
                                inputs[0].checked = checkedState;
                            }
                            else return;
                        }
                    }
                }
            }
        }

        
    </script>

    <script type="text/javascript">
        var TargetBaseControl = null;

        window.onload = function() {
            try {
                TargetBaseControl =
           document.getElementById('<%= Me.UpdatePanel1.ClientID %>');

            }
            catch (err) {
                TargetBaseControl = null;
            }
        }
        function IsCheckedCheckBox() {
            if (TargetBaseControl == null) return false;
            var TargetChildControl = "chkDelete";
            var TimeCon = document.getElementById('<%= Me.UpdatePanel1.ClientID %>');

            var Inputs = TargetBaseControl.getElementsByTagName("input");

            for (var n = 0; n < Inputs.length; ++n)
                if (Inputs[n].type == 'checkbox' && Inputs[n].id.indexOf(TargetChildControl, 0) >= 0 && Inputs[n].checked) {

                if (confirm('Are you sure you want to delete the selected user role(s)? Doing so would delete all the associated users!') == true) {
                    return true;

                }
                else {
                    return false;
                }
            }

            alert('Select atleast one row!');
            return false;

        }

        function DisableNodes(evt) {

            var src = window.event != window.undefined ? window.event.srcElement : evt.target;
            if (src.checked) {
                src.checked = false;
            }
            else {
                src.checked = true;
            }
        }

        function OnTreeClick(evt) {
            var src = window.event != window.undefined ? window.event.srcElement : evt.target;
            var isChkBoxClick = (src.tagName.toLowerCase() == "input" && src.type == "checkbox");
            if (isChkBoxClick) {
                var parentTable = GetParentByTagName("table", src);
                var nxtSibling = parentTable.nextSibling;
                if (nxtSibling && nxtSibling.nodeType == 1)//check if nxt sibling is not null & is an element node
                {
                    if (nxtSibling.tagName.toLowerCase() == "div") //if node has children
                    {
                        //check or uncheck children at all levels
                        CheckUncheckChildren(parentTable.nextSibling, src.checked);
                    }
                }
                //check or uncheck parents at all levels

                // I have uncommented following call for the function...as we dont need to uncheck parents if child is unchecked
                CheckUncheckParents(src, src.checked);
            }
        }

        function CheckUncheckChildren(childContainer, check) {
            var childChkBoxes = childContainer.getElementsByTagName("input");
            var childChkBoxCount = childChkBoxes.length;
            for (var i = 0; i < childChkBoxCount; i++) {
                childChkBoxes[i].checked = check;
            }
        }

        function CheckUncheckParents(srcChild, check) {
            var parentDiv = GetParentByTagName("div", srcChild);
            var parentNodeTable = parentDiv.previousSibling;

            if (parentNodeTable) {
                var checkUncheckSwitch = true;

                if (check) //checkbox checked
                {

                    var isAllSiblingsChecked = AreAllSiblingsChecked(srcChild);
                    if (isAllSiblingsChecked)
                        checkUncheckSwitch = true;
                    //                else    
                    //                    return; //do not need to check parent if any child is not checked
                }
                else //checkbox unchecked
                {

                    //checkUncheckSwitch = false;

                }

                var inpElemsInParentTable = parentNodeTable.getElementsByTagName("input");
                if (inpElemsInParentTable.length > 0) {
                    var parentNodeChkBox = inpElemsInParentTable[0];
                    parentNodeChkBox.checked = checkUncheckSwitch;
                    //do the same recursively
                    CheckUncheckParents(parentNodeChkBox, checkUncheckSwitch);
                }
            }
        }

        function AreAllSiblingsChecked(chkBox) {
            var parentDiv = GetParentByTagName("div", chkBox);
            var childCount = parentDiv.childNodes.length;
            for (var i = 0; i < childCount; i++) {
                if (parentDiv.childNodes[i].nodeType == 1) //check if the child node is an element node
                {
                    if (parentDiv.childNodes[i].tagName.toLowerCase() == "table") {
                        var prevChkBox = parentDiv.childNodes[i].getElementsByTagName("input")[0];
                        //if any of sibling nodes are not checked, return false
                        if (!prevChkBox.checked) {
                            return false;
                        }
                    }
                }
            }
            return true;
        }

        //utility function to get the container of an element by tagname
        function GetParentByTagName(parentTagName, childElementObj) {
            var parent = childElementObj.parentNode;
            while (parent.tagName.toLowerCase() != parentTagName.toLowerCase()) {
                parent = parent.parentNode;
            }
            return parent;
        }

        function CheckAll(cbSelectAll) {
            if (TargetBaseControl == null) return false;
            var TargetChildControl = "chkDelete";
            var TimeCon = document.getElementById('<%= Me.UpdatePanel1.ClientID %>');

            var Inputs = TargetBaseControl.getElementsByTagName("input");

            for
         (
             var n = 0; n < Inputs.length; ++n)
                if (Inputs[n].type == 'checkbox' && Inputs[n].id.indexOf(TargetChildControl, 0) >= 0) {
                Inputs[n].checked = cbSelectAll.checked;
            }

        }

        function pageLoad(sender, args) {
            if (!args.get_isPartialLoad()) {
                //  add our handler to the document's
                //  keydown event
                $addHandler(document, "keydown", onKeyDown);
            }
        }

        function onKeyDown(e) {
            if (e && e.keyCode == Sys.UI.Key.esc) {
                // if the key pressed is the escape key, dismiss the dialog
                $find('ctl00_contentArea_MpInfoError').hide();
                $find('ctl00_contentArea_MpUserRollAccess').hide();
            }
        }
        function uncheckAll() {
            inputs = document.getElementsByTagName("input");
            for (i = 0; i < inputs.length; i++) {
                if (inputs[i].type == "checkbox") {
                    inputs[i].checked = false;
                }
            }


        }
        function DelConfirm() {
            var ddl1Id = '<%= drpUserTypes.ClientID %>';
            var val = document.getElementById(ddl1Id).value
            if (val > 0) {
                ans = confirm('Are you sure you want to delete this user type?');
            }
            else {
                alert('Select user type.');
                ans = false;
            }
            return ans;
        }
    </script>

    <style type="text/css">
        .overlay
        {
            position: fixed;
            z-index: 1000;
            top: 49%;
            left: 49%;
            width: 100%;
            height: 100%;
        }
        * html .overlay
        {
            position: absolute;
            height: expression(document.body.scrollHeight > document.body.offsetHeight ? document.body.scrollHeight : document.body.offsetHeight + 'px');
            width: expression(document.body.scrollWidth > document.body.offsetWidth ? document.body.scrollWidth : document.body.offsetWidth + 'px');
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table width="100%" height="100%" border="0" cellspacing="0" cellpadding="0" bgcolor="#f6f6f6">
        <tr>
            <td id="contentofpage" width="100%" height="76%" class="topshadow">
                <div class="pgtitileposition">
                    <span class="pgtitile3">Manage User Type</span></div>
                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>
                        <table width="auto" border="0" cellpadding="3" cellspacing="10" id="tableformNrml">
                            <tr>
                                <td colspan="2">
                                    <asp:Label ID="lblmsg" runat="server" Font-Bold="True" ForeColor="Maroon"></asp:Label>
                                    <table cellpadding="4" cellspacing="4">
                                  
                                        <tr>
                                            <td class="txtSMBold">
                                                User Type:
                                            </td>
                                            <td class="txtSMBold">
                                                <asp:DropDownList ID="drpUserTypes"  CssClass="inputSM"   Width ="250px" runat="server" AutoPostBack="True" onclick="uncheckAll();">
                                                </asp:DropDownList>
                                                <asp:TextBox ID="txtUserType" Width ="250px"  CssClass="inputSM"   runat="server" Visible="false"></asp:TextBox>
                                            </td>
                                           
                                        </tr>  <tr>
                                <td class="txtSMBold">
                                    Designation:
                                </td>
                                <td>
                                    <asp:DropDownList ID="drpDesignation" Width ="250px" runat="server" AutoPostBack="True"  CssClass="inputSM" >
                                    </asp:DropDownList>
                                </td>
                            </tr>
                                        
                                                  <tr>
                                            <td> </td>
                                            <td class="txtSMBold" >
                                                <asp:Panel ID="pnlDefault" runat="server">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                    <asp:Button ID="btnAddNew" runat="server" Text="Add" CssClass="btnInputBlue" />
                                                    <asp:Button ID="btnModify" runat="server" Text="Modify" CssClass="btnInputOrange" />
                                                    <asp:Button ID="btnDelete" runat="server" Text="Delete" CssClass="btnInputRed" OnClientClick="javascript:return DelConfirm();" />
                                                </asp:Panel>
                                                <asp:Panel runat="server" ID="pnlAdd" Visible="false">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                    <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="btnInputGreen" />
                                                    <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="btnInputRed" />
                                                </asp:Panel>
                                                <asp:Panel runat="server" ID="pnlModify" Visible="false">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                    <asp:Button ID="btnUpdate" runat="server" Text="Update" CssClass="btnInputGreen" />
                                                    <asp:Button ID="btnCancelM" runat="server" Text="Cancel" CssClass="btnInputRed" />
                                                </asp:Panel>
                                            </td>
                                          
                                        </tr>
                                       <tr>
                                        <td class="txtSMBold">
                                               PDA Rights
                                            </td>
                                       
                                            <td class="txtSM" >
                                              <telerik:RadComboBox ID="ddlVanRights" Width ="250px" runat="server" CheckBoxes="true" EnableCheckAllItemsCheckBox="true" 
                  EmptyMessage ="Please select"  Localization-CheckAllString ="Select All"     >
               <%-- <Items>
                <telerik:RadComboBoxItem Value ="C" Text ="Collection" />
                <telerik:RadComboBoxItem Value ="D" Text ="Distribution Check" />
                <telerik:RadComboBoxItem Value ="I" Text ="Invoice" />
                <telerik:RadComboBoxItem Value ="O" Text ="Order" />
                <telerik:RadComboBoxItem Value ="R" Text ="Return" />
                
                </Items>--%>
                       </telerik:RadComboBox>
                                       </tr>
                                        <tr>
                                        <td class="txtSMBold" valign ="top">
                                         Back Office Rights
                                        </td>
                                           <td>
                                          
                                                <asp:TreeView ID="TreeViewRights" runat="server" ShowCheckBoxes="All" Font-Size="12px"
                                                    onclick="OnTreeClick();" BorderStyle="None" BorderWidth="0px" HoverNodeStyle-BorderStyle="None"
                                                    HoverNodeStyle-BorderWidth="0">
                                                    <HoverNodeStyle BorderStyle="None" BorderWidth="0px" />
                                                  
                                                    <DataBindings>
                                                       <asp:TreeNodeBinding DataMember="mainmenu" TextField="value" ValueField="id"  />
                                                        <asp:TreeNodeBinding DataMember="submenu" TextField="value" ValueField="id" />
                                                    </DataBindings>
                                                    <RootNodeStyle Font-Bold="true" NodeSpacing="4"    />
                                                </asp:TreeView>
                                                <asp:XmlDataSource ID="XmlDataSource1" runat="server" DataFile="~/xml/UserRights.xml"
                                                    TransformFile="~/xml/XSLTFile1.xsl" XPath="/*/*"></asp:XmlDataSource>
                                            </td>
                                            
                                        </tr>
                              
                                    </table>
                                </td>
                              
                            </tr>
                           
                            <tr>
                               
                                <td>
                                    <asp:Button ID="btnHidden" runat="Server" Style="display: none" />
                                    <ajaxToolkit:ModalPopupExtender ID="MpInfoError" runat="server" BackgroundCssClass="modalBackground"
                                        TargetControlID="btnHidden" PopupControlID="pnlConfirm" CancelControlID="btnClose"
                                        Drag="true" />
                                    <asp:Panel ID="pnlConfirm" Width="400" runat="server" CssClass="modalPopup" Style="display: none">
                                        <table id="tableinPopupErr" width="400" cellpadding="10" style="background-color: White;">
                                            <tr align="center">
                                                <td align="center" style="cursor: move; background-color: #3399ff; text-align: center;
                                                    border: solid 1px #3399ff; color: White; padding: 3px">
                                                    <asp:Label ID="lblinfo" runat="server" Font-Size ="13px"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="center" style="text-align: center">
                                                    <asp:ValidationSummary ID="valSummary" runat="server" DisplayMode="List" />
                                                    <asp:Label ID="lblMessage" runat="server" Font-Size ="13px"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="center" style="text-align: center;">
                                                    <asp:Button ID="btnClose" runat="server" Text="Ok" CssClass="btnInputBlue" />
                                                </td>
                                            </tr>
                                        </table>
                                    </asp:Panel>
                                </td>
                            </tr>
                        </table>
                    </ContentTemplate>
                </asp:UpdatePanel>
                <asp:UpdateProgress ID="UpdateProgress1" AssociatedUpdatePanelID="UpdatePanel1" runat="server">
                    <ProgressTemplate>
                        <asp:Panel ID="Panel11" CssClass="overlay" runat="server">
                            <img src="../images/Progress.gif" alt="Processing..." style="z-index: 10010; vertical-align: middle;" />
                            <span style="font-size: 12px; font-weight: 700; color: #3399ff;">Processing... </span>
                        </asp:Panel>
                    </ProgressTemplate>
                </asp:UpdateProgress>
            </td>
        </tr>
    </table>
</asp:Content>
