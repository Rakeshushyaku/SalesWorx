<%@ Page Title="Dashboard" Language="vb" AutoEventWireup="false" MasterPageFile="~/html/DefaultLayout.Master" CodeBehind="Dashboard.aspx.vb" Inherits="SalesWorx_BO.Dashboard" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
 <script type="text/javascript" src="../js/jquery.timers.js"></script>
 
    <style type="text/css">
	    .sectionHeader
	    {
		    display: block;
		    height: 20px;
		    width: 100%;

		    border-style: solid;
		    border-width: 1px;
		    border-color: lightgrey;
		    background-color: #EFEFEF;
		    color:#333;
    		
		    text-align: left;
		    font-family: calibri;
		    font-size: 14px;
		    font-weight: bold;
	    }

	    .sectionContent  
	    {
		    display: block;
		    height: 100%;
		    width: 100%;
		    overflow: hidden;

		    border-style: solid;
		    border-width: 1px;
		    border-color: lightgrey;
	    }
    </style>

    <script type="text/javascript">
        var SECTION_SHOW_LINK = '<img src="../images/icon_show.png">';
        var SECTION_HIDE_LINK = '<img src="../images/icon.bmp">';
        
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
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

<table id="TblDashboard" width="950px" cellpadding="4" cellspacing="4" align="center" border="0" runat="server">
</table>
</asp:Content>
