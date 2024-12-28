using System;
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

public partial class ReceiptEntry : ThemeClass
{

    User_Creation user_Create = new User_Creation();
    AppSession aps = new AppSession();
    eroyalmaster erm = new eroyalmaster();

    ReceiptEntry_cs BI = new ReceiptEntry_cs();
    Billing_UserBO ObjUBO = new Billing_UserBO();

    public int i;
    private string savemode = "";
    public string COMPANY_LICENSE; 
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            aps.checkSession();
            COMPANY_LICENSE = Connection.Company_License().ToLower();

            if (!Page.IsPostBack)
            {
                load_Imp_name();
                Load_Bank_Name("");

                btnSave.Visible = true;
                btnUpdate.Visible = false;
                btnDelete.Visible = false;

                btnrpt.Visible = false;
                T.Visible = false;

                WK.Visible = true;
                LoadFinancialYear();
                ddlWorkingPeriod.SelectedValue = Connection.WorkingPeriod();


                if (Request.QueryString["Page"] == null)
                {
                    GetAutono_Receipt();
                    txtDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                }
                else
                {
                    if (Request.QueryString["Page"] != null && Request.QueryString["Page"] != string.Empty)
                    {
                        Update_Item_Load();
                        btnDelete.Visible = true;
                        btnUpdate.Visible = true;
                        btnSave.Visible = false;
                    }
                }
            }
        } 
        catch (Exception ex)
        {
            Connection.Error_Msg(ex.Message);
        }
    }
    private void GetAutono_Receipt()
    {
        try
        {
            DataSet ds = new DataSet();
            ObjUBO.Flag = "Generate-AutoNo";
            ObjUBO.BILL_TYPE = "Payment-Receipt";
            ds = BI.Load_DDL(ObjUBO);
            if (ds.Tables[0].Rows.Count > 0)
            {
                txtReceiptNo.Text = ds.Tables[0].Rows[0][0].ToString();
            }
            ds.Dispose();
            GC.Collect();
        }
        catch (Exception ex)
        {
            Connection.Error_Msg(ex.Message);
        }
    }

      private void LoadPendingJob()
        {
          try
          {
            DataSet ds = new DataSet();   
            ObjUBO.Flag = "Select-ImpExp-CustomerPendingBills";
            ObjUBO.BRANCH_CODE = ddlbranch_No.SelectedValue.ToString();
            ObjUBO.Ref_ID = ddlCustomer.SelectedValue.ToString();

            if (Connection.Billing_Working_Period() == "Y" && HDupdate_id.Value == "" )
            {
                ds = Database_Con_string();
            }
            else
            {
                ds = BI.Load_DDL(ObjUBO);
            }
            if (ds.Tables[0].Rows.Count > 0)
            {
                ViewState["PendingBillList"] = ds.Tables[0];
                if (RdBillsLoad.SelectedIndex != 0) //Means Auto Selection  // if (RdPayFor.SelectedIndex == 0)
                {
                    gv_Chg_Details.DataSource = ds.Tables[0];
                    gv_Chg_Details.DataBind();
                }
                else
                {
                    FirstRow();
                }

            }
            else
            {
                Alert_msg("No Pending Bills, Choose Advance Payment Mode");
            }
          
            ViewState["CurrentTable"] = ds.Tables[1];
         
            if (ds.Tables[2].Rows.Count > 0)
            {
                gvReceiptdetails.DataSource = ds.Tables[2];
                gvReceiptdetails.DataBind();

                ViewState["ReceiptHistory"] = ds.Tables[2];

                txtTotBillAmt.Text = ds.Tables[2].Compute("Sum(ReceivedAmt)", "").ToString();  
                txtTotTDSAmt.Text = ds.Tables[2].Compute("Sum(TDSAmt)", "").ToString();
                txtFinalReceivedAmt.Text = ds.Tables[2].Compute("Sum(TotalReceived)", "").ToString();
                txtTotWriteOff.Text = ds.Tables[2].Compute("Sum(WriteOffAmt)", "").ToString(); 
            }

            ds.Dispose();
            GC.Collect();
          }
          catch (Exception ex)
          {
              Connection.Error_Msg(ex.Message);
          }
        }

      protected void gvReceiptdetails_RowDataBound(object sender, GridViewRowEventArgs e)
      {
          if (e.Row.RowType == DataControlRowType.DataRow)
          {

          }
      }

      protected void gv_Chg_Details_RowDataBound(object sender, GridViewRowEventArgs e)
      {
          if (e.Row.RowType == DataControlRowType.DataRow)
          {
             
          }
      }

      protected void RDBillInvNo_CheckedChanged(object sender, EventArgs e)
      {
          try
          {
              if (Request.Form["RDBillInvNo"] != null && Request.Form["RDBillInvNo"] != string.Empty)
              {
                  string bill = Request.Form["RDBillInvNo"];
                  DataView dv = new DataView((DataTable)ViewState["ReceiptHistory"]);
                  dv.RowFilter = "BillInvNo='" + bill + "' "; 
                  DataTable dtv = dv.ToTable();

                  gvReceiptdetails.DataSource = dtv;
                  gvReceiptdetails.DataBind();

                  txtTotBillAmt.Text =
                  txtTotBillAmt.Text = dtv.Compute("Sum(ReceivedAmt)", "").ToString();
                  txtTotTDSAmt.Text = dtv.Compute("Sum(TDSAmt)", "").ToString();
                  txtFinalReceivedAmt.Text = dtv.Compute("Sum(TotalReceived)", "").ToString();
                  txtTotWriteOff.Text = dtv.Compute("Sum(WriteOffAmt)", "").ToString();
              }

              ScriptManager.RegisterStartupScript(this, typeof(Page), "OnClientClicking", "B_G_tab_page2();", true);
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

            DataSet dss = new DataSet();
            ObjUBO.MODE = "Select Receipt";
            ObjUBO.Flag = "Select-ReceiptSingle";
            ObjUBO.FILE_REF_NO = Request.QueryString["Page"].ToString();
            HDupdate_id.Value = Request.QueryString["Page"].ToString();

            dss = BI.Exp_Pay_Search(ObjUBO);

            if (dss.Tables[0].Rows.Count > 0)
            {
                HDupdate_id.Value = dss.Tables[0].Rows[0]["Receipt_ID"].ToString();
                txtReceiptNo.Text = dss.Tables[0].Rows[0]["Receipt_VoucherNo"].ToString();
                ddlCustomer.SelectedValue = dss.Tables[0].Rows[0]["Customer_Name"].ToString();
                ddlModeofPay.SelectedValue = dss.Tables[0].Rows[0]["PaymentMode"].ToString();
                txtCommNarration.Text = dss.Tables[0].Rows[0]["Narration"].ToString();
                txtDate.Text = dss.Tables[0].Rows[0]["Receipt_Date"].ToString();
                ddlPayFor.SelectedValue = dss.Tables[0].Rows[0]["PaymentFor"].ToString();
                ddlPaymentfrom.SelectedValue = dss.Tables[0].Rows[0]["PaymentFrom"].ToString();
                ddlbranch_No.SelectedValue = dss.Tables[0].Rows[0]["CustBranch"].ToString();
                ddlDepositInBank.SelectedValue = dss.Tables[0].Rows[0]["DepositedBank"].ToString();
                txtBankReceiptNo.Text = dss.Tables[0].Rows[0]["BankReceiptNo"].ToString();

                if (dss.Tables[0].Rows[0]["imp_exp"].ToString() == "Imp")
                {
                    Rd_Job_Type.SelectedIndex = 0;
                    load_Imp_name();
                }
                else if (dss.Tables[0].Rows[0]["imp_exp"].ToString() == "Exp" )
                {
                    Rd_Job_Type.SelectedIndex = 1;
                    load_Imp_name();
                }
                else if (dss.Tables[0].Rows[0]["imp_exp"].ToString() == "Both" || dss.Tables[0].Rows[0]["imp_exp"].ToString() == "I"  || dss.Tables[0].Rows[0]["imp_exp"].ToString() == "E")
                {
                    Rd_Job_Type.SelectedIndex = 2;
                    load_Imp_name();
                }

                ddlCustomer.Text = dss.Tables[0].Rows[0]["Customer_Name"].ToString();

                LoadPayDiv_inSinglePanel();

                if (ddlModeofPay.SelectedValue.ToString() == "Cheque")
                {
                    txtChqNo.Text = dss.Tables[0].Rows[0]["Chq_Net_RefNo"].ToString();
                    txtChqDate.Text = dss.Tables[0].Rows[0]["Chq_Net_Date"].ToString();
                    txtChqBank.Text = dss.Tables[0].Rows[0]["Bank"].ToString();
                    txtChqBranch.Text = dss.Tables[0].Rows[0]["Branch"].ToString();
                    txtChqAmt.Text = dss.Tables[0].Rows[0]["TotalReceiptAmt"].ToString();
                }
                else if (ddlModeofPay.SelectedValue.ToString() == "Cash")
                {
                    txtChqAmt.Text = dss.Tables[0].Rows[0]["TotalReceiptAmt"].ToString();                    
                }
                else if (ddlModeofPay.SelectedValue.ToString() == "Net Banking")
                {
                    txtChqAmt.Text = dss.Tables[0].Rows[0]["TotalReceiptAmt"].ToString();
                    txtChqBank.Text = dss.Tables[0].Rows[0]["Bank"].ToString();
                    txtChqBranch.Text = dss.Tables[0].Rows[0]["Branch"].ToString();
                    txtChqNo.Text = dss.Tables[0].Rows[0]["Chq_Net_RefNo"].ToString();
                    txtChqDate.Text = dss.Tables[0].Rows[0]["Chq_Net_Date"].ToString();
                }

                LoadBranch();

                if (dss.Tables[0].Rows[0]["IsJob_Advance"].ToString() == "Job")
                {
                    ddlPayFor.SelectedIndex = 0;
                }
                else
                {
                    ddlPayFor.SelectedIndex = 1;
                }
                
                    gv_Chg_Details.DataSource = dss.Tables[0];
                    gv_Chg_Details.DataBind();
                    int rowIndex = 0;
                    for (int i = 0; i < dss.Tables[0].Rows.Count; i++)
                    {

                        decimal totamt = 0;

                        TextBox txtUser_BranchName = (TextBox)gv_Chg_Details.Rows[rowIndex].FindControl("txtUser_BranchName");
                        TextBox txtBill_inv_No = (TextBox)gv_Chg_Details.Rows[rowIndex].FindControl("txtBill_inv_No");
                        TextBox txtBillFor = (TextBox)gv_Chg_Details.Rows[rowIndex].FindControl("txtBill_For");

                        TextBox txtBill_inv_date = (TextBox)gv_Chg_Details.Rows[rowIndex].FindControl("txtBill_inv_date");

                        TextBox txtJOBNO = (TextBox)gv_Chg_Details.Rows[rowIndex].FindControl("txtJOBNO");
                        TextBox txtIMP_EXP = (TextBox)gv_Chg_Details.Rows[rowIndex].FindControl("txtIMP_EXP");
                        TextBox txtBillAmt = (TextBox)gv_Chg_Details.Rows[rowIndex].FindControl("txtBillAmt");
                        TextBox txtReceivedAmt = (TextBox)gv_Chg_Details.Rows[rowIndex].FindControl("txtReceivedAmt");

                        CheckBox chksel = (CheckBox)gv_Chg_Details.Rows[rowIndex].FindControl("chk_inv");
                        TextBox txtPayAmt1 = (TextBox)gv_Chg_Details.Rows[rowIndex].FindControl("txtPayAmt");

                        TextBox txtBalAmt1 = (TextBox)gv_Chg_Details.Rows[rowIndex].FindControl("txtBalAmt1");
                        TextBox txtTDSPercent1 = (TextBox)gv_Chg_Details.Rows[rowIndex].FindControl("txtTDSPercent");
                        TextBox txtTDSAmt1 = (TextBox)gv_Chg_Details.Rows[rowIndex].FindControl("txtTDSAmt");
                        TextBox txtBillRemarks1 = (TextBox)gv_Chg_Details.Rows[rowIndex].FindControl("txtBillRemarks");

                        CheckBox chkWrite1 = (CheckBox)gv_Chg_Details.Rows[rowIndex].FindControl("chkWrite");
                        TextBox txtWriteOffAmt1 = (TextBox)gv_Chg_Details.Rows[rowIndex].FindControl("txtWriteOffAmt");

                        chksel.Checked = true;
                        string AA;
                        AA=dss.Tables[0].Rows[i]["TOTAL_AMT_TAX"].ToString();

                        if (AA != "")
                        {
                            totamt = Convert.ToDecimal(AA);
                        }
                        else
                        {
                            totamt = 0;
                        }


                        txtPayAmt1.Text = dss.Tables[0].Rows[i]["Currentpayable"].ToString();
                        txtBillFor.Text = dss.Tables[0].Rows[i]["BillFor"].ToString();
                        txtTDSPercent1.Text = dss.Tables[0].Rows[i]["TDS_Percent"].ToString();
                        txtTDSAmt1.Text = dss.Tables[0].Rows[i]["TDS_Amt"].ToString();
                        txtBillRemarks1.Text = dss.Tables[0].Rows[i]["Remarks"].ToString();

                        if (dss.Tables[0].Rows[i]["IsWriteoff"].ToString() == "Y")
                        {
                            chkWrite1.Checked = true;
                            txtWriteOffAmt1.Text = dss.Tables[0].Rows[i]["WriteoffAmt"].ToString();
                        }

                        rowIndex++;
                    }
                 
            }
            ViewState["CurrentTable"] = dss.Tables[1];  // old coding
            ViewState["NewCurrentTable"] = dss.Tables[0];
            if (dss.Tables[2].Rows.Count > 0)
            {
                gvReceiptdetails.DataSource = dss.Tables[2];
                gvReceiptdetails.DataBind();

                ViewState["ReceiptHistory"] = dss.Tables[2];

                txtTotBillAmt.Text = dss.Tables[2].Compute("Sum(ReceivedAmt)", "").ToString();
                txtTotTDSAmt.Text = dss.Tables[2].Compute("Sum(TDSAmt)", "").ToString();
                txtFinalReceivedAmt.Text = dss.Tables[2].Compute("Sum(TotalReceived)", "").ToString();
                txtTotWriteOff.Text = dss.Tables[2].Compute("Sum(WriteOffAmt)", "").ToString();
            }

            dss.Dispose();
            GC.Collect();
        }

        
        catch (Exception ex)
        {
            Connection.Error_Msg(ex.Message);
        }
    }

    private void FirstRow()
    {
        DataTable dt = new DataTable();
        DataRow dr = dt.NewRow();
        
        dt.Columns.Add(new DataColumn("USER_BRANCHNAME", typeof(string)));
        dt.Columns.Add(new DataColumn("BILL_INV_NO", typeof(string)));
        dt.Columns.Add(new DataColumn("Bill_inv_date", typeof(string)));
        dt.Columns.Add(new DataColumn("BillFor", typeof(string)));
        dt.Columns.Add(new DataColumn("JOBNO", typeof(string)));
        dt.Columns.Add(new DataColumn("IMP_EXP", typeof(string)));

        dt.Columns.Add(new DataColumn("TOTAL_AMT_TAX", typeof(string)));
        dt.Columns.Add(new DataColumn("ReceivedAmt", typeof(string)));
        dt.Columns.Add(new DataColumn("BalanceAmt", typeof(string)));
        dt.Columns.Add(new DataColumn("CurrentPayable", typeof(string)));  //txtPayAmt
        dt.Columns.Add(new DataColumn("TDS_Percent", typeof(string))); //txtTDSPercent
        dt.Columns.Add(new DataColumn("TDS_Amt",typeof(string))); //txtTDSAmt.Text
        dt.Columns.Add(new DataColumn("Remarks", typeof(string)));//txtBillRemarks
        dt.Columns.Add(new DataColumn("Iswriteoff", typeof(string)));//chkWrite
        dt.Columns.Add(new DataColumn("WriteoffAmt", typeof(string)));//txtWriteOffAmt
        dt.Rows.Add(dr);        
        ViewState["NewCurrentTable"] = dt;
        gv_Chg_Details.DataSource = dt;
        gv_Chg_Details.DataBind();  
    }
    private void AddNewRow()
    {
        int rowIndex = 0;
        if (ViewState["NewCurrentTable"] != null)
        {
            DataTable dtCurrentTable = (DataTable)ViewState["NewCurrentTable"];
            DataRow drCurrentRow = null;
            if (dtCurrentTable.Rows.Count > 0)
            {
                int Slno = 1;
                for (int i = 1; i <= dtCurrentTable.Rows.Count; i++)
                {
                    TextBox txtUser_BranchName = (TextBox)gv_Chg_Details.Rows[rowIndex].Cells[1].FindControl("txtUser_BranchName");
                    TextBox txtBill_inv_No = (TextBox)gv_Chg_Details.Rows[rowIndex].Cells[2].FindControl("txtBill_inv_No");
                    TextBox txtBill_inv_date = (TextBox)gv_Chg_Details.Rows[rowIndex].Cells[4].FindControl("txtBill_inv_date");
                    TextBox txtJOBNO = (TextBox)gv_Chg_Details.Rows[rowIndex].Cells[5].FindControl("txtJOBNO");
                    TextBox txtIMP_EXP = (TextBox)gv_Chg_Details.Rows[rowIndex].Cells[6].FindControl("txtIMP_EXP");
                    TextBox txtBillAmt = (TextBox)gv_Chg_Details.Rows[rowIndex].Cells[7].FindControl("txtBillAmt");
                    TextBox txtReceivedAmt = (TextBox)gv_Chg_Details.Rows[rowIndex].Cells[8].FindControl("txtReceivedAmt");
                    TextBox txtBalAmt = (TextBox)gv_Chg_Details.Rows[rowIndex].Cells[9].FindControl("txtBalAmt");
                    TextBox txtPayAmt = (TextBox)gv_Chg_Details.Rows[rowIndex].Cells[10].FindControl("txtPayAmt");
                    TextBox txtBillFor = (TextBox)gv_Chg_Details.Rows[rowIndex].Cells[3].FindControl("txtBill_For");

                    TextBox txtTDSPercent = (TextBox)gv_Chg_Details.Rows[rowIndex].Cells[11].FindControl("txtTDSPercent");
                    TextBox txtTDSAmt = (TextBox)gv_Chg_Details.Rows[rowIndex].Cells[12].FindControl("txtTDSAmt");
                    TextBox txtBillRemarks = (TextBox)gv_Chg_Details.Rows[rowIndex].Cells[13].FindControl("txtBillRemarks");
                    CheckBox chkWrite = (CheckBox)gv_Chg_Details.Rows[rowIndex].Cells[14].FindControl("chkWrite");
                    TextBox txtWriteOffAmt = (TextBox)gv_Chg_Details.Rows[rowIndex].Cells[15].FindControl("txtWriteOffAmt");
                    drCurrentRow = dtCurrentTable.NewRow();

                    dtCurrentTable.Rows[i - 1]["USER_BRANCHNAME"] = txtUser_BranchName.Text;
                    dtCurrentTable.Rows[i - 1]["BILL_INV_NO"] = txtBill_inv_No.Text;
                    dtCurrentTable.Rows[i - 1]["BILL_INV_DATE"] = (txtBill_inv_date.Text == string.Empty) ? DateTime.Parse(("01/01/1900")).ToString("dd/MM/yyyy") : txtBill_inv_date.Text;

                    dtCurrentTable.Rows[i - 1]["JOBNO"] = txtJOBNO.Text;
                    dtCurrentTable.Rows[i - 1][4] = txtIMP_EXP.Text;

                    dtCurrentTable.Rows[i - 1]["TOTAL_AMT_TAX"] = (txtBillAmt.Text == string.Empty) ? "0" : txtBillAmt.Text;
                    dtCurrentTable.Rows[i - 1]["ReceivedAmt"] = (txtReceivedAmt.Text == string.Empty) ? "0" : txtReceivedAmt.Text;
                    dtCurrentTable.Rows[i - 1]["BalanceAmt"] = (txtBalAmt.Text == string.Empty) ? "0" : txtBalAmt.Text;
                    dtCurrentTable.Rows[i - 1]["Currentpayable"] = (txtPayAmt.Text == string.Empty) ? "0" : txtPayAmt.Text;
                    dtCurrentTable.Rows[i - 1]["BillFor"] = txtBillFor.Text;//

                    dtCurrentTable.Rows[i - 1]["TDS_Percent"] = (txtTDSPercent.Text == string.Empty) ? "0" : txtTDSPercent.Text;
                    dtCurrentTable.Rows[i - 1]["TDS_Amt"] = (txtTDSAmt.Text == string.Empty) ? "0" : txtTDSAmt.Text;
                    dtCurrentTable.Rows[i - 1]["Remarks"] = txtBillRemarks.Text;//
                    dtCurrentTable.Rows[i - 1]["IsWriteoff"] = chkWrite.Checked; //
                    dtCurrentTable.Rows[i - 1]["WriteoffAmt"] = (txtWriteOffAmt.Text == string.Empty) ? "0" : txtWriteOffAmt.Text; 
                    rowIndex++;
                    Slno++;
                }

                int index = gv_Chg_Details.Rows.Count - 1;

                dtCurrentTable.Rows.Add(drCurrentRow);
                ViewState["NewCurrentTable"] = dtCurrentTable;

                gv_Chg_Details.DataSource = dtCurrentTable;
                gv_Chg_Details.DataBind();

                int index1 = gv_Chg_Details.Rows.Count - 1;
            }
        }
        SetPreviousData();
    }

    private void SetPreviousData()
    {
        int rowIndex = 0;
        string TempIswriteoffValue;
        if (ViewState["NewCurrentTable"] != null)
        {
            DataTable dt = (DataTable)ViewState["NewCurrentTable"];
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    CheckBox chk_inv = (CheckBox)gv_Chg_Details.Rows[rowIndex].Cells[0].FindControl("chk_inv");

                    TextBox txtUser_BranchName = (TextBox)gv_Chg_Details.Rows[rowIndex].Cells[1].FindControl("txtUser_BranchName");
                    TextBox txtBill_inv_No = (TextBox)gv_Chg_Details.Rows[rowIndex].Cells[2].FindControl("txtBill_inv_No");
                    TextBox txtBillFor = (TextBox)gv_Chg_Details.Rows[rowIndex].Cells[3].FindControl("txtBill_For");
                    TextBox txtBill_inv_date = (TextBox)gv_Chg_Details.Rows[rowIndex].Cells[4].FindControl("txtBill_inv_date");
                    TextBox txtJOBNO = (TextBox)gv_Chg_Details.Rows[rowIndex].Cells[5].FindControl("txtJOBNO");
                    TextBox txtIMP_EXP = (TextBox)gv_Chg_Details.Rows[rowIndex].Cells[6].FindControl("txtIMP_EXP");
                    TextBox txtBillAmt = (TextBox)gv_Chg_Details.Rows[rowIndex].Cells[7].FindControl("txtBillAmt");
                    TextBox txtReceivedAmt = (TextBox)gv_Chg_Details.Rows[rowIndex].Cells[8].FindControl("txtReceivedAmt");
                    TextBox txtBalAmt = (TextBox)gv_Chg_Details.Rows[rowIndex].Cells[9].FindControl("txtBalAmt");
                    TextBox txtPayAmt = (TextBox)gv_Chg_Details.Rows[rowIndex].Cells[10].FindControl("txtPayAmt");
                    TextBox txtTDSPercent = (TextBox)gv_Chg_Details.Rows[rowIndex].Cells[11].FindControl("txtTDSPercent");
                    TextBox txtTDSAmt = (TextBox)gv_Chg_Details.Rows[rowIndex].Cells[12].FindControl("txtTDSAmt");
                    TextBox txtBillRemarks = (TextBox)gv_Chg_Details.Rows[rowIndex].Cells[13].FindControl("txtBillRemarks");
                    CheckBox chkWrite = (CheckBox)gv_Chg_Details.Rows[rowIndex].Cells[14].FindControl("chkWrite");
                    TextBox txtWriteOffAmt = (TextBox)gv_Chg_Details.Rows[rowIndex].Cells[15].FindControl("txtWriteOffAmt");

                    txtUser_BranchName.Text = dt.Rows[i]["USER_BRANCHNAME"].ToString();
                    txtBill_inv_No.Text = dt.Rows[i]["BILL_INV_NO"].ToString();
                    txtBill_inv_date.Text = dt.Rows[i]["Bill_inv_date"].ToString();
                    txtJOBNO.Text = dt.Rows[i]["JOBNO"].ToString();
                    txtIMP_EXP.Text = dt.Rows[i]["IMP_EXP"].ToString();

                    txtBillAmt.Text = dt.Rows[i]["TOTAL_AMT_TAX"].ToString();
                    txtReceivedAmt.Text = dt.Rows[i]["ReceivedAmt"].ToString();
                    txtBalAmt.Text = dt.Rows[i]["BalanceAmt"].ToString();
                    txtPayAmt.Text = dt.Rows[i]["CurrentPayable"].ToString();  //
                    txtBillFor.Text = dt.Rows[i]["BillFor"].ToString();//

                    txtTDSPercent.Text = dt.Rows[i]["TDS_Percent"].ToString();

                    txtTDSAmt.Text = (dt.Rows[i]["TDS_Amt"].ToString() == "") ? "0.00" : dt.Rows[i]["TDS_Amt"].ToString();
                    txtBillRemarks.Text = dt.Rows[i]["Remarks"].ToString();
                    TempIswriteoffValue = dt.Rows[i]["Iswriteoff"].ToString();  //chkWrite
                    if (TempIswriteoffValue == "true" || TempIswriteoffValue == "True" || TempIswriteoffValue == "TRUE")
                    {
                        chkWrite.Checked=true ;
                    }
                    else
                    {
                        chkWrite.Checked =false;
                    }

                    txtWriteOffAmt.Text = dt.Rows[i]["WriteoffAmt"].ToString();

                    rowIndex++;
                }
            }
        }
    }
     
    protected void gvAll_RowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Height = 20;
        }
        if (e.Row.RowType == DataControlRowType.Header)
            e.Row.CssClass = "header";

        if (e.Row.RowType == DataControlRowType.DataRow && e.Row.RowState == DataControlRowState.Normal)
            e.Row.CssClass = "normal";

        if (e.Row.RowType == DataControlRowType.DataRow && e.Row.RowState == DataControlRowState.Alternate)
            e.Row.CssClass = "alternate";

    }
    

    public void Alert_msg(string msg)
    {
         
        string prompt = "<script>$(document).ready(function(){{jAlert('" + msg + "', 'Receipt Entry', function (r) {});}});</script>";
        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", prompt, false);
    }
    public void Alert_msg(string msg, string focus)
    {

        string prompt = "<script>$(document).ready(function(){{jAlert('" + msg + "', 'Receipt Entry', function (r) {document.getElementById('" + focus + "').focus();});}});</script>";
        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", prompt, false);
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
        if (txtDate.Text != string.Empty)
        {

                if (gv_Chg_Details.Rows.Count > 0)
                {
                    savemode = "S";
                    Receipt_Ins_Del_Upd("S", "0");
                }
                else
                {
                    Alert_msg("No Records found in Receipt grid");
                }
            if (i == 1)
            {
                Alert_msg("Saved Successfully");
                btnNew_Click1(sender, e);
            }
            else if (i == -1 || i == -2)
            {
                Alert_msg("Not Saved");

            }
            else if (i == -100)
            {
                Alert_msg("This Receipt No Alredy Exists");
                txtReceiptNo.Focus();

            }
            else
            {
                Alert_msg("Not Saved");
                btnNew_Click1(sender, e);
            }

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
            if (gv_Chg_Details.Rows.Count > 0)
            {
                savemode = "U";
                i = Receipt_Ins_Del_Upd("U", HDupdate_id.Value);
                if (i == 2)
                {
                    Alert_msg("Updated Successfully");
                    //btnNew_Click1(sender, e);
                    Update_Item_Load();
                }
                else
                {
                    Alert_msg("Not Updated");
                }
               
            }
            else
            {
                Alert_msg("No Records found in Receipt grid");
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
            
            i = Receipt_Delete("D", HDupdate_id.Value);

            if (i == 3)
            {

                btnNew_Click1(sender, e);
                Alert_msg("Deleted");
                btnrpt.Visible = false;
            }
            else
            {
                Alert_msg("Not Deleted");
            }
            btnNew_Click1(sender, e);
        }
        catch (Exception ex)
        {
            Connection.Error_Msg(ex.Message);
        }
    }

    public int Receipt_Ins_Del_Upd(string I_Name, string ReceiptID)
    {
        
        DataTable dt;
        string totReceivedAmt = "0";
        if (ddlModeofPay.SelectedValue.ToString() == "Cheque")
        {
            totReceivedAmt = txtChqAmt.Text;

            ObjUBO.Chqno = txtChqNo.Text;
            ObjUBO.ChqDt = txtChqDate.Text;
            ObjUBO.BankName = txtChqBank.Text;
            ObjUBO.BankBranch = txtChqBranch.Text;
        }

        else if (ddlModeofPay.SelectedValue.ToString() == "Cash")
            totReceivedAmt = txtChqAmt.Text;
        else if (ddlModeofPay.SelectedValue.ToString() == "Net Banking")
        {
            totReceivedAmt = txtChqAmt.Text;
            ObjUBO.Chqno = txtChqNo.Text;
            ObjUBO.ChqDt = txtChqDate.Text;
            ObjUBO.BankName = txtChqBank.Text;
            ObjUBO.BankBranch = txtChqBranch.Text;
        }
        object totTDSAmt = "0";
        object RecAmt;
        ConvertGrid_Datatable(gv_Chg_Details);
        dt = (DataTable)ViewState["NewCurrentTable"];// new code
        if (dt.Rows.Count > 0)
        {
                if (I_Name == "S")
                {
                    ObjUBO.ID = ReceiptID;
                    ObjUBO.Flag = "S";
                }
              
                else if (I_Name == "U")
                {
                    ObjUBO.ID = HDupdate_id.Value;
                    ObjUBO.Flag = "U";
                }


                ObjUBO.ExpVoucherNo = txtReceiptNo.Text;
                ObjUBO.BILL_INV_DATE = txtDate.Text;  // Receipt voucher Date
                ObjUBO.IMPORTER_NAME = ddlCustomer.SelectedValue.ToString();  // customer name
                
                ObjUBO.BILL_TYPE = Rd_Job_Type.SelectedValue.ToString(); // imp/exp job 

                if (ddlPayFor.SelectedIndex == 0)
                    ObjUBO.TYPE_OF_INVOICE = "Job";
                else
                    ObjUBO.TYPE_OF_INVOICE = "Advance";

                ObjUBO.ModeOfPay = ddlModeofPay.SelectedValue.ToString(); //PaymentMode
                ObjUBO.TOTAL = totReceivedAmt;
                ObjUBO.TOTAL_TAX_AMT = totTDSAmt.ToString();
                ObjUBO.REMARKS = txtCommNarration.Text;
                ObjUBO.BankReceiptNo = txtBankReceiptNo.Text;
                ObjUBO.PaymentFor = ddlPayFor.SelectedValue;
                ObjUBO.PaymentFrom = ddlPaymentfrom.SelectedValue;
                ObjUBO.CustBranch = ddlbranch_No.SelectedValue;
                ObjUBO.DepositedBank = ddlDepositInBank.SelectedValue;
                ObjUBO.WORKING_PERIOD = ddlWorkingPeriod.SelectedValue;
                ObjUBO.BRANCH_CODE = Connection.Current_Branch();

                BI.ReceiptEntryIDU(ObjUBO, dt);
                i = BI.result;
                return i;
        }
        else
        {
            i = -1;
            return i;
        }
    }

    public int Receipt_Delete(string I_Name, string ReceiptID)
    {
            DataTable dt = (DataTable)ViewState["CurrentTable"];
            ObjUBO.ID = HDupdate_id.Value;
            ObjUBO.Flag = "D";

            BI.ReceiptEntryIDU(ObjUBO, dt);
            i = BI.result;
            return i;
      
    }

    private void Load_Bank_Name(string ddlbank)
    {
        try
        {
            DataSet dss = new DataSet();
            ddlDepositInBank.Items.Clear();
            ObjUBO.Flag = "Select_Bank";
            ObjUBO.BILL_TYPE = ddlbank;
            dss = BI.Load_DDL(ObjUBO);
            if (dss.Tables[0].Rows.Count > 0)
            {
                ddlDepositInBank.DataSource = dss.Tables[0];
                ddlDepositInBank.DataTextField = "BANK_NAME";
                ddlDepositInBank.DataValueField = "BANK_NAME";
                ddlDepositInBank.DataBind();
            }
            ddlDepositInBank.Items.Insert(0, new ListItem("--Select--", "--Select--"));
            ddlDepositInBank.SelectedIndex = 0;

            dss.Dispose();
            GC.Collect();
        }
        catch (Exception ex)
        {
            Connection.Error_Msg(ex.Message);
        }
    }
    private void load_Imp_name()
    {
        try
        {           
            DataSet dss = new DataSet();           
               
                ddlbranch_No.Items.Clear();
                ddlCustomer.Items.Clear();
                    ObjUBO.Flag = "Select-ImpExpBillingCustomer";

                if (Connection.Billing_Working_Period() == "Y" && HDupdate_id.Value == "" )
                {
                   
                    dss = Database_Con_string();
                }
                else
                {
                    dss = BI.Load_DDL(ObjUBO);
                }


                if (dss.Tables[0].Rows.Count > 0)
                {
                    ddlCustomer.DataSource = dss.Tables[0];
                    ddlCustomer.DataTextField = "Cust_name";
                    ddlCustomer.DataValueField = "Cust_name";
                    ddlCustomer.DataBind();
                }
                ddlCustomer.Items.Insert(0, new ListItem("--Select--", "--Select--"));
                ddlCustomer.SelectedIndex = 0;

                dss.Dispose();
                GC.Collect();
        }
        catch (Exception ex)
        {
            Connection.Error_Msg(ex.Message);
        }
    }
    private void load_Jobno()
    {
        try
        {
            DataSet dss = new DataSet();
            ObjUBO.Flag = "Imp_Job_Select";
            //-----------start-----------------
            if (COMPANY_LICENSE == "eri00247" )
            {
                DataSet ds = new DataSet();
                if (Session["COMPANY_DETAILS_DS"] != null)
                {
                    ds = (DataSet)Session["COMPANY_DETAILS_DS"];

                    DataTable ds3 = new DataTable();
                    ds3 = ds.Tables[2];
                    DataView view1 = ds3.DefaultView;
                    string a = ddlWorkingPeriod.SelectedValue.ToString();
                    view1.RowFilter = "WORKING_PERIOD = '" + a + "'";
                    DataTable table1 = view1.ToTable();
                    //-----------------------------------------------------------------//
                    dss = BI.Select_IMP_INV(ObjUBO, table1.Rows[0]["SERVER_NAME"].ToString());
                   
                }
            }
            else
            {
                dss = BI.Select_IMP_INV(ObjUBO);
            }
            //-----------end---------------------
            if (dss.Tables[0].Rows.Count > 0)
            {
                 
            }
            if (dss.Tables[1].Rows.Count > 0)
            {
                ddlbranch_No.DataSource = dss.Tables[1];
                ddlbranch_No.DataTextField = "ADDRESS3";
                ddlbranch_No.DataValueField = "BRANCH_NO";
                ddlbranch_No.DataBind();
            }
            ddlbranch_No.Items.Insert(0, new ListItem(String.Empty, String.Empty));
            ddlbranch_No.SelectedIndex = 0;

            dss.Dispose();
            GC.Collect();
        }
        catch (Exception ex)
        {
            Connection.Error_Msg(ex.Message);
        }
    }

    protected void gv_Chg_Details_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            if (gv_Chg_Details.Rows.Count > 1)
            {
                ConvertGrid_Datatable(gv_Chg_Details);
                DataTable dt = (DataTable)ViewState["CurrentTable"];
                int rowIndex = Convert.ToInt32(e.RowIndex);

                dt.Rows.Remove(dt.Rows[rowIndex]);
                ViewState["CurrentTable"] = dt;

                gv_Chg_Details.DataSource = dt;
                gv_Chg_Details.DataBind();

            }
            else if (gv_Chg_Details.Rows.Count == 1)
            {
            }
        }

        catch (Exception ex)
        {
            Connection.Error_Msg(ex.Message);
        }

    }
    private void ConvertGrid_Datatable(GridView gg )
    {
        try
        {
        int cnt = 0;
        DataTable dt;
        DataRow dr;
        int R = 1;
        dt = (DataTable)ViewState["NewCurrentTable"];// old code
            
        if (dt.Rows.Count > 0)
        {
            dt.Rows.Clear();
        }
        foreach (GridViewRow row in gg.Rows)
        {
            CheckBox chk = (CheckBox)row.FindControl("chk_inv");
            if (chk.Checked == true)
            {
                TextBox txtPayAmt1 = (TextBox)row.FindControl("txtPayAmt");
                TextBox txtTDSPercent1 = (TextBox)row.FindControl("txtTDSPercent");                
                TextBox txtTDSAmt1 = (TextBox)row.FindControl("txtTDSAmt");
                TextBox txtRemarks1 = (TextBox)row.FindControl("txtBillRemarks");
                TextBox txtBalAmt1 = (TextBox)row.FindControl("txtBalAmt");
                CheckBox chkwrite1 = (CheckBox)row.FindControl("chkWrite");
                TextBox txtWriteOffAmt1 = (TextBox)row.FindControl("txtWriteOffAmt");

                TextBox txtJOBNO = (TextBox)row.FindControl("txtJOBNO");
                TextBox txtBill_inv_No = (TextBox)row.FindControl("txtBill_inv_No");
                TextBox txtIMP_EXP = (TextBox)row.FindControl("txtIMP_EXP");
                TextBox txtUser_BranchName = (TextBox)row.FindControl("txtUser_BranchName");
                TextBox txtReceivedAmt = (TextBox)row.FindControl("txtReceivedAmt");
                TextBox txtBillFor = (TextBox)row.FindControl("txtBill_For");
                cnt = 1;

                if (txtPayAmt1.Text.Trim() == "" || txtPayAmt1.Text.Trim() == string.Empty)
                {
                    txtPayAmt1.Text = "0";
                }
                if (txtBalAmt1.Text.Trim() == "" || txtBalAmt1.Text.Trim() == string.Empty)
                {
                    txtBalAmt1.Text = "0";
                }
                if (txtWriteOffAmt1.Text.Trim() == "" || txtWriteOffAmt1.Text.Trim() == string.Empty)
                {
                    txtWriteOffAmt1.Text = "0";
                }

                if (Convert.ToDecimal(txtPayAmt1.Text) > Convert.ToDecimal(txtBalAmt1.Text))
                {
                    if (ddlPayFor.SelectedIndex == 0)
                    {
                        Alert_msg("Current Receivable should less than balance amount ");
                        return;
                    }
                    else
                    {
                    }
                }
                dr = dt.NewRow();
                dr["BalanceAmt"] = R;
                dr["BILL_INV_NO"] = txtBill_inv_No.Text;  
                dr["BillFor"] = txtBillFor.Text;
                dr["JOBNO"] = txtJOBNO.Text.Trim(); // job no from grid column 4   //3
                dr["Imp_Exp"] = txtIMP_EXP.Text.Trim();
                dr["ReceivedAmt"] = txtPayAmt1.Text.Trim(); // TDS no
                dr["TDS_Percent"] = (txtTDSPercent1.Text == "") ? "0" : txtTDSPercent1.Text;   
                dr["TDS_Amt"] = (txtTDSAmt1.Text == "") ? "0" : txtTDSAmt1.Text;
                dr["Remarks"] = txtRemarks1.Text; 
                dr["IsWriteoff"] = (chkwrite1.Checked == true) ? "Y" : "N"; 
                dr["WriteoffAmt"] = txtWriteOffAmt1.Text; 
                dr["USER_BRANCHNAME"] = txtUser_BranchName.Text; 
                dt.Rows.Add(dr);
                R = R + 1;
            }

        }
        DataView dv = new DataView(dt);
        if (savemode == "S")
        {
            dt = dv.ToTable("selected", false, "BalanceAmt", "USER_BRANCHNAME", "ReceivedAmt", "JOBNO", "BillFor", "Bill_Inv_No", "IMP_EXP", "TDS_Percent", "TDS_AMT", "Remarks", "Iswriteoff", "WriteoffAmt");
        }
        else
        {
            dt = dv.ToTable("selected", false, "BalanceAmt", "USER_BRANCHNAME", "ReceivedAmt", "JOBNO", "BillFor", "Bill_Inv_No", "IMP_EXP", "TDS_Percent", "TDS_AMT", "Remarks", "Iswriteoff", "WriteoffAmt");
        }
        if (cnt == 1)
        {
            ViewState["NewCurrentTable"] = dt; //New code
        }
        else
        {
            Alert_msg("Select atleast one Bill");          
        }
        }
        catch (Exception ex)
        {
            Connection.Error_Msg(ex.Message);
        }
    }
    protected void btnNew_Click1(object sender, EventArgs e)
    {
        try
        {
            Transaction trans = new Transaction();
            trans.ResetFields(Page.Controls);
            btnUpdate.CssClass = "updates";
            btnSave.Visible = true;
            btnDelete.Visible = false;
            btnUpdate.Visible = false;

            DataTable dt = new DataTable();
            gv_Chg_Details.DataSource = dt;
            gv_Chg_Details.DataBind();

            gvReceiptdetails.DataSource = dt;
            gvReceiptdetails.DataBind();

            HDupdate_id.Value = string.Empty;
            btnrpt.Visible = false;
            ddlbranch_No.Enabled = true;
            ddlbranch_No.Enabled = true;
            ddlbranch_No.Attributes.Remove("readonly");
            ddlbranch_No.Style.Add("background-color", "white");
            WK.Visible = true;
            txtDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
            PnlCash.Enabled = false;
            PnlCheque.Enabled = false;
            PnlNetBank.Enabled = false;
            GetAutono_Receipt();
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
    protected void Rd_Job_Type_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
        DataTable dt = new DataTable();
        gv_Chg_Details.DataSource = dt;
        gv_Chg_Details.DataBind();
 
        load_Imp_name();

        dt.Dispose();
        GC.Collect();
        }
        catch (Exception ex)
        {
            Connection.Error_Msg(ex.Message);
        }
    }
    protected void ddlWorkingPeriod_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
        DataTable dt = new DataTable();
        gv_Chg_Details.DataSource = dt;
        gv_Chg_Details.DataBind();

        load_Imp_name();
        dt.Dispose();
        GC.Collect();
        }
        catch (Exception ex)
        {
            Connection.Error_Msg(ex.Message);
        }
    }
    public void LoadFinancialYear()
    {
        SqlCommand cmd;
        SqlConnection con;
        SqlDataReader dr;

        con = new SqlConnection(System.Configuration.ConfigurationManager.AppSettings["con"]);
        try
        {
            con.Open();
            ddlWorkingPeriod.Items.Clear();
            cmd = new SqlCommand("select * from FINANCIAL_YEAR order by WORKING_PERIOD desc", con);
            dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                ddlWorkingPeriod.Items.Add(dr["WORKING_PERIOD"].ToString());
            }
            dr.Close();
            con.Close();
        }
        catch (Exception ex)
        {
            Connection.Error_Msg(ex.Message);
        }
        finally
        {
            con.Close();
        }
    }

    protected void ddlbranch_No_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (ddlbranch_No.SelectedItem.Text != string.Empty)
            {
                LoadPendingJob();
            }
        }
        catch (Exception ex)
        {
            Connection.Error_Msg(ex.Message);
        }
    }

    private void load_Branch_Jobno()
    {
        try
        {
            DataSet dss = new DataSet();
            ObjUBO.JOBNO = ddlbranch_No.SelectedValue.ToString();
            ObjUBO.Flag = "Imp_Branch_Job_Select";
            //-----------start-----------------
            if (COMPANY_LICENSE == "eri00247")
            {
                DataSet ds = new DataSet();
                if (Session["COMPANY_DETAILS_DS"] != null)
                {
                    ds = (DataSet)Session["COMPANY_DETAILS_DS"];

                    DataTable ds3 = new DataTable();
                    ds3 = ds.Tables[2];
                    DataView view1 = ds3.DefaultView;
                    string a = ddlWorkingPeriod.SelectedValue.ToString();
                    view1.RowFilter = "WORKING_PERIOD = '" + a + "'";
                    DataTable table1 = view1.ToTable();
                    //-----------------------------------------------------------------//
                    dss = BI.Select_IMP_INV(ObjUBO, table1.Rows[0]["SERVER_NAME"].ToString());

                }
            }
            else
            {
                dss = BI.Select_IMP_INV(ObjUBO);
            }
            //-----------end---------------------
            if (dss.Tables[0].Rows.Count > 0)
            {
                 
            }
            dss.Dispose();
            GC.Collect();
             
        }
        catch (Exception ex)
        {
            Connection.Error_Msg(ex.Message);
        }
    }
    protected void ddlCustomer_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
        DataTable dt =new DataTable();
        gv_Chg_Details.DataSource = dt;
        gv_Chg_Details.DataBind();
        LoadBranch();
        FirstRow();  // for temp purpose
        }
        catch (Exception ex)
        {
            Connection.Error_Msg(ex.Message);
        }
        
    }

    private void LoadBranch()
    {
        try
        {
            DataSet ds = new DataSet();

            ddlbranch_No.Items.Clear();
            ObjUBO.Flag = "Select-ImpExpBranch-CustomerBased";
            ObjUBO.Ref_ID = ddlCustomer.SelectedValue.ToString();

            if (Connection.Billing_Working_Period() == "Y" &&   HDupdate_id.Value == "" )
            {

                ds = Database_Con_string();
            }
            else
            {
                ds = BI.Load_DDL(ObjUBO);
            }
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlbranch_No.DataSource = ds.Tables[0];
                ddlbranch_No.DataTextField = "BRANCH_CODE";
                ddlbranch_No.DataValueField = "BRANCH_CODE";
                ddlbranch_No.DataBind();
            }
            ddlbranch_No.Items.Insert(0, new ListItem("All", "All"));
            ddlbranch_No.SelectedIndex = 0;

            ds.Dispose();
            GC.Collect();
        }
        catch (Exception ex)
        {
            Connection.Error_Msg(ex.Message);
        }

    }
    protected void ddlModeofPay_SelectedIndexChanged(object sender, EventArgs e)
    {
       
    }
    private void LoadPayDiv_inSinglePanel()
    {
        try
        {
            if (ddlModeofPay.SelectedValue.ToString() == "Cheque")
            {

                PnlNetBank.Enabled = false;
                PnlCheque.Enabled = true;
                PnlCash.Enabled = false;

                txtNetBank.Text = "";
                txtNetBranch.Text = "";
                txtNetAmt.Text = "";
                txtNetRefno.Text = "";
                txtNetRefDt.Text = "";

                txtChqNo.Enabled = true;
                txtChqDate.Enabled = true;
                txtChqAmt.Enabled = true;
                txtChqBank.Enabled = true;
                txtChqBranch.Enabled = true;
            }
            else if (ddlModeofPay.SelectedValue.ToString() == "Cash")
            {

                PnlNetBank.Enabled = false;
                PnlCheque.Enabled = true;
                PnlCash.Enabled = false;

                txtNetBank.Text = "";
                txtNetBranch.Text = "";
                txtNetAmt.Text = "";
                txtChqNo.Text = "";
                txtChqDate.Text = "";
                txtChqBank.Text = "";
                txtChqBranch.Text = "";
                txtNetRefno.Text = "";
                txtNetRefDt.Text = "";

                txtChqNo.Enabled = false;
                txtChqDate.Enabled = false;
                txtChqAmt.Enabled = true;
                txtChqBank.Enabled = false;
                txtChqBranch.Enabled = false;
            }
            else if (ddlModeofPay.SelectedValue.ToString() == "Net Banking")
            {

                PnlNetBank.Enabled = false;
                PnlCheque.Enabled = true;
                PnlCash.Enabled = false;
                txtChqNo.Enabled = true;
                txtChqDate.Enabled = true;
                txtChqAmt.Enabled = true;
                txtChqBank.Enabled = true;
                txtChqBranch.Enabled = true;

            }
            else
            {
                PnlCheque.Enabled = false;
            }

        }
        catch (Exception ex)
        {
            Connection.Error_Msg(ex.Message);
        }
    }
    private DataSet Database_Con_string()
    {
        DataSet ds = new DataSet();
        DataSet dss = new DataSet();
        if (Session["COMPANY_DETAILS_DS"] != null)
        {
            ds = (DataSet)Session["COMPANY_DETAILS_DS"];

            DataTable ds3 = new DataTable();
            ds3 = ds.Tables[2];
            DataView view1 = ds3.DefaultView;
            string a = Connection.WorkingPeriod();
            view1.RowFilter = "WORKING_PERIOD = '" + a + "'";
            DataTable table1 = view1.ToTable();
            dss = BI.Load_DDL(ObjUBO, table1.Rows[0]["SERVER_NAME"].ToString());
        }
        return dss;
    }

    [System.Web.Services.WebMethod]
    public static IList<string> LoadPendingJob_webmethod(string mail, string TempRd_Job_Type, string Tempddlbranch_No, string TempddlCustomer)    
    {
        DataSet ds = new DataSet();
        DataTable dt1 = new DataTable();
        DataTable dt =new DataTable();
        IList<string> result = new List<string>();
        int i=0;
        string finder=mail;
        string ReturnStr;
        try
        {
            GST_Imp_Invoice BI = new GST_Imp_Invoice();
            Billing_UserBO ObjUBO = new Billing_UserBO();

            if (TempRd_Job_Type == "Imp")
                ObjUBO.Flag = "Select-Imp-CustomerPendingBills";
            else if (TempRd_Job_Type== "Exp")
                ObjUBO.Flag = "Select-Exp-CustomerPendingBills";
            else if (TempRd_Job_Type== "Both")
                ObjUBO.Flag = "Select-ImpExp-CustomerPendingBills";
            ObjUBO.BRANCH_CODE = Tempddlbranch_No;
            ObjUBO.Ref_ID = TempddlCustomer;

            ds = BI.Load_DDL(ObjUBO);
            dt1 = ds.Tables[0];
            DataView dv = new DataView(dt1);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                ReturnStr=ds.Tables[0].Rows[i]["BILL_INV_NO"].ToString();
                if (ReturnStr.Contains(finder))
                {
                    result.Add(ds.Tables[0].Rows[i]["BILL_INV_NO"].ToString());
                }                
                i++;
            }
           
        }
        catch (Exception ex)
        {
            Connection.Error_Msg(ex.Message);

        }
        return result;
    }

    [System.Web.Services.WebMethod]
    public static string Bill_Details_Pickup_webmethod(string mail, string TempRd_Job_Type, string Tempddlbranch_No, string TempddlCustomer)
    {
        DataSet ds = new DataSet();
        DataTable dt1 = new DataTable();
        DataTable dt = new DataTable();
        IList<string> result = new List<string>();
        int i = 0;
        string BillInvNo = mail;
        string ReturnStr = "", ReturnStrMore="";
        try
        {
            ReceiptEntry_cs BI = new ReceiptEntry_cs();
            Billing_UserBO ObjUBO = new Billing_UserBO();

            if (TempRd_Job_Type == "Imp")
                ObjUBO.Flag = "Select-Imp-CustomerPendingBills";
            else if (TempRd_Job_Type == "Exp")
                ObjUBO.Flag = "Select-Exp-CustomerPendingBills";
            else if (TempRd_Job_Type == "Both")
                ObjUBO.Flag = "Select-ImpExp-CustomerPendingBills";
            ObjUBO.BRANCH_CODE = Tempddlbranch_No;
            ObjUBO.Ref_ID = TempddlCustomer;

            ds = BI.Load_DDL(ObjUBO);
            dt1 = ds.Tables[0];
            DataView dv = new DataView(dt1);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                ReturnStr = ds.Tables[0].Rows[i]["BILL_INV_NO"].ToString();
                if (ReturnStr==BillInvNo)
                {
                    ReturnStrMore = ds.Tables[0].Rows[i]["BILL_INV_NO"].ToString() + "~~" + ds.Tables[0].Rows[i]["BILL_INV_DATE"].ToString() + "~~" + ds.Tables[0].Rows[i]["JOBNO"].ToString() + "~~" + ds.Tables[0].Rows[i]["IMP_EXP"].ToString() + "~~" + ds.Tables[0].Rows[i]["TOTAL_AMT_TAX"].ToString() + "~~" + ds.Tables[0].Rows[i]["RECEIVEDAMT"].ToString() + "~~" + ds.Tables[0].Rows[i]["BALANCEAMT"].ToString();
                }
                i++;
            }
        }
        catch (Exception ex)
        {
            Connection.Error_Msg(ex.Message);
        }
        return ReturnStrMore;
    }
    protected void gv_Chg_Details_RowDeleting(object sender, CommandEventArgs e)
    {
        if (ViewState["CurrentTable"] != null)
        {
            DataTable dt = (DataTable)ViewState["CurrentTable"];
            DataRow drCurrentRow = null;
            int rowIndex = Convert.ToInt32(e.CommandArgument);
            if (dt.Rows.Count > 1)
            {
                dt.Rows.Remove(dt.Rows[rowIndex]);
                drCurrentRow = dt.NewRow();
                ViewState["CurrentTable"] = dt;
                gv_Chg_Details.DataSource = dt;
                gv_Chg_Details.DataBind();

                for (int i = 0; i <= gv_Chg_Details.Rows.Count - 1; i++)
                {
                    gv_Chg_Details.Rows[i].Cells[0].Text = Convert.ToString(i + 1);
                }
            }
        }
        
    }

    protected void gv_Chg_Details_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.Equals("add"))
            {
                AddNewRow();
            }

            if (e.CommandName == "DeleteRow")
            {
                if (gv_Chg_Details.Rows.Count > 1)
                {
                    DataTable dt = (DataTable)ViewState["NewCurrentTable"];
                    DataRow drCurrentRow = null;
                    int rowIndex = Convert.ToInt32(e.CommandArgument);
                    if (dt.Rows.Count > 1)
                    {
                        if (dt.Columns.Contains("PAYMENT_SubID"))
                        {
                            string apay = dt.Rows[rowIndex]["PAYMENT_SubID"].ToString();
                            if (apay == "" || apay == "0")
                            {
                                dt.Rows.Remove(dt.Rows[rowIndex]);
                                drCurrentRow = dt.NewRow();
                                ViewState["NewCurrentTable"] = dt;
                                gv_Chg_Details.DataSource = dt;
                                gv_Chg_Details.DataBind();
                            }
                            else
                            {
                                i = PAYMENT_Delete("DELETE_SUB", apay);

                                if (i == 1)
                                {

                                    btnNew_Click1(sender, e);
                                    Alert_msg("Deleted");
                                    btnrpt.Visible = false;
                                }
                                else
                                {
                                    Alert_msg("Not Deleted");
                                }
                            }

                        }
                        else
                        {
                            dt.Rows.Remove(dt.Rows[rowIndex]);
                            drCurrentRow = dt.NewRow();
                            ViewState["NewCurrentTable"] = dt;
                            gv_Chg_Details.DataSource = dt;
                            gv_Chg_Details.DataBind();
                        }

                    }
                    SetPreviousData();    
                }
                else if (gv_Chg_Details.Rows.Count == 1)
                {
                }

            }
        }
        catch (Exception ex)
        {
            Connection.Error_Msg(ex.Message);
        }
    }
    public int PAYMENT_Delete(string I_Name, string PaymentID)
    {
        DataTable dt = (DataTable)ViewState["NewCurrentTable"];
        ObjUBO.ID = HDupdate_id.Value;
        ObjUBO.Flag = I_Name;

        BI.ReceiptEntryIDU(ObjUBO, dt);
        i = BI.result;
        return i;

    }

    protected void ddlDepositInBank_SelectedIndexChanged(object sender, EventArgs e)
    {
        DataSet dss = new DataSet();
        ObjUBO.Flag = "Select_Bank";
        ObjUBO.BILL_TYPE = ddlDepositInBank.SelectedValue;
        dss = BI.Load_DDL(ObjUBO);
        txtBankReceiptNo.Text = dss.Tables[0].Rows[0][17].ToString() + "/" +dss.Tables[1].Rows[0][0].ToString()+"/"+ dss.Tables[0].Rows[0][18].ToString();
    }
}