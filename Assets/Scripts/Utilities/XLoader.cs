using System;
using System.Collections.Generic;
using NPOI.SS.UserModel;
using NPOI.HSSF.UserModel;
using NPOI.XSSF.UserModel;
using System.IO;
using UnityEngine;

static class XLoader
{
    public static void Load(string filePath)
    {
        XSSFWorkbook book = XLoader.CreateBookWithLoad(filePath);
        if (book != null)
        {
            for (int i = 0; i < book.NumberOfSheets; ++i)
            {
                ISheet sheet = book.GetSheetAt(i);

                for (int rowIdx = 0; rowIdx <= sheet.LastRowNum; rowIdx++)
                {
                    var row = sheet.GetRow(rowIdx);
                    if (row == null) continue;

                    var rowData = new List<string>();
                    for (int cellIdx = 0; cellIdx < row.LastCellNum; cellIdx++)
                    {
                        var cell = row.GetCell(cellIdx);
                        Debug.Log(cell?.ToString());
                    }
                }
            }
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