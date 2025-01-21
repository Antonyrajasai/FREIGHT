﻿using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using BussinessObject;
using System.IO;
using System.Text;
using System.Data.SqlClient;
using System.Configuration;

public partial class PaymentEntry : ThemeClass
{
    AppSession aps = new AppSession();

    GST_Imp_Invoice PE = new GST_Imp_Invoice();
    Global_variables ObjUBO = new Global_variables();

    //PaymentEntry_cs PE = new PaymentEntry_cs();
   

    DataSet ds = new DataSet();
    DataTable dt = new DataTable();

    public int i;
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            aps.checkSession();
            if (!Page.IsPostBack)
            {
                btnSave.Visible = true;
                btnUpdate.Visible = false;
                btnDelete.Visible = false;

                if (Request.QueryString["Page"] == null)
                {
                    load_Vendor_Name();
                    txtDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    btn_Save_Update.Text = "Save";
                }
                else
                {
                    if (Request.QueryString["Page"] != null && Request.QueryString["Page"] != string.Empty)
                    {
                        Update_Item_Load();
                        load_Jobno();
                        
                        btn_Save_Update.Text = "Update";
                    }
                }
                load_Type();
                //Payment_Mode();
                btnSave.Visible = true;
                btnUpdate.Visible = false;
                btnDelete.Visible = false;
            }
        } 
        catch (Exception ex)
        {
            Connection.Error_Msg(ex.Message);
        }
    }
    private void Update_Item_Load()
    {
        try
        {
            ObjUBO.A1 = Request.QueryString["Page"].ToString();
            ObjUBO.A15 = "Updated_Payment_Select";
            ds = PE.Payment_Entry_Select(ObjUBO);

            if (ds.Tables[0].Rows.Count > 0)
            {
                HDupdate_id.Value = ds.Tables[0].Rows[0]["P_ID"].ToString();
                txtPaymentNo.Text = ds.Tables[0].Rows[0]["PAYMENT_NO"].ToString();
                txtDate.Text = ds.Tables[0].Rows[0]["PAYMENT_DATE"].ToString();
                RdPayFor.SelectedValue = ds.Tables[0].Rows[0]["PAYMENT_TYPE"].ToString();

                ddlVendorname.Items.Insert(0, new ListItem(ds.Tables[0].Rows[0]["VENDOR_NAME"].ToString(), ds.Tables[0].Rows[0]["VENDOR_NAME"].ToString()));
                ddlVendorname.SelectedIndex = 0;
                ddlVendorBranch.Items.Insert(0, new ListItem(ds.Tables[0].Rows[0]["VENDOR_BRANCH"].ToString(), ds.Tables[0].Rows[0]["VENDOR_BRANCH"].ToString()));
                ddlVendorBranch.SelectedIndex = 0;
                ddl_Bank_From.Items.Insert(0, new ListItem(ds.Tables[0].Rows[0]["BANK_FROM"].ToString(), ds.Tables[0].Rows[0]["BANK_FROM"].ToString()));
                ddl_Bank_From.SelectedIndex = 0;
                ddl_Acc_No.Items.Insert(0, new ListItem(ds.Tables[0].Rows[0]["BANK_FROM_ACC_NO"].ToString(), ds.Tables[0].Rows[0]["BANK_FROM_ACC_NO"].ToString()));
                ddl_Acc_No.SelectedIndex = 0;
                ddl_ifsc_Code.Items.Insert(0, new ListItem(ds.Tables[0].Rows[0]["BANK_FROM_ACC_IFSC_CODE"].ToString(), ds.Tables[0].Rows[0]["BANK_FROM_ACC_IFSC_CODE"].ToString()));
                ddl_ifsc_Code.SelectedIndex = 0;


                Rd_Mode_Of_Payment.SelectedValue = ds.Tables[0].Rows[0]["PAYMENT_MODE"].ToString();
                txt_Cash_Amount.Text = ds.Tables[0].Rows[0]["PAYMENT_AMOUNT"].ToString();

                txt_Chk_NB_Refno.Text = ds.Tables[0].Rows[0]["CHEQUE_REF_NO"].ToString();
                ddl_To_BankName.SelectedValue = ds.Tables[0].Rows[0]["BANK_TO"].ToString();
                txt_to_Acc_No.Text = ds.Tables[0].Rows[0]["BANK_TO_ACC_NO"].ToString();
                txt_to_Bank_ifsc_Code.Text = ds.Tables[0].Rows[0]["BANK_TO_ACC_IFSC_CODE"].ToString();
                txt_Remarks.Text = ds.Tables[0].Rows[0]["REMARKS"].ToString();
                ddl_chargehead.SelectedValue = ds.Tables[1].Rows[0]["CHARGE_NAME"].ToString(); 
            }
            
            if (ds.Tables[1].Rows.Count > 0)
            {
                gv_Ch.DataSource = ds.Tables[1];
                gv_Ch.DataBind();
                
            }
            else
            {
                gv_Ch.DataSource = dt;
                gv_Ch.DataBind();
                
            }
            /*
            if (ds.Tables[2].Rows.Count > 0)
            {
                txt_D_C_bal_Amt.Text = ds.Tables[2].Rows[0]["Bal_Amt"].ToString();
            }
            */
        }
        catch (Exception ex)
        {
            Connection.Error_Msg(ex.Message);
        }
    }
    private void load_Vendor_Name()
    {
        try
        {
            txtPaymentNo.Text = string.Empty;
            ddlVendorname.Items.Clear();
            ddlVendorBranch.Items.Clear();

            DataSet ds = new DataSet();
            ObjUBO.A15 = "Ven_Name_PNO_Select";
            ds = PE.Payment_Entry_Select(ObjUBO);
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlVendorname.DataSource = ds.Tables[0];
                ddlVendorname.DataTextField = "VEN_NAME";
                ddlVendorname.DataValueField = "VEN_NAME";
                ddlVendorname.DataBind();
            }
            ddlVendorname.Items.Insert(0, new ListItem(String.Empty, String.Empty));
            ddlVendorname.SelectedIndex = 0;

            if (ds.Tables[1].Rows.Count > 0)
            {
                ddl_Bank_From.DataSource = ds.Tables[1];
                ddl_Bank_From.DataTextField = "Bank_Name";
                ddl_Bank_From.DataValueField = "Bank_Name";
                ddl_Bank_From.DataBind();
            }
            ddl_Bank_From.Items.Insert(0, new ListItem(String.Empty, String.Empty));
            ddl_Bank_From.SelectedIndex = 0;

            if (ds.Tables[2].Rows.Count > 0)
            {
                txtPaymentNo.Text = ds.Tables[2].Rows[0]["VOUCHER_NO"].ToString();
            }
        }
        catch (Exception ex)
        {
            Connection.Error_Msg(ex.Message);
        }
    }
    protected void ddlVendorname_SelectedIndexChanged(object sender, EventArgs e)
    {
        //Clear();
        Load_VendorBranch();
    }
    private void Load_VendorBranch()
    {
        ddlVendorBranch.Items.Clear();
        if (ddlVendorname.SelectedValue.ToString() != string.Empty)
        {
            ObjUBO.A3 = ddlVendorname.SelectedValue.ToString();
            ObjUBO.A15 = "Vendor_Branch_Select";
            ds = PE.Payment_Entry_Select(ObjUBO);
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlVendorBranch.DataSource = ds.Tables[0];
                ddlVendorBranch.DataTextField = "Br_Name";
                ddlVendorBranch.DataValueField = "Br_Name";
                ddlVendorBranch.DataBind();
            }
        }
        ddlVendorBranch.Items.Insert(0, new ListItem(String.Empty, String.Empty));
        ddlVendorBranch.SelectedIndex = 0;
    }

      protected void gvPaymentdetails_RowDataBound(object sender, GridViewRowEventArgs e)
      {
          if (e.Row.RowType == DataControlRowType.DataRow)
          {

          }
      }
    protected void gvAll_RowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header)
            e.Row.CssClass = "header";

        if (e.Row.RowType == DataControlRowType.DataRow && e.Row.RowState == DataControlRowState.Normal)
            e.Row.CssClass = "normal";

        if (e.Row.RowType == DataControlRowType.DataRow && e.Row.RowState == DataControlRowState.Alternate)
            e.Row.CssClass = "alternate";
    }
    public void Alert_msg(string msg)
    {
        string prompt = "<script>$(document).ready(function(){{jAlert('" + msg + "', 'Payment Entry', function (r) {});}});</script>";
        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", prompt, false);
    }
    public void Alert_msg(string msg, string focus)
    {
        string prompt = "<script>$(document).ready(function(){{jAlert('" + msg + "', 'Payment Entry', function (r) {document.getElementById('" + focus + "').focus();});}});</script>";
        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", prompt, false);
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
             string jobno = hdnddljob.Value;
            if (jobno != string.Empty)
            {
                if (txt_Payable_Amt.Text != string.Empty && decimal.Parse(txt_Payable_Amt.Text.ToString()) <= decimal.Parse(lbl_Balance_Amt.Text.ToString()))
                        {
                                ds = Payment_Insert_Update("PCS");
                                if (ds.Tables[0].Rows.Count > 0)
                                {
                                    HD_Ch_update_id.Value = ds.Tables[0].Rows[0]["PC_ID"].ToString();

                                    btnUpdate.CssClass = "save";
                                    btnUpdate.Visible = true;
                                    btnSave.Visible = false;
                                    btnSave.Focus();
                                    ViewState["UPDATED_ID"] = null;
                                    Alert_msg("Payment Entry Saved Successfully", "btnSave");
                                }
                                else
                                {
                                    Alert_msg("Payment Not Saved", "btnSave");
                                }
                                  

                                if (ds.Tables[1].Rows.Count > 0)
                                {
                                    gv_Ch.DataSource = ds.Tables[1];
                                    gv_Ch.DataBind();
                                }
                                else
                                {
                                    gv_Ch.DataSource = dt;
                                    gv_Ch.DataBind();
                                }
                        }
                        else
                        {
                            Alert_msg("Enter Ur Payable Amount and Amount Should not Exceed Balance Amount!", "txt_Payable_Amt");
                        }
            }
            else
            {
                Alert_msg("select Ur Jobno !", "ddljobno");
            } 
        }
        catch (Exception ex)
        {
            Connection.Error_Msg(ex.Message);
        }
    }
    protected void btnUpdate_Click(object sender, EventArgs e)
    {
        try
        {
            string jobno = hdnddljob.Value;
            if (jobno != string.Empty)
            {
                if (txt_Payable_Amt.Text != string.Empty && decimal.Parse(txt_Payable_Amt.Text.ToString()) <= decimal.Parse(lbl_Balance_Amt.Text.ToString()))
                {
                    ds = Payment_Insert_Update("PCU");
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        HD_Ch_update_id.Value = ds.Tables[0].Rows[0]["PC_ID"].ToString();

                        ViewState["UPDATED_ID"] = null;
                        Alert_msg("Payment Entry Updated Successfully", "btnUpdate");
                    }
                    else
                    {
                        Alert_msg("Payment Not Updated", "btnUpdate");
                    }

                    if (ds.Tables[1].Rows.Count > 0)
                    {
                        gv_Ch.DataSource = ds.Tables[1];
                        gv_Ch.DataBind();
                    }
                    else
                    {
                        gv_Ch.DataSource = dt;
                        gv_Ch.DataBind();
                    }
                }
                else
                {
                    Alert_msg("Enter Ur Payable Amount and Amount Should not Exceed Balance Amount!", "txt_Payable_Amt");
                }
            }
            else
            {
                Alert_msg("select Ur Jobno !", "ddljobno");
            }
        }
        catch (Exception ex)
        {
            Connection.Error_Msg(ex.Message);
        }
    }
   
    protected void btnDelete_Click(object sender, EventArgs e)
    {
        try
        {
            ds = Payment_Insert_Update("PCD");
            if (ds.Tables[0].Rows.Count == 0)
            {
                ViewState["UPDATED_ID"] = null;
                btnNew_Click(sender, e);
                Alert_msg("Payment Entry Deleted");
            }
            else
            {
                Alert_msg("Payment Not Deleted", "btnDelete");
            }

            if (ds.Tables[1].Rows.Count > 0)
            {
                gv_Ch.DataSource = ds.Tables[1];
                gv_Ch.DataBind();
            }
            else
            {
                gv_Ch.DataSource = dt;
                gv_Ch.DataBind();
            }
        }
        catch (Exception ex)
        {
            Connection.Error_Msg(ex.Message);
        }

    }
    protected void btnNew_Click(object sender, EventArgs e)
    {
        try
        {

            lbl_Jobdate.Text = string.Empty;
            lbl_Cus_Name.Text = string.Empty;
            lbl_Purchase_Amt.Text = "0";
            lbl_Already_Paid_Amt.Text = "0";
            lbl_Balance_Amt.Text = "0";
            txt_Payable_Amt.Text = string.Empty;
            txt_Write_Off_Amt.Text = string.Empty;
            txt_Write_Off_Amt_Remarks.Text = string.Empty;

            btnDelete.Visible = false;
            btnUpdate.Visible = false;
            btnSave.Visible = true;
            btnNew.Visible = true;
            btnUpdate.CssClass = "updates";

            ViewState["UPDATED_ID"] = null;
            HD_Ch_update_id.Value = "";
            load_Jobno();
        }
        catch (Exception ex)
        {
            Connection.Error_Msg(ex.Message);
        }
    }
    protected void GV_RowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header)
            e.Row.CssClass = "header";
    } 
    protected void ddlVendorBranch_SelectedIndexChanged(object sender, EventArgs e)
    {
    }
    protected void btn_Save_Update_Click(object sender, EventArgs e)
    {
        try
        {
            if (txtPaymentNo.Text != string.Empty)
            {
                if (ddlVendorname.SelectedValue.ToString() != string.Empty)
                {
                    if (ddlVendorBranch.SelectedValue.ToString() != string.Empty || hdnvenbranch.Value != string.Empty)
                    {
                         if (ddl_Bank_From.SelectedValue.ToString() != string.Empty)
                            {
                                if (ddl_Acc_No.SelectedValue.ToString() != string.Empty || hdnbankacc.Value != string.Empty)
                                {
                                    if (ddl_ifsc_Code.SelectedValue.ToString() != string.Empty || hdnbankifsc.Value != string.Empty)
                                   {
                                      if (txt_Cash_Amount.Text != string.Empty)
                                      {
                                          if (Rd_Mode_Of_Payment.SelectedValue.ToString() == "CA")
                                          {
                                              Payment_Save();


                                          }
                                          else 
                                          {
                                              if (txt_Chk_NB_Refno.Text != string.Empty)
                                              {
                                                  if (Rd_Mode_Of_Payment.SelectedValue.ToString() == "NB")
                                                  {
                                                      if (ddl_To_BankName.SelectedValue.ToString() != string.Empty)
                                                      {
                                                          if (txt_to_Acc_No.Text != string.Empty)
                                                          {
                                                              if (txt_to_Bank_ifsc_Code.Text != string.Empty)
                                                              {
                                                                  Payment_Save();




                                                              }
                                                              else
                                                              {
                                                                  Alert_msg("Enter Ur To Acc. No ifsc Code!", "txt_to_Bank_ifsc_Code");
                                                              }
                                                          }
                                                          else
                                                          {
                                                              Alert_msg("Enter Ur To Acc. No!", "txt_to_Acc_No");
                                                          }
                                                      }
                                                      else
                                                      {
                                                          Alert_msg("Select Ur Bank To Name!", "ddl_To_BankName");
                                                      }
                                                  }
                                                  else
                                                  {
                                                      Payment_Save();
                                                  }
                                              }
                                              else
                                              {
                                                  Alert_msg("Select Ur Cheque / Ref No!", "txt_Chk_NB_Refno");
                                              }
                                          }
                                      }
                                      else
                                      {
                                          Alert_msg("Enter Ur Amount!", "txt_Cash_Amount");
                                      }
                                    }
                                  else
                                  {
                                      Alert_msg("Select Ur From Bank ifsc Code.!", "ddl_ifsc_Code");
                                  }
                            }
                             else
                             {
                                 Alert_msg("Select Ur From Bank Acc. No.!", "ddl_Acc_No");
                             }
                         }
                         else
                         {
                             Alert_msg("Select Ur From Bank!", "ddl_Bank_From");
                         }
                    }
                    else
                    {
                        Alert_msg("Select Ur Vendor Branch!", "ddlVendorBranch");
                    }
                }
                else
                {
                    Alert_msg("Select Ur VendorName!", "ddlVendorname");
                }
                  
            }
            else
            {
                Alert_msg("Enter Ur Payment!", "txtPaymentNo");
            }
        }
        catch (Exception ex)
        {
            Connection.Error_Msg(ex.Message);
        }
    }

    private void Payment_Save()
    {
        if (btn_Save_Update.Text == "Save")
        {
            ds = Payment_Insert_Update("PS");
            if (ds.Tables[0].Rows.Count > 0)
            {
                HDupdate_id.Value = ds.Tables[0].Rows[0]["P_ID"].ToString();
                btn_Save_Update.Text = "Update";
                Alert_msg("Saved Successfully!", "btn_Save_Update");
              
                Load_VendorBranch();
                ddlVendorBranch.SelectedValue = hdnvenbranch.Value;


                ddl_Bank_From_SelectedIndexChanged(null,null);
                ddl_Acc_No.SelectedValue = hdnbankacc.Value;
                
               
                ddl_Acc_No_SelectedIndexChanged(null, null);
                ddl_ifsc_Code.SelectedValue = hdnbankifsc.Value;
               
                
             load_Jobno();
            
            
            
            }
        }
        else if (btn_Save_Update.Text == "Update")
        {
            ds = Payment_Insert_Update("PU");
            if (ds.Tables[0].Rows.Count > 0)
            {
                btn_Save_Update.Text = "Update";
                Alert_msg("Updated Successfully!", "btn_Save_Update");
            }
        }                                    
    }
      private void load_Type()
    {
        ddlType.Items.Clear();
        ddlType.Items.Add(new ListItem("FORWARDING", "FORWARDING"));
        ddlType.Items.Add(new ListItem("CLEARING", "CLEARING"));
        ddlType.Items.Add(new ListItem("CROSS_COUNTRY", "CROSS_COUNTRY"));
       
    }
    private void load_Jobno()
    {
        try
        {
            ddljobno.Items.Clear();
            DataSet ds = new DataSet();
            ObjUBO.A1 = HDupdate_id.Value.ToString();
            ObjUBO.A2 = txtPaymentNo.Text;
            ObjUBO.A3 = ddlVendorname.SelectedValue.ToString();
            ObjUBO.A4 = ddlVendorBranch.SelectedValue.ToString();
            ObjUBO.A7 = Rd_IMP_EXP.SelectedValue.ToString();
            if (ddlType.SelectedValue == "FORWARDING") { ObjUBO.A9 = "FW"; }
            else if (ddlType.SelectedValue == "CLEARING") { ObjUBO.A9 = "CL"; }
            else if (ddlType.SelectedValue == "CROSS_COUNTRY") { ObjUBO.A9 = "CC"; }

            //ObjUBO.A9 = ddlType.SelectedValue.ToString();
            ObjUBO.A10 = Rd_Mode.SelectedValue.ToString();
            ObjUBO.A15 = "Purchase_Jobno_Select";
            ds = PE.Payment_Entry_Select(ObjUBO);
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddljobno.DataSource = ds.Tables[0];
                ddljobno.DataTextField = "JOBNO";
                ddljobno.DataValueField = "JOBNO";
                ddljobno.DataBind();
            }
            ddljobno.Items.Insert(0, new ListItem(String.Empty, String.Empty));
            ddljobno.SelectedIndex = 0;
        }
        catch (Exception ex)
        {
            Connection.Error_Msg(ex.Message);
        }
    }

    public DataSet Payment_Insert_Update(string S1)
    {
        ObjUBO.A1 = HDupdate_id.Value.ToString();
        ObjUBO.A2 = txtPaymentNo.Text;
        ObjUBO.A3 = txtDate.Text;
        ObjUBO.A4 = RdPayFor.SelectedValue.ToString();



        ObjUBO.A5 = ddlVendorname.SelectedValue.ToString();
       

        if (hdnvenbranch.Value == "")
        {
            ObjUBO.A6 = ddlVendorBranch.SelectedValue.ToString();
        }
        else
        {
            ObjUBO.A6 = hdnvenbranch.Value;
        }

        ObjUBO.A7 = ddl_Bank_From.SelectedValue.ToString();
       



        if (hdnbankacc.Value == "")
        {
            ObjUBO.A8 = ddl_Acc_No.SelectedValue.ToString();
        }
        else
        {
            ObjUBO.A8 = hdnbankacc.Value;
        }

     
        if (hdnbankifsc.Value == "")
        {
            ObjUBO.A9 = ddl_ifsc_Code.SelectedValue.ToString();
        }
        else
        {
            ObjUBO.A9 = hdnbankifsc.Value;
        }

        
        if (hdnCharge.Value == "")
        {
          ObjUBO.A31 = ddl_chargehead.SelectedValue.ToString();
        }
         else
        {
            ObjUBO.A31 = hdnCharge.Value;
        }
        ObjUBO.A10 = Rd_Mode_Of_Payment.SelectedValue.ToString();
        ObjUBO.A11 = txt_Cash_Amount.Text;
        ObjUBO.A12 = txt_Chk_NB_Refno.Text;
        ObjUBO.A13 = ddl_To_BankName.SelectedValue.ToString();
        ObjUBO.A14 = txt_to_Acc_No.Text;
        ObjUBO.A15 = txt_to_Bank_ifsc_Code.Text;
        ObjUBO.A16 = txt_Remarks.Text;

        ObjUBO.A20 = HD_Ch_update_id.Value.ToString();
        ObjUBO.A21 = ddlslno.SelectedValue.ToString();
        ObjUBO.A22 = Rd_IMP_EXP.SelectedValue.ToString(); 
        //ObjUBO.A23 = ddljobno.SelectedValue.ToString();

        if (hdncusdate.Value == "")
        {
            ObjUBO.A24 = lbl_Jobdate.Text;
        }
        else
        {
            ObjUBO.A24 = hdncusdate.Value;
        }

        if (hdncusnam.Value == "")
        {
            ObjUBO.A25 = lbl_Cus_Name.Text;
        }
        else
        {
            ObjUBO.A25 = hdncusnam.Value;
        }
       
      

        ObjUBO.A26 = txt_Payable_Amt.Text;
        ObjUBO.A27 = txt_Write_Off_Amt.Text;
        ObjUBO.A28 = txt_Write_Off_Amt_Remarks.Text;
        ObjUBO.A29 = ddlType.SelectedValue.ToString();
        ObjUBO.A30 = Rd_Mode.SelectedValue.ToString();
        ObjUBO.A60 = S1;
        if (hdnddljob.Value == "")
        {
            ObjUBO.A23 = ddljobno.SelectedValue.ToString();
        }
        else
        {
            ObjUBO.A23 = hdnddljob.Value;
        }
        ds = PE.Payment_Entry_Ins_Upd(ObjUBO);
        return ds;
    }
    protected void ddl_Bank_From_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            ddl_Acc_No.Items.Clear();
            if (ddl_Bank_From.SelectedValue.ToString() != string.Empty)
            {
                ObjUBO.A5 = ddl_Bank_From.SelectedValue.ToString();
                ObjUBO.A15 = "From_Bank_Acc_No_Select";
                ds = PE.Payment_Entry_Select(ObjUBO);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    ddl_Acc_No.DataSource = ds.Tables[0];
                    ddl_Acc_No.DataTextField = "Acc_No_Loc";
                    ddl_Acc_No.DataValueField = "Acc_No";
                    ddl_Acc_No.DataBind();
                }
            }
            ddl_Acc_No.Items.Insert(0, new ListItem(String.Empty, String.Empty));
            ddl_Acc_No.SelectedIndex = 0;
        }
        catch (Exception ex)
        {
            Connection.Error_Msg(ex.Message);
        }
    }
    protected void ddl_Acc_No_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            ddl_ifsc_Code.Items.Clear();
            if (ddl_Bank_From.SelectedValue.ToString() != string.Empty)
            {
                ObjUBO.A5 = ddl_Bank_From.SelectedValue.ToString();
                ObjUBO.A6 = ddl_Acc_No.SelectedValue.ToString();
                ObjUBO.A15 = "From_Bank_Acc_No_ifsc_Select";
                ds = PE.Payment_Entry_Select(ObjUBO);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    ddl_ifsc_Code.DataSource = ds.Tables[0];
                    ddl_ifsc_Code.DataTextField = "IFSC_Br";
                    ddl_ifsc_Code.DataValueField = "IFSCODE";
                    ddl_ifsc_Code.DataBind();
                }
            }
        }
        catch (Exception ex)
        {
            Connection.Error_Msg(ex.Message);
        }
    }

    private void Payment_Mode()
    {
        if (Rd_Mode_Of_Payment.SelectedValue.ToString() == "CA")
        { 
            lbl_Chk_Nb_refno.Visible=false;
            lbl_NB.Visible=false;
        }
        else if (Rd_Mode_Of_Payment.SelectedValue.ToString() == "CH")
        {
            lbl_Chk_Nb_refno.Visible=true;
            lbl_NB.Visible=false;
        }
        else if (Rd_Mode_Of_Payment.SelectedValue.ToString() == "NB")
        {
            lbl_Chk_Nb_refno.Visible = true;
            lbl_NB.Visible = true;
        }
    }
    protected void Rd_Mode_Of_Payment_SelectedIndexChanged(object sender, EventArgs e)
    {
        Payment_Mode();
    }
    protected void Rd_IMP_EXP_SelectedIndexChanged(object sender, EventArgs e)
    {
        btnNew_Click(sender, e);
        load_Jobno();
    }
    protected void ddljobno_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            lbl_Cus_Name.Text = string.Empty;
            lbl_Jobdate.Text = string.Empty;
            lbl_Purchase_Amt.Text = "0";
            lbl_Already_Paid_Amt.Text = "0";

            DataSet ds = new DataSet();
            ObjUBO.A1 = HDupdate_id.Value.ToString();
            ObjUBO.A2 = txtPaymentNo.Text;

            ObjUBO.A3 = ddlVendorname.SelectedValue.ToString();
            ObjUBO.A4 = ddlVendorBranch.SelectedValue.ToString();

            ObjUBO.A7 = Rd_IMP_EXP.SelectedValue.ToString();
            ObjUBO.A8 = ddljobno.SelectedValue.ToString();
           
            ObjUBO.A10 = Rd_Mode.SelectedValue.ToString();

            if (ddlType.SelectedValue == "FORWARDING") { ObjUBO.A9 = "FW"; }
            else if (ddlType.SelectedValue == "CLEARING") { ObjUBO.A9 = "CL"; }
            else if (ddlType.SelectedValue == "CROSS_COUNTRY") { ObjUBO.A9 = "CC"; }
            ObjUBO.A15 = "Purchase_Jobno_Data_Select";

            ds = PE.Payment_Entry_Select(ObjUBO);
            if (ds.Tables[0].Rows.Count > 0)
            {
                lbl_Cus_Name.Text = ds.Tables[0].Rows[0]["CUSTOMER_NAME"].ToString();
                lbl_Jobdate.Text = ds.Tables[0].Rows[0]["JOB_DATE"].ToString();
            }
            Job_Amt_Cal();
        }
        catch (Exception ex)
        {
            Connection.Error_Msg(ex.Message);
        }
    }
   
    protected void gv_Ch_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Enabled = true;
            e.Row.Attributes["onmouseover"] = "this.style.cursor='pointer';";
            e.Row.Attributes.Add("onclick", ClientScript.GetPostBackClientHyperlink(this.gv_Ch, "Select$" + e.Row.RowIndex.ToString()));
        }
    }
    protected void gv_Ch_RowCreated(object sender, GridViewRowEventArgs e)
     {
         if (e.Row.RowType == DataControlRowType.Header)
             e.Row.CssClass = "header";
         if (e.Row.RowType == DataControlRowType.DataRow && e.Row.RowState == DataControlRowState.Normal)
             e.Row.CssClass = "normal";
         if (e.Row.RowType == DataControlRowType.DataRow && e.Row.RowState == DataControlRowState.Alternate)
             e.Row.CssClass = "alternate";
     }
    protected void gv_Ch_SelectedIndexChanged(object sender, EventArgs e)
    {
        ViewState["C_ID"] = Convert.ToString(this.gv_Ch.SelectedDataKey.Value);
        ddljobno.Items.Clear();
        ddl_chargehead.Items.Clear();
        if (ViewState["C_ID"] != "" && ViewState["C_ID"] != null)
        {
            ViewState["UPDATED_ID"] = null;
            ViewState["UPDATED_ID"] = ViewState["C_ID"].ToString();
            btnUpdate.CssClass = "updates";
            //---------------------------------------------
            ddlslno.SelectedValue = gv_Ch.SelectedRow.Cells[1].Text.Replace("&nbsp;", "");
            Rd_IMP_EXP.SelectedValue = gv_Ch.SelectedRow.Cells[2].Text.Replace("&nbsp;", "");
            ddljobno.Items.Insert(0, new ListItem(gv_Ch.SelectedRow.Cells[3].Text.Replace("&nbsp;", ""), gv_Ch.SelectedRow.Cells[3].Text.Replace("&nbsp;", "")));
            ddljobno.SelectedIndex = 0;
            lbl_Jobdate.Text = gv_Ch.SelectedRow.Cells[4].Text.Replace("01/01/1900", "");
            lbl_Cus_Name.Text = gv_Ch.SelectedRow.Cells[5].Text.Replace("&nbsp;", "");

            txt_Payable_Amt.Text = gv_Ch.SelectedRow.Cells[6].Text.Replace("&nbsp;", "");
            txt_Write_Off_Amt.Text = gv_Ch.SelectedRow.Cells[7].Text.Replace("&nbsp;", "");
            txt_Write_Off_Amt_Remarks.Text = gv_Ch.SelectedRow.Cells[8].Text.Replace("&nbsp;", "");

            ddl_chargehead.Items.Insert(0, new ListItem(gv_Ch.SelectedRow.Cells[9].Text.Replace("&nbsp;", ""), gv_Ch.SelectedRow.Cells[9].Text.Replace("&nbsp;", "")));
            ddl_chargehead.SelectedIndex = 0;
            HD_Ch_update_id.Value = ViewState["C_ID"].ToString();
            //--------------------------------------------------

            btnDelete.Visible = true;
            btnUpdate.Visible = true;
            btnSave.Visible = false;
            btnNew.Visible = true;
        }
        else
        {
            btnDelete.Visible = false;
            btnUpdate.Visible = false;
            btnSave.Visible = true;
            btnNew.Visible = true;
        }
        Job_Amt_Cal();
    }

    private void Job_Amt_Cal()
    {
        lbl_Purchase_Amt.Text = "0";
        lbl_Already_Paid_Amt.Text = "0";
        lbl_Balance_Amt.Text = "0";
        DataSet ds = new DataSet();
        ObjUBO.A1 = HDupdate_id.Value.ToString();
        ObjUBO.A2 = txtPaymentNo.Text;
        ObjUBO.A3 = ddlVendorname.SelectedValue.ToString();
        ObjUBO.A4 = ddlVendorBranch.SelectedValue.ToString();
        ObjUBO.A7 = Rd_IMP_EXP.SelectedValue.ToString();
        ObjUBO.A8 = ddljobno.SelectedValue.ToString();
        ObjUBO.A10 = Rd_Mode.SelectedValue.ToString();

        if (ddlType.SelectedValue == "FORWARDING") { ObjUBO.A9 = "FW"; }
        else if (ddlType.SelectedValue == "CLEARING") { ObjUBO.A9 = "CL"; }
        else if (ddlType.SelectedValue == "CROSS_COUNTRY") { ObjUBO.A9 = "CC"; }
        ObjUBO.A11 = ddlType.SelectedValue.ToString();
        ObjUBO.A31 = ddl_chargehead.SelectedValue.ToString();
        ObjUBO.A15 = "Payment_Jobno_Amt_Select";
        ds = PE.Payment_Entry_Select(ObjUBO);
        if (ds.Tables[0].Rows.Count > 0)
        {
            lbl_Purchase_Amt.Text = ds.Tables[0].Rows[0]["PURCHASE_AMT"].ToString();
            lbl_Already_Paid_Amt.Text = ds.Tables[0].Rows[0]["Already_Paid_Amt"].ToString();
            lbl_Balance_Amt.Text = ds.Tables[0].Rows[0]["Bal_Amt"].ToString();
        }

    }


    [System.Web.Services.WebMethod]
    public static string Get_Vendor_Det(string Vendor)
    {
        string Vendor_Branch_detail = "";
        string str = "  ,";
        DataSet ds = new DataSet();
        try
        {
            GST_Imp_Invoice PE = new GST_Imp_Invoice();
            Global_variables ObjUBO = new Global_variables();



            ObjUBO.A3 = Vendor;
            ObjUBO.A15 = "Vendor_Branch_Select";
            ds = PE.Payment_Entry_Select(ObjUBO);
            if (ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    Vendor_Branch_detail = ds.Tables[0].Rows[i]["Br_Name"].ToString();

                    str += Vendor_Branch_detail + ",";


                }
                
            }




            return str;


            //------------------------------------
        }
        catch (Exception ex)
        {
            Connection.Error_Msg(ex.Message);
        }
        return str;
    }

    [System.Web.Services.WebMethod]
    public static string Get_Bank_Det(string Bank)
    {
        string Bank_detail = "";
        string str = "  ,";
        DataSet ds = new DataSet();
        try
        {
            GST_Imp_Invoice PE = new GST_Imp_Invoice();
            Global_variables ObjUBO = new Global_variables();



            ObjUBO.A5 = Bank;
                ObjUBO.A15 = "From_Bank_Acc_No_Select";
                
                


          
            ds = PE.Payment_Entry_Select(ObjUBO);
            if (ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    Bank_detail = ds.Tables[0].Rows[i]["Acc_No"].ToString();

                    str += Bank_detail + ",";


                }

            }




            return str;


            //------------------------------------
        }
        catch (Exception ex)
        {
            Connection.Error_Msg(ex.Message);
        }
        return str;
    }


    [System.Web.Services.WebMethod]
    public static string Get_Ifsc_Det(string Bank,string acc)
    {
        string ifsc = "";
        string str = "  ,";
        DataSet ds = new DataSet();
        try
        {
            GST_Imp_Invoice PE = new GST_Imp_Invoice();
            Global_variables ObjUBO = new Global_variables();


            ObjUBO.A5 = Bank;
            ObjUBO.A6 = acc;
            ObjUBO.A15 = "From_Bank_Acc_No_ifsc_Select";
            
            ds = PE.Payment_Entry_Select(ObjUBO);




            ds = PE.Payment_Entry_Select(ObjUBO);
            if (ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    ifsc = ds.Tables[0].Rows[i]["IFSC_CODE"].ToString();

                    str += ifsc + ",";


                }

            }




            return str;


            //------------------------------------
        }
        catch (Exception ex)
        {
            Connection.Error_Msg(ex.Message);
        }
        return str;
    }



    [System.Web.Services.WebMethod]
    public static string Get_Job_Det(string Vendor,string Vendor_Branch,string Payment_no,string P_ID,string Imp_Exp, string Mode, string Type)
    {
        string Job = "";
        string str = "  ,";
        DataSet ds = new DataSet();
        try
        {
            GST_Imp_Invoice PE = new GST_Imp_Invoice();
            Global_variables ObjUBO = new Global_variables();


            ObjUBO.A1 = P_ID;
            ObjUBO.A2 = Payment_no;
            ObjUBO.A3 = Vendor;
            ObjUBO.A4 = Vendor_Branch;
            ObjUBO.A7 = Imp_Exp;
            
            if (Type == "FORWARDING") { ObjUBO.A9 = "FW"; }
            else if (Type == "CLEARING") { ObjUBO.A9 = "CL"; }
            else if (Type == "CROSS_COUNTRY") { ObjUBO.A9 = "CC"; }

            ObjUBO.A10 = Mode;
            ObjUBO.A15 = "Purchase_Jobno_Select";
            ds = PE.Payment_Entry_Select(ObjUBO);
            if (ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    Job = ds.Tables[0].Rows[i]["JOBNO"].ToString();

                    str += Job + ",";


                }

            }




            return str;


            //------------------------------------
        }
        catch (Exception ex)
        {
            Connection.Error_Msg(ex.Message);
        }
        return str;
    }



    [System.Web.Services.WebMethod]
    public static string Get_Cus_date_Det(string Vendor, string Vendor_Branch, string Payment_no, string P_ID, string Imp_Exp, string Mode, string Type,string jobno)
    {
        string Job = "";
        string str = "  ,";
        string str1 = "";
        string res="";
        DataSet ds = new DataSet();
        try
        {
            GST_Imp_Invoice PE = new GST_Imp_Invoice();
            Global_variables ObjUBO = new Global_variables();


            ObjUBO.A1 = P_ID;
            ObjUBO.A2 = Payment_no;
            ObjUBO.A3 = Vendor;
            ObjUBO.A4 = Vendor_Branch;
            ObjUBO.A7 = Imp_Exp;
            if (Type == "FORWARDING") { ObjUBO.A9 = "FW"; }
            else if (Type == "CLEARING") { ObjUBO.A9 = "CL"; }
            else if (Type == "CROSS_COUNTRY") { ObjUBO.A9 = "CC"; }

            ObjUBO.A10 = Mode;
             ObjUBO.A8 = jobno;
             ObjUBO.A11 = Type;
            ObjUBO.A15 = "Purchase_Jobno_Data_Select";
            ds = PE.Payment_Entry_Select(ObjUBO);
            if (ds.Tables[0].Rows.Count > 0)
            {

                str = ds.Tables[0].Rows[0]["CUSTOMER_NAME"].ToString() + "," + ds.Tables[0].Rows[0]["JOB_DATE"].ToString();

                    


              
            }

            ObjUBO.A15 = "Payment_Jobno_Amt_Select";

            ds = PE.Payment_Entry_Select(ObjUBO);
            if (ds.Tables[0].Rows.Count > 0)
            {

                str1 = ds.Tables[0].Rows[0]["PURCHASE_AMT"].ToString() + "," + ds.Tables[0].Rows[0]["Already_Paid_Amt"].ToString() + ","
                    + ds.Tables[0].Rows[0]["Bal_Amt"].ToString();
            }

            
            res=str+","+str1;
            
            return res;


            //------------------------------------
        }
        catch (Exception ex)
        {
            Connection.Error_Msg(ex.Message);
        }
        return res;
    }



    [System.Web.Services.WebMethod]
    public static string Get_Cus_Det(string Vendor, string Vendor_Branch, string Payment_no, string P_ID, string Imp_Exp, string Mode, string Type,string jobno)
    {
        string Job = "";
        string str = "";
        string str1 = "";
        string res = "";
        DataSet ds = new DataSet();
        try
        {
            GST_Imp_Invoice PE = new GST_Imp_Invoice();
            Global_variables ObjUBO = new Global_variables();


            ObjUBO.A1 = P_ID;
            ObjUBO.A2 = Payment_no;
            ObjUBO.A3 = Vendor;
            ObjUBO.A4 = Vendor_Branch;
            ObjUBO.A7 = Imp_Exp;
            ObjUBO.A8 = jobno;
            if (Type == "FORWARDING") { ObjUBO.A9 = "FW"; }
            else if (Type == "CLEARING") { ObjUBO.A9 = "CL"; }
            else if (Type == "CROSS_COUNTRY") { ObjUBO.A9 = "CC"; }
            ObjUBO.A11 = Type;
            ObjUBO.A10 = Mode;
            ObjUBO.A15 = "Purchase_Jobno_Data_Select";
            ds = PE.Payment_Entry_Select(ObjUBO);
            if (ds.Tables[0].Rows.Count > 0)
            {

                str = ds.Tables[0].Rows[0]["CUSTOMER_NAME"].ToString() + "," + ds.Tables[0].Rows[0]["JOB_DATE"].ToString();





            }

            ObjUBO.A15 = "Payment_Jobno_Amt_Select";

            ds = PE.Payment_Entry_Select(ObjUBO);
            if (ds.Tables[0].Rows.Count > 0)
            {

                str1 = ds.Tables[0].Rows[0]["PURCHASE_AMT"].ToString() + "," + ds.Tables[0].Rows[0]["Already_Paid_Amt"].ToString() + ","
                    + ds.Tables[0].Rows[0]["Bal_Amt"].ToString();
            }


            res = str + ",";//+ str1;

            return res;


            //------------------------------------
        }
        catch (Exception ex)
        {
            Connection.Error_Msg(ex.Message);
        }
        return res;
    }
    

    [System.Web.Services.WebMethod]
    public static string Get_charge_head(string Vendor, string Vendor_Branch, string Payment_no, string P_ID, string Imp_Exp, string Mode, string Type, string jobno)
    {

        string Charge = "";
        string str = "";
        string res = "  ,";
        DataSet ds = new DataSet();
        try
        {
            GST_Imp_Invoice PE = new GST_Imp_Invoice();
            Global_variables ObjUBO = new Global_variables();


            ObjUBO.A1 = P_ID;
            ObjUBO.A2 = Payment_no;
            ObjUBO.A3 = Vendor;
            ObjUBO.A4 = Vendor_Branch;
            ObjUBO.A7 = Imp_Exp;
            ObjUBO.A8 = jobno;
            if (Type == "FORWARDING") { ObjUBO.A9 = "FW"; }
            else if (Type == "CLEARING") { ObjUBO.A9 = "CL"; }
            else if (Type == "CROSS_COUNTRY") { ObjUBO.A9 = "CC"; }
            ObjUBO.A11 = Type;
            ObjUBO.A10 = Mode;
            ObjUBO.A15 = "Purchase_Jobno_Data_Select";
            ds = PE.Payment_Entry_Select(ObjUBO);
            if (ds.Tables[0].Rows.Count > 0)
            {
              
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        Charge = ds.Tables[0].Rows[i]["CHARGE_NAME"].ToString();

                        res += Charge + ",";


                    }
                
            }


            //------------------------------------
        }
        catch (Exception ex)
        {
            Connection.Error_Msg(ex.Message);
        }
        return res;

    }
    [System.Web.Services.WebMethod]
    public static string Get_charge_head_Amt(string Vendor, string Vendor_Branch, string Payment_no, string P_ID, string Imp_Exp, string Mode, string Type, string jobno, string Charge)
    {
        string Job = "";
        string str = "";
        string str1 = "";
        string res = "  ,";
        DataSet ds = new DataSet();
        try
        {
            GST_Imp_Invoice PE = new GST_Imp_Invoice();
            Global_variables ObjUBO = new Global_variables();


            ObjUBO.A1 = P_ID;
            ObjUBO.A2 = Payment_no;
            ObjUBO.A3 = Vendor;
            ObjUBO.A4 = Vendor_Branch;
            ObjUBO.A7 = Imp_Exp;
            ObjUBO.A8 = jobno;
            if (Type == "FORWARDING") { ObjUBO.A9 = "FW"; }
            else if (Type == "CLEARING") { ObjUBO.A9 = "CL"; }
            else if (Type == "CROSS_COUNTRY") { ObjUBO.A9 = "CC"; }
            ObjUBO.A11 = Type;
            ObjUBO.A10 = Mode;
            ObjUBO.A31 = Charge;
            ObjUBO.A15 = "Payment_Jobno_Amt_Select";

            ds = PE.Payment_Entry_Select(ObjUBO);
            if (ds.Tables[0].Rows.Count > 0)
            {

                str1 = ds.Tables[0].Rows[0]["PURCHASE_AMT"].ToString() + "," + ds.Tables[0].Rows[0]["Already_Paid_Amt"].ToString() + ","
                    + ds.Tables[0].Rows[0]["Bal_Amt"].ToString();
            }


            res = str1 + ",";

            return res;


            //------------------------------------
        }
        catch (Exception ex)
        {
            Connection.Error_Msg(ex.Message);
        }
        return res;
    }


}