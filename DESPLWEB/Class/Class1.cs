using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for Class1
/// </summary>
public class myDataComm
{
  
    protected string cnStr = "";
    //System.Configuration.ConfigurationSettings.AppSettings["connStrNewNew"] + HttpContext.Current.Session["Path"].ToString();
    //protected string cnStr = System.Configuration.ConfigurationSettings.AppSettings["connStrNew"].ToString();
    SqlConnection cn = new SqlConnection();
    protected string _mRcvdDate;
    protected int _stComplianceNote;
    public myDataComm()
    {
        //
        // TODO: Add constructor logic here
        //
    }
    public myDataComm(string con)
    {
        cn = new SqlConnection(con);
     }
    public string ReportDetail
    {
        get
        {
            return _mRcvdDate;
        }
        set
        {
            _mRcvdDate = value;
        }
    }
    public int stCompliance
    {
        get
        {
            return _stComplianceNote;
        }
        set
        {
            _stComplianceNote = value;
        }
    }

    public string getConnectionStringForWeb(string Location)
    {
        string Connection = "";
        //if (Loctaion.Equals("Pune"))
        //    Connection = "Data Source=92.204.136.64;Initial Catalog=VeenaLive;User ID=dipl;Password=dipl2020";
        //else if (Loctaion.Equals("Mumbai"))
        //    Connection = "Data Source=92.204.136.64;Initial Catalog=VeenaMumbai;User ID=dipl;Password=dipl2020";
        //else if (Loctaion.ToString().Equals("Nashik"))
        //    Connection = "Data Source=92.204.136.64;Initial Catalog=VeenaNashik;User ID=dipl;Password=dipl2020";

        if (Location.Equals("Pune"))
            Connection = System.Configuration.ConfigurationManager.AppSettings["conStrPune"].ToString();
        else if (Location.Equals("Mumbai"))
            Connection = System.Configuration.ConfigurationManager.AppSettings["conStrMumbai"].ToString();
        else if (Location.Equals("Nashik")) //Nashik
            Connection = System.Configuration.ConfigurationManager.AppSettings["conStrNashik"].ToString();
        else if (Location.Equals("Metro")) 
            Connection = System.Configuration.ConfigurationManager.AppSettings["conStrMetro"].ToString();

        return Connection;
    }
    public SqlConnection OpenConnection(string connetcion)
    {
        //if (connetcion == "veena2016")
        //    cnStr = ConfigurationManager.ConnectionStrings["connStr2016"].ConnectionString;
        //else if (connetcion == "veena2013")
        //    cnStr = ConfigurationManager.ConnectionStrings["connStrPre"].ConnectionString;
        //else
        //    cnStr = ConfigurationManager.ConnectionStrings["connStrNew"].ConnectionString;
        //cn = new SqlConnection(cnStr);
        cn.Open();
        return cn;
    }
    public SqlConnection CloseConnection()
    {
        cn.Close();
        return cn;
    }
    public double verifyLoginUser(string userName, string passWd, string db)
    {
        double clientId = 0;
        try
        {
          
            string mySql;
            if (db == "veena2016" || db == "veena2020")
            {
                mySql = "SELECT CL_Id as client_id FROM tbl_Client where CL_LoginId_var='" + userName + "'";
                mySql = mySql + " and CL_Password_var='" + passWd + "'";
            }
            else
            {
                mySql = "SELECT client_id FROM client_info where LoginId='" + userName + "'";
                mySql = mySql + " and password='" + passWd + "'";
            }
            //OpenConnection(db);
            cn.Open();
            SqlCommand cmd = new SqlCommand(mySql, cn);
            SqlDataReader dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                clientId = Convert.ToInt32(dr.GetValue(0));
                HttpContext.Current.Session["ClientID"] = clientId.ToString();
                dr.NextResult();
            }
        }
        catch (Exception e)
        {
            throw e;
        }
        finally
        {
            CloseConnection();
         }
        return clientId;
    }

    public double verifyLoginUser_UserTable(string userName, string passWd)
    {
        double userId = 0;
        try
        {

            string mySql;
            mySql = "SELECT USER_Id FROM tbl_User where USER_Name_var='" + userName + "'";
            mySql = mySql + " and USER_Password_var='" + passWd + "'"; // + " and USER_Status_bit = 0 ";
            
            cn.Open();
            SqlCommand cmd = new SqlCommand(mySql, cn);
            SqlDataReader dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                userId = Convert.ToInt32(dr.GetValue(0));
                HttpContext.Current.Session["UserId"] = userId.ToString();
                dr.NextResult();
            }
        }
        catch (Exception e)
        {
            throw e;
        }
        finally
        {
            CloseConnection();
        }
        return userId;
    }

    public string getClientName(double clid, string db)
    {
        string clName;
        string mySql;
        //  if (HttpContext.Current.Session["databasename"].ToString() == "veena2016")
        if (db == "veena2016" || db == "veena2020")
        {
            mySql = "SELECT CL_Name_var FROM tbl_Client where CL_Id=" + clid.ToString();
        }
        else
        {
            mySql = "SELECT client_name FROM client_info where client_Id=" + clid.ToString();
        }
        //OpenConnection(db);
        cn.Open();
        SqlCommand cmd = new SqlCommand (mySql, cn);
        SqlDataReader dr = cmd.ExecuteReader();
        clName = "NA";
        while (dr.Read())
        {
            clName = dr.GetValue(0).ToString();

            dr.NextResult();
        }
        CloseConnection();
        cmd.Dispose();
        return clName;
    }

    public string getClientEmail(double clid)
    {
         string email = "";
        // cn = new SqlConnection(cnStr);
        SqlCommand cmd = new SqlCommand();
        cmd.CommandText = "SELECT IsNULL(CL_EmailID_var,'') as email FROM tbl_Client where CL_Id=" + clid.ToString();
        cn.Open();
        cmd.Connection = cn;
        using (SqlDataReader reader = cmd.ExecuteReader())
        {
            while (reader.Read())
            {
                email = reader.GetString(0);
            }
        }
        cmd.Dispose();
        cn.Close();
        return email;
    }
    public DataTable getDiviceLoginDetails(double clId)
    {
        try
        {
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            string strQuery = @"select c.DL_Id,c.DL_Site_id,c.DL_ContEmail_var,c.DL_ContNo_var,c.DL_ContPerson_var,c.DL_BillApproval_bit,
                                CASE WHEN DL_Status_bit = 0   THEN 'Active' 
				                WHEN DL_Status_bit = 1 THEN 'Deactive' 
				                END AS DL_Status_bit
                                 from tbl_DeviceLogin c,tbl_Client a
                                where c.DL_CL_Id = a.CL_Id
                                and a.CL_Id = " + clId.ToString() + " order by DL_id desc ";
           SqlDataAdapter da = new SqlDataAdapter(strQuery, cn);
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
   public DataTable getSiteList(double clId, string db)
    {
        try
        {
            string strQuery = "";
            if (db == "veena2016" || db == "veena2020")
            {
                strQuery = "Select SITE_Name_var,SITE_Id From tbl_Site where SITE_CL_Id= " + clId.ToString() + " order by Site_name_var ";
            }
            else
            {
                strQuery = "Select site_name,site_id From site_table where client_Id= " + clId.ToString() + " order by site_name ";
            }
            DataTable dt = getGeneralData(strQuery, db);
            return dt;
        }
        catch (Exception e)
        {
            throw e;
        }
    }
    public DataTable getSiteListforMetro(double clId, string db)
    {
        try
        {
            string strQuery = "";
            strQuery = "Select SITE_Name_var,SITE_Id From tbl_Site where SITE_CL_Id= " + clId.ToString() + " and  SITE_MetroStatus_bit = 1  order by Site_name_var ";
            DataTable dt = getGeneralData(strQuery, db);
            return dt;
        }
        catch (Exception e)
        {
            throw e;
        }
    }
    public DataTable getClientforMetro(double clId, string db)
    {
        try
        {
            string strQuery = "";
            if (db == "veena2016" || db == "veena2020")
            {
                strQuery = @"Select CL_Name_var ,SITE_CL_Id  From tbl_Site left join tbl_Client on  CL_Id = SITE_CL_Id "+
                           "where SITE_MetroStatus_bit = 1 group by CL_Name_var,SITE_CL_Id  order by CL_Name_var";
            }            
            DataTable dt = getGeneralData(strQuery, db);
            return dt;
        }
        catch (Exception e)
        {
            throw e;
        }
    }
    public DataTable getSupplierList(double clId, double siteId, string db)
    {
        try
        {
            string strQuery = "";
            if (db == "veena2016" || db == "veena2020")
            {
                strQuery = "Select distinct(SupplierName)" +
                " from "+
                " ( "+
                " Select distinct(b.AGGTINWD_SupplierName_var)  as SupplierName "+
                " From tbl_Inward as a join tbl_Aggregate_Inward as b on a.INWD_RecordNo_int = b.AGGTINWD_RecordNo_int and a.INWD_RecordType_var = b.AGGTINWD_RecordType_var " +
                " where  a.INWD_CL_Id=" + clId + " and a.INWD_SITE_Id=" + siteId +
                " Union "+
                " Select distinct(b.BTINWD_SupplierName_var)  as SupplierName " +
                " From tbl_Inward as a join tbl_Brick_Inward as b on a.INWD_RecordNo_int = b.BTINWD_RecordNo_int and a.INWD_RecordType_var = b.BTINWD_RecordType_var " +
                " where  a.INWD_CL_Id=" + clId + " and a.INWD_SITE_Id=" + siteId +
                " Union "+
                " Select distinct(b.CCHINWD_SupplierName_var)  as SupplierName "+
                " From tbl_Inward as a join tbl_CementChemical_Inward as b on a.INWD_RecordNo_int = b.CCHINWD_RecordNo_int  and a.INWD_RecordType_var = b.CCHINWD_RecordType_var" +
                " where  a.INWD_CL_Id=" + clId + " and a.INWD_SITE_Id=" + siteId +
                " Union "+
                " Select distinct(b.CEMTINWD_SupplierName_var)  as SupplierName "+
                " From tbl_Inward as a join tbl_Cement_Inward as b on a.INWD_RecordNo_int = b.CEMTINWD_RecordNo_int and a.INWD_RecordType_var = b.CEMTINWD_RecordType_var " +
                " where  a.INWD_CL_Id=" + clId + " and a.INWD_SITE_Id=" + siteId +
                " Union "+
                " Select distinct(b.CTINWD_SupplierName_var)  as SupplierName "+
                " From tbl_Inward as a join tbl_Cube_Inward as b on a.INWD_RecordNo_int = b.CTINWD_RecordNo_int and a.INWD_RecordType_var = b.CTINWD_RecordType_var " +
                " where  a.INWD_CL_Id=" + clId + " and a.INWD_SITE_Id=" + siteId +
                " Union "+
                " Select distinct(b.FLYASHINWD_SupplierName_var)  as SupplierName "+
                " From tbl_Inward as a join tbl_FlyAsh_Inward as b on a.INWD_RecordNo_int = b.FLYASHINWD_RecordNo_int and a.INWD_RecordType_var = b.FLYASHINWD_RecordType_var " +
                " where  a.INWD_CL_Id=" + clId + " and a.INWD_SITE_Id=" + siteId +
                " Union "+
                " Select distinct(b.SOLIDINWD_SupplierName_var)  as SupplierName "+
                " From tbl_Inward as a join tbl_Solid_Inward as b on a.INWD_RecordNo_int = b.SOLIDINWD_RecordNo_int and a.INWD_RecordType_var = b.SOLIDINWD_RecordType_var " +
                " where  a.INWD_CL_Id=" + clId + " and a.INWD_SITE_Id=" + siteId +
                " Union "+
                " Select distinct(b.PTINWD_SupplierName_var)  as SupplierName "+
                " From tbl_Inward as a join tbl_Pavement_Inward as b on a.INWD_RecordNo_int = b.PTINWD_RecordNo_int and a.INWD_RecordType_var = b.PTINWD_RecordType_var " +
                " where  a.INWD_CL_Id=" + clId + " and a.INWD_SITE_Id=" + siteId +
                " Union "+
                " Select distinct(b.PILEINWD_SupplierName_var)  as SupplierName "+
                " From tbl_Inward as a join tbl_Pile_Inward as b on a.INWD_RecordNo_int = b.PILEINWD_RecordNo_int and a.INWD_RecordType_var = b.PILEINWD_RecordType_var " +
                " where  a.INWD_CL_Id=" + clId + " and a.INWD_SITE_Id=" + siteId +
                " Union "+
                " Select distinct(b.STINWD_SupplierName_var)  as SupplierName "+
                " From tbl_Inward as a join tbl_Steel_Inward as b on a.INWD_RecordNo_int = b.STINWD_RecordNo_int and a.INWD_RecordType_var = b.STINWD_RecordType_var " +
                " where  a.INWD_CL_Id=" + clId + " and a.INWD_SITE_Id=" + siteId +
                " Union "+
                " Select distinct(b.STCINWD_SupplierName_var)  as SupplierName "+
                " From tbl_Inward as a join tbl_SteelChemical_Inward as b on a.INWD_RecordNo_int = b.STCINWD_RecordNo_int and a.INWD_RecordType_var = b.STCINWD_RecordType_var " +
                " where  a.INWD_CL_Id=" + clId + " and a.INWD_SITE_Id=" + siteId +
                " Union "+
                " Select distinct(b.TILEINWD_SupplierName_var)  as SupplierName "+
                " From tbl_Inward as a join tbl_Tile_Inward as b on a.INWD_RecordNo_int = b.TILEINWD_RecordNo_int and a.INWD_RecordType_var = b.TILEINWD_RecordType_var " +
                " where  a.INWD_CL_Id=" + clId + " and a.INWD_SITE_Id=" + siteId + 
                " Union "+
                " Select distinct(b.WTINWD_SupplierName_var)  as SupplierName "+
                " From tbl_Inward as a join tbl_Water_Inward as b on a.INWD_RecordNo_int = b.WTINWD_RecordNo_int and a.INWD_RecordType_var = b.WTINWD_RecordType_var " +
                " where  a.INWD_CL_Id=" + clId + " and a.INWD_SITE_Id=" + siteId +
                " Union " +
                " Select distinct(b.OTINWD_SupplierName_var)  as SupplierName " +
                " From tbl_Inward as a join tbl_Other_Inward as b on a.INWD_RecordNo_int = b.OTINWD_RecordNo_int and a.INWD_RecordType_var = b.OTINWD_RecordType_var " +
                " where  a.INWD_CL_Id=" + clId + " and a.INWD_SITE_Id=" + siteId +
                " ) as x where SupplierName <> ''";
            }
            else
            {
                strQuery = "Select distinct(SupplierName) " +
                " from " +
                " ( " +
                " Select distinct(b.SupplierName)  as SupplierName " +
                " From Record_Table as a join Aggregate_Table as b on a.Record_No= b.Record_No and a.Record_Type = b.Record_Type  " +
                " where  a.Client_ID=" + clId + " and a.Site_ID=" + siteId +
                " Union " +
                " Select distinct(b.Name_Of_Supp)  as SupplierName " +
                " From Record_Table as a join BrickInward as b on a.Record_No= b.Record_No and a.Record_Type = b.Record_Type  " +
                " where  a.Client_ID=" + clId + " and a.Site_ID=" + siteId +
                " Union " +
                " Select distinct(b.SupplierName)  as SupplierName " +
                " From Record_Table as a join CEMTChemicalInward as b on a.Record_No= b.RecordNo and a.Record_Type = b.RecordType " +
                " where  a.Client_ID=" + clId + " and a.Site_ID=" + siteId +
                " Union " +
                " Select distinct(b.SupplierName)  as SupplierName " +
                " From Record_Table as a join Cement_Inward as b  on a.Record_No= b.Record_No and a.Record_Type = b.Record_Type  " +
                " where  a.Client_ID=" + clId + " and a.Site_ID=" + siteId +
                " Union " +
                " Select distinct(b.SupplierName)  as SupplierName " +
                " From Record_Table as a join Fly_Ash_Inward as b on a.Record_No= b.Record_No and a.Record_Type = b.Record_Type  " +
                " where  a.Client_ID=" + clId + " and a.Site_ID=" + siteId +
                " Union " +
                " Select distinct(b.SupplierName)  as SupplierName " +
                " From Record_Table as a join SolidInward as b on a.Record_No= b.Record_No and a.Record_Type = b.Record_Type  " +
                " where  a.Client_ID=" + clId + " and a.Site_ID=" + siteId +
                " Union " +
                " Select distinct(b.SupplierName)  as SupplierName " +
                " From Record_Table as a join Pile_test_master as b on a.Record_No= b.Record_No and a.Record_Type = b.Record_Type  " +
                " where  a.Client_ID=" + clId + " and a.Site_ID=" + siteId +
                " Union " +
                " Select distinct(b.Name_Of_Supp)  as SupplierName " +
                " From Record_Table as a join Steel_Test_Inward_Table as b on a.Record_No= b.Record_No and a.Record_Type = b.Record_Type  " +
                " where  a.Client_ID=" + clId + " and a.Site_ID=" + siteId +
                " Union " +
                " Select distinct(b.NameOfSupp)  as SupplierName " +
                " From Record_Table as a join STChemicalInward as b on a.Record_No= b.RecordNo and a.Record_Type = b.RecordType  " +
                " where  a.Client_ID=" + clId + " and a.Site_ID=" + siteId +
                " Union " +
                " Select distinct(b.SupplierName)  as SupplierName " +
                " From Record_Table as a join TileTestInward as b on a.Record_No= b.Record_No and a.Record_Type = b.Record_Type  " +
                " where  a.Client_ID=" + clId + " and a.Site_ID=" + siteId +
                " ) as x where SupplierName <> '' ";

            }
            DataTable dt = getGeneralData(strQuery, db);
            return dt;
        }
        catch (Exception e)
        {
            throw e;
        }
    }

    public DataTable getBuildingListMetro(double clId, double siteId, string db)
    {
        try
        {
            string strQuery = "";
            strQuery = "Select distinct(INWD_Building_var) From tbl_Inward where ";
            strQuery = strQuery + " INWD_CL_Id=" + clId.ToString();
            strQuery = strQuery + " and INWD_SITE_Id=" + siteId.ToString();
     
            DataTable dt = getGeneralData(strQuery, db);
            return dt;
        }
        catch (Exception e)
        {
            throw e;
        }
    }
    public DataTable getBuildingList(double clId, double siteId, string supplierName, string db)
    {
        try
        {
            string strQuery = "";
            if (db == "veena2016" || db == "veena2020")
            {
                if (supplierName == "---All---")
                {
                    strQuery = "Select distinct(INWD_Building_var) From tbl_Inward where ";
                    strQuery = strQuery + " INWD_CL_Id=" + clId.ToString();
                    strQuery = strQuery + " and INWD_SITE_Id=" + siteId.ToString();
                }
                else
                {
                    strQuery = "Select distinct(INWD_Building_var)" +
                " from " +
                " ( " +
                " Select distinct(a.INWD_Building_var)  " +
                " From tbl_Inward as a join tbl_Aggregate_Inward as b on a.INWD_RecordNo_int = b.AGGTINWD_RecordNo_int and a.INWD_RecordType_var = b.AGGTINWD_RecordType_var " +
                " where  a.INWD_CL_Id=" + clId + " and a.INWD_SITE_Id=" + siteId + " and b.AGGTINWD_SupplierName_var <> '' and b.AGGTINWD_SupplierName_var = '" + supplierName + "' " +
                " Union " +
                " Select distinct(a.INWD_Building_var)  " +
                " From tbl_Inward as a join tbl_Brick_Inward as b on a.INWD_RecordNo_int = b.BTINWD_RecordNo_int and a.INWD_RecordType_var = b.BTINWD_RecordType_var " +
                " where  a.INWD_CL_Id=" + clId + " and a.INWD_SITE_Id=" + siteId + " and b.BTINWD_SupplierName_var <> '' and b.BTINWD_SupplierName_var = '" + supplierName + "' " +
                " Union " +
                " Select distinct(a.INWD_Building_var)  " +
                " From tbl_Inward as a join tbl_CementChemical_Inward as b on a.INWD_RecordNo_int = b.CCHINWD_RecordNo_int and a.INWD_RecordType_var = b.CCHINWD_RecordType_var " +
                " where  a.INWD_CL_Id=" + clId + " and a.INWD_SITE_Id=" + siteId + " and b.CCHINWD_SupplierName_var <> '' and b.CCHINWD_SupplierName_var = '" + supplierName + "' " +
                " Union " +
                " Select distinct(a.INWD_Building_var)  " +
                " From tbl_Inward as a join tbl_Cement_Inward as b on a.INWD_RecordNo_int = b.CEMTINWD_RecordNo_int and a.INWD_RecordType_var = b.CEMTINWD_RecordType_var " +
                " where  a.INWD_CL_Id=" + clId + " and a.INWD_SITE_Id=" + siteId + " and b.CEMTINWD_SupplierName_var <> '' and b.CEMTINWD_SupplierName_var = '" + supplierName + "' " +
                " Union " +
                " Select distinct(a.INWD_Building_var)  " +
                " From tbl_Inward as a join tbl_Cube_Inward as b on a.INWD_RecordNo_int = b.CTINWD_RecordNo_int and a.INWD_RecordType_var = b.CTINWD_RecordType_var " +
                " where  a.INWD_CL_Id=" + clId + " and a.INWD_SITE_Id=" + siteId + " and b.CTINWD_SupplierName_var <> '' and b.CTINWD_SupplierName_var = '" + supplierName + "' " +
                " Union " +
                " Select distinct(a.INWD_Building_var)  " +
                " From tbl_Inward as a join tbl_FlyAsh_Inward as b on a.INWD_RecordNo_int = b.FLYASHINWD_RecordNo_int and a.INWD_RecordType_var = b.FLYASHINWD_RecordType_var " +
                " where  a.INWD_CL_Id=" + clId + " and a.INWD_SITE_Id=" + siteId + " and b.FLYASHINWD_SupplierName_var <> '' and b.FLYASHINWD_SupplierName_var = '" + supplierName + "' " +
                " Union " +
                " Select distinct(a.INWD_Building_var)  " +
                " From tbl_Inward as a join tbl_Solid_Inward as b on a.INWD_RecordNo_int = b.SOLIDINWD_RecordNo_int and a.INWD_RecordType_var = b.SOLIDINWD_RecordType_var " +
                " where  a.INWD_CL_Id=" + clId + " and a.INWD_SITE_Id=" + siteId + " and b.SOLIDINWD_SupplierName_var <> '' and b.SOLIDINWD_SupplierName_var = '" + supplierName + "' " +
                " Union " +
                " Select distinct(a.INWD_Building_var)  " +
                " From tbl_Inward as a join tbl_Pavement_Inward as b on a.INWD_RecordNo_int = b.PTINWD_RecordNo_int and a.INWD_RecordType_var = b.PTINWD_RecordType_var " +
                " where  a.INWD_CL_Id=" + clId + " and a.INWD_SITE_Id=" + siteId + " and b.PTINWD_SupplierName_var <> '' and b.PTINWD_SupplierName_var = '" + supplierName + "' " +
                " Union " +
                " Select distinct(a.INWD_Building_var)  " +
                " From tbl_Inward as a join tbl_Pile_Inward as b on a.INWD_RecordNo_int = b.PILEINWD_RecordNo_int and a.INWD_RecordType_var = b.PILEINWD_RecordType_var " +
                " where  a.INWD_CL_Id=" + clId + " and a.INWD_SITE_Id=" + siteId + " and b.PILEINWD_SupplierName_var <> '' and b.PILEINWD_SupplierName_var = '" + supplierName + "' " +
                " Union " +
                " Select distinct(a.INWD_Building_var)  " +
                " From tbl_Inward as a join tbl_Steel_Inward as b on a.INWD_RecordNo_int = b.STINWD_RecordNo_int and a.INWD_RecordType_var = b.STINWD_RecordType_var " +
                " where  a.INWD_CL_Id=" + clId + " and a.INWD_SITE_Id=" + siteId + " and b.STINWD_SupplierName_var <> '' and b.STINWD_SupplierName_var = '" + supplierName + "' " +
                " Union " +
                " Select distinct(a.INWD_Building_var)  " +
                " From tbl_Inward as a join tbl_SteelChemical_Inward as b on a.INWD_RecordNo_int = b.STCINWD_RecordNo_int and a.INWD_RecordType_var = b.STCINWD_RecordType_var " +
                " where  a.INWD_CL_Id=" + clId + " and a.INWD_SITE_Id=" + siteId + " and b.STCINWD_SupplierName_var <> '' and b.STCINWD_SupplierName_var = '" + supplierName + "' " +
                " Union " +
                " Select distinct(a.INWD_Building_var)  " +
                " From tbl_Inward as a join tbl_Tile_Inward as b on a.INWD_RecordNo_int = b.TILEINWD_RecordNo_int and a.INWD_RecordType_var = b.TILEINWD_RecordType_var " +
                " where  a.INWD_CL_Id=" + clId + " and a.INWD_SITE_Id=" + siteId + " and b.TILEINWD_SupplierName_var <> '' and b.TILEINWD_SupplierName_var = '" + supplierName + "' " +
                " Union " +
                " Select distinct(a.INWD_Building_var)  " +
                " From tbl_Inward as a join tbl_Water_Inward as b on a.INWD_RecordNo_int = b.WTINWD_RecordNo_int and a.INWD_RecordType_var = b.WTINWD_RecordType_var " +
                " where  a.INWD_CL_Id=" + clId + " and a.INWD_SITE_Id=" + siteId + " and b.WTINWD_SupplierName_var <> '' and b.WTINWD_SupplierName_var = '" + supplierName + "' " +
                " Union " +
                " Select distinct(a.INWD_Building_var)  " +
                " From tbl_Inward as a join tbl_Other_Inward as b on a.INWD_RecordNo_int = b.OTINWD_RecordNo_int and a.INWD_RecordType_var = b.OTINWD_RecordType_var " +
                " where  a.INWD_CL_Id=" + clId + " and a.INWD_SITE_Id=" + siteId + " and b.OTINWD_SupplierName_var <> '' and b.OTINWD_SupplierName_var = '" + supplierName + "' " +
                " ) as x ";
                }
            }
            else
            {
                if (supplierName == "---All---")
                {
                    strQuery = "select distinct(building) from record_table where ";
                    strQuery = strQuery + " client_id=" + clId.ToString();
                    strQuery = strQuery + " and site_id=" + siteId.ToString();
                }
                else
                {
                    strQuery = "Select distinct(building) " +
                " from " +
                " ( " +
                " Select distinct(a.building) " +
                " From Record_Table as a join Aggregate_Table as b on a.Record_No= b.Record_No and a.Record_Type = b.Record_Type  " +
                " where  a.Client_ID=" + clId + " and a.Site_ID=" + siteId + " and b.SupplierName <> '' and b.SupplierName = '" + supplierName + "' " +
                " Union " +
                " Select distinct(a.building) " +
                " From Record_Table as a join BrickInward as b on a.Record_No= b.Record_No and a.Record_Type = b.Record_Type  " +
                " where  a.Client_ID=" + clId + " and a.Site_ID=" + siteId + " and b.Name_Of_Supp <> '' and b.Name_Of_Supp = '" + supplierName + "' " +
                " Union " +
                " Select distinct(a.building) " +
                " From Record_Table as a join CEMTChemicalInward as b on a.Record_No= b.RecordNo and a.Record_Type = b.RecordType " +
                " where  a.Client_ID=" + clId + " and a.Site_ID=" + siteId + " and b.SupplierName <> '' and b.SupplierName = '" + supplierName + "' " +
                " Union " +
                " Select distinct(a.building) " +
                " From Record_Table as a join Cement_Inward as b  on a.Record_No= b.Record_No and a.Record_Type = b.Record_Type  " +
                " where  a.Client_ID=" + clId + " and a.Site_ID=" + siteId + " and b.SupplierName <> '' and b.SupplierName = '" + supplierName + "' " +
                " Union " +
                " Select distinct(a.building) " +
                " From Record_Table as a join Fly_Ash_Inward as b on a.Record_No= b.Record_No and a.Record_Type = b.Record_Type  " +
                " where  a.Client_ID=" + clId + " and a.Site_ID=" + siteId + " and b.SupplierName <> '' and b.SupplierName = '" + supplierName + "' " +
                " Union " +
                " Select distinct(a.building) " +
                " From Record_Table as a join SolidInward as b on a.Record_No= b.Record_No and a.Record_Type = b.Record_Type  " +
                " where  a.Client_ID=" + clId + " and a.Site_ID=" + siteId + " and b.SupplierName <> '' and b.SupplierName = '" + supplierName + "' " +
                " Union " +
                " Select distinct(a.building) " +
                " From Record_Table as a join Pile_test_master as b on a.Record_No= b.Record_No and a.Record_Type = b.Record_Type  " +
                " where  a.Client_ID=" + clId + " and a.Site_ID=" + siteId + " and b.SupplierName <> '' and b.SupplierName = '" + supplierName + "' " +
                " Union " +
                " Select distinct(a.building) " +
                " From Record_Table as a join Steel_Test_Inward_Table as b on a.Record_No= b.Record_No and a.Record_Type = b.Record_Type  " +
                " where  a.Client_ID=" + clId + " and a.Site_ID=" + siteId + " and b.Name_Of_Supp <> '' and b.Name_Of_Supp = '" + supplierName + "' " +
                " Union " +
                " Select distinct(a.building) " +
                " From Record_Table as a join STChemicalInward as b on a.Record_No= b.RecordNo and a.Record_Type = b.RecordType  " +
                " where  a.Client_ID=" + clId + " and a.Site_ID=" + siteId + " and b.NameOfSupp <> '' and b.NameOfSupp = '" + supplierName + "' " +
                " Union " +
                " Select distinct(a.building) " +
                " From Record_Table as a join TileTestInward as b on a.Record_No= b.Record_No and a.Record_Type = b.Record_Type  " +
                " where  a.Client_ID=" + clId + " and a.Site_ID=" + siteId + " and b.SupplierName <> '' and b.SupplierName = '" + supplierName + "' " +
                " ) as x ";
                }
            }
            DataTable dt = getGeneralData(strQuery, db);
            return dt;
        }
        catch (Exception e)
        {
            throw e;
        }
    }
    public DataTable getInwardTypeList(double clId, double siteId, string supplierName, string buildingName, string db)
    {
        try
        {
            string strQuery = "";
            
            if (db == "veena2016" || db == "veena2020")
            {
                if (supplierName == "---All---" && buildingName == "---All---")
                {
                    strQuery = "Select distinct(INWD_RecordType_var) From tbl_Inward where ";
                    strQuery = strQuery + " INWD_CL_Id=" + clId.ToString();
                    strQuery = strQuery + " and INWD_SITE_Id=" + siteId.ToString();
                }
                else if (supplierName == "---All---" && buildingName != "---All---")
                {
                    strQuery = "Select distinct(INWD_RecordType_var) From tbl_Inward where ";
                    strQuery = strQuery + " INWD_CL_Id=" + clId.ToString();
                    strQuery = strQuery + " and INWD_SITE_Id=" + siteId.ToString();
                    strQuery = strQuery + " and INWD_Building_var = '" + buildingName + "'";
                }
                else if (supplierName != "---All---" )
                {
                    strQuery = "Select distinct(INWD_RecordType_var)" +
                " from " +
                " ( " +
                " Select distinct(a.INWD_RecordType_var)  " +
                " From tbl_Inward as a join tbl_Aggregate_Inward as b on a.INWD_RecordNo_int = b.AGGTINWD_RecordNo_int and a.INWD_RecordType_var = b.AGGTINWD_RecordType_var " +
                " where  a.INWD_CL_Id=" + clId + " and a.INWD_SITE_Id=" + siteId + " and (a.INWD_Building_var = '" + buildingName + "' or '" + buildingName + "'= '---All---') and b.AGGTINWD_SupplierName_var = '" + supplierName + "' " +
                " Union " +
                " Select distinct(a.INWD_RecordType_var)  " +
                " From tbl_Inward as a join tbl_Brick_Inward as b on a.INWD_RecordNo_int = b.BTINWD_RecordNo_int and a.INWD_RecordType_var = b.BTINWD_RecordType_var " +
                " where  a.INWD_CL_Id=" + clId + " and a.INWD_SITE_Id=" + siteId + " and (a.INWD_Building_var = '" + buildingName + "'  or '" + buildingName + "'= '---All---') and b.BTINWD_SupplierName_var = '" + supplierName + "' " +
                " Union " +
                " Select distinct(a.INWD_RecordType_var)  " +
                " From tbl_Inward as a join tbl_CementChemical_Inward as b on a.INWD_RecordNo_int = b.CCHINWD_RecordNo_int and a.INWD_RecordType_var = b.CCHINWD_RecordType_var " +
                " where  a.INWD_CL_Id=" + clId + " and a.INWD_SITE_Id=" + siteId + " and (a.INWD_Building_var = '" + buildingName + "'  or '" + buildingName + "'= '---All---') and b.CCHINWD_SupplierName_var = '" + supplierName + "' " +
                " Union " +
                " Select distinct(a.INWD_RecordType_var)  " +
                " From tbl_Inward as a join tbl_Cement_Inward as b on a.INWD_RecordNo_int = b.CEMTINWD_RecordNo_int and a.INWD_RecordType_var = b.CEMTINWD_RecordType_var " +
                " where  a.INWD_CL_Id=" + clId + " and a.INWD_SITE_Id=" + siteId + " and (a.INWD_Building_var = '" + buildingName + "'  or '" + buildingName + "'= '---All---') and b.CEMTINWD_SupplierName_var = '" + supplierName + "' " +
                " Union " +
                " Select distinct(a.INWD_RecordType_var)  " +
                " From tbl_Inward as a join tbl_Cube_Inward as b on a.INWD_RecordNo_int = b.CTINWD_RecordNo_int and a.INWD_RecordType_var = b.CTINWD_RecordType_var " +
                " where  a.INWD_CL_Id=" + clId + " and a.INWD_SITE_Id=" + siteId + " and (a.INWD_Building_var = '" + buildingName + "'  or '" + buildingName + "'= '---All---') and b.CTINWD_SupplierName_var = '" + supplierName + "' " +
                " Union " +
                " Select distinct(a.INWD_RecordType_var)  " +
                " From tbl_Inward as a join tbl_FlyAsh_Inward as b on a.INWD_RecordNo_int = b.FLYASHINWD_RecordNo_int and a.INWD_RecordType_var = b.FLYASHINWD_RecordType_var " +
                " where  a.INWD_CL_Id=" + clId + " and a.INWD_SITE_Id=" + siteId + " and (a.INWD_Building_var = '" + buildingName + "'  or '" + buildingName + "'= '---All---') and b.FLYASHINWD_SupplierName_var = '" + supplierName + "' " +
                " Union " +
                " Select distinct(a.INWD_RecordType_var)  " +
                " From tbl_Inward as a join tbl_Solid_Inward as b on a.INWD_RecordNo_int = b.SOLIDINWD_RecordNo_int and a.INWD_RecordType_var = b.SOLIDINWD_RecordType_var " +
                " where  a.INWD_CL_Id=" + clId + " and a.INWD_SITE_Id=" + siteId + " and (a.INWD_Building_var = '" + buildingName + "'  or '" + buildingName + "'= '---All---') and b.SOLIDINWD_SupplierName_var = '" + supplierName + "' " +
                " Union " +
                " Select distinct(a.INWD_RecordType_var)  " +
                " From tbl_Inward as a join tbl_Pavement_Inward as b on a.INWD_RecordNo_int = b.PTINWD_RecordNo_int and a.INWD_RecordType_var = b.PTINWD_RecordType_var " +
                " where  a.INWD_CL_Id=" + clId + " and a.INWD_SITE_Id=" + siteId + " and (a.INWD_Building_var = '" + buildingName + "'  or '" + buildingName + "'= '---All---') and b.PTINWD_SupplierName_var = '" + supplierName + "' " +
                " Union " +
                " Select distinct(a.INWD_RecordType_var)  " +
                " From tbl_Inward as a join tbl_Pile_Inward as b on a.INWD_RecordNo_int = b.PILEINWD_RecordNo_int and a.INWD_RecordType_var = b.PILEINWD_RecordType_var " +
                " where  a.INWD_CL_Id=" + clId + " and a.INWD_SITE_Id=" + siteId + " and (a.INWD_Building_var = '" + buildingName + "'  or '" + buildingName + "'= '---All---') and b.PILEINWD_SupplierName_var = '" + supplierName + "' " +
                " Union " +
                " Select distinct(a.INWD_RecordType_var)  " +
                " From tbl_Inward as a join tbl_Steel_Inward as b on a.INWD_RecordNo_int = b.STINWD_RecordNo_int and a.INWD_RecordType_var = b.STINWD_RecordType_var " +
                " where  a.INWD_CL_Id=" + clId + " and a.INWD_SITE_Id=" + siteId + " and (a.INWD_Building_var = '" + buildingName + "'  or '" + buildingName + "'= '---All---') and b.STINWD_SupplierName_var = '" + supplierName + "' " +
                " Union " +
                " Select distinct(a.INWD_RecordType_var)  " +
                " From tbl_Inward as a join tbl_SteelChemical_Inward as b on a.INWD_RecordNo_int = b.STCINWD_RecordNo_int and a.INWD_RecordType_var = b.STCINWD_RecordType_var " +
                " where  a.INWD_CL_Id=" + clId + " and a.INWD_SITE_Id=" + siteId + " and (a.INWD_Building_var = '" + buildingName + "'  or '" + buildingName + "'= '---All---') and b.STCINWD_SupplierName_var = '" + supplierName + "' " +
                " Union " +
                " Select distinct(a.INWD_RecordType_var)  " +
                " From tbl_Inward as a join tbl_Tile_Inward as b on a.INWD_RecordNo_int = b.TILEINWD_RecordNo_int and a.INWD_RecordType_var = b.TILEINWD_RecordType_var " +
                " where  a.INWD_CL_Id=" + clId + " and a.INWD_SITE_Id=" + siteId + " and (a.INWD_Building_var = '" + buildingName + "'  or '" + buildingName + "'= '---All---') and b.TILEINWD_SupplierName_var = '" + supplierName + "' " +
                " Union " +
                " Select distinct(a.INWD_RecordType_var)  " +
                " From tbl_Inward as a join tbl_Water_Inward as b on a.INWD_RecordNo_int = b.WTINWD_RecordNo_int and a.INWD_RecordType_var = b.WTINWD_RecordType_var " +
                " where  a.INWD_CL_Id=" + clId + " and a.INWD_SITE_Id=" + siteId + " and (a.INWD_Building_var = '" + buildingName + "'  or '" + buildingName + "'= '---All---') and b.WTINWD_SupplierName_var = '" + supplierName + "' " +
                 " Union " +
                " Select distinct(a.INWD_RecordType_var)  " +
                " From tbl_Inward as a join tbl_Other_Inward as b on a.INWD_RecordNo_int = b.OTINWD_RecordNo_int and a.INWD_RecordType_var = b.OTINWD_RecordType_var " +
                " where  a.INWD_CL_Id=" + clId + " and a.INWD_SITE_Id=" + siteId + " and (a.INWD_Building_var = '" + buildingName + "'  or '" + buildingName + "'= '---All---') and b.OTINWD_SupplierName_var = '" + supplierName + "' " +
                " ) as x ";
                }
            }
            else
            {   
                if (supplierName == "---All---" && buildingName == "---All---")
                {
                    strQuery = "select distinct(Record_Type) from record_table where ";
                    strQuery = strQuery + " client_id=" + clId.ToString();
                    strQuery = strQuery + " and site_id=" + siteId.ToString();
                }
                else if (supplierName == "---All---" && buildingName != "---All---")
                {
                    strQuery = "select distinct(Record_Type) from record_table where ";
                    strQuery = strQuery + " client_id=" + clId.ToString();
                    strQuery = strQuery + " and site_id=" + siteId.ToString();
                    strQuery = strQuery + " and building = '" + buildingName + "'";
                }
                else if (supplierName != "---All---")
                {
                    strQuery = "Select distinct(Record_Type) " +
                " from " +
                " ( " +
                " Select distinct(a.Record_Type) " +
                " From Record_Table as a join Aggregate_Table as b on a.Record_No= b.Record_No and a.Record_Type = b.Record_Type  " +
                " where  a.Client_ID=" + clId + " and a.Site_ID=" + siteId + " and (a.building = '" + buildingName + "'  or '" + buildingName + "'= '---All---') and b.SupplierName = '" + supplierName + "' " +
                " Union " +
                " Select distinct(a.Record_Type) " +
                " From Record_Table as a join BrickInward as b on a.Record_No= b.Record_No and a.Record_Type = b.Record_Type  " +
                " where  a.Client_ID=" + clId + " and a.Site_ID=" + siteId + " and (a.building = '" + buildingName + "'  or '" + buildingName + "'= '---All---') and b.Name_Of_Supp = '" + supplierName + "' " +
                " Union " +
                " Select distinct(a.Record_Type) " +
                " From Record_Table as a join CEMTChemicalInward as b on a.Record_No= b.RecordNo and a.Record_Type = b.RecordType " +
                " where  a.Client_ID=" + clId + " and a.Site_ID=" + siteId + " and (a.building = '" + buildingName + "'  or '" + buildingName + "'= '---All---') and b.SupplierName = '" + supplierName + "' " +
                " Union " +
                " Select distinct(a.Record_Type) " +
                " From Record_Table as a join Cement_Inward as b  on a.Record_No= b.Record_No and a.Record_Type = b.Record_Type  " +
                " where  a.Client_ID=" + clId + " and a.Site_ID=" + siteId + " and (a.building = '" + buildingName + "'  or '" + buildingName + "'= '---All---') and b.SupplierName = '" + supplierName + "' " +
                " Union " +
                " Select distinct(a.Record_Type) " +
                " From Record_Table as a join Fly_Ash_Inward as b on a.Record_No= b.Record_No and a.Record_Type = b.Record_Type  " +
                " where  a.Client_ID=" + clId + " and a.Site_ID=" + siteId + " and (a.building = '" + buildingName + "'  or '" + buildingName + "'= '---All---') and b.SupplierName = '" + supplierName + "' " +
                " Union " +
                " Select distinct(a.Record_Type) " +
                " From Record_Table as a join SolidInward as b on a.Record_No= b.Record_No and a.Record_Type = b.Record_Type  " +
                " where  a.Client_ID=" + clId + " and a.Site_ID=" + siteId + " and (a.building = '" + buildingName + "'  or '" + buildingName + "'= '---All---') and b.SupplierName = '" + supplierName + "' " +
                " Union " +
                " Select distinct(a.Record_Type) " +
                " From Record_Table as a join Pile_test_master as b on a.Record_No= b.Record_No and a.Record_Type = b.Record_Type  " +
                " where  a.Client_ID=" + clId + " and a.Site_ID=" + siteId + " and (a.building = '" + buildingName + "'  or '" + buildingName + "'= '---All---') and b.SupplierName = '" + supplierName + "' " +
                " Union " +
                " Select distinct(a.Record_Type) " +
                " From Record_Table as a join Steel_Test_Inward_Table as b on a.Record_No= b.Record_No and a.Record_Type = b.Record_Type  " +
                " where  a.Client_ID=" + clId + " and a.Site_ID=" + siteId + " and (a.building = '" + buildingName + "'  or '" + buildingName + "'= '---All---') and b.Name_Of_Supp = '" + supplierName + "' " +
                " Union " +
                " Select distinct(a.Record_Type) " +
                " From Record_Table as a join STChemicalInward as b on a.Record_No= b.RecordNo and a.Record_Type = b.RecordType  " +
                " where  a.Client_ID=" + clId + " and a.Site_ID=" + siteId + " and (a.building = '" + buildingName + "'  or '" + buildingName + "'= '---All---') and b.NameOfSupp = '" + supplierName + "' " +
                " Union " +
                " Select distinct(a.Record_Type) " +
                " From Record_Table as a join TileTestInward as b on a.Record_No= b.Record_No and a.Record_Type = b.Record_Type  " +
                " where  a.Client_ID=" + clId + " and a.Site_ID=" + siteId + " and (a.building = '" + buildingName + "'  or '" + buildingName + "'= '---All---') and b.SupplierName = '" + supplierName + "' " +
                " ) as x ";
                }
            }
            DataTable dt = getGeneralData(strQuery, db);
            return dt;
        }
        catch (Exception e)
        {
            throw e;
        }
    }

    public bool checkBillApproval(string referenceNo, string db)
    {
        try
        {
            bool billApprFlg = true;
            string strQuery = @"Select INWD_BILL_Id from tbl_Inward where INWD_ReferenceNo_int = " + referenceNo;
            DataTable dt = getGeneralData(strQuery, db);
            if (dt.Rows[0]["INWD_BILL_Id"] != null && dt.Rows[0]["INWD_BILL_Id"].ToString() != "")
            {
                strQuery = @"Select BILL_ClientApproveStatus_bit from tbl_Bill where BILL_Id = '" + dt.Rows[0]["INWD_BILL_Id"] + "'";
                dt = getGeneralData(strQuery, db);
                if (dt.Rows[0]["BILL_ClientApproveStatus_bit"].ToString() == "0")
                    billApprFlg = false;
            }
            else if (dt.Rows[0]["INWD_MonthlyBill_bit"].ToString() == "1")
            {
                strQuery = @"select BILL_ClientApproveStatus_bit from tbl_Bill as a join tbl_BillDetail as b on a.BILL_Id = b.BILLD_BILL_Id and BILLD_ReferenceNo_int = " + referenceNo;
                dt = getGeneralData(strQuery, db);
                if (dt.Rows[0]["BILL_ClientApproveStatus_bit"].ToString() == "0")
                    billApprFlg = false;
            }
            return billApprFlg;
        }
        catch (Exception e)
        {
            throw e;
        }
    }
    public DataTable getGeneralData(string mQueryString, string db)
    {

        try
        {
          //  OpenConnection(db);
            cn.Open();
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(mQueryString, cn);
            da.Fill(ds);
            dt = ds.Tables[0];
            ds.Dispose();
            CloseConnection();
            return dt;
            
        }
        catch (Exception e)
        {
            throw e;
        }

    }
    //    public DataTable getReportList(string rType,double clId,double siteId )
    //public DataTable getReportList(string bldg, double clId, double siteId)
    public DataTable getReportList(double clId, double siteId, string supplier, string bldg, string rType, string db)
    {
        string mySql, tblName = "",recType="";
        string fldRNo;
        try
        {
            DataSet ds = new DataSet();
            DataTable dtReturn = new DataTable();
            mySql = "select distinct(a.record_type) from record_table as a where  ";
            if (rType != "---All---")
            {
                if (rType == "CEMT")
                {
                    mySql = mySql + " (a.record_type='" + rType + "' or a.record_type='" + "CCH" + "') and ";
                }
                else if (rType == "ST")
                {
                    mySql = mySql + " (a.record_type='" + rType + "' or a.record_type='" + "STC" + "') and ";
                }
                else if (rType == "SOLID")
                {
                    mySql = mySql + " (a.record_type='" + rType + "' or a.record_type='" + "AAC" + "' or a.record_type='" + "BT-" + "') and ";
                }
                else
                {
                    mySql = mySql + " a.record_type='" + rType + "' and ";
                }
            }
            if (bldg.ToString() != "---All---")
            {
                mySql = mySql + " a.building='" + bldg + "' and ";
            }

            mySql = mySql + "  a.client_Id= " + clId.ToString("0");
            mySql = mySql + " and a.Site_Id =" + siteId.ToString("0");

            DataTable dt = getGeneralData(mySql,db);
            DataTable dt1 = new DataTable();
            foreach (DataRow rw in dt.Rows)
            {
                rType = rw["Record_type"].ToString();
                fldRNo = "Record_No";
                switch (rType)
                {
                    case "CT":
                        tblName = "Cube_in_record ";
                        recType = "Cube";
                        break;
                    case "ST":
                        tblName = "Steel_Test_Inward_Table ";
                        recType = "Steel";
                        break;
                    case "MF":
                        tblName = "MF_Inward_Table ";
                        recType = "Mix Design";
                        break;
                    case "CEMT":
                        tblName = "Cement_Inward ";
                        recType = "Cement";
                        break;
                    case "AGGT":
                        tblName = "Aggregate_Table ";
                        recType = "Aggregate";
                        break;
                    case "NDT":
                        tblName = "NDT_Table ";
                        recType = "Aggregate";
                        break;
                    case "CR":
                        tblName = "Core_test_master ";
                        recType = "Core Cutting";
                        break;
                    case "FLYASH":
                        tblName = "Fly_Ash_Inward";
                        recType = "FlyAsh";
                        break;
                    case "GGBS":
                        tblName = "tbl_Ggbs_Inward";
                        recType = "GGBS";
                        break;
                    case "GGBSCH":
                        tblName = "tbl_GgbsChemical_Inward";
                        recType = "GGBSCH";
                        break;
                    case "BT-":
                        tblName = "BrickInward ";
                        recType = "Brick";
                        break;
                    case "PT":
                        mySql = "Select c.recd_given_date as dateofreceiving ,'Pavement Block' as record_type,a.building as INWD_Building_var,b.refno ,convert(varchar,d.issuedate) as status " +
                            " e.Contact_No as INWD_ContactNo_var, e.Name as CONT_Name_var,'' as MFINWD_FinalIssueDt" +
                            " from record_table as a,in_out_rec_table as c   ,Cube_in_record  as b ,ptratein as d, Contact_Person_Info as e" +
                            " where  a.record_type='PT' and a.record_type=b.record_type  and a.record_type=c.record_type " +
                            " and a.record_no=c.record_no  and b.cube_inward_id=d.pt_id and a.record_no=b.Record_No " +
                            " and a.Cont_Per_ID = e.Cont_Per_ID ";
                        if (bldg.Trim() != "" && bldg != "---All---")
                        {
                            mySql += " and a.building='" + bldg + "'";
                        }
                        mySql += "  and a.client_Id= " + clId.ToString();
                        mySql += " and a.Site_Id =" + siteId.ToString();
                        mySql += " order by a.record_type ,a.record_no desc";
                        break;
                    case "SOLID":
                        mySql = "Select c.recd_given_date as dateofreceiving ,'Masonary Block' as record_type,a.building as INWD_Building_var,b.refno ,convert(varchar,d.issuedate) as status " +
                            " e.Contact_No as INWD_ContactNo_var, e.Name as CONT_Name_var,'' as MFINWD_FinalIssueDt" +
                                " from record_table as a,in_out_rec_table as c  ,SolidInward  as b ,solidratein as d , Contact_Person_Info as e" +
                                " where  a.record_type='SOLID' and a.record_type=b.record_type  and a.record_type=c.record_type " +
                                " and a.record_no=c.record_no  and b.solid_id=d.solid_id and a.record_no=b.Record_No " +
                        " and a.Cont_Per_ID = e.Cont_Per_ID ";
                        if (bldg.Trim() != "" && bldg != "---All---")
                        {
                            mySql += " and a.building='" + bldg + "'";
                        }
                        mySql += "  and a.client_Id= " + clId.ToString();
                        mySql += " and a.Site_Id =" + siteId.ToString();
                        mySql += " order by a.record_type ,a.record_no desc";
                        break;
                    case "PILE":
                        tblName = "Pile_test_master ";
                        recType = "Pile";
                        break;
                    //case "TILE":
                    //    tblName = "TileTestInward ";
                    //    break;
                    
                    case "STC":
                        tblName = "stChemicalInward";
                        recType = "Steel Chemical";
                        fldRNo = "RecordNo";
                        break;
                    case "CCH":
                        tblName = "CEMTChemicalInward";
                        recType = "Cement Chemical";
                        fldRNo = "RecordNo";
                        break;
                    case "WT":
                        tblName = "WaterTestingInward";
                        recType = "Water";
                        fldRNo = "RecordNo";
                        break;

                    default:
                        goto nxt;
                }
                if (rType == "MF")
                {
                    mySql = "select c.recd_given_date as dateofreceiving ,'Mix Design' as record_type,a.building as INWD_Building_var,set_of_mf as refno";
                }
                else if (rType == "PT" || rType == "SOLID")
                {
                    goto nxt1;
                }
                else if (rType == "NDT")
                {
                    mySql = "select c.recd_given_date as dateofreceiving ,'Non Destructive' as record_type,a.building as INWD_Building_var,convert(varchar,b.record_no) as refno";
                }
                else
                {
                    mySql = "select c.recd_given_date as dateofreceiving ,'"+recType+ "' as record_type,a.building as INWD_Building_var,b.refno";
                }

                if (rType == "FLYASH" || rType == "CEMT")
                {
                    mySql = mySql + " ,convert(varchar,b.prnDate) as status ";
                }
                else if (rType == "CR" || rType == "NDT")
                {
                    //mySql = mySql + " ,cdate(b.Report_Issue_Date)  as status ";
                    mySql = mySql + " ,convert(varchar,b.Report_Issue_Date) as status ";
                }
                else if (rType == "MF")
                {
                    mySql = mySql + " ,convert(varchar,b.MDIssueDate) as status,b.finalIssueDate ";
                }
                else
                {
                    mySql = mySql + " ,convert(varchar,b.issuedate) as status ";
                }
                mySql = mySql + ", e.Contact_No as INWD_ContactNo_var, e.Name as CONT_Name_var,'' as MFINWD_FinalIssueDt ";
                mySql = mySql + " from record_table as a,in_out_rec_table as c , Contact_Person_Info as e";
                mySql = mySql + "  ," + tblName + " as b  where ";
                mySql = mySql + " a.record_type='" + rType + "'";
                mySql = mySql + " and a.Cont_Per_ID = e.Cont_Per_ID ";
                if (rType == "WT" || rType == "STC" || rType == "CCH")
                {
                    mySql = mySql + " and a.record_type=b.recordtype ";
                }
                else
                {
                    mySql = mySql + " and a.record_type=b.record_type ";
                }

                mySql = mySql + " and a.record_type=c.record_type ";
                mySql = mySql + " and a.record_no=c.record_no ";
                mySql = mySql + " and a.record_no=b." + fldRNo;
                if (bldg.Trim() != "" && bldg != "---All---")
                {
                    mySql = mySql + " and a.building='" + bldg + "'";
                }
                if (supplier.Trim() != "" && supplier != "---All---")
                {
                    if (rType == "CR" || rType == "NDT" || rType == "MF" || rType == "CT" || rType == "WT" || rType == "TILE")
                    {
                        mySql += " and a.record_type <> '" + rType + "'";
                    }
                    else if (rType == "BT-")
                    {
                        mySql += " and b.Name_Of_Supp='" + supplier + "'";
                    }
                    else if (rType == "ST")
                    {
                        mySql += " and b.Name_Of_Supp='" + supplier + "'";
                    }
                    else if (rType == "STC")
                    {
                        mySql += " and b.NameOfSupp='" + supplier + "'";
                    }
                    else
                    {
                        mySql += " and b.SupplierName='" + supplier + "'";
                    }
                }
                mySql = mySql + "  and a.client_Id= " + clId.ToString();
                mySql = mySql + " and a.Site_Id =" + siteId.ToString();
                if (rType == "NDT")
                {
                    mySql += " group by c.recd_given_date  ,a.record_type,a.building,b.record_no,report_issue_date, e.Contact_No, e.Name";
                    mySql = mySql + " order by a.record_type,a.building,b.record_no,c.recd_given_date,report_issue_date, e.Contact_No, e.Name"; 
                }
                else
                {
                    mySql = mySql + " order by a.record_type ,a.record_no desc";
                }

            //DataTable dt1 = getGeneralData(mySql);            
            nxt1:
                SqlDataAdapter da = new SqlDataAdapter(mySql, cn);
                ds.Reset();
                da.Fill(ds);
                dt1 = new DataTable();
                dt1 = ds.Tables[0];
                dtReturn.Merge(dt1);
            nxt:
                tblName = "";
            }
            return dtReturn;
        }
        catch (Exception e)
        {
            throw e;
        }

    }
    public DataTable getReportList2016(double clId, double siteId, string supplier, string bldg, string rType, string db, string billId, string testType)
    {
        string mySql = "";
        try
        {
            
            #region  other report query
            string otherStr = @" Union SELECT a.INWD_ReceivedDate_dt as dateofreceiving, 'Other' as record_type, a.INWD_Building_var, 
                              b.OTINWD_ReferenceNo_var as refno, b.OTINWD_ApprovedDate_dt as status, c.CONT_Name_var, a.INWD_ContactNo_var, '' as MFINWD_FinalIssueDt  
                              FROM tbl_Inward AS a, tbl_Other_Inward AS b, tbl_Contact as c   
                              WHERE a.INWD_RecordType_var = 'OT' AND a.INWD_RecordType_var = b.OTINWD_RecordType_var  
                              AND a.INWD_RecordNo_int = b.OTINWD_RecordNo_int AND c.CONT_Id = a.INWD_CONT_Id ";
            otherStr += "and b.OTINWD_Section_var = 'AAC'";
            if (bldg.Trim() != "" && bldg != "---All---")
            {
                otherStr += " and a.INWD_Building_var='" + bldg + "'";
            }
            if (supplier.Trim() != "" && supplier != "---All---")
            {
                otherStr += " and b.OTINWD_SupplierName_var='" + supplier + "'";
            }
            otherStr += "  and a.INWD_CL_Id= " + clId.ToString();
            if (siteId != 0)
            {
                otherStr += " and a.INWD_SITE_Id =" + siteId.ToString("0");
            }
            if (billId != "")
            {
                otherStr += " and (a.INWD_BILL_Id = '" + billId + "'";
                otherStr += " Or (Select count(BILLD_BILL_Id) from tbl_BillDetail Where BILLD_ReferenceNo_int = a.INWD_ReferenceNo_int and BILLD_BILL_Id = '" + billId + "') > 0 ";
                otherStr += " Or (Select count(MBILLD_BILL_Id) from tbl_BillDetailMonthly Where MBILLD_ReferenceNo_int = a.INWD_ReferenceNo_int and MBILLD_BILL_Id = '" + billId + "') > 0 ";
                otherStr += ")";
            }
            #endregion
            #region query
            switch (rType)
            {
                case "CT":
                    mySql = " SELECT a.INWD_ReceivedDate_dt as dateofreceiving, 'Cube' as record_type,a.INWD_Building_var, b.CTINWD_ReferenceNo_var as refno ,b.CTINWD_ApprovedDate_dt as status" +
                            " ,c.CONT_Name_var, a.INWD_ContactNo_var, '' as MFINWD_FinalIssueDt " +
                            " FROM tbl_Inward AS a, tbl_Cube_Inward AS b, tbl_Contact as c " +
                            " WHERE a.INWD_RecordType_var = 'CT' AND a.INWD_RecordType_var = b.CTINWD_RecordType_var " +
                            " AND a.INWD_RecordNo_int = b.CTINWD_RecordNo_int" +
                            " AND c.CONT_Id = a.INWD_CONT_Id";
                    if (bldg.Trim() != "" && bldg != "---All---")
                    {
                        mySql += " and a.INWD_Building_var='" + bldg + "'";
                    }
                    if (supplier.Trim() != "" && supplier != "---All---"  )
                    {
                        mySql += " and b.CTINWD_SupplierName_var='" + supplier + "'";
                    }
                    mySql += "  and a.INWD_CL_Id= " + clId.ToString();
                    if (siteId != 0)
                    {
                        mySql = mySql + " and a.INWD_SITE_Id =" + siteId.ToString("0");
                    }
                    if (billId != "")
                    {
                        mySql = mySql + " and (a.INWD_BILL_Id = '" + billId + "'";
                        mySql = mySql + " Or (Select count(BILLD_BILL_Id) from tbl_BillDetail Where BILLD_ReferenceNo_int = a.INWD_ReferenceNo_int and BILLD_BILL_Id = '" + billId + "') > 0 ";
                        mySql = mySql + " Or (Select count(MBILLD_BILL_Id) from tbl_BillDetailMonthly Where MBILLD_ReferenceNo_int = a.INWD_ReferenceNo_int and MBILLD_BILL_Id = '" + billId + "') > 0 ";
                        mySql = mySql + ")";
                    }
                    mySql += otherStr.Replace("'AAC'", "'CT'");
                    //mySql += " order by a.INWD_RecordType_var ,a.INWD_RecordNo_int desc";
                    mySql += " order by a.INWD_ReceivedDate_dt desc";
                    break;
                case "ST":
                    if (testType == "---All---" || testType == "Physical")
                    {
                        mySql = " SELECT a.INWD_ReceivedDate_dt as dateofreceiving, 'Steel' as record_type,a.INWD_Building_var, b.STINWD_ReferenceNo_var as refno ,b.STINWD_ApprovedDate_dt as status" +
                                " ,c.CONT_Name_var, a.INWD_ContactNo_var, '' as MFINWD_FinalIssueDt " +
                                " FROM tbl_Inward AS a, tbl_Steel_Inward AS b, tbl_Contact as c " +
                                " WHERE a.INWD_RecordType_var = 'ST' AND a.INWD_RecordType_var = b.STINWD_RecordType_var " +
                                " AND a.INWD_RecordNo_int = b.STINWD_RecordNo_int" +
                                " AND c.CONT_Id = a.INWD_CONT_Id";
                        if (bldg.Trim() != "" && bldg != "---All---")
                        {
                            mySql += " and a.INWD_Building_var='" + bldg + "'";
                        }
                        if (supplier.Trim() != "" && supplier != "---All---")
                        {
                            mySql += " and b.STINWD_SupplierName_var='" + supplier + "'";
                        }
                        mySql += "  and a.INWD_CL_Id= " + clId.ToString();
                        if (siteId != 0)
                        {
                            mySql = mySql + " and a.INWD_SITE_Id =" + siteId.ToString("0");
                        }
                        if (billId != "")
                        {
                            mySql = mySql + " and (a.INWD_BILL_Id = '" + billId + "'";
                            mySql = mySql + " Or (Select count(BILLD_BILL_Id) from tbl_BillDetail Where BILLD_ReferenceNo_int = a.INWD_ReferenceNo_int and BILLD_BILL_Id = '" + billId + "') > 0 ";
                            mySql = mySql + " Or (Select count(MBILLD_BILL_Id) from tbl_BillDetailMonthly Where MBILLD_ReferenceNo_int = a.INWD_ReferenceNo_int and MBILLD_BILL_Id = '" + billId + "') > 0 ";
                            mySql = mySql + ")";
                        }
                        mySql += otherStr.Replace("'AAC'", "'ST'");
                    }
                    if (testType == "---All---")
                    {
                        mySql += " Union ";
                    }
                    if (testType == "---All---" || testType == "Chemical")
                    {
                        mySql += " SELECT a.INWD_ReceivedDate_dt as dateofreceiving, 'Steel Chemical' as record_type,a.INWD_Building_var, b.STCINWD_ReferenceNo_var as refno ,b.STCINWD_ApprovedDate_dt as status" +
                        " ,c.CONT_Name_var, a.INWD_ContactNo_var, '' as MFINWD_FinalIssueDt " +
                        " FROM tbl_Inward AS a, tbl_SteelChemical_Inward AS b, tbl_Contact as c  " +
                            " WHERE a.INWD_RecordType_var = 'STC' AND a.INWD_RecordType_var = b.STCINWD_RecordType_var " +
                            " AND a.INWD_RecordNo_int = b.STCINWD_RecordNo_int" +
                            " AND c.CONT_Id = a.INWD_CONT_Id";
                        if (bldg.Trim() != "" && bldg != "---All---")
                        {
                            mySql += " and a.INWD_Building_var='" + bldg + "'";
                        }
                        if (supplier.Trim() != "" && supplier != "---All---")
                        {
                            mySql += " and b.STCINWD_SupplierName_var='" + supplier + "'";
                        }
                        mySql += "  and a.INWD_CL_Id= " + clId.ToString();
                        if (siteId != 0)
                        {
                            mySql = mySql + " and a.INWD_SITE_Id =" + siteId.ToString("0");
                        }
                        if (billId != "")
                        {
                            mySql = mySql + " and (a.INWD_BILL_Id = '" + billId + "'";
                            mySql = mySql + " Or (Select count(BILLD_BILL_Id) from tbl_BillDetail Where BILLD_ReferenceNo_int = a.INWD_ReferenceNo_int and BILLD_BILL_Id = '" + billId + "') > 0 ";
                            mySql = mySql + " Or (Select count(MBILLD_BILL_Id) from tbl_BillDetailMonthly Where MBILLD_ReferenceNo_int = a.INWD_ReferenceNo_int and MBILLD_BILL_Id = '" + billId + "') > 0 ";
                            mySql = mySql + ")";
                        }
                        mySql += otherStr.Replace("'AAC'", "'STC'");
                    }
                    //mySql += " order by a.INWD_RecordType_var ,a.INWD_RecordNo_int desc";
                    mySql += " order by a.INWD_ReceivedDate_dt desc";
                    break;
                case "MF":
                    mySql = " SELECT a.INWD_ReceivedDate_dt as dateofreceiving,'Mix Design' as record_type,a.INWD_Building_var, b.MFINWD_ReferenceNo_var as refno ,b.MFINWD_ApprovedDate_dt as status" +
                        " ,c.CONT_Name_var, a.INWD_ContactNo_var, CONVERT(varchar,b.MFINWD_FinalIssueDt) as MFINWD_FinalIssueDt " +
                        " FROM tbl_Inward AS a, tbl_MixDesign_Inward AS b , tbl_Contact as c " +
                            " WHERE a.INWD_RecordType_var = 'MF' AND a.INWD_RecordType_var = b.MFINWD_RecordType_var " +
                            " AND a.INWD_RecordNo_int = b.MFINWD_RecordNo_int" +
                            " AND c.CONT_Id = a.INWD_CONT_Id";
                    if (bldg.Trim() != "" && bldg != "---All---")
                    {
                        mySql += " and a.INWD_Building_var='" + bldg + "'";
                    }
                    if (supplier.Trim() != "" && supplier != "---All---") //mf, cr, ndt not having suppiler name
                    {
                        mySql += " and a.INWD_RecordType_var <> 'MF'";
                    }

                    mySql += "  and a.INWD_CL_Id= " + clId.ToString();
                    if (siteId != 0)
                    {
                        mySql = mySql + " and a.INWD_SITE_Id =" + siteId.ToString("0");
                    }
                    if (billId != "")
                    {
                        mySql = mySql + " and (a.INWD_BILL_Id = '" + billId + "'";
                        mySql = mySql + " Or (Select count(BILLD_BILL_Id) from tbl_BillDetail Where BILLD_ReferenceNo_int = a.INWD_ReferenceNo_int and BILLD_BILL_Id = '" + billId + "') > 0 ";
                        mySql = mySql + " Or (Select count(MBILLD_BILL_Id) from tbl_BillDetailMonthly Where MBILLD_ReferenceNo_int = a.INWD_ReferenceNo_int and MBILLD_BILL_Id = '" + billId + "') > 0 ";
                        mySql = mySql + ")";
                    }
                    mySql += otherStr.Replace("'AAC'", "'MF'");
                    //mySql += " order by a.INWD_RecordType_var ,a.INWD_RecordNo_int desc";
                    mySql += " order by a.INWD_ReceivedDate_dt desc";
                    break;
                case "CEMT":
                    if (testType == "---All---" || testType == "Physical")
                    {
                        mySql = " SELECT a.INWD_ReceivedDate_dt as dateofreceiving, 'Cement' as record_type,a.INWD_Building_var, b.CEMTINWD_ReferenceNo_var as refno ,b.CEMTINWD_ApprovedDate_dt as status" +
                        " ,c.CONT_Name_var, a.INWD_ContactNo_var, '' as MFINWD_FinalIssueDt " +
                        " FROM tbl_Inward AS a, tbl_Cement_Inward AS b, tbl_Contact as c  " +
                            " WHERE a.INWD_RecordType_var = 'CEMT' AND a.INWD_RecordType_var = b.CEMTINWD_RecordType_var " +
                            " AND a.INWD_RecordNo_int = b.CEMTINWD_RecordNo_int" +
                            " AND c.CONT_Id = a.INWD_CONT_Id";
                        if (bldg.Trim() != "" && bldg != "---All---")
                        {
                            mySql += " and a.INWD_Building_var='" + bldg + "'";
                        }
                        if (supplier.Trim() != "" && supplier != "---All---")
                        {
                            mySql += " and b.CEMTINWD_SupplierName_var='" + supplier + "'";
                        }
                        mySql += "  and a.INWD_CL_Id= " + clId.ToString();
                        if (siteId != 0)
                        {
                            mySql = mySql + " and a.INWD_SITE_Id =" + siteId.ToString("0");
                        }
                        if (billId != "")
                        {
                            mySql = mySql + " and (a.INWD_BILL_Id = '" + billId + "'";
                            mySql = mySql + " Or (Select count(BILLD_BILL_Id) from tbl_BillDetail Where BILLD_ReferenceNo_int = a.INWD_ReferenceNo_int and BILLD_BILL_Id = '" + billId + "') > 0 ";
                            mySql = mySql + " Or (Select count(MBILLD_BILL_Id) from tbl_BillDetailMonthly Where MBILLD_ReferenceNo_int = a.INWD_ReferenceNo_int and MBILLD_BILL_Id = '" + billId + "') > 0 ";
                            mySql = mySql + ")";
                        }
                        mySql += otherStr.Replace("'AAC'", "'CEMT'");
                    }
                    if (testType == "---All---")
                    {
                        mySql += " Union ";
                    }
                    if (testType == "---All---" || testType == "Chemical")
                    {
                        mySql += " SELECT a.INWD_ReceivedDate_dt as dateofreceiving, 'Cement Chemical' as record_type,a.INWD_Building_var, b.CCHINWD_ReferenceNo_var as refno ,b.CCHINWD_ApprovedDate_dt as status" +
                        " ,c.CONT_Name_var, a.INWD_ContactNo_var, '' as MFINWD_FinalIssueDt " +
                        " FROM tbl_Inward AS a, tbl_CementChemical_Inward AS b, tbl_Contact as c  " +
                            " WHERE a.INWD_RecordType_var = 'CCH' AND a.INWD_RecordType_var = b.CCHINWD_RecordType_var " +
                            " AND a.INWD_RecordNo_int = b.CCHINWD_RecordNo_int" +
                            " AND c.CONT_Id = a.INWD_CONT_Id";
                        if (bldg.Trim() != "" && bldg != "---All---")
                        {
                            mySql += " and a.INWD_Building_var='" + bldg + "'";
                        }
                        if (supplier.Trim() != "" && supplier != "---All---")
                        {
                            mySql += " and b.CCHINWD_SupplierName_var='" + supplier + "'";
                        }
                        mySql += "  and a.INWD_CL_Id= " + clId.ToString();
                        if (siteId != 0)
                        {
                            mySql = mySql + " and a.INWD_SITE_Id =" + siteId.ToString("0");
                        }
                        if (billId != "")
                        {
                            mySql = mySql + " and (a.INWD_BILL_Id = '" + billId + "'";
                            mySql = mySql + " Or (Select count(BILLD_BILL_Id) from tbl_BillDetail Where BILLD_ReferenceNo_int = a.INWD_ReferenceNo_int and BILLD_BILL_Id = '" + billId + "') > 0 ";
                            mySql = mySql + " Or (Select count(MBILLD_BILL_Id) from tbl_BillDetailMonthly Where MBILLD_ReferenceNo_int = a.INWD_ReferenceNo_int and MBILLD_BILL_Id = '" + billId + "') > 0 ";
                            mySql = mySql + ")";
                        }
                        mySql += otherStr.Replace("'AAC'", "'CCH'");
                    }
                    //mySql += " order by a.INWD_RecordType_var ,a.INWD_RecordNo_int desc";
                    mySql += " order by a.INWD_ReceivedDate_dt desc";
                    break;
                    /// GGBS
                case "GGBS":
                    if (testType == "---All---" || testType == "Physical")
                    {
                        mySql = " SELECT a.INWD_ReceivedDate_dt as dateofreceiving, 'GGBS' as record_type,a.INWD_Building_var, b.GGBSINWD_ReferenceNo_var as refno ,b.ggbsINWD_ApprovedDate_dt as status" +
                        " ,c.CONT_Name_var, a.INWD_ContactNo_var, '' as MFINWD_FinalIssueDt " +
                        " FROM tbl_Inward AS a, tbl_ggbs_Inward AS b, tbl_Contact as c  " +
                            " WHERE a.INWD_RecordType_var = 'GGBS' AND a.INWD_RecordType_var = b.GGBSINWD_RecordType_var " +
                            " AND a.INWD_RecordNo_int = b.ggbsINWD_RecordNo_int" +
                            " AND c.CONT_Id = a.INWD_CONT_Id";
                        if (bldg.Trim() != "" && bldg != "---All---")
                        {
                            mySql += " and a.INWD_Building_var='" + bldg + "'";
                        }
                        if (supplier.Trim() != "" && supplier != "---All---")
                        {
                            mySql += " and b.ggbsINWD_SupplierName_var='" + supplier + "'";
                        }
                        mySql += "  and a.INWD_CL_Id= " + clId.ToString();
                        if (siteId != 0)
                        {
                            mySql = mySql + " and a.INWD_SITE_Id =" + siteId.ToString("0");
                        }
                        if (billId != "")
                        {
                            mySql = mySql + " and (a.INWD_BILL_Id = '" + billId + "'";
                            mySql = mySql + " Or (Select count(BILLD_BILL_Id) from tbl_BillDetail Where BILLD_ReferenceNo_int = a.INWD_ReferenceNo_int and BILLD_BILL_Id = '" + billId + "') > 0 ";
                            mySql = mySql + " Or (Select count(MBILLD_BILL_Id) from tbl_BillDetailMonthly Where MBILLD_ReferenceNo_int = a.INWD_ReferenceNo_int and MBILLD_BILL_Id = '" + billId + "') > 0 ";
                            mySql = mySql + ")";
                        }
                        mySql += otherStr.Replace("'AAC'", "'GGBS'");
                    }
                    if (testType == "---All---")
                    {
                        mySql += " Union ";
                    }
                    if (testType == "---All---" || testType == "Chemical")
                    {
                        mySql += " SELECT a.INWD_ReceivedDate_dt as dateofreceiving, 'GGBS Chemical' as record_type,a.INWD_Building_var, b.ggbschINWD_ReferenceNo_var as refno ,b.ggbschINWD_ApprovedDate_dt as status" +
                        " ,c.CONT_Name_var, a.INWD_ContactNo_var, '' as MFINWD_FinalIssueDt " +
                        " FROM tbl_Inward AS a, tbl_GgbsChemical_Inward AS b, tbl_Contact as c  " +
                            " WHERE a.INWD_RecordType_var = 'GGBSCH' AND a.INWD_RecordType_var = b.ggbschINWD_RecordType_var " +
                            " AND a.INWD_RecordNo_int = b.ggbschINWD_RecordNo_int" +
                            " AND c.CONT_Id = a.INWD_CONT_Id";
                        if (bldg.Trim() != "" && bldg != "---All---")
                        {
                            mySql += " and a.INWD_Building_var='" + bldg + "'";
                        }
                        if (supplier.Trim() != "" && supplier != "---All---")
                        {
                            mySql += " and b.ggbschINWD_SupplierName_var='" + supplier + "'";
                        }
                        mySql += "  and a.INWD_CL_Id= " + clId.ToString();
                        if (siteId != 0)
                        {
                            mySql = mySql + " and a.INWD_SITE_Id =" + siteId.ToString("0");
                        }
                        if (billId != "")
                        {
                            mySql = mySql + " and (a.INWD_BILL_Id = '" + billId + "'";
                            mySql = mySql + " Or (Select count(BILLD_BILL_Id) from tbl_BillDetail Where BILLD_ReferenceNo_int = a.INWD_ReferenceNo_int and BILLD_BILL_Id = '" + billId + "') > 0 ";
                            mySql = mySql + " Or (Select count(MBILLD_BILL_Id) from tbl_BillDetailMonthly Where MBILLD_ReferenceNo_int = a.INWD_ReferenceNo_int and MBILLD_BILL_Id = '" + billId + "') > 0 ";
                            mySql = mySql + ")";
                        }
                        mySql += otherStr.Replace("'AAC'", "'GGBSCH'");
                    }
                    //mySql += " order by a.INWD_RecordType_var ,a.INWD_RecordNo_int desc";
                    mySql += " order by a.INWD_ReceivedDate_dt desc";
                    break;
                    //ggbs end
                case "AGGT":
                    mySql = " SELECT a.INWD_ReceivedDate_dt as dateofreceiving, 'Aggregate' as record_type,a.INWD_Building_var, b.AGGTINWD_ReferenceNo_var as refno ,b.AGGTINWD_ApprovedDate_dt as status" +
                        " ,c.CONT_Name_var, a.INWD_ContactNo_var, '' as MFINWD_FinalIssueDt " +
                        " FROM tbl_Inward AS a, tbl_Aggregate_Inward AS b, tbl_Contact as c  " +
                            " WHERE a.INWD_RecordType_var = 'AGGT' AND a.INWD_RecordType_var = b.AGGTINWD_RecordType_var " +
                            " AND a.INWD_RecordNo_int = b.AGGTINWD_RecordNo_int" +
                            " AND c.CONT_Id = a.INWD_CONT_Id";
                    if (bldg.Trim() != "" && bldg != "---All---")
                    {
                        mySql += " and a.INWD_Building_var='" + bldg + "'";
                    }
                    if (supplier.Trim() != "" && supplier != "---All---")
                    {
                        mySql += " and b.AGGTINWD_SupplierName_var='" + supplier + "'";
                    }
                    mySql += "  and a.INWD_CL_Id= " + clId.ToString();
                    if (siteId != 0)
                    {
                        mySql = mySql + " and a.INWD_SITE_Id =" + siteId.ToString("0");
                    }
                    if (billId != "")
                    {
                        mySql = mySql + " and (a.INWD_BILL_Id = '" + billId + "'";
                        mySql = mySql + " Or (Select count(BILLD_BILL_Id) from tbl_BillDetail Where BILLD_ReferenceNo_int = a.INWD_ReferenceNo_int and BILLD_BILL_Id = '" + billId + "') > 0 ";
                        mySql = mySql + " Or (Select count(MBILLD_BILL_Id) from tbl_BillDetailMonthly Where MBILLD_ReferenceNo_int = a.INWD_ReferenceNo_int and MBILLD_BILL_Id = '" + billId + "') > 0 ";
                        mySql = mySql + ")";
                    }
                    mySql += otherStr.Replace("'AAC'", "'AGGT'");
                    //mySql += " order by a.INWD_RecordType_var ,a.INWD_RecordNo_int desc";
                    mySql += " order by a.INWD_ReceivedDate_dt desc";
                    break;
                case "AAC":
                    mySql = " SELECT a.INWD_ReceivedDate_dt as dateofreceiving, 'AAC Block' as record_type,a.INWD_Building_var, b.AACINWD_ReferenceNo_var as refno ,b.AACINWD_ApprovedDate_dt as status" +
                        " ,c.CONT_Name_var, a.INWD_ContactNo_var, '' as MFINWD_FinalIssueDt " +
                        " FROM tbl_Inward AS a, tbl_AAC_Inward AS b, tbl_Contact as c  " +
                            " WHERE a.INWD_RecordType_var = 'AAC' AND a.INWD_RecordType_var = b.AACINWD_RecordType_var " +
                            " AND a.INWD_RecordNo_int = b.AACINWD_RecordNo_int" +
                            " AND c.CONT_Id = a.INWD_CONT_Id";
                    if (bldg.Trim() != "" && bldg != "---All---")
                    {
                        mySql += " and a.INWD_Building_var='" + bldg + "'";
                    }
                    if (supplier.Trim() != "" && supplier != "---All---")
                    {
                        mySql += " and b.AACINWD_SupplierName_var='" + supplier + "'";
                    }
                    mySql += "  and a.INWD_CL_Id= " + clId.ToString();
                    if (siteId != 0)
                    {
                        mySql = mySql + " and a.INWD_SITE_Id =" + siteId.ToString("0");
                    }
                    if (billId != "")
                    {
                        mySql = mySql + " and (a.INWD_BILL_Id = '" + billId + "'";
                        mySql = mySql + " Or (Select count(BILLD_BILL_Id) from tbl_BillDetail Where BILLD_ReferenceNo_int = a.INWD_ReferenceNo_int and BILLD_BILL_Id = '" + billId + "') > 0 ";
                        mySql = mySql + " Or (Select count(MBILLD_BILL_Id) from tbl_BillDetailMonthly Where MBILLD_ReferenceNo_int = a.INWD_ReferenceNo_int and MBILLD_BILL_Id = '" + billId + "') > 0 ";
                        mySql = mySql + ")";
                    }
                    mySql += otherStr.Replace("'AAC'", "'AAC'");
                    //mySql += " order by a.INWD_RecordType_var ,a.INWD_RecordNo_int desc";
                    mySql += " order by a.INWD_ReceivedDate_dt desc";
                    break;
                case "NDT":
                    mySql = " SELECT a.INWD_ReceivedDate_dt as dateofreceiving, 'Non Destructive' as record_type,a.INWD_Building_var, b.NDTINWD_ReferenceNo_var as refno ,b.NDTINWD_ApprovedDate_dt as status" +
                        " ,c.CONT_Name_var, a.INWD_ContactNo_var, '' as MFINWD_FinalIssueDt " +
                        " FROM tbl_Inward AS a, tbl_NDT_Inward AS b, tbl_Contact as c  " +
                            " WHERE a.INWD_RecordType_var = 'NDT' AND a.INWD_RecordType_var = b.NDTINWD_RecordType_var " +
                            " AND a.INWD_RecordNo_int = b.NDTINWD_RecordNo_int" +
                            " AND c.CONT_Id = a.INWD_CONT_Id";
                    if (bldg.Trim() != "" && bldg != "---All---")
                    {
                        mySql += " and a.INWD_Building_var='" + bldg + "'";
                    }
                    if (supplier.Trim() != "" && supplier != "---All---") //mf, cr, ndt not having suppiler name
                    {
                        mySql += " and a.INWD_RecordType_var <> 'NDT'";
                    }
                    mySql += "  and a.INWD_CL_Id= " + clId.ToString();
                    if (siteId != 0)
                    {
                        mySql = mySql + " and a.INWD_SITE_Id =" + siteId.ToString("0");
                    }
                    if (billId != "")
                    {
                        mySql = mySql + " and (a.INWD_BILL_Id = '" + billId + "'";
                        mySql = mySql + " Or (Select count(BILLD_BILL_Id) from tbl_BillDetail Where BILLD_ReferenceNo_int = a.INWD_ReferenceNo_int and BILLD_BILL_Id = '" + billId + "') > 0 ";
                        mySql = mySql + " Or (Select count(MBILLD_BILL_Id) from tbl_BillDetailMonthly Where MBILLD_ReferenceNo_int = a.INWD_ReferenceNo_int and MBILLD_BILL_Id = '" + billId + "') > 0 ";
                        mySql = mySql + ")";
                    }
                    mySql += otherStr.Replace("'AAC'", "'NDT'");
                    //mySql += " order by a.INWD_RecordType_var ,a.INWD_RecordNo_int desc";
                    mySql += " order by a.INWD_ReceivedDate_dt desc";
                    break;
                case "CR":
                    mySql = " SELECT a.INWD_ReceivedDate_dt as dateofreceiving,'Core' as record_type,a.INWD_Building_var, b.CRINWD_ReferenceNo_var as refno ,b.CRINWD_ApprovedDate_dt as status" +
                        " ,c.CONT_Name_var, a.INWD_ContactNo_var, '' as MFINWD_FinalIssueDt " +
                        " FROM tbl_Inward AS a, tbl_Core_Inward AS b, tbl_Contact as c  " +
                            " WHERE a.INWD_RecordType_var = 'CR' AND a.INWD_RecordType_var = b.CRINWD_RecordType_var " +
                            " AND a.INWD_RecordNo_int = b.CRINWD_RecordNo_int" +
                            " AND c.CONT_Id = a.INWD_CONT_Id";
                    if (bldg.Trim() != "" && bldg != "---All---")
                    {
                        mySql += " and a.INWD_Building_var='" + bldg + "'";
                    }
                    if (supplier.Trim() != "" && supplier != "---All---") //mf, cr, ndt not having suppiler name
                    {
                        mySql += " and a.INWD_RecordType_var <> 'CR'";
                    }
                    mySql += "  and a.INWD_CL_Id= " + clId.ToString();
                    if (siteId != 0)
                    {
                        mySql = mySql + " and a.INWD_SITE_Id =" + siteId.ToString("0");
                    }
                    if (billId != "")
                    {
                        mySql = mySql + " and (a.INWD_BILL_Id = '" + billId + "'";
                        mySql = mySql + " Or (Select count(BILLD_BILL_Id) from tbl_BillDetail Where BILLD_ReferenceNo_int = a.INWD_ReferenceNo_int and BILLD_BILL_Id = '" + billId + "') > 0 ";
                        mySql = mySql + " Or (Select count(MBILLD_BILL_Id) from tbl_BillDetailMonthly Where MBILLD_ReferenceNo_int = a.INWD_ReferenceNo_int and MBILLD_BILL_Id = '" + billId + "') > 0 ";
                        mySql = mySql + ")";
                    }
                    mySql += otherStr.Replace("'AAC'", "'CR'");
                    //mySql += " order by a.INWD_RecordType_var ,a.INWD_RecordNo_int desc";
                    mySql += " order by a.INWD_ReceivedDate_dt desc";
                    break;
                case "FLYASH":
                    if (testType == "---All---" || testType == "Physical")
                    {
                        mySql = " SELECT a.INWD_ReceivedDate_dt as dateofreceiving, 'FlyAsh' as record_type,a.INWD_Building_var, b.FLYASHINWD_ReferenceNo_var as refno ,b.FLYASHINWD_ApprovedDate_dt as status" +
                        " ,c.CONT_Name_var, a.INWD_ContactNo_var, '' as MFINWD_FinalIssueDt " +
                        " FROM tbl_Inward AS a, tbl_FlyAsh_Inward AS b, tbl_Contact as c  " +
                            " WHERE a.INWD_RecordType_var = 'FLYASH' AND a.INWD_RecordType_var = b.FLYASHINWD_RecordType_var " +
                            " AND a.INWD_RecordNo_int = b.FLYASHINWD_RecordNo_int" +
                            " AND c.CONT_Id = a.INWD_CONT_Id";
                        if (bldg.Trim() != "" && bldg != "---All---")
                        {
                            mySql += " and a.INWD_Building_var='" + bldg + "'";
                        }
                        if (supplier.Trim() != "" && supplier != "---All---")
                        {
                            mySql += " and b.FLYASHINWD_SupplierName_var='" + supplier + "'";
                        }
                        mySql += "  and a.INWD_CL_Id= " + clId.ToString();
                        if (siteId != 0)
                        {
                            mySql = mySql + " and a.INWD_SITE_Id =" + siteId.ToString("0");
                        }
                        if (billId != "")
                        {
                            mySql = mySql + " and (a.INWD_BILL_Id = '" + billId + "'";
                            mySql = mySql + " Or (Select count(BILLD_BILL_Id) from tbl_BillDetail Where BILLD_ReferenceNo_int = a.INWD_ReferenceNo_int and BILLD_BILL_Id = '" + billId + "') > 0 ";
                            mySql = mySql + " Or (Select count(MBILLD_BILL_Id) from tbl_BillDetailMonthly Where MBILLD_ReferenceNo_int = a.INWD_ReferenceNo_int and MBILLD_BILL_Id = '" + billId + "') > 0 ";
                            mySql = mySql + ")";
                        }
                        mySql += otherStr.Replace("'AAC'", "'FLYASH'");
                    }
                    if (testType == "---All---")
                    {
                        mySql += " Union ";
                    }
                    if (testType == "---All---" || testType == "Chemical")
                    {
                        mySql += otherStr.Replace("'AAC'", "'FLYASHCH'").Replace("Union", "");
                    }
                    //mySql += " order by a.INWD_RecordType_var ,a.INWD_RecordNo_int desc";
                    mySql += " order by a.INWD_ReceivedDate_dt desc";
                    break;
                case "BT-":
                    mySql = " SELECT a.INWD_ReceivedDate_dt as dateofreceiving, 'Brick' as record_type,a.INWD_Building_var, b.BTINWD_ReferenceNo_var as refno ,b.BTINWD_ApprovedDate_dt as status" +
                        " ,c.CONT_Name_var, a.INWD_ContactNo_var, '' as MFINWD_FinalIssueDt " +
                        " FROM tbl_Inward AS a, tbl_Brick_Inward AS b, tbl_Contact as c  " +
                            " WHERE a.INWD_RecordType_var = 'BT-' AND a.INWD_RecordType_var = b.BTINWD_RecordType_var " +
                            " AND a.INWD_RecordNo_int = b.BTINWD_RecordNo_int" +
                            " AND c.CONT_Id = a.INWD_CONT_Id";
                    if (bldg.Trim() != "" && bldg != "---All---")
                    {
                        mySql += " and a.INWD_Building_var='" + bldg + "'";
                    }
                    if (supplier.Trim() != "" && supplier != "---All---")
                    {
                        mySql += " and b.BTINWD_SupplierName_var='" + supplier + "'";
                    }
                    mySql += "  and a.INWD_CL_Id= " + clId.ToString();
                    if (siteId != 0)
                    {
                        mySql = mySql + " and a.INWD_SITE_Id =" + siteId.ToString("0");
                    }
                    if (billId != "")
                    {
                        mySql = mySql + " and (a.INWD_BILL_Id = '" + billId + "'";
                        mySql = mySql + " Or (Select count(BILLD_BILL_Id) from tbl_BillDetail Where BILLD_ReferenceNo_int = a.INWD_ReferenceNo_int and BILLD_BILL_Id = '" + billId + "') > 0 ";
                        mySql = mySql + " Or (Select count(MBILLD_BILL_Id) from tbl_BillDetailMonthly Where MBILLD_ReferenceNo_int = a.INWD_ReferenceNo_int and MBILLD_BILL_Id = '" + billId + "') > 0 ";
                        mySql = mySql + ")";
                    }
                    mySql += otherStr.Replace("'AAC'", "'BT-'");
                    //mySql += " order by a.INWD_RecordType_var ,a.INWD_RecordNo_int desc";
                    mySql += " order by a.INWD_ReceivedDate_dt desc";
                    break;
                case "PT":
                    mySql = " SELECT a.INWD_ReceivedDate_dt as dateofreceiving, 'Pavement Block' as record_type,a.INWD_Building_var, b.PTINWD_ReferenceNo_var as refno ,b.PTINWD_ApprovedDate_dt as status" +
                        " ,c.CONT_Name_var, a.INWD_ContactNo_var, '' as MFINWD_FinalIssueDt " +
                        " FROM tbl_Inward AS a, tbl_Pavement_Inward AS b, tbl_Contact as c  " +
                            " WHERE a.INWD_RecordType_var = 'PT' AND a.INWD_RecordType_var = b.PTINWD_RecordType_var " +
                            " AND a.INWD_RecordNo_int = b.PTINWD_RecordNo_int" +
                            " AND c.CONT_Id = a.INWD_CONT_Id";

                    if (bldg.Trim() != "" && bldg != "---All---")
                    {
                        mySql += " and a.INWD_Building_var='" + bldg + "'";
                    }
                    if (supplier.Trim() != "" && supplier != "---All---")
                    {
                        mySql += " and b.PTINWD_SupplierName_var='" + supplier + "'";
                    }
                    mySql += "  and a.INWD_CL_Id= " + clId.ToString();
                    if (siteId != 0)
                    {
                        mySql += " and a.INWD_SITE_Id =" + siteId.ToString("0");
                    }
                    if (billId != "")
                    {
                        mySql += " and (a.INWD_BILL_Id = '" + billId + "'";
                        mySql += " Or (Select count(BILLD_BILL_Id) from tbl_BillDetail Where BILLD_ReferenceNo_int = a.INWD_ReferenceNo_int and BILLD_BILL_Id = '" + billId + "') > 0 ";
                        mySql += " Or (Select count(MBILLD_BILL_Id) from tbl_BillDetailMonthly Where MBILLD_ReferenceNo_int = a.INWD_ReferenceNo_int and MBILLD_BILL_Id = '" + billId + "') > 0 ";
                        mySql += ")";
                    }
                    mySql += otherStr.Replace("'AAC'", "'PT'");
                    //mySql += " order by a.INWD_RecordType_var ,a.INWD_RecordNo_int desc";
                    mySql += " order by a.INWD_ReceivedDate_dt desc";
                    break;
                case "SOLID":
                    if (testType == "---All---" || testType == "Concrete Blocks")
                    {
                        mySql = " SELECT a.INWD_ReceivedDate_dt as dateofreceiving, 'Masonary Block' as record_type,a.INWD_Building_var, b.SOLIDINWD_ReferenceNo_var as refno ,b.SOLIDINWD_ApprovedDate_dt  as status" +
                        " ,c.CONT_Name_var, a.INWD_ContactNo_var, '' as MFINWD_FinalIssueDt " +
                        " FROM tbl_Inward AS a, tbl_Solid_Inward AS b, tbl_Contact as c  " +
                            " WHERE a.INWD_RecordType_var = 'SOLID' AND a.INWD_RecordType_var = b.SOLIDINWD_RecordType_var " +
                            " AND a.INWD_RecordNo_int = b.SOLIDINWD_RecordNo_int" +
                            " AND c.CONT_Id = a.INWD_CONT_Id";
                        if (bldg.Trim() != "" && bldg != "---All---")
                        {
                            mySql += " and a.INWD_Building_var='" + bldg + "'";
                        }
                        if (supplier.Trim() != "" && supplier != "---All---")
                        {
                            mySql += " and b.SOLIDINWD_SupplierName_var='" + supplier + "'";
                        }
                        mySql += "  and a.INWD_CL_Id= " + clId.ToString();
                        if (siteId != 0)
                        {
                            mySql = mySql + " and a.INWD_SITE_Id =" + siteId.ToString("0");
                        }
                        if (billId != "")
                        {
                            mySql = mySql + " and (a.INWD_BILL_Id = '" + billId + "'";
                            mySql = mySql + " Or (Select count(BILLD_BILL_Id) from tbl_BillDetail Where BILLD_ReferenceNo_int = a.INWD_ReferenceNo_int and BILLD_BILL_Id = '" + billId + "') > 0 ";
                            mySql = mySql + " Or (Select count(MBILLD_BILL_Id) from tbl_BillDetailMonthly Where MBILLD_ReferenceNo_int = a.INWD_ReferenceNo_int and MBILLD_BILL_Id = '" + billId + "') > 0 ";
                            mySql = mySql + ")";
                        }
                        mySql += otherStr.Replace("'AAC'", "'SOLID'");
                    }
                    if (testType == "---All---")
                    {
                        mySql += " Union ";
                    }
                    if (testType == "---All---" || testType == "AAC Blocks")
                    {
                        mySql += " SELECT a.INWD_ReceivedDate_dt as dateofreceiving, 'AAC Block' as record_type,a.INWD_Building_var, b.AACINWD_ReferenceNo_var as refno ,b.AACINWD_ApprovedDate_dt as status" +
                        " ,c.CONT_Name_var, a.INWD_ContactNo_var, '' as MFINWD_FinalIssueDt " +
                        " FROM tbl_Inward AS a, tbl_AAC_Inward AS b, tbl_Contact as c  " +
                            " WHERE a.INWD_RecordType_var = 'AAC' AND a.INWD_RecordType_var = b.AACINWD_RecordType_var " +
                            " AND a.INWD_RecordNo_int = b.AACINWD_RecordNo_int" +
                            " AND c.CONT_Id = a.INWD_CONT_Id";
                        if (bldg.Trim() != "" && bldg != "---All---")
                        {
                            mySql += " and a.INWD_Building_var='" + bldg + "'";
                        }
                        if (supplier.Trim() != "" && supplier != "---All---")
                        {
                            mySql += " and b.AACINWD_SupplierName_var='" + supplier + "'";
                        }
                        mySql += "  and a.INWD_CL_Id= " + clId.ToString();
                        if (siteId != 0)
                        {
                            mySql = mySql + " and a.INWD_SITE_Id =" + siteId.ToString("0");
                        }
                        if (billId != "")
                        {
                            mySql = mySql + " and (a.INWD_BILL_Id = '" + billId + "'";
                            mySql = mySql + " Or (Select count(BILLD_BILL_Id) from tbl_BillDetail Where BILLD_ReferenceNo_int = a.INWD_ReferenceNo_int and BILLD_BILL_Id = '" + billId + "') > 0 ";
                            mySql = mySql + " Or (Select count(MBILLD_BILL_Id) from tbl_BillDetailMonthly Where MBILLD_ReferenceNo_int = a.INWD_ReferenceNo_int and MBILLD_BILL_Id = '" + billId + "') > 0 ";
                            mySql = mySql + ")";
                        }
                        mySql += otherStr.Replace("'AAC'", "'AAC'");
                    }
                    if (testType == "---All---")
                    {
                        mySql += " Union ";
                    }
                    if (testType == "---All---" || testType == "Bricks")
                    {
                        mySql += " SELECT a.INWD_ReceivedDate_dt as dateofreceiving, 'Brick' as record_type,a.INWD_Building_var, b.BTINWD_ReferenceNo_var as refno ,b.BTINWD_ApprovedDate_dt as status" +
                        " ,c.CONT_Name_var, a.INWD_ContactNo_var, '' as MFINWD_FinalIssueDt " +
                        " FROM tbl_Inward AS a, tbl_Brick_Inward AS b, tbl_Contact as c  " +
                            " WHERE a.INWD_RecordType_var = 'BT-' AND a.INWD_RecordType_var = b.BTINWD_RecordType_var " +
                            " AND a.INWD_RecordNo_int = b.BTINWD_RecordNo_int" +
                            " AND c.CONT_Id = a.INWD_CONT_Id";
                        if (bldg.Trim() != "" && bldg != "---All---")
                        {
                            mySql += " and a.INWD_Building_var='" + bldg + "'";
                        }
                        if (supplier.Trim() != "" && supplier != "---All---")
                        {
                            mySql += " and b.BTINWD_SupplierName_var='" + supplier + "'";
                        }
                        mySql += "  and a.INWD_CL_Id= " + clId.ToString();
                        if (siteId != 0)
                        {
                            mySql = mySql + " and a.INWD_SITE_Id =" + siteId.ToString("0");
                        }
                        if (billId != "")
                        {
                            mySql = mySql + " and (a.INWD_BILL_Id = '" + billId + "'";
                            mySql = mySql + " Or (Select count(BILLD_BILL_Id) from tbl_BillDetail Where BILLD_ReferenceNo_int = a.INWD_ReferenceNo_int and BILLD_BILL_Id = '" + billId + "') > 0 ";
                            mySql = mySql + " Or (Select count(MBILLD_BILL_Id) from tbl_BillDetailMonthly Where MBILLD_ReferenceNo_int = a.INWD_ReferenceNo_int and MBILLD_BILL_Id = '" + billId + "') > 0 ";
                            mySql = mySql + ")";
                        }
                        mySql += otherStr.Replace("'AAC'", "'BT-'");
                    }
                    //mySql += " order by a.INWD_RecordType_var ,a.INWD_RecordNo_int desc";
                    mySql += " order by a.INWD_ReceivedDate_dt desc";
                    break;
                case "PILE":
                    mySql = " SELECT a.INWD_ReceivedDate_dt as dateofreceiving, 'Pile' as record_type,a.INWD_Building_var, b.PILEINWD_ReferenceNo_var as refno ,b.PILEINWD_ApprovedDate_dt as status" +
                        " ,c.CONT_Name_var, a.INWD_ContactNo_var, '' as MFINWD_FinalIssueDt " +
                        " FROM tbl_Inward AS a, tbl_Pile_Inward AS b, tbl_Contact as c  " +
                            " WHERE a.INWD_RecordType_var = 'PILE' AND a.INWD_RecordType_var = b.PILEINWD_RecordType_var " +
                            " AND a.INWD_RecordNo_int = b.PILEINWD_RecordNo_int" +
                            " AND c.CONT_Id = a.INWD_CONT_Id";
                    if (bldg.Trim() != "" && bldg != "---All---")
                    {
                        mySql += " and a.INWD_Building_var='" + bldg + "'";
                    }
                    if (supplier.Trim() != "" && supplier != "---All---")
                    {
                        mySql += " and b.PILEINWD_SupplierName_var='" + supplier + "'";
                    }
                    mySql += "  and a.INWD_CL_Id= " + clId.ToString();
                    if (siteId != 0)
                    {
                        mySql = mySql + " and a.INWD_SITE_Id =" + siteId.ToString("0");
                    }
                    if (billId != "")
                    {
                        mySql = mySql + " and (a.INWD_BILL_Id = '" + billId + "'";
                        mySql = mySql + " Or (Select count(BILLD_BILL_Id) from tbl_BillDetail Where BILLD_ReferenceNo_int = a.INWD_ReferenceNo_int and BILLD_BILL_Id = '" + billId + "') > 0 ";
                        mySql = mySql + " Or (Select count(MBILLD_BILL_Id) from tbl_BillDetailMonthly Where MBILLD_ReferenceNo_int = a.INWD_ReferenceNo_int and MBILLD_BILL_Id = '" + billId + "') > 0 ";
                        mySql = mySql + ")";
                    }
                    mySql += otherStr.Replace("'AAC'", "'PILE'");
                    //mySql += " order by a.INWD_RecordType_var ,a.INWD_RecordNo_int desc";
                    mySql += " order by a.INWD_ReceivedDate_dt desc";
                    break;
                case "TILE":
                    if (testType == "---All---" || testType == "Physical")
                    {
                        mySql = " SELECT a.INWD_ReceivedDate_dt as dateofreceiving, 'Tile' as record_type,a.INWD_Building_var, b.TILEINWD_ReferenceNo_var as refno ,b.TILEINWD_ApprovedDate_dt as status" +
                        " ,c.CONT_Name_var, a.INWD_ContactNo_var, '' as MFINWD_FinalIssueDt " +
                        " FROM tbl_Inward AS a, tbl_Tile_Inward AS b, tbl_Contact as c  " +
                            " WHERE a.INWD_RecordType_var = 'TILE' AND a.INWD_RecordType_var = b.TILEINWD_RecordType_var " +
                            " AND a.INWD_RecordNo_int = b.TILEINWD_RecordNo_int" +
                            " AND c.CONT_Id = a.INWD_CONT_Id";
                        if (bldg.Trim() != "" && bldg != "---All---")
                        {
                            mySql += " and a.INWD_Building_var='" + bldg + "'";
                        }
                        if (supplier.Trim() != "" && supplier != "---All---")
                        {
                            mySql += " and b.TILEINWD_SupplierName_var='" + supplier + "'";
                        }
                        mySql += "  and a.INWD_CL_Id= " + clId.ToString();
                        if (siteId != 0)
                        {
                            mySql = mySql + " and a.INWD_SITE_Id =" + siteId.ToString("0");
                        }
                        if (billId != "")
                        {
                            mySql = mySql + " and (a.INWD_BILL_Id = '" + billId + "'";
                            mySql = mySql + " Or (Select count(BILLD_BILL_Id) from tbl_BillDetail Where BILLD_ReferenceNo_int = a.INWD_ReferenceNo_int and BILLD_BILL_Id = '" + billId + "') > 0 ";
                            mySql = mySql + " Or (Select count(MBILLD_BILL_Id) from tbl_BillDetailMonthly Where MBILLD_ReferenceNo_int = a.INWD_ReferenceNo_int and MBILLD_BILL_Id = '" + billId + "') > 0 ";
                            mySql = mySql + ")";
                        }
                        mySql += otherStr.Replace("'AAC'", "'TILE'");
                    }
                    if (testType == "---All---")
                    {
                        mySql += " Union ";
                    }
                    if (testType == "---All---" || testType == "Chemical")
                    {
                        mySql += otherStr.Replace("'AAC'", "'TILECH'").Replace("Union", "");
                    }
                    //mySql += " order by a.INWD_RecordType_var ,a.INWD_RecordNo_int desc";
                    mySql += " order by a.INWD_ReceivedDate_dt desc";
                    break;
                case "STC":
                    mySql = " SELECT a.INWD_ReceivedDate_dt as dateofreceiving, 'Steel Chemical' as record_type,a.INWD_Building_var, b.STCINWD_ReferenceNo_var as refno ,b.STCINWD_ApprovedDate_dt as status" +
                        " ,c.CONT_Name_var, a.INWD_ContactNo_var, '' as MFINWD_FinalIssueDt " +
                        " FROM tbl_Inward AS a, tbl_SteelChemical_Inward AS b, tbl_Contact as c  " +
                            " WHERE a.INWD_RecordType_var = 'STC' AND a.INWD_RecordType_var = b.STCINWD_RecordType_var " +
                            " AND a.INWD_RecordNo_int = b.STCINWD_RecordNo_int" +
                            " AND c.CONT_Id = a.INWD_CONT_Id";
                    if (bldg.Trim() != "" && bldg != "---All---")
                    {
                        mySql += " and a.INWD_Building_var='" + bldg + "'";
                    }
                    if (supplier.Trim() != "" && supplier != "---All---")
                    {
                        mySql += " and b.STCINWD_SupplierName_var='" + supplier + "'";
                    }
                    mySql += "  and a.INWD_CL_Id= " + clId.ToString();
                    if (siteId != 0)
                    {
                        mySql = mySql + " and a.INWD_SITE_Id =" + siteId.ToString("0");
                    }
                    if (billId != "")
                    {
                        mySql = mySql + " and (a.INWD_BILL_Id = '" + billId + "'";
                        mySql = mySql + " Or (Select count(BILLD_BILL_Id) from tbl_BillDetail Where BILLD_ReferenceNo_int = a.INWD_ReferenceNo_int and BILLD_BILL_Id = '" + billId + "') > 0 ";
                        mySql = mySql + " Or (Select count(MBILLD_BILL_Id) from tbl_BillDetailMonthly Where MBILLD_ReferenceNo_int = a.INWD_ReferenceNo_int and MBILLD_BILL_Id = '" + billId + "') > 0 ";
                        mySql = mySql + ")";
                    }
                    mySql += otherStr.Replace("'AAC'", "'STC'");
                    //mySql += " order by a.INWD_RecordType_var ,a.INWD_RecordNo_int desc";
                    mySql += " order by a.INWD_ReceivedDate_dt desc";
                    break;
                case "CCH":
                    mySql = " SELECT a.INWD_ReceivedDate_dt as dateofreceiving, 'Cement Chemical' as record_type,a.INWD_Building_var, b.CCHINWD_ReferenceNo_var as refno ,b.CCHINWD_ApprovedDate_dt as status" +
                        " ,c.CONT_Name_var, a.INWD_ContactNo_var, '' as MFINWD_FinalIssueDt " +
                        " FROM tbl_Inward AS a, tbl_CementChemical_Inward AS b, tbl_Contact as c  " +
                            " WHERE a.INWD_RecordType_var = 'CCH' AND a.INWD_RecordType_var = b.CCHINWD_RecordType_var " +
                            " AND a.INWD_RecordNo_int = b.CCHINWD_RecordNo_int" +
                            " AND c.CONT_Id = a.INWD_CONT_Id";
                    if (bldg.Trim() != "" && bldg != "---All---")
                    {
                        mySql += " and a.INWD_Building_var='" + bldg + "'";
                    }
                    if (supplier.Trim() != "" && supplier != "---All---")
                    {
                        mySql += " and b.CCHINWD_SupplierName_var='" + supplier + "'";
                    }
                    mySql += "  and a.INWD_CL_Id= " + clId.ToString();
                    if (siteId != 0)
                    {
                        mySql = mySql + " and a.INWD_SITE_Id =" + siteId.ToString("0");
                    }
                    if (billId != "")
                    {
                        mySql = mySql + " and (a.INWD_BILL_Id = '" + billId + "'";
                        mySql = mySql + " Or (Select count(BILLD_BILL_Id) from tbl_BillDetail Where BILLD_ReferenceNo_int = a.INWD_ReferenceNo_int and BILLD_BILL_Id = '" + billId + "') > 0 ";
                        mySql = mySql + " Or (Select count(MBILLD_BILL_Id) from tbl_BillDetailMonthly Where MBILLD_ReferenceNo_int = a.INWD_ReferenceNo_int and MBILLD_BILL_Id = '" + billId + "') > 0 ";
                        mySql = mySql + ")";
                    }
                    mySql += otherStr.Replace("'AAC'", "'CCH'");
                    //mySql += " order by a.INWD_RecordType_var ,a.INWD_RecordNo_int desc";
                    mySql += " order by a.INWD_ReceivedDate_dt desc";
                    break;
                case "WT":
                    mySql = " SELECT a.INWD_ReceivedDate_dt as dateofreceiving,'Water' as record_type,a.INWD_Building_var, b.WTINWD_ReferenceNo_var as refno ,b.WTINWD_ApprovedDate_dt as status" +
                        " ,c.CONT_Name_var, a.INWD_ContactNo_var, '' as MFINWD_FinalIssueDt " +
                        " FROM tbl_Inward AS a, tbl_Water_Inward AS b, tbl_Contact as c  " +
                            " WHERE a.INWD_RecordType_var = 'WT' AND a.INWD_RecordType_var = b.WTINWD_RecordType_var " +
                            " AND a.INWD_RecordNo_int = b.WTINWD_RecordNo_int" +
                            " AND c.CONT_Id = a.INWD_CONT_Id";
                    if (bldg.Trim() != "" && bldg != "---All---")
                    {
                        mySql += " and a.INWD_Building_var='" + bldg + "'";
                    }
                    if (supplier.Trim() != "" && supplier != "---All---")
                    {
                        mySql += " and b.WTINWD_SupplierName_var='" + supplier + "'";
                    }
                    mySql += "  and a.INWD_CL_Id= " + clId.ToString();
                    if (siteId != 0)
                    {
                        mySql = mySql + " and a.INWD_SITE_Id =" + siteId.ToString("0");
                    }
                    if (billId != "")
                    {
                        mySql = mySql + " and (a.INWD_BILL_Id = '" + billId + "'";
                        mySql = mySql + " Or (Select count(BILLD_BILL_Id) from tbl_BillDetail Where BILLD_ReferenceNo_int = a.INWD_ReferenceNo_int and BILLD_BILL_Id = '" + billId + "') > 0 ";
                        mySql = mySql + " Or (Select count(MBILLD_BILL_Id) from tbl_BillDetailMonthly Where MBILLD_ReferenceNo_int = a.INWD_ReferenceNo_int and MBILLD_BILL_Id = '" + billId + "') > 0 ";
                        mySql = mySql + ")";
                    }
                    mySql += otherStr.Replace("'AAC'", "'WT'");
                    //mySql += " order by a.INWD_RecordType_var ,a.INWD_RecordNo_int desc";
                    mySql += " order by a.INWD_ReceivedDate_dt desc";
                    break;
                case "SO":
                    mySql = "SELECT a.INWD_ReceivedDate_dt as dateofreceiving,'Soil' as record_type," +
                    " a.INWD_Building_var, b.SOINWD_ReferenceNo_var as refno , b.SOINWD_ApprovedDate_dt as status " +
                    " ,c.CONT_Name_var, a.INWD_ContactNo_var, '' as MFINWD_FinalIssueDt " +
                    " FROM tbl_Inward AS a, tbl_Soil_Inward AS b, tbl_Contact as c " +
                    " WHERE a.INWD_RecordType_var = 'SO'  AND a.INWD_RecordType_var = b.SOINWD_RecordType_var AND a.INWD_RecordNo_int = b.SOINWD_RecordNo_int " +
                    " AND c.CONT_Id = a.INWD_CONT_Id";

                    if (bldg.Trim() != "" && bldg != "---All---")
                    {
                        mySql += " and a.INWD_Building_var='" + bldg + "'";
                    }
                    if (supplier.Trim() != "" && supplier != "---All---")
                    {
                        mySql += " and b.SOINWD_SupplierName_var='" + supplier + "'";
                    }
                    mySql += "  and a.INWD_CL_Id= " + clId.ToString();
                    if (siteId != 0)
                    {
                        mySql = mySql + " and a.INWD_SITE_Id =" + siteId.ToString("0");
                    }
                    if (billId != "")
                    {
                        mySql = mySql + " and (a.INWD_BILL_Id = '" + billId + "'";
                        mySql = mySql + " Or (Select count(BILLD_BILL_Id) from tbl_BillDetail Where BILLD_ReferenceNo_int = a.INWD_ReferenceNo_int and BILLD_BILL_Id = '" + billId + "') > 0 ";
                        mySql = mySql + " Or (Select count(MBILLD_BILL_Id) from tbl_BillDetailMonthly Where MBILLD_ReferenceNo_int = a.INWD_ReferenceNo_int and MBILLD_BILL_Id = '" + billId + "') > 0 ";
                        mySql = mySql + ")";
                    }
                    mySql += otherStr.Replace("'AAC'", "'SO'");
                    //mySql += " order by a.INWD_RecordType_var ,a.INWD_RecordNo_int desc";
                    mySql += " order by a.INWD_ReceivedDate_dt desc";
                    break;
                case "OT":
                    mySql = otherStr.Replace("'AAC'", "'OT'").Replace("Union", "");
                    mySql += " order by a.INWD_ReceivedDate_dt desc";
                    break;
                default:
                    mySql = "";
                    break;
            }
            #endregion

            DataTable dtReturn = new DataTable();
            if (mySql != "")
            {
                SqlDataAdapter da = new SqlDataAdapter(mySql, cn);
                DataSet ds = new DataSet();
                ds.Reset();
                da.Fill(ds);
                dtReturn = new DataTable();
                dtReturn = ds.Tables[0];
                //dtReturn.Merge(dt1);
            }
            return dtReturn;
        }
        catch (Exception e)
        {
            throw e;
        }

    }

    public DataTable getReportList2016Metro(double clId, double siteId, 
            string bldg, string rType, string db, string billId, string testType,Int32 mth, Int32 myear)
    {
        string mySql = "";
        try
        {

            #region  other report query
            string otherStr = @" Union SELECT a.INWD_ReceivedDate_dt as dateofreceiving, 'Other' as record_type, a.INWD_Building_var, 
                              b.OTINWD_ReferenceNo_var as refno, b.OTINWD_IssueDate_dt as status, c.CONT_Name_var, a.INWD_ContactNo_var, '' as MFINWD_FinalIssueDt  
                              FROM tbl_Inward AS a, tbl_Other_Inward AS b, tbl_Contact as c   
                              WHERE a.INWD_RecordType_var = 'OT' AND a.INWD_RecordType_var = b.OTINWD_RecordType_var  
                              AND a.INWD_RecordNo_int = b.OTINWD_RecordNo_int AND c.CONT_Id = a.INWD_CONT_Id ";
            otherStr += "and b.OTINWD_Section_var = 'AAC'";
            if (bldg.Trim() != "" && bldg != "---All---")
            {
                otherStr += " and a.INWD_Building_var='" + bldg + "'";
            }
            mySql += " and month(INWD_ReceivedDate_dt )=" + mth.ToString();
            mySql += " and year(INWD_ReceivedDate_dt )=" + myear.ToString();
            otherStr += "  and a.INWD_CL_Id= " + clId.ToString();
            if (siteId != 0)
            {
                otherStr += " and a.INWD_SITE_Id =" + siteId.ToString("0");
            }
            if (billId != "")
            {
                otherStr += " and (a.INWD_BILL_Id = '" + billId + "'";
                otherStr += " Or (Select count(BILLD_BILL_Id) from tbl_BillDetail Where BILLD_ReferenceNo_int = a.INWD_ReferenceNo_int and BILLD_BILL_Id = '" + billId + "') > 0 ";
                otherStr += " Or (Select count(MBILLD_BILL_Id) from tbl_BillDetailMonthly Where MBILLD_ReferenceNo_int = a.INWD_ReferenceNo_int and MBILLD_BILL_Id = '" + billId + "') > 0 ";
                otherStr += ")";
            }
            #endregion
            #region query
            switch (rType)
            {
                case "CT":
                    mySql = " SELECT a.INWD_ReceivedDate_dt as dateofreceiving, 'Cube' as record_type,a.INWD_Building_var, b.CTINWD_ReferenceNo_var as refno ,b.CTINWD_IssueDate_dt as status" +
                            " ,c.CONT_Name_var, a.INWD_ContactNo_var, '' as MFINWD_FinalIssueDt " +
                            " FROM tbl_Inward AS a, tbl_Cube_Inward AS b, tbl_Contact as c " +
                            " WHERE a.INWD_RecordType_var = 'CT' AND a.INWD_RecordType_var = b.CTINWD_RecordType_var " +
                            " AND a.INWD_RecordNo_int = b.CTINWD_RecordNo_int" +
                            " AND c.CONT_Id = a.INWD_CONT_Id";
                    if (bldg.Trim() != "" && bldg != "---All---")
                    {
                        mySql += " and a.INWD_Building_var='" + bldg + "'";
                    }
                    mySql += "  and a.INWD_CL_Id= " + clId.ToString();
                    if (siteId != 0)
                    {
                        mySql = mySql + " and a.INWD_SITE_Id =" + siteId.ToString("0");
                    }

                    mySql += " and month(INWD_ReceivedDate_dt )=" + mth.ToString();
                    mySql += " and year(INWD_ReceivedDate_dt )=" + myear.ToString();
                    
                    if (billId != "")
                    {
                        mySql = mySql + " and (a.INWD_BILL_Id = '" + billId + "'";
                        mySql = mySql + " Or (Select count(BILLD_BILL_Id) from tbl_BillDetail Where BILLD_ReferenceNo_int = a.INWD_ReferenceNo_int and BILLD_BILL_Id = '" + billId + "') > 0 ";
                        mySql = mySql + " Or (Select count(MBILLD_BILL_Id) from tbl_BillDetailMonthly Where MBILLD_ReferenceNo_int = a.INWD_ReferenceNo_int and MBILLD_BILL_Id = '" + billId + "') > 0 ";
                        mySql = mySql + ")";
                    }
                    mySql += otherStr.Replace("'AAC'", "'CT'");
                    //mySql += " order by a.INWD_RecordType_var ,a.INWD_RecordNo_int desc";
                    mySql += " order by a.INWD_ReceivedDate_dt desc";
                    break;
                case "ST":
                    if (testType == "---All---" || testType == "Physical")
                    {
                        mySql = " SELECT a.INWD_ReceivedDate_dt as dateofreceiving, 'Steel' as record_type,a.INWD_Building_var, b.STINWD_ReferenceNo_var as refno ,b.STINWD_IssueDate_dt as status" +
                                " ,c.CONT_Name_var, a.INWD_ContactNo_var, '' as MFINWD_FinalIssueDt " +
                                " FROM tbl_Inward AS a, tbl_Steel_Inward AS b, tbl_Contact as c " +
                                " WHERE a.INWD_RecordType_var = 'ST' AND a.INWD_RecordType_var = b.STINWD_RecordType_var " +
                                " AND a.INWD_RecordNo_int = b.STINWD_RecordNo_int" +
                                " AND c.CONT_Id = a.INWD_CONT_Id";
                        if (bldg.Trim() != "" && bldg != "---All---")
                        {
                            mySql += " and a.INWD_Building_var='" + bldg + "'";
                        }                        
                        mySql += "  and a.INWD_CL_Id= " + clId.ToString();
                        if (siteId != 0)
                        {
                            mySql = mySql + " and a.INWD_SITE_Id =" + siteId.ToString("0");
                        }
                        mySql += " and month(INWD_ReceivedDate_dt )=" + mth.ToString();
                        mySql += " and year(INWD_ReceivedDate_dt )=" + myear.ToString();
                        if (billId != "")
                        {
                            mySql = mySql + " and (a.INWD_BILL_Id = '" + billId + "'";
                            mySql = mySql + " Or (Select count(BILLD_BILL_Id) from tbl_BillDetail Where BILLD_ReferenceNo_int = a.INWD_ReferenceNo_int and BILLD_BILL_Id = '" + billId + "') > 0 ";
                            mySql = mySql + " Or (Select count(MBILLD_BILL_Id) from tbl_BillDetailMonthly Where MBILLD_ReferenceNo_int = a.INWD_ReferenceNo_int and MBILLD_BILL_Id = '" + billId + "') > 0 ";
                            mySql = mySql + ")";
                        }
                        mySql += otherStr.Replace("'AAC'", "'ST'");
                    }
                    if (testType == "---All---")
                    {
                        mySql += " Union ";
                    }
                    if (testType == "---All---" || testType == "Chemical")
                    {
                        mySql += " SELECT a.INWD_ReceivedDate_dt as dateofreceiving, 'Steel Chemical' as record_type,a.INWD_Building_var, b.STCINWD_ReferenceNo_var as refno ,b.STCINWD_IssueDate_dt as status" +
                        " ,c.CONT_Name_var, a.INWD_ContactNo_var, '' as MFINWD_FinalIssueDt " +
                        " FROM tbl_Inward AS a, tbl_SteelChemical_Inward AS b, tbl_Contact as c  " +
                            " WHERE a.INWD_RecordType_var = 'STC' AND a.INWD_RecordType_var = b.STCINWD_RecordType_var " +
                            " AND a.INWD_RecordNo_int = b.STCINWD_RecordNo_int" +
                            " AND c.CONT_Id = a.INWD_CONT_Id";
                        if (bldg.Trim() != "" && bldg != "---All---")
                        {
                            mySql += " and a.INWD_Building_var='" + bldg + "'";
                        }
                        
                        mySql += "  and a.INWD_CL_Id= " + clId.ToString();
                        if (siteId != 0)
                        {
                            mySql = mySql + " and a.INWD_SITE_Id =" + siteId.ToString("0");
                        }
                        mySql += " and month(INWD_ReceivedDate_dt )=" + mth.ToString();
                        mySql += " and year(INWD_ReceivedDate_dt )=" + myear.ToString();
                        if (billId != "")
                        {
                            mySql = mySql + " and (a.INWD_BILL_Id = '" + billId + "'";
                            mySql = mySql + " Or (Select count(BILLD_BILL_Id) from tbl_BillDetail Where BILLD_ReferenceNo_int = a.INWD_ReferenceNo_int and BILLD_BILL_Id = '" + billId + "') > 0 ";
                            mySql = mySql + " Or (Select count(MBILLD_BILL_Id) from tbl_BillDetailMonthly Where MBILLD_ReferenceNo_int = a.INWD_ReferenceNo_int and MBILLD_BILL_Id = '" + billId + "') > 0 ";
                            mySql = mySql + ")";
                        }
                        mySql += otherStr.Replace("'AAC'", "'STC'");
                    }
                    //mySql += " order by a.INWD_RecordType_var ,a.INWD_RecordNo_int desc";
                    mySql += " order by a.INWD_ReceivedDate_dt desc";
                    break;
                case "MF":
                    mySql = " SELECT a.INWD_ReceivedDate_dt as dateofreceiving,'Mix Design' as record_type,a.INWD_Building_var, b.MFINWD_ReferenceNo_var as refno ,b.MFINWD_MDLIssueDt as status" +
                        " ,c.CONT_Name_var, a.INWD_ContactNo_var, CONVERT(varchar,b.MFINWD_FinalIssueDt) as MFINWD_FinalIssueDt " +
                        " FROM tbl_Inward AS a, tbl_MixDesign_Inward AS b , tbl_Contact as c " +
                            " WHERE a.INWD_RecordType_var = 'MF' AND a.INWD_RecordType_var = b.MFINWD_RecordType_var " +
                            " AND a.INWD_RecordNo_int = b.MFINWD_RecordNo_int" +
                            " AND c.CONT_Id = a.INWD_CONT_Id";
                    if (bldg.Trim() != "" && bldg != "---All---")
                    {
                        mySql += " and a.INWD_Building_var='" + bldg + "'";
                    }                  

                    mySql += "  and a.INWD_CL_Id= " + clId.ToString();
                    if (siteId != 0)
                    {
                        mySql = mySql + " and a.INWD_SITE_Id =" + siteId.ToString("0");
                    }
                    mySql += " and month(INWD_ReceivedDate_dt )=" + mth.ToString();
                    mySql += " and year(INWD_ReceivedDate_dt )=" + myear.ToString();
                    if (billId != "")
                    {
                        mySql = mySql + " and (a.INWD_BILL_Id = '" + billId + "'";
                        mySql = mySql + " Or (Select count(BILLD_BILL_Id) from tbl_BillDetail Where BILLD_ReferenceNo_int = a.INWD_ReferenceNo_int and BILLD_BILL_Id = '" + billId + "') > 0 ";
                        mySql = mySql + " Or (Select count(MBILLD_BILL_Id) from tbl_BillDetailMonthly Where MBILLD_ReferenceNo_int = a.INWD_ReferenceNo_int and MBILLD_BILL_Id = '" + billId + "') > 0 ";
                        mySql = mySql + ")";
                    }
                    mySql += otherStr.Replace("'AAC'", "'MF'");
                    //mySql += " order by a.INWD_RecordType_var ,a.INWD_RecordNo_int desc";
                    mySql += " order by a.INWD_ReceivedDate_dt desc";
                    break;
                case "CEMT":
                    if (testType == "---All---" || testType == "Physical")
                    {
                        mySql = " SELECT a.INWD_ReceivedDate_dt as dateofreceiving, 'Cement' as record_type,a.INWD_Building_var, b.CEMTINWD_ReferenceNo_var as refno ,b.CEMTINWD_IssueDate_dt as status" +
                        " ,c.CONT_Name_var, a.INWD_ContactNo_var, '' as MFINWD_FinalIssueDt " +
                        " FROM tbl_Inward AS a, tbl_Cement_Inward AS b, tbl_Contact as c  " +
                            " WHERE a.INWD_RecordType_var = 'CEMT' AND a.INWD_RecordType_var = b.CEMTINWD_RecordType_var " +
                            " AND a.INWD_RecordNo_int = b.CEMTINWD_RecordNo_int" +
                            " AND c.CONT_Id = a.INWD_CONT_Id";
                        if (bldg.Trim() != "" && bldg != "---All---")
                        {
                            mySql += " and a.INWD_Building_var='" + bldg + "'";
                        }
                        mySql += " and month(INWD_ReceivedDate_dt )=" + mth.ToString();
                        mySql += " and year(INWD_ReceivedDate_dt )=" + myear.ToString();
                        mySql += "  and a.INWD_CL_Id= " + clId.ToString();
                        if (siteId != 0)
                        {
                            mySql = mySql + " and a.INWD_SITE_Id =" + siteId.ToString("0");
                        }
                        if (billId != "")
                        {
                            mySql = mySql + " and (a.INWD_BILL_Id = '" + billId + "'";
                            mySql = mySql + " Or (Select count(BILLD_BILL_Id) from tbl_BillDetail Where BILLD_ReferenceNo_int = a.INWD_ReferenceNo_int and BILLD_BILL_Id = '" + billId + "') > 0 ";
                            mySql = mySql + " Or (Select count(MBILLD_BILL_Id) from tbl_BillDetailMonthly Where MBILLD_ReferenceNo_int = a.INWD_ReferenceNo_int and MBILLD_BILL_Id = '" + billId + "') > 0 ";
                            mySql = mySql + ")";
                        }
                        mySql += otherStr.Replace("'AAC'", "'CEMT'");
                    }
                    if (testType == "---All---")
                    {
                        mySql += " Union ";
                    }
                    if (testType == "---All---" || testType == "Chemical")
                    {
                        mySql += " SELECT a.INWD_ReceivedDate_dt as dateofreceiving, 'Cement Chemical' as record_type,a.INWD_Building_var, b.CCHINWD_ReferenceNo_var as refno ,b.CCHINWD_IssueDate_dt as status" +
                        " ,c.CONT_Name_var, a.INWD_ContactNo_var, '' as MFINWD_FinalIssueDt " +
                        " FROM tbl_Inward AS a, tbl_CementChemical_Inward AS b, tbl_Contact as c  " +
                            " WHERE a.INWD_RecordType_var = 'CCH' AND a.INWD_RecordType_var = b.CCHINWD_RecordType_var " +
                            " AND a.INWD_RecordNo_int = b.CCHINWD_RecordNo_int" +
                            " AND c.CONT_Id = a.INWD_CONT_Id";
                        if (bldg.Trim() != "" && bldg != "---All---")
                        {
                            mySql += " and a.INWD_Building_var='" + bldg + "'";
                        }
                        mySql += " and month(INWD_ReceivedDate_dt )=" + mth.ToString();
                        mySql += " and year(INWD_ReceivedDate_dt )=" + myear.ToString();
                        mySql += "  and a.INWD_CL_Id= " + clId.ToString();
                        if (siteId != 0)
                        {
                            mySql = mySql + " and a.INWD_SITE_Id =" + siteId.ToString("0");
                        }
                        if (billId != "")
                        {
                            mySql = mySql + " and (a.INWD_BILL_Id = '" + billId + "'";
                            mySql = mySql + " Or (Select count(BILLD_BILL_Id) from tbl_BillDetail Where BILLD_ReferenceNo_int = a.INWD_ReferenceNo_int and BILLD_BILL_Id = '" + billId + "') > 0 ";
                            mySql = mySql + " Or (Select count(MBILLD_BILL_Id) from tbl_BillDetailMonthly Where MBILLD_ReferenceNo_int = a.INWD_ReferenceNo_int and MBILLD_BILL_Id = '" + billId + "') > 0 ";
                            mySql = mySql + ")";
                        }
                        mySql += otherStr.Replace("'AAC'", "'CCH'");
                    }
                    //mySql += " order by a.INWD_RecordType_var ,a.INWD_RecordNo_int desc";
                    mySql += " order by a.INWD_ReceivedDate_dt desc";
                    break;
                case "AGGT":
                    mySql = " SELECT a.INWD_ReceivedDate_dt as dateofreceiving, 'Aggregate' as record_type,a.INWD_Building_var, b.AGGTINWD_ReferenceNo_var as refno ,b.AGGTINWD_IssueDate_dt as status" +
                        " ,c.CONT_Name_var, a.INWD_ContactNo_var, '' as MFINWD_FinalIssueDt " +
                        " FROM tbl_Inward AS a, tbl_Aggregate_Inward AS b, tbl_Contact as c  " +
                            " WHERE a.INWD_RecordType_var = 'AGGT' AND a.INWD_RecordType_var = b.AGGTINWD_RecordType_var " +
                            " AND a.INWD_RecordNo_int = b.AGGTINWD_RecordNo_int" +
                            " AND c.CONT_Id = a.INWD_CONT_Id";
                    if (bldg.Trim() != "" && bldg != "---All---")
                    {
                        mySql += " and a.INWD_Building_var='" + bldg + "'";
                    }
                    mySql += " and month(INWD_ReceivedDate_dt )=" + mth.ToString();
                    mySql += " and year(INWD_ReceivedDate_dt )=" + myear.ToString();
                    mySql += "  and a.INWD_CL_Id= " + clId.ToString();
                    if (siteId != 0)
                    {
                        mySql = mySql + " and a.INWD_SITE_Id =" + siteId.ToString("0");
                    }
                    if (billId != "")
                    {
                        mySql = mySql + " and (a.INWD_BILL_Id = '" + billId + "'";
                        mySql = mySql + " Or (Select count(BILLD_BILL_Id) from tbl_BillDetail Where BILLD_ReferenceNo_int = a.INWD_ReferenceNo_int and BILLD_BILL_Id = '" + billId + "') > 0 ";
                        mySql = mySql + " Or (Select count(MBILLD_BILL_Id) from tbl_BillDetailMonthly Where MBILLD_ReferenceNo_int = a.INWD_ReferenceNo_int and MBILLD_BILL_Id = '" + billId + "') > 0 ";
                        mySql = mySql + ")";
                    }
                    mySql += otherStr.Replace("'AAC'", "'AGGT'");
                    //mySql += " order by a.INWD_RecordType_var ,a.INWD_RecordNo_int desc";
                    mySql += " order by a.INWD_ReceivedDate_dt desc";
                    break;
                case "AAC":
                    mySql = " SELECT a.INWD_ReceivedDate_dt as dateofreceiving, 'AAC Block' as record_type,a.INWD_Building_var, b.AACINWD_ReferenceNo_var as refno ,b.AACINWD_IssueDate_dt as status" +
                        " ,c.CONT_Name_var, a.INWD_ContactNo_var, '' as MFINWD_FinalIssueDt " +
                        " FROM tbl_Inward AS a, tbl_AAC_Inward AS b, tbl_Contact as c  " +
                            " WHERE a.INWD_RecordType_var = 'AAC' AND a.INWD_RecordType_var = b.AACINWD_RecordType_var " +
                            " AND a.INWD_RecordNo_int = b.AACINWD_RecordNo_int" +
                            " AND c.CONT_Id = a.INWD_CONT_Id";
                    if (bldg.Trim() != "" && bldg != "---All---")
                    {
                        mySql += " and a.INWD_Building_var='" + bldg + "'";
                    }
                    mySql += " and month(INWD_ReceivedDate_dt )=" + mth.ToString();
                    mySql += " and year(INWD_ReceivedDate_dt )=" + myear.ToString();
                    mySql += "  and a.INWD_CL_Id= " + clId.ToString();
                    if (siteId != 0)
                    {
                        mySql = mySql + " and a.INWD_SITE_Id =" + siteId.ToString("0");
                    }
                    if (billId != "")
                    {
                        mySql = mySql + " and (a.INWD_BILL_Id = '" + billId + "'";
                        mySql = mySql + " Or (Select count(BILLD_BILL_Id) from tbl_BillDetail Where BILLD_ReferenceNo_int = a.INWD_ReferenceNo_int and BILLD_BILL_Id = '" + billId + "') > 0 ";
                        mySql = mySql + " Or (Select count(MBILLD_BILL_Id) from tbl_BillDetailMonthly Where MBILLD_ReferenceNo_int = a.INWD_ReferenceNo_int and MBILLD_BILL_Id = '" + billId + "') > 0 ";
                        mySql = mySql + ")";
                    }
                    mySql += otherStr.Replace("'AAC'", "'AAC'");
                    //mySql += " order by a.INWD_RecordType_var ,a.INWD_RecordNo_int desc";
                    mySql += " order by a.INWD_ReceivedDate_dt desc";
                    break;
                case "NDT":
                    mySql = " SELECT a.INWD_ReceivedDate_dt as dateofreceiving, 'Non Destructive' as record_type,a.INWD_Building_var, b.NDTINWD_ReferenceNo_var as refno ,b.NDTINWD_IssueDate_dt as status" +
                        " ,c.CONT_Name_var, a.INWD_ContactNo_var, '' as MFINWD_FinalIssueDt " +
                        " FROM tbl_Inward AS a, tbl_NDT_Inward AS b, tbl_Contact as c  " +
                            " WHERE a.INWD_RecordType_var = 'NDT' AND a.INWD_RecordType_var = b.NDTINWD_RecordType_var " +
                            " AND a.INWD_RecordNo_int = b.NDTINWD_RecordNo_int" +
                            " AND c.CONT_Id = a.INWD_CONT_Id";
                    if (bldg.Trim() != "" && bldg != "---All---")
                    {
                        mySql += " and a.INWD_Building_var='" + bldg + "'";
                    }
                    mySql += " and month(INWD_ReceivedDate_dt )=" + mth.ToString();
                    mySql += " and year(INWD_ReceivedDate_dt )=" + myear.ToString();
                    mySql += "  and a.INWD_CL_Id= " + clId.ToString();
                    if (siteId != 0)
                    {
                        mySql = mySql + " and a.INWD_SITE_Id =" + siteId.ToString("0");
                    }
                    if (billId != "")
                    {
                        mySql = mySql + " and (a.INWD_BILL_Id = '" + billId + "'";
                        mySql = mySql + " Or (Select count(BILLD_BILL_Id) from tbl_BillDetail Where BILLD_ReferenceNo_int = a.INWD_ReferenceNo_int and BILLD_BILL_Id = '" + billId + "') > 0 ";
                        mySql = mySql + " Or (Select count(MBILLD_BILL_Id) from tbl_BillDetailMonthly Where MBILLD_ReferenceNo_int = a.INWD_ReferenceNo_int and MBILLD_BILL_Id = '" + billId + "') > 0 ";
                        mySql = mySql + ")";
                    }
                    mySql += otherStr.Replace("'AAC'", "'NDT'");
                    //mySql += " order by a.INWD_RecordType_var ,a.INWD_RecordNo_int desc";
                    mySql += " order by a.INWD_ReceivedDate_dt desc";
                    break;
                case "CR":
                    mySql = " SELECT a.INWD_ReceivedDate_dt as dateofreceiving,'Core' as record_type,a.INWD_Building_var, b.CRINWD_ReferenceNo_var as refno ,b.CRINWD_IssueDate_dt as status" +
                        " ,c.CONT_Name_var, a.INWD_ContactNo_var, '' as MFINWD_FinalIssueDt " +
                        " FROM tbl_Inward AS a, tbl_Core_Inward AS b, tbl_Contact as c  " +
                            " WHERE a.INWD_RecordType_var = 'CR' AND a.INWD_RecordType_var = b.CRINWD_RecordType_var " +
                            " AND a.INWD_RecordNo_int = b.CRINWD_RecordNo_int" +
                            " AND c.CONT_Id = a.INWD_CONT_Id";
                    if (bldg.Trim() != "" && bldg != "---All---")
                    {
                        mySql += " and a.INWD_Building_var='" + bldg + "'";
                    }
                    mySql += " and month(INWD_ReceivedDate_dt )=" + mth.ToString();
                    mySql += " and year(INWD_ReceivedDate_dt )=" + myear.ToString();
                    mySql += "  and a.INWD_CL_Id= " + clId.ToString();
                    if (siteId != 0)
                    {
                        mySql = mySql + " and a.INWD_SITE_Id =" + siteId.ToString("0");
                    }
                    if (billId != "")
                    {
                        mySql = mySql + " and (a.INWD_BILL_Id = '" + billId + "'";
                        mySql = mySql + " Or (Select count(BILLD_BILL_Id) from tbl_BillDetail Where BILLD_ReferenceNo_int = a.INWD_ReferenceNo_int and BILLD_BILL_Id = '" + billId + "') > 0 ";
                        mySql = mySql + " Or (Select count(MBILLD_BILL_Id) from tbl_BillDetailMonthly Where MBILLD_ReferenceNo_int = a.INWD_ReferenceNo_int and MBILLD_BILL_Id = '" + billId + "') > 0 ";
                        mySql = mySql + ")";
                    }
                    mySql += otherStr.Replace("'AAC'", "'CR'");
                    //mySql += " order by a.INWD_RecordType_var ,a.INWD_RecordNo_int desc";
                    mySql += " order by a.INWD_ReceivedDate_dt desc";
                    break;
                case "FLYASH":
                    if (testType == "---All---" || testType == "Physical")
                    {
                        mySql = " SELECT a.INWD_ReceivedDate_dt as dateofreceiving, 'FlyAsh' as record_type,a.INWD_Building_var, b.FLYASHINWD_ReferenceNo_var as refno ,b.FLYASHINWD_IssueDate_dt as status" +
                        " ,c.CONT_Name_var, a.INWD_ContactNo_var, '' as MFINWD_FinalIssueDt " +
                        " FROM tbl_Inward AS a, tbl_FlyAsh_Inward AS b, tbl_Contact as c  " +
                            " WHERE a.INWD_RecordType_var = 'FLYASH' AND a.INWD_RecordType_var = b.FLYASHINWD_RecordType_var " +
                            " AND a.INWD_RecordNo_int = b.FLYASHINWD_RecordNo_int" +
                            " AND c.CONT_Id = a.INWD_CONT_Id";
                        if (bldg.Trim() != "" && bldg != "---All---")
                        {
                            mySql += " and a.INWD_Building_var='" + bldg + "'";
                        }
                        mySql += " and month(INWD_ReceivedDate_dt )=" + mth.ToString();
                        mySql += " and year(INWD_ReceivedDate_dt )=" + myear.ToString();
                        mySql += "  and a.INWD_CL_Id= " + clId.ToString();
                        if (siteId != 0)
                        {
                            mySql = mySql + " and a.INWD_SITE_Id =" + siteId.ToString("0");
                        }
                        if (billId != "")
                        {
                            mySql = mySql + " and (a.INWD_BILL_Id = '" + billId + "'";
                            mySql = mySql + " Or (Select count(BILLD_BILL_Id) from tbl_BillDetail Where BILLD_ReferenceNo_int = a.INWD_ReferenceNo_int and BILLD_BILL_Id = '" + billId + "') > 0 ";
                            mySql = mySql + " Or (Select count(MBILLD_BILL_Id) from tbl_BillDetailMonthly Where MBILLD_ReferenceNo_int = a.INWD_ReferenceNo_int and MBILLD_BILL_Id = '" + billId + "') > 0 ";
                            mySql = mySql + ")";
                        }
                        mySql += otherStr.Replace("'AAC'", "'FLYASH'");
                    }
                    if (testType == "---All---")
                    {
                        mySql += " Union ";
                    }
                    if (testType == "---All---" || testType == "Chemical")
                    {
                        mySql += otherStr.Replace("'AAC'", "'FLYASHCH'").Replace("Union", "");
                    }
                    //mySql += " order by a.INWD_RecordType_var ,a.INWD_RecordNo_int desc";
                    mySql += " order by a.INWD_ReceivedDate_dt desc";
                    break;
                case "BT-":
                    mySql = " SELECT a.INWD_ReceivedDate_dt as dateofreceiving, 'Brick' as record_type,a.INWD_Building_var, b.BTINWD_ReferenceNo_var as refno ,b.BTINWD_IssueDate_dt as status" +
                        " ,c.CONT_Name_var, a.INWD_ContactNo_var, '' as MFINWD_FinalIssueDt " +
                        " FROM tbl_Inward AS a, tbl_Brick_Inward AS b, tbl_Contact as c  " +
                            " WHERE a.INWD_RecordType_var = 'BT-' AND a.INWD_RecordType_var = b.BTINWD_RecordType_var " +
                            " AND a.INWD_RecordNo_int = b.BTINWD_RecordNo_int" +
                            " AND c.CONT_Id = a.INWD_CONT_Id";
                    if (bldg.Trim() != "" && bldg != "---All---")
                    {
                        mySql += " and a.INWD_Building_var='" + bldg + "'";
                    }
                    mySql += " and month(INWD_ReceivedDate_dt )=" + mth.ToString();
                    mySql += " and year(INWD_ReceivedDate_dt )=" + myear.ToString();
                    mySql += "  and a.INWD_CL_Id= " + clId.ToString();
                    if (siteId != 0)
                    {
                        mySql = mySql + " and a.INWD_SITE_Id =" + siteId.ToString("0");
                    }
                    mySql += " and month(INWD_ReceivedDate_dt )=" + mth.ToString();
                    mySql += " and year(INWD_ReceivedDate_dt )=" + myear.ToString();
                    if (billId != "")
                    {
                        mySql = mySql + " and (a.INWD_BILL_Id = '" + billId + "'";
                        mySql = mySql + " Or (Select count(BILLD_BILL_Id) from tbl_BillDetail Where BILLD_ReferenceNo_int = a.INWD_ReferenceNo_int and BILLD_BILL_Id = '" + billId + "') > 0 ";
                        mySql = mySql + " Or (Select count(MBILLD_BILL_Id) from tbl_BillDetailMonthly Where MBILLD_ReferenceNo_int = a.INWD_ReferenceNo_int and MBILLD_BILL_Id = '" + billId + "') > 0 ";
                        mySql = mySql + ")";
                    }
                    mySql += otherStr.Replace("'AAC'", "'BT-'");
                    //mySql += " order by a.INWD_RecordType_var ,a.INWD_RecordNo_int desc";
                    mySql += " order by a.INWD_ReceivedDate_dt desc";
                    break;
                case "PT":
                    mySql = " SELECT a.INWD_ReceivedDate_dt as dateofreceiving, 'Pavement Block' as record_type,a.INWD_Building_var, b.PTINWD_ReferenceNo_var as refno ,b.PTINWD_IssueDate_dt as status" +
                        " ,c.CONT_Name_var, a.INWD_ContactNo_var, '' as MFINWD_FinalIssueDt " +
                        " FROM tbl_Inward AS a, tbl_Pavement_Inward AS b, tbl_Contact as c  " +
                            " WHERE a.INWD_RecordType_var = 'PT' AND a.INWD_RecordType_var = b.PTINWD_RecordType_var " +
                            " AND a.INWD_RecordNo_int = b.PTINWD_RecordNo_int" +
                            " AND c.CONT_Id = a.INWD_CONT_Id";

                    if (bldg.Trim() != "" && bldg != "---All---")
                    {
                        mySql += " and a.INWD_Building_var='" + bldg + "'";
                    }
                    mySql += " and month(INWD_ReceivedDate_dt )=" + mth.ToString();
                    mySql += " and year(INWD_ReceivedDate_dt )=" + myear.ToString();
                    mySql += "  and a.INWD_CL_Id= " + clId.ToString();
                    if (siteId != 0)
                    {
                        mySql += " and a.INWD_SITE_Id =" + siteId.ToString("0");
                    }
                    if (billId != "")
                    {
                        mySql += " and (a.INWD_BILL_Id = '" + billId + "'";
                        mySql += " Or (Select count(BILLD_BILL_Id) from tbl_BillDetail Where BILLD_ReferenceNo_int = a.INWD_ReferenceNo_int and BILLD_BILL_Id = '" + billId + "') > 0 ";
                        mySql += " Or (Select count(MBILLD_BILL_Id) from tbl_BillDetailMonthly Where MBILLD_ReferenceNo_int = a.INWD_ReferenceNo_int and MBILLD_BILL_Id = '" + billId + "') > 0 ";
                        mySql += ")";
                    }
                    mySql += otherStr.Replace("'AAC'", "'PT'");
                    //mySql += " order by a.INWD_RecordType_var ,a.INWD_RecordNo_int desc";
                    mySql += " order by a.INWD_ReceivedDate_dt desc";
                    break;
                case "SOLID":
                    if (testType == "---All---" || testType == "Concrete Blocks")
                    {
                        mySql = " SELECT a.INWD_ReceivedDate_dt as dateofreceiving, 'Masonary Block' as record_type,a.INWD_Building_var, b.SOLIDINWD_ReferenceNo_var as refno ,b.SOLIDINWD_IssueDate_dt  as status" +
                        " ,c.CONT_Name_var, a.INWD_ContactNo_var, '' as MFINWD_FinalIssueDt " +
                        " FROM tbl_Inward AS a, tbl_Solid_Inward AS b, tbl_Contact as c  " +
                            " WHERE a.INWD_RecordType_var = 'SOLID' AND a.INWD_RecordType_var = b.SOLIDINWD_RecordType_var " +
                            " AND a.INWD_RecordNo_int = b.SOLIDINWD_RecordNo_int" +
                            " AND c.CONT_Id = a.INWD_CONT_Id";
                        if (bldg.Trim() != "" && bldg != "---All---")
                        {
                            mySql += " and a.INWD_Building_var='" + bldg + "'";
                        }
                        mySql += " and month(INWD_ReceivedDate_dt )=" + mth.ToString();
                        mySql += " and year(INWD_ReceivedDate_dt )=" + myear.ToString();
                        mySql += "  and a.INWD_CL_Id= " + clId.ToString();
                        if (siteId != 0)
                        {
                            mySql = mySql + " and a.INWD_SITE_Id =" + siteId.ToString("0");
                        }
                        if (billId != "")
                        {
                            mySql = mySql + " and (a.INWD_BILL_Id = '" + billId + "'";
                            mySql = mySql + " Or (Select count(BILLD_BILL_Id) from tbl_BillDetail Where BILLD_ReferenceNo_int = a.INWD_ReferenceNo_int and BILLD_BILL_Id = '" + billId + "') > 0 ";
                            mySql = mySql + " Or (Select count(MBILLD_BILL_Id) from tbl_BillDetailMonthly Where MBILLD_ReferenceNo_int = a.INWD_ReferenceNo_int and MBILLD_BILL_Id = '" + billId + "') > 0 ";
                            mySql = mySql + ")";
                        }
                        mySql += otherStr.Replace("'AAC'", "'SOLID'");
                    }
                    if (testType == "---All---")
                    {
                        mySql += " Union ";
                    }
                    if (testType == "---All---" || testType == "AAC Blocks")
                    {
                        mySql += " SELECT a.INWD_ReceivedDate_dt as dateofreceiving, 'AAC Block' as record_type,a.INWD_Building_var, b.AACINWD_ReferenceNo_var as refno ,b.AACINWD_IssueDate_dt as status" +
                        " ,c.CONT_Name_var, a.INWD_ContactNo_var, '' as MFINWD_FinalIssueDt " +
                        " FROM tbl_Inward AS a, tbl_AAC_Inward AS b, tbl_Contact as c  " +
                            " WHERE a.INWD_RecordType_var = 'AAC' AND a.INWD_RecordType_var = b.AACINWD_RecordType_var " +
                            " AND a.INWD_RecordNo_int = b.AACINWD_RecordNo_int" +
                            " AND c.CONT_Id = a.INWD_CONT_Id";
                        if (bldg.Trim() != "" && bldg != "---All---")
                        {
                            mySql += " and a.INWD_Building_var='" + bldg + "'";
                        }
                        mySql += " and month(INWD_ReceivedDate_dt )=" + mth.ToString();
                        mySql += " and year(INWD_ReceivedDate_dt )=" + myear.ToString();
                        mySql += "  and a.INWD_CL_Id= " + clId.ToString();
                        if (siteId != 0)
                        {
                            mySql = mySql + " and a.INWD_SITE_Id =" + siteId.ToString("0");
                        }
                        if (billId != "")
                        {
                            mySql = mySql + " and (a.INWD_BILL_Id = '" + billId + "'";
                            mySql = mySql + " Or (Select count(BILLD_BILL_Id) from tbl_BillDetail Where BILLD_ReferenceNo_int = a.INWD_ReferenceNo_int and BILLD_BILL_Id = '" + billId + "') > 0 ";
                            mySql = mySql + " Or (Select count(MBILLD_BILL_Id) from tbl_BillDetailMonthly Where MBILLD_ReferenceNo_int = a.INWD_ReferenceNo_int and MBILLD_BILL_Id = '" + billId + "') > 0 ";
                            mySql = mySql + ")";
                        }
                        mySql += otherStr.Replace("'AAC'", "'AAC'");
                    }
                    if (testType == "---All---")
                    {
                        mySql += " Union ";
                    }
                    if (testType == "---All---" || testType == "Bricks")
                    {
                        mySql += " SELECT a.INWD_ReceivedDate_dt as dateofreceiving, 'Brick' as record_type,a.INWD_Building_var, b.BTINWD_ReferenceNo_var as refno ,b.BTINWD_IssueDate_dt as status" +
                        " ,c.CONT_Name_var, a.INWD_ContactNo_var, '' as MFINWD_FinalIssueDt " +
                        " FROM tbl_Inward AS a, tbl_Brick_Inward AS b, tbl_Contact as c  " +
                            " WHERE a.INWD_RecordType_var = 'BT-' AND a.INWD_RecordType_var = b.BTINWD_RecordType_var " +
                            " AND a.INWD_RecordNo_int = b.BTINWD_RecordNo_int" +
                            " AND c.CONT_Id = a.INWD_CONT_Id";
                        if (bldg.Trim() != "" && bldg != "---All---")
                        {
                            mySql += " and a.INWD_Building_var='" + bldg + "'";
                        }
                        mySql += " and month(INWD_ReceivedDate_dt )=" + mth.ToString();
                        mySql += " and year(INWD_ReceivedDate_dt )=" + myear.ToString();
                        mySql += "  and a.INWD_CL_Id= " + clId.ToString();
                        if (siteId != 0)
                        {
                            mySql = mySql + " and a.INWD_SITE_Id =" + siteId.ToString("0");
                        }
                        if (billId != "")
                        {
                            mySql = mySql + " and (a.INWD_BILL_Id = '" + billId + "'";
                            mySql = mySql + " Or (Select count(BILLD_BILL_Id) from tbl_BillDetail Where BILLD_ReferenceNo_int = a.INWD_ReferenceNo_int and BILLD_BILL_Id = '" + billId + "') > 0 ";
                            mySql = mySql + " Or (Select count(MBILLD_BILL_Id) from tbl_BillDetailMonthly Where MBILLD_ReferenceNo_int = a.INWD_ReferenceNo_int and MBILLD_BILL_Id = '" + billId + "') > 0 ";
                            mySql = mySql + ")";
                        }
                        mySql += otherStr.Replace("'AAC'", "'BT-'");
                    }
                    //mySql += " order by a.INWD_RecordType_var ,a.INWD_RecordNo_int desc";
                    mySql += " order by a.INWD_ReceivedDate_dt desc";
                    break;
                case "PILE":
                    mySql = " SELECT a.INWD_ReceivedDate_dt as dateofreceiving, 'Pile' as record_type,a.INWD_Building_var, b.PILEINWD_ReferenceNo_var as refno ,b.PILEINWD_IssueDate_dt as status" +
                        " ,c.CONT_Name_var, a.INWD_ContactNo_var, '' as MFINWD_FinalIssueDt " +
                        " FROM tbl_Inward AS a, tbl_Pile_Inward AS b, tbl_Contact as c  " +
                            " WHERE a.INWD_RecordType_var = 'PILE' AND a.INWD_RecordType_var = b.PILEINWD_RecordType_var " +
                            " AND a.INWD_RecordNo_int = b.PILEINWD_RecordNo_int" +
                            " AND c.CONT_Id = a.INWD_CONT_Id";
                    if (bldg.Trim() != "" && bldg != "---All---")
                    {
                        mySql += " and a.INWD_Building_var='" + bldg + "'";
                    }
                    mySql += " and month(INWD_ReceivedDate_dt )=" + mth.ToString();
                    mySql += " and year(INWD_ReceivedDate_dt )=" + myear.ToString();
                    mySql += "  and a.INWD_CL_Id= " + clId.ToString();
                    if (siteId != 0)
                    {
                        mySql = mySql + " and a.INWD_SITE_Id =" + siteId.ToString("0");
                    }
                    if (billId != "")
                    {
                        mySql = mySql + " and (a.INWD_BILL_Id = '" + billId + "'";
                        mySql = mySql + " Or (Select count(BILLD_BILL_Id) from tbl_BillDetail Where BILLD_ReferenceNo_int = a.INWD_ReferenceNo_int and BILLD_BILL_Id = '" + billId + "') > 0 ";
                        mySql = mySql + " Or (Select count(MBILLD_BILL_Id) from tbl_BillDetailMonthly Where MBILLD_ReferenceNo_int = a.INWD_ReferenceNo_int and MBILLD_BILL_Id = '" + billId + "') > 0 ";
                        mySql = mySql + ")";
                    }
                    mySql += otherStr.Replace("'AAC'", "'PILE'");
                    //mySql += " order by a.INWD_RecordType_var ,a.INWD_RecordNo_int desc";
                    mySql += " order by a.INWD_ReceivedDate_dt desc";
                    break;
                case "TILE":
                    if (testType == "---All---" || testType == "Physical")
                    {
                        mySql = " SELECT a.INWD_ReceivedDate_dt as dateofreceiving, 'Tile' as record_type,a.INWD_Building_var, b.TILEINWD_ReferenceNo_var as refno ,b.TILEINWD_IssueDate_dt as status" +
                        " ,c.CONT_Name_var, a.INWD_ContactNo_var, '' as MFINWD_FinalIssueDt " +
                        " FROM tbl_Inward AS a, tbl_Tile_Inward AS b, tbl_Contact as c  " +
                            " WHERE a.INWD_RecordType_var = 'TILE' AND a.INWD_RecordType_var = b.TILEINWD_RecordType_var " +
                            " AND a.INWD_RecordNo_int = b.TILEINWD_RecordNo_int" +
                            " AND c.CONT_Id = a.INWD_CONT_Id";
                        if (bldg.Trim() != "" && bldg != "---All---")
                        {
                            mySql += " and a.INWD_Building_var='" + bldg + "'";
                        }
                        mySql += " and month(INWD_ReceivedDate_dt )=" + mth.ToString();
                        mySql += " and year(INWD_ReceivedDate_dt )=" + myear.ToString();
                        mySql += "  and a.INWD_CL_Id= " + clId.ToString();
                        if (siteId != 0)
                        {
                            mySql = mySql + " and a.INWD_SITE_Id =" + siteId.ToString("0");
                        }
                        if (billId != "")
                        {
                            mySql = mySql + " and (a.INWD_BILL_Id = '" + billId + "'";
                            mySql = mySql + " Or (Select count(BILLD_BILL_Id) from tbl_BillDetail Where BILLD_ReferenceNo_int = a.INWD_ReferenceNo_int and BILLD_BILL_Id = '" + billId + "') > 0 ";
                            mySql = mySql + " Or (Select count(MBILLD_BILL_Id) from tbl_BillDetailMonthly Where MBILLD_ReferenceNo_int = a.INWD_ReferenceNo_int and MBILLD_BILL_Id = '" + billId + "') > 0 ";
                            mySql = mySql + ")";
                        }
                        mySql += otherStr.Replace("'AAC'", "'TILE'");
                    }
                    if (testType == "---All---")
                    {
                        mySql += " Union ";
                    }
                    if (testType == "---All---" || testType == "Chemical")
                    {
                        mySql += otherStr.Replace("'AAC'", "'TILECH'").Replace("Union", "");
                    }
                    //mySql += " order by a.INWD_RecordType_var ,a.INWD_RecordNo_int desc";
                    mySql += " order by a.INWD_ReceivedDate_dt desc";
                    break;
                case "STC":
                    mySql = " SELECT a.INWD_ReceivedDate_dt as dateofreceiving, 'Steel Chemical' as record_type,a.INWD_Building_var, b.STCINWD_ReferenceNo_var as refno ,b.STCINWD_IssueDate_dt as status" +
                        " ,c.CONT_Name_var, a.INWD_ContactNo_var, '' as MFINWD_FinalIssueDt " +
                        " FROM tbl_Inward AS a, tbl_SteelChemical_Inward AS b, tbl_Contact as c  " +
                            " WHERE a.INWD_RecordType_var = 'STC' AND a.INWD_RecordType_var = b.STCINWD_RecordType_var " +
                            " AND a.INWD_RecordNo_int = b.STCINWD_RecordNo_int" +
                            " AND c.CONT_Id = a.INWD_CONT_Id";
                    if (bldg.Trim() != "" && bldg != "---All---")
                    {
                        mySql += " and a.INWD_Building_var='" + bldg + "'";
                    }
                    mySql += " and month(INWD_ReceivedDate_dt )=" + mth.ToString();
                    mySql += " and year(INWD_ReceivedDate_dt )=" + myear.ToString();
                    mySql += "  and a.INWD_CL_Id= " + clId.ToString();
                    if (siteId != 0)
                    {
                        mySql = mySql + " and a.INWD_SITE_Id =" + siteId.ToString("0");
                    }
                    if (billId != "")
                    {
                        mySql = mySql + " and (a.INWD_BILL_Id = '" + billId + "'";
                        mySql = mySql + " Or (Select count(BILLD_BILL_Id) from tbl_BillDetail Where BILLD_ReferenceNo_int = a.INWD_ReferenceNo_int and BILLD_BILL_Id = '" + billId + "') > 0 ";
                        mySql = mySql + " Or (Select count(MBILLD_BILL_Id) from tbl_BillDetailMonthly Where MBILLD_ReferenceNo_int = a.INWD_ReferenceNo_int and MBILLD_BILL_Id = '" + billId + "') > 0 ";
                        mySql = mySql + ")";
                    }
                    mySql += otherStr.Replace("'AAC'", "'STC'");
                    //mySql += " order by a.INWD_RecordType_var ,a.INWD_RecordNo_int desc";
                    mySql += " order by a.INWD_ReceivedDate_dt desc";
                    break;
                case "CCH":
                    mySql = " SELECT a.INWD_ReceivedDate_dt as dateofreceiving, 'Cement Chemical' as record_type,a.INWD_Building_var, b.CCHINWD_ReferenceNo_var as refno ,b.CCHINWD_IssueDate_dt as status" +
                        " ,c.CONT_Name_var, a.INWD_ContactNo_var, '' as MFINWD_FinalIssueDt " +
                        " FROM tbl_Inward AS a, tbl_CementChemical_Inward AS b, tbl_Contact as c  " +
                            " WHERE a.INWD_RecordType_var = 'CCH' AND a.INWD_RecordType_var = b.CCHINWD_RecordType_var " +
                            " AND a.INWD_RecordNo_int = b.CCHINWD_RecordNo_int" +
                            " AND c.CONT_Id = a.INWD_CONT_Id";
                    if (bldg.Trim() != "" && bldg != "---All---")
                    {
                        mySql += " and a.INWD_Building_var='" + bldg + "'";
                    }
                    mySql += " and month(INWD_ReceivedDate_dt )=" + mth.ToString();
                    mySql += " and year(INWD_ReceivedDate_dt )=" + myear.ToString();
                    mySql += "  and a.INWD_CL_Id= " + clId.ToString();
                    if (siteId != 0)
                    {
                        mySql = mySql + " and a.INWD_SITE_Id =" + siteId.ToString("0");
                    }
                    if (billId != "")
                    {
                        mySql = mySql + " and (a.INWD_BILL_Id = '" + billId + "'";
                        mySql = mySql + " Or (Select count(BILLD_BILL_Id) from tbl_BillDetail Where BILLD_ReferenceNo_int = a.INWD_ReferenceNo_int and BILLD_BILL_Id = '" + billId + "') > 0 ";
                        mySql = mySql + " Or (Select count(MBILLD_BILL_Id) from tbl_BillDetailMonthly Where MBILLD_ReferenceNo_int = a.INWD_ReferenceNo_int and MBILLD_BILL_Id = '" + billId + "') > 0 ";
                        mySql = mySql + ")";
                    }
                    mySql += otherStr.Replace("'AAC'", "'CCH'");
                    //mySql += " order by a.INWD_RecordType_var ,a.INWD_RecordNo_int desc";
                    mySql += " order by a.INWD_ReceivedDate_dt desc";
                    break;
                case "WT":
                    mySql = " SELECT a.INWD_ReceivedDate_dt as dateofreceiving,'Water' as record_type,a.INWD_Building_var, b.WTINWD_ReferenceNo_var as refno ,b.WTINWD_IssueDate_dt as status" +
                        " ,c.CONT_Name_var, a.INWD_ContactNo_var, '' as MFINWD_FinalIssueDt " +
                        " FROM tbl_Inward AS a, tbl_Water_Inward AS b, tbl_Contact as c  " +
                            " WHERE a.INWD_RecordType_var = 'WT' AND a.INWD_RecordType_var = b.WTINWD_RecordType_var " +
                            " AND a.INWD_RecordNo_int = b.WTINWD_RecordNo_int" +
                            " AND c.CONT_Id = a.INWD_CONT_Id";
                    if (bldg.Trim() != "" && bldg != "---All---")
                    {
                        mySql += " and a.INWD_Building_var='" + bldg + "'";
                    }
                    mySql += " and month(INWD_ReceivedDate_dt )=" + mth.ToString();
                    mySql += " and year(INWD_ReceivedDate_dt )=" + myear.ToString();
                    mySql += "  and a.INWD_CL_Id= " + clId.ToString();
                    if (siteId != 0)
                    {
                        mySql = mySql + " and a.INWD_SITE_Id =" + siteId.ToString("0");
                    }
                    if (billId != "")
                    {
                        mySql = mySql + " and (a.INWD_BILL_Id = '" + billId + "'";
                        mySql = mySql + " Or (Select count(BILLD_BILL_Id) from tbl_BillDetail Where BILLD_ReferenceNo_int = a.INWD_ReferenceNo_int and BILLD_BILL_Id = '" + billId + "') > 0 ";
                        mySql = mySql + " Or (Select count(MBILLD_BILL_Id) from tbl_BillDetailMonthly Where MBILLD_ReferenceNo_int = a.INWD_ReferenceNo_int and MBILLD_BILL_Id = '" + billId + "') > 0 ";
                        mySql = mySql + ")";
                    }
                    mySql += otherStr.Replace("'AAC'", "'WT'");
                    //mySql += " order by a.INWD_RecordType_var ,a.INWD_RecordNo_int desc";
                    mySql += " order by a.INWD_ReceivedDate_dt desc";
                    break;
                case "SO":
                    mySql = "SELECT a.INWD_ReceivedDate_dt as dateofreceiving,'Soil' as record_type," +
                    " a.INWD_Building_var, b.SOINWD_ReferenceNo_var as refno , b.SOINWD_IssueDate_dt as status " +
                    " ,c.CONT_Name_var, a.INWD_ContactNo_var, '' as MFINWD_FinalIssueDt " +
                    " FROM tbl_Inward AS a, tbl_Soil_Inward AS b, tbl_Contact as c " +
                    " WHERE a.INWD_RecordType_var = 'SO'  AND a.INWD_RecordType_var = b.SOINWD_RecordType_var AND a.INWD_RecordNo_int = b.SOINWD_RecordNo_int " +
                    " AND c.CONT_Id = a.INWD_CONT_Id";

                    if (bldg.Trim() != "" && bldg != "---All---")
                    {
                        mySql += " and a.INWD_Building_var='" + bldg + "'";
                    }
                    mySql += " and month(INWD_ReceivedDate_dt )=" + mth.ToString();
                    mySql += " and year(INWD_ReceivedDate_dt )=" + myear.ToString();
                    mySql += "  and a.INWD_CL_Id= " + clId.ToString();
                    if (siteId != 0)
                    {
                        mySql = mySql + " and a.INWD_SITE_Id =" + siteId.ToString("0");
                    }
                    if (billId != "")
                    {
                        mySql = mySql + " and (a.INWD_BILL_Id = '" + billId + "'";
                        mySql = mySql + " Or (Select count(BILLD_BILL_Id) from tbl_BillDetail Where BILLD_ReferenceNo_int = a.INWD_ReferenceNo_int and BILLD_BILL_Id = '" + billId + "') > 0 ";
                        mySql = mySql + " Or (Select count(MBILLD_BILL_Id) from tbl_BillDetailMonthly Where MBILLD_ReferenceNo_int = a.INWD_ReferenceNo_int and MBILLD_BILL_Id = '" + billId + "') > 0 ";
                        mySql = mySql + ")";
                    }
                    mySql += otherStr.Replace("'AAC'", "'SO'");
                    //mySql += " order by a.INWD_RecordType_var ,a.INWD_RecordNo_int desc";
                    mySql += " order by a.INWD_ReceivedDate_dt desc";
                    break;
                case "OT":
                    mySql = otherStr.Replace("'AAC'", "'OT'").Replace("Union", "");
                    mySql += " order by a.INWD_ReceivedDate_dt desc";
                    break;
                default:
                    mySql = "";
                    break;
            }
            #endregion

            DataTable dtReturn = new DataTable();
            if (mySql != "")
            {
                SqlDataAdapter da = new SqlDataAdapter(mySql, cn);
                DataSet ds = new DataSet();
                ds.Reset();
                da.Fill(ds);
                dtReturn = new DataTable();
                dtReturn = ds.Tables[0];
                //dtReturn.Merge(dt1);
            }
            return dtReturn;
        }
        catch (Exception e)
        {
            throw e;
        }

    }

    public string getBill(double clId, double siteId,Int32 mnth,Int32 myear, string conStr)
    {
        string mySql = "Select top (1) bill_id from tbl_bill where BILL_CL_Id =" + clId.ToString()+ " and BILL_SITE_Id =" + siteId.ToString() + " and month(BILL_Date_dt) = " + mnth.ToString() + " and YEAR(BILL_Date_dt) =" + myear.ToString();
        //cn.ConnectionString= System.Configuration.ConfigurationManager.ConnectionStrings["connStrNew"].ToString();
        cn.ConnectionString = conStr;
        cn.Open();
        DataSet ds = new DataSet();
        DataTable dt = new DataTable();
        SqlDataAdapter da = new SqlDataAdapter(mySql, cn);
        da.Fill(ds);
        dt = ds.Tables[0];
        ds.Dispose();
        CloseConnection();
        return dt.Rows[0]["bill_id"].ToString() ;

    }


    public DataTable getReportList2016_BillApproval(double clId, double siteId, string supplier, string bldg, string rType, string db, string billId, string testType)
    {
        string mySql = "";
        try
        {
            DataSet ds = new DataSet();
            DataTable dtReturn = new DataTable();
            string selRecType = rType;
            mySql = "select distinct(a.INWD_RecordType_var) from tbl_Inward as a where ";
            mySql = mySql + "  a.INWD_CL_Id= " + clId.ToString("0");
            if (siteId != 0)
            {
                mySql = mySql + " and a.INWD_SITE_Id =" + siteId.ToString("0");
            }
            if (billId != "")
            {
                mySql = mySql + " and (a.INWD_BILL_Id = '" + billId + "'";
                mySql = mySql + " Or (Select count(BILLD_BILL_Id) from tbl_BillDetail Where BILLD_ReferenceNo_int = a.INWD_ReferenceNo_int and BILLD_BILL_Id = '" + billId + "') > 0 ";
                mySql = mySql + " Or (Select count(MBILLD_BILL_Id) from tbl_BillDetailMonthly Where MBILLD_ReferenceNo_int = a.INWD_ReferenceNo_int and MBILLD_BILL_Id = '" + billId + "') > 0 ";
                mySql = mySql + ")";
            }
            #region  other report query
            string otherStr = @" Union SELECT a.INWD_ReceivedDate_dt as dateofreceiving, 'Other' as record_type, a.INWD_Building_var, 
                              b.OTINWD_ReferenceNo_var as refno, b.OTINWD_IssueDate_dt as status, c.CONT_Name_var, a.INWD_ContactNo_var, '' as MFINWD_FinalIssueDt  
                              FROM tbl_Inward AS a, tbl_Other_Inward AS b, tbl_Contact as c   
                              WHERE a.INWD_RecordType_var = 'OT' AND a.INWD_RecordType_var = b.OTINWD_RecordType_var  
                              AND a.INWD_RecordNo_int = b.OTINWD_RecordNo_int AND c.CONT_Id = a.INWD_CONT_Id ";
            otherStr += "and b.OTINWD_Section_var = 'AAC'";
            if (bldg.Trim() != "" && bldg != "---All---")
            {
                otherStr += " and a.INWD_Building_var='" + bldg + "'";
            }
            if (supplier.Trim() != "" && supplier != "---All---")
            {
                otherStr += " and b.OTINWD_SupplierName_var='" + supplier + "'";
            }
            otherStr += "  and a.INWD_CL_Id= " + clId.ToString();
            if (siteId != 0)
            {
                otherStr += " and a.INWD_SITE_Id =" + siteId.ToString("0");
            }
            if (billId != "")
            {
                otherStr += " and (a.INWD_BILL_Id = '" + billId + "'";
                otherStr += " Or (Select count(BILLD_BILL_Id) from tbl_BillDetail Where BILLD_ReferenceNo_int = a.INWD_ReferenceNo_int and BILLD_BILL_Id = '" + billId + "') > 0 ";
                otherStr += " Or (Select count(MBILLD_BILL_Id) from tbl_BillDetailMonthly Where MBILLD_ReferenceNo_int = a.INWD_ReferenceNo_int and MBILLD_BILL_Id = '" + billId + "') > 0 ";
                otherStr += ")";
            }
            #endregion
            #region query
            DataTable dt = getGeneralData(mySql, db);
            DataTable dt1 = new DataTable();
            foreach (DataRow rw in dt.Rows)
            {
                rType = rw["INWD_RecordType_var"].ToString();
                switch (rType)
                {
                    case "CT":
                        mySql = " SELECT a.INWD_ReceivedDate_dt as dateofreceiving, 'Cube' as record_type,a.INWD_Building_var, b.CTINWD_ReferenceNo_var as refno ,b.CTINWD_IssueDate_dt as status" +
                                " ,c.CONT_Name_var, a.INWD_ContactNo_var, '' as MFINWD_FinalIssueDt " +
                                " FROM tbl_Inward AS a, tbl_Cube_Inward AS b, tbl_Contact as c " +
                                " WHERE a.INWD_RecordType_var = 'CT' AND a.INWD_RecordType_var = b.CTINWD_RecordType_var " +
                                " AND a.INWD_RecordNo_int = b.CTINWD_RecordNo_int" +
                                " AND c.CONT_Id = a.INWD_CONT_Id";
                        if (bldg.Trim() != "" && bldg != "---All---")
                        {
                            mySql += " and a.INWD_Building_var='" + bldg + "'";
                        }
                        if (supplier.Trim() != "" && supplier != "---All---")
                        {
                            mySql += " and b.CTINWD_SupplierName_var='" + supplier + "'";
                        }
                        mySql += "  and a.INWD_CL_Id= " + clId.ToString();
                        if (siteId != 0)
                        {
                            mySql = mySql + " and a.INWD_SITE_Id =" + siteId.ToString("0");
                        }
                        if (billId != "")
                        {
                            mySql = mySql + " and (a.INWD_BILL_Id = '" + billId + "'";
                            mySql = mySql + " Or (Select count(BILLD_BILL_Id) from tbl_BillDetail Where BILLD_ReferenceNo_int = a.INWD_ReferenceNo_int and BILLD_BILL_Id = '" + billId + "') > 0 ";
                            mySql = mySql + " Or (Select count(MBILLD_BILL_Id) from tbl_BillDetailMonthly Where MBILLD_ReferenceNo_int = a.INWD_ReferenceNo_int and MBILLD_BILL_Id = '" + billId + "') > 0 ";
                            mySql = mySql + ")";
                        }
                        mySql += otherStr.Replace("'AAC'", "'CT'");
                        //mySql += " order by a.INWD_RecordType_var ,a.INWD_RecordNo_int desc";
                        mySql += " order by a.INWD_ReceivedDate_dt desc";
                        break;
                    case "ST":
                        if (testType == "---All---" || testType == "Physical")
                        {
                            mySql = " SELECT a.INWD_ReceivedDate_dt as dateofreceiving, 'Steel' as record_type,a.INWD_Building_var, b.STINWD_ReferenceNo_var as refno ,b.STINWD_IssueDate_dt as status" +
                                    " ,c.CONT_Name_var, a.INWD_ContactNo_var, '' as MFINWD_FinalIssueDt " +
                                    " FROM tbl_Inward AS a, tbl_Steel_Inward AS b, tbl_Contact as c " +
                                    " WHERE a.INWD_RecordType_var = 'ST' AND a.INWD_RecordType_var = b.STINWD_RecordType_var " +
                                    " AND a.INWD_RecordNo_int = b.STINWD_RecordNo_int" +
                                    " AND c.CONT_Id = a.INWD_CONT_Id";
                            if (bldg.Trim() != "" && bldg != "---All---")
                            {
                                mySql += " and a.INWD_Building_var='" + bldg + "'";
                            }
                            if (supplier.Trim() != "" && supplier != "---All---")
                            {
                                mySql += " and b.STINWD_SupplierName_var='" + supplier + "'";
                            }
                            mySql += "  and a.INWD_CL_Id= " + clId.ToString();
                            if (siteId != 0)
                            {
                                mySql = mySql + " and a.INWD_SITE_Id =" + siteId.ToString("0");
                            }
                            if (billId != "")
                            {
                                mySql = mySql + " and (a.INWD_BILL_Id = '" + billId + "'";
                                mySql = mySql + " Or (Select count(BILLD_BILL_Id) from tbl_BillDetail Where BILLD_ReferenceNo_int = a.INWD_ReferenceNo_int and BILLD_BILL_Id = '" + billId + "') > 0 ";
                                mySql = mySql + " Or (Select count(MBILLD_BILL_Id) from tbl_BillDetailMonthly Where MBILLD_ReferenceNo_int = a.INWD_ReferenceNo_int and MBILLD_BILL_Id = '" + billId + "') > 0 ";
                                mySql = mySql + ")";
                            }
                            mySql += otherStr.Replace("'AAC'", "'ST'");
                        }
                        if (testType == "---All---")
                        {
                            mySql += " Union ";
                        }
                        if (testType == "---All---" || testType == "Chemical")
                        {
                            mySql += " SELECT a.INWD_ReceivedDate_dt as dateofreceiving, 'Steel Chemical' as record_type,a.INWD_Building_var, b.STCINWD_ReferenceNo_var as refno ,b.STCINWD_IssueDate_dt as status" +
                            " ,c.CONT_Name_var, a.INWD_ContactNo_var, '' as MFINWD_FinalIssueDt " +
                            " FROM tbl_Inward AS a, tbl_SteelChemical_Inward AS b, tbl_Contact as c  " +
                                " WHERE a.INWD_RecordType_var = 'STC' AND a.INWD_RecordType_var = b.STCINWD_RecordType_var " +
                                " AND a.INWD_RecordNo_int = b.STCINWD_RecordNo_int" +
                                " AND c.CONT_Id = a.INWD_CONT_Id";
                            if (bldg.Trim() != "" && bldg != "---All---")
                            {
                                mySql += " and a.INWD_Building_var='" + bldg + "'";
                            }
                            if (supplier.Trim() != "" && supplier != "---All---")
                            {
                                mySql += " and b.STCINWD_SupplierName_var='" + supplier + "'";
                            }
                            mySql += "  and a.INWD_CL_Id= " + clId.ToString();
                            if (siteId != 0)
                            {
                                mySql = mySql + " and a.INWD_SITE_Id =" + siteId.ToString("0");
                            }
                            if (billId != "")
                            {
                                mySql = mySql + " and (a.INWD_BILL_Id = '" + billId + "'";
                                mySql = mySql + " Or (Select count(BILLD_BILL_Id) from tbl_BillDetail Where BILLD_ReferenceNo_int = a.INWD_ReferenceNo_int and BILLD_BILL_Id = '" + billId + "') > 0 ";
                                mySql = mySql + " Or (Select count(MBILLD_BILL_Id) from tbl_BillDetailMonthly Where MBILLD_ReferenceNo_int = a.INWD_ReferenceNo_int and MBILLD_BILL_Id = '" + billId + "') > 0 ";
                                mySql = mySql + ")";
                            }
                            mySql += otherStr.Replace("'AAC'", "'STC'");
                        }
                        //mySql += " order by a.INWD_RecordType_var ,a.INWD_RecordNo_int desc";
                        mySql += " order by a.INWD_ReceivedDate_dt desc";
                        break;
                    case "MF":
                        mySql = " SELECT a.INWD_ReceivedDate_dt as dateofreceiving,'Mix Design' as record_type,a.INWD_Building_var, b.MFINWD_ReferenceNo_var as refno ,b.MFINWD_MDLIssueDt as status" +
                            " ,c.CONT_Name_var, a.INWD_ContactNo_var, CONVERT(varchar,b.MFINWD_FinalIssueDt) as MFINWD_FinalIssueDt " +
                            " FROM tbl_Inward AS a, tbl_MixDesign_Inward AS b , tbl_Contact as c " +
                                " WHERE a.INWD_RecordType_var = 'MF' AND a.INWD_RecordType_var = b.MFINWD_RecordType_var " +
                                " AND a.INWD_RecordNo_int = b.MFINWD_RecordNo_int" +
                                " AND c.CONT_Id = a.INWD_CONT_Id";
                        if (bldg.Trim() != "" && bldg != "---All---")
                        {
                            mySql += " and a.INWD_Building_var='" + bldg + "'";
                        }
                        if (supplier.Trim() != "" && supplier != "---All---") //mf, cr, ndt not having suppiler name
                        {
                            mySql += " and a.INWD_RecordType_var <> 'MF'";
                        }

                        mySql += "  and a.INWD_CL_Id= " + clId.ToString();
                        if (siteId != 0)
                        {
                            mySql = mySql + " and a.INWD_SITE_Id =" + siteId.ToString("0");
                        }
                        if (billId != "")
                        {
                            mySql = mySql + " and (a.INWD_BILL_Id = '" + billId + "'";
                            mySql = mySql + " Or (Select count(BILLD_BILL_Id) from tbl_BillDetail Where BILLD_ReferenceNo_int = a.INWD_ReferenceNo_int and BILLD_BILL_Id = '" + billId + "') > 0 ";
                            mySql = mySql + " Or (Select count(MBILLD_BILL_Id) from tbl_BillDetailMonthly Where MBILLD_ReferenceNo_int = a.INWD_ReferenceNo_int and MBILLD_BILL_Id = '" + billId + "') > 0 ";
                            mySql = mySql + ")";
                        }
                        mySql += otherStr.Replace("'AAC'", "'MF'");
                        //mySql += " order by a.INWD_RecordType_var ,a.INWD_RecordNo_int desc";
                        mySql += " order by a.INWD_ReceivedDate_dt desc";
                        break;
                    case "CEMT":
                        if (testType == "---All---" || testType == "Physical")
                        {
                            mySql = " SELECT a.INWD_ReceivedDate_dt as dateofreceiving, 'Cement' as record_type,a.INWD_Building_var, b.CEMTINWD_ReferenceNo_var as refno ,b.CEMTINWD_IssueDate_dt as status" +
                            " ,c.CONT_Name_var, a.INWD_ContactNo_var, '' as MFINWD_FinalIssueDt " +
                            " FROM tbl_Inward AS a, tbl_Cement_Inward AS b, tbl_Contact as c  " +
                                " WHERE a.INWD_RecordType_var = 'CEMT' AND a.INWD_RecordType_var = b.CEMTINWD_RecordType_var " +
                                " AND a.INWD_RecordNo_int = b.CEMTINWD_RecordNo_int" +
                                " AND c.CONT_Id = a.INWD_CONT_Id";
                            if (bldg.Trim() != "" && bldg != "---All---")
                            {
                                mySql += " and a.INWD_Building_var='" + bldg + "'";
                            }
                            if (supplier.Trim() != "" && supplier != "---All---")
                            {
                                mySql += " and b.CEMTINWD_SupplierName_var='" + supplier + "'";
                            }
                            mySql += "  and a.INWD_CL_Id= " + clId.ToString();
                            if (siteId != 0)
                            {
                                mySql = mySql + " and a.INWD_SITE_Id =" + siteId.ToString("0");
                            }
                            if (billId != "")
                            {
                                mySql = mySql + " and (a.INWD_BILL_Id = '" + billId + "'";
                                mySql = mySql + " Or (Select count(BILLD_BILL_Id) from tbl_BillDetail Where BILLD_ReferenceNo_int = a.INWD_ReferenceNo_int and BILLD_BILL_Id = '" + billId + "') > 0 ";
                                mySql = mySql + " Or (Select count(MBILLD_BILL_Id) from tbl_BillDetailMonthly Where MBILLD_ReferenceNo_int = a.INWD_ReferenceNo_int and MBILLD_BILL_Id = '" + billId + "') > 0 ";
                                mySql = mySql + ")";
                            }
                            mySql += otherStr.Replace("'AAC'", "'CEMT'");
                        }
                        if (testType == "---All---")
                        {
                            mySql += " Union ";
                        }
                        if (testType == "---All---" || testType == "Chemical")
                        {
                            mySql += " SELECT a.INWD_ReceivedDate_dt as dateofreceiving, 'Cement Chemical' as record_type,a.INWD_Building_var, b.CCHINWD_ReferenceNo_var as refno ,b.CCHINWD_IssueDate_dt as status" +
                            " ,c.CONT_Name_var, a.INWD_ContactNo_var, '' as MFINWD_FinalIssueDt " +
                            " FROM tbl_Inward AS a, tbl_CementChemical_Inward AS b, tbl_Contact as c  " +
                                " WHERE a.INWD_RecordType_var = 'CCH' AND a.INWD_RecordType_var = b.CCHINWD_RecordType_var " +
                                " AND a.INWD_RecordNo_int = b.CCHINWD_RecordNo_int" +
                                " AND c.CONT_Id = a.INWD_CONT_Id";
                            if (bldg.Trim() != "" && bldg != "---All---")
                            {
                                mySql += " and a.INWD_Building_var='" + bldg + "'";
                            }
                            if (supplier.Trim() != "" && supplier != "---All---")
                            {
                                mySql += " and b.CCHINWD_SupplierName_var='" + supplier + "'";
                            }
                            mySql += "  and a.INWD_CL_Id= " + clId.ToString();
                            if (siteId != 0)
                            {
                                mySql = mySql + " and a.INWD_SITE_Id =" + siteId.ToString("0");
                            }
                            if (billId != "")
                            {
                                mySql = mySql + " and (a.INWD_BILL_Id = '" + billId + "'";
                                mySql = mySql + " Or (Select count(BILLD_BILL_Id) from tbl_BillDetail Where BILLD_ReferenceNo_int = a.INWD_ReferenceNo_int and BILLD_BILL_Id = '" + billId + "') > 0 ";
                                mySql = mySql + " Or (Select count(MBILLD_BILL_Id) from tbl_BillDetailMonthly Where MBILLD_ReferenceNo_int = a.INWD_ReferenceNo_int and MBILLD_BILL_Id = '" + billId + "') > 0 ";
                                mySql = mySql + ")";
                            }
                            mySql += otherStr.Replace("'AAC'", "'CCH'");
                        }
                        //mySql += " order by a.INWD_RecordType_var ,a.INWD_RecordNo_int desc";
                        mySql += " order by a.INWD_ReceivedDate_dt desc";
                        break;
                    case "AGGT":
                        mySql = " SELECT a.INWD_ReceivedDate_dt as dateofreceiving, 'Aggregate' as record_type,a.INWD_Building_var, b.AGGTINWD_ReferenceNo_var as refno ,b.AGGTINWD_IssueDate_dt as status" +
                            " ,c.CONT_Name_var, a.INWD_ContactNo_var, '' as MFINWD_FinalIssueDt " +
                            " FROM tbl_Inward AS a, tbl_Aggregate_Inward AS b, tbl_Contact as c  " +
                                " WHERE a.INWD_RecordType_var = 'AGGT' AND a.INWD_RecordType_var = b.AGGTINWD_RecordType_var " +
                                " AND a.INWD_RecordNo_int = b.AGGTINWD_RecordNo_int" +
                                " AND c.CONT_Id = a.INWD_CONT_Id";
                        if (bldg.Trim() != "" && bldg != "---All---")
                        {
                            mySql += " and a.INWD_Building_var='" + bldg + "'";
                        }
                        if (supplier.Trim() != "" && supplier != "---All---")
                        {
                            mySql += " and b.AGGTINWD_SupplierName_var='" + supplier + "'";
                        }
                        mySql += "  and a.INWD_CL_Id= " + clId.ToString();
                        if (siteId != 0)
                        {
                            mySql = mySql + " and a.INWD_SITE_Id =" + siteId.ToString("0");
                        }
                        if (billId != "")
                        {
                            mySql = mySql + " and (a.INWD_BILL_Id = '" + billId + "'";
                            mySql = mySql + " Or (Select count(BILLD_BILL_Id) from tbl_BillDetail Where BILLD_ReferenceNo_int = a.INWD_ReferenceNo_int and BILLD_BILL_Id = '" + billId + "') > 0 ";
                            mySql = mySql + " Or (Select count(MBILLD_BILL_Id) from tbl_BillDetailMonthly Where MBILLD_ReferenceNo_int = a.INWD_ReferenceNo_int and MBILLD_BILL_Id = '" + billId + "') > 0 ";
                            mySql = mySql + ")";
                        }
                        mySql += otherStr.Replace("'AAC'", "'AGGT'");
                        //mySql += " order by a.INWD_RecordType_var ,a.INWD_RecordNo_int desc";
                        mySql += " order by a.INWD_ReceivedDate_dt desc";
                        break;
                    case "AAC":
                        mySql = " SELECT a.INWD_ReceivedDate_dt as dateofreceiving, 'AAC Block' as record_type,a.INWD_Building_var, b.AACINWD_ReferenceNo_var as refno ,b.AACINWD_IssueDate_dt as status" +
                            " ,c.CONT_Name_var, a.INWD_ContactNo_var, '' as MFINWD_FinalIssueDt " +
                            " FROM tbl_Inward AS a, tbl_AAC_Inward AS b, tbl_Contact as c  " +
                                " WHERE a.INWD_RecordType_var = 'AAC' AND a.INWD_RecordType_var = b.AACINWD_RecordType_var " +
                                " AND a.INWD_RecordNo_int = b.AACINWD_RecordNo_int" +
                                " AND c.CONT_Id = a.INWD_CONT_Id";
                        if (bldg.Trim() != "" && bldg != "---All---")
                        {
                            mySql += " and a.INWD_Building_var='" + bldg + "'";
                        }
                        if (supplier.Trim() != "" && supplier != "---All---")
                        {
                            mySql += " and b.AACINWD_SupplierName_var='" + supplier + "'";
                        }
                        mySql += "  and a.INWD_CL_Id= " + clId.ToString();
                        if (siteId != 0)
                        {
                            mySql = mySql + " and a.INWD_SITE_Id =" + siteId.ToString("0");
                        }
                        if (billId != "")
                        {
                            mySql = mySql + " and (a.INWD_BILL_Id = '" + billId + "'";
                            mySql = mySql + " Or (Select count(BILLD_BILL_Id) from tbl_BillDetail Where BILLD_ReferenceNo_int = a.INWD_ReferenceNo_int and BILLD_BILL_Id = '" + billId + "') > 0 ";
                            mySql = mySql + " Or (Select count(MBILLD_BILL_Id) from tbl_BillDetailMonthly Where MBILLD_ReferenceNo_int = a.INWD_ReferenceNo_int and MBILLD_BILL_Id = '" + billId + "') > 0 ";
                            mySql = mySql + ")";
                        }
                        mySql += otherStr.Replace("'AAC'", "'AAC'");
                        //mySql += " order by a.INWD_RecordType_var ,a.INWD_RecordNo_int desc";
                        mySql += " order by a.INWD_ReceivedDate_dt desc";
                        break;
                    case "NDT":
                        mySql = " SELECT a.INWD_ReceivedDate_dt as dateofreceiving, 'Non Destructive' as record_type,a.INWD_Building_var, b.NDTINWD_ReferenceNo_var as refno ,b.NDTINWD_IssueDate_dt as status" +
                            " ,c.CONT_Name_var, a.INWD_ContactNo_var, '' as MFINWD_FinalIssueDt " +
                            " FROM tbl_Inward AS a, tbl_NDT_Inward AS b, tbl_Contact as c  " +
                                " WHERE a.INWD_RecordType_var = 'NDT' AND a.INWD_RecordType_var = b.NDTINWD_RecordType_var " +
                                " AND a.INWD_RecordNo_int = b.NDTINWD_RecordNo_int" +
                                " AND c.CONT_Id = a.INWD_CONT_Id";
                        if (bldg.Trim() != "" && bldg != "---All---")
                        {
                            mySql += " and a.INWD_Building_var='" + bldg + "'";
                        }
                        if (supplier.Trim() != "" && supplier != "---All---") //mf, cr, ndt not having suppiler name
                        {
                            mySql += " and a.INWD_RecordType_var <> 'NDT'";
                        }
                        mySql += "  and a.INWD_CL_Id= " + clId.ToString();
                        if (siteId != 0)
                        {
                            mySql = mySql + " and a.INWD_SITE_Id =" + siteId.ToString("0");
                        }
                        if (billId != "")
                        {
                            mySql = mySql + " and (a.INWD_BILL_Id = '" + billId + "'";
                            mySql = mySql + " Or (Select count(BILLD_BILL_Id) from tbl_BillDetail Where BILLD_ReferenceNo_int = a.INWD_ReferenceNo_int and BILLD_BILL_Id = '" + billId + "') > 0 ";
                            mySql = mySql + " Or (Select count(MBILLD_BILL_Id) from tbl_BillDetailMonthly Where MBILLD_ReferenceNo_int = a.INWD_ReferenceNo_int and MBILLD_BILL_Id = '" + billId + "') > 0 ";
                            mySql = mySql + ")";
                        }
                        mySql += otherStr.Replace("'AAC'", "'NDT'");
                        //mySql += " order by a.INWD_RecordType_var ,a.INWD_RecordNo_int desc";
                        mySql += " order by a.INWD_ReceivedDate_dt desc";
                        break;
                    case "CR":
                        mySql = " SELECT a.INWD_ReceivedDate_dt as dateofreceiving,'Core' as record_type,a.INWD_Building_var, b.CRINWD_ReferenceNo_var as refno ,b.CRINWD_IssueDate_dt as status" +
                            " ,c.CONT_Name_var, a.INWD_ContactNo_var, '' as MFINWD_FinalIssueDt " +
                            " FROM tbl_Inward AS a, tbl_Core_Inward AS b, tbl_Contact as c  " +
                                " WHERE a.INWD_RecordType_var = 'CR' AND a.INWD_RecordType_var = b.CRINWD_RecordType_var " +
                                " AND a.INWD_RecordNo_int = b.CRINWD_RecordNo_int" +
                                " AND c.CONT_Id = a.INWD_CONT_Id";
                        if (bldg.Trim() != "" && bldg != "---All---")
                        {
                            mySql += " and a.INWD_Building_var='" + bldg + "'";
                        }
                        if (supplier.Trim() != "" && supplier != "---All---") //mf, cr, ndt not having suppiler name
                        {
                            mySql += " and a.INWD_RecordType_var <> 'CR'";
                        }
                        mySql += "  and a.INWD_CL_Id= " + clId.ToString();
                        if (siteId != 0)
                        {
                            mySql = mySql + " and a.INWD_SITE_Id =" + siteId.ToString("0");
                        }
                        if (billId != "")
                        {
                            mySql = mySql + " and (a.INWD_BILL_Id = '" + billId + "'";
                            mySql = mySql + " Or (Select count(BILLD_BILL_Id) from tbl_BillDetail Where BILLD_ReferenceNo_int = a.INWD_ReferenceNo_int and BILLD_BILL_Id = '" + billId + "') > 0 ";
                            mySql = mySql + " Or (Select count(MBILLD_BILL_Id) from tbl_BillDetailMonthly Where MBILLD_ReferenceNo_int = a.INWD_ReferenceNo_int and MBILLD_BILL_Id = '" + billId + "') > 0 ";
                            mySql = mySql + ")";
                        }
                        mySql += otherStr.Replace("'AAC'", "'CR'");
                        //mySql += " order by a.INWD_RecordType_var ,a.INWD_RecordNo_int desc";
                        mySql += " order by a.INWD_ReceivedDate_dt desc";
                        break;
                    case "FLYASH":
                        if (testType == "---All---" || testType == "Physical")
                        {
                            mySql = " SELECT a.INWD_ReceivedDate_dt as dateofreceiving, 'FlyAsh' as record_type,a.INWD_Building_var, b.FLYASHINWD_ReferenceNo_var as refno ,b.FLYASHINWD_IssueDate_dt as status" +
                            " ,c.CONT_Name_var, a.INWD_ContactNo_var, '' as MFINWD_FinalIssueDt " +
                            " FROM tbl_Inward AS a, tbl_FlyAsh_Inward AS b, tbl_Contact as c  " +
                                " WHERE a.INWD_RecordType_var = 'FLYASH' AND a.INWD_RecordType_var = b.FLYASHINWD_RecordType_var " +
                                " AND a.INWD_RecordNo_int = b.FLYASHINWD_RecordNo_int" +
                                " AND c.CONT_Id = a.INWD_CONT_Id";
                            if (bldg.Trim() != "" && bldg != "---All---")
                            {
                                mySql += " and a.INWD_Building_var='" + bldg + "'";
                            }
                            if (supplier.Trim() != "" && supplier != "---All---")
                            {
                                mySql += " and b.FLYASHINWD_SupplierName_var='" + supplier + "'";
                            }
                            mySql += "  and a.INWD_CL_Id= " + clId.ToString();
                            if (siteId != 0)
                            {
                                mySql = mySql + " and a.INWD_SITE_Id =" + siteId.ToString("0");
                            }
                            if (billId != "")
                            {
                                mySql = mySql + " and (a.INWD_BILL_Id = '" + billId + "'";
                                mySql = mySql + " Or (Select count(BILLD_BILL_Id) from tbl_BillDetail Where BILLD_ReferenceNo_int = a.INWD_ReferenceNo_int and BILLD_BILL_Id = '" + billId + "') > 0 ";
                                mySql = mySql + " Or (Select count(MBILLD_BILL_Id) from tbl_BillDetailMonthly Where MBILLD_ReferenceNo_int = a.INWD_ReferenceNo_int and MBILLD_BILL_Id = '" + billId + "') > 0 ";
                                mySql = mySql + ")";
                            }
                            mySql += otherStr.Replace("'AAC'", "'FLYASH'");
                        }
                        if (testType == "---All---")
                        {
                            mySql += " Union ";
                        }
                        if (testType == "---All---" || testType == "Chemical")
                        {
                            mySql += otherStr.Replace("'AAC'", "'FLYASHCH'").Replace("Union", "");
                        }
                        //mySql += " order by a.INWD_RecordType_var ,a.INWD_RecordNo_int desc";
                        mySql += " order by a.INWD_ReceivedDate_dt desc";
                        break;
                    case "BT-":
                        mySql = " SELECT a.INWD_ReceivedDate_dt as dateofreceiving, 'Brick' as record_type,a.INWD_Building_var, b.BTINWD_ReferenceNo_var as refno ,b.BTINWD_IssueDate_dt as status" +
                            " ,c.CONT_Name_var, a.INWD_ContactNo_var, '' as MFINWD_FinalIssueDt " +
                            " FROM tbl_Inward AS a, tbl_Brick_Inward AS b, tbl_Contact as c  " +
                                " WHERE a.INWD_RecordType_var = 'BT-' AND a.INWD_RecordType_var = b.BTINWD_RecordType_var " +
                                " AND a.INWD_RecordNo_int = b.BTINWD_RecordNo_int" +
                                " AND c.CONT_Id = a.INWD_CONT_Id";
                        if (bldg.Trim() != "" && bldg != "---All---")
                        {
                            mySql += " and a.INWD_Building_var='" + bldg + "'";
                        }
                        if (supplier.Trim() != "" && supplier != "---All---")
                        {
                            mySql += " and b.BTINWD_SupplierName_var='" + supplier + "'";
                        }
                        mySql += "  and a.INWD_CL_Id= " + clId.ToString();
                        if (siteId != 0)
                        {
                            mySql = mySql + " and a.INWD_SITE_Id =" + siteId.ToString("0");
                        }
                        if (billId != "")
                        {
                            mySql = mySql + " and (a.INWD_BILL_Id = '" + billId + "'";
                            mySql = mySql + " Or (Select count(BILLD_BILL_Id) from tbl_BillDetail Where BILLD_ReferenceNo_int = a.INWD_ReferenceNo_int and BILLD_BILL_Id = '" + billId + "') > 0 ";
                            mySql = mySql + " Or (Select count(MBILLD_BILL_Id) from tbl_BillDetailMonthly Where MBILLD_ReferenceNo_int = a.INWD_ReferenceNo_int and MBILLD_BILL_Id = '" + billId + "') > 0 ";
                            mySql = mySql + ")";
                        }
                        mySql += otherStr.Replace("'AAC'", "'BT-'");
                        //mySql += " order by a.INWD_RecordType_var ,a.INWD_RecordNo_int desc";
                        mySql += " order by a.INWD_ReceivedDate_dt desc";
                        break;
                    case "PT":
                        mySql = " SELECT a.INWD_ReceivedDate_dt as dateofreceiving, 'Pavement Block' as record_type,a.INWD_Building_var, b.PTINWD_ReferenceNo_var as refno ,b.PTINWD_IssueDate_dt as status" +
                            " ,c.CONT_Name_var, a.INWD_ContactNo_var, '' as MFINWD_FinalIssueDt " +
                            " FROM tbl_Inward AS a, tbl_Pavement_Inward AS b, tbl_Contact as c  " +
                                " WHERE a.INWD_RecordType_var = 'PT' AND a.INWD_RecordType_var = b.PTINWD_RecordType_var " +
                                " AND a.INWD_RecordNo_int = b.PTINWD_RecordNo_int" +
                                " AND c.CONT_Id = a.INWD_CONT_Id";

                        if (bldg.Trim() != "" && bldg != "---All---")
                        {
                            mySql += " and a.INWD_Building_var='" + bldg + "'";
                        }
                        if (supplier.Trim() != "" && supplier != "---All---")
                        {
                            mySql += " and b.PTINWD_SupplierName_var='" + supplier + "'";
                        }
                        mySql += "  and a.INWD_CL_Id= " + clId.ToString();
                        if (siteId != 0)
                        {
                            mySql += " and a.INWD_SITE_Id =" + siteId.ToString("0");
                        }
                        if (billId != "")
                        {
                            mySql += " and (a.INWD_BILL_Id = '" + billId + "'";
                            mySql += " Or (Select count(BILLD_BILL_Id) from tbl_BillDetail Where BILLD_ReferenceNo_int = a.INWD_ReferenceNo_int and BILLD_BILL_Id = '" + billId + "') > 0 ";
                            mySql += " Or (Select count(MBILLD_BILL_Id) from tbl_BillDetailMonthly Where MBILLD_ReferenceNo_int = a.INWD_ReferenceNo_int and MBILLD_BILL_Id = '" + billId + "') > 0 ";
                            mySql += ")";
                        }
                        mySql += otherStr.Replace("'AAC'", "'PT'");
                        //mySql += " order by a.INWD_RecordType_var ,a.INWD_RecordNo_int desc";
                        mySql += " order by a.INWD_ReceivedDate_dt desc";
                        break;
                    case "SOLID":
                        if (testType == "---All---" || testType == "Concrete Blocks")
                        {
                            mySql = " SELECT a.INWD_ReceivedDate_dt as dateofreceiving, 'Masonary Block' as record_type,a.INWD_Building_var, b.SOLIDINWD_ReferenceNo_var as refno ,b.SOLIDINWD_IssueDate_dt  as status" +
                            " ,c.CONT_Name_var, a.INWD_ContactNo_var, '' as MFINWD_FinalIssueDt " +
                            " FROM tbl_Inward AS a, tbl_Solid_Inward AS b, tbl_Contact as c  " +
                                " WHERE a.INWD_RecordType_var = 'SOLID' AND a.INWD_RecordType_var = b.SOLIDINWD_RecordType_var " +
                                " AND a.INWD_RecordNo_int = b.SOLIDINWD_RecordNo_int" +
                                " AND c.CONT_Id = a.INWD_CONT_Id";
                            if (bldg.Trim() != "" && bldg != "---All---")
                            {
                                mySql += " and a.INWD_Building_var='" + bldg + "'";
                            }
                            if (supplier.Trim() != "" && supplier != "---All---")
                            {
                                mySql += " and b.SOLIDINWD_SupplierName_var='" + supplier + "'";
                            }
                            mySql += "  and a.INWD_CL_Id= " + clId.ToString();
                            if (siteId != 0)
                            {
                                mySql = mySql + " and a.INWD_SITE_Id =" + siteId.ToString("0");
                            }
                            if (billId != "")
                            {
                                mySql = mySql + " and (a.INWD_BILL_Id = '" + billId + "'";
                                mySql = mySql + " Or (Select count(BILLD_BILL_Id) from tbl_BillDetail Where BILLD_ReferenceNo_int = a.INWD_ReferenceNo_int and BILLD_BILL_Id = '" + billId + "') > 0 ";
                                mySql = mySql + " Or (Select count(MBILLD_BILL_Id) from tbl_BillDetailMonthly Where MBILLD_ReferenceNo_int = a.INWD_ReferenceNo_int and MBILLD_BILL_Id = '" + billId + "') > 0 ";
                                mySql = mySql + ")";
                            }
                            mySql += otherStr.Replace("'AAC'", "'SOLID'");
                        }
                        if (testType == "---All---")
                        {
                            mySql += " Union ";
                        }
                        if (testType == "---All---" || testType == "AAC Blocks")
                        {
                            mySql += " SELECT a.INWD_ReceivedDate_dt as dateofreceiving, 'AAC Block' as record_type,a.INWD_Building_var, b.AACINWD_ReferenceNo_var as refno ,b.AACINWD_IssueDate_dt as status" +
                            " ,c.CONT_Name_var, a.INWD_ContactNo_var, '' as MFINWD_FinalIssueDt " +
                            " FROM tbl_Inward AS a, tbl_AAC_Inward AS b, tbl_Contact as c  " +
                                " WHERE a.INWD_RecordType_var = 'AAC' AND a.INWD_RecordType_var = b.AACINWD_RecordType_var " +
                                " AND a.INWD_RecordNo_int = b.AACINWD_RecordNo_int" +
                                " AND c.CONT_Id = a.INWD_CONT_Id";
                            if (bldg.Trim() != "" && bldg != "---All---")
                            {
                                mySql += " and a.INWD_Building_var='" + bldg + "'";
                            }
                            if (supplier.Trim() != "" && supplier != "---All---")
                            {
                                mySql += " and b.AACINWD_SupplierName_var='" + supplier + "'";
                            }
                            mySql += "  and a.INWD_CL_Id= " + clId.ToString();
                            if (siteId != 0)
                            {
                                mySql = mySql + " and a.INWD_SITE_Id =" + siteId.ToString("0");
                            }
                            if (billId != "")
                            {
                                mySql = mySql + " and (a.INWD_BILL_Id = '" + billId + "'";
                                mySql = mySql + " Or (Select count(BILLD_BILL_Id) from tbl_BillDetail Where BILLD_ReferenceNo_int = a.INWD_ReferenceNo_int and BILLD_BILL_Id = '" + billId + "') > 0 ";
                                mySql = mySql + " Or (Select count(MBILLD_BILL_Id) from tbl_BillDetailMonthly Where MBILLD_ReferenceNo_int = a.INWD_ReferenceNo_int and MBILLD_BILL_Id = '" + billId + "') > 0 ";
                                mySql = mySql + ")";
                            }
                            mySql += otherStr.Replace("'AAC'", "'AAC'");
                        }
                        if (testType == "---All---")
                        {
                            mySql += " Union ";
                        }
                        if (testType == "---All---" || testType == "Bricks")
                        {
                            mySql += " SELECT a.INWD_ReceivedDate_dt as dateofreceiving, 'Brick' as record_type,a.INWD_Building_var, b.BTINWD_ReferenceNo_var as refno ,b.BTINWD_IssueDate_dt as status" +
                            " ,c.CONT_Name_var, a.INWD_ContactNo_var, '' as MFINWD_FinalIssueDt " +
                            " FROM tbl_Inward AS a, tbl_Brick_Inward AS b, tbl_Contact as c  " +
                                " WHERE a.INWD_RecordType_var = 'BT-' AND a.INWD_RecordType_var = b.BTINWD_RecordType_var " +
                                " AND a.INWD_RecordNo_int = b.BTINWD_RecordNo_int" +
                                " AND c.CONT_Id = a.INWD_CONT_Id";
                            if (bldg.Trim() != "" && bldg != "---All---")
                            {
                                mySql += " and a.INWD_Building_var='" + bldg + "'";
                            }
                            if (supplier.Trim() != "" && supplier != "---All---")
                            {
                                mySql += " and b.BTINWD_SupplierName_var='" + supplier + "'";
                            }
                            mySql += "  and a.INWD_CL_Id= " + clId.ToString();
                            if (siteId != 0)
                            {
                                mySql = mySql + " and a.INWD_SITE_Id =" + siteId.ToString("0");
                            }
                            if (billId != "")
                            {
                                mySql = mySql + " and (a.INWD_BILL_Id = '" + billId + "'";
                                mySql = mySql + " Or (Select count(BILLD_BILL_Id) from tbl_BillDetail Where BILLD_ReferenceNo_int = a.INWD_ReferenceNo_int and BILLD_BILL_Id = '" + billId + "') > 0 ";
                                mySql = mySql + " Or (Select count(MBILLD_BILL_Id) from tbl_BillDetailMonthly Where MBILLD_ReferenceNo_int = a.INWD_ReferenceNo_int and MBILLD_BILL_Id = '" + billId + "') > 0 ";
                                mySql = mySql + ")";
                            }
                            mySql += otherStr.Replace("'AAC'", "'BT-'");
                        }
                        //mySql += " order by a.INWD_RecordType_var ,a.INWD_RecordNo_int desc";
                        mySql += " order by a.INWD_ReceivedDate_dt desc";
                        break;
                    case "PILE":
                        mySql = " SELECT a.INWD_ReceivedDate_dt as dateofreceiving, 'Pile' as record_type,a.INWD_Building_var, b.PILEINWD_ReferenceNo_var as refno ,b.PILEINWD_IssueDate_dt as status" +
                            " ,c.CONT_Name_var, a.INWD_ContactNo_var, '' as MFINWD_FinalIssueDt " +
                            " FROM tbl_Inward AS a, tbl_Pile_Inward AS b, tbl_Contact as c  " +
                                " WHERE a.INWD_RecordType_var = 'PILE' AND a.INWD_RecordType_var = b.PILEINWD_RecordType_var " +
                                " AND a.INWD_RecordNo_int = b.PILEINWD_RecordNo_int" +
                                " AND c.CONT_Id = a.INWD_CONT_Id";
                        if (bldg.Trim() != "" && bldg != "---All---")
                        {
                            mySql += " and a.INWD_Building_var='" + bldg + "'";
                        }
                        if (supplier.Trim() != "" && supplier != "---All---")
                        {
                            mySql += " and b.PILEINWD_SupplierName_var='" + supplier + "'";
                        }
                        mySql += "  and a.INWD_CL_Id= " + clId.ToString();
                        if (siteId != 0)
                        {
                            mySql = mySql + " and a.INWD_SITE_Id =" + siteId.ToString("0");
                        }
                        if (billId != "")
                        {
                            mySql = mySql + " and (a.INWD_BILL_Id = '" + billId + "'";
                            mySql = mySql + " Or (Select count(BILLD_BILL_Id) from tbl_BillDetail Where BILLD_ReferenceNo_int = a.INWD_ReferenceNo_int and BILLD_BILL_Id = '" + billId + "') > 0 ";
                            mySql = mySql + " Or (Select count(MBILLD_BILL_Id) from tbl_BillDetailMonthly Where MBILLD_ReferenceNo_int = a.INWD_ReferenceNo_int and MBILLD_BILL_Id = '" + billId + "') > 0 ";
                            mySql = mySql + ")";
                        }
                        mySql += otherStr.Replace("'AAC'", "'PILE'");
                        //mySql += " order by a.INWD_RecordType_var ,a.INWD_RecordNo_int desc";
                        mySql += " order by a.INWD_ReceivedDate_dt desc";
                        break;
                    case "TILE":
                        if (testType == "---All---" || testType == "Physical")
                        {
                            mySql = " SELECT a.INWD_ReceivedDate_dt as dateofreceiving, 'Tile' as record_type,a.INWD_Building_var, b.TILEINWD_ReferenceNo_var as refno ,b.TILEINWD_IssueDate_dt as status" +
                            " ,c.CONT_Name_var, a.INWD_ContactNo_var, '' as MFINWD_FinalIssueDt " +
                            " FROM tbl_Inward AS a, tbl_Tile_Inward AS b, tbl_Contact as c  " +
                                " WHERE a.INWD_RecordType_var = 'TILE' AND a.INWD_RecordType_var = b.TILEINWD_RecordType_var " +
                                " AND a.INWD_RecordNo_int = b.TILEINWD_RecordNo_int" +
                                " AND c.CONT_Id = a.INWD_CONT_Id";
                            if (bldg.Trim() != "" && bldg != "---All---")
                            {
                                mySql += " and a.INWD_Building_var='" + bldg + "'";
                            }
                            if (supplier.Trim() != "" && supplier != "---All---")
                            {
                                mySql += " and b.TILEINWD_SupplierName_var='" + supplier + "'";
                            }
                            mySql += "  and a.INWD_CL_Id= " + clId.ToString();
                            if (siteId != 0)
                            {
                                mySql = mySql + " and a.INWD_SITE_Id =" + siteId.ToString("0");
                            }
                            if (billId != "")
                            {
                                mySql = mySql + " and (a.INWD_BILL_Id = '" + billId + "'";
                                mySql = mySql + " Or (Select count(BILLD_BILL_Id) from tbl_BillDetail Where BILLD_ReferenceNo_int = a.INWD_ReferenceNo_int and BILLD_BILL_Id = '" + billId + "') > 0 ";
                                mySql = mySql + " Or (Select count(MBILLD_BILL_Id) from tbl_BillDetailMonthly Where MBILLD_ReferenceNo_int = a.INWD_ReferenceNo_int and MBILLD_BILL_Id = '" + billId + "') > 0 ";
                                mySql = mySql + ")";
                            }
                            mySql += otherStr.Replace("'AAC'", "'TILE'");
                        }
                        if (testType == "---All---")
                        {
                            mySql += " Union ";
                        }
                        if (testType == "---All---" || testType == "Chemical")
                        {
                            mySql += otherStr.Replace("'AAC'", "'TILECH'").Replace("Union", "");
                        }
                        //mySql += " order by a.INWD_RecordType_var ,a.INWD_RecordNo_int desc";
                        mySql += " order by a.INWD_ReceivedDate_dt desc";
                        break;
                    case "STC":
                        mySql = " SELECT a.INWD_ReceivedDate_dt as dateofreceiving, 'Steel Chemical' as record_type,a.INWD_Building_var, b.STCINWD_ReferenceNo_var as refno ,b.STCINWD_IssueDate_dt as status" +
                            " ,c.CONT_Name_var, a.INWD_ContactNo_var, '' as MFINWD_FinalIssueDt " +
                            " FROM tbl_Inward AS a, tbl_SteelChemical_Inward AS b, tbl_Contact as c  " +
                                " WHERE a.INWD_RecordType_var = 'STC' AND a.INWD_RecordType_var = b.STCINWD_RecordType_var " +
                                " AND a.INWD_RecordNo_int = b.STCINWD_RecordNo_int" +
                                " AND c.CONT_Id = a.INWD_CONT_Id";
                        if (bldg.Trim() != "" && bldg != "---All---")
                        {
                            mySql += " and a.INWD_Building_var='" + bldg + "'";
                        }
                        if (supplier.Trim() != "" && supplier != "---All---")
                        {
                            mySql += " and b.STCINWD_SupplierName_var='" + supplier + "'";
                        }
                        mySql += "  and a.INWD_CL_Id= " + clId.ToString();
                        if (siteId != 0)
                        {
                            mySql = mySql + " and a.INWD_SITE_Id =" + siteId.ToString("0");
                        }
                        if (billId != "")
                        {
                            mySql = mySql + " and (a.INWD_BILL_Id = '" + billId + "'";
                            mySql = mySql + " Or (Select count(BILLD_BILL_Id) from tbl_BillDetail Where BILLD_ReferenceNo_int = a.INWD_ReferenceNo_int and BILLD_BILL_Id = '" + billId + "') > 0 ";
                            mySql = mySql + " Or (Select count(MBILLD_BILL_Id) from tbl_BillDetailMonthly Where MBILLD_ReferenceNo_int = a.INWD_ReferenceNo_int and MBILLD_BILL_Id = '" + billId + "') > 0 ";
                            mySql = mySql + ")";
                        }
                        mySql += otherStr.Replace("'AAC'", "'STC'");
                        //mySql += " order by a.INWD_RecordType_var ,a.INWD_RecordNo_int desc";
                        mySql += " order by a.INWD_ReceivedDate_dt desc";
                        break;
                    case "CCH":
                        mySql = " SELECT a.INWD_ReceivedDate_dt as dateofreceiving, 'Cement Chemical' as record_type,a.INWD_Building_var, b.CCHINWD_ReferenceNo_var as refno ,b.CCHINWD_IssueDate_dt as status" +
                            " ,c.CONT_Name_var, a.INWD_ContactNo_var, '' as MFINWD_FinalIssueDt " +
                            " FROM tbl_Inward AS a, tbl_CementChemical_Inward AS b, tbl_Contact as c  " +
                                " WHERE a.INWD_RecordType_var = 'CCH' AND a.INWD_RecordType_var = b.CCHINWD_RecordType_var " +
                                " AND a.INWD_RecordNo_int = b.CCHINWD_RecordNo_int" +
                                " AND c.CONT_Id = a.INWD_CONT_Id";
                        if (bldg.Trim() != "" && bldg != "---All---")
                        {
                            mySql += " and a.INWD_Building_var='" + bldg + "'";
                        }
                        if (supplier.Trim() != "" && supplier != "---All---")
                        {
                            mySql += " and b.CCHINWD_SupplierName_var='" + supplier + "'";
                        }
                        mySql += "  and a.INWD_CL_Id= " + clId.ToString();
                        if (siteId != 0)
                        {
                            mySql = mySql + " and a.INWD_SITE_Id =" + siteId.ToString("0");
                        }
                        if (billId != "")
                        {
                            mySql = mySql + " and (a.INWD_BILL_Id = '" + billId + "'";
                            mySql = mySql + " Or (Select count(BILLD_BILL_Id) from tbl_BillDetail Where BILLD_ReferenceNo_int = a.INWD_ReferenceNo_int and BILLD_BILL_Id = '" + billId + "') > 0 ";
                            mySql = mySql + " Or (Select count(MBILLD_BILL_Id) from tbl_BillDetailMonthly Where MBILLD_ReferenceNo_int = a.INWD_ReferenceNo_int and MBILLD_BILL_Id = '" + billId + "') > 0 ";
                            mySql = mySql + ")";
                        }
                        mySql += otherStr.Replace("'AAC'", "'CCH'");
                        //mySql += " order by a.INWD_RecordType_var ,a.INWD_RecordNo_int desc";
                        mySql += " order by a.INWD_ReceivedDate_dt desc";
                        break;
                    case "WT":
                        mySql = " SELECT a.INWD_ReceivedDate_dt as dateofreceiving,'Water' as record_type,a.INWD_Building_var, b.WTINWD_ReferenceNo_var as refno ,b.WTINWD_IssueDate_dt as status" +
                            " ,c.CONT_Name_var, a.INWD_ContactNo_var, '' as MFINWD_FinalIssueDt " +
                            " FROM tbl_Inward AS a, tbl_Water_Inward AS b, tbl_Contact as c  " +
                                " WHERE a.INWD_RecordType_var = 'WT' AND a.INWD_RecordType_var = b.WTINWD_RecordType_var " +
                                " AND a.INWD_RecordNo_int = b.WTINWD_RecordNo_int" +
                                " AND c.CONT_Id = a.INWD_CONT_Id";
                        if (bldg.Trim() != "" && bldg != "---All---")
                        {
                            mySql += " and a.INWD_Building_var='" + bldg + "'";
                        }
                        if (supplier.Trim() != "" && supplier != "---All---")
                        {
                            mySql += " and b.WTINWD_SupplierName_var='" + supplier + "'";
                        }
                        mySql += "  and a.INWD_CL_Id= " + clId.ToString();
                        if (siteId != 0)
                        {
                            mySql = mySql + " and a.INWD_SITE_Id =" + siteId.ToString("0");
                        }
                        if (billId != "")
                        {
                            mySql = mySql + " and (a.INWD_BILL_Id = '" + billId + "'";
                            mySql = mySql + " Or (Select count(BILLD_BILL_Id) from tbl_BillDetail Where BILLD_ReferenceNo_int = a.INWD_ReferenceNo_int and BILLD_BILL_Id = '" + billId + "') > 0 ";
                            mySql = mySql + " Or (Select count(MBILLD_BILL_Id) from tbl_BillDetailMonthly Where MBILLD_ReferenceNo_int = a.INWD_ReferenceNo_int and MBILLD_BILL_Id = '" + billId + "') > 0 ";
                            mySql = mySql + ")";
                        }
                        mySql += otherStr.Replace("'AAC'", "'WT'");
                        //mySql += " order by a.INWD_RecordType_var ,a.INWD_RecordNo_int desc";
                        mySql += " order by a.INWD_ReceivedDate_dt desc";
                        break;
                    case "SO":
                        mySql = "SELECT a.INWD_ReceivedDate_dt as dateofreceiving,'Soil' as record_type," +
                        " a.INWD_Building_var, b.SOINWD_ReferenceNo_var as refno , b.SOINWD_IssueDate_dt as status " +
                        " ,c.CONT_Name_var, a.INWD_ContactNo_var, '' as MFINWD_FinalIssueDt " +
                        " FROM tbl_Inward AS a, tbl_Soil_Inward AS b, tbl_Contact as c " +
                        " WHERE a.INWD_RecordType_var = 'SO'  AND a.INWD_RecordType_var = b.SOINWD_RecordType_var AND a.INWD_RecordNo_int = b.SOINWD_RecordNo_int " +
                        " AND c.CONT_Id = a.INWD_CONT_Id";

                        if (bldg.Trim() != "" && bldg != "---All---")
                        {
                            mySql += " and a.INWD_Building_var='" + bldg + "'";
                        }
                        if (supplier.Trim() != "" && supplier != "---All---")
                        {
                            mySql += " and b.SOINWD_SupplierName_var='" + supplier + "'";
                        }
                        mySql += "  and a.INWD_CL_Id= " + clId.ToString();
                        if (siteId != 0)
                        {
                            mySql = mySql + " and a.INWD_SITE_Id =" + siteId.ToString("0");
                        }
                        if (billId != "")
                        {
                            mySql = mySql + " and (a.INWD_BILL_Id = '" + billId + "'";
                            mySql = mySql + " Or (Select count(BILLD_BILL_Id) from tbl_BillDetail Where BILLD_ReferenceNo_int = a.INWD_ReferenceNo_int and BILLD_BILL_Id = '" + billId + "') > 0 ";
                            mySql = mySql + " Or (Select count(MBILLD_BILL_Id) from tbl_BillDetailMonthly Where MBILLD_ReferenceNo_int = a.INWD_ReferenceNo_int and MBILLD_BILL_Id = '" + billId + "') > 0 ";
                            mySql = mySql + ")";
                        }
                        mySql += otherStr.Replace("'AAC'", "'SO'");
                        //mySql += " order by a.INWD_RecordType_var ,a.INWD_RecordNo_int desc";
                        mySql += " order by a.INWD_ReceivedDate_dt desc";
                        break;
                    case "OT":
                        mySql = otherStr.Replace("and b.OTINWD_Section_var = 'AAC'", "").Replace("Union", "");
                        mySql += " order by a.INWD_ReceivedDate_dt desc";
                        break;
                    default:
                        mySql = "";
                        break;
                }
                #endregion

                if (mySql != "")
                {
                    SqlDataAdapter da = new SqlDataAdapter(mySql, cn);
                    ds.Reset();
                    da.Fill(ds);
                    dt1 = new DataTable();
                    dt1 = ds.Tables[0];
                    dtReturn.Merge(dt1);
                }
            }
            return dtReturn;
        }
        catch (Exception e)
        {
            throw e;
        }

    }

    public DataTable getReportList2016_Old(double clId, double siteId, string supplier, string bldg, string rType, string db, string billId, string testType)
    {
        string mySql;
        try
        {
            DataSet ds = new DataSet();
            DataTable dtReturn = new DataTable();
            string selRecType = rType;
            mySql = "select distinct(a.INWD_RecordType_var) from tbl_Inward as a where ";
            if (rType != "---All---")
            {
                if (rType == "CEMT")
                {
                    mySql = mySql + " (a.INWD_RecordType_var='" + rType + "' or a.INWD_RecordType_var='" + "CCH" + "') and ";
                }
                else if (rType == "ST")
                {
                    mySql = mySql + " (a.INWD_RecordType_var='" + rType + "' or a.INWD_RecordType_var='" + "STC" + "') and ";
                }
                else if (rType == "FLYASH" || rType == "TILE")
                {
                    mySql = mySql + " (a.INWD_RecordType_var='" + rType + "' or a.INWD_RecordType_var='" + "OT" + "') and ";
                }
                else if (rType == "SOLID")
                {
                    mySql = mySql + " (a.INWD_RecordType_var='" + rType + "' or a.INWD_RecordType_var='" + "AAC" + "' or a.INWD_RecordType_var='" + "BT-" + "') and ";
                }
                else
                {
                    mySql = mySql + " a.INWD_RecordType_var='" + rType + "' and ";
                }
            }
            if (bldg.ToString() != "---All---")
            {
                mySql = mySql + " a.INWD_Building_var='" + bldg + "' and ";
            }

            mySql = mySql + "  a.INWD_CL_Id= " + clId.ToString("0");
            if (siteId != 0)
            {
                mySql = mySql + " and a.INWD_SITE_Id =" + siteId.ToString("0");
            }
            if (billId != "")
            {
                mySql = mySql + " and (a.INWD_BILL_Id = '" + billId + "'";
                mySql = mySql + " Or (Select count(BILLD_BILL_Id) from tbl_BillDetail Where BILLD_ReferenceNo_int = a.INWD_ReferenceNo_int and BILLD_BILL_Id = '" + billId + "') > 0 ";
                mySql = mySql + " Or (Select count(MBILLD_BILL_Id) from tbl_BillDetailMonthly Where MBILLD_ReferenceNo_int = a.INWD_ReferenceNo_int and MBILLD_BILL_Id = '" + billId + "') > 0 ";
                mySql = mySql + ")";
            }
            #region  other report
            string otherStr = @" Union SELECT a.INWD_ReceivedDate_dt as dateofreceiving, 'Other' as record_type, a.INWD_Building_var, 
                              b.OTINWD_ReferenceNo_var as refno, b.OTINWD_IssueDate_dt as status, c.CONT_Name_var, a.INWD_ContactNo_var, '' as MFINWD_FinalIssueDt  
                              FROM tbl_Inward AS a, tbl_Other_Inward AS b, tbl_Contact as c   
                              WHERE a.INWD_RecordType_var = 'OT' AND a.INWD_RecordType_var = b.OTINWD_RecordType_var  
                              AND a.INWD_RecordNo_int = b.OTINWD_RecordNo_int AND c.CONT_Id = a.INWD_CONT_Id ";
            otherStr += "and b.OTINWD_Section_var = 'AAC'";
            if (bldg.Trim() != "" && bldg != "---All---")
            {
                otherStr += " and a.INWD_Building_var='" + bldg + "'";
            }
            if (supplier.Trim() != "" && supplier != "---All---")
            {
                otherStr += " and b.OTINWD_SupplierName_var='" + supplier + "'";
            }
            otherStr += "  and a.INWD_CL_Id= " + clId.ToString();
            if (siteId != 0)
            {
                otherStr += " and a.INWD_SITE_Id =" + siteId.ToString("0");
            }
            if (billId != "")
            {
                otherStr += " and (a.INWD_BILL_Id = '" + billId + "'";
                otherStr += " Or (Select count(BILLD_BILL_Id) from tbl_BillDetail Where BILLD_ReferenceNo_int = a.INWD_ReferenceNo_int and BILLD_BILL_Id = '" + billId + "') > 0 ";
                otherStr += " Or (Select count(MBILLD_BILL_Id) from tbl_BillDetailMonthly Where MBILLD_ReferenceNo_int = a.INWD_ReferenceNo_int and MBILLD_BILL_Id = '" + billId + "') > 0 ";
                otherStr += ")";
            }
            #endregion
            DataTable dt = getGeneralData(mySql, db);
            DataTable dt1 = new DataTable();
            foreach (DataRow rw in dt.Rows)
            {
                rType = rw["INWD_RecordType_var"].ToString();
                switch (rType)
                {
                    case "CT":
                        mySql = " SELECT a.INWD_ReceivedDate_dt as dateofreceiving, 'Cube' as record_type,a.INWD_Building_var, b.CTINWD_ReferenceNo_var as refno ,b.CTINWD_IssueDate_dt as status" +
                                " ,c.CONT_Name_var, a.INWD_ContactNo_var, '' as MFINWD_FinalIssueDt " +
                                " FROM tbl_Inward AS a, tbl_Cube_Inward AS b, tbl_Contact as c " +
                                " WHERE a.INWD_RecordType_var = 'CT' AND a.INWD_RecordType_var = b.CTINWD_RecordType_var " +
                                " AND a.INWD_RecordNo_int = b.CTINWD_RecordNo_int" +
                                " AND c.CONT_Id = a.INWD_CONT_Id";
                        if (bldg.Trim() != "" && bldg != "---All---")
                        {
                            mySql += " and a.INWD_Building_var='" + bldg + "'";
                        }
                        if (supplier.Trim() != "" && supplier != "---All---")
                        {
                            mySql += " and b.CTINWD_SupplierName_var='" + supplier + "'";
                        }
                        mySql += "  and a.INWD_CL_Id= " + clId.ToString();
                        if (siteId != 0)
                        {
                            mySql = mySql + " and a.INWD_SITE_Id =" + siteId.ToString("0");
                        }
                        if (billId != "")
                        {
                            mySql = mySql + " and (a.INWD_BILL_Id = '" + billId + "'";
                            mySql = mySql + " Or (Select count(BILLD_BILL_Id) from tbl_BillDetail Where BILLD_ReferenceNo_int = a.INWD_ReferenceNo_int and BILLD_BILL_Id = '" + billId + "') > 0 ";
                            mySql = mySql + " Or (Select count(MBILLD_BILL_Id) from tbl_BillDetailMonthly Where MBILLD_ReferenceNo_int = a.INWD_ReferenceNo_int and MBILLD_BILL_Id = '" + billId + "') > 0 ";
                            mySql = mySql + ")";
                        }
                        mySql += otherStr.Replace("'AAC'", "'CT'");
                        //mySql += " order by a.INWD_RecordType_var ,a.INWD_RecordNo_int desc";
                        mySql += " order by a.INWD_ReceivedDate_dt desc";
                        break;
                    case "ST":
                        if (testType == "---All---" || testType == "Physical")
                        {
                            mySql = " SELECT a.INWD_ReceivedDate_dt as dateofreceiving, 'Steel' as record_type,a.INWD_Building_var, b.STINWD_ReferenceNo_var as refno ,b.STINWD_IssueDate_dt as status" +
                                    " ,c.CONT_Name_var, a.INWD_ContactNo_var, '' as MFINWD_FinalIssueDt " +
                                    " FROM tbl_Inward AS a, tbl_Steel_Inward AS b, tbl_Contact as c " +
                                    " WHERE a.INWD_RecordType_var = 'ST' AND a.INWD_RecordType_var = b.STINWD_RecordType_var " +
                                    " AND a.INWD_RecordNo_int = b.STINWD_RecordNo_int" +
                                    " AND c.CONT_Id = a.INWD_CONT_Id";
                            if (bldg.Trim() != "" && bldg != "---All---")
                            {
                                mySql += " and a.INWD_Building_var='" + bldg + "'";
                            }
                            if (supplier.Trim() != "" && supplier != "---All---")
                            {
                                mySql += " and b.STINWD_SupplierName_var='" + supplier + "'";
                            }
                            mySql += "  and a.INWD_CL_Id= " + clId.ToString();
                            if (siteId != 0)
                            {
                                mySql = mySql + " and a.INWD_SITE_Id =" + siteId.ToString("0");
                            }
                            if (billId != "")
                            {
                                mySql = mySql + " and (a.INWD_BILL_Id = '" + billId + "'";
                                mySql = mySql + " Or (Select count(BILLD_BILL_Id) from tbl_BillDetail Where BILLD_ReferenceNo_int = a.INWD_ReferenceNo_int and BILLD_BILL_Id = '" + billId + "') > 0 ";
                                mySql = mySql + " Or (Select count(MBILLD_BILL_Id) from tbl_BillDetailMonthly Where MBILLD_ReferenceNo_int = a.INWD_ReferenceNo_int and MBILLD_BILL_Id = '" + billId + "') > 0 ";
                                mySql = mySql + ")";
                            }
                            mySql += otherStr.Replace("'AAC'", "'ST'");
                        }
                        if (testType == "---All---")
                        {
                            mySql += " Union ";
                        }
                        if (testType == "---All---" || testType == "Chemical")
                        {
                            mySql = " SELECT a.INWD_ReceivedDate_dt as dateofreceiving, 'Steel Chemical' as record_type,a.INWD_Building_var, b.STCINWD_ReferenceNo_var as refno ,b.STCINWD_IssueDate_dt as status" +
                            " ,c.CONT_Name_var, a.INWD_ContactNo_var, '' as MFINWD_FinalIssueDt " +
                            " FROM tbl_Inward AS a, tbl_SteelChemical_Inward AS b, tbl_Contact as c  " +
                                " WHERE a.INWD_RecordType_var = 'STC' AND a.INWD_RecordType_var = b.STCINWD_RecordType_var " +
                                " AND a.INWD_RecordNo_int = b.STCINWD_RecordNo_int" +
                                " AND c.CONT_Id = a.INWD_CONT_Id";
                            if (bldg.Trim() != "" && bldg != "---All---")
                            {
                                mySql += " and a.INWD_Building_var='" + bldg + "'";
                            }
                            if (supplier.Trim() != "" && supplier != "---All---")
                            {
                                mySql += " and b.STCINWD_SupplierName_var='" + supplier + "'";
                            }
                            mySql += "  and a.INWD_CL_Id= " + clId.ToString();
                            if (siteId != 0)
                            {
                                mySql = mySql + " and a.INWD_SITE_Id =" + siteId.ToString("0");
                            }
                            if (billId != "")
                            {
                                mySql = mySql + " and (a.INWD_BILL_Id = '" + billId + "'";
                                mySql = mySql + " Or (Select count(BILLD_BILL_Id) from tbl_BillDetail Where BILLD_ReferenceNo_int = a.INWD_ReferenceNo_int and BILLD_BILL_Id = '" + billId + "') > 0 ";
                                mySql = mySql + " Or (Select count(MBILLD_BILL_Id) from tbl_BillDetailMonthly Where MBILLD_ReferenceNo_int = a.INWD_ReferenceNo_int and MBILLD_BILL_Id = '" + billId + "') > 0 ";
                                mySql = mySql + ")";
                            }
                            mySql += otherStr.Replace("'AAC'", "'STC'");
                        }
                        //mySql += " order by a.INWD_RecordType_var ,a.INWD_RecordNo_int desc";
                        mySql += " order by a.INWD_ReceivedDate_dt desc";
                        break;
                    case "MF":
                        mySql = " SELECT a.INWD_ReceivedDate_dt as dateofreceiving,'Mix Design' as record_type,a.INWD_Building_var, b.MFINWD_ReferenceNo_var as refno ,b.MFINWD_MDLIssueDt as status" +
                            " ,c.CONT_Name_var, a.INWD_ContactNo_var, CONVERT(varchar,b.MFINWD_FinalIssueDt) as MFINWD_FinalIssueDt " +
                            " FROM tbl_Inward AS a, tbl_MixDesign_Inward AS b , tbl_Contact as c " +
                                " WHERE a.INWD_RecordType_var = 'MF' AND a.INWD_RecordType_var = b.MFINWD_RecordType_var " +
                                " AND a.INWD_RecordNo_int = b.MFINWD_RecordNo_int" +
                                " AND c.CONT_Id = a.INWD_CONT_Id";
                        if (bldg.Trim() != "" && bldg != "---All---")
                        {
                            mySql += " and a.INWD_Building_var='" + bldg + "'";
                        }
                        if (supplier.Trim() != "" && supplier != "---All---") //mf, cr, ndt not having suppiler name
                        {
                            mySql += " and a.INWD_RecordType_var <> 'MF'";
                        }

                        mySql += "  and a.INWD_CL_Id= " + clId.ToString();
                        if (siteId != 0)
                        {
                            mySql = mySql + " and a.INWD_SITE_Id =" + siteId.ToString("0");
                        }
                        if (billId != "")
                        {
                            mySql = mySql + " and (a.INWD_BILL_Id = '" + billId + "'";
                            mySql = mySql + " Or (Select count(BILLD_BILL_Id) from tbl_BillDetail Where BILLD_ReferenceNo_int = a.INWD_ReferenceNo_int and BILLD_BILL_Id = '" + billId + "') > 0 ";
                            mySql = mySql + " Or (Select count(MBILLD_BILL_Id) from tbl_BillDetailMonthly Where MBILLD_ReferenceNo_int = a.INWD_ReferenceNo_int and MBILLD_BILL_Id = '" + billId + "') > 0 ";
                            mySql = mySql + ")";
                        }
                        mySql += otherStr.Replace("'AAC'", "'MF'");
                        //mySql += " order by a.INWD_RecordType_var ,a.INWD_RecordNo_int desc";
                        mySql += " order by a.INWD_ReceivedDate_dt desc";
                        break;
                    case "CEMT":
                        if (testType == "---All---" || testType == "Physical")
                        {
                            mySql = " SELECT a.INWD_ReceivedDate_dt as dateofreceiving, 'Cement' as record_type,a.INWD_Building_var, b.CEMTINWD_ReferenceNo_var as refno ,b.CEMTINWD_IssueDate_dt as status" +
                            " ,c.CONT_Name_var, a.INWD_ContactNo_var, '' as MFINWD_FinalIssueDt " +
                            " FROM tbl_Inward AS a, tbl_Cement_Inward AS b, tbl_Contact as c  " +
                                " WHERE a.INWD_RecordType_var = 'CEMT' AND a.INWD_RecordType_var = b.CEMTINWD_RecordType_var " +
                                " AND a.INWD_RecordNo_int = b.CEMTINWD_RecordNo_int" +
                                " AND c.CONT_Id = a.INWD_CONT_Id";
                            if (bldg.Trim() != "" && bldg != "---All---")
                            {
                                mySql += " and a.INWD_Building_var='" + bldg + "'";
                            }
                            if (supplier.Trim() != "" && supplier != "---All---")
                            {
                                mySql += " and b.CEMTINWD_SupplierName_var='" + supplier + "'";
                            }
                            mySql += "  and a.INWD_CL_Id= " + clId.ToString();
                            if (siteId != 0)
                            {
                                mySql = mySql + " and a.INWD_SITE_Id =" + siteId.ToString("0");
                            }
                            if (billId != "")
                            {
                                mySql = mySql + " and (a.INWD_BILL_Id = '" + billId + "'";
                                mySql = mySql + " Or (Select count(BILLD_BILL_Id) from tbl_BillDetail Where BILLD_ReferenceNo_int = a.INWD_ReferenceNo_int and BILLD_BILL_Id = '" + billId + "') > 0 ";
                                mySql = mySql + " Or (Select count(MBILLD_BILL_Id) from tbl_BillDetailMonthly Where MBILLD_ReferenceNo_int = a.INWD_ReferenceNo_int and MBILLD_BILL_Id = '" + billId + "') > 0 ";
                                mySql = mySql + ")";
                            }
                            mySql += otherStr.Replace("'AAC'", "'CEMT'");
                        }
                        if (testType == "---All---")
                        {
                            mySql += " Union ";
                        }
                        if (testType == "---All---" || testType == "Chemical")
                        {
                            mySql = " SELECT a.INWD_ReceivedDate_dt as dateofreceiving, 'Cement Chemical' as record_type,a.INWD_Building_var, b.CCHINWD_ReferenceNo_var as refno ,b.CCHINWD_IssueDate_dt as status" +
                            " ,c.CONT_Name_var, a.INWD_ContactNo_var, '' as MFINWD_FinalIssueDt " +
                            " FROM tbl_Inward AS a, tbl_CementChemical_Inward AS b, tbl_Contact as c  " +
                                " WHERE a.INWD_RecordType_var = 'CCH' AND a.INWD_RecordType_var = b.CCHINWD_RecordType_var " +
                                " AND a.INWD_RecordNo_int = b.CCHINWD_RecordNo_int" +
                                " AND c.CONT_Id = a.INWD_CONT_Id";
                            if (bldg.Trim() != "" && bldg != "---All---")
                            {
                                mySql += " and a.INWD_Building_var='" + bldg + "'";
                            }
                            if (supplier.Trim() != "" && supplier != "---All---")
                            {
                                mySql += " and b.CCHINWD_SupplierName_var='" + supplier + "'";
                            }
                            mySql += "  and a.INWD_CL_Id= " + clId.ToString();
                            if (siteId != 0)
                            {
                                mySql = mySql + " and a.INWD_SITE_Id =" + siteId.ToString("0");
                            }
                            if (billId != "")
                            {
                                mySql = mySql + " and (a.INWD_BILL_Id = '" + billId + "'";
                                mySql = mySql + " Or (Select count(BILLD_BILL_Id) from tbl_BillDetail Where BILLD_ReferenceNo_int = a.INWD_ReferenceNo_int and BILLD_BILL_Id = '" + billId + "') > 0 ";
                                mySql = mySql + " Or (Select count(MBILLD_BILL_Id) from tbl_BillDetailMonthly Where MBILLD_ReferenceNo_int = a.INWD_ReferenceNo_int and MBILLD_BILL_Id = '" + billId + "') > 0 ";
                                mySql = mySql + ")";
                            }
                            mySql += otherStr.Replace("'AAC'", "'CCH'");
                        }
                        //mySql += " order by a.INWD_RecordType_var ,a.INWD_RecordNo_int desc";
                        mySql += " order by a.INWD_ReceivedDate_dt desc";
                        break;
                    case "AGGT":
                        mySql = " SELECT a.INWD_ReceivedDate_dt as dateofreceiving, 'Aggregate' as record_type,a.INWD_Building_var, b.AGGTINWD_ReferenceNo_var as refno ,b.AGGTINWD_IssueDate_dt as status" +
                            " ,c.CONT_Name_var, a.INWD_ContactNo_var, '' as MFINWD_FinalIssueDt " +
                            " FROM tbl_Inward AS a, tbl_Aggregate_Inward AS b, tbl_Contact as c  " +
                                " WHERE a.INWD_RecordType_var = 'AGGT' AND a.INWD_RecordType_var = b.AGGTINWD_RecordType_var " +
                                " AND a.INWD_RecordNo_int = b.AGGTINWD_RecordNo_int" +
                                " AND c.CONT_Id = a.INWD_CONT_Id";
                        if (bldg.Trim() != "" && bldg != "---All---")
                        {
                            mySql += " and a.INWD_Building_var='" + bldg + "'";
                        }
                        if (supplier.Trim() != "" && supplier != "---All---")
                        {
                            mySql += " and b.AGGTINWD_SupplierName_var='" + supplier + "'";
                        }
                        mySql += "  and a.INWD_CL_Id= " + clId.ToString();
                        if (siteId != 0)
                        {
                            mySql = mySql + " and a.INWD_SITE_Id =" + siteId.ToString("0");
                        }
                        if (billId != "")
                        {
                            mySql = mySql + " and (a.INWD_BILL_Id = '" + billId + "'";
                            mySql = mySql + " Or (Select count(BILLD_BILL_Id) from tbl_BillDetail Where BILLD_ReferenceNo_int = a.INWD_ReferenceNo_int and BILLD_BILL_Id = '" + billId + "') > 0 ";
                            mySql = mySql + " Or (Select count(MBILLD_BILL_Id) from tbl_BillDetailMonthly Where MBILLD_ReferenceNo_int = a.INWD_ReferenceNo_int and MBILLD_BILL_Id = '" + billId + "') > 0 ";
                            mySql = mySql + ")";
                        }
                        mySql += otherStr.Replace("'AAC'", "'AGGT'");
                        //mySql += " order by a.INWD_RecordType_var ,a.INWD_RecordNo_int desc";
                        mySql += " order by a.INWD_ReceivedDate_dt desc";
                        break;
                    case "AAC":
                        mySql = " SELECT a.INWD_ReceivedDate_dt as dateofreceiving, 'AAC Block' as record_type,a.INWD_Building_var, b.AACINWD_ReferenceNo_var as refno ,b.AACINWD_IssueDate_dt as status" +
                            " ,c.CONT_Name_var, a.INWD_ContactNo_var, '' as MFINWD_FinalIssueDt " +
                            " FROM tbl_Inward AS a, tbl_AAC_Inward AS b, tbl_Contact as c  " +
                                " WHERE a.INWD_RecordType_var = 'AAC' AND a.INWD_RecordType_var = b.AACINWD_RecordType_var " +
                                " AND a.INWD_RecordNo_int = b.AACINWD_RecordNo_int" +
                                " AND c.CONT_Id = a.INWD_CONT_Id";
                        if (bldg.Trim() != "" && bldg != "---All---")
                        {
                            mySql += " and a.INWD_Building_var='" + bldg + "'";
                        }
                        if (supplier.Trim() != "" && supplier != "---All---")
                        {
                            mySql += " and b.AACINWD_SupplierName_var='" + supplier + "'";
                        }
                        mySql += "  and a.INWD_CL_Id= " + clId.ToString();
                        if (siteId != 0)
                        {
                            mySql = mySql + " and a.INWD_SITE_Id =" + siteId.ToString("0");
                        }
                        if (billId != "")
                        {
                            mySql = mySql + " and (a.INWD_BILL_Id = '" + billId + "'";
                            mySql = mySql + " Or (Select count(BILLD_BILL_Id) from tbl_BillDetail Where BILLD_ReferenceNo_int = a.INWD_ReferenceNo_int and BILLD_BILL_Id = '" + billId + "') > 0 ";
                            mySql = mySql + " Or (Select count(MBILLD_BILL_Id) from tbl_BillDetailMonthly Where MBILLD_ReferenceNo_int = a.INWD_ReferenceNo_int and MBILLD_BILL_Id = '" + billId + "') > 0 ";
                            mySql = mySql + ")";
                        }
                        mySql += otherStr.Replace("'AAC'", "'AAC'");
                        //mySql += " order by a.INWD_RecordType_var ,a.INWD_RecordNo_int desc";
                        mySql += " order by a.INWD_ReceivedDate_dt desc";
                        break;
                    case "NDT":
                        mySql = " SELECT a.INWD_ReceivedDate_dt as dateofreceiving, 'Non Destructive' as record_type,a.INWD_Building_var, b.NDTINWD_ReferenceNo_var as refno ,b.NDTINWD_IssueDate_dt as status" +
                            " ,c.CONT_Name_var, a.INWD_ContactNo_var, '' as MFINWD_FinalIssueDt " +
                            " FROM tbl_Inward AS a, tbl_NDT_Inward AS b, tbl_Contact as c  " +
                                " WHERE a.INWD_RecordType_var = 'NDT' AND a.INWD_RecordType_var = b.NDTINWD_RecordType_var " +
                                " AND a.INWD_RecordNo_int = b.NDTINWD_RecordNo_int" +
                                " AND c.CONT_Id = a.INWD_CONT_Id";
                        if (bldg.Trim() != "" && bldg != "---All---")
                        {
                            mySql += " and a.INWD_Building_var='" + bldg + "'";
                        }
                        if (supplier.Trim() != "" && supplier != "---All---") //mf, cr, ndt not having suppiler name
                        {
                            mySql += " and a.INWD_RecordType_var <> 'NDT'";
                        }
                        mySql += "  and a.INWD_CL_Id= " + clId.ToString();
                        if (siteId != 0)
                        {
                            mySql = mySql + " and a.INWD_SITE_Id =" + siteId.ToString("0");
                        }
                        if (billId != "")
                        {
                            mySql = mySql + " and (a.INWD_BILL_Id = '" + billId + "'";
                            mySql = mySql + " Or (Select count(BILLD_BILL_Id) from tbl_BillDetail Where BILLD_ReferenceNo_int = a.INWD_ReferenceNo_int and BILLD_BILL_Id = '" + billId + "') > 0 ";
                            mySql = mySql + " Or (Select count(MBILLD_BILL_Id) from tbl_BillDetailMonthly Where MBILLD_ReferenceNo_int = a.INWD_ReferenceNo_int and MBILLD_BILL_Id = '" + billId + "') > 0 ";
                            mySql = mySql + ")";
                        }
                        mySql += otherStr.Replace("'AAC'", "'NDT'");
                        //mySql += " order by a.INWD_RecordType_var ,a.INWD_RecordNo_int desc";
                        mySql += " order by a.INWD_ReceivedDate_dt desc";
                        break;
                    case "CR":
                        mySql = " SELECT a.INWD_ReceivedDate_dt as dateofreceiving,'Core' as record_type,a.INWD_Building_var, b.CRINWD_ReferenceNo_var as refno ,b.CRINWD_IssueDate_dt as status" +
                            " ,c.CONT_Name_var, a.INWD_ContactNo_var, '' as MFINWD_FinalIssueDt " +
                            " FROM tbl_Inward AS a, tbl_Core_Inward AS b, tbl_Contact as c  " +
                                " WHERE a.INWD_RecordType_var = 'CR' AND a.INWD_RecordType_var = b.CRINWD_RecordType_var " +
                                " AND a.INWD_RecordNo_int = b.CRINWD_RecordNo_int" +
                                " AND c.CONT_Id = a.INWD_CONT_Id";
                        if (bldg.Trim() != "" && bldg != "---All---")
                        {
                            mySql += " and a.INWD_Building_var='" + bldg + "'";
                        }
                        if (supplier.Trim() != "" && supplier != "---All---") //mf, cr, ndt not having suppiler name
                        {
                            mySql += " and a.INWD_RecordType_var <> 'CR'";
                        }
                        mySql += "  and a.INWD_CL_Id= " + clId.ToString();
                        if (siteId != 0)
                        {
                            mySql = mySql + " and a.INWD_SITE_Id =" + siteId.ToString("0");
                        }
                        if (billId != "")
                        {
                            mySql = mySql + " and (a.INWD_BILL_Id = '" + billId + "'";
                            mySql = mySql + " Or (Select count(BILLD_BILL_Id) from tbl_BillDetail Where BILLD_ReferenceNo_int = a.INWD_ReferenceNo_int and BILLD_BILL_Id = '" + billId + "') > 0 ";
                            mySql = mySql + " Or (Select count(MBILLD_BILL_Id) from tbl_BillDetailMonthly Where MBILLD_ReferenceNo_int = a.INWD_ReferenceNo_int and MBILLD_BILL_Id = '" + billId + "') > 0 ";
                            mySql = mySql + ")";
                        }
                        mySql += otherStr.Replace("'AAC'", "'CR'");
                        //mySql += " order by a.INWD_RecordType_var ,a.INWD_RecordNo_int desc";
                        mySql += " order by a.INWD_ReceivedDate_dt desc";
                        break;
                    case "FLYASH":
                        if (testType == "---All---" || testType == "Physical")
                        {
                            mySql = " SELECT a.INWD_ReceivedDate_dt as dateofreceiving, 'FlyAsh' as record_type,a.INWD_Building_var, b.FLYASHINWD_ReferenceNo_var as refno ,b.FLYASHINWD_IssueDate_dt as status" +
                            " ,c.CONT_Name_var, a.INWD_ContactNo_var, '' as MFINWD_FinalIssueDt " +
                            " FROM tbl_Inward AS a, tbl_FlyAsh_Inward AS b, tbl_Contact as c  " +
                                " WHERE a.INWD_RecordType_var = 'FLYASH' AND a.INWD_RecordType_var = b.FLYASHINWD_RecordType_var " +
                                " AND a.INWD_RecordNo_int = b.FLYASHINWD_RecordNo_int" +
                                " AND c.CONT_Id = a.INWD_CONT_Id";
                            if (bldg.Trim() != "" && bldg != "---All---")
                            {
                                mySql += " and a.INWD_Building_var='" + bldg + "'";
                            }
                            if (supplier.Trim() != "" && supplier != "---All---")
                            {
                                mySql += " and b.FLYASHINWD_SupplierName_var='" + supplier + "'";
                            }
                            mySql += "  and a.INWD_CL_Id= " + clId.ToString();
                            if (siteId != 0)
                            {
                                mySql = mySql + " and a.INWD_SITE_Id =" + siteId.ToString("0");
                            }
                            if (billId != "")
                            {
                                mySql = mySql + " and (a.INWD_BILL_Id = '" + billId + "'";
                                mySql = mySql + " Or (Select count(BILLD_BILL_Id) from tbl_BillDetail Where BILLD_ReferenceNo_int = a.INWD_ReferenceNo_int and BILLD_BILL_Id = '" + billId + "') > 0 ";
                                mySql = mySql + " Or (Select count(MBILLD_BILL_Id) from tbl_BillDetailMonthly Where MBILLD_ReferenceNo_int = a.INWD_ReferenceNo_int and MBILLD_BILL_Id = '" + billId + "') > 0 ";
                                mySql = mySql + ")";
                            }
                            mySql += otherStr.Replace("'AAC'", "'FLYASH'");
                        }
                        if (testType == "---All---")
                        {
                            mySql += " Union ";
                        }
                        if (testType == "---All---" || testType == "Chemical")
                        {
                            mySql += otherStr.Replace("'AAC'", "'FLYASHCH'").Replace("Union", "");
                        }
                        //mySql += " order by a.INWD_RecordType_var ,a.INWD_RecordNo_int desc";
                        mySql += " order by a.INWD_ReceivedDate_dt desc";
                        break;
                    case "BT-":
                        mySql = " SELECT a.INWD_ReceivedDate_dt as dateofreceiving, 'Brick' as record_type,a.INWD_Building_var, b.BTINWD_ReferenceNo_var as refno ,b.BTINWD_IssueDate_dt as status" +
                            " ,c.CONT_Name_var, a.INWD_ContactNo_var, '' as MFINWD_FinalIssueDt " +
                            " FROM tbl_Inward AS a, tbl_Brick_Inward AS b, tbl_Contact as c  " +
                                " WHERE a.INWD_RecordType_var = 'BT-' AND a.INWD_RecordType_var = b.BTINWD_RecordType_var " +
                                " AND a.INWD_RecordNo_int = b.BTINWD_RecordNo_int" +
                                " AND c.CONT_Id = a.INWD_CONT_Id";
                        if (bldg.Trim() != "" && bldg != "---All---")
                        {
                            mySql += " and a.INWD_Building_var='" + bldg + "'";
                        }
                        if (supplier.Trim() != "" && supplier != "---All---")
                        {
                            mySql += " and b.BTINWD_SupplierName_var='" + supplier + "'";
                        }
                        mySql += "  and a.INWD_CL_Id= " + clId.ToString();
                        if (siteId != 0)
                        {
                            mySql = mySql + " and a.INWD_SITE_Id =" + siteId.ToString("0");
                        }
                        if (billId != "")
                        {
                            mySql = mySql + " and (a.INWD_BILL_Id = '" + billId + "'";
                            mySql = mySql + " Or (Select count(BILLD_BILL_Id) from tbl_BillDetail Where BILLD_ReferenceNo_int = a.INWD_ReferenceNo_int and BILLD_BILL_Id = '" + billId + "') > 0 ";
                            mySql = mySql + " Or (Select count(MBILLD_BILL_Id) from tbl_BillDetailMonthly Where MBILLD_ReferenceNo_int = a.INWD_ReferenceNo_int and MBILLD_BILL_Id = '" + billId + "') > 0 ";
                            mySql = mySql + ")";
                        }
                        mySql += otherStr.Replace("'AAC'", "'BT-'");
                        //mySql += " order by a.INWD_RecordType_var ,a.INWD_RecordNo_int desc";
                        mySql += " order by a.INWD_ReceivedDate_dt desc";
                        break;
                    case "PT":
                        mySql = " SELECT a.INWD_ReceivedDate_dt as dateofreceiving, 'Pavement Block' as record_type,a.INWD_Building_var, b.PTINWD_ReferenceNo_var as refno ,b.PTINWD_IssueDate_dt as status" +
                            " ,c.CONT_Name_var, a.INWD_ContactNo_var, '' as MFINWD_FinalIssueDt " +
                            " FROM tbl_Inward AS a, tbl_Pavement_Inward AS b, tbl_Contact as c  " +
                                " WHERE a.INWD_RecordType_var = 'PT' AND a.INWD_RecordType_var = b.PTINWD_RecordType_var " +
                                " AND a.INWD_RecordNo_int = b.PTINWD_RecordNo_int" +
                                " AND c.CONT_Id = a.INWD_CONT_Id";

                        if (bldg.Trim() != "" && bldg != "---All---")
                        {
                            mySql += " and a.INWD_Building_var='" + bldg + "'";
                        }
                        if (supplier.Trim() != "" && supplier != "---All---")
                        {
                            mySql += " and b.PTINWD_SupplierName_var='" + supplier + "'";
                        }
                        mySql += "  and a.INWD_CL_Id= " + clId.ToString();
                        if (siteId != 0)
                        {
                            mySql += " and a.INWD_SITE_Id =" + siteId.ToString("0");
                        }
                        if (billId != "")
                        {
                            mySql += " and (a.INWD_BILL_Id = '" + billId + "'";
                            mySql += " Or (Select count(BILLD_BILL_Id) from tbl_BillDetail Where BILLD_ReferenceNo_int = a.INWD_ReferenceNo_int and BILLD_BILL_Id = '" + billId + "') > 0 ";
                            mySql += " Or (Select count(MBILLD_BILL_Id) from tbl_BillDetailMonthly Where MBILLD_ReferenceNo_int = a.INWD_ReferenceNo_int and MBILLD_BILL_Id = '" + billId + "') > 0 ";
                            mySql += ")";
                        }
                        mySql += otherStr.Replace("'AAC'", "'PT'");
                        //mySql += " order by a.INWD_RecordType_var ,a.INWD_RecordNo_int desc";
                        mySql += " order by a.INWD_ReceivedDate_dt desc";
                        break;
                    case "SOLID":
                        if (testType == "---All---" || testType == "Concrete Blocks")
                        {
                            mySql = " SELECT a.INWD_ReceivedDate_dt as dateofreceiving, 'Masonary Block' as record_type,a.INWD_Building_var, b.SOLIDINWD_ReferenceNo_var as refno ,b.SOLIDINWD_IssueDate_dt  as status" +
                            " ,c.CONT_Name_var, a.INWD_ContactNo_var, '' as MFINWD_FinalIssueDt " +
                            " FROM tbl_Inward AS a, tbl_Solid_Inward AS b, tbl_Contact as c  " +
                                " WHERE a.INWD_RecordType_var = 'SOLID' AND a.INWD_RecordType_var = b.SOLIDINWD_RecordType_var " +
                                " AND a.INWD_RecordNo_int = b.SOLIDINWD_RecordNo_int" +
                                " AND c.CONT_Id = a.INWD_CONT_Id";
                            if (bldg.Trim() != "" && bldg != "---All---")
                            {
                                mySql += " and a.INWD_Building_var='" + bldg + "'";
                            }
                            if (supplier.Trim() != "" && supplier != "---All---")
                            {
                                mySql += " and b.SOLIDINWD_SupplierName_var='" + supplier + "'";
                            }
                            mySql += "  and a.INWD_CL_Id= " + clId.ToString();
                            if (siteId != 0)
                            {
                                mySql = mySql + " and a.INWD_SITE_Id =" + siteId.ToString("0");
                            }
                            if (billId != "")
                            {
                                mySql = mySql + " and (a.INWD_BILL_Id = '" + billId + "'";
                                mySql = mySql + " Or (Select count(BILLD_BILL_Id) from tbl_BillDetail Where BILLD_ReferenceNo_int = a.INWD_ReferenceNo_int and BILLD_BILL_Id = '" + billId + "') > 0 ";
                                mySql = mySql + " Or (Select count(MBILLD_BILL_Id) from tbl_BillDetailMonthly Where MBILLD_ReferenceNo_int = a.INWD_ReferenceNo_int and MBILLD_BILL_Id = '" + billId + "') > 0 ";
                                mySql = mySql + ")";
                            }
                            mySql += otherStr.Replace("'AAC'", "'SOLID'");
                        }
                        if (testType == "---All---")
                        {
                            mySql += " Union ";
                        }
                        if (testType == "---All---" || testType == "AAC Blocks")
                        {
                            mySql = " SELECT a.INWD_ReceivedDate_dt as dateofreceiving, 'AAC Block' as record_type,a.INWD_Building_var, b.AACINWD_ReferenceNo_var as refno ,b.AACINWD_IssueDate_dt as status" +
                            " ,c.CONT_Name_var, a.INWD_ContactNo_var, '' as MFINWD_FinalIssueDt " +
                            " FROM tbl_Inward AS a, tbl_AAC_Inward AS b, tbl_Contact as c  " +
                                " WHERE a.INWD_RecordType_var = 'AAC' AND a.INWD_RecordType_var = b.AACINWD_RecordType_var " +
                                " AND a.INWD_RecordNo_int = b.AACINWD_RecordNo_int" +
                                " AND c.CONT_Id = a.INWD_CONT_Id";
                            if (bldg.Trim() != "" && bldg != "---All---")
                            {
                                mySql += " and a.INWD_Building_var='" + bldg + "'";
                            }
                            if (supplier.Trim() != "" && supplier != "---All---")
                            {
                                mySql += " and b.AACINWD_SupplierName_var='" + supplier + "'";
                            }
                            mySql += "  and a.INWD_CL_Id= " + clId.ToString();
                            if (siteId != 0)
                            {
                                mySql = mySql + " and a.INWD_SITE_Id =" + siteId.ToString("0");
                            }
                            if (billId != "")
                            {
                                mySql = mySql + " and (a.INWD_BILL_Id = '" + billId + "'";
                                mySql = mySql + " Or (Select count(BILLD_BILL_Id) from tbl_BillDetail Where BILLD_ReferenceNo_int = a.INWD_ReferenceNo_int and BILLD_BILL_Id = '" + billId + "') > 0 ";
                                mySql = mySql + " Or (Select count(MBILLD_BILL_Id) from tbl_BillDetailMonthly Where MBILLD_ReferenceNo_int = a.INWD_ReferenceNo_int and MBILLD_BILL_Id = '" + billId + "') > 0 ";
                                mySql = mySql + ")";
                            }
                            mySql += otherStr.Replace("'AAC'", "'AAC'");
                        }
                        if (testType == "---All---")
                        {
                            mySql += " Union ";
                        }
                        if (testType == "---All---" || testType == "Bricks")
                        {
                            mySql = " SELECT a.INWD_ReceivedDate_dt as dateofreceiving, 'Brick' as record_type,a.INWD_Building_var, b.BTINWD_ReferenceNo_var as refno ,b.BTINWD_IssueDate_dt as status" +
                            " ,c.CONT_Name_var, a.INWD_ContactNo_var, '' as MFINWD_FinalIssueDt " +
                            " FROM tbl_Inward AS a, tbl_Brick_Inward AS b, tbl_Contact as c  " +
                                " WHERE a.INWD_RecordType_var = 'BT-' AND a.INWD_RecordType_var = b.BTINWD_RecordType_var " +
                                " AND a.INWD_RecordNo_int = b.BTINWD_RecordNo_int" +
                                " AND c.CONT_Id = a.INWD_CONT_Id";
                            if (bldg.Trim() != "" && bldg != "---All---")
                            {
                                mySql += " and a.INWD_Building_var='" + bldg + "'";
                            }
                            if (supplier.Trim() != "" && supplier != "---All---")
                            {
                                mySql += " and b.BTINWD_SupplierName_var='" + supplier + "'";
                            }
                            mySql += "  and a.INWD_CL_Id= " + clId.ToString();
                            if (siteId != 0)
                            {
                                mySql = mySql + " and a.INWD_SITE_Id =" + siteId.ToString("0");
                            }
                            if (billId != "")
                            {
                                mySql = mySql + " and (a.INWD_BILL_Id = '" + billId + "'";
                                mySql = mySql + " Or (Select count(BILLD_BILL_Id) from tbl_BillDetail Where BILLD_ReferenceNo_int = a.INWD_ReferenceNo_int and BILLD_BILL_Id = '" + billId + "') > 0 ";
                                mySql = mySql + " Or (Select count(MBILLD_BILL_Id) from tbl_BillDetailMonthly Where MBILLD_ReferenceNo_int = a.INWD_ReferenceNo_int and MBILLD_BILL_Id = '" + billId + "') > 0 ";
                                mySql = mySql + ")";
                            }
                            mySql += otherStr.Replace("'AAC'", "'BT-'");
                        }
                        //mySql += " order by a.INWD_RecordType_var ,a.INWD_RecordNo_int desc";
                        mySql += " order by a.INWD_ReceivedDate_dt desc";
                        break;
                    case "PILE":
                        mySql = " SELECT a.INWD_ReceivedDate_dt as dateofreceiving, 'Pile' as record_type,a.INWD_Building_var, b.PILEINWD_ReferenceNo_var as refno ,b.PILEINWD_IssueDate_dt as status" +
                            " ,c.CONT_Name_var, a.INWD_ContactNo_var, '' as MFINWD_FinalIssueDt " +
                            " FROM tbl_Inward AS a, tbl_Pile_Inward AS b, tbl_Contact as c  " +
                                " WHERE a.INWD_RecordType_var = 'PILE' AND a.INWD_RecordType_var = b.PILEINWD_RecordType_var " +
                                " AND a.INWD_RecordNo_int = b.PILEINWD_RecordNo_int" +
                                " AND c.CONT_Id = a.INWD_CONT_Id";
                        if (bldg.Trim() != "" && bldg != "---All---")
                        {
                            mySql += " and a.INWD_Building_var='" + bldg + "'";
                        }
                        if (supplier.Trim() != "" && supplier != "---All---")
                        {
                            mySql += " and b.PILEINWD_SupplierName_var='" + supplier + "'";
                        }
                        mySql += "  and a.INWD_CL_Id= " + clId.ToString();
                        if (siteId != 0)
                        {
                            mySql = mySql + " and a.INWD_SITE_Id =" + siteId.ToString("0");
                        }
                        if (billId != "")
                        {
                            mySql = mySql + " and (a.INWD_BILL_Id = '" + billId + "'";
                            mySql = mySql + " Or (Select count(BILLD_BILL_Id) from tbl_BillDetail Where BILLD_ReferenceNo_int = a.INWD_ReferenceNo_int and BILLD_BILL_Id = '" + billId + "') > 0 ";
                            mySql = mySql + " Or (Select count(MBILLD_BILL_Id) from tbl_BillDetailMonthly Where MBILLD_ReferenceNo_int = a.INWD_ReferenceNo_int and MBILLD_BILL_Id = '" + billId + "') > 0 ";
                            mySql = mySql + ")";
                        }
                        mySql += otherStr.Replace("'AAC'", "'PILE'");
                        //mySql += " order by a.INWD_RecordType_var ,a.INWD_RecordNo_int desc";
                        mySql += " order by a.INWD_ReceivedDate_dt desc";
                        break;
                    case "TILE":
                        if (testType == "---All---" || testType == "Physical")
                        {
                            mySql = " SELECT a.INWD_ReceivedDate_dt as dateofreceiving, 'Tile' as record_type,a.INWD_Building_var, b.TILEINWD_ReferenceNo_var as refno ,b.TILEINWD_IssueDate_dt as status" +
                            " ,c.CONT_Name_var, a.INWD_ContactNo_var, '' as MFINWD_FinalIssueDt " +
                            " FROM tbl_Inward AS a, tbl_Tile_Inward AS b, tbl_Contact as c  " +
                                " WHERE a.INWD_RecordType_var = 'TILE' AND a.INWD_RecordType_var = b.TILEINWD_RecordType_var " +
                                " AND a.INWD_RecordNo_int = b.TILEINWD_RecordNo_int" +
                                " AND c.CONT_Id = a.INWD_CONT_Id";
                            if (bldg.Trim() != "" && bldg != "---All---")
                            {
                                mySql += " and a.INWD_Building_var='" + bldg + "'";
                            }
                            if (supplier.Trim() != "" && supplier != "---All---")
                            {
                                mySql += " and b.TILEINWD_SupplierName_var='" + supplier + "'";
                            }
                            mySql += "  and a.INWD_CL_Id= " + clId.ToString();
                            if (siteId != 0)
                            {
                                mySql = mySql + " and a.INWD_SITE_Id =" + siteId.ToString("0");
                            }
                            if (billId != "")
                            {
                                mySql = mySql + " and (a.INWD_BILL_Id = '" + billId + "'";
                                mySql = mySql + " Or (Select count(BILLD_BILL_Id) from tbl_BillDetail Where BILLD_ReferenceNo_int = a.INWD_ReferenceNo_int and BILLD_BILL_Id = '" + billId + "') > 0 ";
                                mySql = mySql + " Or (Select count(MBILLD_BILL_Id) from tbl_BillDetailMonthly Where MBILLD_ReferenceNo_int = a.INWD_ReferenceNo_int and MBILLD_BILL_Id = '" + billId + "') > 0 ";
                                mySql = mySql + ")";
                            }
                            mySql += otherStr.Replace("'AAC'", "'TILE'");
                        }
                        if (testType == "---All---")
                        {
                            mySql += " Union ";
                        }
                        if (testType == "---All---" || testType == "Chemical")
                        {
                            mySql += otherStr.Replace("'AAC'", "'TILECH'").Replace("Union", "");
                        }
                        //mySql += " order by a.INWD_RecordType_var ,a.INWD_RecordNo_int desc";
                        mySql += " order by a.INWD_ReceivedDate_dt desc";
                        break;
                    case "STC":
                        mySql = " SELECT a.INWD_ReceivedDate_dt as dateofreceiving, 'Steel Chemical' as record_type,a.INWD_Building_var, b.STCINWD_ReferenceNo_var as refno ,b.STCINWD_IssueDate_dt as status" +
                            " ,c.CONT_Name_var, a.INWD_ContactNo_var, '' as MFINWD_FinalIssueDt " +
                            " FROM tbl_Inward AS a, tbl_SteelChemical_Inward AS b, tbl_Contact as c  " +
                                " WHERE a.INWD_RecordType_var = 'STC' AND a.INWD_RecordType_var = b.STCINWD_RecordType_var " +
                                " AND a.INWD_RecordNo_int = b.STCINWD_RecordNo_int" +
                                " AND c.CONT_Id = a.INWD_CONT_Id";
                        if (bldg.Trim() != "" && bldg != "---All---")
                        {
                            mySql += " and a.INWD_Building_var='" + bldg + "'";
                        }
                        if (supplier.Trim() != "" && supplier != "---All---")
                        {
                            mySql += " and b.STCINWD_SupplierName_var='" + supplier + "'";
                        }
                        mySql += "  and a.INWD_CL_Id= " + clId.ToString();
                        if (siteId != 0)
                        {
                            mySql = mySql + " and a.INWD_SITE_Id =" + siteId.ToString("0");
                        }
                        if (billId != "")
                        {
                            mySql = mySql + " and (a.INWD_BILL_Id = '" + billId + "'";
                            mySql = mySql + " Or (Select count(BILLD_BILL_Id) from tbl_BillDetail Where BILLD_ReferenceNo_int = a.INWD_ReferenceNo_int and BILLD_BILL_Id = '" + billId + "') > 0 ";
                            mySql = mySql + " Or (Select count(MBILLD_BILL_Id) from tbl_BillDetailMonthly Where MBILLD_ReferenceNo_int = a.INWD_ReferenceNo_int and MBILLD_BILL_Id = '" + billId + "') > 0 ";
                            mySql = mySql + ")";
                        }
                        mySql += otherStr.Replace("'AAC'", "'STC'");
                        //mySql += " order by a.INWD_RecordType_var ,a.INWD_RecordNo_int desc";
                        mySql += " order by a.INWD_ReceivedDate_dt desc";
                        break;
                    case "CCH":
                        mySql = " SELECT a.INWD_ReceivedDate_dt as dateofreceiving, 'Cement Chemical' as record_type,a.INWD_Building_var, b.CCHINWD_ReferenceNo_var as refno ,b.CCHINWD_IssueDate_dt as status" +
                            " ,c.CONT_Name_var, a.INWD_ContactNo_var, '' as MFINWD_FinalIssueDt " +
                            " FROM tbl_Inward AS a, tbl_CementChemical_Inward AS b, tbl_Contact as c  " +
                                " WHERE a.INWD_RecordType_var = 'CCH' AND a.INWD_RecordType_var = b.CCHINWD_RecordType_var " +
                                " AND a.INWD_RecordNo_int = b.CCHINWD_RecordNo_int" +
                                " AND c.CONT_Id = a.INWD_CONT_Id";
                        if (bldg.Trim() != "" && bldg != "---All---")
                        {
                            mySql += " and a.INWD_Building_var='" + bldg + "'";
                        }
                        if (supplier.Trim() != "" && supplier != "---All---")
                        {
                            mySql += " and b.CCHINWD_SupplierName_var='" + supplier + "'";
                        }
                        mySql += "  and a.INWD_CL_Id= " + clId.ToString();
                        if (siteId != 0)
                        {
                            mySql = mySql + " and a.INWD_SITE_Id =" + siteId.ToString("0");
                        }
                        if (billId != "")
                        {
                            mySql = mySql + " and (a.INWD_BILL_Id = '" + billId + "'";
                            mySql = mySql + " Or (Select count(BILLD_BILL_Id) from tbl_BillDetail Where BILLD_ReferenceNo_int = a.INWD_ReferenceNo_int and BILLD_BILL_Id = '" + billId + "') > 0 ";
                            mySql = mySql + " Or (Select count(MBILLD_BILL_Id) from tbl_BillDetailMonthly Where MBILLD_ReferenceNo_int = a.INWD_ReferenceNo_int and MBILLD_BILL_Id = '" + billId + "') > 0 ";
                            mySql = mySql + ")";
                        }
                        mySql += otherStr.Replace("'AAC'", "'CCH'");
                        //mySql += " order by a.INWD_RecordType_var ,a.INWD_RecordNo_int desc";
                        mySql += " order by a.INWD_ReceivedDate_dt desc";
                        break;
                    case "WT":
                        mySql = " SELECT a.INWD_ReceivedDate_dt as dateofreceiving,'Water' as record_type,a.INWD_Building_var, b.WTINWD_ReferenceNo_var as refno ,b.WTINWD_IssueDate_dt as status" +
                            " ,c.CONT_Name_var, a.INWD_ContactNo_var, '' as MFINWD_FinalIssueDt " +
                            " FROM tbl_Inward AS a, tbl_Water_Inward AS b, tbl_Contact as c  " +
                                " WHERE a.INWD_RecordType_var = 'WT' AND a.INWD_RecordType_var = b.WTINWD_RecordType_var " +
                                " AND a.INWD_RecordNo_int = b.WTINWD_RecordNo_int" +
                                " AND c.CONT_Id = a.INWD_CONT_Id";
                        if (bldg.Trim() != "" && bldg != "---All---")
                        {
                            mySql += " and a.INWD_Building_var='" + bldg + "'";
                        }
                        if (supplier.Trim() != "" && supplier != "---All---")
                        {
                            mySql += " and b.WTINWD_SupplierName_var='" + supplier + "'";
                        }
                        mySql += "  and a.INWD_CL_Id= " + clId.ToString();
                        if (siteId != 0)
                        {
                            mySql = mySql + " and a.INWD_SITE_Id =" + siteId.ToString("0");
                        }
                        if (billId != "")
                        {
                            mySql = mySql + " and (a.INWD_BILL_Id = '" + billId + "'";
                            mySql = mySql + " Or (Select count(BILLD_BILL_Id) from tbl_BillDetail Where BILLD_ReferenceNo_int = a.INWD_ReferenceNo_int and BILLD_BILL_Id = '" + billId + "') > 0 ";
                            mySql = mySql + " Or (Select count(MBILLD_BILL_Id) from tbl_BillDetailMonthly Where MBILLD_ReferenceNo_int = a.INWD_ReferenceNo_int and MBILLD_BILL_Id = '" + billId + "') > 0 ";
                            mySql = mySql + ")";
                        }
                        mySql += otherStr.Replace("'AAC'", "'WT'");
                        //mySql += " order by a.INWD_RecordType_var ,a.INWD_RecordNo_int desc";
                        mySql += " order by a.INWD_ReceivedDate_dt desc";
                        break;
                    case "SO":
                        mySql = "SELECT a.INWD_ReceivedDate_dt as dateofreceiving,'Soil' as record_type," +
                        " a.INWD_Building_var, b.SOINWD_ReferenceNo_var as refno , b.SOINWD_IssueDate_dt as status " +
                        " ,c.CONT_Name_var, a.INWD_ContactNo_var, '' as MFINWD_FinalIssueDt " +
                        " FROM tbl_Inward AS a, tbl_Soil_Inward AS b, tbl_Contact as c " +
                        " WHERE a.INWD_RecordType_var = 'SO'  AND a.INWD_RecordType_var = b.SOINWD_RecordType_var AND a.INWD_RecordNo_int = b.SOINWD_RecordNo_int " +
                        " AND c.CONT_Id = a.INWD_CONT_Id";

                        if (bldg.Trim() != "" && bldg != "---All---")
                        {
                            mySql += " and a.INWD_Building_var='" + bldg + "'";
                        }
                        if (supplier.Trim() != "" && supplier != "---All---")
                        {
                            mySql += " and b.SOINWD_SupplierName_var='" + supplier + "'";
                        }
                        mySql += "  and a.INWD_CL_Id= " + clId.ToString();
                        if (siteId != 0)
                        {
                            mySql = mySql + " and a.INWD_SITE_Id =" + siteId.ToString("0");
                        }
                        if (billId != "")
                        {
                            mySql = mySql + " and (a.INWD_BILL_Id = '" + billId + "'";
                            mySql = mySql + " Or (Select count(BILLD_BILL_Id) from tbl_BillDetail Where BILLD_ReferenceNo_int = a.INWD_ReferenceNo_int and BILLD_BILL_Id = '" + billId + "') > 0 ";
                            mySql = mySql + " Or (Select count(MBILLD_BILL_Id) from tbl_BillDetailMonthly Where MBILLD_ReferenceNo_int = a.INWD_ReferenceNo_int and MBILLD_BILL_Id = '" + billId + "') > 0 ";
                            mySql = mySql + ")";
                        }
                        mySql += otherStr.Replace("'AAC'", "'SO'");
                        //mySql += " order by a.INWD_RecordType_var ,a.INWD_RecordNo_int desc";
                        mySql += " order by a.INWD_ReceivedDate_dt desc";
                        break;
                    case "OT":
                        if (selRecType == "FLYASH" && testType == "Chemical")
                        {
                            mySql = otherStr.Replace("'AAC'", "'FLYASHCH'").Replace("Union", "");
                            mySql += " order by a.INWD_ReceivedDate_dt desc";
                        }
                        else if (selRecType == "TILE" && testType == "Chemical")
                        {
                            mySql = otherStr.Replace("'AAC'", "'TILECH'").Replace("Union", "");
                            mySql += " order by a.INWD_ReceivedDate_dt desc";
                        }
                        else if (selRecType == "OT")
                        {
                            mySql = otherStr.Replace("'AAC'", "'OT'").Replace("Union", "");
                            mySql += " order by a.INWD_ReceivedDate_dt desc";
                        }

                        break;
                    default:
                        mySql = "";
                        break;
                }
                if (mySql != "")
                {
                    SqlDataAdapter da = new SqlDataAdapter(mySql, cn);
                    ds.Reset();
                    da.Fill(ds);
                    dt1 = new DataTable();
                    dt1 = ds.Tables[0];
                    dtReturn.Merge(dt1);
                }
            }
            return dtReturn;
        }
        catch (Exception e)
        {
            throw e;
        }

    }

    public String AddRowForClientDetail(string Col1, string col3, string col5, String col7)
    {
        string myStr, prnColon;
        //col1
        myStr = "<tr><td width= 14% align=left valign=top height=19 ><font size=2>" + Col1 + "</font></td>";
        //'col2(colon)
        prnColon = ":";
        if (Col1 == "")
        { prnColon = ""; }
        myStr += "<td width= 1% align=left valign=top height=19 ><font size=2>" + prnColon + "</font></td>";

        //col3 ,col4 (gap), col 5
        myStr += "<td width= 45% align=left valign=top height=19 ><font size=2>" + col3 + "</font></td>";
        myStr += "<td width= 8% align=left valign=top height=19 ><font size=2>&nbsp;</font></td>";
        myStr += "<td width= 16% align=left valign=top height=19 ><font size=2>" + col5 + "</font></td>";
        //'col 6 (colon)
        prnColon = ":";
        if (col5 == "")
        { prnColon = ""; }

        myStr += "<td width= 1% align=left valign=top height=19 ><font size=2>" + prnColon + "</font></td>";
        //'col 7
        myStr += "<td width= 15% align=left valign=top height=19 ><font size=2>" + col7 + " </font></td></tr>";
        return (myStr);
    }
    public String getCubeRemarks(string referenceNo)
    {
        DataSet ds = new DataSet();
        DataTable dt = new DataTable();
        SqlDataAdapter da = new SqlDataAdapter();
        string myQueStr = "", retStr = "", tmpStr = "";
        string CompNote = "", StrenthNote = "", testType = "";
        int cnt = 0, mAge = 0, mGrade = 0;
        decimal mAvg = 0;

        myQueStr = "select c.id_mark,c.age,c.s1 as len,c.s2 as ht,c.s3 as bt,c.wt as weight,";
        myQueStr += " c.C_S_Area as csArea,c.density,c.Reading as load,c.comp_str as compstr,b.avg_str as avg123 , a.Record_No as RecNo, a.Grade_Of_Concrete as grade, d.TestType as TestType";
        myQueStr += " from cube_in_record as a,record_avg_str as b,cube_testing_lab  as c ,Rec_Total_qty as d ";
        myQueStr += " where a.cube_inward_id=c.cube_inward_id and  b.avg_id = c.avg_id  ";
        myQueStr += " and a.Record_No = d.Record_No and a.Record_type=d.Record_Type";
        myQueStr += " and a.refno='" + referenceNo + "'";

        da = new SqlDataAdapter(myQueStr, cn);
        da.Fill(ds);
        dt = ds.Tables[0];
        ds.Dispose();
        
        mAge = Convert.ToInt32(dt.Rows[0].ItemArray[1]);
        testType = dt.Rows[0]["TestType"].ToString();
        if (decimal.TryParse(dt.Rows[0].ItemArray[10].ToString(), out mAvg) == true)
            mAvg = Convert.ToDecimal(dt.Rows[0].ItemArray[10]);
        if (int.TryParse(dt.Rows[0].ItemArray[12].ToString().Replace("M ", ""), out mGrade) == true)
            mGrade = Convert.ToInt32(dt.Rows[0].ItemArray[12].ToString().Replace("M ", ""));
        
        if (testType == "Site Cube Casting")
            testType = "Concrete Cube";
        else if (testType == "")
            testType = "Concrete Cube";
        if (testType == "Concrete Cube")
        {
            if (mAge >= 28 && mAvg > 0 && mGrade > 0)
            {
                if (mAvg >= mGrade + 3)
                    CompNote = "The test result complies with the requirement of IS 456-2000, subject to standard deviation less than 4.";
                else if (mAvg > mGrade - 3 && mAvg < mGrade + 3)
                    CompNote = "The test result does not comply with requirement of IS 456-2000. However for acceptance of concrete average of 4 consecutive non overlapping test samples should be considered. Refer page 30 table XI IS 456-2000.";
                else if (mAvg <= mGrade - 3)
                    CompNote = "The test result does not comply with the requirement of IS 456-2000.";

            }
            else if (mAge >= 3 && mAge < 28 && mAvg > 0 && mGrade > 0)
            {
                //StrenthNote = Convert.ToString(Math.Round(mAge / (4.7 + 0.833 * mAge) * mGrade));
                //StrenthNote = "* As per SP : 24-1983 (Exp Handbook on IS 456) expected strength at " + mAge.ToString() + " days for M " + mGrade.ToString() + " grade of concrete is " + StrenthNote + ".0 N/mm<sup>2";
            }
        }
        myQueStr = "SELECT Remarks from cube_in_record as a,st_ct_new_rem as b,record_remarks as c" +
                     " where a.cube_inward_id=b.cube_inward_id and b.rem_id=c.rem_id " +
                    " and a.RefNo='" + referenceNo.Trim() + "'";

        dt.Dispose();
        da = new SqlDataAdapter(myQueStr, cn);
        ds = new DataSet();
        da.Fill(ds);
        DataTable dtRem = new DataTable();
        dtRem = ds.Tables[0];
        ds.Dispose();
        da.Dispose();

        if (CompNote == "")
        {
            for (int i = 0; i < dtRem.Rows.Count; i++)
            {
                CompNote = dtRem.Rows[i].ItemArray[0].ToString();
                if (dtRem.Rows[i].ItemArray[0].ToString().Contains("15 %") == true)
                {
                    if (CompNote.Substring(0, 4).Contains(")") == true)
                    {
                        CompNote = CompNote.Substring(CompNote.IndexOf(")") + 1);
                        break;
                    }
                }
                else
                    CompNote = "";
            }
        }
        retStr = "<table width='90%'>";
        if (CompNote != "")
        {
            retStr += "<tr><td width='89%' colspan=2 align=left valign=top height=19><font size=3><u><b> Compliance Note: </b></u></font></td></tr>";

            retStr += "<tr><td width= 1% align=right valign=top height=19 ><font size=3><b>" + 1.ToString() + ")</b></font></td>";
            retStr += "<td width='89%' align=left valign=top height=19><font size=3><b>" + CompNote + "</b></font></td></tr>";
        }

        //References
        myQueStr = "";
        myQueStr = "SELECT  ct_rem as Remarks  from ct_rem_st as b,st_ct_Rem as a,cube_in_record as c";
        myQueStr += " where  a.cube_inward_id=c.cube_inward_id and a.ct_rem_id=b.ct_rem_id";
        myQueStr += " and c.RefNo='" + referenceNo.Trim() + "'";

        dt.Dispose();
        da = new SqlDataAdapter(myQueStr, cn);
        ds = new DataSet();
        da.Fill(ds);
        dt = new DataTable();
        dt = ds.Tables[0];
        ds.Dispose();
        cnt = 0;
        //retStr = "<table width='90%'>";
        for (int i = 0; i < dt.Rows.Count; i++)
        {

            tmpStr = dt.Rows[i].ItemArray[0].ToString();
            cnt = cnt + 1;
            if (i == 0)
            {
                retStr += "<tr><td width='89%' colspan=2 align=left valign=top height=19><font size=2><u><b> References: </b></u></font></td></tr>";
            }
            if (tmpStr.Trim() != "")
            {
                if (tmpStr.Substring(0, 4).Contains(")") == true)
                {
                    tmpStr = tmpStr.Substring(tmpStr.IndexOf(")") + 1);
                }
                retStr += "<tr><td width= 1% align=right valign=top height=19 ><font size=2>" + cnt.ToString() + ")</font></td>";
                retStr += "<td width='89%' align=left valign=top height=19><font size=2>" + tmpStr + "</font></td></tr>";
            }
        }
        dt.Reset();
        // remarks print start
        cnt = 0;
        for (int i = 0; i < dtRem.Rows.Count; i++)
        {
            tmpStr = dtRem.Rows[i].ItemArray[0].ToString();
            if (tmpStr.ToString().Contains("15 %") != true)
            {
                if (cnt == 0)
                {
                    retStr += "<tr><td width='89%' colspan=2 align=left valign=top height=19><font size=2><u><b> Remarks: </b></u></font></td></tr>";

                }
                if (tmpStr.Trim() != "" && tmpStr.Trim().ToLower() != "remarks")
                {
                    cnt = cnt + 1;
                    if (tmpStr.Substring(0, 4).Contains(")") == true)
                    {
                        tmpStr = tmpStr.Substring(tmpStr.IndexOf(")") + 1);
                    }

                    retStr += "<tr><td width= 1% align=right valign=top height=19 ><font size=2>" + cnt.ToString() + ")</font></td>";
                    retStr += "<td width='89%' align=left valign=top height=19><font size=2>" + tmpStr + "</font></td></tr>";
                }
            }
        }
        if (StrenthNote != "")
        {
            if (cnt == 0)
            {
                retStr += "<tr><td width='89%' colspan=2 align=left valign=top height=19><font size=2><u><b> Remarks: </b></u></font></td></tr>";
            }
            cnt = cnt + 1;
            retStr += "<tr><td width= 1% align=right valign=top height=19 ><font size=2>" + cnt.ToString() + ")</font></td>";
            retStr += "<td width='89%' align=left valign=top height=19><font size=2>" + StrenthNote + "</font></td></tr>";
        }
        retStr += "</table>";

        return retStr;

    }
    //remarks for all reports
    public String getRemarks(string referenceNo, string rType)
    {

        try
        {
            string mySql, retStr;
            string tmpStr;
            Int32 cnt;

            DataSet ds = new DataSet();
            DataTable dt = new DataTable();

            //References
            mySql = "";
            retStr = "";
            switch (rType)
            {
                case "CT":  // references
                    mySql = "SELECT  ct_rem as Remarks  from ct_rem_st as b,st_ct_Rem as a,cube_in_record as c";
                    mySql += " where  a.cube_inward_id=c.cube_inward_id and a.ct_rem_id=b.ct_rem_id";
                    mySql += " and c.RefNo='" + referenceNo.Trim() + "'";
                    break;
                case "ST":
                    mySql = "SELECT  b.st_rem  from steel_St_rem_in as a,steel_st_rem as b,steel_test_inward_table as c";
                    mySql += " where a.st_in_id=c.st_in_id and a.st_rem_id=b.st_rem_id ";
                    mySql += " and c.RefNo='" + referenceNo.Trim() + "'";
                    break;
                case "AGGT":
                    mySql = " select b.aggr_rem from Aggt_St_Rem as b ";
                    break;
                case "CEMT":
                    mySql = "SELECT  c.cemt_rem from cement_inward as a,cemt_st_rem_in as b,cemt_rem_st as c " +
                    " where a.cem_id=b.cem_id and b.cemt_rem_id=c.cemt_rem_id " +
                    " and a.Record_type='" + rType + "'" +
                    " and a.RefNo='" + referenceNo.Trim() + "'";
                    break;
                case "FLYASH":
                    mySql = "select ISCode from StdISCodeMaster where recordtype='" + rType + "'";
                    break;
                case "PT":
                    mySql = "select remarks from st_pt_new_rem as a,cube_in_record as b,record_remarks as c " +
                            //" where a.pt_id=b.cube_inward_id and a.rem_id=c.rem_id  and instr(remarks,'IS ') > 0 " +
                            " where a.pt_id=b.cube_inward_id and a.rem_id=c.rem_id  and charindex('IS ', remarks) > 0 " +
                            " and b.Record_type='" + rType + "'" +
                            " and b.RefNo='" + referenceNo.Trim() + "'";
                    break;
                case "SOLID":
                    mySql = " select remarks from solidInward as b, ST_Solid_New_Rem as a,record_remarks as c " +
                    " where b.solid_id=a.solid_id  and a.rem_id=c.rem_id  and b.RefNo='" + referenceNo.Trim() + "'" +
                    //" and instr(remarks,'IS ') > 0 ";
                    " and charindex('IS ',remarks) > 0 ";
                    break;
                case "BT-":
                    mySql = " SELECT remarks from st_allBrick_new_rem as a,brickinward as b,record_remarks as c " +
                           " where a.bt_id=b.bt_id and a.rem_id=c.rem_id and b.RefNo='" + referenceNo.Trim() + "'" +
                           //" and instr(remarks,'IS ') > 0 ";
                           " and charindex('IS ',remarks) > 0 ";
                    break;
                case "MF":
                    mySql = "select ISCode from StdISCodeMaster where recordType='MF'";
                    break;
                case "CR":
                    mySql = "select ISCode from StdISCodeMaster where recordType='CR'";
                    break;
                case "STC":
                    mySql = "select ISCode from StdISCodeMaster where recordType='STC'";
                    break;
                case "WT":
                    mySql = "select ISCode from StdISCodeMaster where recordType='WT'";
                    //mySql = " SELECT * FROM StdISCodeMaster where ucase(recordtype)='" + rType + "' and " +
                    //    " FromDate <=cdate('" + _mRcvdDate + "') and ( toDate >=cdate('" + _mRcvdDate + "') or IsNull(toDate) =  True)";
                    break;
                default:
                    retStr = "<table  width='90%'>";
                    goto remPrint;

            }
            SqlDataAdapter da = new SqlDataAdapter(mySql, cn);
            da.Fill(ds);
            dt = ds.Tables[0];
            ds.Dispose();
            cnt = 0;
            retStr = "<table width='90%'>";
            for (int i = 0; i < dt.Rows.Count; i++)
            {

                tmpStr = dt.Rows[i].ItemArray[0].ToString();
                cnt = cnt + 1;
                if (i == 0)
                {
                    retStr += "<tr><td width='89%' colspan=2 align=left valign=top height=19><font size=2><u><b> References: </b></u></font></td></tr>";
                }
                if (tmpStr.Trim() != "")
                {
                    if (tmpStr.Substring(0, 4).Contains(")") == true)
                    {
                        tmpStr = tmpStr.Substring(tmpStr.IndexOf(")") + 1);
                    }
                    retStr += "<tr><td width= 1% align=right valign=top height=19 ><font size=2>" + cnt.ToString() + ")</font></td>";
                    retStr += "<td width='89%' align=left valign=top height=19><font size=2>" + tmpStr + "</font></td></tr>";
                }
            }
            dt.Reset();
        // references end

    remPrint:
            switch (rType)
            {
                case "CT":
                    mySql = "SELECT Remarks from cube_in_record as a,st_ct_new_rem as b,record_remarks as c" +
                             " where a.cube_inward_id=b.cube_inward_id and b.rem_id=c.rem_id " +
                            " and a.RefNo='" + referenceNo.Trim() + "'";
                    break;
                case "ST":
                    mySql = " SELECT b.remarks from st_st_new_rem as a,record_remarks as b,steel_test_inward_table as c" +
                            " where a.st_in_id=c.st_in_id and a.rem_id=b.rem_id " +
                            " and c.RefNo='" + referenceNo.Trim() + "'";
                    break;
                case "AGGT":
                    mySql = "Select c.remarks from aggregate_table as a, st_aggt_notes as b," +
                            " Record_remarks as c where a.aggr_id=b.aggr_id and b.rem_id=c.rem_id " +
                            " and a.RefNo='" + referenceNo.Trim() + "'";
                    break;
                case "CEMT":
                    mySql = "select c.remarks from cement_inward as a,st_cemt_new_rem as b,record_remarks as c" +
                        " where a.cem_id=b.cem_id and b.rem_id=c.rem_id " +
                        " and a.Record_type='" + rType + "'" +
                        " and a.RefNo='" + referenceNo.Trim() + "'";
                    break;
                case "FLYASH":
                    mySql = "select a.remarks from flyash_remarks as a,Fly_Ash_Inward as b " +
                        " where a.FlyAsh_ID=b.fly_Ash_id and b.Record_type='" + rType + "'" +
                        " and b.RefNo='" + referenceNo.Trim() + "'";
                    break;
                case "PT":
                    mySql = "select remarks from st_pt_new_rem as a,cube_in_record as b,record_remarks as c " +
                            //" where a.pt_id=b.cube_inward_id and a.rem_id=c.rem_id  and instr(remarks,'IS ') <= 0 " +
                            " where a.pt_id=b.cube_inward_id and a.rem_id=c.rem_id  and charindex('IS ', remarks) <= 0 " +
                            " and b.Record_type='" + rType + "'" +
                            " and b.RefNo='" + referenceNo.Trim() + "'";
                    break;
                case "SOLID":
                    mySql = " select remarks from solidInward as b, ST_Solid_New_Rem as a,record_remarks as c " +
                        " where b.solid_id=a.solid_id  and a.rem_id=c.rem_id  and b.RefNo='" + referenceNo.Trim() + "'" +
                        //" and instr(remarks,'IS ') <= 0 ";
                        " and charindex('IS ',remarks) <= 0 ";
                    break;

                case "PILE":
                    mySql = " SELECT  b.remark from pile_test_master as a, pile_remarks as b" +
                            " where a.set_of_pile=b.set_of_pile and rem_type='Reg'" +
                            " and a.RefNo='" + referenceNo.Trim() + "'";
                    break;
                case "BT-":
                    mySql = " SELECT remarks from st_allBrick_new_rem as a,brickinward as b,record_remarks as c " +
                           " where a.bt_id=b.bt_id and a.rem_id=c.rem_id and b.RefNo='" + referenceNo.Trim() + "'" +
                           //" and instr(remarks,'IS ') <= 0 ";
                           " and charindex('IS ',remarks) <= 0 ";
                    break;
                case "MF":
                    mySql = " SELECT remarks from mf_inward_table as a,st_mf_new_rem as b,record_remarks as c " +
                            " where a.mf_inward_id=b.mf_inward_id and b.rem_id=c.rem_id and a.set_of_MF='" + referenceNo.Trim() + "'";
                    break;
                case "CR":
                    mySql = "SELECT remarks from st_core_new_rem as a,core_Test_master as b, record_remarks as c " +
                           " where a.set_of_core=b.set_of_core and a.rem_id=c.rem_id and b.RefNo='" + referenceNo + "'";
                    break;
                case "NDT":
                    mySql = " SELECT remarks from st_ndt_new_rem as a,record_remarks as b where a.rem_id=b.rem_id " +
                          " and a.record_no=" + Convert.ToDouble(referenceNo).ToString("0");

                    break;
                case "STC":
                case "CCH":
                    mySql = "SELECT Remark FROM ChemicalTestRemarks where RecType = '" + rType + "'" +
                            " and RefNo = '" + referenceNo + "'";
                    break;
                case "WT":
                    mySql = "SELECT Remark FROM WaterTestRemarks where RefNo = '" + referenceNo + "'";
                    break;

            }


            SqlDataAdapter da1 = new SqlDataAdapter(mySql, cn);
            da1.Fill(ds);
            dt = ds.Tables[0];
            ds.Dispose();
            da1.Dispose();
            // remarks print start
            cnt = 0;
            for (int i = 0; i < dt.Rows.Count; i++)
            {

                tmpStr = dt.Rows[i].ItemArray[0].ToString();

                if (i == 0)
                {
                    if (rType == "NDT")
                    {
                        retStr += "<tr><td width='89%' colspan=2 align=left valign=top height=19><font size=2><u><b> Notes:  </b></u></font></td></tr>";
                    }
                    else
                    {
                        retStr += "<tr><td width='89%' colspan=2 align=left valign=top height=19><font size=2><u><b> Remarks: </b></u></font></td></tr>";
                    }
                }
                if (tmpStr.Trim() != "" && tmpStr.Trim().ToLower() != "remarks")
                {
                    cnt = cnt + 1;
                    if (tmpStr.Substring(0, 4).Contains(")") == true)
                    {
                        tmpStr = tmpStr.Substring(tmpStr.IndexOf(")") + 1);
                    }

                    retStr += "<tr><td width= 1% align=right valign=top height=19 ><font size=2>" + cnt.ToString() + ")</font></td>";
                    retStr += "<td width='89%' align=left valign=top height=19><font size=2>" + tmpStr + "</font></td></tr>";
                }
            }
            retStr += "</table>";

            return retStr;
        }
        catch (Exception e)
        {
            throw e;
        }
    }
    //report header for all reports
    public String getReportHeader(string rType, string referenceNo, ref  Boolean flgIsFinal, string db)
    {
        string mySql;
        string reportHead;
        string issueDate;
        string[] strDate = new string[3];
        DataSet ds = new DataSet();
        DataTable dt = new DataTable();
        mySql = "select client_name,Office_Address,Site_name"; ;
        //client,Address,Site
        reportHead = "Test Report";
        switch (rType)
        {
            case "CT":
                mySql += ",IssueDate as DateOfIssue,Set_Of_Cube as SetOfRecord,refno,f.CollectionDate as CollectionDate";
                mySql += ",block_Test_Date as DateOfTesting,Description,Bill_No as billDetails,Nature_Of_Work as nature";
                mySql += ",Grade_Of_Concrete as grade,a.Coupon_No as counponNo,block_cast_Date as DateOfCasting ";
                mySql += " From cube_inward as a,cube_in_record as b,record_table as c,client_info as d,site_table as e,in_out_rec_table as f ";
                mySql += " where a.cube_inward_id=b.cube_inward_id and b.record_no=c.record_no ";
                mySql += " and b.record_type=c.record_type and c.client_id=d.client_id and c.site_id=e.site_id ";
                mySql += "  and b.record_no=f.record_no and b.record_type=f.record_type";
                mySql += " and refno='" + referenceNo + "'";
                mySql += " and b.record_type='" + rType + "'";
                reportHead = "Concrete Cube Compressive Strength";
                break;
            case "ST":
                mySql += ",b.Issuedate,b.set_of_steel as SetOfRecord,b.refno,f.CollectionDate as DateOfCollection,";
                mySql += " b.testing_date as Testingdate,b.type_of_bar as TypeOfBar,";
                mySql += "b.bill_no as billno,b.grade_of_steel as GradeOfSteel,";
                mySql += " b.name_of_supp as SupplierName,b.name_of_manu as manufacturar,b.Sample_Description,f.recd_given_date as DateOfReceiving,b.ComplianceNote ";
                mySql += " From steel_test_inward_table as b,record_table as c,client_info as d,site_table as e,in_out_rec_table as f ";
                mySql += " where b.record_no=c.record_no and b.record_type=c.record_type ";
                mySql += " and c.client_id=d.client_id and c.site_id=e.site_id ";
                mySql += "  and b.record_no=f.record_no and b.record_type=f.record_type";
                mySql += " and b.refno='" + referenceNo + "'";
                mySql += " and b.record_type='" + rType + "'";
                reportHead = "Reinforcement Steel / Rebars ";
                break;
            case "AGGT":
                mySql += ",b.issuedate,b.Set_Of_Agg as SetOfAggt,refno,";
                mySql += "f.recd_given_date as DateOfReceiving,b.Add_Aggr_Label as aggtName,b.bill_no as  billNo";
                mySql += " From Aggregate_table as b,record_table as c,client_info as d,site_table as e,in_out_rec_table as f ";
                mySql += " where b.record_no=c.record_no and b.record_type=c.record_type and c.client_id=d.client_id ";
                mySql += " and c.site_id=e.site_id and b.record_no=f.record_no and b.record_type=f.record_type";
                mySql += " and b.refno='" + referenceNo + "'";
                mySql += " and b.record_type='" + rType + "'";
                reportHead = "Fine & Coarse Aggregate ";
                break;
            case "CEMT":
                mySql += ",b.IssueDate as dateofissue,b.Set_Of_Cemt as setofCement,refno,f.recd_given_date as DateofReceiving, " +
                " a.report_date as DateofTesting,b.Cement_Name as cementName,b.bill_no as billno " +
                " From Cement_inward  as b,cement_tests_lab as a,record_table as c,client_info as d,site_table as e,in_out_rec_table as f " +
                " where b.record_no=c.record_no and b.record_type=c.record_type and c.client_id=d.client_id and c.site_id=e.site_id  and b.record_no=f.record_no " +
                " and b.CEM_ID=a.CEM_ID and b.record_type=f.record_type and b.refno='" + referenceNo + "'" +
                " and b.record_type='" + rType + "' order by a.report_date";
                reportHead = "Cement";
                break;
            case "FLYASH":
                mySql += ",b.IssueDate as ReportIssueDate,b.Set_Of_FlyAsh as SetOfFlyAsh,refno,f.recd_given_date as DateofReceiving,";
                mySql += " b.Tested_Date as DateofTesting,b.CemtName as cementName,b.Fly_Ash_Name as FlyAshName,b.bill_no as billno";
                mySql += " From Fly_Ash_inward  as b,record_table as c,client_info as d,site_table as e,in_out_rec_table as f ";
                mySql += " where b.record_no=c.record_no and b.record_type=c.record_type ";
                mySql += " and c.client_id=d.client_id and c.site_id=e.site_id ";
                mySql += " and b.record_no=f.record_no and b.record_type=f.record_type";
                mySql += " and b.refno='" + referenceNo + "'";
                mySql += " and b.record_type='" + rType + "'";
                reportHead = "Pulverized Fuel Ash (Fly Ash)";
                break;
            case "PT":
                mySql += ",g.IssueDate as DateOfIssue,Set_Of_Cube as SetOfRecord,refno,f.recd_given_date as DateOfReceiving ";
                mySql += ",block_Test_Date as DateOfTesting,Description,Bill_No as billDetails,Nature_Of_Work as nature";
                mySql += ",Grade_Of_Concrete as grade,a.Coupon_No as counponNo,block_cast_Date as DateOfCasting,g.rate_id ";
                mySql += " From cube_inward as a,cube_in_record as b,record_table as c,client_info as d,site_table as e,in_out_rec_table as f,ptRateIn as g ";
                mySql += " where a.cube_inward_id=b.cube_inward_id and b.record_no=c.record_no ";
                mySql += " and b.record_type=c.record_type and c.client_id=d.client_id and c.site_id=e.site_id ";
                mySql += "  and b.record_no=f.record_no and b.record_type=f.record_type";
                mySql += " and a.cube_inward_id=g.PT_ID ";
                mySql += " and refno='" + referenceNo + "'";
                mySql += " and b.record_type='" + rType + "'";
                reportHead = "#Paving Block#";
                break;
            case "SOLID":
                mySql += ",b.IssueDate,a.SetOfBlocks,a.refno,f.recd_given_date as DateofReceiving," +
                " b.Test_Date as testDate,a.Cast_date as castdate,bill_no as billno," +
                " solid_type as solidType" +
                " From solidInward a,SolidRateIn as b,record_table as c,client_info as d,site_table as e,in_out_rec_table as f" +
                " where a.record_no=c.record_no  and a.record_type=c.record_type and c.client_id=d.client_id and c.site_id=e.site_id " +
                " and a.record_no=f.record_no and a.Solid_id=b.Solid_id " +
                " and a.Solid_id=b.Solid_id and a.refno='" + referenceNo + "'" +
                 " and a.record_type='" + rType + "'";
                reportHead = "Masonry Unit";
                break;
            case "CR":
                mySql += ",b.report_issue_Date as ReportIssueDate,b.set_of_core as SetOfCore," +
                " b.refno,f.recd_given_Date as DateofReceiving,b.Date_Of_Testing as DateOfTesting,b.DateofspExt as DateOfExtraction," +
                " b.dt_no as billno,b.ConcreteMem as ConcreteMember,a.Grade_Of_Concrete as GradeOfConcrete,b.CurringCondi as CurringCondition" +
                " From core_test_lab as a,core_test_master as b,record_table as c,client_info as d,site_table as e,in_out_rec_table as f " +
                " where b.record_no=c.record_no and b.record_type=c.record_type  and  b.title_id=a.title_id " +
                " and c.client_id=d.client_id and c.site_id=e.site_id " +
                " and b.record_no=f.record_no and b.record_type=f.record_type " +
                " and b.refno='" + referenceNo + "'" +
                " and b.record_type='" + rType + "'";
                reportHead = "Concrete Core Compressive Strength";
                break;
            case "BT-":
                mySql += ",b.IssueDate,b.SetOfBrick,b.refno,f.recd_given_Date as DateofReceiving,b.Test_Date as TestingDate,b.Name_Of_Supp as NameOfSupp,b.bill_no as billno,b.brick_type as BrickType,b.Description";
                mySql += " From BrickInward as b,record_table as c,client_info as d,site_table as e,in_out_rec_table as f ";
                mySql += " where b.record_no=c.record_no and b.record_type=c.record_type ";
                mySql += " and c.client_id=d.client_id and c.site_id=e.site_id ";
                mySql += "  and b.record_no=f.record_no and b.record_type=f.record_type";
                mySql += " and b.refno='" + referenceNo + "'";
                mySql += " and b.record_type='" + rType + "'";
                reportHead = "Brick";
                break;
            case "NDT":
                mySql += ",b.report_issue_Date as ReportIssueDate,b.record_no ,f.recd_given_Date as DateofReceiving," +
                    " b.Date_Of_Testing as DateOfTesting,b.Nature_Of_Testing,b.Kind_Attn,b.dt_No " +
                    " From Ndt_table as b,record_table as c,client_info as d,site_table as e,in_out_rec_table as f " +
                    " Where b.record_no=c.record_no and b.record_type=c.record_type and c.client_id=d.client_id and " +
                    " c.site_id=e.site_id  and b.record_no=f.record_no and b.record_type=f.record_type  " +
                    " and b.record_no=" + Convert.ToDouble(referenceNo).ToString("0");
                reportHead = "Non Destructive Testing";
                break;
            case "MF":
                //mySql += " ,iif(isdate(finalissuedate)=true,finalissuedate,mdissuedate) as issuedate " +
                mySql += " ,case when finalissuedate is null then mdissuedate else finalissuedate end as issuedate " +
                        ",set_of_mf,Bill_no,finalissuedate from  mf_inward_table as a, " +
                        " record_table as c,client_info as d,site_table as e,in_out_rec_table as f " +
                        " where a.record_no=c.record_no  and a.record_type=c.record_type and c.client_id=d.client_id " +
                        " and c.site_id=e.site_id  and a.record_no=f.record_no and " +
                        " a.Set_Of_MF='" + referenceNo + "'";
                reportHead = "[*]";
                break;
            case "PILE":
                mySql += ",IssueDate as DateOfIssue,set_of_pile as SetOfPile,refno,f.recd_given_date as DateofReceiving,b.Testing_Date as DateofTesting,b.bill_no as billno," +
                            " b.Description,b.KindAttn From Pile_test_master as b ,record_table as c,client_info as d,site_table as e,in_out_rec_table as f" +
                            " where b.record_no=c.record_no and b.record_type=c.record_type and c.client_id=d.client_id and c.site_id=e.site_id " +
                            " and c.record_no=f.record_no and c.record_type=f.record_type and b.refno='" + referenceNo + "'";
                reportHead = "Pile Intigrity";
                break;
            case "TILE":
                mySql += ",IssuDate,SetOfTile,refno,DateofReceiving,TestingDate,billno,Description,TileType,testtype";
                reportHead = "";
                break;
            case "STC":
                mySql += ",b.Issuedate,b.setofstc as SetOfRecord,b.refno,f.recd_given_Date as DateOfReceiving," +
                    " b.testingdate as Testingdate,b.typeofbar as TypeOfBar,b.billno as billno,b.gradeofsteel as GradeOfSteel," +
                    " b.nameofsupp as SupplierName,b.nameofmanu as manufacturar, b.SampleDescription as Description " +
                    " From stChemicalInward as b,record_table as c,client_info as d,site_table as e,in_out_rec_table as f " +
                    " where b.recordno=c.record_no and b.recordtype=c.record_type and c.client_id=d.client_id and c.site_id=e.site_id " +
                    " and b.recordno=f.record_no and b.recordtype=f.record_type " +
                    " and b.refno='" + referenceNo + "'" +
                    " and b.recordtype='" + rType + "'";
                reportHead = "Reinforcement Steel Chemical ";
                break;
            case "CCH":
                mySql += ",b.Issuedate,b.SetOfCCH as SetOfRecord,b.refno,f.recd_given_Date as DateOfReceiving," +
                        " b.testingdate as Testingdate,b.cementname, b.billno as billno,description,b.SupplierName as SupplierName " +
                        " From CEMTChemicalInward as b,record_table as c,client_info as d,site_table as e,in_out_rec_table as f " +
                        " where b.recordno=c.record_no and b.recordtype=c.record_type and c.client_id=d.client_id and c.site_id=e.site_id " +
                        " and b.recordno=f.record_no and b.recordtype=f.record_type " +
                        " and b.refno='" + referenceNo + "'" +
                        " and b.recordtype='" + rType + "'";
                reportHead = "Cement Chemical ";
                break;
            case "WT":
                mySql += ",b.Issuedate,b.SetOfwt as SetOfRecord,b.refno,f.recd_given_Date as DateOfReceiving," +
                        " b.testingdate as Testingdate,b.description, b.billno as billno,b.SupplierName as SupplierName " +
                        " from WaterTestingInward as b,record_table as c,client_info as d,site_table as e,in_out_rec_table as f " +
                        " where b.recordno=c.record_no and b.recordtype=c.record_type and c.client_id=d.client_id and c.site_id=e.site_id " +
                        " and b.recordno=f.record_no and b.recordtype=f.record_type " +
                        " and b.refno='" + referenceNo + "'";
                reportHead = "Water Testing";
                break;
        }
        //OpenConnection(db);
        cn.Open();
        SqlDataAdapter da = new SqlDataAdapter(mySql, cn);
        da.Fill(ds);
        dt = ds.Tables[0];
        mySql = dt.Rows[0].ItemArray[3].ToString();
        issueDate = mySql;
        if (mySql.Contains("Days"))
        {

            string[] strVal = mySql.Split('|');
            if (strVal.Length > 0)
            {
                string[] strVal1 = strVal[strVal.Length - 1].Split('=');
                issueDate = strVal1[1].ToString();
            }
        }
        DataRow rw = dt.Rows[0];
        DateTime dt1 = DateTime.Now;
        if (rType == "MF")
        {
            if (DateTime.TryParse(rw["FinalIssueDate"].ToString(), out dt1))
            { flgIsFinal = true; }

        }
        mySql = dt.Rows[0].ItemArray[3].ToString();
        mySql = "";
        mySql += "<html>";
        mySql += "<head>";
        mySql += "<style type='text/css'>";
        mySql += "body { margin-left:5em}"; /* Or another measurement unit, like px */
        mySql += "</style>";
        mySql += "</head>";
        mySql += "<tr><td width='100%'height=105>";
        string imgName = "";
        if (HttpContext.Current.Session["databasename"].ToString().Contains("Pune")==true )
        {
            if (rType == "MF")
                imgName = "WONABLPune";
            else
                imgName = "NABLPune";
        }
        else if (HttpContext.Current.Session["databasename"].ToString() == "Mumbai.mdb")
        {
            if (rType == "MF")
                imgName = "WONABLMumbai";
            else
                imgName = "NABLMumbai";
        }
        else if (HttpContext.Current.Session["databasename"].ToString() == "Nashik.mdb")
        {
            if (rType == "MF")
                imgName = "WONABLNashik";
            else
                imgName = "NABLNashik";
        }
        mySql += "<img border=0 src='Images/" + imgName + ".JPG' ></td></tr>"; ////width='95%'height=75
        mySql += "<table border=0 cellpadding=0 cellspacing=0 style=border-collapse: collapse bordercolor=#111111 width=90% id=AutoNumber1>";
        mySql += "<tr><td width='90%' colspan=7 height=19>&nbsp;</td></tr>";
        mySql += "<tr><td width='90%' colspan=7 align=center valign=top height=19><font size=4><b>Test Report</b></font></td></tr>";
        //mySql += "<tr>";
        //mySql += "<a href='javascript:javascript:history.go(-1)'>Back</a>";
        //mySql += "</tr>";
        mySql += "<tr><td width='90%' colspan=7 align=center valign=top height=19><font size=4><b>" + reportHead + "</b></font></td></tr>";
        mySql += "<tr><td width='90%' colspan=7>&nbsp;</td></tr>";
        if (dt.Rows.Count > 0)
        {
            //client,Address,Site,DateOfIssue,SetOfRecord,refno,DateOfReceiving 
            // 0        1      2      3           4         5       6
            DateTime tempDate;
            if (rType == "CEMT" || rType == "FLYASH" || rType == "CCH" || rType == "WT")
            {
                //tempDate = Convert.ToDateTime(issueDate);
                strDate = issueDate.Split('/');
                if (strDate[2].ToString().Contains(" ") == true)
                {
                    strDate = issueDate.Substring(0, issueDate.IndexOf(" ")).Split('/');
                }
                tempDate = new DateTime(Convert.ToInt32(strDate[2]), Convert.ToInt32(strDate[1]), Convert.ToInt32(strDate[0]));
            }
            else if (rType == "CR" || rType =="NDT")
            {
                //tempDate = Convert.ToDateTime(dt.Rows[0].ItemArray[3].ToString());
                strDate = dt.Rows[0].ItemArray[3].ToString().Split('/');
                if (strDate[2].ToString().Contains(" ") == true)
                {
                    strDate = dt.Rows[0].ItemArray[3].ToString().Substring(0, dt.Rows[0].ItemArray[3].ToString().IndexOf(" ")).ToString().Split('/');
                }
                tempDate = new DateTime(Convert.ToInt32(strDate[2]), Convert.ToInt32(strDate[1]), Convert.ToInt32(strDate[0]));
            }
            else
            {
                tempDate = Convert.ToDateTime(dt.Rows[0].ItemArray[3].ToString());
            }
            mySql += AddRowForClientDetail("Customer name", dt.Rows[0].ItemArray[0].ToString(), "Date of Issue", tempDate.ToString("dd-MMM-yy"));
            if (rType == "NDT")
            {
                mySql += AddRowForClientDetail("Office address", dt.Rows[0].ItemArray[1].ToString(), "Record No.", rType + " - " + dt.Rows[0].ItemArray[4].ToString());
            }
            else
            {
                mySql += AddRowForClientDetail("Office address", dt.Rows[0].ItemArray[1].ToString(), "Record No.", rType + " - " + dt.Rows[0].ItemArray[4].ToString());

                if (rType == "MF")
                {
                    mySql += AddRowForClientDetail("", "", "Bill No.", "DT-" + dt.Rows[0].ItemArray[5].ToString());
                }
                else
                {
                    mySql += AddRowForClientDetail("", "", "Sample Ref.No.", rType + " - " + dt.Rows[0].ItemArray[5].ToString().Substring(0, dt.Rows[0].ItemArray[5].ToString().IndexOf("/")));
                }

            }

            switch (rType)
            {
                case "CT":
                    //DateOfTesting,Description,billDetails,nature,grade,DateOfCasting
                    //     7            8           9          10    11     12
                    if (dt.Rows[0]["counponNo"].ToString() != "---")
                        mySql += AddRowForClientDetail("Site name", dt.Rows[0].ItemArray[2].ToString(), "Coupon No", dt.Rows[0]["counponNo"].ToString());
                    else
                        mySql += AddRowForClientDetail("Site name", dt.Rows[0].ItemArray[2].ToString(), "Bill No.", "DT-" + dt.Rows[0].ItemArray[9].ToString());
                    //tempDate = Convert.ToDateTime(dt.Rows[0]["dateOfCasting"]);

                    strDate = dt.Rows[0]["dateOfCasting"].ToString().Split('/');
                    tempDate = new DateTime(Convert.ToInt32(strDate[2]), Convert.ToInt32(strDate[1]), Convert.ToInt32(strDate[0]));

                    mySql += AddRowForClientDetail("Nature of work", dt.Rows[0].ItemArray[10].ToString(), "Date of casting", tempDate.ToString("dd-MMM-yy"));
                    tempDate = Convert.ToDateTime(dt.Rows[0].ItemArray[6]);
                    mySql += AddRowForClientDetail("Grade of concrete", dt.Rows[0].ItemArray[11].ToString(), "Date of receipt", tempDate.ToString("dd-MMM-yy"));
                    tempDate = Convert.ToDateTime(dt.Rows[0].ItemArray[7]);
                    mySql += AddRowForClientDetail("Description", dt.Rows[0]["description"].ToString(), "Date of testing", tempDate.ToString("dd-MMM-yy"));
                    break;
                case "ST":
                    //Testingdate,TypeOfBar,billno,GradeOfSteel,SupplierName,SupplierStatus 
                    //  7            8        9           10        11  
                    mySql += AddRowForClientDetail("Site name", dt.Rows[0].ItemArray[2].ToString(), "Bill No.", "DT-" + dt.Rows[0].ItemArray[9].ToString());
                    tempDate = Convert.ToDateTime(dt.Rows[0].ItemArray[6].ToString());
                    mySql += AddRowForClientDetail("Type of steel", dt.Rows[0].ItemArray[8].ToString(), "Date of receipt", tempDate.ToString("dd-MMM-yy"));
                    tempDate = Convert.ToDateTime(dt.Rows[0].ItemArray[7].ToString());
                    mySql += AddRowForClientDetail("Grade of steel", dt.Rows[0].ItemArray[10].ToString(), "Date of testing", tempDate.ToString("dd-MMM-yy"));
                    mySql += AddRowForClientDetail("Description", dt.Rows[0].ItemArray[13].ToString(), "", "");
                    if (dt.Rows[0].ItemArray[11].ToString().Trim() != "---")
                    {
                        mySql += AddRowForClientDetail("Supplier name", dt.Rows[0].ItemArray[11].ToString(), "", "");
                    }
                    //_mRcvdDate = Convert.ToDateTime(dt.Rows[0].ItemArray[14].ToString()).ToShortDateString().ToString();
                    _mRcvdDate = Convert.ToDateTime(dt.Rows[0].ItemArray[14].ToString()).ToString();
                    _stComplianceNote = Convert.ToInt32(dt.Rows[0].ItemArray[15].ToString());
                    break;
                case "STC":
                    //Testingdate,TypeOfBar,billno,GradeOfSteel,SupplierName,SupplierStatus 
                    //  7            8        9           10        11  
                    mySql += AddRowForClientDetail("Site name", dt.Rows[0].ItemArray[2].ToString(), "Bill No.", "DT-" + dt.Rows[0].ItemArray[9].ToString());
                    tempDate = Convert.ToDateTime(dt.Rows[0].ItemArray[6].ToString());
                    mySql += AddRowForClientDetail("Type of steel", dt.Rows[0].ItemArray[8].ToString(), "Date of receipt", tempDate.ToString("dd-MMM-yy"));
                    tempDate = Convert.ToDateTime(dt.Rows[0].ItemArray[7].ToString());
                    mySql += AddRowForClientDetail("Grade of steel", dt.Rows[0].ItemArray[10].ToString(), "Date of testing", tempDate.ToString("dd-MMM-yy"));
                    mySql += AddRowForClientDetail("Description", dt.Rows[0].ItemArray[13].ToString(), "", "");
                    if (dt.Rows[0].ItemArray[11].ToString().Trim() != "---")
                    {
                        mySql += AddRowForClientDetail("Supplier name", dt.Rows[0].ItemArray[11].ToString(), "", "");
                    }
                    //_mRcvdDate = Convert.ToDateTime(dt.Rows[0].ItemArray[14].ToString()).ToShortDateString().ToString();
                    //_mRcvdDate = Convert.ToDateTime(dt.Rows[0].ItemArray[8].ToString()).ToString();
                    //_stComplianceNote = Convert.ToInt32(dt.Rows[0].ItemArray[15].ToString());
                    break;
                case "AGGT":
                    //issuedate,SetOfAggt,refno,DateOfReceiving,aggtName,billNo
                    //  3         4         5      6               7     8
                    mySql += AddRowForClientDetail("", "", "Bill No.", "DT-" + dt.Rows[0].ItemArray[8].ToString());

                    mySql += AddRowForClientDetail("Site name", dt.Rows[0].ItemArray[2].ToString(), "Date of receiving", Convert.ToDateTime(dt.Rows[0].ItemArray[6].ToString()).ToShortDateString());
                    break;
                case "CEMT":
                    //,dateofissue,setofCemt,refno,DateofReceiving,DateofTesting,cementName,billno";
                    //    3           4        5        6              7           8
                    mySql += AddRowForClientDetail("Site name", dt.Rows[0].ItemArray[2].ToString(), "Bill No.", "DT-" + dt.Rows[0].ItemArray[9].ToString());

                    mySql += AddRowForClientDetail("", "", "Date of receiving", Convert.ToDateTime(dt.Rows[0].ItemArray[6].ToString()).ToShortDateString());
                    mySql += AddRowForClientDetail("Cement name", dt.Rows[0].ItemArray[8].ToString(), "Date of testing", Convert.ToDateTime(dt.Rows[0].ItemArray[7].ToString()).ToShortDateString());
                    break;

                case "CCH":
                    mySql += AddRowForClientDetail("Site name", dt.Rows[0].ItemArray[2].ToString(), "Bill No.", "DT-" + dt.Rows[0].ItemArray[9].ToString());
                    mySql += AddRowForClientDetail("Description ", dt.Rows[0].ItemArray[10].ToString(), "Date of receiving", Convert.ToDateTime(dt.Rows[0].ItemArray[6].ToString()).ToShortDateString());
                    mySql += AddRowForClientDetail("Cement name", dt.Rows[0].ItemArray[8].ToString(), "Date of testing", Convert.ToDateTime(dt.Rows[0].ItemArray[7].ToString()).ToShortDateString());
                    mySql += AddRowForClientDetail("Suuplier Name", dt.Rows[0].ItemArray[11].ToString(), "", "");
                    break;
                case "FLYASH":
                    //,dateofissue,setofCemt,refno,DateofReceiving,DateofTesting,cementName,billno";
                    //    3           4        5        6              7           8
                    mySql += AddRowForClientDetail("Site name", dt.Rows[0].ItemArray[2].ToString(), "Bill No.", "DT-" + dt.Rows[0].ItemArray[9].ToString());

                    mySql += AddRowForClientDetail("Fly Ash name", dt.Rows[0].ItemArray[10].ToString(), "Date of receiving", Convert.ToDateTime(dt.Rows[0].ItemArray[6].ToString()).ToShortDateString());
                    mySql += AddRowForClientDetail("Cement name", dt.Rows[0].ItemArray[8].ToString(), "Date of testing", Convert.ToDateTime(dt.Rows[0].ItemArray[7].ToString()).ToShortDateString());

                    break;
                case "SOLID":
                    //IssueDate,SetOfBlocks,refno,DateofReceiving,testDate,castdate,billno,solidType";
                    //   3         4         5         6            7          8     9      10
                    mySql += AddRowForClientDetail("Site name", dt.Rows[0].ItemArray[2].ToString(), "Bill No.", "DT-" + dt.Rows[0].ItemArray[9].ToString());

                    mySql += AddRowForClientDetail("Type of block", dt.Rows[0].ItemArray[10].ToString(), "Date of casting", dt.Rows[0].ItemArray[8].ToString());
                    mySql += AddRowForClientDetail("", "", "Date of testing", Convert.ToDateTime(dt.Rows[0].ItemArray[7].ToString()).ToShortDateString());
                    // check for date                             
                    mySql += AddRowForClientDetail("", "", "Date of casting", dt.Rows[0].ItemArray[8].ToString());
                    break;
                case "CR":
                    //      3                 4     5          6           7                   8             9
                    //",ReportIssueDate,SetOfCore,refno,DateofReceiving,DateOfTesting,DateOfExtraction,billno,";
                    //",ConcreteMember,GradeOfConcrete,CurringCondition";
                                        
                    mySql += AddRowForClientDetail("Site name", dt.Rows[0].ItemArray[2].ToString(), "Bill No.", "DT-" + dt.Rows[0].ItemArray[9].ToString());
                    mySql += AddRowForClientDetail("Concrete member", dt.Rows[0].ItemArray[10].ToString(), "", "");
                    //tempDate = Convert.ToDateTime(dt.Rows[0].ItemArray[8].ToString());
                    strDate = dt.Rows[0].ItemArray[8].ToString().Split('/');
                    tempDate = new DateTime(Convert.ToInt32(strDate[2]), Convert.ToInt32(strDate[1]), Convert.ToInt32(strDate[0]));
                    mySql += AddRowForClientDetail("Grade of concrete", dt.Rows[0].ItemArray[11].ToString(), "Specimen extraction date", tempDate.ToString("dd-MMM-yy"));
                    tempDate = Convert.ToDateTime(dt.Rows[0].ItemArray[7].ToString());
                    mySql += AddRowForClientDetail("Curring condition", dt.Rows[0].ItemArray[12].ToString(), "Date of testing", tempDate.ToString("dd-MMM-yy"));

                    break;
                case "BT-":
                    //IssueDate,SetOfBrick,refno,DateofReceiving,testDate,NameOfSupp,billno,BrickType,Description";                    
                    //    3         4         5           6         7        8         9      10        11
                    mySql += AddRowForClientDetail("Site name", dt.Rows[0].ItemArray[2].ToString(), "Bill No.", "DT-" + dt.Rows[0].ItemArray[9].ToString());
                    mySql += AddRowForClientDetail("Description", dt.Rows[0].ItemArray[11].ToString(), "Brick Type", dt.Rows[0].ItemArray[10].ToString());
                    mySql += AddRowForClientDetail("", "", "Date of receiving", Convert.ToDateTime(dt.Rows[0].ItemArray[6].ToString()).ToShortDateString());
                    mySql += AddRowForClientDetail("", "", "Date of testing", Convert.ToDateTime(dt.Rows[0].ItemArray[7].ToString()).ToShortDateString());
                    mySql += AddRowForClientDetail("", "", "Supplier name", dt.Rows[0].ItemArray[8].ToString());
                    break;
                case "PT":
                    //IssueDate,SetOfBlocks,refno,DateofReceiving,testDate,castdate,billno,solidType";
                    //   3         4         5         6            7          8     9      10
                    mySql += AddRowForClientDetail("Site name", dt.Rows[0].ItemArray[2].ToString(), "Bill No.", "DT-" + dt.Rows[0].ItemArray[9].ToString());

                    mySql += AddRowForClientDetail("Type of block", dt.Rows[0].ItemArray[10].ToString(), "Date of casting", dt.Rows[0].ItemArray[8].ToString());
                    mySql += AddRowForClientDetail("", "", "Date of testing", Convert.ToDateTime(dt.Rows[0].ItemArray[7].ToString()).ToShortDateString());
                    // check for date                             
                    mySql += AddRowForClientDetail("", "", "Date of casting", dt.Rows[0].ItemArray[8].ToString());
                    break;
                case "NDT":
                    //ReportIssueDate 3,Record_No 4,DateOfReceiving 4,DateOfTesting 6,NatureOfTesting 7,KindAttention 8,BillNo 9";
                    mySql += AddRowForClientDetail("Site name", dt.Rows[0].ItemArray[2].ToString(), "Bill No.", "DT-" + dt.Rows[0].ItemArray[9].ToString());
                    mySql += AddRowForClientDetail("Kind Attention", dt.Rows[0].ItemArray[8].ToString(), "Date of testing", Convert.ToDateTime(dt.Rows[0].ItemArray[6].ToString()).ToShortDateString());
                    break;
                case "MF":
                    //IssueDate 3,setOfMF 4,DateOfReceiving 5,ReportTitle 6,MDpara 7,SPReq 8,BillNo 9";
                    mySql += AddRowForClientDetail("Site name", dt.Rows[0].ItemArray[2].ToString(), "Date ", Convert.ToDateTime(dt.Rows[0].ItemArray[3].ToString()).ToShortDateString());
                    break;
                case "PILE":
                    //DateOfIssue,SetOfPile,refno,DateofReceiving,DateofTesting,billno,Description,A,b,c,d,KindAttention";
                    //    3         4         5           6         7            8         9       10        14
                    mySql += AddRowForClientDetail("Site name", dt.Rows[0].ItemArray[2].ToString(), "Bill No.", "DT-" + dt.Rows[0].ItemArray[8].ToString());
                    mySql += AddRowForClientDetail("Description", dt.Rows[0].ItemArray[9].ToString(), "Date of receiving", Convert.ToDateTime(dt.Rows[0].ItemArray[6].ToString()).ToShortDateString());
                    mySql += AddRowForClientDetail("", "", "Date of testing", Convert.ToDateTime(dt.Rows[0].ItemArray[7].ToString()).ToShortDateString());
                    mySql += AddRowForClientDetail("", "", "Kind Attention", dt.Rows[0].ItemArray[dt.Columns.Count - 1].ToString());
                    break;
                case "TILE":
                    mySql += AddRowForClientDetail("Site name", dt.Rows[0].ItemArray[2].ToString(), "Bill No.", "DT-" + dt.Rows[0].ItemArray[8].ToString());
                    mySql += AddRowForClientDetail("Description", dt.Rows[0].ItemArray[9].ToString(), "Date of receiving", Convert.ToDateTime(dt.Rows[0].ItemArray[6].ToString()).ToShortDateString());
                    mySql += AddRowForClientDetail("", "", "Date of testing", Convert.ToDateTime(dt.Rows[0].ItemArray[7].ToString()).ToShortDateString());
                    mySql += AddRowForClientDetail("", "", "Tile type", dt.Rows[0].ItemArray[10].ToString());
                    break;
                case "WT":
                    mySql += AddRowForClientDetail("Site name", dt.Rows[0].ItemArray[2].ToString(), "Bill No.", "DT-" + dt.Rows[0].ItemArray[9].ToString());
                    mySql += AddRowForClientDetail("Description ", dt.Rows[0].ItemArray[8].ToString(), "Date of receiving", Convert.ToDateTime(dt.Rows[0].ItemArray[6].ToString()).ToShortDateString());
                    mySql += AddRowForClientDetail("Supplier Name", dt.Rows[0].ItemArray[10].ToString(), "Date of testing", Convert.ToDateTime(dt.Rows[0].ItemArray[7].ToString()).ToShortDateString());
                    //_mRcvdDate = Convert.ToDateTime(dt.Rows[0].ItemArray[6].ToString()).ToShortDateString().ToString();
                    _mRcvdDate = Convert.ToDateTime(dt.Rows[0].ItemArray[6].ToString()).ToString();
                    break;

            }
        }
        //rptStr += myData.AddRowForClientDetail("1", "2", "3", "4");
        dt.Clear();

        CloseConnection();
        mySql += "</table>";
        return mySql;
    }
    //cube test report -1
    public String DisplayCubeTestReport(string referenceNo)
    {
        DataSet ds = new DataSet();
        DataTable dt = new DataTable();
        string mySql, myQueStr;
        Int32 cnt;
        myQueStr = "select c.id_mark,c.age,c.s1 as len,c.s2 as ht,c.s3 as bt,c.wt as weight,";
        myQueStr += " c.C_S_Area as csArea,c.density,c.Reading as load,c.comp_str as compstr,b.avg_str as avg123 , a.Record_No as RecNo, a.Grade_Of_Concrete as grade";
        myQueStr += " from cube_in_record as a,record_avg_str as b,cube_testing_lab  as c  ";
        myQueStr += " where a.cube_inward_id=c.cube_inward_id and  b.avg_id = c.avg_id  ";
        myQueStr += " and a.refno='" + referenceNo + "'";

        SqlDataAdapter da = new SqlDataAdapter(myQueStr, cn);
        da.Fill(ds);
        dt = ds.Tables[0];
        ds.Dispose();
        cnt = 0;
        dt.Dispose();

        myQueStr = "SELECT * FROM Rec_Total_qty where record_no = " + dt.Rows[0].ItemArray[11].ToString() + " and record_type = 'CT'";
        da = new SqlDataAdapter(myQueStr, cn);
        ds = new DataSet();
        da.Fill(ds);
        DataTable dt1 = new DataTable();
        dt1 = ds.Tables[0];
        ds.Dispose();
        decimal mAvg = 0;
        int mGrade = 0;
        string testType = dt1.Rows[0]["TestType"].ToString();

        int mAge = Convert.ToInt32(dt.Rows[0].ItemArray[1]);
        if (decimal.TryParse(dt.Rows[0].ItemArray[10].ToString(), out mAvg) == true)
            mAvg = Convert.ToDecimal(dt.Rows[0].ItemArray[10]);
        if (int.TryParse(dt.Rows[0].ItemArray[12].ToString().Replace("M ", ""), out mGrade) == true)
            mGrade = Convert.ToInt32(dt.Rows[0].ItemArray[12].ToString().Replace("M ", ""));

        if (testType == "Site Cube Casting")
            testType = "Concrete Cube";
        else if (testType == "")
            testType = "Concrete Cube";
        if (testType == "Concrete Cube")
        {
            if (mAge >= 3 && mAge < 28 && mAvg > 0 && mGrade > 0)
            {
                testType = "*</br>";
            }
            else
                testType = "";
        }

        mySql = "";

        mySql = "<tr>";
        mySql += "<td width=6% height=19 align=center >&nbsp;</td>";
        mySql += "<td width=79% valign=top colspan=2 height=30 align=left>";
        mySql += "<table border=1 cellpadding=0 cellspacing=0 style=border-collapse: collapse bordercolor=#111111 width=90% id=AutoNumber1>";
        mySql += "<tr>";
        mySql += "<td width=6% align=center valign=top height=19 style=border-bottom:none><font size=2><b>Sr. No. </b></font></td>";
        mySql += "<td width=14% align=center valign=top height=19 style=border-bottom:none;border-top:none><font size=2><b>ID Mark on Sample</b></font></td>";
        mySql += "<td width=4% align=center valign=top height=19 style=border-bottom:none;border-top:none><font size=2><b> Age</b></font></td>";
        mySql += "<td width=21% align=center valign=top height=19 style=border-bottom:none;border-top:none><font size=2><b> Size of specimen</b></font></td>";
        mySql += "<td width=7% align=center valign=top height=19 style=border-bottom:none;border-top:none><font size=2><b> Weight</b></font></td>";
        mySql += "<td width=9% align=center valign=top height=19 style=border-bottom:none;border-top:none><font size=2><b>C/s area </b></font></td>";
        mySql += "<td width=9% align=center valign=top height=19 style=border-bottom:none;border-top:none><font size=2><b> Density</b></font></td>";
        mySql += "<td width=8% align=center valign=top height=19 style=border-bottom:none;border-top:none><font size=2><b> Load</b></font></td>";
        mySql += "<td width=8% align=center valign=top height=19 style=border-bottom:none;border-top:none><font size=2><b> Comp. strength </b></font></td>";
        mySql += "<td width=14% align=center valign=top height=19 style=border-top:none;border-bottom:none><font size=2><b> Avg. comp. strength </b></font></td>";
        mySql += "</tr>";
        mySql += "<td width=6% align=center valign=bottom height=19 style=border-top:none><font size=2><b>&nbsp;</b></font></td>";
        mySql += "<td width=14% align=center valign=bottom height=19 style=border-top:none><font size=2><b>&nbsp; </b></font></td>";
        mySql += "<td width=4% align=center valign=bottom height=19 style=border-top:none><font size=2><b>(Days)</b></font></td>";
        mySql += "<td width=21% align=center valign=bottom height=19 style=border-top:none><font size=2><b>(mm)</b></font></td>";
        mySql += "<td width=7% align=center valign=bottom height=19 style=border-top:none><font size=2><b>(kg)</b></font></td>";
        mySql += "<td width=9% align=center valign=bottom height=19 style=border-top:none><font size=2><b>(mm<sup>2</sup>)</b></font></td>";
        mySql += "<td width=9% align=center valign=bottom height=19 style=border-top:none><font size=2><b>(kg/m<sup>3</sup>)</b></font></td>";
        mySql += "<td width=8% align=center valign=bottom height=19 style=border-top:none><font size=2><b>(kN)</b></font></td>";
        mySql += "<td width=8% align=center valign=bottom height=19 style=border-top:none><font size=2><b>(N/mm<sup>2</sup>)</b></font></td>";
        mySql += "<td width=14% align=center valign=bottom height=19 style=rorder-right:none;border-top:none><font size=2><b>(N/mm<sup>2</sup>)</b></font></td>";
        mySql += "<br>";


        for (Int32 i = 0; i < dt.Rows.Count; i++)
        {
            //tmpStr = dt.Rows[i].ItemArray[0].ToString();
            cnt = cnt + 1;
            mySql += "<tr>";
            mySql += "<td align=center valign=center height=19><font size=2>" + (cnt).ToString() + "</font></td>"; // srno
            mySql += "<td align=center valign=center height=19><font size=2>" + dt.Rows[i].ItemArray[0].ToString() + "</font></td>"; //idmark
            mySql += "<td align=center valign=center height=19><font size=2>" + dt.Rows[i].ItemArray[1].ToString() + "</font></td>"; //age
            //specifications ht *wt *bt
            mySql += "<td align=center valign=center height=19><font size=2>" + dt.Rows[i].ItemArray[2].ToString() + " X " + dt.Rows[i].ItemArray[3].ToString() + " X " + dt.Rows[i].ItemArray[4].ToString() + "</font></td>";
            mySql += "<td align=center valign=center height=19><font size=2>" + dt.Rows[i].ItemArray[5].ToString() + "</font></td>"; //wt
            mySql += "<td align=center valign=center height=19><font size=2>" + dt.Rows[i].ItemArray[6].ToString() + "</font></td>"; //cs area
            mySql += "<td align=center valign=center height=19><font size=2>" + dt.Rows[i].ItemArray[7].ToString() + "</font></td>"; //density
            mySql += "<td align=center valign=center height=19><font size=2>" + dt.Rows[i].ItemArray[8].ToString() + "</font></td>"; //load
            mySql += "<td align=center valign=center height=19><font size=2>" + dt.Rows[i].ItemArray[9].ToString() + "</font></td>"; //comp str
            if (i == 0)
            {    //<td width=8% align=center valign=bottom height=19 style=border-left:none;border-top:none><font size=2>
                mySql += "<td align=center valign=center height=19 rowspan=" + dt.Rows.Count.ToString() + "><font size=2>" + testType + dt.Rows[i].ItemArray[10].ToString() + "</font></td>"; //avg str
            }
            mySql += "</tr>";
        }
        mySql += "</table>";
        mySql += "</td></tr>";
        mySql += "<tr>";
        mySql += "<td width=6% height=19 align=center >&nbsp;</td>";
        mySql += "<td width=79% valign=top colspan=2 height=30 align=left>";
        return mySql;
    }
    //cement & fly ash test report 1+1
    public String displayCementTestReport(string rType, string referenceNo)
    {
        string mySql, myQueStr;
        DataSet ds = new DataSet();
        DataTable dt = new DataTable();

        //table defination
        mySql = "<tr>";
        mySql += "<td width=6% height=19 align=center >&nbsp;</td>";
        mySql += "<td width=79% valign=top colspan=2 height=30 align=left>";
        mySql += "<table border=1 cellpadding=0 cellspacing=0 style=border-collapse: collapse bordercolor=#111111 width=90% id=AutoNumber1>";
        mySql += "<tr><td width=30% valign=center align=left height=19><font size=2> &nbsp; Name of test</font></td>";
        mySql += "<td width=12% valign=center align=center height=19><font size=2>Result</font></td>";
        mySql += "<td width=28% valign=center align=left height=19><font size=2>&nbsp; Specified limits</font></td>";
        mySql += "<td width=30% valign=center align=left height=19><font size=2>&nbsp; Method of testing</font></td></tr>";

        // test detail printed

        if (rType == "CEMT")
        {
            myQueStr = "select a.NameOfTest as nameOfTesting,a.Cem_Result as CementResult,a.Spec_Lim as SpecificLimit,a.Method_Of_Testing as MethodofTesting" +
                       " From Cement_tests_lab  as a,cement_inward as b where a.cem_id=b.cem_id and" +
                       " b.refno='" + referenceNo + "'";
        }
        else
        {
            myQueStr = "select d.part as NameOfTesting,a.TestResult as Result,c.SpecifiedLimit,c.MethodofTesting" +
                        " From FlyAsh_Testing  as a,fly_Ash_inward as b,flyAsh_spec as c,rate_list as d" +
                        " where a.flyash_id=b.fly_ash_id  and a.rate_id=c.rateid and a.rate_id=d.rate_id " +
                         " and b.refno='" + referenceNo + "'";
        }


        SqlDataAdapter ad = new SqlDataAdapter(myQueStr, cn);
        ad.Fill(ds);
        dt = ds.Tables[0];
        ds.Dispose();


        for (Int32 i = 0; i < dt.Rows.Count; i++)
        {
            mySql += "<tr><td align=left valign=center height=19><font size=2>" + "&nbsp; " + dt.Rows[i].ItemArray[0].ToString() + "</font></td>";
            myQueStr = dt.Rows[i].ItemArray[1].ToString();
            if (myQueStr.Contains("#") == true)
            {

                myQueStr = myQueStr.Substring(0, myQueStr.IndexOf("#"));
            }
            mySql += "<td align=center valign=center height=19><font size=2>" + myQueStr + "</font></td>";

            mySql += "<td align=left valign=center height=19><font size=2>" + "&nbsp; " + dt.Rows[i].ItemArray[2].ToString() + "</font></td>";
            mySql += "<td align=left valign=center height=19><font size=2>" + "&nbsp; " + dt.Rows[i].ItemArray[3].ToString() + "</font></td></tr>";
        }
        return mySql;
    }
    // aggregate report -1
    public String displayAggtTestReport(string referenceNo)
    {
        string mySql, myQueStr;
        string aggrName;
        string FM;
        DataSet ds = new DataSet();
        DataTable dt = new DataTable();
        DataSet ds1 = new DataSet();

        myQueStr = "select  b.Aggr_Name as AggtName,a.Total_Wt_Aggr   as FinenessModulus," +
                "a.AggImpVal   as ImpactValue,a.AggCrushVal   as CrushingValue," +
                "a.[M/C] as ConditionOfTheSample,a.Sp_Grav   as SpecificGravity," +
                "a.[M/C] as MoistureContent,a.Water_Abs   as WaterAbsorption,a.LBD  as LooseBulkDensity," +
                "a.Flak_Ind   as FlakinessIndex,a.Silt_Content   as MaterialFinerThan75,a.ElongInd   as ElongationIndex" +
                " from  sand_properties_table as a,aggregate_Table as b " +
                " where a.aggr_id=b.aggr_id and b.refNo='" + referenceNo + "'";
        SqlDataAdapter ad = new SqlDataAdapter(myQueStr, cn);
        ad.Fill(ds);
        dt = ds.Tables[0];
        ds.Dispose();

        aggrName = dt.Rows[0].ItemArray[0].ToString();
        FM = dt.Rows[0].ItemArray[1].ToString();
        mySql = "";
        mySql += "<table border=0 cellpadding=0 cellspacing=0 style=border-collapse: collapse bordercolor=#111111 width=90% id=AutoNumber1>";
        mySql += "<tr>";
        myQueStr = dt.Rows[0].ItemArray[2].ToString();
        if (myQueStr != "")
        { myQueStr = "Impact value = " + myQueStr; }
        mySql += "<td width=25% align=left valign=top height=12 style=border-left:none><font size=2>" + myQueStr + " </font></td>";
        mySql += "<td width=25% align=left valign=top height=12 style=border-left:none><font size=2>" + "&nbsp;" + " </font></td>";
        myQueStr = dt.Rows[0].ItemArray[3].ToString();
        if (myQueStr != "")
        { myQueStr = "Crushing value = " + myQueStr; }
        mySql += "<td width=25% align=left valign=top height=12 style=border-left:none><font size=2>" + myQueStr + " </font></td>";
        mySql += "</tr>";

        //
        mySql += "<tr>";
        myQueStr = dt.Rows[0].ItemArray[4].ToString();
        if (myQueStr != "")
        { myQueStr = "Condition of the sample = " + myQueStr; }
        mySql += "<td width=25% align=left valign=top height=12 style=border-left:none><font size=2>" + myQueStr + " </font></td>";
        mySql += "<td width=25% align=left valign=top height=12 style=border-left:none><font size=2>" + "&nbsp;" + " </font></td>";
        myQueStr = dt.Rows[0].ItemArray[5].ToString();
        if (myQueStr != "")
        { myQueStr = "Specific gravity = " + myQueStr; }
        mySql += "<td width=25% align=left valign=top height=12 style=border-left:none><font size=2>" + myQueStr + " </font></td>";
        mySql += "</tr>";
        //

        mySql += "<tr>";
        myQueStr = dt.Rows[0].ItemArray[6].ToString();
        if (myQueStr != "")
        { myQueStr = "Moisture content = " + myQueStr; }
        mySql += "<td width=25% align=left valign=top height=12 style=border-left:none><font size=2>" + myQueStr + " </font></td>";
        mySql += "<td width=25% align=left valign=top height=12 style=border-left:none><font size=2>" + "&nbsp;" + " </font></td>";
        myQueStr = dt.Rows[0].ItemArray[7].ToString();
        if (myQueStr != "")
        { myQueStr = "Water absorption = " + myQueStr; }
        mySql += "<td width=25% align=left valign=top height=12 style=border-left:none><font size=2>" + myQueStr + " </font></td>";
        mySql += "</tr>";
        //
        mySql += "<tr>";
        myQueStr = dt.Rows[0].ItemArray[8].ToString();
        if (myQueStr != "")
        { myQueStr = "Loose bulk density = " + myQueStr; }
        mySql += "<td width=25% align=left valign=top height=12 style=border-left:none><font size=2>" + myQueStr + " </font></td>";
        mySql += "<td width=25% align=left valign=top height=12 style=border-left:none><font size=2>" + "&nbsp;" + " </font></td>";
        myQueStr = dt.Rows[0].ItemArray[9].ToString();
        if (myQueStr != "")
        { myQueStr = "Flakiness index = " + myQueStr; }
        mySql += "<td width=25% align=left valign=top height=12 style=border-left:none><font size=2>" + myQueStr + " </font></td>";
        mySql += "</tr>";
        //
        mySql += "<tr>";
        myQueStr = dt.Rows[0].ItemArray[10].ToString();
        if (myQueStr != "")
        { myQueStr = "Material Finer than 75 micron = " + myQueStr; }
        mySql += "<td width=25% align=left valign=top height=12 style=border-left:none><font size=2>" + myQueStr + " </font></td>";
        mySql += "<td width=25% align=left valign=top height=12 style=border-left:none><font size=2>" + "&nbsp;" + " </font></td>";
        myQueStr = dt.Rows[0].ItemArray[11].ToString();
        if (myQueStr != "")
        { myQueStr = "Elongation index = " + myQueStr; }
        mySql += "<td width=25% align=left valign=top height=12 style=border-left:none><font size=2>" + myQueStr + " </font></td>";
        mySql += "</tr>";
        //
        mySql += "</table><br>";
        dt.Rows.Clear();
        //======================================= test results other than sieve ana ends============

        myQueStr = "select b.Sieve_Sizes as [SieveSize],b.[Wt_Ret(gms)] as [WtRetained(g)]," +
                  "b.Wt_Ret as WtRetainedPer,b.Cumu_Wt_Ret as CummuWtRetainedPer,b.Cum_Pass as PassingPer," +
                  "b.IS_Pass_Lim as ISPassingLimits " +
                  " from Sieve_Sizes_Passing_Table as b,aggregate_table as a " +
                  " where b.aggr_id=a.aggr_id and a.refno='" + referenceNo + "'" +
                  " order by b.Sr_No";
        SqlDataAdapter ad1 = new SqlDataAdapter(myQueStr, cn);
        ad1.Fill(ds1);
        dt = ds1.Tables[0];
        ds1.Dispose();

        if (dt.Rows.Count > 0)
        {

            mySql += "<table border=1 cellpadding=0 cellspacing=0 style=border-collapse: collapse bordercolor=#111111 width=85% id=AutoNumber1>";
            // sieve analysis
            mySql += "<td width=16% align=center valign=top height=12 style=border-left:none><font size=2>Sieve Size </font></td>";
            mySql += "<td width=16% align=center valign=top height=12 style=border-left:none><font size=2>Wt. retained </font></td>";
            mySql += "<td width=16% align=center valign=top height=12 style=border-left:none><font size=2> Wt. retained (%)</font></td>";
            mySql += "<td width=16% align=center valign=top height=12 style=border-left:none><font size=2>Cummu Wt. retained (%)</font></td>";
            if (aggrName.Contains("mm"))
            {
                mySql += "<td width=16% align=center valign=top height=12 style=border-left:none><font size=2> Passing (%) </font></td>";
                mySql += "<td width=16% align=center valign=top height=35 style=border-left:none;border-right:none><font size=2> IS passing limits </font></td>";
            }
            else
            {
                mySql += "<td width=16% align=center valign=top height=12 style=border-left:none><font size=2 colspan=2> Passing (%) </font></td>";
            }
            mySql += "</tr>";

            // table printing
            for (Int32 i = 0; i < dt.Rows.Count; i++)
            {
                mySql += "<tr>";
                mySql += "<td width=16% align=center valign=center height=19><font size=2>" + dt.Rows[i].ItemArray[0].ToString() + "</font></td>";
                mySql += "<td width=16% align=center valign=center height=19><font size=2>" + dt.Rows[i].ItemArray[1].ToString() + "</font></td>";
                mySql += "<td width=16% align=center valign=center height=19><font size=2>" + dt.Rows[i].ItemArray[2].ToString() + "</font></td>";
                if (aggrName.Contains("mm")) // coarse aggt.
                {
                    mySql += "<td width=16% align=center valign=center height=19><font size=2>" + dt.Rows[i].ItemArray[3].ToString() + "</font></td>";
                    mySql += "<td width=16% align=center valign=center height=19><font size=2>" + dt.Rows[i].ItemArray[4].ToString() + "</font></td>";
                    mySql += "<td width=16% align=center valign=center height=19><font size=2>" + dt.Rows[i].ItemArray[5].ToString() + "</font></td>";
                }
                else  //fine aggt.
                {
                    if (dt.Rows[i].ItemArray[0].ToString() == "Total")  // print fineness modulus for sand
                    {
                        mySql += "<td width=16% align=right valign=center height=19><font size=2 ><b>" + "Fineness Modulus " + "&nbsp;" + "</b></font></td>";
                        mySql += "<td width=16% align=left valign=center height=19><font size=2 colspan=2>" + "&nbsp;&nbsp; &nbsp;&nbsp;" + FM + "</font></td>";
                    }
                    else
                    {
                        mySql += "<td width=16% align= center valign=center height=19><font size=2>" + dt.Rows[i].ItemArray[3].ToString() + "</font></td>";
                        mySql += "<td width=16% align=center  valign=center height=19><font size=2 colspan=2>" + dt.Rows[i].ItemArray[4].ToString() + "</font></td>";
                    }
                }
                mySql += "</tr>";
            }
        }
        else   // without sieve analysis
        {

        }
        mySql += "</table>";
        return mySql;

    }
    //Masonry block test report -2
    public String displaySolidTestReport(string referenceNo)
    {
        string mySql, myQueStr;
        DataSet ds = new DataSet();
        DataTable dt = new DataTable();
        double tot = 0;
        myQueStr = "select * from SolidCompStrTest as a ,SolidInward as b" +
                    " where a.solid_Id=b.solid_id and b.refno='" + referenceNo + "'";
        SqlDataAdapter ad = new SqlDataAdapter(myQueStr, cn);
        ad.Fill(ds);
        dt = ds.Tables[0];
        ds.Dispose();
        mySql = "";
        if (dt.Rows.Count > 0)
        { //table definitiona for comp str           
            mySql += "<table border=1 cellpadding=0 cellspacing=0 style=border-collapse: collapse bordercolor=#111111 width='90%' id=AutoNumber1>";
            mySql += "<tr>";
            mySql += "<td width=5% align=center valign=top height=19 style=border-left:none;border-top:none rowspan=2><font size=2>Sr.No </font></td>";
            mySql += "<td width=12% align=center valign=top height=19 style=border-left:none;border-top:none rowspan=2><font size=2> ID Mark  </font></td>";
            mySql += "<td width=10% align='center' valign='top' height=19 style=border-left:none;border-top:none rowspan=2><font size=2>Age</font></td>";
            mySql += "<td width=18% align='center' valign='top' height=19 style=border-left:none;border-top:none colspan=2><font size=2>Dimensions (mm)</font></td>";
            mySql += "<td width=15% align='center' valign='top' height= 19 style=border-left:none;border-top:none rowspan=2><font size=2>Cross section <br>area <br>(mm<sup>2</sup>)</font</td>";
            mySql += "<td width=9% align='center' valign='top' height=19 style=border-left:none;border-top:none rowspan=2><font size=2>Load&nbsp;<br><br>(kN)</font></td>";
            mySql += "<td width=10% align='center' valign='top' height=19 style=border-left:none;border-top:none rowspan=2><font size=2>Compressive strength&nbsp;&nbsp;&nbsp;&nbsp; (N/mm<sup>2</sup>)</font></td>";
            mySql += "<td width=12% align='center' valign='top' height=19 style=border-left:none;border-right:none;border-top:none rowspan=2><font size=2>Average strength&nbsp; (N/mm<sup>2</sup>)</font></td>";
            mySql += "</tr>";
            mySql += "<tr><td width=11% align='center' valign='top' height=19 style=border-left:none;border-top:none><font size=2>length</font></td>";
            mySql += "<td width=9% align='center' valign='top' height=19 style=border-left:none;border-top:none><font size=2>width</font></td>";
            /// result printing

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DataRow rw = dt.Rows[i];
                mySql += "<tr><td align=center valign=center height=19><font size=2>" + rw["SrNo"].ToString() + "</font></td>";
                mySql += "<td align=center valign=center height=19><font size=2>" + rw["Solid_IdMark"].ToString() + "</font></td>";
                mySql += "<td align=center valign=center height=19><font size=2>" + rw["Age"].ToString() + "</font></td>";
                mySql += "<td align=center valign=center height=19><font size=2>" + rw["Length"].ToString() + "</font></td>";
                mySql += "<td align=center valign=center height=19><font size=2>" + rw["Width"].ToString() + "</font></td>";
                double mLength = Convert.ToInt32(rw["Length"].ToString()) * Convert.ToInt32(rw["Width"].ToString());
                mySql += "<td align=center valign=center height=19><font size=2>" + mLength.ToString() + "</font></td>";
                myQueStr = rw["Load"].ToString();
                mySql += "<td align=center valign=center height=19><font size=2>" + Convert.ToDouble(myQueStr).ToString("F") + "</font></td>";

                double compStr = Convert.ToDouble(rw["Load"]) / mLength * 1000;
                mySql += "<td align=center valign=center height=19><font size=2>" + compStr.ToString("F") + "</font></td>";
                tot = tot + Convert.ToDouble((compStr.ToString("F")));
                if (i == 0)
                {
                    mySql += "<td align=center valign=center height=19 rowspan=" + dt.Rows.Count + "><font size=2>Result***</font></td>";
                }
                if (i == dt.Rows.Count - 1)
                {
                    if (dt.Rows.Count == 8)
                    {
                        tot = tot / dt.Rows.Count;
                        mySql = mySql.Replace("Result***", tot.ToString("F"));
                    }
                    else
                    {
                        mySql = mySql.Replace("Result***", "***");
                    }
                }
                mySql += "</tr>";
            }
            mySql += "</table>";

        }
        dt.Reset();

        //Water Absorbtion
        myQueStr = "select * from SolidWATest as a ,SolidInward as b" +
                    " where a.solid_Id=b.solid_id and b.refno='" + referenceNo + "'";

        SqlDataAdapter ad1 = new SqlDataAdapter(myQueStr, cn);
        ad1.Fill(ds);
        dt = ds.Tables[0];
        ds.Dispose();
        if (dt.Rows.Count > 0)
        {
            mySql += "<table border=1 cellpadding=0 cellspacing=0 style=border-collapse: collapse bordercolor=#111111 width=85% id=AutoNumber1>";
            mySql += "<tr>";
            mySql += "<td width=5% align=center valign=top height=19 style=border-left:none;border-top:none><font size=2>Sr. No</font></td>";
            mySql += "<td width=25% align=center valign=top height=19 style=border-left:none;border-top:none><font size=2> ID Mark </font></td>";
            mySql += "<td width=12% align=center valign=top height=19 style=border-left:none;border-top:none><font size=2>Dry weight<br>(g)</font></td>";
            mySql += "<td width=12% align=center valign=top height=19 style=border-left:none;border-top:none><font size=2> Wet weight <br>(g)</font></td>";
            mySql += "<td width=16% align=center valign=top height=19 style=border-left:none;border-top:none><font size=2> Water absorption <br>(%)</font></td>";
            mySql += "<td width=24% align=center valign=top height=19 style=border-left:none;border-right:none;border-top:none><font size=2>Average water absorption <br>(%)</font></td>";
            //table data start
            tot = 0;
            double mDryWt;
            double mWetWt, mWAPer;
            for (Int32 i = 0; i < dt.Rows.Count; i++)
            {
                DataRow rw = dt.Rows[i];
                mySql += "<tr><td align=center valign=center height=19><font size=2>" + rw["SrNo"].ToString() + "</font></td>";
                mySql += "<td align=center valign=center height=19><font size=2>" + rw["Solid_IdMark"].ToString() + "</font></td>";

                string[] strVal1 = rw["Weights"].ToString().Split('|');
                mDryWt = Convert.ToDouble(strVal1[0].ToString().Replace(",", ""));
                mWetWt = Convert.ToDouble(strVal1[1].ToString().Replace(",", ""));
                mWAPer = 0;
                mWAPer = 100 * (mWetWt - mDryWt) / mDryWt;
                tot = tot + mWAPer;
                mySql += "<td align=center valign=center height=19><font size=2>" + mDryWt.ToString() + "</font></td>";
                mySql += "<td align=center valign=center height=19><font size=2>" + mWetWt.ToString() + "</font></td>";
                mySql += "<td align=center valign=center height=19><font size=2>" + mWAPer.ToString("F") + "</font></td>";
                if (i == 0)
                {
                    mySql += "<td align=center valign=center height=19 rowspan=" + dt.Rows.Count + "><font size=2>Result***</font></td>";
                }
                if (i == dt.Rows.Count - 1)
                {
                    if (dt.Rows.Count == 3)
                    {
                        mWAPer = tot / dt.Rows.Count;
                        mySql = mySql.Replace("Result***", mWAPer.ToString("F"));
                    }
                    else
                    {
                        mySql = mySql.Replace("Result***", "***");
                    }
                }

                mySql += "</tr>";
            }
            mySql += "</table>";
        }

        return mySql;

    }
    //core test report -1
    public String displayCoreTestReport(string referenceNo, string rType)
    {
        string mySql, myQueStr;
        DataSet ds = new DataSet();
        DataSet ds1 = new DataSet();
        DataTable dt = new DataTable();
        DataTable dtTitles = new DataTable();
        SqlDataAdapter ad1 = new SqlDataAdapter();
        DataSet ds2 = new DataSet();
        SqlCommand cmd = new SqlCommand(); 
        Int32 maxCols = 0;
        string ndtType = "";
        mySql = "";
        switch (rType)
        {
            case "CR":
                mySql = "<td width=2% height=19>&nbsp;</td>";
                mySql += "<td width=76% height=19 colspan=12>";
                mySql += "<table border=1 cellpadding=0 cellspacing=0 style='border-collapse: collapse; text-align: center' bordercolor=#111111 width='95%' id='AutoNumber1' height=29>";
                mySql += "<tr><td width='4%' valign='top' height=29><font size=2>Sr.No.</font></td>";
                mySql += "<td width='16%' valign='top' height=29><font size=2>ID Mark</font></td>";
                mySql += "<td width='5%' valign='top' height=29><font size=2>Core<br> Diameter <br><br><br>(mm)</font></td>";
                mySql += "<td width='14%' valign='top' height=29><font size=2>Date Of Casting</font></td>";
                mySql += "<td width='8%' valign='top' height=29><font size=2>Age of Concrete<br><br><br>(Days)</font></td>";
                mySql += "<td width='10%' valign='top' height=29><font size=2>Area of Cross Section<br><br><br>(mm<sup>2</sup>)</font></td>";
                mySql += "<td width='10%' valign='top' height=29><font size=2>Weight before Capping <br><br><br>(kg/m<sup>3</sup>)</font></td>";
                mySql += "<td width='8%' valign='top' height=29><font size=2>Density of Concrete<p><br>(Kg/m<sup>3</sup>)</font></td>";
                mySql += "<td width='8%' valign='top' height=29><font size=2>Load at <br> failure <br><br><br>(kN)</font></td>";
                mySql += "<td width='8%' valign='top' height=29><font size=2>Comp. Strength&nbsp;<p>(N/mm<sup>2</sup>)</font></td>";
                mySql += "<td width='8%' valign='top' height=29><font size=2>Corrected Comp.<br>Strength <br><br>(N/mm<sup>2</sup>)</font></td>";
                mySql += "<td width='12%' valign='top' height=29><font size=2>Equivalent Cube Comp Strength &nbsp;&nbsp;&nbsp;(N/mm<sup>2</sup>)</font></td></tr>";

                myQueStr = "select * from core_test_master as a,core_test_lab as b " +
                        "where a.title_id=b.title_id and  a.refno='" + referenceNo + "'";
                SqlDataAdapter ad = new SqlDataAdapter(myQueStr, cn);
                ad.Fill(ds);
                dt = ds.Tables[0];
                ds.Dispose();
                DataRow rw1 = dt.Rows[0];
                mySql += "<tr><td  align='center' height=29 colspan=12><font size=2>" + rw1["Title"].ToString() + "</font></td></tr>";

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    DataRow rw = dt.Rows[i];
                    mySql += "<tr><td width='4%' valign='top' height=29><font size=2>" + rw["Sr_No"].ToString() + "</font></td>";
                    mySql += "<td width='16%' valign='top' height=29><font size=2>" + rw["Descr"].ToString() + "</font></td>";
                    mySql += "<td width='5%' valign='top' height=29><font size=2>" + rw["Dia"].ToString() + "</font></td>";
                    mySql += "<td width='14%' valign='top' height=29><font size=2>" + rw["Date_Of_Casting"].ToString() + "</font></td>";
                    mySql += "<td width='8%' valign='top' height=29><font size=2>" + rw["Age"].ToString() + "</font></td>";
                    mySql += "<td width='10%' valign='top' height=29><font size=2>" + rw["CS_Area"].ToString() + "</font></td>";
                    mySql += "<td width='10%' valign='top' height=29><font size=2>" + rw["Wt"].ToString() + "</font></td>";
                    mySql += "<td width='8%' valign='top' height=29><font size=2>" + rw["Density"].ToString() + "</font></td>";
                    mySql += "<td width='8%' valign='top' height=29><font size=2>" + rw["Read"].ToString() + "</font></td>";
                    mySql += "<td width='8%' valign='top' height=29><font size=2>" + rw["Comp_Str"].ToString() + "</font></td>";
                    mySql += "<td width='8%' valign='top' height=29><font size=2>" + rw["CorrCompStr"].ToString() + "</font></td>";
                    mySql += "<td width='12%' valign='top' height=29><font size=2>" + rw["Eq_Str"].ToString() + "</font></td></tr>";
                }
                mySql += "</table>";
                mySql += "<br><tr><td width=2% height=19></td><td width=77% height=19 colspan=8 ><font size=2><b>GENERAL INFORMATION & MODE OF FAILURE</b></font></td></tr>";
                mySql += "<td width=2% height=19>&nbsp;</td>";
                mySql += "<td width=77% height=19 colspan=11>";
                mySql += "<table border=1 cellspacing=0 width=575 style= 'border-collapse: collapse' bordercolor=#111111 cellpadding=0>";
                mySql += "<tr><td width='63' valign='top' height=29 rowspan=2><p align='center'><font size=2>Sr. No.</font></td>";
                mySql += "<td width=136 valign='top' height=29 rowspan=2><p align='center'><font size=2>ID Mark</font></td>";
                mySql += "<td width='155' valign='top' height=15 colspan=2><p align='center'><font size=2>Correction factor</font></td>";
                mySql += "<td width='150' valign='top' height=15 colspan=2><p align='center'><font size=2>Core lengths (mm)</font></td>";
                mySql += "<td width=150 valign='top' height=29 rowspan=2><p align='center'><font size=2>Mode of Failure</font></td>";

                mySql += "<tr><td width=78 valign='top' height=14><p align='center'><font size=2>L/D </font></td>";
                mySql += "<td width=77 valign='top' height=14><p align='center'><font size=2>Diameter</font></td>";
                mySql += "<td width=80 valign='top' height=14><p align='center'><font size=2>original</font></td>";
                mySql += "<td width=70 valign='top' height=14><p align='center'><font size=2>with cap</font></td>";

                mySql += "<tr><td  align='center' height=29 colspan=8><font size=2>" + rw1["Title"].ToString() + "</font></td></tr>";

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    DataRow rw2 = dt.Rows[i];
                    mySql += "<tr>";
                    //srno & desc
                    mySql += "<td align=center valign=center height=19><font size=2>" + rw2["Sr_No"].ToString() + "</font></td>";
                    mySql += "<td align=center valign=center height=19><font size=2>" + rw2["Descr"].ToString() + "</font></td>";
                    //L/D - corrL/D 
                    myQueStr = "&nbsp;";
                    if (rw2["LenWithCaping"].ToString() != null)
                    {
                        myQueStr = (Convert.ToSingle(rw2["LenWithCaping"].ToString()) / Convert.ToSingle(rw2["Dia"].ToString())).ToString();
                        myQueStr = (0.106 * Convert.ToSingle(myQueStr) + 0.786).ToString("0.000");
                    }

                    mySql += "<td align=center valign=center height=19><font size=2>" + myQueStr + "</font></td>";
                    // corr diameter
                    mySql += "<td align=center valign=center height=19><font size=2>" + rw2["Dia"].ToString() + "</font></td>";
                    // Lengths original and with capping.
                    mySql += "<td align=center valign=center height=19><font size=2>" + rw2["Len"].ToString() + "</font></td>";
                    mySql += "<td align=center valign=center height=19><font size=2>" + rw2["LenWithCaping"].ToString() + "</font></td>";
                    // mode of failure.
                    mySql += "<td align=center valign=center height=19><font size=2>" + rw2["Modeoffail"].ToString() + "</font></td>";

                    mySql += "</tr>";
                }
                mySql += "</table>";
                break;
            case "NDT":
                myQueStr = "select Nature_Of_Testing,Title_Id,Title from NDT_Table ";
                myQueStr += " where record_no=" + Convert.ToDouble(referenceNo).ToString("0");
                cmd.CommandText = myQueStr;
                cmd.Connection = cn;
                ad1.SelectCommand = cmd;
                ad1.Fill(ds1);
                dtTitles = ds1.Tables[0];
                ndtType = dtTitles.Rows[0].ItemArray[0].ToString();
                ds1.Dispose();
                ad1.Dispose();
                double[] mTitles = new double[dtTitles.Rows.Count];
                for (int i = 0; i < dtTitles.Rows.Count; i++)
                {
                    mTitles[i] = Convert.ToDouble(dtTitles.Rows[i].ItemArray[1].ToString());
                }
                mySql = "<table border=1 cellpadding=0 cellspacing=0 style=border-collapse: collapse; border-left-width: 0; border-right-width:0 bordercolor=#111111 width=90%>";
                mySql += "<tr>";
                mySql += "<td width=4% align=center style=border-left-color: #111111; border-left-width: 1; border-bottom-style: none; border-bottom-width: medium rowspan=2 valign=top> Sr.No.</td>";
                mySql += "<td width=20%  align=center style=border-bottom-style: none; border-bottom-width: medium rowspan=2 valign=top>Description</td>";
                mySql += "<td width=6% align=center style=border-bottom-style: none; border-bottom-width: medium rowspan=2 valign=top> Grade of concrete</td>";
                mySql += "<td width=11% align=center style=border-bottom-style: none; border-bottom-width: medium rowspan=2 valign=top>Date of casting</td>";
                mySql += "<td width=6% align=center style=border-bottom-style: none; border-bottom-width: medium rowspan=2 valign=top>Age <br><br><br>(Days)</td>";
                if (ndtType == "Both")
                {
                    mySql += "<td width=18% colspan=2 align=center valign=top>Mech &nbsp; Sclerometer <br>(Rebound Hammer)</td>";
                    mySql += "<td width=11% align=center style=border-bottom-style: none; border-bottom-width: medium rowspan=2 valign=top>Pulse <br>velocity <br><br>(km/s)</td>";
                    mySql += "<td width=11% align=center style=border-right-color: #111111; border-right-width: 1; border-bottom-style: none; border-bottom-width: medium rowspan=2 valign=top>Indicative <br>strength <br><br> (N/mm<sup>2</sup>)</td>";
                    mySql += "</tr>";
                    mySql += "<tr><td width=9% style=border-left-style: none; border-left-width: medium; border-right-style: solid; border-right-width: 1; border-top-style: none; border-top-width: medium; border-bottom-style: none; border-bottom-width: medium align=center valign=top> Angle of<br> inclination</td>";
                    mySql += "<td width=9% style=border-left-style: solid; border-left-width: 1; border-right-style: none; border-right-width: medium; border-top-style: none; border-top-width: medium; border-bottom-style: none; border-bottom-width: medium align=center valign=top> Average <br> reading</td>";
                    maxCols = 9;
                }
                else if (ndtType == "DUCT")
                {
                    mySql += "<td width=11% align=center style=border-bottom-style: none; border-bottom-width: medium rowspan=2 valign=top>Pulse <br>velocity <br><br>(km/s)</td>";
                    mySql += "<td width=11% align=center style=border-right-color: #111111; border-right-width: 1; border-bottom-style: none; border-bottom-width: medium rowspan=2 valign=top>Indicative <br>strength <br><br> (N/mm<sup>2</sup>)</td><tr>";
                    maxCols = 7;
                }

                else if (ndtType == "UPV with Grading")
                {
                    mySql += "<td width=11% align=center style=border-bottom-style: none; border-bottom-width: medium rowspan=2 valign=top>Pulse <br>velocity <br><br>(km/s)</td>";
                    mySql += "<td width=11% align=center style=border-right-color: #111111; border-right-width: 1; border-bottom-style: none; border-bottom-width: medium rowspan=2 valign=top>Concrete Quality Grading</td><tr>";
                    maxCols = 7;
                }
                else
                {
                    mySql += "<td width=18% colspan=2 align=center valign=top>Mech &nbsp; Sclerometer <br>(Rebound Hammer)</td>";
                    mySql += "<td width=11% align=center style=border-right-color: #111111; border-right-width: 1; border-bottom-style: none; border-bottom-width: medium rowspan=2 valign=top>Indicative <br>strength <br><br> (N/mm<sup>2</sup>)</td>";
                    mySql += "</tr>";
                    mySql += "<tr><td width=9% style=border-left-style: none; border-left-width: medium; border-right-style: solid; border-right-width: 1; border-top-style: none; border-top-width: medium; border-bottom-style: none; border-bottom-width: medium align=center valign=top> Angle of<br> inclination</td>";
                    mySql += "<td width=9% style=border-left-style: solid; border-left-width: 1; border-right-style: none; border-right-width: medium; border-top-style: none; border-top-width: medium; border-bottom-style: none; border-bottom-width: medium align=center valign=top> Average <br> reading</td>";
                    maxCols = 8;

                }
                mySql += "</tr>";
                // ndt
                for (Int32 i = 0; i <= mTitles.Length - 1; i++)
                {
                    // title 
                    mySql += "<tr><td align=center colspan=" + maxCols.ToString() + "><b>" + dtTitles.Rows[i].ItemArray[2].ToString() + "</b></td></tr>";

                    myQueStr = "SELECT Sr_No,descr,grade,date_cast,age,angle,Scl_Avg_Read,Pulse_Vel,Ind_Str,Ind_str_Mod " +
                        " from ndt_Titlewise_table as b where b.Title_ID=" + mTitles[i].ToString("0");
                    dt.Reset();
                    //ds1.Reset();
                    cmd.CommandText = myQueStr;
                    cmd.Connection = cn;
                    ad1.SelectCommand = cmd;
                    //ad1.Fill(ds1);
                    //dt = ds1.Tables[0];
                    //ds1.Dispose();
                    //ad1.Dispose();
                    ad1.Fill(ds2);
                    dt = ds2.Tables[0];
                    ds2.Dispose();
                    ad1.Dispose();

                    if (dt.Rows[0].ItemArray[dt.Columns.Count - 1].ToString() != null)
                    {
                        if (dt.Rows[0].ItemArray[dt.Columns.Count - 1].ToString() != "")
                        {
                            for (int x = 0; x < dt.Rows.Count - 1; x++)
                            {
                                DataRow rw = dt.Rows[x];

                                if (dt.Rows[x].ItemArray[dt.Columns.Count - 1].ToString().Trim() != "")
                                {
                                    dt.Rows[x].ItemArray[dt.Columns.Count - 2] = dt.Rows[x].ItemArray[dt.Columns.Count - 1];
                                }
                            }
                        }
                    }
                    for (Int32 j = 0; j <= dt.Rows.Count - 1; j++)
                    {
                        mySql += "<tr>";

                        if (ndtType == "UPV with Grading")
                        {

                            mySql += "<td align=center valign=center height=19><font size=2>" + dt.Rows[j].ItemArray[0].ToString() + "</font></td>";
                            mySql += "<td align=center valign=center height=19><font size=2>" + dt.Rows[j].ItemArray[1].ToString() + "</font></td>";
                            mySql += "<td align=center valign=center height=19><font size=2>" + dt.Rows[j].ItemArray[2].ToString() + "</font></td>";
                            mySql += "<td align=center valign=center height=19><font size=2>" + dt.Rows[j].ItemArray[3].ToString() + "</font></td>";
                            mySql += "<td align=center valign=center height=19><font size=2>" + dt.Rows[j].ItemArray[4].ToString() + "</font></td>";
                            mySql += "<td align=center valign=center height=19><font size=2>" + dt.Rows[j].ItemArray[7].ToString() + "</font></td>";
                            mySql += "<td align=center valign=center height=19><font size=2>" + dt.Rows[j].ItemArray[8].ToString() + "</font></td>";

                        }
                        else
                        {
                            for (Int32 k = 0; k < maxCols; k++)
                            {
                                mySql += "<td align=center valign=center height=19><font size=2>" + dt.Rows[j].ItemArray[k].ToString() + "</font></td>";

                            }
                        }
                        mySql += "</tr>";
                    }
                    ad1.Dispose();
                    //dt.Rows.Clear();                    
                }
                mySql += "</table>";
                break;                
        }
        return mySql;

    }
    //brick test report -1
    public String displayBrickTestReport(string referenceNo)
    {
        string mySql;
        string queStr;
        DataSet ds = new DataSet();
        DataTable dt = new DataTable();
        SqlCommand cmd = new SqlCommand();
        double tot = 0, avgVal = 0;
        //DataTable dt1 = new DataTable();
        //DataSet ds1 = new DataSet();
        mySql = "";
        queStr = " select  * from brickCompstrTest as a,brickInward as b " +
                " where a.bt_id=b.bt_id and b.refno='" + referenceNo + "'";
        SqlDataAdapter ad = new SqlDataAdapter(queStr, cn);
        ad.Fill(ds);
        dt = ds.Tables[0];
        ds.Dispose();
        if (dt.Rows.Count > 0)
        {
            mySql += "<br><tr><td align=left valign=center height=19><font size=2>" + "Compressive strength " + "</font></td></tr>";
            //table defn                
            mySql += "<table border=1 cellspacing=0 width='90%'  style= 'border-collapse: collapse' bordercolor=#111111 cellpadding=0>";
            mySql += "<tr><td align='center' valign='center' rowspan=2 height=11><font size=2>Sr. No.</font></td>";
            mySql += "<td  align='center'  rowspan=2 height=11><font size=2>ID Mark</font></td>";
            mySql += "<td align='center'  colspan=2 height=11><font size=2>Dimension (mm)</font></td>";
            mySql += "<td align='center'  rowspan=2 height=11><font size=2>C/s<br>area<br>(mm<sup>2</sup>)</font></td>";
            mySql += "<td align='center'  rowspan=2 height=11><font size=2>Load <br><br>(kN)</font></td>";
            mySql += "<td align='center' rowspan=2 height=11><font size=2>Compressive <br>strength<br>(N/mm<sup>2</sup>)</font></td>";
            mySql += "<td align='center'  rowspan=2 height=11><font size=2>Avg.compressive<br>strength<br>(N/mm<sup>2</sup>)</font></td></tr>";
            mySql += "<tr><td align='center' valign='center' height=11><font size=2>Length</font></td>";
            mySql += "<td align='center' valign='center' height=11><font size=2>Width</font></td></tr>";
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DataRow rw = dt.Rows[i];
                mySql += "<tr><td align='center' valign='center' height=11><font size=2>" + rw["Sr_No"] + "</font></td>";
                mySql += "<td align='center' height=11><font size=2>" + rw["Id_Mark"] + "</font></td>";
                string[] strVal = rw["Dimensions"].ToString().Split(',');
                mySql += "<td align='center' height=11><font size=2>" + strVal[0] + "</font></td>";
                mySql += "<td align='center' height=11><font size=2>" + strVal[1] + "</font></td>";
                queStr = (Convert.ToDouble(strVal[0]) * Convert.ToDouble(strVal[1])).ToString("F");
                mySql += "<td align='center' height=11><font size=2>" + queStr + "</font></td>";
                mySql += "<td align='center' height=11><font size=2>" + rw["Load"] + "</font></td>";
                queStr = (Convert.ToDouble(rw["load"]) / (Convert.ToDouble(strVal[0]) * Convert.ToDouble(strVal[1])) * 1000).ToString("F");
                mySql += "<td align='center' height=11><font size=2>" + queStr + "</font></td>";
                tot = tot + Convert.ToDouble(queStr);
                if (i == 0)
                {
                    mySql += "<td align='center' rowspan=" + dt.Rows.Count.ToString() + "height=11><font size=2>Result***</font></td>";
                }
                else if (i == dt.Rows.Count - 1)
                {
                    if (dt.Rows.Count < 5)
                    {
                        mySql = mySql.Replace("Result***", "***");
                    }
                    else
                    {
                        avgVal = tot / dt.Rows.Count;
                        mySql = mySql.Replace("Result***", avgVal.ToString("F"));
                    }

                }
                mySql += "</tr>";
            }
            mySql += "</table>";
        }
        dt.Reset();

        cmd.CommandText = queStr = " select  * from brickWATest as a,brickInward as b " +
                " where a.bt_id=b.bt_id and b.refno='" + referenceNo + "'";
        ad.SelectCommand = cmd;
        cmd.Connection = cn;
        ad.Fill(ds);
        dt = ds.Tables[0];
        ds.Dispose();
        // water abs
        if (dt.Rows.Count > 0)
        {

            mySql += "<tr><td align=left valign=center height=19><font size=2>" + "Water absorption " + "</font></td></tr>";
            //table defn                
            mySql += "<table border=1 cellspacing=0 width='90%' style= 'border-collapse: collapse' bordercolor=#111111 cellpadding=0>";
            mySql += "<tr><td align='center' valign='center' rowspan=2 height=11><font size=2>Sr. No.</font></td>";
            mySql += "<td align='center'  rowspan=2 height=11><font size=2>ID Mark</font></td>";
            mySql += "<td align='center'  colspan=2 height=11><font size=2>Weight </font></td>";
            mySql += "<td align='center'  rowspan=2 height=11><font size=2>water absorption (%)</font></td>";
            mySql += "<td align='center'  rowspan=2 height=11><font size=2>Avg.water absorption (%)</font></td>";
            mySql += "<td align='center' rowspan=2 height=11><font size=2>Specified limit</font></td></tr>";
            mySql += "<tr><td align='center' valign='center' height=11><font size=2>Dry (g)</font></td>" +
                    "<td align='center' valign='center' height=11><font size=2>Wet (g)</font></td></tr>";

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DataRow rw = dt.Rows[i];
                mySql += "<tr><td align='center' valign='center' height=11><font size=2>" + rw["Sr_No"] + "</font></td>";
                mySql += "<td align='center' valign='center' height=11><font size=2>" + rw["IdMark"] + "</font></td>";
                string[] strVal = rw["Weights"].ToString().Replace(",", "").Split('|');
                mySql += "<td align='center' valign='center' height=11><font size=2>" + strVal[0].ToString() + "</font></td>";
                mySql += "<td align='center' valign='center' height=11><font size=2>" + strVal[1].ToString() + "</font></td>";
                avgVal = 100 * (Convert.ToDouble(strVal[1].ToString()) - Convert.ToDouble(strVal[0].ToString())) / Convert.ToDouble(strVal[0].ToString());
                queStr = avgVal.ToString("F");
                mySql += "<td align='center' valign='center' height=11><font size=2>" + queStr + "</font></td>";
                tot = tot + Convert.ToDouble(queStr);
                if (i == 0)
                {
                    mySql += "<td align='center' rowspan=" + dt.Rows.Count.ToString() + "height=11><font size=2>Result***</font></td>";
                    queStr = " Avg.should not be more than 20%";
                    mySql += "<td align='center' valign='center' rowspan=" + dt.Rows.Count.ToString() + " height=11><font size=2>" + queStr + "</font></td>";

                }
                else if (i == dt.Rows.Count - 1)
                {
                    if (dt.Rows.Count < 5)
                    {
                        mySql = mySql.Replace("Result***", "***");
                    }
                    else
                    {
                        avgVal = tot / dt.Rows.Count;
                        mySql = mySql.Replace("Result***", avgVal.ToString("F"));
                    }

                }
                mySql += "</tr>";
            }
            mySql += "</table>";
        }
        // Dimensions

        dt.Reset();
        cmd.CommandText = queStr = " select  * from BTDimAnalysis as a,brickInward as b " +
                " where a.bt_id=b.bt_id and b.refno='" + referenceNo + "'";
        ad.SelectCommand = cmd;
        cmd.Connection = cn;
        ad.Fill(ds);
        dt = ds.Tables[0];
        ds.Dispose();

        if (dt.Rows.Count > 0)
        {
            mySql += "<tr><td align=left valign=center height=19><font size=2>" + "Dimension analysis " + "</font></td></tr>";
            //table defn                
            mySql += "<table border=1 cellspacing=0 width='85%' style= 'border-collapse: collapse' bordercolor=#111111 cellpadding=0>";
            mySql += "<tr><td width='9%' align='center' valign='center' rowspan=2 height=11><font size=2>Sr. No.</font></td>";
            mySql += "<td width='27%' align='center' valign='center' rowspan=2 height=11><font size=2>ID Mark</font></td>";
            mySql += "<td width='64%' align='center' valign='center' colspan=3 height=11><font size=2> Dimensions </font></td></tr>";
            mySql += "<tr><td width='25%' align='center' valign='center' height=11><font size=2>Length (mm)</font></td>";
            mySql += "<td width='18%' align='center' valign='center' height=11><font size=2>Width   (mm)</font></td>";
            mySql += "<td width='21%' align='center' valign='center' height=11><font size=2> Height (mm)</font></td></tr>";
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DataRow rw = dt.Rows[i];
                mySql += "<tr><td width='9%' align='center' valign='center'height=11><font size=2>" + rw["Sr_No"] + "</font></td>";
                mySql += "<td width='27%' align='center' valign='center' height=11><font size=2>" + rw["Id_Mark"] + "</font></td>";
                string[] strVal = rw["Dimensions"].ToString().Split('|');
                //a,b,c,|p,q,r,|x,y,z,
                for (int j = 0; j < strVal.Length; j++)
                {
                    string[] strVal1 = strVal[j].Split(',');
                    avgVal = 0;
                    for (int k = 0; k < strVal1.Length; k++)
                    {
                        avgVal = avgVal + Convert.ToDouble(strVal1[k]);
                    }
                    mySql += "<td width='25%' align='center' valign='center' height=11><font size=2>" + avgVal.ToString("####") + "</font></td>";
                }
                mySql += "</tr>";
            }
            mySql += "</tr>";
            mySql += "</table>";

        }
        // efflsn
        dt.Reset();
        cmd.CommandText = queStr = " select  * from BrickEffloreTest as a,brickInward as b " +
                " where a.bt_id=b.bt_id and b.refno='" + referenceNo + "'";
        ad.SelectCommand = cmd;
        cmd.Connection = cn;
        ad.Fill(ds);
        dt = ds.Tables[0];
        ds.Dispose();

        if (dt.Rows.Count > 0)
        {
            mySql += "<tr><td align=left valign=center height=19><font size=2>" + "Efflorescence" + "</font></td></tr>";

            mySql += "<table border=1 cellspacing=0 width=385 style= 'border-collapse: collapse' bordercolor=#111111 cellpadding=0>";
            mySql += "<tr><td width='11%' align='center' valign='center' height=11><Font Size=2>Sr. No.</font></td>";
            mySql += "<td width='36%' align='center' valign='center' height=11><Font Size=2>ID Mark</font></td>";
            mySql += "<td width='53%' align='center' valign='center' height=11><Font Size=2>Observations</font></td></tr>";
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DataRow rw = dt.Rows[i];
                mySql += "<table border=1 cellspacing=0 width=385 style= 'border-collapse: collapse' bordercolor=#111111 cellpadding=0>";
                mySql += "<tr><td width='11%' align='center' valign='center' height=11><Font Size=2>" + rw["Sr_No"].ToString() + "</font></td>";
                mySql += "<td width='36%' align='center' valign='center' height=11><Font Size=2>" + rw["IDMark"].ToString() + "</font></td>";
                mySql += "<td width='53%' align='center' valign='center' height=11><Font Size=2>" + rw["Observation"].ToString() + "</font></td></tr>";
            }
            mySql += "</table>";

        }
        // Density

        dt.Reset();
        cmd.CommandText = queStr = " select  * from BrickDensityTest as a,brickInward as b " +
                " where a.bt_id=b.bt_id and b.refno='" + referenceNo + "'";
        ad.SelectCommand = cmd;
        cmd.Connection = cn;
        ad.Fill(ds);
        dt = ds.Tables[0];
        ds.Dispose();

        if (dt.Rows.Count > 0)
        {
            mySql += "<tr><td align=left valign=center height=19><font size=2>" + "Brick Density  " + "</font></td></tr>";
            //table defn                
            mySql += "<table border=1 cellspacing=0 width='85%'  style= 'border-collapse: collapse' bordercolor=#111111 cellpadding=0>";
            mySql += "<tr><td align='center' valign='center' rowspan=2 height=11><font size=2>Sr. No.</font></td>";
            mySql += "<td align='center'  rowspan=2 height=11><font size=2>ID Mark</font></td>";
            mySql += "<td align='center'  colspan=3 height=11><font size=2>Size of specimen <br>(mm)</br></font></td>";
            mySql += "<td align='center'  rowspan=2 height=11><font size=2>Weight<br>(kg)</br></font></td>";
            mySql += "<td align='center'  rowspan=2 height=11><font size=2>Density <br>(kg/m<sup>3</sup>)</br></font></td>";
            mySql += "<td align='center'  rowspan=2 height=11><font size=2>Average <br>Density <br>(kg/m<sup>3</sup>)</font></td></tr>";

            mySql += "<tr><td align='center' valign='center' height=11><font size=2>Length</font></td>";
            mySql += "<td align='center' valign='center' height=11><font size=2>Width</font></td>";
            mySql += "<td align='center' valign='center' height=11><font size=2>Thickness</font></td></tr>";
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DataRow rw = dt.Rows[i];
                mySql += "<tr><td align='center' valign='center' height=11><font size=2>" + rw["sr_no"].ToString() + "</font></td>";
                mySql += "<td align='center' height=11><font size=2>" + rw["Id_mark"].ToString() + "</font></td>";
                mySql += "<td align='center' height=11><font size=2>" + rw["Length"].ToString() + "</font></td>";
                mySql += "<td align='center' height=11><font size=2>" + rw["Width"].ToString() + "</font></td>";
                mySql += "<td align='center' height=11><font size=2>" + rw["Thickness"].ToString() + "</br></font></td>";
                mySql += "<td align='center' height=11><font size=2>" + Convert.ToDouble(rw["DryWt"]).ToString("0.000") + "</font></td>";
                mySql += "<td align='center' height=11><font size=2>" + rw["Density"].ToString() + "</font></td>";
                if (i == 0)
                {
                    mySql += "<td align='center' rowspan=" + dt.Rows.Count.ToString() + " height=11><font size=2>" + rw["AvgDensity"].ToString() + "</font></td>";
                }
                mySql += "</tr>";
            }
            mySql += "</table>";

        }
        dt.Dispose();
        return mySql;
    }
    //pavement test report - 4
    public String displayPavementTestReport(string referenceNo, string mySql)
    {
        //string mySql;
        string queStr;
        DataSet ds = new DataSet();
        DataTable dt = new DataTable();

        queStr = "select * ";
        queStr += " from cube_in_record as a ,cube_testing_lab as b,record_avg_str as c ";
        queStr += " where a.cube_inward_id=b.cube_inward_id and b.avg_id=c.avg_id ";
        queStr += " and a.refno='" + referenceNo + "'";
        SqlDataAdapter ad = new SqlDataAdapter(queStr, cn);
        ad.Fill(ds);
        dt = ds.Tables[0];
        ad.Dispose();
        ds.Dispose();
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            DataRow rw = dt.Rows[i];
            if (i == 0)
            {
                switch (rw["PT_test"].ToString().Trim().ToLower())
                {
                    case "compressive":
                        mySql = mySql.Replace("#Paving Block#", "Precast Concrete Block of Paving - Compressive Strength");
                        mySql += "<br>";
                        mySql += "<table border=1 cellpadding=0 cellspacing=0 style='border-collapse: collapse' bordercolor='#111111' width='85%' id='AutoNumber1'>";
                        mySql += "<tr><td width='7%' align=center valign=top height=19 style=border-left:none;border-bottom:none><font size=2>Sr. No. </font></td>";
                        mySql += "<td width='13%' align=center valign=top height=19 style=border-left:none;border-bottom:none><font size=2>ID Mark </font></td>";
                        mySql += "<td width='8%' align=center valign=top height=19 style=border-left:none;border-bottom:none><font size=2> Age<br><br>(Days)</font></td>";
                        mySql += "<td width='10%' align=center valign=top height=19 style=border-left:none;border-bottom:none><font size=2> Plan area <br><br>(mm<sup>2</sup>)</font></td>";
                        mySql += "<td width='9%' align=center valign=top height=19 style=border-left:none;border-bottom:none><font size=2> Actual Thickness <br>(mm)</font></td>";
                        mySql += "<td width='10%' align=center valign=top height=19 style=border-left:none;border-bottom:none><font size=2>Weight <br><br> (kg)</font></td>";
                        mySql += "<td width='10%' align=center valign=top height=19 style=border-left:none;border-bottom:none><font size=2> Density <br><br>(kg/m<sup>3</sup>)</font></td>";
                        mySql += "<td width='9%' align=center valign=top height=19 style=border-left:none;border-bottom:none><font size=2> Reading <br><br>(kN)</font></td>";
                        mySql += "<td width='9%' align=center valign=top height=19 style=border-left:none;border-bottom:none><font size=2> Comp. strength <br>(N/mm<sup>2</sup>)</font></td>";
                        mySql += "<td width='10%' align=center valign=top height=19 style=border-left:none;border-bottom:none><font size=2> Avg. comp. strength (N/mm<sup>2</sup>)</font></td></tr>";


                        break;
                    case "water":
                        mySql = mySql.Replace("#Paving Block#", "Precast Concrete Block of Paving - Water Absorption");
                        mySql += "<table border=1 cellpadding=0 cellspacing=0 style='border-collapse: collapse' bordercolor='#111111' width='70%' id='AutoNumber1'>";
                        mySql += "<tr><td width='5%' align=center valign=top height=19 style=border-left:none;border-bottom:none><font size=2>Sr. No. </font></td>";
                        mySql += "<td width='15%' align=center valign=top height=19 style=border-left:none;border-bottom:none><font size=2>ID Mark </font></td>";
                        mySql += "<td width='15%' align=center valign=top height=19 style=border-left:none;border-bottom:none><font size=2> Dry Weight<br>(g)</font></td>";
                        mySql += "<td width='15%' align=center valign=top height=19 style=border-left:none;border-bottom:none><font size=2> Wet Weight<br>(g)</font></td>";
                        mySql += "<td width='10%' align=center valign=top height=19 style=border-left:none;border-bottom:none><font size=2> Water Absorption <br>(%)</font></td>";
                        mySql += "<td width='10%' align=center valign=top height=19 style=border-left:none;border-bottom:none><font size=2> Average Water Absorption (%)</font></td></tr>";

                        break;
                    case "flexural":
                        mySql = mySql.Replace("#Paving Block#", "Precast Concrete Block of Paving - Flexural Strength");

                        mySql += "<table border=1 cellpadding=0 cellspacing=0 style='border-collapse: collapse' bordercolor='#111111' width='90%' id='AutoNumber1'>";
                        mySql += "<tr><td width='7%' align=center valign=top height=19 style=border-left:none;border-bottom:none><font size=2>Sr. No. </font></td>";
                        mySql += "<td width='13%' align=center valign=top height=19 style=border-left:none;border-bottom:none><font size=2>ID Mark </font></td>";
                        mySql += "<td width='8%' align=center valign=top height=19 style=border-left:none;border-bottom:none><font size=2> Age <br><br>(Days)</font></td>";
                        mySql += "<td width='10%' align=center valign=top height=19 style=border-left:none;border-bottom:none><font size=2>Thickness <br><br>(mm)</font></td>";
                        mySql += "<td width='10%' align=center valign=top height=19 style=border-left:none;border-bottom:none><font size=2>Breaking <br>load <br>(kN)</font></td>";
                        mySql += "<td width='9%' align=center valign=top height=19 style=border-left:none;border-bottom:none><font size=2> Flexural <br>strength<br> (N/mm<sup>2</sup>) </font></td>";
                        mySql += "<td width='10%' align=center height=19 style=border-left:none;border-bottom:none><font size=2> Avg.Flexural<br> strength <br>(N/mm<sup>2</sup>)</font></td></tr>";
                        break;

                    case "tensile":
                        mySql = mySql.Replace("#Paving Block#", "Precast Concrete Block of Paving - Tensile Strength");
                        mySql += "<br>";
                        mySql += "<table border=1 cellpadding=0 cellspacing=0 style='border-collapse: collapse' bordercolor='#111111' width='85%' id='AutoNumber1'>";
                        mySql += "<tr><td width='6%' align=center valign=top height=19 style=border-left:none;border-bottom:none><font size=2>Sr. No. </font></td>";
                        mySql += "<td width='11%' align=center valign=top height=19 style=border-left:none;border-bottom:none><font size=2>ID Mark </font></td>";
                        mySql += "<td width='5%' align=center valign=top height=19 style=border-left:none;border-bottom:none><font size=2>Age<br><br><br>(Days)</font></td>";
                        mySql += "<td width='5%' align=center valign=top height=19  style=border-left:none;border-bottom:none><font size=2> Thickness <br><br><br>(mm)</font></td>";
                        mySql += "<td width='10%' align=center valign=top height=19  style=border-left:none;border-bottom:none><font size=2> Failure load (N)</font></td>";
                        mySql += "<td width='8%' align=center valign=top height=19 style=border-left:none;border-bottom:none> Mean failure lenth <br>(mm)</td>";
                        mySql += "<td width='10%' align=center valign=top height=19 style=border-left:none;border-bottom:none> Mean failure thickness <br>(mm)</td>";
                        mySql += "<td width='11%' align=center valign=top height=19  style=border-left:none;border-bottom:none><font size=2>Failure load <br>per unit length<br> <br>(N/mm) </font></td>";
                        mySql += "<td width='12%' align=center valign=top height=19 style=border-left:none;border-bottom:none><font size=2> Splitting tensile strength<br><br> (N/mm<sup>2</sup>)</font></td>";
                        mySql += "<td width='12%' align=center valign=top height=19 style=border-left:none;border-bottom:none><font size=2> Avg. splitting<br> tensile strength <br> <br>(N/mm<sup>2</sup>)</font></td></tr>";

                        break;
                }
            }
            mySql += "<tr>";
            mySql += "<td align=center valign=center height=19 rowspan=1><font size=2>" + rw["Sr_no"].ToString() + "</font></td>";
            switch (rw["pt_test"].ToString().Trim().ToLower())
            {
                case "compressive":
                    mySql += "<td align=center valign=center height=19 rowspan=1><font size=2>" + rw["ID_Mark"].ToString() + "</font></td>";
                    mySql += "<td align=center valign=center height=19 rowspan=1><font size=2>" + rw["Age"].ToString() + "</font></td>";
                    mySql += "<td align=center valign=center height=19 rowspan=1><font size=2>" + rw["C_s_Area"].ToString() + "</font></td>";
                    string[] strVal1 = rw["S2"].ToString().Split(',');
                    queStr = strVal1[strVal1.Length - 1].ToString();

                    mySql += "<td align=center valign=center height=19 rowspan=1><font size=2>" + queStr + "</font></td>";
                    mySql += "<td align=center valign=center height=19 rowspan=1><font size=2>" + rw["Wt"].ToString() + "</font></td>";
                    mySql += "<td align=center valign=center height=19 rowspan=1><font size=2>" + rw["Density"].ToString() + "</font></td>";
                    mySql += "<td align=center valign=center height=19 rowspan=1><font size=2>" + rw["Reading"].ToString() + "</font></td>";
                    mySql += "<td align=center valign=center height=19 rowspan=1><font size=2>" + rw["Comp_Str"].ToString() + "</font></td>";
                    break;
                case "water":
                    mySql += "<td align=center valign=center height=19 rowspan=1><font size=2>" + rw["ID_Mark"].ToString() + "</font></td>";
                    mySql += "<td align=center valign=center height=19 rowspan=1><font size=2>" + rw["S1"].ToString() + "</font></td>";
                    mySql += "<td align=center valign=center height=19 rowspan=1><font size=2>" + rw["S2"].ToString() + "</font></td>";
                    mySql += "<td align=center valign=center height=19 rowspan=1><font size=2>" + rw["WT"].ToString() + "</font></td>";
                    break;

                case "flexural":
                    queStr = rw["Thickness"].ToString().Replace("mm", "");
                    mySql += "<td align=center valign=center height=19 rowspan=1><font size=2>" + rw["ID_Mark"].ToString() + "</font></td>";
                    mySql += "<td align=center valign=center height=19 rowspan=1><font size=2>" + rw["Age"].ToString() + "</font></td>";
                    mySql += "<td align=center valign=center height=19 rowspan=1><font size=2>" + queStr + "</font></td>";
                    mySql += "<td align=center valign=center height=19 rowspan=1><font size=2>" + rw["S1"].ToString() + "</font></td>";
                    mySql += "<td align=center valign=center height=19 rowspan=1><font size=2>" + rw["Comp_Str"].ToString() + "</font></td>";
                    break;
                case "tensile":
                    queStr = rw["Reading"].ToString().Replace("mm", "");

                    mySql += "<td align=center valign=center height=19 rowspan=1><font size=2>" + rw["ID_Mark"].ToString() + "</font></td>";
                    mySql += "<td align=center valign=center height=19 rowspan=1><font size=2>" + rw["Age"].ToString() + "</font></td>";
                    mySql += "<td align=center valign=center height=19 rowspan=1><font size=2>" + queStr + "</font></td>";
                    mySql += "<td align=center valign=center height=19 rowspan=1><font size=2>" + rw["S1"].ToString() + "</font></td>";
                    string[] strVal = rw["S2"].ToString().Split(',');
                    queStr = strVal[strVal.Length - 1].ToString();
                    mySql += "<td align=center valign=center height=19 rowspan=1><font size=2>" + queStr + "</font></td>";
                    string[] strVal2 = rw["S3"].ToString().Split(',');
                    queStr = strVal2[strVal2.Length - 1].ToString();
                    mySql += "<td align=center valign=center height=19 rowspan=1><font size=2>" + queStr + "</font></td>";

                    mySql += "<td align=center valign=center height=19 rowspan=1><font size=2>" + rw["Incr_Comp_Str"].ToString() + "</font></td>";
                    mySql += "<td align=center valign=center height=19 rowspan=1><font size=2>" + rw["Comp_Str"].ToString() + "</font></td>";

                    break;
            }
            if (i == 0)
            {
                queStr = (dt.Rows.Count).ToString(); ;
                mySql += "<td align='center' rowspan= '" + queStr + "' height=11><font size=2>" + rw["Avg_str"].ToString() + "</font></td>";
            }
            mySql += "</tr>";
        }

        mySql += "</table>";


        return mySql;
    }


    public String displaySteelTestReport(string referenceNo)
    {
        string mySql;
        string myQueStr;
        Boolean flgBend = false;
        Boolean flgRebend = false;
        Boolean flgElongation = false;
        Boolean flgTensile = false;
        Boolean flgWtMeter = false;

        DataSet ds = new DataSet();
        DataTable dt = new DataTable();

        DataSet ds1 = new DataSet();
        DataTable dt1 = new DataTable();
        mySql = "";

        myQueStr = "select  c.part  from steel_test_inward_table as a,steel_in_rate as b , rate_list as c";
        myQueStr += " where a.st_in_id=b.st_in_id and b.rate_id=c.rate_id and refNo='" + referenceNo + "'";
        SqlDataAdapter ad = new SqlDataAdapter(myQueStr, cn);
        ad.Fill(ds);
        dt = ds.Tables[0];
        ds.Dispose();
        ad.Dispose();
        foreach (DataRow objRw in dt.Rows)
        {

            switch (objRw["part"].ToString().ToLower().Trim())
            {
                case "bend":
                    flgBend = true;
                    break;
                case "bend & rebend test":
                    flgRebend = true;
                    break;
                case "percentage elongation":
                    flgElongation = true;
                    break;
                case "tensile strength":
                    flgTensile = true;
                    break;
                case "tensile strength & percentage elongation test":
                    flgTensile = true;
                    flgElongation = true;
                    break;
                case "weight per meter test":
                    flgWtMeter = true;
                    break;
            }
        }

        mySql += "<table border=1 cellpadding=1 cellspacing=0 style=border:collapse; collapse bordercolor=#111111 width=97% id=AutoNumber1>";
        mySql += "<tr>";
        //common columns
        mySql += "<td width=6% align=center valign=top height=19 style=border-left:none;border-top:none;border-bottom:none><font size=2>Sr. No.</font></td>";
        mySql += "<td width=8% align=center valign=top height=19 style=border-left:none;border-top:none;border-bottom:none><font size=2>Dia.of bar</font></td>";
        mySql += "<td width=18% align=center valign=top height=19 style=border-left:none;border-top:none;border-bottom:none><font size=2>ID Mark</font></td>";
        //b.CSArea  -3,[b.Wt/Meter]4,[b.%ageElong] 5,b.YieldStress 6 ,b.UltStress 7,b.Rebend 8,b.Bend 9,a.avg" ;        
        if (flgTensile == true || flgElongation == true)
        {
            mySql += "<td width=8% align=center valign=top height=19 style=border-left:none;border-top:none;border-bottom:none><font size=2> C/s area </font></td>";
        }
        //wt per meter
        if (flgWtMeter == true)
        {
            mySql += "<td width=6% align=center valign=top height=19 style=border-left:none;border-top:none;border-bottom:none><font size=2>Wt/m </font></td>";
            mySql += "<td width=6% align=center valign=top height=19 style=border-left:none;border-top:none;border-bottom:none><font size=2> Avg. Wt/m   </font></td>";
        }
        //rebend
        if (flgRebend == true)
        {
            mySql += "<td width=9% align=center valign=top height=19 style=border-left:none;border-top:none;border-bottom:none><font size=2> Rebend Test</font></td>";
        }
        //bend
        if (flgBend == true)
        {
            mySql += "<td width=8% align=center valign=top height=19 style=border-left:none;border-top:none;border-bottom:none ><font size=2>Bend Test</font></td>";
        }
        //elongation
        if (flgElongation == true)
        {
            mySql += "<td width=8% align=center valign=top height=19 style=border-left:none;border-top:none;border-bottom:none><font size=2>Elongation</font></td>";
        }
        //tensile stress
        if (flgTensile == true)
        {
            mySql += "<td width=25% align=center valign=top height=19 style=border-right:none;border-left:none;border-top:none colspan=2 ><font size=2>Tensile stress (N/mm<sup>2</sup>)</font></td>";
        }
        //b.CSArea  -3,[b.Wt/Meter]4,[b.%ageElong] 5,b.YieldStress 6 ,b.UltStress 7,b.Rebend 8,b.Bend 9,a.avg" ;            
        mySql += "</tr>";
        mySql += "<td width=6% align=center valign=bottom height=19 style=border-left:none;border-top:none><font size=2>&nbsp;</font></td>";
        mySql += "<td width=8% align=center valign=bottom height=19 style=border-left:none;border-top:none><font size=2>&nbsp;(mm)</font></td>";
        mySql += "<td width=14% align=center valign=bottom height=19 style=border-left:none;border-top:none><font size=2>&nbsp;</font></td>";
        //only 4 tensile stress.
        if (flgTensile == true)
        {
            mySql += "<td width=8% align=center valign=bottom height=19 style=border-left:none;border-top:none><font size=2>(mm<sup>2</sup>)</font></td>";
        }
        //wt/meter
        if (flgWtMeter == true)
        {
            mySql += "<td width=7% align=center valign=bottom height=19 style=border-left:none;border-top:none><font size=2>(kg)</font></td>";
            mySql += "<td width=6% align=center valign=bottom height=19 style=border-left:none;border-top:none><font size=2>(kg)</font></td>";
        }
        //rebend
        if (flgRebend == true)
        {
            mySql += "<td width=6% align=center valign=bottom height=19 style=border-left:none;border-top:none><font size=2>(135<Sup>0</Sup>/157.5<Sup>0</Sup>)</font></td>";
        }
        //bend
        if (flgBend == true)
        {
            mySql += "<td width=8% align=center valign=bottom height=19 style=border-left:none;border-top:none><font size=2>(180<Sup>0</Sup>)</font></td>";
        }
        //elongation
        if (flgElongation == true)
        {
            mySql += "<td width=9% align=center valign=bottom height=19 style=border-left:none;border-top:none><font size=2>&nbsp;(%)</font></td>";
        }
        //tensile stress
        if (flgTensile == true)
        {
            mySql += "<td width=10% align=center valign=bottom height=19 style=border-left:none;border-top:none><font size=2>0.2 % Proof </font></td>";
            mySql += "<td width=15% align=center valign=bottom height=19 style=border-right:none;border-left:none;border-top:none><font size=2>Ultimate </font></td>";
        }

        myQueStr = "select b.sr_no as  srno,b.diam as DiaBar,b.id_mark as idmark,";
        myQueStr += "b.C_S_Area as CSArea,b.[Wt/Meter],b.[%age_Elong] as [%ageElong],";
        myQueStr += "b.Yield_Stress as YieldStress,b.Ult_Stress as UltStress,b.Bend_Rebend_Rem as Rebend,";
        myQueStr += "b.Bend,c.Avg_Str as [avg]";
        myQueStr += " from steel_test_inward_table as a,steel_Test_lab_data as b,record_avg_str as c";
        myQueStr += " where a.st_in_id=b.st_in_id and b.avg_id=c.avg_id ";
        myQueStr += " and a.refno='" + referenceNo + "'";

        SqlDataAdapter ad1 = new SqlDataAdapter(myQueStr, cn);
        ad1.Fill(ds1);
        dt1 = ds1.Tables[0];
        ds1.Dispose();

        for (Int32 i = 0; i < dt1.Rows.Count; i++)
        {
            //b.CSArea  -3,[b.Wt/Meter]4,[b.%ageElong] 5,b.YieldStress 6 ,b.UltStress 7,b.Rebend 8,b.Bend 9,a.avg" ;
            mySql += "<tr>";
            mySql += "<td align=center valign=center height=19 rowspan=1><font size=2>" + dt1.Rows[i].ItemArray[0].ToString() + "</font></td>";
            mySql += "<td align=center valign=center height=19 rowspan=1><font size=2>" + dt1.Rows[i].ItemArray[1].ToString() + "</font></td>";
            mySql += "<td align=center valign=center height=19 rowspan=1><font size=2>" + dt1.Rows[i].ItemArray[2].ToString() + "</font></td>";
            //only for tensile stress
            if (dt1.Rows[i].ItemArray[3].ToString() != "")
            {
                mySql += "<td align=center valign=center height=19 rowspan=1><font size=2>" + dt1.Rows[i].ItemArray[3].ToString() + "</font></td>";
            }
            //wt per meter
            if (dt1.Rows[i].ItemArray[4].ToString() != "")
            {
                mySql += "<td width=6% align=center valign=top height=19 ><font size=2>" + dt1.Rows[i].ItemArray[4].ToString() + "</font></td>";
                if (i == 0)
                {
                    mySql += "<td width=6% align=center valign=center height=19 rowspan=" + dt1.Rows.Count + "><font size=2>" + dt1.Rows[i].ItemArray[10].ToString() + "</font></td>";
                }
            }
            //rebend
            if (dt1.Rows[i].ItemArray[8].ToString() != "")
            {

                mySql += "<td width=9% align=center valign=top height=19 ><font size=2>" + dt1.Rows[i].ItemArray[8].ToString() + "</font></td>";
            }
            //bend
            if (dt1.Rows[i].ItemArray[9].ToString() != "")
            {
                mySql += "<td width=8% align=center valign=top height=19 ><font size=2>" + dt1.Rows[i].ItemArray[9].ToString() + "</font></td>";
            }
            //elongation
            if (dt1.Rows[i].ItemArray[5].ToString() != "")
            {
                mySql += "<td width=8% align=center valign=top height=19 ><font size=2>" + dt1.Rows[i].ItemArray[5].ToString() + "</font></td>";
            }
            // tensile streess
            if (dt1.Rows[i].ItemArray[6].ToString() != "")
            {
                mySql += "<td align=center valign=center height=19 rowspan=1><font size=2>" + dt1.Rows[i].ItemArray[6].ToString() + "</font></td>";
                mySql += "<td align=center valign=center height=19 rowspan=1><font size=2>" + dt1.Rows[i].ItemArray[7].ToString() + "</font></td>";
            }

            mySql += "</tr>";
        }

        return mySql;
    }
    public String displayMixDesignReport(string referenceNo, Boolean flgIsFinal)
    {
        string mySql;
        string queStr;
        DataSet ds = new DataSet();
        DataSet ds1 = new DataSet();
        DataTable dtTest = new DataTable();
        DataTable dtInward = new DataTable();
        SqlCommand cmd = new SqlCommand();

        queStr = "select  * from  mf_inward_table as a, trial_table as b where  a.mf_inward_id=b.mf_inward_id " +
            "and a.Set_Of_MF='" + referenceNo + "' and status=1 ";
        if (flgIsFinal == true)
        {
            queStr += " and trial_name='Final Trial'";
        }
        else { queStr += " and trial_name<>'Final Trial'"; }

        SqlDataAdapter ad = new SqlDataAdapter(queStr, cn);
        ad.Fill(ds);
        dtInward = ds.Tables[0];
        DataRow rw = dtInward.Rows[0];
        mySql = "<tr>";
        mySql += "<td>" + "Mix design parameters : Mix design for " + rw["Grade_Of_Conc"] + " grade concrete for " + rw["Nat_Of_Work"] + "</td>";
        mySql += "</tr><br>";

        mySql += "<tr>";
        mySql += "<td>" + "Special requirement/considerations : " + rw["MF_Type"].ToString() + "</td>";
        mySql += "</tr><br>";

        queStr = "select * from Trial_Prop_Cement  ";
        queStr += "where trial_id=" + rw["trial_id"].ToString();
        SqlDataAdapter ad1 = new SqlDataAdapter(queStr, cn);
        ad1.Fill(ds1);
        dtTest = ds1.Tables[0];

        string[,] msRpt = new string[3, 12];
        string[] tblOther = new string[3];
        msRpt[0, 0] = "For";
        msRpt[1, 0] = "One bag";
        msRpt[2, 0] = "Per m<sup>3</sup>";
        for (int x = 0; x < 3; x++)
        {
            tblOther[x] = "";
        }
        Int32 i = 0;
        if (dtTest.Rows.Count > 0)
        {
            DataRow rwTest = dtTest.Rows[0];
            i = i + 1;
            msRpt[0, i] = "Cement";
            msRpt[1, i] = rwTest["Cem_By_Wt"].ToString();
            msRpt[2, i] = rwTest["Cem_Cont_Prop"].ToString();
            tblOther[0] = rwTest["Cem_Cont_Prop"].ToString();
        }

        //FlyASh
        dtTest.Reset();
        cmd.CommandText = "select * from Trial_Prop_FlyAsh where trial_id=" + rw["trial_id"].ToString();
        ad1.SelectCommand = cmd;
        cmd.Connection = cn;
        ad1.Fill(ds1);
        dtTest = ds1.Tables[0];
        if (rw["Final_WC_Ratio"].ToString() != null)
        {
            tblOther[1] = "Water Cement ratio = " + rw["Final_WC_Ratio"].ToString();
        }
        if (dtTest.Rows.Count > 0)
        {
            DataRow rwTest = dtTest.Rows[0];
            i = i + 1;
            msRpt[0, i] = "Fly Ash";
            msRpt[1, i] = rwTest["Fly_Ash_By_Min_Adm"].ToString();
            msRpt[2, i] = rwTest["Fly_Ash_Cont"].ToString();
            tblOther[2] = "Total cementatious material = " + (Convert.ToSingle(tblOther[0].ToString()) + Convert.ToSingle(rwTest["Fly_Ash_Cont"].ToString())) + " kg/m<sup>3</sup>";
            if (rw["Final_WC_Ratio"].ToString() != null)
            {
                tblOther[1] = "Water binder ratio = " + rw["Final_WC_Ratio"].ToString();
            }

        }

        // Crushed Sand
        dtTest.Reset();
        cmd.CommandText = "select * from Trial_Prop_CS where trial_id=" + rw["trial_id"].ToString();
        ad1.SelectCommand = cmd;
        cmd.Connection = cn;
        ad1.Fill(ds1);
        dtTest = ds1.Tables[0];

        if (dtTest.Rows.Count > 0)
        {
            DataRow rwTest = dtTest.Rows[0];
            i = i + 1;
            msRpt[0, i] = "Crushed Sand";
            msRpt[1, i] = rwTest["Cs_By_Wt"].ToString();
            msRpt[2, i] = rwTest["Cs_kgm3"].ToString();
        }

        // Natural Sand
        dtTest.Reset();
        cmd.CommandText = "select * from Trial_Prop_NS where trial_id=" + rw["trial_id"].ToString();
        ad1.SelectCommand = cmd;
        cmd.Connection = cn;
        ad1.Fill(ds1);
        dtTest = ds1.Tables[0];

        if (dtTest.Rows.Count > 0)
        {
            DataRow rwTest = dtTest.Rows[0];
            i = i + 1;
            msRpt[0, i] = "Natural Sand";
            msRpt[1, i] = rwTest["Ns_By_Wt"].ToString();
            msRpt[2, i] = rwTest["Ns_kgm3"].ToString();
        }

        // Stone Sand
        dtTest.Reset();
        cmd.CommandText = "select * from Trial_Prop_SD where trial_id=" + rw["trial_id"].ToString();
        ad1.SelectCommand = cmd;
        cmd.Connection = cn;
        ad1.Fill(ds1);
        dtTest = ds1.Tables[0];

        if (dtTest.Rows.Count > 0)
        {
            DataRow rwTest = dtTest.Rows[0];
            i = i + 1;
            msRpt[0, i] = "Stone Dust";
            msRpt[1, i] = rwTest["sd_By_Wt"].ToString();
            msRpt[2, i] = rwTest["sd_kgm3"].ToString();
        }
        // grit
        dtTest.Reset();
        cmd.CommandText = "select * from Trial_Prop_gt where trial_id=" + rw["trial_id"].ToString();
        ad1.SelectCommand = cmd;
        cmd.Connection = cn;
        ad1.Fill(ds1);
        dtTest = ds1.Tables[0];

        if (dtTest.Rows.Count > 0)
        {
            DataRow rwTest = dtTest.Rows[0];
            i = i + 1;
            msRpt[0, i] = "Natural Sand";
            msRpt[1, i] = rwTest["Grit_By_Wt"].ToString();
            msRpt[2, i] = rwTest["Grit_kgm3"].ToString();
        }

        // 10 mm
        dtTest.Reset();
        cmd.CommandText = "select * from Trial_Prop_10mm where trial_id=" + rw["trial_id"].ToString();
        ad1.SelectCommand = cmd;
        cmd.Connection = cn;
        ad1.Fill(ds1);
        dtTest = ds1.Tables[0];

        if (dtTest.Rows.Count > 0)
        {
            DataRow rwTest = dtTest.Rows[0];
            i = i + 1;
            msRpt[0, i] = "10 mm aggt.";
            msRpt[1, i] = rwTest["Ten_By_Wt"].ToString();
            msRpt[2, i] = rwTest["Ten_MM_Kgm3"].ToString();
        }

        // 20 mm
        dtTest.Reset();
        cmd.CommandText = "select * from Trial_Prop_20mm where trial_id=" + rw["trial_id"].ToString();
        ad1.SelectCommand = cmd;
        cmd.Connection = cn;
        ad1.Fill(ds1);
        dtTest = ds1.Tables[0];
        if (dtTest.Rows.Count > 0)
        {
            DataRow rwTest = dtTest.Rows[0];
            i = i + 1;
            msRpt[0, i] = "20 mm aggt.";
            msRpt[1, i] = rwTest["Twenty_By_Wt"].ToString();
            msRpt[2, i] = rwTest["Twenty_MM_Kgm3"].ToString();
        }
        // 40 mm
        dtTest.Reset();
        cmd.CommandText = "select * from Trial_Prop_40mm where trial_id=" + rw["trial_id"].ToString();
        ad1.SelectCommand = cmd;
        cmd.Connection = cn;
        ad1.Fill(ds1);
        dtTest = ds1.Tables[0];

        if (dtTest.Rows.Count > 0)
        {
            DataRow rwTest = dtTest.Rows[0];
            i = i + 1;
            msRpt[0, i] = "40 mm aggt.";
            msRpt[1, i] = rwTest["Forty_By_Wt"].ToString();
            msRpt[2, i] = rwTest["Forty_MM_Kgm3"].ToString();
        }

        //Water
        i = i + 1;
        msRpt[0, i] = "Water";
        msRpt[1, i] = rw["Water_Kg"].ToString();
        msRpt[2, i] = Convert.ToDouble(rw["Water_Kgm3"].ToString()).ToString("F");

        // Admixture
        dtTest.Reset();
        cmd.CommandText = "select * from Trial_Prop_adm where trial_id=" + rw["trial_id"].ToString();
        ad1.SelectCommand = cmd;
        cmd.Connection = cn;
        ad1.Fill(ds1);
        dtTest = ds1.Tables[0];

        if (dtTest.Rows.Count > 0)
        {
            DataRow rwTest = dtTest.Rows[0];
            i = i + 1;
            msRpt[0, i] = "Admixture";
            msRpt[1, i] = rwTest["Adm_By_Wt"].ToString() + " ml";
            msRpt[2, i] = rwTest["Adm_Kgm3"].ToString() + " ml";
        }

        // table 
        mySql += "<tr><td><b>Proposed Mix Proportions :</b> (all weights in kg)</td>";
        mySql += "</tr>";
        mySql += "<table border=1 cellspacing=0 style='border-collapse:collapse' bordercolor=#111111 cellpadding=0 width=85%>";
        for (int k = 0; k <= 2; k++)
        {
            mySql += "<tr>";
            for (int l = 0; l <= i; l++)
            {
                if (l > 0)
                { mySql += "<td align=center>" + msRpt[k, l].ToString().Trim() + "</td>"; }
                else
                    mySql += "<td align=left>" + "&nbsp;" + msRpt[k, l].ToString().Trim() + "</td>";
            }
            mySql += "</tr>";
        }
        mySql += "</table>";


        ///      Part b.etn test detail and remarks 

        mySql += "<br>";
        mySql += "<table border=0 cellspacing=0 style='border-collapse:collapse' bordercolor=#111111 cellpadding=0 width=85%>";
        // water cement/binder ratio , cementatious material
        mySql += "<tr>";
        mySql += "<td align=left>" + tblOther[1].ToString() + "</td>";
        if (tblOther[2].ToString().Trim() != "")
        { mySql += "<td align=left>" + tblOther[2].ToString() + "</td>"; }
        else { mySql += "<td align=left> </td>"; }
        mySql += "</tr>";
        //cement name
        dtTest.Reset();
        cmd.CommandText = "SELECT cement_name from cement_inward as a,mf_in_cem as b " +
                //" where a.cem_id=b.cem_id and b.mf_inward_id =" + rw["a.mf_inward_id"].ToString();
                " where a.cem_id=b.cem_id and b.mf_inward_id =" + rw["mf_inward_id"].ToString();
        ad1.SelectCommand = cmd;
        cmd.Connection = cn;
        ad1.Fill(ds1);
        dtTest = ds1.Tables[0];
        mySql += "<tr>";
        mySql += "<td align=left> Cement used " + dtTest.Rows[0].ItemArray[0].ToString() + "</td>";
        //flyAsh Name
        dtTest.Reset();
        cmd.CommandText = "SELECT Fly_Ash_Name from Fly_ash_inward as a,mf_in_Fly_Ash as b " +
                    //" where a.fly_ash_id=b.fly_ash_id and b.mf_inward_id =" + rw["a.mf_inward_id"].ToString();
                    " where a.fly_ash_id=b.fly_ash_id and b.mf_inward_id =" + rw["mf_inward_id"].ToString();
        ad1.SelectCommand = cmd;
        cmd.Connection = cn;
        ad1.Fill(ds1);
        dtTest = ds1.Tables[0];
        if (dtTest.Rows.Count > 0)
        { mySql += "<td align=left>Fly Ash used " + dtTest.Rows[0].ItemArray[0].ToString() + "</td>"; }
        else
        { mySql += "<td align=left> </td>"; }
        mySql += "</tr>";
        //Admixture
        dtTest.Reset();
        cmd.CommandText = "SELECT adm_name from adm_inward_table as a,mf_in_adm as b" +
                    //" where a.adm_id=b.adm_id and b.mf_inward_id =" + rw["a.mf_inward_id"].ToString();
                    " where a.adm_id=b.adm_id and b.mf_inward_id =" + rw["mf_inward_id"].ToString();
        ad1.SelectCommand = cmd;
        cmd.Connection = cn;
        ad1.Fill(ds1);
        dtTest = ds1.Tables[0];
        mySql += "<td align=left> </td>";
        if (dtTest.Rows.Count > 0)
        { mySql += "<td align=left>Admixture used " + dtTest.Rows[0].ItemArray[0].ToString() + "</td>"; }
        else { mySql += "<td align=left> </td>"; }
        mySql += "</tr>";

        //  
        mySql += "<tr><td align left>Observations of Laboratory Trials : </td>";
        string lblMinSlump = "";
        string[] strTemp2 = rw["Slump"].ToString().Split('|');
        queStr = "";
        if (strTemp2.Length == 2)
        {
            string[] strTemp1 = strTemp2[1].ToString().Split('=');
            queStr = strTemp1[0].ToString();
            if (Convert.ToDouble(queStr) > 0)
            {
                lblMinSlump = "Initial slump : " + strTemp2[0] + " mm";
                if (queStr.Trim() != "")
                {
                    queStr = " after " + queStr;
                }
                if (queStr.Trim() != "" && strTemp1.Length > 1)
                { queStr += " minutes slump : " + strTemp1[1].ToString() + " mm"; }

            }
            else
            {
                lblMinSlump = "Slump achieved  " + strTemp2[0] + " mm";
            }
        }
        else
        {
            lblMinSlump = "Slump achieved  " + strTemp2[0] + " mm";
            queStr = "&nbsp;";
        }

        mySql += "<tr><td align left>" + lblMinSlump + " </td></tr>";
        if (queStr != "&nbsp;")
        {
            //if (Convert.ToInt32(queStr) > 0)
            //{
                mySql += "<tr><td align left>" + queStr + " </td> </tr>";
            //}
        }
        if (flgIsFinal == true)
        {

            queStr = rw["Grade_of_Conc"].ToString().Replace("M", "") + "(1.65 x " + rw["Adm_Name"].ToString() + ")= " + Convert.ToSingle(rw["Grade_Of_Conc"].ToString().Replace("M", "")) + (1.65 * Convert.ToSingle(rw["Adm_Name"]));
            queStr += "&nbsp;&nbsp;&nbsp;N/mm" + "<sup>" + "2" + "</sup>";

            lblMinSlump = "Target mean strength = f<sub>ck </sub>N/mm<sup>2</sup> + (K x Standard Deviation)";
            mySql += "<tr><td align left>" + lblMinSlump + " </td></tr>";
            lblMinSlump = " K = Himsworth Constant which is 1.65 for 5 % results to fall below the characteristic strength.";
            mySql += "<tr><td align left>" + lblMinSlump + " </td></tr>";
            lblMinSlump = "Standard Deviation Assumed = " + rw["Adm_name"].ToString() + " N/mm<sup>2</sup>&nbsp;&nbsp; (As per IS 456 - 2000 Table 8, Clause 9.2.4.2)";
            mySql += "<tr><td align left>" + lblMinSlump + " </td></tr>";

            mySql += "<tr><td align left>" + queStr + " </td></tr>";
        }
        mySql += "<tr>";
        if (rw["Yield"].ToString() != null)
        {
            mySql += "<td align left>" + "Plastic density = " + rw["Yield"].ToString() + " </td>";
        }
        else { mySql += "<tr><td align=left> </td>"; }

        if (rw["Compaction_Factor"].ToString() != null)
        {
            if (Convert.ToSingle(rw["Compaction_Factor"].ToString()) > 0)
            {
                mySql += "<td align left>" + "Compaction factor = " + rw["Compaction_Factor"].ToString() + " </td>";
            }
        }
        else { mySql += "<td align=left> </td>"; }
        mySql += "</tr>";

        if (flgIsFinal == true && rw["Segregation"].ToString() != "")
        {

            queStr = "Average 7 days compressive strength    =" + rw["Segregation"] + "&nbsp;&nbsp;&nbsp;N/mm" + "<sup>" + "2" + "</sup>";
            mySql += "<td align left>" + queStr + " </td>";
        }
        if (flgIsFinal == true && rw["Bleeding"].ToString() != null)
        {

            queStr = "Average 28 days compressive strength    =" + rw["Bleeding"] + "&nbsp;&nbsp;&nbsp;N/mm" + "<sup>" + "2" + "</sup>";
            mySql += "<td align left>" + queStr + " </td>";
        }
        mySql += "</table>";

        return mySql;
    }
    public String displayPileTestReport(string referenceNo)
    {
        string mySql;
        string queStr;
        DataTable dtCategory = new DataTable();
        DataTable dtInward = new DataTable();
        DataSet ds = new DataSet();
        DataSet ds1 = new DataSet();

        mySql = "<table border=1 cellpadding=0 cellspacing=0 style='border-collapse: collapse' bordercolor='#111111' width=85% id='AutoNumber1'>";
        mySql += "<tr><td  align=center valign=top height=19 style=border-left:none;border-bottom:none><font size=2>Sr. No. </font></td>";
        mySql += "<td  align=center valign=top height=19 style=border-left:none;border-bottom:none><font size=2>Pile Category</font></td>";
        mySql += "<td  align=center valign=top height=19 style=border-left:none;border-bottom:none><font size=2> Description </font></td>";
        mySql += "<td  align=center valign=top height=19 style=border-left:none;border-bottom:none><font size=2> Pile Identification</font></td></tr>";

        queStr = "Select TestDetail from pile_test_master where refno='" + referenceNo + "'";

        SqlDataAdapter adInw = new SqlDataAdapter(queStr, cn);
        adInw.Fill(ds1);
        dtInward = ds1.Tables[0];
        ds1.Dispose();
        queStr = "Select * from Pile_Categories";
        SqlDataAdapter ad = new SqlDataAdapter(queStr, cn);
        ad.Fill(ds);
        dtCategory = ds.Tables[0];
        ds.Dispose();
        string[] strVal1 = dtInward.Rows[0].ItemArray[0].ToString().Split('|');

        for (Int32 i = 0; i <= dtCategory.Rows.Count - 1; i++)
        {
            mySql += "<tr><td width='8%' align=center valign=top height=19 style=border-left:none;border-bottom:none><font size=2>" + (i + 1).ToString() + "</font></td>";
            mySql += "<td width='12%' align=center valign=top height=19 style=border-left:none;border-bottom:none><font size=2>" + dtCategory.Rows[i].ItemArray[0].ToString() + "</font></td>";
            mySql += "<td width='40%' align=center valign=top height=19 style=border-left:none;border-bottom:none><font size=2>" + dtCategory.Rows[i].ItemArray[1].ToString() + " </font></td>";
            mySql += "<td width='40%' align=center valign=top height=19 style=border-left:none;border-bottom:none><font size=2>" + strVal1[i].ToString() + " </font></td></tr>";
        }

        mySql += "</table>";
        return mySql;
    }
    public String displayTileTestReport(string referenceNo)
    {
        string mySql;
        string queStr;
        Int32 maxColumns = 0;
        DataTable dtInward = new DataTable();
        DataSet ds1 = new DataSet();
        DataTable dt = new DataTable();
        DataSet ds = new DataSet();
        queStr = "Select * from TileInward where refno='" + referenceNo + "'";
        SqlDataAdapter ad1 = new SqlDataAdapter(queStr, cn);
        ad1.Fill(ds);
        dtInward = ds.Tables[0];
        // test detail
        mySql = "";

        ds.Dispose();


        switch (dtInward.Rows[0].ItemArray[21].ToString())
        {
            case "WT":
                queStr = "select refno,idmark,length  ";
                queStr += " from TileTestDetails   where refno='" + referenceNo + "'";

                mySql = "<table border=1 cellpadding=0 cellspacing=0 style='border-collapse: collapse' bordercolor='#111111' width=85% id='AutoNumber1' height=33>";
                mySql += "<tr><td width='10%' align=center valign=top height=19 style=border-left:none;border-bottom:none><font size=2>Sr.No.</font></td>";
                mySql += "<td width='20%' align=center valign=top height=19 ><font size=2>ID Mark </font></td>";
                mySql += "<td width='20%' align=center valign=top height=19 ><font size=2>Wet transverse strength" + "<br>" + "(N/mm<sup>2</sup>) </font></td>";
                mySql += "<td width='20%' align=center valign=top height=19 ><font size=2> Average wet transverse strength (N/mm<sup>2</sup>)</font></td>";
                mySql += "<td width='20%' align=center valign=top height=19><font size=2> Specified limit </font></td></tr>";
                maxColumns = 2;
                break;
            case "DA":
                queStr = "select refno,idmark,length,width,thickness  ";
                queStr += " from TileTestDetails   where refno='" + referenceNo + "'";


                mySql = "<table border=1 cellpadding=0 cellspacing=0 style='border-collapse: collapse' bordercolor='#111111' width=85% id='AutoNumber1' height=33>";
                mySql += "<tr>";
                mySql += "<td width='56' valign=top rowspan=2 height=19 align='center'><font size = 2>Sr.No.</font></td>";
                mySql += "<td width='65' valign=top rowspan=2 height=19 align='center'><font size = 2>Id Mark</font></td>";
                mySql += "<td width='187' colspan=3 height=19 align='center' valign=top><font size = 2>Dimensions(mm)</font></td>";
                mySql += "<td width='170' colspan=3 height=19 align='center' valign=top><font size = 2>Average dimensions (mm)</font></td>";
                mySql += "<td width='166' height=19 valign=top align='center'><font size = 2>Specified limit(mm)</font></td></tr>";
                mySql += "<td width='65' valign=top height=19 align='center'><font size = 2>Length</font></td>";
                mySql += "<td width='55' valign=top height=19 align='center'><font size = 2>Width</font></td>";
                mySql += "<td width='65' valign=top height=19 align='center'><font size = 2>Thickness</font></td>";
                mySql += "<td width='56' valign=top height=19 align='center'><font size = 2>Length</font></td>";
                mySql += "<td width='53' valign='top' height=19 align='center'><font size = 2>Width</font></td>";
                mySql += "<td width='59' valign='top' height=19 align='center'><font size = 2>Thickness</font></td>";
                maxColumns = 4;
                break;
            case "CR":
                queStr = "select refno,idmark,length ";
                queStr += " from TileTestDetails   where refno='" + referenceNo + "'";
                mySql = "<table border=1 cellpadding=0 cellspacing=0 style='border-collapse: collapse' bordercolor='#111111' width=70% id='AutoNumber1' height=33>";
                mySql += "<tr>";
                mySql += "<td width='10%' valign=top height=19 align='center'>Sr.No.</td>";
                mySql += "<td width='20%' valign=top height=19 align='center'>Id Mark</td>";
                mySql += "<td width='20%' valign=top height=19 align='center'>Crazing resistance</td></tr>";
                maxColumns = 2;
                break;
            case "WA":
                queStr = "select refno,idmark,length,width,thickness  ";
                queStr += " from TileTestDetails   where refno='" + referenceNo + "'";
                mySql = "<table border=1 cellpadding=0 cellspacing=0 style='border-collapse: collapse' bordercolor='#111111' width=85% id='AutoNumber1' height=19>";
                mySql += "<tr>";
                mySql += "<td width='5%' height=19 align='center' valign ='top'><Font Size = 2>Sr.No.</font></td>";
                mySql += "<td width='20%' height=19 align='center' valign ='top'><Font Size = 2>Id Mark</font></td>";
                mySql += "<td width='10%' height=19 align='center' valign ='top'><Font Size = 2>Dry <br>weight</font></td>";
                mySql += "<td width='10%' height=19 align='center' valign ='top'><Font Size = 2>Wet<br>weight</Font></td>";
                mySql += "<td width='10%' height=19 align='center' valign ='top'><Font Size = 2>Water absorption <br><br>(%)</font></td>";
                mySql += "<td width='20%' height=19 align='center' valign ='top'><Font Size = 2>Average water<br> absorption <br>(%)</font></td>";
                if (dtInward.Rows[0].ItemArray[21].ToString() == "Ceramic")
                {
                    mySql += "<td width='20%' height=19 align='center'valign ='top'><Font Size = 2>Specified limits as per IS 4457:2007</font></td></tr>";
                }

                maxColumns = 4;
                break;
            case "MOR":
                queStr = "select refno,idmark,length,width,thickness  ";
                queStr += " from TileTestDetails   where refno='" + referenceNo + "'";

                mySql = "<table border=1 cellpadding=0 cellspacing=0 style='border-collapse: collapse' bordercolor='#111111' width=85% id='AutoNumber1' height=33>";
                mySql += "<tr>";
                mySql += "<td width='27' valign='middle' height=19 align='center'> <Font size=2> Sr.No. </Font> </td>";
                mySql += "<td width='49' valign='middle' height=19 align='center'> <Font size=2> Id Mark </Font> </td>";
                mySql += "<td width='42' valign='middle' height=19 align='center'> <Font size=2> Breaking Load </Font> </td>";
                mySql += "<td width='41' valign='middle' height=19 align='center'> <Font size=2> <p>Average Breaking Load</p></Font></td>";
                mySql += "<td width='42' valign='middle' height=19 align='center'> <Font size=2> Breaking Strength </Font> </td>";
                mySql += "<td width='44' valign='middle' height=19 align='center'> <Font size=2> Average Breaking Strength </Font> </td>";
                mySql += "<td width='47' valign='middle' height=19 align='center'> <Font size=2> Modulus of rupture (N/mm<sup>2</sup>) </Font> </td>";
                mySql += "<td width='53' valign='middle' height=19 align='center'> <Font size=2> <p>Average modulus of rupture (N/mm<sup>2</sup>)</p> </Font> </td>";
                mySql += "<td width='130' valign='middle' height=19 align='left'> <Font size=2> Specified Limits as per 4457:2007 </Font> </td></tr>";

                maxColumns = 2;
                break;
        }

        SqlDataAdapter ad = new SqlDataAdapter(queStr, cn);
        ad.Fill(ds1);
        dt = ds1.Tables[0];

        for (Int32 i = 0; i <= dt.Rows.Count - 1; i++)
        {

            mySql += "<tr>";
            for (Int32 j = 0; j <= maxColumns; j++)
            {
                if (j == 0)
                {
                    mySql += "<td align=center valign=top height=19 ><font size=2>" + (i + 1).ToString() + "</font></td>";

                }
                else
                {
                    mySql += "<td align=center valign=top height=19 ><font size=2>" + dt.Rows[i].ItemArray[j].ToString() + "</font></td>";
                }
            }
            if (i == 0)
            {
                switch (dtInward.Rows[0].ItemArray[21].ToString())
                {
                    case "WT":
                        mySql += "<td align=center valign=center height=19  rowspan=" + (dt.Rows.Count) + "><font size=2>" + dtInward.Rows[0].ItemArray[7].ToString() + "</font></td>";
                        mySql += "<td align=center valign=center height=19 rowspan=" + (dt.Rows.Count) + "><font size=2>" + dtInward.Rows[0].ItemArray[8].ToString() + "</font></td>";
                        break;
                    case "DA":
                        mySql += "<td align=center valign=center height=19  rowspan=" + (dt.Rows.Count) + "><font size=2>" + dtInward.Rows[0].ItemArray[7].ToString() + "</font></td>";
                        mySql += "<td align=center valign=center height=19 rowspan=" + (dt.Rows.Count) + "><font size=2>" + dtInward.Rows[0].ItemArray[8].ToString() + "</font></td>";
                        mySql += "<td align=center valign=center height=19  rowspan=" + (dt.Rows.Count) + "><font size=2>" + dtInward.Rows[0].ItemArray[9].ToString() + "</font></td>";
                        mySql += "<td height=19 align='Left' rowspan =" + (dt.Rows.Count) + "><font size=2> Surface Area 190 < S <= 410 <br>---------------------0.75%|   0.50%  |  5.0% <br> -----------------------------------------Surface Area S > 410  <br> ----------------------------------------- 0.60%   |   0.50%   | 5.0% </font></td>";
                        break;
                    case "WA":
                        mySql += "<td align=center valign=center height=19  rowspan=" + (dt.Rows.Count) + "><font size=2>" + dtInward.Rows[0].ItemArray[7].ToString() + "</font></td>";
                        if (dtInward.Rows[0].ItemArray[21].ToString() == "Ceramic")
                        {
                            mySql += "<td align=center valign=center height=19 rowspan=" + (dt.Rows.Count) + "><font size=2>" + dtInward.Rows[0].ItemArray[8].ToString() + "</font></td>";
                        }
                        break;
                    case "MOR":
                        mySql += "<td align=center valign=center height=19  rowspan=" + (dt.Rows.Count) + "><font size=2>" + dtInward.Rows[0].ItemArray[7].ToString() + "</font></td>";
                        mySql += "<td align=center valign=center height=19 rowspan=" + (dt.Rows.Count) + "><font size=2>" + dtInward.Rows[0].ItemArray[8].ToString() + "</font></td>";
                        mySql += "<td align=center valign=center height=19  rowspan=" + (dt.Rows.Count) + "><font size=2>" + dtInward.Rows[0].ItemArray[9].ToString() + "</font></td>";
                        break;

                }
                mySql += "</tr>";
            }
        }
        mySql += "</table>";


        return mySql;


    }
    public String getReportinString(string myRefNo, string rType, Boolean FlgIsFinal, string db)
    {
        string rptStr;
        rptStr = "";
        rptStr = getReportHeader(rType, myRefNo, ref FlgIsFinal, db );
        if (rType != "MF")
        {
            rptStr += "<tr><td height=19></br></td></tr>";

            rptStr += "<tr><td height=19><font size=2><b>OBSERVATIONS & CALCULATIONS:</b></font></td></tr>";
        }
        else
        {
            if (FlgIsFinal == true)
            {
                rptStr = rptStr.Replace("[*]", "Final Concrete Mix Design ");
            }
            else
            { rptStr = rptStr.Replace("[*]", "Interim Concrete Mix Design"); }
        }
        switch (rType)
        {
            case "CT":
                rptStr += DisplayCubeTestReport(myRefNo);
                break;
            case "ST":
                rptStr += displaySteelTestReport(myRefNo);
                break;
            case "CEMT":
                rptStr += displayCementTestReport(rType, myRefNo);
                break;
            case "FLYASH":
                rptStr += displayCementTestReport(rType, myRefNo);
                break;
            case "AGGT":
                rptStr += displayAggtTestReport(myRefNo);
                break;
            case "CR":
                rptStr += displayCoreTestReport(myRefNo, rType);
                break;
            case "NDT":
                rptStr += displayCoreTestReport(myRefNo, rType);
                break;
            case "SOLID":
                rptStr += displaySolidTestReport(myRefNo);
                break;
            case "BT-":
                rptStr += displayBrickTestReport(myRefNo);
                break;
            case "PT":
                rptStr = displayPavementTestReport(myRefNo, rptStr);
                break;
            case "MF":
                rptStr += displayMixDesignReport(myRefNo, FlgIsFinal);
                break;
            case "PILE":
                rptStr += displayPileTestReport(myRefNo);
                break;
            case "TILE":
                rptStr += displayTileTestReport(myRefNo);
                break;
            case "STC":
                rptStr += displaySteelChemicalReport(myRefNo);
                break;
            case "CCH":
                rptStr += displayCementChemicalReport(myRefNo);
                break;
            case "WT":
                rptStr += displayWaterTestingReport(myRefNo);
                break;

        }
        // print remarks for passed ref no. 
        // this is common for all type of inwards.
        if (rType == "CT")
            rptStr += getCubeRemarks(myRefNo);
        else if (rType == "ST" && _stComplianceNote == 0)
            rptStr += getSteelRemarks(myRefNo, rType);
        else
            rptStr += getRemarks(myRefNo, rType);

        return rptStr;

    }
    public string displaySteelChemicalReport(string mRefNo)
    {
        string myStr = "";
        string queStr;
        DataSet ds = new DataSet();
        DataTable dt = new DataTable();
        SqlCommand cmd = new SqlCommand();
        myStr = "<table border=1 cellspacing=0 width=100%  style= 'border-collapse: collapse' bordercolor=#111111 cellpadding=0>";
        myStr += "<td width='7%' align='middle' valign='top' heigth=11><font size=2>Sr. No.</font></td>";
        myStr += "<td width='20%' align='middle' height=11 valign='top'><font size=2>ID Mark</font></td>";
        myStr += "<td width='10%' align='middle' height=11 style='border-left-style: solid; border-left-width: 1; border-right-style: solid; border-right-width: 1; border-top-style: solid; border-top-width: 1; border-bottom-width: 1'><font size=2>Diameter</font><p><font size=2>(mm)</font></td>";
        myStr += "<td width='10%' align='middle' height=11><font size=2>Carbon </font><p><font size=2>(%)</font></td>";
        myStr += "<td width='10%' align='middle' height=11><font size=2>Manganese</font><p><font size=2>(%)</font></td>";
        myStr += "<td width='10%' align='middle' height=11><font size=2>Sulphur</font><p><font size=2>(%)</font></td>";
        myStr += "<td width='10%' align='middle' height=11><font size=2>Phosphorous </font><p><font size=2>(%)</font></td>";
        myStr += "<td width='20%' align='middle' height=11><font size=2>Sulphur + Phosphorous </font><p><font size=2>(%)</font></td>";

        queStr = "select * from stChemicalReport as a ,stChemicalInward as b where a.refno=b.refno and  a.refno='" + mRefNo + "'";

        SqlDataAdapter ad = new SqlDataAdapter(queStr, cn);
        ad.Fill(ds);
        dt = ds.Tables[0];
        ds.Dispose();
        ad.Dispose();
        for (Int32 i = 0; i < dt.Rows.Count; i++)
        {
            DataRow rw = dt.Rows[i];
            myStr += "<tr>";
            myStr += "<td width = '7%' align= 'middle'>" + "<font size=2>" + rw["SrNo"] + "</font>" + "</td>";
            myStr += "<td width = '20%' align= 'middle'>" + "<font size=2>" + rw["IdMark"] + "</font>" + "</td>";
            myStr += "<td width = '10%' align= 'middle'>" + "<font size=2>" + rw["DiaBar"] + "</font>" + "</td>";
            myStr += "<td width = '10%' align= 'middle'>" + "<font size=2>" + Convert.ToSingle(rw["Carbon"]).ToString("0.000") + "</font></td>";
            myStr += "<td width = '10%' align= 'middle'>" + "<font size=2>" + Convert.ToSingle(rw["Manganese"]).ToString("0.000") + "</font></td>";
            myStr += "<td width = '10%' align= 'middle'>" + "<font size=2>" + Convert.ToSingle(rw["Sulphur"]).ToString("0.000") + "</font></td>";
            myStr += "<td width = '10%' align= 'middle'>" + "<font size=2>" + Convert.ToSingle(rw["Phosphorous"]).ToString("0.000") + "</font></td>";
            myStr += "<td width = '22%' align= 'middle'>" + "<font size=2>" + Convert.ToSingle(rw["SulphurPhosphorous"]).ToString("0.000") + "</font></td>";
            myStr += "</tr>";
            queStr = rw["GradeOfSteel"].ToString();
        }

        dt.Reset();
        cmd.CommandText = " select  * from chemicaltestspec " +
                " where grade='" + queStr + "' or grade='% Variation' order by grade desc";
        ad.SelectCommand = cmd;
        cmd.Connection = cn;
        ad.Fill(ds);
        dt = ds.Tables[0];
        ds.Dispose();
        myStr += "<tr>";
        myStr += "<td width = '37%' align= 'middle' colspan=3> <font size=2>Specified Limits as per IS 1786-2008</font></td>";
        myStr += "<td width = '10%' align= 'middle'>" + "<font size=2>" + Convert.ToSingle(dt.Rows[0].ItemArray[3].ToString()).ToString("0.000") + "</font></td>";
        myStr += "<td width = '10%' align= 'middle'>" + "<font size=2>---</font></td>";
        myStr += "<td width = '10%' align= 'middle'>" + "<font size=2>" + Convert.ToSingle(dt.Rows[1].ItemArray[3].ToString()).ToString("0.000") + "</font></td>";
        myStr += "<td width = '10%' align= 'middle'>" + "<font size=2>" + Convert.ToSingle(dt.Rows[2].ItemArray[3].ToString()).ToString("0.000") + "</font></td>";
        myStr += "<td width = '20%' align= 'middle'>" + "<font size=2>" + Convert.ToSingle(dt.Rows[3].ItemArray[3].ToString()).ToString("0.000") + "</font></td>";
        myStr += "</tr><tr>";
        myStr += "<td width = '37%' align= 'middle'colspan=3 > <font size=2>Variation, over specified maximum limit, % max</font></td>";
        myStr += "<td width = '10%' align= 'middle'>" + "<font size=2>" + Convert.ToSingle(dt.Rows[4].ItemArray[3].ToString()).ToString("0.000") + "</font></td>";
        myStr += "<td width = '10%' align= 'middle'>" + "<font size=2>---</font></td>";
        myStr += "<td width = '10%' align= 'middle'>" + "<font size=2>" + Convert.ToSingle(dt.Rows[5].ItemArray[3].ToString()).ToString("0.000") + "</font></td>";
        myStr += "<td width = '10%' align= 'middle'>" + "<font size=2>" + Convert.ToSingle(dt.Rows[6].ItemArray[3].ToString()).ToString("0.000") + "</font></td>";
        myStr += "<td width = '20%' align= 'middle'>" + "<font size=2>" + Convert.ToSingle(dt.Rows[7].ItemArray[3].ToString()).ToString("0.000") + "</font></td>";
        myStr += "</tr></table>";
        return myStr;

    }
    public string displayCementChemicalReport(string mRefNo)
    {
        string myStr = "";
        string queStr;
        string cementGrade;
        DataSet ds2 = new DataSet();
        DataSet ds = new DataSet();
        DataTable dt = new DataTable();
        SqlCommand cmd = new SqlCommand();
        DataTable dt1 = new DataTable();
        queStr = "select *,b.RateId as bRateId from  cemtchemicalInward as a,CEMTChemicalReport as b" +
                " where a.refno=b.refno and a.refno='" + mRefNo + "' order by b.srno";
        SqlDataAdapter ad = new SqlDataAdapter(queStr, cn);
        ad.Fill(ds);
        dt = ds.Tables[0];
        ds.Dispose();
        ad.Dispose();

        cementGrade = "";
        for (Int32 i = 0; i < dt.Rows.Count; i++)
        {
            DataRow rw = dt.Rows[i];
            if (i == 0)
            {
                queStr = "Specified Limits";
                if (rw["cementName"].ToString().ToUpper().Contains("43 GRADE"))
                {
                    queStr += " (IS-8112)";
                    cementGrade = "43 Grade";
                }
                else if (rw["cementName"].ToString().ToUpper().Contains("53 GRADE"))
                {
                    queStr += " (IS-12269)";
                    cementGrade = "53 Grade";
                }

                else if (rw["cementName"].ToString().ToUpper().Contains("PSC"))
                {
                    queStr += " (IS-455)";
                    cementGrade = "PSC Cement";
                }
                else if (rw["cementName"].ToString().ToUpper().Contains("PPC"))
                {
                    queStr += " (IS-1489)";
                    cementGrade = "PPC Cement";
                }
                myStr = "<table border=1 cellspacing=0 width=90%  style= 'border-collapse: collapse' bordercolor=#111111 cellpadding=0>";
                myStr += "<tr><td width='7%' height='22' align='middle' valign='middle' rowspan=1 height=11><font size=2><b>Sr. No.</b></font></td>" +
                        "<td width='45%' align='middle' height='22' rowspan=1 height=11><font size=2><b>Name of Test</b></font></td>" +
                        "<td width='10%' align='middle' height='22' rowspan=1 height=11><font size=2><b>Result(%)</b></font></td>" +
                        "<td width='40%' align='middle' height='22' rowspan=1 height=11><font size=2><b>" + queStr + "</b></font></td></tr>";
            }
            myStr += "<tr>";
            //myStr += "<td width = '7%' align= 'middle'>" + "<font size=2>" + rw["SrNo"] + "</font>" + "</td>";
            myStr += "<td width = '7%' align= 'middle'>" + "<font size=2>" + (i+1) + "</font>" + "</td>";
            myStr += "<td width = '45%' align= 'middle'>" + "<font size=2>" + rw["TestName"] + "</font>" + "</td>";
            myStr += "<td width = '10%' align= 'middle'>" + "<font size=2>" + rw["result"] + "</font>" + "</td>";
            if (i <= 1)
            //if (rw["RateID"].ToString() == "")
            {
                queStr = "select SpecifiedLimit from flyash_spec where MethodOfTesting='" + rw["testname"] + "'" +
                        " and Unit = '" + cementGrade + "'";
            }
            else
            {
                queStr = "select SpecifiedLimit from flyash_spec where RateID=" + rw["bRateId"].ToString() +
                    " and Unit = '" + cementGrade + "'";
            }
            ds2.Reset();
            cmd.CommandText = queStr;
            cmd.Connection = cn;
            ad.SelectCommand = cmd;
            ad.Fill(ds2);
            dt1 = ds2.Tables[0];
            queStr = "---";
            if (dt1.Rows.Count > 0)
            {
                if (dt1.Rows[0].ItemArray[0].ToString() != null)
                {
                    queStr = dt1.Rows[0].ItemArray[0].ToString();
                }
            }
            myStr += "<td width = '40%' align= 'middle'>" + "<font size=2>" + queStr + "</font></td>";
            myStr += "</tr>";
            dt1.Reset();
        }

        myStr += "</table>";
        return myStr;

    }
    public string displayWaterTestingReport(string mRefNo)
    {
        string myStr = "";
        string queStr;
        DataSet ds = new DataSet();
        DataTable dt = new DataTable();
        SqlCommand cmd = new SqlCommand();

        queStr = "select * from  WaterTestingReport as a,FlyAsh_Spec as b" +
                " where  a.RateId = b.RateId and a.refno='" + mRefNo + "'";
        SqlDataAdapter ad = new SqlDataAdapter(queStr, cn);
        ad.Fill(ds);
        dt = ds.Tables[0];
        ds.Dispose();
        ad.Dispose();
        myStr = "<table border=1 cellspacing=0 width=90%  style= 'border-collapse: collapse' bordercolor=#111111 cellpadding=0>";
        myStr += "<tr><td width='5%' align='middle' valign='middle' rowspan=2 height=11><font size=2><b>Sr. No.</b></font></td>" +
             "<td width='22%' align='middle'  rowspan=2 height=11><font size=2><b>Test Parameters</b></font></td>" +
             "<td width='8%' align='middle'  rowspan=2 height=11><font size=2><b>Unit</b></font></td>" +
             "<td width='15%' align='middle'  rowspan=2 height=11><font size=2><b>Observations</b></font></td>" +
             "<td width='25%' align='middle'  rowspan=2 height=11><font size=2><b>Test Method Specification Used</b></font></td>" +
             "<td width='30%' align='middle'  rowspan=1 height=11><font size=2><b>Permissible Limit IS:456-2000</b></font></td></tr>" +
             "<tr><td width='30%' align='middle'  rowspan=1 height=11><font size=2><b>Mixing and Curing water Clause 5:4 Table 1</b></font></td></tr>";

        for (Int32 i = 0; i < dt.Rows.Count; i++)
        {
            DataRow rw = dt.Rows[i];
            myStr += "<tr><td width = '5%' align= 'middle'><font size=2>" + rw["SrNo"].ToString() + "</font></td>";
            myStr += "<td width = '22%' align= 'middle' ><font size=2>" + getTestNameToDisplay(rw["TestName"].ToString()) + "</font></td>";
            if (rw["Unit"].ToString() != null)
            {
                myStr += "<td width = '8%' align= 'middle' ><font size=2>" + rw["Unit"].ToString() + "</font></td>";
            }
            else
            {
                myStr += "<td width = '8%' align= 'middle'><font size=2>&nbsp;</font></td>";
            }
            myStr += "<td width = '15%' align= 'middle'><font size=2>" + rw["Result"].ToString() + "</font></td>";

            if (rw["MethodOfTesting"].ToString() != null)
            {
                myStr += "<td width = '25%' align= 'middle'><font size=2>" + rw["MethodOfTesting"].ToString() + "</font></td>";
            }
            else
            {
                myStr += "<td width = '25%' align= 'middle' ><font size=2>&nbsp;</font></td>";
            }

            if (rw["SpecifiedLimit"].ToString() != null)
            {
                myStr += "<td width = '30%' align= 'middle'><font size=2>" + rw["SpecifiedLimit"].ToString() + "</font></td>";
            }
            else
            {
                myStr += "<td width = '30%' align= 'middle' ><font size=2>&nbsp;</font></td>";
            }

            myStr += "</tr>";
        }
        myStr += "</table>";
        return myStr;

    }
    protected string getTestNameToDisplay(string TestName)
    {
        string retStr;
        retStr = TestName.Replace("SiO2", "SiO<Sub>2</Sub>");
        retStr = TestName.Replace("SO3", "SO<Sub>3</Sub>");
        retStr = TestName.Replace("Al2O3", "Al<Sub>2</Sub>O<Sub>3</Sub>");
        retStr = TestName.Replace("Fe2O3", "Fe<Sub>2</Sub>O<Sub>3</Sub>");
        retStr = TestName.Replace("C3A", "C<Sub>3</Sub>A");
        retStr = TestName.Replace("CaCO3", "CaCO<Sub>3</Sub>");
        retStr = TestName.Replace("SO4", "SO<Sub>4</Sub>");

        retStr = TestName.Replace("Alkalinity", "Alkalinity (ml of 0.02N H<Sub>2</Sub>SO<Sub>4</Sub> Consumed to neutralize 100ml of water using mixed indicator)");
        retStr = TestName.Replace("Acidity", "Acidity (ml of 0.02N NaOH Consumed to neutralize 100ml of water using phenolphthalein as indicator)");
        return retStr;
    }
    public String getSteelRemarks(string referenceNo, string rType)
    {

        try
        {
            string mySql, retStr;
            string tmpStr, compNote;
            Int32 cnt;

            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            mySql = "";
            retStr = "";
            compNote = "";
            string[] strDate = _mRcvdDate.Split('/');
            if (strDate[2].ToString().Contains(" ") == true)
            {
                strDate = _mRcvdDate.Substring(0, _mRcvdDate.IndexOf(" ")).Split('/');
            }
            //DateTime tempDate = new DateTime(Convert.ToInt32(strDate[2]), Convert.ToInt32(strDate[1]), Convert.ToInt32(strDate[0])).ToString();
            string tempDate = (strDate[1] + "/" + strDate[0] + "/" + strDate[2]).ToString();
            switch (rType)
            {
                case "ST":
                    if (_stComplianceNote == 0)
                    {
                        mySql = "SELECT [Desc] FROM MethodOfTesting where RecordType= 'ST' and Type= 'Compnote'";
                        //mySql += " and fromDate <=cdate('" + _mRcvdDate + "') and toDate >=cdate('" + _mRcvdDate + "')";
                        mySql += " and fromDate <=convert(date,'" + tempDate + "') and toDate >=convert(date,'" + tempDate + "')";

                    }
                    else
                    {
                        mySql = "SELECT [Desc] FROM MethodOfTesting where RecordType= 'ST' and Type= 'Compnote'";
                        //mySql += " and fromDate <=cdate('" + _mRcvdDate + "') and toDate >=cdate('" + _mRcvdDate + "')";
                        mySql += " and fromDate <=convert(date,'" + tempDate + "') and toDate >=convert(date,'" + tempDate + "')";

                    }

                    break;
                default:
                    retStr = "<table  width='90%'>";
                    goto remPrint;

            }
            SqlDataAdapter da = new SqlDataAdapter(mySql, cn);
            da.Fill(ds);
            dt = ds.Tables[0];
            ds.Dispose();
            cnt = 0;
            retStr = "<table width='90%'>";
            for (int i = 0; i < dt.Rows.Count; i++)
            {

                tmpStr = dt.Rows[i].ItemArray[0].ToString();
                if (tmpStr.ToLower() == "remarks")
                {
                    goto nxt123;
                }
                cnt = cnt + 1;
                if (i == 0)
                {
                    if (_stComplianceNote == 0)
                    {
                        retStr += "<tr><td width='89%' colspan=2 align=left valign=top height=19><font size=2></font><u><b> Compliance </b></u></td></tr>";
                    }
                }
                if (tmpStr.Trim() != "")
                {
                    if (tmpStr.Substring(0, 4).Contains(")") == true)
                    {
                        tmpStr = tmpStr.Substring(tmpStr.IndexOf(")") + 1);
                    }

                    retStr += "<tr><td width= 1% align=right valign=top height=19 ><font size=2><b>" + cnt.ToString() + ")</b></font></td>";
                    retStr += "<td width='89%' align=left valign=top height=19><font size=2></font><b>" + tmpStr + "</b></td></tr>";
                    compNote = tmpStr;
                }
            nxt123:
                cnt = 0;
            }
            dt.Reset();

            // method of testing             
            mySql = "SELECT [Desc] FROM MethodOfTesting where RecordType= 'ST' and Type= 'Method'";
            //mySql += " and fromDate <=cdate('" + _mRcvdDate + "') and toDate >=cdate('" + _mRcvdDate + "')";
            mySql += " and fromDate <=convert(date,'" + tempDate + "') and toDate >=convert(date,'" + tempDate + "')";
            SqlDataAdapter da1 = new SqlDataAdapter(mySql, cn);
            da1.Fill(ds);
            dt = ds.Tables[0];
            ds.Dispose();
            da1.Dispose();

            cnt = 0;
            for (int i = 0; i < dt.Rows.Count; i++)
            {

                tmpStr = dt.Rows[i].ItemArray[0].ToString();
                if (tmpStr.Contains("Remarks") == false)
                {

                    cnt = cnt + 1;
                    if (i == 0)
                    {
                        if (_stComplianceNote == 0)
                        {
                            retStr += "<tr><td width='89%' colspan=2 align=left valign=top height=19><font size=2><u><b> Method of Testing </b></u></font></td></tr>";
                        }
                    }
                    if (tmpStr.Trim() != "")
                    {
                        if (tmpStr.Substring(0, 4).Contains(")") == true)
                        {
                            tmpStr = tmpStr.Substring(tmpStr.IndexOf(")") + 1);
                        }
                        retStr += "<tr><td width= 1% align=right valign=top height=19 ><font size=2>" + cnt.ToString() + ")</font></td>";
                        retStr += "<td width='89%' align=left valign=top height=19><font size=2>" + tmpStr + "</font></td></tr>";
                    }
                }
            }
            dt.Reset();
            // methods from std iscode
            mySql = "SELECT ISCode FROM StdISCodeMaster where RecordType= 'ST'";
            //mySql += " and fromDate <= cdate('" + _mRcvdDate + "') and ( toDate >=cdate('" + _mRcvdDate + "')";
            mySql += " and fromDate <= convert(date,'" + tempDate + "') and ( toDate >=convert(date,'" + tempDate + "')";
            //mySql += " Or IsNull(toDate) =  True)";
            mySql += " Or toDate is null)";
            SqlDataAdapter da2 = new SqlDataAdapter(mySql, cn);
            da2.Fill(ds);
            dt = ds.Tables[0];
            ds.Dispose();
            da2.Dispose();

            for (int i = 0; i < dt.Rows.Count; i++)
            {

                tmpStr = dt.Rows[i].ItemArray[0].ToString();
                if (tmpStr.Contains("Remarks") == false)
                {
                    cnt = cnt + 1;
                    if (i == 0 && cnt == 1)
                    {
                        retStr += "<tr><td width='89%' colspan=2 align=left valign=top height=19><font size=2><u><b> Method of Testing </b></u></font></td></tr>";
                    }
                    if (tmpStr.Trim() != "")
                    {
                        if (tmpStr.Substring(0, 4).Contains(")") == true)
                        {
                            tmpStr = tmpStr.Substring(tmpStr.IndexOf(")") + 1);
                        }
                        retStr += "<tr><td width= 1% align=right valign=top height=19 ><font size=2>" + cnt.ToString() + ")</font></td>";
                        retStr += "<td width='89%' align=left valign=top height=19><font size=2>" + tmpStr + "</font></td></tr>";
                    }
                }
            }


            // remarks print start
            mySql = " SELECT b.remarks from st_st_new_rem as a,record_remarks as b,steel_test_inward_table as c" +
                            " where a.st_in_id=c.st_in_id and a.rem_id=b.rem_id " +
                            " and c.RefNo='" + referenceNo.Trim() + "'";

            dt.Reset();

        remPrint:
            SqlDataAdapter da3 = new SqlDataAdapter(mySql, cn);
            da3.Fill(ds);
            dt = ds.Tables[0];
            ds.Dispose();
            da3.Dispose();
            // remarks print start
            cnt = 0;
            for (int i = 0; i < dt.Rows.Count; i++)
            {

                tmpStr = dt.Rows[i].ItemArray[0].ToString();
                if (tmpStr != compNote && tmpStr.Contains("IS 1786-2008") == false)
                {
                    if (tmpStr.Trim() != "" && tmpStr.Trim().ToLower() != "remarks")
                    {
                        if (i == 0)
                        {
                            retStr += "<tr><td width='89%' colspan=2 align=left valign=top height=19><font size=2><u><b> Remarks </b></u></font></td></tr>";
                        }
                        cnt = cnt + 1;
                        if (tmpStr.Substring(0, 4).Contains(")") == true)
                        {
                            tmpStr = tmpStr.Substring(tmpStr.IndexOf(")") + 1);
                        }

                        retStr += "<tr><td width= 1% align=right valign=top height=19 ><font size=2>" + cnt.ToString() + ")</font></td>";
                        retStr += "<td width='89%' align=left valign=top height=19><font size=2>" + tmpStr + "</font></td></tr>";
                    }
                }
            }

            retStr += "</table>";

            return retStr;
        }
        catch (Exception e)
        {
            throw e;
        }
    }

    public DataTable getBillList(double clId, double siteId, string fromDate, string toDate, string apprStatus, string db)
    {
        string mySql;
        try
        {
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            mySql = "select * ";
            mySql = mySql + " , (Select b.DL_ContPerson_var from tbl_DeviceLogin as b where b.DL_Id = a.BILL_ClientApprovedBy_int) as BILL_ClientApprovedBy";
            mySql = mySql + " , (Select b.DL_ContNo_var from tbl_DeviceLogin as b where b.DL_Id = a.BILL_ClientApprovedBy_int) as BILL_ClientApprovedByTel";
            mySql = mySql + " from tbl_BILL as a Where";
            if (apprStatus != "All")
            {
                mySql = mySql + " a.BILL_ClientApproveStatus_bit ='" + apprStatus + "' and ";
            }

            mySql = mySql + "     a.BILL_CL_Id= " + clId.ToString("0");
            if (siteId > 0)
            {
                mySql = mySql + " and a.BILL_SITE_Id =" + siteId.ToString("0");
            }
            mySql = mySql + " and BILL_Date_dt >= convert(date,'" + fromDate + "') and BILL_Date_dt <= convert(date,'" + toDate + "')";
            mySql = mySql + " and a.BILL_ApproveStatus_bit = 1 ";
            SqlDataAdapter da = new SqlDataAdapter(mySql, cn);
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

    public DataTable getDiviceLoginForBillApproval(string loginName, string password)
    {
        try
        {
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            string strQuery = @"select * from tbl_DeviceLogin c
                                where c.DL_BillApproval_bit = 1 and c.DL_Status_bit = 0 
                                and c.DL_LoginId_var = '" + loginName + "' and c.DL_Password_var = '" + password + "'";
                                
            SqlDataAdapter da = new SqlDataAdapter(strQuery, cn);
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
    public bool checkBillApprovalStatus(string referenceNo)
    {
        try
        {
            bool billPendingStatus = false;
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            string strQuery = @"select * from tbl_Inward, tbl_Bill  where INWD_BILL_Id = BILL_Id and BILL_ClientApproveStatus_bit = 0 
                            and BILL_Status_bit = 0 and BILL_ApproveStatus_bit = 1 and INWD_ReferenceNo_int = " + referenceNo;
            SqlDataAdapter da = new SqlDataAdapter(strQuery, cn);
            da.Fill(ds);
            dt = ds.Tables[0];
            if (dt.Rows.Count > 0)
            {
                billPendingStatus = true;
            }
            else
            {   
                ds = new DataSet();
                dt = new DataTable();
                strQuery = @"select * from tbl_Bill, tbl_BillDetail where BILL_Id = BILLD_BILL_Id 
                        and BILL_ClientApproveStatus_bit = 0 and BILL_Status_bit = 0 and BILL_ApproveStatus_bit = 1 and BILLD_ReferenceNo_int =  " + referenceNo;
                da = new SqlDataAdapter(strQuery, cn);
                da.Fill(ds);
                dt = ds.Tables[0];
                if (dt.Rows.Count > 0)
                {
                    billPendingStatus = true;
                }
            }
            ds.Dispose();
            return billPendingStatus;
        }
        catch (Exception e)
        {
            throw e;
        }
    }
}