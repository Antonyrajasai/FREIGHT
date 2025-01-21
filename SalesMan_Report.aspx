<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SalesMan_Report.aspx.cs" Inherits="Accounts_SalesMan_Report"  MasterPageFile="~/MasterPage/eRoyalFreightMasters.master" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <style type="text/css">
        .ad
        {
            width: 50px;
        }
    </style>

     <style type="text/css">

    .txtbox_none {
    text-transform: none;
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

    function ChangeRowColor(rowID, Value, Job_no) {
        document.getElementById("<%=hdrowindex.ClientID %>").value = Value;

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


<div id="innerbox_MidMain"  style="height:150px; width:1350px;" >


    <div id="tag_srcinner1">

<div id="tag_srcinner1" >
                

                <div id="verslic">
                </div>
                <div id="editbu">
                    <a href="#" class="edit"></a>
                </div>
                <div id="mainmastop2container_rght_tag2_txt1" style="width:250px;">
                    SalesMan Client Details</div>
                     <div id="verslic"> </div>
                     <div id="editbu" style="padding-top:3px;display:none;">
                      <asp:ImageButton ID="btncopy" runat="server"  ToolTip="Inv. No Edit" OnClientClick="open_GST_Imp_Inv_no_Edit(); return false" ImageUrl="~/images/icons/INV_No_Edit.png"  Height="28px" Width="28px" />
                      
                   
                </div>
                  <%-- <div id="verslic"> </div>--%>
                     <div id="editbu" style="padding-top:3px; width:35px;visibility:hidden;">
                    <asp:ImageButton ID="ImageButton1" Height="26px" Width="30px" ToolTip="Invoice Report"  ImageUrl="~/images/icons/Bill_pending.jpg" runat="server" OnClientClick="Inv_Bill();return false;"  />
                </div>
                <%--  <div id="verslic"> </div>--%>
                     <div id="editbu" style="padding-top:3px;visibility:visible;">
                      <asp:ImageButton ID="btnexcel" runat="server"   ImageUrl="~/images/xls.jpg"  Height="28px" Width="28px"  OnClientClick="EXcel('T')"/>
                </div>
                <div id="editbu" style="width:150px; margin-top: 10px;">
                      <%--<asp:ImageButton ID="ImageButton2" runat="server"   ImageUrl="~/images/xls.jpg"  Height="28px" Width="28px"  OnClientClick="EXcel()"/>--%>
                    <%--<asp:LinkButton ID="LinkButton1" runat="server" Height="40px" Width="110px"  OnClientClick="EXcel_sales()">Profit and sales Sea</asp:LinkButton>--%>
                </div>
                <div id="editbu" style="width:110px; margin-top: 10px;">
                        </div>
                <div id="editbu" style="padding-top:3px;">
                        </div>
               

            </div>
             <%-- --------------------------------------------Start---------------------------%>
             <div id="tag_transact_src_inner" style="width: 1030px;" height="125px">
                    <div id="tag_Exchange_inner_lft">

                    <div id="tag_Exchange_inner_lft">
                        <div id="innerbox_transac_bot_inn" style="width: 1035px;">
                         
                          
                           
                        <div id="tag_transact_lft_in1" style="width: 1030px;">
                        <%--<div id="tag_label_transact_Src" style="width: 65px;">
                                Job No</div>
                        <div id="txtcon-m_transaction_code"style="width: 100px;"  >
                                
                                   <asp:TextBox ID="txtsearch" runat="server"  ClientIDMode="Static"
                                    CssClass="txtbox_none_Mid_transac_Inv_No" Width="100px" 
                                       ontextchanged="txtsearch_TextChanged"></asp:TextBox>
                                  </div>--%>
<div id="tag_label_transact_Src" style="width: 85px;">
                          
                            <asp:Label ID="lblList" runat="server" Text="Sales Person"></asp:Label>
                            </div>
                              <div id="tag_label_transact_Src" style="width: 100px; display:none;">
                                  
                                       <asp:DropDownList ID="ddlSearch" runat="server" TabIndex="4" style="width: 100px;">
                                     
                                        <asp:ListItem>Sales</asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                     <div id="tag_label_transaction_popup_IGM2_empty"style="width: 25px;">
                            </div>
                                     <div id="txtcon-m_transaction_code"style="width: 175px;"  >
                                    
                                    <div id="txtcon-m_Exchange" style="width:120px;height:400px; margin-left:15px;" >
                                    <asp:DropDownList ID="ddlSales" runat="server" ClientIDMode="Static"
                                           class="chosen-select-deselect" data-placeholder="Select Sales" 
                                        TabIndex="3" Width="120px"  ></asp:DropDownList>
                                        
                                    </div>
                                    </div>

                            <div id="tag_label_transaction_popup_IGM2_empty"style="width: 25px;">
                            </div>
                           
                            <div id="tag_label_transact_Src" style="width:75px;">
                                From Date</div>
                            <div id="txtcon-m_transaction_code" >
                                <asp:TextBox ID="txtfromdate" runat="server" CssClass="txtbox_none_Mid_transac_code"
                                    MaxLength="10" TabIndex="3" Height="21px"></asp:TextBox>
                                <cc1:maskededitextender ID="MaskedEditExtender5" runat="server" TargetControlID="txtfromdate"
                                    Mask="99/99/9999" MessageValidatorTip="true" OnInvalidCssClass="MaskedEditError"
                                    MaskType="Date" DisplayMoney="Left" AcceptNegative="Left" 
                                    ErrorTooltipEnabled="True">
                                </cc1:maskededitextender>
                                <cc1:calendarextender ID="CalendarExtender1" runat="server" TargetControlID="txtfromdate"
                                    Format="dd/MM/yyyy">
                                </cc1:calendarextender>
                            </div>
                            <div id="tag_label_transaction_popup_IGM2_empty" style="width: 30px;">
                            </div>
                            <div id="tag_label_transact_Src"  style="width: 60px;">
                                To Date
                            </div>
                            <div id="txtcon-m_transaction_code" style="width: 100px;">
                                <asp:TextBox ID="txttodate" runat="server" CssClass="txtbox_none_Mid_transac_code"
                                    MaxLength="10" TabIndex="4" Height="21px" Width="100px"></asp:TextBox>
                                <cc1:maskededitextender ID="MaskedEditExtender1" runat="server" TargetControlID="txttodate"
                                    Mask="99/99/9999" MessageValidatorTip="true" OnInvalidCssClass="MaskedEditError"
                                    MaskType="Date" DisplayMoney="Left" AcceptNegative="Left" 
                                    ErrorTooltipEnabled="True">
                                </cc1:maskededitextender>
                                <cc1:calendarextender ID="CalendarExtender2" runat="server" TargetControlID="txttodate"
                                    Format="dd/MM/yyyy">
                                </cc1:calendarextender>
                            </div>

                            <div id="tag_label_transaction_popup_IGM2_empty" style="width: 25px;">
                            </div>

                            
                               </div>
                               <div id="tag_transact_lft_in1" style="width: 1030px; height:5px" >
                               </div>
                             
                        </div>
                         </div>
                       
                    </div>
                                               
                           
                    </div>
                    <div id="tag_transact_srcinner_rght" style="margin-top: 20px; margin-right:50px" >
                        <div id="small-butt-left">
                            <a href="#" class="srcbu" id="btnSearch" runat="server" onclick="return validdate();" onserverclick="btnSearch_Click"
                                tabindex="5"></a>
                        </div>
                        <div id="small-butt-rght">
                            <a href="Profit_Loss.aspx" class="cancelButt" tabindex="6"></a>
                        </div>
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
               <div id="mainmastop2container_rght_tag2_txt1" style="margin-top:08px; width:100px;">
                 <asp:Literal ID="ltl1" Text="Pending Count:"   runat="server"></asp:Literal>
                 <span style="color:Red;" > <asp:Literal ID="lblpending_cnt"   runat="server"></asp:Literal> </span>
             </div>
              <div id="verslic"></div>

                <div id="mainmastop2container_rght_tag2_txt1" style="margin-top:08px; width:100px;">
                 <asp:Literal ID="ltl2" Text="Completed  Count:"   runat="server"></asp:Literal>
                 <span style="color:Red;" > <asp:Literal ID="lblCompleted_cnt"   runat="server"></asp:Literal> </span>
             </div>
              <div id="verslic"></div>
              <div id="mainmastop2container_rght_tag2_txt1" style="margin-top:08px; width:100px;">
                 <asp:Literal ID="ltl3" Text="Cancelled Count:"   runat="server"></asp:Literal>
                 <span style="color:Red;" > <asp:Literal ID="lblCancelled_cnt"   runat="server"></asp:Literal> </span>
             </div>
              <div id="verslic"></div>
        </div>
   </div>

    <div id="innerbox_MidMain_Grid" style="width:1350px;"> 

            <div id="Main_Grid_Container" runat="server" >  
                <asp:GridView runat="server" ID="gvdetails" EmptyDataText="NO RECORD FOUND" AutoGenerateColumns="false"
                    CssClass="grid-view" ShowHeader="true" ShowHeaderWhenEmpty="true" Width="100%"
                     OnRowDataBound="gvdetails_RowDataBound" CellPadding="1"  DataKeyNames="JOBNO" 
                    CellSpacing="1" OnRowCreated="gvdetails_RowCreated">

                    <AlternatingRowStyle BackColor="White" BorderColor="#C8C8C8" BorderStyle="Solid"  CssClass="column_style_right" BorderWidth="1px" />
                    <HeaderStyle Font-Underline="false" ForeColor="Black" />
                    <Columns>
                        <asp:TemplateField HeaderText="S.No" HeaderStyle-Width="45px">
                            <ItemTemplate>
                                <asp:Label ID="lblitemslno" runat="server" Text='<%# Container.DataItemIndex + 1 %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle  Width="45px" CssClass="column_style_left5" />
                        </asp:TemplateField>
                    <asp:BoundField DataField="JOBNO">
                            <HeaderStyle CssClass="hideGridColumn" />
                            <ItemStyle CssClass="hideGridColumn" />
                            </asp:BoundField>
                      
                        <asp:BoundField DataField="SALES_BY" HeaderText="Sales"  ControlStyle-Width="100px" HeaderStyle-Width="100px"> 
                            <ItemStyle CssClass="column_style_left5" Width="100px" Font-Size="12px"  />
                        </asp:BoundField>
                        <asp:BoundField DataField="SALESMAN_STATUS" HeaderText="Status"  ControlStyle-Width="90px" HeaderStyle-Width="90px">
                            <ItemStyle CssClass="column_style_left5" Width="90px" Font-Size="12px" />
                        </asp:BoundField>
                       <asp:BoundField DataField="IA_CLIENT" HeaderText="Imp Air" ControlStyle-Width="90px" HeaderStyle-Width="90px" >
                            <ItemStyle CssClass="column_style_left5" Width="90px" Font-Size="12px"/>
                        </asp:BoundField>
                        <asp:BoundField DataField="IS_CLIENT" HeaderText="Imp Sea" ControlStyle-Width="90px" HeaderStyle-Width="90px" >
                            <ItemStyle CssClass="column_style_left5" Width="90px" Font-Size="12px"/>
                        </asp:BoundField>
                         <asp:BoundField DataField="ES_CLIENT" HeaderText="Exp Sea" ControlStyle-Width="90px" HeaderStyle-Width="90px" >
                            <ItemStyle CssClass="column_style_left5" Width="90px" Font-Size="12px"/>
                        </asp:BoundField>
                        <asp:BoundField DataField="EA_CLIENT" HeaderText="Exp Air" ControlStyle-Width="90px" HeaderStyle-Width="90px" >
                            <ItemStyle CssClass="column_style_left5" Width="90px" Font-Size="12px"/>
                        </asp:BoundField>
                       
                       <asp:BoundField DataField="VOLUME" HeaderText="Volumn"  ControlStyle-Width="150px" HeaderStyle-Width="150px">
                            <ItemStyle  CssClass="column_style_left5"  Width="150px" Font-Size="12px" />
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


 

  <script type="text/javascript">
      $(document).ready(function () {

          var totalRows = $("#<%=gvdetails.ClientID %> tr").length;
          var lastProductId = $("#<%=gvdetails.ClientID %> tr:last").children("td:first").html();

          if (totalRows > 2) {
              gridviewScroll();
          }
          else {

              if (lastProductId > 0) {
                  gridviewScroll();
              }
              else {
                  document.getElementById('<%=Main_Grid_Container.ClientID %>').style.overflow = "Auto";
                  document.getElementById('<%=gvdetails.ClientID %>').style.overflow = "100%";
                  document.getElementById('<%=gvdetails.ClientID %>').style.width = "100%";
                  /*alert (lastProductId);*/
              }
          }

      });

      function gridviewScroll() {
          gridView1 = $('#<%=gvdetails.ClientID %>').gridviewScroll({

              width: "99.9%",
              height: 520,
              railcolor: "#F0F0F0",
              barcolor: "#CDCDCD",
              barhovercolor: "#606060",
              bgcolor: "#F0F0F0",
              freezesize: 2,
              arrowsize: 30,
              varrowtopimg: "../GridScroll_Images/arrowvt.png",
              varrowbottomimg: "../GridScroll_Images/arrowvb.png",
              harrowleftimg: "../GridScroll_Images/arrowhl.png",
              harrowrightimg: "../GridScroll_Images/arrowhr.png",
              headerrowcount: 1,
              railsize: 16,
              barsize: 15,
              IsInUpdatePanel: true
          });
      }


	</script>
    <script type="text/javascript">

        $(function () {
            $("#txtAgent").autocomplete({
                source: function (request, response) {
                    $.ajax({
                        url: "../AutoComplete_Pages/Auto_Complete_Searching.asmx/Agent_List_Value",
                        data: JSON.stringify({
                            mail: request.term,
                            Agent_List: $('#<%= ddlSearch.ClientID %>').val()
                        }),

                        dataType: "json",
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        async: true,
                        success: function (data) {
                            if (data.d && data.d.length > 0) {
                                response(data.d);
                                jQuery('#txtConsignee').validationEngine('hidePrompt', '', 'error', 'topRight', true);
                            } else {
                                jQuery('#txtConsignee').validationEngine('showPrompt', 'Incorrect Consignee Name', 'error', 'topRight', true);
                            }
                        },
                        error: function (XMLHttpRequest, textStatus, errorThrown) {
                            console.error("Error: " + textStatus + " " + errorThrown);
                        }
                    });
                },
                minLength: 1
            });
        });



        function EXcel() {


            var win = null;
            var Sales = $('#<%= ddlSales.ClientID %>').val();
            var Fromdate = $('#<%= txtfromdate.ClientID %>').val();
            var Todate = $('#<%= txttodate.ClientID %>').val();


            win = window.open('../FlatFile/MIS_report_1.aspx?SalesReportsXL=Yes&&Sales=' + Sales + '&&Fromdate=' + Fromdate + '&Todate=' + Todate + '', 'ndf');


            Loading_HideImage();
        }


    </script>
    
</asp:Content>

