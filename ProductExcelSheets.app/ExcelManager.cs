namespace ProductExcelSheets.app
{
    using GGG_OnlineShop.Common.Services.Contracts;
    using OfficeOpenXml;
    using System.IO;
    using System.Linq;
    using System.Text.RegularExpressions;
    using System.Collections.Generic;

    public class ExcelManager : IExcelManager
    {
        private const int DescriptionStringMaxLength = 135;
        private string infofilePathToWrite;
        private string filePath;

        public ExcelManager(ILogger logger, string infofilePathToWrite, string filepath)
        {
            this.fileLogger = logger;
            this.infofilePathToWrite = infofilePathToWrite;
            this.filePath = filepath;
        }

        public ILogger fileLogger { get; set; }

        public void AdaptDescription(string sheetName)
        {
            var fileinfo = new FileInfo(this.filePath);
            if (fileinfo.Exists)
            {
                using (ExcelPackage excelPackage = new ExcelPackage(fileinfo))
                {
                    ExcelWorkbook excelWorkBook = excelPackage.Workbook;
                    ExcelWorksheet excelWorksheet = excelWorkBook.Worksheets.Where(s => s.Name == sheetName).First();

                    var column = excelWorksheet.Cells["1:1"].First(c => c.Value.ToString() == "Description").Start.Column;

                    int rangeMaxRows = excelWorksheet.Dimension.End.Row;
                    fileLogger.LogInfo($"-------------------------- DESCRIPTION ADAPTATION: {sheetName} --------------------------", infofilePathToWrite);
                    for (int i = 1; i <= rangeMaxRows; i++)
                    {
                        string cellStr = excelWorksheet.Cells[i, column].Value.ToString();
                        cellStr = Regex.Replace(cellStr, @"[^a-zA-Z0-9-]+", " ");

                        if (cellStr.Length > DescriptionStringMaxLength)
                        {
                            fileLogger.LogInfo($"LENGTH CUT; length > 135", infofilePathToWrite);
                            cellStr = cellStr.Substring(0, DescriptionStringMaxLength);
                        }

                        if (cellStr != excelWorksheet.Cells[i, column].Value.ToString())
                        {
                            fileLogger.LogInfo($"{excelWorksheet.Cells[i, column].Value } -->", infofilePathToWrite);
                            fileLogger.LogInfo(cellStr, infofilePathToWrite);
                        }

                        excelWorksheet.Cells[i, column].Value = cellStr;
                    }

                    excelPackage.Save();
                }
            }
        }

        public void AdaptProductsPlace(string sourceSheet, string targetSheet, string interchangeablesSheetName)
        {
            var fileinfo = new FileInfo(filePath);

            if (fileinfo.Exists)
            {
                using (ExcelPackage excelPackage = new ExcelPackage(fileinfo))
                {
                    ExcelWorkbook excelWorkBook = excelPackage.Workbook;

                    ExcelWorksheet excelSourceSheet = excelWorkBook.Worksheets.Where(s => s.Name == sourceSheet).First();
                    var codeSourceColumn = excelSourceSheet.Cells["1:1"].First(c => c.Value.ToString() == "Code").Start.Column;
                    var liulinSourceColumn = excelSourceSheet.Cells["1:1"].First(c => c.Value.ToString() == "Liulin").Start.Column;
                    var slatinaSourceColumn = excelSourceSheet.Cells["1:1"].First(c => c.Value.ToString() == "Slatina").Start.Column;
                    var rouseSourceColumn = excelSourceSheet.Cells["1:1"].First(c => c.Value.ToString() == "Rouse").Start.Column;

                    ExcelWorksheet interchangeablesSheet = excelWorkBook.Worksheets.Where(s => s.Name == interchangeablesSheetName).First();

                    ExcelWorksheet excelTargetSheet = excelWorkBook.Worksheets.Where(s => s.Name == targetSheet).First();
                    var euroCodeTargetColumn = excelTargetSheet.Cells["1:1"].First(c => c.Value.ToString() == "Eurocode").Start.Column;
                    var liulinTargetColumn = excelTargetSheet.Cells["1:1"].First(c => c.Value.ToString() == "Liulin").Start.Column;
                    var slatinaTargetColumn = excelTargetSheet.Cells["1:1"].First(c => c.Value.ToString() == "Slatina").Start.Column;
                    var rouseTargetColumn = excelTargetSheet.Cells["1:1"].First(c => c.Value.ToString() == "Rouse").Start.Column;

                    int targetRangeMaxRows = excelTargetSheet.Dimension.End.Row;
                    int sourceRangeMaxRows = excelSourceSheet.Dimension.End.Row;

                    fileLogger.LogInfo($"-------------------------- Adding Products places: {targetSheet} --------------------------", infofilePathToWrite);

                    for (int i = 2; i <= sourceRangeMaxRows; i++)
                    {
                        string sourceCodeString = excelSourceSheet.Cells[i, codeSourceColumn].Value.ToString();
                        string codeToCheck = sourceCodeString.Substring(0, 5);

                        Regex re = new Regex("^[0-9][0-9][0-9][0-9][A-Z]");
                        if (re.IsMatch(codeToCheck))
                        {
                            var codesToBeChecked = this.GetInterchangeablesForGlassType(codeToCheck, interchangeablesSheet, re);
                            codesToBeChecked.Add(codeToCheck);

                            for (int j = 2; j <= targetRangeMaxRows; j++)
                            {
                                // ---------iterate eurocode
                                if (!IsCellEmpty(excelTargetSheet.Cells[j, euroCodeTargetColumn].Value))
                                {
                                    string targetEurocodeString = excelTargetSheet.Cells[j, euroCodeTargetColumn].Value.ToString();
                                    if (targetEurocodeString.Length > 4)
                                    {
                                        if (codesToBeChecked.Contains(targetEurocodeString.Substring(0, 5)))
                                        {
                                            // match - same glass shape
                                            fileLogger.LogInfo($"Eurocode: {targetEurocodeString}", infofilePathToWrite);
                                            
                                            if (IsCellEmpty(excelTargetSheet.Cells[j, liulinTargetColumn].Value)
                                                && !IsCellEmpty(excelSourceSheet.Cells[i, liulinSourceColumn].Value))
                                            {
                                                excelTargetSheet.Cells[j, liulinTargetColumn].Value = excelSourceSheet.Cells[i, liulinSourceColumn].Value;
                                                fileLogger.LogInfo("   Liulin added", infofilePathToWrite);
                                            }
                                            if (IsCellEmpty(excelTargetSheet.Cells[j, slatinaTargetColumn].Value)
                                                && !IsCellEmpty(excelSourceSheet.Cells[i, slatinaSourceColumn].Value))
                                            {
                                                excelTargetSheet.Cells[j, slatinaTargetColumn].Value = excelSourceSheet.Cells[i, slatinaSourceColumn].Value;
                                                fileLogger.LogInfo("   Slatina added", infofilePathToWrite);
                                            }
                                            if (IsCellEmpty(excelTargetSheet.Cells[j, rouseTargetColumn].Value)
                                                && !IsCellEmpty(excelSourceSheet.Cells[i, rouseSourceColumn].Value))
                                            {
                                                excelTargetSheet.Cells[j, rouseTargetColumn].Value = excelSourceSheet.Cells[i, rouseSourceColumn].Value;
                                                fileLogger.LogInfo("   Ruse added", infofilePathToWrite);
                                            }

                                            fileLogger.LogInfo("", infofilePathToWrite);
                                        }
                                    }
                                }
                            }
                        }
                    }

                    excelPackage.Save();
                }
            }
        }

        // could be in one method
        public void ReplaceGivenColumnWithSourceOne(string sourceSheet, string targetSheet, string columnToReplace)
        {
            var fileinfo = new FileInfo(filePath);
            if (fileinfo.Exists)
            {
                using (ExcelPackage excelPackage = new ExcelPackage(fileinfo))
                {
                    ExcelWorkbook excelWorkBook = excelPackage.Workbook;

                    ExcelWorksheet excelSourceSheet = excelWorkBook.Worksheets.Where(s => s.Name == sourceSheet).First();
                    var eurocodeSourceColumn = excelSourceSheet.Cells["1:1"].First(c => c.Value.ToString() == "Eurocode").Start.Column;
                    var sourceColumn = excelSourceSheet.Cells["1:1"].First(c => c.Value.ToString() == columnToReplace).Start.Column;

                    ExcelWorksheet excelTargetSheet = excelWorkBook.Worksheets.Where(s => s.Name == targetSheet).First();
                    var eurocodeTargetColumn = excelTargetSheet.Cells["1:1"].First(c => c.Value.ToString() == "Eurocode").Start.Column;
                    var targetColumn = excelTargetSheet.Cells["1:1"].First(c => c.Value.ToString() == columnToReplace).Start.Column;

                    int targetRangeMaxRows = excelTargetSheet.Dimension.End.Row;
                    int sourceRangeMaxRows = excelSourceSheet.Dimension.End.Row;
                    fileLogger.LogInfo($"-------------------------- {columnToReplace.ToUpper()} REPLACEMENT: {targetSheet} --------------------------", infofilePathToWrite);

                    for (int i = 2; i <= targetRangeMaxRows; i++)
                    {
                        if (!IsCellEmpty(excelTargetSheet.Cells[i, eurocodeTargetColumn].Value))
                        {
                            string targetEurocodeString = excelTargetSheet.Cells[i, eurocodeTargetColumn].Value.ToString();

                            // clean eurocodes
                            targetEurocodeString = this.CleanEurocode(targetEurocodeString);
                            excelTargetSheet.Cells[i, eurocodeTargetColumn].Value = targetEurocodeString;

                            for (int j = 2; j <= sourceRangeMaxRows; j++)
                            {
                                if (!IsCellEmpty(excelSourceSheet.Cells[j, eurocodeSourceColumn].Value))
                                {
                                    string sourceEurocodeString = excelSourceSheet.Cells[j, eurocodeSourceColumn].Value.ToString();
                                    if (targetEurocodeString == sourceEurocodeString)
                                    {
                                        // match - same glass
                                        if (!IsCellEmpty(excelSourceSheet.Cells[j, sourceColumn].Value))
                                        {
                                            string neededInfo = excelSourceSheet.Cells[j, sourceColumn].Value.ToString();

                                            fileLogger.LogInfo(targetEurocodeString, infofilePathToWrite);
                                            fileLogger.LogInfo($"   {excelTargetSheet.Cells[i, targetColumn].Value } -->", infofilePathToWrite);
                                            fileLogger.LogInfo($"   {neededInfo}", infofilePathToWrite);
                                            fileLogger.LogInfo("", infofilePathToWrite);

                                            excelTargetSheet.Cells[i, targetColumn].Value = neededInfo;
                                            break;
                                        }
                                    }
                                }
                            }
                        }
                    }

                    excelPackage.Save();
                }
            }
        }

        public void AddQuantities(string sourceSheet, string targetSheet)
        {
            var fileinfo = new FileInfo(filePath);
            if (fileinfo.Exists)
            {
                using (ExcelPackage excelPackage = new ExcelPackage(fileinfo))
                {
                    ExcelWorkbook excelWorkBook = excelPackage.Workbook;

                    ExcelWorksheet excelSourceSheet = excelWorkBook.Worksheets.Where(s => s.Name == sourceSheet).First();
                    var groupSourceColumn = excelSourceSheet.Cells["1:1"].First(c => c.Value.ToString() == "Group").Start.Column;
                    var codeSourceColumn = excelSourceSheet.Cells["1:1"].First(c => c.Value.ToString() == "Code").Start.Column;
                    var liulinSourceColumn = excelSourceSheet.Cells["1:1"].First(c => c.Value.ToString() == "Liulin").Start.Column;
                    var slatinaSourceColumn = excelSourceSheet.Cells["1:1"].First(c => c.Value.ToString() == "Slatina").Start.Column;
                    var rouseSourceColumn = excelSourceSheet.Cells["1:1"].First(c => c.Value.ToString() == "Rouse").Start.Column;
                    var quantityLiulinSourceColumn = excelSourceSheet.Cells["1:1"].First(c => c.Value.ToString() == "Q_Liulin").Start.Column;
                    var quantitySlatinaSourceColumn = excelSourceSheet.Cells["1:1"].First(c => c.Value.ToString() == "Q_Slatina").Start.Column;
                    var quantityRouseSourceColumn = excelSourceSheet.Cells["1:1"].First(c => c.Value.ToString() == "Q_Rouse").Start.Column;

                    ExcelWorksheet excelTargetSheet = excelWorkBook.Worksheets.Where(s => s.Name == targetSheet).First();
                    var euroCodeTargetColumn = excelTargetSheet.Cells["1:1"].First(c => c.Value.ToString() == "Eurocode").Start.Column;
                    var liulinTargetColumn = excelTargetSheet.Cells["1:1"].First(c => c.Value.ToString() == "Liulin").Start.Column;
                    var slatinaTargetColumn = excelTargetSheet.Cells["1:1"].First(c => c.Value.ToString() == "Slatina").Start.Column;
                    var rouseTargetColumn = excelTargetSheet.Cells["1:1"].First(c => c.Value.ToString() == "Rouse").Start.Column;
                    var quantityLiulinTargetColumn = excelTargetSheet.Cells["1:1"].First(c => c.Value.ToString() == "Q_Liulin").Start.Column;
                    var quantitySlatinaTargetColumn = excelTargetSheet.Cells["1:1"].First(c => c.Value.ToString() == "Q_Slatina").Start.Column;
                    var quantityRouseTargetColumn = excelTargetSheet.Cells["1:1"].First(c => c.Value.ToString() == "Q_Rouse").Start.Column;

                    int targetRangeMaxRows = excelTargetSheet.Dimension.End.Row;
                    int sourceRangeMaxRows = excelSourceSheet.Dimension.End.Row;
                    fileLogger.LogInfo($"-------------------------- Adding Quantities: {targetSheet} --------------------------", infofilePathToWrite);

                    for (int i = 2; i <= sourceRangeMaxRows; i++)
                    {
                        bool found = false;
                        if (!IsCellEmpty(excelSourceSheet.Cells[i, groupSourceColumn].Value))
                        {
                            var groupStringName = excelSourceSheet.Cells[i, groupSourceColumn].Value.ToString();
                            if (groupStringName == targetSheet)
                            {
                                if (!IsCellEmpty(excelSourceSheet.Cells[i, codeSourceColumn].Value))
                                {
                                    string sourceCodeString = excelSourceSheet.Cells[i, codeSourceColumn].Value.ToString();
                                    for (int j = 2; j <= targetRangeMaxRows; j++)
                                    {
                                        // ---------iterate eurocode
                                        if (!IsCellEmpty(excelTargetSheet.Cells[j, euroCodeTargetColumn].Value))
                                        {
                                            string targetEurocodeString = excelTargetSheet.Cells[j, euroCodeTargetColumn].Value.ToString();
                                            if (targetEurocodeString == sourceCodeString)
                                            {
                                                // match - same glass
                                                excelTargetSheet.Cells[j, liulinTargetColumn].Value = excelSourceSheet.Cells[i, liulinSourceColumn].Value;
                                                excelTargetSheet.Cells[j, slatinaTargetColumn].Value = excelSourceSheet.Cells[i, slatinaSourceColumn].Value;
                                                excelTargetSheet.Cells[j, rouseTargetColumn].Value = excelSourceSheet.Cells[i, rouseSourceColumn].Value;
                                                excelTargetSheet.Cells[j, quantityLiulinTargetColumn].Value = excelSourceSheet.Cells[i, quantityLiulinSourceColumn].Value;
                                                excelTargetSheet.Cells[j, quantitySlatinaTargetColumn].Value = excelSourceSheet.Cells[i, quantitySlatinaSourceColumn].Value;
                                                excelTargetSheet.Cells[j, quantityRouseTargetColumn].Value = excelSourceSheet.Cells[i, quantityRouseSourceColumn].Value;
                                                fileLogger.LogInfo($"Eurocode: {targetEurocodeString }", infofilePathToWrite);
                                                found = true;
                                                break;
                                            }
                                        }
                                    }
                                    if (!found)
                                    {
                                        fileLogger.LogInfo($"Code NOT added: {sourceCodeString}", infofilePathToWrite);
                                    }
                                }
                            }
                        }
                    }

                    excelPackage.Save();
                }
            }
        }

        public void AddInterchangeables(string sourceSheet, string targetSheet)
        {
            var fileinfo = new FileInfo(filePath);
            if (fileinfo.Exists)
            {
                using (ExcelPackage excelPackage = new ExcelPackage(fileinfo))
                {
                    ExcelWorkbook excelWorkBook = excelPackage.Workbook;

                    ExcelWorksheet excelSourceSheet = excelWorkBook.Worksheets.Where(s => s.Name == sourceSheet).First();
                    var eurocodeSourceColumn = excelSourceSheet.Cells["1:1"].First(c => c.Value.ToString() == "Eurocode").Start.Column;
                    var sameAs1SourceColumn = excelSourceSheet.Cells["1:1"].First(c => c.Value.ToString() == "SameAs1").Start.Column;
                    var sameAs2SourceColumn = excelSourceSheet.Cells["1:1"].First(c => c.Value.ToString() == "SameAs2").Start.Column;
                    var sameAs3SourceColumn = excelSourceSheet.Cells["1:1"].First(c => c.Value.ToString() == "SameAs3").Start.Column;

                    ExcelWorksheet excelTargetSheet = excelWorkBook.Worksheets.Where(s => s.Name == targetSheet).First();
                    var euroCodeTargetColumn = excelTargetSheet.Cells["1:1"].First(c => c.Value.ToString() == "Eurocode").Start.Column;
                    var sameAs1TargetColumn = excelTargetSheet.Cells["1:1"].First(c => c.Value.ToString() == "SameAs1").Start.Column;
                    var sameAs2TargetColumn = excelTargetSheet.Cells["1:1"].First(c => c.Value.ToString() == "SameAs2").Start.Column;
                    var sameAs3TargetColumn = excelTargetSheet.Cells["1:1"].First(c => c.Value.ToString() == "SameAs3").Start.Column;

                    int targetRangeMaxRows = excelTargetSheet.Dimension.End.Row;
                    int sourceRangeMaxRows = excelSourceSheet.Dimension.End.Row;
                    fileLogger.LogInfo($"-------------------------- Adding Interchangeables: {targetSheet} --------------------------", infofilePathToWrite);

                    for (int i = 2; i <= targetRangeMaxRows; i++)
                    {
                        bool found = false;
                        if (!IsCellEmpty(excelTargetSheet.Cells[i, euroCodeTargetColumn].Value))
                        {
                            string targetEurocodeString = excelTargetSheet.Cells[i, euroCodeTargetColumn].Value.ToString();
                            for (int j = 2; j <= sourceRangeMaxRows; j++)
                            {
                                if (!IsCellEmpty(excelSourceSheet.Cells[j, eurocodeSourceColumn].Value))
                                {
                                    string sourceCodeString = excelSourceSheet.Cells[j, eurocodeSourceColumn].Value.ToString();
                                    if (targetEurocodeString == sourceCodeString)
                                    {
                                        // match - same glass
                                        excelTargetSheet.Cells[i, sameAs1TargetColumn].Value = excelSourceSheet.Cells[j, sameAs1SourceColumn].Value;
                                        excelTargetSheet.Cells[i, sameAs2TargetColumn].Value = excelSourceSheet.Cells[j, sameAs2SourceColumn].Value;
                                        excelTargetSheet.Cells[i, sameAs3TargetColumn].Value = excelSourceSheet.Cells[j, sameAs3SourceColumn].Value;
                                        fileLogger.LogInfo($"Eurocode: {targetEurocodeString }", infofilePathToWrite);
                                        found = true;
                                        break;
                                    }
                                }
                            }
                            if (!found)
                            {
                                fileLogger.LogInfo($"Eurocode NOT added: {targetEurocodeString}", infofilePathToWrite);
                            }
                        }
                    }

                    excelPackage.Save();
                }
            }
        }

        private string CleanEurocode(string eurocode)
        {
            // NordGlass
            if (eurocode[0] == 'M')
            {
                eurocode = eurocode.Substring(1);
            }

            // AGC and Lamex - get last 2 symbols
            string lastTwoSymbols = eurocode.Substring(eurocode.Length - 2);
            // Lamex - +
            if (lastTwoSymbols[1] == '+')
            {
                eurocode = eurocode.Substring(0, eurocode.Length - 1);
            }
            // AGC - -2
            else if (lastTwoSymbols == "-2")
            {
                eurocode = eurocode.Substring(0, eurocode.Length - 2);
            }

            return eurocode;
        }

        public bool IsCellEmpty(object cellContent)
        {
            if (cellContent != null)
            {
                if (!string.IsNullOrEmpty(cellContent.ToString()))
                {
                    return false;
                }
            }

            return true;
        }

        private HashSet<string> GetInterchangeablesForGlassType(string codeToCheck, ExcelWorksheet interchangeables, Regex reToMatch)
        {
            // interchangeables Sheet should be sorted by Eurocode. Eurocodes starting with letters are placed in the end
            // codes strarting not matching the pattern will not be checked at all

            var eurocodeInterchangeableColumn = interchangeables.Cells["1:1"].First(c => c.Value.ToString() == "Eurocode").Start.Column;
            var sameAs1Column = interchangeables.Cells["1:1"].First(c => c.Value.ToString() == "SameAs1").Start.Column;
            var sameAs2Column = interchangeables.Cells["1:1"].First(c => c.Value.ToString() == "SameAs2").Start.Column;
            var sameAs3Column = interchangeables.Cells["1:1"].First(c => c.Value.ToString() == "SameAs3").Start.Column;
            int interchangeablesMaxRows = interchangeables.Dimension.End.Row;

            var interchangeableCodes = new HashSet<string>();
            for (int k = 2; k <= interchangeablesMaxRows; k++)
            {
                if (!IsCellEmpty(interchangeables.Cells[k, eurocodeInterchangeableColumn].Value))
                {
                    string interchangeableCodeInfo = interchangeables.Cells[k, eurocodeInterchangeableColumn].Value.ToString().Substring(0, 5);

                    int stringsComparement = codeToCheck.CompareTo(interchangeableCodeInfo);
                    if (stringsComparement < 0) // interchangeableCode = 2233B
                    {
                        break;
                    }

                    if (stringsComparement == 0 && reToMatch.IsMatch(interchangeableCodeInfo))
                    {
                        // match - same glass
                        if (!IsCellEmpty(interchangeables.Cells[k, sameAs1Column].Value))
                        {
                            var interchangable1 = interchangeables.Cells[k, sameAs1Column].Value.ToString();
                            if (interchangable1.Length > 4)
                            {
                                interchangeableCodes.Add(interchangeables.Cells[k, sameAs1Column].Value.ToString().Substring(0, 5));
                            }
                        }
                        if (!IsCellEmpty(interchangeables.Cells[k, sameAs2Column].Value))
                        {
                            var interchangable2 = interchangeables.Cells[k, sameAs2Column].Value.ToString();
                            if (interchangable2.Length > 4)
                            {
                                interchangeableCodes.Add(interchangeables.Cells[k, sameAs2Column].Value.ToString().Substring(0, 5));
                            }
                        }
                        if (!IsCellEmpty(interchangeables.Cells[k, sameAs3Column].Value))
                        {
                            var interchangable3 = interchangeables.Cells[k, sameAs3Column].Value.ToString();
                            if (interchangable3.Length > 4)
                            {
                                interchangeableCodes.Add(interchangeables.Cells[k, sameAs3Column].Value.ToString().Substring(0, 5));
                            }
                        }
                    }
                }
            }

            return interchangeableCodes;
        }
    }
}
