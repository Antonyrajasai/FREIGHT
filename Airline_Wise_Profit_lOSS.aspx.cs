using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using BussinessObject;

public partial class Accounts_Airline_Wise_Profit_lOSS : System.Web.UI.Page
{
    AppSession aps = new AppSession();

    GST_Imp_Invoice BP = new GST_Imp_Invoice();
    Global_variables ObjUBO = new Global_variables();
    eroyalmaster erm = new eroyalmaster();
    DataTable dt = new DataTable();

    // Billing_UserBO ObjUBO = new Billing_UserBO();

    public string Working_Period, currentbranch;

    string[] arr = new string[] { };
    string[] arr_Month = new string[] { };

    string Start_date = string.Empty, End_date = string.Empty, From_month = string.Empty, To_month = string.Empty;



    protected void Page_Load(object sender, EventArgs e)
    {
        aps.checkSession();
        currentbranch = Session["currentbranch"].ToString();
        Working_Period = Session["WorkingPeriod"].ToString();
        HDBranch.Value = currentbranch;

        hdrowindex.Value = String.Empty;
        hdBill_invno.Value = String.Empty;

        if (!IsPostBack)
        {
            btnSearch_Click(sender, e);

        }
    }
    protected void btnSearch_Click(object sender, EventArgs e)
    {

        DataSet dss = new DataSet();
        if (txtfromdate.Text != string.Empty || txttodate.Text != string.Empty)
        {
            erm.fromdate = txtfromdate.Text;
            erm.todate = txttodate.Text;
        }
        else
        {
            DateTime now = DateTime.Now;
            DateTime start = new DateTime(now.Year, now.Month, 1);
            DateTime end = start.AddMonths(1).AddDays(-1);
            erm.fromdate = txtfromdate.Text = start.ToShortDateString();
            erm.todate = txttodate.Text = end.ToShortDateString();


        }

        erm.jobno = "";
        erm.JOBTYPE = ddlType.SelectedValue;

        erm.CATEGORY = ddlCategory.SelectedValue;

        erm.MODE = ddlMode.SelectedValue;
        erm.SearchCat = ddlSearch.SelectedValue;
        erm.BUYER_NAME = txtSales.Text;

        dss = erm.RetrieveAll_SALES_PERSON_PROFIT_LOSS();
        if (dss.Tables.Count > 0)
        {
            if (dss.Tables[1].Rows.Count > 0)
            {
                gvdetails.DataSource = dss.Tables[1];
            }
            else
            {
                gvdetails.DataSource = dt;
            }
        }
        else { gvdetails.DataSource = dt; }
        if (ddlSearch.SelectedValue == "Airline")
        {
            gvdetails.Columns[2].Visible = false;
            gvdetails.Columns[3].Visible = false;
            gvdetails.Columns[4].Visible = false;
            gvdetails.Columns[5].Visible = false;
            gvdetails.Columns[6].Visible = true;
            gvdetails.Columns[7].Visible = false;
            gvdetails.Columns[8].Visible = false;
        }
        else
        {
            gvdetails.Columns[2].Visible = false;
            gvdetails.Columns[3].Visible = false;
            gvdetails.Columns[4].Visible = false;
            gvdetails.Columns[5].Visible = false;
            gvdetails.Columns[6].Visible = false;
            gvdetails.Columns[7].Visible = true;
            gvdetails.Columns[8].Visible = false;
           
        }
        gvdetails.DataBind();


    }







    private void Load_Grid()
    {
        DataSet ds1 = new DataSet();
        if (txtSales.Text != "NULL" && txtSales.Text != string.Empty)
        {
            erm.BUYER_NAME = txtSales.Text;
        }

        else if (txtfromdate.Text != string.Empty || txttodate.Text != string.Empty)
        {
            erm.fromdate = txtfromdate.Text;
            erm.todate = txttodate.Text;
        }


        erm.BUYER_NAME = txtSales.Text;

        ds1 = erm.RetrieveAll_SALES_PERSON_PROFIT_LOSS();
        if (ds1.Tables[1].Rows.Count > 0)
        {
            gvdetails.DataSource = ds1;
            gvdetails.DataBind();
        }
        else
        {
            gvdetails.DataSource = dt;
            gvdetails.DataBind();
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


        var row = e.Row;
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            string jobNo = DataBinder.Eval(e.Row.DataItem, "JOBNO").ToString();
            string values = this.gvdetails.DataKeys[e.Row.RowIndex]["JOBNO"].ToString();
            e.Row.Attributes["onmouseover"] = "this.style.cursor='pointer';";
            e.Row.Attributes.Add("onclick", "opennew_Profit_Loss_update('" + values + "')");

            var des0 = row.Cells[0].Text.Replace("&amp;", "&");
            var des1 = row.Cells[1].Text.Replace("&amp;", "&");
            var des2 = row.Cells[2].Text.Replace("&amp;", "&");


        }


    }

    public void AUTO_JOBNO()
    {
        DataSet dss = new DataSet();

        erm.fromdate = txtfromdate.Text;
        erm.todate = txttodate.Text;
        //erm.jobno = txtsearch.Text;
        erm.BUYER_NAME = txtSales.Text;
        dss = erm.RetrieveAll_SALES_PERSON_PROFIT_LOSS();

    }

    protected void txtsearch_TextChanged(object sender, EventArgs e)
    {
        AUTO_JOBNO();
    }



    public void gridbind(int currentPageNumber1)
    {
        eroyalmaster erm = new eroyalmaster();
        DataSet dss = new DataSet();

        erm.jobno = "0";

        erm.BUYER_NAME = txtSales.Text;
        gvdetails.EmptyDataText = "No Record Found";

        erm.startRowIndex = Convert.ToString(currentPageNumber1);
        erm.maximumRows = Convert.ToString(gvdetails.PageSize);
        gvdetails.DataSource = erm.RetrieveAll_SALES_PERSON_PROFIT_LOSS();
        //if (ddlSearch.SelectedValue == "Sales By")
        //{

        //    gvdetails.Columns[1].Visible = true; 
        //    gvdetails.Columns[2].Visible = false; 
        //}
        //else if (ddlSearch.SelectedValue == "Client By")
        //{

        //    gvdetails.Columns[1].Visible = false; 
        //    gvdetails.Columns[2].Visible = true; 
        //}

        gvdetails.DataBind();

        Grid_Total(currentPageNumber1, erm.result);





    }

    public void Grid_Total(int currentPageNumber1, double totalRows)
    {
        if (gvdetails.Rows.Count > 0)
        {

        }
        else if (gvdetails.Rows.Count == 0)
        {

        }

    }


}
