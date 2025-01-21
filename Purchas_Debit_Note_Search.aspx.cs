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

public partial class Account_Purches_Purchas_Debit_Note_Search : ThemeClass
{


    AppSession aps = new AppSession();
    User_Creation user_Create = new User_Creation();

    string[] arr = new string[] { };
    string[] Month = new string[] { };

    string Start_date = string.Empty, End_date = string.Empty;

    Purchase_cs BI = new Purchase_cs();
    Billing_UserBO ObjUBO = new Billing_UserBO();
    DataTable dt = new DataTable();
    protected void Page_Load(object sender, EventArgs e)
    {
        aps.checkSession();
        if (!IsPostBack)
        {
            Load_Month();
        }
        search();

    }
     protected void gvdetails_RowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header)
            e.Row.CssClass = "header";
        if (e.Row.RowType == DataControlRowType.DataRow && e.Row.RowState == DataControlRowState.Normal)
            e.Row.CssClass = "normal";
        if (e.Row.RowType == DataControlRowType.DataRow && e.Row.RowState == DataControlRowState.Alternate)
            e.Row.CssClass = "alternate";
    }
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        search();
    }
    private void search()
    {
        
        DataSet ds = new DataSet();
       ObjUBO.PURCHASE_DEBIT_NO=txtdebitno.Text;
        ObjUBO.PURCHASE_DEBIT_DATE=txtdebitdate.Text;
       // ObjUBO.VOUCHER_NO = txtsearch.Text;
        if (ddlvoucher.SelectedValue == "Voucher_No")
        {
            ObjUBO.VOUCHER_NO = txtsearch.Text;
        }
        else if (ddlvoucher.SelectedValue == "Vendor_Name")
        {
            ObjUBO.GSTN = txtsearch.Text;
        }
        else if (ddlvoucher.SelectedValue == "Vendor_Bill_No")
        {
            ObjUBO.VENDOR_BILL = txtsearch.Text;
        }
        else
        {
            ObjUBO.VOUCHER_NO = "";
        }
        if (txtFdate.Text != string.Empty || txtTodate.Text != string.Empty)
        {
            ObjUBO.VOUCHER_DATE = txtFdate.Text;   //from date
            ObjUBO.VENDOR_NAME = txtTodate.Text;  //To date
        }
        else
        {
            if (ddlMonth.SelectedItem.Text != string.Empty)
            {
                Month = ddlMonth.SelectedValue.ToString().Split('-');
                ObjUBO.VOUCHER_DATE = Month[0];
                ObjUBO.VENDOR_NAME = Month[1];
            }
            else
             {
                 ObjUBO.VOUCHER_DATE = "";
                ObjUBO.VENDOR_NAME = "";
            }
        }
        ObjUBO.VENDOR_STATE = ""; //month

        ObjUBO.ENAME = "SELECT_GRID_SEARCH";
        ObjUBO.BRANCH_CODE = Connection.Current_Branch();
        ObjUBO.GSTN_TYPE = ddl_Gst_type.SelectedValue;
        ds = BI.PurchaseEntry_DebitNote(ObjUBO);
         if (ds.Tables[0].Rows.Count > 0)
        {
            gvdetails.DataSource = ds.Tables[0];
        }
        else
        {
            gvdetails.DataSource = dt;
        }
        gvdetails.DataBind();
    }
    protected void gvdetails_RowDataBound(object sender, GridViewRowEventArgs e)
    {

        string jobno = string.Empty;
        string rowID = String.Empty;

        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            string PurchaseVoucherNo = this.gvdetails.DataKeys[e.Row.RowIndex]["VOUCHER_ID"].ToString();
            e.Row.Attributes["onmouseover"] = "this.style.cursor='pointer';";
            e.Row.Attributes.Add("ondblclick", "open_PurchaseDebitNote('" + PurchaseVoucherNo + "')");
            rowID = e.Row.RowIndex.ToString();
            e.Row.Attributes.Add("id", "row" + e.Row.RowIndex);
            e.Row.Attributes.Add("onclick", "ChangeRowColor('" + rowID + "','" + PurchaseVoucherNo + "')");
        }
    }
    private void Load_Month()
    {
        DataSet ds = new DataSet();
        ds = Common_CS.Load_Month();

        ddlMonth.DataSource = ds.Tables[0];
        ddlMonth.DataTextField = "TheMonth";
        ddlMonth.DataValueField = "TheMonth_No";
        ddlMonth.DataBind();

        ddlMonth.Items.Insert(ddlMonth.Items.Count, new ListItem(String.Empty, String.Empty));
        ddlMonth.SelectedValue = ds.Tables[1].Rows[0][0].ToString();
    }
    protected void sendmail_Click(object sender, EventArgs e)
    {

        string Mail_Attach;

        Mail_Attach = Server.MapPath("~/Email_files/" + Session["COMPANY_ID"].ToString() + "/" + Session["currentbranch"].ToString() + "/" + Session["currentuser"].ToString() + "/");

        if (Directory.Exists(Mail_Attach) == false)
        {
            Directory.CreateDirectory(Mail_Attach);
        }

        BI.Purchase_Debit_note_XL_Data(txtsearch.Text, txtFdate.Text, txtTodate.Text, ddlMonth.SelectedValue, ddlvoucher.SelectedValue, txtdebitdate.Text, txtdebitno.Text, "");

        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "Func()", true);

    }
}
