using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Diagnostics;
using System.Web;
using System.Linq;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Data.SqlClient;
using System.IO;

//using Port_Load_MasterDetails;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.html.simpleparser;
using BussinessObject;
public partial class Accounts_Cost_Sheet : ThemeClass
{
    eFreightQuotation_Transactions BI = new eFreightQuotation_Transactions();
    Billing_UserBO ObjUBO = new Billing_UserBO();
    Import_Planning Transact = new Import_Planning();
    SqlConnection con;
    SqlCommand cmd;
    DataTable dt = new DataTable();

    DataSet dtuser = new DataSet();
    DataSet dss = new DataSet();
    AppSession aps = new AppSession();

    public string Trans_ID = "", IGMSLNO = "";
    ErrorLogs er = new ErrorLogs();
    public int i, j, k, l;
    public string AUTO_ID;
    string SP_QUERY;


    public string currentuser;
    public string currentbranch;
    public string currentcomp_name;
    public string comp_ConnectionStr;
    public string Working_Period;
    public string LoginDate;
    public string JOBNO, TYPE, JOBNO_PS;
    public string sno = "", flag = "", IGM_SLNO_UPDATE = "", Imp_Cost_Sell_ID = "", CONTAINER_ID = "", CERTIFICATE_ID = "";
    public string USER_RIGHTS_ID, SCREEN_ID, PAGE_READ, PAGE_WRITE, PAGE_MODIFY, PAGE_DELETE, Is_Master_Id;
    public int Screen_Id;
    public string JOB_LOCK, JOB_RELEASE;
    public string SELECT_TYPE, SELECT_BRANCH;

    User_Creation user_Create = new User_Creation();
    protected void Page_Load(object sender, EventArgs e)
    {
        aps.checkSession();

        currentuser = Session["currentuser"].ToString();
        currentbranch = Session["currentbranch"].ToString();
        currentcomp_name = Session["currentcomp_name"].ToString();
        comp_ConnectionStr = Session["currentcomp_db"].ToString();
        Working_Period = Session["WorkingPeriod"].ToString();
        LoginDate = Session["LoginDate"].ToString();
        JOBNO = Session["JOBNO"].ToString();
        TYPE = Session["TYPE"].ToString();
      

        //  Import_HAWBJOBNO = Session["IMP_SEA_CARGOARRIVAL_HAWBJOBNO"].ToString();
        SELECT_BRANCH = Connection.Get_Company_Type();



        if (!IsPostBack)
        {
            //----------------SETTING SCREEN PERMISSION---------------//
            Screen_Id = 7;
           

        }
        Gridbind_BOND();




    }
    protected void gv_CONT_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            //  if Read Permiision given true for gridview enabled or Disabled----------//
            if (hdn_Grid_Disable.Value == "1")
            {

                e.Row.Enabled = false;

            }
            else
            {
                e.Row.Enabled = true;

                e.Row.Attributes["onmouseover"] = "this.style.cursor='pointer';";
                //e.Row.Attributes["onmouseout"] = "this.style.background='white'";
                e.Row.Attributes.Add("onclick", ClientScript.GetPostBackClientHyperlink(this.gv_CONT, "Select$" + e.Row.RowIndex.ToString()));


            }
            //  if Read Permiision given true for gridview enabled or Disabled----------//

        }
    }
    protected void gv_CONT_RowCreated(object sender, GridViewRowEventArgs e)
    {
        //Add CSS class on header row.
        if (e.Row.RowType == DataControlRowType.Header)
            e.Row.CssClass = "header";

        //Add CSS class on normal row.
        if (e.Row.RowType == DataControlRowType.DataRow && e.Row.RowState == DataControlRowState.Normal)
            e.Row.CssClass = "normal";

        //Add CSS class on alternate row.
        if (e.Row.RowType == DataControlRowType.DataRow && e.Row.RowState == DataControlRowState.Alternate)
            e.Row.CssClass = "alternate";
    }
    public void Gridbind_BOND()
    {

        Transact.JOBNO = Request.QueryString["JOBNO"];


        Transact.WORKING_PERIOD = Connection.Current_Branch();

        Transact.TYPE = Request.QueryString["TYPE"];
        if (Request.QueryString["IMP_EXP"].ToString() == "I" && Request.QueryString["MODE"].ToString() == "Air")
        {
            Transact.Ename = "select_Cost";
        }
        else if (Request.QueryString["IMP_EXP"].ToString() == "I" && Request.QueryString["MODE"].ToString() == "Sea")
        {
            Transact.Ename = "select_Cost_Sea";
        }
        else if (Request.QueryString["IMP_EXP"].ToString() == "E" && Request.QueryString["MODE"].ToString() == "Air")
        {
            Transact.Ename = "select_Cost_Exp_Air";
        }
        else
        {
            Transact.Ename = "select_Cost_Exp_Sea";
        }
        dss = Transact.Import_Sea_Planning_Cost_And_Sell_RetrieveAll_Details();

        if (dss.Tables[0].Rows.Count > 0)
        {
            gv_CONT.DataSource = dss.Tables[0];
            gv_CONT.DataBind();
        }
      
       
    }
}

