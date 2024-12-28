﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using BussinessObject;
using System.Text;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;

public partial class PurchaseEntry : ThemeClass
{
    public string currentuser;
    public string currentbranch;
    public string COMPANY_LICENSE;
    eFreightExport_BookingForm Transact = new eFreightExport_BookingForm();
    User_Creation user_Create = new User_Creation();
    AppSession aps = new AppSession();

    Purchase_cs BI = new Purchase_cs();
    Billing_UserBO ObjUBO = new Billing_UserBO();
    public string Value = "";
    public string SCREEN_ID, PAGE_READ, PAGE_WRITE, PAGE_MODIFY, PAGE_DELETE, Is_Master_Id, Screen_IdNew, COMPANY_ID;
    public int i, Screen_Id;
    protected void Page_Load(object sender, EventArgs e)
    {
        
        CalendarExtender1.EndDate = DateTime.Now;
        DataSet ds = new DataSet();
        aps.checkSession();
        currentuser = Session["currentuser"].ToString();
        COMPANY_LICENSE = Connection.Company_License().ToLower();
        currentbranch = Session["currentbranch"].ToString();

        Screen_Id = 100;
        Page_Rights(Screen_Id);
        if (!IsPostBack)
        {
            currentuser = Session["currentuser"].ToString();
            if (Connection.Company_License().ToLower() == "erf00026")
            {
                if (currentuser.ToUpper() == "ACCOUNTS" || currentuser.ToUpper() == "BOMACC" || currentuser.ToUpper() == "ACCOUNTS3")
                {
                    btnDelete.Visible = true;
                }
                else { btnDelete.Visible = false; }
            }

            if (Request.QueryString["voucherno"] != null && Request.QueryString["voucherno"] != string.Empty)
            {
                LoadUser();
                Update_Item_Load();                
                btnSave.Visible = false;
                btnUpdate.Visible = true;
                
            }
            else
            {
                txtVoucherdate.Text = DateTime.Now.ToShortDateString();
                //AUTO_JOBNO();
                LoadUser();
                LoadVendor();
                string a = Connection.Company_License();
                if (Connection.Company_License() == "ERF00027")
                {
                    LoadVendor_Name();
                }
                btnSave.Visible = true;
                btnUpdate.Visible = false;
                btnDelete.Visible = false;
                
                //ObjUBO.ENAME = "VOURCHERID";
                //ObjUBO.BRANCH_CODE = currentbranch;
                //ObjUBO.WORKING_PERIOD = Connection.WorkingPeriod();
                //ds = BI.PurchaseEntry(ObjUBO);
                //if (ds.Tables[1].Rows.Count > 0 && ds.Tables[2].Rows.Count > 0 && ds.Tables[0].Rows.Count>0)
                //{
                //    txtVoucherno.Text = ds.Tables[1].Rows[0][0].ToString() + ds.Tables[0].Rows[0][0].ToString() + ds.Tables[2].Rows[0][0].ToString();
                //}
                //else { Alert_msg("Must set prefix sufix"); }

                txtInvoiceCurrency.Attributes.Add("onblur", "javascript:Call__Inv_Currency_Excahnge_Rate('" + txtInvoiceCurrency.ClientID + "', '" + txtExrate.ClientID + "')");

                //txtInvoiceCurrency.Text = "INR";
                //txtExrate.Text = "1";
               
                ViewState["RowNumber"] = 1;
                DataTable dt = new DataTable();
                DataRow dr = dt.NewRow();
                DropDownList drp = new DropDownList();
                dt.Columns.Add(new DataColumn("SNo", typeof(string)));
                dt.Columns.Add(new DataColumn("DR_CR", typeof(string)));
                dt.Columns.Add(new DataColumn("Imp_Exp", typeof(string)));

                dt.Columns.Add(new DataColumn("JobNo", typeof(string)));
                dt.Columns.Add(new DataColumn("File_Ref_No", typeof(string)));
                dt.Columns.Add(new DataColumn("TAX_INVNO_PS", typeof(string)));   //add new one by rosi
                dt.Columns.Add(new DataColumn("Date", typeof(string)));
              
                dt.Columns.Add(new DataColumn("Customer_Name", typeof(string)));
             
                dt.Columns.Add(new DataColumn("Charge_Name", typeof(string)));
                dt.Columns.Add(new DataColumn("Gst_Type", typeof(string)));
                dt.Columns.Add(new DataColumn("Tax_Rate", typeof(string)));
                dt.Columns.Add(new DataColumn("Credit_FC", typeof(string)));
                dt.Columns.Add(new DataColumn("Debit_FC", typeof(string)));
                dt.Columns.Add(new DataColumn("Credit", typeof(string)));
                dt.Columns.Add(new DataColumn("Debit", typeof(string)));
                dt.Columns.Add(new DataColumn("CGST", typeof(string)));
                dt.Columns.Add(new DataColumn("SCGST", typeof(string)));
                dt.Columns.Add(new DataColumn("IGST", typeof(string)));
                dt.Columns.Add(new DataColumn("TDS", typeof(string)));
                dt.Columns.Add(new DataColumn("TDS_Amount", typeof(string)));

                dt.Columns.Add(new DataColumn("Remarks", typeof(string)));
                dt.Columns.Add(new DataColumn("FileUp", typeof(string)));
                dt.Columns.Add(new DataColumn("FILE_NAME", typeof(string)));
                dt.Rows.Add(dr);
                dr["DR_CR"] = new ListItem("DR");
                dr["Imp_exp"] = new ListItem("---");
                dr["SNo"] = ViewState["RowNumber"].ToString();
                dr["Gst_Type"] = new ListItem("");

                ViewState["CurrentTable"] = dt;
                gvdetails.Visible = true;
                gvdetails.DataSource = dt;
                gvdetails.DataBind();
                LinkButton lnkBtn = (LinkButton)gvdetails.Rows[0].FindControl("LinkButton1"); 
                lnkBtn.Visible = false; 
                FileUpload Fileupl = (FileUpload)gvdetails.Rows[0].FindControl("FileUpload1"); 
                Fileupl.Visible = true; 


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
        //Hidden_Voucher_date.Value = "";
    }

    public void Page_Show_Hide(int i)
    {
        bool is_page;
        if (i == 100)
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

    private void AddNewRow()
    {


        int rowIndex = 0;
        decimal totalcredit = 0;
        decimal totaldebit = 0;
        decimal totalcgst = 0;
        decimal totalsgst = 0;
        decimal totaligst = 0;
        decimal totaltds = 0;
        decimal total=0;
        decimal fctotalCr = 0;
        decimal fctotalDb = 0;
        if (ViewState["CurrentTable"] != null)
        {
            DataTable dtCurrentTable = (DataTable)ViewState["CurrentTable"];
            DataRow drCurrentRow = null;
            if (dtCurrentTable.Rows.Count > 0)
            {
                int Slno = 1;
               
                for (int i = 1; i <= dtCurrentTable.Rows.Count; i++)
                {
                    DropDownList drpgDrCr = (DropDownList)gvdetails.Rows[rowIndex].Cells[1].FindControl("drpDrcr");
                    DropDownList drpgImpexp = (DropDownList)gvdetails.Rows[rowIndex].Cells[2].FindControl("drpImpexp");
                  TextBox txtJOBNO = (TextBox)gvdetails.Rows[rowIndex].Cells[3].FindControl("txtJOBNO");
                     TextBox txtgFileRefNo = (TextBox)gvdetails.Rows[rowIndex].Cells[4].FindControl("txtgFileRefNo");
                     TextBox txtTAX_INVNO_PS = (TextBox)gvdetails.Rows[rowIndex].Cells[5].FindControl("txtTAX_INVNO_PS"); //add new one by rosi --26-08-2023
                     TextBox txtgDate = (TextBox)gvdetails.Rows[rowIndex].Cells[6].FindControl("txtgDate");
                    TextBox txtgCusname = (TextBox)gvdetails.Rows[rowIndex].Cells[7].FindControl("txtgCusName");
                 
                    TextBox txtgCharhead = (TextBox)gvdetails.Rows[rowIndex].Cells[8].FindControl("txtgCharHead");
                    DropDownList ddlgsttype = (DropDownList)gvdetails.Rows[rowIndex].Cells[9].FindControl("ddl_Gst_type");
                    TextBox txtgTaxrate = (TextBox)gvdetails.Rows[rowIndex].Cells[10].FindControl("txtgTaxrate");

                    TextBox txtgCreditfc = (TextBox)gvdetails.Rows[rowIndex].Cells[11].FindControl("txtgCreditfc");
                    TextBox txtgDebitfc = (TextBox)gvdetails.Rows[rowIndex].Cells[12].FindControl("txtgDebitfc");
                    TextBox txtgCredit1 = (TextBox)gvdetails.Rows[rowIndex].Cells[13].FindControl("txtgCredit1");
                    TextBox txtgDepit = (TextBox)gvdetails.Rows[rowIndex].Cells[14].FindControl("txtgDebit1");
                    TextBox txtgCgst = (TextBox)gvdetails.Rows[rowIndex].Cells[15].FindControl("txtgCgst");
                    TextBox txtgScgst = (TextBox)gvdetails.Rows[rowIndex].Cells[16].FindControl("txtgScgst");
                    TextBox txtgIgst = (TextBox)gvdetails.Rows[rowIndex].Cells[17].FindControl("txtgIgst");
                    TextBox txtgTds = (TextBox)gvdetails.Rows[rowIndex].Cells[18].FindControl("txtgTds");
                    TextBox txtgTdsamt = (TextBox)gvdetails.Rows[rowIndex].Cells[19].FindControl("txtgdtsamt");
                    FileUpload file = (FileUpload)gvdetails.Rows[rowIndex].Cells[20].FindControl("FileUpload1"); 
                    HiddenField hfFile = (HiddenField)gvdetails.Rows[rowIndex].Cells[20].FindControl("hfFileByte");
                    TextBox txtRemarks = (TextBox)gvdetails.Rows[rowIndex].Cells[21].FindControl("txtRemarks");
                    if (txtgTds.Text == "") txtgTds.Text = "0.00";
                    if (txtgCredit1.Text != "")
                    {
                        totalcredit += Convert.ToDecimal(txtgCredit1.Text);

                    }
                    else txtgCredit1.Text = "0.00";
                    if (txtgCreditfc.Text != "") { fctotalCr += Convert.ToDecimal(txtgCreditfc.Text); }
                    else txtgCreditfc.Text = "0.00";
                    if (txtgDepit.Text != "")
                    {
                        totaldebit += Convert.ToDecimal(txtgDepit.Text);

                    }
                    else txtgDepit.Text = "0.00";
                    if (txtgDebitfc.Text != "") { fctotalDb += Convert.ToDecimal(txtgDebitfc.Text); }
                    else txtgDebitfc.Text = "0.00";
                    if (txtgCgst.Text != "") { totalcgst += Convert.ToDecimal(txtgCgst.Text); }
                    if (txtgScgst.Text != "") { totalsgst += Convert.ToDecimal(txtgScgst.Text); }
                    if (txtgIgst.Text != "") { totaligst += Convert.ToDecimal(txtgIgst.Text); }
                    if (txtgTdsamt.Text != "") { totaltds += Convert.ToDecimal(txtgTdsamt.Text); }
                    //total = totaldebit + totalcgst + totaligst + totalsgst - totaltds;
                    total = totaldebit + totalcgst + totaligst + totalsgst ;

                    drCurrentRow = dtCurrentTable.NewRow();
                    drCurrentRow["DR_CR"] = new ListItem("DR");
                    drCurrentRow["Imp_Exp"] = new ListItem("---");
                    drCurrentRow["Gst_Type"] = new ListItem("");

                    dtCurrentTable.Rows[i - 1]["SNo"] = Slno;
                    dtCurrentTable.Rows[i - 1]["DR_CR"] = drpgDrCr.SelectedValue;
                    dtCurrentTable.Rows[i - 1]["Imp_Exp"] = drpgImpexp.SelectedValue;
                    dtCurrentTable.Rows[i - 1]["JobNo"] = txtJOBNO.Text;
                    dtCurrentTable.Rows[i - 1]["File_Ref_No"] = txtgFileRefNo.Text;
                    dtCurrentTable.Rows[i - 1]["TAX_INVNO_PS"] = txtTAX_INVNO_PS.Text;    // add new one by rosi
                    dtCurrentTable.Rows[i - 1]["Date"] = txtgDate.Text;
                 
                    dtCurrentTable.Rows[i - 1]["Customer_Name"] = txtgCusname.Text;
                  
                    dtCurrentTable.Rows[i - 1]["Charge_Name"] = txtgCharhead.Text;
                    dtCurrentTable.Rows[i - 1]["Gst_Type"] = ddlgsttype.SelectedValue;
                    dtCurrentTable.Rows[i - 1]["Tax_Rate"] = txtgTaxrate.Text;
                    dtCurrentTable.Rows[i - 1]["Credit_FC"] = txtgCreditfc.Text;
                    dtCurrentTable.Rows[i - 1]["Debit_FC"] = txtgDebitfc.Text;
                    dtCurrentTable.Rows[i - 1]["Credit"] = txtgCredit1.Text;
                    dtCurrentTable.Rows[i - 1]["Debit"] = txtgDepit.Text;
                    dtCurrentTable.Rows[i - 1]["CGST"] = txtgCgst.Text;
                    dtCurrentTable.Rows[i - 1]["SCGST"] = txtgScgst.Text;
                    dtCurrentTable.Rows[i - 1]["IGST"] = txtgIgst.Text;
                    dtCurrentTable.Rows[i - 1]["TDS"] = txtgTds.Text;
                    dtCurrentTable.Rows[i - 1]["TDS_Amount"] = txtgTdsamt.Text;
                    dtCurrentTable.Rows[i - 1]["FILE_NAME"] = "";
                    dtCurrentTable.Rows[i - 1]["Remarks"] = txtRemarks.Text;
                    rowIndex++;
                    Slno++;
                    
                }
                

                int index = gvdetails.Rows.Count-1 ;

                drCurrentRow["SNo"] = Slno ;
                if (fctotalDb != null && fctotalDb!= 0)
                {

                    drCurrentRow["Debit_FC"] = Convert.ToDecimal(txtbillamount.Text) - fctotalDb;
                }
                if (fctotalCr != null && fctotalCr != 0)
                {

                    drCurrentRow["Credit_FC"] = Convert.ToDecimal(txtbillamount.Text) - fctotalCr;
                }
                if (Convert.ToDecimal(txtTotalamtininr.Text) >= total)
                {
                    drCurrentRow["Debit"] = Convert.ToDecimal(txtTotalamtininr.Text) - total;
                }
                else
                {
                    totalcredit = Convert.ToDecimal(txtTotalamtininr.Text) - total;
                    drCurrentRow["Credit"] = total - Convert.ToDecimal(txtTotalamtininr.Text);
                 
                   
                }

            
                dtCurrentTable.Rows.Add(drCurrentRow);
                ViewState["CurrentTable"] = dtCurrentTable;

                gvdetails.DataSource = dtCurrentTable;
                gvdetails.DataBind();
                DropDownList drpgImpex = (DropDownList)gvdetails.Rows[index + 1].Cells[2].FindControl("drpImpexp");
                drpgImpex.Focus();
                TextBox txtfcredittotal = (TextBox)gvdetails.FooterRow.FindControl("txtcredittotal");
                TextBox txtfdebittotal = (TextBox)gvdetails.FooterRow.FindControl("txtdebittotal");
                TextBox txtfcgsttotal = (TextBox)gvdetails.FooterRow.FindControl("txtcgsttotal");
                TextBox txtfsgsttotal = (TextBox)gvdetails.FooterRow.FindControl("txtsgsttotal");
                TextBox txtfigsttotal = (TextBox)gvdetails.FooterRow.FindControl("txtigsttotal");
                TextBox txtftdstotal = (TextBox)gvdetails.FooterRow.FindControl("txttdstotal");
                txtfcredittotal.Text = totalcredit.ToString();
                txtfdebittotal.Text = totaldebit.ToString();
                txtfcgsttotal.Text = totalcgst.ToString();
                txtfsgsttotal.Text = totalsgst.ToString();
                txtfigsttotal.Text = totaligst.ToString();
                txtftdstotal.Text = totaltds.ToString();
                
                int index1 = gvdetails.Rows.Count - 1;
            }
        }
        SetPreviousData();
    }
    private void SetPreviousData()
    {
        int rowIndex = 0;

        if (ViewState["CurrentTable"] != null)
        {
            DataTable dt = (DataTable)ViewState["CurrentTable"];
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    TextBox txtgSLNo = (TextBox)gvdetails.Rows[rowIndex].Cells[0].FindControl("txtgSLNo");
                    DropDownList drpgDrCr = (DropDownList)gvdetails.Rows[rowIndex].Cells[1].FindControl("drpDrcr");
                    DropDownList drpgImpexp = (DropDownList)gvdetails.Rows[rowIndex].Cells[2].FindControl("drpImpexp");
                    TextBox txtJOBNO = (TextBox)gvdetails.Rows[rowIndex].Cells[3].FindControl("txtJOBNO");
                    TextBox txtgFileRefNo = (TextBox)gvdetails.Rows[rowIndex].Cells[4].FindControl("txtgFileRefNo");
                    TextBox txtTAX_INVNO_PS = (TextBox)gvdetails.Rows[rowIndex].Cells[5].FindControl("txtTAX_INVNO_PS");
                    TextBox txtgDate = (TextBox)gvdetails.Rows[rowIndex].Cells[6].FindControl("txtgDate");
                    TextBox txtgCusname = (TextBox)gvdetails.Rows[rowIndex].Cells[7].FindControl("txtgCusName");

                    TextBox txtgCharhead = (TextBox)gvdetails.Rows[rowIndex].Cells[8].FindControl("txtgCharHead");
                    DropDownList ddlgsttype = (DropDownList)gvdetails.Rows[rowIndex].Cells[9].FindControl("ddl_Gst_type");
                    TextBox txtgTaxrate = (TextBox)gvdetails.Rows[rowIndex].Cells[10].FindControl("txtgTaxrate");

                    TextBox txtgCreditfc = (TextBox)gvdetails.Rows[rowIndex].Cells[11].FindControl("txtgCreditfc");
                    TextBox txtgDebitfc = (TextBox)gvdetails.Rows[rowIndex].Cells[12].FindControl("txtgDebitfc");
                    TextBox txtgCredit1 = (TextBox)gvdetails.Rows[rowIndex].Cells[13].FindControl("txtgCredit1");
                    TextBox txtgDepit = (TextBox)gvdetails.Rows[rowIndex].Cells[14].FindControl("txtgDebit1");
                    TextBox txtgCgst = (TextBox)gvdetails.Rows[rowIndex].Cells[15].FindControl("txtgCgst");
                    TextBox txtgScgst = (TextBox)gvdetails.Rows[rowIndex].Cells[16].FindControl("txtgScgst");
                    TextBox txtgIgst = (TextBox)gvdetails.Rows[rowIndex].Cells[17].FindControl("txtgIgst");
                    TextBox txtgTds = (TextBox)gvdetails.Rows[rowIndex].Cells[18].FindControl("txtgTds");
                    TextBox txtgTdsamt = (TextBox)gvdetails.Rows[rowIndex].Cells[19].FindControl("txtgdtsamt");
                    FileUpload file = (FileUpload)gvdetails.Rows[rowIndex].Cells[20].FindControl("FileUpload1");
                    HiddenField hfFile = (HiddenField)gvdetails.Rows[rowIndex].Cells[20].FindControl("hfFileByte");
                    TextBox txtRemarks = (TextBox)gvdetails.Rows[rowIndex].Cells[21].FindControl("txtRemarks");


                    LinkButton lnkBtn = (LinkButton)gvdetails.Rows[rowIndex].Cells[16].FindControl("LinkButton1");
                    lnkBtn.Visible = false;
                    file.Visible = true;
                   
                   

                    txtgSLNo.Text = dt.Rows[i]["SNo"].ToString();
                    drpgDrCr.SelectedValue = dt.Rows[i]["DR_CR"].ToString();
                    drpgImpexp.SelectedValue = dt.Rows[i]["Imp_Exp"].ToString();
                    txtJOBNO.Text = dt.Rows[i]["JobNo"].ToString();
                    txtgFileRefNo.Text = dt.Rows[i]["File_Ref_No"].ToString();
                    txtTAX_INVNO_PS.Text = dt.Rows[i]["TAX_INVNO_PS"].ToString();//add new one
                    txtgDate.Text = dt.Rows[i]["Date"].ToString();
                
                    txtgCusname.Text = dt.Rows[i]["Customer_Name"].ToString();
                 
                    txtgCharhead.Text = dt.Rows[i]["Charge_Name"].ToString();
                    ddlgsttype.SelectedValue = dt.Rows[i]["Gst_Type"].ToString();
                    txtgTaxrate.Text = dt.Rows[i]["Tax_Rate"].ToString();
                    txtgCreditfc.Text = dt.Rows[i]["Credit_FC"].ToString();
                    txtgDebitfc.Text = dt.Rows[i]["Debit_FC"].ToString();
                    txtgCredit1.Text = dt.Rows[i]["Credit"].ToString();
                    txtgDepit.Text = dt.Rows[i]["Debit"].ToString();
                    txtgCgst.Text = dt.Rows[i]["CGST"].ToString();
                    txtgScgst.Text = dt.Rows[i]["SCGST"].ToString();
                    txtgIgst.Text = dt.Rows[i]["IGST"].ToString();
                    txtgTds.Text = dt.Rows[i]["TDS"].ToString();
                    txtgTdsamt.Text = dt.Rows[i]["TDS_Amount"].ToString();
                    hfFile.Value = file.HasFile ? Path.GetFileName(file.PostedFile.FileName) : file.FileName;
                    txtRemarks.Text = dt.Rows[i]["Remarks"].ToString();
                    rowIndex++;
                }
            }
        }
    }
    protected void gvdetails_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName.Equals("add"))
        {
            AddNewRow();
        }
    }
    private void SetRowData()
    {
        int rowIndex = 0;
        decimal totalcredit = 0;
        decimal totaldebit = 0;
        decimal totalcgst = 0;
        decimal totalsgst = 0;
        decimal totaligst = 0;
        decimal totaltds = 0;

        if (ViewState["CurrentTable"] != null)
        {
            DataTable dtCurrentTable = (DataTable)ViewState["CurrentTable"];
            DataRow drCurrentRow = null;
            if (dtCurrentTable.Rows.Count > 0)
            {
                for (int i = 1; i <= dtCurrentTable.Rows.Count; i++)
                {
                    DropDownList drpgDrCr = (DropDownList)gvdetails.Rows[rowIndex].Cells[1].FindControl("drpDrcr");
                    DropDownList drpgImpexp = (DropDownList)gvdetails.Rows[rowIndex].Cells[2].FindControl("drpImpexp");
                    TextBox txtJOBNO = (TextBox)gvdetails.Rows[rowIndex].Cells[3].FindControl("txtJOBNO");
                    TextBox txtgFileRefNo = (TextBox)gvdetails.Rows[rowIndex].Cells[4].FindControl("txtgFileRefNo");
                    TextBox txtTAX_INVNO_PS = (TextBox)gvdetails.Rows[rowIndex].Cells[5].FindControl("txtTAX_INVNO_PS");
                    TextBox txtgDate = (TextBox)gvdetails.Rows[rowIndex].Cells[6].FindControl("txtgDate");
                    TextBox txtgCusname = (TextBox)gvdetails.Rows[rowIndex].Cells[7].FindControl("txtgCusName");

                    TextBox txtgCharhead = (TextBox)gvdetails.Rows[rowIndex].Cells[8].FindControl("txtgCharHead");
                    DropDownList ddlgsttype = (DropDownList)gvdetails.Rows[rowIndex].Cells[9].FindControl("ddl_Gst_type");
                    TextBox txtgTaxrate = (TextBox)gvdetails.Rows[rowIndex].Cells[10].FindControl("txtgTaxrate");

                    TextBox txtgCreditfc = (TextBox)gvdetails.Rows[rowIndex].Cells[11].FindControl("txtgCreditfc");
                    TextBox txtgDebitfc = (TextBox)gvdetails.Rows[rowIndex].Cells[12].FindControl("txtgCreditfc");
                    TextBox txtgCredit1 = (TextBox)gvdetails.Rows[rowIndex].Cells[13].FindControl("txtgCredit1");
                    TextBox txtgDepit = (TextBox)gvdetails.Rows[rowIndex].Cells[14].FindControl("txtgDebit1");
                    TextBox txtgCgst = (TextBox)gvdetails.Rows[rowIndex].Cells[15].FindControl("txtgCgst");
                    TextBox txtgScgst = (TextBox)gvdetails.Rows[rowIndex].Cells[16].FindControl("txtgScgst");
                    TextBox txtgIgst = (TextBox)gvdetails.Rows[rowIndex].Cells[17].FindControl("txtgIgst");
                    TextBox txtgTds = (TextBox)gvdetails.Rows[rowIndex].Cells[18].FindControl("txtgTds");
                    TextBox txtgTdsamt = (TextBox)gvdetails.Rows[rowIndex].Cells[19].FindControl("txtgdtsamt");
                    FileUpload file = (FileUpload)gvdetails.Rows[rowIndex].Cells[20].FindControl("FileUpload1");
                    HiddenField hfFile = (HiddenField)gvdetails.Rows[rowIndex].Cells[20].FindControl("hfFileByte");
                    TextBox txtRemarks = (TextBox)gvdetails.Rows[rowIndex].Cells[21].FindControl("txtRemarks");
                  
                    if (txtgCredit1.Text != "")
                    {
                        totalcredit += Convert.ToDecimal(txtgCredit1.Text);
                    }
                    if (txtgDepit.Text != "")
                    {
                        totaldebit += Convert.ToDecimal(txtgDepit.Text);
                    }
                    if (txtgCgst.Text != "") { totalcgst += Convert.ToDecimal(txtgCgst.Text); }
                    if (txtgScgst.Text != "") { totalsgst += Convert.ToDecimal(txtgScgst.Text); }
                    if (txtgIgst.Text != "") { totaligst += Convert.ToDecimal(txtgIgst.Text); }
                    if (txtgTdsamt.Text != "") { totaltds += Convert.ToDecimal(txtgTdsamt.Text); }
                    ViewState["credit"] = totalcredit;
                    ViewState["debit"] = totaldebit;
                    ViewState["cgst"] = totalcgst;
                    ViewState["sgst"] = totalsgst;
                    ViewState["igst"] = totaligst;
                    ViewState["tdsamt"] = totaltds;

                 
                    dtCurrentTable.Rows[i - 1]["DR_CR"] = drpgDrCr.SelectedValue;
                    dtCurrentTable.Rows[i - 1]["Imp_Exp"] = drpgImpexp.SelectedValue;
                    dtCurrentTable.Rows[i - 1]["JobNo"] = txtJOBNO.Text;
                    dtCurrentTable.Rows[i - 1]["File_Ref_No"] = txtgFileRefNo.Text;
                    dtCurrentTable.Rows[i - 1]["TAX_INVNO_PS"] = txtTAX_INVNO_PS.Text;
                  //  dtCurrentTable.Rows[i - 1]["Date"] = txtgDate.Text;

                    dtCurrentTable.Rows[i - 1]["Customer_Name"] = txtgCusname.Text;

                    dtCurrentTable.Rows[i - 1]["Charge_Name"] = txtgCharhead.Text;
                    dtCurrentTable.Rows[i - 1]["Gst_Type"] = ddlgsttype.SelectedValue;
                    //dtCurrentTable.Rows[i - 1]["Tax_Rate"] = txtgTaxrate.Text;
                   // dtCurrentTable.Rows[i - 1]["Credit_FC"] = txtgCreditfc.Text;
                   // dtCurrentTable.Rows[i - 1]["Debit_FC"] = txtgDebitfc.Text;
                   // dtCurrentTable.Rows[i - 1]["Credit"] = txtgCredit1.Text;
                    //dtCurrentTable.Rows[i - 1]["Debit"] = txtgDepit.Text;
                   // dtCurrentTable.Rows[i - 1]["CGST"] = txtgCgst.Text;
                   // dtCurrentTable.Rows[i - 1]["SCGST"] = txtgScgst.Text;
                   // dtCurrentTable.Rows[i - 1]["IGST"] = txtgIgst.Text;
                   // dtCurrentTable.Rows[i - 1]["TDS"] = txtgTds.Text;
                  //  dtCurrentTable.Rows[i - 1]["TDS_Amount"] = txtgTdsamt.Text;
                    dtCurrentTable.Rows[i - 1]["FILE_NAME"] = "";
                    dtCurrentTable.Rows[i - 1]["Remarks"] = txtRemarks.Text;
                    rowIndex++;
                  
                }

                ViewState["CurrentTable"] = dtCurrentTable;
            }
        }
    }

    protected void OnRowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            DropDownList customerId = (e.Row.FindControl("drpDrcr") as DropDownList);
            if (customerId.Text == "CR")
            {
                TextBox txtdebit = (e.Row.FindControl("txtgDebit1") as TextBox);
                TextBox txtcredit = (e.Row.FindControl("txtgCredit1") as TextBox);

                txtdebit.ReadOnly = true;
                txtcredit.ReadOnly = false;
            }
            else if (customerId.Text == "DR")
            {
                TextBox txtdebit = (e.Row.FindControl("txtgDebit1") as TextBox);
                TextBox txtcredit = (e.Row.FindControl("txtgCredit1") as TextBox);
                txtdebit.ReadOnly = false;
                txtcredit.ReadOnly = true;
            }
        }
    }
    private void LoadUser()
    {
        DataSet ds = new DataSet();
        ObjUBO.ENAME = "Select_User";
        ObjUBO.BRANCH_CODE = currentbranch;
        ds = BI.PurchaseEntry(ObjUBO);
        if (ds.Tables[0].Rows.Count > 0)
        {
            ddl_Approved.DataSource = ds.Tables[0];
            ddl_Approved.DataTextField = "USER_NAME";
            ddl_Approved.DataValueField = "USER_NAME";
            ddl_Approved.DataBind();
        }

        ddl_Approved.Items.Insert(0, new System.Web.UI.WebControls.ListItem("--Select--", "--Select--"));
        ddl_Approved.SelectedIndex = 0;
        ds.Dispose();
        GC.Collect();
    }
    private void LoadVendor()
    {
        DataSet ds = new DataSet();
        ObjUBO.ENAME = "Select_Vendor";
        ObjUBO.BRANCH_CODE = currentbranch;
        ds = BI.PurchaseEntry(ObjUBO);
        if (ds.Tables[0].Rows.Count > 0)
        {
            ddlVendorname.DataSource = ds.Tables[0];
            ddlVendorname.DataTextField = "VEN_NAME";
            ddlVendorname.DataValueField = "VEN_NAME";
            ddlVendorname.DataBind();
        }

        ddlVendorname.Items.Insert(0, new System.Web.UI.WebControls.ListItem("--Select--", "--Select--"));
        ddlVendorname.SelectedIndex = 0;
        ds.Dispose();
        GC.Collect();
    }
    private void LoadVendor_Name()
    {
        DataSet ds = new DataSet();
        ObjUBO.ENAME = "Select_Vendor_Coloader";
        ObjUBO.BRANCH_CODE = currentbranch;
        ds = BI.PurchaseEntry(ObjUBO);
        if (ds.Tables[0].Rows.Count > 0)
        {
            ddlVendorname.DataSource = ds.Tables[0];
            ddlVendorname.DataTextField = "VEN_NAME";
            ddlVendorname.DataValueField = "VEN_NAME";
            ddlVendorname.DataBind();
        }

        ddlVendorname.Items.Insert(0, new System.Web.UI.WebControls.ListItem("--Select--", "--Select--"));
        ddlVendorname.SelectedIndex = 0;
        ds.Dispose();
        GC.Collect();
    }
    protected void ddlVendorname_SelectedIndexChanged(object sender, EventArgs e)
    {
        Load_VendorBranch();
        if (Connection.Company_License() == "ERF00027")
        {
            Load_VendorBranch_Coloader();
        }
    }
    protected void Voucherdate_changed(object sender, EventArgs e)
    {
        if (Hidden_Voucher_date.Value!= "")
        {
            //AUTO_JOBNO();
       }
       
    }
    
    private void Load_VendorBranch()
    {
        ddlVendorBranch.Items.Clear();
        DataSet ds = new DataSet();
        ObjUBO.ENAME = "Select_VendorLocation";
        ObjUBO.VOUCHER_NO = ddlVendorname.SelectedValue.ToString();
        ObjUBO.BRANCH_CODE = currentbranch;
        ds = BI.PurchaseEntry(ObjUBO);
        if (ds.Tables.Count > 0)
        {
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlVendorBranch.DataSource = ds.Tables[0];
                ddlVendorBranch.DataTextField = "BR_CODE_LOCATION";
                ddlVendorBranch.DataValueField = "BR_SL_NO";
                ddlVendorBranch.DataBind();
            }
            //ddlVendorBranch.Items.Insert(0, new System.Web.UI.WebControls.ListItem("--Select--", "--Select--"));
            //ddlVendorBranch.SelectedIndex = 0;
        }
        ds.Dispose();
        GC.Collect();
        load_BranchEtryDetails();
    }
    private void Load_VendorBranch_Coloader()
    {
        ddlVendorBranch.Items.Clear();
        DataSet ds = new DataSet();
        ObjUBO.ENAME = "Select_Coloader_Branch";
        ObjUBO.VOUCHER_NO = ddlVendorname.SelectedValue.ToString();
        ObjUBO.BRANCH_CODE = currentbranch;
        ds = BI.PurchaseEntry(ObjUBO);
        if (ds.Tables.Count > 0)
        {
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlVendorBranch.DataSource = ds.Tables[0];
                ddlVendorBranch.DataTextField = "CITY";
                ddlVendorBranch.DataValueField = "CITY";
                ddlVendorBranch.DataBind();
            }
            //ddlVendorBranch.Items.Insert(0, new System.Web.UI.WebControls.ListItem("--Select--", "--Select--"));
            //ddlVendorBranch.SelectedIndex = 0;
        }
        ds.Dispose();
        GC.Collect();
        load_BranchEtryDetails_Coloader();
    }
    private void load_BranchEtryDetails_Coloader()
    {
        DataSet ds = new DataSet();
        ObjUBO.ENAME = "Select_coloader_Location";
        ObjUBO.VOUCHER_NO = ddlVendorname.SelectedValue.ToString();
        ObjUBO.VENDOR_BRANCH = ddlVendorBranch.SelectedValue.ToString();
        ObjUBO.BRANCH_CODE = currentbranch;
        ds = BI.PurchaseEntry(ObjUBO);
        if (ds.Tables.Count > 0)
        {
            if (ds.Tables[0].Rows.Count > 0)
            {
                txtVendorstate.Text = ds.Tables[0].Rows[0]["BR_STATE"].ToString();
                txtGstn.Text = ds.Tables[0].Rows[0]["BR_GSTIN_NO"].ToString();
                txtGstntype.Text = ds.Tables[0].Rows[0]["TAX_TYPE"].ToString();
                txtCountry.Text = ds.Tables[0].Rows[0]["BR_COUNTRY_NAME"].ToString();
            }
        }
        ds.Dispose();
        GC.Collect();
    }
    protected void ddlVendorBranch_SelectedIndexChanged(object sender, EventArgs e)
    {
     
        if (Connection.Company_License() == "ERF00027")
        {
            load_BranchEtryDetails_Coloader();
        }
        else
        {
            load_BranchEtryDetails();
        }
    }
    private void load_BranchEtryDetails()
    {
        DataSet ds = new DataSet();
        ObjUBO.ENAME = "Select_VendorLocation";
        ObjUBO.VOUCHER_NO = ddlVendorname.SelectedValue.ToString();
        ObjUBO.VENDOR_BRANCH = ddlVendorBranch.SelectedValue.ToString();
        ObjUBO.BRANCH_CODE = currentbranch;
        ds = BI.PurchaseEntry(ObjUBO);
        if (ds.Tables.Count > 0)
        {
            if (ds.Tables[0].Rows.Count > 0)
            {
                txtVendorstate.Text = ds.Tables[1].Rows[0]["BR_STATE"].ToString();
                txtGstn.Text = ds.Tables[1].Rows[0]["BR_GSTIN_NO"].ToString();
                txtGstntype.Text = ds.Tables[2].Rows[0]["TAX_TYPE"].ToString();
                txtCountry.Text = ds.Tables[1].Rows[0]["BR_COUNTRY_NAME"].ToString();
            }
        }
        ds.Dispose();
        GC.Collect();
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        if (Convert.ToDecimal(txtTotalamtininr.Text) == Convert.ToDecimal(txtGrandtotal.Text))
        {
            if (Convert.ToDateTime(txtVoucherdate.Text) >= Convert.ToDateTime(txtBilldate.Text))
            {
                AUTO_JOBNO();
                save_update("INSERT");
            }
            else
            {
                string cleanMessage = "Bill date should be lesser than voucher date".Replace("'", "\'");
                Page page = HttpContext.Current.CurrentHandler as Page;
                string script = string.Format("alert('{0}');", cleanMessage);
                if (page != null && !page.ClientScript.IsClientScriptBlockRegistered("alert"))
                {
                    page.ClientScript.RegisterClientScriptBlock(page.GetType(), "alert", script, true /* addScriptTags */);
                }
            }
        }
        else
        {
            string cleanMessage = "Bill amount & Grand total amount not equal".Replace("'", "\'");
            Page page = HttpContext.Current.CurrentHandler as Page;
            string script = string.Format("alert('{0}');", cleanMessage);
            if (page != null && !page.ClientScript.IsClientScriptBlockRegistered("alert"))
            {
                page.ClientScript.RegisterClientScriptBlock(page.GetType(), "alert", script, true /* addScriptTags */);
            }
        }
        Hidden_Voucher_date.Value = "";
    }

    private void save_update(string name)
    {
        string expdate = "", billdate="";
        if (txtVoucherdate.Text != "")
        {
            expdate = txtVoucherdate.Text.Trim();
            expdate = expdate.ToString().Substring(3, 3) + expdate.ToString().Substring(0, 3) + expdate.ToString().Substring(6, 4);
        }
        if (txtBilldate.Text != "")
        {
            billdate = txtBilldate.Text.Trim();
            billdate = billdate.ToString().Substring(3, 3) + billdate.ToString().Substring(0, 3) + billdate.ToString().Substring(6, 4);
        }

        DataSet ds = new DataSet();
        DataTable dt = new DataTable();
        if (name == "UPDATE")
        {
            if (Hidden_VOUCHER_ID.Value != "")
            {
                ObjUBO.VOUCHER_ID = Hidden_VOUCHER_ID.Value;
            }
            else
            {
                ObjUBO.VOUCHER_ID = Request.QueryString["voucherno"].ToString();
            }
        }
        ObjUBO.VOUCHER_NO = txtVoucherno.Text;
        ObjUBO.VOUCHER_DATE = expdate;
        ObjUBO.VENDOR_NAME = ddlVendorname.SelectedItem.Text.ToString();
        ObjUBO.VENDOR_BRANCH = ddlVendorBranch.SelectedItem.Text.ToString();
        ObjUBO.VENDOR_STATE = txtVendorstate.Text;
        ObjUBO.GSTN = txtGstn.Text;
        ObjUBO.GSTN_TYPE = txtGstntype.Text;
        ObjUBO.COUNTRY = txtCountry.Text;
        ObjUBO.VENDOR_BILL = txtVendorbillno.Text;
        ObjUBO.BILL_DATE = billdate;
        ObjUBO.BILL_AMOUNT = (txtbillamount.Text == "" ? "0.00" : txtbillamount.Text); 
        ObjUBO.CURRENCY = txtInvoiceCurrency.Text;
        ObjUBO.EX_RATE = txtExrate.Text;
        ObjUBO.AMT_IN_INR = txtTotalamtininr.Text;
        ObjUBO.NARRATION = txtNarration.Text;
        ObjUBO.USER_ID = Connection.Current_User();
        ObjUBO.BRANCH_CODE = Connection.Current_Branch();
        ObjUBO.WORKING_PERIOD = Connection.WorkingPeriod();
        ObjUBO.BILL_TYPE = Rd_Bill_Type.SelectedValue.ToString();
        ObjUBO.JOB_PREFIX = hdprefix.Value.ToString();
        ObjUBO.JOB_SUFFIX = hdsuffix.Value.ToString();
        ObjUBO.VOUCHERNO_PS = txtVoucherno_ps.Text;
        ObjUBO.GRAND_TOTAL = txtGrandtotal.Text;
        ObjUBO.NET_AMOUNT = txtNetamt.Text;
        ObjUBO.WORKING_PERIOD_DETAILS = ddlworking_Period.SelectedValue;
        //ObjUBO.A62 = ddlworking_Period.SelectedValue;
        dt = gridtodt();
        ds.Tables.Add(dt);
        ObjUBO.XML_DETAIL = ds.GetXml();
        ObjUBO.ENAME = name ;
        ObjUBO.Approved_By = ddl_Approved.SelectedValue;
       
        ds = BI.PurchaseEntry(ObjUBO);
        string pathFolder = Server.MapPath("~/Submission/" + Session["COMPANY_ID"].ToString() + "/" + "PurchaseEntry");
        if (Directory.Exists(pathFolder) == false)
        {
            Directory.CreateDirectory(pathFolder);
        }
        foreach (GridViewRow row in gvdetails.Rows)
        {
            FileUpload file = row.FindControl("FileUpload1") as FileUpload;
            if (file.HasFile)
            {
                file.SaveAs(pathFolder + "/" + file.FileName);
            }
        }
       
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
                if (ds.Tables[0].Rows[0][0].ToString() == "1")
                {
                    btnUpdate.Visible = true;

                    btnDelete.Visible = true;
                    btnSave.Visible = false;
                    Hidden_VOUCHER_ID.Value = ds.Tables[1].Rows[0]["VOUCHER_ID"].ToString();
                    Update_Item_Load();
                }

                //DataTable dt1 = new DataTable();
                //gvdetails.DataSource = ds.Tables[1];
                //gvdetails.DataBind();
            }
        }

    }
    protected void btnUpdate_Click(object sender, EventArgs e)
    {
        if (Convert.ToDecimal(txtTotalamtininr.Text) == Convert.ToDecimal(txtGrandtotal.Text))
        {
            string a = Connection.Get_Company_Type();
            string b = Connection.Current_User();
            if (Convert.ToDateTime(txtVoucherdate.Text) >= Convert.ToDateTime(txtBilldate.Text))
            {

                save_update("UPDATE");
                if (BI.result == 1)
                {
                    string cleanMessage = "Updated Successfully".Replace("'", "\'");
                    Page page = HttpContext.Current.CurrentHandler as Page;
                    string script = string.Format("alert('{0}');", cleanMessage);
                    if (page != null && !page.ClientScript.IsClientScriptBlockRegistered("alert"))
                    {
                        page.ClientScript.RegisterClientScriptBlock(page.GetType(), "alert", script, true /* addScriptTags */);
                    }
                   
                }
                else { Alert_msg("Not Updated"); }
            }
            else
            {
                string cleanMessage = "Bill date should be lesser than voucher date".Replace("'", "\'");
                Page page = HttpContext.Current.CurrentHandler as Page;
                string script = string.Format("alert('{0}');", cleanMessage);
                if (page != null && !page.ClientScript.IsClientScriptBlockRegistered("alert"))
                {
                    page.ClientScript.RegisterClientScriptBlock(page.GetType(), "alert", script, true /* addScriptTags */);
                }
            }
        }
        else
        {
            string cleanMessage = "Bill amount & Grand total amount not equal".Replace("'", "\'");
            Page page = HttpContext.Current.CurrentHandler as Page;
            string script = string.Format("alert('{0}');", cleanMessage);
            if (page != null && !page.ClientScript.IsClientScriptBlockRegistered("alert"))
            {
                page.ClientScript.RegisterClientScriptBlock(page.GetType(), "alert", script, true /* addScriptTags */);
            }
        }
        
    }


    protected void btnDelete_Click(object sender, EventArgs e)
    {
        try
        {
            DataSet ds = new DataSet();
            if (Hidden_VOUCHER_ID.Value != "")
            {
                ObjUBO.VOUCHER_ID = Hidden_VOUCHER_ID.Value;
            }
            else
            {
                ObjUBO.VOUCHER_ID = Request.QueryString["voucherno"].ToString();
            }
            ObjUBO.ENAME = "DELETE";
             BI.PurchaseEntry(ObjUBO);
            if (BI.result == 1)
            {
                string cleanMessage = "Deleted Successfully".Replace("'", "\'");
                Page page = HttpContext.Current.CurrentHandler as Page;
                string script = string.Format("alert('{0}');", cleanMessage);
                if (page != null && !page.ClientScript.IsClientScriptBlockRegistered("alert"))
                {
                    page.ClientScript.RegisterClientScriptBlock(page.GetType(), "alert", script, true /* addScriptTags */);
                }
                //BI.ResetFields(Page.Controls);
            }
            else { Alert_msg("Not Updated"); }
          

        }
        catch (Exception ex)
        {
            Connection.Error_Msg(ex.Message);
        }
    }
    public void Alert_msg(string msg)
    {
        string prompt = "<script>$(document).ready(function(){{jAlert('" + msg + "', 'Purchase Entry', function (r) {});}});</script>";
        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", prompt, false);
    }
    public void Alert_msg(string msg, string focus)
    {
        string prompt = "<script>$(document).ready(function(){{jAlert('" + msg + "', 'Purchase Entry', function (r) {document.getElementById('" + focus + "').focus();});}});</script>";
        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", prompt, false);
    }

    private static string ConvertDataTableToXML(DataTable dtData)
    {
        DataSet dsData = new DataSet();
        StringBuilder sbSQL;
        StringWriter swSQL;
        string XMLformat;
        try
        {
            sbSQL = new StringBuilder();
            swSQL = new StringWriter(sbSQL);
            dsData.Merge(dtData, true, MissingSchemaAction.AddWithKey);
            dsData.Tables[0].TableName = "SampleDataTable";
            foreach (DataColumn col in dsData.Tables[0].Columns)
            {
                col.ColumnMapping = MappingType.Attribute;
            }
            dsData.WriteXml(swSQL, XmlWriteMode.WriteSchema);
            XMLformat = sbSQL.ToString();
            return XMLformat;
        }
        catch (Exception sysException)
        {
            throw sysException;
        }
    }

    private DataTable gridtodt()
    {
        string expdate = "";
        DataTable dt = new DataTable();
        if (gvdetails.Rows.Count != 0)
        {
            for (int i = 0; i < gvdetails.HeaderRow.Cells.Count-2; i++)
            {
                dt.Columns.Add(gvdetails.HeaderRow.Cells[i].Text);
            }
            dt.Columns.Add("filename");


            foreach (GridViewRow row in gvdetails.Rows)
            {
                DataRow dr = dt.NewRow();
                for (int j = 0; j < row.Cells.Count; j++)
                {
                    if (j == 0)
                    {
                        TextBox txtgSLNo = row.FindControl("txtgSLNo") as TextBox;
                        dr[gvdetails.HeaderRow.Cells[j].Text] = txtgSLNo.Text;
                    }
                    else if (j == 1)
                    {
                        DropDownList drpcdr = row.FindControl("drpDrcr") as DropDownList;
                        dr[gvdetails.HeaderRow.Cells[j].Text] = drpcdr.SelectedValue;

                    }
                    else if (j == 2)
                    {
                        DropDownList drpimexp = row.FindControl("drpImpexp") as DropDownList;
                        dr[gvdetails.HeaderRow.Cells[j].Text] = drpimexp.SelectedValue;

                    }
                    else if (j == 3)
                    {
                        TextBox txtJOBNO = row.FindControl("txtJOBNO") as TextBox;
                        dr[gvdetails.HeaderRow.Cells[j].Text] = txtJOBNO.Text;
                    }
                    else if (j == 4)
                    {
                        TextBox txtFilerefno = row.FindControl("txtgFileRefNo") as TextBox;
                        dr[gvdetails.HeaderRow.Cells[j].Text] = txtFilerefno.Text;
                    }

                    else if (j == 5)
                    {
                        TextBox txtTAX_INVNO_PS = row.FindControl("txtTAX_INVNO_PS") as TextBox;
                        dr[gvdetails.HeaderRow.Cells[j].Text] = txtTAX_INVNO_PS.Text;
                    }
                    else if (j == 6)
                    {
                        TextBox txtDate = row.FindControl("txtgDate") as TextBox;
                        
                        if (txtDate.Text != "")
                        {
                            expdate = txtDate.Text.Trim();
                            expdate = expdate.ToString().Substring(3, 3) + expdate.ToString().Substring(0, 3) + expdate.ToString().Substring(6, 4);
                            dr[gvdetails.HeaderRow.Cells[j].Text] = expdate;
                        }
                        else
                        {
                            dr[gvdetails.HeaderRow.Cells[j].Text] = "01/01/1900";
                        }


                    }
                   
                    else if (j == 7)
                    {
                        TextBox txtCusname = row.FindControl("txtgCusName") as TextBox;
                        dr[gvdetails.HeaderRow.Cells[j].Text] = txtCusname.Text;

                    }
                    else if (j ==8)
                    {
                        TextBox txtcharge = row.FindControl("txtgCharHead") as TextBox;
                        dr[gvdetails.HeaderRow.Cells[j].Text] = txtcharge.Text;
                    }
                    else if (j == 9)
                    {
                        DropDownList ddlgsttype = row.FindControl("ddl_Gst_type") as DropDownList;
                        dr[gvdetails.HeaderRow.Cells[j].Text] = ddlgsttype.SelectedValue;
                    }
                    else if (j == 10)
                    {
                        TextBox txtTaxrate = row.FindControl("txtgTaxrate") as TextBox;
                        if (txtTaxrate.Text == "")
                        { txtTaxrate.Text = "0"; }
                        dr[gvdetails.HeaderRow.Cells[j].Text] = txtTaxrate.Text;
                    }
                    else if (j == 11)
                    {
                        TextBox txtgCreditfc = row.FindControl("txtgCreditfc") as TextBox;
                        if (txtgCreditfc.Text == "")
                        { txtgCreditfc.Text = "0"; }
                        dr[gvdetails.HeaderRow.Cells[j].Text] = txtgCreditfc.Text;
                    }
                    else if (j == 12)
                    {
                        TextBox txtgDebitfc = row.FindControl("txtgDebitfc") as TextBox;
                        if (txtgDebitfc.Text == "")
                        { txtgDebitfc.Text = "0"; }
                        dr[gvdetails.HeaderRow.Cells[j].Text] = txtgDebitfc.Text;
                    }
                    else if (j == 13)
                    {
                        TextBox txtCredit = row.FindControl("txtgCredit1") as TextBox;
                        if (txtCredit.Text == "")
                        { txtCredit.Text = "0"; }
                        dr[gvdetails.HeaderRow.Cells[j].Text] = txtCredit.Text;
                    }
                    else if (j == 14)
                    {
                        TextBox txtDebit = row.FindControl("txtgDebit1") as TextBox;
                        if (txtDebit.Text == "")
                        { txtDebit.Text = "0"; }
                        dr[gvdetails.HeaderRow.Cells[j].Text] = txtDebit.Text;
                    }
                    else if (j == 15)
                    {
                        TextBox txtCgst = row.FindControl("txtgCgst") as TextBox;
                        if (txtCgst.Text == "")
                        { txtCgst.Text = "0"; }
                        dr[gvdetails.HeaderRow.Cells[j].Text] = txtCgst.Text;
                    }
                    else if (j == 16)
                    {
                        TextBox txtScgst = row.FindControl("txtgScgst") as TextBox;
                        if (txtScgst.Text == "")
                        { txtScgst.Text = "0"; }
                        dr[gvdetails.HeaderRow.Cells[j].Text] = txtScgst.Text;
                    }
                    else if (j == 17)
                    {
                        TextBox txtIgst = row.FindControl("txtgIgst") as TextBox;
                        if (txtIgst.Text == "")
                        { txtIgst.Text = "0"; }
                        dr[gvdetails.HeaderRow.Cells[j].Text] = txtIgst.Text;
                    }
                    else if (j == 18)
                    {
                        TextBox txtTds = row.FindControl("txtgTds") as TextBox;
                        if (txtTds.Text == "")
                        { txtTds.Text = "0"; }
                        dr[gvdetails.HeaderRow.Cells[j].Text] = txtTds.Text;
                    }
                    else if (j == 19)
                    {
                        TextBox txtTdsamt = row.FindControl("txtgdtsamt") as TextBox;
                        if (txtTdsamt.Text == "")
                        { txtTdsamt.Text = "0"; }
                        dr[gvdetails.HeaderRow.Cells[j].Text] = txtTdsamt.Text;
                    }
                    else if (j == 20)
                    {
                        TextBox txtRemarks = row.FindControl("txtRemarks") as TextBox;
                        if (txtRemarks.Text == "")
                        { txtRemarks.Text = "0"; }
                        dr[gvdetails.HeaderRow.Cells[j].Text] = txtRemarks.Text;
                    }
                    else if (j == 21)
                    {
                     
                        FileUpload Fileupl = row.FindControl("FileUpload1") as FileUpload;
                        Fileupl.Visible = true;
                        FileUpload file = row.FindControl("FileUpload1") as FileUpload;
                      

                        dr[gvdetails.HeaderRow.Cells[j].Text] = "~/Submission/" + Session["COMPANY_ID"].ToString() + "/" + "PurchaseEntry" + "/" + file.FileName;
                        dr["filename"] = file.FileName;
                    }

                }
                dt.Rows.Add(dr);
            }

        }
        return dt;
    }
    protected void DownloadFile(object sender, EventArgs e)
    {
        DataSet ds = new DataSet();
        int id = int.Parse((sender as LinkButton).CommandArgument);
        byte[] bytes;
        string fileName, contentType;
        ObjUBO.ENAME = "DW_SELECT";
        ObjUBO.VENDOR_BILL = Convert.ToString(id);
        ObjUBO.VOUCHER_NO = txtVoucherno.Text;
        ds = BI.PurchaseEntry(ObjUBO);

        string f = ds.Tables[0].Rows[0]["FILE_PATH"].ToString();
        fileName = ds.Tables[0].Rows[0]["FILE_NAME"].ToString();
        contentType = System.IO.Path.GetExtension(fileName);
        switch (contentType.ToLower())
        {
            case ".doc":
                contentType = "application/vnd.ms-word";
                break;
            case ".docx":
                contentType = "application/vnd.openxmlformats-officedocument.wordprocessingml.document";
                break;
            case ".xls":
                contentType = "application/vnd.ms-excel";
                break;
            case ".xlsx":
                contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                break;
            case ".jpg":
                contentType = "image/jpg";
                break;
            case ".png":
                contentType = "image/png";
                break;
            case ".gif":
                contentType = "image/gif";
                break;
            case ".pdf":
                contentType = "application/pdf";
                break;
            case ".txt":
                contentType = "text/plain";
                break;
        }

        Response.Clear();
        Response.Buffer = true;
        Response.Charset = "";
        Response.Cache.SetCacheability(HttpCacheability.NoCache);
        Response.ContentType = contentType;
        Response.AppendHeader("Content-Disposition", "attachment; filename="+fileName);
        Response.TransmitFile(Server.MapPath(f)); 
        Response.Flush();
        Response.End(); 
    }  
    [System.Web.Services.WebMethod]
    public static string GetExchange_Rate(string custid, string Inv_Curr, string Inv_Exrate)
    {
        if (custid.Length == 3)
        {
            if (custid == Inv_Curr)
            {
                custid = Inv_Exrate;
            }
            else
            {
                Auto_Search Auto = new Auto_Search();
                DataSet ds = new DataSet();
                Auto.Ename = "EXCHANGE_MASTER_RATE_EXPORTWISE";
                Auto.Enamesearch = custid;
                ds = Auto.RetrieveAll_AUTOCOMPLETE_SEARCH();
                if (ds.Tables[0].Rows.Count > 0)
                {
                    custid = ds.Tables[0].Rows[0]["EXCHANGE_RATE_EXPORT"].ToString();
                }
                else
                {
                    Auto.Ename = "NON_STD_CUR_RATE_EXPORTWISE";
                    Auto.Enamesearch = custid;
                    ds = Auto.ERoyalmaste_RetrieveAll_AUTOCOMPLETE_SEARCH();
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        string NON_STD_RATE_EXPORT = ds.Tables[0].Rows[0]["NON_STD_RATE_EXPORT"].ToString();
                        custid = NON_STD_RATE_EXPORT;
                    }
                    else

                        custid = "";
                }
            }
        }
        else
        {
            custid = "";
        }
        return custid;
    }
    [System.Web.Services.WebMethod]
    public static string GetExchange_Rate_Invoice_Cur(string custid)
    {
        if (custid.Length == 3)
        {
            Auto_Search Auto = new Auto_Search();
            DataSet ds = new DataSet();
            Auto.Ename = "EXCHANGE_MASTER_RATE_EXPORTWISE";
            Auto.Enamesearch = custid;
            ds = Auto.RetrieveAll_AUTOCOMPLETE_SEARCH();
            if (ds.Tables[0].Rows.Count > 0)
            {
                custid = ds.Tables[0].Rows[0]["EXCHANGE_RATE_EXPORT"].ToString();
            }
            else
            {
                Auto.Ename = "NON_STD_CUR_RATE_EXPORTWISE";
                Auto.Enamesearch = custid;
                ds = Auto.ERoyalmaste_RetrieveAll_AUTOCOMPLETE_SEARCH();
                if (ds.Tables[0].Rows.Count > 0)
                {


                    string NON_STD_RATE_EXPORT = ds.Tables[0].Rows[0]["NON_STD_RATE_EXPORT"].ToString();
                    string NON_STD_EFFECT_DT_EXP = ds.Tables[0].Rows[0]["NON_STD_EFFECT_DT_EXP"].ToString();
                    string NON_STD_BANKNAME = ds.Tables[0].Rows[0]["NON_STD_BANKNAME"].ToString();
                    string NON_STD_CERTIFICATE_NO = ds.Tables[0].Rows[0]["NON_STD_CERTIFICATE_NO"].ToString();
                    string NON_STD_CERTIFICATE_DATE = ds.Tables[0].Rows[0]["NON_STD_CERTIFICATE_DATE"].ToString();

                    custid = NON_STD_RATE_EXPORT + "~~" + NON_STD_EFFECT_DT_EXP + "~~" + NON_STD_BANKNAME + "~~" + NON_STD_CERTIFICATE_NO + "~~" + NON_STD_CERTIFICATE_DATE;
                }
                else

                    custid = "";
            }
        }
        else
        {
            custid = "";
        }
        return custid;
    }
    protected void txtVendorbillno_TextChanged(object sender, EventArgs e)
    {
        DataSet ds = new DataSet();
        ObjUBO.VENDOR_BILL = txtVendorbillno.Text;
        ObjUBO.ENAME = "VendorBill";
        ds = BI.PurchaseEntry(ObjUBO);
        if (ds.Tables.Count > 0)
        {
            Page page = HttpContext.Current.CurrentHandler as Page;
            string script = string.Format("alert('{0}');", ds.Tables[0].Rows[0][1].ToString());
            if (page != null && !page.ClientScript.IsClientScriptBlockRegistered("alert"))
            {
                page.ClientScript.RegisterClientScriptBlock(page.GetType(), "alert", script, true /* addScriptTags */);
            }
        }
    }
    protected void gvdetails_RowDeleting(object sender, CommandEventArgs e)
    {
        decimal totalcredit = 0;
        decimal totaldebit = 0;
        decimal totalcgst = 0;
        decimal totalsgst = 0;
        decimal totaligst = 0;
        decimal totaltds = 0;
        SetRowData();
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
                gvdetails.DataSource = dt;
                gvdetails.DataBind();
               

                for (int i = 0; i < gvdetails.Rows.Count ; i++)
                {
                    gvdetails.Rows[i].Cells[0].Text = Convert.ToString(i + 1);
                    TextBox txtgCredit1 = (TextBox)gvdetails.Rows[i].Cells[9].FindControl("txtgCredit1");
                    TextBox txtgDepit = (TextBox)gvdetails.Rows[i].Cells[10].FindControl("txtgDebit1");
                    TextBox txtgCgst = (TextBox)gvdetails.Rows[i].Cells[11].FindControl("txtgCgst");
                    TextBox txtgScgst = (TextBox)gvdetails.Rows[i].Cells[12].FindControl("txtgScgst");
                    TextBox txtgIgst = (TextBox)gvdetails.Rows[i].Cells[13].FindControl("txtgIgst");
                    TextBox txtgTds = (TextBox)gvdetails.Rows[i].Cells[14].FindControl("txtgTds");
                    TextBox txtgTdsamt = (TextBox)gvdetails.Rows[i].Cells[15].FindControl("txtgdtsamt");
                    if (txtgCredit1.Text != "")
                    {
                        totalcredit += Convert.ToDecimal(txtgCredit1.Text);
                    }
                    if (txtgDepit.Text != "")
                    {
                        totaldebit += Convert.ToDecimal(txtgDepit.Text);
                    }
                    if (txtgCgst.Text != "") { totalcgst += Convert.ToDecimal(txtgCgst.Text); }
                    if (txtgScgst.Text != "") { totalsgst += Convert.ToDecimal(txtgScgst.Text); }
                    if (txtgIgst.Text != "") { totaligst += Convert.ToDecimal(txtgIgst.Text); }
                    if (txtgTdsamt.Text != "") { totaltds += Convert.ToDecimal(txtgTdsamt.Text); }

                }
                TextBox txtfcredittotal = (TextBox)gvdetails.FooterRow.FindControl("txtcredittotal");
                TextBox txtfdebittotal = (TextBox)gvdetails.FooterRow.FindControl("txtdebittotal");
                TextBox txtfcgsttotal = (TextBox)gvdetails.FooterRow.FindControl("txtcgsttotal");
                TextBox txtfsgsttotal = (TextBox)gvdetails.FooterRow.FindControl("txtsgsttotal");
                TextBox txtfigsttotal = (TextBox)gvdetails.FooterRow.FindControl("txtigsttotal");
                TextBox txtftdstotal = (TextBox)gvdetails.FooterRow.FindControl("txttdstotal");
                txtfcredittotal.Text = totalcredit.ToString();
                txtfdebittotal.Text = totaldebit.ToString();
                txtfcgsttotal.Text = totalcgst.ToString();
                txtfsgsttotal.Text = totalsgst.ToString();
                txtfigsttotal.Text = totaligst.ToString();
                txtftdstotal.Text = totaltds.ToString();
                txtNetamt.Text = Convert.ToString(totaldebit + totalcgst + totaligst + totalsgst - totaltds);
                txtGrandtotal.Text = Convert.ToString(totaldebit + totalcgst + totaligst + totalsgst ); 
            }
        }
    }
     [System.Web.Services.WebMethod]
    //[WebMethod(EnableSession = true)]
    public static string Cust_Name(string ChargeName, string Imp_Name, string Mode,string Comp_Det_Des)
    {
        string cname="";
         DataSet ds=new DataSet();
          Purchase_cs BI = new Purchase_cs();
          Billing_UserBO ObjUBO = new Billing_UserBO();

         ObjUBO.ENAME="CUST_NAME";
         ObjUBO.VENDOR_NAME = ChargeName;
         ObjUBO.VOUCHER_NO = Imp_Name;


        

         if (HttpContext.Current.Session["COMPANY_DETAILS_DS"] != null)
         {
             DataSet dss = new DataSet();
             dss = (DataSet)HttpContext.Current.Session["COMPANY_DETAILS_DS"];
             string DB_NAME = "";
             DataTable ds3 = new DataTable();
             ds3 = dss.Tables[2];
             DataView view1 = ds3.DefaultView;
             string a = Comp_Det_Des;
             view1.RowFilter = "WORKING_PERIOD = '" + a + "'";
             DataTable table1 = view1.ToTable();
             DB_NAME = table1.Rows[0]["SERVER_NAME"].ToString();
           
             ds = BI.PurchaseEntry_BD_Name(ObjUBO, DB_NAME);
         }
        
         //ds = BI.PurchaseEntry(ObjUBO, DB_NAME);

         
         if (ds.Tables[0].Rows.Count > 0)
         {
             cname = ds.Tables[0].Rows[0]["CONSIGNEENAME"].ToString() + "~~" + ds.Tables[0].Rows[0]["JOBDATE"].ToString() + "~~" +
                 ds.Tables[0].Rows[0]["FILE_REF_NO"].ToString() + "~~" + ds.Tables[0].Rows[0]["TAX_INVNO_PS"].ToString();
         }
         else
         {
             cname = "";
         }

         return cname;
    }
     //protected void txtJOBNO_TextChanged(object sender, EventArgs e)
     //{
     //    DataSet ds = new DataSet();
     //    GridViewRow currentRow = ((GridViewRow)((TextBox)sender).NamingContainer);
     //    TextBox txtgFileRef = (TextBox)currentRow.FindControl("txtJOBNO");
     //    TextBox txtgCusName = (TextBox)currentRow.FindControl("txtgCusName");
     //    //string refno =txtgFileRef.Text; 


     //    //txtgCusName.Text = "PARAMOUNT";
     //}
     protected void txtgCharHead_TextChanged(object sender, EventArgs e)
     {
         GridViewRow currentRow = ((GridViewRow)((TextBox)sender).NamingContainer);
         TextBox txtgcharh = (TextBox)currentRow.FindControl("txtgCharHead");
         TextBox txtgcgst = (TextBox)currentRow.FindControl("txtgCgst");
         TextBox txtgsgst = (TextBox)currentRow.FindControl("txtgScgst");
         TextBox txtgigst = (TextBox)currentRow.FindControl("txtgIgst");
         TextBox txtgtds = (TextBox)currentRow.FindControl("txtgdtsamt");
         //txtgcharh.Text = "AAI EXPENSES";
         //txtgcgst.Text = "2";
         //txtgsgst.Text = "3";
         //txtgigst.Text = "2";
         //txtgtds.Text = "";
             
         
     }
    
     //[System.Web.Services.WebMethod]
     //public static string Get_Cha_rate(string ChargeName, string Imp_Name, string Mode)
     //{
     //    string Ch_Name = "";
     //    DataSet ds = new DataSet();
     //    try
     //    {
     //        GST_Imp_Invoice BI = new GST_Imp_Invoice();
     //        Billing_UserBO ObjUBO = new Billing_UserBO();

     //        ObjUBO.charge_name = ChargeName;
     //        ObjUBO.Flag = "select_charge";
     //        ds = BI.Select_Char_val(ObjUBO);

     //        if (ds.Tables[0].Rows.Count > 0)
     //        {
     //            Ch_Name = ds.Tables[0].Rows[0]["CGST_RATE"].ToString() + "~~" + ds.Tables[0].Rows[0]["SGST_RATE"].ToString() + "~~" + ds.Tables[0].Rows[0]["IGST_RATE"].ToString() + "~~" + ds.Tables[0].Rows[0]["TDS_RATE"].ToString() + "~~" + ds.Tables[0].Rows[0]["SA_CODE"].ToString();
     //        }
     //        else
     //        {
     //            Ch_Name = "";
     //        }
     //    }
     //    catch (Exception ex)
     //    {
     //        Connection.Error_Msg(ex.Message);
     //    }
     //    return Ch_Name;
     //}
     [System.Web.Services.WebMethod]
     public static string Get_Cha_exrate(string currency, string IE)
     {
         string exrate = "";
         DataSet ds = new DataSet();
         try
         {
             eroyalmaster BI = new eroyalmaster();


             if (IE == "Imp FW Air" || IE == "Imp FW Sea" || IE == "Imp CL Air" || IE == "Imp CL Sea" || IE == "Imp CC Air" || IE == "Imp CC Sea")
             {
                 BI.Eexchange_nont_no = currency;
                 BI.Ename = "I_EXG_RATE";
             }
             else if (IE == "Exp FW Air" || IE == "Exp FW Sea" || IE == "Exp CL Air" || IE == "Exp CL Sea" || IE == "Exp CC Air" || IE == "Exp CC Sea")
             {
                 BI.Eexchange_nont_no = currency;
                 BI.Ename = "E_EXG_RATE";

             }

             ds = BI.RetrieveAll_Exchange_master();
             if (BI.result == 1)
             {

                 if (ds.Tables[0].Rows.Count > 0)
                 {
                     exrate = ds.Tables[0].Rows[0]["EX_RATE"].ToString();
                 }
                 else
                 {
                     exrate = "";
                 }
             }
             else if (BI.result == 2)
             {

             }
             //------------------------------------
         }
         catch (Exception ex)
         {
             Connection.Error_Msg(ex.Message);
         }
         return exrate;
     }
     [System.Web.Services.WebMethod]
     public static string Get_Cha_rate(string ChargeName, string Imp_Name, string Mode)
     {
         string Ch_Name;
         DataSet ds = new DataSet();
        
             Purchase_cs BI = new Purchase_cs();
             Billing_UserBO ObjUBO = new Billing_UserBO();

             ObjUBO.VENDOR_NAME = ChargeName;
             ObjUBO.ENAME = "CHARGE_DETAILS";
             ds = BI.PurchaseEntry(ObjUBO);

             if (ds.Tables[0].Rows.Count > 0)
             {
                 Ch_Name = ds.Tables[0].Rows[0]["CGST_RATE"].ToString() + " ~~" + ds.Tables[0].Rows[0]["SGST_RATE"].ToString() + " ~~" + ds.Tables[0].Rows[0]["IGST_RATE"].ToString() + " ~~" + ds.Tables[0].Rows[0]["TDS_RATE"].ToString() + " ~~" + ds.Tables[0].Rows[0]["TAX_MODE"].ToString();
             }
             else
             {
                 Ch_Name = "";
             }
         
         return ChargeName = Ch_Name;
     }

     [System.Web.Services.WebMethod]
     public static string Get_Vendordetail(string VendorName, string VendorBranch, string Mode)
     {
         string Vendor_Name = "";
         DataSet ds = new DataSet();
         try
         {
             GST_Imp_Invoice BI = new GST_Imp_Invoice();
             Billing_UserBO ObjUBO = new Billing_UserBO();

             ObjUBO.VENDOR_NAME = VendorName;
             ObjUBO.VENDOR_BRANCH = VendorBranch;
             ObjUBO.Flag = "SELECT";
             ds = BI.Select_VendorDetaill(ObjUBO);

             if (ds.Tables[0].Rows.Count > 0)
             {
                 Vendor_Name = ds.Tables[0].Rows[0]["BR_STATE"].ToString() + "~~" + ds.Tables[0].Rows[0]["BR_GSTIN_NO"].ToString() + "~~" + ds.Tables[0].Rows[0]["BR_TAX_TYPE"].ToString() + "~~" + ds.Tables[0].Rows[0]["BR_COUNTRY_NAME"].ToString() + "~~" + ds.Tables[1].Rows[0]["TAX_TYPE"].ToString();
             }
             else
             {
                 Vendor_Name = "";
             }
         }
         catch (Exception ex)
         {
             Connection.Error_Msg(ex.Message);
         }
         return Vendor_Name;
     }
     private void Update_Item_Load()
     {
         decimal totalcredit = 0;
         decimal totaldebit = 0;
         decimal totalcgst = 0;
         decimal totalsgst = 0;
         decimal totaligst = 0;
         decimal totaltds = 0;
        
         try
         {

             DataSet dss = new DataSet();
             if (Hidden_VOUCHER_ID.Value != "")
             {
                 ObjUBO.VOUCHER_ID = Hidden_VOUCHER_ID.Value;
             }
             else
             {
                 ObjUBO.VOUCHER_ID = Request.QueryString["voucherno"].ToString();
               
             }
           
             ObjUBO.ENAME = "SELETG";
             dss = BI.PurchaseEntry(ObjUBO);
             if (dss.Tables[0].Rows.Count > 0)
             {
                 if (dss.Tables[0].Rows[0]["BILL_TYPE"].ToString() == "L")
                 {
                     Rd_Bill_Type.SelectedIndex = 0;
                 }
                 else
                 { Rd_Bill_Type.SelectedIndex = 1; }
                 LoadVendor();
                 txtVoucherno.Text = dss.Tables[0].Rows[0]["VOUCHER_NO"].ToString();
                 txtVoucherno_ps.Text = dss.Tables[0].Rows[0]["VOUCHERNO_PS"].ToString();
                 ddlVendorname.SelectedValue = dss.Tables[0].Rows[0]["VENDOR_NAME"].ToString();
                 Load_VendorBranch();
                 txtVoucherdate.Text = dss.Tables[0].Rows[0]["VOUCHER_DATE"].ToString();
                 ddlVendorBranch.SelectedItem.Text = dss.Tables[0].Rows[0]["LOCATION"].ToString();
                 txtVendorstate.Text = dss.Tables[0].Rows[0]["STATE_CODE"].ToString();
                 txtGstn.Text = dss.Tables[0].Rows[0]["GSTN"].ToString();
                 txtGstntype.Text = dss.Tables[0].Rows[0]["GSTN_TYPE"].ToString();
                 txtCountry.Text = dss.Tables[0].Rows[0]["COUNTRY_CODE"].ToString();
                 txtNarration.Text = dss.Tables[0].Rows[0]["NARRATION"].ToString();
                 txtVendorbillno.Text = dss.Tables[0].Rows[0]["VENDOR_BILL"].ToString();
                 txtBilldate.Text = dss.Tables[0].Rows[0]["BILL_DATE"].ToString();
                 txtbillamount.Text = dss.Tables[0].Rows[0]["BILL_AMOUNT"].ToString();
                 txtInvoiceCurrency.Text = dss.Tables[0].Rows[0]["CURRENCY"].ToString();
                 txtExrate.Text = dss.Tables[0].Rows[0]["EX_RATE"].ToString();
                 txtTotalamtininr.Text = dss.Tables[0].Rows[0]["AMT_IN_INR"].ToString();
                 txtGrandtotal.Text = dss.Tables[0].Rows[0]["GRAND_TOTAL"].ToString();
                 txtNetamt.Text = dss.Tables[0].Rows[0]["NET_AMOUNT"].ToString();
                 ddl_Approved.SelectedValue = dss.Tables[0].Rows[0]["Approved_By"].ToString();
                 ddlworking_Period.SelectedValue = dss.Tables[0].Rows[0]["WORKING_PERIOD_DETAILS"].ToString();
                 Hidden_VOUCHER_ID.Value = dss.Tables[0].Rows[0]["VOUCHER_ID"].ToString();
                 Hidden_Voucher_date.Value = "";
             }
             ViewState["CurrentTable"] = dss.Tables[1];

             if (dss.Tables[1].Rows.Count > 0)
             {
                 gvdetails.DataSource = dss.Tables[1];
                 gvdetails.DataBind();

                 for (int i = 0; i < gvdetails.Rows.Count; i++)
                 {
                     gvdetails.Rows[i].Cells[0].Text = Convert.ToString(i + 1);
                     string FILE=dss.Tables[1].Rows[i]["FILE_NAME"].ToString();
                     LinkButton lnkBtn = (LinkButton)gvdetails.Rows[i].FindControl("LinkButton1"); 
                     lnkBtn.Visible = true; 
                     FileUpload Fileupl = (FileUpload)gvdetails.Rows[i].FindControl("FileUpload1");
                     if (FILE != "")
                     {
                         Fileupl.Visible = true;
                     }
                     else
                     {
                         Fileupl.Visible = true;
                     }

                     TextBox txtJOBNO = (TextBox)gvdetails.Rows[i].Cells[1].FindControl("txtJOBNO");
                     TextBox txtgCredit1 = (TextBox)gvdetails.Rows[i].Cells[9].FindControl("txtgCredit1");
                     TextBox txtgDepit = (TextBox)gvdetails.Rows[i].Cells[10].FindControl("txtgDebit1");
                     TextBox txtgCgst = (TextBox)gvdetails.Rows[i].Cells[11].FindControl("txtgCgst");
                     TextBox txtgScgst = (TextBox)gvdetails.Rows[i].Cells[12].FindControl("txtgScgst");
                     TextBox txtgIgst = (TextBox)gvdetails.Rows[i].Cells[13].FindControl("txtgIgst");
                     TextBox txtgTds = (TextBox)gvdetails.Rows[i].Cells[14].FindControl("txtgTds");
                     TextBox txtgTdsamt = (TextBox)gvdetails.Rows[i].Cells[15].FindControl("txtgdtsamt");
                     if (txtgCredit1.Text != "")
                     {
                         totalcredit += Convert.ToDecimal(txtgCredit1.Text);
                     }
                     if (txtgDepit.Text != "")
                     {
                         totaldebit += Convert.ToDecimal(txtgDepit.Text);
                     }
                     if (txtgCgst.Text != "") { totalcgst += Convert.ToDecimal(txtgCgst.Text); }
                     if (txtgScgst.Text != "") { totalsgst += Convert.ToDecimal(txtgScgst.Text); }
                     if (txtgIgst.Text != "") { totaligst += Convert.ToDecimal(txtgIgst.Text); }
                     if (txtgTdsamt.Text != "") { totaltds += Convert.ToDecimal(txtgTdsamt.Text); }
                 //     if (txtJOBNO.Text != "")
                 //    {


                 //        txtJOBNO.Attributes.Add("readonly", "readonly");
                         
                 //}
                 }
                  
                 //TextBox txtfcredittotal = (TextBox)gvdetails.FooterRow.FindControl("txtcredittotal");
                 //TextBox txtfdebittotal = (TextBox)gvdetails.FooterRow.FindControl("txtdebittotal");
                 //TextBox txtfcgsttotal = (TextBox)gvdetails.FooterRow.FindControl("txtcgsttotal");
                 //TextBox txtfsgsttotal = (TextBox)gvdetails.FooterRow.FindControl("txtsgsttotal");
                 //TextBox txtfigsttotal = (TextBox)gvdetails.FooterRow.FindControl("txtigsttotal");
                 //TextBox txtftdstotal = (TextBox)gvdetails.FooterRow.FindControl("txttdstotal");
                 //txtfcredittotal.Text = totalcredit.ToString();
                 //txtfdebittotal.Text = totaldebit.ToString();
                 //txtfcgsttotal.Text = totalcgst.ToString();
                 //txtfsgsttotal.Text = totalsgst.ToString();
                 //txtfigsttotal.Text = totaligst.ToString();
                 //txtftdstotal.Text = totaltds.ToString();
                 //txtNetamt.Text = Convert.ToString(totaldebit + totalcgst + totaligst + totalsgst - totaltds);
                 //txtGrandtotal.Text = Convert.ToString(totaldebit + totalcgst + totaligst + totalsgst );
                 if (COMPANY_LICENSE == "erf00026")
                 {
                     if (currentuser.ToUpper() == "ACCOUNTS" || currentuser.ToUpper() == "BOMACC" || currentuser.ToUpper() == "ACCOUNTS3") { btnDelete.Visible = true; }
                     else { btnDelete.Visible = false; }
                 }
                 else
                 {
                     btnDelete.Visible = true;
                 }
             }
             dss.Dispose();
             GC.Collect();
         }


         catch (Exception ex)
         {
             Connection.Error_Msg(ex.Message);
         }
     }
     protected void gvbind_RowCreated(object sender, GridViewRowEventArgs e)
     {
         if (e.Row.RowType == DataControlRowType.Header)
             e.Row.CssClass = "header";
     }
     public void AUTO_JOBNO()
     {
       
         ObjUBO.BRANCH_CODE = Connection.Current_Branch();
         ObjUBO.WORKING_PERIOD = Connection.WorkingPeriod();
         string types = "Purchase_Preffix_Suffix";
         ObjUBO.ENAME = "VOURCHERID";
         ObjUBO.VOUCHER_DATE = txtVoucherdate.Text;
         BI.PurchaseEntry(ObjUBO);
         txtVoucherno.Text = txtVoucherno_ps.Text = BI.result.ToString();
         JobNo_Prefix_Sufix(types);
         Session["Jobno_Excel"] = txtVoucherno.Text;
     }
     public void JobNo_Prefix_Sufix(string types)
     {
         Admin_job_Prefix_Suffix JPS = new Admin_job_Prefix_Suffix();
         DataSet dats = new DataSet();
         JPS.Branch = Connection.Current_Branch();
         JPS.Working_period = Connection.WorkingPeriod();
         //JPS.Tr_mode = ddlTransportMode.SelectedItem.Text;
         JPS.Ename = types;
         dats = JPS.Job_prefix_Suffix_Select();

         int Jno = 0;
         string JobNo = string.Empty;
         JobNo = txtVoucherno_ps.Text;
         if (dats.Tables[0].Rows.Count > 0)
         {
             string jseries = dats.Tables[0].Rows[0][2].ToString();
             if (JobNo.Length != jseries.Length)
             {
                 int L = jseries.Length - JobNo.Length;
                 if (L == 1)
                 {
                     JobNo = "0" + txtVoucherno_ps.Text;
                 }
                 else if (L == 2)
                 {
                     JobNo = "00" + txtVoucherno_ps.Text;
                 }
                 //else
                 //{
                 //    JobNo = "000" + txtVoucherno_ps.Text;
                 //}
             }
         }
         //if (JobNo != string.Empty)
         //{
         //    Jno = Convert.ToInt32(JobNo);
         //    if (Jno < 1000)
         //    {
         //        if (Jno > 100)
         //        {
         //            JobNo = "0" + txtjobps.Text;
         //        }
         //        else if (Jno > 10)
         //        {
         //            JobNo = "00" + txtjobps.Text;
         //        }
         //        else
         //        {
         //            JobNo = "000" + txtjobps.Text;
         //        }
         //    }
         //}

         if (Session["COMPANY_LICENSE"] != null)
         {

             if (dats.Tables[0].Rows.Count > 0)
             {
                 if (dats.Tables[0].Rows[0][0].ToString() == string.Empty && dats.Tables[0].Rows[0][1].ToString() != string.Empty)
                 {
                     txtVoucherno_ps.Text = txtVoucherno.Text + "/" + ((dats.Tables[0].Rows[0][1].ToString()).TrimStart()).TrimEnd();
                     hdprefix.Value = string.Empty;
                     hdsuffix.Value = ((dats.Tables[0].Rows[0][1].ToString()).TrimStart()).TrimEnd();
                 }
                 else if (dats.Tables[0].Rows[0][0].ToString() != string.Empty && dats.Tables[0].Rows[0][1].ToString() == string.Empty)
                 {
                     string sMonth;
                     if (txtVoucherdate.Text == DateTime.Now.ToShortDateString())
                     {
                         sMonth = DateTime.Now.ToString("MM");
                     }
                     else
                     {
                         string[] M =  txtVoucherdate.Text.Split('/');
                         sMonth = M[1].Trim().ToString();
                     }
                     string S = Connection.WorkingPeriod();
                     string[] T = S.Split('-');
                     string Val = (T[2].Trim() + '-' + T[5].Trim()).Replace("20", "").ToString();

                     txtVoucherno_ps.Text = ((dats.Tables[0].Rows[0][0].ToString() + "/" + sMonth + "/" + Val + "/" + JobNo).TrimEnd()).TrimStart();

                     hdprefix.Value = ((dats.Tables[0].Rows[0][0].ToString() + "/" + sMonth + "/" + Val).TrimEnd()).TrimStart();
                     hdsuffix.Value = "";
                 }
                 else if (dats.Tables[0].Rows[0][0].ToString() != string.Empty && dats.Tables[0].Rows[0][1].ToString() != string.Empty)
                 {
                     string sMonth;
                     if (txtVoucherdate.Text == DateTime.Now.ToShortDateString())
                     {
                          sMonth = DateTime.Now.ToString("MM");
                     }
                     else
                     {
                         string[] M = txtVoucherdate.Text.Split('/');
                         sMonth = M[1].Trim().ToString();
                     }
                     string S=Connection.WorkingPeriod();
                     string []T = S.Split('-');
                     string Val = (T[2].Trim() + '-' + T[5].Trim()).Replace("20", "").ToString(); 
                  
                     txtVoucherno_ps.Text = ((dats.Tables[0].Rows[0][0].ToString() + "/" + sMonth + "/" + Val + "/" + JobNo).TrimEnd()).TrimStart();

                     hdprefix.Value = ((dats.Tables[0].Rows[0][0].ToString() + "/" + sMonth + "/" + Val).TrimEnd()).TrimStart();
                     hdsuffix.Value = "";
                 }
                 else if (dats.Tables[0].Rows[0][0].ToString() == string.Empty && dats.Tables[0].Rows[0][1].ToString() == string.Empty)
                 {

                     hdprefix.Value = string.Empty;
                     hdsuffix.Value = string.Empty;
                 }
             }
             else
             {

                 hdprefix.Value = string.Empty;
                 hdsuffix.Value = string.Empty;
             }

         }
     }
     protected void btnNew_Click(object sender, EventArgs e)
     {


         Transact.ResetFields(Page.Controls);
         txtVoucherdate.Text = DateTime.Now.ToShortDateString();
         //AUTO_JOBNO();
         LoadUser();
         LoadVendor();
         string a = Connection.Company_License();
         if (Connection.Company_License() == "ERF00027")
         {
             LoadVendor_Name();
         }
         btnSave.Visible = true;
         btnUpdate.Visible = false;
         btnDelete.Visible = false; 
         txtInvoiceCurrency.Attributes.Add("onblur", "javascript:Call__Inv_Currency_Excahnge_Rate('" + txtInvoiceCurrency.ClientID + "', '" + txtExrate.ClientID + "')");

         ViewState["RowNumber"] = 1;
         DataTable dt = new DataTable();
         DataRow dr = dt.NewRow();
         DropDownList drp = new DropDownList();
         dt.Columns.Add(new DataColumn("SNo", typeof(string)));
         dt.Columns.Add(new DataColumn("DR_CR", typeof(string)));
         dt.Columns.Add(new DataColumn("Imp_Exp", typeof(string)));

         dt.Columns.Add(new DataColumn("JobNo", typeof(string)));
         dt.Columns.Add(new DataColumn("File_Ref_No", typeof(string)));
         dt.Columns.Add(new DataColumn("TAX_INVNO_PS", typeof(string)));
         dt.Columns.Add(new DataColumn("Date", typeof(string)));

         dt.Columns.Add(new DataColumn("Customer_Name", typeof(string)));

         dt.Columns.Add(new DataColumn("Charge_Name", typeof(string)));
         dt.Columns.Add(new DataColumn("Gst_Type", typeof(string)));
         dt.Columns.Add(new DataColumn("Tax_Rate", typeof(string)));
         dt.Columns.Add(new DataColumn("Credit_FC", typeof(string)));
         dt.Columns.Add(new DataColumn("Debit_FC", typeof(string)));
         dt.Columns.Add(new DataColumn("Credit", typeof(string)));
         dt.Columns.Add(new DataColumn("Debit", typeof(string)));
         dt.Columns.Add(new DataColumn("CGST", typeof(string)));
         dt.Columns.Add(new DataColumn("SCGST", typeof(string)));
         dt.Columns.Add(new DataColumn("IGST", typeof(string)));
         dt.Columns.Add(new DataColumn("TDS", typeof(string)));
         dt.Columns.Add(new DataColumn("TDS_Amount", typeof(string)));

         dt.Columns.Add(new DataColumn("Remarks", typeof(string)));
         dt.Columns.Add(new DataColumn("FileUp", typeof(string)));
         dt.Columns.Add(new DataColumn("FILE_NAME", typeof(string)));
         dt.Rows.Add(dr);
         dr["DR_CR"] = new ListItem("DR");
         dr["Imp_exp"] = new ListItem("---");
         dr["SNo"] = ViewState["RowNumber"].ToString();
         dr["Gst_Type"] = new ListItem("");

         ViewState["CurrentTable"] = dt;
         gvdetails.Visible = true;
         gvdetails.DataSource = dt;
         gvdetails.DataBind();
         LinkButton lnkBtn = (LinkButton)gvdetails.Rows[0].FindControl("LinkButton1");
         lnkBtn.Visible = false;
         FileUpload Fileupl = (FileUpload)gvdetails.Rows[0].FindControl("FileUpload1");
         Fileupl.Visible = true; 
     }
     
}