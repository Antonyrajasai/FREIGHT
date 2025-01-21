
using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Net.Mail;
using BussinessObject;

public partial class Accounts_GeneratrInvoiceold : ThemeClass
{
    GST_Imp_Invoice BP = new GST_Imp_Invoice();
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
        SELECT_BRANCH = Connection.Current_Branch(); ;
        COMPANY_ID = Session["COMPANY_ID"].ToString();
        hdn_CompanyID.Value = COMPANY_ID;
        hdn_currentPage.Value = "General";
        hdnbrancharea.Value = currentbranch;

        Screen_Id = 7;
        Page_Rights(Screen_Id);

        txtJobDate.Focus();
        Session.Remove("IMP_AIR_EXBND_JOBID");




        if (!this.IsPostBack)
        {
            string[] words = currentbranch.Split(',');

            hdnarea.Value = words[1].Replace(" ", "").ToString();
            //hdnbrancharea.Value = Session["AREABRANCH"].ToString();
            //if (Connection.Ryal() != string.Empty)
            //{
            //    txt_FF_Jobno.Attributes.Add("readonly", "readonly");
            //    txt_FF_Jobno.Visible = true;
            //    ND.Visible = false;
            //    WD.Visible = true;
            //}
            //else
            //{
            //    txt_FF_Jobno.Attributes.Add("readonly", "readonly");
            //    txt_FF_Jobno.Visible = false;
            //    ND.Visible = true;
            //    WD.Visible = false;
            //}


           // hdnFreightMode.Value = Request["F_Mode"].ToString();
            //if (hdnFreightMode.Value == "Air")
            //{

            //    ddlTransportMode.Items.Add(new ListItem("Air", "Air"));
            //    spancontrol.InnerText = "Air-Import Planning";
            //}
            //else if (hdnFreightMode.Value == "Sea")
            //{

            //    ddlTransportMode.Items.Add(new ListItem("Sea", "Sea"));
            //    spancontrol.InnerText = "Sea-Import Planning";

            //}
            //load_SalesBy();
            ////load_Tr_Mode();
            //load_infoType();
            //load_IncoTerms();
            //load_AEOType();
            //load_FreightType();
            //load_Type();
            //load_BLType();
            //load_Contype();
            txtJobDate.Text = dt.Today_Date();
            if (!string.IsNullOrEmpty(Request["Page"]))
            {
                string s = Request["Page"].ToString();
                if (Request["Page"] == "New")
                {


                    hdn_New_Update.Value = "New";
                    txtImporterName.Attributes.Add("onblur", "javascript:CallServerside_Importer_2('" + txtImporterName.ClientID + "', '" + hd_Brslno.ClientID + "', '" + hdn_New_Update.Value + "', '" + txtJobNo.Text + "')");
                    txtImporterName.Attributes.Add("onload", "javascript:CallServerside_Importer_2('" + txtImporterName.ClientID + "', '" + hd_Brslno.ClientID + "', '" + hdn_New_Update.Value + "', '" + txtJobNo.Text + "')");
                    txtImporterName.Attributes.Add("onchange", "javascript:CallServerside_Importer_2('" + txtImporterName.ClientID + "', '" + hd_Brslno.ClientID + "', '" + hdn_New_Update.Value + "', '" + txtJobNo.Text + "')");

                    txtBranchNo.Attributes.Add("onblur", "javascript:CallServerside_Importer_Branch('" + txtBranchNo.ClientID + "', '" + txtIECNo.ClientID + "', '" + hdn_New_Update.Value + "', '" + txtJobNo.Text + "')");

                    hdn_Jobno.Value = "";
                    Session.Remove("IMP_AIR_TRANSPORT_MODE");
                    Session.Remove("IMP_AIR_IMPORT_JOBNO");
                    Session.Remove("IMP_AIR_IMPORT_JOBDATE");
                    Session.Remove("IMP_AIR_FILING_STATUS");
                    Session.Remove("IMP_AIR_COUNTRY_OF_ORIGIN");

                    //-------------------------------FROM LOGIN PAGE BASED ON BRANCH SELECT-----------------//

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
                    //-------------------------------FROM LOGIN PAGE BASED ON BRANCH SELECT-----------------//



                }
            }
            else
            {
                hdn_Jobno.Value = txtjobps.Text;

            }
            //--------------- dont delete-------------------------------------//

            //txtIECNo.Attributes.Add("readonly", "readonly");


            //--------------- dont delete--------------------------------------//

            //Get_Client_Default_Settings();

            ///------------------------------------------------auto search-----------------------------------------------------------------

            // txtBranchSLNo.Attributes.Add("onblur", "javascript:CallServerside_BranchSeller('" + txtBranchSLNo.ClientID + "', '" + txtHighSeaSaleIECNo.ClientID + "')");
            txtCountryName.Attributes.Add("onblur", "javascript:CallServerside_Country_of_orgin('" + txtCountryName.ClientID + "', '" + ddlTransportMode.ClientID + "')");
            txtCountryShipment.Attributes.Add("onblur", "javascript:CallServerside_Country_of_Shipment('" + txtCountryShipment.ClientID + "', '" + ddlTransportMode.ClientID + "')");
            txtPortOrigin.Attributes.Add("onblur", "javascript:CallServerside_Port_of_orgin('" + txtPortOrigin.ClientID + "', '" + ddlTransportMode.ClientID + "')");
            txtPortShipment.Attributes.Add("onblur", "javascript:CallServerside_Port_of_Shipment('" + txtPortShipment.ClientID + "', '" + ddlTransportMode.ClientID + "')");

            txtCountryName.Attributes.Add("onfocus", "javascript:CallServerside_Country_of_orgin('" + txtCountryName.ClientID + "', '" + ddlTransportMode.ClientID + "')");
            txtCountryShipment.Attributes.Add("onfocus", "javascript:CallServerside_Country_of_Shipment('" + txtCountryShipment.ClientID + "', '" + ddlTransportMode.ClientID + "')");
            txtPortOrigin.Attributes.Add("onfocus", "javascript:CallServerside_Port_of_orgin('" + txtPortOrigin.ClientID + "', '" + ddlTransportMode.ClientID + "')");
            txtPortShipment.Attributes.Add("onfocus", "javascript:CallServerside_Port_of_Shipment('" + txtPortShipment.ClientID + "', '" + ddlTransportMode.ClientID + "')");


            txtCustomsHouse.Attributes.Add("onblur", "javascript:CallServerside_Custom_House_Name('" + txtCustomsHouse.ClientID + "', '" + hdn_CustomsHouse_Name.ClientID + "')");



            Edit_GeneralDetails();
        }
        //hdfTransport_mode.Value = ddlTransportMode.SelectedValue.ToString();

        try
        {
            if (Session["USER_DETAILS_DS"] != null)
            {
                if (Session["IMP_AIR_JOB_TYPE"] != null)
                {
                    ds = (DataSet)Session["USER_DETAILS_DS"];
                }

                DataView view1 = ds.Tables[1].DefaultView;
                string USER_CREATION_ID, BRANCH;
                //COMPANY_ID = Session["COMPANY_ID"].ToString();
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
            TypeHide();
        }

        //-----------------job edit S---------------------------



        hdsaveandclose.Value = "No";

        ddlBookingNo.Attributes.Add("onchange", "javascript:CallServerside_BookingNo('" + ddlBookingNo.ClientID + "','" + ddlBEType.ClientID + "')");
        ddlBookingNo.Attributes.Add("onload", "javascript:CallServerside_BookingNo('" + ddlBookingNo.ClientID + "','" + ddlBEType.ClientID + "')");

    }

    public void TypeHide()
    {
        if (ddlBEType.SelectedValue.ToString() == "FORWARDING")
            Adcode_div.Style.Add("display", "none");
        else
            Adcode_div.Style.Add("display", "block");
    }
    private void load_Tr_Mode()
    {
        ddlTransportMode.Items.Clear();
        ddlTransportMode.Items.Add(new ListItem("Air", "Air"));
        ddlTransportMode.Items.Add(new ListItem("Sea", "Sea"));
        ddlTransportMode.Items.Add(new ListItem("Land", "Land"));
    }
    private void load_infoType()
    {
        ddl_info_type.Items.Clear();
        ddl_info_type.Items.Add(new ListItem("Non DG", "Non DG"));
        ddl_info_type.Items.Add(new ListItem("DG", "DG"));
        ddl_info_type.Items.Add(new ListItem("OTHERS", "OTHERS"));
    }
    private void load_Type()
    {
        ddlBEType.Items.Clear();
        ddlBEType.Items.Add(new ListItem("FORWARDING", "FORWARDING"));
        ddlBEType.Items.Add(new ListItem("CLEARING", "CLEARING"));
        //ddlBEType.Items.Add(new ListItem("OTHERS", "OTHERS"));
        ddlBEType.Items.Add(new ListItem("BOTH", "BOTH"));
    }
    private void load_FreightType()
    {
        ddlFreight.Items.Clear();
        ddlFreight.Items.Add(new ListItem("PREPAID", "PREPAID"));
        ddlFreight.Items.Add(new ListItem("COLLECT", "COLLECT"));
        //ddlFreight.Items.Add(new ListItem("OTHERS&FREE", "OTHERS&FREE"));

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
    public void Bind_Quotation_No()
    {
        eFreightQuotation_Transactions Trans = new eFreightQuotation_Transactions();
        Trans.JOBID = HiddenField1.Value;
        Trans.JOBNO = "";
        Trans.EXPORTER_NAME = "";
        Trans.TRANSPORT_MODE = "";
        Trans.CUSTOMS_HOUSE = "";
        Trans.Ename = "LOAD_IMP_AIR_JOBNO";
        Trans.WORKING_PERIOD = currentbranch;
        Trans.TYPE = ddlBEType.SelectedValue.ToString();

        ddlQuotationNO.DataSource = Trans.RetrieveAll_QUOTATION_Details();
        ddlQuotationNO.DataValueField = "JOBNO";
        ddlQuotationNO.DataTextField = "JOBNO";
        ddlQuotationNO.DataBind();
        ddlQuotationNO.Items.Remove("0");
        ddlQuotationNO.Items.Insert(0, "Select");
    }
    public void Bind_BookingNo()
    {
        eFreighImport_BookingForm Trans = new eFreighImport_BookingForm();
        Trans.JOBID = HiddenField1.Value;
        Trans.JOBNO = "";
        Trans.EXPORTER_NAME = "";
        Trans.TRANSPORT_MODE = "";
        Trans.CUSTOMS_HOUSE = "";
        Trans.Ename = "LOAD_IMP_AIR_JOBNO";
        Trans.WORKING_PERIOD = currentbranch;
        Trans.TYPE = ddlBEType.SelectedValue.ToString();

        ddlBookingNo.DataSource = Trans.RetrieveAll_BOOKINGFORM_AIR_Details();
        ddlBookingNo.DataValueField = "JOBNO";
        ddlBookingNo.DataTextField = "JOBNO";
        ddlBookingNo.DataBind();
        ddlBookingNo.Items.Remove("0");
        ddlBookingNo.Items.Insert(0, "Select");
    }

    private void load_SalesBy()
    {
        ddlFilingStatus.Items.Clear();
        ddlFilingStatus.Items.Add(new ListItem("Advanced", "Advanced"));
        ddlFilingStatus.Items.Add(new ListItem("Prior", "Prior"));
        ddlFilingStatus.Items.Add(new ListItem("Normal", "Normal"));
    }
    private void load_Contype()
    {
        ddlContainer_Type.Items.Clear();
        ddlContainer_Type.Items.Add(new ListItem("LCL", "LCL"));
        ddlContainer_Type.Items.Add(new ListItem("FCL", "FCL"));
        ddlContainer_Type.Items.Add(new ListItem("BULK", "BULK"));
    }

    private void load_AEOType()
    {
        ddl_AEOtypoe.Items.Clear();
        ddl_AEOtypoe.Items.Add(new ListItem("GENERAL", "GENERAL"));
        ddl_AEOtypoe.Items.Add(new ListItem("TYRE-I", "TYRE-I"));
        ddl_AEOtypoe.Items.Add(new ListItem("TYRE-II", "TYRE-II"));
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
                Submission.Style.Add("display", "block");
            }
            else
            {
                Submission.Style.Add("display", "none");
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
    private void load_BLType()
    {
        ddlBLType.Items.Clear();
        ddlBLType.Items.Add(new ListItem("OBL", "OBL"));
        ddlBLType.Items.Add(new ListItem("Surrender", "Surrender"));
        ddlBLType.Items.Add(new ListItem("Seaway", "Seaway"));

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
           
           
            int i = 0;
            gridbind();
            if (dtuser.Tables[0].Rows.Count > 0)
            {
                ddlBEType.AutoPostBack = false;
                ddlBEType.Enabled = false;
              


                txtjobps.Text = dtuser.Tables[0].Rows[0]["JOBNO_PS"].ToString() == "" ? dtuser.Tables[0].Rows[0]["JOBNO"].ToString() : dtuser.Tables[0].Rows[0]["JOBNO_PS"].ToString();
                hd_Brslno.Value = dtuser.Tables[0].Rows[0]["BRANCH_NO"].ToString();

                hdprefix.Value = dtuser.Tables[0].Rows[0]["JOB_PREFIX"].ToString() == null ? string.Empty : dtuser.Tables[0].Rows[0]["JOB_PREFIX"].ToString();
                hdsuffix.Value = dtuser.Tables[0].Rows[0]["JOB_SUFFIX"].ToString() == null ? string.Empty : dtuser.Tables[0].Rows[0]["JOB_SUFFIX"].ToString();

                // hdn_ImporterType.Value = dtuser.Tables[0].Rows[0]["IMP_EXP_TYPE"].ToString();
                txtJobNo.Text = dtuser.Tables[0].Rows[0]["JOBNO"].ToString();
                Session["IMP_AIR_Jobno_Excel"] = txtJobNo.Text;

                txt_jobno.Text = txtjobps.Text;
                txt_M_jobps.Text = dtuser.Tables[0].Rows[0]["JOB_PREFIX"].ToString() == null ? string.Empty : dtuser.Tables[0].Rows[0]["JOB_PREFIX"].ToString();
                txt_M_jobsf.Text = dtuser.Tables[0].Rows[0]["JOB_SUFFIX"].ToString() == null ? string.Empty : dtuser.Tables[0].Rows[0]["JOB_SUFFIX"].ToString();

                hdn_New_Update.Value = "Update";
                txtImporterName.Attributes.Add("onblur", "javascript:CallServerside_Importer_2('" + txtImporterName.ClientID + "', '" + hd_Brslno.ClientID + "', '" + hdn_New_Update.Value + "', '" + txtJobNo.Text + "')");

                txtBranchNo.Attributes.Add("onblur", "javascript:CallServerside_Importer_Branch('" + txtBranchNo.ClientID + "', '" + txtIECNo.ClientID + "', '" + hdn_New_Update.Value + "', '" + txtJobNo.Text + "')");

                txtJobDate.Text = dtuser.Tables[0].Rows[0]["JOBDATE"].ToString();

                //txtBE_Date.Text = dtuser.Tables[0].Rows[0]["BE_DATE"].ToString();

                string mode = dtuser.Tables[0].Rows[0]["TRANSPORT_MODE"].ToString();

                //hdfTransport_mode.Value = mode;
                Session["IMP_AIR_TRANSPORT_MODE"] = mode;

                ddlTransportMode.SelectedValue = mode;


                txtCustomsHouse.Text = dtuser.Tables[0].Rows[0]["CUSTOMS_HOUSE"].ToString();
                Session["CUSTOMHOUSE_CODE"] = dtuser.Tables[0].Rows[0]["CUSTOMS_HOUSE"].ToString();

                hdn_CustomsHouse_Name.Value = dtuser.Tables[0].Rows[0]["CUSTOMS_HOUSE_NAME"].ToString();
                hdn_Customhouse_chk.Value = Set_Customs_House_Validation(txtCustomsHouse.Text);

                txtImporterName.Text = dt.Replace(dtuser.Tables[0].Rows[0]["IMPORTER_NAME"].ToString());

                txtIECNo.Text = dtuser.Tables[0].Rows[0]["IEC_NO"].ToString();
                txtBranchNo.Text = dtuser.Tables[0].Rows[0]["BRANCH_NO"].ToString();

                txtCountryName.Text = dt.Replace(dtuser.Tables[0].Rows[0]["COUNTRY_ORIGIN"].ToString());
                //Hdcountryoforgin_chk.Value = Ser_Country_of_orgin(txtCountryName.Text, hdfTransport_mode.Value);

                txtPortOrigin.Text = dt.Replace(dtuser.Tables[0].Rows[0]["PORT_ORIGIN"].ToString());
                //Hdportoforgin_Chk.Value = Ser_Port_of_orgin(txtPortOrigin.Text, hdfTransport_mode.Value);

                txtPortShipment.Text = dt.Replace(dtuser.Tables[0].Rows[0]["PORT_SHIPMENT"].ToString());
                //Hdportofshipment_chk.Value = Ser_Port_of_orgin(txtPortShipment.Text, hdfTransport_mode.Value);

                txtCountryShipment.Text = dt.Replace(dtuser.Tables[0].Rows[0]["COUNTRY_SHIPMENT"].ToString());
                //Hdcountryofshipment_chk.Value = Ser_Country_of_orgin(txtCountryShipment.Text, hdfTransport_mode.Value);

                string fil_status = dtuser.Tables[0].Rows[0]["SALES_BY"].ToString();
                txtSalesby.Text = dtuser.Tables[0].Rows[0]["SALES_BY"].ToString();
                load_Type();
                load_SalesBy();
                ddlFilingStatus.Items.Remove(fil_status);

                ddlFilingStatus.Items.Insert(0, fil_status);

                txtClientName.Text = dtuser.Tables[0].Rows[0]["CLIENT_NAME"].ToString();
                txtSuppliername.Text = dtuser.Tables[0].Rows[0]["SUPPLIER_NAME"].ToString();               
                txtNotify.Text = dtuser.Tables[0].Rows[0]["NOTIFY_NAME"].ToString();                
                txtPortofdelivery.Text = dtuser.Tables[0].Rows[0]["PORTDELIVERY"].ToString();
                txtFinalPortofdelivery.Text = dtuser.Tables[0].Rows[0]["FINAL_PORTDELIVERY"].ToString();
                txtclearanceplace.Text = dtuser.Tables[0].Rows[0]["CLEARNCEPLACE"].ToString();
                txtCleardate.Text = dtuser.Tables[0].Rows[0]["CLEARANCE_DATE"].ToString();

                txtClientName.Text = dtuser.Tables[0].Rows[0]["CLIENT_NAME"].ToString();
                txtFile_Ref_No.Text = dtuser.Tables[0].Rows[0]["FILE_REF_NO"].ToString();                
                Session["IMP_AIR_JOB_LOCK"] = JOB_LOCK;
                Session["IMP_AIR_JOB_RELEASE"] = JOB_RELEASE;
                Session["IMP_AIR_JOB_USER"] = JOB_USER;
                txtETAdate.Text = dtuser.Tables[0].Rows[0]["ETA_DATE"].ToString().Replace("01/01/1900", "");
                string[] d2 = dtuser.Tables[0].Rows[0]["ETA_TIME"].ToString().Split('/');
                ddlhrs_ETA_dat.SelectedValue = d2[0].ToString();
                ddlsec_ETA_dat.SelectedValue = d2[1].ToString();

                txtETDdate.Text = dtuser.Tables[0].Rows[0]["ETD_DATE"].ToString().Replace("01/01/1900", "");
                string[] d3 = dtuser.Tables[0].Rows[0]["ETD_TIME"].ToString().Split('/');
                ddlhrs_ETD_dat.SelectedValue = d3[0].ToString();
                ddlsec_ETD_dat.SelectedValue = d3[1].ToString();

                txtATAdate.Text = dtuser.Tables[0].Rows[0]["ATA_DATE"].ToString().Replace("01/01/1900", "");
                string[] d4 = dtuser.Tables[0].Rows[0]["ATA_TIME"].ToString().Split('/');
                ddlhrs_ATA_dat.SelectedValue = d4[0].ToString();
                ddlsec_ATA_dat.SelectedValue = d4[1].ToString();
                txtJobhandledby.Text = dtuser.Tables[0].Rows[0]["JOB_HANDLEDBY"].ToString();
                ddlIncoterms.SelectedValue = dtuser.Tables[0].Rows[0]["INCOTERMS"].ToString();
                ddlFreight.SelectedValue = dtuser.Tables[0].Rows[0]["FREIGHTTYPE"].ToString();

                Bind_BookingNo();
                Bind_Quotation_No();
                
                if (!string.IsNullOrWhiteSpace(dtuser.Tables[0].Rows[0]["QUOTATION_NO"].ToString()))
                {


                    ddlQuotationNO.SelectedValue = dtuser.Tables[0].Rows[0]["QUOTATION_NO"].ToString();
                }
                
                ViewState["UPDATED_ID"] = null;
                ViewState["UPDATED_ID"] = Request.QueryString["JOBID"].ToString();
                Session.Remove("IMP_AIR_TRANSPORT_MODE");
                Session.Remove("IMP_AIR_IMPORT_JOBNO");
                Session.Remove("IMP_AIR_IMPORT_JOBDATE");
                Session.Remove("IMP_AIR_FILING_STATUS");
                Session.Remove("IMP_AIR_COUNTRY_OF_ORIGIN");
                Session.Remove("IMP_AIR_IMPORTER_NAME");
                Session.Remove("IMP_AIR_COMMODITY");
                Session["IMP_AIR_TRANSPORT_MODE"] = ddlTransportMode.SelectedValue.ToString();
                Session["IMP_AIR_IMPORT_JOBNO"] = txtJobNo.Text;
                Session["IMP_AIR_IMPORTER_NAME"] = txtImporterName.Text;
                Session["IMP_AIR_IMPORT_JOBDATE"] = txtJobDate.Text;
                Session["IMP_AIR_FILING_STATUS"] = ddlFilingStatus.SelectedValue.ToString();
                Session["IMP_AIR_SALESBY"] = txtSalesby.Text;
                Session["IMP_AIR_COUNTRY_OF_ORIGIN"] = txtCountryName.Text;
                Session["IMP_AIR_COMMODITY"] = txtCommodity.Text;
                ddlBEType.SelectedValue = dtuser.Tables[0].Rows[0]["TYPE"].ToString();
                Session.Remove("TYPE");
                Session["TYPE"] = ddlBEType.SelectedValue.ToString();


                Session.Remove("IMP_AIR_BOOKINGNO");
                Session["IMP_AIR_BOOKINGNO"] = ddlBookingNo.SelectedValue.ToString();

                // added by jayanthi 
                ddlJobSts.SelectedValue = dtuser.Tables[0].Rows[0]["JOB_STS"].ToString();

            }

           


        }
        else
        {

            if (!IsPostBack)
            {
                //-------------Dont delete----------------------------
                //btnSave.Attributes.Add("onclick", "return validate();");
                //btnNew.Attributes.Add("onclick", "jQuery('#form1').validationEngine('detach');");
                //btnUpdate.Attributes.Add("onclick", "return validate();");
                //-----------------------------------------------------
                pageload_auto();
                Bind_BookingNo();
                Bind_Quotation_No();
            }
        }

        TypeHide();
    }
    public void pageload_auto()
    {
        
        ddlBEType.AutoPostBack = true;
        ddlBEType.Enabled = true;
        //hdfTransport_mode.Value = ddlTransportMode.SelectedValue.ToString();
        Session["IMP_AIR_TRANSPORT_MODE"] = ddlTransportMode.SelectedValue.ToString();


        Operation_SEA1.Style.Add("display", "Block");
        Operation_SEA2.Style.Add("display", "none");
        //btnDelete.Visible = false;
       

        hdn_Jobno.Value = "";

        Session.Remove("IMP_AIR_TRANSPORT_MODE");
        Session.Remove("IMP_AIR_IMPORT_JOBNO");
        Session.Remove("IMP_AIR_IMPORT_JOBDATE");
        Session.Remove("IMP_AIR_FILING_STATUS");
        Session.Remove("IMP_AIR_COUNTRY_OF_ORIGIN");
        //Session.Remove("IMP_AIR_IMPORTER_NAME");
        Session["IMP_AIR_commercialYN"] = "N";
    }
    public int insert_update(string jobid, string flag)
    {
        return 1;
        
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
            ObjUBO.A2 ="E";
            ObjUBO.A26 = "Sea";
        }
        ObjUBO.A3 ="L";
        ObjUBO.A4 = txtClientName.Text;
        //ObjUBO.A5 = ddlbranch_No.SelectedValue.ToString();
        ObjUBO.A5 = hd_Brslno.Value;
        ObjUBO.A23 = txtBranchNo.Text;

        ObjUBO.A6 = "";
        ObjUBO.A7 = txtjobps.Text;
        ObjUBO.A8 = "";
        ObjUBO.A9 = DateTime.Now.ToShortDateString();
        ObjUBO.A10 = txtClientName.Text;
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
        ObjUBO.A42 = hdnbrancharea.Value;
        ObjUBO.A43 = ddlBEType.SelectedValue;
        ObjUBO.A46 = "";
        ObjUBO.A49 = "";
        ObjUBO.A50 = "";
        ObjUBO.A44 =  "N";
        ObjUBO.A45 =  "N";
        ObjUBO.A47 = "Approved";
        ObjUBO.A48 = "";
        ObjUBO.A51 = "";
        ObjUBO.A52 ="";
        
       //ds= gen.Billing_Invoice_INS_UPD(ObjUBO);
       //if (ds.Tables[0].Rows.Count > 0)
       //{
           //Alert_msg("Invoice Generated Successfully"); 
           //btngenerate.Visible = false;
           //Session["Value"] = "G";
           //string str = "<script>window.parent.location.href = window.parent.location.href</script>";
           //Page.ClientScript.RegisterStartupScript(this.GetType(), "Script", str, false);
        string qstr = "Page=GI&IMP_EXP=" + ObjUBO.A2 + "&MODE=" + ObjUBO.A26 + "&CUS_BRANCH_NO=" + ObjUBO.A5 + "&CUS_BRANCH_NO_LOC=" + ObjUBO.A23 + "&JOBNO=" + ObjUBO.A7 + "&CUS_NAME=" + ObjUBO.A4 + "&PARTY_NAME=" + ObjUBO.A10 + "&Type=" + ObjUBO.A43 + "";
        //Response.Redirect("../Accounts/GST_Imp_Exp_Invoice_Job.aspx?Page=" + ds.Tables[0].Rows[0][0].ToString() + "&IMP_EXP=" + ds.Tables[0].Rows[0][1].ToString() + "&Billinvno=" + ds.Tables[0].Rows[0][2].ToString());
        Response.Redirect("../Accounts/GST_Imp_Exp_Invoice_Job.aspx?"+qstr+"");
           //btngenerate.Attributes.Add("onclick", "javascript:open_GST_Billing_paramount_Update('" + ds.Tables[0].Rows[0][0].ToString() + "', '" + ds.Tables[0].Rows[0][1].ToString() + "', '" + ds.Tables[0].Rows[0][2].ToString() + "')");


       //}
    }
   

    
    public void Alert_msg_Operation(string msg, string focus)
    {
        string prompt = "<script>$(document).ready(function(){{jAlert('" + msg + "', 'GENERAL', function (r) {RefreshParent();});}});</script>";
        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", prompt, false);
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
        string prompt = "<script>$(document).ready(function(){{jAlert('" + msg + "', 'GENERAL', function (r) {Import_session('" + ddlTransportMode.SelectedValue.ToString() + "','" + txtJobNo.Text + "','" + txtJobDate.Text + "','" + txtSalesby.Text + "');document.getElementById('" + focus + "').focus();});}});</script>";
        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", prompt, false);
    }
    ///auto search
    ///
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
    public static string GetImporter(string custid)
    {
        Auto_Search Auto = new Auto_Search();
        DataSet ds = new DataSet();
        Auto.Ename = "IMP_EXP_MASTER_IMPORTER_SEARCH";
        Auto.Enamesearch = custid;

        ds = Auto.ERoyalmaste_RetrieveAll_AUTOCOMPLETE_SEARCH();
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
            custid = Address1 + "~~" + Address2 + "~~" + Address3 + "~~" + Address4 + "~~" + Address5 + "~~" + IECNo + "~~" + BranchNo + "~~" + IMP_EXP_TYPE;
        }
        return custid;
    }
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
            job_release.Visible = false;
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
    [System.Web.Services.WebMethod]
    public static string BookingNo_Search(string custid, string type)
    {

        DataSet dtuser = new DataSet();

        eFreighImport_BookingForm Trans = new eFreighImport_BookingForm();
        Trans.JOBID = "0";
        Trans.JOBNO = custid;
        Trans.EXPORTER_NAME = "";
        Trans.TRANSPORT_MODE = "";
        Trans.CUSTOMS_HOUSE = "";
        Trans.Ename = "SEARCHJOBNO_IMP_AIR";
        Trans.WORKING_PERIOD = Connection.Current_Branch();
        Trans.TYPE = type;

        dtuser = Trans.RetrieveAll_BOOKINGFORM_AIR_Details();

        if (dtuser.Tables[0].Rows.Count > 0)
        {
            Trans.INCOTERMS = dtuser.Tables[0].Rows[0]["INCOTERMS"].ToString();
            Trans.FREIGHTTYPE = dtuser.Tables[0].Rows[0]["FREIGHTTYPE"].ToString();

            Trans.CLIENT_NAME = dtuser.Tables[0].Rows[0]["CLIENT_NAME"].ToString();
            Trans.EXPORTER_NAME = dtuser.Tables[0].Rows[0]["EXPORTERNAME"].ToString();

            Trans.PORT_ORIGIN = dtuser.Tables[0].Rows[0]["PORTOFORIGIN"].ToString();
            Trans.PORTDELIVERY = dtuser.Tables[0].Rows[0]["PORTOFDELIVERY"].ToString();

            Trans.INVOICENO = dtuser.Tables[0].Rows[0]["INVOICENO"].ToString();
            Trans.INVOICEDATE = dtuser.Tables[0].Rows[0]["INVOICEDATE"].ToString().Replace("01/01/1900", "");
            Trans.BRANCH_NO = dtuser.Tables[0].Rows[0]["BRANCHNO"].ToString();

            Trans.IEC_NO = dtuser.Tables[0].Rows[0]["IECNO"].ToString();
            Trans.ADCODE = dtuser.Tables[0].Rows[0]["ADCODE"].ToString();
            Trans.JOB_HANDLEDBY = dtuser.Tables[0].Rows[0]["JOB_HANDLEDBY"].ToString();
            Trans.SALES_BY = dtuser.Tables[0].Rows[0]["SALES_BY"].ToString();
            Trans.COMMODITY = dtuser.Tables[0].Rows[0]["COMMODITY"].ToString();
            Trans.COMMODITY_OTHERSPECIFY = dtuser.Tables[0].Rows[0]["CONSIGNEENAME"].ToString();
            Trans.COMMODITY_TYPE = dtuser.Tables[0].Rows[0]["COMMODITYTYPE"].ToString();
            Trans.GROSSWEIGHT = dtuser.Tables[0].Rows[0]["GROSSWEIGHT"].ToString();
            Trans.CHARGEABLEWEIGHT = dtuser.Tables[0].Rows[0]["CHARGEABLEWEIGHT"].ToString();
            Trans.GROSSUNIT = dtuser.Tables[0].Rows[0]["GROSSUNIT"].ToString();
            Trans.CHARGEUNIT = dtuser.Tables[0].Rows[0]["CHARGEABLEUNIT"].ToString();
            Trans.EXPORTER_NAME = dtuser.Tables[0].Rows[0]["EXPORTERNAME"].ToString();
            Trans.FINAL_PORTDELIVERY = dtuser.Tables[0].Rows[0]["FINALPORTOFDELIVERY"].ToString();
            //Trans.CHARGEUNIT = dtuser.Tables[0].Rows[0]["CHARGEABLEUNIT"].ToString();
            // Trans.NOTIFY_NAME = dtuser.Tables[0].Rows[0]["NOTIFY_NAME"].ToString();
            //Trans.CLEARNCEPLACE = dtuser.Tables[0].Rows[0]["CLEARNCEPLACE"].ToString();
            //Trans.MARKSNOS = dtuser.Tables[0].Rows[0]["MARKSNOS"].ToString();
            Trans.AGENT_NAME = dtuser.Tables[0].Rows[0]["CLEARANCEPLACE"].ToString();
            Trans.SUPPLIERNAME = dtuser.Tables[0].Rows[0]["NOTIFY_NAME"].ToString();
            //Trans.VESSELNAME = dtuser.Tables[0].Rows[0]["VESSELNAME"].ToString();
        }

        custid = Trans.INCOTERMS + "~~" + Trans.FREIGHTTYPE + "~~" + Trans.CLIENT_NAME + "~~" + Trans.EXPORTER_NAME + "~~" + Trans.PORT_ORIGIN + "~~" + Trans.PORTDELIVERY
            + "~~" + Trans.INVOICENO + "~~" + Trans.INVOICEDATE + "~~" + Trans.BRANCH_NO + "~~" + Trans.IEC_NO + "~~" + Trans.ADCODE + "~~" + Trans.JOB_HANDLEDBY
            + "~~" + Trans.SALES_BY + "~~" + Trans.COMMODITY + "~~" + Trans.COMMODITY_OTHERSPECIFY + "~~" + Trans.COMMODITY_TYPE + "~~" + Trans.GROSSWEIGHT
            + "~~" + Trans.CHARGEABLEWEIGHT + "~~" + Trans.GROSSUNIT + "~~" + Trans.CHARGEUNIT + "~~" + Trans.EXPORTER_NAME + "~~" + Trans.FINAL_PORTDELIVERY
            + "~~" + Trans.AGENT_NAME + "~~" + Trans.SUPPLIERNAME + "~~" + Trans.VESSELNAME;
        return custid;
    }

    protected void ddlBEType_SelectedIndexChanged(object sender, EventArgs e)
    {
        pageload_auto();
        TypeHide();
        Bind_BookingNo();
        Bind_Quotation_No();
    }
}
