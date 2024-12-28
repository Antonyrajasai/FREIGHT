using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using BussinessObject;
using System.IO;
using System.Text;
using System.Data.SqlClient;
using System.Linq;

public partial class GenerateInvoice_Search  : ThemeClass
{

 
    AppSession aps = new AppSession();

    eFreightImport_Transactions Imp = new eFreightImport_Transactions();
    Generate_Invoice gen = new Generate_Invoice();
    DataSet ds = new DataSet();

    protected void Page_Load(object sender, EventArgs e)
    {
        aps.checkSession();
        if (!IsPostBack)
        {

            //if (Session["Value"] == "G")
            //{
            //    Session.Remove("Value");
            //    Response.Redirect("../Accounts/GST_Imp_Exp_Invoice_Job_Search.aspx");
                
            //}
            load_Type();
            ddlTransportMode.SelectedIndex = 0;
            //hdnFreightMode.Value = Request["F_Mode"].ToString();
            //if (hdnFreightMode.Value == "Air")
            //{

            //    //ddlTransportMode.Items.Add(new ListItem("Air", "Air"));
            //    spancontrol.InnerText = "Air-Import Planning";
            //}
            //else if (hdnFreightMode.Value == "Sea")
            //{

            //    //ddlTransportMode.Items.Add(new ListItem("Sea", "Sea"));
            //    spancontrol.InnerText = "Sea-Import Planning";

            //}


            HDBranch.Value = Connection.Get_Company_Type();
            // Tr_mode_load();
            Page_load_Grid();
            Update_BE_Status();

            btnSearch_Click(sender, e);
        }
    }
    private void load_Type()
    {
        ddlTransportMode.Items.Clear();
        ddlTransportMode.Items.Add(new ListItem("FORWARDING", "FORWARDING"));
        ddlTransportMode.Items.Add(new ListItem("CLEARING", "CLEARING"));
        ddlTransportMode.Items.Add(new ListItem("CROSS-COUNTRY", "CROSS-COUNTRY"));
        ddlTransportMode.Items.Add(new ListItem("BOTH", "BOTH"));
        ddlTransportMode.Items.Add(new ListItem("OTHERS", "OTHERS"));
        

        
    }
    private void Page_load_Grid()
    {
        ViewState["AZ"] = "";
        ddlterms_load();
        gen.SEARCH_CAT = "";
        gen.SEARCH_ITEM = "";
        gen.EXPORTERNAME = "";
        gen.TRANSPORT_MODE = ddlTransportMode.SelectedValue.ToString();
        gen.CUSTOMS_HOUSE = "";
        gen.Selected_Index = "1";
        gen.Flag = "G";
        
        ds = gen.Generate_Invoice_General_Search();
        Load_Grid(ds);
    }
    private void Load_Grid(DataSet dss)
    {
        try
        {
            DataTable dt = new DataTable();
            if (ds.Tables[0].Rows.Count > 0)
            {
                gvdetails1.DataSource = ds.Tables[0];
                gvdetails1.DataBind();
                Add_Pageno(gen.result);
            }
            else
            {
                gvdetails1.DataSource = dt;
                gvdetails1.DataBind();
            }
        }
        catch (Exception ex)
        {
            Connection.Error_Msg(ex.Message);
        }
    }
    protected void btnSearch_Click(object sender, EventArgs e)
    {
       
        ViewState["AZ"] = "";
        Load_Search("1");
    }
    private void Load_Search(string Selected_Value)
    {
        try
        {
            DataSet dss = new DataSet();
            if (txtfromdate.Text != string.Empty || txttodate.Text != string.Empty)
            {
                gen.fromdate = txtfromdate.Text;
                gen.todate = txttodate.Text;
            }
            else
            {
                DateTime now = DateTime.Now;
                DateTime start = new DateTime(now.Year, now.Month, 1);
                DateTime end = start.AddMonths(1).AddDays(-1);
                gen.fromdate = txtfromdate.Text = start.ToShortDateString();
                gen.todate = txttodate.Text = end.ToShortDateString();


            }

            //if (ddlTerms.SelectedValue.ToString() != "0" && txtsearch.Text != string.Empty)
            //{
                //if (ddlTerms.SelectedValue.ToString() == "JOB_NO")
                //{
                    gen.SEARCH_CAT = "JOBNO_PS";
                //}
                gen.SEARCH_ITEM = txtsearch.Text;
                //gen.EXPORTERNAME = "";
                gen.TRANSPORT_MODE = ddlTransportMode.SelectedValue.ToString();
                //gen.CUSTOMS_HOUSE = "";
            //}
            //else
            //{
            //    gen.SEARCH_CAT = "";
            //    gen.SEARCH_ITEM = "";
                gen.EXPORTERNAME = txtImporterName.Text;
                //gen.TRANSPORT_MODE = ddlTransportMode.SelectedValue.ToString();
                gen.CUSTOMS_HOUSE = txtCustomsHouse.Text;
            //}
            gen.Selected_Index = Selected_Value;
            ds = gen.Generate_Invoice_General_Search();
            Load_Grid(ds);
        }
        catch (Exception ex)
        {
            Connection.Error_Msg(ex.Message);
        }
    }
    protected void ddlSelectPageno_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (ViewState["AZ"].ToString() != string.Empty)
            {                
                gen.SEARCH_CAT = "";
                gen.SEARCH_ITEM = "";
                gen.EXPORTERNAME = ViewState["AZ"].ToString();
                gen.TRANSPORT_MODE = "";
                gen.CUSTOMS_HOUSE = "";
            }
            else
            {
                if (ddlTerms.SelectedValue != "0" && txtsearch.Text != string.Empty)
                {
                    if (ddlTerms.SelectedValue.ToString() == "JOB_NO")
                    {
                        gen.SEARCH_CAT = "JOBNO_PS";
                    }
                    gen.SEARCH_ITEM = txtsearch.Text;
                    gen.EXPORTERNAME = "";
                    gen.TRANSPORT_MODE = "";
                    gen.CUSTOMS_HOUSE = "";
                }
                else
                {
                    gen.SEARCH_CAT = "";
                    gen.SEARCH_ITEM = "";
                    gen.EXPORTERNAME = txtImporterName.Text;
                    gen.TRANSPORT_MODE = ddlTransportMode.SelectedValue.ToString();
                    gen.CUSTOMS_HOUSE = txtCustomsHouse.Text;
                }
            }

            gen.Selected_Index = ddlSelectPageno.SelectedValue.ToString();
            ds = gen.Generate_Invoice_General_Search();
            DataTable dt = new DataTable();

            if (ds.Tables[0].Rows.Count > 0)
            {
                gvdetails1.DataSource = ds.Tables[0];
                gvdetails1.DataBind();
            }
            else
            {
                gvdetails1.DataSource = dt;
                gvdetails1.DataBind();
            }
        }
        catch (Exception ex)
        {
            Connection.Error_Msg(ex.Message);
        }
    }

    protected void btnSearchAll_Click(object sender, EventArgs e)
    {
        Page_load_Grid();
    }
    protected void btn_A_TO_Z_Search_Click(object sender, EventArgs e)
    {
        try
        {
            txtsearch.Text = string.Empty;
            ddlTerms.SelectedIndex = -1;
            if (hdn_A_to_Z_Search.Value.ToString() != string.Empty)
            {
                ViewState["AZ"] = hdn_A_to_Z_Search.Value.ToString();
                Imp.SEARCH_CAT = "";
                Imp.SEARCH_ITEM = "";
                Imp.IMPORTER_NAME = hdn_A_to_Z_Search.Value.ToString();
                Imp.TRANSPORT_MODE = "";
                Imp.CUSTOMS_HOUSE = "";
                Imp.Selected_Index = "1";
                ds = Imp.Import_Sea_General_Search();
                Load_Grid(ds);
            }
        }
        catch (Exception ex)
        {
            Connection.Error_Msg(ex.Message);
        }
    }
    protected void gvdetails_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            Session.Contents.Remove("TRANSPORT_MODE");
            Session.Contents.Remove("IMPORT_JOBNO");
            Session.Contents.Remove("IMPORT_JOBDATE");
            Session.Contents.Remove("FILING_STATUS");

            string jobno = string.Empty;
            string BE_TYPE = string.Empty;
            string rowID = String.Empty;
            var row = e.Row;
            string Imp_Name = string.Empty;

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                string values = this.gvdetails1.DataKeys[e.Row.RowIndex]["JOBNO_PS"].ToString();
                string type = this.gvdetails1.DataKeys[e.Row.RowIndex]["TYP"].ToString();
                e.Row.Attributes["onmouseover"] = "this.style.cursor='pointer';";
                //e.Row.Attributes.Add("ondblclick", "Import_Update('" + values + "')");

                e.Row.Attributes.Add("ondblclick", "Generate_Grid_Update('" + values + "','" + ddlTransportMode.SelectedValue + "','" + type + "')");

                jobno = this.gvdetails1.DataKeys[e.Row.RowIndex]["JOBNO"].ToString();
                BE_TYPE = this.gvdetails1.DataKeys[e.Row.RowIndex]["BE_TYPE"].ToString();
                Imp_Name = this.gvdetails1.DataKeys[e.Row.RowIndex]["IMPORTER_NAME"].ToString();

                rowID = e.Row.RowIndex.ToString();
                e.Row.Attributes.Add("id", "row" + e.Row.RowIndex);
                e.Row.Attributes.Add("onclick", "ChangeRowColor('" + rowID + "','" + jobno + "','" + BE_TYPE + "' )");

                //var des = row.Cells[27].Text.Replace("&nbsp;", "").Replace("&amp;", "&");
                // row.Cells[5].ToolTip = des;
                row.Cells[5].ToolTip = Imp_Name;

                if (DataBinder.Eval(e.Row.DataItem, "JOB_FILED_STATUS").ToString() == "Created")
                {
                    e.Row.ForeColor = System.Drawing.Color.Black;
                }
                else if (DataBinder.Eval(e.Row.DataItem, "JOB_FILED_STATUS").ToString() == "BE_Sent")
                {
                    e.Row.ForeColor = System.Drawing.Color.Blue;
                    e.Row.Font.Bold = true;
                }
                else if (DataBinder.Eval(e.Row.DataItem, "JOB_FILED_STATUS").ToString() == "Cancelled")
                {
                    e.Row.ForeColor = System.Drawing.Color.Red;
                }
                else if (DataBinder.Eval(e.Row.DataItem, "JOB_FILED_STATUS").ToString() == "BE_Recipt")
                {
                    e.Row.ForeColor = System.Drawing.Color.Green;
                    e.Row.Font.Bold = true;
                }
            }



        }
        catch (Exception ex)
        {
            Connection.Error_Msg(ex.Message);
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

    private void Add_Pageno(int p)
    {
        if (p != 0)
        {
            ddlSelectPageno.Items.Clear();
            for (int i = 1; i <= p; i++)
            {
                ddlSelectPageno.Items.Add(Convert.ToString(i));
            }
        }
    }

    private void ddlterms_load()
    {
        //ddlTerms.Items.Add(new ListItem(" ", "0"));
        ddlTerms.Items.Add(new ListItem("Job No", "JOB_NO"));
        //ddlTerms.Items.Add(new ListItem("Invoice No", "INV_NO"));
        //ddlTerms.Items.Add(new ListItem("Container No", "CONTAINER_NO"));
        ////ddlTerms.Items.Add(new ListItem("MAWB No", "MAWB_NO"));
        ////ddlTerms.Items.Add(new ListItem("HAWB No", "HAWB_NO"));
        ////ddlTerms.Items.Add(new ListItem("Bond No", "BOND_NO"));

        //ddlTerms.Items.Add(new ListItem("BE No", "BE_NO"));
        //ddlTerms.Items.Add(new ListItem("File Ref.no", "FILE_REF_NO"));
        //ddlTerms.Items.Add(new ListItem("Sup. Name", "S_NAME"));
        //ddlTerms.Items.Add(new ListItem("Item Desc", "ITEM_DESC"));
        //ddlTerms.Items.Add(new ListItem("WH To Exbond", "WH_To_Ex_Bond"));
        //ddlTerms.Items.Add(new ListItem("License No", "LICENSE_NO"));
    }
    private void Update_BE_Status()
    {
        Global_variables ObjUBO = new Global_variables();
        //Imp_Exp_Job_Status IEJ = new Imp_Exp_Job_Status();
        //ObjUBO.A3 = "BE_Status_Update";
        //IEJ.Job_Status(ObjUBO);
    }


  
    private void Tr_mode_load()
    {
        string SELECT_TYPE = string.Empty;
        if (Connection.Get_Company_Type() != "")
        {
            SELECT_TYPE = HttpContext.Current.Session["SELECT_TYPE"].ToString();
            if (SELECT_TYPE == "Areawise Job")
            {
                ddlTransportMode.Items.Add(new ListItem("", ""));
                ddlTransportMode.Items.Add(new ListItem("Air", "Air"));
                ddlTransportMode.Items.Add(new ListItem("Sea", "Sea"));
                ddlTransportMode.Items.Add(new ListItem("Land", "Land"));
            }
            else
            {
                string TRANS_MODE = Session["CUSTOMHOUSE_TRANSPORT_MODE"].ToString();
                if (TRANS_MODE == "A")
                {
                    ddlTransportMode.Items.Add(new ListItem("Air", "Air"));
                }
                else if (TRANS_MODE == "S")
                {
                    ddlTransportMode.Items.Add(new ListItem("Sea", "Sea"));
                }
                else if (TRANS_MODE == "L")
                {
                    ddlTransportMode.Items.Add(new ListItem("Land", "Land"));
                }
            }
        }
        else
        {
            ddlTransportMode.Items.Add(new ListItem("", ""));
            ddlTransportMode.Items.Add(new ListItem("Air", "Air"));
            ddlTransportMode.Items.Add(new ListItem("Sea", "Sea"));
            ddlTransportMode.Items.Add(new ListItem("Land", "Land"));
        }
    }
}