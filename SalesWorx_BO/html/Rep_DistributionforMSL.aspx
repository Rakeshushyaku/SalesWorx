<%@ Page Title="Distribution for MSL Items" Language="vb" AutoEventWireup="false" MasterPageFile="~/html/Site.Master" CodeBehind="Rep_DistributionforMSL.aspx.vb" Inherits="SalesWorx_BO.Rep_DistributionforMSL" %>
<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=9.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    
    <style type="text/css">
        div.RadGrid .rgPager .rgAdvPart {
            display: none;
        }
         #ctl00_MainContent_gvRep_OT > tbody > tr:first-child {
            height:106px !important;
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
    <script>
        function alertCallBackFn(arg) {

        }

        function clickSearch() {
            $("#MainContent_SearchBtn").click()
        }
        function clickExportExcel() {
            $("#MainContent_BtnExportExcel").click()
            return false

        }
        function clickExportPDF() {
            $("#MainContent_BtnExportPDF").click()
            return false
        }

        function UpdateHeader() {

            //createChart3();

            $("#ctl00_MainContent_gvRep_ctl00_DataZone_ctl03_20").css("background-color", "#97c95d")
            $("#ctl00_MainContent_gvRep_ctl00_DataZone_ctl03_20").css("color", "#FFFFFF")
            $("#ctl00_MainContent_gvRep_ctl00_DataZone_ctl03_20").css("background-image", "none")
            $("#ctl00_MainContent_gvRep_ctl00_DataZone_ctl03_20").html("")

            $("#ctl00_MainContent_gvRep_ctl00_DataZone_ctl03_23").css("background-color", "#97c95d")
            $("#ctl00_MainContent_gvRep_ctl00_DataZone_ctl03_23").css("color", "#FFFFFF")
            $("#ctl00_MainContent_gvRep_ctl00_DataZone_ctl03_23").css("background-image", "none")
            $("#ctl00_MainContent_gvRep_ctl00_DataZone_ctl03_23").html("")

            $("#ctl00_MainContent_gvRep_ctl00_DataZone_ctl03_26").css("background-color", "#97c95d")
            $("#ctl00_MainContent_gvRep_ctl00_DataZone_ctl03_26").css("color", "#FFFFFF")
            $("#ctl00_MainContent_gvRep_ctl00_DataZone_ctl03_26").css("background-image", "none")
            $("#ctl00_MainContent_gvRep_ctl00_DataZone_ctl03_26").html("")
            $(".abc:nth-child(1)").removeAttr("colspan")
            $(".abc:nth-child(2)").removeAttr("colspan")
            $(".abc:nth-child(3)").removeAttr("colspan")

            $(".abc:nth-child(1)").css("background-color", "#97c95d")
            $(".abc:nth-child(2)").css("background-color", "#97c95d")
            $(".abc:nth-child(3)").css("background-color", "#97c95d")
            $(".abc:nth-child(1)").css("color", "#FFFFFF")
            $(".abc:nth-child(2)").css("color", "#FFFFFF")
            $(".abc:nth-child(3)").css("color", "#FFFFFF")
            $(".abc:nth-child(1)").css("background-image", "none")
            $(".abc:nth-child(2)").css("background-image", "none")
            $(".abc:nth-child(3)").css("background-image", "none")



            $("#ctl00_MainContent_gvRep_ctl00_DataZone_ctl03_29").css("background-color", "#14B4FC")
            $("#ctl00_MainContent_gvRep_ctl00_DataZone_ctl03_29").css("color", "#FFFFFF")
            $("#ctl00_MainContent_gvRep_ctl00_DataZone_ctl03_29").css("background-image", "none")
            $("#ctl00_MainContent_gvRep_ctl00_DataZone_ctl03_29").html("Entry")

            $("#ctl00_MainContent_gvRep_ctl00_DataZone_ctl03_210").css("background-color", "#14B4FC")
            $("#ctl00_MainContent_gvRep_ctl00_DataZone_ctl03_210").css("color", "#FFFFFF")
            $("#ctl00_MainContent_gvRep_ctl00_DataZone_ctl03_210").css("background-image", "none")
            $("#ctl00_MainContent_gvRep_ctl00_DataZone_ctl03_210").html("Invoiced")

            $("#ctl00_MainContent_gvRep_ctl00_DataZone_ctl03_211").css("background-color", "#14B4FC")
            $("#ctl00_MainContent_gvRep_ctl00_DataZone_ctl03_211").css("color", "#FFFFFF")
            $("#ctl00_MainContent_gvRep_ctl00_DataZone_ctl03_211").css("background-image", "none")
            $("#ctl00_MainContent_gvRep_ctl00_DataZone_ctl03_211").html("Exit")



            $("#ctl00_MainContent_gvRep_ctl00_DataZone_ctl03_212").css("background-color", "#14B4FC")
            $("#ctl00_MainContent_gvRep_ctl00_DataZone_ctl03_212").css("color", "#FFFFFF")
            $("#ctl00_MainContent_gvRep_ctl00_DataZone_ctl03_212").css("background-image", "none")
            $("#ctl00_MainContent_gvRep_ctl00_DataZone_ctl03_212").html("Entry")

            $("#ctl00_MainContent_gvRep_ctl00_DataZone_ctl03_213").css("background-color", "#14B4FC")
            $("#ctl00_MainContent_gvRep_ctl00_DataZone_ctl03_213").css("color", "#FFFFFF")
            $("#ctl00_MainContent_gvRep_ctl00_DataZone_ctl03_213").css("background-image", "none")
            $("#ctl00_MainContent_gvRep_ctl00_DataZone_ctl03_213").html("Invoiced")

            $("#ctl00_MainContent_gvRep_ctl00_DataZone_ctl03_214").css("background-color", "#14B4FC")
            $("#ctl00_MainContent_gvRep_ctl00_DataZone_ctl03_214").css("color", "#FFFFFF")
            $("#ctl00_MainContent_gvRep_ctl00_DataZone_ctl03_214").css("background-image", "none")
            $("#ctl00_MainContent_gvRep_ctl00_DataZone_ctl03_214").html("Exit")


            $("#ctl00_MainContent_gvRep_ctl00_DataZone_ctl03_215").css("background-color", "#14B4FC")
            $("#ctl00_MainContent_gvRep_ctl00_DataZone_ctl03_215").css("color", "#FFFFFF")
            $("#ctl00_MainContent_gvRep_ctl00_DataZone_ctl03_215").css("background-image", "none")
            $("#ctl00_MainContent_gvRep_ctl00_DataZone_ctl03_215").html("Entry")

            $("#ctl00_MainContent_gvRep_ctl00_DataZone_ctl03_216").css("background-color", "#14B4FC")
            $("#ctl00_MainContent_gvRep_ctl00_DataZone_ctl03_216").css("color", "#FFFFFF")
            $("#ctl00_MainContent_gvRep_ctl00_DataZone_ctl03_216").css("background-image", "none")
            $("#ctl00_MainContent_gvRep_ctl00_DataZone_ctl03_216").html("Invoiced")

            $("#ctl00_MainContent_gvRep_ctl00_DataZone_ctl03_217").css("background-color", "#14B4FC")
            $("#ctl00_MainContent_gvRep_ctl00_DataZone_ctl03_217").css("color", "#FFFFFF")
            $("#ctl00_MainContent_gvRep_ctl00_DataZone_ctl03_217").css("background-image", "none")
            $("#ctl00_MainContent_gvRep_ctl00_DataZone_ctl03_217").html("Exit")
             
            $(".abc:nth-child(4)").css("background-color", "#14B4FC")
            $(".abc:nth-child(5)").css("background-color", "#14B4FC")
            $(".abc:nth-child(6)").css("background-color", "#14B4FC")

            $(".abc:nth-child(4)").css("color", "#FFFFFF")
            $(".abc:nth-child(5)").css("color", "#FFFFFF")
            $(".abc:nth-child(6)").css("color", "#FFFFFF")
            $(".abc:nth-child(4)").css("background-image", "none")
            $(".abc:nth-child(5)").css("background-image", "none")
            $(".abc:nth-child(6)").css("background-image", "none")


            

            $("#ctl00_MainContent_gvRep_ctl00_DataZone_ctl01_ctl00_ColumnExpandCollapseButton_0_0").hide()
            $("#ctl00_MainContent_gvRep_ctl00_DataZone_ctl01_ctl01_ColumnExpandCollapseButton_9_0").hide()
            $("#ctl00_MainContent_gvRep_ctl00_DataZone_ctl01_ctl02_ColumnExpandCollapseButton_18_0").hide()
            


            $("#ctl00_MainContent_gvRep_ctl00_DataZone_ctl03_21").hide()
            $("#ctl00_MainContent_gvRep_ctl00_DataZone_ctl03_22").hide()
            $("#ctl00_MainContent_gvRep_ctl00_DataZone_ctl03_24").hide()
            $("#ctl00_MainContent_gvRep_ctl00_DataZone_ctl03_25").hide()
            $("#ctl00_MainContent_gvRep_ctl00_DataZone_ctl03_27").hide()
            $("#ctl00_MainContent_gvRep_ctl00_DataZone_ctl03_28").hide()
            $("#ctl00_MainContent_gvRep_ctl00_DataZone_ctl03_219").hide()
            $("#ctl00_MainContent_gvRep_ctl00_DataZone_ctl03_220").hide()
            $("#ctl00_MainContent_gvRep_ctl00_DataZone_ctl03_222").hide()
            $("#ctl00_MainContent_gvRep_ctl00_DataZone_ctl03_223").hide()
            $("#ctl00_MainContent_gvRep_ctl00_DataZone_ctl03_225").hide()
            $("#ctl00_MainContent_gvRep_ctl00_DataZone_ctl03_226").hide()

             

            $("#ctl00_MainContent_gvRep_ctl00_DataZone_ctl03_218").css("background-color", "#EF9933")
            $("#ctl00_MainContent_gvRep_ctl00_DataZone_ctl03_218").css("color", "#FFFFFF")
            $("#ctl00_MainContent_gvRep_ctl00_DataZone_ctl03_218").css("background-image", "none")
            $("#ctl00_MainContent_gvRep_ctl00_DataZone_ctl03_218").html("")

            $("#ctl00_MainContent_gvRep_ctl00_DataZone_ctl03_221").css("background-color", "#EF9933")
            $("#ctl00_MainContent_gvRep_ctl00_DataZone_ctl03_221").css("color", "#FFFFFF")
            $("#ctl00_MainContent_gvRep_ctl00_DataZone_ctl03_221").css("background-image", "none")
            $("#ctl00_MainContent_gvRep_ctl00_DataZone_ctl03_221").html("")

            $("#ctl00_MainContent_gvRep_ctl00_DataZone_ctl03_224").css("background-color", "#EF9933")
            $("#ctl00_MainContent_gvRep_ctl00_DataZone_ctl03_224").css("color", "#FFFFFF")
            $("#ctl00_MainContent_gvRep_ctl00_DataZone_ctl03_224").css("background-image", "none")
            $("#ctl00_MainContent_gvRep_ctl00_DataZone_ctl03_224").html("")


            $(".abc:nth-child(7)").removeAttr("colspan")
            $(".abc:nth-child(8)").removeAttr("colspan")
            $(".abc:nth-child(9)").removeAttr("colspan")


            $(".abc:nth-child(7)").css("background-color", "#EF9933")
            $(".abc:nth-child(8)").css("background-color", "#EF9933")
            $(".abc:nth-child(9)").css("background-color", "#EF9933")
            $(".abc:nth-child(7)").css("color", "#FFFFFF")
            $(".abc:nth-child(8)").css("color", "#FFFFFF")
            $(".abc:nth-child(9)").css("color", "#FFFFFF")
            $(".abc:nth-child(7)").css("background-image", "none")
            $(".abc:nth-child(8)").css("background-image", "none")
            $(".abc:nth-child(9)").css("background-image", "none")


            
        }


    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    
    <h4>Distribution for MSL Items</h4>

<telerik:RadWindowManager ID="RadWindowManager1" Skin="Simple" runat="server" EnableShadow="true">
    </telerik:RadWindowManager>

    <telerik:RadAjaxManager ID="RadAjaxManager2" runat="server">

        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="RadAjaxManager2">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadAjaxManager2" />
                </UpdatedControls>
            </telerik:AjaxSetting>

        </AjaxSettings>
    </telerik:RadAjaxManager>

   <telerik:RadAjaxPanel runat="server" ID="g">
           
        <asp:HiddenField ID="HSID" runat="server" />
        <asp:HiddenField ID="hfOrg" runat="server" />
       <asp:HiddenField ID="HUID" runat="server" />

        <asp:HiddenField ID="HM" runat="server" />
        <asp:HiddenField ID="HM1" runat="server" />
       <asp:HiddenField ID="HM2" runat="server" />
        <asp:HiddenField ID="HTitle" runat="server" />

                 <telerik:RadPanelBar runat="server" ID="rpbFilter" Skin="Simple" Width="100%"
                            ExpandMode="MultipleExpandedItems">
                            <Items>
                                <telerik:RadPanelItem Expanded="True" Text=" ">

                                    <ContentTemplate>

                                        <div class="row">
                                            <div class="col-sm-10">
                                                <div class="row">
                                                     <div class="col-sm-4">
                                            <div class="form-group">
                                                <label>Organization<em><span>&nbsp;</span>*</em></label>
                                                 <telerik:RadComboBox Skin="Simple"  Filter="Contains" ID="ddlOrganization"  Width ="100%"
                    runat="server" DataTextField="Description" DataValueField="MAS_Org_ID" AutoPostBack="true"> </telerik:RadComboBox>
                                            </div>
                                          </div>
          
                                                     <div class="col-sm-4">
                                            <div class="form-group">
                                                <label>Van/FSR</label>
                                                   <telerik:RadComboBox Skin="Simple" CheckBoxes="true" EmptyMessage="Select Van/FSR" EnableCheckAllItemsCheckBox="true" ID="ddl_Van" Width ="100%" runat="server" DataTextField="SalesRep_Name" DataValueField="SalesRep_ID" Filter="Contains">
                                        </telerik:RadComboBox >
                                                 </div>
                                             </div>
                                                    <div class="col-sm-4">
                                                    <div class="form-group">
                                                          <label>Month</label>
                                                        <telerik:RadMonthYearPicker RenderMode="Lightweight"  Width ="100%" ID="txtFromDate" runat="server">
                                                            <DateInput ReadOnly="true" DisplayDateFormat="MMM-yyyy">
                                                        </DateInput>
                                                         
                                                        </telerik:RadMonthYearPicker>   
                                                           
                                                    </div>
                                                  </div>
                                                    </div>
                                              </div>
                                                    
                                                    <div class="col-sm-2">
                                                 <div class="form-group">
                                                    <label>&nbsp;</label>
                                                <asp:Button  CssClass ="btn btn-sm btn-block btn-primary"  ID="SearchBtn" runat="server" Text="Search" />
                                                     <asp:Button  CssClass ="btn btn-sm btn-block btn-default"  ID="ClearBtn" runat="server" Text="Clear"  />
                                                    </div>
                                                <div class="form-group fontbig text-center">
                                                    <label>&nbsp;</label>
                                                <asp:HyperLink href="" CssClass=""  ID="BtnExportDummyExcel" runat="server" OnClick="return clickExportExcel()"><i class="fa fa-file-excel-o text-success"></i></asp:HyperLink>
                                                <asp:HyperLink href=""  CssClass =""  ID="BtnExportDummyPDF" runat="server" OnClick="return clickExportPDF()"><i class="fa fa-file-pdf-o text-danger"></i></asp:HyperLink>
                                                
                                            </div>
                                            </div>
                                        </div>

                                         </ContentTemplate>
                                        </telerik:RadPanelItem> 
                                     </Items> 
                    </telerik:RadPanelBar> 
       <div id="RepDiv" runat="server" >
                        <div id="Args" runat="server" visible="false">
        <div id="popoverblkouter">
            Hover on icon to view search criteria <i class="fa fa-info-circle">
            <span class="popoverblk">
              <p><strong>Organisation: </strong><asp:Label ID="lbl_org" runat="server" Text=""></asp:Label></p>
              <p><strong>Van: </strong> <asp:Label ID="lbl_van" runat="server" Text=""></asp:Label></p>
              <p><strong>Month: </strong> <asp:Label ID="lbl_Month" runat="server" Text=""></asp:Label></p>
               
            </span>
            </i>      
        </div>
    </div>

                            
            <div id="summary" runat="server" class="row"></div>
<p><br /></p>             
                              <div class="table-responsive">
                                 <telerik:RadPivotGrid  RenderMode="Lightweight" AllowPaging="true"  PageSize="10" EnableViewState ="true" 
                                                    ID="gvRep" runat="server"  AllowSorting="false" 
                                                    ShowFilterHeaderZone="false" AllowFiltering="false" ShowColumnHeaderZone="false" ShowRowHeaderZone="false" ShowDataHeaderZone="false"  
                                                     >
                                                    <TotalsSettings GrandTotalsVisibility="None"   ColumnsSubTotalsPosition="None"   />
                                                                                                        
                                                    <Fields>
                                                       
                                                          <telerik:PivotGridRowField DataField="Description" ZoneIndex="0"  UniqueName="Description" SortOrder="None" CellStyle-CssClass="nowhitespace" >
                                                                 
                                                                </telerik:PivotGridRowField>
                                                        <telerik:PivotGridColumnField DataField="Type" SortOrder="None">
                 
                                                        </telerik:PivotGridColumnField>
                                                        
                                                        <telerik:PivotGridColumnField DataField="Year" SortOrder="None" CellStyle-CssClass="abc nowhitespace"  >
                 
                                                        </telerik:PivotGridColumnField>

                                                         <telerik:PivotGridAggregateField DataField="C1" SortOrder="None" >  
                                                                     
                                                        </telerik:PivotGridAggregateField>
                                                        
                                                        
                                                         <telerik:PivotGridAggregateField DataField="C2" SortOrder="None" >  
                                                                     
                                                        </telerik:PivotGridAggregateField>

                                                        <telerik:PivotGridAggregateField DataField="C3" SortOrder="None" >  
                                                                     
                                                        </telerik:PivotGridAggregateField>
                                                         
                                                    </Fields>
                                                    <PagerStyle PageSizeControlType="None" Mode="NumericPages" />
                                                </telerik:RadPivotGrid>

                              </div>
            
       
        </div>
              

    </telerik:RadAjaxPanel>

    <div style="display: none">
        <asp:Button CssClass="btn btn-primary" ID="BtnExportExcel" runat="server" Text="Export" />
        <asp:Button CssClass="btn btn-primary" ID="BtnExportPDF" runat="server" Text="Export" />
    </div>
   <asp:UpdateProgress ID="UpdateProgress2" DisplayAfter="10"
        runat="server">
        <progresstemplate>
                                    <asp:Panel ID="Panel14" CssClass="overlay" runat="server">
                                        <img src="../assets/img/ajax-loader.gif" alt="Processing..." style="z-index: 10010; vertical-align: middle;" />
                                        <span style="font-size: 12px; color: #3399ff;">Processing... </span>
                                    </asp:Panel>
                                </progresstemplate>
    </asp:UpdateProgress>


</asp:Content>



