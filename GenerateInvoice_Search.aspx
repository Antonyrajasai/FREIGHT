<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MasterPage/eRoyalFreightMasters.master" CodeFile="GenerateInvoice_Search.aspx.cs" Inherits="GenerateInvoice_Search" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
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
    }
</style>

      <script language ="javascript" type="text/javascript">

          document.body.style.cursor = 'pointer';
          var oldColor = '';
          var jobno;

          function ChangeRowColor(rowID, Value, Val2) {

              document.getElementById("<%=hdrowindex.ClientID %>").value = Value;
              document.getElementById("<%=BE_Type.ClientID %>").value = Val2;
          }

          function myfunc() {

              return document.getElementById("<%=hdnFreightMode.ClientID %>").value;

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
                    url: "../AutoComplete_Pages/Auto_Complete_Searching.asmx/IMP_EXP_GENERAL_SEARCH",
                    data: "{ 'mail': '" + request.term + "','Branch': '" + document.getElementById("<%=HDBranch.ClientID %>").value + "' ,'Aflags': '" + document.getElementById("<%=ddlTerms.ClientID %>").value + "','IMP_EXP': 'Import'}",
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
        
        <div id="innerbox_MidMain">
            <div id="tag_srcinner1"  >
                <div id="newbu" style="display: none;">
                    <input type="submit" value="Submit" class="new" onclick="Import_Air_New('New', myfunc()); return false" /></div>
                <div id="verslic">
                </div>
                <div id="editbu">
                    <a href="#" class="edit"></a>
                </div>
                <div id="mainmastop2container_rght_tag2_txt1"  style="width:200px;">
                <span id="spancontrol" runat="server"></span> </div>
                   <div id="verslic"> </div>
                    <div style="display:none;">
                     <div id="editbu" style="padding-top:3px;">
                    <asp:ImageButton ID="ImageButton1" Height="26px" Width="30px" ToolTip="Check List"  ImageUrl="~/images/icons/check list.png" runat="server" OnClientClick="checklist();return false;"  />
                </div>
                  <div id="editbu" class="padding"  >
                    <asp:ImageButton ID="ImageButton2" Height="26px" Width="30px" ToolTip="Submission" ImageUrl="~/images/icons/Submission.png" runat="server" OnClientClick="Submission(); return false;" />
                </div>
                  <div id="editbu" class="padding">
                    <asp:ImageButton ID="ImageButton3" Height="26px" Width="30px" ToolTip="Tracking"  ImageUrl="~/images/icons/tracking.png" runat="server" OnClientClick="Tracking(); return false;" />
                </div>

                  <div id="editbu" class="padding">
                    <asp:ImageButton ID="ImageButton4" Height="26px" Width="30px" ToolTip="Annexure" ImageUrl="~/images/icons/annexure.png" runat="server" OnClientClick="Annexure();return false;"/>
                </div>
                <div id="editbu" class="padding">
                    <asp:ImageButton ID="ImageButton5" Height="26px" Width="30px" ToolTip="Gatt"  ImageUrl="~/images/icons/gatt.png" runat="server"  OnClientClick="Gatt();return false;"/>
                </div>
                <div id="editbu" class="padding">
                    <asp:Image ID="Image1" runat="server"  Height="26px" Width="30px" ToolTip="Job History" ImageUrl="~/images/icons/job history.png"/>
                </div>
                <div id="editbu" class="padding">
                    <asp:ImageButton ID="btnwh" Height="26px" Width="30px" ToolTip="Exbond Report" ImageUrl="~/images/icons/WH.png" runat="server"
                     OnClientClick="Go_Warehouse(); return false;" /> 
                </div>
                 <div id="editbu" class="padding">
                    <asp:ImageButton ID="btn_BOE" Height="26px" Width="30px" ToolTip="BE Entry" ImageUrl="~/images/icons/bill of entry.png" runat="server"
                     OnClientClick="BE_Entry(); return false;" /> 
                </div>
               
                <div id="editbu" class="padding">
                   <asp:ImageButton ID="btn_error_chk" Height="26px" Width="30px" ToolTip="Error Find" ImageUrl="~/images/icons/Error_icon.png" runat="server"
                     OnClientClick="Error_Find(); return false;" /> 
                  </div>
                    <div id="editbu" class="padding">
                    <asp:ImageButton ID="btn_Tr_Mode_Change" ToolTip="Job Duplicate" 
                            runat="server" ImageUrl="~/images/icons/Duplicate.jpg"  Height="26px" Width="30px"
                            OnClientClick="jQuery('#form1').validationEngine('hideAll');open_Imp_Branch_Change('I'); return false" /> 
                  </div>
                   <div id="editbu" class="padding">
                      <asp:ImageButton ID="btn_Job_Status" Height="26px" Width="30px" ToolTip="Job Status" ImageUrl="~/images/icons/Job_Status.png" runat="server"
                     OnClientClick="Job_Status(); return false;" /> 
                  </div>
                  </div>
            </div>



            <div id="tag_transact_src_inner" style="width: 1300px;">
                <div id="tag_Exchange_inner_lft">

                    <div id="tag_transact_lft_in1" style="width:1256px; height:30px">

                        <div id="txt_container_Transact_Main_l" >
                            <div id="tag_label_transact_Src" style="width:110px;">
                                 <asp:DropDownList ID="ddlTerms" CssClass="listtxt_transac_item_gen_notn" Visible="false"  Width="105px" runat="server" TabIndex="1">
                                </asp:DropDownList>
                                Job No
                            </div>
                            
                            <div id="txtcon-m_Exchange">
                              <asp:TextBox ID="txtsearch" runat="server" CssClass="txtbox_none" autocomplete="OFF" Width="157px" ClientIDMode="Static"
                                    MaxLength="40" TabIndex="2" onkeypress="return numcharspl1(event)" ></asp:TextBox>
                            </div>
                        </div>

                        <div id="txt_container_Transact_Main_l">
                            <div id="tag_label_transact_Src">
                                Importer Name
                            </div>
                           
                            <div id="txtcon-m_Exchange">
                                
                           
                           <asp:TextBox ID="txtImporterName" runat="server" CssClass="txtbox_none_Mid" 
                                    MaxLength="50" TabIndex="3" onkeypress="return numcharspl1(event)"></asp:TextBox>
                            </div>
                            
                        </div>

                          <div id="txt_container_Transact_Main_l">
                            <div id="tag_label_transact_Src">
                               Suplier Name
                            </div>
                            
                            <div id="txtcon-m_Exchange">
                            <asp:TextBox ID="txtCustomsHouse" runat="server" CssClass="txtbox_none_Mid" 
                                    MaxLength="50" TabIndex="4" onkeypress="return numcharcomma(event)"></asp:TextBox>
                            </div>
                           
                        </div>

                           <div id="txt_container_Transact_Main_l" style=" visibility:hidden;" >
                            <div id="tag_label_transact_Src" style="width:115px;">
                            </div>
                           
                            <div id="txtcon-m_Exchange" >
                            <asp:TextBox ID="txtJobNo" runat="server" CssClass="txtbox_none_Mid" 
                                    MaxLength="10" TabIndex="5" onkeypress="return pin(event)"></asp:TextBox>
                            </div>
                            
                        </div>


                    </div>
                    <div id="tag_transact_lft_in1" style="width:1256px; height:30px">
                        <div id="txt_container_Transact_Main_l">
                            <div id="tag_label_transact_Src" style="width:110px;">
                                Types</div>
                           
                            <div id="txtcon-m_Exchange">
                                
                       
                               <asp:DropDownList ID="ddlTransportMode" runat="server" CssClass="listtxt_transac_mode_new"  Width="160px" TabIndex="6">
                               </asp:DropDownList>

                            </div>
                            
                       </div>
                        <div id="txt_container_Transact_Main_l">
                        <div id="tag_label_transact_Src">
                                From Date</div>
                            <div id="txtcon-m_Exchange">
                                <asp:TextBox ID="txtfromdate" runat="server" CssClass="txtbox_none_Mid" 
                                    MaxLength="10" TabIndex="7"></asp:TextBox>
                                <cc1:MaskedEditExtender ID="MaskedEditExtender5" runat="server" TargetControlID="txtfromdate"
                                    Mask="99/99/9999" MessageValidatorTip="true" OnInvalidCssClass="MaskedEditError"
                                    MaskType="Date" DisplayMoney="Left" AcceptNegative="Left" ErrorTooltipEnabled="True">
                                </cc1:MaskedEditExtender>
                                <cc1:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtfromdate"
                                    Format="dd/MM/yyyy">
                                </cc1:CalendarExtender>
                            </div>
                           </div>

                         <div id="txt_container_Transact_Main_l">
                       <div id="tag_label_transact_Src">
                                To Date
                            </div>
                            <div id="txtcon-m_Exchange">
                                <asp:TextBox ID="txttodate" runat="server" CssClass="txtbox_none_Mid"
                                    MaxLength="10" TabIndex="8"></asp:TextBox>
                                <cc1:MaskedEditExtender ID="MaskedEditExtender1" runat="server" TargetControlID="txttodate"
                                    Mask="99/99/9999" MessageValidatorTip="true" OnInvalidCssClass="MaskedEditError"
                                    MaskType="Date" DisplayMoney="Left" AcceptNegative="Left" ErrorTooltipEnabled="True">
                                </cc1:MaskedEditExtender>
                                <cc1:CalendarExtender ID="CalendarExtender2" runat="server" TargetControlID="txttodate"
                                    Format="dd/MM/yyyy">
                                </cc1:CalendarExtender>
                           </div>
                           </div>
                          </div>
                          
                </div>
                <div id="tag_transact_srcinner_rght" style="margin-top: 20px; margin-right:150px" >
                    <div id="small-butt-left">
                        <a href="#" class="srcbu" id="btnSearch" runat="server" onserverclick="btnSearch_Click" onclick="page_Loading();document.getElementById('hdn_A_to_Z_Search').value='';">
                        </a>
                    </div>
                    <div id="small-butt-rght">
                        <a href="GenerateInvoice_Search.aspx" class="cancelButt"></a>
                    </div>
                </div>
                </div>
            </div>
        </div>
        </div>
        <div id="innerbox_MidMain_Grid">
            <div id="tag_srcinner1grid" style="width:1050px;">
                <div id="allbu">
                    <input id="Submit1" type="submit" value="Submit" class="allgrid" runat="server" onserverclick="btnSearchAll_Click" 
                    onclick="document.getElementById('hdn_A_to_Z_Search').value='';page_Loading();"/></div>
                <div id="verslic">
                </div>
                <div id="mainmastop2container_rght_tag3">
                    <div id="small-alph-icon-tag">
                        <a href="#" class="small-alph-iconButt">#</a></div>
                    <div id="small-alph-icon-tag">
                        <a href="#" class="small-alph-iconButt" onclick="Get_A_to_Z_Value('A')" >A</a></div>
                    <div id="small-alph-icon-tag">
                        <a href="#" class="small-alph-iconButt" onclick="Get_A_to_Z_Value('B')" >B</a></div>
                    <div id="small-alph-icon-tag">
                        <a href="#" class="small-alph-iconButt" onclick="Get_A_to_Z_Value('C')" >C</a></div>
                    <div id="small-alph-icon-tag">
                        <a href="#" class="small-alph-iconButt" onclick="Get_A_to_Z_Value('D')" >D</a></div>
                    <div id="small-alph-icon-tag">
                        <a href="#" class="small-alph-iconButt" onclick="Get_A_to_Z_Value('E')" >E</a></div>
                    <div id="small-alph-icon-tag">
                        <a href="#" class="small-alph-iconButt" onclick="Get_A_to_Z_Value('F')" >F</a></div>
                    <div id="small-alph-icon-tag">
                        <a href="#" class="small-alph-iconButt" onclick="Get_A_to_Z_Value('G')" >G</a></div>
                    <div id="small-alph-icon-tag">
                        <a href="#" class="small-alph-iconButt" onclick="Get_A_to_Z_Value('H')" >H</a></div>
                    <div id="small-alph-icon-tag">
                        <a href="#" class="small-alph-iconButt" onclick="Get_A_to_Z_Value('I')" >I</a></div>
                    <div id="small-alph-icon-tag">
                        <a href="#" class="small-alph-iconButt" onclick="Get_A_to_Z_Value('J')" >J</a></div>
                    <div id="small-alph-icon-tag">
                        <a href="#" class="small-alph-iconButt" onclick="Get_A_to_Z_Value('K')" >K</a></div>
                    <div id="small-alph-icon-tag">
                        <a href="#" class="small-alph-iconButt" onclick="Get_A_to_Z_Value('L')" >L</a></div>
                    <div id="small-alph-icon-tag">
                        <a href="#" class="small-alph-iconButt" onclick="Get_A_to_Z_Value('M')" >M</a></div>
                    <div id="small-alph-icon-tag">
                        <a href="#" class="small-alph-iconButt" onclick="Get_A_to_Z_Value('N')" >N</a></div>
                    <div id="small-alph-icon-tag">
                        <a href="#" class="small-alph-iconButt" onclick="Get_A_to_Z_Value('O')" >O</a></div>
                    <div id="small-alph-icon-tag">
                        <a href="#" class="small-alph-iconButt" onclick="Get_A_to_Z_Value('P')" >P</a></div>
                    <div id="small-alph-icon-tag">
                        <a href="#" class="small-alph-iconButt" onclick="Get_A_to_Z_Value('Q')" >Q</a></div>
                    <div id="small-alph-icon-tag">
                        <a href="#" class="small-alph-iconButt" onclick="Get_A_to_Z_Value('R')" >R</a></div>
                    <div id="small-alph-icon-tag">
                        <a href="#" class="small-alph-iconButt" onclick="Get_A_to_Z_Value('S')" >S</a></div>
                    <div id="small-alph-icon-tag">
                        <a href="#" class="small-alph-iconButt" onclick="Get_A_to_Z_Value('T')" >T</a></div>
                    <div id="small-alph-icon-tag">
                        <a href="#" class="small-alph-iconButt" onclick="Get_A_to_Z_Value('U')" >U</a></div>
                    <div id="small-alph-icon-tag">
                        <a href="#" class="small-alph-iconButt" onclick="Get_A_to_Z_Value('V')" >V</a></div>
                    <div id="small-alph-icon-tag">
                        <a href="#" class="small-alph-iconButt" onclick="Get_A_to_Z_Value('W')" >W</a></div>
                    <div id="small-alph-icon-tag">
                        <a href="#" class="small-alph-iconButt" onclick="Get_A_to_Z_Value('X')" >X</a></div>
                    <div id="small-alph-icon-tag">
                        <a href="#" class="small-alph-iconButt" onclick="Get_A_to_Z_Value('Y')" >Y</a></div>
                    <div id="small-alph-icon-tag">
                        <a href="#" class="small-alph-iconButt" onclick="Get_A_to_Z_Value('Z')" >Z</a></div>
                        
                </div>

                  <div id="tag_label_transaction_popup_IGM_Container" style="margin-top:10px;">
                 &nbsp; Page No :
             </div>
              <div id="txtcon-m_transaction_list1_fcl" style="margin-top:10px;">
                  <asp:DropDownList ID="ddlSelectPageno" runat="server"  AutoPostBack="true" CssClass="listtxt_transac_FCL" OnSelectedIndexChanged="ddlSelectPageno_SelectedIndexChanged">
                </asp:DropDownList>
                </div>

                <div id="mainmastop2container_rght_tag3" style="display:none;">
                    <span >* Created </span> 
                    <span style="color:Blue;" >* BE Sent</span> 
                    <span style="color:Green;">* BE Received</span>
                    <br />
                    <span style="color:Red;">* Cancelled</span>
                </div>
            </div>


                <div id="tag_srcinner1grid_table_Container" 
                style="width: 1240px;height:600px; overflow:scroll">
                <div id="Main_Grid_Container" style="height:580px;" runat="server">
                    <asp:GridView runat="server" ID="gvdetails1" DataKeyNames="JOBID,JOBNO_PS,TYP" EmptyDataText="NO RECORD FOUND" 
                        AutoGenerateColumns="False" Width="100%" CssClass="grid-view" ShowHeader="true" 
                        ShowHeaderWhenEmpty="true" AllowPaging="True" OnRowDataBound="gvdetails_RowDataBound"
                        AllowSorting="True" CellPadding="2" CellSpacing="2" OnRowCreated="gvdetails_RowCreated" 
                        PageSize="23"  >
                        <AlternatingRowStyle BackColor="White" BorderColor="#C8C8C8" BorderStyle="Solid"
                            BorderWidth="1px" />
                             <HeaderStyle  Font-Underline="false" ForeColor="Black"   />
                        <Columns>
                            <asp:BoundField DataField="JOBNO_PS" HeaderText="Job No"   HeaderStyle-Width="100px" ControlStyle-Width="100px" SortExpression="JOBNO_PS" >
                                <ItemStyle CssClass="column_style_left5" Width="100px" Font-Size="12px" />
                            </asp:BoundField>

                            <asp:BoundField DataField="JOBDATE" HeaderText="Job Date"   HeaderStyle-Width="80px" ControlStyle-Width="80px" DataFormatString="{0:DD/MM/YYYY}" SortExpression="JOBDATE">
                                <ItemStyle CssClass="column_style_left5" Width="80px" Font-Size="12px" />
                            </asp:BoundField>

                             <asp:BoundField DataField="FILE_REF_NO" HeaderText="File Ref. No"   HeaderStyle-Width="80px" ControlStyle-Width="80px" >
                                <ItemStyle CssClass="column_style_left5" Width="80px" Font-Size="12px" />
                            </asp:BoundField>
                            <%--<asp:BoundField DataField="SBNO" HeaderText="BE No"   HeaderStyle-Width="80px" ControlStyle-Width="80px" >
                                <ItemStyle CssClass="column_style_left5" Width="80px" Font-Size="12px" />
                            </asp:BoundField>--%>
                            <asp:BoundField DataField="JOB_STS" HeaderText="Job Status"   HeaderStyle-Width="80px" ControlStyle-Width="80px" >
                                <ItemStyle CssClass="column_style_left5" Width="80px" Font-Size="12px" />
                            </asp:BoundField>

                           <%--

                            <asp:BoundField DataField="Imp_Name_Short"   HeaderText="Importer Name" ControlStyle-Width="170px" HeaderStyle-Width="170px" SortExpression="IMPORTER_NAME">
                                <ItemStyle CssClass="column_style_left5" Width="150px" Font-Size="12px"  />
                            </asp:BoundField>--%>

                            <asp:BoundField DataField="TYP"   HeaderText="Catory" ControlStyle-Width="100px" HeaderStyle-Width="100px" >
                                <ItemStyle CssClass="column_style_left5" Width="100px" Font-Size="12px"  />
                            </asp:BoundField>
                             <asp:BoundField DataField="TYPE"   HeaderText="Type" ControlStyle-Width="80px" HeaderStyle-Width="80px" >
                                <ItemStyle CssClass="column_style_left5" Width="80px" Font-Size="12px"  />
                            </asp:BoundField>

                      
                        
                        
                               <asp:BoundField DataField="TRANSPORT_MODE" HeaderText="Mode" ControlStyle-Width="80px" HeaderStyle-Width="80px" >
                                <ItemStyle CssClass="column_style_left5" Width="80px" Font-Size="12px"  />
                            </asp:BoundField>

                                   <%-- <asp:BoundField DataField="client_Short" HeaderText="Client Name" ControlStyle-Width="100px" HeaderStyle-Width="100px" SortExpression="client_Short" >
                                <ItemStyle CssClass="column_style_left5" Width="100px"  Font-Size="12px"  />
                            </asp:BoundField>--%>

                              <asp:BoundField DataField="SUPPLIER_SHORT" HeaderText="Supplier Name" ControlStyle-Width="100px" HeaderStyle-Width="100px" SortExpression="Supplier_Short" >
                                <ItemStyle CssClass="column_style_left5" Width="100px"  Font-Size="12px"  />
                            </asp:BoundField>

                            <asp:BoundField DataField="Imp_Name_Short" HeaderText="Importer Name" ControlStyle-Width="100px" HeaderStyle-Width="100px" SortExpression="Imp_Name_Short" >
                                <ItemStyle CssClass="column_style_left5" Width="100px"  Font-Size="12px"  />
                            </asp:BoundField>

                          

                            <asp:BoundField DataField="PORT_ORIGIN"   HeaderText="POL" ControlStyle-Width="100px" HeaderStyle-Width="100px" >
                                <ItemStyle CssClass="column_style_left5" Width="100px" Font-Size="12px"  />
                            </asp:BoundField>
                          
                              <asp:BoundField DataField="PORTDELIVERY"   HeaderText="POD" ControlStyle-Width="100px" HeaderStyle-Width="100px" >
                                <ItemStyle CssClass="column_style_left5" Width="100px" Font-Size="12px"  />
                            </asp:BoundField>

                              <%--<asp:BoundField DataField="NOOFPACKAGE"   HeaderText="No.of Pkgs" ControlStyle-Width="80px" HeaderStyle-Width="80px" >
                                <ItemStyle CssClass="column_style_left5" Width="80px" Font-Size="12px"  />
                            </asp:BoundField>--%>
                          
                           <%-- <asp:BoundField DataField="GROSSWEIGHT"   HeaderText="Weight" ControlStyle-Width="80px" HeaderStyle-Width="80px" >
                                <ItemStyle CssClass="column_style_left5" Width="80px" Font-Size="12px"  />
                            </asp:BoundField>--%>

                             <%--<asp:BoundField    HeaderText="Status" ControlStyle-Width="120px" HeaderStyle-Width="120px" >
                                <ItemStyle CssClass="column_style_left5" Width="120px" Font-Size="12px"  />
                            </asp:BoundField>--%>

                           <%-- <asp:BoundField    HeaderText="Job Pending" ControlStyle-Width="120px" HeaderStyle-Width="120px" >
                                <ItemStyle CssClass="column_style_left5" Width="120px" Font-Size="12px"  />
                            </asp:BoundField>

                             <asp:BoundField    HeaderText="Job Approved" ControlStyle-Width="120px" HeaderStyle-Width="120px" >
                                <ItemStyle CssClass="column_style_left5" Width="120px" Font-Size="12px"  />
                            </asp:BoundField>

                              <asp:BoundField    HeaderText="Invoice Pending" ControlStyle-Width="120px" HeaderStyle-Width="120px" >
                                <ItemStyle CssClass="column_style_left5" Width="120px" Font-Size="12px"  />
                            </asp:BoundField>

                             <asp:BoundField    HeaderText="Invoice Completed" ControlStyle-Width="120px" HeaderStyle-Width="120px" >
                                <ItemStyle CssClass="column_style_left5" Width="120px" Font-Size="12px"  />
                            </asp:BoundField>

                             <asp:BoundField    HeaderText="Job Closed" ControlStyle-Width="120px" HeaderStyle-Width="120px" >
                                <ItemStyle CssClass="column_style_left5" Width="120px" Font-Size="12px"  />
                            </asp:BoundField>

                             <asp:BoundField    HeaderText="Job Cancelled" ControlStyle-Width="120px" HeaderStyle-Width="120px" >
                                <ItemStyle CssClass="column_style_left5" Width="120px" Font-Size="12px"  />
                            </asp:BoundField>--%>
                         <%--   <asp:BoundField DataField="CUSTOMS_HOUSE" HeaderText="Customs House" ControlStyle-Width="110px" HeaderStyle-Width="110px" SortExpression="CUSTOMS_HOUSE">
                                <ItemStyle CssClass="column_style_left5" Width="110px"  Font-Size="12px"/>
                            </asp:BoundField>--%>
                          

                          <%--  <asp:BoundField DataField="IMPORTER_NAME" HeaderText="Impname">
                            <HeaderStyle CssClass="hideGridColumn" />
                            <ItemStyle CssClass="hideGridColumn" />
                            </asp:BoundField>
--%>
                            <asp:BoundField DataField="JOB_FILED_STATUS">
                            <HeaderStyle CssClass="hideGridColumn" />
                            <ItemStyle CssClass="hideGridColumn" />
                            </asp:BoundField>
                       
                        </Columns>
                        <PagerSettings Visible="False" />
                    </asp:GridView>
                </div>

              <%--  test-grid start--%>
                    <div id="dvHeader" style="display: none">
                        <table cellspacing="0" rules="all" border="1" id="Table1" style="font-family: Arial;
                            font-size: 11pt; width: 450px; border-collapse: collapse;">
                            <tr style="color: White; background-color: Green;">
                                <th scope="col" style="width: 150px;">
                                    CustomerID
                                </th>
                                <th scope="col" style="width: 150px;">
                                    City
                                </th>
                                <th scope="col" style="width: 150px">
                                    Country
                                </th>
                            </tr>
                        </table>
                    </div>
            </div>
          
        </div>
        
            <asp:Button ID="btn_A_TO_Z_Search" runat="server" OnClick="btn_A_TO_Z_Search_Click" style="display:none;"/>
            <asp:HiddenField ID="hdn_A_to_Z_Search" runat="server" />
            <asp:HiddenField ID="HD_tbl_sort" runat="server" />
            <asp:HiddenField ID="hdrowindex" runat="server" />
            <asp:HiddenField ID="BE_Type" runat="server" />
            <asp:HiddenField ID="hdnFreightMode" runat="server" />
            <asp:HiddenField ID="HDBranch" runat="server" />

   <script language='javascript'>
       window.onload = happycode;
       function happycode() {
           document.getElementById("MASTER_ID").click();
       }

       function Get_A_to_Z_Value(val) {
           document.getElementById("<%=hdn_A_to_Z_Search.ClientID %>").value = val;
           document.getElementById("<%=btn_A_TO_Z_Search.ClientID %>").click();
       }
       
</script>

<script language="javascript">
    function checklist() {
        var jobno = document.getElementById("<%=hdrowindex.ClientID %>").value;
        if (jobno != '') {
            NewWindow1('../Report/Import_ReportViewer.aspx?jobno=' + document.getElementById("<%=hdrowindex.ClientID %>").value + '&CH=L', 'List', '810', '1024', 'yes');
        }
        else {
            alert('Select a Job');
        }
    }

    function Submission() {
        var jobno = document.getElementById("<%=hdrowindex.ClientID %>").value;
        if (jobno != '') {
            location.href = '<%= Page.ResolveUrl("../FlatFile/Import_Flat_File_Generation.aspx?jobno=' + jobno + '&SUB=Y") %>';
        }
        else {
            alert('Select a Job');
        }
    }

    function Tracking() {
        var jobno = document.getElementById("<%=hdrowindex.ClientID %>").value;
        if (jobno != '') {
            location.href = '<%= Page.ResolveUrl("~/Read_Mail/General_Search.aspx?JOBTYPE=Import") %>';
        }
        else {
            alert('Select a Job');
        }

    }

    function Annexure() {

        var jobno = document.getElementById("<%=hdrowindex.ClientID %>").value;
        if (jobno != '') {
            NewWindow1('../Import_Annexure/Import_Annexure_Report.aspx?jobno=' + document.getElementById("<%=hdrowindex.ClientID %>").value + '&Anx=Y', 'List', '810', '1024', 'yes');
        }
        else {
            alert('Select a Job');
        }

    }

    function Gatt() {
        var jobno = document.getElementById("<%=hdrowindex.ClientID %>").value;
        if (jobno != '') {

            NewWindow1('GATT_form.aspx?jobno=' + document.getElementById("<%=hdrowindex.ClientID %>").value + '', 'List', '810', '1024', 'yes');
        }
        else {
            alert('Select a Job');
        }
    }
    function Go_Warehouse() {

        var jobno = document.getElementById("<%=hdrowindex.ClientID %>").value;
        var BE_Type = document.getElementById("<%=BE_Type.ClientID %>").value;
        if (jobno != '' && BE_Type == 'Warehouse') {

            NewWindow1('../Exbond_Report/WareHouse_Report_Details.aspx?jobno=' + document.getElementById("<%=hdrowindex.ClientID %>").value + '', 'List', '810', '1024', 'yes');
        }
        else {
            alert('Select a Warehouse Job');
        }
    }

    function BE_Entry() {

        var jobno = document.getElementById("<%=hdrowindex.ClientID %>").value;
        if (jobno != '') {
            NewWindow1('../Transaction/BE_Entry_General.aspx?JOBID=' + document.getElementById("<%=hdrowindex.ClientID %>").value + '', 'List', '810', '500', 'yes');
        }
        else {
            alert('Select a Job');
        }
    }

    function Error_Find() {
        var jobno = document.getElementById("<%=hdrowindex.ClientID %>").value;
        if (jobno != '') {
            NewWindow1('../FlatFile/IMP_Flatfile_Validation.aspx?Jobno=' + encodeURIComponent(jobno) + '', 'List', '1100', '1024', 'yes');
        }
        else {
            alert('Select a Job');
        }
    }
    function Job_Status() {
        var JobNo_Status = document.getElementById("<%=hdrowindex.ClientID %>").value;
        if (JobNo_Status != '') {
            NewWindow1('../Transaction/Imp_Exp_Job_Status.aspx?JOBID=' + JobNo_Status + '&IMP_EXP=I', 'List', '450', '160', 'yes');
        }
        else {
            alert('Select a Job');
        }
    }
 
 </script>


 <script type="text/javascript">
     $(document).ready(function () {
         /* gridviewScroll();*/

         var totalRows = $("#<%=gvdetails1.ClientID %> tr").length;
         var lastProductId = $("#<%=gvdetails1.ClientID %> tr:last").children("td:first").html();

         if (totalRows > 2) {
             gridviewScroll();
         }
         else {

             if (lastProductId > 0) {
                 gridviewScroll();
             }
             else {
                 /*alert (lastProductId);*/
             }
         }
     });

     function gridviewScroll() {
         gridView1 = $('#<%=gvdetails1.ClientID %>').gridviewScroll({

             width: 1230,
             height: 700,
             railcolor: "#F0F0F0",
             barcolor: "#CDCDCD",
             barhovercolor: "#606060",
             bgcolor: "#F0F0F0",
             freezesize: 1,
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
       </asp:Content>

