<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PaymentEntry.aspx.cs" Inherits="PaymentEntry" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=iso-8859-1" />
    <title>Payments </title>
    <link href="../css/MainStyle.css" rel="stylesheet" media="screen, projection" type="text/css" />
    <link href="../main.css" rel="stylesheet" media="screen, projection" type="text/css" />
    <link href="../gridmain.css" rel="stylesheet" media="screen, projection" type="text/css" />
    <link rel="stylesheet" href="../css/jquery-ui.css" />
    <link rel="stylesheet" type="text/css" href="../css/dddropdownpanel.css" />
    <link rel="stylesheet" href="../windowfiles/dhtmlwindow.css" type="text/css" />
    <link rel="stylesheet" href="../modalfiles/modal.css" type="text/css" />
   
    <link href="../AutoComplete_CSS/jquery-ui.css.css" rel="stylesheet" type="text/css" />
    <!-- VALIDATION SCRIPT -->
    <link rel="stylesheet" href="../Validation Files/validationEngine.css" type="text/css" />
    <link rel="stylesheet" href="../Validation Files/template.css" type="text/css" />
    <!-- VALIDATION SCRIPT -->
    <script src="../MessageBox_js/jquery.js" type="text/javascript"></script>
    <link href="../MessageBox_js/jquery.alerts.css" rel="stylesheet" type="text/css" />
    <script src="../MessageBox_js/jquery.alerts.js" type="text/javascript"></script>
    <script src="../js/Billing/Invoice.js" type="text/javascript"></script>
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
    <style type="text/css">
        input.underlined
        {
            border: solid 1px #000;
        }
    </style>

     <script language="javascript">
         function B_G_tab_page2() {

             document.getElementById('page-1').style.display = 'Block';
           
             document.getElementById("page-6").style.display = 'none';
             document.getElementById("page-7").style.display = 'block';
             
         }
   </script>
     
    <script type="text/javascript">
        function Char_Inv(evt) {
            var charCode = (evt.which) ? evt.which : event.keyCode
            if (charCode > 31 && (charCode < 48 || charCode > 57) && (charCode < 65 || charCode > 90) && (charCode < 97 || charCode > 122) && (charCode < 45 || charCode > 45) && (charCode < 47 || charCode > 47))
                return false;
            return true;
        }
    </script>

</head>
<body style="overflow: hidden;">
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePartialRendering="false"
        LoadScriptsBeforeUI="false" EnablePageMethods="true">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional" Visible="true">
        <ContentTemplate>
            <div class="loading" align="center" id="load" style="display: none;">
                <img src="../Loading_Images/indicator_mozilla_blu.gif" alt="" />
            </div>
            
            <div id="innerbox_MidMain_Billing" style="height: 130px;">
                <div id="tag_srcinner1">
                    <div id="mainmastop2container_rght_tag2_txt1" style="width: 160px;">
                      Payment-Entry
                    </div>

                     <div id="txt_container_Transact_Main_l" style="width: 510px;">
                        <div id="tag_label_transact_Src" style="width: 100px;">
                            Payment No
                        </div>
                        <div id="txtcon-m_Exchange" style="width: 160px;">
                            <asp:TextBox ID="txtPaymentNo" Width="140px" CssClass="validate[required] txtbox_none_Mid_transac_code" Enabled="false"
                                TabIndex="6" runat="server"></asp:TextBox>
                        </div>
                        <div id="tag_label_transact_Src" style="width: 100px;">
                            Payment Date
                        </div>
                         <div id="txtcon-m_Exchange" style="width: 150px;">
                             <asp:TextBox ID="txtDate" CssClass="validate[required] txtbox_none_Mid_transac_code"
                                 TabIndex="8" Width="100px" runat="server"></asp:TextBox>
                             <asp:MaskedEditExtender ID="MaskedEditExtender1" runat="server" TargetControlID="txtDate"
                                 Mask="99/99/9999" MaskType="Date" ErrorTooltipEnabled="True" CultureAMPMPlaceholder=""
                                 CultureCurrencySymbolPlaceholder="" CultureDateFormat="" CultureDatePlaceholder=""
                                 CultureDecimalPlaceholder="" CultureThousandsPlaceholder="" CultureTimePlaceholder=""
                                 Enabled="True">
                             </asp:MaskedEditExtender>
                             <asp:CalendarExtender ID="CalendarExtender2" runat="server" TargetControlID="txtDate"
                                 Format="dd/MM/yyyy" Enabled="True">
                             </asp:CalendarExtender>
                         </div>
                    </div>
                    <div id="tag_transac_lft_in2" style="margin-top: 5px;">
                        <div id="tag_label_transact_Src" style="width: 80px;">
                            Payment  For:
                        </div>
                        <div id="txtcon-m_Exchange" style="width: 200px;">
                            <asp:RadioButtonList ID="RdPayFor" runat="server" RepeatDirection="Horizontal" TabIndex="9">
                                <asp:ListItem Text="Bill" Value="B" Selected="True"></asp:ListItem>
                                <asp:ListItem Text="AdvPayment" Value="A"></asp:ListItem>
                            </asp:RadioButtonList>
                        </div>
                    </div>
                    <div id="popupwindow_closebut_right_new">
                        <input type="close" value="Submit" class="clsicon_new" onclick="win_hide();parent.adcodewindow.hide();return false;" />
                    </div>
                </div>

                <div id="tag_transact_src_inner" style="width: 1280px;height:100px;">
                    <div id="tag_Exchange_inner_lft" style="width: 1280px;height:90px;">
                        <div id="tag_transact_lft_in1" style="width: 1280px; height:80px;">                            
                            <div style="width: 1400px; height: 30px">
                                <div id="txt_container_Transact_Main_l" style="width: 1280px;">
                                    <div id="tag_label_transact_Src" style="width: 90px;">
                                        Vendor Name
                                    </div>
                                    <div id="txtcon-m_Exchange" style="width: 360px;">
                                        <asp:DropDownList ID="ddlVendorname" class="chosen-select-deselect" Style="width: 355px;"
                                            data-placeholder="Choose a Vendorname" onchange="Vendor_Details();"
                                            runat="server" TabIndex="10">
                                        </asp:DropDownList>
                                    </div>
                                    <div id="tag_label_transact_Src" style="width: 65px;">
                                        Branch
                                    </div>
                                    <div id="txtcon-m_Exchange" style="width: 230px">


                                          <asp:DropDownList ID="ddlVendorBranch" runat="server" 
                                                     CssClass="listtxt_transac_mode_new" 
                                                     ClientIDMode="Static"  onchange="Vendor_Branch();Job_Details();" 
                                              TabIndex="12" Width="225px">
                                                         </asp:DropDownList>
                        

<%--
                                        <asp:DropDownList ID="ddlVendorBranch" class="chosen-select-deselect" 
                                            Style="width: 225px;" data-placeholder="Choose a Branch No" 
                                            runat="server" TabIndex="12">
                                        </asp:DropDownList>--%>
                                    </div>

                                     <div id="tag_label_transact_Src" style="width: 60px;">
                                       Remarks
                                    </div>
                                    <div id="txtcon-m_Exchange" style="width: 400px;">
                                        <asp:TextBox ID="txt_Remarks"  runat="server"    ForeColor="Green" Font-Bold="true"
                                          Width="395px"  TabIndex="23" 
                                         MaxLength="50" CssClass="txtbox_none_Mid_transac_pop_gen_srcic" ></asp:TextBox>
                                    </div>

                                </div>
                                
                                <div  id="txt_container_Transact_Main_l"  style="width: 1450px; height:30px;" >
                                    <div id="tag_label_transact_Src" style="width: 90px;">
                                        Bank From
                                    </div>
                                    <div id="txtcon-m_Exchange" style="width: 360px;">
                                        <asp:DropDownList ID="ddl_Bank_From" 
                                            class="validate[required]  chosen-select-deselect" Style="width: 355px;"
                                            data-placeholder="Choose a BankName" 
                                            runat="server" TabIndex="10" onchange="Bank_Details();" 
                                         >
                                        </asp:DropDownList>
                                    </div>

                                    <div id="tag_label_transact_Src" style="width: 65px;">
                                        Acc. No
                                    </div>
                                    <div id="txtcon-m_Exchange" style="width: 230px">


                                        <asp:DropDownList ID="ddl_Acc_No" runat="server" 
                                                     CssClass="listtxt_transac_mode_new" 
                                                     ClientIDMode="Static" 
                                              TabIndex="10" Width="225px" onchange="IFSC_Details();Bank_Acc();">
                                                         </asp:DropDownList>
                        
<%--
                                        <asp:DropDownList ID="ddl_Acc_No" class="chosen-select-deselect" Style="width: 225px;"
                                            data-placeholder="Choose Account No" runat="server" TabIndex="10"  AutoPostBack="true"
                                            onselectedindexchanged="ddl_Acc_No_SelectedIndexChanged">
                                        </asp:DropDownList>--%>
                                    </div>

                                    <div id="tag_label_transact_Src" style="width: 70px;">
                                      IFSC  Code
                                    </div>
                                    <div id="txtcon-m_Exchange" style="width: 180px">


                                      <asp:DropDownList ID="ddl_ifsc_Code" runat="server" 
                                                     CssClass="listtxt_transac_mode_new" 
                                                     ClientIDMode="Static" 
                                              TabIndex="10" Width="175px" onchange="Bank_Ifsc();">
                                                         </asp:DropDownList>


                                    <%--    <asp:DropDownList ID="ddl_ifsc_Code" class="chosen-select-deselect" Style="width: 175px;"
                                            data-placeholder="Choose ifsc Code" 
                                            runat="server" TabIndex="10">
                                        </asp:DropDownList>--%>
                                    </div>

                                     <div id="tag_label_transact_Src" style="width: 230px;">
                                      <asp:RadioButtonList ID="Rd_Mode_Of_Payment" runat="server" Width="220px" RepeatDirection="Horizontal"
                                        TabIndex="9" onchange="Mode_of_transactions();" >
                                       <%-- AutoPostBack="true" onselectedindexchanged="Rd_Mode_Of_Payment_SelectedIndexChanged">--%>
                                        <asp:ListItem Text="Cash" Value="CA" Selected="True"></asp:ListItem>
                                        <asp:ListItem Text="Cheque" Value="CH"></asp:ListItem>
                                        <asp:ListItem Text="Net Banking" Value="NB"></asp:ListItem>
                                        </asp:RadioButtonList>
                                    </div>
                                    
                                </div>
                                 
                           
                           <div style="width: 1400px; height: 30px">                             
                                    <div id="tag_label_transact_Src" style="width: 90px;">
                                        Amount
                                    </div>
                                    <div id="txtcon-m_Exchange" style="width: 110px;">
                                         <asp:TextBox ID="txt_Cash_Amount"  width="100px" ForeColor="Blue" Font-Bold="true"  runat="server" style="text-align:right;"  
                                        CssClass="validate[required,custom[number]] txtbox_none_Mid_transac_item_maindet"
                                        onblur="extractNumber(this,2,false);" onkeyup="extractNumber(this,2,false);return Clear_Dot(this);"  onkeypress="return blockNonNumbers(this, event, true, false);"
                                        autocomplete="off" TabIndex="20" MaxLength="50"  ></asp:TextBox>
                                    </div>
                                
                                <span id="lbl_Chk_Nb_refno" runat="server">
                                <div id="tag_label_transact_Src" style="width: 100px;">
                                         Cheque / Ref No. 
                                    </div>
                               <div id="txtcon-m_Exchange" style="width: 150px;">
                                  <asp:TextBox ID="txt_Chk_NB_Refno" Width="140px" CssClass="txtbox_none_Mid_transac_code"
                                TabIndex="6" runat="server"></asp:TextBox>
                               </div>                
                                </span>

                                <span id="lbl_NB" runat="server">
                                    <div id="tag_label_transact_Src" style="width: 65px;">
                                       Bank To
                                    </div>
                                    <div id="txtcon-m_Exchange" style="width: 230px;">
                                      <asp:DropDownList ID="ddl_To_BankName" class="chosen-select-deselect" Style="width: 225px;"
                                            data-placeholder="Choose a BankName"  runat="server" TabIndex="10">
                                            <asp:ListItem Text="" Value=""></asp:ListItem>
                                            <asp:ListItem Text="Axis Bank Ltd." Value="Axis Bank Ltd."></asp:ListItem>
                                            <asp:ListItem Text="Bandhan Bank Ltd." Value="Bandhan Bank Ltd."></asp:ListItem>
                                            <asp:ListItem Text="Bank of Baroda" Value="Bank of Baroda"></asp:ListItem>
                                            <asp:ListItem Text="Bank of India" Value="Bank of India"></asp:ListItem>
                                            <asp:ListItem Text="Bank of Maharashtra" Value="Bank of Maharashtra"></asp:ListItem>
                                            <asp:ListItem Text="Canara Bank" Value="Canara Bank"></asp:ListItem>
                                            <asp:ListItem Text="City Union Bank Ltd." Value="City Union Bank Ltd."></asp:ListItem>
                                            <asp:ListItem Text="CSB Bank Ltd." Value="CSB Bank Ltd."></asp:ListItem>
                                            <asp:ListItem Text="DCB Bank Ltd." Value="DCB Bank Ltd."></asp:ListItem>
                                            <asp:ListItem Text="Dhanlaxmi Bank Ltd." Value="Dhanlaxmi Bank Ltd."></asp:ListItem>
                                            <asp:ListItem Text="Federal Bank Ltd." Value="Federal Bank Ltd."></asp:ListItem>
                                            <asp:ListItem Text="HDFC Bank Ltd" Value="HDFC Bank Ltd"></asp:ListItem>
                                            <asp:ListItem Text="ICICI Bank Ltd." Value="ICICI Bank Ltd."></asp:ListItem>
                                            <asp:ListItem Text="IDBI Bank Ltd." Value="IDBI Bank Ltd."></asp:ListItem>
                                            <asp:ListItem Text="IDFC First Bank Ltd." Value="IDFC First Bank Ltd."></asp:ListItem>
                                            <asp:ListItem Text="Indian Bank" Value="Indian Bank"></asp:ListItem>
                                            <asp:ListItem Text="Indian Overseas Bank" Value="Indian Overseas Bank"></asp:ListItem>
                                            <asp:ListItem Text="Induslnd Bank Ltd" Value="Induslnd Bank Ltd"></asp:ListItem>
                                            <asp:ListItem Text="Jammu & Kashmir Bank Ltd." Value="Jammu & Kashmir Bank Ltd."></asp:ListItem>
                                            <asp:ListItem Text="Karnataka Bank Ltd." Value="Karnataka Bank Ltd."></asp:ListItem>
                                            <asp:ListItem Text="Karur Vysya Bank Ltd." Value="Karur Vysya Bank Ltd."></asp:ListItem>
                                            <asp:ListItem Text="Kotak Mahindra Bank Ltd" Value="Kotak Mahindra Bank Ltd"></asp:ListItem>
                                            <asp:ListItem Text="Nainital Bank Ltd." Value="Nainital Bank Ltd."></asp:ListItem>
                                            <asp:ListItem Text="Punjab National Bank" Value="Punjab National Bank"></asp:ListItem>
                                            <asp:ListItem Text="RBL Bank Ltd." Value="RBL Bank Ltd."></asp:ListItem>
                                            <asp:ListItem Text="South Indian Bank Ltd." Value="South Indian Bank Ltd."></asp:ListItem>
                                            <asp:ListItem Text="State Bank of India" Value="State Bank of India"></asp:ListItem>
                                            <asp:ListItem Text="Tamilnad Mercantile Bank Ltd." Value="Tamilnad Mercantile Bank Ltd."></asp:ListItem>
                                            <asp:ListItem Text="UCO Bank" Value="UCO Bank"></asp:ListItem>
                                            <asp:ListItem Text="Union Bank of India" Value="Union Bank of India"></asp:ListItem>
                                            <asp:ListItem Text="YES Bank Ltd." Value="YES Bank Ltd."></asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                                       
                                    <div id="tag_label_transact_Src" style="width: 60px;">
                                       Acc. No
                                    </div>
                                    <div id="txtcon-m_Exchange" style="width: 160px;">
                                        <asp:TextBox ID="txt_to_Acc_No"  runat="server"    ForeColor="Green" Font-Bold="true"
                                         onfocusout="tap()"  Width="150px"  TabIndex="23" 
                                         MaxLength="50" CssClass="txtbox_none_Mid_transac_code" ></asp:TextBox>
                                    </div>
                                     <div id="tag_label_transact_Src" style="width: 60px;">
                                       ifsc Code
                                    </div>
                                    <div id="txtcon-m_Exchange" style="width: 120px;">
                                        <asp:TextBox ID="txt_to_Bank_ifsc_Code"  runat="server"    ForeColor="Green" Font-Bold="true"
                                         onfocusout="tap()"  Width="100px"  TabIndex="23" 
                                         MaxLength="50" CssClass="txtbox_none_Mid_transac_code" ></asp:TextBox>
                                    </div>
                                    
                                </span>


                                    <div id="txtcon-m_Exchange" style="width: 100px;">
                                        <asp:Button ID="btn_Save_Update" CssClass="autowidmovebut" width="100px" TabIndex="30" Height="25px"
                                            runat="server" onclick="btn_Save_Update_Click" />
                                    </div>
                                </div>                             
                           </div>
                           </div>
                           
                        </div>
                    </div>


            </div>

            <div class="content" id="page-1" style="margin-top: -28px;">
                <div id="innerbox_MidMain_Trans_new" style="height: 450px; width: 1280px; margin-left: -16px;
                    margin-top: -5px;">


                    <ol id="toc_new">
                        <li><a href="#page-6" onclick="jQuery('#form1').validationEngine('hideAll');B_G_tab_page1('0');"  id="Page_A" runat="server">
                        <span>General </span></a></li>
                        <li ><a href="#page-7" onclick="jQuery('#form1').validationEngine('hideAll');B_G_tab_page2('0');"
                            id="Page_B" runat="server" visible="false"  ><span>Payment History</span></a></li>
                       <%-- <li ><a href="#page-8" onclick="jQuery('#form1').validationEngine('hideAll');B_G_tab_page3('0');" id="Page_C" runat="server" visible="false">
                        <span>Gen -3</span></a></li>

                     <li><a href="#page-9" onclick="jQuery('#form1').validationEngine('hideAll');B_G_tab_page4('0');" id="Page_D" runat="server" visible="false">
                        <span> Gen -4 </span></a></li>--%>
                    </ol>
                    <div class="content" id="page-6">
                        <div id="innerbox_MidMain_Trans_new" style="height: 417px; width: 1298px; margin-left: -16px;
            margin-top: -40px;">
            <div id="pop_text_area_transac_popup_inn_container_export" style="height: 160px;
                margin-top: 30px;">
                <div id="tag_transact_src_inner" style="width: 1280px">
                    <div id="tag_Exchange_inner_lft" style="width: 1280px">
                        <div id="tag_transact_lft_in1" style="width: 1280px; height: 40px;">
                            <div id="txt_container_Transact_Main_l" style="width: 95px;">
                                <div id="tag_label_transact_Src" style="width: 35px;">
                                    Slno:
                                </div>
                                <div id="txtcon-m_Exchange" style="width: 60px;">
                                    <asp:DropDownList ID="ddlslno" CssClass="listtxt_transac_mode_new" Style="width: 50px;"
                                        runat="server" TabIndex="32">
                                        <asp:ListItem Text="1" Value="1"></asp:ListItem>
                                        <asp:ListItem Text="2" Value="2"></asp:ListItem>
                                        <asp:ListItem Text="3" Value="3"></asp:ListItem>
                                        <asp:ListItem Text="4" Value="4"></asp:ListItem>
                                        <asp:ListItem Text="5" Value="5"></asp:ListItem>
                                        <asp:ListItem Text="6" Value="6"></asp:ListItem>
                                        <asp:ListItem Text="7" Value="7"></asp:ListItem>
                                        <asp:ListItem Text="8" Value="8"></asp:ListItem>
                                        <asp:ListItem Text="9" Value="9"></asp:ListItem>
                                        <asp:ListItem Text="10" Value="10"></asp:ListItem>
                                        <asp:ListItem Text="11" Value="11"></asp:ListItem>
                                        <asp:ListItem Text="12" Value="12"></asp:ListItem>
                                        <asp:ListItem Text="13" Value="13"></asp:ListItem>
                                        <asp:ListItem Text="14" Value="14"></asp:ListItem>
                                        <asp:ListItem Text="15" Value="15"></asp:ListItem>
                                        <asp:ListItem Text="16" Value="16"></asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                            </div>

                             

                            <div id="txt_container_Transact_Main_l" style="width: 120px;margin-top:3px">
                                <div id="txtcon-m_Exchange" style="width: 120px">
                                    <asp:RadioButtonList ID="Rd_IMP_EXP" runat="server" RepeatDirection="Horizontal"
                                        ForeColor="Blue" Font-Bold="true" TabIndex="34" onchange="Job_Details();"
                                         Width="110px">
                                        <asp:ListItem Text="Imp" Value="Imp" Selected="True"></asp:ListItem>
                                        <asp:ListItem Text="Exp" Value="Exp"></asp:ListItem>
                                    </asp:RadioButtonList>
                                </div>
                            </div>
                            <div id="txt_container_Transact_Main_l" style="width: 167px;">
                                <div id="tag_label_transact_Src" style="width: 40px">
                                    Type
                                </div>
                                <div id="txtcon-m_Exchange" style="width: 125px;">
                                    <asp:DropDownList ID="ddlType" class="chosen-select-deselect" Style="width: 120px;"
                                        data-placeholder="Choose Type"  runat="server" TabIndex="35"
                                       onchange="Job_Details();">
                                       
                                    </asp:DropDownList>
                                </div>
                            </div>

                            <div id="txt_container_Transact_Main_l" style="width: 105px;margin-top:3px">
                                <div id="txtcon-m_Exchange" style="width: 110px">
                                    <asp:RadioButtonList ID="Rd_Mode" runat="server" RepeatDirection="Horizontal"
                                        ForeColor="Blue" Font-Bold="true" TabIndex="34" onchange="Job_Details();">
                                        <asp:ListItem Text="Air" Value="Air" Selected="True"></asp:ListItem>
                                        <asp:ListItem Text="Sea" Value="Sea"></asp:ListItem>
                                    </asp:RadioButtonList>
                                </div>
                            </div>

                              

                            <div id="txt_container_Transact_Main_l" style="width: 220px;">
                                <div id="tag_label_transact_Src" style="width: 50px">
                                    Jobno
                                </div>
                                <div id="txtcon-m_Exchange" style="width: 160px;">


                                <asp:DropDownList ID="ddljobno" runat="server" 
                                                     CssClass="listtxt_transac_mode_new" 
                                                     ClientIDMode="Static" 
                                              TabIndex="35" Width="160px" onchange="customer_details();Charge_Head();">
                                                         </asp:DropDownList>



                               <%--     <asp:DropDownList ID="ddljobno" class="chosen-select-deselect" Style="width: 160px;"
                                        data-placeholder="Choose a Jobno" AutoPostBack="true" runat="server" TabIndex="35"
                                        OnSelectedIndexChanged="ddljobno_SelectedIndexChanged">
                                    </asp:DropDownList>--%>
                                </div>
                            </div>
                            <div id="txt_container_Transact_Main_l" style="width:135px;">
                                <div id="tag_label_transact_Src" style="width: 60px;color:Gray;">
                                    Job Date :
                                </div>
                                <div id="txtcon-m_Exchange" style="width: 75px; ">
                                    <%--<asp:TextBox ID="txt_JobDate" runat="server" Width="80px" MaxLength="10" CssClass="validate[required] txtbox_none_Mid_transac_code"
                                        TabIndex="37"></asp:TextBox>
                                    <asp:MaskedEditExtender ID="MaskedEditExtender2" runat="server" TargetControlID="txt_JobDate"
                                        Mask="99/99/9999" MaskType="Date" ErrorTooltipEnabled="True" CultureAMPMPlaceholder=""
                                        CultureCurrencySymbolPlaceholder="" CultureDateFormat="" CultureDatePlaceholder=""
                                        CultureDecimalPlaceholder="" CultureThousandsPlaceholder="" CultureTimePlaceholder=""
                                        Enabled="True">
                                    </asp:MaskedEditExtender>
                                    <asp:CalendarExtender ID="CalendarExtender3" runat="server" TargetControlID="txt_JobDate"
                                        Format="dd/MM/yyyy" Enabled="True">
                                    </asp:CalendarExtender>--%>
                                     <span style="color:Maroon;font-size:15px">    <asp:label ID="lbl_Jobdate" runat="server"></asp:label> </span> 
                                </div>
                            </div>
                            <div id="txt_container_Transact_Main_l" style="width: 400px;">
                                <div id="tag_label_transact_Src" style="width: 100px; color:Gray;">
                                    Customer Name:
                                </div>
                                <div id="txtcon-m_Exchange" style="width: 300px;">
                                <span style="color:Maroon;font-size: smaller;">    <asp:label ID="lbl_Cus_Name" runat="server"></asp:label> </span>
                                </div>
                            </div>
                        </div>
                        <div id="tag_transact_lft_in1" style="width: 1280px; height: 35px;">
                         <div id="tag_label_transact_Src" style="width: 90px;">
                                        Charge Head
                                    </div>
                                    <div id="txtcon-m_Exchange" style="width: 200px;">
                                        <asp:DropDownList ID="ddl_chargehead" CssClass="listtxt_transac_mode_new" 
                                                     ClientIDMode="Static"  Style="width: 200px;"
                                            data-placeholder="Choose a Charge Head" onchange="Chargedown();Charge_Head_Amt();"
                                            runat="server" TabIndex="10">
                                        </asp:DropDownList>
                                    </div>
                                    <div id="txt_container_Transact_Main_l" style="width: 200px;">
                                    </div>
                            <div id="txt_container_Transact_Main_l" style="width: 200px;">
                                <div id="tag_label_transact_Src" style="width: 100px; color: gray; font-weight: bold;">
                                    Purchase Amt :
                                </div>
                                <div id="txtcon-m_Exchange" style="width: 100px;">
                                     <asp:TextBox ID="lbl_Purchase_Amt" runat="server" Width="90px" Text="0" ForeColor="Brown" Font-Bold="true"
                                        Style="border: 0px none;" MaxLength="50" TabIndex="47"></asp:TextBox>
                                </div>
                            </div>

                            <div id="txt_container_Transact_Main_l" style="width: 250px;">
                                <div id="tag_label_transact_Src" style="width: 130px;color: gray; font-weight: bold;">
                                  Already Paid Amt :
                                </div>
                                <div id="txtcon-m_Exchange" style="width: 100px;">
                                     <asp:TextBox ID="lbl_Already_Paid_Amt" runat="server"  Text="0" Width="90px" ForeColor="Brown" Font-Bold="true"
                                        Style="border: 0px none;" MaxLength="50" TabIndex="47"></asp:TextBox>
                                </div>
                            </div>

                            <div id="txt_container_Transact_Main_l" style="width: 200px;">
                                <div id="tag_label_transact_Src" style="width: 100px; color:gray; font-weight: bold;">
                                    Balance Amt :
                                </div>
                                <div id="txtcon-m_Exchange" style="width: 100px;">
                                     <asp:TextBox ID="lbl_Balance_Amt" runat="server"  Text="0" Width="90px" ForeColor="Brown" Font-Bold="true"
                                        Style="border: 0px none;" MaxLength="50" TabIndex="47"></asp:TextBox>
                                </div>
                            </div>
                            
                            
                        </div>
                        <div id="tag_transact_lft_in1" style="width: 1280px; height: 25px;">

                        <div id="txt_container_Transact_Main_l" style="width: 200px;">
                                <div id="tag_label_transact_Src" style="width: 100px;  font-weight: bold;">
                                    Payable Amt :
                                </div>
                                <div id="txtcon-m_Exchange" style="width: 100px;">
                                        
                                        <asp:TextBox ID="txt_Payable_Amt" Width="90px" Font-Bold="true" ForeColor="Maroon"
                                        TabIndex="42" onblur="extractNumber(this,2,false);" onkeyup="extractNumber(this,2,false);return Clear_Dot(this);"
                                        onkeypress="return blockNonNumbers(this, event, true, false);" autocomplete="off"
                                        CssClass="txtbox_none_Mid_transac_code" runat="server"></asp:TextBox>
                                </div>
                            </div>

                            <div id="txt_container_Transact_Main_l" style="width: 250px;">
                                <div id="tag_label_transact_Src" style="width: 130px; font-weight: bold;">
                                  Write off Amt :
                                </div>
                                <div id="txtcon-m_Exchange" style="width: 100px;">
                                        <asp:TextBox ID="txt_Write_Off_Amt" Width="90px" Font-Bold="true" ForeColor="Maroon"
                                        TabIndex="42" onblur="extractNumber(this,2,false);" onkeyup="extractNumber(this,2,false);return Clear_Dot(this);"
                                        onkeypress="return blockNonNumbers(this, event, true, false);" autocomplete="off"
                                        CssClass="txtbox_none_Mid_transac_code" runat="server"></asp:TextBox>
                                </div>
                            </div>
                            <%--<div id="txt_container_Transact_Main_l" style="width: 480px;">
                                <div id="tag_label_transact_Src" style="width: 100px;">
                                    Remarks
                                </div>
                                <div id="txtcon-m_Exchange" style="width: 360px;">
                                <asp:TextBox ID="txt_Write_Off_Amt_Remarks"  runat="server"  
                                          Width="350px"  TabIndex="23" 
                                         MaxLength="50" CssClass="txtbox_none_Mid_transac_code" ></asp:TextBox>
                                </div>
                            </div>--%>
                            <div id="txt_container_Transact_Main_l" style="width: 480px;">
                                <div id="tag_label_transact_Src" style="width: 100px;">
                                    Remarks
                                </div>
                                <div id="txtcon-m_Exchange" style="width: 360px;">
                                <asp:TextBox ID="txt_Write_Off_Amt_Remarks"  runat="server"  
                                          Width="350px"  TabIndex="23" 
                                         MaxLength="50" CssClass="txtbox_none_Mid_transac_pop_gen_srcic" ></asp:TextBox>
                                </div>
                            </div>


                            
                        </div>
                        <div id="tag_transact_lft_in1" style="width: 1280px">
                            <div id="txt_container_Transact_Main_l" style="width: 570px;">
                                <div id="tag_label_transact_Src" style="width: 100px;">
                                </div>
                                <div id="txtcon-m_Exchange" style="width: 460px;">
                                </div>
                            </div>
                            <div id="txt_container_Transact_Main_l" style="width: 200px;">
                                <div id="tag_label_transact_Src" style="width: 80px">
                                </div>
                                <div id="txtcon-m_Exchange" style="width: 110px;">
                                </div>
                            </div>
                            <div id="txt_container_Transact_Main_l" style="width: 500px;">
                                <div id="tag_label_transact_Src" style="width: 120px;">
                                </div>
                                <div id="txtcon-m_Exchange" style="width: 130px;">
                                </div>
    <div id="tag_label_transact_Src" style="width: 100px;">
    </div>
    <div id="txtcon-m_Exchange" style="width: 120px;">
    </div>
    </div> </div>
    <div id="innerbox_MidMain_bot_transact" style="margin-top: 0px">
        <div id="innerbox_transac_bot_inn" style="width: 500px;">
            <div id="newbu">
                <asp:Button ID="btnNew" runat="server" TabIndex="54" CssClass="new" OnClick="btnNew_Click"
                    OnClientClick="jQuery('#form1').validationEngine('hide')" CausesValidation="false"
                    UseSubmitBehavior="false" />
            </div>
            <div id="editbu">
                <asp:Button ID="btnSave" runat="server" CssClass="save" OnClick="btnSave_Click" CommandName="s"
                    OnClientClick="return validate();" TabIndex="51" />
            </div>
            <div id="editbu">
                <asp:Button ID="btnUpdate" runat="server" CssClass="updates" ValidationGroup="dt"
                    OnClick="btnUpdate_Click" Visible="false" TabIndex="52" OnClientClick="return validation();" />
            </div>
            <div id="editbu">
                <asp:Button ID="btnDelete" runat="server" Visible="false" TabIndex="53" CausesValidation="false"
                    UseSubmitBehavior="false" CssClass="dlete" OnClientClick="jQuery('#form1').validationEngine('hideAll');jConfirm('Delete this Entry?', 'Payment', function(r) {
                  var i = r + 'ok';
                  if(i == 'trueok')
                  {
                      document.getElementById('btn').click();
            
                  }
                  else {
                  }
                    });return false;" />
                <asp:Button ID="btn" runat="server" TabIndex="53" CausesValidation="false" UseSubmitBehavior="false"
                    OnClientClick="jQuery('#form1').validationEngine('hideAll');" OnClick="btnDelete_Click"
                    CssClass="dlete" Style="display: none;" />
            </div>
            <div id="editbu">
                
            </div>
        </div>
    </div>
    </div> </div> </div>
    <div id="pop_text_area_transac_popup_inn_container_IGM_gateway_grid" style="margin-top: 0px;
        margin-left: 3px; width: 1290px;">
        <div id="pop_text_area_transac_popup_inn_container_IGM_Container2_grid_gateway" runat="server"
            style="overflow: auto; height: 232px; width: 1290px;">
            <asp:GridView ID="gv_Ch" runat="server" DataKeyNames="PC_ID,PAYMENT_NO" EmptyDataText="NO RECORD FOUND"
                AutoGenerateColumns="False" BackColor="WhiteSmoke" CssClass="grid-view" Width="100%"
                ShowHeaderWhenEmpty="True" PageSize="5" AllowPaging="True" OnRowDataBound="gv_Ch_RowDataBound"
                OnSelectedIndexChanged="gv_Ch_SelectedIndexChanged" AllowSorting="True" BorderColor="#C8C8C8"
                BorderStyle="Solid" BorderWidth="1px" CellPadding="1" CellSpacing="1" OnRowCreated="gv_Ch_RowCreated">
                <AlternatingRowStyle BackColor="White" BorderColor="#C8C8C8" BorderStyle="Solid"
                    BorderWidth="1px" />
                <Columns>
                    <asp:CommandField ShowSelectButton="True">
                        <HeaderStyle CssClass="hideGridColumn" />
                        <ItemStyle CssClass="hideGridColumn" />
                    </asp:CommandField>
                    <asp:BoundField DataField="SLNO" HeaderText="S.No">
                        <ItemStyle CssClass="column_style_left5" Width="50px" />
                    </asp:BoundField>
                    <asp:BoundField DataField="IMP_EXP" HeaderText="Imp/Exp">
                        <ItemStyle CssClass="column_style_left5" Width="50px" />
                    </asp:BoundField>
                    <asp:BoundField DataField="JOBNO" HeaderText="Jobno">
                        <ItemStyle CssClass="column_style_left5" Width="50px" />
                    </asp:BoundField>
                    <asp:BoundField DataField="JOB_DATE" HeaderText="Jobdate">
                        <ItemStyle CssClass="column_style_left5" Width="50px" />
                    </asp:BoundField>
                    <asp:BoundField DataField="CUSTOMER_NAME" HeaderText="Customer Name">
                        <ItemStyle CssClass="column_style_left5" Width="200px" />
                    </asp:BoundField>
                    <asp:BoundField DataField="PAYABLE_AMT" HeaderText="Payable Amt">
                        <ItemStyle CssClass="column_style_left5" Width="100px" />
                    </asp:BoundField>
                    <asp:BoundField DataField="WRITE_OFF_AMT" HeaderText="Write off Amt">
                        <ItemStyle CssClass="column_style_left5" Width="50px" />
                    </asp:BoundField>
                    <asp:BoundField DataField="REMARKS" HeaderText="">
                        <HeaderStyle CssClass="hideGridColumn" />
                        <ItemStyle CssClass="hideGridColumn" />
                    </asp:BoundField>
                    <asp:BoundField DataField="CHARGE_NAME" HeaderText="">
                    <HeaderStyle CssClass="hideGridColumn" />
                     <ItemStyle CssClass="hideGridColumn" />
                    </asp:BoundField>

                </Columns>
                <EmptyDataTemplate>
                    NO RECORD FOUND
                </EmptyDataTemplate>
            </asp:GridView>
        </div>
    </div>
    </div>
                    </div>

                         <div  class="content"  id="page-7"  style="display:none;height:440px;" >
                        
                            <style type="text/css">
                                      input.underlined
                                        {
                                           border:solid 1px #000;
                                        }
                                        </style>
                             <div id="tag_transac_lft_Item_maindet_Grid_area" style="overflow: auto; height: 305px;
                                 width: 1094px; margin-top: 10px; margin-left: -18px;">
                                 <asp:GridView runat="server" ID="gvPaymentdetails" EmptyDataText="NO RECORD FOUND"
                                     AutoGenerateColumns="true" CssClass="grid-view" ShowHeader="true" ShowHeaderWhenEmpty="false"
                                     Width="100%"   OnRowDataBound="gvPaymentdetails_RowDataBound"
                                     CellPadding="1" CellSpacing="1" OnRowCreated="gvAll_RowCreated">
                                     <AlternatingRowStyle BackColor="White" BorderColor="#C8C8C8" BorderStyle="Solid"
                                         CssClass="column_style_left" BorderWidth="1px" />
                                     <HeaderStyle Font-Underline="false" ForeColor="Black" />
                                     <Columns>
                                         <asp:TemplateField HeaderText="S.No" HeaderStyle-Width="45px">
                                             <ItemTemplate>
                                                 <%#Container.DataItemIndex+1 %>
                                             </ItemTemplate>
                                         </asp:TemplateField>
                                     </Columns>
                                 </asp:GridView>



                             <asp:TextBox ID="txtfocus" Width="1px" MaxLength="1" runat="server" BorderColor="White" BorderStyle="None"
                                ForeColor="White" ></asp:TextBox>
                                 
                                  <p id="pTotal" style="color:Red; font-size:large;">
                            </div>
 
     <div id="tag_transact_lft_in1" style="width: 1150px">
                            <div id="txt_container_Transact_Main_l" style="width: 290px;">
                                <div id="tag_label_transact_Src" style="width: 150px;">
                                    Total Bill Amt Received:
                                </div>
                                <div id="txtcon-m_Exchange" style="width: 120px">
                                   <asp:TextBox ID="txtTotBillAmt" runat="server"   BackColor="#F5F5F5" Width="120px" 
                                    CssClass="txtbox_none_Mid_transac_Inv_No" TabIndex="100" ReadOnly="true" ></asp:TextBox>
                                </div>
                            </div>
                           <div id="txt_container_Transact_Main_l"   style="width: 250px;">
                                <div id="tag_label_transact_Src" style="width: 135px">
                                  Total TDS:
                                </div>
                                <div id="txtcon-m_Exchange" style="width: 80px">
                                    <asp:TextBox ID="txtTotTDSAmt" runat="server" BackColor="#F5F5F5" Width="100px" 
                                    CssClass="txtbox_none_Mid_transac_Inv_No"   ReadOnly="true" ></asp:TextBox>
                                </div>
                            </div>
                              <div id="txt_container_Transact_Main_l" style="width: 280px">
                                <div id="tag_label_transact_Src" style="width: 155px">
                                 Total Received:
                                </div>
                                <div id="txtcon-m_Exchange" style="width: 120px">
                                    <asp:TextBox ID="txtFinalReceivedAmt" Width="100px"  runat="server"  ReadOnly="true"  
                                    CssClass="txtbox_none_Mid_transac_Inv_No" ></asp:TextBox>
                                     
                                </div>
                            </div>
                             <div id="txt_container_Transact_Main_l" style="width: 280px">
                                <div id="tag_label_transact_Src" style="width: 155px">
                                 Total WriteOff:
                                </div>
                                <div id="txtcon-m_Exchange" style="width: 120px">
                                    <asp:TextBox ID="txtTotWriteOff" Width="100px"  runat="server"   CssClass="txtbox_none_Mid_transac_Inv_No"  ReadOnly="true" ></asp:TextBox>
                                     
                                </div>
                            </div>
                        
                        </div>
                    </div>

                </div>
            </div>
            
            </div>

            <script src="../activatables.js" type="text/javascript"></script>
                <script type="text/javascript">
                    activatables('page', ['page-6', 'page-7', 'page-8']);
            </script>
            <asp:HiddenField ID="HDupdate_id" runat="server" />
            <asp:HiddenField ID="HD_Ch_update_id" runat="server" />
              <asp:HiddenField ID="hdnvenbranch" runat="server" />
               <asp:HiddenField ID="hdnbankacc" runat="server" />
                <asp:HiddenField ID="hdnbankifsc" runat="server" />
                <asp:HiddenField ID="hdnCharge" runat="server" />
                <asp:HiddenField ID="hdncusnam" runat="server" />
                <asp:HiddenField ID="hdncusdate" runat="server" />
            <asp:HiddenField ID="Hd_row_id" runat="server" />
             <asp:HiddenField ID="hdnddljob" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
    </form>
    <script src="../js/Billing/GST/Imp_auto_Search.js" type="text/javascript"></script>
    <script type="text/javascript" src="../windowfiles/dhtmlwindow.js"></script>
    <script type="text/javascript" src="../modalfiles/modal.js"></script>
    <script type="text/javascript" src="../js/slide.js"></script>
    <script type="text/javascript" src="../js/jscolor.js"></script>

    <script type="text/javascript" src="../js/Accounts/Acc_iframepopupwin.js"></script>


    <script src="../js/checkboxJScript.js" type="text/javascript"></script>
    <script src="../js/listpopup.js" type="text/javascript"></script>
    <script src="../js/Validation.js" type="text/javascript"></script>
    <script src="../AutoComplete_JS/jquery.min.js" type="text/javascript"></script>
    <script src="../AutoComplete_JS/jquery-ui.min.js" type="text/javascript"></script>
    <script src="../Validation Files/jquery_002.js" type="text/javascript" charset="utf-8">
    </script>
    <script src="../Validation Files/jquery.js" type="text/javascript" charset="utf-8">
    </script>
    <script src="../Validation%20Files/Ascii.js" type="text/javascript"></script>
    <script src="../js/Billing/BillingValid.js" type="text/javascript"></script>
    <!-- VALIDATION SCRIPT -->
    <script type="text/javascript">
        jQuery(document).ready(function () {
            jQuery("#form1").validationEngine();
        });


        function close_button() {
            document.getElementById("General").click();
        }
        








    </script>



   <link rel="Stylesheet" href="../Dropdown/chosen.css" />
    <link rel="Stylesheet" href="../Dropdown/chosen.min.css" />
    <script src="../Dropdown/chosen.jquery.js" type="text/javascript"></script>
    <script src="../Dropdown/chosen.jquery.min.js" type="text/javascript"></script>
    <script src="../Dropdown/chosen.proto.js" type="text/javascript"></script>
    <script src="../Dropdown/chosen.proto.min.js" type="text/javascript"></script>

     <script type="text/javascript">
         var config = {
             '.chosen-select': {},
             '.chosen-select-deselect': { allow_single_deselect: true },
             '.chosen-select-no-single': { disable_search_threshold: 10 },
             '.chosen-select-no-results': { no_results_text: 'Oops, nothing found!' },
             '.chosen-select-width': { width: "95%" }
         }
         for (var selector in config) {
             $(selector).chosen(config[selector]);
         }
        </script>


        
     <script type="text/javascript">

         function Vendor_Details() {

             var Vendor = document.getElementById("ddlVendorname").value;


             PageMethods.Get_Vendor_Det(Vendor, CallSuccess_Ven_Det);

         }


         function CallSuccess_Ven_Det(res) {

             document.getElementById("ddlVendorBranch").options.length = 0;
             var dropdown = document.getElementById("ddlVendorBranch");


             var data = res;

             var c = data.split(',');

            
             var j = c.length - 1;
             

             for (var i = 0; c[i] > c[j]; i++) {
                 
                 dropdown[dropdown.length] = new Option(c[i], c[i])
               
             }

         }



         function Vendor_Branch() {
             var VendorBranch = document.querySelector('#ddlVendorBranch').value;

            
             document.getElementById('<%=hdnvenbranch.ClientID %>').value = VendorBranch;
         }




         function Bank_Details() {

             var Bank = document.getElementById("ddl_Bank_From").value;


             PageMethods.Get_Bank_Det(Bank, CallSuccess_Bank_Det);

         }


         function CallSuccess_Bank_Det(res) {

             document.getElementById("ddl_Acc_No").options.length = 0;
             var dropdown = document.getElementById("ddl_Acc_No");


             var data = res;

             var c = data.split(',');

            
             var j = c.length - 1;
            

             for (var i = 0; c[i] > c[j]; i++) {

                 dropdown[dropdown.length] = new Option(c[i], c[i])

             }

         }



         function Bank_Acc() {
             var Bankacc = document.querySelector('#ddl_Acc_No').value;

          
             document.getElementById('<%=hdnbankacc.ClientID %>').value = Bankacc;
         }




         function IFSC_Details() {

             var Bank = document.getElementById("ddl_Bank_From").value;
             var Acc_no = document.getElementById("ddl_Acc_No").value;

             PageMethods.Get_Ifsc_Det(Bank,Acc_no,CallSuccess_ifsc_Det);

         }


         function CallSuccess_ifsc_Det(res) {

             document.getElementById("ddl_ifsc_Code").options.length = 0;
             var dropdown = document.getElementById("ddl_ifsc_Code");


             var data = res;

             var c = data.split(',');

          
             var j = c.length - 1;
        

             for (var i = 0; c[i] > c[j]; i++) {

                 dropdown[dropdown.length] = new Option(c[i], c[i])

             }

         }

         function Bank_Ifsc() {
             var bankifsc = document.querySelector('#ddl_ifsc_Code').value;
             

             document.getElementById('<%=hdnbankifsc.ClientID %>').value = bankifsc;
         }



         function Job_Details() {
             var Vendor = document.getElementById("ddlVendorname").value;
             var Vendor_Branch = document.getElementById("ddlVendorBranch").value;
             var Payment_no = document.getElementById("txtPaymentNo").value;
             var P_ID = document.getElementById('<%=HDupdate_id.ClientID %>').value;
             var Imp_Exp = $('input:radio[name=Rd_IMP_EXP]:checked').val();
             var Mode = $('input:radio[name=Rd_Mode]:checked').val();
             var Type = document.getElementById("ddlType").value;

             PageMethods.Get_Job_Det(Vendor, Vendor_Branch, Payment_no, P_ID,Imp_Exp, Mode, Type, CallSuccess_Job_Det);

         }


         function CallSuccess_Job_Det(res) {

             document.getElementById("ddljobno").options.length = 0;
             var dropdown = document.getElementById("ddljobno");


             var data = res;
            
             var c = data.split(',');


             var j = c.length - 1;


             for (var i = 0; c[i] > c[j]; i++) {

                 dropdown[dropdown.length] = new Option(c[i], c[i])

             }

         }

         




         function customer_details() {
             var Vendor = document.getElementById("ddlVendorname").value;
             var Jobno = document.getElementById("ddljobno").value;
             
             var Vendor_Branch = document.getElementById("ddlVendorBranch").value;
             var Payment_no = document.getElementById("txtPaymentNo").value;
             var P_ID = document.getElementById('<%=HDupdate_id.ClientID %>').value;
             var Imp_Exp = $('input:radio[name=Rd_IMP_EXP]:checked').val();
             var Mode = $('input:radio[name=Rd_Mode]:checked').val();
             var Type = document.getElementById("ddlType").value;

             PageMethods.Get_Cus_Det(Vendor, Vendor_Branch, Payment_no, P_ID, Imp_Exp, Mode, Type,Jobno, CallSuccess_Cus_Det);

         }


         function CallSuccess_Cus_Det(res) {

           
             var data = res;


             var s = data.split(',');
             var cus_name = s[0];
             var date = s[1];
//             var purchase_amt = s[2];
//             var Already_paid = s[3];
//             var bal = s[4];
             document.getElementById('lbl_Cus_Name').innerText = cus_name;
             document.getElementById('<%=hdncusnam.ClientID %>').value = cus_name;
             document.getElementById('lbl_Jobdate').innerHTML = date;

             document.getElementById('<%=hdncusdate.ClientID %>').value = date;

//             document.getElementById('lbl_Purchase_Amt').value = purchase_amt;
//             document.getElementById('lbl_Already_Paid_Amt').value = Already_paid;
//             document.getElementById('lbl_Balance_Amt').value = bal;
             document.getElementById('<%=hdnddljob.ClientID %>').value = document.getElementById("ddljobno").value;
         }




         function customer_date_details() {
             var Vendor = document.getElementById("ddlVendorname").value;
             var Jobno = document.getElementById("ddljobno").value;
             
             var Vendor_Branch = document.getElementById("ddlVendorBranch").value;
             var Payment_no = document.getElementById("txtPaymentNo").value;
             var P_ID = document.getElementById('<%=HDupdate_id.ClientID %>').value;
             var Imp_Exp = $('input:radio[name=Rd_IMP_EXP]:checked').val();
             var Mode = $('input:radio[name=Rd_Mode]:checked').val();
             var Type = document.getElementById("ddlType").value;

             PageMethods.Get_Cus_date_Det(Vendor, Vendor_Branch, Payment_no, P_ID, Imp_Exp, Mode, Type,Jobno, CallSuccess_Cus_Date_Det);

         }


         function CallSuccess_Cus_Date_Det(res) {



             var data = res;
       
             document.getElementById('lbl_Jobdate').innerHTML = data;
             

             
         }






         function Amount_Cal() {
             var Vendor = document.getElementById("ddlVendorname").value;
             var Jobno = document.getElementById("ddljobno").value;

             var Vendor_Branch = document.getElementById("ddlVendorBranch").value;
             var Payment_no = document.getElementById("txtPaymentNo").value;
             var P_ID = document.getElementById('<%=HDupdate_id.ClientID %>').value;
             var Imp_Exp = $('input:radio[name=Rd_IMP_EXP]:checked').val();
             var Mode = $('input:radio[name=Rd_Mode]:checked').val();
             var Type = document.getElementById("ddlType").value;

             PageMethods.Get_Amt_cal(Vendor, Vendor_Branch, Payment_no, P_ID, Imp_Exp, Mode, Type, Jobno, CallSuccess_Amt_cal);

         }


         function CallSuccess_Amt_cal(res) {



             var data = res;

             document.getElementById('lbl_Jobdate').innerHTML = data;



         }



         window.onload = Mode_of_transactions;
         function Mode_of_transactions() {

             var Mode_of_tran = $('input:radio[name=Rd_Mode_Of_Payment]:checked').val();
            

             if (Mode_of_tran == 'CA') {

                 document.getElementById("lbl_Chk_Nb_refno").style.display = 'none';
                 document.getElementById("lbl_NB").style.display = 'none';

              
//                 lbl_Chk_Nb_refno.Visible = false;
//                 lbl_NB.Visible = false;
             }
             else if (Mode_of_tran == 'CH') {

           
                 document.getElementById("lbl_Chk_Nb_refno").style.display = 'block';
                 document.getElementById("lbl_NB").style.display = 'none';

               

//                 lbl_Chk_Nb_refno.Visible = true;
//                 lbl_NB.Visible = false;
             }
             else if (Mode_of_tran == 'NB') {


                 document.getElementById("lbl_Chk_Nb_refno").style.display = 'block';
                 document.getElementById("lbl_NB").style.display = 'block';

          

//                 lbl_Chk_Nb_refno.Visible = true;
//                 lbl_NB.Visible = true;
             }

         }



         </script>




<script type="text/javascript">
    function Charge_Head() {
        var Vendor = document.getElementById("ddlVendorname").value;
        var Jobno = document.getElementById("ddljobno").value;
        var Vendor_Branch = document.getElementById("ddlVendorBranch").value;
        var Payment_no = document.getElementById("txtPaymentNo").value;
        var P_ID = document.getElementById('<%=HDupdate_id.ClientID %>').value;
        var Imp_Exp = $('input:radio[name=Rd_IMP_EXP]:checked').val();
        var Mode = $('input:radio[name=Rd_Mode]:checked').val();
        var Type = document.getElementById("ddlType").value;
        PageMethods.Get_charge_head(Vendor, Vendor_Branch, Payment_no, P_ID, Imp_Exp, Mode, Type, Jobno, CallSuccess_charge_head);


    }

    function CallSuccess_charge_head(res) {

        document.getElementById("ddl_chargehead").options.length = 0;
        var dropdown = document.getElementById("ddl_chargehead");


        var data = res;

        var c = data.split(',');


        var j = c.length - 1;


        for (var i = 0; c[i] > c[j]; i++) {

            dropdown[dropdown.length] = new Option(c[i], c[i])

        }

    }

    function Chargedown() {
        var Charge_Name = document.querySelector('#ddl_chargehead').value;

        Branchdropdown(Charge_Name);
    }
    function Branchdropdown(a) {

        document.getElementById('<%=hdnCharge.ClientID %>').value = a;
    }

</script>
<script type="text/javascript">
    function Charge_Head_Amt() {
        var Vendor = document.getElementById("ddlVendorname").value;
        var Jobno = document.getElementById("ddljobno").value;
        var Vendor_Branch = document.getElementById("ddlVendorBranch").value;
        var Payment_no = document.getElementById("txtPaymentNo").value;
        var P_ID = document.getElementById('<%=HDupdate_id.ClientID %>').value;
        var Imp_Exp = $('input:radio[name=Rd_IMP_EXP]:checked').val();
        var Mode = $('input:radio[name=Rd_Mode]:checked').val();
        var Type = document.getElementById("ddlType").value;
        var Charge = document.getElementById("ddl_chargehead").value;

        PageMethods.Get_charge_head_Amt(Vendor, Vendor_Branch, Payment_no, P_ID, Imp_Exp, Mode, Type, Jobno,Charge, CallSuccess_charge_head_Amt);


    }
    function CallSuccess_charge_head_Amt(res) {

        var data = res;
        var s = data.split(',');
//        var cus_name = s[0];
//        var date = s[1];
        var purchase_amt = s[0];
        var Already_paid = s[1];
        var bal = s[2];
//        document.getElementById('lbl_Cus_Name').innerText = cus_name;
//        document.getElementById('<%=hdncusnam.ClientID %>').value = cus_name;
//        document.getElementById('lbl_Jobdate').innerHTML = date;
//        document.getElementById('<%=hdncusdate.ClientID %>').value = date;

                     document.getElementById('lbl_Purchase_Amt').value = purchase_amt;
                     document.getElementById('lbl_Already_Paid_Amt').value = Already_paid;
                     document.getElementById('lbl_Balance_Amt').value = bal;
//        document.getElementById('<%=hdnddljob.ClientID %>').value = document.getElementById("ddljobno").value;
    }

</script>





</body>
</html>

