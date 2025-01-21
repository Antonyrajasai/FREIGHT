using System;
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

public partial class DebitNoteEntry : ThemeClass
{
    public string currentbranch;

    User_Creation user_Create = new User_Creation();
    AppSession aps = new AppSession();

    DebitNote_cs BI = new DebitNote_cs();
    Billing_UserBO ObjUBO = new Billing_UserBO();
    protected void Page_Load(object sender, EventArgs e)
    {
        CalendarExtender1.EndDate = DateTime.Now;
        DataSet ds = new DataSet();
        aps.checkSession(); 
        currentbranch = Session["currentbranch"].ToString();  
        
        if (!IsPostBack)
        {
            if (Request.QueryString["voucherno"] != null && Request.QueryString["voucherno"] != string.Empty)
            {
                Update_Item_Load();                
                btnSave.Visible = false;
                btnUpdate.Visible = true;

                txtInvoiceCurrency.Attributes.Add("onblur", "javascript:Call__Inv_Currency_Excahnge_Rate('" + txtInvoiceCurrency.ClientID + "', '" + txtExrate.ClientID + "')");
                txtInvoiceCurrency.Text = "INR";
                txtExrate.Text = "1";
            }
            else
            {
                LoadVendor();
                btnSave.Visible = true;
                btnUpdate.Visible = false;
                ObjUBO.ENAME = "DEBIT_NOTE_ID";
                ObjUBO.BRANCH_CODE = currentbranch;
                ObjUBO.WORKING_PERIOD = Connection.WorkingPeriod();
                ds = BI.DebitNoteEntry(ObjUBO);
                if (ds.Tables[1].Rows.Count > 0 && ds.Tables[2].Rows.Count > 0 && ds.Tables[0].Rows.Count>0)
                {
                    txtVoucherno.Text = ds.Tables[1].Rows[0][0].ToString() + ds.Tables[0].Rows[0][0].ToString() + ds.Tables[2].Rows[0][0].ToString();
                }
                else { Alert_msg("Must set prefix sufix"); }

                txtInvoiceCurrency.Attributes.Add("onblur", "javascript:Call__Inv_Currency_Excahnge_Rate('" + txtInvoiceCurrency.ClientID + "', '" + txtExrate.ClientID + "')");
                txtInvoiceCurrency.Text = "INR";
                txtExrate.Text = "1";
                txtVoucherdate.Text = DateTime.Now.ToShortDateString();
                //Rownum = 1;
                ViewState["RowNumber"] = 1;
                DataTable dt = new DataTable();
                DataRow dr = dt.NewRow();
                DropDownList drp = new DropDownList();
                dt.Columns.Add(new DataColumn("SNo", typeof(string)));
                dt.Columns.Add(new DataColumn("File_Ref_No", typeof(string)));
                dt.Columns.Add(new DataColumn("Date", typeof(string)));
                dt.Columns.Add(new DataColumn("DR_CR", typeof(string)));
                dt.Columns.Add(new DataColumn("Customer_Name", typeof(string)));
                dt.Columns.Add(new DataColumn("Imp_Exp", typeof(string)));
                dt.Columns.Add(new DataColumn("Charge_Name", typeof(string)));
                dt.Columns.Add(new DataColumn("Gst_Type", typeof(string)));
                dt.Columns.Add(new DataColumn("Tax_Rate", typeof(string)));
                dt.Columns.Add(new DataColumn("Credit", typeof(string)));
                dt.Columns.Add(new DataColumn("Debit", typeof(string)));
                dt.Columns.Add(new DataColumn("CGST", typeof(string)));
                dt.Columns.Add(new DataColumn("SCGST", typeof(string)));
                dt.Columns.Add(new DataColumn("IGST", typeof(string)));
                dt.Columns.Add(new DataColumn("TDS", typeof(string)));
                dt.Columns.Add(new DataColumn("TDS_Amount", typeof(string)));
                dt.Columns.Add(new DataColumn("FileUp", typeof(string)));
                dt.Rows.Add(dr);
                dr["DR_CR"] = new ListItem("DR");
                dr["Imp_exp"] = new ListItem("---");
                dr["SNo"] = ViewState["RowNumber"].ToString();
                dr["Gst_Type"] = new ListItem("GST_TYPE_01");

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
    }


    private void LoadVendor()
    {
        DataSet ds = new DataSet();      
        ObjUBO.ENAME = "Select_Vendor";
        ObjUBO.BRANCH_CODE = currentbranch;
        ds = BI.DebitNoteEntry(ObjUBO);
        if (ds.Tables[0].Rows.Count > 0)
        {
            ddlVendorname.DataSource = ds.Tables[0];
            ddlVendorname.DataTextField = "VENDOR_NAME";
            ddlVendorname.DataValueField = "VENDOR_NAME";
            ddlVendorname.DataBind();
        }

        ddlVendorname.Items.Insert(0, new System.Web.UI.WebControls.ListItem("--Select--", "--Select--"));
        ddlVendorname.SelectedIndex = 0;

        ds.Dispose();
        GC.Collect();
    }

    protected void ddlVendorname_SelectedIndexChanged(object sender, EventArgs e)
    {
        Load_Voucher();
    }

    private void Load_Voucher()
    {
        DataSet ds = new DataSet();
        ObjUBO.ENAME = "Select_Vendor";
        ObjUBO.VENDOR_NAME = ddlVendorname.SelectedValue.ToString();
        ObjUBO.BRANCH_CODE = currentbranch;
        ds = BI.DebitNoteEntry(ObjUBO);
        if (ds.Tables.Count > 1)
        {
            if (ds.Tables[1].Rows.Count > 0)
            {
                ddlPurchaseVoucherID.DataSource = ds.Tables[1];
                ddlPurchaseVoucherID.DataTextField = "VOUCHER_NO";
                ddlPurchaseVoucherID.DataValueField = "VOUCHER_ID";
                ddlPurchaseVoucherID.DataBind();
            }
            ddlPurchaseVoucherID.Items.Insert(0, new System.Web.UI.WebControls.ListItem("--Select--", "--Select--"));
            ddlPurchaseVoucherID.SelectedIndex = 0;
        }
        ds.Dispose();
        GC.Collect();

    }

    protected void ddlPurchaseVoucherID_SelectedIndexChanged(object sender, EventArgs e)
    {
        load_purchaseEtryDetails();
        hdnVoucherID.Value = ddlPurchaseVoucherID.SelectedValue.ToString();
    }

    private void load_purchaseEtryDetails()
    {
        DataSet ds = new DataSet();
        ObjUBO.ENAME = "Select_VoucherDetails";
        ObjUBO.VOUCHER_NO = ddlPurchaseVoucherID.SelectedItem.Text.ToString();
        ObjUBO.BRANCH_CODE = currentbranch;
        ds = BI.DebitNoteEntry(ObjUBO);
        if (ds.Tables.Count > 0)
        {
            if (ds.Tables[0].Rows.Count > 0)
            {
                gvdetails.DataSource = ds.Tables[0];
                gvdetails.DataBind();
            }
             
        }
        ds.Dispose();
        GC.Collect();
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

        if (ViewState["CurrentTable"] != null)
        {
            DataTable dtCurrentTable = (DataTable)ViewState["CurrentTable"];
            DataRow drCurrentRow = null;
            if (dtCurrentTable.Rows.Count > 0)
            {
                int Slno = 1;
                for (int i = 1; i <= dtCurrentTable.Rows.Count; i++)
                {
                    TextBox txtgFileRefNo = (TextBox)gvdetails.Rows[rowIndex].Cells[1].FindControl("txtgFileRefNo");
                    TextBox txtgDate = (TextBox)gvdetails.Rows[rowIndex].Cells[2].FindControl("txtgDate");
                    DropDownList drpgDrCr = (DropDownList)gvdetails.Rows[rowIndex].Cells[3].FindControl("drpDrcr");
                    TextBox txtgCusname = (TextBox)gvdetails.Rows[rowIndex].Cells[4].FindControl("txtgCusName");
                    DropDownList drpgImpexp = (DropDownList)gvdetails.Rows[rowIndex].Cells[5].FindControl("drpImpexp");
                    TextBox txtgCharhead = (TextBox)gvdetails.Rows[rowIndex].Cells[6].FindControl("txtgCharHead");
                    DropDownList ddlgsttype = (DropDownList)gvdetails.Rows[rowIndex].Cells[7].FindControl("ddl_Gst_type");
                    TextBox txtgTaxrate = (TextBox)gvdetails.Rows[rowIndex].Cells[8].FindControl("txtgTaxrate");
                    TextBox txtgCredit = (TextBox)gvdetails.Rows[rowIndex].Cells[9].FindControl("txtgCredit");
                    TextBox txtgDepit = (TextBox)gvdetails.Rows[rowIndex].Cells[10].FindControl("txtgDebit");
                    TextBox txtgCgst = (TextBox)gvdetails.Rows[rowIndex].Cells[11].FindControl("txtgCgst");
                    TextBox txtgScgst = (TextBox)gvdetails.Rows[rowIndex].Cells[12].FindControl("txtgScgst");
                    TextBox txtgIgst = (TextBox)gvdetails.Rows[rowIndex].Cells[13].FindControl("txtgIgst");
                    TextBox txtgTds = (TextBox)gvdetails.Rows[rowIndex].Cells[14].FindControl("txtgTds");
                    TextBox txtgTdsamt = (TextBox)gvdetails.Rows[rowIndex].Cells[15].FindControl("txtgdtsamt");
                    FileUpload file = (FileUpload)gvdetails.Rows[rowIndex].Cells[16].FindControl("FileUpload1"); 
                    HiddenField hfFile = (HiddenField)gvdetails.Rows[rowIndex].Cells[16].FindControl("hfFileByte");

                    if (txtgCredit.Text != "")
                    {
                        totalcredit += Convert.ToDecimal(txtgCredit.Text);
                    }
                        if (txtgDepit.Text != "")
                        {
                        totaldebit += Convert.ToDecimal(txtgDepit.Text);
                    }
                    if (txtgCgst.Text != "") { totalcgst += Convert.ToDecimal(txtgCgst.Text); }
                    if (txtgScgst.Text != "") { totalsgst += Convert.ToDecimal(txtgScgst.Text); }
                    if (txtgIgst.Text != "") { totaligst += Convert.ToDecimal(txtgIgst.Text); }
                    if (txtgTdsamt.Text != "") { totaltds += Convert.ToDecimal(txtgTdsamt.Text); }
                     total = totaldebit + totalcgst + totaligst + totalsgst - totaltds;

                    drCurrentRow = dtCurrentTable.NewRow();
                    drCurrentRow["DR_CR"] = new ListItem("DR");
                    drCurrentRow["Imp_Exp"] = new ListItem("Imp");
                    drCurrentRow["Gst_Type"] = new ListItem("GST_TYPE_01");

                    dtCurrentTable.Rows[i - 1]["SNo"] = Slno;
                    dtCurrentTable.Rows[i - 1]["File_Ref_No"] = txtgFileRefNo.Text;
                    dtCurrentTable.Rows[i - 1]["Date"] = txtgDate.Text;
                    dtCurrentTable.Rows[i - 1]["DR_CR"] = drpgDrCr.SelectedValue;
                    dtCurrentTable.Rows[i - 1]["Customer_Name"] = txtgCusname.Text;
                    dtCurrentTable.Rows[i - 1]["Imp_Exp"] = drpgImpexp.SelectedValue;
                    dtCurrentTable.Rows[i - 1]["Charge_Name"] = txtgCharhead.Text;
                    dtCurrentTable.Rows[i - 1]["Gst_Type"] = ddlgsttype.SelectedValue;
                    dtCurrentTable.Rows[i - 1]["Tax_Rate"] = txtgTaxrate.Text;
                    dtCurrentTable.Rows[i - 1]["Credit"] = txtgCredit.Text;
                    dtCurrentTable.Rows[i - 1]["Debit"] = txtgDepit.Text;
                    dtCurrentTable.Rows[i - 1]["CGST"] = txtgCgst.Text;
                    dtCurrentTable.Rows[i - 1]["SCGST"] = txtgScgst.Text;
                    dtCurrentTable.Rows[i - 1]["IGST"] = txtgIgst.Text;
                    dtCurrentTable.Rows[i - 1]["TDS"] = txtgTds.Text;
                    dtCurrentTable.Rows[i - 1]["TDS_Amount"] = txtgTdsamt.Text;
                    rowIndex++;
                    Slno++;
                    
                }
                int index = gvdetails.Rows.Count-1 ;
                drCurrentRow["SNo"] = Slno ;
                drCurrentRow["Debit"] =   total;
                
                dtCurrentTable.Rows.Add(drCurrentRow);
                ViewState["CurrentTable"] = dtCurrentTable;

                gvdetails.DataSource = dtCurrentTable;
                gvdetails.DataBind();
                DropDownList drpgImpex = (DropDownList)gvdetails.Rows[index + 1].Cells[5].FindControl("drpImpexp");
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
                    TextBox txtgFileRefNo = (TextBox)gvdetails.Rows[rowIndex].Cells[1].FindControl("txtgFileRefNo");
                    TextBox txtgDate = (TextBox)gvdetails.Rows[rowIndex].Cells[2].FindControl("txtgDate");
                    DropDownList drpgDrCr = (DropDownList)gvdetails.Rows[rowIndex].Cells[3].FindControl("drpDrcr");
                    TextBox txtgCusname = (TextBox)gvdetails.Rows[rowIndex].Cells[4].FindControl("txtgCusName");
                    DropDownList drpgImpexp = (DropDownList)gvdetails.Rows[rowIndex].Cells[5].FindControl("drpImpexp");
                    TextBox txtgCharhead = (TextBox)gvdetails.Rows[rowIndex].Cells[6].FindControl("txtgCharHead");
                    DropDownList ddlgsttype = (DropDownList)gvdetails.Rows[rowIndex].Cells[7].FindControl("ddl_Gst_type");
                    TextBox txtgTaxrate = (TextBox)gvdetails.Rows[rowIndex].Cells[8].FindControl("txtgTaxrate");
                    TextBox txtgCredit = (TextBox)gvdetails.Rows[rowIndex].Cells[9].FindControl("txtgCredit");
                    TextBox txtgDepit = (TextBox)gvdetails.Rows[rowIndex].Cells[10].FindControl("txtgDebit");
                    TextBox txtgCgst = (TextBox)gvdetails.Rows[rowIndex].Cells[11].FindControl("txtgCgst");
                    TextBox txtgScgst = (TextBox)gvdetails.Rows[rowIndex].Cells[12].FindControl("txtgScgst");
                    TextBox txtgIgst = (TextBox)gvdetails.Rows[rowIndex].Cells[13].FindControl("txtgIgst");
                    TextBox txtgTds = (TextBox)gvdetails.Rows[rowIndex].Cells[14].FindControl("txtgTds");
                    TextBox txtgTdsamt = (TextBox)gvdetails.Rows[rowIndex].Cells[15].FindControl("txtgdtsamt");
                    FileUpload file = (FileUpload)gvdetails.Rows[rowIndex].Cells[16].FindControl("FileUpload1"); 
                    HiddenField hfFile = (HiddenField)gvdetails.Rows[rowIndex].Cells[16].FindControl("hfFileByte"); 
                    LinkButton lnkBtn = (LinkButton)gvdetails.Rows[rowIndex].Cells[16].FindControl("LinkButton1");
                    lnkBtn.Visible = false;
                    file.Visible = true;
                   
                    txtgSLNo.Text = dt.Rows[i]["SNo"].ToString();
                    txtgFileRefNo.Text = dt.Rows[i]["File_Ref_No"].ToString();
                    txtgDate.Text = dt.Rows[i]["Date"].ToString();
                    drpgDrCr.SelectedValue = dt.Rows[i]["DR_CR"].ToString();
                    txtgCusname.Text = dt.Rows[i]["Customer_Name"].ToString();
                    drpgImpexp.SelectedValue = dt.Rows[i]["Imp_Exp"].ToString();
                    txtgCharhead.Text = dt.Rows[i]["Charge_Name"].ToString();
                    ddlgsttype.SelectedValue = dt.Rows[i]["Gst_Type"].ToString();
                    txtgTaxrate.Text = dt.Rows[i]["Tax_Rate"].ToString();
                    txtgCredit.Text = dt.Rows[i]["Credit"].ToString();
                    txtgDepit.Text = dt.Rows[i]["Debit"].ToString();
                    txtgCgst.Text = dt.Rows[i]["CGST"].ToString();
                    txtgScgst.Text = dt.Rows[i]["SCGST"].ToString();
                    txtgIgst.Text = dt.Rows[i]["IGST"].ToString();
                    txtgTds.Text = dt.Rows[i]["TDS"].ToString();
                    txtgTdsamt.Text = dt.Rows[i]["TDS_Amount"].ToString();
                    hfFile.Value = file.HasFile ? Path.GetFileName(file.PostedFile.FileName) : file.FileName; 
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
                    TextBox txtgSLNo = (TextBox)gvdetails.Rows[rowIndex].Cells[0].FindControl("txtgSLNo");
                    TextBox txtgFileRefNo = (TextBox)gvdetails.Rows[rowIndex].Cells[1].FindControl("txtgFileRefNo");
                    TextBox txtgDate = (TextBox)gvdetails.Rows[rowIndex].Cells[2].FindControl("txtgDate");
                    DropDownList drpgDrCr = (DropDownList)gvdetails.Rows[rowIndex].Cells[3].FindControl("drpDrcr");
                    TextBox txtgCusname = (TextBox)gvdetails.Rows[rowIndex].Cells[4].FindControl("txtgCusName");
                    DropDownList drpgImpexp = (DropDownList)gvdetails.Rows[rowIndex].Cells[5].FindControl("drpImpexp");
                    TextBox txtgCharhead = (TextBox)gvdetails.Rows[rowIndex].Cells[6].FindControl("txtgCharHead");
                    DropDownList ddlgsttype = (DropDownList)gvdetails.Rows[rowIndex].Cells[7].FindControl("ddl_Gst_type");
                    TextBox txtgTaxrate = (TextBox)gvdetails.Rows[rowIndex].Cells[8].FindControl("txtgTaxrate");
                    TextBox txtgCredit = (TextBox)gvdetails.Rows[rowIndex].Cells[9].FindControl("txtgCredit");
                    TextBox txtgDepit = (TextBox)gvdetails.Rows[rowIndex].Cells[10].FindControl("txtgDebit");
                    TextBox txtgCgst = (TextBox)gvdetails.Rows[rowIndex].Cells[11].FindControl("txtgCgst");
                    TextBox txtgScgst = (TextBox)gvdetails.Rows[rowIndex].Cells[12].FindControl("txtgScgst");
                    TextBox txtgIgst = (TextBox)gvdetails.Rows[rowIndex].Cells[13].FindControl("txtgIgst");
                    TextBox txtgTds = (TextBox)gvdetails.Rows[rowIndex].Cells[14].FindControl("txtgTds");
                    TextBox txtgTdsamt = (TextBox)gvdetails.Rows[rowIndex].Cells[15].FindControl("txtgdtsamt");

                    if (txtgCredit.Text != "")
                    {
                        totalcredit += Convert.ToDecimal(txtgCredit.Text);
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
                    dtCurrentTable.Rows[i - 1]["File_Ref_No"] = txtgFileRefNo.Text;
                    dtCurrentTable.Rows[i - 1]["Date"] = txtgDate.Text;
                    dtCurrentTable.Rows[i - 1]["Customer_Name"] = txtgCusname.Text;
                    dtCurrentTable.Rows[i - 1]["Charge_Name"] = txtgCharhead.Text;
                    dtCurrentTable.Rows[i - 1]["Gst_Type"] = ddlgsttype.SelectedValue;
                    dtCurrentTable.Rows[i - 1]["Tax_Rate"] = txtgTaxrate.Text;
                    dtCurrentTable.Rows[i - 1]["Credit"] = txtgCredit.Text;
                    dtCurrentTable.Rows[i - 1]["Debit"] = txtgDepit.Text;
                    dtCurrentTable.Rows[i - 1]["CGST"] = txtgCgst.Text;
                    dtCurrentTable.Rows[i - 1]["SCGST"] = txtgScgst.Text;
                    dtCurrentTable.Rows[i - 1]["IGST"] = txtgIgst.Text;
                    dtCurrentTable.Rows[i - 1]["TDS"] = txtgTds.Text;
                    dtCurrentTable.Rows[i - 1]["TDS_Amount"] = txtgTdsamt.Text;
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
                TextBox txtdebit = (e.Row.FindControl("txtgDebit") as TextBox);
                TextBox txtcredit = (e.Row.FindControl("txtgCredit") as TextBox);

                txtdebit.ReadOnly = true;
                txtcredit.ReadOnly = false;
            }
            else if (customerId.Text == "DR")
            {
                TextBox txtdebit = (e.Row.FindControl("txtgDebit") as TextBox);
                TextBox txtcredit = (e.Row.FindControl("txtgCredit") as TextBox);
                txtdebit.ReadOnly = false;
                txtcredit.ReadOnly = true;
            }
        }
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
            save_update("INSERT");
    }

    private void save_update(string name)
    {
        string expdate = "";
        if (txtVoucherdate.Text != "")
        {
            expdate = txtVoucherdate.Text.Trim();
            expdate = expdate.ToString().Substring(3, 3) + expdate.ToString().Substring(0, 3) + expdate.ToString().Substring(6, 4);
        }

        DataTable dt = new DataTable();
        DataSet ds = new DataSet();
        ObjUBO.DEBITNOTE_MAIN_ID = HDupdate_id.Value;
        ObjUBO.DEBIT_NOTE_NO = txtVoucherno.Text;
        ObjUBO.VOUCHER_NO = ddlPurchaseVoucherID.SelectedItem.Text;
        ObjUBO.VOUCHER_ID = ddlPurchaseVoucherID.SelectedValue.ToString();
        ObjUBO.VOUCHER_DATE = expdate;

        ObjUBO.VENDOR_NAME = ddlVendorname.SelectedValue.ToString();
        ObjUBO.DEBITNOTE_SUB_ID = "0";

        ObjUBO.NARRATION = txtNarration.Text;
        ObjUBO.USER_ID = Connection.Current_User();
        ObjUBO.BRANCH_CODE = currentbranch;
        ObjUBO.WORKING_PERIOD = Connection.WorkingPeriod();
        dt = gridtodt();
        ds.Tables.Add(dt);
        ObjUBO.XML_DETAIL = ds.GetXml();
        ObjUBO.ENAME = name ;
        ds = BI.DebitNoteEntry(ObjUBO);
        string pathFolder = Server.MapPath("~/Submission/" + Session["COMPANY_ID"].ToString() + "/" + "DebitNoteEntry");
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
                btnUpdate.Visible = false;

                DataTable dt1 = new DataTable();
                gvdetails.DataSource = dt1;
                gvdetails.DataBind();
            }
        }
    }

    private void cleardata()
    {
            
        DataTable dt1 = new DataTable();
        gvdetails.DataSource = dt1;
        gvdetails.DataBind();

        txtVoucherno.Text = ""; txtVoucherdate.Text = ""; txtNarration.Text = ""; ddlPurchaseVoucherID.SelectedIndex = 0; ddlVendorname.SelectedIndex = 0;
        txtInvoiceCurrency.Text = ""; txtExrate.Text = "";  txtGrandtotal.Text = ""; txtNetamt.Text = "";
    }

    protected void btnDelete_Click(object sender, EventArgs e)
    {
        try
        {           
            DataSet ds = new DataSet();
            ObjUBO.DEBITNOTE_MAIN_ID = HDupdate_id.Value;
            ObjUBO.ENAME = "DELETE";
            ds = BI.DebitNoteEntry(ObjUBO); 
            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {

                    Alert_msg(ds.Tables[0].Rows[0][1].ToString()); 
                    btnUpdate.Visible = false;

                    DataTable dt1 = new DataTable();
                    gvdetails.DataSource = dt1;
                    gvdetails.DataBind();
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
            save_update("UPDATE");
    }
    public void Alert_msg(string msg)
    {
        string prompt = "<script>$(document).ready(function(){{jAlert('" + msg + "', 'Debit Note Entry', function (r) {});}});</script>";
        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", prompt, false);
    }
    public void Alert_msg(string msg, string focus)
    {
        string prompt = "<script>$(document).ready(function(){{jAlert('" + msg + "', 'Debit Note Entry', function (r) {document.getElementById('" + focus + "').focus();});}});</script>";
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
            dsData.Tables[0].TableName = "MessageDetails";
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
    private DataSet gridtodtNew()
    {
        DataSet ds = new DataSet();
        DataTable dt = new DataTable();
        ds.Tables.Add("MessageDetails");
        string expdate = "";

        if (gvdetails.Rows.Count != 0)
        {
            for (int i = 0; i < gvdetails.HeaderRow.Cells.Count - 2; i++)
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
                        DropDownList drpimexp = row.FindControl("drpImpexp") as DropDownList;
                        dr[gvdetails.HeaderRow.Cells[j].Text] = drpimexp.SelectedValue;

                    }
                    else if (j == 2)
                    {
                        TextBox txtFilerefno = row.FindControl("txtgFileRefNo") as TextBox;
                        dr[gvdetails.HeaderRow.Cells[j].Text] = txtFilerefno.Text;
                    }
                    else if (j == 3)
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
                    else if (j == 4)
                    {
                        DropDownList drpcdr = row.FindControl("drpDrcr") as DropDownList;
                        dr[gvdetails.HeaderRow.Cells[j].Text] = drpcdr.SelectedValue;

                    }
                    else if (j == 5)
                    {
                        TextBox txtCusname = row.FindControl("txtgCusName") as TextBox;
                        dr[gvdetails.HeaderRow.Cells[j].Text] = txtCusname.Text;

                    }
                    else if (j == 6)
                    {
                        TextBox txtcharge = row.FindControl("txtgCharHead") as TextBox;
                        dr[gvdetails.HeaderRow.Cells[j].Text] = txtcharge.Text;
                    }
                    else if (j == 7)
                    {
                        DropDownList ddlgsttype = row.FindControl("ddl_Gst_type") as DropDownList;
                        dr[gvdetails.HeaderRow.Cells[j].Text] = ddlgsttype.SelectedValue;
                    }
                    else if (j == 8)
                    {
                        TextBox txtTaxrate = row.FindControl("txtgTaxrate") as TextBox;
                        dr[gvdetails.HeaderRow.Cells[j].Text] = txtTaxrate.Text;
                    }
                    else if (j == 9)
                    {
                        TextBox txtCredit = row.FindControl("txtgCredit") as TextBox;
                        dr[gvdetails.HeaderRow.Cells[j].Text] = txtCredit.Text;
                    }
                    else if (j == 10)
                    {
                        TextBox txtDebit = row.FindControl("txtgDebit") as TextBox;
                        dr[gvdetails.HeaderRow.Cells[j].Text] = txtDebit.Text;
                    }
                    else if (j == 11)
                    {
                        TextBox txtCgst = row.FindControl("txtgCgst") as TextBox;
                        dr[gvdetails.HeaderRow.Cells[j].Text] = txtCgst.Text;
                    }
                    else if (j == 12)
                    {
                        TextBox txtScgst = row.FindControl("txtgScgst") as TextBox;
                        dr[gvdetails.HeaderRow.Cells[j].Text] = txtScgst.Text;
                    }
                    else if (j == 13)
                    {
                        TextBox txtIgst = row.FindControl("txtgIgst") as TextBox;
                        dr[gvdetails.HeaderRow.Cells[j].Text] = txtIgst.Text;
                    }
                    else if (j == 14)
                    {
                        TextBox txtTds = row.FindControl("txtgTds") as TextBox;
                        dr[gvdetails.HeaderRow.Cells[j].Text] = txtTds.Text;
                    }
                    else if (j == 15)
                    {
                        TextBox txtTdsamt = row.FindControl("txtgdtsamt") as TextBox;
                        dr[gvdetails.HeaderRow.Cells[j].Text] = txtTdsamt.Text;
                    }
                    else if (j == 16)
                    {
                        FileUpload file = row.FindControl("FileUpload1") as FileUpload;
                        dr[gvdetails.HeaderRow.Cells[j].Text] = "~/Submission/" + Session["COMPANY_ID"].ToString() + "/" + "DebitNoteEntry" + "/" + file.FileName;
                        dr["filename"] = file.FileName;
                    }
                }
                dt.Rows.Add(dr);
            }
        }
        ds.Tables[0].Rows.Add(dt);
        return ds;
    }

    private DataTable gridtodt()
    {
        DataTable dt = new DataTable();
        string expdate = "";
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
                        DropDownList drpimexp = row.FindControl("drpImpexp") as DropDownList;
                        dr[gvdetails.HeaderRow.Cells[j].Text] = drpimexp.SelectedValue;

                    }
                    else if (j == 2)
                    {
                        TextBox txtFilerefno = row.FindControl("txtgFileRefNo") as TextBox;
                        dr[gvdetails.HeaderRow.Cells[j].Text] = txtFilerefno.Text;
                    }
                    else if (j == 3)
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
                    else if (j == 4)
                    {
                        DropDownList drpcdr = row.FindControl("drpDrcr") as DropDownList;
                        dr[gvdetails.HeaderRow.Cells[j].Text] = drpcdr.SelectedValue;

                    }
                    else if (j == 5)
                    {
                        TextBox txtCusname = row.FindControl("txtgCusName") as TextBox;
                        dr[gvdetails.HeaderRow.Cells[j].Text] = txtCusname.Text;

                    }
                    else if (j == 6)
                    {
                        TextBox txtcharge = row.FindControl("txtgCharHead") as TextBox;
                        dr[gvdetails.HeaderRow.Cells[j].Text] = txtcharge.Text;
                    }
                    else if (j == 7)
                    {
                        DropDownList ddlgsttype = row.FindControl("ddl_Gst_type") as DropDownList;
                        dr[gvdetails.HeaderRow.Cells[j].Text] = ddlgsttype.SelectedValue;
                    }
                    else if (j == 8)
                    {
                        TextBox txtTaxrate = row.FindControl("txtgTaxrate") as TextBox;
                        if (txtTaxrate.Text == "")
                        { txtTaxrate.Text = "0"; }
                        dr[gvdetails.HeaderRow.Cells[j].Text] = txtTaxrate.Text;
                    }
                    else if (j == 9)
                    {
                        TextBox txtCredit = row.FindControl("txtgCredit") as TextBox;
                        if (txtCredit.Text == "")
                        { txtCredit.Text = "0"; }
                        dr[gvdetails.HeaderRow.Cells[j].Text] = txtCredit.Text;
                    }
                    else if (j == 10)
                    {
                        TextBox txtDebit = row.FindControl("txtgDebit") as TextBox;
                        if (txtDebit.Text == "")
                        { txtDebit.Text = "0"; }
                        dr[gvdetails.HeaderRow.Cells[j].Text] = txtDebit.Text;
                    }
                    else if (j == 11)
                    {
                        TextBox txtCgst = row.FindControl("txtgCgst") as TextBox;
                        if (txtCgst.Text == "")
                        { txtCgst.Text = "0"; }
                        dr[gvdetails.HeaderRow.Cells[j].Text] = txtCgst.Text;
                    }
                    else if (j == 12)
                    {
                        TextBox txtScgst = row.FindControl("txtgScgst") as TextBox;
                        if (txtScgst.Text == "")
                        { txtScgst.Text = "0"; }
                        dr[gvdetails.HeaderRow.Cells[j].Text] = txtScgst.Text;
                    }
                    else if (j == 13)
                    {
                        TextBox txtIgst = row.FindControl("txtgIgst") as TextBox;
                        if (txtIgst.Text == "")
                        { txtIgst.Text = "0"; }
                        dr[gvdetails.HeaderRow.Cells[j].Text] = txtIgst.Text;
                    }
                    else if (j == 14)
                    {
                        TextBox txtTds = row.FindControl("txtgTds") as TextBox;
                        if (txtTds.Text == "")
                        { txtTds.Text = "0"; }
                        dr[gvdetails.HeaderRow.Cells[j].Text] = txtTds.Text;
                    }
                    else if (j == 15)
                    {
                        TextBox txtTdsamt = row.FindControl("txtgdtsamt") as TextBox;
                        if (txtTdsamt.Text == "")
                        { txtTdsamt.Text = "0"; }
                        dr[gvdetails.HeaderRow.Cells[j].Text] = txtTdsamt.Text;
                    }
                    else if (j == 16)
                    {
                        FileUpload file = row.FindControl("FileUpload1") as FileUpload;
                        dr[gvdetails.HeaderRow.Cells[j].Text] = "~/Submission/" + Session["COMPANY_ID"].ToString() + "/" + "DebitNoteEntry" + "/" + file.FileName;
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

        string f = ds.Tables[0].Rows[0]["file"].ToString();
        fileName = ds.Tables[0].Rows[0]["filename"].ToString();
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
        Response.AppendHeader("Content-Disposition", "attachment; filename=" + f+"/"+fileName);
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
                    TextBox txtgCredit = (TextBox)gvdetails.Rows[i].Cells[9].FindControl("txtgCredit");
                    TextBox txtgDepit = (TextBox)gvdetails.Rows[i].Cells[10].FindControl("txtgDebit");
                    TextBox txtgCgst = (TextBox)gvdetails.Rows[i].Cells[11].FindControl("txtgCgst");
                    TextBox txtgScgst = (TextBox)gvdetails.Rows[i].Cells[12].FindControl("txtgScgst");
                    TextBox txtgIgst = (TextBox)gvdetails.Rows[i].Cells[13].FindControl("txtgIgst");
                    TextBox txtgTds = (TextBox)gvdetails.Rows[i].Cells[14].FindControl("txtgTds");
                    TextBox txtgTdsamt = (TextBox)gvdetails.Rows[i].Cells[15].FindControl("txtgdtsamt");
                    if (txtgCredit.Text != "")
                    {
                        totalcredit += Convert.ToDecimal(txtgCredit.Text);
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
    public static string Cust_Name(string ChargeName, string Imp_Name, string Mode)
    {
        string cname="";
         DataSet ds=new DataSet();
         GST_Imp_Invoice BI = new GST_Imp_Invoice();
          Billing_UserBO ObjUBO = new Billing_UserBO();
         ObjUBO.ENAME="CUST_NAME";
         ObjUBO.VENDOR_NAME = ChargeName;
         ds= BI.PurchaseEntry(ObjUBO);
         if (ds.Tables.Count > 0)
         {
             if (ds.Tables[0].Rows.Count > 0)
             {
                 cname = ds.Tables[0].Rows[0][0].ToString() + "~~" + ds.Tables[0].Rows[0]["JOBDATE"].ToString();
             }
             else
             {
                 cname = "";
             }
         }

         return cname;
    }
     protected void txtgFileRefNo_TextChanged(object sender, EventArgs e)
     {
          
         DataSet ds = new DataSet();
         GridViewRow currentRow = ((GridViewRow)((TextBox)sender).NamingContainer);
         TextBox txtgFileRef = (TextBox)currentRow.FindControl("txtgFileRefNo");
         TextBox txtgCusName = (TextBox)currentRow.FindControl("txtgCusName");
         txtgFileRef.Text = "FILE/2020/12";
         txtgCusName.Text = "PARAMOUNT";
     }
     protected void txtgCharHead_TextChanged(object sender, EventArgs e)
     {
         GridViewRow currentRow = ((GridViewRow)((TextBox)sender).NamingContainer);
         TextBox txtgcharh = (TextBox)currentRow.FindControl("txtgCharHead");
         TextBox txtgcgst = (TextBox)currentRow.FindControl("txtgCgst");
         TextBox txtgsgst = (TextBox)currentRow.FindControl("txtgScgst");
         TextBox txtgigst = (TextBox)currentRow.FindControl("txtgIgst");
         TextBox txtgtds = (TextBox)currentRow.FindControl("txtgdtsamt");
         txtgcharh.Text = "AAI EXPENSES";
         txtgcgst.Text = "2";
         txtgsgst.Text = "3";
         txtgigst.Text = "2";
         txtgtds.Text = "";
     }
     
     [System.Web.Services.WebMethod]
     public static string Get_Cha_rate(string ChargeName, string Imp_Name, string Mode)
     {
         string Ch_Name = "";
         DataSet ds = new DataSet();
         try
         {
             DebitNote_cs BI = new DebitNote_cs();
             Billing_UserBO ObjUBO = new Billing_UserBO();

             ObjUBO.charge_name = ChargeName;
             ObjUBO.Flag = "select_charge";
             ds = BI.Select_Char_val(ObjUBO);

             if (ds.Tables[0].Rows.Count > 0)
             {
                 Ch_Name = ds.Tables[0].Rows[0]["CGST_RATE"].ToString() + "~~" + ds.Tables[0].Rows[0]["SGST_RATE"].ToString() + "~~" + ds.Tables[0].Rows[0]["IGST_RATE"].ToString() + "~~" + ds.Tables[0].Rows[0]["TDS_RATE"].ToString() + "~~" + ds.Tables[0].Rows[0]["SA_CODE"].ToString();
             }
             else
             {
                 Ch_Name = "";
             }
         }
         catch (Exception ex)
         {
             Connection.Error_Msg(ex.Message);
         }
         return Ch_Name;
     }

     [System.Web.Services.WebMethod]
     public static string Get_Vendordetail(string VendorName, string VendorBranch, string Mode)
     {
         string Vendor_Name = "";
         DataSet ds = new DataSet();
         try
         {
             DebitNote_cs BI = new DebitNote_cs();
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
             ObjUBO.DEBITNOTE_MAIN_ID = Request.QueryString["voucherno"].ToString();
             HDupdate_id.Value = Request.QueryString["voucherno"].ToString();
             ObjUBO.ENAME = "SELECT";         
             dss = BI.DebitNoteEntry(ObjUBO);
             if (dss.Tables[0].Rows.Count > 0)
             {
                 LoadVendor();
                 txtVoucherno.Text = dss.Tables[0].Rows[0]["DEBITNOTE_NO"].ToString();
                 ddlVendorname.SelectedValue = dss.Tables[0].Rows[0]["VENDOR_NAME"].ToString();
                 Load_Voucher();

                 txtVoucherdate.Text = dss.Tables[0].Rows[0]["DEBITNOTE_DATE"].ToString();
                 ddlPurchaseVoucherID.SelectedValue = dss.Tables[0].Rows[0]["VOUCHER_ID"].ToString();
                 ddlPurchaseVoucherID.SelectedItem.Text = dss.Tables[0].Rows[0]["VOUCHER_NO"].ToString();
                 txtNarration.Text = dss.Tables[0].Rows[0]["Narration"].ToString();

             }

             ViewState["CurrentTable"] = dss.Tables[0];

             if (dss.Tables[0].Rows.Count > 0)
             {
                 gvdetails.DataSource = dss.Tables[0];
                 gvdetails.DataBind();

                 for (int i = 0; i < gvdetails.Rows.Count; i++)
                 {
                     gvdetails.Rows[i].Cells[0].Text = Convert.ToString(i + 1);

                     LinkButton lnkBtn = (LinkButton)gvdetails.Rows[i].FindControl("LinkButton1"); 
                     lnkBtn.Visible = true; 
                     FileUpload Fileupl = (FileUpload)gvdetails.Rows[i].FindControl("FileUpload1"); 
                     Fileupl.Visible = false; 


                     TextBox txtgCredit = (TextBox)gvdetails.Rows[i].Cells[9].FindControl("txtgCredit");
                     TextBox txtgDepit = (TextBox)gvdetails.Rows[i].Cells[10].FindControl("txtgDebit");
                     TextBox txtgCgst = (TextBox)gvdetails.Rows[i].Cells[11].FindControl("txtgCgst");
                     TextBox txtgScgst = (TextBox)gvdetails.Rows[i].Cells[12].FindControl("txtgScgst");
                     TextBox txtgIgst = (TextBox)gvdetails.Rows[i].Cells[13].FindControl("txtgIgst");
                     TextBox txtgTds = (TextBox)gvdetails.Rows[i].Cells[14].FindControl("txtgTds");
                     TextBox txtgTdsamt = (TextBox)gvdetails.Rows[i].Cells[15].FindControl("txtgdtsamt");
                     if (txtgCredit.Text != "")
                     {
                         totalcredit += Convert.ToDecimal(txtgCredit.Text);
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
     
}