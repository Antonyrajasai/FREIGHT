using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using BussinessObject;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using System.Text;
using System.Data.SqlClient;
using System.Configuration;

public partial class PaymentEntry : ThemeClass
{
    public string currentuser;
    public string currentbranch;
    public string Working_Period;

    //public string COMPANY_LICENSE;

    AppSession aps = new AppSession();
    User_Creation user_Create = new User_Creation();
    
    PaymentEntry_cs BI = new PaymentEntry_cs();
    Billing_UserBO ObjUBO = new Billing_UserBO();

    public int i, Screen_Id;
    public string COMPANY_LICENSE;
    public string SCREEN_ID, PAGE_MODIFY, PAGE_DELETE;
    public string PAGE_WRITE, PAGE_READ, COMPANY_ID, Screen_IdNew;
    protected void Page_Load(object sender, EventArgs e)
    {
        CalendarExtender1.EndDate = DateTime.Now;
        DataSet ds = new DataSet();
        aps.checkSession();
        currentuser = Session["currentuser"].ToString();
        COMPANY_LICENSE = Connection.Company_License().ToLower();
        currentbranch = Session["currentbranch"].ToString();
        Working_Period = Session["WorkingPeriod"].ToString();
      
        try
        {

            Screen_Id = 102;
            Page_Rights(Screen_Id);
             if (!Page.IsPostBack)
            {
                load_Imp_name();                
                btnSave.Visible = true;
                btnUpdate.Visible = false;
                btnDelete.Visible = false;

                btnrpt.Visible = false;
                T.Visible = false;

                WK.Visible = true;
                //LoadFinancialYear();
                ddlWorkingPeriod.SelectedValue = Connection.WorkingPeriod();

                if (Request.QueryString["Page"] == null)
                {
                    txtDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    GetAutono_Payment(); 
                    load_Imp_name();
                    
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

             try
             {
                 if (Session["USER_DETAILS_DS"] != null)
                 {
                     if (Session["JOB_TYPE"] != null)
                     {
                         ds = (DataSet)Session["USER_DETAILS_DS"];
                     }

                     DataView view1 = ds.Tables[1].DefaultView;
                     string USER_CREATION_ID, BRANCH;
                     COMPANY_ID = Session["COMPANY_ID"].ToString();
                     USER_CREATION_ID = Session["USER_CREATION_ID"].ToString();
                     BRANCH = Session["currentbranch"].ToString();

                     view1.RowFilter = "BRANCH = '" + BRANCH + "' and " + "COMPANY_ID = " + COMPANY_ID + " and " + "USER_CREATION_ID = " + USER_CREATION_ID + "";
                     DataTable table1 = view1.ToTable();
                     DataSet dt_UserRights = new DataSet();
                     dt_UserRights.Tables.Add(table1);
                     if (dt_UserRights.Tables[0].Rows.Count > 0)
                     {
                         for (int a = 0; a < dt_UserRights.Tables[0].Rows.Count; a++)
                         {
                             PAGE_READ = dt_UserRights.Tables[0].Rows[a]["PAGE_READ"].ToString();
                             PAGE_WRITE = dt_UserRights.Tables[0].Rows[a]["PAGE_WRITE"].ToString();
                             PAGE_MODIFY = dt_UserRights.Tables[0].Rows[a]["PAGE_MODIFY"].ToString();
                             PAGE_DELETE = dt_UserRights.Tables[0].Rows[a]["PAGE_DELETE"].ToString();
                             Screen_IdNew = dt_UserRights.Tables[0].Rows[a]["SCREEN_ID"].ToString();

                             Page_Show_Hide(Convert.ToInt32(Screen_IdNew));
                         }
                     }
                     else
                     {
                         Screen_IdNew = "0";
                         Page_Show_Hide(Convert.ToInt32(Screen_IdNew));
                     }
                 }
             }
             catch (Exception ex)
             {
                 Connection.Error_Msg(ex.Message);
             }

        } 
        catch (Exception ex)
        {
            Connection.Error_Msg(ex.Message);
        }
    }


    public void Page_Show_Hide(int i)
    {
        bool is_page;
        if (i == 102)
        {
            is_page = Page_Rights();
            if (is_page == true)
            {

            }
            else
            {

            }
        }
    }
    public bool Page_Rights()
    {
        bool Is_Page;
        if (PAGE_READ == "False" && PAGE_WRITE == "False" && PAGE_MODIFY == "False" && PAGE_DELETE == "False")
        {
            Is_Page = false;
        }
        else
        {
            Is_Page = true;
        }
        return Is_Page;
    }
    public void Page_Rights(int i)
    {
        user_Create.RetrieveAll_User_Screen_Rights_Details(i);
        PAGE_WRITE = user_Create.PAGE_WRITE;
        PAGE_READ = user_Create.PAGE_READ;
        PAGE_MODIFY = user_Create.PAGE_MODIFY;
        PAGE_DELETE = user_Create.PAGE_DELETE;

        //---------------READ ONLY------------------//

        if (PAGE_READ == "False")
        {
            btnSave.Enabled = false;
            btnUpdate.Enabled = false;
            btnDelete.Enabled = false;


            btnSave.ToolTip = "You don't have permission to access";
            btnUpdate.ToolTip = "You don't have permission to access";
            btnDelete.ToolTip = "You don't have permission to access";

        }

        if (PAGE_WRITE == "True")
        {
            btnSave.Enabled = true;
            btnSave.ToolTip = "";
        }
        else
        {
            btnSave.Enabled = false;
            btnSave.ToolTip = "You don't have permission to access";


        }

        if (PAGE_MODIFY == "True")
        {
            btnUpdate.Enabled = true;
            btnUpdate.ToolTip = "";

        }
        else
        {
            btnUpdate.Enabled = false;

            btnUpdate.ToolTip = "You don't have permission to access";

        }
        if (PAGE_DELETE == "True")
        {
            btnDelete.Enabled = true;

            btnDelete.ToolTip = "";

        }
        else
        {
            btnDelete.Enabled = false;
            btnDelete.ToolTip = "You don't have permission to access";

        }

    }
    private void GetAutono_Payment()
    {
        
        try
        {

            DataSet ds = new DataSet();
            ObjUBO.Flag = "Generate-AutoNo";
            ds = BI.PaymentEntryIDU_NEW(ObjUBO);
            if (ds.Tables[0].Rows.Count > 0)
            {
                txtPaymentNo.Text = ds.Tables[0].Rows[0][0].ToString();
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
            ObjUBO.Flag = "Select-ImpExp-VendorPendingBills";
            ObjUBO.BRANCH_CODE = ddlbranch_No.SelectedValue.ToString();
            ObjUBO.Ref_ID = ddlCustomer.SelectedValue.ToString();
            ds = BI.Load_DDL_PAYMENTENTRY(ObjUBO);
            if (ds.Tables[0].Rows.Count > 0)
            {
                ViewState["PendingBillList"] = ds.Tables[0]; 
                gv_Chg_Details.DataSource = ds.Tables[0];
                gv_Chg_Details.DataBind();
            }
            else
            {
                Alert_msg("No Pending Bills, Choose Advance Payment Mode");
            }
          
            ViewState["CurrentTable"] = ds.Tables[1];
            if (ds.Tables[2].Rows.Count > 0)
            {
                gvPaymentdetails.DataSource = ds.Tables[2];
                gvPaymentdetails.DataBind();

                ViewState["PaymentHistory"] = ds.Tables[2];
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

    protected void gvPaymentdetails_RowDataBound(object sender, GridViewRowEventArgs e)
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
                  DataView dv = new DataView((DataTable)ViewState["PaymentHistory"]);
                  dv.RowFilter = "BillInvNo='" + bill + "' "; 
                  DataTable dtv = dv.ToTable();

                  gvPaymentdetails.DataSource = dtv;
                  gvPaymentdetails.DataBind();

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
            //ObjUBO.BRANCH_CODE = Connection.Current_Branch();
            //ObjUBO.WORKING_PERIOD = Connection.WorkingPeriod();
            FirstRow();
            DataSet dss = new DataSet();
            ObjUBO.MODE = "Select PaymentEntry";
            ObjUBO.Flag = "Select-PaymentSingle";
            ObjUBO.FILE_REF_NO = Request.QueryString["Page"].ToString();
            HDupdate_id.Value = Request.QueryString["Page"].ToString();
            dss = BI.Exp_Pay_Search(ObjUBO);

            if (dss.Tables[0].Rows.Count > 0)
            {
                HDupdate_id.Value = dss.Tables[0].Rows[0]["PAYMENT_ID"].ToString();
                txtPaymentNo.Text = dss.Tables[0].Rows[0]["PAYMENT_VoucherNo"].ToString();
                ddlModeofPay.SelectedValue = dss.Tables[0].Rows[0]["PaymentMode"].ToString();
                txtCommNarration.Text = dss.Tables[0].Rows[0]["Narration"].ToString();
                txtDate.Text = dss.Tables[0].Rows[0]["PAYMENT_Date"].ToString();

                if (dss.Tables[0].Rows[0]["imp_exp"].ToString() == "Imp")
                {
                    Rd_Job_Type.SelectedIndex = 0;
                    load_Imp_name();
                }
                else if (dss.Tables[0].Rows[0]["imp_exp"].ToString() == "Exp")
                {
                    Rd_Job_Type.SelectedIndex = 1;
                    load_Imp_name();
                }
                else if (dss.Tables[0].Rows[0]["imp_exp"].ToString() == "Both")
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
                    txtChqAmt.Text = dss.Tables[0].Rows[0]["TotalPAYMENTAmt"].ToString();
                }
                else if (ddlModeofPay.SelectedValue.ToString() == "Cash")
                {
                    txtChqAmt.Text = dss.Tables[0].Rows[0]["TotalPAYMENTAmt"].ToString();                    
                }
                else if (ddlModeofPay.SelectedValue.ToString() == "Net Banking")
                {
                     
                    txtChqAmt.Text = dss.Tables[0].Rows[0]["TotalPAYMENTAmt"].ToString();
                    txtChqBank.Text = dss.Tables[0].Rows[0]["Bank"].ToString();
                    txtChqBranch.Text = dss.Tables[0].Rows[0]["Branch"].ToString();
                    txtChqNo.Text = dss.Tables[0].Rows[0]["Chq_Net_RefNo"].ToString();
                    txtChqDate.Text = dss.Tables[0].Rows[0]["Chq_Net_Date"].ToString();
                }

                LoadBranch();

                if (dss.Tables[0].Rows[0]["IsJob_Advance"].ToString() == "Job")
                {
                    RdPayFor.SelectedIndex = 0;
                }
                else
                {
                    RdPayFor.SelectedIndex = 1;
                }
                
                    gv_Chg_Details.DataSource = dss.Tables[0];
                    gv_Chg_Details.DataBind();
                    int rowIndex = 0;
                    for (int i = 0; i < dss.Tables[0].Rows.Count; i++)
                    {
                        decimal totamt = 0;
                        TextBox txtUser_BranchName = (TextBox)gv_Chg_Details.Rows[rowIndex].FindControl("txtUser_BranchName");
                        TextBox txtBill_inv_No = (TextBox)gv_Chg_Details.Rows[rowIndex].FindControl("txtBill_inv_No");
                        TextBox txtBill_inv_date = (TextBox)gv_Chg_Details.Rows[rowIndex].FindControl("txtBill_inv_date");
                        TextBox txtJOBNO = (TextBox)gv_Chg_Details.Rows[rowIndex].FindControl("txtJOBNO");
                        TextBox txtIMP_EXP = (TextBox)gv_Chg_Details.Rows[rowIndex].FindControl("txtIMP_EXP");
                        TextBox txtBillAmt = (TextBox)gv_Chg_Details.Rows[rowIndex].FindControl("txtBillAmt");
                        TextBox txtReceivedAmt = (TextBox)gv_Chg_Details.Rows[rowIndex].FindControl("txtReceivedAmt");
                        CheckBox chksel = (CheckBox)gv_Chg_Details.Rows[rowIndex].FindControl("chk_inv");
                        TextBox txtPayAmt1 = (TextBox)gv_Chg_Details.Rows[rowIndex].FindControl("txtPayAmt");
                        TextBox txtBalAmt1 = (TextBox)gv_Chg_Details.Rows[rowIndex].FindControl("txtBalAmt1");
                        TextBox txtTDSNo1 = (TextBox)gv_Chg_Details.Rows[rowIndex].FindControl("txtTDSNo");
                        TextBox txtTDSDate1 = (TextBox)gv_Chg_Details.Rows[rowIndex].FindControl("txtTDSDate");
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
                        txtTDSNo1.Text = dss.Tables[0].Rows[i]["TDS_No"].ToString();
                        txtTDSDate1.Text = dss.Tables[0].Rows[i]["TDS_Date"].ToString();
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
                gvPaymentdetails.DataSource = dss.Tables[2];
                gvPaymentdetails.DataBind();

                ViewState["PaymentHistory"] = dss.Tables[2];

                txtTotBillAmt.Text = dss.Tables[2].Compute("Sum(ReceivedAmt)", "").ToString();
                txtTotTDSAmt.Text = dss.Tables[2].Compute("Sum(TDSAmt)", "").ToString();
                txtFinalReceivedAmt.Text = dss.Tables[2].Compute("Sum(TotalReceived)", "").ToString();
                txtTotWriteOff.Text = dss.Tables[2].Compute("Sum(WriteOffAmt)", "").ToString();
            }

            //dss.Dispose();
            //GC.Collect();
        }
        catch (Exception ex)
        {
            Connection.Error_Msg(ex.Message);
        }
    }


    protected void gv_Chg_Details_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        DataSet ds = new DataSet();
        try
        {
            FirstRow();
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
                    if (dt.Rows.Count >1)
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
                                ObjUBO.Flag = "DELETE_SUB";
                                ObjUBO.ID = apay;
                                ds = BI.PaymentEntryIDU_NEW(ObjUBO);
                                if (ds.Tables.Count > 0)
                                {
                                    if (ds.Tables[0].Rows.Count > 0)
                                    {
                                        Page page = HttpContext.Current.CurrentHandler as Page;
                                        string script = string.Format("alert('{0}');", ds.Tables[0].Rows[0][1].ToString());
                                        if (page != null && !page.ClientScript.IsClientScriptBlockRegistered("alert"))
                                        {
                                            page.ClientScript.RegisterClientScriptBlock(page.GetType(), "alert", script, true /* addScriptTags */);
                                        }
                                        btnUpdate.Visible = false;

                                        DataTable dt1 = new DataTable();
                                        gv_Chg_Details.DataSource = dt1;
                                        gv_Chg_Details.DataBind();
                                        

                                    }
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
                    SetPreviousData();    //gv_Chg_Details.DeleteRow(index);
                }
                else if (gv_Chg_Details.Rows.Count>=1)
                {
                }
            }
        }
        catch (Exception ex)
        {
            Connection.Error_Msg(ex.Message);
        }
    }

    private void ResetRowID(DataTable dt)
    {
        int rowNumber = 1;
        if (dt.Rows.Count >= 0)
        {
            foreach (DataRow row in dt.Rows)
            {
                row[0] = rowNumber;
                rowNumber++;
            }
        }
    }   
    private void FirstRow()
    {
        DataTable dt = new DataTable();
        DataRow dr = dt.NewRow();

        dt.Columns.Add(new DataColumn("USER_BRANCHNAME", typeof(string)));
        dt.Columns.Add(new DataColumn("BILL_INV_NO", typeof(string)));
        dt.Columns.Add(new DataColumn("Bill_inv_date", typeof(string)));
        dt.Columns.Add(new DataColumn("JOBNO", typeof(string)));
        dt.Columns.Add(new DataColumn("IMP_EXP", typeof(string)));

        dt.Columns.Add(new DataColumn("TOTAL_AMT_TAX", typeof(string)));
        dt.Columns.Add(new DataColumn("ReceivedAmt", typeof(string)));
        dt.Columns.Add(new DataColumn("BalanceAmt", typeof(string)));
        dt.Columns.Add(new DataColumn("CurrentPayable", typeof(string)));  //txtPayAmt
        dt.Columns.Add(new DataColumn("TDS_No", typeof(string)));  //txtTDSNo

        dt.Columns.Add(new DataColumn("TDS_Date", typeof(string))); //txtTDSDate
        dt.Columns.Add(new DataColumn("TDS_Percent", typeof(string))); //txtTDSPercent
        dt.Columns.Add(new DataColumn("TDS_Amt", typeof(string)));//txtTDSAmt.Text
        dt.Columns.Add(new DataColumn("Remarks", typeof(string)));//txtBillRemarks
        dt.Columns.Add(new DataColumn("Iswriteoff", typeof(string)));//chkWrite
        dt.Columns.Add(new DataColumn("WriteoffAmt", typeof(string)));//txtWriteOffAmt
        
        
        dt.Rows.Add(dr);        
        ViewState["NewCurrentTable"] = dt;
        //gv_Chg_Details.DataSource = dt;
        //gv_Chg_Details.DataBind();  
    }
    private void AddNewRow()
    {
        int rowIndex = 0;
        //decimal txtTotTDSAmt = 0;
        //decimal txtReceivedAmt = 0;
        //decimal txtTDSNo = 0;
       
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
                    TextBox txtBill_inv_date = (TextBox)gv_Chg_Details.Rows[rowIndex].Cells[3].FindControl("txtBill_inv_date");
                    TextBox txtJOBNO = (TextBox)gv_Chg_Details.Rows[rowIndex].Cells[4].FindControl("txtJOBNO");
                    TextBox txtIMP_EXP = (TextBox)gv_Chg_Details.Rows[rowIndex].Cells[5].FindControl("txtIMP_EXP");


                    TextBox txtBillAmt = (TextBox)gv_Chg_Details.Rows[rowIndex].Cells[6].FindControl("txtBillAmt");
                    TextBox txtReceivedAmt = (TextBox)gv_Chg_Details.Rows[rowIndex].Cells[7].FindControl("txtReceivedAmt");
                    TextBox txtBalAmt = (TextBox)gv_Chg_Details.Rows[rowIndex].Cells[8].FindControl("txtBalAmt");
                    TextBox txtPayAmt = (TextBox)gv_Chg_Details.Rows[rowIndex].Cells[9].FindControl("txtPayAmt");
                    TextBox txtTDSNo = (TextBox)gv_Chg_Details.Rows[rowIndex].Cells[10].FindControl("txtTDSNo");

                    TextBox txtTDSDate = (TextBox)gv_Chg_Details.Rows[rowIndex].Cells[11].FindControl("txtTDSDate");
                    TextBox txtTDSPercent = (TextBox)gv_Chg_Details.Rows[rowIndex].Cells[12].FindControl("txtTDSPercent");
                    TextBox txtTDSAmt = (TextBox)gv_Chg_Details.Rows[rowIndex].Cells[13].FindControl("txtTDSAmt");
                    TextBox txtBillRemarks = (TextBox)gv_Chg_Details.Rows[rowIndex].Cells[14].FindControl("txtBillRemarks");
                    CheckBox chkWrite = (CheckBox)gv_Chg_Details.Rows[rowIndex].Cells[15].FindControl("chkWrite");
                    TextBox txtWriteOffAmt = (TextBox)gv_Chg_Details.Rows[rowIndex].Cells[16].FindControl("txtWriteOffAmt");

                    drCurrentRow = dtCurrentTable.NewRow();

                    dtCurrentTable.Rows[i - 1]["USER_BRANCHNAME"] = txtUser_BranchName.Text;
                    dtCurrentTable.Rows[i - 1]["BILL_INV_NO"] = txtBill_inv_No.Text;
                    dtCurrentTable.Rows[i - 1]["BILL_INV_DATE"] = (txtBill_inv_date.Text == string.Empty) ? DateTime.Parse(("01/01/1900")).ToString("dd/MM/yyyy") : txtBill_inv_date.Text;
                     
                    dtCurrentTable.Rows[i - 1]["JOBNO"] = txtJOBNO.Text;

                    dtCurrentTable.Rows[i - 1]["TOTAL_AMT_TAX"] = (txtBillAmt.Text == string.Empty) ? "0" : txtBillAmt.Text; 
                    dtCurrentTable.Rows[i - 1]["ReceivedAmt"] = (txtReceivedAmt.Text == string.Empty) ? "0.00" : txtReceivedAmt.Text; 
                    dtCurrentTable.Rows[i - 1]["BalanceAmt"] = (txtBalAmt.Text == string.Empty) ? "0" : txtBalAmt.Text; 
                    dtCurrentTable.Rows[i - 1]["Currentpayable"] = (txtPayAmt.Text == string.Empty) ? "0" : txtPayAmt.Text; 
                    dtCurrentTable.Rows[i - 1]["TDS_No"] = txtTDSNo.Text;//

                    dtCurrentTable.Rows[i - 1]["TDS_Date"] = (txtTDSDate.Text == string.Empty) ? DateTime.Parse(("01/01/1900")).ToString("dd/MM/yyyy") : txtTDSDate.Text;
                    
                    dtCurrentTable.Rows[i - 1]["TDS_Percent"] = (txtTDSPercent.Text == string.Empty) ? "0.00" : txtTDSPercent.Text;
                    dtCurrentTable.Rows[i - 1]["TDS_Amt"] = (txtTDSAmt.Text == string.Empty) ? "0.00" : txtTDSAmt.Text; 
                    dtCurrentTable.Rows[i - 1]["Remarks"] = txtBillRemarks.Text;//
                    dtCurrentTable.Rows[i - 1]["IsWriteoff"] = chkWrite.Checked; //
                    dtCurrentTable.Rows[i - 1]["WriteoffAmt"] = (txtWriteOffAmt.Text == string.Empty) ? "0.00" : txtWriteOffAmt.Text; 

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
       
        if (ViewState["NewCurrentTable"] != null)
        {
            DataTable dt = (DataTable)ViewState["NewCurrentTable"];
            DataRow drCurrentRow = null;
            if (dt.Rows.Count >0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    CheckBox chk_inv = (CheckBox)gv_Chg_Details.Rows[rowIndex].Cells[0].FindControl("chk_inv");

                    TextBox txtUser_BranchName = (TextBox)gv_Chg_Details.Rows[rowIndex].Cells[1].FindControl("txtUser_BranchName");
                    TextBox txtBill_inv_No = (TextBox)gv_Chg_Details.Rows[rowIndex].Cells[2].FindControl("txtBill_inv_No");
                    TextBox txtBill_inv_date = (TextBox)gv_Chg_Details.Rows[rowIndex].Cells[3].FindControl("txtBill_inv_date");
                    TextBox txtJOBNO = (TextBox)gv_Chg_Details.Rows[rowIndex].Cells[4].FindControl("txtJOBNO");
                    TextBox txtIMP_EXP = (TextBox)gv_Chg_Details.Rows[rowIndex].Cells[5].FindControl("txtIMP_EXP");

                    TextBox txtBillAmt = (TextBox)gv_Chg_Details.Rows[rowIndex].Cells[6].FindControl("txtBillAmt");
                    TextBox txtReceivedAmt = (TextBox)gv_Chg_Details.Rows[rowIndex].Cells[7].FindControl("txtReceivedAmt");
                    TextBox txtBalAmt = (TextBox)gv_Chg_Details.Rows[rowIndex].Cells[8].FindControl("txtBalAmt");
                    TextBox txtPayAmt = (TextBox)gv_Chg_Details.Rows[rowIndex].Cells[9].FindControl("txtPayAmt");
                    TextBox txtTDSNo = (TextBox)gv_Chg_Details.Rows[rowIndex].Cells[10].FindControl("txtTDSNo");

                    TextBox txtTDSDate = (TextBox)gv_Chg_Details.Rows[rowIndex].Cells[11].FindControl("txtTDSDate");
                    TextBox txtTDSPercent = (TextBox)gv_Chg_Details.Rows[rowIndex].Cells[12].FindControl("txtTDSPercent");
                    TextBox txtTDSAmt = (TextBox)gv_Chg_Details.Rows[rowIndex].Cells[13].FindControl("txtTDSAmt");
                    TextBox txtBillRemarks = (TextBox)gv_Chg_Details.Rows[rowIndex].Cells[14].FindControl("txtBillRemarks");
                    CheckBox chkWrite = (CheckBox)gv_Chg_Details.Rows[rowIndex].Cells[15].FindControl("chkWrite");
                    TextBox txtWriteOffAmt = (TextBox)gv_Chg_Details.Rows[rowIndex].Cells[16].FindControl("txtWriteOffAmt");

                    txtUser_BranchName.Text = dt.Rows[i]["USER_BRANCHNAME"].ToString();
                    txtBill_inv_No.Text = dt.Rows[i]["BILL_INV_NO"].ToString();
                    txtBill_inv_date.Text = dt.Rows[i]["Bill_inv_date"].ToString();
                    txtJOBNO.Text = dt.Rows[i]["JOBNO"].ToString();
                    txtIMP_EXP.Text = dt.Rows[i]["IMP_EXP"].ToString();

                    txtBillAmt.Text = dt.Rows[i]["TOTAL_AMT_TAX"].ToString();
                    txtReceivedAmt.Text = dt.Rows[i]["ReceivedAmt"].ToString();
                    txtBalAmt.Text = dt.Rows[i]["BalanceAmt"].ToString();
                    txtPayAmt.Text = dt.Rows[i]["CurrentPayable"].ToString();  //
                    txtTDSNo.Text = dt.Rows[i]["TDS_No"].ToString();//

                    txtTDSDate.Text = (dt.Rows[i]["TDS_Date"].ToString() == "01/01/1900") ? "" : dt.Rows[i]["TDS_Date"].ToString();
                   
                    txtTDSPercent.Text = dt.Rows[i]["TDS_Percent"].ToString();
                    txtTDSAmt.Text = dt.Rows[i]["TDS_Amt"].ToString();
                    txtBillRemarks.Text = dt.Rows[i]["Remarks"].ToString();
                    chkWrite.Text = dt.Rows[i]["Iswriteoff"].ToString();  //chkWrite
                    if (chkWrite.Text == "True")
                        chkWrite.Checked = true;
                    else
                        chkWrite.Checked = false;
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
            ObjUBO.BRANCH_CODE = Connection.Current_Branch();
            ObjUBO.WORKING_PERIOD = Connection.WorkingPeriod();
            ObjUBO.USER_ID = Connection.Current_User();
           
            if (txtDate.Text != string.Empty)
            {

                if (gv_Chg_Details.Rows.Count > 0)
                {
                    save_update("INSERT");
                }
                else
                {
                    Alert_msg("No Records found in Payment grid");
                }
            }
            btnUpdate.Enabled = true;
            
         
        }
        catch (Exception ex)
        {
            Connection.Error_Msg(ex.Message);
        }
    }

    private void save_update(string name)
    {

         DataTable dt;
         string dat = "01/01/1900";
        string totReceivedAmt = "0";

        if (txtChqDate.Text != "")
        {
            dat = txtChqDate.Text.Trim();
            dat = dat.ToString().Substring(3, 3) + dat.ToString().Substring(0, 3) + dat.ToString().Substring(6, 4);
        }

        if (ddlModeofPay.SelectedValue.ToString() == "Cheque")
        {
            totReceivedAmt = txtChqAmt.Text;
            ObjUBO.Chqno = txtChqNo.Text;
            ObjUBO.BankName = txtChqBank.Text;
            ObjUBO.BankBranch = txtChqBranch.Text;
        }
        else if (ddlModeofPay.SelectedValue.ToString() == "Cash")
            totReceivedAmt = txtChqAmt.Text;
        else if (ddlModeofPay.SelectedValue.ToString() == "Net Banking")
        {            
            totReceivedAmt = txtChqAmt.Text;
            ObjUBO.Chqno = txtChqNo.Text;
             
            ObjUBO.BankName = txtChqBank.Text;
            ObjUBO.BankBranch = txtChqBranch.Text;
        }
            ObjUBO.ExpVoucherNo = txtPaymentNo.Text;
            ObjUBO.IMPORTER_NAME = ddlCustomer.SelectedValue.ToString();  // customer name
            ObjUBO.BILL_TYPE = Rd_Job_Type.SelectedValue.ToString(); // imp/exp job 


            if (RdPayFor.SelectedIndex == 0)
                ObjUBO.TYPE_OF_INVOICE = "Job";
            else
                ObjUBO.TYPE_OF_INVOICE = "Advance";

            ObjUBO.ModeOfPay = ddlModeofPay.SelectedValue.ToString(); //PaymentMode


            ObjUBO.TOTAL = totReceivedAmt;
            ObjUBO.TOTAL_TAX_AMT = "0.00";
            ObjUBO.REMARKS = txtCommNarration.Text;
      

        string Paydate = "";
        if (txtDate.Text != "")
        {
            Paydate = txtDate.Text.Trim();
            Paydate = Paydate.ToString().Substring(3, 3) + Paydate.ToString().Substring(0, 3) + Paydate.ToString().Substring(6, 4);
        }
        
        DataSet ds = new DataSet();
        
        if (name == "UPDATE")
        {
            ObjUBO.ID = HDupdate_id.Value;
        }
        ObjUBO.BILL_INV_DATE = Paydate;  // Payment voucher Date
        ObjUBO.ChqDt = dat;
        ObjUBO.JOB_PREFIX = hdprefix.Value.ToString();
        ObjUBO.JOB_SUFFIX = hdsuffix.Value.ToString();
        dt = gridtodt();
        ds.Tables.Add(dt);
        ObjUBO.XML_DETAIL = ds.GetXml();
        ObjUBO.Flag = name;
        ds = BI.PaymentEntryIDU_NEW(ObjUBO);

        if (ds.Tables.Count > 0)
        {
            if (ds.Tables[0].Rows.Count > 0)
            {
                Page page = HttpContext.Current.CurrentHandler as Page;
                string script = string.Format("alert('{0}');", ds.Tables[0].Rows[0][1].ToString());
                if (page != null && !page.ClientScript.IsClientScriptBlockRegistered("alert"))
                {
                    page.ClientScript.RegisterClientScriptBlock(page.GetType(), "alert", script, true /* addScriptTags */);
                }
                btnUpdate.Visible = true ;

                //DataTable dt1 = new DataTable();
                //gv_Chg_Details.DataSource = dt1;
                //gv_Chg_Details.DataBind();
                btnSave.Visible = false;
            }
        }

    }


    private DataTable gridtodt()
    {
        string expdate = "", tdsDt = "";
        DataTable dt = new DataTable();
        if (gv_Chg_Details.Rows.Count != 0)
        {
            
            dt.Columns.Add("User_BranchName"); dt.Columns.Add("Bill_Inv_RefNo"); dt.Columns.Add("VOUCHER_NO"); dt.Columns.Add("Imp_Exp"); dt.Columns.Add("ReceivedAmt");
            dt.Columns.Add("TDSNo"); dt.Columns.Add("TDS_Date"); dt.Columns.Add("TDS_Percent"); dt.Columns.Add("TDS_Amt"); dt.Columns.Add("Remarks");
            dt.Columns.Add("IsWriteoff"); dt.Columns.Add("WriteoffAmt");  
           
            foreach (GridViewRow row in gv_Chg_Details.Rows)
            {
                CheckBox chk = (CheckBox)row.FindControl("chk_inv");
                if (chk.Checked == true)
                { 
                    DataRow dr = dt.NewRow();
                    TextBox txtBranch = row.FindControl("txtUser_BranchName") as TextBox;  TextBox txtBillNo = row.FindControl("txtBill_inv_No") as TextBox;
                    TextBox txtJob = row.FindControl("txtJOBNO") as TextBox;  TextBox txtImpExp = row.FindControl("txtIMP_EXP") as TextBox;
                    TextBox txtPay = row.FindControl("txtPayAmt") as TextBox;  TextBox txtTDSNumber = row.FindControl("txtTDSNo") as TextBox;
                    TextBox txtTDSDt = row.FindControl("txtTDSDate") as TextBox; TextBox txtTDSPrc = row.FindControl("txtTDSPercent") as TextBox;
                    TextBox txtTDSAmount = row.FindControl("txtTDSAmt") as TextBox;   TextBox txtRem = row.FindControl("txtBillRemarks") as TextBox;
                    CheckBox isWrite = row.FindControl("chkWrite") as CheckBox;
                    TextBox txtWriteOffAmount = row.FindControl("txtWriteOffAmt") as TextBox;

                    dr["User_BranchName"] = txtBranch.Text; dr["Bill_Inv_RefNo"] = txtBillNo.Text; dr["VOUCHER_NO"] = txtJob.Text;
                    dr["Imp_Exp"] = txtImpExp.Text; dr["ReceivedAmt"] = txtPay.Text; dr["TDSNo"] = txtTDSNumber.Text;

                    if (txtTDSDt.Text != "")
                    {
                        expdate = txtTDSDt.Text.Trim();
                        expdate = expdate.ToString().Substring(3, 3) + expdate.ToString().Substring(0, 3) + expdate.ToString().Substring(6, 4);
                        dr["TDS_Date"] = expdate;
                         
                    }
                    else { dr["TDS_Date"] = "01/01/1900"; }
                    dr["TDS_Percent"] = txtTDSPrc.Text; dr["TDS_Amt"] = txtTDSAmount.Text;
                    dr["Remarks"] = txtRem.Text;
                    if (isWrite.Checked)
                    { dr["IsWriteoff"] = "Y"; }
                    else { dr["IsWriteoff"] = "N"; }
                    dr["WriteoffAmt"] = txtWriteOffAmount.Text;
                    dt.Rows.Add(dr);
                }
            }
        }
        return dt;
    }

    protected void btnUpdate_Click(object sender, EventArgs e)
    {
        ObjUBO.BRANCH_CODE = Connection.Current_Branch();
        ObjUBO.WORKING_PERIOD = Connection.WorkingPeriod();
        ObjUBO.USER_ID = Connection.Current_User();
           
        try
        {
            if (gv_Chg_Details.Rows.Count > 0)
            {
                save_update("UPDATE");
            }
            else
            {
                Alert_msg("No Records found in Payment grid");
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
            DataSet ds = new DataSet();
            ObjUBO.Flag = "DELETE";
            ds = BI.PaymentEntryIDU_NEW(ObjUBO);
            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    Page page = HttpContext.Current.CurrentHandler as Page;
                    string script = string.Format("alert('{0}');", ds.Tables[0].Rows[0][1].ToString());
                    if (page != null && !page.ClientScript.IsClientScriptBlockRegistered("alert"))
                    {
                        page.ClientScript.RegisterClientScriptBlock(page.GetType(), "alert", script, true /* addScriptTags */);
                    }
                    btnUpdate.Visible = false;

                    DataTable dt1 = new DataTable();
                    gv_Chg_Details.DataSource = dt1;
                    gv_Chg_Details.DataBind();
                }
            }
        
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
                ObjUBO.Flag = "Select-PurchaseEntryVendorName";
                dss = BI.Load_DDL(ObjUBO);
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
            else if (gv_Chg_Details.Rows.Count >= 1)
            {
                //SetInitialRow();
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
                TextBox txtTDSNo1 = (TextBox)row.FindControl("txtTDSNo");
                TextBox txtTDSPercent1 = (TextBox)row.FindControl("txtTDSPercent");                
                TextBox txtTDSDate1 = (TextBox)row.FindControl("txtTDSDate");
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

                cnt = 1;
                if (txtTDSDate1.Text == null)
                {
                    txtTDSDate1.Text = string.Empty;
                }

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
                    if (RdPayFor.SelectedIndex == 0)
                    {
                        Alert_msg("Current Receivable should less than balance amount ");
                        return;
                    }
                    else
                    {
                    }
                }
                
                dr = dt.NewRow();
                dr[0] = R;
                dr["BILL_INV_NO"] = txtBill_inv_No.Text;  // Payment                
                dr["JOBNO"] = txtJOBNO.Text.Trim(); // job no from grid column 4   //3
                dr["Imp_Exp"] = txtIMP_EXP.Text.Trim();
                dr["ReceivedAmt"] = txtPayAmt1.Text.Trim(); // TDS no
                dr["TDS_No"] = txtTDSNo1.Text.Trim(); // TDS dt
                dr["TDS_Date"] = (txtTDSDate1.Text == string.Empty) ? DateTime.Parse(("01/01/1900")).ToString("dd/MM/yyyy") : DateTime.Parse((txtTDSDate1.Text)).ToString("dd/MM/yyyy");  // TDS dt 
              
                dr["TDS_Percent"] = (txtTDSPercent1.Text == "") ? "0" : txtTDSPercent1.Text;   // TDS %
                dr["TDS_Amt"] = (txtTDSAmt1.Text == "") ? "0" : txtTDSAmt1.Text;   // TDSAmt
                dr["Remarks"] = txtRemarks1.Text;  // Remarks
                dr["IsWriteoff"] = (chkwrite1.Checked == true) ? "Y" : "N";  // is write off or not
                dr["WriteoffAmt"] = txtWriteOffAmt1.Text;  // write off amt
               
                dt.Rows.Add(dr);
                R = R + 1;
            }
            DataView dv = new DataView(dt);
            dt = dv.ToTable("selected", false, "USER_BRANCHNAME", "ReceivedAmt", "JOBNO", "Bill_Inv_No", "IMP_EXP", "TDS_No", "TDS_Date", "TDS_Percent", "TDS_AMT", "Remarks", "Iswriteoff", "WriteoffAmt");
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

            gvPaymentdetails.DataSource = dt;
            gvPaymentdetails.DataBind();
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
            GetAutono_Payment();
        }
        catch (Exception ex)
        {
            Connection.Error_Msg(ex.Message);
        }

    }
    protected void gvdetails_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName.Equals("add"))
        {
            AddNewRow();
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
    //public void LoadFinancialYear()
    //{
    //    SqlCommand cmd;
    //    SqlConnection con;
    //    SqlDataReader dr;

    //    con = new SqlConnection(System.Configuration.ConfigurationManager.AppSettings["con"]);
    //    try
    //    {
    //        con.Open();
    //        ddlWorkingPeriod.Items.Clear();
    //        cmd = new SqlCommand("select * from FINANCIAL_YEAR order by WORKING_PERIOD desc", con);
    //        dr = cmd.ExecuteReader();

    //        while (dr.Read())
    //        {
    //            ddlWorkingPeriod.Items.Add(dr["WORKING_PERIOD"].ToString());
    //        }
    //        dr.Close();
    //        con.Close();
    //    }
    //    catch (Exception ex)
    //    {
    //        Connection.Error_Msg(ex.Message);
    //    }
    //    finally
    //    {
    //        con.Close();
    //    }
    //}

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
            if (COMPANY_LICENSE == "eri00001")
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
            
           
            ObjUBO.Flag = "Select-Branch-PurchaseVendor_CustomerBased";
            ObjUBO.Ref_ID = ddlCustomer.SelectedValue.ToString();
            ds = BI.Load_DDL(ObjUBO);
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlbranch_No.DataSource = ds.Tables[0];
                ddlbranch_No.DataTextField = "BRANCH_CODE";
                ddlbranch_No.DataValueField = "BRANCH_CODE";
                ddlbranch_No.DataBind();
            }
            ddlbranch_No.Items.Insert(0, new ListItem("All", "All"));
            ddlbranch_No.SelectedIndex = 0;
            if (ddlbranch_No.SelectedItem.Text != string.Empty)
            {
                LoadPendingJob();
            }
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
        try
        {
            LoadPayDiv_inSinglePanel();
        }
        catch (Exception ex)
        {
            Connection.Error_Msg(ex.Message);
        }
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

                txtChqNo.Text = "";
                txtChqDate.Text = "";
                txtChqBank.Text = "";
                txtChqBranch.Text = "";
                txtCashAmt.Text = "";

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
    private void LoadPayDiv()
    {
        try
        {
        if (ddlModeofPay.SelectedValue.ToString() == "Cheque")
        {
             
            PnlNetBank.Enabled = false;
            PnlCheque.Enabled = true;
            PnlCash.Enabled = false;
            txtCashAmt.Text = "";
            txtNetBank.Text = "";
            txtNetBranch.Text = "";
            txtNetAmt.Text = "";
            txtNetRefno.Text = "";
            txtNetRefDt.Text = "";
        }
        else if (ddlModeofPay.SelectedValue.ToString() == "Cash")
        {
           
            PnlNetBank.Enabled = false;
            PnlCheque.Enabled = false;
            PnlCash.Enabled = true;
            
            txtNetBank.Text = "";
            txtNetBranch.Text = "";
            txtNetAmt.Text = "";
            txtChqNo.Text = "";
            txtChqDate.Text = "";
            txtChqBank.Text = "";
            txtChqBranch.Text = "";
            txtNetRefno.Text = "";
            txtNetRefDt.Text = "";

        }
        else if (ddlModeofPay.SelectedValue.ToString() == "Net Banking")
        {
             
            PnlNetBank.Enabled = true;
            PnlCheque.Enabled = false;
            PnlCash.Enabled = false;

            txtChqNo.Text = "";
            txtChqDate.Text = "";
            txtChqBank.Text = "";
            txtChqBranch.Text = "";
            txtCashAmt.Text = ""; 

        }
        }
        catch (Exception ex)
        {
            Connection.Error_Msg(ex.Message);
        }
    }

    //private DataSet Database_Con_string()
    //{
    //    DataSet ds = new DataSet();
    //    DataSet dss = new DataSet();
    //    if (Session["COMPANY_DETAILS_DS"] != null)
    //    {
    //        ds = (DataSet)Session["COMPANY_DETAILS_DS"];

    //        DataTable ds3 = new DataTable();
    //        ds3 = ds.Tables[2];
    //        DataView view1 = ds3.DefaultView;
    //        string a = ddlWorkingPeriod.SelectedValue.ToString();
    //        view1.RowFilter = "WORKING_PERIOD = '" + a + "'";
    //        DataTable table1 = view1.ToTable();
    //        dss = BI.Load_DDL(ObjUBO, table1.Rows[0]["SERVER_NAME"].ToString());

    //    }
    //    return dss;
    //}

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
            PaymentEntry_cs BI = new PaymentEntry_cs();
            Billing_UserBO ObjUBO = new Billing_UserBO();
            ObjUBO.Flag = "Select-ImpExp-VendorPendingBills";
            ObjUBO.BRANCH_CODE = Tempddlbranch_No;
            ObjUBO.Ref_ID = TempddlCustomer;
            ds = BI.Load_DDL_PAYMENTENTRY(ObjUBO);
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
            PaymentEntry_cs BI = new PaymentEntry_cs();
            Billing_UserBO ObjUBO = new Billing_UserBO();

            ObjUBO.Flag = "Select-ImpExp-VendorPendingBills";
            ObjUBO.BRANCH_CODE = Tempddlbranch_No;
            ObjUBO.Ref_ID = TempddlCustomer;
            ds = BI.Load_DDL_PAYMENTENTRY(ObjUBO);
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


    
   

   
}