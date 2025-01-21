using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using BussinessObject;

public partial class Accounts_Accounts_Credit_Note_Search : ThemeClass
{
    AppSession aps = new AppSession();

    Accounts_Credit_Debit BP = new Accounts_Credit_Debit();
    Global_variables ObjUBO = new Global_variables();

    DataTable dt = new DataTable();

    // Billing_UserBO ObjUBO = new Billing_UserBO();

    public string Working_Period, currentbranch;

    string[] arr = new string[] { };
    string[] arr_Month = new string[] { };

    string Start_date = string.Empty, End_date = string.Empty, From_month = string.Empty, To_month = string.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {
        aps.checkSession();
        //currentuser = Session["currentuser"].ToString();
        currentbranch = Session["currentbranch"].ToString();
        Working_Period = Session["WorkingPeriod"].ToString();
        HDBranch.Value = currentbranch;

        hdrowindex.Value = String.Empty;
        hdBill_invno.Value = String.Empty;

        if (!IsPostBack)
        {
            Load_Month();
            load_Imp_name();
            chk();
            btnSearch_Click(sender, e);
        }
    }
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        if (chkqs.Checked == true && ddltype.SelectedItem.Value != "0" && txtsearch.Text != string.Empty)
        {
            DataSet dss = new DataSet();
            if (txtfromdate.Text != string.Empty || txttodate.Text != string.Empty)
            {
                ObjUBO.A1 = txtfromdate.Text;
                ObjUBO.A2 = txttodate.Text;
            }
            else
            {
                if (ddlmonth.SelectedItem.Text != string.Empty)
                {
                    arr_Month = ddlmonth.SelectedValue.ToString().Split('-');
                    ObjUBO.A1 = arr_Month[0];
                    ObjUBO.A2 = arr_Month[1];
                }
                else
                {
                    ObjUBO.A1 = txtfromdate.Text;
                    ObjUBO.A2 = txttodate.Text;
                }
            }

            ObjUBO.A3 = ddlCus_name.SelectedValue.ToString();
            ObjUBO.A4 = ddltype.SelectedValue.ToString();
            ObjUBO.A5 = txtsearch.Text;
            ObjUBO.A6 = ddl_Type.SelectedValue.ToString();
            ObjUBO.A8 = ddlJobstatus.SelectedValue;
            ObjUBO.A7 = "Quick_Search";
            dss = BP.CREDIT_INV_GRID_SEARCH(ObjUBO);
            if (dss.Tables[0].Rows.Count > 0)
            {
                gvdetails.DataSource = dss.Tables[0];
            }
            else
            {
                gvdetails.DataSource = dt;
            }
            gvdetails.DataBind();
        }
        else
        {
            Load_Grid();
            txtsearch.Text = string.Empty;
            chkqs.Checked = false;
            ddltype.SelectedIndex = -1;
        }
        chk();
    }
    private void chk()
    {
        if (chkqs.Checked == true)
        {
            ddltype.Enabled = true;
            txtsearch.Enabled = true;
        }
        else
        {
            ddltype.Enabled = false;
            txtsearch.Enabled = false;
        }
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

    protected void gvdetails_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        string rowID = String.Empty;
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            string ID = this.gvdetails.DataKeys[e.Row.RowIndex]["ID"].ToString();
            string IMP_EXP = this.gvdetails.DataKeys[e.Row.RowIndex]["IMP_EXP"].ToString();
            string Invoice_No = this.gvdetails.DataKeys[e.Row.RowIndex]["CREDIT_NO"].ToString();
            string TYPE = this.gvdetails.DataKeys[e.Row.RowIndex]["TYPE"].ToString();
            //string credit_no = this.gvdetails.DataKeys[e.Row.RowIndex]["CREDIT_NO"].ToString();
            
                    
            e.Row.Attributes["onmouseover"] = "this.style.cursor='pointer';";
            e.Row.Attributes.Add("ondblclick", "open_Account_Credit_Update('" + ID + "','" + IMP_EXP + "','" + Invoice_No + "','" + TYPE + "')");

            rowID = e.Row.RowIndex.ToString();
            e.Row.Attributes.Add("id", "row" + e.Row.RowIndex);
            e.Row.Attributes.Add("onclick", "ChangeRowColor('" + rowID + "','" + ID + "','" + Invoice_No + "','" + IMP_EXP + "')");
        }
    }

    private void load_Imp_name()
    {
        ddlCus_name.Items.Clear();
        DataSet ds = new DataSet();
        ObjUBO.A10 = ddl_Type.SelectedValue.ToString();
        ObjUBO.A8 = "Customer_Name_Search_Select";
      
        ds = BP.Select_Inv_Credit(ObjUBO);
        if (ds.Tables[0].Rows.Count > 0)
        {
            ddlCus_name.DataSource = ds.Tables[0];
            ddlCus_name.DataTextField = "CUS_NAME";
            ddlCus_name.DataValueField = "CUS_NAME";
            ddlCus_name.DataBind();
        }
        ddlCus_name.Items.Insert(0, new ListItem(String.Empty, String.Empty));
        ddlCus_name.SelectedIndex = 0;
    }
    protected void ddlbranch_SelectedIndexChanged(object sender, EventArgs e)
    {
        load_Imp_name();
    }
    private void Load_Grid()
    {
        DataSet ds1 = new DataSet();
        if (txtfromdate.Text != string.Empty || txttodate.Text != string.Empty)
        {
            ObjUBO.A1 = txtfromdate.Text;
            ObjUBO.A2 = txttodate.Text;
        }
        else
        {
            if (ddlmonth.SelectedItem.Text != string.Empty)
            {
                arr_Month = ddlmonth.SelectedValue.ToString().Split('-');
                ObjUBO.A1 = arr_Month[0];
                ObjUBO.A2 = arr_Month[1];
            }
            else
            {
                ObjUBO.A1 = txtfromdate.Text;
                ObjUBO.A2 = txttodate.Text;
            }
        }
        ObjUBO.A3 = ddlCus_name.SelectedValue.ToString();
        ObjUBO.A4 = "";
        ObjUBO.A5 = "";
        ObjUBO.A6 = ddl_Type.SelectedValue.ToString();
        ObjUBO.A8 = ddlJobstatus.SelectedValue;
        ObjUBO.A7 = "Grid_Search";
        ds1 = BP.CREDIT_INV_GRID_SEARCH(ObjUBO);
        if (ds1.Tables[0].Rows.Count > 0)
        {
            gvdetails.DataSource = ds1.Tables[0];
        }
        else
        {
            gvdetails.DataSource = dt;
        }
        gvdetails.DataBind();
    }

    public void Load_Month()
    {
        DataSet ds = new DataSet();
        ds.Clear();
        ds = Common_CS.Load_Month();

        if (ds.Tables[0].Rows.Count > 0)
        {
            ddlmonth.DataSource = ds.Tables[0];
            ddlmonth.DataTextField = "TheMonth";
            ddlmonth.DataValueField = "TheMonth_No";
            ddlmonth.DataBind();

            ddlmonth.Items.Insert(ddlmonth.Items.Count, new ListItem(String.Empty, String.Empty));
            ddlmonth.SelectedValue = ds.Tables[1].Rows[0][0].ToString();
        }
    }

    protected void ddl_Type_SelectedIndexChanged(object sender, EventArgs e)
    {
        load_Imp_name();
    }

    protected void ddlmonth_SelectedIndexChanged(object sender, EventArgs e)
    {
        DataSet ds = new DataSet();
        ds = Common_CS.Load_Month();
       
        string a = ddlmonth.SelectedValue.ToString();
        arr_Month = ddlmonth.SelectedValue.ToString().Split('-');
        if (ddlmonth.SelectedItem.Text != string.Empty)
        {

            txtfromdate.Text = arr_Month[0];
            txttodate.Text = arr_Month[1];

        }
        else
        {
            ObjUBO.A1 = "";
            ObjUBO.A2 = "";
        }

        ddlmonth.SelectedValue = a;

    }
    
}