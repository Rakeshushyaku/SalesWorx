<%@ Page Title="Van Device Configuartion" Language="vb" AutoEventWireup="false" MasterPageFile="~/html/Site.Master"
    CodeBehind="VanUserSyncSetting.aspx.vb" Inherits="SalesWorx_BO.VanUserSyncSetting" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>


<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <style>
        .tablecellalign th label, .tablecellalign td label{
            display:inline-block;
            margin-right:10px;
            margin-bottom: 0;
            vertical-align: text-bottom;
        }
        .tablecellalign th span{
            display:block;
            margin-bottom:10px;
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
   
    <script language="javascript" type="text/javascript">


        var prm = Sys.WebForms.PageRequestManager.getInstance();

        prm.add_initializeRequest(InitializeRequest);
        prm.add_endRequest(EndRequest);
        var postBackElement;
        function InitializeRequest(sender, args) {

            if (prm.get_isInAsyncPostBack())
                args.set_cancel(true);
            postBackElement = args.get_postBackElement();
            $get('<%=UpdateProgress1.ClientID %>').style.display = 'block';
            postBackElement.disabled = true;
        }

        function EndRequest(sender, args) {
            $get('<%=UpdateProgress1.ClientID %>').style.display = 'none';
            postBackElement.disabled = false;
        }
        function alertCallBackFn(arg) {

        }
    </script>

    <script type = "text/javascript">

        //Task1 Rakesh


</script>

    <script type="text/javascript">
        $(document).ready(function () {
            $(".rdb_main_enabl_sync input").change(function () {
                $(".rdb_enable_sync input[value='" + $(this).val() + "']").prop('checked', true);
            });
        });
    </script>

    <script type = "text/javascript">


        function check(objRef) {
            debugger;
            var GridView = objRef.parentNode.parentNode.parentNode;

            //var inputList = GridView.getElementsByTagName("input");
            var inputList1 = GridView.getElementsByName("rdb_DefaultsateNo");
            var inputList1 = GridView.getElementsByName("rdb_DefaultsateYes");


            for (var i = 0; i < inputList.length; i++) {

                //Get the Cell To find out ColumnIndex

                var row = inputList[i].parentNode.parentNode;
                parent = document.getElementById("<%= gvConfig.ClientID %>");
        var el = parent.document.getElementById("rdb_DefaultsateYes");
        el.checked = false;


    }

}


function checkAllTrue(objRef) {
    debugger;
    var GridView = objRef.parentNode.parentNode.parentNode;

    var inputList = GridView.getElementsByTagName("input");
    //var inputList1 = GridView.getElementsByName("rdb_main_enabl_sync");


    for (var i = 0; i < inputList.length; i++) {

        //Get the Cell To find out ColumnIndex

        var row = inputList[i].parentNode.parentNode;

        if (inputList[i].type == "radio" && objRef != inputList[i]) {
            debugger;
            if (objRef.checked) {
                inputList[i].checked = false;
            }
            else {
                inputList[i].checked = false;
            }

        }

    }

}
function checkAllFalse(objRef) {
    debugger;
    var GridView = objRef.parentNode.parentNode.parentNode;

    var inputList = GridView.getElementsByTagName("input");
    //var inputList1 = GridView.getElementsByName("rdb_main_enabl_sync");


    for (var i = 0; i < inputList.length; i++) {

        //Get the Cell To find out ColumnIndex

        var row = inputList[i].parentNode.parentNode;

        if (inputList[i].type == "radio" && objRef != inputList[i]) {
            debugger;
            if (objRef.checked) {
                inputList[i].checked = false;
            }
            else {
                inputList[i].checked = false;
            }

        }

    }

}

</script> 


 </asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
<h4>Userwise Background Sync Settings </h4>
                        <div id="pagenote" runat="server" visible="true">
                            <p>Please enable background sync from Main->System Setting ->Application Control</p></div>

    <telerik:RadAjaxLoadingPanel ID="AjaxLoadingPanel1" runat="server">
        <div id="ProgressPanel" class="overlay">

            <img src="../assets/img/ajax-loader.gif" alt="Processing..." style="z-index: 10010; vertical-align: middle;" />
        </div>
    </telerik:RadAjaxLoadingPanel>

    <asp:Panel ID="Panel1" runat="server"></asp:Panel>

    <telerik:RadWindowManager ID="RadWindowManager1" Skin="Simple" runat="server" EnableShadow="true">
        </telerik:RadWindowManager>

                            <asp:UpdatePanel runat="server" ID="TopPanel" UpdateMode ="Conditional" >
        <ContentTemplate>

            <div class="row">
	            <div class="col-sm-4">
		            <div class="form-group">
                        <label>Organization</label>
                        <telerik:RadComboBox ID="ddOraganisation"  Width="100%" Skin="Simple"  AutoPostBack="true" runat="server" ></telerik:RadComboBox>
		            </div>
	            </div>
                <div class="col-sm-4">
		            <div class="form-group">
                        <label>Sales Rep Name</label>
                         <telerik:RadComboBox Skin="Simple" CheckBoxes="true" EmptyMessage="Select Sales Rep Name" EnableCheckAllItemsCheckBox="true" ID="ddlSalesRep" Width ="100%" runat="server" DataTextField="SalesRep_Name" DataValueField="SalesRep_ID" Filter="Contains">
                                                            </telerik:RadComboBox >
		            </div>
	            </div>
                <div class="col-sm-4">
		            <div class="form-group">
                        <label><br /></label>
                        <asp:Button  ID="BtnSearch" runat="server" Text="Search" CssClass="btn btn-primary" />
                        <asp:Button  ID="btnUpdate" runat="server" Text="Update" CssClass="btn btn-success" />
                        <asp:Button  ID="btnReset" runat="server" Text="Reset" CssClass="btn btn-default" />
		            </div>
	            </div>
            </div>

                       <div class="table-responsive">
                           
                                             <asp:GridView Width="100%" ID="gvConfig" runat="server" OnRowDataBound="gvConfig_RowDataBound" EmptyDataText="No records to display"
                                                    EmptyDataRowStyle-Font-Bold="true"  AutoGenerateColumns="False"
                                                    AllowPaging="false" AllowSorting="false"  PageSize="25" CellPadding="0" CellSpacing="0" CssClass="tablecellalign">
                                                   
                                                    <EmptyDataRowStyle Font-Bold="True" />
                                                   
                                                    <PagerSettings Mode="NumericFirstLast" Position="Bottom" />
                                                    <Columns>
                                                       <asp:BoundField DataField="Row_ID" Visible ="false"  HeaderText="Row ID" SortExpression="Row_ID">
                                                            <ItemStyle Wrap="False" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="SalesRep_name" Visible ="true"  HeaderText="Sales Rep Name" HeaderStyle-Font-Bold="true" SortExpression="SalesRep_name">
                                                            <HeaderStyle Font-Bold="True" />
                                                            <ItemStyle Wrap="False" />
                                                        </asp:BoundField>
                                                       
                                                       
                                                         <asp:TemplateField HeaderText="Enable Background sync" HeaderStyle-Font-Bold="true" AccessibleHeaderText="asa"  >
                                                             <HeaderTemplate>
                                                                 <asp:Label ID="TypeId1" runat="server" Text="Enable Background sync" ></asp:Label>
                                                                  <%--<asp:RadioButtonList ID="rdb_main_enabl_sync" runat="server" RepeatDirection="Horizontal" EnableViewState="false" onclick = "checkAllTrue(this)" AutoPostBack="true" >
                                                                
                                                                      <asp:ListItem Text="Yes" Value="Y" ></asp:ListItem>
                                                                      <asp:ListItem Text="No" Value="N"></asp:ListItem>

                                                                  </asp:RadioButtonList>--%>
                                                                 <asp:RadioButton ID="rdb1" runat="server" Text="Yes"  EnableViewState ="true"  OnCheckedChanged="rdb1_CheckedChanged" AutoPostBack="true" GroupName="g1"  />
                                                                 <asp:RadioButton ID="rdb2" runat="server" Text="No"  AutoPostBack="true" EnableViewState="true" OnCheckedChanged="rdb2_CheckedChanged" GroupName="g1"  />

                                                             </HeaderTemplate>
                                                                      <ItemTemplate>
                                                                     <asp:Label runat ="server" Visible ="false" ID="lblSalesRep_name" Text ='<%#Eval("SalesRep_name") %>' ></asp:Label>
                                                                     <asp:Label runat ="server" Visible ="false" ID="lblRowID" Text ='<%#Eval("Row_ID") %>' ></asp:Label>
     
                                                                     <asp:RadioButtonList ID="rdb_enable_sync"   runat="server" TextAlign="Right" SelectedValue='<%#Eval("Config_Type")%>'  CssClass="rdb_enable_sync" RepeatDirection="Horizontal">
                                                                     <asp:ListItem Text="Yes" Value="Y" Selected="True">
                                                                         </asp:ListItem>
                                                                     <asp:ListItem Text="No" Value="N"></asp:ListItem>
                                                              </asp:RadioButtonList>
    
      </ItemTemplate>
                                                             <HeaderStyle Font-Bold="True" />
                                                        </asp:TemplateField> 
                                                         <asp:TemplateField HeaderText="Default State" HeaderStyle-Font-Bold="true" >


                                                            <HeaderTemplate>
                                                                 <asp:Label ID="TypeId" runat="server" Text="Default State"  ></asp:Label>
                                                                <%--OnSelectedIndexChanged="rdb_Defaultsate_sync_SelectedIndexChanged"--%>
                                                                  <%--<asp:RadioButtonList ID="rdb_Defaultsate_sync" CssClass="rdb_Defaultsate_sync" AutoPostBack="true" runat="server" EnableViewState="false" RepeatDirection="Horizontal"  RepeatLayout="Table"  
                                                                    TextAlign="Right" >
                                                                      <asp:ListItem Text="Yes" Value="Y"></asp:ListItem>
                                                                      <asp:ListItem Text="No" Value="N"></asp:ListItem>

                                                                  </asp:RadioButtonList>--%>

                                                                 <asp:RadioButton ID="rdb_DefaultsateYes" runat="server" Text="Yes" EnableViewState ="true"   OnCheckedChanged="rdb_DefaultsateYes_CheckedChanged" AutoPostBack="true" GroupName="g12"  />
                                                                 <asp:RadioButton ID="rdb_DefaultsateNo" runat="server" Text="No" EnableViewState ="true"   AutoPostBack="true" OnCheckedChanged="rdb_DefaultsateNo_CheckedChanged" GroupName="g12"  />



                                                             </HeaderTemplate>

                                                             <ItemTemplate>
        
    
                                                             <asp:RadioButtonList ID="rdb_defaultState" runat="server" SelectedValue='<%#Eval("Config_Value")%>' RepeatDirection="Horizontal" >
                                                               <asp:ListItem Text="Yes" Value="Y" Selected="True">
                                                                </asp:ListItem>
                                                               <asp:ListItem Text="No" Value="N"></asp:ListItem>
                                                              </asp:RadioButtonList>
    
                                                              </ItemTemplate>
                                                             <HeaderStyle Font-Bold="True" />
    </asp:TemplateField> 
                                                    </Columns>
                                                   <PagerStyle CssClass="pagerstyle" />
                                                    <HeaderStyle />
                                                    <RowStyle CssClass="tdstyle" />
                                                    <AlternatingRowStyle CssClass="alttdstyle" />
                                                </asp:GridView>
                          </div>
           
                         </ContentTemplate>
     </asp:UpdatePanel> 
    <asp:Label ID="lblsync" runat="server" Visible="false"></asp:Label>
    <asp:Label ID="lbldefault" runat="server" Visible="false"></asp:Label>

                        
                                       
    
             
           
                       
    
  
   
    
  
           <asp:UpdateProgress ID="UpdateProgress1" AssociatedUpdatePanelID="TopPanel"
                    runat="server">
                    <ProgressTemplate>
                        <asp:Panel ID="Panel11" CssClass="overlay" runat="server">
                            <img src="../assets/img/ajax-loader.gif" alt="Processing..." style="z-index: 10010; vertical-align: middle;" />
                            <span style="font-size: 12px; color: #3399ff;">Processing... </span>
                        </asp:Panel>
                    </ProgressTemplate>
                </asp:UpdateProgress>
              
         
</asp:Content>
