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


public partial class Expense_Entry : ThemeClass
{
    AppSession aps = new AppSession();
    Global_variables ObjUBO = new Global_variables();
    GST_Sales_Entry_cs SE = new GST_Sales_Entry_cs();

    public int i;

    protected void Page_Load(object sender, EventArgs e)
    {
        aps.checkSession();

        if (!IsPostBack)
        {
            if (Request.QueryString["Job_no"] != null && Request.QueryString["Job_no"] != string.Empty && Request.QueryString["Type"] != null && Request.QueryString["Type"] != string.Empty)
            {
                DataSet dss = new DataSet();
                ObjUBO.A4 = Request.QueryString["Job_no"].ToString();
                ObjUBO.A7 = Request.QueryString["Type"].ToString();
                ObjUBO.A8 = "Expense_Data_Select";
                dss = SE.Select_Inv(ObjUBO);

                if (dss.Tables[0].Rows.Count > 0)
                {
                    gvAll.DataSource = dss.Tables[0];
                    gvAll.DataBind();
                }
                else
                {
                    DataTable dt = new DataTable();
                    gvAll.DataSource = dt;
                    gvAll.DataBind();
                }
            }

        }
    }

}