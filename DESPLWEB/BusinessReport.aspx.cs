using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace DESPLWEB
{
    public partial class BusinessReport : System.Web.UI.Page
    {
        LabDataDataContext dc = new LabDataDataContext();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ViewState["RefUrl"] = Request.UrlReferrer.ToString();
                Label lblheading = (Label)Master.FindControl("lblheading");
                lblheading.Text = "Business Report";
                //txtFromDate.Text = DateTime.Today.AddYears(-1).ToString("yyyy");
                //txtToDate.Text = DateTime.Today.ToString("yyyy");
                txtFromDate.Text = DateTime.Today.AddDays(-DateTime.Today.Day).AddDays(1).ToString("dd/MM/yyyy");
                txtToDate.Text = DateTime.Today.ToString("dd/MM/yyyy");
                //bool userRight = false;
                if (Session["LoginId"] == null)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "Showalert", "alert('Your Session has been Expired !' +'\\n'+ 'Login Again !');", true);
                    Response.Redirect("Login.aspx");
                }
                //else
                //{
                //    var user = dc.User_View(Convert.ToInt32(Session["LoginId"].ToString()), 0, "", "", "");
                //    foreach (var u in user)
                //    {
                //        if (u.USER_SuperAdmin_right_bit == true)
                //            userRight = true;
                //    }
                //    if (userRight == false)
                //    {
                //        pnlContent.Visible = false;
                //        lblAccess.Visible = true;
                //        lblAccess.Text = "Access is Denied.. ";
                //    }
                //}
            }
        }

        protected void lnkDisplay_Click(object sender, EventArgs e)
        {
            bool validData = true;
            string mySql = "";  

//            string[] strDate = txtFromDate.Text.Split('/');
            //DateTime FromDate = new DateTime(Convert.ToInt32(strDate[1]), Convert.ToInt32(strDate[0]), 1);
            //DateTime ToDate = new DateTime(Convert.ToInt32(strDate[1]), Convert.ToInt32(strDate[0]), DateTime.DaysInMonth(Convert.ToInt32(strDate[1]), Convert.ToInt32(strDate[0])));
            
            DateTime Fromdate = DateTime.ParseExact(txtFromDate.Text, "dd/MM/yyyy", null);
            DateTime Todate = DateTime.ParseExact(txtToDate.Text, "dd/MM/yyyy", null);
            string strFromDate = Fromdate.ToString("MM/dd/yyyy");
            string strToDate = Todate.ToString("MM/dd/yyyy");

            if (validData == true)
            {
                DataTable dt = new DataTable();
                DataTable dt1 = new DataTable();
                decimal totSale = 0;
                dt.Columns.Add(new DataColumn("SrNo", typeof(string)));
                dt.Columns.Add(new DataColumn("MaterialType", typeof(string)));
                dt.Columns.Add(new DataColumn("Material", typeof(string)));
                dt.Columns.Add(new DataColumn("Sales(Excluding.Tax)", typeof(decimal)));
                clsData obj = new clsData();
                dt1 = obj.getGeneralData("select MATERIAL_Name_var,MATERIAL_RecordType_var from tbl_Material order by MATERIAL_Name_var ");
                DataRow dr = null;
                
                for (int i = 0; i < dt1.Rows.Count; i++)
                {
                    dr = dt.NewRow();
                    dr["SrNo"] = (i + 1).ToString();
                    dr["MaterialType"] = dt1.Rows[i]["MATERIAL_RecordType_var"];
                    dr["Material"] = dt1.Rows[i]["MATERIAL_Name_var"];
                    dr["Sales(Excluding.Tax)"] =  0;
                    dt.Rows.Add(dr);
                }
                dt1.Dispose();
                // Non Monthly + OT
                #region 
                mySql = @"select b.BILL_RecordType_var  ,sum(a.BILLD_Amt_num) as SalesAmt
                            from tbl_Bill as b  inner join tbl_BillDetail a on 
                          a.BILLD_BILL_Id = b.BILL_Id  and b.BILL_Status_bit = 0 
                            and BILL_RecordType_var <>'Monthly' 
                           and  BILL_Date_dt >=convert(date,'" + strFromDate+ "')" +
                         " and BILL_Date_dt <= convert(date,'" +strToDate + "') " +
                         " group by b.BILL_RecordType_var  ";                
                dt1 = obj.getGeneralData(mySql );
                mySql = "";
                for (int i = 0; i < dt1.Rows.Count ; i++)
                {
                    mySql = dt1.Rows[i]["BILL_RecordType_var"].ToString();
                    if (mySql == "---")
                        mySql = "CT";
                    for (int j = 0; j < dt.Rows.Count ; j++)
                    {
                        if (dt.Rows[j]["MaterialType"].ToString() == mySql)
                        {
                            dt.Rows[j]["Sales(Excluding.Tax)"] = Convert.ToDecimal(dt.Rows[j]["Sales(Excluding.Tax)"].ToString()) + Convert.ToDecimal(dt1.Rows[i]["SalesAmt"].ToString());
                            //totSale += Convert.ToDecimal(dt1.Rows[i]["SalesAmt"].ToString());
                            break;
                        }
                    }
                }
                dt1.Dispose();
                #endregion
                // Monthly - with OT
                #region
                
                mySql = @"select INWD_RecordType_var, sum(a.BILLD_Amt_num)  as SalesAmt
                        from tbl_BillDetail as a, tbl_Bill as b, tbl_Inward as c
                         where a.BILLD_BILL_Id = b.BILL_Id and b.BILL_RecordType_var = 'Monthly' and b.BILL_Status_bit = 0
                          and  a.BILLD_ReferenceNo_int =c.INWD_ReferenceNo_int 
                           and  BILL_Date_dt >=convert(date,'" + strFromDate + "')" + 
                          " and BILL_Date_dt <= convert(date,'" + strToDate + "') " +
                         " group by c.INWD_RecordType_var ";
                
                dt1 = obj.getGeneralData(mySql);
                mySql = "";
                for (int i = 0; i < dt1.Rows.Count; i++)
                {
                    mySql = dt1.Rows[i]["INWD_RecordType_var"].ToString();
                    for (int j = 0; j < dt.Rows.Count; j++)
                    {
                        if (dt.Rows[j]["MaterialType"].ToString() == mySql)
                        {
                            dt.Rows[j]["Sales(Excluding.Tax)"] = Convert.ToDecimal(dt.Rows[j]["Sales(Excluding.Tax)"].ToString()) + Convert.ToDecimal(dt1.Rows[i]["SalesAmt"].ToString());
                          //  totSale += Convert.ToDecimal(dt1.Rows[i]["SalesAmt"].ToString());
                            break;
                        }
                    }
                }
                dt1.Dispose();
                #endregion

                //Other Monthly
                #region Other 
                decimal lessOTSale = 0;
                mySql = @"select d.OTINWD_Section_var , sum(a.BILLD_Amt_num)  as SalesAmt
                        from tbl_BillDetail as a, tbl_Bill as b, tbl_Inward as c,tbl_Other_Inward  d
                        where a.BILLD_BILL_Id = b.BILL_Id and b.BILL_RecordType_var = 'Monthly'
                        and c.INWD_RecordNo_int=d.OTINWD_RecordNo_int and c.INWD_RecordType_var=d.OTINWD_RecordType_var 
                        and b.BILL_Status_bit = 0  and  BILL_Date_dt >=convert(date,'" + strFromDate + "')" +
                        " and BILL_Date_dt <= convert(date,'" + strToDate + "') " +
                        " and c.INWD_ReferenceNo_int = a.BILLD_ReferenceNo_int  and c.INWD_RecordType_var = 'OT' " +
                        " group by d.OTINWD_Section_var ";

                dt1 = obj.getGeneralData(mySql);
                mySql = "";
                Boolean flgFound;
                for (int i = 0; i < dt1.Rows.Count; i++)
                {
                    mySql = dt1.Rows[i]["OTINWD_Section_var"].ToString();
                    flgFound = false;
                    for (int j = 0; j < dt.Rows.Count; j++)
                    {
                        if (dt.Rows[j]["MaterialType"].ToString() == mySql)
                        {
                            dt.Rows[j]["Sales(Excluding.Tax)"] = Convert.ToDecimal(dt.Rows[j]["Sales(Excluding.Tax)"].ToString()) + Convert.ToDecimal(dt1.Rows[i]["SalesAmt"].ToString());
                            lessOTSale += Convert.ToDecimal(dt1.Rows[i]["SalesAmt"].ToString());
                            flgFound = true;
                            break;
                        }
                    }

                }
                dt1.Dispose();
                #endregion
                // OT - Non Monthly
                #region Other Non Monthly

                mySql = @"select d.OTINWD_Section_var , sum(a.BILLD_Amt_num)  as SalesAmt
                        from tbl_BillDetail as a, tbl_Bill as b, tbl_Inward as c,tbl_Other_Inward  d
                        where a.BILLD_BILL_Id = b.BILL_Id and b.BILL_RecordType_var = 'OT'
                        and ( b.BILL_RecordNo_int =c.INWD_RecordNo_int  or b.BILL_RecordNo_int =0)
                        and c.INWD_RecordType_var=b.BILL_RecordType_var  and  c.INWD_MonthlyBill_bit=0
                        and b.BILL_RecordNo_int=d.OTINWD_RecordNo_int  and b.BILL_Status_bit = 0                        
                        and   BILL_Date_dt >=convert(date,'" + strFromDate + "')" +
                        " and BILL_Date_dt <= convert(date,'" + strToDate + "') " +
                        " group by d.OTINWD_Section_var ";

                dt1 = obj.getGeneralData(mySql);
                mySql = "";
                for (int i = 0; i < dt1.Rows.Count; i++)
                {
                    mySql = dt1.Rows[i]["OTINWD_Section_var"].ToString();
                    flgFound = false;
                    for (int j = 0; j < dt.Rows.Count; j++)
                    {
                        if (dt.Rows[j]["MaterialType"].ToString() == mySql)
                        {
                            dt.Rows[j]["Sales(Excluding.Tax)"] = Convert.ToDecimal(dt.Rows[j]["Sales(Excluding.Tax)"].ToString()) + Convert.ToDecimal(dt1.Rows[i]["SalesAmt"].ToString());
                            lessOTSale += Convert.ToDecimal(dt1.Rows[i]["SalesAmt"].ToString());
                            flgFound = true;
                            break;
                        }
                    }
                    //if (flgFound == false)
                    //{
                    //    mySql = "OT";
                    //    for (int j = 0; j < dt.Rows.Count; j++)
                    //    {
                    //        if (dt.Rows[j]["MaterialType"].ToString() == mySql)
                    //        {
                    //            dt.Rows[j]["Sales(Excluding.Tax)"] = Convert.ToDecimal(dt.Rows[j]["Sales(Excluding.Tax)"].ToString()) + Convert.ToDecimal(dt1.Rows[i]["SalesAmt"].ToString());
                    //            totSale += Convert.ToDecimal(dt1.Rows[i]["SalesAmt"].ToString());
                    //            flgFound = true;
                    //            break;
                    //        }
                    //    }

                    //}
                }
                dt1.Dispose();




                #endregion
                for (int i = 0; i < dt.Rows.Count ; i++)
                {
                    mySql = dt.Rows[i]["MaterialType"].ToString();
                    if (mySql == "OT")
                    {
                        dt.Rows[i]["Sales(Excluding.Tax)"] = Convert.ToDecimal(dt.Rows[i]["Sales(Excluding.Tax)"].ToString()) - lessOTSale;
                        //lessOTSale += Convert.ToDecimal(dt1.Rows[i]["SalesAmt"].ToString());                        
                    }
                    if (Convert.ToDecimal(dt.Rows[i]["Sales(Excluding.Tax)"].ToString()) <= 0)
                    {
                        dt.Rows.Remove(dt.Rows[i]);
                    }                                        
                }
                totSale = 0;
                for (int i = 0; i <dt.Rows.Count ; i++)
                {
                    totSale += Convert.ToDecimal(dt.Rows[i]["Sales(Excluding.Tax)"].ToString());
                }


                dr = dt.NewRow();
                dr["SrNo"] = "";
                dr["MaterialType"] = "";
                dr["Material"] = "      Total :>  ";
                dr["Sales(Excluding.Tax)"] = totSale.ToString();
                dt.Rows.Add(dr);


                grdClientList.DataSource = dt;
                grdClientList.DataBind();
            }
        }

        protected void lnkPrint_Click(object sender, EventArgs e)
        {
            if (grdClientList.Rows.Count > 0 && grdClientList.Visible == true)
            {
                string Subheader = "";

                Subheader = "Businesss for the period - " + lblFromDate.Text + " - " + txtFromDate.Text;
                PrintGrid.PrintGridView(grdClientList, Subheader, "Business_Report");
            }
        }

        protected void imgClosePopup_Click(object sender, ImageClickEventArgs e)
        {
            object refUrl = ViewState["RefUrl"];
            if (refUrl != null)
            {
                Response.Redirect((string)refUrl);
            }
        }
    }
}