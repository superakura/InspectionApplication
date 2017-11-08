using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Data.SqlClient ;
using System.Data.OleDb;

/// <summary>
/// Summary description for DB
/// </summary>
public class DB2
{
	public DB2()
	{
		//
		// TODO: Add constructor logic here
		//
	}
    //����һ��sqlserver���ݿ����Ӷ���
    public static SqlConnection CreateCon()
    {
        return new SqlConnection("server=10.126.10.180;database=ProducingAuxiliaryMaterials;uid=sa;pwd=d3xvkub4;");
        //return new SqlConnection("server=.;database=ProducingAuxiliaryMaterials;Integrated Security=SSPI");
    }
    //ִ��һ��sql���
    public static string ExeSql(string sql)
    {
        string result;
        SqlConnection con = DB2.CreateCon();
        con.Open();
        SqlCommand command = new SqlCommand(sql, con);
        SqlTransaction trans = con.BeginTransaction();
        command.Transaction = trans;
        try
        {
           result=command.ExecuteNonQuery().ToString();
            trans.Commit();
            //result = "ok";
        }
        catch (Exception ex)
        {
            trans.Rollback();
            result = ex.Message;   
        }
        finally
        {
            con.Close();
        }
        return result;
    }
    //������������ֵ
    public static string GetFirstValue(string sqlget)
    {
        SqlConnection con = DB2.CreateCon();
        SqlCommand cmd = new SqlCommand(sqlget,con);
        try
        {
            con.Open();
            string info = cmd.ExecuteScalar().ToString();
            if (info == null)
            {
                return "";
            }
            else
            {
                return info;
            }
        }
        catch
        {
            return "ʧ��";
        }
        finally
        {
            con.Close();
        }
    }
    
    public static DataSet InitDs(string sql, string table_name)
    {
        SqlConnection con = DB2.CreateCon();
        SqlDataAdapter sda = new SqlDataAdapter(sql, con);
        DataSet ds = new DataSet();
        con.Open();
        sda.Fill(ds, table_name);
        con.Close();
     
        return ds;
    }
    //�����־��Ϣ
    public static void InsertLog(string LogType, string LogUser, string LogInfo)
    {
        string sql_log = @"insert into ������־ values
                            ('" + LogType + "','" + LogUser + "','" + DateTime.Now + "','" + LogInfo + "')";
        DB2.ExeSql(sql_log);
    }

    //��ʼ�������б�-ͨ��
    public static bool InitDropDownList(DataSet ds, ListItemCollection lic, string table_name,  string field_text, string field_value)
    {
        DataView dv = new DataView();
        dv.Table = ds.Tables[table_name];
        foreach (DataRowView drv in dv)
        {
            ListItem tmpli = new ListItem();
            tmpli.Text = drv[field_text].ToString();
            tmpli.Value = drv[field_value].ToString();
            lic.Add(tmpli);
        } 
        return true;
    }
    //��ʼ�������б�-�µķ���
    public static bool newInitDropDownList(DataSet ds, DropDownList ddl, string table_name, string field_text, string field_value)
    {
        ddl.DataSource = ds.Tables[table_name].DefaultView;
        ddl.DataValueField = ds.Tables[table_name].Columns[field_value].ColumnName;
        ddl.DataTextField = ds.Tables[table_name].Columns[field_text].ColumnName;
        ddl.DataBind();
        return true;
    }
    //�GridView
    public static bool BindGridView(GridView gv, string selsql)
    {
        SqlConnection con = DB2.CreateCon();
        SqlDataAdapter sda = new SqlDataAdapter(selsql, con);
        DataSet ds = new DataSet();
        sda.Fill(ds, "table");
        gv.DataSource = ds.Tables["table"];
        gv.DataBind();
        return true;
    }
    //�GridView����GridView�е�ID
    public static bool BindGridView_id(GridView gv, string selsql,string id)
    {
        SqlConnection con = DB2.CreateCon();
        SqlDataAdapter sda = new SqlDataAdapter(selsql, con);
        DataSet ds = new DataSet();
        sda.Fill(ds, "table");
        gv.DataSource = ds.Tables["table"];
        gv.DataKeyNames = new string[] {id};
        gv.DataBind();
        return true;
    }
}
