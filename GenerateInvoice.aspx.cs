using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Net.Mail;
using BussinessObject;

public partial class Accounts_GeneratrInvoice : ThemeClass
{
    eFreightExport_BookingForm Transact = new eFreightExport_BookingForm();
    Global_variables ObjUBO = new Global_variables();
    Generate_Invoice gen = new Generate_Invoice();
    Date_Time dt = new Date_Time();
    User_Creation user_Create = new User_Creation();
    AppSession aps = new AppSession();

    DataSet dtuser = new DataSet();
    DataSet ds = new DataSet();

    public int i, Screen_Id;
    public string currentuser, currentbranch, Working_Period, SELECT_BRANCH, COMPANY_ID;
    public string SCREEN_ID, PAGE_READ, PAGE_WRITE, PAGE_MODIFY, PAGE_DELETE, Is_Master_Id, Screen_IdNew;
    public string JOB_LOCK, JOB_RELEASE, JOB_USER;

    protected void Page_Load(object sender, EventArgs e)
    {
        this.Load += Page_Load;
        aps.checkSession();

        currentuser = Connection.Current_User();
        currentbranch = Connection.Current_Branch();
        Working_Period = Connection.WorkingPeriod();
        SELECT_BRANCH = Connection.Get_Company_Type();
        COMPANY_ID = Session["COMPANY_ID"].ToString();
        hdn_CompanyID.Value = COMPANY_ID;
        hdn_currentPage.Value = "General";

        Screen_Id = 7;
        Page_Rights(Screen_Id);

        //txtJobDate.Focus();
        txtLinerNo.Focus();
        txtConsignee.Focus();
        Session.Remove("EXBND_JOBID");

      
        if (!string.IsNullOrEmpty(Session["JOBIDexcel"] as string))
        {
            string aa = Session["JOBIDexcel"].ToString();

            System.Reflection.PropertyInfo isreadonly = typeof(System.Collections.Specialized.NameValueCollection)
                                             .GetProperty("IsReadOnly", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
            isreadonly.SetValue(HttpContext.Current.Request.QueryString, false, null);
           
            Request.QueryString.Remove("JOBID");
            Request.QueryString["JOBID"] = Session["JOBIDexcel"].ToString();
            Session.Remove("JOBIDexcel");
         
        }
      
        ////-------------------------------end----------------------------------------------------

        Session["SB_TYPE"] = ddlType.SelectedValue.ToString();
        Session["SB_No"] = "";

        if (ddlType.SelectedValue.ToString() == "Ex-Bond")
        {

            lnkbtn_Exbond.Style.Add("display", "none");
        }
        else
        {

            lnkbtn_Exbond.Style.Add("display", "none");
        }


        if (!this.IsPostBack)
        {
            if (ddl_BookingStatus.SelectedValue.ToString() != "REJECTED")
            {
                txtReason.Enabled = false;
            }
            else
            {
                txtReason.Enabled = true;

            }
        
           load_Tr_Mode();
            load_infoType();
            load_IncoTerms();
            load_FreightType();
            load_Type();
          
            load_AEOType();
        
            load_infoType1();
            txtJobDate.Text = dt.Today_Date();
            if (!string.IsNullOrEmpty(Request["Page"]))
            {
                string s = Request["Page"].ToString();
                if (Request["Page"] == "New")
                {
                   
                    hdn_New_Update.Value = "New";
                  hdn_Jobno.Value = "";
                    Session.Remove("TRANSPORT_MODE");
                    Session.Remove("AIR_JOBNO");
                    Session.Remove("AIR_JOBDATE");
                    Session.Remove("FILING_STATUS");
                    Session.Remove("COUNTRY_OF_ORIGIN");

                   
                    string TRANS_MODE = Session["CUSTOMHOUSE_TRANSPORT_MODE"].ToString();

                    if (TRANS_MODE == "A")
                    {
                        ddlTransportMode.SelectedValue = "Air";
                    }
                    else if (TRANS_MODE == "S")
                    {
                        ddlTransportMode.SelectedValue = "Sea";
                    }
                    else if (TRANS_MODE == "L")
                    {
                        ddlTransportMode.SelectedValue = "Land";
                    }

                    aps.Get_CustomeCodeBy_ModeofTransport(Session["currentbranch_code"].ToString(), "Air");
                    txtCustomsHouse.Text = Session["CUSTOMHOUSE_CODE"].ToString();

                    hdn_Customhouse_chk.Value = Set_Customs_House_Validation(txtCustomsHouse.Text);
                 

                   

                }
            }
            else
            {
                hdn_Jobno.Value = txtjobps.Text;

            }
         

            Edit_GeneralDetails();
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

        if (!IsPostBack)
        {
            txtid.Text = string.Empty;
           
        }

      



        hdsaveandclose.Value = "No";

    }

    private void Load_jobedit_Visible()
    {
        if (Session["COMPANY_LICENSE"] != null)
        {
          
        }
        btn_Tr_Mode_Change.Visible = true;
    }
  
    private void load_infoType()
    {
        ddl_BookingStatus.Items.Clear();
        ddl_BookingStatus.Items.Add(new ListItem("DRAFT", "DRAFT"));
        ddl_BookingStatus.Items.Add(new ListItem("CONFIRMED", "CONFIRMED"));
        ddl_BookingStatus.Items.Add(new ListItem("REJECTED", "REJECTED"));

    }
    private void load_Tr_Mode()
    {
        ddlTransportMode.Items.Clear();
        ddlTransportMode.Items.Add(new ListItem("Air", "Air"));
        //ddlTransportMode.Items.Add(new ListItem("Sea", "Sea"));
        //ddlTransportMode.Items.Add(new ListItem("Land", "Land"));
    }
    private void load_infoType1()
    {
        ddl_info_type.Items.Clear();
        ddl_info_type.Items.Add(new ListItem("Non DG", "Non DG"));
        ddl_info_type.Items.Add(new ListItem("DG", "DG"));
        ddl_info_type.Items.Add(new ListItem("OTHERS", "OTHERS"));

    }
    private void load_Type()
    {
        ddlType.Items.Clear();
        ddlType.Items.Add(new ListItem("FORWARDING", "FORWARDING"));
        ddlType.Items.Add(new ListItem("CLEARING", "CLEARING"));
        ddlType.Items.Add(new ListItem("CROSS_COUNTRY", "CROSS_COUNTRY"));
       
        ddlType.Items.Add(new ListItem("OTHERS", "OTHERS"));
    }
    private void load_FreightType()
    {
        ddlFreight.Items.Clear();
        ddlFreight.Items.Add(new ListItem("PREPAID", "PREPAID"));
        ddlFreight.Items.Add(new ListItem("COLLECT", "COLLECT"));

    }
    private void load_IncoTerms()
    {
        ddlIncoterms.Items.Clear();
        ddlIncoterms.Items.Add(new ListItem("FOB", "FOB"));
        ddlIncoterms.Items.Add(new ListItem("CIF", "CIF"));
        ddlIncoterms.Items.Add(new ListItem("CF", "CF"));
        ddlIncoterms.Items.Add(new ListItem("CI", "CI"));
        ddlIncoterms.Items.Add(new ListItem("CIP", "CIP"));
        ddlIncoterms.Items.Add(new ListItem("EXW", "EXW"));
        ddlIncoterms.Items.Add(new ListItem("FAS", "FAS"));
        ddlIncoterms.Items.Add(new ListItem("CFR", "CFR"));
        ddlIncoterms.Items.Add(new ListItem("CPT", "CPT"));
        ddlIncoterms.Items.Add(new ListItem("DAP", "DAP"));
        ddlIncoterms.Items.Add(new ListItem("DDP", "DDP"));
        ddlIncoterms.Items.Add(new ListItem("OTHER", "OTHER"));

    }
    private void load_AEOType()
    {
        ddl_AEOtypoe.Items.Clear();
        ddl_AEOtypoe.Items.Add(new ListItem("CLASS", "CLASS"));
        ddl_AEOtypoe.Items.Add(new ListItem("PACKAGING GROUP", "PACKAGING GROUP"));
        ddl_AEOtypoe.Items.Add(new ListItem("UN NO", "UN NO"));
        ddl_AEOtypoe.Items.Add(new ListItem("TYRE-III", "TYRE-III"));
    }
    
  
    public void Page_Show_Hide(int i)
    {
        bool is_page;
        if (i == 8)
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

        
    }
    public DataSet gridbind()
    {
        gen.JOBID = HiddenField1.Value;
        gen.JOBNO = "";
        gen.EXPORTERNAME = "";
        gen.TRANSPORT_MODE = hdfTransport_mode.Value;
        gen.CUSTOMS_HOUSE = "";
        gen.Ename = "select_Id";
        gen.WORKING_PERIOD = currentbranch;
        gen.TYPE = Request["F_Mode"].ToString();

        dtuser = gen.RetrieveAll_Generate_Details();
        return dtuser;
    }
   
    public void Edit_GeneralDetails()
    {

        if (Request.QueryString["JOBID"] != "" && Request.QueryString["JOBID"] != null)
        {
           

            Hdcountryoforgin_chk.Value = "Yes";
            Hdcountryofshipment_chk.Value = "Yes";

            Hdportoforgin_Chk.Value = "Yes";
            Hdportofshipment_chk.Value = "Yes";

            hd_joblck.Value = "No";
            hdn_Jobno.Value = "1";
            HiddenField1.Value = Request.QueryString["JOBID"].ToString();
            hdfTransport_mode.Value = Request.QueryString["TYPE"].ToString();
            if (hdfTransport_mode.Value == "IA") { spancontrol.InnerText = "Generate_Invoice(Import_Air)"; }
            if (hdfTransport_mode.Value == "IS") { spancontrol.InnerText = "Generate_Invoice(Import_Sea)"; }
            if (hdfTransport_mode.Value == "EA") { spancontrol.InnerText = "Generate_Invoice(Export_Air)"; }
            if (hdfTransport_mode.Value == "ES") { spancontrol.InnerText = "Generate_Invoice(Export_Sea)"; }

            int i = 0;
            gridbind();
            if (dtuser.Tables[0].Rows.Count > 0)
            {

                ddlType.AutoPostBack = false;
                ddlType.Enabled = false;
                //---------------------------OPERATION FIELDS----------------------------------------------//
                ddlType.SelectedValue = dtuser.Tables[0].Rows[0]["TYPE"].ToString();
                

                txtjobps.Text = dtuser.Tables[0].Rows[0]["JOBNO_PS"].ToString() == "" ? dtuser.Tables[0].Rows[0]["JOBNO"].ToString() : dtuser.Tables[0].Rows[0]["JOBNO_PS"].ToString();
              
                txtjobps.Text = txtjobps.Text;
                   txtJobNo.Text = dtuser.Tables[0].Rows[0]["JOBNO"].ToString();
                txt_M_jobps.Text = dtuser.Tables[0].Rows[0]["JOB_PREFIX"].ToString() == null ? string.Empty : dtuser.Tables[0].Rows[0]["JOB_PREFIX"].ToString();
                txt_M_jobsf.Text = dtuser.Tables[0].Rows[0]["JOB_SUFFIX"].ToString() == null ? string.Empty : dtuser.Tables[0].Rows[0]["JOB_SUFFIX"].ToString();

              
                txtJobDate.Text = dtuser.Tables[0].Rows[0]["JOBDATE"].ToString().Replace("01/01/1900", "");
             

                string mode = dtuser.Tables[0].Rows[0]["TRANSPORT_MODE"].ToString();

                //hdfTransport_mode.Value = mode;
                Session["Transport_Mode_Port"] = mode;

                ddlTransportMode.SelectedValue = mode;
              

                txtCustomsHouse.Text = dtuser.Tables[0].Rows[0]["CUSTOMS_HOUSE"].ToString();
                Session["IMP_Job_CUSTOMHOUSE_CODE"] = dtuser.Tables[0].Rows[0]["CUSTOMS_HOUSE"].ToString();

                txtPortOrigin.Text = dt.Replace(dtuser.Tables[0].Rows[0]["PORTOFORIGIN"].ToString());
                //Hdportoforgin_Chk.Value = Ser_Port_of_orgin(txtPortOrigin.Text, hdfTransport_mode.Value);

                string fil_status = dtuser.Tables[0].Rows[0]["SALES_BY"].ToString();
                txtSalesby.Text = dtuser.Tables[0].Rows[0]["SALES_BY"].ToString();
             
                txtFile_Ref_No.Text = dtuser.Tables[0].Rows[0]["FILE_REF_NO"].ToString();

                //JOB_USER = dtuser.Tables[0].Rows[0]["USERID"].ToString();

               
                Session["JOB_USER"] = JOB_USER;


                txtGrossWeight.Text = dtuser.Tables[0].Rows[0]["GROSSWEIGHT"].ToString();
                ddlTransportMode.SelectedValue = dtuser.Tables[0].Rows[0]["TRANSPORT_MODE"].ToString();
                ddlType.SelectedValue = dtuser.Tables[0].Rows[0]["TRANSPORT_MODE"].ToString();
                ddlIncoterms.SelectedValue = dtuser.Tables[0].Rows[0]["INCOTERMS"].ToString();
                ddlFreight.SelectedValue = dtuser.Tables[0].Rows[0]["FREIGHTTYPE"].ToString();
                txtJobDate.Text = dtuser.Tables[0].Rows[0]["JOBDATE"].ToString();

                txtSuppliername.Text = dtuser.Tables[0].Rows[0]["SUPLIER_NAME"].ToString();
                txtLinerNo.Text = dtuser.Tables[0].Rows[0]["LINER_NO"].ToString();
                txtJobhandledby.Text = dtuser.Tables[0].Rows[0]["JOB_HANDLEDBY"].ToString();
                txtLinerbookingdate.Text = dtuser.Tables[0].Rows[0]["LINER_BOOKING_DATE"].ToString();
                txtplaceofpickup.Text = dtuser.Tables[0].Rows[0]["PLACE_OF_PICKUP"].ToString();

                txtPortOrigin.Text = dtuser.Tables[0].Rows[0]["PORTOFORIGIN"].ToString();
                txtConsignee.Text = dtuser.Tables[0].Rows[0]["CONSIGNEENAME"].ToString();
                txtbranch.Text = dtuser.Tables[0].Rows[0]["BRANCHNO"].ToString();
                txtSuppliername.Text = dtuser.Tables[0].Rows[0]["SUPLIER_NAME"].ToString();
                txtCha.Text = dtuser.Tables[0].Rows[0]["CHA"].ToString();
                txtlinerName.Text = dtuser.Tables[0].Rows[0]["LINER_NAME"].ToString();
                txtPlaceofdelivery.Text = dtuser.Tables[0].Rows[0]["PORTDELIVERY"].ToString();
                txtPortofdelivery.Text = dtuser.Tables[0].Rows[0]["PORT_SHIPMENT"].ToString();
                ddl_BookingStatus.SelectedValue = dtuser.Tables[0].Rows[0]["OPERATION_JOB_STATUS"].ToString();
                if (ddl_BookingStatus.SelectedValue.ToString() != "REJECTED")
                {
                    txtReason.Enabled = false;
                }
                else
                {
                    txtReason.Enabled = true;

                }

                txtForwarder.Text = dtuser.Tables[0].Rows[0]["FORWARDERNAME"].ToString();
                txtClearingAgent.Text = dtuser.Tables[0].Rows[0]["CLEARINGAGENTNAME"].ToString();
                txtCoLoaderName.Text = dtuser.Tables[0].Rows[0]["COLOADERNAME"].ToString();
                txtOriginAgent.Text = dtuser.Tables[0].Rows[0]["ORIGIN_AGENT"].ToString();

                txtafs.Text = dtuser.Tables[0].Rows[0]["AFS"].ToString();
                txtChargeableType.Text = dtuser.Tables[0].Rows[0]["CHARGEABLEUNIT"].ToString();

                txttransporter.Text = dtuser.Tables[0].Rows[0]["TRANSPORTER_NAME"].ToString();
                txtotheragent.Text = dtuser.Tables[0].Rows[0]["OTHER_AGENT"].ToString();
                txtPackageType.Text = dtuser.Tables[0].Rows[0]["PACKAGE_TYPE"].ToString();
                ddl_AEOtypoe.SelectedValue = dtuser.Tables[0].Rows[0]["AEO_TYPE"].ToString();
                ddl_info_type.SelectedValue = dtuser.Tables[0].Rows[0]["AEO_CATEGORY"].ToString();
                txtPackages.Text = dtuser.Tables[0].Rows[0]["NO_PACKAGE"].ToString();
                txtQuantity.Text = dtuser.Tables[0].Rows[0]["NO_QUANTITY"].ToString();
                txtClearingAgent.Text = dtuser.Tables[0].Rows[0]["CLEARINGAGENTNAME"].ToString();
                txtNetWeightType.Text = dtuser.Tables[0].Rows[0]["QUANTITY_TYPE"].ToString();
                txtCoLoaderName.Text = dtuser.Tables[0].Rows[0]["COLOADERNAME"].ToString();

                txtReason.Text = dtuser.Tables[0].Rows[0]["REASON"].ToString();
                txtOtherSpecify.Text = dtuser.Tables[0].Rows[0]["IF_OTHERSPECIFY"].ToString();
                txtNetWeight.Text = dtuser.Tables[0].Rows[0]["NETWEIGHT"].ToString();
                txtChargable_Wgt.Text = dtuser.Tables[0].Rows[0]["CHARGEABLEWEIGHT"].ToString();
                ddl_AEOtypoe.SelectedValue = dtuser.Tables[0].Rows[0]["AEO_CATEGORY"].ToString();
                txt_Commodity.Text = dtuser.Tables[0].Rows[0]["CARGO_DISCRIPTION"].ToString();


                txtmarks.Text = dtuser.Tables[0].Rows[0]["MARKS_NOS"].ToString();
                txtCustomsHouse.Text = dtuser.Tables[0].Rows[0]["CUSTOMS_HOUSE"].ToString();

              

                //txtClearingAgent.Text = dtuser.Tables[0].Rows[0]["CLEARINGAGENTNAME"].ToString();





                ddlIncoterms.SelectedValue = dtuser.Tables[0].Rows[0]["INCOTERMS"].ToString();
                ddlFreight.SelectedValue = dtuser.Tables[0].Rows[0]["FREIGHTTYPE"].ToString();
                ViewState["UPDATED_ID"] = null;
                ViewState["UPDATED_ID"] = Request.QueryString["JOBID"].ToString();
                Session.Remove("TRANSPORT_MODE");
                Session.Remove("AIR_JOBNO");
                Session.Remove("AIR_JOBDATE");
                Session.Remove("FILING_STATUS");
                Session.Remove("COUNTRY_OF_ORIGIN");
                Session.Remove("IMPORTER_NAME");
                Session.Remove("EXP_AIR_COMMODITY");
                Session["TRANSPORT_MODE"] = ddlTransportMode.SelectedValue.ToString();
                Session["BOOKINGFORM_AIR_JOBNO"] = txtJobNo.Text;
             
                Session["BOOKINGFORM_AIR_JOBDATE"] = txtJobDate.Text;
                Session["TYPE"] = ddlType.SelectedValue;
              


            }

            if (Request.QueryString["Is_SEA"] != "" && Request.QueryString["Is_SEA"] != null)
            {
                hdnIsea.Value = "Y";
               
            }
            else
            {
                hdnIsea.Value = "N";
            }

            ddlType.SelectedValue = dtuser.Tables[0].Rows[0]["TYPE"].ToString();
        }
        else
        {

            if (!IsPostBack)
            {
               
                pageload_auto();
            }
        }
      
    }
    public void pageload_auto()
    {
        ddlType.AutoPostBack = true;
        ddlType.Enabled = true;
        AUTO_JOBNO();
       
        //hdfTransport_mode.Value = ddlTransportMode.SelectedValue.ToString();
        Session["Transport_Mode_Port"] = ddlTransportMode.SelectedValue.ToString();
        hdn_Jobno.Value = "";
        Session.Remove("TRANSPORT_MODE");
        Session.Remove("AIR_JOBNO");
        Session.Remove("AIR_JOBDATE");
        Session.Remove("FILING_STATUS");
        Session.Remove("COUNTRY_OF_ORIGIN");
       
        Session["commercialYN"] = "N";

    }

    
    public int insert_update(string jobid, string flag)
    {

        Transact.JOBID = jobid;
        Transact.BOOKINGNO = txtjobps.Text;
        Transact.JOBNO = txtJobNo.Text;
        Transact.JOBDATE = txtJobDate.Text;
        Transact.TRANSPORT_MODE = ddlTransportMode.SelectedValue.ToString();
        Transact.TYPE = ddlType.SelectedValue.ToString();
        Transact.INCOTERMS = ddlIncoterms.SelectedValue.ToString();
        Transact.FREIGHTTYPE = ddlFreight.SelectedValue.ToString();
        Transact.SALES_BY = txtSalesby.Text;
        Transact.LINER_NO=txtLinerNo.Text;
        Transact.JOB_HANDLEDBY= txtJobhandledby.Text;
        Transact.PORTDELIVERY = txtPlaceofdelivery.Text;
        Transact.PORT_SHIPMENT = txtPortofdelivery.Text;  
        Transact.LINER_BOOKING_DATE = txtLinerbookingdate.Text;
        Transact.PLACE_OF_PICKUP=txtplaceofpickup.Text;
        Transact.PORT_ORIGIN=txtPortOrigin.Text;
        Transact.CONSIGNEENAME=txtConsignee.Text;
        Transact.BRANCH_NO=txtbranch.Text;
        Transact.SUPLIER_NAME=txtSuppliername.Text;
        Transact.CHA=txtCha.Text;
        Transact.LINER_NAME=txtlinerName.Text;
        Transact.FORWARDERNAME=txtForwarder.Text;
        Transact.CLEARINGAGENTNAME=txtClearingAgent.Text;
        Transact.COLOADERNAME=txtCoLoaderName.Text;
        Transact.ORIGIN_AGENT=txtOriginAgent.Text;
        Transact.AFS=txtafs.Text;
        Transact.TRANSPORTER_NAME=txttransporter.Text;
        Transact.OTHER_AGENT = txtotheragent.Text;
        Transact.AEO_TYPE=ddl_AEOtypoe.SelectedValue;
        Transact.AEO_CATEGORY=ddl_info_type.SelectedValue;
        Transact.PACKAGE_TYPE=txtPackageType.Text;
        Transact.NO_PACKAGE=txtPackages.Text;
        Transact.NO_QUANTITY=txtQuantity.Text;
        Transact.QUANTITY_TYPE=txtNetWeightType.Text;
        Transact.NETWEIGHT=txtNetWeight.Text;
        Transact.CHARGEABLEWEIGHT=txtChargable_Wgt.Text;
        Transact.CARGO_DISCRIPTION=txt_Commodity.Text;
        Transact.MARKSNOS=txtmarks.Text;
        Transact.CUSTOMS_HOUSE = txtCustomsHouse.Text.ToUpper();
        Transact.USERID = currentuser;
        Transact.flag = flag;
        Transact.BRANCHCODE = currentbranch;
        Transact.WORKING_PERIOD = Working_Period;
        Transact.FILE_REF_NO = txtFile_Ref_No.Text.ToUpper();
        Transact.BRANCH_COMMON = SELECT_BRANCH;
        Transact.JOBNO_PS = txtjobps.Text;
        Transact.JOB_PREFIX = hdprefix.Value.ToString();
        Transact.JOB_SUFFIX = hdsuffix.Value.ToString();
        Transact.GROSSWEIGHT = txtGrossWeight.Text;
        Transact.CLEARINGAGENTNAME = txtClearingAgent.Text;
        Transact.IF_OTHERSPECIFY = txtOtherSpecify.Text;
        Transact.REASON = txtReason.Text;
        Transact.AEO_TYPE = ddl_AEOtypoe.SelectedValue.ToString();
        Transact.AEO_NO = ddl_BookingStatus.SelectedValue.ToString();
        Transact.CHARGEABLEUNIT = txtChargeableType.Text;
        Transact.OPERATION_STATUS = ddl_BookingStatus.SelectedValue.ToString();
        i = Transact.BOOKINGFORM_AIR_Planning_insertupdate();
       
        return i;
    }

    
    public void AUTO_JOBNO()
    {
        Transact.BRANCH_CODE = Connection.Current_Branch();
        Transact.WORKING_PERIOD = Working_Period;

        string types = "";
        if (ddlType.SelectedValue.ToString() == "CLEARING")
        {
            Transact.MODULETYPE = "CLEARING";
            types = "CLEARING_EXP_BOOKING_AIR_JOB_SELECT";
        }
        else if (ddlType.SelectedValue.ToString() == "FORWARDING")
        {
            Transact.MODULETYPE = "FORWARDING";
            types = "FORWARDING_EXP_BOOKING_AIR_JOB_SELECT";

        }
        else if (ddlType.SelectedValue.ToString() == "OTHERS")
        {
            Transact.MODULETYPE = "OTHERS";
            types = "OTHERS_EXP_BOOKING_AIR_JOB_SELECT";

        }
        txtJobNo.Text = txtjobps.Text = Convert.ToString(Transact.AIR_RetrieveID());
        JobNo_Prefix_Sufix(types);
        Session["Jobno_Excel"] = txtJobNo.Text;
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
        JobNo = txtjobps.Text;

        if (JobNo != string.Empty)
        {
            Jno = Convert.ToInt32(JobNo);
            if (Jno < 1000)
            {
                if (Jno > 100)
                {
                    JobNo = "0" + txtjobps.Text;
                }
                else if (Jno > 10)
                {
                    JobNo = "00" + txtjobps.Text;
                }
                else
                {
                    JobNo = "000" + txtjobps.Text;
                }
            }
        }

        if (Session["COMPANY_LICENSE"] != null)
        {

            if (dats.Tables[0].Rows.Count > 0)
            {
                if (dats.Tables[0].Rows[0][0].ToString() == string.Empty && dats.Tables[0].Rows[0][1].ToString() != string.Empty)
                {
                    txtjobps.Text = txtJobNo.Text + "/" + ((dats.Tables[0].Rows[0][1].ToString()).TrimStart()).TrimEnd();
                    hdprefix.Value = string.Empty;
                    hdsuffix.Value = ((dats.Tables[0].Rows[0][1].ToString()).TrimStart()).TrimEnd();
                }
                else if (dats.Tables[0].Rows[0][0].ToString() != string.Empty && dats.Tables[0].Rows[0][1].ToString() == string.Empty)
                {
                    txtjobps.Text = ((dats.Tables[0].Rows[0][0].ToString() + "/" + txtJobNo.Text).TrimEnd()).TrimStart();
                   
                    txtjobps.Text = ((dats.Tables[0].Rows[0][0].ToString() + "/" + JobNo).TrimEnd()).TrimStart();
                   

                    hdprefix.Value = ((dats.Tables[0].Rows[0][0].ToString()).TrimEnd()).TrimStart();
                    hdsuffix.Value = string.Empty;
                }
                else if (dats.Tables[0].Rows[0][0].ToString() != string.Empty && dats.Tables[0].Rows[0][1].ToString() != string.Empty)
                {
                    
                    hdprefix.Value = ((dats.Tables[0].Rows[0][0].ToString()).TrimEnd()).TrimStart();
                    hdsuffix.Value = ((dats.Tables[0].Rows[0][1].ToString()).TrimEnd()).TrimStart();
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
        
        btn_Tr_Mode_Change.Visible = false;

        hdn_Jobno.Value = "";
        txtid.Text = string.Empty;
        Session.Remove("TRANSPORT_MODE");
        Session.Remove("AIR_JOBNO");
        Session.Remove("AIR_JOBDATE");
        Session.Remove("FILING_STATUS");
        Session.Remove("COUNTRY_OF_ORIGIN");
        Session.Remove("IMPORTER_NAME");
        Transact.ResetFields(Page.Controls);
        txtJobDate.Text = dt.Today_Date();
        AUTO_JOBNO();
        ViewState["UPDATED_ID"] = null;
        hdn_Update_General.Value = "";
        ddlType.Enabled = true;
        string TRANS_MODE = "A";// Session["CUSTOMHOUSE_TRANSPORT_MODE"].ToString();
        string Category_mode = "";
        if (TRANS_MODE == "A")
            Category_mode = "Air";
        if (TRANS_MODE == "S")
            Category_mode = "Sea";
        if (TRANS_MODE == "L")
            Category_mode = "Land";
        aps.Get_CustomeCodeBy_ModeofTransport(Session["currentbranch_code"].ToString(), "Air");
        txtCustomsHouse.Text = Session["CUSTOMHOUSE_CODE"].ToString();
        hd_joblck.Value = "No";

       
    }
    protected void btngenerate_Click(object sender, EventArgs e)
    {
        ObjUBO.A1 = "";
        if (hdfTransport_mode.Value == "IA")
        {
            ObjUBO.A2 = "I";
            ObjUBO.A26 = "Air";
        }
        else if (hdfTransport_mode.Value == "IS")
        {
            ObjUBO.A2 = "I";
            ObjUBO.A26 = "Sea";
        }
        else if (hdfTransport_mode.Value == "EA")
        {
            ObjUBO.A2 = "E";
            ObjUBO.A26 = "Air";
        }
        else if (hdfTransport_mode.Value == "ES")
        {
            ObjUBO.A2 = "E";
            ObjUBO.A26 = "Sea";
        }
        ObjUBO.A3 = "L";
        ObjUBO.A4 = "";
        //ObjUBO.A5 = ddlbranch_No.SelectedValue.ToString();
        ObjUBO.A5 = hd_Brslno.Value;
        ObjUBO.A23 = "";

        ObjUBO.A6 = "";
        ObjUBO.A7 = txtjobps.Text;
        ObjUBO.A8 = "";
        ObjUBO.A9 = DateTime.Now.ToShortDateString();
        ObjUBO.A10 = "";
        ObjUBO.A11 = "N";
        ObjUBO.A12 = "";
        ObjUBO.A13 = "";
        ObjUBO.A14 = "";

        ObjUBO.A15 = "";
        ObjUBO.A16 = "";
        ObjUBO.A17 = "";
        ObjUBO.A18 = "";
        ObjUBO.A19 = "";
        ObjUBO.A20 = "";
        ObjUBO.A21 = "";
        ObjUBO.A22 = "";
        ObjUBO.A24 = "T";
        ObjUBO.A25 = "";


        ObjUBO.A27 = "";


        ObjUBO.A39 = "S";
        ObjUBO.A40 = "";
        ObjUBO.A41 = "";
        ObjUBO.A42 = "";
        ObjUBO.A43 = "";
        ObjUBO.A46 = "";
        ObjUBO.A49 = "";
        ObjUBO.A50 = "";
        ObjUBO.A44 = "N";
        ObjUBO.A45 = "N";
        ObjUBO.A47 = "Approved";
        ObjUBO.A48 = "";
        ObjUBO.A51 = "";
        ObjUBO.A52 = "";

        //ds= gen.Billing_Invoice_INS_UPD(ObjUBO);
        //if (ds.Tables[0].Rows.Count > 0)
        //{
        //Alert_msg("Invoice Generated Successfully"); 
        //btngenerate.Visible = false;
        //Session["Value"] = "G";
        //string str = "<script>window.parent.location.href = window.parent.location.href</script>";
        //Page.ClientScript.RegisterStartupScript(this.GetType(), "Script", str, false);
        string qstr = "Page=GI&IMP_EXP=" + ObjUBO.A2 + "&MODE=" + ObjUBO.A26 + "&CUS_BRANCH_NO=" + txtbranch.Text + "&CUS_BRANCH_NO_LOC=" + txtbranch.Text + "&JOBNO=" + ObjUBO.A7 + "&CUS_NAME=" + HttpUtility.UrlEncode( txtConsignee.Text) + "&PARTY_NAME=" + txtConsignee.Text + "&Type=" + ObjUBO.A43 + "";
        //Response.Redirect("../Accounts/GST_Imp_Exp_Invoice_Job.aspx?Page=" + ds.Tables[0].Rows[0][0].ToString() + "&IMP_EXP=" + ds.Tables[0].Rows[0][1].ToString() + "&Billinvno=" + ds.Tables[0].Rows[0][2].ToString());
        Response.Redirect("../Accounts/GST_Imp_Exp_Invoice_Job.aspx?" + qstr + "");
        //btngenerate.Attributes.Add("onclick", "javascript:open_GST_Billing_paramount_Update('" + ds.Tables[0].Rows[0][0].ToString() + "', '" + ds.Tables[0].Rows[0][1].ToString() + "', '" + ds.Tables[0].Rows[0][2].ToString() + "')");


        //}
    }
    protected void btn_Operation_update_Click(object sender, EventArgs e)
    {

        try
        {

            string f = "U";
            i = insert_update(HiddenField1.Value, f);

            if (i == 1)
            {
                hdn_Jobno.Value = "1";
                txtjobps.Focus();

                Session.Remove("TRANSPORT_MODE");
                Session.Remove("AIR_JOBNO");
                Session.Remove("AIR_JOBDATE");
                Session.Remove("FILING_STATUS");
                Session.Remove("COUNTRY_OF_ORIGIN");
                Session.Remove("IMPORTER_NAME");
                Session.Remove("JOB_USER");
                Session.Remove("EXP_AIR_COMMODITY");
              
                Session["BOOKINGFORM_AIR_JOBNO"] = txtJobNo.Text;
                Session["TYPE"] = ddlType.SelectedValue;
             
                Session["BOOKINGFORM_AIR_JOBDATE"] = txtJobDate.Text;
                
                Session["JOB_USER"] = currentuser;
                if (ViewState["UPDATED_ID"] != null)
                {
                    if (Request.QueryString["Is_SEA"] != "" && Request.QueryString["Is_SEA"] != null)
                    {

                        Alert_msg_Operation("Updated Successfully", "btnUpdate");
                    }
                    else
                    {
                      
                        string prompt = "<script>$(document).ready(function(){{jAlert('Updated Successfully', 'GENERAL', function (r) {var i = r + 'ok';if (i == 'trueok'){} else {}});}});</script>";
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", prompt, false);
                    }
                }
                else
                {
                    Alert_msg("Saved Successfully", "btnUpdate");
                }
            }
            else if (i == 3)
            {
                Alert_msg("File Ref. No Already Exist! Not Update", "btnUpdate");
            }
            else
            {
                hdn_Jobno.Value = "";
                Alert_msg("Job Not Saved", "btnUpdate");
            }
          
        }

        catch (Exception ex)
        {
            Connection.Error_Msg(ex.Message);
        }

    }

    public void Alert_msg_Operation(string msg, string focus)
    {
        string prompt = "<script>$(document).ready(function(){{jAlert('" + msg + "', 'GENERAL', function (r) {RefreshParent();});}});</script>";
        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", prompt, false);
    }
    protected void btnUpdate_Click(object sender, EventArgs e)
    {
        if (txtjobps.Text != "" && txtJobDate.Text != "")
        {
            try
            {
                if (Request.QueryString["Is_SEA"] != "" && Request.QueryString["Is_SEA"] != null)
                {
                    hdnOperation.Value = "1";
                    btn_Operation_update_Click(sender, e);
                    string prompt = "<script>$(document).ready(function(){{jConfirm('Do you want to Confirm?', 'GENERAL', function (r) {var i = r + 'ok';if (i == 'trueok'){document.getElementById('btn_Operation_update').click();} else {}});}});</script>";

                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", prompt, false);
                }

                else
                {
                    hdnOperation.Value = "0";
                    btn_Operation_update_Click(sender, e);

                }
            }
            catch (Exception ex)
            {
                Connection.Error_Msg(ex.Message);
            }
        }
        else
        {
            Alert_msg("Enter All Mandatory Field");
        }
    }

    protected void btnDelete_Click(object sender, EventArgs e)
    {
        try
        {
            Transact.JOBNO = txtjobps.Text;
            Transact.TYPE = ddlType.SelectedValue;
            Transact.EXPORTER_NAME = "0";
            Transact.TRANSPORT_MODE = "0";
            Transact.CUSTOMS_HOUSE = "0";
            Transact.Ename = "delete";
            Transact.WORKING_PERIOD = Connection.Current_Branch(); //Passing Barnch
          
            ds = Transact.Delete_BOOKINGFORM_Airjobs();


          
        }
        catch (Exception ex)
        {
            Connection.Error_Msg(ex.Message);
        }
    }
    protected void btnAuto_Jobno_Click(object sender, EventArgs e)
    {
        AUTO_JOBNO();
    }
  

   

    public void Alert_msg(string msg)
    {
        string prompt = "<script>$(document).ready(function(){{jAlert('" + msg + "', 'GENERAL', function (r) {});}});</script>";
        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", prompt, false);
    }

    public void Alert_msg(string msg, string focus)
    {
        string prompt = "<script>$(document).ready(function(){{jAlert('" + msg + "', 'GENERAL', function (r) {document.getElementById('" + focus + "').focus();});}});</script>";
        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", prompt, false);
    }
    public void Alert_msg_Save(string msg, string focus)
    {
        string prompt = "<script>$(document).ready(function(){{jAlert('" + msg + "', 'GENERAL', function (r) {Import_session('" + ddlTransportMode.SelectedValue.ToString() + "','" + txtjobps.Text + "','" + txtJobDate.Text + "','" + "" + "');document.getElementById('" + focus + "').focus();});}});</script>";
        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", prompt, false);
    }
    ///auto search
    ///

    [System.Web.Services.WebMethod]
    public static string GetImporter_2(string custid, string custid_2, string Adcode, string Jobno)
    {
        Auto_Search Auto = new Auto_Search();
        DataSet ds = new DataSet();
        DataSet dss = new DataSet();

        string STD_Type = "";
        string commercialYN = "";

        //if (!string.IsNullOrEmpty(HttpContext.Current.Session["IMP_AIR_commercialYN"].ToString()))
        //{
        //     commercialYN = HttpContext.Current.Session["IMP_AIR_commercialYN"].ToString();
        //}

        if (HttpContext.Current.Session["IMP_AIR_STANDARD_TYPE"] != null)
        {
            STD_Type = HttpContext.Current.Session["IMP_AIR_STANDARD_TYPE"].ToString();

            if (STD_Type == "Non_STD")
            {
                STD_Type = STD_Type;
            }
            else
            {
                STD_Type = "";
            }
        }

        Auto.Ename = "IMP_EXP_MASTER_IMPORTER_SEARCH_Slno";
        Auto.Enamesearch = custid;
        Auto.NSearch = custid_2;
        Auto.STANDARD_TYPE = STD_Type;

        ds = Auto.ERoyalmaste_RetrieveAll_AUTOCOMPLETE_SEARCH_STD_Type();
        if (ds.Tables[0].Rows.Count > 0)
        {

            string Address1 = ds.Tables[0].Rows[0]["IMP_EXP_ADD1"].ToString();
            string Address2 = ds.Tables[0].Rows[0]["IMP_EXP_ADD2"].ToString();
            string Address3 = ds.Tables[0].Rows[0]["IMP_EXP_ADD3"].ToString();
            string Address4 = ds.Tables[0].Rows[0]["IMP_EXP_ADD4"].ToString();
            string Address5 = ds.Tables[0].Rows[0]["IMP_EXP_PIN_CODE"].ToString();
            string IECNo = ds.Tables[0].Rows[0]["IMP_EXP_IECNO"].ToString();
            string BranchNo = ds.Tables[0].Rows[0]["IMP_EXP_BRSLNO"].ToString();
            string IMP_EXP_TYPE = ds.Tables[0].Rows[0]["IMP_EXP_TYPE"].ToString();
            string TIN_NO = ds.Tables[0].Rows[0]["TIN_NO"].ToString();
            string STATECODE = ds.Tables[0].Rows[0]["STATE_CODE"].ToString();
            string ADCODE_IEC_ADCODE = "";

            string M_COMMERCIAL_DETAILS_STATUS = ds.Tables[0].Rows[0]["M_COMMERCIAL_DETAILS_STATUS"].ToString();
            string M_COMMERCIAL_TAX_STATENAME = ds.Tables[0].Rows[0]["M_COMMERCIAL_TAX_STATENAME"].ToString();
            string M_COMMERCIAL_TAX_STATECODE = ds.Tables[0].Rows[0]["M_COMMERCIAL_TAX_STATECODE"].ToString();
            string M_COMMERCIAL_TAX_TYPE = ds.Tables[0].Rows[0]["M_COMMERCIAL_TAX_TYPE"].ToString();
            string M_COMMERCIAL_TAX_REGISTRATION_NO = ds.Tables[0].Rows[0]["M_COMMERCIAL_TAX_REGISTRATION_NO"].ToString();
            string GSTN_TYPE = ds.Tables[0].Rows[0]["GSTN_TYPE"].ToString();
            string GSTN_ID = ds.Tables[0].Rows[0]["GSTN_ID"].ToString();

            if (Adcode == "Update")
            {
                Auto.JobID = "";
                Auto.Jobno = Jobno;
                Auto.STANDARD_TYPE = STD_Type;
                Auto.Ename = "Select_Adcode_from_ImportJob";
                Auto.Enamesearch = custid;
                Auto.NSearch = custid_2;
                Auto.Working_Period = Connection.Current_Branch(); ;
                ds = Auto.ERoyalmaste_Retrieve_ADCode_Export();

                if (ds.Tables[0].Rows.Count > 0)
                {
                    if (custid == ds.Tables[0].Rows[0]["IMPORTER_NAME"].ToString())
                    {
                        ADCODE_IEC_ADCODE = ds.Tables[0].Rows[0]["ADCODE"].ToString();
                    }
                    else
                    {
                        if (IECNo != "")
                        {
                            Auto.Ename = "ADCODE_MASTER_SEARCH_branch_slno_wise";
                            Auto.Enamesearch = BranchNo;
                            Auto.NSearch = IECNo;
                            dss = Auto.ERoyalmaste_RetrieveAll_AUTOCOMPLETE_SEARCH();

                            if (dss.Tables[0].Rows.Count == 0)
                            {
                                Auto.Ename = "ADCODE_MASTER_SEARCH_branch_slno_wise";
                                Auto.Enamesearch = "";
                                Auto.NSearch = IECNo;
                                dss = Auto.ERoyalmaste_RetrieveAll_AUTOCOMPLETE_SEARCH();
                            }


                            if (dss.Tables[0].Rows.Count > 0)
                            {
                                if (dss.Tables[0].Rows.Count == 1)
                                {
                                    if (dss.Tables[0].Rows[0]["IMPORT_ADCODE"].ToString() != "")
                                    {
                                        ADCODE_IEC_ADCODE = dss.Tables[0].Rows[0]["IMPORT_ADCODE"].ToString();
                                    }
                                    else
                                    {
                                        ADCODE_IEC_ADCODE = dss.Tables[0].Rows[0]["ADCODE_IEC_ADCODE"].ToString();
                                    }
                                }
                            }
                        }
                    }
                }
                else
                {
                    if (IECNo != "")
                    {
                        Auto.Ename = "ADCODE_MASTER_SEARCH_branch_slno_wise";
                        Auto.Enamesearch = BranchNo;
                        Auto.NSearch = IECNo;
                        dss = Auto.ERoyalmaste_RetrieveAll_AUTOCOMPLETE_SEARCH();

                        if (dss.Tables[0].Rows.Count == 0)
                        {
                            Auto.Ename = "ADCODE_MASTER_SEARCH_branch_slno_wise";
                            Auto.Enamesearch = "";
                            Auto.NSearch = IECNo;
                            dss = Auto.ERoyalmaste_RetrieveAll_AUTOCOMPLETE_SEARCH();
                        }


                        if (dss.Tables[0].Rows.Count > 0)
                        {
                            if (dss.Tables[0].Rows.Count == 1)
                            {
                                if (dss.Tables[0].Rows[0]["IMPORT_ADCODE"].ToString() != "")
                                {
                                    ADCODE_IEC_ADCODE = dss.Tables[0].Rows[0]["IMPORT_ADCODE"].ToString();
                                }
                                else
                                {
                                    ADCODE_IEC_ADCODE = dss.Tables[0].Rows[0]["ADCODE_IEC_ADCODE"].ToString();
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                if (IECNo != "")
                {
                    Auto.Ename = "ADCODE_MASTER_SEARCH_branch_slno_wise";
                    Auto.Enamesearch = BranchNo;
                    Auto.NSearch = IECNo;
                    dss = Auto.ERoyalmaste_RetrieveAll_AUTOCOMPLETE_SEARCH();

                    if (dss.Tables[0].Rows.Count == 0)
                    {
                        Auto.Ename = "ADCODE_MASTER_SEARCH_branch_slno_wise";
                        Auto.Enamesearch = "";
                        Auto.NSearch = IECNo;
                        dss = Auto.ERoyalmaste_RetrieveAll_AUTOCOMPLETE_SEARCH();
                    }


                    if (dss.Tables[0].Rows.Count > 0)
                    {
                        if (dss.Tables[0].Rows.Count == 1)
                        {
                            if (dss.Tables[0].Rows[0]["IMPORT_ADCODE"].ToString() != "")
                            {
                                ADCODE_IEC_ADCODE = dss.Tables[0].Rows[0]["IMPORT_ADCODE"].ToString();
                            }
                            else
                            {
                                ADCODE_IEC_ADCODE = dss.Tables[0].Rows[0]["ADCODE_IEC_ADCODE"].ToString();
                            }
                        }
                    }
                }
            }

            custid = Address1 + "~~" + Address2 + "~~" + Address3 + "~~" + Address4 + "~~" + Address5 + "~~" + IECNo + "~~" + BranchNo + "~~" + IMP_EXP_TYPE + "~~" + TIN_NO + "~~" + STATECODE + "~~" + commercialYN + "~~" + ADCODE_IEC_ADCODE
                     + "~~" + M_COMMERCIAL_DETAILS_STATUS + "~~" + M_COMMERCIAL_TAX_STATENAME + "~~" + M_COMMERCIAL_TAX_STATECODE + "~~" + M_COMMERCIAL_TAX_REGISTRATION_NO + "~~" + M_COMMERCIAL_TAX_TYPE + "~~" + GSTN_TYPE + "~~" + GSTN_ID;
        }
        return custid;
    }

    [System.Web.Services.WebMethod]
    public static string GetCustom_House_Name(string custid)
    {
        Auto_Search Auto = new Auto_Search();
        DataSet ds = new DataSet();
        string name = "", rows = "", custom_name;
        custid = Auto.Country_Master_FromClientSide_Calling(custid, name = "CUSTOM_HOUSE_MASTER_CUSTOM_NAME", rows = "CUSTOMHOUSE_NAME");

        return custid + "~~" + name;
    }

    [System.Web.Services.WebMethod]
    public static string Ser_Country_of_orgin(string custid, string custid_2)
    {
        Auto_Search Auto = new Auto_Search();
        DataSet ds = new DataSet();

        Auto.Enamesearch = custid;
        Auto.Ename = "IMP_EXP_Country_of_orgin_Chk";
        ds = Auto.RetrieveAll_AUTOCOMPLETE_SEARCH();

        if (ds.Tables[0].Rows.Count > 0)
        {
            custid = "Yes";
        }
        else
        {
            custid = "No";
        }

        return custid;
    }

    [System.Web.Services.WebMethod]
    public static string Ser_Port_of_orgin(string custid, string custid_2)
    {
        Auto_Search Auto = new Auto_Search();
        DataSet ds = new DataSet();

        Auto.Enamesearch = custid;
        if (custid_2 == "Air")
        {
            Auto.Notn_No_Search = "3";
        }
        else if (custid_2 == "Sea" || custid_2 == "Land")
        {
            Auto.Notn_No_Search = "5";
        }
        Auto.Ename = "IMP_EXP_Port_of_orgin_Chk";
        ds = Auto.RetrieveAll_AUTOCOMPLETE_SEARCH();

        if (ds.Tables[0].Rows.Count > 0)
        {
            custid = "Yes";
        }
        else
        {
            string flag = "";
            if (custid_2 == "Air")
            {
                flag = "PORT_MASTER_USER_WISE_LIST_AIR_general";
            }
            else
            {
                flag = "PORT_MASTER_USER_WISE_LIST_SEA_LAND_general";
            }
            Auto.Ename = flag;
            Auto.Enamesearch = custid;
            Auto.NSearch = "";

            ds.Clear();
            //ds = Auto.ERoyalmaste_RetrieveAll_AUTOCOMPLETE_SEARCH();
            //if (ds.Tables[0].Rows.Count > 0)
            //{
            //    custid = "Yes";
            //}
            //else
            //{
            //    custid = "No";
            //}
        }

        return custid;
    }


    [System.Web.Services.WebMethod]
    public static string GetImporter_Branchwise(string custid, string custid1, string Adcode, string Jobno)
    {
        Auto_Search Auto = new Auto_Search();
        DataSet ds = new DataSet();
        DataSet dss = new DataSet();
        string STD_Type = "";
        string ADCODE_IEC_ADCODE = "";

        if (HttpContext.Current.Session["IMP_AIR_STANDARD_TYPE"] != null)
        {
            STD_Type = HttpContext.Current.Session["IMP_AIR_STANDARD_TYPE"].ToString();

            if (STD_Type == "Non_STD")
            {
                STD_Type = STD_Type;
            }
            else
            {
                STD_Type = "";
            }
        }

        Auto.Ename = "IMP_EXP_MASTER_IMPORTER_BRANCH_WISE";
        Auto.Enamesearch = custid;
        Auto.NSearch = custid1;
        Auto.STANDARD_TYPE = STD_Type;
        ds = Auto.ERoyalmaste_RetrieveAll_AUTOCOMPLETE_SEARCH_STD_Type();

        if (ds.Tables[0].Rows.Count > 0)
        {
            string IMPORTERNAME = ds.Tables[0].Rows[0]["IMP_EXP_NAME"].ToString();
            string Address1 = ds.Tables[0].Rows[0]["IMP_EXP_ADD1"].ToString();
            string Address2 = ds.Tables[0].Rows[0]["IMP_EXP_ADD2"].ToString();
            string Address3 = ds.Tables[0].Rows[0]["IMP_EXP_ADD3"].ToString();
            string Address4 = ds.Tables[0].Rows[0]["IMP_EXP_ADD4"].ToString();
            string Address5 = ds.Tables[0].Rows[0]["IMP_EXP_PIN_CODE"].ToString();
            string IECNo = ds.Tables[0].Rows[0]["IMP_EXP_IECNO"].ToString();
            string BranchNo = ds.Tables[0].Rows[0]["IMP_EXP_BRSLNO"].ToString();
            string IMP_EXP_TYPE = ds.Tables[0].Rows[0]["IMP_EXP_TYPE"].ToString();

            string M_COMMERCIAL_TAX_STATENAME = ds.Tables[0].Rows[0]["M_COMMERCIAL_TAX_STATENAME"].ToString();
            string M_COMMERCIAL_TAX_STATECODE = ds.Tables[0].Rows[0]["M_COMMERCIAL_TAX_STATECODE"].ToString();
            string M_COMMERCIAL_TAX_REGISTRATION_NO = ds.Tables[0].Rows[0]["M_COMMERCIAL_TAX_REGISTRATION_NO"].ToString();
            string M_COMMERCIAL_TAX_TYPE = ds.Tables[0].Rows[0]["M_COMMERCIAL_TAX_TYPE"].ToString();

            //if (IECNo != "")
            //{
            //    Auto.Ename = "ADCODE_MASTER_SEARCH_branch_slno_wise";
            //    Auto.Enamesearch = BranchNo;
            //    Auto.NSearch = IECNo;
            //    dss = Auto.ERoyalmaste_RetrieveAll_AUTOCOMPLETE_SEARCH();


            //    if (dss.Tables[0].Rows.Count > 0)
            //    {
            //        if (dss.Tables[0].Rows.Count == 1)
            //        {
            //            if (dss.Tables[0].Rows[0]["IMPORT_ADCODE"].ToString() != "")
            //            {
            //                ADCODE_IEC_ADCODE = dss.Tables[0].Rows[0]["IMPORT_ADCODE"].ToString();
            //            }
            //            else
            //            {
            //                ADCODE_IEC_ADCODE = dss.Tables[0].Rows[0]["ADCODE_IEC_ADCODE"].ToString();
            //            }
            //        }
            //    }

            if (Adcode == "Update")
            {
                Auto.JobID = "";
                Auto.Jobno = Jobno;
                Auto.STANDARD_TYPE = STD_Type;
                Auto.Ename = "Select_Adcode_from_ImportJob";
                Auto.Enamesearch = custid;
                Auto.NSearch = custid1;
                Auto.Working_Period = Connection.Current_Branch(); ;
                ds = Auto.ERoyalmaste_Retrieve_ADCode_Export();

                if (ds.Tables[0].Rows.Count > 0)
                {
                    if (IMPORTERNAME == ds.Tables[0].Rows[0]["IMPORTER_NAME"].ToString())
                    {
                        ADCODE_IEC_ADCODE = ds.Tables[0].Rows[0]["ADCODE"].ToString();
                    }
                    else
                    {
                        if (IECNo != "")
                        {
                            Auto.Ename = "ADCODE_MASTER_SEARCH_branch_slno_wise";
                            Auto.Enamesearch = BranchNo;
                            Auto.NSearch = IECNo;
                            dss = Auto.ERoyalmaste_RetrieveAll_AUTOCOMPLETE_SEARCH();

                            if (dss.Tables[0].Rows.Count == 0)
                            {
                                Auto.Ename = "ADCODE_MASTER_SEARCH_branch_slno_wise";
                                Auto.Enamesearch = "";
                                Auto.NSearch = IECNo;
                                dss = Auto.ERoyalmaste_RetrieveAll_AUTOCOMPLETE_SEARCH();
                            }


                            if (dss.Tables[0].Rows.Count > 0)
                            {
                                if (dss.Tables[0].Rows.Count == 1)
                                {
                                    if (dss.Tables[0].Rows[0]["IMPORT_ADCODE"].ToString() != "")
                                    {
                                        ADCODE_IEC_ADCODE = dss.Tables[0].Rows[0]["IMPORT_ADCODE"].ToString();
                                    }
                                    else
                                    {
                                        ADCODE_IEC_ADCODE = dss.Tables[0].Rows[0]["ADCODE_IEC_ADCODE"].ToString();
                                    }
                                }
                            }
                        }
                    }
                }
                else
                {
                    if (IECNo != "")
                    {
                        Auto.Ename = "ADCODE_MASTER_SEARCH_branch_slno_wise";
                        Auto.Enamesearch = BranchNo;
                        Auto.NSearch = IECNo;
                        dss = Auto.ERoyalmaste_RetrieveAll_AUTOCOMPLETE_SEARCH();

                        if (dss.Tables[0].Rows.Count == 0)
                        {
                            Auto.Ename = "ADCODE_MASTER_SEARCH_branch_slno_wise";
                            Auto.Enamesearch = "";
                            Auto.NSearch = IECNo;
                            dss = Auto.ERoyalmaste_RetrieveAll_AUTOCOMPLETE_SEARCH();
                        }


                        if (dss.Tables[0].Rows.Count > 0)
                        {
                            if (dss.Tables[0].Rows.Count == 1)
                            {
                                if (dss.Tables[0].Rows[0]["IMPORT_ADCODE"].ToString() != "")
                                {
                                    ADCODE_IEC_ADCODE = dss.Tables[0].Rows[0]["IMPORT_ADCODE"].ToString();
                                }
                                else
                                {
                                    ADCODE_IEC_ADCODE = dss.Tables[0].Rows[0]["ADCODE_IEC_ADCODE"].ToString();
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                if (IECNo != "")
                {
                    Auto.Ename = "ADCODE_MASTER_SEARCH_branch_slno_wise";
                    Auto.Enamesearch = BranchNo;
                    Auto.NSearch = IECNo;
                    dss = Auto.ERoyalmaste_RetrieveAll_AUTOCOMPLETE_SEARCH();

                    if (dss.Tables[0].Rows.Count == 0)
                    {
                        Auto.Ename = "ADCODE_MASTER_SEARCH_branch_slno_wise";
                        Auto.Enamesearch = "";
                        Auto.NSearch = IECNo;
                        dss = Auto.ERoyalmaste_RetrieveAll_AUTOCOMPLETE_SEARCH();
                    }


                    if (dss.Tables[0].Rows.Count > 0)
                    {
                        if (dss.Tables[0].Rows.Count == 1)
                        {
                            if (dss.Tables[0].Rows[0]["IMPORT_ADCODE"].ToString() != "")
                            {
                                ADCODE_IEC_ADCODE = dss.Tables[0].Rows[0]["IMPORT_ADCODE"].ToString();
                            }
                            else
                            {
                                ADCODE_IEC_ADCODE = dss.Tables[0].Rows[0]["ADCODE_IEC_ADCODE"].ToString();
                            }
                        }
                    }
                }
            }

            /*
            if (IECNo != "")
            {
                Auto.Ename = "ADCODE_MASTER_SEARCH_branch_slno_wise";
                Auto.Enamesearch = BranchNo;
                Auto.NSearch = IECNo;
                dss = Auto.ERoyalmaste_RetrieveAll_AUTOCOMPLETE_SEARCH();

                if (dss.Tables[0].Rows.Count == 0)
                {
                    Auto.Ename = "ADCODE_MASTER_SEARCH_branch_slno_wise";
                    Auto.Enamesearch = "";
                    Auto.NSearch = IECNo;
                    dss = Auto.ERoyalmaste_RetrieveAll_AUTOCOMPLETE_SEARCH();
                }


                if (dss.Tables[0].Rows.Count > 0)
                {
                    if (dss.Tables[0].Rows.Count == 1)
                    {
                        if (dss.Tables[0].Rows[0]["IMPORT_ADCODE"].ToString() != "")
                        {
                            ADCODE_IEC_ADCODE = dss.Tables[0].Rows[0]["IMPORT_ADCODE"].ToString();
                        }
                        else
                        {
                            ADCODE_IEC_ADCODE = dss.Tables[0].Rows[0]["ADCODE_IEC_ADCODE"].ToString();
                        }
                    }
                    //}
                    else
                    {
                        if (Adcode == "Update")
                        {
                            Auto.JobID = "";
                            Auto.Jobno = Jobno;
                            Auto.STANDARD_TYPE = STD_Type;
                            Auto.Ename = "Select_Adcode_from_ImportJob";
                            Auto.Enamesearch = custid;
                            Auto.NSearch = custid1;
                            Auto.Working_Period = Connection.Current_Branch();;
                            ds = Auto.ERoyalmaste_Retrieve_ADCode_Export();

                            if (ds.Tables[0].Rows.Count > 0)
                            {
                                ADCODE_IEC_ADCODE = ds.Tables[0].Rows[0]["ADCODE"].ToString();
                            }
                        }
                        else
                        {
                            ADCODE_IEC_ADCODE = "";
                        }
                    }
                }
            }
            */
            custid = Address1 + "~~" + Address2 + "~~" + Address3 + "~~" + Address4 + "~~" + Address5 + "~~" + IECNo + "~~" + BranchNo + "~~" + IMP_EXP_TYPE
               + "~~" + M_COMMERCIAL_TAX_STATENAME
               + "~~" + M_COMMERCIAL_TAX_STATECODE
               + "~~" + M_COMMERCIAL_TAX_REGISTRATION_NO
               + "~~" + M_COMMERCIAL_TAX_TYPE
               + "~~" + ADCODE_IEC_ADCODE
                ;
        }
        return custid;
    }

    protected void JobRelease_Click(object sender, EventArgs e)
    {
        Joblock JL = new Joblock();
        JL.Jobtype = "Import";
        JL.CURRENT_USER = currentuser;
        JL.JOBNO = Session["IMP_AIR_IMPORT_JOBNO"].ToString();
        JL.Commorn_or_Branchwise = SELECT_BRANCH;
        JL.Select_or_Update = "Update";
        JL.Job_Release_data();
        if (JL.result == 1)
        {
            //job_release.Visible = false;
        }
    }
    protected void btnok_Click(object sender, EventArgs e)
    {

    }

    public void Get_Client_Default_Settings()
    {
        Default_Settings Default = new Default_Settings();
        Default.STANDARD_TYPE = "";
        Default.USER_ID = currentuser;
        Default.BRANCH_CODE = currentbranch;
        Default.YEAR = "";
        Default.WORKING_PERIOD = Working_Period;
        Default.BRANCH_COMMON = SELECT_BRANCH;
        Default.flag = "Get_Details";
        ds = Default.Client_DefaultSettings_Insert_Update();

        if (ds.Tables[0].Rows.Count > 0)
        {
            if (ds.Tables[0].Rows.Count > 0)
            {
                ViewState["STANDARD_TYPE"] = ds.Tables[0].Rows[0]["STANDARD_TYPE"].ToString();
                Hdn_STANDARD_TYPE.Value = ViewState["STANDARD_TYPE"].ToString();
                Session["IMP_AIR_STANDARD_TYPE"] = Hdn_STANDARD_TYPE.Value;
            }
            else
            {
                Hdn_STANDARD_TYPE.Value = "STD";
                Session["IMP_AIR_STANDARD_TYPE"] = Hdn_STANDARD_TYPE.Value;
            }
        }
    }



    [System.Web.Services.WebMethod]
    public static string Set_State_Name_Validation(string custid)
    {
        Auto_Search Auto = new Auto_Search();
        DataSet ds = new DataSet();

        Auto.Enamesearch = custid;
        Auto.Ename = "State_Name_Validate";
        ds = Auto.RetrieveAll_AUTOCOMPLETE_SEARCH();

        if (ds.Tables[0].Rows.Count > 0)
        {
            custid = "Yes";
        }
        else
        {
            custid = "No";
        }
        return custid;
    }

    [System.Web.Services.WebMethod]
    public static string Set_Customs_House_Validation(string custid)
    {
        Auto_Search Auto = new Auto_Search();
        DataSet ds = new DataSet();

        Auto.Enamesearch = custid;
        Auto.Ename = "CUSTOM_HOUSE_MASTER";
        ds = Auto.RetrieveAll_AUTOCOMPLETE_SEARCH();

        if (ds.Tables[0].Rows.Count > 0)
        {
            custid = "Yes";
        }
        else
        {
            custid = "No";
        }
        return custid;
    }
    protected void ddlBEType_SelectedIndexChanged(object sender, EventArgs e)
    {
        pageload_auto();
        
    }

    [System.Web.Services.WebMethod]
    public static string GetAirLine_Code(string custid)
    {
        //Auto_Search Auto = new Auto_Search();
        //string name = "AIRLINE_MASTER_CODE";
        //string rows = "AIRLINE_CODE";
        //custid = Auto.Country_Master_FromClientSide_Calling(custid, name, rows);
        //if (Auto.Ename != "1")
        //{
        //    custid = "";
        //}
        //return custid;

        eroyalmaster Auto = new eroyalmaster();
        UserBO objUBO = new UserBO();
        DataSet ds = new DataSet();
        objUBO.AIRLINE_ID = "0";
        objUBO.flag = "AIRLINE_MASTER_CODE";
        objUBO.AIRLINE_CODE = custid;
        objUBO.AIRLINE_NAME = custid;
        objUBO.ITEM_REMARKS = "";
        objUBO.USER_ID = "";
        objUBO.BRANCH_CODE = "";
        objUBO.WORKING_PERIOD = Connection.WorkingPeriod();

        objUBO.startRowIndex = Convert.ToString(0);
        objUBO.maximumRows = Convert.ToString(0);
        objUBO.totalRows = Convert.ToString(0);

        ds = Auto.Airline_MASTER_USERWISE_Insert_update(objUBO);


        if (ds.Tables[0].Rows.Count > 0)
        {
            custid = ds.Tables[0].Rows[0]["AIRLINE_CODE"].ToString();
            Auto.Ename = "1";
        }


        if (Auto.Ename != "1")
        {
            custid = "";
        }
        return custid;
    }
}
