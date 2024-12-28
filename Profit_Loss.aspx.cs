using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using BussinessObject;

public partial class Accounts_Profit_Loss : ThemeClass
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
        //currentuser = Session["currentuser"].ToString();
        currentbranch = Session["currentbranch"].ToString();
        Working_Period = Session["WorkingPeriod"].ToString();
        HDBranch.Value = currentbranch;

        hdrowindex.Value = String.Empty;
        hdBill_invno.Value = String.Empty;

        if (!IsPostBack)
        {
            //Load_Month();
            //load_Imp_name();
            //chk();
            btnSearch_Click(sender, e);
            Load_SalesMan();
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
            erm.fromdate =  txtfromdate.Text = start.ToShortDateString();
            erm.todate = txttodate.Text = end.ToShortDateString();


        }
     
            erm.jobno = txtsearch.Text;
            erm.JOBTYPE = ddlType.SelectedValue;
          
                erm.CATEGORY = ddlCategory.SelectedValue;
          
            erm.MODE = ddlMode.SelectedValue;
            erm.BUYER_NAME = ddlSales.SelectedValue;
        //erm.flag = "GRID_SEARCH";
      
        //ObjUBO.A7 = "JOB_GEN";
        dss = erm.RetrieveAll_PROFIT_LOSS_SEARCH();
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
         //Load_Grid();
         //txtsearch.Text = string.Empty;
         
    }
    //}
       

   



 private void Load_Grid()
    {
        DataSet ds1 = new DataSet();
        if (txtsearch.Text != "NULL" && txtsearch.Text != string.Empty)
        {
            erm.jobno = txtsearch.Text;
        }

       else if (txtfromdate.Text != string.Empty || txttodate.Text != string.Empty)
        {
            erm.fromdate= txtfromdate.Text;
            erm.todate = txttodate.Text;
        }

     
        erm.jobno = txtsearch.Text; ;
       
        //erm.flag = "GRID_SEARCH";
      
        ds1 = erm.RetrieveAll_PROFIT_LOSS_SEARCH();
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
            string values = this.gvdetails.DataKeys[e.Row.RowIndex]["JobNo"].ToString();
            e.Row.Attributes["onmouseover"] = "this.style.cursor='pointer';";
            e.Row.Attributes.Add("onclick", "opennew_Profit_Loss_update('" + values + "')");

            var des0 = row.Cells[0].Text.Replace("&amp;", "&");
            var des1 = row.Cells[1].Text.Replace("&amp;", "&");
            var des2 = row.Cells[2].Text.Replace("&amp;", "&");


        }
    }

    //      string jobno = string.Empty;
    //    string rowID = String.Empty;

    //    if (e.Row.RowType == DataControlRowType.DataRow)
    //    {
    //        string JobNo = this.gvdetails.DataKeys[e.Row.RowIndex]["JobNo"].ToString();
    //        string Type = this.gvdetails.DataKeys[e.Row.RowIndex]["Type"].ToString();
    //        string Mode = this.gvdetails.DataKeys[e.Row.RowIndex]["Mode"].ToString();
    //        string Profit = this.gvdetails.DataKeys[e.Row.RowIndex]["Profit"].ToString();
    //        string Lose = this.gvdetails.DataKeys[e.Row.RowIndex]["Lose"].ToString();

    //        e.Row.Attributes["onmouseover"] = "this.style.cursor='pointer';";
    //        e.Row.Attributes.Add("ondblclick", "opennew_Profit_Loss_update('" + JobNo + "','" + Server.UrlEncode(Type) + "','" + Mode + "','" + Profit + "','" + Lose + "')");
            
    //        rowID = e.Row.RowIndex.ToString();
    //        e.Row.Attributes.Add("id", "row" + e.Row.RowIndex);
    //        e.Row.Attributes.Add("onclick", "ChangeRowColor('" + rowID + "','" + JobNo + "','" + Type + "')");
    //    }
    //}
      


    public void AUTO_JOBNO()
    {
        DataSet dss = new DataSet();
       
        erm.fromdate= txtfromdate.Text;
        erm.todate = txttodate.Text;
        erm.jobno= txtsearch.Text;

        dss = erm.RetrieveAll_PROFIT_LOSS_SEARCH();

        //Session["IMP_AIR_Jobno_Excel"] = txtsearch.Text;
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
        
        erm.jobno=txtsearch.Text;
        gvdetails.EmptyDataText = "No Record Found";
        
        erm.startRowIndex = Convert.ToString(currentPageNumber1);
        erm.maximumRows = Convert.ToString(gvdetails.PageSize);
        gvdetails.DataSource = erm.RetrieveAll_PROFIT_LOSS_SEARCH();


        gvdetails.DataBind();

        Grid_Total(currentPageNumber1, erm.result);
      
  


    }
        //Grid_Total(currentPageNumber1,dss.result);
    
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
        DataSet ds = new DataSet();
        //erm.jobno = txtsearch.Text;
        //erm.JOBTYPE = ddlType.SelectedValue;

        //erm.CATEGORY = ddlCategory.SelectedValue;

        //erm.MODE = ddlMode.SelectedValue;
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
}
