using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using BussinessObject;

public partial class Accounts_SalesMan_Report : System.Web.UI.Page
{
    AppSession aps = new AppSession();

    GST_Imp_Invoice BP = new GST_Imp_Invoice();
    Global_variables ObjUBO = new Global_variables();
    eroyalmaster erm = new eroyalmaster();
    DataTable dt = new DataTable();

    public string Working_Period, currentbranch;

    string[] arr = new string[] { };
    string[] arr_Month = new string[] { };

    string Start_date = string.Empty, End_date = string.Empty, From_month = string.Empty, To_month = string.Empty;



    protected void Page_Load(object sender, EventArgs e)
    {
        try
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
                Load_SalesMan();

            }
        }
        catch (Exception ex)
        {
            Page.ClientScript.RegisterStartupScript(this.GetType(), "ErrorAlert", "alert('An Error has been Detected In This Process');", true);
        }
    }
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        try
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
            erm.SearchCat = ddlSearch.SelectedValue.ToString();
            erm.BUYER_NAME = ddlSales.SelectedValue.ToString();
            erm.flag = "SALES_MAN";
            dss = erm.RetrieveAll_SalesMan_Customer_Report();
            if (dss.Tables.Count > 0)
            {
                if (dss.Tables[0].Rows.Count > 0)
                {
                    gvdetails.DataSource = dss.Tables[0];
                }
                else
                {
                    gvdetails.DataSource = dt;
                }
            }
            else { gvdetails.DataSource = dt; }

            gvdetails.DataBind();
            Load_Grid();
        }
        catch (Exception ex)
        {
            Page.ClientScript.RegisterStartupScript(this.GetType(), "ErrorAlert", "alert('An Error has been Detected In This Process');", true);
        }
    }

    private void Load_Grid()
    {
        try
        {
            DataSet ds1 = new DataSet();
            if (ddlSales.SelectedValue.ToString() != "NULL" && ddlSales.SelectedValue.ToString() != string.Empty)
            {
                erm.BUYER_NAME = ddlSales.SelectedValue.ToString();
            }

            else if (txtfromdate.Text != string.Empty || txttodate.Text != string.Empty)
            {
                erm.fromdate = txtfromdate.Text;
                erm.todate = txttodate.Text;
            }


            erm.BUYER_NAME = ddlSales.SelectedValue.ToString();

            ds1 = erm.RetrieveAll_SalesMan_Customer_Report();
            if (ds1.Tables[0].Rows.Count > 0)
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
        catch (Exception ex)
        {
            Page.ClientScript.RegisterStartupScript(this.GetType(), "ErrorAlert", "alert('An Error has been Detected In This Process');", true);
        }

    }


    protected void gvdetails_RowCreated(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.Header)
                e.Row.CssClass = "header";

            if (e.Row.RowType == DataControlRowType.DataRow && e.Row.RowState == DataControlRowState.Normal)
                e.Row.CssClass = "normal";

            if (e.Row.RowType == DataControlRowType.DataRow && e.Row.RowState == DataControlRowState.Alternate)
                e.Row.CssClass = "alternate";
        }

        catch (Exception ex)
        {
            Page.ClientScript.RegisterStartupScript(this.GetType(), "ErrorAlert", "alert('An Error has been Detected In This Process');", true);
        }
    }

    protected void gvdetails_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
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
                var des3 = row.Cells[3].Text.Replace("&amp;", "&");



                if (des3 == "1")
                {
                    row.Cells[3].Text = "Active";
                    row.Cells[3].ToolTip = "Active";
                }
                else if (des3 == "0")
                {
                    row.Cells[3].Text = "In Active";
                    row.Cells[3].ToolTip = "In Active";
                }
            }
        }
        catch (Exception ex)
        {
            Page.ClientScript.RegisterStartupScript(this.GetType(), "ErrorAlert", "alert('An Error has been Detected In This Process');", true);
        }
        

    }

   
   


    public void gridbind(int currentPageNumber1)
    {
        try
        {
            eroyalmaster erm = new eroyalmaster();
            DataSet dss = new DataSet();

            erm.jobno = "0";

            erm.BUYER_NAME = ddlSales.SelectedValue.ToString();
            gvdetails.EmptyDataText = "No Record Found";

            erm.startRowIndex = Convert.ToString(currentPageNumber1);
            erm.maximumRows = Convert.ToString(gvdetails.PageSize);
            gvdetails.DataSource = erm.RetrieveAll_SalesMan_Customer_Report();
            gvdetails.DataBind();

            Grid_Total(currentPageNumber1, erm.result);

        }
        catch (Exception ex)
        {
            Page.ClientScript.RegisterStartupScript(this.GetType(), "ErrorAlert", "alert('An Error has been Detected In This Process');", true);
        }



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

    private void Load_SalesMan()
    {
        try
        {
        DataSet ds = new DataSet();
        erm.flag = "LOAD_SALES_PERSION";
        ds = erm.RetrieveAll_PROFIT_LOSS_SEARCH();
        if (ds.Tables[1].Rows.Count > 0)
        {
            ddlSales.DataSource = ds.Tables[1];
            ddlSales.DataTextField = "SALESMAN_NAME";
            ddlSales.DataValueField = "SALESMAN_NAME";
            ddlSales.DataBind();
        }
        ddlSales.Items.Insert(0, new ListItem("", ""));
        ddlSales.SelectedIndex = 0;
        }
        catch (Exception ex)
        {
            Page.ClientScript.RegisterStartupScript(this.GetType(), "ErrorAlert", "alert('An Error has been Detected In This Process');", true);
        }

    }
}
