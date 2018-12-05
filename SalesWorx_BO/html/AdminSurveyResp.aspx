<%@ Page Title="Survey Response" Language="vb" AutoEventWireup="false" MasterPageFile="~/html/Site.Master"
    CodeBehind="AdminSurveyResp.aspx.vb" Inherits="SalesWorx_BO.AdminSurveyResp" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">

    <script type="text/javascript">
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
  </asp:Content>
 <asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">

     
        <h4>Survey Responses</h4>

    <telerik:RadAjaxLoadingPanel  ID="AjaxLoadingPanel1" runat="server" >
             <div ID="ProgressPanel" class ="overlay"  >
     
         <img src="../assets/img/ajax-loader.gif" alt="Processing..." style="z-index:10010;vertical-align:middle;" />      
       </div>
           </telerik:RadAjaxLoadingPanel>  

        <asp:Panel ID="Panel2" runat="server">      </asp:Panel> 
      <telerik:RadWindowManager ID="RadWindowManager1"  Skin="Simple" runat="server" EnableShadow="true">
     </telerik:RadWindowManager>



     
                            <asp:UpdatePanel ID="TopPanel" runat="server">
                                <ContentTemplate>
                                
                                     <div class="row">
                                        <div class="col-sm-3">
                                            <div class="form-group">
                                                <label id="lblSurvey" runat="server" >Survey</label>
                                             
                                                 <telerik:RadComboBox ID="ddSurvey" Skin="Simple"    Filter="Contains"  AutoPostBack="true"   Width="100%" Height="250px" TabIndex="2"
                                                        runat="server"> </telerik:RadComboBox>
                                                
                                            </div>
                                        </div>
                                    
                                        <div class="col-sm-3">
                                            <div class="form-group">
                                                <label id="lblQuestion1" runat="server" >Question</label>
                                             
                                                <telerik:RadComboBox ID="ddlQuestions" Skin="Simple" AppendDataBoundItems="true"   AutoPostBack="true"   Width="100%" Height="250px" TabIndex="2"
                                                        runat="server"> 
                                                    <Items>

                                                            <telerik:RadComboBoxItem Selected="True" Text="--Select a Question--" Value=""></telerik:RadComboBoxItem>                                                           
                                                     </Items> 

                                                </telerik:RadComboBox>
                                                
                                            </div>
                                        </div>
                                    
                                        <div class="col-sm-3">
                                            <div class="form-group">
                                                <label id="lblResponse" runat="server" >Response Type</label>
                                             
                                                <telerik:RadComboBox ID="ddlResponsType" Skin="Simple" AppendDataBoundItems="true"   AutoPostBack="true"   Width="100%" Height="250px" TabIndex="2"
                                                        runat="server">                          

                                                </telerik:RadComboBox>
                                                
                                            </div>
                                        </div>
                                    
                                        <div class="col-sm-3">
                                            <div class="form-group">
                                                <label id="lblOptCnt" runat="server" >Options</label>
                                             
                                                <telerik:RadComboBox ID="ddlOptCount" Skin="Simple" AppendDataBoundItems="true"   AutoPostBack="true"   Width="100%" Height="250px" TabIndex="2"
                                                        runat="server">                          

                                                </telerik:RadComboBox>
                                                
                                            </div>
                                        </div>
                                    </div>
                                     <div class="row">
                                        <div class="col-sm-6">
                                            <div class="form-group">
                                                 <asp:GridView Width="100%" ID="gvResponse" runat="server"  AutoGenerateColumns="False"
                                                                    PageSize="10" CellPadding="0" CellSpacing="0" CssClass="tablecellalign">
                                                                  
                                                                    <Columns>
                                                                        <asp:TemplateField>
                                                                            <HeaderTemplate>
                                                                                <asp:Label ID="lblHeader" runat="server" Text="Option" />
                                                                            </HeaderTemplate>
                                                                            <HeaderStyle HorizontalAlign="Center" />
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblOption" Font-Bold="false" runat="server" Text='<%# Bind("SrNo") %>' />
                                                                            </ItemTemplate>
                                                                            <ItemStyle HorizontalAlign="Left" />
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField>
                                                                            <HeaderTemplate>
                                                                                <asp:Label ID="lblText" runat="server" Text="Response Text" />
                                                                            </HeaderTemplate>
                                                                            <HeaderStyle HorizontalAlign="Center" />
                                                                            <ItemTemplate>
                                                                                <asp:TextBox ID="txtOptValue" runat="server" Text='<%# Bind("OptValue") %>' Width="100%" CssClass="inputSM"
                                                                                    TabIndex="5" />
                                                                            </ItemTemplate>
                                                                            <ItemStyle HorizontalAlign="Left" />
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField>
                                                                            <HeaderTemplate>
                                                                                <asp:Label ID="lblDefault" runat="server" Text="Is Default" />
                                                                            </HeaderTemplate>
                                                                            <HeaderStyle HorizontalAlign="Left" />
                                                                            <ItemTemplate>
                                                                                <div style="padding-top:14px;">
                                                                                <asp:CheckBox ID="ChkDefault" AutoPostBack="True" runat="server" Text="" TabIndex="6"
                                                                                    OnCheckedChanged="ChkDefault_CheckedChanged" Checked='<%# Bind("DefValue") %>' />
                                                                                    </div>
                                                                            </ItemTemplate>
                                                                            <ItemStyle HorizontalAlign="Left" />
                                                                        </asp:TemplateField>
                                                                    </Columns>
                                                                  <PagerStyle CssClass="pagerstyle" />
                                                    <HeaderStyle />
                                                    <RowStyle CssClass="tdstyle" />
                                                    <AlternatingRowStyle CssClass="alttdstyle" />
                                                                </asp:GridView>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-sm-12">
                                            <div class="form-group">
                                           <telerik:RadButton ID="btnAdd"   Skin ="Simple" TabIndex="5"   runat="server" Text="Add" 
                                               CssClass ="btn btn-success" />
                                          <telerik:RadButton ID="btnSave"   Skin ="Simple" TabIndex="6"   runat="server" Text="Save" 
                                               CssClass ="btn btn-success" />
                                         <telerik:RadButton ID="btnModify"  Skin ="Simple" TabIndex="7"   runat="server" Text="Modify" 
                                               CssClass ="btn btn-warning" />
                                           <telerik:RadButton ID="btnDelete"  Skin ="Simple" TabIndex="8"   runat="server" Text="Delete" 
                                               CssClass ="btn btn-danger" />
                                        <telerik:RadButton ID="btnCancel"   Skin ="Simple" TabIndex="9"   runat="server" Text="Cancel" 
                                               CssClass ="btn btn-default" />
                                                </div>
                                       </div>
                                    </div>

                                          
                                 
                                </ContentTemplate>
                            </asp:UpdatePanel>
              
      
           
                <asp:UpdateProgress ID="UpdateProgress1"  runat="server">
                    <ProgressTemplate>
                        <asp:Panel ID="Panel11" CssClass="overlay" runat="server">
                            <img src="../assets/img/ajax-loader.gif" alt="Processing..." style="z-index: 10010; vertical-align: middle;" />
                            <span style="font-size: 12px; color: #e10000;">Processing... </span>
                        </asp:Panel>
                    </ProgressTemplate>
                </asp:UpdateProgress>

</asp:Content>
