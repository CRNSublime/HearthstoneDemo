using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Data;
using OfficeOpenXml;
using System.IO;
using System.Text;
using System;
using System.Runtime.Remoting;
using System.Reflection;

public class ExcelConfigTool:EditorWindow {

	[MenuItem("CustomTool/CreateExcelConfig", false)]
    public static void CreateExcelConfig()
    {
        GetWindow<ExcelConfigTool>("导出Excel表配置信息");
    }

    [MenuItem("Assets/ExportExcelConfig", false, 1)]
    public static void CreateConfig()
    {
        string[] guids = Selection.assetGUIDs;
        foreach (string guid in guids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            string[] splitStr = path.Split('.');
            int index = path.IndexOf('/');
            path = path.Substring(index + 1, path.Length - index - 1);
            if (splitStr.Length==1)
            {
                ReadExcel(path);
            }
            else
            {
                if (splitStr[1] == "xlsx")
                {
                    path = Application.dataPath + "/" + path;
                    AnalysisExcel(path);
                }
                else
                {
                    Debug.LogError(" 选中的不是 .xlsx 文件! ");
                }
            }
        }      
    }
    string excelPath = "";
    void OnGUI()
    {
        GUILayout.Space(20);
        excelPath = EditorGUILayout.TextField("Excel配置表路径:", excelPath);
        GUILayout.Space(300);
        if(GUILayout.Button("Export"))
        {
            ReadExcel(excelPath);
        }
    }

    private static void ReadExcel(string excelFloder)
    {
        if (string.IsNullOrEmpty(excelFloder))
            Debug.LogError(" 请输入文件夹路径！");
        try
        {
            bool excelExist = false;
            string[] excels = Directory.GetFiles(Application.dataPath + "/" + excelFloder);
            for (int i = 0; i < excels.Length; i++)
            {
                if (excels[i].EndsWith(".xlsx"))
                {
                    excelExist = true;
                    AnalysisExcel(excels[i]);
                }
            }
            if (!excelExist)
                Debug.Log("该文件夹内没有Excel表，文件夹路径：" + Application.dataPath + "/" + excelFloder);
        }
        catch (Exception e)
        {
            Debug.LogError("路径有误，请检查输入路径是否正确！  错误信息：" + e.Message);
        }
    }

    private static void AnalysisExcel(string excel)
    {
        Debug.Log(" ------------>>>>>>> 解析当前 excel: " + excel);
        FileStream excelFile = new FileStream(excel, FileMode.Open);  // Stream流 打开Excel表 
        using (ExcelPackage package = new ExcelPackage(excelFile)) // 理解为实例化Excel表对象 用此对象就可以获取Excel表中各种数据
        {
            // package.Workbook.Worksheets  -- 可获得Excel表中的 sheet 页数 

            ExcelWorksheet excelSheet = package.Workbook.Worksheets[1];  // 只查一页 不解析多页
            CreateCsConfig(excelSheet);
            CreateAsset(excelSheet);
        }
    }

    // 生成 Excel 对应 cs 配置脚本
    private static void CreateCsConfig(ExcelWorksheet excelData)
    {
        string className = excelData.Name + "Config";
        string configDirec = Application.dataPath + "/ExcelConfig";
        string configPath = configDirec + "/" + className + ".cs";
        if(!Directory.Exists(configDirec))
        {
            Directory.CreateDirectory(configDirec);
        }
        StringBuilder csConfig = new StringBuilder("using System.Collections;\r\nusing System.Collections.Generic;\r\nusing UnityEngine;\r\nusing System;\r\n");

        csConfig.AppendFormat("public class {0} : ScriptableObject {{", className);
        string structName = excelData.Name.Substring(0, 1).ToUpper() + excelData.Name.Substring(1, excelData.Name.Length - 1);
        string listStr = "\r\n    public List<" + structName + "> " + excelData.Name.ToLower() + "List = new List<" + structName + ">();\r\n";
        csConfig.Append(listStr);
        //string listStr = "\r\n    public Dictionary<int, "+ structName +"> " + excelData.Name.ToLower() + "Dic = new Dictionary<int, " + structName + ">();\r\n";
        //csConfig.Append(listStr);
        string assetStr = "    public string assetName = \"" + excelData.Name + "Asset\";\r\n}\r\n";
        csConfig.Append(assetStr);
        string structStr = "[Serializable]\r\npublic struct " + structName + "\r\n{\r\n";
        csConfig.Append(structStr);

        string fieldsStr = "";
        // excelSheet.Dimension  --- 可获得Excel表的行列信息
        int startColumn = excelData.Dimension.Start.Column;  // 起始列
        int endColumn = excelData.Dimension.End.Column;      // 结束列
        int startRow = excelData.Dimension.Start.Row;        // 起始行
        int endRow = excelData.Dimension.End.Row;           // 结束行

        for (int i = startColumn; i <= endColumn; i++)
        {
            fieldsStr += "    public " + excelData.GetValue<string>(3, i).ToLower() + " " + excelData.GetValue<string>(1, i) + ";\r\n";
        }

        csConfig.AppendFormat("{0}", fieldsStr);
        csConfig.Append("}\r\n");

        File.WriteAllText(configPath, csConfig.ToString());
        Debug.Log(" ======== CreateCsConfig ====== configPath: " + configPath);
        AssetDatabase.Refresh();
    }

    // 生成 cs 脚本对应 asset 文件
    private static void CreateAsset(ExcelWorksheet excelData)
    {
        string floder = "Resources/AssetData";
        string assetPath = "Assets/" + floder + "/" + excelData.Name + "Asset.asset";
        string director = Application.dataPath + "/" + floder;
        if (!Directory.Exists(director))
        {
            Directory.CreateDirectory(director);
        }

        string listName = excelData.Name.ToLower() + "List";
        string className = excelData.Name + "Config";
        string structName = excelData.Name.Substring(0, 1).ToUpper() + excelData.Name.Substring(1, excelData.Name.Length - 1);
        int row = excelData.Dimension.Rows;
        int column = excelData.Dimension.Columns;
        
        // 反射创建对应类的实例  这个类就是上面解析Excel表生成的类
        ObjectHandle classHandle = Activator.CreateInstance("Assembly-CSharp", className);
        UnityEngine.Object classObj = (UnityEngine.Object)classHandle.Unwrap();
        Type classType = classObj.GetType();

        // 通过获取类中的 List<> 类型  反射创建一个新的 List<> 实例 用来最后给asset文件中的List<>赋值
        Type listType = classType.GetField(listName).FieldType;
        object listIns = Activator.CreateInstance(listType);
        MethodInfo method = listType.GetMethod("Add", BindingFlags.Public | BindingFlags.Instance);

        for (int i = 4; i <= row; i++)
        {
            // 反射创建 struct 的对象 解析Excel填充字段数据
            ObjectHandle handle = Activator.CreateInstance("Assembly-CSharp", structName);
            object curObject = handle.Unwrap();
            Type curStruct = curObject.GetType();

            for (int j = 1; j <= column; j++)
            {
                FieldInfo field = curStruct.GetField(excelData.GetValue<string>(1, j));
                string fieldType = excelData.GetValue<string>(3, j);

                switch (fieldType)
                {
                    case ("int"):
                        int inter = excelData.GetValue<int>(i, j);
                        field.SetValue(curObject, inter);
                        break;
                    case ("double"):
                        double doub = excelData.GetValue<double>(i, j);
                        field.SetValue(curObject, doub);
                        break;
                    case ("string"):
                        string str = excelData.GetValue<string>(i, j);
                        field.SetValue(curObject, str);
                        break;
                    default:
                        Debug.LogError(" No Such Field Type: " + fieldType);
                        break;
                }
            }
            method.Invoke(listIns, new object[] { curObject });   // 往List<>中添加数据
            int key = excelData.GetValue<int>(i, 1);
        }

        // 创建asset文件，把上面填充好的List<> 赋值给asset文件
        ScriptableObject asset = CreateInstance(className);
        FieldInfo list = asset.GetType().GetField(listName);
        list.SetValue(asset, listIns);
        AssetDatabase.CreateAsset(asset, assetPath);
        AssetDatabase.Refresh();
    }

}
