//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Web;
//using System.Configuration;
//using System.Data;
//using System.Data.SqlClient;
//namespace DESPLWEB
//{
//    public class MyData
//    {
//      static string cnStr = ConfigurationManager.AppSettings["DuroConnectionString"].ToString();
//        protected static SqlConnection con;

//        public DataTable getGeneralData(string mQueryString)
//        {
//            SqlConnection con = new SqlConnection(cnStr);
//            try
//            {
//                DataSet ds = new DataSet();
//                DataTable dt = new DataTable();
//                SqlDataAdapter da = new SqlDataAdapter(mQueryString, con);
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


//        public void updateMDLApprovedBy(int approvedByID,string refNo)
//        {
                       
         
//            SqlDataAdapter adapter = new SqlDataAdapter();
//            string sql = null;
           
//            con = new SqlConnection(cnStr );
//            sql = "update tbl_MixDesign_Inward set MFINWD_ApprovedBy_tint = approvedByID where MFINWD_ReferenceNo_var='" + refNo +"'";
//            try
//            {
//                con.Open();
//                adapter.UpdateCommand = con.CreateCommand();
//                adapter.UpdateCommand.CommandText = sql;
//                adapter.UpdateCommand.ExecuteNonQuery();
               
//            }
//            catch (Exception ex)
//            {
                
//            }



         
//        }
//        public Int32 getRecordNo(string refNo)
//        {
//            DataTable dt = getGeneralData("SELECT MFINWD_RecordNo_int FROM tbl_MixDesign_Inward where MFINWD_ReferenceNo_var='"+refNo +"'");
//            return Convert.ToInt32(dt.Rows[0]["MFINWD_RecordNo_int"].ToString());
        
//        }
//        public void updateGeneralData(string mQueryString)
//        {
//            SqlConnection con = new SqlConnection(cnStr);

//        }
//    }
//}