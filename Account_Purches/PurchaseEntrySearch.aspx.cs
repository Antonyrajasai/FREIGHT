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

public partial class PurchaseEntrySearch : ThemeClass
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
             btnSearch_Click(sender, e);
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
       
       // ObjUBO.VOUCHER_NO = txtsearch.Text;
        if (ddlvoucher.SelectedValue == "Voucher_No")
        {
            ObjUBO.VOUCHER_NO = txtsearch.Text;
            ObjUBO.VENDOR_BILL = "";
            ObjUBO.GSTN = "";
        }
        else if (ddlvoucher.SelectedValue == "Vendor_Name")
        {
            ObjUBO.GSTN = txtsearch.Text;
            ObjUBO.VENDOR_BILL = "";
            ObjUBO.VOUCHER_NO = "";
        }
        else if (ddlvoucher.SelectedValue == "Vendor_Bill_No")
        {
            ObjUBO.VENDOR_BILL = txtsearch.Text;
            ObjUBO.GSTN = "";
            ObjUBO.VOUCHER_NO = "";
        }
        else
        {
            ObjUBO.VOUCHER_NO = "";
            ObjUBO.GSTN = "";
            ObjUBO.VENDOR_BILL = "";
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
                txtFdate.Text = Month[0];
                txtTodate.Text = Month[1];
                ObjUBO.VOUCHER_DATE = txtFdate.Text;
                ObjUBO.VENDOR_NAME = txtTodate.Text;
            }
            else
             {
                 ObjUBO.VOUCHER_DATE = "";
                ObjUBO.VENDOR_NAME = "";
            }
        }
        ObjUBO.VENDOR_STATE = ""; //month
        ObjUBO.IMP_EXP = ddl_cattype.SelectedValue;
        ObjUBO.TYPE = ddl_Type.SelectedValue;
        ObjUBO.MODE = ddl_mode.SelectedValue;
        ObjUBO.ENAME = "SELECT_GRID_SEARCH";
        ObjUBO.BRANCH_CODE = Connection.Current_Branch();
        ObjUBO.GSTN_TYPE = ddl_Gst_type.SelectedValue;
       
        ds = BI.PurchaseEntry(ObjUBO);
        if (ds.Tables.Count > 0)
        {
            if (ds.Tables[0].Rows.Count > 0)
            {
                gvdetails.DataSource = ds.Tables[0];
                gvdetails.DataBind();
            }
            else
            {
                gvdetails.DataSource = dt;
                gvdetails.DataBind();
            }
        }
        else
        {
            gvdetails.DataSource = dt;
            gvdetails.DataBind();
        }
        //gvdetails.DataBind();
    }
    protected void gvdetails_RowDataBound(object sender, GridViewRowEventArgs e)
    {

        string jobno = string.Empty;
        string rowID = String.Empty;

        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            string PurchaseVoucherNo = this.gvdetails.DataKeys[e.Row.RowIndex]["VOUCHER_ID"].ToString();
            e.Row.Attributes["onmouseover"] = "this.style.cursor='pointer';";
            e.Row.Attributes.Add("ondblclick", "open_PurchaseUpd('" + PurchaseVoucherNo + "')");
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



    protected void ddlMonth_SelectedIndexChanged(object sender, EventArgs e)
    {
        DataSet ds = new DataSet();
        ds = Common_CS.Load_Month();

        string a = ddlMonth.SelectedValue.ToString();
        Month = ddlMonth.SelectedValue.ToString().Split('-');
        if (ddlMonth.SelectedItem.Text != string.Empty)
        {

            txtFdate.Text = Month[0];
            txtTodate.Text = Month[1];

        }
        else
        {
            txtFdate.Text = "";
            txtTodate.Text = "";
            //ObjUBO.VOUCHER_DATE = "";
            //ObjUBO.VENDOR_NAME = "";
        }
       
        ddlMonth.SelectedValue = a;
    
    }
    
}