using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System;
using GeneraReport;
using System.Data;
using System.Data.SqlClient;
using Microsoft.Reporting.WebForms;
using System.Reflection;



public partial class Acc_Reports_GSTR3B_IFrame : System.Web.UI.Page
{
    DataSet ds = new DataSet();
    DataTable dt = new DataTable();
    AppSession aps = new AppSession();
    Expense_Entry_cs BI = new Expense_Entry_cs();

    protected void Page_Load(object sender, EventArgs e)
    {
           aps.checkSession();

            if (!IsPostBack)
            {
                string ReportName = string.Empty;
                ReportName = "GSTR3B";
                BI.Flag = "XL_SEARCH";
                string From_date = Request["FromDate"].ToString();
                string To_date = Request["ToDate"].ToString();
                string Month = Request["MONTH"].ToString();
                if (From_date != string.Empty || To_date != string.Empty)
                {
                    BI.From_date = From_date;
                    BI.To_date = To_date;
                }
                else
                {
                    if (Month != string.Empty)
                    {
                        string[] arr_Month = new string[] { };
                        arr_Month = Month.Split('-');
                        BI.From_date = arr_Month[0];
                        BI.To_date = arr_Month[1];
                    }
                    else
                    {
                        BI.From_date = From_date;
                        BI.To_date = To_date;
                    }
                }
                ds = BI.Select_GSTR3B_Search();
                if (ds.Tables[0].Rows.Count > 0)
                {
                    dt = ds.Tables[0];
                }

                if (dt.Rows.Count > 0)
                {
                    ReportGenerator gen = new ReportGenerator(dt, ReportName);
                    ReportDataSource dss = new ReportDataSource(ReportName, dt);
                    ReportViewer1.Reset();
                    ReportViewer1.LocalReport.DataSources.Add(dss);
                    ReportViewer1.LocalReport.DisplayName = ReportName;
                    LocalReport localReport = ReportViewer1.LocalReport;
                    localReport.ReportPath = @"Acc_Reports/GSTR3B.rdlc";
                    ReportViewer1.LocalReport.EnableExternalImages = true;
                    ReportDataSource datasource = new ReportDataSource("DataSet1", ds.Tables[0]);
                    ReportViewer1.LocalReport.DataSources.Add(datasource);
                    ReportViewer1.LocalReport.LoadReportDefinition(gen.GeneraReport());

                }
                else
                {
                    ltmsg.Text = "NO RECORD FOUND";
                }
            }
        }


    protected void ReportViewer1_PreRender(object sender, EventArgs e)
    {
        DisableUnwantedExportFormats(ReportViewer1.LocalReport);
    }
    

    public static void DisableUnwantedExportFormats(LocalReport rvServer)
    {
        foreach (RenderingExtension extension in rvServer.ListRenderingExtensions())
        {
            if (extension.Name == "WORD")
            {
                ReflectivelySetVisibilityFalse(extension);
            }
        }
    }

    public static void ReflectivelySetVisibilityFalse(RenderingExtension extension)
    {
        FieldInfo info = extension.GetType().GetField("m_isVisible", BindingFlags.NonPublic | BindingFlags.Instance);

        if (info != null)
        {
            info.SetValue(extension, false);
        }
    }
 }