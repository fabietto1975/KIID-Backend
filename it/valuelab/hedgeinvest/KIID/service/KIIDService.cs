﻿using m = KIID.it.valuelab.hedgeinvest.KIID.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using KIID.it.valuelab.hedgeinvest.KIID.service.helpers;

namespace KIID.it.valuelab.hedgeinvest.KIID.service
{
    public class KIIDService
    {
        public List<m.KIIDData> readFundsData()

        {
            string inputFileName = @"D:\LAVORO\PROGETTI\HEDGEINVEST\KKID\INPUT\DATIKIDD.XLSX"; //TODO: esternalizzare property
            const string mainSheetname = "DATI KIID";
            const string performanceSheetname = "PERFORMANCE";
            List<m.KIIDData> result = new List<m.KIIDData>();
            using (ExcelHelper excelHelper = new ExcelHelper(inputFileName))
            {
                //Performance
                int row = 2;
                string  isin = excelHelper.GetValue(performanceSheetname, "A", row.ToString());
                Dictionary<string, SortedDictionary<string,string>> isinPerformanceAnnoMap = new Dictionary<string, SortedDictionary<string, string>>() ;
                while (!isin.Equals(""))
                {
                    string anno = excelHelper.GetValue(performanceSheetname, "B", row.ToString());
                    string dato = excelHelper.GetValue(performanceSheetname, "C", row.ToString());

                    SortedDictionary<string, string> isinPerformanceAnno; 
                    if (!isinPerformanceAnnoMap.TryGetValue(isin, out isinPerformanceAnno))
                    {
                        isinPerformanceAnno = new SortedDictionary<string, string>();
                    }
                    isinPerformanceAnno[anno] = dato;

                    isinPerformanceAnnoMap[isin] =  isinPerformanceAnno;
                    row++;
                    isin = excelHelper.GetValue(performanceSheetname, "A", row.ToString());
                }

                //Dati Fondo
                row = 2;
                string classe = excelHelper.GetValue(mainSheetname, "A", row.ToString());
                while (!classe.Equals(""))
                {
                    row++;
                    string currentIsin = excelHelper.GetValue(mainSheetname, "C", row.ToString());
                    SortedDictionary<string,string> performances = new SortedDictionary<string, string>();
                    isinPerformanceAnnoMap.TryGetValue(isin, out performances);
                    m.KIIDData item = new m.KIIDData(
                        classe,
                        excelHelper.GetValue(mainSheetname, "B", row.ToString()),
                        currentIsin,
                        excelHelper.GetValue(mainSheetname, "D", row.ToString()),
                        excelHelper.GetValue(mainSheetname, "E", row.ToString()),
                        excelHelper.GetValue(mainSheetname, "F", row.ToString()),
                        excelHelper.GetValue(mainSheetname, "H", row.ToString()),
                        excelHelper.GetValue(mainSheetname, "I", row.ToString()),
                        excelHelper.GetValue(mainSheetname, "J", row.ToString()),
                        excelHelper.GetValue(mainSheetname, "K", row.ToString()),
                        excelHelper.GetValue(mainSheetname, "L", row.ToString()),
                        excelHelper.GetValue(mainSheetname, "M", row.ToString()),
                        excelHelper.GetValue(mainSheetname, "N", row.ToString()),
                        performances
                        );
                    result.Add(item);
                    classe = excelHelper.GetValue(mainSheetname, "A", row.ToString());
                }

            }
            return result;
        }

    }
}