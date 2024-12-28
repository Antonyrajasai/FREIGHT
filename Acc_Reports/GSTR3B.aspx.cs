using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using BussinessObject;

public partial class Acc_Reports_GSTR3B : ThemeClass
{
    AppSession aps = new AppSession();

    Journal_Entry_cs JE = new Journal_Entry_cs();
    Global_variables ObjUBO = new Global_variables();

    DataTable dt = new DataTable();
    DataSet ds = new DataSet();

    string[] arr = new string[] { };
    string[] arr_Month = new string[] { };

    protected void Page_Load(object sender, EventArgs e)
    {
        aps.checkSession();       
        
        if (!IsPostBack)
        {
            Load_Month();
            btnSearch_Click(sender, e);
            iframe1.Visible = false;
        }
    }
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        iframe1.Visible = true;
        iframe1.Attributes["src"] = "GSTR3B_IFrame.aspx?MONTH=" + ddlmonth.SelectedValue.ToString() + "&FromDate=" + txtfromdate.Text + "&ToDate=" + txttodate.Text + "";
        //Load_Grid();
    } 


  
    private void Load_Month()
    {
        DataSet ds = new DataSet();
        ds = Common_CS.Load_Month();
        ddlmonth.DataSource = ds.Tables[0];
        ddlmonth.DataTextField = "TheMonth";
        ddlmonth.DataValueField = "TheMonth_No";
        ddlmonth.DataBind();
        ddlmonth.Items.Insert(ddlmonth.Items.Count, new ListItem(String.Empty, String.Empty));
        ddlmonth.SelectedValue = ds.Tables[1].Rows[0][0].ToString();
    }
    //private void Load_Grid()
    //{
    //    DataSet dss = new DataSet();
    //    if (txtfromdate.Text != string.Empty || txttodate.Text != string.Empty)
    //    {
    //        ObjUBO.A1 = txtfromdate.Text;
    //        ObjUBO.A2 = txttodate.Text;
    //    }
    //    else
    //    {
    //        if (ddlmonth.SelectedItem.Text != string.Empty)
    //        {
    //            arr_Month = ddlmonth.SelectedValue.ToString().Split('-');
    //            ObjUBO.A1 = arr_Month[0];
    //            ObjUBO.A2 = arr_Month[1];
    //        }
    //        else
    //        {
    //            ObjUBO.A1 = txtfromdate.Text;
    //            ObjUBO.A2 = txttodate.Text;
    //        }
    //    }

    //    ObjUBO.A7 = "Load_Grid_Data";
    //    dss = JE.Journal_Entry_Search(ObjUBO);
    //    if (dss.Tables[0].Rows.Count > 0)
    //    {
    //        gvdetails.DataSource = dss.Tables[0];
    //    }
    //    else
    //    {
    //        gvdetails.DataSource = dt;
    //    }
    //    gvdetails.DataBind();

    //}
    
}