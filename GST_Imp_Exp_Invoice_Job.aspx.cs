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
using System.Net;
using System.Text.RegularExpressions;
using QRCoder;
using System.Drawing;
public partial class GST_Imp_Exp_Invoice_Job : ThemeClass
{
    public string currentuser;
    public string currentbranch;
    public string Working_Period;
    User_Creation user_Create = new User_Creation();
    AppSession aps = new AppSession();
    eroyalmaster ERM = new eroyalmaster();

    GST_Imp_Invoice BP = new GST_Imp_Invoice();
    Global_variables ObjUBO = new Global_variables();
    DataSet ds = new DataSet();
    DataTable dt = new DataTable();

    public int i, Screen_Id, Approval_ID;
    public string SCREEN_ID, PAGE_MODIFY, PAGE_DELETE;
    public string PAGE_WRITE, PAGE_READ, COMPANY_ID, Screen_IdNew, APPROVAL_CATEGORY;
    decimal tax_amt, non_tax_amt, gst_toal, Reverse_total;

    string Imp_name = string.Empty, J_no = string.Empty, Ch_type = string.Empty ;
    public bool chk = false;
    static string _invoiceJSONString = string.Empty, errorFolder = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        aps.checkSession();
        
        CalendarExtender1.EndDate = DateTime.Today;      
        if (!Page.IsPostBack)
        {
           
            Rd_Imp_Exp.Focus();
            Screen_Id = 97;
            Page_Rights(Screen_Id);
            Approval_ID = 1;
            Page_Approval(Approval_ID);
            hdnCompany.Value = Connection.Company_License().ToLower();
            currentuser = Session["currentuser"].ToString();
            currentbranch = Session["currentbranch"].ToString();
            Working_Period = Session["WorkingPeriod"].ToString();
            //----------THIRU ---START---INVOICE TO JOB LINK ---18-09-2024--------------------//
            Session["GROUP_ID"] = HttpContext.Current.Session["GroupID"].ToString();
            //----------THIRU ---START---INVOICE TO JOB LINK ---18-09-2024--------------------//
            hdnbranch.Value= currentbranch;
            hdnuser.Value = currentuser;
            DataTable dtt = new DataTable();
            GV.DataSource = dtt;
            GV.DataBind();
            GV1.DataSource = dtt;
            GV1.DataBind();
            //------------------THIRU-----------------WORKING--PERIOD---START---------------//
            Working_Period_Load();
            //------------------THIRU-----------------WORKING--PERIOD---END---------------//

            Jobno();
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
            //----------------SETTING SCREEN PERMISSION---------------//
            //Screen_Id = 11;
            //----------------SETTING SCREEN PERMISSION---------------//
          
            btnSave.Visible = true;
            btnUpdate.Visible = false;
            btnDelete.Visible = false;
            btnrpt.Visible = false;
            btnprorpt.Visible = false;
            txtCancelReason.Visible = false;
            T.Visible = false;
            //txt_Cus_name.Enabled = false;
            txt_tot_Rev_Amt.Attributes.Add("readonly", "readonly");
            txtnon_tax_amt.Attributes.Add("readonly", "readonly");
            txttax_amt.Attributes.Add("readonly", "readonly");
            txt_Expense_Amt.Attributes.Add("readonly", "readonly");
            tag_transac_lft_Inv_in1.Visible = false;
            if (Connection.Company_License().ToLower() == "erf00026")
            {
                tag_transac_lft_Inv_in1.Visible = true;
                load_category();
            }
            Load_Bank_Details();
            Checked_Jobs();
            if (Request.QueryString["Page"] == null)
            {
                load_Cus_name();
                txtDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                txtprodate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                HDupdate_id.Value = "";
                Chkdepartment.Checked = true;
                lbldeptapp.Text = hdnuser.Value;
            }
            else
            {
                if (Request.QueryString["Page"] == "GI")
                {
                    Rd_Imp_Exp.SelectedValue = Request.QueryString["IMP_EXP"].ToString();
                    ddl_tr_mode.SelectedValue = Request.QueryString["MODE"].ToString();
                    //ddlbranch_No.SelectedValue = Request.QueryString["CUS_BRANCH_NO_LOC"].ToString();
                    //txt_Jobno.Text = Request.QueryString["JOBNO"].ToString();
                    //ddlCus_name.SelectedValue = Request.QueryString["CUS_NAME"].ToString();
                    btnloadData_Click(new object(), new EventArgs());
                    txtDate.Text = DateTime.Now.ToShortDateString();
                    txtprodate.Text = DateTime.Now.ToShortDateString();
                    txt_Cus_name.Text = Request.QueryString["PARTY_NAME"].ToString();
                    ddlType.SelectedValue = Request.QueryString["Type"].ToString();
                    ddlCus_name.Items.Insert(0, new ListItem(Request.QueryString["CUS_NAME"].ToString(), Request.QueryString["CUS_NAME"].ToString()));
                    ddlCus_name.SelectedIndex = 0;
                    ddlCus_name_SelectedIndexChanged(null, new EventArgs());
                    ddlbranch_No.SelectedValue = Request.QueryString["CUS_BRANCH_NO_LOC"].ToString();
                    List<string> JOBN=new List<string>();
                    JOBN.Add(Request.QueryString["JOBNO"].ToString());
                    cbList.DataSource = JOBN;                    
                    cbList.DataBind();
                    Chkdepartment.Checked = true;
                    lbldeptapp.Text = hdnuser.Value;
                    
                }
               else  if (Request.QueryString["Page"] == "CAN")
                {
                   DataSet ds = new DataSet();
                   ObjUBO.A4=Request.QueryString["JOBNO"].ToString();
                   ObjUBO.A5 = Request.QueryString["MODE"].ToString();
                   ObjUBO.A7 = Request.QueryString["IMP_EXP"].ToString();
                   ObjUBO.A10 = Request.QueryString["Type"].ToString();
                   ObjUBO.A12 = hdnbranch.Value;
                   ObjUBO.A8 = "CANCHECK";
                   ds = BP.Select_Inv(ObjUBO);
                   if (ds.Tables[0].Rows.Count == 0)
                   {
                       if (ds.Tables[1].Rows.Count > 0)
                       {
                           txt_Cus_name.Text = ds.Tables[1].Rows[0]["CLIENT_NAME"].ToString();
                           ddlCus_name.Items.Insert(0, new ListItem((ds.Tables[1].Rows[0]["CONSIGNEENAME"].ToString()), ds.Tables[1].Rows[0]["CONSIGNEENAME"].ToString()));
                           ddlCus_name.SelectedIndex = 0;
                           ddlCus_name_SelectedIndexChanged(null, new EventArgs());
                           ddlbranch_No.SelectedValue = ds.Tables[1].Rows[0]["BRANCHNO"].ToString();
                       }
                       Rd_Imp_Exp.SelectedValue = Request.QueryString["IMP_EXP"].ToString();
                       ddl_tr_mode.SelectedValue = Request.QueryString["MODE"].ToString();
                       //ddlbranch_No.SelectedValue = Request.QueryString["CUS_BRANCH_NO_LOC"].ToString();
                       //txt_Jobno.Text = Request.QueryString["JOBNO"].ToString();
                       //ddlCus_name.SelectedValue = Request.QueryString["CUS_NAME"].ToString();
                       txtDate.Text = DateTime.Now.ToShortDateString();
                       txtprodate.Text = DateTime.Now.ToShortDateString();
                       //txt_Cus_name.Text = Request.QueryString["PARTY_NAME"].ToString();
                       ddlType.SelectedValue = Request.QueryString["Type"].ToString();
                       //ddlCus_name.Items.Insert(0, new ListItem(Request.QueryString["CUS_NAME"].ToString(), Request.QueryString["CUS_NAME"].ToString()));
                       //ddlCus_name.SelectedIndex = 0;
                       //ddlCus_name_SelectedIndexChanged(null, new EventArgs());
                       //ddlbranch_No.SelectedValue = Request.QueryString["CUS_BRANCH_NO_LOC"].ToString();
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
                       btnprorpt.Visible = false; 
                       //string prompt = "<script>$(document).ready(function(){{jAlert('', 'GENERAL', function (r) {document.getElementById('Page_B').click();} );}});</script>";
                       //ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", prompt, false);
                       //btnSave_Click(new object(), new EventArgs());
                   }
                   else
                   {
                        ViewState["Billinvno"]=ds.Tables[0].Rows[0]["BILL_INV_NO"].ToString();
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
                       btnprorpt.Visible = false;
                        //string prompt = "<script>$(document).ready(function(){{jAlert('', 'GENERAL', function (r) {document.getElementById('Page_B').click();});}});</script>";
                        //ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", prompt, false);
                      //btnUpdate.per
                       //btnUpdate_Click(sender, e);
                   }
                   //Page.ClientScript.RegisterStartupScript(Page.GetType(), "Open", "javascript:inv()", true);
                   //Page.ClientScript.RegisterStartupScript(this.GetType(), "Open", "javascript:inv()", true);

                }
                if (Request.QueryString["Page"] != null && Request.QueryString["Page"] != string.Empty && Request.QueryString["Page"] != "GI" && Request.QueryString["Page"] != "CAN")
                {
                    Update_Item_Load();
                    ViewState["UPDATED_ID"] = Request.QueryString["Billinvno"].ToString();                    
                    btnDelete.Visible = false;
                    btnUpdate.Visible = true;
                    btnSave.Visible = false;

                    txtInvoiceNo.Enabled = false;
                    ddlCus_name.Enabled = false;
                    Rd_Imp_Exp.Enabled = false;
                    if (Connection.Company_License().ToLower() == "erf00026")
                    {
                        //if (currentuser.ToUpper() == "CHANDRAKUMAR" || currentuser.ToUpper() == "BLRBM" || currentuser.ToUpper() == "SPRABHU" || currentuser.ToUpper() == "ADMIN" || currentuser.ToUpper() == "NAVANEETH" || currentuser.ToUpper() == "ACCOUNTS" || currentuser.ToUpper() == "ACCOUNTS1" || currentuser.ToUpper() == "ACCOUNTS2" || currentuser.ToUpper() == "ACCOUNTS3" || currentuser.ToUpper() == "ACCOUNTS4" || currentuser.ToUpper() == "SRIDEVI" || currentuser.ToUpper() == "BOMACC" || currentuser.ToUpper() == "REENA" || currentuser.ToUpper() == "JEGAN" || currentuser.ToUpper() == "CRS1" || currentuser.ToUpper() == "CRS2" || currentuser.ToUpper() == "VIGNESH" || currentuser.ToUpper() == "AIREXP1") -- commented due to movemax requirement given by Geno on 01/09/2023
                        if (currentuser.ToUpper() == "GKR" || currentuser.ToUpper() == "BLRBM" || currentuser.ToUpper() == "CHANDRAKUMAR" || currentuser.ToUpper() == "SPRABHU" || currentuser.ToUpper() == "ACCOUNTS" || currentuser.ToUpper() == "REENA" || currentuser.ToUpper() == "NAVANEETH" || currentuser.ToUpper() == "BOMACC" || currentuser.ToUpper() == "VAIDEKI") // changed by Antony requirement by Geno on 01/09/2023
                        {
                            Rd_Bill_Type.Enabled = true;
                        }
                        else { Rd_Bill_Type.Enabled = false; }
                    }
                    else if (Connection.Company_License().ToLower() == "erf00051")
                    {
                        if (currentuser.ToUpper() == "ACCOUNTSADMIN") { Rd_Bill_Type.Enabled = true; }
                    }
                    else { 
                         if (Session["Roll"].ToString() == "A") { Rd_Bill_Type.Enabled = true; Rd_Tax_NonTax.Enabled = true; btnUpdate.Enabled = true; }
                        else{
                        Rd_Bill_Type.Enabled = false; }}
                    ddlbranch_No.Enabled = false;
                    ddl_tr_mode.Enabled = false;
                    //ddlContainerTypes.Enabled = false;
                    HD_Showcon.Value = "SAVED";
                    //btnrpt.Visible = false;
                }
            }
            //Printaccess();
            //if (Connection.Company_License().ToLower() == "erf00010")
            //{
            //    if (Chkdepartment.Checked == true && Chkacc.Checked == true)
            //        btn_EInvoice.Visible = true;
            //    else
            //        btn_EInvoice.Visible = false;
            //}
            if (Connection.Company_License().ToLower() == "erf00011")
            {
                Lblfrmquot.Text = "From Quotation";
                if (currentuser.ToUpper() == "ACCMAA" || currentuser.ToUpper() == "ACCTUT" || currentuser.ToUpper() == "FWDCJB" || currentuser.ToUpper() == "ACCMAA1") 
                { Chkacc.Enabled = true; ChkCancelInv.Enabled = true; Chkdepartment.Enabled = true; }
                else { btnUpdate.Enabled = false; }
                ddlbank_name.SelectedValue = "UNION BANK OF INDIA";
                //if (currentuser.ToUpper() != "ACCMAA1" && currentuser.ToUpper() != "ACCTUT" && currentuser.ToUpper() != "FWDCJB" ) 
                
            }
            else if (Connection.Company_License().ToLower() == "erf00018")
            {
                Lblfrmquot.Text = "From CAN";
                txtInvoiceNo1.Visible = true;
                lblinvno.Visible = true;

                Chkacc.Enabled = true; ChkCancelInv.Enabled = true; Chkdepartment.Enabled = true;
                
            }
            else if (Connection.Company_License().ToLower() == "erf00026")
            {
                Lblfrmquot.Text = "From CAN";
                txtInvoiceNo1.Visible = true;
                lblinvno.Visible = true;
                if (currentuser.ToUpper() == "ACCOUNTS" || currentuser.ToUpper() == "ACCOUNTS3" || currentuser.ToUpper() == "BOMACC") { Chkacc.Enabled = true; ChkCancelInv.Enabled = true; Chkdepartment.Enabled = true; btnrpt.Visible = true; }
                else if (currentuser.ToUpper() == "SUMATHI" || currentuser.ToUpper() == "ANANDA" || currentuser.ToUpper() == "GKR" || currentuser.ToUpper() == "CHANDRAKUMAR" || currentuser.ToUpper() == "BLRBM" || currentuser.ToUpper() == "SPRABHU" || currentuser.ToUpper() == "ADMIN" || currentuser.ToUpper() == "NAVANEETH" || currentuser.ToUpper() == "ACCOUNTS1" || currentuser.ToUpper() == "ACCOUNTS2" || currentuser.ToUpper() == "ACCOUNTS3" || currentuser.ToUpper() == "ACCOUNTS4" || currentuser.ToUpper() == "SRIDEVI" || currentuser.ToUpper() == "BOMACC" || currentuser.ToUpper() == "REENA" || currentuser.ToUpper() == "JEGAN" || currentuser.ToUpper() == "CRS1" || currentuser.ToUpper() == "CRS2" || currentuser.ToUpper() == "VIGNESH" || currentuser.ToUpper() == "AIREXP1" || currentuser.ToUpper() == "BOMACC1" || currentuser.ToUpper() == "CHANDRIKA" || currentuser.ToUpper() == "KAMAKSHI" || currentuser.ToUpper() == "ALAUDEEN")
                { 
                    Chkacc.Enabled = true; ChkCancelInv.Enabled = false; Chkdepartment.Enabled = true; btnrpt.Visible = true; 
                }
                    
                else { Chkacc.Enabled = false; ChkCancelInv.Enabled = false; }
                //Chkacc.Enabled = true; ChkCancelInv.Enabled = true; Chkdepartment.Enabled = true;
                Printaccess();
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
        }
        ddl_state_name.Items.Clear();
        ddl_state_name.Items.Insert(0, new ListItem(hdnstate.Value, hdnstate.Value));
        ddl_state_name.SelectedIndex = 0;
        Session["JOBNO"] = Get_Jobs();
        Session["TYPE"] = ddlType.SelectedValue;
        Session["IMP_EXP"] = Rd_Imp_Exp.SelectedValue.ToString();
        Session["MODE"] = ddl_tr_mode.SelectedValue;

        if (lblmsg.Text != "" || Rd_Tax_NonTax.SelectedValue == "R" || ChkCancelInv.Checked == true || Chkacc.Checked == false) { btn_EInvoice.Visible = false; }
        if (lblmsg.Text != "") { btnUpdate.Visible = false; }
        //Jobno();
        //txtImportername.Attributes.Add("onblur", "javascript:CallServerside_Importer('" + txtImportername.ClientID + "', '" + txtInvoiceNo.ClientID + "')");
        //txtInvoiceNo.Attributes.Add("onblur", "javascript:CallServerside_IMP_INVCHK('" + txtInvoiceNo.ClientID + "', '" + txtInvoiceNo.ClientID + "')");
    }
    public void Page_Show_Hide(int i)
    {
        bool is_page;
        if (i == 97)
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
            btnDelete.Enabled = false;

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
            if (i == 1)
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
            else if (i == 2)
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
        if (Chkdepartment.Checked == true && Chkacc.Checked == true && ChkCancelInv.Checked == false) { btnrpt.Visible = true; }
        else if(ChkCancelInv.Checked==true) { btnrpt.Visible = false; btnprorpt.Visible = false; }

        else { btnrpt.Visible = false; btnprorpt.Visible = true; }
    }
    private void load_category()
    {
        ddlcategory.Items.Clear();
        ddlcategory.Items.Add(new ListItem("", ""));
        ddlcategory.Items.Add(new ListItem("Indian Customers", "Indian Customers"));
        ddlcategory.Items.Add(new ListItem("Overseas Customers", "Overseas Customers"));
        ddlcategory.Items.Add(new ListItem("Korean Customers", "Korean Customers"));
    }
    private void Update_Item_Load()
    {
        DataSet dss = new DataSet();
        hdnTransHistory.Value = " ";
        ddlCus_name.Items.Clear();
        if (Request.QueryString["Page"] != "CAN")
        {
            ObjUBO.A1 = Request.QueryString["Billinvno"].ToString();
            ObjUBO.A4 = Request.QueryString["Page"].ToString();
        }
        else
        {
            ObjUBO.A1 = ViewState["Billinvno"].ToString();
            ObjUBO.A4 = ViewState["Page"].ToString();
        }
            ObjUBO.A7 = Request.QueryString["IMP_EXP"].ToString();
        ObjUBO.A12 = hdnbranch.Value;
        ObjUBO.A8 = "Billing_Updated_Data";

        dss = BP.Select_Inv(ObjUBO);

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
            btnprorpt.Visible = true;
            //-----------------------BA--S---------------------------------------
            HDupdate_id.Value = dss.Tables[0].Rows[0]["ID"].ToString();
            HDupdate_IMP_EXP_id.Value = dss.Tables[0].Rows[0]["IMP_EXP"].ToString();
            Rd_Imp_Exp.SelectedValue = dss.Tables[0].Rows[0]["IMP_EXP"].ToString();
            Hdnmode.Value = dss.Tables[0].Rows[0]["MODE"].ToString();
            Hdntype.Value = dss.Tables[0].Rows[0]["TYPE"].ToString();
            ddlType.SelectedValue = dss.Tables[0].Rows[0]["TYPE"].ToString();
           cbList.DataSource = dss.Tables[0];
           cbList.DataValueField = "JOBNO";
           //cbList.DataValueField = "JOBNO";

           cbList.DataBind();
            ddlCus_name.Items.Insert(0, new ListItem(dss.Tables[0].Rows[0]["CUS_NAME"].ToString(), dss.Tables[0].Rows[0]["CUS_NAME"].ToString()));
            ddlCus_name.SelectedIndex = 0;

            //ddlbranch_No.Items.Insert(0, new ListItem(dss.Tables[0].Rows[0]["CUS_BRANCH_NO_LOC"].ToString(), dss.Tables[0].Rows[0]["CUS_BRANCH_NO"].ToString()));
            //ddlbranch_No.Items.Insert(0, new ListItem(dss.Tables[0].Rows[0]["CUS_BRANCH_NO_LOC"].ToString(), dss.Tables[0].Rows[0]["CUS_BRANCH_NO_LOC"].ToString().Substring(0, 1)));
            ddlbranch_No.SelectedIndex = 0;
            //ddlbranch_No.DataSource = dss.Tables[0];
            //ddlbranch_No.DataValueField = dss.Tables[0].Rows[0]["CUS_BRANCH_NO_LOC"].ToString().Substring(0, 1);
            //ddlbranch_No.DataTextField = dss.Tables[0].Rows[0]["CUS_BRANCH_NO_LOC"].ToString();
            //ddlbranch_No.DataBind();

            //From_Quotation.Checked = Convert.ToBoolean(dss.Tables[0].Rows[0]["FROM_QUOTATION"].ToString());


             txtprodate.Text = dss.Tables[0].Rows[0]["BILL_INV_DATE"].ToString();
             txtDate.Text = dss.Tables[0].Rows[0]["PRO_INV_DATE"].ToString();
            txt_Cus_name.Text = dss.Tables[0].Rows[0]["PARTY_NAME"].ToString();

            if (dss.Tables[0].Rows[0]["AC_BEHALF"].ToString() == "Y")
            {
                ch_Behalf.Checked = true;
            }

            //ddl_state_name.SelectedValue = dss.Tables[0].Rows[0]["STATE_NAME"].ToString();
            hdnstate.Value = dss.Tables[0].Rows[0]["STATE_NAME"].ToString();
            txt_GSTN_Id.Text = dss.Tables[0].Rows[0]["GSTN_ID"].ToString();
            txt_Party_Add.Text = dss.Tables[0].Rows[0]["PARTY_ADDRESS"].ToString();
            Rd_Tax_NonTax.SelectedValue = dss.Tables[0].Rows[0]["TAX_NON_TAX"].ToString();
            Rd_Bill_Type.SelectedValue = dss.Tables[0].Rows[0]["LOCAL_OTHER"].ToString();

            ddlbank_name.SelectedValue = dss.Tables[0].Rows[0]["BANK_DETAILS"].ToString();
            //ddlbank_name.DataSource = dss.Tables[0];
            //ddlbank_name.DataTextField = "BANK_DETAILS";
            //ddlbank_name.DataValueField = "BANK_DETAILS";
            //ddlbank_name.DataBind();
            ddl_tr_mode.SelectedValue = dss.Tables[0].Rows[0]["MODE"].ToString();
            shipmenttype1();
            ddlshipment_type.SelectedValue = dss.Tables[0].Rows[0]["SHIP_TYPE"].ToString();
            txtconttype.Text= dss.Tables[0].Rows[0]["CONTAINER_TYPE"].ToString();
            ddlcategory.SelectedValue = dss.Tables[0].Rows[0]["CATEGORY_TYPE"].ToString();
            txtpino.Text = dss.Tables[0].Rows[0]["TAX_INVNO_PS"].ToString();
            txtCancelReason.Text = dss.Tables[0].Rows[0]["CANCEL_REASON"].ToString();
            ddlworking_Period.SelectedValue = dss.Tables[0].Rows[0]["WORKING_PERIOD_DETAILS"].ToString();
            
            //ddlCus_name.Items.Insert(0, new ListItem(dss.Tables[0].Rows[0]["CUS_NAME"].ToString(), dss.Tables[0].Rows[0]["CUS_NAME"].ToString()));

            if (dss.Tables[0].Rows[0]["QUOT"].ToString() != "" && dss.Tables[0].Rows[0]["QUOT"].ToString() != null)
            {
                ddlQuot.Enabled = true;
                From_Quotation.Checked = true;
                ddlQuot.SelectedValue = dss.Tables[0].Rows[0]["QUOT"].ToString();
                string a = dss.Tables[0].Rows[0]["QUOT"].ToString();
            }
            txtAccHead.Text = dss.Tables[0].Rows[0]["ACC_HEAD"].ToString();
            //ddlContainerTypes.SelectedValue = dss.Tables[0].Rows[0]["CONTAINER_TYPE"].ToString();
            txtPONumber.Text = dss.Tables[0].Rows[0]["PO_NO"].ToString();
            txtVerifiedBy.Text = dss.Tables[0].Rows[0]["VERIFIED_BY"].ToString();
            txtDueDate.Text = dss.Tables[0].Rows[0]["DUE_DATE"].ToString();
            if (dss.Tables[0].Rows[0]["DEPT_APPROV"].ToString() != "N") 
            { Chkdepartment.Checked = true;
            if (dss.Tables[0].Rows[0]["DEPT_APPROV"].ToString() != "Y")
            {
                lbldeptapp.Text = dss.Tables[0].Rows[0]["DEPT_APPROV"].ToString();
            }
            }
            else { Chkdepartment.Checked = false; }
            if (dss.Tables[0].Rows[0]["ACC_APPROV"].ToString() != "N") 
            { Chkacc.Checked = true;
            if (dss.Tables[0].Rows[0]["ACC_APPROV"].ToString() != "Y")
            {
                lblaccapp.Text = dss.Tables[0].Rows[0]["ACC_APPROV"].ToString();
            }
            } else { Chkacc.Checked = false; }

            if (dss.Tables[0].Rows[0]["DEPT_APPROV"].ToString() != "N" || dss.Tables[0].Rows[0]["ACC_APPROV"].ToString() != "N") 
            {
                txtpino.Text = dss.Tables[0].Rows[0]["TAX_INVNO_PS"].ToString();
            
            }
            txtInvoiceNo.Text = dss.Tables[0].Rows[0]["BILL_INV_NO"].ToString();
            txtInvoiceNo1.Text = dss.Tables[0].Rows[0]["ALT_INV_NO"].ToString();
            if (dss.Tables[0].Rows[0]["JOB_STATUS"].ToString() != "Approved" && dss.Tables[0].Rows[0]["JOB_STATUS"].ToString() != "") 
            { ChkCancelInv.Checked = true;
            txtCancelReason.Visible = true;
            if (dss.Tables[0].Rows[0]["JOB_STATUS"].ToString() != "Canceled")
            {
                lblcancel.Text = dss.Tables[0].Rows[0]["JOB_STATUS"].ToString();
            }
            }
            else { ChkCancelInv.Checked = false; }
            txtNarration.Text = dss.Tables[0].Rows[0]["NARRATION"].ToString();
            txtNarration.Text = dss.Tables[0].Rows[0]["NARRATION"].ToString();
            txtPendingreason.Text = dss.Tables[0].Rows[0]["PENDINGREASON"].ToString();
            if (dss.Tables[0].Rows[0]["FROM_CHA"].ToString() == "true")
            {
                Chk_Cha.Checked = true;
                ddlJobno_N.Enabled = false;
            }
            else
            {
                Chk_Cha.Checked = false;
                ddlJobno_N.Enabled = true;
            }
            if (dss.Tables[0].Rows[0]["UPD_PENDINGREASON"].ToString() == "Y")
            {
                chkPendingreasonupdate.Checked = true;
            }
            else { chkPendingreasonupdate.Checked = false; }
            //Rdbbillseztype.SelectedValue = dss.Tables[0].Rows[0]["BILL_TYPE"].ToString();
            Rd_Bill_Type.Enabled=false;
            if (Connection.Company_License().ToLower() == "erf00026")
            {
                //if (currentuser.ToUpper() == "CHANDRAKUMAR" || currentuser.ToUpper() == "BLRBM" || currentuser.ToUpper() == "SPRABHU" || currentuser.ToUpper() == "ADMIN" || currentuser.ToUpper() == "NAVANEETH" || currentuser.ToUpper() == "ACCOUNTS" || currentuser.ToUpper() == "ACCOUNTS1" || currentuser.ToUpper() == "ACCOUNTS2" || currentuser.ToUpper() == "ACCOUNTS3" || currentuser.ToUpper() == "ACCOUNTS4" || currentuser.ToUpper() == "SRIDEVI" || currentuser.ToUpper() == "BOMACC" || currentuser.ToUpper() == "REENA" || currentuser.ToUpper() == "JEGAN" || currentuser.ToUpper() == "CRS1" || currentuser.ToUpper() == "CRS2" || currentuser.ToUpper() == "VIGNESH" || currentuser.ToUpper() == "AIREXP1") -- commented due to movemax requirement given by Geno on 01/09/2023
                if (currentuser.ToUpper() == "GKR" || currentuser.ToUpper() == "BLRBM" || currentuser.ToUpper() == "CHANDRAKUMAR" || currentuser.ToUpper() == "SPRABHU" || currentuser.ToUpper() == "ACCOUNTS" || currentuser.ToUpper() == "REENA" || currentuser.ToUpper() == "NAVANEETH" || Connection.Current_User_Upper() == "BOMACC" || Connection.Current_User_Upper() == "ACCOUNTS3" || Connection.Current_User_Upper() == "VAIDEKI") // changed by Antony requirement by Geno on 01/09/2023
                {
                    Rd_Tax_NonTax.Enabled = true; 
                }
                else
                {
                    Rd_Tax_NonTax.Enabled = false;
                }
                if (Chkacc.Checked == true && Chkdepartment.Checked == true)
                {
                    
                    //if (ChkCancelInv.Checked == true && txtCancelReason.Text != "")
                    //{

                    //    //if (currentuser.ToUpper() == "CHANDRAKUMAR" || currentuser.ToUpper() == "BLRBM" || currentuser.ToUpper() == "SPRABHU" || currentuser.ToUpper() == "ADMIN" || currentuser.ToUpper() == "NAVANEETH" || currentuser.ToUpper() == "ACCOUNTS" || currentuser.ToUpper() == "ACCOUNTS1" || currentuser.ToUpper() == "ACCOUNTS2" || currentuser.ToUpper() == "ACCOUNTS3" || currentuser.ToUpper() == "ACCOUNTS4" || currentuser.ToUpper() == "SRIDEVI" || currentuser.ToUpper() == "BOMACC" || currentuser.ToUpper() == "REENA" || currentuser.ToUpper() == "JEGAN" || currentuser.ToUpper() == "CRS1" || currentuser.ToUpper() == "CRS2" || currentuser.ToUpper() == "VIGNESH" || currentuser.ToUpper() == "AIREXP1") -- commented due to movemax requirement given by Geno on 01/09/2023
                    //    if (currentuser.ToUpper() == "GKR" || currentuser.ToUpper() == "BLRBM" || currentuser.ToUpper() == "CHANDRAKUMAR" || currentuser.ToUpper() == "SPRABHU" || currentuser.ToUpper() == "ACCOUNTS" || currentuser.ToUpper() == "REENA" || currentuser.ToUpper() == "NAVANEETH") // changed by Antony requirement by Geno on 01/09/2023
                    //    {
                    //        btnUpdate.Enabled = true;
                    //    }
                    //}
                    if (Connection.Current_User_Upper() == "ACCOUNTS" || Connection.Current_User_Upper() == "BOMACC" || Connection.Current_User_Upper() == "ACCOUNTS3")
                    {
                        btnUpdate.Enabled = true;
                    }
                    else { btnUpdate.Enabled = false; }
                }
            }
            else if (Connection.Company_License().ToLower() == "erf00055")
            {
                 btnUpdate.Enabled = true; 
            }
            else if (Connection.Company_License().ToLower() == "erf00051")
            {
                if (currentuser.ToUpper() == "ACCOUNTSADMIN") { Rd_Bill_Type.Enabled = true; }
            }
            else
            {
                if (Chkacc.Checked == true && Chkdepartment.Checked == true && Session["roll"].ToString()!="A")
                { btnUpdate.Enabled = false; }
                else if (Session["Roll"].ToString() == "A") { Rd_Bill_Type.Enabled = true; Rd_Tax_NonTax.Enabled = true; btnUpdate.Enabled = true; }
                Rd_Tax_NonTax.Enabled = false;
            }
            Rd_Imp_Exp.Enabled = false;
            ddlType.Enabled = false;
            ddlworking_Period.Enabled = false;
            Chk_Cha.Enabled = false;
            if (Chkacc.Checked != true && Chkdepartment.Checked == true)
            {
                btnprorpt.Visible = true;
                btnemail.Visible = false;
                btnrpt.Visible = false;
            }
            else if (Chkacc.Checked == true && Chkdepartment.Checked == true)
            {
                btnprorpt.Visible = false;
                btnrpt.Visible = true;
                btnemail.Visible = true;
            }
        }

        //if (Chkacc.Checked == true && Chkdepartment.Checked == true) {
        //    if (currentuser.ToUpper() == "CHANDRAKUMAR" || currentuser.ToUpper() == "BLRBM" || currentuser.ToUpper() == "SPRABHU" || currentuser.ToUpper() == "ADMIN" || currentuser.ToUpper() == "NAVANEETH" || currentuser.ToUpper() == "ACCOUNTS" || currentuser.ToUpper() == "ACCOUNTS1" || currentuser.ToUpper() == "ACCOUNTS2" || currentuser.ToUpper() == "ACCOUNTS3" || currentuser.ToUpper() == "ACCOUNTS4" || currentuser.ToUpper() == "SRIDEVI" || currentuser.ToUpper() == "BOMACC" || currentuser.ToUpper() == "REENA" || currentuser.ToUpper() == "JEGAN" || currentuser.ToUpper() == "CRS1" || currentuser.ToUpper() == "CRS2" || currentuser.ToUpper() == "VIGNESH" || currentuser.ToUpper() == "AIREXP1")
        //    {
        //        btnUpdate.Enabled = true;
        //    }
        //    else { btnUpdate.Enabled = false; }
        //    }

        //if (dss.Tables[1].Rows.Count > 0)
        //{
        //    cbList.DataSource = dss.Tables[1];
        //    cbList.DataTextField = "JOBNO_PS";
        //    cbList.DataValueField = "JOBNO";
        //    cbList.DataBind();
        //    Billing_Others();
        //}

        if (dss.Tables[2].Rows.Count > 0)
        {
            gv_Gen_Item_I.DataSource = dss.Tables[2];
            gv_Gen_Item_I.DataBind();
        }
        else
        {
            gv_Gen_Item_I.DataSource = dt;
            gv_Gen_Item_I.DataBind();
        }

        //if (dss.Tables[3].Rows.Count > 0)
        //{
        //    gv_Gen_Item_II.DataSource = dss.Tables[3];
        //    gv_Gen_Item_II.DataBind();
        //}
        //else
        //{
        //    gv_Gen_Item_II.DataSource = dt;
        //    gv_Gen_Item_II.DataBind();
        //}

        if (dss.Tables[3].Rows.Count > 0)
        {
            gv_Chg_Details.DataSource = dss.Tables[3];
            gv_Chg_Details.DataBind();

            DataTable firstTable = dss.Tables[3];
            ViewState["CurrentTable"] = firstTable;
        }
        else
        {
            SetInitialRow();
        }

        if (dss.Tables[4].Rows.Count > 0)
        {
            gv_Gen_Annexure.DataSource = dss.Tables[4];
            gv_Gen_Annexure.DataBind();
        }
        else
        {
            // SetInitialRow();
        }
        Session["JOBNO"] = dss.Tables[0].Rows[0]["JOBNO"].ToString();
        Session["TYPE"] = dss.Tables[0].Rows[0]["TYPE"].ToString();
        Session["IMP_EXP"] = dss.Tables[0].Rows[0]["IMP_EXP"].ToString();
        Session["MODE"] = dss.Tables[0].Rows[0]["MODE"].ToString();
        //--------------THIRU----SESSION FOR MAIL OFF----//
        Session["MAIL_JOB_ID"] = HDupdate_id.Value;
        Session["JOBNO_MAIL"] = dss.Tables[0].Rows[0]["BILL_INV_NO"].ToString();
        //--------------THIRU----SESSION FOR MAIL OFF----//
        //----------THIRU ---START---INVOICE TO JOB LINK ---18-09-2024--------------------//
        Session["GROUP_ID"] = HttpContext.Current.Session["GroupID"].ToString();
        //----------THIRU ---START---INVOICE TO JOB LINK ---18-09-2024--------------------//
        txt_Jobno.Focus();
        if (Connection.Company_License().ToLower() == "erf00010" || (Connection.Company_License().ToLower() == "erf00026" && txtpino.Text != "MAA2425AI-50661" && txtpino.Text != "MAA2425SI-20746" && (Connection.Current_User_Upper() == "ACCOUNTS"||Connection.Current_User_Upper() == "TEST")))
        {
            if (Chkdepartment.Checked == true && Chkacc.Checked == true && ChkCancelInv.Checked!=true && Rd_Tax_NonTax.SelectedValue != "R")
                btn_EInvoice.Visible = true;
            else
                btn_EInvoice.Visible = false;
        }
        else { btn_EInvoice.Visible = false; }
        
        if (dss.Tables[5].Rows.Count > 0) { lblmsg.Text = "Einvoice Generated"; }
        else { lblmsg.Text = ""; }
    }
    private void load_Cus_name()
    {
        try
        {
            ddlCus_name.Items.Clear();
            DataSet dss = new DataSet();
            ObjUBO.A8 = "Cus_Name_Select";
            dss = BP.Select_Inv(ObjUBO);
            if (dss.Tables[0].Rows.Count > 0)
            {
                ddlCus_name.DataSource = dss.Tables[0];
                ddlCus_name.DataTextField = "Cus_Name";
                ddlCus_name.DataValueField = "Cus_Name";
                ddlCus_name.DataBind();
            }
           ddlCus_name.Items.Insert(0, new ListItem(String.Empty, String.Empty));
           ddlCus_name.SelectedIndex = 0;
        }
        catch (Exception ex)
        {
            Connection.Error_Msg(ex.Message);
        }
    }
    protected void ddlCus_name_SelectedIndexChanged(object sender, EventArgs e)
    {
        hdn_cusname.Value = "Customer Name Changed";
        ddlbranch_No.Items.Clear();
        if (ddlCus_name.SelectedValue.ToString() != string.Empty)
        {
            Load_Branch();
        }
    }
    private void Load_Branch()
    {
        ddlbranch_No.Items.Clear();
        DataSet dss = new DataSet();
        ObjUBO.A2 = ddlCus_name.SelectedValue.ToString();
        ObjUBO.A8 = "Cus_Branch_Select";
        dss = BP.Select_Inv(ObjUBO);
        if (dss.Tables[0].Rows.Count > 0)
        {
            ddlbranch_No.DataSource = dss.Tables[0];
            ddlbranch_No.DataTextField = "Branch_Name";
            ddlbranch_No.DataValueField = "Branch_Name";
            ddlbranch_No.DataBind();
        }
        ddlbranch_No.Items.Insert(0, new ListItem(String.Empty, String.Empty));
        ddlbranch_No.SelectedIndex = 0;
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
        hdnTransHistory.Value = "New Row Created in Charge ";
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
                    //---------THIRU--- ADDED ---DISCOUNT---START--08-11-2024-//
                    TextBox txtDiscountRate = (TextBox)gv_Chg_Details.Rows[rowIndex].Cells[11].FindControl("txtDiscountRate");
                    TextBox txtDiscount_AMT = (TextBox)gv_Chg_Details.Rows[rowIndex].Cells[12].FindControl("txtDiscount_AMT");
                    //---------THIRU--- ADDED ---DISCOUNT---END--08-11-2024-//
                    TextBox txtamt = (TextBox)gv_Chg_Details.Rows[rowIndex].Cells[13].FindControl("txtamt");
                    TextBox txtCGST_RATE = (TextBox)gv_Chg_Details.Rows[rowIndex].Cells[14].FindControl("txtCGST_RATE");
                    TextBox txtCGST_AMT = (TextBox)gv_Chg_Details.Rows[rowIndex].Cells[15].FindControl("txtCGST_AMT");
                    TextBox txtSGST_RATE = (TextBox)gv_Chg_Details.Rows[rowIndex].Cells[16].FindControl("txtSGST_RATE");
                    TextBox txtSGST_AMT = (TextBox)gv_Chg_Details.Rows[rowIndex].Cells[17].FindControl("txtSGST_AMT");
                    TextBox txtIGST_RATE = (TextBox)gv_Chg_Details.Rows[rowIndex].Cells[18].FindControl("txtIGST_RATE");
                    TextBox txtIGST_AMT = (TextBox)gv_Chg_Details.Rows[rowIndex].Cells[19].FindControl("txtIGST_AMT");
                    

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
//----------------THIRU ADDED---DISCOUNT---START---08-11-2024-------//
                    dtCurrentTable.Rows[i - 1]["DISC_RATE"] = txtDiscountRate.Text;
                    dtCurrentTable.Rows[i - 1]["DISC_AMT"] = txtDiscount_AMT.Text;
//----------------THIRU ADDED---DISCOUNT---END---08-11-2024-------//
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
                                (Convert.ToDecimal(txtamt.Text == "" ? "0.00" : txtamt.Text) * Convert.ToDecimal(txtCGST_RATE.Text == "" ? "0.00" : txtCGST_RATE.Text)/100)
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
                    //----------------THIRU ADDED---DISCOUNT---START---08-11-2024-------//

                    drCurrentRow["DISC_RATE"] = "";
                    drCurrentRow["DISC_AMT"] = "0";
                    //----------------THIRU ADDED---DISCOUNT---END---08-11-2024-------//


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

                    //---------THIRU--- ADDED ---DISCOUNT---START--08-11-2024-//
                    TextBox txtDiscountRate = (TextBox)gv_Chg_Details.Rows[rowIndex].Cells[10].FindControl("txtDiscountRate");
                    TextBox txtDiscount_AMT = (TextBox)gv_Chg_Details.Rows[rowIndex].Cells[11].FindControl("txtDiscount_AMT");
                    //---------THIRU--- ADDED ---DISCOUNT---END--08-11-2024-//



                    TextBox txtamt = (TextBox)gv_Chg_Details.Rows[rowIndex].Cells[12].FindControl("txtamt");
                    TextBox txtCGST_RATE = (TextBox)gv_Chg_Details.Rows[rowIndex].Cells[13].FindControl("txtCGST_RATE");
                    TextBox txtCGST_AMT = (TextBox)gv_Chg_Details.Rows[rowIndex].Cells[14].FindControl("txtCGST_AMT");
                    TextBox txtSGST_RATE = (TextBox)gv_Chg_Details.Rows[rowIndex].Cells[15].FindControl("txtSGST_RATE");
                    TextBox txtSGST_AMT = (TextBox)gv_Chg_Details.Rows[rowIndex].Cells[16].FindControl("txtSGST_AMT");
                    TextBox txtIGST_RATE = (TextBox)gv_Chg_Details.Rows[rowIndex].Cells[17].FindControl("txtIGST_RATE");
                    TextBox txtIGST_AMT = (TextBox)gv_Chg_Details.Rows[rowIndex].Cells[18].FindControl("txtIGST_AMT");
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

                    //----------------THIRU ADDED---DISCOUNT---START---08-11-2024-------//
                    txtDiscountRate.Text = dt.Rows[i]["DISC_RATE"].ToString();
                    txtDiscount_AMT.Text = dt.Rows[i]["DISC_AMT"].ToString();                    
                    //----------------THIRU ADDED---DISCOUNT---END---08-11-2024-------//


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
        string prompt = "<script>$(document).ready(function(){{jAlert('" + msg + "', 'Billing Invoice', function (r) {});}});</script>";
        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", prompt, false);
    }
    public void Alert_msg(string msg, string focus)
    {
        string prompt = "<script>$(document).ready(function(){{jAlert('" + msg + "', 'Billing Invoice', function (r) {document.getElementById('" + focus + "').focus();});}});</script>";
        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", prompt, false);
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
                    string Re_Inv = string.Empty;
                    DataSet ss = new DataSet();
                    hd_Brslno.Value = ddlbranch_No.SelectedValue.ToString();
                    //--------------------------------------------------------------------------------------------------------------------------


                    //if (Chkdepartment.Checked == true && Chkacc.Checked == true)
                    //{
                    //    ss = Billing_Invoice_Jobno_Insert_Update(Get_Jobs(), "Save with Approval", Product_Item_I_details(), Product_details(), Product_Annexure_details());

                    //}

                    //else
                    //{
                    if (ChkCancelInv.Checked == true && txtCancelReason.Text == "")
                    {
                        Alert_msg("Enter Cancel Reason");}
                    else
                    {
                        if (Rd_Bill_Type.SelectedValue != "" && Rd_Tax_NonTax.SelectedValue != "")
                        {
                            if (gv_Gen_Item_I.Rows.Count > 0 )
                            {
                                ss = Billing_Invoice_Jobno_Insert_Update(Get_Jobs(), "S", Product_Item_I_details(), Product_details(), Product_Annexure_details());

                                //}
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
                                    if (Rd_Tax_NonTax.SelectedValue == "R" || ChkCancelInv.Checked == true || Chkacc.Checked == false) { btn_EInvoice.Visible = false; }
                                    Session["JOBNO"] = Get_Jobs();
                                    Session["TYPE"] = ddlType.SelectedValue;
                                    Session["IMP_EXP"] = Rd_Imp_Exp.SelectedValue.ToString();
                                    Session["MODE"] = ddl_tr_mode.SelectedValue;
                                    //--------------THIRU----SESSION FOR MAIL OFF----//
                                    Session["MAIL_JOB_ID"] = HDupdate_id.Value;
                                    //--------------THIRU----SESSION FOR MAIL OFF----//

                                    //--------------THIRU----SESSION FOR MAIL OFF----//
                                    Session["JOBNO_MAIL"] = hdninvoice.Value;
                                    //--------------THIRU----SESSION FOR MAIL OFF----//
                                    //----------THIRU ---START---INVOICE TO JOB LINK ---18-09-2024--------------------//
                                    Session["GROUP_ID"] = HttpContext.Current.Session["GroupID"].ToString();
                                    //----------THIRU ---START---INVOICE TO JOB LINK ---18-09-2024--------------------//
                                    HDupdate_id.Value = ss.Tables[0].Rows[0][0].ToString();
                                    string dept_approv = ss.Tables[0].Rows[0][2].ToString();
                                    string ACC_approv = ss.Tables[0].Rows[0][3].ToString();
                                    if (Chkdepartment.Checked == true || Chkacc.Checked == true)
                                    {
                                        txtpino.Text = ss.Tables[0].Rows[0][4].ToString();

                                    }
                                    txtInvoiceNo1.Text = ss.Tables[0].Rows[0][5].ToString();
                                    txtprodate.Text = ss.Tables[0].Rows[0][6].ToString();
                                    hdninvoice.Value = ss.Tables[0].Rows[0][1].ToString();
                                    txtInvoiceNo.Text = ss.Tables[0].Rows[0][1].ToString();
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
                                    btnprorpt.Visible = true;

                                    //btnrpt.Visible = true;

                                    F.Visible = false;
                                    T.Visible = true;
                                    /*
                                    ddlCus_name.Attributes.Add("readonly", "readonly");
                                    ddlCus_name.Style.Add("background-color", "#F0F0F0");

                                    txtInvoiceNo.Attributes.Add("readonly", "readonly");
                                    txtInvoiceNo.Style.Add("background-color", "#F0F0F0");
                                    */
                                    Chk_Cha.Enabled = false;
                                    ddlCus_name.Enabled = false;
                                    ddlbranch_No.Enabled = false;
                                    Rd_Bill_Type.Enabled = false;
                                    if (Connection.Company_License().ToLower() == "erf00026")
                                    {

                                        //if (currentuser.ToUpper() == "CHANDRAKUMAR" || currentuser.ToUpper() == "BLRAIR" || currentuser.ToUpper() == "SPRABHU" || currentuser.ToUpper() == "ADMIN" || currentuser.ToUpper() == "NAVANEETH" || currentuser.ToUpper() == "ACCOUNTS" || currentuser.ToUpper() == "ACCOUNTS1" || currentuser.ToUpper() == "ACCOUNTS2" || currentuser.ToUpper() == "ACCOUNTS3" || currentuser.ToUpper() == "ACCOUNTS4" || currentuser.ToUpper() == "SRIDEVI" || currentuser.ToUpper() == "BOMACC" || currentuser.ToUpper() == "REENA" || currentuser.ToUpper() == "JEGAN" || currentuser.ToUpper() == "CRS1" || currentuser.ToUpper() == "CRS2" || currentuser.ToUpper() == "VIGNESH" || currentuser.ToUpper() == "AIREXP1")
                                        if (Connection.Current_User_Upper() == "GKR" || Connection.Current_User_Upper() == "CHANDRAKUMAR" || Connection.Current_User_Upper() == "BLRBM" || Connection.Current_User_Upper() == "SPRABHU" || Connection.Current_User_Upper() == "ADMIN" || Connection.Current_User_Upper() == "NAVANEETH" || Connection.Current_User_Upper() == "ACCOUNTS" || Connection.Current_User_Upper() == "ACCOUNTS1" || Connection.Current_User_Upper() == "ACCOUNTS2" || Connection.Current_User_Upper() == "ACCOUNTS3" || Connection.Current_User_Upper() == "ACCOUNTS4" || Connection.Current_User_Upper() == "SRIDEVI" || Connection.Current_User_Upper() == "BOMACC" || Connection.Current_User_Upper() == "REENA" || Connection.Current_User_Upper() == "JEGAN" || Connection.Current_User_Upper() == "CRS1" || Connection.Current_User_Upper() == "CRS2" || Connection.Current_User_Upper() == "VIGNESH" || Connection.Current_User_Upper() == "AIREXP1" || Connection.Current_User_Upper() == "VAIDEKI")
                                        {
                                            Rd_Tax_NonTax.Enabled = true;
                                        }
                                        else
                                        {
                                            Rd_Tax_NonTax.Enabled = false;
                                        }
                                        if (Chkacc.Checked == true && Chkdepartment.Checked == true)
                                        {
                                            Rd_Tax_NonTax.Enabled = false;
                                            Rd_Bill_Type.Enabled = false;
                                            //if (currentuser.ToUpper() == "CHANDRAKUMAR" || currentuser.ToUpper() == "BLRBM" || currentuser.ToUpper() == "SPRABHU" || currentuser.ToUpper() == "ADMIN" || currentuser.ToUpper() == "NAVANEETH" || currentuser.ToUpper() == "ACCOUNTS" || currentuser.ToUpper() == "ACCOUNTS1" || currentuser.ToUpper() == "ACCOUNTS2" || currentuser.ToUpper() == "ACCOUNTS3" || currentuser.ToUpper() == "ACCOUNTS4" || currentuser.ToUpper() == "SRIDEVI" || currentuser.ToUpper() == "BOMACC" || currentuser.ToUpper() == "REENA" || currentuser.ToUpper() == "JEGAN" || currentuser.ToUpper() == "CRS1" || currentuser.ToUpper() == "CRS2" || currentuser.ToUpper() == "VIGNESH" || currentuser.ToUpper() == "AIREXP1") -- commented due to movemax requirement given by Geno on 01/09/2023
                                            //if (Connection.Current_User_Upper() == "GKR" || Connection.Current_User_Upper() == "CHANDRAKUMAR" || Connection.Current_User_Upper() == "SPRABHU" || Connection.Current_User_Upper() == "ACCOUNTS" || Connection.Current_User_Upper() == "REENA" || Connection.Current_User_Upper() == "NAVANEETH") // changed by Antony requirement by Geno on 01/09/2023
                                            //{
                                            //    btnUpdate.Enabled = false;
                                            //}
                                            if (Connection.Current_User_Upper() == "ACCOUNTS")
                                            {
                                                btnUpdate.Enabled = true;
                                            }
                                            else { btnUpdate.Enabled = false; }
                                        }
                                    }
                                    else if (Connection.Company_License().ToLower() == "erf00051")
                                    {
                                        if (Connection.Current_User_Upper() == "ACCOUNTSADMIN") { Rd_Bill_Type.Enabled = true; }
                                    }
                                    else if (Connection.Company_License().ToLower() == "erf00055")
                                    {
                                         btnUpdate.Enabled = true; 
                                    }
                                    else
                                    {
                                        if (Session["Roll"].ToString() == "A") { Rd_Bill_Type.Enabled = true; Rd_Tax_NonTax.Enabled = true; btnUpdate.Enabled = true; }
                                        else
                                        {
                                            Rd_Tax_NonTax.Enabled = false;
                                            //btnUpdate.Enabled = false;
                                            btnUpdate.Enabled = true;
                                        }
                                    }
                                    //if (Chkacc.Checked == true && Chkdepartment.Checked == true)
                                    //{

                                    //    btnUpdate.Enabled = false;

                                    //}
                                    Rd_Imp_Exp.Enabled = false;
                                    ddlType.Enabled = false;
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
                            }
                            else
                            { Alert_msg("Click Load Date and Save", "btnSave"); }
                        }

                        else
                        {
                            Alert_msg("Select Tax type & Bill To", "btnSave");
                        }
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
            string j= cbList.Items[0].Text;
            hd_Brslno.Value = ddlbranch_No.SelectedValue.ToString();
            hdninvoice.Value = txtInvoiceNo.Text;
            //dr = Billing_Invoice_Jobno_Insert_Update(Get_Jobs(), "U", Product_Item_I_details(), Product_details(), Product_Annexure_details());
            if (ChkCancelInv.Checked == true && txtCancelReason.Text == "")
            {
                Alert_msg("Enter Cancel Reason");
                
            }
            else
            {
                if (Chkdepartment.Checked == true && Chkacc.Checked == true && txtpino.Text == "")
                {
                    dr = Billing_Invoice_Jobno_Insert_Update(Get_Jobs(), "UPADTE_WITH_APPROVAL", Product_Item_I_details(), Product_details(), Product_Annexure_details());
                }

                else
                {

                    dr = Billing_Invoice_Jobno_Insert_Update(Get_Jobs(), "U", Product_Item_I_details(), Product_details(), Product_Annexure_details());
                }

                //dr = Billing_Invoice_Jobno_Insert_Update(Get_Jobs(), "U", Product_Item_II_details());
                if (dr.Tables[0].Rows.Count > 0)
                {
                    //if (ViewState["UPDATED_ID"] != null)
                    //{
                    Alert_msg("Updated Successfully", "btnUpdate");
                    if (Rd_Tax_NonTax.SelectedValue == "R" || ChkCancelInv.Checked == true || Chkacc.Checked == false) { btn_EInvoice.Visible = false; }
                    Session["JOBNO"] = Get_Jobs();
                    Session["TYPE"] = ddlType.SelectedValue;
                    Session["IMP_EXP"] = Rd_Imp_Exp.SelectedValue.ToString();
                    Session["MODE"] = ddl_tr_mode.SelectedValue;
                    //--------------THIRU----SESSION FOR MAIL OFF----//
                    Session["MAIL_JOB_ID"] = HDupdate_id.Value;
                    Session["JOBNO_MAIL"] = dr.Tables[0].Rows[0]["BILL_INV_NO"].ToString();
                    //--------------THIRU----SESSION FOR MAIL OFF----//

                    //----------THIRU ---START---INVOICE TO JOB LINK ---18-09-2024--------------------//
                    Session["GROUP_ID"] = HttpContext.Current.Session["GroupID"].ToString();
                    //----------THIRU ---START---INVOICE TO JOB LINK ---18-09-2024--------------------//

                    HDupdate_id.Value = dr.Tables[0].Rows[0][0].ToString();
                    string dept_approv = dr.Tables[0].Rows[0][2].ToString();
                    string ACC_approv = dr.Tables[0].Rows[0][3].ToString();
                    if (Chkdepartment.Checked == true || Chkacc.Checked == true)
                    {
                        txtpino.Text = dr.Tables[0].Rows[0][4].ToString();

                    }
                    txtInvoiceNo1.Text = dr.Tables[0].Rows[0][5].ToString();
                    txtInvoiceNo.Text = dr.Tables[0].Rows[0][1].ToString();
                    txtprodate.Text = dr.Tables[0].Rows[0][6].ToString();
                    //txtInvoiceNo1.Text = dr.Tables[0].Rows[0][1].ToString();
                    hdninvoice.Value = dr.Tables[0].Rows[0][1].ToString();
                    HDupdate_IMP_EXP_id.Value = Rd_Imp_Exp.SelectedValue.ToString();
                    Hdnmode.Value = ddl_tr_mode.SelectedValue.ToString();
                    Hdntype.Value = ddlType.SelectedValue.ToString();
                    hdnmisc.Value = drpmisc.SelectedValue;
                    if (dr.Tables[0].Rows[0][2].ToString() != "N" && dr.Tables[0].Rows[0][2].ToString() != "" && dr.Tables[0].Rows[0][2].ToString() != null)
                    {
                        //lbldeptapp.Text=ss.Tables[0].Rows[0][2].ToString();
                        btnrpt.Visible = true;
                    }
                    if (dr.Tables[0].Rows[0][3].ToString() != "N" && dr.Tables[0].Rows[0][3].ToString() != "" && dr.Tables[0].Rows[0][3].ToString() != null)
                    {
                        //lbldeptapp.Text=ss.Tables[0].Rows[0][2].ToString();
                        btnrpt.Visible = true;
                    }
                    btnprorpt.Visible = true;
                    //}
                    //else
                    //{
                    //    Alert_msg("Saved Successfully", "btnUpdate");
                    //    HD_Showcon.Value = "SAVED";
                    //}
                }
                else
                {
                    btnUpdate.Visible = true;
                    btnDelete.Visible = false;
                    btnSave.Visible = false;
                    btnNew.Visible = true;
                    Alert_msg("Not Saved", "btnUpdate");
                }
                if (Request.QueryString["Page"] == "CAN")
                {
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "Open", "javascript:inv('" + HDupdate_id.Value + "','" + HDupdate_IMP_EXP_id.Value + "','" + hdninvoice.Value + "','" + Hdnmode.Value + "','" + Hdntype.Value + "','" + hdnmisc.Value + "')", true);
                }
                if (Connection.Company_License().ToLower() == "erf00026")
                {
                    if (Chkacc.Checked == true && Chkdepartment.Checked == true)
                    {
                        Rd_Tax_NonTax.Enabled = false;
                        Rd_Bill_Type.Enabled = false;
                        //if (currentuser.ToUpper() == "CHANDRAKUMAR" || currentuser.ToUpper() == "BLRBM" || currentuser.ToUpper() == "SPRABHU" || currentuser.ToUpper() == "ADMIN" || currentuser.ToUpper() == "NAVANEETH" || currentuser.ToUpper() == "ACCOUNTS" || currentuser.ToUpper() == "ACCOUNTS1" || currentuser.ToUpper() == "ACCOUNTS2" || currentuser.ToUpper() == "ACCOUNTS3" || currentuser.ToUpper() == "ACCOUNTS4" || currentuser.ToUpper() == "SRIDEVI" || currentuser.ToUpper() == "BOMACC" || currentuser.ToUpper() == "REENA" || currentuser.ToUpper() == "JEGAN" || currentuser.ToUpper() == "CRS1" || currentuser.ToUpper() == "CRS2" || currentuser.ToUpper() == "VIGNESH" || currentuser.ToUpper() == "AIREXP1") -- commented due to movemax requirement given by Geno on 01/09/2023
                        //if (ChkCancelInv.Checked == true && txtCancelReason.Text != "")
                        //{
                        //    if (currentuser.ToUpper() == "GKR" || currentuser.ToUpper() == "CHANDRAKUMAR" || currentuser.ToUpper() == "SPRABHU" || currentuser.ToUpper() == "ACCOUNTS" || currentuser.ToUpper() == "REENA" || currentuser.ToUpper() == "NAVANEETH") // changed by Antony requirement by Geno on 01/09/2023
                        //    {
                        //        btnUpdate.Enabled = true;
                        //    }
                        //}
                        if (Connection.Current_User_Upper() == "ACCOUNTS")
                        {
                            btnUpdate.Enabled = true;
                        }
                        else { btnUpdate.Enabled = false; }
                    }
                }
                else
                {
                    if (Chkacc.Checked == true && Chkdepartment.Checked == true)
                    {
                        if (ChkCancelInv.Checked == true && txtCancelReason.Text != "")
                        {

                            btnUpdate.Enabled = true;

                        }
                        else if (Session["Roll"].ToString() == "A") { Rd_Bill_Type.Enabled = true; Rd_Tax_NonTax.Enabled = true; btnUpdate.Enabled = true; }
                        else
                        {
                            btnUpdate.Enabled = true;
                        }

                    }
                }
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
            ObjUBO.A1 = txtInvoiceNo.Text;
            ObjUBO.A8 = "Delete";
            ObjUBO.A12 = hdnbranch.Value;
            dss = BP.Select_Inv(ObjUBO);
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
        hdn_item.Value = "Item Details Changed";
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
        dtt.Columns.Add(new System.Data.DataColumn("SALES", typeof(String)));

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
            TextBox txt_Sales = (TextBox)row.FindControl("txt_Sales");

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
            dr[14] = txt_Sales.Text;
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
        hdn_value.Value = "Product Item II details Changed";
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
        hdn_charge.Value = "Product Charge Changed";
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

        //----------------THIRU ADDED---DISCOUNT---START---08-11-2024-------//
        dtt.Columns.Add(new System.Data.DataColumn("DISC_RATE", typeof(String)));
        dtt.Columns.Add(new System.Data.DataColumn("DISC_AMT", typeof(String)));
        
        //----------------THIRU ADDED---DISCOUNT---END---08-11-2024-------//
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
            //---------THIRU--- ADDED ---DISCOUNT---START--08-11-2024-//
            TextBox txtDiscountRate = (TextBox)row.FindControl("txtDiscountRate");
            TextBox txtDiscount_AMT = (TextBox)row.FindControl("txtDiscount_AMT");
            //---------THIRU--- ADDED ---DISCOUNT---END--08-11-2024-//
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
                //----------------THIRU ADDED---DISCOUNT---START---08-11-2024-------//
                dr[9] = txtDiscountRate.Text;
                dr[10] = txtDiscount_AMT.Text;
                //----------------THIRU ADDED---DISCOUNT---END---08-11-2024-------//
                dr[11] = txtamt.Text;
                dr[12] = txtCGST_RATE.Text;
                dr[13] = txtCGST_AMT.Text;
                dr[14] = txtSGST_RATE.Text;
                dr[15] = txtSGST_AMT.Text;
                dr[16] = txtIGST_RATE.Text;
                dr[17] = txtIGST_AMT.Text;
                dr[18] = R;
                dr[19] = txtOrder_by.Text;
                dr[20] = txt_charge_desc.Text;
                dtt.Rows.Add(dr);
                R = R + 1;
            }
        }
        DataSet dsData = new DataSet();
        dsData.Tables.Add(dtt);
        String xmlData = ConvertDataTableToXML(dsData.Tables[0]);
        return xmlData;
    }
    private string Product_Annexure_details()
    {
        DataTable dtt = new DataTable();
        DataRow dr;

        dtt.Columns.Add(new System.Data.DataColumn("JOBNO", typeof(String)));
        dtt.Columns.Add(new System.Data.DataColumn("CH_AMT_A", typeof(String)));
        dtt.Columns.Add(new System.Data.DataColumn("CH_AMT_B", typeof(String)));
        dtt.Columns.Add(new System.Data.DataColumn("CH_AMT_C", typeof(String)));
        dtt.Columns.Add(new System.Data.DataColumn("CH_AMT_D", typeof(String)));
        dtt.Columns.Add(new System.Data.DataColumn("CH_AMT_E", typeof(String)));
        dtt.Columns.Add(new System.Data.DataColumn("CH_AMT_F", typeof(String)));
        dtt.Columns.Add(new System.Data.DataColumn("CH_AMT_G", typeof(String)));
        dtt.Columns.Add(new System.Data.DataColumn("CH_AMT_H", typeof(String)));
        dtt.Columns.Add(new System.Data.DataColumn("CH_AMT_I", typeof(String)));
        dtt.Columns.Add(new System.Data.DataColumn("CH_AMT_J", typeof(String)));
        dtt.Columns.Add(new System.Data.DataColumn("CH_AMT_K", typeof(String)));
        dtt.Columns.Add(new System.Data.DataColumn("CH_AMT_L", typeof(String)));
        dtt.Columns.Add(new System.Data.DataColumn("CH_AMT_M", typeof(String)));
        dtt.Columns.Add(new System.Data.DataColumn("CH_AMT_N", typeof(String)));
        dtt.Columns.Add(new System.Data.DataColumn("CH_AMT_O", typeof(String)));

        int R = 1;
        //foreach (GridViewRow row in gv_Gen_Annexure.Rows)
        //{
        //    TextBox txt_Anx_Jobno = (TextBox)row.FindControl("txt_Anx_Jobno");
        //    TextBox txt_Ch_Amt_A = (TextBox)row.FindControl("txt_Ch_Amt_A");
        //    TextBox txt_Ch_Amt_B = (TextBox)row.FindControl("txt_Ch_Amt_B");
        //    TextBox txt_Ch_Amt_C = (TextBox)row.FindControl("txt_Ch_Amt_C");
        //    TextBox txt_Ch_Amt_D = (TextBox)row.FindControl("txt_Ch_Amt_D");
        //    TextBox txt_Ch_Amt_E = (TextBox)row.FindControl("txt_Ch_Amt_E");
        //    TextBox txt_Ch_Amt_F = (TextBox)row.FindControl("txt_Ch_Amt_F");
        //    TextBox txt_Ch_Amt_G = (TextBox)row.FindControl("txt_Ch_Amt_G");
        //    TextBox txt_Ch_Amt_H = (TextBox)row.FindControl("txt_Ch_Amt_H");
        //    TextBox txt_Ch_Amt_I = (TextBox)row.FindControl("txt_Ch_Amt_I");
        //    TextBox txt_Ch_Amt_J = (TextBox)row.FindControl("txt_Ch_Amt_J");
        //    TextBox txt_Ch_Amt_K = (TextBox)row.FindControl("txt_Ch_Amt_K");
        //    TextBox txt_Ch_Amt_L = (TextBox)row.FindControl("txt_Ch_Amt_L");
        //    TextBox txt_Ch_Amt_M = (TextBox)row.FindControl("txt_Ch_Amt_M");
        //    TextBox txt_Ch_Amt_N = (TextBox)row.FindControl("txt_Ch_Amt_N");
        //    TextBox txt_Ch_Amt_O = (TextBox)row.FindControl("txt_Ch_Amt_O");

        //    dr = dtt.NewRow();
        //    dr[0] = txt_Anx_Jobno.Text;
        //    dr[1] = txt_Ch_Amt_A.Text;
        //    dr[2] = txt_Ch_Amt_B.Text;
        //    dr[3] = txt_Ch_Amt_C.Text;
        //    dr[4] = txt_Ch_Amt_D.Text;
        //    dr[5] = txt_Ch_Amt_E.Text;
        //    dr[6] = txt_Ch_Amt_F.Text;
        //    dr[7] = txt_Ch_Amt_G.Text;
        //    dr[8] = txt_Ch_Amt_H.Text;
        //    dr[9] = txt_Ch_Amt_I.Text;
        //    dr[10] = txt_Ch_Amt_J.Text;
        //    dr[11] = txt_Ch_Amt_K.Text;
        //    dr[12] = txt_Ch_Amt_L.Text;
        //    dr[13] = txt_Ch_Amt_M.Text;
        //    dr[14] = txt_Ch_Amt_N.Text;
        //    dr[15] = txt_Ch_Amt_O.Text;
        //    dtt.Rows.Add(dr);
        //    R = R + 1;
        //}
        DataSet dsData = new DataSet();
        dsData.Tables.Add(dtt);
        String xmlData = ConvertDataTableToXML(dsData.Tables[0]);
        return xmlData;
    }
    //public DataSet Billing_Invoice_Jobno_Insert_Update(string Jobs, string S1, String xmlData_B)
    public DataSet Billing_Invoice_Jobno_Insert_Update(string Jobs, string S1, String xmlData_A,  String xmlData_C, String xmlData_D)

    {
        
            ObjUBO.A1 = HDupdate_id.Value.ToString();
            ObjUBO.A2 = Rd_Imp_Exp.SelectedValue.ToString();
            ObjUBO.A3 = Rd_Bill_Type.SelectedValue.ToString();
            ObjUBO.A4 = ddlCus_name.SelectedValue.ToString();
            //ObjUBO.A5 = ddlbranch_No.SelectedValue.ToString();
            ObjUBO.A5 = hd_Brslno.Value;
            if (ddlbranch_No.SelectedItem != null)
            {
                ObjUBO.A23 = ddlbranch_No.SelectedItem.ToString();
            }
            ObjUBO.A6 = From_Quotation.Checked.ToString();
            ObjUBO.A7 = Jobs;
            ObjUBO.A8 = hdninvoice.Value;
            ObjUBO.A9 = txtprodate.Text;
            ObjUBO.A10 = txt_Cus_name.Text;
            ObjUBO.A11 = ch_Behalf.Checked == true ? "Y" : "N";
            //ObjUBO.A12 = ddl_state_name.SelectedValue.ToString();
            ObjUBO.A12 = hdnstate.Value;
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
            ObjUBO.A27 = ddlshipment_type.SelectedValue.ToString();

            ObjUBO.A35 = xmlData_A;
            //ObjUBO.A36 = xmlData_B;
            ObjUBO.A37 = xmlData_C;
            ObjUBO.A38 = xmlData_D;
            ObjUBO.A39 = S1;
            ObjUBO.A40 = txtAccHead.Text;
            ObjUBO.A41 = txtPONumber.Text;
            ObjUBO.A42 = hdnbranch.Value;
            ObjUBO.A43 = ddlType.SelectedValue.ToString();
            ObjUBO.A46 = txtNarration.Text;
            ObjUBO.A49 = ddlQuot.SelectedValue;
            //ObjUBO.A50 = ddlContainerTypes.SelectedValue;
            ObjUBO.A44 = Chkdepartment.Checked == true ? lbldeptapp.Text : "N";
            ObjUBO.A45 = Chkacc.Checked == true ? lblaccapp.Text : "N";
            ObjUBO.A47 = ChkCancelInv.Checked == true ? lblcancel.Text : "Approved";
            ObjUBO.A48 = txtVerifiedBy.Text;
            ObjUBO.A51 = txtDueDate.Text;
            ObjUBO.A52 = txtInvoiceNo1.Text;
            ObjUBO.A53 = Rdbbillseztype.SelectedValue;
            ObjUBO.A54 = txtpino.Text;
            //ObjUBO.A55 = "";
            ObjUBO.A56 = txtDate.Text;
            ObjUBO.A50 = txtconttype.Text;
            ObjUBO.A58 = ddlcategory.SelectedValue.ToString();
            ObjUBO.A59 = txtCancelReason.Text;
            ObjUBO.A60 = txtPendingreason.Text;
            ObjUBO.A61 = chkPendingreasonupdate.Checked == true ? "Y" : "N";
            ObjUBO.A62 = ddlworking_Period.SelectedValue;
            ObjUBO.A63 = Chk_Cha.Checked == true ? "true" : "false";
            ObjUBO.A64 = hdn_billto.Value + "  " + hdn_add.Value + "  " + hdn_load.Value + "  " + hdn_remove.Value + "  " + hdn_cusname.Value + "  " + hdn_deppart.Value + "  " + hdn_approved.Value + "  " + hdn_cancel.Value + "  " + hdnTransHistory.Value + "  " + hdn_rowdelete.Value + " " + hdn_item.Value + "  " + hdn_value.Value + "  " + hdn_charge.Value + " " + hdn_einvoice.Value;
           
            //if (Chkdepartment.Checked == true) { ObjUBO.A44 = lbldeptapp.Text; } else { ObjUBO.A44 = "N"; }
            //if (Chkacc.Checked == true) { ObjUBO.A45 = lblaccapp.Text; } else { ObjUBO.A45 = "N"; }
            //if (ChkCancelInv.Checked == true) { ObjUBO.A47 = lblcancel.Text; } else { ObjUBO.A47 = "Approved"; }
            return BP.Billing_Invoice_INS_UPD(ObjUBO);
        
        
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
        //----------------THIRU ADDED---DISCOUNT---START---08-11-2024-------//
        dt.Columns.Add(new DataColumn("DISC_RATE", typeof(string)));
        dt.Columns.Add(new DataColumn("DISC_AMT", typeof(string)));        
        //----------------THIRU ADDED---DISCOUNT---END---08-11-2024-------//
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
        //----------------THIRU ADDED---DISCOUNT---START---08-11-2024-------//
        dr["DISC_RATE"] = "";
        dr["DISC_AMT"] = "0";        
        
        //----------------THIRU ADDED---DISCOUNT---END---08-11-2024-------//
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
        hdn_rowdelete.Value = "Charge Row Deleting";
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
                //----------------THIRU ADDED---DISCOUNT---START---08-11-2024-------//
                dtnew.Columns.Add(new DataColumn("DISC_RATE", typeof(string)));
                dtnew.Columns.Add(new DataColumn("DISC_AMT", typeof(string)));
                //----------------THIRU ADDED---DISCOUNT---END---08-11-2024-------//

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
                    //----------------THIRU ADDED---DISCOUNT---START---08-11-2024-------//
                    drnew["DISC_RATE"] = dt.Rows[i][11].ToString();
                    drnew["DISC_AMT"] = dt.Rows[i][12].ToString();                    
                    //----------------THIRU ADDED---DISCOUNT---END---08-11-2024-------//
                    drnew["AMOUNT"] = dt.Rows[i][13].ToString();
                    drnew["CGST_RATE"] = dt.Rows[i][14].ToString();
                    drnew["CGST_AMT"] = dt.Rows[i][15].ToString();
                    drnew["SGST_RATE"] = dt.Rows[i][16].ToString();
                    drnew["SGST_AMT"] = dt.Rows[i][17].ToString();
                    drnew["IGST_RATE"] = dt.Rows[i][18].ToString();
                    drnew["IGST_AMT"] = dt.Rows[i][19].ToString();
                    drnew["CHARGE_DESC"] = dt.Rows[i][20].ToString();

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
                            (Convert.ToDecimal(dt.Rows[i][11].ToString() == "" ? "0.00" : dt.Rows[i][11].ToString()) * Convert.ToDecimal(dt.Rows[i][12].ToString() == "" ? "0.00" : dt.Rows[i][12].ToString())/100)
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

                //----------------THIRU ADDED---DISCOUNT---START---08-11-2024-------//
                dtt.Columns.Add(new DataColumn("DISC_RATE", typeof(string)));
                dtt.Columns.Add(new DataColumn("DISC_AMT", typeof(string)));                
                
                //----------------THIRU ADDED---DISCOUNT---END---08-11-2024-------//
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
                //----------------THIRU ADDED---DISCOUNT---START---08-11-2024-------//
                drr["DISC_RATE"] = "";
                drr["DISC_AMT"] = "0";

                //----------------THIRU ADDED---DISCOUNT---END---08-11-2024-------//
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

        //Clear();
        HDupdate_id.Value = string.Empty;

        ddlCus_name.Enabled = true;
        ddlbranch_No.Enabled = true;
        btnrpt.Visible = false;

        load_Cus_name();
        ddlbranch_No.Items.Clear();
        ddlbranch_No.Items.Insert(0, new ListItem(String.Empty, String.Empty));
        ddlbranch_No.SelectedIndex = 0;

        ddlCus_name.Style.Add("background-color", "white");
        Rd_Bill_Type.Enabled = true;
        Rd_Imp_Exp.Enabled = true;
        //load_date();
        txtDate.Text = DateTime.Now.ToString("dd/MM/yyyy");

        gv_Gen_Item_I.DataSource = dt;
        gv_Gen_Item_I.DataBind();

        //gv_Gen_Item_II.DataSource = dt;
        //gv_Gen_Item_II.DataBind();

        gv_Gen_Annexure.DataSource = dt;
        gv_Gen_Annexure.DataBind();
        ddl_tr_mode.Enabled = true;
        btnprorpt.Visible = false;
        ddlType.Enabled = true;
        ddlworking_Period.Enabled = true;
        lblaccapp.Text = "";        
        lblcancel.Text = "";
        cbList.Items.Clear();
        Chkdepartment.Checked = true;
        lbldeptapp.Text = hdnuser.Value;
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

    //[System.Web.Services.WebMethod]
    //public static string Get_Importer_Inv_Chk(string custid, string Mode, string Branch)
    //{
    //    GST_Imp_Invoice BI = new GST_Imp_Invoice();
    //    Billing_UserBO ObjUBO = new Billing_UserBO();

    //    if (custid != string.Empty)
    //    {
    //        DataSet dss = new DataSet();
    //        ObjUBO.JOBNO = Mode;
    //        ObjUBO.BILL_INV_NO = custid;
    //        ObjUBO.Flag = "Imp_Inv_No_Check";
    //        dss = BI.Select_IMP_INV(ObjUBO);

    //        if (dss.Tables[0].Rows.Count > 0)
    //        {
    //            custid = "No";
    //        }
    //        else
    //        {
    //            custid = "Yes";
    //        }
    //    }
    //    else
    //    {
    //        custid = "Yes";
    //    }
    //    return custid;
    //}
    [System.Web.Services.WebMethod]
    public static string Get_Supplier(string custid,string Jobno)
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
    public static string Get_Cha_Type(string ChargeName, string Imp_Name, string Mode,string branch)
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
                BI.Ename= "E_EXG_RATE";
                BI.Eexchange_effect_dt_to = dat;

                 
            }

            ds = BI.RetrieveAll_Exchange_master();
            if (BI.result == 1)
            {

                if (ds.Tables[0].Rows.Count > 0)
                {
                    //if (IO != "IO")
                    //{
                        exrate = ds.Tables[0].Rows[0]["EX_RATE"].ToString();
                    //}
                    //else if (IO == "IO") {
                    //    exrate = "1"; 
                    //}
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

    protected void Chk_Cha_Type_CheckedChanged(object sender, EventArgs e)
    {
        if (Chk_Cha.Checked == true)
        {
            ddlJobno_N.Enabled = false;
        }
        else
        {
            ddlJobno_N.Enabled = true;
        }
    }

    protected void Rd_Bill_Type_SelectedIndexChanged(object sender, EventArgs e)
    {
        hdn_billto.Value = "Bill To Type Changed";
        Hidden_val.Value = "1";
        if (cbList.Items.Count != 0)
        {
            btnloadData_Click(new object(), new EventArgs());
        }
        if (Rd_Bill_Type.SelectedValue == "L" || Rd_Bill_Type.SelectedValue == "O")
        {
            Rd_Tax_NonTax.SelectedValue = "T";
        }
        else if (Rd_Bill_Type.SelectedValue == "IO")
        {
            Rd_Tax_NonTax.SelectedValue = "N";
        }
        Rd_Tax_NonTax.Focus();
        //if (Rd_Bill_Type.SelectedValue != "IO")
        //{
        //    Get_Cha_exrate(Rd_Bill_Type.SelectedValue, Rd_Imp_Exp.SelectedValue, txtDate.Text);
        //}
    }
    //[System.Web.Services.WebMethod]
    //public static string Inv_Refresh(string Inv)
    //{
    //    DataSet ds =new DataSet();
    //    GST_Imp_Invoice BI = new GST_Imp_Invoice();
    //    Billing_UserBO ObjUBO = new Billing_UserBO();

    //    ObjUBO.Flag = "Refresh_Invoice";
    //    ds=BI.Select_IMP_INV(ObjUBO);

    //    if (ds.Tables[0].Rows.Count > 0)
    //    {
    //        return ds.Tables[0].Rows[0][0].ToString();
    //    }
    //    else
    //    {
    //        return "";
    //    }
    //}
    protected void btn_Addjobno_Click(object sender, EventArgs e)
    {

          try
        {
            hdn_add.Value = "Added jobno";
        string msgc="";
        //string confirm = "jConfirm('Already invoice created for this job, Do you want to continue?', 'INVOICE', function(r) {" +
        //          "var i = r + 'ok';" +
        // " if(i == 'trueok'){ document.getElementById('hdconfirm').value='n';}" +
        //  "else { }});return false;";
        //btn_Addjobno.Attributes.Add("onclick", confirm);
             DataSet s = new DataSet();
        eFreightQuotation_Transactions Trans = new eFreightQuotation_Transactions();

        if (ddlType.SelectedValue == "CLEARING" && Chk_Cha.Checked==true)
        {
            if (txt_Jobno.Text != string.Empty)
            {

                //ObjUBO.A4 = Get_Jobs();
                Trans.jobno = Get_Jobs();
                Trans.Jobnops = txt_Jobno.Text;
                Trans.flag = "Bill_Jobno_Validate";
                Trans.SEARCH_ITEM = Rd_Imp_Exp.SelectedValue.ToString();

                //s = BP.Select_Inv(ObjUBO);
                //if (Connection.Billing_Working_Period() == "Y")
                //{
                //    s = Database_Con_string();
                //}
                //else
                //{
                s = Trans.CHA_IMP_EXP_INVOICE_Load();
                //}
                if (s.Tables[0].Columns.Contains("JOBNO_PS"))
                {
                    if (s.Tables[0].Rows.Count > 0)
                    {
                        Rd_Imp_Exp.Enabled = false;

                        cbList.DataSource = s.Tables[0];
                        cbList.DataTextField = "JOBNO_PS";
                        cbList.DataValueField = "JOBNO_PS";
                        cbList.DataBind();

                        txt_Jobno.Text = string.Empty;
                        Checked_Jobs();
                        Billing_Others();
                    }
                    else
                    {
                        Alert_msg("Invalid Jobno !");
                    }
                }
                else
                {
                    Alert_msg("Clearing Job cannot be Loaded");
                }
            }
            else
            {
                Alert_msg("Enter Ur Jobno", "txt_Jobno");
            }
        }
        else
        {
            if (txt_Jobno.Text != string.Empty)
            {
                if (ddl_tr_mode.SelectedIndex != 0)
                {
                    ObjUBO.A2 = ddlCus_name.SelectedValue.ToString();
                    ObjUBO.A3 = ddlbranch_No.SelectedValue.ToString();
                    ObjUBO.A4 = Get_Jobs();
                    //ObjUBO.A4 = txt_Jobno.Text;
                    ObjUBO.A11 = txt_Jobno.Text;
                    ObjUBO.A5 = ddl_tr_mode.SelectedValue.ToString();
                    ObjUBO.A7 = Rd_Imp_Exp.SelectedValue.ToString();
                    ObjUBO.A8 = "Bill_Jobno_Validate";
                    ObjUBO.A12 = hdnbranch.Value;
                    if (Session["COMPANY_DETAILS_DS"] != null)
                    {
                        ds = (DataSet)Session["COMPANY_DETAILS_DS"];

                        DataTable ds3 = new DataTable();
                        ds3 = ds.Tables[2];
                        DataView view1 = ds3.DefaultView;
                        string a = ddlworking_Period.SelectedValue;
                        view1.RowFilter = "WORKING_PERIOD = '" + a + "'";
                        DataTable table1 = view1.ToTable();
                        s = BP.Select_Inv_dB(ObjUBO, table1.Rows[0]["SERVER_NAME"].ToString());
                    }
                    //s = BP.Select_Inv(ObjUBO);
                    if (s.Tables[0].Rows.Count > 0)
                    {
                        //if (s.Tables[1].Rows[0][0].ToString() == "EXIST")
                        //{
                        //    msgc = "Already invoice created for this job";    
                        //    //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Already invoice created for this job')", true);

                        //}
                        //    string confirm =
                        //    "if(confirm('Are you surely want to do this ??')) __doPostBack('', 'confirmed');";
                        //  string a=  ScriptManager.RegisterStartupScript(this, this.GetType(), "temp", confirm, true);
                        //    //if(confirm(
                        //if (hdconfirm.Value != "n")
                        //{
                        Rd_Imp_Exp.Enabled = false;
                        //if (cbList.Items.Count >= 1)
                        //{
                        //    msgc = "Do you want to add multi job";    

                        //    //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Do you want to add multi job')", true);
                        //}
                        //if (msgc != "")
                        //{
                        //    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('" + msgc + "')", true);
                        //}
                        cbList.DataSource = s.Tables[0];
                        cbList.DataTextField = "JOBNO_PS";
                        cbList.DataValueField = "JOBNO_PS";
                        cbList.DataBind();

                        txt_Jobno.Text = string.Empty;
                        Checked_Jobs();
                        Billing_Others();
                        if (Connection.Company_License().ToLower() == "erf00018")
                        {
                            ObjUBO.A4 = Get_Jobs();
                            ObjUBO.A11 = txt_Jobno.Text;
                            if (ddl_tr_mode.SelectedValue == "Air")
                            {
                                ObjUBO.A8 = "CANNOA";
                            }
                            else if (ddl_tr_mode.SelectedValue == "Sea")
                            {
                                ObjUBO.A8 = "CANNOS";
                            }
                            ObjUBO.A12 = hdnbranch.Value;
                            s = BP.Select_Inv(ObjUBO);
                            ddlQuot.DataSource = s.Tables[0];
                            ddlQuot.DataTextField = "CANNO";
                            ddlQuot.DataValueField = "CANNO";
                            ddlQuot.DataBind();
                        }
                        //}
                        //}
                    }
                    else
                    {
                        Alert_msg("Invalid Jobno !");
                    }
                }
                else { Alert_msg("Select Transport mode"); }
            }
            else
            {
                Alert_msg("Enter Ur Jobno", "txt_Jobno");
            }
        }

        }
          catch (Exception ex)
          {
              Connection.Error_Msg(ex.Message);
          }
    }

    private void Checked_Jobs()
    {
        string values = "", Val=string.Empty;
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
    public void state_local_national()
    {
        DataSet dss = new DataSet();
        ObjUBO.A2 = txt_Cus_name.Text;
        ObjUBO.A6 = ddl_state_name.SelectedValue;
        ObjUBO.A12 = Session["currentbranch"].ToString();
        ObjUBO.A2 = txt_Cus_name.Text;
        ObjUBO.A8 = "BILL_TO_COUNTRY";
        dss = BP.Select_Inv(ObjUBO);
        if (dss.Tables[0].Rows.Count > 0)
        {
            string state_name = ddl_state_name.SelectedValue.ToString();
            Regex myRegex = new Regex(@"--");
            var result = myRegex.Matches(state_name);
            if (dss.Tables[1].Rows.Count > 0)
            {
                if (dss.Tables[1].Rows[0][0].ToString() == "Domestic" )
                {
                    if (ddl_state_name.SelectedValue != "" && dss.Tables[0].Rows[0][0].ToString() != "")
                    {
                        if (result.Count > 0 && ddl_state_name.SelectedValue == dss.Tables[0].Rows[0][0].ToString())
                        {
                            Rd_Bill_Type.SelectedValue = "L";
                        }

                        else if (result.Count > 0 && ddl_state_name.SelectedValue != dss.Tables[0].Rows[0][0].ToString())
                        {
                            Rd_Bill_Type.SelectedValue = "O";
                        }

                        else
                        {
                            Rd_Bill_Type.ClearSelection();
                        }


                      

                    }
                    else
                    {
                        Rd_Bill_Type.ClearSelection();
                    }
                }

                else if (dss.Tables[1].Rows[0][0].ToString() == "OverSeas")
                {
                    if (result.Count == 0)
                    {
                        Rd_Bill_Type.SelectedValue = "IO";
                    }
                    else
                    {
                        Rd_Bill_Type.ClearSelection();
                    }



                  
                }
                else
                {
                    Rd_Bill_Type.ClearSelection();
                }
            }
            else if (Chk_Cha.Checked == true && ddlType.SelectedValue == "CLEARING")
            {
                if (ddl_state_name.SelectedValue != "" && dss.Tables[0].Rows[0][0].ToString() != "")
                {
                    if (result.Count > 0 && ddl_state_name.SelectedValue == dss.Tables[0].Rows[0][0].ToString())
                    {
                        Rd_Bill_Type.SelectedValue = "L";
                    }

                    else if (result.Count > 0 && ddl_state_name.SelectedValue != dss.Tables[0].Rows[0][0].ToString())
                    {
                        Rd_Bill_Type.SelectedValue = "O";
                    }

                    else
                    {
                        Rd_Bill_Type.ClearSelection();
                    }




                }
                else
                {
                    Rd_Bill_Type.ClearSelection();
                }
            }


        }


    }
    protected void btnloadData_Click(object sender, EventArgs e)
    {
        try
        {
            hdn_load.Value = "Job No Loaded";
            DataTable dt = new DataTable();
            string values = "";
            values = Get_Jobs();

            if (values != "" && values != string.Empty)
            {
                //if (ddlCus_name.SelectedItem.Text != string.Empty && ddlCus_name.SelectedItem.Text != "")
                //{
                //if (ddlbranch_No.SelectedValue != string.Empty && ddlbranch_No.SelectedValue != "")
                //{
                if (Chkacc.Checked == false )
                {
                    eFreightQuotation_Transactions Trans = new eFreightQuotation_Transactions();

                    DataSet dss = new DataSet();
                    if (ddlType.SelectedValue == "CLEARING" && Chk_Cha.Checked==true)
                    {
                        //Hdjobno.Value = values;
                        //Hd_I_E.Value = Rd_Imp_Exp.SelectedValue.ToString();
                        //DataSet dss = new DataSet();
                        Trans.TRANSPORT_MODE = ddl_tr_mode.SelectedValue.ToString();


                        Trans.jobno = values;
                        if (Rd_Imp_Exp.SelectedValue == "I")
                        {
                            Trans.flag = "Load_Invoice_Details_Imp";
                        }
                        else
                        {
                            Trans.flag = "Load_Invoice_Details_Exp";
                        }
                      
                        dss = Trans.CHA_IMP_EXP_INVOICE_Load();
                     
                        if (dss.Tables[0].Rows.Count > 0)
                        {
                            if (txt_Cus_name.Text == "" || txt_Party_Add.Text == "")
                            {
                                //ddl_state_name.SelectedValue = dss.Tables[0].Rows[0]["COMMERCIAL_TAX_STATENAME"].ToString();
                                hdnstate.Value = dss.Tables[0].Rows[0]["COMMERCIAL_TAX_STATENAME"].ToString();
                                ddl_state_name.Items.Clear();
                                ddl_state_name.Items.Insert(0, new ListItem(dss.Tables[0].Rows[0]["COMMERCIAL_TAX_STATENAME"].ToString(), dss.Tables[0].Rows[0]["COMMERCIAL_TAX_STATENAME"].ToString()));
                                ddl_state_name.SelectedIndex = 0;
                                txt_GSTN_Id.Text = dss.Tables[0].Rows[0]["COMMERCIAL_TAX_REGISTRATION_NO"].ToString();
                                txt_Cus_name.Text = dss.Tables[0].Rows[0]["Party_name"].ToString();
                                txt_Party_Add.Text = dss.Tables[0].Rows[0]["Party_Address"].ToString();
                                //Rd_Bill_Type.SelectedValue = dss.Tables[0].Rows[0]["Local_Other"].ToString();
                                if (Hidden_val.Value != "1")
                                {
                                    state_local_national();
                                }
                                if (Rd_Bill_Type.SelectedValue == "" && Rd_Bill_Type.SelectedValue == string.Empty)
                                {
                                    if (Rd_Bill_Type.SelectedValue == "L" || Rd_Bill_Type.SelectedValue == "O")
                                    {
                                        Rd_Tax_NonTax.SelectedValue = "T";
                                    }
                                    else if (Rd_Bill_Type.SelectedValue == "IO")
                                    {
                                        Rd_Tax_NonTax.SelectedValue = "N";
                                    }
                                }
                                //ddlshipment_type.Text = dss.Tables[0].Rows[0]["STYPE"].ToString();
                                //ddlcategory.SelectedValue = dss.Tables[0].Rows[0]["CATEGORY"].ToString();
                            }
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
                    else
                    {
                        ObjUBO.A1 = txtInvoiceNo.Text;
                        //ObjUBO.A2 = ddlCus_name.SelectedValue.ToString();
                        ObjUBO.A2 = txt_Cus_name.Text;
                        ObjUBO.A3 = ddlbranch_No.SelectedValue.ToString();
                        ObjUBO.A4 = values;
                        ObjUBO.A5 = ddl_tr_mode.SelectedValue.ToString();
                        ObjUBO.A6 = From_Quotation.Checked.ToString().ToUpper();
                        ObjUBO.A7 = Rd_Imp_Exp.SelectedValue.ToString();
                        ObjUBO.A12 = hdnbranch.Value;
                        ObjUBO.A8 = "Select_Imp_Exp_Job_Data";
                        ObjUBO.A10 = ddlType.SelectedValue.ToString();
                        ObjUBO.A11 = ddlQuot.SelectedValue.ToString();
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
                            dss = BP.Select_Inv_dB(ObjUBO, table1.Rows[0]["SERVER_NAME"].ToString());
                        }
                        //state_local_national();
                        ////dss = BP.Select_Inv_dB(ObjUBO, table1.Rows[0]["SERVER_NAME"].ToString());
                        if (dss.Tables[0].Rows.Count > 0)
                        {
                            if (txt_Cus_name.Text == "" || txt_Party_Add.Text == "")
                            {
                                //ddl_state_name.SelectedValue = dss.Tables[0].Rows[0]["COMMERCIAL_TAX_STATENAME"].ToString();
                                hdnstate.Value = dss.Tables[0].Rows[0]["COMMERCIAL_TAX_STATENAME"].ToString();
                                ddl_state_name.Items.Clear();
                                ddl_state_name.Items.Insert(0, new ListItem(dss.Tables[0].Rows[0]["COMMERCIAL_TAX_STATENAME"].ToString(), dss.Tables[0].Rows[0]["COMMERCIAL_TAX_STATENAME"].ToString()));
                                ddl_state_name.SelectedIndex = 0;
                                txt_GSTN_Id.Text = dss.Tables[0].Rows[0]["COMMERCIAL_TAX_REGISTRATION_NO"].ToString();
                                txt_Cus_name.Text = dss.Tables[0].Rows[0]["Party_name"].ToString();
                                txt_Party_Add.Text = dss.Tables[0].Rows[0]["Party_Address"].ToString();
                                ddlshipment_type.Text = dss.Tables[0].Rows[0]["STYPE"].ToString();
                                ddlcategory.SelectedValue = dss.Tables[0].Rows[0]["CATEGORY"].ToString();
                                if (Hidden_val.Value != "1")
                                {
                                    state_local_national();
                                }
                                if (Rd_Bill_Type.SelectedValue == "" && Rd_Bill_Type.SelectedValue == string.Empty)
                                {
                                    if (Rd_Bill_Type.SelectedValue == "L" || Rd_Bill_Type.SelectedValue == "O")
                                    {
                                        Rd_Tax_NonTax.SelectedValue = "T";
                                    }
                                    else if (Rd_Bill_Type.SelectedValue == "IO")
                                    {
                                        Rd_Tax_NonTax.SelectedValue = "N";
                                    }
                                }
                            }
                        }
                        //else
                        //{
                        //    ddl_state_name.SelectedValue = string.Empty;
                        //    txt_GSTN_Id.Text = string.Empty;
                        //    txt_Cus_name.Text = string.Empty;
                        //    txt_Party_Add.Text = string.Empty;
                        //}

                        //ObjUBO.A8 = "BILL_INV_LOAD_DATA";

                        //dss = BP.Select_Inv(ObjUBO);
                        //if (dss.Tables[0].Rows.Count > 0)
                        //{
                        //        ddlshipment_type.SelectedValue = dss.Tables[0].Rows[0]["SHIPMENT_TYPE"].ToString();
                        //        txtconttype.Text = dss.Tables[0].Rows[0]["CONTAINER_NO_TYPE"].ToString();

                        //}
                        //else
                        //{
                        //    ddlshipment_type.Items.Clear();
                        //    txtconttype.Text = string.Empty;
                        //}

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

                        //if (dss.Tables[1].Rows.Count > 0)
                        //{
                        //    gv_Gen_Item_II.DataSource = dss.Tables[1];
                        //    gv_Gen_Item_II.DataBind();
                        //}
                        //else
                        //{
                        //    gv_Gen_Item_II.DataSource = dt;
                        //    gv_Gen_Item_II.DataBind();
                        //}

                        if (dss.Tables[3].Rows.Count > 0)
                        {
                            gv_Chg_Details.DataSource = dss.Tables[3];
                            gv_Chg_Details.DataBind();

                            ViewState["CurrentTable"] = dss.Tables[3];
                        }
                        else
                        {
                            SetInitialRow();
                        }
                        if (dss.Tables[4].Rows.Count > 0)
                        {
                            gv_Gen_Annexure.DataSource = dss.Tables[4];
                            gv_Gen_Annexure.DataBind();
                        }
                        if (dss.Tables[5].Rows.Count > 0 && ddl_tr_mode.SelectedValue.ToString() == "Sea")
                        {
                            ddlshipment_type.SelectedValue = dss.Tables[5].Rows[0]["SHIPMENT_TYPE"].ToString();
                            txtconttype.Text = dss.Tables[5].Rows[0]["CONTAINER_NO_TYPE"].ToString();
                        }
                        else
                        {
                            // SetInitialRow();
                        }
                    }
                }
            }

        //else { Alert_msg("Select Branch No", "ddlbranch_No"); }
            //}
            //else
            //{
            //    Alert_msg("Select Ur Customer Name", "ddlCus_name");
            //}
            //}
            else
            {
                //state_local_national();
                Alert_msg("Select Ur Jobno !", "cbList");
            }
        }
        catch (Exception EX) { Alert_msg("Give Valid Charge");
        Page.ClientScript.RegisterStartupScript(this.GetType(), "Open", "javascript:inv('" + HDupdate_id.Value + "','" + HDupdate_IMP_EXP_id.Value + "','" + hdninvoice.Value + "','" + Hdnmode.Value + "','" + Hdntype.Value + "','" + hdnmisc.Value + "')", true);
        }
    }
    protected void gv_Gen_Annexure_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            if (e.Row.RowIndex == 0)
            {
                TextBox txt_Anx_Jobno = (e.Row.FindControl("txt_Anx_Jobno") as TextBox);
                TextBox txt_Ch_Amt_A = (e.Row.FindControl("txt_Ch_Amt_A") as TextBox);
                TextBox txt_Ch_Amt_B = (e.Row.FindControl("txt_Ch_Amt_B") as TextBox);
                TextBox txt_Ch_Amt_C = (e.Row.FindControl("txt_Ch_Amt_C") as TextBox);
                TextBox txt_Ch_Amt_D = (e.Row.FindControl("txt_Ch_Amt_D") as TextBox);
                TextBox txt_Ch_Amt_E = (e.Row.FindControl("txt_Ch_Amt_E") as TextBox);
                TextBox txt_Ch_Amt_F = (e.Row.FindControl("txt_Ch_Amt_F") as TextBox);
                TextBox txt_Ch_Amt_G = (e.Row.FindControl("txt_Ch_Amt_G") as TextBox);
                TextBox txt_Ch_Amt_H = (e.Row.FindControl("txt_Ch_Amt_H") as TextBox);
                TextBox txt_Ch_Amt_I = (e.Row.FindControl("txt_Ch_Amt_I") as TextBox);
                TextBox txt_Ch_Amt_J = (e.Row.FindControl("txt_Ch_Amt_J") as TextBox);
                TextBox txt_Ch_Amt_K = (e.Row.FindControl("txt_Ch_Amt_K") as TextBox);
                TextBox txt_Ch_Amt_L = (e.Row.FindControl("txt_Ch_Amt_L") as TextBox);
                TextBox txt_Ch_Amt_M = (e.Row.FindControl("txt_Ch_Amt_M") as TextBox);
                TextBox txt_Ch_Amt_N = (e.Row.FindControl("txt_Ch_Amt_N") as TextBox);
                TextBox txt_Ch_Amt_O = (e.Row.FindControl("txt_Ch_Amt_O") as TextBox);

                txt_Anx_Jobno.BackColor = System.Drawing.ColorTranslator.FromHtml("#008000");
                txt_Ch_Amt_A.BackColor = System.Drawing.ColorTranslator.FromHtml("#E5E4E2");
                txt_Ch_Amt_B.BackColor = System.Drawing.ColorTranslator.FromHtml("#E5E4E2");
                txt_Ch_Amt_C.BackColor = System.Drawing.ColorTranslator.FromHtml("#E5E4E2");
                txt_Ch_Amt_D.BackColor = System.Drawing.ColorTranslator.FromHtml("#E5E4E2");
                txt_Ch_Amt_E.BackColor = System.Drawing.ColorTranslator.FromHtml("#E5E4E2");

                txt_Ch_Amt_F.BackColor = System.Drawing.ColorTranslator.FromHtml("#E5E4E2");
                txt_Ch_Amt_G.BackColor = System.Drawing.ColorTranslator.FromHtml("#E5E4E2");
                txt_Ch_Amt_H.BackColor = System.Drawing.ColorTranslator.FromHtml("#E5E4E2");
                txt_Ch_Amt_I.BackColor = System.Drawing.ColorTranslator.FromHtml("#E5E4E2");
                txt_Ch_Amt_J.BackColor = System.Drawing.ColorTranslator.FromHtml("#E5E4E2");

                txt_Ch_Amt_K.BackColor = System.Drawing.ColorTranslator.FromHtml("#E5E4E2");
                txt_Ch_Amt_L.BackColor = System.Drawing.ColorTranslator.FromHtml("#E5E4E2");
                txt_Ch_Amt_M.BackColor = System.Drawing.ColorTranslator.FromHtml("#E5E4E2");
                txt_Ch_Amt_N.BackColor = System.Drawing.ColorTranslator.FromHtml("#E5E4E2");
                txt_Ch_Amt_O.BackColor = System.Drawing.ColorTranslator.FromHtml("#E5E4E2");

                txt_Ch_Amt_A.ForeColor = System.Drawing.ColorTranslator.FromHtml("#808080");
                txt_Ch_Amt_B.ForeColor = System.Drawing.ColorTranslator.FromHtml("#808080");
                txt_Ch_Amt_C.ForeColor = System.Drawing.ColorTranslator.FromHtml("#808080");
                txt_Ch_Amt_D.ForeColor = System.Drawing.ColorTranslator.FromHtml("#808080");
                txt_Ch_Amt_E.ForeColor = System.Drawing.ColorTranslator.FromHtml("#808080");

                txt_Ch_Amt_F.ForeColor = System.Drawing.ColorTranslator.FromHtml("#808080");
                txt_Ch_Amt_G.ForeColor = System.Drawing.ColorTranslator.FromHtml("#808080");
                txt_Ch_Amt_H.ForeColor = System.Drawing.ColorTranslator.FromHtml("#808080");
                txt_Ch_Amt_I.ForeColor = System.Drawing.ColorTranslator.FromHtml("#808080");
                txt_Ch_Amt_J.ForeColor = System.Drawing.ColorTranslator.FromHtml("#808080");

                txt_Ch_Amt_K.ForeColor = System.Drawing.ColorTranslator.FromHtml("#808080");
                txt_Ch_Amt_L.ForeColor = System.Drawing.ColorTranslator.FromHtml("#808080");
                txt_Ch_Amt_M.ForeColor = System.Drawing.ColorTranslator.FromHtml("#808080");
                txt_Ch_Amt_N.ForeColor = System.Drawing.ColorTranslator.FromHtml("#808080");
                txt_Ch_Amt_O.ForeColor = System.Drawing.ColorTranslator.FromHtml("#808080");
            }
            else
            {
                TextBox txt_Anx_Jobno = (e.Row.FindControl("txt_Anx_Jobno") as TextBox);
                txt_Anx_Jobno.BackColor = System.Drawing.ColorTranslator.FromHtml("#E5E4E2");
                txt_Anx_Jobno.Enabled = false;
            }
        }
    }
    private string Get_Jobs()
    {
       string values = "";
       //if (cbList.Items.Count > 1)
       //{
           foreach (ListItem objItem in cbList.Items)
           {
               values += objItem.Value + ",";
           }
        if(values.Contains(",,"))
           values = values.Remove(values.Length - 1);
       //}
       //else
       //{
       //    foreach (ListItem objItem in cbList.Items)
       //    {
       //        values += objItem.Value ;
       //    }
       //}
         return values;
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

    protected void btn_Remove_Click(object sender, EventArgs e)
    {
        hdn_remove.Value = "Bill Job No Removed";
        DataSet ds_R = new DataSet();
        string Jobs = string.Empty;
        Jobs = Get_Remove_Jobs();
        if (Jobs != string.Empty)
        {
         
           DataSet dss = new DataSet();
           ObjUBO.A4 = Jobs;
           ObjUBO.A7 = Rd_Imp_Exp.SelectedValue.ToString();
           ObjUBO.A8 = "Bill_Job_Remove";
           ObjUBO.A5 = ddl_tr_mode.SelectedValue;
           ObjUBO.A9 = Product_Item_I_details();
           ObjUBO.A10 = Product_Item_II_details();
           ObjUBO.A11 = Product_Annexure_details();
           ObjUBO.A12 = hdnbranch.Value;
           if (Session["COMPANY_DETAILS_DS"] != null)
           {
               ds = (DataSet)Session["COMPANY_DETAILS_DS"];

               DataTable ds3 = new DataTable();
               ds3 = ds.Tables[2];
               DataView view1 = ds3.DefaultView;
               string a = ddlworking_Period.SelectedValue;
               view1.RowFilter = "WORKING_PERIOD = '" + a + "'";
               DataTable table1 = view1.ToTable();
               ds_R = BP.Select_Inv_dB(ObjUBO, table1.Rows[0]["SERVER_NAME"].ToString());
           }
           //ds_R = BP.Select_Inv(ObjUBO);
           if (ds_R.Tables[0].Rows.Count > 0)
           {
               gv_Gen_Item_I.DataSource = ds_R.Tables[0];
               gv_Gen_Item_I.DataBind();
           }
           else
           {
               gv_Gen_Item_I.DataSource = dt;
               gv_Gen_Item_I.DataBind();
           }
           //if (ds_R.Tables[1].Rows.Count > 0)
           //{
           //    gv_Gen_Item_II.DataSource = ds_R.Tables[1];
           //    gv_Gen_Item_II.DataBind();
           //}
           //else
           //{
           //    gv_Gen_Item_II.DataSource = dt;
           //    gv_Gen_Item_II.DataBind();
           //}

           if (ds_R.Tables[2].Rows.Count > 0)
           {
               gv_Gen_Annexure.DataSource = ds_R.Tables[2];
               gv_Gen_Annexure.DataBind();
           }
           else
           {
               gv_Gen_Annexure.DataSource = dt;
               gv_Gen_Annexure.DataBind();
           }

           if (ds_R.Tables[3].Rows.Count > 0)
           {
               cbList.DataSource = ds_R.Tables[3];
               cbList.DataTextField = "JOBNO_PS";
               cbList.DataValueField = "JOBNO";
               cbList.DataBind();
               Billing_Others();
           }
           else
           {
               cbList.Items.Clear();
           }

        }
        else
        {
            Alert_msg("Select Ur Remove Job(s)!");
        }

    }
    private string Get_Remove_Jobs()
    {
        string values = "";
        foreach (ListItem objItem in cbList.Items)
        {
            if (objItem.Selected)
             {
                     values += objItem.Value + ",";
             }
        }
        return values;
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
        ddlbank_name.Items.Insert(0,"---Select Bank---");
        //string Company_id = Connection.COMPANYID();
        //ddlbank_name.Items.Clear();
        //ddlbank_name.Items.Add(new ListItem("", ""));
        //if (Company_id == "744")
        //{
        //    ddlbank_name.Items.Add(new ListItem("BENEFICIARY NAME : PARAMOUNT SHIPPING SERVICES PVT LTD, BANK NAME : IDBI BANK LTD,BRANCH NAME : PARRYS, CURRENT A/C NO : #907102000039127,IFS CODE NO. : IBKL0000907,SWIFT CODE : IBKLINBB005,MICR NO. : 600259007", "BENEFICIARY NAME : PARAMOUNT SHIPPING SERVICES PVT LTD,BANK NAME        : IDBI BANK LTD,BRANCH NAME      : PARRYS,CURRENT A/C NO   : #907102000039127,IFS CODE NO.     : IBKL0000907,SWIFT CODE       : IBKLINBB005,MICR NO.         : 600259007"));
        //    ddlbank_name.Items.Add(new ListItem("BENEFICIARY NAME : PARAMOUNT SHIPPING SERVICES PVT LTD,BANK NAME : AXIS BANK LTD,BRANCH NAME: GEORGE TOWN, CHENNAI,CURRENT A/C NO: #424010200003926,IFS CODE NO.: UTIB0000424,SWIFT CODE: AXISINBB424,MICR NO. : 600211016", "BENEFICIARY NAME : PARAMOUNT SHIPPING SERVICES PVT LTD,BANK NAME        : AXIS BANK LTD,BRANCH NAME      : GEORGE TOWN CHENNAI,CURRENT A/C NO   : #424010200003926,IFS CODE NO.     : UTIB0000424,SWIFT CODE       : AXISINBB424,MICR NO.         : 600211016"));
        //    ddlbank_name.Items.Add(new ListItem("BENEFICIARY NAME : PARAMOUNT SHIPPING SERVICES PVT LTD,BANK NAME: AXIS BANK LTD,BRANCH NAME: CBB, CHENNAI,CURRENT A/C NO : #006010300017471,IFS CODE NO. : UTIB0001165,SWIFT CODE : AXISINBBA01,MICR NO. : 600211036", "BENEFICIARY NAME : PARAMOUNT SHIPPING SERVICES PVT LTD,BANK NAME        : AXIS BANK LTD,BRANCH NAME      : CBB CHENNAI,CURRENT A/C NO   : #006010300017471,IFS CODE NO.     : UTIB0001165,SWIFT CODE       : AXISINBBA01,MICR NO.         : 600211036"));
        //}
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
        //    //BO.SHIPMENT_TYPE = ddlContainerTypes.SelectedValue;
        //    BO.Flag = "QUOT_NO";
        //    DS = QM.Quotation_Master_Select(BO);
        //    ddlQuot.Items.Clear();
        //    //ddlQuot.Items.Insert(0,new ListItem(string.Empty,string.Empty));

        //    ddlQuot.DataSource = DS.Tables[0];
        //    ddlQuot.DataTextField = "R_ID";
        //    ddlQuot.DataValueField = "R_ID";
        //    ddlQuot.DataBind();
        //}
        Jobno();
    }


    public void Jobno()
    {
        try
        {
            DataSet ds_R = new DataSet();
            ObjUBO.A8 = "LOAD_JOBNO_NOT_IN_INVOICE";
            ObjUBO.A7 = Rd_Imp_Exp.SelectedValue;
            ObjUBO.A10 = ddlType.SelectedValue;
            ObjUBO.A5 = ddl_tr_mode.SelectedValue;
            ObjUBO.A12 = hdnbranch.Value;
            if (Session["COMPANY_DETAILS_DS"] != null)
            {
                ds = (DataSet)Session["COMPANY_DETAILS_DS"];

                DataTable ds3 = new DataTable();
                ds3 = ds.Tables[2];
                DataView view1 = ds3.DefaultView;
                string a = ddlworking_Period.SelectedValue;
                view1.RowFilter = "WORKING_PERIOD = '" + a + "'";
                DataTable table1 = view1.ToTable();
                ds_R = BP.Select_Inv_dB(ObjUBO, table1.Rows[0]["SERVER_NAME"].ToString());
            }
            if (ds_R.Tables[0].Rows.Count > 0)
            {
                ddlJobno_N.DataTextField = "JOBNO_PS";
                ddlJobno_N.DataSource = ds_R;
                ddlJobno_N.DataBind();

                ddlJobno_N.Items.Insert(0, new System.Web.UI.WebControls.ListItem("", ""));
                ddlJobno_N.SelectedIndex = 0;
            }
        }
        catch (Exception ex)
        {
            Connection.Error_Msg(ex.Message);
        }
    }
    private void shipmenttype1()
    {
        if (ddl_tr_mode.SelectedValue == "Air")
        {
            ddlshipment_type.Enabled = false;

            txtconttype.Enabled = false;
        }
        if (ddl_tr_mode.SelectedValue == "Sea")
        {
            ddlshipment_type.Enabled = true;
            txtconttype.Enabled = true;
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
        hdn_deppart.Value = "Department Approved Changed";
        if (Chkdepartment.Checked == true) { lbldeptapp.Text = hdnuser.Value; btnprorpt.Visible = true; }
        //Printaccess();
    }
    protected void chkFrom_Quot_Changed(object sender, EventArgs e)
    {
        if (From_Quotation.Checked == true) { ddlQuot.Enabled = true; }
        else { ddlQuot.Enabled = false; }
    }
    protected void Chkacc_Changed(object sender, EventArgs e)
    {
        hdn_approved.Value = "Account Approved Changed";
        if (Chkdepartment.Checked == true) { lblaccapp.Text = hdnuser.Value; txtVerifiedBy.Text = hdnuser.Value; btnrpt.Visible = true; btnemail.Visible = true; }

        //Printaccess();
    }
    protected void ChkCancelInv_Changed(object sender, EventArgs e)
    {
        hdn_cancel.Value = "Cancel Invoice No";
        if (ChkCancelInv.Checked == true)
        {
            lblcancel.Text = hdnuser.Value;
            txtCancelReason.Visible = true;
        }
        else { txtCancelReason.Visible = false; txtCancelReason.Text = ""; }

        Chkdepartment.Checked = false;
        Chkacc.Checked = false;
        if (ChkCancelInv.Checked == true)
        { Chkdepartment.Enabled = false; Chkacc.Enabled = false; txtCancelReason.Visible = true; }
        else { Chkdepartment.Enabled = true; Chkacc.Enabled = true;
        txtCancelReason.Visible = false; txtCancelReason.Text = "";
        }
        if (ChkCancelInv.Checked == true)
        {

            btnUpdate.Enabled = true;

        }
        else { btnUpdate.Enabled = false; }
       
        Printaccess();
    }
    //------------------THIRU-----------------WORKING--PERIOD---START---------------//
    private void Working_Period_Load()
    {
        Company_Master Comp = new Company_Master();
        Comp.COMPANY_LICENSE = Connection.Company_License().ToLower();
        Comp.CLIENT_NAME = "";
        Comp.CONTACT_PERSON = "";
        Comp.MOBILE_NO = "";
        Comp.Ename = "select_Company_Login_Details";
        Comp.startRowIndex = "0";
        Comp.maximumRows = "0";
        DataSet ds = new DataSet();
        ds = Comp.RetrieveAll_Company_Details();
        if (ds.Tables.Count >= 2)
        {
            if (ds.Tables[2].Rows.Count > 0)
            {
                ddlworking_Period.DataSource = ds.Tables[2];
                ddlworking_Period.DataTextField = "WORKING_PERIOD";
                ddlworking_Period.DataValueField = "WORKING_PERIOD";
                ddlworking_Period.DataBind();
            }
        }
        ddlworking_Period.SelectedValue = Working_Period;
    }
    //------------------THIRU-----------------WORKING--PERIOD---END---------------//
    protected void btnJSON_Post_Click(object sender, EventArgs e)
    {
        try
        {
            DataSet ds = new DataSet();
            string job_type = Rd_Imp_Exp.SelectedItem.Text;
            //GST_BILL_IMPORT_INV_SELECT
            ObjUBO.A1 = Request.QueryString["Billinvno"].ToString();
            //ObjUBO.A1 = ViewState["Billinvno"].ToString();
            //ObjUBO.A4 = ViewState["Page"].ToString();
            ObjUBO.A4 = Request.QueryString["Page"];
            ObjUBO.A7 = Request.QueryString["IMP_EXP"].ToString();
            ObjUBO.A12 = hdnbranch.Value;
            ObjUBO.A8 = "BILLING_UPDATED_DATA_EINVOICE";
            ds = BP.Select_Inv(ObjUBO);
            string temp;
            //string TAX = Rd_Tax_NonTax.SelectedValue;

            //if ((Rdbbillseztype.SelectedValue == "Sez") && (Rd_Tax_NonTax.SelectedItem.Text == "Tax") && (job_type == "Imp"))
            //{
            //    TAX = "SEZWP";
            //}
            //else if (((Rdbbillseztype.SelectedValue == "Sez") && (Rd_Tax_NonTax.SelectedItem.Text == "Non-Tax") && (job_type == "Imp")))
            //{
            //    TAX = "SEZWOP";
            //}
            //else if ((Rd_Tax_NonTax.SelectedItem.Text == "Non-Tax") && (job_type == "Exp"))
            //{
            //    TAX = "EXPWOP";
            //}
            //else if ((Rd_Tax_NonTax.SelectedItem.Text == "Tax") && (job_type == "Exp"))
            //{
            //    TAX = "EXPWP";
            //}
            //else
            //{
            //    TAX = "B2B";
            //}


            //string IGSTOTRA = Rd_Bill_Type.SelectedValue;
            //if (IGSTOTRA == "L")
            //{
            //    IGSTOTRA = "Y";
            //}
            //else
            //{
            //    IGSTOTRA = "N";
            //}

            string IGSTOTRA ;
            if (ds.Tables[0].Rows[0]["LOCAL_OTHER"].ToString() == "L" && ds.Tables[3].Rows[0]["IGST_RATE"].ToString() != "0")
                IGSTOTRA = "Y";
            else
                IGSTOTRA = "N";
            string TAX = Rd_Tax_NonTax.SelectedValue;

            if ((Rdbbillseztype.SelectedValue == "Sez") && (Rd_Tax_NonTax.SelectedItem.Text == "Tax"))
            {
                TAX = "SEZWP";
            }
            else if ((Rdbbillseztype.SelectedValue == "Sez") && (Rd_Tax_NonTax.SelectedItem.Text == "Non-Tax"))
            {
                TAX = "SEZWOP";
            }
            else if ((Rd_Tax_NonTax.SelectedItem.Text == "Non-Tax") && (job_type == "Exp" || job_type == "Imp") && Rd_Bill_Type.SelectedValue == "IO")
            {
                TAX = "EXPWOP";
            }
            else if ((Rd_Tax_NonTax.SelectedItem.Text == "Tax") && (job_type == "Exp" || job_type == "Imp") && Rd_Bill_Type.SelectedValue == "IO")
            {
                TAX = "EXPWP";
            }
            else
            {
                TAX = "B2B";
            }
            errorFolder = Server.MapPath("~/Submission/" + Session["COMPANY_ID"].ToString() + "/" + Session["currentbranch"].ToString() + "/" + Session["currentuser"].ToString() + "/");

            //errorFolder = Server.MapPath("~/Submission/" + Session["COMPANY_ID"].ToString() + "/" + Session["currentbranch"].ToString() + "/" );

            string state = ddl_state_name.SelectedItem.Text;
            string sts1 = "", sts2 = "";
            if (state.Contains("--"))
            {
                sts1 = state.Substring(0, state.IndexOf("--"));
                sts2 = state.Substring(state.IndexOf("--"), state.Length - state.IndexOf("--"));

            }
            string add = txt_Party_Add.Text.Replace("\n", "").Replace("\r", "");

            if (Directory.Exists(errorFolder) == false)
            {
                Directory.CreateDirectory(errorFolder);
            }



            DirectoryInfo dir = new DirectoryInfo(errorFolder);
            foreach (FileInfo fi in dir.GetFiles())
            {

                fi.Delete();

            }

            errorFolder = errorFolder + "Inv_" + txtpino.Text.Replace("/", "") + ".JSON";
            //errorFolder = errorFolder + "Inv_" + ".JSON";
            string str = add;

            string lastsixDigits = str.Substring((str.Length - 6), 6);
            if (str.Length > 100)
            {
                temp = add.Substring(0, 95);
            }
            else
            {
                temp = add;
            }
            string sno = "";

            //using (

            StreamWriter sw = new StreamWriter(@errorFolder, true);

            //)
            //{

            if (ds.Tables[0].Rows.Count > 0)
            {
                // sw.WriteLine();  
                sw.WriteLine("{   " + "\"einvoiceusername\"" + ":" + "\"" + ds.Tables[5].Rows[0]["GSTNID"] + "\"" + ","); //hided for testing next lline given testing gstid given from mazework
                //sw.WriteLine("{   " + "\"einvoiceusername\"" + ":" + "\"" + "33AMBPG7773M014" + "\"" + ",");
                sw.WriteLine("      \"Invoice\"" + ":{");
                sw.WriteLine("       \"version\"" + ":" + "\"1.1\"" + ",");

                // Header files start //
                sw.WriteLine("       \"TranDtls\"" + ": {");
                sw.WriteLine("            \"TaxSch\"" + ":" + "\"GST" + "\"" + ",");
                sw.WriteLine("            \"SupTyp\"" + ":" + "\"" + TAX + "\"" + ",");
                sw.WriteLine("            \"RegRev\"" + ":" + "\"N" + "\"" + ",");
                sw.WriteLine("            \"IgstOnIntra\"" + ":" + "\"" + IGSTOTRA + "\"");

                sw.WriteLine("        },");

                // Header files End //

                sw.WriteLine("      \"DocDtls\"" + ": {");

                sw.WriteLine("            \"Typ\"" + ":" + "\"INV" + "\"" + ",");
                // sw.WriteLine("            \"TaxSch\"" + ":" + " \""+ " GST" + "\"" +",");
                sw.WriteLine("		     \"No\"" + ":" + "\"" + txtpino.Text + "\"" + ",");
                sw.WriteLine("		     \"Dt\"" + ":" + "\"" + txtDate.Text + "\"");
                sw.WriteLine("        },");
                sw.WriteLine("      \"SellerDtls\"" + ": {");
                //sw.WriteLine("		      \"SellerDtls\"" + ":" + " \{" + ",");
                sw.WriteLine("            \"Gstin\"" + ":" + "\"" + ds.Tables[5].Rows[0]["GSTNID"] + "\"" + ","); //hided for testing next lline given testing gstid given from mazework
                //sw.WriteLine("            \"Gstin\"" + ":" + "\"" + "33AMBPG7773M014" + "\"" + ",");
                //sw.WriteLine("            \"Gstin\"" + ":" + "\"" + "33AMBPG7773M014" + "\"" + ",");
                //sw.WriteLine("            \"Gstin\"" + ":" + "\"" + ds.Tables[5].Rows[0]["GSTNID"] + "\"" + ",");
                sw.WriteLine("            \"LglNm\"" + ":" + "\"" + ds.Tables[5].Rows[0]["COMPANY_NAME"] + "\"" + ",");
                if (ds.Tables[5].Rows[0]["ADDRESS"].ToString().Length > 100)
                {
                    sw.WriteLine("            \"Addr1\"" + ":" + "\"" + Regex.Replace(ds.Tables[5].Rows[0]["ADDRESS"].ToString().Substring(0, 100), @"\t|\n|\r", "") + "\"" + ",");
                    if (ds.Tables[5].Rows[0]["ADDRESS"].ToString().Length > 100 && ds.Tables[5].Rows[0]["ADDRESS"].ToString().Length < 200)
                    {
                        sw.WriteLine("            \"Addr2\"" + ":" + "\"" + Regex.Replace(ds.Tables[5].Rows[0]["ADDRESS"].ToString().Substring(101, ds.Tables[5].Rows[0]["ADDRESS"].ToString().Length), @"\t|\n|\r", "") + "\"" + ",");
                    }
                    else { sw.WriteLine("            \"Addr2\"" + ":" + "\"" + Regex.Replace(ds.Tables[5].Rows[0]["ADDRESS"].ToString().Substring(101, 100), @"\t|\n|\r", "") + "\"" + ","); }
                    if (ds.Tables[5].Rows[0]["ADDRESS"].ToString().Length > 200)
                        sw.WriteLine("            \"Addr3\"" + ":" + "\"" + Regex.Replace(ds.Tables[5].Rows[0]["ADDRESS"].ToString().Substring(201, ds.Tables[5].Rows[0]["ADDRESS"].ToString().Length), @"\t|\n|\r", "") + "\"" + ",");
                }
                else
                {
                    sw.WriteLine("            \"Addr1\"" + ":" + "\"" + Regex.Replace(ds.Tables[5].Rows[0]["ADDRESS"].ToString().Substring(0, ds.Tables[5].Rows[0]["ADDRESS"].ToString().Length), @"\t|\n|\r", "") + "\"" + ",");
                }
                sw.WriteLine("            \"Loc\"" + ":" + "\"" + ds.Tables[5].Rows[0]["STATENAME"] + "\"" + ",");
                sw.WriteLine("            \"Pin\"" + ":" + ds.Tables[5].Rows[0]["PINCODE"] + ",");

                sw.WriteLine("            \"Stcd\"" + ":" + "\"" + ds.Tables[5].Rows[0]["STATECODE"] + "\"" + ",");
                sw.WriteLine("            \"Ph\"" + ":" + "\"" + ds.Tables[5].Rows[0]["MOBILE_NO"] + "\"" + ",");
                sw.WriteLine("            \"Em\"" + ":" + "\"" + ds.Tables[5].Rows[0]["EMAILID"] + "\"");

                sw.WriteLine("	     },");
                string gstn;

                if (Rd_Bill_Type.SelectedValue == "IO")
                {
                    gstn = "URP";
                    sts1 = "96";
                    lastsixDigits = "999999";
                    sts2 = "Other Country";
                }
                else
                {
                    gstn = txt_GSTN_Id.Text;
                }

                sw.WriteLine("     \"BuyerDtls\"" + ":" + "{");
                //sw.WriteLine("		     \"Gstin\"" + ":" + "\"" + txt_GSTN_Id.Text + "\"" + ",");
                sw.WriteLine("		     \"Gstin\"" + ":" + "\"" + gstn + "\"" + ",");
                sw.WriteLine("		     \"LglNm\"" + ":" + "\"" + txt_Cus_name.Text + "\"" + ",");
                sw.WriteLine("		     \"Pos\"" + ":" + "\"" + sts1 + "\"" + ",");// need to check
                sw.WriteLine("		     \"Addr1\"" + ":" + "\"" + temp + "\"" + ",");
                //sw.WriteLine("		     \"Addr1\"" + ":" + "\"" + "PLOT 39,DOOR NO.1/1,LOGAIAH COLONY" + "\"" + ",");
                sw.WriteLine("		     \"Loc\"" + ":" + "\"" + sts2.Replace("-", "") + "\"" + ",");// need to check
                sw.WriteLine("           \"Pin\"" + ":" + lastsixDigits + ",");// need to check
                sw.WriteLine("		     \"Stcd\"" + ":" + "\"" + sts1 + "\"");// need to check
                sw.WriteLine("		 },");

                //item detais 

                if (ds.Tables[3].Rows.Count > 0)
                {
                    sw.WriteLine("      \"ItemList\"" + ": [{");

                    for (int i = 0; i < ds.Tables[3].Rows.Count; i++)
                    {

                        if ((i + 1).ToString().Length > 2)
                        {
                            sno = (i + 1).ToString();
                        }
                        else
                        {
                            sno = "0" + (i + 1).ToString();
                        }


                        sw.WriteLine("          \"SlNo\"" + ":" + "\"" + sno + "\"" + ",");
                        sw.WriteLine("          \"PrdDesc\"" + ":" + "\"" + ds.Tables[3].Rows[i]["CHARGE_NAME"] + "\"" + ",");
                        //sw.WriteLine("          \"IsServc\"" + ":" + "\"" + ds.Tables[0].Rows[i]["IsServc"] + "\"" + ",");
                        sw.WriteLine("          \"IsServc\"" + ":" + "\"" + "Y" + "\"" + ",");
                        //sw.WriteLine("          \"HsnCd\"" + ":" + "\"" + ds.Tables[0].Rows[i]["HSN_CODE"] + "\"" + ",");
                        sw.WriteLine("          \"HsnCd\"" + ":" + "\"" + ds.Tables[3].Rows[i]["SA_CODE"] + "\"" + ",");
                        //for ( i = 0; i < ds.Tables[2].Rows.Count; i++)
                        //{


                        sw.WriteLine("          \"Qty\"" + ":" + "\"" + ds.Tables[3].Rows[i]["QTYY"] + "\"" + ",");
                        //sw.WriteLine("          \"Qty\"" + ":" + "\"" + ds.Tables[3].Rows[i]["QTYY"] + "\"" + ",");
                        //}
                        sw.WriteLine("          \"FreeQty\"" + ":" + "0" + ",");
                        //sw.WriteLine("          \"Unit\"" + ":" + "\"" + ds.Tables[2].Rows[0]["PKGS_TYPE"] + "\"" + ",");//GIVEN O
                        sw.WriteLine("          \"Unit\"" + ":" + "\"" +"PAC"+ "\"" + ",");//GIVEN O
                        if (Rd_Bill_Type.SelectedValue == "IO")
                        {
                            sw.WriteLine("          \"UnitPrice\"" + ":" + Math.Round(Convert.ToDouble(ds.Tables[3].Rows[i]["UNIT_RATE"]) * Convert.ToDouble(ds.Tables[3].Rows[i]["EX_RATE"])) + ",");
                            sw.WriteLine("          \"TotAmt\"" + ":" +  Math.Round(Convert.ToDouble(ds.Tables[3].Rows[i]["Amount"]) * Convert.ToDouble(ds.Tables[3].Rows[i]["EX_RATE"])) + ",");
                            sw.WriteLine("          \"Discount\"" + ":" + "0" + ",");//GIVEN O
                            sw.WriteLine("          \"PreTaxVal\"" + ":" +   Math.Round(Convert.ToDouble(ds.Tables[3].Rows[i]["Igst_Amt"]) * Convert.ToDouble(ds.Tables[3].Rows[i]["EX_RATE"])) + ",");
                            sw.WriteLine("          \"AssAmt\"" + ":" +  Math.Round(Convert.ToDouble(ds.Tables[3].Rows[i]["Amount"]) * Convert.ToDouble(ds.Tables[3].Rows[i]["EX_RATE"])) + ",");//NEED TO CHECK ASSAMT NOT AVAILABLE IN DT TABEL
                            double val1;
                            string va = ds.Tables[3].Rows[i]["IGST_RATE"].ToString();
                            if (va == "0")
                            {
                                val1 = Convert.ToDouble(ds.Tables[3].Rows[i]["CGST_RATE"].ToString()) + Convert.ToDouble(ds.Tables[3].Rows[i]["SGST_RATE"].ToString());
                            }
                            else
                            {
                                val1 = Convert.ToDouble(ds.Tables[3].Rows[i]["IGST_RATE"].ToString());
                            }
                            sw.WriteLine("          \"GstRt\"" + ":" + val1 + ",");
                            //sw.WriteLine("          \"GstRt\"" + ":" +"9" + ",");
                            //sw.WriteLine("          \"IgstAmt\"" + ":" + ds.Tables[0].Rows[i]["Igst_Amt"] + ",");
                            //sw.WriteLine("          \"CgstAmt\"" + ":" + ds.Tables[0].Rows[i]["Cgst_Amt"] + ",");
                            //sw.WriteLine("          \"SgstAmt\"" + ":" + ds.Tables[0].Rows[i]["Sgst_Amt"] + ",");
                            sw.WriteLine("          \"IgstAmt\"" + ":" +  Math.Round(Convert.ToDouble(ds.Tables[3].Rows[i]["Igst_Amt"]) * Convert.ToDouble(ds.Tables[3].Rows[i]["EX_RATE"])) + ",");
                            sw.WriteLine("          \"CgstAmt\"" + ":" +  Math.Round(Convert.ToDouble(ds.Tables[3].Rows[i]["CGST_AMT"]) * Convert.ToDouble(ds.Tables[3].Rows[i]["EX_RATE"])) + ",");
                            sw.WriteLine("          \"SgstAmt\"" + ":" +  Math.Round(Convert.ToDouble(ds.Tables[3].Rows[i]["SGST_AMT"]) * Convert.ToDouble(ds.Tables[3].Rows[i]["EX_RATE"])) + ",");
                            sw.WriteLine("          \"CesRt\"" + ":" + "0" + ",");//GIVEN 0
                            sw.WriteLine("          \"CesAmt\"" + ":" + "0" + ",");//GIVEN 0
                            sw.WriteLine("          \"CesNonAdvlAmt\"" + ":" + "0" + ",");//GIVEN 0
                            sw.WriteLine("          \"StateCesRt\"" + ":" + "0" + ",");//GIVEN 0
                            sw.WriteLine("          \"StateCesAmt\"" + ":" + "0" + ",");//GIVEN 0
                            sw.WriteLine("          \"StateCesNonAdvlAmt\"" + ":" + "0" + ",");//GIVEN 0
                            sw.WriteLine("          \"OthChrg\"" + ":" + "0" + ",");
                            sw.WriteLine("          \"TotItemVal\"" + ":" + Math.Round(Convert.ToDouble(ds.Tables[3].Rows[i]["TOTAL_EACH_ITEM_VAL"]) * Convert.ToDouble(ds.Tables[3].Rows[i]["EX_RATE"])) + ",");// GRAND TOTAL GIVEN
                            sw.WriteLine("          \"OrdLineRef\"" + ":" + "\"" + "\"" + ",");
                            // sw.WriteLine("          \"OrgCntry\"" + ":" + "\"" + ds.Tables[0].Rows[i]["Destination"] + "\"" + ",");
                            sw.WriteLine("          \"OrgCntry\"" + ":" + "\"" + "IN" + "\"" + ",");
                            sw.WriteLine("          \"PrdSlNo\"" + ":" + "\"" + "0" + sno + "\"");
                        }
                        else
                        {
                            sw.WriteLine("          \"UnitPrice\"" + ":" + ds.Tables[3].Rows[i]["UNIT_RATE"] + ",");
                            sw.WriteLine("          \"TotAmt\"" + ":" + ds.Tables[3].Rows[i]["Amount"] + ",");
                            sw.WriteLine("          \"Discount\"" + ":" + "0" + ",");//GIVEN O
                            sw.WriteLine("          \"PreTaxVal\"" + ":" + ds.Tables[3].Rows[i]["Igst_Amt"] + ",");
                            sw.WriteLine("          \"AssAmt\"" + ":" + ds.Tables[3].Rows[i]["Amount"] + ",");//NEED TO CHECK ASSAMT NOT AVAILABLE IN DT TABEL
                            double val1;
                            string va = ds.Tables[3].Rows[i]["IGST_RATE"].ToString();
                            if (va == "0")
                            {
                                val1 = Convert.ToDouble(ds.Tables[3].Rows[i]["CGST_RATE"].ToString()) + Convert.ToDouble(ds.Tables[3].Rows[i]["SGST_RATE"].ToString());
                            }
                            else
                            {
                                val1 = Convert.ToDouble(ds.Tables[3].Rows[i]["IGST_RATE"].ToString());
                            }
                            sw.WriteLine("          \"GstRt\"" + ":" + val1 + ",");
                            //sw.WriteLine("          \"GstRt\"" + ":" +"9" + ",");
                            //sw.WriteLine("          \"IgstAmt\"" + ":" + ds.Tables[0].Rows[i]["Igst_Amt"] + ",");
                            //sw.WriteLine("          \"CgstAmt\"" + ":" + ds.Tables[0].Rows[i]["Cgst_Amt"] + ",");
                            //sw.WriteLine("          \"SgstAmt\"" + ":" + ds.Tables[0].Rows[i]["Sgst_Amt"] + ",");
                            sw.WriteLine("          \"IgstAmt\"" + ":" + ds.Tables[3].Rows[i]["Igst_Amt"] + ",");
                            sw.WriteLine("          \"CgstAmt\"" + ":" + ds.Tables[3].Rows[i]["CGST_AMT"] + ",");
                            sw.WriteLine("          \"SgstAmt\"" + ":" + ds.Tables[3].Rows[i]["SGST_AMT"] + ",");
                            sw.WriteLine("          \"CesRt\"" + ":" + "0" + ",");//GIVEN 0
                            sw.WriteLine("          \"CesAmt\"" + ":" + "0" + ",");//GIVEN 0
                            sw.WriteLine("          \"CesNonAdvlAmt\"" + ":" + "0" + ",");//GIVEN 0
                            sw.WriteLine("          \"StateCesRt\"" + ":" + "0" + ",");//GIVEN 0
                            sw.WriteLine("          \"StateCesAmt\"" + ":" + "0" + ",");//GIVEN 0
                            sw.WriteLine("          \"StateCesNonAdvlAmt\"" + ":" + "0" + ",");//GIVEN 0
                            sw.WriteLine("          \"OthChrg\"" + ":" + "0" + ",");
                            sw.WriteLine("          \"TotItemVal\"" + ":" + ds.Tables[3].Rows[i]["TOTAL_EACH_ITEM_VAL"] + ",");// GRAND TOTAL GIVEN
                            sw.WriteLine("          \"OrdLineRef\"" + ":" + "\"" + "\"" + ",");
                            // sw.WriteLine("          \"OrgCntry\"" + ":" + "\"" + ds.Tables[0].Rows[i]["Destination"] + "\"" + ",");
                            sw.WriteLine("          \"OrgCntry\"" + ":" + "\"" + "IN" + "\"" + ",");
                            sw.WriteLine("          \"PrdSlNo\"" + ":" + "\"" + "0" + sno + "\"");
                        }
                        if (i == ds.Tables[3].Rows.Count - 1)
                        {
                            sw.WriteLine("    }],");  //end of trnsprtEqmt
                        }
                        else
                        {
                            sw.WriteLine("}, {");

                        }
                    }


                }

                if (ds.Tables.Count > 1)
                {
                    if (ds.Tables[3].Rows.Count > 0)
                    {
                        if (Rd_Bill_Type.SelectedValue == "IO")
                        {
                            
sw.WriteLine("     \"ValDtls\"" + ": {");
                        sw.WriteLine("         \"AssVal\"" + ":" +  Math.Round(Convert.ToDouble(ds.Tables[3].Rows[i]["TOTAL_AMOUNT_WITHOUT_GST"]) * Convert.ToDouble(ds.Tables[3].Rows[i]["EX_RATE"])) + ",");//GIVEN 0
                        //sw.WriteLine("         \"CgstVal\"" + ":" + ds.Tables[1].Rows[i]["TOT_CGST_AMT"] + ",");
                        //sw.WriteLine("         \"SgstVal\"" + ":" + ds.Tables[1].Rows[i]["TOT_SGST_AMT"] + ",");
                        //sw.WriteLine("         \"IgstVal\"" + ":" + ds.Tables[1].Rows[i]["TOT_IGST_AMT"] + ",");
                        sw.WriteLine("         \"CgstVal\"" + ":" +  Math.Round(Convert.ToDouble(ds.Tables[3].Rows[i]["TOTAL_CGST_AMT"]) * Convert.ToDouble(ds.Tables[3].Rows[i]["EX_RATE"])) + ",");//GIVEN CGST AMT
                        sw.WriteLine("         \"SgstVal\"" + ":" + Math.Round(Convert.ToDouble(ds.Tables[3].Rows[i]["TOTAL_SGST_AMT"]) * Convert.ToDouble(ds.Tables[3].Rows[i]["EX_RATE"])) + ",");//GIVEN SGST AMT
                        sw.WriteLine("         \"IgstVal\"" + ":" +  Math.Round(Convert.ToDouble(ds.Tables[3].Rows[i]["TOTAL_IGST_AMT"]) * Convert.ToDouble(ds.Tables[3].Rows[i]["EX_RATE"])) + ",");//GIVEN IGST AMT
                        sw.WriteLine("         \"CesVal\"" + ":" + "0" + ",");//GIVEN 0
                        sw.WriteLine("         \"StCesVal\"" + ":" + "0" + ",");//GIVEN 0
                        sw.WriteLine("         \"Discount\"" + ":" + "0" + ",");//GIVEN 0
                        sw.WriteLine("         \"OthChrg\"" + ":" + "0" + ",");//GIVEN 0
                        sw.WriteLine("         \"RndOffAmt\"" + ":" + "0" + ",");//GIVEN 0
                        sw.WriteLine("         \"TotInvVal\"" + ":" + Math.Round(Convert.ToDouble(txt_grand_total.Text) * Convert.ToDouble(ds.Tables[3].Rows[i]["EX_RATE"])));//GIVEN GRAND TOTAL TEXTBOX
                        sw.WriteLine("	     }");
                        }
                        else
                        {
                            sw.WriteLine("     \"ValDtls\"" + ": {");
                            sw.WriteLine("         \"AssVal\"" + ":" + ds.Tables[3].Rows[i]["TOTAL_AMOUNT_WITHOUT_GST"] + ",");//GIVEN 0
                            //sw.WriteLine("         \"CgstVal\"" + ":" + ds.Tables[1].Rows[i]["TOT_CGST_AMT"] + ",");
                            //sw.WriteLine("         \"SgstVal\"" + ":" + ds.Tables[1].Rows[i]["TOT_SGST_AMT"] + ",");
                            //sw.WriteLine("         \"IgstVal\"" + ":" + ds.Tables[1].Rows[i]["TOT_IGST_AMT"] + ",");
                            sw.WriteLine("         \"CgstVal\"" + ":" + ds.Tables[3].Rows[i]["TOTAL_CGST_AMT"] + ",");//GIVEN CGST AMT
                            sw.WriteLine("         \"SgstVal\"" + ":" + ds.Tables[3].Rows[i]["TOTAL_SGST_AMT"] + ",");//GIVEN SGST AMT
                            sw.WriteLine("         \"IgstVal\"" + ":" + ds.Tables[3].Rows[i]["TOTAL_IGST_AMT"] + ",");//GIVEN IGST AMT
                            sw.WriteLine("         \"CesVal\"" + ":" + "0" + ",");//GIVEN 0
                            sw.WriteLine("         \"StCesVal\"" + ":" + "0" + ",");//GIVEN 0
                            sw.WriteLine("         \"Discount\"" + ":" + "0" + ",");//GIVEN 0
                            sw.WriteLine("         \"OthChrg\"" + ":" + "0" + ",");//GIVEN 0
                            sw.WriteLine("         \"RndOffAmt\"" + ":" + "0" + ",");//GIVEN 0
                            sw.WriteLine("         \"TotInvVal\"" + ":" + txt_grand_total.Text);//GIVEN GRAND TOTAL TEXTBOX
                            sw.WriteLine("	     }");
                        }
                    }

                }

                sw.WriteLine("    }");


                sw.WriteLine("}");  // End of Json

                Response.Write(sw.ToString());
                //Response.Flush();
                //Response.Close();
                HttpContext.Current.ApplicationInstance.CompleteRequest();

                ViewState["JSON_filename"] = "Inv_" + txtpino.Text + ".JSON";
                sw.Close();
                sw.Dispose();

                _invoiceJSONString = File.ReadAllText(errorFolder);

                //File.AppendAllText(errorFolder, sw.ToString());
            }
            sw.Close();
            sw.Dispose();
            SubmitInvoice();
            //}
        }

        catch (Exception ex)
        {
            Connection.Error_Msg(ex.Message);
        }

    }
    private void SubmitInvoice()
    {
        string AccessToken="", urla="";
        string branch = "";
        string result = "";
        if (Connection.Company_License().ToLower() == "erf00026")
        {
            if (hdnbranch.Value == "ACC, Chennai" || hdnbranch.Value == "CusHouse, Chennai")
            {
                urla = "http://einvoice.mazeworkssolutions.com/maze_eserver/public/api/einvoicegenerate";
                AccessToken = "eyJ0eXAiOiJKV1QiLCJhbGciOiJSUzI1NiIsImp0aSI6Ijk4ZmQxOTBhMmJmM2Y1Mjk3NDZlYWZjNDMxOWE0OGNmMDQ1MzFhZjk0ODNkZTUyODExNTQxOWM2YjkyNTJlYTg3NjZmN2E2OGZlYWViMWMxIn0.eyJhdWQiOiIxIiwianRpIjoiOThmZDE5MGEyYmYzZjUyOTc0NmVhZmM0MzE5YTQ4Y2YwNDUzMWFmOTQ4M2RlNTI4MTE1NDE5YzZiOTI1MmVhODc2NmY3YTY4ZmVhZWIxYzEiLCJpYXQiOjE2Nzg0MzY4NDYsIm5iZiI6MTY3ODQzNjg0NiwiZXhwIjo0ODM0MTEwNDQ1LCJzdWIiOiIzMCIsInNjb3BlcyI6W119.GwceolSLtLIyyRQ3bw3pfZqsv8P1OOxli2gnn2TpRA1uWj4xeX7ZdXBFe2-hpXb2MF-DNAbtUUP_F_yYz6to-VjAj7WfjV2q3EH57B5BAv6m35hMQVhl_SjlxCyvRbGp3eh6DFwWJIjIPGRxB7dg4iTRes_oADVB8oxS2PR6WOjPixriGgwm8JQPDcslB9stsA_NN_zGGLjmqgEuW9yP91FYOH8ulWjcL57HT-qAPbhyGZ4KgGy_Kt7jZBjW0pz905yiN4azfydV2VbxyJyrvhySMHy0gMwRaiy4PBv6agal_Y3Ex_PQF8WX-bwrFCpr3sAJXZZPX1Y7DwhVqAeBfwy2-EfrauBpkBs44qBRR5OAIzEYqEZLImGWQGk1lEyGZdn5sPXO0l1eg97ihhn357UaBpK9DH920_rYlWDnfsi14_QyYh3dVf3ehMzDJQd_N2GuXW4T-MjbaW4XKyEz1kri82oENujTAmVTEnlxj4ZQ5R65SoQx9lGPMPwpEVbdCC26g7apWoZa9TysTbQkYByWPoyRpmC2MHeRiefOMUxq-4_-38fC9G8ybhMvBcvOr-RuFLygCW98lhtqTB79hQ39jTRbjKD4SDOj8jIOyy5eOQvxPdn7lYhyimOqI93fzTJGwoZKJTr9RH4wzJs0VH6EI8_-Sc-JRnUoW-l_tMk";
            }
            else if (hdnbranch.Value == "ACC, Mumbai" || hdnbranch.Value == "CusHouse, Mumbai")
            {
                urla = "http://einvoice2.mazeworkssolutions.com/maze_eserver/api/einvoicegenerate";
                AccessToken = "eyJ0eXAiOiJKV1QiLCJhbGciOiJSUzI1NiJ9.eyJhdWQiOiIxIiwianRpIjoiYmYxODAwZDkwNWMyYjYyNTlkMjk3MmFhMTg5ZDNiYzY3YjQwOTAxYTEzYzg5ZjRmMTA1MDMxMzcwYzcxMWY5YmUzMmE3YjQzOTFmODc4MTciLCJpYXQiOjE3MTQ2NTAxODEuMTI0Mzg3LCJuYmYiOjE3MTQ2NTAxODEuMTI0Mzg5LCJleHAiOjE3NDYxODYxODEuMTIyNzM2LCJzdWIiOiIxMiIsInNjb3BlcyI6W119.AbtKtYD3XoU25BMeNXhnrzOfrCOIuOL2CzZNDTKaPh6JnM8FgMwxGZV35AxX9oZCS6iVNMFOM_4SGH3dr-sjQSUH9XPYiBLrEI7WERoLs8vaUNkU4h-F7jCxTr2nZx0HWK6hgRTmzXFYsR-dQuSgTl9BMst5lXmTknvD_VJGKRwOXJvJDKvVzSPRbrj3lxD_QGtBRcqtfDSv6fkdNQR92vsrePJnYDXL0Qn2bzN82EYWxZIQ12saN17ByzhNzvLwd_Pcpq89UTFHe0mDaGpJ8HKeQxKVqfaMnyMPgFZ6F4l16ML8CFny2oT5CW09zII0l2In51LsSK2gy0TQDJUdgltd5lCiCGMDkzA-gpHnKo0vnN0ryrNy7JXFsnXBhWdvVK1rXMfsEz2baj8OszNSTImXSaztk6l1PN4A023yNSpizDecOiQBF391xF0xPcji1MniFFfLxNJrcy6pPUFtnl6tMkss2sxfwnmlkyfELfJx1A-MCBjZAY09BLNB5Lm2r8bnsGU370AcEigScpB3CXBR-SAhdWXCJwJY3rCOtbSObgzAuyWDB7pT-Qn3wMg_WZ5GhIwVvqOAY9yaJmg4s2rL6D2HRFN2CMvpMtNs6gXDEgno20zZRCMX6vFxrby7NA6wMGgHHNq_yqSTHIM7X2n9b83xz7aMgfHhsLA4eQM";
            }
            else if (hdnbranch.Value == "ACC, Bangalore")
            {
                urla = "http://einvoice2.mazeworkssolutions.com/maze_eserver/api/einvoicegenerate";
                AccessToken = "eyJ0eXAiOiJKV1QiLCJhbGciOiJSUzI1NiJ9.eyJhdWQiOiIxIiwianRpIjoiYjA4MjI5OTYyOWQwY2FjYzc3MWU0Nzc0YzM5ZjJkNGY0YmI2MTE3ZjRlNmFjMzVjZDBlZDYxMzU0MGQzZmMxYWYxYzE2Njg5MGVhZmY0YWMiLCJpYXQiOjE3MTQ2NTAwNDcuNjU1MDQ4LCJuYmYiOjE3MTQ2NTAwNDcuNjU1MDQ5LCJleHAiOjE3NDYxODYwNDcuNjUyNCwic3ViIjoiMTMiLCJzY29wZXMiOltdfQ.VJ7y-B6OquEpWw1ZQ_hzjtJGaVvaCwGjmq_cvpH8mX8AmifS1_rSqkkkn12JalJkSlGR8VlJK2Eh5fRrQejGpbbifMWvZLAV5L-MJQuFmvLeWK_y_uV-7tVEFuM0wQMTBR-dzysB-nSnRAL1XwOrGWoSoYYPBQchrqS9iH61-K4KnTDIzBJIgkoHaz8vOpwvoBPPDd4oOabK0-x8C3cc1UKhUEW01lpvejLvbhMiuYWNk66rIVzGTl1kvaW9Ql0l7loHGYj0ec8ghynybFKl0wB8-XmVp5HUIoJQNtrjrrgVq1WDiP_e9DcFVLEEMwRdVJOck-x37pU-J_pyxTBnx5tZ_TB-BN77GdnnLDBnLX-QZFSIA8PKB2CeJEgFK9b4idS4K9grLbeWPhVktDcXQ1L52zyuZvK8UQC1TIv-Yud6coGeXu0IUlCL1Aa-wcA-RIZMcHJ_IxhB5ogjtIKjpNLg4O-gRYV35_WjZ8sVFvdPKlc1EysWUTfMJCRFJYXdOJDpECM33P5Ps-m_PUv1XU5Lc32mrF9RRrkKhMxSH4_A5aD_aKmOcuOakEOScln7uwX4eWgx_OIaApvjcW-7Sg6-z6kkeFpYL7JpkyCF0XqY3uinVxpv89W1Ekv6CwhLLojGkYhPOWwqHiIKLJwkjNhZZ2I7u2v9pYBgM6Lhcjo";
            }
        }
        else
        {
            urla = "http://einvoice.mazeworkssolutions.com/maze_eserver/public/api/einvoicegenerate";
            AccessToken = "eyJ0eXAiOiJKV1QiLCJhbGciOiJSUzI1NiIsImp0aSI6IjUzNTkyNzdkY2Y2MmQyYTE5YTkyNzU2ODQ3NTQxYmEzMzM4YTkzMTZmNzhlZmQxZGE5YzRiMWYwYTQ2MDkzYjQwY2NlY2YwYWQ5OTNiMjhhIn0.eyJhdWQiOiIxIiwianRpIjoiNTM1OTI3N2RjZjYyZDJhMTlhOTI3NTY4NDc1NDFiYTMzMzhhOTMxNmY3OGVmZDFkYTljNGIxZjBhNDYwOTNiNDBjY2VjZjBhZDk5M2IyOGEiLCJpYXQiOjE2NDg2MzM0NTYsIm5iZiI6MTY0ODYzMzQ1NiwiZXhwIjo0ODA0MzA3MDU2LCJzdWIiOiIxIiwic2NvcGVzIjpbXX0.uJpi9z5J_T97qJeNcoTFIsFCSMiq2JmZ1wfMMiVA5iQhOKkTC3b2TO56i8AGRjI__GQK2H5nX-28AJqFpxG0yoYb8sckd3IiQ-LUmk3ziSAUPSqSQvAsvbq5UhA66kPxQRnEnCN5o516uXT8tJdOssg3r5p7Z6xdA4Np3ysCerERhhrCUikxE0ppxyEQStXwVoLPAfFQshW5UpfqABE_uUMj07J6iWkR4xi1Hqn7sNs7Z_416Cs-Fw2HLg69TVzpKoqWJprgZUHQZSPMjIy5EX6M56m8SsscNPvhA39-QGUmS02c48scv8bvaE7o1RlpRNfFs5Ah72PUKQmqClvjwdkXxHzEnGf2ER6D1w22NSL5RQTjKpEquNNigTtT7-1XfDh30CD8dvlR_02MhHpLttnGikjhmfWWnkycRZAVw06sCEcFrkfY2DdrwhEUiT1jKxAY-UaTVEtT_82cC9EoCUMmRjw2UyzKAdDVzd5Kh_8HdAXorva_4Mfgr9CbMRKFagQm-_exGYLStiqExWfBzLnY8hJFg5gRrKZQr8De0eWH2AYRp0mzRrjPtS4SR9fUn2pric07YocCHDIVW2BoOW5lXtj8n_Yu1SpoqazwHQkgLmdsMHocq7gnzwwRU45UVtBjUV4R7rBgfKrS9B_I4eXSqeUwjGMlKrQ6vQrIffg";
        }
        string reqstr = _invoiceJSONString;
        string job_type = Rd_Imp_Exp.SelectedItem.Text;
        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(urla);
        request.Method = "POST";
        request.Headers.Add("Authorization", "Bearer " + AccessToken);
        request.ContentType = "application/json; charset=utf-8";

        using (var streamWriter = new StreamWriter(request.GetRequestStream()))
        {
            streamWriter.Write(reqstr);
            streamWriter.Flush();
            streamWriter.Close();
            //streamWriter.Dispose();
        }

        HttpWebResponse response = (HttpWebResponse)request.GetResponse();
        if (response.StatusCode == HttpStatusCode.OK)
        {
            using (var streamReader = new StreamReader(response.GetResponseStream()))
            {
                result = streamReader.ReadToEnd();


                //Console.WriteLine(result);
            }
        }




        string checkbrn = Connection.Get_Company_Type();
        //string output = task.ToString();

        if (result != "")
        {
            string[] Resut_Res;
            Resut_Res = result.Split(',');
            if (Resut_Res.Length > 9)
            {
                if (Resut_Res[0].ToString().ToLower().Contains("success"))
                {
                    DataTable dt = new DataTable();
                    DataSet dss = new DataSet();
                    DataSet dsc = new DataSet();
                    DataSet ds = new DataSet();
                    Job_Histroy jh = new Job_Histroy();

                    branch = Connection.Current_Branch();
                    //}

                    //errorFolder = Server.MapPath("~/Submission/" + Session["COMPANY_ID"].ToString() + "/" + grpname + "/GST_BILL_QRCODE/" + jh.JOB_TYPE + "/" + wkp + "/");

                    string errorFolder = "";

                    errorFolder = Server.MapPath("~/Submission/" + Session["COMPANY_ID"].ToString() + "/" + Connection.Current_Branch() + "/GST_BILL_QRCODE/" + job_type + "/");

                    if (Directory.Exists(errorFolder) == false)
                    {
                        Directory.CreateDirectory(errorFolder);
                    }


                    string QRImg = Resut_Res[8].Substring(Resut_Res[8].ToString().IndexOf(":") + 2, Resut_Res[8].Length - (Resut_Res[8].ToString().IndexOf(":") + 3));




                    //jh.JOB_ID = "";

                    //jh.INV_No = txtInvoiceNo.Text;

                    //string jobno_name = "";

                    //jobno_name = jh.JOB_TYPE + "_Invoice_" + txtpino.Text + "_" + DateTime.Now.Year.ToString() + ".png";

                    //string SQ_Path = errorFolder + jobno_name;

                    QRCodeGenerator qrGenerator = new QRCodeGenerator();
                    QRCodeData qrCodeData = qrGenerator.CreateQrCode(QRImg, QRCodeGenerator.ECCLevel.Q);
                    QRCode qrCode = new QRCode(qrCodeData);
                    Bitmap qrCodeImage = qrCode.GetGraphic(20);

                    //qrCodeImage.Save(errorFolder + txtpino.Text+".png");




                    //string url = HttpContext.Current.Request.Url.AbsoluteUri.ToLower();


                    //    // url = url.Substring(0, url.IndexOf("loginpage")) + SQ_Path.Substring(SQ_Path.IndexOf("Submission"), SQ_Path.Length - (SQ_Path.IndexOf("Submission")));
                    //url = url.Substring(0, url.IndexOf("localhost")) + url.Split('/')[2] + "/ERoyalFreight_NEW_Einvoice/" + SQ_Path.Substring(SQ_Path.IndexOf("Submission"), SQ_Path.Length - (SQ_Path.IndexOf("Submission")));


                    ////url=HttpContext.Current.Request.Url.AbsoluteUri.Substring(0,HttpContext.Current.Request.Url.AbsoluteUri.IndexOf("ERoyalimpex_Branchwise_Job_SingleWindow"))+SQ_Path.Substring(SQ_Path.IndexOf("Submission"), SQ_Path.Length - (SQ_Path.IndexOf("Submission")));
                    //url = url.Replace(@"\", @"/");


                    //var base64EncodedBytes = System.Convert.FromBase64String(SignedQRCodeImage);
                    //string jobdata = System.Text.Encoding.UTF8.GetString(base64EncodedBytes);

                    if (Directory.Exists(errorFolder) == false)
                    {
                        Directory.CreateDirectory(errorFolder);
                    }


                    jh.JOB_ID = "";
                    jh.JOB_TYPE = job_type;
                    jh.INV_No = txtpino.Text;


                    string jobno_name = "";
                    //InvoiceNo = InvoiceNo.Replace("/", "_");
                    jobno_name = job_type + "_Invoice_" + txtpino.Text + "_" + DateTime.Now.Year.ToString() + ".png";
                    //System.IO.File.WriteAllBytes(errorFolder + jobno_name);
                    string SQ_Path = errorFolder + jobno_name;

                    //http://demo.eroyalimpex.co.in/FlatFile_DSC/1/IMP/3002020.be  // http path


                    Uri a = Request.UrlReferrer;

                    //string url = HttpContext.Current.Request.Url.AbsoluteUri;

                    string url = a.ToString();
                    if (url.Contains("localhost"))
                    {
                        //url = url.Substring(0, url.IndexOf("localhost")) + url.Split('/')[2] + "/ERoyalFreight_NEW_Einvoice/" + SQ_Path.Substring(SQ_Path.IndexOf("Submission"), SQ_Path.Length - (SQ_Path.IndexOf("Submission")));

                        url = SQ_Path.Substring(SQ_Path.IndexOf("Submission"), SQ_Path.Length - (SQ_Path.IndexOf("Submission")));
                    }
                    else if (url.Contains("demo.eroyalfreight.com"))
                    {
                        url = "http://demo.eroyalfreight.com/" + SQ_Path.Substring(SQ_Path.IndexOf("Submission"), SQ_Path.Length - (SQ_Path.IndexOf("Submission")));
                    }
                    else if (url.Contains("https://eroyalfreight.com/"))
                    {
                        url = "https://eroyalfreight.com/" + SQ_Path.Substring(SQ_Path.IndexOf("Submission"), SQ_Path.Length - (SQ_Path.IndexOf("Submission")));
                    }
                    //else if (url.Contains("demo.eroyalimpex.net"))
                    //{
                    //    url = "http://demo.eroyalimpex.net/" + SQ_Path.Substring(SQ_Path.IndexOf("Submission"), SQ_Path.Length - (SQ_Path.IndexOf("Submission")));
                    //}
                    //else if (url.Contains("http://eroyalimpex.net/"))
                    //{
                    //    url = "http://eroyalimpex.net/" + SQ_Path.Substring(SQ_Path.IndexOf("Submission"), SQ_Path.Length - (SQ_Path.IndexOf("Submission")));
                    //}
                    //else if (url.Contains("demo.eroyalimpex.org"))
                    //{
                    //    url = "http://demo.eroyalimpex.org/" + SQ_Path.Substring(SQ_Path.IndexOf("Submission"), SQ_Path.Length - (SQ_Path.IndexOf("Submission")));
                    //}
                    //else if (url.Contains("http://eroyalimpex.org/"))
                    //{
                    //    url = "http://eroyalimpex.org/" + SQ_Path.Substring(SQ_Path.IndexOf("Submission"), SQ_Path.Length - (SQ_Path.IndexOf("Submission")));
                    //}

                    //url=HttpContext.Current.Request.Url.AbsoluteUri.Substring(0,HttpContext.Current.Request.Url.AbsoluteUri.IndexOf("ERoyalimpex_Branchwise_Job_SingleWindow"))+SQ_Path.Substring(SQ_Path.IndexOf("Submission"), SQ_Path.Length - (SQ_Path.IndexOf("Submission")));
                    url = url.Replace(@"\", @"/");
                    jh.IRN_No = Resut_Res[6].Substring(Resut_Res[6].ToString().IndexOf(":") + 2, Resut_Res[6].Length - (Resut_Res[6].ToString().IndexOf(":") + 3)); // "6e554c9cbd3e76c271b0ea0b9ba97fc060a9ef1ac79f3216f7"; // IRN_No;
                    jh.ACK_No = Resut_Res[1].Substring(Resut_Res[1].ToString().IndexOf(":") + 2, Resut_Res[1].Length - (Resut_Res[1].ToString().IndexOf(":") + 3)); // "AckNo";// AckNo;
                    jh.ACK_Dt = Resut_Res[5].Substring(Resut_Res[5].ToString().IndexOf(":") + 2, 10);// AckDt;
                    jh.SignedQRCodeImage = Resut_Res[7].Substring(Resut_Res[7].ToString().IndexOf(":") + 2, Resut_Res[7].Length - (Resut_Res[7].ToString().IndexOf(":") + 3));  // ""; // SignedQRCodeImage;
                    jh.SignedQRCode = Resut_Res[8].Substring(Resut_Res[8].ToString().IndexOf(":") + 2, Resut_Res[8].Length - (Resut_Res[8].ToString().IndexOf(":") + 3)); // SignedQRCode;
                    jh.STATUS = Resut_Res[9].Substring(Resut_Res[9].ToString().IndexOf(":") + 2, Resut_Res[9].Length - (Resut_Res[9].ToString().IndexOf(":") + 3)); //Status

                    jh.QRCode_ImgPath = url;
                    jh.InvFile_Path = "";

                    jh.FLAG = "Save";

                    jh.BRANCH_CODE = branch;
                    jh.JOB_TYPE = job_type;
                    jh.BRANCH_COMMON = branch;
                    jh.DESCRIPTION = "WebService";
                    jh.QRCode_PhysicalPath = SQ_Path;
                    jh.InvFile_PhysicalPath = "";
                    jh.YEAR = HttpContext.Current.Session["currentcomp_db"].ToString();
                    dss = jh.EINVOCIE_IRN_INSERT_UPDATE();

                    if (dss.Tables.Count > 0)
                    {
                        if (dss.Tables[0].Rows.Count > 0)
                        {
                            dt = dss.Tables[0];
                            Alert_msg(dss.Tables[0].Rows[0]["column2"].ToString() + " " + "Total Inv Value:" + txt_grand_total.Text);
                        }
                        else
                        {
                            dt.Rows.Add("Failure", "UnSuccess", "");
                        }
                    }
                    else
                    {
                        dt.Rows.Add("Failure", "UnSuccess", "");
                    }
                    btn_EInvoice.Visible = false;

                }

                else
                {
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "ErrorAlert", "alert('Not Success');", true);
                    //Alert_msg("Not Success");
                }
            }
            else
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "ErrorAlert", "alert('Not Success'" + result + "'');", true);
                //Alert_msg("Not Success" + result);
            }
        }
        else
        {
            Page.ClientScript.RegisterStartupScript(this.GetType(), "ErrorAlert", "alert('Not Success');", true);
            //Alert_msg("Not Success");
        }

    }
    protected void btn_Generate_EInvoice_Click(object sender, EventArgs e)
    {
        hdn_einvoice.Value = "Generated EInvoice";
        string job_type = Rd_Imp_Exp.SelectedItem.Text;
        //GST_BILL_IMPORT_INV_SELECT
        ObjUBO.A1 = Request.QueryString["Billinvno"].ToString();
        //ObjUBO.A1 = ViewState["Billinvno"].ToString();
        //ObjUBO.A4 = ViewState["Page"].ToString();
        ObjUBO.A4 = Request.QueryString["Page"];
        ObjUBO.A7 = Request.QueryString["IMP_EXP"].ToString();
        ObjUBO.A12 = hdnbranch.Value;
        ObjUBO.A8 = "BILLING_UPDATED_DATA_EINVOICE";
        DataSet ds = BP.Select_Inv(ObjUBO);
        string alertMessage = "";
        string Bill_No, tax_amt, Non_Tax, Total_Amt, cgst_Amt, sgst_amt, Igst_Amt, Total_Inv_val, Bill_To;
        if (ds.Tables[0].Rows.Count > 0)
        {
            if (Rd_Bill_Type.SelectedValue == "IO")
            {
                 Bill_No = ds.Tables[0].Rows[0]["ALT_INV_NO"].ToString();
                 tax_amt = (Convert.ToDouble(ds.Tables[0].Rows[0]["TAXABLE_AMT"]) * Convert.ToDouble(ds.Tables[3].Rows[0]["EX_RATE"])).ToString();
                 Non_Tax = (Convert.ToDouble(ds.Tables[0].Rows[0]["NON_TAXABLE_AMT"]) * Convert.ToDouble(ds.Tables[3].Rows[0]["EX_RATE"])).ToString();
                 Total_Amt = (Convert.ToDouble(ds.Tables[3].Rows[0]["TOTAL_AMOUNT_WITHOUT_GST"]) * Convert.ToDouble(ds.Tables[3].Rows[0]["EX_RATE"])).ToString();
                 cgst_Amt = (Convert.ToDouble(ds.Tables[3].Rows[0]["TOTAL_CGST_AMT"]) * Convert.ToDouble(ds.Tables[3].Rows[0]["EX_RATE"])).ToString();
                 sgst_amt = (Convert.ToDouble(ds.Tables[3].Rows[0]["TOTAL_SGST_AMT"]) * Convert.ToDouble(ds.Tables[3].Rows[0]["EX_RATE"])).ToString();
                 Igst_Amt = (Convert.ToDouble(ds.Tables[3].Rows[0]["TOTAL_IGST_AMT"]) * Convert.ToDouble(ds.Tables[3].Rows[0]["EX_RATE"])).ToString();
                Total_Inv_val = (Convert.ToDouble(ds.Tables[0].Rows[0]["GRAND_TOTAL"]) * Convert.ToDouble(ds.Tables[3].Rows[0]["EX_RATE"])).ToString();
                Bill_To = ds.Tables[0].Rows[0]["LOCAL_OTHER"].ToString();
            }
            else
            {
                 Bill_No = ds.Tables[0].Rows[0]["ALT_INV_NO"].ToString();
                 tax_amt = ds.Tables[0].Rows[0]["TAXABLE_AMT"].ToString();
                 Non_Tax = ds.Tables[0].Rows[0]["NON_TAXABLE_AMT"].ToString();
                 Total_Amt = ds.Tables[3].Rows[0]["TOTAL_AMOUNT_WITHOUT_GST"].ToString();
                 cgst_Amt = ds.Tables[3].Rows[0]["TOTAL_CGST_AMT"].ToString();
                 sgst_amt = ds.Tables[3].Rows[0]["TOTAL_SGST_AMT"].ToString();
                 Igst_Amt = ds.Tables[3].Rows[0]["TOTAL_IGST_AMT"].ToString();
                 Total_Inv_val = ds.Tables[0].Rows[0]["GRAND_TOTAL"].ToString();
                 Bill_To = ds.Tables[0].Rows[0]["LOCAL_OTHER"].ToString();
            }
            string lineWidth = "20%";
            alertMessage = "<table style='width:100%;height:80%'>" +
       "<tr><td style='width:30%;'>Invoice No</td><td style='text-align:left;'>: <span style='color: red;'>" + Bill_No +
       "</span>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span style='color: black;'>Bill To: </span><span style='color: #8E4585;'>" + Bill_To + "</span></td></tr>" +
       "<tr><td style='width:30%;'>Tax Amt</td><td style='text-align:left;'>: <span style='color: #191970;'>" + tax_amt + "</span></td></tr>" +
       "<tr><td style='width:30%;'>Non Tax Amt</td><td style='text-align:left;'>: <span style='color: #191970;'>" + Non_Tax + "</span></td></tr>" +
       "<tr><td colspan='2' style='text-align:left;'><hr style='width: " + lineWidth + "; margin: auto; margin-left: 150px;'/></td></tr>" +
       "<tr><td style='width:30%;'>Total Amt</td><td style='text-align:left;'>: <span style='color: #800020;'>" + Total_Amt + "</span></td></tr>" +
       "<tr><td colspan='2' style='text-align:center;'><hr style='width: " + lineWidth + "; margin: auto; margin-left:150px;'/></td></tr>" +
       "<tr><td style='width:30%;'>CGST</td><td style='text-align:left;'>: <span style='color: green;'>" + cgst_Amt + "</span></td></tr>" +
       "<tr><td style='width:30%;'>SGST</td><td style='text-align:left;'>: <span style='color: green;'>" + sgst_amt + "</span></td></tr>" +
       "<tr><td style='width:30%;'>IGST</td><td style='text-align:left;'>: <span style='color: #4B0082;'>" + Igst_Amt + "</span></td></tr>" +
       "<tr><td colspan='2' style='text-align:center;'><hr style='width: " + lineWidth + "; margin: auto; margin-left:150px;'/></td></tr>" +
       "<tr><td style='width:30%;'>Grand Total</td><td style='text-align:left;'>: <span style='color: blue;'>" + Total_Inv_val + "</span></td></tr>" +
       "<tr><td colspan='2' style='text-align:center;'><hr style='width: " + lineWidth + "; margin: auto; margin-left:150px;'/></td></tr>" +
       "</table>" +
       "<span>Do you want to proceed with generating the e-invoice?</span>";


        }
        Alert_Einvoice(alertMessage);

    }
    public void Alert_Einvoice(string msg)
    {
        int buttonWidth = 50, buttonHeight = 5, dialogWidth = 530, dialogHeight = 348;
        string buttonId = btnJSON_Post.ClientID;
        string prompt = "<script>" +
    "$(document).ready(function() {" +
    "$('<div></div>').dialog({" +
    "modal: true," +
    "title: 'E-Invoice Charges'," +
    "width: " + dialogWidth + "," +
    "height: " + dialogHeight + "," +
    "buttons: {" +
    "'OK': function() {" +
    "$(this).dialog('close');" +
    "setTimeout(function() {" +
    "document.getElementById('" + buttonId + "').click();" +
    "}, 0);" +
    "}," +
    "'Cancel': function() {" +
    "$(this).dialog('close');" +
    "}" +
    "}," +
    "open: function() {" +
    "$(this).html('" + msg.Replace("'", "\\'").Replace("\n", "\\n") + "');" +
    "$(this).dialog('option', 'position', { my: 'center', at: 'top', of: window });" +
    "$(this).css({'overflow': 'hidden'}); " +
    "}," +
    "create: function() {" +
    "$(this).parent().find('.ui-dialog-buttonpane button').css({" +
    "'width': '50px'," +
    "'height': '5px'," +
    "'padding': '1px 1px', " +
    "'margin': '2px' " +
    "});" +
    "}" +
    "});" +
    "});" +
    "</script>";

        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", prompt, false);
    }
    //[System.Web.Services.WebMethod]
    //public static DataSet Quot_select(string CUS, string MODE)
    //{
    //    try
    //    {
    //        GST_Quotation_Master QM = new GST_Quotation_Master();
    //        Billing_UserBO BO = new Billing_UserBO();
    //        DataSet DS = new DataSet();
    //        BO.Imp_Name = CUS;
    //        BO.MODE = MODE;
    //        BO.flag = "QUOT_NO";
    //        DS = QM.Quotation_Master_Select(BO);
    //        return DS; 
    //    }

    //    catch (Exception ex)
    //    {
    //        Connection.Error_Msg(ex.Message);

    //    }
        
        
    //}

}
