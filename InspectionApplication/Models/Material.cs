using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Material 的摘要说明
/// </summary>
public class Material
{
	public Material()
	{
		//
		// TODO: 在此处添加构造函数逻辑
		//
	}
    public int Material_ID { get; set; }
    public string Material_Name { get; set; }
    public int Material_Type { get; set; }
    public string ExecutionStandard_Type { get; set; }
    public string ExecutionStandard_Name { get; set; }
    public string ExecutionStandard_Year { get; set; }
    public int ExecutionStandard_Status { get; set; }
    public string ExecutionStandard_File { get; set; }
    public string TechnicalProtocol_Name { get; set; }
    public string TechnicalProtocol_File { get; set; }
    public string User_ID { get; set; }
    public DateTime Execution_Date { get; set; }
    public string MaterialType_Name { get; set; }
    public string Material_Request { get; set; }
    public int Expr1 { get; set; }
    public string User_Name { get; set; }
    public string dept { get; set; }
}