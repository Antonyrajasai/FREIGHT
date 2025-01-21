<%@ Page Language="C#" AutoEventWireup="true" CodeFile="GSTR3B_IFrame.aspx.cs" Inherits="Acc_Reports_GSTR3B_IFrame" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
   <style type="text/css">
   #ReportViewer1
   {
       background-color:white;
   }
   
   </style>
 
</head>
<body style="overflow:hidden;">
    <form id="form1" runat="server">
     <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePartialRendering="false"></asp:ScriptManager>
     <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional" Visible="true">
        <ContentTemplate>
    <div class="loading" align="center" id="load" style="display:none;">
   
    <img src="../Loading_Images/indicator_mozilla_blu.gif" alt="" />
     
</div>
  <asp:Literal ID="ltmsg" runat="server"></asp:Literal>
   <div style="margin:0px 10px 5px 10px; ">
       
<%--       <div ID="mainmastop2container_rght_tag2_txt1" style="width:100%; margin-top:22px;">
               <asp:ImageButton ID="btnexcel" runat="server"   ImageUrl="~/images/xls.jpg"  
                   Height="28px" Width="28px" onclick="btnexcel_Click"  />--%>
       </div>
   
       <div id="innerbox_MidMain_Trans_IGM_Invoice" style=" width:100%; height:500px;"> <%--width:820px; toc_new--%>
           <rsweb:ReportViewer ID="ReportViewer1" runat="server" AsyncRendering="False"  
               BackColor="#F2F5F2" EnableTheming="false"  
               ExportContentDisposition="AlwaysAttachment" Height="500px" Width="100%" ProcessingMode="Local" 
               ShowBackButton="false" ShowDocumentMapButton="true" ShowExportControls="true" 
               ShowFindControls="False" ShowPageNavigationControls="true" 
               ShowParameterPrompts="false" ShowPrintButton="false" ShowRefreshButton="true" 
               ShowReportBody="true" ShowToolBar="true" ShowZoomControl="false" 
               SizeToReportContent="false" ToolBarItemBorderColor="Black" 
               ToolBarItemBorderStyle="Solid" BorderColor="#CCCCCC" BorderStyle="Solid" 
               BorderWidth="2px">
              
           </rsweb:ReportViewer>
       </div>
       
    

           </div>
             </ContentTemplate>
    </asp:UpdatePanel>       
           </form>
           
</body>
</html>
