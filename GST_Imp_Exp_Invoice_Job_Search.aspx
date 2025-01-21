<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/eRoyalFreightMasters.master" AutoEventWireup="true" 
CodeFile="GST_Imp_Exp_Invoice_Job_Search.aspx.cs" Inherits="GST_Imp_Exp_Invoice_Job_Search" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <style type="text/css">
        .ad
        {
            width: 50px;
        }
    </style>

       <style type="text/css">
        th a
        {
            text-decoration: none;
        }
        .padding
        {
           padding-left:10px;
           padding-top:3px;
        }
        
    </style>

    <style type="text/css">
   
    td
    {
        cursor: pointer;
    }
    .selected_row
    {
        background-color: white;
        font-weight:bold;
        /*color:#5E4C4C;*/
         color:#3C3535;
    }
    
    
</style>


<script language ="javascript" type="text/javascript">
    document.body.style.cursor = 'pointer';
    var oldColor = '';
    var jobno;

    function ChangeRowColor(rowID, Value, Bill_invno, IMP_EXP) {
        document.getElementById("<%=hdrowindex.ClientID %>").value = Value;
        document.getElementById("<%=hdBill_invno.ClientID %>").value = Bill_invno;
        document.getElementById("<%=hd_Imp_Exp.ClientID %>").value = IMP_EXP;
    }
</script>

<script type = "text/javascript">
    function MutExChkList(chk) {
        if (chk.checked == true) {
            document.getElementById("<%=ddltype.ClientID %>").disabled = false;
            document.getElementById("<%=txtsearch.ClientID %>").disabled = false;
        }
        else {
            document.getElementById("<%=ddltype.ClientID %>").disabled = true;
            document.getElementById("<%=txtsearch.ClientID %>").disabled = true;
            document.getElementById("<%=txtsearch.ClientID %>").value = '';
            document.getElementById("<%=ddltype.ClientID %>").value = '0';

        }
    }
        </script>



 <script type="text/javascript">
     $(function () {
         $("[id*=gvdetails] td").bind("click", function () {
             var row = $(this).parent();
             $("[id*=gvdetails] tr").each(function () {
                 if ($(this)[0] != row[0]) {
                     $("td", this).removeClass("selected_row");
                 }
             });
             $("td", row).each(function () {
                 if (!$(this).hasClass("selected_row")) {
                     $(this).addClass("selected_row");
                 } else {
                     $(this).removeClass("selected_row");
                 }
             });
         });
     });
</script>

<script type="text/javascript">
    $(function () {
        $("#txtsearch").autocomplete({
            source: function (request, response) {
                $.ajax({
                    url: "../AutoComplete_Pages/Auto_Complete_Searching.asmx/GST_Billing_Paramount_Search",
                    data: "{ 'mail': '" + request.term + "','Branch': '" + '' + "' ,'Aflags': '" + document.getElementById("<%=ddltype.ClientID %>").value + "','Chk': '" + document.getElementById("<%=chkqs.ClientID %>").checked + "' }",
                    dataType: "json",
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    dataFilter: function (data) { return data; },
                    async: true,
                    success: function (data) {
                        response(data.d);
                        if (data.d == '') {
                            jQuery('#txtsearch').validationEngine('showPrompt', 'Incorrect', 'error', 'topRight', true);
                        }
                        else {
                            jQuery('#txtsearch').validationEngine('hidePrompt', '', 'error', 'topRight', true);
                        }
                    },
                    error: function (XMLHttpRequest, textStatus, errorThrown) {
                        ////alert(textStatus);
                    }
                });
            },
            minLength: 1

        });
        return false;
        scroll: true;
    });
 </script>

<div id="innerbox_MidMain"  style="height:125px;" >


<div id="tag_srcinner1">

<div id="tag_srcinner1" >
                <div id="newbu">
                    <input type="submit" value="Submit" class="new" onclick="open_GST_Billing_paramount_New('New'); return false" />
                </div>

                <div id="verslic">
                </div>
                <div id="editbu">
                    <a href="#" class="edit"></a>
                </div>
                <div id="mainmastop2container_rght_tag2_txt1" style="width:250px;">
                   Billing
                 </div>
                     <div id="verslic"> </div>
                     <div id="editbu" style="padding-top:3px;display:none;">
                      <asp:ImageButton ID="btncopy" runat="server"  ToolTip="Inv. No Edit" OnClientClick="open_GST_Imp_Inv_no_Edit(); return false" ImageUrl="~/images/icons/INV_No_Edit.png"  Height="28px" Width="28px" />
                      
                   
                </div>
                  <%-- <div id="verslic"> </div>--%>
                     <div id="editbu" style="padding-top:3px; width:55px;visibility:visible;">
                    <%--<asp:ImageButton ID="ImageButton1" Height="26px" Width="30px" ToolTip="Invoice Report"  ImageUrl="~/images/icons/Bill_pending.jpg" runat="server" OnClientClick="Inv_Bill();return false;"  />--%>
                    <asp:LinkButton ID="LbGst" runat="server" Height="28px" Width="28px"  OnClientClick="GSTR1()">GSTR1</asp:LinkButton>
                </div>
                <%--  <div id="verslic"> </div>--%>
                     <div id="editbu" style="padding-top:3px;visibility:visible;">
                      <asp:ImageButton ID="btnexcel" runat="server"   ImageUrl="~/images/xls.jpg"  Height="28px" Width="28px"  OnClick="btncheck_Click" />
                </div>
                <div id="editbu" style="padding-top:3px;width:78px;">
                      <%--<asp:ImageButton ID="ImageButton2" runat="server"   ImageUrl="~/images/xls.jpg"  Height="28px" Width="28px"  OnClientClick="EXcel()"/>--%>
                    <asp:LinkButton ID="LinkButton1" runat="server" Height="28px" Width="78px"  OnClientClick="EXcel('S')">Statement</asp:LinkButton>
                </div>
                <div id="editbu" style="padding-top:3px;width:48px;">
                      <%--<asp:ImageButton ID="ImageButton2" runat="server"   ImageUrl="~/images/xls.jpg"  Height="28px" Width="28px"  OnClientClick="EXcel()"/>--%>
                    <asp:LinkButton ID="pdf" runat="server" Height="28px" Width="48px"  OnClientClick="return GST_ALL_Job_Rpt();">Bulk Inv</asp:LinkButton>
                </div>
               <div id="editbu" style="padding-top:3px;width:78px;">
                           <asp:LinkButton ID="btnremittance" runat="server" Height="28px" 
                        Width="152px"  OnClientClick="remittance()">Inward Remittance</asp:LinkButton> 
                       </div>
                       <%-- // -------added by rosi for HSN --------4-111-2024--------START------------//--%>
                        <div id="editbu" style="padding-top:3px;width:78px;">
                           <asp:LinkButton ID="btnHsn" runat="server" Height="28px" 
                        Width="152px"  OnClientClick="HSN()">HSN</asp:LinkButton> 
                       </div>
                        <%-- // -------added by rosi for HSN --------4-111-2024--------END------------//--%>


            </div>
             <%-- --------------------------------------------Start---------------------------%>
             <div id="tag_transact_src_inner" style="width: 1220px;">
                    <div id="tag_Exchange_inner_lft">

                    <div id="tag_Exchange_inner_lft">
                        <div id="innerbox_transac_bot_inn" style="width: 1220px;">
                        <div id="tag_label_transaction_popup_IGM2_empty" style="margin-left:-20px">
                        <asp:CheckBox ID="chkqs" onclick = "MutExChkList(this);" runat="server" />
                            </div>

                          <div id="txtcon-m_src" style="width: 100px;">
                               <asp:DropDownList ID="ddltype" runat="server" CssClass="txtbox_none" Font-Size="12px"  Width="95px"  >
                               <asp:ListItem Text=" " Value="0" ></asp:ListItem>
                               <asp:ListItem Text="Inv. no" Value="Inv_no" ></asp:ListItem>
                               <asp:ListItem Text="Job no" Value="JOBNO" ></asp:ListItem>
                                <asp:ListItem Text="Pro Inv no" Value="PRO_INV" ></asp:ListItem>
                                  <asp:ListItem Text="Hawb no" Value="HAWB_NO" ></asp:ListItem>
                                  <asp:ListItem Text="Mawb no" Value="MAWB_NO" ></asp:ListItem>
                                  
                               </asp:DropDownList>
                            </div>
                            <div id="txtcon-m_transaction_code" style="width: 170px;">
                                
                                   <asp:TextBox ID="txtsearch" runat="server"  ClientIDMode="Static"
                                    CssClass="txtbox_none_Mid_transac_Inv_No" ></asp:TextBox>
                                
                            </div>
                              <div id="tag_label_transaction_popup_IGM2_empty" style="width: 5px">
                            </div>
                               <%-- onselectedindexchanged="Rd_Inv_Type_SelectedIndexChanged"--%>

                                 <div id="tag_label_transact_Src" style="width: 70px;">
                              Category
                            </div>
                            <div id="txtcon-m_src" style="width: 80px;">
                                
                                <asp:DropDownList ID="ddl_Type" runat="server" CssClass="txtbox_none" 
                                    AutoPostBack="true"  Font-Size="12px" Width="75px" 
                                    onselectedindexchanged="ddl_Type_SelectedIndexChanged">
                                <asp:ListItem Text="" Value=""></asp:ListItem>
                                <asp:ListItem Text="Imp" Value="I"></asp:ListItem>
                                <asp:ListItem Text="Exp" Value="E"></asp:ListItem>
                                </asp:DropDownList>

                            </div>
                            
                            
                              <div id="tag_label_transaction_popup_IGM2_empty">
                            </div>
                              

                                 <div id="tag_label_transact_Src" style="width: 65px;">
                              Mode
                            </div>
                            <div id="txtcon-m_src" style="width: 100px;">
                                
                                <asp:DropDownList ID="ddl_mode" runat="server" CssClass="txtbox_none" 
                                    Font-Size="12px" Width="100px" >
                                   
                            
                                <asp:ListItem Text="" ></asp:ListItem>
                                <asp:ListItem Text="Air"></asp:ListItem>
                                   <asp:ListItem Text="Sea"></asp:ListItem>
                                      <asp:ListItem Text="Others"></asp:ListItem>
                                </asp:DropDownList>

                            </div>
                              <div id="tag_label_transaction_popup_IGM2_empty">
                            </div>
                             

                                 <div id="tag_label_transact_Src" style="width: 63px;">
                              Type
                            </div>
                            <div id="txtcon-m_src" style="width: 100px;">
                                
                                <asp:DropDownList ID="ddl_cattype" runat="server" CssClass="txtbox_none" 
                                     Font-Size="12px" Width="100px">
                               <asp:ListItem Text="" ></asp:ListItem>
                                <asp:ListItem Text="Forwarding"></asp:ListItem>
                                   <asp:ListItem Text="Clearing"></asp:ListItem>
                                      <asp:ListItem Text="Cross_country"></asp:ListItem>
                                      <asp:ListItem Text="Others"></asp:ListItem>
                                </asp:DropDownList>

                            </div>
                             <div id="tag_label_transaction_popup_IGM2_empty">
                            </div>
                            <div id="tag_label_transaction_Src_longlabel" style="width: 65px;">Inv Status</div>
                            
                        <div id="txtcon-m_transaction_code" style="width: 150px;">
                            <asp:DropDownList ID="ddlJobstatus" runat="server" CssClass="txtbox_none"  
                                Font-Size="12px" Width="135px" TabIndex="5" >
                                <asp:ListItem></asp:ListItem>
                                <asp:ListItem Value="Approved">Tax</asp:ListItem>
                                <asp:ListItem Value="Pending">Proforma</asp:ListItem>
                                <asp:ListItem Value="Cancel">Cancel</asp:ListItem>
                            </asp:DropDownList>
                        </div>
                         <div id="tag_label_transaction_popup_IGM2_empty">
                            </div>
                        
                                 <div id="tag_label_transact_Src" style="width: 63px;">
                              E-invoice
                            </div>

                         <div id="txtcon-m_transaction_code" style="width: 140px;">
                            <asp:DropDownList ID="ddleinvoice" runat="server" CssClass="txtbox_none"  
                                Font-Size="12px" Width="135px" TabIndex="6" >
                                <asp:ListItem></asp:ListItem>
                                <asp:ListItem>Generated</asp:ListItem>
                                <asp:ListItem>Not Generated</asp:ListItem>
                                
                            </asp:DropDownList>
                        </div>
                        </div>


                        <div id="tag_transact_lft_in1" style="width: 1030px;">

                       
                             <div id="tag_label_transact_Src" style="width: 120px; margin-left: -15px;">
                              Customer Name
                            </div>
                            <div id="txtcon-m_src" style="width: 320px;">
                                <asp:DropDownList ID="ddlCus_name" runat="server" CssClass="txtbox_none" AutoPostBack="true"
                                     Font-Size="12px" Width="315px"  >
                                </asp:DropDownList>
                            </div>
                             <div id="tag_label_transaction_popup_IGM2_empty">
                            </div>
                            <div id="tag_label_transact_Src" style="width: 65px;">
                                From Date</div>
                            <div id="txtcon-m_transaction_code">
                                <asp:TextBox ID="txtfromdate" runat="server" CssClass="txtbox_none_Mid_transac_code"
                                    MaxLength="10" TabIndex="3"></asp:TextBox>
                                <cc1:MaskedEditExtender ID="MaskedEditExtender5" runat="server" TargetControlID="txtfromdate"
                                    Mask="99/99/9999" MessageValidatorTip="true" OnInvalidCssClass="MaskedEditError"
                                    MaskType="Date" DisplayMoney="Left" AcceptNegative="Left" ErrorTooltipEnabled="True">
                                </cc1:MaskedEditExtender>
                                <cc1:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtfromdate"
                                    Format="dd/MM/yyyy">
                                </cc1:CalendarExtender>
                            </div>
                            <div id="tag_label_transaction_popup_IGM2_empty">
                            </div>
                            <div id="tag_label_transact_Src"  style="width: 62px;">
                                To Date
                            </div>
                            <div id="txtcon-m_transaction_code">
                                <asp:TextBox ID="txttodate" runat="server" CssClass="txtbox_none_Mid_transac_code"
                                    MaxLength="10" TabIndex="4"></asp:TextBox>
                                <cc1:MaskedEditExtender ID="MaskedEditExtender1" runat="server" TargetControlID="txttodate"
                                    Mask="99/99/9999" MessageValidatorTip="true" OnInvalidCssClass="MaskedEditError"
                                    MaskType="Date" DisplayMoney="Left" AcceptNegative="Left" ErrorTooltipEnabled="True">
                                </cc1:MaskedEditExtender>
                                <cc1:CalendarExtender ID="CalendarExtender2" runat="server" TargetControlID="txttodate"
                                    Format="dd/MM/yyyy">
                                </cc1:CalendarExtender>
                            </div>




                            <div  id="tag_label_transaction_popup_no_empty">
                            </div>
                              <div id="tag_label_transaction_Src_longlabel" style="width: 65px;"> Month</div>
                            
                        <div id="txtcon-m_transaction_code" style="width: 140px;">
                            <asp:DropDownList ID="ddlmonth" runat="server" CssClass="txtbox_none"  
                                Font-Size="12px" Width="135px" TabIndex="5" >
                            </asp:DropDownList>
                        </div>

                           
                      <%-- -------------------------------------------------------------------------------%>
                        </div>

                        <%------------------------------------------------------%>
                    </div>
                                               
                        
                    </div>
                    <div id="tag_transact_srcinner_rght" style="margin-top: 40px">
                        <div id="small-butt-left">
                            <a href="#" class="srcbu" id="btnSearch" runat="server" onclick="return validdate();" onserverclick="btnSearch_Click"
                                tabindex="5"></a>
                        </div>
                        <div id="small-butt-rght">
                            <a href="GST_Imp_Exp_Invoice_Job_Search.aspx" class="cancelButt" tabindex="6"></a>
                        </div>

                         <asp:Button ID="btnHidden" runat="server" OnClick="btncheck_Click" style="display:none" />
                    </div>
                </div>
                  <%-- --------------------------------------------End---------------------------%>
 </div>

        </div>


           <div id="innerbox_MidMain" style="height: 28px; margin-top:-13px; display:none;">
     <div id="tag_srcinner1grid" style="height: 28px;  ">
             <div id="mainmastop2container_rght_tag2_txt1" style="margin-top:08px; width:100px;">
                 <asp:Literal ID="ltl" Text="Inv Count:"   runat="server"></asp:Literal>
                 <span style="color:Red;" > <asp:Literal ID="lbljobcnt"   runat="server"></asp:Literal> </span>
             </div>
              <div id="verslic"></div>
               <div id="mainmastop2container_rght_tag2_txt1" style="margin-top:08px; width:150px;">
                 <asp:Literal ID="ltl1" Text="Pending Count:"   runat="server"></asp:Literal>
                 <span style="color:Red;" > <asp:Literal ID="lblpending_cnt"   runat="server"></asp:Literal> </span>
             </div>
              <div id="verslic"></div>

                <div id="mainmastop2container_rght_tag2_txt1" style="margin-top:08px; width:170px;">
                 <asp:Literal ID="ltl2" Text="Completed  Count:"   runat="server"></asp:Literal>
                 <span style="color:Red;" > <asp:Literal ID="lblCompleted_cnt"   runat="server"></asp:Literal> </span>
             </div>
              <div id="verslic"></div>
              <div id="mainmastop2container_rght_tag2_txt1" style="margin-top:08px; width:170px;">
                 <asp:Literal ID="ltl3" Text="Cancelled Count:"   runat="server"></asp:Literal>
                 <span style="color:Red;" > <asp:Literal ID="lblCancelled_cnt"   runat="server"></asp:Literal> </span>
             </div>
              <div id="verslic"></div>
        </div>
   </div>

    <div id="innerbox_MidMain_Grid" style="height: 580px;margin-top:-13px;width:1300px;"> 

            <div id="Main_Grid_Container" runat="server" style="overflow:scroll; height:580px;" >  
                <asp:GridView runat="server" ID="gvdetails" EmptyDataText="NO RECORD FOUND" AutoGenerateColumns="false"
                    CssClass="grid-view" ShowHeader="true" ShowHeaderWhenEmpty="true" Width="110%"
                     OnRowDataBound="gvdetails_RowDataBound" CellPadding="1" DataKeyNames="ID,IMP_EXP,BILL_INV_NO,CUS_NAME"
                    CellSpacing="1" OnRowCreated="gvdetails_RowCreated" 
                   >

                    <AlternatingRowStyle BackColor="White" BorderColor="#C8C8C8" BorderStyle="Solid"  CssClass="column_style_right" BorderWidth="1px" />
                    <HeaderStyle Font-Underline="false" ForeColor="Black" />
                    <Columns>

                    <asp:TemplateField>
            <ItemTemplate>
                <asp:CheckBox ID="SelectCheckBox" runat="server" 
               
                />
            </ItemTemplate>
        </asp:TemplateField>
                        <asp:TemplateField HeaderText="S.No" HeaderStyle-Width="45px">
                            <ItemTemplate>
                                <asp:Label ID="lblitemslno" runat="server" Text='<%# Container.DataItemIndex + 1 %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" Width="45px" />
                        </asp:TemplateField>

                          <asp:BoundField DataField="TAX_INVNO_PS" HeaderText="Tax Inv No"  HeaderStyle-Width="125px">
                            <ItemStyle CssClass="column_style_left5" HorizontalAlign="left" Font-Bold="true" Width="110px" Font-Size="11px" />
                        </asp:BoundField>

                       <asp:BoundField DataField="BILL_INV_NO" HeaderText="Pro Inv No"  HeaderStyle-Width="60px">
                            <ItemStyle CssClass="column_style_left5" HorizontalAlign="left" Font-Bold="true" Width="60px" Font-Size="11px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="JOBNO" HeaderText="Job No"  HeaderStyle-Width="120px">
                            <ItemStyle CssClass="column_style_left5" HorizontalAlign="left" Font-Bold="true"  Width="120px" Font-Size="11px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="JOB_STATUS" HeaderText="Status"  HeaderStyle-Width="70px">
                            <ItemStyle CssClass="column_style_left5" HorizontalAlign="left" Font-Bold="true"  Width="70px" Font-Size="11px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="TYPE" HeaderText="Type"  HeaderStyle-Width="120px">
                            <ItemStyle CssClass="column_style_left5" HorizontalAlign="left" Font-Bold="true"  Width="100px" Font-Size="11px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="IMP_EXP" HeaderText="Imp/Exp"  HeaderStyle-Width="70px">
                            <ItemStyle  CssClass="column_style_left5" HorizontalAlign="Left" Font-Bold="true"  Width="50px" Font-Size="11px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="LOCAL_OTHER" HeaderText="Inv. Type"  HeaderStyle-Width="60px">
                            <ItemStyle  CssClass="column_style_left5" HorizontalAlign="Left" Font-Bold="true"  Width="50px" Font-Size="11px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="BILL_INV_DATE" HeaderText="Invoice Date"  HeaderStyle-Width="80px">
                            <ItemStyle CssClass="column_style_left5" HorizontalAlign="Center" Font-Bold="true"  Width="80px" Font-Size="12px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="CUS_NAME" HeaderText="Customer Name"  HeaderStyle-Width="250px">
                            <ItemStyle  CssClass="column_style_left5" HorizontalAlign="left" Font-Bold="true"  Width="360px" Font-Size="11px"/>
                        </asp:BoundField>
                        <asp:BoundField DataField="GRAND_TOTAL" HeaderText="Grand Total"  HeaderStyle-Width="110px"  >
                            <ItemStyle CssClass="column_style_left5" HorizontalAlign="left" Font-Bold="true"  Width="110px"  Font-Size="11px" />
                        </asp:BoundField>

                        <asp:BoundField DataField="SALES" HeaderText="Sales Person"  HeaderStyle-Width="110px"  >
                            <ItemStyle CssClass="column_style_left5" HorizontalAlign="left" Font-Bold="true"  Width="110px"  Font-Size="11px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="AckNo" HeaderText="Ack No"  HeaderStyle-Width="110px"  >
                            <ItemStyle CssClass="column_style_left5" HorizontalAlign="left" Font-Bold="true"  Width="110px"  Font-Size="11px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="AckDt" HeaderText="Ack Date"  HeaderStyle-Width="110px"  >
                            <ItemStyle CssClass="column_style_left5" HorizontalAlign="left" Font-Bold="true"  Width="110px"  Font-Size="11px" />
                        </asp:BoundField>
                         <asp:BoundField DataField="PENDINGREASON" HeaderText="Pending Reason"  HeaderStyle-Width="110px"  >
                            <HeaderStyle CssClass="hideGridColumn" />
                            <ItemStyle CssClass="hideGridColumn" />
                        </asp:BoundField>
                    </Columns>
                </asp:GridView>

            </div>
         </div>

        <script src="../js/Import_Jscript/Imp_MIS.js" type="text/javascript"></script>
        <script src="../js/iframepopupwin.js" type="text/javascript"></script>


        <style type="text/css">
            .ui-autocomplete-loading
        {
            background: white url('../images/ui-anim_basic_16x16.gif') right center no-repeat;
        }
       
    </style>
     <asp:HiddenField ID="hdrowindex" runat="server" />
     <asp:HiddenField ID="hdBill_invno" runat="server" />
     <asp:HiddenField ID="hd_Imp_Exp" runat="server" />

      <asp:HiddenField ID="HDBranch" runat="server" />
         <asp:HiddenField ID="Hidden_TaxInvo_No" runat="server" />
      <script language="javascript">
    function Inv_Bill() {
        var jobno = document.getElementById("<%=hdrowindex.ClientID %>").value;
        var Bill_Invo = document.getElementById("<%=hdBill_invno.ClientID %>").value;
        var Imp_Exp = document.getElementById("<%=hd_Imp_Exp.ClientID %>").value;

        if (jobno != '') {
            NewWindow1('../Billing_Paramount/GST_Imp_Exp_rpt.aspx?Job_Id=' + jobno + '&Imp_Exp=' + Imp_Exp + '&Bill_Inv_No=' + Bill_Invo + '', 'List', '870', '1024', 'yes');
        }
        else {
            jAlert('Select a Grid Job', 'Bill Invoice', function (r) { document.getElementById("ddlCus_name").focus(); });
        }
        
    }
    </script>
     <script type="text/javascript">
         var win = null;

         var ddlSearch_Cat = $('#<%= ddltype.ClientID %>').val();
         var txtsearch = $('#<%= txtsearch.ClientID %>').val();

         var Imp_name = $('#<%= ddlCus_name.ClientID %>').val();
         var Fromdate = $('#<%= txtfromdate.ClientID %>').val();
         var Todate = $('#<%= txttodate.ClientID %>').val();
         var Month = $('#<%= ddlmonth.ClientID %>').val();

         var totalRows = $("#<%=gvdetails.ClientID %> tr").length;
         var lastProductId = $("#<%=gvdetails.ClientID %> tr:last").children("td:first").html();

         if (lastProductId != 'NO RECORD FOUND') {
             function EXcel(T) {
         
                 var ddlSearch_Cat = $('#<%= ddltype.ClientID %>').val();
                 var txtsearch = $('#<%= txtsearch.ClientID %>').val();

                 var Imp_name = $('#<%= ddlCus_name.ClientID %>').val();
                 var Fromdate = $('#<%= txtfromdate.ClientID %>').val();
                 var Todate = $('#<%= txttodate.ClientID %>').val();
                 var Month = $('#<%= ddlmonth.ClientID %>').val();
                 var Mode = $('#<%= ddl_mode.ClientID %>').val();
                 var Cattype = $('#<%= ddl_cattype.ClientID %>').val();
                 var IMP_EXP = $('#<%= ddl_Type.ClientID %>').val();
                 var Status = $('#<%= ddlJobstatus.ClientID %>').val();
                 var Einv = $('#<%= ddleinvoice.ClientID %>').val();
                
                 if (T == 'T') {
                     if (document.getElementById("<%=Hidden_TaxInvo_No.ClientID %>").value != "") {
                         var txtsearch = document.getElementById("<%=Hidden_TaxInvo_No.ClientID %>").value;
                        
                         var ddlSearch_Cat = "Inv_No_M"
                     }
                 }
                
                 var totalRows = $("#<%=gvdetails.ClientID %> tr").length;
                
                 var Type = T;

                 win = window.open('../FlatFile/MIS_report_1.aspx?GST_EXP_Tally_Data=Yes&&ddlSearch_Cat=' + ddlSearch_Cat + '&&txtsearch=' + txtsearch + '&&Exp_name=' + encodeURIComponent(Imp_name) + '&&Fromdate=' + Fromdate + '&&Todate=' + Todate + '&&Month=' + Month + '&Type=' + Type + '&&Cattype=' + Cattype + '&&Mode=' + Mode + '&&Status=' + Status + '&&Einv=' + Einv + '&&IMP_EXP=' + IMP_EXP + '', 'ndf');

                
                 Loading_HideImage();
             }
            
             function Tally() {
                 var Type = 'T';
                 win = window.open('../FlatFile/MIS_report_1.aspx?GST_IMP_Tally_Data_COMBINED=Yes&ddlSearch_Cat=' + ddlSearch_Cat + '&txtsearch=' + txtsearch + '&Imp_name=' + Imp_name + '&Fromdate=' + Fromdate + '&Todate=' + Todate + '&Month=' + Month + '&Type=' + Type + '', 'ndf');
                 Loading_HideImage();
             }
         }
         function remittance() {
             var ddlSearch_Cat = $('#<%= ddltype.ClientID %>').val();
             var txtsearch = $('#<%= txtsearch.ClientID %>').val();

             var Imp_name = $('#<%= ddlCus_name.ClientID %>').val();
             var Fromdate = $('#<%= txtfromdate.ClientID %>').val();
             var Todate = $('#<%= txttodate.ClientID %>').val();
             var Month = $('#<%= ddlmonth.ClientID %>').val();
             var Mode = $('#<%= ddl_mode.ClientID %>').val();
             var Cattype = $('#<%= ddl_cattype.ClientID %>').val();
             var Status = $('#<%= ddlJobstatus.ClientID %>').val();
             var Einv = $('#<%= ddleinvoice.ClientID %>').val();

             var totalRows = $("#<%=gvdetails.ClientID %> tr").length;



             win = window.open('../FlatFile/MIS_report_1.aspx?Remittance_Invoice=Yes&&ddlSearch_Cat=' + ddlSearch_Cat + '&&txtsearch=' + txtsearch + '&&Exp_name=' + encodeURIComponent(Imp_name) + '&&Fromdate=' + Fromdate + '&&Todate=' + Todate + '&&Month=' + Month + '&Type=' + Type + '&&Cattype=' + Cattype + '&&Mode=' + Mode + '&&Status=' + Status + '&&Einv=' + Einv + '', 'ndf');


             Loading_HideImage();
         }
         function GSTR1() {
             var ddlSearch_Cat = $('#<%= ddltype.ClientID %>').val();
             var txtsearch = $('#<%= txtsearch.ClientID %>').val();

             var Imp_name = $('#<%= ddlCus_name.ClientID %>').val();
             var Fromdate = $('#<%= txtfromdate.ClientID %>').val();
             var Todate = $('#<%= txttodate.ClientID %>').val();
             var Month = $('#<%= ddlmonth.ClientID %>').val();
             var Mode = $('#<%= ddl_mode.ClientID %>').val();
             var Cattype = $('#<%= ddl_cattype.ClientID %>').val();
             var Status = $('#<%= ddlJobstatus.ClientID %>').val();
             var Einv = $('#<%= ddleinvoice.ClientID %>').val();

             var totalRows = $("#<%=gvdetails.ClientID %> tr").length;

             var Type = '';

             win = window.open('../FlatFile/MIS_report_1.aspx?GSTR1=Yes&&ddlSearch_Cat=' + ddlSearch_Cat + '&&txtsearch=' + txtsearch + '&&Exp_name=' + encodeURIComponent(Imp_name) + '&&Fromdate=' + Fromdate + '&&Todate=' + Todate + '&&Month=' + Month + '&Type=' + Type + '&&Cattype=' + Cattype + '&&Mode=' + Mode + '&&Status=' + Status + '&&Einv=' + Einv + '', 'ndf');


             Loading_HideImage();
         }
         function HSN() {
             var ddlSearch_Cat = $('#<%= ddltype.ClientID %>').val();
             var txtsearch = $('#<%= txtsearch.ClientID %>').val();

             var Imp_name = $('#<%= ddlCus_name.ClientID %>').val();
             var Fromdate = $('#<%= txtfromdate.ClientID %>').val();
             var Todate = $('#<%= txttodate.ClientID %>').val();
             var Month = $('#<%= ddlmonth.ClientID %>').val();
             var Mode = $('#<%= ddl_mode.ClientID %>').val();
             var Cattype = $('#<%= ddl_cattype.ClientID %>').val();
             var Status = $('#<%= ddlJobstatus.ClientID %>').val();
             var Einv = $('#<%= ddleinvoice.ClientID %>').val();

             var totalRows = $("#<%=gvdetails.ClientID %> tr").length;

             var Type = '';

             win = window.open('../FlatFile/MIS_report_1.aspx?HSN=Yes&&ddlSearch_Cat=' + ddlSearch_Cat + '&&txtsearch=' + txtsearch + '&&Exp_name=' + encodeURIComponent(Imp_name) + '&&Fromdate=' + Fromdate + '&&Todate=' + Todate + '&&Month=' + Month + '&Type=' + Type + '&&Cattype=' + Cattype + '&&Mode=' + Mode + '&&Status=' + Status + '&&Einv=' + Einv + '', 'ndf');


             Loading_HideImage();
         }
  </script>
      

 

</asp:Content>

