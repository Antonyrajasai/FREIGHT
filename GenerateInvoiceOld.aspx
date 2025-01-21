<%@ Page Language="C#" AutoEventWireup="true" CodeFile="GenerateInvoiceOld.aspx.cs" Inherits="Accounts_GeneratrInvoiceold" %>

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
        function jsFunction(value) {
            if (value == 'Y') {

                var hdn_TIN_NO1 = document.getElementById("hdn_TIN_NO").value;
                var hdn_StateName1 = document.getElementById("hdn_StateName").value;
                var hdn_StateCode1 = document.getElementById("hdn_StateCode").value;
                var hdn_commercialType1 = document.getElementById("hdn_commercialType").value;

                document.getElementById("txtCommercial_RegistrationNo").value = hdn_TIN_NO1;
                document.getElementById("txtCommercial_StateName").value = hdn_StateName1;
                document.getElementById("txtCommercial_StateCode").value = hdn_StateCode1;
                document.getElementById("txtCommercial_Taxtype").value = hdn_commercialType1;
            }
            else {

                document.getElementById("txtCommercial_RegistrationNo").value = "";
                document.getElementById("txtCommercial_StateName").value = "";
                document.getElementById("txtCommercial_StateCode").value = "";
                document.getElementById("txtCommercial_Taxtype").value = "";
            }
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
            <div id="innerbox_MidMain_Transact_popup_new" style="height: 85px; width: 1142px;">
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
                <div id="tag_transaction_inner_new" style="margin-left: 10px">
                    <div id="tag_Exchange_lft_in1_new1" style="width: 1080px;">
                        <div id="tag_label_transaction_Src_new" style="width: 220px; margin-left: 20px">
                            Job No
                            <asp:TextBox ID="txtjobps" runat="server" Font-Bold="true" MaxLength="20" Font-Size="12px"
                                CssClass="txtbox_none_Mid_transac_code" Width="157px" ReadOnly="True" TabIndex="1"
                                ClientIDMode="Static"></asp:TextBox>
                        </div>
                        <div id="tag_label_transaction_popup_IGM2_empty" style="margin-top: 4px;">
                            <asp:ImageButton ID="btnedit" runat="server" ImageUrl="~/images/icons/icon_edit.jpg" Visible="false"
                                Height="15px" Width="15px" OnClientClick="return ShowModalPopup();" ImageAlign="Middle" />
                        </div>
                        <div id="tag_label_transaction_date" style="width: 60px;">
                           Job Date
                        </div>
                        <div id="txtcon-m_transaction_code" style="width: 110px;"> 
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
                       
                        <div id="tag_label_transaction_Src_longlabel" style="width: 95px;">
                            Transport Mode</div>
                        <div id="txtcon-m_transaction_mode" style="width: 50px;" >
                            <asp:DropDownList ID="ddlTransportMode" runat="server" CssClass="listtxt_transac_mode_new"
                                TabIndex="3">
                            </asp:DropDownList>
                        </div>
                        <div id="tag_label_transaction_popup_no_empty" style="width: 37px; margin-left: 2px;">
                         
                            <asp:ImageButton ID="btn_Tr_Mode_Change" ToolTip="Change Tr. Mode" Visible="false"
                                runat="server" ImageUrl="~/images/icons/icon_edit.jpg" Height="15px" Width="16px"
                                OnClientClick="jQuery('#form1').validationEngine('hideAll');open_Change_Mode('I'); return false" />
                        </div>
                        <div id="tag_label_transaction_Src" style="width: 80px;" >
                            Inco Terms
                        </div>
                        <div id="txtcon-m_transaction_code"  style="width: 110px;">
                            <asp:DropDownList ID="ddlIncoterms" runat="server" CssClass="listtxt_transac_mode_new"
                                Width="105px" TabIndex="4">
                            </asp:DropDownList>
                        </div>
                        
                        <div id="tag_label_transaction_Src" style="width: 50px;" >
                            Freight
                        </div>
                        <div id="txtcon-m_transaction_code">
                            <asp:TextBox ID="txtBE_Date" runat="server" MaxLength="10" CssClass="txtbox_none_Mid_transac_code"
                                Width="73px" ReadOnly="True" TabIndex="5" ClientIDMode="Static" Enabled="false"
                                Height="16px" Visible="false"></asp:TextBox>
                            <asp:DropDownList ID="ddlFreight" runat="server" CssClass="listtxt_transac_mode_new"
                                Width="80px" TabIndex="5">
                            </asp:DropDownList>
                        </div>

                         <div id="tag_label_transaction_Src">
                            Job Status
                        </div>
                        <div id="txtcon-m_transaction_code" style="width: 50px;">                             
                            <asp:DropDownList ID="ddlJobSts" runat="server"  
                                Width="80px" TabIndex="5">
                                <asp:ListItem>Pending</asp:ListItem>
                                <asp:ListItem>Completed</asp:ListItem>
                                <asp:ListItem>Cancelled</asp:ListItem>
                            </asp:DropDownList>
                        </div>

                    </div>
                    <div id="tag_label_transaction_popup_IGM2_empty_gen">
                    </div>
                    <div id="tag_Exchange_lft_in1_new1" style="width: 1100px;">
                        <div id="tag_label_transaction_Src_new" style="width: 220px; margin-left: 20px">
                            Type &nbsp;&nbsp;
                      
                            <asp:DropDownList ID="ddlBEType" runat="server" CssClass="listtxt_transac_mode_new"
                                Width="163px" TabIndex="6" AutoPostBack="true">
                               <%-- onselectedindexchanged="ddlBEType_SelectedIndexChanged">--%>
                            </asp:DropDownList>
                            <asp:TextBox ID="txtJobNo" runat="server" MaxLength="50" Font-Size="12px" CssClass="txtbox_none_Mid_transac_code"
                                BorderColor="White" Width="0px" ReadOnly="True" TabIndex="1" ClientIDMode="Static"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server" ValidationGroup="draw"
                                ErrorMessage="*" ForeColor="Red" ControlToValidate="txtJobNo"></asp:RequiredFieldValidator>
                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender4" runat="server" TargetControlID="txtJobNo"
                                FilterMode="InvalidChars" InvalidChars="qwertyuiopasdfghjklzxcvbnm[{]}`~,.<>/?:;'!@#$%^&*()_+|\-=\">
                            </cc1:FilteredTextBoxExtender>
                            
                        </div>
                     <%--   <div id="tag_label_transaction_popup_IGM2_empty_Exbond">
                            <a href="#" id="lnkbtn_Exbond" runat="server" style="display: none;" onclick="open_Exbond_Details(document.getElementById('txtJobNo').value);return false;">
                                Ex</a>
                        </div>--%>
                       <div id="tag_label_transaction_popup_IGM2_empty" style="margin-top: 4px;"></div>
                        <div id="tag_label_transaction_date" style="width: 125px;">
                          &nbsp;&nbsp; Custom House</div>
                        <div id="txtcon-m_transaction_code">
                            <asp:TextBox ID="txtCustomsHouse" runat="server" CssClass="txtbox_none_Mid_transac_code"
                                MaxLength="6" TabIndex="7" ClientIDMode="Static"></asp:TextBox>
                            <asp:HiddenField ID="hdn_CustomsHouse_Name" runat="server" ClientIDMode="Static" />
                        </div>
                        <div id="magnify_area">
                            <a href="#" class="magnfy" onclick="OpenPopupCustom_House(); return false" style="display: none;">
                            </a>
                        </div>
                        <div id="tag_label_transaction_Src_longlabel" style="width: 95px;">
                            Shipment Type</div>
                        <div id="txtcon-m_transaction_mode">
                            <asp:DropDownList ID="ddlContainer_Type" runat="server" CssClass="listtxt_transac_mode_new"
                                TabIndex="8" Enabled="false">
                            </asp:DropDownList>
                        </div>
                        <div id="tag_label_transaction_popup_no_empty" style="width: 40px">
                        </div>
                        <div id="tag_label_transaction_Src" style="width: 67px;">
                            File Ref.No
                        </div>
                        <div id="txtcon-m_transaction_code">
                            <asp:TextBox ID="txtFile_Ref_No" runat="server" MaxLength="20" CssClass="txtbox_none_Mid_transac_code"
                                TabIndex="9" ClientIDMode="Static"></asp:TextBox>
                        </div>
                        <div id="tag_label_transaction_popup_no_empty">
                        </div>
                             <div id="tag_label_transaction_Src">
                            Booking No
                        </div>
                        <div id="txtcon-m_transaction_code">
                           <asp:DropDownList ID="ddlBookingNo" runat="server" CssClass="listtxt_transac_src1"  Width="110px"
                                TabIndex="10">
                            </asp:DropDownList>
                            <asp:DropDownList ID="ddlFilingStatus" runat="server" CssClass="listtxt_transac_src1"
                                TabIndex="10" Visible="false">
                            </asp:DropDownList>
                        </div>
                        <div id="magnify_area">
                           
                        </div>
                       <%-- <div id="tag_label_transaction_Src">
                            Sales by
                        </div>
                        <div id="txtcon-m_transaction_code">
                            <asp:TextBox ID="txtSalesby" runat="server" MaxLength="30" CssClass="txtbox_none_Mid_transac_code"
                                TabIndex="9" ClientIDMode="Static"></asp:TextBox>
                            <asp:DropDownList ID="ddlFilingStatus" runat="server" CssClass="listtxt_transac_src1"
                                TabIndex="10" Visible="false">
                            </asp:DropDownList>
                        </div>
                        <div id="magnify_area">
                            <a href="#" class="magnfy" onclick="jQuery('#form1').validationEngine('hideAll');OpenPopupSalesMaster('Sales'); return false">
                            </a>
                        </div>
                        <asp:Button ID="Button3" CssClass="autowidnewbut" Text="+" runat="server" OnClientClick="jQuery('#form1').validationEngine('hideAll');opennewsalesnew(); return false" />
                    --%>
                    </div>
                </div>
            </div>
            <div id="innerbox_MidMain_Trans_new" style="height: 500px; width: 1150px;">
                <ol id="toc_new">
                    <li><a href="#page-1" onclick="tab_page1();" id="General"><span>Planning</span></a></li>
                   <%-- <li><a href="#page-2" onclick="tab_page2();" id="IGM"><span>IGM</span></a></li>--%>
                    <li><a href="#page-3" onclick="tab_page3();" id="INVOICE"><span></span></a></li>
                    <li><a href="#page-4" onclick="tab_page4();" id="ITEM"><span></span></a></li>
                    <%--<li><a href="#page-5" target="maineroyal" onclick="tab('0');"><span>Docket</span></a></li>--%>
                    <%--<li><a href="#page-6" target="maineroyal" onclick="tab('0');"><span>Tracking</span></a></li>--%>
                    <li style="display: none;"><a href="#page-5" onclick="tab_page5('../Report/Import_CheckList.aspx?jobno=',document.getElementById('txtJobNo').value);"
                        id="Checklist"><span>Checklist</span></a></li>
                    <li style="display: none;"><a href="#page-6" onclick="tab_page5('../FlatFile/Import_Flat_File_Generation.aspx?jobno=',document.getElementById('txtJobNo').value);"
                        id="Submission" runat="server"><span>Submission 2.1</span></a> </li>
                    <li style="display: none;"><a href="#page-7" onclick="tab_page6('../Job_History/Job_history.aspx?jobno=',document.getElementById('txtJobNo').value,' &jobdate=',document.getElementById('txtJobDate').value,' &type=IMP')"
                        id="Jobhistry"><span>Job History</span></a> </li>
                    <li style="display: none;"><a href="#page-8" visible="false" onclick="tab_page5('../FlatFile/E_Way_Bill_Generation.aspx?jobno=',document.getElementById('txtJobNo').value);"
                        id="E_Way_Bill" runat="server"><span></span></a></li>
                    <div id="Imp_Billing_Others1" runat="server" visible="false">
                        <li><a href="#page-9" onclick="tab_page9();" id="Imp_Billing_Others"><span></span></a>
                        </li>
                    </div>
                    <%--<li><a href="#page-8" target="maineroyal" onclick="tab('0');"><span>Job History</span></a></li>--%>
                    <div id="job_release" runat="server" visible="false">
                        <li><span style="margin-left: 340px;">
                            <asp:Button ID="JobRelease" runat="server" UseSubmitBehavior="false" 
                            OnClientClick="jQuery('#form1').validationEngine('hideAll')"
                                CausesValidation="false" Text="Job Release" OnClick="JobRelease_Click" />
                        </span>
                            <%--<a href="#page-8" style="text-align:right" id="JobRelease"><span style="text-align:right">Job Release</span></a></li>  --%>
                        </li>
                    </div>
                    <div style="display: none;">
                        &nbsp;&nbsp;
                        <asp:TextBox ID="txt_FF_Jobno" placeholder="Fr. Jobno" Width="150px" runat="server"
                            MaxLength="50" CssClass="txtbox_none_Mid_transac_exp_invoice_addr" TabIndex="8"></asp:TextBox>
                    </div>
                </ol>
                <div class="content" id="page-1">
                <div id="innerbox_MidMain_Trans_IGM" style="margin-top:-10px;margin-left: -17px;width:1144px;height:395px">
                <ol id="toc">
                    <li style="height:20px"><a href="#page-10" onclick="tab_page10('0'); jQuery('#form1').validationEngine('hideAll')"
                        id="Sub_General" runat="server"><span>General </span></a></li>
                   <%-- <li style="height:20px"><a href="#page-11" onclick="tab_page11('0'); jQuery('#form1').validationEngine('hideAll')"
                         id="CONTAINER" runat="server"><span>Invoice</span></a></li>
                  
--%>
                        
                </ol>
                <div class="content" id="page-10">
                    <div id="area_transac_container_genral" >
                        <asp:Panel ID="Panel1" runat="server">
                            <div id="pop_text_area_transac_popup_inn_container_new" style="width: 1120px">
                                <div id="pop_text_area_transac_popup_inn_container_left_new" style="width: 630px">
                                    <div id="tag_transac_lft_in1" style="width: 630px">
                                        <div id="tag_label_transaction_popup_gen">
                                            Client Name</div>
                                        <div id="txtcon-m_transaction_pop_gen1_srcic_new">
                                            <asp:TextBox ID="txtClientName" runat="server" MaxLength="50" onblur="ChangeCase(this);"
                                                Font-Bold="true" ClientIDMode="Static" CssClass="validate[required] txtbox_none_Mid_transac_pop_gen_srcic_new"
                                                Onkeypress="return numcharspl1(event)" ReadOnly="true" TabIndex="11"></asp:TextBox>
                                        </div>
                                        <div id="magnify_area">
                                            <div id="CND" runat="server" style="display: none;">
                                                <a href="#"  class="magnfy"  onclick="jQuery('#form1').validationEngine('hideAll');OpenPopupImporter_Master('CLIENTDETAILS'); return false">
                                                </a>
                                            </div>
                                            <div id="CWD" runat="server" style="display: none;">
                                                <a href="#" class="magnfy" onclick="jQuery('#form1').validationEngine('hideAll');OpenPopup_Imp_Exp_Ryal_Job('IMPORTER_DETAILS'); return false">
                                                </a>
                                            </div>
                                        </div>
                                        <asp:Button ID="Button1" Style="display: none;" CssClass="autowidnewbut" Text="+" runat="server" OnClientClick="jQuery('#form1').validationEngine('hideAll');openclient_Masternew_Imp_Exp('IMP'); return false" />
                                        <%--   <a href="#" onclick="jQuery('#form1').validationEngine('hideAll');openclient_Masternew(); return false">
                                            New</a>--%>
                                    </div>
                                    <div id="tag_transac_lft_in1" style="width: 630px">
                                        <div id="tag_label_transaction_popup_gen">
                                            Importer Name</div>
                                        <div id="txtcon-m_transaction_pop_gen1_srcic_new">
                                            <asp:TextBox ID="txtImporterName" runat="server" MaxLength="50" onblur="ChangeCase(this);"
                                                Font-Bold="true" ClientIDMode="Static" CssClass="validate[required] txtbox_none_Mid_transac_pop_gen_srcic_new"
                                                Onkeypress="return numcharspl1(event)" ReadOnly="true" TabIndex="12"></asp:TextBox>
                                        </div>
                                        <div id="magnify_area">
                                            <div id="ND" runat="server" style="display: none;">
                                                <a href="#" class="magnfy" onclick="jQuery('#form1').validationEngine('hideAll');OpenPopupImporter_Master('IMPORTER_DETAILS'); return false">
                                                </a>
                                            </div>
                                            <div id="WD" runat="server">
                                                <%--<a href="#" class="magnfy" onclick="jQuery('#form1').validationEngine('hideAll');OpenPopup_Imp_Exp_Ryal_Job('IMPORTER_DETAILS'); return false">
                                            </a>--%>
                                            </div>
                                        </div>
                                        <asp:Button ID="btnnew1" Style="display: none;" CssClass="autowidnewbut" Text="+" runat="server" OnClientClick="jQuery('#form1').validationEngine('hideAll');openclient_Masternew_Imp_Exp('IMP'); return false" />
                                        <%--   <a href="#" onclick="jQuery('#form1').validationEngine('hideAll');openclient_Masternew(); return false">
                                            New</a>--%>
                                    </div>
                                    <div id="tag_transac_lft_in1" style="width: 630px; display: none">
                                        <div id="tag_label_transaction_popup_gen">
                                            IEC_No
                                        </div>
                                        <div id="tag_label_transaction_popup_IGM2_empty">
                                        </div>
                                        <div id="divparty_ref" runat="server">
                                            <div id="tag_label_transaction_Src" style="text-align: center">
                                                Party Ref.No</div>
                                            <div id="txtcon-m_transaction_code" style="width: 163px;">
                                                <asp:TextBox ID="txtParty_RefNo" runat="server" MaxLength="25" CssClass="txtbox_none_Mid_transac_code"
                                                    Width="143px" ReadOnly="true" ClientIDMode="Static" TabIndex="12"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>
                                    <div id="tag_transac_lft_in1" style="width: 630px">
                                        <div id="tag_label_transaction_popup_gen">
                                            Branch SL.No</div>
                                        <div id="txtcon-m_transaction_item_maindet">
                                            <asp:TextBox ID="txtBranchNo" ReadOnly="true" runat="server" MaxLength="3" CssClass="validate[required,custom[number]]  txtbox_none_Mid_transac_item_maindet"
                                                ClientIDMode="Static" TabIndex="13" onkeypress="return pin(event)"></asp:TextBox>
                                        </div>
                                        <div id="magnify_area" style="display: none;">
                                            <a href="#" class="magnfy" onclick="jQuery('#form1').validationEngine('hideAll');OpenPopupImporter_Master_BranchSlno('Import_Branch');">
                                            </a>
                                        </div>
                                        <div id="tag_label_transaction_Src" style="text-align: center">
                                            IEC No</div>
                                        <div id="txtcon-m_transaction_code">
                                            <asp:TextBox ID="txtIECNo" runat="server" ReadOnly="true" MaxLength="10" ClientIDMode="Static" CssClass=" txtbox_none_Mid_transac_code"
                                                TabIndex="14" onkeypress="return charnum2(event)"></asp:TextBox>
                                        </div>
                                    </div>
                                     <div id="Adcode_div" runat="server" >
                                    <div id="tag_transac_lft_in1" style="width: 630px">
                                        <div id="tag_label_transaction_popup_gen">
                                            AD Code</div>
                                        <div id="txtcon-m_transaction_pop_gen1_srcic_new">
                                            <asp:TextBox ID="txt_ADCode" runat="server" ReadOnly="true" MaxLength="14" CssClass="txtbox_none_Mid_transac_pop_gen_srcic_new"
                                                Onkeypress="return numchar(event)" onkeyup="ChangeCase(this);" onfocus="ChangeCase(this);"
                                                onblur="ChangeCase(this);" ClientIDMode="Static" TabIndex="15"></asp:TextBox>
                                        </div>
                                        <div id="magnify_area" style="display: none;">
                                            <a class="magnfy" href="#" onclick="jQuery('#form1').validationEngine('hideAll');OpenPopupAdcode(document.getElementById('txtImporterName').value,document.getElementById('txtIECNo').value,'IMPORT'); return false">
                                            </a>
                                        </div>
                                    </div>
                                    </div>
                                    <div id="tag_transac_lft_in1" style="width: 630px">
                                        <div id="tag_label_transaction_popup_gen">
                                            Supplier Name</div>
                                        <div id="txtcon-m_transaction_pop_gen1_srcic_new">
                                            <asp:TextBox ID="txtSuppliername" runat="server" MaxLength="50" onblur="ChangeCase(this);"
                                                Font-Bold="true" ClientIDMode="Static" ReadOnly="true" CssClass="txtbox_none_Mid_transac_pop_gen_srcic_new"
                                                Onkeypress="return numcharspl1(event)" TabIndex="16"></asp:TextBox>
                                        </div>
                                        <div id="magnify_area" style="display: none;">
                                            <div id="CND1" runat="server">
                                                <a href="#" class="magnfy" onclick="jQuery('#form1').validationEngine('hideAll');OpenPopupSupplier('Sup_Supplier'); return false">
                                                </a>
                                            </div>
                                            <div id="CWD1" runat="server">
                                                <a href="#" class="magnfy" onclick="jQuery('#form1').validationEngine('hideAll');OpenPopup_Imp_Exp_Ryal_Job('IMPORTER_DETAILS'); return false">
                                                </a>
                                            </div>
                                        </div>
                                        <asp:Button ID="Button11" Style="display: none;" CssClass="autowidnewbut" Text="+" runat="server" OnClientClick="jQuery('#form1').validationEngine('hideAll');openSupplier_Masternew(); return false" />
                                        <%--   <a href="#" onclick="jQuery('#form1').validationEngine('hideAll');openclient_Masternew(); return false">
                                            New</a>--%>
                                    </div>
                                    <div id="tag_transac_lft_in1" style="width: 630px">
                                        <div id="tag_label_transaction_popup_gen">
                                            Origin Agent</div>
                                        <div id="txtcon-m_transaction_pop_gen1_srcic_new">
                                            <asp:TextBox ID="txtOriginAgent" runat="server" ReadOnly="true" MaxLength="50" onblur="ChangeCase(this);"
                                                Font-Bold="true" ClientIDMode="Static" CssClass="txtbox_none_Mid_transac_pop_gen_srcic_new"
                                                Onkeypress="return numcharspl1(event)" TabIndex="17"></asp:TextBox>
                                        </div>
                                        <div id="magnify_area" style="display: none;">
                                            <div id="CND2" runat="server">
                                                <a href="#" class="magnfy" onclick="jQuery('#form1').validationEngine('hideAll');OpenPopupAgent(document.getElementById('txtImporterName').value,document.getElementById('txtIECNo').value,'IMPORT'); return false">
                                                </a>
                                            </div>
                                            <div id="CWD2" runat="server">
                                                <a href="#" class="magnfy" onclick="jQuery('#form1').validationEngine('hideAll');OpenPopupAgent(document.getElementById('txtImporterName').value,document.getElementById('txtIECNo').value,'IMPORT'); return false">
                                                </a>
                                            </div>
                                        </div>
                                        <asp:Button ID="Button12" Style="display: none;" CssClass="autowidnewbut" Text="+" runat="server" OnClientClick="jQuery('#form1').validationEngine('hideAll');opennewAgentnew(); return false" />
                                        <%--   <a href="#" onclick="jQuery('#form1').validationEngine('hideAll');openclient_Masternew(); return false">
                                            New</a>--%>
                                    </div>
                                    <div id="tag_transac_lft_in1" style="width: 630px">
                                        <div id="tag_label_transaction_popup_gen">
                                            Notify</div>
                                        <div id="txtcon-m_transaction_pop_gen1_srcic_new">
                                            <asp:TextBox ID="txtNotify" runat="server" ReadOnly="true" MaxLength="50" onblur="ChangeCase(this);"
                                                Font-Bold="true" ClientIDMode="Static" CssClass="txtbox_none_Mid_transac_pop_gen_srcic_new"
                                                Onkeypress="return numcharspl1(event)" TabIndex="18"></asp:TextBox>
                                        </div>
                                        <div id="magnify_area" style="display: none;">
                                            <div id="CND3" runat="server">
                                           <%--   <a href="#" class="magnfy" onclick="jQuery('#form1').validationEngine('hideAll');OpenPopupImporter_Master('NOTIFY'); return false">
                                                </a>--%>
                                                <a href="#" class="magnfy" onclick="jQuery('#form1').validationEngine('hideAll');OpenPopupNotify(document.getElementById('txtImporterName').value,document.getElementById('txtIECNo').value,'IMPORT'); return false">
                                                </a>
                                            </div>
                                        </div>
                                        <asp:Button ID="Button14" Style="display: none;" CssClass="autowidnewbut" Text="+" runat="server" OnClientClick="jQuery('#form1').validationEngine('hideAll');opennewNotifynew(); return false" />
                                        <%--   <a href="#" onclick="jQuery('#form1').validationEngine('hideAll');openclient_Masternew(); return false">
                                            New</a>--%>
                                    </div>
									
									<div id="tag_transac_lft_in1" style="width: 630px">
                                        <div id="tag_label_transaction_popup_gen">
                                            Forwarder</div>
                                        <div id="txtcon-m_transaction_pop_gen1_srcic_new">
                                            <asp:TextBox ID="txtForwarder" runat="server" ReadOnly="true" MaxLength="50" onblur="ChangeCase(this);"
                                                Font-Bold="true" ClientIDMode="Static" CssClass="txtbox_none_Mid_transac_pop_gen_srcic_new"
                                                Onkeypress="return numcharspl1(event)" TabIndex="19"></asp:TextBox>
                                        </div>
                                        <div id="magnify_area" style="display: none;">
                                            <div id="CND13" runat="server">
                                                <a href="#" class="magnfy" onclick="jQuery('#form1').validationEngine('hideAll');Listpopup_OpenPopupForwarder('FORWARDER'); return false">
                                                </a>
                                            </div>
                                        </div>
                                        <asp:Button ID="Button15" Style="display: none;" CssClass="autowidnewbut" Text="+" runat="server" OnClientClick="jQuery('#form1').validationEngine('hideAll');opennewforwardernew(); return false" />
                                        <%--   <a href="#" onclick="jQuery('#form1').validationEngine('hideAll');openclient_Masternew(); return false">
                                            New</a>--%>
                                    </div>
									
									<div id="tag_transac_lft_in1" style="width: 630px">
                                        <div id="tag_label_transaction_popup_gen">
                                            Third Party</div>
                                        <div id="txtcon-m_transaction_pop_gen1_srcic_new">
                                            <asp:TextBox ID="txtThirdParty" runat="server" ReadOnly="true" MaxLength="50" onblur="ChangeCase(this);"
                                                Font-Bold="true" ClientIDMode="Static" CssClass="txtbox_none_Mid_transac_pop_gen_srcic_new"
                                                Onkeypress="return numcharspl1(event)" TabIndex="20"></asp:TextBox>
                                        </div>
                                        <div id="magnify_area" style="display: none;">
                                            <div id="CND14" runat="server">
                                                <a href="#" class="magnfy" onclick="jQuery('#form1').validationEngine('hideAll');Listpopup_OpenPopupThirdparty('THIRDPARTY'); return false">
                                                </a>
                                            </div>
                                        </div>
                                        <asp:Button ID="Button16" Style="display: none;" CssClass="autowidnewbut" Text="+" runat="server" OnClientClick="jQuery('#form1').validationEngine('hideAll');opennewThirdpartynew(); return false" />
                                        <%--   <a href="#" onclick="jQuery('#form1').validationEngine('hideAll');openclient_Masternew(); return false">
                                            New</a>--%>
                                    </div>
									<div id="tag_transac_lft_in1" style="width: 630px">
                                        <div id="tag_label_transaction_popup_gen">
                                            Clearing Agent</div>
                                        <div id="txtcon-m_transaction_pop_gen1_srcic_new">
                                            <asp:TextBox ID="txtClearingAgent" runat="server" ReadOnly="true" MaxLength="50" onblur="ChangeCase(this);"
                                                Font-Bold="true" ClientIDMode="Static" CssClass="txtbox_none_Mid_transac_pop_gen_srcic_new"
                                                Onkeypress="return numcharspl1(event)" TabIndex="21"></asp:TextBox>
                                        </div>
                                        <div id="magnify_area" style="display: none;">
                                            <div id="CND15" runat="server">
                                                <a href="#" class="magnfy" onclick="jQuery('#form1').validationEngine('hideAll');Listpopup_OpenPopupClearingagent('CLEARINGAGENT'); return false">
                                                </a>
                                            </div>
                                        </div>
                                        <asp:Button ID="Button17" Style="display: none;" CssClass="autowidnewbut" Text="+" runat="server" OnClientClick="jQuery('#form1').validationEngine('hideAll');opennewClearingAgentnew(); return false" />
                                        <%--   <a href="#" onclick="jQuery('#form1').validationEngine('hideAll');openclient_Masternew(); return false">
                                            New</a>--%>
                                    </div>
									<div id="tag_transac_lft_in1" style="width: 630px">
                                        <div id="tag_label_transaction_popup_gen">
                                            Nomination</div>
                                        <div id="txtcon-m_transaction_pop_gen1_srcic_new">
                                            <asp:TextBox ID="txtNomination" runat="server" ReadOnly="true" MaxLength="50" onblur="ChangeCase(this);"
                                                Font-Bold="true" ClientIDMode="Static" CssClass="txtbox_none_Mid_transac_pop_gen_srcic_new"
                                                Onkeypress="return numcharspl1(event)" TabIndex="22"></asp:TextBox>
                                        </div>
                                        <div id="magnify_area" style="display: none;">
                                            <div id="CND16" runat="server">
                                                <a href="#" class="magnfy" onclick="jQuery('#form1').validationEngine('hideAll');Listpopup_OpenPopupNomination('NOMINATION'); return false">
                                                </a>
                                            </div>
                                        </div>
                                        <asp:Button ID="Button18" Style="display: none;" CssClass="autowidnewbut" Text="+" runat="server" OnClientClick="jQuery('#form1').validationEngine('hideAll');opennewNominationnew(); return false" />
                                        <%--   <a href="#" onclick="jQuery('#form1').validationEngine('hideAll');openclient_Masternew(); return false">
                                            New</a>--%>
                                    </div>
									<div id="tag_transac_lft_in1" style="width: 630px">
                                        <div id="tag_label_transaction_popup_gen">
                                            Co-Loader Name</div>
                                        <div id="txtcon-m_transaction_pop_gen1_srcic_new">
                                            <asp:TextBox ID="txtCoLoaderName" ReadOnly="true" runat="server"  MaxLength="50" onblur="ChangeCase(this);"
                                                Font-Bold="true" ClientIDMode="Static" CssClass="txtbox_none_Mid_transac_pop_gen_srcic_new"
                                                Onkeypress="return numcharspl1(event)" TabIndex="23" Enabled="false"></asp:TextBox>
                                        </div>
                                        <div id="magnify_area" style="display: none;">
                                            <div id="CND1c7" runat="server">
                                                <a href="#" class="magnfy" onclick="jQuery('#form1').validationEngine('hideAll');Listpopup_OpenPopupColoader('COLOADER'); return false" style="pointer-events: none; cursor: default;">
                                                </a>
                                            </div>
                                        </div>
                                        <asp:Button ID="Buttonc19"  CssClass="autowidnewbut" Text="+" runat="server" OnClientClick="jQuery('#form1').validationEngine('hideAll');opennewColoadernew(); return false" style="display: none; cursor: default;" />
                                       
                                    </div>
									
                               
                                    </div>
                                   
                                <div id="pop_text_area_transac_popup_inn_container_rght_new" style="width: 475px">
                                    <div id="tag_transac_lft_in1" style="width: 630px">
                                        <div id="tag_label_transaction_popup_gen">
                                            Country Origin
                                        </div>
                                        <div id="txtcon-m_transaction_pop_gen1_srcic_new">
                                          
                                            <asp:TextBox ID="txtCountryName" ReadOnly="true" runat="server" MaxLength="30" onkeyup="ChangeCase(this);"
                                                onblur="ChangeCase(this);updateTextbox();" CssClass="txtbox_none_Mid_transac_pop_gen_srcic_new"
                                                ClientIDMode="Static" TabIndex="24" onkeypress="return char(event)" onchange="updateTextbox()"
                                                onfocus="updateTextbox();" onkeydown="updateTextbox();"></asp:TextBox>
                                            <asp:HiddenField ID="txtCountryCode" runat="server" ClientIDMode="Static" />
                                        </div>
                                        <div id="magnify_area" style="display: none;">
                                            <a href="#" class="magnfy" onclick="jQuery('#form1').validationEngine('hideAll');OpenPopupCountry_Master('Country_origin'); return false">
                                            </a>
                                        </div>
                                    </div>
                                    <div id="tag_transac_lft_in1" style="width: 630px">
                                        <div id="tag_label_transaction_popup_gen">
                                            Country of Shipment
                                        </div>
                                        <div id="txtcon-m_transaction_pop_gen1_srcic_new">
                                          
                                            <asp:TextBox ID="txtCountryShipment" ReadOnly="true" runat="server" MaxLength="30" onblur="ChangeCase(this);"
                                                CssClass="txtbox_none_Mid_transac_pop_gen_srcic_new" ClientIDMode="Static" TabIndex="25"
                                                onkeypress="return char(event)" onkeyup="ChangeCase(this);"></asp:TextBox>
                                            <asp:HiddenField ID="txtCountryShipmentCode" runat="server" ClientIDMode="Static" />
                                        </div>
                                        <div id="magnify_area" style="display: none;">
                                            <a href="#" class="magnfy" onclick="jQuery('#form1').validationEngine('hideAll');OpenPopupCountry_Master('ship'); return false">
                                            </a>
                                        </div>
                                    </div>

                                    <div id="tag_transac_lft_in1" style="width: 630px">
                                        <div id="tag_label_transaction_popup_gen">
                                            Port of Origin
                                        </div>
                                        <div id="txtcon-m_transaction_pop_gen1_srcic_new">
                                          
                                            <asp:TextBox ID="txtPortOrigin" runat="server" ReadOnly="true" MaxLength="30" onkeypress="return char(event),TextBoxes(this);"
                                                onblur="ChangeCase(this);updateTextbox2();" onkeyup="ChangeCase(this);" onfocus="updateTextbox2();"
                                                onkeydown="var data = val;Port_origin(this);" onchange="updateTextbox2();" CssClass="txtbox_none_Mid_transac_pop_gen_srcic_new"
                                                ClientIDMode="Static" TabIndex="26"></asp:TextBox>
                                        </div>
                                        <div id="magnify_area" style="display: none;">
                                            <a href="#" class="magnfy" onclick="jQuery('#form1').validationEngine('hideAll');OpenPopupPort_CountrySearch_Master(''); return false">
                                            </a>
                                        </div>
                                    </div>
									
									
 <div id="tag_transac_lft_in1" style="width: 630px">
 <div id="tag_label_transaction_popup_gen">
                                            ETD </div>
                                        <div id="txtcon-m_transaction_code">
                                            <asp:TextBox ID="txtETDdate" ReadOnly="true" Font-Size="12px" runat="server" MaxLength="8" CssClass="txtbox_none_Mid_transac_code"
                                                ClientIDMode="Static" TabIndex="27"></asp:TextBox>
                                            <cc1:MaskedEditExtender ID="MaskedEditExtender41"  runat="server" TargetControlID="txtETDdate"
                                                Mask="99/99/9999" MessageValidatorTip="true" OnInvalidCssClass="MaskedEditError"
                                                MaskType="Date" DisplayMoney="Left" AcceptNegative="Left" ErrorTooltipEnabled="True">
                                            </cc1:MaskedEditExtender>
                                            <cc1:CalendarExtender ID="CalendarExtender51" runat="server" TargetControlID="txtETDdate"
                                                PopupButtonID="txtETDdate" Format="dd/MM/yyyy">
                                            </cc1:CalendarExtender>
                                        </div>
                                        <div id="tag_label_transaction_popup_IGM2_empty">
                                        </div>
                                        <div id="tag_label_transaction_Src_new" style="width: 45px; text-align: center;">
                                            Time
                                        </div>
                                        <div id="txtcon-m_transaction_code">
                                         <asp:XmlDataSource ID="XmlDataSource1" runat="server" DataFile="~/INDEV/Hrs.xml"  XPath="ListItems/ListItem"></asp:XmlDataSource>
                                         <asp:XmlDataSource ID="XmlDataSource2" runat="server" DataFile="~/INDEV/Mins.xml"  XPath="ListItems/ListItem"></asp:XmlDataSource>

                                            <asp:DropDownList ID="ddlhrs_ETD_dat" runat="server" CssClass="listtxt_transac_mode_new" TabIndex="28"
                                                Width="40px" DataSourceID="XmlDataSource1" DataTextField="text" DataValueField="value">
                                            </asp:DropDownList>
                                            <asp:DropDownList ID="ddlsec_ETD_dat" runat="server" CssClass="listtxt_transac_mode_new" TabIndex="29"
                                                Width="40px" DataSourceID="XmlDataSource1" DataTextField="text" DataValueField="value">
                                            </asp:DropDownList>
                                        </div>
                                    </div>
                                    <div id="tag_transac_lft_in1" style="width: 630px">
                                        <div id="tag_label_transaction_popup_gen">
                                            Port of Loading
                                        </div>
                                        <div id="txtcon-m_transaction_pop_gen1_srcic_new">
                                        
                                            <asp:TextBox ID="txtPortShipment" ReadOnly="true" runat="server" MaxLength="30" onkeypress="return char(event)"
                                                onblur="ChangeCase(this);" onkeyup="ChangeCase(this);" onkeydown="ChangeCase(this);var data = val;Port_origin(this);"
                                                CssClass="txtbox_none_Mid_transac_pop_gen_srcic_new" ClientIDMode="Static" TabIndex="30"></asp:TextBox>
                                        </div>
                                        <div id="magnify_area" style="display: none;">
                                            <a href="#" class="magnfy"  onclick="jQuery('#form1').validationEngine('hideAll');OpenPopupPort_CountrySearch_Master('Port_Ship'); return false">
                                            </a>
                                        </div>
                                    </div>
                                    <div id="tag_transac_lft_in1" style="width: 630px">
                                        <div id="tag_label_transaction_popup_gen">
                                            Port of Discharge
                                        </div>
                                        <div id="txtcon-m_transaction_pop_gen1_srcic_new">
                                        
                                            <asp:TextBox ID="txtPortofdelivery" ReadOnly="true" runat="server" MaxLength="30" onkeypress="return char(event)"
                                                onblur="ChangeCase(this);" onkeyup="ChangeCase(this);" onkeydown="ChangeCase(this);var data = val;Port_origin(this);"
                                                CssClass="txtbox_none_Mid_transac_pop_gen_srcic_new" ClientIDMode="Static" TabIndex="31"></asp:TextBox>
                                        </div>
                                        <div id="magnify_area" style="display: none;">
                                            <a href="#" class="magnfy" onclick="jQuery('#form1').validationEngine('hideAll');OpenPopupPort_CountrySearch_Master('Delivery'); return false">
                                            </a>
                                        </div>
                                    </div>
									
 <div id="tag_transac_lft_in1" style="width: 630px">
 <div id="tag_label_transaction_popup_gen">
                                            ETA </div>
                                        <div id="txtcon-m_transaction_code">
                                            <asp:TextBox ID="txtETAdate" Font-Size="12px" ReadOnly="true" runat="server" MaxLength="8" CssClass="txtbox_none_Mid_transac_code"
                                                ClientIDMode="Static" TabIndex="32"></asp:TextBox>
                                            <cc1:MaskedEditExtender ID="MaskedEditExtender42" runat="server" TargetControlID="txtETAdate"
                                                Mask="99/99/9999" MessageValidatorTip="true" OnInvalidCssClass="MaskedEditError"
                                                MaskType="Date" DisplayMoney="Left" AcceptNegative="Left" ErrorTooltipEnabled="True">
                                            </cc1:MaskedEditExtender>
                                            <cc1:CalendarExtender ID="CalendarExtender52" runat="server" TargetControlID="txtETAdate"
                                                PopupButtonID="txtETAdate" Format="dd/MM/yyyy">
                                            </cc1:CalendarExtender>
                                        </div>
                                        <div id="tag_label_transaction_popup_IGM2_empty">
                                        </div>
                                        <div id="tag_label_transaction_Src_new" style="width: 45px; text-align: center;">
                                            Time
                                        </div>
                                        <div id="txtcon-m_transaction_code">
                                         
                                            <asp:DropDownList ID="ddlhrs_ETA_dat" runat="server" CssClass="listtxt_transac_mode_new" TabIndex="33"
                                                Width="40px" DataSourceID="XmlDataSource1" DataTextField="text" DataValueField="value">
                                            </asp:DropDownList>
                                            <asp:DropDownList ID="ddlsec_ETA_dat" runat="server" CssClass="listtxt_transac_mode_new" TabIndex="34"
                                                Width="40px" DataSourceID="XmlDataSource1" DataTextField="text" DataValueField="value">
                                            </asp:DropDownList>
                                        </div>
                                    </div>
									
                                    <div id="tag_transac_lft_in1" style="width: 630px">
                                        <div id="tag_label_transaction_popup_gen">
                                            Final Port of Delivery
                                        </div>
                                        <div id="txtcon-m_transaction_pop_gen1_srcic_new">
                                            
                                            <asp:TextBox ID="txtFinalPortofdelivery" ReadOnly="true" runat="server" MaxLength="30" onkeypress="return char(event)"
                                                onblur="ChangeCase(this);" onkeyup="ChangeCase(this);" onkeydown="ChangeCase(this);var data = val;Port_origin(this);"
                                                CssClass="txtbox_none_Mid_transac_pop_gen_srcic_new" ClientIDMode="Static" TabIndex="35"></asp:TextBox>
                                        </div>
                                        <div id="magnify_area" style="display: none;">
                                            <a href="#" class="magnfy" onclick="jQuery('#form1').validationEngine('hideAll');OpenPopupPort_CountrySearch_Master('Final'); return false">
                                            </a>
                                        </div>
                                    </div>
									
 <div id="tag_transac_lft_in1" style="width: 630px">
 <div id="tag_label_transaction_popup_gen">
                                            ATA </div>
                                        <div id="txtcon-m_transaction_code">
                                            <asp:TextBox ID="txtATAdate" ReadOnly="true" Font-Size="12px" runat="server" MaxLength="8" CssClass="txtbox_none_Mid_transac_code"
                                                ClientIDMode="Static" TabIndex="36"></asp:TextBox>
                                            <cc1:MaskedEditExtender ID="MaskedEditExtender43" runat="server" TargetControlID="txtATAdate"
                                                Mask="99/99/9999" MessageValidatorTip="true" OnInvalidCssClass="MaskedEditError"
                                                MaskType="Date" DisplayMoney="Left" AcceptNegative="Left" ErrorTooltipEnabled="True">
                                            </cc1:MaskedEditExtender>
                                            <cc1:CalendarExtender ID="CalendarExtender53" runat="server" TargetControlID="txtATAdate"
                                                PopupButtonID="txtATAdate" Format="dd/MM/yyyy">
                                            </cc1:CalendarExtender>
                                        </div>
                                        <div id="tag_label_transaction_popup_IGM2_empty">
                                        </div>
                                        <div id="tag_label_transaction_Src_new" style="width: 45px; text-align: center;">
                                            Time
                                        </div>
                                        <div id="txtcon-m_transaction_code">
                                         
                                            <asp:DropDownList ID="ddlhrs_ATA_dat" runat="server" CssClass="listtxt_transac_mode_new" TabIndex="37"
                                                Width="40px" DataSourceID="XmlDataSource1" DataTextField="text" DataValueField="value">
                                            </asp:DropDownList>
                                            <asp:DropDownList ID="ddlsec_ATA_dat" runat="server" CssClass="listtxt_transac_mode_new" TabIndex="38"
                                                Width="40px" DataSourceID="XmlDataSource1" DataTextField="text" DataValueField="value">
                                            </asp:DropDownList>
                                        </div>
                                    </div>
                                    <div id="tag_transac_lft_in1" style="width: 630px">
                                        <div id="tag_label_transaction_popup_gen">
                                            Clearance Place
                                        </div>
                                        <div id="txtcon-m_transaction_item_maindet">
                                           
                                            <asp:TextBox ID="txtclearanceplace" ReadOnly="true" runat="server" MaxLength="30" onkeypress="return char(event)"
                                                onblur="ChangeCase(this);" onkeyup="ChangeCase(this);" onkeydown="ChangeCase(this);var data = val;Port_origin(this);"
                                                CssClass="txtbox_none_Mid_transac_item_maindet" ClientIDMode="Static" TabIndex="39"></asp:TextBox>
                                        </div>
                                        <div id="magnify_area" style="display: none;">
                                            <a href="#" class="magnfy" onclick="jQuery('#form1').validationEngine('hideAll');ListPopupOpenPopupClearance('Air'); return false">
                                            </a>
                                        </div>
                                         <div id="tag_label_transaction_popup_no_empty">
                        </div>
                                            <div id="tag_label_transaction_popup_gen" style="width: 100px;">
                                                 Clearance Date</div>
                                            <div id="txtcon-m_transaction_code">
                                                <asp:TextBox ID="txtCleardate" ReadOnly="true" runat="server" MaxLength="10" ClientIDMode="Static"
                                                    CssClass="txtbox_none_Mid_transac_code" TabIndex="49" onkeypress="return numcharspl1(event)"></asp:TextBox>
                                                <cc1:MaskedEditExtender ID="MaskedEditExtender6" runat="server" TargetControlID="txtCleardate"
                                                    Mask="99/99/9999" MessageValidatorTip="true" OnInvalidCssClass="MaskedEditError"
                                                    MaskType="Date" DisplayMoney="Left" AcceptNegative="Left" ErrorTooltipEnabled="True">
                                                </cc1:MaskedEditExtender>
                                                <cc1:CalendarExtender ID="CalendarExtender6" runat="server" TargetControlID="txtCleardate"
                                                    PopupButtonID="txtCleardate" Format="dd/MM/yyyy">
                                                </cc1:CalendarExtender>
                                            </div>

                                    </div>
                                  <div id="Operation_SEA1" runat="server" >
								<div id="tag_transac_lft_in1" style="width: 630px">
                                            <div id="tag_label_transaction_popup_gen">
                                                Job HandleBy</div>
                                            <div id="txtcon-m_transaction_pop_gen1_srcic_new">
                                                <asp:TextBox ID="txtJobhandledby" ReadOnly="true" runat="server" MaxLength="50" CssClass="txtbox_none_Mid_transac_pop_gen_srcic_new"
                                                    onkeypress="return numcharspl1(event);" ClientIDMode="Static" TabIndex="40"></asp:TextBox>
                                            </div>
                                            <div id="magnify_area" style="display: none;">
                                             <a href="#" class="magnfy" onclick="jQuery('#form1').validationEngine('hideAll');ListPopupOpenPopupJobuser('Air'); return false">
                                </a>
                                            </div>
                                        </div>
								
                                  </div>
                                      
                                       
                                        
                                   <div id="Operation_SEA2" runat="server" style="display: none;">
                                        <div id="tag_transac_lft_in1" style="width: 630px">
                                            <div id="tag_label_transaction_popup_gen">
                                                Operation HandleBy</div>
                                            <div id="txtcon-m_transaction_pop_gen1_srcic_new">
                                                <asp:TextBox ID="txtOperHandledBy" runat="server" MaxLength="50" CssClass="txtbox_none_Mid_transac_pop_gen_srcic_new"
                                                    onkeypress="return numcharspl1(event);" ClientIDMode="Static" TabIndex="40"></asp:TextBox>
                                            </div>
                                            <div id="magnify_area" style="display: none;">
                                             <a href="#" class="magnfy" onclick="jQuery('#form1').validationEngine('hideAll');ListPopupOpenPopupJobuser('Sea'); return false">
                                </a>
                                            </div>
                                        </div>
                                   </div>

                                    <div id="tag_transac_lft_in1" style="width: 520px">
                            <div id="tag_label_transaction_popup_gen" >
                               Sales by
                            </div>
                            <div id="txtcon-m_transaction_pop_gen1_srcic_new" style="width:250px">
                                   <asp:TextBox ID="txtSalesby" ReadOnly="true" TabIndex="40" CssClass="txtbox_none_Mid_transac_pop_gen_srcic_new" Width="250px"
                                runat="server" MaxLength="25"></asp:TextBox>

                        
                            </div>
                            <div id="magnify_area" style="display: none;">
                                <a href="#" class="magnfy" onclick="jQuery('#form1').validationEngine('hideAll');OpenPopupSalesMaster('Sales'); return false">
                            </a>
                            </div>
                             <asp:Button ID="Button3" Style="display: none;" CssClass="autowidnewbut" Text="+" runat="server" 
                        OnClientClick="jQuery('#form1').validationEngine('hideAll');opennewsalesnew(); return false" />
                        </div>

                             <div id="tag_transac_lft_in1" style="width: 630px">
                                        <div id="tag_label_transaction_popup_gen">
                                            Quotation No</div>
                                        <div id="txtcon-m_transaction_pop_gen1_srcic_new" style="width:163px">
                                           <asp:DropDownList ID="ddlQuotationNO" ReadOnly="true" runat="server" CssClass="listtxt_transac_src1" Width="163px"></asp:DropDownList>
                                        </div>
                                       
                                    </div>
                                </div>
                            </div>
                             </asp:Panel>
                    </div>
                </div>
                   <div class="content" id="page-11">
                    <div id="area_transac_container_genral" >
					<div id="pop_text_area_transac_popup_inn_container_new" style="width: 1120px">
                                <div id="pop_text_area_transac_popup_inn_container_left_new" style="width: 630px">
								 <div id="tag_transac_lft_in1" style="width: 630px">
                                        <div id="tag_label_transaction_popup_gen">
                                            HS Code</div>
                                        <div id="txtcon-m_transaction_pop_gen1_srcic_new">
                                            <asp:TextBox ID="txt_HSCode" runat="server" MaxLength="14" CssClass="txtbox_none_Mid_transac_pop_gen_srcic_new"
                                                onkeypress="return pin(event);" ClientIDMode="Static" TabIndex="41"></asp:TextBox>
                                        </div>
                                        <div id="magnify_area">
                                            <a class="magnfy" href="#" onclick="jQuery('#form1').validationEngine('hideAll');OpenPopupCTH('HS_CODE'); return false">
                                            </a>
                                        </div>
                                    </div>
   <div id="tag_transac_lft_in1" style="width: 630px; height: 80px">
                                        <div id="tag_label_transaction_popup_gen">
                                            Commodity</div>
                                        <div id="txtcon-m_transaction_item_maindet" style="width: 420px;height: 80px">
                                            <asp:TextBox ID="txtCommodity" runat="server" TextMode="MultiLine" MaxLength="120"
                                                onkeyDown="return checkTextAreaMaxLength(this,event,'120');" onpaste="return checkTextAreaMaxLength(this,event,'120');"
                                                CssClass="txtbox_none_Mid_transac_item_other_rmarks_imp" ClientIDMode="Static"
                                                TabIndex="42"  Height="70px"></asp:TextBox>
                                        </div>
                                        <div id="tag_label_transaction_popup_no_empty">
                                        </div>
                                       
                                    </div>
									   <div id="tag_transac_lft_in1" style="width: 630px">
									    <div id="tag_label_transaction_popup_gen">
                                            Commodity Type</div>
                                        <div id="txtcon-m_transaction_item_maindet">
                                            <%--<asp:TextBox ID="txtCommoditytype" runat="server" MaxLength="10" ClientIDMode="Static" CssClass="validate[required,minSize[10]] txtbox_none_Mid_transac_code"
                                                TabIndex="11" onkeypress="return pin(event)"></asp:TextBox>--%>
                                            <asp:DropDownList ID="ddl_info_type" runat="server" TabIndex="43" class="chosen-select-deselect"
                                                Style="width: 75px; font-family: Arial,Helvetica,sans-serif;" data-placeholder="Choose a Type"
                                                AutoPostBack="false" >
                                            </asp:DropDownList>
                                        </div>
										 <div id="tag_label_transaction_popup_no_empty">
                        </div>
										
                                        <div id="tag_label_transaction_popup_gen">
                                            If Others Specify</div>
                                        <div id="txtcon-m_transaction_pop_gen1_srcic_new">
                                            <asp:TextBox ID="txtOtherSpecify" runat="server" MaxLength="14" CssClass="txtbox_none_Mid_transac_pop_gen_srcic_new"
                                                onkeypress="return numcharspl1(event);" ClientIDMode="Static" TabIndex="44" Width="275px"></asp:TextBox>
                                        </div>
                                      
                                   
									   </div>
									    <div id="tag_transac_lft_in1" style="width: 630px">
									    <div id="tag_label_transaction_popup_gen">
                                            AEO Type</div>
                                        <div id="txtcon-m_transaction_item_maindet">
                                            <%--<asp:TextBox ID="txtAEOtype" runat="server" MaxLength="10" ClientIDMode="Static" CssClass="validate[required,minSize[10]] txtbox_none_Mid_transac_code"
                                                TabIndex="11" onkeypress="return pin(event)"></asp:TextBox>--%>
                                            <asp:DropDownList ID="ddl_AEOtypoe" runat="server" TabIndex="45" class="chosen-select-deselect"
                                                Style="width: 75px; font-family: Arial,Helvetica,sans-serif;" data-placeholder="Choose a Type"
                                                AutoPostBack="false" >
                                            </asp:DropDownList>
                                        </div>
										 <div id="tag_label_transaction_popup_no_empty">
                        </div>
										
                                        <div id="tag_label_transaction_popup_gen">
                                            AEO No</div>
                                        <div id="txtcon-m_transaction_pop_gen1_srcic_new">
                                            <asp:TextBox ID="txtAEONo" runat="server" MaxLength="14" CssClass="txtbox_none_Mid_transac_pop_gen_srcic_new"
                                                onkeypress="return numcharspl1(event);" ClientIDMode="Static" TabIndex="46" Width="275px"></asp:TextBox>
                                        </div>
                                      
                                   
									   </div>
                                   
                                    <div id="tag_transac_lft_in1" style="width: 630px;">
                                        <div id="tag_label_transaction_popup_gen">
                                            Remarks</div>
                                        <div id="txtcon-m_transaction_item_maindet" style="width: 420px;">
                                            <asp:TextBox ID="txtMarksNos" TextMode="MultiLine" TabIndex="47" runat="server" CssClass="txtbox_none_Mid_transac_item_other_rmarks_imp"
                                                MaxLength="300" onkeyDown="return checkTextAreaMaxLength(this,event,'300');"
                                                onpaste="return checkTextAreaMaxLength(this,event,'300');" Height="80px"></asp:TextBox>
                                        </div>
                                    </div>
                    </div>
					 
					  <div id="pop_text_area_transac_popup_inn_container_rght_new" style="width: 475px">
					  
					    <div id="tag_transac_lft_in1" style="width: 630px">
                                            <div id="tag_label_transaction_popup_gen">
                                                Invoice No</div>
                                            <div id="txtcon-m_transaction_item_maindet">
                                                <asp:TextBox ID="txtInvNO" runat="server"  CssClass="txtbox_none_Mid_transac_item_maindet"
                                                    ClientIDMode="Static" TabIndex="48" onkeypress="return numcharspl1(event)"  Width="100px"></asp:TextBox>
                                            </div>
                                            <div id="magnify_area">
                                            </div>
											 <div id="tag_label_transaction_popup_no_empty">
                        </div>
                                            <div id="tag_label_transaction_popup_gen" style="width: 100px;">
                                                Invoice Date</div>
                                            <div id="txtcon-m_transaction_code">
                                                <asp:TextBox ID="txtInvoiceDate" runat="server" MaxLength="10" ClientIDMode="Static"
                                                    CssClass="txtbox_none_Mid_transac_code" TabIndex="49" onkeypress="return numcharspl1(event)"></asp:TextBox>
                                                <cc1:MaskedEditExtender ID="MaskedEditExtender1" runat="server" TargetControlID="txtInvoiceDate"
                                                    Mask="99/99/9999" MessageValidatorTip="true" OnInvalidCssClass="MaskedEditError"
                                                    MaskType="Date" DisplayMoney="Left" AcceptNegative="Left" ErrorTooltipEnabled="True">
                                                </cc1:MaskedEditExtender>
                                                <cc1:CalendarExtender ID="CalendarExtender2" runat="server" TargetControlID="txtInvoiceDate"
                                                    PopupButtonID="txtInvoiceDate" Format="dd/MM/yyyy">
                                                </cc1:CalendarExtender>
                                            </div>
                                        </div>
										 <div id="tag_transac_lft_in1" style="width: 630px">
                                            <div id="tag_label_transaction_popup_gen">
                                                Invoice Value</div>
                                            <div id="txtcon-m_transaction_item_maindet">
                                               <asp:TextBox ID="txtInvoiceValue" runat="server" CssClass="validate[custom[number]]  txtbox_none_Mid_transac_item_maindet"
                                    ClientIDMode="Static" TabIndex="50" onblur="extractNumber(this,3,false);" onkeyup="extractNumber(this,3,false);return Clear_Dot(this);"
                                            onkeypress="return blockNonNumbers(this, event, true, false);" Width="100px"></asp:TextBox>
                                            </div>
                                            <div id="magnify_area">
                                            </div>
											 <div id="tag_label_transaction_popup_no_empty">
                        </div>
                                            <div id="tag_label_transaction_popup_gen" style="width: 100px;">
                                                 Currency</div>
                                            <div id="txtcon-m_transaction_code">
                                              <asp:TextBox ID="txtInvoiceCurrency" runat="server" CssClass="validate[minSize[3],custom[onlyLetterSp]] txtbox_none_Mid_transac_code"
                                    ClientIDMode="Static" TabIndex="51" onkeypress="return char1(event)" onblur="return ChangeCase(this);"
                                    onkeyup="return ChangeCase(this);autotab(this);" MaxLength="3"></asp:TextBox>
                            </div>
                            <div id="magnify_area">
                                <a href="#" onclick="jQuery('#form1').validationEngine('hideAll');OpenPopupExchange('InvoiceCurrency'); return false"
                                    class="magnfy"></a>
                            </div>
                                        </div>
                                        <div id="tag_transac_lft_in1" style="width: 630px">
                                            <div id="tag_label_transaction_popup_gen">
                                                BE No</div>
                                            <div id="txtcon-m_transaction_item_maindet">
                                                <asp:TextBox ID="txtBEno" runat="server" MaxLength="50" CssClass="txtbox_none_Mid_transac_item_maindet"
                                                    ClientIDMode="Static" TabIndex="52" onkeypress="return pin(event)"  Width="100px"></asp:TextBox>
                                            </div>
                                            <div id="magnify_area">
                                            </div>
											 <div id="tag_label_transaction_popup_no_empty">
                        </div>
                                            <div id="tag_label_transaction_popup_gen" style="width: 100px;">
                                                BE Date</div>
                                            <div id="txtcon-m_transaction_code">
                                                <asp:TextBox ID="txtBEDate" runat="server" MaxLength="50" ClientIDMode="Static" CssClass="txtbox_none_Mid_transac_code"
                                                    TabIndex="53" onkeypress="return numcharspl1(event)"></asp:TextBox>
                                                <cc1:MaskedEditExtender ID="MaskedEditExtender2" runat="server" TargetControlID="txtBEDate"
                                                    Mask="99/99/9999" MessageValidatorTip="true" OnInvalidCssClass="MaskedEditError"
                                                    MaskType="Date" DisplayMoney="Left" AcceptNegative="Left" ErrorTooltipEnabled="True">
                                                </cc1:MaskedEditExtender>
                                                <cc1:CalendarExtender ID="CalendarExtender3" runat="server" TargetControlID="txtBEDate"
                                                    PopupButtonID="txtBEDate" Format="dd/MM/yyyy">
                                                </cc1:CalendarExtender>
                                            </div>
                                        </div>
										
										<div id="tag_transac_lft_in1" style="width: 630px">
                                            <div id="tag_label_transaction_popup_gen">
                                                Assessable Value</div>
                                            <div id="txtcon-m_transaction_item_maindet">
                                                <asp:TextBox ID="txtAssval" runat="server" MaxLength="50" CssClass="txtbox_none_Mid_transac_item_maindet"
                                                    ClientIDMode="Static" TabIndex="54" onkeypress="return numcharspl1(event)"  Width="100px"></asp:TextBox>
                                            </div>
                                            <div id="magnify_area">
                                            </div>
											 <div id="tag_label_transaction_popup_no_empty">
                        </div>
                                            <div id="tag_label_transaction_popup_gen" style="width: 100px;">
                                                Duty</div>
                                            <div id="txtcon-m_transaction_code">
                                                <asp:TextBox ID="txtDuty" runat="server" MaxLength="50" ClientIDMode="Static" CssClass="txtbox_none_Mid_transac_code"
                                                    TabIndex="55" onkeypress="return numcharspl1(event)"></asp:TextBox>
                                                
                                            </div>
                                        </div>
										 <div id="tag_transac_lft_in1" style="width: 630px;display:none">
                                            <div id="tag_label_transaction_popup_gen">
                                                BL Type</div>
                                            <div id="txtcon-m_transaction_pop_gen1_srcic_new">
                                                <asp:DropDownList ID="ddlBLType" runat="server" Width="290px" CssClass="listtxt_transac_mode_new"
                                                    TabIndex="56">
                                                </asp:DropDownList>
                                            </div>
                                            <div id="magnify_area">
                                            </div>
                                        </div>
                                        <div id="tag_transac_lft_in1" style="width: 630px">
                                            <div id="tag_label_transaction_popup_gen">
                                                Transporter</div>
                                            <div id="txtcon-m_transaction_pop_gen1_srcic_new">
                                                <asp:TextBox ID="txtTransporter" runat="server" MaxLength="50" CssClass="txtbox_none_Mid_transac_pop_gen_srcic_new"
                                                    onkeypress="return numcharspl1(event);" ClientIDMode="Static" TabIndex="57"></asp:TextBox>
                                            </div>
                                            <div id="magnify_area">
                                            </div>
                                        </div>
                                          <div id="tag_transac_lft_in1" style="width: 630px">
                                            <div id="tag_label_transaction_popup_gen" >
                                                Vehical Name </div>
                                            <div id="txtcon-m_transaction_item_maindet">
                                                <asp:TextBox ID="txtVehicle" runat="server" MaxLength="50" CssClass="txtbox_none_Mid_transac_code"
                                                    ClientIDMode="Static" TabIndex="58" onkeypress="return numcharspl1(event)"></asp:TextBox>
                                            </div>
                                            <div id="magnify_area">
                                            </div>
                                            <div id="tag_label_transaction_popup_no_empty">
                        </div>
                                            <div id="tag_label_transaction_Src" style="width: 100px;">
                                                Truck No</div>
                                            <div id="txtcon-m_transaction_code">
                                                <asp:TextBox ID="txtTruckno" runat="server" MaxLength="50" ClientIDMode="Static"
                                                    CssClass="txtbox_none_Mid_transac_code" TabIndex="59" onkeypress="return numcharspl1(event)"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div id="tag_transac_lft_in1" style="width: 630px">
                                            <div id="tag_label_transaction_popup_gen">
                                                PickUp Name </div>
                                            <div id="txtcon-m_transaction_item_maindet">
                                                <asp:TextBox ID="txtPickup" runat="server" MaxLength="50" CssClass="txtbox_none_Mid_transac_code"
                                                    ClientIDMode="Static" TabIndex="60" onkeypress="return numcharspl1(event)"></asp:TextBox>
                                            </div>
                                            <div id="magnify_area">
                                            </div>
                                            <div id="tag_label_transaction_popup_no_empty">
                        </div>
                                            <div id="tag_label_transaction_Src" style="width: 100px;">
                                                Date </div>
                                            <div id="txtcon-m_transaction_code">
                                                  <asp:TextBox ID="txtPickupDate" runat="server" MaxLength="50" CssClass="txtbox_none_Mid_transac_code"
                                                    ClientIDMode="Static" TabIndex="60" onkeypress="return numcharspl1(event)"></asp:TextBox>
                                        <cc1:MaskedEditExtender ID="MaskedEditExtender3" runat="server" TargetControlID="txtPickupDate"
                                                    Mask="99/99/9999" MessageValidatorTip="true" OnInvalidCssClass="MaskedEditError"
                                                    MaskType="Date" DisplayMoney="Left" AcceptNegative="Left" ErrorTooltipEnabled="True">
                                                </cc1:MaskedEditExtender>
                                                <cc1:CalendarExtender ID="CalendarExtender4" runat="server" TargetControlID="txtPickupDate"
                                                    PopupButtonID="txtPickupDate" Format="dd/MM/yyyy">
                                                </cc1:CalendarExtender>
                                            </div>
                                        </div>
                                           <div id="tag_transac_lft_in1" style="width: 630px">
                                            <div id="tag_label_transaction_popup_gen">
                                                Drop Name </div>
                                            <div id="txtcon-m_transaction_item_maindet">
                                               <asp:TextBox ID="txtDrop" runat="server" MaxLength="50" ClientIDMode="Static" CssClass="txtbox_none_Mid_transac_code"
                                                    TabIndex="61" onkeypress="return numcharspl1(event)"></asp:TextBox>
                                            </div>
                                            <div id="magnify_area">
                                            </div>
                                            <div id="tag_label_transaction_popup_no_empty">
                        </div>
                                            <div id="tag_label_transaction_Src" style="width: 100px;">
                                                Date </div>
                                            <div id="txtcon-m_transaction_code">
                                                <asp:TextBox ID="txtDropDate" runat="server" MaxLength="50" ClientIDMode="Static" CssClass="txtbox_none_Mid_transac_code"
                                                    TabIndex="61" onkeypress="return numcharspl1(event)"></asp:TextBox>
                                                      <cc1:MaskedEditExtender ID="MaskedEditExtender4" runat="server" TargetControlID="txtDropDate"
                                                    Mask="99/99/9999" MessageValidatorTip="true" OnInvalidCssClass="MaskedEditError"
                                                    MaskType="Date" DisplayMoney="Left" AcceptNegative="Left" ErrorTooltipEnabled="True">
                                                </cc1:MaskedEditExtender>
                                                <cc1:CalendarExtender ID="CalendarExtender5" runat="server" TargetControlID="txtDropDate"
                                                    PopupButtonID="txtDropDate" Format="dd/MM/yyyy">
                                                </cc1:CalendarExtender>
                                            </div>
                                        </div>
                                        <div id="tag_transac_lft_in1" style="width: 630px">
                                            <div id="tag_label_transaction_popup_gen">
                                                Halting</div>
                                            <div id="txtcon-m_transaction_pop_gen1_srcic_new">
                                                <asp:TextBox ID="txtHalting" runat="server" MaxLength="50" CssClass="txtbox_none_Mid_transac_pop_gen_srcic_new"
                                                    ClientIDMode="Static" TabIndex="62" onkeypress="return numcharspl1(event)"></asp:TextBox>
                                            </div>
                                            <div id="magnify_area">
                                            </div>
                                            <div id="tag_label_transaction_Src" style="text-align: center">
                                            </div>
                                            <div id="txtcon-m_transaction_code">
                                            </div>
                                        </div>
										  <div id="tag_transac_lft_in1" style="width: 630px">
                                            <div id="tag_label_transaction_popup_gen" style="display:none;">
                                                Clearance CheckList</div>
                                            <div id="txtcon-m_transaction_pop_gen1_srcic_new" style="display:none;">
                                           <asp:CheckBox ID="chkboxClearanceCheckList" runat="server" Checked="false" />
                                            </div>
                                            <div id="magnify_area">
                                            </div>
                                            <div id="tag_label_transaction_Src" style="text-align: center">
                                            </div>
                                            <div id="txtcon-m_transaction_code">
                                            </div>
                                        </div>
										
										<%--<div id="Operation_SEA2" runat="server" style="display: none;">
                                        <div id="tag_transac_lft_in1" style="width: 630px">
                                            <div id="tag_label_transaction_popup_gen">
                                                Operation HandleBy</div>
                                            <div id="txtcon-m_transaction_pop_gen1_srcic_new">
                                                <asp:TextBox ID="txtOperHandledBy" runat="server" MaxLength="50" CssClass="txtbox_none_Mid_transac_pop_gen_srcic_new"
                                                    onkeypress="return numcharspl1(event);" ClientIDMode="Static" TabIndex="63"></asp:TextBox>
                                            </div>
                                            <div id="magnify_area">
                                            </div>
                                        </div>--%>
                                   </div>
					   </div>
					 </div>
					 </div>
				   
                </div>
               
                    <div id="area_transac_container_genral" >
                            <div id="innerbox_MidMain_bot_transact" runat="server" style="width: 900px;">
                                <div id="innerbox_transac_bot_inn" style="width: 890px;">
                                    
                                    <div id="editbu">
                                       
                                        <asp:Button ID="btngenerate" runat="server" OnClick="btngenerate_Click" CausesValidation="false" UseSubmitBehavior="false"
                                            Visible="true" CssClass="autowidmovebut" Text="Generate Invoice" 
                                             
                                            Width="110px" />
                                    </div>                                          
                                </div>
                            </div>
                       
                
               
                </div>
               </div>
                
                
                <div class="content" id="page-2" style="display: none;">
                    <iframe src="" name="maineroyal" scrolling="no" id="Trans_igm_Iframe"></iframe>
                </div>
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
                <asp:HiddenField ID="hdn_New_Update" runat="server" />
                <asp:HiddenField ID="hdn_currentPage" runat="server" />
                <asp:HiddenField ID="hdn_source" runat="server" />
                <asp:HiddenField ID="hdn_typ" runat="server" />
                <asp:HiddenField ID="hdn_StateName_chk" runat="server" ClientIDMode="Static" />
                <asp:HiddenField ID="hdn_Customhouse_chk" runat="server" ClientIDMode="Static" />
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
    <div style="display: none;">
        <asp:HiddenField ID="hdnFreightMode" runat="server" />
        <asp:HiddenField ID="hdnbrancharea" runat="server" ClientIDMode="Static" />
        <asp:HiddenField ID="hdnarea" runat="server" ClientIDMode="Static" />
        <asp:HiddenField ID="hdn_Regn_no" runat="server" />
        <asp:HiddenField ID="hdn_CompanyID" runat="server" ClientIDMode="Static" />
       
    </div>
    </form>
    <script src="../js/Import_Jscript/Import_General.js" type="text/javascript"></script>
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
            document.getElementById("General").click();
        }
    </script>
    <script language="javascript">

        function calculateTotal(thisCheckbox) {

            var TextBox1 = document.getElementById("txt_Branch").value;
            var TextBox2 = document.getElementById("txt_jobno").value;
            var TextBox3 = document.getElementById("txtyear").value;

            if (document.getElementById("Chbranch").checked == true && document.getElementById("chyear").checked == true) {

                if (TextBox1.trim() != '' && TextBox2.trim() != '' && TextBox3.trim() != '') {

                    var TextBox1 = document.getElementById("txt_Branch").value + '/';
                    var TextBox2 = document.getElementById("txt_jobno").value + '/';
                    var TextBox3 = document.getElementById("txtyear").value;

                    var res = TextBox1.concat(TextBox2, TextBox3);
                    document.getElementById("txtpreview").value = res;
                }
                else if (TextBox1.trim() != '' && TextBox2.trim() != '' && TextBox3.trim() == '') {

                    var TextBox1 = document.getElementById("txt_Branch").value + '/';
                    var TextBox2 = document.getElementById("txt_jobno").value;

                    var res1 = TextBox1.concat(TextBox2);
                    document.getElementById("txtpreview").value = res1;
                }
                else if (TextBox1.trim() == '' && TextBox2.trim() != '' && TextBox3.trim() != '') {

                    var TextBox2 = document.getElementById("txt_jobno").value + '/';
                    var TextBox3 = document.getElementById("txtyear").value;

                    var res2 = TextBox2.concat(TextBox3);
                    document.getElementById("txtpreview").value = res2;

                }
                else if (TextBox1.trim() != '' && TextBox2.trim() == '' && TextBox3.trim() != '') {
                    document.getElementById("txtpreview").value = '';

                }
            }
            //----------------------------------------------------------------------------------------------------------
            else if (document.getElementById("Chbranch").checked == true && document.getElementById("chyear").checked == false) {

                if (TextBox1.trim() != '' && TextBox2.trim() != '') {
                    var TextBox1 = document.getElementById("txt_Branch").value + '/';
                    var TextBox2 = document.getElementById("txt_jobno").value;

                    var res3 = TextBox1.concat(TextBox2);
                    document.getElementById("txtpreview").value = res3;

                }
                else {
                    document.getElementById("txtpreview").value = document.getElementById("txt_jobno").value;
                }

            }
            //-------------------------------------------------------------------------------------------------------------------------------------
            else if (document.getElementById("Chbranch").checked == false && document.getElementById("chyear").checked == true) {

                if (TextBox2.trim() != '') {
                    var TextBox2 = document.getElementById("txt_jobno").value + '/';
                    var TextBox3 = document.getElementById("txtyear").value;

                    var res4 = TextBox2.concat(TextBox3);
                    document.getElementById("txtpreview").value = res4;
                }
                else {

                    document.getElementById("txtpreview").value = '';
                }

            }
            else if (document.getElementById("Chbranch").checked == false && document.getElementById("chyear").checked == false) {

                document.getElementById("txtpreview").value = document.getElementById("txt_jobno").value;
            }
        }
    </script>
    <script type="text/javascript">
        $(document).ready(function () {

            $('#txtImporterName').focus(function () {

                var data = $(this).val();
                Item_desc(data);

            });
            $('#txtImporterName').change(function () {

                var data = $(this).val();
                Item_desc(data);

            });
            $('#txtImporterName').keyup(function () {

                var data = $(this).val();
                Item_desc(data);

            });
            $('#txtImporterName').keydown(function () {

                var data = $(this).val();
                Item_desc(data);

            });

        });

        function Item_desc(val) {

            var data = val;
            if (data == '') {
                $('#txtImporterName').val('');
            }
            if (data.indexOf("--") != -1) {

                var s = data.split('--');

                var before = s[0];
                var after = s[1];

                $('#txtImporterName').val(after);

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
    <script src="../js/Import_Jscript/BE_Status.js" type="text/javascript"></script>
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
        $(document).ready(function () {
           
            if ($("#ddlTransportMode option:selected").text() == 'Air') {

                $("#ddlContainer_Type option[value='LCL']").remove();
                $("#ddlContainer_Type option[value='FCL']").remove();
                $("#ddlContainer_Type option[value='Bulk']").remove();

                $('#ddlContainer_Type').attr('disabled', 'disabled');
                //$('#dropdown').removeAttr('disabled');
            }
            else {
                $("#ddlContainer_Type option[value='']").remove();
            }

            $('#ddlTransportMode').change(function () {

                if ($("#ddlTransportMode option:selected").text() == 'Air') {

                    $("#ddlContainer_Type option[value='LCL']").remove();
                    $("#ddlContainer_Type option[value='FCL']").remove();
                    $("#ddlContainer_Type option[value='Bulk']").remove();

                    $('#ddlContainer_Type').attr('disabled', 'disabled');
                }
                else {
                    $("#ddlContainer_Type option[value='']").remove();

                    $("#ddlContainer_Type option[value='LCL']").remove();
                    $("#ddlContainer_Type option[value='FCL']").remove();
                    $("#ddlContainer_Type option[value='Bulk']").remove();

                    $("#ddlContainer_Type").append('<option value="LCL">LCL</option>');
                    $("#ddlContainer_Type").append('<option value="LCL">FCL</option>');
                    $("#ddlContainer_Type").append('<option value="LCL">Bulk</option>');

                    $('#ddlContainer_Type').removeAttr('disabled');
                }

                
        });
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
    </script>
</body>
</html>
