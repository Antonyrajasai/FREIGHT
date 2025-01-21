<%@ Page Language="C#" AutoEventWireup="true" CodeFile="GenerateInvoice.aspx.cs" Inherits="Accounts_GeneratrInvoice" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <link href="../css/MainStyle.css" rel="stylesheet" media="screen, projection" type="text/css" />
    <link href="../main.css" rel="stylesheet" media="screen, projection" type="text/css" />
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
    <style type="text/css">
        /* Autocomplete Textbox Search Loading Image*/
        .ui-autocomplete-loading
        {
            background: white url('../images/ui-anim_basic_16x16.gif') right center no-repeat;
        }
        /* Autocomplete Textbox Search Loading Image*/
    </style>
    <script type="text/javascript">

        function HideModalPopup_U() {

            $find("MP_U").hide();
            return false;
        }
       
    </script>
    <script type="text/javascript">
        function ShowModalPopup() {
            $('#txtid').val('');
            $find("MP_JOB").show();
            return false;
        }
        function HideModalPopup() {
            $('#txtid').val('');
            $find("MP_JOB").hide();
            return false;
        }

        function validate() {
         var txtQuantity = document.getElementById("txtQuantity").value;
    if (txtQuantity.length <= 0) {
        jAlert('Enter All Mandatory Field', 'General1', function (r) { tabclick("Contain", "txtQuantity"); });
        return false;
    }
  
    </script>
    <style type="text/css">
        .modalPopup12
        {
            background-color: transparent;
            width: 940px;
            height: 180px;
        }
    </style>
    <script src="../MessageBox_js/jquery.js" type="text/javascript"></script>
    <link href="../MessageBox_js/jquery.alerts.css" rel="stylesheet" type="text/css" />
    <script src="../MessageBox_js/jquery.alerts.js" type="text/javascript"></script>
</head>
<body style="overflow: hidden;">
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePageMethods="true" EnablePartialRendering="false"
        LoadScriptsBeforeUI="false">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional" Visible="true">
        <ContentTemplate>
            <div class="loading" align="center" id="load" style="display: none;">
                <img src="../Loading_Images/indicator_mozilla_blu.gif" alt="" />
            </div>
            <div id="innerbox_MidMain_Transact_popup_new" style="height: 110px; width: 1142px;">
                <div id="tag_srcinner1_new">
                    <div id="editbu">
                        <a href="#" class="edit"></a>
                    </div>
                    <div id="mainmastop2container_rght_tag2_txt1">
                        &nbsp;&nbsp;<span id="spancontrol" runat="server"></span>
                    </div>
                    <div id="mainmastop2container_rght_tag2_txt1" style="width: 120px; padding-left: 400px;
                        display: none">
                        <span id="lblTrkBENo" title="BE Status" runat="server" onclick="BE_Track();return false;"
                            style="color: Blue; text-decoration: underline; cursor: hand; font-size: smaller;">
                            Check BE Status</span>
                    </div>
                    <div id="popupwindow_closebut_right_new">
                        <asp:Button ID="btnClose" runat="server" CssClass="clsicon_new" OnClientClick="RefreshParent();return false;"
                            CausesValidation="false" UseSubmitBehavior="false" />
                    </div>
                </div>
                <div id="tag_transaction_inner_new" style="margin-left: 10px; width: 1130px;">
                    <div id="tag_Exchange_lft_in1_new1" style="width: 1130px;">
                        <div id="tag_label_transaction_date" style="width: 100px; margin-left: 20px">
                            Booking No   
                             </div>
                           <div id="txtcon-m_transaction_code" style="width: 170px;">
                            <asp:TextBox ID="txtjobps" runat="server" Font-Bold="true" MaxLength="50" Font-Size="12px"
                                CssClass="txtbox_none_Mid_transac_code" Width="157px" ReadOnly="True" TabIndex="1"
                                ClientIDMode="Static"></asp:TextBox>
                        </div>
                        
                        <div id="tag_label_transaction_date" style="width: 95px;">
                            Job Date
                        </div>
                        <div id="txtcon-m_transaction_code" style="width: 130px;">
                            <asp:TextBox ID="txtJobDate" runat="server" CssClass="txtbox_none_Mid_transac_code"
                                MaxLength="10" TabIndex="2"></asp:TextBox>
                            <cc1:MaskedEditExtender ID="MaskedEditExtender5" runat="server" TargetControlID="txtJobDate"
                                Mask="99/99/9999" MessageValidatorTip="true" OnInvalidCssClass="MaskedEditError"
                                MaskType="Date" DisplayMoney="Left" AcceptNegative="Left" ErrorTooltipEnabled="True">
                            </cc1:MaskedEditExtender>
                            <cc1:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtJobDate"
                                PopupButtonID="txtJobDate" Format="dd/MM/yyyy">
                            </cc1:CalendarExtender>
                        </div>
                        <div id="tag_label_transaction_Src_longlabel" style="width: 107px;">
                            Transport Mode</div>
                        <div id="txtcon-m_transaction_mode" style="width: 80px;">
                            <asp:DropDownList ID="ddlTransportMode" runat="server" CssClass="listtxt_transac_mode_new"
                                TabIndex="3" Width="100px">
                            </asp:DropDownList>
                        </div>
                         <div id="tag_label_transaction_popup_no_empty" style="width: 37px; margin-left: 2px;">
                            <%--<asp:ImageButton ID="btn_Tr_Mode_Change" ToolTip="Change Tr. Mode" Visible="false"
                                runat="server"  ImageUrl="~/images/icons/icon_edit.jpg"  Height="15px" Width="16px"
                                OnClientClick="jQuery('#form1').validationEngine('hideAll');open_Change_Mode(document.getElementById('txtJobNo').value); return false" /> --%>
                            <asp:ImageButton ID="btn_Tr_Mode_Change" ToolTip="Change Tr. Mode" Visible="false"
                                runat="server" ImageUrl="~/images/icons/icon_edit.jpg" Height="15px" Width="16px"
                                OnClientClick="jQuery('#form1').validationEngine('hideAll');open_Change_Mode('I'); return false" />
                        </div>
                        <div id="tag_label_transaction_Src" style="width: 75px;">
                            Inco Terms
                        </div>
                        <div id="txtcon-m_transaction_code" style="width: 110px;">
                            <asp:DropDownList ID="ddlIncoterms" runat="server" CssClass="listtxt_transac_mode_new"
                                Width="105px" TabIndex="4">
                            </asp:DropDownList>
                            <%--<asp:TextBox ID="txtBE_No" runat="server" MaxLength="10" CssClass="txtbox_none_Mid_transac_code"
                                ReadOnly="True" TabIndex="4" ClientIDMode="Static" Enabled="false" Visible="false"></asp:TextBox>--%>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ValidationGroup="draw"
                                ErrorMessage="*" ForeColor="Red" ControlToValidate="txtjobps"></asp:RequiredFieldValidator>
                            <%--<cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" TargetControlID="txtjobps"
                                FilterMode="InvalidChars" InvalidChars="qwertyuiopasdfghjklzxcvbnm[{]}`~,.<>/?:;'!@#$%^&*()_+|\-=\">
                            </cc1:FilteredTextBoxExtender>--%>
                        </div>
                        <div id="tag_label_transaction_Src" style="width: 80px;">
                            Freight
                        </div>
                        <div id="txtcon-m_transaction_code">
                            <%--<asp:TextBox ID="txtBE_Date" runat="server" MaxLength="10" CssClass="txtbox_none_Mid_transac_code"  Width="73px"
                                ReadOnly="True" TabIndex="5" ClientIDMode="Static" Enabled="false" Height="16px" Visible="false"  ></asp:TextBox>--%>
                            <asp:DropDownList ID="ddlFreight" runat="server" CssClass="listtxt_transac_mode_new"
                                Width="80px" TabIndex="5">
                            </asp:DropDownList>
                        </div>
                       
                    </div>
                    <div id="tag_label_transaction_popup_IGM2_empty_gen">
                    </div>
                    <div id="tag_Exchange_lft_in1_new1" style="width: 1130px;">
                        <div id="tag_label_transaction_Src_new" style="width: 270px; margin-left: 20px">
                            Planning Type &nbsp;&nbsp;
                            <asp:DropDownList ID="ddlType" runat="server" CssClass="listtxt_transac_mode_new"
                                Width="163px" TabIndex="6" AutoPostBack="true" OnSelectedIndexChanged="ddlBEType_SelectedIndexChanged">
                            </asp:DropDownList>
                            <asp:TextBox ID="txtJobNo" runat="server" MaxLength="50" Font-Size="12px" CssClass="txtbox_none_Mid_transac_code"
                                BorderColor="White" Width="0px" ReadOnly="True" TabIndex="1" ClientIDMode="Static"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server" ValidationGroup="draw"
                                ErrorMessage="*" ForeColor="Red" ControlToValidate="ddlType"></asp:RequiredFieldValidator>
                        </div>
                        <div id="tag_label_transaction_popup_IGM2_empty_Exbond" style="display: none;">
                            <a href="#" id="lnkbtn_Exbond" runat="server" style="display: none;" onclick="open_Exbond_Details(document.getElementById('txtjobps').value);return false;">
                                Ex</a>
                        </div>
                        
                        
                        <div id="tag_label_transaction_Src_longlabel" style="width: 95px;">
                            Booking Status</div>
                        <div id="txtcon-m_transaction_mode" style="width: 100px">
                            <asp:DropDownList ID="ddl_BookingStatus" runat="server" CssClass="listtxt_transac_mode_new"
                                TabIndex="7" Enabled="True" Width="100px">
                            </asp:DropDownList>
                        </div>
                        <div id="tag_label_transaction_popup_no_empty" style="width: 30px">
                        </div>
                        <div id="tag_label_transaction_Src" style="width: 107px;">
                            Reference No
                        </div>
                        <div id="txtcon-m_transaction_code">
                            <asp:TextBox ID="txtFile_Ref_No" runat="server" MaxLength="20" CssClass="txtbox_none_Mid_transac_code"
                                TabIndex="8" ClientIDMode="Static"></asp:TextBox>
                        </div>
                        <div id="tag_label_transaction_popup_no_empty" style="width: 20px">
                        </div>
                        <div id="tag_label_transaction_Src">
                            Reason
                        </div>
                        <div id="txtcon-m_transaction_code">
                             <asp:TextBox ID="txtReason" runat="server" MaxLength="20" CssClass="txtbox_none_Mid_transac_code"
                                TabIndex="9" ClientIDMode="Static"></asp:TextBox>
                        </div>
                        <div id="magnify_area">
                        </div>
                    </div>
                   
                    <div id="tag_label_transaction_popup_IGM2_empty" style="margin-top: 4px;display:none">
                        <div id="tag_label_transaction_date" style="width: 125px;display:none">
                          &nbsp;&nbsp;  Custom House</div>
                        <div id="txtcon-m_transaction_code" style="display:none">
                            <asp:TextBox ID="txtCustomsHouse" runat="server" CssClass="txtbox_none_Mid_transac_code"
                                MaxLength="6" TabIndex="7" ClientIDMode="Static"></asp:TextBox>
                            <asp:HiddenField ID="hdn_CustomsHouse_Name" runat="server" ClientIDMode="Static" />
                        </div>
                        </div>
                </div>
            </div>
            <div id="innerbox_MidMain_Trans_new" 
                style="height: 500px; width: 1150px; margin-top: 5px; padding-left: -2px;">
                <ol id="toc_new">
                    <li><a href="#page-1" onclick="tab_page1();" id="General1"><span>General</span></a></li>
                 
                    <div style="display: none">
                        <li><a href="#page-3" onclick="tab_page3();" id="INVOICE"><span></span></a></li>
                        <li><a href="#page-4" onclick="tab_page4();" id="ITEM"><span></span></a></li>
                        <li><a href="#page-5" onclick="tab_page5('../Report/Import_CheckList.aspx?jobno=',document.getElementById('txtjobps').value);"
                            id="Checklist"><span>Checklist</span></a></li>
                        <li><a href="#page-6" onclick="tab_page5('../FlatFile/Import_Flat_File_Generation.aspx?jobno=',document.getElementById('txtjobps').value);"
                            id="Submission" runat="server"><span>Submission 2.1</span></a> </li>
                        <li><a href="#page-7" onclick="tab_page6('../Job_History/Job_history.aspx?jobno=',document.getElementById('txtjobps').value,' &jobdate=',document.getElementById('txtJobDate').value,' &type=IMP')"
                            id="Jobhistry"><span>Job History</span></a> </li>
                        <li><a href="#page-8" visible="false" onclick="tab_page5('../FlatFile/E_Way_Bill_Generation.aspx?jobno=',document.getElementById('txtjobps').value);"
                            id="E_Way_Bill" runat="server"><span></span></a></li>
                        <div id="Imp_Billing_Others1" runat="server" visible="false">
                            <li><a href="#page-9" onclick="tab_page9();" id="Imp_Billing_Others"><span></span></a>
                            </li>
                        </div>
                        <div id="job_release" runat="server" visible="false">
                            <li><span style="margin-left: 340px;">
                                <asp:Button ID="JobRelease" runat="server" UseSubmitBehavior="false" OnClientClick="jQuery('#form1').validationEngine('hideAll')"
                                    CausesValidation="false" Text="Job Release" OnClick="JobRelease_Click" />
                            </span></li>
                        </div>
                        <div>
                            &nbsp;&nbsp;
                            <asp:TextBox ID="txt_FF_Jobno" placeholder="Fr. Jobno" Width="150px" runat="server"
                                MaxLength="50" CssClass="txtbox_none_Mid_transac_exp_invoice_addr" TabIndex="8"></asp:TextBox>
                        </div>
                    </div>
                </ol>
                <div class="content" id="page-1">
                    <div id="innerbox_MidMain_Trans_IGM" style="margin-top: -10px; margin-left: -17px;
                        width: 1144px; height: 390px">
                         <ol id="toc">
                           <li style="height: 20px"><a href="#page-10" onclick="tab_page10('0'); jQuery('#form1').validationEngine('hideAll')"
                                id="Sub_General"><span>General </span></a></li>
                            <li style="height: 20px"><a href="#page-11" onclick="tab_page11('0'); jQuery('#form1').validationEngine('hideAll')"
                                id="CONTAINER" runat="server"><span>Shipment Details</span></a></li>
                                 <%--<li><a href="#page-2" onclick="ta_page2(); " id="IGM" ><span>Cargo Dimensions</span></a></li>--%>
                        </ol>
                        <div class="content" id="page-10">
                            <div id="area_transac_container_genral">
                                <asp:Panel ID="Panel1" runat="server">
                                    <div id="pop_text_area_transac_popup_inn_container_new" style="width: 1120px">
                                        <div id="pop_text_area_transac_popup_inn_container_left_new" style="width: 550px;">
                                            <div id="tag_transac_lft_in1" style="width: 550px; height: 35px;">
                                                <div id="tag_label_transaction_popup_gen">
                                                    Liner Booking</div>
                                                <div id="txtcon-m_transaction_pop_gen1_srcic_new">
                                                    <asp:TextBox ID="txtLinerNo" runat="server" MaxLength="50" onblur="ChangeCase(this);"
                                                        Font-Bold="true" ClientIDMode="Static" CssClass="validate[required] txtbox_none_Mid_transac_pop_gen_srcic_new"
                                                        Onkeypress="return numcharspl1(event)" TabIndex="10"></asp:TextBox>
                                                </div>
                                                
                                               
                                            </div>
                                            <div id="tag_transac_lft_in1" style="width: 550px; height: 35px;">
                                                <div id="tag_label_transaction_popup_gen">
                                                    Booking Handled By</div>
                                                <div id="txtcon-m_transaction_pop_gen1_srcic_new">
                                                    <asp:TextBox ID="txtJobhandledby" runat="server" MaxLength="50" onblur="ChangeCase(this);"
                                                        Font-Bold="true" ClientIDMode="Static" CssClass="validate[required] txtbox_none_Mid_transac_pop_gen_srcic_new"
                                                        Onkeypress="return numcharspl1(event)" TabIndex="12"></asp:TextBox>
                                                </div>
                                                <div id="magnify_area">
                                                    <div id="ND" runat="server">
                                                       <a href="#" class="magnfy" onclick="jQuery('#form1').validationEngine('hideAll');ListPopupOpenPopupJobuser('Air'); return false"></a>
                                            
                                                    </div>
                                                    <div id="WD" runat="server">
                                                        <%--<a href="#" class="magnfy" onclick="jQuery('#form1').validationEngine('hideAll');OpenPopup_Imp_Exp_Ryal_Job('IMPORTER_DETAILS'); return false">
                                            </a>--%>
                                                    </div>
                                                </div>
                                                
                                                <%--   <a href="#" onclick="jQuery('#form1').validationEngine('hideAll');openclient_Masternew(); return false">
                                            New</a>--%>
                                            </div>
                                            
                                         
                                           
                                            <div id="tag_transac_lft_in1" style="width: 550px; height: 35px;">
                                                <div id="tag_label_transaction_popup_gen">
                                                    Place of Pickup</div>
                                                <div id="txtcon-m_transaction_pop_gen1_srcic_new">
                                                    <asp:TextBox ID="txtplaceofpickup" runat="server" MaxLength="50" onblur="ChangeCase(this);"
                                                        Font-Bold="true" ClientIDMode="Static" CssClass="txtbox_none_Mid_transac_pop_gen_srcic_new"
                                                        Onkeypress="return numcharspl1(event)" TabIndex="14"></asp:TextBox>
                                                </div>
                                                <div id="magnify_area">
                                                    <div id="CND3" runat="server">
                                                        
                                                        
                                                    </div>
                                                </div>
                                                
                                                
                                            </div>
                                            <div id="tag_transac_lft_in1" style="width: 550px; height: 35px;">
                                                <div id="tag_label_transaction_popup_gen">
                                                   Place of Origin</div>
                                                <div id="txtcon-m_transaction_pop_gen1_srcic_new">
                                                   <asp:TextBox ID="txtPortOrigin" runat="server" MaxLength="50" onkeypress="return char(event),TextBoxes(this);"
                                    onblur="ChangeCase(this);updateTextbox2();" onkeyup="ChangeCase(this);" onfocus="updateTextbox2();"
                                    onkeydown="var data = val;Port_origin(this);" onchange="updateTextbox2();" CssClass="txtbox_none_Mid_transac_pop_gen_srcic_new"
                                    ClientIDMode="Static" TabIndex="16"></asp:TextBox>
                                                </div>
                                              
                                                
                                              <div id="magnify_area" style="width: 40px">
                                                   
                                 <div id="CND13" runat="server">
                                                <a href="#" class="magnfy" onclick="jQuery('#form1').validationEngine('hideAll');OpenPopupPort_CountrySearch_Master(''); return false">
                                </a>
                                            </div>
                                           
                                                </div>
                                            
                                                
                                            </div>
                                            
                                            
                                            
                                        </div>
                                        <div id="pop_text_area_transac_popup_inn_container_rght_new" style="width: 475px;
                                            margin-right: 50px">
                                            
                                            <div id="tag_transac_lft_in1" style="width: 520px; height: 35px;">
                                                <div id="tag_label_transaction_popup_gen" style="width: 190px">
                                                    Liner Booking date
                                                </div>
                                                <div id="txtcon-m_transaction_pop_gen1_srcic_new">
                                                 <asp:TextBox ID="txtLinerbookingdate" runat="server" CssClass="validate[required] txtbox_none_Mid_transac_code"
                                MaxLength="10"  TabIndex="11" Width="285px"></asp:TextBox>
                            <cc1:MaskedEditExtender ID="MaskedEditExtender1" runat="server" TargetControlID="txtLinerbookingdate"
                                Mask="99/99/9999" MessageValidatorTip="true" OnInvalidCssClass="MaskedEditError"
                                MaskType="Date" DisplayMoney="Left" AcceptNegative="Left" ErrorTooltipEnabled="True">
                            </cc1:MaskedEditExtender>
                            <cc1:CalendarExtender ID="CalendarExtender2" runat="server" TargetControlID="txtLinerbookingdate"
                                PopupButtonID="txtLinerbookingdate" Format="dd/MM/yyyy">
                            </cc1:CalendarExtender>
                                                  
                                                </div>
                                                <div id="magnify_area">
                                                    
                                                </div>
                                            </div>
                                            <div id="tag_transac_lft_in1" style="width: 520px; height: 35px;">
                                                <div id="tag_label_transaction_popup_gen" style="width: 190px">
                                                    Sales Person
                                                </div>
                                                <div id="txtcon-m_transaction_pop_gen1_srcic_new">
                                                    <asp:TextBox ID="txtSalesby" runat="server" MaxLength="50" onkeypress="return char(event),TextBoxes(this);"
                                                        onblur="ChangeCase(this);updateTextbox2();" onkeyup="ChangeCase(this);" onfocus="updateTextbox2();"
                                                        onkeydown="var data = val;Port_origin(this);" onchange="updateTextbox2();" CssClass="txtbox_none_Mid_transac_pop_gen_srcic_new"
                                                        ClientIDMode="Static" TabIndex="13"></asp:TextBox>
                                                </div>
                                                <div id="magnify_area">
                               <a href="#" class="magnfy" onclick="jQuery('#form1').validationEngine('hideAll');OpenPopupSalesMaster('Sales'); return false">
                            </a>
                            </div>
                                            </div>
                                           
                                            
                                            
                                            <div id="tag_transac_lft_in1" style="width: 520px; height: 35px;">
                                                <div id="tag_label_transaction_popup_gen" style="width: 190px">
                                                    Place Of Delivery 
                                                </div>
                                                <div id="txtcon-m_transaction_pop_gen1_srcic_new">
                                                    <asp:TextBox ID="txtPlaceofdelivery" runat="server" MaxLength="50" onkeypress="return char(event)"
                                                        onblur="ChangeCase(this);" onkeyup="ChangeCase(this);" onkeydown="ChangeCase(this);var data = val;Port_origin(this);"
                                                        CssClass="txtbox_none_Mid_transac_pop_gen_srcic_new"  TabIndex="15"></asp:TextBox>
                                                </div>
                                                <div id="magnify_area">
                                                    
                                                </div>
                                            </div>
                                            <div id="tag_transac_lft_in1" style="width: 520px; height: 35px;">
                                                <div id="tag_label_transaction_popup_gen" style="width: 190px">
                                                     Port of Loading
                                                </div>
                                                <div id="txtcon-m_transaction_pop_gen1_srcic_new">
                                                    <asp:TextBox ID="txtPortofdelivery" runat="server" MaxLength="50" onkeypress="return char(event)"
                                                        onblur="ChangeCase(this);" onkeyup="ChangeCase(this);" onkeydown="ChangeCase(this);var data = val;Port_origin(this);"
                                                        CssClass="txtbox_none_Mid_transac_pop_gen_srcic_new" ClientIDMode="Static" TabIndex="17"></asp:TextBox>
                                                </div>
                                                <div id="magnify_area" style="width: 40px">
                                                   <a href="#" class="magnfy" onclick="jQuery('#form1').validationEngine('hideAll');OpenPopupPort_CountrySearch_Master('Delivery'); return false">
                                </a>
                                                </div>
                                            </div>
                                            
                                            
                                           
                                            
                                            
                                            
                                            
                                            
                                        </div>
                                    </div>
                                </asp:Panel>
                            </div>
                        </div>
                        <div class="content" id="page-11">
                            <div id="area_transac_container_genral">
                                <div id="pop_text_area_transac_popup_inn_container_new" style="width: 1120px">
                                    <div id="pop_text_area_transac_popup_inn_container_left_new" style="width: 500px">
                          <div id="tag_label_transaction_popup_gen" 
                                style="width: 1400px; padding-bottom: 25px;">
                            <div id="tag_label_transaction_popup_gen" style="width:160px">
                                Consignee Name
                               </div>
                            <div id="txtcon-m_transaction_pop_gen1_srcic_new" style="width: 155px">
                              <asp:TextBox ID="txtConsignee" runat="server" MaxLength="50" onblur="ChangeCase(this);"
                                    Font-Bold="true" ClientIDMode="Static" CssClass="validate[required] txtbox_none_Mid_transac_pop_gen_srcic_new"
                                    Onkeypress="return numcharspl1(event)"  TabIndex="19" Width="150px"></asp:TextBox>
                                            
                          
                            </div>
                              <div id="magnify_area">

                           <a href="#" class="magnfy" onclick="jQuery('#form1').validationEngine('hideAll');OpenPopup_CompanyMaster('ConsigneeName')">
                                </a>
                               
                            </div>
                              <div id="magnify_area" style="width: 30px"> </div>
                            <div id="tag_label_transaction_popup_gen2" style="width: 130px">
                                Branch</div>
                            <div id="txtcon-m_transaction_pop_gen1_srcic" style="width: 155px">
                            <asp:TextBox ID="txtbranch" runat="server"  CssClass="validate[required]  txtbox_none_Mid_transac_pop_gen_srcic_new"
                                                ClientIDMode="Static" Width="150px" TabIndex="20" ></asp:TextBox>
                         
                            </div>
                            <div id="magnify_area" style="width: 57px"></div>
                            <div id="tag_label_transaction_popup_gen" style="width: 115px">
                                Shipper Name</div>
                            <div id="txtcon-m_transaction_pop_gen1_srcic_new" style="width: 155px">
                              
                                       <asp:TextBox ID="txtSuppliername" runat="server" MaxLength="50" onblur="ChangeCase(this);"
                                    Font-Bold="true" ClientIDMode="Static" CssClass="txtbox_none_Mid_transac_pop_gen_srcic_new"
                                     TabIndex="21" Width="150px"></asp:TextBox>
                            </div>
                              <div id="magnify_area">

                         
                                            <div id="CND1" runat="server">
                                                <a href="#" class="magnfy" onclick="jQuery('#form1').validationEngine('hideAll');OpenPopup_CompanyMaster('Sup_Supplier'); return false">
                                                </a>
                                            </div>
                           </div>
                     </div>
                              <div id="tag_label_transaction_popup_gen" 
                                style="width: 1400px; padding-bottom: 25px;">
                            <div id="tag_label_transaction_popup_gen" style="width: 160px">
                                CHA
                               </div>
                            <div id="txtcon-m_transaction_pop_gen1_srcic" style="width: 155px">
                             
                                      <asp:TextBox ID="txtCha" runat="server" MaxLength="50" onblur="ChangeCase(this);"
                                    Font-Bold="true" ClientIDMode="Static" CssClass="txtbox_none_Mid_transac_pop_gen_srcic_new"
                                     TabIndex="22" Width="150px"></asp:TextBox>        
                          
                            </div>
                             <div id="magnify_area">

                           <a href="#" class="magnfy" onclick="jQuery('#form1').validationEngine('hideAll');OpenPopup_CompanyMaster('cha')">
                                </a>
                               
                            </div>
                             
                              <div id="magnify_area" style="width: 30px"> </div>
                            <div id="tag_label_transaction_popup_gen2" style="width: 130px">
                                Liner</div>
                            <div id="txtcon-m_transaction_pop_gen1_srcic" style="width: 155px">
                              <asp:TextBox ID="txtlinerName" runat="server" MaxLength="50" onblur="ChangeCase(this);"
                                    Font-Bold="true" ClientIDMode="Static" CssClass="txtbox_none_Mid_transac_pop_gen_srcic_new"
                                    Onkeypress="return numcharspl1(event)"  TabIndex="23" Width="150px"></asp:TextBox>       
                              
                            </div>
                             <div id="magnify_area">

                           <a href="#" class="magnfy" onclick="jQuery('#form1').validationEngine('hideAll');OpenPopup_CompanyMaster('Liner')">
                                </a>
                               
                            </div>
                            <div id="magnify_area" style="width: 30px"></div>
                            <div id="tag_label_transaction_popup_gen" style="width: 110px">
                                Freight Forwader</div>
                            <div id="txtcon-m_transaction_pop_gen1_srcic" style="width: 155px">
                             <asp:TextBox ID="txtForwarder" runat="server" MaxLength="50" onblur="ChangeCase(this);"
                                    Font-Bold="true" ClientIDMode="Static" CssClass="txtbox_none_Mid_transac_pop_gen_srcic_new"
                                     TabIndex="24" Width="150px"></asp:TextBox>     
                              
                            </div>
                            <div id="magnify_area">
                                            <div id="CND2" runat="server">
                                             <a href="#" class="magnfy" onclick="jQuery('#form1').validationEngine('hideAll');OpenPopup_CompanyMaster('FORWARDER'); return false">
                                                </a>
                                               <%-- <a href="#" class="magnfy" onclick="jQuery('#form1').validationEngine('hideAll');OpenPopupAgent(document.getElementById('txtExporterName').value,document.getElementById('txtIECNo').value,'IMPORT'); return false">
                                                </a>--%>
                                            </div>
                                            </div>
                     </div>               
                             
                              <div id="tag_label_transaction_popup_gen" 
                                style="width: 1400px; padding-bottom: 25px;">
                            <div id="tag_label_transaction_popup_gen" style="width: 160px">
                               Consol Agent
                               </div>
                            <div id="txtcon-m_transaction_pop_gen1_srcic" style="width: 155px">
                             
                                   <asp:TextBox ID="txtClearingAgent" runat="server" MaxLength="50" onblur="ChangeCase(this);"
                                    Font-Bold="true" ClientIDMode="Static" CssClass="txtbox_none_Mid_transac_pop_gen_srcic_new"
                                    TabIndex="25" Width="150px"></asp:TextBox>             
                          
                            </div>
                             <div id="magnify_area">

                           <a href="#" class="magnfy" onclick="jQuery('#form1').validationEngine('hideAll');OpenPopup_CompanyMaster('ConsolAgent')">
                                </a>
                               
                            </div>
                             
                              <div id="magnify_area" style="width: 30px"> </div>
                            <div id="tag_label_transaction_popup_gen2" style="width: 130px">
                                Co Loader</div>
                            <div id="txtcon-m_transaction_pop_gen1_srcic" style="width: 155px">
                               <asp:TextBox ID="txtCoLoaderName" runat="server" MaxLength="50" onblur="ChangeCase(this);"
                                    Font-Bold="true" ClientIDMode="Static" CssClass="txtbox_none_Mid_transac_pop_gen_srcic_new"
                                     TabIndex="26" Width="150px"></asp:TextBox>    
                             </div>
                             <div id="magnify_area">
                                            <div id="CND1c7" runat="server">
                                                <a href="#" class="magnfy" onclick="jQuery('#form1').validationEngine('hideAll');OpenPopup_CompanyMaster('COLOADER'); return false">
                                                </a>
                                            </div>
                                        </div>
                           

                            <div id="magnify_area" style="width: 30px"></div>
                            <div id="tag_label_transaction_popup_gen" style="width: 115px">
                                Origin Agent</div>
                            <div id="txtcon-m_transaction_pop_gen1_srcic" style="width: 155px">
                              <asp:TextBox ID="txtOriginAgent" runat="server" MaxLength="50" onblur="ChangeCase(this);"
                                    Font-Bold="true" ClientIDMode="Static" CssClass="txtbox_none_Mid_transac_pop_gen_srcic_new"
                                      TabIndex="27" Width="150px"></asp:TextBox>    
                            
                            </div>
                              <div id="magnify_area">
                                           <%-- <div id="CND9" runat="server">--%>
                                                 <a href="#" class="magnfy" onclick="jQuery('#form1').validationEngine('hideAll');OpenPopup_CompanyMaster('OriginAgent'); return false">
                                                </a>
                                          <%--  </div>--%>
                                         </div>
                     </div>  
                      <div id="tag_label_transaction_popup_gen" 
                                style="width: 1400px; padding-bottom: 25px;">
                            <div id="tag_label_transaction_popup_gen2" style="width: 160px">
                              AFS
                               </div>
                            <div id="txtcon-m_transaction_pop_gen1_srcic" style="width: 155px">
                              <asp:TextBox ID="txtafs" runat="server" MaxLength="50" onblur="ChangeCase(this);"
                                    Font-Bold="true" ClientIDMode="Static" CssClass="txtbox_none_Mid_transac_pop_gen_srcic_new"
                                      TabIndex="28" Width="150px"></asp:TextBox> 
                          
                                            
                          
                            </div>
                              <div id="magnify_area">

                           <a href="#" class="magnfy" onclick="jQuery('#form1').validationEngine('hideAll');OpenPopup_CompanyMaster('Afs')">
                                </a>
                               
                            </div>
                              <div id="magnify_area" style="width: 30px"> </div>
                            <div id="tag_label_transaction_popup_gen2" style="width: 130px">
                               Transporter</div>
                            <div id="txtcon-m_transaction_pop_gen1_srcic" style="width: 155px">
                                 <asp:TextBox ID="txttransporter" runat="server" MaxLength="50" onblur="ChangeCase(this);"
                                    Font-Bold="true" ClientIDMode="Static" CssClass="txtbox_none_Mid_transac_pop_gen_srcic_new"
                                     TabIndex="29" Width="150px"></asp:TextBox> 
                             
                            </div>
                             <div id="magnify_area">

                           <a href="#" class="magnfy" onclick="jQuery('#form1').validationEngine('hideAll');OpenPopup_CompanyMaster('Transporter')">
                                </a>
                               
                            </div>
                            <div id="magnify_area" style="width: 30px"></div>
                            <div id="tag_label_transaction_popup_gen" style="width: 115px">
                                Other Agents</div>
                            <div id="txtcon-m_transaction_pop_gen1_srcic_new">
                              <asp:TextBox ID="txtotheragent" runat="server" MaxLength="50" onblur="ChangeCase(this);"
                                    Font-Bold="true" ClientIDMode="Static" CssClass="txtbox_none_Mid_transac_pop_gen_srcic_new"
                                      TabIndex="30" Width="150px"></asp:TextBox> 
                             
                            </div>
                             <%--<div id="magnify_area">

                           <a href="#" class="magnfy" onclick="jQuery('#form1').validationEngine('hideAll');OpenPopup_CompanyMaster('ConsigneeName')">
                                </a>
                               
                            </div>--%>
                     </div>                                   
                             <div id="tag_label_transaction_popup_gen" 
                                style="width: 1400px; padding-bottom: 25px;">
                            <div id="tag_label_transaction_popup_gen" style="width: 160px">
                             Commodity Type
                               </div>
                           
                             <div id="txtcon-m_transaction_pop_gen1_srcic_new" style="width: 155px">
                           <asp:DropDownList ID="ddl_info_type" runat="server" CssClass="listtxt_transac_mode_new"
                                Width="155px" TabIndex="31" >
                            </asp:DropDownList>
                          
                            </div>
                          
                            
                             
                              <div id="magnify_area" style="width: 63px"> </div>
                            <div id="tag_label_transaction_popup_gen2" style="width: 130px">
                               If Others Specify</div>
                            <div id="txtcon-m_transaction_pop_gen1_srcic" style="width: 155px">
                               <asp:TextBox ID="txtOtherSpecify" runat="server" MaxLength="50" onblur="ChangeCase(this);"
                                    Font-Bold="true" ClientIDMode="Static" CssClass="txtbox_none_Mid_transac_pop_gen_srcic_new"
                                     TabIndex="32" Width="150px"></asp:TextBox>
                           
                            </div>
                            <div id="magnify_area" style="width: 63px"></div>
                            <div id="tag_label_transaction_popup_gen" style="width: 115px">
                                Type of DG</div>
                            <div id="txtcon-m_transaction_pop_gen1_srcic_new" style="width: 175px">
                           
                                             <asp:DropDownList ID="ddl_AEOtypoe" runat="server" CssClass="listtxt_transac_mode_new"
                                Width="153px" TabIndex="33" >
                            </asp:DropDownList>
                          </div>
                     </div>           
                     <div id="tag_label_transaction_popup_gen" 
                                style="width: 1400px; padding-bottom: 25px;">
                            <div id="tag_label_transaction_popup_gen" style="width: 160px">
                             No of Qty
                               </div>
                            <div id="txtcon-m_transaction_pop_gen1_srcic_new" style="width: 95px">
                             <asp:TextBox ID="txtQuantity" runat="server" CssClass="validate[required] validate[custom[number]] txtbox_none_Mid_transac_code"
                                         style="width:90px"   MaxLength="8" TabIndex="34" onkeypress="return pin(event)"></asp:TextBox>
                                    
                          
                            </div>
                             <div id="txtcon_m_transaction_IGM_Wght_small" style="width: 10px">
                                    </div>
                                    <div id="txtcon_m_transaction_IGM_Wght_small" style="width: 55px">
                                        <asp:TextBox ID="txtNetWeightType" runat="server" CssClass="validate[custom[onlyLetterSp],minSize[3]] txtbox_none_Mid_transac_IGM_wght_small"
                                            ClientIDMode="Static" MaxLength="3" TabIndex="35" onkeypress="return char1(event)"
                                            onblur="ChangeCase(this);"></asp:TextBox>
                                    </div>
                                     <div id="magnify_area">

                                       <a href="#" onclick="OpenPopupUnit('Netweight'); return false" class="magnfy"></a>
                                       
                                    </div>
                              <div id="magnify_area" style="width: 25px"> </div>
                            <div id="tag_label_transaction_popup_gen2" style="width: 130px">
                               No of PKG</div>
                           <div id="txtcon-m_transaction_pop_gen1_srcic_new" style="width: 95px">
                             <asp:TextBox ID="txtPackages" runat="server" CssClass="validate[required] validate[custom[number]] txtbox_none_Mid_transac_code"
                                          style="width:90px"  MaxLength="8" TabIndex="36" onkeypress="return pin(event)"></asp:TextBox>
                                    
                          
                            </div>
                             <div id="txtcon_m_transaction_IGM_Wght_small" style="width: 5px">
                                    </div>
                                    <div id="txtcon_m_transaction_IGM_Wght_small" style="width: 55px">
                                        <asp:TextBox ID="txtPackageType" runat="server" CssClass="validate[custom[onlyLetterSp],minSize[3]] txtbox_none_Mid_transac_IGM_wght_small"
                                            ClientIDMode="Static" MaxLength="3" TabIndex="37" onkeypress="return char1(event)"
                                            onblur="ChangeCase(this);"></asp:TextBox>
                                    </div>
                                     <div id="magnify_area">
                                        <a href="#" onclick="OpenPopup_Package_Details(); return false" class="magnfy"></a>
                                    </div>

                            <div id="magnify_area" style="width: 30px"></div>
                            <div id="tag_label_transaction_popup_gen" style="width: 115px">
                                Net Weight</div>
                            <div id="txtcon-m_transaction_pop_gen1_srcic_new">
                              <asp:TextBox ID="txtNetWeight" runat="server" CssClass="validate[required] validate[custom[number]] txtbox_none_Mid_transac_code"
                                            MaxLength="7" TabIndex="38" onblur="extractNumber(this,6,false);" onkeyup="extractNumber(this,6,false);return Clear_Dot(this);"
                                            onkeypress="return blockNonNumbers(this, event, true, false);" 
                                    style="width: 150px"></asp:TextBox>
                            </div>
                     </div>                 
                            <div id="tag_label_transaction_popup_gen" 
                                style="width: 1400px; padding-bottom: 25px;">
                            <div id="tag_label_transaction_popup_gen" style="width:160px">
                                Gross Weight
                               </div>
                            <div id="txtcon-m_transaction_pop_gen1_srcic_new" style="width: 175px">
                              <asp:TextBox ID="txtGrossWeight" runat="server" CssClass="validate[required] validate[custom[number]]  txtbox_none_Mid_transac_item_maindet"
                                    ClientIDMode="Static" TabIndex="39" onblur="extractNumber(this,3,false);" onkeyup="extractNumber(this,3,false);return Clear_Dot(this);"
                                            onkeypress="return blockNonNumbers(this, event, true, false);" Width="150px"></asp:TextBox>
                                            
                           
                            </div>
                          
                                    
                             
                              <div id="magnify_area" style="width: 40px"> </div>
                            <div id="tag_label_transaction_popup_gen2" style="width: 135px">
                                Chargable Weight</div>
                            <div id="txtcon-m_transaction_pop_gen1_srcic" style="width: 95px">
                              <asp:TextBox ID="txtChargable_Wgt" runat="server" CssClass="validate[required] txtbox_none_Mid_transac_code" MaxLength="12" TabIndex="40"
                                            onblur="extractNumber(this,3,false);" onkeyup="extractNumber(this,3,false);return Clear_Dot(this);"
                                          style="width:90px"  onkeypress="return blockNonNumbers(this, event, true, false);"></asp:TextBox>
                                            </div>
                                             <div id="txtcon_m_transaction_IGM_Wght_small" style="width: 5px">
                                    </div>
                                    <div id="txtcon_m_transaction_IGM_Wght_small" style="width: 55px">
                                        <asp:TextBox ID="txtChargeableType" runat="server" CssClass="validate[custom[onlyLetterSp],minSize[3]] txtbox_none_Mid_transac_IGM_wght_small"
                                            ClientIDMode="Static" MaxLength="3" TabIndex="41" onkeypress="return char1(event)"
                                            onblur="ChangeCase(this);"></asp:TextBox>
                                    </div>
                                     <div id="magnify_area">
                                        <a href="#" onclick="OpenPopupUnit('CharagbleWeight'); return false" class="magnfy">
                                        </a>
                                    </div>
                            <div id="magnify_area" style="width: 30px"></div>
                            <div id="tag_label_transaction_popup_gen" style="width: 115px">
                                Cargo Description</div>
                            <div id="txtcon-m_transaction_pop_gen1_srcic_new">
                              <asp:TextBox ID="txt_Commodity" runat="server" style="text-transform:uppercase" CssClass="txtbox_none_Mid_transac_Marks_nos" 
                                            TextMode="MultiLine" onkeyDown=" return (event.keyCode!=13); return checkTextAreaMaxLength(this,event,'300');" 
                                            onpaste="return checkTextAreaMaxLength(this,event,'300');" MaxLength="300" TabIndex="42"  Height="55px" width="150px"></asp:TextBox> </div>
                     </div>            
                     <div id="tag_label_transaction_popup_gen" 
                                style="width: 1400px; padding-bottom: 25px;">
                            <div id="tag_label_transaction_popup_gen" style="width:160px">
                               Marks & Nos
                               </div>
                            <div id="txtcon-m_transaction_pop_gen1_srcic_new" style="width: 175px">
                              <asp:TextBox ID="txtmarks" runat="server" style="text-transform:uppercase" CssClass=" txtbox_none_Mid_transac_Marks_nos" 
                                            TextMode="MultiLine" onkeyDown=" return (event.keyCode!=13); return checkTextAreaMaxLength(this,event,'300');" 
                                            onpaste="return checkTextAreaMaxLength(this,event,'300');" MaxLength="300" TabIndex="43"  Height="55px" width="150px"></asp:TextBox> </div>
                                       
                                      
                                        </div>
										
                                       
										

									   
                        </div>
                                    
                                </div>
                            </div>
                        </div>
                          <div class="content" id="page-2" style="position:relative" 
                   >
                    <iframe src="" name="cargo" scrolling="no" id="Trans_Cargo_Iframe" style=" width: 1180px;
                         height: 500px;"
                        ></iframe>
                         

                </div>
                    </div>
                    <div id="area_transac_container_genral">
                        <div id="innerbox_MidMain_bot_transact" runat="server" style="width: 900px;">
                            <div id="innerbox_transac_bot_inn" style="width: 890px;">
                               <asp:Button ID="btngenerate" runat="server" OnClick="btngenerate_Click" 
                                    CausesValidation="false" UseSubmitBehavior="false"
                                            Visible="true" CssClass="autowidmovebut" Text="Generate Invoice" 
                                             
                                            Width="110px" Height="25px" />
                            </div>
                        </div>
                    </div>
                </div>
               <%-- <div class="content" id="page-2" 
                   >
                    <iframe src="" name="maineroyal" scrolling="no" id="Trans_Cargo_Iframe" style=" width: 1180px;
                         height: 750px;"
                        ></iframe>
                </div>--%>
                <div class="content" id="page-3" style="display: none;">
                    <iframe src="" name="maineroyal1" scrolling="no" id="Trans_igm_Invoice_Iframe"></iframe>
                </div>
                <div class="content" id="page-4" style="display: none;">
                    <iframe src="" name="maineroyal2" scrolling="no" id="Trans_igm_item_Iframe"></iframe>
                </div>
                <div class="content" id="page-5" style="display: none;">
                </div>
                <div class="content" id="page-6" style="display: none;">
                </div>
                <div class="content" id="page-9" style="display: none;">
                </div>
                <div class="content" id="page-7" style="display: none;">
                </div>
                <div class="content" id="page-8" style="display: none;">
                    <iframe src="" name="maineroyal2" scrolling="no" id="Iframe_E_Way_Bill"></iframe>
                </div>
            </div>
            <script src="../activatables.js" type="text/javascript"></script>
            <script type="text/javascript">
                activatables('page', ['page-1', 'page-2', 'page-3', 'page-4', 'page-5', 'page-6', 'page-7', 'page-8', 'page-9', 'page-10', 'page-11']);
            </script>
            <div style="display: none;">
                <asp:Label ID="lblMsg" runat="server" ForeColor="Red"></asp:Label>
                <asp:HiddenField ID="chksec46" runat="server" Value="N" ClientIDMode="Static" />
                <asp:HiddenField ID="chkFirCheck" runat="server" Value="N" ClientIDMode="Static" />
                <asp:HiddenField ID="chkGrnChannel" runat="server" Value="N" ClientIDMode="Static" />
                <asp:HiddenField ID="chkKach" runat="server" Value="N" ClientIDMode="Static" />
                <asp:HiddenField ID="chkSec48" runat="server" Value="N" ClientIDMode="Static" />
                <asp:HiddenField ID="chkHghSeaSale" runat="server" Value="N" ClientIDMode="Static" />
                <asp:HiddenField ID="HiddenField1" runat="server" ClientIDMode="Static" />
                <asp:HiddenField ID="hdn_val_updateid" runat="server" ClientIDMode="Static" />
                <asp:HiddenField ID="hdn_Jobno" runat="server" ClientIDMode="Static" />
                <asp:HiddenField ID="hdn_ImporterType" runat="server" ClientIDMode="Static" />
                <asp:HiddenField ID="hdn_Update_General" runat="server" ClientIDMode="Static" />
                <asp:HiddenField ID="hdn" runat="server" ClientIDMode="Static" />
                <asp:HiddenField ID="hdfTransport_mode" runat="server" ClientIDMode="Static" />
                <asp:HiddenField ID="Hdcountryoforgin_chk" runat="server" ClientIDMode="Static" />
                <asp:HiddenField ID="Hdcountryofshipment_chk" runat="server" ClientIDMode="Static" />
                <asp:HiddenField ID="Hdportoforgin_Chk" runat="server" ClientIDMode="Static" />
                <asp:HiddenField ID="Hdportofshipment_chk" runat="server" ClientIDMode="Static" />
                <asp:HiddenField ID="hd_joblck" runat="server" ClientIDMode="Static" />
                <asp:HiddenField ID="hdimp" runat="server" />
                <asp:HiddenField ID="hd_Brslno" runat="server" />
                <asp:HiddenField ID="hd_hss_imp" runat="server" />
                <asp:HiddenField ID="hd_hss_br_sl_no" runat="server" />
                <asp:HiddenField ID="Hdn_STANDARD_TYPE" runat="server" />
                <asp:HiddenField ID="hdn_TIN_NO" runat="server" />
                <asp:HiddenField ID="hdn_StateCode" runat="server" />
                <asp:HiddenField ID="hdn_StateName" runat="server" />
                <asp:HiddenField ID="hdn_commercialType" runat="server" />
                <asp:HiddenField ID="hdsaveandclose" runat="server" />
                <asp:HiddenField ID="hdprefix" runat="server" />
                <asp:HiddenField ID="hdsuffix" runat="server" />
                <asp:HiddenField ID="hdn_Comp_License" runat="server" />
                <asp:HiddenField ID="hdn_New_Update" runat="server" />
                <asp:HiddenField ID="hdn_currentPage" runat="server" />
                <asp:HiddenField ID="hdn_source" runat="server" />
                <asp:HiddenField ID="hdn_StateName_chk" runat="server" ClientIDMode="Static" />
                <asp:HiddenField ID="hdn_Customhouse_chk" runat="server" ClientIDMode="Static" />
                <asp:HiddenField ID="hdnFreightMode" runat="server" />
                <asp:HiddenField ID="hdnIsea" runat="server" ClientIDMode="Static" />
                <asp:HiddenField ID="hdnOperation" runat="server" ClientIDMode="Static" />
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <%-- ---------------------------------------------------------ModelpopUp---1------------------------------------------------------------------------------%>
    <asp:ModalPopupExtender ID="MP_U" runat="server" TargetControlID="HDU" PopupControlID="MessageDivupdated_U"
        BackgroundCssClass="modalBackground" DropShadow="false">
    </asp:ModalPopupExtender>
    <asp:HiddenField ID="HDU" runat="server" />
    <asp:Panel ID="MessageDivupdated_U" runat="server" CssClass="modalPopup12" align="left"
        BorderColor="Transparent">
        <div id="innerbox_MidMain_popup_adcode_ItemMove" style="width: 500px; height: 80px;">
            <table width="100%" style="padding-top: 10px;">
                <tr>
                    <td align="center">
                        <asp:Label ID="lblUser_Msg" runat="server" Text=""></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td style="padding-left: 125px; padding-top: 10px;">
                        <asp:Button ID="Button2" runat="server" Text="Ok" OnClientClick="return HideModalPopup_U()"
                            CausesValidation="false" UseSubmitBehavior="false" />
                    </td>
                </tr>
            </table>
        </div>
    </asp:Panel>
    <%-- ---------------------------------------------------------ModelpopUp---2------------------------------------------------------------------------------%>
    <asp:ModalPopupExtender ID="MP_JOB" runat="server" TargetControlID="HDU_JOB" PopupControlID="MessageDivupdated_Job"
        BackgroundCssClass="modalBackground" DropShadow="false">
    </asp:ModalPopupExtender>
    <asp:HiddenField ID="HDU_JOB" runat="server" />
    <asp:Panel ID="MessageDivupdated_Job" runat="server" CssClass="modalPopup12" align="left"
        BorderColor="Transparent">
        <div id="innerbox_MidMain_popup_adcode_ItemMove" style="width: 450px; height: 129px;">
            <div id="innerbox_MidMain_Transact_popup_new" style="width: 449px; height: 129px;">
                <div id="tag_srcinner1_new">
                    <div id="editbu">
                        <a href="#" class="edit"></a>
                    </div>
                    <div id="mainmastop2container_rght_tag2_txt1">
                        Job Edit
                    </div>
                    <div id="popupwindow_closebut_right_new">
                        <asp:Button ID="btnCl" runat="server" CssClass="clsicon_new" OnClientClick="return HideModalPopup()"
                            CausesValidation="false" UseSubmitBehavior="false" />
                    </div>
                </div>
                <div id="tag_transaction_inner_new" style="margin-left: 20px; margin-top: 10px; width: 500px;">
                    <div id="tag_Exchange_lft_in1_new1">
                        <div id="tag_label_transaction_date" style="width: 75px; text-align: center;">
                            Edit Jobno
                        </div>
                        <div id="txtcon-m_transaction_code" style="width: 350px;">
                            <asp:TextBox ID="txt_M_jobps" placeholder="Prefix" Width="80px" runat="server" Onkeypress="return numcharspl1(event)"
                                MaxLength="30" CssClass="txtbox_none_Mid_transac_pop_gen_srcic_new" autocomplete="off"></asp:TextBox>
                            <asp:TextBox ID="txt_jobno" runat="server" MaxLength="10" CssClass="txtbox_none_Mid_transac_code"
                                autocomplete="off" onkeypress="return pin(event)"></asp:TextBox>
                            <asp:TextBox ID="txt_M_jobsf" runat="server" MaxLength="30" placeholder="Suffix"
                                Onkeypress="return numcharspl1(event)" CssClass="txtbox_none_Mid_transac_pop_gen_srcic_new"
                                Width="80px" autocomplete="off"></asp:TextBox>
                        </div>
                        <div id="tag_label_transaction_popup_mode_empty" style="margin-left: 10px;">
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" ControlToValidate="txt_jobno"
                                ForeColor="Red" runat="server" ErrorMessage="*">
                            </asp:RequiredFieldValidator>
                        </div>
                        <div id="tag_label_transaction_Src_longlabel" style="width: 30px;">
                            <%--  <asp:Label ID="lblalt" runat="server" ></asp:Label>--%>
                            <%-- <asp:TextBox ID="txtid" runat="server" ForeColor="Red" BorderStyle="none" ReadOnly="true"></asp:TextBox>--%>
                        </div>
                        <%--<div id="txtcon-m_transaction_mode" style="width:60px;" >
                        </div>--%>
                    </div>
                    <div id="tag_label_transaction_popup_IGM2_empty_gen">
                    </div>
                    <div id="tag_label_transaction_popup_IGM2_empty_gen">
                    </div>
                    <div id="tag_label_transaction_popup_IGM2_empty_gen">
                    </div>
                    <div id="tag_Exchange_lft_in1_new1">
                        <div id="txtcon-m_transaction_code" style="width: 300px;">
                            <span style="color: Red;">
                                <asp:Literal ID="txtid" runat="server"></asp:Literal></span>
                        </div>
                    </div>
                    <div id="tag_Exchange_lft_in1_new1">
                        <div id="tag_label_transaction_Src_new" style="width: 30px;">
                        </div>
                        <div id="txtcon-m_transaction_code" style="width: 60px;">
                        </div>
                        <div id="tag_label_transaction_date" style="width: 75px; text-align: center;">
                            <asp:Button ID="btnok" runat="server" Text="Ok" UseSubmitBehavior="false" OnClick="btnok_Click" />
                        </div>
                        <div id="txtcon-m_transaction_code">
                            <asp:Button ID="btncancel" runat="server" Text="Cancel" OnClientClick="return HideModalPopup()"
                                CausesValidation="false" UseSubmitBehavior="false" />
                        </div>
                        <div id="tag_label_transaction_popup_mode_empty">
                        </div>
                        <div id="tag_label_transaction_Src_longlabel" style="width: 30px;">
                        </div>
                        <div id="txtcon-m_transaction_mode" style="width: 60px;">
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </asp:Panel>
    <%-- ---------------------------------------------------------End ModelpopUp---2------------------------------------------------------------------------------%>
    <asp:HiddenField ID="hdn_Regn_no" runat="server" />
    <asp:HiddenField ID="hdn_CompanyID" runat="server" ClientIDMode="Static" />
    </form>
     <script src="../js/Export_Jscript/Export_BookingForm_Air_General.js" type="text/javascript"></script>
    <script type="text/javascript" src="../windowfiles/dhtmlwindow.js"></script>
    <script type="text/javascript" src="../modalfiles/modal.js"></script>
    <script type="text/javascript" src="../js/slide.js"></script>
    <script type="text/javascript" src="../js/jscolor.js"></script>
    <script src="../js/iframepopupwin.js" type="text/javascript"></script>
    <script src="../js/listpopup.js" type="text/javascript"></script>
    <script src="../js/checkboxJScript.js" type="text/javascript"></script>
    <script src="../js/Validation.js" type="text/javascript"></script>
    <script src="../AutoComplete_JS/jquery.min.js" type="text/javascript"></script>
    <script src="../AutoComplete_JS/jquery-ui.min.js" type="text/javascript"></script>
    <!-- VALIDATION SCRIPT -->
    <script src="../Validation Files/jquery_002.js" type="text/javascript" charset="utf-8">
    </script>
    <script src="../Validation Files/jquery.js" type="text/javascript" charset="utf-8">
    </script>
    <script src="../Validation%20Files/Ascii.js" type="text/javascript"></script>
    <!-- VALIDATION SCRIPT -->
    <script src="../js/MaskedEditFix.js" type="text/javascript"></script>
    <script type="text/javascript">
        jQuery(document).ready(function () {

            jQuery("#form1").validationEngine();
        });


        function checkHELLO(field, rules, i, options) {
            if (field.val() != "HELLO") {

                return options.allrules.validate2fields.alertText;
            }
        }
        function close_button() {
            //document.getElementById("btnCancel").click();
            document.getElementById("General1").click();
        }
    </script>
    <script type="text/javascript">
        $(document).ready(function () {

            $('#txtExporterName').focus(function () {

                var data = $(this).val();
                Item_desc(data);

            });
            $('#txtExporterName').change(function () {

                var data = $(this).val();
                Item_desc(data);

            });
            $('#txtExporterName').keyup(function () {

                var data = $(this).val();
                Item_desc(data);

            });
            $('#txtExporterName').keydown(function () {

                var data = $(this).val();
                Item_desc(data);

            });

        });

        function Item_desc(val) {

            var data = val;
            if (data == '') {
                $('#txtExporterName').val('');
            }
            if (data.indexOf("--") != -1) {

                var s = data.split('--');

                var before = s[0];
                var after = s[1];

                $('#txtExporterName').val(after);

                $('#hdimp').val(after);
                $('#hd_Brslno').val(before);

            }
            else {

            }
        }
        $(document).ready(function () {

            $('#txtClientName').focus(function () {

                var data = $(this).val();
                Item_descc(data);

            });
            $('#txtClientName').change(function () {

                var data = $(this).val();
                Item_descc(data);

            });
            $('#txtClientName').keyup(function () {

                var data = $(this).val();
                Item_descc(data);

            });
            $('#txtClientName').keydown(function () {

                var data = $(this).val();
                Item_descc(data);

            });

        });

        function Item_descc(val) {

            var data = val;
            if (data == '') {
                $('#txtClientName').val('');
            }
            if (data.indexOf("--") != -1) {

                var s = data.split('--');

                var before = s[0];
                var after = s[1];

                $('#txtClientName').val(after);

                $('#hdimp').val(after);
                $('#hd_Brslno').val(before);

            }
            else {

            }
        }
    </script>
   
    <script src="../js/Import_Jscript/Import_Jobedit.js" type="text/javascript"></script>
    <%-- <script src="../js/Import_Jscript/BE_Status.js" type="text/javascript"></script>--%>
    <script type="text/javascript">

        function BE_Track() {
            var BeNo = document.getElementById("txtBE_No").value;
            var Date = document.getElementById("txtBE_Date").value;
            var Port = document.getElementById("txtCustomsHouse").value;
            var ddl = document.getElementById('ddlTransportMode');
            var mode = ddl.options[ddl.selectedIndex].value;

            var values = Date.split('/');

            var dd = values[0];
            var mm = values[1];
            var yy = values[2];

            var Bedate = yy + '/' + mm + '/' + dd;
            if (mode == 'Air' || mode == 'Sea' || mode == 'Land') {
                BE_Status(BeNo, Bedate, Port, mode);
            }
        }
    </script>
    <script type="text/javascript">
        //     $(document).ready(function () {

        //         if ($("#ddlTransportMode option:selected").text() == 'Air') {

        //             $("#ddlContainer_Type option[value='LCL']").remove();
        //             $("#ddlContainer_Type option[value='FCL']").remove();
        //             $("#ddlContainer_Type option[value='Bulk']").remove();

        //             $('#ddlContainer_Type').attr('disabled', 'disabled');
        //             //$('#dropdown').removeAttr('disabled');
        //         }
        //         else {
        //             $("#ddlContainer_Type option[value='']").remove();
        //         }

        //         $('#ddlTransportMode').change(function () {

        //             if ($("#ddlTransportMode option:selected").text() == 'Air') {

        //                 $("#ddlContainer_Type option[value='LCL']").remove();
        //                 $("#ddlContainer_Type option[value='FCL']").remove();
        //                 $("#ddlContainer_Type option[value='Bulk']").remove();

        //                 $('#ddlContainer_Type').attr('disabled', 'disabled');
        //             }
        //             else {
        //                 $("#ddlContainer_Type option[value='']").remove();

        //                 $("#ddlContainer_Type option[value='LCL']").remove();
        //                 $("#ddlContainer_Type option[value='FCL']").remove();
        //                 $("#ddlContainer_Type option[value='Bulk']").remove();

        //                 $("#ddlContainer_Type").append('<option value="LCL">LCL</option>');
        //                 $("#ddlContainer_Type").append('<option value="LCL">FCL</option>');
        //                 $("#ddlContainer_Type").append('<option value="LCL">Bulk</option>');

        //                 $('#ddlContainer_Type').removeAttr('disabled');
        //             }


        //         });
        //     });
    </script>
    <script type="text/javascript">
        $(document).ready(function () {
            // document.onkeydown = click_TABS;
            document.onkeypress = click_TABS;
            document.onkeyup = click_TABS;

            function click_TABS(e) {
                //if (window.event);
                if (window.event.ctrlKey)
                    var keyCode = window.event.keyCode;       // IE

                else
                    var keyCode = e.which;

                var ctrl_n = document.getElementById("hdn_currentPage").value;

                if (e.ctrlKey) {
                    if (keyCode == 39) {
                        if (ctrl_n == 'General') {
                            document.getElementById('IGM').click();
                        }
                    }
                }
            }
        });
    </script>
    <script>
        $(function () {

            $("#General").click(function () {
                //alert(document.getElementById("hdn_source").value);
                //------------------------------- Forward Pages - start ------------------------

                if (document.getElementById("hdn_source").value == 'Tab_IGM') {
                    document.getElementById('INVOICE').click();
                }

                if (document.getElementById("hdn_source").value == 'Tab_Invoice') {
                    document.getElementById('ITEM').click();
                }

                if (document.getElementById("hdn_source").value == 'Tab_Item') {
                    document.getElementById('Checklist').click();
                }

                if (document.getElementById("hdn_source").value == 'Tab_Checklist') {
                    document.getElementById('Submission').click();
                }
                if (document.getElementById("hdn_source").value == 'Tab_Submission') {
                    document.getElementById('Jobhistry').click();
                }

                //------------------------------- Backward Pages - start ------------------------

                if (document.getElementById("hdn_source").value == 'Tab_IGM_Prev') {
                    document.getElementById('General').click();
                }

                if (document.getElementById("hdn_source").value == 'Tab_Invoice_Prev') {
                    document.getElementById('IGM').click();
                }

                if (document.getElementById("hdn_source").value == 'Tab_Item_Prev') {
                    document.getElementById('INVOICE').click();
                }

                if (document.getElementById("hdn_source").value == 'Tab_Checklist_Prev') {
                    document.getElementById('ITEM').click();
                }
                if (document.getElementById("hdn_source").value == 'Tab_Submission_Prev') {
                    document.getElementById('Checklist').click();
                }

                //------------------------------- Backward Pages -end ------------------------
            });


        });


        $(function () {


            $('#rdoVessel input').change(function () {
                if ($(this).val() == 1) {
                    $("#txtPortShipment").removeAttr("disabled");
                    $("#txtPortofdelivery").removeAttr("disabled");
                }
                else {
                    $("#txtPortShipment").attr("disabled", "disabled");
                    $("#txtPortofdelivery").attr("disabled", "disabled");
                    $("#txtPortShipment").val('');
                    $("#txtPortofdelivery").val('');
                }
            });


        });
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
    window.onload = happycode;
function happycode() {
    document.getElementById("General1").click();
    document.getElementById("Sub_General").click();
}


function tab_page1() {

    document.getElementById('page-1').style.display = 'Block';
    document.getElementById('page-2').style.display = 'Block';
    document.getElementById('page-3').style.display = 'none';
    document.getElementById('page-4').style.display = 'none';
    document.getElementById('page-10').style.display = 'block';
    document.getElementById('page-11').style.display = 'none';


}



function ta_page2(val) {


    document.getElementById('page-1').style.display = 'block';
    document.getElementById('page-2').style.display = 'Block';
    document.getElementById('page-3').style.display = 'none';
    document.getElementById('page-4').style.display = 'none';
    document.getElementById('page-10').style.display = 'none';
    document.getElementById('page-11').style.display = 'none';

    document.getElementById("Trans_Cargo_Iframe").src = "../Transaction_Export/Booking_Export_Air_CargoDimension.aspx";



}


function tab_page10(val) {



    // if (document.getElementById('hdn_Jobno').value != '') {

    document.getElementById('page-1').style.display = 'block';
    document.getElementById('page-2').style.display = 'none';
    document.getElementById('page-3').style.display = 'none';
    document.getElementById('page-4').style.display = 'none';

    document.getElementById('page-10').style.display = 'block';
    document.getElementById('page-11').style.display = 'none';


    //    }
    //    else {
    //        happycode();
    //    }
}
function tab_page11(val) {



    // if (document.getElementById('hdn_Jobno').value != '') {

    document.getElementById('page-1').style.display = 'block';
    document.getElementById('page-2').style.display = 'none';
    document.getElementById('page-3').style.display = 'none';
    document.getElementById('page-4').style.display = 'none';

    document.getElementById('page-10').style.display = 'none';
    document.getElementById('page-11').style.display = 'block';


    //    }
    //    else {
    //        happycode();
    //}
}


$(function () {
    $("#txtChargeableType").autocomplete({
        source: function (request, response) {
            var field = $(this.element).attr('id');

            $.ajax({
                url: "../AutoComplete_Pages/Auto_Complete_Searching.asmx/Unit_Master_List",
                data: "{ 'mail': '" + request.term + "' }",
                dataType: "json",
                type: "POST",
                contentType: "application/json; charset=utf-8",
                dataFilter: function (data) { return data; },
                async: true,
                success: function (data) {
                    response(data.d);
                    if (data.d == '') {
                        if (field == 'txtGrossWeightType') {
                            jQuery('#' + field).validationEngine('showPrompt', 'Incorrect Gross Weight Type', 'error', 'topRight', true);

                            //jAlert('Gross Weight Type is Incorrect', 'IGM', function (r) { document.getElementById(field).focus(); });
                            //return false;
                        }
                        if (field == 'txtNetWeightType') {
                            jQuery('#' + field).validationEngine('showPrompt', 'Incorrect Net Weight Type', 'error', 'topRight', true);

                            //jAlert('Net Weight Type is Incorrect', 'IGM', function (r) { document.getElementById(field).focus(); });
                            //return false;
                        }
                    }
                    else {
                        jQuery('#' + field).validationEngine('hidePrompt', '', 'error', 'topRight', true);
                    }
                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    //alert(textStatus);
                }
            });
        },
        minLength: 1

    });
    return false;
    scroll: true;
});




</script>
</body>
</html>
