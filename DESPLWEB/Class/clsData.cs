using System;
using System.Collections.Generic;
using System.Web;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.IO;
namespace DESPLWEB
{
    public class clsData
    {
        static string cnStr = ConfigurationManager.AppSettings["conStr"].ToString();
        const int a = 0;
        protected static SqlConnection con;
        public Int32 getComplaintId()
        {
            DataTable dt = getGeneralData("Select Max(COMP_Id) as [COMP_Id] from tbl_ComplaintRegister");
            if (dt.Rows.Count > 0 && dt.Rows[0][0].ToString() != "")
                return Convert.ToInt32(dt.Rows[0]["COMP_Id"].ToString());
            else
                return 0;
        }
        public string getEnquiryMobileStatus(int enqNo)
        {
            DataTable dt = getGeneralData("Select ENQ_MobileAppEnqNo_int,Enq_Material_Id  from tbl_Enquiry where ENQ_Id =" + enqNo);
            if (dt.Rows.Count > 0 && dt.Rows[0][0].ToString() != "")
                return Convert.ToInt32(dt.Rows[0]["ENQ_MobileAppEnqNo_int"].ToString()) + "," + Convert.ToInt32(dt.Rows[0]["Enq_Material_Id"].ToString());
            else
                return "0";
        }
        public void updateCylinder(string refNo, Boolean flgCylinder)
        {
            SqlConnection con = new SqlConnection(cnStr);
            try
            {
                string mQueryString;
                if (flgCylinder == true)
                    mQueryString = @"update tbl_Core_Inward set CRINWD_Cylinder_bit=" + 1 + " where CRINWD_ReferenceNo_var='" + refNo + "'";
                else
                {

                    mQueryString = @"update tbl_Core_Inward set CRINWD_Cylinder_bit=" + 0 + " where CRINWD_ReferenceNo_var='" + refNo + "'";
                }
                con.Open();
                SqlCommand cmd = new SqlCommand(mQueryString, con);
                cmd.ExecuteNonQuery();
                cmd.Dispose();
            }
            catch (Exception e)
            {
                throw e;
            }

        }

        public void updateNABLDetails(string refNo, string recType, string nablScope, int nablLoc)
        {
            try
            {
                LabDataDataContext dc = new LabDataDataContext();
                dc.ReportDetails_Update_NABLDetails(recType, refNo, nablLoc, nablScope, "");
                string nablStatus = "";
                if (nablScope == "NA")
                    nablStatus = "NON-NABL";
                else
                    nablStatus = "NABL";
                dc.Inward_Update_NablStatus(refNo, recType, nablStatus);
            }
            catch (Exception e)
            {
                throw e;
            }

        }

        public string updateCubeInward(string mySql)
        {
            string outputStr = "";
            SqlConnection conn = new SqlConnection(cnStr);
            SqlDataAdapter da = new SqlDataAdapter();
            DataTable dt = new DataTable();
            conn.Open();
            da = new SqlDataAdapter();
            da.UpdateCommand = new SqlCommand(mySql, conn);
            da.UpdateCommand.ExecuteNonQuery();
            conn.Close();
            return outputStr;
        }

        public string getMaterialTypeValue(string MATERIAL_Id)
        {
            string matRecType = "";
            SqlConnection cn = new SqlConnection(cnStr);
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "SELECT MATERIAL_RecordType_var FROM tbl_Material where MATERIAL_Id='" + MATERIAL_Id + "' ";
            cn.Open();
            cmd.Connection = cn;
            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    matRecType = reader.GetString(0);
                }
            }
            cmd.Dispose();
            cn.Close();
            return matRecType;
        }
        public void updateContactEmail(int Cl_ID, int Site_Id, string emailID)
        {
            SqlConnection con = new SqlConnection(cnStr);
            try
            {
                string mQueryString = @"update tbl_Contact set CONT_EmailID_var='" + emailID + "' where CONT_CL_Id='" + Cl_ID + "' and CONT_Site_Id='" + Site_Id + "'";
                con.Open();
                SqlCommand cmd = new SqlCommand(mQueryString, con);
                cmd.ExecuteNonQuery();
                cmd.Dispose();
            }
            catch (Exception e)
            {
                throw e;
            }

        }
        public void sendInwardReportMsg(int EnqNo, string EnqDate, string monthlyStatus, string creditLimitExceeded, string totalCost, string ReferenceNo, string ContactNo, string Action)
        {
            clsSendMail objcls = new clsSendMail();
            string tempId;
            string smsContent = "";
            totalCost = "0";
            if (EnqDate != "")
            {
                EnqDate = Convert.ToDateTime(EnqDate).ToString("dd/MM/yyyy");
            }
            tempId = "";
            if (Action == "Inward")
            {
                if (monthlyStatus == "0" && creditLimitExceeded == "1")//not monthly and  o/s > credit limit
                {
                    //if (totalCost != "0")
                    //    smsContent = "Dear Customer, We have collected material against your enquiry no " + EnqNo + " dated " + EnqDate.ToString() + " testing is in progress, total cost of testing is Rs. " + totalCost + ", please arrange to make the payment in advance.";
                    //else
                    smsContent = "Dear Customer, We have collected material against your enquiry no " + EnqNo + " dated " + EnqDate.ToString() + " testing is in progress, please arrange to make the payment in advance.";
                    tempId = "1007620993152932147";
                }
                else
                {
                    //if (totalCost != "0")
                    //    smsContent = "Dear Customer, We have collected material against your enquiry no " + EnqNo + " dated " + EnqDate.ToString() + " testing is in progress, total cost of testing is Rs. " + totalCost;
                    //else
                    smsContent = "Dear Customer, We have collected material against your enquiry no " + EnqNo + " dated " + EnqDate.ToString() + " testing is in progress.";                    
                    tempId = "1007620993152932147";
                }
            }
            else if (Action == "Report")
            {
                if (monthlyStatus == "0" && creditLimitExceeded == "1")//not monthly and  o/s > credit limit
                {
                    smsContent = "Dear Customer, We have tested material against your enquiry no " + EnqNo + " dated " + EnqDate.ToString() + " (Reference No : " + ReferenceNo + "). The report is ready with us, please arrange to make the payment to receive the report. Please ignore if payment already done.";
                    tempId = "1007819479428055084";
                }
                else
                {
                    smsContent = "Dear Customer, We have tested material against your enquiry no " + EnqNo + " dated " + EnqDate.ToString() + " (Reference No : " + ReferenceNo + ").";
                    tempId = "1007819479428055084";
                }
            }


            if (ContactNo != "")
                objcls.sendSMS(ContactNo, smsContent, "DUROCR",tempId );

        }

        public void sendEnquiryProposalMsg(string EnqId, DateTime ENQ_Date_dt, string ContactNo, string Action)
        {
            clsSendMail objcls = new clsSendMail();
            string templateID ;
            string tollFree = "", smsContent = "";
            if (PrintPDFReport.cnStr.ToLower().Contains("mumbai") == true)
                tollFree = "9850500013";
            else if (PrintPDFReport.cnStr.ToLower().Contains("nashik") == true)
                tollFree = "7720006754";
            else
                tollFree = "Toll Free No. 18001206465";

            templateID = "1007893849166095256";
            if (Action == "Enquiry")
            {
                if (EnqId.Contains(",") == true)
                {
                    smsContent = "Dear Customer, we have received your enquiry numbers are " + EnqId + " dated " + ENQ_Date_dt.ToString("dd/MM/yyyy") + ". Please contact us on " + tollFree + " for any further queries.";
                    //templateID = "1007893849166095256";
                }
                else
                {
                    smsContent = "Dear Customer, we have received your enquiry number " + EnqId + " dated " + ENQ_Date_dt.ToString("dd/MM/yyyy") + ". Please contact us on " + tollFree + " for any further queries.";
                    //templateID = "1007893849166095256";
                }
            }
            else if (Action == "Proposal")
            {
                smsContent = "Dear Customer, please approve the proposal number " + EnqId + " for further action and send approval email to us from your registered email id.";
                //templateID = "1007893849166095256";

            }
            if (ContactNo != "")
                objcls.sendSMS(ContactNo, smsContent, "DUROCR",templateID );

        }

        public int getClientCrlBypassBit(string MATERIAL_RecordType_var, int RecordNo)
        {

            SqlConnection cn = new SqlConnection(cnStr);
            SqlCommand cmd = new SqlCommand();
            DataTable dt = getGeneralData(@"select CL_ByPassCRLimitChecking_bit 
                                            from  tbl_Client as a, tbl_Inward as b
                                            Where  b.INWD_CL_Id =a.CL_Id 
											And b.INWD_RecordNo_int = " + RecordNo + " and b.INWD_RecordType_var = '" + MATERIAL_RecordType_var + "' ");
            if (dt.Rows.Count > 0)
            {
                if (Convert.ToString(dt.Rows[0]["CL_ByPassCRLimitChecking_bit"].ToString()) != null && Convert.ToString(dt.Rows[0]["CL_ByPassCRLimitChecking_bit"].ToString()) != "")
                    return Convert.ToInt32(Convert.ToBoolean(dt.Rows[0]["CL_ByPassCRLimitChecking_bit"].ToString()));
                else
                    return 0;
            }
            else
                return 0;
        }

        public int getSITECRBypassBit(string MATERIAL_RecordType_var, int RecordNo)
        {

            // SqlConnection cn = new SqlConnection(cnStr);
            // SqlCommand cmd = new SqlCommand();
            // DataTable dt = getGeneralData(@"select SITE_CRBypass_bit
            //                                 from  tbl_Site as a, tbl_Inward as b
            //                                 Where  b.INWD_SITE_Id=a.SITE_Id
            //And b.INWD_RecordNo_int = " + RecordNo + " and b.INWD_RecordType_var = '" + MATERIAL_RecordType_var + "' ");
            // if (dt.Rows.Count > 0)
            // {
            //     if (Convert.ToString(dt.Rows[0]["SITE_CRBypass_bit"].ToString()) != null && Convert.ToString(dt.Rows[0]["SITE_CRBypass_bit"].ToString()) != "")
            //         return Convert.ToInt32(Convert.ToBoolean(dt.Rows[0]["SITE_CRBypass_bit"].ToString()));
            //     else
            //         return 0;
            // }
            // else
            //     return 0;
            SqlConnection cn = new SqlConnection(cnStr);
            SqlCommand cmd = new SqlCommand();
            DataTable dt = getGeneralData(@"select CL_ByPassCRLimitChecking_bit 
                                            from  tbl_Client as a, tbl_Inward as b
                                            Where  b.INWD_CL_Id =a.CL_Id 
											And b.INWD_RecordNo_int = " + RecordNo + " and b.INWD_RecordType_var = '" + MATERIAL_RecordType_var + "' ");
            if (dt.Rows.Count > 0)
            {
                if (Convert.ToString(dt.Rows[0]["CL_ByPassCRLimitChecking_bit"].ToString()) != null && Convert.ToString(dt.Rows[0]["CL_ByPassCRLimitChecking_bit"].ToString()) != "")
                    return Convert.ToInt32(Convert.ToBoolean(dt.Rows[0]["CL_ByPassCRLimitChecking_bit"].ToString()));
                else
                    return 0;
            }
            else
                return 0;
        }
        public int getMaterialTypeId(string MATERIAL_RecordType_var)
        {
            //string matRecType = "";
            SqlConnection cn = new SqlConnection(cnStr);
            SqlCommand cmd = new SqlCommand();
            DataTable dt = getGeneralData("SELECT MATERIAL_Id FROM tbl_Material where MATERIAL_RecordType_var='" + MATERIAL_RecordType_var + "'");
            if (dt.Rows.Count > 0)
                return Convert.ToInt32(dt.Rows[0]["MATERIAL_Id"].ToString());
            else
                return 0;
        }

        public DataTable getAlertIds(string AlertMap_EmailId_var, int type)
        {
            SqlConnection con = new SqlConnection(cnStr);
            try
            {
                DataSet ds = new DataSet();
                DataTable dt = new DataTable();
                string str = "";
                if (type == 0)
                    str = @"select distinct(AlertMap_AlertId_int) from tbl_AlertMapping as a,tbl_Alert as b
			                where a.AlertMap_AlertId_int=b.Alert_Id
			                and b.Alert_Type_var='Trigger'
			                and  AlertMap_EmailId_var='" + AlertMap_EmailId_var + "'";
                else
                    str = @"select distinct(AlertMap_AlertId_int) from tbl_AlertMapping as a,tbl_Alert as b
			                where a.AlertMap_AlertId_int=b.Alert_Id
			                and b.Alert_Type_var='Escalation'
			                and AlertMap_EmailId_var='" + AlertMap_EmailId_var + "'";

                SqlDataAdapter da = new SqlDataAdapter(str, con);//and CL_name_var like '" + str + "%'
                da.Fill(ds);
                dt = ds.Tables[0];
                ds.Dispose();
                return dt;
            }
            catch (Exception e)
            {
                throw e;
            }

        }

        public void DeleteAlertMappingData(string tblName)
        {
            try
            {
                SqlConnection con = new SqlConnection(cnStr);
                DataSet ds = new DataSet();
                SqlDataAdapter da = new SqlDataAdapter("delete from " + tblName, con);
                da.Fill(ds);
                ds.Dispose();

            }
            catch (Exception e)
            {
                throw e;
            }
        }
        internal void InsertAlertMapping(DataTable dtM, int parentId)
        {
            DeleteAlertMappingData("tbl_AlertMapping where AlertMap_ParentLevelId_int = " + parentId);
            DataTable dt = new DataTable();
            dt = dtM;
            SqlConnection con = new SqlConnection(cnStr);
            DataSet ds2 = new DataSet();
            con.Open();
            ds2.Tables.Add(dt);
            using (SqlBulkCopy bulkCopy = new SqlBulkCopy(con))
            {
                bulkCopy.DestinationTableName = "dbo.tbl_AlertMapping";
                bulkCopy.WriteToServer(ds2.Tables[0]);
                con.Close();
            }
        }

        //        public DataTable AlertDetailsViewForTrigger(int Alert_Id)
        //        {
        //            SqlConnection con = new SqlConnection(cnStr);
        //            try
        //            {
        //                DataSet ds = new DataSet();
        //                DataTable dt = new DataTable();
        //                string str = "";
        //                if (Alert_Id == 1)
        //                    str = @"Select a.ENQ_Id,a.ENQ_Date_dt,'' as Bill_Id,null as BILL_Date_dt,0 as BILL_NetAmt_num,
        //						'' as Proposal_No,0 as Proposal_Id,null as Proposal_Date,'' as MISRefNo,0 as MISRecordNo,
        //						c.CL_Name_var,d.SITE_Name_var,e.MATERIAL_Name_var,c.CL_Id,d.SITE_Id,1 as AlertId
        //						from tbl_Enquiry as a,tbl_Proposal as b,tbl_Client as c,tbl_Site as d,tbl_Material as e
        //						where a.ENQ_Id=b.Proposal_EnqNo
        //						and a.ENQ_CL_Id=c.CL_Id
        //						and a.ENQ_SITE_Id=d.SITE_Id
        //						and a.ENQ_MATERIAL_Id=e.MATERIAL_Id
        //						and datediff(d,convert(date,a.ENQ_Date_dt),convert(date,b.Proposal_Date)) > 1
        //						and datediff(MONTH, ENQ_Date_dt,CURRENT_TIMESTAMP) < 4
        //						and b.Proposal_NewClientStatus=0
        //						
        //						Union
        //						
        //                        Select a.ENQNEW_Id as ENQ_Id,a.ENQNEW_Date_dt as ENQ_Date_dt,'' as Bill_Id,null as BILL_Date_dt,0 as BILL_NetAmt_num,
        //						'' as Proposal_No,0 as Proposal_Id,null as Proposal_Date,'' as MISRefNo,0 as MISRecordNo,
        //						ENQNEW_ClientName_var,ENQNEW_SiteName_var,e.MATERIAL_Name_var,c.CL_Id,d.SITE_Id,1 as AlertId
        //						from tbl_EnquiryNewClient as a,tbl_Proposal as b,tbl_Client as c,tbl_Site as d,tbl_Material as e
        //						where a.ENQNEW_Id=b.Proposal_EnqNo
        //						and (a.ENQNEW_CL_Id=c.CL_Id or a.ENQNEW_CL_Id=0)
        //						and (a.ENQNEW_SITE_Id=d.SITE_Id or a.ENQNEW_SITE_Id=0)
        //						and a.ENQNEW_MATERIAL_Id=e.MATERIAL_Id
        //						and datediff(d,convert(date,a.ENQNEW_Date_dt),convert(date,b.Proposal_Date)) > 1
        //						and datediff(MONTH, ENQNEW_Date_dt,CURRENT_TIMESTAMP) < 4
        //						and b.Proposal_NewClientStatus=1";

        //                else if (Alert_Id == 2)
        //                    str = @"Select a.ENQ_Id,a.ENQ_Date_dt,'' as Bill_Id,null as BILL_Date_dt,0 as BILL_NetAmt_num,
        //						b.Proposal_No,b.Proposal_Id,b.Proposal_Date,'' as MISRefNo,0 as MISRecordNo,
        //						c.CL_Name_var,d.SITE_Name_var,e.MATERIAL_Name_var,c.CL_Id,d.SITE_Id,2 as AlertId
        //						from tbl_Enquiry as a,tbl_Proposal as b,tbl_Client as c,tbl_Site as d,tbl_Material as e
        //						where a.ENQ_Id=b.Proposal_EnqNo
        //						and a.ENQ_CL_Id=c.CL_Id
        //						and a.ENQ_SITE_Id=d.SITE_Id
        //						and a.ENQ_MATERIAL_Id=e.MATERIAL_Id
        //						and datediff(d,convert(date,b.Proposal_Date),convert(date,b.Proposal_OrderDate_dt)) > 7
        //						and datediff(MONTH, Proposal_Date,CURRENT_TIMESTAMP) < 4
        //						and b.Proposal_NewClientStatus=0
        //						
        //						Union
        //						
        //						Select a.ENQNEW_Id as ENQ_Id,a.ENQNEW_Date_dt as ENQ_Date_dt,'' as Bill_Id,null as BILL_Date_dt,0 as BILL_NetAmt_num,
        //						b.Proposal_No,b.Proposal_Id,b.Proposal_Date,'' as MISRefNo,0 as MISRecordNo
        //						,ENQNEW_ClientName_var,ENQNEW_SiteName_var,e.MATERIAL_Name_var,0 as CL_Id,0 as SITE_Id,2 as AlertId
        //						from tbl_EnquiryNewClient as a,tbl_Proposal as b,tbl_Material as e--tbl_Client as c,tbl_Site as d,
        //						where a.ENQNEW_Id=b.Proposal_EnqNo
        //						--and (a.ENQNEW_CL_Id=0 or a.ENQNEW_CL_Id=c.CL_Id)
        //						--and (a.ENQNEW_SITE_Id=d.SITE_Id or a.ENQNEW_SITE_Id=0)
        //						and a.ENQNEW_MATERIAL_Id=e.MATERIAL_Id 
        //						and datediff(d,convert(date,b.Proposal_Date),convert(date,b.Proposal_OrderDate_dt)) > 7
        //						and datediff(MONTH, Proposal_Date,CURRENT_TIMESTAMP) < 4
        //						and b.Proposal_NewClientStatus=1";

        //                else if (Alert_Id == 3)
        //                    str = @"Select a.ENQ_Id,a.ENQ_Date_dt,'' as Bill_Id,null as BILL_Date_dt,0 as BILL_NetAmt_num,
        //						'' as Proposal_No,0 as Proposal_Id,null as Proposal_Date,'' as MISRefNo,0 as MISRecordNo,
        //						c.CL_Name_var,d.SITE_Name_var,e.MATERIAL_Name_var,c.CL_Id,d.SITE_Id,3 as AlertId
        //						from tbl_Enquiry as a,tbl_Proposal as b,tbl_Client as c,tbl_Site as d,tbl_Material as e
        //						where a.ENQ_Id=b.Proposal_EnqNo
        //						and a.ENQ_CL_Id=c.CL_Id
        //						and a.ENQ_SITE_Id=d.SITE_Id
        //						and a.ENQ_MATERIAL_Id=e.MATERIAL_Id
        //						and datediff(d,convert(date,b.Proposal_OrderDate_dt),convert(date,a.ENQ_CollectionDate_dt)) > 2
        //						and datediff(MONTH, Proposal_OrderDate_dt,CURRENT_TIMESTAMP) < 4";

        //                else if (Alert_Id == 4)
        //                    str = @"Select a.ENQ_Id,a.ENQ_Date_dt,'' as Bill_Id,null as BILL_Date_dt,0 as BILL_NetAmt_num,
        //						'' as Proposal_No,0 as Proposal_Id,null as Proposal_Date,'' as MISRefNo,0 as MISRecordNo,
        //						c.CL_Name_var,d.SITE_Name_var,e.MATERIAL_Name_var,c.CL_Id,d.SITE_Id,4 as AlertId
        //						from tbl_Enquiry as a,tbl_Inward as b,tbl_Client as c,tbl_Site as d,tbl_Material as e
        //						where a.ENQ_Id=b.INWD_ENQ_Id
        //						and a.ENQ_CL_Id=c.CL_Id
        //						and a.ENQ_SITE_Id=d.SITE_Id
        //						and a.ENQ_MATERIAL_Id=e.MATERIAL_Id
        //						and datediff(d,convert(date,a.ENQ_CollectionDate_dt),convert(date,b.INWD_ReceivedDate_dt)) > 1
        //						and datediff(MONTH, ENQ_CollectionDate_dt,CURRENT_TIMESTAMP) < 4
        //						and a.ENQ_Status_tint <> 2";

        //                else if (Alert_Id == 7)
        //                    str = @"
        //						Select a.ENQ_Id,a.ENQ_Date_dt,'' as Bill_Id,null as BILL_Date_dt,0 as BILL_NetAmt_num,
        //						'' as Proposal_No,0 as Proposal_Id,null as Proposal_Date, MISRefNo,MISRecordNo,
        //						c.CL_Name_var,d.SITE_Name_var,e.MATERIAL_Name_var,c.CL_Id,d.SITE_Id,7 as AlertId
        //						from tbl_Enquiry as a,tbl_Inward as b,tbl_Client as c,tbl_Site as d,tbl_Material as e,tbl_MISDetail as f
        //						where a.ENQ_Id=b.INWD_ENQ_Id
        //						and b.INWD_RecordNo_int=f.MISRecordNo
        //						and b.INWD_RecordType_var=f.MISRecType
        //						and a.ENQ_CL_Id=c.CL_Id
        //						and a.ENQ_SITE_Id=d.SITE_Id
        //						and a.ENQ_MATERIAL_Id=e.MATERIAL_Id
        //						and datediff(d,convert(date,b.INWD_ReceivedDate_dt),convert(date,f.MISEnteredDt)) > 1
        //						and datediff(MONTH, INWD_ReceivedDate_dt,CURRENT_TIMESTAMP) < 4";

        //                else if (Alert_Id == 8)
        //                    str = @"Select a.ENQ_Id,a.ENQ_Date_dt,'' as Bill_Id,null as BILL_Date_dt,0 as BILL_NetAmt_num,
        //						'' as Proposal_No,0 as Proposal_Id,null as Proposal_Date,f.MISRefNo,f.MISRecordNo,
        //						c.CL_Name_var,d.SITE_Name_var,e.MATERIAL_Name_var,c.CL_Id,d.SITE_Id,8 as AlertId
        //						from tbl_Enquiry as a,tbl_Inward as b,tbl_Client as c,tbl_Site as d,tbl_Material as e,tbl_MISDetail as f
        //						where a.ENQ_Id=b.INWD_ENQ_Id
        //						and b.INWD_RecordNo_int=f.MISRecordNo
        //						and b.INWD_RecordType_var=f.MISRecType
        //						and a.ENQ_CL_Id=c.CL_Id
        //						and a.ENQ_SITE_Id=d.SITE_Id
        //						and a.ENQ_MATERIAL_Id=e.MATERIAL_Id
        //						and datediff(d,convert(date,f.MISEnteredDt),convert(date,f.MISCheckedDt)) > 1
        //						and datediff(MONTH, MISEnteredDt,CURRENT_TIMESTAMP) < 4";

        //                else if (Alert_Id == 9)
        //                    str = @"Select a.ENQ_Id,a.ENQ_Date_dt,'' as Bill_Id,null as BILL_Date_dt,0 as BILL_NetAmt_num,
        //						'' as Proposal_No,0 as Proposal_Id,null as Proposal_Date,f.MISRefNo,f.MISRecordNo,
        //						c.CL_Name_var,d.SITE_Name_var,e.MATERIAL_Name_var,c.CL_Id,d.SITE_Id,9 as AlertId
        //						from tbl_Enquiry as a,tbl_Inward as b,tbl_Client as c,tbl_Site as d,tbl_Material as e,tbl_MISDetail as f
        //						where a.ENQ_Id=b.INWD_ENQ_Id
        //						and b.INWD_RecordNo_int=f.MISRecordNo
        //						and b.INWD_RecordType_var=f.MISRecType
        //						and a.ENQ_CL_Id=c.CL_Id
        //						and a.ENQ_SITE_Id=d.SITE_Id
        //						and a.ENQ_MATERIAL_Id=e.MATERIAL_Id
        //						and datediff(d,convert(date,f.MISCheckedDt),convert(date,f.MISApprovedDt)) > 1
        //						and datediff(MONTH, MISCheckedDt,CURRENT_TIMESTAMP) < 4";

        //                else if (Alert_Id == 11)
        //                    str = @"Select a.ENQ_Id,a.ENQ_Date_dt,'' as Bill_Id,null as BILL_Date_dt,0 as BILL_NetAmt_num,
        //						'' as Proposal_No,0 as Proposal_Id,null as Proposal_Date,f.MISRefNo,f.MISRecordNo,
        //						c.CL_Name_var,d.SITE_Name_var,e.MATERIAL_Name_var,c.CL_Id,d.SITE_Id,11 as AlertId
        //						from tbl_Enquiry as a,tbl_Inward as b,tbl_Client as c,tbl_Site as d,tbl_Material as e,tbl_MISDetail as f
        //						where a.ENQ_Id=b.INWD_ENQ_Id
        //						and b.INWD_RecordNo_int=f.MISRecordNo
        //						and b.INWD_RecordType_var=f.MISRecType
        //						and a.ENQ_CL_Id=c.CL_Id
        //						and a.ENQ_SITE_Id=d.SITE_Id
        //						and a.ENQ_MATERIAL_Id=e.MATERIAL_Id
        //						and datediff(d,convert(date,f.MISApprovedDt),convert(date,f.MISIssueDt)) <= 1
        //						and datediff(MONTH, MISApprovedDt,CURRENT_TIMESTAMP) < 4";
        //                else if (Alert_Id == 12)
        //                    str = @"Select a.ENQ_Id,a.ENQ_Date_dt,'' as Bill_Id,null as BILL_Date_dt,0 as BILL_NetAmt_num,
        //						'' as Proposal_No,0 as Proposal_Id,null as Proposal_Date,f.MISRefNo,f.MISRecordNo,
        //						c.CL_Name_var,d.SITE_Name_var,e.MATERIAL_Name_var,c.CL_Id,d.SITE_Id,12 as AlertId
        //						from tbl_Enquiry as a,tbl_Inward as b,tbl_Client as c,tbl_Site as d,tbl_Material as e,tbl_MISDetail as f,tbl_Bill as g
        //						where a.ENQ_Id=b.INWD_ENQ_Id
        //						and b.INWD_RecordNo_int=f.MISRecordNo
        //						and b.INWD_BILL_Id=g.BILL_Id
        //						and b.INWD_RecordType_var=f.MISRecType
        //						and a.ENQ_CL_Id=c.CL_Id
        //						and a.ENQ_SITE_Id=d.SITE_Id
        //						and a.ENQ_MATERIAL_Id=e.MATERIAL_Id
        //						and datediff(d,convert(date,f.MISApprovedDt),convert(date,f.MISIssueDt)) <= 3
        //						and datediff(MONTH, MISApprovedDt,CURRENT_TIMESTAMP) < 4";

        //                else if (Alert_Id == 13)
        //                    str = @"Select a.ENQ_Id,a.ENQ_Date_dt,'' as Bill_Id,null as BILL_Date_dt,0 as BILL_NetAmt_num,
        //						'' as Proposal_No,0 as Proposal_Id,null as Proposal_Date,f.MISRefNo,f.MISRecordNo,
        //						c.CL_Name_var,d.SITE_Name_var,e.MATERIAL_Name_var,c.CL_Id,d.SITE_Id,13 as AlertId
        //						from tbl_Enquiry as a,tbl_Inward as b,tbl_Client as c,tbl_Site as d,tbl_Material as e,tbl_MISDetail as f,tbl_Bill as g
        //						where a.ENQ_Id=b.INWD_ENQ_Id
        //						and b.INWD_RecordNo_int=f.MISRecordNo
        //						and b.INWD_BILL_Id=g.BILL_Id
        //						and b.INWD_RecordType_var=f.MISRecType
        //						and a.ENQ_CL_Id=c.CL_Id
        //						and a.ENQ_SITE_Id=d.SITE_Id
        //						and a.ENQ_MATERIAL_Id=e.MATERIAL_Id
        //						and datediff(d,convert(date,f.MISApprovedDt),convert(date,f.MISIssueDt)) > 3
        //						and datediff(MONTH, MISApprovedDt,CURRENT_TIMESTAMP) < 4";

        //                else if (Alert_Id == 15)
        //                    str = @"Select 0 as ENQ_Id,null as ENQ_Date_dt,a.BILL_Id,a.BILL_Date_dt,a.BILL_NetAmt_num,
        //						'' as Proposal_No,0 as Proposal_Id,null as Proposal_Date,'' as MISRefNo,0 as MISRecordNo,
        //						c.CL_Name_var,d.SITE_Name_var,a.BILL_RecordType_var  as MATERIAL_Name_var,c.CL_Id,d.SITE_Id,15 as AlertId
        //						from tbl_Client as c,tbl_Site as d,tbl_Bill as a
        //						where  a.BILL_CL_Id=c.CL_Id 
        //						and a.Bill_SITE_Id=d.SITE_Id
        //						and datediff(d,convert(date,a.BILL_Date_dt),convert(date,a.BILL_ModifiedOn_dt)) > 2
        //						and datediff(MONTH, BILL_Date_dt,CURRENT_TIMESTAMP) < 4";

        //                else if (Alert_Id == 17)
        //                    str = @"Select 0 as ENQ_Id,null as ENQ_Date_dt,a.BILL_Id,a.BILL_Date_dt,a.BILL_NetAmt_num,
        //						'' as Proposal_No,0 as Proposal_Id,null as Proposal_Date,'' as MISRefNo,0 as MISRecordNo,
        //						c.CL_Name_var,d.SITE_Name_var,a.BILL_RecordType_var as MATERIAL_Name_var,c.CL_Id,d.SITE_Id,17 as AlertId
        //						from tbl_Bill as a,tbl_Client as c,tbl_Site as d
        //						where  a.BILL_CL_Id=c.CL_Id 
        //						and a.Bill_SITE_Id=d.SITE_Id
        //						and datediff(d,convert(date,a.BILL_Date_dt),convert(date,GETDATE())) >= 60
        //						and datediff(MONTH, BILL_Date_dt,CURRENT_TIMESTAMP) < 4
        //						and a.Bill_Status_bit=0
        //						and (
        //						 isNULL ((Select sum(b.BILL_NetAmt_num) from tbl_Bill as b where  b.BILL_Status_bit=0
        //							),0) + 
        //						isNULL ((Select sum(c.Journal_Amount_dec) from tbl_Journal as c,tbl_JournalDetail as d where c.Journal_Status_bit=0
        //							and c.Journal_Note_var=d.JournalDetail_NoteNo_var
        //							and a.BILL_Id=d.JournalDetail_BillNo_var
        //							And CHARINDEX('DB/', c.Journal_NoteNo_var,1) > 0
        //						),0) 
        //						+
        //						isNULL ((Select sum(d.CashDetail_Amount_money) from tbl_CashDetail as d where 
        //							d.CashDetail_Status_bit=0 and a.BILL_Id=d.CashDetail_BillNo_int),0) 
        //						)> 0 ";

        //                else if (Alert_Id == 18)
        //                    str = @"Select 0 as ENQ_Id,null as ENQ_Date_dt,a.BILL_Id,a.BILL_Date_dt,a.BILL_NetAmt_num,
        //						'' as Proposal_No,0 as Proposal_Id,null as Proposal_Date,'' as MISRefNo,0 as MISRecordNo,
        //						c.CL_Name_var,d.SITE_Name_var,a.BILL_RecordType_var as MATERIAL_Name_var,c.CL_Id,d.SITE_Id,18 as AlertId
        //						from tbl_Bill as a,tbl_Client as c,tbl_Site as d
        //						where  a.BILL_CL_Id=c.CL_Id 
        //						and a.Bill_SITE_Id=d.SITE_Id
        //						and datediff(d,convert(date,a.BILL_Date_dt),convert(date,GETDATE())) >= 90
        //						and datediff(MONTH, BILL_Date_dt,CURRENT_TIMESTAMP) < 4
        //						and a.Bill_Status_bit=0
        //						and (
        //						 isNULL ((Select sum(b.BILL_NetAmt_num) from tbl_Bill as b where  b.BILL_Status_bit=0
        //							),0) + 
        //						isNULL ((Select sum(c.Journal_Amount_dec) from tbl_Journal as c,tbl_JournalDetail as d where c.Journal_Status_bit=0
        //							and c.Journal_Note_var=d.JournalDetail_NoteNo_var
        //							and a.BILL_Id=d.JournalDetail_BillNo_var
        //							And CHARINDEX('DB/', c.Journal_NoteNo_var,1) > 0
        //						),0) 
        //						+
        //						isNULL ((Select sum(d.CashDetail_Amount_money) from tbl_CashDetail as d where 
        //							d.CashDetail_Status_bit=0 and a.BILL_Id=d.CashDetail_BillNo_int),0) 
        //						)> 0 ";

        //                else if (Alert_Id == 19)
        //                    str = @"Select 0 as ENQ_Id,null as ENQ_Date_dt,a.BILL_Id,a.BILL_Date_dt,a.BILL_NetAmt_num,
        //						'' as Proposal_No,0 as Proposal_Id,null as Proposal_Date,'' as MISRefNo,0 as MISRecordNo,
        //						c.CL_Name_var,d.SITE_Name_var,a.BILL_RecordType_var as MATERIAL_Name_var,c.CL_Id,d.SITE_Id,19 as AlertId
        //						from tbl_Bill as a,tbl_Client as c,tbl_Site as d
        //						where  a.BILL_CL_Id=c.CL_Id 
        //						and a.Bill_SITE_Id=d.SITE_Id
        //						and datediff(d,convert(date,a.BILL_Date_dt),convert(date,GETDATE())) >= 120
        //						and datediff(MONTH, BILL_Date_dt,CURRENT_TIMESTAMP) < 4
        //						and a.Bill_Status_bit=0
        //						and (
        //						 isNULL ((Select sum(b.BILL_NetAmt_num) from tbl_Bill as b where  b.BILL_Status_bit=0
        //							),0) + 
        //						isNULL ((Select sum(c.Journal_Amount_dec) from tbl_Journal as c,tbl_JournalDetail as d where c.Journal_Status_bit=0
        //							and c.Journal_Note_var=d.JournalDetail_NoteNo_var
        //							and a.BILL_Id=d.JournalDetail_BillNo_var
        //							And CHARINDEX('DB/', c.Journal_NoteNo_var,1) > 0
        //						),0) 
        //						+
        //						isNULL ((Select sum(d.CashDetail_Amount_money) from tbl_CashDetail as d where 
        //							d.CashDetail_Status_bit=0 and a.BILL_Id=d.CashDetail_BillNo_int),0) 
        //						)> 0 ";

        //                else if (Alert_Id == 20)
        //                    str = @"Select 0 as ENQ_Id,null as ENQ_Date_dt,a.BILL_Id,a.BILL_Date_dt,a.BILL_NetAmt_num,
        //						'' as Proposal_No,0 as Proposal_Id,null as Proposal_Date,'' as MISRefNo,0 as MISRecordNo
        //						,c.CL_Name_var,d.SITE_Name_var,a.BILL_RecordType_var as MATERIAL_Name_var,c.CL_Id,d.SITE_Id,20 as AlertId
        //						from tbl_Bill as a,tbl_Client as c,tbl_Site as d
        //						where  a.BILL_CL_Id=c.CL_Id 
        //						and a.Bill_SITE_Id=d.SITE_Id
        //						and datediff(d,convert(date,a.BILL_Date_dt),convert(date,GETDATE())) > 120
        //						and datediff(MONTH, BILL_Date_dt,CURRENT_TIMESTAMP) < 4
        //						and a.Bill_Status_bit=0
        //						and (
        //						 isNULL ((Select sum(b.BILL_NetAmt_num) from tbl_Bill as b where  b.BILL_Status_bit=0
        //							),0) + 
        //						isNULL ((Select sum(c.Journal_Amount_dec) from tbl_Journal as c,tbl_JournalDetail as d where c.Journal_Status_bit=0
        //							and c.Journal_Note_var=d.JournalDetail_NoteNo_var
        //							and a.BILL_Id=d.JournalDetail_BillNo_var
        //							And CHARINDEX('DB/', c.Journal_NoteNo_var,1) > 0
        //						),0) 
        //						+
        //						isNULL ((Select sum(d.CashDetail_Amount_money) from tbl_CashDetail as d where 
        //							d.CashDetail_Status_bit=0 and a.BILL_Id=d.CashDetail_BillNo_int),0) 
        //						)> 0";

        //                SqlDataAdapter da = new SqlDataAdapter(str, con);//and CL_name_var like '" + str + "%'
        //                da.Fill(ds);
        //                dt = ds.Tables[0];
        //                ds.Dispose();
        //                return dt;
        //            }
        //            catch (Exception e)
        //            {
        //                throw e;
        //            }

        //        }
        //        public Int32 AlertDetailsViewForEscalation(int Alert_Id)
        //        {
        //            SqlConnection con = new SqlConnection(cnStr);
        //            try
        //            {
        //                int count = 0;
        //                SqlConnection cn = new SqlConnection(cnStr);
        //                SqlCommand cmd = new SqlCommand();

        //                if (Alert_Id == 21)
        //                    cmd.CommandText = @" select(Select COUNT(Enq_Id)
        //				from tbl_Enquiry as a,tbl_Proposal as b,tbl_Client as c,tbl_Site as d,tbl_Material as e
        //				where a.ENQ_Id=b.Proposal_EnqNo
        //				and a.ENQ_CL_Id=c.CL_Id
        //				and a.ENQ_SITE_Id=d.SITE_Id
        //				and a.ENQ_MATERIAL_Id=e.MATERIAL_Id
        //				and datediff(d,convert(date,a.ENQ_Date_dt),convert(date,b.Proposal_Date)) > 2
        //				and datediff(MONTH, ENQ_Date_dt,CURRENT_TIMESTAMP) < 4
        //				and b.Proposal_NewClientStatus=0)
        //				
        //				+
        //				
        //				(Select COUNT(EnqNEW_Id)
        //				from tbl_EnquiryNewClient as a,tbl_Proposal as b,tbl_Client as c,tbl_Site as d,tbl_Material as e
        //				where a.ENQNEW_Id=b.Proposal_EnqNo
        //				and (a.ENQNEW_CL_Id=c.CL_Id or a.ENQNEW_CL_Id=0)
        //				and (a.ENQNEW_SITE_Id=d.SITE_Id or a.ENQNEW_SITE_Id=0)
        //				and a.ENQNEW_MATERIAL_Id=e.MATERIAL_Id
        //				and datediff(d,convert(date,a.ENQNEW_Date_dt),convert(date,b.Proposal_Date)) > 2
        //				and datediff(MONTH, ENQNEW_Date_dt,CURRENT_TIMESTAMP) < 4
        //				and b.Proposal_NewClientStatus=1)as AlertCount,21 as AlertId";

        //                else if (Alert_Id == 22)
        //                    cmd.CommandText = @"select(Select COUNT(Proposal_OrderNo_var)
        //				from tbl_Enquiry as a,tbl_Proposal as b,tbl_Client as c,tbl_Site as d,tbl_Material as e
        //				where a.ENQ_Id=b.Proposal_EnqNo
        //				and a.ENQ_CL_Id=c.CL_Id
        //				and a.ENQ_SITE_Id=d.SITE_Id
        //				and a.ENQ_MATERIAL_Id=e.MATERIAL_Id
        //				and datediff(d,convert(date,b.Proposal_Date),convert(date,b.Proposal_OrderDate_dt)) > 15
        //				and datediff(MONTH, ENQ_Date_dt,CURRENT_TIMESTAMP) < 4
        //				and b.Proposal_NewClientStatus=0)
        //				
        //				+
        //				
        //				(Select COUNT(Proposal_OrderNo_var)
        //				from tbl_EnquiryNewClient as a,tbl_Proposal as b,tbl_Material as e--tbl_Client as c,tbl_Site as d,
        //				where a.ENQNEW_Id=b.Proposal_EnqNo
        //				--and (a.ENQNEW_CL_Id=0 or a.ENQNEW_CL_Id=c.CL_Id)
        //				--and (a.ENQNEW_SITE_Id=d.SITE_Id or a.ENQNEW_SITE_Id=0)
        //				and a.ENQNEW_MATERIAL_Id=e.MATERIAL_Id 
        //				and datediff(d,convert(date,b.Proposal_Date),convert(date,b.Proposal_OrderDate_dt)) > 15
        //				and datediff(MONTH, Proposal_Date,CURRENT_TIMESTAMP) < 4
        //				and b.Proposal_NewClientStatus=1) as AlertCount,22 as AlertId";

        //                else if (Alert_Id == 23)
        //                    cmd.CommandText = @"Select COUNT(Proposal_OrderNo_var),23 as AlertId
        //				from tbl_Enquiry as a,tbl_Proposal as b,tbl_Client as c,tbl_Site as d,tbl_Material as e
        //				where a.ENQ_Id=b.Proposal_EnqNo
        //				and a.ENQ_CL_Id=c.CL_Id
        //				and a.ENQ_SITE_Id=d.SITE_Id
        //				and a.ENQ_MATERIAL_Id=e.MATERIAL_Id
        //				and datediff(d,convert(date,b.Proposal_OrderDate_dt),convert(date,a.ENQ_CollectionDate_dt)) > 3
        //				and datediff(MONTH, Proposal_OrderDate_dt,CURRENT_TIMESTAMP) < 4
        //				and ENQ_Status_tint=5";

        //                else if (Alert_Id == 24)
        //                    cmd.CommandText = @"Select COUNT(ENQ_Id),24 as AlertId
        //				from tbl_Enquiry as a,tbl_Inward as b,tbl_Client as c,tbl_Site as d,tbl_Material as e
        //				where a.ENQ_Id=b.INWD_ENQ_Id
        //				and a.ENQ_CL_Id=c.CL_Id
        //				and a.ENQ_SITE_Id=d.SITE_Id
        //				and a.ENQ_MATERIAL_Id=e.MATERIAL_Id
        //				and datediff(d,convert(date,ENQ_CollectionDate_dt),convert(date,GETDATE())) > 2
        //				and datediff(MONTH, ENQ_CollectionDate_dt,CURRENT_TIMESTAMP) < 4
        //				and a.ENQ_Status_tint <> 2";

        //                else if (Alert_Id == 25)
        //                    cmd.CommandText = @"Select COUNT(ENQ_Id),25 as AlertId
        //				from tbl_Enquiry as a,tbl_Inward as b,tbl_Client as c,tbl_Site as d,tbl_Material as e,tbl_MISDetail as f
        //				where a.ENQ_Id=b.INWD_ENQ_Id
        //				and b.INWD_RecordNo_int=f.MISRecordNo
        //				and b.INWD_RecordType_var=f.MISRecType
        //				and a.ENQ_CL_Id=c.CL_Id
        //				and a.ENQ_SITE_Id=d.SITE_Id
        //				and a.ENQ_MATERIAL_Id=e.MATERIAL_Id
        //				and datediff(d,convert(date,b.INWD_ReceivedDate_dt),convert(date,f.MISEnteredDt)) > 2
        //				and datediff(MONTH, INWD_ReceivedDate_dt,CURRENT_TIMESTAMP) < 4
        //				and a.ENQ_Status_tint=2";

        //                else if (Alert_Id == 28)
        //                    cmd.CommandText = @"	Select COUNT(ENQ_Id),28 as AlertId
        //				from tbl_Enquiry as a,tbl_Inward as b,tbl_Client as c,tbl_Site as d,tbl_Material as e,tbl_MISDetail as f
        //				where a.ENQ_Id=b.INWD_ENQ_Id
        //				and b.INWD_RecordNo_int=f.MISRecordNo
        //				and b.INWD_RecordType_var=f.MISRecType
        //				and a.ENQ_CL_Id=c.CL_Id
        //				and a.ENQ_SITE_Id=d.SITE_Id
        //				and a.ENQ_MATERIAL_Id=e.MATERIAL_Id
        //				and datediff(d,convert(date,f.MISEnteredDt),convert(date,f.MISCheckedDt)) > 1
        //				and datediff(MONTH, MISEnteredDt,CURRENT_TIMESTAMP) < 4
        //				and a.ENQ_Status_tint=2";

        //                else if (Alert_Id == 29)
        //                    cmd.CommandText = @"Select  COUNT(b.INWD_BILL_Id),29 as AlertId
        //				from tbl_Enquiry as a,tbl_Inward as b,tbl_Client as c,tbl_Site as d,tbl_Material as e,tbl_MISDetail as f
        //				where a.ENQ_Id=b.INWD_ENQ_Id
        //				and b.INWD_RecordNo_int=f.MISRecordNo
        //				and b.INWD_RecordType_var=f.MISRecType
        //				and a.ENQ_CL_Id=c.CL_Id
        //				and a.ENQ_SITE_Id=d.SITE_Id
        //				and a.ENQ_MATERIAL_Id=e.MATERIAL_Id
        //				and datediff(MONTH, MISEnteredDt,CURRENT_TIMESTAMP) < 4
        //				and datediff(d,convert(date,f.MISEnteredDt),convert(date,f.MISApprovedDt)) > 1";

        //                else if (Alert_Id == 30)
        //                    cmd.CommandText = @"Select  COUNT(b.INWD_BILL_Id),30 as AlertId
        //				from tbl_Enquiry as a,tbl_Inward as b,tbl_Client as c,tbl_Site as d,tbl_Material as e,tbl_MISDetail as f,tbl_Bill as g
        //				where a.ENQ_Id=b.INWD_ENQ_Id
        //				and b.INWD_BILL_Id=g.BILL_Id
        //				and b.INWD_RecordNo_int=f.MISRecordNo
        //				and b.INWD_RecordType_var=f.MISRecType
        //				and a.ENQ_CL_Id=c.CL_Id
        //				and a.ENQ_SITE_Id=d.SITE_Id
        //				and a.ENQ_MATERIAL_Id=e.MATERIAL_Id
        //				and datediff(MONTH, MISApprovedDt,CURRENT_TIMESTAMP) < 4
        //				and datediff(d,convert(date,f.MISApprovedDt),convert(date,g.BILL_Date_dt)) > 1
        //		";
        //                else if (Alert_Id == 31)
        //                    cmd.CommandText = @"Select COUNT(f.ID),31 as AlertId
        //				from tbl_Enquiry as a,tbl_Inward as b,tbl_Client as c,tbl_Site as d,tbl_Material as e,tbl_MISDetail as f
        //				where a.ENQ_Id=b.INWD_ENQ_Id
        //				and b.INWD_RecordNo_int=f.MISRecordNo
        //				and b.INWD_RecordType_var=f.MISRecType
        //				and a.ENQ_CL_Id=c.CL_Id
        //				and a.ENQ_SITE_Id=d.SITE_Id
        //				and a.ENQ_MATERIAL_Id=e.MATERIAL_Id
        //				and datediff(MONTH, MISApprovedDt,CURRENT_TIMESTAMP) < 4
        //				and datediff(d,convert(date,f.MISApprovedDt),convert(date,f.MISPrintedDt)) > 1
        //				";

        //                else if (Alert_Id == 32)
        //                    cmd.CommandText = @"Select COUNT(f.ID),32 as AlertId
        //				from tbl_Enquiry as a,tbl_Inward as b,tbl_Client as c,tbl_Site as d,tbl_Material as e,tbl_MISDetail as f
        //				where a.ENQ_Id=b.INWD_ENQ_Id
        //				and b.INWD_RecordNo_int=f.MISRecordNo
        //				and b.INWD_RecordType_var=f.MISRecType
        //				and a.ENQ_CL_Id=c.CL_Id
        //				and a.ENQ_SITE_Id=d.SITE_Id
        //				and a.ENQ_MATERIAL_Id=e.MATERIAL_Id
        //				and datediff(d,convert(date,f.MISApprovedDt),convert(date,f.MISPrintedDt)) <= 3
        //				and datediff(MONTH, MISApprovedDt,CURRENT_TIMESTAMP) < 4
        //				";

        //                else if (Alert_Id == 33)
        //                    cmd.CommandText = @"Select COUNT(f.ID),33 as AlertId
        //				from tbl_Enquiry as a,tbl_Inward as b,tbl_Client as c,tbl_Site as d,tbl_Material as e,tbl_MISDetail as f
        //				where a.ENQ_Id=b.INWD_ENQ_Id
        //				and b.INWD_RecordNo_int=f.MISRecordNo
        //				and b.INWD_RecordType_var=f.MISRecType
        //				and a.ENQ_CL_Id=c.CL_Id
        //				and a.ENQ_SITE_Id=d.SITE_Id
        //				and a.ENQ_MATERIAL_Id=e.MATERIAL_Id
        //				and datediff(d,convert(date,f.MISApprovedDt),convert(date,f.MISPrintedDt)) > 3
        //				and datediff(MONTH, MISApprovedDt,CURRENT_TIMESTAMP) < 4";

        //                else if (Alert_Id == 35)
        //                    cmd.CommandText = @"Select COUNT(BILL_Id),35 as AlertId
        //				from tbl_Client as c,tbl_Site as d,tbl_Bill as a
        //				where  a.BILL_CL_Id=c.CL_Id 
        //				and a.Bill_SITE_Id=d.SITE_Id
        //				and datediff(d,convert(date,a.BILL_Date_dt),convert(date,a.BILL_ModifiedOn_dt)) > 2
        //				and datediff(MONTH, BILL_Date_dt,CURRENT_TIMESTAMP) < 4
        //			";

        //                cn.Open();
        //                cmd.Connection = cn;

        //                using (SqlDataReader reader = cmd.ExecuteReader())
        //                {
        //                    while (reader.Read())
        //                    {
        //                        count = reader.GetInt32(0);
        //                    }
        //                }
        //                cmd.Dispose();
        //                cn.Close();
        //                return count;
        //            }
        //            catch (Exception e)
        //            {
        //                throw e;
        //            }

        //        }


        //public string getMeContct(string USER_Name_var)
        //{
        //    DataTable dt = getGeneralData("Select USER_ContactNo_var as ContactNo from tbl_User where USER_Mkt_right_bit = 1 and USER_Status_bit=0 and USER_Name_var='" + USER_Name_var + "' ");
        //    if (dt.Rows.Count > 0 && dt.Rows[0][0].ToString() != "")
        //        return dt.Rows[0]["ContactNo"].ToString();
        //    else
        //        return null;
        //}
        public DataTable getClientList()
        {
            SqlConnection con = new SqlConnection(cnStr);
            try
            {
                DataSet ds = new DataSet();
                DataTable dt = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter("select CL_Id,CL_Name_var from tbl_Client where CL_status_bit=0 ", con);//and CL_name_var like '" + str + "%'
                da.Fill(ds);
                dt = ds.Tables[0];
                ds.Dispose();
                return dt;
            }
            catch (Exception e)
            {
                throw e;
            }

        }
        public DataTable getMonthlyClientList(string dtFrom, string dtTo, int flag)
        {
            SqlConnection con = new SqlConnection(cnStr);
            try
            {
                DataSet ds = new DataSet();
                DataTable dt = new DataTable();
                string str = "";
                if (flag == 0)
                {
                    str = @"Select a.CL_Id, a.CL_Name_var
			                From tbl_Client as a left join tbl_Site as b on a.CL_Id = b.SITE_CL_Id
			                left join tbl_Inward as c on c.INWD_SITE_Id = b.SITE_Id 
			                left join tbl_MISDetail as d on c.INWD_RecordType_var = d.MISRecType and c.INWD_RecordNo_int = d.MISRecordNo 
			                Where b.SITE_MonthlyBillingStatus_bit = 1
			                And c.INWD_CL_Id = a.CL_Id
			                And (d.MISIssueDt is not null or d.MISRecType = 'MF')
			                And c.INWD_MonthlyBill_bit = 0
			                And c.INWD_BILL_Id = '0'	
			                And c.INWD_RecordNo_int not in (select x.WBILL_RecordNo_int from tbl_WithoutBill as x where c.INWD_RecordType_var = x.WBILL_RecordType_var )
			                And (c.INWD_RecordType_var <> 'CT' or  (c.INWD_RecordType_var = 'CT' and c.INWD_RecordNo_int not in (select x.CTINWD_RecordNo_int from tbl_Cube_Inward as x where x.CTINWD_CouponNo_var is not null and x.CTINWD_CouponNo_var <> '' )))
			                And (
                            (convert(date, d.MISIssueDt ) >= CONVERT(date, '" + dtFrom + "') And convert(date, d.MISIssueDt ) <= CONVERT(date, '" + dtTo + "') )" +
                            " Or " +
                            "(c.INWD_RecordType_var = 'MF' and convert(date, c.INWD_ReceivedDate_dt) >= CONVERT(date, '" + dtFrom + "') And convert(date, c.INWD_ReceivedDate_dt) <= CONVERT(date, '" + dtTo + "') )" +
                            @")
                            Group by a.CL_Id, a.CL_Name_var
			                Order by a.CL_Name_var";
                }
                else
                {
                    // str = @"Select a.CL_Name_var,b.Site_Name_var
                    //From tbl_Client as a, tbl_Site as b
                    //Where a.CL_Id = b.SITE_CL_Id
                    //And b.SITE_MonthlyBillingStatus_bit = 1
                    //Group by  a.CL_Name_var,b.SITE_Name_var
                    //Order by a.CL_Name_var";

                    str = @"select CL_Name_var, SITE_Name_var, ROUTE_Name_var
                        from tbl_Site inner join tbl_Route on ROUTE_Id = SITE_Route_Id  left
                        join tbl_Client a on CL_Id = SITE_CL_Id
                        where SITE_MonthlyBillingStatus_bit = 1 order by CL_Name_var ,SITE_Name_var";
                }
                SqlDataAdapter da = new SqlDataAdapter(str, con);//and CL_name_var like '" + str + "%'
                da.Fill(ds);
                dt = ds.Tables[0];
                ds.Dispose();
                return dt;
            }
            catch (Exception e)
            {
                throw e;
            }

        }
        public string getProformaBillNetAmount(string ProBillInvNo, int flag)
        {
            string proformaBillNetAmt = "0";
            SqlConnection cn = new SqlConnection(cnStr);
            SqlCommand cmd = new SqlCommand();
            if (flag == 1)
                cmd.CommandText = "SELECT ISNULL(PROINV_NetAmt_num,0) as PROINV_NetAmt_num FROM tbl_ProformaInvoice where PROINV_Id='" + ProBillInvNo + "' ";
            else
                cmd.CommandText = "SELECT ISNULL(Bill_NetAmt_num,0) as Bill_NetAmt_num FROM tbl_Bill where Bill_Id='" + ProBillInvNo + "' ";
            cn.Open();
            cmd.Connection = cn;
            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    proformaBillNetAmt = reader.GetDecimal(0).ToString();
                }
            }
            cmd.Dispose();
            cn.Close();
            return proformaBillNetAmt;
        }

        public DataTable getGeneralData(string mQueryString)
        {
            SqlConnection con = new SqlConnection(cnStr);
            try
            {
                DataSet ds = new DataSet();
                DataTable dt = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter(mQueryString, con);
                da.Fill(ds);
                dt = ds.Tables[0];
                ds.Dispose();
                return dt;
            }
            catch (Exception e)
            {
                throw e;
            }

        }
        public DataTable getOtherSubTest()
        {
            SqlConnection con = new SqlConnection(cnStr);
            try
            {
                DataSet ds = new DataSet();
                DataTable dt = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter("select Test_name_var from tbl_test where Test_name_var!='Earth Resistivity Test' and Test_name_var!='SBC by SPT' and Test_name_var!='Plate Load Testing' and test_subtest_Id=0 and test_rectype_var='OTHER'  and TEST_Status_int=0", con);
                da.Fill(ds);
                dt = ds.Tables[0];
                ds.Dispose();
                return dt;
            }
            catch (Exception e)
            {
                throw e;
            }

        }
        public DataTable getProposalNo(int enqNo, int status)
        {
            SqlConnection con = new SqlConnection(cnStr);
            try
            {
                DataSet ds = new DataSet();
                DataTable dt = new DataTable(); string mQueryString = "";
                if (status == 0)//exiting table client
                {
                    mQueryString = @"select Proposal_No
			from tbl_Proposal where Proposal_NewClientStatus=0 and	Proposal_EnqNo ='" + enqNo + "' order by Proposal_Id desc";
                }
                else//non exting client
                {
                    mQueryString = @"select Proposal_No
			from tbl_Proposal where Proposal_NewClientStatus=1 and	Proposal_EnqNo ='" + enqNo + "'  order by Proposal_Id desc";

                }
                SqlDataAdapter da = new SqlDataAdapter(mQueryString, con);
                da.Fill(ds);
                dt = ds.Tables[0];
                ds.Dispose();
                return dt;
            }
            catch (Exception e)
            {
                throw e;
            }

        }

        internal string getContctDetailsFromAppEnquiry(string mobId)
        {
            string contct = "";
            SqlConnection cn = new SqlConnection(cnStr);
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = " Select (ENQ_contact_person+'|'+ENQ_contact_number+'|'+ENQ_contact_emailid)  as contact from tbl_enquiry_new  where ENQ_contact_number='" + mobId.ToString() + "' ";
            cn.Open();
            cmd.Connection = cn;
            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    contct = reader.GetString(0);
                }
            }
            cmd.Dispose();
            cn.Close();
            return contct;
        }

        public DataTable getEnquiryDetails(int enqId, int status)
        {
            SqlConnection con = new SqlConnection(cnStr);
            try
            {
                DataSet ds = new DataSet();
                DataTable dt = new DataTable(); string mQueryString = "";
                if (status == 0)
                {
                    mQueryString = @"select *
			,(Select CONT_Name_var from tbl_Contact where CONT_Id = a.ENQ_CONT_Id) as CONT_Name_var,
			(Select CONT_ContactNo_var from tbl_Contact where CONT_Id = a.ENQ_CONT_Id) as CONT_ContactNo_var,
			 (Select USER_Name_var from tbl_User f,tbl_Route g where g.ROUTE_Id = d.SITE_Route_Id and f.USER_Id=g.ROUTE_ME_Id) as ME_Name_var,
			(Select USER_ContactNo_var from tbl_User f,tbl_Route g where g.ROUTE_Id = d.SITE_Route_Id and f.USER_Id=g.ROUTE_ME_Id) as ME_Contact_No
			from tbl_Enquiry as a, tbl_Material as b,tbl_Client as c,tbl_Site as d
			where a.ENQ_MATERIAL_Id = b.MATERIAL_Id
			and a.ENQ_CL_Id = c.CL_Id
			and a.ENQ_SITE_Id = d.SITE_Id
			and a.ENQ_Id ='" + enqId + "'";
                }
                else
                {
                    mQueryString = @"select *,
			(Select CONT_Name_var from tbl_Contact where CONT_Id = a.ENQ_CONT_Id) as CONT_Name_var,
			(Select CONT_ContactNo_var from tbl_Contact where CONT_Id = a.ENQ_CONT_Id) as CONT_ContactNo_var,
			(Select USER_Name_var from tbl_User f,tbl_Route g where g.ROUTE_Id = d.SITE_Route_Id and f.USER_Id=g.ROUTE_ME_Id) as ME_Name_var,
			(Select USER_ContactNo_var from tbl_User f,tbl_Route g where g.ROUTE_Id = d.SITE_Route_Id and f.USER_Id=g.ROUTE_ME_Id) as ME_Contact_No
			 from tbl_Enquiry as a, tbl_Material as b,tbl_Client as c,tbl_Site as d, tbl_Test as e
			where a.ENQ_MATERIAL_Id = b.MATERIAL_Id
			and a.ENQ_CL_Id = c.CL_Id
			and a.ENQ_SITE_Id = d.SITE_Id
			and e.TEST_INWARD_Id = b.MATERIAL_Id
			and a.ENQ_Id ='" + enqId + "'";

                }
                SqlDataAdapter da = new SqlDataAdapter(mQueryString, con);
                da.Fill(ds);
                dt = ds.Tables[0];
                ds.Dispose();
                return dt;
            }
            catch (Exception e)
            {
                throw e;
            }

        }
        public void Equipment_Insert(string EQP_InternalIdMark_var, string EQP_Name_var, string EQP_ShortName_var, string EQP_Section_var, string EQP_CalibStatus_var, string EQP_SerialNo_var, string EQP_Make_var, string EQP_Certificate_var, string EQP_LeastCount_var, string EQP_RecdOnDate_dt, string EQP_Range_var, int EQPD_SrNo_int, string EQPD_CalibDueOnDate_dt, string EQPD_Agency_var)
        {
            using (SqlConnection connection = new SqlConnection(cnStr))
            {
                int EQPD_InternalIdMark_var = 0;
                connection.Open();
                SqlCommand command = connection.CreateCommand();
                SqlTransaction transaction;
                // Start a local transaction.
                transaction = connection.BeginTransaction("EqpTransaction");
                // Must assign both transaction object and connection
                // to Command object for a pending local transaction
                command.Connection = connection;
                command.Transaction = transaction;
                try
                {

                    //Insert record 
                    if (EQP_RecdOnDate_dt != "")
                    {
                        string[] arr = EQP_RecdOnDate_dt.Split('/');
                        string dt = arr[2] + "-" + arr[1] + "-" + arr[0];
                        //DateTime RecOn = DateTime.ParseExact(dt, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                        command.CommandText =
                           "insert into tbl_Equipment (EQP_InternalIdMark_var,EQP_Name_var,EQP_ShortName_var,EQP_Section_var,EQP_CalibStatus_var, " +
                               "EQP_SerialNo_var,EQP_Make_var,EQP_Certificate_var,EQP_LeastCount_var,EQP_RecdOnDate_dt,	EQP_Range_var) " +
                               "values ('" + EQP_InternalIdMark_var + "', '" + EQP_Name_var + "' , '" + EQP_ShortName_var + "', '" + EQP_Section_var + "', '" + EQP_CalibStatus_var + "', " +
                               "'" + EQP_SerialNo_var + "','" + EQP_Make_var + "',	'" + EQP_Certificate_var + "', '" + EQP_LeastCount_var + "', '" + dt + "', '" + EQP_Range_var + "' )";
                    }
                    else
                    {
                        command.CommandText =
                                              "insert into tbl_Equipment (EQP_InternalIdMark_var,EQP_Name_var,EQP_ShortName_var,EQP_Section_var,EQP_CalibStatus_var, " +
                                                  "EQP_SerialNo_var,EQP_Make_var,EQP_Certificate_var,EQP_LeastCount_var,	EQP_Range_var) " +
                                                  "values ('" + EQP_InternalIdMark_var + "', '" + EQP_Name_var + "' , '" + EQP_ShortName_var + "', '" + EQP_Section_var + "', '" + EQP_CalibStatus_var + "', " +
                                                  "'" + EQP_SerialNo_var + "','" + EQP_Make_var + "',	'" + EQP_Certificate_var + "', '" + EQP_LeastCount_var + "', '" + EQP_Range_var + "' )";
                    }
                    command.ExecuteNonQuery();
                    //get inserted reference number
                    command.CommandText = "SELECT @@IDENTITY";
                    EQPD_InternalIdMark_var = Convert.ToInt32(command.ExecuteScalar());
                    // Attempt to commit the transaction.

                    //transaction.Commit();


                    //Insert record 
                    if (EQPD_CalibDueOnDate_dt != "")
                    {
                        string[] arr = EQPD_CalibDueOnDate_dt.Split('/');
                        string dt = arr[2] + "-" + arr[1] + "-" + arr[0];
                        command.CommandText =
                        "insert into tbl_EquipmentDetail (EQPD_InternalIdMark_var,EQPD_SrNo_int,EQPD_CalibDueOnDate_dt,EQPD_Agency_var) " +
                            "values ('" + EQP_InternalIdMark_var + "'," + EQPD_SrNo_int + ",  '" + dt + "', '" + EQPD_Agency_var + "' )";
                    }
                    else
                    {
                        command.CommandText =
                   "insert into tbl_EquipmentDetail (EQPD_InternalIdMark_var,EQPD_SrNo_int,EQPD_Agency_var) " +
                       "values ('" + EQP_InternalIdMark_var + "'," + EQPD_SrNo_int + ", '" + EQPD_Agency_var + "' )";

                    }
                    command.ExecuteNonQuery();
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Commit Exception Type: {0}", ex.GetType());
                    Console.WriteLine("  Message: {0}", ex.Message);

                    // Attempt to roll back the transaction.
                    try
                    {
                        transaction.Rollback();
                    }
                    catch (Exception ex2)
                    {
                        // This catch block will handle any errors that may have occurred
                        // on the server that would cause the rollback to fail, such as
                        // a closed connection.
                        Console.WriteLine("Rollback Exception Type: {0}", ex2.GetType());
                        Console.WriteLine("  Message: {0}", ex2.Message);
                    }
                }

            }

        }
        //public string getRecordTypeValueWeb(string recTypr)
        //{
        //    string inwType = "";
        //    SqlConnection cn = new SqlConnection(cnStr);
        //    SqlCommand cmd = new SqlCommand();
        //    cmd.CommandText = " Select MATERIAL_RecordType_var from tbl_Material where MATERIAL_Name_var like '%" + recTypr.ToString() + "%' ";
        //    cn.Open();
        //    cmd.Connection = cn;
        //    using (SqlDataReader reader = cmd.ExecuteReader())
        //    {
        //        while (reader.Read())
        //        {
        //            inwType = reader.GetString(0);
        //        }
        //    }
        //    cmd.Dispose();
        //    cn.Close();
        //    return inwType;
        //}
        public string getInwardTypeValue(string inwardType)
        {
            string inwType = "";
            SqlConnection cn = new SqlConnection(cnStr);
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = " Select MATERIAL_RecordType_var from tbl_Material where MATERIAL_Name_var='" + inwardType.ToString() + "' ";
            cn.Open();
            cmd.Connection = cn;
            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    inwType = reader.GetString(0);
                }
            }
            cmd.Dispose();
            cn.Close();
            return inwType;
        }
        public string getInwardTypeName(string inwardValueType)
        {
            string inwType = "";
            SqlConnection cn = new SqlConnection(cnStr);
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = " Select MATERIAL_Name_var from tbl_Material where MATERIAL_RecordType_var='" + inwardValueType.ToString() + "' ";
            cn.Open();
            cmd.Connection = cn;
            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    inwType = reader.GetString(0);
                }
            }
            cmd.Dispose();
            cn.Close();
            return inwType;
        }
        public string getRouteName(int Route_Id)
        {
            string RouteName = "";
            SqlConnection cn = new SqlConnection(cnStr);
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = " Select ROUTE_Name_var from tbl_Route where ROUTE_Id='" + Route_Id + "' ";
            cn.Open();
            cmd.Connection = cn;
            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    RouteName = reader.GetString(0);
                }
            }
            cmd.Dispose();
            cn.Close();
            return RouteName;
        }

        public void updateBillGst(string billId, string siteGst, string clientGst)
        {
            SqlConnection con = new SqlConnection(cnStr);
            try
            {
                string mQueryString = @"update tbl_Bill set BILL_SITE_GSTNo_var='" + siteGst + "',BILL_CL_GSTNo_var ='" + clientGst + "' where BILL_Id='" + billId + "'";
                con.Open();
                SqlCommand cmd = new SqlCommand(mQueryString, con);
                cmd.ExecuteNonQuery();
                cmd.Dispose();
            }
            catch (Exception e)
            {
                throw e;
            }

        }
        public DataTable getSiteWiseData(int clientId, int siteId)
        {
            SqlConnection con = new SqlConnection(cnStr);
            try
            {
                DataSet ds = new DataSet();
                DataTable dt = new DataTable();
                string mQueryString = @"select a.TEST_Id ,a.TEST_Sr_No,a.TEST_Name_var,a.Test_RecType_var ,a.TEST_INWARD_Id ,a.TEST_Rate_int,a.TEST_From_num,a.TEST_To_num,
				(select b.SITERATE_Id  from tbl_SiteWiseRate as b where b.SITERATE_Test_Id=a.TEST_Id and b.SITERATE_Client_Id = '" + clientId + "' and b.SITERATE_Site_Id = '" + siteId + "') as SITERATE_Id," +
                "(select b.SITERATE_Test_Rate_int  from tbl_SiteWiseRate as b where b.SITERATE_Test_Id=a.TEST_Id and b.SITERATE_Client_Id = '" + clientId + "' and b.SITERATE_Site_Id ='" + siteId + "') as SITERATE_Test_Rate_int" +
               " from tbl_Test as a  where a.TEST_Status_int=0 and a.Test_RecType_var!='OT' and a.TEST_SiteWiseRate_flag=1  order by a.Test_RecType_var,a.Test_Name_var";
                SqlDataAdapter da = new SqlDataAdapter(mQueryString, con);
                da.Fill(ds);
                dt = ds.Tables[0];
                ds.Dispose();
                return dt;
            }
            catch (Exception e)
            {
                throw e;
            }

        }

        public Int32 getRecordNo(string refNo)
        {
            DataTable dt = getGeneralData("SELECT MFINWD_RecordNo_int FROM tbl_MixDesign_Inward where MFINWD_ReferenceNo_var='" + refNo + "'");
            if (dt.Rows.Count > 0)
                return Convert.ToInt32(dt.Rows[0]["MFINWD_RecordNo_int"].ToString());
            else
                return 0;
        }

        public DataTable getMaterialCollectionDetails()
        {
            SqlConnection con = new SqlConnection(cnStr);
            DataTable dt = new DataTable();
            try
            {
                DataSet ds = new DataSet();
                string mysql = "";
                mysql = @"select a.ENQ_MATERIAL_Id ,  
                        (select MATERIAL_Name_var  from tbl_Material where MATERIAL_Id = a.ENQ_MATERIAL_Id ) as TestName,
                        month(a.ENQ_Date_dt ) month,
                        year(a.ENQ_Date_dt ) Year,DATENAME(MONTH,a.ENQ_Date_dt ) as Mname,
                        COUNT(a.ENQ_Id )  totalEnq
                        from tbl_Enquiry a 
                        where a.ENQ_Status_tint =2
                        group by a.ENQ_MATERIAL_Id ,month(a.ENQ_Date_dt ),year(a.ENQ_Date_dt ),DATENAME(MONTH,a.ENQ_Date_dt )
                        order by year(a.ENQ_Date_dt ) ,month(a.ENQ_Date_dt )";

                SqlCommand cmd = new SqlCommand(mysql, con);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);
                dt = ds.Tables[0];


                ds.Dispose();
                return dt;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public DataTable getCollectionDetails()
        {
            SqlConnection con = new SqlConnection(cnStr);
            DataTable dt = new DataTable();
            try
            {
                DataSet ds = new DataSet();
                string mysql = "";
                //                mysql = @"select  DATENAME(MONTH,BILL_Date_dt ) as [Month], YEar(BILL_Date_dt ) [Year]
                //                        ,(select  ROUTE_Name_var  from tbl_Route where ROUTE_Id=a.BILL_Route_Id ) as RouteName 
                //                        ,COUNT(a.BILL_SITE_Id ) as SiteCount
                //                        ,  SUM(BILL_NetAmt_num)  as BillingAmount 
                //                        ,a.BILL_Route_Id as RouteId
                //                        ,Month(BILL_Date_dt ) [Mon],BILL_Date_dt as Dt
                //                        from tbl_Bill a 
                //                        where  a.BILL_Status_bit=0 and a.BILL_Id>0  
                //                        group by MONTH(BILL_Date_dt ),DATENAME(MONTH,BILL_Date_dt ),YEar(BILL_Date_dt ),BILL_Route_Id,BILL_Date_dt
                //                        order by Year(BILL_Date_dt ) ,Month(BILL_Date_dt ) ";
                mysql = @"select MONTH(BILL_Date_dt ) as [Mon],YEar(BILL_Date_dt) as [Year],SUM(BILL_NetAmt_num) as [BillingAmount],ROUTE_Id,COUNT(BILL_SITE_Id) as [SiteCount],ROUTE_Name_var from tbl_Bill,tbl_Site,tbl_Route where tbl_Bill.BILL_SITE_Id=tbl_Site.SITE_Id 
                and  tbl_Site.SITE_Route_Id=tbl_Route.ROUTE_Id and Bill_Status_Bit=0 
                group by  MONTH(BILL_Date_dt ),YEar(BILL_Date_dt ),ROUTE_Id,ROUTE_Name_var
                order by ROUTE_Name_var";
                SqlCommand cmd = new SqlCommand(mysql, con);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);
                dt = ds.Tables[0];


                ds.Dispose();
                return dt;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public DataTable getBillEntriesMonthly(string routeId, string Month, string year)
        {
            SqlConnection con = new SqlConnection(cnStr);
            DataTable dt = new DataTable();
            try
            {
                DataSet ds = new DataSet();
                string mysql = "";
                mysql = @"select distinct Bill_Id from tbl_Bill,tbl_Site,tbl_Route where tbl_Bill.BILL_SITE_Id=tbl_Site.SITE_Id 
                and  tbl_Site.SITE_Route_Id=tbl_Route.ROUTE_Id and Bill_Status_Bit=0 and   tbl_Route.ROUTE_Id='" + routeId + "' " +
               " and MONTH(BILL_Date_dt)='" + Month + "' and  YEAR(BILL_Date_dt)='" + year + "'";

                //                    mysql = @"select distinct Bill_Id
                //                    from tbl_Bill where BILL_Id>0 and Bill_Status_Bit=0 and   BILL_Route_Id='" + routeId + "' and MONTH(BILL_Date_dt)='" + Month + "' and  YEAR(BILL_Date_dt)='" + year + "'";

                //                mysql = @" select distinct(Bill_Site_id),MONTH(BILL_Date_dt) ,YEAR(BILL_Date_dt) from  tbl_Bill  where Bill_Route_Id>0 and Bill_Route_id IN(select distinct(c.Bill_Route_id)
                //from tbl_Bill c ) group by Bill_Site_id,MONTH(BILL_Date_dt),
                // YEAR(BILL_Date_dt) order by MONTH(BILL_Date_dt) desc,YEAR(BILL_Date_dt) desc ";
                //                mysql = @"select Bill_Id, BILL_SITE_Id,Bill_Route_Id,MONTH(BILL_Date_dt),YEAR(BILL_Date_dt),
                //(select  ROUTE_Name_var  from tbl_Route where ROUTE_Id=BILL_Route_Id ) as RouteName
                //from tbl_Bill where  Bill_Route_Id>0 and BILL_SITE_Id In(
                //select distinct(c.Bill_Site_id)from tbl_Bill c  group by (c.Bill_Site_id),MONTH(c.BILL_Date_dt),
                //                 YEAR(c.BILL_Date_dt) )order by MONTH(BILL_Date_dt) desc,YEAR(BILL_Date_dt) desc,Bill_Route_Id desc ";
                SqlCommand cmd = new SqlCommand(mysql, con);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);
                dt = ds.Tables[0];
                ds.Dispose();
                return dt;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public double getBillEntriesMonthlySum(string BillId)
        {
            string str = "";
            SqlConnection con = new SqlConnection(cnStr);
            DataTable dt = new DataTable();
            try
            {
                DataSet ds = new DataSet();
                string mysql = "";

                mysql = @"select SUM(CashDetail_Amount_money) 
                from tbl_CashDetail a , tbl_Bill b where a.CashDetail_BillNo_int=b.BILL_Id
                and  b.BILL_Status_bit=0 and  a.CashDetail_Status_bit='0' and  a.CashDetail_BillNo_int='" + BillId + "' ";
                SqlCommand cmd = new SqlCommand(mysql, con);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);
                dt = ds.Tables[0];
                ds.Dispose();
                str = dt.Rows[0][0].ToString();
                if (str != "")
                    return Convert.ToDouble(str);
                else
                    return 0;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public DataTable getCubeInward(string strRecordNo)
        {
            SqlConnection con = new SqlConnection(cnStr);
            DataTable dt = new DataTable();
            try
            {
                DataSet ds = new DataSet();
                string mysql = "";
                mysql = "select * from tbl_Cube_Inward " +
                    " where tbl_Cube_Inward.CTINWD_RecordType_var='CT'" +
                    " and tbl_Cube_Inward.CTINWD_RecordNo_int=" + strRecordNo +
                    " Order by CTINWD_SrNo_int ";
                SqlCommand cmd = new SqlCommand(mysql, con);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);
                dt = ds.Tables[0];
                ds.Dispose();
                return dt;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public DataTable getClientOutstanding_Recovery(int ClientId, string AsOn_Date, string Till_Date)
        {
            SqlConnection con = new SqlConnection(cnStr);
            DataTable dt = new DataTable();
            try
            {
                DataSet ds = new DataSet();
                string mysql = "";

                mysql = "Select a.CL_Id, a.CL_Name_var,a.CL_Limit_mny, " +

        " isNULL ((Select sum(b.BILL_NetAmt_num) from tbl_Bill as b where a.CL_Id = b.BILL_CL_Id and b.BILL_Status_bit=0" +
           " And convert(date,b.BILL_Date_dt) <= convert(date,'" + AsOn_Date + "') " +
       " ),0) as billAmountAsOn," +
       " isNULL ((Select sum(c.Journal_Amount_dec) from tbl_Journal as c where a.CL_Id = c.Journal_ClientId_int and c.Journal_Status_bit=0" +
           " And convert(date,c.Journal_Date_dt) <= convert(date,'" + AsOn_Date + "') " +
           " And CHARINDEX('DB/', c.Journal_NoteNo_var,1) > 0" +
       " ),0) as journalDBAmountAsOn," +
       " isNULL ((Select sum(c.Journal_Amount_dec) from tbl_Journal as c where a.CL_Id = c.Journal_ClientId_int and c.Journal_Status_bit=0" +
           " And convert(date,c.Journal_Date_dt) <= convert(date,'" + AsOn_Date + "') " +
           " And CHARINDEX('CR/', c.Journal_NoteNo_var,1) > 0" +
       " ),0) as journalCRAmountAsOn," +
       " isNULL ((Select sum(d.CashDetail_Amount_money) from tbl_CashDetail as d where a.CL_Id = d.CashDetail_ClientId and d.CashDetail_Status_bit=0" +
           " And convert(date,d.CashDetail_Date_date) <= convert(date,'" + AsOn_Date + "') " +
       " ),0) as receivedAmountAsOn, " +

        " isNULL ((Select sum(b.BILL_NetAmt_num) from tbl_Bill as b where a.CL_Id = b.BILL_CL_Id and b.BILL_Status_bit=0" +
           " And convert(date,b.BILL_Date_dt) <= convert(date,'" + Till_Date + "') " +
       " ),0) as billAmount," +
       " isNULL ((Select sum(c.Journal_Amount_dec) from tbl_Journal as c where a.CL_Id = c.Journal_ClientId_int and c.Journal_Status_bit=0" +
           " And convert(date,c.Journal_Date_dt) <= convert(date,'" + Till_Date + "') " +
           " And CHARINDEX('DB/', c.Journal_NoteNo_var,1) > 0" +
       " ),0) as journalDBAmount," +
       " isNULL ((Select sum(c.Journal_Amount_dec) from tbl_Journal as c where a.CL_Id = c.Journal_ClientId_int and c.Journal_Status_bit=0" +
           " And convert(date,c.Journal_Date_dt) <= convert(date,'" + Till_Date + "') " +
           " And CHARINDEX('CR/', c.Journal_NoteNo_var,1) > 0" +
       " ),0) as journalCRAmount," +
       " isNULL ((Select sum(d.CashDetail_Amount_money) from tbl_CashDetail as d where a.CL_Id = d.CashDetail_ClientId and d.CashDetail_Status_bit=0" +
           " And convert(date,d.CashDetail_Date_date) <= convert(date,'" + Till_Date + "') " +
       " ),0) as receivedAmount " +

       " from tbl_Client as a" +
       " where " +
       " (" +
       " isNULL ((Select sum(b.BILL_NetAmt_num) from tbl_Bill as b where a.CL_Id = b.BILL_CL_Id and b.BILL_Status_bit=0" +
           " And convert(date,b.BILL_Date_dt) <= convert(date,'" + AsOn_Date + "') " +
       " ),0) + " +
       " isNULL ((Select sum(c.Journal_Amount_dec) from tbl_Journal as c where a.CL_Id = c.Journal_ClientId_int and c.Journal_Status_bit=0" +
           " And convert(date,c.Journal_Date_dt) <= convert(date,'" + AsOn_Date + "') " +
           " And CHARINDEX('DB/', c.Journal_NoteNo_var,1) > 0" +
       " ),0) +	" +
       " isNULL ((Select sum(d.CashDetail_Amount_money) from tbl_CashDetail as d where a.CL_Id = d.CashDetail_ClientId and d.CashDetail_Status_bit=0" +
           " And convert(date,d.CashDetail_Date_date) <= convert(date,'" + AsOn_Date + "') " +
       " ),0) " +
       " )> 0 and (a.CL_Id = " + ClientId + " or " + ClientId + " = 0)" +
       " order by a.CL_Name_var";


                SqlCommand cmd = new SqlCommand(mysql, con);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);
                dt = ds.Tables[0];
                ds.Dispose();
                return dt;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public DataTable getClientOutstanding(int ClientId, string From_Date, string To_Date, int AsOnStatus)
        {
            SqlConnection con = new SqlConnection(cnStr);
            DataTable dt = new DataTable();
            try
            {
                DataSet ds = new DataSet();
                string mysql = "";
                if (AsOnStatus == 1)
                {
                    mysql = "Select a.CL_Id, a.CL_Name_var,a.CL_Limit_mny, a.CL_UnderReconciliation_bit, " +
           " isNULL ((Select sum(b.BILL_NetAmt_num) from tbl_Bill as b where a.CL_Id = b.BILL_CL_Id and b.BILL_Status_bit=0" +
               " And convert(date,b.BILL_Date_dt) <= convert(date,'" + To_Date + "') " +

           " ),0) as billAmount," +
           " isNULL ((Select sum(c.Journal_Amount_dec) from tbl_Journal as c where a.CL_Id = c.Journal_ClientId_int and c.Journal_Status_bit=0" +
               " And convert(date,c.Journal_Date_dt) <= convert(date,'" + To_Date + "') " +
               " And CHARINDEX('DB/', c.Journal_NoteNo_var,1) > 0" +
           " ),0) as journalDBAmount," +
           " isNULL ((Select sum(c.Journal_Amount_dec) from tbl_Journal as c where a.CL_Id = c.Journal_ClientId_int and c.Journal_Status_bit=0" +
               " And convert(date,c.Journal_Date_dt) <= convert(date,'" + To_Date + "') " +
               " And CHARINDEX('CR/', c.Journal_NoteNo_var,1) > 0" +
           " ),0) as journalCRAmount," +
           " isNULL ((Select sum(d.CashDetail_Amount_money) from tbl_CashDetail as d where a.CL_Id = d.CashDetail_ClientId and d.CashDetail_Status_bit=0" +
               " And convert(date,d.CashDetail_Date_date) <= convert(date,'" + To_Date + "') " +
           " ),0) as receivedAmount" +

           " from tbl_Client as a" +
           " where " +
           " (" +
           " isNULL ((Select sum(b.BILL_NetAmt_num) from tbl_Bill as b where a.CL_Id = b.BILL_CL_Id and b.BILL_Status_bit=0" +
               " And convert(date,b.BILL_Date_dt) <= convert(date,'" + To_Date + "') " +
           " ),0) + " +
           " isNULL ((Select sum(c.Journal_Amount_dec) from tbl_Journal as c where a.CL_Id = c.Journal_ClientId_int and c.Journal_Status_bit=0" +
               " And convert(date,c.Journal_Date_dt) <= convert(date,'" + To_Date + "') " +
               " And CHARINDEX('DB/', c.Journal_NoteNo_var,1) > 0" +
           " ),0) 	" +
           " +" +
           " isNULL ((Select sum(d.CashDetail_Amount_money) from tbl_CashDetail as d where a.CL_Id = d.CashDetail_ClientId and d.CashDetail_Status_bit=0" +
               " And convert(date,d.CashDetail_Date_date) <= convert(date,'" + To_Date + "') " +
           " ),0) " +
           " )> 0 and (a.CL_Id = " + ClientId + " or " + ClientId + " = 0)" +
           " order by a.CL_Name_var";
                }
                else
                {
                    mysql = "Select a.CL_Id, a.CL_Name_var,a.CL_Limit_mny, a.CL_UnderReconciliation_bit, " +
            " isNULL ((Select sum(b.BILL_NetAmt_num) from tbl_Bill as b where a.CL_Id = b.BILL_CL_Id and b.BILL_Status_bit=0" +
                " And convert(date,b.BILL_Date_dt) >= convert(date,'" + From_Date + "') and convert(date,b.BILL_Date_dt) <= convert(date,'" + To_Date + "')" +
            " ),0) as billAmount," +
            " isNULL ((Select sum(c.Journal_Amount_dec) from tbl_Journal as c where a.CL_Id = c.Journal_ClientId_int and c.Journal_Status_bit=0" +
                " And convert(date,c.Journal_Date_dt) >= convert(date,'" + From_Date + "') and convert(date,c.Journal_Date_dt) <= convert(date,'" + To_Date + "')" +
                " And CHARINDEX('DB/', c.Journal_NoteNo_var,1) > 0" +
            " ),0) as journalDBAmount," +
            " isNULL ((Select sum(c.Journal_Amount_dec) from tbl_Journal as c where a.CL_Id = c.Journal_ClientId_int and c.Journal_Status_bit=0" +
                " And convert(date,c.Journal_Date_dt) >= convert(date,'" + From_Date + "') and convert(date,c.Journal_Date_dt) <= convert(date,'" + To_Date + "')" +
                " And CHARINDEX('CR/', c.Journal_NoteNo_var,1) > 0" +
            " ),0) as journalCRAmount," +
            " isNULL ((Select sum(d.CashDetail_Amount_money) from tbl_CashDetail as d where a.CL_Id = d.CashDetail_ClientId and d.CashDetail_Status_bit=0" +
                " And convert(date,d.CashDetail_Date_date) >= convert(date,'" + From_Date + "') and convert(date,d.CashDetail_Date_date) <= convert(date,'" + To_Date + "')" +
            " ),0) as receivedAmount" +

            " from tbl_Client as a" +
            " where " +
            " (" +
            " isNULL ((Select sum(b.BILL_NetAmt_num) from tbl_Bill as b where a.CL_Id = b.BILL_CL_Id and b.BILL_Status_bit=0" +
                " And convert(date,b.BILL_Date_dt) >= convert(date,'" + From_Date + "') and convert(date,b.BILL_Date_dt) <= convert(date,'" + To_Date + "')" +
            " ),0) + " +
            " isNULL ((Select sum(c.Journal_Amount_dec) from tbl_Journal as c where a.CL_Id = c.Journal_ClientId_int and c.Journal_Status_bit=0" +
                " And convert(date,c.Journal_Date_dt) >= convert(date,'" + From_Date + "') and convert(date,c.Journal_Date_dt) <= convert(date,'" + To_Date + "')" +
                " And CHARINDEX('DB/', c.Journal_NoteNo_var,1) > 0" +
            " ),0) 	" +
            " +" +
            " isNULL ((Select sum(d.CashDetail_Amount_money) from tbl_CashDetail as d where a.CL_Id = d.CashDetail_ClientId and d.CashDetail_Status_bit=0" +
                " And convert(date,d.CashDetail_Date_date) >= convert(date,'" + From_Date + "') and convert(date,d.CashDetail_Date_date) <= convert(date,'" + To_Date + "')" +
            " ),0) " +
            " )> 0 and (a.CL_Id = " + ClientId + " or " + ClientId + " = 0)" +
            " order by a.CL_Name_var";
                }
                SqlCommand cmd = new SqlCommand(mysql, con);
                //SqlCommand cmd = new SqlCommand("Client_Outstanding", con);
                //cmd.Parameters.Add("@ClientId", SqlDbType.Int, 4).Value = ClientId;
                //cmd.Parameters.Add("@From_Date", SqlDbType.DateTime, 10).Value = From_Date;
                //cmd.Parameters.Add("@To_Date", SqlDbType.DateTime, 10).Value = To_Date;
                //cmd.Parameters.Add("@AsOnStatus", SqlDbType.Bit).Value = AsOnStatus;
                //cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);
                dt = ds.Tables[0];
                ds.Dispose();
                return dt;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public DataTable getClientOutstanding2(int ClientId, string From_Date, string To_Date, int AsOnStatus)
        {
            SqlConnection con = new SqlConnection(cnStr);
            DataTable dt = new DataTable();
            try
            {
                DataSet ds = new DataSet();

                string mysql = "";
                if (AsOnStatus == 1)
                {
                    mysql = "select BILL_CL_Id,CL_Name_var from " +
            " ( " +
            " (Select convert(varchar,a.BILL_Id) as BILL_Id, a.BILL_CL_Id,a.BILL_NetAmt_num, 0 as CashDetail_Amount_money, " +
            " (select c.CL_Name_var from tbl_Client as c where c.CL_Id = a.BILL_CL_Id) as CL_Name_var " +
            " from tbl_Bill as a " +
            " where a.BILL_Status_bit = 0 and (a.BILL_CL_Id = " + ClientId + " or " + ClientId + " = 0) and a.BILL_CL_Id > 0" +
            " And convert(date,a.BILL_Date_dt) <= convert(date,'" + To_Date + "')  " +
            " AND convert(varchar,a.BILL_Id) NOT IN  " +
            " (SELECT b.CashDetail_BillNo_int from tbl_CashDetail as b where b.CashDetail_Status_bit= 0 " +
            " And  convert(date, b.CashDetail_Date_date) <= convert(date,'" + To_Date + "')  " +
            " ) " +
            " Group by a.BILL_Id, a.BILL_NetAmt_num,a.BILL_CL_Id ) " +

            " Union " +

            " (Select convert(varchar,a.BILL_Id) as BILL_Id, a.BILL_CL_Id,a.BILL_NetAmt_num,sum(b.CashDetail_Amount_money), " +
            " (select c.CL_Name_var from tbl_Client as c where c.CL_Id = a.BILL_CL_Id) as CL_Name_var " +
            " from tbl_Bill as a, tbl_CashDetail as b " +
            " where a.BILL_Status_bit = 0 and (a.BILL_CL_Id = " + ClientId + " or " + ClientId + " = 0)	 and a.BILL_CL_Id > 0" +
            " And convert(date,a.BILL_Date_dt) <= convert(date,'" + To_Date + "')  " +
            " And convert(date, b.CashDetail_Date_date) <= convert(date,'" + To_Date + "')  " +
            " and convert(varchar,a.BILL_Id) = b.CashDetail_BillNo_int  " +
            " and b.CashDetail_Status_bit = 0	 " +
            " Group by a.BILL_Id, a.BILL_CL_Id, a.BILL_NetAmt_num " +
            " Having a.BILL_NetAmt_num + sum(b.CashDetail_Amount_money) > 0) " +

            " Union " +

            " (Select a.Journal_NoteNo_var as BILL_Id,a.Journal_ClientId_int as BILL_CL_Id, a.Journal_Amount_dec 	 as BILL_NetAmt_num, 	0 as CashDetail_Amount_money,	 " +
            " (select c.CL_Name_var from tbl_Client as c where c.CL_Id = a.Journal_ClientId_int) as CL_Name_var " +
            " from tbl_Journal as a " +
            " where a.Journal_Status_bit = 0 and (a.Journal_ClientId_int = " + ClientId + " or " + ClientId + " = 0) and a.Journal_ClientId_int > 0" +
            " And convert(date,a.Journal_Date_dt) <= convert(date,'" + To_Date + "') " +
            " And CHARINDEX('DB/',a.Journal_NoteNo_var) > 0 " +
            " AND a.Journal_NoteNo_var NOT IN  " +
            " (SELECT b.CashDetail_BillNo_int from tbl_CashDetail as b where b.CashDetail_Status_bit= 0) " +
            " Group by a.Journal_NoteNo_var, a.Journal_Amount_dec,a.Journal_ClientId_int) " +

            " Union " +

            " (Select a.Journal_NoteNo_var as BILL_Id, a.Journal_ClientId_int as BILL_CL_Id,	 a.Journal_Amount_dec 	 as BILL_NetAmt_num, 	0 as CashDetail_Amount_money,	 " +
            " (select c.CL_Name_var from tbl_Client as c where c.CL_Id = a.Journal_ClientId_int) as CL_Name_var " +
            " from tbl_Journal as a, tbl_CashDetail as b " +
            " where a.Journal_Status_bit = 0 and (a.Journal_ClientId_int = " + ClientId + " or " + ClientId + " = 0)	and a.Journal_ClientId_int > 0 " +
            " And convert(date,a.Journal_Date_dt) <= convert(date,'" + To_Date + "')  " +
            " And convert(date, b.CashDetail_Date_date) <= convert(date,'" + To_Date + "')  " +
            " and a.Journal_NoteNo_var = b.CashDetail_BillNo_int  " +
            " and b.CashDetail_Status_bit = 0	 " +
            " And CHARINDEX('DB/',a.Journal_NoteNo_var) > 0 " +
            " Group by a.Journal_NoteNo_var, a.Journal_ClientId_int , a.Journal_Amount_dec " +
            " Having a.Journal_Amount_dec + sum(b.CashDetail_Amount_money) > 0) " +
             " ) as q " +
        " group by BILL_CL_Id,CL_Name_var";
                }
                else
                {
                    mysql = "select BILL_CL_Id,CL_Name_var from " +
            " ( " +
            " (Select convert(varchar,a.BILL_Id) as BILL_Id, a.BILL_CL_Id,a.BILL_NetAmt_num, 0 as CashDetail_Amount_money, " +
            " (select c.CL_Name_var from tbl_Client as c where c.CL_Id = a.BILL_CL_Id) as CL_Name_var " +
            " from tbl_Bill as a " +
            " where a.BILL_Status_bit = 0 and (a.BILL_CL_Id = " + ClientId + " or " + ClientId + " = 0) and a.BILL_CL_Id > 0 " +
            " And ((convert(date,a.BILL_Date_dt) >= convert(date,'" + From_Date + "') and convert(date,a.BILL_Date_dt) <= convert(date,'" + To_Date + "')) ) " +
            " AND convert(varchar,a.BILL_Id) NOT IN  " +
            " (SELECT b.CashDetail_BillNo_int from tbl_CashDetail as b where b.CashDetail_Status_bit= 0 " +
            " And ((convert(date,b.CashDetail_Date_date) >= convert(date,'" + From_Date + "') and convert(date,b.CashDetail_Date_date) <= convert(date,'" + To_Date + "')) ) " +
            " ) " +
            " Group by a.BILL_Id, a.BILL_NetAmt_num,a.BILL_CL_Id ) " +

            " Union " +

            " (Select convert(varchar,a.BILL_Id) as BILL_Id, a.BILL_CL_Id,a.BILL_NetAmt_num,sum(b.CashDetail_Amount_money), " +
            " (select c.CL_Name_var from tbl_Client as c where c.CL_Id = a.BILL_CL_Id) as CL_Name_var " +
            " from tbl_Bill as a, tbl_CashDetail as b " +
            " where a.BILL_Status_bit = 0 and (a.BILL_CL_Id = " + ClientId + " or " + ClientId + " = 0)	and a.BILL_CL_Id > 0 " +
            " And ((convert(date,a.BILL_Date_dt) >= convert(date,'" + From_Date + "') and convert(date,a.BILL_Date_dt) <= convert(date,'" + To_Date + "')) ) " +
            " And ((convert(date,b.CashDetail_Date_date) >= convert(date,'" + From_Date + "') and convert(date,b.CashDetail_Date_date) <= convert(date,'" + To_Date + "')) ) " +
            " and convert(varchar,a.BILL_Id) = b.CashDetail_BillNo_int  " +
            " and b.CashDetail_Status_bit = 0	 " +
            " Group by a.BILL_Id, a.BILL_CL_Id, a.BILL_NetAmt_num " +
            " Having a.BILL_NetAmt_num + sum(b.CashDetail_Amount_money) > 0) " +

            " Union " +

            " (Select a.Journal_NoteNo_var as BILL_Id,a.Journal_ClientId_int as BILL_CL_Id, a.Journal_Amount_dec 	 as BILL_NetAmt_num, 	0 as CashDetail_Amount_money,	 " +
            " (select c.CL_Name_var from tbl_Client as c where c.CL_Id = a.Journal_ClientId_int) as CL_Name_var " +
            " from tbl_Journal as a " +
            " where a.Journal_Status_bit = 0 and (a.Journal_ClientId_int = " + ClientId + " or " + ClientId + " = 0) and a.Journal_ClientId_int > 0" +
            " And ((convert(date,a.Journal_Date_dt) >= convert(date,'" + From_Date + "') and convert(date,a.Journal_Date_dt) <= convert(date,'" + To_Date + "')) ) " +
            " And CHARINDEX('DB/',a.Journal_NoteNo_var) > 0 " +
            " AND a.Journal_NoteNo_var NOT IN  " +
            " (SELECT b.CashDetail_BillNo_int from tbl_CashDetail as b where b.CashDetail_Status_bit='false') " +
            " Group by a.Journal_NoteNo_var, a.Journal_Amount_dec,a.Journal_ClientId_int) " +

            " Union " +

            " (Select a.Journal_NoteNo_var as BILL_Id, a.Journal_ClientId_int as BILL_CL_Id,	 a.Journal_Amount_dec 	 as BILL_NetAmt_num, 	0 as CashDetail_Amount_money,	 " +
            " (select c.CL_Name_var from tbl_Client as c where c.CL_Id = a.Journal_ClientId_int) as CL_Name_var " +
            " from tbl_Journal as a, tbl_CashDetail as b " +
            " where a.Journal_Status_bit = 0 and (a.Journal_ClientId_int = " + ClientId + " or " + ClientId + " = 0) and a.Journal_ClientId_int > 0	 " +
            " And ((convert(date,a.Journal_Date_dt) >= convert(date,'" + From_Date + "') and convert(date,a.Journal_Date_dt) <= convert(date,'" + To_Date + "')) ) " +
            " And ((convert(date,b.CashDetail_Date_date) >= convert(date,'" + From_Date + "') and convert(date,b.CashDetail_Date_date) <= convert(date,'" + To_Date + "')) ) " +
            " and a.Journal_NoteNo_var = b.CashDetail_BillNo_int  " +
            " and b.CashDetail_Status_bit = 0	 " +
            " And CHARINDEX('DB/',a.Journal_NoteNo_var) > 0 " +
            " Group by a.Journal_NoteNo_var, a.Journal_ClientId_int , a.Journal_Amount_dec " +
            " Having a.Journal_Amount_dec + sum(b.CashDetail_Amount_money) > 0) " +
             " ) as q " +
        " group by BILL_CL_Id,CL_Name_var";
                }
                SqlCommand cmd = new SqlCommand(mysql, con);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);
                dt = ds.Tables[0];
                ds.Dispose();
                return dt;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public DataTable getClientOutstanding3(int ClientId, string From_Date, string To_Date, int AsOnStatus)
        {
            SqlConnection con = new SqlConnection(cnStr);
            DataTable dt = new DataTable();
            try
            {
                DataSet ds = new DataSet();
                string mysql = "";
                if (AsOnStatus == 1)
                {
                    mysql = "Select a.CL_Id, a.CL_Name_var,a.CL_Limit_mny, a.CL_UnderReconciliation_bit, " +
           " isNULL ((Select sum(b.BILL_NetAmt_num) from tbl_Bill as b where a.CL_Id = b.BILL_CL_Id and b.BILL_Status_bit=0" +
               " And convert(date,b.BILL_Date_dt) <= convert(date,'" + To_Date + "') " +

           " ),0) as billAmount," +
           " isNULL ((Select sum(c.Journal_Amount_dec) from tbl_Journal as c where a.CL_Id = c.Journal_ClientId_int and c.Journal_Status_bit=0" +
               " And convert(date,c.Journal_Date_dt) <= convert(date,'" + To_Date + "') " +
               " And CHARINDEX('DB/', c.Journal_NoteNo_var,1) > 0" +
           " ),0) as journalDBAmount," +
           " isNULL ((Select sum(c.Journal_Amount_dec) from tbl_Journal as c where a.CL_Id = c.Journal_ClientId_int and c.Journal_Status_bit=0" +
               " And convert(date,c.Journal_Date_dt) <= convert(date,'" + To_Date + "') " +
               " And CHARINDEX('CR/', c.Journal_NoteNo_var,1) > 0" +
           " ),0) as journalCRAmount," +
           " isNULL ((Select sum(d.CashDetail_Amount_money) from tbl_CashDetail as d where a.CL_Id = d.CashDetail_ClientId and d.CashDetail_Status_bit=0" +
               " And convert(date,d.CashDetail_Date_date) <= convert(date,'" + To_Date + "') " +
           " ),0) as receivedAmount," +
           //" isNULL ((Select sum(d.CashDetail_Amount_money) from tbl_CashDetail as d where a.CL_Id = d.CashDetail_ClientId and d.CashDetail_Status_bit=0" +
           //    " And convert(date,d.CashDetail_Date_date) <= convert(date,'" + To_Date + "') " +
           //    " And (d.CashDetail_BillNo_int = 'On A/c' or (d.CashDetail_Settlement_var = 'On A/c' and CHARINDEX('TRF/', d.CashDetail_NoteNo_var )=0) " +
           //    " or (d.CashDetail_Settlement_var = 'On A/c' and CHARINDEX('TRF/', d.CashDetail_NoteNo_var )>0 and (d.CashDetail_BillNo_int is null or d.CashDetail_BillNo_int = '')) )" +
           //" ),0) as OnAccAmount" +

           //" isNULL ((Select sum(d.CashDetail_Amount_money) from tbl_CashDetail as d where a.CL_Id = d.CashDetail_ClientId and d.CashDetail_Status_bit=0" +
           //    " And convert(date,d.CashDetail_Date_date) <= convert(date,'" + To_Date + "') " +
           //    " And d.CashDetail_BillNo_int <> 'On A/c' And d.CashDetail_Settlement_var <> 'On A/c' " +
           //" ),0) as receivedAmount," +
           " isNULL ((Select sum(d.CashDetail_Amount_money) from tbl_CashDetail as d where a.CL_Id = d.CashDetail_ClientId and d.CashDetail_Status_bit=0" +
               " And convert(date,d.CashDetail_Date_date) <= convert(date,'" + To_Date + "') " +
               " And (d.CashDetail_BillNo_int = 'On A/c' or (d.CashDetail_Settlement_var = 'On A/c' and CHARINDEX('TRF/', d.CashDetail_NoteNo_var )=0) " +
               " or (d.CashDetail_Settlement_var = 'On A/c' and CHARINDEX('TRF/', d.CashDetail_NoteNo_var )>0 and (d.CashDetail_BillNo_int is null or d.CashDetail_BillNo_int = '')) )" +
           " ),0) as OnAccAmount" +


           " from tbl_Client as a" +
           " where " +
           " (" +
           " isNULL ((Select sum(b.BILL_NetAmt_num) from tbl_Bill as b where a.CL_Id = b.BILL_CL_Id and b.BILL_Status_bit=0" +
               " And convert(date,b.BILL_Date_dt) <= convert(date,'" + To_Date + "') " +
           " ),0) + " +
           " isNULL ((Select sum(c.Journal_Amount_dec) from tbl_Journal as c where a.CL_Id = c.Journal_ClientId_int and c.Journal_Status_bit=0" +
               " And convert(date,c.Journal_Date_dt) <= convert(date,'" + To_Date + "') " +
               " And CHARINDEX('DB/', c.Journal_NoteNo_var,1) > 0" +
           " ),0) 	" +
           " +" +
           //" isNULL ((Select sum(d.CashDetail_Amount_money) from tbl_CashDetail as d where a.CL_Id = d.CashDetail_ClientId and d.CashDetail_Status_bit=0" +
           //    " And convert(date,d.CashDetail_Date_date) <= convert(date,'" + To_Date + "') " +
           //" ),0) " +
           //" - " +
           //" isNULL ((Select sum(d.CashDetail_Amount_money) from tbl_CashDetail as d where a.CL_Id = d.CashDetail_ClientId and d.CashDetail_Status_bit=0" +
           //    " And convert(date,d.CashDetail_Date_date) <= convert(date,'" + To_Date + "') " +
           //    " And (d.CashDetail_BillNo_int = 'On A/c' or (d.CashDetail_Settlement_var = 'On A/c' and CHARINDEX('TRF/', d.CashDetail_NoteNo_var )=0) " +
           //    " or (d.CashDetail_Settlement_var = 'On A/c' and CHARINDEX('TRF/', d.CashDetail_NoteNo_var )>0 and (d.CashDetail_BillNo_int is null or d.CashDetail_BillNo_int = '')) )" +
           //" ),0) " +
           " isNULL ((Select sum(d.CashDetail_Amount_money) from tbl_CashDetail as d where a.CL_Id = d.CashDetail_ClientId and d.CashDetail_Status_bit=0" +
               " And convert(date,d.CashDetail_Date_date) <= convert(date,'" + To_Date + "') " +
               " And d.CashDetail_BillNo_int <> 'On A/c' And d.CashDetail_Settlement_var <> 'On A/c' " +
           " ),0) " +
           " - " +
           " isNULL ((Select sum(d.CashDetail_Amount_money) from tbl_CashDetail as d where a.CL_Id = d.CashDetail_ClientId and d.CashDetail_Status_bit=0" +
               " And convert(date,d.CashDetail_Date_date) <= convert(date,'" + To_Date + "') " +
               " And (d.CashDetail_BillNo_int = 'On A/c' or (d.CashDetail_Settlement_var = 'On A/c' and CHARINDEX('TRF/', d.CashDetail_NoteNo_var )=0) " +
               " or (d.CashDetail_Settlement_var = 'On A/c' and CHARINDEX('TRF/', d.CashDetail_NoteNo_var )>0 ) )" +
           " ),0) " +
           " )> 0 and (a.CL_Id = " + ClientId + " or " + ClientId + " = 0)" +
           " order by a.CL_Name_var";
                }
                else
                {
                    mysql = "Select a.CL_Id, a.CL_Name_var,a.CL_Limit_mny, a.CL_UnderReconciliation_bit, " +
            " isNULL ((Select sum(b.BILL_NetAmt_num) from tbl_Bill as b where a.CL_Id = b.BILL_CL_Id and b.BILL_Status_bit=0" +
                " And convert(date,b.BILL_Date_dt) >= convert(date,'" + From_Date + "') and convert(date,b.BILL_Date_dt) <= convert(date,'" + To_Date + "')" +
            " ),0) as billAmount," +
            " isNULL ((Select sum(c.Journal_Amount_dec) from tbl_Journal as c where a.CL_Id = c.Journal_ClientId_int and c.Journal_Status_bit=0" +
                " And convert(date,c.Journal_Date_dt) >= convert(date,'" + From_Date + "') and convert(date,c.Journal_Date_dt) <= convert(date,'" + To_Date + "')" +
                " And CHARINDEX('DB/', c.Journal_NoteNo_var,1) > 0" +
            " ),0) as journalDBAmount," +
            " isNULL ((Select sum(c.Journal_Amount_dec) from tbl_Journal as c where a.CL_Id = c.Journal_ClientId_int and c.Journal_Status_bit=0" +
                " And convert(date,c.Journal_Date_dt) >= convert(date,'" + From_Date + "') and convert(date,c.Journal_Date_dt) <= convert(date,'" + To_Date + "')" +
                " And CHARINDEX('CR/', c.Journal_NoteNo_var,1) > 0" +
            " ),0) as journalCRAmount," +
            " isNULL ((Select sum(d.CashDetail_Amount_money) from tbl_CashDetail as d where a.CL_Id = d.CashDetail_ClientId and d.CashDetail_Status_bit=0" +
                " And convert(date,d.CashDetail_Date_date) >= convert(date,'" + From_Date + "') and convert(date,d.CashDetail_Date_date) <= convert(date,'" + To_Date + "')" +
            " ),0) as receivedAmount," +
            // " isNULL ((Select sum(d.CashDetail_Amount_money) from tbl_CashDetail as d where a.CL_Id = d.CashDetail_ClientId and d.CashDetail_Status_bit=0" +
            //    " And convert(date,d.CashDetail_Date_date) >= convert(date,'" + From_Date + "') and convert(date,d.CashDetail_Date_date) <= convert(date,'" + To_Date + "')" +
            //    " And (d.CashDetail_BillNo_int = 'On A/c' or (d.CashDetail_Settlement_var = 'On A/c' and CHARINDEX('TRF/', d.CashDetail_NoteNo_var )=0) " +
            //    " or (d.CashDetail_Settlement_var = 'On A/c' and CHARINDEX('TRF/', d.CashDetail_NoteNo_var )>0 and (d.CashDetail_BillNo_int is null or d.CashDetail_BillNo_int = '')) )" +
            //" ),0) as OnAccAmount" +

            //" isNULL ((Select sum(d.CashDetail_Amount_money) from tbl_CashDetail as d where a.CL_Id = d.CashDetail_ClientId and d.CashDetail_Status_bit=0" +
            //     " And convert(date,d.CashDetail_Date_date) >= convert(date,'" + From_Date + "') and convert(date,d.CashDetail_Date_date) <= convert(date,'" + To_Date + "')" +
            //     " And d.CashDetail_BillNo_int <> 'On A/c' And d.CashDetail_Settlement_var <> 'On A/c' " +
            // " ),0) as receivedAmount," +
            " isNULL ((Select sum(d.CashDetail_Amount_money) from tbl_CashDetail as d where a.CL_Id = d.CashDetail_ClientId and d.CashDetail_Status_bit=0" +
               " And convert(date,d.CashDetail_Date_date) >= convert(date,'" + From_Date + "') and convert(date,d.CashDetail_Date_date) <= convert(date,'" + To_Date + "')" +
               " And (d.CashDetail_BillNo_int = 'On A/c' or (d.CashDetail_Settlement_var = 'On A/c' and CHARINDEX('TRF/', d.CashDetail_NoteNo_var )=0) " +
               " or (d.CashDetail_Settlement_var = 'On A/c' and CHARINDEX('TRF/', d.CashDetail_NoteNo_var )>0 and (d.CashDetail_BillNo_int is null or d.CashDetail_BillNo_int = '')) )" +
           " ),0) as OnAccAmount" +

            " from tbl_Client as a" +
            " where " +
            " (" +
            " isNULL ((Select sum(b.BILL_NetAmt_num) from tbl_Bill as b where a.CL_Id = b.BILL_CL_Id and b.BILL_Status_bit=0" +
                " And convert(date,b.BILL_Date_dt) >= convert(date,'" + From_Date + "') and convert(date,b.BILL_Date_dt) <= convert(date,'" + To_Date + "')" +
            " ),0) + " +
            " isNULL ((Select sum(c.Journal_Amount_dec) from tbl_Journal as c where a.CL_Id = c.Journal_ClientId_int and c.Journal_Status_bit=0" +
                " And convert(date,c.Journal_Date_dt) >= convert(date,'" + From_Date + "') and convert(date,c.Journal_Date_dt) <= convert(date,'" + To_Date + "')" +
                " And CHARINDEX('DB/', c.Journal_NoteNo_var,1) > 0" +
            " ),0) 	" +
            " +" +
            " isNULL ((Select sum(d.CashDetail_Amount_money) from tbl_CashDetail as d where a.CL_Id = d.CashDetail_ClientId and d.CashDetail_Status_bit=0" +
                " And convert(date,d.CashDetail_Date_date) >= convert(date,'" + From_Date + "') and convert(date,d.CashDetail_Date_date) <= convert(date,'" + To_Date + "')" +
                " And d.CashDetail_BillNo_int <> 'On A/c' And d.CashDetail_Settlement_var <> 'On A/c' " +
            " ),0) " +
            " - " +
           " isNULL ((Select sum(d.CashDetail_Amount_money) from tbl_CashDetail as d where a.CL_Id = d.CashDetail_ClientId and d.CashDetail_Status_bit=0" +
               " And convert(date,d.CashDetail_Date_date) >= convert(date,'" + From_Date + "') and convert(date,d.CashDetail_Date_date) <= convert(date,'" + To_Date + "')" +
               " And (d.CashDetail_BillNo_int = 'On A/c' or (d.CashDetail_Settlement_var = 'On A/c' and CHARINDEX('TRF/', d.CashDetail_NoteNo_var )=0) " +
               " or (d.CashDetail_Settlement_var = 'On A/c' and CHARINDEX('TRF/', d.CashDetail_NoteNo_var )>0 ) )" +
           " ),0) " +
            " )> 0 and (a.CL_Id = " + ClientId + " or " + ClientId + " = 0)" +
            " order by a.CL_Name_var";
                }
                SqlCommand cmd = new SqlCommand(mysql, con);
                //SqlCommand cmd = new SqlCommand("Client_Outstanding", con);
                //cmd.Parameters.Add("@ClientId", SqlDbType.Int, 4).Value = ClientId;
                //cmd.Parameters.Add("@From_Date", SqlDbType.DateTime, 10).Value = From_Date;
                //cmd.Parameters.Add("@To_Date", SqlDbType.DateTime, 10).Value = To_Date;
                //cmd.Parameters.Add("@AsOnStatus", SqlDbType.Bit).Value = AsOnStatus;
                //cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);
                dt = ds.Tables[0];
                ds.Dispose();
                return dt;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public DataTable getClientSiteOutstanding2(int ClientId, string From_Date, string To_Date, int AsOnStatus)
        {
            SqlConnection con = new SqlConnection(cnStr);
            DataTable dt = new DataTable();
            try
            {
                DataSet ds = new DataSet();

                string mysql = "";
                if (AsOnStatus == 1)
                {
                    mysql = "select BILL_CL_Id,CL_Name_var,BILL_SITE_Id,SITE_Name_var from " +
            " ( " +
            " (Select convert(varchar,a.BILL_Id) as BILL_Id, a.BILL_CL_Id,a.BILL_NetAmt_num, 0 as CashDetail_Amount_money, a.BILL_SITE_Id, " +
            " (select c.CL_Name_var from tbl_Client as c where c.CL_Id = a.BILL_CL_Id) as CL_Name_var, " +
            " (select c.SITE_Name_var from tbl_Site as c where c.SITE_Id = a.BILL_SITE_Id) as SITE_Name_var " +
            " from tbl_Bill as a " +
            " where a.BILL_Status_bit = 0 and (a.BILL_CL_Id = " + ClientId + " or " + ClientId + " = 0) and a.BILL_CL_Id > 0" +
            " And convert(date,a.BILL_Date_dt) <= convert(date,'" + To_Date + "')  " +
            " AND convert(varchar,a.BILL_Id) NOT IN  " +
            " (SELECT b.CashDetail_BillNo_int from tbl_CashDetail as b where b.CashDetail_Status_bit= 0 " +
            " And  convert(date, b.CashDetail_Date_date) <= convert(date,'" + To_Date + "')  " +
            " ) " +
            " Group by a.BILL_Id, a.BILL_NetAmt_num,a.BILL_CL_Id, a.BILL_SITE_Id ) " +

            " Union " +

            " (Select convert(varchar,a.BILL_Id) as BILL_Id, a.BILL_CL_Id,a.BILL_NetAmt_num,sum(b.CashDetail_Amount_money), a.BILL_SITE_Id, " +
            " (select c.CL_Name_var from tbl_Client as c where c.CL_Id = a.BILL_CL_Id) as CL_Name_var, " +
            " (select c.SITE_Name_var from tbl_Site as c where c.SITE_Id = a.BILL_SITE_Id) as SITE_Name_var " +
            " from tbl_Bill as a, tbl_CashDetail as b " +
            " where a.BILL_Status_bit = 0 and (a.BILL_CL_Id = " + ClientId + " or " + ClientId + " = 0)	 and a.BILL_CL_Id > 0" +
            " And convert(date,a.BILL_Date_dt) <= convert(date,'" + To_Date + "')  " +
            " And convert(date, b.CashDetail_Date_date) <= convert(date,'" + To_Date + "')  " +
            " and convert(varchar,a.BILL_Id) = b.CashDetail_BillNo_int  " +
            " and b.CashDetail_Status_bit = 0	 " +
            " Group by a.BILL_Id, a.BILL_CL_Id, a.BILL_NetAmt_num, a.BILL_SITE_Id " +
            " Having a.BILL_NetAmt_num + sum(b.CashDetail_Amount_money) > 0) " +

            " Union " +

            " (Select a.Journal_NoteNo_var as BILL_Id,a.Journal_ClientId_int as BILL_CL_Id, a.Journal_Amount_dec 	 as BILL_NetAmt_num, 	0 as CashDetail_Amount_money, a.Journal_SiteId_int	as BILL_SITE_Id, " +
            " (select c.CL_Name_var from tbl_Client as c where c.CL_Id = a.Journal_ClientId_int) as CL_Name_var, " +
            " (select c.SITE_Name_var from tbl_Site as c where c.SITE_Id = a.Journal_SiteId_int) as SITE_Name_var " +
            " from tbl_Journal as a " +
            " where a.Journal_Status_bit = 0 and (a.Journal_ClientId_int = " + ClientId + " or " + ClientId + " = 0) and a.Journal_ClientId_int > 0" +
            " And convert(date,a.Journal_Date_dt) <= convert(date,'" + To_Date + "') " +
            " And CHARINDEX('DB/',a.Journal_NoteNo_var) > 0 " +
            " AND a.Journal_NoteNo_var NOT IN  " +
            " (SELECT b.CashDetail_BillNo_int from tbl_CashDetail as b where b.CashDetail_Status_bit= 0) " +
            " Group by a.Journal_NoteNo_var, a.Journal_Amount_dec,a.Journal_ClientId_int, a.Journal_SiteId_int) " +

            " Union " +

            " (Select a.Journal_NoteNo_var as BILL_Id, a.Journal_ClientId_int as BILL_CL_Id,	 a.Journal_Amount_dec 	 as BILL_NetAmt_num, 	0 as CashDetail_Amount_money, a.Journal_SiteId_int as BILL_SITE_Id, " +
            " (select c.CL_Name_var from tbl_Client as c where c.CL_Id = a.Journal_ClientId_int) as CL_Name_var, " +
            " (select c.SITE_Name_var from tbl_Site as c where c.SITE_Id = a.Journal_SiteId_int) as SITE_Name_var " +
            " from tbl_Journal as a, tbl_CashDetail as b " +
            " where a.Journal_Status_bit = 0 and (a.Journal_ClientId_int = " + ClientId + " or " + ClientId + " = 0)	and a.Journal_ClientId_int > 0 " +
            " And convert(date,a.Journal_Date_dt) <= convert(date,'" + To_Date + "')  " +
            " And convert(date, b.CashDetail_Date_date) <= convert(date,'" + To_Date + "')  " +
            " and a.Journal_NoteNo_var = b.CashDetail_BillNo_int  " +
            " and b.CashDetail_Status_bit = 0	 " +
            " And CHARINDEX('DB/',a.Journal_NoteNo_var) > 0 " +
            " Group by a.Journal_NoteNo_var, a.Journal_ClientId_int , a.Journal_Amount_dec, a.Journal_SiteId_int " +
            " Having a.Journal_Amount_dec + sum(b.CashDetail_Amount_money) > 0) " +
             " ) as q " +
        " group by BILL_CL_Id,CL_Name_var,BILL_SITE_Id,SITE_Name_var";
                }
                else
                {
                    mysql = "select BILL_CL_Id,CL_Name_var,BILL_SITE_Id,SITE_Name_var from " +
            " ( " +
            " (Select convert(varchar,a.BILL_Id) as BILL_Id, a.BILL_CL_Id,a.BILL_NetAmt_num, 0 as CashDetail_Amount_money, a.BILL_SITE_Id, " +
            " (select c.CL_Name_var from tbl_Client as c where c.CL_Id = a.BILL_CL_Id) as CL_Name_var, " +
            " (select c.SITE_Name_var from tbl_Site as c where c.SITE_Id = a.BILL_SITE_Id) as SITE_Name_var " +
            " from tbl_Bill as a " +
            " where a.BILL_Status_bit = 0 and (a.BILL_CL_Id = " + ClientId + " or " + ClientId + " = 0) and a.BILL_CL_Id > 0 " +
            " And ((convert(date,a.BILL_Date_dt) >= convert(date,'" + From_Date + "') and convert(date,a.BILL_Date_dt) <= convert(date,'" + To_Date + "')) ) " +
            " AND convert(varchar,a.BILL_Id) NOT IN  " +
            " (SELECT b.CashDetail_BillNo_int from tbl_CashDetail as b where b.CashDetail_Status_bit= 0 " +
            " And ((convert(date,b.CashDetail_Date_date) >= convert(date,'" + From_Date + "') and convert(date,b.CashDetail_Date_date) <= convert(date,'" + To_Date + "')) ) " +
            " ) " +
            " Group by a.BILL_Id, a.BILL_NetAmt_num,a.BILL_CL_Id, a.BILL_SITE_Id ) " +

            " Union " +

            " (Select convert(varchar,a.BILL_Id) as BILL_Id, a.BILL_CL_Id,a.BILL_NetAmt_num,sum(b.CashDetail_Amount_money), a.BILL_SITE_Id, " +
            " (select c.CL_Name_var from tbl_Client as c where c.CL_Id = a.BILL_CL_Id) as CL_Name_var, " +
            " (select c.SITE_Name_var from tbl_Site as c where c.SITE_Id = a.BILL_SITE_Id) as SITE_Name_var " +
            " from tbl_Bill as a, tbl_CashDetail as b " +
            " where a.BILL_Status_bit = 0 and (a.BILL_CL_Id = " + ClientId + " or " + ClientId + " = 0)	and a.BILL_CL_Id > 0 " +
            " And ((convert(date,a.BILL_Date_dt) >= convert(date,'" + From_Date + "') and convert(date,a.BILL_Date_dt) <= convert(date,'" + To_Date + "')) ) " +
            " And ((convert(date,b.CashDetail_Date_date) >= convert(date,'" + From_Date + "') and convert(date,b.CashDetail_Date_date) <= convert(date,'" + To_Date + "')) ) " +
            " and convert(varchar,a.BILL_Id) = b.CashDetail_BillNo_int  " +
            " and b.CashDetail_Status_bit = 0	 " +
            " Group by a.BILL_Id, a.BILL_CL_Id, a.BILL_NetAmt_num, a.BILL_SITE_Id " +
            " Having a.BILL_NetAmt_num + sum(b.CashDetail_Amount_money) > 0) " +

            " Union " +

            " (Select a.Journal_NoteNo_var as BILL_Id,a.Journal_ClientId_int as BILL_CL_Id, a.Journal_Amount_dec 	 as BILL_NetAmt_num, 	0 as CashDetail_Amount_money, a.Journal_SiteId_int	as BILL_SITE_Id,  " +
            " (select c.CL_Name_var from tbl_Client as c where c.CL_Id = a.Journal_ClientId_int) as CL_Name_var, " +
            " (select c.SITE_Name_var from tbl_Site as c where c.SITE_Id = a.Journal_SiteId_int) as SITE_Name_var " +
            " from tbl_Journal as a " +
            " where a.Journal_Status_bit = 0 and (a.Journal_ClientId_int = " + ClientId + " or " + ClientId + " = 0) and a.Journal_ClientId_int > 0" +
            " And ((convert(date,a.Journal_Date_dt) >= convert(date,'" + From_Date + "') and convert(date,a.Journal_Date_dt) <= convert(date,'" + To_Date + "')) ) " +
            " And CHARINDEX('DB/',a.Journal_NoteNo_var) > 0 " +
            " AND a.Journal_NoteNo_var NOT IN  " +
            " (SELECT b.CashDetail_BillNo_int from tbl_CashDetail as b where b.CashDetail_Status_bit='false') " +
            " Group by a.Journal_NoteNo_var, a.Journal_Amount_dec,a.Journal_ClientId_int, a.Journal_SiteId_int) " +

            " Union " +

            " (Select a.Journal_NoteNo_var as BILL_Id, a.Journal_ClientId_int as BILL_CL_Id,	 a.Journal_Amount_dec 	 as BILL_NetAmt_num, 	0 as CashDetail_Amount_money, a.Journal_SiteId_int	as BILL_SITE_Id, 	 " +
            " (select c.CL_Name_var from tbl_Client as c where c.CL_Id = a.Journal_ClientId_int) as CL_Name_var, " +
            " (select c.SITE_Name_var from tbl_Site as c where c.SITE_Id = a.Journal_SiteId_int) as SITE_Name_var " +
            " from tbl_Journal as a, tbl_CashDetail as b " +
            " where a.Journal_Status_bit = 0 and (a.Journal_ClientId_int = " + ClientId + " or " + ClientId + " = 0) and a.Journal_ClientId_int > 0	 " +
            " And ((convert(date,a.Journal_Date_dt) >= convert(date,'" + From_Date + "') and convert(date,a.Journal_Date_dt) <= convert(date,'" + To_Date + "')) ) " +
            " And ((convert(date,b.CashDetail_Date_date) >= convert(date,'" + From_Date + "') and convert(date,b.CashDetail_Date_date) <= convert(date,'" + To_Date + "')) ) " +
            " and a.Journal_NoteNo_var = b.CashDetail_BillNo_int  " +
            " and b.CashDetail_Status_bit = 0	 " +
            " And CHARINDEX('DB/',a.Journal_NoteNo_var) > 0 " +
            " Group by a.Journal_NoteNo_var, a.Journal_ClientId_int , a.Journal_Amount_dec, a.Journal_SiteId_int " +
            " Having a.Journal_Amount_dec + sum(b.CashDetail_Amount_money) > 0) " +
             " ) as q " +
        " group by BILL_CL_Id,CL_Name_var,BILL_SITE_Id,SITE_Name_var";
                }
                SqlCommand cmd = new SqlCommand(mysql, con);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);
                dt = ds.Tables[0];
                ds.Dispose();
                return dt;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public DataTable getClientSiteOutstanding3(int ClientId, int SiteId, string From_Date, string To_Date, int AsOnStatus)
        {
            SqlConnection con = new SqlConnection(cnStr);
            DataTable dt = new DataTable();
            try
            {
                DataSet ds = new DataSet();
                string mysql = "";
                if (AsOnStatus == 1)
                {
                    mysql = "Select a.CL_Id, a.CL_Name_var,a.CL_Limit_mny, b.SITE_Id, b.SITE_Name_var, " +
            //" (select c.USER_Name_var from tbl_User as c where c.USER_Id = b.SITE_MEID_int) as mktUser, " +
            " (select u.USER_Name_var from tbl_Route as rt, tbl_User as u  where b.SITE_Route_Id = rt.ROUTE_Id and rt.ROUTE_ME_Id = u.USER_Id ) as mktUser ," +
            " (select c.ROUTE_Name_var from tbl_Route as c where c.ROUTE_Id = b.SITE_Route_Id) as routeName, " +
           "isNULL ((Select sum(b.BILL_NetAmt_num) from tbl_Bill as b where a.CL_Id = b.BILL_CL_Id and b.BILL_Status_bit=0" +
           " And convert(date,b.BILL_Date_dt) <= convert(date,'" + To_Date + "') " +
           " ),0) as billAmount," +
           " isNULL ((Select sum(c.Journal_Amount_dec) from tbl_Journal as c where a.CL_Id = c.Journal_ClientId_int and c.Journal_Status_bit=0" +
               " And convert(date,c.Journal_Date_dt) <= convert(date,'" + To_Date + "') " +
               " And CHARINDEX('DB/', c.Journal_NoteNo_var,1) > 0" +
           " ),0) as journalDBAmount," +
           " isNULL ((Select sum(bl.BILL_NetAmt_num) from tbl_Bill as bl where a.CL_Id = bl.BILL_CL_Id and bl.BILL_SITE_Id = b.SITE_Id and bl.BILL_Status_bit=0" +
               " And convert(date,bl.BILL_Date_dt) <= convert(date,'" + To_Date + "') " +
           " ),0) as billAmountSite," +
           " isNULL ((Select sum(c.Journal_Amount_dec) from tbl_Journal as c where a.CL_Id = c.Journal_ClientId_int and c.Journal_SiteId_int = b.SITE_Id and c.Journal_Status_bit=0" +
               " And convert(date,c.Journal_Date_dt) <= convert(date,'" + To_Date + "') " +
               " And CHARINDEX('DB/', c.Journal_NoteNo_var,1) > 0" +
           " ),0) as journalDBAmountSite," +
           " isNULL ((Select sum(d.CashDetail_Amount_money) from tbl_CashDetail as d, tbl_Bill as bl where  d.CashDetail_BillNo_int =bl.BILL_Id and d.CashDetail_ClientId= a.CL_Id and bl.BILL_SITE_Id = b.SITE_Id and d.CashDetail_Status_bit=0 and bl.BILL_Status_bit=0" +
               " And convert(date,d.CashDetail_Date_date) <= convert(date,'" + To_Date + "') " +
           " ),0) as receivedAmountBill," +
           " isNULL ((Select sum(d.CashDetail_Amount_money) from tbl_CashDetail as d, tbl_Journal as bl where  d.CashDetail_BillNo_int =bl.Journal_NoteNo_var and d.CashDetail_ClientId= a.CL_Id and bl.Journal_SiteId_int = b.SITE_Id and d.CashDetail_Status_bit=0 and bl.Journal_Status_bit=0" +
               " And convert(date,d.CashDetail_Date_date) <= convert(date,'" + To_Date + "') " +
               " And CHARINDEX('DB/', bl.Journal_NoteNo_var,1) > 0" +
           " ),0) as receivedAmountJournal," +
           " isNULL ((Select sum(d.CashDetail_Amount_money) from tbl_CashDetail as d where a.CL_Id = d.CashDetail_ClientId and d.CashDetail_Status_bit=0" +
               " And convert(date,d.CashDetail_Date_date) <= convert(date,'" + To_Date + "') " +
           " ),0) as receivedAmount," +
           " isNULL ((Select sum(d.CashDetail_Amount_money) from tbl_CashDetail as d where a.CL_Id = d.CashDetail_ClientId and d.CashDetail_Status_bit=0" +
               " And convert(date,d.CashDetail_Date_date) <= convert(date,'" + To_Date + "') " +
               " And (d.CashDetail_BillNo_int = 'On A/c' or (d.CashDetail_Settlement_var = 'On A/c' and CHARINDEX('TRF/', d.CashDetail_NoteNo_var )=0) " +
               " or (d.CashDetail_Settlement_var = 'On A/c' and CHARINDEX('TRF/', d.CashDetail_NoteNo_var )>0 ) )" +
           " ),0) as OnAccAmount" +

           " from tbl_Client as a, tbl_Site as b" +
           " where a.CL_Id = b.SITE_CL_Id And " +
           " (" +
           " isNULL ((Select sum(bl.BILL_NetAmt_num) from tbl_Bill as bl where a.CL_Id = bl.BILL_CL_Id and bl.BILL_SITE_Id = b.SITE_Id and bl.BILL_Status_bit=0" +
               " And convert(date,bl.BILL_Date_dt) <= convert(date,'" + To_Date + "') " +
           " ),0) + " +
           " isNULL ((Select sum(c.Journal_Amount_dec) from tbl_Journal as c where a.CL_Id = c.Journal_ClientId_int and c.Journal_SiteId_int = b.SITE_Id and c.Journal_Status_bit=0" +
               " And convert(date,c.Journal_Date_dt) <= convert(date,'" + To_Date + "') " +
               " And CHARINDEX('DB/', c.Journal_NoteNo_var,1) > 0" +
           " ),0) 	" +
           " +" +
           " isNULL ((Select sum(d.CashDetail_Amount_money) from tbl_CashDetail as d, tbl_Bill as bl where  d.CashDetail_BillNo_int =bl.BILL_Id and d.CashDetail_ClientId= a.CL_Id and bl.BILL_SITE_Id = b.SITE_Id and d.CashDetail_Status_bit=0 and bl.BILL_Status_bit=0" +
               " And convert(date,d.CashDetail_Date_date) <= convert(date,'" + To_Date + "') " +
           " ),0) " +
           " +" +
           " isNULL ((Select sum(d.CashDetail_Amount_money) from tbl_CashDetail as d, tbl_Journal as bl where  d.CashDetail_BillNo_int =bl.Journal_NoteNo_var and d.CashDetail_ClientId= a.CL_Id and bl.Journal_SiteId_int = b.SITE_Id and d.CashDetail_Status_bit=0 and bl.Journal_Status_bit=0" +
               " And convert(date,d.CashDetail_Date_date) <= convert(date,'" + To_Date + "') " +
               " And CHARINDEX('DB/', bl.Journal_NoteNo_var,1) > 0" +
           " ),0) " +
           //" isNULL ((Select sum(d.CashDetail_Amount_money) from tbl_CashDetail as d where a.CL_Id = d.CashDetail_ClientId and d.CashDetail_Status_bit=0" +
           //     " And convert(date,d.CashDetail_Date_date) <= convert(date,'" + To_Date + "') " +
           //     " And d.CashDetail_BillNo_int <> 'On A/c' And d.CashDetail_Settlement_var <> 'On A/c' " +
           // " ),0) " +
           // " - " +
           // " isNULL ((Select sum(d.CashDetail_Amount_money) from tbl_CashDetail as d where a.CL_Id = d.CashDetail_ClientId and d.CashDetail_Status_bit=0" +
           //     " And convert(date,d.CashDetail_Date_date) <= convert(date,'" + To_Date + "') " +
           //     " And (d.CashDetail_BillNo_int = 'On A/c' or (d.CashDetail_Settlement_var = 'On A/c' and CHARINDEX('TRF/', d.CashDetail_NoteNo_var )=0) " +
           //     " or (d.CashDetail_Settlement_var = 'On A/c' and CHARINDEX('TRF/', d.CashDetail_NoteNo_var )>0 ) )" +
           // " ),0) " +

           " )> 0 and (a.CL_Id = " + ClientId + " or " + ClientId + " = 0)" +
           " and (b.SITE_Id = " + SiteId + " or " + SiteId + " = 0)" +
           " order by a.CL_Name_var, b.SITE_Name_var";
                }
                else
                {
                    mysql = "Select a.CL_Id, a.CL_Name_var,a.CL_Limit_mny, b.SITE_Id, b.SITE_Name_var, " +
            //" (select c.USER_Name_var from tbl_User as c where c.USER_Id = b.SITE_MEID_int) as mktUser, " +
            " (select u.USER_Name_var from tbl_Route as rt, tbl_User as u  where b.SITE_Route_Id = rt.ROUTE_Id and rt.ROUTE_ME_Id = u.USER_Id ) as mktUser ," +
            " (select c.ROUTE_Name_var from tbl_Route as c where c.ROUTE_Id = b.SITE_Route_Id) as routeName, " +
            " isNULL ((Select sum(b.BILL_NetAmt_num) from tbl_Bill as b where a.CL_Id = b.BILL_CL_Id and b.BILL_Status_bit=0" +
                " And convert(date,b.BILL_Date_dt) >= convert(date,'" + From_Date + "') and convert(date,b.BILL_Date_dt) <= convert(date,'" + To_Date + "')" +
            " ),0) as billAmount," +
            " isNULL ((Select sum(c.Journal_Amount_dec) from tbl_Journal as c where a.CL_Id = c.Journal_ClientId_int and c.Journal_Status_bit=0" +
                " And convert(date,c.Journal_Date_dt) >= convert(date,'" + From_Date + "') and convert(date,c.Journal_Date_dt) <= convert(date,'" + To_Date + "')" +
                " And CHARINDEX('DB/', c.Journal_NoteNo_var,1) > 0" +
            " ),0) as journalDBAmount," +
            " isNULL ((Select sum(bl.BILL_NetAmt_num) from tbl_Bill as bl where a.CL_Id = bl.BILL_CL_Id and bl.BILL_SITE_Id = b.SITE_Id and bl.BILL_Status_bit=0" +
                " And convert(date,bl.BILL_Date_dt) >= convert(date,'" + From_Date + "') and convert(date,bl.BILL_Date_dt) <= convert(date,'" + To_Date + "')" +
            " ),0) as billAmountSite," +
            " isNULL ((Select sum(c.Journal_Amount_dec) from tbl_Journal as c where a.CL_Id = c.Journal_ClientId_int and c.Journal_SiteId_int = b.SITE_Id and c.Journal_Status_bit=0" +
                " And convert(date,c.Journal_Date_dt) >= convert(date,'" + From_Date + "') and convert(date,c.Journal_Date_dt) <= convert(date,'" + To_Date + "')" +
                " And CHARINDEX('DB/', c.Journal_NoteNo_var,1) > 0" +
            " ),0) as journalDBAmountSite," +
            " isNULL ((Select sum(d.CashDetail_Amount_money) from tbl_CashDetail as d, tbl_Bill as bl where  d.CashDetail_BillNo_int =bl.BILL_Id and d.CashDetail_ClientId= a.CL_Id and bl.BILL_SITE_Id = b.SITE_Id and d.CashDetail_Status_bit=0 and bl.BILL_Status_bit=0" +
               " And convert(date,d.CashDetail_Date_date) >= convert(date,'" + From_Date + "') And convert(date,d.CashDetail_Date_date) <= convert(date,'" + To_Date + "') " +
           " ),0) as receivedAmountBill," +
           " isNULL ((Select sum(d.CashDetail_Amount_money) from tbl_CashDetail as d, tbl_Journal as bl where  d.CashDetail_BillNo_int =bl.Journal_NoteNo_var and d.CashDetail_ClientId= a.CL_Id and bl.Journal_SiteId_int = b.SITE_Id and d.CashDetail_Status_bit=0 and bl.Journal_Status_bit=0" +
               " And convert(date,d.CashDetail_Date_date) >= convert(date,'" + From_Date + "') And convert(date,d.CashDetail_Date_date) <= convert(date,'" + To_Date + "') " +
               " And CHARINDEX('DB/', bl.Journal_NoteNo_var,1) > 0" +
           " ),0) as receivedAmountJournal," +
           " isNULL ((Select sum(d.CashDetail_Amount_money) from tbl_CashDetail as d where a.CL_Id = d.CashDetail_ClientId and d.CashDetail_Status_bit=0" +
                " And convert(date,d.CashDetail_Date_date) >= convert(date,'" + From_Date + "') and convert(date,d.CashDetail_Date_date) <= convert(date,'" + To_Date + "')" +
            " ),0) as receivedAmount," +
            " isNULL ((Select sum(d.CashDetail_Amount_money) from tbl_CashDetail as d where a.CL_Id = d.CashDetail_ClientId and d.CashDetail_Status_bit=0" +
               " And convert(date,d.CashDetail_Date_date) >= convert(date,'" + From_Date + "') and convert(date,d.CashDetail_Date_date) <= convert(date,'" + To_Date + "')" +
               " And (d.CashDetail_BillNo_int = 'On A/c' or (d.CashDetail_Settlement_var = 'On A/c' and CHARINDEX('TRF/', d.CashDetail_NoteNo_var )=0) " +
               " or (d.CashDetail_Settlement_var = 'On A/c' and CHARINDEX('TRF/', d.CashDetail_NoteNo_var )>0 ) )" +
           " ),0) as OnAccAmount" +

            " from tbl_Client as a, tbl_Site as b" +
            " where a.CL_Id = b.SITE_CL_Id And " +
            " (" +
            " isNULL ((Select sum(bl.BILL_NetAmt_num) from tbl_Bill as bl where a.CL_Id = bl.BILL_CL_Id and bl.BILL_SITE_Id = b.SITE_Id and bl.BILL_Status_bit=0" +
                " And convert(date,bl.BILL_Date_dt) >= convert(date,'" + From_Date + "') and convert(date,bl.BILL_Date_dt) <= convert(date,'" + To_Date + "')" +
            " ),0) + " +
            " isNULL ((Select sum(c.Journal_Amount_dec) from tbl_Journal as c where a.CL_Id = c.Journal_ClientId_int and c.Journal_SiteId_int = b.SITE_Id and c.Journal_Status_bit=0" +
                " And convert(date,c.Journal_Date_dt) >= convert(date,'" + From_Date + "') and convert(date,c.Journal_Date_dt) <= convert(date,'" + To_Date + "')" +
                " And CHARINDEX('DB/', c.Journal_NoteNo_var,1) > 0" +
            " ),0) 	" +
            " +" +
            " isNULL ((Select sum(d.CashDetail_Amount_money) from tbl_CashDetail as d, tbl_Bill as bl where  d.CashDetail_BillNo_int =bl.BILL_Id and d.CashDetail_ClientId= a.CL_Id and bl.BILL_SITE_Id = b.SITE_Id and d.CashDetail_Status_bit=0 and bl.BILL_Status_bit=0" +
               " And convert(date,d.CashDetail_Date_date) >= convert(date,'" + From_Date + "') And convert(date,d.CashDetail_Date_date) <= convert(date,'" + To_Date + "') " +
           " ),0) " +
           " +" +
           " isNULL ((Select sum(d.CashDetail_Amount_money) from tbl_CashDetail as d, tbl_Journal as bl where  d.CashDetail_BillNo_int =bl.Journal_NoteNo_var and d.CashDetail_ClientId= a.CL_Id and bl.Journal_SiteId_int = b.SITE_Id and d.CashDetail_Status_bit=0 and bl.Journal_Status_bit=0" +
               " And convert(date,d.CashDetail_Date_date) >= convert(date,'" + From_Date + "') And convert(date,d.CashDetail_Date_date) <= convert(date,'" + To_Date + "') " +
               " And CHARINDEX('DB/', bl.Journal_NoteNo_var,1) > 0" +
           " ),0) " +
            //" isNULL ((Select sum(d.CashDetail_Amount_money) from tbl_CashDetail as d where a.CL_Id = d.CashDetail_ClientId and d.CashDetail_Status_bit=0" +
            //     " And convert(date,d.CashDetail_Date_date) >= convert(date,'" + From_Date + "') and convert(date,d.CashDetail_Date_date) <= convert(date,'" + To_Date + "')" +
            //     " And d.CashDetail_BillNo_int <> 'On A/c' And d.CashDetail_Settlement_var <> 'On A/c' " +
            // " ),0) " +
            // " - " +
            //" isNULL ((Select sum(d.CashDetail_Amount_money) from tbl_CashDetail as d where a.CL_Id = d.CashDetail_ClientId and d.CashDetail_Status_bit=0" +
            //    " And convert(date,d.CashDetail_Date_date) >= convert(date,'" + From_Date + "') and convert(date,d.CashDetail_Date_date) <= convert(date,'" + To_Date + "')" +
            //    " And (d.CashDetail_BillNo_int = 'On A/c' or (d.CashDetail_Settlement_var = 'On A/c' and CHARINDEX('TRF/', d.CashDetail_NoteNo_var )=0) " +
            //    " or (d.CashDetail_Settlement_var = 'On A/c' and CHARINDEX('TRF/', d.CashDetail_NoteNo_var )>0 ) )" +
            //" ),0) " +

            " )> 0 and (a.CL_Id = " + ClientId + " or " + ClientId + " = 0)" +
            " and (b.SITE_Id = " + SiteId + " or " + SiteId + " = 0)" +
            " order by a.CL_Name_var, b.SITE_Name_var";
                }
                SqlCommand cmd = new SqlCommand(mysql, con);
                //SqlCommand cmd = new SqlCommand("Client_Outstanding", con);
                //cmd.Parameters.Add("@ClientId", SqlDbType.Int, 4).Value = ClientId;
                //cmd.Parameters.Add("@From_Date", SqlDbType.DateTime, 10).Value = From_Date;
                //cmd.Parameters.Add("@To_Date", SqlDbType.DateTime, 10).Value = To_Date;
                //cmd.Parameters.Add("@AsOnStatus", SqlDbType.Bit).Value = AsOnStatus;
                //cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);
                dt = ds.Tables[0];
                ds.Dispose();
                return dt;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public DataTable getClientOutstanding2_forPOUpload(int ClientId, string From_Date, string To_Date, int AsOnStatus)
        {
            SqlConnection con = new SqlConnection(cnStr);
            DataTable dt = new DataTable();
            try
            {
                DataSet ds = new DataSet();

                string mysql = "";
                if (AsOnStatus == 1)
                {
                    mysql = "select BILL_CL_Id,CL_Name_var from " +
            " ( " +
            " (Select convert(varchar,a.BILL_Id) as BILL_Id, a.BILL_CL_Id,a.BILL_NetAmt_num, 0 as CashDetail_Amount_money, " +
            " (select c.CL_Name_var from tbl_Client as c where c.CL_Id = a.BILL_CL_Id) as CL_Name_var " +
            " from tbl_Bill as a " +
            " where a.BILL_Status_bit = 0 and (a.BILL_CL_Id = " + ClientId + " or " + ClientId + " = 0) and a.BILL_CL_Id > 0" +
            " And convert(date,a.BILL_Date_dt) <= convert(date,'" + To_Date + "')  " +
            " AND convert(varchar,a.BILL_Id) NOT IN  " +
            " (SELECT b.CashDetail_BillNo_int from tbl_CashDetail as b where b.CashDetail_Status_bit= 0 " +
            " And  convert(date, b.CashDetail_Date_date) <= convert(date,'" + To_Date + "')  " +
            " ) " +
            " Group by a.BILL_Id, a.BILL_NetAmt_num,a.BILL_CL_Id ) " +

            " Union " +

            " (Select convert(varchar,a.BILL_Id) as BILL_Id, a.BILL_CL_Id,a.BILL_NetAmt_num,sum(b.CashDetail_Amount_money), " +
            " (select c.CL_Name_var from tbl_Client as c where c.CL_Id = a.BILL_CL_Id) as CL_Name_var " +
            " from tbl_Bill as a, tbl_CashDetail as b " +
            " where a.BILL_Status_bit = 0 and (a.BILL_CL_Id = " + ClientId + " or " + ClientId + " = 0)	 and a.BILL_CL_Id > 0" +
            " And convert(date,a.BILL_Date_dt) <= convert(date,'" + To_Date + "')  " +
            " And convert(date, b.CashDetail_Date_date) <= convert(date,'" + To_Date + "')  " +
            " and convert(varchar,a.BILL_Id) = b.CashDetail_BillNo_int  " +
            " and b.CashDetail_Status_bit = 0	 " +
            " Group by a.BILL_Id, a.BILL_CL_Id, a.BILL_NetAmt_num " +
            " Having a.BILL_NetAmt_num + sum(b.CashDetail_Amount_money) > 0) " +

             " ) as q " +
        " group by BILL_CL_Id,CL_Name_var";
                }
                else
                {
                    mysql = "select BILL_CL_Id,CL_Name_var from " +
            " ( " +
            " (Select convert(varchar,a.BILL_Id) as BILL_Id, a.BILL_CL_Id,a.BILL_NetAmt_num, 0 as CashDetail_Amount_money, " +
            " (select c.CL_Name_var from tbl_Client as c where c.CL_Id = a.BILL_CL_Id) as CL_Name_var " +
            " from tbl_Bill as a " +
            " where a.BILL_Status_bit = 0 and (a.BILL_CL_Id = " + ClientId + " or " + ClientId + " = 0) and a.BILL_CL_Id > 0 " +
            " And ((convert(date,a.BILL_Date_dt) >= convert(date,'" + From_Date + "') and convert(date,a.BILL_Date_dt) <= convert(date,'" + To_Date + "')) ) " +
            " AND convert(varchar,a.BILL_Id) NOT IN  " +
            " (SELECT b.CashDetail_BillNo_int from tbl_CashDetail as b where b.CashDetail_Status_bit= 0 " +
            " And ((convert(date,b.CashDetail_Date_date) >= convert(date,'" + From_Date + "') and convert(date,b.CashDetail_Date_date) <= convert(date,'" + To_Date + "')) ) " +
            " ) " +
            " and (a.BILL_POFileName_var = '' or a.BILL_POFileName_var is null) and a.BILL_ClientApproveStatus_bit = 0 " +
            " Group by a.BILL_Id, a.BILL_NetAmt_num,a.BILL_CL_Id ) " +

            " Union " +

            " (Select convert(varchar,a.BILL_Id) as BILL_Id, a.BILL_CL_Id,a.BILL_NetAmt_num,sum(b.CashDetail_Amount_money), " +
            " (select c.CL_Name_var from tbl_Client as c where c.CL_Id = a.BILL_CL_Id) as CL_Name_var " +
            " from tbl_Bill as a, tbl_CashDetail as b " +
            " where a.BILL_Status_bit = 0 and (a.BILL_CL_Id = " + ClientId + " or " + ClientId + " = 0)	and a.BILL_CL_Id > 0 " +
            " And ((convert(date,a.BILL_Date_dt) >= convert(date,'" + From_Date + "') and convert(date,a.BILL_Date_dt) <= convert(date,'" + To_Date + "')) ) " +
            " And ((convert(date,b.CashDetail_Date_date) >= convert(date,'" + From_Date + "') and convert(date,b.CashDetail_Date_date) <= convert(date,'" + To_Date + "')) ) " +
            " and convert(varchar,a.BILL_Id) = b.CashDetail_BillNo_int  " +
            " and b.CashDetail_Status_bit = 0	 " +
            " and (a.BILL_POFileName_var = '' or a.BILL_POFileName_var is null) and a.BILL_ClientApproveStatus_bit = 0 " +
            " Group by a.BILL_Id, a.BILL_CL_Id, a.BILL_NetAmt_num " +
            " Having a.BILL_NetAmt_num + sum(b.CashDetail_Amount_money) > 0) " +

             " ) as q " +
        " group by BILL_CL_Id,CL_Name_var";
                }
                SqlCommand cmd = new SqlCommand(mysql, con);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);
                dt = ds.Tables[0];
                ds.Dispose();
                return dt;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public DataTable getClientOutstanding3_forPOUpload(int ClientId, string From_Date, string To_Date, int AsOnStatus)
        {
            SqlConnection con = new SqlConnection(cnStr);
            DataTable dt = new DataTable();
            try
            {
                DataSet ds = new DataSet();
                string mysql = "";
                if (AsOnStatus == 1)
                {
                    mysql = "Select a.CL_Id, a.CL_Name_var,a.CL_Limit_mny, a.CL_UnderReconciliation_bit, " +
           " isNULL ((Select sum(b.BILL_NetAmt_num) from tbl_Bill as b where a.CL_Id = b.BILL_CL_Id and b.BILL_Status_bit=0" +
               " And convert(date,b.BILL_Date_dt) <= convert(date,'" + To_Date + "') " +

           " ),0) as billAmount," +
           " isNULL ((Select sum(c.Journal_Amount_dec) from tbl_Journal as c where a.CL_Id = c.Journal_ClientId_int and c.Journal_Status_bit=0" +
               " And convert(date,c.Journal_Date_dt) <= convert(date,'" + To_Date + "') " +
               " And CHARINDEX('DB/', c.Journal_NoteNo_var,1) > 0" +
           " ),0) as journalDBAmount," +
           " isNULL ((Select sum(c.Journal_Amount_dec) from tbl_Journal as c where a.CL_Id = c.Journal_ClientId_int and c.Journal_Status_bit=0" +
               " And convert(date,c.Journal_Date_dt) <= convert(date,'" + To_Date + "') " +
               " And CHARINDEX('CR/', c.Journal_NoteNo_var,1) > 0" +
           " ),0) as journalCRAmount," +
           " isNULL ((Select sum(d.CashDetail_Amount_money) from tbl_CashDetail as d where a.CL_Id = d.CashDetail_ClientId and d.CashDetail_Status_bit=0" +
               " And convert(date,d.CashDetail_Date_date) <= convert(date,'" + To_Date + "') " +
           " ),0) as receivedAmount," +

           " isNULL ((Select sum(d.CashDetail_Amount_money) from tbl_CashDetail as d where a.CL_Id = d.CashDetail_ClientId and d.CashDetail_Status_bit=0" +
               " And convert(date,d.CashDetail_Date_date) <= convert(date,'" + To_Date + "') " +
               " And (d.CashDetail_BillNo_int = 'On A/c' or (d.CashDetail_Settlement_var = 'On A/c' and CHARINDEX('TRF/', d.CashDetail_NoteNo_var )=0) " +
               " or (d.CashDetail_Settlement_var = 'On A/c' and CHARINDEX('TRF/', d.CashDetail_NoteNo_var )>0 and (d.CashDetail_BillNo_int is null or d.CashDetail_BillNo_int = '')) )" +
           " ),0) as OnAccAmount" +


           " from tbl_Client as a" +
           " where " +
           " (" +
           " isNULL ((Select sum(b.BILL_NetAmt_num) from tbl_Bill as b where a.CL_Id = b.BILL_CL_Id and b.BILL_Status_bit=0" +
               " And convert(date,b.BILL_Date_dt) <= convert(date,'" + To_Date + "') " +
           " ),0) + " +
           " isNULL ((Select sum(c.Journal_Amount_dec) from tbl_Journal as c where a.CL_Id = c.Journal_ClientId_int and c.Journal_Status_bit=0" +
               " And convert(date,c.Journal_Date_dt) <= convert(date,'" + To_Date + "') " +
               " And CHARINDEX('DB/', c.Journal_NoteNo_var,1) > 0" +
           " ),0) 	" +
           " +" +

           " isNULL ((Select sum(d.CashDetail_Amount_money) from tbl_CashDetail as d where a.CL_Id = d.CashDetail_ClientId and d.CashDetail_Status_bit=0" +
               " And convert(date,d.CashDetail_Date_date) <= convert(date,'" + To_Date + "') " +
               " And d.CashDetail_BillNo_int <> 'On A/c' And d.CashDetail_Settlement_var <> 'On A/c' " +
           " ),0) " +
           " - " +
           " isNULL ((Select sum(d.CashDetail_Amount_money) from tbl_CashDetail as d where a.CL_Id = d.CashDetail_ClientId and d.CashDetail_Status_bit=0" +
               " And convert(date,d.CashDetail_Date_date) <= convert(date,'" + To_Date + "') " +
               " And (d.CashDetail_BillNo_int = 'On A/c' or (d.CashDetail_Settlement_var = 'On A/c' and CHARINDEX('TRF/', d.CashDetail_NoteNo_var )=0) " +
               " or (d.CashDetail_Settlement_var = 'On A/c' and CHARINDEX('TRF/', d.CashDetail_NoteNo_var )>0 ) )" +
           " ),0) " +
           " )> 0 and (a.CL_Id = " + ClientId + " or " + ClientId + " = 0)" +
           " order by a.CL_Name_var";
                }
                else
                {
                    mysql = "Select a.CL_Id, a.CL_Name_var,a.CL_Limit_mny, a.CL_UnderReconciliation_bit, " +
            " isNULL ((Select sum(b.BILL_NetAmt_num) from tbl_Bill as b where a.CL_Id = b.BILL_CL_Id and b.BILL_Status_bit=0" +
                " And convert(date,b.BILL_Date_dt) >= convert(date,'" + From_Date + "') and convert(date,b.BILL_Date_dt) <= convert(date,'" + To_Date + "')" +
            " ),0) as billAmount," +
            " isNULL ((Select sum(c.Journal_Amount_dec) from tbl_Journal as c where a.CL_Id = c.Journal_ClientId_int and c.Journal_Status_bit=0" +
                " And convert(date,c.Journal_Date_dt) >= convert(date,'" + From_Date + "') and convert(date,c.Journal_Date_dt) <= convert(date,'" + To_Date + "')" +
                " And CHARINDEX('DB/', c.Journal_NoteNo_var,1) > 0" +
            " ),0) as journalDBAmount," +
            " isNULL ((Select sum(c.Journal_Amount_dec) from tbl_Journal as c where a.CL_Id = c.Journal_ClientId_int and c.Journal_Status_bit=0" +
                " And convert(date,c.Journal_Date_dt) >= convert(date,'" + From_Date + "') and convert(date,c.Journal_Date_dt) <= convert(date,'" + To_Date + "')" +
                " And CHARINDEX('CR/', c.Journal_NoteNo_var,1) > 0" +
            " ),0) as journalCRAmount," +
            " isNULL ((Select sum(d.CashDetail_Amount_money) from tbl_CashDetail as d where a.CL_Id = d.CashDetail_ClientId and d.CashDetail_Status_bit=0" +
                " And convert(date,d.CashDetail_Date_date) >= convert(date,'" + From_Date + "') and convert(date,d.CashDetail_Date_date) <= convert(date,'" + To_Date + "')" +
            " ),0) as receivedAmount," +

            " isNULL ((Select sum(d.CashDetail_Amount_money) from tbl_CashDetail as d where a.CL_Id = d.CashDetail_ClientId and d.CashDetail_Status_bit=0" +
               " And convert(date,d.CashDetail_Date_date) >= convert(date,'" + From_Date + "') and convert(date,d.CashDetail_Date_date) <= convert(date,'" + To_Date + "')" +
               " And (d.CashDetail_BillNo_int = 'On A/c' or (d.CashDetail_Settlement_var = 'On A/c' and CHARINDEX('TRF/', d.CashDetail_NoteNo_var )=0) " +
               " or (d.CashDetail_Settlement_var = 'On A/c' and CHARINDEX('TRF/', d.CashDetail_NoteNo_var )>0 and (d.CashDetail_BillNo_int is null or d.CashDetail_BillNo_int = '')) )" +
           " ),0) as OnAccAmount" +

            " from tbl_Client as a" +
            " where " +
            " (" +
            " isNULL ((Select sum(b.BILL_NetAmt_num) from tbl_Bill as b where a.CL_Id = b.BILL_CL_Id and b.BILL_Status_bit=0" +
                " And convert(date,b.BILL_Date_dt) >= convert(date,'" + From_Date + "') and convert(date,b.BILL_Date_dt) <= convert(date,'" + To_Date + "')" +
            " ),0) + " +
            " isNULL ((Select sum(c.Journal_Amount_dec) from tbl_Journal as c where a.CL_Id = c.Journal_ClientId_int and c.Journal_Status_bit=0" +
                " And convert(date,c.Journal_Date_dt) >= convert(date,'" + From_Date + "') and convert(date,c.Journal_Date_dt) <= convert(date,'" + To_Date + "')" +
                " And CHARINDEX('DB/', c.Journal_NoteNo_var,1) > 0" +
            " ),0) 	" +
            " +" +
            " isNULL ((Select sum(d.CashDetail_Amount_money) from tbl_CashDetail as d where a.CL_Id = d.CashDetail_ClientId and d.CashDetail_Status_bit=0" +
                " And convert(date,d.CashDetail_Date_date) >= convert(date,'" + From_Date + "') and convert(date,d.CashDetail_Date_date) <= convert(date,'" + To_Date + "')" +
                " And d.CashDetail_BillNo_int <> 'On A/c' And d.CashDetail_Settlement_var <> 'On A/c' " +
            " ),0) " +
            " - " +
           " isNULL ((Select sum(d.CashDetail_Amount_money) from tbl_CashDetail as d where a.CL_Id = d.CashDetail_ClientId and d.CashDetail_Status_bit=0" +
               " And convert(date,d.CashDetail_Date_date) >= convert(date,'" + From_Date + "') and convert(date,d.CashDetail_Date_date) <= convert(date,'" + To_Date + "')" +
               " And (d.CashDetail_BillNo_int = 'On A/c' or (d.CashDetail_Settlement_var = 'On A/c' and CHARINDEX('TRF/', d.CashDetail_NoteNo_var )=0) " +
               " or (d.CashDetail_Settlement_var = 'On A/c' and CHARINDEX('TRF/', d.CashDetail_NoteNo_var )>0 ) )" +
           " ),0) " +
            " )> 0 and (a.CL_Id = " + ClientId + " or " + ClientId + " = 0)" +
            " and (b.BILL_POFileName_var = '' or b.BILL_POFileName_var is null) and b.BILL_ClientApproveStatus_bit = 0 " +
            " order by a.CL_Name_var";
                }
                SqlCommand cmd = new SqlCommand(mysql, con);
                //SqlCommand cmd = new SqlCommand("Client_Outstanding", con);
                //cmd.Parameters.Add("@ClientId", SqlDbType.Int, 4).Value = ClientId;
                //cmd.Parameters.Add("@From_Date", SqlDbType.DateTime, 10).Value = From_Date;
                //cmd.Parameters.Add("@To_Date", SqlDbType.DateTime, 10).Value = To_Date;
                //cmd.Parameters.Add("@AsOnStatus", SqlDbType.Bit).Value = AsOnStatus;
                //cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);
                dt = ds.Tables[0];
                ds.Dispose();
                return dt;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public DataTable getClientForBusinessReport(string From_Date, string To_Date, string From_DateQ1, string To_DateQ1, string From_DateQ2, string To_DateQ2, string From_DateQ3, string To_DateQ3, string From_DateQ4, string To_DateQ4)
        {
            SqlConnection con = new SqlConnection(cnStr);
            DataTable dt = new DataTable();
            try
            {
                DataSet ds = new DataSet();

                string mysql = "";

                mysql = "select a.BILL_CL_Id, d.CL_Name_var, " +

                        " isNULL ((select SUM(b.BILL_NetAmt_num) - SUM(b.BILL_SerTaxAmt_num) " +
                        " from tbl_Bill as b where b.BILL_CL_Id = a.BILL_CL_Id" +
                        " and b.BILL_Status_bit = 0 " +
                        " And convert(date,b.BILL_Date_dt) >= convert(date,'" + From_DateQ1 + "')  " +
                        " And convert(date,b.BILL_Date_dt) <= convert(date,'" + To_DateQ1 + "')  " +
                        " ),0) as Q1BusiAmt," +

                        " isNULL ((select SUM(b.BILL_NetAmt_num) - SUM(b.BILL_SerTaxAmt_num) " +
                        " from tbl_Bill as b where b.BILL_CL_Id = a.BILL_CL_Id" +
                        " and b.BILL_Status_bit = 0 " +
                        " And convert(date,b.BILL_Date_dt) >= convert(date,'" + From_DateQ2 + "')  " +
                        " And convert(date,b.BILL_Date_dt) <= convert(date,'" + To_DateQ2 + "')  " +
                        " ),0) as Q2BusiAmt," +

                        " isNULL ((select SUM(b.BILL_NetAmt_num) - SUM(b.BILL_SerTaxAmt_num) " +
                        " from tbl_Bill as b where b.BILL_CL_Id = a.BILL_CL_Id" +
                        " and b.BILL_Status_bit = 0 " +
                        " And convert(date,b.BILL_Date_dt) >= convert(date,'" + From_DateQ3 + "')  " +
                        " And convert(date,b.BILL_Date_dt) <= convert(date,'" + To_DateQ3 + "')  " +
                        " ),0) as Q3BusiAmt," +

                        " isNULL ((select SUM(b.BILL_NetAmt_num) - SUM(b.BILL_SerTaxAmt_num) " +
                        " from tbl_Bill as b where b.BILL_CL_Id = a.BILL_CL_Id " +
                        " and b.BILL_Status_bit = 0 " +
                        " And convert(date,b.BILL_Date_dt) >= convert(date,'" + From_DateQ4 + "')  " +
                        " And convert(date,b.BILL_Date_dt) <= convert(date,'" + To_DateQ4 + "')  " +
                        " ),0) as Q4BusiAmt," +

                        " isNULL ((select SUM(b.BILL_NetAmt_num) " +
                        " from tbl_Bill as b where b.BILL_CL_Id = a.BILL_CL_Id" +
                        " and b.BILL_Status_bit = 0 " +
                        " And convert(date,b.BILL_Date_dt) >= convert(date,'" + From_DateQ1 + "')  " +
                        " And convert(date,b.BILL_Date_dt) <= convert(date,'" + To_DateQ1 + "')  " +
                        " ),0) as Q1BusiAmtWithTax," +

                        " isNULL ((select SUM(b.BILL_NetAmt_num) " +
                        " from tbl_Bill as b where b.BILL_CL_Id = a.BILL_CL_Id" +
                        " and b.BILL_Status_bit = 0 " +
                        " And convert(date,b.BILL_Date_dt) >= convert(date,'" + From_DateQ2 + "')  " +
                        " And convert(date,b.BILL_Date_dt) <= convert(date,'" + To_DateQ2 + "')  " +
                        " ),0) as Q2BusiAmtWithTax," +

                        " isNULL ((select SUM(b.BILL_NetAmt_num) " +
                        " from tbl_Bill as b where b.BILL_CL_Id = a.BILL_CL_Id" +
                        " and b.BILL_Status_bit = 0 " +
                        " And convert(date,b.BILL_Date_dt) >= convert(date,'" + From_DateQ3 + "')  " +
                        " And convert(date,b.BILL_Date_dt) <= convert(date,'" + To_DateQ3 + "')  " +
                        " ),0) as Q3BusiAmtWithTax," +

                        " isNULL ((select SUM(b.BILL_NetAmt_num) " +
                        " from tbl_Bill as b where b.BILL_CL_Id = a.BILL_CL_Id " +
                        " and b.BILL_Status_bit = 0 " +
                        " And convert(date,b.BILL_Date_dt) >= convert(date,'" + From_DateQ4 + "')  " +
                        " And convert(date,b.BILL_Date_dt) <= convert(date,'" + To_DateQ4 + "')  " +
                        " ),0) as Q4BusiAmtWithTax," +

                        " (ISNULL((Select SUM(c.CashDetail_Amount_money)  " +
                        " from tbl_CashDetail as c where c.CashDetail_ClientId = a.BILL_CL_Id  " +
                        " and c.CashDetail_Status_bit = 0	and convert(date,c.CashDetail_Date_date) < convert(date,'" + From_DateQ1 + "')  " + "),0) +  " +
                        " ISNULL((Select SUM(c.BILL_NetAmt_num )  " +
                        " from tbl_Bill as c where c.BILL_CL_Id =  a.BILL_CL_Id  " +
                        " and c.BILL_Status_bit = 0 and convert(date,c.BILL_Date_dt) < convert(date,'" + From_DateQ1 + "')  " + "),0) +   " +
                        " ISNULL((Select SUM(c.Journal_Amount_dec)  " +
                        " from tbl_Journal as c where c.Journal_ClientId_int =  a.BILL_CL_Id  " +
                        " and c.Journal_Status_bit = 0 and convert(date,c.Journal_Date_dt) < convert(date,'" + From_DateQ1 + "')  " + " and CHARINDEX('DB/', c.Journal_NoteNo_var,1) > 0),0)) as openingQ1,  " +

                        " (ISNULL((Select SUM(c.CashDetail_Amount_money)  " +
                        " from tbl_CashDetail as c where c.CashDetail_ClientId =  a.BILL_CL_Id  " +
                        " and c.CashDetail_Status_bit = 0	and convert(date,c.CashDetail_Date_date) <= convert(date,'" + To_DateQ1 + "')  " + "),0) +  " +
                        " ISNULL((Select SUM(c.BILL_NetAmt_num )  " +
                        " from tbl_Bill as c where c.BILL_CL_Id = a.BILL_CL_Id  " +
                        " and c.BILL_Status_bit = 0 and convert(date,c.BILL_Date_dt) <= convert(date,'" + To_DateQ1 + "')  " + "),0) +   " +
                        " ISNULL((Select SUM(c.Journal_Amount_dec)  " +
                        " from tbl_Journal as c where c.Journal_ClientId_int =  a.BILL_CL_Id  " +
                        " and c.Journal_Status_bit = 0 and convert(date,c.Journal_Date_dt) <= convert(date,'" + To_DateQ1 + "')  " + " and CHARINDEX('DB/', c.Journal_NoteNo_var,1) > 0),0)) as closingQ1,  " +

                        " (ISNULL((Select SUM(c.CashDetail_Amount_money)  " +
                        " from tbl_CashDetail as c where c.CashDetail_ClientId = a.BILL_CL_Id  " +
                        " and c.CashDetail_Status_bit = 0	and convert(date,c.CashDetail_Date_date) < convert(date,'" + From_DateQ2 + "')  " + "),0) +  " +
                        " ISNULL((Select SUM(c.BILL_NetAmt_num )  " +
                        " from tbl_Bill as c where c.BILL_CL_Id =  a.BILL_CL_Id  " +
                        " and c.BILL_Status_bit = 0 and convert(date,c.BILL_Date_dt) < convert(date,'" + From_DateQ2 + "')  " + "),0) +   " +
                        " ISNULL((Select SUM(c.Journal_Amount_dec)  " +
                        " from tbl_Journal as c where c.Journal_ClientId_int =  a.BILL_CL_Id  " +
                        " and c.Journal_Status_bit = 0 and convert(date,c.Journal_Date_dt) < convert(date,'" + From_DateQ2 + "')  " + " and CHARINDEX('DB/', c.Journal_NoteNo_var,1) > 0),0)) as openingQ2,  " +

                        " (ISNULL((Select SUM(c.CashDetail_Amount_money)  " +
                        " from tbl_CashDetail as c where c.CashDetail_ClientId =  a.BILL_CL_Id  " +
                        " and c.CashDetail_Status_bit = 0	and convert(date,c.CashDetail_Date_date) <= convert(date,'" + To_DateQ2 + "')  " + "),0) +  " +
                        " ISNULL((Select SUM(c.BILL_NetAmt_num )  " +
                        " from tbl_Bill as c where c.BILL_CL_Id = a.BILL_CL_Id  " +
                        " and c.BILL_Status_bit = 0 and convert(date,c.BILL_Date_dt) <= convert(date,'" + To_DateQ2 + "')  " + "),0) +   " +
                        " ISNULL((Select SUM(c.Journal_Amount_dec)  " +
                        " from tbl_Journal as c where c.Journal_ClientId_int =  a.BILL_CL_Id  " +
                        " and c.Journal_Status_bit = 0 and convert(date,c.Journal_Date_dt) <= convert(date,'" + To_DateQ2 + "')  " + " and CHARINDEX('DB/', c.Journal_NoteNo_var,1) > 0),0)) as closingQ2,  " +

                        " (ISNULL((Select SUM(c.CashDetail_Amount_money)  " +
                        " from tbl_CashDetail as c where c.CashDetail_ClientId = a.BILL_CL_Id  " +
                        " and c.CashDetail_Status_bit = 0	and convert(date,c.CashDetail_Date_date) < convert(date,'" + From_DateQ3 + "')  " + "),0) +  " +
                        " ISNULL((Select SUM(c.BILL_NetAmt_num )  " +
                        " from tbl_Bill as c where c.BILL_CL_Id =  a.BILL_CL_Id  " +
                        " and c.BILL_Status_bit = 0 and convert(date,c.BILL_Date_dt) < convert(date,'" + From_DateQ3 + "')  " + "),0) +   " +
                        " ISNULL((Select SUM(c.Journal_Amount_dec)  " +
                        " from tbl_Journal as c where c.Journal_ClientId_int =  a.BILL_CL_Id  " +
                        " and c.Journal_Status_bit = 0 and convert(date,c.Journal_Date_dt) < convert(date,'" + From_DateQ3 + "')  " + " and CHARINDEX('DB/', c.Journal_NoteNo_var,1) > 0),0)) as openingQ3,  " +

                        " (ISNULL((Select SUM(c.CashDetail_Amount_money)  " +
                        " from tbl_CashDetail as c where c.CashDetail_ClientId =  a.BILL_CL_Id  " +
                        " and c.CashDetail_Status_bit = 0	and convert(date,c.CashDetail_Date_date) <= convert(date,'" + To_DateQ3 + "')  " + "),0) +  " +
                        " ISNULL((Select SUM(c.BILL_NetAmt_num )  " +
                        " from tbl_Bill as c where c.BILL_CL_Id = a.BILL_CL_Id  " +
                        " and c.BILL_Status_bit = 0 and convert(date,c.BILL_Date_dt) <= convert(date,'" + To_DateQ3 + "')  " + "),0) +   " +
                        " ISNULL((Select SUM(c.Journal_Amount_dec)  " +
                        " from tbl_Journal as c where c.Journal_ClientId_int =  a.BILL_CL_Id  " +
                        " and c.Journal_Status_bit = 0 and convert(date,c.Journal_Date_dt) <= convert(date,'" + To_DateQ3 + "')  " + " and CHARINDEX('DB/', c.Journal_NoteNo_var,1) > 0),0)) as closingQ3,  " +

                        " (ISNULL((Select SUM(c.CashDetail_Amount_money)  " +
                        " from tbl_CashDetail as c where c.CashDetail_ClientId = a.BILL_CL_Id  " +
                        " and c.CashDetail_Status_bit = 0	and convert(date,c.CashDetail_Date_date) < convert(date,'" + From_DateQ4 + "')  " + "),0) +  " +
                        " ISNULL((Select SUM(c.BILL_NetAmt_num )  " +
                        " from tbl_Bill as c where c.BILL_CL_Id =  a.BILL_CL_Id  " +
                        " and c.BILL_Status_bit = 0 and convert(date,c.BILL_Date_dt) < convert(date,'" + From_DateQ4 + "')  " + "),0) +   " +
                        " ISNULL((Select SUM(c.Journal_Amount_dec)  " +
                        " from tbl_Journal as c where c.Journal_ClientId_int =  a.BILL_CL_Id  " +
                        " and c.Journal_Status_bit = 0 and convert(date,c.Journal_Date_dt) < convert(date,'" + From_DateQ4 + "')  " + " and CHARINDEX('DB/', c.Journal_NoteNo_var,1) > 0),0)) as openingQ4,  " +

                        " (ISNULL((Select SUM(c.CashDetail_Amount_money)  " +
                        " from tbl_CashDetail as c where c.CashDetail_ClientId =  a.BILL_CL_Id  " +
                        " and c.CashDetail_Status_bit = 0	and convert(date,c.CashDetail_Date_date) <= convert(date,'" + To_DateQ4 + "')  " + "),0) +  " +
                        " ISNULL((Select SUM(c.BILL_NetAmt_num )  " +
                        " from tbl_Bill as c where c.BILL_CL_Id = a.BILL_CL_Id  " +
                        " and c.BILL_Status_bit = 0 and convert(date,c.BILL_Date_dt) <= convert(date,'" + To_DateQ4 + "')  " + "),0) +   " +
                        " ISNULL((Select SUM(c.Journal_Amount_dec)  " +
                        " from tbl_Journal as c where c.Journal_ClientId_int =  a.BILL_CL_Id  " +
                        " and c.Journal_Status_bit = 0 and convert(date,c.Journal_Date_dt) <= convert(date,'" + To_DateQ4 + "')  " + " and CHARINDEX('DB/', c.Journal_NoteNo_var,1) > 0),0)) as closingQ4  " +

                    " from tbl_Bill as a, tbl_Client as d " +
                    " Where a.BILL_CL_Id = d.CL_Id " +
                    " And convert(date,a.BILL_Date_dt) >= convert(date,'" + From_Date + "')  " +
                    " And convert(date,a.BILL_Date_dt) <= convert(date,'" + To_Date + "')  " +
                    " group by a.BILL_CL_Id, d.CL_Name_var" +
                    " Order by d.CL_Name_var, a.BILL_CL_Id";

                SqlCommand cmd = new SqlCommand(mysql, con);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);
                dt = ds.Tables[0];
                ds.Dispose();
                return dt;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public DataTable getClientSiteOutstanding(int ClientId, string From_Date, string To_Date, int AsOnStatus)
        {
            SqlConnection con = new SqlConnection(cnStr);
            DataTable dt = new DataTable();
            try
            {
                DataSet ds = new DataSet();

                string mysql = "";
                if (AsOnStatus == 1)
                {
                    mysql = "select BILL_CL_Id,CL_Name_var,BILL_SITE_Id,SITE_Name_var from " +
            " ( " +
            " (Select convert(varchar,a.BILL_Id) as BILL_Id, a.BILL_CL_Id,a.BILL_NetAmt_num, 0 as CashDetail_Amount_money, a.BILL_SITE_Id, " +
            " (select c.CL_Name_var from tbl_Client as c where c.CL_Id = a.BILL_CL_Id) as CL_Name_var, " +
            " (select c.SITE_Name_var from tbl_Site as c where c.SITE_Id = a.BILL_SITE_Id) as SITE_Name_var " +
            " from tbl_Bill as a " +
            " where a.BILL_Status_bit = 0 and (a.BILL_CL_Id = " + ClientId + " or " + ClientId + " = 0) and a.BILL_CL_Id > 0" +
            " And convert(date,a.BILL_Date_dt) <= convert(date,'" + To_Date + "')  " +
            " AND convert(varchar,a.BILL_Id) NOT IN  " +
            " (SELECT b.CashDetail_BillNo_int from tbl_CashDetail as b where b.CashDetail_Status_bit= 0 " +
            " And  convert(date, b.CashDetail_Date_date) <= convert(date,'" + To_Date + "')  " +
            " ) " +
            " Group by a.BILL_Id, a.BILL_NetAmt_num,a.BILL_CL_Id, a.BILL_SITE_Id ) " +

            " Union " +

            " (Select convert(varchar,a.BILL_Id) as BILL_Id, a.BILL_CL_Id,a.BILL_NetAmt_num,sum(b.CashDetail_Amount_money), a.BILL_SITE_Id, " +
            " (select c.CL_Name_var from tbl_Client as c where c.CL_Id = a.BILL_CL_Id) as CL_Name_var, " +
            " (select c.SITE_Name_var from tbl_Site as c where c.SITE_Id = a.BILL_SITE_Id) as SITE_Name_var " +
            " from tbl_Bill as a, tbl_CashDetail as b " +
            " where a.BILL_Status_bit = 0 and (a.BILL_CL_Id = " + ClientId + " or " + ClientId + " = 0)	 and a.BILL_CL_Id > 0" +
            " And convert(date,a.BILL_Date_dt) <= convert(date,'" + To_Date + "')  " +
            " And convert(date, b.CashDetail_Date_date) <= convert(date,'" + To_Date + "')  " +
            " and convert(varchar,a.BILL_Id) = b.CashDetail_BillNo_int  " +
            " and b.CashDetail_Status_bit = 0	 " +
            " Group by a.BILL_Id, a.BILL_CL_Id, a.BILL_NetAmt_num, a.BILL_SITE_Id " +
            " Having a.BILL_NetAmt_num + sum(b.CashDetail_Amount_money) > 0) " +

            " Union " +

            " (Select a.Journal_NoteNo_var as BILL_Id,a.Journal_ClientId_int as BILL_CL_Id, a.Journal_Amount_dec 	 as BILL_NetAmt_num, 	0 as CashDetail_Amount_money, a.Journal_SiteId_int	as BILL_SITE_Id, " +
            " (select c.CL_Name_var from tbl_Client as c where c.CL_Id = a.Journal_ClientId_int) as CL_Name_var, " +
            " (select c.SITE_Name_var from tbl_Site as c where c.SITE_Id = a.Journal_SiteId_int) as SITE_Name_var " +
            " from tbl_Journal as a " +
            " where a.Journal_Status_bit = 0 and (a.Journal_ClientId_int = " + ClientId + " or " + ClientId + " = 0) and a.Journal_ClientId_int > 0" +
            " And convert(date,a.Journal_Date_dt) <= convert(date,'" + To_Date + "') " +
            " And CHARINDEX('DB/',a.Journal_NoteNo_var) > 0 " +
            " AND a.Journal_NoteNo_var NOT IN  " +
            " (SELECT b.CashDetail_BillNo_int from tbl_CashDetail as b where b.CashDetail_Status_bit= 0) " +
            " Group by a.Journal_NoteNo_var, a.Journal_Amount_dec,a.Journal_ClientId_int, a.Journal_SiteId_int) " +

            " Union " +

            " (Select a.Journal_NoteNo_var as BILL_Id, a.Journal_ClientId_int as BILL_CL_Id,	 a.Journal_Amount_dec 	 as BILL_NetAmt_num, 	0 as CashDetail_Amount_money, a.Journal_SiteId_int as BILL_SITE_Id, " +
            " (select c.CL_Name_var from tbl_Client as c where c.CL_Id = a.Journal_ClientId_int) as CL_Name_var, " +
            " (select c.SITE_Name_var from tbl_Site as c where c.SITE_Id = a.Journal_SiteId_int) as SITE_Name_var " +
            " from tbl_Journal as a, tbl_CashDetail as b " +
            " where a.Journal_Status_bit = 0 and (a.Journal_ClientId_int = " + ClientId + " or " + ClientId + " = 0)	and a.Journal_ClientId_int > 0 " +
            " And convert(date,a.Journal_Date_dt) <= convert(date,'" + To_Date + "')  " +
            " And convert(date, b.CashDetail_Date_date) <= convert(date,'" + To_Date + "')  " +
            " and a.Journal_NoteNo_var = b.CashDetail_BillNo_int  " +
            " and b.CashDetail_Status_bit = 0	 " +
            " And CHARINDEX('DB/',a.Journal_NoteNo_var) > 0 " +
            " Group by a.Journal_NoteNo_var, a.Journal_ClientId_int , a.Journal_Amount_dec, a.Journal_SiteId_int " +
            " Having a.Journal_Amount_dec + sum(b.CashDetail_Amount_money) > 0) " +
             " ) as q " +
        " group by BILL_CL_Id,CL_Name_var";
                }
                else
                {
                    mysql = "select BILL_CL_Id,CL_Name_var from " +
            " ( " +
            " (Select convert(varchar,a.BILL_Id) as BILL_Id, a.BILL_CL_Id,a.BILL_NetAmt_num, 0 as CashDetail_Amount_money, " +
            " (select c.CL_Name_var from tbl_Client as c where c.CL_Id = a.BILL_CL_Id) as CL_Name_var " +
            " from tbl_Bill as a " +
            " where a.BILL_Status_bit = 0 and (a.BILL_CL_Id = " + ClientId + " or " + ClientId + " = 0) and a.BILL_CL_Id > 0 " +
            " And ((convert(date,a.BILL_Date_dt) >= convert(date,'" + From_Date + "') and convert(date,a.BILL_Date_dt) <= convert(date,'" + To_Date + "')) ) " +
            " AND convert(varchar,a.BILL_Id) NOT IN  " +
            " (SELECT b.CashDetail_BillNo_int from tbl_CashDetail as b where b.CashDetail_Status_bit= 0 " +
            " And ((convert(date,b.CashDetail_Date_date) >= convert(date,'" + From_Date + "') and convert(date,b.CashDetail_Date_date) <= convert(date,'" + To_Date + "')) ) " +
            " ) " +
            " Group by a.BILL_Id, a.BILL_NetAmt_num,a.BILL_CL_Id ) " +

            " Union " +

            " (Select convert(varchar,a.BILL_Id) as BILL_Id, a.BILL_CL_Id,a.BILL_NetAmt_num,sum(b.CashDetail_Amount_money), " +
            " (select c.CL_Name_var from tbl_Client as c where c.CL_Id = a.BILL_CL_Id) as CL_Name_var " +
            " from tbl_Bill as a, tbl_CashDetail as b " +
            " where a.BILL_Status_bit = 0 and (a.BILL_CL_Id = " + ClientId + " or " + ClientId + " = 0)	and a.BILL_CL_Id > 0 " +
            " And ((convert(date,a.BILL_Date_dt) >= convert(date,'" + From_Date + "') and convert(date,a.BILL_Date_dt) <= convert(date,'" + To_Date + "')) ) " +
            " And ((convert(date,b.CashDetail_Date_date) >= convert(date,'" + From_Date + "') and convert(date,b.CashDetail_Date_date) <= convert(date,'" + To_Date + "')) ) " +
            " and convert(varchar,a.BILL_Id) = b.CashDetail_BillNo_int  " +
            " and b.CashDetail_Status_bit = 0	 " +
            " Group by a.BILL_Id, a.BILL_CL_Id, a.BILL_NetAmt_num " +
            " Having a.BILL_NetAmt_num + sum(b.CashDetail_Amount_money) > 0) " +

            " Union " +

            " (Select a.Journal_NoteNo_var as BILL_Id,a.Journal_ClientId_int as BILL_CL_Id, a.Journal_Amount_dec 	 as BILL_NetAmt_num, 	0 as CashDetail_Amount_money,	 " +
            " (select c.CL_Name_var from tbl_Client as c where c.CL_Id = a.Journal_ClientId_int) as CL_Name_var " +
            " from tbl_Journal as a " +
            " where a.Journal_Status_bit = 0 and (a.Journal_ClientId_int = " + ClientId + " or " + ClientId + " = 0) and a.Journal_ClientId_int > 0" +
            " And ((convert(date,a.Journal_Date_dt) >= convert(date,'" + From_Date + "') and convert(date,a.Journal_Date_dt) <= convert(date,'" + To_Date + "')) ) " +
            " And CHARINDEX('DB/',a.Journal_NoteNo_var) > 0 " +
            " AND a.Journal_NoteNo_var NOT IN  " +
            " (SELECT b.CashDetail_BillNo_int from tbl_CashDetail as b where b.CashDetail_Status_bit='false') " +
            " Group by a.Journal_NoteNo_var, a.Journal_Amount_dec,a.Journal_ClientId_int) " +

            " Union " +

            " (Select a.Journal_NoteNo_var as BILL_Id, a.Journal_ClientId_int as BILL_CL_Id,	 a.Journal_Amount_dec 	 as BILL_NetAmt_num, 	0 as CashDetail_Amount_money,	 " +
            " (select c.CL_Name_var from tbl_Client as c where c.CL_Id = a.Journal_ClientId_int) as CL_Name_var " +
            " from tbl_Journal as a, tbl_CashDetail as b " +
            " where a.Journal_Status_bit = 0 and (a.Journal_ClientId_int = " + ClientId + " or " + ClientId + " = 0) and a.Journal_ClientId_int > 0	 " +
            " And ((convert(date,a.Journal_Date_dt) >= convert(date,'" + From_Date + "') and convert(date,a.Journal_Date_dt) <= convert(date,'" + To_Date + "')) ) " +
            " And ((convert(date,b.CashDetail_Date_date) >= convert(date,'" + From_Date + "') and convert(date,b.CashDetail_Date_date) <= convert(date,'" + To_Date + "')) ) " +
            " and a.Journal_NoteNo_var = b.CashDetail_BillNo_int  " +
            " and b.CashDetail_Status_bit = 0	 " +
            " And CHARINDEX('DB/',a.Journal_NoteNo_var) > 0 " +
            " Group by a.Journal_NoteNo_var, a.Journal_ClientId_int , a.Journal_Amount_dec " +
            " Having a.Journal_Amount_dec + sum(b.CashDetail_Amount_money) > 0) " +
             " ) as q " +
        " group by BILL_CL_Id,CL_Name_var";
                }
                SqlCommand cmd = new SqlCommand(mysql, con);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);
                dt = ds.Tables[0];
                ds.Dispose();
                return dt;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public void updateGeneralData(string mQueryString)
        {
            SqlConnection con = new SqlConnection(cnStr);

        }

        public decimal myRoundOffFn(decimal mValue)
        {
            ////decimal retValue
            //string[] strAmt = Convert.ToString(mValue).Split('.');
            //if (strAmt.Length > 1)
            //{
            //    if (Convert.ToInt32(strAmt[1].ToString().Substring(0, 1)) >= 5)
            //        return Convert.ToDecimal(Convert.ToDouble(strAmt[0]) + 1);
            //    else
            //        return Convert.ToDecimal(strAmt[0]);
            //}
            //else
            //{
            return mValue;

            //}
        }

        public int Inward_Insert_old(int EnquiryNo, int ProposalId, string RecordType, int ClientId, int SiteId, int ContactPersonId, string Building, string Charges, string WorkOrderNo, byte Status, int ReceivedBy, DateTime CollectionDate, TimeSpan CollectionTime, int TotalQty, int Subsets, string ContactNo, string EmailId, DateTime ReceivedDate)
        {
            //SqlConnection conn = new SqlConnection(cnStr);
            ////conn.Open();
            //SqlDataAdapter da = new SqlDataAdapter();
            //DataTable dt = new DataTable();
            //string mySql = "";
            //int RecordNo = 0;
            //dt = getGeneralData("select MAX(INWD_RecordNo_int) as CurrentNo from tbl_Inward where INWD_RecordType_var= 	" + RecordType );
            //if (dt.Rows.Count > 0)
            //{
            //    RecordNo = Convert.ToInt32(dt.Rows[0]["CurrentNo"].ToString());
            //}
            //RecordNo = RecordNo + 1;

            //mySql = "insert into tbl_Inward (INWD_ENQ_Id, INWD_PROP_Id, INWD_RecordNo_int, INWD_RecordType_var,	INWD_CL_Id,	INWD_SITE_Id, " +
            //    "INWD_CONT_Id, INWD_Building_var, INWD_Charges_var, INWD_WorkOrderNo_var, INWD_Status_tint, INWD_ReceivedBy_int,	INWD_CollectionDate_dt," +
            //    "INWD_CollectionTime_time, INWD_TotalQty_int, INWD_ReportCount_int, INWD_ContactNo_var, INWD_EmailId_var, INWD_ReceivedDate_dt) " +
            //    "values (" + EnquiryNo + ", " + ProposalId + " , " + RecordNo + ", " + RecordType + ", " + ClientId + ", " + SiteId + ", " + ContactPersonId + ", " +
            //    Building + ", " + Charges + ",	" + WorkOrderNo + ", " + Status + ", " + ReceivedBy + ", " + CollectionDate + ", " +
            //    CollectionTime + ", " + TotalQty + ", " + Subsets + ", " + ContactNo + ", " + EmailId + ", " + ReceivedDate + " )";

            //da = new SqlDataAdapter();
            //da.UpdateCommand = new SqlCommand(mySql, conn);
            //da.UpdateCommand.ExecuteNonQuery();

            using (SqlConnection connection = new SqlConnection(cnStr))
            {
                int ReferenceNo = 0;
                connection.Open();
                SqlCommand command = connection.CreateCommand();
                SqlTransaction transaction;
                // Start a local transaction.
                transaction = connection.BeginTransaction("InwardTransaction");
                // Must assign both transaction object and connection
                // to Command object for a pending local transaction
                command.Connection = connection;
                command.Transaction = transaction;
                try
                {
                    int RecordNo = 0;
                    command.CommandText = "select MAX(INWD_RecordNo_int) as CurrentNo from tbl_Inward where INWD_RecordType_var= '" + RecordType + "'";
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            RecordNo = reader.GetInt32(0);
                        }
                    }
                    RecordNo = RecordNo + 1;

                    command.CommandText =
                        "insert into tbl_Inward (INWD_ENQ_Id, INWD_PROP_Id, INWD_RecordNo_int, INWD_RecordType_var,	INWD_CL_Id,	INWD_SITE_Id, " +
                            "INWD_CONT_Id, INWD_Building_var, INWD_Charges_var, INWD_WorkOrderNo_var, INWD_Status_tint, INWD_ReceivedBy_int,	INWD_CollectionDate_dt," +
                            "INWD_CollectionTime_time, INWD_TotalQty_int, INWD_ReportCount_int, INWD_ContactNo_var, INWD_EmailId_var, INWD_ReceivedDate_dt) " +
                            "values (" + EnquiryNo + ", " + ProposalId + " , " + RecordNo + ", '" + RecordType + "', " + ClientId + ", " + SiteId + ", " + ContactPersonId + ", " +
                            "'" + Building + "', " + Charges + ",	'" + WorkOrderNo + "', " + Status + ", " + ReceivedBy + ", '" + CollectionDate + "', " +
                            "'" + CollectionTime + "', " + TotalQty + ", " + Subsets + ", '" + ContactNo + "', '" + EmailId + "', '" + ReceivedDate + "' )";
                    command.ExecuteNonQuery();
                    command.CommandText = "SELECT @@IDENTITY";
                    ReferenceNo = Convert.ToInt32(command.ExecuteScalar());
                    // Attempt to commit the transaction.
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Commit Exception Type: {0}", ex.GetType());
                    Console.WriteLine("  Message: {0}", ex.Message);

                    // Attempt to roll back the transaction.
                    try
                    {
                        transaction.Rollback();
                    }
                    catch (Exception ex2)
                    {
                        // This catch block will handle any errors that may have occurred
                        // on the server that would cause the rollback to fail, such as
                        // a closed connection.
                        Console.WriteLine("Rollback Exception Type: {0}", ex2.GetType());
                        Console.WriteLine("  Message: {0}", ex2.Message);
                    }
                }
                return ReferenceNo;
            }

        }

        public int Inward_Insert(int EnquiryNo, int ProposalId, string RecordType, int ClientId, int SiteId, int ContactPersonId, string Building, string Charges, string WorkOrderNo, byte Status, int ReceivedBy, DateTime CollectionDate, TimeSpan CollectionTime, int TotalQty, int Subsets, string ContactNo, string EmailId, DateTime ReceivedDate)
        {
            using (SqlConnection connection = new SqlConnection(cnStr))
            {
                int ReferenceNo = 0;
                connection.Open();
                SqlCommand command = connection.CreateCommand();
                SqlTransaction transaction;
                // Start a local transaction.
                transaction = connection.BeginTransaction("InwardTransaction");
                // Must assign both transaction object and connection
                // to Command object for a pending local transaction
                command.Connection = connection;
                command.Transaction = transaction;
                try
                {
                    //update record no as next
                    command.CommandText =
                        "Update tbl_RecordNo Set " + RecordType.Replace("-", "") + "_RecordNo = " + RecordType.Replace("-", "") + "_RecordNo + 1";
                    command.ExecuteNonQuery();
                    //get updated record no
                    int RecordNo = 0;
                    command.CommandText = " Select " + RecordType.Replace("-", "") + "_RecordNo From tbl_RecordNo ";
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            RecordNo = reader.GetInt32(0);

                            RecordNo = RecordNo - 1;
                        }
                    }
                    //Insert record 
                    command.CommandText =
                        "insert into tbl_Inward (INWD_ENQ_Id, INWD_PROP_Id, INWD_RecordNo_int, INWD_RecordType_var,	INWD_CL_Id,	INWD_SITE_Id, " +
                            "INWD_CONT_Id, INWD_Building_var, INWD_Charges_var, INWD_WorkOrderNo_var, INWD_Status_tint, INWD_ReceivedBy_int,	INWD_CollectionDate_dt," +
                            "INWD_CollectionTime_time, INWD_TotalQty_int, INWD_ReportCount_int, INWD_ContactNo_var, INWD_EmailId_var, INWD_ReceivedDate_dt) " +
                            "values (" + EnquiryNo + ", " + ProposalId + " , " + RecordNo + ", '" + RecordType + "', " + ClientId + ", " + SiteId + ", " + ContactPersonId + ", " +
                            "'" + Building + "', " + Charges + ",	'" + WorkOrderNo + "', " + Status + ", " + ReceivedBy + ", '" + CollectionDate + "', " +
                            "'" + CollectionTime + "', " + TotalQty + ", " + Subsets + ", '" + ContactNo + "', '" + EmailId + "', '" + ReceivedDate + "' )";
                    command.ExecuteNonQuery();
                    //get inserted reference number
                    command.CommandText = "SELECT @@IDENTITY";
                    ReferenceNo = Convert.ToInt32(command.ExecuteScalar());
                    // Attempt to commit the transaction.
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Commit Exception Type: {0}", ex.GetType());
                    Console.WriteLine("  Message: {0}", ex.Message);

                    // Attempt to roll back the transaction.
                    try
                    {
                        transaction.Rollback();
                    }
                    catch (Exception ex2)
                    {
                        // This catch block will handle any errors that may have occurred
                        // on the server that would cause the rollback to fail, such as
                        // a closed connection.
                        Console.WriteLine("Rollback Exception Type: {0}", ex2.GetType());
                        Console.WriteLine("  Message: {0}", ex2.Message);
                    }
                }
                return ReferenceNo;
            }

        }
        public Int32 GetCurrentRecordNo(string recType)
        {
            Int32 RecNo = 1;
            SqlConnection cn = new SqlConnection(cnStr);
            SqlTransaction transaction;
            cn.Open();
            //get updated record no
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = " Select " + recType + " From tbl_RecordNo ";
            cmd.Connection = cn;
            transaction = cn.BeginTransaction();
            cmd.Transaction = transaction;
            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    RecNo = reader.GetInt32(0);
                }
            }
            cmd.Dispose();
            //new SqlCommand("update tbl_recordNo set  " + recType + "= " + Convert.ToInt32(RecNo + 1).ToString(), cn, transaction).ExecuteNonQuery();
            transaction.Commit();
            if (cn.State == ConnectionState.Open) cn.Close();
            return RecNo;
        }
        public Int32 GetnUpdateRecordNo(string recType)
        {
            Int32 RecNo = 1;
            SqlConnection cn = new SqlConnection(cnStr);
            SqlTransaction transaction;

            cn.Open();

            //try
            //{
            //get updated record no
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = " Select " + recType + " From tbl_RecordNo ";
            cmd.Connection = cn;
            transaction = cn.BeginTransaction();
            cmd.Transaction = transaction;
            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    RecNo = reader.GetInt32(0);
                }
            }
            cmd.Dispose();
            new SqlCommand("update tbl_recordNo set  " + recType + "= " + Convert.ToInt32(RecNo + 1).ToString(), cn, transaction).ExecuteNonQuery();

            transaction.Commit();
            //}
            //catch (Exception ex)
            //{
            //    Console.WriteLine("  Message: {0}", ex.Message);
            ////    transaction.Rollback();
            //}

            //finally 
            //{
            if (cn.State == ConnectionState.Open) cn.Close();

            //}
            return RecNo;
        }

        public void insertRecordTable(Int32 mENQId, Int32 mRecordNo, string mRecordType)
        {
            SqlConnection cn = new SqlConnection(cnStr);
            SqlTransaction transaction;
            cn.Open();
            transaction = cn.BeginTransaction();
            string mySql = "INSERT INTO RecordTable (ENQId,RecordNo,RecordType) values (" + mENQId + "," + mRecordNo + ",'" + mRecordType + "')";
            new SqlCommand(mySql, cn, transaction).ExecuteNonQuery();
            transaction.Commit();
            cn.Close();
        }

        public Int32 insertRecordTable_2(Int32 mENQId, Int32 mRecordNo, string mRecordType)
        {
            Int32 refno = 0;
            SqlConnection cn = new SqlConnection(cnStr);
            SqlTransaction transaction;
            cn.Open();
            transaction = cn.BeginTransaction();
            string mySql = "INSERT INTO RecordTable (ENQId,RecordNo,RecordType)  Output Inserted.ReferenceNo values (" + mENQId + "," + mRecordNo + ",'" + mRecordType + "')";

            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = mySql;
            cmd.Connection = cn;
            cmd.Transaction = transaction;
            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    refno = reader.GetInt32(0);
                }
            }
            cmd.Dispose();
            
            transaction.Commit();
            cn.Close();

            return refno;
        }

        public Int32 getRefNo(Int32 enqId)
        {
            Int32 refno = 0;
            SqlConnection cn = new SqlConnection(cnStr);
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = " Select referenceno from RecordTable where enqid=" + enqId.ToString();
            cn.Open();
            cmd.Connection = cn;
            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    refno = reader.GetInt32(0);
                }
            }
            cmd.Dispose();
            cn.Close();
            return refno;
        }

        public bool checkGSTInfoUpdated(string clientId, string siteId, string recType)
        {
            bool gstFlag = true;
            int couponCount = 0;
            if (recType == "CT")
            {
                LabDataDataContext dc = new LabDataDataContext();
                var couponsitespec = dc.Coupon_View_Sitewise(Convert.ToInt32(clientId), Convert.ToInt32(siteId), 0, DateTime.Now).ToList();
                couponCount = couponsitespec.Count();
                if (couponsitespec.Count() == 0)
                {
                    var coupon = dc.Coupon_View("", 0, 0, Convert.ToInt32(clientId), Convert.ToInt32(siteId), 0, DateTime.Now).ToList();
                    couponCount = coupon.Count();
                }
            }
            if (couponCount == 0)
            {
                if (clientId != "" && clientId != "0")
                {
                    DataTable dt = getGeneralData("SELECT CL_GST_bit FROM tbl_Client where CL_Id =" + clientId);
                    if (dt.Rows.Count > 0)
                    {
                        if (dt.Rows[0]["CL_GST_bit"].ToString() == "" || dt.Rows[0]["CL_GST_bit"].ToString() == string.Empty)
                            gstFlag = false;
                    }
                    else
                    {
                        gstFlag = false;
                    }
                }
                if (siteId != "" && siteId != "0")
                {
                    DataTable dt = getGeneralData("SELECT SITE_GST_bit FROM tbl_Site where SITE_Id =" + siteId);
                    if (dt.Rows.Count > 0)
                    {
                        if (dt.Rows[0]["SITE_GST_bit"].ToString() == "" || dt.Rows[0]["SITE_GST_bit"].ToString() == string.Empty)
                            gstFlag = false;
                    }
                    else
                    {
                        gstFlag = false;
                    }
                }
            }
            return gstFlag;
        }

        public void updateClientDiscount(int CL_Id, int disc)
        {
            SqlConnection con = new SqlConnection(cnStr);
            try
            {
                string mQueryString = @"update tbl_Client set CL_DiscSetting_tint='" + disc + "' where CL_Id='" + CL_Id + "'";
                con.Open();
                SqlCommand cmd = new SqlCommand(mQueryString, con);
                cmd.ExecuteNonQuery();
                cmd.Dispose();
            }
            catch (Exception e)
            {
                throw e;
            }

        }




        public DataTable getMaterialList()
        {
            SqlConnection con = new SqlConnection(cnStr);
            try
            {
                DataSet ds = new DataSet();
                DataTable dt = new DataTable();
                string mQueryString = @" select [MATERIAL_Id],[MATERIAL_Name_var],[MATERIAL_RecordType_var], (MATERIAL_RecordType_var +'  (' + MATERIAL_Name_var +')') as MatNewColm from tbl_Material Order by MATERIAL_Name_var";
                SqlDataAdapter da = new SqlDataAdapter(mQueryString, con);
                da.Fill(ds);
                dt = ds.Tables[0];
                ds.Dispose();
                return dt;
            }
            catch (Exception e)
            {
                throw e;
            }

        }


        public DataTable AlertDetailsViewTriggerEscalation(int Alert_Id)
        {
            SqlConnection con = new SqlConnection(cnStr);
            try
            {
                DataSet ds = new DataSet();
                DataTable dt = new DataTable();
                string str = "select * from tbl_AlertDetails where AlertDtl_Alert_Id=" + Alert_Id;
                SqlDataAdapter da = new SqlDataAdapter(str, con);//
                da.Fill(ds);
                dt = ds.Tables[0];
                ds.Dispose();
                return dt;
            }
            catch (Exception e)
            {
                throw e;
            }

        }

        public DataTable AlertDetailsViewForTrigger(int Alert_Id, string fromDate, string ToDate, int flagStatus)
        {
            SqlConnection con = new SqlConnection(cnStr);
            try
            {
                DataSet ds = new DataSet();
                DataTable dt = new DataTable();
                string str = "";
                if (Alert_Id == 1)
                {
                    if (flagStatus == 0)//all
                    {
                        str = @"----Proposal not sent for more than  1   days after receiving enquiry
				                Select a.ENQ_Id as AlertDtl_EnqNo_int,a.ENQ_Date_dt as AlertDtl_EnqDate_date,null as AlertDtl_CollctnEnqDate_date,'' as AlertDtl_BillNo_int,null as AlertDtl_BillDate_date,0 as AlertDtl_BillAmt_dec,
                                '' as AlertDtl_ProNo_var,0 as AlertDtl_ProNo_int,null as AlertDtl_ProDate_date,'' as AlertDtl_RefNo_var,0 as AlertDtl_RecNo_int,
                                c.CL_Name_var as AlertDtl_CL_Name_var,d.SITE_Name_var as AlertDtl_SITE_Name_var,e.MATERIAL_Name_var as AlertDtl_RecType_var,c.CL_Id as AlertDtl_CLId_int,d.SITE_Id as AlertDtl_SiteId_int,1 as AlertDtl_Alert_Id
                                ,(select USER_NAME_var from tbl_User x where x.USER_Id IN (select ROUTE_ME_Id from tbl_Route where ROUTE_Id=d.SITE_Route_Id)) as AlertDtl_MEName_var
                                ,null as AlertDtl_ProOrderDate_date,null as AlertDtl_BillModifiedDate_date,null as AlertDtl_InwdRecDate_date
                                ,null as AlertDtl_AppDate_date,null as AlertDtl_EnteredDate_date,null as AlertDtl_CheckedDate_date,null as AlertDtl_OutwrdDate_date,'' as AlertDtl_TestName_var
                                from tbl_Enquiry as a,tbl_Proposal as b,tbl_Client as c,tbl_Site as d,tbl_Material as e
                                where a.ENQ_Id=b.Proposal_EnqNo and
                                 a.ENQ_CL_Id=c.CL_Id
                                and a.ENQ_SITE_Id=d.SITE_Id
                                and a.ENQ_MATERIAL_Id=e.MATERIAL_Id
                                and CONVERT (DATE, a.ENQ_Date_dt) >=CONVERT (DATE,'" + fromDate + "') and CONVERT (DATE, a.ENQ_Date_dt) <=CONVERT (DATE,'" + ToDate + "') " +
                                @"and datediff(d,convert(date,a.ENQ_Date_dt),convert(date,GETDATE())) > 1 
                                and datediff(d,convert(date,a.ENQ_Date_dt),convert(date,b.Proposal_Date)) > 1 
                               
                                						
                                Union
                                						
                                Select a.ENQNEW_Id as AlertDtl_EnqNo_int,a.ENQNEW_Date_dt as AlertDtl_EnqDate_date,null as AlertDtl_CollctnEnqDate_date,'' as AlertDtl_BillNo_int,null as AlertDtl_BillDate_date,0 as AlertDtl_BillAmt_dec,
                                '' as AlertDtl_ProNo_var,0 as AlertDtl_ProNo_int,null as AlertDtl_ProDate_date,'' as AlertDtl_RefNo_var,0 as AlertDtl_RecNo_int,
                                ENQNEW_ClientName_var as AlertDtl_CL_Name_var,ENQNEW_SiteName_var as AlertDtl_SITE_Name_var,e.MATERIAL_Name_var as AlertDtl_RecType_var,0 as CL_Id,0 as SITE_Id,1 as AlertDtl_Alert_Id
                                ,(select USER_NAME_var from tbl_User x where x.USER_Id IN (select ROUTE_ME_Id from tbl_Route where ROUTE_Id=a.ENQNEW_ROUTE_Id)) as AlertDtl_MEName_var
                                   ,null as AlertDtl_ProOrderDate_date,null as AlertDtl_BillModifiedDate_date,null as AlertDtl_InwdRecDate_date
                                ,null as AlertDtl_AppDate_date,null as AlertDtl_EnteredDate_date,null as AlertDtl_CheckedDate_date,null as AlertDtl_OutwrdDate_date,'' as AlertDtl_TestName_var
                              from tbl_EnquiryNewClient as a,tbl_Proposal as b,tbl_Material as e
                                where a.ENQNEW_Id=b.Proposal_EnqNo and
                                 a.ENQNEW_MATERIAL_Id=e.MATERIAL_Id
                                and CONVERT (DATE, a.ENQNEW_Date_dt) >=CONVERT (DATE,'" + fromDate + "') and CONVERT (DATE, a.ENQNEW_Date_dt) <=CONVERT (DATE,'" + ToDate + "') " +
                                @"and datediff(d,convert(date,a.ENQNEW_Date_dt),convert(date,GETDATE())) > 1 
                               and datediff(d,convert(date,a.ENQNEW_Date_dt),convert(date,b.Proposal_Date)) > 1 
                                 order by AlertDtl_RecType_var";
                    }
                    else//pending
                    {
                        str = @"----Proposal not sent for more than  1   days after receiving enquiry
				                Select a.ENQ_Id as AlertDtl_EnqNo_int,a.ENQ_Date_dt as AlertDtl_EnqDate_date,null as AlertDtl_CollctnEnqDate_date,'' as AlertDtl_BillNo_int,null as AlertDtl_BillDate_date,0 as AlertDtl_BillAmt_dec,
                                '' as AlertDtl_ProNo_var,0 as AlertDtl_ProNo_int,null as AlertDtl_ProDate_date,'' as AlertDtl_RefNo_var,0 as AlertDtl_RecNo_int,
                                c.CL_Name_var as AlertDtl_CL_Name_var,d.SITE_Name_var as AlertDtl_SITE_Name_var,e.MATERIAL_Name_var as AlertDtl_RecType_var,c.CL_Id as AlertDtl_CLId_int,d.SITE_Id as AlertDtl_SiteId_int,1 as AlertDtl_Alert_Id
                                ,(select USER_NAME_var from tbl_User x where x.USER_Id IN (select ROUTE_ME_Id from tbl_Route where ROUTE_Id=d.SITE_Route_Id)) as AlertDtl_MEName_var
                                   ,null as AlertDtl_ProOrderDate_date,null as AlertDtl_BillModifiedDate_date,null as AlertDtl_InwdRecDate_date
                                ,null as AlertDtl_AppDate_date,null as AlertDtl_EnteredDate_date,null as AlertDtl_CheckedDate_date,null as AlertDtl_OutwrdDate_date,'' as AlertDtl_TestName_var
                             from tbl_Enquiry as a,tbl_Proposal as b,tbl_Client as c,tbl_Site as d,tbl_Material as e
                                where a.ENQ_Id=b.Proposal_EnqNo and
                                 a.ENQ_CL_Id=c.CL_Id
                                and a.ENQ_SITE_Id=d.SITE_Id
                                and a.ENQ_MATERIAL_Id=e.MATERIAL_Id
                                and CONVERT (DATE, a.ENQ_Date_dt) >=CONVERT (DATE,'" + fromDate + "') and CONVERT (DATE, a.ENQ_Date_dt) <=CONVERT (DATE,'" + fromDate + "') " +
                               @"and Proposal_Date is null
				                and datediff(d,convert(date,a.ENQ_Date_dt),convert(date,GETDATE())) > 1 
				               and a.ENQ_Status_tint  <> 2 and a.ENQ_OpenEnquiryStatus_var <> 'Already Collected' and a.ENQ_OpenEnquiryStatus_var <> 'To be Collected'
				
			                   
                                						
                                Union
                                						
                                Select a.ENQNEW_Id as AlertDtl_EnqNo_int,a.ENQNEW_Date_dt as AlertDtl_EnqDate_date,null as AlertDtl_CollctnEnqDate_date,'' as AlertDtl_BillNo_int,null as AlertDtl_BillDate_date,0 as AlertDtl_BillAmt_dec,
                                '' as AlertDtl_ProNo_var,0 as AlertDtl_ProNo_int,null as AlertDtl_ProDate_date,'' as AlertDtl_RefNo_var,0 as AlertDtl_RecNo_int,
                                ENQNEW_ClientName_var as AlertDtl_CL_Name_var,ENQNEW_SiteName_var as AlertDtl_SITE_Name_var,e.MATERIAL_Name_var as AlertDtl_RecType_var,c.CL_Id as AlertDtl_CLId_int,d.SITE_Id as AlertDtl_SiteId_int,1 as AlertDtl_Alert_Id
                                ,(select USER_NAME_var from tbl_User x where x.USER_Id IN (select ROUTE_ME_Id from tbl_Route where ROUTE_Id=a.ENQNEW_ROUTE_Id)) as AlertDtl_MEName_var
                                ,null as AlertDtl_ProOrderDate_date,null as AlertDtl_BillModifiedDate_date,null as AlertDtl_InwdRecDate_date
                                ,null as AlertDtl_AppDate_date,null as AlertDtl_EnteredDate_date,null as AlertDtl_CheckedDate_date,null as AlertDtl_OutwrdDate_date,'' as AlertDtl_TestName_var
                                from tbl_EnquiryNewClient as a,tbl_Proposal as b,tbl_Client as c,tbl_Site as d,tbl_Material as e
                                where  a.ENQNEW_Id=b.Proposal_EnqNo  and
                                a.ENQNEW_MATERIAL_Id=e.MATERIAL_Id
						        and CONVERT (DATE, a.ENQNEW_Date_dt) >=CONVERT (DATE,'" + fromDate + "') and CONVERT (DATE, a.ENQNEW_Date_dt) <=CONVERT (DATE,'" + fromDate + "') " +
                                @"and Proposal_Date is null
				                and datediff(d,convert(date,a.ENQNEW_Date_dt),convert(date,GETDATE())) > 1 
				               and a.ENQNEW_Status_tint  <> 2 and a.ENQNEW_OpenEnquiryStatus_var <> 'Already Collected' and a.ENQNEW_OpenEnquiryStatus_var <> 'To be Collected'
				                order by AlertDtl_RecType_var";
                    }
                }
                else if (Alert_Id == 2)
                {
                    if (flagStatus == 0)
                    {
                        str = @"----Order not received in 7 days after sending proposal
				                Select a.ENQ_Id as AlertDtl_EnqNo_int,a.ENQ_Date_dt as AlertDtl_EnqDate_date,null as AlertDtl_CollctnEnqDate_date,'' as AlertDtl_BillNo_int,null as AlertDtl_BillDate_date,0 as AlertDtl_BillAmt_dec,
                                b.Proposal_No as AlertDtl_ProNo_var,b.Proposal_Id as AlertDtl_ProNo_int,b.Proposal_Date as AlertDtl_ProDate_date,'' as AlertDtl_RefNo_var,0 as AlertDtl_RecNo_int,
                                c.CL_Name_var as AlertDtl_CL_Name_var,d.SITE_Name_var as AlertDtl_SITE_Name_var,e.MATERIAL_Name_var as AlertDtl_RecType_var,c.CL_Id as AlertDtl_CLId_int,d.SITE_Id as AlertDtl_SiteId_int,2 as AlertDtl_Alert_Id,
                                (select USER_NAME_var from tbl_User x where x.USER_Id IN (select ROUTE_ME_Id from tbl_Route where ROUTE_Id=d.SITE_Route_Id)) as AlertDtl_MEName_var
                                ,null as AlertDtl_ProOrderDate_date,null as AlertDtl_BillModifiedDate_date,null as AlertDtl_InwdRecDate_date
                                ,null as AlertDtl_AppDate_date,null as AlertDtl_EnteredDate_date,null as AlertDtl_CheckedDate_date,null as AlertDtl_OutwrdDate_date,'' as AlertDtl_TestName_var
                                from tbl_Enquiry as a,tbl_Proposal as b,tbl_Client as c,tbl_Site as d,tbl_Material as e
                                where a.ENQ_Id=b.Proposal_EnqNo
                                and a.ENQ_CL_Id=c.CL_Id
                                and a.ENQ_SITE_Id=d.SITE_Id
                                and a.ENQ_MATERIAL_Id=e.MATERIAL_Id
                                and CONVERT (DATE, b.Proposal_Date) >=CONVERT (DATE,'" + fromDate + "') and CONVERT (DATE, b.Proposal_Date) <=CONVERT (DATE,'" + ToDate + "') " +
                                @"and datediff(d,convert(date,b.Proposal_Date),convert(date,b.Proposal_OrderDate_dt)) > 7
				                and datediff(d,convert(date,b.Proposal_Date),convert(date,GETDATE())) > 7 
				                and b.Proposal_NewClientStatus=0
                                and b.Proposal_ActiveStatus_bit=0 and b.Proposal_AppEnqStatus_bit=0
                        
                                Union
                                						
                                Select a.ENQNEW_Id as AlertDtl_EnqNo_int,a.ENQNEW_Date_dt as AlertDtl_EnqDate_date,null as AlertDtl_CollctnEnqDate_date,'' as AlertDtl_BillNo_int,null as AlertDtl_BillDate_date,0 as AlertDtl_BillAmt_dec,
                                b.Proposal_No as AlertDtl_ProNo_var,b.Proposal_Id as AlertDtl_ProNo_int,b.Proposal_Date as AlertDtl_ProDate_date,'' as AlertDtl_RefNo_var,0 as MISRecordNo
                                ,ENQNEW_ClientName_var as AlertDtl_CL_Name_var,ENQNEW_SiteName_var as AlertDtl_SITE_Name_var,e.MATERIAL_Name_var as AlertDtl_RecType_var,0 as CL_Id,0 as SITE_Id,2 as AlertDtl_Alert_Id
                               ,(select USER_NAME_var from tbl_User x where x.USER_Id IN (select ROUTE_ME_Id from tbl_Route where ROUTE_Id=a.ENQNEW_ROUTE_Id)) as AlertDtl_MEName_var
                                ,null as AlertDtl_ProOrderDate_date,null as AlertDtl_BillModifiedDate_date,null as AlertDtl_InwdRecDate_date
                                ,null as AlertDtl_AppDate_date,null as AlertDtl_EnteredDate_date,null as AlertDtl_CheckedDate_date,null as AlertDtl_OutwrdDate_date,'' as AlertDtl_TestName_var
                                from tbl_EnquiryNewClient as a,tbl_Proposal as b,tbl_Material as e
                                where a.ENQNEW_Id=b.Proposal_EnqNo
                                and a.ENQNEW_MATERIAL_Id=e.MATERIAL_Id 
                                and CONVERT (DATE, b.Proposal_Date) >=CONVERT (DATE,'" + fromDate + "') and CONVERT (DATE, b.Proposal_Date) <=CONVERT (DATE,'" + ToDate + "') " +
                                @"and datediff(d,convert(date,b.Proposal_Date),convert(date,b.Proposal_OrderDate_dt)) > 7
				                and datediff(d,convert(date,b.Proposal_Date),convert(date,GETDATE())) > 7 
				                and b.Proposal_NewClientStatus=1 
                                and b.Proposal_ActiveStatus_bit=0 and b.Proposal_AppEnqStatus_bit=0
                                order by AlertDtl_RecType_var";
                    }
                    else
                    {
                        str = @"----Order not received in 7 days after sending proposal
				               Select a.ENQ_Id as AlertDtl_EnqNo_int,a.ENQ_Date_dt as AlertDtl_EnqDate_date,null as AlertDtl_CollctnEnqDate_date,'' as AlertDtl_BillNo_int,null as AlertDtl_BillDate_date,0 as AlertDtl_BillAmt_dec,
                                 b.Proposal_No as AlertDtl_ProNo_var,b.Proposal_Id as AlertDtl_ProNo_int,b.Proposal_Date as AlertDtl_ProDate_date,'' as AlertDtl_RefNo_var,0 as AlertDtl_RecNo_int,
                                c.CL_Name_var as AlertDtl_CL_Name_var,d.SITE_Name_var as AlertDtl_SITE_Name_var,e.MATERIAL_Name_var as AlertDtl_RecType_var,c.CL_Id as AlertDtl_CLId_int,d.SITE_Id as AlertDtl_SiteId_int,2 as AlertDtl_Alert_Id,
                                (select USER_NAME_var from tbl_User x where x.USER_Id IN (select ROUTE_ME_Id from tbl_Route where ROUTE_Id=d.SITE_Route_Id)) as AlertDtl_MEName_var
                                ,null as AlertDtl_ProOrderDate_date,null as AlertDtl_BillModifiedDate_date,null as AlertDtl_InwdRecDate_date
                                ,null as AlertDtl_AppDate_date,null as AlertDtl_EnteredDate_date,null as AlertDtl_CheckedDate_date,null as AlertDtl_OutwrdDate_date,'' as AlertDtl_TestName_var
                                from tbl_Enquiry as a,tbl_Proposal as b,tbl_Client as c,tbl_Site as d,tbl_Material as e
                                where a.ENQ_Id=b.Proposal_EnqNo
                                and a.ENQ_CL_Id=c.CL_Id
                                and a.ENQ_SITE_Id=d.SITE_Id
                                and a.ENQ_MATERIAL_Id=e.MATERIAL_Id
                                and CONVERT (DATE, b.Proposal_Date) >=CONVERT (DATE,'" + fromDate + "') and CONVERT (DATE, b.Proposal_Date) <=CONVERT (DATE,'" + ToDate + "') " +
                                @"and b.Proposal_OrderDate_dt is null
                                and datediff(d,convert(date,b.Proposal_Date),convert(date,GETDATE())) > 7
				                and b.Proposal_NewClientStatus=0
                                and b.Proposal_ActiveStatus_bit=0 and b.Proposal_AppEnqStatus_bit=0
                        
                                Union
                                						
                                Select a.ENQNEW_Id as AlertDtl_EnqNo_int,a.ENQNEW_Date_dt as AlertDtl_EnqDate_date,null as AlertDtl_CollctnEnqDate_date,'' as AlertDtl_BillNo_int,null as AlertDtl_BillDate_date,0 as AlertDtl_BillAmt_dec,
                                b.Proposal_No as AlertDtl_ProNo_var,b.Proposal_Id as AlertDtl_ProNo_int,b.Proposal_Date as AlertDtl_ProDate_date,'' as AlertDtl_RefNo_var,0 as MISRecordNo
                                ,ENQNEW_ClientName_var as AlertDtl_CL_Name_var,ENQNEW_SiteName_var as AlertDtl_SITE_Name_var,e.MATERIAL_Name_var as AlertDtl_RecType_var,0 as CL_Id,0 as SITE_Id,2 as AlertDtl_Alert_Id
                                ,(select USER_NAME_var from tbl_User x where x.USER_Id IN (select ROUTE_ME_Id from tbl_Route where ROUTE_Id=a.ENQNEW_ROUTE_Id)) as AlertDtl_MEName_var
                                ,null as AlertDtl_ProOrderDate_date,null as AlertDtl_BillModifiedDate_date,null as AlertDtl_InwdRecDate_date
                                ,null as AlertDtl_AppDate_date,null as AlertDtl_EnteredDate_date,null as AlertDtl_CheckedDate_date,null as AlertDtl_OutwrdDate_date,'' as AlertDtl_TestName_var
                                from tbl_EnquiryNewClient as a,tbl_Proposal as b,tbl_Material as e--tbl_Client as c,tbl_Site as d,
                                where a.ENQNEW_Id=b.Proposal_EnqNo
                                and a.ENQNEW_MATERIAL_Id=e.MATERIAL_Id 
                                and CONVERT (DATE, b.Proposal_Date) >=CONVERT (DATE,'" + fromDate + "') and CONVERT (DATE, b.Proposal_Date) <=CONVERT (DATE,'" + ToDate + "') " +
                                @"and b.Proposal_OrderDate_dt is null
                                and datediff(d,convert(date,b.Proposal_Date),convert(date,GETDATE())) > 7
				                and b.Proposal_NewClientStatus=1 
                                and b.Proposal_ActiveStatus_bit=0 and b.Proposal_AppEnqStatus_bit=0
                                order by AlertDtl_RecType_var";
                    }
                }
                else if (Alert_Id == 3)
                {
                    if (flagStatus == 0)
                    {
                        str = @"----Material not collected in 2 days after receiving order
				                Select a.ENQ_Id as AlertDtl_EnqNo_int,a.ENQ_Date_dt as AlertDtl_EnqDate_date,b.Proposal_OrderDate_dt as AlertDtl_ProOrderDate_date,'' as AlertDtl_BillNo_int,null as AlertDtl_BillDate_date,0 as AlertDtl_BillAmt_dec,
                                b.Proposal_No as AlertDtl_ProNo_var,b.Proposal_Id as AlertDtl_ProNo_int,b.Proposal_Date as AlertDtl_ProDate_date,'' as AlertDtl_RefNo_var,0 as AlertDtl_RecNo_int,
                                c.CL_Name_var as AlertDtl_CL_Name_var,d.SITE_Name_var as AlertDtl_SITE_Name_var,e.MATERIAL_Name_var as AlertDtl_RecType_var,c.CL_Id as AlertDtl_CLId_int,d.SITE_Id as AlertDtl_SiteId_int,3 as AlertDtl_Alert_Id,
                                (select USER_NAME_var from tbl_User x where x.USER_Id IN (select ROUTE_ME_Id from tbl_Route where ROUTE_Id=d.SITE_Route_Id)) as AlertDtl_MEName_var
                                ,null as AlertDtl_CollctnEnqDate_date,null as AlertDtl_BillModifiedDate_date,null as AlertDtl_InwdRecDate_date
                                ,null as AlertDtl_AppDate_date,null as AlertDtl_EnteredDate_date,null as AlertDtl_CheckedDate_date,null as AlertDtl_OutwrdDate_date,'' as AlertDtl_TestName_var
                                from tbl_Enquiry as a,tbl_Proposal as b,tbl_Client as c,tbl_Site as d,tbl_Material as e
                                where a.ENQ_Id=b.Proposal_EnqNo
                                and a.ENQ_CL_Id=c.CL_Id
                                and a.ENQ_SITE_Id=d.SITE_Id
                                and a.ENQ_MATERIAL_Id=e.MATERIAL_Id
                                and CONVERT (DATE, b.Proposal_OrderDate_dt) >=CONVERT (DATE,'" + fromDate + "') and CONVERT (DATE, b.Proposal_OrderDate_dt) <=CONVERT (DATE,'" + ToDate + "') " +
                                @"and datediff(d,convert(date,b.Proposal_OrderDate_dt),convert(date,a.ENQ_CollectionDate_dt)) > 2
				                and datediff(d,convert(date,b.Proposal_OrderDate_dt),convert(date,GETDATE())) > 2
                                and b.Proposal_NewClientStatus=0 and b.Proposal_ActiveStatus_bit=0  and b.Proposal_AppEnqStatus_bit=0
                                
                                Union
                                						
                                Select a.ENQNEW_Id as AlertDtl_EnqNo_int,a.ENQNEW_Date_dt as AlertDtl_EnqDate_date,b.Proposal_OrderDate_dt as AlertDtl_ProOrderDate_date,'' as AlertDtl_BillNo_int,null as AlertDtl_BillDate_date,0 as AlertDtl_BillAmt_dec,
                                b.Proposal_No as AlertDtl_ProNo_var,b.Proposal_Id as AlertDtl_ProNo_int,b.Proposal_Date as AlertDtl_ProDate_date,'' as AlertDtl_RefNo_var,0 as MISRecordNo
                                ,ENQNEW_ClientName_var as AlertDtl_CL_Name_var,ENQNEW_SiteName_var as AlertDtl_SITE_Name_var,e.MATERIAL_Name_var as AlertDtl_RecType_var,0 as CL_Id,0 as SITE_Id,2 as AlertDtl_Alert_Id
                                ,(select USER_NAME_var from tbl_User x where x.USER_Id IN (select ROUTE_ME_Id from tbl_Route where ROUTE_Id=a.ENQNEW_ROUTE_Id)) as AlertDtl_MEName_var
                                ,null as AlertDtl_CollctnEnqDate_date,null as AlertDtl_BillModifiedDate_date,null as AlertDtl_InwdRecDate_date
                                ,null as AlertDtl_AppDate_date,null as AlertDtl_EnteredDate_date,null as AlertDtl_CheckedDate_date,null as AlertDtl_OutwrdDate_date,'' as AlertDtl_TestName_var
                                from tbl_EnquiryNewClient as a,tbl_Proposal as b,tbl_Material as e
                                where a.ENQNEW_Id=b.Proposal_EnqNo
                                and a.ENQNEW_MATERIAL_Id=e.MATERIAL_Id 
                                and CONVERT (DATE, b.Proposal_OrderDate_dt) >=CONVERT (DATE,'" + fromDate + "') and CONVERT (DATE, b.Proposal_OrderDate_dt) <=CONVERT (DATE,'" + ToDate + "') " +
                                @"and datediff(d,convert(date,b.Proposal_OrderDate_dt),convert(date,a.ENQNEW_CollectionDate_dt)) > 2
				                and datediff(d,convert(date,b.Proposal_OrderDate_dt),convert(date,GETDATE())) > 2
				                and b.Proposal_NewClientStatus=1 and b.Proposal_ActiveStatus_bit=0 and b.Proposal_AppEnqStatus_bit=0
                                order by AlertDtl_RecType_var";

                    }
                    else
                    {
                        str = @"----Material not collected in 2 days after receiving order
				                Select a.ENQ_Id as AlertDtl_EnqNo_int,a.ENQ_Date_dt as AlertDtl_EnqDate_date,b.Proposal_OrderDate_dt as AlertDtl_ProOrderDate_date,'' as AlertDtl_BillNo_int,null as AlertDtl_BillDate_date,0 as AlertDtl_BillAmt_dec,
                                b.Proposal_No as AlertDtl_ProNo_var,b.Proposal_Id as AlertDtl_ProNo_int,b.Proposal_Date as AlertDtl_ProDate_date,'' as AlertDtl_RefNo_var,0 as AlertDtl_RecNo_int,
                                c.CL_Name_var as AlertDtl_CL_Name_var,d.SITE_Name_var as AlertDtl_SITE_Name_var,e.MATERIAL_Name_var as AlertDtl_RecType_var,c.CL_Id as AlertDtl_CLId_int,d.SITE_Id as AlertDtl_SiteId_int,3 as AlertDtl_Alert_Id,
                                (select USER_NAME_var from tbl_User x where x.USER_Id IN (select ROUTE_ME_Id from tbl_Route where ROUTE_Id=d.SITE_Route_Id)) as AlertDtl_MEName_var
                                ,null as AlertDtl_CollctnEnqDate_date,null as AlertDtl_BillModifiedDate_date,null as AlertDtl_InwdRecDate_date
                                ,null as AlertDtl_AppDate_date,null as AlertDtl_EnteredDate_date,null as AlertDtl_CheckedDate_date,null as AlertDtl_OutwrdDate_date,'' as AlertDtl_TestName_var
                                from tbl_Enquiry as a,tbl_Proposal as b,tbl_Client as c,tbl_Site as d,tbl_Material as e
                                where a.ENQ_Id=b.Proposal_EnqNo
                                and a.ENQ_CL_Id=c.CL_Id
                                and a.ENQ_SITE_Id=d.SITE_Id
                                and a.ENQ_MATERIAL_Id=e.MATERIAL_Id
                                and CONVERT (DATE, b.Proposal_OrderDate_dt) >=CONVERT (DATE,'" + fromDate + "') and CONVERT (DATE, b.Proposal_OrderDate_dt) <=CONVERT (DATE,'" + ToDate + "') " +
                                @"and a.ENQ_CollectionDate_dt is null
                                and datediff(d,convert(date,b.Proposal_OrderDate_dt),convert(date,GETDATE())) > 2 
                                and b.Proposal_NewClientStatus=0 and b.Proposal_ActiveStatus_bit=0 and b.Proposal_AppEnqStatus_bit=0
                                 
                                Union
                                						
                                Select a.ENQNEW_Id as AlertDtl_EnqNo_int,a.ENQNEW_Date_dt as AlertDtl_EnqDate_date,b.Proposal_OrderDate_dt as AlertDtl_ProOrderDate_date,'' as AlertDtl_BillNo_int,null as AlertDtl_BillDate_date,0 as AlertDtl_BillAmt_dec,
                                b.Proposal_No as AlertDtl_ProNo_var,b.Proposal_Id as AlertDtl_ProNo_int,b.Proposal_Date as AlertDtl_ProDate_date,'' as AlertDtl_RefNo_var,0 as MISRecordNo
                                ,ENQNEW_ClientName_var as AlertDtl_CL_Name_var,ENQNEW_SiteName_var as AlertDtl_SITE_Name_var,e.MATERIAL_Name_var as AlertDtl_RecType_var,0 as CL_Id,0 as SITE_Id,2 as AlertDtl_Alert_Id
                                ,(select USER_NAME_var from tbl_User x where x.USER_Id IN (select ROUTE_ME_Id from tbl_Route where ROUTE_Id=a.ENQNEW_ROUTE_Id)) as AlertDtl_MEName_var
                                ,null as AlertDtl_CollctnEnqDate_date,null as AlertDtl_BillModifiedDate_date,null as AlertDtl_InwdRecDate_date
                                ,null as AlertDtl_AppDate_date,null as AlertDtl_EnteredDate_date,null as AlertDtl_CheckedDate_date,null as AlertDtl_OutwrdDate_date,'' as AlertDtl_TestName_var
                                from tbl_EnquiryNewClient as a,tbl_Proposal as b,tbl_Material as e
                                where a.ENQNEW_Id=b.Proposal_EnqNo
                                and a.ENQNEW_MATERIAL_Id=e.MATERIAL_Id 
                                and CONVERT (DATE, b.Proposal_OrderDate_dt) >=CONVERT (DATE,'" + fromDate + "') and CONVERT (DATE, b.Proposal_OrderDate_dt) <=CONVERT (DATE,'" + ToDate + "') " +
                                @"and a.ENQNEW_CollectionDate_dt is null
                                and datediff(d,convert(date,b.Proposal_OrderDate_dt),convert(date,GETDATE())) > 2
				                and b.Proposal_NewClientStatus=1 and b.Proposal_ActiveStatus_bit=0 and b.Proposal_AppEnqStatus_bit=0
                                order by AlertDtl_RecType_var";

                    }
                }
                else if (Alert_Id == 4)
                {
                    if (flagStatus == 0)
                    {
                        str = @"----Material not inwarded in 1 day after collection
				                Select a.ENQ_Id as AlertDtl_EnqNo_int,a.ENQ_Date_dt as AlertDtl_EnqDate_date,a.ENQ_CollectionDate_dt as AlertDtl_CollctnEnqDate_date,'' as AlertDtl_BillNo_int,null as AlertDtl_BillDate_date,0 as AlertDtl_BillAmt_dec,
                                '' as AlertDtl_ProNo_var,0 as AlertDtl_ProNo_int,null as AlertDtl_ProDate_date,'' as AlertDtl_RefNo_var,0 as AlertDtl_RecNo_int,
                                c.CL_Name_var as AlertDtl_CL_Name_var,d.SITE_Name_var as AlertDtl_SITE_Name_var,e.MATERIAL_Name_var as AlertDtl_RecType_var,c.CL_Id as AlertDtl_CLId_int,d.SITE_Id as AlertDtl_SiteId_int,4 as AlertDtl_Alert_Id,
                                (select USER_NAME_var from tbl_User x where x.USER_Id IN (select ROUTE_ME_Id from tbl_Route where ROUTE_Id=d.SITE_Route_Id)) as AlertDtl_MEName_var
                                ,null as AlertDtl_ProOrderDate_date,null as AlertDtl_BillModifiedDate_date,null as AlertDtl_InwdRecDate_date
                                ,null as AlertDtl_AppDate_date,null as AlertDtl_EnteredDate_date,null as AlertDtl_CheckedDate_date,null as AlertDtl_OutwrdDate_date,'' as AlertDtl_TestName_var
                                from tbl_Enquiry as a,tbl_Inward as b,tbl_Client as c,tbl_Site as d,tbl_Material as e
                                where a.ENQ_Id=b.INWD_ENQ_Id
                                and a.ENQ_CL_Id=c.CL_Id
                                and a.ENQ_SITE_Id=d.SITE_Id
                                and a.ENQ_MATERIAL_Id=e.MATERIAL_Id
                                and CONVERT (DATE, ENQ_CollectionDate_dt) >=CONVERT (DATE,'" + fromDate + "') and CONVERT (DATE,ENQ_CollectionDate_dt) <=CONVERT (DATE,'" + ToDate + "') " +
                                @"and datediff(d,convert(date,a.ENQ_CollectionDate_dt),convert(date,b.INWD_ReceivedDate_dt)) > 1
				                and datediff(d,convert(date,a.ENQ_CollectionDate_dt),convert(date,GETDATE())) > 1 
                                and a.ENQ_Status_tint <> 2 order by AlertDtl_RecType_var";
                    }
                    else
                    {
                        str = @"----Material not inwarded in 1 day after collection
				                Select a.ENQ_Id as AlertDtl_EnqNo_int,a.ENQ_Date_dt as AlertDtl_EnqDate_date,a.ENQ_CollectionDate_dt as AlertDtl_CollctnEnqDate_date,'' as AlertDtl_BillNo_int,null as AlertDtl_BillDate_date,0 as AlertDtl_BillAmt_dec,
                                '' as AlertDtl_ProNo_var,0 as AlertDtl_ProNo_int,null as AlertDtl_ProDate_date,'' as AlertDtl_RefNo_var,0 as AlertDtl_RecNo_int,
                                c.CL_Name_var as AlertDtl_CL_Name_var,d.SITE_Name_var as AlertDtl_SITE_Name_var,e.MATERIAL_Name_var as AlertDtl_RecType_var,c.CL_Id as AlertDtl_CLId_int,d.SITE_Id as AlertDtl_SiteId_int,4 as AlertDtl_Alert_Id,
                                (select USER_NAME_var from tbl_User x where x.USER_Id IN (select ROUTE_ME_Id from tbl_Route where ROUTE_Id=d.SITE_Route_Id)) as AlertDtl_MEName_var
                                ,null as AlertDtl_ProOrderDate_date,null as AlertDtl_BillModifiedDate_date,null as AlertDtl_InwdRecDate_date
                                ,null as AlertDtl_AppDate_date,null as AlertDtl_EnteredDate_date,null as AlertDtl_CheckedDate_date,null as AlertDtl_OutwrdDate_date,'' as AlertDtl_TestName_var
                                from tbl_Enquiry as a,tbl_Inward as b,tbl_Client as c,tbl_Site as d,tbl_Material as e
                                where a.ENQ_Id=b.INWD_ENQ_Id
                                and a.ENQ_CL_Id=c.CL_Id
                                and a.ENQ_SITE_Id=d.SITE_Id
                                and a.ENQ_MATERIAL_Id=e.MATERIAL_Id
                                and CONVERT (DATE, ENQ_CollectionDate_dt) >=CONVERT (DATE,'" + fromDate + "') and CONVERT (DATE,ENQ_CollectionDate_dt) <=CONVERT (DATE,'" + ToDate + "') " +
                                @"and b.INWD_ReceivedDate_dt is null 
                                and datediff(d,convert(date,a.ENQ_CollectionDate_dt),convert(date,GETDATE())) > 1 
                                and a.ENQ_Status_tint <> 2 order by AlertDtl_RecType_var";

                    }
                }
                else if (Alert_Id == 5)
                {
                    if (flagStatus == 0)
                    {
                        str = @"----Inward not approved 1 day after material inward
				                Select 0 as AlertDtl_EnqNo_int,null as AlertDtl_EnqDate_date,b.INWD_ReceivedDate_dt as AlertDtl_InwdRecDate_date,'' as AlertDtl_BillNo_int,null as AlertDtl_BillDate_date,0 as AlertDtl_BillAmt_dec,
                                '' as AlertDtl_ProNo_var,0 as AlertDtl_ProNo_int,null as AlertDtl_ProDate_date, MISRefNo as AlertDtl_RefNo_var,MISRecordNo as AlertDtl_RecNo_int,
                                c.CL_Name_var as AlertDtl_CL_Name_var,d.SITE_Name_var as AlertDtl_SITE_Name_var,
                                (select MATERIAL_Name_var from tbl_Material e where e.MATERIAL_RecordType_var=b.INWD_RecordType_var) as AlertDtl_RecType_var
				                ,c.CL_Id as AlertDtl_CLId_int,d.SITE_Id as AlertDtl_SiteId_int,5 as AlertDtl_Alert_Id,
                                (select USER_NAME_var from tbl_User x where x.USER_Id IN (select ROUTE_ME_Id from tbl_Route where ROUTE_Id=d.SITE_Route_Id)) as AlertDtl_MEName_var
                                ,null as AlertDtl_ProOrderDate_date,null as AlertDtl_BillModifiedDate_date,null as AlertDtl_CollctnEnqDate_date
                                ,null as AlertDtl_AppDate_date,null as AlertDtl_EnteredDate_date,null as AlertDtl_CheckedDate_date,null as AlertDtl_OutwrdDate_date,'' as AlertDtl_TestName_var
                                from tbl_Inward as b,tbl_Client as c,tbl_Site as d,tbl_MISDetail as f--,tbl_Material as e
                                where --a.ENQ_Id=b.INWD_ENQ_Id and
                                b.INWD_RecordNo_int=f.MISRecordNo
                                and b.INWD_RecordType_var=f.MISRecType
                                and b.INWD_CL_Id=c.CL_Id
                                and b.INWD_SITE_Id=d.SITE_Id
                                and CONVERT (DATE, b.INWD_ReceivedDate_dt) >=CONVERT (DATE,'" + fromDate + "') and CONVERT (DATE,b.INWD_ReceivedDate_dt) <=CONVERT (DATE,'" + ToDate + "') " +
                                @"and datediff(d,convert(date,b.INWD_ReceivedDate_dt),convert(date,f.MISApprovedDt)) > 1
				                and datediff(d,convert(date,b.INWD_ReceivedDate_dt),convert(date,GETDATE())) > 1
                                order by AlertDtl_RecType_var";
                    }
                    else
                    {
                        str = @"----Inward not approved 1 day after material inward
				                Select 0 as AlertDtl_EnqNo_int,null as AlertDtl_EnqDate_date,a.INWD_ReceivedDate_dt as AlertDtl_InwdRecDate_date,'' as AlertDtl_BillNo_int,null as AlertDtl_BillDate_date,0 as AlertDtl_BillAmt_dec,
                                '' as AlertDtl_ProNo_var,0 as AlertDtl_ProNo_int,null as AlertDtl_ProDate_date, MISRefNo as AlertDtl_RefNo_var,MISRecordNo as AlertDtl_RecNo_int,
                                c.CL_Name_var as AlertDtl_CL_Name_var,d.SITE_Name_var as AlertDtl_SITE_Name_var,
                                (select MATERIAL_Name_var from tbl_Material e where e.MATERIAL_RecordType_var=a.INWD_RecordType_var) as AlertDtl_RecType_var
				                ,c.CL_Id as AlertDtl_CLId_int,d.SITE_Id as AlertDtl_SiteId_int,5 as AlertDtl_Alert_Id,
                                (select USER_NAME_var from tbl_User x where x.USER_Id IN (select ROUTE_ME_Id from tbl_Route where ROUTE_Id=d.SITE_Route_Id)) as AlertDtl_MEName_var
                                ,null as AlertDtl_ProOrderDate_date,null as AlertDtl_BillModifiedDate_date,null as AlertDtl_CollctnEnqDate_date
                                ,null as AlertDtl_AppDate_date,null as AlertDtl_EnteredDate_date,null as AlertDtl_CheckedDate_date,null as AlertDtl_OutwrdDate_date,'' as AlertDtl_TestName_var
                                from tbl_Inward as b,tbl_Client as c,tbl_Site as d,tbl_MISDetail as f
                                where --a.ENQ_Id=b.INWD_ENQ_Id and --tbl_Enquiry as a,
                                b.INWD_RecordNo_int=f.MISRecordNo
                                and b.INWD_RecordType_var=f.MISRecType
                                and b.INWD_CL_Id=c.CL_Id
                                and b.INWD_SITE_Id=d.SITE_Id
                                and CONVERT (DATE, b.INWD_ReceivedDate_dt) >=CONVERT (DATE,'" + fromDate + "') and CONVERT (DATE,b.INWD_ReceivedDate_dt) <=CONVERT (DATE,'" + ToDate + "') " +
                                @" and f.MISApprovedDt is null 
                                and datediff(d,convert(date,b.INWD_ReceivedDate_dt),convert(date,GETDATE())) > 1
                                order by AlertDtl_RecType_var";

                    }
                }
                else if (Alert_Id == 6)
                {
                    if (flagStatus == 0)
                    {
                        str = @"----Data Not enterd in 5 day after material inward approval
				                Select 0 as AlertDtl_EnqNo_int,null as AlertDtl_EnqDate_date,f.MISApprovedDt as AlertDtl_AppDate_date,'' as AlertDtl_BillNo_int,null as AlertDtl_BillDate_date,0 as AlertDtl_BillAmt_dec,
                                '' as AlertDtl_ProNo_var,0 as AlertDtl_ProNo_int,null as AlertDtl_ProDate_date, MISRefNo as AlertDtl_RefNo_var,MISRecordNo as AlertDtl_RecNo_int,
                                c.CL_Name_var as AlertDtl_CL_Name_var,d.SITE_Name_var as AlertDtl_SITE_Name_var,
                                (select MATERIAL_Name_var from tbl_Material e where e.MATERIAL_RecordType_var=b.INWD_RecordType_var) as AlertDtl_RecType_var
				                ,c.CL_Id as AlertDtl_CLId_int,d.SITE_Id as AlertDtl_SiteId_int,6 as AlertDtl_Alert_Id,
                                (select USER_NAME_var from tbl_User x where x.USER_Id IN (select ROUTE_ME_Id from tbl_Route where ROUTE_Id=d.SITE_Route_Id)) as AlertDtl_MEName_var
                                ,null as AlertDtl_ProOrderDate_date,null as AlertDtl_BillModifiedDate_date,null as AlertDtl_CollctnEnqDate_date
                                ,null as AlertDtl_InwdRecDate_date,null as AlertDtl_EnteredDate_date,null as AlertDtl_CheckedDate_date,null as AlertDtl_OutwrdDate_date,'' as AlertDtl_TestName_var
                                from tbl_Inward as b,tbl_Client as c,tbl_Site as d,tbl_MISDetail as f--,tbl_Material as e
                                where --a.ENQ_Id=b.INWD_ENQ_Id and
                                b.INWD_RecordNo_int=f.MISRecordNo
                                and b.INWD_RecordType_var=f.MISRecType
                                and b.INWD_CL_Id=c.CL_Id
                                and b.INWD_SITE_Id=d.SITE_Id
                                and CONVERT (DATE, f.MISApprovedDt) >=CONVERT (DATE,'" + fromDate + "') and CONVERT (DATE,f.MISApprovedDt) <=CONVERT (DATE,'" + ToDate + "') " +
                                @"and datediff(d,convert(date,f.MISApprovedDt),convert(date,f.MISEnteredDt)) > 5
				                and datediff(d,convert(date,f.MISApprovedDt),convert(date,GETDATE())) > 5
                                order by AlertDtl_RecType_var";
                    }
                    else
                    {
                        str = @"----Data Not enterd in 5 day after material inward approval
				                Select 0 as AlertDtl_EnqNo_int,null as AlertDtl_EnqDate_date,f.MISApprovedDt as AlertDtl_AppDate_date,'' as AlertDtl_BillNo_int,null as AlertDtl_BillDate_date,0 as AlertDtl_BillAmt_dec,
                                '' as AlertDtl_ProNo_var,0 as AlertDtl_ProNo_int,null as AlertDtl_ProDate_date, MISRefNo as AlertDtl_RefNo_var,MISRecordNo as AlertDtl_RecNo_int,
                                c.CL_Name_var as AlertDtl_CL_Name_var,d.SITE_Name_var as AlertDtl_SITE_Name_var,
                                (select MATERIAL_Name_var from tbl_Material e where e.MATERIAL_RecordType_var=b.INWD_RecordType_var) as AlertDtl_RecType_var
				                ,c.CL_Id as AlertDtl_CLId_int,d.SITE_Id as AlertDtl_SiteId_int,6 as AlertDtl_Alert_Id,
                                (select USER_NAME_var from tbl_User x where x.USER_Id IN (select ROUTE_ME_Id from tbl_Route where ROUTE_Id=d.SITE_Route_Id)) as AlertDtl_MEName_var
                                ,null as AlertDtl_ProOrderDate_date,null as AlertDtl_BillModifiedDate_date,null as AlertDtl_CollctnEnqDate_date
                                ,null as AlertDtl_InwdRecDate_date,null as AlertDtl_EnteredDate_date,null as AlertDtl_CheckedDate_date,null as AlertDtl_OutwrdDate_date,'' as AlertDtl_TestName_var
                                from tbl_Inward as b,tbl_Client as c,tbl_Site as d,tbl_MISDetail as f
                                where --a.ENQ_Id=b.INWD_ENQ_Id and --tbl_Enquiry as a,
                                b.INWD_RecordNo_int=f.MISRecordNo
                                and b.INWD_RecordType_var=f.MISRecType
                                and b.INWD_CL_Id=c.CL_Id
                                and b.INWD_SITE_Id=d.SITE_Id
                                and CONVERT (DATE,f.MISApprovedDt) >=CONVERT (DATE,'" + fromDate + "') and CONVERT (DATE,f.MISApprovedDt) <=CONVERT (DATE,'" + ToDate + "') " +
                                @" and f.MISEnteredDt is null 
                                and datediff(d,convert(date,f.MISApprovedDt),convert(date,GETDATE())) > 5
                                order by AlertDtl_RecType_var";

                    }
                }
                else if (Alert_Id == 7)
                {
                    if (flagStatus == 0)
                    {
                        str = @"---Category wise test not completed in X days
                select  0 as AlertDtl_EnqNo_int,null as AlertDtl_EnqDate_date,a.AACINWD_CollectionDate_dt as AlertDtl_CollctnEnqDate_date,'' as AlertDtl_BillNo_int,null as AlertDtl_BillDate_date,0 as AlertDtl_BillAmt_dec,
                '' as AlertDtl_ProNo_var,0 as AlertDtl_ProNo_int,null as AlertDtl_ProDate_date,AACINWD_ReferenceNo_var as AlertDtl_RefNo_var,AACINWD_RecordNo_int as AlertDtl_RecNo_int
                ,CL_Name_var as AlertDtl_CL_Name_var,SITE_Name_var as AlertDtl_SITE_Name_var,'AAC' as AlertDtl_RecType_var,AACINWD_CL_Id  as AlertDtl_CLId_int,AACINWD_SITE_Id  as AlertDtl_SiteId_int,7  as AlertDtl_Alert_Id,
                (select USER_NAME_var from tbl_User x where x.USER_Id IN (select ROUTE_ME_Id from tbl_Route where ROUTE_Id=c.SITE_Route_Id)) as AlertDtl_MEName_var			
                 ,null as AlertDtl_ProOrderDate_date,null as AlertDtl_BillModifiedDate_date,null as AlertDtl_InwdRecDate_date
                ,null as AlertDtl_AppDate_date,null as AlertDtl_EnteredDate_date,null as AlertDtl_CheckedDate_date,null as AlertDtl_OutwrdDate_date
                ,(select TEST_Name_var from tbl_Test as d where a.AACINWD_TEST_Id=d.TEST_Id ) as  AlertDtl_TestName_var
                from tbl_AAC_Inward  as a,tbl_Client as b,tbl_Site as c
                where a.AACINWD_CL_Id=b.CL_Id
                and a.AACINWD_SITE_Id=c.SITE_Id
              	and CONVERT (DATE, a.AACINWD_CollectionDate_dt) >=CONVERT (DATE,'" + fromDate + "') and CONVERT (DATE,a.AACINWD_CollectionDate_dt) <=CONVERT (DATE,'" + ToDate + "') " +
                @"and datediff(d,convert(date,a.AACINWD_CollectionDate_dt),convert(date,a.AACINWD_OutwardDate_dt)) > ((select distinct(TEST_TimeFrame) from tbl_Test as d where Test_RecType_var='AAC' and TEST_TimeFrame is NOT Null)+1)
				and datediff(d,convert(date,a.AACINWD_CollectionDate_dt),convert(date,GETDATE())) > ((select distinct(TEST_TimeFrame) from tbl_Test as d where Test_RecType_var='AAC' and TEST_TimeFrame is NOT Null)+1)

                Union

                select  0 as AlertDtl_EnqNo_int,null as AlertDtl_EnqDate_date,a.AGGTINWD_CollectionDate_dt as AlertDtl_CollctnEnqDate_date,'' as AlertDtl_BillNo_int,null as AlertDtl_BillDate_date,0 as AlertDtl_BillAmt_dec,
                '' as AlertDtl_ProNo_var,0 as AlertDtl_ProNo_int,null as AlertDtl_ProDate_date,AGGTINWD_ReferenceNo_var as AlertDtl_RefNo_var,AGGTINWD_RecordNo_int as AlertDtl_RecNo_int,
                CL_Name_var as AlertDtl_CL_Name_var,SITE_Name_var as AlertDtl_SITE_Name_var,'AGGT' as AlertDtl_RecType_var,AGGTINWD_CL_Id  as AlertDtl_CLId_int,AGGTINWD_SITE_Id  as AlertDtl_SiteId_int,7  as AlertDtl_Alert_Id,
                (select USER_NAME_var from tbl_User x where x.USER_Id IN (select ROUTE_ME_Id from tbl_Route where ROUTE_Id=c.SITE_Route_Id)) as AlertDtl_MEName_var			
                  ,null as AlertDtl_ProOrderDate_date,null as AlertDtl_BillModifiedDate_date,null as AlertDtl_InwdRecDate_date
                ,null as AlertDtl_AppDate_date,null as AlertDtl_EnteredDate_date,null as AlertDtl_CheckedDate_date,null as AlertDtl_OutwrdDate_date,
                (select TEST_Name_var from tbl_Test as d where e.AGGTTEST_TEST_Id=d.TEST_Id ) as AlertDtl_TestName_var
               from tbl_Aggregate_Inward  as a,tbl_Aggregate_Test as e,tbl_Client as b,tbl_Site as c
                where a.AGGTINWD_CL_Id=b.CL_Id
                and a.AGGTINWD_SITE_Id=c.SITE_Id
                and a.AGGTINWD_ReferenceNo_var=e.AGGTTEST_ReferenceNo_var
                and CONVERT (DATE, a.AGGTINWD_CollectionDate_dt) >=CONVERT (DATE,'" + fromDate + "') and CONVERT (DATE,a.AGGTINWD_CollectionDate_dt) <=CONVERT (DATE,'" + ToDate + "') " +
                @"and datediff(d,convert(date,a.AGGTINWD_CollectionDate_dt),convert(date,a.AGGTINWD_OutwardDate_dt)) > ((select distinct(TEST_TimeFrame) from tbl_Test as d where e.AGGTTEST_TEST_Id=d.TEST_Id and  Test_RecType_var='AGGT' and TEST_TimeFrame is NOT Null)+1)
				and datediff(d,convert(date,a.AGGTINWD_CollectionDate_dt),convert(date,GETDATE())) > ((select distinct(TEST_TimeFrame) from tbl_Test as d where e.AGGTTEST_TEST_Id=d.TEST_Id  and TEST_TimeFrame is NOT Null)+1)

                Union

                 select  0 as AlertDtl_EnqNo_int,null as AlertDtl_EnqDate_date,a.GTINW_CollectionDate_dt as AlertDtl_CollctnEnqDate_date,'' as AlertDtl_BillNo_int,null as AlertDtl_BillDate_date,0 as AlertDtl_BillAmt_dec,
                '' as AlertDtl_ProNo_var,0 as AlertDtl_ProNo_int,null as AlertDtl_ProDate_date,GTINW_RefNo_var as AlertDtl_RefNo_var,GTINW_RecordNo_int as AlertDtl_RecNo_int,
                CL_Name_var as AlertDtl_CL_Name_var,SITE_Name_var as AlertDtl_SITE_Name_var,'GT' as AlertDtl_RecType_var,GTINW_CL_Id  as AlertDtl_CLId_int,GTINW_SITE_Id  as AlertDtl_SiteId_int,7  as AlertDtl_Alert_Id,
                (select USER_NAME_var from tbl_User x where x.USER_Id IN (select ROUTE_ME_Id from tbl_Route where ROUTE_Id=c.SITE_Route_Id)) as AlertDtl_MEName_var			
                  ,null as AlertDtl_ProOrderDate_date,null as AlertDtl_BillModifiedDate_date,null as AlertDtl_InwdRecDate_date
                ,null as AlertDtl_AppDate_date,null as AlertDtl_EnteredDate_date,null as AlertDtl_CheckedDate_date,null as AlertDtl_OutwrdDate_date
                ,(select TEST_Name_var from tbl_Test as d where e.GTTEST_Srno_tint=d.TEST_Id ) as AlertDtl_TestName_var
               from tbl_GT_Inward  as a,tbl_GT_Test as e,tbl_Client as b,tbl_Site as c
                where a.GTINW_CL_Id=b.CL_Id
                and a.GTINW_SITE_Id=c.SITE_Id
                and a.GTINW_RefNo_var=e.GTTEST_RefNo_var
                and CONVERT (DATE, a.GTINW_CollectionDate_dt) >=CONVERT (DATE,'" + fromDate + "') and CONVERT (DATE,a.GTINW_CollectionDate_dt) <=CONVERT (DATE,'" + ToDate + "') " +
                @"and datediff(d,convert(date,a.GTINW_CollectionDate_dt),convert(date,a.GTINW_OutwardDate_dt)) > ((Select distinct(TEST_TimeFrame) from tbl_Test as d where Test_RecType_var='GT' and TEST_TimeFrame is NOT Null)+1)
				and datediff(d,convert(date,a.GTINW_CollectionDate_dt),convert(date,GETDATE())) > ((Select distinct(TEST_TimeFrame) from tbl_Test as d where Test_RecType_var='GT' and TEST_TimeFrame is NOT Null)+1)

                Union

                select  0 as AlertDtl_EnqNo_int,null as AlertDtl_EnqDate_date,a.CCHINWD_CollectionDate_dt as AlertDtl_CollctnEnqDate_date,'' as AlertDtl_BillNo_int,null as AlertDtl_BillDate_date,0 as AlertDtl_BillAmt_dec,
                '' as AlertDtl_ProNo_var,0 as AlertDtl_ProNo_int,null as AlertDtl_ProDate_date,CCHINWD_ReferenceNo_var as AlertDtl_RefNo_var,CCHINWD_RecordNo_int as AlertDtl_RecNo_int,
                CL_Name_var as AlertDtl_CL_Name_var,SITE_Name_var as AlertDtl_SITE_Name_var,'CCH' as AlertDtl_RecType_var,CCHINWD_CL_Id  as AlertDtl_CLId_int,CCHINWD_SITE_Id  as AlertDtl_SiteId_int,7  as AlertDtl_Alert_Id,
                (select USER_NAME_var from tbl_User x where x.USER_Id IN (select ROUTE_ME_Id from tbl_Route where ROUTE_Id=c.SITE_Route_Id)) as AlertDtl_MEName_var			
                  ,null as AlertDtl_ProOrderDate_date,null as AlertDtl_BillModifiedDate_date,null as AlertDtl_InwdRecDate_date
                ,null as AlertDtl_AppDate_date,null as AlertDtl_EnteredDate_date,null as AlertDtl_CheckedDate_date,null as AlertDtl_OutwrdDate_date,
                (select TEST_Name_var from tbl_Test as d where e.CCHTEST_TEST_Id=d.TEST_Id ) as AlertDtl_TestName_var
               from tbl_CementChemical_Inward  as a,tbl_CementChemical_Test as e,tbl_Client as b,tbl_Site as c 
                where a.CCHINWD_CL_Id=b.CL_Id
                and a.CCHINWD_SITE_Id=c.SITE_Id
                and a.CCHINWD_ReferenceNo_var=e.CCHTEST_ReferenceNo_var
                 and CONVERT (DATE, a.CCHINWD_CollectionDate_dt) >=CONVERT (DATE,'" + fromDate + "') and CONVERT (DATE,a.CCHINWD_CollectionDate_dt) <=CONVERT (DATE,'" + ToDate + "') " +
                @"and datediff(d,convert(date,a.CCHINWD_CollectionDate_dt),convert(date,a.CCHINWD_OutwardDate_dt)) > ((Select distinct(TEST_TimeFrame) from tbl_Test as d where Test_RecType_var='CCH' and TEST_TimeFrame is NOT Null)+1)
				and datediff(d,convert(date,a.CCHINWD_CollectionDate_dt),convert(date,GETDATE())) > ((Select distinct(TEST_TimeFrame) from tbl_Test as d where Test_RecType_var='CCH' and TEST_TimeFrame is NOT Null)+1)

                Union

                select  0 as AlertDtl_EnqNo_int,null as AlertDtl_EnqDate_date,a.BTINWD_CollectionDate_dt as AlertDtl_CollctnEnqDate_date,'' as AlertDtl_BillNo_int,null as AlertDtl_BillDate_date,0 as AlertDtl_BillAmt_dec,
                '' as AlertDtl_ProNo_var,0 as AlertDtl_ProNo_int,null as AlertDtl_ProDate_date,BTINWD_ReferenceNo_var as AlertDtl_RefNo_var,BTINWD_RecordNo_int as AlertDtl_RecNo_int,
                CL_Name_var as AlertDtl_CL_Name_var,SITE_Name_var as AlertDtl_SITE_Name_var,'BT-' as AlertDtl_RecType_var,BTINWD_CL_Id  as AlertDtl_CLId_int,BTINWD_SITE_Id  as AlertDtl_SiteId_int,7  as AlertDtl_Alert_Id,
                (select USER_NAME_var from tbl_User x where x.USER_Id IN (select ROUTE_ME_Id from tbl_Route where ROUTE_Id=c.SITE_Route_Id)) as AlertDtl_MEName_var			
                  ,null as AlertDtl_ProOrderDate_date,null as AlertDtl_BillModifiedDate_date,null as AlertDtl_InwdRecDate_date
                ,null as AlertDtl_AppDate_date,null as AlertDtl_EnteredDate_date,null as AlertDtl_CheckedDate_date,null as AlertDtl_OutwrdDate_date,
               (select TEST_Name_var from tbl_Test as d where e.BTTEST_TEST_Id=d.TEST_Id ) as AlertDtl_TestName_var
                from tbl_Brick_Inward  as a,tbl_Brick_Test as e,tbl_Client as b,tbl_Site as c 
                where a.BTINWD_CL_Id=b.CL_Id
                and a.BTINWD_SITE_Id=c.SITE_Id
                and a.BTINWD_ReferenceNo_var=e.BTTEST_ReferenceNo_var
                and CONVERT (DATE, a.BTINWD_CollectionDate_dt) >=CONVERT (DATE,'" + fromDate + "') and CONVERT (DATE,a.BTINWD_CollectionDate_dt) <=CONVERT (DATE,'" + ToDate + "') " +
                @"and datediff(d,convert(date,a.BTINWD_CollectionDate_dt),convert(date,a.BTINWD_OutwardDate_dt))>((select TEST_TimeFrame from tbl_Test as d where e.BTTEST_TEST_Id=d.TEST_Id and d.TEST_TimeFrame is not null)+1)
				and datediff(d,convert(date,a.BTINWD_CollectionDate_dt),convert(date,GETDATE())) > ((select TEST_TimeFrame from tbl_Test as d where e.BTTEST_TEST_Id=d.TEST_Id and d.TEST_TimeFrame is not null)+1)

                Union

                select  0 as AlertDtl_EnqNo_int,null as AlertDtl_EnqDate_date,a.CEMTINWD_CollectionDate_dt as AlertDtl_CollctnEnqDate_date,'' as AlertDtl_BillNo_int,null as AlertDtl_BillDate_date,0 as AlertDtl_BillAmt_dec,
                '' as AlertDtl_ProNo_var,0 as AlertDtl_ProNo_int,null as AlertDtl_ProDate_date,CEMTINWD_ReferenceNo_var as AlertDtl_RefNo_var,CEMTINWD_RecordNo_int as AlertDtl_RecNo_int,CL_Name_var as AlertDtl_CL_Name_var,SITE_Name_var as AlertDtl_SITE_Name_var,
                'CEMT' as AlertDtl_RecType_var,CEMTINWD_CL_Id  as AlertDtl_CLId_int,CEMTINWD_SITE_Id  as AlertDtl_SiteId_int,7  as AlertDtl_Alert_Id,
                (select USER_NAME_var from tbl_User x where x.USER_Id IN (select ROUTE_ME_Id from tbl_Route where ROUTE_Id=c.SITE_Route_Id)) as AlertDtl_MEName_var			
                  ,null as AlertDtl_ProOrderDate_date,null as AlertDtl_BillModifiedDate_date,null as AlertDtl_InwdRecDate_date
                ,null as AlertDtl_AppDate_date,null as AlertDtl_EnteredDate_date,null as AlertDtl_CheckedDate_date,null as AlertDtl_OutwrdDate_date,
               (select TEST_Name_var from tbl_Test as d where e.CEMTTEST_TEST_Id=d.TEST_Id ) as AlertDtl_TestName_var
                from tbl_Cement_Inward  as a,tbl_Cement_Test as e,tbl_Client as b,tbl_Site as c 
                where a.CEMTINWD_CL_Id=b.CL_Id
                and a.CEMTINWD_SITE_Id=c.SITE_Id
                and a.CEMTINWD_ReferenceNo_var=e.CEMTTEST_ReferenceNo_var
                and CONVERT (DATE, a.CEMTINWD_CollectionDate_dt) >=CONVERT (DATE,'" + fromDate + "') and CONVERT (DATE,a.CEMTINWD_CollectionDate_dt) <=CONVERT (DATE,'" + ToDate + "') " +
                @"and datediff(d,convert(date,a.CEMTINWD_CollectionDate_dt),convert(date,a.CEMTINWD_OutwardDate_dt))>((select TEST_TimeFrame from tbl_Test as d where e.CEMTTEST_TEST_Id=d.TEST_Id and d.TEST_TimeFrame is not null)+1)
				and datediff(d,convert(date,a.CEMTINWD_CollectionDate_dt),convert(date,GETDATE())) >((select TEST_TimeFrame from tbl_Test as d where e.CEMTTEST_TEST_Id=d.TEST_Id and d.TEST_TimeFrame is not null)+1)

                Union

                select  0 as AlertDtl_EnqNo_int,null as AlertDtl_EnqDate_date,a.CRINWD_CollectionDate_dt as AlertDtl_CollctnEnqDate_date,'' as AlertDtl_BillNo_int,null as AlertDtl_BillDate_date,0 as AlertDtl_BillAmt_dec,
                '' as AlertDtl_ProNo_var,0 as AlertDtl_ProNo_int,null as AlertDtl_ProDate_date,CRINWD_ReferenceNo_var as AlertDtl_RefNo_var,CRINWD_RecordNo_int as AlertDtl_RecNo_int,
                CL_Name_var as AlertDtl_CL_Name_var,SITE_Name_var as AlertDtl_SITE_Name_var,'CR' as AlertDtl_RecType_var,CRINWD_CL_Id  as AlertDtl_CLId_int,CRINWD_SITE_Id  as AlertDtl_SiteId_int,7  as AlertDtl_Alert_Id,
                (select USER_NAME_var from tbl_User x where x.USER_Id IN (select ROUTE_ME_Id from tbl_Route where ROUTE_Id=c.SITE_Route_Id)) as AlertDtl_MEName_var			
                  ,null as AlertDtl_ProOrderDate_date,null as AlertDtl_BillModifiedDate_date,null as AlertDtl_InwdRecDate_date
                ,null as AlertDtl_AppDate_date,null as AlertDtl_EnteredDate_date,null as AlertDtl_CheckedDate_date,null as AlertDtl_OutwrdDate_date,
                (select TEST_Name_var from tbl_Test as d where a.CRINWD_TestId_int=d.TEST_Id ) as AlertDtl_TestName_var
                from tbl_Core_Inward  as a,tbl_Client as b,tbl_Site as c 
                where a.CRINWD_CL_Id=b.CL_Id
                and a.CRINWD_SITE_Id=c.SITE_Id
                and CONVERT (DATE, a.CRINWD_CollectionDate_dt) >=CONVERT (DATE,'" + fromDate + "') and CONVERT (DATE,a.CRINWD_CollectionDate_dt) <=CONVERT (DATE,'" + ToDate + "') " +
                @"and datediff(d,convert(date,a.CRINWD_CollectionDate_dt),convert(date,a.CRINWD_OutwardDate_dt))>((Select distinct(TEST_TimeFrame) from tbl_Test as d where Test_RecType_var='CR' and TEST_TimeFrame is NOT Null)+1)
				and datediff(d,convert(date,a.CRINWD_CollectionDate_dt),convert(date,GETDATE())) > ((Select distinct(TEST_TimeFrame) from tbl_Test as d where Test_RecType_var='CR' and TEST_TimeFrame is NOT Null)+1)

                Union

                select  0 as AlertDtl_EnqNo_int,null as AlertDtl_EnqDate_date,a.CoreCutINWD_CollectionDate_dt as AlertDtl_CollctnEnqDate_date,'' as AlertDtl_BillNo_int,null as AlertDtl_BillDate_date,0 as AlertDtl_BillAmt_dec,
                '' as AlertDtl_ProNo_var,0 as AlertDtl_ProNo_int,null as AlertDtl_ProDate_date,CORECUTINWD_ReferenceNo_var as AlertDtl_RefNo_var,CORECUTINWD_RecordNo_int as AlertDtl_RecNo_int,
                CL_Name_var as AlertDtl_CL_Name_var,SITE_Name_var as AlertDtl_SITE_Name_var,'CORECUT' as AlertDtl_RecType_var,CORECUTINWD_CL_Id  as AlertDtl_CLId_int,CORECUTINWD_SITE_Id  as AlertDtl_SiteId_int,7  as AlertDtl_Alert_Id,
                (select USER_NAME_var from tbl_User x where x.USER_Id IN (select ROUTE_ME_Id from tbl_Route where ROUTE_Id=c.SITE_Route_Id)) as AlertDtl_MEName_var			
                  ,null as AlertDtl_ProOrderDate_date,null as AlertDtl_BillModifiedDate_date,null as AlertDtl_InwdRecDate_date
                ,null as AlertDtl_AppDate_date,null as AlertDtl_EnteredDate_date,null as AlertDtl_CheckedDate_date,null as AlertDtl_OutwrdDate_date,
                 (select TEST_Name_var from tbl_Test as d where a.CoreCutINWD_TEST_Id=d.TEST_Id ) as AlertDtl_TestName_var
               from tbl_CoreCutting_Inward  as a,tbl_Client as b,tbl_Site as c 
                where a.CoreCutINWD_CL_Id=b.CL_Id
                and a.CoreCutINWD_SITE_Id=c.SITE_Id
                and CONVERT (DATE, a.CoreCutINWD_CollectionDate_dt) >=CONVERT (DATE,'" + fromDate + "') and CONVERT (DATE,a.CoreCutINWD_CollectionDate_dt) <=CONVERT (DATE,'" + ToDate + "') " +
                @"and datediff(d,convert(date,a.CoreCutINWD_CollectionDate_dt),convert(date,a.CoreCutINWD_OutwardDate_dt))>((Select distinct(TEST_TimeFrame) from tbl_Test as d where Test_RecType_var='CORECUT' and TEST_TimeFrame is NOT Null)+1)
				and datediff(d,convert(date,a.CoreCutINWD_CollectionDate_dt),convert(date,GETDATE())) > ((Select distinct(TEST_TimeFrame) from tbl_Test as d where Test_RecType_var='CORECUT' and TEST_TimeFrame is NOT Null)+1)

                Union

                select  0 as AlertDtl_EnqNo_int,null as AlertDtl_EnqDate_date,a.CTINWD_CollectionDate_dt as AlertDtl_CollctnEnqDate_date,'' as AlertDtl_BillNo_int,null as AlertDtl_BillDate_date,0 as AlertDtl_BillAmt_dec,
                '' as AlertDtl_ProNo_var,0 as AlertDtl_ProNo_int,null as AlertDtl_ProDate_date,CTINWD_ReferenceNo_var as AlertDtl_RefNo_var,CTINWD_RecordNo_int as AlertDtl_RecNo_int,CL_Name_var as AlertDtl_CL_Name_var,SITE_Name_var as AlertDtl_SITE_Name_var,
                'CT' as AlertDtl_RecType_var,CTINWD_CL_Id  as AlertDtl_CLId_int,CTINWD_SITE_Id  as AlertDtl_SiteId_int,7  as AlertDtl_Alert_Id,
                (select USER_NAME_var from tbl_User x where x.USER_Id IN (select ROUTE_ME_Id from tbl_Route where ROUTE_Id=c.SITE_Route_Id)) as AlertDtl_MEName_var			
                  ,null as AlertDtl_ProOrderDate_date,null as AlertDtl_BillModifiedDate_date,null as AlertDtl_InwdRecDate_date
                ,null as AlertDtl_AppDate_date,null as AlertDtl_EnteredDate_date,null as AlertDtl_CheckedDate_date,null as AlertDtl_OutwrdDate_date,
                  (select TEST_Name_var from tbl_Test as d where a.CTINWD_TEST_Id=d.TEST_Id ) as AlertDtl_TestName_var
               from tbl_Cube_Inward  as a,tbl_Client as b,tbl_Site as c 
                where a.CTINWD_CL_Id=b.CL_Id
                and a.CTINWD_SITE_Id=c.SITE_Id
                and CONVERT (DATE, a.CTINWD_CollectionDate_dt) >=CONVERT (DATE,'" + fromDate + "') and CONVERT (DATE,a.CTINWD_CollectionDate_dt) <=CONVERT (DATE,'" + ToDate + "') " +
                @"and datediff(d,convert(date,a.CTINWD_CollectionDate_dt),convert(date,a.CTINWD_OutwardDate_dt))>((Select distinct(TEST_TimeFrame) from tbl_Test as d where Test_RecType_var='CT' and TEST_TimeFrame is NOT Null)+1)
				and datediff(d,convert(date,a.CTINWD_CollectionDate_dt),convert(date,GETDATE())) > ((Select distinct(TEST_TimeFrame) from tbl_Test as d where Test_RecType_var='CT' and TEST_TimeFrame is NOT Null)+1)

                Union

                select  0 as AlertDtl_EnqNo_int,null as AlertDtl_EnqDate_date,a.FlyAshINWD_CollectionDate_dt as AlertDtl_CollctnEnqDate_date,'' as AlertDtl_BillNo_int,null as AlertDtl_BillDate_date,0 as AlertDtl_BillAmt_dec,
                '' as AlertDtl_ProNo_var,0 as AlertDtl_ProNo_int,null as AlertDtl_ProDate_date,FlyAshINWD_ReferenceNo_var as AlertDtl_RefNo_var,FlyAshINWD_RecordNo_int as AlertDtl_RecNo_int,
                CL_Name_var as AlertDtl_CL_Name_var,SITE_Name_var as AlertDtl_SITE_Name_var,'FLYASH' as AlertDtl_RecType_var,FlyAshINWD_CL_Id  as AlertDtl_CLId_int,FlyAshINWD_SITE_Id  as AlertDtl_SiteId_int,7  as AlertDtl_Alert_Id,
                (select USER_NAME_var from tbl_User x where x.USER_Id IN (select ROUTE_ME_Id from tbl_Route where ROUTE_Id=c.SITE_Route_Id)) as AlertDtl_MEName_var			
                  ,null as AlertDtl_ProOrderDate_date,null as AlertDtl_BillModifiedDate_date,null as AlertDtl_InwdRecDate_date
                ,null as AlertDtl_AppDate_date,null as AlertDtl_EnteredDate_date,null as AlertDtl_CheckedDate_date,null as AlertDtl_OutwrdDate_date,
               (select TEST_Name_var from tbl_Test as d where e.FlyAshTEST_TEST_Id=d.TEST_Id ) as AlertDtl_TestName_var
                from tbl_FlyAsh_Inward  as a,tbl_FlyAsh_Test as e,tbl_Client as b,tbl_Site as c 
                where a.FlyAshINWD_CL_Id=b.CL_Id
                and a.FlyAshINWD_SITE_Id=c.SITE_Id
                and a.FlyAshINWD_ReferenceNo_var=e.FLYASHTEST_ReferenceNo_var
                and CONVERT (DATE, a.FlyAshINWD_CollectionDate_dt) >=CONVERT (DATE,'" + fromDate + "') and CONVERT (DATE,a.FlyAshINWD_CollectionDate_dt) <=CONVERT (DATE,'" + ToDate + "') " +
                @"and datediff(d,convert(date,a.FlyAshINWD_CollectionDate_dt),convert(date,a.FlyAshINWD_OutwardDate_dt))>((select TEST_TimeFrame from tbl_Test as d where E.FLYASHTEST_TEST_Id=d.TEST_Id and d.TEST_TimeFrame is not null)+1)
				and datediff(d,convert(date,a.FlyAshINWD_CollectionDate_dt),convert(date,GETDATE())) > ((select TEST_TimeFrame from tbl_Test as d where E.FLYASHTEST_TEST_Id=d.TEST_Id and d.TEST_TimeFrame is not null)+1)

                Union

                 select  0 as AlertDtl_EnqNo_int,null as AlertDtl_EnqDate_date,a.MFINWD_CollectionDate_dt as AlertDtl_CollctnEnqDate_date,'' as AlertDtl_BillNo_int,null as AlertDtl_BillDate_date,0 as AlertDtl_BillAmt_dec,
                '' as AlertDtl_ProNo_var,0 as AlertDtl_ProNo_int,null as AlertDtl_ProDate_date,MFINWD_ReferenceNo_var as AlertDtl_RefNo_var,MFINWD_RecordNo_int as AlertDtl_RecNo_int,
                CL_Name_var as AlertDtl_CL_Name_var,SITE_Name_var as AlertDtl_SITE_Name_var,'MF' as AlertDtl_RecType_var,MFINWD_CL_Id  as AlertDtl_CLId_int,MFINWD_SITE_Id  as AlertDtl_SiteId_int,7  as AlertDtl_Alert_Id,
                 (select USER_NAME_var from tbl_User x where x.USER_Id IN (select ROUTE_ME_Id from tbl_Route where ROUTE_Id=c.SITE_Route_Id)) as AlertDtl_MEName_var			
                  ,null as AlertDtl_ProOrderDate_date,null as AlertDtl_BillModifiedDate_date,null as AlertDtl_InwdRecDate_date
                ,null as AlertDtl_AppDate_date,null as AlertDtl_EnteredDate_date,null as AlertDtl_CheckedDate_date,null as AlertDtl_OutwrdDate_date,
                (select TEST_Name_var from tbl_Test as d where a.MFINWD_TestId_int=d.TEST_Id ) as AlertDtl_TestName_var
              from tbl_MixDesign_Inward  as a,tbl_Client as b,tbl_Site as c 
                where a.MFINWD_CL_Id=b.CL_Id
                and a.MFINWD_SITE_Id=c.SITE_Id
                and CONVERT (DATE, a.MFINWD_CollectionDate_dt) >=CONVERT (DATE,'" + fromDate + "') and CONVERT (DATE,a.MFINWD_CollectionDate_dt) <=CONVERT (DATE,'" + ToDate + "') " +
                @"and datediff(d,convert(date,a.MFINWD_CollectionDate_dt),convert(date,a.MFINWD_OutwardDate_dt))>((select TEST_TimeFrame from tbl_Test as d where a.MFINWD_TestId_int=d.TEST_Id and d.TEST_TimeFrame is not null)+1)
				and datediff(d,convert(date,a.MFINWD_CollectionDate_dt),convert(date,GETDATE())) > ((select TEST_TimeFrame from tbl_Test as d where a.MFINWD_TestId_int=d.TEST_Id and d.TEST_TimeFrame is not null)+1)

                Union

                select  0 as AlertDtl_EnqNo_int,null as AlertDtl_EnqDate_date,a.NDTINWD_CollectionDate_dt as AlertDtl_CollctnEnqDate_date,'' as AlertDtl_BillNo_int,null as AlertDtl_BillDate_date,0 as AlertDtl_BillAmt_dec,
                '' as AlertDtl_ProNo_var,0 as AlertDtl_ProNo_int,null as AlertDtl_ProDate_date,NDTINWD_ReferenceNo_var as AlertDtl_RefNo_var,NDTINWD_RecordNo_int as AlertDtl_RecNo_int,
                CL_Name_var as AlertDtl_CL_Name_var,SITE_Name_var as AlertDtl_SITE_Name_var,'NDT' as AlertDtl_RecType_var,NDTINWD_CL_Id  as AlertDtl_CLId_int,NDTINWD_SITE_Id  as AlertDtl_SiteId_int,7  as AlertDtl_Alert_Id,
               (select USER_NAME_var from tbl_User x where x.USER_Id IN (select ROUTE_ME_Id from tbl_Route where ROUTE_Id=c.SITE_Route_Id)) as AlertDtl_MEName_var			
                  ,null as AlertDtl_ProOrderDate_date,null as AlertDtl_BillModifiedDate_date,null as AlertDtl_InwdRecDate_date
                ,null as AlertDtl_AppDate_date,null as AlertDtl_EnteredDate_date,null as AlertDtl_CheckedDate_date,null as AlertDtl_OutwrdDate_date,
                     (select TEST_Name_var from tbl_Test as d where e.NDTTEST_TEST_Id=d.TEST_Id ) as AlertDtl_TestName_var
				from tbl_NDT_Inward  as a,tbl_NDT_Test as e,tbl_Client as b,tbl_Site as c 
                where a.NDTINWD_CL_Id=b.CL_Id
                and a.NDTINWD_SITE_Id=c.SITE_Id
                and a.NDTINWD_ReferenceNo_var=e.NDTTEST_RefNo_var
                and CONVERT (DATE, a.NDTINWD_CollectionDate_dt) >=CONVERT (DATE,'" + fromDate + "') and CONVERT (DATE,a.NDTINWD_CollectionDate_dt) <=CONVERT (DATE,'" + ToDate + "') " +
                @"and datediff(d,convert(date,a.NDTINWD_CollectionDate_dt),convert(date,a.NDTINWD_OutwardDate_dt))>((Select distinct(TEST_TimeFrame) from tbl_Test as d where Test_RecType_var='NDT' and TEST_TimeFrame is NOT Null)+1)
				and datediff(d,convert(date,a.NDTINWD_CollectionDate_dt),convert(date,GETDATE())) > ((Select distinct(TEST_TimeFrame) from tbl_Test as d where Test_RecType_var='NDT' and TEST_TimeFrame is NOT Null)+1)

                Union

                select  0 as AlertDtl_EnqNo_int,null as AlertDtl_EnqDate_date,a.OTINWD_CollectionDate_dt as AlertDtl_CollctnEnqDate_date,'' as AlertDtl_BillNo_int,null as AlertDtl_BillDate_date,0 as AlertDtl_BillAmt_dec,
                '' as AlertDtl_ProNo_var,0 as AlertDtl_ProNo_int,null as AlertDtl_ProDate_date,OTINWD_ReferenceNo_var as AlertDtl_RefNo_var,OTINWD_RecordNo_int as AlertDtl_RecNo_int,
                CL_Name_var as AlertDtl_CL_Name_var,SITE_Name_var as AlertDtl_SITE_Name_var,'OT' as AlertDtl_RecType_var,OTINWD_CL_Id  as AlertDtl_CLId_int,OTINWD_SITE_Id  as AlertDtl_SiteId_int,7  as AlertDtl_Alert_Id,
                (select USER_NAME_var from tbl_User x where x.USER_Id IN (select ROUTE_ME_Id from tbl_Route where ROUTE_Id=c.SITE_Route_Id)) as AlertDtl_MEName_var			
                  ,null as AlertDtl_ProOrderDate_date,null as AlertDtl_BillModifiedDate_date,null as AlertDtl_InwdRecDate_date
                ,null as AlertDtl_AppDate_date,null as AlertDtl_EnteredDate_date,null as AlertDtl_CheckedDate_date,null as AlertDtl_OutwrdDate_date,
               (select TEST_Name_var from tbl_Test as d where e.OTTEST_TEST_Id=d.TEST_Id ) as AlertDtl_TestName_var
                from tbl_Other_Inward  as a,tbl_Other_Test as e,tbl_Client as b,tbl_Site as c 
                where a.OTINWD_CL_Id=b.CL_Id
                and a.OTINWD_SITE_Id=c.SITE_Id
                and a.OTINWD_ReferenceNo_var=e.OTTEST_ReferenceNo_var
                and CONVERT (DATE, a.OTINWD_CollectionDate_dt) >=CONVERT (DATE,'" + fromDate + "') and CONVERT (DATE,a.OTINWD_CollectionDate_dt) <=CONVERT (DATE,'" + ToDate + "') " +
                @"and datediff(d,convert(date,a.OTINWD_CollectionDate_dt),convert(date,a.OTINWD_OutwardDate_dt))>((Select distinct(TEST_TimeFrame) from tbl_Test as d where Test_RecType_var='OT' and TEST_TimeFrame is NOT Null)+1)
				and datediff(d,convert(date,a.OTINWD_CollectionDate_dt),convert(date,GETDATE())) >((Select distinct(TEST_TimeFrame) from tbl_Test as d where Test_RecType_var='OT' and TEST_TimeFrame is NOT Null)+1)

                Union

              select  0 as AlertDtl_EnqNo_int,null as AlertDtl_EnqDate_date,a.PTINWD_CollectionDate_dt as AlertDtl_CollctnEnqDate_date,'' as AlertDtl_BillNo_int,null as AlertDtl_BillDate_date,0 as AlertDtl_BillAmt_dec,
                '' as AlertDtl_ProNo_var,0 as AlertDtl_ProNo_int,null as AlertDtl_ProDate_date,PTINWD_ReferenceNo_var as AlertDtl_RefNo_var,PTINWD_RecordNo_int as AlertDtl_RecNo_int
                ,CL_Name_var as AlertDtl_CL_Name_var,SITE_Name_var as AlertDtl_SITE_Name_var,'PT' as AlertDtl_RecType_var,PTINWD_CL_Id  as AlertDtl_CLId_int,PTINWD_SITE_Id  as AlertDtl_SiteId_int,7  as AlertDtl_Alert_Id,
                (select USER_NAME_var from tbl_User x where x.USER_Id IN (select ROUTE_ME_Id from tbl_Route where ROUTE_Id=c.SITE_Route_Id)) as AlertDtl_MEName_var			
                  ,null as AlertDtl_ProOrderDate_date,null as AlertDtl_BillModifiedDate_date,null as AlertDtl_InwdRecDate_date
                ,null as AlertDtl_AppDate_date,null as AlertDtl_EnteredDate_date,null as AlertDtl_CheckedDate_date,null as AlertDtl_OutwrdDate_date,
                 (select TEST_Name_var from tbl_Test as d where a.PTINWD_TEST_Id=d.TEST_Id ) as AlertDtl_TestName_var 
               from tbl_Pavement_Inward  as a,tbl_Client as b,tbl_Site as c
                where a.PTINWD_CL_Id=b.CL_Id
                and a.PTINWD_SITE_Id=c.SITE_Id
                and CONVERT (DATE, a.PTINWD_CollectionDate_dt) >=CONVERT (DATE,'" + fromDate + "') and CONVERT (DATE,a.PTINWD_CollectionDate_dt) <=CONVERT (DATE,'" + ToDate + "') " +
                @"and datediff(d,convert(date,a.PTINWD_CollectionDate_dt),convert(date,a.PTINWD_OutwardDate_dt))>((select TEST_TimeFrame from tbl_Test as d where a.PTINWD_TEST_Id=d.TEST_Id and d.TEST_TimeFrame is not null)+1)
				and datediff(d,convert(date,a.PTINWD_CollectionDate_dt),convert(date,GETDATE())) > ((select TEST_TimeFrame from tbl_Test as d where a.PTINWD_TEST_Id=d.TEST_Id and d.TEST_TimeFrame is not null)+1)

                Union

                select  0 as AlertDtl_EnqNo_int,null as AlertDtl_EnqDate_date,a.PILEINWD_CollectionDate_dt as AlertDtl_CollctnEnqDate_date,'' as AlertDtl_BillNo_int,null as AlertDtl_BillDate_date,0 as AlertDtl_BillAmt_dec,
                '' as AlertDtl_ProNo_var,0 as AlertDtl_ProNo_int,null as AlertDtl_ProDate_date,PILEINWD_ReferenceNo_var as AlertDtl_RefNo_var,PILEINWD_RecordNo_int as AlertDtl_RecNo_int,
                CL_Name_var as AlertDtl_CL_Name_var,SITE_Name_var as AlertDtl_SITE_Name_var,'PILE' as AlertDtl_RecType_var,PILEINWD_CL_Id  as AlertDtl_CLId_int,PILEINWD_SITE_Id  as AlertDtl_SiteId_int,7  as AlertDtl_Alert_Id,
                (select USER_NAME_var from tbl_User x where x.USER_Id IN (select ROUTE_ME_Id from tbl_Route where ROUTE_Id=c.SITE_Route_Id)) as AlertDtl_MEName_var			
                  ,null as AlertDtl_ProOrderDate_date,null as AlertDtl_BillModifiedDate_date,null as AlertDtl_InwdRecDate_date
                ,null as AlertDtl_AppDate_date,null as AlertDtl_EnteredDate_date,null as AlertDtl_CheckedDate_date,null as AlertDtl_OutwrdDate_date,
                 (select TEST_Name_var from tbl_Test as d where a.PILEINWD_TEST_Id=d.TEST_Id ) as AlertDtl_TestName_var 
                from tbl_Pile_Inward  as a,tbl_Client as b,tbl_Site as c
                where a.PILEINWD_CL_Id=b.CL_Id
                and a.PILEINWD_SITE_Id=c.SITE_Id
                and CONVERT (DATE, a.PILEINWD_CollectionDate_dt) >=CONVERT (DATE,'" + fromDate + "') and CONVERT (DATE,a.PILEINWD_CollectionDate_dt) <=CONVERT (DATE,'" + ToDate + "') " +
                @"and datediff(d,convert(date,a.PILEINWD_CollectionDate_dt),convert(date,a.PILEINWD_OutwardDate_dt))>((Select distinct(TEST_TimeFrame) from tbl_Test as d where Test_RecType_var='PILE' and TEST_TimeFrame is NOT Null)+1)
				and datediff(d,convert(date,a.PILEINWD_CollectionDate_dt),convert(date,GETDATE())) > ((Select distinct(TEST_TimeFrame) from tbl_Test as d where Test_RecType_var='PILE' and TEST_TimeFrame is NOT Null)+1)

                Union

               select  0 as AlertDtl_EnqNo_int,null as AlertDtl_EnqDate_date,a.SOINWD_CollectionDate_dt as AlertDtl_CollctnEnqDate_date,'' as AlertDtl_BillNo_int,null as AlertDtl_BillDate_date,0 as AlertDtl_BillAmt_dec,
                '' as AlertDtl_ProNo_var,0 as AlertDtl_ProNo_int,null as AlertDtl_ProDate_date,SOINWD_ReferenceNo_var as AlertDtl_RefNo_var,SOINWD_RecordNo_int as AlertDtl_RecNo_int,
                CL_Name_var as AlertDtl_CL_Name_var,SITE_Name_var as AlertDtl_SITE_Name_var,'SO' as AlertDtl_RecType_var,SOINWD_CL_Id  as AlertDtl_CLId_int,SOINWD_SITE_Id  as AlertDtl_SiteId_int,7  as AlertDtl_Alert_Id,
                (select USER_NAME_var from tbl_User x where x.USER_Id IN (select ROUTE_ME_Id from tbl_Route where ROUTE_Id=c.SITE_Route_Id)) as AlertDtl_MEName_var			
                  ,null as AlertDtl_ProOrderDate_date,null as AlertDtl_BillModifiedDate_date,null as AlertDtl_InwdRecDate_date
                ,null as AlertDtl_AppDate_date,null as AlertDtl_EnteredDate_date,null as AlertDtl_CheckedDate_date,null as AlertDtl_OutwrdDate_date,
                (select TEST_Name_var from tbl_Test as d where e.SOTEST_TEST_int=d.TEST_Id ) as AlertDtl_TestName_var
               from tbl_Soil_Inward  as a,tbl_Soil_Test as e,tbl_Client as b,tbl_Site as c
                where a.SOINWD_CL_Id=b.CL_Id
                and a.SOINWD_SITE_Id=c.SITE_Id
                and a.SOINWD_ReferenceNo_var=e.SOTEST_ReferenceNo_var
                and CONVERT (DATE, a.SOINWD_CollectionDate_dt) >=CONVERT (DATE,'" + fromDate + "') and CONVERT (DATE,a.SOINWD_CollectionDate_dt) <=CONVERT (DATE,'" + ToDate + "') " +
                @"and datediff(d,convert(date,a.SOINWD_CollectionDate_dt),convert(date,a.SOINWD_OutwardDate_dt))>((select TEST_TimeFrame from tbl_Test as d where E.SOTEST_TEST_int=d.TEST_Id and d.TEST_TimeFrame is not null)+1)
				and datediff(d,convert(date,a.SOINWD_CollectionDate_dt),convert(date,GETDATE())) > ((select TEST_TimeFrame from tbl_Test as d where E.SOTEST_TEST_int=d.TEST_Id and d.TEST_TimeFrame is not null)+1)

                Union

                 select  0 as AlertDtl_EnqNo_int,null as AlertDtl_EnqDate_date,a.SolidINWD_CollectionDate_dt as AlertDtl_CollctnEnqDate_date,'' as AlertDtl_BillNo_int,null as AlertDtl_BillDate_date,0 as AlertDtl_BillAmt_dec,
                '' as AlertDtl_ProNo_var,0 as AlertDtl_ProNo_int,null as AlertDtl_ProDate_date,SolidINWD_ReferenceNo_var as AlertDtl_RefNo_var,SolidINWD_RecordNo_int as AlertDtl_RecNo_int,
                CL_Name_var as AlertDtl_CL_Name_var,SITE_Name_var as AlertDtl_SITE_Name_var,'SOLID' as AlertDtl_RecType_var,SolidINWD_CL_Id  as AlertDtl_CLId_int,SolidINWD_SITE_Id  as AlertDtl_SiteId_int,7  as AlertDtl_Alert_Id,
                (select USER_NAME_var from tbl_User x where x.USER_Id IN (select ROUTE_ME_Id from tbl_Route where ROUTE_Id=c.SITE_Route_Id)) as AlertDtl_MEName_var			
                  ,null as AlertDtl_ProOrderDate_date,null as AlertDtl_BillModifiedDate_date,null as AlertDtl_InwdRecDate_date
                ,null as AlertDtl_AppDate_date,null as AlertDtl_EnteredDate_date,null as AlertDtl_CheckedDate_date,null as AlertDtl_OutwrdDate_date,
                (select TEST_Name_var from tbl_Test as d where a.SolidINWD_TEST_Id=d.TEST_Id ) as AlertDtl_TestName_var
               from tbl_Solid_Inward  as a,tbl_Client as b,tbl_Site as c
                where a.SolidINWD_CL_Id=b.CL_Id
                and a.SolidINWD_SITE_Id=c.SITE_Id
                and CONVERT (DATE, a.SolidINWD_CollectionDate_dt) >=CONVERT (DATE,'" + fromDate + "') and CONVERT (DATE,a.SolidINWD_CollectionDate_dt) <=CONVERT (DATE,'" + ToDate + "') " +
                @"and datediff(d,convert(date,a.SolidINWD_CollectionDate_dt),convert(date,a.SolidINWD_OutwardDate_dt))>((select TEST_TimeFrame from tbl_Test as d where a.SolidINWD_TEST_Id=d.TEST_Id and d.TEST_TimeFrame is not null)+1)
				and datediff(d,convert(date,a.SolidINWD_CollectionDate_dt),convert(date,GETDATE())) >((select TEST_TimeFrame from tbl_Test as d where a.SolidINWD_TEST_Id=d.TEST_Id and d.TEST_TimeFrame is not null)+1)

                Union

                select  0 as AlertDtl_EnqNo_int,null as AlertDtl_EnqDate_date,a.STINWD_CollectionDate_dt as AlertDtl_CollctnEnqDate_date,'' as AlertDtl_BillNo_int,null as AlertDtl_BillDate_date,0 as AlertDtl_BillAmt_dec,
                '' as AlertDtl_ProNo_var,0 as AlertDtl_ProNo_int,null as AlertDtl_ProDate_date,STINWD_ReferenceNo_var as AlertDtl_RefNo_var,STINWD_RecordNo_int as AlertDtl_RecNo_int
                ,CL_Name_var as AlertDtl_CL_Name_var,SITE_Name_var as AlertDtl_SITE_Name_var,'ST' as AlertDtl_RecType_var,STINWD_CL_Id  as AlertDtl_CLId_int,STINWD_SITE_Id  as AlertDtl_SiteId_int,7  as AlertDtl_Alert_Id,
                (select USER_NAME_var from tbl_User x where x.USER_Id IN (select ROUTE_ME_Id from tbl_Route where ROUTE_Id=c.SITE_Route_Id)) as AlertDtl_MEName_var			
                  ,null as AlertDtl_ProOrderDate_date,null as AlertDtl_BillModifiedDate_date,null as AlertDtl_InwdRecDate_date
                ,null as AlertDtl_AppDate_date,null as AlertDtl_EnteredDate_date,null as AlertDtl_CheckedDate_date,null as AlertDtl_OutwrdDate_date,
                (select TEST_Name_var from tbl_Test as d where e.STTEST_TEST_id=d.TEST_Id ) as AlertDtl_TestName_var
                 from tbl_Steel_Inward  as a,tbl_Steel_Test as e,tbl_Client as b,tbl_Site as c
                where a.STINWD_CL_Id=b.CL_Id
                and a.STINWD_SITE_Id=c.SITE_Id
                and a.STINWD_ReferenceNo_var=e.STTEST_ReferenceNo_var
                and CONVERT (DATE, a.STINWD_CollectionDate_dt) >=CONVERT (DATE,'" + fromDate + "') and CONVERT (DATE,a.STINWD_CollectionDate_dt) <=CONVERT (DATE,'" + ToDate + "') " +
                @"and datediff(d,convert(date,a.STINWD_CollectionDate_dt),convert(date,a.STINWD_OutwardDate_dt))>((Select distinct(TEST_TimeFrame) from tbl_Test as d where Test_RecType_var='ST' and TEST_TimeFrame is NOT Null)+1)
				and datediff(d,convert(date,a.STINWD_CollectionDate_dt),convert(date,GETDATE())) >((Select distinct(TEST_TimeFrame) from tbl_Test as d where Test_RecType_var='ST' and TEST_TimeFrame is NOT Null)+1)

                Union

                 select  0 as AlertDtl_EnqNo_int,null as AlertDtl_EnqDate_date,a.STCINWD_CollectionDate_dt as AlertDtl_CollctnEnqDate_date,'' as AlertDtl_BillNo_int,null as AlertDtl_BillDate_date,0 as AlertDtl_BillAmt_dec,
                '' as AlertDtl_ProNo_var,0 as AlertDtl_ProNo_int,null as AlertDtl_ProDate_date,STCINWD_ReferenceNo_var as AlertDtl_RefNo_var,STCINWD_RecordNo_int as AlertDtl_RecNo_int,
                CL_Name_var as AlertDtl_CL_Name_var,SITE_Name_var as AlertDtl_SITE_Name_var,'STC' as AlertDtl_RecType_var,STCINWD_CL_Id  as AlertDtl_CLId_int,STCINWD_SITE_Id  as AlertDtl_SiteId_int,7  as AlertDtl_Alert_Id,
                 (select USER_NAME_var from tbl_User x where x.USER_Id IN (select ROUTE_ME_Id from tbl_Route where ROUTE_Id=c.SITE_Route_Id)) as AlertDtl_MEName_var			
                  ,null as AlertDtl_ProOrderDate_date,null as AlertDtl_BillModifiedDate_date,null as AlertDtl_InwdRecDate_date
                ,null as AlertDtl_AppDate_date,null as AlertDtl_EnteredDate_date,null as AlertDtl_CheckedDate_date,null as AlertDtl_OutwrdDate_date,
               (select TEST_Name_var from tbl_Test as d where a.STCINWD_TEST_id=d.TEST_Id ) as AlertDtl_TestName_var
               from tbl_SteelChemical_Inward  as a,tbl_Client as b,tbl_Site as c
                where a.STCINWD_CL_Id=b.CL_Id
                and a.STCINWD_SITE_Id=c.SITE_Id
                and CONVERT (DATE, a.STCINWD_CollectionDate_dt) >=CONVERT (DATE,'" + fromDate + "') and CONVERT (DATE,a.STCINWD_CollectionDate_dt) <=CONVERT (DATE,'" + ToDate + "') " +
                @"and datediff(d,convert(date,a.STCINWD_CollectionDate_dt),convert(date,a.STCINWD_OutwardDate_dt))>((Select distinct(TEST_TimeFrame) from tbl_Test as d where Test_RecType_var='STC' and TEST_TimeFrame is NOT Null)+1)
				and datediff(d,convert(date,a.STCINWD_CollectionDate_dt),convert(date,GETDATE())) > ((Select distinct(TEST_TimeFrame) from tbl_Test as d where Test_RecType_var='STC' and TEST_TimeFrame is NOT Null)+1)

                Union

                 select  0 as AlertDtl_EnqNo_int,null as AlertDtl_EnqDate_date,a.TILEINWD_CollectionDate_dt as AlertDtl_CollctnEnqDate_date,'' as AlertDtl_BillNo_int,null as AlertDtl_BillDate_date,0 as AlertDtl_BillAmt_dec,
                '' as AlertDtl_ProNo_var,0 as AlertDtl_ProNo_int,null as AlertDtl_ProDate_date,TILEINWD_ReferenceNo_var as AlertDtl_RefNo_var,TILEINWD_RecordNo_int as AlertDtl_RecNo_int
                ,CL_Name_var as AlertDtl_CL_Name_var,SITE_Name_var as AlertDtl_SITE_Name_var,'TILE' as AlertDtl_RecType_var,TILEINWD_CL_Id  as AlertDtl_CLId_int,TILEINWD_SITE_Id  as AlertDtl_SiteId_int,7  as AlertDtl_Alert_Id,
                (select USER_NAME_var from tbl_User x where x.USER_Id IN (select ROUTE_ME_Id from tbl_Route where ROUTE_Id=c.SITE_Route_Id)) as AlertDtl_MEName_var			
                  ,null as AlertDtl_ProOrderDate_date,null as AlertDtl_BillModifiedDate_date,null as AlertDtl_InwdRecDate_date
                ,null as AlertDtl_AppDate_date,null as AlertDtl_EnteredDate_date,null as AlertDtl_CheckedDate_date,null as AlertDtl_OutwrdDate_date,
                (select TEST_Name_var from tbl_Test as d where a.TileINWD_TEST_id=d.TEST_Id ) as AlertDtl_TestName_var
                from tbl_Tile_Inward  as a,tbl_Client as b,tbl_Site as c
                where a.TileINWD_CL_Id=b.CL_Id
                and a.TileINWD_SITE_Id=c.SITE_Id
               and CONVERT (DATE, a.TILEINWD_CollectionDate_dt) >=CONVERT (DATE,'" + fromDate + "') and CONVERT (DATE,a.TILEINWD_CollectionDate_dt) <=CONVERT (DATE,'" + ToDate + "') " +
                @"and datediff(d,convert(date,a.TILEINWD_CollectionDate_dt),convert(date,a.TILEINWD_OutwardDate_dt))>((Select distinct(TEST_TimeFrame) from tbl_Test as d where Test_RecType_var='TILE' and TEST_TimeFrame is NOT Null)+1)
				and datediff(d,convert(date,a.TILEINWD_CollectionDate_dt),convert(date,GETDATE())) > ((Select distinct(TEST_TimeFrame) from tbl_Test as d where Test_RecType_var='TILE' and TEST_TimeFrame is NOT Null)+1)

                Union


                select  0 as AlertDtl_EnqNo_int,null as AlertDtl_EnqDate_date,a.WTINWD_CollectionDate_dt as AlertDtl_CollctnEnqDate_date,'' as AlertDtl_BillNo_int,null as AlertDtl_BillDate_date,0 as AlertDtl_BillAmt_dec,
                '' as AlertDtl_ProNo_var,0 as AlertDtl_ProNo_int,null as AlertDtl_ProDate_date,WTINWD_ReferenceNo_var as AlertDtl_RefNo_var,WTINWD_RecordNo_int as AlertDtl_RecNo_int
                ,CL_Name_var as AlertDtl_CL_Name_var,SITE_Name_var as AlertDtl_SITE_Name_var,'WT' as AlertDtl_RecType_var,WTINWD_CL_Id  as AlertDtl_CLId_int,WTINWD_SITE_Id  as AlertDtl_SiteId_int,7  as AlertDtl_Alert_Id,
                (select USER_NAME_var from tbl_User x where x.USER_Id IN (select ROUTE_ME_Id from tbl_Route where ROUTE_Id=c.SITE_Route_Id)) as AlertDtl_MEName_var			
                  ,null as AlertDtl_ProOrderDate_date,null as AlertDtl_BillModifiedDate_date,null as AlertDtl_InwdRecDate_date
                ,null as AlertDtl_AppDate_date,null as AlertDtl_EnteredDate_date,null as AlertDtl_CheckedDate_date,null as AlertDtl_OutwrdDate_date,
               (select TEST_Name_var from tbl_Test as d where e.WTTEST_TEST_id=d.TEST_Id ) as AlertDtl_TestName_var
               from tbl_Water_Inward  as a,tbl_Water_Test as e,tbl_Client as b,tbl_Site as c
                where a.WTINWD_CL_Id=b.CL_Id
                and a.WTINWD_SITE_Id=c.SITE_Id
                and a.WTINWD_ReferenceNo_var=e.WTTEST_ReferenceNo_var
                and CONVERT (DATE, a.WTINWD_CollectionDate_dt) >=CONVERT (DATE,'" + fromDate + "') and CONVERT (DATE,a.WTINWD_CollectionDate_dt) <=CONVERT (DATE,'" + ToDate + "') " +
                @"and datediff(d,convert(date,a.WTINWD_CollectionDate_dt),convert(date,a.WTINWD_OutwardDate_dt))>((Select distinct(TEST_TimeFrame) from tbl_Test as d where Test_RecType_var='WT' and TEST_TimeFrame is NOT Null)+1)
				and datediff(d,convert(date,a.WTINWD_CollectionDate_dt),convert(date,GETDATE())) > ((Select distinct(TEST_TimeFrame) from tbl_Test as d where Test_RecType_var='WT' and TEST_TimeFrame is NOT Null)+1)
                                ";
                    }
                    else
                    {
                        str = @"---Category wise test not completed in X days
               select  0 as AlertDtl_EnqNo_int,null as AlertDtl_EnqDate_date,a.AACINWD_CollectionDate_dt as AlertDtl_CollctnEnqDate_date,'' as AlertDtl_BillNo_int,null as AlertDtl_BillDate_date,0 as AlertDtl_BillAmt_dec,
                '' as AlertDtl_ProNo_var,0 as AlertDtl_ProNo_int,null as AlertDtl_ProDate_date,AACINWD_ReferenceNo_var as AlertDtl_RefNo_var,AACINWD_RecordNo_int as AlertDtl_RecNo_int
                ,CL_Name_var as AlertDtl_CL_Name_var,SITE_Name_var as AlertDtl_SITE_Name_var,'AAC' as AlertDtl_RecType_var,AACINWD_CL_Id  as AlertDtl_CLId_int,AACINWD_SITE_Id  as AlertDtl_SiteId_int,7  as AlertDtl_Alert_Id,
                (select USER_NAME_var from tbl_User x where x.USER_Id IN (select ROUTE_ME_Id from tbl_Route where ROUTE_Id=c.SITE_Route_Id)) as AlertDtl_MEName_var			
                 ,null as AlertDtl_ProOrderDate_date,null as AlertDtl_BillModifiedDate_date,null as AlertDtl_InwdRecDate_date
                ,null as AlertDtl_AppDate_date,null as AlertDtl_EnteredDate_date,null as AlertDtl_CheckedDate_date,null as AlertDtl_OutwrdDate_date
                ,(select TEST_Name_var from tbl_Test as d where a.AACINWD_TEST_Id=d.TEST_Id ) as  AlertDtl_TestName_var
                from tbl_AAC_Inward  as a,tbl_Client as b,tbl_Site as c
                where a.AACINWD_CL_Id=b.CL_Id
                and a.AACINWD_SITE_Id=c.SITE_Id
                and a.AACINWD_OutwardDate_dt is null
               and CONVERT (DATE, a.AACINWD_CollectionDate_dt) >=CONVERT (DATE,'" + fromDate + "') and CONVERT (DATE,a.AACINWD_CollectionDate_dt) <=CONVERT (DATE,'" + ToDate + "') " +
                @"and datediff(d,convert(date,a.AACINWD_CollectionDate_dt),convert(date,GETDATE())) > ((select distinct(TEST_TimeFrame) from tbl_Test as d where Test_RecType_var='AAC' and TEST_TimeFrame is NOT Null)+1)

                Union

                 select  0 as AlertDtl_EnqNo_int,null as AlertDtl_EnqDate_date,a.AGGTINWD_CollectionDate_dt as AlertDtl_CollctnEnqDate_date,'' as AlertDtl_BillNo_int,null as AlertDtl_BillDate_date,0 as AlertDtl_BillAmt_dec,
                '' as AlertDtl_ProNo_var,0 as AlertDtl_ProNo_int,null as AlertDtl_ProDate_date,AGGTINWD_ReferenceNo_var as AlertDtl_RefNo_var,AGGTINWD_RecordNo_int as AlertDtl_RecNo_int,
                CL_Name_var as AlertDtl_CL_Name_var,SITE_Name_var as AlertDtl_SITE_Name_var,'AGGT' as AlertDtl_RecType_var,AGGTINWD_CL_Id  as AlertDtl_CLId_int,AGGTINWD_SITE_Id  as AlertDtl_SiteId_int,7  as AlertDtl_Alert_Id,
                (select USER_NAME_var from tbl_User x where x.USER_Id IN (select ROUTE_ME_Id from tbl_Route where ROUTE_Id=c.SITE_Route_Id)) as AlertDtl_MEName_var			
                  ,null as AlertDtl_ProOrderDate_date,null as AlertDtl_BillModifiedDate_date,null as AlertDtl_InwdRecDate_date
                ,null as AlertDtl_AppDate_date,null as AlertDtl_EnteredDate_date,null as AlertDtl_CheckedDate_date,null as AlertDtl_OutwrdDate_date,
                (select TEST_Name_var from tbl_Test as d where e.AGGTTEST_TEST_Id=d.TEST_Id ) as AlertDtl_TestName_var
               from tbl_Aggregate_Inward  as a,tbl_Aggregate_Test as e,tbl_Client as b,tbl_Site as c
                where a.AGGTINWD_CL_Id=b.CL_Id
                and a.AGGTINWD_SITE_Id=c.SITE_Id
                and a.AGGTINWD_ReferenceNo_var=e.AGGTTEST_ReferenceNo_var
                and a.AGGTINWD_OutwardDate_dt is null
                and CONVERT (DATE, a.AGGTINWD_CollectionDate_dt) >=CONVERT (DATE,'" + fromDate + "') and CONVERT (DATE,a.AGGTINWD_CollectionDate_dt) <=CONVERT (DATE,'" + ToDate + "') " +
                @"and datediff(d,convert(date,a.AGGTINWD_CollectionDate_dt),convert(date,GETDATE())) > ((select TEST_TimeFrame from tbl_Test as d where e.AGGTTEST_TEST_Id=d.TEST_Id and d.TEST_TimeFrame is not null)+1)

                Union

               select  0 as AlertDtl_EnqNo_int,null as AlertDtl_EnqDate_date,a.GTINW_CollectionDate_dt as AlertDtl_CollctnEnqDate_date,'' as AlertDtl_BillNo_int,null as AlertDtl_BillDate_date,0 as AlertDtl_BillAmt_dec,
                '' as AlertDtl_ProNo_var,0 as AlertDtl_ProNo_int,null as AlertDtl_ProDate_date,GTINW_RefNo_var as AlertDtl_RefNo_var,GTINW_RecordNo_int as AlertDtl_RecNo_int,
                CL_Name_var as AlertDtl_CL_Name_var,SITE_Name_var as AlertDtl_SITE_Name_var,'GT' as AlertDtl_RecType_var,GTINW_CL_Id  as AlertDtl_CLId_int,GTINW_SITE_Id  as AlertDtl_SiteId_int,7  as AlertDtl_Alert_Id,
                (select USER_NAME_var from tbl_User x where x.USER_Id IN (select ROUTE_ME_Id from tbl_Route where ROUTE_Id=c.SITE_Route_Id)) as AlertDtl_MEName_var			
                  ,null as AlertDtl_ProOrderDate_date,null as AlertDtl_BillModifiedDate_date,null as AlertDtl_InwdRecDate_date
                ,null as AlertDtl_AppDate_date,null as AlertDtl_EnteredDate_date,null as AlertDtl_CheckedDate_date,null as AlertDtl_OutwrdDate_date
                ,(select TEST_Name_var from tbl_Test as d where e.GTTEST_Srno_tint=d.TEST_Id ) as AlertDtl_TestName_var
               from tbl_GT_Inward  as a,tbl_GT_Test as e,tbl_Client as b,tbl_Site as c
                where a.GTINW_CL_Id=b.CL_Id
                and a.GTINW_SITE_Id=c.SITE_Id
                and a.GTINW_RefNo_var=e.GTTEST_RefNo_var
                and a.GTINW_OutwardDate_dt is null
                and CONVERT (DATE, a.GTINW_CollectionDate_dt) >=CONVERT (DATE,'" + fromDate + "') and CONVERT (DATE,a.GTINW_CollectionDate_dt) <=CONVERT (DATE,'" + ToDate + "') " +
                @"and datediff(d,convert(date,a.GTINW_CollectionDate_dt),convert(date,GETDATE())) > ((Select distinct(TEST_TimeFrame) from tbl_Test as d where Test_RecType_var='GT' and TEST_TimeFrame is NOT Null)+1)

                Union

                 select  0 as AlertDtl_EnqNo_int,null as AlertDtl_EnqDate_date,a.CCHINWD_CollectionDate_dt as AlertDtl_CollctnEnqDate_date,'' as AlertDtl_BillNo_int,null as AlertDtl_BillDate_date,0 as AlertDtl_BillAmt_dec,
                '' as AlertDtl_ProNo_var,0 as AlertDtl_ProNo_int,null as AlertDtl_ProDate_date,CCHINWD_ReferenceNo_var as AlertDtl_RefNo_var,CCHINWD_RecordNo_int as AlertDtl_RecNo_int,
                CL_Name_var as AlertDtl_CL_Name_var,SITE_Name_var as AlertDtl_SITE_Name_var,'CCH' as AlertDtl_RecType_var,CCHINWD_CL_Id  as AlertDtl_CLId_int,CCHINWD_SITE_Id  as AlertDtl_SiteId_int,7  as AlertDtl_Alert_Id,
                (select USER_NAME_var from tbl_User x where x.USER_Id IN (select ROUTE_ME_Id from tbl_Route where ROUTE_Id=c.SITE_Route_Id)) as AlertDtl_MEName_var			
                  ,null as AlertDtl_ProOrderDate_date,null as AlertDtl_BillModifiedDate_date,null as AlertDtl_InwdRecDate_date
                ,null as AlertDtl_AppDate_date,null as AlertDtl_EnteredDate_date,null as AlertDtl_CheckedDate_date,null as AlertDtl_OutwrdDate_date,
                (select TEST_Name_var from tbl_Test as d where e.CCHTEST_TEST_Id=d.TEST_Id ) as AlertDtl_TestName_var
               from tbl_CementChemical_Inward  as a,tbl_CementChemical_Test as e,tbl_Client as b,tbl_Site as c 
                where a.CCHINWD_CL_Id=b.CL_Id
                and a.CCHINWD_SITE_Id=c.SITE_Id
                and a.CCHINWD_ReferenceNo_var=e.CCHTEST_ReferenceNo_var
                and a.CCHINWD_OutwardDate_dt is null
                and CONVERT (DATE, a.CCHINWD_CollectionDate_dt) >=CONVERT (DATE,'" + fromDate + "') and CONVERT (DATE,a.CCHINWD_CollectionDate_dt) <=CONVERT (DATE,'" + ToDate + "') " +
                @"and datediff(d,convert(date,a.CCHINWD_CollectionDate_dt),convert(date,GETDATE())) > ((Select distinct(TEST_TimeFrame) from tbl_Test as d where Test_RecType_var='CCH' and TEST_TimeFrame is NOT Null)+1)

                Union

                select  0 as AlertDtl_EnqNo_int,null as AlertDtl_EnqDate_date,a.BTINWD_CollectionDate_dt as AlertDtl_CollctnEnqDate_date,'' as AlertDtl_BillNo_int,null as AlertDtl_BillDate_date,0 as AlertDtl_BillAmt_dec,
                '' as AlertDtl_ProNo_var,0 as AlertDtl_ProNo_int,null as AlertDtl_ProDate_date,BTINWD_ReferenceNo_var as AlertDtl_RefNo_var,BTINWD_RecordNo_int as AlertDtl_RecNo_int,
                CL_Name_var as AlertDtl_CL_Name_var,SITE_Name_var as AlertDtl_SITE_Name_var,'BT-' as AlertDtl_RecType_var,BTINWD_CL_Id  as AlertDtl_CLId_int,BTINWD_SITE_Id  as AlertDtl_SiteId_int,7  as AlertDtl_Alert_Id,
                (select USER_NAME_var from tbl_User x where x.USER_Id IN (select ROUTE_ME_Id from tbl_Route where ROUTE_Id=c.SITE_Route_Id)) as AlertDtl_MEName_var			
                  ,null as AlertDtl_ProOrderDate_date,null as AlertDtl_BillModifiedDate_date,null as AlertDtl_InwdRecDate_date
                ,null as AlertDtl_AppDate_date,null as AlertDtl_EnteredDate_date,null as AlertDtl_CheckedDate_date,null as AlertDtl_OutwrdDate_date,
               (select TEST_Name_var from tbl_Test as d where e.BTTEST_TEST_Id=d.TEST_Id ) as AlertDtl_TestName_var
                from tbl_Brick_Inward  as a,tbl_Brick_Test as e,tbl_Client as b,tbl_Site as c 
                where a.BTINWD_CL_Id=b.CL_Id
                and a.BTINWD_SITE_Id=c.SITE_Id
                and a.BTINWD_ReferenceNo_var=e.BTTEST_ReferenceNo_var
                and a.BTINWD_OutwardDate_dt is null
                and CONVERT (DATE, a.BTINWD_CollectionDate_dt) >=CONVERT (DATE,'" + fromDate + "') and CONVERT (DATE,a.BTINWD_CollectionDate_dt) <=CONVERT (DATE,'" + ToDate + "') " +
                @"and datediff(d,convert(date,a.BTINWD_CollectionDate_dt),convert(date,GETDATE())) > ((select TEST_TimeFrame from tbl_Test as d where e.BTTEST_TEST_Id=d.TEST_Id and d.TEST_TimeFrame is not null)+1)

                Union

                select  0 as AlertDtl_EnqNo_int,null as AlertDtl_EnqDate_date,a.CEMTINWD_CollectionDate_dt as AlertDtl_CollctnEnqDate_date,'' as AlertDtl_BillNo_int,null as AlertDtl_BillDate_date,0 as AlertDtl_BillAmt_dec,
                '' as AlertDtl_ProNo_var,0 as AlertDtl_ProNo_int,null as AlertDtl_ProDate_date,CEMTINWD_ReferenceNo_var as AlertDtl_RefNo_var,CEMTINWD_RecordNo_int as AlertDtl_RecNo_int,CL_Name_var as AlertDtl_CL_Name_var,SITE_Name_var as AlertDtl_SITE_Name_var,
                'CEMT' as AlertDtl_RecType_var,CEMTINWD_CL_Id  as AlertDtl_CLId_int,CEMTINWD_SITE_Id  as AlertDtl_SiteId_int,7  as AlertDtl_Alert_Id,
                (select USER_NAME_var from tbl_User x where x.USER_Id IN (select ROUTE_ME_Id from tbl_Route where ROUTE_Id=c.SITE_Route_Id)) as AlertDtl_MEName_var			
                  ,null as AlertDtl_ProOrderDate_date,null as AlertDtl_BillModifiedDate_date,null as AlertDtl_InwdRecDate_date
                ,null as AlertDtl_AppDate_date,null as AlertDtl_EnteredDate_date,null as AlertDtl_CheckedDate_date,null as AlertDtl_OutwrdDate_date,
               (select TEST_Name_var from tbl_Test as d where e.CEMTTEST_TEST_Id=d.TEST_Id ) as AlertDtl_TestName_var
                from tbl_Cement_Inward  as a,tbl_Cement_Test as e,tbl_Client as b,tbl_Site as c 
                where a.CEMTINWD_CL_Id=b.CL_Id
                and a.CEMTINWD_SITE_Id=c.SITE_Id
                and a.CEMTINWD_ReferenceNo_var=e.CEMTTEST_ReferenceNo_var
                and a.CEMTINWD_OutwardDate_dt is null
                 and CONVERT (DATE, a.CEMTINWD_CollectionDate_dt) >=CONVERT (DATE,'" + fromDate + "') and CONVERT (DATE,a.CEMTINWD_CollectionDate_dt) <=CONVERT (DATE,'" + ToDate + "') " +
                @"and datediff(d,convert(date,a.CEMTINWD_CollectionDate_dt),convert(date,GETDATE())) > ((select TEST_TimeFrame from tbl_Test as d where e.CEMTTEST_TEST_Id=d.TEST_Id and d.TEST_TimeFrame is not null)+1)

                Union

                select  0 as AlertDtl_EnqNo_int,null as AlertDtl_EnqDate_date,a.CRINWD_CollectionDate_dt as AlertDtl_CollctnEnqDate_date,'' as AlertDtl_BillNo_int,null as AlertDtl_BillDate_date,0 as AlertDtl_BillAmt_dec,
                '' as AlertDtl_ProNo_var,0 as AlertDtl_ProNo_int,null as AlertDtl_ProDate_date,CRINWD_ReferenceNo_var as AlertDtl_RefNo_var,CRINWD_RecordNo_int as AlertDtl_RecNo_int,
                CL_Name_var as AlertDtl_CL_Name_var,SITE_Name_var as AlertDtl_SITE_Name_var,'CR' as AlertDtl_RecType_var,CRINWD_CL_Id  as AlertDtl_CLId_int,CRINWD_SITE_Id  as AlertDtl_SiteId_int,7  as AlertDtl_Alert_Id,
                (select USER_NAME_var from tbl_User x where x.USER_Id IN (select ROUTE_ME_Id from tbl_Route where ROUTE_Id=c.SITE_Route_Id)) as AlertDtl_MEName_var			
                  ,null as AlertDtl_ProOrderDate_date,null as AlertDtl_BillModifiedDate_date,null as AlertDtl_InwdRecDate_date
                ,null as AlertDtl_AppDate_date,null as AlertDtl_EnteredDate_date,null as AlertDtl_CheckedDate_date,null as AlertDtl_OutwrdDate_date,
                (select TEST_Name_var from tbl_Test as d where a.CRINWD_TestId_int=d.TEST_Id ) as AlertDtl_TestName_var
                from tbl_Core_Inward  as a,tbl_Client as b,tbl_Site as c 
                where a.CRINWD_CL_Id=b.CL_Id
                and a.CRINWD_SITE_Id=c.SITE_Id
                and a.CRINWD_OutwardDate_dt is null
                and CONVERT (DATE, a.CRINWD_CollectionDate_dt) >=CONVERT (DATE,'" + fromDate + "') and CONVERT (DATE,a.CRINWD_CollectionDate_dt) <=CONVERT (DATE,'" + ToDate + "') " +
                @"and datediff(d,convert(date,a.CRINWD_CollectionDate_dt),convert(date,GETDATE())) > ((Select distinct(TEST_TimeFrame) from tbl_Test as d where Test_RecType_var='CR' and TEST_TimeFrame is NOT Null)+1)

                Union

                select  0 as AlertDtl_EnqNo_int,null as AlertDtl_EnqDate_date,a.CoreCutINWD_CollectionDate_dt as AlertDtl_CollctnEnqDate_date,'' as AlertDtl_BillNo_int,null as AlertDtl_BillDate_date,0 as AlertDtl_BillAmt_dec,
                '' as AlertDtl_ProNo_var,0 as AlertDtl_ProNo_int,null as AlertDtl_ProDate_date,CORECUTINWD_ReferenceNo_var as AlertDtl_RefNo_var,CORECUTINWD_RecordNo_int as AlertDtl_RecNo_int,
                CL_Name_var as AlertDtl_CL_Name_var,SITE_Name_var as AlertDtl_SITE_Name_var,'CORECUT' as AlertDtl_RecType_var,CORECUTINWD_CL_Id  as AlertDtl_CLId_int,CORECUTINWD_SITE_Id  as AlertDtl_SiteId_int,7  as AlertDtl_Alert_Id,
                (select USER_NAME_var from tbl_User x where x.USER_Id IN (select ROUTE_ME_Id from tbl_Route where ROUTE_Id=c.SITE_Route_Id)) as AlertDtl_MEName_var			
                  ,null as AlertDtl_ProOrderDate_date,null as AlertDtl_BillModifiedDate_date,null as AlertDtl_InwdRecDate_date
                ,null as AlertDtl_AppDate_date,null as AlertDtl_EnteredDate_date,null as AlertDtl_CheckedDate_date,null as AlertDtl_OutwrdDate_date,
                 (select TEST_Name_var from tbl_Test as d where a.CoreCutINWD_TEST_Id=d.TEST_Id ) as AlertDtl_TestName_var
                from tbl_CoreCutting_Inward  as a,tbl_Client as b,tbl_Site as c 
                where a.CoreCutINWD_CL_Id=b.CL_Id
                and a.CoreCutINWD_SITE_Id=c.SITE_Id
                and a.CoreCutINWD_OutwardDate_dt is null
                 and CONVERT (DATE, a.CoreCutINWD_CollectionDate_dt) >=CONVERT (DATE,'" + fromDate + "') and CONVERT (DATE,a.CoreCutINWD_CollectionDate_dt) <=CONVERT (DATE,'" + ToDate + "') " +
                @"and datediff(d,convert(date,a.CoreCutINWD_CollectionDate_dt),convert(date,GETDATE())) > ((Select distinct(TEST_TimeFrame) from tbl_Test as d where Test_RecType_var='CORECUT' and TEST_TimeFrame is NOT Null)+1)

                Union

                select  0 as AlertDtl_EnqNo_int,null as AlertDtl_EnqDate_date,a.CTINWD_CollectionDate_dt as AlertDtl_CollctnEnqDate_date,'' as AlertDtl_BillNo_int,null as AlertDtl_BillDate_date,0 as AlertDtl_BillAmt_dec,
                '' as AlertDtl_ProNo_var,0 as AlertDtl_ProNo_int,null as AlertDtl_ProDate_date,CTINWD_ReferenceNo_var as AlertDtl_RefNo_var,CTINWD_RecordNo_int as AlertDtl_RecNo_int,CL_Name_var as AlertDtl_CL_Name_var,SITE_Name_var as AlertDtl_SITE_Name_var,
                'CT' as AlertDtl_RecType_var,CTINWD_CL_Id  as AlertDtl_CLId_int,CTINWD_SITE_Id  as AlertDtl_SiteId_int,7  as AlertDtl_Alert_Id,
                (select USER_NAME_var from tbl_User x where x.USER_Id IN (select ROUTE_ME_Id from tbl_Route where ROUTE_Id=c.SITE_Route_Id)) as AlertDtl_MEName_var			
                  ,null as AlertDtl_ProOrderDate_date,null as AlertDtl_BillModifiedDate_date,null as AlertDtl_InwdRecDate_date
                ,null as AlertDtl_AppDate_date,null as AlertDtl_EnteredDate_date,null as AlertDtl_CheckedDate_date,null as AlertDtl_OutwrdDate_date,
                  (select TEST_Name_var from tbl_Test as d where a.CTINWD_TEST_Id=d.TEST_Id ) as AlertDtl_TestName_var
               from tbl_Cube_Inward  as a,tbl_Client as b,tbl_Site as c 
                where a.CTINWD_CL_Id=b.CL_Id
                and a.CTINWD_SITE_Id=c.SITE_Id
                and a.CTINWD_OutwardDate_dt is null
                and CONVERT (DATE, a.CTINWD_CollectionDate_dt) >=CONVERT (DATE,'" + fromDate + "') and CONVERT (DATE,a.CTINWD_CollectionDate_dt) <=CONVERT (DATE,'" + ToDate + "') " +
                @"and datediff(d,convert(date,a.CTINWD_CollectionDate_dt),convert(date,GETDATE())) > ((Select distinct(TEST_TimeFrame) from tbl_Test as d where Test_RecType_var='CT' and TEST_TimeFrame is NOT Null)+1)

                Union

                select  0 as AlertDtl_EnqNo_int,null as AlertDtl_EnqDate_date,a.FlyAshINWD_CollectionDate_dt as AlertDtl_CollctnEnqDate_date,'' as AlertDtl_BillNo_int,null as AlertDtl_BillDate_date,0 as AlertDtl_BillAmt_dec,
                '' as AlertDtl_ProNo_var,0 as AlertDtl_ProNo_int,null as AlertDtl_ProDate_date,FlyAshINWD_ReferenceNo_var as AlertDtl_RefNo_var,FlyAshINWD_RecordNo_int as AlertDtl_RecNo_int,
                CL_Name_var as AlertDtl_CL_Name_var,SITE_Name_var as AlertDtl_SITE_Name_var,'FLYASH' as AlertDtl_RecType_var,FlyAshINWD_CL_Id  as AlertDtl_CLId_int,FlyAshINWD_SITE_Id  as AlertDtl_SiteId_int,7  as AlertDtl_Alert_Id,
                (select USER_NAME_var from tbl_User x where x.USER_Id IN (select ROUTE_ME_Id from tbl_Route where ROUTE_Id=c.SITE_Route_Id)) as AlertDtl_MEName_var			
                  ,null as AlertDtl_ProOrderDate_date,null as AlertDtl_BillModifiedDate_date,null as AlertDtl_InwdRecDate_date
                ,null as AlertDtl_AppDate_date,null as AlertDtl_EnteredDate_date,null as AlertDtl_CheckedDate_date,null as AlertDtl_OutwrdDate_date,
               (select TEST_Name_var from tbl_Test as d where e.FlyAshTEST_TEST_Id=d.TEST_Id ) as AlertDtl_TestName_var
                from tbl_FlyAsh_Inward  as a,tbl_FlyAsh_Test as e,tbl_Client as b,tbl_Site as c 
                where a.FlyAshINWD_CL_Id=b.CL_Id
                and a.FlyAshINWD_SITE_Id=c.SITE_Id
                and a.FlyAshINWD_ReferenceNo_var=e.FLYASHTEST_ReferenceNo_var
                and a.FlyAshINWD_OutwardDate_dt is null
                and CONVERT (DATE, a.FlyAshINWD_CollectionDate_dt) >=CONVERT (DATE,'" + fromDate + "') and CONVERT (DATE,a.FlyAshINWD_CollectionDate_dt) <=CONVERT (DATE,'" + ToDate + "') " +
                @"and datediff(d,convert(date,a.FlyAshINWD_CollectionDate_dt),convert(date,GETDATE())) > ((select TEST_TimeFrame from tbl_Test as d where E.FLYASHTEST_TEST_Id=d.TEST_Id and d.TEST_TimeFrame is not null)+1)

                Union

                 select  0 as AlertDtl_EnqNo_int,null as AlertDtl_EnqDate_date,a.MFINWD_CollectionDate_dt as AlertDtl_CollctnEnqDate_date,'' as AlertDtl_BillNo_int,null as AlertDtl_BillDate_date,0 as AlertDtl_BillAmt_dec,
                '' as AlertDtl_ProNo_var,0 as AlertDtl_ProNo_int,null as AlertDtl_ProDate_date,MFINWD_ReferenceNo_var as AlertDtl_RefNo_var,MFINWD_RecordNo_int as AlertDtl_RecNo_int,
                CL_Name_var as AlertDtl_CL_Name_var,SITE_Name_var as AlertDtl_SITE_Name_var,'MF' as AlertDtl_RecType_var,MFINWD_CL_Id  as AlertDtl_CLId_int,MFINWD_SITE_Id  as AlertDtl_SiteId_int,7  as AlertDtl_Alert_Id,
                 (select USER_NAME_var from tbl_User x where x.USER_Id IN (select ROUTE_ME_Id from tbl_Route where ROUTE_Id=c.SITE_Route_Id)) as AlertDtl_MEName_var			
                  ,null as AlertDtl_ProOrderDate_date,null as AlertDtl_BillModifiedDate_date,null as AlertDtl_InwdRecDate_date
                ,null as AlertDtl_AppDate_date,null as AlertDtl_EnteredDate_date,null as AlertDtl_CheckedDate_date,null as AlertDtl_OutwrdDate_date,
                (select TEST_Name_var from tbl_Test as d where a.MFINWD_TestId_int=d.TEST_Id ) as AlertDtl_TestName_var
             from tbl_MixDesign_Inward  as a,tbl_Client as b,tbl_Site as c 
                where a.MFINWD_CL_Id=b.CL_Id
                and a.MFINWD_SITE_Id=c.SITE_Id
                and a.MFINWD_OutwardDate_dt is null
                and CONVERT (DATE, a.MFINWD_CollectionDate_dt) >=CONVERT (DATE,'" + fromDate + "') and CONVERT (DATE,a.MFINWD_CollectionDate_dt) <=CONVERT (DATE,'" + ToDate + "') " +
                @"and datediff(d,convert(date,a.MFINWD_CollectionDate_dt),convert(date,GETDATE())) > ((select TEST_TimeFrame from tbl_Test as d where a.MFINWD_TestId_int=d.TEST_Id and d.TEST_TimeFrame is not null)+1)

                Union

                select  0 as AlertDtl_EnqNo_int,null as AlertDtl_EnqDate_date,a.NDTINWD_CollectionDate_dt as AlertDtl_CollctnEnqDate_date,'' as AlertDtl_BillNo_int,null as AlertDtl_BillDate_date,0 as AlertDtl_BillAmt_dec,
                '' as AlertDtl_ProNo_var,0 as AlertDtl_ProNo_int,null as AlertDtl_ProDate_date,NDTINWD_ReferenceNo_var as AlertDtl_RefNo_var,NDTINWD_RecordNo_int as AlertDtl_RecNo_int,
                CL_Name_var as AlertDtl_CL_Name_var,SITE_Name_var as AlertDtl_SITE_Name_var,'NDT' as AlertDtl_RecType_var,NDTINWD_CL_Id  as AlertDtl_CLId_int,NDTINWD_SITE_Id  as AlertDtl_SiteId_int,7  as AlertDtl_Alert_Id,
               (select USER_NAME_var from tbl_User x where x.USER_Id IN (select ROUTE_ME_Id from tbl_Route where ROUTE_Id=c.SITE_Route_Id)) as AlertDtl_MEName_var			
                  ,null as AlertDtl_ProOrderDate_date,null as AlertDtl_BillModifiedDate_date,null as AlertDtl_InwdRecDate_date
                ,null as AlertDtl_AppDate_date,null as AlertDtl_EnteredDate_date,null as AlertDtl_CheckedDate_date,null as AlertDtl_OutwrdDate_date,
                     (select TEST_Name_var from tbl_Test as d where e.NDTTEST_TEST_Id=d.TEST_Id ) as AlertDtl_TestName_var
				from tbl_NDT_Inward  as a,tbl_NDT_Test as e,tbl_Client as b,tbl_Site as c 
                where a.NDTINWD_CL_Id=b.CL_Id
                and a.NDTINWD_SITE_Id=c.SITE_Id
                and a.NDTINWD_ReferenceNo_var=e.NDTTEST_RefNo_var
                and a.NDTINWD_OutwardDate_dt is null
                and CONVERT (DATE, a.NDTINWD_CollectionDate_dt) >=CONVERT (DATE,'" + fromDate + "') and CONVERT (DATE,a.NDTINWD_CollectionDate_dt) <=CONVERT (DATE,'" + ToDate + "') " +
                @"and datediff(d,convert(date,a.NDTINWD_CollectionDate_dt),convert(date,GETDATE())) > ((Select distinct(TEST_TimeFrame) from tbl_Test as d where Test_RecType_var='NDT' and TEST_TimeFrame is NOT Null)+1)

                Union

               select  0 as AlertDtl_EnqNo_int,null as AlertDtl_EnqDate_date,a.OTINWD_CollectionDate_dt as AlertDtl_CollctnEnqDate_date,'' as AlertDtl_BillNo_int,null as AlertDtl_BillDate_date,0 as AlertDtl_BillAmt_dec,
                '' as AlertDtl_ProNo_var,0 as AlertDtl_ProNo_int,null as AlertDtl_ProDate_date,OTINWD_ReferenceNo_var as AlertDtl_RefNo_var,OTINWD_RecordNo_int as AlertDtl_RecNo_int,
                CL_Name_var as AlertDtl_CL_Name_var,SITE_Name_var as AlertDtl_SITE_Name_var,'OT' as AlertDtl_RecType_var,OTINWD_CL_Id  as AlertDtl_CLId_int,OTINWD_SITE_Id  as AlertDtl_SiteId_int,7  as AlertDtl_Alert_Id,
                (select USER_NAME_var from tbl_User x where x.USER_Id IN (select ROUTE_ME_Id from tbl_Route where ROUTE_Id=c.SITE_Route_Id)) as AlertDtl_MEName_var			
                  ,null as AlertDtl_ProOrderDate_date,null as AlertDtl_BillModifiedDate_date,null as AlertDtl_InwdRecDate_date
                ,null as AlertDtl_AppDate_date,null as AlertDtl_EnteredDate_date,null as AlertDtl_CheckedDate_date,null as AlertDtl_OutwrdDate_date,
               (select TEST_Name_var from tbl_Test as d where e.OTTEST_TEST_Id=d.TEST_Id ) as AlertDtl_TestName_var
                from tbl_Other_Inward  as a,tbl_Other_Test as e,tbl_Client as b,tbl_Site as c 
                where a.OTINWD_CL_Id=b.CL_Id
                and a.OTINWD_SITE_Id=c.SITE_Id
                and a.OTINWD_ReferenceNo_var=e.OTTEST_ReferenceNo_var
                and a.OTINWD_OutwardDate_dt is null
                and CONVERT (DATE, a.OTINWD_CollectionDate_dt) >=CONVERT (DATE,'" + fromDate + "') and CONVERT (DATE,a.OTINWD_CollectionDate_dt) <=CONVERT (DATE,'" + ToDate + "') " +
                @"and datediff(d,convert(date,a.OTINWD_CollectionDate_dt),convert(date,GETDATE())) > ((Select distinct(TEST_TimeFrame) from tbl_Test as d where Test_RecType_var='OT' and TEST_TimeFrame is NOT Null)+1)

                Union

               select  0 as AlertDtl_EnqNo_int,null as AlertDtl_EnqDate_date,a.PTINWD_CollectionDate_dt as AlertDtl_CollctnEnqDate_date,'' as AlertDtl_BillNo_int,null as AlertDtl_BillDate_date,0 as AlertDtl_BillAmt_dec,
                '' as AlertDtl_ProNo_var,0 as AlertDtl_ProNo_int,null as AlertDtl_ProDate_date,PTINWD_ReferenceNo_var as AlertDtl_RefNo_var,PTINWD_RecordNo_int as AlertDtl_RecNo_int
                ,CL_Name_var as AlertDtl_CL_Name_var,SITE_Name_var as AlertDtl_SITE_Name_var,'PT' as AlertDtl_RecType_var,PTINWD_CL_Id  as AlertDtl_CLId_int,PTINWD_SITE_Id  as AlertDtl_SiteId_int,7  as AlertDtl_Alert_Id,
                (select USER_NAME_var from tbl_User x where x.USER_Id IN (select ROUTE_ME_Id from tbl_Route where ROUTE_Id=c.SITE_Route_Id)) as AlertDtl_MEName_var			
                  ,null as AlertDtl_ProOrderDate_date,null as AlertDtl_BillModifiedDate_date,null as AlertDtl_InwdRecDate_date
                ,null as AlertDtl_AppDate_date,null as AlertDtl_EnteredDate_date,null as AlertDtl_CheckedDate_date,null as AlertDtl_OutwrdDate_date,
                 (select TEST_Name_var from tbl_Test as d where a.PTINWD_TEST_Id=d.TEST_Id ) as AlertDtl_TestName_var 
               from tbl_Pavement_Inward  as a,tbl_Client as b,tbl_Site as c
                where a.PTINWD_CL_Id=b.CL_Id
                and a.PTINWD_SITE_Id=c.SITE_Id
                and a.PTINWD_OutwardDate_dt is null
                and CONVERT (DATE, a.PTINWD_CollectionDate_dt) >=CONVERT (DATE,'" + fromDate + "') and CONVERT (DATE,a.PTINWD_CollectionDate_dt) <=CONVERT (DATE,'" + ToDate + "') " +
                @"and datediff(d,convert(date,a.PTINWD_CollectionDate_dt),convert(date,GETDATE())) > ((select TEST_TimeFrame from tbl_Test as d where a.PTINWD_TEST_Id=d.TEST_Id and d.TEST_TimeFrame is not null)+1)

                Union

                 select  0 as AlertDtl_EnqNo_int,null as AlertDtl_EnqDate_date,a.PILEINWD_CollectionDate_dt as AlertDtl_CollctnEnqDate_date,'' as AlertDtl_BillNo_int,null as AlertDtl_BillDate_date,0 as AlertDtl_BillAmt_dec,
                '' as AlertDtl_ProNo_var,0 as AlertDtl_ProNo_int,null as AlertDtl_ProDate_date,PILEINWD_ReferenceNo_var as AlertDtl_RefNo_var,PILEINWD_RecordNo_int as AlertDtl_RecNo_int,
                CL_Name_var as AlertDtl_CL_Name_var,SITE_Name_var as AlertDtl_SITE_Name_var,'PILE' as AlertDtl_RecType_var,PILEINWD_CL_Id  as AlertDtl_CLId_int,PILEINWD_SITE_Id  as AlertDtl_SiteId_int,7  as AlertDtl_Alert_Id,
                (select USER_NAME_var from tbl_User x where x.USER_Id IN (select ROUTE_ME_Id from tbl_Route where ROUTE_Id=c.SITE_Route_Id)) as AlertDtl_MEName_var			
                  ,null as AlertDtl_ProOrderDate_date,null as AlertDtl_BillModifiedDate_date,null as AlertDtl_InwdRecDate_date
                ,null as AlertDtl_AppDate_date,null as AlertDtl_EnteredDate_date,null as AlertDtl_CheckedDate_date,null as AlertDtl_OutwrdDate_date,
                 (select TEST_Name_var from tbl_Test as d where a.PILEINWD_TEST_Id=d.TEST_Id ) as AlertDtl_TestName_var 
               from tbl_Pile_Inward  as a,tbl_Client as b,tbl_Site as c
                where a.PILEINWD_CL_Id=b.CL_Id
                and a.PILEINWD_SITE_Id=c.SITE_Id
                and a.PILEINWD_OutwardDate_dt is null
                and CONVERT (DATE, a.PILEINWD_CollectionDate_dt) >=CONVERT (DATE,'" + fromDate + "') and CONVERT (DATE,a.PILEINWD_CollectionDate_dt) <=CONVERT (DATE,'" + ToDate + "') " +
                @"and datediff(d,convert(date,a.PILEINWD_CollectionDate_dt),convert(date,GETDATE())) > ((Select distinct(TEST_TimeFrame) from tbl_Test as d where Test_RecType_var='PILE' and TEST_TimeFrame is NOT Null)+1)

                Union

                select  0 as AlertDtl_EnqNo_int,null as AlertDtl_EnqDate_date,a.SOINWD_CollectionDate_dt as AlertDtl_CollctnEnqDate_date,'' as AlertDtl_BillNo_int,null as AlertDtl_BillDate_date,0 as AlertDtl_BillAmt_dec,
                '' as AlertDtl_ProNo_var,0 as AlertDtl_ProNo_int,null as AlertDtl_ProDate_date,SOINWD_ReferenceNo_var as AlertDtl_RefNo_var,SOINWD_RecordNo_int as AlertDtl_RecNo_int,
                CL_Name_var as AlertDtl_CL_Name_var,SITE_Name_var as AlertDtl_SITE_Name_var,'SO' as AlertDtl_RecType_var,SOINWD_CL_Id  as AlertDtl_CLId_int,SOINWD_SITE_Id  as AlertDtl_SiteId_int,7  as AlertDtl_Alert_Id,
                (select USER_NAME_var from tbl_User x where x.USER_Id IN (select ROUTE_ME_Id from tbl_Route where ROUTE_Id=c.SITE_Route_Id)) as AlertDtl_MEName_var			
                  ,null as AlertDtl_ProOrderDate_date,null as AlertDtl_BillModifiedDate_date,null as AlertDtl_InwdRecDate_date
                ,null as AlertDtl_AppDate_date,null as AlertDtl_EnteredDate_date,null as AlertDtl_CheckedDate_date,null as AlertDtl_OutwrdDate_date,
                (select TEST_Name_var from tbl_Test as d where e.SOTEST_TEST_int=d.TEST_Id ) as AlertDtl_TestName_var
               from tbl_Soil_Inward  as a,tbl_Soil_Test as e,tbl_Client as b,tbl_Site as c
                where a.SOINWD_CL_Id=b.CL_Id
                and a.SOINWD_SITE_Id=c.SITE_Id
                and a.SOINWD_ReferenceNo_var=e.SOTEST_ReferenceNo_var
                and a.SOINWD_OutwardDate_dt is null
               and CONVERT (DATE, a.SOINWD_CollectionDate_dt) >=CONVERT (DATE,'" + fromDate + "') and CONVERT (DATE,a.SOINWD_CollectionDate_dt) <=CONVERT (DATE,'" + ToDate + "') " +
                @"and datediff(d,convert(date,a.SOINWD_CollectionDate_dt),convert(date,GETDATE())) > ((select TEST_TimeFrame from tbl_Test as d where E.SOTEST_TEST_int=d.TEST_Id and d.TEST_TimeFrame is not null)+1)

                Union

               select  0 as AlertDtl_EnqNo_int,null as AlertDtl_EnqDate_date,a.SolidINWD_CollectionDate_dt as AlertDtl_CollctnEnqDate_date,'' as AlertDtl_BillNo_int,null as AlertDtl_BillDate_date,0 as AlertDtl_BillAmt_dec,
                '' as AlertDtl_ProNo_var,0 as AlertDtl_ProNo_int,null as AlertDtl_ProDate_date,SolidINWD_ReferenceNo_var as AlertDtl_RefNo_var,SolidINWD_RecordNo_int as AlertDtl_RecNo_int,
                CL_Name_var as AlertDtl_CL_Name_var,SITE_Name_var as AlertDtl_SITE_Name_var,'SOLID' as AlertDtl_RecType_var,SolidINWD_CL_Id  as AlertDtl_CLId_int,SolidINWD_SITE_Id  as AlertDtl_SiteId_int,7  as AlertDtl_Alert_Id,
                (select USER_NAME_var from tbl_User x where x.USER_Id IN (select ROUTE_ME_Id from tbl_Route where ROUTE_Id=c.SITE_Route_Id)) as AlertDtl_MEName_var			
                  ,null as AlertDtl_ProOrderDate_date,null as AlertDtl_BillModifiedDate_date,null as AlertDtl_InwdRecDate_date
                ,null as AlertDtl_AppDate_date,null as AlertDtl_EnteredDate_date,null as AlertDtl_CheckedDate_date,null as AlertDtl_OutwrdDate_date,
                (select TEST_Name_var from tbl_Test as d where a.SolidINWD_TEST_Id=d.TEST_Id ) as AlertDtl_TestName_var
                from tbl_Solid_Inward  as a,tbl_Client as b,tbl_Site as c
                where a.SolidINWD_CL_Id=b.CL_Id
                and a.SolidINWD_SITE_Id=c.SITE_Id
                and a.SolidINWD_OutwardDate_dt is null
               and CONVERT (DATE, a.SolidINWD_CollectionDate_dt) >=CONVERT (DATE,'" + fromDate + "') and CONVERT (DATE,a.SolidINWD_CollectionDate_dt) <=CONVERT (DATE,'" + ToDate + "') " +
                @"and datediff(d,convert(date,a.SolidINWD_CollectionDate_dt),convert(date,GETDATE())) > ((select TEST_TimeFrame from tbl_Test as d where a.SolidINWD_TEST_Id=d.TEST_Id and d.TEST_TimeFrame is not null)+1)

                Union

                select  0 as AlertDtl_EnqNo_int,null as AlertDtl_EnqDate_date,a.STINWD_CollectionDate_dt as AlertDtl_CollctnEnqDate_date,'' as AlertDtl_BillNo_int,null as AlertDtl_BillDate_date,0 as AlertDtl_BillAmt_dec,
                '' as AlertDtl_ProNo_var,0 as AlertDtl_ProNo_int,null as AlertDtl_ProDate_date,STINWD_ReferenceNo_var as AlertDtl_RefNo_var,STINWD_RecordNo_int as AlertDtl_RecNo_int
                ,CL_Name_var as AlertDtl_CL_Name_var,SITE_Name_var as AlertDtl_SITE_Name_var,'ST' as AlertDtl_RecType_var,STINWD_CL_Id  as AlertDtl_CLId_int,STINWD_SITE_Id  as AlertDtl_SiteId_int,7  as AlertDtl_Alert_Id,
                (select USER_NAME_var from tbl_User x where x.USER_Id IN (select ROUTE_ME_Id from tbl_Route where ROUTE_Id=c.SITE_Route_Id)) as AlertDtl_MEName_var			
                  ,null as AlertDtl_ProOrderDate_date,null as AlertDtl_BillModifiedDate_date,null as AlertDtl_InwdRecDate_date
                ,null as AlertDtl_AppDate_date,null as AlertDtl_EnteredDate_date,null as AlertDtl_CheckedDate_date,null as AlertDtl_OutwrdDate_date,
                (select TEST_Name_var from tbl_Test as d where e.STTEST_TEST_id=d.TEST_Id ) as AlertDtl_TestName_var
                from tbl_Steel_Inward  as a,tbl_Steel_Test as e,tbl_Client as b,tbl_Site as c
                where a.STINWD_CL_Id=b.CL_Id
                and a.STINWD_SITE_Id=c.SITE_Id
                and a.STINWD_ReferenceNo_var=e.STTEST_ReferenceNo_var
                and a.STINWD_OutwardDate_dt is null
                 and CONVERT (DATE, a.STINWD_CollectionDate_dt) >=CONVERT (DATE,'" + fromDate + "') and CONVERT (DATE,a.STINWD_CollectionDate_dt) <=CONVERT (DATE,'" + ToDate + "') " +
                @"and datediff(d,convert(date,a.STINWD_CollectionDate_dt),convert(date,GETDATE())) > ((Select distinct(TEST_TimeFrame) from tbl_Test as d where Test_RecType_var='ST' and TEST_TimeFrame is NOT Null)+1)

                Union

                 select  0 as AlertDtl_EnqNo_int,null as AlertDtl_EnqDate_date,a.STCINWD_CollectionDate_dt as AlertDtl_CollctnEnqDate_date,'' as AlertDtl_BillNo_int,null as AlertDtl_BillDate_date,0 as AlertDtl_BillAmt_dec,
                '' as AlertDtl_ProNo_var,0 as AlertDtl_ProNo_int,null as AlertDtl_ProDate_date,STCINWD_ReferenceNo_var as AlertDtl_RefNo_var,STCINWD_RecordNo_int as AlertDtl_RecNo_int,
                CL_Name_var as AlertDtl_CL_Name_var,SITE_Name_var as AlertDtl_SITE_Name_var,'STC' as AlertDtl_RecType_var,STCINWD_CL_Id  as AlertDtl_CLId_int,STCINWD_SITE_Id  as AlertDtl_SiteId_int,7  as AlertDtl_Alert_Id,
                 (select USER_NAME_var from tbl_User x where x.USER_Id IN (select ROUTE_ME_Id from tbl_Route where ROUTE_Id=c.SITE_Route_Id)) as AlertDtl_MEName_var			
                  ,null as AlertDtl_ProOrderDate_date,null as AlertDtl_BillModifiedDate_date,null as AlertDtl_InwdRecDate_date
                ,null as AlertDtl_AppDate_date,null as AlertDtl_EnteredDate_date,null as AlertDtl_CheckedDate_date,null as AlertDtl_OutwrdDate_date,
               (select TEST_Name_var from tbl_Test as d where a.STCINWD_TEST_id=d.TEST_Id ) as AlertDtl_TestName_var
             from tbl_SteelChemical_Inward  as a,tbl_Client as b,tbl_Site as c
                where a.STCINWD_CL_Id=b.CL_Id
                and a.STCINWD_SITE_Id=c.SITE_Id
                and a.STCINWD_OutwardDate_dt is null
                and CONVERT (DATE, a.STCINWD_CollectionDate_dt) >=CONVERT (DATE,'" + fromDate + "') and CONVERT (DATE,a.STCINWD_CollectionDate_dt) <=CONVERT (DATE,'" + ToDate + "') " +
                @"and datediff(d,convert(date,a.STCINWD_CollectionDate_dt),convert(date,GETDATE())) > ((Select distinct(TEST_TimeFrame) from tbl_Test as d where Test_RecType_var='STC' and TEST_TimeFrame is NOT Null)+1)

                Union

                select  0 as AlertDtl_EnqNo_int,null as AlertDtl_EnqDate_date,a.TILEINWD_CollectionDate_dt as AlertDtl_CollctnEnqDate_date,'' as AlertDtl_BillNo_int,null as AlertDtl_BillDate_date,0 as AlertDtl_BillAmt_dec,
                '' as AlertDtl_ProNo_var,0 as AlertDtl_ProNo_int,null as AlertDtl_ProDate_date,TILEINWD_ReferenceNo_var as AlertDtl_RefNo_var,TILEINWD_RecordNo_int as AlertDtl_RecNo_int
                ,CL_Name_var as AlertDtl_CL_Name_var,SITE_Name_var as AlertDtl_SITE_Name_var,'TILE' as AlertDtl_RecType_var,TILEINWD_CL_Id  as AlertDtl_CLId_int,TILEINWD_SITE_Id  as AlertDtl_SiteId_int,7  as AlertDtl_Alert_Id,
                (select USER_NAME_var from tbl_User x where x.USER_Id IN (select ROUTE_ME_Id from tbl_Route where ROUTE_Id=c.SITE_Route_Id)) as AlertDtl_MEName_var			
                  ,null as AlertDtl_ProOrderDate_date,null as AlertDtl_BillModifiedDate_date,null as AlertDtl_InwdRecDate_date
                ,null as AlertDtl_AppDate_date,null as AlertDtl_EnteredDate_date,null as AlertDtl_CheckedDate_date,null as AlertDtl_OutwrdDate_date,
                (select TEST_Name_var from tbl_Test as d where a.TileINWD_TEST_id=d.TEST_Id ) as AlertDtl_TestName_var
                from tbl_Tile_Inward  as a,tbl_Client as b,tbl_Site as c
                where a.TileINWD_CL_Id=b.CL_Id
                and a.TileINWD_SITE_Id=c.SITE_Id
                and a.TILEINWD_OutwardDate_dt is null
                and CONVERT (DATE, a.TILEINWD_CollectionDate_dt) >=CONVERT (DATE,'" + fromDate + "') and CONVERT (DATE,a.TILEINWD_CollectionDate_dt) <=CONVERT (DATE,'" + ToDate + "') " +
                @"and datediff(d,convert(date,a.TILEINWD_CollectionDate_dt),convert(date,GETDATE())) > ((Select distinct(TEST_TimeFrame) from tbl_Test as d where Test_RecType_var='TILE' and TEST_TimeFrame is NOT Null)+1)

                Union


                 select  0 as AlertDtl_EnqNo_int,null as AlertDtl_EnqDate_date,a.WTINWD_CollectionDate_dt as AlertDtl_CollctnEnqDate_date,'' as AlertDtl_BillNo_int,null as AlertDtl_BillDate_date,0 as AlertDtl_BillAmt_dec,
                '' as AlertDtl_ProNo_var,0 as AlertDtl_ProNo_int,null as AlertDtl_ProDate_date,WTINWD_ReferenceNo_var as AlertDtl_RefNo_var,WTINWD_RecordNo_int as AlertDtl_RecNo_int
                ,CL_Name_var as AlertDtl_CL_Name_var,SITE_Name_var as AlertDtl_SITE_Name_var,'WT' as AlertDtl_RecType_var,WTINWD_CL_Id  as AlertDtl_CLId_int,WTINWD_SITE_Id  as AlertDtl_SiteId_int,7  as AlertDtl_Alert_Id,
                (select USER_NAME_var from tbl_User x where x.USER_Id IN (select ROUTE_ME_Id from tbl_Route where ROUTE_Id=c.SITE_Route_Id)) as AlertDtl_MEName_var			
                ,null as AlertDtl_ProOrderDate_date,null as AlertDtl_BillModifiedDate_date,null as AlertDtl_InwdRecDate_date
                ,null as AlertDtl_AppDate_date,null as AlertDtl_EnteredDate_date,null as AlertDtl_CheckedDate_date,null as AlertDtl_OutwrdDate_date,
               (select TEST_Name_var from tbl_Test as d where e.WTTEST_TEST_id=d.TEST_Id ) as AlertDtl_TestName_var
               from tbl_Water_Inward  as a,tbl_Water_Test as e,tbl_Client as b,tbl_Site as c
                where a.WTINWD_CL_Id=b.CL_Id
                and a.WTINWD_SITE_Id=c.SITE_Id
                and a.WTINWD_ReferenceNo_var=e.WTTEST_ReferenceNo_var
                and a.WTINWD_OutwardDate_dt is null
               and CONVERT (DATE, a.WTINWD_CollectionDate_dt) >=CONVERT (DATE,'" + fromDate + "') and CONVERT (DATE,a.WTINWD_CollectionDate_dt) <=CONVERT (DATE,'" + ToDate + "') " +
                @"and datediff(d,convert(date,a.WTINWD_CollectionDate_dt),convert(date,GETDATE())) > ((Select distinct(TEST_TimeFrame) from tbl_Test as d where Test_RecType_var='WT' and TEST_TimeFrame is NOT Null)+1)

			
             ";
                    }

                }
                else if (Alert_Id == 8)
                {
                    if (flagStatus == 0)
                    {
                        str = @"----Report not generated in 1 day after test completion
				                Select 0 as AlertDtl_EnqNo_int,null as AlertDtl_EnqDate_date,b.INWD_ReceivedDate_dt as AlertDtl_InwdRecDate_date,'' as AlertDtl_BillNo_int,null as AlertDtl_BillDate_date,0 as AlertDtl_BillAmt_dec,
                                '' as AlertDtl_ProNo_var,0 as AlertDtl_ProNo_int,null as AlertDtl_ProDate_date, MISRefNo as AlertDtl_RefNo_var,MISRecordNo as AlertDtl_RecNo_int,
                                c.CL_Name_var as AlertDtl_CL_Name_var,d.SITE_Name_var as AlertDtl_SITE_Name_var,
                                (select MATERIAL_Name_var from tbl_Material e where e.MATERIAL_RecordType_var=b.INWD_RecordType_var) as AlertDtl_RecType_var
				                ,c.CL_Id as AlertDtl_CLId_int,d.SITE_Id as AlertDtl_SiteId_int,8 as AlertDtl_Alert_Id,
                                (select USER_NAME_var from tbl_User x where x.USER_Id IN (select ROUTE_ME_Id from tbl_Route where ROUTE_Id=d.SITE_Route_Id)) as AlertDtl_MEName_var
                                ,null as AlertDtl_ProOrderDate_date,null as AlertDtl_BillModifiedDate_date,null as AlertDtl_CollctnEnqDate_date
                                ,null as AlertDtl_AppDate_date,null as AlertDtl_EnteredDate_date,null as AlertDtl_CheckedDate_date,null as AlertDtl_OutwrdDate_date,'' as AlertDtl_TestName_var
                                from tbl_Inward as b,tbl_Client as c,tbl_Site as d,tbl_MISDetail as f--,tbl_Material as e
                                where --a.ENQ_Id=b.INWD_ENQ_Id and
                                b.INWD_RecordNo_int=f.MISRecordNo
                                and b.INWD_RecordType_var=f.MISRecType
                                and b.INWD_CL_Id=c.CL_Id
                                and b.INWD_SITE_Id=d.SITE_Id
                                and CONVERT (DATE, b.INWD_ReceivedDate_dt) >=CONVERT (DATE,'" + fromDate + "') and CONVERT (DATE,b.INWD_ReceivedDate_dt) <=CONVERT (DATE,'" + ToDate + "') " +
                                @"and datediff(d,convert(date,b.INWD_ReceivedDate_dt),convert(date,f.MISEnteredDt)) > 1
				                and datediff(d,convert(date,b.INWD_ReceivedDate_dt),convert(date,GETDATE())) > 1
                                order by AlertDtl_RecType_var";
                    }
                    else
                    {
                        str = @"----Report not generated in 1 day after test completion
				                Select 0 as AlertDtl_EnqNo_int,null as AlertDtl_EnqDate_date,b.INWD_ReceivedDate_dt as AlertDtl_InwdRecDate_date,'' as AlertDtl_BillNo_int,null as AlertDtl_BillDate_date,0 as AlertDtl_BillAmt_dec,
                                '' as AlertDtl_ProNo_var,0 as AlertDtl_ProNo_int,null as AlertDtl_ProDate_date, MISRefNo as AlertDtl_RefNo_var,MISRecordNo as AlertDtl_RecNo_int,
                                c.CL_Name_var as AlertDtl_CL_Name_var,d.SITE_Name_var as AlertDtl_SITE_Name_var,
                                (select MATERIAL_Name_var from tbl_Material e where e.MATERIAL_RecordType_var=b.INWD_RecordType_var) as AlertDtl_RecType_var
				                ,c.CL_Id as AlertDtl_CLId_int,d.SITE_Id as AlertDtl_SiteId_int,8 as AlertDtl_Alert_Id,
                                (select USER_NAME_var from tbl_User x where x.USER_Id IN (select ROUTE_ME_Id from tbl_Route where ROUTE_Id=d.SITE_Route_Id)) as AlertDtl_MEName_var
                                ,null as AlertDtl_ProOrderDate_date,null as AlertDtl_BillModifiedDate_date,null as AlertDtl_CollctnEnqDate_date
                                ,null as AlertDtl_AppDate_date,null as AlertDtl_EnteredDate_date,null as AlertDtl_CheckedDate_date,null as AlertDtl_OutwrdDate_date,'' as AlertDtl_TestName_var
                                from tbl_Inward as b,tbl_Client as c,tbl_Site as d,tbl_MISDetail as f
                                where --a.ENQ_Id=b.INWD_ENQ_Id and --tbl_Enquiry as a,
                                b.INWD_RecordNo_int=f.MISRecordNo
                                and b.INWD_RecordType_var=f.MISRecType
                                and b.INWD_CL_Id=c.CL_Id
                                and b.INWD_SITE_Id=d.SITE_Id
                                and CONVERT (DATE, b.INWD_ReceivedDate_dt) >=CONVERT (DATE,'" + fromDate + "') and CONVERT (DATE,b.INWD_ReceivedDate_dt) <=CONVERT (DATE,'" + ToDate + "') " +
                                @" and f.MISEnteredDt is null 
                                and datediff(d,convert(date,b.INWD_ReceivedDate_dt),convert(date,GETDATE())) > 1
                                order by AlertDtl_RecType_var";

                    }
                }
                else if (Alert_Id == 9)
                {
                    if (flagStatus == 0)
                    {
                        str = @"----Report not checked in 1 day after report generation
				                Select  0 as AlertDtl_EnqNo_int,null as AlertDtl_EnqDate_date,f.MISEnteredDt as AlertDtl_EnteredDate_date,'' as AlertDtl_BillNo_int,null as AlertDtl_BillDate_date,0 as AlertDtl_BillAmt_dec,
                                '' as AlertDtl_ProNo_var,0 as AlertDtl_ProNo_int,null as AlertDtl_ProDate_date,f.MISRefNo as AlertDtl_RefNo_var,f.MISRecordNo as AlertDtl_RecNo_int,
                                c.CL_Name_var as AlertDtl_CL_Name_var,d.SITE_Name_var as AlertDtl_SITE_Name_var,
                                (select MATERIAL_Name_var from tbl_Material e where e.MATERIAL_RecordType_var=b.INWD_RecordType_var) as AlertDtl_RecType_var
				                ,c.CL_Id as AlertDtl_CLId_int,d.SITE_Id as AlertDtl_SiteId_int,9 as AlertDtl_Alert_Id,
                                (select USER_NAME_var from tbl_User x where x.USER_Id IN (select ROUTE_ME_Id from tbl_Route where ROUTE_Id=d.SITE_Route_Id)) as AlertDtl_MEName_var
                                ,null as AlertDtl_ProOrderDate_date,null as AlertDtl_BillModifiedDate_date,null as AlertDtl_CollctnEnqDate_date
                                ,null as AlertDtl_AppDate_date,null as AlertDtl_InwdRecDate_date,null as AlertDtl_CheckedDate_date,null as AlertDtl_OutwrdDate_date,'' as AlertDtl_TestName_var
                                from tbl_Inward as b,tbl_Client as c,tbl_Site as d,tbl_MISDetail as f
                                where --a.ENQ_Id=b.INWD_ENQ_Id and
                                b.INWD_RecordNo_int=f.MISRecordNo
                                and b.INWD_RecordType_var=f.MISRecType
                                and b.INWD_CL_Id=c.CL_Id
                                and b.INWD_SITE_Id=d.SITE_Id
                                and CONVERT (DATE, f.MISEnteredDt) >=CONVERT (DATE,'" + fromDate + "') and CONVERT (DATE,f.MISEnteredDt) <=CONVERT (DATE,'" + ToDate + "') " +
                                 @" and datediff(d,convert(date,f.MISEnteredDt),convert(date,f.MISCheckedDt)) > 1
				                and datediff(d,convert(date,f.MISEnteredDt),convert(date,GETDATE())) > 1
                                order by AlertDtl_RecType_var";
                    }
                    else
                    {
                        str = @"----Report not checked in 1 day after report generation
				                Select  0 as AlertDtl_EnqNo_int,null as AlertDtl_EnqDate_date,f.MISEnteredDt as AlertDtl_EnteredDate_date,'' as AlertDtl_BillNo_int,null as AlertDtl_BillDate_date,0 as AlertDtl_BillAmt_dec,
                                '' as AlertDtl_ProNo_var,0 as AlertDtl_ProNo_int,null as AlertDtl_ProDate_date,f.MISRefNo as AlertDtl_RefNo_var,f.MISRecordNo as AlertDtl_RecNo_int,
                                c.CL_Name_var as AlertDtl_CL_Name_var,d.SITE_Name_var as AlertDtl_SITE_Name_var,
                                 (select MATERIAL_Name_var from tbl_Material e where e.MATERIAL_RecordType_var=b.INWD_RecordType_var) as AlertDtl_RecType_var
				                ,c.CL_Id as AlertDtl_CLId_int,d.SITE_Id as AlertDtl_SiteId_int,9 as AlertDtl_Alert_Id,
                                (select USER_NAME_var from tbl_User x where x.USER_Id IN (select ROUTE_ME_Id from tbl_Route where ROUTE_Id=d.SITE_Route_Id)) as AlertDtl_MEName_var
                                ,null as AlertDtl_ProOrderDate_date,null as AlertDtl_BillModifiedDate_date,null as AlertDtl_CollctnEnqDate_date
                                ,null as AlertDtl_AppDate_date,null as AlertDtl_InwdRecDate_date,null as AlertDtl_CheckedDate_date,null as AlertDtl_OutwrdDate_date,'' as AlertDtl_TestName_var
                                from tbl_Inward as b,tbl_Client as c,tbl_Site as d,tbl_MISDetail as f
                                where  b.INWD_RecordNo_int=f.MISRecordNo
                                and b.INWD_RecordType_var=f.MISRecType
                                and b.INWD_CL_Id=c.CL_Id
                                and b.INWD_SITE_Id=d.SITE_Id
                                and CONVERT (DATE, f.MISEnteredDt) >=CONVERT (DATE,'" + fromDate + "') and CONVERT (DATE,f.MISEnteredDt) <=CONVERT (DATE,'" + ToDate + "') " +
                                @" and f.MISCheckedDt is null and datediff(d,convert(date,f.MISEnteredDt),convert(date,GETDATE())) > 1 
                                order by AlertDtl_RecType_var";

                    }
                }
                else if (Alert_Id == 10)
                {
                    if (flagStatus == 0)
                    {
                        str = @"----Report not approved in 1 day after report checked
				                Select  0 as AlertDtl_EnqNo_int,null as AlertDtl_EnqDate_date,f.MISCheckedDt as AlertDtl_CheckedDate_date,'' as AlertDtl_BillNo_int,null as AlertDtl_BillDate_date,0 as AlertDtl_BillAmt_dec,
                                '' as AlertDtl_ProNo_var,0 as AlertDtl_ProNo_int,null as AlertDtl_ProDate_date,f.MISRefNo as AlertDtl_RefNo_var,f.MISRecordNo as AlertDtl_RecNo_int,
                                c.CL_Name_var as AlertDtl_CL_Name_var,d.SITE_Name_var as AlertDtl_SITE_Name_var,
                                 (select MATERIAL_Name_var from tbl_Material e where e.MATERIAL_RecordType_var=b.INWD_RecordType_var) as AlertDtl_RecType_var
				                ,c.CL_Id as AlertDtl_CLId_int,d.SITE_Id as AlertDtl_SiteId_int,10 as AlertDtl_Alert_Id,
                                (select USER_NAME_var from tbl_User x where x.USER_Id IN (select ROUTE_ME_Id from tbl_Route where ROUTE_Id=d.SITE_Route_Id)) as AlertDtl_MEName_var
                                ,null as AlertDtl_ProOrderDate_date,null as AlertDtl_BillModifiedDate_date,null as AlertDtl_CollctnEnqDate_date
                                ,null as AlertDtl_AppDate_date,null as AlertDtl_InwdRecDate_date,null as AlertDtl_EnteredDate_date,null as AlertDtl_OutwrdDate_date,'' as AlertDtl_TestName_var
                                from tbl_Inward as b,tbl_Client as c,tbl_Site as d,tbl_MISDetail as f
                                where --a.ENQ_Id=b.INWD_ENQ_Id and
                                b.INWD_RecordNo_int=f.MISRecordNo
                                and b.INWD_RecordType_var=f.MISRecType
                                and b.INWD_CL_Id=c.CL_Id
                                and b.INWD_SITE_Id=d.SITE_Id
                                and CONVERT (DATE, f.MISCheckedDt) >=CONVERT (DATE,'" + fromDate + "') and CONVERT (DATE,f.MISCheckedDt) <=CONVERT (DATE,'" + ToDate + "') " +
                                @"and datediff(d,convert(date,f.MISCheckedDt),convert(date,f.MISApprovedDt)) > 1
				                and datediff(d,convert(date,f.MISCheckedDt),convert(date,GETDATE())) > 1 
                                order by AlertDtl_RecType_var";
                    }
                    else
                    {
                        str = @"----Report not approved in 1 day after report checked
				                Select  0 as AlertDtl_EnqNo_int,null as AlertDtl_EnqDate_date,f.MISEnteredDt as AlertDtl_EnteredDate_date,'' as AlertDtl_BillNo_int,null as AlertDtl_BillDate_date,0 as AlertDtl_BillAmt_dec,
                                '' as AlertDtl_ProNo_var,0 as AlertDtl_ProNo_int,null as AlertDtl_ProDate_date,f.MISRefNo as AlertDtl_RefNo_var,f.MISRecordNo as AlertDtl_RecNo_int,
                                c.CL_Name_var as AlertDtl_CL_Name_var,d.SITE_Name_var as AlertDtl_SITE_Name_var,
                                 (select MATERIAL_Name_var from tbl_Material e where e.MATERIAL_RecordType_var=b.INWD_RecordType_var) as AlertDtl_RecType_var
				                ,c.CL_Id as AlertDtl_CLId_int,d.SITE_Id as AlertDtl_SiteId_int,10 as AlertDtl_Alert_Id,
                                (select USER_NAME_var from tbl_User x where x.USER_Id IN (select ROUTE_ME_Id from tbl_Route where ROUTE_Id=d.SITE_Route_Id)) as AlertDtl_MEName_var
                                 ,null as AlertDtl_ProOrderDate_date,null as AlertDtl_BillModifiedDate_date,null as AlertDtl_CollctnEnqDate_date
                                ,null as AlertDtl_AppDate_date,null as AlertDtl_InwdRecDate_date,null as AlertDtl_EnteredDate_date,null as AlertDtl_OutwrdDate_date,'' as AlertDtl_TestName_var
                                from tbl_Inward as b,tbl_Client as c,tbl_Site as d,tbl_MISDetail as f
                                where --a.ENQ_Id=b.INWD_ENQ_Id and 
                                b.INWD_RecordNo_int=f.MISRecordNo
                                and b.INWD_RecordType_var=f.MISRecType
                                and b.INWD_CL_Id=c.CL_Id
                                and b.INWD_SITE_Id=d.SITE_Id
                                and CONVERT (DATE, f.MISCheckedDt) >=CONVERT (DATE,'" + fromDate + "') and CONVERT (DATE,f.MISCheckedDt) <=CONVERT (DATE,'" + ToDate + "') " +
                                @" and f.MISApprovedDt is null and datediff(d,convert(date,f.MISCheckedDt),convert(date,GETDATE())) > 1 
                                order by AlertDtl_RecType_var";

                    }
                }
                else if (Alert_Id == 12)
                {
                    if (flagStatus == 0)
                    {
                        str = @"----Reports not printed within 1 day after report approval
				                Select  0 as AlertDtl_EnqNo_int,null as AlertDtl_EnqDate_date,f.MISApprovedDt as AlertDtl_AppDate_date,'' as AlertDtl_BillNo_int,null as AlertDtl_BillDate_date,0 as AlertDtl_BillAmt_dec,
                                '' as AlertDtl_ProNo_var,0 as AlertDtl_ProNo_int,null as AlertDtl_ProDate_date,f.MISRefNo as AlertDtl_RefNo_var,f.MISRecordNo as AlertDtl_RecNo_int,
                                c.CL_Name_var as AlertDtl_CL_Name_var,d.SITE_Name_var as AlertDtl_SITE_Name_var,
                                 (select MATERIAL_Name_var from tbl_Material e where e.MATERIAL_RecordType_var=b.INWD_RecordType_var) as AlertDtl_RecType_var
				                ,c.CL_Id as AlertDtl_CLId_int,d.SITE_Id as AlertDtl_SiteId_int,12 as AlertDtl_Alert_Id,
                                (select USER_NAME_var from tbl_User x where x.USER_Id IN (select ROUTE_ME_Id from tbl_Route where ROUTE_Id=d.SITE_Route_Id)) as AlertDtl_MEName_var
                                ,null as AlertDtl_ProOrderDate_date,null as AlertDtl_BillModifiedDate_date,null as AlertDtl_CollctnEnqDate_date
                                ,null as AlertDtl_InwdRecDate_date,null as AlertDtl_EnteredDate_date,null as AlertDtl_CheckedDate_date,null as AlertDtl_OutwrdDate_date,'' as AlertDtl_TestName_var
                                from tbl_Inward as b,tbl_Client as c,tbl_Site as d,tbl_MISDetail as f
                                where --a.ENQ_Id=b.INWD_ENQ_Id and
                                 b.INWD_RecordNo_int=f.MISRecordNo
                                and b.INWD_RecordType_var=f.MISRecType
                                and b.INWD_CL_Id=c.CL_Id
                                and b.INWD_SITE_Id=d.SITE_Id
                                and CONVERT (DATE, f.MISApprovedDt) >=CONVERT (DATE,'" + fromDate + "') and CONVERT (DATE,f.MISApprovedDt) <=CONVERT (DATE,'" + ToDate + "') " +
                                 @" and datediff(d,convert(date,f.MISApprovedDt),convert(date,f.MISIssueDt)) > 1
				                and datediff(d,convert(date,f.MISApprovedDt),convert(date,GETDATE())) > 1 
                                order by AlertDtl_RecType_var";
                    }
                    else
                    {
                        str = @"----Reports not printed within 1 day after report approval
				                Select  0 as AlertDtl_EnqNo_int,null as AlertDtl_EnqDate_date,f.MISApprovedDt as AlertDtl_AppDate_date,'' as AlertDtl_BillNo_int,null as AlertDtl_BillDate_date,0 as AlertDtl_BillAmt_dec,
                                '' as AlertDtl_ProNo_var,0 as AlertDtl_ProNo_int,null as AlertDtl_ProDate_date,f.MISRefNo as AlertDtl_RefNo_var,f.MISRecordNo as AlertDtl_RecNo_int,
                                c.CL_Name_var as AlertDtl_CL_Name_var,d.SITE_Name_var as AlertDtl_SITE_Name_var,
                                 (select MATERIAL_Name_var from tbl_Material e where e.MATERIAL_RecordType_var=b.INWD_RecordType_var) as AlertDtl_RecType_var
				                ,c.CL_Id as AlertDtl_CLId_int,d.SITE_Id as AlertDtl_SiteId_int,12 as AlertDtl_Alert_Id,
                                (select USER_NAME_var from tbl_User x where x.USER_Id IN (select ROUTE_ME_Id from tbl_Route where ROUTE_Id=d.SITE_Route_Id)) as AlertDtl_MEName_var
                               ,null as AlertDtl_ProOrderDate_date,null as AlertDtl_BillModifiedDate_date,null as AlertDtl_CollctnEnqDate_date
                                ,null as AlertDtl_InwdRecDate_date,null as AlertDtl_EnteredDate_date,null as AlertDtl_CheckedDate_date,null as AlertDtl_OutwrdDate_date,'' as AlertDtl_TestName_var
                                 from tbl_Inward as b,tbl_Client as c,tbl_Site as d,tbl_MISDetail as f
                                where --a.ENQ_Id=b.INWD_ENQ_Id and
                                b.INWD_RecordNo_int=f.MISRecordNo
                                and b.INWD_RecordType_var=f.MISRecType
                                and b.INWD_CL_Id=c.CL_Id
                                and b.INWD_SITE_Id=d.SITE_Id
                                and CONVERT (DATE,f.MISApprovedDt) >=CONVERT (DATE,'" + fromDate + "') and CONVERT (DATE,f.MISApprovedDt) <=CONVERT (DATE,'" + ToDate + "') " +
                                @" and f.MISIssueDt is null 
                                and datediff(d,convert(date,f.MISApprovedDt),convert(date,GETDATE())) > 1
                                order by AlertDtl_RecType_var";

                    }
                }
                else if (Alert_Id == 13)
                {
                    if (flagStatus == 0)
                    {
                        str = @"----Reports not printed within 3 days after report approval
				                Select 0 as AlertDtl_EnqNo_int,null as AlertDtl_EnqDate_date,f.MISApprovedDt as AlertDtl_AppDate_date,'' as AlertDtl_BillNo_int,null as AlertDtl_BillDate_date,0 as AlertDtl_BillAmt_dec,
                                '' as AlertDtl_ProNo_var,0 as AlertDtl_ProNo_int,null as AlertDtl_ProDate_date,f.MISRefNo as AlertDtl_RefNo_var,f.MISRecordNo as AlertDtl_RecNo_int,
                                c.CL_Name_var as AlertDtl_CL_Name_var,d.SITE_Name_var as AlertDtl_SITE_Name_var,
                                 (select MATERIAL_Name_var from tbl_Material e where e.MATERIAL_RecordType_var=b.INWD_RecordType_var) as AlertDtl_RecType_var
				                ,c.CL_Id as AlertDtl_CLId_int,d.SITE_Id as AlertDtl_SiteId_int,13 as AlertDtl_Alert_Id,
                                (select USER_NAME_var from tbl_User x where x.USER_Id IN (select ROUTE_ME_Id from tbl_Route where ROUTE_Id=d.SITE_Route_Id)) as AlertDtl_MEName_var
                                ,null as AlertDtl_ProOrderDate_date,null as AlertDtl_BillModifiedDate_date,null as AlertDtl_CollctnEnqDate_date
                                ,null as AlertDtl_InwdRecDate_date,null as AlertDtl_EnteredDate_date,null as AlertDtl_CheckedDate_date,null as AlertDtl_OutwrdDate_date,'' as AlertDtl_TestName_var
                                from tbl_Inward as b,tbl_Client as c,tbl_Site as d,tbl_MISDetail as f
                                where  b.INWD_RecordNo_int=f.MISRecordNo
                                and b.INWD_RecordType_var=f.MISRecType
                                and b.INWD_CL_Id=c.CL_Id
                                and b.INWD_SITE_Id=d.SITE_Id
                                and CONVERT (DATE, f.MISApprovedDt) >=CONVERT (DATE,'" + fromDate + "') and CONVERT (DATE,f.MISApprovedDt) <=CONVERT (DATE,'" + ToDate + "') " +
                                @" and datediff(d,convert(date,f.MISApprovedDt),convert(date,f.MISIssueDt)) > 3
				                and datediff(d,convert(date,f.MISApprovedDt),convert(date,GETDATE())) > 3
                                order by AlertDtl_RecType_var";
                    }
                    else
                    {
                        str = @"----Reports not printed within 3 days after report approval
				                Select 0 as AlertDtl_EnqNo_int,null as AlertDtl_EnqDate_date,f.MISApprovedDt as AlertDtl_AppDate_date,'' as AlertDtl_BillNo_int,null as AlertDtl_BillDate_date,0 as AlertDtl_BillAmt_dec,
                                '' as AlertDtl_ProNo_var,0 as AlertDtl_ProNo_int,null as AlertDtl_ProDate_date,f.MISRefNo as AlertDtl_RefNo_var,f.MISRecordNo as AlertDtl_RecNo_int,
                                c.CL_Name_var as AlertDtl_CL_Name_var,d.SITE_Name_var as AlertDtl_SITE_Name_var,
                                (select MATERIAL_Name_var from tbl_Material e where e.MATERIAL_RecordType_var=b.INWD_RecordType_var) as AlertDtl_RecType_var
				                ,c.CL_Id as AlertDtl_CLId_int,d.SITE_Id as AlertDtl_SiteId_int,13 as AlertDtl_Alert_Id,
                                (select USER_NAME_var from tbl_User x where x.USER_Id IN (select ROUTE_ME_Id from tbl_Route where ROUTE_Id=d.SITE_Route_Id)) as AlertDtl_MEName_var
                                ,null as AlertDtl_ProOrderDate_date,null as AlertDtl_BillModifiedDate_date,null as AlertDtl_CollctnEnqDate_date
                                ,null as AlertDtl_InwdRecDate_date,null as AlertDtl_EnteredDate_date,null as AlertDtl_CheckedDate_date,null as AlertDtl_OutwrdDate_date,'' as AlertDtl_TestName_var
                                from tbl_Inward as b,tbl_Client as c,tbl_Site as d,tbl_MISDetail as f
                                where  b.INWD_RecordNo_int=f.MISRecordNo
                                and b.INWD_RecordType_var=f.MISRecType
                                and b.INWD_CL_Id=c.CL_Id
                                and b.INWD_SITE_Id=d.SITE_Id
                                and CONVERT (DATE, f.MISApprovedDt) >=CONVERT (DATE,'" + fromDate + "') and CONVERT (DATE,f.MISApprovedDt) <=CONVERT (DATE,'" + ToDate + "') " +
                                @"and f.MISIssueDt is null 
                                and datediff(d,convert(date,f.MISApprovedDt),convert(date,GETDATE())) > 3 
                                order by AlertDtl_RecType_var";

                    }
                }
                else if (Alert_Id == 14)
                {
                    if (flagStatus == 0)
                    {
                        str = @"----Reports not printed for more than 3 days after report approval
				                Select 0 as AlertDtl_EnqNo_int,null as AlertDtl_EnqDate_date,f.MISApprovedDt as AlertDtl_AppDate_date,'' as AlertDtl_BillNo_int,null as AlertDtl_BillDate_date,0 as AlertDtl_BillAmt_dec,
                                '' as AlertDtl_ProNo_var,0 as AlertDtl_ProNo_int,null as AlertDtl_ProDate_date,f.MISRefNo as AlertDtl_RefNo_var,f.MISRecordNo as AlertDtl_RecNo_int,
                                c.CL_Name_var as AlertDtl_CL_Name_var,d.SITE_Name_var as AlertDtl_SITE_Name_var, 
                                (select MATERIAL_Name_var from tbl_Material e where e.MATERIAL_RecordType_var=b.INWD_RecordType_var) as AlertDtl_RecType_var
				                ,c.CL_Id as AlertDtl_CLId_int,d.SITE_Id as AlertDtl_SiteId_int,14 as AlertDtl_Alert_Id,
                                (select USER_NAME_var from tbl_User x where x.USER_Id IN (select ROUTE_ME_Id from tbl_Route where ROUTE_Id=d.SITE_Route_Id)) as AlertDtl_MEName_var
                                ,null as AlertDtl_ProOrderDate_date,null as AlertDtl_BillModifiedDate_date,null as AlertDtl_CollctnEnqDate_date
                                ,null as AlertDtl_InwdRecDate_date,null as AlertDtl_EnteredDate_date,null as AlertDtl_CheckedDate_date,null as AlertDtl_OutwrdDate_date,'' as AlertDtl_TestName_var
                                from tbl_Inward as b,tbl_Client as c,tbl_Site as d,tbl_MISDetail as f
                                where  b.INWD_RecordNo_int=f.MISRecordNo
                                and b.INWD_RecordType_var=f.MISRecType
                                and b.INWD_CL_Id=c.CL_Id
                                and b.INWD_SITE_Id=d.SITE_Id
                                and CONVERT (DATE, f.MISApprovedDt) >=CONVERT (DATE,'" + fromDate + "') and CONVERT (DATE,f.MISApprovedDt) <=CONVERT (DATE,'" + ToDate + "') " +
                                @" and datediff(d,convert(date,f.MISApprovedDt),convert(date,f.MISIssueDt)) > 4
				                and datediff(d,convert(date,f.MISApprovedDt),convert(date,GETDATE())) > 4
                                order by AlertDtl_RecType_var";
                    }
                    else
                    {
                        str = @"----Reports not printed for more than 3 days after report approval
				                Select 0 as AlertDtl_EnqNo_int,null as AlertDtl_EnqDate_date,f.MISApprovedDt as AlertDtl_AppDate_date,'' as AlertDtl_BillNo_int,null as AlertDtl_BillDate_date,0 as AlertDtl_BillAmt_dec,
                                '' as AlertDtl_ProNo_var,0 as AlertDtl_ProNo_int,null as AlertDtl_ProDate_date,f.MISRefNo as AlertDtl_RefNo_var,f.MISRecordNo as AlertDtl_RecNo_int,
                                c.CL_Name_var as AlertDtl_CL_Name_var,d.SITE_Name_var as AlertDtl_SITE_Name_var,
                                (select MATERIAL_Name_var from tbl_Material e where e.MATERIAL_RecordType_var=b.INWD_RecordType_var) as AlertDtl_RecType_var
				                ,c.CL_Id as AlertDtl_CLId_int,d.SITE_Id as AlertDtl_SiteId_int,14 as AlertDtl_Alert_Id,
                                (select USER_NAME_var from tbl_User x where x.USER_Id IN (select ROUTE_ME_Id from tbl_Route where ROUTE_Id=d.SITE_Route_Id)) as AlertDtl_MEName_var
                                ,null as AlertDtl_ProOrderDate_date,null as AlertDtl_BillModifiedDate_date,null as AlertDtl_CollctnEnqDate_date,null as AlertDtl_OutwrdDate_date
                                ,null as AlertDtl_InwdRecDate_date,null as AlertDtl_EnteredDate_date,null as AlertDtl_CheckedDate_date,'' as AlertDtl_TestName_var
                                from tbl_Inward as b,tbl_Client as c,tbl_Site as d,tbl_MISDetail as f
                                where  b.INWD_RecordNo_int=f.MISRecordNo
                                 and b.INWD_RecordType_var=f.MISRecType
                                and b.INWD_CL_Id=c.CL_Id
                                and b.INWD_SITE_Id=d.SITE_Id
                                and CONVERT (DATE, f.MISApprovedDt) >=CONVERT (DATE,'" + fromDate + "') and CONVERT (DATE,f.MISApprovedDt) <=CONVERT (DATE,'" + ToDate + "') " +
                                @" and f.MISIssueDt is null 
                                and datediff(d,convert(date,f.MISApprovedDt),convert(date,GETDATE())) > 4
                                order by AlertDtl_RecType_var";

                    }
                }
                else if (Alert_Id == 15)
                {
                    if (flagStatus == 0)
                    {
                        str = @"---- Bills modified weekly
				                Select 0 as AlertDtl_EnqNo_int,null as AlertDtl_EnqDate_date,a.BILL_Id as AlertDtl_BillNo_int,a.BILL_Date_dt as AlertDtl_BillDate_date,a.BILL_NetAmt_num as AlertDtl_BillAmt_dec,
                                '' as AlertDtl_ProNo_var,0 as AlertDtl_ProNo_int,null as AlertDtl_ProDate_date,'' as AlertDtl_RefNo_var,0 as AlertDtl_RecNo_int,
                                c.CL_Name_var as AlertDtl_CL_Name_var,d.SITE_Name_var as AlertDtl_SITE_Name_var,(select MATERIAL_Name_var from tbl_Material e where e.MATERIAL_RecordType_var=a.BILL_RecordType_var)  as AlertDtl_RecType_var,
                                c.CL_Id as AlertDtl_CLId_int,d.SITE_Id as AlertDtl_SiteId_int,15 as AlertDtl_Alert_Id,
                                (select USER_NAME_var from tbl_User x where x.USER_Id IN (select ROUTE_ME_Id from tbl_Route where ROUTE_Id=d.SITE_Route_Id)) as AlertDtl_MEName_var
                                ,null as AlertDtl_ProOrderDate_date,null as AlertDtl_BillModifiedDate_date,null as AlertDtl_CollctnEnqDate_date
                                ,null as AlertDtl_InwdRecDate_date,null as AlertDtl_EnteredDate_date,null as AlertDtl_CheckedDate_date,null as AlertDtl_AppDate_date,null as AlertDtl_OutwrdDate_date,'' as AlertDtl_TestName_var
                                from tbl_Client as c,tbl_Site as d,tbl_Bill as a
                                where  a.BILL_CL_Id=c.CL_Id 
                                and a.Bill_SITE_Id=d.SITE_Id
                                 and CONVERT (DATE, a.BILL_Date_dt) >=CONVERT (DATE,'" + fromDate + "') and CONVERT (DATE,a.BILL_Date_dt) <=CONVERT (DATE,'" + ToDate + "') " +
                                 @" and datediff(d,convert(date,a.BILL_Date_dt),convert(date,a.BILL_ModifiedOn_dt)) > 7
				                and datediff(d,convert(date,a.BILL_Date_dt),convert(date,GETDATE())) > 7
                              ";
                    }
                    else
                    {
                        str = @"---- Bills modified weekly
				                Select 0 as AlertDtl_EnqNo_int,null as AlertDtl_EnqDate_date,a.BILL_Id as AlertDtl_BillNo_int,a.BILL_Date_dt as AlertDtl_BillDate_date,a.BILL_NetAmt_num as AlertDtl_BillAmt_dec,
                                '' as AlertDtl_ProNo_var,0 as AlertDtl_ProNo_int,null as AlertDtl_ProDate_date,'' as AlertDtl_RefNo_var,0 as AlertDtl_RecNo_int,
                                c.CL_Name_var as AlertDtl_CL_Name_var,d.SITE_Name_var as AlertDtl_SITE_Name_var,(select MATERIAL_Name_var from tbl_Material e where e.MATERIAL_RecordType_var=a.BILL_RecordType_var)  as AlertDtl_RecType_var,
                                c.CL_Id as AlertDtl_CLId_int,d.SITE_Id as AlertDtl_SiteId_int,15 as AlertDtl_Alert_Id,
                                (select USER_NAME_var from tbl_User x where x.USER_Id IN (select ROUTE_ME_Id from tbl_Route where ROUTE_Id=d.SITE_Route_Id)) as AlertDtl_MEName_var
                                ,null as AlertDtl_ProOrderDate_date,null as AlertDtl_BillModifiedDate_date,null as AlertDtl_CollctnEnqDate_date
                                ,null as AlertDtl_InwdRecDate_date,null as AlertDtl_EnteredDate_date,null as AlertDtl_CheckedDate_date,null as AlertDtl_AppDate_date,null as AlertDtl_OutwrdDate_date,'' as AlertDtl_TestName_var
                                from tbl_Client as c,tbl_Site as d,tbl_Bill as a
                                where  a.BILL_CL_Id=c.CL_Id 
                                and a.Bill_SITE_Id=d.SITE_Id
                                and CONVERT (DATE, a.BILL_Date_dt) >=CONVERT (DATE,'" + fromDate + "') and CONVERT (DATE,a.BILL_Date_dt) <=CONVERT (DATE,'" + ToDate + "') " +
                                @" and a.BILL_ModifiedOn_dt is null 
                                and datediff(d,convert(date,a.BILL_Date_dt),convert(date,GETDATE())) > 7
                               ";

                    }
                }
                else if (Alert_Id == 16)
                {
                    if (flagStatus == 0)
                    {
                        str = @"---- Bills modified in more than 2 days after generation
				                Select 0 as AlertDtl_EnqNo_int,null as AlertDtl_EnqDate_date,a.BILL_ModifiedOn_dt as AlertDtl_BillModifiedDate_date,a.BILL_Id as AlertDtl_BillNo_int,a.BILL_Date_dt as AlertDtl_BillDate_date,a.BILL_NetAmt_num as AlertDtl_BillAmt_dec,
                                '' as AlertDtl_ProNo_var,0 as AlertDtl_ProNo_int,null as AlertDtl_ProDate_date,'' as AlertDtl_RefNo_var,0 as AlertDtl_RecNo_int,
                                c.CL_Name_var as AlertDtl_CL_Name_var,d.SITE_Name_var as AlertDtl_SITE_Name_var,(select MATERIAL_Name_var from tbl_Material e where e.MATERIAL_RecordType_var=a.BILL_RecordType_var)  as AlertDtl_RecType_var,
                                c.CL_Id as AlertDtl_CLId_int,d.SITE_Id as AlertDtl_SiteId_int,16 as AlertDtl_Alert_Id,
                                (select USER_NAME_var from tbl_User x where x.USER_Id IN (select ROUTE_ME_Id from tbl_Route where ROUTE_Id=d.SITE_Route_Id)) as AlertDtl_MEName_var
                                ,null as AlertDtl_ProOrderDate_date,null as AlertDtl_BillModifiedDate_date,null as AlertDtl_CollctnEnqDate_date
                                ,null as AlertDtl_InwdRecDate_date,null as AlertDtl_EnteredDate_date,null as AlertDtl_CheckedDate_date,null as AlertDtl_AppDate_date,null as AlertDtl_OutwrdDate_date,'' as AlertDtl_TestName_var
                                from tbl_Client as c,tbl_Site as d,tbl_Bill as a
                                where  a.BILL_CL_Id=c.CL_Id 
                                and a.Bill_SITE_Id=d.SITE_Id
                                 and CONVERT (DATE, a.BILL_Date_dt) >=CONVERT (DATE,'" + fromDate + "') and CONVERT (DATE,a.BILL_Date_dt) <=CONVERT (DATE,'" + ToDate + "') " +
                                 @" and datediff(d,convert(date,a.BILL_Date_dt),convert(date,a.BILL_ModifiedOn_dt)) > 2
				                and datediff(d,convert(date,a.BILL_Date_dt),convert(date,GETDATE())) > 2
                                ";
                    }
                    else
                    {
                        str = @"---- Bills modified in more than 2 days after generation
				                Select 0 as AlertDtl_EnqNo_int,null as AlertDtl_EnqDate_date,a.BILL_Id as AlertDtl_BillNo_int,a.BILL_Date_dt as AlertDtl_BillDate_date,a.BILL_NetAmt_num as AlertDtl_BillAmt_dec,
                                '' as AlertDtl_ProNo_var,0 as AlertDtl_ProNo_int,null as AlertDtl_ProDate_date,'' as AlertDtl_RefNo_var,0 as AlertDtl_RecNo_int,
                                c.CL_Name_var as AlertDtl_CL_Name_var,d.SITE_Name_var as AlertDtl_SITE_Name_var,(select MATERIAL_Name_var from tbl_Material e where e.MATERIAL_RecordType_var=a.BILL_RecordType_var)  as AlertDtl_RecType_var,
                                c.CL_Id as AlertDtl_CLId_int,d.SITE_Id as AlertDtl_SiteId_int,16 as AlertDtl_Alert_Id,
                                (select USER_NAME_var from tbl_User x where x.USER_Id IN (select ROUTE_ME_Id from tbl_Route where ROUTE_Id=d.SITE_Route_Id)) as AlertDtl_MEName_var
                                ,null as AlertDtl_ProOrderDate_date,null as AlertDtl_BillModifiedDate_date,null as AlertDtl_CollctnEnqDate_date
                                ,null as AlertDtl_InwdRecDate_date,null as AlertDtl_EnteredDate_date,null as AlertDtl_CheckedDate_date,null as AlertDtl_AppDate_date,null as AlertDtl_OutwrdDate_date,'' as AlertDtl_TestName_var
                                from tbl_Client as c,tbl_Site as d,tbl_Bill as a
                                where  a.BILL_CL_Id=c.CL_Id 
                                and a.Bill_SITE_Id=d.SITE_Id
                                and CONVERT (DATE, a.BILL_Date_dt) >=CONVERT (DATE,'" + fromDate + "') and CONVERT (DATE,a.BILL_Date_dt) <=CONVERT (DATE,'" + ToDate + "') " +
                                @" and a.BILL_ModifiedOn_dt is null and datediff(d,convert(date,a.BILL_Date_dt),convert(date,GETDATE())) > 2
                               ";

                    }
                }
                else if (Alert_Id == 18)
                {
                    if (flagStatus == 0)
                    {
                        str = @"---- Bills not sent in 3 days after bill generation 
				                Select 0 as AlertDtl_EnqNo_int,null as AlertDtl_EnqDate_date,a.BILL_Id as AlertDtl_BillNo_int,a.BILL_Date_dt as AlertDtl_BillDate_date,a.BILL_NetAmt_num as AlertDtl_BillAmt_dec,
                                '' as AlertDtl_ProNo_var,0 as AlertDtl_ProNo_int,null as AlertDtl_ProDate_date,'' as AlertDtl_RefNo_var,0 as AlertDtl_RecNo_int,
                                c.CL_Name_var as AlertDtl_CL_Name_var,d.SITE_Name_var as AlertDtl_SITE_Name_var,(select MATERIAL_Name_var from tbl_Material e where e.MATERIAL_RecordType_var=a.BILL_RecordType_var)  as AlertDtl_RecType_var,
                                c.CL_Id as AlertDtl_CLId_int,d.SITE_Id as AlertDtl_SiteId_int,18 as AlertDtl_Alert_Id,
                                (select USER_NAME_var from tbl_User x where x.USER_Id IN (select ROUTE_ME_Id from tbl_Route where ROUTE_Id=d.SITE_Route_Id)) as AlertDtl_MEName_var
                                ,null as AlertDtl_ProOrderDate_date,null as AlertDtl_BillModifiedDate_date,null as AlertDtl_CollctnEnqDate_date
                                ,null as AlertDtl_InwdRecDate_date,null as AlertDtl_EnteredDate_date,null as AlertDtl_CheckedDate_date,null as AlertDtl_AppDate_date,null as AlertDtl_OutwrdDate_date,'' as AlertDtl_TestName_var
                                from tbl_Client as c,tbl_Site as d,tbl_Bill as a,tbl_Outward b
                                where  a.BILL_CL_Id=c.CL_Id 
                                and a.BILL_Id = b.OUTW_ReferenceNo_var And b.OUTW_RecordType_var ='DT'
                                and a.Bill_SITE_Id=d.SITE_Id
                                 and CONVERT (DATE, a.BILL_Date_dt) >=CONVERT (DATE,'" + fromDate + "') and CONVERT (DATE,a.BILL_Date_dt) <=CONVERT (DATE,'" + ToDate + "') " +
                                 @" and datediff(d,convert(date,a.BILL_Date_dt),convert(date,b.OUTW_OutwardDate_dt)) > 3
				                and datediff(d,convert(date,a.BILL_Date_dt),convert(date,GETDATE())) > 3
                                ";
                    }
                    else
                    {
                        str = @"----Bills not sent in 3 days after bill generation 
				                Select 0 as AlertDtl_EnqNo_int,null as AlertDtl_EnqDate_date,a.BILL_Id as AlertDtl_BillNo_int,a.BILL_Date_dt as AlertDtl_BillDate_date,a.BILL_NetAmt_num as AlertDtl_BillAmt_dec,
                                '' as AlertDtl_ProNo_var,0 as AlertDtl_ProNo_int,null as AlertDtl_ProDate_date,'' as AlertDtl_RefNo_var,0 as AlertDtl_RecNo_int,
                                c.CL_Name_var as AlertDtl_CL_Name_var,d.SITE_Name_var as AlertDtl_SITE_Name_var,(select MATERIAL_Name_var from tbl_Material e where e.MATERIAL_RecordType_var=a.BILL_RecordType_var)  as AlertDtl_RecType_var,
                                c.CL_Id as AlertDtl_CLId_int,d.SITE_Id as AlertDtl_SiteId_int,18 as AlertDtl_Alert_Id,
                                (select USER_NAME_var from tbl_User x where x.USER_Id IN (select ROUTE_ME_Id from tbl_Route where ROUTE_Id=d.SITE_Route_Id)) as AlertDtl_MEName_var
                                ,null as AlertDtl_ProOrderDate_date,null as AlertDtl_BillModifiedDate_date,null as AlertDtl_CollctnEnqDate_date
                                ,null as AlertDtl_InwdRecDate_date,null as AlertDtl_EnteredDate_date,null as AlertDtl_CheckedDate_date,null as AlertDtl_AppDate_date,null as AlertDtl_OutwrdDate_date,'' as AlertDtl_TestName_var
                                from tbl_Client as c,tbl_Site as d,tbl_Bill as a
                                where  a.BILL_CL_Id=c.CL_Id 
                                and a.Bill_SITE_Id=d.SITE_Id
                                and CONVERT (DATE, a.BILL_Date_dt) >=CONVERT (DATE,'" + fromDate + "') and CONVERT (DATE,a.BILL_Date_dt) <=CONVERT (DATE,'" + ToDate + "') " +
                                @" and a.BILL_Id Not In( select OUTW_ReferenceNo_var from tbl_Outward b where a.BILL_Id = b.OUTW_ReferenceNo_var And b.OUTW_RecordType_var ='DT')
                                and datediff(d,convert(date,a.BILL_Date_dt),convert(date,GETDATE())) > 2
                               ";

                    }
                }
                else if (Alert_Id == 19)
                {
                    if (flagStatus == 0)
                    {
                        str = @"---- Payment not received in 60 days after bill received by the client
                                Select 0 as AlertDtl_EnqNo_int,null as AlertDtl_EnqDate_date,a.BILL_Id as AlertDtl_BillNo_int,a.BILL_Date_dt as AlertDtl_BillDate_date,a.BILL_NetAmt_num as AlertDtl_BillAmt_dec,
                                '' as AlertDtl_ProNo_var,0 as AlertDtl_ProNo_int,null as AlertDtl_ProDate_date,'' as AlertDtl_RefNo_var,0 as AlertDtl_RecNo_int,
                                c.CL_Name_var as AlertDtl_CL_Name_var,d.SITE_Name_var as AlertDtl_SITE_Name_var,
                                (select MATERIAL_Name_var from tbl_Material e where e.MATERIAL_RecordType_var=a.BILL_RecordType_var)  as AlertDtl_RecType_var,
                                c.CL_Id as AlertDtl_CLId_int,d.SITE_Id as AlertDtl_SiteId_int,19 as AlertDtl_Alert_Id,
                                (select USER_NAME_var from tbl_User x where x.USER_Id IN (select ROUTE_ME_Id from tbl_Route where ROUTE_Id=d.SITE_Route_Id)) as AlertDtl_MEName_var
                                ,null as AlertDtl_ProOrderDate_date,null as AlertDtl_BillModifiedDate_date,null as AlertDtl_CollctnEnqDate_date
                                ,null as AlertDtl_InwdRecDate_date,null as AlertDtl_EnteredDate_date,null as AlertDtl_CheckedDate_date,null as AlertDtl_AppDate_date,b.OUTW_OutwardDate_dt as AlertDtl_OutwrdDate_date,'' as AlertDtl_TestName_var
                                from tbl_Bill as a,tbl_Client as c,tbl_Site as d,tbl_Outward b,tbl_cashDetail e
				                where  a.BILL_CL_Id=c.CL_Id 
				                and a.BILL_Id = b.OUTW_ReferenceNo_var  and b.OUTW_RecordType_var ='DT'
				                and a.Bill_SITE_Id=d.SITE_Id
				                and a.BILL_OutwardStatus_bit =1
				                and a.BILL_Id=e.CashDetail_BillNo_int
                                and CONVERT (DATE, b.OUTW_OutwardDate_dt) >=CONVERT (DATE,'" + fromDate + "') and CONVERT (DATE,b.OUTW_OutwardDate_dt) <=CONVERT (DATE,'" + ToDate + "') " +
                                @" and datediff(d,convert(date,b.OUTW_OutwardDate_dt),convert(date,GETDATE())) > 61
				                and datediff(d,convert(date,b.OUTW_OutwardDate_dt),convert(date,e.CashDetail_Date_date)) > 61         
                                and a.Bill_Status_bit=0
                                and (a.BILL_NetAmt_num + (select Sum(b.CashDetail_Amount_money) from tbl_cashDetail b
                                where b.CashDetail_BillNo_int=a.BILL_Id and b.CashDetail_Status_bit=0))>0
                                order by AlertDtl_RecType_var";
                    }
                    else
                    {
                        str = @"---- Payment not received in 60 days after bill received by the client
                                Select 0 as AlertDtl_EnqNo_int,null as AlertDtl_EnqDate_date,a.BILL_Id as AlertDtl_BillNo_int,a.BILL_Date_dt as AlertDtl_BillDate_date,a.BILL_NetAmt_num as AlertDtl_BillAmt_dec,
                                '' as AlertDtl_ProNo_var,0 as AlertDtl_ProNo_int,null as AlertDtl_ProDate_date,'' as AlertDtl_RefNo_var,0 as AlertDtl_RecNo_int,
                                c.CL_Name_var as AlertDtl_CL_Name_var,d.SITE_Name_var as AlertDtl_SITE_Name_var,(select MATERIAL_Name_var from tbl_Material e where e.MATERIAL_RecordType_var=a.BILL_RecordType_var)  as AlertDtl_RecType_var,
                                c.CL_Id as AlertDtl_CLId_int,d.SITE_Id as AlertDtl_SiteId_int,19 as AlertDtl_Alert_Id,
                                (select USER_NAME_var from tbl_User x where x.USER_Id IN (select ROUTE_ME_Id from tbl_Route where ROUTE_Id=d.SITE_Route_Id)) as AlertDtl_MEName_var
                                ,null as AlertDtl_ProOrderDate_date,null as AlertDtl_BillModifiedDate_date,null as AlertDtl_CollctnEnqDate_date
                                ,null as AlertDtl_InwdRecDate_date,null as AlertDtl_EnteredDate_date,null as AlertDtl_CheckedDate_date,null as AlertDtl_AppDate_date,b.OUTW_OutwardDate_dt as AlertDtl_OutwrdDate_date,'' as AlertDtl_TestName_var
                                from tbl_Bill as a,tbl_Client as c,tbl_Site as d,tbl_Outward b
				                where  a.BILL_CL_Id=c.CL_Id 
				                and a.BILL_Id = b.OUTW_ReferenceNo_var  and b.OUTW_RecordType_var ='DT'
				                and a.Bill_SITE_Id=d.SITE_Id
				                and a.BILL_OutwardStatus_bit =1
				                and CONVERT (DATE,b.OUTW_OutwardDate_dt) >=CONVERT (DATE,'" + fromDate + "') and CONVERT (DATE,b.OUTW_OutwardDate_dt) <=CONVERT (DATE,'" + ToDate + "') " +
                                @" and a.Bill_id not in  (select CashDetail_BillNo_int from tbl_CashDetail where CashDetail_Status_bit=0)
				                and datediff(d,convert(date,b.OUTW_OutwardDate_dt),convert(date,GETDATE())) > 61 
                                and a.Bill_Status_bit=0
                                order by AlertDtl_RecType_var";
                    }
                }
                else if (Alert_Id == 20)
                {
                    if (flagStatus == 0)
                    {
                        str = @"---- Payment not received in 90 days after bill received by the client
                                 Select 0 as AlertDtl_EnqNo_int,null as AlertDtl_EnqDate_date,a.BILL_Id as AlertDtl_BillNo_int,a.BILL_Date_dt as AlertDtl_BillDate_date,a.BILL_NetAmt_num as AlertDtl_BillAmt_dec,
                                '' as AlertDtl_ProNo_var,0 as AlertDtl_ProNo_int,null as AlertDtl_ProDate_date,'' as AlertDtl_RefNo_var,0 as AlertDtl_RecNo_int,
                                c.CL_Name_var as AlertDtl_CL_Name_var,d.SITE_Name_var as AlertDtl_SITE_Name_var,
                                (select MATERIAL_Name_var from tbl_Material e where e.MATERIAL_RecordType_var=a.BILL_RecordType_var)  as AlertDtl_RecType_var,
                                c.CL_Id as AlertDtl_CLId_int,d.SITE_Id as AlertDtl_SiteId_int,20 as AlertDtl_Alert_Id,
                                (select USER_NAME_var from tbl_User x where x.USER_Id IN (select ROUTE_ME_Id from tbl_Route where ROUTE_Id=d.SITE_Route_Id)) as AlertDtl_MEName_var
                                ,null as AlertDtl_ProOrderDate_date,null as AlertDtl_BillModifiedDate_date,null as AlertDtl_CollctnEnqDate_date
                                ,null as AlertDtl_InwdRecDate_date,null as AlertDtl_EnteredDate_date,null as AlertDtl_CheckedDate_date,null as AlertDtl_AppDate_date,b.OUTW_OutwardDate_dt as AlertDtl_OutwrdDate_date,'' as AlertDtl_TestName_var
                                from tbl_Bill as a,tbl_Client as c,tbl_Site as d,tbl_Outward b,tbl_cashDetail e
				                where  a.BILL_CL_Id=c.CL_Id 
				                and a.BILL_Id = b.OUTW_ReferenceNo_var  and b.OUTW_RecordType_var ='DT'
				                and a.Bill_SITE_Id=d.SITE_Id
				                and a.BILL_OutwardStatus_bit =1
				                and a.BILL_Id=e.CashDetail_BillNo_int
                                and CONVERT (DATE, b.OUTW_OutwardDate_dt) >=CONVERT (DATE,'" + fromDate + "') and CONVERT (DATE,b.OUTW_OutwardDate_dt) <=CONVERT (DATE,'" + ToDate + "') " +
                                @" and datediff(d,convert(date,b.OUTW_OutwardDate_dt),convert(date,GETDATE())) > 91
				                and datediff(d,convert(date,b.OUTW_OutwardDate_dt),convert(date,e.CashDetail_Date_date)) > 91         
                                and a.Bill_Status_bit=0
                                and (a.BILL_NetAmt_num + (select Sum(b.CashDetail_Amount_money) from tbl_cashDetail b
                                where b.CashDetail_BillNo_int=a.BILL_Id and b.CashDetail_Status_bit=0))>0
                                order by AlertDtl_RecType_var";
                    }
                    else
                    {
                        str = @"---- Payment not received in 90 days after bill received by the client
                                 Select 0 as AlertDtl_EnqNo_int,null as AlertDtl_EnqDate_date,a.BILL_Id as AlertDtl_BillNo_int,a.BILL_Date_dt as AlertDtl_BillDate_date,a.BILL_NetAmt_num as AlertDtl_BillAmt_dec,
                                '' as AlertDtl_ProNo_var,0 as AlertDtl_ProNo_int,null as AlertDtl_ProDate_date,'' as AlertDtl_RefNo_var,0 as AlertDtl_RecNo_int,
                                c.CL_Name_var as AlertDtl_CL_Name_var,d.SITE_Name_var as AlertDtl_SITE_Name_var,(select MATERIAL_Name_var from tbl_Material e where e.MATERIAL_RecordType_var=a.BILL_RecordType_var)  as AlertDtl_RecType_var,
                                c.CL_Id as AlertDtl_CLId_int,d.SITE_Id as AlertDtl_SiteId_int,20 as AlertDtl_Alert_Id,
                                (select USER_NAME_var from tbl_User x where x.USER_Id IN (select ROUTE_ME_Id from tbl_Route where ROUTE_Id=d.SITE_Route_Id)) as AlertDtl_MEName_var
                                ,null as AlertDtl_ProOrderDate_date,null as AlertDtl_BillModifiedDate_date,null as AlertDtl_CollctnEnqDate_date
                                ,null as AlertDtl_InwdRecDate_date,null as AlertDtl_EnteredDate_date,null as AlertDtl_CheckedDate_date,null as AlertDtl_AppDate_date,b.OUTW_OutwardDate_dt as AlertDtl_OutwrdDate_date,'' as AlertDtl_TestName_var
                                from tbl_Bill as a,tbl_Client as c,tbl_Site as d,tbl_Outward b
				                where  a.BILL_CL_Id=c.CL_Id 
				                and a.BILL_Id = b.OUTW_ReferenceNo_var  and b.OUTW_RecordType_var ='DT'
				                and a.Bill_SITE_Id=d.SITE_Id
				                and a.BILL_OutwardStatus_bit =1
				                and CONVERT (DATE,b.OUTW_OutwardDate_dt) >=CONVERT (DATE,'" + fromDate + "') and CONVERT (DATE,b.OUTW_OutwardDate_dt) <=CONVERT (DATE,'" + ToDate + "') " +
                                @" and a.Bill_id not in  (select CashDetail_BillNo_int from tbl_CashDetail where CashDetail_Status_bit=0)
				                and datediff(d,convert(date,b.OUTW_OutwardDate_dt),convert(date,GETDATE())) > 91 
                                and a.Bill_Status_bit=0
                                order by AlertDtl_RecType_var";
                    }
                }
                else if (Alert_Id == 21)
                {
                    if (flagStatus == 0)
                    {
                        str = @"---- Payment not received in 120 days after bill received by the client
                                 Select 0 as AlertDtl_EnqNo_int,null as AlertDtl_EnqDate_date,a.BILL_Id as AlertDtl_BillNo_int,a.BILL_Date_dt as AlertDtl_BillDate_date,a.BILL_NetAmt_num as AlertDtl_BillAmt_dec,
                                '' as AlertDtl_ProNo_var,0 as AlertDtl_ProNo_int,null as AlertDtl_ProDate_date,'' as AlertDtl_RefNo_var,0 as AlertDtl_RecNo_int,
                                c.CL_Name_var as AlertDtl_CL_Name_var,d.SITE_Name_var as AlertDtl_SITE_Name_var,
                                (select MATERIAL_Name_var from tbl_Material e where e.MATERIAL_RecordType_var=a.BILL_RecordType_var)  as AlertDtl_RecType_var,
                                c.CL_Id as AlertDtl_CLId_int,d.SITE_Id as AlertDtl_SiteId_int,21 as AlertDtl_Alert_Id,
                                (select USER_NAME_var from tbl_User x where x.USER_Id IN (select ROUTE_ME_Id from tbl_Route where ROUTE_Id=d.SITE_Route_Id)) as AlertDtl_MEName_var
                                ,null as AlertDtl_ProOrderDate_date,null as AlertDtl_BillModifiedDate_date,null as AlertDtl_CollctnEnqDate_date
                                ,null as AlertDtl_InwdRecDate_date,null as AlertDtl_EnteredDate_date,null as AlertDtl_CheckedDate_date,null as AlertDtl_AppDate_date,b.OUTW_OutwardDate_dt as AlertDtl_OutwrdDate_date,'' as AlertDtl_TestName_var
                                from tbl_Bill as a,tbl_Client as c,tbl_Site as d,tbl_Outward b,tbl_cashDetail e
				                where  a.BILL_CL_Id=c.CL_Id 
				                and a.BILL_Id = b.OUTW_ReferenceNo_var  and b.OUTW_RecordType_var ='DT'
				                and a.Bill_SITE_Id=d.SITE_Id
				                and a.BILL_OutwardStatus_bit =1
				                and a.BILL_Id=e.CashDetail_BillNo_int
                                and CONVERT (DATE, b.OUTW_OutwardDate_dt) >=CONVERT (DATE,'" + fromDate + "') and CONVERT (DATE,b.OUTW_OutwardDate_dt) <=CONVERT (DATE,'" + ToDate + "') " +
                                @" and datediff(d,convert(date,b.OUTW_OutwardDate_dt),convert(date,GETDATE())) > 121
				                and datediff(d,convert(date,b.OUTW_OutwardDate_dt),convert(date,e.CashDetail_Date_date)) > 121         
                                and a.Bill_Status_bit=0
                                and (a.BILL_NetAmt_num + (select Sum(b.CashDetail_Amount_money) from tbl_cashDetail b
                                where b.CashDetail_BillNo_int=a.BILL_Id and b.CashDetail_Status_bit=0))>0
                                order by AlertDtl_RecType_var";
                    }
                    else
                    {
                        str = @"---- Payment not received in 120 days after bill received by the client
                                 Select 0 as AlertDtl_EnqNo_int,null as AlertDtl_EnqDate_date,a.BILL_Id as AlertDtl_BillNo_int,a.BILL_Date_dt as AlertDtl_BillDate_date,a.BILL_NetAmt_num as AlertDtl_BillAmt_dec,
                                '' as AlertDtl_ProNo_var,0 as AlertDtl_ProNo_int,null as AlertDtl_ProDate_date,'' as AlertDtl_RefNo_var,0 as AlertDtl_RecNo_int,
                                c.CL_Name_var as AlertDtl_CL_Name_var,d.SITE_Name_var as AlertDtl_SITE_Name_var,(select MATERIAL_Name_var from tbl_Material e where e.MATERIAL_RecordType_var=a.BILL_RecordType_var)  as AlertDtl_RecType_var,
                                c.CL_Id as AlertDtl_CLId_int,d.SITE_Id as AlertDtl_SiteId_int,21 as AlertDtl_Alert_Id,
                                (select USER_NAME_var from tbl_User x where x.USER_Id IN (select ROUTE_ME_Id from tbl_Route where ROUTE_Id=d.SITE_Route_Id)) as AlertDtl_MEName_var
                                ,null as AlertDtl_ProOrderDate_date,null as AlertDtl_BillModifiedDate_date,null as AlertDtl_CollctnEnqDate_date
                                ,null as AlertDtl_InwdRecDate_date,null as AlertDtl_EnteredDate_date,null as AlertDtl_CheckedDate_date,null as AlertDtl_AppDate_date,b.OUTW_OutwardDate_dt as AlertDtl_OutwrdDate_date,'' as AlertDtl_TestName_var
                                from tbl_Bill as a,tbl_Client as c,tbl_Site as d,tbl_Outward b
				                where  a.BILL_CL_Id=c.CL_Id 
				                and a.BILL_Id = b.OUTW_ReferenceNo_var  and b.OUTW_RecordType_var ='DT'
				                and a.Bill_SITE_Id=d.SITE_Id
				                and a.BILL_OutwardStatus_bit =1
				                and CONVERT (DATE,b.OUTW_OutwardDate_dt) >=CONVERT (DATE,'" + fromDate + "') and CONVERT (DATE,b.OUTW_OutwardDate_dt) <=CONVERT (DATE,'" + ToDate + "') " +
                                @" and a.Bill_id not in  (select CashDetail_BillNo_int from tbl_CashDetail where CashDetail_Status_bit=0)
				                and datediff(d,convert(date,b.OUTW_OutwardDate_dt),convert(date,GETDATE())) > 121 
                                and a.Bill_Status_bit=0
                                order by AlertDtl_RecType_var";
                    }
                }
                else if (Alert_Id == 22)
                {
                    if (flagStatus == 0)
                    {
                        str = @" ----Payment outstanding for more than 120 days
				                Select 0 as AlertDtl_EnqNo_int,null as AlertDtl_EnqDate_date,a.BILL_Id as AlertDtl_BillNo_int,a.BILL_Date_dt as AlertDtl_BillDate_date,a.BILL_NetAmt_num as AlertDtl_BillAmt_dec,
                                '' as AlertDtl_ProNo_var,0 as AlertDtl_ProNo_int,null as AlertDtl_ProDate_date,'' as AlertDtl_RefNo_var,0 as AlertDtl_RecNo_int,
                                c.CL_Name_var as AlertDtl_CL_Name_var,d.SITE_Name_var as AlertDtl_SITE_Name_var,
                                (select MATERIAL_Name_var from tbl_Material e where e.MATERIAL_RecordType_var=a.BILL_RecordType_var)  as AlertDtl_RecType_var,
                                c.CL_Id as AlertDtl_CLId_int,d.SITE_Id as AlertDtl_SiteId_int,22 as AlertDtl_Alert_Id,
                                (select USER_NAME_var from tbl_User x where x.USER_Id IN (select ROUTE_ME_Id from tbl_Route where ROUTE_Id=d.SITE_Route_Id)) as AlertDtl_MEName_var
                                ,null as AlertDtl_ProOrderDate_date,null as AlertDtl_BillModifiedDate_date,null as AlertDtl_CollctnEnqDate_date
                                ,null as AlertDtl_InwdRecDate_date,null as AlertDtl_EnteredDate_date,null as AlertDtl_CheckedDate_date,null as AlertDtl_AppDate_date,b.OUTW_OutwardDate_dt as AlertDtl_OutwrdDate_date,'' as AlertDtl_TestName_var
                                from tbl_Bill as a,tbl_Client as c,tbl_Site as d,tbl_Outward b,tbl_cashDetail e
				                where  a.BILL_CL_Id=c.CL_Id 
				                and a.BILL_Id = b.OUTW_ReferenceNo_var  and b.OUTW_RecordType_var ='DT'
				                and a.Bill_SITE_Id=d.SITE_Id
				                and a.BILL_OutwardStatus_bit =1
				                and a.BILL_Id=e.CashDetail_BillNo_int
                                and CONVERT (DATE, b.OUTW_OutwardDate_dt) >=CONVERT (DATE,'" + fromDate + "') and CONVERT (DATE,b.OUTW_OutwardDate_dt) <=CONVERT (DATE,'" + ToDate + "') " +
                                @" and datediff(d,convert(date,b.OUTW_OutwardDate_dt),convert(date,GETDATE())) > 122
				                and datediff(d,convert(date,b.OUTW_OutwardDate_dt),convert(date,e.CashDetail_Date_date)) > 122        
                                and a.Bill_Status_bit=0
                                and (a.BILL_NetAmt_num + (select Sum(b.CashDetail_Amount_money) from tbl_cashDetail b
                                where b.CashDetail_BillNo_int=a.BILL_Id and b.CashDetail_Status_bit=0))>0
                                order by AlertDtl_RecType_var";
                    }
                    else
                    {
                        str = @" ----Payment outstanding for more than 120 days
				               Select 0 as AlertDtl_EnqNo_int,null as AlertDtl_EnqDate_date,a.BILL_Id as AlertDtl_BillNo_int,a.BILL_Date_dt as AlertDtl_BillDate_date,a.BILL_NetAmt_num as AlertDtl_BillAmt_dec,
                                '' as AlertDtl_ProNo_var,0 as AlertDtl_ProNo_int,null as AlertDtl_ProDate_date,'' as AlertDtl_RefNo_var,0 as AlertDtl_RecNo_int,
                                c.CL_Name_var as AlertDtl_CL_Name_var,d.SITE_Name_var as AlertDtl_SITE_Name_var,(select MATERIAL_Name_var from tbl_Material e where e.MATERIAL_RecordType_var=a.BILL_RecordType_var)  as AlertDtl_RecType_var,
                                c.CL_Id as AlertDtl_CLId_int,d.SITE_Id as AlertDtl_SiteId_int,22 as AlertDtl_Alert_Id,
                                (select USER_NAME_var from tbl_User x where x.USER_Id IN (select ROUTE_ME_Id from tbl_Route where ROUTE_Id=d.SITE_Route_Id)) as AlertDtl_MEName_var
                                ,null as AlertDtl_ProOrderDate_date,null as AlertDtl_BillModifiedDate_date,null as AlertDtl_CollctnEnqDate_date
                                ,null as AlertDtl_InwdRecDate_date,null as AlertDtl_EnteredDate_date,null as AlertDtl_CheckedDate_date,null as AlertDtl_AppDate_date,b.OUTW_OutwardDate_dt as AlertDtl_OutwrdDate_date,'' as AlertDtl_TestName_var
                                from tbl_Bill as a,tbl_Client as c,tbl_Site as d,tbl_Outward b
				                where  a.BILL_CL_Id=c.CL_Id 
				                and a.BILL_Id = b.OUTW_ReferenceNo_var  and b.OUTW_RecordType_var ='DT'
				                and a.Bill_SITE_Id=d.SITE_Id
				                and a.BILL_OutwardStatus_bit =1
				                and CONVERT (DATE,b.OUTW_OutwardDate_dt) >=CONVERT (DATE,'" + fromDate + "') and CONVERT (DATE,b.OUTW_OutwardDate_dt) <=CONVERT (DATE,'" + ToDate + "') " +
                                @" and a.Bill_id not in  (select CashDetail_BillNo_int from tbl_CashDetail where CashDetail_Status_bit=0)
				                and datediff(d,convert(date,b.OUTW_OutwardDate_dt),convert(date,GETDATE())) > 122
                                and a.Bill_Status_bit=0
                                order by AlertDtl_RecType_var";

                    }
                }
                SqlDataAdapter da = new SqlDataAdapter(str, con);//and CL_name_var like '" + str + "%'
                da.Fill(ds);
                dt = ds.Tables[0];
                ds.Dispose();
                return dt;
            }
            catch (Exception e)
            {
                throw e;
            }

        }


        public DataTable AlertDetailsViewForEscalation(int Alert_Id, string fromDate, string ToDate, int flagStatus)
        {
            SqlConnection con = new SqlConnection(cnStr);
            try
            {
                DataSet ds = new DataSet();
                DataTable dt = new DataTable();
                string str = "";
                if (Alert_Id == 23)
                {
                    if (flagStatus == 0)
                    {
                        str = @"--No of proposal delayed by more than 2 days after receiving enquiry
                select a.ENQ_Id as AlertDtl_EnqNo_int,a.ENQ_Date_dt as AlertDtl_EnqDate_date,null as AlertDtl_CollctnEnqDate_date,'' as AlertDtl_BillNo_int,null as AlertDtl_BillDate_date,0 as AlertDtl_BillAmt_dec,
				b.Proposal_No as AlertDtl_ProNo_var,b.Proposal_ID as AlertDtl_ProNo_int,b.Proposal_Date as AlertDtl_ProDate_date,'' as AlertDtl_RefNo_var,0 as AlertDtl_RecNo_int,
				c.CL_Name_var as AlertDtl_CL_Name_var,d.SITE_Name_var as AlertDtl_SITE_Name_var,
                e.MATERIAL_RecordType_var as AlertDtl_RecType_var
				 ,c.CL_Id,d.SITE_Id,23 as AlertDtl_Alert_Id
				,(select USER_NAME_var from tbl_User x where x.USER_Id IN (select ROUTE_ME_Id from tbl_Route where ROUTE_Id=d.SITE_Route_Id)) as AlertDtl_MEName_var
				,null as AlertDtl_ProOrderDate_date,null as AlertDtl_BillModifiedDate_date,null as AlertDtl_AppDate_date
                ,null as AlertDtl_InwdRecDate_date,null as AlertDtl_EnteredDate_date,null as AlertDtl_CheckedDate_date,null as AlertDtl_OutwrdDate_date,'' as AlertDtl_TestName_var
                from tbl_Enquiry as a,tbl_Proposal as b,tbl_Client as c,tbl_Site as d,tbl_Material as e
				where a.ENQ_Id=b.Proposal_EnqNo
				and a.ENQ_CL_Id=c.CL_Id
				and a.ENQ_SITE_Id=d.SITE_Id
				and a.ENQ_MATERIAL_Id=e.MATERIAL_Id
				and CONVERT (DATE, a.ENQ_Date_dt) >=CONVERT (DATE,'" + fromDate + "') and CONVERT (DATE, a.ENQ_Date_dt) <=CONVERT (DATE,'" + ToDate + "') " +
                @"and datediff(d,convert(date,a.ENQ_Date_dt),convert(date,b.Proposal_Date)) > 2
				and datediff(d,convert(date,a.ENQ_Date_dt),convert(date,GETDATE())) > 2 
				and b.Proposal_NewClientStatus=0 and b.Proposal_ActiveStatus_bit=0 and b.Proposal_AppEnqStatus_bit=0
				
				Union
				
				Select a.ENQNEW_Id as AlertDtl_EnqNo_int,a.ENQNEW_Date_dt as AlertDtl_EnqDate_date,null as AlertDtl_CollctnEnqDate_date,'' as AlertDtl_BillNo_int,null as AlertDtl_BillDate_date,0 as AlertDtl_BillAmt_dec,
				b.Proposal_No as AlertDtl_ProNo_var,b.Proposal_ID as AlertDtl_ProNo_int,b.Proposal_Date as AlertDtl_ProDate_date,'' as AlertDtl_RefNo_var,0 as AlertDtl_RecNo_int,
				ENQNEW_ClientName_var as AlertDtl_CL_Name_var,ENQNEW_SiteName_var as AlertDtl_SITE_Name_var, 
                e.MATERIAL_RecordType_var as AlertDtl_RecType_var
				,0 as CL_Id,0 as SITE_Id,23 as AlertDtl_Alert_Id
                ,(select USER_NAME_var from tbl_User x where x.USER_Id IN (select ROUTE_ME_Id from tbl_Route where ROUTE_Id=a.ENQNEW_ROUTE_Id)) as AlertDtl_MEName_var				
				,null as AlertDtl_ProOrderDate_date,null as AlertDtl_BillModifiedDate_date,null as AlertDtl_AppDate_date
                ,null as AlertDtl_InwdRecDate_date,null as AlertDtl_EnteredDate_date,null as AlertDtl_CheckedDate_date,null as AlertDtl_OutwrdDate_date,'' as AlertDtl_TestName_var
                from tbl_EnquiryNewClient as a,tbl_Proposal as b,tbl_Material as e
				where a.ENQNEW_Id=b.Proposal_EnqNo
				and a.ENQNEW_MATERIAL_Id=e.MATERIAL_Id
				and CONVERT (DATE, a.ENQNEW_Date_dt) >=CONVERT(DATE,'" + fromDate + "') and CONVERT (DATE, a.ENQNEW_Date_dt) <=CONVERT(DATE,'" + ToDate + "')  " +
                @"and datediff(d,convert(date,a.ENQNEW_Date_dt),convert(date,b.Proposal_Date)) > 2
				and datediff(d,convert(date,a.ENQNEW_Date_dt),convert(date,GETDATE())) > 2
				and b.Proposal_NewClientStatus=1  and b.Proposal_ActiveStatus_bit=0 and b.Proposal_AppEnqStatus_bit=0
                order by AlertDtl_RecType_var";
                    }
                    else
                    {
                        str = @"--No of proposal delayed by more than 2 days after receiving enquiry
                select a.ENQ_Id as AlertDtl_EnqNo_int,a.ENQ_Date_dt as AlertDtl_EnqDate_date,null as AlertDtl_CollctnEnqDate_date,'' as AlertDtl_BillNo_int,null as AlertDtl_BillDate_date,0 as AlertDtl_BillAmt_dec,
				b.Proposal_No as AlertDtl_ProNo_var,b.Proposal_ID as AlertDtl_ProNo_int,b.Proposal_Date as AlertDtl_ProDate_date,'' as AlertDtl_RefNo_var,0 as AlertDtl_RecNo_int,
				c.CL_Name_var as AlertDtl_CL_Name_var,d.SITE_Name_var as AlertDtl_SITE_Name_var,e.MATERIAL_Name_var as AlertDtl_RecType_var,c.CL_Id,d.SITE_Id,23 as AlertDtl_Alert_Id
				,(select USER_NAME_var from tbl_User x where x.USER_Id IN (select ROUTE_ME_Id from tbl_Route where ROUTE_Id=d.SITE_Route_Id)) as AlertDtl_MEName_var
				,null as AlertDtl_ProOrderDate_date,null as AlertDtl_BillModifiedDate_date,null as AlertDtl_AppDate_date
                ,null as AlertDtl_InwdRecDate_date,null as AlertDtl_EnteredDate_date,null as AlertDtl_CheckedDate_date,null as AlertDtl_OutwrdDate_date,'' as AlertDtl_TestName_var
                from tbl_Enquiry as a,tbl_Proposal as b,tbl_Client as c,tbl_Site as d,tbl_Material as e
				where a.ENQ_Id=b.Proposal_EnqNo
				and a.ENQ_CL_Id=c.CL_Id
				and a.ENQ_SITE_Id=d.SITE_Id
				and a.ENQ_MATERIAL_Id=e.MATERIAL_Id
				and CONVERT (DATE, a.ENQ_Date_dt) >=CONVERT(DATE,'" + fromDate + "') and CONVERT (DATE, a.ENQ_Date_dt) <=CONVERT(DATE,'" + ToDate + "') " +
               @"and b.Proposal_Date is null and datediff(d,convert(date,a.ENQ_Date_dt),convert(date,GETDATE())) > 2 
				and (a.ENQ_Status_tint  < 2 or a.ENQ_Status_tint = 5) 
				and b.Proposal_NewClientStatus=0 and b.Proposal_ActiveStatus_bit=0 and b.Proposal_AppEnqStatus_bit=0
				
				Union
				
				Select a.ENQNEW_Id as AlertDtl_EnqNo_int,a.ENQNEW_Date_dt as AlertDtl_EnqDate_date,null as AlertDtl_CollctnEnqDate_date,'' as AlertDtl_BillNo_int,null as AlertDtl_BillDate_date,0 as AlertDtl_BillAmt_dec,
				b.Proposal_No as AlertDtl_ProNo_var,b.Proposal_ID as AlertDtl_ProNo_int,b.Proposal_Date as AlertDtl_ProDate_date,'' as AlertDtl_RefNo_var,0 as AlertDtl_RecNo_int,
				ENQNEW_ClientName_var as AlertDtl_CL_Name_var,ENQNEW_SiteName_var as AlertDtl_SITE_Name_var, 
                e.MATERIAL_RecordType_var as AlertDtl_RecType_var
				,0 as CL_Id,0 as SITE_Id,23 as AlertDtl_Alert_Id
               ,(select USER_NAME_var from tbl_User x where x.USER_Id IN (select ROUTE_ME_Id from tbl_Route where ROUTE_Id=a.ENQNEW_ROUTE_Id)) as AlertDtl_MEName_var				
				,null as AlertDtl_ProOrderDate_date,null as AlertDtl_BillModifiedDate_date,null as AlertDtl_AppDate_date
                ,null as AlertDtl_InwdRecDate_date,null as AlertDtl_EnteredDate_date,null as AlertDtl_CheckedDate_date,null as AlertDtl_OutwrdDate_date,'' as AlertDtl_TestName_var
                from tbl_EnquiryNewClient as a,tbl_Proposal as b,tbl_Material as e
				where a.ENQNEW_Id=b.Proposal_EnqNo
				and a.ENQNEW_MATERIAL_Id=e.MATERIAL_Id
				and CONVERT (DATE, a.ENQNEW_Date_dt) >=CONVERT(DATE,'" + fromDate + "') and CONVERT (DATE, a.ENQNEW_Date_dt) <=CONVERT(DATE,'" + ToDate + "')  " +
                @"and  b.Proposal_Date is null and datediff(d,convert(date,a.ENQNEW_Date_dt),convert(date,GETDATE())) > 2 
				and (a.ENQNEW_Status_tint  < 2 or a.ENQNEW_Status_tint = 5) 
				and b.Proposal_NewClientStatus=1  and b.Proposal_ActiveStatus_bit=0 and b.Proposal_AppEnqStatus_bit=0
                    order by AlertDtl_RecType_var";
                    }

                }
                else if (Alert_Id == 24)
                {
                    if (flagStatus == 0)
                    {
                        str = @"--No of orders not received in 15 days after sending proposal
                Select a.ENQ_Id as AlertDtl_EnqNo_int,a.ENQ_Date_dt as AlertDtl_EnqDate_date,null as AlertDtl_CollctnEnqDate_date,'' as AlertDtl_BillNo_int,null as AlertDtl_BillDate_date,0 as AlertDtl_BillAmt_dec,
				b.Proposal_No as AlertDtl_ProNo_var,b.Proposal_Id,b.Proposal_Date as AlertDtl_ProDate_date,'' as AlertDtl_RefNo_var,0 as AlertDtl_RecNo_int,
				c.CL_Name_var as AlertDtl_CL_Name_var,d.SITE_Name_var as AlertDtl_SITE_Name_var,e.MATERIAL_Name_var as AlertDtl_RecType_var,c.CL_Id,d.SITE_Id,24 as AlertDtl_Alert_Id
				,(select USER_NAME_var from tbl_User x where x.USER_Id IN (select ROUTE_ME_Id from tbl_Route where ROUTE_Id=d.SITE_Route_Id)) as AlertDtl_MEName_var
				,null as AlertDtl_ProOrderDate_date,null as AlertDtl_BillModifiedDate_date,null as AlertDtl_AppDate_date
                ,null as AlertDtl_InwdRecDate_date,null as AlertDtl_EnteredDate_date,null as AlertDtl_CheckedDate_date,null as AlertDtl_OutwrdDate_date,'' as AlertDtl_TestName_var
                from tbl_Enquiry as a,tbl_Proposal as b,tbl_Client as c,tbl_Site as d,tbl_Material as e
				where a.ENQ_Id=b.Proposal_EnqNo
				and a.ENQ_CL_Id=c.CL_Id
				and a.ENQ_SITE_Id=d.SITE_Id
				and a.ENQ_MATERIAL_Id=e.MATERIAL_Id
				and CONVERT (DATE, Proposal_Date) >=CONVERT(DATE,'" + fromDate + "') and CONVERT (DATE, Proposal_Date) <=CONVERT(DATE,'" + ToDate + "') " +
                @"and datediff(d,convert(date,b.Proposal_Date),convert(date,b.Proposal_OrderDate_dt)) > 15
				and datediff(d,convert(date,b.Proposal_Date),convert(date,GETDATE())) > 15 and b.Proposal_AppEnqStatus_bit=0 and b.Proposal_NewClientStatus=0 and b.Proposal_ActiveStatus_bit=0
				
				Union
				
				Select a.ENQNEW_Id as AlertDtl_EnqNo_int,a.ENQNEW_Date_dt as AlertDtl_EnqDate_date,null as AlertDtl_CollctnEnqDate_date,'' as AlertDtl_BillNo_int,null as AlertDtl_BillDate_date,0 as AlertDtl_BillAmt_dec,
				b.Proposal_No as AlertDtl_ProNo_var,b.Proposal_Id,b.Proposal_Date as AlertDtl_ProDate_date,'' as AlertDtl_RefNo_var,0 as AlertDtl_RecNo_int,
				ENQNEW_ClientName_var as AlertDtl_CL_Name_var,ENQNEW_SiteName_var as AlertDtl_SITE_Name_var, 
                e.MATERIAL_RecordType_var as AlertDtl_RecType_var
				,0 as CL_Id,0 as SITE_Id,24 as AlertDtl_Alert_Id
               ,(select USER_NAME_var from tbl_User x where x.USER_Id IN (select ROUTE_ME_Id from tbl_Route where ROUTE_Id=a.ENQNEW_ROUTE_Id)) as AlertDtl_MEName_var				
				,null as AlertDtl_ProOrderDate_date,null as AlertDtl_BillModifiedDate_date,null as AlertDtl_AppDate_date
                ,null as AlertDtl_InwdRecDate_date,null as AlertDtl_EnteredDate_date,null as AlertDtl_CheckedDate_date,null as AlertDtl_OutwrdDate_date,'' as AlertDtl_TestName_var
                from tbl_EnquiryNewClient as a,tbl_Proposal as b,tbl_Material as e
				where a.ENQNEW_Id=b.Proposal_EnqNo
				and a.ENQNEW_MATERIAL_Id=e.MATERIAL_Id
				  and CONVERT (DATE, Proposal_Date) >=CONVERT(DATE,'" + fromDate + "') and CONVERT (DATE, Proposal_Date) <=CONVERT(DATE,'" + ToDate + "') " +
                @"and datediff(d,convert(date,b.Proposal_Date),convert(date,b.Proposal_OrderDate_dt)) > 15
				and datediff(d,convert(date,b.Proposal_Date),convert(date,GETDATE())) > 15
                and b.Proposal_NewClientStatus=1  and b.Proposal_ActiveStatus_bit=0 and b.Proposal_AppEnqStatus_bit=0 order by AlertDtl_RecType_var";
                    }
                    else
                    {
                        str = @"--No of orders not received in 15 days after sending proposal
                Select a.ENQ_Id as AlertDtl_EnqNo_int,a.ENQ_Date_dt as AlertDtl_EnqDate_date,null as AlertDtl_CollctnEnqDate_date,'' as AlertDtl_BillNo_int,null as AlertDtl_BillDate_date,0 as AlertDtl_BillAmt_dec,
				b.Proposal_No as AlertDtl_ProNo_var,b.Proposal_Id,b.Proposal_Date as AlertDtl_ProDate_date,'' as AlertDtl_RefNo_var,0 as AlertDtl_RecNo_int,
				c.CL_Name_var as AlertDtl_CL_Name_var,d.SITE_Name_var as AlertDtl_SITE_Name_var,e.MATERIAL_Name_var as AlertDtl_RecType_var,c.CL_Id,d.SITE_Id,24 as AlertDtl_Alert_Id
				,(select USER_NAME_var from tbl_User x where x.USER_Id IN (select ROUTE_ME_Id from tbl_Route where ROUTE_Id=d.SITE_Route_Id)) as AlertDtl_MEName_var
				,null as AlertDtl_ProOrderDate_date,null as AlertDtl_BillModifiedDate_date,null as AlertDtl_AppDate_date
                ,null as AlertDtl_InwdRecDate_date,null as AlertDtl_EnteredDate_date,null as AlertDtl_CheckedDate_date,null as AlertDtl_OutwrdDate_date,'' as AlertDtl_TestName_var
                from tbl_Enquiry as a,tbl_Proposal as b,tbl_Client as c,tbl_Site as d,tbl_Material as e
				where a.ENQ_Id=b.Proposal_EnqNo
				and a.ENQ_CL_Id=c.CL_Id
				and a.ENQ_SITE_Id=d.SITE_Id
				and a.ENQ_MATERIAL_Id=e.MATERIAL_Id
				and CONVERT (DATE, Proposal_Date) >=CONVERT(DATE,'" + fromDate + "') and CONVERT (DATE, Proposal_Date) <=CONVERT(DATE,'" + ToDate + "') " +
                @"and b.Proposal_OrderDate_dt is null and datediff(d,convert(date,b.Proposal_Date),convert(date,GETDATE())) > 15 
				and b.Proposal_NewClientStatus=0  and b.Proposal_ActiveStatus_bit=0 and b.Proposal_AppEnqStatus_bit=0
				
				Union
				
				Select a.ENQNEW_Id as AlertDtl_EnqNo_int,a.ENQNEW_Date_dt as AlertDtl_EnqDate_date,null as AlertDtl_CollctnEnqDate_date,'' as AlertDtl_BillNo_int,null as AlertDtl_BillDate_date,0 as AlertDtl_BillAmt_dec,
				b.Proposal_No as AlertDtl_ProNo_var,b.Proposal_Id,b.Proposal_Date as AlertDtl_ProDate_date,'' as AlertDtl_RefNo_var,0 as AlertDtl_RecNo_int,
				ENQNEW_ClientName_var as AlertDtl_CL_Name_var,ENQNEW_SiteName_var as AlertDtl_SITE_Name_var, 
               e.MATERIAL_RecordType_var as AlertDtl_RecType_var
				,0 as CL_Id,0 as SITE_Id,24 as AlertDtl_Alert_Id
               ,(select USER_NAME_var from tbl_User x where x.USER_Id IN (select ROUTE_ME_Id from tbl_Route where ROUTE_Id=a.ENQNEW_ROUTE_Id)) as AlertDtl_MEName_var				
				,null as AlertDtl_ProOrderDate_date,null as AlertDtl_BillModifiedDate_date,null as AlertDtl_AppDate_date
                ,null as AlertDtl_InwdRecDate_date,null as AlertDtl_EnteredDate_date,null as AlertDtl_CheckedDate_date,null as AlertDtl_OutwrdDate_date,'' as AlertDtl_TestName_var
                from tbl_EnquiryNewClient as a,tbl_Proposal as b,tbl_Material as e
				where a.ENQNEW_Id=b.Proposal_EnqNo
				and a.ENQNEW_MATERIAL_Id=e.MATERIAL_Id
				and a.ENQNEW_MATERIAL_Id=e.MATERIAL_Id 
				and CONVERT (DATE, Proposal_Date) >=CONVERT(DATE,'" + fromDate + "') and CONVERT (DATE, Proposal_Date) <=CONVERT(DATE,'" + ToDate + "') " +
                @"and b.Proposal_OrderDate_dt is null and datediff(d,convert(date,b.Proposal_Date),convert(date,GETDATE())) > 15 
				and b.Proposal_NewClientStatus=1 and b.Proposal_ActiveStatus_bit=0 and b.Proposal_AppEnqStatus_bit=0 order by AlertDtl_RecType_var";
                    }
                }
                else if (Alert_Id == 25)
                {
                    if (flagStatus == 0)
                    {
                        str = @"--No of orders for which material not collected in 3 days after receiving order
                Select a.ENQ_Id as AlertDtl_EnqNo_int,a.ENQ_Date_dt as AlertDtl_EnqDate_date,b.Proposal_OrderDate_dt as AlertDtl_ProOrderDate_date,'' as AlertDtl_BillNo_int,null as AlertDtl_BillDate_date,0 as AlertDtl_BillAmt_dec,
				b.Proposal_No as AlertDtl_ProNo_var,b.Proposal_Id as AlertDtl_ProNo_int,b.Proposal_Date as AlertDtl_ProDate_date,'' as AlertDtl_RefNo_var,0 as AlertDtl_RecNo_int,
				c.CL_Name_var as AlertDtl_CL_Name_var,d.SITE_Name_var as AlertDtl_SITE_Name_var,e.MATERIAL_Name_var as AlertDtl_RecType_var,c.CL_Id,d.SITE_Id,25 as AlertDtl_Alert_Id
			   ,(select USER_NAME_var from tbl_User x where x.USER_Id IN (select ROUTE_ME_Id from tbl_Route where ROUTE_Id=d.SITE_Route_Id)) as AlertDtl_MEName_var
				,null as AlertDtl_CollctnEnqDate_date,null as AlertDtl_BillModifiedDate_date,null as AlertDtl_AppDate_date
                ,null as AlertDtl_InwdRecDate_date,null as AlertDtl_EnteredDate_date,null as AlertDtl_CheckedDate_date,null as AlertDtl_OutwrdDate_date,'' as AlertDtl_TestName_var
                from tbl_Enquiry as a,tbl_Proposal as b,tbl_Client as c,tbl_Site as d,tbl_Material as e
				where a.ENQ_Id=b.Proposal_EnqNo
				and a.ENQ_CL_Id=c.CL_Id
				and a.ENQ_SITE_Id=d.SITE_Id
				and a.ENQ_MATERIAL_Id=e.MATERIAL_Id
				and CONVERT (DATE, b.Proposal_OrderDate_dt) >=CONVERT(DATE,'" + fromDate + "') and CONVERT (DATE, b.Proposal_OrderDate_dt) <=CONVERT(DATE,'" + ToDate + "') " +
                @"and datediff(d,convert(date,b.Proposal_OrderDate_dt),convert(date,a.ENQ_CollectionDate_dt)) > 3
				and datediff(d,convert(date,b.Proposal_OrderDate_dt),convert(date,GETDATE())) > 3 
                and ENQ_Status_tint=5    
                and b.Proposal_NewClientStatus=0 and b.Proposal_ActiveStatus_bit=0 and b.Proposal_AppEnqStatus_bit=0
                 
                Union 
                
                 Select a.ENQNEW_Id as AlertDtl_EnqNo_int,a.ENQNEW_Date_dt as AlertDtl_EnqDate_date,b.Proposal_OrderDate_dt as AlertDtl_ProOrderDate_date,'' as AlertDtl_BillNo_int,null as AlertDtl_BillDate_date,0 as AlertDtl_BillAmt_dec,
				b.Proposal_No as AlertDtl_ProNo_var,b.Proposal_Id,b.Proposal_Date as AlertDtl_ProDate_date,'' as AlertDtl_RefNo_var,0 as AlertDtl_RecNo_int,
				ENQNEW_ClientName_var as AlertDtl_CL_Name_var,ENQNEW_SiteName_var as AlertDtl_SITE_Name_var, 
                e.MATERIAL_RecordType_var as AlertDtl_RecType_var
				,0 as CL_Id,0 as SITE_Id,25 as AlertDtl_Alert_Id
               ,(select USER_NAME_var from tbl_User x where x.USER_Id IN (select ROUTE_ME_Id from tbl_Route where ROUTE_Id=a.ENQNEW_ROUTE_Id)) as AlertDtl_MEName_var				
				,null as AlertDtl_CollctnEnqDate_date,null as AlertDtl_BillModifiedDate_date,null as AlertDtl_AppDate_date
                ,null as AlertDtl_InwdRecDate_date,null as AlertDtl_EnteredDate_date,null as AlertDtl_CheckedDate_date,null as AlertDtl_OutwrdDate_date,'' as AlertDtl_TestName_var
                from tbl_EnquiryNewClient as a,tbl_Proposal as b,tbl_Material as e
				where a.ENQNEW_Id=b.Proposal_EnqNo
				and a.ENQNEW_MATERIAL_Id=e.MATERIAL_Id
				and b.Proposal_ActiveStatus_bit=0
                and b.Proposal_NewClientStatus=1 and b.Proposal_AppEnqStatus_bit=0
                and CONVERT (DATE, b.Proposal_OrderDate_dt) >=CONVERT(DATE,'" + fromDate + "') and CONVERT (DATE, b.Proposal_OrderDate_dt) <=CONVERT(DATE,'" + ToDate + "') " +
                @" and datediff(d,convert(date,b.Proposal_OrderDate_dt),convert(date,GETDATE())) > 3 
                and datediff(d,convert(date,b.Proposal_OrderDate_dt),convert(date,GETDATE())) > 3
               and ENQNEW_Status_tint=5
                order by AlertDtl_RecType_var";
                    }
                    else
                    {
                        str = @"--No of orders for which material not collected in 3 days after receiving order
                Select a.ENQ_Id as AlertDtl_EnqNo_int,a.ENQ_Date_dt as AlertDtl_EnqDate_date,b.Proposal_OrderDate_dt as AlertDtl_ProOrderDate_date,'' as AlertDtl_BillNo_int,null as AlertDtl_BillDate_date,0 as AlertDtl_BillAmt_dec,
				b.Proposal_No as AlertDtl_ProNo_var,b.Proposal_Id,b.Proposal_Date as AlertDtl_ProDate_date,'' as AlertDtl_RefNo_var,0 as AlertDtl_RecNo_int,
				c.CL_Name_var as AlertDtl_CL_Name_var,d.SITE_Name_var as AlertDtl_SITE_Name_var,e.MATERIAL_Name_var as AlertDtl_RecType_var,c.CL_Id,d.SITE_Id,25 as AlertDtl_Alert_Id
			   ,(select USER_NAME_var from tbl_User x where x.USER_Id IN (select ROUTE_ME_Id from tbl_Route where ROUTE_Id=d.SITE_Route_Id)) as AlertDtl_MEName_var
				,null as AlertDtl_CollctnEnqDate_date,null as AlertDtl_BillModifiedDate_date,null as AlertDtl_AppDate_date
                ,null as AlertDtl_InwdRecDate_date,null as AlertDtl_EnteredDate_date,null as AlertDtl_CheckedDate_date,null as AlertDtl_OutwrdDate_date,'' as AlertDtl_TestName_var
                from tbl_Enquiry as a,tbl_Proposal as b,tbl_Client as c,tbl_Site as d,tbl_Material as e
				where a.ENQ_Id=b.Proposal_EnqNo
				and a.ENQ_CL_Id=c.CL_Id
				and a.ENQ_SITE_Id=d.SITE_Id
				and a.ENQ_MATERIAL_Id=e.MATERIAL_Id
				and CONVERT (DATE, b.Proposal_OrderDate_dt) >=CONVERT(DATE,'" + fromDate + "') and CONVERT (DATE, b.Proposal_OrderDate_dt) <=CONVERT(DATE,'" + ToDate + "') " +
                @"and a.ENQ_CollectionDate_dt is null  and b.Proposal_AppEnqStatus_bit=0
                and datediff(d,convert(date,b.Proposal_OrderDate_dt),convert(date,GETDATE())) > 3
                and ENQ_Status_tint=5   and b.Proposal_NewClientStatus=0 and b.Proposal_ActiveStatus_bit=0
                   
                Union 
                
                 Select a.ENQNEW_Id as AlertDtl_EnqNo_int,a.ENQNEW_Date_dt as AlertDtl_EnqDate_date,b.Proposal_OrderDate_dt as AlertDtl_ProOrderDate_date,'' as AlertDtl_BillNo_int,null as AlertDtl_BillDate_date,0 as AlertDtl_BillAmt_dec,
				b.Proposal_No as AlertDtl_ProNo_var,b.Proposal_Id,b.Proposal_Date as AlertDtl_ProDate_date,'' as AlertDtl_RefNo_var,0 as AlertDtl_RecNo_int,
				ENQNEW_ClientName_var as AlertDtl_CL_Name_var,ENQNEW_SiteName_var as AlertDtl_SITE_Name_var, 
               e.MATERIAL_RecordType_var as AlertDtl_RecType_var
				,0 as CL_Id,0 as SITE_Id,25 as AlertDtl_Alert_Id
               ,(select USER_NAME_var from tbl_User x where x.USER_Id IN (select ROUTE_ME_Id from tbl_Route where ROUTE_Id=a.ENQNEW_ROUTE_Id)) as AlertDtl_MEName_var				
			    ,null as AlertDtl_CollctnEnqDate_date,null as AlertDtl_BillModifiedDate_date,null as AlertDtl_AppDate_date
                ,null as AlertDtl_InwdRecDate_date,null as AlertDtl_EnteredDate_date,null as AlertDtl_CheckedDate_date,null as AlertDtl_OutwrdDate_date,'' as AlertDtl_TestName_var
                from tbl_EnquiryNewClient as a,tbl_Proposal as b,tbl_Material as e
				where a.ENQNEW_Id=b.Proposal_EnqNo
				and a.ENQNEW_MATERIAL_Id=e.MATERIAL_Id
				and b.Proposal_ActiveStatus_bit=0 and b.Proposal_AppEnqStatus_bit=0
                and b.Proposal_NewClientStatus=1
                and CONVERT (DATE, b.Proposal_OrderDate_dt) >=CONVERT(DATE,'" + fromDate + "') and CONVERT (DATE, b.Proposal_OrderDate_dt) <=CONVERT(DATE,'" + ToDate + "') " +
                @"and Proposal_OrderNo_var is not null
				and a.ENQNEW_CollectionDate_dt is null 
                and datediff(d,convert(date,b.Proposal_OrderDate_dt),convert(date,GETDATE())) > 3 
                and ENQNEW_Status_tint=5
                order by AlertDtl_RecType_var";
                    }
                }
                else if (Alert_Id == 26)
                {
                    if (flagStatus == 0)
                    {
                        str = @"--No of orders for which material is not inwarded in 2 days after collection
                Select a.ENQ_Id as AlertDtl_EnqNo_int,a.ENQ_Date_dt as AlertDtl_EnqDate_date,ENQ_CollectionDate_dt as AlertDtl_CollctnEnqDate_date,'' as AlertDtl_BillNo_int,null as AlertDtl_BillDate_date,0 as AlertDtl_BillAmt_dec,
				'' as AlertDtl_ProNo_var,0 as AlertDtl_ProNo_int,null as AlertDtl_ProDate_date,'' as AlertDtl_RefNo_var,0 as AlertDtl_RecNo_int,
				c.CL_Name_var as AlertDtl_CL_Name_var,d.SITE_Name_var as AlertDtl_SITE_Name_var,e.MATERIAL_Name_var as AlertDtl_RecType_var,c.CL_Id,d.SITE_Id,26 as AlertDtl_Alert_Id
				,(select USER_NAME_var from tbl_User x where x.USER_Id IN (select ROUTE_ME_Id from tbl_Route where ROUTE_Id=d.SITE_Route_Id)) as AlertDtl_MEName_var
				,null as AlertDtl_ProOrderDate_date,null as AlertDtl_BillModifiedDate_date,null as AlertDtl_AppDate_date
                ,null as AlertDtl_InwdRecDate_date,null as AlertDtl_EnteredDate_date,null as AlertDtl_CheckedDate_date,null as AlertDtl_OutwrdDate_date,'' as AlertDtl_TestName_var
                from tbl_Enquiry as a,tbl_Inward as b,tbl_Client as c,tbl_Site as d,tbl_Material as e
				where a.ENQ_Id=b.INWD_ENQ_Id
				and a.ENQ_CL_Id=c.CL_Id
				and a.ENQ_SITE_Id=d.SITE_Id
				and a.ENQ_MATERIAL_Id=e.MATERIAL_Id
				and CONVERT (DATE, ENQ_CollectionDate_dt) >=CONVERT(DATE,'" + fromDate + "') and CONVERT (DATE, ENQ_CollectionDate_dt) <=CONVERT(DATE,'" + ToDate + "') " +
               @"and datediff(d,convert(date,a.ENQ_CollectionDate_dt),convert(date,b.INWD_ReceivedDate_dt)) > 2
				and datediff(d,convert(date,a.ENQ_CollectionDate_dt),convert(date,GETDATE())) > 2 
                 order by AlertDtl_RecType_var";
                    }
                    else
                    {
                        str = @"Select a.ENQ_Id as AlertDtl_EnqNo_int,a.ENQ_Date_dt as AlertDtl_EnqDate_date,ENQ_CollectionDate_dt as AlertDtl_CollctnEnqDate_date,'' as AlertDtl_BillNo_int,null as AlertDtl_BillDate_date,0 as AlertDtl_BillAmt_dec,
				'' as AlertDtl_ProNo_var,0 as AlertDtl_ProNo_int,null as AlertDtl_ProDate_date,'' as AlertDtl_RefNo_var,0 as AlertDtl_RecNo_int,
				c.CL_Name_var as AlertDtl_CL_Name_var,d.SITE_Name_var as AlertDtl_SITE_Name_var,e.MATERIAL_Name_var as AlertDtl_RecType_var,c.CL_Id,d.SITE_Id,26 as AlertDtl_Alert_Id
				,(select USER_NAME_var from tbl_User x where x.USER_Id IN (select ROUTE_ME_Id from tbl_Route where ROUTE_Id=d.SITE_Route_Id)) as AlertDtl_MEName_var
				,null as AlertDtl_ProOrderDate_date,null as AlertDtl_BillModifiedDate_date,null as AlertDtl_AppDate_date
                ,null as AlertDtl_InwdRecDate_date,null as AlertDtl_EnteredDate_date,null as AlertDtl_CheckedDate_date,null as AlertDtl_OutwrdDate_date,'' as AlertDtl_TestName_var
                from tbl_Enquiry as a,tbl_Inward as b,tbl_Client as c,tbl_Site as d,tbl_Material as e
				where a.ENQ_Id=b.INWD_ENQ_Id
				and a.ENQ_CL_Id=c.CL_Id
				and a.ENQ_SITE_Id=d.SITE_Id
				and a.ENQ_MATERIAL_Id=e.MATERIAL_Id
				and CONVERT (DATE, ENQ_CollectionDate_dt) >=CONVERT(DATE,'" + fromDate + "') and CONVERT (DATE, ENQ_CollectionDate_dt) <=CONVERT(DATE,'" + ToDate + "') " +
                 @"and b.INWD_ReceivedDate_dt is null 
                and datediff(d,convert(date,a.ENQ_CollectionDate_dt),convert(date,GETDATE())) > 2 
                and a.ENQ_Status_tint <> 2 order by AlertDtl_RecType_var";
                    }
                }

                else if (Alert_Id == 27)
                {
                    if (flagStatus == 0)
                    {
                        str = @"--No of inwards for which test is not started in 2 days after inward  
                Select 0 as AlertDtl_EnqNo_int,null as AlertDtl_EnqDate_date,b.INWD_ReceivedDate_dt as AlertDtl_InwdRecDate_date,'' as AlertDtl_BillNo_int,null as AlertDtl_BillDate_date,0 as AlertDtl_BillAmt_dec,
				'' as AlertDtl_ProNo_var,0 as AlertDtl_ProNo_int,null as AlertDtl_ProDate_date,MISRefNo as AlertDtl_RefNo_var,MISRecordNo  as AlertDtl_RecNo_int,
				c.CL_Name_var as AlertDtl_CL_Name_var,d.SITE_Name_var as AlertDtl_SITE_Name_var,e.MATERIAL_Name_var as AlertDtl_RecType_var,c.CL_Id,d.SITE_Id,27 as AlertDtl_Alert_Id
				,(select USER_NAME_var from tbl_User x where x.USER_Id IN (select ROUTE_ME_Id from tbl_Route where ROUTE_Id=d.SITE_Route_Id)) as AlertDtl_MEName_var
				,null as AlertDtl_ProOrderDate_date,null as AlertDtl_BillModifiedDate_date,null as AlertDtl_AppDate_date
                ,null as AlertDtl_CollctnEnqDate_date,null as AlertDtl_EnteredDate_date,null as AlertDtl_CheckedDate_date,null as AlertDtl_OutwrdDate_date,'' as AlertDtl_TestName_var
                from tbl_Enquiry as a,tbl_Inward as b,tbl_Client as c,tbl_Site as d,tbl_Material as e,tbl_MISDetail as f
				where a.ENQ_Id=b.INWD_ENQ_Id
				and b.INWD_RecordNo_int=f.MISRecordNo
				and b.INWD_RecordType_var=f.MISRecType
				and a.ENQ_CL_Id=c.CL_Id
				and a.ENQ_SITE_Id=d.SITE_Id
				and a.ENQ_MATERIAL_Id=e.MATERIAL_Id
                --and f.MISTestStartedbit = 0
                and f.MISTestType <> 'Final'
                and f.MISInwdApprovedDt is not null
                and DATEDIFF(hh, b.INWD_ReceivedDate_dt, getdate()) > 48
				and CONVERT (DATE, b.INWD_ReceivedDate_dt) >=CONVERT(DATE,'" + fromDate + "') and CONVERT (DATE,b.INWD_ReceivedDate_dt) <=CONVERT(DATE,'" + ToDate + "') " +
                @"--and datediff(d,convert(date,b.INWD_ReceivedDate_dt),convert(date,f.MISEnteredDt)) > 2
				--and datediff(d,convert(date,b.INWD_ReceivedDate_dt),convert(date,GETDATE())) > 2 
                 order by AlertDtl_RecType_var";
                    }
                    else
                    {
                        str = @"Select 0 as AlertDtl_EnqNo_int,null as AlertDtl_EnqDate_date,b.INWD_ReceivedDate_dt as AlertDtl_InwdRecDate_date,'' as AlertDtl_BillNo_int,null as AlertDtl_BillDate_date,0 as AlertDtl_BillAmt_dec,
				'' as AlertDtl_ProNo_var,0 as AlertDtl_ProNo_int,null as AlertDtl_ProDate_date,MISRefNo as AlertDtl_RefNo_var,MISRecordNo  as AlertDtl_RecNo_int,
				c.CL_Name_var as AlertDtl_CL_Name_var,d.SITE_Name_var as AlertDtl_SITE_Name_var,e.MATERIAL_Name_var as AlertDtl_RecType_var,c.CL_Id,d.SITE_Id,27 as AlertDtl_Alert_Id
				,(select USER_NAME_var from tbl_User x where x.USER_Id IN (select ROUTE_ME_Id from tbl_Route where ROUTE_Id=d.SITE_Route_Id)) as AlertDtl_MEName_var
				,null as AlertDtl_ProOrderDate_date,null as AlertDtl_BillModifiedDate_date,null as AlertDtl_AppDate_date
                ,null as AlertDtl_CollctnEnqDate_date,null as AlertDtl_EnteredDate_date,null as AlertDtl_CheckedDate_date,null as AlertDtl_OutwrdDate_date,'' as AlertDtl_TestName_var
                from tbl_Enquiry as a,tbl_Inward as b,tbl_Client as c,tbl_Site as d,tbl_Material as e,tbl_MISDetail as f
				where a.ENQ_Id=b.INWD_ENQ_Id
				and b.INWD_RecordNo_int=f.MISRecordNo
				and b.INWD_RecordType_var=f.MISRecType
				and a.ENQ_CL_Id=c.CL_Id
				and a.ENQ_SITE_Id=d.SITE_Id
				and a.ENQ_MATERIAL_Id=e.MATERIAL_Id
				and CONVERT (DATE, b.INWD_ReceivedDate_dt) >=CONVERT(DATE,'" + fromDate + "') and CONVERT (DATE,b.INWD_ReceivedDate_dt) <=CONVERT(DATE,'" + ToDate + "') " +
                @" and f.MISTestStartedbit = 0
                and f.MISTestType <> 'Final'
                and f.MISInwdApprovedDt is not null
                and DATEDIFF(hh, b.INWD_ReceivedDate_dt, getdate()) > 48
                --and f.MISEnteredDt is null 
                --and datediff(d,convert(date,b.INWD_ReceivedDate_dt),convert(date,GETDATE())) > 2 
				order by AlertDtl_RecType_var";
                    }
                }
                else if (Alert_Id == 28)
                {
                    if (flagStatus == 0)
                    {
                        str = @"---Category wise test not completed in X+2 days
                select  0 as AlertDtl_EnqNo_int,null as AlertDtl_EnqDate_date,a.AACINWD_CollectionDate_dt as AlertDtl_CollctnEnqDate_date,'' as AlertDtl_BillNo_int,null as AlertDtl_BillDate_date,0 as AlertDtl_BillAmt_dec,
                '' as AlertDtl_ProNo_var,0 as AlertDtl_ProNo_int,null as AlertDtl_ProDate_date,AACINWD_ReferenceNo_var as AlertDtl_RefNo_var,AACINWD_RecordNo_int as AlertDtl_RecNo_int
                ,CL_Name_var as AlertDtl_CL_Name_var,SITE_Name_var as AlertDtl_SITE_Name_var,'AAC' as AlertDtl_RecType_var,AACINWD_CL_Id  as AlertDtl_CLId_int,AACINWD_SITE_Id  as AlertDtl_SiteId_int,28  as AlertDtl_Alert_Id,
                (select USER_NAME_var from tbl_User x where x.USER_Id IN (select ROUTE_ME_Id from tbl_Route where ROUTE_Id=c.SITE_Route_Id)) as AlertDtl_MEName_var			
                 ,null as AlertDtl_ProOrderDate_date,null as AlertDtl_BillModifiedDate_date,null as AlertDtl_InwdRecDate_date
                ,null as AlertDtl_AppDate_date,null as AlertDtl_EnteredDate_date,null as AlertDtl_CheckedDate_date,null as AlertDtl_OutwrdDate_date
                ,(select TEST_Name_var from tbl_Test as d where a.AACINWD_TEST_Id=d.TEST_Id ) as  AlertDtl_TestName_var
                from tbl_AAC_Inward  as a,tbl_Client as b,tbl_Site as c
                where a.AACINWD_CL_Id=b.CL_Id
                and a.AACINWD_SITE_Id=c.SITE_Id
              	and CONVERT (DATE, a.AACINWD_CollectionDate_dt) >=CONVERT (DATE,'" + fromDate + "') and CONVERT (DATE,a.AACINWD_CollectionDate_dt) <=CONVERT (DATE,'" + ToDate + "') " +
                @"and datediff(d,convert(date,a.AACINWD_CollectionDate_dt),convert(date,a.AACINWD_OutwardDate_dt)) > ((select distinct(TEST_TimeFrame) from tbl_Test as d where Test_RecType_var='AAC' and TEST_TimeFrame is NOT Null)+2)
				and datediff(d,convert(date,a.AACINWD_CollectionDate_dt),convert(date,GETDATE())) > ((select distinct(TEST_TimeFrame) from tbl_Test as d where Test_RecType_var='AAC' and TEST_TimeFrame is NOT Null)+2)

                Union

                select  0 as AlertDtl_EnqNo_int,null as AlertDtl_EnqDate_date,a.AGGTINWD_CollectionDate_dt as AlertDtl_CollctnEnqDate_date,'' as AlertDtl_BillNo_int,null as AlertDtl_BillDate_date,0 as AlertDtl_BillAmt_dec,
                '' as AlertDtl_ProNo_var,0 as AlertDtl_ProNo_int,null as AlertDtl_ProDate_date,AGGTINWD_ReferenceNo_var as AlertDtl_RefNo_var,AGGTINWD_RecordNo_int as AlertDtl_RecNo_int,
                CL_Name_var as AlertDtl_CL_Name_var,SITE_Name_var as AlertDtl_SITE_Name_var,'AGGT' as AlertDtl_RecType_var,AGGTINWD_CL_Id  as AlertDtl_CLId_int,AGGTINWD_SITE_Id  as AlertDtl_SiteId_int,28  as AlertDtl_Alert_Id,
                (select USER_NAME_var from tbl_User x where x.USER_Id IN (select ROUTE_ME_Id from tbl_Route where ROUTE_Id=c.SITE_Route_Id)) as AlertDtl_MEName_var			
                  ,null as AlertDtl_ProOrderDate_date,null as AlertDtl_BillModifiedDate_date,null as AlertDtl_InwdRecDate_date
                ,null as AlertDtl_AppDate_date,null as AlertDtl_EnteredDate_date,null as AlertDtl_CheckedDate_date,null as AlertDtl_OutwrdDate_date,
                (select TEST_Name_var from tbl_Test as d where e.AGGTTEST_TEST_Id=d.TEST_Id ) as AlertDtl_TestName_var
               from tbl_Aggregate_Inward  as a,tbl_Aggregate_Test as e,tbl_Client as b,tbl_Site as c
                where a.AGGTINWD_CL_Id=b.CL_Id
                and a.AGGTINWD_SITE_Id=c.SITE_Id
                and a.AGGTINWD_ReferenceNo_var=e.AGGTTEST_ReferenceNo_var
                and CONVERT (DATE, a.AGGTINWD_CollectionDate_dt) >=CONVERT (DATE,'" + fromDate + "') and CONVERT (DATE,a.AGGTINWD_CollectionDate_dt) <=CONVERT (DATE,'" + ToDate + "') " +
                @"and datediff(d,convert(date,a.AGGTINWD_CollectionDate_dt),convert(date,a.AGGTINWD_OutwardDate_dt)) > ((select distinct(TEST_TimeFrame) from tbl_Test as d where e.AGGTTEST_TEST_Id=d.TEST_Id and TEST_TimeFrame is NOT Null)+2)
				and datediff(d,convert(date,a.AGGTINWD_CollectionDate_dt),convert(date,GETDATE())) > ((select distinct(TEST_TimeFrame) from tbl_Test as d where e.AGGTTEST_TEST_Id=d.TEST_Id and TEST_TimeFrame is NOT Null)+2)

                Union

                 select  0 as AlertDtl_EnqNo_int,null as AlertDtl_EnqDate_date,a.GTINW_CollectionDate_dt as AlertDtl_CollctnEnqDate_date,'' as AlertDtl_BillNo_int,null as AlertDtl_BillDate_date,0 as AlertDtl_BillAmt_dec,
                '' as AlertDtl_ProNo_var,0 as AlertDtl_ProNo_int,null as AlertDtl_ProDate_date,GTINW_RefNo_var as AlertDtl_RefNo_var,GTINW_RecordNo_int as AlertDtl_RecNo_int,
                CL_Name_var as AlertDtl_CL_Name_var,SITE_Name_var as AlertDtl_SITE_Name_var,'GT' as AlertDtl_RecType_var,GTINW_CL_Id  as AlertDtl_CLId_int,GTINW_SITE_Id  as AlertDtl_SiteId_int,28  as AlertDtl_Alert_Id,
                (select USER_NAME_var from tbl_User x where x.USER_Id IN (select ROUTE_ME_Id from tbl_Route where ROUTE_Id=c.SITE_Route_Id)) as AlertDtl_MEName_var			
                  ,null as AlertDtl_ProOrderDate_date,null as AlertDtl_BillModifiedDate_date,null as AlertDtl_InwdRecDate_date
                ,null as AlertDtl_AppDate_date,null as AlertDtl_EnteredDate_date,null as AlertDtl_CheckedDate_date,null as AlertDtl_OutwrdDate_date
                ,(select TEST_Name_var from tbl_Test as d where e.GTTEST_Srno_tint=d.TEST_Id ) as AlertDtl_TestName_var
               from tbl_GT_Inward  as a,tbl_GT_Test as e,tbl_Client as b,tbl_Site as c
                where a.GTINW_CL_Id=b.CL_Id
                and a.GTINW_SITE_Id=c.SITE_Id
                and a.GTINW_RefNo_var=e.GTTEST_RefNo_var
                and CONVERT (DATE, a.GTINW_CollectionDate_dt) >=CONVERT (DATE,'" + fromDate + "') and CONVERT (DATE,a.GTINW_CollectionDate_dt) <=CONVERT (DATE,'" + ToDate + "') " +
                @"and datediff(d,convert(date,a.GTINW_CollectionDate_dt),convert(date,a.GTINW_OutwardDate_dt)) > ((Select distinct(TEST_TimeFrame) from tbl_Test as d where Test_RecType_var='GT' and TEST_TimeFrame is NOT Null)+2)
				and datediff(d,convert(date,a.GTINW_CollectionDate_dt),convert(date,GETDATE())) > ((Select distinct(TEST_TimeFrame) from tbl_Test as d where Test_RecType_var='GT' and TEST_TimeFrame is NOT Null)+2)

                Union

                select  0 as AlertDtl_EnqNo_int,null as AlertDtl_EnqDate_date,a.CCHINWD_CollectionDate_dt as AlertDtl_CollctnEnqDate_date,'' as AlertDtl_BillNo_int,null as AlertDtl_BillDate_date,0 as AlertDtl_BillAmt_dec,
                '' as AlertDtl_ProNo_var,0 as AlertDtl_ProNo_int,null as AlertDtl_ProDate_date,CCHINWD_ReferenceNo_var as AlertDtl_RefNo_var,CCHINWD_RecordNo_int as AlertDtl_RecNo_int,
                CL_Name_var as AlertDtl_CL_Name_var,SITE_Name_var as AlertDtl_SITE_Name_var,'CCH' as AlertDtl_RecType_var,CCHINWD_CL_Id  as AlertDtl_CLId_int,CCHINWD_SITE_Id  as AlertDtl_SiteId_int,28  as AlertDtl_Alert_Id,
                (select USER_NAME_var from tbl_User x where x.USER_Id IN (select ROUTE_ME_Id from tbl_Route where ROUTE_Id=c.SITE_Route_Id)) as AlertDtl_MEName_var			
                  ,null as AlertDtl_ProOrderDate_date,null as AlertDtl_BillModifiedDate_date,null as AlertDtl_InwdRecDate_date
                ,null as AlertDtl_AppDate_date,null as AlertDtl_EnteredDate_date,null as AlertDtl_CheckedDate_date,null as AlertDtl_OutwrdDate_date,
                (select TEST_Name_var from tbl_Test as d where e.CCHTEST_TEST_Id=d.TEST_Id ) as AlertDtl_TestName_var
               from tbl_CementChemical_Inward  as a,tbl_CementChemical_Test as e,tbl_Client as b,tbl_Site as c 
                where a.CCHINWD_CL_Id=b.CL_Id
                and a.CCHINWD_SITE_Id=c.SITE_Id
                and a.CCHINWD_ReferenceNo_var=e.CCHTEST_ReferenceNo_var
                 and CONVERT (DATE, a.CCHINWD_CollectionDate_dt) >=CONVERT (DATE,'" + fromDate + "') and CONVERT (DATE,a.CCHINWD_CollectionDate_dt) <=CONVERT (DATE,'" + ToDate + "') " +
                @"and datediff(d,convert(date,a.CCHINWD_CollectionDate_dt),convert(date,a.CCHINWD_OutwardDate_dt)) > ((Select distinct(TEST_TimeFrame) from tbl_Test as d where Test_RecType_var='CCH' and TEST_TimeFrame is NOT Null)+2)
				and datediff(d,convert(date,a.CCHINWD_CollectionDate_dt),convert(date,GETDATE())) > ((Select distinct(TEST_TimeFrame) from tbl_Test as d where Test_RecType_var='CCH' and TEST_TimeFrame is NOT Null)+2)

                Union

                select  0 as AlertDtl_EnqNo_int,null as AlertDtl_EnqDate_date,a.BTINWD_CollectionDate_dt as AlertDtl_CollctnEnqDate_date,'' as AlertDtl_BillNo_int,null as AlertDtl_BillDate_date,0 as AlertDtl_BillAmt_dec,
                '' as AlertDtl_ProNo_var,0 as AlertDtl_ProNo_int,null as AlertDtl_ProDate_date,BTINWD_ReferenceNo_var as AlertDtl_RefNo_var,BTINWD_RecordNo_int as AlertDtl_RecNo_int,
                CL_Name_var as AlertDtl_CL_Name_var,SITE_Name_var as AlertDtl_SITE_Name_var,'BT-' as AlertDtl_RecType_var,BTINWD_CL_Id  as AlertDtl_CLId_int,BTINWD_SITE_Id  as AlertDtl_SiteId_int,28  as AlertDtl_Alert_Id,
                (select USER_NAME_var from tbl_User x where x.USER_Id IN (select ROUTE_ME_Id from tbl_Route where ROUTE_Id=c.SITE_Route_Id)) as AlertDtl_MEName_var			
                  ,null as AlertDtl_ProOrderDate_date,null as AlertDtl_BillModifiedDate_date,null as AlertDtl_InwdRecDate_date
                ,null as AlertDtl_AppDate_date,null as AlertDtl_EnteredDate_date,null as AlertDtl_CheckedDate_date,null as AlertDtl_OutwrdDate_date,
               (select TEST_Name_var from tbl_Test as d where e.BTTEST_TEST_Id=d.TEST_Id ) as AlertDtl_TestName_var
                from tbl_Brick_Inward  as a,tbl_Brick_Test as e,tbl_Client as b,tbl_Site as c 
                where a.BTINWD_CL_Id=b.CL_Id
                and a.BTINWD_SITE_Id=c.SITE_Id
                and a.BTINWD_ReferenceNo_var=e.BTTEST_ReferenceNo_var
                and CONVERT (DATE, a.BTINWD_CollectionDate_dt) >=CONVERT (DATE,'" + fromDate + "') and CONVERT (DATE,a.BTINWD_CollectionDate_dt) <=CONVERT (DATE,'" + ToDate + "') " +
                @"and datediff(d,convert(date,a.BTINWD_CollectionDate_dt),convert(date,a.BTINWD_OutwardDate_dt))>((select TEST_TimeFrame from tbl_Test as d where e.BTTEST_TEST_Id=d.TEST_Id and d.TEST_TimeFrame is not null)+2)
				and datediff(d,convert(date,a.BTINWD_CollectionDate_dt),convert(date,GETDATE())) > ((select TEST_TimeFrame from tbl_Test as d where e.BTTEST_TEST_Id=d.TEST_Id and d.TEST_TimeFrame is not null)+2)

                Union

                select  0 as AlertDtl_EnqNo_int,null as AlertDtl_EnqDate_date,a.CEMTINWD_CollectionDate_dt as AlertDtl_CollctnEnqDate_date,'' as AlertDtl_BillNo_int,null as AlertDtl_BillDate_date,0 as AlertDtl_BillAmt_dec,
                '' as AlertDtl_ProNo_var,0 as AlertDtl_ProNo_int,null as AlertDtl_ProDate_date,CEMTINWD_ReferenceNo_var as AlertDtl_RefNo_var,CEMTINWD_RecordNo_int as AlertDtl_RecNo_int,CL_Name_var as AlertDtl_CL_Name_var,SITE_Name_var as AlertDtl_SITE_Name_var,
                'CEMT' as AlertDtl_RecType_var,CEMTINWD_CL_Id  as AlertDtl_CLId_int,CEMTINWD_SITE_Id  as AlertDtl_SiteId_int,28  as AlertDtl_Alert_Id,
                (select USER_NAME_var from tbl_User x where x.USER_Id IN (select ROUTE_ME_Id from tbl_Route where ROUTE_Id=c.SITE_Route_Id)) as AlertDtl_MEName_var			
                  ,null as AlertDtl_ProOrderDate_date,null as AlertDtl_BillModifiedDate_date,null as AlertDtl_InwdRecDate_date
                ,null as AlertDtl_AppDate_date,null as AlertDtl_EnteredDate_date,null as AlertDtl_CheckedDate_date,null as AlertDtl_OutwrdDate_date,
               (select TEST_Name_var from tbl_Test as d where e.CEMTTEST_TEST_Id=d.TEST_Id ) as AlertDtl_TestName_var
                from tbl_Cement_Inward  as a,tbl_Cement_Test as e,tbl_Client as b,tbl_Site as c 
                where a.CEMTINWD_CL_Id=b.CL_Id
                and a.CEMTINWD_SITE_Id=c.SITE_Id
                and a.CEMTINWD_ReferenceNo_var=e.CEMTTEST_ReferenceNo_var
                and CONVERT (DATE, a.CEMTINWD_CollectionDate_dt) >=CONVERT (DATE,'" + fromDate + "') and CONVERT (DATE,a.CEMTINWD_CollectionDate_dt) <=CONVERT (DATE,'" + ToDate + "') " +
                @"and datediff(d,convert(date,a.CEMTINWD_CollectionDate_dt),convert(date,a.CEMTINWD_OutwardDate_dt))>((select TEST_TimeFrame from tbl_Test as d where e.CEMTTEST_TEST_Id=d.TEST_Id and d.TEST_TimeFrame is not null)+2)
				and datediff(d,convert(date,a.CEMTINWD_CollectionDate_dt),convert(date,GETDATE())) >((select TEST_TimeFrame from tbl_Test as d where e.CEMTTEST_TEST_Id=d.TEST_Id and d.TEST_TimeFrame is not null)+2)

                Union

                select  0 as AlertDtl_EnqNo_int,null as AlertDtl_EnqDate_date,a.CRINWD_CollectionDate_dt as AlertDtl_CollctnEnqDate_date,'' as AlertDtl_BillNo_int,null as AlertDtl_BillDate_date,0 as AlertDtl_BillAmt_dec,
                '' as AlertDtl_ProNo_var,0 as AlertDtl_ProNo_int,null as AlertDtl_ProDate_date,CRINWD_ReferenceNo_var as AlertDtl_RefNo_var,CRINWD_RecordNo_int as AlertDtl_RecNo_int,
                CL_Name_var as AlertDtl_CL_Name_var,SITE_Name_var as AlertDtl_SITE_Name_var,'CR' as AlertDtl_RecType_var,CRINWD_CL_Id  as AlertDtl_CLId_int,CRINWD_SITE_Id  as AlertDtl_SiteId_int,28  as AlertDtl_Alert_Id,
                (select USER_NAME_var from tbl_User x where x.USER_Id IN (select ROUTE_ME_Id from tbl_Route where ROUTE_Id=c.SITE_Route_Id)) as AlertDtl_MEName_var			
                  ,null as AlertDtl_ProOrderDate_date,null as AlertDtl_BillModifiedDate_date,null as AlertDtl_InwdRecDate_date
                ,null as AlertDtl_AppDate_date,null as AlertDtl_EnteredDate_date,null as AlertDtl_CheckedDate_date,null as AlertDtl_OutwrdDate_date,
                (select TEST_Name_var from tbl_Test as d where a.CRINWD_TestId_int=d.TEST_Id ) as AlertDtl_TestName_var
                from tbl_Core_Inward  as a,tbl_Client as b,tbl_Site as c 
                where a.CRINWD_CL_Id=b.CL_Id
                and a.CRINWD_SITE_Id=c.SITE_Id
                and CONVERT (DATE, a.CRINWD_CollectionDate_dt) >=CONVERT (DATE,'" + fromDate + "') and CONVERT (DATE,a.CRINWD_CollectionDate_dt) <=CONVERT (DATE,'" + ToDate + "') " +
                @"and datediff(d,convert(date,a.CRINWD_CollectionDate_dt),convert(date,a.CRINWD_OutwardDate_dt))>((Select distinct(TEST_TimeFrame) from tbl_Test as d where Test_RecType_var='CR' and TEST_TimeFrame is NOT Null)+2)
				and datediff(d,convert(date,a.CRINWD_CollectionDate_dt),convert(date,GETDATE())) > ((Select distinct(TEST_TimeFrame) from tbl_Test as d where Test_RecType_var='CR' and TEST_TimeFrame is NOT Null)+2)

                Union

                select  0 as AlertDtl_EnqNo_int,null as AlertDtl_EnqDate_date,a.CoreCutINWD_CollectionDate_dt as AlertDtl_CollctnEnqDate_date,'' as AlertDtl_BillNo_int,null as AlertDtl_BillDate_date,0 as AlertDtl_BillAmt_dec,
                '' as AlertDtl_ProNo_var,0 as AlertDtl_ProNo_int,null as AlertDtl_ProDate_date,CORECUTINWD_ReferenceNo_var as AlertDtl_RefNo_var,CORECUTINWD_RecordNo_int as AlertDtl_RecNo_int,
                CL_Name_var as AlertDtl_CL_Name_var,SITE_Name_var as AlertDtl_SITE_Name_var,'CORECUT' as AlertDtl_RecType_var,CORECUTINWD_CL_Id  as AlertDtl_CLId_int,CORECUTINWD_SITE_Id  as AlertDtl_SiteId_int,28  as AlertDtl_Alert_Id,
                (select USER_NAME_var from tbl_User x where x.USER_Id IN (select ROUTE_ME_Id from tbl_Route where ROUTE_Id=c.SITE_Route_Id)) as AlertDtl_MEName_var			
                  ,null as AlertDtl_ProOrderDate_date,null as AlertDtl_BillModifiedDate_date,null as AlertDtl_InwdRecDate_date
                ,null as AlertDtl_AppDate_date,null as AlertDtl_EnteredDate_date,null as AlertDtl_CheckedDate_date,null as AlertDtl_OutwrdDate_date,
                 (select TEST_Name_var from tbl_Test as d where a.CoreCutINWD_TEST_Id=d.TEST_Id ) as AlertDtl_TestName_var
               from tbl_CoreCutting_Inward  as a,tbl_Client as b,tbl_Site as c 
                where a.CoreCutINWD_CL_Id=b.CL_Id
                and a.CoreCutINWD_SITE_Id=c.SITE_Id
                and CONVERT (DATE, a.CoreCutINWD_CollectionDate_dt) >=CONVERT (DATE,'" + fromDate + "') and CONVERT (DATE,a.CoreCutINWD_CollectionDate_dt) <=CONVERT (DATE,'" + ToDate + "') " +
                @"and datediff(d,convert(date,a.CoreCutINWD_CollectionDate_dt),convert(date,a.CoreCutINWD_OutwardDate_dt))>((Select distinct(TEST_TimeFrame) from tbl_Test as d where Test_RecType_var='CORECUT' and TEST_TimeFrame is NOT Null)+2)
				and datediff(d,convert(date,a.CoreCutINWD_CollectionDate_dt),convert(date,GETDATE())) > ((Select distinct(TEST_TimeFrame) from tbl_Test as d where Test_RecType_var='CORECUT' and TEST_TimeFrame is NOT Null)+2)

                Union

                select  0 as AlertDtl_EnqNo_int,null as AlertDtl_EnqDate_date,a.CTINWD_CollectionDate_dt as AlertDtl_CollctnEnqDate_date,'' as AlertDtl_BillNo_int,null as AlertDtl_BillDate_date,0 as AlertDtl_BillAmt_dec,
                '' as AlertDtl_ProNo_var,0 as AlertDtl_ProNo_int,null as AlertDtl_ProDate_date,CTINWD_ReferenceNo_var as AlertDtl_RefNo_var,CTINWD_RecordNo_int as AlertDtl_RecNo_int,CL_Name_var as AlertDtl_CL_Name_var,SITE_Name_var as AlertDtl_SITE_Name_var,
                'CT' as AlertDtl_RecType_var,CTINWD_CL_Id  as AlertDtl_CLId_int,CTINWD_SITE_Id  as AlertDtl_SiteId_int,28  as AlertDtl_Alert_Id,
                (select USER_NAME_var from tbl_User x where x.USER_Id IN (select ROUTE_ME_Id from tbl_Route where ROUTE_Id=c.SITE_Route_Id)) as AlertDtl_MEName_var			
                  ,null as AlertDtl_ProOrderDate_date,null as AlertDtl_BillModifiedDate_date,null as AlertDtl_InwdRecDate_date
                ,null as AlertDtl_AppDate_date,null as AlertDtl_EnteredDate_date,null as AlertDtl_CheckedDate_date,null as AlertDtl_OutwrdDate_date,
                  (select TEST_Name_var from tbl_Test as d where a.CTINWD_TEST_Id=d.TEST_Id ) as AlertDtl_TestName_var
               from tbl_Cube_Inward  as a,tbl_Client as b,tbl_Site as c 
                where a.CTINWD_CL_Id=b.CL_Id
                and a.CTINWD_SITE_Id=c.SITE_Id
                and CONVERT (DATE, a.CTINWD_CollectionDate_dt) >=CONVERT (DATE,'" + fromDate + "') and CONVERT (DATE,a.CTINWD_CollectionDate_dt) <=CONVERT (DATE,'" + ToDate + "') " +
                @"and datediff(d,convert(date,a.CTINWD_CollectionDate_dt),convert(date,a.CTINWD_OutwardDate_dt))>((Select distinct(TEST_TimeFrame) from tbl_Test as d where Test_RecType_var='CT' and TEST_TimeFrame is NOT Null)+2)
				and datediff(d,convert(date,a.CTINWD_CollectionDate_dt),convert(date,GETDATE())) > ((Select distinct(TEST_TimeFrame) from tbl_Test as d where Test_RecType_var='CT' and TEST_TimeFrame is NOT Null)+2)

                Union

                select  0 as AlertDtl_EnqNo_int,null as AlertDtl_EnqDate_date,a.FlyAshINWD_CollectionDate_dt as AlertDtl_CollctnEnqDate_date,'' as AlertDtl_BillNo_int,null as AlertDtl_BillDate_date,0 as AlertDtl_BillAmt_dec,
                '' as AlertDtl_ProNo_var,0 as AlertDtl_ProNo_int,null as AlertDtl_ProDate_date,FlyAshINWD_ReferenceNo_var as AlertDtl_RefNo_var,FlyAshINWD_RecordNo_int as AlertDtl_RecNo_int,
                CL_Name_var as AlertDtl_CL_Name_var,SITE_Name_var as AlertDtl_SITE_Name_var,'FLYASH' as AlertDtl_RecType_var,FlyAshINWD_CL_Id  as AlertDtl_CLId_int,FlyAshINWD_SITE_Id  as AlertDtl_SiteId_int,28  as AlertDtl_Alert_Id,
                (select USER_NAME_var from tbl_User x where x.USER_Id IN (select ROUTE_ME_Id from tbl_Route where ROUTE_Id=c.SITE_Route_Id)) as AlertDtl_MEName_var			
                  ,null as AlertDtl_ProOrderDate_date,null as AlertDtl_BillModifiedDate_date,null as AlertDtl_InwdRecDate_date
                ,null as AlertDtl_AppDate_date,null as AlertDtl_EnteredDate_date,null as AlertDtl_CheckedDate_date,null as AlertDtl_OutwrdDate_date,
               (select TEST_Name_var from tbl_Test as d where e.FlyAshTEST_TEST_Id=d.TEST_Id ) as AlertDtl_TestName_var
                from tbl_FlyAsh_Inward  as a,tbl_FlyAsh_Test as e,tbl_Client as b,tbl_Site as c 
                where a.FlyAshINWD_CL_Id=b.CL_Id
                and a.FlyAshINWD_SITE_Id=c.SITE_Id
                and a.FlyAshINWD_ReferenceNo_var=e.FLYASHTEST_ReferenceNo_var
                and CONVERT (DATE, a.FlyAshINWD_CollectionDate_dt) >=CONVERT (DATE,'" + fromDate + "') and CONVERT (DATE,a.FlyAshINWD_CollectionDate_dt) <=CONVERT (DATE,'" + ToDate + "') " +
                @"and datediff(d,convert(date,a.FlyAshINWD_CollectionDate_dt),convert(date,a.FlyAshINWD_OutwardDate_dt))>((select TEST_TimeFrame from tbl_Test as d where E.FLYASHTEST_TEST_Id=d.TEST_Id and d.TEST_TimeFrame is not null)+2)
				and datediff(d,convert(date,a.FlyAshINWD_CollectionDate_dt),convert(date,GETDATE())) > ((select TEST_TimeFrame from tbl_Test as d where E.FLYASHTEST_TEST_Id=d.TEST_Id and d.TEST_TimeFrame is not null)+2)

                Union

                 select  0 as AlertDtl_EnqNo_int,null as AlertDtl_EnqDate_date,a.MFINWD_CollectionDate_dt as AlertDtl_CollctnEnqDate_date,'' as AlertDtl_BillNo_int,null as AlertDtl_BillDate_date,0 as AlertDtl_BillAmt_dec,
                '' as AlertDtl_ProNo_var,0 as AlertDtl_ProNo_int,null as AlertDtl_ProDate_date,MFINWD_ReferenceNo_var as AlertDtl_RefNo_var,MFINWD_RecordNo_int as AlertDtl_RecNo_int,
                CL_Name_var as AlertDtl_CL_Name_var,SITE_Name_var as AlertDtl_SITE_Name_var,'MF' as AlertDtl_RecType_var,MFINWD_CL_Id  as AlertDtl_CLId_int,MFINWD_SITE_Id  as AlertDtl_SiteId_int,28  as AlertDtl_Alert_Id,
                 (select USER_NAME_var from tbl_User x where x.USER_Id IN (select ROUTE_ME_Id from tbl_Route where ROUTE_Id=c.SITE_Route_Id)) as AlertDtl_MEName_var			
                  ,null as AlertDtl_ProOrderDate_date,null as AlertDtl_BillModifiedDate_date,null as AlertDtl_InwdRecDate_date
                ,null as AlertDtl_AppDate_date,null as AlertDtl_EnteredDate_date,null as AlertDtl_CheckedDate_date,null as AlertDtl_OutwrdDate_date,
                (select TEST_Name_var from tbl_Test as d where a.MFINWD_TestId_int=d.TEST_Id ) as AlertDtl_TestName_var
              from tbl_MixDesign_Inward  as a,tbl_Client as b,tbl_Site as c 
                where a.MFINWD_CL_Id=b.CL_Id
                and a.MFINWD_SITE_Id=c.SITE_Id
                and CONVERT (DATE, a.MFINWD_CollectionDate_dt) >=CONVERT (DATE,'" + fromDate + "') and CONVERT (DATE,a.MFINWD_CollectionDate_dt) <=CONVERT (DATE,'" + ToDate + "') " +
                @"and datediff(d,convert(date,a.MFINWD_CollectionDate_dt),convert(date,a.MFINWD_OutwardDate_dt))>((select TEST_TimeFrame from tbl_Test as d where a.MFINWD_TestId_int=d.TEST_Id and d.TEST_TimeFrame is not null)+2)
				and datediff(d,convert(date,a.MFINWD_CollectionDate_dt),convert(date,GETDATE())) > ((select TEST_TimeFrame from tbl_Test as d where a.MFINWD_TestId_int=d.TEST_Id and d.TEST_TimeFrame is not null)+2)

                Union

                select  0 as AlertDtl_EnqNo_int,null as AlertDtl_EnqDate_date,a.NDTINWD_CollectionDate_dt as AlertDtl_CollctnEnqDate_date,'' as AlertDtl_BillNo_int,null as AlertDtl_BillDate_date,0 as AlertDtl_BillAmt_dec,
                '' as AlertDtl_ProNo_var,0 as AlertDtl_ProNo_int,null as AlertDtl_ProDate_date,NDTINWD_ReferenceNo_var as AlertDtl_RefNo_var,NDTINWD_RecordNo_int as AlertDtl_RecNo_int,
                CL_Name_var as AlertDtl_CL_Name_var,SITE_Name_var as AlertDtl_SITE_Name_var,'NDT' as AlertDtl_RecType_var,NDTINWD_CL_Id  as AlertDtl_CLId_int,NDTINWD_SITE_Id  as AlertDtl_SiteId_int,28  as AlertDtl_Alert_Id,
               (select USER_NAME_var from tbl_User x where x.USER_Id IN (select ROUTE_ME_Id from tbl_Route where ROUTE_Id=c.SITE_Route_Id)) as AlertDtl_MEName_var			
                  ,null as AlertDtl_ProOrderDate_date,null as AlertDtl_BillModifiedDate_date,null as AlertDtl_InwdRecDate_date
                ,null as AlertDtl_AppDate_date,null as AlertDtl_EnteredDate_date,null as AlertDtl_CheckedDate_date,null as AlertDtl_OutwrdDate_date,
                     (select TEST_Name_var from tbl_Test as d where e.NDTTEST_TEST_Id=d.TEST_Id ) as AlertDtl_TestName_var
				from tbl_NDT_Inward  as a,tbl_NDT_Test as e,tbl_Client as b,tbl_Site as c 
                where a.NDTINWD_CL_Id=b.CL_Id
                and a.NDTINWD_SITE_Id=c.SITE_Id
                and a.NDTINWD_ReferenceNo_var=e.NDTTEST_RefNo_var
                and CONVERT (DATE, a.NDTINWD_CollectionDate_dt) >=CONVERT (DATE,'" + fromDate + "') and CONVERT (DATE,a.NDTINWD_CollectionDate_dt) <=CONVERT (DATE,'" + ToDate + "') " +
                @"and datediff(d,convert(date,a.NDTINWD_CollectionDate_dt),convert(date,a.NDTINWD_OutwardDate_dt))>((Select distinct(TEST_TimeFrame) from tbl_Test as d where Test_RecType_var='NDT' and TEST_TimeFrame is NOT Null)+2)
				and datediff(d,convert(date,a.NDTINWD_CollectionDate_dt),convert(date,GETDATE())) > ((Select distinct(TEST_TimeFrame) from tbl_Test as d where Test_RecType_var='NDT' and TEST_TimeFrame is NOT Null)+2)

                Union

                select  0 as AlertDtl_EnqNo_int,null as AlertDtl_EnqDate_date,a.OTINWD_CollectionDate_dt as AlertDtl_CollctnEnqDate_date,'' as AlertDtl_BillNo_int,null as AlertDtl_BillDate_date,0 as AlertDtl_BillAmt_dec,
                '' as AlertDtl_ProNo_var,0 as AlertDtl_ProNo_int,null as AlertDtl_ProDate_date,OTINWD_ReferenceNo_var as AlertDtl_RefNo_var,OTINWD_RecordNo_int as AlertDtl_RecNo_int,
                CL_Name_var as AlertDtl_CL_Name_var,SITE_Name_var as AlertDtl_SITE_Name_var,'OT' as AlertDtl_RecType_var,OTINWD_CL_Id  as AlertDtl_CLId_int,OTINWD_SITE_Id  as AlertDtl_SiteId_int,28  as AlertDtl_Alert_Id,
                (select USER_NAME_var from tbl_User x where x.USER_Id IN (select ROUTE_ME_Id from tbl_Route where ROUTE_Id=c.SITE_Route_Id)) as AlertDtl_MEName_var			
                  ,null as AlertDtl_ProOrderDate_date,null as AlertDtl_BillModifiedDate_date,null as AlertDtl_InwdRecDate_date
                ,null as AlertDtl_AppDate_date,null as AlertDtl_EnteredDate_date,null as AlertDtl_CheckedDate_date,null as AlertDtl_OutwrdDate_date,
               (select TEST_Name_var from tbl_Test as d where e.OTTEST_TEST_Id=d.TEST_Id ) as AlertDtl_TestName_var
                from tbl_Other_Inward  as a,tbl_Other_Test as e,tbl_Client as b,tbl_Site as c 
                where a.OTINWD_CL_Id=b.CL_Id
                and a.OTINWD_SITE_Id=c.SITE_Id
                and a.OTINWD_ReferenceNo_var=e.OTTEST_ReferenceNo_var
                and CONVERT (DATE, a.OTINWD_CollectionDate_dt) >=CONVERT (DATE,'" + fromDate + "') and CONVERT (DATE,a.OTINWD_CollectionDate_dt) <=CONVERT (DATE,'" + ToDate + "') " +
                @"and datediff(d,convert(date,a.OTINWD_CollectionDate_dt),convert(date,a.OTINWD_OutwardDate_dt))>((Select distinct(TEST_TimeFrame) from tbl_Test as d where Test_RecType_var='OT' and TEST_TimeFrame is NOT Null)+2)
				and datediff(d,convert(date,a.OTINWD_CollectionDate_dt),convert(date,GETDATE())) >((Select distinct(TEST_TimeFrame) from tbl_Test as d where Test_RecType_var='OT' and TEST_TimeFrame is NOT Null)+2)

                Union

              select  0 as AlertDtl_EnqNo_int,null as AlertDtl_EnqDate_date,a.PTINWD_CollectionDate_dt as AlertDtl_CollctnEnqDate_date,'' as AlertDtl_BillNo_int,null as AlertDtl_BillDate_date,0 as AlertDtl_BillAmt_dec,
                '' as AlertDtl_ProNo_var,0 as AlertDtl_ProNo_int,null as AlertDtl_ProDate_date,PTINWD_ReferenceNo_var as AlertDtl_RefNo_var,PTINWD_RecordNo_int as AlertDtl_RecNo_int
                ,CL_Name_var as AlertDtl_CL_Name_var,SITE_Name_var as AlertDtl_SITE_Name_var,'PT' as AlertDtl_RecType_var,PTINWD_CL_Id  as AlertDtl_CLId_int,PTINWD_SITE_Id  as AlertDtl_SiteId_int,28  as AlertDtl_Alert_Id,
                (select USER_NAME_var from tbl_User x where x.USER_Id IN (select ROUTE_ME_Id from tbl_Route where ROUTE_Id=c.SITE_Route_Id)) as AlertDtl_MEName_var			
                  ,null as AlertDtl_ProOrderDate_date,null as AlertDtl_BillModifiedDate_date,null as AlertDtl_InwdRecDate_date
                ,null as AlertDtl_AppDate_date,null as AlertDtl_EnteredDate_date,null as AlertDtl_CheckedDate_date,null as AlertDtl_OutwrdDate_date,
                 (select TEST_Name_var from tbl_Test as d where a.PTINWD_TEST_Id=d.TEST_Id ) as AlertDtl_TestName_var 
               from tbl_Pavement_Inward  as a,tbl_Client as b,tbl_Site as c
                where a.PTINWD_CL_Id=b.CL_Id
                and a.PTINWD_SITE_Id=c.SITE_Id
                and CONVERT (DATE, a.PTINWD_CollectionDate_dt) >=CONVERT (DATE,'" + fromDate + "') and CONVERT (DATE,a.PTINWD_CollectionDate_dt) <=CONVERT (DATE,'" + ToDate + "') " +
                @"and datediff(d,convert(date,a.PTINWD_CollectionDate_dt),convert(date,a.PTINWD_OutwardDate_dt))>((select TEST_TimeFrame from tbl_Test as d where a.PTINWD_TEST_Id=d.TEST_Id and d.TEST_TimeFrame is not null)+2)
				and datediff(d,convert(date,a.PTINWD_CollectionDate_dt),convert(date,GETDATE())) > ((select TEST_TimeFrame from tbl_Test as d where a.PTINWD_TEST_Id=d.TEST_Id and d.TEST_TimeFrame is not null)+2)

                Union

                select  0 as AlertDtl_EnqNo_int,null as AlertDtl_EnqDate_date,a.PILEINWD_CollectionDate_dt as AlertDtl_CollctnEnqDate_date,'' as AlertDtl_BillNo_int,null as AlertDtl_BillDate_date,0 as AlertDtl_BillAmt_dec,
                '' as AlertDtl_ProNo_var,0 as AlertDtl_ProNo_int,null as AlertDtl_ProDate_date,PILEINWD_ReferenceNo_var as AlertDtl_RefNo_var,PILEINWD_RecordNo_int as AlertDtl_RecNo_int,
                CL_Name_var as AlertDtl_CL_Name_var,SITE_Name_var as AlertDtl_SITE_Name_var,'PILE' as AlertDtl_RecType_var,PILEINWD_CL_Id  as AlertDtl_CLId_int,PILEINWD_SITE_Id  as AlertDtl_SiteId_int,28  as AlertDtl_Alert_Id,
                (select USER_NAME_var from tbl_User x where x.USER_Id IN (select ROUTE_ME_Id from tbl_Route where ROUTE_Id=c.SITE_Route_Id)) as AlertDtl_MEName_var			
                  ,null as AlertDtl_ProOrderDate_date,null as AlertDtl_BillModifiedDate_date,null as AlertDtl_InwdRecDate_date
                ,null as AlertDtl_AppDate_date,null as AlertDtl_EnteredDate_date,null as AlertDtl_CheckedDate_date,null as AlertDtl_OutwrdDate_date,
                 (select TEST_Name_var from tbl_Test as d where a.PILEINWD_TEST_Id=d.TEST_Id ) as AlertDtl_TestName_var 
                from tbl_Pile_Inward  as a,tbl_Client as b,tbl_Site as c
                where a.PILEINWD_CL_Id=b.CL_Id
                and a.PILEINWD_SITE_Id=c.SITE_Id
                and CONVERT (DATE, a.PILEINWD_CollectionDate_dt) >=CONVERT (DATE,'" + fromDate + "') and CONVERT (DATE,a.PILEINWD_CollectionDate_dt) <=CONVERT (DATE,'" + ToDate + "') " +
                @"and datediff(d,convert(date,a.PILEINWD_CollectionDate_dt),convert(date,a.PILEINWD_OutwardDate_dt))>((Select distinct(TEST_TimeFrame) from tbl_Test as d where Test_RecType_var='PILE' and TEST_TimeFrame is NOT Null)+2)
				and datediff(d,convert(date,a.PILEINWD_CollectionDate_dt),convert(date,GETDATE())) > ((Select distinct(TEST_TimeFrame) from tbl_Test as d where Test_RecType_var='PILE' and TEST_TimeFrame is NOT Null)+2)

                Union

               select  0 as AlertDtl_EnqNo_int,null as AlertDtl_EnqDate_date,a.SOINWD_CollectionDate_dt as AlertDtl_CollctnEnqDate_date,'' as AlertDtl_BillNo_int,null as AlertDtl_BillDate_date,0 as AlertDtl_BillAmt_dec,
                '' as AlertDtl_ProNo_var,0 as AlertDtl_ProNo_int,null as AlertDtl_ProDate_date,SOINWD_ReferenceNo_var as AlertDtl_RefNo_var,SOINWD_RecordNo_int as AlertDtl_RecNo_int,
                CL_Name_var as AlertDtl_CL_Name_var,SITE_Name_var as AlertDtl_SITE_Name_var,'SO' as AlertDtl_RecType_var,SOINWD_CL_Id  as AlertDtl_CLId_int,SOINWD_SITE_Id  as AlertDtl_SiteId_int,28  as AlertDtl_Alert_Id,
                (select USER_NAME_var from tbl_User x where x.USER_Id IN (select ROUTE_ME_Id from tbl_Route where ROUTE_Id=c.SITE_Route_Id)) as AlertDtl_MEName_var			
                  ,null as AlertDtl_ProOrderDate_date,null as AlertDtl_BillModifiedDate_date,null as AlertDtl_InwdRecDate_date
                ,null as AlertDtl_AppDate_date,null as AlertDtl_EnteredDate_date,null as AlertDtl_CheckedDate_date,null as AlertDtl_OutwrdDate_date,
                (select TEST_Name_var from tbl_Test as d where e.SOTEST_TEST_int=d.TEST_Id ) as AlertDtl_TestName_var
               from tbl_Soil_Inward  as a,tbl_Soil_Test as e,tbl_Client as b,tbl_Site as c
                where a.SOINWD_CL_Id=b.CL_Id
                and a.SOINWD_SITE_Id=c.SITE_Id
                and a.SOINWD_ReferenceNo_var=e.SOTEST_ReferenceNo_var
                and CONVERT (DATE, a.SOINWD_CollectionDate_dt) >=CONVERT (DATE,'" + fromDate + "') and CONVERT (DATE,a.SOINWD_CollectionDate_dt) <=CONVERT (DATE,'" + ToDate + "') " +
                @"and datediff(d,convert(date,a.SOINWD_CollectionDate_dt),convert(date,a.SOINWD_OutwardDate_dt))>((select TEST_TimeFrame from tbl_Test as d where E.SOTEST_TEST_int=d.TEST_Id and d.TEST_TimeFrame is not null)+2)
				and datediff(d,convert(date,a.SOINWD_CollectionDate_dt),convert(date,GETDATE())) > ((select TEST_TimeFrame from tbl_Test as d where E.SOTEST_TEST_int=d.TEST_Id and d.TEST_TimeFrame is not null)+2)

                Union

                 select  0 as AlertDtl_EnqNo_int,null as AlertDtl_EnqDate_date,a.SolidINWD_CollectionDate_dt as AlertDtl_CollctnEnqDate_date,'' as AlertDtl_BillNo_int,null as AlertDtl_BillDate_date,0 as AlertDtl_BillAmt_dec,
                '' as AlertDtl_ProNo_var,0 as AlertDtl_ProNo_int,null as AlertDtl_ProDate_date,SolidINWD_ReferenceNo_var as AlertDtl_RefNo_var,SolidINWD_RecordNo_int as AlertDtl_RecNo_int,
                CL_Name_var as AlertDtl_CL_Name_var,SITE_Name_var as AlertDtl_SITE_Name_var,'SOLID' as AlertDtl_RecType_var,SolidINWD_CL_Id  as AlertDtl_CLId_int,SolidINWD_SITE_Id  as AlertDtl_SiteId_int,28  as AlertDtl_Alert_Id,
                (select USER_NAME_var from tbl_User x where x.USER_Id IN (select ROUTE_ME_Id from tbl_Route where ROUTE_Id=c.SITE_Route_Id)) as AlertDtl_MEName_var			
                  ,null as AlertDtl_ProOrderDate_date,null as AlertDtl_BillModifiedDate_date,null as AlertDtl_InwdRecDate_date
                ,null as AlertDtl_AppDate_date,null as AlertDtl_EnteredDate_date,null as AlertDtl_CheckedDate_date,null as AlertDtl_OutwrdDate_date,
                (select TEST_Name_var from tbl_Test as d where a.SolidINWD_TEST_Id=d.TEST_Id ) as AlertDtl_TestName_var
               from tbl_Solid_Inward  as a,tbl_Client as b,tbl_Site as c
                where a.SolidINWD_CL_Id=b.CL_Id
                and a.SolidINWD_SITE_Id=c.SITE_Id
                and CONVERT (DATE, a.SolidINWD_CollectionDate_dt) >=CONVERT (DATE,'" + fromDate + "') and CONVERT (DATE,a.SolidINWD_CollectionDate_dt) <=CONVERT (DATE,'" + ToDate + "') " +
                @"and datediff(d,convert(date,a.SolidINWD_CollectionDate_dt),convert(date,a.SolidINWD_OutwardDate_dt))>((select TEST_TimeFrame from tbl_Test as d where a.SolidINWD_TEST_Id=d.TEST_Id and d.TEST_TimeFrame is not null)+2)
				and datediff(d,convert(date,a.SolidINWD_CollectionDate_dt),convert(date,GETDATE())) >((select TEST_TimeFrame from tbl_Test as d where a.SolidINWD_TEST_Id=d.TEST_Id and d.TEST_TimeFrame is not null)+2)

                Union

                select  0 as AlertDtl_EnqNo_int,null as AlertDtl_EnqDate_date,a.STINWD_CollectionDate_dt as AlertDtl_CollctnEnqDate_date,'' as AlertDtl_BillNo_int,null as AlertDtl_BillDate_date,0 as AlertDtl_BillAmt_dec,
                '' as AlertDtl_ProNo_var,0 as AlertDtl_ProNo_int,null as AlertDtl_ProDate_date,STINWD_ReferenceNo_var as AlertDtl_RefNo_var,STINWD_RecordNo_int as AlertDtl_RecNo_int
                ,CL_Name_var as AlertDtl_CL_Name_var,SITE_Name_var as AlertDtl_SITE_Name_var,'ST' as AlertDtl_RecType_var,STINWD_CL_Id  as AlertDtl_CLId_int,STINWD_SITE_Id  as AlertDtl_SiteId_int,28  as AlertDtl_Alert_Id,
                (select USER_NAME_var from tbl_User x where x.USER_Id IN (select ROUTE_ME_Id from tbl_Route where ROUTE_Id=c.SITE_Route_Id)) as AlertDtl_MEName_var			
                  ,null as AlertDtl_ProOrderDate_date,null as AlertDtl_BillModifiedDate_date,null as AlertDtl_InwdRecDate_date
                ,null as AlertDtl_AppDate_date,null as AlertDtl_EnteredDate_date,null as AlertDtl_CheckedDate_date,null as AlertDtl_OutwrdDate_date,
                (select TEST_Name_var from tbl_Test as d where e.STTEST_TEST_id=d.TEST_Id ) as AlertDtl_TestName_var
                 from tbl_Steel_Inward  as a,tbl_Steel_Test as e,tbl_Client as b,tbl_Site as c
                where a.STINWD_CL_Id=b.CL_Id
                and a.STINWD_SITE_Id=c.SITE_Id
                and a.STINWD_ReferenceNo_var=e.STTEST_ReferenceNo_var
                and CONVERT (DATE, a.STINWD_CollectionDate_dt) >=CONVERT (DATE,'" + fromDate + "') and CONVERT (DATE,a.STINWD_CollectionDate_dt) <=CONVERT (DATE,'" + ToDate + "') " +
                @"and datediff(d,convert(date,a.STINWD_CollectionDate_dt),convert(date,a.STINWD_OutwardDate_dt))>((Select distinct(TEST_TimeFrame) from tbl_Test as d where Test_RecType_var='ST' and TEST_TimeFrame is NOT Null)+2)
				and datediff(d,convert(date,a.STINWD_CollectionDate_dt),convert(date,GETDATE())) >((Select distinct(TEST_TimeFrame) from tbl_Test as d where Test_RecType_var='ST' and TEST_TimeFrame is NOT Null)+2)

                Union

                 select  0 as AlertDtl_EnqNo_int,null as AlertDtl_EnqDate_date,a.STCINWD_CollectionDate_dt as AlertDtl_CollctnEnqDate_date,'' as AlertDtl_BillNo_int,null as AlertDtl_BillDate_date,0 as AlertDtl_BillAmt_dec,
                '' as AlertDtl_ProNo_var,0 as AlertDtl_ProNo_int,null as AlertDtl_ProDate_date,STCINWD_ReferenceNo_var as AlertDtl_RefNo_var,STCINWD_RecordNo_int as AlertDtl_RecNo_int,
                CL_Name_var as AlertDtl_CL_Name_var,SITE_Name_var as AlertDtl_SITE_Name_var,'STC' as AlertDtl_RecType_var,STCINWD_CL_Id  as AlertDtl_CLId_int,STCINWD_SITE_Id  as AlertDtl_SiteId_int,28  as AlertDtl_Alert_Id,
                 (select USER_NAME_var from tbl_User x where x.USER_Id IN (select ROUTE_ME_Id from tbl_Route where ROUTE_Id=c.SITE_Route_Id)) as AlertDtl_MEName_var			
                  ,null as AlertDtl_ProOrderDate_date,null as AlertDtl_BillModifiedDate_date,null as AlertDtl_InwdRecDate_date
                ,null as AlertDtl_AppDate_date,null as AlertDtl_EnteredDate_date,null as AlertDtl_CheckedDate_date,null as AlertDtl_OutwrdDate_date,
               (select TEST_Name_var from tbl_Test as d where a.STCINWD_TEST_id=d.TEST_Id ) as AlertDtl_TestName_var
               from tbl_SteelChemical_Inward  as a,tbl_Client as b,tbl_Site as c
                where a.STCINWD_CL_Id=b.CL_Id
                and a.STCINWD_SITE_Id=c.SITE_Id
                and CONVERT (DATE, a.STCINWD_CollectionDate_dt) >=CONVERT (DATE,'" + fromDate + "') and CONVERT (DATE,a.STCINWD_CollectionDate_dt) <=CONVERT (DATE,'" + ToDate + "') " +
                @"and datediff(d,convert(date,a.STCINWD_CollectionDate_dt),convert(date,a.STCINWD_OutwardDate_dt))>((Select distinct(TEST_TimeFrame) from tbl_Test as d where Test_RecType_var='STC' and TEST_TimeFrame is NOT Null)+2)
				and datediff(d,convert(date,a.STCINWD_CollectionDate_dt),convert(date,GETDATE())) > ((Select distinct(TEST_TimeFrame) from tbl_Test as d where Test_RecType_var='STC' and TEST_TimeFrame is NOT Null)+2)

                Union

                 select  0 as AlertDtl_EnqNo_int,null as AlertDtl_EnqDate_date,a.TILEINWD_CollectionDate_dt as AlertDtl_CollctnEnqDate_date,'' as AlertDtl_BillNo_int,null as AlertDtl_BillDate_date,0 as AlertDtl_BillAmt_dec,
                '' as AlertDtl_ProNo_var,0 as AlertDtl_ProNo_int,null as AlertDtl_ProDate_date,TILEINWD_ReferenceNo_var as AlertDtl_RefNo_var,TILEINWD_RecordNo_int as AlertDtl_RecNo_int
                ,CL_Name_var as AlertDtl_CL_Name_var,SITE_Name_var as AlertDtl_SITE_Name_var,'TILE' as AlertDtl_RecType_var,TILEINWD_CL_Id  as AlertDtl_CLId_int,TILEINWD_SITE_Id  as AlertDtl_SiteId_int,28  as AlertDtl_Alert_Id,
                (select USER_NAME_var from tbl_User x where x.USER_Id IN (select ROUTE_ME_Id from tbl_Route where ROUTE_Id=c.SITE_Route_Id)) as AlertDtl_MEName_var			
                  ,null as AlertDtl_ProOrderDate_date,null as AlertDtl_BillModifiedDate_date,null as AlertDtl_InwdRecDate_date
                ,null as AlertDtl_AppDate_date,null as AlertDtl_EnteredDate_date,null as AlertDtl_CheckedDate_date,null as AlertDtl_OutwrdDate_date,
                (select TEST_Name_var from tbl_Test as d where a.TileINWD_TEST_id=d.TEST_Id ) as AlertDtl_TestName_var
                from tbl_Tile_Inward  as a,tbl_Client as b,tbl_Site as c
                where a.TileINWD_CL_Id=b.CL_Id
                and a.TileINWD_SITE_Id=c.SITE_Id
               and CONVERT (DATE, a.TILEINWD_CollectionDate_dt) >=CONVERT (DATE,'" + fromDate + "') and CONVERT (DATE,a.TILEINWD_CollectionDate_dt) <=CONVERT (DATE,'" + ToDate + "') " +
                @"and datediff(d,convert(date,a.TILEINWD_CollectionDate_dt),convert(date,a.TILEINWD_OutwardDate_dt))>((Select distinct(TEST_TimeFrame) from tbl_Test as d where Test_RecType_var='TILE' and TEST_TimeFrame is NOT Null)+2)
				and datediff(d,convert(date,a.TILEINWD_CollectionDate_dt),convert(date,GETDATE())) > ((Select distinct(TEST_TimeFrame) from tbl_Test as d where Test_RecType_var='TILE' and TEST_TimeFrame is NOT Null)+2)

                Union


                select  0 as AlertDtl_EnqNo_int,null as AlertDtl_EnqDate_date,a.WTINWD_CollectionDate_dt as AlertDtl_CollctnEnqDate_date,'' as AlertDtl_BillNo_int,null as AlertDtl_BillDate_date,0 as AlertDtl_BillAmt_dec,
                '' as AlertDtl_ProNo_var,0 as AlertDtl_ProNo_int,null as AlertDtl_ProDate_date,WTINWD_ReferenceNo_var as AlertDtl_RefNo_var,WTINWD_RecordNo_int as AlertDtl_RecNo_int
                ,CL_Name_var as AlertDtl_CL_Name_var,SITE_Name_var as AlertDtl_SITE_Name_var,'WT' as AlertDtl_RecType_var,WTINWD_CL_Id  as AlertDtl_CLId_int,WTINWD_SITE_Id  as AlertDtl_SiteId_int,28  as AlertDtl_Alert_Id,
                (select USER_NAME_var from tbl_User x where x.USER_Id IN (select ROUTE_ME_Id from tbl_Route where ROUTE_Id=c.SITE_Route_Id)) as AlertDtl_MEName_var			
                  ,null as AlertDtl_ProOrderDate_date,null as AlertDtl_BillModifiedDate_date,null as AlertDtl_InwdRecDate_date
                ,null as AlertDtl_AppDate_date,null as AlertDtl_EnteredDate_date,null as AlertDtl_CheckedDate_date,null as AlertDtl_OutwrdDate_date,
               (select TEST_Name_var from tbl_Test as d where e.WTTEST_TEST_id=d.TEST_Id ) as AlertDtl_TestName_var
               from tbl_Water_Inward  as a,tbl_Water_Test as e,tbl_Client as b,tbl_Site as c
                where a.WTINWD_CL_Id=b.CL_Id
                and a.WTINWD_SITE_Id=c.SITE_Id
                and a.WTINWD_ReferenceNo_var=e.WTTEST_ReferenceNo_var
                and CONVERT (DATE, a.WTINWD_CollectionDate_dt) >=CONVERT (DATE,'" + fromDate + "') and CONVERT (DATE,a.WTINWD_CollectionDate_dt) <=CONVERT (DATE,'" + ToDate + "') " +
                @"and datediff(d,convert(date,a.WTINWD_CollectionDate_dt),convert(date,a.WTINWD_OutwardDate_dt))>((Select distinct(TEST_TimeFrame) from tbl_Test as d where Test_RecType_var='WT' and TEST_TimeFrame is NOT Null)+2)
				and datediff(d,convert(date,a.WTINWD_CollectionDate_dt),convert(date,GETDATE())) > ((Select distinct(TEST_TimeFrame) from tbl_Test as d where Test_RecType_var='WT' and TEST_TimeFrame is NOT Null)+2)
                                ";
                    }
                    else
                    {
                        str = @"---Category wise test not completed in X+2 days
               select  0 as AlertDtl_EnqNo_int,null as AlertDtl_EnqDate_date,a.AACINWD_CollectionDate_dt as AlertDtl_CollctnEnqDate_date,'' as AlertDtl_BillNo_int,null as AlertDtl_BillDate_date,0 as AlertDtl_BillAmt_dec,
                '' as AlertDtl_ProNo_var,0 as AlertDtl_ProNo_int,null as AlertDtl_ProDate_date,AACINWD_ReferenceNo_var as AlertDtl_RefNo_var,AACINWD_RecordNo_int as AlertDtl_RecNo_int
                ,CL_Name_var as AlertDtl_CL_Name_var,SITE_Name_var as AlertDtl_SITE_Name_var,'AAC' as AlertDtl_RecType_var,AACINWD_CL_Id  as AlertDtl_CLId_int,AACINWD_SITE_Id  as AlertDtl_SiteId_int,28  as AlertDtl_Alert_Id,
                (select USER_NAME_var from tbl_User x where x.USER_Id IN (select ROUTE_ME_Id from tbl_Route where ROUTE_Id=c.SITE_Route_Id)) as AlertDtl_MEName_var			
                 ,null as AlertDtl_ProOrderDate_date,null as AlertDtl_BillModifiedDate_date,null as AlertDtl_InwdRecDate_date
                ,null as AlertDtl_AppDate_date,null as AlertDtl_EnteredDate_date,null as AlertDtl_CheckedDate_date,null as AlertDtl_OutwrdDate_date
                ,(select TEST_Name_var from tbl_Test as d where a.AACINWD_TEST_Id=d.TEST_Id ) as  AlertDtl_TestName_var
                from tbl_AAC_Inward  as a,tbl_Client as b,tbl_Site as c
                where a.AACINWD_CL_Id=b.CL_Id
                and a.AACINWD_SITE_Id=c.SITE_Id
                and a.AACINWD_OutwardDate_dt is null
               and CONVERT (DATE, a.AACINWD_CollectionDate_dt) >=CONVERT (DATE,'" + fromDate + "') and CONVERT (DATE,a.AACINWD_CollectionDate_dt) <=CONVERT (DATE,'" + ToDate + "') " +
                @"and datediff(d,convert(date,a.AACINWD_CollectionDate_dt),convert(date,GETDATE())) > ((select distinct(TEST_TimeFrame) from tbl_Test as d where Test_RecType_var='AAC' and TEST_TimeFrame is NOT Null)+2)

                Union

                 select  0 as AlertDtl_EnqNo_int,null as AlertDtl_EnqDate_date,a.AGGTINWD_CollectionDate_dt as AlertDtl_CollctnEnqDate_date,'' as AlertDtl_BillNo_int,null as AlertDtl_BillDate_date,0 as AlertDtl_BillAmt_dec,
                '' as AlertDtl_ProNo_var,0 as AlertDtl_ProNo_int,null as AlertDtl_ProDate_date,AGGTINWD_ReferenceNo_var as AlertDtl_RefNo_var,AGGTINWD_RecordNo_int as AlertDtl_RecNo_int,
                CL_Name_var as AlertDtl_CL_Name_var,SITE_Name_var as AlertDtl_SITE_Name_var,'AGGT' as AlertDtl_RecType_var,AGGTINWD_CL_Id  as AlertDtl_CLId_int,AGGTINWD_SITE_Id  as AlertDtl_SiteId_int,28  as AlertDtl_Alert_Id,
                (select USER_NAME_var from tbl_User x where x.USER_Id IN (select ROUTE_ME_Id from tbl_Route where ROUTE_Id=c.SITE_Route_Id)) as AlertDtl_MEName_var			
                  ,null as AlertDtl_ProOrderDate_date,null as AlertDtl_BillModifiedDate_date,null as AlertDtl_InwdRecDate_date
                ,null as AlertDtl_AppDate_date,null as AlertDtl_EnteredDate_date,null as AlertDtl_CheckedDate_date,null as AlertDtl_OutwrdDate_date,
                (select TEST_Name_var from tbl_Test as d where e.AGGTTEST_TEST_Id=d.TEST_Id ) as AlertDtl_TestName_var
               from tbl_Aggregate_Inward  as a,tbl_Aggregate_Test as e,tbl_Client as b,tbl_Site as c
                where a.AGGTINWD_CL_Id=b.CL_Id
                and a.AGGTINWD_SITE_Id=c.SITE_Id
                and a.AGGTINWD_ReferenceNo_var=e.AGGTTEST_ReferenceNo_var
                and a.AGGTINWD_OutwardDate_dt is null
                and CONVERT (DATE, a.AGGTINWD_CollectionDate_dt) >=CONVERT (DATE,'" + fromDate + "') and CONVERT (DATE,a.AGGTINWD_CollectionDate_dt) <=CONVERT (DATE,'" + ToDate + "') " +
                @"and datediff(d,convert(date,a.AGGTINWD_CollectionDate_dt),convert(date,GETDATE())) > ((select TEST_TimeFrame from tbl_Test as d where e.AGGTTEST_TEST_Id=d.TEST_Id and d.TEST_TimeFrame is not null)+2)

                Union

               select  0 as AlertDtl_EnqNo_int,null as AlertDtl_EnqDate_date,a.GTINW_CollectionDate_dt as AlertDtl_CollctnEnqDate_date,'' as AlertDtl_BillNo_int,null as AlertDtl_BillDate_date,0 as AlertDtl_BillAmt_dec,
                '' as AlertDtl_ProNo_var,0 as AlertDtl_ProNo_int,null as AlertDtl_ProDate_date,GTINW_RefNo_var as AlertDtl_RefNo_var,GTINW_RecordNo_int as AlertDtl_RecNo_int,
                CL_Name_var as AlertDtl_CL_Name_var,SITE_Name_var as AlertDtl_SITE_Name_var,'GT' as AlertDtl_RecType_var,GTINW_CL_Id  as AlertDtl_CLId_int,GTINW_SITE_Id  as AlertDtl_SiteId_int,28  as AlertDtl_Alert_Id,
                (select USER_NAME_var from tbl_User x where x.USER_Id IN (select ROUTE_ME_Id from tbl_Route where ROUTE_Id=c.SITE_Route_Id)) as AlertDtl_MEName_var			
                  ,null as AlertDtl_ProOrderDate_date,null as AlertDtl_BillModifiedDate_date,null as AlertDtl_InwdRecDate_date
                ,null as AlertDtl_AppDate_date,null as AlertDtl_EnteredDate_date,null as AlertDtl_CheckedDate_date,null as AlertDtl_OutwrdDate_date
                ,(select TEST_Name_var from tbl_Test as d where e.GTTEST_Srno_tint=d.TEST_Id ) as AlertDtl_TestName_var
               from tbl_GT_Inward  as a,tbl_GT_Test as e,tbl_Client as b,tbl_Site as c
                where a.GTINW_CL_Id=b.CL_Id
                and a.GTINW_SITE_Id=c.SITE_Id
                and a.GTINW_RefNo_var=e.GTTEST_RefNo_var
                and a.GTINW_OutwardDate_dt is null
                and CONVERT (DATE, a.GTINW_CollectionDate_dt) >=CONVERT (DATE,'" + fromDate + "') and CONVERT (DATE,a.GTINW_CollectionDate_dt) <=CONVERT (DATE,'" + ToDate + "') " +
                @"and datediff(d,convert(date,a.GTINW_CollectionDate_dt),convert(date,GETDATE())) > ((Select distinct(TEST_TimeFrame) from tbl_Test as d where Test_RecType_var='GT' and TEST_TimeFrame is NOT Null)+2)

                Union

                 select  0 as AlertDtl_EnqNo_int,null as AlertDtl_EnqDate_date,a.CCHINWD_CollectionDate_dt as AlertDtl_CollctnEnqDate_date,'' as AlertDtl_BillNo_int,null as AlertDtl_BillDate_date,0 as AlertDtl_BillAmt_dec,
                '' as AlertDtl_ProNo_var,0 as AlertDtl_ProNo_int,null as AlertDtl_ProDate_date,CCHINWD_ReferenceNo_var as AlertDtl_RefNo_var,CCHINWD_RecordNo_int as AlertDtl_RecNo_int,
                CL_Name_var as AlertDtl_CL_Name_var,SITE_Name_var as AlertDtl_SITE_Name_var,'CCH' as AlertDtl_RecType_var,CCHINWD_CL_Id  as AlertDtl_CLId_int,CCHINWD_SITE_Id  as AlertDtl_SiteId_int,28  as AlertDtl_Alert_Id,
                (select USER_NAME_var from tbl_User x where x.USER_Id IN (select ROUTE_ME_Id from tbl_Route where ROUTE_Id=c.SITE_Route_Id)) as AlertDtl_MEName_var			
                  ,null as AlertDtl_ProOrderDate_date,null as AlertDtl_BillModifiedDate_date,null as AlertDtl_InwdRecDate_date
                ,null as AlertDtl_AppDate_date,null as AlertDtl_EnteredDate_date,null as AlertDtl_CheckedDate_date,null as AlertDtl_OutwrdDate_date,
                (select TEST_Name_var from tbl_Test as d where e.CCHTEST_TEST_Id=d.TEST_Id ) as AlertDtl_TestName_var
               from tbl_CementChemical_Inward  as a,tbl_CementChemical_Test as e,tbl_Client as b,tbl_Site as c 
                where a.CCHINWD_CL_Id=b.CL_Id
                and a.CCHINWD_SITE_Id=c.SITE_Id
                and a.CCHINWD_ReferenceNo_var=e.CCHTEST_ReferenceNo_var
                and a.CCHINWD_OutwardDate_dt is null
                and CONVERT (DATE, a.CCHINWD_CollectionDate_dt) >=CONVERT (DATE,'" + fromDate + "') and CONVERT (DATE,a.CCHINWD_CollectionDate_dt) <=CONVERT (DATE,'" + ToDate + "') " +
                @"and datediff(d,convert(date,a.CCHINWD_CollectionDate_dt),convert(date,GETDATE())) > ((Select distinct(TEST_TimeFrame) from tbl_Test as d where Test_RecType_var='CCH' and TEST_TimeFrame is NOT Null)+2)

                Union

                select  0 as AlertDtl_EnqNo_int,null as AlertDtl_EnqDate_date,a.BTINWD_CollectionDate_dt as AlertDtl_CollctnEnqDate_date,'' as AlertDtl_BillNo_int,null as AlertDtl_BillDate_date,0 as AlertDtl_BillAmt_dec,
                '' as AlertDtl_ProNo_var,0 as AlertDtl_ProNo_int,null as AlertDtl_ProDate_date,BTINWD_ReferenceNo_var as AlertDtl_RefNo_var,BTINWD_RecordNo_int as AlertDtl_RecNo_int,
                CL_Name_var as AlertDtl_CL_Name_var,SITE_Name_var as AlertDtl_SITE_Name_var,'BT-' as AlertDtl_RecType_var,BTINWD_CL_Id  as AlertDtl_CLId_int,BTINWD_SITE_Id  as AlertDtl_SiteId_int,28  as AlertDtl_Alert_Id,
                (select USER_NAME_var from tbl_User x where x.USER_Id IN (select ROUTE_ME_Id from tbl_Route where ROUTE_Id=c.SITE_Route_Id)) as AlertDtl_MEName_var			
                  ,null as AlertDtl_ProOrderDate_date,null as AlertDtl_BillModifiedDate_date,null as AlertDtl_InwdRecDate_date
                ,null as AlertDtl_AppDate_date,null as AlertDtl_EnteredDate_date,null as AlertDtl_CheckedDate_date,null as AlertDtl_OutwrdDate_date,
               (select TEST_Name_var from tbl_Test as d where e.BTTEST_TEST_Id=d.TEST_Id ) as AlertDtl_TestName_var
                from tbl_Brick_Inward  as a,tbl_Brick_Test as e,tbl_Client as b,tbl_Site as c 
                where a.BTINWD_CL_Id=b.CL_Id
                and a.BTINWD_SITE_Id=c.SITE_Id
                and a.BTINWD_ReferenceNo_var=e.BTTEST_ReferenceNo_var
                and a.BTINWD_OutwardDate_dt is null
                and CONVERT (DATE, a.BTINWD_CollectionDate_dt) >=CONVERT (DATE,'" + fromDate + "') and CONVERT (DATE,a.BTINWD_CollectionDate_dt) <=CONVERT (DATE,'" + ToDate + "') " +
                @"and datediff(d,convert(date,a.BTINWD_CollectionDate_dt),convert(date,GETDATE())) > ((select TEST_TimeFrame from tbl_Test as d where e.BTTEST_TEST_Id=d.TEST_Id and d.TEST_TimeFrame is not null)+2)

                Union

                select  0 as AlertDtl_EnqNo_int,null as AlertDtl_EnqDate_date,a.CEMTINWD_CollectionDate_dt as AlertDtl_CollctnEnqDate_date,'' as AlertDtl_BillNo_int,null as AlertDtl_BillDate_date,0 as AlertDtl_BillAmt_dec,
                '' as AlertDtl_ProNo_var,0 as AlertDtl_ProNo_int,null as AlertDtl_ProDate_date,CEMTINWD_ReferenceNo_var as AlertDtl_RefNo_var,CEMTINWD_RecordNo_int as AlertDtl_RecNo_int,CL_Name_var as AlertDtl_CL_Name_var,SITE_Name_var as AlertDtl_SITE_Name_var,
                'CEMT' as AlertDtl_RecType_var,CEMTINWD_CL_Id  as AlertDtl_CLId_int,CEMTINWD_SITE_Id  as AlertDtl_SiteId_int,28  as AlertDtl_Alert_Id,
                (select USER_NAME_var from tbl_User x where x.USER_Id IN (select ROUTE_ME_Id from tbl_Route where ROUTE_Id=c.SITE_Route_Id)) as AlertDtl_MEName_var			
                  ,null as AlertDtl_ProOrderDate_date,null as AlertDtl_BillModifiedDate_date,null as AlertDtl_InwdRecDate_date
                ,null as AlertDtl_AppDate_date,null as AlertDtl_EnteredDate_date,null as AlertDtl_CheckedDate_date,null as AlertDtl_OutwrdDate_date,
               (select TEST_Name_var from tbl_Test as d where e.CEMTTEST_TEST_Id=d.TEST_Id ) as AlertDtl_TestName_var
                from tbl_Cement_Inward  as a,tbl_Cement_Test as e,tbl_Client as b,tbl_Site as c 
                where a.CEMTINWD_CL_Id=b.CL_Id
                and a.CEMTINWD_SITE_Id=c.SITE_Id
                and a.CEMTINWD_ReferenceNo_var=e.CEMTTEST_ReferenceNo_var
                and a.CEMTINWD_OutwardDate_dt is null
                 and CONVERT (DATE, a.CEMTINWD_CollectionDate_dt) >=CONVERT (DATE,'" + fromDate + "') and CONVERT (DATE,a.CEMTINWD_CollectionDate_dt) <=CONVERT (DATE,'" + ToDate + "') " +
                @"and datediff(d,convert(date,a.CEMTINWD_CollectionDate_dt),convert(date,GETDATE())) > ((select TEST_TimeFrame from tbl_Test as d where e.CEMTTEST_TEST_Id=d.TEST_Id and d.TEST_TimeFrame is not null)+2)

                Union

                select  0 as AlertDtl_EnqNo_int,null as AlertDtl_EnqDate_date,a.CRINWD_CollectionDate_dt as AlertDtl_CollctnEnqDate_date,'' as AlertDtl_BillNo_int,null as AlertDtl_BillDate_date,0 as AlertDtl_BillAmt_dec,
                '' as AlertDtl_ProNo_var,0 as AlertDtl_ProNo_int,null as AlertDtl_ProDate_date,CRINWD_ReferenceNo_var as AlertDtl_RefNo_var,CRINWD_RecordNo_int as AlertDtl_RecNo_int,
                CL_Name_var as AlertDtl_CL_Name_var,SITE_Name_var as AlertDtl_SITE_Name_var,'CR' as AlertDtl_RecType_var,CRINWD_CL_Id  as AlertDtl_CLId_int,CRINWD_SITE_Id  as AlertDtl_SiteId_int,28  as AlertDtl_Alert_Id,
                (select USER_NAME_var from tbl_User x where x.USER_Id IN (select ROUTE_ME_Id from tbl_Route where ROUTE_Id=c.SITE_Route_Id)) as AlertDtl_MEName_var			
                  ,null as AlertDtl_ProOrderDate_date,null as AlertDtl_BillModifiedDate_date,null as AlertDtl_InwdRecDate_date
                ,null as AlertDtl_AppDate_date,null as AlertDtl_EnteredDate_date,null as AlertDtl_CheckedDate_date,null as AlertDtl_OutwrdDate_date,
                (select TEST_Name_var from tbl_Test as d where a.CRINWD_TestId_int=d.TEST_Id ) as AlertDtl_TestName_var
                from tbl_Core_Inward  as a,tbl_Client as b,tbl_Site as c 
                where a.CRINWD_CL_Id=b.CL_Id
                and a.CRINWD_SITE_Id=c.SITE_Id
                and a.CRINWD_OutwardDate_dt is null
                and CONVERT (DATE, a.CRINWD_CollectionDate_dt) >=CONVERT (DATE,'" + fromDate + "') and CONVERT (DATE,a.CRINWD_CollectionDate_dt) <=CONVERT (DATE,'" + ToDate + "') " +
                @"and datediff(d,convert(date,a.CRINWD_CollectionDate_dt),convert(date,GETDATE())) > ((Select distinct(TEST_TimeFrame) from tbl_Test as d where Test_RecType_var='CR' and TEST_TimeFrame is NOT Null)+2)

                Union

                select  0 as AlertDtl_EnqNo_int,null as AlertDtl_EnqDate_date,a.CoreCutINWD_CollectionDate_dt as AlertDtl_CollctnEnqDate_date,'' as AlertDtl_BillNo_int,null as AlertDtl_BillDate_date,0 as AlertDtl_BillAmt_dec,
                '' as AlertDtl_ProNo_var,0 as AlertDtl_ProNo_int,null as AlertDtl_ProDate_date,CORECUTINWD_ReferenceNo_var as AlertDtl_RefNo_var,CORECUTINWD_RecordNo_int as AlertDtl_RecNo_int,
                CL_Name_var as AlertDtl_CL_Name_var,SITE_Name_var as AlertDtl_SITE_Name_var,'CORECUT' as AlertDtl_RecType_var,CORECUTINWD_CL_Id  as AlertDtl_CLId_int,CORECUTINWD_SITE_Id  as AlertDtl_SiteId_int,28  as AlertDtl_Alert_Id,
                (select USER_NAME_var from tbl_User x where x.USER_Id IN (select ROUTE_ME_Id from tbl_Route where ROUTE_Id=c.SITE_Route_Id)) as AlertDtl_MEName_var			
                  ,null as AlertDtl_ProOrderDate_date,null as AlertDtl_BillModifiedDate_date,null as AlertDtl_InwdRecDate_date
                ,null as AlertDtl_AppDate_date,null as AlertDtl_EnteredDate_date,null as AlertDtl_CheckedDate_date,null as AlertDtl_OutwrdDate_date,
                 (select TEST_Name_var from tbl_Test as d where a.CoreCutINWD_TEST_Id=d.TEST_Id ) as AlertDtl_TestName_var
                from tbl_CoreCutting_Inward  as a,tbl_Client as b,tbl_Site as c 
                where a.CoreCutINWD_CL_Id=b.CL_Id
                and a.CoreCutINWD_SITE_Id=c.SITE_Id
                and a.CoreCutINWD_OutwardDate_dt is null
                 and CONVERT (DATE, a.CoreCutINWD_CollectionDate_dt) >=CONVERT (DATE,'" + fromDate + "') and CONVERT (DATE,a.CoreCutINWD_CollectionDate_dt) <=CONVERT (DATE,'" + ToDate + "') " +
                @"and datediff(d,convert(date,a.CoreCutINWD_CollectionDate_dt),convert(date,GETDATE())) > ((Select distinct(TEST_TimeFrame) from tbl_Test as d where Test_RecType_var='CORECUT' and TEST_TimeFrame is NOT Null)+2)

                Union

                select  0 as AlertDtl_EnqNo_int,null as AlertDtl_EnqDate_date,a.CTINWD_CollectionDate_dt as AlertDtl_CollctnEnqDate_date,'' as AlertDtl_BillNo_int,null as AlertDtl_BillDate_date,0 as AlertDtl_BillAmt_dec,
                '' as AlertDtl_ProNo_var,0 as AlertDtl_ProNo_int,null as AlertDtl_ProDate_date,CTINWD_ReferenceNo_var as AlertDtl_RefNo_var,CTINWD_RecordNo_int as AlertDtl_RecNo_int,CL_Name_var as AlertDtl_CL_Name_var,SITE_Name_var as AlertDtl_SITE_Name_var,
                'CT' as AlertDtl_RecType_var,CTINWD_CL_Id  as AlertDtl_CLId_int,CTINWD_SITE_Id  as AlertDtl_SiteId_int,28  as AlertDtl_Alert_Id,
                (select USER_NAME_var from tbl_User x where x.USER_Id IN (select ROUTE_ME_Id from tbl_Route where ROUTE_Id=c.SITE_Route_Id)) as AlertDtl_MEName_var			
                  ,null as AlertDtl_ProOrderDate_date,null as AlertDtl_BillModifiedDate_date,null as AlertDtl_InwdRecDate_date
                ,null as AlertDtl_AppDate_date,null as AlertDtl_EnteredDate_date,null as AlertDtl_CheckedDate_date,null as AlertDtl_OutwrdDate_date,
                  (select TEST_Name_var from tbl_Test as d where a.CTINWD_TEST_Id=d.TEST_Id ) as AlertDtl_TestName_var
               from tbl_Cube_Inward  as a,tbl_Client as b,tbl_Site as c 
                where a.CTINWD_CL_Id=b.CL_Id
                and a.CTINWD_SITE_Id=c.SITE_Id
                and a.CTINWD_OutwardDate_dt is null
                and CONVERT (DATE, a.CTINWD_CollectionDate_dt) >=CONVERT (DATE,'" + fromDate + "') and CONVERT (DATE,a.CTINWD_CollectionDate_dt) <=CONVERT (DATE,'" + ToDate + "') " +
                @"and datediff(d,convert(date,a.CTINWD_CollectionDate_dt),convert(date,GETDATE())) > ((Select distinct(TEST_TimeFrame) from tbl_Test as d where Test_RecType_var='CT' and TEST_TimeFrame is NOT Null)+2)

                Union

                select  0 as AlertDtl_EnqNo_int,null as AlertDtl_EnqDate_date,a.FlyAshINWD_CollectionDate_dt as AlertDtl_CollctnEnqDate_date,'' as AlertDtl_BillNo_int,null as AlertDtl_BillDate_date,0 as AlertDtl_BillAmt_dec,
                '' as AlertDtl_ProNo_var,0 as AlertDtl_ProNo_int,null as AlertDtl_ProDate_date,FlyAshINWD_ReferenceNo_var as AlertDtl_RefNo_var,FlyAshINWD_RecordNo_int as AlertDtl_RecNo_int,
                CL_Name_var as AlertDtl_CL_Name_var,SITE_Name_var as AlertDtl_SITE_Name_var,'FLYASH' as AlertDtl_RecType_var,FlyAshINWD_CL_Id  as AlertDtl_CLId_int,FlyAshINWD_SITE_Id  as AlertDtl_SiteId_int,28  as AlertDtl_Alert_Id,
                (select USER_NAME_var from tbl_User x where x.USER_Id IN (select ROUTE_ME_Id from tbl_Route where ROUTE_Id=c.SITE_Route_Id)) as AlertDtl_MEName_var			
                  ,null as AlertDtl_ProOrderDate_date,null as AlertDtl_BillModifiedDate_date,null as AlertDtl_InwdRecDate_date
                ,null as AlertDtl_AppDate_date,null as AlertDtl_EnteredDate_date,null as AlertDtl_CheckedDate_date,null as AlertDtl_OutwrdDate_date,
               (select TEST_Name_var from tbl_Test as d where e.FlyAshTEST_TEST_Id=d.TEST_Id ) as AlertDtl_TestName_var
                from tbl_FlyAsh_Inward  as a,tbl_FlyAsh_Test as e,tbl_Client as b,tbl_Site as c 
                where a.FlyAshINWD_CL_Id=b.CL_Id
                and a.FlyAshINWD_SITE_Id=c.SITE_Id
                and a.FlyAshINWD_ReferenceNo_var=e.FLYASHTEST_ReferenceNo_var
                and a.FlyAshINWD_OutwardDate_dt is null
                and CONVERT (DATE, a.FlyAshINWD_CollectionDate_dt) >=CONVERT (DATE,'" + fromDate + "') and CONVERT (DATE,a.FlyAshINWD_CollectionDate_dt) <=CONVERT (DATE,'" + ToDate + "') " +
                @"and datediff(d,convert(date,a.FlyAshINWD_CollectionDate_dt),convert(date,GETDATE())) > ((select TEST_TimeFrame from tbl_Test as d where E.FLYASHTEST_TEST_Id=d.TEST_Id and d.TEST_TimeFrame is not null)+2)

                Union

                 select  0 as AlertDtl_EnqNo_int,null as AlertDtl_EnqDate_date,a.MFINWD_CollectionDate_dt as AlertDtl_CollctnEnqDate_date,'' as AlertDtl_BillNo_int,null as AlertDtl_BillDate_date,0 as AlertDtl_BillAmt_dec,
                '' as AlertDtl_ProNo_var,0 as AlertDtl_ProNo_int,null as AlertDtl_ProDate_date,MFINWD_ReferenceNo_var as AlertDtl_RefNo_var,MFINWD_RecordNo_int as AlertDtl_RecNo_int,
                CL_Name_var as AlertDtl_CL_Name_var,SITE_Name_var as AlertDtl_SITE_Name_var,'MF' as AlertDtl_RecType_var,MFINWD_CL_Id  as AlertDtl_CLId_int,MFINWD_SITE_Id  as AlertDtl_SiteId_int,28  as AlertDtl_Alert_Id,
                 (select USER_NAME_var from tbl_User x where x.USER_Id IN (select ROUTE_ME_Id from tbl_Route where ROUTE_Id=c.SITE_Route_Id)) as AlertDtl_MEName_var			
                  ,null as AlertDtl_ProOrderDate_date,null as AlertDtl_BillModifiedDate_date,null as AlertDtl_InwdRecDate_date
                ,null as AlertDtl_AppDate_date,null as AlertDtl_EnteredDate_date,null as AlertDtl_CheckedDate_date,null as AlertDtl_OutwrdDate_date,
                (select TEST_Name_var from tbl_Test as d where a.MFINWD_TestId_int=d.TEST_Id ) as AlertDtl_TestName_var
             from tbl_MixDesign_Inward  as a,tbl_Client as b,tbl_Site as c 
                where a.MFINWD_CL_Id=b.CL_Id
                and a.MFINWD_SITE_Id=c.SITE_Id
                and a.MFINWD_OutwardDate_dt is null
                and CONVERT (DATE, a.MFINWD_CollectionDate_dt) >=CONVERT (DATE,'" + fromDate + "') and CONVERT (DATE,a.MFINWD_CollectionDate_dt) <=CONVERT (DATE,'" + ToDate + "') " +
                @"and datediff(d,convert(date,a.MFINWD_CollectionDate_dt),convert(date,GETDATE())) > ((select TEST_TimeFrame from tbl_Test as d where a.MFINWD_TestId_int=d.TEST_Id and d.TEST_TimeFrame is not null)+2)

                Union

                select  0 as AlertDtl_EnqNo_int,null as AlertDtl_EnqDate_date,a.NDTINWD_CollectionDate_dt as AlertDtl_CollctnEnqDate_date,'' as AlertDtl_BillNo_int,null as AlertDtl_BillDate_date,0 as AlertDtl_BillAmt_dec,
                '' as AlertDtl_ProNo_var,0 as AlertDtl_ProNo_int,null as AlertDtl_ProDate_date,NDTINWD_ReferenceNo_var as AlertDtl_RefNo_var,NDTINWD_RecordNo_int as AlertDtl_RecNo_int,
                CL_Name_var as AlertDtl_CL_Name_var,SITE_Name_var as AlertDtl_SITE_Name_var,'NDT' as AlertDtl_RecType_var,NDTINWD_CL_Id  as AlertDtl_CLId_int,NDTINWD_SITE_Id  as AlertDtl_SiteId_int,28  as AlertDtl_Alert_Id,
               (select USER_NAME_var from tbl_User x where x.USER_Id IN (select ROUTE_ME_Id from tbl_Route where ROUTE_Id=c.SITE_Route_Id)) as AlertDtl_MEName_var			
                  ,null as AlertDtl_ProOrderDate_date,null as AlertDtl_BillModifiedDate_date,null as AlertDtl_InwdRecDate_date
                ,null as AlertDtl_AppDate_date,null as AlertDtl_EnteredDate_date,null as AlertDtl_CheckedDate_date,null as AlertDtl_OutwrdDate_date,
                     (select TEST_Name_var from tbl_Test as d where e.NDTTEST_TEST_Id=d.TEST_Id ) as AlertDtl_TestName_var
				from tbl_NDT_Inward  as a,tbl_NDT_Test as e,tbl_Client as b,tbl_Site as c 
                where a.NDTINWD_CL_Id=b.CL_Id
                and a.NDTINWD_SITE_Id=c.SITE_Id
                and a.NDTINWD_ReferenceNo_var=e.NDTTEST_RefNo_var
                and a.NDTINWD_OutwardDate_dt is null
                and CONVERT (DATE, a.NDTINWD_CollectionDate_dt) >=CONVERT (DATE,'" + fromDate + "') and CONVERT (DATE,a.NDTINWD_CollectionDate_dt) <=CONVERT (DATE,'" + ToDate + "') " +
                @"and datediff(d,convert(date,a.NDTINWD_CollectionDate_dt),convert(date,GETDATE())) > ((Select distinct(TEST_TimeFrame) from tbl_Test as d where Test_RecType_var='NDT' and TEST_TimeFrame is NOT Null)+2)

                Union

               select  0 as AlertDtl_EnqNo_int,null as AlertDtl_EnqDate_date,a.OTINWD_CollectionDate_dt as AlertDtl_CollctnEnqDate_date,'' as AlertDtl_BillNo_int,null as AlertDtl_BillDate_date,0 as AlertDtl_BillAmt_dec,
                '' as AlertDtl_ProNo_var,0 as AlertDtl_ProNo_int,null as AlertDtl_ProDate_date,OTINWD_ReferenceNo_var as AlertDtl_RefNo_var,OTINWD_RecordNo_int as AlertDtl_RecNo_int,
                CL_Name_var as AlertDtl_CL_Name_var,SITE_Name_var as AlertDtl_SITE_Name_var,'OT' as AlertDtl_RecType_var,OTINWD_CL_Id  as AlertDtl_CLId_int,OTINWD_SITE_Id  as AlertDtl_SiteId_int,28  as AlertDtl_Alert_Id,
                (select USER_NAME_var from tbl_User x where x.USER_Id IN (select ROUTE_ME_Id from tbl_Route where ROUTE_Id=c.SITE_Route_Id)) as AlertDtl_MEName_var			
                  ,null as AlertDtl_ProOrderDate_date,null as AlertDtl_BillModifiedDate_date,null as AlertDtl_InwdRecDate_date
                ,null as AlertDtl_AppDate_date,null as AlertDtl_EnteredDate_date,null as AlertDtl_CheckedDate_date,null as AlertDtl_OutwrdDate_date,
               (select TEST_Name_var from tbl_Test as d where e.OTTEST_TEST_Id=d.TEST_Id ) as AlertDtl_TestName_var
                from tbl_Other_Inward  as a,tbl_Other_Test as e,tbl_Client as b,tbl_Site as c 
                where a.OTINWD_CL_Id=b.CL_Id
                and a.OTINWD_SITE_Id=c.SITE_Id
                and a.OTINWD_ReferenceNo_var=e.OTTEST_ReferenceNo_var
                and a.OTINWD_OutwardDate_dt is null
                and CONVERT (DATE, a.OTINWD_CollectionDate_dt) >=CONVERT (DATE,'" + fromDate + "') and CONVERT (DATE,a.OTINWD_CollectionDate_dt) <=CONVERT (DATE,'" + ToDate + "') " +
                @"and datediff(d,convert(date,a.OTINWD_CollectionDate_dt),convert(date,GETDATE())) > ((Select distinct(TEST_TimeFrame) from tbl_Test as d where Test_RecType_var='OT' and TEST_TimeFrame is NOT Null)+2)

                Union

               select  0 as AlertDtl_EnqNo_int,null as AlertDtl_EnqDate_date,a.PTINWD_CollectionDate_dt as AlertDtl_CollctnEnqDate_date,'' as AlertDtl_BillNo_int,null as AlertDtl_BillDate_date,0 as AlertDtl_BillAmt_dec,
                '' as AlertDtl_ProNo_var,0 as AlertDtl_ProNo_int,null as AlertDtl_ProDate_date,PTINWD_ReferenceNo_var as AlertDtl_RefNo_var,PTINWD_RecordNo_int as AlertDtl_RecNo_int
                ,CL_Name_var as AlertDtl_CL_Name_var,SITE_Name_var as AlertDtl_SITE_Name_var,'PT' as AlertDtl_RecType_var,PTINWD_CL_Id  as AlertDtl_CLId_int,PTINWD_SITE_Id  as AlertDtl_SiteId_int,28  as AlertDtl_Alert_Id,
                (select USER_NAME_var from tbl_User x where x.USER_Id IN (select ROUTE_ME_Id from tbl_Route where ROUTE_Id=c.SITE_Route_Id)) as AlertDtl_MEName_var			
                  ,null as AlertDtl_ProOrderDate_date,null as AlertDtl_BillModifiedDate_date,null as AlertDtl_InwdRecDate_date
                ,null as AlertDtl_AppDate_date,null as AlertDtl_EnteredDate_date,null as AlertDtl_CheckedDate_date,null as AlertDtl_OutwrdDate_date,
                 (select TEST_Name_var from tbl_Test as d where a.PTINWD_TEST_Id=d.TEST_Id ) as AlertDtl_TestName_var 
               from tbl_Pavement_Inward  as a,tbl_Client as b,tbl_Site as c
                where a.PTINWD_CL_Id=b.CL_Id
                and a.PTINWD_SITE_Id=c.SITE_Id
                and a.PTINWD_OutwardDate_dt is null
                and CONVERT (DATE, a.PTINWD_CollectionDate_dt) >=CONVERT (DATE,'" + fromDate + "') and CONVERT (DATE,a.PTINWD_CollectionDate_dt) <=CONVERT (DATE,'" + ToDate + "') " +
                @"and datediff(d,convert(date,a.PTINWD_CollectionDate_dt),convert(date,GETDATE())) > ((select TEST_TimeFrame from tbl_Test as d where a.PTINWD_TEST_Id=d.TEST_Id and d.TEST_TimeFrame is not null)+2)

                Union

                 select  0 as AlertDtl_EnqNo_int,null as AlertDtl_EnqDate_date,a.PILEINWD_CollectionDate_dt as AlertDtl_CollctnEnqDate_date,'' as AlertDtl_BillNo_int,null as AlertDtl_BillDate_date,0 as AlertDtl_BillAmt_dec,
                '' as AlertDtl_ProNo_var,0 as AlertDtl_ProNo_int,null as AlertDtl_ProDate_date,PILEINWD_ReferenceNo_var as AlertDtl_RefNo_var,PILEINWD_RecordNo_int as AlertDtl_RecNo_int,
                CL_Name_var as AlertDtl_CL_Name_var,SITE_Name_var as AlertDtl_SITE_Name_var,'PILE' as AlertDtl_RecType_var,PILEINWD_CL_Id  as AlertDtl_CLId_int,PILEINWD_SITE_Id  as AlertDtl_SiteId_int,28  as AlertDtl_Alert_Id,
                (select USER_NAME_var from tbl_User x where x.USER_Id IN (select ROUTE_ME_Id from tbl_Route where ROUTE_Id=c.SITE_Route_Id)) as AlertDtl_MEName_var			
                  ,null as AlertDtl_ProOrderDate_date,null as AlertDtl_BillModifiedDate_date,null as AlertDtl_InwdRecDate_date
                ,null as AlertDtl_AppDate_date,null as AlertDtl_EnteredDate_date,null as AlertDtl_CheckedDate_date,null as AlertDtl_OutwrdDate_date,
                 (select TEST_Name_var from tbl_Test as d where a.PILEINWD_TEST_Id=d.TEST_Id ) as AlertDtl_TestName_var 
               from tbl_Pile_Inward  as a,tbl_Client as b,tbl_Site as c
                where a.PILEINWD_CL_Id=b.CL_Id
                and a.PILEINWD_SITE_Id=c.SITE_Id
                and a.PILEINWD_OutwardDate_dt is null
                and CONVERT (DATE, a.PILEINWD_CollectionDate_dt) >=CONVERT (DATE,'" + fromDate + "') and CONVERT (DATE,a.PILEINWD_CollectionDate_dt) <=CONVERT (DATE,'" + ToDate + "') " +
                @"and datediff(d,convert(date,a.PILEINWD_CollectionDate_dt),convert(date,GETDATE())) > ((Select distinct(TEST_TimeFrame) from tbl_Test as d where Test_RecType_var='PILE' and TEST_TimeFrame is NOT Null)+2)

                Union

                select  0 as AlertDtl_EnqNo_int,null as AlertDtl_EnqDate_date,a.SOINWD_CollectionDate_dt as AlertDtl_CollctnEnqDate_date,'' as AlertDtl_BillNo_int,null as AlertDtl_BillDate_date,0 as AlertDtl_BillAmt_dec,
                '' as AlertDtl_ProNo_var,0 as AlertDtl_ProNo_int,null as AlertDtl_ProDate_date,SOINWD_ReferenceNo_var as AlertDtl_RefNo_var,SOINWD_RecordNo_int as AlertDtl_RecNo_int,
                CL_Name_var as AlertDtl_CL_Name_var,SITE_Name_var as AlertDtl_SITE_Name_var,'SO' as AlertDtl_RecType_var,SOINWD_CL_Id  as AlertDtl_CLId_int,SOINWD_SITE_Id  as AlertDtl_SiteId_int,28  as AlertDtl_Alert_Id,
                (select USER_NAME_var from tbl_User x where x.USER_Id IN (select ROUTE_ME_Id from tbl_Route where ROUTE_Id=c.SITE_Route_Id)) as AlertDtl_MEName_var			
                  ,null as AlertDtl_ProOrderDate_date,null as AlertDtl_BillModifiedDate_date,null as AlertDtl_InwdRecDate_date
                ,null as AlertDtl_AppDate_date,null as AlertDtl_EnteredDate_date,null as AlertDtl_CheckedDate_date,null as AlertDtl_OutwrdDate_date,
                (select TEST_Name_var from tbl_Test as d where e.SOTEST_TEST_int=d.TEST_Id ) as AlertDtl_TestName_var
               from tbl_Soil_Inward  as a,tbl_Soil_Test as e,tbl_Client as b,tbl_Site as c
                where a.SOINWD_CL_Id=b.CL_Id
                and a.SOINWD_SITE_Id=c.SITE_Id
                and a.SOINWD_ReferenceNo_var=e.SOTEST_ReferenceNo_var
                and a.SOINWD_OutwardDate_dt is null
               and CONVERT (DATE, a.SOINWD_CollectionDate_dt) >=CONVERT (DATE,'" + fromDate + "') and CONVERT (DATE,a.SOINWD_CollectionDate_dt) <=CONVERT (DATE,'" + ToDate + "') " +
                @"and datediff(d,convert(date,a.SOINWD_CollectionDate_dt),convert(date,GETDATE())) > ((select TEST_TimeFrame from tbl_Test as d where E.SOTEST_TEST_int=d.TEST_Id and d.TEST_TimeFrame is not null)+2)

                Union

               select  0 as AlertDtl_EnqNo_int,null as AlertDtl_EnqDate_date,a.SolidINWD_CollectionDate_dt as AlertDtl_CollctnEnqDate_date,'' as AlertDtl_BillNo_int,null as AlertDtl_BillDate_date,0 as AlertDtl_BillAmt_dec,
                '' as AlertDtl_ProNo_var,0 as AlertDtl_ProNo_int,null as AlertDtl_ProDate_date,SolidINWD_ReferenceNo_var as AlertDtl_RefNo_var,SolidINWD_RecordNo_int as AlertDtl_RecNo_int,
                CL_Name_var as AlertDtl_CL_Name_var,SITE_Name_var as AlertDtl_SITE_Name_var,'SOLID' as AlertDtl_RecType_var,SolidINWD_CL_Id  as AlertDtl_CLId_int,SolidINWD_SITE_Id  as AlertDtl_SiteId_int,28  as AlertDtl_Alert_Id,
                (select USER_NAME_var from tbl_User x where x.USER_Id IN (select ROUTE_ME_Id from tbl_Route where ROUTE_Id=c.SITE_Route_Id)) as AlertDtl_MEName_var			
                  ,null as AlertDtl_ProOrderDate_date,null as AlertDtl_BillModifiedDate_date,null as AlertDtl_InwdRecDate_date
                ,null as AlertDtl_AppDate_date,null as AlertDtl_EnteredDate_date,null as AlertDtl_CheckedDate_date,null as AlertDtl_OutwrdDate_date,
                (select TEST_Name_var from tbl_Test as d where a.SolidINWD_TEST_Id=d.TEST_Id ) as AlertDtl_TestName_var
                from tbl_Solid_Inward  as a,tbl_Client as b,tbl_Site as c
                where a.SolidINWD_CL_Id=b.CL_Id
                and a.SolidINWD_SITE_Id=c.SITE_Id
                and a.SolidINWD_OutwardDate_dt is null
               and CONVERT (DATE, a.SolidINWD_CollectionDate_dt) >=CONVERT (DATE,'" + fromDate + "') and CONVERT (DATE,a.SolidINWD_CollectionDate_dt) <=CONVERT (DATE,'" + ToDate + "') " +
                @"and datediff(d,convert(date,a.SolidINWD_CollectionDate_dt),convert(date,GETDATE())) > ((select TEST_TimeFrame from tbl_Test as d where a.SolidINWD_TEST_Id=d.TEST_Id and d.TEST_TimeFrame is not null)+2)

                Union

                select  0 as AlertDtl_EnqNo_int,null as AlertDtl_EnqDate_date,a.STINWD_CollectionDate_dt as AlertDtl_CollctnEnqDate_date,'' as AlertDtl_BillNo_int,null as AlertDtl_BillDate_date,0 as AlertDtl_BillAmt_dec,
                '' as AlertDtl_ProNo_var,0 as AlertDtl_ProNo_int,null as AlertDtl_ProDate_date,STINWD_ReferenceNo_var as AlertDtl_RefNo_var,STINWD_RecordNo_int as AlertDtl_RecNo_int
                ,CL_Name_var as AlertDtl_CL_Name_var,SITE_Name_var as AlertDtl_SITE_Name_var,'ST' as AlertDtl_RecType_var,STINWD_CL_Id  as AlertDtl_CLId_int,STINWD_SITE_Id  as AlertDtl_SiteId_int,28  as AlertDtl_Alert_Id,
                (select USER_NAME_var from tbl_User x where x.USER_Id IN (select ROUTE_ME_Id from tbl_Route where ROUTE_Id=c.SITE_Route_Id)) as AlertDtl_MEName_var			
                  ,null as AlertDtl_ProOrderDate_date,null as AlertDtl_BillModifiedDate_date,null as AlertDtl_InwdRecDate_date
                ,null as AlertDtl_AppDate_date,null as AlertDtl_EnteredDate_date,null as AlertDtl_CheckedDate_date,null as AlertDtl_OutwrdDate_date,
                (select TEST_Name_var from tbl_Test as d where e.STTEST_TEST_id=d.TEST_Id ) as AlertDtl_TestName_var
                from tbl_Steel_Inward  as a,tbl_Steel_Test as e,tbl_Client as b,tbl_Site as c
                where a.STINWD_CL_Id=b.CL_Id
                and a.STINWD_SITE_Id=c.SITE_Id
                and a.STINWD_ReferenceNo_var=e.STTEST_ReferenceNo_var
                and a.STINWD_OutwardDate_dt is null
                 and CONVERT (DATE, a.STINWD_CollectionDate_dt) >=CONVERT (DATE,'" + fromDate + "') and CONVERT (DATE,a.STINWD_CollectionDate_dt) <=CONVERT (DATE,'" + ToDate + "') " +
                @"and datediff(d,convert(date,a.STINWD_CollectionDate_dt),convert(date,GETDATE())) > ((Select distinct(TEST_TimeFrame) from tbl_Test as d where Test_RecType_var='ST' and TEST_TimeFrame is NOT Null)+2)

                Union

                 select  0 as AlertDtl_EnqNo_int,null as AlertDtl_EnqDate_date,a.STCINWD_CollectionDate_dt as AlertDtl_CollctnEnqDate_date,'' as AlertDtl_BillNo_int,null as AlertDtl_BillDate_date,0 as AlertDtl_BillAmt_dec,
                '' as AlertDtl_ProNo_var,0 as AlertDtl_ProNo_int,null as AlertDtl_ProDate_date,STCINWD_ReferenceNo_var as AlertDtl_RefNo_var,STCINWD_RecordNo_int as AlertDtl_RecNo_int,
                CL_Name_var as AlertDtl_CL_Name_var,SITE_Name_var as AlertDtl_SITE_Name_var,'STC' as AlertDtl_RecType_var,STCINWD_CL_Id  as AlertDtl_CLId_int,STCINWD_SITE_Id  as AlertDtl_SiteId_int,28  as AlertDtl_Alert_Id,
                 (select USER_NAME_var from tbl_User x where x.USER_Id IN (select ROUTE_ME_Id from tbl_Route where ROUTE_Id=c.SITE_Route_Id)) as AlertDtl_MEName_var			
                  ,null as AlertDtl_ProOrderDate_date,null as AlertDtl_BillModifiedDate_date,null as AlertDtl_InwdRecDate_date
                ,null as AlertDtl_AppDate_date,null as AlertDtl_EnteredDate_date,null as AlertDtl_CheckedDate_date,null as AlertDtl_OutwrdDate_date,
               (select TEST_Name_var from tbl_Test as d where a.STCINWD_TEST_id=d.TEST_Id ) as AlertDtl_TestName_var
             from tbl_SteelChemical_Inward  as a,tbl_Client as b,tbl_Site as c
                where a.STCINWD_CL_Id=b.CL_Id
                and a.STCINWD_SITE_Id=c.SITE_Id
                and a.STCINWD_OutwardDate_dt is null
                and CONVERT (DATE, a.STCINWD_CollectionDate_dt) >=CONVERT (DATE,'" + fromDate + "') and CONVERT (DATE,a.STCINWD_CollectionDate_dt) <=CONVERT (DATE,'" + ToDate + "') " +
                @"and datediff(d,convert(date,a.STCINWD_CollectionDate_dt),convert(date,GETDATE())) > ((Select distinct(TEST_TimeFrame) from tbl_Test as d where Test_RecType_var='STC' and TEST_TimeFrame is NOT Null)+2)

                Union

                select  0 as AlertDtl_EnqNo_int,null as AlertDtl_EnqDate_date,a.TILEINWD_CollectionDate_dt as AlertDtl_CollctnEnqDate_date,'' as AlertDtl_BillNo_int,null as AlertDtl_BillDate_date,0 as AlertDtl_BillAmt_dec,
                '' as AlertDtl_ProNo_var,0 as AlertDtl_ProNo_int,null as AlertDtl_ProDate_date,TILEINWD_ReferenceNo_var as AlertDtl_RefNo_var,TILEINWD_RecordNo_int as AlertDtl_RecNo_int
                ,CL_Name_var as AlertDtl_CL_Name_var,SITE_Name_var as AlertDtl_SITE_Name_var,'TILE' as AlertDtl_RecType_var,TILEINWD_CL_Id  as AlertDtl_CLId_int,TILEINWD_SITE_Id  as AlertDtl_SiteId_int,28  as AlertDtl_Alert_Id,
                (select USER_NAME_var from tbl_User x where x.USER_Id IN (select ROUTE_ME_Id from tbl_Route where ROUTE_Id=c.SITE_Route_Id)) as AlertDtl_MEName_var			
                  ,null as AlertDtl_ProOrderDate_date,null as AlertDtl_BillModifiedDate_date,null as AlertDtl_InwdRecDate_date
                ,null as AlertDtl_AppDate_date,null as AlertDtl_EnteredDate_date,null as AlertDtl_CheckedDate_date,null as AlertDtl_OutwrdDate_date,
                (select TEST_Name_var from tbl_Test as d where a.TileINWD_TEST_id=d.TEST_Id ) as AlertDtl_TestName_var
                from tbl_Tile_Inward  as a,tbl_Client as b,tbl_Site as c
                where a.TileINWD_CL_Id=b.CL_Id
                and a.TileINWD_SITE_Id=c.SITE_Id
                and a.TILEINWD_OutwardDate_dt is null
                and CONVERT (DATE, a.TILEINWD_CollectionDate_dt) >=CONVERT (DATE,'" + fromDate + "') and CONVERT (DATE,a.TILEINWD_CollectionDate_dt) <=CONVERT (DATE,'" + ToDate + "') " +
                @"and datediff(d,convert(date,a.TILEINWD_CollectionDate_dt),convert(date,GETDATE())) > ((Select distinct(TEST_TimeFrame) from tbl_Test as d where Test_RecType_var='TILE' and TEST_TimeFrame is NOT Null)+2)

                Union


                 select  0 as AlertDtl_EnqNo_int,null as AlertDtl_EnqDate_date,a.WTINWD_CollectionDate_dt as AlertDtl_CollctnEnqDate_date,'' as AlertDtl_BillNo_int,null as AlertDtl_BillDate_date,0 as AlertDtl_BillAmt_dec,
                '' as AlertDtl_ProNo_var,0 as AlertDtl_ProNo_int,null as AlertDtl_ProDate_date,WTINWD_ReferenceNo_var as AlertDtl_RefNo_var,WTINWD_RecordNo_int as AlertDtl_RecNo_int
                ,CL_Name_var as AlertDtl_CL_Name_var,SITE_Name_var as AlertDtl_SITE_Name_var,'WT' as AlertDtl_RecType_var,WTINWD_CL_Id  as AlertDtl_CLId_int,WTINWD_SITE_Id  as AlertDtl_SiteId_int,28  as AlertDtl_Alert_Id,
                (select USER_NAME_var from tbl_User x where x.USER_Id IN (select ROUTE_ME_Id from tbl_Route where ROUTE_Id=c.SITE_Route_Id)) as AlertDtl_MEName_var			
                ,null as AlertDtl_ProOrderDate_date,null as AlertDtl_BillModifiedDate_date,null as AlertDtl_InwdRecDate_date
                ,null as AlertDtl_AppDate_date,null as AlertDtl_EnteredDate_date,null as AlertDtl_CheckedDate_date,null as AlertDtl_OutwrdDate_date,
               (select TEST_Name_var from tbl_Test as d where e.WTTEST_TEST_id=d.TEST_Id ) as AlertDtl_TestName_var
               from tbl_Water_Inward  as a,tbl_Water_Test as e,tbl_Client as b,tbl_Site as c
                where a.WTINWD_CL_Id=b.CL_Id
                and a.WTINWD_SITE_Id=c.SITE_Id
                and a.WTINWD_ReferenceNo_var=e.WTTEST_ReferenceNo_var
                and a.WTINWD_OutwardDate_dt is null
               and CONVERT (DATE, a.WTINWD_CollectionDate_dt) >=CONVERT (DATE,'" + fromDate + "') and CONVERT (DATE,a.WTINWD_CollectionDate_dt) <=CONVERT (DATE,'" + ToDate + "') " +
                @"and datediff(d,convert(date,a.WTINWD_CollectionDate_dt),convert(date,GETDATE())) > ((Select distinct(TEST_TimeFrame) from tbl_Test as d where Test_RecType_var='WT' and TEST_TimeFrame is NOT Null)+2)

			
             ";
                    }
                }
                else if (Alert_Id == 30)
                {
                    if (flagStatus == 0)
                    {
                        str = @"--No of tests completed for which reports are not checked in 1 day
                Select 0 as AlertDtl_EnqNo_int,null as AlertDtl_EnqDate_date,f.MISEnteredDt as AlertDtl_EnteredDate_date,'' as AlertDtl_BillNo_int,null as AlertDtl_BillDate_date,0 as AlertDtl_BillAmt_dec,
				'' as AlertDtl_ProNo_var,0 as AlertDtl_ProNo_int,null as AlertDtl_ProDate_date,MISRefNo as AlertDtl_RefNo_var,MISRecordNo  as AlertDtl_RecNo_int,
				c.CL_Name_var as AlertDtl_CL_Name_var,d.SITE_Name_var as AlertDtl_SITE_Name_var,e.MATERIAL_Name_var as AlertDtl_RecType_var,c.CL_Id,d.SITE_Id,30 as AlertDtl_Alert_Id
				,(select USER_NAME_var from tbl_User x where x.USER_Id IN (select ROUTE_ME_Id from tbl_Route where ROUTE_Id=d.SITE_Route_Id)) as AlertDtl_MEName_var
				,null as AlertDtl_ProOrderDate_date,null as AlertDtl_BillModifiedDate_date,null as AlertDtl_AppDate_date
                ,null as AlertDtl_CollctnEnqDate_date,null as AlertDtl_InwdRecDate_date,null as AlertDtl_CheckedDate_date,null as AlertDtl_OutwrdDate_date,'' as AlertDtl_TestName_var
                from tbl_Enquiry as a,tbl_Inward as b,tbl_Client as c,tbl_Site as d,tbl_Material as e,tbl_MISDetail as f
				where a.ENQ_Id=b.INWD_ENQ_Id
				and b.INWD_RecordNo_int=f.MISRecordNo
				and b.INWD_RecordType_var=f.MISRecType
				and a.ENQ_CL_Id=c.CL_Id
				and a.ENQ_SITE_Id=d.SITE_Id
				and a.ENQ_MATERIAL_Id=e.MATERIAL_Id
				and CONVERT (DATE, f.MISEnteredDt) >=CONVERT(DATE,'" + fromDate + "') and CONVERT (DATE, f.MISEnteredDt) <=CONVERT(DATE,'" + ToDate + "')  " +
                @"and datediff(d,convert(date,f.MISEnteredDt),convert(date,f.MISCheckedDt)) > 1
				and datediff(d,convert(date,f.MISEnteredDt),convert(date,GETDATE())) > 1 
                 order by AlertDtl_RecType_var";
                    }
                    else
                    {
                        str = @"Select 0 as AlertDtl_EnqNo_int,null as AlertDtl_EnqDate_date,f.MISEnteredDt as AlertDtl_EnteredDate_date,'' as AlertDtl_BillNo_int,null as AlertDtl_BillDate_date,0 as AlertDtl_BillAmt_dec,
				'' as AlertDtl_ProNo_var,0 as AlertDtl_ProNo_int,null as AlertDtl_ProDate_date,MISRefNo as AlertDtl_RefNo_var,MISRecordNo  as AlertDtl_RecNo_int,
				c.CL_Name_var as AlertDtl_CL_Name_var,d.SITE_Name_var as AlertDtl_SITE_Name_var,e.MATERIAL_Name_var as AlertDtl_RecType_var,c.CL_Id,d.SITE_Id,30 as AlertDtl_Alert_Id
				,(select USER_NAME_var from tbl_User x where x.USER_Id IN (select ROUTE_ME_Id from tbl_Route where ROUTE_Id=d.SITE_Route_Id)) as AlertDtl_MEName_var
				,null as AlertDtl_ProOrderDate_date,null as AlertDtl_BillModifiedDate_date,null as AlertDtl_AppDate_date
                ,null as AlertDtl_CollctnEnqDate_date,null as AlertDtl_InwdRecDate_date,null as AlertDtl_CheckedDate_date,null as AlertDtl_OutwrdDate_date,'' as AlertDtl_TestName_var
                from tbl_Enquiry as a,tbl_Inward as b,tbl_Client as c,tbl_Site as d,tbl_Material as e,tbl_MISDetail as f
				where a.ENQ_Id=b.INWD_ENQ_Id
				and b.INWD_RecordNo_int=f.MISRecordNo
				and b.INWD_RecordType_var=f.MISRecType
				and a.ENQ_CL_Id=c.CL_Id
				and a.ENQ_SITE_Id=d.SITE_Id
				and a.ENQ_MATERIAL_Id=e.MATERIAL_Id
				and CONVERT (DATE, f.MISEnteredDt) >=CONVERT(DATE,'" + fromDate + "') and CONVERT (DATE, f.MISEnteredDt) <=CONVERT(DATE,'" + ToDate + "')  " +
                @"and f.MISCheckedDt is null and datediff(d,convert(date,f.MISEnteredDt),convert(date,GETDATE())) > 1 
				order by AlertDtl_RecType_var";
                    }
                }
                else if (Alert_Id == 31)
                {
                    if (flagStatus == 0)
                    {
                        str = @"--No of reports not approved in 1 day after generation 
                Select a.ENQ_Id as AlertDtl_EnqNo_int,a.ENQ_Date_dt as AlertDtl_EnqDate_date,f.MISEnteredDt as AlertDtl_EnteredDate_date,'' as AlertDtl_BillNo_int,null as AlertDtl_BillDate_date,0 as AlertDtl_BillAmt_dec,
				'' as AlertDtl_ProNo_var,0 as AlertDtl_ProNo_int,null as AlertDtl_ProDate_date,MISRefNo as AlertDtl_RefNo_var,MISRecordNo  as AlertDtl_RecNo_int,
				c.CL_Name_var as AlertDtl_CL_Name_var,d.SITE_Name_var as AlertDtl_SITE_Name_var,e.MATERIAL_Name_var as AlertDtl_RecType_var,c.CL_Id,d.SITE_Id,31 as AlertDtl_Alert_Id
				,(select USER_NAME_var from tbl_User x where x.USER_Id IN (select ROUTE_ME_Id from tbl_Route where ROUTE_Id=d.SITE_Route_Id)) as AlertDtl_MEName_var
				,null as AlertDtl_ProOrderDate_date,null as AlertDtl_BillModifiedDate_date,null as AlertDtl_AppDate_date
                ,null as AlertDtl_CollctnEnqDate_date,null as AlertDtl_InwdRecDate_date,null as AlertDtl_CheckedDate_date,null as AlertDtl_OutwrdDate_date,'' as AlertDtl_TestName_var
                from tbl_Enquiry as a,tbl_Inward as b,tbl_Client as c,tbl_Site as d,tbl_Material as e,tbl_MISDetail as f
				where a.ENQ_Id=b.INWD_ENQ_Id
				and b.INWD_RecordNo_int=f.MISRecordNo
				and b.INWD_RecordType_var=f.MISRecType
				and a.ENQ_CL_Id=c.CL_Id
				and a.ENQ_SITE_Id=d.SITE_Id
				and a.ENQ_MATERIAL_Id=e.MATERIAL_Id
				and CONVERT (DATE, f.MISEnteredDt) >=CONVERT(DATE,'" + fromDate + "') and CONVERT (DATE, f.MISEnteredDt) <=CONVERT(DATE,'" + ToDate + "')  " +
                @"and datediff(d,convert(date,f.MISEnteredDt),convert(date,f.MISApprovedDt)) > 1
				and datediff(d,convert(date,f.MISEnteredDt),convert(date,GETDATE())) > 1  order by AlertDtl_RecType_var";
                    }
                    else
                    {
                        str = @"Select a.ENQ_Id as AlertDtl_EnqNo_int,a.ENQ_Date_dt as AlertDtl_EnqDate_date,f.MISEnteredDt as AlertDtl_EnteredDate_date,'' as AlertDtl_BillNo_int,null as AlertDtl_BillDate_date,0 as AlertDtl_BillAmt_dec,
				'' as AlertDtl_ProNo_var,0 as AlertDtl_ProNo_int,null as AlertDtl_ProDate_date,MISRefNo as AlertDtl_RefNo_var,MISRecordNo  as AlertDtl_RecNo_int,
				c.CL_Name_var as AlertDtl_CL_Name_var,d.SITE_Name_var as AlertDtl_SITE_Name_var,e.MATERIAL_Name_var as AlertDtl_RecType_var,c.CL_Id,d.SITE_Id,31 as AlertDtl_Alert_Id
				,(select USER_NAME_var from tbl_User x where x.USER_Id IN (select ROUTE_ME_Id from tbl_Route where ROUTE_Id=d.SITE_Route_Id)) as AlertDtl_MEName_var
				,null as AlertDtl_ProOrderDate_date,null as AlertDtl_BillModifiedDate_date,null as AlertDtl_AppDate_date
                ,null as AlertDtl_CollctnEnqDate_date,null as AlertDtl_InwdRecDate_date,null as AlertDtl_CheckedDate_date,null as AlertDtl_OutwrdDate_date,'' as AlertDtl_TestName_var
                from tbl_Enquiry as a,tbl_Inward as b,tbl_Client as c,tbl_Site as d,tbl_Material as e,tbl_MISDetail as f
				where a.ENQ_Id=b.INWD_ENQ_Id
				and b.INWD_RecordNo_int=f.MISRecordNo
				and b.INWD_RecordType_var=f.MISRecType
				and a.ENQ_CL_Id=c.CL_Id
				and a.ENQ_SITE_Id=d.SITE_Id
				and a.ENQ_MATERIAL_Id=e.MATERIAL_Id
			    and CONVERT (DATE, f.MISEnteredDt) >=CONVERT(DATE,'" + fromDate + "') and CONVERT (DATE, f.MISEnteredDt) <=CONVERT(DATE,'" + ToDate + "')  " +
                 @"and f.MISApprovedDt is null 
                and datediff(d,convert(date,f.MISEnteredDt),convert(date,GETDATE())) > 1 
				 order by AlertDtl_RecType_var";
                    }
                }
                else if (Alert_Id == 32)
                {
                    if (flagStatus == 0)
                    {
                        str = @"--No of reports which are approved but bills are not generated in 1 day
                Select a.ENQ_Id as AlertDtl_EnqNo_int,a.ENQ_Date_dt as AlertDtl_EnqDate_date,f.MISApprovedDt as AlertDtl_AppDate_date,'' as AlertDtl_BillNo_int,null as AlertDtl_BillDate_date,0 as AlertDtl_BillAmt_dec,
				'' as AlertDtl_ProNo_var,0 as AlertDtl_ProNo_int,null as AlertDtl_ProDate_date,MISRefNo as AlertDtl_RefNo_var,MISRecordNo  as AlertDtl_RecNo_int,
				c.CL_Name_var as AlertDtl_CL_Name_var,d.SITE_Name_var as AlertDtl_SITE_Name_var,e.MATERIAL_Name_var as AlertDtl_RecType_var,c.CL_Id,d.SITE_Id,32 as AlertDtl_Alert_Id
				,(select USER_NAME_var from tbl_User x where x.USER_Id IN (select ROUTE_ME_Id from tbl_Route where ROUTE_Id=d.SITE_Route_Id)) as AlertDtl_MEName_var
				,null as AlertDtl_ProOrderDate_date,null as AlertDtl_BillModifiedDate_date,null as AlertDtl_EnteredDate_date
                ,null as AlertDtl_CollctnEnqDate_date,null as AlertDtl_InwdRecDate_date,null as AlertDtl_CheckedDate_date,null as AlertDtl_OutwrdDate_date,'' as AlertDtl_TestName_var
                from tbl_Enquiry as a,tbl_Inward as b,tbl_Client as c,tbl_Site as d,tbl_Material as e,tbl_MISDetail as f,tbl_Bill as g
				where a.ENQ_Id=b.INWD_ENQ_Id
				and b.INWD_BILL_Id=g.BILL_Id
				and b.INWD_RecordNo_int=f.MISRecordNo
				and b.INWD_RecordType_var=f.MISRecType
				and a.ENQ_CL_Id=c.CL_Id
				and a.ENQ_SITE_Id=d.SITE_Id
				and a.ENQ_MATERIAL_Id=e.MATERIAL_Id
				and CONVERT (DATE, f.MISApprovedDt) >=CONVERT(DATE,'" + fromDate + "') and CONVERT (DATE, f.MISApprovedDt) <=CONVERT(DATE,'" + ToDate + "')  " +
                @" and datediff(d,convert(date,f.MISApprovedDt),convert(date,g.BILL_Date_dt)) > 1
				and datediff(d,convert(date,f.MISApprovedDt),convert(date,GETDATE())) > 1 
				order by AlertDtl_RecType_var";
                    }
                    else
                    {
                        str = @"Select a.ENQ_Id as AlertDtl_EnqNo_int,a.ENQ_Date_dt as AlertDtl_EnqDate_date,f.MISApprovedDt as AlertDtl_AppDate_date,'' as AlertDtl_BillNo_int,null as AlertDtl_BillDate_date,0 as AlertDtl_BillAmt_dec,
				'' as AlertDtl_ProNo_var,0 as AlertDtl_ProNo_int,null as AlertDtl_ProDate_date,MISRefNo as AlertDtl_RefNo_var,MISRecordNo  as AlertDtl_RecNo_int,
				c.CL_Name_var as AlertDtl_CL_Name_var,d.SITE_Name_var as AlertDtl_SITE_Name_var,e.MATERIAL_Name_var as AlertDtl_RecType_var,c.CL_Id,d.SITE_Id,32 as AlertDtl_Alert_Id
				,(select USER_NAME_var from tbl_User x where x.USER_Id IN (select ROUTE_ME_Id from tbl_Route where ROUTE_Id=d.SITE_Route_Id)) as AlertDtl_MEName_var
				,null as AlertDtl_ProOrderDate_date,null as AlertDtl_BillModifiedDate_date,null as AlertDtl_EnteredDate_date
                ,null as AlertDtl_CollctnEnqDate_date,null as AlertDtl_InwdRecDate_date,null as AlertDtl_CheckedDate_date,null as AlertDtl_OutwrdDate_date,'' as AlertDtl_TestName_var
                from tbl_Enquiry as a,tbl_Inward as b,tbl_Client as c,tbl_Site as d,tbl_Material as e,tbl_MISDetail as f,tbl_Bill as g
				where a.ENQ_Id=b.INWD_ENQ_Id
				and b.INWD_BILL_Id=g.BILL_Id
				and b.INWD_RecordNo_int=f.MISRecordNo
				and b.INWD_RecordType_var=f.MISRecType
				and a.ENQ_CL_Id=c.CL_Id
				and a.ENQ_SITE_Id=d.SITE_Id
				and a.ENQ_MATERIAL_Id=e.MATERIAL_Id
				 and CONVERT (DATE, f.MISApprovedDt) >=CONVERT(DATE,'" + fromDate + "') and CONVERT (DATE, f.MISApprovedDt) <=CONVERT(DATE,'" + ToDate + "')  " +
                @"and g.BILL_Date_dt is null
                and datediff(d,convert(date,f.MISApprovedDt),convert(date,GETDATE())) > 1
				order by AlertDtl_RecType_var";
                    }
                }
                else if (Alert_Id == 33)
                {
                    if (flagStatus == 0)
                    {
                        str = @"--No of reports pending for printing  for 1 day after approval of report
                Select a.ENQ_Id as AlertDtl_EnqNo_int,a.ENQ_Date_dt as AlertDtl_EnqDate_date,f.MISApprovedDt as AlertDtl_AppDate_date,'' as AlertDtl_BillNo_int,null as AlertDtl_BillDate_date,0 as AlertDtl_BillAmt_dec,
				'' as AlertDtl_ProNo_var,0 as AlertDtl_ProNo_int,null as AlertDtl_ProDate_date,f.MISRefNo as AlertDtl_RefNo_var,f.MISRecordNo  as AlertDtl_RecNo_int,
				c.CL_Name_var as AlertDtl_CL_Name_var,d.SITE_Name_var as AlertDtl_SITE_Name_var,e.MATERIAL_Name_var as AlertDtl_RecType_var,c.CL_Id,d.SITE_Id,33 as AlertDtl_Alert_Id
				,(select USER_NAME_var from tbl_User x where x.USER_Id IN (select ROUTE_ME_Id from tbl_Route where ROUTE_Id=d.SITE_Route_Id)) as AlertDtl_MEName_var
				,null as AlertDtl_ProOrderDate_date,null as AlertDtl_BillModifiedDate_date,null as AlertDtl_EnteredDate_date
                ,null as AlertDtl_CollctnEnqDate_date,null as AlertDtl_InwdRecDate_date,null as AlertDtl_CheckedDate_date,null as AlertDtl_OutwrdDate_date,'' as AlertDtl_TestName_var
                from tbl_Enquiry as a,tbl_Inward as b,tbl_Client as c,tbl_Site as d,tbl_Material as e,tbl_MISDetail as f
				where a.ENQ_Id=b.INWD_ENQ_Id
				and b.INWD_RecordNo_int=f.MISRecordNo
				and b.INWD_RecordType_var=f.MISRecType
				and a.ENQ_CL_Id=c.CL_Id
				and a.ENQ_SITE_Id=d.SITE_Id
				and a.ENQ_MATERIAL_Id=e.MATERIAL_Id
				and CONVERT (DATE, f.MISApprovedDt) >=CONVERT(DATE,'" + fromDate + "') and CONVERT (DATE, f.MISApprovedDt) <=CONVERT(DATE,'" + ToDate + "')  " +
                @"and datediff(d,convert(date,f.MISApprovedDt),convert(date,f.MISIssueDt)) > 1
				and datediff(d,convert(date,f.MISApprovedDt),convert(date,GETDATE())) > 1 
				order by AlertDtl_RecType_var";
                    }
                    else
                    {
                        str = @"Select a.ENQ_Id as AlertDtl_EnqNo_int,a.ENQ_Date_dt as AlertDtl_EnqDate_date,f.MISApprovedDt as AlertDtl_AppDate_date,'' as AlertDtl_BillNo_int,null as AlertDtl_BillDate_date,0 as AlertDtl_BillAmt_dec,
				'' as AlertDtl_ProNo_var,0 as AlertDtl_ProNo_int,null as AlertDtl_ProDate_date,f.MISRefNo as AlertDtl_RefNo_var,f.MISRecordNo  as AlertDtl_RecNo_int,
				c.CL_Name_var as AlertDtl_CL_Name_var,d.SITE_Name_var as AlertDtl_SITE_Name_var,e.MATERIAL_Name_var as AlertDtl_RecType_var,c.CL_Id,d.SITE_Id,33 as AlertDtl_Alert_Id
				,(select USER_NAME_var from tbl_User x where x.USER_Id IN (select ROUTE_ME_Id from tbl_Route where ROUTE_Id=d.SITE_Route_Id)) as AlertDtl_MEName_var
				,null as AlertDtl_ProOrderDate_date,null as AlertDtl_BillModifiedDate_date,null as AlertDtl_EnteredDate_date
                ,null as AlertDtl_CollctnEnqDate_date,null as AlertDtl_InwdRecDate_date,null as AlertDtl_CheckedDate_date,null as AlertDtl_OutwrdDate_date,'' as AlertDtl_TestName_var
                from tbl_Enquiry as a,tbl_Inward as b,tbl_Client as c,tbl_Site as d,tbl_Material as e,tbl_MISDetail as f
				where a.ENQ_Id=b.INWD_ENQ_Id
				and b.INWD_RecordNo_int=f.MISRecordNo
				and b.INWD_RecordType_var=f.MISRecType
				and a.ENQ_CL_Id=c.CL_Id
				and a.ENQ_SITE_Id=d.SITE_Id
				and a.ENQ_MATERIAL_Id=e.MATERIAL_Id
				and CONVERT (DATE, f.MISApprovedDt) >=CONVERT(DATE,'" + fromDate + "') and CONVERT (DATE, f.MISApprovedDt) <=CONVERT(DATE,'" + ToDate + "')  " +
                @"and f.MISIssueDt is null and datediff(d,convert(date,f.MISApprovedDt),convert(date,GETDATE())) > 1 
				order by AlertDtl_RecType_var";
                    }
                }
                else if (Alert_Id == 34)
                {
                    if (flagStatus == 0)
                    {
                        str = @"--No of reports pending for printing  for 3 days after approval of report
                Select a.ENQ_Id as AlertDtl_EnqNo_int,a.ENQ_Date_dt as AlertDtl_EnqDate_date,f.MISApprovedDt as AlertDtl_AppDate_date,'' as AlertDtl_BillNo_int,null as AlertDtl_BillDate_date,0 as AlertDtl_BillAmt_dec,
				'' as AlertDtl_ProNo_var,0 as AlertDtl_ProNo_int,null as AlertDtl_ProDate_date,f.MISRefNo as AlertDtl_RefNo_var,f.MISRecordNo  as AlertDtl_RecNo_int,
				c.CL_Name_var as AlertDtl_CL_Name_var,d.SITE_Name_var as AlertDtl_SITE_Name_var,e.MATERIAL_Name_var as AlertDtl_RecType_var,c.CL_Id,d.SITE_Id,34 as AlertDtl_Alert_Id
				,(select USER_NAME_var from tbl_User x where x.USER_Id IN (select ROUTE_ME_Id from tbl_Route where ROUTE_Id=d.SITE_Route_Id)) as AlertDtl_MEName_var
				,null as AlertDtl_ProOrderDate_date,null as AlertDtl_BillModifiedDate_date,null as AlertDtl_EnteredDate_date
                ,null as AlertDtl_CollctnEnqDate_date,null as AlertDtl_InwdRecDate_date,null as AlertDtl_CheckedDate_date,null as AlertDtl_OutwrdDate_date,'' as AlertDtl_TestName_var
                from tbl_Enquiry as a,tbl_Inward as b,tbl_Client as c,tbl_Site as d,tbl_Material as e,tbl_MISDetail as f
				where a.ENQ_Id=b.INWD_ENQ_Id
				and b.INWD_RecordNo_int=f.MISRecordNo
				and b.INWD_RecordType_var=f.MISRecType
				and a.ENQ_CL_Id=c.CL_Id
				and a.ENQ_SITE_Id=d.SITE_Id
				and a.ENQ_MATERIAL_Id=e.MATERIAL_Id
				and CONVERT (DATE, f.MISApprovedDt) >=CONVERT(DATE,'" + fromDate + "') and CONVERT (DATE, f.MISApprovedDt) <=CONVERT(DATE,'" + ToDate + "')  " +
                @"and datediff(d,convert(date,f.MISApprovedDt),convert(date,f.MISIssueDt)) <=3
				and datediff(d,convert(date,f.MISApprovedDt),convert(date,GETDATE())) <=3 order by AlertDtl_RecType_var";
                    }
                    else
                    {
                        str = @"Select a.ENQ_Id as AlertDtl_EnqNo_int,a.ENQ_Date_dt as AlertDtl_EnqDate_date,f.MISApprovedDt as AlertDtl_AppDate_date,'' as AlertDtl_BillNo_int,null  as AlertDtl_BillDate_date,
                    0 as AlertDtl_BillAmt_dec,
				'' as AlertDtl_ProNo_var as AlertDtl_ProNo_var ,0 as AlertDtl_ProNo_int ,null as AlertDtl_ProDate_date asAlertDtl_ProDate_date,
                f.MISRefNo as AlertDtl_RefNo_var,f.MISRecordNo as AlertDtl_RecNo_int,
				c.CL_Name_var as AlertDtl_CL_Name_var as AlertDtl_CL_Name_var,d.SITE_Name_var as AlertDtl_SITE_Name_var as AlertDtl_SITE_Name_var,e.MATERIAL_Name_var as AlertDtl_RecType_var,c.CL_Id,d.SITE_Id,34 as AlertDtl_Alert_Id
				,(select USER_NAME_var from tbl_User x where x.USER_Id IN (select ROUTE_ME_Id from tbl_Route where ROUTE_Id=d.SITE_Route_Id)) as AlertDtl_MEName_var
				,null as AlertDtl_ProOrderDate_date,null as AlertDtl_BillModifiedDate_date,null as AlertDtl_EnteredDate_date
                ,null as AlertDtl_CollctnEnqDate_date,null as AlertDtl_InwdRecDate_date,null as AlertDtl_CheckedDate_date,null as AlertDtl_OutwrdDate_date,'' as AlertDtl_TestName_var
                from tbl_Enquiry as a,tbl_Inward as b,tbl_Client as c,tbl_Site as d,tbl_Material as e,tbl_MISDetail as f
				where a.ENQ_Id=b.INWD_ENQ_Id
				and b.INWD_RecordNo_int=f.MISRecordNo
				and b.INWD_RecordType_var=f.MISRecType
				and a.ENQ_CL_Id=c.CL_Id
				and a.ENQ_SITE_Id=d.SITE_Id
				and a.ENQ_MATERIAL_Id=e.MATERIAL_Id
				and CONVERT (DATE, f.MISApprovedDt) >=CONVERT(DATE,'" + fromDate + "') and CONVERT (DATE, f.MISApprovedDt) <=CONVERT(DATE,'" + ToDate + "')  " +
                @"and f.MISIssueDt is null and datediff(d,convert(date,f.MISApprovedDt),convert(date,GETDATE())) <= 3
				order by AlertDtl_RecType_var";
                    }
                }
                else if (Alert_Id == 35)
                {
                    if (flagStatus == 0)
                    {
                        str = @"--No of reports pending for printing  for more than 3 days after approval of report
                    Select a.ENQ_Id as AlertDtl_EnqNo_int,a.ENQ_Date_dt as AlertDtl_EnqDate_date,f.MISApprovedDt as AlertDtl_AppDate_date,'' as AlertDtl_BillNo_int,null as AlertDtl_BillDate_date,0 as AlertDtl_BillAmt_dec,
				'' as AlertDtl_ProNo_var,0 as AlertDtl_ProNo_int,null as AlertDtl_ProDate_date,f.MISRefNo as AlertDtl_RefNo_var,f.MISRecordNo  as AlertDtl_RecNo_int,
				c.CL_Name_var as AlertDtl_CL_Name_var,d.SITE_Name_var as AlertDtl_SITE_Name_var,e.MATERIAL_Name_var as AlertDtl_RecType_var,c.CL_Id,d.SITE_Id,35 as AlertDtl_Alert_Id
				,(select USER_NAME_var from tbl_User x where x.USER_Id IN (select ROUTE_ME_Id from tbl_Route where ROUTE_Id=d.SITE_Route_Id)) as AlertDtl_MEName_var
				,null as AlertDtl_ProOrderDate_date,null as AlertDtl_BillModifiedDate_date,null as AlertDtl_EnteredDate_date
                ,null as AlertDtl_CollctnEnqDate_date,null as AlertDtl_InwdRecDate_date,null as AlertDtl_CheckedDate_date,null as AlertDtl_OutwrdDate_date,'' as AlertDtl_TestName_var
                from tbl_Enquiry as a,tbl_Inward as b,tbl_Client as c,tbl_Site as d,tbl_Material as e,tbl_MISDetail as f
				where a.ENQ_Id=b.INWD_ENQ_Id
				and b.INWD_RecordNo_int=f.MISRecordNo
				and b.INWD_RecordType_var=f.MISRecType
				and a.ENQ_CL_Id=c.CL_Id
				and a.ENQ_SITE_Id=d.SITE_Id
				and a.ENQ_MATERIAL_Id=e.MATERIAL_Id
				and CONVERT (DATE, f.MISApprovedDt) >=CONVERT(DATE,'" + fromDate + "') and CONVERT (DATE, f.MISApprovedDt) <=CONVERT(DATE,'" + ToDate + "')  " +
                @"and datediff(d,convert(date,f.MISApprovedDt),convert(date,f.MISIssueDt)) >3
				and datediff(d,convert(date,f.MISApprovedDt),convert(date,GETDATE())) >3 
				order by AlertDtl_RecType_var";
                    }
                    else
                    {
                        str = @"Select a.ENQ_Id as AlertDtl_EnqNo_int,a.ENQ_Date_dt as AlertDtl_EnqDate_date,f.MISApprovedDt as AlertDtl_AppDate_date,'' as AlertDtl_BillNo_int,null as AlertDtl_BillDate_date,0 as AlertDtl_BillAmt_dec,
				'' as AlertDtl_ProNo_var,0 as AlertDtl_ProNo_int,null as AlertDtl_ProDate_date,f.MISRefNo as AlertDtl_RefNo_var,f.MISRecordNo,
				c.CL_Name_var as AlertDtl_CL_Name_var,d.SITE_Name_var as AlertDtl_SITE_Name_var,e.MATERIAL_Name_var as AlertDtl_RecType_var,c.CL_Id,d.SITE_Id,35 as AlertDtl_Alert_Id
				,(select USER_NAME_var from tbl_User x where x.USER_Id IN (select ROUTE_ME_Id from tbl_Route where ROUTE_Id=d.SITE_Route_Id)) as AlertDtl_MEName_var
				,null as AlertDtl_ProOrderDate_date,null as AlertDtl_BillModifiedDate_date,null as AlertDtl_EnteredDate_date
                ,null as AlertDtl_CollctnEnqDate_date,null as AlertDtl_InwdRecDate_date,null as AlertDtl_CheckedDate_date,null as AlertDtl_OutwrdDate_date,'' as AlertDtl_TestName_var
                from tbl_Enquiry as a,tbl_Inward as b,tbl_Client as c,tbl_Site as d,tbl_Material as e,tbl_MISDetail as f
				where a.ENQ_Id=b.INWD_ENQ_Id
				and b.INWD_RecordNo_int=f.MISRecordNo
				and b.INWD_RecordType_var=f.MISRecType
				and a.ENQ_CL_Id=c.CL_Id
				and a.ENQ_SITE_Id=d.SITE_Id
				and a.ENQ_MATERIAL_Id=e.MATERIAL_Id
				and CONVERT (DATE, f.MISApprovedDt) >=CONVERT(DATE,'" + fromDate + "') and CONVERT (DATE, f.MISApprovedDt) <=CONVERT(DATE,'" + ToDate + "')  " +
                @"and f.MISIssueDt is null and datediff(d,convert(date,f.MISApprovedDt),convert(date,GETDATE())) > 3 
				order by AlertDtl_RecType_var";
                    }
                }
                else if (Alert_Id == 36)
                {
                    if (flagStatus == 0)
                    {
                        str = @"--No of bills modified 
                Select 0  as AlertDtl_EnqNo_int,null  as AlertDtl_EnqDate_date,a.BILL_ModifiedOn_dt as AlertDtl_BillModifiedDate_date,a.BILL_Id as AlertDtl_BillNo_int,a.BILL_Date_dt as AlertDtl_BillDate_date,a.BILL_NetAmt_num as AlertDtl_BillAmt_dec,
				'' as AlertDtl_ProNo_var,0 as AlertDtl_ProNo_int,null as AlertDtl_ProDate_date,'' as AlertDtl_RefNo_var,0 as AlertDtl_RecNo_int,
				c.CL_Name_var as AlertDtl_CL_Name_var,d.SITE_Name_var as AlertDtl_SITE_Name_var,(select MATERIAL_Name_var from tbl_Material e where e.MATERIAL_RecordType_var=a.BILL_RecordType_var)  as AlertDtl_RecType_var,
				c.CL_Id,d.SITE_Id,36 as AlertDtl_Alert_Id
				,(select USER_NAME_var from tbl_User x where x.USER_Id IN (select ROUTE_ME_Id from tbl_Route where ROUTE_Id=d.SITE_Route_Id)) as AlertDtl_MEName_var
				,null as AlertDtl_ProOrderDate_date,null as AlertDtl_EnteredDate_date
                ,null as AlertDtl_CollctnEnqDate_date,null as AlertDtl_InwdRecDate_date,null as AlertDtl_CheckedDate_date,null as AlertDtl_OutwrdDate_date,null as AlertDtl_AppDate_date,'' as AlertDtl_TestName_var
                from tbl_Client as c,tbl_Site as d,tbl_Bill as a
				where  a.BILL_CL_Id=c.CL_Id 
				and a.Bill_SITE_Id=d.SITE_Id
				and CONVERT (DATE, A.BILL_Date_dt) >=CONVERT(DATE,'" + fromDate + "') and CONVERT (DATE, A.BILL_Date_dt) <=CONVERT(DATE,'" + ToDate + "') " +
                @"and datediff(d,convert(date,a.BILL_Date_dt),convert(date,a.BILL_ModifiedOn_dt)) = 1
				and datediff(d,convert(date,a.BILL_Date_dt),convert(date,GETDATE())) > 2 
				order by AlertDtl_RecType_var";
                    }
                    else
                    {
                        str = @"Select 0  as AlertDtl_EnqNo_int,null  as AlertDtl_EnqDate_date,a.BILL_ModifiedOn_dt as AlertDtl_BillModifiedDate_date,a.BILL_Id as AlertDtl_BillNo_int,a.BILL_Date_dt as AlertDtl_BillDate_date,a.BILL_NetAmt_num as AlertDtl_BillAmt_dec,
				'' as AlertDtl_ProNo_var,0 as AlertDtl_ProNo_int,null as AlertDtl_ProDate_date,'' as AlertDtl_RefNo_var,0 as AlertDtl_RecNo_int,
				c.CL_Name_var as AlertDtl_CL_Name_var,d.SITE_Name_var as AlertDtl_SITE_Name_var,(select MATERIAL_Name_var from tbl_Material e where e.MATERIAL_RecordType_var=a.BILL_RecordType_var)  as AlertDtl_RecType_var,
				c.CL_Id,d.SITE_Id,36 as AlertDtl_Alert_Id
				,(select USER_NAME_var from tbl_User x where x.USER_Id IN (select ROUTE_ME_Id from tbl_Route where ROUTE_Id=d.SITE_Route_Id)) as AlertDtl_MEName_var
				,null as AlertDtl_ProOrderDate_date,null as AlertDtl_EnteredDate_date
                ,null as AlertDtl_CollctnEnqDate_date,null as AlertDtl_InwdRecDate_date,null as AlertDtl_CheckedDate_date,null as AlertDtl_OutwrdDate_date,null as AlertDtl_AppDate_date,'' as AlertDtl_TestName_var
                from tbl_Client as c,tbl_Site as d,tbl_Bill as a
				where  a.BILL_CL_Id=c.CL_Id 
				and a.Bill_SITE_Id=d.SITE_Id
				and CONVERT (DATE, A.BILL_Date_dt) >=CONVERT(DATE,'" + fromDate + "') and CONVERT (DATE, A.BILL_Date_dt) <=CONVERT(DATE,'" + ToDate + "') " +
                @"and a.BILL_ModifiedOn_dt is null 
                and datediff(d,convert(date,a.BILL_Date_dt),convert(date,GETDATE())) > 2 
				order by AlertDtl_RecType_var";
                    }
                }
                else if (Alert_Id == 37)
                {
                    if (flagStatus == 0)
                    {
                        str = @"--No of bills modified 2 days after generation
                Select 0 as AlertDtl_EnqNo_int,null  as AlertDtl_EnqDate_date,a.BILL_ModifiedOn_dt as AlertDtl_BillModifiedDate_date,a.BILL_Id as AlertDtl_BillNo_int,a.BILL_Date_dt as AlertDtl_BillDate_date,a.BILL_NetAmt_num as AlertDtl_BillAmt_dec,
				'' as AlertDtl_ProNo_var,0 as AlertDtl_ProNo_int,null as AlertDtl_ProDate_date,'' as AlertDtl_RefNo_var,0 as AlertDtl_RecNo_int,
				c.CL_Name_var as AlertDtl_CL_Name_var,d.SITE_Name_var as AlertDtl_SITE_Name_var,(select MATERIAL_Name_var from tbl_Material e where e.MATERIAL_RecordType_var=a.BILL_RecordType_var)  as AlertDtl_RecType_var,
				c.CL_Id,d.SITE_Id,37 as AlertDtl_Alert_Id
				,(select USER_NAME_var from tbl_User x where x.USER_Id IN (select ROUTE_ME_Id from tbl_Route where ROUTE_Id=d.SITE_Route_Id)) as AlertDtl_MEName_var
				,null as AlertDtl_ProOrderDate_date,null as AlertDtl_EnteredDate_date
                ,null as AlertDtl_CollctnEnqDate_date,null as AlertDtl_InwdRecDate_date,null as AlertDtl_CheckedDate_date,null as AlertDtl_OutwrdDate_date,null as AlertDtl_AppDate_date,'' as AlertDtl_TestName_var
                from tbl_Client as c,tbl_Site as d,tbl_Bill as a
				where  a.BILL_CL_Id=c.CL_Id 
				and a.Bill_SITE_Id=d.SITE_Id
				and CONVERT (DATE, A.BILL_Date_dt) >=CONVERT(DATE,'" + fromDate + "') and CONVERT (DATE, A.BILL_Date_dt) <=CONVERT(DATE,'" + ToDate + "') " +
                @"and datediff(d,convert(date,a.BILL_Date_dt),convert(date,a.BILL_ModifiedOn_dt)) <= 2
				and datediff(d,convert(date,a.BILL_Date_dt),convert(date,GETDATE())) > 2 
				order by AlertDtl_RecType_var";
                    }
                    else
                    {
                        str = @"Select 0  as AlertDtl_EnqNo_int,null  as AlertDtl_EnqDate_date,a.BILL_ModifiedOn_dt as AlertDtl_BillModifiedDate_date,a.BILL_Id as AlertDtl_BillNo_int,a.BILL_Date_dt as AlertDtl_BillDate_date,a.BILL_NetAmt_num as AlertDtl_BillAmt_dec,
				'' as AlertDtl_ProNo_var,0 as AlertDtl_ProNo_int,null as AlertDtl_ProDate_date,'' as AlertDtl_RefNo_var,0 as AlertDtl_RecNo_int,
				c.CL_Name_var as AlertDtl_CL_Name_var,d.SITE_Name_var as AlertDtl_SITE_Name_var,(select MATERIAL_Name_var from tbl_Material e where e.MATERIAL_RecordType_var=a.BILL_RecordType_var)  as AlertDtl_RecType_var,
				c.CL_Id,d.SITE_Id,37 as AlertDtl_Alert_Id
				,(select USER_NAME_var from tbl_User x where x.USER_Id IN (select ROUTE_ME_Id from tbl_Route where ROUTE_Id=d.SITE_Route_Id)) as AlertDtl_MEName_var
				,null as AlertDtl_ProOrderDate_date,null as AlertDtl_EnteredDate_date
                ,null as AlertDtl_CollctnEnqDate_date,null as AlertDtl_InwdRecDate_date,null as AlertDtl_CheckedDate_date,null as AlertDtl_OutwrdDate_date,null as AlertDtl_AppDate_date,'' as AlertDtl_TestName_var
                from tbl_Client as c,tbl_Site as d,tbl_Bill as a
				where  a.BILL_CL_Id=c.CL_Id 
				and a.Bill_SITE_Id=d.SITE_Id
				and CONVERT (DATE, A.BILL_Date_dt) >=CONVERT(DATE,'" + fromDate + "') and CONVERT (DATE, A.BILL_Date_dt) <=CONVERT(DATE,'" + ToDate + "') " +
                @"and a.BILL_ModifiedOn_dt is null 
                and datediff(d,convert(date,a.BILL_Date_dt),convert(date,GETDATE())) > 2 
				order by AlertDtl_RecType_var";
                    }
                }
                else if (Alert_Id == 38)
                {
                    if (flagStatus == 0)
                    {
                        str = @"--No of bills not received by client in 6 days after generation
                Select 0 as AlertDtl_EnqNo_int,null  as AlertDtl_EnqDate_date,a.BILL_Id as AlertDtl_BillNo_int,a.BILL_Date_dt as AlertDtl_BillDate_date,a.BILL_NetAmt_num as AlertDtl_BillAmt_dec,
				'' as AlertDtl_ProNo_var,0 as AlertDtl_ProNo_int,null as AlertDtl_ProDate_date,'' as AlertDtl_RefNo_var,0 as AlertDtl_RecNo_int,
				c.CL_Name_var as AlertDtl_CL_Name_var,d.SITE_Name_var as AlertDtl_SITE_Name_var,(select MATERIAL_Name_var from tbl_Material e where e.MATERIAL_RecordType_var=a.BILL_RecordType_var)  as AlertDtl_RecType_var,
				c.CL_Id,d.SITE_Id,38 as AlertDtl_Alert_Id
				,(select USER_NAME_var from tbl_User x where x.USER_Id IN (select ROUTE_ME_Id from tbl_Route where ROUTE_Id=d.SITE_Route_Id)) as AlertDtl_MEName_var
				  ,null as AlertDtl_ProOrderDate_date,null as AlertDtl_BillModifiedDate_date,null as AlertDtl_EnteredDate_date
                ,null as AlertDtl_CollctnEnqDate_date,null as AlertDtl_InwdRecDate_date,null as AlertDtl_CheckedDate_date,null as AlertDtl_OutwrdDate_date,null as AlertDtl_AppDate_date,'' as AlertDtl_TestName_var
                from tbl_Client as c,tbl_Site as d,tbl_Bill as a,tbl_Outward b
				where  a.BILL_CL_Id=c.CL_Id 
                and a.BILL_Id = b.OUTW_ReferenceNo_var And b.OUTW_RecordType_var ='DT'
				and a.Bill_SITE_Id=d.SITE_Id
				and CONVERT (DATE, A.BILL_Date_dt) >=CONVERT(DATE,'" + fromDate + "') and CONVERT (DATE, A.BILL_Date_dt) <=CONVERT(DATE,'" + ToDate + "') " +
                @"and datediff(d,convert(date,a.BILL_Date_dt),convert(date,b.OUTW_OutwardDate_dt)) > 6
				and datediff(d,convert(date,a.BILL_Date_dt),convert(date,GETDATE())) > 6
				order by AlertDtl_RecType_var";
                    }
                    else
                    {
                        str = @"Select 0 as AlertDtl_EnqNo_int,null  as AlertDtl_EnqDate_date,a.BILL_Id as AlertDtl_BillNo_int,a.BILL_Date_dt as AlertDtl_BillDate_date,a.BILL_NetAmt_num as AlertDtl_BillAmt_dec,
				'' as AlertDtl_ProNo_var,0 as AlertDtl_ProNo_int,null as AlertDtl_ProDate_date,'' as AlertDtl_RefNo_var,0 as AlertDtl_RecNo_int,
				c.CL_Name_var as AlertDtl_CL_Name_var,d.SITE_Name_var as AlertDtl_SITE_Name_var,(select MATERIAL_Name_var from tbl_Material e where e.MATERIAL_RecordType_var=a.BILL_RecordType_var)  as AlertDtl_RecType_var,
				c.CL_Id,d.SITE_Id,38 as AlertDtl_Alert_Id
				,(select USER_NAME_var from tbl_User x where x.USER_Id IN (select ROUTE_ME_Id from tbl_Route where ROUTE_Id=d.SITE_Route_Id)) as AlertDtl_MEName_var
				  ,null as AlertDtl_ProOrderDate_date,null as AlertDtl_BillModifiedDate_date,null as AlertDtl_EnteredDate_date
                ,null as AlertDtl_CollctnEnqDate_date,null as AlertDtl_InwdRecDate_date,null as AlertDtl_CheckedDate_date,null as AlertDtl_OutwrdDate_date,null as AlertDtl_AppDate_date,'' as AlertDtl_TestName_var
                from tbl_Client as c,tbl_Site as d,tbl_Bill as a,tbl_Outward b
				where  a.BILL_CL_Id=c.CL_Id 
                and a.BILL_Id = b.OUTW_ReferenceNo_var And b.OUTW_RecordType_var ='DT'
				and a.Bill_SITE_Id=d.SITE_Id
				and CONVERT (DATE, A.BILL_Date_dt) >=CONVERT(DATE,'" + fromDate + "') and CONVERT (DATE, A.BILL_Date_dt) <=CONVERT(DATE,'" + ToDate + "') " +
                @"and b.OUTW_OutwardDate_dt is null 
                and datediff(d,convert(date,a.BILL_Date_dt),convert(date,GETDATE())) > 6 
				order by AlertDtl_RecType_var";
                    }
                }
                else if (Alert_Id == 39)
                {
                    if (flagStatus == 0)
                    {
                        str = @"---- Payment not received in 60 days after bill received by the client
                                Select 0 as AlertDtl_EnqNo_int,null as AlertDtl_EnqDate_date,a.BILL_Id as AlertDtl_BillNo_int,a.BILL_Date_dt as AlertDtl_BillDate_date,a.BILL_NetAmt_num as AlertDtl_BillAmt_dec,
                                '' as AlertDtl_ProNo_var,0 as AlertDtl_ProNo_int,null as AlertDtl_ProDate_date,'' as AlertDtl_RefNo_var,0 as AlertDtl_RecNo_int,
                                c.CL_Name_var as AlertDtl_CL_Name_var,d.SITE_Name_var as AlertDtl_SITE_Name_var,
                                (select MATERIAL_Name_var from tbl_Material e where e.MATERIAL_RecordType_var=a.BILL_RecordType_var)  as AlertDtl_RecType_var,
                                c.CL_Id as AlertDtl_CLId_int,d.SITE_Id as AlertDtl_SiteId_int,39 as AlertDtl_Alert_Id,
                                (select USER_NAME_var from tbl_User x where x.USER_Id IN (select ROUTE_ME_Id from tbl_Route where ROUTE_Id=d.SITE_Route_Id)) as AlertDtl_MEName_var
                                ,null as AlertDtl_ProOrderDate_date,null as AlertDtl_BillModifiedDate_date,null as AlertDtl_EnteredDate_date
                                ,null as AlertDtl_CollctnEnqDate_date,null as AlertDtl_InwdRecDate_date,null as AlertDtl_CheckedDate_date,b.OUTW_OutwardDate_dt as AlertDtl_OutwrdDate_date,null as AlertDtl_AppDate_date,'' as AlertDtl_TestName_var
                                from tbl_Bill as a,tbl_Client as c,tbl_Site as d,tbl_Outward b,tbl_cashDetail e
				                where  a.BILL_CL_Id=c.CL_Id 
				                and a.BILL_Id = b.OUTW_ReferenceNo_var  and b.OUTW_RecordType_var ='DT'
				                and a.Bill_SITE_Id=d.SITE_Id
				                and a.BILL_OutwardStatus_bit =1
				                and a.BILL_Id=e.CashDetail_BillNo_int
                                and CONVERT (DATE, b.OUTW_OutwardDate_dt) >=CONVERT (DATE,'" + fromDate + "') and CONVERT (DATE,b.OUTW_OutwardDate_dt) <=CONVERT (DATE,'" + ToDate + "') " +
                                @" and datediff(d,convert(date,b.OUTW_OutwardDate_dt),convert(date,GETDATE())) > 61
				                and datediff(d,convert(date,b.OUTW_OutwardDate_dt),convert(date,e.CashDetail_Date_date)) > 61         
                                and a.Bill_Status_bit=0
                                and (a.BILL_NetAmt_num + (select Sum(b.CashDetail_Amount_money) from tbl_cashDetail b
                                where b.CashDetail_BillNo_int=a.BILL_Id and b.CashDetail_Status_bit=0))>0
                                order by AlertDtl_RecType_var";
                    }
                    else
                    {
                        str = @"---- Payment not received in 60 days after bill received by the client
                                Select 0 as AlertDtl_EnqNo_int,null as AlertDtl_EnqDate_date,a.BILL_Id as AlertDtl_BillNo_int,a.BILL_Date_dt as AlertDtl_BillDate_date,a.BILL_NetAmt_num as AlertDtl_BillAmt_dec,
                                '' as AlertDtl_ProNo_var,0 as AlertDtl_ProNo_int,null as AlertDtl_ProDate_date,'' as AlertDtl_RefNo_var,0 as AlertDtl_RecNo_int,
                                c.CL_Name_var as AlertDtl_CL_Name_var,d.SITE_Name_var as AlertDtl_SITE_Name_var,(select MATERIAL_Name_var from tbl_Material e where e.MATERIAL_RecordType_var=a.BILL_RecordType_var)  as AlertDtl_RecType_var,
                                c.CL_Id as AlertDtl_CLId_int,d.SITE_Id as AlertDtl_SiteId_int,39 as AlertDtl_Alert_Id,
                                (select USER_NAME_var from tbl_User x where x.USER_Id IN (select ROUTE_ME_Id from tbl_Route where ROUTE_Id=d.SITE_Route_Id)) as AlertDtl_MEName_var
                                  ,null as AlertDtl_ProOrderDate_date,null as AlertDtl_BillModifiedDate_date,null as AlertDtl_EnteredDate_date
                                ,null as AlertDtl_CollctnEnqDate_date,null as AlertDtl_InwdRecDate_date,null as AlertDtl_CheckedDate_date,b.OUTW_OutwardDate_dt as AlertDtl_OutwrdDate_date,null as AlertDtl_AppDate_date,'' as AlertDtl_TestName_var
                                from tbl_Bill as a,tbl_Client as c,tbl_Site as d,tbl_Outward b
				                where  a.BILL_CL_Id=c.CL_Id 
				                and a.BILL_Id = b.OUTW_ReferenceNo_var  and b.OUTW_RecordType_var ='DT'
				                and a.Bill_SITE_Id=d.SITE_Id
				                and a.BILL_OutwardStatus_bit =1
				                and CONVERT (DATE,b.OUTW_OutwardDate_dt) >=CONVERT (DATE,'" + fromDate + "') and CONVERT (DATE,b.OUTW_OutwardDate_dt) <=CONVERT (DATE,'" + ToDate + "') " +
                                @" and a.Bill_id not in  (select CashDetail_BillNo_int from tbl_CashDetail where CashDetail_Status_bit=0)
				                and datediff(d,convert(date,b.OUTW_OutwardDate_dt),convert(date,GETDATE())) > 61 
                                and a.Bill_Status_bit=0
                                order by AlertDtl_RecType_var";
                    }
                }
                else if (Alert_Id == 40)
                {
                    if (flagStatus == 0)
                    {
                        str = @"---- Payment not received in 90 days after bill received by the client
                                 Select 0 as AlertDtl_EnqNo_int,null as AlertDtl_EnqDate_date,a.BILL_Id as AlertDtl_BillNo_int,a.BILL_Date_dt as AlertDtl_BillDate_date,a.BILL_NetAmt_num as AlertDtl_BillAmt_dec,
                                '' as AlertDtl_ProNo_var,0 as AlertDtl_ProNo_int,null as AlertDtl_ProDate_date,'' as AlertDtl_RefNo_var,0 as AlertDtl_RecNo_int,
                                c.CL_Name_var as AlertDtl_CL_Name_var,d.SITE_Name_var as AlertDtl_SITE_Name_var,
                                (select MATERIAL_Name_var from tbl_Material e where e.MATERIAL_RecordType_var=a.BILL_RecordType_var)  as AlertDtl_RecType_var,
                                c.CL_Id as AlertDtl_CLId_int,d.SITE_Id as AlertDtl_SiteId_int,40 as AlertDtl_Alert_Id,
                                (select USER_NAME_var from tbl_User x where x.USER_Id IN (select ROUTE_ME_Id from tbl_Route where ROUTE_Id=d.SITE_Route_Id)) as AlertDtl_MEName_var
                                  ,null as AlertDtl_ProOrderDate_date,null as AlertDtl_BillModifiedDate_date,null as AlertDtl_EnteredDate_date
                                ,null as AlertDtl_CollctnEnqDate_date,null as AlertDtl_InwdRecDate_date,null as AlertDtl_CheckedDate_date,b.OUTW_OutwardDate_dt as AlertDtl_OutwrdDate_date,null as AlertDtl_AppDate_date,'' as AlertDtl_TestName_var
                                from tbl_Bill as a,tbl_Client as c,tbl_Site as d,tbl_Outward b,tbl_cashDetail e
				                where  a.BILL_CL_Id=c.CL_Id 
				                and a.BILL_Id = b.OUTW_ReferenceNo_var  and b.OUTW_RecordType_var ='DT'
				                and a.Bill_SITE_Id=d.SITE_Id
				                and a.BILL_OutwardStatus_bit =1
				                and a.BILL_Id=e.CashDetail_BillNo_int
                                and CONVERT (DATE, b.OUTW_OutwardDate_dt) >=CONVERT (DATE,'" + fromDate + "') and CONVERT (DATE,b.OUTW_OutwardDate_dt) <=CONVERT (DATE,'" + ToDate + "') " +
                                @" and datediff(d,convert(date,b.OUTW_OutwardDate_dt),convert(date,GETDATE())) > 91
				                and datediff(d,convert(date,b.OUTW_OutwardDate_dt),convert(date,e.CashDetail_Date_date)) > 91         
                                and a.Bill_Status_bit=0
                                and (a.BILL_NetAmt_num + (select Sum(b.CashDetail_Amount_money) from tbl_cashDetail b
                                where b.CashDetail_BillNo_int=a.BILL_Id and b.CashDetail_Status_bit=0))>0
                                order by AlertDtl_RecType_var";
                    }
                    else
                    {
                        str = @"---- Payment not received in 90 days after bill received by the client
                                 Select 0 as AlertDtl_EnqNo_int,null as AlertDtl_EnqDate_date,a.BILL_Id as AlertDtl_BillNo_int,a.BILL_Date_dt as AlertDtl_BillDate_date,a.BILL_NetAmt_num as AlertDtl_BillAmt_dec,
                                '' as AlertDtl_ProNo_var,0 as AlertDtl_ProNo_int,null as AlertDtl_ProDate_date,'' as AlertDtl_RefNo_var,0 as AlertDtl_RecNo_int,
                                c.CL_Name_var as AlertDtl_CL_Name_var,d.SITE_Name_var as AlertDtl_SITE_Name_var,(select MATERIAL_Name_var from tbl_Material e where e.MATERIAL_RecordType_var=a.BILL_RecordType_var)  as AlertDtl_RecType_var,
                                c.CL_Id as AlertDtl_CLId_int,d.SITE_Id as AlertDtl_SiteId_int,40 as AlertDtl_Alert_Id,
                                (select USER_NAME_var from tbl_User x where x.USER_Id IN (select ROUTE_ME_Id from tbl_Route where ROUTE_Id=d.SITE_Route_Id)) as AlertDtl_MEName_var
                                  ,null as AlertDtl_ProOrderDate_date,null as AlertDtl_BillModifiedDate_date,null as AlertDtl_EnteredDate_date
                                ,null as AlertDtl_CollctnEnqDate_date,null as AlertDtl_InwdRecDate_date,null as AlertDtl_CheckedDate_date,b.OUTW_OutwardDate_dt as AlertDtl_OutwrdDate_date,null as AlertDtl_AppDate_date,'' as AlertDtl_TestName_var
                                from tbl_Bill as a,tbl_Client as c,tbl_Site as d,tbl_Outward b
				                where  a.BILL_CL_Id=c.CL_Id 
				                and a.BILL_Id = b.OUTW_ReferenceNo_var  and b.OUTW_RecordType_var ='DT'
				                and a.Bill_SITE_Id=d.SITE_Id
				                and a.BILL_OutwardStatus_bit =1
				                and CONVERT (DATE,b.OUTW_OutwardDate_dt) >=CONVERT (DATE,'" + fromDate + "') and CONVERT (DATE,b.OUTW_OutwardDate_dt) <=CONVERT (DATE,'" + ToDate + "') " +
                                @" and a.Bill_id not in  (select CashDetail_BillNo_int from tbl_CashDetail where CashDetail_Status_bit=0)
				                and datediff(d,convert(date,b.OUTW_OutwardDate_dt),convert(date,GETDATE())) > 91 
                                and a.Bill_Status_bit=0
                                order by AlertDtl_RecType_var";
                    }
                }
                else if (Alert_Id == 41)
                {
                    if (flagStatus == 0)
                    {
                        str = @"---- Payment not received in 120 days after bill received by the client
                                 Select 0 as AlertDtl_EnqNo_int,null as AlertDtl_EnqDate_date,a.BILL_Id as AlertDtl_BillNo_int,a.BILL_Date_dt as AlertDtl_BillDate_date,a.BILL_NetAmt_num as AlertDtl_BillAmt_dec,
                                '' as AlertDtl_ProNo_var,0 as AlertDtl_ProNo_int,null as AlertDtl_ProDate_date,'' as AlertDtl_RefNo_var,0 as AlertDtl_RecNo_int,
                                c.CL_Name_var as AlertDtl_CL_Name_var,d.SITE_Name_var as AlertDtl_SITE_Name_var,
                                (select MATERIAL_Name_var from tbl_Material e where e.MATERIAL_RecordType_var=a.BILL_RecordType_var)  as AlertDtl_RecType_var,
                                c.CL_Id as AlertDtl_CLId_int,d.SITE_Id as AlertDtl_SiteId_int,41 as AlertDtl_Alert_Id,
                                (select USER_NAME_var from tbl_User x where x.USER_Id IN (select ROUTE_ME_Id from tbl_Route where ROUTE_Id=d.SITE_Route_Id)) as AlertDtl_MEName_var
                                  ,null as AlertDtl_ProOrderDate_date,null as AlertDtl_BillModifiedDate_date,null as AlertDtl_EnteredDate_date
                                ,null as AlertDtl_CollctnEnqDate_date,null as AlertDtl_InwdRecDate_date,null as AlertDtl_CheckedDate_date,b.OUTW_OutwardDate_dt as AlertDtl_OutwrdDate_date,null as AlertDtl_AppDate_date,'' as AlertDtl_TestName_var
                                from tbl_Bill as a,tbl_Client as c,tbl_Site as d,tbl_Outward b,tbl_cashDetail e
				                where  a.BILL_CL_Id=c.CL_Id 
				                and a.BILL_Id = b.OUTW_ReferenceNo_var  and b.OUTW_RecordType_var ='DT'
				                and a.Bill_SITE_Id=d.SITE_Id
				                and a.BILL_OutwardStatus_bit =1
				                and a.BILL_Id=e.CashDetail_BillNo_int
                                and CONVERT (DATE, b.OUTW_OutwardDate_dt) >=CONVERT (DATE,'" + fromDate + "') and CONVERT (DATE,b.OUTW_OutwardDate_dt) <=CONVERT (DATE,'" + ToDate + "') " +
                                @" and datediff(d,convert(date,b.OUTW_OutwardDate_dt),convert(date,GETDATE())) > 121
				                and datediff(d,convert(date,b.OUTW_OutwardDate_dt),convert(date,e.CashDetail_Date_date)) > 121         
                                and a.Bill_Status_bit=0
                                and (a.BILL_NetAmt_num + (select Sum(b.CashDetail_Amount_money) from tbl_cashDetail b
                                where b.CashDetail_BillNo_int=a.BILL_Id and b.CashDetail_Status_bit=0))>0
                                order by AlertDtl_RecType_var";
                    }
                    else
                    {
                        str = @"---- Payment not received in 120 days after bill received by the client
                                 Select 0 as AlertDtl_EnqNo_int,null as AlertDtl_EnqDate_date,a.BILL_Id as AlertDtl_BillNo_int,a.BILL_Date_dt as AlertDtl_BillDate_date,a.BILL_NetAmt_num as AlertDtl_BillAmt_dec,
                                '' as AlertDtl_ProNo_var,0 as AlertDtl_ProNo_int,null as AlertDtl_ProDate_date,'' as AlertDtl_RefNo_var,0 as AlertDtl_RecNo_int,
                                c.CL_Name_var as AlertDtl_CL_Name_var,d.SITE_Name_var as AlertDtl_SITE_Name_var,(select MATERIAL_Name_var from tbl_Material e where e.MATERIAL_RecordType_var=a.BILL_RecordType_var)  as AlertDtl_RecType_var,
                                c.CL_Id as AlertDtl_CLId_int,d.SITE_Id as AlertDtl_SiteId_int,41 as AlertDtl_Alert_Id,
                                (select USER_NAME_var from tbl_User x where x.USER_Id IN (select ROUTE_ME_Id from tbl_Route where ROUTE_Id=d.SITE_Route_Id)) as AlertDtl_MEName_var
                                ,null as AlertDtl_ProOrderDate_date,null as AlertDtl_BillModifiedDate_date,null as AlertDtl_EnteredDate_date
                                ,null as AlertDtl_CollctnEnqDate_date,null as AlertDtl_InwdRecDate_date,null as AlertDtl_CheckedDate_date,b.OUTW_OutwardDate_dt as AlertDtl_OutwrdDate_date,null as AlertDtl_AppDate_date,'' as AlertDtl_TestName_var
                                from tbl_Bill as a,tbl_Client as c,tbl_Site as d,tbl_Outward b
				                where  a.BILL_CL_Id=c.CL_Id 
				                and a.BILL_Id = b.OUTW_ReferenceNo_var  and b.OUTW_RecordType_var ='DT'
				                and a.Bill_SITE_Id=d.SITE_Id
				                and a.BILL_OutwardStatus_bit =1
				                and CONVERT (DATE,b.OUTW_OutwardDate_dt) >=CONVERT (DATE,'" + fromDate + "') and CONVERT (DATE,b.OUTW_OutwardDate_dt) <=CONVERT (DATE,'" + ToDate + "') " +
                                @" and a.Bill_id not in  (select CashDetail_BillNo_int from tbl_CashDetail where CashDetail_Status_bit=0)
				                and datediff(d,convert(date,b.OUTW_OutwardDate_dt),convert(date,GETDATE())) > 121 
                                and a.Bill_Status_bit=0
                                order by AlertDtl_RecType_var";
                    }
                }
                SqlDataAdapter da = new SqlDataAdapter(str, con);//and CL_name_var like '" + str + "%'
                da.Fill(ds);
                dt = ds.Tables[0];
                ds.Dispose();
                return dt;
            }
            catch (Exception e)
            {
                throw e;
            }

        }

        //        public DataTable AlertDetailsViewForTrigger(int Alert_Id, string fromDate, string ToDate, int flagStatus)
        //        {
        //            SqlConnection con = new SqlConnection(cnStr);
        //            try
        //            {
        //                DataSet ds = new DataSet();
        //                DataTable dt = new DataTable();
        //                string str = "";
        //                if (Alert_Id == 1)
        //                {
        //                    if (flagStatus == 0)//all
        //                    {
        //                        str = @"----Proposal not sent for more than  1   days after receiving enquiry
        //				                Select a.ENQ_Id as AlertDtl_EnqNo_int,a.ENQ_Date_dt as AlertDtl_EnqDate_date,null as AlertDtl_CollctnEnqDate_date,'' as AlertDtl_BillNo_int,null as AlertDtl_BillDate_date,0 as AlertDtl_BillAmt_dec,
        //                                '' as AlertDtl_ProNo_var,0 as AlertDtl_ProNo_int,null as AlertDtl_ProDate_date,'' as AlertDtl_RefNo_var,0 as AlertDtl_RecNo_int,
        //                                c.CL_Name_var as AlertDtl_CL_Name_var,d.SITE_Name_var as AlertDtl_SITE_Name_var,e.MATERIAL_Name_var as AlertDtl_RecType_var,c.CL_Id as AlertDtl_CLId_int,d.SITE_Id as AlertDtl_SiteId_int,1 as AlertDtl_Alert_Id
        //                                ,(select USER_NAME_var from tbl_User x where x.USER_Id IN (select ROUTE_ME_Id from tbl_Route where ROUTE_Id=d.SITE_Route_Id)) as AlertDtl_MEName_var
        //                                ,null as AlertDtl_ProOrderDate_date,null as AlertDtl_BillModifiedDate_date,null as AlertDtl_InwdRecDate_date
        //                                ,null as AlertDtl_AppDate_date,null as AlertDtl_EnteredDate_date,null as AlertDtl_CheckedDate_date,null as AlertDtl_OutwrdDate_date,'' as AlertDtl_TestName_var
        //                                from tbl_Enquiry as a,tbl_Proposal as b,tbl_Client as c,tbl_Site as d,tbl_Material as e
        //                                where a.ENQ_Id=b.Proposal_EnqNo and
        //                                 a.ENQ_CL_Id=c.CL_Id
        //                                and a.ENQ_SITE_Id=d.SITE_Id
        //                                and a.ENQ_MATERIAL_Id=e.MATERIAL_Id
        //                                and CONVERT (DATE, a.ENQ_Date_dt) >=CONVERT (DATE,'" + fromDate + "') and CONVERT (DATE, a.ENQ_Date_dt) <=CONVERT (DATE,'" + ToDate + "') " +
        //                                @"and datediff(d,convert(date,a.ENQ_Date_dt),convert(date,GETDATE())) > 1 
        //                               and datediff(d,convert(date,a.ENQ_Date_dt),convert(date,b.Proposal_Date)) > 1 
        //                               --and ENQ_Id not in (select Proposal_EnqNo from tbl_Proposal where Proposal_NewClientStatus=0)
        //				                
        //                                						
        //                                Union
        //                                						
        //                                Select a.ENQNEW_Id as AlertDtl_EnqNo_int,a.ENQNEW_Date_dt as AlertDtl_EnqDate_date,null as AlertDtl_CollctnEnqDate_date,'' as AlertDtl_BillNo_int,null as AlertDtl_BillDate_date,0 as AlertDtl_BillAmt_dec,
        //                                '' as AlertDtl_ProNo_var,0 as AlertDtl_ProNo_int,null as AlertDtl_ProDate_date,'' as AlertDtl_RefNo_var,0 as AlertDtl_RecNo_int,
        //                                ENQNEW_ClientName_var as AlertDtl_CL_Name_var,ENQNEW_SiteName_var as AlertDtl_SITE_Name_var,e.MATERIAL_Name_var as AlertDtl_RecType_var,0 as CL_Id,0 as SITE_Id,1 as AlertDtl_Alert_Id
        //                                ,(select USER_NAME_var from tbl_User x where x.USER_Id IN (select ROUTE_ME_Id from tbl_Route where ROUTE_Id=a.ENQNEW_ROUTE_Id)) as AlertDtl_MEName_var
        //                                   ,null as AlertDtl_ProOrderDate_date,null as AlertDtl_BillModifiedDate_date,null as AlertDtl_InwdRecDate_date
        //                                ,null as AlertDtl_AppDate_date,null as AlertDtl_EnteredDate_date,null as AlertDtl_CheckedDate_date,null as AlertDtl_OutwrdDate_date,'' as AlertDtl_TestName_var
        //                              from tbl_EnquiryNewClient as a,tbl_Proposal as b,tbl_Material as e
        //                                where a.ENQNEW_Id=b.Proposal_EnqNo and
        //                                 a.ENQNEW_MATERIAL_Id=e.MATERIAL_Id
        //                                and CONVERT (DATE, a.ENQNEW_Date_dt) >=CONVERT (DATE,'" + fromDate + "') and CONVERT (DATE, a.ENQNEW_Date_dt) <=CONVERT (DATE,'" + ToDate + "') " +
        //                                @"and datediff(d,convert(date,a.ENQNEW_Date_dt),convert(date,GETDATE())) > 1 
        //                               and datediff(d,convert(date,a.ENQNEW_Date_dt),convert(date,b.Proposal_Date)) > 1 
        //                               --and ENQNEW_Id not in (select Proposal_EnqNo from tbl_Proposal where Proposal_NewClientStatus=1)
        //				                order by AlertDtl_RecType_var";
        //                    }
        //                    else//pending
        //                    {
        //                        str = @"----Proposal not sent for more than  1   days after receiving enquiry
        //				                Select a.ENQ_Id as AlertDtl_EnqNo_int,a.ENQ_Date_dt as AlertDtl_EnqDate_date,null as AlertDtl_CollctnEnqDate_date,'' as AlertDtl_BillNo_int,null as AlertDtl_BillDate_date,0 as AlertDtl_BillAmt_dec,
        //                                '' as AlertDtl_ProNo_var,0 as AlertDtl_ProNo_int,null as AlertDtl_ProDate_date,'' as AlertDtl_RefNo_var,0 as AlertDtl_RecNo_int,
        //                                c.CL_Name_var as AlertDtl_CL_Name_var,d.SITE_Name_var as AlertDtl_SITE_Name_var,e.MATERIAL_Name_var as AlertDtl_RecType_var,c.CL_Id as AlertDtl_CLId_int,d.SITE_Id as AlertDtl_SiteId_int,1 as AlertDtl_Alert_Id
        //                                ,(select USER_NAME_var from tbl_User x where x.USER_Id IN (select ROUTE_ME_Id from tbl_Route where ROUTE_Id=d.SITE_Route_Id)) as AlertDtl_MEName_var
        //                                   ,null as AlertDtl_ProOrderDate_date,null as AlertDtl_BillModifiedDate_date,null as AlertDtl_InwdRecDate_date
        //                                ,null as AlertDtl_AppDate_date,null as AlertDtl_EnteredDate_date,null as AlertDtl_CheckedDate_date,null as AlertDtl_OutwrdDate_date,'' as AlertDtl_TestName_var
        //                             from tbl_Enquiry as a,tbl_Proposal as b,tbl_Client as c,tbl_Site as d,tbl_Material as e
        //                                where a.ENQ_Id=b.Proposal_EnqNo and
        //                                 a.ENQ_CL_Id=c.CL_Id
        //                                and a.ENQ_SITE_Id=d.SITE_Id
        //                                and a.ENQ_MATERIAL_Id=e.MATERIAL_Id
        //                                and CONVERT (DATE, a.ENQ_Date_dt) >=CONVERT (DATE,'" + fromDate + "') and CONVERT (DATE, a.ENQ_Date_dt) <=CONVERT (DATE,'" + fromDate + "') " +
        //                               @"and Proposal_Date is null
        //				                and datediff(d,convert(date,a.ENQ_Date_dt),convert(date,GETDATE())) > 1 
        //				                and (a.ENQ_Status_tint  < 2 or a.ENQ_Status_tint = 5) 
        //			                   
        //                                						
        //                                Union
        //                                						
        //                                Select a.ENQNEW_Id as AlertDtl_EnqNo_int,a.ENQNEW_Date_dt as AlertDtl_EnqDate_date,null as AlertDtl_CollctnEnqDate_date,'' as AlertDtl_BillNo_int,null as AlertDtl_BillDate_date,0 as AlertDtl_BillAmt_dec,
        //                                '' as AlertDtl_ProNo_var,0 as AlertDtl_ProNo_int,null as AlertDtl_ProDate_date,'' as AlertDtl_RefNo_var,0 as AlertDtl_RecNo_int,
        //                                ENQNEW_ClientName_var as AlertDtl_CL_Name_var,ENQNEW_SiteName_var as AlertDtl_SITE_Name_var,e.MATERIAL_Name_var as AlertDtl_RecType_var,c.CL_Id as AlertDtl_CLId_int,d.SITE_Id as AlertDtl_SiteId_int,1 as AlertDtl_Alert_Id
        //                                ,(select USER_NAME_var from tbl_User x where x.USER_Id IN (select ROUTE_ME_Id from tbl_Route where ROUTE_Id=a.ENQNEW_ROUTE_Id)) as AlertDtl_MEName_var
        //                                ,null as AlertDtl_ProOrderDate_date,null as AlertDtl_BillModifiedDate_date,null as AlertDtl_InwdRecDate_date
        //                                ,null as AlertDtl_AppDate_date,null as AlertDtl_EnteredDate_date,null as AlertDtl_CheckedDate_date,null as AlertDtl_OutwrdDate_date,'' as AlertDtl_TestName_var
        //                                from tbl_EnquiryNewClient as a,tbl_Proposal as b,tbl_Client as c,tbl_Site as d,tbl_Material as e
        //                                where  a.ENQNEW_Id=b.Proposal_EnqNo  and
        //                                a.ENQNEW_MATERIAL_Id=e.MATERIAL_Id
        //						        and CONVERT (DATE, a.ENQNEW_Date_dt) >=CONVERT (DATE,'" + fromDate + "') and CONVERT (DATE, a.ENQNEW_Date_dt) <=CONVERT (DATE,'" + fromDate + "') " +
        //                                @"and Proposal_Date is null
        //				                and datediff(d,convert(date,a.ENQNEW_Date_dt),convert(date,GETDATE())) > 1 
        //				                and (a.ENQNEW_Status_tint  < 2 or a.ENQNEW_Status_tint = 5) 
        //			                    order by AlertDtl_RecType_var";
        //                    }
        //                }
        //                else if (Alert_Id == 2)
        //                {
        //                    if (flagStatus == 0)
        //                    {
        //                        str = @"----Order not received in 7 days after sending proposal
        //				                Select a.ENQ_Id as AlertDtl_EnqNo_int,a.ENQ_Date_dt as AlertDtl_EnqDate_date,null as AlertDtl_CollctnEnqDate_date,'' as AlertDtl_BillNo_int,null as AlertDtl_BillDate_date,0 as AlertDtl_BillAmt_dec,
        //                                b.Proposal_No as AlertDtl_ProNo_var,b.Proposal_Id as AlertDtl_ProNo_int,b.Proposal_Date as AlertDtl_ProDate_date,'' as AlertDtl_RefNo_var,0 as AlertDtl_RecNo_int,
        //                                c.CL_Name_var as AlertDtl_CL_Name_var,d.SITE_Name_var as AlertDtl_SITE_Name_var,e.MATERIAL_Name_var as AlertDtl_RecType_var,c.CL_Id as AlertDtl_CLId_int,d.SITE_Id as AlertDtl_SiteId_int,2 as AlertDtl_Alert_Id,
        //                                (select USER_NAME_var from tbl_User x where x.USER_Id IN (select ROUTE_ME_Id from tbl_Route where ROUTE_Id=d.SITE_Route_Id)) as AlertDtl_MEName_var
        //                                ,null as AlertDtl_ProOrderDate_date,null as AlertDtl_BillModifiedDate_date,null as AlertDtl_InwdRecDate_date
        //                                ,null as AlertDtl_AppDate_date,null as AlertDtl_EnteredDate_date,null as AlertDtl_CheckedDate_date,null as AlertDtl_OutwrdDate_date,'' as AlertDtl_TestName_var
        //                                from tbl_Enquiry as a,tbl_Proposal as b,tbl_Client as c,tbl_Site as d,tbl_Material as e
        //                                where a.ENQ_Id=b.Proposal_EnqNo
        //                                and a.ENQ_CL_Id=c.CL_Id
        //                                and a.ENQ_SITE_Id=d.SITE_Id
        //                                and a.ENQ_MATERIAL_Id=e.MATERIAL_Id
        //                                and CONVERT (DATE, b.Proposal_Date) >=CONVERT (DATE,'" + fromDate + "') and CONVERT (DATE, b.Proposal_Date) <=CONVERT (DATE,'" + ToDate + "') " +
        //                                @"and datediff(d,convert(date,b.Proposal_Date),convert(date,b.Proposal_OrderDate_dt)) > 7
        //				                and datediff(d,convert(date,b.Proposal_Date),convert(date,GETDATE())) > 7 
        //				                and b.Proposal_NewClientStatus=0
        //                                and b.Proposal_ActiveStatus_bit=0
        //                        
        //                                Union
        //                                						
        //                                Select a.ENQNEW_Id as AlertDtl_EnqNo_int,a.ENQNEW_Date_dt as AlertDtl_EnqDate_date,null as AlertDtl_CollctnEnqDate_date,'' as AlertDtl_BillNo_int,null as AlertDtl_BillDate_date,0 as AlertDtl_BillAmt_dec,
        //                                b.Proposal_No as AlertDtl_ProNo_var,b.Proposal_Id as AlertDtl_ProNo_int,b.Proposal_Date as AlertDtl_ProDate_date,'' as AlertDtl_RefNo_var,0 as MISRecordNo
        //                                ,ENQNEW_ClientName_var as AlertDtl_CL_Name_var,ENQNEW_SiteName_var as AlertDtl_SITE_Name_var,e.MATERIAL_Name_var as AlertDtl_RecType_var,0 as CL_Id,0 as SITE_Id,2 as AlertDtl_Alert_Id
        //                               ,(select USER_NAME_var from tbl_User x where x.USER_Id IN (select ROUTE_ME_Id from tbl_Route where ROUTE_Id=a.ENQNEW_ROUTE_Id)) as AlertDtl_MEName_var
        //                                ,null as AlertDtl_ProOrderDate_date,null as AlertDtl_BillModifiedDate_date,null as AlertDtl_InwdRecDate_date
        //                                ,null as AlertDtl_AppDate_date,null as AlertDtl_EnteredDate_date,null as AlertDtl_CheckedDate_date,null as AlertDtl_OutwrdDate_date,'' as AlertDtl_TestName_var
        //                                from tbl_EnquiryNewClient as a,tbl_Proposal as b,tbl_Material as e
        //                                where a.ENQNEW_Id=b.Proposal_EnqNo
        //                                and a.ENQNEW_MATERIAL_Id=e.MATERIAL_Id 
        //                                and CONVERT (DATE, b.Proposal_Date) >=CONVERT (DATE,'" + fromDate + "') and CONVERT (DATE, b.Proposal_Date) <=CONVERT (DATE,'" + ToDate + "') " +
        //                                @"and datediff(d,convert(date,b.Proposal_Date),convert(date,b.Proposal_OrderDate_dt)) > 7
        //				                and datediff(d,convert(date,b.Proposal_Date),convert(date,GETDATE())) > 7 
        //				                and b.Proposal_NewClientStatus=1 
        //                                and b.Proposal_ActiveStatus_bit=0
        //                                order by AlertDtl_RecType_var";
        //                    }
        //                    else
        //                    {
        //                        str = @"----Order not received in 7 days after sending proposal
        //				               Select a.ENQ_Id as AlertDtl_EnqNo_int,a.ENQ_Date_dt as AlertDtl_EnqDate_date,null as AlertDtl_CollctnEnqDate_date,'' as AlertDtl_BillNo_int,null as AlertDtl_BillDate_date,0 as AlertDtl_BillAmt_dec,
        //                                 b.Proposal_No as AlertDtl_ProNo_var,b.Proposal_Id as AlertDtl_ProNo_int,b.Proposal_Date as AlertDtl_ProDate_date,'' as AlertDtl_RefNo_var,0 as AlertDtl_RecNo_int,
        //                                c.CL_Name_var as AlertDtl_CL_Name_var,d.SITE_Name_var as AlertDtl_SITE_Name_var,e.MATERIAL_Name_var as AlertDtl_RecType_var,c.CL_Id as AlertDtl_CLId_int,d.SITE_Id as AlertDtl_SiteId_int,2 as AlertDtl_Alert_Id,
        //                                (select USER_NAME_var from tbl_User x where x.USER_Id IN (select ROUTE_ME_Id from tbl_Route where ROUTE_Id=d.SITE_Route_Id)) as AlertDtl_MEName_var
        //                                ,null as AlertDtl_ProOrderDate_date,null as AlertDtl_BillModifiedDate_date,null as AlertDtl_InwdRecDate_date
        //                                ,null as AlertDtl_AppDate_date,null as AlertDtl_EnteredDate_date,null as AlertDtl_CheckedDate_date,null as AlertDtl_OutwrdDate_date,'' as AlertDtl_TestName_var
        //                                from tbl_Enquiry as a,tbl_Proposal as b,tbl_Client as c,tbl_Site as d,tbl_Material as e
        //                                where a.ENQ_Id=b.Proposal_EnqNo
        //                                and a.ENQ_CL_Id=c.CL_Id
        //                                and a.ENQ_SITE_Id=d.SITE_Id
        //                                and a.ENQ_MATERIAL_Id=e.MATERIAL_Id
        //                                and CONVERT (DATE, b.Proposal_Date) >=CONVERT (DATE,'" + fromDate + "') and CONVERT (DATE, b.Proposal_Date) <=CONVERT (DATE,'" + ToDate + "') " +
        //                                @"and b.Proposal_OrderDate_dt is null
        //                                and datediff(d,convert(date,b.Proposal_Date),convert(date,GETDATE())) > 7
        //				                and b.Proposal_NewClientStatus=0
        //                                and b.Proposal_ActiveStatus_bit=0
        //                        
        //                                Union
        //                                						
        //                                Select a.ENQNEW_Id as AlertDtl_EnqNo_int,a.ENQNEW_Date_dt as AlertDtl_EnqDate_date,null as AlertDtl_CollctnEnqDate_date,'' as AlertDtl_BillNo_int,null as AlertDtl_BillDate_date,0 as AlertDtl_BillAmt_dec,
        //                                b.Proposal_No as AlertDtl_ProNo_var,b.Proposal_Id as AlertDtl_ProNo_int,b.Proposal_Date as AlertDtl_ProDate_date,'' as AlertDtl_RefNo_var,0 as MISRecordNo
        //                                ,ENQNEW_ClientName_var as AlertDtl_CL_Name_var,ENQNEW_SiteName_var as AlertDtl_SITE_Name_var,e.MATERIAL_Name_var as AlertDtl_RecType_var,0 as CL_Id,0 as SITE_Id,2 as AlertDtl_Alert_Id
        //                                ,(select USER_NAME_var from tbl_User x where x.USER_Id IN (select ROUTE_ME_Id from tbl_Route where ROUTE_Id=a.ENQNEW_ROUTE_Id)) as AlertDtl_MEName_var
        //                                ,null as AlertDtl_ProOrderDate_date,null as AlertDtl_BillModifiedDate_date,null as AlertDtl_InwdRecDate_date
        //                                ,null as AlertDtl_AppDate_date,null as AlertDtl_EnteredDate_date,null as AlertDtl_CheckedDate_date,null as AlertDtl_OutwrdDate_date,'' as AlertDtl_TestName_var
        //                                from tbl_EnquiryNewClient as a,tbl_Proposal as b,tbl_Material as e--tbl_Client as c,tbl_Site as d,
        //                                where a.ENQNEW_Id=b.Proposal_EnqNo
        //                                and a.ENQNEW_MATERIAL_Id=e.MATERIAL_Id 
        //                                and CONVERT (DATE, b.Proposal_Date) >=CONVERT (DATE,'" + fromDate + "') and CONVERT (DATE, b.Proposal_Date) <=CONVERT (DATE,'" + ToDate + "') " +
        //                                @"and b.Proposal_OrderDate_dt is null
        //                                and datediff(d,convert(date,b.Proposal_Date),convert(date,GETDATE())) > 7
        //				                and b.Proposal_NewClientStatus=1 
        //                                and b.Proposal_ActiveStatus_bit=0
        //                                order by AlertDtl_RecType_var";
        //                    }
        //                }
        //                else if (Alert_Id == 3)
        //                {
        //                    if (flagStatus == 0)
        //                    {
        //                        str = @"----Material not collected in 2 days after receiving order
        //				                Select a.ENQ_Id as AlertDtl_EnqNo_int,a.ENQ_Date_dt as AlertDtl_EnqDate_date,b.Proposal_OrderDate_dt as AlertDtl_ProOrderDate_date,'' as AlertDtl_BillNo_int,null as AlertDtl_BillDate_date,0 as AlertDtl_BillAmt_dec,
        //                                b.Proposal_No as AlertDtl_ProNo_var,b.Proposal_Id as AlertDtl_ProNo_int,b.Proposal_Date as AlertDtl_ProDate_date,'' as AlertDtl_RefNo_var,0 as AlertDtl_RecNo_int,
        //                                c.CL_Name_var as AlertDtl_CL_Name_var,d.SITE_Name_var as AlertDtl_SITE_Name_var,e.MATERIAL_Name_var as AlertDtl_RecType_var,c.CL_Id as AlertDtl_CLId_int,d.SITE_Id as AlertDtl_SiteId_int,3 as AlertDtl_Alert_Id,
        //                                (select USER_NAME_var from tbl_User x where x.USER_Id IN (select ROUTE_ME_Id from tbl_Route where ROUTE_Id=d.SITE_Route_Id)) as AlertDtl_MEName_var
        //                                ,null as AlertDtl_CollctnEnqDate_date,null as AlertDtl_BillModifiedDate_date,null as AlertDtl_InwdRecDate_date
        //                                ,null as AlertDtl_AppDate_date,null as AlertDtl_EnteredDate_date,null as AlertDtl_CheckedDate_date,null as AlertDtl_OutwrdDate_date,'' as AlertDtl_TestName_var
        //                                from tbl_Enquiry as a,tbl_Proposal as b,tbl_Client as c,tbl_Site as d,tbl_Material as e
        //                                where a.ENQ_Id=b.Proposal_EnqNo
        //                                and a.ENQ_CL_Id=c.CL_Id
        //                                and a.ENQ_SITE_Id=d.SITE_Id
        //                                and a.ENQ_MATERIAL_Id=e.MATERIAL_Id
        //                                and CONVERT (DATE, b.Proposal_OrderDate_dt) >=CONVERT (DATE,'" + fromDate + "') and CONVERT (DATE, b.Proposal_OrderDate_dt) <=CONVERT (DATE,'" + ToDate + "') " +
        //                                @"and datediff(d,convert(date,b.Proposal_OrderDate_dt),convert(date,a.ENQ_CollectionDate_dt)) > 2
        //				                and datediff(d,convert(date,b.Proposal_OrderDate_dt),convert(date,GETDATE())) > 2
        //                                and b.Proposal_NewClientStatus=0 and b.Proposal_ActiveStatus_bit=0 
        //                                
        //                                Union
        //                                						
        //                                Select a.ENQNEW_Id as AlertDtl_EnqNo_int,a.ENQNEW_Date_dt as AlertDtl_EnqDate_date,b.Proposal_OrderDate_dt as AlertDtl_ProOrderDate_date,'' as AlertDtl_BillNo_int,null as AlertDtl_BillDate_date,0 as AlertDtl_BillAmt_dec,
        //                                b.Proposal_No as AlertDtl_ProNo_var,b.Proposal_Id as AlertDtl_ProNo_int,b.Proposal_Date as AlertDtl_ProDate_date,'' as AlertDtl_RefNo_var,0 as MISRecordNo
        //                                ,ENQNEW_ClientName_var as AlertDtl_CL_Name_var,ENQNEW_SiteName_var as AlertDtl_SITE_Name_var,e.MATERIAL_Name_var as AlertDtl_RecType_var,0 as CL_Id,0 as SITE_Id,2 as AlertDtl_Alert_Id
        //                                ,(select USER_NAME_var from tbl_User x where x.USER_Id IN (select ROUTE_ME_Id from tbl_Route where ROUTE_Id=a.ENQNEW_ROUTE_Id)) as AlertDtl_MEName_var
        //                                ,null as AlertDtl_CollctnEnqDate_date,null as AlertDtl_BillModifiedDate_date,null as AlertDtl_InwdRecDate_date
        //                                ,null as AlertDtl_AppDate_date,null as AlertDtl_EnteredDate_date,null as AlertDtl_CheckedDate_date,null as AlertDtl_OutwrdDate_date,'' as AlertDtl_TestName_var
        //                                from tbl_EnquiryNewClient as a,tbl_Proposal as b,tbl_Material as e
        //                                where a.ENQNEW_Id=b.Proposal_EnqNo
        //                                and a.ENQNEW_MATERIAL_Id=e.MATERIAL_Id 
        //                                and CONVERT (DATE, b.Proposal_OrderDate_dt) >=CONVERT (DATE,'" + fromDate + "') and CONVERT (DATE, b.Proposal_OrderDate_dt) <=CONVERT (DATE,'" + ToDate + "') " +
        //                                @"and datediff(d,convert(date,b.Proposal_OrderDate_dt),convert(date,a.ENQNEW_CollectionDate_dt)) > 2
        //				                and datediff(d,convert(date,b.Proposal_OrderDate_dt),convert(date,GETDATE())) > 2
        //				                and b.Proposal_NewClientStatus=1 and b.Proposal_ActiveStatus_bit=0
        //                                order by AlertDtl_RecType_var";

        //                    }
        //                    else
        //                    {
        //                        str = @"----Material not collected in 2 days after receiving order
        //				                Select a.ENQ_Id as AlertDtl_EnqNo_int,a.ENQ_Date_dt as AlertDtl_EnqDate_date,b.Proposal_OrderDate_dt as AlertDtl_ProOrderDate_date,'' as AlertDtl_BillNo_int,null as AlertDtl_BillDate_date,0 as AlertDtl_BillAmt_dec,
        //                                b.Proposal_No as AlertDtl_ProNo_var,b.Proposal_Id as AlertDtl_ProNo_int,b.Proposal_Date as AlertDtl_ProDate_date,'' as AlertDtl_RefNo_var,0 as AlertDtl_RecNo_int,
        //                                c.CL_Name_var as AlertDtl_CL_Name_var,d.SITE_Name_var as AlertDtl_SITE_Name_var,e.MATERIAL_Name_var as AlertDtl_RecType_var,c.CL_Id as AlertDtl_CLId_int,d.SITE_Id as AlertDtl_SiteId_int,3 as AlertDtl_Alert_Id,
        //                                (select USER_NAME_var from tbl_User x where x.USER_Id IN (select ROUTE_ME_Id from tbl_Route where ROUTE_Id=d.SITE_Route_Id)) as AlertDtl_MEName_var
        //                                ,null as AlertDtl_CollctnEnqDate_date,null as AlertDtl_BillModifiedDate_date,null as AlertDtl_InwdRecDate_date
        //                                ,null as AlertDtl_AppDate_date,null as AlertDtl_EnteredDate_date,null as AlertDtl_CheckedDate_date,null as AlertDtl_OutwrdDate_date,'' as AlertDtl_TestName_var
        //                                from tbl_Enquiry as a,tbl_Proposal as b,tbl_Client as c,tbl_Site as d,tbl_Material as e
        //                                where a.ENQ_Id=b.Proposal_EnqNo
        //                                and a.ENQ_CL_Id=c.CL_Id
        //                                and a.ENQ_SITE_Id=d.SITE_Id
        //                                and a.ENQ_MATERIAL_Id=e.MATERIAL_Id
        //                                and CONVERT (DATE, b.Proposal_OrderDate_dt) >=CONVERT (DATE,'" + fromDate + "') and CONVERT (DATE, b.Proposal_OrderDate_dt) <=CONVERT (DATE,'" + ToDate + "') " +
        //                                @"and a.ENQ_CollectionDate_dt is null
        //                                and datediff(d,convert(date,b.Proposal_OrderDate_dt),convert(date,GETDATE())) > 2 
        //                                and b.Proposal_NewClientStatus=0 and b.Proposal_ActiveStatus_bit=0
        //                                 
        //                                Union
        //                                						
        //                                Select a.ENQNEW_Id as AlertDtl_EnqNo_int,a.ENQNEW_Date_dt as AlertDtl_EnqDate_date,b.Proposal_OrderDate_dt as AlertDtl_ProOrderDate_date,'' as AlertDtl_BillNo_int,null as AlertDtl_BillDate_date,0 as AlertDtl_BillAmt_dec,
        //                                b.Proposal_No as AlertDtl_ProNo_var,b.Proposal_Id as AlertDtl_ProNo_int,b.Proposal_Date as AlertDtl_ProDate_date,'' as AlertDtl_RefNo_var,0 as MISRecordNo
        //                                ,ENQNEW_ClientName_var as AlertDtl_CL_Name_var,ENQNEW_SiteName_var as AlertDtl_SITE_Name_var,e.MATERIAL_Name_var as AlertDtl_RecType_var,0 as CL_Id,0 as SITE_Id,2 as AlertDtl_Alert_Id
        //                                ,(select USER_NAME_var from tbl_User x where x.USER_Id IN (select ROUTE_ME_Id from tbl_Route where ROUTE_Id=a.ENQNEW_ROUTE_Id)) as AlertDtl_MEName_var
        //                                ,null as AlertDtl_CollctnEnqDate_date,null as AlertDtl_BillModifiedDate_date,null as AlertDtl_InwdRecDate_date
        //                                ,null as AlertDtl_AppDate_date,null as AlertDtl_EnteredDate_date,null as AlertDtl_CheckedDate_date,null as AlertDtl_OutwrdDate_date,'' as AlertDtl_TestName_var
        //                                from tbl_EnquiryNewClient as a,tbl_Proposal as b,tbl_Material as e
        //                                where a.ENQNEW_Id=b.Proposal_EnqNo
        //                                and a.ENQNEW_MATERIAL_Id=e.MATERIAL_Id 
        //                                and CONVERT (DATE, b.Proposal_OrderDate_dt) >=CONVERT (DATE,'" + fromDate + "') and CONVERT (DATE, b.Proposal_OrderDate_dt) <=CONVERT (DATE,'" + ToDate + "') " +
        //                                @"and a.ENQNEW_CollectionDate_dt is null
        //                                and datediff(d,convert(date,b.Proposal_OrderDate_dt),convert(date,GETDATE())) > 2
        //				                and b.Proposal_NewClientStatus=1 and b.Proposal_ActiveStatus_bit=0
        //                                order by AlertDtl_RecType_var";

        //                    }
        //                }
        //                else if (Alert_Id == 4)
        //                {
        //                    if (flagStatus == 0)
        //                    {
        //                        str = @"----Material not inwarded in 1 day after collection
        //				                Select a.ENQ_Id as AlertDtl_EnqNo_int,a.ENQ_Date_dt as AlertDtl_EnqDate_date,a.ENQ_CollectionDate_dt as AlertDtl_CollctnEnqDate_date,'' as AlertDtl_BillNo_int,null as AlertDtl_BillDate_date,0 as AlertDtl_BillAmt_dec,
        //                                '' as AlertDtl_ProNo_var,0 as AlertDtl_ProNo_int,null as AlertDtl_ProDate_date,'' as AlertDtl_RefNo_var,0 as AlertDtl_RecNo_int,
        //                                c.CL_Name_var as AlertDtl_CL_Name_var,d.SITE_Name_var as AlertDtl_SITE_Name_var,e.MATERIAL_Name_var as AlertDtl_RecType_var,c.CL_Id as AlertDtl_CLId_int,d.SITE_Id as AlertDtl_SiteId_int,4 as AlertDtl_Alert_Id,
        //                                (select USER_NAME_var from tbl_User x where x.USER_Id IN (select ROUTE_ME_Id from tbl_Route where ROUTE_Id=d.SITE_Route_Id)) as AlertDtl_MEName_var
        //                                ,null as AlertDtl_ProOrderDate_date,null as AlertDtl_BillModifiedDate_date,null as AlertDtl_InwdRecDate_date
        //                                ,null as AlertDtl_AppDate_date,null as AlertDtl_EnteredDate_date,null as AlertDtl_CheckedDate_date,null as AlertDtl_OutwrdDate_date,'' as AlertDtl_TestName_var
        //                                from tbl_Enquiry as a,tbl_Inward as b,tbl_Client as c,tbl_Site as d,tbl_Material as e
        //                                where a.ENQ_Id=b.INWD_ENQ_Id
        //                                and a.ENQ_CL_Id=c.CL_Id
        //                                and a.ENQ_SITE_Id=d.SITE_Id
        //                                and a.ENQ_MATERIAL_Id=e.MATERIAL_Id
        //                                and CONVERT (DATE, ENQ_CollectionDate_dt) >=CONVERT (DATE,'" + fromDate + "') and CONVERT (DATE,ENQ_CollectionDate_dt) <=CONVERT (DATE,'" + ToDate + "') " +
        //                                @"and datediff(d,convert(date,a.ENQ_CollectionDate_dt),convert(date,b.INWD_ReceivedDate_dt)) > 1
        //				                and datediff(d,convert(date,a.ENQ_CollectionDate_dt),convert(date,GETDATE())) > 1 
        //                                and a.ENQ_Status_tint <> 2 order by AlertDtl_RecType_var";
        //                    }
        //                    else
        //                    {
        //                        str = @"----Material not inwarded in 1 day after collection
        //				                Select a.ENQ_Id as AlertDtl_EnqNo_int,a.ENQ_Date_dt as AlertDtl_EnqDate_date,a.ENQ_CollectionDate_dt as AlertDtl_CollctnEnqDate_date,'' as AlertDtl_BillNo_int,null as AlertDtl_BillDate_date,0 as AlertDtl_BillAmt_dec,
        //                                '' as AlertDtl_ProNo_var,0 as AlertDtl_ProNo_int,null as AlertDtl_ProDate_date,'' as AlertDtl_RefNo_var,0 as AlertDtl_RecNo_int,
        //                                c.CL_Name_var as AlertDtl_CL_Name_var,d.SITE_Name_var as AlertDtl_SITE_Name_var,e.MATERIAL_Name_var as AlertDtl_RecType_var,c.CL_Id as AlertDtl_CLId_int,d.SITE_Id as AlertDtl_SiteId_int,4 as AlertDtl_Alert_Id,
        //                                (select USER_NAME_var from tbl_User x where x.USER_Id IN (select ROUTE_ME_Id from tbl_Route where ROUTE_Id=d.SITE_Route_Id)) as AlertDtl_MEName_var
        //                                ,null as AlertDtl_ProOrderDate_date,null as AlertDtl_BillModifiedDate_date,null as AlertDtl_InwdRecDate_date
        //                                ,null as AlertDtl_AppDate_date,null as AlertDtl_EnteredDate_date,null as AlertDtl_CheckedDate_date,null as AlertDtl_OutwrdDate_date,'' as AlertDtl_TestName_var
        //                                from tbl_Enquiry as a,tbl_Inward as b,tbl_Client as c,tbl_Site as d,tbl_Material as e
        //                                where a.ENQ_Id=b.INWD_ENQ_Id
        //                                and a.ENQ_CL_Id=c.CL_Id
        //                                and a.ENQ_SITE_Id=d.SITE_Id
        //                                and a.ENQ_MATERIAL_Id=e.MATERIAL_Id
        //                                and CONVERT (DATE, ENQ_CollectionDate_dt) >=CONVERT (DATE,'" + fromDate + "') and CONVERT (DATE,ENQ_CollectionDate_dt) <=CONVERT (DATE,'" + ToDate + "') " +
        //                                @"and b.INWD_ReceivedDate_dt is null 
        //                                and datediff(d,convert(date,a.ENQ_CollectionDate_dt),convert(date,GETDATE())) > 1 
        //                                and a.ENQ_Status_tint <> 2 order by AlertDtl_RecType_var";

        //                    }
        //                }
        //                else if (Alert_Id == 5)
        //                {
        //                    if (flagStatus == 0)
        //                    {
        //                        str = @"----Inward not approved 1 day after material inward
        //				                Select 0 as AlertDtl_EnqNo_int,null as AlertDtl_EnqDate_date,b.INWD_ReceivedDate_dt as AlertDtl_InwdRecDate_date,'' as AlertDtl_BillNo_int,null as AlertDtl_BillDate_date,0 as AlertDtl_BillAmt_dec,
        //                                '' as AlertDtl_ProNo_var,0 as AlertDtl_ProNo_int,null as AlertDtl_ProDate_date, MISRefNo as AlertDtl_RefNo_var,MISRecordNo as AlertDtl_RecNo_int,
        //                                c.CL_Name_var as AlertDtl_CL_Name_var,d.SITE_Name_var as AlertDtl_SITE_Name_var,
        //                                (select MATERIAL_Name_var from tbl_Material e where e.MATERIAL_RecordType_var=b.INWD_RecordType_var) as AlertDtl_RecType_var
        //				                ,c.CL_Id as AlertDtl_CLId_int,d.SITE_Id as AlertDtl_SiteId_int,5 as AlertDtl_Alert_Id,
        //                                (select USER_NAME_var from tbl_User x where x.USER_Id IN (select ROUTE_ME_Id from tbl_Route where ROUTE_Id=d.SITE_Route_Id)) as AlertDtl_MEName_var
        //                                ,null as AlertDtl_ProOrderDate_date,null as AlertDtl_BillModifiedDate_date,null as AlertDtl_CollctnEnqDate_date
        //                                ,null as AlertDtl_AppDate_date,null as AlertDtl_EnteredDate_date,null as AlertDtl_CheckedDate_date,null as AlertDtl_OutwrdDate_date,'' as AlertDtl_TestName_var
        //                                from tbl_Inward as b,tbl_Client as c,tbl_Site as d,tbl_MISDetail as f--,tbl_Material as e
        //                                where --a.ENQ_Id=b.INWD_ENQ_Id and
        //                                b.INWD_RecordNo_int=f.MISRecordNo
        //                                and b.INWD_RecordType_var=f.MISRecType
        //                                and b.INWD_CL_Id=c.CL_Id
        //                                and b.INWD_SITE_Id=d.SITE_Id
        //                                and CONVERT (DATE, b.INWD_ReceivedDate_dt) >=CONVERT (DATE,'" + fromDate + "') and CONVERT (DATE,b.INWD_ReceivedDate_dt) <=CONVERT (DATE,'" + ToDate + "') " +
        //                                @"and datediff(d,convert(date,b.INWD_ReceivedDate_dt),convert(date,f.MISApprovedDt)) > 1
        //				                and datediff(d,convert(date,b.INWD_ReceivedDate_dt),convert(date,GETDATE())) > 1
        //                                order by AlertDtl_RecType_var";
        //                    }
        //                    else
        //                    {
        //                        str = @"----Inward not approved 1 day after material inward
        //				                Select 0 as AlertDtl_EnqNo_int,null as AlertDtl_EnqDate_date,a.INWD_ReceivedDate_dt as AlertDtl_InwdRecDate_date,'' as AlertDtl_BillNo_int,null as AlertDtl_BillDate_date,0 as AlertDtl_BillAmt_dec,
        //                                '' as AlertDtl_ProNo_var,0 as AlertDtl_ProNo_int,null as AlertDtl_ProDate_date, MISRefNo as AlertDtl_RefNo_var,MISRecordNo as AlertDtl_RecNo_int,
        //                                c.CL_Name_var as AlertDtl_CL_Name_var,d.SITE_Name_var as AlertDtl_SITE_Name_var,
        //                                (select MATERIAL_Name_var from tbl_Material e where e.MATERIAL_RecordType_var=a.INWD_RecordType_var) as AlertDtl_RecType_var
        //				                ,c.CL_Id as AlertDtl_CLId_int,d.SITE_Id as AlertDtl_SiteId_int,5 as AlertDtl_Alert_Id,
        //                                (select USER_NAME_var from tbl_User x where x.USER_Id IN (select ROUTE_ME_Id from tbl_Route where ROUTE_Id=d.SITE_Route_Id)) as AlertDtl_MEName_var
        //                                ,null as AlertDtl_ProOrderDate_date,null as AlertDtl_BillModifiedDate_date,null as AlertDtl_CollctnEnqDate_date
        //                                ,null as AlertDtl_AppDate_date,null as AlertDtl_EnteredDate_date,null as AlertDtl_CheckedDate_date,null as AlertDtl_OutwrdDate_date,'' as AlertDtl_TestName_var
        //                                from tbl_Inward as b,tbl_Client as c,tbl_Site as d,tbl_MISDetail as f
        //                                where --a.ENQ_Id=b.INWD_ENQ_Id and --tbl_Enquiry as a,
        //                                b.INWD_RecordNo_int=f.MISRecordNo
        //                                and b.INWD_RecordType_var=f.MISRecType
        //                                and b.INWD_CL_Id=c.CL_Id
        //                                and b.INWD_SITE_Id=d.SITE_Id
        //                                and CONVERT (DATE, b.INWD_ReceivedDate_dt) >=CONVERT (DATE,'" + fromDate + "') and CONVERT (DATE,b.INWD_ReceivedDate_dt) <=CONVERT (DATE,'" + ToDate + "') " +
        //                                @" and f.MISApprovedDt is null 
        //                                and datediff(d,convert(date,b.INWD_ReceivedDate_dt),convert(date,GETDATE())) > 1
        //                                order by AlertDtl_RecType_var";

        //                    }
        //                }
        //                else if (Alert_Id == 6)
        //                {
        //                    if (flagStatus == 0)
        //                    {
        //                        str = @"----Data Not enterd in 5 day after material inward approval
        //				                Select 0 as AlertDtl_EnqNo_int,null as AlertDtl_EnqDate_date,f.MISApprovedDt as AlertDtl_AppDate_date,'' as AlertDtl_BillNo_int,null as AlertDtl_BillDate_date,0 as AlertDtl_BillAmt_dec,
        //                                '' as AlertDtl_ProNo_var,0 as AlertDtl_ProNo_int,null as AlertDtl_ProDate_date, MISRefNo as AlertDtl_RefNo_var,MISRecordNo as AlertDtl_RecNo_int,
        //                                c.CL_Name_var as AlertDtl_CL_Name_var,d.SITE_Name_var as AlertDtl_SITE_Name_var,
        //                                (select MATERIAL_Name_var from tbl_Material e where e.MATERIAL_RecordType_var=b.INWD_RecordType_var) as AlertDtl_RecType_var
        //				                ,c.CL_Id as AlertDtl_CLId_int,d.SITE_Id as AlertDtl_SiteId_int,6 as AlertDtl_Alert_Id,
        //                                (select USER_NAME_var from tbl_User x where x.USER_Id IN (select ROUTE_ME_Id from tbl_Route where ROUTE_Id=d.SITE_Route_Id)) as AlertDtl_MEName_var
        //                                ,null as AlertDtl_ProOrderDate_date,null as AlertDtl_BillModifiedDate_date,null as AlertDtl_CollctnEnqDate_date
        //                                ,null as AlertDtl_InwdRecDate_date,null as AlertDtl_EnteredDate_date,null as AlertDtl_CheckedDate_date,null as AlertDtl_OutwrdDate_date,'' as AlertDtl_TestName_var
        //                                from tbl_Inward as b,tbl_Client as c,tbl_Site as d,tbl_MISDetail as f--,tbl_Material as e
        //                                where --a.ENQ_Id=b.INWD_ENQ_Id and
        //                                b.INWD_RecordNo_int=f.MISRecordNo
        //                                and b.INWD_RecordType_var=f.MISRecType
        //                                and b.INWD_CL_Id=c.CL_Id
        //                                and b.INWD_SITE_Id=d.SITE_Id
        //                                and CONVERT (DATE, f.MISApprovedDt) >=CONVERT (DATE,'" + fromDate + "') and CONVERT (DATE,f.MISApprovedDt) <=CONVERT (DATE,'" + ToDate + "') " +
        //                                @"and datediff(d,convert(date,f.MISApprovedDt),convert(date,f.MISEnteredDt)) > 1
        //				                and datediff(d,convert(date,f.MISApprovedDt),convert(date,GETDATE())) > 1
        //                                order by AlertDtl_RecType_var";
        //                    }
        //                    else
        //                    {
        //                        str = @"----Data Not enterd in 5 day after material inward approval
        //				                Select 0 as AlertDtl_EnqNo_int,null as AlertDtl_EnqDate_date,f.MISApprovedDt as AlertDtl_AppDate_date,'' as AlertDtl_BillNo_int,null as AlertDtl_BillDate_date,0 as AlertDtl_BillAmt_dec,
        //                                '' as AlertDtl_ProNo_var,0 as AlertDtl_ProNo_int,null as AlertDtl_ProDate_date, MISRefNo as AlertDtl_RefNo_var,MISRecordNo as AlertDtl_RecNo_int,
        //                                c.CL_Name_var as AlertDtl_CL_Name_var,d.SITE_Name_var as AlertDtl_SITE_Name_var,
        //                                (select MATERIAL_Name_var from tbl_Material e where e.MATERIAL_RecordType_var=b.INWD_RecordType_var) as AlertDtl_RecType_var
        //				                ,c.CL_Id as AlertDtl_CLId_int,d.SITE_Id as AlertDtl_SiteId_int,6 as AlertDtl_Alert_Id,
        //                                (select USER_NAME_var from tbl_User x where x.USER_Id IN (select ROUTE_ME_Id from tbl_Route where ROUTE_Id=d.SITE_Route_Id)) as AlertDtl_MEName_var
        //                                ,null as AlertDtl_ProOrderDate_date,null as AlertDtl_BillModifiedDate_date,null as AlertDtl_CollctnEnqDate_date
        //                                ,null as AlertDtl_InwdRecDate_date,null as AlertDtl_EnteredDate_date,null as AlertDtl_CheckedDate_date,null as AlertDtl_OutwrdDate_date,'' as AlertDtl_TestName_var
        //                                from tbl_Inward as b,tbl_Client as c,tbl_Site as d,tbl_MISDetail as f
        //                                where --a.ENQ_Id=b.INWD_ENQ_Id and --tbl_Enquiry as a,
        //                                b.INWD_RecordNo_int=f.MISRecordNo
        //                                and b.INWD_RecordType_var=f.MISRecType
        //                                and b.INWD_CL_Id=c.CL_Id
        //                                and b.INWD_SITE_Id=d.SITE_Id
        //                                and CONVERT (DATE,f.MISApprovedDt) >=CONVERT (DATE,'" + fromDate + "') and CONVERT (DATE,f.MISApprovedDt) <=CONVERT (DATE,'" + ToDate + "') " +
        //                                @" and f.MISEnteredDt is null 
        //                                and datediff(d,convert(date,f.MISApprovedDt),convert(date,GETDATE())) > 1
        //                                order by AlertDtl_RecType_var";

        //                    }
        //                }
        //                else if (Alert_Id == 7)
        //                {
        //                    if (flagStatus == 0)
        //                    {
        //                        str = @"---Category wise test not completed in X days
        //                select CL_Name_var,SITE_Name_var,AACINWD_ReferenceNo_var as RefNo,AACINWD_CollectionDate_dt as CollctnDt,'AAC' as testType,
        //                (select TEST_Name_var from tbl_Test as d where a.AACINWD_TEST_Id=d.TEST_Id ) as TEST_Name_var,7 as AlertId
        //                from tbl_AAC_Inward  as a,tbl_Client as b,tbl_Site as c
        //                where a.AACINWD_CL_Id=b.CL_Id
        //                and a.AACINWD_SITE_Id=c.SITE_Id
        //                and Convert(Date,a.AACINWD_CollectionDate_dt) In (dateadd(dd,-((select distinct(TEST_TimeFrame) from tbl_Test as d where Test_RecType_var='AAC' and TEST_TimeFrame is NOT Null)+1),cast(getdate() as date)))
        //				and CONVERT (DATE, a.AACINWD_CollectionDate_dt) >=CONVERT (DATE,'" + fromDate + "') and CONVERT (DATE,a.AACINWD_CollectionDate_dt) <=CONVERT (DATE,'" + ToDate + "') " +
        //                @"and datediff(d,convert(date,a.AACINWD_CollectionDate_dt),convert(date,a.AACINWD_CollectionDate_dt)) > 1
        //				and datediff(d,convert(date,a.AACINWD_CollectionDate_dt),convert(date,GETDATE())) > 1
        //
        //                Union
        //
        //                select CL_Name_var,SITE_Name_var,AGGTINWD_ReferenceNo_var as RefNo,AGGTINWD_CollectionDate_dt as CollctnDt,'AGGT' as testType,
        //                (select TEST_Name_var from tbl_Test as d where e.AGGTTEST_TEST_Id=d.TEST_Id ) as TEST_Name_var,7 as AlertId
        //                from tbl_Aggregate_Inward  as a,tbl_Aggregate_Test as e,tbl_Client as b,tbl_Site as c
        //                where a.AGGTINWD_CL_Id=b.CL_Id
        //                and a.AGGTINWD_SITE_Id=c.SITE_Id
        //                and a.AGGTINWD_ReferenceNo_var=e.AGGTTEST_ReferenceNo_var
        //                and Convert(Date,a.AGGTINWD_CollectionDate_dt) In (dateadd(dd,-((select TEST_TimeFrame from tbl_Test as d where e.AGGTTEST_TEST_Id=d.TEST_Id and d.TEST_TimeFrame is not null)+1),cast(getdate() as date)))
        //                and CONVERT (DATE, a.AGGTINWD_CollectionDate_dt) >=CONVERT (DATE,'" + fromDate + "') and CONVERT (DATE,a.AGGTINWD_CollectionDate_dt) <=CONVERT (DATE,'" + ToDate + "') " +
        //                @"and datediff(d,convert(date,a.AGGTINWD_CollectionDate_dt),convert(date,a.AGGTINWD_CollectionDate_dt)) > 1
        //				and datediff(d,convert(date,a.AGGTINWD_CollectionDate_dt),convert(date,GETDATE())) > 1
        //
        //                Union
        //
        //                select CL_Name_var,SITE_Name_var,GTINW_RefNo_var as RefNo,GTINW_CollectionDate_dt as CollctnDt,'GT' as testType,
        //                (select TEST_Name_var from tbl_Test as d where e.GTTEST_Srno_tint=d.TEST_Id ) as TEST_Name_var,7 as AlertId
        //                from tbl_GT_Inward  as a,tbl_GT_Test as e,tbl_Client as b,tbl_Site as c
        //                where a.GTINW_CL_Id=b.CL_Id
        //                and a.GTINW_SITE_Id=c.SITE_Id
        //                and a.GTINW_RefNo_var=e.GTTEST_RefNo_var
        //                and Convert(Date,a.GTINW_CollectionDate_dt) In (dateadd(dd,-((Select distinct(TEST_TimeFrame) from tbl_Test as d where Test_RecType_var='GT' and TEST_TimeFrame is NOT Null)+1),cast(getdate() as date)))
        //                and CONVERT (DATE, a.GTINW_CollectionDate_dt) >=CONVERT (DATE,'" + fromDate + "') and CONVERT (DATE,a.GTINW_CollectionDate_dt) <=CONVERT (DATE,'" + ToDate + "') " +
        //                @"and datediff(d,convert(date,a.GTINW_CollectionDate_dt),convert(date,a.GTINW_CollectionDate_dt)) > 1
        //				and datediff(d,convert(date,a.GTINW_CollectionDate_dt),convert(date,GETDATE())) > 1
        //
        //                Union
        //
        //                select CL_Name_var,SITE_Name_var,CCHINWD_ReferenceNo_var as RefNo,CCHINWD_CollectionDate_dt as CollctnDt,'CCH' as testType,
        //                (select TEST_Name_var from tbl_Test as d where e.CCHTEST_TEST_Id=d.TEST_Id ) as TEST_Name_var,7 as AlertId
        //                from tbl_CementChemical_Inward  as a,tbl_CementChemical_Test as e,tbl_Client as b,tbl_Site as c 
        //                where a.CCHINWD_CL_Id=b.CL_Id
        //                and a.CCHINWD_SITE_Id=c.SITE_Id
        //                and a.CCHINWD_ReferenceNo_var=e.CCHTEST_ReferenceNo_var
        //                and Convert(Date,a.CCHINWD_CollectionDate_dt) In (dateadd(dd,-((Select distinct(TEST_TimeFrame) from tbl_Test as d where Test_RecType_var='CCH' and TEST_TimeFrame is NOT Null)+1),cast(getdate() as date)))
        //                and CONVERT (DATE, a.CCHINWD_CollectionDate_dt) >=CONVERT (DATE,'" + fromDate + "') and CONVERT (DATE,a.CCHINWD_CollectionDate_dt) <=CONVERT (DATE,'" + ToDate + "') " +
        //                @"and datediff(d,convert(date,a.CCHINWD_CollectionDate_dt),convert(date,a.CCHINWD_CollectionDate_dt)) > 1
        //				and datediff(d,convert(date,a.CCHINWD_CollectionDate_dt),convert(date,GETDATE())) > 1
        //
        //                Union
        //
        //                select CL_Name_var,SITE_Name_var,BTINWD_ReferenceNo_var as RefNo,BTINWD_CollectionDate_dt as CollctnDt,'BT-' as testType,
        //                (select TEST_Name_var from tbl_Test as d where e.BTTEST_TEST_Id=d.TEST_Id ) as TEST_Name_var,7 as AlertId
        //                from tbl_Brick_Inward  as a,tbl_Brick_Test as e,tbl_Client as b,tbl_Site as c 
        //                where a.BTINWD_CL_Id=b.CL_Id
        //                and a.BTINWD_SITE_Id=c.SITE_Id
        //                and a.BTINWD_ReferenceNo_var=e.BTTEST_ReferenceNo_var
        //                and Convert(Date,a.BTINWD_CollectionDate_dt) In (dateadd(dd,-((select TEST_TimeFrame from tbl_Test as d where e.BTTEST_TEST_Id=d.TEST_Id and d.TEST_TimeFrame is not null)+1),cast(getdate() as date)))
        //                and CONVERT (DATE, a.BTINWD_CollectionDate_dt) >=CONVERT (DATE,'" + fromDate + "') and CONVERT (DATE,a.BTINWD_CollectionDate_dt) <=CONVERT (DATE,'" + ToDate + "') " +
        //                @"and datediff(d,convert(date,a.BTINWD_CollectionDate_dt),convert(date,a.BTINWD_CollectionDate_dt)) > 1
        //				and datediff(d,convert(date,a.BTINWD_CollectionDate_dt),convert(date,GETDATE())) > 1
        //
        //                Union
        //
        //                select CL_Name_var,SITE_Name_var,CEMTINWD_ReferenceNo_var as RefNo,CEMTINWD_CollectionDate_dt as CollctnDt,'CEMT' as testType,
        //                (select TEST_Name_var from tbl_Test as d where e.CEMTTEST_TEST_Id=d.TEST_Id ) as TEST_Name_var,7 as AlertId
        //                from tbl_Cement_Inward  as a,tbl_Cement_Test as e,tbl_Client as b,tbl_Site as c 
        //                where a.CEMTINWD_CL_Id=b.CL_Id
        //                and a.CEMTINWD_SITE_Id=c.SITE_Id
        //                and a.CEMTINWD_ReferenceNo_var=e.CEMTTEST_ReferenceNo_var
        //                and Convert(Date,a.CEMTINWD_CollectionDate_dt) In (dateadd(dd,-((select TEST_TimeFrame from tbl_Test as d where e.CEMTTEST_TEST_Id=d.TEST_Id and d.TEST_TimeFrame is not null)+1),cast(getdate() as date)))
        //                and CONVERT (DATE, a.CEMTINWD_CollectionDate_dt) >=CONVERT (DATE,'" + fromDate + "') and CONVERT (DATE,a.CEMTINWD_CollectionDate_dt) <=CONVERT (DATE,'" + ToDate + "') " +
        //                @"and datediff(d,convert(date,a.CEMTINWD_CollectionDate_dt),convert(date,a.CEMTINWD_CollectionDate_dt)) > 1
        //				and datediff(d,convert(date,a.CEMTINWD_CollectionDate_dt),convert(date,GETDATE())) > 1
        //
        //                Union
        //
        //                select CL_Name_var,SITE_Name_var,CRINWD_ReferenceNo_var as RefNo,CRINWD_CollectionDate_dt as CollctnDt,'CR' as testType,
        //                (select TEST_Name_var from tbl_Test as d where a.CRINWD_TestId_int=d.TEST_Id ) as TEST_Name_var,7 as AlertId
        //                from tbl_Core_Inward  as a,tbl_Client as b,tbl_Site as c 
        //                where a.CRINWD_CL_Id=b.CL_Id
        //                and a.CRINWD_SITE_Id=c.SITE_Id
        //                and Convert(Date,a.CRINWD_CollectionDate_dt) In (dateadd(dd,-((Select distinct(TEST_TimeFrame) from tbl_Test as d where Test_RecType_var='CR' and TEST_TimeFrame is NOT Null)+1),cast(getdate() as date)))
        //                and CONVERT (DATE, a.CRINWD_CollectionDate_dt) >=CONVERT (DATE,'" + fromDate + "') and CONVERT (DATE,a.CRINWD_CollectionDate_dt) <=CONVERT (DATE,'" + ToDate + "') " +
        //                @"and datediff(d,convert(date,a.CRINWD_CollectionDate_dt),convert(date,a.CRINWD_CollectionDate_dt)) > 1
        //				and datediff(d,convert(date,a.CRINWD_CollectionDate_dt),convert(date,GETDATE())) > 1
        //
        //                Union
        //
        //                select CL_Name_var,SITE_Name_var,CoreCutINWD_ReferenceNo_var as RefNo,CoreCutINWD_CollectionDate_dt as CollctnDt,'CORECUT' as testType,
        //                (select TEST_Name_var from tbl_Test as d where a.CoreCutINWD_TEST_Id=d.TEST_Id ) as TEST_Name_var,7 as AlertId
        //                from tbl_CoreCutting_Inward  as a,tbl_Client as b,tbl_Site as c 
        //                where a.CoreCutINWD_CL_Id=b.CL_Id
        //                and a.CoreCutINWD_SITE_Id=c.SITE_Id
        //                and Convert(Date,a.CoreCutINWD_CollectionDate_dt) In (dateadd(dd,-((Select distinct(TEST_TimeFrame) from tbl_Test as d where Test_RecType_var='CORECUT' and TEST_TimeFrame is NOT Null)+1),cast(getdate() as date)))
        //                and CONVERT (DATE, a.CoreCutINWD_CollectionDate_dt) >=CONVERT (DATE,'" + fromDate + "') and CONVERT (DATE,a.CoreCutINWD_CollectionDate_dt) <=CONVERT (DATE,'" + ToDate + "') " +
        //                @"and datediff(d,convert(date,a.CoreCutINWD_CollectionDate_dt),convert(date,a.CoreCutINWD_CollectionDate_dt)) > 1
        //				and datediff(d,convert(date,a.CoreCutINWD_CollectionDate_dt),convert(date,GETDATE())) > 1
        //
        //                Union
        //
        //                select CL_Name_var,SITE_Name_var,CTINWD_ReferenceNo_var as RefNo,CTINWD_CollectionDate_dt as CollctnDt,'CT' as testType,
        //                (select TEST_Name_var from tbl_Test as d where a.CTINWD_TEST_Id=d.TEST_Id ) as TEST_Name_var,7 as AlertId
        //                from tbl_Cube_Inward  as a,tbl_Client as b,tbl_Site as c 
        //                where a.CTINWD_CL_Id=b.CL_Id
        //                and a.CTINWD_SITE_Id=c.SITE_Id
        //               and Convert(Date,a.CTINWD_CollectionDate_dt) In (dateadd(dd,-((Select distinct(TEST_TimeFrame) from tbl_Test as d where Test_RecType_var='CT' and TEST_TimeFrame is NOT Null)+1),cast(getdate() as date)))
        //                and CONVERT (DATE, a.CTINWD_CollectionDate_dt) >=CONVERT (DATE,'" + fromDate + "') and CONVERT (DATE,a.CTINWD_CollectionDate_dt) <=CONVERT (DATE,'" + ToDate + "') " +
        //                @"and datediff(d,convert(date,a.CTINWD_CollectionDate_dt),convert(date,a.CTINWD_CollectionDate_dt)) > 1
        //				and datediff(d,convert(date,a.CTINWD_CollectionDate_dt),convert(date,GETDATE())) > 1
        //
        //                Union
        //
        //                select CL_Name_var,SITE_Name_var,FlyAshINWD_ReferenceNo_var as RefNo,FlyAshINWD_CollectionDate_dt as CollctnDt,'FLYASH' as testType,
        //                (select TEST_Name_var from tbl_Test as d where e.FlyAshTEST_TEST_Id=d.TEST_Id ) as TEST_Name_var,7 as AlertId
        //                from tbl_FlyAsh_Inward  as a,tbl_FlyAsh_Test as e,tbl_Client as b,tbl_Site as c 
        //                where a.FlyAshINWD_CL_Id=b.CL_Id
        //                and a.FlyAshINWD_SITE_Id=c.SITE_Id
        //                and a.FlyAshINWD_ReferenceNo_var=e.FLYASHTEST_ReferenceNo_var
        //                and Convert(Date,a.FlyAshINWD_CollectionDate_dt) In (dateadd(dd,-((select TEST_TimeFrame from tbl_Test as d where E.FLYASHTEST_TEST_Id=d.TEST_Id and d.TEST_TimeFrame is not null)+1),cast(getdate() as date)))
        //                and CONVERT (DATE, a.FlyAshINWD_CollectionDate_dt) >=CONVERT (DATE,'" + fromDate + "') and CONVERT (DATE,a.FlyAshINWD_CollectionDate_dt) <=CONVERT (DATE,'" + ToDate + "') " +
        //                @"and datediff(d,convert(date,a.FlyAshINWD_CollectionDate_dt),convert(date,a.FlyAshINWD_CollectionDate_dt)) > 1
        //				and datediff(d,convert(date,a.FlyAshINWD_CollectionDate_dt),convert(date,GETDATE())) > 1
        //
        //                Union
        //
        //                select CL_Name_var,SITE_Name_var,MFINWD_ReferenceNo_var as RefNo,MFINWD_CollectionDate_dt as CollctnDt,'MF' as testType,
        //                (select TEST_Name_var from tbl_Test as d where a.MFINWD_TestId_int=d.TEST_Id ) as TEST_Name_var,7 as AlertId
        //                from tbl_MixDesign_Inward  as a,tbl_Client as b,tbl_Site as c 
        //                where a.MFINWD_CL_Id=b.CL_Id
        //                and a.MFINWD_SITE_Id=c.SITE_Id
        //                and Convert(Date,a.MFINWD_CollectionDate_dt) In (dateadd(dd,-((select TEST_TimeFrame from tbl_Test as d where a.MFINWD_TestId_int=d.TEST_Id and d.TEST_TimeFrame is not null)+1),cast(getdate() as date)))
        //                and CONVERT (DATE, a.MFINWD_CollectionDate_dt) >=CONVERT (DATE,'" + fromDate + "') and CONVERT (DATE,a.MFINWD_CollectionDate_dt) <=CONVERT (DATE,'" + ToDate + "') " +
        //                @"and datediff(d,convert(date,a.MFINWD_CollectionDate_dt),convert(date,a.MFINWD_CollectionDate_dt)) > 1
        //				and datediff(d,convert(date,a.MFINWD_CollectionDate_dt),convert(date,GETDATE())) > 1
        //
        //                Union
        //
        //                select CL_Name_var,SITE_Name_var,NDTINWD_ReferenceNo_var as RefNo,NDTINWD_CollectionDate_dt as CollctnDt,'NDT' as testType,
        //                (select TEST_Name_var from tbl_Test as d where e.NDTTEST_TEST_Id=d.TEST_Id ) as TEST_Name_var,7 as AlertId
        //                from tbl_NDT_Inward  as a,tbl_NDT_Test as e,tbl_Client as b,tbl_Site as c 
        //                where a.NDTINWD_CL_Id=b.CL_Id
        //                and a.NDTINWD_SITE_Id=c.SITE_Id
        //                and a.NDTINWD_ReferenceNo_var=e.NDTTEST_RefNo_var
        //                and Convert(Date,a.NDTINWD_CollectionDate_dt) In (dateadd(dd,-((Select distinct(TEST_TimeFrame) from tbl_Test as d where Test_RecType_var='NDT' and TEST_TimeFrame is NOT Null)+1),cast(getdate() as date)))
        //                and CONVERT (DATE, a.NDTINWD_CollectionDate_dt) >=CONVERT (DATE,'" + fromDate + "') and CONVERT (DATE,a.NDTINWD_CollectionDate_dt) <=CONVERT (DATE,'" + ToDate + "') " +
        //                @"and datediff(d,convert(date,a.NDTINWD_CollectionDate_dt),convert(date,a.NDTINWD_CollectionDate_dt)) > 1
        //				and datediff(d,convert(date,a.NDTINWD_CollectionDate_dt),convert(date,GETDATE())) > 1
        //
        //                Union
        //
        //                select CL_Name_var,SITE_Name_var,OTINWD_ReferenceNo_var as RefNo,OTINWD_CollectionDate_dt as CollctnDt,'OT' as testType,
        //                (select TEST_Name_var from tbl_Test as d where e.OTTEST_TEST_Id=d.TEST_Id ) as TEST_Name_var,7 as AlertId
        //                from tbl_Other_Inward  as a,tbl_Other_Test as e,tbl_Client as b,tbl_Site as c 
        //                where a.OTINWD_CL_Id=b.CL_Id
        //                and a.OTINWD_SITE_Id=c.SITE_Id
        //                and a.OTINWD_ReferenceNo_var=e.OTTEST_ReferenceNo_var
        //                and Convert(Date,a.OTINWD_CollectionDate_dt) In (dateadd(dd,-((Select distinct(TEST_TimeFrame) from tbl_Test as d where Test_RecType_var='OT' and TEST_TimeFrame is NOT Null)+1),cast(getdate() as date)))
        //                and CONVERT (DATE, a.OTINWD_CollectionDate_dt) >=CONVERT (DATE,'" + fromDate + "') and CONVERT (DATE,a.OTINWD_CollectionDate_dt) <=CONVERT (DATE,'" + ToDate + "') " +
        //                @"and datediff(d,convert(date,a.OTINWD_CollectionDate_dt),convert(date,a.OTINWD_CollectionDate_dt)) > 1
        //				and datediff(d,convert(date,a.OTINWD_CollectionDate_dt),convert(date,GETDATE())) > 1
        //
        //                Union
        //
        //                select CL_Name_var,SITE_Name_var,PTINWD_ReferenceNo_var as RefNo,PTINWD_CollectionDate_dt as CollctnDt,'PT' as testType,
        //                (select TEST_Name_var from tbl_Test as d where a.PTINWD_TEST_Id=d.TEST_Id ) as TEST_Name_var,7 as AlertId 
        //                from tbl_Pavement_Inward  as a,tbl_Client as b,tbl_Site as c
        //                where a.PTINWD_CL_Id=b.CL_Id
        //                and a.PTINWD_SITE_Id=c.SITE_Id
        //                and Convert(Date,a.PTINWD_CollectionDate_dt) In (dateadd(dd,-((select TEST_TimeFrame from tbl_Test as d where a.PTINWD_TEST_Id=d.TEST_Id and d.TEST_TimeFrame is not null)+1),cast(getdate() as date)))
        //                and CONVERT (DATE, a.PTINWD_CollectionDate_dt) >=CONVERT (DATE,'" + fromDate + "') and CONVERT (DATE,a.PTINWD_CollectionDate_dt) <=CONVERT (DATE,'" + ToDate + "') " +
        //                @"and datediff(d,convert(date,a.PTINWD_CollectionDate_dt),convert(date,a.PTINWD_CollectionDate_dt)) > 1
        //				and datediff(d,convert(date,a.PTINWD_CollectionDate_dt),convert(date,GETDATE())) > 1
        //
        //                Union
        //
        //                select CL_Name_var,SITE_Name_var,PILEINWD_ReferenceNo_var as RefNo,PILEINWD_CollectionDate_dt as CollctnDt,'PILE' as testType,
        //                (select TEST_Name_var from tbl_Test as d where a.PILEINWD_TEST_Id=d.TEST_Id ) as TEST_Name_var,7 as AlertId 
        //                from tbl_Pile_Inward  as a,tbl_Client as b,tbl_Site as c
        //                where a.PILEINWD_CL_Id=b.CL_Id
        //                and a.PILEINWD_SITE_Id=c.SITE_Id
        //                and Convert(Date,a.PILEINWD_CollectionDate_dt) In (dateadd(dd,-((Select distinct(TEST_TimeFrame) from tbl_Test as d where Test_RecType_var='PILE' and TEST_TimeFrame is NOT Null)+1),cast(getdate() as date)))
        //                and CONVERT (DATE, a.PILEINWD_CollectionDate_dt) >=CONVERT (DATE,'" + fromDate + "') and CONVERT (DATE,a.PILEINWD_CollectionDate_dt) <=CONVERT (DATE,'" + ToDate + "') " +
        //                @"and datediff(d,convert(date,a.PILEINWD_CollectionDate_dt),convert(date,a.PILEINWD_CollectionDate_dt)) > 1
        //				and datediff(d,convert(date,a.PILEINWD_CollectionDate_dt),convert(date,GETDATE())) > 1
        //
        //                Union
        //
        //                select CL_Name_var,SITE_Name_var,SOINWD_ReferenceNo_var as RefNo,SOINWD_CollectionDate_dt as CollctnDt,'SO' as testType,
        //                (select TEST_Name_var from tbl_Test as d where e.SOTEST_TEST_int=d.TEST_Id ) as TEST_Name_var,7 as AlertId
        //                from tbl_Soil_Inward  as a,tbl_Soil_Test as e,tbl_Client as b,tbl_Site as c
        //                where a.SOINWD_CL_Id=b.CL_Id
        //                and a.SOINWD_SITE_Id=c.SITE_Id
        //                and a.SOINWD_ReferenceNo_var=e.SOTEST_ReferenceNo_var
        //                and Convert(Date,a.SOINWD_CollectionDate_dt) In (dateadd(dd,-((select TEST_TimeFrame from tbl_Test as d where E.SOTEST_TEST_int=d.TEST_Id and d.TEST_TimeFrame is not null)+1),cast(getdate() as date)))
        //                and CONVERT (DATE, a.SOINWD_CollectionDate_dt) >=CONVERT (DATE,'" + fromDate + "') and CONVERT (DATE,a.SOINWD_CollectionDate_dt) <=CONVERT (DATE,'" + ToDate + "') " +
        //                @"and datediff(d,convert(date,a.SOINWD_CollectionDate_dt),convert(date,a.SOINWD_CollectionDate_dt)) > 1
        //				and datediff(d,convert(date,a.SOINWD_CollectionDate_dt),convert(date,GETDATE())) > 1
        //
        //                Union
        //
        //                select CL_Name_var,SITE_Name_var,SolidINWD_ReferenceNo_var as RefNo,SolidINWD_CollectionDate_dt as CollctnDt,'SOLID' as testType,
        //                (select TEST_Name_var from tbl_Test as d where a.SolidINWD_TEST_Id=d.TEST_Id ) as TEST_Name_var,7 as AlertId
        //                from tbl_Solid_Inward  as a,tbl_Client as b,tbl_Site as c
        //                where a.SolidINWD_CL_Id=b.CL_Id
        //                and a.SolidINWD_SITE_Id=c.SITE_Id
        //                and Convert(Date,a.SolidINWD_CollectionDate_dt) In (dateadd(dd,-((select TEST_TimeFrame from tbl_Test as d where a.SolidINWD_TEST_Id=d.TEST_Id and d.TEST_TimeFrame is not null)+1),cast(getdate() as date)))
        //                and CONVERT (DATE, a.SolidINWD_CollectionDate_dt) >=CONVERT (DATE,'" + fromDate + "') and CONVERT (DATE,a.SolidINWD_CollectionDate_dt) <=CONVERT (DATE,'" + ToDate + "') " +
        //                @"and datediff(d,convert(date,a.SolidINWD_CollectionDate_dt),convert(date,a.SolidINWD_CollectionDate_dt)) > 1
        //				and datediff(d,convert(date,a.SolidINWD_CollectionDate_dt),convert(date,GETDATE())) > 1
        //
        //                Union
        //
        //                select CL_Name_var,SITE_Name_var,STINWD_ReferenceNo_var as RefNo,STINWD_CollectionDate_dt as CollctnDt,'ST' as testType,
        //                (select TEST_Name_var from tbl_Test as d where e.STTEST_TEST_id=d.TEST_Id ) as TEST_Name_var,7 as AlertId
        //                from tbl_Steel_Inward  as a,tbl_Steel_Test as e,tbl_Client as b,tbl_Site as c
        //                where a.STINWD_CL_Id=b.CL_Id
        //                and a.STINWD_SITE_Id=c.SITE_Id
        //                and a.STINWD_ReferenceNo_var=e.STTEST_ReferenceNo_var
        //                and Convert(Date,a.STINWD_CollectionDate_dt) In (dateadd(dd,-((Select distinct(TEST_TimeFrame) from tbl_Test as d where Test_RecType_var='ST' and TEST_TimeFrame is NOT Null)+1),cast(getdate() as date)))
        //                and CONVERT (DATE, a.STINWD_CollectionDate_dt) >=CONVERT (DATE,'" + fromDate + "') and CONVERT (DATE,a.STINWD_CollectionDate_dt) <=CONVERT (DATE,'" + ToDate + "') " +
        //                @"and datediff(d,convert(date,a.STINWD_CollectionDate_dt),convert(date,a.STINWD_CollectionDate_dt)) > 1
        //				and datediff(d,convert(date,a.STINWD_CollectionDate_dt),convert(date,GETDATE())) > 1
        //
        //                Union
        //
        //                select CL_Name_var,SITE_Name_var,STCINWD_ReferenceNo_var as RefNo,STCINWD_CollectionDate_dt as CollctnDt,'STC' as testType,
        //                (select TEST_Name_var from tbl_Test as d where a.STCINWD_TEST_id=d.TEST_Id ) as TEST_Name_var,7 as AlertId
        //                from tbl_SteelChemical_Inward  as a,tbl_Client as b,tbl_Site as c
        //                where a.STCINWD_CL_Id=b.CL_Id
        //                and a.STCINWD_SITE_Id=c.SITE_Id
        //                and Convert(Date,a.STCINWD_CollectionDate_dt) In (dateadd(dd,-((Select distinct(TEST_TimeFrame) from tbl_Test as d where Test_RecType_var='STC' and TEST_TimeFrame is NOT Null)+1),cast(getdate() as date)))
        //                and CONVERT (DATE, a.STCINWD_CollectionDate_dt) >=CONVERT (DATE,'" + fromDate + "') and CONVERT (DATE,a.STCINWD_CollectionDate_dt) <=CONVERT (DATE,'" + ToDate + "') " +
        //                @"and datediff(d,convert(date,a.STCINWD_CollectionDate_dt),convert(date,a.STCINWD_CollectionDate_dt)) > 1
        //				and datediff(d,convert(date,a.STCINWD_CollectionDate_dt),convert(date,GETDATE())) > 1
        //
        //                Union
        //
        //                select CL_Name_var,SITE_Name_var,TileINWD_ReferenceNo_var as RefNo,TileINWD_CollectionDate_dt as CollctnDt,'TILE' as testType,
        //                (select TEST_Name_var from tbl_Test as d where a.TileINWD_TEST_id=d.TEST_Id ) as TEST_Name_var,7 as AlertId
        //                from tbl_Tile_Inward  as a,tbl_Client as b,tbl_Site as c
        //                where a.TileINWD_CL_Id=b.CL_Id
        //                and a.TileINWD_SITE_Id=c.SITE_Id
        //                and Convert(Date,a.TILEINWD_CollectionDate_dt) In (dateadd(dd,-((Select distinct(TEST_TimeFrame) from tbl_Test as d where Test_RecType_var='TILE' and TEST_TimeFrame is NOT Null)+1),cast(getdate() as date)))
        //                and CONVERT (DATE, a.TILEINWD_CollectionDate_dt) >=CONVERT (DATE,'" + fromDate + "') and CONVERT (DATE,a.TILEINWD_CollectionDate_dt) <=CONVERT (DATE,'" + ToDate + "') " +
        //                @"and datediff(d,convert(date,a.TILEINWD_CollectionDate_dt),convert(date,a.TILEINWD_CollectionDate_dt)) > 1
        //				and datediff(d,convert(date,a.TILEINWD_CollectionDate_dt),convert(date,GETDATE())) > 1
        //
        //                Union
        //
        //
        //                select CL_Name_var,SITE_Name_var,WTINWD_ReferenceNo_var as RefNo,WTINWD_CollectionDate_dt as CollctnDt,'WT' as testType,
        //                (select TEST_Name_var from tbl_Test as d where e.WTTEST_TEST_id=d.TEST_Id ) as TEST_Name_var,7 as AlertId
        //                from tbl_Water_Inward  as a,tbl_Water_Test as e,tbl_Client as b,tbl_Site as c
        //                where a.WTINWD_CL_Id=b.CL_Id
        //                and a.WTINWD_SITE_Id=c.SITE_Id
        //                and a.WTINWD_ReferenceNo_var=e.WTTEST_ReferenceNo_var
        //                and Convert(Date,a.WTINWD_CollectionDate_dt) In (dateadd(dd,-((Select distinct(TEST_TimeFrame) from tbl_Test as d where Test_RecType_var='WT' and TEST_TimeFrame is NOT Null)+1),cast(getdate() as date)))
        //                and CONVERT (DATE, a.WTINWD_CollectionDate_dt) >=CONVERT (DATE,'" + fromDate + "') and CONVERT (DATE,a.WTINWD_CollectionDate_dt) <=CONVERT (DATE,'" + ToDate + "') " +
        //                @"and datediff(d,convert(date,a.WTINWD_CollectionDate_dt),convert(date,a.WTINWD_CollectionDate_dt)) > 1
        //				and datediff(d,convert(date,a.WTINWD_CollectionDate_dt),convert(date,GETDATE())) > 1
        //                                ";
        //                    }
        //                    else
        //                    {
        //                        str = @"---Category wise test not completed in X days
        //                select CL_Name_var,SITE_Name_var,AACINWD_ReferenceNo_var as RefNo,AACINWD_CollectionDate_dt as CollctnDt,'AAC' as testType,
        //                (select TEST_Name_var from tbl_Test as d where a.AACINWD_TEST_Id=d.TEST_Id ) as TEST_Name_var,7 as AlertId
        //                from tbl_AAC_Inward  as a,tbl_Client as b,tbl_Site as c
        //                where a.AACINWD_CL_Id=b.CL_Id
        //                and a.AACINWD_SITE_Id=c.SITE_Id
        //                and a.AACINWD_OutwardDate_dt is null
        //                and Convert(Date,a.AACINWD_CollectionDate_dt) In (dateadd(dd,-((select distinct(TEST_TimeFrame) from tbl_Test as d where Test_RecType_var='AAC' and TEST_TimeFrame is NOT Null)+1),cast(getdate() as date)))
        //				and CONVERT (DATE, a.AACINWD_CollectionDate_dt) >=CONVERT (DATE,'" + fromDate + "') and CONVERT (DATE,a.AACINWD_CollectionDate_dt) <=CONVERT (DATE,'" + ToDate + "') " +
        //                @"and datediff(d,convert(date,a.AACINWD_CollectionDate_dt),convert(date,GETDATE())) > 1
        //
        //                Union
        //
        //                select CL_Name_var,SITE_Name_var,AGGTINWD_ReferenceNo_var as RefNo,AGGTINWD_CollectionDate_dt as CollctnDt,'AGGT' as testType,
        //                (select TEST_Name_var from tbl_Test as d where e.AGGTTEST_TEST_Id=d.TEST_Id ) as TEST_Name_var,7 as AlertId
        //                from tbl_Aggregate_Inward  as a,tbl_Aggregate_Test as e,tbl_Client as b,tbl_Site as c
        //                where a.AGGTINWD_CL_Id=b.CL_Id
        //                and a.AGGTINWD_SITE_Id=c.SITE_Id
        //                and a.AGGTINWD_ReferenceNo_var=e.AGGTTEST_ReferenceNo_var
        //                and a.AGGTINWD_OutwardDate_dt is null
        //                and Convert(Date,a.AGGTINWD_CollectionDate_dt) In (dateadd(dd,-((select TEST_TimeFrame from tbl_Test as d where e.AGGTTEST_TEST_Id=d.TEST_Id and d.TEST_TimeFrame is not null)+1),cast(getdate() as date)))
        //	            and CONVERT (DATE, a.AGGTINWD_CollectionDate_dt) >=CONVERT (DATE,'" + fromDate + "') and CONVERT (DATE,a.AGGTINWD_CollectionDate_dt) <=CONVERT (DATE,'" + ToDate + "') " +
        //                @"and datediff(d,convert(date,a.AGGTINWD_CollectionDate_dt),convert(date,GETDATE())) > 1
        //
        //                Union
        //
        //                select CL_Name_var,SITE_Name_var,GTINW_RefNo_var as RefNo,GTINW_CollectionDate_dt as CollctnDt,'GT' as testType,
        //                (select TEST_Name_var from tbl_Test as d where e.GTTEST_Srno_tint=d.TEST_Id ) as TEST_Name_var,7 as AlertId
        //                from tbl_GT_Inward  as a,tbl_GT_Test as e,tbl_Client as b,tbl_Site as c
        //                where a.GTINW_CL_Id=b.CL_Id
        //                and a.GTINW_SITE_Id=c.SITE_Id
        //                and a.GTINW_RefNo_var=e.GTTEST_RefNo_var
        //                and a.GTINW_OutwardDate_dt is null
        //                and Convert(Date,a.GTINW_CollectionDate_dt) In (dateadd(dd,-((Select distinct(TEST_TimeFrame) from tbl_Test as d where Test_RecType_var='GT' and TEST_TimeFrame is NOT Null)+1),cast(getdate() as date)))
        //	            and CONVERT (DATE, a.GTINW_CollectionDate_dt) >=CONVERT (DATE,'" + fromDate + "') and CONVERT (DATE,a.GTINW_CollectionDate_dt) <=CONVERT (DATE,'" + ToDate + "') " +
        //                @"and datediff(d,convert(date,a.GTINW_CollectionDate_dt),convert(date,GETDATE())) > 1
        //
        //                Union
        //
        //                select CL_Name_var,SITE_Name_var,CCHINWD_ReferenceNo_var as RefNo,CCHINWD_CollectionDate_dt as CollctnDt,'CCH' as testType,
        //                (select TEST_Name_var from tbl_Test as d where e.CCHTEST_TEST_Id=d.TEST_Id ) as TEST_Name_var,7 as AlertId
        //                from tbl_CementChemical_Inward  as a,tbl_CementChemical_Test as e,tbl_Client as b,tbl_Site as c 
        //                where a.CCHINWD_CL_Id=b.CL_Id
        //                and a.CCHINWD_SITE_Id=c.SITE_Id
        //                and a.CCHINWD_ReferenceNo_var=e.CCHTEST_ReferenceNo_var
        //                and a.CCHINWD_OutwardDate_dt is null
        //                and Convert(Date,a.CCHINWD_CollectionDate_dt) In (dateadd(dd,-((Select distinct(TEST_TimeFrame) from tbl_Test as d where Test_RecType_var='CCH' and TEST_TimeFrame is NOT Null)+1),cast(getdate() as date)))
        //	            and CONVERT (DATE, a.CCHINWD_CollectionDate_dt) >=CONVERT (DATE,'" + fromDate + "') and CONVERT (DATE,a.CCHINWD_CollectionDate_dt) <=CONVERT (DATE,'" + ToDate + "') " +
        //                @"and datediff(d,convert(date,a.CCHINWD_CollectionDate_dt),convert(date,GETDATE())) > 1
        //
        //                Union
        //
        //                select CL_Name_var,SITE_Name_var,BTINWD_ReferenceNo_var as RefNo,BTINWD_CollectionDate_dt as CollctnDt,'BT-' as testType,
        //                (select TEST_Name_var from tbl_Test as d where e.BTTEST_TEST_Id=d.TEST_Id ) as TEST_Name_var,7 as AlertId
        //                from tbl_Brick_Inward  as a,tbl_Brick_Test as e,tbl_Client as b,tbl_Site as c 
        //                where a.BTINWD_CL_Id=b.CL_Id
        //                and a.BTINWD_SITE_Id=c.SITE_Id
        //                and a.BTINWD_ReferenceNo_var=e.BTTEST_ReferenceNo_var
        //                and a.BTINWD_OutwardDate_dt is null
        //                and Convert(Date,a.BTINWD_CollectionDate_dt) In (dateadd(dd,-((select TEST_TimeFrame from tbl_Test as d where e.BTTEST_TEST_Id=d.TEST_Id and d.TEST_TimeFrame is not null)+1),cast(getdate() as date)))
        //	            and CONVERT (DATE, a.BTINWD_CollectionDate_dt) >=CONVERT (DATE,'" + fromDate + "') and CONVERT (DATE,a.BTINWD_CollectionDate_dt) <=CONVERT (DATE,'" + ToDate + "') " +
        //                @"and datediff(d,convert(date,a.BTINWD_CollectionDate_dt),convert(date,GETDATE())) > 1
        //
        //                Union
        //
        //                select CL_Name_var,SITE_Name_var,CEMTINWD_ReferenceNo_var as RefNo,CEMTINWD_CollectionDate_dt as CollctnDt,'CEMT' as testType,
        //                (select TEST_Name_var from tbl_Test as d where e.CEMTTEST_TEST_Id=d.TEST_Id ) as TEST_Name_var,7 as AlertId
        //                from tbl_Cement_Inward  as a,tbl_Cement_Test as e,tbl_Client as b,tbl_Site as c 
        //                where a.CEMTINWD_CL_Id=b.CL_Id
        //                and a.CEMTINWD_SITE_Id=c.SITE_Id
        //                and a.CEMTINWD_ReferenceNo_var=e.CEMTTEST_ReferenceNo_var
        //                and a.CEMTINWD_OutwardDate_dt is null
        //                and Convert(Date,a.CEMTINWD_CollectionDate_dt) In (dateadd(dd,-((select TEST_TimeFrame from tbl_Test as d where e.CEMTTEST_TEST_Id=d.TEST_Id and d.TEST_TimeFrame is not null)+1),cast(getdate() as date)))
        //	            and CONVERT (DATE, a.CEMTINWD_CollectionDate_dt) >=CONVERT (DATE,'" + fromDate + "') and CONVERT (DATE,a.CEMTINWD_CollectionDate_dt) <=CONVERT (DATE,'" + ToDate + "') " +
        //                @"and datediff(d,convert(date,a.CEMTINWD_CollectionDate_dt),convert(date,GETDATE())) > 1
        //
        //                Union
        //
        //                select CL_Name_var,SITE_Name_var,CRINWD_ReferenceNo_var as RefNo,CRINWD_CollectionDate_dt as CollctnDt,'CR' as testType,
        //                (select TEST_Name_var from tbl_Test as d where a.CRINWD_TestId_int=d.TEST_Id ) as TEST_Name_var,7 as AlertId
        //                from tbl_Core_Inward  as a,tbl_Client as b,tbl_Site as c 
        //                where a.CRINWD_CL_Id=b.CL_Id
        //                and a.CRINWD_SITE_Id=c.SITE_Id
        //                and a.CRINWD_OutwardDate_dt is null
        //                and Convert(Date,a.CRINWD_CollectionDate_dt) In (dateadd(dd,-((Select distinct(TEST_TimeFrame) from tbl_Test as d where Test_RecType_var='CR' and TEST_TimeFrame is NOT Null)+1),cast(getdate() as date)))
        //	            and CONVERT (DATE, a.CRINWD_CollectionDate_dt) >=CONVERT (DATE,'" + fromDate + "') and CONVERT (DATE,a.CRINWD_CollectionDate_dt) <=CONVERT (DATE,'" + ToDate + "') " +
        //                @"and datediff(d,convert(date,a.CRINWD_CollectionDate_dt),convert(date,GETDATE())) > 1
        //
        //                Union
        //
        //                select CL_Name_var,SITE_Name_var,CoreCutINWD_ReferenceNo_var as RefNo,CoreCutINWD_CollectionDate_dt as CollctnDt,'CORECUT' as testType,
        //                (select TEST_Name_var from tbl_Test as d where a.CoreCutINWD_TEST_Id=d.TEST_Id ) as TEST_Name_var,7 as AlertId
        //                from tbl_CoreCutting_Inward  as a,tbl_Client as b,tbl_Site as c 
        //                where a.CoreCutINWD_CL_Id=b.CL_Id
        //                and a.CoreCutINWD_SITE_Id=c.SITE_Id
        //                and a.CoreCutINWD_OutwardDate_dt is null
        //                and Convert(Date,a.CoreCutINWD_CollectionDate_dt) In (dateadd(dd,-((Select distinct(TEST_TimeFrame) from tbl_Test as d where Test_RecType_var='CORECUT' and TEST_TimeFrame is NOT Null)+1),cast(getdate() as date)))
        //	            and CONVERT (DATE, a.CoreCutINWD_CollectionDate_dt) >=CONVERT (DATE,'" + fromDate + "') and CONVERT (DATE,a.CoreCutINWD_CollectionDate_dt) <=CONVERT (DATE,'" + ToDate + "') " +
        //                @"and datediff(d,convert(date,a.CoreCutINWD_CollectionDate_dt),convert(date,GETDATE())) > 1
        //
        //                Union
        //
        //                select CL_Name_var,SITE_Name_var,CTINWD_ReferenceNo_var as RefNo,CTINWD_CollectionDate_dt as CollctnDt,'CT' as testType,
        //                (select TEST_Name_var from tbl_Test as d where a.CTINWD_TEST_Id=d.TEST_Id ) as TEST_Name_var,7 as AlertId
        //                from tbl_Cube_Inward  as a,tbl_Client as b,tbl_Site as c 
        //                where a.CTINWD_CL_Id=b.CL_Id
        //                and a.CTINWD_SITE_Id=c.SITE_Id
        //                and a.CTINWD_OutwardDate_dt is null
        //                and Convert(Date,a.CTINWD_CollectionDate_dt) In (dateadd(dd,-((Select distinct(TEST_TimeFrame) from tbl_Test as d where Test_RecType_var='CT' and TEST_TimeFrame is NOT Null)+1),cast(getdate() as date)))
        //	            and CONVERT (DATE, a.CTINWD_CollectionDate_dt) >=CONVERT (DATE,'" + fromDate + "') and CONVERT (DATE,a.CTINWD_CollectionDate_dt) <=CONVERT (DATE,'" + ToDate + "') " +
        //                @"and datediff(d,convert(date,a.CTINWD_CollectionDate_dt),convert(date,GETDATE())) > 1
        //
        //                Union
        //
        //                select CL_Name_var,SITE_Name_var,FlyAshINWD_ReferenceNo_var as RefNo,FlyAshINWD_CollectionDate_dt as CollctnDt,'FLYASH' as testType,
        //                (select TEST_Name_var from tbl_Test as d where e.FlyAshTEST_TEST_Id=d.TEST_Id ) as TEST_Name_var,7 as AlertId
        //                from tbl_FlyAsh_Inward  as a,tbl_FlyAsh_Test as e,tbl_Client as b,tbl_Site as c 
        //                where a.FlyAshINWD_CL_Id=b.CL_Id
        //                and a.FlyAshINWD_SITE_Id=c.SITE_Id
        //                and a.FlyAshINWD_ReferenceNo_var=e.FLYASHTEST_ReferenceNo_var
        //                and a.FlyAshINWD_OutwardDate_dt is null
        //                and Convert(Date,a.FlyAshINWD_CollectionDate_dt) In (dateadd(dd,-((select TEST_TimeFrame from tbl_Test as d where E.FLYASHTEST_TEST_Id=d.TEST_Id and d.TEST_TimeFrame is not null)+1),cast(getdate() as date)))
        //	            and CONVERT (DATE, a.FlyAshINWD_CollectionDate_dt) >=CONVERT (DATE,'" + fromDate + "') and CONVERT (DATE,a.FlyAshINWD_CollectionDate_dt) <=CONVERT (DATE,'" + ToDate + "') " +
        //                @"and datediff(d,convert(date,a.FlyAshINWD_CollectionDate_dt),convert(date,GETDATE())) > 1
        //
        //                Union
        //
        //                select CL_Name_var,SITE_Name_var,MFINWD_ReferenceNo_var as RefNo,MFINWD_CollectionDate_dt as CollctnDt,'MF' as testType,
        //                (select TEST_Name_var from tbl_Test as d where a.MFINWD_TestId_int=d.TEST_Id ) as TEST_Name_var,7 as AlertId
        //                from tbl_MixDesign_Inward  as a,tbl_Client as b,tbl_Site as c 
        //                where a.MFINWD_CL_Id=b.CL_Id
        //                and a.MFINWD_SITE_Id=c.SITE_Id
        //                and a.MFINWD_OutwardDate_dt is null
        //                and Convert(Date,a.MFINWD_CollectionDate_dt) In (dateadd(dd,-((select TEST_TimeFrame from tbl_Test as d where a.MFINWD_TestId_int=d.TEST_Id and d.TEST_TimeFrame is not null)+1),cast(getdate() as date)))
        //	            and CONVERT (DATE, a.MFINWD_CollectionDate_dt) >=CONVERT (DATE,'" + fromDate + "') and CONVERT (DATE,a.MFINWD_CollectionDate_dt) <=CONVERT (DATE,'" + ToDate + "') " +
        //                @"and datediff(d,convert(date,a.MFINWD_CollectionDate_dt),convert(date,GETDATE())) > 1
        //
        //                Union
        //
        //                select CL_Name_var,SITE_Name_var,NDTINWD_ReferenceNo_var as RefNo,NDTINWD_CollectionDate_dt as CollctnDt,'NDT' as testType,
        //                (select TEST_Name_var from tbl_Test as d where e.NDTTEST_TEST_Id=d.TEST_Id ) as TEST_Name_var,7 as AlertId
        //                from tbl_NDT_Inward  as a,tbl_NDT_Test as e,tbl_Client as b,tbl_Site as c 
        //                where a.NDTINWD_CL_Id=b.CL_Id
        //                and a.NDTINWD_SITE_Id=c.SITE_Id
        //                and a.NDTINWD_ReferenceNo_var=e.NDTTEST_RefNo_var
        //                and a.NDTINWD_OutwardDate_dt is null
        //                and Convert(Date,a.NDTINWD_CollectionDate_dt) In (dateadd(dd,-((Select distinct(TEST_TimeFrame) from tbl_Test as d where Test_RecType_var='NDT' and TEST_TimeFrame is NOT Null)+1),cast(getdate() as date)))
        //	            and CONVERT (DATE, a.NDTINWD_CollectionDate_dt) >=CONVERT (DATE,'" + fromDate + "') and CONVERT (DATE,a.NDTINWD_CollectionDate_dt) <=CONVERT (DATE,'" + ToDate + "') " +
        //                @"and datediff(d,convert(date,a.NDTINWD_CollectionDate_dt),convert(date,GETDATE())) > 1
        //
        //                Union
        //
        //                select CL_Name_var,SITE_Name_var,OTINWD_ReferenceNo_var as RefNo,OTINWD_CollectionDate_dt as CollctnDt,'OT' as testType,
        //                (select TEST_Name_var from tbl_Test as d where e.OTTEST_TEST_Id=d.TEST_Id ) as TEST_Name_var,7 as AlertId
        //                from tbl_Other_Inward  as a,tbl_Other_Test as e,tbl_Client as b,tbl_Site as c 
        //                where a.OTINWD_CL_Id=b.CL_Id
        //                and a.OTINWD_SITE_Id=c.SITE_Id
        //                and a.OTINWD_ReferenceNo_var=e.OTTEST_ReferenceNo_var
        //                and a.OTINWD_OutwardDate_dt is null
        //                and Convert(Date,a.OTINWD_CollectionDate_dt) In (dateadd(dd,-((Select distinct(TEST_TimeFrame) from tbl_Test as d where Test_RecType_var='OT' and TEST_TimeFrame is NOT Null)+1),cast(getdate() as date)))
        //	            and CONVERT (DATE, a.OTINWD_CollectionDate_dt) >=CONVERT (DATE,'" + fromDate + "') and CONVERT (DATE,a.OTINWD_CollectionDate_dt) <=CONVERT (DATE,'" + ToDate + "') " +
        //                @"and datediff(d,convert(date,a.OTINWD_CollectionDate_dt),convert(date,GETDATE())) > 1
        //
        //                Union
        //
        //                select CL_Name_var,SITE_Name_var,PTINWD_ReferenceNo_var as RefNo,PTINWD_CollectionDate_dt as CollctnDt,'PT' as testType,
        //                (select TEST_Name_var from tbl_Test as d where a.PTINWD_TEST_Id=d.TEST_Id ) as TEST_Name_var,7 as AlertId 
        //                from tbl_Pavement_Inward  as a,tbl_Client as b,tbl_Site as c
        //                where a.PTINWD_CL_Id=b.CL_Id
        //                and a.PTINWD_SITE_Id=c.SITE_Id
        //                and a.PTINWD_OutwardDate_dt is null
        //                and Convert(Date,a.PTINWD_CollectionDate_dt) In (dateadd(dd,-((select TEST_TimeFrame from tbl_Test as d where a.PTINWD_TEST_Id=d.TEST_Id and d.TEST_TimeFrame is not null)+1),cast(getdate() as date)))
        //	            and CONVERT (DATE, a.PTINWD_CollectionDate_dt) >=CONVERT (DATE,'" + fromDate + "') and CONVERT (DATE,a.PTINWD_CollectionDate_dt) <=CONVERT (DATE,'" + ToDate + "') " +
        //                @"and datediff(d,convert(date,a.PTINWD_CollectionDate_dt),convert(date,GETDATE())) > 1
        //
        //                Union
        //
        //                select CL_Name_var,SITE_Name_var,PILEINWD_ReferenceNo_var as RefNo,PILEINWD_CollectionDate_dt as CollctnDt,'PILE' as testType,
        //                (select TEST_Name_var from tbl_Test as d where a.PILEINWD_TEST_Id=d.TEST_Id ) as TEST_Name_var,7 as AlertId 
        //                from tbl_Pile_Inward  as a,tbl_Client as b,tbl_Site as c
        //                where a.PILEINWD_CL_Id=b.CL_Id
        //                and a.PILEINWD_SITE_Id=c.SITE_Id
        //                and a.PILEINWD_OutwardDate_dt is null
        //                and Convert(Date,a.PILEINWD_CollectionDate_dt) In (dateadd(dd,-((Select distinct(TEST_TimeFrame) from tbl_Test as d where Test_RecType_var='PILE' and TEST_TimeFrame is NOT Null)+1),cast(getdate() as date)))
        //	            and CONVERT (DATE, a.PILEINWD_CollectionDate_dt) >=CONVERT (DATE,'" + fromDate + "') and CONVERT (DATE,a.PILEINWD_CollectionDate_dt) <=CONVERT (DATE,'" + ToDate + "') " +
        //                @"and datediff(d,convert(date,a.PILEINWD_CollectionDate_dt),convert(date,GETDATE())) > 1
        //
        //                Union
        //
        //                select CL_Name_var,SITE_Name_var,SOINWD_ReferenceNo_var as RefNo,SOINWD_CollectionDate_dt as CollctnDt,'SO' as testType,
        //                (select TEST_Name_var from tbl_Test as d where e.SOTEST_TEST_int=d.TEST_Id ) as TEST_Name_var,7 as AlertId
        //                from tbl_Soil_Inward  as a,tbl_Soil_Test as e,tbl_Client as b,tbl_Site as c
        //                where a.SOINWD_CL_Id=b.CL_Id
        //                and a.SOINWD_SITE_Id=c.SITE_Id
        //                and a.SOINWD_ReferenceNo_var=e.SOTEST_ReferenceNo_var
        //                and a.SOINWD_OutwardDate_dt is null
        //                and Convert(Date,a.SOINWD_CollectionDate_dt) In (dateadd(dd,-((select TEST_TimeFrame from tbl_Test as d where E.SOTEST_TEST_int=d.TEST_Id and d.TEST_TimeFrame is not null)+1),cast(getdate() as date)))
        //	            and CONVERT (DATE, a.SOINWD_CollectionDate_dt) >=CONVERT (DATE,'" + fromDate + "') and CONVERT (DATE,a.SOINWD_CollectionDate_dt) <=CONVERT (DATE,'" + ToDate + "') " +
        //                @"and datediff(d,convert(date,a.SOINWD_CollectionDate_dt),convert(date,GETDATE())) > 1
        //
        //                Union
        //
        //                select CL_Name_var,SITE_Name_var,SolidINWD_ReferenceNo_var as RefNo,SolidINWD_CollectionDate_dt as CollctnDt,'SOLID' as testType,
        //                (select TEST_Name_var from tbl_Test as d where a.SolidINWD_TEST_Id=d.TEST_Id ) as TEST_Name_var,7 as AlertId
        //                from tbl_Solid_Inward  as a,tbl_Client as b,tbl_Site as c
        //                where a.SolidINWD_CL_Id=b.CL_Id
        //                and a.SolidINWD_SITE_Id=c.SITE_Id
        //                and a.SolidINWD_OutwardDate_dt is null
        //                and Convert(Date,a.SolidINWD_CollectionDate_dt) In (dateadd(dd,-((select TEST_TimeFrame from tbl_Test as d where a.SolidINWD_TEST_Id=d.TEST_Id and d.TEST_TimeFrame is not null)+1),cast(getdate() as date)))
        //	            and CONVERT (DATE, a.SolidINWD_CollectionDate_dt) >=CONVERT (DATE,'" + fromDate + "') and CONVERT (DATE,a.SolidINWD_CollectionDate_dt) <=CONVERT (DATE,'" + ToDate + "') " +
        //                @"and datediff(d,convert(date,a.SolidINWD_CollectionDate_dt),convert(date,GETDATE())) > 1
        //
        //                Union
        //
        //                select CL_Name_var,SITE_Name_var,STINWD_ReferenceNo_var as RefNo,STINWD_CollectionDate_dt as CollctnDt,'ST' as testType,
        //                (select TEST_Name_var from tbl_Test as d where e.STTEST_TEST_id=d.TEST_Id ) as TEST_Name_var,7 as AlertId
        //                from tbl_Steel_Inward  as a,tbl_Steel_Test as e,tbl_Client as b,tbl_Site as c
        //                where a.STINWD_CL_Id=b.CL_Id
        //                and a.STINWD_SITE_Id=c.SITE_Id
        //                and a.STINWD_ReferenceNo_var=e.STTEST_ReferenceNo_var
        //                and a.STINWD_OutwardDate_dt is null
        //                and Convert(Date,a.STINWD_CollectionDate_dt) In (dateadd(dd,-((Select distinct(TEST_TimeFrame) from tbl_Test as d where Test_RecType_var='ST' and TEST_TimeFrame is NOT Null)+1),cast(getdate() as date)))
        //	            and CONVERT (DATE, a.STINWD_CollectionDate_dt) >=CONVERT (DATE,'" + fromDate + "') and CONVERT (DATE,a.STINWD_CollectionDate_dt) <=CONVERT (DATE,'" + ToDate + "') " +
        //                @"and datediff(d,convert(date,a.STINWD_CollectionDate_dt),convert(date,GETDATE())) > 1
        //
        //                Union
        //
        //                select CL_Name_var,SITE_Name_var,STCINWD_ReferenceNo_var as RefNo,STCINWD_CollectionDate_dt as CollctnDt,'STC' as testType,
        //                (select TEST_Name_var from tbl_Test as d where a.STCINWD_TEST_id=d.TEST_Id ) as TEST_Name_var,7 as AlertId
        //                from tbl_SteelChemical_Inward  as a,tbl_Client as b,tbl_Site as c
        //                where a.STCINWD_CL_Id=b.CL_Id
        //                and a.STCINWD_SITE_Id=c.SITE_Id
        //                and a.STCINWD_OutwardDate_dt is null
        //                and Convert(Date,a.STCINWD_CollectionDate_dt) In (dateadd(dd,-((Select distinct(TEST_TimeFrame) from tbl_Test as d where Test_RecType_var='STC' and TEST_TimeFrame is NOT Null)+1),cast(getdate() as date)))
        //	            and CONVERT (DATE, a.STCINWD_CollectionDate_dt) >=CONVERT (DATE,'" + fromDate + "') and CONVERT (DATE,a.STCINWD_CollectionDate_dt) <=CONVERT (DATE,'" + ToDate + "') " +
        //                @"and datediff(d,convert(date,a.STCINWD_CollectionDate_dt),convert(date,GETDATE())) > 1
        //
        //                Union
        //
        //                select CL_Name_var,SITE_Name_var,TileINWD_ReferenceNo_var as RefNo,TileINWD_CollectionDate_dt as CollctnDt,'TILE' as testType,
        //                (select TEST_Name_var from tbl_Test as d where a.TileINWD_TEST_id=d.TEST_Id ) as TEST_Name_var,7 as AlertId
        //                from tbl_Tile_Inward  as a,tbl_Client as b,tbl_Site as c
        //                where a.TileINWD_CL_Id=b.CL_Id
        //                and a.TileINWD_SITE_Id=c.SITE_Id
        //                and a.TILEINWD_OutwardDate_dt is null
        //                and Convert(Date,a.TILEINWD_CollectionDate_dt) In (dateadd(dd,-((Select distinct(TEST_TimeFrame) from tbl_Test as d where Test_RecType_var='TILE' and TEST_TimeFrame is NOT Null)+1),cast(getdate() as date)))
        //	            and CONVERT (DATE, a.TILEINWD_CollectionDate_dt) >=CONVERT (DATE,'" + fromDate + "') and CONVERT (DATE,a.TILEINWD_CollectionDate_dt) <=CONVERT (DATE,'" + ToDate + "') " +
        //                @"and datediff(d,convert(date,a.TILEINWD_CollectionDate_dt),convert(date,GETDATE())) > 1
        //
        //                Union
        //
        //
        //                select CL_Name_var,SITE_Name_var,WTINWD_ReferenceNo_var as RefNo,WTINWD_CollectionDate_dt as CollctnDt,'WT' as testType,
        //                (select TEST_Name_var from tbl_Test as d where e.WTTEST_TEST_id=d.TEST_Id ) as TEST_Name_var,7 as AlertId
        //                from tbl_Water_Inward  as a,tbl_Water_Test as e,tbl_Client as b,tbl_Site as c
        //                where a.WTINWD_CL_Id=b.CL_Id
        //                and a.WTINWD_SITE_Id=c.SITE_Id
        //                and a.WTINWD_ReferenceNo_var=e.WTTEST_ReferenceNo_var
        //                and a.WTINWD_OutwardDate_dt is null
        //                and Convert(Date,a.WTINWD_CollectionDate_dt) In (dateadd(dd,-((Select distinct(TEST_TimeFrame) from tbl_Test as d where Test_RecType_var='WT' and TEST_TimeFrame is NOT Null)+1),cast(getdate() as date)))
        //	            and CONVERT (DATE, a.WTINWD_CollectionDate_dt) >=CONVERT (DATE,'" + fromDate + "') and CONVERT (DATE,a.WTINWD_CollectionDate_dt) <=CONVERT (DATE,'" + ToDate + "') " +
        //                @"and datediff(d,convert(date,a.WTINWD_CollectionDate_dt),convert(date,GETDATE())) > 1
        //
        //			
        //             ";
        //                    }

        //                }
        //                else if (Alert_Id == 8)
        //                {
        //                    if (flagStatus == 0)
        //                    {
        //                        str = @"----Report not generated in 1 day after test completion
        //				                Select 0 as AlertDtl_EnqNo_int,null as AlertDtl_EnqDate_date,b.INWD_ReceivedDate_dt as AlertDtl_InwdRecDate_date,'' as AlertDtl_BillNo_int,null as AlertDtl_BillDate_date,0 as AlertDtl_BillAmt_dec,
        //                                '' as AlertDtl_ProNo_var,0 as AlertDtl_ProNo_int,null as AlertDtl_ProDate_date, MISRefNo as AlertDtl_RefNo_var,MISRecordNo as AlertDtl_RecNo_int,
        //                                c.CL_Name_var as AlertDtl_CL_Name_var,d.SITE_Name_var as AlertDtl_SITE_Name_var,
        //                                (select MATERIAL_Name_var from tbl_Material e where e.MATERIAL_RecordType_var=b.INWD_RecordType_var) as AlertDtl_RecType_var
        //				                ,c.CL_Id as AlertDtl_CLId_int,d.SITE_Id as AlertDtl_SiteId_int,8 as AlertDtl_Alert_Id,
        //                                (select USER_NAME_var from tbl_User x where x.USER_Id IN (select ROUTE_ME_Id from tbl_Route where ROUTE_Id=d.SITE_Route_Id)) as AlertDtl_MEName_var
        //                                ,null as AlertDtl_ProOrderDate_date,null as AlertDtl_BillModifiedDate_date,null as AlertDtl_CollctnEnqDate_date
        //                                ,null as AlertDtl_AppDate_date,null as AlertDtl_EnteredDate_date,null as AlertDtl_CheckedDate_date,null as AlertDtl_OutwrdDate_date,'' as AlertDtl_TestName_var
        //                                from tbl_Inward as b,tbl_Client as c,tbl_Site as d,tbl_MISDetail as f--,tbl_Material as e
        //                                where --a.ENQ_Id=b.INWD_ENQ_Id and
        //                                b.INWD_RecordNo_int=f.MISRecordNo
        //                                and b.INWD_RecordType_var=f.MISRecType
        //                                and b.INWD_CL_Id=c.CL_Id
        //                                and b.INWD_SITE_Id=d.SITE_Id
        //                                and CONVERT (DATE, b.INWD_ReceivedDate_dt) >=CONVERT (DATE,'" + fromDate + "') and CONVERT (DATE,b.INWD_ReceivedDate_dt) <=CONVERT (DATE,'" + ToDate + "') " +
        //                                @"and datediff(d,convert(date,b.INWD_ReceivedDate_dt),convert(date,f.MISEnteredDt)) > 1
        //				                and datediff(d,convert(date,b.INWD_ReceivedDate_dt),convert(date,GETDATE())) > 1
        //                                order by AlertDtl_RecType_var";
        //                    }
        //                    else
        //                    {
        //                        str = @"----Report not generated in 1 day after test completion
        //				                Select 0 as AlertDtl_EnqNo_int,null as AlertDtl_EnqDate_date,b.INWD_ReceivedDate_dt as AlertDtl_InwdRecDate_date,'' as AlertDtl_BillNo_int,null as AlertDtl_BillDate_date,0 as AlertDtl_BillAmt_dec,
        //                                '' as AlertDtl_ProNo_var,0 as AlertDtl_ProNo_int,null as AlertDtl_ProDate_date, MISRefNo as AlertDtl_RefNo_var,MISRecordNo as AlertDtl_RecNo_int,
        //                                c.CL_Name_var as AlertDtl_CL_Name_var,d.SITE_Name_var as AlertDtl_SITE_Name_var,
        //                                (select MATERIAL_Name_var from tbl_Material e where e.MATERIAL_RecordType_var=b.INWD_RecordType_var) as AlertDtl_RecType_var
        //				                ,c.CL_Id as AlertDtl_CLId_int,d.SITE_Id as AlertDtl_SiteId_int,8 as AlertDtl_Alert_Id,
        //                                (select USER_NAME_var from tbl_User x where x.USER_Id IN (select ROUTE_ME_Id from tbl_Route where ROUTE_Id=d.SITE_Route_Id)) as AlertDtl_MEName_var
        //                                ,null as AlertDtl_ProOrderDate_date,null as AlertDtl_BillModifiedDate_date,null as AlertDtl_CollctnEnqDate_date
        //                                ,null as AlertDtl_AppDate_date,null as AlertDtl_EnteredDate_date,null as AlertDtl_CheckedDate_date,null as AlertDtl_OutwrdDate_date,'' as AlertDtl_TestName_var
        //                                from tbl_Inward as b,tbl_Client as c,tbl_Site as d,tbl_MISDetail as f
        //                                where --a.ENQ_Id=b.INWD_ENQ_Id and --tbl_Enquiry as a,
        //                                b.INWD_RecordNo_int=f.MISRecordNo
        //                                and b.INWD_RecordType_var=f.MISRecType
        //                                and b.INWD_CL_Id=c.CL_Id
        //                                and b.INWD_SITE_Id=d.SITE_Id
        //                                and CONVERT (DATE, b.INWD_ReceivedDate_dt) >=CONVERT (DATE,'" + fromDate + "') and CONVERT (DATE,b.INWD_ReceivedDate_dt) <=CONVERT (DATE,'" + ToDate + "') " +
        //                                @" and f.MISEnteredDt is null 
        //                                and datediff(d,convert(date,b.INWD_ReceivedDate_dt),convert(date,GETDATE())) > 1
        //                                order by AlertDtl_RecType_var";

        //                    }
        //                }
        //                else if (Alert_Id == 9)
        //                {
        //                    if (flagStatus == 0)
        //                    {
        //                        str = @"----Report not checked in 1 day after report generation
        //				                Select  0 as AlertDtl_EnqNo_int,null as AlertDtl_EnqDate_date,f.MISEnteredDt as AlertDtl_EnteredDate_date,'' as AlertDtl_BillNo_int,null as AlertDtl_BillDate_date,0 as AlertDtl_BillAmt_dec,
        //                                '' as AlertDtl_ProNo_var,0 as AlertDtl_ProNo_int,null as AlertDtl_ProDate_date,f.MISRefNo as AlertDtl_RefNo_var,f.MISRecordNo as AlertDtl_RecNo_int,
        //                                c.CL_Name_var as AlertDtl_CL_Name_var,d.SITE_Name_var as AlertDtl_SITE_Name_var,
        //                                (select MATERIAL_Name_var from tbl_Material e where e.MATERIAL_RecordType_var=b.INWD_RecordType_var) as AlertDtl_RecType_var
        //				                ,c.CL_Id as AlertDtl_CLId_int,d.SITE_Id as AlertDtl_SiteId_int,9 as AlertDtl_Alert_Id,
        //                                (select USER_NAME_var from tbl_User x where x.USER_Id IN (select ROUTE_ME_Id from tbl_Route where ROUTE_Id=d.SITE_Route_Id)) as AlertDtl_MEName_var
        //                                ,null as AlertDtl_ProOrderDate_date,null as AlertDtl_BillModifiedDate_date,null as AlertDtl_CollctnEnqDate_date
        //                                ,null as AlertDtl_AppDate_date,null as AlertDtl_InwdRecDate_date,null as AlertDtl_CheckedDate_date,null as AlertDtl_OutwrdDate_date,'' as AlertDtl_TestName_var
        //                                from tbl_Inward as b,tbl_Client as c,tbl_Site as d,tbl_MISDetail as f
        //                                where --a.ENQ_Id=b.INWD_ENQ_Id and
        //                                b.INWD_RecordNo_int=f.MISRecordNo
        //                                and b.INWD_RecordType_var=f.MISRecType
        //                                and b.INWD_CL_Id=c.CL_Id
        //                                and b.INWD_SITE_Id=d.SITE_Id
        //                                and CONVERT (DATE, f.MISEnteredDt) >=CONVERT (DATE,'" + fromDate + "') and CONVERT (DATE,f.MISEnteredDt) <=CONVERT (DATE,'" + ToDate + "') " +
        //                                 @" and datediff(d,convert(date,f.MISEnteredDt),convert(date,f.MISCheckedDt)) > 1
        //				                and datediff(d,convert(date,f.MISEnteredDt),convert(date,GETDATE())) > 1
        //                                order by AlertDtl_RecType_var";
        //                    }
        //                    else
        //                    {
        //                        str = @"----Report not checked in 1 day after report generation
        //				                Select  0 as AlertDtl_EnqNo_int,null as AlertDtl_EnqDate_date,f.MISEnteredDt as AlertDtl_EnteredDate_date,'' as AlertDtl_BillNo_int,null as AlertDtl_BillDate_date,0 as AlertDtl_BillAmt_dec,
        //                                '' as AlertDtl_ProNo_var,0 as AlertDtl_ProNo_int,null as AlertDtl_ProDate_date,f.MISRefNo as AlertDtl_RefNo_var,f.MISRecordNo as AlertDtl_RecNo_int,
        //                                c.CL_Name_var as AlertDtl_CL_Name_var,d.SITE_Name_var as AlertDtl_SITE_Name_var,
        //                                 (select MATERIAL_Name_var from tbl_Material e where e.MATERIAL_RecordType_var=b.INWD_RecordType_var) as AlertDtl_RecType_var
        //				                ,c.CL_Id as AlertDtl_CLId_int,d.SITE_Id as AlertDtl_SiteId_int,9 as AlertDtl_Alert_Id,
        //                                (select USER_NAME_var from tbl_User x where x.USER_Id IN (select ROUTE_ME_Id from tbl_Route where ROUTE_Id=d.SITE_Route_Id)) as AlertDtl_MEName_var
        //                                ,null as AlertDtl_ProOrderDate_date,null as AlertDtl_BillModifiedDate_date,null as AlertDtl_CollctnEnqDate_date
        //                                ,null as AlertDtl_AppDate_date,null as AlertDtl_InwdRecDate_date,null as AlertDtl_CheckedDate_date,null as AlertDtl_OutwrdDate_date,'' as AlertDtl_TestName_var
        //                                from tbl_Inward as b,tbl_Client as c,tbl_Site as d,tbl_MISDetail as f
        //                                where  b.INWD_RecordNo_int=f.MISRecordNo
        //                                and b.INWD_RecordType_var=f.MISRecType
        //                                and b.INWD_CL_Id=c.CL_Id
        //                                and b.INWD_SITE_Id=d.SITE_Id
        //                                and CONVERT (DATE, f.MISEnteredDt) >=CONVERT (DATE,'" + fromDate + "') and CONVERT (DATE,f.MISEnteredDt) <=CONVERT (DATE,'" + ToDate + "') " +
        //                                @" and f.MISCheckedDt is null and datediff(d,convert(date,f.MISEnteredDt),convert(date,GETDATE())) > 1 
        //                                order by AlertDtl_RecType_var";

        //                    }
        //                }
        //                else if (Alert_Id == 10)
        //                {
        //                    if (flagStatus == 0)
        //                    {
        //                        str = @"----Report not approved in 1 day after report checked
        //				                Select  0 as AlertDtl_EnqNo_int,null as AlertDtl_EnqDate_date,f.MISCheckedDt as AlertDtl_CheckedDate_date,'' as AlertDtl_BillNo_int,null as AlertDtl_BillDate_date,0 as AlertDtl_BillAmt_dec,
        //                                '' as AlertDtl_ProNo_var,0 as AlertDtl_ProNo_int,null as AlertDtl_ProDate_date,f.MISRefNo as AlertDtl_RefNo_var,f.MISRecordNo as AlertDtl_RecNo_int,
        //                                c.CL_Name_var as AlertDtl_CL_Name_var,d.SITE_Name_var as AlertDtl_SITE_Name_var,
        //                                 (select MATERIAL_Name_var from tbl_Material e where e.MATERIAL_RecordType_var=b.INWD_RecordType_var) as AlertDtl_RecType_var
        //				                ,c.CL_Id as AlertDtl_CLId_int,d.SITE_Id as AlertDtl_SiteId_int,10 as AlertDtl_Alert_Id,
        //                                (select USER_NAME_var from tbl_User x where x.USER_Id IN (select ROUTE_ME_Id from tbl_Route where ROUTE_Id=d.SITE_Route_Id)) as AlertDtl_MEName_var
        //                                ,null as AlertDtl_ProOrderDate_date,null as AlertDtl_BillModifiedDate_date,null as AlertDtl_CollctnEnqDate_date
        //                                ,null as AlertDtl_AppDate_date,null as AlertDtl_InwdRecDate_date,null as AlertDtl_EnteredDate_date,null as AlertDtl_OutwrdDate_date,'' as AlertDtl_TestName_var
        //                                from tbl_Inward as b,tbl_Client as c,tbl_Site as d,tbl_MISDetail as f
        //                                where --a.ENQ_Id=b.INWD_ENQ_Id and
        //                                b.INWD_RecordNo_int=f.MISRecordNo
        //                                and b.INWD_RecordType_var=f.MISRecType
        //                                and b.INWD_CL_Id=c.CL_Id
        //                                and b.INWD_SITE_Id=d.SITE_Id
        //                                and CONVERT (DATE, f.MISCheckedDt) >=CONVERT (DATE,'" + fromDate + "') and CONVERT (DATE,f.MISCheckedDt) <=CONVERT (DATE,'" + ToDate + "') " +
        //                                @"and datediff(d,convert(date,f.MISCheckedDt),convert(date,f.MISApprovedDt)) > 1
        //				                and datediff(d,convert(date,f.MISCheckedDt),convert(date,GETDATE())) > 1 
        //                                order by AlertDtl_RecType_var";
        //                    }
        //                    else
        //                    {
        //                        str = @"----Report not approved in 1 day after report checked
        //				                Select  0 as AlertDtl_EnqNo_int,null as AlertDtl_EnqDate_date,f.MISEnteredDt as AlertDtl_EnteredDate_date,'' as AlertDtl_BillNo_int,null as AlertDtl_BillDate_date,0 as AlertDtl_BillAmt_dec,
        //                                '' as AlertDtl_ProNo_var,0 as AlertDtl_ProNo_int,null as AlertDtl_ProDate_date,f.MISRefNo as AlertDtl_RefNo_var,f.MISRecordNo as AlertDtl_RecNo_int,
        //                                c.CL_Name_var as AlertDtl_CL_Name_var,d.SITE_Name_var as AlertDtl_SITE_Name_var,
        //                                 (select MATERIAL_Name_var from tbl_Material e where e.MATERIAL_RecordType_var=b.INWD_RecordType_var) as AlertDtl_RecType_var
        //				                ,c.CL_Id as AlertDtl_CLId_int,d.SITE_Id as AlertDtl_SiteId_int,10 as AlertDtl_Alert_Id,
        //                                (select USER_NAME_var from tbl_User x where x.USER_Id IN (select ROUTE_ME_Id from tbl_Route where ROUTE_Id=d.SITE_Route_Id)) as AlertDtl_MEName_var
        //                                 ,null as AlertDtl_ProOrderDate_date,null as AlertDtl_BillModifiedDate_date,null as AlertDtl_CollctnEnqDate_date
        //                                ,null as AlertDtl_AppDate_date,null as AlertDtl_InwdRecDate_date,null as AlertDtl_EnteredDate_date,null as AlertDtl_OutwrdDate_date,'' as AlertDtl_TestName_var
        //                                from tbl_Inward as b,tbl_Client as c,tbl_Site as d,tbl_MISDetail as f
        //                                where --a.ENQ_Id=b.INWD_ENQ_Id and 
        //                                b.INWD_RecordNo_int=f.MISRecordNo
        //                                and b.INWD_RecordType_var=f.MISRecType
        //                                and b.INWD_CL_Id=c.CL_Id
        //                                and b.INWD_SITE_Id=d.SITE_Id
        //                                and CONVERT (DATE, f.MISCheckedDt) >=CONVERT (DATE,'" + fromDate + "') and CONVERT (DATE,f.MISCheckedDt) <=CONVERT (DATE,'" + ToDate + "') " +
        //                                @" and f.MISApprovedDt is null and datediff(d,convert(date,f.MISCheckedDt),convert(date,GETDATE())) > 1 
        //                                order by AlertDtl_RecType_var";

        //                    }
        //                }
        //                else if (Alert_Id == 12)
        //                {
        //                    if (flagStatus == 0)
        //                    {
        //                        str = @"----Reports not printed within 1 day after report approval
        //				                Select  0 as AlertDtl_EnqNo_int,null as AlertDtl_EnqDate_date,f.MISApprovedDt as AlertDtl_AppDate_date,'' as AlertDtl_BillNo_int,null as AlertDtl_BillDate_date,0 as AlertDtl_BillAmt_dec,
        //                                '' as AlertDtl_ProNo_var,0 as AlertDtl_ProNo_int,null as AlertDtl_ProDate_date,f.MISRefNo as AlertDtl_RefNo_var,f.MISRecordNo as AlertDtl_RecNo_int,
        //                                c.CL_Name_var as AlertDtl_CL_Name_var,d.SITE_Name_var as AlertDtl_SITE_Name_var,
        //                                 (select MATERIAL_Name_var from tbl_Material e where e.MATERIAL_RecordType_var=b.INWD_RecordType_var) as AlertDtl_RecType_var
        //				                ,c.CL_Id as AlertDtl_CLId_int,d.SITE_Id as AlertDtl_SiteId_int,12 as AlertDtl_Alert_Id,
        //                                (select USER_NAME_var from tbl_User x where x.USER_Id IN (select ROUTE_ME_Id from tbl_Route where ROUTE_Id=d.SITE_Route_Id)) as AlertDtl_MEName_var
        //                                ,null as AlertDtl_ProOrderDate_date,null as AlertDtl_BillModifiedDate_date,null as AlertDtl_CollctnEnqDate_date
        //                                ,null as AlertDtl_InwdRecDate_date,null as AlertDtl_EnteredDate_date,null as AlertDtl_CheckedDate_date,null as AlertDtl_OutwrdDate_date,'' as AlertDtl_TestName_var
        //                                from tbl_Inward as b,tbl_Client as c,tbl_Site as d,tbl_MISDetail as f
        //                                where --a.ENQ_Id=b.INWD_ENQ_Id and
        //                                 b.INWD_RecordNo_int=f.MISRecordNo
        //                                and b.INWD_RecordType_var=f.MISRecType
        //                                and b.INWD_CL_Id=c.CL_Id
        //                                and b.INWD_SITE_Id=d.SITE_Id
        //                                and CONVERT (DATE, f.MISApprovedDt) >=CONVERT (DATE,'" + fromDate + "') and CONVERT (DATE,f.MISApprovedDt) <=CONVERT (DATE,'" + ToDate + "') " +
        //                                 @" and datediff(d,convert(date,f.MISApprovedDt),convert(date,f.MISIssueDt)) > 1
        //				                and datediff(d,convert(date,f.MISApprovedDt),convert(date,GETDATE())) > 1 
        //                                order by AlertDtl_RecType_var";
        //                    }
        //                    else
        //                    {
        //                        str = @"----Reports not printed within 1 day after report approval
        //				                Select  0 as AlertDtl_EnqNo_int,null as AlertDtl_EnqDate_date,f.MISApprovedDt as AlertDtl_AppDate_date,'' as AlertDtl_BillNo_int,null as AlertDtl_BillDate_date,0 as AlertDtl_BillAmt_dec,
        //                                '' as AlertDtl_ProNo_var,0 as AlertDtl_ProNo_int,null as AlertDtl_ProDate_date,f.MISRefNo as AlertDtl_RefNo_var,f.MISRecordNo as AlertDtl_RecNo_int,
        //                                c.CL_Name_var as AlertDtl_CL_Name_var,d.SITE_Name_var as AlertDtl_SITE_Name_var,
        //                                 (select MATERIAL_Name_var from tbl_Material e where e.MATERIAL_RecordType_var=b.INWD_RecordType_var) as AlertDtl_RecType_var
        //				                ,c.CL_Id as AlertDtl_CLId_int,d.SITE_Id as AlertDtl_SiteId_int,12 as AlertDtl_Alert_Id,
        //                                (select USER_NAME_var from tbl_User x where x.USER_Id IN (select ROUTE_ME_Id from tbl_Route where ROUTE_Id=d.SITE_Route_Id)) as AlertDtl_MEName_var
        //                               ,null as AlertDtl_ProOrderDate_date,null as AlertDtl_BillModifiedDate_date,null as AlertDtl_CollctnEnqDate_date
        //                                ,null as AlertDtl_InwdRecDate_date,null as AlertDtl_EnteredDate_date,null as AlertDtl_CheckedDate_date,null as AlertDtl_OutwrdDate_date,'' as AlertDtl_TestName_var
        //                                 from tbl_Inward as b,tbl_Client as c,tbl_Site as d,tbl_MISDetail as f
        //                                where --a.ENQ_Id=b.INWD_ENQ_Id and
        //                                b.INWD_RecordNo_int=f.MISRecordNo
        //                                and b.INWD_RecordType_var=f.MISRecType
        //                                and b.INWD_CL_Id=c.CL_Id
        //                                and b.INWD_SITE_Id=d.SITE_Id
        //                                and CONVERT (DATE,f.MISApprovedDt) >=CONVERT (DATE,'" + fromDate + "') and CONVERT (DATE,f.MISApprovedDt) <=CONVERT (DATE,'" + ToDate + "') " +
        //                                @" and f.MISIssueDt is null 
        //                                and datediff(d,convert(date,f.MISApprovedDt),convert(date,GETDATE())) > 1
        //                                order by AlertDtl_RecType_var";

        //                    }
        //                }
        //                else if (Alert_Id == 13)
        //                {
        //                    if (flagStatus == 0)
        //                    {
        //                        str = @"----Reports not printed within 3 days after report approval
        //				                Select 0 as AlertDtl_EnqNo_int,null as AlertDtl_EnqDate_date,f.MISApprovedDt as AlertDtl_AppDate_date,'' as AlertDtl_BillNo_int,null as AlertDtl_BillDate_date,0 as AlertDtl_BillAmt_dec,
        //                                '' as AlertDtl_ProNo_var,0 as AlertDtl_ProNo_int,null as AlertDtl_ProDate_date,f.MISRefNo as AlertDtl_RefNo_var,f.MISRecordNo as AlertDtl_RecNo_int,
        //                                c.CL_Name_var as AlertDtl_CL_Name_var,d.SITE_Name_var as AlertDtl_SITE_Name_var,
        //                                 (select MATERIAL_Name_var from tbl_Material e where e.MATERIAL_RecordType_var=b.INWD_RecordType_var) as AlertDtl_RecType_var
        //				                ,c.CL_Id as AlertDtl_CLId_int,d.SITE_Id as AlertDtl_SiteId_int,13 as AlertDtl_Alert_Id,
        //                                (select USER_NAME_var from tbl_User x where x.USER_Id IN (select ROUTE_ME_Id from tbl_Route where ROUTE_Id=d.SITE_Route_Id)) as AlertDtl_MEName_var
        //                                ,null as AlertDtl_ProOrderDate_date,null as AlertDtl_BillModifiedDate_date,null as AlertDtl_CollctnEnqDate_date
        //                                ,null as AlertDtl_InwdRecDate_date,null as AlertDtl_EnteredDate_date,null as AlertDtl_CheckedDate_date,null as AlertDtl_OutwrdDate_date,'' as AlertDtl_TestName_var
        //                                from tbl_Inward as b,tbl_Client as c,tbl_Site as d,tbl_MISDetail as f
        //                                where  b.INWD_RecordNo_int=f.MISRecordNo
        //                                and b.INWD_RecordType_var=f.MISRecType
        //                                and b.INWD_CL_Id=c.CL_Id
        //                                and b.INWD_SITE_Id=d.SITE_Id
        //                                and CONVERT (DATE, f.MISApprovedDt) >=CONVERT (DATE,'" + fromDate + "') and CONVERT (DATE,f.MISApprovedDt) <=CONVERT (DATE,'" + ToDate + "') " +
        //                                @" and datediff(d,convert(date,f.MISApprovedDt),convert(date,f.MISIssueDt)) > 3
        //				                and datediff(d,convert(date,f.MISApprovedDt),convert(date,GETDATE())) > 3
        //                                order by AlertDtl_RecType_var";
        //                    }
        //                    else
        //                    {
        //                        str = @"----Reports not printed within 3 days after report approval
        //				                Select 0 as AlertDtl_EnqNo_int,null as AlertDtl_EnqDate_date,f.MISApprovedDt as AlertDtl_AppDate_date,'' as AlertDtl_BillNo_int,null as AlertDtl_BillDate_date,0 as AlertDtl_BillAmt_dec,
        //                                '' as AlertDtl_ProNo_var,0 as AlertDtl_ProNo_int,null as AlertDtl_ProDate_date,f.MISRefNo as AlertDtl_RefNo_var,f.MISRecordNo as AlertDtl_RecNo_int,
        //                                c.CL_Name_var as AlertDtl_CL_Name_var,d.SITE_Name_var as AlertDtl_SITE_Name_var,
        //                                (select MATERIAL_Name_var from tbl_Material e where e.MATERIAL_RecordType_var=b.INWD_RecordType_var) as AlertDtl_RecType_var
        //				                ,c.CL_Id as AlertDtl_CLId_int,d.SITE_Id as AlertDtl_SiteId_int,13 as AlertDtl_Alert_Id,
        //                                (select USER_NAME_var from tbl_User x where x.USER_Id IN (select ROUTE_ME_Id from tbl_Route where ROUTE_Id=d.SITE_Route_Id)) as AlertDtl_MEName_var
        //                                ,null as AlertDtl_ProOrderDate_date,null as AlertDtl_BillModifiedDate_date,null as AlertDtl_CollctnEnqDate_date
        //                                ,null as AlertDtl_InwdRecDate_date,null as AlertDtl_EnteredDate_date,null as AlertDtl_CheckedDate_date,null as AlertDtl_OutwrdDate_date,'' as AlertDtl_TestName_var
        //                                from tbl_Inward as b,tbl_Client as c,tbl_Site as d,tbl_MISDetail as f
        //                                where  b.INWD_RecordNo_int=f.MISRecordNo
        //                                and b.INWD_RecordType_var=f.MISRecType
        //                                and b.INWD_CL_Id=c.CL_Id
        //                                and b.INWD_SITE_Id=d.SITE_Id
        //                                and CONVERT (DATE, f.MISApprovedDt) >=CONVERT (DATE,'" + fromDate + "') and CONVERT (DATE,f.MISApprovedDt) <=CONVERT (DATE,'" + ToDate + "') " +
        //                                @"and f.MISIssueDt is null 
        //                                and datediff(d,convert(date,f.MISApprovedDt),convert(date,GETDATE())) > 3 
        //                                order by AlertDtl_RecType_var";

        //                    }
        //                }
        //                else if (Alert_Id == 14)
        //                {
        //                    if (flagStatus == 0)
        //                    {
        //                        str = @"----Reports not printed for more than 3 days after report approval
        //				                Select 0 as AlertDtl_EnqNo_int,null as AlertDtl_EnqDate_date,f.MISApprovedDt as AlertDtl_AppDate_date,'' as AlertDtl_BillNo_int,null as AlertDtl_BillDate_date,0 as AlertDtl_BillAmt_dec,
        //                                '' as AlertDtl_ProNo_var,0 as AlertDtl_ProNo_int,null as AlertDtl_ProDate_date,f.MISRefNo as AlertDtl_RefNo_var,f.MISRecordNo as AlertDtl_RecNo_int,
        //                                c.CL_Name_var as AlertDtl_CL_Name_var,d.SITE_Name_var as AlertDtl_SITE_Name_var, 
        //                                (select MATERIAL_Name_var from tbl_Material e where e.MATERIAL_RecordType_var=b.INWD_RecordType_var) as AlertDtl_RecType_var
        //				                ,c.CL_Id as AlertDtl_CLId_int,d.SITE_Id as AlertDtl_SiteId_int,14 as AlertDtl_Alert_Id,
        //                                (select USER_NAME_var from tbl_User x where x.USER_Id IN (select ROUTE_ME_Id from tbl_Route where ROUTE_Id=d.SITE_Route_Id)) as AlertDtl_MEName_var
        //                                ,null as AlertDtl_ProOrderDate_date,null as AlertDtl_BillModifiedDate_date,null as AlertDtl_CollctnEnqDate_date
        //                                ,null as AlertDtl_InwdRecDate_date,null as AlertDtl_EnteredDate_date,null as AlertDtl_CheckedDate_date,null as AlertDtl_OutwrdDate_date,'' as AlertDtl_TestName_var
        //                                from tbl_Inward as b,tbl_Client as c,tbl_Site as d,tbl_MISDetail as f
        //                                where  b.INWD_RecordNo_int=f.MISRecordNo
        //                                and b.INWD_RecordType_var=f.MISRecType
        //                                and b.INWD_CL_Id=c.CL_Id
        //                                and b.INWD_SITE_Id=d.SITE_Id
        //                                and CONVERT (DATE, f.MISApprovedDt) >=CONVERT (DATE,'" + fromDate + "') and CONVERT (DATE,f.MISApprovedDt) <=CONVERT (DATE,'" + ToDate + "') " +
        //                                @" and datediff(d,convert(date,f.MISApprovedDt),convert(date,f.MISIssueDt)) > 4
        //				                and datediff(d,convert(date,f.MISApprovedDt),convert(date,GETDATE())) > 4
        //                                order by AlertDtl_RecType_var";
        //                    }
        //                    else
        //                    {
        //                        str = @"----Reports not printed for more than 3 days after report approval
        //				                Select 0 as AlertDtl_EnqNo_int,null as AlertDtl_EnqDate_date,f.MISApprovedDt as AlertDtl_AppDate_date,'' as AlertDtl_BillNo_int,null as AlertDtl_BillDate_date,0 as AlertDtl_BillAmt_dec,
        //                                '' as AlertDtl_ProNo_var,0 as AlertDtl_ProNo_int,null as AlertDtl_ProDate_date,f.MISRefNo as AlertDtl_RefNo_var,f.MISRecordNo as AlertDtl_RecNo_int,
        //                                c.CL_Name_var as AlertDtl_CL_Name_var,d.SITE_Name_var as AlertDtl_SITE_Name_var,
        //                                (select MATERIAL_Name_var from tbl_Material e where e.MATERIAL_RecordType_var=b.INWD_RecordType_var) as AlertDtl_RecType_var
        //				                ,c.CL_Id as AlertDtl_CLId_int,d.SITE_Id as AlertDtl_SiteId_int,14 as AlertDtl_Alert_Id,
        //                                (select USER_NAME_var from tbl_User x where x.USER_Id IN (select ROUTE_ME_Id from tbl_Route where ROUTE_Id=d.SITE_Route_Id)) as AlertDtl_MEName_var
        //                                ,null as AlertDtl_ProOrderDate_date,null as AlertDtl_BillModifiedDate_date,null as AlertDtl_CollctnEnqDate_date,null as AlertDtl_OutwrdDate_date
        //                                ,null as AlertDtl_InwdRecDate_date,null as AlertDtl_EnteredDate_date,null as AlertDtl_CheckedDate_date,'' as AlertDtl_TestName_var
        //                                from tbl_Inward as b,tbl_Client as c,tbl_Site as d,tbl_MISDetail as f
        //                                where  b.INWD_RecordNo_int=f.MISRecordNo
        //                                 and b.INWD_RecordType_var=f.MISRecType
        //                                and b.INWD_CL_Id=c.CL_Id
        //                                and b.INWD_SITE_Id=d.SITE_Id
        //                                and CONVERT (DATE, f.MISApprovedDt) >=CONVERT (DATE,'" + fromDate + "') and CONVERT (DATE,f.MISApprovedDt) <=CONVERT (DATE,'" + ToDate + "') " +
        //                                @" and f.MISIssueDt is null 
        //                                and datediff(d,convert(date,f.MISApprovedDt),convert(date,GETDATE())) > 4
        //                                order by AlertDtl_RecType_var";

        //                    }
        //                }
        //                else if (Alert_Id == 15)
        //                {
        //                    if (flagStatus == 0)
        //                    {
        //                        str = @"---- Bills modified weekly
        //				                Select 0 as AlertDtl_EnqNo_int,null as AlertDtl_EnqDate_date,a.BILL_Id as AlertDtl_BillNo_int,a.BILL_Date_dt as AlertDtl_BillDate_date,a.BILL_NetAmt_num as AlertDtl_BillAmt_dec,
        //                                '' as AlertDtl_ProNo_var,0 as AlertDtl_ProNo_int,null as AlertDtl_ProDate_date,'' as AlertDtl_RefNo_var,0 as AlertDtl_RecNo_int,
        //                                c.CL_Name_var as AlertDtl_CL_Name_var,d.SITE_Name_var as AlertDtl_SITE_Name_var,(select MATERIAL_Name_var from tbl_Material e where e.MATERIAL_RecordType_var=a.BILL_RecordType_var)  as AlertDtl_RecType_var,
        //                                c.CL_Id as AlertDtl_CLId_int,d.SITE_Id as AlertDtl_SiteId_int,15 as AlertDtl_Alert_Id,
        //                                (select USER_NAME_var from tbl_User x where x.USER_Id IN (select ROUTE_ME_Id from tbl_Route where ROUTE_Id=d.SITE_Route_Id)) as AlertDtl_MEName_var
        //                                ,null as AlertDtl_ProOrderDate_date,null as AlertDtl_BillModifiedDate_date,null as AlertDtl_CollctnEnqDate_date
        //                                ,null as AlertDtl_InwdRecDate_date,null as AlertDtl_EnteredDate_date,null as AlertDtl_CheckedDate_date,null as AlertDtl_AppDate_date,null as AlertDtl_OutwrdDate_date,'' as AlertDtl_TestName_var
        //                                from tbl_Client as c,tbl_Site as d,tbl_Bill as a
        //                                where  a.BILL_CL_Id=c.CL_Id 
        //                                and a.Bill_SITE_Id=d.SITE_Id
        //                                 and CONVERT (DATE, a.BILL_Date_dt) >=CONVERT (DATE,'" + fromDate + "') and CONVERT (DATE,a.BILL_Date_dt) <=CONVERT (DATE,'" + ToDate + "') " +
        //                                 @" and datediff(d,convert(date,a.BILL_Date_dt),convert(date,a.BILL_ModifiedOn_dt)) > 7
        //				                and datediff(d,convert(date,a.BILL_Date_dt),convert(date,GETDATE())) > 7
        //                              ";
        //                    }
        //                    else
        //                    {
        //                        str = @"---- Bills modified weekly
        //				                Select 0 as AlertDtl_EnqNo_int,null as AlertDtl_EnqDate_date,a.BILL_Id as AlertDtl_BillNo_int,a.BILL_Date_dt as AlertDtl_BillDate_date,a.BILL_NetAmt_num as AlertDtl_BillAmt_dec,
        //                                '' as AlertDtl_ProNo_var,0 as AlertDtl_ProNo_int,null as AlertDtl_ProDate_date,'' as AlertDtl_RefNo_var,0 as AlertDtl_RecNo_int,
        //                                c.CL_Name_var as AlertDtl_CL_Name_var,d.SITE_Name_var as AlertDtl_SITE_Name_var,(select MATERIAL_Name_var from tbl_Material e where e.MATERIAL_RecordType_var=a.BILL_RecordType_var)  as AlertDtl_RecType_var,
        //                                c.CL_Id as AlertDtl_CLId_int,d.SITE_Id as AlertDtl_SiteId_int,15 as AlertDtl_Alert_Id,
        //                                (select USER_NAME_var from tbl_User x where x.USER_Id IN (select ROUTE_ME_Id from tbl_Route where ROUTE_Id=d.SITE_Route_Id)) as AlertDtl_MEName_var
        //                                ,null as AlertDtl_ProOrderDate_date,null as AlertDtl_BillModifiedDate_date,null as AlertDtl_CollctnEnqDate_date
        //                                ,null as AlertDtl_InwdRecDate_date,null as AlertDtl_EnteredDate_date,null as AlertDtl_CheckedDate_date,null as AlertDtl_AppDate_date,null as AlertDtl_OutwrdDate_date,'' as AlertDtl_TestName_var
        //                                from tbl_Client as c,tbl_Site as d,tbl_Bill as a
        //                                where  a.BILL_CL_Id=c.CL_Id 
        //                                and a.Bill_SITE_Id=d.SITE_Id
        //                                and CONVERT (DATE, a.BILL_Date_dt) >=CONVERT (DATE,'" + fromDate + "') and CONVERT (DATE,a.BILL_Date_dt) <=CONVERT (DATE,'" + ToDate + "') " +
        //                                @" and a.BILL_ModifiedOn_dt is null 
        //                                and datediff(d,convert(date,a.BILL_Date_dt),convert(date,GETDATE())) > 7
        //                               ";

        //                    }
        //                }
        //                else if (Alert_Id == 16)
        //                {
        //                    if (flagStatus == 0)
        //                    {
        //                        str = @"---- Bills modified in more than 2 days after generation
        //				                Select 0 as AlertDtl_EnqNo_int,null as AlertDtl_EnqDate_date,a.BILL_ModifiedOn_dt as AlertDtl_BillModifiedDate_date,a.BILL_Id as AlertDtl_BillNo_int,a.BILL_Date_dt as AlertDtl_BillDate_date,a.BILL_NetAmt_num as AlertDtl_BillAmt_dec,
        //                                '' as AlertDtl_ProNo_var,0 as AlertDtl_ProNo_int,null as AlertDtl_ProDate_date,'' as AlertDtl_RefNo_var,0 as AlertDtl_RecNo_int,
        //                                c.CL_Name_var as AlertDtl_CL_Name_var,d.SITE_Name_var as AlertDtl_SITE_Name_var,(select MATERIAL_Name_var from tbl_Material e where e.MATERIAL_RecordType_var=a.BILL_RecordType_var)  as AlertDtl_RecType_var,
        //                                c.CL_Id as AlertDtl_CLId_int,d.SITE_Id as AlertDtl_SiteId_int,16 as AlertDtl_Alert_Id,
        //                                (select USER_NAME_var from tbl_User x where x.USER_Id IN (select ROUTE_ME_Id from tbl_Route where ROUTE_Id=d.SITE_Route_Id)) as AlertDtl_MEName_var
        //                                ,null as AlertDtl_ProOrderDate_date,null as AlertDtl_BillModifiedDate_date,null as AlertDtl_CollctnEnqDate_date
        //                                ,null as AlertDtl_InwdRecDate_date,null as AlertDtl_EnteredDate_date,null as AlertDtl_CheckedDate_date,null as AlertDtl_AppDate_date,null as AlertDtl_OutwrdDate_date,'' as AlertDtl_TestName_var
        //                                from tbl_Client as c,tbl_Site as d,tbl_Bill as a
        //                                where  a.BILL_CL_Id=c.CL_Id 
        //                                and a.Bill_SITE_Id=d.SITE_Id
        //                                 and CONVERT (DATE, a.BILL_Date_dt) >=CONVERT (DATE,'" + fromDate + "') and CONVERT (DATE,a.BILL_Date_dt) <=CONVERT (DATE,'" + ToDate + "') " +
        //                                 @" and datediff(d,convert(date,a.BILL_Date_dt),convert(date,a.BILL_ModifiedOn_dt)) > 2
        //				                and datediff(d,convert(date,a.BILL_Date_dt),convert(date,GETDATE())) > 2
        //                                ";
        //                    }
        //                    else
        //                    {
        //                        str = @"---- Bills modified in more than 2 days after generation
        //				                Select 0 as AlertDtl_EnqNo_int,null as AlertDtl_EnqDate_date,a.BILL_Id as AlertDtl_BillNo_int,a.BILL_Date_dt as AlertDtl_BillDate_date,a.BILL_NetAmt_num as AlertDtl_BillAmt_dec,
        //                                '' as AlertDtl_ProNo_var,0 as AlertDtl_ProNo_int,null as AlertDtl_ProDate_date,'' as AlertDtl_RefNo_var,0 as AlertDtl_RecNo_int,
        //                                c.CL_Name_var as AlertDtl_CL_Name_var,d.SITE_Name_var as AlertDtl_SITE_Name_var,(select MATERIAL_Name_var from tbl_Material e where e.MATERIAL_RecordType_var=a.BILL_RecordType_var)  as AlertDtl_RecType_var,
        //                                c.CL_Id as AlertDtl_CLId_int,d.SITE_Id as AlertDtl_SiteId_int,16 as AlertDtl_Alert_Id,
        //                                (select USER_NAME_var from tbl_User x where x.USER_Id IN (select ROUTE_ME_Id from tbl_Route where ROUTE_Id=d.SITE_Route_Id)) as AlertDtl_MEName_var
        //                                ,null as AlertDtl_ProOrderDate_date,null as AlertDtl_BillModifiedDate_date,null as AlertDtl_CollctnEnqDate_date
        //                                ,null as AlertDtl_InwdRecDate_date,null as AlertDtl_EnteredDate_date,null as AlertDtl_CheckedDate_date,null as AlertDtl_AppDate_date,null as AlertDtl_OutwrdDate_date,'' as AlertDtl_TestName_var
        //                                from tbl_Client as c,tbl_Site as d,tbl_Bill as a
        //                                where  a.BILL_CL_Id=c.CL_Id 
        //                                and a.Bill_SITE_Id=d.SITE_Id
        //                                and CONVERT (DATE, a.BILL_Date_dt) >=CONVERT (DATE,'" + fromDate + "') and CONVERT (DATE,a.BILL_Date_dt) <=CONVERT (DATE,'" + ToDate + "') " +
        //                                @" and a.BILL_ModifiedOn_dt is null and datediff(d,convert(date,a.BILL_Date_dt),convert(date,GETDATE())) > 2
        //                               ";

        //                    }
        //                }
        //                else if (Alert_Id == 18)
        //                {
        //                    if (flagStatus == 0)
        //                    {
        //                        str = @"---- Bills not sent in 3 days after bill generation 
        //				                Select 0 as AlertDtl_EnqNo_int,null as AlertDtl_EnqDate_date,a.BILL_Id as AlertDtl_BillNo_int,a.BILL_Date_dt as AlertDtl_BillDate_date,a.BILL_NetAmt_num as AlertDtl_BillAmt_dec,
        //                                '' as AlertDtl_ProNo_var,0 as AlertDtl_ProNo_int,null as AlertDtl_ProDate_date,'' as AlertDtl_RefNo_var,0 as AlertDtl_RecNo_int,
        //                                c.CL_Name_var as AlertDtl_CL_Name_var,d.SITE_Name_var as AlertDtl_SITE_Name_var,(select MATERIAL_Name_var from tbl_Material e where e.MATERIAL_RecordType_var=a.BILL_RecordType_var)  as AlertDtl_RecType_var,
        //                                c.CL_Id as AlertDtl_CLId_int,d.SITE_Id as AlertDtl_SiteId_int,18 as AlertDtl_Alert_Id,
        //                                (select USER_NAME_var from tbl_User x where x.USER_Id IN (select ROUTE_ME_Id from tbl_Route where ROUTE_Id=d.SITE_Route_Id)) as AlertDtl_MEName_var
        //                                ,null as AlertDtl_ProOrderDate_date,null as AlertDtl_BillModifiedDate_date,null as AlertDtl_CollctnEnqDate_date
        //                                ,null as AlertDtl_InwdRecDate_date,null as AlertDtl_EnteredDate_date,null as AlertDtl_CheckedDate_date,null as AlertDtl_AppDate_date,null as AlertDtl_OutwrdDate_date,'' as AlertDtl_TestName_var
        //                                from tbl_Client as c,tbl_Site as d,tbl_Bill as a,tbl_Outward b
        //                                where  a.BILL_CL_Id=c.CL_Id 
        //                                and a.BILL_Id = b.OUTW_ReferenceNo_var And b.OUTW_RecordType_var ='DT'
        //                                and a.Bill_SITE_Id=d.SITE_Id
        //                                 and CONVERT (DATE, a.BILL_Date_dt) >=CONVERT (DATE,'" + fromDate + "') and CONVERT (DATE,a.BILL_Date_dt) <=CONVERT (DATE,'" + ToDate + "') " +
        //                                 @" and datediff(d,convert(date,a.BILL_Date_dt),convert(date,b.OUTW_OutwardDate_dt)) > 3
        //				                and datediff(d,convert(date,a.BILL_Date_dt),convert(date,GETDATE())) > 3
        //                                ";
        //                    }
        //                    else
        //                    {
        //                        str = @"----Bills not sent in 3 days after bill generation 
        //				                Select 0 as AlertDtl_EnqNo_int,null as AlertDtl_EnqDate_date,a.BILL_Id as AlertDtl_BillNo_int,a.BILL_Date_dt as AlertDtl_BillDate_date,a.BILL_NetAmt_num as AlertDtl_BillAmt_dec,
        //                                '' as AlertDtl_ProNo_var,0 as AlertDtl_ProNo_int,null as AlertDtl_ProDate_date,'' as AlertDtl_RefNo_var,0 as AlertDtl_RecNo_int,
        //                                c.CL_Name_var as AlertDtl_CL_Name_var,d.SITE_Name_var as AlertDtl_SITE_Name_var,(select MATERIAL_Name_var from tbl_Material e where e.MATERIAL_RecordType_var=a.BILL_RecordType_var)  as AlertDtl_RecType_var,
        //                                c.CL_Id as AlertDtl_CLId_int,d.SITE_Id as AlertDtl_SiteId_int,18 as AlertDtl_Alert_Id,
        //                                (select USER_NAME_var from tbl_User x where x.USER_Id IN (select ROUTE_ME_Id from tbl_Route where ROUTE_Id=d.SITE_Route_Id)) as AlertDtl_MEName_var
        //                                ,null as AlertDtl_ProOrderDate_date,null as AlertDtl_BillModifiedDate_date,null as AlertDtl_CollctnEnqDate_date
        //                                ,null as AlertDtl_InwdRecDate_date,null as AlertDtl_EnteredDate_date,null as AlertDtl_CheckedDate_date,null as AlertDtl_AppDate_date,null as AlertDtl_OutwrdDate_date,'' as AlertDtl_TestName_var
        //                                from tbl_Client as c,tbl_Site as d,tbl_Bill as a
        //                                where  a.BILL_CL_Id=c.CL_Id 
        //                                and a.Bill_SITE_Id=d.SITE_Id
        //                                and CONVERT (DATE, a.BILL_Date_dt) >=CONVERT (DATE,'" + fromDate + "') and CONVERT (DATE,a.BILL_Date_dt) <=CONVERT (DATE,'" + ToDate + "') " +
        //                                @" and a.BILL_Id Not In( select OUTW_ReferenceNo_var from tbl_Outward b where a.BILL_Id = b.OUTW_ReferenceNo_var And b.OUTW_RecordType_var ='DT')
        //                                and datediff(d,convert(date,a.BILL_Date_dt),convert(date,GETDATE())) > 2
        //                               ";

        //                    }
        //                }
        //                else if (Alert_Id == 19)
        //                {
        //                    if (flagStatus == 0)
        //                    {
        //                        str = @"---- Payment not received in 60 days after bill received by the client
        //                                Select 0 as AlertDtl_EnqNo_int,null as AlertDtl_EnqDate_date,a.BILL_Id as AlertDtl_BillNo_int,a.BILL_Date_dt as AlertDtl_BillDate_date,a.BILL_NetAmt_num as AlertDtl_BillAmt_dec,
        //                                '' as AlertDtl_ProNo_var,0 as AlertDtl_ProNo_int,null as AlertDtl_ProDate_date,'' as AlertDtl_RefNo_var,0 as AlertDtl_RecNo_int,
        //                                c.CL_Name_var as AlertDtl_CL_Name_var,d.SITE_Name_var as AlertDtl_SITE_Name_var,
        //                                (select MATERIAL_Name_var from tbl_Material e where e.MATERIAL_RecordType_var=a.BILL_RecordType_var)  as AlertDtl_RecType_var,
        //                                c.CL_Id as AlertDtl_CLId_int,d.SITE_Id as AlertDtl_SiteId_int,19 as AlertDtl_Alert_Id,
        //                                (select USER_NAME_var from tbl_User x where x.USER_Id IN (select ROUTE_ME_Id from tbl_Route where ROUTE_Id=d.SITE_Route_Id)) as AlertDtl_MEName_var
        //                                ,null as AlertDtl_ProOrderDate_date,null as AlertDtl_BillModifiedDate_date,null as AlertDtl_CollctnEnqDate_date
        //                                ,null as AlertDtl_InwdRecDate_date,null as AlertDtl_EnteredDate_date,null as AlertDtl_CheckedDate_date,null as AlertDtl_AppDate_date,b.OUTW_OutwardDate_dt as AlertDtl_OutwrdDate_date,'' as AlertDtl_TestName_var
        //                                from tbl_Bill as a,tbl_Client as c,tbl_Site as d,tbl_Outward b,tbl_cashDetail e
        //				                where  a.BILL_CL_Id=c.CL_Id 
        //				                and a.BILL_Id = b.OUTW_ReferenceNo_var  and b.OUTW_RecordType_var ='DT'
        //				                and a.Bill_SITE_Id=d.SITE_Id
        //				                and a.BILL_OutwardStatus_bit =1
        //				                and a.BILL_Id=e.CashDetail_BillNo_int
        //                                and CONVERT (DATE, b.OUTW_OutwardDate_dt) >=CONVERT (DATE,'" + fromDate + "') and CONVERT (DATE,b.OUTW_OutwardDate_dt) <=CONVERT (DATE,'" + ToDate + "') " +
        //                                @" and datediff(d,convert(date,b.OUTW_OutwardDate_dt),convert(date,GETDATE())) > 61
        //				                and datediff(d,convert(date,b.OUTW_OutwardDate_dt),convert(date,e.CashDetail_Date_date)) > 61         
        //                                and a.Bill_Status_bit=0
        //                                and (a.BILL_NetAmt_num + (select Sum(b.CashDetail_Amount_money) from tbl_cashDetail b
        //                                where b.CashDetail_BillNo_int=a.BILL_Id and b.CashDetail_Status_bit=0))>0
        //                                order by AlertDtl_RecType_var";
        //                    }
        //                    else
        //                    {
        //                        str = @"---- Payment not received in 60 days after bill received by the client
        //                                Select 0 as AlertDtl_EnqNo_int,null as AlertDtl_EnqDate_date,a.BILL_Id as AlertDtl_BillNo_int,a.BILL_Date_dt as AlertDtl_BillDate_date,a.BILL_NetAmt_num as AlertDtl_BillAmt_dec,
        //                                '' as AlertDtl_ProNo_var,0 as AlertDtl_ProNo_int,null as AlertDtl_ProDate_date,'' as AlertDtl_RefNo_var,0 as AlertDtl_RecNo_int,
        //                                c.CL_Name_var as AlertDtl_CL_Name_var,d.SITE_Name_var as AlertDtl_SITE_Name_var,(select MATERIAL_Name_var from tbl_Material e where e.MATERIAL_RecordType_var=a.BILL_RecordType_var)  as AlertDtl_RecType_var,
        //                                c.CL_Id as AlertDtl_CLId_int,d.SITE_Id as AlertDtl_SiteId_int,19 as AlertDtl_Alert_Id,
        //                                (select USER_NAME_var from tbl_User x where x.USER_Id IN (select ROUTE_ME_Id from tbl_Route where ROUTE_Id=d.SITE_Route_Id)) as AlertDtl_MEName_var
        //                                ,null as AlertDtl_ProOrderDate_date,null as AlertDtl_BillModifiedDate_date,null as AlertDtl_CollctnEnqDate_date
        //                                ,null as AlertDtl_InwdRecDate_date,null as AlertDtl_EnteredDate_date,null as AlertDtl_CheckedDate_date,null as AlertDtl_AppDate_date,b.OUTW_OutwardDate_dt as AlertDtl_OutwrdDate_date,'' as AlertDtl_TestName_var
        //                                from tbl_Bill as a,tbl_Client as c,tbl_Site as d,tbl_Outward b
        //				                where  a.BILL_CL_Id=c.CL_Id 
        //				                and a.BILL_Id = b.OUTW_ReferenceNo_var  and b.OUTW_RecordType_var ='DT'
        //				                and a.Bill_SITE_Id=d.SITE_Id
        //				                and a.BILL_OutwardStatus_bit =1
        //				                and CONVERT (DATE,b.OUTW_OutwardDate_dt) >=CONVERT (DATE,'" + fromDate + "') and CONVERT (DATE,b.OUTW_OutwardDate_dt) <=CONVERT (DATE,'" + ToDate + "') " +
        //                                @" and a.Bill_id not in  (select CashDetail_BillNo_int from tbl_CashDetail where CashDetail_Status_bit=0)
        //				                and datediff(d,convert(date,b.OUTW_OutwardDate_dt),convert(date,GETDATE())) > 61 
        //                                and a.Bill_Status_bit=0
        //                                order by AlertDtl_RecType_var";
        //                    }
        //                }
        //                else if (Alert_Id == 20)
        //                {
        //                    if (flagStatus == 0)
        //                    {
        //                        str = @"---- Payment not received in 90 days after bill received by the client
        //                                 Select 0 as AlertDtl_EnqNo_int,null as AlertDtl_EnqDate_date,a.BILL_Id as AlertDtl_BillNo_int,a.BILL_Date_dt as AlertDtl_BillDate_date,a.BILL_NetAmt_num as AlertDtl_BillAmt_dec,
        //                                '' as AlertDtl_ProNo_var,0 as AlertDtl_ProNo_int,null as AlertDtl_ProDate_date,'' as AlertDtl_RefNo_var,0 as AlertDtl_RecNo_int,
        //                                c.CL_Name_var as AlertDtl_CL_Name_var,d.SITE_Name_var as AlertDtl_SITE_Name_var,
        //                                (select MATERIAL_Name_var from tbl_Material e where e.MATERIAL_RecordType_var=a.BILL_RecordType_var)  as AlertDtl_RecType_var,
        //                                c.CL_Id as AlertDtl_CLId_int,d.SITE_Id as AlertDtl_SiteId_int,20 as AlertDtl_Alert_Id,
        //                                (select USER_NAME_var from tbl_User x where x.USER_Id IN (select ROUTE_ME_Id from tbl_Route where ROUTE_Id=d.SITE_Route_Id)) as AlertDtl_MEName_var
        //                                ,null as AlertDtl_ProOrderDate_date,null as AlertDtl_BillModifiedDate_date,null as AlertDtl_CollctnEnqDate_date
        //                                ,null as AlertDtl_InwdRecDate_date,null as AlertDtl_EnteredDate_date,null as AlertDtl_CheckedDate_date,null as AlertDtl_AppDate_date,b.OUTW_OutwardDate_dt as AlertDtl_OutwrdDate_date,'' as AlertDtl_TestName_var
        //                                from tbl_Bill as a,tbl_Client as c,tbl_Site as d,tbl_Outward b,tbl_cashDetail e
        //				                where  a.BILL_CL_Id=c.CL_Id 
        //				                and a.BILL_Id = b.OUTW_ReferenceNo_var  and b.OUTW_RecordType_var ='DT'
        //				                and a.Bill_SITE_Id=d.SITE_Id
        //				                and a.BILL_OutwardStatus_bit =1
        //				                and a.BILL_Id=e.CashDetail_BillNo_int
        //                                and CONVERT (DATE, b.OUTW_OutwardDate_dt) >=CONVERT (DATE,'" + fromDate + "') and CONVERT (DATE,b.OUTW_OutwardDate_dt) <=CONVERT (DATE,'" + ToDate + "') " +
        //                                @" and datediff(d,convert(date,b.OUTW_OutwardDate_dt),convert(date,GETDATE())) > 91
        //				                and datediff(d,convert(date,b.OUTW_OutwardDate_dt),convert(date,e.CashDetail_Date_date)) > 91         
        //                                and a.Bill_Status_bit=0
        //                                and (a.BILL_NetAmt_num + (select Sum(b.CashDetail_Amount_money) from tbl_cashDetail b
        //                                where b.CashDetail_BillNo_int=a.BILL_Id and b.CashDetail_Status_bit=0))>0
        //                                order by AlertDtl_RecType_var";
        //                    }
        //                    else
        //                    {
        //                        str = @"---- Payment not received in 90 days after bill received by the client
        //                                 Select 0 as AlertDtl_EnqNo_int,null as AlertDtl_EnqDate_date,a.BILL_Id as AlertDtl_BillNo_int,a.BILL_Date_dt as AlertDtl_BillDate_date,a.BILL_NetAmt_num as AlertDtl_BillAmt_dec,
        //                                '' as AlertDtl_ProNo_var,0 as AlertDtl_ProNo_int,null as AlertDtl_ProDate_date,'' as AlertDtl_RefNo_var,0 as AlertDtl_RecNo_int,
        //                                c.CL_Name_var as AlertDtl_CL_Name_var,d.SITE_Name_var as AlertDtl_SITE_Name_var,(select MATERIAL_Name_var from tbl_Material e where e.MATERIAL_RecordType_var=a.BILL_RecordType_var)  as AlertDtl_RecType_var,
        //                                c.CL_Id as AlertDtl_CLId_int,d.SITE_Id as AlertDtl_SiteId_int,20 as AlertDtl_Alert_Id,
        //                                (select USER_NAME_var from tbl_User x where x.USER_Id IN (select ROUTE_ME_Id from tbl_Route where ROUTE_Id=d.SITE_Route_Id)) as AlertDtl_MEName_var
        //                                ,null as AlertDtl_ProOrderDate_date,null as AlertDtl_BillModifiedDate_date,null as AlertDtl_CollctnEnqDate_date
        //                                ,null as AlertDtl_InwdRecDate_date,null as AlertDtl_EnteredDate_date,null as AlertDtl_CheckedDate_date,null as AlertDtl_AppDate_date,b.OUTW_OutwardDate_dt as AlertDtl_OutwrdDate_date,'' as AlertDtl_TestName_var
        //                                from tbl_Bill as a,tbl_Client as c,tbl_Site as d,tbl_Outward b
        //				                where  a.BILL_CL_Id=c.CL_Id 
        //				                and a.BILL_Id = b.OUTW_ReferenceNo_var  and b.OUTW_RecordType_var ='DT'
        //				                and a.Bill_SITE_Id=d.SITE_Id
        //				                and a.BILL_OutwardStatus_bit =1
        //				                and CONVERT (DATE,b.OUTW_OutwardDate_dt) >=CONVERT (DATE,'" + fromDate + "') and CONVERT (DATE,b.OUTW_OutwardDate_dt) <=CONVERT (DATE,'" + ToDate + "') " +
        //                                @" and a.Bill_id not in  (select CashDetail_BillNo_int from tbl_CashDetail where CashDetail_Status_bit=0)
        //				                and datediff(d,convert(date,b.OUTW_OutwardDate_dt),convert(date,GETDATE())) > 91 
        //                                and a.Bill_Status_bit=0
        //                                order by AlertDtl_RecType_var";
        //                    }
        //                }
        //                else if (Alert_Id == 21)
        //                {
        //                    if (flagStatus == 0)
        //                    {
        //                        str = @"---- Payment not received in 120 days after bill received by the client
        //                                 Select 0 as AlertDtl_EnqNo_int,null as AlertDtl_EnqDate_date,a.BILL_Id as AlertDtl_BillNo_int,a.BILL_Date_dt as AlertDtl_BillDate_date,a.BILL_NetAmt_num as AlertDtl_BillAmt_dec,
        //                                '' as AlertDtl_ProNo_var,0 as AlertDtl_ProNo_int,null as AlertDtl_ProDate_date,'' as AlertDtl_RefNo_var,0 as AlertDtl_RecNo_int,
        //                                c.CL_Name_var as AlertDtl_CL_Name_var,d.SITE_Name_var as AlertDtl_SITE_Name_var,
        //                                (select MATERIAL_Name_var from tbl_Material e where e.MATERIAL_RecordType_var=a.BILL_RecordType_var)  as AlertDtl_RecType_var,
        //                                c.CL_Id as AlertDtl_CLId_int,d.SITE_Id as AlertDtl_SiteId_int,21 as AlertDtl_Alert_Id,
        //                                (select USER_NAME_var from tbl_User x where x.USER_Id IN (select ROUTE_ME_Id from tbl_Route where ROUTE_Id=d.SITE_Route_Id)) as AlertDtl_MEName_var
        //                                ,null as AlertDtl_ProOrderDate_date,null as AlertDtl_BillModifiedDate_date,null as AlertDtl_CollctnEnqDate_date
        //                                ,null as AlertDtl_InwdRecDate_date,null as AlertDtl_EnteredDate_date,null as AlertDtl_CheckedDate_date,null as AlertDtl_AppDate_date,b.OUTW_OutwardDate_dt as AlertDtl_OutwrdDate_date,'' as AlertDtl_TestName_var
        //                                from tbl_Bill as a,tbl_Client as c,tbl_Site as d,tbl_Outward b,tbl_cashDetail e
        //				                where  a.BILL_CL_Id=c.CL_Id 
        //				                and a.BILL_Id = b.OUTW_ReferenceNo_var  and b.OUTW_RecordType_var ='DT'
        //				                and a.Bill_SITE_Id=d.SITE_Id
        //				                and a.BILL_OutwardStatus_bit =1
        //				                and a.BILL_Id=e.CashDetail_BillNo_int
        //                                and CONVERT (DATE, b.OUTW_OutwardDate_dt) >=CONVERT (DATE,'" + fromDate + "') and CONVERT (DATE,b.OUTW_OutwardDate_dt) <=CONVERT (DATE,'" + ToDate + "') " +
        //                                @" and datediff(d,convert(date,b.OUTW_OutwardDate_dt),convert(date,GETDATE())) > 121
        //				                and datediff(d,convert(date,b.OUTW_OutwardDate_dt),convert(date,e.CashDetail_Date_date)) > 121         
        //                                and a.Bill_Status_bit=0
        //                                and (a.BILL_NetAmt_num + (select Sum(b.CashDetail_Amount_money) from tbl_cashDetail b
        //                                where b.CashDetail_BillNo_int=a.BILL_Id and b.CashDetail_Status_bit=0))>0
        //                                order by AlertDtl_RecType_var";
        //                    }
        //                    else
        //                    {
        //                        str = @"---- Payment not received in 120 days after bill received by the client
        //                                 Select 0 as AlertDtl_EnqNo_int,null as AlertDtl_EnqDate_date,a.BILL_Id as AlertDtl_BillNo_int,a.BILL_Date_dt as AlertDtl_BillDate_date,a.BILL_NetAmt_num as AlertDtl_BillAmt_dec,
        //                                '' as AlertDtl_ProNo_var,0 as AlertDtl_ProNo_int,null as AlertDtl_ProDate_date,'' as AlertDtl_RefNo_var,0 as AlertDtl_RecNo_int,
        //                                c.CL_Name_var as AlertDtl_CL_Name_var,d.SITE_Name_var as AlertDtl_SITE_Name_var,(select MATERIAL_Name_var from tbl_Material e where e.MATERIAL_RecordType_var=a.BILL_RecordType_var)  as AlertDtl_RecType_var,
        //                                c.CL_Id as AlertDtl_CLId_int,d.SITE_Id as AlertDtl_SiteId_int,21 as AlertDtl_Alert_Id,
        //                                (select USER_NAME_var from tbl_User x where x.USER_Id IN (select ROUTE_ME_Id from tbl_Route where ROUTE_Id=d.SITE_Route_Id)) as AlertDtl_MEName_var
        //                                ,null as AlertDtl_ProOrderDate_date,null as AlertDtl_BillModifiedDate_date,null as AlertDtl_CollctnEnqDate_date
        //                                ,null as AlertDtl_InwdRecDate_date,null as AlertDtl_EnteredDate_date,null as AlertDtl_CheckedDate_date,null as AlertDtl_AppDate_date,b.OUTW_OutwardDate_dt as AlertDtl_OutwrdDate_date,'' as AlertDtl_TestName_var
        //                                from tbl_Bill as a,tbl_Client as c,tbl_Site as d,tbl_Outward b
        //				                where  a.BILL_CL_Id=c.CL_Id 
        //				                and a.BILL_Id = b.OUTW_ReferenceNo_var  and b.OUTW_RecordType_var ='DT'
        //				                and a.Bill_SITE_Id=d.SITE_Id
        //				                and a.BILL_OutwardStatus_bit =1
        //				                and CONVERT (DATE,b.OUTW_OutwardDate_dt) >=CONVERT (DATE,'" + fromDate + "') and CONVERT (DATE,b.OUTW_OutwardDate_dt) <=CONVERT (DATE,'" + ToDate + "') " +
        //                                @" and a.Bill_id not in  (select CashDetail_BillNo_int from tbl_CashDetail where CashDetail_Status_bit=0)
        //				                and datediff(d,convert(date,b.OUTW_OutwardDate_dt),convert(date,GETDATE())) > 121 
        //                                and a.Bill_Status_bit=0
        //                                order by AlertDtl_RecType_var";
        //                    }
        //                }
        //                else if (Alert_Id == 22)
        //                {
        //                    if (flagStatus == 0)
        //                    {
        //                        str = @" ----Payment outstanding for more than 120 days
        //				                Select 0 as AlertDtl_EnqNo_int,null as AlertDtl_EnqDate_date,a.BILL_Id as AlertDtl_BillNo_int,a.BILL_Date_dt as AlertDtl_BillDate_date,a.BILL_NetAmt_num as AlertDtl_BillAmt_dec,
        //                                '' as AlertDtl_ProNo_var,0 as AlertDtl_ProNo_int,null as AlertDtl_ProDate_date,'' as AlertDtl_RefNo_var,0 as AlertDtl_RecNo_int,
        //                                c.CL_Name_var as AlertDtl_CL_Name_var,d.SITE_Name_var as AlertDtl_SITE_Name_var,
        //                                (select MATERIAL_Name_var from tbl_Material e where e.MATERIAL_RecordType_var=a.BILL_RecordType_var)  as AlertDtl_RecType_var,
        //                                c.CL_Id as AlertDtl_CLId_int,d.SITE_Id as AlertDtl_SiteId_int,22 as AlertDtl_Alert_Id,
        //                                (select USER_NAME_var from tbl_User x where x.USER_Id IN (select ROUTE_ME_Id from tbl_Route where ROUTE_Id=d.SITE_Route_Id)) as AlertDtl_MEName_var
        //                                ,null as AlertDtl_ProOrderDate_date,null as AlertDtl_BillModifiedDate_date,null as AlertDtl_CollctnEnqDate_date
        //                                ,null as AlertDtl_InwdRecDate_date,null as AlertDtl_EnteredDate_date,null as AlertDtl_CheckedDate_date,null as AlertDtl_AppDate_date,b.OUTW_OutwardDate_dt as AlertDtl_OutwrdDate_date,'' as AlertDtl_TestName_var
        //                                from tbl_Bill as a,tbl_Client as c,tbl_Site as d,tbl_Outward b,tbl_cashDetail e
        //				                where  a.BILL_CL_Id=c.CL_Id 
        //				                and a.BILL_Id = b.OUTW_ReferenceNo_var  and b.OUTW_RecordType_var ='DT'
        //				                and a.Bill_SITE_Id=d.SITE_Id
        //				                and a.BILL_OutwardStatus_bit =1
        //				                and a.BILL_Id=e.CashDetail_BillNo_int
        //                                and CONVERT (DATE, b.OUTW_OutwardDate_dt) >=CONVERT (DATE,'" + fromDate + "') and CONVERT (DATE,b.OUTW_OutwardDate_dt) <=CONVERT (DATE,'" + ToDate + "') " +
        //                                @" and datediff(d,convert(date,b.OUTW_OutwardDate_dt),convert(date,GETDATE())) > 122
        //				                and datediff(d,convert(date,b.OUTW_OutwardDate_dt),convert(date,e.CashDetail_Date_date)) > 122        
        //                                and a.Bill_Status_bit=0
        //                                and (a.BILL_NetAmt_num + (select Sum(b.CashDetail_Amount_money) from tbl_cashDetail b
        //                                where b.CashDetail_BillNo_int=a.BILL_Id and b.CashDetail_Status_bit=0))>0
        //                                order by AlertDtl_RecType_var";
        //                    }
        //                    else
        //                    {
        //                        str = @" ----Payment outstanding for more than 120 days
        //				               Select 0 as AlertDtl_EnqNo_int,null as AlertDtl_EnqDate_date,a.BILL_Id as AlertDtl_BillNo_int,a.BILL_Date_dt as AlertDtl_BillDate_date,a.BILL_NetAmt_num as AlertDtl_BillAmt_dec,
        //                                '' as AlertDtl_ProNo_var,0 as AlertDtl_ProNo_int,null as AlertDtl_ProDate_date,'' as AlertDtl_RefNo_var,0 as AlertDtl_RecNo_int,
        //                                c.CL_Name_var as AlertDtl_CL_Name_var,d.SITE_Name_var as AlertDtl_SITE_Name_var,(select MATERIAL_Name_var from tbl_Material e where e.MATERIAL_RecordType_var=a.BILL_RecordType_var)  as AlertDtl_RecType_var,
        //                                c.CL_Id as AlertDtl_CLId_int,d.SITE_Id as AlertDtl_SiteId_int,22 as AlertDtl_Alert_Id,
        //                                (select USER_NAME_var from tbl_User x where x.USER_Id IN (select ROUTE_ME_Id from tbl_Route where ROUTE_Id=d.SITE_Route_Id)) as AlertDtl_MEName_var
        //                                ,null as AlertDtl_ProOrderDate_date,null as AlertDtl_BillModifiedDate_date,null as AlertDtl_CollctnEnqDate_date
        //                                ,null as AlertDtl_InwdRecDate_date,null as AlertDtl_EnteredDate_date,null as AlertDtl_CheckedDate_date,null as AlertDtl_AppDate_date,b.OUTW_OutwardDate_dt as AlertDtl_OutwrdDate_date,'' as AlertDtl_TestName_var
        //                                from tbl_Bill as a,tbl_Client as c,tbl_Site as d,tbl_Outward b
        //				                where  a.BILL_CL_Id=c.CL_Id 
        //				                and a.BILL_Id = b.OUTW_ReferenceNo_var  and b.OUTW_RecordType_var ='DT'
        //				                and a.Bill_SITE_Id=d.SITE_Id
        //				                and a.BILL_OutwardStatus_bit =1
        //				                and CONVERT (DATE,b.OUTW_OutwardDate_dt) >=CONVERT (DATE,'" + fromDate + "') and CONVERT (DATE,b.OUTW_OutwardDate_dt) <=CONVERT (DATE,'" + ToDate + "') " +
        //                                @" and a.Bill_id not in  (select CashDetail_BillNo_int from tbl_CashDetail where CashDetail_Status_bit=0)
        //				                and datediff(d,convert(date,b.OUTW_OutwardDate_dt),convert(date,GETDATE())) > 122
        //                                and a.Bill_Status_bit=0
        //                                order by AlertDtl_RecType_var";

        //                    }
        //                }
        //                SqlDataAdapter da = new SqlDataAdapter(str, con);//and CL_name_var like '" + str + "%'
        //                da.Fill(ds);
        //                dt = ds.Tables[0];
        //                ds.Dispose();
        //                return dt;
        //            }
        //            catch (Exception e)
        //            {
        //                throw e;
        //            }

        //        }
        //public DataTable AlertDetailsViewForTriggerFixedTime(int Alert_Id)
        //{
        //    SqlConnection con = new SqlConnection(cnStr);
        //    try
        //    {
        //        DataSet ds = new DataSet();
        //        DataTable dt = new DataTable();
        //        string str = "select * from tbl_AlertDetails where AlertDtl_Alert_Id=" + Alert_Id;
        //        //                if (Alert_Id == 1)
        //        //                    str = @"Select a.ENQ_Id,a.ENQ_Date_dt,'' as Bill_Id,null as BILL_Date_dt,0 as BILL_NetAmt_num,
        //        //                						'' as Proposal_No,0 as Proposal_Id,null as Proposal_Date,'' as MISRefNo,0 as MISRecordNo,
        //        //                						c.CL_Name_var,d.SITE_Name_var,e.MATERIAL_Name_var,c.CL_Id,d.SITE_Id,1 as AlertId
        //        //                						,
        //        //                				(select USER_NAME_var from tbl_User x where x.USER_Id IN (select ROUTE_ME_Id from tbl_Route where ROUTE_Id=d.SITE_Route_Id)) as MEName
        //        //                				from tbl_Enquiry as a,tbl_Proposal as b,tbl_Client as c,tbl_Site as d,tbl_Material as e
        //        //                						where a.ENQ_Id=b.Proposal_EnqNo
        //        //                						and a.ENQ_CL_Id=c.CL_Id
        //        //                						and a.ENQ_SITE_Id=d.SITE_Id
        //        //                						and a.ENQ_MATERIAL_Id=e.MATERIAL_Id
        //        //                						and datediff(d,convert(date,a.ENQ_Date_dt),convert(date,b.Proposal_Date)) > 1
        //        //                						and datediff(MONTH, ENQ_Date_dt,CURRENT_TIMESTAMP) < 4
        //        //                						and b.Proposal_NewClientStatus=0 and b.Proposal_ActiveStatus_bit=0
        //        //                						
        //        //                						Union
        //        //                						
        //        //                                        Select a.ENQNEW_Id as ENQ_Id,a.ENQNEW_Date_dt as ENQ_Date_dt,'' as Bill_Id,null as BILL_Date_dt,0 as BILL_NetAmt_num,
        //        //                						'' as Proposal_No,0 as Proposal_Id,null as Proposal_Date,'' as MISRefNo,0 as MISRecordNo,
        //        //                						ENQNEW_ClientName_var,ENQNEW_SiteName_var,e.MATERIAL_Name_var,c.CL_Id,d.SITE_Id,1 as AlertId,'' as MEName
        //        //                						from tbl_EnquiryNewClient as a,tbl_Proposal as b,tbl_Client as c,tbl_Site as d,tbl_Material as e
        //        //                						where a.ENQNEW_Id=b.Proposal_EnqNo
        //        //                						and (a.ENQNEW_CL_Id=c.CL_Id or a.ENQNEW_CL_Id=0)
        //        //                						and (a.ENQNEW_SITE_Id=d.SITE_Id or a.ENQNEW_SITE_Id=0)
        //        //                						and a.ENQNEW_MATERIAL_Id=e.MATERIAL_Id
        //        //                						and datediff(d,convert(date,a.ENQNEW_Date_dt),convert(date,b.Proposal_Date)) > 1
        //        //                						and datediff(MONTH, ENQNEW_Date_dt,CURRENT_TIMESTAMP) < 4
        //        //                		and b.Proposal_NewClientStatus=1  and b.Proposal_ActiveStatus_bit=0
        //        //                	                    order by MATERIAL_Name_var
        //        //                			                ";

        //        //                else if (Alert_Id == 2)
        //        //                    str = @"Select a.ENQ_Id,a.ENQ_Date_dt,'' as Bill_Id,null as BILL_Date_dt,0 as BILL_NetAmt_num,
        //        //                						b.Proposal_No,b.Proposal_Id,b.Proposal_Date,'' as MISRefNo,0 as MISRecordNo,
        //        //                						c.CL_Name_var,d.SITE_Name_var,e.MATERIAL_Name_var,c.CL_Id,d.SITE_Id,2 as AlertId,
        //        //                				(select USER_NAME_var from tbl_User x where x.USER_Id IN (select ROUTE_ME_Id from tbl_Route where ROUTE_Id=d.SITE_Route_Id)) as MEName
        //        //                				from tbl_Enquiry as a,tbl_Proposal as b,tbl_Client as c,tbl_Site as d,tbl_Material as e
        //        //                						where a.ENQ_Id=b.Proposal_EnqNo
        //        //                						and a.ENQ_CL_Id=c.CL_Id
        //        //                						and a.ENQ_SITE_Id=d.SITE_Id
        //        //                						and a.ENQ_MATERIAL_Id=e.MATERIAL_Id
        //        //                						and datediff(d,convert(date,b.Proposal_Date),convert(date,b.Proposal_OrderDate_dt)) > 7
        //        //                						and datediff(MONTH, Proposal_Date,CURRENT_TIMESTAMP) < 4
        //        //                						and b.Proposal_NewClientStatus=0
        //        //                 					    and b.Proposal_ActiveStatus_bit=0
        //        //                						Union
        //        //                						
        //        //                						Select a.ENQNEW_Id as ENQ_Id,a.ENQNEW_Date_dt as ENQ_Date_dt,'' as Bill_Id,null as BILL_Date_dt,0 as BILL_NetAmt_num,
        //        //                						b.Proposal_No,b.Proposal_Id,b.Proposal_Date,'' as MISRefNo,0 as MISRecordNo
        //        //                						,ENQNEW_ClientName_var,ENQNEW_SiteName_var,e.MATERIAL_Name_var,0 as CL_Id,0 as SITE_Id,2 as AlertId
        //        //                						,
        //        //                				'' as MEName
        //        //                				from tbl_EnquiryNewClient as a,tbl_Proposal as b,tbl_Material as e--tbl_Client as c,tbl_Site as d,
        //        //                						where a.ENQNEW_Id=b.Proposal_EnqNo
        //        //                						--and (a.ENQNEW_CL_Id=0 or a.ENQNEW_CL_Id=c.CL_Id)
        //        //                						--and (a.ENQNEW_SITE_Id=d.SITE_Id or a.ENQNEW_SITE_Id=0)
        //        //                						and a.ENQNEW_MATERIAL_Id=e.MATERIAL_Id 
        //        //                						and datediff(d,convert(date,b.Proposal_Date),convert(date,b.Proposal_OrderDate_dt)) > 7
        //        //                						and datediff(MONTH, Proposal_Date,CURRENT_TIMESTAMP) < 4
        //        //                						and b.Proposal_NewClientStatus=1 
        //        //                 					    and b.Proposal_ActiveStatus_bit=0
        //        //                                        order by MATERIAL_Name_var";

        //        //                else if (Alert_Id == 3)
        //        //                    str = @"Select a.ENQ_Id,a.ENQ_Date_dt,'' as Bill_Id,null as BILL_Date_dt,0 as BILL_NetAmt_num,
        //        //                						'' as Proposal_No,0 as Proposal_Id,null as Proposal_Date,'' as MISRefNo,0 as MISRecordNo,
        //        //                						c.CL_Name_var,d.SITE_Name_var,e.MATERIAL_Name_var,c.CL_Id,d.SITE_Id,3 as AlertId,
        //        //                				(select USER_NAME_var from tbl_User x where x.USER_Id IN (select ROUTE_ME_Id from tbl_Route where ROUTE_Id=d.SITE_Route_Id)) as MEName
        //        //                				from tbl_Enquiry as a,tbl_Proposal as b,tbl_Client as c,tbl_Site as d,tbl_Material as e
        //        //                						where a.ENQ_Id=b.Proposal_EnqNo
        //        //                						and a.ENQ_CL_Id=c.CL_Id
        //        //                						and a.ENQ_SITE_Id=d.SITE_Id
        //        //                						and a.ENQ_MATERIAL_Id=e.MATERIAL_Id
        //        //                						  and b.Proposal_NewClientStatus=0 and b.Proposal_ActiveStatus_bit=0
        //        //and datediff(d,convert(date,b.Proposal_OrderDate_dt),convert(date,a.ENQ_CollectionDate_dt)) > 2
        //        //                						and datediff(MONTH, Proposal_OrderDate_dt,CURRENT_TIMESTAMP) < 4 order by MATERIAL_Name_var";

        //        //                else if (Alert_Id == 4)
        //        //                    str = @"Select a.ENQ_Id,a.ENQ_Date_dt,'' as Bill_Id,null as BILL_Date_dt,0 as BILL_NetAmt_num,
        //        //                						'' as Proposal_No,0 as Proposal_Id,null as Proposal_Date,'' as MISRefNo,0 as MISRecordNo,
        //        //                						c.CL_Name_var,d.SITE_Name_var,e.MATERIAL_Name_var,c.CL_Id,d.SITE_Id,4 as AlertId,
        //        //                				(select USER_NAME_var from tbl_User x where x.USER_Id IN (select ROUTE_ME_Id from tbl_Route where ROUTE_Id=d.SITE_Route_Id)) as MEName
        //        //                				from tbl_Enquiry as a,tbl_Inward as b,tbl_Client as c,tbl_Site as d,tbl_Material as e
        //        //                						where a.ENQ_Id=b.INWD_ENQ_Id
        //        //                						and a.ENQ_CL_Id=c.CL_Id
        //        //                						and a.ENQ_SITE_Id=d.SITE_Id
        //        //                						and a.ENQ_MATERIAL_Id=e.MATERIAL_Id
        //        //                						and datediff(d,convert(date,a.ENQ_CollectionDate_dt),convert(date,b.INWD_ReceivedDate_dt)) > 1
        //        //                						and datediff(MONTH, ENQ_CollectionDate_dt,CURRENT_TIMESTAMP) < 4
        //        //                						and a.ENQ_Status_tint <> 2 order by MATERIAL_Name_var";

        //        //                else if (Alert_Id == 7)
        //        //                    str = @"
        //        //                						Select a.ENQ_Id,a.ENQ_Date_dt,'' as Bill_Id,null as BILL_Date_dt,0 as BILL_NetAmt_num,
        //        //                						'' as Proposal_No,0 as Proposal_Id,null as Proposal_Date, MISRefNo,MISRecordNo,
        //        //                						c.CL_Name_var,d.SITE_Name_var,e.MATERIAL_Name_var,c.CL_Id,d.SITE_Id,7 as AlertId,
        //        //                				(select USER_NAME_var from tbl_User x where x.USER_Id IN (select ROUTE_ME_Id from tbl_Route where ROUTE_Id=d.SITE_Route_Id)) as MEName
        //        //                				from tbl_Enquiry as a,tbl_Inward as b,tbl_Client as c,tbl_Site as d,tbl_Material as e,tbl_MISDetail as f
        //        //                						where a.ENQ_Id=b.INWD_ENQ_Id
        //        //                						and b.INWD_RecordNo_int=f.MISRecordNo
        //        //                						and b.INWD_RecordType_var=f.MISRecType
        //        //                						and a.ENQ_CL_Id=c.CL_Id
        //        //                						and a.ENQ_SITE_Id=d.SITE_Id
        //        //                						and a.ENQ_MATERIAL_Id=e.MATERIAL_Id
        //        //                						and datediff(d,convert(date,b.INWD_ReceivedDate_dt),convert(date,f.MISEnteredDt)) > 1
        //        //                						and datediff(MONTH, INWD_ReceivedDate_dt,CURRENT_TIMESTAMP) < 4 order by MATERIAL_Name_var";

        //        //                else if (Alert_Id == 8)
        //        //                    str = @"Select a.ENQ_Id,a.ENQ_Date_dt,'' as Bill_Id,null as BILL_Date_dt,0 as BILL_NetAmt_num,
        //        //                						'' as Proposal_No,0 as Proposal_Id,null as Proposal_Date,f.MISRefNo,f.MISRecordNo,
        //        //                						c.CL_Name_var,d.SITE_Name_var,e.MATERIAL_Name_var,c.CL_Id,d.SITE_Id,8 as AlertId,
        //        //                				(select USER_NAME_var from tbl_User x where x.USER_Id IN (select ROUTE_ME_Id from tbl_Route where ROUTE_Id=d.SITE_Route_Id)) as MEName
        //        //                				from tbl_Enquiry as a,tbl_Inward as b,tbl_Client as c,tbl_Site as d,tbl_Material as e,tbl_MISDetail as f
        //        //                						where a.ENQ_Id=b.INWD_ENQ_Id
        //        //                						and b.INWD_RecordNo_int=f.MISRecordNo
        //        //                						and b.INWD_RecordType_var=f.MISRecType
        //        //                						and a.ENQ_CL_Id=c.CL_Id
        //        //                						and a.ENQ_SITE_Id=d.SITE_Id
        //        //                						and a.ENQ_MATERIAL_Id=e.MATERIAL_Id
        //        //                						and datediff(d,convert(date,f.MISEnteredDt),convert(date,f.MISCheckedDt)) > 1
        //        //                						and datediff(MONTH, MISEnteredDt,CURRENT_TIMESTAMP) < 4 order by MATERIAL_Name_var";

        //        //                else if (Alert_Id == 9)
        //        //                    str = @"Select a.ENQ_Id,a.ENQ_Date_dt,'' as Bill_Id,null as BILL_Date_dt,0 as BILL_NetAmt_num,
        //        //                						'' as Proposal_No,0 as Proposal_Id,null as Proposal_Date,f.MISRefNo,f.MISRecordNo,
        //        //                						c.CL_Name_var,d.SITE_Name_var,e.MATERIAL_Name_var,c.CL_Id,d.SITE_Id,9 as AlertId,
        //        //                				(select USER_NAME_var from tbl_User x where x.USER_Id IN (select ROUTE_ME_Id from tbl_Route where ROUTE_Id=d.SITE_Route_Id)) as MEName
        //        //                				from tbl_Enquiry as a,tbl_Inward as b,tbl_Client as c,tbl_Site as d,tbl_Material as e,tbl_MISDetail as f
        //        //                						where a.ENQ_Id=b.INWD_ENQ_Id
        //        //                						and b.INWD_RecordNo_int=f.MISRecordNo
        //        //                						and b.INWD_RecordType_var=f.MISRecType
        //        //                						and a.ENQ_CL_Id=c.CL_Id
        //        //                						and a.ENQ_SITE_Id=d.SITE_Id
        //        //                						and a.ENQ_MATERIAL_Id=e.MATERIAL_Id
        //        //                						and datediff(d,convert(date,f.MISCheckedDt),convert(date,f.MISApprovedDt)) > 1
        //        //                						and datediff(MONTH, MISCheckedDt,CURRENT_TIMESTAMP) < 4 order by MATERIAL_Name_var";

        //        //                else if (Alert_Id == 11)
        //        //                    str = @"Select a.ENQ_Id,a.ENQ_Date_dt,'' as Bill_Id,null as BILL_Date_dt,0 as BILL_NetAmt_num,
        //        //                						'' as Proposal_No,0 as Proposal_Id,null as Proposal_Date,f.MISRefNo,f.MISRecordNo,
        //        //                						c.CL_Name_var,d.SITE_Name_var,e.MATERIAL_Name_var,c.CL_Id,d.SITE_Id,11 as AlertId,
        //        //                				(select USER_NAME_var from tbl_User x where x.USER_Id IN (select ROUTE_ME_Id from tbl_Route where ROUTE_Id=d.SITE_Route_Id)) as MEName
        //        //                				from tbl_Enquiry as a,tbl_Inward as b,tbl_Client as c,tbl_Site as d,tbl_Material as e,tbl_MISDetail as f
        //        //                						where a.ENQ_Id=b.INWD_ENQ_Id
        //        //                						and b.INWD_RecordNo_int=f.MISRecordNo
        //        //                						and b.INWD_RecordType_var=f.MISRecType
        //        //                						and a.ENQ_CL_Id=c.CL_Id
        //        //                						and a.ENQ_SITE_Id=d.SITE_Id
        //        //                						and a.ENQ_MATERIAL_Id=e.MATERIAL_Id
        //        //                						and datediff(d,convert(date,f.MISApprovedDt),convert(date,f.MISIssueDt)) <= 1
        //        //                						and datediff(MONTH, MISApprovedDt,CURRENT_TIMESTAMP) < 4 order by MATERIAL_Name_var";
        //        //                else if (Alert_Id == 12)
        //        //                    str = @"Select a.ENQ_Id,a.ENQ_Date_dt,'' as Bill_Id,null as BILL_Date_dt,0 as BILL_NetAmt_num,
        //        //                						'' as Proposal_No,0 as Proposal_Id,null as Proposal_Date,f.MISRefNo,f.MISRecordNo,
        //        //                						c.CL_Name_var,d.SITE_Name_var,e.MATERIAL_Name_var,c.CL_Id,d.SITE_Id,12 as AlertId,
        //        //                				(select USER_NAME_var from tbl_User x where x.USER_Id IN (select ROUTE_ME_Id from tbl_Route where ROUTE_Id=d.SITE_Route_Id)) as MEName
        //        //                				from tbl_Enquiry as a,tbl_Inward as b,tbl_Client as c,tbl_Site as d,tbl_Material as e,tbl_MISDetail as f,tbl_Bill as g
        //        //                						where a.ENQ_Id=b.INWD_ENQ_Id
        //        //                						and b.INWD_RecordNo_int=f.MISRecordNo
        //        //                						and b.INWD_BILL_Id=g.BILL_Id
        //        //                						and b.INWD_RecordType_var=f.MISRecType
        //        //                						and a.ENQ_CL_Id=c.CL_Id
        //        //                						and a.ENQ_SITE_Id=d.SITE_Id
        //        //                						and a.ENQ_MATERIAL_Id=e.MATERIAL_Id
        //        //                						and datediff(d,convert(date,f.MISApprovedDt),convert(date,f.MISIssueDt)) <= 3
        //        //                						and datediff(MONTH, MISApprovedDt,CURRENT_TIMESTAMP) < 4 order by MATERIAL_Name_var";

        //        //                else if (Alert_Id == 13)
        //        //                    str = @"Select a.ENQ_Id,a.ENQ_Date_dt,'' as Bill_Id,null as BILL_Date_dt,0 as BILL_NetAmt_num,
        //        //                						'' as Proposal_No,0 as Proposal_Id,null as Proposal_Date,f.MISRefNo,f.MISRecordNo,
        //        //                						c.CL_Name_var,d.SITE_Name_var,e.MATERIAL_Name_var,c.CL_Id,d.SITE_Id,13 as AlertId,
        //        //                				(select USER_NAME_var from tbl_User x where x.USER_Id IN (select ROUTE_ME_Id from tbl_Route where ROUTE_Id=d.SITE_Route_Id)) as MEName
        //        //                				from tbl_Enquiry as a,tbl_Inward as b,tbl_Client as c,tbl_Site as d,tbl_Material as e,tbl_MISDetail as f,tbl_Bill as g
        //        //                						where a.ENQ_Id=b.INWD_ENQ_Id
        //        //                						and b.INWD_RecordNo_int=f.MISRecordNo
        //        //                						and b.INWD_BILL_Id=g.BILL_Id
        //        //                						and b.INWD_RecordType_var=f.MISRecType
        //        //                						and a.ENQ_CL_Id=c.CL_Id
        //        //                						and a.ENQ_SITE_Id=d.SITE_Id
        //        //                						and a.ENQ_MATERIAL_Id=e.MATERIAL_Id
        //        //                						and datediff(d,convert(date,f.MISApprovedDt),convert(date,f.MISIssueDt)) > 3
        //        //                						and datediff(MONTH, MISApprovedDt,CURRENT_TIMESTAMP) < 4 order by MATERIAL_Name_var";

        //        //                else if (Alert_Id == 15)
        //        //                    str = @"Select 0 as ENQ_Id,null as ENQ_Date_dt,a.BILL_Id,a.BILL_Date_dt,a.BILL_NetAmt_num,
        //        //                						'' as Proposal_No,0 as Proposal_Id,null as Proposal_Date,'' as MISRefNo,0 as MISRecordNo,
        //        //                						c.CL_Name_var,d.SITE_Name_var,(select MATERIAL_Name_var from tbl_Material e where e.MATERIAL_RecordType_var=a.BILL_RecordType_var)  as MATERIAL_Name_var,
        //        //                				        c.CL_Id,d.SITE_Id,15 as AlertId,
        //        //                				(select USER_NAME_var from tbl_User x where x.USER_Id IN (select ROUTE_ME_Id from tbl_Route where ROUTE_Id=d.SITE_Route_Id)) as MEName
        //        //                				from tbl_Client as c,tbl_Site as d,tbl_Bill as a
        //        //                						where  a.BILL_CL_Id=c.CL_Id 
        //        //                						and a.Bill_SITE_Id=d.SITE_Id
        //        //                						and datediff(d,convert(date,a.BILL_Date_dt),convert(date,a.BILL_ModifiedOn_dt)) > 2
        //        //                						and datediff(MONTH, BILL_Date_dt,CURRENT_TIMESTAMP) < 4";

        //        //                else if (Alert_Id == 17)
        //        //                    str = @"Select 0 as ENQ_Id,null as ENQ_Date_dt,a.BILL_Id,a.BILL_Date_dt,a.BILL_NetAmt_num,
        //        //                						'' as Proposal_No,0 as Proposal_Id,null as Proposal_Date,'' as MISRefNo,0 as MISRecordNo,
        //        //                						c.CL_Name_var,d.SITE_Name_var,(select MATERIAL_Name_var from tbl_Material e where e.MATERIAL_RecordType_var=a.BILL_RecordType_var)  as MATERIAL_Name_var,
        //        //                				        c.CL_Id,d.SITE_Id,17 as AlertId,
        //        //                				(select USER_NAME_var from tbl_User x where x.USER_Id IN (select ROUTE_ME_Id from tbl_Route where ROUTE_Id=d.SITE_Route_Id)) as MEName
        //        //                				from tbl_Bill as a,tbl_Client as c,tbl_Site as d,tbl_CashDetail e
        //        //				where  a.BILL_CL_Id=c.CL_Id 
        //        //				and a.BILL_Id = e.CashDetail_BillNo_int
        //        //                						and a.Bill_SITE_Id=d.SITE_Id
        //        //                						--and datediff(d,convert(date,a.BILL_Date_dt),convert(date,GETDATE())) >= 60
        //        //                						and datediff(d,convert(date,a.BILL_Date_dt),convert(date,e.CashDetail_Date_date)) >= 60
        //        //                						and datediff(MONTH, BILL_Date_dt,CURRENT_TIMESTAMP) < 4
        //        //                						and a.Bill_Status_bit=0
        //        //                						and e.CashDetail_BillNo_int is not null
        //        //				and (
        //        //                						 isNULL ((Select sum(b.BILL_NetAmt_num) from tbl_Bill as b where  b.BILL_Status_bit=0
        //        //                							),0) + 
        //        //                						isNULL ((Select sum(c.Journal_Amount_dec) from tbl_Journal as c,tbl_JournalDetail as d where c.Journal_Status_bit=0
        //        //                							and c.Journal_Note_var=d.JournalDetail_NoteNo_var
        //        //                							and a.BILL_Id=d.JournalDetail_BillNo_var
        //        //                							And CHARINDEX('DB/', c.Journal_NoteNo_var,1) > 0
        //        //                						),0) 
        //        //                						+
        //        //                						isNULL ((Select sum(d.CashDetail_Amount_money) from tbl_CashDetail as d where 
        //        //                							d.CashDetail_Status_bit=0 and a.BILL_Id=d.CashDetail_BillNo_int),0) 
        //        //                						)> 0  order by MATERIAL_Name_var";

        //        //                else if (Alert_Id == 18)
        //        //                    str = @"Select 0 as ENQ_Id,null as ENQ_Date_dt,a.BILL_Id,a.BILL_Date_dt,a.BILL_NetAmt_num,
        //        //                						'' as Proposal_No,0 as Proposal_Id,null as Proposal_Date,'' as MISRefNo,0 as MISRecordNo,
        //        //                						c.CL_Name_var,d.SITE_Name_var,(select MATERIAL_Name_var from tbl_Material e where e.MATERIAL_RecordType_var=a.BILL_RecordType_var)  as MATERIAL_Name_var,
        //        //                				        c.CL_Id,d.SITE_Id,18 as AlertId,
        //        //                				(select USER_NAME_var from tbl_User x where x.USER_Id IN (select ROUTE_ME_Id from tbl_Route where ROUTE_Id=d.SITE_Route_Id)) as MEName
        //        //                				from tbl_Bill as a,tbl_Client as c,tbl_Site as d,tbl_CashDetail e
        //        //				where  a.BILL_CL_Id=c.CL_Id 
        //        //				and a.BILL_Id = e.CashDetail_BillNo_int
        //        //                						and a.Bill_SITE_Id=d.SITE_Id
        //        //                						--and datediff(d,convert(date,a.BILL_Date_dt),convert(date,GETDATE())) >= 90
        //        //                						and datediff(d,convert(date,a.BILL_Date_dt),convert(date,e.CashDetail_Date_date)) >= 90
        //        //                						and datediff(MONTH, BILL_Date_dt,CURRENT_TIMESTAMP) < 4
        //        //                						and a.Bill_Status_bit=0
        //        //                						and e.CashDetail_BillNo_int is not null
        //        //				and (
        //        //                						 isNULL ((Select sum(b.BILL_NetAmt_num) from tbl_Bill as b where  b.BILL_Status_bit=0
        //        //                							),0) + 
        //        //                						isNULL ((Select sum(c.Journal_Amount_dec) from tbl_Journal as c,tbl_JournalDetail as d where c.Journal_Status_bit=0
        //        //                							and c.Journal_Note_var=d.JournalDetail_NoteNo_var
        //        //                							and a.BILL_Id=d.JournalDetail_BillNo_var
        //        //                							And CHARINDEX('DB/', c.Journal_NoteNo_var,1) > 0
        //        //                						),0) 
        //        //                						+
        //        //                						isNULL ((Select sum(d.CashDetail_Amount_money) from tbl_CashDetail as d where 
        //        //                							d.CashDetail_Status_bit=0 and a.BILL_Id=d.CashDetail_BillNo_int),0) 
        //        //                						)> 0 order by MATERIAL_Name_var";

        //        //                else if (Alert_Id == 19)
        //        //                    str = @"Select 0 as ENQ_Id,null as ENQ_Date_dt,a.BILL_Id,a.BILL_Date_dt,a.BILL_NetAmt_num,
        //        //                						'' as Proposal_No,0 as Proposal_Id,null as Proposal_Date,'' as MISRefNo,0 as MISRecordNo,
        //        //                						c.CL_Name_var,d.SITE_Name_var,(select MATERIAL_Name_var from tbl_Material e where e.MATERIAL_RecordType_var=a.BILL_RecordType_var)  as MATERIAL_Name_var,
        //        //                				        c.CL_Id,d.SITE_Id,19 as AlertId,
        //        //                				(select USER_NAME_var from tbl_User x where x.USER_Id IN (select ROUTE_ME_Id from tbl_Route where ROUTE_Id=d.SITE_Route_Id)) as MEName
        //        //                				from tbl_Bill as a,tbl_Client as c,tbl_Site as d,tbl_CashDetail e
        //        //				where  a.BILL_CL_Id=c.CL_Id 
        //        //				and a.BILL_Id = e.CashDetail_BillNo_int
        //        //                						and a.Bill_SITE_Id=d.SITE_Id
        //        //                						--and datediff(d,convert(date,a.BILL_Date_dt),convert(date,GETDATE())) >= 120
        //        //                						and datediff(d,convert(date,a.BILL_Date_dt),convert(date,e.CashDetail_Date_date)) >= 120
        //        //                						and datediff(MONTH, BILL_Date_dt,CURRENT_TIMESTAMP) < 4
        //        //                						and a.Bill_Status_bit=0
        //        //                						and e.CashDetail_BillNo_int is not null
        //        //				and (
        //        //                						 isNULL ((Select sum(b.BILL_NetAmt_num) from tbl_Bill as b where  b.BILL_Status_bit=0
        //        //                							),0) + 
        //        //                						isNULL ((Select sum(c.Journal_Amount_dec) from tbl_Journal as c,tbl_JournalDetail as d where c.Journal_Status_bit=0
        //        //                							and c.Journal_Note_var=d.JournalDetail_NoteNo_var
        //        //                							and a.BILL_Id=d.JournalDetail_BillNo_var
        //        //                							And CHARINDEX('DB/', c.Journal_NoteNo_var,1) > 0
        //        //                						),0) 
        //        //                						+
        //        //                						isNULL ((Select sum(d.CashDetail_Amount_money) from tbl_CashDetail as d where 
        //        //                							d.CashDetail_Status_bit=0 and a.BILL_Id=d.CashDetail_BillNo_int),0) 
        //        //                						)> 0 order by MATERIAL_Name_var";

        //        //                else if (Alert_Id == 20)
        //        //                    str = @"Select 0 as ENQ_Id,null as ENQ_Date_dt,a.BILL_Id,a.BILL_Date_dt,a.BILL_NetAmt_num,
        //        //                						'' as Proposal_No,0 as Proposal_Id,null as Proposal_Date,'' as MISRefNo,0 as MISRecordNo
        //        //                						,c.CL_Name_var,d.SITE_Name_var,(select MATERIAL_Name_var from tbl_Material e where e.MATERIAL_RecordType_var=a.BILL_RecordType_var)  as MATERIAL_Name_var,
        //        //                				        c.CL_Id,d.SITE_Id,20 as AlertId,
        //        //                				(select USER_NAME_var from tbl_User x where x.USER_Id IN (select ROUTE_ME_Id from tbl_Route where ROUTE_Id=d.SITE_Route_Id)) as MEName
        //        //                				from tbl_Bill as a,tbl_Client as c,tbl_Site as d,tbl_CashDetail e
        //        //				where  a.BILL_CL_Id=c.CL_Id 
        //        //				and a.BILL_Id = e.CashDetail_BillNo_int
        //        //                						and a.Bill_SITE_Id=d.SITE_Id
        //        //                						--and datediff(d,convert(date,a.BILL_Date_dt),convert(date,GETDATE())) > 120
        //        //                						and datediff(d,convert(date,a.BILL_Date_dt),convert(date,e.CashDetail_Date_date)) > 120
        //        //                						and datediff(MONTH, BILL_Date_dt,CURRENT_TIMESTAMP) < 4
        //        //                						and a.Bill_Status_bit=0
        //        //                						and e.CashDetail_BillNo_int is not null
        //        //				                        and (
        //        //                						 isNULL ((Select sum(b.BILL_NetAmt_num) from tbl_Bill as b where  b.BILL_Status_bit=0
        //        //                							),0) + 
        //        //                						isNULL ((Select sum(c.Journal_Amount_dec) from tbl_Journal as c,tbl_JournalDetail as d where c.Journal_Status_bit=0
        //        //                							and c.Journal_Note_var=d.JournalDetail_NoteNo_var
        //        //                							and a.BILL_Id=d.JournalDetail_BillNo_var
        //        //                							And CHARINDEX('DB/', c.Journal_NoteNo_var,1) > 0
        //        //                						),0) 
        //        //                						+
        //        //                						isNULL ((Select sum(d.CashDetail_Amount_money) from tbl_CashDetail as d where 
        //        //                							d.CashDetail_Status_bit=0 and a.BILL_Id=d.CashDetail_BillNo_int),0) 
        //        //                						)> 0 order by MATERIAL_Name_var";

        //        SqlDataAdapter da = new SqlDataAdapter(str, con);//and CL_name_var like '" + str + "%'
        //        da.Fill(ds);
        //        dt = ds.Tables[0];
        //        ds.Dispose();
        //        return dt;
        //    }
        //    catch (Exception e)
        //    {
        //        throw e;
        //    }

        //}
        //public Int32 AlertDetailsCountViewForEscalation(int Alert_Id)
        //{
        //    SqlConnection con = new SqlConnection(cnStr);
        //    try
        //    {
        //        int count = 0;
        //        SqlConnection cn = new SqlConnection(cnStr);
        //        SqlCommand cmd = new SqlCommand();
        //        cmd.CommandText = "select AlertDtl_EsclatnCount_int from tbl_AlertDetails where AlertDtl_Alert_Id=" + Alert_Id;
        //        //                if (Alert_Id == 21)
        //        //                    cmd.CommandText = @" select(Select COUNT(Enq_Id)
        //        //				from tbl_Enquiry as a,tbl_Proposal as b,tbl_Client as c,tbl_Site as d,tbl_Material as e
        //        //				where a.ENQ_Id=b.Proposal_EnqNo
        //        //				and a.ENQ_CL_Id=c.CL_Id
        //        //				and a.ENQ_SITE_Id=d.SITE_Id
        //        //				and a.ENQ_MATERIAL_Id=e.MATERIAL_Id
        //        //				and datediff(d,convert(date,a.ENQ_Date_dt),convert(date,b.Proposal_Date)) > 2
        //        //				and datediff(MONTH, ENQ_Date_dt,CURRENT_TIMESTAMP) < 4
        //        //				and b.Proposal_NewClientStatus=0 and b.Proposal_ActiveStatus_bit=0)
        //        //				
        //        //				+
        //        //				
        //        //				(Select COUNT(EnqNEW_Id)
        //        //				from tbl_EnquiryNewClient as a,tbl_Proposal as b,tbl_Client as c,tbl_Site as d,tbl_Material as e
        //        //				where a.ENQNEW_Id=b.Proposal_EnqNo
        //        //				and (a.ENQNEW_CL_Id=c.CL_Id or a.ENQNEW_CL_Id=0)
        //        //				and (a.ENQNEW_SITE_Id=d.SITE_Id or a.ENQNEW_SITE_Id=0)
        //        //				and a.ENQNEW_MATERIAL_Id=e.MATERIAL_Id
        //        //				and datediff(d,convert(date,a.ENQNEW_Date_dt),convert(date,b.Proposal_Date)) > 2
        //        //				and datediff(MONTH, ENQNEW_Date_dt,CURRENT_TIMESTAMP) < 4
        //        //				and b.Proposal_NewClientStatus=1 and b.Proposal_ActiveStatus_bit=0)as AlertCount,21 as AlertId";

        //        //                else if (Alert_Id == 22)
        //        //                    cmd.CommandText = @"select(Select COUNT(Proposal_OrderNo_var)
        //        //				from tbl_Enquiry as a,tbl_Proposal as b,tbl_Client as c,tbl_Site as d,tbl_Material as e
        //        //				where a.ENQ_Id=b.Proposal_EnqNo
        //        //				and a.ENQ_CL_Id=c.CL_Id
        //        //				and a.ENQ_SITE_Id=d.SITE_Id
        //        //				and a.ENQ_MATERIAL_Id=e.MATERIAL_Id
        //        //				and datediff(d,convert(date,b.Proposal_Date),convert(date,b.Proposal_OrderDate_dt)) > 15
        //        //				and datediff(MONTH, ENQ_Date_dt,CURRENT_TIMESTAMP) < 4
        //        //				and b.Proposal_NewClientStatus=0 and b.Proposal_ActiveStatus_bit=0)
        //        //				
        //        //				+
        //        //				
        //        //				(Select COUNT(Proposal_OrderNo_var)
        //        //				from tbl_EnquiryNewClient as a,tbl_Proposal as b,tbl_Material as e--tbl_Client as c,tbl_Site as d,
        //        //				where a.ENQNEW_Id=b.Proposal_EnqNo
        //        //				--and (a.ENQNEW_CL_Id=0 or a.ENQNEW_CL_Id=c.CL_Id)
        //        //				--and (a.ENQNEW_SITE_Id=d.SITE_Id or a.ENQNEW_SITE_Id=0)
        //        //				and a.ENQNEW_MATERIAL_Id=e.MATERIAL_Id 
        //        //				and datediff(d,convert(date,b.Proposal_Date),convert(date,b.Proposal_OrderDate_dt)) > 15
        //        //				and datediff(MONTH, Proposal_Date,CURRENT_TIMESTAMP) < 4
        //        //				and b.Proposal_NewClientStatus=1 and b.Proposal_ActiveStatus_bit=0) as AlertCount,22 as AlertId";

        //        //                else if (Alert_Id == 23)
        //        //                    cmd.CommandText = @"Select COUNT(Proposal_OrderNo_var),23 as AlertId
        //        //				from tbl_Enquiry as a,tbl_Proposal as b,tbl_Client as c,tbl_Site as d,tbl_Material as e
        //        //				where a.ENQ_Id=b.Proposal_EnqNo
        //        //				and a.ENQ_CL_Id=c.CL_Id
        //        //				and a.ENQ_SITE_Id=d.SITE_Id
        //        //				and a.ENQ_MATERIAL_Id=e.MATERIAL_Id
        //        //				and datediff(d,convert(date,b.Proposal_OrderDate_dt),convert(date,a.ENQ_CollectionDate_dt)) > 3
        //        //				and datediff(MONTH, Proposal_OrderDate_dt,CURRENT_TIMESTAMP) < 4
        //        //				and ENQ_Status_tint=5";

        //        //                else if (Alert_Id == 24)
        //        //                    cmd.CommandText = @"Select COUNT(ENQ_Id),24 as AlertId
        //        //				from tbl_Enquiry as a,tbl_Inward as b,tbl_Client as c,tbl_Site as d,tbl_Material as e
        //        //				where a.ENQ_Id=b.INWD_ENQ_Id
        //        //				and a.ENQ_CL_Id=c.CL_Id
        //        //				and a.ENQ_SITE_Id=d.SITE_Id
        //        //				and a.ENQ_MATERIAL_Id=e.MATERIAL_Id
        //        //				and datediff(d,convert(date,ENQ_CollectionDate_dt),convert(date,GETDATE())) > 2
        //        //				and datediff(MONTH, ENQ_CollectionDate_dt,CURRENT_TIMESTAMP) < 4
        //        //				and a.ENQ_Status_tint <> 2";

        //        //                else if (Alert_Id == 25)
        //        //                    cmd.CommandText = @"Select COUNT(ENQ_Id),25 as AlertId
        //        //				from tbl_Enquiry as a,tbl_Inward as b,tbl_Client as c,tbl_Site as d,tbl_Material as e,tbl_MISDetail as f
        //        //				where a.ENQ_Id=b.INWD_ENQ_Id
        //        //				and b.INWD_RecordNo_int=f.MISRecordNo
        //        //				and b.INWD_RecordType_var=f.MISRecType
        //        //				and a.ENQ_CL_Id=c.CL_Id
        //        //				and a.ENQ_SITE_Id=d.SITE_Id
        //        //				and a.ENQ_MATERIAL_Id=e.MATERIAL_Id
        //        //				and datediff(d,convert(date,b.INWD_ReceivedDate_dt),convert(date,f.MISEnteredDt)) > 2
        //        //				and datediff(MONTH, INWD_ReceivedDate_dt,CURRENT_TIMESTAMP) < 4
        //        //				and a.ENQ_Status_tint=2";

        //        //                else if (Alert_Id == 28)
        //        //                    cmd.CommandText = @"	Select COUNT(ENQ_Id),28 as AlertId
        //        //				from tbl_Enquiry as a,tbl_Inward as b,tbl_Client as c,tbl_Site as d,tbl_Material as e,tbl_MISDetail as f
        //        //				where a.ENQ_Id=b.INWD_ENQ_Id
        //        //				and b.INWD_RecordNo_int=f.MISRecordNo
        //        //				and b.INWD_RecordType_var=f.MISRecType
        //        //				and a.ENQ_CL_Id=c.CL_Id
        //        //				and a.ENQ_SITE_Id=d.SITE_Id
        //        //				and a.ENQ_MATERIAL_Id=e.MATERIAL_Id
        //        //				and datediff(d,convert(date,f.MISEnteredDt),convert(date,f.MISCheckedDt)) > 1
        //        //				and datediff(MONTH, MISEnteredDt,CURRENT_TIMESTAMP) < 4
        //        //				and a.ENQ_Status_tint=2";

        //        //                else if (Alert_Id == 29)
        //        //                    cmd.CommandText = @"Select  COUNT(b.INWD_BILL_Id),29 as AlertId
        //        //				from tbl_Enquiry as a,tbl_Inward as b,tbl_Client as c,tbl_Site as d,tbl_Material as e,tbl_MISDetail as f
        //        //				where a.ENQ_Id=b.INWD_ENQ_Id
        //        //				and b.INWD_RecordNo_int=f.MISRecordNo
        //        //				and b.INWD_RecordType_var=f.MISRecType
        //        //				and a.ENQ_CL_Id=c.CL_Id
        //        //				and a.ENQ_SITE_Id=d.SITE_Id
        //        //				and a.ENQ_MATERIAL_Id=e.MATERIAL_Id
        //        //				and datediff(MONTH, MISEnteredDt,CURRENT_TIMESTAMP) < 4
        //        //				and datediff(d,convert(date,f.MISEnteredDt),convert(date,f.MISApprovedDt)) > 1";

        //        //                else if (Alert_Id == 30)
        //        //                    cmd.CommandText = @"Select  COUNT(b.INWD_BILL_Id),30 as AlertId
        //        //				from tbl_Enquiry as a,tbl_Inward as b,tbl_Client as c,tbl_Site as d,tbl_Material as e,tbl_MISDetail as f,tbl_Bill as g
        //        //				where a.ENQ_Id=b.INWD_ENQ_Id
        //        //				and b.INWD_BILL_Id=g.BILL_Id
        //        //				and b.INWD_RecordNo_int=f.MISRecordNo
        //        //				and b.INWD_RecordType_var=f.MISRecType
        //        //				and a.ENQ_CL_Id=c.CL_Id
        //        //				and a.ENQ_SITE_Id=d.SITE_Id
        //        //				and a.ENQ_MATERIAL_Id=e.MATERIAL_Id
        //        //				and datediff(MONTH, MISApprovedDt,CURRENT_TIMESTAMP) < 4
        //        //				and datediff(d,convert(date,f.MISApprovedDt),convert(date,g.BILL_Date_dt)) > 1
        //        //		";
        //        //                else if (Alert_Id == 31)
        //        //                    cmd.CommandText = @"Select COUNT(f.ID),31 as AlertId
        //        //				from tbl_Enquiry as a,tbl_Inward as b,tbl_Client as c,tbl_Site as d,tbl_Material as e,tbl_MISDetail as f
        //        //				where a.ENQ_Id=b.INWD_ENQ_Id
        //        //				and b.INWD_RecordNo_int=f.MISRecordNo
        //        //				and b.INWD_RecordType_var=f.MISRecType
        //        //				and a.ENQ_CL_Id=c.CL_Id
        //        //				and a.ENQ_SITE_Id=d.SITE_Id
        //        //				and a.ENQ_MATERIAL_Id=e.MATERIAL_Id
        //        //				and datediff(MONTH, MISApprovedDt,CURRENT_TIMESTAMP) < 4
        //        //				and datediff(d,convert(date,f.MISApprovedDt),convert(date,f.MISPrintedDt)) > 1
        //        //				";

        //        //                else if (Alert_Id == 32)
        //        //                    cmd.CommandText = @"Select COUNT(f.ID),32 as AlertId
        //        //				from tbl_Enquiry as a,tbl_Inward as b,tbl_Client as c,tbl_Site as d,tbl_Material as e,tbl_MISDetail as f
        //        //				where a.ENQ_Id=b.INWD_ENQ_Id
        //        //				and b.INWD_RecordNo_int=f.MISRecordNo
        //        //				and b.INWD_RecordType_var=f.MISRecType
        //        //				and a.ENQ_CL_Id=c.CL_Id
        //        //				and a.ENQ_SITE_Id=d.SITE_Id
        //        //				and a.ENQ_MATERIAL_Id=e.MATERIAL_Id
        //        //				and datediff(d,convert(date,f.MISApprovedDt),convert(date,f.MISPrintedDt)) <= 3
        //        //				and datediff(MONTH, MISApprovedDt,CURRENT_TIMESTAMP) < 4
        //        //				";

        //        //                else if (Alert_Id == 33)
        //        //                    cmd.CommandText = @"Select COUNT(f.ID),33 as AlertId
        //        //				from tbl_Enquiry as a,tbl_Inward as b,tbl_Client as c,tbl_Site as d,tbl_Material as e,tbl_MISDetail as f
        //        //				where a.ENQ_Id=b.INWD_ENQ_Id
        //        //				and b.INWD_RecordNo_int=f.MISRecordNo
        //        //				and b.INWD_RecordType_var=f.MISRecType
        //        //				and a.ENQ_CL_Id=c.CL_Id
        //        //				and a.ENQ_SITE_Id=d.SITE_Id
        //        //				and a.ENQ_MATERIAL_Id=e.MATERIAL_Id
        //        //				and datediff(d,convert(date,f.MISApprovedDt),convert(date,f.MISPrintedDt)) > 3
        //        //				and datediff(MONTH, MISApprovedDt,CURRENT_TIMESTAMP) < 4";

        //        //                else if (Alert_Id == 35)
        //        //                    cmd.CommandText = @"Select COUNT(BILL_Id),35 as AlertId
        //        //				from tbl_Client as c,tbl_Site as d,tbl_Bill as a
        //        //				where  a.BILL_CL_Id=c.CL_Id 
        //        //				and a.Bill_SITE_Id=d.SITE_Id
        //        //				and datediff(d,convert(date,a.BILL_Date_dt),convert(date,a.BILL_ModifiedOn_dt)) > 2
        //        //				and datediff(MONTH, BILL_Date_dt,CURRENT_TIMESTAMP) < 4
        //        //			";

        //        cn.Open();
        //        cmd.Connection = cn;

        //        using (SqlDataReader reader = cmd.ExecuteReader())
        //        {
        //            while (reader.Read())
        //            {
        //                count = reader.GetInt32(0);
        //            }
        //        }
        //        cmd.Dispose();
        //        cn.Close();
        //        return count;
        //    }
        //    catch (Exception e)
        //    {
        //        throw e;
        //    }

        //}
        //        public DataTable AlertDetailsViewForEscalation(int Alert_Id, string fromDate, string ToDate, int flagStatus)
        //        {
        //            SqlConnection con = new SqlConnection(cnStr);
        //            try
        //            {
        //                DataSet ds = new DataSet();
        //                DataTable dt = new DataTable();
        //                string str = "";
        //                if (Alert_Id == 23)
        //                {
        //                    if (flagStatus == 0)
        //                    {
        //                        str = @"select a.ENQ_Id as AlertDtl_EnqNo_int,a.ENQ_Date_dt as AlertDtl_EnqDate_date,'' as AlertDtl_BillNo_int,null as AlertDtl_BillDate_date,0 as AlertDtl_BillAmt_dec,
        //				'' as AlertDtl_ProNo_var,0 as AlertDtl_ProNo_int,null as AlertDtl_ProDate_date,'' as AlertDtl_RefNo_var,0 as AlertDtl_RecNo_int,
        //				c.CL_Name_var as AlertDtl_CL_Name_var,d.SITE_Name_var as AlertDtl_SITE_Name_var,
        //                e.MATERIAL_RecordType_var as AlertDtl_RecType_var
        //				 ,c.CL_Id,d.SITE_Id,23 as AlertDtl_Alert_Id
        //				,(select USER_NAME_var from tbl_User x where x.USER_Id IN (select ROUTE_ME_Id from tbl_Route where ROUTE_Id=d.SITE_Route_Id)) as AlertDtl_MEName_var
        //				from tbl_Enquiry as a,tbl_Proposal as b,tbl_Client as c,tbl_Site as d,tbl_Material as e
        //				where a.ENQ_Id=b.Proposal_EnqNo
        //				and a.ENQ_CL_Id=c.CL_Id
        //				and a.ENQ_SITE_Id=d.SITE_Id
        //				and a.ENQ_MATERIAL_Id=e.MATERIAL_Id
        //				and CONVERT (DATE, a.ENQ_Date_dt) >=CONVERT (DATE,'" + fromDate + "') and CONVERT (DATE, a.ENQ_Date_dt) <=CONVERT (DATE,'" + ToDate + "') " +
        //                @"and datediff(d,convert(date,a.ENQ_Date_dt),convert(date,b.Proposal_Date)) > 2
        //				and datediff(d,convert(date,a.ENQ_Date_dt),convert(date,GETDATE())) > 2 
        //				and b.Proposal_NewClientStatus=0 and b.Proposal_ActiveStatus_bit=0
        //				
        //				Union
        //				
        //				Select a.ENQNEW_Id as AlertDtl_EnqNo_int,a.ENQNEW_Date_dt as AlertDtl_EnqDate_date,'' as AlertDtl_BillNo_int,null as AlertDtl_BillDate_date,0 as AlertDtl_BillAmt_dec,
        //				'' as AlertDtl_ProNo_var,0 as AlertDtl_ProNo_int,null as AlertDtl_ProDate_date,'' as AlertDtl_RefNo_var,0 as AlertDtl_RecNo_int,
        //				ENQNEW_ClientName_var as AlertDtl_CL_Name_var,ENQNEW_SiteName_var as AlertDtl_SITE_Name_var, 
        //                e.MATERIAL_RecordType_var as AlertDtl_RecType_var
        //				,0 as CL_Id,0 as SITE_Id,23 as AlertDtl_Alert_Id
        //                ,(select USER_NAME_var from tbl_User x where x.USER_Id IN (select ROUTE_ME_Id from tbl_Route where ROUTE_Id=a.ENQNEW_ROUTE_Id)) as AlertDtl_MEName_var				
        //				from tbl_EnquiryNewClient as a,tbl_Proposal as b,tbl_Material as e
        //				where a.ENQNEW_Id=b.Proposal_EnqNo
        //				and a.ENQNEW_MATERIAL_Id=e.MATERIAL_Id
        //				and CONVERT (DATE, a.ENQNEW_Date_dt) >=CONVERT(DATE,'" + fromDate + "') and CONVERT (DATE, a.ENQNEW_Date_dt) <=CONVERT(DATE,'" + ToDate + "')  " +
        //                @"and datediff(d,convert(date,a.ENQNEW_Date_dt),convert(date,b.Proposal_Date)) > 2
        //				and datediff(d,convert(date,a.ENQNEW_Date_dt),convert(date,GETDATE())) > 2
        //				and b.Proposal_NewClientStatus=1  and b.Proposal_ActiveStatus_bit=0
        //                order by AlertDtl_RecType_var";
        //                    }
        //                    else
        //                    {
        //                        str = @"  select a.ENQ_Id as AlertDtl_EnqNo_int,a.ENQ_Date_dt as AlertDtl_EnqDate_date,'' as AlertDtl_BillNo_int,null as AlertDtl_BillDate_date,0 as AlertDtl_BillAmt_dec,
        //				'' as AlertDtl_ProNo_var,0 as AlertDtl_ProNo_int,null as AlertDtl_ProDate_date,'' as AlertDtl_RefNo_var,0 as AlertDtl_RecNo_int,
        //				c.CL_Name_var as AlertDtl_CL_Name_var,d.SITE_Name_var as AlertDtl_SITE_Name_var,e.MATERIAL_Name_var as AlertDtl_RecType_var,c.CL_Id,d.SITE_Id,23 as AlertDtl_Alert_Id
        //				,(select USER_NAME_var from tbl_User x where x.USER_Id IN (select ROUTE_ME_Id from tbl_Route where ROUTE_Id=d.SITE_Route_Id)) as AlertDtl_MEName_var
        //				from tbl_Enquiry as a,tbl_Proposal as b,tbl_Client as c,tbl_Site as d,tbl_Material as e
        //				where a.ENQ_Id=b.Proposal_EnqNo
        //				and a.ENQ_CL_Id=c.CL_Id
        //				and a.ENQ_SITE_Id=d.SITE_Id
        //				and a.ENQ_MATERIAL_Id=e.MATERIAL_Id
        //				and CONVERT (DATE, a.ENQ_Date_dt) >=CONVERT(DATE,'" + fromDate + "') and CONVERT (DATE, a.ENQ_Date_dt) <=CONVERT(DATE,'" + ToDate + "') " +
        //               @"and b.Proposal_Date is null and datediff(d,convert(date,a.ENQ_Date_dt),convert(date,GETDATE())) > 2 
        //				and (a.ENQ_Status_tint  < 2 or a.ENQ_Status_tint = 5) 
        //				and b.Proposal_NewClientStatus=0 and b.Proposal_ActiveStatus_bit=0
        //				
        //				Union
        //				
        //				Select a.ENQNEW_Id as AlertDtl_EnqNo_int,a.ENQNEW_Date_dt as AlertDtl_EnqDate_date,'' as AlertDtl_BillNo_int,null as AlertDtl_BillDate_date,0 as AlertDtl_BillAmt_dec,
        //				'' as AlertDtl_ProNo_var,0 as AlertDtl_ProNo_int,null as AlertDtl_ProDate_date,'' as AlertDtl_RefNo_var,0 as AlertDtl_RecNo_int,
        //				ENQNEW_ClientName_var as AlertDtl_CL_Name_var,ENQNEW_SiteName_var as AlertDtl_SITE_Name_var, 
        //                e.MATERIAL_RecordType_var as AlertDtl_RecType_var
        //				,0 as CL_Id,0 as SITE_Id,23 as AlertDtl_Alert_Id
        //               ,(select USER_NAME_var from tbl_User x where x.USER_Id IN (select ROUTE_ME_Id from tbl_Route where ROUTE_Id=a.ENQNEW_ROUTE_Id)) as AlertDtl_MEName_var				
        //				from tbl_EnquiryNewClient as a,tbl_Proposal as b,tbl_Material as e
        //				where a.ENQNEW_Id=b.Proposal_EnqNo
        //				and a.ENQNEW_MATERIAL_Id=e.MATERIAL_Id
        //				and CONVERT (DATE, a.ENQNEW_Date_dt) >=CONVERT(DATE,'" + fromDate + "') and CONVERT (DATE, a.ENQNEW_Date_dt) <=CONVERT(DATE,'" + ToDate + "')  " +
        //                @"and  b.Proposal_Date is null and datediff(d,convert(date,a.ENQNEW_Date_dt),convert(date,GETDATE())) > 2 
        //				and (a.ENQNEW_Status_tint  < 2 or a.ENQNEW_Status_tint = 5) 
        //				and b.Proposal_NewClientStatus=1  and b.Proposal_ActiveStatus_bit=0
        //                    order by AlertDtl_RecType_var";
        //                    }

        //                }
        //                else if (Alert_Id == 24)
        //                {
        //                    if (flagStatus == 0)
        //                    {
        //                        str = @"Select a.ENQ_Id as AlertDtl_EnqNo_int,a.ENQ_Date_dt as AlertDtl_EnqDate_date,'' as AlertDtl_BillNo_int,null as AlertDtl_BillDate_date,0 as AlertDtl_BillAmt_dec,
        //				b.Proposal_No as AlertDtl_ProNo_var,b.Proposal_Id,b.Proposal_Date as AlertDtl_ProDate_date,'' as AlertDtl_RefNo_var,0 as AlertDtl_RecNo_int,
        //				c.CL_Name_var as AlertDtl_CL_Name_var,d.SITE_Name_var as AlertDtl_SITE_Name_var,e.MATERIAL_Name_var as AlertDtl_RecType_var,c.CL_Id,d.SITE_Id,24 as AlertDtl_Alert_Id
        //				,(select USER_NAME_var from tbl_User x where x.USER_Id IN (select ROUTE_ME_Id from tbl_Route where ROUTE_Id=d.SITE_Route_Id)) as AlertDtl_MEName_var
        //				from tbl_Enquiry as a,tbl_Proposal as b,tbl_Client as c,tbl_Site as d,tbl_Material as e
        //				where a.ENQ_Id=b.Proposal_EnqNo
        //				and a.ENQ_CL_Id=c.CL_Id
        //				and a.ENQ_SITE_Id=d.SITE_Id
        //				and a.ENQ_MATERIAL_Id=e.MATERIAL_Id
        //				and CONVERT (DATE, Proposal_Date) >=CONVERT(DATE,'" + fromDate + "') and CONVERT (DATE, Proposal_Date) <=CONVERT(DATE,'" + ToDate + "') " +
        //                @"and datediff(d,convert(date,b.Proposal_Date),convert(date,b.Proposal_OrderDate_dt)) > 15
        //				and datediff(d,convert(date,b.Proposal_Date),convert(date,GETDATE())) > 15 and b.Proposal_NewClientStatus=0 and b.Proposal_ActiveStatus_bit=0
        //				
        //				Union
        //				
        //				Select a.ENQNEW_Id as AlertDtl_EnqNo_int,a.ENQNEW_Date_dt as AlertDtl_EnqDate_date,'' as AlertDtl_BillNo_int,null as AlertDtl_BillDate_date,0 as AlertDtl_BillAmt_dec,
        //				b.Proposal_No as AlertDtl_ProNo_var,b.Proposal_Id,b.Proposal_Date as AlertDtl_ProDate_date,'' as AlertDtl_RefNo_var,0 as AlertDtl_RecNo_int,
        //				ENQNEW_ClientName_var as AlertDtl_CL_Name_var,ENQNEW_SiteName_var as AlertDtl_SITE_Name_var, 
        //                e.MATERIAL_RecordType_var as AlertDtl_RecType_var
        //				,0 as CL_Id,0 as SITE_Id,24 as AlertDtl_Alert_Id
        //               ,(select USER_NAME_var from tbl_User x where x.USER_Id IN (select ROUTE_ME_Id from tbl_Route where ROUTE_Id=a.ENQNEW_ROUTE_Id)) as AlertDtl_MEName_var				
        //				from tbl_EnquiryNewClient as a,tbl_Proposal as b,tbl_Material as e
        //				where a.ENQNEW_Id=b.Proposal_EnqNo
        //				and a.ENQNEW_MATERIAL_Id=e.MATERIAL_Id
        //				  and CONVERT (DATE, Proposal_Date) >=CONVERT(DATE,'" + fromDate + "') and CONVERT (DATE, Proposal_Date) <=CONVERT(DATE,'" + ToDate + "') " +
        //                @"and datediff(d,convert(date,b.Proposal_Date),convert(date,b.Proposal_OrderDate_dt)) > 15
        //				and datediff(d,convert(date,b.Proposal_Date),convert(date,GETDATE())) > 15
        //                and b.Proposal_NewClientStatus=1  and b.Proposal_ActiveStatus_bit=0 order by AlertDtl_RecType_var";
        //                    }
        //                    else
        //                    {
        //                        str = @"Select a.ENQ_Id as AlertDtl_EnqNo_int,a.ENQ_Date_dt as AlertDtl_EnqDate_date,'' as AlertDtl_BillNo_int,null as AlertDtl_BillDate_date,0 as AlertDtl_BillAmt_dec,
        //				b.Proposal_No as AlertDtl_ProNo_var,b.Proposal_Id,b.Proposal_Date as AlertDtl_ProDate_date,'' as AlertDtl_RefNo_var,0 as AlertDtl_RecNo_int,
        //				c.CL_Name_var as AlertDtl_CL_Name_var,d.SITE_Name_var as AlertDtl_SITE_Name_var,e.MATERIAL_Name_var as AlertDtl_RecType_var,c.CL_Id,d.SITE_Id,24 as AlertDtl_Alert_Id
        //				,(select USER_NAME_var from tbl_User x where x.USER_Id IN (select ROUTE_ME_Id from tbl_Route where ROUTE_Id=d.SITE_Route_Id)) as AlertDtl_MEName_var
        //				from tbl_Enquiry as a,tbl_Proposal as b,tbl_Client as c,tbl_Site as d,tbl_Material as e
        //				where a.ENQ_Id=b.Proposal_EnqNo
        //				and a.ENQ_CL_Id=c.CL_Id
        //				and a.ENQ_SITE_Id=d.SITE_Id
        //				and a.ENQ_MATERIAL_Id=e.MATERIAL_Id
        //				and CONVERT (DATE, Proposal_Date) >=CONVERT(DATE,'" + fromDate + "') and CONVERT (DATE, Proposal_Date) <=CONVERT(DATE,'" + ToDate + "') " +
        //                @"and b.Proposal_OrderDate_dt is null and datediff(d,convert(date,b.Proposal_Date),convert(date,GETDATE())) > 15 
        //				and b.Proposal_NewClientStatus=0  and b.Proposal_ActiveStatus_bit=0
        //				
        //				Union
        //				
        //				Select a.ENQNEW_Id as AlertDtl_EnqNo_int,a.ENQNEW_Date_dt as AlertDtl_EnqDate_date,'' as AlertDtl_BillNo_int,null as AlertDtl_BillDate_date,0 as AlertDtl_BillAmt_dec,
        //				b.Proposal_No as AlertDtl_ProNo_var,b.Proposal_Id,b.Proposal_Date as AlertDtl_ProDate_date,'' as AlertDtl_RefNo_var,0 as AlertDtl_RecNo_int,
        //				ENQNEW_ClientName_var as AlertDtl_CL_Name_var,ENQNEW_SiteName_var as AlertDtl_SITE_Name_var, 
        //               e.MATERIAL_RecordType_var as AlertDtl_RecType_var
        //				,0 as CL_Id,0 as SITE_Id,24 as AlertDtl_Alert_Id
        //               ,(select USER_NAME_var from tbl_User x where x.USER_Id IN (select ROUTE_ME_Id from tbl_Route where ROUTE_Id=a.ENQNEW_ROUTE_Id)) as AlertDtl_MEName_var				
        //				from tbl_EnquiryNewClient as a,tbl_Proposal as b,tbl_Material as e
        //				where a.ENQNEW_Id=b.Proposal_EnqNo
        //				and a.ENQNEW_MATERIAL_Id=e.MATERIAL_Id
        //				and a.ENQNEW_MATERIAL_Id=e.MATERIAL_Id 
        //				and CONVERT (DATE, Proposal_Date) >=CONVERT(DATE,'" + fromDate + "') and CONVERT (DATE, Proposal_Date) <=CONVERT(DATE,'" + ToDate + "') " +
        //               @"and b.Proposal_OrderDate_dt is null and datediff(d,convert(date,b.Proposal_Date),convert(date,GETDATE())) > 15 
        //				and b.Proposal_NewClientStatus=1 and b.Proposal_ActiveStatus_bit=0 order by AlertDtl_RecType_var";
        //                    }
        //                }
        //                else if (Alert_Id == 25)
        //                {
        //                    if (flagStatus == 0)
        //                    {
        //                        str = @"Select a.ENQ_Id as AlertDtl_EnqNo_int,a.ENQ_Date_dt as AlertDtl_EnqDate_date,'' as AlertDtl_BillNo_int,null as AlertDtl_BillDate_date,0 as AlertDtl_BillAmt_dec,
        //				'' as AlertDtl_ProNo_var,0 as AlertDtl_ProNo_int,null as AlertDtl_ProDate_date,'' as AlertDtl_RefNo_var,0 as AlertDtl_RecNo_int,
        //				c.CL_Name_var as AlertDtl_CL_Name_var,d.SITE_Name_var as AlertDtl_SITE_Name_var,e.MATERIAL_Name_var as AlertDtl_RecType_var,c.CL_Id,d.SITE_Id,25 as AlertDtl_Alert_Id
        //			   ,(select USER_NAME_var from tbl_User x where x.USER_Id IN (select ROUTE_ME_Id from tbl_Route where ROUTE_Id=d.SITE_Route_Id)) as AlertDtl_MEName_var
        //				from tbl_Enquiry as a,tbl_Proposal as b,tbl_Client as c,tbl_Site as d,tbl_Material as e
        //				where a.ENQ_Id=b.Proposal_EnqNo
        //				and a.ENQ_CL_Id=c.CL_Id
        //				and a.ENQ_SITE_Id=d.SITE_Id
        //				and a.ENQ_MATERIAL_Id=e.MATERIAL_Id
        //				and CONVERT (DATE, b.Proposal_OrderDate_dt) >=CONVERT(DATE,'" + fromDate + "') and CONVERT (DATE, b.Proposal_OrderDate_dt) <=CONVERT(DATE,'" + ToDate + "') " +
        //                @"and datediff(d,convert(date,b.Proposal_OrderDate_dt),convert(date,a.ENQ_CollectionDate_dt)) > 3
        //				and datediff(d,convert(date,b.Proposal_OrderDate_dt),convert(date,GETDATE())) > 3 
        //                and ENQ_Status_tint=5    
        //                and b.Proposal_NewClientStatus=0 and b.Proposal_ActiveStatus_bit=0
        //                 
        //                Union 
        //                
        //                 Select a.ENQNEW_Id as AlertDtl_EnqNo_int,a.ENQNEW_Date_dt as AlertDtl_EnqDate_date,'' as AlertDtl_BillNo_int,null as AlertDtl_BillDate_date,0 as AlertDtl_BillAmt_dec,
        //				b.Proposal_No as AlertDtl_ProNo_var,b.Proposal_Id,b.Proposal_Date as AlertDtl_ProDate_date,'' as AlertDtl_RefNo_var,0 as AlertDtl_RecNo_int,
        //				ENQNEW_ClientName_var as AlertDtl_CL_Name_var,ENQNEW_SiteName_var as AlertDtl_SITE_Name_var, 
        //                e.MATERIAL_RecordType_var as AlertDtl_RecType_var
        //				,0 as CL_Id,0 as SITE_Id,25 as AlertDtl_Alert_Id
        //               ,(select USER_NAME_var from tbl_User x where x.USER_Id IN (select ROUTE_ME_Id from tbl_Route where ROUTE_Id=a.ENQNEW_ROUTE_Id)) as AlertDtl_MEName_var				
        //				from tbl_EnquiryNewClient as a,tbl_Proposal as b,tbl_Material as e
        //				where a.ENQNEW_Id=b.Proposal_EnqNo
        //				and a.ENQNEW_MATERIAL_Id=e.MATERIAL_Id
        //				and b.Proposal_ActiveStatus_bit=0
        //                and b.Proposal_NewClientStatus=1
        //                and CONVERT (DATE, b.Proposal_OrderDate_dt) >=CONVERT(DATE,'" + fromDate + "') and CONVERT (DATE, b.Proposal_OrderDate_dt) <=CONVERT(DATE,'" + ToDate + "') " +
        //                @" and datediff(d,convert(date,b.Proposal_OrderDate_dt),convert(date,GETDATE())) > 3 
        //                and datediff(d,convert(date,b.Proposal_OrderDate_dt),convert(date,GETDATE())) > 3
        //               and ENQNEW_Status_tint=5
        //                order by AlertDtl_RecType_var";
        //                    }
        //                    else
        //                    {
        //                        str = @"Select a.ENQ_Id as AlertDtl_EnqNo_int,a.ENQ_Date_dt as AlertDtl_EnqDate_date,'' as AlertDtl_BillNo_int,null as AlertDtl_BillDate_date,0 as AlertDtl_BillAmt_dec,
        //				'' as AlertDtl_ProNo_var,0 as AlertDtl_ProNo_int,null as AlertDtl_ProDate_date,'' as AlertDtl_RefNo_var,0 as AlertDtl_RecNo_int,
        //				c.CL_Name_var as AlertDtl_CL_Name_var,d.SITE_Name_var as AlertDtl_SITE_Name_var,e.MATERIAL_Name_var as AlertDtl_RecType_var,c.CL_Id,d.SITE_Id,25 as AlertDtl_Alert_Id
        //			   ,(select USER_NAME_var from tbl_User x where x.USER_Id IN (select ROUTE_ME_Id from tbl_Route where ROUTE_Id=d.SITE_Route_Id)) as AlertDtl_MEName_var
        //				from tbl_Enquiry as a,tbl_Proposal as b,tbl_Client as c,tbl_Site as d,tbl_Material as e
        //				where a.ENQ_Id=b.Proposal_EnqNo
        //				and a.ENQ_CL_Id=c.CL_Id
        //				and a.ENQ_SITE_Id=d.SITE_Id
        //				and a.ENQ_MATERIAL_Id=e.MATERIAL_Id
        //				and CONVERT (DATE, b.Proposal_OrderDate_dt) >=CONVERT(DATE,'" + fromDate + "') and CONVERT (DATE, b.Proposal_OrderDate_dt) <=CONVERT(DATE,'" + ToDate + "') " +
        //                @"and a.ENQ_CollectionDate_dt is null 
        //                and datediff(d,convert(date,b.Proposal_OrderDate_dt),convert(date,GETDATE())) > 3
        //                and ENQ_Status_tint=5   and b.Proposal_NewClientStatus=0 and b.Proposal_ActiveStatus_bit=0
        //                   
        //                Union 
        //                
        //                 Select a.ENQNEW_Id as AlertDtl_EnqNo_int,a.ENQNEW_Date_dt as AlertDtl_EnqDate_date,'' as AlertDtl_BillNo_int,null as AlertDtl_BillDate_date,0 as AlertDtl_BillAmt_dec,
        //				b.Proposal_No as AlertDtl_ProNo_var,b.Proposal_Id,b.Proposal_Date as AlertDtl_ProDate_date,'' as AlertDtl_RefNo_var,0 as AlertDtl_RecNo_int,
        //				ENQNEW_ClientName_var as AlertDtl_CL_Name_var,ENQNEW_SiteName_var as AlertDtl_SITE_Name_var, 
        //               e.MATERIAL_RecordType_var as AlertDtl_RecType_var
        //				,0 as CL_Id,0 as SITE_Id,25 as AlertDtl_Alert_Id
        //               ,(select USER_NAME_var from tbl_User x where x.USER_Id IN (select ROUTE_ME_Id from tbl_Route where ROUTE_Id=a.ENQNEW_ROUTE_Id)) as AlertDtl_MEName_var				
        //			from tbl_EnquiryNewClient as a,tbl_Proposal as b,tbl_Material as e
        //				where a.ENQNEW_Id=b.Proposal_EnqNo
        //				and a.ENQNEW_MATERIAL_Id=e.MATERIAL_Id
        //					and b.Proposal_ActiveStatus_bit=0
        //                and b.Proposal_NewClientStatus=1
        //                and CONVERT (DATE, b.Proposal_OrderDate_dt) >=CONVERT(DATE,'" + fromDate + "') and CONVERT (DATE, b.Proposal_OrderDate_dt) <=CONVERT(DATE,'" + ToDate + "') " +
        //                @"and Proposal_OrderNo_var is not null
        //				and a.ENQNEW_CollectionDate_dt is null 
        //                and datediff(d,convert(date,b.Proposal_OrderDate_dt),convert(date,GETDATE())) > 3 
        //                and ENQNEW_Status_tint=5
        //                order by AlertDtl_RecType_var";
        //                    }
        //                }
        //                else if (Alert_Id == 26)
        //                {
        //                    if (flagStatus == 0)
        //                    {
        //                        str = @"Select a.ENQ_Id as AlertDtl_EnqNo_int,a.ENQ_Date_dt as AlertDtl_EnqDate_date,'' as AlertDtl_BillNo_int,null as AlertDtl_BillDate_date,0 as AlertDtl_BillAmt_dec,
        //				'' as AlertDtl_ProNo_var,0 as AlertDtl_ProNo_int,null as AlertDtl_ProDate_date,'' as AlertDtl_RefNo_var,0 as AlertDtl_RecNo_int,
        //				c.CL_Name_var as AlertDtl_CL_Name_var,d.SITE_Name_var as AlertDtl_SITE_Name_var,e.MATERIAL_Name_var as AlertDtl_RecType_var,c.CL_Id,d.SITE_Id,26 as AlertDtl_Alert_Id
        //				,(select USER_NAME_var from tbl_User x where x.USER_Id IN (select ROUTE_ME_Id from tbl_Route where ROUTE_Id=d.SITE_Route_Id)) as AlertDtl_MEName_var
        //				from tbl_Enquiry as a,tbl_Inward as b,tbl_Client as c,tbl_Site as d,tbl_Material as e
        //				where a.ENQ_Id=b.INWD_ENQ_Id
        //				and a.ENQ_CL_Id=c.CL_Id
        //				and a.ENQ_SITE_Id=d.SITE_Id
        //				and a.ENQ_MATERIAL_Id=e.MATERIAL_Id
        //				and CONVERT (DATE, ENQ_CollectionDate_dt) >=CONVERT(DATE,'" + fromDate + "') and CONVERT (DATE, ENQ_CollectionDate_dt) <=CONVERT(DATE,'" + ToDate + "') " +
        //               @"and datediff(d,convert(date,a.ENQ_CollectionDate_dt),convert(date,b.INWD_ReceivedDate_dt)) > 2
        //				and datediff(d,convert(date,a.ENQ_CollectionDate_dt),convert(date,GETDATE())) > 2 
        //                 order by AlertDtl_RecType_var";
        //                    }
        //                    else
        //                    {
        //                        str = @"Select a.ENQ_Id as AlertDtl_EnqNo_int,a.ENQ_Date_dt as AlertDtl_EnqDate_date,'' as AlertDtl_BillNo_int,null as AlertDtl_BillDate_date,0 as AlertDtl_BillAmt_dec,
        //				'' as AlertDtl_ProNo_var,0 as AlertDtl_ProNo_int,null as AlertDtl_ProDate_date,'' as AlertDtl_RefNo_var,0 as AlertDtl_RecNo_int,
        //				c.CL_Name_var as AlertDtl_CL_Name_var,d.SITE_Name_var as AlertDtl_SITE_Name_var,e.MATERIAL_Name_var as AlertDtl_RecType_var,c.CL_Id,d.SITE_Id,26 as AlertDtl_Alert_Id
        //				,(select USER_NAME_var from tbl_User x where x.USER_Id IN (select ROUTE_ME_Id from tbl_Route where ROUTE_Id=d.SITE_Route_Id)) as AlertDtl_MEName_var
        //				from tbl_Enquiry as a,tbl_Inward as b,tbl_Client as c,tbl_Site as d,tbl_Material as e
        //				where a.ENQ_Id=b.INWD_ENQ_Id
        //				and a.ENQ_CL_Id=c.CL_Id
        //				and a.ENQ_SITE_Id=d.SITE_Id
        //				and a.ENQ_MATERIAL_Id=e.MATERIAL_Id
        //				and CONVERT (DATE, ENQ_CollectionDate_dt) >=CONVERT(DATE,'" + fromDate + "') and CONVERT (DATE, ENQ_CollectionDate_dt) <=CONVERT(DATE,'" + ToDate + "') " +
        //                 @"and b.INWD_ReceivedDate_dt is null 
        //                and datediff(d,convert(date,a.ENQ_CollectionDate_dt),convert(date,GETDATE())) > 2 
        //                and a.ENQ_Status_tint <> 2 order by AlertDtl_RecType_var";
        //                    }
        //                }

        //                else if (Alert_Id == 27)
        //                {
        //                    if (flagStatus == 0)
        //                    {
        //                        str = @"Select a.ENQ_Id as AlertDtl_EnqNo_int,a.ENQ_Date_dt as AlertDtl_EnqDate_date,'' as AlertDtl_BillNo_int,null as AlertDtl_BillDate_date,0 as AlertDtl_BillAmt_dec,
        //				'' as AlertDtl_ProNo_var,0 as AlertDtl_ProNo_int,null as AlertDtl_ProDate_date,MISRefNo as AlertDtl_RefNo_var,MISRecordNo  as AlertDtl_RecNo_int,
        //				c.CL_Name_var as AlertDtl_CL_Name_var,d.SITE_Name_var as AlertDtl_SITE_Name_var,e.MATERIAL_Name_var as AlertDtl_RecType_var,c.CL_Id,d.SITE_Id,27 as AlertDtl_Alert_Id
        //				,(select USER_NAME_var from tbl_User x where x.USER_Id IN (select ROUTE_ME_Id from tbl_Route where ROUTE_Id=d.SITE_Route_Id)) as AlertDtl_MEName_var
        //				from tbl_Enquiry as a,tbl_Inward as b,tbl_Client as c,tbl_Site as d,tbl_Material as e,tbl_MISDetail as f
        //				where a.ENQ_Id=b.INWD_ENQ_Id
        //				and b.INWD_RecordNo_int=f.MISRecordNo
        //				and b.INWD_RecordType_var=f.MISRecType
        //				and a.ENQ_CL_Id=c.CL_Id
        //				and a.ENQ_SITE_Id=d.SITE_Id
        //				and a.ENQ_MATERIAL_Id=e.MATERIAL_Id
        //				and CONVERT (DATE, b.INWD_ReceivedDate_dt) >=CONVERT(DATE,'" + fromDate + "') and CONVERT (DATE,b.INWD_ReceivedDate_dt) <=CONVERT(DATE,'" + ToDate + "') " +
        //                @"and datediff(d,convert(date,b.INWD_ReceivedDate_dt),convert(date,f.MISEnteredDt)) > 2
        //				and datediff(d,convert(date,b.INWD_ReceivedDate_dt),convert(date,GETDATE())) > 2 
        //                 order by AlertDtl_RecType_var";
        //                    }
        //                    else
        //                    {
        //                        str = @"Select a.ENQ_Id as AlertDtl_EnqNo_int,a.ENQ_Date_dt as AlertDtl_EnqDate_date,'' as AlertDtl_BillNo_int,null as AlertDtl_BillDate_date,0 as AlertDtl_BillAmt_dec,
        //				'' as AlertDtl_ProNo_var,0 as AlertDtl_ProNo_int,null as AlertDtl_ProDate_date,MISRefNo as AlertDtl_RefNo_var,MISRecordNo  as AlertDtl_RecNo_int,
        //				c.CL_Name_var as AlertDtl_CL_Name_var,d.SITE_Name_var as AlertDtl_SITE_Name_var,e.MATERIAL_Name_var as AlertDtl_RecType_var,c.CL_Id,d.SITE_Id,27 as AlertDtl_Alert_Id
        //				,(select USER_NAME_var from tbl_User x where x.USER_Id IN (select ROUTE_ME_Id from tbl_Route where ROUTE_Id=d.SITE_Route_Id)) as AlertDtl_MEName_var
        //				from tbl_Enquiry as a,tbl_Inward as b,tbl_Client as c,tbl_Site as d,tbl_Material as e,tbl_MISDetail as f
        //				where a.ENQ_Id=b.INWD_ENQ_Id
        //				and b.INWD_RecordNo_int=f.MISRecordNo
        //				and b.INWD_RecordType_var=f.MISRecType
        //				and a.ENQ_CL_Id=c.CL_Id
        //				and a.ENQ_SITE_Id=d.SITE_Id
        //				and a.ENQ_MATERIAL_Id=e.MATERIAL_Id
        //				and CONVERT (DATE, b.INWD_ReceivedDate_dt) >=CONVERT(DATE,'" + fromDate + "') and CONVERT (DATE,b.INWD_ReceivedDate_dt) <=CONVERT(DATE,'" + ToDate + "') " +
        //                @"and f.MISEnteredDt is null 
        //                and datediff(d,convert(date,b.INWD_ReceivedDate_dt),convert(date,GETDATE())) > 2 
        //				order by AlertDtl_RecType_var";
        //                    }
        //                }
        //                else if (Alert_Id == 30)
        //                {
        //                    if (flagStatus == 0)
        //                    {
        //                        str = @"Select a.ENQ_Id as AlertDtl_EnqNo_int,a.ENQ_Date_dt as AlertDtl_EnqDate_date,'' as AlertDtl_BillNo_int,null as AlertDtl_BillDate_date,0 as AlertDtl_BillAmt_dec,
        //				'' as AlertDtl_ProNo_var,0 as AlertDtl_ProNo_int,null as AlertDtl_ProDate_date,MISRefNo as AlertDtl_RefNo_var,MISRecordNo  as AlertDtl_RecNo_int,
        //				c.CL_Name_var as AlertDtl_CL_Name_var,d.SITE_Name_var as AlertDtl_SITE_Name_var,e.MATERIAL_Name_var as AlertDtl_RecType_var,c.CL_Id,d.SITE_Id,30 as AlertDtl_Alert_Id
        //				,(select USER_NAME_var from tbl_User x where x.USER_Id IN (select ROUTE_ME_Id from tbl_Route where ROUTE_Id=d.SITE_Route_Id)) as AlertDtl_MEName_var
        //				from tbl_Enquiry as a,tbl_Inward as b,tbl_Client as c,tbl_Site as d,tbl_Material as e,tbl_MISDetail as f
        //				where a.ENQ_Id=b.INWD_ENQ_Id
        //				and b.INWD_RecordNo_int=f.MISRecordNo
        //				and b.INWD_RecordType_var=f.MISRecType
        //				and a.ENQ_CL_Id=c.CL_Id
        //				and a.ENQ_SITE_Id=d.SITE_Id
        //				and a.ENQ_MATERIAL_Id=e.MATERIAL_Id
        //				and CONVERT (DATE, f.MISEnteredDt) >=CONVERT(DATE,'" + fromDate + "') and CONVERT (DATE, f.MISEnteredDt) <=CONVERT(DATE,'" + ToDate + "')  " +
        //                @"and datediff(d,convert(date,f.MISEnteredDt),convert(date,f.MISCheckedDt)) > 1
        //				and datediff(d,convert(date,f.MISEnteredDt),convert(date,GETDATE())) > 1 
        //                 order by AlertDtl_RecType_var";
        //                    }
        //                    else
        //                    {
        //                        str = @"Select a.ENQ_Id as AlertDtl_EnqNo_int,a.ENQ_Date_dt as AlertDtl_EnqDate_date,'' as AlertDtl_BillNo_int,null as AlertDtl_BillDate_date,0 as AlertDtl_BillAmt_dec,
        //				'' as AlertDtl_ProNo_var,0 as AlertDtl_ProNo_int,null as AlertDtl_ProDate_date,MISRefNo as AlertDtl_RefNo_var,MISRecordNo  as AlertDtl_RecNo_int,
        //				c.CL_Name_var as AlertDtl_CL_Name_var,d.SITE_Name_var as AlertDtl_SITE_Name_var,e.MATERIAL_Name_var as AlertDtl_RecType_var,c.CL_Id,d.SITE_Id,30 as AlertDtl_Alert_Id
        //				,(select USER_NAME_var from tbl_User x where x.USER_Id IN (select ROUTE_ME_Id from tbl_Route where ROUTE_Id=d.SITE_Route_Id)) as AlertDtl_MEName_var
        //				from tbl_Enquiry as a,tbl_Inward as b,tbl_Client as c,tbl_Site as d,tbl_Material as e,tbl_MISDetail as f
        //				where a.ENQ_Id=b.INWD_ENQ_Id
        //				and b.INWD_RecordNo_int=f.MISRecordNo
        //				and b.INWD_RecordType_var=f.MISRecType
        //				and a.ENQ_CL_Id=c.CL_Id
        //				and a.ENQ_SITE_Id=d.SITE_Id
        //				and a.ENQ_MATERIAL_Id=e.MATERIAL_Id
        //				and CONVERT (DATE, f.MISEnteredDt) >=CONVERT(DATE,'" + fromDate + "') and CONVERT (DATE, f.MISEnteredDt) <=CONVERT(DATE,'" + ToDate + "')  " +
        //                @"and f.MISCheckedDt is null and datediff(d,convert(date,f.MISEnteredDt),convert(date,GETDATE())) > 1 
        //				order by AlertDtl_RecType_var";
        //                    }
        //                }
        //                else if (Alert_Id == 31)
        //                {
        //                    if (flagStatus == 0)
        //                    {
        //                        str = @"Select a.ENQ_Id as AlertDtl_EnqNo_int,a.ENQ_Date_dt as AlertDtl_EnqDate_date,'' as AlertDtl_BillNo_int,null as AlertDtl_BillDate_date,0 as AlertDtl_BillAmt_dec,
        //				'' as AlertDtl_ProNo_var,0 as AlertDtl_ProNo_int,null as AlertDtl_ProDate_date,MISRefNo as AlertDtl_RefNo_var,MISRecordNo  as AlertDtl_RecNo_int,
        //				c.CL_Name_var as AlertDtl_CL_Name_var,d.SITE_Name_var as AlertDtl_SITE_Name_var,e.MATERIAL_Name_var as AlertDtl_RecType_var,c.CL_Id,d.SITE_Id,31 as AlertDtl_Alert_Id
        //				,(select USER_NAME_var from tbl_User x where x.USER_Id IN (select ROUTE_ME_Id from tbl_Route where ROUTE_Id=d.SITE_Route_Id)) as AlertDtl_MEName_var
        //				from tbl_Enquiry as a,tbl_Inward as b,tbl_Client as c,tbl_Site as d,tbl_Material as e,tbl_MISDetail as f
        //				where a.ENQ_Id=b.INWD_ENQ_Id
        //				and b.INWD_RecordNo_int=f.MISRecordNo
        //				and b.INWD_RecordType_var=f.MISRecType
        //				and a.ENQ_CL_Id=c.CL_Id
        //				and a.ENQ_SITE_Id=d.SITE_Id
        //				and a.ENQ_MATERIAL_Id=e.MATERIAL_Id
        //				and CONVERT (DATE, f.MISEnteredDt) >=CONVERT(DATE,'" + fromDate + "') and CONVERT (DATE, f.MISEnteredDt) <=CONVERT(DATE,'" + ToDate + "')  " +
        //                @"and datediff(d,convert(date,f.MISEnteredDt),convert(date,f.MISApprovedDt)) > 1
        //				and datediff(d,convert(date,f.MISEnteredDt),convert(date,GETDATE())) > 1  order by AlertDtl_RecType_var";
        //                    }
        //                    else
        //                    {
        //                        str = @"Select a.ENQ_Id as AlertDtl_EnqNo_int,a.ENQ_Date_dt as AlertDtl_EnqDate_date,'' as AlertDtl_BillNo_int,null as AlertDtl_BillDate_date,0 as AlertDtl_BillAmt_dec,
        //				'' as AlertDtl_ProNo_var,0 as AlertDtl_ProNo_int,null as AlertDtl_ProDate_date,MISRefNo as AlertDtl_RefNo_var,MISRecordNo  as AlertDtl_RecNo_int,
        //				c.CL_Name_var as AlertDtl_CL_Name_var,d.SITE_Name_var as AlertDtl_SITE_Name_var,e.MATERIAL_Name_var as AlertDtl_RecType_var,c.CL_Id,d.SITE_Id,31 as AlertDtl_Alert_Id
        //				,(select USER_NAME_var from tbl_User x where x.USER_Id IN (select ROUTE_ME_Id from tbl_Route where ROUTE_Id=d.SITE_Route_Id)) as AlertDtl_MEName_var
        //				from tbl_Enquiry as a,tbl_Inward as b,tbl_Client as c,tbl_Site as d,tbl_Material as e,tbl_MISDetail as f
        //				where a.ENQ_Id=b.INWD_ENQ_Id
        //				and b.INWD_RecordNo_int=f.MISRecordNo
        //				and b.INWD_RecordType_var=f.MISRecType
        //				and a.ENQ_CL_Id=c.CL_Id
        //				and a.ENQ_SITE_Id=d.SITE_Id
        //				and a.ENQ_MATERIAL_Id=e.MATERIAL_Id
        //			    and CONVERT (DATE, f.MISEnteredDt) >=CONVERT(DATE,'" + fromDate + "') and CONVERT (DATE, f.MISEnteredDt) <=CONVERT(DATE,'" + ToDate + "')  " +
        //                 @"and f.MISApprovedDt is null 
        //                and datediff(d,convert(date,f.MISEnteredDt),convert(date,GETDATE())) > 1 
        //				 order by AlertDtl_RecType_var";
        //                    }
        //                }
        //                else if (Alert_Id == 32)
        //                {
        //                    if (flagStatus == 0)
        //                    {
        //                        str = @"Select a.ENQ_Id as AlertDtl_EnqNo_int,a.ENQ_Date_dt as AlertDtl_EnqDate_date,'' as AlertDtl_BillNo_int,null as AlertDtl_BillDate_date,0 as AlertDtl_BillAmt_dec,
        //				'' as AlertDtl_ProNo_var,0 as AlertDtl_ProNo_int,null as AlertDtl_ProDate_date,MISRefNo as AlertDtl_RefNo_var,MISRecordNo  as AlertDtl_RecNo_int,
        //				c.CL_Name_var as AlertDtl_CL_Name_var,d.SITE_Name_var as AlertDtl_SITE_Name_var,e.MATERIAL_Name_var as AlertDtl_RecType_var,c.CL_Id,d.SITE_Id,32 as AlertDtl_Alert_Id
        //				,(select USER_NAME_var from tbl_User x where x.USER_Id IN (select ROUTE_ME_Id from tbl_Route where ROUTE_Id=d.SITE_Route_Id)) as AlertDtl_MEName_var
        //				from tbl_Enquiry as a,tbl_Inward as b,tbl_Client as c,tbl_Site as d,tbl_Material as e,tbl_MISDetail as f,tbl_Bill as g
        //				where a.ENQ_Id=b.INWD_ENQ_Id
        //				and b.INWD_BILL_Id=g.BILL_Id
        //				and b.INWD_RecordNo_int=f.MISRecordNo
        //				and b.INWD_RecordType_var=f.MISRecType
        //				and a.ENQ_CL_Id=c.CL_Id
        //				and a.ENQ_SITE_Id=d.SITE_Id
        //				and a.ENQ_MATERIAL_Id=e.MATERIAL_Id
        //				and CONVERT (DATE, f.MISApprovedDt) >=CONVERT(DATE,'" + fromDate + "') and CONVERT (DATE, f.MISApprovedDt) <=CONVERT(DATE,'" + ToDate + "')  " +
        //                @" and datediff(d,convert(date,f.MISApprovedDt),convert(date,g.BILL_Date_dt)) > 1
        //				and datediff(d,convert(date,f.MISApprovedDt),convert(date,GETDATE())) > 1 
        //				order by AlertDtl_RecType_var";
        //                    }
        //                    else
        //                    {
        //                        str = @"Select a.ENQ_Id as AlertDtl_EnqNo_int,a.ENQ_Date_dt as AlertDtl_EnqDate_date,'' as AlertDtl_BillNo_int,null as AlertDtl_BillDate_date,0 as AlertDtl_BillAmt_dec,
        //				'' as AlertDtl_ProNo_var,0 as AlertDtl_ProNo_int,null as AlertDtl_ProDate_date,MISRefNo as AlertDtl_RefNo_var,MISRecordNo  as AlertDtl_RecNo_int,
        //				c.CL_Name_var as AlertDtl_CL_Name_var,d.SITE_Name_var as AlertDtl_SITE_Name_var,e.MATERIAL_Name_var as AlertDtl_RecType_var,c.CL_Id,d.SITE_Id,32 as AlertDtl_Alert_Id
        //				,(select USER_NAME_var from tbl_User x where x.USER_Id IN (select ROUTE_ME_Id from tbl_Route where ROUTE_Id=d.SITE_Route_Id)) as AlertDtl_MEName_var
        //				from tbl_Enquiry as a,tbl_Inward as b,tbl_Client as c,tbl_Site as d,tbl_Material as e,tbl_MISDetail as f,tbl_Bill as g
        //				where a.ENQ_Id=b.INWD_ENQ_Id
        //				and b.INWD_BILL_Id=g.BILL_Id
        //				and b.INWD_RecordNo_int=f.MISRecordNo
        //				and b.INWD_RecordType_var=f.MISRecType
        //				and a.ENQ_CL_Id=c.CL_Id
        //				and a.ENQ_SITE_Id=d.SITE_Id
        //				and a.ENQ_MATERIAL_Id=e.MATERIAL_Id
        //				 and CONVERT (DATE, f.MISApprovedDt) >=CONVERT(DATE,'" + fromDate + "') and CONVERT (DATE, f.MISApprovedDt) <=CONVERT(DATE,'" + ToDate + "')  " +
        //                @"and g.BILL_Date_dt is null
        //                and datediff(d,convert(date,f.MISApprovedDt),convert(date,GETDATE())) > 1
        //				order by AlertDtl_RecType_var";
        //                    }
        //                }
        //                else if (Alert_Id == 33)
        //                {
        //                    if (flagStatus == 0)
        //                    {
        //                        str = @"Select a.ENQ_Id as AlertDtl_EnqNo_int,a.ENQ_Date_dt as AlertDtl_EnqDate_date,'' as AlertDtl_BillNo_int,null as AlertDtl_BillDate_date,0 as AlertDtl_BillAmt_dec,
        //				'' as AlertDtl_ProNo_var,0 as AlertDtl_ProNo_int,null as AlertDtl_ProDate_date,f.MISRefNo as AlertDtl_RefNo_var,f.MISRecordNo  as AlertDtl_RecNo_int,
        //				c.CL_Name_var as AlertDtl_CL_Name_var,d.SITE_Name_var as AlertDtl_SITE_Name_var,e.MATERIAL_Name_var as AlertDtl_RecType_var,c.CL_Id,d.SITE_Id,33 as AlertDtl_Alert_Id
        //				,(select USER_NAME_var from tbl_User x where x.USER_Id IN (select ROUTE_ME_Id from tbl_Route where ROUTE_Id=d.SITE_Route_Id)) as AlertDtl_MEName_var
        //				from tbl_Enquiry as a,tbl_Inward as b,tbl_Client as c,tbl_Site as d,tbl_Material as e,tbl_MISDetail as f
        //				where a.ENQ_Id=b.INWD_ENQ_Id
        //				and b.INWD_RecordNo_int=f.MISRecordNo
        //				and b.INWD_RecordType_var=f.MISRecType
        //				and a.ENQ_CL_Id=c.CL_Id
        //				and a.ENQ_SITE_Id=d.SITE_Id
        //				and a.ENQ_MATERIAL_Id=e.MATERIAL_Id
        //				and CONVERT (DATE, f.MISApprovedDt) >=CONVERT(DATE,'" + fromDate + "') and CONVERT (DATE, f.MISApprovedDt) <=CONVERT(DATE,'" + ToDate + "')  " +
        //                @"and datediff(d,convert(date,f.MISApprovedDt),convert(date,f.MISIssueDt)) > 1
        //				and datediff(d,convert(date,f.MISApprovedDt),convert(date,GETDATE())) > 1 
        //				order by AlertDtl_RecType_var";
        //                    }
        //                    else
        //                    {
        //                        str = @"Select a.ENQ_Id as AlertDtl_EnqNo_int,a.ENQ_Date_dt as AlertDtl_EnqDate_date,'' as AlertDtl_BillNo_int,null as AlertDtl_BillDate_date,0 as AlertDtl_BillAmt_dec,
        //				'' as AlertDtl_ProNo_var,0 as AlertDtl_ProNo_int,null as AlertDtl_ProDate_date,f.MISRefNo as AlertDtl_RefNo_var,f.MISRecordNo  as AlertDtl_RecNo_int,
        //				c.CL_Name_var as AlertDtl_CL_Name_var,d.SITE_Name_var as AlertDtl_SITE_Name_var,e.MATERIAL_Name_var as AlertDtl_RecType_var,c.CL_Id,d.SITE_Id,33 as AlertDtl_Alert_Id
        //				,(select USER_NAME_var from tbl_User x where x.USER_Id IN (select ROUTE_ME_Id from tbl_Route where ROUTE_Id=d.SITE_Route_Id)) as AlertDtl_MEName_var
        //				from tbl_Enquiry as a,tbl_Inward as b,tbl_Client as c,tbl_Site as d,tbl_Material as e,tbl_MISDetail as f
        //				where a.ENQ_Id=b.INWD_ENQ_Id
        //				and b.INWD_RecordNo_int=f.MISRecordNo
        //				and b.INWD_RecordType_var=f.MISRecType
        //				and a.ENQ_CL_Id=c.CL_Id
        //				and a.ENQ_SITE_Id=d.SITE_Id
        //				and a.ENQ_MATERIAL_Id=e.MATERIAL_Id
        //				and CONVERT (DATE, f.MISApprovedDt) >=CONVERT(DATE,'" + fromDate + "') and CONVERT (DATE, f.MISApprovedDt) <=CONVERT(DATE,'" + ToDate + "')  " +
        //                @"and f.MISIssueDt is null and datediff(d,convert(date,f.MISApprovedDt),convert(date,GETDATE())) > 1 
        //				order by AlertDtl_RecType_var";
        //                    }
        //                }
        //                else if (Alert_Id == 34)
        //                {
        //                    if (flagStatus == 0)
        //                    {
        //                        str = @"Select a.ENQ_Id as AlertDtl_EnqNo_int,a.ENQ_Date_dt as AlertDtl_EnqDate_date,'' as AlertDtl_BillNo_int,null as AlertDtl_BillDate_date,0 as AlertDtl_BillAmt_dec,
        //				'' as AlertDtl_ProNo_var,0 as AlertDtl_ProNo_int,null as AlertDtl_ProDate_date,f.MISRefNo as AlertDtl_RefNo_var,f.MISRecordNo  as AlertDtl_RecNo_int,
        //				c.CL_Name_var as AlertDtl_CL_Name_var,d.SITE_Name_var as AlertDtl_SITE_Name_var,e.MATERIAL_Name_var as AlertDtl_RecType_var,c.CL_Id,d.SITE_Id,34 as AlertDtl_Alert_Id
        //				,(select USER_NAME_var from tbl_User x where x.USER_Id IN (select ROUTE_ME_Id from tbl_Route where ROUTE_Id=d.SITE_Route_Id)) as AlertDtl_MEName_var
        //				from tbl_Enquiry as a,tbl_Inward as b,tbl_Client as c,tbl_Site as d,tbl_Material as e,tbl_MISDetail as f
        //				where a.ENQ_Id=b.INWD_ENQ_Id
        //				and b.INWD_RecordNo_int=f.MISRecordNo
        //				and b.INWD_RecordType_var=f.MISRecType
        //				and a.ENQ_CL_Id=c.CL_Id
        //				and a.ENQ_SITE_Id=d.SITE_Id
        //				and a.ENQ_MATERIAL_Id=e.MATERIAL_Id
        //				and CONVERT (DATE, f.MISApprovedDt) >=CONVERT(DATE,'" + fromDate + "') and CONVERT (DATE, f.MISApprovedDt) <=CONVERT(DATE,'" + ToDate + "')  " +
        //                @"and datediff(d,convert(date,f.MISApprovedDt),convert(date,f.MISIssueDt)) <=3
        //				and datediff(d,convert(date,f.MISApprovedDt),convert(date,GETDATE())) <=3 order by AlertDtl_RecType_var";
        //                    }
        //                    else
        //                    {
        //                        str = @"Select a.ENQ_Id as AlertDtl_EnqNo_int,a.ENQ_Date_dt as AlertDtl_EnqDate_date,'' as AlertDtl_BillNo_int,null  as AlertDtl_BillDate_date,
        //                    0 as AlertDtl_BillAmt_dec,
        //				'' as AlertDtl_ProNo_var as AlertDtl_ProNo_var ,0 as AlertDtl_ProNo_int ,null as AlertDtl_ProDate_date asAlertDtl_ProDate_date,
        //                f.MISRefNo as AlertDtl_RefNo_var,f.MISRecordNo as AlertDtl_RecNo_int,
        //				c.CL_Name_var as AlertDtl_CL_Name_var as AlertDtl_CL_Name_var,d.SITE_Name_var as AlertDtl_SITE_Name_var as AlertDtl_SITE_Name_var,e.MATERIAL_Name_var as AlertDtl_RecType_var,c.CL_Id,d.SITE_Id,34 as AlertDtl_Alert_Id
        //				,(select USER_NAME_var from tbl_User x where x.USER_Id IN (select ROUTE_ME_Id from tbl_Route where ROUTE_Id=d.SITE_Route_Id)) as AlertDtl_MEName_var
        //				from tbl_Enquiry as a,tbl_Inward as b,tbl_Client as c,tbl_Site as d,tbl_Material as e,tbl_MISDetail as f
        //				where a.ENQ_Id=b.INWD_ENQ_Id
        //				and b.INWD_RecordNo_int=f.MISRecordNo
        //				and b.INWD_RecordType_var=f.MISRecType
        //				and a.ENQ_CL_Id=c.CL_Id
        //				and a.ENQ_SITE_Id=d.SITE_Id
        //				and a.ENQ_MATERIAL_Id=e.MATERIAL_Id
        //				and CONVERT (DATE, f.MISApprovedDt) >=CONVERT(DATE,'" + fromDate + "') and CONVERT (DATE, f.MISApprovedDt) <=CONVERT(DATE,'" + ToDate + "')  " +
        //                @"and f.MISIssueDt is null and datediff(d,convert(date,f.MISApprovedDt),convert(date,GETDATE())) <= 3
        //				order by AlertDtl_RecType_var";
        //                    }
        //                }
        //                else if (Alert_Id == 35)
        //                {
        //                    if (flagStatus == 0)
        //                    {
        //                        str = @"Select a.ENQ_Id as AlertDtl_EnqNo_int,a.ENQ_Date_dt as AlertDtl_EnqDate_date,'' as AlertDtl_BillNo_int,null as AlertDtl_BillDate_date,0 as AlertDtl_BillAmt_dec,
        //				'' as AlertDtl_ProNo_var,0 as AlertDtl_ProNo_int,null as AlertDtl_ProDate_date,f.MISRefNo as AlertDtl_RefNo_var,f.MISRecordNo  as AlertDtl_RecNo_int,
        //				c.CL_Name_var as AlertDtl_CL_Name_var,d.SITE_Name_var as AlertDtl_SITE_Name_var,e.MATERIAL_Name_var as AlertDtl_RecType_var,c.CL_Id,d.SITE_Id,35 as AlertDtl_Alert_Id
        //				,(select USER_NAME_var from tbl_User x where x.USER_Id IN (select ROUTE_ME_Id from tbl_Route where ROUTE_Id=d.SITE_Route_Id)) as AlertDtl_MEName_var
        //				from tbl_Enquiry as a,tbl_Inward as b,tbl_Client as c,tbl_Site as d,tbl_Material as e,tbl_MISDetail as f
        //				where a.ENQ_Id=b.INWD_ENQ_Id
        //				and b.INWD_RecordNo_int=f.MISRecordNo
        //				and b.INWD_RecordType_var=f.MISRecType
        //				and a.ENQ_CL_Id=c.CL_Id
        //				and a.ENQ_SITE_Id=d.SITE_Id
        //				and a.ENQ_MATERIAL_Id=e.MATERIAL_Id
        //				and CONVERT (DATE, f.MISApprovedDt) >=CONVERT(DATE,'" + fromDate + "') and CONVERT (DATE, f.MISApprovedDt) <=CONVERT(DATE,'" + ToDate + "')  " +
        //                @"and datediff(d,convert(date,f.MISApprovedDt),convert(date,f.MISIssueDt)) >3
        //				and datediff(d,convert(date,f.MISApprovedDt),convert(date,GETDATE())) >3 
        //				order by AlertDtl_RecType_var";
        //                    }
        //                    else
        //                    {
        //                        str = @"Select a.ENQ_Id as AlertDtl_EnqNo_int,a.ENQ_Date_dt as AlertDtl_EnqDate_date,'' as AlertDtl_BillNo_int,null as AlertDtl_BillDate_date,0 as AlertDtl_BillAmt_dec,
        //				'' as AlertDtl_ProNo_var,0 as AlertDtl_ProNo_int,null as AlertDtl_ProDate_date,f.MISRefNo as AlertDtl_RefNo_var,f.MISRecordNo,
        //				c.CL_Name_var as AlertDtl_CL_Name_var,d.SITE_Name_var as AlertDtl_SITE_Name_var,e.MATERIAL_Name_var as AlertDtl_RecType_var,c.CL_Id,d.SITE_Id,35 as AlertDtl_Alert_Id
        //				,(select USER_NAME_var from tbl_User x where x.USER_Id IN (select ROUTE_ME_Id from tbl_Route where ROUTE_Id=d.SITE_Route_Id)) as AlertDtl_MEName_var
        //				from tbl_Enquiry as a,tbl_Inward as b,tbl_Client as c,tbl_Site as d,tbl_Material as e,tbl_MISDetail as f
        //				where a.ENQ_Id=b.INWD_ENQ_Id
        //				and b.INWD_RecordNo_int=f.MISRecordNo
        //				and b.INWD_RecordType_var=f.MISRecType
        //				and a.ENQ_CL_Id=c.CL_Id
        //				and a.ENQ_SITE_Id=d.SITE_Id
        //				and a.ENQ_MATERIAL_Id=e.MATERIAL_Id
        //				and CONVERT (DATE, f.MISApprovedDt) >=CONVERT(DATE,'" + fromDate + "') and CONVERT (DATE, f.MISApprovedDt) <=CONVERT(DATE,'" + ToDate + "')  " +
        //                @"and f.MISIssueDt is null and datediff(d,convert(date,f.MISApprovedDt),convert(date,GETDATE())) > 3 
        //				order by AlertDtl_RecType_var";
        //                    }
        //                }
        //                else if (Alert_Id == 36)
        //                {
        //                    if (flagStatus == 0)
        //                    {
        //                        str = @"Select 0  as AlertDtl_EnqNo_int,null  as AlertDtl_EnqDate_date,a.BILL_Id as AlertDtl_BillNo_int,a.BILL_Date_dt as AlertDtl_BillDate_date,a.BILL_NetAmt_num as AlertDtl_BillAmt_dec,
        //				'' as AlertDtl_ProNo_var,0 as AlertDtl_ProNo_int,null as AlertDtl_ProDate_date,'' as AlertDtl_RefNo_var,0 as AlertDtl_RecNo_int,
        //				c.CL_Name_var as AlertDtl_CL_Name_var,d.SITE_Name_var as AlertDtl_SITE_Name_var,(select MATERIAL_Name_var from tbl_Material e where e.MATERIAL_RecordType_var=a.BILL_RecordType_var)  as AlertDtl_RecType_var,
        //				c.CL_Id,d.SITE_Id,36 as AlertDtl_Alert_Id
        //				,(select USER_NAME_var from tbl_User x where x.USER_Id IN (select ROUTE_ME_Id from tbl_Route where ROUTE_Id=d.SITE_Route_Id)) as AlertDtl_MEName_var
        //				from tbl_Client as c,tbl_Site as d,tbl_Bill as a
        //				where  a.BILL_CL_Id=c.CL_Id 
        //				and a.Bill_SITE_Id=d.SITE_Id
        //				and CONVERT (DATE, A.BILL_Date_dt) >=CONVERT(DATE,'" + fromDate + "') and CONVERT (DATE, A.BILL_Date_dt) <=CONVERT(DATE,'" + ToDate + "') " +
        //                @"and datediff(d,convert(date,a.BILL_Date_dt),convert(date,a.BILL_ModifiedOn_dt)) = 1
        //				and datediff(d,convert(date,a.BILL_Date_dt),convert(date,GETDATE())) > 2 
        //				order by AlertDtl_RecType_var";
        //                    }
        //                    else
        //                    {
        //                        str = @"Select 0  as AlertDtl_EnqNo_int,null  as AlertDtl_EnqDate_date,a.BILL_Id as AlertDtl_BillNo_int,a.BILL_Date_dt as AlertDtl_BillDate_date,a.BILL_NetAmt_num as AlertDtl_BillAmt_dec,
        //				'' as AlertDtl_ProNo_var,0 as AlertDtl_ProNo_int,null as AlertDtl_ProDate_date,'' as AlertDtl_RefNo_var,0 as AlertDtl_RecNo_int,
        //				c.CL_Name_var as AlertDtl_CL_Name_var,d.SITE_Name_var as AlertDtl_SITE_Name_var,(select MATERIAL_Name_var from tbl_Material e where e.MATERIAL_RecordType_var=a.BILL_RecordType_var)  as AlertDtl_RecType_var,
        //				c.CL_Id,d.SITE_Id,36 as AlertDtl_Alert_Id
        //				,(select USER_NAME_var from tbl_User x where x.USER_Id IN (select ROUTE_ME_Id from tbl_Route where ROUTE_Id=d.SITE_Route_Id)) as AlertDtl_MEName_var
        //				from tbl_Client as c,tbl_Site as d,tbl_Bill as a
        //				where  a.BILL_CL_Id=c.CL_Id 
        //				and a.Bill_SITE_Id=d.SITE_Id
        //				and CONVERT (DATE, A.BILL_Date_dt) >=CONVERT(DATE,'" + fromDate + "') and CONVERT (DATE, A.BILL_Date_dt) <=CONVERT(DATE,'" + ToDate + "') " +
        //                @"and a.BILL_ModifiedOn_dt is null 
        //                and datediff(d,convert(date,a.BILL_Date_dt),convert(date,GETDATE())) > 2 
        //				order by AlertDtl_RecType_var";
        //                    }
        //                }
        //                else if (Alert_Id == 37)
        //                {
        //                    if (flagStatus == 0)
        //                    {
        //                        str = @"Select 0 as AlertDtl_EnqNo_int,null  as AlertDtl_EnqDate_date,a.BILL_Id as AlertDtl_BillNo_int,a.BILL_Date_dt as AlertDtl_BillDate_date,a.BILL_NetAmt_num as AlertDtl_BillAmt_dec,
        //				'' as AlertDtl_ProNo_var,0 as AlertDtl_ProNo_int,null as AlertDtl_ProDate_date,'' as AlertDtl_RefNo_var,0 as AlertDtl_RecNo_int,
        //				c.CL_Name_var as AlertDtl_CL_Name_var,d.SITE_Name_var as AlertDtl_SITE_Name_var,(select MATERIAL_Name_var from tbl_Material e where e.MATERIAL_RecordType_var=a.BILL_RecordType_var)  as AlertDtl_RecType_var,
        //				c.CL_Id,d.SITE_Id,37 as AlertDtl_Alert_Id
        //				,(select USER_NAME_var from tbl_User x where x.USER_Id IN (select ROUTE_ME_Id from tbl_Route where ROUTE_Id=d.SITE_Route_Id)) as AlertDtl_MEName_var
        //				from tbl_Client as c,tbl_Site as d,tbl_Bill as a
        //				where  a.BILL_CL_Id=c.CL_Id 
        //				and a.Bill_SITE_Id=d.SITE_Id
        //				and CONVERT (DATE, A.BILL_Date_dt) >=CONVERT(DATE,'" + fromDate + "') and CONVERT (DATE, A.BILL_Date_dt) <=CONVERT(DATE,'" + ToDate + "') " +
        //                @"and datediff(d,convert(date,a.BILL_Date_dt),convert(date,a.BILL_ModifiedOn_dt)) <= 2
        //				and datediff(d,convert(date,a.BILL_Date_dt),convert(date,GETDATE())) > 2 
        //				order by AlertDtl_RecType_var";
        //                    }
        //                    else
        //                    {
        //                        str = @"Select 0  as AlertDtl_EnqNo_int,null  as AlertDtl_EnqDate_date,a.BILL_Id as AlertDtl_BillNo_int,a.BILL_Date_dt as AlertDtl_BillDate_date,a.BILL_NetAmt_num as AlertDtl_BillAmt_dec,
        //				'' as AlertDtl_ProNo_var,0 as AlertDtl_ProNo_int,null as AlertDtl_ProDate_date,'' as AlertDtl_RefNo_var,0 as AlertDtl_RecNo_int,
        //				c.CL_Name_var as AlertDtl_CL_Name_var,d.SITE_Name_var as AlertDtl_SITE_Name_var,(select MATERIAL_Name_var from tbl_Material e where e.MATERIAL_RecordType_var=a.BILL_RecordType_var)  as AlertDtl_RecType_var,
        //				c.CL_Id,d.SITE_Id,37 as AlertDtl_Alert_Id
        //				,(select USER_NAME_var from tbl_User x where x.USER_Id IN (select ROUTE_ME_Id from tbl_Route where ROUTE_Id=d.SITE_Route_Id)) as AlertDtl_MEName_var
        //				from tbl_Client as c,tbl_Site as d,tbl_Bill as a
        //				where  a.BILL_CL_Id=c.CL_Id 
        //				and a.Bill_SITE_Id=d.SITE_Id
        //				and CONVERT (DATE, A.BILL_Date_dt) >=CONVERT(DATE,'" + fromDate + "') and CONVERT (DATE, A.BILL_Date_dt) <=CONVERT(DATE,'" + ToDate + "') " +
        //                @"and a.BILL_ModifiedOn_dt is null 
        //                and datediff(d,convert(date,a.BILL_Date_dt),convert(date,GETDATE())) > 2 
        //				order by AlertDtl_RecType_var";
        //                    }
        //                }
        //                else if (Alert_Id == 38)
        //                {
        //                    if (flagStatus == 0)
        //                    {
        //                        str = @"Select 0 as AlertDtl_EnqNo_int,null  as AlertDtl_EnqDate_date,a.BILL_Id as AlertDtl_BillNo_int,a.BILL_Date_dt as AlertDtl_BillDate_date,a.BILL_NetAmt_num as AlertDtl_BillAmt_dec,
        //				'' as AlertDtl_ProNo_var,0 as AlertDtl_ProNo_int,null as AlertDtl_ProDate_date,'' as AlertDtl_RefNo_var,0 as AlertDtl_RecNo_int,
        //				c.CL_Name_var as AlertDtl_CL_Name_var,d.SITE_Name_var as AlertDtl_SITE_Name_var,(select MATERIAL_Name_var from tbl_Material e where e.MATERIAL_RecordType_var=a.BILL_RecordType_var)  as AlertDtl_RecType_var,
        //				c.CL_Id,d.SITE_Id,38 as AlertDtl_Alert_Id
        //				,(select USER_NAME_var from tbl_User x where x.USER_Id IN (select ROUTE_ME_Id from tbl_Route where ROUTE_Id=d.SITE_Route_Id)) as AlertDtl_MEName_var
        //				from tbl_Client as c,tbl_Site as d,tbl_Bill as a,tbl_Outward b
        //				where  a.BILL_CL_Id=c.CL_Id 
        //                and a.BILL_Id = b.OUTW_ReferenceNo_var And b.OUTW_RecordType_var ='DT'
        //				and a.Bill_SITE_Id=d.SITE_Id
        //				and CONVERT (DATE, A.BILL_Date_dt) >=CONVERT(DATE,'" + fromDate + "') and CONVERT (DATE, A.BILL_Date_dt) <=CONVERT(DATE,'" + ToDate + "') " +
        //                @"and datediff(d,convert(date,a.BILL_Date_dt),convert(date,b.OUTW_OutwardDate_dt)) > 6
        //				and datediff(d,convert(date,a.BILL_Date_dt),convert(date,GETDATE())) > 6
        //				order by AlertDtl_RecType_var";
        //                    }
        //                    else
        //                    {
        //                        str = @"Select 0 as AlertDtl_EnqNo_int,null  as AlertDtl_EnqDate_date,a.BILL_Id as AlertDtl_BillNo_int,a.BILL_Date_dt as AlertDtl_BillDate_date,a.BILL_NetAmt_num as AlertDtl_BillAmt_dec,
        //				'' as AlertDtl_ProNo_var,0 as AlertDtl_ProNo_int,null as AlertDtl_ProDate_date,'' as AlertDtl_RefNo_var,0 as AlertDtl_RecNo_int,
        //				c.CL_Name_var as AlertDtl_CL_Name_var,d.SITE_Name_var as AlertDtl_SITE_Name_var,(select MATERIAL_Name_var from tbl_Material e where e.MATERIAL_RecordType_var=a.BILL_RecordType_var)  as AlertDtl_RecType_var,
        //				c.CL_Id,d.SITE_Id,38 as AlertDtl_Alert_Id
        //				,(select USER_NAME_var from tbl_User x where x.USER_Id IN (select ROUTE_ME_Id from tbl_Route where ROUTE_Id=d.SITE_Route_Id)) as AlertDtl_MEName_var
        //				from tbl_Client as c,tbl_Site as d,tbl_Bill as a,tbl_Outward b
        //				where  a.BILL_CL_Id=c.CL_Id 
        //                and a.BILL_Id = b.OUTW_ReferenceNo_var And b.OUTW_RecordType_var ='DT'
        //				and a.Bill_SITE_Id=d.SITE_Id
        //				and CONVERT (DATE, A.BILL_Date_dt) >=CONVERT(DATE,'" + fromDate + "') and CONVERT (DATE, A.BILL_Date_dt) <=CONVERT(DATE,'" + ToDate + "') " +
        //                @"and b.OUTW_OutwardDate_dt is null 
        //                and datediff(d,convert(date,a.BILL_Date_dt),convert(date,GETDATE())) > 6 
        //				order by AlertDtl_RecType_var";
        //                    }
        //                }
        //                else if (Alert_Id == 39)
        //                {
        //                    if (flagStatus == 0)
        //                    {
        //                        str = @"---- Payment not received in 60 days after bill received by the client
        //                                Select 0 as AlertDtl_EnqNo_int,null as AlertDtl_EnqDate_date,a.BILL_Id as AlertDtl_BillNo_int,a.BILL_Date_dt as AlertDtl_BillDate_date,a.BILL_NetAmt_num as AlertDtl_BillAmt_dec,
        //                                '' as AlertDtl_ProNo_var,0 as AlertDtl_ProNo_int,null as AlertDtl_ProDate_date,'' as AlertDtl_RefNo_var,0 as AlertDtl_RecNo_int,
        //                                c.CL_Name_var as AlertDtl_CL_Name_var,d.SITE_Name_var as AlertDtl_SITE_Name_var,
        //                                (select MATERIAL_Name_var from tbl_Material e where e.MATERIAL_RecordType_var=a.BILL_RecordType_var)  as AlertDtl_RecType_var,
        //                                c.CL_Id as AlertDtl_CLId_int,d.SITE_Id as AlertDtl_SiteId_int,39 as AlertDtl_Alert_Id,
        //                                (select USER_NAME_var from tbl_User x where x.USER_Id IN (select ROUTE_ME_Id from tbl_Route where ROUTE_Id=d.SITE_Route_Id)) as AlertDtl_MEName_var
        //                                from tbl_Bill as a,tbl_Client as c,tbl_Site as d,tbl_Outward b,tbl_cashDetail e
        //				                where  a.BILL_CL_Id=c.CL_Id 
        //				                and a.BILL_Id = b.OUTW_ReferenceNo_var  and b.OUTW_RecordType_var ='DT'
        //				                and a.Bill_SITE_Id=d.SITE_Id
        //				                and a.BILL_OutwardStatus_bit =1
        //				                and a.BILL_Id=e.CashDetail_BillNo_int
        //                                and CONVERT (DATE, b.OUTW_OutwardDate_dt) >=CONVERT (DATE,'" + fromDate + "') and CONVERT (DATE,b.OUTW_OutwardDate_dt) <=CONVERT (DATE,'" + ToDate + "') " +
        //                                @" and datediff(d,convert(date,b.OUTW_OutwardDate_dt),convert(date,GETDATE())) > 61
        //				                and datediff(d,convert(date,b.OUTW_OutwardDate_dt),convert(date,e.CashDetail_Date_date)) > 61         
        //                                and a.Bill_Status_bit=0
        //                                and (a.BILL_NetAmt_num + (select Sum(b.CashDetail_Amount_money) from tbl_cashDetail b
        //                                where b.CashDetail_BillNo_int=a.BILL_Id and b.CashDetail_Status_bit=0))>0
        //                                order by AlertDtl_RecType_var";
        //                    }
        //                    else
        //                    {
        //                        str = @"---- Payment not received in 60 days after bill received by the client
        //                                Select 0 as AlertDtl_EnqNo_int,null as AlertDtl_EnqDate_date,a.BILL_Id as AlertDtl_BillNo_int,a.BILL_Date_dt as AlertDtl_BillDate_date,a.BILL_NetAmt_num as AlertDtl_BillAmt_dec,
        //                                '' as AlertDtl_ProNo_var,0 as AlertDtl_ProNo_int,null as AlertDtl_ProDate_date,'' as AlertDtl_RefNo_var,0 as AlertDtl_RecNo_int,
        //                                c.CL_Name_var as AlertDtl_CL_Name_var,d.SITE_Name_var as AlertDtl_SITE_Name_var,(select MATERIAL_Name_var from tbl_Material e where e.MATERIAL_RecordType_var=a.BILL_RecordType_var)  as AlertDtl_RecType_var,
        //                                c.CL_Id as AlertDtl_CLId_int,d.SITE_Id as AlertDtl_SiteId_int,39 as AlertDtl_Alert_Id,
        //                                (select USER_NAME_var from tbl_User x where x.USER_Id IN (select ROUTE_ME_Id from tbl_Route where ROUTE_Id=d.SITE_Route_Id)) as AlertDtl_MEName_var
        //                                from tbl_Bill as a,tbl_Client as c,tbl_Site as d,tbl_Outward b
        //				                where  a.BILL_CL_Id=c.CL_Id 
        //				                and a.BILL_Id = b.OUTW_ReferenceNo_var  and b.OUTW_RecordType_var ='DT'
        //				                and a.Bill_SITE_Id=d.SITE_Id
        //				                and a.BILL_OutwardStatus_bit =1
        //				                and CONVERT (DATE,b.OUTW_OutwardDate_dt) >=CONVERT (DATE,'" + fromDate + "') and CONVERT (DATE,b.OUTW_OutwardDate_dt) <=CONVERT (DATE,'" + ToDate + "') " +
        //                                @" and a.Bill_id not in  (select CashDetail_BillNo_int from tbl_CashDetail where CashDetail_Status_bit=0)
        //				                and datediff(d,convert(date,b.OUTW_OutwardDate_dt),convert(date,GETDATE())) > 61 
        //                                and a.Bill_Status_bit=0
        //                                order by AlertDtl_RecType_var";
        //                    }
        //                }
        //                else if (Alert_Id == 40)
        //                {
        //                    if (flagStatus == 0)
        //                    {
        //                        str = @"---- Payment not received in 90 days after bill received by the client
        //                                 Select 0 as AlertDtl_EnqNo_int,null as AlertDtl_EnqDate_date,a.BILL_Id as AlertDtl_BillNo_int,a.BILL_Date_dt as AlertDtl_BillDate_date,a.BILL_NetAmt_num as AlertDtl_BillAmt_dec,
        //                                '' as AlertDtl_ProNo_var,0 as AlertDtl_ProNo_int,null as AlertDtl_ProDate_date,'' as AlertDtl_RefNo_var,0 as AlertDtl_RecNo_int,
        //                                c.CL_Name_var as AlertDtl_CL_Name_var,d.SITE_Name_var as AlertDtl_SITE_Name_var,
        //                                (select MATERIAL_Name_var from tbl_Material e where e.MATERIAL_RecordType_var=a.BILL_RecordType_var)  as AlertDtl_RecType_var,
        //                                c.CL_Id as AlertDtl_CLId_int,d.SITE_Id as AlertDtl_SiteId_int,40 as AlertDtl_Alert_Id,
        //                                (select USER_NAME_var from tbl_User x where x.USER_Id IN (select ROUTE_ME_Id from tbl_Route where ROUTE_Id=d.SITE_Route_Id)) as AlertDtl_MEName_var
        //                                from tbl_Bill as a,tbl_Client as c,tbl_Site as d,tbl_Outward b,tbl_cashDetail e
        //				                where  a.BILL_CL_Id=c.CL_Id 
        //				                and a.BILL_Id = b.OUTW_ReferenceNo_var  and b.OUTW_RecordType_var ='DT'
        //				                and a.Bill_SITE_Id=d.SITE_Id
        //				                and a.BILL_OutwardStatus_bit =1
        //				                and a.BILL_Id=e.CashDetail_BillNo_int
        //                                and CONVERT (DATE, b.OUTW_OutwardDate_dt) >=CONVERT (DATE,'" + fromDate + "') and CONVERT (DATE,b.OUTW_OutwardDate_dt) <=CONVERT (DATE,'" + ToDate + "') " +
        //                                @" and datediff(d,convert(date,b.OUTW_OutwardDate_dt),convert(date,GETDATE())) > 91
        //				                and datediff(d,convert(date,b.OUTW_OutwardDate_dt),convert(date,e.CashDetail_Date_date)) > 91         
        //                                and a.Bill_Status_bit=0
        //                                and (a.BILL_NetAmt_num + (select Sum(b.CashDetail_Amount_money) from tbl_cashDetail b
        //                                where b.CashDetail_BillNo_int=a.BILL_Id and b.CashDetail_Status_bit=0))>0
        //                                order by AlertDtl_RecType_var";
        //                    }
        //                    else
        //                    {
        //                        str = @"---- Payment not received in 90 days after bill received by the client
        //                                 Select 0 as AlertDtl_EnqNo_int,null as AlertDtl_EnqDate_date,a.BILL_Id as AlertDtl_BillNo_int,a.BILL_Date_dt as AlertDtl_BillDate_date,a.BILL_NetAmt_num as AlertDtl_BillAmt_dec,
        //                                '' as AlertDtl_ProNo_var,0 as AlertDtl_ProNo_int,null as AlertDtl_ProDate_date,'' as AlertDtl_RefNo_var,0 as AlertDtl_RecNo_int,
        //                                c.CL_Name_var as AlertDtl_CL_Name_var,d.SITE_Name_var as AlertDtl_SITE_Name_var,(select MATERIAL_Name_var from tbl_Material e where e.MATERIAL_RecordType_var=a.BILL_RecordType_var)  as AlertDtl_RecType_var,
        //                                c.CL_Id as AlertDtl_CLId_int,d.SITE_Id as AlertDtl_SiteId_int,40 as AlertDtl_Alert_Id,
        //                                (select USER_NAME_var from tbl_User x where x.USER_Id IN (select ROUTE_ME_Id from tbl_Route where ROUTE_Id=d.SITE_Route_Id)) as AlertDtl_MEName_var
        //                                from tbl_Bill as a,tbl_Client as c,tbl_Site as d,tbl_Outward b
        //				                where  a.BILL_CL_Id=c.CL_Id 
        //				                and a.BILL_Id = b.OUTW_ReferenceNo_var  and b.OUTW_RecordType_var ='DT'
        //				                and a.Bill_SITE_Id=d.SITE_Id
        //				                and a.BILL_OutwardStatus_bit =1
        //				                and CONVERT (DATE,b.OUTW_OutwardDate_dt) >=CONVERT (DATE,'" + fromDate + "') and CONVERT (DATE,b.OUTW_OutwardDate_dt) <=CONVERT (DATE,'" + ToDate + "') " +
        //                                @" and a.Bill_id not in  (select CashDetail_BillNo_int from tbl_CashDetail where CashDetail_Status_bit=0)
        //				                and datediff(d,convert(date,b.OUTW_OutwardDate_dt),convert(date,GETDATE())) > 91 
        //                                and a.Bill_Status_bit=0
        //                                order by AlertDtl_RecType_var";
        //                    }
        //                }
        //                else if (Alert_Id == 41)
        //                {
        //                    if (flagStatus == 0)
        //                    {
        //                        str = @"---- Payment not received in 120 days after bill received by the client
        //                                 Select 0 as AlertDtl_EnqNo_int,null as AlertDtl_EnqDate_date,a.BILL_Id as AlertDtl_BillNo_int,a.BILL_Date_dt as AlertDtl_BillDate_date,a.BILL_NetAmt_num as AlertDtl_BillAmt_dec,
        //                                '' as AlertDtl_ProNo_var,0 as AlertDtl_ProNo_int,null as AlertDtl_ProDate_date,'' as AlertDtl_RefNo_var,0 as AlertDtl_RecNo_int,
        //                                c.CL_Name_var as AlertDtl_CL_Name_var,d.SITE_Name_var as AlertDtl_SITE_Name_var,
        //                                (select MATERIAL_Name_var from tbl_Material e where e.MATERIAL_RecordType_var=a.BILL_RecordType_var)  as AlertDtl_RecType_var,
        //                                c.CL_Id as AlertDtl_CLId_int,d.SITE_Id as AlertDtl_SiteId_int,41 as AlertDtl_Alert_Id,
        //                                (select USER_NAME_var from tbl_User x where x.USER_Id IN (select ROUTE_ME_Id from tbl_Route where ROUTE_Id=d.SITE_Route_Id)) as AlertDtl_MEName_var
        //                                from tbl_Bill as a,tbl_Client as c,tbl_Site as d,tbl_Outward b,tbl_cashDetail e
        //				                where  a.BILL_CL_Id=c.CL_Id 
        //				                and a.BILL_Id = b.OUTW_ReferenceNo_var  and b.OUTW_RecordType_var ='DT'
        //				                and a.Bill_SITE_Id=d.SITE_Id
        //				                and a.BILL_OutwardStatus_bit =1
        //				                and a.BILL_Id=e.CashDetail_BillNo_int
        //                                and CONVERT (DATE, b.OUTW_OutwardDate_dt) >=CONVERT (DATE,'" + fromDate + "') and CONVERT (DATE,b.OUTW_OutwardDate_dt) <=CONVERT (DATE,'" + ToDate + "') " +
        //                                @" and datediff(d,convert(date,b.OUTW_OutwardDate_dt),convert(date,GETDATE())) > 121
        //				                and datediff(d,convert(date,b.OUTW_OutwardDate_dt),convert(date,e.CashDetail_Date_date)) > 121         
        //                                and a.Bill_Status_bit=0
        //                                and (a.BILL_NetAmt_num + (select Sum(b.CashDetail_Amount_money) from tbl_cashDetail b
        //                                where b.CashDetail_BillNo_int=a.BILL_Id and b.CashDetail_Status_bit=0))>0
        //                                order by AlertDtl_RecType_var";
        //                    }
        //                    else
        //                    {
        //                        str = @"---- Payment not received in 120 days after bill received by the client
        //                                 Select 0 as AlertDtl_EnqNo_int,null as AlertDtl_EnqDate_date,a.BILL_Id as AlertDtl_BillNo_int,a.BILL_Date_dt as AlertDtl_BillDate_date,a.BILL_NetAmt_num as AlertDtl_BillAmt_dec,
        //                                '' as AlertDtl_ProNo_var,0 as AlertDtl_ProNo_int,null as AlertDtl_ProDate_date,'' as AlertDtl_RefNo_var,0 as AlertDtl_RecNo_int,
        //                                c.CL_Name_var as AlertDtl_CL_Name_var,d.SITE_Name_var as AlertDtl_SITE_Name_var,(select MATERIAL_Name_var from tbl_Material e where e.MATERIAL_RecordType_var=a.BILL_RecordType_var)  as AlertDtl_RecType_var,
        //                                c.CL_Id as AlertDtl_CLId_int,d.SITE_Id as AlertDtl_SiteId_int,41 as AlertDtl_Alert_Id,
        //                                (select USER_NAME_var from tbl_User x where x.USER_Id IN (select ROUTE_ME_Id from tbl_Route where ROUTE_Id=d.SITE_Route_Id)) as AlertDtl_MEName_var
        //                                from tbl_Bill as a,tbl_Client as c,tbl_Site as d,tbl_Outward b
        //				                where  a.BILL_CL_Id=c.CL_Id 
        //				                and a.BILL_Id = b.OUTW_ReferenceNo_var  and b.OUTW_RecordType_var ='DT'
        //				                and a.Bill_SITE_Id=d.SITE_Id
        //				                and a.BILL_OutwardStatus_bit =1
        //				                and CONVERT (DATE,b.OUTW_OutwardDate_dt) >=CONVERT (DATE,'" + fromDate + "') and CONVERT (DATE,b.OUTW_OutwardDate_dt) <=CONVERT (DATE,'" + ToDate + "') " +
        //                                @" and a.Bill_id not in  (select CashDetail_BillNo_int from tbl_CashDetail where CashDetail_Status_bit=0)
        //				                and datediff(d,convert(date,b.OUTW_OutwardDate_dt),convert(date,GETDATE())) > 121 
        //                                and a.Bill_Status_bit=0
        //                                order by AlertDtl_RecType_var";
        //                    }
        //                }
        //                SqlDataAdapter da = new SqlDataAdapter(str, con);//and CL_name_var like '" + str + "%'
        //                da.Fill(ds);
        //                dt = ds.Tables[0];
        //                ds.Dispose();
        //                return dt;
        //            }
        //            catch (Exception e)
        //            {
        //                throw e;
        //            }

        //        }

    }
}