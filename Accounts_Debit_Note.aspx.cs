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


public partial class Accounts_Accounts_Debit_Note : ThemeClass
{
    public string currentuser;
    public string currentbranch;
    public string Working_Period;
    User_Creation user_Create = new User_Creation();
    AppSession aps = new AppSession();
    eroyalmaster ERM = new eroyalmaster();

    Accounts_Credit_Debit BP = new Accounts_Credit_Debit();
    Global_variables ObjUBO = new Global_variables();
    DataSet ds = new DataSet();
    DataTable dt = new DataTable();

    public int i, Screen_Id, Approval_ID;
    public string SCREEN_ID, PAGE_MODIFY, PAGE_DELETE, PAGE_WRITE, PAGE_READ, COMPANY_ID, Screen_IdNew, APPROVAL_CATEGORY;

    decimal tax_amt, non_tax_amt, gst_toal, Reverse_total;

    string Imp_name = string.Empty, J_no = string.Empty, Ch_type = string.Empty;
    public bool chk = false;

    protected void Page_Load(object sender, EventArgs e)
    {
        aps.checkSession();

        CalendarExtender1.EndDate = DateTime.Today;
        Screen_Id = 99;
        Page_Rights(Screen_Id);
        Approval_ID = 6;
        Page_Approval(Approval_ID);
        if (!Page.IsPostBack)
        {
            currentuser = Session["currentuser"].ToString();
            currentbranch = Session["currentbranch"].ToString();
            Working_Period = Session["WorkingPeriod"].ToString();
            hdnbranch.Value = currentbranch;
            hdnuser.Value = currentuser;
            DataTable dtt = new DataTable();
            GV.DataSource = dtt;
            GV.DataBind();
            GV1.DataSource = dtt;
            GV1.DataBind();

            //----------------SETTING SCREEN PERMISSION---------------//
            //Screen_Id = 11;
            //----------------SETTING SCREEN PERMISSION---------------//
            //Page_Rights(Screen_Id);
            btnSave.Visible = true;
            btnUpdate.Visible = false;
            btnDelete.Visible = false;
            btnrpt.Visible = false;
            T.Visible = false;

            txt_tot_Rev_Amt.Attributes.Add("readonly", "readonly");
            txtnon_tax_amt.Attributes.Add("readonly", "readonly");
            txttax_amt.Attributes.Add("readonly", "readonly");
            txt_Expense_Amt.Attributes.Add("readonly", "readonly");
            Load_Bank_Details();
            Auto_Debitno();
            Checked_Jobs();
            if (Request.QueryString["Page"] == null)
            {

                txtDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                HDupdate_id.Value = "";
            }
            else
            {
                if (Request.QueryString["Page"] == "GI")
                {
                    Rd_Imp_Exp.SelectedValue = Request.QueryString["IMP_EXP"].ToString();
                    ddl_tr_mode.SelectedValue = Request.QueryString["MODE"].ToString();

                    txtDate.Text = DateTime.Now.ToShortDateString();
                    txt_Cus_name.Text = Request.QueryString["PARTY_NAME"].ToString();
                    ddlType.SelectedValue = Request.QueryString["Type"].ToString();


                    ddlbranch_No.SelectedValue = Request.QueryString["CUS_BRANCH_NO_LOC"].ToString();
                    List<string> JOBN = new List<string>();
                    JOBN.Add(Request.QueryString["JOBNO"].ToString());
                    cbList.DataSource = JOBN;
                    cbList.DataBind();

                }
                else if (Request.QueryString["Page"] == "CAN")
                {
                    DataSet ds = new DataSet();
                    ObjUBO.A4 = Request.QueryString["JOBNO"].ToString();
                    ObjUBO.A5 = Request.QueryString["MODE"].ToString();
                    ObjUBO.A7 = Request.QueryString["IMP_EXP"].ToString();
                    ObjUBO.A10 = Request.QueryString["Type"].ToString();
                    ObjUBO.A12 = hdnbranch.Value;
                    ObjUBO.A8 = "CANCHECK";
                    ds = BP.Select_Inv_Debit(ObjUBO);
                    if (ds.Tables[0].Rows.Count == 0)
                    {
                        if (ds.Tables[1].Rows.Count > 0)
                        {
                            txt_Cus_name.Text = ds.Tables[1].Rows[0]["CLIENT_NAME"].ToString();

                        }
                        Rd_Imp_Exp.SelectedValue = Request.QueryString["IMP_EXP"].ToString();
                        ddl_tr_mode.SelectedValue = Request.QueryString["MODE"].ToString();

                        txtDate.Text = DateTime.Now.ToShortDateString();

                        ddlType.SelectedValue = Request.QueryString["Type"].ToString();

                        List<string> JOBN = new List<string>();
                        JOBN.Add(Request.QueryString["JOBNO"].ToString());
                        cbList.DataSource = JOBN;
                        cbList.DataBind();
                        From_Quotation.Checked = true;
                        btnloadData_Click(new object(), new EventArgs());
                        if (ds.Tables[1].Rows.Count > 0)
                        {
                            txt_Cus_name.Text = ds.Tables[1].Rows[0]["CLIENT_NAME"].ToString();
                        }
                        Chkdepartment.Checked = true;
                        Chkdepartment_Changed(new object(), new EventArgs());
                        btnUpdate.Visible = false;
                        btnSave.Visible = true;
                        btnNew.Visible = false;
                        btnDelete.Visible = false;
                        btnrpt.Visible = false;

                    }
                    else
                    {
                        ViewState["Billinvno"] = ds.Tables[0].Rows[0]["BILL_INV_NO"].ToString();
                        ViewState["Page"] = ds.Tables[0].Rows[0]["ID"].ToString();
                        Update_Item_Load();
                        From_Quotation.Checked = true;
                        btnloadData_Click(new object(), new EventArgs());
                        Chkdepartment.Checked = true;
                        Chkdepartment_Changed(new object(), new EventArgs());
                        btnUpdate.Visible = true;
                        btnSave.Visible = false;
                        btnNew.Visible = false;
                        btnDelete.Visible = false;
                        btnrpt.Visible = false;

                    }


                }
                if (Request.QueryString["Page"] != null && Request.QueryString["Page"] != string.Empty && Request.QueryString["Page"] != "GI" && Request.QueryString["Page"] != "CAN")
                {
                    Update_Item_Load();
                    ViewState["UPDATED_ID"] = Request.QueryString["DEBIT_NO"].ToString();
                    btnDelete.Visible = true;
                    btnUpdate.Visible = true;
                    btnSave.Visible = false;

                    txtInvoiceNo.Enabled = false;
                    ddlCus_name.Enabled = false;
                    Rd_Imp_Exp.Enabled = false;
                    Rd_Bill_Type.Enabled = true;
                    ddlbranch_No.Enabled = false;
                    ddl_tr_mode.Enabled = false;
                    ddlContainerTypes.Enabled = false;
                    HD_Showcon.Value = "SAVED";
                    //btnrpt.Visible = false;
                }
            }
            //Printaccess();
            if (Connection.Company_License().ToLower() == "erf00011")
            {
                Lblfrmquot.Text = "From Quotation";
                if (currentuser.ToUpper() == "ACCMAA" || currentuser.ToUpper() == "ACCTUT" || currentuser.ToUpper() == "FWDCJB" || currentuser.ToUpper() == "ACCMAA1")
                { Chkacc.Enabled = true; ChkCancelInv.Enabled = true; Chkdepartment.Enabled = true; }
                else { btnUpdate.Enabled = false; }
                ddlbank_name.SelectedValue = "UNION BANK OF INDIA";
                //if (currentuser.ToUpper() != "ACCMAA1" && currentuser.ToUpper() != "ACCTUT" && currentuser.ToUpper() != "FWDCJB" ) 

            }
            else if (Connection.Company_License().ToLower() == "erf00018" || Connection.Company_License().ToLower() == "erf00026")
            {
                Lblfrmquot.Text = "From CAN";
                txtInvoiceNo1.Visible = true;
                lblinvno.Visible = true;
                Chkacc.Enabled = true; ChkCancelInv.Enabled = true; Chkdepartment.Enabled = true;

            }
            else if (Connection.Company_License().ToLower() == "erf00016")
            {
                Lblfrmquot.Text = "From Quotation";
                Chkacc.Enabled = true; ChkCancelInv.Enabled = true; Chkdepartment.Enabled = true;

            }
            else if (Connection.Company_License().ToLower() == "erf00001")
            {
                Lblfrmquot.Text = "From Quotation";
                if (currentuser.ToUpper() == "A")
                { Chkacc.Enabled = true; ChkCancelInv.Enabled = true; Chkdepartment.Enabled = true; }
                //ddlbank_name.SelectedValue = "UNION BANK OF INDIA";
            }
            else { Lblfrmquot.Text = "From Quotation"; }
            Rd_Bill_Type.Enabled = false;
            Rd_Tax_NonTax.Enabled = false;
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
        //txtImportername.Attributes.Add("onblur", "javascript:CallServerside_Importer('" + txtImportername.ClientID + "', '" + txtInvoiceNo.ClientID + "')");
        //txtInvoiceNo.Attributes.Add("onblur", "javascript:CallServerside_IMP_INVCHK('" + txtInvoiceNo.ClientID + "', '" + txtInvoiceNo.ClientID + "')");
    }
   
    public void Page_Show_Hide(int i)
    {
        bool is_page;
        if (i == 99)
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
    public void Page_Approval(int i)
    {

        for (i = Approval_ID; i <= Approval_ID + 2; i++)
        {
            user_Create.RetrieveAll_User_Approval_Details(i);
            APPROVAL_CATEGORY = user_Create.APPROVAL_CATEGORY;


            //---------------READ ONLY------------------//
            if (i == 7)
            {
                if (APPROVAL_CATEGORY == "False")
                {
                    Chkdepartment.Enabled = false;



                    Chkdepartment.ToolTip = "You don't have permission to access";

                }
                else
                {
                    Chkdepartment.Enabled = true;
                    Chkdepartment.ToolTip = "";
                }
            }
            else if (i == 8)
            {
                if (APPROVAL_CATEGORY == "False")
                {
                    Chkacc.Enabled = false;



                    Chkacc.ToolTip = "You don't have permission to access";

                }
                else
                {
                    Chkacc.Enabled = true;
                    Chkacc.ToolTip = "";
                }
            }
            else
            {
                if (APPROVAL_CATEGORY == "False")
                {
                    ChkCancelInv.Enabled = false;



                    ChkCancelInv.ToolTip = "You don't have permission to access";

                }
                else
                {
                    ChkCancelInv.Enabled = true;
                    ChkCancelInv.ToolTip = "";
                }
            }

        }

    }
    private void Printaccess()
    {
        if (Chkdepartment.Checked == true || Chkacc.Checked == true) { btnrpt.Visible = true; }
        else { btnrpt.Visible = false; }
    }

    private void Update_Item_Load()
    {
        DataSet dss = new DataSet();

        ddlCus_name.Items.Clear();
        if (Request.QueryString["Page"] != "CAN")
        {
            ObjUBO.A4 = Request.QueryString["DEBIT_NO"].ToString();
            ObjUBO.A11 = Request.QueryString["Page"].ToString();
            ObjUBO.A10 = Request.QueryString["TYPE"].ToString();
        }
        else
        {
            ObjUBO.A4 = ViewState["DEBIT_NO"].ToString();
            ObjUBO.A11 = ViewState["Page"].ToString();
            ObjUBO.A10 = Request.QueryString["TYPE"].ToString();
        }
        ObjUBO.A7 = Request.QueryString["IMP_EXP"].ToString();
        ObjUBO.A12 = hdnbranch.Value;
        ObjUBO.A8 = "SELECT_CON";
        dss = BP.Select_Inv_Debit(ObjUBO);

        if (dss.Tables[0].Rows.Count > 0)
        {
            if (dss.Tables[0].Rows[0]["DEPT_APPROV"].ToString() != "N" && dss.Tables[0].Rows[0]["DEPT_APPROV"].ToString() != "" && dss.Tables[0].Rows[0]["DEPT_APPROV"].ToString() != null)
            {
                //lbldeptapp.Text=ss.Tables[0].Rows[0][2].ToString();
                btnrpt.Visible = true;
            }
            if (dss.Tables[0].Rows[0]["ACC_APPROV"].ToString() != "N" && dss.Tables[0].Rows[0]["ACC_APPROV"].ToString() != "" && dss.Tables[0].Rows[0]["ACC_APPROV"].ToString() != null)
            {
                //lbldeptapp.Text=ss.Tables[0].Rows[0][2].ToString();
                btnrpt.Visible = true;
            }
            //-----------------------BA--S---------------------------------------
            HDupdate_id.Value = dss.Tables[0].Rows[0]["ID"].ToString();
            HDupdate_IMP_EXP_id.Value = dss.Tables[0].Rows[0]["IMP_EXP"].ToString();
            Rd_Imp_Exp.SelectedValue = dss.Tables[0].Rows[0]["IMP_EXP"].ToString();
            Hdnmode.Value = dss.Tables[0].Rows[0]["MODE"].ToString();
            Hdntype.Value = dss.Tables[0].Rows[0]["TYPE"].ToString();
            ddlType.SelectedValue = dss.Tables[0].Rows[0]["TYPE"].ToString();

            txtDebitNo.Text = dss.Tables[0].Rows[0]["DEBIT_NO"].ToString();
            Session["DEBIT_NO"] = dss.Tables[0].Rows[0]["DEBIT_NO"].ToString();
            txtInvoiceNo.Text = dss.Tables[0].Rows[0]["BILL_INV_NO"].ToString();
            txtInvoiceNo1.Text = dss.Tables[0].Rows[0]["ALT_INV_NO"].ToString();
            txtDate.Text = dss.Tables[0].Rows[0]["DEBIT_DATE"].ToString();
            txt_Cus_name.Text = dss.Tables[0].Rows[0]["PARTY_NAME"].ToString();

            if (dss.Tables[0].Rows[0]["AC_BEHALF"].ToString() == "Y")
            {
                ch_Behalf.Checked = true;
            }

            ddl_state_name.SelectedValue = dss.Tables[0].Rows[0]["STATE_NAME"].ToString();
            txt_GSTN_Id.Text = dss.Tables[0].Rows[0]["GSTN_ID"].ToString();
            txt_Party_Add.Text = dss.Tables[0].Rows[0]["PARTY_ADDRESS"].ToString();
            Rd_Tax_NonTax.SelectedValue = dss.Tables[0].Rows[0]["TAX_NON_TAX"].ToString();
            Rd_Bill_Type.SelectedValue = dss.Tables[0].Rows[0]["LOCAL_OTHER"].ToString();
            ddlbank_name.SelectedValue = dss.Tables[0].Rows[0]["BANK_DETAILS"].ToString();

            ddl_tr_mode.SelectedValue = dss.Tables[0].Rows[0]["MODE"].ToString();
            shipmenttype1();
            ddlshipment_type.Text = dss.Tables[0].Rows[0]["SHIP_TYPE"].ToString();

            txtAccHead.Text = dss.Tables[0].Rows[0]["ACC_HEAD"].ToString();
            ddlContainerTypes.SelectedValue = dss.Tables[0].Rows[0]["CONTAINER_TYPE"].ToString();
            txtPONumber.Text = dss.Tables[0].Rows[0]["PO_NO"].ToString();
            txtVerifiedBy.Text = dss.Tables[0].Rows[0]["VERIFIED_BY"].ToString();
            txtDueDate.Text = dss.Tables[0].Rows[0]["DUE_DATE"].ToString();
            if (dss.Tables[0].Rows[0]["DEPT_APPROV"].ToString() != "N")
            {
                Chkdepartment.Checked = true;
                if (dss.Tables[0].Rows[0]["DEPT_APPROV"].ToString() != "Y")
                {
                    lbldeptapp.Text = dss.Tables[0].Rows[0]["DEPT_APPROV"].ToString();
                }
            }
            else { Chkdepartment.Checked = false; }
            if (dss.Tables[0].Rows[0]["ACC_APPROV"].ToString() != "N")
            {
                Chkacc.Checked = true;
                if (dss.Tables[0].Rows[0]["ACC_APPROV"].ToString() != "Y")
                {
                    lblaccapp.Text = dss.Tables[0].Rows[0]["ACC_APPROV"].ToString();
                }
            }
            else { Chkacc.Checked = false; }
            if (dss.Tables[0].Rows[0]["JOB_STATUS"].ToString() != "Approved" && dss.Tables[0].Rows[0]["JOB_STATUS"].ToString() != "")
            {
                ChkCancelInv.Checked = true;
                if (dss.Tables[0].Rows[0]["JOB_STATUS"].ToString() != "Canceled")
                {
                    lblcancel.Text = dss.Tables[0].Rows[0]["JOB_STATUS"].ToString();
                }
            }
            else { ChkCancelInv.Checked = false; }
            txtNarration.Text = dss.Tables[0].Rows[0]["NARRATION"].ToString();
            ddlworking_Period.SelectedValue = dss.Tables[0].Rows[0]["WORKING_PERIOD_DETAILS"].ToString();
        }


        if (dss.Tables[1].Rows.Count > 0)
        {
            gv_Gen_Item_I.DataSource = dss.Tables[1];
            gv_Gen_Item_I.DataBind();
        }
        else
        {
            gv_Gen_Item_I.DataSource = dt;
            gv_Gen_Item_I.DataBind();
        }

        if (dss.Tables[2].Rows.Count > 0)
        {
            gv_Chg_Details.DataSource = dss.Tables[2];
            gv_Chg_Details.DataBind();

            DataTable firstTable = dss.Tables[2];
            ViewState["CurrentTable"] = firstTable;
        }
        else
        {
            SetInitialRow();
        }
        ddlworking_Period.Enabled = false;

    }


    protected void gvAll_RowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Height = 20;
        }
    }
    protected void ButtonAdd_Click(object sender, EventArgs e)
    {
        AddNewRowToGrid();
        //txtfocus.Focus();
        ScriptManager.RegisterStartupScript(this, typeof(Page), "OnClientClicking", "B_G_tab_page2();", true);
    }
    private void AddNewRowToGrid()
    {
        int rowIndex = 0;
        gst_toal = 0;
        Reverse_total = 0;
        string Emptyrow = string.Empty;

        if (ViewState["CurrentTable"] != null)
        {
            DataTable dtCurrentTable = (DataTable)ViewState["CurrentTable"];
            DataRow drCurrentRow = null;
            if (dtCurrentTable.Rows.Count > 0)
            {
                for (int i = 1; i <= dtCurrentTable.Rows.Count; i++)
                {
                    CheckBox INV_CHECK = (CheckBox)gv_Chg_Details.Rows[rowIndex].Cells[1].FindControl("chk_inv");
                    TextBox txtOrder_by = (TextBox)gv_Chg_Details.Rows[rowIndex].Cells[2].FindControl("txtOrder_by");


                    TextBox txtch_name = (TextBox)gv_Chg_Details.Rows[rowIndex].Cells[3].FindControl("txtch_name");
                    TextBox txt_charge_desc = (TextBox)gv_Chg_Details.Rows[rowIndex].Cells[3].FindControl("txt_charge_desc");

                    DropDownList ddl_tax_nontax = (DropDownList)gv_Chg_Details.Rows[rowIndex].Cells[4].FindControl("ddl_tax_nontax");
                    TextBox txt_HSN_Code = (TextBox)gv_Chg_Details.Rows[rowIndex].Cells[5].FindControl("txt_HSN_Code");
                    TextBox txt_SA_Code = (TextBox)gv_Chg_Details.Rows[rowIndex].Cells[6].FindControl("txt_SA_Code");

                    TextBox txtqty = (TextBox)gv_Chg_Details.Rows[rowIndex].Cells[7].FindControl("txtqty");
                    DropDownList ddl_CUR = (DropDownList)gv_Chg_Details.Rows[rowIndex].Cells[8].FindControl("ddl_CUR");
                    TextBox txt_Ex_Rate = (TextBox)gv_Chg_Details.Rows[rowIndex].Cells[9].FindControl("txt_Ex_Rate");
                    TextBox txtunit_price = (TextBox)gv_Chg_Details.Rows[rowIndex].Cells[10].FindControl("txtunit_price");
                    TextBox txtamt = (TextBox)gv_Chg_Details.Rows[rowIndex].Cells[11].FindControl("txtamt");
                    TextBox txtCGST_RATE = (TextBox)gv_Chg_Details.Rows[rowIndex].Cells[12].FindControl("txtCGST_RATE");
                    TextBox txtCGST_AMT = (TextBox)gv_Chg_Details.Rows[rowIndex].Cells[13].FindControl("txtCGST_AMT");
                    TextBox txtSGST_RATE = (TextBox)gv_Chg_Details.Rows[rowIndex].Cells[14].FindControl("txtSGST_RATE");
                    TextBox txtSGST_AMT = (TextBox)gv_Chg_Details.Rows[rowIndex].Cells[15].FindControl("txtSGST_AMT");
                    TextBox txtIGST_RATE = (TextBox)gv_Chg_Details.Rows[rowIndex].Cells[16].FindControl("txtIGST_RATE");
                    TextBox txtIGST_AMT = (TextBox)gv_Chg_Details.Rows[rowIndex].Cells[17].FindControl("txtIGST_AMT");


                    string Amt = string.Empty;
                    if (txtamt.Text == string.Empty || txtamt.Text == "0" || txtamt.Text == "0.00")
                    {
                        Amt = string.Empty;
                    }
                    else
                    {
                        Amt = "Yes";
                    }

                    if (INV_CHECK.Checked == true)
                    {
                        if (txtch_name.Text == string.Empty || txtunit_price.Text == string.Empty || txtunit_price.Text == "0.00" || Amt == string.Empty)
                        {
                            Emptyrow = "Y";
                        }
                    }

                    drCurrentRow = dtCurrentTable.NewRow();
                    dtCurrentTable.Rows[i - 1]["INV_CHECK"] = INV_CHECK.Checked == true ? "True" : "False";
                    drCurrentRow["RowNumber"] = i + 1;
                    dtCurrentTable.Rows[i - 1]["ORDERBY"] = txtOrder_by.Text;

                    dtCurrentTable.Rows[i - 1]["CHARGE_NAME"] = txtch_name.Text;
                    dtCurrentTable.Rows[i - 1]["TAX_NONTAX"] = ddl_tax_nontax.SelectedValue.ToString();

                    dtCurrentTable.Rows[i - 1]["HSN_CODE"] = txt_HSN_Code.Text;
                    dtCurrentTable.Rows[i - 1]["SA_CODE"] = txt_SA_Code.Text;

                    dtCurrentTable.Rows[i - 1]["QTY"] = txtqty.Text;
                    dtCurrentTable.Rows[i - 1]["CUR"] = ddl_CUR.SelectedValue.ToString();
                    dtCurrentTable.Rows[i - 1]["EX_RATE"] = txt_Ex_Rate.Text;
                    dtCurrentTable.Rows[i - 1]["UNIT_RATE"] = txtunit_price.Text;
                    dtCurrentTable.Rows[i - 1]["AMOUNT"] = txtamt.Text;
                    dtCurrentTable.Rows[i - 1]["CGST_RATE"] = txtCGST_RATE.Text;
                    dtCurrentTable.Rows[i - 1]["CGST_AMT"] = txtCGST_AMT.Text;
                    dtCurrentTable.Rows[i - 1]["SGST_RATE"] = txtSGST_RATE.Text;
                    dtCurrentTable.Rows[i - 1]["SGST_AMT"] = txtSGST_AMT.Text;
                    dtCurrentTable.Rows[i - 1]["IGST_RATE"] = txtIGST_RATE.Text;
                    dtCurrentTable.Rows[i - 1]["IGST_AMT"] = txtIGST_AMT.Text;
                    dtCurrentTable.Rows[i - 1]["CHARGE_DESC"] = txt_charge_desc.Text;
                    rowIndex++;

                    if (ddl_tax_nontax.SelectedValue.ToString() == "T" && INV_CHECK.Checked == true)
                    {

                        if (Rd_Bill_Type.SelectedValue.ToString() == "L")
                        {
                            gst_toal = gst_toal + (Convert.ToDecimal(txtCGST_AMT.Text == "" ? "0.00" : txtCGST_AMT.Text)
                                              + Convert.ToDecimal(txtSGST_AMT.Text == "" ? "0.00" : txtSGST_AMT.Text));
                        }
                        else
                        {
                            gst_toal = gst_toal + Convert.ToDecimal(txtIGST_AMT.Text == "" ? "0.00" : txtIGST_AMT.Text);
                        }
                        tax_amt += Convert.ToDecimal(txtamt.Text == "" ? "0.00" : txtamt.Text);

                    }
                    else if (INV_CHECK.Checked == true && ddl_tax_nontax.SelectedValue.ToString() == "N")
                    {
                        non_tax_amt = non_tax_amt + Convert.ToDecimal(txtamt.Text == "" ? "0.00" : txtamt.Text);
                    }
                    else if (INV_CHECK.Checked == true && ddl_tax_nontax.SelectedValue.ToString() == "E")
                    {
                        non_tax_amt = non_tax_amt + Convert.ToDecimal(txtamt.Text == "" ? "0.00" : txtamt.Text);
                    }
                    else if (ddl_tax_nontax.SelectedValue.ToString() == "P" && INV_CHECK.Checked == true)
                    {
                        gst_toal = gst_toal + (Convert.ToDecimal(txtCGST_AMT.Text == "" ? "0.00" : txtCGST_AMT.Text)
                                              + Convert.ToDecimal(txtSGST_AMT.Text == "" ? "0.00" : txtSGST_AMT.Text)
                                              + Convert.ToDecimal(txtIGST_AMT.Text == "" ? "0.00" : txtIGST_AMT.Text));

                        tax_amt += Convert.ToDecimal(txtamt.Text == "" ? "0.00" : txtamt.Text);
                    }
                    else if (ddl_tax_nontax.SelectedValue.ToString() == "R" && INV_CHECK.Checked == true)
                    {
                        if (Rd_Bill_Type.SelectedValue.ToString() == "L")
                        {
                            Reverse_total = Reverse_total + (
                                (Convert.ToDecimal(txtamt.Text == "" ? "0.00" : txtamt.Text) * Convert.ToDecimal(txtCGST_RATE.Text == "" ? "0.00" : txtCGST_RATE.Text) / 100)
                                + (Convert.ToDecimal(txtamt.Text == "" ? "0.00" : txtamt.Text) * Convert.ToDecimal(txtSGST_RATE.Text == "" ? "0.00" : txtSGST_RATE.Text) / 100)
                                );
                        }
                        else
                        {
                            Reverse_total = Reverse_total + (
                            (Convert.ToDecimal(txtamt.Text == "" ? "0.00" : txtamt.Text) * Convert.ToDecimal(txtIGST_RATE.Text == "" ? "0.00" : txtIGST_RATE.Text) / 100)
                            );
                        }
                        tax_amt += Convert.ToDecimal(txtamt.Text == "" ? "0.00" : txtamt.Text);
                    }
                }
                if (Emptyrow != "Y")
                {

                    drCurrentRow = dtCurrentTable.NewRow();
                    drCurrentRow["INV_CHECK"] = "True";
                    drCurrentRow["RowNumber"] = dtCurrentTable.Rows.Count + 1;
                    drCurrentRow["ORDERBY"] = dtCurrentTable.Rows.Count + 1;

                    drCurrentRow["CHARGE_NAME"] = string.Empty;
                    drCurrentRow["TAX_NONTAX"] = "N";

                    drCurrentRow["HSN_CODE"] = "";
                    drCurrentRow["SA_CODE"] = "";

                    drCurrentRow["QTY"] = "1";
                    drCurrentRow["CUR"] = "INR";
                    drCurrentRow["EX_RATE"] = "1";

                    drCurrentRow["UNIT_RATE"] = "0";
                    drCurrentRow["AMOUNT"] = "0";

                    drCurrentRow["CGST_RATE"] = "0";
                    drCurrentRow["CGST_AMT"] = "0";
                    drCurrentRow["SGST_RATE"] = "0";
                    drCurrentRow["SGST_AMT"] = "0";

                    drCurrentRow["IGST_RATE"] = "0";
                    drCurrentRow["IGST_AMT"] = "0";
                    drCurrentRow["CHARGE_DESC"] = string.Empty;

                    dtCurrentTable.Rows.Add(drCurrentRow);
                    ViewState["CurrentTable"] = dtCurrentTable;

                    gv_Chg_Details.DataSource = dtCurrentTable;
                    gv_Chg_Details.DataBind();

                }
                else
                {
                    Alert_msg("Enter the value in Empty Row");
                }

                txtnon_tax_amt.Text = non_tax_amt.ToString();
                txttax_amt.Text = tax_amt.ToString();
                txt_total_tax_amt.Text = gst_toal.ToString();
                //-----------------------------
                txt_tot_Rev_Amt.Text = Reverse_total.ToString();

                //----------------------------
                txt_total_amt_With_tax.Text = (Convert.ToDecimal(txt_total_tax_amt.Text) + Convert.ToDecimal(txttax_amt.Text)).ToString("#0.00");
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
                    CheckBox INV_CHECK = (CheckBox)gv_Chg_Details.Rows[rowIndex].Cells[0].FindControl("chk_inv");
                    TextBox txtch_name = (TextBox)gv_Chg_Details.Rows[rowIndex].Cells[1].FindControl("txtch_name");
                    TextBox txt_charge_desc = (TextBox)gv_Chg_Details.Rows[rowIndex].Cells[1].FindControl("txt_charge_desc");

                    TextBox txtOrder_by = (TextBox)gv_Chg_Details.Rows[rowIndex].Cells[2].FindControl("txtOrder_by");
                    DropDownList ddl_tax_nontax = (DropDownList)gv_Chg_Details.Rows[rowIndex].Cells[3].FindControl("ddl_tax_nontax");

                    TextBox txt_HSN_Code = (TextBox)gv_Chg_Details.Rows[rowIndex].Cells[4].FindControl("txt_HSN_Code");
                    TextBox txt_SA_Code = (TextBox)gv_Chg_Details.Rows[rowIndex].Cells[5].FindControl("txt_SA_Code");

                    TextBox txtqty = (TextBox)gv_Chg_Details.Rows[rowIndex].Cells[6].FindControl("txtqty");
                    DropDownList ddl_CUR = (DropDownList)gv_Chg_Details.Rows[rowIndex].Cells[7].FindControl("ddl_CUR");
                    TextBox txt_Ex_Rate = (TextBox)gv_Chg_Details.Rows[rowIndex].Cells[8].FindControl("txt_Ex_Rate");

                    TextBox txtunit_price = (TextBox)gv_Chg_Details.Rows[rowIndex].Cells[9].FindControl("txtunit_price");
                    TextBox txtamt = (TextBox)gv_Chg_Details.Rows[rowIndex].Cells[10].FindControl("txtamt");
                    TextBox txtCGST_RATE = (TextBox)gv_Chg_Details.Rows[rowIndex].Cells[11].FindControl("txtCGST_RATE");
                    TextBox txtCGST_AMT = (TextBox)gv_Chg_Details.Rows[rowIndex].Cells[12].FindControl("txtCGST_AMT");
                    TextBox txtSGST_RATE = (TextBox)gv_Chg_Details.Rows[rowIndex].Cells[13].FindControl("txtSGST_RATE");
                    TextBox txtSGST_AMT = (TextBox)gv_Chg_Details.Rows[rowIndex].Cells[14].FindControl("txtSGST_AMT");
                    TextBox txtIGST_RATE = (TextBox)gv_Chg_Details.Rows[rowIndex].Cells[15].FindControl("txtIGST_RATE");
                    TextBox txtIGST_AMT = (TextBox)gv_Chg_Details.Rows[rowIndex].Cells[16].FindControl("txtIGST_AMT");
                    //--------------------------------------------------------------------

                    gv_Chg_Details.Rows[i].Cells[1].Text = Convert.ToString(i + 1);

                    txtch_name.Text = dt.Rows[i]["CHARGE_NAME"].ToString();
                    txtOrder_by.Text = dt.Rows[i]["ORDERBY"].ToString();
                    ddl_tax_nontax.SelectedValue = dt.Rows[i]["TAX_NONTAX"].ToString();

                    txt_HSN_Code.Text = dt.Rows[i]["HSN_CODE"].ToString();
                    txt_SA_Code.Text = dt.Rows[i]["SA_CODE"].ToString();

                    txtqty.Text = dt.Rows[i]["QTY"].ToString();
                    ddl_CUR.SelectedValue = dt.Rows[i]["CUR"].ToString();
                    txt_Ex_Rate.Text = dt.Rows[i]["EX_RATE"].ToString();

                    txtunit_price.Text = dt.Rows[i]["UNIT_RATE"].ToString();
                    txtamt.Text = dt.Rows[i]["AMOUNT"].ToString();
                    txtCGST_RATE.Text = dt.Rows[i]["CGST_RATE"].ToString();
                    txtCGST_AMT.Text = dt.Rows[i]["CGST_AMT"].ToString();
                    txtSGST_RATE.Text = dt.Rows[i]["SGST_RATE"].ToString();
                    txtSGST_AMT.Text = dt.Rows[i]["SGST_AMT"].ToString();
                    txtIGST_RATE.Text = dt.Rows[i]["IGST_RATE"].ToString();
                    txtIGST_AMT.Text = dt.Rows[i]["IGST_AMT"].ToString();

                    txt_charge_desc.Text = dt.Rows[i]["CHARGE_DESC"].ToString();
                    rowIndex++;
                }
            }
            TextBox txtch_namefocus = (TextBox)gv_Chg_Details.Rows[rowIndex - 1].Cells[3].FindControl("txtch_name");
            txtch_namefocus.Focus();
        }
    }
    public void Alert_msg(string msg)
    {
        string prompt = "<script>$(document).ready(function(){{jAlert('" + msg + "', 'DEBIT NOTE DETAILS', function (r) {});}});</script>";
        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", prompt, false);
    }
    public void Alert_msg(string msg, string focus)
    {
        string prompt = "<script>$(document).ready(function(){{jAlert('" + msg + "', 'DEBIT NOTE DETAILS', function (r) {document.getElementById('" + focus + "').focus();});}});</script>";
        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", prompt, false);
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        string Re_Inv = string.Empty;
        DataSet ss = new DataSet();
        hd_Brslno.Value = ddlbranch_No.SelectedValue.ToString();
        //--------------------------------------------------------------------------------------------------------------------------
        ss = Billing_Invoice_Jobno_Insert_Update("S", Product_Item_I_details(), Product_details());
        //ss = Billing_Invoice_Jobno_Insert_Update(Get_Jobs(), "S",  Product_Item_II_details());
        if (ss.Tables[0].Rows.Count > 0)
        {
            //-----------------------FOR USER RIGHTS--------------------------------//
            user_Create.RetrieveAll_User_Screen_Rights_Details(Screen_Id);
            PAGE_MODIFY = user_Create.PAGE_MODIFY;
            //---------------SAVE ONLY------------------//
            if (PAGE_MODIFY == "False")
            {
                btnNew_Click1(sender, e);
            }
            else
            {
                //btnUpdate.CssClass = "save";
                //btnUpdate.CssClass = "save";
                btnUpdate.Visible = true;
                btnSave.Visible = false;
                btnSave.Focus();
            }
            //-----------------------FOR USER RIGHTS--------------------------------//

            ViewState["UPDATED_ID"] = null;
            HD_Showcon.Value = "SAVED";
            Alert_msg("Saved Successfully", "btnSave");

            HDupdate_id.Value = ss.Tables[0].Rows[0][0].ToString();
            txtInvoiceNo.Text = hdninvoice.Value = ss.Tables[0].Rows[0][1].ToString();
            txtInvoiceNo1.Text = ss.Tables[0].Rows[0]["ALT_INV_NO"].ToString();
            Session["DEBIT_NO"] = ss.Tables[0].Rows[0][5].ToString();
            HDupdate_IMP_EXP_id.Value = Rd_Imp_Exp.SelectedValue.ToString();
            Hdnmode.Value = ddl_tr_mode.SelectedValue.ToString();
            Hdntype.Value = ddlType.SelectedValue.ToString();
            hdnmisc.Value = drpmisc.SelectedValue;
            if (ss.Tables[0].Rows[0]["DEPT_APPROV"].ToString() != "N" && ss.Tables[0].Rows[0]["DEPT_APPROV"].ToString() != "" && ss.Tables[0].Rows[0]["DEPT_APPROV"].ToString() != null)
            {
                //lbldeptapp.Text=ss.Tables[0].Rows[0][2].ToString();
                btnrpt.Visible = true;
            }
            if (ss.Tables[0].Rows[0]["ACC_APPROV"].ToString() != "N" && ss.Tables[0].Rows[0]["ACC_APPROV"].ToString() != "" && ss.Tables[0].Rows[0]["ACC_APPROV"].ToString() != null)
            {
                //lbldeptapp.Text=ss.Tables[0].Rows[0][2].ToString();
                btnrpt.Visible = true;
            }

            //btnrpt.Visible = true;

            F.Visible = false;
            T.Visible = true;
            /*
            ddlCus_name.Attributes.Add("readonly", "readonly");
            ddlCus_name.Style.Add("background-color", "#F0F0F0");

            txtInvoiceNo.Attributes.Add("readonly", "readonly");
            txtInvoiceNo.Style.Add("background-color", "#F0F0F0");
            */
            ddlCus_name.Enabled = false;
            ddlbranch_No.Enabled = false;
            ddlworking_Period.Enabled = false;
        }
        else
        {
            btnUpdate.Visible = false;
            btnDelete.Visible = false;
            btnSave.Visible = true;
            btnNew.Visible = true;
            Alert_msg("Not Saved", "btnSave");
        }
        if (Request.QueryString["Page"] == "CAN")
        {
            Page.ClientScript.RegisterStartupScript(this.GetType(), "Open", "javascript:inv('" + HDupdate_id.Value + "','" + HDupdate_IMP_EXP_id.Value + "','" + hdninvoice.Value + "','" + Hdnmode.Value + "','" + Hdntype.Value + "','" + hdnmisc.Value + "')", true);
        }
        //-------------------------------------------------------------
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
    protected void btnUpdate_Click(object sender, EventArgs e)
    {
        DataSet dr = new DataSet();
        try
        {
            //string j = cbList.Items[0].Text;
            //hd_Brslno.Value = ddlbranch_No.SelectedValue.ToString();
            //dr = Billing_Invoice_Jobno_Insert_Update(Get_Jobs(), "U", Product_Item_I_details(), Product_details(), Product_Annexure_details());
            dr = Billing_Invoice_Jobno_Insert_Update("U", Product_Item_I_details(), Product_details());

            //dr = Billing_Invoice_Jobno_Insert_Update(Get_Jobs(), "U", Product_Item_II_details());
            if (dr.Tables[0].Rows.Count > 0)
            {
                //if (ViewState["UPDATED_ID"] != null)
                //{
                Alert_msg("Updated Successfully", "btnUpdate");
                HDupdate_id.Value = dr.Tables[0].Rows[0][0].ToString();
                txtInvoiceNo.Text = hdninvoice.Value = dr.Tables[0].Rows[0][1].ToString();
                txtInvoiceNo1.Text = dr.Tables[0].Rows[0]["ALT_INV_NO"].ToString();
                Session["DEBIT_NO"] = dr.Tables[0].Rows[0][5].ToString();
                HDupdate_IMP_EXP_id.Value = Rd_Imp_Exp.SelectedValue.ToString();
                Hdnmode.Value = ddl_tr_mode.SelectedValue.ToString();
                Hdntype.Value = ddlType.SelectedValue.ToString();
                hdnmisc.Value = drpmisc.SelectedValue;
                if (dr.Tables[0].Rows[0][2].ToString() != "N" && dr.Tables[0].Rows[0][2].ToString() != "" && dr.Tables[0].Rows[0][2].ToString() != null)
                {

                    btnrpt.Visible = true;
                }
                if (dr.Tables[0].Rows[0][3].ToString() != "N" && dr.Tables[0].Rows[0][3].ToString() != "" && dr.Tables[0].Rows[0][3].ToString() != null)
                {

                    btnrpt.Visible = true;
                }

            }
            else
            {
                btnUpdate.Visible = true;
                btnDelete.Visible = true;
                btnSave.Visible = false;
                btnNew.Visible = true;
                Alert_msg("Not Saved", "btnUpdate");
            }
            if (Request.QueryString["Page"] == "CAN")
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "Open", "javascript:inv('" + HDupdate_id.Value + "','" + HDupdate_IMP_EXP_id.Value + "','" + hdninvoice.Value + "','" + Hdnmode.Value + "','" + Hdntype.Value + "','" + hdnmisc.Value + "')", true);
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
            DataSet dss = new DataSet();
            ObjUBO.A4 = txtDebitNo.Text;
            ObjUBO.A8 = "Delete";
            ObjUBO.A12 = hdnbranch.Value;
             ObjUBO.A10 = ddlType.SelectedValue;
            dss = BP.Select_Inv_Debit(ObjUBO);
            if (dss.Tables[0].Rows.Count > 0)
            {
                Alert_msg("Not Deleted");
            }
            else
            {
                btnNew_Click1(sender, e);
                Alert_msg("Deleted");
                btnrpt.Visible = false;
            }
        }
        catch (Exception ex)
        {
            Connection.Error_Msg(ex.Message);
        }
    }
    private string Product_Item_I_details()
    {
        DataTable dtt = new DataTable();
        DataRow dr;

        dtt.Columns.Add(new System.Data.DataColumn("JOBNO", typeof(String)));
        dtt.Columns.Add(new System.Data.DataColumn("TR_MODE", typeof(String)));
        dtt.Columns.Add(new System.Data.DataColumn("FILE_REF_NO", typeof(String)));
        //dtt.Columns.Add(new System.Data.DataColumn("PARTY_REF_NO", typeof(String)));
        dtt.Columns.Add(new System.Data.DataColumn("NO_OF_PKGS", typeof(String)));
        dtt.Columns.Add(new System.Data.DataColumn("PKGS_TYPE", typeof(String)));
        dtt.Columns.Add(new System.Data.DataColumn("FLT_VSL_NO", typeof(String)));
        dtt.Columns.Add(new System.Data.DataColumn("MAWB_MBL_NO", typeof(String)));
        dtt.Columns.Add(new System.Data.DataColumn("MAWB_MBL_DATE", typeof(String)));
        dtt.Columns.Add(new System.Data.DataColumn("HAWB_HBL_NO", typeof(String)));
        dtt.Columns.Add(new System.Data.DataColumn("HAWB_HBL_DATE", typeof(String)));
        dtt.Columns.Add(new System.Data.DataColumn("GROSS_WGT", typeof(String)));
        dtt.Columns.Add(new System.Data.DataColumn("GROSS_WGT_TYPE", typeof(String)));
        dtt.Columns.Add(new System.Data.DataColumn("NET_WGT", typeof(String)));
        dtt.Columns.Add(new System.Data.DataColumn("NET_WGT_TYPE", typeof(String)));

        int R = 1;
        foreach (GridViewRow row in gv_Gen_Item_I.Rows)
        {
            TextBox txt_Jobno = (TextBox)row.FindControl("txt_Jobno_I");
            TextBox txt_tr_mode = (TextBox)row.FindControl("txt_tr_mode");
            TextBox txt_file_Refno = (TextBox)row.FindControl("txt_file_Refno");
            //TextBox txt_Party_Refno = (TextBox)row.FindControl("txt_Party_Refno");
            TextBox txt_no_of_pkg = (TextBox)row.FindControl("txt_no_of_pkg");
            TextBox txt_no_of_pkg_type = (TextBox)row.FindControl("txt_no_of_pkg_type");
            TextBox txt_flt_vessel_no = (TextBox)row.FindControl("txt_flt_vessel_no");
            TextBox txt_MAWB_MBL = (TextBox)row.FindControl("txt_MAWB_MBL");
            TextBox txt_MAWB_MBL_Date = (TextBox)row.FindControl("txt_MAWB_MBL_Date");
            TextBox txt_HAWB_HBLNO = (TextBox)row.FindControl("txt_HAWB_HBLNO");
            TextBox txt_HAWB_HBLDATE = (TextBox)row.FindControl("txt_HAWB_HBLDATE");
            TextBox txt_Gross_Wgt = (TextBox)row.FindControl("txt_Gross_Wgt");
            TextBox txt_Gross_Wgt_Type = (TextBox)row.FindControl("txt_Gross_Wgt_Type");
            TextBox txt_net_Wgt = (TextBox)row.FindControl("txt_net_Wgt");
            TextBox txt_net_Wgt_Type = (TextBox)row.FindControl("txt_net_Wgt_Type");

            dr = dtt.NewRow();
            dr[0] = txt_Jobno.Text;
            dr[1] = txt_tr_mode.Text;
            dr[2] = txt_file_Refno.Text;
            //dr[3] = txt_Party_Refno.Text;
            dr[3] = txt_no_of_pkg.Text;
            dr[4] = txt_no_of_pkg_type.Text;
            dr[5] = txt_flt_vessel_no.Text;
            dr[6] = txt_MAWB_MBL.Text;
            dr[7] = txt_MAWB_MBL_Date.Text;
            dr[8] = txt_HAWB_HBLNO.Text;
            dr[9] = txt_HAWB_HBLDATE.Text;
            dr[10] = txt_Gross_Wgt.Text;
            dr[11] = txt_Gross_Wgt_Type.Text;
            dr[12] = txt_net_Wgt.Text;
            dr[13] = txt_net_Wgt_Type.Text;
            dtt.Rows.Add(dr);
            R = R + 1;
        }
        DataSet dsData = new DataSet();
        dsData.Tables.Add(dtt);
        String xmlData = ConvertDataTableToXML(dsData.Tables[0]);
        return xmlData;
    }
    private string Product_Item_II_details()
    {
        DataTable dtt = new DataTable();
        DataRow dr;

        dtt.Columns.Add(new System.Data.DataColumn("JOBNO", typeof(String)));
        dtt.Columns.Add(new System.Data.DataColumn("INV_NO", typeof(String)));
        dtt.Columns.Add(new System.Data.DataColumn("INV_DATE", typeof(String)));
        //dtt.Columns.Add(new System.Data.DataColumn("ITEM_DESC", typeof(String)));
        //dtt.Columns.Add(new System.Data.DataColumn("ASS_VALUE", typeof(String)));
        //dtt.Columns.Add(new System.Data.DataColumn("DUTY_VALUE", typeof(String)));
        //dtt.Columns.Add(new System.Data.DataColumn("CIF_VALUE", typeof(String)));
        dtt.Columns.Add(new System.Data.DataColumn("BE_SB_NO", typeof(String)));
        dtt.Columns.Add(new System.Data.DataColumn("BE_SB_DATE", typeof(String)));

        int R = 1;
        //foreach (GridViewRow row in gv_Gen_Item_II.Rows)
        //{
        //    TextBox txt_Jobno = (TextBox)row.FindControl("txt_Jobno_II");
        //    TextBox txt_Inv_No = (TextBox)row.FindControl("txt_Inv_No");
        //    TextBox txt_Inv_Date = (TextBox)row.FindControl("txt_Inv_Date");
        //    //TextBox txt_Item_Dec = (TextBox)row.FindControl("txt_Item_Dec");
        //    //TextBox txt_Ass_Value = (TextBox)row.FindControl("txt_Ass_Value");
        //    //TextBox txt_Duty_Value = (TextBox)row.FindControl("txt_Duty_Value");
        //    //TextBox txt_CIF_Value = (TextBox)row.FindControl("txt_CIF_Value");
        //    TextBox txt_BE_SB_NO = (TextBox)row.FindControl("txt_BE_SB_NO");
        //    TextBox txt_BE_SB_DATE = (TextBox)row.FindControl("txt_BE_SB_DATE");

        //    dr = dtt.NewRow();
        //    dr[0] = txt_Jobno.Text;
        //    dr[1] = txt_Inv_No.Text;
        //    dr[2] = txt_Inv_Date.Text;
        //    //dr[3] = txt_Item_Dec.Text;
        //    //dr[4] = txt_Ass_Value.Text;
        //    //dr[5] = txt_Duty_Value.Text;
        //    //dr[6] = txt_CIF_Value.Text;
        //    dr[3] = txt_BE_SB_NO.Text;
        //    dr[4] = txt_BE_SB_DATE.Text;
        //    dtt.Rows.Add(dr);
        //    R = R + 1;
        //}
        DataSet dsData = new DataSet();
        dsData.Tables.Add(dtt);
        String xmlData = ConvertDataTableToXML(dsData.Tables[0]);
        return xmlData;
    }
    private string Product_details()
    {
        DataTable dtt = new DataTable();
        DataRow dr;

        dtt.Columns.Add(new System.Data.DataColumn("INV_CHECK", typeof(String)));
        dtt.Columns.Add(new System.Data.DataColumn("CHARGE_NAME", typeof(String)));
        dtt.Columns.Add(new System.Data.DataColumn("TAX_NONTAX", typeof(String)));

        dtt.Columns.Add(new System.Data.DataColumn("HSN_CODE", typeof(String)));
        dtt.Columns.Add(new System.Data.DataColumn("SA_CODE", typeof(String)));

        dtt.Columns.Add(new System.Data.DataColumn("QTY", typeof(String)));
        dtt.Columns.Add(new System.Data.DataColumn("CUR", typeof(String)));
        dtt.Columns.Add(new System.Data.DataColumn("EX_RATE", typeof(String)));
        dtt.Columns.Add(new System.Data.DataColumn("UNIT_RATE", typeof(String)));
        dtt.Columns.Add(new System.Data.DataColumn("AMOUNT", typeof(String)));
        dtt.Columns.Add(new System.Data.DataColumn("CGST_RATE", typeof(String)));
        dtt.Columns.Add(new System.Data.DataColumn("CGST_AMT", typeof(String)));
        dtt.Columns.Add(new System.Data.DataColumn("SGST_RATE", typeof(String)));
        dtt.Columns.Add(new System.Data.DataColumn("SGST_AMT", typeof(String)));
        dtt.Columns.Add(new System.Data.DataColumn("IGST_RATE", typeof(String)));
        dtt.Columns.Add(new System.Data.DataColumn("IGST_AMT", typeof(String)));
        dtt.Columns.Add(new System.Data.DataColumn("ROWNUM", typeof(String)));

        dtt.Columns.Add(new System.Data.DataColumn("ORDERBY", typeof(String)));
        dtt.Columns.Add(new System.Data.DataColumn("CHARGE_DESC", typeof(String)));

        int R = 1;
        foreach (GridViewRow row in gv_Chg_Details.Rows)
        {
            CheckBox INV_CHECK = (CheckBox)row.FindControl("chk_inv");
            TextBox txtch_name = (TextBox)row.FindControl("txtch_name");
            TextBox txt_charge_desc = (TextBox)row.FindControl("txt_charge_desc");

            DropDownList ddl_tax_nontax = (DropDownList)row.FindControl("ddl_tax_nontax");

            TextBox txt_HSN_Code = (TextBox)row.FindControl("txt_HSN_Code");
            TextBox txt_SA_Code = (TextBox)row.FindControl("txt_SA_Code");

            TextBox txtqty = (TextBox)row.FindControl("txtqty");
            DropDownList ddl_CUR = (DropDownList)row.FindControl("ddl_CUR");
            TextBox txt_Ex_Rate = (TextBox)row.FindControl("txt_Ex_Rate");
            TextBox txtunit_price = (TextBox)row.FindControl("txtunit_price");
            TextBox txtamt = (TextBox)row.FindControl("txtamt");
            TextBox txtCGST_RATE = (TextBox)row.FindControl("txtCGST_RATE");
            TextBox txtCGST_AMT = (TextBox)row.FindControl("txtCGST_AMT");
            TextBox txtSGST_RATE = (TextBox)row.FindControl("txtSGST_RATE");
            TextBox txtSGST_AMT = (TextBox)row.FindControl("txtSGST_AMT");
            TextBox txtIGST_RATE = (TextBox)row.FindControl("txtIGST_RATE");
            TextBox txtIGST_AMT = (TextBox)row.FindControl("txtIGST_AMT");
            TextBox txtOrder_by = (TextBox)row.FindControl("txtOrder_by");

            if (txtch_name.Text == string.Empty && txtqty.Text == string.Empty && txtunit_price.Text == string.Empty && txtamt.Text == string.Empty)
            {

            }
            else
            {
                dr = dtt.NewRow();
                dr[0] = INV_CHECK.Checked == true ? "True" : "False";
                dr[1] = txtch_name.Text;
                dr[2] = ddl_tax_nontax.SelectedValue.ToString();

                dr[3] = txt_HSN_Code.Text;
                dr[4] = txt_SA_Code.Text;
                dr[5] = txtqty.Text;
                dr[6] = ddl_CUR.SelectedValue.ToString();
                dr[7] = txt_Ex_Rate.Text;

                dr[8] = txtunit_price.Text;
                dr[9] = txtamt.Text;
                dr[10] = txtCGST_RATE.Text;
                dr[11] = txtCGST_AMT.Text;
                dr[12] = txtSGST_RATE.Text;
                dr[13] = txtSGST_AMT.Text;
                dr[14] = txtIGST_RATE.Text;
                dr[15] = txtIGST_AMT.Text;
                dr[16] = R;
                dr[17] = txtOrder_by.Text;
                dr[18] = txt_charge_desc.Text;
                dtt.Rows.Add(dr);
                R = R + 1;
            }
        }
        DataSet dsData = new DataSet();
        dsData.Tables.Add(dtt);
        String xmlData = ConvertDataTableToXML(dsData.Tables[0]);
        return xmlData;
    }


    public DataSet Billing_Invoice_Jobno_Insert_Update(string S1, String xmlData_A, String xmlData_C)
    {
        ObjUBO.A1 = HDupdate_id.Value.ToString();
        ObjUBO.A2 = Rd_Imp_Exp.SelectedValue.ToString();
        ObjUBO.A3 = Rd_Bill_Type.SelectedValue.ToString();
        ObjUBO.A4 = ddlCus_name.SelectedValue.ToString();

        ObjUBO.A5 = hd_Brslno.Value;
        if (ddlbranch_No.SelectedItem != null)
        {
            ObjUBO.A23 = ddlbranch_No.SelectedItem.ToString();
        }
        ObjUBO.A6 = From_Quotation.Checked.ToString();
        ObjUBO.A7 = txtDebitNo.Text;
        ObjUBO.A8 = txtInvoiceNo.Text;
        ObjUBO.A9 = txtDate.Text;
        ObjUBO.A10 = txt_Cus_name.Text;
        ObjUBO.A11 = ch_Behalf.Checked == true ? "Y" : "N";
        ObjUBO.A12 = ddl_state_name.SelectedValue.ToString();
        ObjUBO.A13 = txt_GSTN_Id.Text;
        ObjUBO.A14 = txt_Party_Add.Text;

        ObjUBO.A15 = txtnon_tax_amt.Text;
        ObjUBO.A16 = txttax_amt.Text;
        ObjUBO.A17 = txt_tot_Rev_Amt.Text;
        ObjUBO.A18 = txt_total_tax_amt.Text;
        ObjUBO.A19 = txt_total_amt_With_tax.Text;
        ObjUBO.A20 = txt_grand_total.Text;
        ObjUBO.A21 = txt_Adv_amt.Text;
        ObjUBO.A22 = txt_Expense_Amt.Text;
        ObjUBO.A24 = Rd_Tax_NonTax.SelectedValue.ToString();
        ObjUBO.A25 = ddlbank_name.SelectedValue.ToString();

        ObjUBO.A26 = ddl_tr_mode.SelectedValue.ToString();
        ObjUBO.A27 = ddlshipment_type.Text.ToString();

        ObjUBO.A35 = xmlData_A;

        ObjUBO.A37 = xmlData_C;

        ObjUBO.A39 = S1;
        ObjUBO.A40 = txtAccHead.Text;
        ObjUBO.A41 = txtPONumber.Text;
        ObjUBO.A42 = hdnbranch.Value;
        ObjUBO.A43 = ddlType.SelectedValue.ToString();
        ObjUBO.A46 = txtNarration.Text;
        ObjUBO.A49 = ddlQuot.SelectedValue;
        ObjUBO.A50 = ddlContainerTypes.SelectedValue;
        ObjUBO.A44 = Chkdepartment.Checked == true ? lbldeptapp.Text : "N";
        ObjUBO.A45 = Chkacc.Checked == true ? lblaccapp.Text : "N";
        ObjUBO.A47 = ChkCancelInv.Checked == true ? lblcancel.Text : "Approved";
        ObjUBO.A48 = txtVerifiedBy.Text;
        ObjUBO.A51 = txtDueDate.Text;
        ObjUBO.A52 = txtInvoiceNo1.Text;
        ObjUBO.A53 = Rdbbillseztype.SelectedValue;
      
   ObjUBO.A54 = hdprefix.Value;
        ObjUBO.A55 = hdsuffix.Value;
        ObjUBO.A58 = ddlworking_Period.SelectedValue;   //ROSI
       
       
        return BP.DEBIT_INS_UPD(ObjUBO);
    }
    private void Clear()
    {
        txtInvoiceNo.Text = string.Empty;
        ch_Behalf.Checked = false;
        txt_Cus_name.Text = string.Empty;

        txt_Party_Add.Text = string.Empty;
        txtnon_tax_amt.Text = string.Empty;
        txttax_amt.Text = string.Empty;

        txt_total_tax_amt.Text = string.Empty;
        txt_total_amt_With_tax.Text = string.Empty;
        ddl_state_name.Text = string.Empty;
        txt_GSTN_Id.Text = string.Empty;
        txt_tot_Rev_Amt.Text = string.Empty;
        txt_Expense_Amt.Text = string.Empty;
        cbList.Items.Clear();

        ddl_tr_mode.SelectedIndex = -1;
        ddlshipment_type.Text = string.Empty;
    }
    protected void gv_Chg_Details_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            if (DataBinder.Eval(e.Row.DataItem, "TAX_NONTAX").ToString() == "T")
            {
                e.Row.BackColor = System.Drawing.ColorTranslator.FromHtml("#F0F0F0");
            }
            else
            {
                e.Row.BackColor = System.Drawing.ColorTranslator.FromHtml("#C0C0C0");
            }

            TextBox txtamt = (e.Row.FindControl("txtamt") as TextBox);
            TextBox txtCGST_AMT = (e.Row.FindControl("txtCGST_AMT") as TextBox);
            TextBox txtSGST_AMT = (e.Row.FindControl("txtSGST_AMT") as TextBox);
            TextBox txtIGST_AMT = (e.Row.FindControl("txtIGST_AMT") as TextBox);

            txtamt.Attributes.Add("readonly", "readonly");
            txtIGST_AMT.Attributes.Add("readonly", "readonly");
            txtCGST_AMT.Attributes.Add("readonly", "readonly");
            txtSGST_AMT.Attributes.Add("readonly", "readonly");
        }
    }
    private void SetInitialRow()
    {
        DataTable dt = new DataTable();
        DataRow dr = null;
        DataTable dtCurrentTable = (DataTable)ViewState["CurrentTable"];

        dt.Columns.Add(new DataColumn("INV_CHECK", typeof(string)));
        dt.Columns.Add(new DataColumn("RowNumber", typeof(string)));
        dt.Columns.Add(new DataColumn("ORDERBY", typeof(string)));
        dt.Columns.Add(new DataColumn("CHARGE_NAME", typeof(string)));
        dt.Columns.Add(new DataColumn("TAX_NONTAX", typeof(string)));

        dt.Columns.Add(new DataColumn("HSN_CODE", typeof(string)));
        dt.Columns.Add(new DataColumn("SA_CODE", typeof(string)));
        dt.Columns.Add(new DataColumn("Qty", typeof(string)));
        dt.Columns.Add(new DataColumn("CUR", typeof(string)));
        dt.Columns.Add(new DataColumn("EX_RATE", typeof(string)));

        dt.Columns.Add(new DataColumn("UNIT_RATE", typeof(string)));
        dt.Columns.Add(new DataColumn("AMOUNT", typeof(string)));
        dt.Columns.Add(new DataColumn("CGST_RATE", typeof(string)));
        dt.Columns.Add(new DataColumn("CGST_AMT", typeof(string)));
        dt.Columns.Add(new DataColumn("SGST_RATE", typeof(string)));
        dt.Columns.Add(new DataColumn("SGST_AMT", typeof(string)));
        dt.Columns.Add(new DataColumn("IGST_RATE", typeof(string)));
        dt.Columns.Add(new DataColumn("IGST_AMT", typeof(string)));
        dt.Columns.Add(new DataColumn("CHARGE_DESC", typeof(string)));

        dr = dt.NewRow();
        dr["INV_CHECK"] = "True";
        dr["RowNumber"] = 1;
        dr["ORDERBY"] = string.Empty;
        dr["CHARGE_NAME"] = string.Empty;
        dr["TAX_NONTAX"] = "N";

        dr["HSN_CODE"] = string.Empty;
        dr["SA_CODE"] = string.Empty;
        dr["Qty"] = "1";
        dr["CUR"] = "INR";
        dr["EX_RATE"] = "1";

        dr["UNIT_RATE"] = "0";
        dr["AMOUNT"] = "0";
        dr["CGST_RATE"] = "0";
        dr["CGST_AMT"] = "0";

        dr["SGST_RATE"] = "0";
        dr["SGST_AMT"] = "0";
        dr["IGST_RATE"] = "0";
        dr["IGST_AMT"] = "0";
        dr["CHARGE_DESC"] = string.Empty;

        dt.Rows.Add(dr);

        //Store the DataTable in ViewState
        ViewState["CurrentTable"] = dt;

        gv_Chg_Details.DataSource = dt;
        gv_Chg_Details.DataBind();

        txttax_amt.Text = "0.00";
        txtnon_tax_amt.Text = "0.00";
        txt_tot_Rev_Amt.Text = "0.00";

        txt_total_tax_amt.Text = "0.00";
        txt_total_amt_With_tax.Text = "0.00";
    }
    protected void gv_Chg_Details_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        // SetRowData();
        gst_toal = 0;
        if (ViewState["CurrentTable"] != null)
        {
            DataTable dt = (DataTable)ViewState["CurrentTable"];

            int rowIndex = Convert.ToInt32(e.RowIndex);
            if (dt.Rows.Count > 1)
            {
                dt.Rows.Remove(dt.Rows[rowIndex]);
                ViewState["CurrentTable"] = dt;
                //---------------------------------------------------------------------------------------
                DataTable dtnew = new DataTable();
                DataRow drnew = null;

                dtnew.Columns.Add(new DataColumn("INV_CHECK", typeof(string)));
                dtnew.Columns.Add(new DataColumn("RowNumber", typeof(string)));
                dtnew.Columns.Add(new DataColumn("ORDERBY", typeof(string)));

                dtnew.Columns.Add(new DataColumn("CHARGE_NAME", typeof(string)));
                dtnew.Columns.Add(new DataColumn("TAX_NONTAX", typeof(string)));
                dtnew.Columns.Add(new DataColumn("HSN_CODE", typeof(string)));
                dtnew.Columns.Add(new DataColumn("SA_CODE", typeof(string)));
                dtnew.Columns.Add(new DataColumn("Qty", typeof(string)));
                dtnew.Columns.Add(new DataColumn("CUR", typeof(string)));
                dtnew.Columns.Add(new DataColumn("EX_RATE", typeof(string)));

                dtnew.Columns.Add(new DataColumn("UNIT_RATE", typeof(string)));
                dtnew.Columns.Add(new DataColumn("AMOUNT", typeof(string)));
                dtnew.Columns.Add(new DataColumn("CGST_RATE", typeof(string)));
                dtnew.Columns.Add(new DataColumn("CGST_AMT", typeof(string)));
                dtnew.Columns.Add(new DataColumn("SGST_RATE", typeof(string)));
                dtnew.Columns.Add(new DataColumn("SGST_AMT", typeof(string)));
                dtnew.Columns.Add(new DataColumn("IGST_RATE", typeof(string)));
                dtnew.Columns.Add(new DataColumn("IGST_AMT", typeof(string)));
                dtnew.Columns.Add(new DataColumn("CHARGE_DESC", typeof(string)));

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    drnew = dtnew.NewRow();

                    drnew["INV_CHECK"] = dt.Rows[i][0].ToString();
                    drnew["RowNumber"] = i + 1;
                    drnew["ORDERBY"] = dt.Rows[i][2].ToString();

                    drnew["CHARGE_NAME"] = dt.Rows[i][3].ToString();
                    drnew["TAX_NONTAX"] = dt.Rows[i][4].ToString();

                    drnew["HSN_CODE"] = dt.Rows[i][5].ToString();
                    drnew["SA_CODE"] = dt.Rows[i][6].ToString();
                    drnew["Qty"] = dt.Rows[i][7].ToString();
                    drnew["CUR"] = dt.Rows[i][8].ToString();
                    drnew["EX_RATE"] = dt.Rows[i][9].ToString();

                    drnew["UNIT_RATE"] = dt.Rows[i][10].ToString();
                    drnew["AMOUNT"] = dt.Rows[i][11].ToString();
                    drnew["CGST_RATE"] = dt.Rows[i][12].ToString();
                    drnew["CGST_AMT"] = dt.Rows[i][13].ToString();
                    drnew["SGST_RATE"] = dt.Rows[i][14].ToString();
                    drnew["SGST_AMT"] = dt.Rows[i][15].ToString();
                    drnew["IGST_RATE"] = dt.Rows[i][16].ToString();
                    drnew["IGST_AMT"] = dt.Rows[i][17].ToString();
                    drnew["CHARGE_DESC"] = dt.Rows[i][18].ToString();

                    dtnew.Rows.Add(drnew);


                    if (dt.Rows[i][0].ToString() == "True" && dt.Rows[i][4].ToString() == "T")
                    {
                        if (Rd_Bill_Type.SelectedValue.ToString() == "L")
                        {
                            gst_toal = gst_toal + (Convert.ToDecimal(dt.Rows[i][13].ToString() == "" ? "0.00" : dt.Rows[i][13].ToString())
                                              + Convert.ToDecimal(dt.Rows[i][15].ToString() == "" ? "0.00" : dt.Rows[i][15].ToString()));
                        }
                        else
                        {
                            gst_toal = gst_toal + Convert.ToDecimal(dt.Rows[i][17].ToString() == "" ? "0.00" : dt.Rows[i][17].ToString());
                        }

                        tax_amt = tax_amt + Convert.ToDecimal(dt.Rows[i][11].ToString() == "" ? "0.00" : dt.Rows[i][11].ToString());
                    }
                    else if (dt.Rows[i][0].ToString() == "True" && dt.Rows[i][4].ToString() == "N")
                    {
                        non_tax_amt = non_tax_amt + Convert.ToDecimal(dt.Rows[i][11].ToString() == "" ? "0.00" : dt.Rows[i][11].ToString());
                    }
                    else if (dt.Rows[i][0].ToString() == "True" && dt.Rows[i][4].ToString() == "E")
                    {
                        non_tax_amt = non_tax_amt + Convert.ToDecimal(dt.Rows[i][11].ToString() == "" ? "0.00" : dt.Rows[i][11].ToString());
                    }
                    else if (dt.Rows[i][0].ToString() == "True" && dt.Rows[i][4].ToString() == "P")
                    {
                        gst_toal = gst_toal + (Convert.ToDecimal(dt.Rows[i][13].ToString() == "" ? "0.00" : dt.Rows[i][13].ToString())
                                          + Convert.ToDecimal(dt.Rows[i][15].ToString() == "" ? "0.00" : dt.Rows[i][15].ToString())
                                          + Convert.ToDecimal(dt.Rows[i][17].ToString() == "" ? "0.00" : dt.Rows[i][17].ToString())
                                          );

                        tax_amt = tax_amt + Convert.ToDecimal(dt.Rows[i][11].ToString() == "" ? "0.00" : dt.Rows[i][11].ToString());
                    }
                    else if (dt.Rows[i][0].ToString() == "True" && dt.Rows[i][4].ToString() == "R")
                    {
                        if (Rd_Bill_Type.SelectedValue.ToString() == "L")
                        {
                            Reverse_total = Reverse_total + (
                            (Convert.ToDecimal(dt.Rows[i][11].ToString() == "" ? "0.00" : dt.Rows[i][11].ToString()) * Convert.ToDecimal(dt.Rows[i][12].ToString() == "" ? "0.00" : dt.Rows[i][12].ToString()) / 100)
                            + (Convert.ToDecimal(dt.Rows[i][11].ToString() == "" ? "0.00" : dt.Rows[i][11].ToString()) * Convert.ToDecimal(dt.Rows[i][14].ToString() == "" ? "0.00" : dt.Rows[i][14].ToString()) / 100)
                            );
                        }
                        else
                        {
                            Reverse_total = Reverse_total + (
                            (Convert.ToDecimal(dt.Rows[i][11].ToString() == "" ? "0.00" : dt.Rows[i][11].ToString()) * Convert.ToDecimal(dt.Rows[i][16].ToString() == "" ? "0.00" : dt.Rows[i][16].ToString()) / 100)
                            );
                        }

                        tax_amt = tax_amt + Convert.ToDecimal(dt.Rows[i][11].ToString() == "" ? "0.00" : dt.Rows[i][11].ToString());

                        //-----------------------------------------------------
                    }
                }

                //---------------------------------------------------------------------------------------
                gv_Chg_Details.DataSource = dtnew;
                gv_Chg_Details.DataBind();

                txtnon_tax_amt.Text = non_tax_amt.ToString();
                txttax_amt.Text = tax_amt.ToString();
                txt_total_tax_amt.Text = gst_toal.ToString();
                txt_tot_Rev_Amt.Text = Reverse_total.ToString();

                txt_total_amt_With_tax.Text = (Convert.ToDecimal(txt_total_tax_amt.Text) + Convert.ToDecimal(txttax_amt.Text)).ToString("#0.00");
            }
            else if (dt.Rows.Count == 1)
            {
                DataTable dtt = new DataTable();
                DataRow drr = null;

                dtt.Columns.Add(new DataColumn("INV_CHECK", typeof(string)));
                dtt.Columns.Add(new DataColumn("RowNumber", typeof(string)));
                dtt.Columns.Add(new DataColumn("ORDERBY", typeof(string)));
                dtt.Columns.Add(new DataColumn("CHARGE_NAME", typeof(string)));
                dtt.Columns.Add(new DataColumn("TAX_NONTAX", typeof(string)));

                dtt.Columns.Add(new DataColumn("HSN_CODE", typeof(string)));
                dtt.Columns.Add(new DataColumn("SA_CODE", typeof(string)));
                dtt.Columns.Add(new DataColumn("Qty", typeof(string)));
                dtt.Columns.Add(new DataColumn("CUR", typeof(string)));
                dtt.Columns.Add(new DataColumn("EX_RATE", typeof(string)));

                dtt.Columns.Add(new DataColumn("UNIT_RATE", typeof(string)));
                dtt.Columns.Add(new DataColumn("AMOUNT", typeof(string)));
                dtt.Columns.Add(new DataColumn("CGST_RATE", typeof(string)));
                dtt.Columns.Add(new DataColumn("CGST_AMT", typeof(string)));
                dtt.Columns.Add(new DataColumn("SGST_RATE", typeof(string)));
                dtt.Columns.Add(new DataColumn("SGST_AMT", typeof(string)));
                dtt.Columns.Add(new DataColumn("IGST_RATE", typeof(string)));
                dtt.Columns.Add(new DataColumn("IGST_AMT", typeof(string)));
                dtt.Columns.Add(new DataColumn("CHARGE_DESC", typeof(string)));

                drr = dtt.NewRow();
                drr["INV_CHECK"] = "True";
                drr["RowNumber"] = 1;
                drr["ORDERBY"] = string.Empty;
                drr["CHARGE_NAME"] = string.Empty;
                drr["TAX_NONTAX"] = "N";

                drr["HSN_CODE"] = string.Empty;
                drr["SA_CODE"] = string.Empty;
                drr["Qty"] = "1";
                drr["CUR"] = "INR";
                drr["EX_RATE"] = "1";

                drr["UNIT_RATE"] = "0";
                drr["AMOUNT"] = "0";
                drr["CGST_RATE"] = "0";
                drr["CGST_AMT"] = "0";
                drr["SGST_RATE"] = "0";
                drr["SGST_AMT"] = "0";
                drr["IGST_RATE"] = "0";
                drr["IGST_AMT"] = "0";
                drr["CHARGE_DESC"] = string.Empty;

                dtt.Rows.Add(drr);

                ViewState["CurrentTable"] = dtt;

                gv_Chg_Details.DataSource = dtt;
                gv_Chg_Details.DataBind();

                txtnon_tax_amt.Text = "0";
                txttax_amt.Text = "0";
                txt_total_tax_amt.Text = "0";
                txt_tot_Rev_Amt.Text = "0";
                txt_total_amt_With_tax.Text = "0";

            }
        }
        ScriptManager.RegisterStartupScript(this, typeof(Page), "OnClientClicking", "B_G_tab_page2();", true);
        txtfocus.Focus();
    }
    protected void ddlType_SelectedIndexChanged(object sender, EventArgs e)
    {
        Auto_Debitno();

    }
    private void Auto_Debitno()
    {
       
      

        if (ddlType.SelectedValue.ToString() == "CLEARING")
        {
            BP.MODULETYPE = "CLEARING";
            BP.CATEGORYTYPE = "CLEARING_DEBIT_NO";
        }
        else if (ddlType.SelectedValue.ToString() == "FORWARDING")
        {
            BP.MODULETYPE = "FORWARDING";
            BP.CATEGORYTYPE = "FORWARDING_DEBIT_NO";

        }
        else if (ddlType.SelectedValue.ToString() == "OTHERS")
        {
            BP.MODULETYPE = "OTHERS";
            BP.CATEGORYTYPE = "OTHERS_DEBIT_NO";

        }
        else if (ddlType.SelectedValue.ToString() == "CROSS_COUNTRY")
        {
            BP.MODULETYPE = "CROSS_COUNTRY";
            BP.CATEGORYTYPE = "CROSS_DEBIT_NO";
            //types = "CROSS_COUNTRY_IMP_AIR";

        }
        else if (ddlType.SelectedValue.ToString() == "VAS_Misc")
        {
            BP.MODULETYPE = "VAS_Misc";
            BP.CATEGORYTYPE = "VAS_DEBIT_NO";

        }
        else if (ddlType.SelectedValue.ToString() == "BOTH")
        {
            BP.MODULETYPE = "BOTH";
            BP.CATEGORYTYPE = "BOTH_DEBIT_NO";

        }

        string CATEGORYTYPE = BP.CATEGORYTYPE;
        
        //txtJobNo.Text = Convert.ToString(Transact.AIR_RetrieveID());
        txtDebitNo.Text = txtDebitNo.Text = Convert.ToString(BP.Debit_RetrieveID());
         JobNo_Prefix_Sufix(CATEGORYTYPE);
      //  Session["IMP_AIR_Jobno_Excel"] = txtDebitNo.Text;
    }
    public void JobNo_Prefix_Sufix(string CATEGORYTYPE)
    {
        Admin_job_Prefix_Suffix JPS = new Admin_job_Prefix_Suffix();
        DataSet dats = new DataSet();
        JPS.Branch = Connection.Current_Branch();
        JPS.Working_period = Connection.WorkingPeriod();
       // JPS.Tr_mode = ddlTransportMode.SelectedItem.Text;
        JPS.Ename = CATEGORYTYPE;
        dats = JPS.Job_prefix_Suffix_Select();

        int Jno = 0;
        string JobNo = string.Empty;
        JobNo = txtDebitNo.Text;

        if (JobNo != string.Empty)
        {
            Jno = Convert.ToInt32(JobNo);
            if (Jno < 1000)
            {
                if (Jno > 100)
                {
                    JobNo = "0" + txtDebitNo.Text;
                }
                else if (Jno > 10)
                {
                    JobNo = "00" + txtDebitNo.Text;
                }
                else
                {
                    JobNo = "000" + txtDebitNo.Text;
                }
            }
        }
        if (Session["COMPANY_LICENSE"] != null)
        {

            if (dats.Tables[0].Rows.Count > 0)
            {
                if (dats.Tables[0].Rows[0][0].ToString() == string.Empty && dats.Tables[0].Rows[0][1].ToString() != string.Empty)
                {
                    txtDebitNo.Text = txtDebitNo.Text + "/" + ((dats.Tables[0].Rows[0][1].ToString()).TrimStart()).TrimEnd();
                    hdprefix.Value = string.Empty;
                    hdsuffix.Value = ((dats.Tables[0].Rows[0][1].ToString()).TrimStart()).TrimEnd();
                }
                else if (dats.Tables[0].Rows[0][0].ToString() != string.Empty && dats.Tables[0].Rows[0][1].ToString() == string.Empty)
                {
                    // txtjobps.Text = ((dats.Tables[0].Rows[0][0].ToString() + "/" + txtJobNo.Text).TrimEnd()).TrimStart();
                    // if (Connection.COMPANYID() == "1163" || Connection.COMPANYID() == "1164" || Connection.COMPANYID() == "1165" || Connection.COMPANYID() == "1166")
                    //{
                    // txtjobps.Text = ((dats.Tables[0].Rows[0][0].ToString() + "/" + JobNo).TrimEnd()).TrimStart();
                    // }
                    //else
                    //{
                    txtDebitNo.Text = ((dats.Tables[0].Rows[0][0].ToString() + "/" + txtDebitNo.Text).TrimEnd()).TrimStart();
                    //}

                    hdprefix.Value = ((dats.Tables[0].Rows[0][0].ToString()).TrimEnd()).TrimStart();
                    hdsuffix.Value = string.Empty;
                }
                else if (dats.Tables[0].Rows[0][0].ToString() != string.Empty && dats.Tables[0].Rows[0][1].ToString() != string.Empty)
                {
                    //txtjobps.Text = ((dats.Tables[0].Rows[0][0].ToString() + "/" + txtJobNo.Text + "/" + dats.Tables[0].Rows[0][1].ToString()).TrimEnd()).TrimStart();

                    //if (Connection.COMPANYID() == "1163" || Connection.COMPANYID() == "1164" || Connection.COMPANYID() == "1165" || Connection.COMPANYID() == "1166")
                    //{
                    //txtjobps.Text = ((dats.Tables[0].Rows[0][0].ToString() + "/" + JobNo + "/" + dats.Tables[0].Rows[0][1].ToString()).TrimEnd()).TrimStart();
                    // }
                    // else
                    //{
                    txtDebitNo.Text = ((dats.Tables[0].Rows[0][0].ToString() + "/" + txtDebitNo.Text + "/" + dats.Tables[0].Rows[0][1].ToString()).TrimEnd()).TrimStart();
                    //}
                    hdprefix.Value = ((dats.Tables[0].Rows[0][0].ToString()).TrimEnd()).TrimStart();
                    hdsuffix.Value = ((dats.Tables[0].Rows[0][1].ToString()).TrimEnd()).TrimStart();
                    txtDebitNo.Text = txtDebitNo.Text;
                    txtInvoiceNo1.Text = txtDebitNo.Text;
                }
                else if (dats.Tables[0].Rows[0][0].ToString() == string.Empty && dats.Tables[0].Rows[0][1].ToString() == string.Empty)
                {
                    txtDebitNo.Text = txtDebitNo.Text;
                    txtInvoiceNo1.Text = txtDebitNo.Text;
                    hdprefix.Value = string.Empty;
                    hdsuffix.Value = string.Empty;
                }
            }
            else
            {
                txtDebitNo.Text = txtDebitNo.Text;
                txtInvoiceNo1.Text = txtDebitNo.Text;
                hdprefix.Value = string.Empty;
                hdsuffix.Value = string.Empty;
            }

        }    }
    protected void btnNew_Click1(object sender, EventArgs e)
    {
        Transaction trans = new Transaction();
        trans.ResetFields(Page.Controls);
        btnUpdate.CssClass = "updates";

        ViewState["UPDATED_ID"] = null;
        btnSave.Visible = true;
        btnDelete.Visible = false;
        btnUpdate.Visible = false;

        DataTable dt = new DataTable();
        gv_Chg_Details.DataSource = dt;
        gv_Chg_Details.DataBind();

        SetInitialRow();

        Clear();
        HDupdate_id.Value = string.Empty;


        btnrpt.Visible = false;



        Rd_Bill_Type.Enabled = true;
        Rd_Imp_Exp.Enabled = true;

        // txtDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
        Auto_Debitno();
        gv_Gen_Item_I.DataSource = dt;
        gv_Gen_Item_I.DataBind();



    }

    [System.Web.Services.WebMethod]
    public static string Get_Importer(string custid, string Jobno, string Branch)
    {
        Auto_Search Auto = new Auto_Search();
        DataSet ds = new DataSet();

        Auto.Name_Search = custid;
        Auto.Flag = "IMP_Billing_IMPORTER_NAME_SEARCH";
        Auto.Branch = Branch;

        ds = Auto.Export_Billing_Mastersearch();

        if (ds.Tables[0].Rows.Count > 0)
        {
            string Address1 = ds.Tables[0].Rows[0]["IMPORTER_ADDRESS"].ToString();
            string Address2 = ds.Tables[0].Rows[0]["GSTN_ID"].ToString();

            string State = ds.Tables[0].Rows[0]["M_COMMERCIAL_TAX_STATENAME"].ToString();
            custid = Address1 + "~~" + Address2 + "~~" + State;
        }
        else
        {
            custid = "";
        }

        return custid;
    }


    [System.Web.Services.WebMethod]
    public static string Get_Supplier(string custid, string Jobno)
    {
        Auto_Search Auto = new Auto_Search();
        DataSet ds = new DataSet();

        Auto.Name_Search = custid;
        Auto.Flag = "IMP_SUPPLIER_NAME_SEARCH";
        Auto.Branch = Connection.Get_Company_Type();

        ds = Auto.Export_Billing_Mastersearch();
        if (ds.Tables[0].Rows.Count > 0)
        {
            string Address = ds.Tables[0].Rows[0]["SUPPLIER_ADDRESS"].ToString();
            custid = Address;
        }
        else
        {
            custid = "";
        }
        return custid;
    }
    protected void GV_RowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header)
            e.Row.CssClass = "header";
    }
    [System.Web.Services.WebMethod]
    public static string GetImporter(string custid, string Branch)
    {
        Auto_Search Auto = new Auto_Search();
        DataSet ds = new DataSet();

        Auto.Name_Search = custid;
        //Auto.Jobno = Jobno;
        Auto.Flag = "IMP_BILLING_IMPORTER_NAME_SEARCH";
        Auto.Branch = Branch;

        ds = Auto.Export_Billing_Mastersearch();

        if (ds.Tables[0].Rows.Count > 0)
        {
            string Address = ds.Tables[0].Rows[0]["IMPORTER_ADDRESS"].ToString();
            custid = Address;
        }
        else
        {
            custid = "";
        }

        return custid;
    }
    [System.Web.Services.WebMethod]
    public static string Get_Cha_Type(string ChargeName, string Imp_Name, string Mode, string branch)
    {
        string Ch_Name = "";
        DataSet ds = new DataSet();
        try
        {
            GST_Imp_Invoice BI = new GST_Imp_Invoice();
            Billing_UserBO ObjUBO = new Billing_UserBO();


            ObjUBO.Imp_Name = ChargeName;
            ObjUBO.Flag = "GST_Imp_EXP_Charge_type";
            ObjUBO.CustBranch = branch;
            ds = BI.Select_IMP_INV(ObjUBO);

            if (ds.Tables[0].Rows.Count > 0)
            {
                Ch_Name = ds.Tables[0].Rows[0]["TAX_MODE"].ToString() + "~~" + ds.Tables[0].Rows[0]["HSN_CODE"].ToString() + "~~" + ds.Tables[0].Rows[0]["SA_CODE"].ToString() + "~~" + ds.Tables[0].Rows[0]["CGST_RATE"].ToString() + "~~" + ds.Tables[0].Rows[0]["SGST_RATE"].ToString() + "~~" + ds.Tables[0].Rows[0]["IGST_RATE"].ToString();
            }
            else
            {
                Ch_Name = "";
            }
            //------------------------------------
        }
        catch (Exception ex)
        {
            Connection.Error_Msg(ex.Message);
        }
        return Ch_Name;
    }
    [System.Web.Services.WebMethod]
    public static string Get_Cha_exrate(string currency, string IE, string dat, string IO)
    {
        string exrate = "";
        DataSet ds = new DataSet();
        try
        {
            eroyalmaster BI = new eroyalmaster();
            Billing_UserBO ObjUBO = new Billing_UserBO();

            if (IE == "I")
            {
                BI.Eexchange_nont_no = currency;
                BI.Ename = "I_EXG_RATE";
                BI.Eexchange_effect_dt_to = dat;
            }
            else if (IE == "E")
            {
                BI.Eexchange_nont_no = currency;
                BI.Ename = "E_EXG_RATE";
                BI.Eexchange_effect_dt_to = dat;


            }

            ds = BI.RetrieveAll_Exchange_master();
            if (BI.result == 1)
            {

                if (ds.Tables[0].Rows.Count > 0)
                {
                    if (IO != "IO")
                    {
                        exrate = ds.Tables[0].Rows[0]["EX_RATE"].ToString();
                    }
                    else if (IO == "IO") { exrate = "1"; }
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
    protected void Rd_Bill_Type_SelectedIndexChanged(object sender, EventArgs e)
    {
        //if (Rd_Bill_Type.SelectedValue != "IO")
        //{
        //    Get_Cha_exrate(Rd_Bill_Type.SelectedValue, Rd_Imp_Exp.SelectedValue, txtDate.Text);
        //}
    }


    private void Checked_Jobs()
    {
        string values = "", Val = string.Empty;
        foreach (ListItem objItem in cbList.Items)
        {
            values += objItem.ToString() + ",";
        }
        hd_Selected_Jobs.Value = values;
    }

    private void Billing_Others()
    {
        string Val = string.Empty;
        foreach (ListItem objItem in cbList.Items)
        {
            Val += objItem.Value.ToString() + ",";
        }
        hd_Jobs.Value = Val;
        hd_Imp_Exp.Value = Rd_Imp_Exp.SelectedValue.ToString();
    }
    protected void GV_Gen_head_RowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header)
            e.Row.CssClass = "header";
        if (e.Row.RowType == DataControlRowType.DataRow && e.Row.RowState == DataControlRowState.Normal)
            e.Row.CssClass = "normal";
        if (e.Row.RowType == DataControlRowType.DataRow && e.Row.RowState == DataControlRowState.Alternate)
            e.Row.CssClass = "alternate";
    }
    protected void btnloadData_Click(object sender, EventArgs e)
    {
        try
        {
            DataTable dt = new DataTable();
            string values = "";

            DataSet dss = new DataSet();


            ObjUBO.A4 = txtInvoiceNo.Text;
            ObjUBO.A5 = ddl_tr_mode.SelectedValue.ToString();

            ObjUBO.A7 = Rd_Imp_Exp.SelectedValue.ToString();

            ObjUBO.A8 = "SELECT_INVOICE_DATA";
            ObjUBO.A10 = ddlType.SelectedValue.ToString();

            //ObjUBO.A9 = ViewState["UPDATED_ID"] == null ? "" : ViewState["UPDATED_ID"].ToString();
            ObjUBO.A9 = "";
            if (Session["COMPANY_DETAILS_DS"] != null)
            {
                ds = (DataSet)Session["COMPANY_DETAILS_DS"];

                DataTable ds3 = new DataTable();
                ds3 = ds.Tables[2];
                DataView view1 = ds3.DefaultView;
                string a = ddlworking_Period.SelectedValue;
                view1.RowFilter = "WORKING_PERIOD = '" + a + "'";
                DataTable table1 = view1.ToTable();
                dss = BP.Select_Inv_Debit_DB(ObjUBO, table1.Rows[0]["SERVER_NAME"].ToString());
            }
            //dss = BP.Select_Inv_Debit(ObjUBO);
            if (dss.Tables[0].Rows.Count > 0)
            {
                txtInvoiceNo.Text = dss.Tables[0].Rows[0]["ALT_INV_NO"].ToString();
               // txtInvoiceNo1.Text = dss.Tables[0].Rows[0]["ALT_INV_NO"].ToString();
                txtInvoiceNo1.Text = txtDebitNo.Text;
                txtDate.Text = dss.Tables[0].Rows[0]["DEBIT_DATE"].ToString();
                txt_Cus_name.Text = dss.Tables[0].Rows[0]["PARTY_NAME"].ToString();

                if (dss.Tables[0].Rows[0]["AC_BEHALF"].ToString() == "Y")
                {
                    ch_Behalf.Checked = true;
                }

                ddl_state_name.SelectedValue = dss.Tables[0].Rows[0]["STATE_NAME"].ToString();
                txt_GSTN_Id.Text = dss.Tables[0].Rows[0]["GSTN_ID"].ToString();
                txt_Party_Add.Text = dss.Tables[0].Rows[0]["PARTY_ADDRESS"].ToString();
                Rd_Tax_NonTax.SelectedValue = dss.Tables[0].Rows[0]["TAX_NON_TAX"].ToString();
                Rd_Bill_Type.SelectedValue = dss.Tables[0].Rows[0]["LOCAL_OTHER"].ToString();
                ddlbank_name.SelectedValue = dss.Tables[0].Rows[0]["BANK_DETAILS"].ToString();

                ddl_tr_mode.SelectedValue = dss.Tables[0].Rows[0]["MODE"].ToString();

                ddlshipment_type.Text = dss.Tables[0].Rows[0]["SHIP_TYPE"].ToString();

                //if (dss.Tables[0].Rows[0]["QUOT"].ToString() != "" && dss.Tables[0].Rows[0]["QUOT"].ToString() != null)
                //{
                //    ddlQuot.Enabled = true;
                //    From_Quotation.Checked = true;
                //    ddlQuot.SelectedValue = dss.Tables[0].Rows[0]["QUOT"].ToString();
                //    string a = dss.Tables[0].Rows[0]["QUOT"].ToString();
                //}
                txtAccHead.Text = dss.Tables[0].Rows[0]["ACC_HEAD"].ToString();
                // ddlContainerTypes.SelectedValue = dss.Tables[0].Rows[0]["CONTAINER_TYPE"].ToString();
                txtPONumber.Text = dss.Tables[0].Rows[0]["PO_NO"].ToString();
                txtVerifiedBy.Text = dss.Tables[0].Rows[0]["VERIFIED_BY"].ToString();
                txtDueDate.Text = dss.Tables[0].Rows[0]["DUE_DATE"].ToString();

                //if (dss.Tables[0].Rows[0]["DEPT_APPROV"].ToString() != "N")
                //{
                //    Chkdepartment.Checked = true;
                //    if (dss.Tables[0].Rows[0]["DEPT_APPROV"].ToString() != "Y")
                //    {
                //        lbldeptapp.Text = dss.Tables[0].Rows[0]["DEPT_APPROV"].ToString();
                //    }
                //}
                //else { Chkdepartment.Checked = false; }
                //if (dss.Tables[0].Rows[0]["ACC_APPROV"].ToString() != "N")
                //{
                //    Chkacc.Checked = true;
                //    if (dss.Tables[0].Rows[0]["ACC_APPROV"].ToString() != "Y")
                //    {
                //        lblaccapp.Text = dss.Tables[0].Rows[0]["ACC_APPROV"].ToString();
                //    }
                //}
                //else { Chkacc.Checked = false; }
                //if (dss.Tables[0].Rows[0]["JOB_STATUS"].ToString() != "Approved" && dss.Tables[0].Rows[0]["JOB_STATUS"].ToString() != "")
                //{
                //    ChkCancelInv.Checked = true;
                //    if (dss.Tables[0].Rows[0]["JOB_STATUS"].ToString() != "Canceled")
                //    {
                //        lblcancel.Text = dss.Tables[0].Rows[0]["JOB_STATUS"].ToString();
                //    }
                //}
                //else { ChkCancelInv.Checked = false; }

                txtNarration.Text = dss.Tables[0].Rows[0]["NARRATION"].ToString();
                //Rdbbillseztype.SelectedValue = dss.Tables[0].Rows[0]["BILL_TYPE"].ToString();

            }
            else
            {
                ddl_state_name.SelectedValue = string.Empty;
                txt_GSTN_Id.Text = string.Empty;
                txt_Cus_name.Text = string.Empty;
                txt_Party_Add.Text = string.Empty;
            }

            if (dss.Tables[1].Rows.Count > 0)
            {
                gv_Gen_Item_I.DataSource = dss.Tables[1];
                gv_Gen_Item_I.DataBind();
            }
            else
            {
                gv_Gen_Item_I.DataSource = dt;
                gv_Gen_Item_I.DataBind();
            }



            if (dss.Tables[2].Rows.Count > 0)
            {
                gv_Chg_Details.DataSource = dss.Tables[2];
                gv_Chg_Details.DataBind();

                ViewState["CurrentTable"] = dss.Tables[2];
            }
            else
            {
                SetInitialRow();
            }

        }
        catch
        {
            Alert_msg("Give Valid Charge");
            Page.ClientScript.RegisterStartupScript(this.GetType(), "Open", "javascript:inv('" + HDupdate_id.Value + "','" + HDupdate_IMP_EXP_id.Value + "','" + hdninvoice.Value + "','" + Hdnmode.Value + "','" + Hdntype.Value + "','" + hdnmisc.Value + "')", true);
        }
    }


    protected void gv_Gen_Item_I_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            TextBox txt_Jobno_I = (e.Row.FindControl("txt_Jobno_I") as TextBox);
            txt_Jobno_I.BackColor = System.Drawing.ColorTranslator.FromHtml("#E5E4E2");
            txt_Jobno_I.Enabled = false;
        }
    }
    protected void gv_Gen_Item_II_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            TextBox txt_Jobno_II = (e.Row.FindControl("txt_Jobno_II") as TextBox);
            txt_Jobno_II.BackColor = System.Drawing.ColorTranslator.FromHtml("#E5E4E2");
            txt_Jobno_II.Enabled = false;
        }
    }



    private void Load_Bank_Details()
    {
        ERM.Ename = "BANK_NAME";
        DataSet ds = new DataSet();
        ds = ERM.RetrieveAll_Bank_MASTER();
        ddlbank_name.DataSource = ERM.RetrieveAll_Bank_MASTER();
        ddlbank_name.DataTextField = "BANK_NAME";
        ddlbank_name.DataValueField = "BANK_NAME";
        ddlbank_name.DataBind();
        ddlbank_name.Items.Insert(0, "---Select Bank---");

    }
    protected void ddl_tr_mode_SelectedIndexChanged(object sender, EventArgs e)
    {
        //if (Connection.Company_License().ToLower() != "erf00018")
        //{
        //    shipmenttype1();
        //    GST_Quotation_Master QM = new GST_Quotation_Master();
        //    Billing_UserBO BO = new Billing_UserBO();
        //    DataSet DS = new DataSet();
        //    BO.Imp_Name = ddlCus_name.SelectedValue;
        //    BO.MODE = ddl_tr_mode.SelectedValue.ToString().Substring(0, 1) + Rd_Imp_Exp.SelectedValue;
        //    BO.SHIPMENT_TYPE = ddlContainerTypes.SelectedValue;
        //    BO.Flag = "QUOT_NO";
        //    DS = QM.Quotation_Master_Select(BO);
        //    ddlQuot.Items.Clear();
        //    //ddlQuot.Items.Insert(0,new ListItem(string.Empty,string.Empty));

        //    ddlQuot.DataSource = DS.Tables[0];
        //    ddlQuot.DataTextField = "R_ID";
        //    ddlQuot.DataValueField = "R_ID";
        //    ddlQuot.DataBind();
        //}

    }

    private void shipmenttype1()
    {
        if (ddl_tr_mode.SelectedValue == "Air")
        {
            ddlshipment_type.Visible = false;
            lblshiptype2.Visible = false;
        }
        if (ddl_tr_mode.SelectedValue == "Sea")
        {
            ddlshipment_type.Visible = true;
            lblshiptype2.Visible = true;
        }
    }
    //protected void ddlbank_name_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    ERM.BANK_ID = ddlbank_name.SelectedValue.ToString();
    //    ERM.Ename = "select_conwithcode";
    //    DataSet ds = new DataSet();
    //    ds = ERM.RetrieveAll_Bank_MASTER();
    //    txtBranch_Name.Text = ds.Tables[0].Rows[0]["BRANCH_NAME"].ToString();
    //    txtIFSC_Code.Text = ds.Tables[0].Rows[0]["IFSC_CODE"].ToString();
    //    txtAcc_No.Text = ds.Tables[0].Rows[0]["ACCOUNT_NUMBER"].ToString();



    //}
    protected void Chkdepartment_Changed(object sender, EventArgs e)
    {

        if (Chkdepartment.Checked == true) { lbldeptapp.Text = hdnuser.Value; }
        //Printaccess();
    }
    protected void chkFrom_Quot_Changed(object sender, EventArgs e)
    {
        if (From_Quotation.Checked == true) { ddlQuot.Enabled = true; }
        else { ddlQuot.Enabled = false; }
    }
    protected void Chkacc_Changed(object sender, EventArgs e)
    {
        if (Chkdepartment.Checked == true) { lblaccapp.Text = hdnuser.Value; txtVerifiedBy.Text = hdnuser.Value; }

        //Printaccess();
    }
    protected void ChkCancelInv_Changed(object sender, EventArgs e)
    {
        if (ChkCancelInv.Checked == true) { lblcancel.Text = hdnuser.Value; }

        Chkdepartment.Checked = false;
        Chkacc.Checked = false;
        if (ChkCancelInv.Checked == true)
        { Chkdepartment.Enabled = false; Chkacc.Enabled = false; }
        else { Chkdepartment.Enabled = true; Chkacc.Enabled = true; }
        Printaccess();
    }



}
