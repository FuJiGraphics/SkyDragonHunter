using System;
using System.Collections.Generic;
using NPOI.SS.UserModel;
using NPOI.HSSF.UserModel;
using NPOI.XSSF.UserModel;
using System.IO;
using UnityEngine;

public static class XLoader
{
    public static XSSFWorkbook Load(string filePath)
    {
        XSSFWorkbook book = XLoader.CreateBookWithLoad(filePath);
        return book;
    }

    public static void SaveFromExcelSheetToCSV(ISheet sheet, string csvFilePath)
    {
        using (StreamWriter sw = new StreamWriter(csvFilePath))
        {
            for (int i = 0; i <= sheet.LastRowNum; i++)
            {
                IRow row = sheet.GetRow(i);
                if (row == null) continue;

                string line = "";
                for (int j = 0; j < row.LastCellNum; j++)
                {
                    ICell cell = row.GetCell(j);
                    if (cell != null)
                    {
                        line += GetCellValue(cell) + ",";
                    }
                    else
                    {
                        line += ",";
                    }
                }

                line = line.TrimEnd(',');
                sw.WriteLine(line);
            }
        }

        Debug.Log("CSV 저장 완료: " + csvFilePath);
    }

    private static string GetCellValue(ICell cell)
    {
        switch (cell.CellType)
        {
            case CellType.String: return cell.StringCellValue;
            case CellType.Boolean: return cell.BooleanCellValue.ToString();
            case CellType.Numeric: return cell.NumericCellValue.ToString();
            case CellType.Formula: return cell.ToString(); // 계산값으로 출력하려면 Evaluate 사용
            default: return "";
        }
    }

    private static XSSFWorkbook CreateBookWithLoad(string filePath)
    {
        XSSFWorkbook book = null;
        if (XLoader.Vaildation(filePath))
        {
            Debug.Log($"Complate: Successfully created the file = {filePath}");
            book = new XSSFWorkbook(new FileStream(filePath, FileMode.Open));
        }
        return book;
    }

    private static bool Vaildation(string filePath)
    {
        bool invaildPath = File.Exists(filePath);
        if (!invaildPath)
        {
            Debug.LogError($"Error: invalid file path = {filePath}");
            return false;
        }

        var extension = Path.GetExtension(filePath);
        if (".xlsx" != extension)
        {
            Debug.LogError($"Error: invalid extension = {extension}");
            return false;
        }

        return true;
    }
} // static class XLoader