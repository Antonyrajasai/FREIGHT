using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;

public partial class Accounts_Invoice_Setting : ThemeClass
{
    User_Creation user_Create = new User_Creation();
    eroyalmaster erm = new eroyalmaster();
    Manipulation mo = new Manipulation();
    AppSession aps = new AppSession();

    DataSet dtuser = new DataSet();

    public int i, Screen_Id;
    public string INVOICE_ID = "", Working_Period;
    public string USER_RIGHTS_ID, SCREEN_ID, PAGE_READ, PAGE_WRITE, PAGE_MODIFY, PAGE_DELETE, Is_Master_Id;

    protected void Page_Load(object sender, EventArgs e)
    {
        aps.checkSession();
        Working_Period = Session["WorkingPeriod"].ToString();
     

        if (!IsPostBack)
        {
            //----------------SETTING SCREEN PERMISSION---------------//
            Screen_Id = 5;
            Page_Rights(Screen_Id);
            //----------------SETTING SCREEN PERMISSION---------------//
            EditSupDetails();
        }
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
            btnNew.Enabled = false;
            btnNew.ToolTip = "You don't have permission to access";
            btnSave.ToolTip = "You don't have permission to access";
            btnUpdate.ToolTip = "You don't have permission to access";
            btnDelete.ToolTip = "You don't have permission to access";
        }
        else
        {
            //---------------SAVE BUTTON------------------//
            if (PAGE_WRITE == "True")
            {
                btnSave.Enabled = true;
                btnNew.Enabled = true;
            }
            else
            {
                btnSave.Enabled = false;
                btnSave.ToolTip = "You don't have permission to access";
                btnNew.Enabled = false;
            }
            //---------------UPDATE BUTTON------------------//
            if (PAGE_MODIFY == "True")
                btnUpdate.Enabled = true;
            else
            {
                btnUpdate.Enabled = false;
                btnUpdate.ToolTip = "You don't have permission to access";

            }
            //---------------DELETE BUTTON------------------//

            if (PAGE_DELETE == "True")
                btnDelete.Enabled = true;
            else
            {
                btnDelete.Enabled = false;
                btnDelete.ToolTip = "You don't have permission to access";

            }
        }
    }
    public void EditSupDetails()
    {
        //if (Request.QueryString["INVOICE_ID"] != "" && Request.QueryString["INVOICE_ID"] != null)
        //{
            int i = 0;
           // INVOICE_ID = Request.QueryString["INVOICE_ID"];

          
            erm.Ename = "select_conwithcode";


            dtuser = erm.RetrieveAll_Invoice_Setting();
            if (dtuser.Tables[0].Rows.Count > 0)
            {

                txtCompanyName.Text = dtuser.Tables[0].Rows[0]["COMPANY_NAME"].ToString();
                txtGstnid.Text = dtuser.Tables[0].Rows[0]["GSTNID"].ToString();
                txtAddress1.Text = dtuser.Tables[0].Rows[0]["ADDRESS"].ToString();
                txtPincode.Text = dtuser.Tables[0].Rows[0]["PINCODE"].ToString();
                txtCommercial_StateName.Text = dtuser.Tables[0].Rows[0]["STATENAME"].ToString();
                txtCommercial_StateCode.Text = dtuser.Tables[0].Rows[0]["STATECODE"].ToString();

                txtmobileno.Text = dtuser.Tables[0].Rows[0]["MOBILE_NO"].ToString();
                txtEmailid.Text = dtuser.Tables[0].Rows[0]["EMAILID"].ToString();
                HiddenField1.Value = dtuser.Tables[0].Rows[0]["INVOICE_ID"].ToString();
                //FOR UPDATED SUCCESSFULLY
                ViewState["UPDATED_ID"] = null;
                ViewState["UPDATED_ID"] = dtuser.Tables[0].Rows[0]["INVOICE_ID"].ToString();
                //FOR UPDATED SUCCESSFULLY
                btnDelete.Visible = true;
                btnUpdate.Visible = true;
                btnSave.Visible = false;
                btnNew.Visible = false;

            }
            else
            {




                btnDelete.Visible = false;
                btnUpdate.Visible = false;
                btnNew.Visible = true;
                btnSave.Visible = true;
            }
    }

    public int insert_update(string a1, string a2, string a3)
    {
        erm.INVOICE_ID = HiddenField1.Value;
        erm.COMPANY_NAME = txtCompanyName.Text;
        erm.GSTNID = txtGstnid.Text;
        erm.ADDRESS = txtAddress1.Text;
        erm.PINCODE = txtPincode.Text;

        erm.STATENAME = txtCommercial_StateName.Text;
        erm.STATECODE = txtCommercial_StateCode.Text;
        erm.EMAILID = txtEmailid.Text;
        erm.MOBILE_NO = txtmobileno.Text;
      
      
        erm.flag = a2;
      
     

        i = erm.Invoice_setting_Insert_update();
        return i;
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        if (txtCompanyName.Text != "" && txtAddress1.Text != "")
        {
            try
            {
                i = insert_update("", "S", "");
                HiddenField1.Value = Convert.ToString(erm.UPDATE_ID);

                if (i == 1)
                {
                    //-----------------------FOR USER RIGHTS--------------------------------//
                    user_Create.RetrieveAll_User_Screen_Rights_Details(Screen_Id);
                    PAGE_MODIFY = user_Create.PAGE_MODIFY;
                    //Load_Elogix_Service("S");
                    //---------------SAVE ONLY------------------//
                    if (PAGE_MODIFY == "False")
                    {
                        btnNew_Click(sender, e);
                    }
                    else
                    {
                        // btnUpdate.CssClass = "save";
                        btnUpdate.Visible = true;
                        btnDelete.Visible = true;
                        btnSave.Visible = false;
                        btnNew.Visible = false;
                    }
                    //-----------------------FOR USER RIGHTS--------------------------------//
                    ViewState["UPDATED_ID"] = null;
                    Alert_msg("Saved Successfully", "btnSave");

                }
                else if (i == 2)
                {
                    Alert_msg("Invoice Setting Name already exixts!", "btnSave");
                }
                else
                {
                    Alert_msg("Invoice Setting Not Saved", "btnNew");
                }

            }
            catch (Exception ex)
            {
                Connection.Error_Msg(ex.Message);
            }
        }
        else
        {
            Alert_msg("Enter All Mandatory Field", "btnNew");
        }
    }

    protected void btnNew_Click(object sender, EventArgs e)
    {
        Session["SUP_ID"] = "";

        mo.ResetFields(Page.Controls);
        txtCompanyName.Focus();
        btnDelete.Visible = false;
        btnUpdate.Visible = false;
        btnNew.Visible = true;
        btnSave.Visible = true;
        hdn_Update_BUYER_Name.Value = "";
        ViewState["UPDATED_ID"] = null;
    }
    protected void btnUpdate_Click(object sender, EventArgs e)
    {
        try
        {
            i = insert_update(HiddenField1.Value, "U", "");
            if (i == 1)
            {
                hdn_Update_BUYER_Name.Value = txtCompanyName.Text;
                //Load_Elogix_Service("U");
                if (ViewState["UPDATED_ID"] != null)
                {
                    Alert_msg("Updated Successfully", "btnUpdate");
                }
                else
                {
                    Alert_msg("Saved Successfully", "btnUpdate");
                }

            }
           
            else
            {
                Alert_msg("Invoice setting Details Not Saved", "btnUpdate");
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
            erm.INVOICE_ID = HiddenField1.Value;


            erm.Ename = "delete";


            dtuser = erm.RetrieveAll_Invoice_Setting();
            int i = erm.result;
            if (i > 0)
            {
                string prompt = "<script>$(document).ready(function(){{jAlert('Invoice Setting Details Deleted', 'Invoice Setting', function (r) {var i = r + 'ok';if (i == 'trueok'){RefreshParent();} else {}});}});</script>";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", prompt, false);
            }
            else
            {
                Alert_msg("Invoice Setting Details Not Deleted");
            }
        }
        catch (Exception ex)
        {
            Connection.Error_Msg(ex.Message);
        }
    }

    //protected void txtName_TextChanged(object sender, EventArgs e)
    //{
    //    if (hdn_Update_BUYER_Name.Value != txtName.Text)
    //    {
    //        try
    //        {
    //            erm.Ebuyerid = "0";
    //            erm.Ebuyer_name = txtName.Text;
    //            erm.Ename = "Check_BUYER_Name";
    //            erm.EWorking_Period = Working_Period;


    //           dtuser= erm.RetrieveAll_Salesman_MASTER();
    //            if (dtuser.Tables[0].Rows.Count > 0)
    //            {
    //                Alert_msg("Consignor already exists!", "txtAdd1");
    //            }
    //        }
    //        catch (Exception ex)
    //        {
    //            Connection.Error_Msg(ex.Message);
    //        }

    //    }
    //    txtAdd1.Focus();
    //}

    public void Alert_msg(string msg)
    {
        string prompt = "<script>$(document).ready(function(){{jAlert('" + msg + "', 'Invoice Setting', function (r) {});}});</script>";
        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", prompt, false);
    }

    public void Alert_msg(string msg, string focus)
    {
        string prompt = "<script>$(document).ready(function(){{jAlert('" + msg + "', 'Invoice Setting', function (r) {document.getElementById('" + focus + "').focus();});}});</script>";
        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", prompt, false);
    }

    [System.Web.Services.WebMethod]
    public static string GetCountry_code(string custid)
    {
        Auto_Search Auto = new Auto_Search();
        string name = "COUNTRY_MASTER_CODE";
        string rows = "COUNTRY_CODE";
        custid = Auto.Country_Master_FromClientSide_Calling(custid, name, rows);
        if (Auto.Ename != "1")
        {
            custid = "";
        }
        return custid;
    }


}