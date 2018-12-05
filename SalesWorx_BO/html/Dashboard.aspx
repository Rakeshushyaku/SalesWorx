<%@ Page Title="Dashboard" Language="vb" AutoEventWireup="false" MasterPageFile="~/html/Site.Master" CodeBehind="Dashboard.aspx.vb" Inherits="SalesWorx_BO.Dashboard" %>
<asp:Content ID="Content3" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="FeaturedContent" runat="server">
 <script type="text/javascript" src="../js/jquery.timers.js"></script>
 
    <style type="text/css">
	    .sectionHeader
	    {
		    display: block;
		    overflow:hidden;
		    width: 100%;
		    border-bottom:  #ccc solid 1px;
		    background-color: #fff;
		    color:#333;
		    text-align: left;
		    font-size: 14px;
            padding:8px;
	    }
        .sectionHeader i{
            font-size:21px;
            margin-left:5px;
        }

        .sectionHeader i.fa-info-circle{
            font-size:16px;
            margin:4px 5px 0;
            display:none;
        }

	    .sectionContent  
	    {
		    display: block;
		    height: 100%;
		    width: 100%;
		    overflow: hidden;
            border:  #fff solid 1px;
		    background-color: #fff;
            padding:15px;
            margin:0 0 25px;
	    }
    </style>

    <script type="text/javascript">
        var SECTION_SHOW_LINK = '<i class="fa fa-angle-down"></i>';
        var SECTION_HIDE_LINK = '<i class="fa fa-angle-up"></i>';
        
        $(window).load(function() {
            try {
                loadSections();
            }
            catch (exception) {
            }
        });

        function refreshSection(sectionID) {
            var tempSectionID = '#' + sectionID;

            if ($(tempSectionID)) {
                if ($(tempSectionID).is(':visible')) {
                    $(tempSectionID).attr("src", $(tempSectionID).attr("src"));
                }
            }
        }

        function toggleVisibility(sectionID) {
            var tempSectionID = '#' + sectionID;

            if ($(tempSectionID)) {
                setVisibilityIndicator(sectionID);

                $(tempSectionID).slideToggle();
            }
        }

        function setVisibilityIndicator(sectionID) {
            var tempSectionID = '#' + sectionID;

            if ($(tempSectionID).is(':visible')) {
                $('#VI_' + sectionID).html(SECTION_SHOW_LINK);
            }
            else {
                $('#VI_' + sectionID).html(SECTION_HIDE_LINK);
            }
        }

        function showInfo(sectionID) {
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    
    <h4>
       
       Dashboard</h4>
<table id="TblDashboard" width="100%" cellpadding="4" cellspacing="4" align="center" border="0" runat="server">
</table>
</asp:Content>
