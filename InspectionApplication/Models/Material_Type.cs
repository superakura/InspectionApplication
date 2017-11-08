using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Material_Type 的摘要说明
/// </summary>
public class Material_Type
{
	public Material_Type()
	{
		//
		// TODO: 在此处添加构造函数逻辑
		//
	}
    private int _materialtype_id;
    private string _materialtype_name;
    /// <summary>
    /// 物料类型ID，自动增长。
    /// </summary>
    public int MaterialType_ID
    {
        set { _materialtype_id = value; }
        get { return _materialtype_id; }
    }
    /// <summary>
    /// 物料类型名称
    /// </summary>
    public string MaterialType_Name
    {
        set { _materialtype_name = value; }
        get { return _materialtype_name; }
    }
}