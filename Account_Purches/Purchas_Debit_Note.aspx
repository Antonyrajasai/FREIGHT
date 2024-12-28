<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Purchas_Debit_Note.aspx.cs" Inherits="Account_Purches_Purchas_Debit_Note" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=iso-8859-1" />
    <title>Purchase Debit Note</title>
    <link href="../css/MainStyle.css" rel="stylesheet" media="screen, projection" type="text/css" />
    <link href="../main.css" rel="stylesheet" media="screen, projection" type="text/css" />
    <link href="../gridmain.css" rel="stylesheet" media="screen, projection" type="text/css" />
    <link rel="stylesheet" href="../css/jquery-ui.css" />
    <link rel="stylesheet" type="text/css" href="../css/dddropdownpanel.css" />
    <link rel="stylesheet" href="../windowfiles/dhtmlwindow.css" type="text/css" />
    <link rel="stylesheet" href="../modalfiles/modal.css" type="text/css" />
    <link href="../tabs/tabs.css" rel="stylesheet" type="text/css" />
    <link href="../AutoComplete_CSS/jquery-ui.css.css" rel="stylesheet" type="text/css" />
    <!-- VALIDATION SCRIPT -->
    <link rel="stylesheet" href="../Validation Files/validationEngine.css" type="text/css" />
    <link rel="stylesheet" href="../Validation Files/template.css" type="text/css" />
    <!-- VALIDATION SCRIPT -->
    <script src="../MessageBox_js/jquery.js" type="text/javascript"></script>
    <link href="../MessageBox_js/jquery.alerts.css" rel="stylesheet" type="text/css" />
    <script src="../MessageBox_js/jquery.alerts.js" type="text/javascript"></script>
    <script src="../js/Billing/Invoice.js" type="text/javascript"></script>
 
<script type="text/javascript" language="javascript">
    function EnableTextbox(ddl, txtD, txtC) {
        var ddl_PaymentMethod = document.getElementById(ddl);
        var txt_Debit = document.getElementById(txtD);
        var txt_Credit = document.getElementById(txtC);
        var ddl_Value = ddl_PaymentMethod.value;
        if (ddl_Value == "CR") {
            txt_Debit.disabled = true;
            txt_Credit.disabled = false;
        }
        else if (ddl_Value == "DR") {
            txt_Debit.disabled = false;
            txt_Credit.disabled = true;
        }
    }
    $(document).ready(function () {
        var today = new Date();
        $('.datepicker').datepicker({
            format: 'dd-mm-yyyy',
            autoclose: true,
            endDate: "today",
            maxDate: today
        }).on('changeDate', function (ev) {
            $(this).datepicker('hide');
        });


        $('.datepicker').keyup(function () {
            if (this.value.match(/[^0-9]/g)) {
                this.value = this.value.replace(/[^0-9^-]/g, '');
            }
        });
    });

    </script>
    <style type="text/css">
        .modalPopup12
        {
            background-color: transparent;
            width: 940px;
            height: 180px;
        }
    </style>
    <style type="text/css">
        .ListControl input[type=radio] + label
        {
            display: table-cell;
            width: 7.5em;
            height: 5em;
            line-height: 0em;
            text-align: left;
        }
        
        .ListControlWidth input[type=radio] + label
        {
            display: table-cell;
            width: 10em;
            height: 5em;
            line-height: 0em;
            text-align: left;
        }
    </style>
    <style type="text/css">
        .CalendarStyle
        {
            background-color: Crimson;
            color: White;
            border: solid 2px DarkRed;
        }
        .ajax__calendar_header
        {
            background-color: #FBB117;
            color: black;
        }
        .ajax__calendar_footer
        {
            background-color: #FBB117;
            color: black;
        }
    </style>
    <style type="text/css">
        .FixedHeader
        {
            position: absolute;
            font-weight: bold;
        }
        
        .hideGridColumn
        {
            display: none;
        }
        
        .hideGridColumn1
        {
            display: none;
        }
    </style>
    <style>
        div.fileinputs {
            position: relative;
        }

        div.fakefile {
            position: absolute;
            top: 0px;
            left: 0px;
            z-index: 1;
        }

        input.file {
            position: relative;
            text-align: right;
            -moz-opacity: 0;
            filter: alpha(opacity: 0);
            opacity: 0;
            z-index: 2;
        }
 </style>

</head>
<body style="overflow: hidden;">
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePartialRendering="false"
        LoadScriptsBeforeUI="false" EnablePageMethods="true">
    </asp:ScriptManager>
   
            <div class="loading" align="center" id="load" style="display: none;">
                <img src="../Loading_Images/indicator_mozilla_blu.gif" alt="" />
            </div>
            <div id="innerbox_MidMain" style="height: 150px;">
                <div id="tag_srcinner1">
                    <div id="mainmastop2container_rght_tag2_txt1" style="width: 130px;">
                        Purchase Debit Note 
                    </div>
                    <div id="verslic">
                    </div>
                    <div id="tag_transac_lft_in2" style="margin-top: 5px;">
                        <div id="tag_label_transact_Src" style="width: 80px;">
                            Generate Bill:
                        </div>
                        <div id="txtcon-m_Exchange" style="width: 200px;">
                            <asp:RadioButtonList ID="Rd_Bill_Type" runat="server" RepeatDirection="Horizontal"
                                AutoPostBack="true" >
                                <asp:ListItem Text="Local State" Value="L" Selected="True"></asp:ListItem>
                                <asp:ListItem Text="Other State" Value="O"></asp:ListItem>
                            </asp:RadioButtonList>
                        </div>
                    </div>
                    <div id="tag_transac_lft_in2" style="margin-top: 5px;">
                        
                    </div>
                    
                      <div id="popupwindow_closebut_right_new">
                  <input type="close" value="Submit" class="clsicon_new" onclick="win_hide();parent.adcodewindow.hide();return false;" />
                  </div>
                </div>
                <div id="tag_transact_src_inner" style="width: 1500px;height:120px;">
                    <div id="tag_Exchange_inner_lft" style="width: 1500px;height:120px;">
                        <div id="tag_transact_lft_in1" style="width: 1500px; height:120px;">                            
                            <div style="width: 1500px; height: 40px">
                                <div id="txt_container_Transact_Main_l" style="width:210px;">
                                    <div id="tag_label_transact_Src" style="width: 100px;">
                                       P DebitNo
                                    </div>
                                    <div id="txtcon-m_Exchange" style="width:110px;" >
                                        <asp:TextBox ID="txtDebitno_ps" CssClass="txtbox_none_Mid_transac_code" Enabled="true" runat="server"></asp:TextBox>
                                          <asp:TextBox ID="txtDebitno" runat="server" MaxLength="50" Font-Size="12px" CssClass="txtbox_none_Mid_transac_code"
                                              BorderColor="White" Width="0px" Style="display:none" ReadOnly="True" TabIndex="1" ClientIDMode="Static"></asp:TextBox>
                                    </div>
                                </div>
                                <div id="txt_container_Transact_Main_l" style="width:1257px;">
                                    <div id="tag_label_transact_Src" style="width: 80px;">
                                           P DebitDate
                                    </div>
                                    <div id="txtcon-m_Exchange" style="width:160px;">
                                   
                                        <%-- <asp:TextBox ID="txtVoucherdate" CssClass="txtbox_none_Mid_transac_code" OnTextChanged="Voucherdate_changed" 
                                           onChange="Voucher();"   Width="140px" runat="server" AutoPostBack="True"></asp:TextBox>
                                         <cc1:MaskedEditExtender ID="MaskedEditExtender2" runat="server" TargetControlID="txtVoucherdate"
                                Mask="99/99/9999" MessageValidatorTip="true" OnInvalidCssClass="MaskedEditError"
                                MaskType="Date" DisplayMoney="Left" AcceptNegative="Left" ErrorTooltipEnabled="True" >
                            </cc1:MaskedEditExtender>
                            <cc1:CalendarExtender ID="CalendarExtender3" runat="server" TargetControlID="txtVoucherdate"
                                PopupButtonID="txtJobDate" Enabled="true" Format="dd/MM/yyyy">
                            </cc1:CalendarExtender>
                                     </div>--%>
                                         <asp:TextBox ID="txtDebitdate" CssClass="txtbox_none_Mid_transac_code"  OnTextChanged="Debitdate_changed" 
                                          onChange="Voucher();"  AutoPostBack="True" Width="140px" runat="server" TabIndex="2" ></asp:TextBox>
                                         <cc1:MaskedEditExtender ID="MaskedEditExtender5" runat="server" TargetControlID="txtDebitdate"
                                Mask="99/99/9999" MessageValidatorTip="true" OnInvalidCssClass="MaskedEditError"
                                MaskType="Date" DisplayMoney="Left" AcceptNegative="Left" ErrorTooltipEnabled="True" > 
                            </cc1:MaskedEditExtender>
                            <cc1:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtDebitdate"
                                PopupButtonID="txtJobDate" Enabled="true" Format="dd/MM/yyyy">
                            </cc1:CalendarExtender>
                            </div>

                                      <div id="tag_label_transact_Src" style="width: 380px;">
                                         Vendor Name
                                    </div>
                                    <div id="txtcon-m_Exchange" style="width:180px;height:400px;position:absolute;margin-left:345px;" >
                                      

                                         <asp:DropDownList ID="ddlVendorname" runat="server" class="chosen-select-deselect" ClientIDMode="Static"
                                          AutoPostBack="true"   OnSelectedIndexChanged="ddlVendorname_SelectedIndexChanged"
                                        TabIndex="3" Width="245px"  ></asp:DropDownList>

                                         </div>
                                
                                     <div id="tag_label_transact_Src" style="width: 65px;">
                                         BillNo
                                    </div>
                                    <div id="txtcon-m_Exchange" style="width:130px;" >
                                        <asp:DropDownList ID="ddlPurchesbillno" CssClass="listtxt_transac_item_gen_notn" Width="120px" AutoPostBack="true" OnSelectedIndexChanged="ddlPurchesbillno_SelectedIndexChanged"
                                            runat="server" TabIndex="4"  >
                                        </asp:DropDownList>    
                                    </div>
                                    <div id="tag_label_transact_Src" style="width: 90px;">
                                        Vendor Branch 
                                    </div>
                                    <div id="txtcon-m_Exchange" style="width: 130px">
                                         
                                         <asp:DropDownList ID="ddlVendorBranch" CssClass="listtxt_transac_item_gen_notn" Width="120px" AutoPostBack="true"
                                     OnSelectedIndexChanged="ddlVendorBranch_SelectedIndexChanged"
                                            runat="server" TabIndex="5"  >
                                        </asp:DropDownList>

                                    </div>
                                      <div id="tag_label_transact_Src" style="width: 50px;visibility:hidden;">
                                         Ref No
                                    </div>
                                    <div id="txtcon-m_Exchange" style="width:130px;visibility:hidden;" >
                                        <asp:TextBox ID="txtrefno" CssClass="txtbox_none_Mid_transac_code" runat="server" TabIndex="6" style="width:120px"></asp:TextBox>
                                          
                                    </div>
                                </div>
                                
                                <div  id="txt_container_Transact_Main_l"  style="width: 1450px; height: 28px;" >
                                    
                                    
                                    <div id="tag_label_transact_Src" style="width: 100px;">
                                       Vendor State 
                                    </div>
                                    <div id="txtcon-m_Exchange" style="width: 110px">
                                        <asp:TextBox ID="txtVendorstate" CssClass="txtbox_none_Mid_transac_code" Enabled="false"  runat="server" TabIndex="7"></asp:TextBox>
                                    </div>
                                    
                                    <div id="tag_label_transact_Src" style="width: 80px;">
                                       GSTN 
                                    </div>
                                    <div id="txtcon-m_Exchange" style="width: 150px">
                                        <asp:TextBox ID="txtGstn" CssClass="txtbox_none_Mid_transac_code" Enabled="false" Width="140px"  runat="server" TabIndex="8"></asp:TextBox>
                                    </div>
                                    <div id="tag_label_transact_Src" style="width: 80px;">
                                       GSTN Types
                                    </div>
                                    <div id="txtcon-m_Exchange" style="width: 150px">
                                        <asp:TextBox ID="txtGstntype" CssClass="txtbox_none_Mid_transac_code" Enabled="false"  Width="140px"  runat="server" TabIndex="9"></asp:TextBox>
                                    </div>
                                    <div id="tag_label_transact_Src" style="width: 85px;">
                                       Country 
                                    </div>
                                    <div id="txtcon-m_Exchange" style="width: 130px">
                                        <asp:TextBox ID="txtCountry" CssClass="txtbox_none_Mid_transac_code" 
                                            Enabled="false"  runat="server" Width="120px" TabIndex="10"></asp:TextBox>
                                    </div>
                                    <div id="tag_label_transact_Src" style="width: 90px;">
                                       Narration 
                                    </div>
                                    <div id="txtcon-m_Exchange" style="width: 300px">
                                        <asp:TextBox ID="txtNarration" CssClass=" txtbox_none_Mid_transac_code" Width="300px" Height="20px"  TextMode="MultiLine" runat="server" TabIndex="11"></asp:TextBox>
                                    </div>
                                </div>
                                 
                           
                           <div style="width: 1400px; height: 30px">                             
                                    <div id="tag_label_transact_Src" style="width: 100px;">
                                        Bill No
                                    </div>
                                    <div id="txtcon-m_Exchange" style="width:110px;" >
                                        <asp:TextBox ID="txtVoucherno_ps" AutoPostBack="true"  CssClass="validate[required] txtbox_none_Mid_transac_code"
                                        runat="server" TabIndex="12" OnTextChanged="txtVoucherno_ps_TextChanged"></asp:TextBox>
                                          <asp:TextBox ID="txtVoucherno" runat="server" MaxLength="50" Font-Size="12px" CssClass="txtbox_none_Mid_transac_code"
                                              BorderColor="White" Width="0px" Style="display:none"   ClientIDMode="Static"></asp:TextBox>
                                    </div>
                                
                                <div id="tag_label_transact_Src" style="width: 80px;">
                                         Bill Date
                                    </div>
                                    <div id="txtcon-m_Exchange" style="width: 150px;">
                                        <asp:TextBox ID="txtBilldate" CssClass="validate[required] txtbox_none_Mid_transac_code" Width="140px" onfocusout="isDate()"  runat="server" TabIndex="13"></asp:TextBox>
                                        <cc1:MaskedEditExtender ID="MaskedEditExtender1" runat="server" TargetControlID="txtBilldate"
                                Mask="99/99/9999" MessageValidatorTip="true" OnInvalidCssClass="MaskedEditError"
                                MaskType="Date" DisplayMoney="Left" AcceptNegative="Left" ErrorTooltipEnabled="True" >
                            </cc1:MaskedEditExtender>
                            <cc1:CalendarExtender ID="CalendarExtender2" runat="server" TargetControlID="txtBilldate"
                                PopupButtonID="txtJobDate" Enabled="true" Format="dd/MM/yyyy">
                            </cc1:CalendarExtender>
                                    </div> 
                                       <div id="tag_label_transact_Src" style="width: 80px;">
                                        Currency
                                    </div>
                                    <div id="txtcon-m_Exchange" style="width: 150px;">
                                        <asp:TextBox ID="txtInvoiceCurrency"  runat="server" ClientIDMode="Static"  TabIndex="14"  style="width: 140px;"  onchange="Calculationex();" onkeypress="return char1(event)"  MaxLength="3" CssClass="validate[required,minSize[3],custom[onlyLetterSp]] txtbox_none_Mid_transac_code" ></asp:TextBox>
                                    </div>
                                                       
                                    <div id="tag_label_transact_Src" style="width: 80px;">
                                       Ex Rate
                                    </div>
                                    <div id="txtcon-m_Exchange" style="width: 130px;">
                                        <asp:TextBox ID="txtExrate"  runat="server" Enabled="true"   TabIndex="15"
                                         MaxLength="50" CssClass="validate[required] txtbox_none_Mid_transac_code" 
                                            Width="120px" ></asp:TextBox>
                                    </div>               
                                    <div id="tag_label_transact_Src" style="width: 97px;">
                                    Foreign Amount
 
                                    </div>
                                    <div id="txtcon-m_Exchange"  style="width: 90px; ">
                                        <asp:TextBox ID="txtbillamount"  runat="server" style="text-align:right;"  
                                        CssClass=" txtbox_none_Mid_transac_item_maindet" onchange="TOTALAmt();"
                                        onblur="foreign_amt();"
                                        onkeyup="extractNumber(this,5,false);return Clear_Dot(this);"
                                        onkeypress="return blockNonNumbers(this, event, true, false);"
                                        TabIndex="16" MaxLength="50"  ></asp:TextBox>
                                        
                                    </div>
                                                          

                                    <div id="tag_label_transact_Src" style="width: 120px;">
                                       Total Amount INR
                                    </div>
                                    <div id="txtcon-m_Exchange" style="width: 150px;">
                                        <asp:TextBox ID="txtTotalamtininr" Enabled="true"  runat="server" TabIndex="17" ClientIDMode="Static" 
                                        style="text-align:right;"  onchange="totalAmt()" onfocusout="tap()"
                                        CssClass="validate[required,custom[number]] txtbox_none_Mid_transac_item_maindet"
                                        onblur="extractNumber(this,5,false);remove_zero_after_decimal_point(this.value,'txtbillamount');"
                                        onkeyup="extractNumber(this,5,false);return Clear_Dot(this);"
                                        onkeypress="return blockNonNumbers(this, event, true, false);"
                                          MaxLength="50"  ></asp:TextBox>
                                    </div>
                                </div>                             
                           </div>
                           </div>
                           
                        </div>
                    </div>
                </div>
            
            <%-- -------------------------------------Tab--------------------------------------------------------%>
            <div class="content" id="page-1" style="margin-top: -30px;">
                <%---28 --%>
                <div id="innerbox_MidMain_Trans_new" style="height: 240px; width: 1300px; margin-left: -16px; overflow-y:scroll; overflow-x:scroll;
                    margin-top: -5px;">
                    
                                        <asp:GridView ID="gvdetails" runat="server" EmptyDataText="NO RECORD FOUND" AutoGenerateColumns="false" 
                                            BackColor="WhiteSmoke" CssClass="grid-view" Width="100%" ShowHeaderWhenEmpty="True" 
                                             AllowSorting="True" OnRowCommand="gvdetails_RowCommand"  OnRowDataBound="OnRowDataBound"
                                               
                                             BorderColor="#C8C8C8" BorderStyle="Solid" 
                                            HorizontalAlign="Left"    ShowFooter="true" Style="overflow: auto;">
                                            <AlternatingRowStyle BackColor="White" BorderColor="#C8C8C8" BorderStyle="Solid"
                                            BorderWidth="1px" />
                                            <Columns>
                                          
                                            <asp:TemplateField HeaderText="SNo" HeaderStyle-Width="30px" >
                        <ItemTemplate>
                            <asp:TextBox ID="txtgSLNo" runat="server" Text='<%# Eval("SNo") %>'  Width="30px"></asp:TextBox>
                        </ItemTemplate>
                        <ItemStyle Width="30px" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="DR_CR">
                        <ItemTemplate>
                            <asp:DropDownList ID="drpDrcr" DataValueField="DRCR" Width="45px" onchange="drcr(this)"  Name="drpDrcr" 
                                SelectedValue='<%# Eval("DR_CR") %>' AppendDataBoundItems="true"  runat="server"  >
                            <asp:ListItem Value="DR">DR</asp:ListItem>
                            <asp:ListItem Value="CR">CR</asp:ListItem>
                            </asp:DropDownList>
                        </ItemTemplate>
                        <ItemStyle Width="50px" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Imp_Exp">
                        <ItemTemplate>
                            <asp:DropDownList ID="drpImpexp" Width="55px" SelectedValue='<%# Eval("Imp_Exp") %>'  AppendDataBoundItems="true"     runat="server"  >
                            <asp:ListItem>---</asp:ListItem>
                             <asp:ListItem Text="FF AI" Value="Imp FW Air"></asp:ListItem>                            <asp:ListItem Text="FF SI" Value="Imp FW Sea"></asp:ListItem>                            <asp:ListItem Text="CL AI" Value="Imp CL Air"></asp:ListItem>                               <asp:ListItem Text="CL SI" Value="Imp CL Sea"></asp:ListItem>                                <asp:ListItem Text="CC AI" Value="Imp CC Air"></asp:ListItem>                                 <asp:ListItem Text="CC SI" Value="Imp CC Sea"></asp:ListItem>                                   <asp:ListItem Text="FF AE" Value="Exp FW Air"></asp:ListItem>                                     <asp:ListItem Text="FF SE" Value="Exp FW Sea"></asp:ListItem>                                        <asp:ListItem Text="CL AE" Value="Exp CL Air"></asp:ListItem>                                          <asp:ListItem Text="CL SE" Value="Exp CL Sea"></asp:ListItem>
                            <%--<asp:ListItem >Imp FW Air</asp:ListItem>
                            <asp:ListItem >Imp FW Sea</asp:ListItem>
                            <asp:ListItem >Imp CL Air</asp:ListItem>
                            <asp:ListItem >Imp CL Sea</asp:ListItem>
                            <asp:ListItem >Exp FW Air</asp:ListItem>
                            <asp:ListItem >Exp FW Sea</asp:ListItem>
                            <asp:ListItem >Exp CL Air</asp:ListItem>
                              <asp:ListItem >Exp CL Sea</asp:ListItem>--%>
                            </asp:DropDownList>
                        </ItemTemplate>
                        <ItemStyle Width="50px" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="JobNo" HeaderStyle-Width="90px">
                        <ItemTemplate>
                            <asp:TextBox ID="txtJOBNO" runat="server"    AutoPostBack="false"  onchange="cusname(this)" onfocusout="cusname(this)"  Text='<%# Eval("JobNo") %>'   Width="120px" CssClass=" Fileref" ></asp:TextBox>
                            
                         <asp:HiddenField ID="hdngFileRefNo" runat="server" />
                        </ItemTemplate>
                        <ItemStyle Width="90px" />
                    </asp:TemplateField>   
                    <asp:TemplateField HeaderText="File_Ref_No" HeaderStyle-Width="90px">
                        <ItemTemplate>
                            <asp:TextBox ID="txtgFileRefNo" runat="server"    AutoPostBack="false"  Text='<%# Eval("File_Ref_No") %>'   Width="120px" ></asp:TextBox>
                            
                      
                        </ItemTemplate>
                        <ItemStyle Width="90px" />
                    </asp:TemplateField>

                    <%--//ADD NEW ONE TAX KINVOICE NO BY ROSI--%>
                     <asp:TemplateField HeaderText="Invoice_No" HeaderStyle-Width="90px">
                        <ItemTemplate>
                            <asp:TextBox ID="txtTAX_INVNO_PS" runat="server"    AutoPostBack="false"  Text='<%# Eval("TAX_INVNO_PS") %>'   Width="120px" ></asp:TextBox>
                            
                      
                        </ItemTemplate>
                        <ItemStyle Width="90px" />
                    </asp:TemplateField>       
                                     
                    <asp:TemplateField HeaderText="Date">
                        <ItemTemplate>
                            <asp:TextBox ID="txtgDate" Enabled="true" runat="server" Text='<%# Eval("Date") %>'  Width="80px"></asp:TextBox>
                             
                            <cc1:CalendarExtender ID="CalendarExtender2" runat="server" TargetControlID="txtgDate"
                                PopupButtonID="txtgDate" Enabled="true" Format="dd/MM/yyyy">
                            </cc1:CalendarExtender>
                        </ItemTemplate>
                        <ItemStyle Width="80px" />
                    </asp:TemplateField>
                    
                    <asp:TemplateField HeaderText="Customer_Name">
                        <ItemTemplate>
                            <asp:TextBox ID="txtgCusName" runat="server" Enabled="true" Text='<%# Eval("Customer_Name") %>'   Width="120px"  ></asp:TextBox>
                        </ItemTemplate>
                        <ItemStyle Width="90px" />
                    </asp:TemplateField>
                    
                    <asp:TemplateField HeaderText="Charge_Head">
                        <ItemTemplate>
                            <asp:TextBox ID="txtgCharHead" Width="120px" runat="server"   AutoPostBack="false"  onblur="Calculation(this)" 
                             Text='<%# Eval("Charge_Name") %>' CssClass=" Search" ></asp:TextBox>
                            <asp:HiddenField ID="hfCustomerId" runat="server" />
                        </ItemTemplate>
                        <ItemStyle Width="90px" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Gst_Type">
                        <ItemTemplate>
                           
                            <asp:DropDownList ID="ddl_Gst_type" runat="server" SelectedValue='<%# Eval("GST_TYPE") %>'   onblur="Calculation(this)" Width="90px">
                            <%--<asp:ListItem Value="GST_TYPE_01">Imports exempt</asp:ListItem>
                            <asp:ListItem Value="GST_TYPE_02">Imports  Nil rated</asp:ListItem>
                            <asp:ListItem Value="GST_TYPE_03">Imports taxable</asp:ListItem>
                            <asp:ListItem Value="GST_TYPE_04">interstate purchase exempt</asp:ListItem>
                            <asp:ListItem Value="GST_TYPE_05">Interstate purchase from unregistered dealer - taxable</asp:ListItem>
                            <asp:ListItem Value="GST_TYPE_06">Interstate purchase from unregistered dealer - exempt</asp:ListItem>
                            <asp:ListItem Value="GST_TYPE_07">Interstate purchase from unregistered dealer - Nil rated</asp:ListItem>
                            <asp:ListItem Value="GST_TYPE_08">Interstate purchase from unregistered dealer - services</asp:ListItem>
                            <asp:ListItem Value="GST_TYPE_09">Interstate purchase taxable</asp:ListItem>
                            <asp:ListItem Value="GST_TYPE_10">Interstate purchase Nil rated</asp:ListItem>
                            <asp:ListItem Value="GST_TYPE_11">Intrastate PURCHASE deemed exports - Exempt</asp:ListItem>
                            <asp:ListItem Value="GST_TYPE_12"> Intrastate PURCHASE deemed exports - taxable</asp:ListItem>
                            <asp:ListItem Value="GST_TYPE_13">Intrastate PURCHASE deemed exports - Nil rated</asp:ListItem>
                            <asp:ListItem Value="GST_TYPE_14">PURCHASE deemed exports - exempt</asp:ListItem>
                            <asp:ListItem Value="GST_TYPE_15">PURCHASE deemed exports - taxable</asp:ListItem>
                            <asp:ListItem Value="GST_TYPE_16">PURCHASE deemed exports - Nil rated</asp:ListItem>
                            <asp:ListItem Value="GST_TYPE_17">purchase exempt</asp:ListItem>
                            <asp:ListItem Value="GST_TYPE_18">purchase From composition dealer</asp:ListItem>
                            <asp:ListItem Value="GST_TYPE_19">PURCHASE from Sez - exempt</asp:ListItem>
                            <asp:ListItem Value="GST_TYPE_20">PURCHASE from Sez - LUT/BOND</asp:ListItem>
                            <asp:ListItem Value="GST_TYPE_21">PURCHASE from Sez - NIL RATED</asp:ListItem>
                            <asp:ListItem Value="GST_TYPE_22">PURCHASE from Sez - TAXABLE</asp:ListItem>
                            <asp:ListItem Value="GST_TYPE_23">PURCHASE from Sez (Without bill of entry) -exempt</asp:ListItem>
                            <asp:ListItem Value="GST_TYPE_24">PURCHASE from Sez (Without bill of entry) -NIL RATED</asp:ListItem>
                            <asp:ListItem Value="GST_TYPE_25">PURCHASE from Sez (Without bill of entry)- TAXABLE</asp:ListItem>
                            <asp:ListItem Value="GST_TYPE_26">PURCHASE FROM UNREGISTERED DEALER -EXEMPT</asp:ListItem>
                            <asp:ListItem Value="GST_TYPE_27">PURCHASE FROM UNREGISTERED DEALER - NIL RATED</asp:ListItem>
                            <asp:ListItem Value="GST_TYPE_28">PURCHASE FROM UNREGISTERED DEALER - TAXABLE</asp:ListItem>
                            <asp:ListItem Value="GST_TYPE_29"> PURCHASE NIL RATED</asp:ListItem>
                            <asp:ListItem Value="GST_TYPE_30">PURCHASE TAXBLE </asp:ListItem>--%>
                             <asp:ListItem Value=""></asp:ListItem>
                            <asp:ListItem Value="ITC">ITC</asp:ListItem>
                            <asp:ListItem Value="RCM">RCM</asp:ListItem>
                            <asp:ListItem Value="OTH">OTH</asp:ListItem>
                            </asp:DropDownList>
                        </ItemTemplate>
                        <ItemStyle Width="60px" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Tax_Rate">
                        <ItemTemplate>
                            <asp:TextBox ID="txtgTaxrate" Enabled="true" runat="server"
                             style="text-align:right;" 
                                        CssClass="validate[custom[number]] "
                                        onblur="extractNumber(this,5,false);remove_zero_after_decimal_point(this.value,'txtbillamount');"
                                        onkeyup="extractNumber(this,5,false);return Clear_Dot(this);"
                                        onkeypress="return blockNonNumbers(this, event, true, false);"
                                         Text='<%# Eval("Tax_Rate","{0:0.00}") %>'  Width="60px" ></asp:TextBox>
                        </ItemTemplate>
                        <ItemStyle Width="50px" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Credit_FC">
                        <ItemTemplate>
                        <asp:TextBox ID="txtgCreditfc"  runat="server"  onchange="Deb_cre_value(this);"
                            Text='<%# Eval("Credit_FC","{0:0.00}") %>' Width="85px"  CssClass="validate[custom[number]] creditgstfc " ></asp:TextBox>
                            <%--<asp:TextBox ID="txtgCreditfc" runat="server"  
                            style="text-align:right;"  
                                        CssClass="validate[custom[number]] "  onchange="Deb_cre_value(this);"
                                        onblur="extractNumber(this,5,false);remove_zero_after_decimal_point(this.value,'txtbillamount');"
                                        onkeyup="extractNumber(this,5,false);return Clear_Dot(this);"
                                        onkeypress="return blockNonNumbers(this, event, true, false);" Text='<%# Eval("Credit_FC","{0:0.00}") %>' Width="85px" ClientIDMode="Static"></asp:TextBox>--%>
                        </ItemTemplate>
                         <ItemStyle Width="60px" />
                         
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Debit_FC">
                        <ItemTemplate>
                            <asp:TextBox ID="txtgDebitfc" runat="server"  style="text-align:right;" 
                                        CssClass="validate[custom[number]] debitgstfc"  onchange="Deb_cre_value(this);"
                                        onblur="extractNumber(this,5,false);remove_zero_after_decimal_point(this.value,'txtbillamount');"
                                        onkeyup="extractNumber(this,5,false);return Clear_Dot(this);"
                                        onkeypress="return blockNonNumbers(this, event, true, false);" Text='<%#Eval("Debit_FC","{0:0.00}") %>' Width="85px"  ClientIDMode="Static" ></asp:TextBox>

                        </ItemTemplate>
                         <ItemStyle Width="60px" />
                         
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Credit">
                        <ItemTemplate>
                            <asp:TextBox ID="txtgCredit1" runat="server"  
                            style="text-align:right;" 
                                        CssClass="validate[custom[number]] creditgst" onfocusout="gst(this)" 
                                        onblur="extractNumber(this,5,false);remove_zero_after_decimal_point(this.value,'txtbillamount');"
                                        onkeyup="extractNumber(this,5,false);return Clear_Dot(this);"
                                        onkeypress="return blockNonNumbers(this, event, true, false);" Text='<%# Eval("Credit","{0:0.00}") %>' Width="85px" ></asp:TextBox>
                                        <%--<asp:TextBox ID="txtgCredit1" runat="server"  
                            style="text-align:right;"  
                                        CssClass="validate[custom[number]] creditgst" onfocusout="gst(this)"
                                        onblur="extractNumber(this,5,false);remove_zero_after_decimal_point(this.value,'txtbillamount');"
                                        onkeyup="extractNumber(this,5,false);return Clear_Dot(this);"
                                        onkeypress="return blockNonNumbers(this, event, true, false);" Text='<%# Eval("Credit","{0:0.00}") %>' Width="85px"></asp:TextBox>--%>
                        </ItemTemplate>
                         <ItemStyle Width="60px" />
                         <FooterTemplate>
                             <asp:TextBox ID="txtcredittotal" runat="server" Enabled="false"  Width="85px" ></asp:TextBox>
                         </FooterTemplate>
                    </asp:TemplateField>
                   
                    <asp:TemplateField HeaderText="Debit">
                        <ItemTemplate>
                            <asp:TextBox ID="txtgDebit1" runat="server"  style="text-align:right;"  onblur="Calculation(this)" 
                                        CssClass="validate[custom[number]] debitgst" 
                                        
                                        onkeyup="extractNumber(this,5,false);return Clear_Dot(this);"
                                        onkeypress="return blockNonNumbers(this, event, true, false);" Text='<%#Eval("Debit","{0:0.00}") %>' Width="85px" ></asp:TextBox>

                                        <%--<asp:TextBox ID="txtgDebit1" runat="server"  style="text-align:right;"  onfocusout="gst(this)"
                                        CssClass="validate[custom[number]] debitgst" 
                                        onblur="extractNumber(this,5,false);remove_zero_after_decimal_point(this.value,'txtbillamount');"
                                        onkeyup="extractNumber(this,5,false);return Clear_Dot(this);"
                                        onkeypress="return blockNonNumbers(this, event, true, false);" Text='<%#Eval("Debit","{0:0.00}") %>' Width="85px"></asp:TextBox>--%>
                        </ItemTemplate>
                         <ItemStyle Width="60px" />
                         <FooterTemplate>
                             <asp:TextBox ID="txtdebittotal" runat="server" Enabled="false" Width="85px" ></asp:TextBox>
                         </FooterTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="CGST">
                        <ItemTemplate>
                            <asp:TextBox ID="txtgCgst" runat="server" Enabled="true"  style="text-align:right;"    
                                        CssClass="validate[custom[number]] cgst"
                                        onblur="extractNumber(this,5,false);remove_zero_after_decimal_point(this.value,'txtbillamount');"
                                        onkeyup="extractNumber(this,5,false);return Clear_Dot(this);"
                                        onkeypress="return blockNonNumbers(this, event, true, false);" Text='<%#Eval("CGST","{0:0.00}") %>' Width="60px" ></asp:TextBox>
                        </ItemTemplate>
                         <ItemStyle Width="60px" />
                         <FooterTemplate>
                             <asp:TextBox ID="txtcgsttotal" runat="server" Width="60px"  Enabled="false" ></asp:TextBox>
                         </FooterTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="SGST">
                        <ItemTemplate>
                            <asp:TextBox ID="txtgScgst" runat="server" Enabled="true"  style="text-align:right;"  
                                        CssClass="validate[custom[number]] sgst"
                                        onblur="extractNumber(this,5,false);remove_zero_after_decimal_point(this.value,'txtbillamount');"
                                        onkeyup="extractNumber(this,5,false);return Clear_Dot(this);"
                                        onkeypress="return blockNonNumbers(this, event, true, false);" Text='<%#Eval("SCGST","{0:0.00}") %>' Width="60px" ></asp:TextBox>
                        </ItemTemplate>
                         <ItemStyle Width="60px" />
                         <FooterTemplate>
                             <asp:TextBox ID="txtsgsttotal" runat="server" Width="60px"  Enabled="false" ></asp:TextBox>
                         </FooterTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="IGST">
                        <ItemTemplate>
                            <asp:TextBox ID="txtgIgst" runat="server" Enabled="true"
                            style="text-align:right;"  
                                        CssClass="validate[custom[number]] igst"
                                        onblur="extractNumber(this,5,false);remove_zero_after_decimal_point(this.value,'txtbillamount');"
                                        onkeyup="extractNumber(this,5,false);return Clear_Dot(this);"
                                        onkeypress="return blockNonNumbers(this, event, true, false);" Text='<%#Eval("IGST","{0:0.00}") %>' Width="60px" ></asp:TextBox>
                        </ItemTemplate>
                         <ItemStyle Width="60px" />
                         <FooterTemplate>
                             <asp:TextBox ID="txtigsttotal" runat="server" Width="60px"  Enabled="false" TabIndex="38"></asp:TextBox>
                         </FooterTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="TDS">
                        <ItemTemplate>
                            <asp:TextBox ID="txtgTds" runat="server" Enabled="true" onfocusout="Calculationtds(this)" style="text-align:right;"  
                                        CssClass="validate[custom[number]] "
                                        onblur="extractNumber(this,5,false);remove_zero_after_decimal_point(this.value,'txtbillamount');"
                                        onkeyup="extractNumber(this,5,false);return Clear_Dot(this);"
                                        onkeypress="return blockNonNumbers(this, event, true, false);" Text='<%#Eval("TDS","{0:0.00}") %>' Width="60px" ></asp:TextBox>
                        </ItemTemplate>
                         <ItemStyle Width="60px" />
                         <FooterTemplate>
                             <asp:TextBox ID="TextBox1" runat="server" Width="60px" Enabled="false" ></asp:TextBox>
                         </FooterTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="TDS_Amount">
                        <ItemTemplate>
                            <asp:TextBox ID="txtgdtsamt" runat="server" Enabled="true" onfocusout="Calculationtdsamt(this)"  style="text-align:right;"  
                                        CssClass="validate[custom[number]] tdsamt" 
                                        onblur="extractNumber(this,5,false);remove_zero_after_decimal_point(this.value,'txtbillamount');"
                                        onkeyup="extractNumber(this,5,false);return Clear_Dot(this);"
                                        onkeypress="return blockNonNumbers(this, event, true, false);" Text='<%#Eval("TDS_Amount","{0:0.00}") %>' Width="85px" ></asp:TextBox>
                        </ItemTemplate>
                         <ItemStyle Width="60px" />
                         <FooterTemplate>
                             <asp:TextBox ID="txttdstotal" runat="server" Width="85px" Enabled="false"></asp:TextBox>
                              <asp:Button ID="Button2" runat="server" Width="40px"  CommandName="add"  Font-Size="Small"
                               CssClass="coma" Visible="true" Text="Add"   OnClientClick="jQuery('#form1').validationEngine('hideAll');"  /> 
                         </FooterTemplate>
                        
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Remarks">
                        <ItemTemplate>
                            <asp:TextBox ID="txtRemarks" runat="server" Enabled="true"  style="text-align:right;"  
                                       Text='<%#Eval("Remarks") %>' Width="155px" ></asp:TextBox>
                        </ItemTemplate>
                         <ItemStyle Width="60px" />
                         
                        
                    </asp:TemplateField>
                    <asp:TemplateField  HeaderText="FileUp" Visible="false">
                     <ItemTemplate>
                         <asp:FileUpload ID="FileUpload1"  runat="server" />
                         <asp:HiddenField ID="hfFileByte" runat="server" />
                         <asp:LinkButton ID="LinkButton1" OnClick="DownloadFile" CommandArgument='<%# Eval("SNo") %>' text='<%# Eval("FILE_NAME") %>' runat="server" ></asp:LinkButton>
                       
                                            </ItemTemplate>                                   
                                            
                                            <FooterTemplate>
                            
                        </FooterTemplate>

                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Cost Sheet"  >
                                                <ItemTemplate>
                                                <%--<asp:Button ID="btn_DocsUpload" runat="server" Text="Cost"  OnClientClick="upload(this);"
                                  Height="20px" Width="80px" 
                                   />--%>
                                                   <asp:Button ID="btn_Cost" runat="server" Text="Cost"  OnClientClick="Cost_Sheet(this);"
                                  Height="20px" Width="80px" 
                                   />   
                                                </ItemTemplate>
                                                <ItemStyle Width="20px" HorizontalAlign="center" Font-Size="12px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField>
                                            <ItemTemplate>
                                        <asp:ImageButton ID="imgRemove" runat="server" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" CausesValidation="false"  OnCommand="gvdetails_RowDeleting"  ImageUrl="../images/icons/delete-sign.png" Width="15px"  />
                                               
                                                
                                            </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:CommandField DeleteImageUrl="../js/delete-sign.png" />
                                        
                                        </Columns>
                                        <PagerSettings FirstPageText="First" LastPageText="Last" Mode="NextPreviousFirstLast"
                                                                NextPageText="Next" Position="Top" PreviousPageText="Previous" />
                                        <PagerStyle CssClass="pager" />
                                    <EmptyDataTemplate>
                                        NO RECORD FOUND
                                    </EmptyDataTemplate>
                                        </asp:GridView>
                    
                                    <style type="text/css">
                                        input.underlined
                                        {
                                            border: solid 1px #000;
                                        }
                                    </style>
                                    
                                    <%--------------------------------------Row End-----------------------------------------%>
                                </div>
                            </div>

                         <div style="width: 1400px; height: 30px">
<div id="tag_label_transact_Src" style="width: 90px;">
                                       Grand Total
                                    </div>
                                    <div id="txtcon-m_Exchange" style="width: 110px;">
                                        <asp:TextBox ID="txtGrandtotal"  runat="server" Enabled="true"   TabIndex="45"
                                         MaxLength="50" CssClass=" txtbox_none_Mid_transac_code" ></asp:TextBox>
                                    </div>
                                    <div id="tag_label_transact_Src" style="width: 90px;">
                                       Net Amount
                                    </div>
                                    <div id="txtcon-m_Exchange" style="width: 110px;">
                                        <asp:TextBox ID="txtNetamt"  runat="server" Enabled="true"   TabIndex="46"
                                         MaxLength="50" CssClass=" txtbox_none_Mid_transac_code" ></asp:TextBox>
                                    </div>
                                    </div>
                                    
                                    <div style="width: 1400px; height: 30px">
<div id="tag_label_transact_Src" style="width: 90px;">
                                     Approved By
                                    </div>
                                    <div id="txtcon-m_Exchange" style="width: 110px;">
                                        <asp:DropDownList ID="ddl_Approved" CssClass="listtxt_transac_item_gen_notn" Width="150px" 
                                    
                                            runat="server" TabIndex="47"  >
                                        </asp:DropDownList>
                                    </div>
                                    
                                    
                                    </div>
                <%-------------------------------------------------------------------------------%>
            
             <%-- --------------------Test Start----------------------------%>
            <div id="innerbox_MidMain_bot_transact" runat="server" style="height: 20px;">
                <div id="innerbox_transac_bot_inn" >
                    <div id="newbu">
                              <asp:Button ID="btnUpdate" runat="server" OnClick="btnUpdate_Click" CssClass="updates" 
                             TabIndex="48" />
                    </div>
                    <div id="editbu">
                        <asp:Button ID="btnSave" runat="server" CssClass="save"  OnClick="btnSave_Click" OnClientClick="return validate_Exp_V2();"
                            TabIndex="49"  CommandName="s" />
                    </div>
                        <div id="editbu">
                    <asp:Button ID="btnDelete" runat="server" TabIndex="93" CausesValidation="false" 
                                        UseSubmitBehavior="false" CssClass="dlete" OnClientClick="jQuery('#form1').validationEngine('hideAll');jConfirm('Delete this Job?', 'GENERAL', function(r) {
                  var i = r + 'ok';
          if(i == 'trueok')
          {
          
              document.getElementById('btn').click();
            
          }
          else {
          }
    
});return false;" />
<asp:Button ID="btn" runat="server" TabIndex="50" OnClick="btnDelete_Click" CausesValidation="false"
                                        UseSubmitBehavior="false" OnClientClick="jQuery('#form1').validationEngine('hideAll');"
                                        CssClass="dlete" Style="display: none;" />
     </div>
                   <%-- <div id="editbu">
                       <asp:Button ID="btnprint" runat="server" Height="30px" Text="Export" TabIndex="51" OnClientClick="open_jobno(); return false"  />

                    </div>--%>
                    
                    <div id="editbu">
                        <input type="submit" value="Submit" class="scancel" onclick="RefreshParent();return false;"
                            id="btnCancel" tabindex="52" Style="display: none;">
                    </div>
                   
                    <div id="editbu" style="width: 10px;">
                    </div>

                    
                </div>
            </div>
            <%-- --------------------Test End----------------------------%>
            
            <script src="../activatables.js" type="text/javascript"></script>
            <script type="text/javascript">
                activatables('page', ['page-6', 'page-7', 'page-8']);
            </script>
            <asp:HiddenField ID="HDupdate_id" runat="server" />
            <asp:HiddenField ID="Hdnfileref" runat="server" />
    <asp:HiddenField ID="Hdncgst" runat="server" />
    <asp:HiddenField ID="Hdnsgst" runat="server" />
    <asp:HiddenField ID="Hdnigst" runat="server" />
    <asp:HiddenField ID="hdntaxmode" runat="server" />
    <asp:HiddenField ID="hdntds" runat="server" />
    <asp:HiddenField ID="Hd_row_id" runat="server" />
       <asp:HiddenField ID="hdprefix" runat="server" />
                <asp:HiddenField ID="hdsuffix" runat="server" />
                  <asp:HiddenField ID="Hidden_VOUCHER_ID" runat="server" />
                    <asp:HiddenField ID="Hidden_PURCHASE_DEBIT_NO" runat="server" />
                   <asp:HiddenField ID="Hidden_Voucher_date" runat="server" />
        <%--</ContentTemplate>
    </asp:UpdatePanel>--%>
    </form>
    <script src="../js/Billing/GST/Imp_auto_Search.js" type="text/javascript"></script>
    <script type="text/javascript" src="../windowfiles/dhtmlwindow.js"></script>
    <script type="text/javascript" src="../modalfiles/modal.js"></script>
    <script type="text/javascript" src="../js/slide.js"></script>
    <script type="text/javascript" src="../js/jscolor.js"></script>
    <script type="text/javascript" src="../js/iframepopupwin.js"></script>
    <script src="../js/checkboxJScript.js" type="text/javascript"></script>
    <script src="../js/listpopup.js" type="text/javascript"></script>
    <script src="../js/Validation.js" type="text/javascript"></script>
    <script src="../js/Export_Jscript/Export_Invoice.js" type="text/javascript"></script>
    <script src="../AutoComplete_JS/jquery.min.js" type="text/javascript"></script>
    <script src="../AutoComplete_JS/jquery-ui.min.js" type="text/javascript"></script>
    <script src="../Validation Files/jquery_002.js" type="text/javascript" charset="utf-8">
    </script>
    <script src="../Validation Files/jquery.js" type="text/javascript" charset="utf-8">
    </script>
    <script src="../Validation%20Files/Ascii.js" type="text/javascript"></script>
      <!-- VALIDATION SCRIPT -->
     <link rel="Stylesheet" href="../Dropdown/chosen.css" />
    <link rel="Stylesheet" href="../Dropdown/chosen.min.css" />
    <script src="../js/MaskedEditFix.js" type="text/javascript"></script>
     <script src="../Dropdown/chosen.jquery.js" type="text/javascript"></script>
    <script src="../Dropdown/chosen.jquery.min.js" type="text/javascript"></script>
    <script src="../Dropdown/chosen.proto.js" type="text/javascript"></script>
    <script type="text/javascript">

        function open_jobno() {

            if (document.getElementById("<%=txtVoucherno.ClientID %>").value != '') {
                var f = document.getElementById("<%=txtVoucherno.ClientID %>").value;
                NewWindow1('../Account_Purches/Purchase_Entry_Report.aspx?voucherno=' + f + '', 'List', '870', '1024', 'yes');
                return false
            }

        }
        function open_file() {

            NewWindow1('../Billing_Imp/FileUpload.aspx', 'List', '570', '100', 'yes');

        }
     </script>
     
 <script type="text/javascript">
     $(function () {
         $("#fileupload1").change(function () {
             $("#spnName").html($("#fileupload1").val().substring($("#fileupload1").val().lastIndexOf('\\') + 1));
         });
     });
</script>
     <script src="../js/Billing/BillingValid.js" type="text/javascript"></script>
    <!-- VALIDATION SCRIPT -->
    <script type="text/javascript">
        jQuery(document).ready(function () {
            jQuery("#form1").validationEngine();

            //            $("#txtbillamount, #txtInvoiceCurrency").change(function () {
            //                $("#txtTotalamtininr").val('');
            //                var $input = $(this);
            //                if ($input.is('#txtInvoiceCurrency')) {
            //                    setTimeout(
            //                      function () {
            //                          var amt = $("#txtbillamount").val();
            //                          var exrate = $("#txtExrate").val();
            //                          if (amt != '' && amt != 'undefined' && exrate != '' && exrate != 'undefined') {
            //                              $("#txtTotalamtininr").val(amt * exrate);
            //                          }
            //                      }, 1000);
            //                }
            //                else {
            //                    var amt = $("#txtbillamount").val();
            //                    var exrate = $("#txtExrate").val();
            //                    if (amt != '' && amt != 'undefined' && exrate != '' && exrate != 'undefined') {
            //                        $("#txtTotalamtininr").val(amt * exrate);
            //                    }
            //                }
            //            });
        });


        function close_button() {
            document.getElementById("General").click();
        }
    </script>
    

    <script>
        $(function () {
            $('#txtCurrency').autocomplete({
                source: function (request, response) {
                    //                    var field = $(this.element).attr('id');

                    $.ajax({
                        url: "../AutoComplete_Pages/Auto_Complete_Searching.asmx/Exchage_Master_List",
                        data: "{ 'mail': '" + request.term + "' }",
                        dataType: "json",
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        dataFilter: function (data) { return data; },
                        async: true,
                        success: function (data) {
                            response(data.d);
                            if (data.d == '') {

                                jQuery('#txtCurrency').validationEngine('showPrompt', 'Incorrect Currency', 'error', 'topRight', true);
                                //                                $('#txtCurrency').val('');

                                //jAlert('Invoice Currency is Incorrect', 'INVOICE', function (r) { document.getElementById(field).focus(); });
                                //return false;


                            }
                            else {

                                jQuery('#' + field).validationEngine('hidePrompt', '', 'error', 'topRight', true);


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

        $(function () {
            $('#txtVendorname').autocomplete({
                source: function (request, response) {
                    //                    var field = $(this.element).attr('id');


                    $.ajax({
                        url: "../AutoComplete_Pages/Auto_Complete_Searching.asmx/Vendor_Name_List",
                        data: "{ 'mail': '" + request.term + "' }",
                        dataType: "json",
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        dataFilter: function (data) { return data; },
                        async: true,

                        success: function (data) {
                            response(data.d);
                            if (data.d == '') {

                                //                                jQuery('#txtVendorname').validationEngine('showPrompt', 'Incorrect Vendor Name', 'error', 'topRight', true);

                                $('#txtVendorname').val('');

                                //jAlert('Invoice Currency is Incorrect', 'INVOICE', function (r) { document.getElementById(field).focus(); });
                                //return false;


                            }
                            else {

                                jQuery('#' + field).validationEngine('hidePrompt', '', 'error', 'topRight', true);


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

        $(function () {
            $('#txtVendorbranch').autocomplete({
                source: function (request, response) {
                    //                    var field = $(this.element).attr('id');
                    var venname = document.getElementById("txtVendorname");
                    $.ajax({
                        url: "../AutoComplete_Pages/Auto_Complete_Searching.asmx/Vendor_Branch_List",
                        data: "{ 'mail': '" + request.term + "' ,'code':'" + venname.value + "'}",
                        dataType: "json",
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        dataFilter: function (data) { return data; },
                        async: true,
                        success: function (data) {
                            response(data.d);
                            if (data.d == '') {

                                jQuery('#txtVendorbranch').validationEngine('showPrompt', 'Incorrect Branch', 'error', 'topRight', true);
                                $('#txtVendorbranch').val('');
                                //jAlert('Invoice Currency is Incorrect', 'INVOICE', function (r) { document.getElementById(field).focus(); });
                                //return false;


                            }
                            else {

                                jQuery('#' + field).validationEngine('hidePrompt', '', 'error', 'topRight', true);


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

        $(function () {

            $(".Search").autocomplete({
                source: function (request, response) {
                    //                     var field = $(this.element).attr('id');

                    $.ajax({
                        url: "../AutoComplete_Pages/Auto_Complete_Searching.asmx/Charge_Master_List",
                        data: "{ 'mail': '" + request.term + "' }",
                        dataType: "json",
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        dataFilter: function (data) { return data; },
                        async: true,
                        success: function (data) {
                            response($.map(data.d, function (item) {
                                return {
                                    label: item,
                                    val: item
                                }
                            }))
                        },
                        error: function (response) {
                            alert(response.responseText);
                        },
                        failure: function (response) {
                            alert(response.responseText);

                        }
                    });
                },
                minLength: 1

            });
            return false;
            scroll: true;

        });
        $(function () {

            $(".Fileref").autocomplete({
                source: function (request, response) {
                    //                     var field = $(this.element).attr('id');

                    $.ajax({
                        url: "../AutoComplete_Pages/Auto_Complete_Searching.asmx/Job_No",
                        data: "{ 'mail': '" + request.term + "' }",
                        dataType: "json",
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        dataFilter: function (data) { return data; },
                        async: true,
                        success: function (data) {
                            response($.map(data.d, function (item) {
                                return {
                                    label: item,
                                    val: item
                                }
                            }))
                        },
                        error: function (response) {
                            alert(response.responseText);
                        },
                        failure: function (response) {
                            alert(response.responseText);
                        }
                    });
                },
                minLength: 1
            });
            return false;
            scroll: true;

        });



        //        $(function () {
        //            var availableTags = [
        //      "Royal Tech Systems",

        //    ];
        //            var availableTagsfile = [
        //      "FILE/2020/12",

        //    ];
        //            var availableTagsbr = [
        //      "Chennai",

        //    ];
        //            $("#txtVendorname").autocomplete({
        //                source: availableTags
        //            });
        //            $(".Fileref").autocomplete({
        //                source: availableTagsfile
        //            });
        //            $("#txtVendorbranch").autocomplete({
        //                source: availableTagsbr
        //            });
        //        });


    </script>
   
     <script type="text/javascript">
         function Excel_download() {
             var txtvoucherno = $("[id*=txtVoucherno]")
             var Type = 'excel';
             win = window.open('../FlatFile/MIS_report_1.aspx?Excel_Download_XLs=Yes&voucherno=' + txtvoucherno + '', 'ndf');
             Loading_HideImage();
         }
         function Calculation(lnk) {
             //            $(function () {
             //             $("[id*=gvdetails]").find("[id*=txtgCharHead]").blur(function () {

             //            //Reference the GridView Row.
             //            var row = $(this).closest("tr");
             // 
             //            var a= row.find("td").eq(6).html();
             
             var grid = document.getElementById("<%= gvdetails.ClientID%>");
             var txtAmountReceive = $(".Search")
             //             var txtAmountReceive = $("[id*=txtgCharHead]")
             //             alert(txtAmountReceive);
             //             var txtAmountReceive_2 = $("[id*=ddl_tax_nontax]")
             var row = lnk.parentNode.parentNode;

             var rowIndex = row.rowIndex - 1;
             //             var a = "AAI EXPENSES";
             //             alert(rowIndex);
             //             var a = "gvdetails_" + "txtgCharHead_" + rowIndex.value;
             var a = txtAmountReceive[rowIndex].value;
             //             alert(a);
             var b = '1';

             $('#Hd_row_id').val(rowIndex);

             Cha_Type(a, b);
             //             Tax_Cal();
         }  
          </script>
    <script type="text/javascript">

        function Cha_Type(src, dest) {
            PageMethods.Get_Cha_rate(src, '', '', CallSuccess_Cha_Type, CallFailed, dest);
        }
        function CallSuccess_Cha_Type(res, destCtrl) {

            var data = res;

            if (res != '') {

                //                var selectedindex = $('#Hd_row_id').val;
                //                alert(selectedindex);
                //                var chargetype = $("[id*=ddl_tax_nontax]")
                //                var txt_HSN_Code = $("[id*=txt_HSN_Code]")
                //                var txt_SA_Code = $("[id*=txt_SA_Code]") //****New Implementation by Elaiyaa on 26Jun2019*********//

                var selected = $('input:radio[name=Rd_Bill_Type]:checked').val();
                var gstype = $("[id*=ddl_Gst_type]");
                //                alert('hi');
                var txtCGST = $("[id*=txtgCgst]")
                var txtTaxrate = $("[id*=txtgTaxrate]")
                var txtSGST = $("[id*=txtgScgst]")
                var txtdts = $("[id*=txtgTds]")
                var txtIGST = $("[id*=txtgIgst]")
                var txtsaccode = $("[id*=ddl_Gst_type]")
                //                $(".cgst").val(before);

                //                alert(document.getElementById("Hd_row_id").value);

                //                var Hd_Company_Id = document.getElementById("Hd_Company_Id").value;

                if (data.indexOf("~~") != -1 && data.indexOf("~~") != '') {

                    var s = data.split('~~');

                    if (gstype[document.getElementById("Hd_row_id").value].value == 'ITC') {

                        var cgst = s[0];
                        var sgst = s[1];
                        var igst = s[2]; //****New Implementation by Elaiyaa on 26Jun2019*********//
                        //                    var tds = s[3];
                        var saccode = s[4];   //line 6.1
                    }
                    else {

                        var cgst = '0.00';
                        var sgst = '0.00';
                        var igst = '0.00'; //****New Implementation by Elaiyaa on 26Jun2019*********//
                        //                    var tds = s[3];
                        var saccode = '0.00';   //line 6.1
                    }
                    var tax_mode = s[4];
                    //line 6.1
                    //                    var tempSGST_RATE = s[4];     //line 6.2
                    //                    var tempIGST_RATE = s[5];     //line 6.3
                    //                    alert(before
                    //                    $(".cgst").val(before);

                    //                    alert(txttds[document.getElementById("Hd_row_id").value]);
                    //                    txtsaccode[document.getElementById("Hd_row_id").value].value = saccode;

                    var state = $('input[name="Rd_Bill_Type"]:checked').val();

                    if (state == 'L') {

                        if (tax_mode = 'T' && cgst != ' ') {
                            //                            txtTaxrate[document.getElementById("Hd_row_id").value].value = parseFloat(cgst) + parseFloat(sgst);
                            txtTaxrate[document.getElementById("Hd_row_id").value].value = parseFloat(cgst) + parseFloat(sgst);
                        }
                        //                        else { txtTaxrate[document.getElementById("Hd_row_id").value].value = parseFloat(igst); }
                        else if (tax_mode = 'T' && cgst == ' ') { txtTaxrate[document.getElementById("Hd_row_id").value].value = parseFloat(igst).toFixed(2); }
                        else { txtTaxrate[document.getElementById("Hd_row_id").value].value = "0.00"; }
                    }
                    else {
                        if (igst != ' ') {
                            //                        txtTaxrate[document.getElementById("Hd_row_id").value].value = parseFloat(igst);
                            txtTaxrate[document.getElementById("Hd_row_id").value].value = parseFloat(igst).toFixed(2);
                        }
                        else { txtTaxrate[document.getElementById("Hd_row_id").value].value = "0.00"; }
                    }
                    $("[id*=txtgTaxrate]").attr('readonly', 'readonly');
                    //                    txtCGST[document.getElementById("Hd_row_id").value].value = cgst;
                    //                    txtSGST[document.getElementById("Hd_row_id").value].value = sgst;
                    //                    
                    //                    txtIGST[document.getElementById("Hd_row_id").value].value = igst;
                    //                    if (state == 'L') {
                    //                        if (tax_mode = 'T' && cgst != ' ' ) {
                    //                            txtCGST[document.getElementById("Hd_row_id").value].value = cgst;
                    //                            txtSGST[document.getElementById("Hd_row_id").value].value = sgst;
                    //                            txtIGST[document.getElementById("Hd_row_id").value].value = 0;
                    //                        }
                    //                        else {
                    //                            txtCGST[document.getElementById("Hd_row_id").value].value = 0;
                    //                            txtSGST[document.getElementById("Hd_row_id").value].value = 0;
                    //                            txtIGST[document.getElementById("Hd_row_id").value].value = igst;
                    //                        }

                    //                    }
                    //                    else {
                    //                        txtCGST[document.getElementById("Hd_row_id").value].value = 0;
                    //                        txtSGST[document.getElementById("Hd_row_id").value].value = 0;
                    //                        txtIGST[document.getElementById("Hd_row_id").value].value = igst;
                    //                    }
                    //                    txtdts[document.getElementById("Hd_row_id").value].value = '0';
                    $("[id*=Hdncgst]").val(cgst);
                    $("[id*=Hdnsgst]").val(sgst);
                    $("[id*=Hdnigst]").val(igst);
                    //                    $("[id*=hdntds]").val(tds);
                    $("[id*=hdntaxmode]").val(tax_mode);

                    //                     $("[id*=txtgCgst]").val(cgst);
                    //                     $("[id*=txtgScgst]").val(sgst);
                    //                     $("[id*=txtgIgst]").val(igst);
                    //                     $("[id*=txtgTds]").val(tds);
                    var rowIndex = document.getElementById("Hd_row_id").value;

                    if ($(".debitgst").val() != null && $(".debitgst").val() != '') {
                        var txtAmountReceive = $(".debitgst")


                    }
                    else if ($(".creditgst").val() != null && $(".creditgst").val() != '') {
                        var txtAmountReceive = $(".creditgst")

                    }

                    //                    var row = lnk.parentNode.parentNode;
                    //                    var rowIndex = row.rowIndex - 1;
                    var a = txtAmountReceive[rowIndex].value;

                    //                alert(a);
                    if (a != null && a != '') {

                        var cgst = MathRound(a * (document.getElementById("Hdncgst").value / 100))

                        var sgst = MathRound(a * (document.getElementById("Hdnsgst").value / 100))


                        var igst = MathRound(a * (document.getElementById("Hdnigst").value / 100))



                        //                    var tds = document.getElementById("hdntds").value;
                        var tds = $("[id*=txtgTds]");

                        var tdsamt = MathRound(a * (tds[document.getElementById("Hd_row_id").value].value / 100));

                        //                var txtcgst = $("[id*=");
                        //                var txtcgst = $("[id*=");
                        //                var txtigst = $("[id*=");
                        //                var txtcgst = $("[id*=");

                        var txtCgst = $("[id*=txtgCgst]")
                        var txtSgst = $("[id*=txtgScgst]")
                        var txtIgst = $("[id*=txtgIgst]")
                        var txttds = $("[id*=txtgTds]")
                        var txtdtsamt = $("[id*=txtgdtsamt]")
                        var state = $('input[name="Rd_Bill_Type"]:checked').val();
                        var tax_mode = document.getElementById("hdntaxmode").value;

                        if (state == 'L') {
                            if (tax_mode = 'T' && cgst != '0') {
                                txtCgst[document.getElementById("Hd_row_id").value].value = cgst;
                                txtSgst[document.getElementById("Hd_row_id").value].value = sgst
                                txtIgst[document.getElementById("Hd_row_id").value].value = 0
                            }
                            else {
                                txtCgst[document.getElementById("Hd_row_id").value].value = 0;
                                txtSgst[document.getElementById("Hd_row_id").value].value = 0
                                txtIgst[document.getElementById("Hd_row_id").value].value = igst
                            }

                        }
                        else {
                            txtCgst[document.getElementById("Hd_row_id").value].value = 0;
                            txtSgst[document.getElementById("Hd_row_id").value].value = 0
                            txtIgst[document.getElementById("Hd_row_id").value].value = igst
                        }
                        txttds[document.getElementById("Hd_row_id").value].value = tds[document.getElementById("Hd_row_id").value].value;
                        txtdtsamt[document.getElementById("Hd_row_id").value].value = tdsamt;
                        calculate()


                        var totalcredit = $("[id*=txtcredittotal").val()
                        var totaldebit = $("[id*=txtdebittotal]").val()
                        var totalcgst = $("[id*=txtcgsttotal]").val()
                        var totalsgst = $("[id*=txtsgsttotal]").val()
                        var totaligst = $("[id*=txtigsttotal]").val()
                        var totaltds = $("[id*=txttdstotal]").val()
                        var grandtotal, Nettotal
                        var curr = document.getElementById("txtInvoiceCurrency").value;
                        if (totalcredit != null && totaldebit != null && totalcredit != '' && totaldebit != '') {


                            if (totalcgst == 'NaN') { totalcgst = 0 }
                            if (totalsgst == 'NaN') { totalsgst = 0 }
                            if (totaligst == 'NaN') { totaligst = 0 }
                            if (totaltds == 'NaN') { totaltds = 0 }
                            if (curr == 'INR') {
                                //                            grandtotal = Math.round(parseFloat(totalcredit) + parseFloat(totaldebit) + parseFloat(totalcgst) + parseFloat(totalsgst) + parseFloat(totaligst))
                                //                            Nettotal = Math.round(parseFloat(totalcredit) - parseFloat(totaldebit) + parseFloat(totalcgst) + parseFloat(totalsgst) + parseFloat(totaligst) - parseFloat(totaltds))
                                grandtotal = Math.round((parseFloat(totaldebit) + parseFloat(totalcgst) + parseFloat(totalsgst) + parseFloat(totaligst)) - (parseFloat(totalcredit)))
                                Nettotal = Math.round((parseFloat(totaldebit) + parseFloat(totalcgst) + parseFloat(totalsgst) + parseFloat(totaligst)) - (parseFloat(totaltds) + parseFloat(totalcredit)))
                            }
                            else {
                                //                            grandtotal = (parseFloat(totalcredit) + parseFloat(totaldebit) + parseFloat(totalcgst) + parseFloat(totalsgst) + parseFloat(totaligst)).toFixed(2)
                                //                            Nettotal = (parseFloat(totalcredit) - parseFloat(totaldebit) + parseFloat(totalcgst) + parseFloat(totalsgst) + parseFloat(totaligst) - parseFloat(totaltds)).toFixed(2)
                                grandtotal = (parseFloat(totaldebit) + parseFloat(totalcgst) + parseFloat(totalsgst) + parseFloat(totaligst)) - (parseFloat(totalcredit))
                                Nettotal = (parseFloat(totaldebit) + parseFloat(totalcgst) + parseFloat(totalsgst) + parseFloat(totaligst)) - (parseFloat(totaltds) + parseFloat(totalcredit))
                            }
                        }
                        else if (totalcredit != null && totalcredit != '') {
                            if (curr == 'INR') {
                                //                            grandtotal = Math.round(parseFloat(totalcredit) + parseFloat(totalcgst) + parseFloat(totalsgst) + parseFloat(totaligst))
                                //                            Nettotal = Math.round(parseFloat(totalcredit) + parseFloat(totalcgst) + parseFloat(totalsgst) + parseFloat(totaligst) - parseFloat(totaltds))
                                grandtotal = Math.round((parseFloat(totaldebit) + parseFloat(totalcgst) + parseFloat(totalsgst) + parseFloat(totaligst)) - (parseFloat(totalcredit)))
                                Nettotal = Math.round((parseFloat(totaldebit) + parseFloat(totalcgst) + parseFloat(totalsgst) + parseFloat(totaligst)) - (parseFloat(totaltds) + parseFloat(totalcredit)))
                            }
                            else {
                                //                            grandtotal = (parseFloat(totalcredit) + parseFloat(totalcgst) + parseFloat(totalsgst) + parseFloat(totaligst)).toFixed(2)
                                //                            Nettotal = (parseFloat(totalcredit) + parseFloat(totalcgst) + parseFloat(totalsgst) + parseFloat(totaligst) - parseFloat(totaltds)).toFixed(2)
                                grandtotal = (parseFloat(totaldebit) + parseFloat(totalcgst) + parseFloat(totalsgst) + parseFloat(totaligst)) - (parseFloat(totalcredit))
                                Nettotal = (parseFloat(totaldebit) + parseFloat(totalcgst) + parseFloat(totalsgst) + parseFloat(totaligst)) - (parseFloat(totaltds) + parseFloat(totalcredit))
                            }
                        }
                        else if (totaldebit != null && totaldebit != '') {
                            if (curr == 'INR') {
                                //                            grandtotal = Math.round(parseFloat(totaldebit) + parseFloat(totalcgst) + parseFloat(totalsgst) + parseFloat(totaligst))
                                //                            Nettotal = Math.round(parseFloat(totaldebit) + parseFloat(totalcgst) + parseFloat(totalsgst) + parseFloat(totaligst) - parseFloat(totaltds))
                                grandtotal = Math.round((parseFloat(totaldebit) + parseFloat(totalcgst) + parseFloat(totalsgst) + parseFloat(totaligst)) - (parseFloat(totalcredit)))
                                Nettotal = Math.round((parseFloat(totaldebit) + parseFloat(totalcgst) + parseFloat(totalsgst) + parseFloat(totaligst)) - (parseFloat(totaltds) + parseFloat(totalcredit)))
                            }
                            else {
                                //                            grandtotal = (parseFloat(totaldebit) + parseFloat(totalcgst) + parseFloat(totalsgst) + parseFloat(totaligst)).toFixed(2)
                                //                            Nettotal = (parseFloat(totaldebit) + parseFloat(totalcgst) + parseFloat(totalsgst) + parseFloat(totaligst) - parseFloat(totaltds)).toFixed(2)
                                grandtotal = (parseFloat(totaldebit) + parseFloat(totalcgst) + parseFloat(totalsgst) + parseFloat(totaligst)) - (parseFloat(totalcredit))
                                Nettotal = (parseFloat(totaldebit) + parseFloat(totalcgst) + parseFloat(totalsgst) + parseFloat(totaligst)) - (parseFloat(totaltds) + parseFloat(totalcredit))
                            }

                        }
                        //                alert(totaldebit)

                        $("#txtGrandtotal").val(grandtotal)

                        //                    var Nettotal = parseFloat(totaldebit) + parseFloat(totalcgst) + parseFloat(totalsgst) + parseFloat(totaligst) - parseFloat(totaltds)

                        $("#txtNetamt").val(Nettotal)

                        //                    $("[id*=txtgCgst]").attr('readonly', 'readonly');
                        //                    $("[id*=txtgScgst]").attr('readonly', 'readonly');
                        //                    $("[id*=txtgIgst]").attr('readonly', 'readonly');
                        //                    $("[id*=txtgTds]").attr('readonly', 'readonly');
                        //                    $("[id*=txtgdtsamt]").attr('readonly', 'readonly');
                    }

                }

            }
            else {

                var selected = $('input:radio[name=Rd_Bill_Type]:checked').val();
                var gstype = $("[id*=ddl_Gst_type]");
                //                alert('hi');
                var txtCGST = $("[id*=txtgCgst]")
                var txtTaxrate = $("[id*=txtgTaxrate]")
                var txtSGST = $("[id*=txtgScgst]")
                var txtdts = $("[id*=txtgTds]")
                var txtIGST = $("[id*=txtgIgst]")
                var txtsaccode = $("[id*=ddl_Gst_type]")

                var cgst = '0.00';
                var sgst = '0.00';
                var igst = '0.00'; //****New Implementation by Elaiyaa on 26Jun2019*********//
                //                    var tds = s[3];
                var saccode = '0.00';   //line 6.1
                var tax_mode = 'N';
                var state = $('input[name="Rd_Bill_Type"]:checked').val();
                txtTaxrate[document.getElementById("Hd_row_id").value].value = "0.00";
                $("[id*=txtgTaxrate]").attr('readonly', 'readonly');
                //                    txtCGST[document.getElementById("Hd_row_id").value].value = cgst;
                //                    txtSGST[document.getElementById("Hd_row_id").value].value = sgst;
                //                    
                //                    txtIGST[document.getElementById("Hd_row_id").value].value = igst;
                //                    if (state == 'L') {
                //                        if (tax_mode = 'T' && cgst != ' ' ) {
                //                            txtCGST[document.getElementById("Hd_row_id").value].value = cgst;
                //                            txtSGST[document.getElementById("Hd_row_id").value].value = sgst;
                //                            txtIGST[document.getElementById("Hd_row_id").value].value = 0;
                //                        }
                //                        else {
                //                            txtCGST[document.getElementById("Hd_row_id").value].value = 0;
                //                            txtSGST[document.getElementById("Hd_row_id").value].value = 0;
                //                            txtIGST[document.getElementById("Hd_row_id").value].value = igst;
                //                        }

                //                    }
                //                    else {
                //                        txtCGST[document.getElementById("Hd_row_id").value].value = 0;
                //                        txtSGST[document.getElementById("Hd_row_id").value].value = 0;
                //                        txtIGST[document.getElementById("Hd_row_id").value].value = igst;
                //                    }
                //                    txtdts[document.getElementById("Hd_row_id").value].value = '0';
                $("[id*=Hdncgst]").val(cgst);
                $("[id*=Hdnsgst]").val(sgst);
                $("[id*=Hdnigst]").val(igst);
                //                    $("[id*=hdntds]").val(tds);
                $("[id*=hdntaxmode]").val(tax_mode);

                //                     $("[id*=txtgCgst]").val(cgst);
                //                     $("[id*=txtgScgst]").val(sgst);
                //                     $("[id*=txtgIgst]").val(igst);
                //                     $("[id*=txtgTds]").val(tds);
                var rowIndex = document.getElementById("Hd_row_id").value;

                if ($(".debitgst").val() != null && $(".debitgst").val() != '') {
                    var txtAmountReceive = $(".debitgst")


                }
                else if ($(".creditgst").val() != null && $(".creditgst").val() != '') {
                    var txtAmountReceive = $(".creditgst")

                }

                //                    var row = lnk.parentNode.parentNode;
                //                    var rowIndex = row.rowIndex - 1;
                var a = txtAmountReceive[rowIndex].value;

                //                alert(a);
                if (a != null && a != '') {

                    var cgst = MathRound(a * (document.getElementById("Hdncgst").value / 100))

                    var sgst = MathRound(a * (document.getElementById("Hdnsgst").value / 100))


                    var igst = MathRound(a * (document.getElementById("Hdnigst").value / 100))



                    //                    var tds = document.getElementById("hdntds").value;
                    var tds = $("[id*=txtgTds]");

                    var tdsamt = MathRound(a * (tds[document.getElementById("Hd_row_id").value].value / 100));

                    //                var txtcgst = $("[id*=");
                    //                var txtcgst = $("[id*=");
                    //                var txtigst = $("[id*=");
                    //                var txtcgst = $("[id*=");

                    var txtCgst = $("[id*=txtgCgst]")
                    var txtSgst = $("[id*=txtgScgst]")
                    var txtIgst = $("[id*=txtgIgst]")
                    var txttds = $("[id*=txtgTds]")
                    var txtdtsamt = $("[id*=txtgdtsamt]")
                    var state = $('input[name="Rd_Bill_Type"]:checked').val();
                    var tax_mode = document.getElementById("hdntaxmode").value;

                    if (state == 'L') {
                        if (tax_mode = 'T' && cgst != '0') {
                            txtCgst[document.getElementById("Hd_row_id").value].value = cgst;
                            txtSgst[document.getElementById("Hd_row_id").value].value = sgst
                            txtIgst[document.getElementById("Hd_row_id").value].value = 0
                        }
                        else {
                            txtCgst[document.getElementById("Hd_row_id").value].value = 0;
                            txtSgst[document.getElementById("Hd_row_id").value].value = 0
                            txtIgst[document.getElementById("Hd_row_id").value].value = igst
                        }

                    }
                    else {
                        txtCgst[document.getElementById("Hd_row_id").value].value = 0;
                        txtSgst[document.getElementById("Hd_row_id").value].value = 0
                        txtIgst[document.getElementById("Hd_row_id").value].value = igst
                    }
                    txttds[document.getElementById("Hd_row_id").value].value = tds[document.getElementById("Hd_row_id").value].value;
                    txtdtsamt[document.getElementById("Hd_row_id").value].value = tdsamt;
                    calculate()


                    var totalcredit = $("[id*=txtcredittotal").val()
                    var totaldebit = $("[id*=txtdebittotal]").val()
                    var totalcgst = $("[id*=txtcgsttotal]").val()
                    var totalsgst = $("[id*=txtsgsttotal]").val()
                    var totaligst = $("[id*=txtigsttotal]").val()
                    var totaltds = $("[id*=txttdstotal]").val()
                    var grandtotal, Nettotal
                    var curr = document.getElementById("txtInvoiceCurrency").value;
                    if (totalcredit != null && totaldebit != null && totalcredit != '' && totaldebit != '') {


                        if (totalcgst == 'NaN') { totalcgst = 0 }
                        if (totalsgst == 'NaN') { totalsgst = 0 }
                        if (totaligst == 'NaN') { totaligst = 0 }
                        if (totaltds == 'NaN') { totaltds = 0 }
                        if (curr == 'INR') {
                            //                            grandtotal = Math.round(parseFloat(totalcredit) + parseFloat(totaldebit) + parseFloat(totalcgst) + parseFloat(totalsgst) + parseFloat(totaligst))
                            //                            Nettotal = Math.round(parseFloat(totalcredit) - parseFloat(totaldebit) + parseFloat(totalcgst) + parseFloat(totalsgst) + parseFloat(totaligst) - parseFloat(totaltds))
                            grandtotal = Math.round((parseFloat(totaldebit) + parseFloat(totalcgst) + parseFloat(totalsgst) + parseFloat(totaligst)) - (parseFloat(totalcredit)))
                            Nettotal = Math.round((parseFloat(totaldebit) + parseFloat(totalcgst) + parseFloat(totalsgst) + parseFloat(totaligst)) - (parseFloat(totaltds) + parseFloat(totalcredit)))
                        }
                        else {
                            //                            grandtotal = (parseFloat(totalcredit) + parseFloat(totaldebit) + parseFloat(totalcgst) + parseFloat(totalsgst) + parseFloat(totaligst)).toFixed(2)
                            //                            Nettotal = (parseFloat(totalcredit) - parseFloat(totaldebit) + parseFloat(totalcgst) + parseFloat(totalsgst) + parseFloat(totaligst) - parseFloat(totaltds)).toFixed(2)
                            grandtotal = (parseFloat(totaldebit) + parseFloat(totalcgst) + parseFloat(totalsgst) + parseFloat(totaligst)) - (parseFloat(totalcredit))
                            Nettotal = (parseFloat(totaldebit) + parseFloat(totalcgst) + parseFloat(totalsgst) + parseFloat(totaligst)) - (parseFloat(totaltds) + parseFloat(totalcredit))
                        }
                    }
                    else if (totalcredit != null && totalcredit != '') {
                        if (curr == 'INR') {
                            //                            grandtotal = Math.round(parseFloat(totalcredit) + parseFloat(totalcgst) + parseFloat(totalsgst) + parseFloat(totaligst))
                            //                            Nettotal = Math.round(parseFloat(totalcredit) + parseFloat(totalcgst) + parseFloat(totalsgst) + parseFloat(totaligst) - parseFloat(totaltds))
                            grandtotal = Math.round((parseFloat(totaldebit) + parseFloat(totalcgst) + parseFloat(totalsgst) + parseFloat(totaligst)) - (parseFloat(totalcredit)))
                            Nettotal = Math.round((parseFloat(totaldebit) + parseFloat(totalcgst) + parseFloat(totalsgst) + parseFloat(totaligst)) - (parseFloat(totaltds) + parseFloat(totalcredit)))
                        }
                        else {
                            //                            grandtotal = (parseFloat(totalcredit) + parseFloat(totalcgst) + parseFloat(totalsgst) + parseFloat(totaligst)).toFixed(2)
                            //                            Nettotal = (parseFloat(totalcredit) + parseFloat(totalcgst) + parseFloat(totalsgst) + parseFloat(totaligst) - parseFloat(totaltds)).toFixed(2)
                            grandtotal = (parseFloat(totaldebit) + parseFloat(totalcgst) + parseFloat(totalsgst) + parseFloat(totaligst)) - (parseFloat(totalcredit))
                            Nettotal = (parseFloat(totaldebit) + parseFloat(totalcgst) + parseFloat(totalsgst) + parseFloat(totaligst)) - (parseFloat(totaltds) + parseFloat(totalcredit))
                        }
                    }
                    else if (totaldebit != null && totaldebit != '') {
                        if (curr == 'INR') {
                            //                            grandtotal = Math.round(parseFloat(totaldebit) + parseFloat(totalcgst) + parseFloat(totalsgst) + parseFloat(totaligst))
                            //                            Nettotal = Math.round(parseFloat(totaldebit) + parseFloat(totalcgst) + parseFloat(totalsgst) + parseFloat(totaligst) - parseFloat(totaltds))
                            grandtotal = Math.round((parseFloat(totaldebit) + parseFloat(totalcgst) + parseFloat(totalsgst) + parseFloat(totaligst)) - (parseFloat(totalcredit)))
                            Nettotal = Math.round((parseFloat(totaldebit) + parseFloat(totalcgst) + parseFloat(totalsgst) + parseFloat(totaligst)) - (parseFloat(totaltds) + parseFloat(totalcredit)))
                        }
                        else {
                            //                            grandtotal = (parseFloat(totaldebit) + parseFloat(totalcgst) + parseFloat(totalsgst) + parseFloat(totaligst)).toFixed(2)
                            //                            Nettotal = (parseFloat(totaldebit) + parseFloat(totalcgst) + parseFloat(totalsgst) + parseFloat(totaligst) - parseFloat(totaltds)).toFixed(2)
                            grandtotal = (parseFloat(totaldebit) + parseFloat(totalcgst) + parseFloat(totalsgst) + parseFloat(totaligst)) - (parseFloat(totalcredit))
                            Nettotal = (parseFloat(totaldebit) + parseFloat(totalcgst) + parseFloat(totalsgst) + parseFloat(totaligst)) - (parseFloat(totaltds) + parseFloat(totalcredit))
                        }

                    }
                    //                alert(totaldebit)

                    $("#txtGrandtotal").val(grandtotal)

                    //                    var Nettotal = parseFloat(totaldebit) + parseFloat(totalcgst) + parseFloat(totalsgst) + parseFloat(totaligst) - parseFloat(totaltds)

                    $("#txtNetamt").val(Nettotal)

                    //                    $("[id*=txtgCgst]").attr('readonly', 'readonly');
                    //                    $("[id*=txtgScgst]").attr('readonly', 'readonly');
                    //                    $("[id*=txtgIgst]").attr('readonly', 'readonly');
                    //                    $("[id*=txtgTds]").attr('readonly', 'readonly');
                    //                    $("[id*=txtgdtsamt]").attr('readonly', 'readonly');
                }
            }
            //            else {
            //                chargetype[document.getElementById("Hd_row_id").value].selectedIndex = 1;
            //            }
        }

        function CallFailed(res, destCtrl) {
        }
    </script>
        <script type="text/javascript">
            function Calculationtdsamt(lnk) {
                if ($(".debitgst").val() != null && $(".debitgst").val() != '') {
                    var txtAmountReceive = $(".debitgst")
                }
                else if ($(".creditgst").val() != null && $(".creditgst").val() != '') {
                    var txtAmountReceive = $(".creditgst")
                }

                var row = lnk.parentNode.parentNode;
                var rowIndex = row.rowIndex - 1;
                var a = txtAmountReceive[rowIndex].value;
                //                var txttds = $("[id*=txtgTds]");
                //                var tds = txttds[rowIndex].value;
                //                var tdsamt = MathRound(a * (tds / 100));

                //                var txtdtsamt = $("[id*=txtgdtsamt]")
                //                txtdtsamt[rowIndex].value = tdsamt;
                calculate()
                var totalcredit = $("[id*=txtcredittotal").val()
                var totaldebit = $("[id*=txtdebittotal]").val()
                var totalcgst = $("[id*=txtcgsttotal]").val()
                var totalsgst = $("[id*=txtsgsttotal]").val()
                var totaligst = $("[id*=txtigsttotal]").val()
                var totaltds = $("[id*=txttdstotal]").val()
                var grandtotal, Nettotal
                var curr = document.getElementById("txtInvoiceCurrency").value;
                if (totalcredit != null && totaldebit != null && totalcredit != '' && totaldebit != '') {
                    if (curr == 'INR') {
                        grandtotal = Math.round(parseFloat(totalcredit) + parseFloat(totaldebit) + parseFloat(totalcgst) + parseFloat(totalsgst) + parseFloat(totaligst))
                        Nettotal = Math.round(parseFloat(totalcredit) + parseFloat(totaldebit) + parseFloat(totalcgst) + parseFloat(totalsgst) + parseFloat(totaligst) - parseFloat(totaltds))
                    }
                    else {
                        grandtotal = parseFloat(totalcredit) + parseFloat(totaldebit) + parseFloat(totalcgst) + parseFloat(totalsgst) + parseFloat(totaligst)
                        Nettotal = parseFloat(totalcredit) + parseFloat(totaldebit) + parseFloat(totalcgst) + parseFloat(totalsgst) + parseFloat(totaligst) - parseFloat(totaltds)
                    }
                }
                else if (totalcredit != null && totalcredit != '') {
                    if (curr == 'INR') {
                        grandtotal = Math.round(parseFloat(totalcredit) + parseFloat(totalcgst) + parseFloat(totalsgst) + parseFloat(totaligst))
                        Nettotal = Math.round(parseFloat(totalcredit) + parseFloat(totalcgst) + parseFloat(totalsgst) + parseFloat(totaligst) - parseFloat(totaltds))
                    }
                    else {
                        grandtotal = parseFloat(totalcredit) + parseFloat(totalcgst) + parseFloat(totalsgst) + parseFloat(totaligst)
                        Nettotal = parseFloat(totalcredit) + parseFloat(totalcgst) + parseFloat(totalsgst) + parseFloat(totaligst) - parseFloat(totaltds)
                    }
                }
                else if (totaldebit != null && totaldebit != '') {
                    if (curr == 'INR') {
                        grandtotal = Math.round(parseFloat(totaldebit) + parseFloat(totalcgst) + parseFloat(totalsgst) + parseFloat(totaligst))
                        Nettotal = Math.round(parseFloat(totaldebit) + parseFloat(totalcgst) + parseFloat(totalsgst) + parseFloat(totaligst) - parseFloat(totaltds))
                    }
                    else {
                        grandtotal = parseFloat(totaldebit) + parseFloat(totalcgst) + parseFloat(totalsgst) + parseFloat(totaligst)
                        Nettotal = parseFloat(totaldebit) + parseFloat(totalcgst) + parseFloat(totalsgst) + parseFloat(totaligst) - parseFloat(totaltds)
                    }
                }
                //                alert(totaldebit)
                $("#txtGrandtotal").val(grandtotal)
                //                    var Nettotal = parseFloat(totaldebit) + parseFloat(totalcgst) + parseFloat(totalsgst) + parseFloat(totaligst) - parseFloat(totaltds)
                $("#txtNetamt").val(Nettotal)

            }
            function Calculationtds(lnk) {
                if ($(".debitgst").val() != null && $(".debitgst").val() != '') {
                    var txtAmountReceive = $(".debitgst")
                }
                else if ($(".creditgst").val() != null && $(".creditgst").val() != '') {
                    var txtAmountReceive = $(".creditgst")
                }


                var row = lnk.parentNode.parentNode;
                var rowIndex = row.rowIndex - 1;
                var a = txtAmountReceive[rowIndex].value;
                var txttds = $("[id*=txtgTds]");

                tds = txttds[rowIndex].value;
                var tdsamt = Math.round(a * (tds / 100));

                var txtdtsamt = $("[id*=txtgdtsamt]")
                txtdtsamt[rowIndex].value = tdsamt;
                calculate()
                var totalcredit = $("[id*=txtcredittotal").val()
                var totaldebit = $("[id*=txtdebittotal]").val()
                var totalcgst = $("[id*=txtcgsttotal]").val()
                var totalsgst = $("[id*=txtsgsttotal]").val()
                var totaligst = $("[id*=txtigsttotal]").val()
                var totaltds = $("[id*=txttdstotal]").val()
                var grandtotal, Nettotal
                var curr = document.getElementById("txtInvoiceCurrency").value;
                if (totalcredit != null && totaldebit != null && totalcredit != '' && totaldebit != '') {
                    if (curr == 'INR') {
                        grandtotal = Math.round(parseFloat(totalcredit) + parseFloat(totaldebit) + parseFloat(totalcgst) + parseFloat(totalsgst) + parseFloat(totaligst))
                        Nettotal = Math.round(parseFloat(totalcredit) + parseFloat(totaldebit) + parseFloat(totalcgst) + parseFloat(totalsgst) + parseFloat(totaligst) - parseFloat(totaltds))
                    }
                    else {
                        grandtotal = parseFloat(totalcredit) + parseFloat(totaldebit) + parseFloat(totalcgst) + parseFloat(totalsgst) + parseFloat(totaligst)
                        Nettotal = parseFloat(totalcredit) + parseFloat(totaldebit) + parseFloat(totalcgst) + parseFloat(totalsgst) + parseFloat(totaligst) - parseFloat(totaltds)
                    }
                }
                else if (totalcredit != null && totalcredit != '') {
                    if (curr == 'INR') {
                        grandtotal = Math.round(parseFloat(totalcredit) + parseFloat(totalcgst) + parseFloat(totalsgst) + parseFloat(totaligst))
                        Nettotal = Math.round(parseFloat(totalcredit) + parseFloat(totalcgst) + parseFloat(totalsgst) + parseFloat(totaligst) - parseFloat(totaltds))
                    }
                    else {
                        grandtotal = parseFloat(totalcredit) + parseFloat(totalcgst) + parseFloat(totalsgst) + parseFloat(totaligst)
                        Nettotal = parseFloat(totalcredit) + parseFloat(totalcgst) + parseFloat(totalsgst) + parseFloat(totaligst) - parseFloat(totaltds)
                    }
                }
                else if (totaldebit != null && totaldebit != '') {
                    if (curr == 'INR') {
                        grandtotal = Math.round(parseFloat(totaldebit) + parseFloat(totalcgst) + parseFloat(totalsgst) + parseFloat(totaligst))
                        Nettotal = Math.round(parseFloat(totaldebit) + parseFloat(totalcgst) + parseFloat(totalsgst) + parseFloat(totaligst) - parseFloat(totaltds))
                    }
                    else {
                        grandtotal = parseFloat(totaldebit) + parseFloat(totalcgst) + parseFloat(totalsgst) + parseFloat(totaligst)
                        Nettotal = parseFloat(totaldebit) + parseFloat(totalcgst) + parseFloat(totalsgst) + parseFloat(totaligst) - parseFloat(totaltds)
                    }
                }
                //                alert(totaldebit)
                $("#txtGrandtotal").val(grandtotal)
                //                    var Nettotal = parseFloat(totaldebit) + parseFloat(totalcgst) + parseFloat(totalsgst) + parseFloat(totaligst) - parseFloat(totaltds)
                $("#txtNetamt").val(Nettotal)

            }
            function gst(lnk) {



                if ($(".debitgst").val() != null && $(".debitgst").val() != '') {
                    var txtAmountReceive = $(".debitgst")


                }
                else if ($(".creditgst").val() != null && $(".creditgst").val() != '') {
                    var txtAmountReceive = $(".creditgst")

                }

                var row = lnk.parentNode.parentNode;
                var rowIndex = row.rowIndex - 1;
                var a = txtAmountReceive[rowIndex].value;

                //                alert(a);
                if (a != null && a != '') {

                    var cgst = MathRound(a * (document.getElementById("Hdncgst").value / 100))

                    var sgst = MathRound(a * (document.getElementById("Hdnsgst").value / 100))


                    var igst = MathRound(a * (document.getElementById("Hdnigst").value / 100))



                    //                    var tds = document.getElementById("hdntds").value;
                    var tds = $("[id*=txtgTds]");

                    var tdsamt = MathRound(a * (tds[document.getElementById("Hd_row_id").value].value / 100));

                    //                var txtcgst = $("[id*=");
                    //                var txtcgst = $("[id*=");
                    //                var txtigst = $("[id*=");
                    //                var txtcgst = $("[id*=");

                    var txtCgst = $("[id*=txtgCgst]")
                    var txtSgst = $("[id*=txtgScgst]")
                    var txtIgst = $("[id*=txtgIgst]")
                    var txttds = $("[id*=txtgTds]")
                    var txtdtsamt = $("[id*=txtgdtsamt]")
                    var state = $('input[name="Rd_Bill_Type"]:checked').val();
                    var tax_mode = document.getElementById("hdntaxmode").value;

                    if (state == 'L') {
                        if (tax_mode = 'T' && cgst != '0') {
                            txtCgst[document.getElementById("Hd_row_id").value].value = cgst;
                            txtSgst[document.getElementById("Hd_row_id").value].value = sgst
                            txtIgst[document.getElementById("Hd_row_id").value].value = 0
                        }
                        else {
                            txtCgst[document.getElementById("Hd_row_id").value].value = 0;
                            txtSgst[document.getElementById("Hd_row_id").value].value = 0
                            txtIgst[document.getElementById("Hd_row_id").value].value = igst
                        }

                    }
                    else {
                        txtCgst[document.getElementById("Hd_row_id").value].value = 0;
                        txtSgst[document.getElementById("Hd_row_id").value].value = 0
                        txtIgst[document.getElementById("Hd_row_id").value].value = igst
                    }
                    txttds[document.getElementById("Hd_row_id").value].value = tds[document.getElementById("Hd_row_id").value].value;
                    txtdtsamt[document.getElementById("Hd_row_id").value].value = tdsamt;
                    calculate()


                    var totalcredit = $("[id*=txtcredittotal").val()
                    var totaldebit = $("[id*=txtdebittotal]").val()
                    var totalcgst = $("[id*=txtcgsttotal]").val()
                    var totalsgst = $("[id*=txtsgsttotal]").val()
                    var totaligst = $("[id*=txtigsttotal]").val()
                    var totaltds = $("[id*=txttdstotal]").val()
                    var grandtotal, Nettotal
                    var curr = document.getElementById("txtInvoiceCurrency").value;
                    if (totalcredit != null && totaldebit != null && totalcredit != '' && totaldebit != '') {


                        if (totalcgst == 'NaN') { totalcgst = 0 }
                        if (totalsgst == 'NaN') { totalsgst = 0 }
                        if (totaligst == 'NaN') { totaligst = 0 }
                        if (totaltds == 'NaN') { totaltds = 0 }
                        if (curr == 'INR') {
                            //                            grandtotal = Math.round(parseFloat(totalcredit) + parseFloat(totaldebit) + parseFloat(totalcgst) + parseFloat(totalsgst) + parseFloat(totaligst))
                            //                            Nettotal = Math.round(parseFloat(totalcredit) - parseFloat(totaldebit) + parseFloat(totalcgst) + parseFloat(totalsgst) + parseFloat(totaligst) - parseFloat(totaltds))
                            grandtotal = Math.round((parseFloat(totaldebit) + parseFloat(totalcgst) + parseFloat(totalsgst) + parseFloat(totaligst)) - (parseFloat(totalcredit)))
                            Nettotal = Math.round((parseFloat(totaldebit) + parseFloat(totalcgst) + parseFloat(totalsgst) + parseFloat(totaligst)) - (parseFloat(totaltds) + parseFloat(totalcredit)))
                        }
                        else {
                            //                            grandtotal = (parseFloat(totalcredit) + parseFloat(totaldebit) + parseFloat(totalcgst) + parseFloat(totalsgst) + parseFloat(totaligst)).toFixed(2)
                            //                            Nettotal = (parseFloat(totalcredit) - parseFloat(totaldebit) + parseFloat(totalcgst) + parseFloat(totalsgst) + parseFloat(totaligst) - parseFloat(totaltds)).toFixed(2)
                            grandtotal = (parseFloat(totaldebit) + parseFloat(totalcgst) + parseFloat(totalsgst) + parseFloat(totaligst)) - (parseFloat(totalcredit))
                            Nettotal = (parseFloat(totaldebit) + parseFloat(totalcgst) + parseFloat(totalsgst) + parseFloat(totaligst)) - (parseFloat(totaltds) + parseFloat(totalcredit))
                        }
                    }
                    else if (totalcredit != null && totalcredit != '') {
                        if (curr == 'INR') {
                            //                            grandtotal = Math.round(parseFloat(totalcredit) + parseFloat(totalcgst) + parseFloat(totalsgst) + parseFloat(totaligst))
                            //                            Nettotal = Math.round(parseFloat(totalcredit) + parseFloat(totalcgst) + parseFloat(totalsgst) + parseFloat(totaligst) - parseFloat(totaltds))
                            grandtotal = Math.round((parseFloat(totaldebit) + parseFloat(totalcgst) + parseFloat(totalsgst) + parseFloat(totaligst)) - (parseFloat(totalcredit)))
                            Nettotal = Math.round((parseFloat(totaldebit) + parseFloat(totalcgst) + parseFloat(totalsgst) + parseFloat(totaligst)) - (parseFloat(totaltds) + parseFloat(totalcredit)))
                        }
                        else {
                            //                            grandtotal = (parseFloat(totalcredit) + parseFloat(totalcgst) + parseFloat(totalsgst) + parseFloat(totaligst)).toFixed(2)
                            //                            Nettotal = (parseFloat(totalcredit) + parseFloat(totalcgst) + parseFloat(totalsgst) + parseFloat(totaligst) - parseFloat(totaltds)).toFixed(2)
                            grandtotal = (parseFloat(totaldebit) + parseFloat(totalcgst) + parseFloat(totalsgst) + parseFloat(totaligst)) - (parseFloat(totalcredit))
                            Nettotal = (parseFloat(totaldebit) + parseFloat(totalcgst) + parseFloat(totalsgst) + parseFloat(totaligst)) - (parseFloat(totaltds) + parseFloat(totalcredit))
                        }
                    }
                    else if (totaldebit != null && totaldebit != '') {
                        if (curr == 'INR') {
                            //                            grandtotal = Math.round(parseFloat(totaldebit) + parseFloat(totalcgst) + parseFloat(totalsgst) + parseFloat(totaligst))
                            //                            Nettotal = Math.round(parseFloat(totaldebit) + parseFloat(totalcgst) + parseFloat(totalsgst) + parseFloat(totaligst) - parseFloat(totaltds))
                            grandtotal = Math.round((parseFloat(totaldebit) + parseFloat(totalcgst) + parseFloat(totalsgst) + parseFloat(totaligst)) - (parseFloat(totalcredit)))
                            Nettotal = Math.round((parseFloat(totaldebit) + parseFloat(totalcgst) + parseFloat(totalsgst) + parseFloat(totaligst)) - (parseFloat(totaltds) + parseFloat(totalcredit)))
                        }
                        else {
                            //                            grandtotal = (parseFloat(totaldebit) + parseFloat(totalcgst) + parseFloat(totalsgst) + parseFloat(totaligst)).toFixed(2)
                            //                            Nettotal = (parseFloat(totaldebit) + parseFloat(totalcgst) + parseFloat(totalsgst) + parseFloat(totaligst) - parseFloat(totaltds)).toFixed(2)
                            grandtotal = (parseFloat(totaldebit) + parseFloat(totalcgst) + parseFloat(totalsgst) + parseFloat(totaligst)) - (parseFloat(totalcredit))
                            Nettotal = (parseFloat(totaldebit) + parseFloat(totalcgst) + parseFloat(totalsgst) + parseFloat(totaligst)) - (parseFloat(totaltds) + parseFloat(totalcredit))
                        }

                    }
                    //                alert(totaldebit)

                    $("#txtGrandtotal").val(grandtotal)

                    //                    var Nettotal = parseFloat(totaldebit) + parseFloat(totalcgst) + parseFloat(totalsgst) + parseFloat(totaligst) - parseFloat(totaltds)

                    $("#txtNetamt").val(Nettotal)

                    //                    $("[id*=txtgCgst]").attr('readonly', 'readonly');
                    //                    $("[id*=txtgScgst]").attr('readonly', 'readonly');
                    //                    $("[id*=txtgIgst]").attr('readonly', 'readonly');
                    //                    $("[id*=txtgTds]").attr('readonly', 'readonly');
                    //                    $("[id*=txtgdtsamt]").attr('readonly', 'readonly');
                }
                //                
            }
            function tap() {

                $("[id*=drpImpexp]").focus();
            }

            function cusname(lnk) {

                var grid = document.getElementById("<%= gvdetails.ClientID%>");
                var txtAmountReceive = $(".Fileref")
                var Imp_Exp = $("[id*=drpImpexp]")
                var row = lnk.parentNode.parentNode;
                var rowIndex = row.rowIndex - 1;
                var a = txtAmountReceive[rowIndex].value;
                var b = Imp_Exp[rowIndex].value;

                //                alert(a);
                //                var b = '1';
                $('#Hd_row_id').val(rowIndex);
                Cha_cus(a, b);
            }
            function Cha_cus(src, dest) {

                PageMethods.Cust_Name(src, dest, '', CallSuccess_Cusname, CallFaileda, dest);
            }
            function CallSuccess_Cusname(res, destCtrl) {

                var data = res;
                //                                alert(data);
                var txtC = $("[id*=txtgCusName]")
                var txtD = $("[id*=txtgDate]")
                var txtE = $("[id*=txtgFileRefNo]")
                var txtF = $("[id*=txtTAX_INVNO_PS]")         // add this one by rosi
           
                if (res != '') {
                    if (data.indexOf("~~") != -1 && data.indexOf("~~") != '') {
                        var s = data.split('~~');
                        var name = s[0];
                        var date = s[1];
                        var ref = s[2];
                        var inv = s[3];
                        txtC[document.getElementById("Hd_row_id").value].value = name;
                        txtD[document.getElementById("Hd_row_id").value].value = date;
                        txtE[document.getElementById("Hd_row_id").value].value = ref;
                        txtF[document.getElementById("Hd_row_id").value].value = inv;

                    }

                    //                    var cusname = data;

                }
                else {
                    txtC[document.getElementById("Hd_row_id").value].selectedIndex = 1;
                }
            }

            function CallFaileda(res, destCtrl) {
            }
            function drcr(lnk) {

                var txtAmountReceive = $("[id*=drpDrcr]")
                var row = lnk.parentNode.parentNode;
                var rowIndex = row.rowIndex - 1;
                var a = txtAmountReceive[rowIndex].value;
                //                alert(a)
                //                var txtdebit=$("[id*=txtgDebit1]")
                //                var txtcredit=$("[id*=txtgCredit1]")
                //                alert(a)
                if (a == 'CR') {
                    $("[id*=txtgDebit1]").attr('readonly', 'readonly');
                    $("[id*=txtgCredit1]").removeAttr('readonly');
                }
                else if (a == 'DR') {
                    $("[id*=txtgCredit1]").attr('readonly', 'readonly');
                    $("[id*=txtgDebit1]").removeAttr('readonly');
                }

            }
    </script>
    <script  type="text/javascript">
        window.onload = calculate();
        function calculate() {
            var txtTotal = 0.00, txttdstot = 0.00, txtigtot = 0.00, txtsgtot = 0.00, txtcgtot = 0.00, txtcredittotal = 0.00
            $(".creditgst").each(function (index, value) {
                var valcre = value.value;
                val = valcre.replace(",", ".");
                txtcredittotal = MathRound(parseFloat(txtcredittotal) + parseFloat(valcre));
                //                    alert(txtTotal)
            });
            $(".debitgst").each(function (index, value) {
                var val = value.value;
                val = val.replace(",", ".");
                txtTotal = MathRound(parseFloat(txtTotal) + parseFloat(val));
                //                    alert(txtTotal)
            });
            $(".cgst").each(function (index1, value1) {
                var valcg = value1.value;
                valcg = valcg.replace(",", ".");
                txtcgtot = MathRound(parseFloat(txtcgtot) + parseFloat(valcg));
                //                    alert(txtcgtot)
            });
            $(".sgst").each(function (index2, value2) {
                var valsg = value2.value;
                valsg = valsg.replace(",", ".");
                txtsgtot = MathRound(parseFloat(txtsgtot) + parseFloat(valsg));
                //                    alert(txtsgtot)
            });
            $(".igst").each(function (index3, value3) {
                var valig = value3.value;
                valig = valig.replace(",", ".");
                txtigtot = MathRound(parseFloat(txtigtot) + parseFloat(valig));
            });
            $(".tdsamt").each(function (index4, value4) {
                var valtds = value4.value;
                valtds = valtds.replace(",", ".");
                txttdstot = MathRound(parseFloat(txttdstot) + parseFloat(valtds));

            });
            //                alert(txtTotal)
            if (!isNaN(txtcredittotal)) {
                $("[id*=txtcredittotal]").val(txtcredittotal)
            }
            if (!isNaN(txtTotal)) {
                $("[id*=txtdebittotal]").val(txtTotal)
            }
            $("[id*=txtcgsttotal]").val(txtcgtot)
            $("[id*=txtsgsttotal]").val(txtsgtot)
            $("[id*=txtigsttotal]").val(txtigtot)
            $("[id*=txttdstotal]").val(txttdstot)
        }
        function MathRound(number) {
            var result = Math.round((number + Number.EPSILON) * 100) / 100;
            return result;
        }
            </script>

    <script type="text/javascript">
        function isDate() {

            var txt = document.getElementById("txtBilldate").value;
            var validformat = /^(0?[1-9]|[12][0-9]|3[01])[\/\-](0?[1-9]|1[012])[\/\-]\d{4}$/
            //            var validformat = /^(0?[1-9]|1[012])[\/\-](0?[1-9]|[12][0-9]|3[01])[\/\-]\d{4}$/;
            ////            var validformat = /^(?=\d)(?:(?:31(?!.(?:0?[2469]|11))|(?:30|29)(?!.0?2)|29(?=.0?2.(?:(?:(?:1[6-9]|[2-9]\d)?(?:0[48]|[2468][048]|[13579][26])|(?:(?:16|[2468][048]|[3579][26])00)))(?:\x20|$))|(?:2[0-8]|1\d|0?[1-9]))([-.\/])(?:1[012]|0?[1-9])\1(?:1[6-9]|[2-9]\d)?\d\d(?:(?=\x20\d)\x20|$))?(((0?[1-9]|1[012])(:[0-5]\d){0,2}(\x20[AP]M))|([01]\d|2[0-3])(:[0-5]\d){1,2})?$/;
            ////            var validformat = /^\d{2}\/\d{2}\/\d{4}$/ //Basic check for format validity
            ////            alert(!validformat.test(txt))
            if (validformat.test(txt) == false && txt != "") {
                alert("Invalid Date")
            }

        }
        function vendordetail() {
            var txtVendorname = $("#txtVendorname").val();
            var txtVendorbr = $("#txtVendorbranch").val();
            //        var a = txtVendorname.value;
            //        alert('hello');
            //        alert(txtVendorname);
            //        Vend(txtVendorname, txtVendorbr);
            PageMethods.Get_Vendordetail(txtVendorname, txtVendorbr, '', CallSuccess_vendordetail, CallFailed_vendordetail);


            //        PageMethods.Get_Vendordetail(txtVendorname, txtVendorbr, '', CallSuccess_vendordetail, CallFailed_vendordetail);
            //        alert('hi');
        }
        function Vend(src, dest) {
            //        alert('vendhi');
            PageMethods.Get_Vendordetail(txtVendorname, '', '', CallSuccess_vendordetail, CallFailed_vendordetail, dest);

        }
        function CallSuccess_vendordetail(res, destCtrl) {
            //        alert('success');
            var data = res;
            //                                alert(data);
            //        var txtstate = $("[id*=txtgCusName]").val(
            //        var txtGstn = $("[id*=txtgDate]")
            //        var txtGstntype = $("[id*=txtgDate]")
            //        var txtCountry = $("[id*=txtgDate]")
            //        alert(data);
            if (res != '') {
                if (data.indexOf("~~") != -1 && data.indexOf("~~") != '') {
                    var s = data.split('~~');
                    var state = s[0];
                    var Gstn = s[1];
                    var GstnType = s[2];
                    var country = s[3];
                    var gsttype = s[4];
                    ////                alert(state);
                    $("#txtVendorstate").val(state);
                    $("#txtGstn").val(Gstn);
                    $("#txtGstntype").val(GstnType);
                    $("#txtCountry").val(country);
                    $("#txtGstntype").val(gsttype);
                    //                $("#txtVendorstate").value = state;
                    ////                txtC[document.getElementById("Hd_row_id").value].value = name;
                    ////                txtD[document.getElementById("Hd_row_id").value].value = date;

                }

                //                    var cusname = data;

            }
            else {
                //            txtC[document.getElementById("Hd_row_id").value].selectedIndex = 1;
            }
        }

        function CallFailed_vendordetail(res, destCtrl) {
            ////    alert('fail');
        }
        function Charge(lnk) {

            var charge = $("[id*=txtgCharHead]")
            var row = lnk.parentNode.parentNode;
            var rowIndex = row.rowIndex - 1;
            var a = charge[rowIndex].value;

            PageMethods.Get_Charge(a, CallSuccess_Charge);
        }
        function CallSuccess_Charge(res) {


            var IGST = 0.00, TDSRATE = 0.00, CREDIT = 0.00
            data = res;
            if (data.indexOf("~~") != '') {
                var s = data.split('~~');

                var IGST_VALUE = s[2];
                var TDSRATE_VALUE = s[3];
                $("[id*=txtgTaxrate]").val(IGST_VALUE);
                $("[id*=txtgTds]").val(TDSRATE_VALUE);
                //            var gridView = document.getElementById("<%=gvdetails.ClientID %>");
                //                var rows = gridView.getElementsByTagName("tr")
                //                for (var i = 0; i < rows.length; i++) {
                //                    var CREDIT = $("input[id*=txtgCredit1]")[i].value;
                //                    alert(CREDIT);
                //                   
                //                    $("#txtgTaxrate").val(IGST_VALUE);
                //                    $("#txtgdtsamt").val(TDSRATE_VALUE);
                //                }

            }

        }
    </script>  
    <script type="text/javascript">

        $(function () {

            $("[id*=txtJOBNO]").autocomplete({
                source: function (request, response) {
                    var field = $(this.element).attr('id');

                    $.ajax({

                        url: "../AutoComplete_Pages/Auto_Complete_Searching.asmx/purchase_Imp_Exp_List",
                        data: "{ 'mail': '" + request.term + "' ,'Imp_Exp':'" + $("[id*=drpImpexp").val() + "'}",

                        dataType: "json",
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        dataFilter: function (data) { return data; },
                        async: true,
                        success: function (data) {
                            response(data.d);
                            if (data.d == '') {
                                if (field == 'txtJOBNO') {
                                    jQuery('#' + field).validationEngine('showPrompt', 'Incorrect Currency', 'error', 'topRight', true);

                                }

                            }
                            else {

                                jQuery('#' + field).validationEngine('hidePrompt', '', 'error', 'topRight', true);

                                var result = data.d;

                                var stringsearch = "-", str = result;

                                for (var i = count = 0; i < str.length; count += +(stringsearch === str[i++]));

                                if (i == 1) {
                                    if (field == 'txtJOBNO') {
                                        $('#txtJOBNO').val(result);
                                    }


                                }
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

 <script type="text/javascript">
     $(document).ready(function () {
         $("[id*=txtgCharHead]").autocomplete({
             source: function (request, response) {
                 $.ajax({
                     url: "../AutoComplete_Pages/Auto_Complete_Searching.asmx/Purchase_GST_Billing_Charge_Master_List",
                     data: "{ 'mail': '" + request.term + "' }",
                     dataType: "json",
                     type: "POST",
                     contentType: "application/json; charset=utf-8",
                     success: function (data) {
                         response($.map(data.d, function (item) {
                             return {
                                 label: item,
                                 val: item
                             }
                         }))
                     },
                     error: function (response) {
                         alert(response.responseText);
                     },
                     failure: function (response) {
                         alert(response.responseText);
                     }
                 });
             },
             select: function (e, i) {
                 $("[id*=hfCustomerId]", $(e.target).closest("td")).val(i.item.val);
             },
             minLength: 1
         });

     });
     $(function () {
         $('#txtInvoiceCurrency').autocomplete({
             source: function (request, response) {
                 var field = $(this.element).attr('id');

                 $.ajax({
                     url: "../AutoComplete_Pages/Auto_Complete_Searching.asmx/Exchage_Master_List",
                     data: "{ 'mail': '" + request.term + "' }",
                     dataType: "json",
                     type: "POST",
                     contentType: "application/json; charset=utf-8",
                     dataFilter: function (data) { return data; },
                     async: true,
                     success: function (data) {
                         response(data.d);
                         if (data.d == '') {
                             if (field == 'txtInvoiceCurrency') {
                                 jQuery('#' + field).validationEngine('showPrompt', 'Incorrect Currency', 'error', 'topRight', true);

                             }

                         }
                         else {

                             jQuery('#' + field).validationEngine('hidePrompt', '', 'error', 'topRight', true);

                             var result = data.d;

                             var stringsearch = "-", str = result;

                             for (var i = count = 0; i < str.length; count += +(stringsearch === str[i++]));

                             if (i == 1) {
                                 if (field == 'txtInvoiceCurrency') {
                                     $('#txtInvoiceCurrency').val(result);
                                 }


                             }
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

     function Calculationex() {

         var currency = document.getElementById("txtInvoiceCurrency").value;



         var a = currency;
         var b = $("[id*=drpImpexp").val();
         if (b == '---') {
             Cha_exrate(a, 'Imp FW Air');
         } else {
             Cha_exrate(a, b);
         }
         if (currency == 'INR') { document.getElementById("txtbillamount").disabled = true; }
         else { document.getElementById("txtbillamount").disabled = false; }

     }

     function Cha_exrate(src, dest) {



         PageMethods.Get_Cha_exrate(src, dest, CallSuccess_Cha_exrate, CallFailedexrate);



     }
     function CallSuccess_Cha_exrate(res) {

         var data = res;

         if (res != '') {

             document.getElementById("txtExrate").value = res;
             var Bill_Amt = document.getElementById("txtbillamount").value;
             document.getElementById("txtTotalamtininr").value = (Bill_Amt * res).toFixed(2);
         }

     }
     function CallFailedexrate(res, destCtrl) {
     }

     //     function totalAmt() {

     //         var Amt = document.getElementById("txtTotalamtininr").value;
     //         alert(Amt);
     //         if (Amt != null && Amt!='') {
     //             var Value1 = Amt;

     //             var a = $("[id*=drpDrcr]")
     //             if (a == 'CR') {

     //                 $("[id*=txtgCredit1").val(Value1);
     //             } else {
     //                 $("[id*=txtgDebit1").val(Value1);
     //             }
     //         }

     //     }

     function foreign_amt() {


         var Bill_Amt = document.getElementById("txtbillamount").value;
         var Bill_inr_Amt = document.getElementById("txtTotalamtininr").value;

         if (Bill_Amt != null && Bill_Amt != '') {
             var Value = Bill_Amt;

             var a = $("[id*=drpDrcr]")
       
             if (a == 'DR') {

                 $("[id*=txtgCreditfc]").attr('readonly', 'readonly');
                 $("[id*=txtgDebitfc]").removeAttr('readonly');
                 $("[id*=txtgDebitfc]").val(Value);
                 $("[id*=txtgDebit1]").val(Bill_inr_Amt);
                 

             } else {

                

                 $("[id*=txtgDebitfc]").attr('readonly', 'readonly');
                 $("[id*=txtgCreditfc]").removeAttr('readonly');
                 $("[id*=txtgCreditfc]").val(Value);
                 $("[id*=txtgCredit1]").val(Bill_inr_Amt);



             }
         }

     }
     function TOTALAmt() {
         var res = document.getElementById("txtExrate").value;
         var Bill_Amt = document.getElementById("txtbillamount").value;
         document.getElementById("txtTotalamtininr").value = (Bill_Amt * res).toFixed(2);

     }
     function Deb_cre_value(lnk) {
         var dr = $("[id*=drpDrcr]")

         var exr = document.getElementById("txtExrate").value;
         var row = lnk.parentNode.parentNode;

         var rowIndex = row.rowIndex - 1;

         var a = dr[rowIndex].value;
         if (document.getElementById("txtbillamount").value != '') {
             if (a == 'CR') {


                 var credit = $(".creditgstfc")
                 var B = credit[rowIndex].value;
                 var vc = B * exr;

                 //             txtCgst[document.getElementById("txtgCrefc").value].value = cgst;

                 var txtcr = $("[id*=txtgCredit1]")
                 txtcr[rowIndex].value = vc;

             } else {
                 var Debit = $(".debitgstfc")
                 var a = Debit[rowIndex].value;

                 var vb = parseFloat(a) * exr;

                 var txtdB = $(".debitgst")

                 txtdB[rowIndex].value = vb;


             }
         }
     }
     function Cost_Sheet(lnk) {
         var grid = document.getElementById("<%= gvdetails.ClientID%>");
         var row = lnk.parentNode.parentNode;

         var rowIndex = row.rowIndex - 1;
         var a = $("[id*=txtJOBNO]")
         var JobNo = a[rowIndex].value;
         var Voucher_No = document.getElementById("txtVoucherno").value;
         var SNo = rowIndex + 1;


         NewWindow1('../Reports/Purchase_Report/Purchase_Cost_Sheet.aspx?JobNo=' + JobNo + '&Voucher_No=' + Voucher_No + '&SNo=' + SNo + '', 'List', '870', '1024', 'yes');

     }
     function Voucher() {
         Hidden_Voucher_date.value = '1';

     }
</script>
   
     <script type="text/javascript">
         var config = {
             '.chosen-select': {},
             '.chosen-select-deselect': { allow_single_deselect: true, search_contains: true },
             '.chosen-select-no-single': { disable_search_threshold: 10 },
             '.chosen-select-no-results': { no_results_text: 'Oops, nothing found!' },
             '.chosen-select-width': { width: "45%" }

         }
         for (var selector in config) {
             $(selector).chosen(config[selector]);
         }
    </script>
           

           
</body>
</html>
