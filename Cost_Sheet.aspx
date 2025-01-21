<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Cost_Sheet.aspx.cs" Inherits="Accounts_Cost_Sheet" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
     <meta http-equiv="Content-Type" content="text/html; charset=iso-8859-1" />
    <title>Main Master</title>
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
    <script src="../MessageBox_js/jquery.js" type="text/javascript"></script>
    <link href="../MessageBox_js/jquery.alerts.css" rel="stylesheet" type="text/css" />
    <script src="../MessageBox_js/jquery.alerts.js" type="text/javascript"></script>
     <style type="text/css">
        .hideGridColumn
        {
            display: none;
        }
    </style>

 

     <script type="text/javascript" language="javascript">

         function GetRowValue() {

             window.close();

         }
         function number(evt) {
             var charCode = (evt.which) ? evt.which : event.keyCode
             if (charCode > 32 && (charCode < 48 || charCode > 57))
                 return false;
             return true;

         }
        


    </script>
   
 
 <script type ="text/javascript">
     function MaskMoney(evt) {
         if (!(evt.keyCode == 46 || (evt.keyCode >= 48 && evt.keyCode <= 57))) return false;
         var parts = evt.srcElement.value.split('.');
         if (parts.length > 2) return false;
         if (evt.keyCode == 46) return (parts.length == 1);
         if (parts[0].length >= 14) return false;
         if (parts.length == 2 && parts[1].length >= 2) return false;
     }

     String.prototype.startsWith = function (str) {
         return (this.indexOf(str) === 0);
     }
     function extractno(obj, decimalPlaces, allowNegative) {


         var temp = obj.value;


         if (temp.startsWith(".") || temp.startsWith("0") || temp.startsWith("0.")) // true
         {
             temp.value = "";
             alert("you can not insert dot and zero as first character");
         }
     }




     function extractNumber(obj, decimalPlaces, allowNegative) {
         var temp = obj.value;




         if (temp.startsWith(".") || temp.startsWith("0") || temp.startsWith("0.")) // true
         {
             temp.value = "";
             alert("you can not insert dot and zero as first character");
         }
         // avoid changing things if already formatted correctly
         var reg0Str = '[0-9]*';
         if (decimalPlaces > 0) {
             reg0Str += '\\.?[0-9]{0,' + decimalPlaces + '}';
         } else if (decimalPlaces < 0) {
             reg0Str += '\\.?[0-9]*';
         }
         reg0Str = allowNegative ? '^-?' + reg0Str : '^' + reg0Str;
         reg0Str = reg0Str + '$';
         var reg0 = new RegExp(reg0Str);
         if (reg0.test(temp)) return true;

         // first replace all non numbers
         var reg1Str = '[^0-9' + (decimalPlaces != 0 ? '.' : '') + (allowNegative ? '-' : '') + ']';
         var reg1 = new RegExp(reg1Str, 'g');
         temp = temp.replace(reg1, '');

         if (allowNegative) {
             // replace extra negative
             var hasNegative = temp.length > 0 && temp.charAt(0) == '-';
             var reg2 = /-/g;
             temp = temp.replace(reg2, '');
             if (hasNegative) temp = '-' + temp;
         }

         if (decimalPlaces != 0) {
             var reg3 = /\./g;
             var reg3Array = reg3.exec(temp);
             if (reg3Array != null) {
                 // keep only first occurrence of .
                 //  and the number of places specified by decimalPlaces or the entire string if decimalPlaces < 0
                 var reg3Right = temp.substring(reg3Array.index + reg3Array[0].length);
                 reg3Right = reg3Right.replace(reg3, '');
                 reg3Right = decimalPlaces > 0 ? reg3Right.substring(0, decimalPlaces) : reg3Right;
                 temp = temp.substring(0, reg3Array.index) + '.' + reg3Right;
             }
         }

         obj.value = temp;
     }
     function blockNonNumbers(obj, e, allowDecimal, allowNegative) {
         var key;
         var isCtrl = false;
         var keychar;
         var reg;

         if (window.event) {
             key = e.keyCode;
             isCtrl = window.event.ctrlKey
         }
         else if (e.which) {
             key = e.which;
             isCtrl = e.ctrlKey;
         }

         if (isNaN(key)) return true;

         keychar = String.fromCharCode(key);

         // check for backspace or delete, or if Ctrl was pressed
         if (key == 8 || isCtrl) {
             return true;
         }

         reg = /\d/;
         var isFirstN = allowNegative ? keychar == '-' && obj.value.indexOf('-') == -1 : false;
         var isFirstD = allowDecimal ? keychar == '.' && obj.value.indexOf('.') == -1 : false;

         return isFirstN || isFirstD || reg.test(keychar);
     }
    </script>
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
             
            <div id="innerbox_MidMain_bot_transact" style="height: 360px; width:1104px;">
            <div id="innerbox_MidMain_Trans_IGM_Invoice" style="background-image:none;height:305px;width:1010px">
                
                <div class="content" id="page-1" style="height:405px;">
                <asp:Panel ID="Panel1" runat="server" Height="280px">

                    <div id="innerbox_MidMain_Transact_popup_new" 
                        style="width:990px; height: 270px; margin-left: -12px; margin-top: -12px;">
                        
                        
                        
                        
                        <div id="tag_srcinner1_new">
                    <div id="editbu">
                        <a href="#" class="edit"></a>
                    </div>
                    <div id="mainmastop2container_rght_tag2_txt1" style="width: 900px">
                        COST SHEET
                    </div>
                    <div id="popupwindow_closebut_right_new">
                        <input type="close" value="Submit" class="clsicon_new" onclick="win_hide();parent.adcodewindow.hide();RefreshParent();return false;" />
                    </div>
                   
                </div>
                       

                        <%--<div id="mainmastop2container_rght_tag2_txt1" 
                            
                               style="padding: 0px; width: 900px; height: 30px; margin-left: 0px; margin-top: -10px;">
                                COST SHEET
                            </div>--%>
                                                    
                      
            
                        <div id="pop_text_area_transac_popup_inn_container_IGM_JobSeries2_grid" 
                            style="width:1053px; height:200px; margin-top: 70px;">
              <div id="tag_transac_lft_Item_maindet_Grid_area" style="width:990px; overflow:auto; height:240px;margin-top:-38px">

                   <asp:GridView ID="gv_CONT" runat="server" DataKeyNames="Imp_Cost_Sell_ID"  AllowSorting="True" 
                                 EmptyDataText="No Record Found" 
                                    AutoGenerateColumns="False" BackColor="White" BorderColor="#C8C8C8" 
                                    BorderStyle="Solid" 
                                    BorderWidth="1px" CellPadding="1" CellSpacing="1" 
                       CssClass="grid-view" AllowPaging="false"
                                     ShowHeaderWhenEmpty="True" Width="100%" 
                                  
                        PageSize="7"  OnRowDataBound="gv_CONT_RowDataBound" 
                        ShowHeader = "true" 
                        OnRowCreated="gv_CONT_RowCreated">
                        <AlternatingRowStyle BackColor="WhiteSmoke" BorderColor="#C8C8C8" BorderStyle="Solid"
                            BorderWidth="1px" />
                        <Columns>
                   

                            <asp:CommandField ShowSelectButton="True">
                                <HeaderStyle CssClass="hideGridColumn" />
                                <ItemStyle CssClass="hideGridColumn" />
                            </asp:CommandField>

                              <asp:TemplateField HeaderText="S.No">
                                            <ItemTemplate>
                                                <%# Container.DataItemIndex + 1 %>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                         
                            <asp:BoundField DataField="CHARGE_NAME" HeaderText="Charge Name">
                                <ItemStyle CssClass="column_style_left"  />
                            </asp:BoundField>
                          
                            <%--<asp:BoundField DataField="DESCRIPTION" HeaderText="DESCRIPTION">
                                <ItemStyle CssClass="column_style_left" />
                            </asp:BoundField>--%>
                             <asp:BoundField DataField="QTY" HeaderText="No of QTY" >
                                <ItemStyle CssClass="column_style_left" />
                            </asp:BoundField>
                              <asp:BoundField DataField="UNIT" HeaderText="Unit">
                                <ItemStyle CssClass="column_style_left" />
                            </asp:BoundField>

                         
                           <asp:BoundField DataField="Currency_Code" HeaderText="Currency">
                                <ItemStyle CssClass="column_style_left" />
                            </asp:BoundField>
                            <asp:BoundField DataField="ExchangRate" HeaderText="Ex Rate">
                                   <ItemStyle CssClass="column_style_left" />
                            </asp:BoundField>
                            <asp:BoundField DataField="COST_RATE" HeaderText="Cost Rate">
                                       <ItemStyle CssClass="column_style_left" />
                            </asp:BoundField>
                             
                          
                            <asp:BoundField DataField="Cost_Total_Amount" HeaderText="Cost Tot Amt">
                                <ItemStyle CssClass="column_style_left" />
                            </asp:BoundField>
                            
                             <asp:BoundField DataField="SELL_RATE" HeaderText="Sell Rate">
                                  <HeaderStyle CssClass="hideGridColumn" />
                                <ItemStyle CssClass="hideGridColumn" />
                            </asp:BoundField>
                             
                               <asp:BoundField DataField="Sell_Total_Amount" HeaderText="Sell Tot Amt">
                                 <HeaderStyle CssClass="hideGridColumn" />
                                <ItemStyle CssClass="hideGridColumn" />
                            </asp:BoundField>
                         <asp:BoundField DataField="Imp_Cost_Sell_ID" HeaderText="Cost Tot Amt">
                                 <HeaderStyle CssClass="hideGridColumn" />
                                <ItemStyle CssClass="hideGridColumn" />
                            </asp:BoundField>
                            <asp:BoundField DataField="AGREED_RATE" HeaderText="Agreed Rate" >
                                       <HeaderStyle CssClass="hideGridColumn" />
                                <ItemStyle CssClass="hideGridColumn" />
                            </asp:BoundField>

                            <asp:BoundField DataField="AGREED_Total_Amount" HeaderText="Agg Total Amt">
                                <HeaderStyle CssClass="hideGridColumn" />
                                <ItemStyle CssClass="hideGridColumn" />
                            </asp:BoundField>

                               <asp:BoundField DataField="SELL_AGREED_RATE" HeaderText="Sell Agreed Rate">
                                        <HeaderStyle CssClass="hideGridColumn" />
                                <ItemStyle CssClass="hideGridColumn" />
                             
                            </asp:BoundField>

                            <asp:BoundField DataField="SELL_AGREED_Total_Amount" HeaderText="Sell Tot Ag.Amt">
                                 <HeaderStyle CssClass="hideGridColumn" />
                                <ItemStyle CssClass="hideGridColumn" />
                               
                            </asp:BoundField>


                            <asp:BoundField DataField="TOTAL_COST_INR" HeaderText="Tot Cost in INR">
                                 <HeaderStyle CssClass="hideGridColumn" />
                                <ItemStyle CssClass="hideGridColumn" />
                            </asp:BoundField>
                              <asp:BoundField DataField="TOTAL_COST_AGG_INR" HeaderText="Tot Ag.Amt in INR">
                               <HeaderStyle CssClass="hideGridColumn" />
                                <ItemStyle CssClass="hideGridColumn" />
                            </asp:BoundField>
                              <asp:BoundField DataField="TOTAL_SELL_INR" HeaderText="Tot Sell in INR">
                               <HeaderStyle CssClass="hideGridColumn" />
                                <ItemStyle CssClass="hideGridColumn" />
                            </asp:BoundField>
                              <asp:BoundField DataField="TOTAL_SELL_AGG_INR" HeaderText="Tot Ag.Amt in INR">
                               <HeaderStyle CssClass="hideGridColumn" />
                                <ItemStyle CssClass="hideGridColumn" />
                            </asp:BoundField>

                            
                             <asp:BoundField DataField="Sell_Currency_Code" HeaderText="Sel curr">
                               <HeaderStyle CssClass="hideGridColumn" />
                                <ItemStyle CssClass="hideGridColumn" />
                            </asp:BoundField>
                             <asp:BoundField DataField="Sell_ExchangRate" HeaderText="Sel ex">
                               <HeaderStyle CssClass="hideGridColumn" />
                                <ItemStyle CssClass="hideGridColumn" />
                            </asp:BoundField>
                            <asp:BoundField DataField="VendorName" HeaderText="Vendor Name">
                                <ItemStyle CssClass="column_style_left" />
                            </asp:BoundField>
                        </Columns>

                         <PagerSettings FirstPageText="First" LastPageText="Last" Mode="NextPreviousFirstLast"
                                    NextPageText="Next" Position="Top" PreviousPageText="Previous" />
                                     <PagerStyle CssClass="pager" />
                                    <EmptyDataTemplate>
                                        NO RECORD FOUND
                                    </EmptyDataTemplate>

                    </asp:GridView>
                </div>

                 
                   



                    </div>

                        </div>







                    </div>
                    </asp:Panel>
                </div>
               
            </div>
           

            </div>
            

          <div style="display: none;">
                    <asp:Label ID="lblMsg" runat="server" ForeColor="Red"></asp:Label>
                    <asp:HiddenField ID="hfdport_code_origin" runat="server" ClientIDMode="Static" />
                    <asp:HiddenField ID="hfdport_code_dest" runat="server" ClientIDMode="Static" />
                    <asp:HiddenField ID="HiddenField1" runat="server" ClientIDMode="Static" />
                    <asp:HiddenField ID="hdfTransport_mode" runat="server" />
                    <asp:HiddenField ID="hdn_Update_General" runat="server" />
                    <asp:HiddenField ID="hdn_Edit_TransportMode" runat="server" />
                    <asp:HiddenField ID="hdnImportAirJobLinked" runat="server" />
                    <asp:HiddenField ID="hdn_Grid_Disable" runat="server" />
                    <asp:HiddenField ID="hdn_Cost_Sell_Id" runat="server" />
                    <asp:HiddenField ID="hdn_Amount" runat="server" />
                    <asp:HiddenField ID="hdn_update_Bond" runat="server" />
                    <asp:HiddenField ID="hdn_Jobno" runat="server" />
                </div>
          
        </ContentTemplate>
    </asp:UpdatePanel>
    </form>
     <script src="../js/Import_Jscript/Can_Cargo_Air_Script/Air_HAWB.js" type="text/javascript"></script>
    <script src="../js/Import_Jscript/import_General.js" type="text/javascript"></script>
   
    <script type="text/javascript" src="../js/Import_Jscript/Master_Air_Script/Air_HAWB.js"></script>
    <script type="text/javascript" src="../windowfiles/dhtmlwindow.js"></script>
    <script type="text/javascript" src="../modalfiles/modal.js"></script>
    <script type="text/javascript" src="../js/slide.js"></script>
    <script type="text/javascript" src="../js/jscolor.js"></script>
    <script type="text/javascript" src="../js/iframepopupwin.js"></script>
    <script src="../js/checkboxJScript.js" type="text/javascript"></script>
    <script src="../js/listpopup.js" type="text/javascript"></script>
    <script src="../js/Validation.js" type="text/javascript"></script>

    <style type="text/css">
        .ui-autocomplete-loading
        {
            background: white url('../images/ui-anim_basic_16x16.gif') right center no-repeat;
        }
        body
        {
            font-size: 62.5%;
        }
    </style>

    <script src="../AutoComplete_JS/jquery.min.js" type="text/javascript"></script>
    <script src="../AutoComplete_JS/jquery-ui.min.js" type="text/javascript"></script>
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



        $(function () {
            $('#txtExchange_Cur_Code,#txtsellExchange_Cur_Code').autocomplete({
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
                                if (field == 'txtExchange_Cur_Code') {
                                    jQuery('#' + field).validationEngine('showPrompt', 'Incorrect Currency', 'error', 'topRight', true);

                                }

                            }
                            else {

                                jQuery('#' + field).validationEngine('hidePrompt', '', 'error', 'topRight', true);

                                var result = data.d;

                                var stringsearch = "-", str = result;

                                for (var i = count = 0; i < str.length; count += +(stringsearch === str[i++]));

                                if (i == 1) {
                                    if (field == 'txtExchange_Cur_Code') {
                                        $('#txtExchange_Cur_Code').val(result);
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

        $(function () {
            $("#txtUnit").autocomplete({
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
                                if (field == 'txtUnit') {
                                    jQuery('#' + field).validationEngine('showPrompt', 'Incorrect Unit', 'error', 'topRight', true);
                                    //jAlert('Gross Weight Type is Incorrect', 'IGM', function (r) { document.getElementById(field).focus(); });
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

        $(function () {
            $("#TxtVendorName").autocomplete({
                source: function (request, response) {
                    var field = $(this.element).attr('id');
                    $.ajax({
                        url: "../AutoComplete_Pages/Auto_Complete_Searching.asmx/Vendor_List",
                        data: "{ 'mail': '" + request.term + "' }",
                        dataType: "json",
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        dataFilter: function (data) { return data; },
                        async: true,
                        success: function (data) {
                            response(data.d);
                            if (data.d == '') {
                                if (field == 'TxtVendorName') {
                                    jQuery('#' + field).validationEngine('showPrompt', 'Incorrect Unit', 'error', 'topRight', true);
                                    //jAlert('Gross Weight Type is Incorrect', 'IGM', function (r) { document.getElementById(field).focus(); });
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

        function checkHELLO(field, rules, i, options) {
            if (field.val() != "HELLO") {

                return options.allrules.validate2fields.alertText;
            }
        }
        function close_button() {
            //document.getElementById("btn_cancel").click();
            document.getElementById("General").click();
        }
        function Calculationex() {

            var currency = document.getElementById("txtExchange_Cur_Code").value;


            var a = currency;
            var b = '1';


            Cha_exrate(a, b);

        }

        function Cha_exrate(src, dest) {



            PageMethods.Get_Cha_exrate(src, 'I', CallSuccess_Cha_exrate, CallFailedexrate, dest);



        }
        function CallSuccess_Cha_exrate(res, destCtrl) {

            var data = res;

            if (res != '') {

                document.getElementById("txtExRate").value = res;




            }

        }

        function CallFailedexrate(res, destCtrl) {
        }



        function Calculationselex() {

            var currency = document.getElementById("txtsellExchange_Cur_Code").value;


            var a = currency;
            var b = '1';


            Cha_selexrate(a, b);

        }

        function Cha_selexrate(src, dest) {



            PageMethods.Get_Cha_exrate(src, 'I', CallSuccess_Cha_selexrate, CallFailedexrate, dest);



        }
        function CallSuccess_Cha_selexrate(res, destCtrl) {

            var data = res;

            if (res != '') {

                document.getElementById("txtsellExRate").value = res;

            }

        }

        function CallFailedexrate(res, destCtrl) {
        }


        function Calc() {

            var ExRate = document.getElementById("txtExRate").value;
            var SelExRate = document.getElementById("txtsellExRate").value;
            var costRate = document.getElementById("TxtCostRate").value;
            var SellRate = document.getElementById("txtSellrate").value;
            var AggRate = document.getElementById("txtaggreedrate").value;
            var SellAggRate = document.getElementById("txtsellaggreedrate").value;
            var Qnt = document.getElementById("txtQnt").value;
            if (costRate != "" && SellRate != "") {
                document.getElementById("txtTot_Cost_Amt").value = parseFloat(costRate * Qnt).toFixed(2);
                document.getElementById("txtaggreedtot").value = parseFloat(AggRate * Qnt).toFixed(2);
                document.getElementById("txtTot_Sell_Amt").value = parseFloat(SellRate * Qnt).toFixed(2);
                document.getElementById("txtaggreedSelltot").value = parseFloat(SellAggRate * Qnt).toFixed(2);
                document.getElementById("txttotcostinr").value = parseFloat(costRate * Qnt * ExRate).toFixed(2);
                document.getElementById("txttotcostaginr").value = parseFloat(AggRate * Qnt * ExRate).toFixed(2);
                document.getElementById("txtselltotinr").value = parseFloat(SellRate * Qnt * SelExRate).toFixed(2);
                document.getElementById("txtsellaginr").value = parseFloat(SellAggRate * Qnt * SelExRate).toFixed(2);
            } else if (costRate != "") {
                document.getElementById("txtTot_Cost_Amt").value = parseFloat(costRate * Qnt).toFixed(2);
                document.getElementById("txtaggreedtot").value = parseFloat(AggRate * Qnt).toFixed(2);
                document.getElementById("txttotcostinr").value = parseFloat(costRate * Qnt * ExRate).toFixed(2);
                document.getElementById("txttotcostaginr").value = parseFloat(AggRate * Qnt * ExRate).toFixed(2);
            } else {
                document.getElementById("txtTot_Sell_Amt").value = parseFloat(SellRate * Qnt).toFixed(2);
                document.getElementById("txtaggreedSelltot").value = parseFloat(SellAggRate * Qnt).toFixed(2);
                document.getElementById("txtselltotinr").value = parseFloat(SellRate * Qnt * SelExRate).toFixed(2);
                document.getElementById("txtsellaginr").value = parseFloat(SellAggRate * Qnt * SelExRate).toFixed(2);
            }

        }
        function Resum() {
            if (txtQnt.Text == "null" && TxtRate.Text == "") {
                txtQnt.Text = '0';

            }
            else {
                var ExRate = document.getElementById("txtExRate").value;
                var Rate = document.getElementById("txtQnt").value;

                document.getElementById("TxtAmount").value = parseFloat(ExRate * Rate).toFixed(2);
            }
        }
        $(document).ready(function () {
            $("#txtCharge").autocomplete({
                source: function (request, response) {
                    $.ajax({
                        url: "../AutoComplete_Pages/Auto_Complete_Searching.asmx/GST_Billing_Charge_Master_List",
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

       
        
    </script>
</body>
</html>
