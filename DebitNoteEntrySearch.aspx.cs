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

public partial class DebitNoteEntrySearch : ThemeClass
{
    User_Creation user_Create = new User_Creation();
    AppSession aps = new AppSession();
    string[] arr = new string[] { };
    string[] Month = new string[] { };

    DebitNote_cs BI = new DebitNote_cs();
    Billing_UserBO ObjUBO = new Billing_UserBO();
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
        ObjUBO.VOUCHER_NO = txtsearch.Text;
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
        ObjUBO.ENAME = "SELECT";
        ObjUBO.BRANCH_CODE = Connection.Current_Branch();
        ObjUBO.GSTN_TYPE = ddl_Gst_type.SelectedValue;
        ds = BI.DebitNoteEntry(ObjUBO);
        gvdetails.DataSource = ds;
        gvdetails.DataBind();
    }
    protected void gvdetails_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        string jobno = string.Empty;
        string rowID = String.Empty;
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            string PurchaseVoucherNo = this.gvdetails.DataKeys[e.Row.RowIndex]["DEBITNOTE_MAIN_ID"].ToString();
            e.Row.Attributes["onmouseover"] = "this.style.cursor='pointer';";
            e.Row.Attributes.Add("ondblclick", "open_DebitNote_Upd('" + PurchaseVoucherNo + "')");

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
}