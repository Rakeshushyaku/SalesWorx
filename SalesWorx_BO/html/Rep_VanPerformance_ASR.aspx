<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/html/Site.Master" CodeBehind="Rep_VanPerformance_ASR.aspx.vb" Inherits="SalesWorx_BO.Rep_VanPerformance_ASR" %>
<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=9.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <style type="text/css">
        div[id*="ReportDiv"] {
            overflow: hidden !important;
            WIDTH: 100%;
            direction: ltr;
        }
    </style>
    <script type="text/javascript">
        function alertCallBackFn(arg) {

        }
        function clickExportBiffExcel() {

            $("#MainContent_BtnExportBiffExcel").click()
            return false

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
            /* $("#ctl00_MainContent_gvRep_ctl00_DataZone_ctl03_212").hide()
            $("#ctl00_MainContent_gvRep_ctl00_DataZone_ctl03_222").hide()
            $("#ctl00_MainContent_gvRep_ctl00_DataZone_ctl03_223").hide()
            $("#ctl00_MainContent_gvRep_ctl00_DataZone_ctl03_233").hide()
            $("#ctl00_MainContent_gvRep_ctl00_DataZone_ctl03_234").hide()
            $("#ctl00_MainContent_gvRep_ctl00_DataZone_ctl03_235").hide()
            $("#ctl00_MainContent_gvRep_ctl00_DataZone_ctl03_29").hide()
            $("#ctl00_MainContent_gvRep_ctl00_DataZone_ctl03_210").hide()
            $("#ctl00_MainContent_gvRep_ctl00_DataZone_ctl03_211").hide()*/

          /*  $("#ctl00_MainContent_gvRep_ctl00_DataZone_ctl03_20").html("Total Calls")
            $("#ctl00_MainContent_gvRep_ctl00_DataZone_ctl03_20").css("background-color", "#F97166")
            $("#ctl00_MainContent_gvRep_ctl00_DataZone_ctl03_20").css("color", "#FFFFFF")
            $("#ctl00_MainContent_gvRep_ctl00_DataZone_ctl03_20").css("background-image", "none")

            $("#ctl00_MainContent_gvRep_ctl00_DataZone_ctl03_213").html("Total Calls")
            $("#ctl00_MainContent_gvRep_ctl00_DataZone_ctl03_213").css("background-color", "#F97166")
            $("#ctl00_MainContent_gvRep_ctl00_DataZone_ctl03_213").css("color", "#FFFFFF")
            $("#ctl00_MainContent_gvRep_ctl00_DataZone_ctl03_213").css("background-image", "none")

            $("#ctl00_MainContent_gvRep_ctl00_DataZone_ctl03_224").html("Total Calls")
            $("#ctl00_MainContent_gvRep_ctl00_DataZone_ctl03_224").css("background-color", "#F97166")
            $("#ctl00_MainContent_gvRep_ctl00_DataZone_ctl03_224").css("color", "#FFFFFF")
            $("#ctl00_MainContent_gvRep_ctl00_DataZone_ctl03_224").css("background-image", "none") 
            //

            $("#ctl00_MainContent_gvRep_ctl00_DataZone_ctl03_21").html("Total Productive Calls")
            $("#ctl00_MainContent_gvRep_ctl00_DataZone_ctl03_21").css("background-color", "#36C5B2")
            $("#ctl00_MainContent_gvRep_ctl00_DataZone_ctl03_21").css("color", "#FFFFFF")
            $("#ctl00_MainContent_gvRep_ctl00_DataZone_ctl03_21").css("background-image", "none")

            $("#ctl00_MainContent_gvRep_ctl00_DataZone_ctl03_214").html("Total Productive Calls")
            $("#ctl00_MainContent_gvRep_ctl00_DataZone_ctl03_214").css("background-color", "#36C5B2")
            $("#ctl00_MainContent_gvRep_ctl00_DataZone_ctl03_214").css("color", "#FFFFFF")
            $("#ctl00_MainContent_gvRep_ctl00_DataZone_ctl03_214").css("background-image", "none")

            $("#ctl00_MainContent_gvRep_ctl00_DataZone_ctl03_225").html("Total Productive Calls")
            $("#ctl00_MainContent_gvRep_ctl00_DataZone_ctl03_225").css("background-color", "#36C5B2")
            $("#ctl00_MainContent_gvRep_ctl00_DataZone_ctl03_225").css("color", "#FFFFFF")
            $("#ctl00_MainContent_gvRep_ctl00_DataZone_ctl03_225").css("background-image", "none")

            //

            $("#ctl00_MainContent_gvRep_ctl00_DataZone_ctl03_22").html("Call Productive %")
            $("#ctl00_MainContent_gvRep_ctl00_DataZone_ctl03_22").css("background-color", "#EB963C")
            $("#ctl00_MainContent_gvRep_ctl00_DataZone_ctl03_22").css("color", "#FFFFFF")
            $("#ctl00_MainContent_gvRep_ctl00_DataZone_ctl03_22").css("background-image", "none")

            $("#ctl00_MainContent_gvRep_ctl00_DataZone_ctl03_215").html("Call Productive %")
            $("#ctl00_MainContent_gvRep_ctl00_DataZone_ctl03_215").css("background-color", "#EB963C")
            $("#ctl00_MainContent_gvRep_ctl00_DataZone_ctl03_215").css("color", "#FFFFFF")
            $("#ctl00_MainContent_gvRep_ctl00_DataZone_ctl03_215").css("background-image", "none")

            $("#ctl00_MainContent_gvRep_ctl00_DataZone_ctl03_226").html("Call Productive %")
            $("#ctl00_MainContent_gvRep_ctl00_DataZone_ctl03_226").css("background-color", "#EB963C")
            $("#ctl00_MainContent_gvRep_ctl00_DataZone_ctl03_226").css("color", "#FFFFFF")
            $("#ctl00_MainContent_gvRep_ctl00_DataZone_ctl03_226").css("background-image", "none")

            //


            $("#ctl00_MainContent_gvRep_ctl00_DataZone_ctl03_23").html("JP Calls")
            $("#ctl00_MainContent_gvRep_ctl00_DataZone_ctl03_23").css("background-color", "#36C5B2")
            $("#ctl00_MainContent_gvRep_ctl00_DataZone_ctl03_23").css("color", "#FFFFFF")
            $("#ctl00_MainContent_gvRep_ctl00_DataZone_ctl03_23").css("background-image", "none")

            $("#ctl00_MainContent_gvRep_ctl00_DataZone_ctl03_216").html("JP Calls")
            $("#ctl00_MainContent_gvRep_ctl00_DataZone_ctl03_216").css("background-color", "#36C5B2")
            $("#ctl00_MainContent_gvRep_ctl00_DataZone_ctl03_216").css("color", "#FFFFFF")
            $("#ctl00_MainContent_gvRep_ctl00_DataZone_ctl03_216").css("background-image", "none")

            $("#ctl00_MainContent_gvRep_ctl00_DataZone_ctl03_227").html("JP Calls")
            $("#ctl00_MainContent_gvRep_ctl00_DataZone_ctl03_227").css("background-color", "#36C5B2")
            $("#ctl00_MainContent_gvRep_ctl00_DataZone_ctl03_227").css("color", "#FFFFFF")
            $("#ctl00_MainContent_gvRep_ctl00_DataZone_ctl03_227").css("background-image", "none")


            //


            $("#ctl00_MainContent_gvRep_ctl00_DataZone_ctl03_24").html("Actual Calls as per JP")
            $("#ctl00_MainContent_gvRep_ctl00_DataZone_ctl03_24").css("background-color", "#36C5B2")
            $("#ctl00_MainContent_gvRep_ctl00_DataZone_ctl03_24").css("color", "#FFFFFF")
            $("#ctl00_MainContent_gvRep_ctl00_DataZone_ctl03_24").css("background-image", "none")

            $("#ctl00_MainContent_gvRep_ctl00_DataZone_ctl03_217").html("Actual Calls as per JP")
            $("#ctl00_MainContent_gvRep_ctl00_DataZone_ctl03_217").css("background-color", "#36C5B2")
            $("#ctl00_MainContent_gvRep_ctl00_DataZone_ctl03_217").css("color", "#FFFFFF")
            $("#ctl00_MainContent_gvRep_ctl00_DataZone_ctl03_217").css("background-image", "none")

            $("#ctl00_MainContent_gvRep_ctl00_DataZone_ctl03_228").html("Actual Calls as per JP")
            $("#ctl00_MainContent_gvRep_ctl00_DataZone_ctl03_228").css("background-color", "#36C5B2")
            $("#ctl00_MainContent_gvRep_ctl00_DataZone_ctl03_228").css("color", "#FFFFFF")
            $("#ctl00_MainContent_gvRep_ctl00_DataZone_ctl03_228").css("background-image", "none")

            //


            $("#ctl00_MainContent_gvRep_ctl00_DataZone_ctl03_25").html("JP Adherence %")
            $("#ctl00_MainContent_gvRep_ctl00_DataZone_ctl03_25").css("background-color", "#36C5B2")
            $("#ctl00_MainContent_gvRep_ctl00_DataZone_ctl03_25").css("color", "#FFFFFF")
            $("#ctl00_MainContent_gvRep_ctl00_DataZone_ctl03_25").css("background-image", "none")

            $("#ctl00_MainContent_gvRep_ctl00_DataZone_ctl03_218").html("JP Adherence %")
            $("#ctl00_MainContent_gvRep_ctl00_DataZone_ctl03_218").css("background-color", "#36C5B2")
            $("#ctl00_MainContent_gvRep_ctl00_DataZone_ctl03_218").css("color", "#FFFFFF")
            $("#ctl00_MainContent_gvRep_ctl00_DataZone_ctl03_218").css("background-image", "none")

            $("#ctl00_MainContent_gvRep_ctl00_DataZone_ctl03_229").html("JP Adherence %")
            $("#ctl00_MainContent_gvRep_ctl00_DataZone_ctl03_229").css("background-color", "#36C5B2")
            $("#ctl00_MainContent_gvRep_ctl00_DataZone_ctl03_229").css("color", "#FFFFFF")
            $("#ctl00_MainContent_gvRep_ctl00_DataZone_ctl03_229").css("background-image", "none")*/


            //
/*
            $("#ctl00_MainContent_gvRep_ctl00_DataZone_ctl03_26").html("Unique Customers Covered")
            $("#ctl00_MainContent_gvRep_ctl00_DataZone_ctl03_26").css("background-color", "#398C59")
            $("#ctl00_MainContent_gvRep_ctl00_DataZone_ctl03_26").css("color", "#FFFFFF")
            $("#ctl00_MainContent_gvRep_ctl00_DataZone_ctl03_26").css("background-image", "none")

            $("#ctl00_MainContent_gvRep_ctl00_DataZone_ctl03_219").html("Unique Customers Covered")
            $("#ctl00_MainContent_gvRep_ctl00_DataZone_ctl03_219").css("background-color", "#398C59")
            $("#ctl00_MainContent_gvRep_ctl00_DataZone_ctl03_219").css("color", "#FFFFFF")
            $("#ctl00_MainContent_gvRep_ctl00_DataZone_ctl03_219").css("background-image", "none")

            $("#ctl00_MainContent_gvRep_ctl00_DataZone_ctl03_230").html("Unique Customers Covered")
            $("#ctl00_MainContent_gvRep_ctl00_DataZone_ctl03_230").css("background-color", "#398C59")
            $("#ctl00_MainContent_gvRep_ctl00_DataZone_ctl03_230").css("color", "#FFFFFF")
            $("#ctl00_MainContent_gvRep_ctl00_DataZone_ctl03_230").css("background-image", "none")

            //

            $("#ctl00_MainContent_gvRep_ctl00_DataZone_ctl03_220").html("Unique Productive Customers")
            $("#ctl00_MainContent_gvRep_ctl00_DataZone_ctl03_220").css("background-color", "#A5304D")
            $("#ctl00_MainContent_gvRep_ctl00_DataZone_ctl03_220").css("color", "#FFFFFF")
            $("#ctl00_MainContent_gvRep_ctl00_DataZone_ctl03_220").css("background-image", "none")

            $("#ctl00_MainContent_gvRep_ctl00_DataZone_ctl03_27").html("Unique Productive Customers")
            $("#ctl00_MainContent_gvRep_ctl00_DataZone_ctl03_27").css("background-color", "#A5304D")
            $("#ctl00_MainContent_gvRep_ctl00_DataZone_ctl03_27").css("color", "#FFFFFF")
            $("#ctl00_MainContent_gvRep_ctl00_DataZone_ctl03_27").css("background-image", "none")

            $("#ctl00_MainContent_gvRep_ctl00_DataZone_ctl03_231").html("Unique Productive Customers")
            $("#ctl00_MainContent_gvRep_ctl00_DataZone_ctl03_231").css("background-color", "#A5304D")
            $("#ctl00_MainContent_gvRep_ctl00_DataZone_ctl03_231").css("color", "#FFFFFF")
            $("#ctl00_MainContent_gvRep_ctl00_DataZone_ctl03_231").css("background-image", "none")
            //

            $("#ctl00_MainContent_gvRep_ctl00_DataZone_ctl03_28").html("Customer Productive %")
            $("#ctl00_MainContent_gvRep_ctl00_DataZone_ctl03_28").css("background-color", "#F97166")
            $("#ctl00_MainContent_gvRep_ctl00_DataZone_ctl03_28").css("color", "#FFFFFF")
            $("#ctl00_MainContent_gvRep_ctl00_DataZone_ctl03_28").css("background-image", "none")

            $("#ctl00_MainContent_gvRep_ctl00_DataZone_ctl03_221").html("Customer Productive %")
            $("#ctl00_MainContent_gvRep_ctl00_DataZone_ctl03_221").css("background-color", "#F97166")
            $("#ctl00_MainContent_gvRep_ctl00_DataZone_ctl03_221").css("color", "#FFFFFF")
            $("#ctl00_MainContent_gvRep_ctl00_DataZone_ctl03_221").css("background-image", "none")

            $("#ctl00_MainContent_gvRep_ctl00_DataZone_ctl03_232").html("Customer Productive %")
            $("#ctl00_MainContent_gvRep_ctl00_DataZone_ctl03_232").css("background-color", "#F97166")
            $("#ctl00_MainContent_gvRep_ctl00_DataZone_ctl03_232").css("color", "#FFFFFF")
            $("#ctl00_MainContent_gvRep_ctl00_DataZone_ctl03_232").css("background-image", "none")*/
           
        }
        function pageLoad(sender, args) {

            $('[data-toggle="tooltip"]').tooltip();
        }

    </script>
    <style type="text/css">
        div.RadPivotGrid .rgPager .rgAdvPart {
            display: none;
        }

        .RadPivotGrid .rpgRowsZone span.rpgFieldItem {
            background: none;
            border: none;
        }

        .RadPivotGrid span.rpgFieldItem {
            background: none;
            border: none;
        }
    </style>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">

    <h4>Van/FSR Performance</h4>
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

    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
        <contenttemplate>
      

                 <telerik:RadPanelBar runat="server" ID="rpbFilter" Skin="Simple" Width="100%"
                            ExpandMode="MultipleExpandedItems">
                            <Items>
                                <telerik:RadPanelItem Expanded="True" Text=" ">

                                    <ContentTemplate>

                                        <div class="row">
                                            <div class="col-sm-10">
                                                <div class="row">
                                                    <div class="col-sm-3" runat="server" id="dvCountry">
                                            <div class="form-group">
                                                <label>Country</label>
                                                <telerik:RadComboBox Skin="Simple" EmptyMessage="Select Country" ID="ddlCountry" Width ="100%" runat="server" DataTextField="Country" DataValueField="MAS_ORG_ID"  AutoPostBack="true" >
                                            </telerik:RadComboBox>
                                               
                                            </div>
                                        </div>
                                                     <div class="col-sm-5">
                                            <div class="form-group">
                                                <label>Organization<em><span>&nbsp;</span>*</em></label>
                                                <telerik:RadComboBox Skin="Simple"  CheckBoxes="true" EnableCheckAllItemsCheckBox="true" Filter="Contains" ID="ddlOrganization" Width ="100%" runat="server" DataTextField="Description" DataValueField="MAS_Org_ID" AutoPostBack="true">
                                        </telerik:RadComboBox>
                                            </div>
                                          </div>
          
                                                     <div class="col-sm-4">
                                                          <div class="form-group">
                                                            <label>Van/FSR</label>
                                                           <telerik:RadComboBox Skin="Simple" CheckBoxes="true" EmptyMessage="Select Van/FSR" EnableCheckAllItemsCheckBox="true" ID="ddlVan" Width ="100%" runat="server" DataTextField="SalesRep_Name" DataValueField="SalesRep_ID" Filter="Contains">
                                                            </telerik:RadComboBox >
                                                         </div>
                                                     </div>
                                    
                                               
                                                      <div class="col-sm-3">
                                                           <div class="form-group">
                                                               <label>From Month </label>
                                                                 <telerik:RadMonthYearPicker RenderMode="Lightweight" ID="txtFromDate" runat="server" Width ="100%">
                                                                <DateInput ReadOnly="true" DisplayDateFormat="MMM-yyyy">
                                                                    </DateInput>
                                                         
                                                            </telerik:RadMonthYearPicker>    

                                                            </div>
                                                      </div>
                                                      <div class="col-sm-3">
                                                           <div class="form-group">
                                                               <label>End Month </label>
                                                               <telerik:RadMonthYearPicker RenderMode="Lightweight" ID="txtToDate" runat="server" Width ="100%">
                                                                 <DateInput ReadOnly="true" DisplayDateFormat="MMM-yyyy">
                                                            </DateInput>
                                                            </telerik:RadMonthYearPicker> 

                                                            </div>
                                                      </div>
                                                      

             
                                                      <div class="col-sm-6" style="display:none ">
                                                        <div class="form-group"  >
                                                            <label>Sales District</label>
                                                             <telerik:RadComboBox Skin="Simple"  Filter="Contains" ID="ddlSalesDist"  Width ="100%"
                                                              runat="server" DataTextField="Description" DataValueField="Sales_District_ID">
                                                             </telerik:RadComboBox>
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

            

                        <div id="Args" runat="server" visible="false">
        <div id="popoverblkouter">
            Hover on icon to view search criteria <i class="fa fa-info-circle">
            <span class="popoverblk">
              <p><strong>Organisation: </strong><asp:Label ID="lbl_org" runat="server" Text=""></asp:Label></p>
              <p><strong>Van: </strong> <asp:Label ID="lbl_van" runat="server" Text=""></asp:Label></p>
              <p><strong>From Date: </strong><asp:Label ID="lbl_from" runat="server" Text=""></asp:Label></p>
              <p><strong>To Date: </strong><asp:Label ID="lbl_To" runat="server" Text=""></asp:Label></p>  
             <p><strong>Sales District: </strong><asp:Label ID="lbl_district" runat="server" Text=""></asp:Label></p>  
            </span>
            </i>      
        </div>
    </div>
           
                            
            <div id="summary" runat="server" class="row"></div> 
                               

       <div style="position:relative;">
            <div style="position:absolute;padding:5px;top:0;left:0;" >
                                     <asp:ImageButton ID="img_export" runat="server" ImageUrl ="../assets/img/export-excel.png"  Visible="false"  OnClientClick="clickExportBiffExcel()"></asp:ImageButton>
                            </div>
          

             <telerik:RadPivotGrid  RenderMode="Lightweight" AllowPaging="true"  PageSize="10" EnableViewState ="true" 
                                                    ID="gvRep" runat="server"  
                                                    ShowFilterHeaderZone="false" AllowFiltering="false" ShowColumnHeaderZone="false" ShowRowHeaderZone="false" ShowDataHeaderZone="false" cssClass="no-wrap"  
                                                     >
                                                    <TotalsSettings GrandTotalsVisibility="None"  ColumnsSubTotalsPosition="None" />
                                                                                                        
                                                    <Fields>
                                                       
                                                       
                                                       
                                                          <telerik:PivotGridRowField DataField="Description" ZoneIndex="0"  Caption="Name" UniqueName="Description" >
                                                                </telerik:PivotGridRowField>
                                                        <telerik:PivotGridColumnField DataField="Year" SortOrder="None">
                 
                                                        </telerik:PivotGridColumnField>
                                                        <telerik:PivotGridColumnField DataField="Type" SortOrder="None" >
                 
                                                        </telerik:PivotGridColumnField>
                                                        
                                                         <telerik:PivotGridAggregateField DataField="C1" SortOrder="None" >  
                                                                     
                                                        </telerik:PivotGridAggregateField>
                                                        <telerik:PivotGridAggregateField DataField="C2" SortOrder="None">                 
                                                        </telerik:PivotGridAggregateField>
                                                        <telerik:PivotGridAggregateField  DataField="C3" SortOrder="None">  
                                                                    
                                                        </telerik:PivotGridAggregateField>
                                                         <telerik:PivotGridAggregateField  DataField="C4" SortOrder="None">  
                                                                    
                                                        </telerik:PivotGridAggregateField>
                                                        <telerik:PivotGridAggregateField  DataField="C5" SortOrder="None">  
                                                                    
                                                        </telerik:PivotGridAggregateField>
                                                        <telerik:PivotGridAggregateField  DataField="C6" SortOrder="None">  
                                                                    
                                                        </telerik:PivotGridAggregateField>
                                                         
                                                       
                                                    </Fields>
                                                    <PagerStyle PageSizeControlType="None" Mode="NumericPages" />
                                                </telerik:RadPivotGrid>
                                                                                                               
                                 </div>                       
                                                        


        

             <asp:HiddenField id="hfCurrency" runat="server"></asp:HiddenField>
             <asp:HiddenField id="hfCurDecimal" runat="server"></asp:HiddenField>
            
    </contenttemplate>
    </asp:UpdatePanel>
    <div style="display: none">
        <asp:Button CssClass="btn btn-primary" ID="BtnExportExcel" runat="server" Text="Export" />
        <asp:Button CssClass="btn btn-primary" ID="BtnExportPDF" runat="server" Text="Export" />
        <asp:Button  CssClass ="btn btn-primary"  ID="BtnExportBiffExcel" runat="server" Text="Export" />
    </div>
    <asp:UpdateProgress ID="UpdateProgress2" AssociatedUpdatePanelID="UpdatePanel1" runat="server" DisplayAfter="10">
        <progresstemplate>
     <asp:Panel ID="Panel11" CssClass="overlay" runat="server">
     
         <img src="../assets/img/ajax-loader.gif" alt="Processing..." style="vertical-align:middle;" />            
           <span style="font-size:12px; font-weight:bold; color:#3399ff;">Processing... </span>
       </asp:Panel>
            
       
         
    </progresstemplate>
    </asp:UpdateProgress>

</asp:Content>
