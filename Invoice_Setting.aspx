<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Invoice_Setting.aspx.cs" Inherits="Accounts_Invoice_Setting" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Consignee Master | Updation</title>
    <meta http-equiv="Content-Type" content="text/html; charset=iso-8859-1" />
    <link href="../main.css" rel="stylesheet" type="text/css" media="screen, projection" />
    <link href="../AutoComplete_CSS/jquery-ui.css.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        /* Autocomplete Textbox Search Loading Image*/
        .ui-autocomplete-loading
        {
            background: white url('../images/ui-anim_basic_16x16.gif') right center no-repeat;
        }
        /* Autocomplete Textbox Search Loading Image*/
    </style>
    <link rel="stylesheet" href="../Validation Files/validationEngine.css" type="text/css" />
    <link rel="stylesheet" href="../Validation Files/template.css" type="text/css" />
    <link href="../MessageBox_js/jquery.alerts.css" rel="stylesheet" type="text/css" />
    <script src="../MessageBox_js/jquery.js" type="text/javascript"></script>
    <script src="../MessageBox_js/jquery.alerts.js" type="text/javascript"></script>
     <script src="../js/CrossCountry_Jscript/Sea_General.js" type="text/javascript"></script>
</head>
<body style="background: #f2f2f2; overflow: hidden;">
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePartialRendering="false"
        EnablePageMethods="true" LoadScriptsBeforeUI="false">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional" Visible="true">
        <ContentTemplate>
            <div class="loading" align="center" id="load" style="display: none;">
                <img src="../Loading_Images/indicator_mozilla_blu.gif" alt="" />
            </div>
            <div id="innerbox_MidMain_popup_supplier" style="height:420px">
                <div id="innerbox_MidMain_top_adcode">
                    Invoice Setting
                    <div id="popupwindow_closebut_right">
                        <input type="close" value="Submit" class="clsicon" onclick="parent.adcodewindow.hide();RefreshParent();return false;" />
                    </div>
                </div>
                <div id="innerbox_MidMain_Mid_Consignee" style="height:360px">
                    <div id="pop_text_area">
                        <div id="tag_label_adcode"  style="width: 100px">
                            Company Name</div>
                        <div id="txtcon-l_src">
                        </div>
                        <div id="txtcon-m_src">
                            <asp:TextBox ID="txtCompanyName" runat="server"  Onkeypress="return numcharspl1(event)"
                                CssClass="validate[required] txtbox_none" TabIndex="1" ></asp:TextBox>
                        </div>
                        
                    </div>
                    <div id="pop_text_area">
                        <div id="tag_label_adcode" style="width: 100px">
                            Gstn_id
                        </div>
                        <div id="txtcon-l_src">
                        </div>
                        <div id="txtcon-m_src">
                            <asp:TextBox ID="txtGstnid" runat="server" MaxLength="15" CssClass="txtbox_none" TabIndex="2"></asp:TextBox>
                        </div>
                        
                    </div>
                    <div id="pop_text_area" style="height: 40px">
                        <div id="tag_label_adcode" style="width: 100px">
                            Address</div>
                        
                        <div id="txtcon-m_src" style="Height:50px; width: 260px;">
                            <asp:TextBox ID="txtAddress1" runat="server" CssClass="txtbox_none_Mid_transac_item_other_rmarks_imp"
                                MaxLength="300" TabIndex="3" Height="40px" TextMode="MultiLine" 
                                Width="220px"></asp:TextBox>
                        </div>
                    </div>
                    <div id="pop_text_area">
                        <div id="tag_label_adcode"  style="width: 100px">
                            Pincode
                        </div>
                        <div id="txtcon-l_src">
                        </div>
                        <div id="txtcon-m_src">
                            <asp:TextBox ID="txtPincode" runat="server" MaxLength="6" CssClass="txtbox_none" TabIndex="4"></asp:TextBox>
                        </div>
                        
                    </div>
                     <div id="pop_text_area" style="height: 20px">
                            <div id="tag_label_adcode"  style="width: 100px">
                                State Name
                            </div>
                            <div id="txtcon-m_src">
                                <asp:TextBox ID="txtCommercial_StateName" runat="server" onkeypress="return char(event)"
                                    TabIndex="5" CssClass="validate[required] txtbox_none" ClientIDMode="Static"></asp:TextBox>
                            </div>
                            
                        </div>

                        <div id="pop_text_area" style="height: 20px">
                            <div id="tag_label_adcode"  style="width: 100px">
                                State Code
                            </div>
                            <div id="txtcon-m_src" style="width: 70px;">
                                <asp:TextBox ID="txtCommercial_StateCode" runat="server" MaxLength="2" onkeypress="return num1(event)"
                                    TabIndex="6" CssClass="validate[required] txtbox_none_Mid_transac_item_CTH_small" ClientIDMode="Static"></asp:TextBox>
                            </div>
                            <div id="txtcon-r_src">
                                <div runat="server" id="ST_Code">
                                    <a href="#" class="magnfy" onclick="jQuery('#form1').validationEngine('hideAll');opennewStatecode('IMPORT_STATECODE'); return false">
                                    </a>
                                </div>
                            </div>
                        </div>
                    
                    <div id="pop_text_area">
                        <div id="tag_label_adcode"  style="width: 100px">
                            Mobile No</div>
                        <div id="txtcon-l_src">
                        </div>
                        <div id="txtcon-m_src">
                            <asp:TextBox ID="txtmobileno" runat="server" MaxLength="15" CssClass="txtbox_none" TabIndex="7" onkeypress="return pin(event)"></asp:TextBox>
                        </div>
                        
                    </div>
                   

                    <div id="pop_text_area">
                        <div id="tag_label_adcode"  style="width: 100px">
                            Email ID
                        </div>
                        <div id="txtcon-l_src">
                        </div>
                        <div id="txtcon_mid_size">
                        <asp:TextBox ID="txtEmailid" runat="server" CssClass="validate[required] txtbox_none"
                                 MaxLength="35" TabIndex="8"></asp:TextBox>
                           <%-- <asp:TextBox ID="txtCountryName" runat="server" CssClass="validate[required] txtbox_none_mid_size"
                                Onkeypress="return char(event)" MaxLength="35" TabIndex="5"></asp:TextBox>--%>
                        </div>
                        
                       
                    </div>
                    
                 
                  <div id="pop_text_area" style="display: none">
                        <div id="tag_label_adcode"  style="width: 100px">
                            Email ID
                        </div>
                        <div id="txtcon-l_src">
                        </div>
                        <div id="txtcon_mid_size">
                        <asp:TextBox ID="txtInvoice" runat="server" CssClass="txtbox_none"
                                 MaxLength="35" TabIndex="8"></asp:TextBox>
                           <%-- <asp:TextBox ID="txtCountryName" runat="server" CssClass="validate[required] txtbox_none_mid_size"
                                Onkeypress="return char(event)" MaxLength="35" TabIndex="5"></asp:TextBox>--%>
                        </div>
                        
                       
                    </div>
                    
                       
                    
                </div>
                <div id="innerbox_MidMain_top_adcode">
                    <div id="innerbox_adcode_bot_inn">
                        <div id="newbu">
                            <asp:Button ID="btnNew" runat="server" CausesValidation="false" UseSubmitBehavior="false"
                                OnClick="btnNew_Click" CssClass="new" OnClientClick="jQuery('#form1').validationEngine('hide')"
                                TabIndex="13" />
                        </div>
                        <div id="editbu">
                            <%--<asp:Button ID="btnSave" runat="server" onclick="btnSave_Click" OnClientClick="return validate(event)" CssClass="save"  />--%>
                            <asp:Button ID="btnSave" runat="server" OnClick="btnSave_Click" CssClass="save" OnClientClick=" return validate_Sales()"
                                TabIndex="14" />
                        </div>
                        <div id="editbu">
                            <asp:Button ID="btnUpdate" runat="server" OnClick="btnUpdate_Click" CssClass="updates"
                                OnClientClick="return validate_Sales()" TabIndex="15" />
                        </div>
                        <div id="editbu">
                            <asp:Button ID="btnDelete" runat="server" TabIndex="16" CssClass="dlete" OnClientClick="jConfirm('Delete this Entry?', 'Invoice Setting', function(r) {
                  var i = r + 'ok';
          if(i == 'trueok')
          {
              document.getElementById('btn').click();
            
          }
          else {
          }
    
});return false;" />
                            <asp:Button ID="btn" runat="server" TabIndex="17" OnClick="btnDelete_Click" CssClass="dlete"
                                Style="display: none;" />
                        </div>
                        <div id="editbu">
                            <input type="submit" value="Submit" class="scancel" onclick="jQuery('#form1').validationEngine('hideAll');parent.adcodewindow.hide();RefreshParent();return false;"
                                tabindex="18" id="btnCancel" />
                            <asp:HiddenField ID="hdn_Update_BUYER_Name" runat="server" />
                            <asp:HiddenField ID="HiddenField1" runat="server" />
                            <asp:HiddenField ID="HiddenField2" runat="server" />
                        </div>
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    </form>
    <script src="../js/listpopup.js" type="text/javascript"></script>
    <script src="../js/Validation.js" type="text/javascript"></script>
    <script src="../js/JScript_validation_eRoyalMasters.js" type="text/javascript"></script>
    <script src="../AutoComplete_JS/jquery.min.js" type="text/javascript"></script>
    <script src="../AutoComplete_JS/jquery-ui.min.js" type="text/javascript"></script>
    <!-- VALIDATION SCRIPT -->
    <script src="../Validation Files/jquery_002.js" type="text/javascript" charset="utf-8">
    </script>
    <script src="../Validation Files/jquery.js" type="text/javascript" charset="utf-8">
    </script>
    <script src="../Validation%20Files/Ascii.js" type="text/javascript"></script>
    <!-- VALIDATION SCRIPT -->
    <script type="text/javascript">
        jQuery(document).ready(function () {

            jQuery("#form1").validationEngine();
        });


        function checkHELLO(field, rules, i, options) {
            if (field.val() != "HELLO") {

                return options.allrules.validate2fields.alertText;
            }
        }







        function CallMe(src, dest) {
            var ctrl = document.getElementById(src);
            // call server side method
            //alert('l');
            PageMethods.GetCountry_code(ctrl.value, CallSuccess, CallFailed, dest);

        }
        // set the destination textbox value with the ContactName
        function CallSuccess(res, destCtrl) {
            var dest = document.getElementById(destCtrl);
            dest.value = res;
        }
        // alert message on some failure
        function CallFailed(res, destCtrl) {
            //alert(res.get_message());
        }


        function validate() {
            var emailPat = /^(([^<>()\[\]\.,;:\s@\"]+(\.[^<>()\[\]\.,;:\s@\"]+)*)|(\".+\"))@(([^<>()[\]\.,;:\s@\"]+\.)+[^<>()[\]\.,;:\s@\"]{2,})$/i;
            var emailid = document.getElementById("txtEmailid").value;
            var matchArray = emailid.match(emailPat);
            if (document.getElementById("txtEmailid").value != "") {
                if (matchArray == null) {

                    jAlert('Invalid Email Id', 'Invoice setting', function (r) { document.getElementById("txtEmailid").focus(); });
                    return false;
                }
            }
        }
       
    </script>
</body>
</html>
