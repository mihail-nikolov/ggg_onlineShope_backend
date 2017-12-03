namespace ProductExcelSheets.app.tests
{
    using GGG_OnlineShop.Common.Services.Contracts;
    using Moq;
    using NUnit.Framework;
    using OfficeOpenXml;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using System.Text.RegularExpressions;

    [TestFixture]
    class ExcelManagerTests
    {
        private static string projectDirectory = Directory.GetCurrentDirectory();
        private string filePath = $@"{projectDirectory}\ProductExcelSheets.app.tests\testGlasses.xlsx";
        private const int DescriptionStringMaxLength = 135;

        Mock<ILogger> mockedFileLogger;

        [SetUp]
        public void Init()
        {
            mockedFileLogger = new Mock<ILogger>();
        }

        public void RemoveFile(string filePath)
        {
            File.Delete(filePath);
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public string GetCurrentMethod()
        {
            StackTrace st = new StackTrace();
            StackFrame sf = st.GetFrame(1);

            return sf.GetMethod().Name;
        }

        // ============================================================================
        [TestCase]
        //"[^a-zA-Z0-9-]+
        public void AdaptDescription_ShouldCleanNotNeededSymbols()
        {
            string testFile = $@"{projectDirectory}\ProductExcelSheets.app.tests\testGlasses_{this.GetCurrentMethod()}.xlsx";
            if (File.Exists(testFile))
            {
                File.Delete(testFile);
            }

            File.Copy(filePath, testFile);

            ExcelManager excelManager = new ExcelManager(mockedFileLogger.Object, "test.info", testFile);

            string sheetName = "SaintGobain";
            excelManager.AdaptDescription(sheetName);

            var fileinfo = new FileInfo(testFile);
            if (fileinfo.Exists)
            {
                using (ExcelPackage excelPackage = new ExcelPackage(fileinfo))
                {
                    ExcelWorkbook excelWorkBook = excelPackage.Workbook;
                    ExcelWorksheet excelWorksheet = excelWorkBook.Worksheets.Where(s => s.Name == sheetName).FirstOrDefault();

                    var columnDescription = excelWorksheet.Cells["1:1"].First(c => c.Value.ToString() == "Description").Start.Column;

                    int rangeMaxRows = excelWorksheet.Dimension.End.Row;
                    for (int i = 2; i <= rangeMaxRows; i++)
                    {
                        string cellStrDescription = excelWorksheet.Cells[i, columnDescription].Value.ToString();

                        Assert.IsTrue(cellStrDescription.Length <= DescriptionStringMaxLength);

                        Regex re = new Regex(@"^[a-zA-Z0-9- ]+$");
                        Assert.IsTrue(re.IsMatch(cellStrDescription));
                    }
                }
            }

            this.RemoveFile(testFile);
        }

        // ============================================================================
        [TestCase]
        public void ReplaceGivenColumnWithSourceOne_ShouldUseTheSourceContent_ForDescriptionByEurocode()
        {
            string testFile = $@"{projectDirectory}\ProductExcelSheets.app.tests\testGlasses_{this.GetCurrentMethod()}.xlsx";
            if (File.Exists(testFile))
            {
                File.Delete(testFile);
            }

            File.Copy(filePath, testFile);

            ExcelManager excelManager = new ExcelManager(mockedFileLogger.Object, "test.info", testFile);

            string sourceSheet = "SaintGobain";
            string targetSheet = "NordGlass";
            excelManager.ReplaceGivenColumnWithSourceOne(sourceSheet, targetSheet, "Description");

            var fileinfo = new FileInfo(testFile);
            if (fileinfo.Exists)
            {
                using (ExcelPackage excelPackage = new ExcelPackage(fileinfo))
                {
                    ExcelWorkbook excelWorkBook = excelPackage.Workbook;
                    ExcelWorksheet excelWorksheet = excelWorkBook.Worksheets.Where(s => s.Name == targetSheet).FirstOrDefault();

                    var columnDescription = excelWorksheet.Cells["1:1"].First(c => c.Value.ToString() == "Description").Start.Column;
                    var columnEurocode = excelWorksheet.Cells["1:1"].First(c => c.Value.ToString() == "Eurocode").Start.Column;

                    int rangeMaxRows = excelWorksheet.Dimension.End.Row;
                    for (int i = 2; i <= rangeMaxRows; i++)
                    {
                        string cellStrDescription = excelWorksheet.Cells[i, columnDescription].Value.ToString();

                        if (!excelManager.IsCellEmpty(excelWorksheet.Cells[i, columnEurocode].Value))
                        {
                            string cellStrEurocode = excelWorksheet.Cells[i, columnEurocode].Value.ToString();
                            Assert.IsFalse(cellStrEurocode.StartsWith("M"));
                            Assert.IsFalse(cellStrEurocode.EndsWith("-2"));
                            Assert.IsFalse(cellStrEurocode.EndsWith("+"));

                            if (cellStrEurocode == "8534AGNGN" || cellStrEurocode == "8834AGN")
                            {
                                Assert.IsTrue(cellStrDescription.Contains("Saint-Gobain"));
                            }
                            else
                            {
                                Assert.IsTrue(cellStrDescription.Contains("NordGlass"));
                            }
                        }
                        else
                        {
                            // The case with empty eurocode
                            Assert.IsTrue(cellStrDescription.Contains("NordGlass"));
                        }
                    }
                }
            }

            this.RemoveFile(testFile);
        }

        // ============================================================================
        [TestCase]
        public void ReplaceGivenColumnWithSourceOne_ShouldUseTheSourceContent_ForOESCodeByEurocode()
        {
            string testFile = $@"{projectDirectory}\ProductExcelSheets.app.tests\testGlasses_{this.GetCurrentMethod()}.xlsx";
            if (File.Exists(testFile))
            {
                File.Delete(testFile);
            }

            File.Copy(filePath, testFile);

            ExcelManager excelManager = new ExcelManager(mockedFileLogger.Object, "test.info", testFile);

            string sourceSheet = "SaintGobain";
            string targetSheet = "NordGlass";
            excelManager.ReplaceGivenColumnWithSourceOne(sourceSheet, targetSheet, "OESCode");

            var fileinfo = new FileInfo(testFile);
            if (fileinfo.Exists)
            {
                using (ExcelPackage excelPackage = new ExcelPackage(fileinfo))
                {
                    ExcelWorkbook excelWorkBook = excelPackage.Workbook;
                    ExcelWorksheet excelWorksheet = excelWorkBook.Worksheets.Where(s => s.Name == targetSheet).FirstOrDefault();

                    var columnOES = excelWorksheet.Cells["1:1"].First(c => c.Value.ToString() == "OESCode").Start.Column;
                    var columnEurocode = excelWorksheet.Cells["1:1"].First(c => c.Value.ToString() == "Eurocode").Start.Column;

                    int rangeMaxRows = excelWorksheet.Dimension.End.Row;
                    for (int i = 2; i <= rangeMaxRows; i++)
                    {
                        if (!excelManager.IsCellEmpty(excelWorksheet.Cells[i, columnEurocode].Value))
                        {
                            string cellStrEurocode = excelWorksheet.Cells[i, columnEurocode].Value.ToString();
                            Assert.IsFalse(cellStrEurocode.StartsWith("M"));
                            Assert.IsFalse(cellStrEurocode.EndsWith("-2"));
                            Assert.IsFalse(cellStrEurocode.EndsWith("+"));

                            if (cellStrEurocode == "8834AGN")
                            {
                                string cellStrOES = excelWorksheet.Cells[i, columnOES].Value.ToString();
                                Assert.AreEqual(cellStrOES, "testOES");
                            }
                            else
                            {
                                Assert.IsTrue(excelManager.IsCellEmpty(excelWorksheet.Cells[i, columnOES].Value));
                            }
                        }
                        else
                        {
                            // The case with empty eurocode
                            Assert.IsTrue(excelManager.IsCellEmpty(excelWorksheet.Cells[i, columnOES].Value));
                        }
                    }
                }
            }

            this.RemoveFile(testFile);
        }

        // ============================================================================
        [TestCase]
        public void AddQuantities_ShouldRecognizeByEurocodeAndManifacturer_And_addquantitiesWitPlace()
        {
            string testFile = $@"{projectDirectory}\ProductExcelSheets.app.tests\testGlasses_{this.GetCurrentMethod()}.xlsx";
            if (File.Exists(testFile))
            {
                File.Delete(testFile);
            }

            File.Copy(filePath, testFile);

            ExcelManager excelManager = new ExcelManager(mockedFileLogger.Object, "test.info", testFile);

            string sourceSheet = "quantities";
            string targetSheet = "SaintGobain";
            excelManager.AddQuantities(sourceSheet, targetSheet);

            var fileinfo = new FileInfo(testFile);
            if (fileinfo.Exists)
            {
                using (ExcelPackage excelPackage = new ExcelPackage(fileinfo))
                {
                    ExcelWorkbook excelWorkBook = excelPackage.Workbook;
                    ExcelWorksheet excelWorksheet = excelWorkBook.Worksheets.Where(s => s.Name == targetSheet).FirstOrDefault();

                    var euroCodeTargetColumn = excelWorksheet.Cells["1:1"].First(c => c.Value.ToString() == "Eurocode").Start.Column;
                    var liulinTargetColumn = excelWorksheet.Cells["1:1"].First(c => c.Value.ToString() == "Liulin").Start.Column;
                    var slatinaTargetColumn = excelWorksheet.Cells["1:1"].First(c => c.Value.ToString() == "Slatina").Start.Column;
                    var rouseTargetColumn = excelWorksheet.Cells["1:1"].First(c => c.Value.ToString() == "Rouse").Start.Column;
                    var quantityLiulinTargetColumn = excelWorksheet.Cells["1:1"].First(c => c.Value.ToString() == "Q_Liulin").Start.Column;
                    var quantitySlatinaTargetColumn = excelWorksheet.Cells["1:1"].First(c => c.Value.ToString() == "Q_Slatina").Start.Column;
                    var quantityRouseTargetColumn = excelWorksheet.Cells["1:1"].First(c => c.Value.ToString() == "Q_Rouse").Start.Column;

                    int rangeMaxRows = excelWorksheet.Dimension.End.Row;
                    for (int i = 2; i <= rangeMaxRows; i++)
                    {
                        string cellStrEurocode = excelWorksheet.Cells[i, euroCodeTargetColumn].Value.ToString();
                        if (cellStrEurocode == "2436LGNS4RD")
                        {
                            string cellLiulin = excelWorksheet.Cells[i, liulinTargetColumn].Value.ToString();
                            Assert.AreEqual(cellLiulin, "Стойка 1");
                            string cellSlatina = excelWorksheet.Cells[i, slatinaTargetColumn].Value.ToString();
                            Assert.AreEqual(cellSlatina, "Стойка 2");
                            string cellRuse = excelWorksheet.Cells[i, rouseTargetColumn].Value.ToString();
                            Assert.AreEqual(cellRuse, "Стойка 3");
                            string cellQLiulin = excelWorksheet.Cells[i, quantityLiulinTargetColumn].Value.ToString();
                            Assert.AreEqual(cellQLiulin, "0");
                            string cellQSlatina = excelWorksheet.Cells[i, quantitySlatinaTargetColumn].Value.ToString();
                            Assert.AreEqual(cellQSlatina, "1");
                            string cellQRuse = excelWorksheet.Cells[i, quantityRouseTargetColumn].Value.ToString();
                            Assert.AreEqual(cellQRuse, "0");
                        }
                        else if (cellStrEurocode == "3030ASMM")
                        {
                            string cellLiulin = excelWorksheet.Cells[i, liulinTargetColumn].Value.ToString();
                            Assert.AreEqual(cellLiulin, "16");
                            string cellSlatina = excelWorksheet.Cells[i, slatinaTargetColumn].Value.ToString();
                            Assert.AreEqual(cellSlatina, "18");
                            string cellRuse = excelWorksheet.Cells[i, rouseTargetColumn].Value.ToString();
                            Assert.AreEqual(cellRuse, "35");
                            string cellQLiulin = excelWorksheet.Cells[i, quantityLiulinTargetColumn].Value.ToString();
                            Assert.AreEqual(cellQLiulin, "0");
                            string cellQSlatina = excelWorksheet.Cells[i, quantitySlatinaTargetColumn].Value.ToString();
                            Assert.AreEqual(cellQSlatina, "1");
                            string cellQRuse = excelWorksheet.Cells[i, quantityRouseTargetColumn].Value.ToString();
                            Assert.AreEqual(cellQRuse, "2");
                        }
                        else
                        {
                            Assert.IsTrue(excelManager.IsCellEmpty(excelWorksheet.Cells[i, liulinTargetColumn].Value));
                            Assert.IsTrue(excelManager.IsCellEmpty(excelWorksheet.Cells[i, slatinaTargetColumn].Value));
                            Assert.IsTrue(excelManager.IsCellEmpty(excelWorksheet.Cells[i, rouseTargetColumn].Value));
                            Assert.IsTrue(excelManager.IsCellEmpty(excelWorksheet.Cells[i, quantityLiulinTargetColumn].Value));
                            Assert.IsTrue(excelManager.IsCellEmpty(excelWorksheet.Cells[i, quantitySlatinaTargetColumn].Value));
                            Assert.IsTrue(excelManager.IsCellEmpty(excelWorksheet.Cells[i, quantityRouseTargetColumn].Value));
                        }
                    }
                }
            }

            this.RemoveFile(testFile);
        }

        // ============================================================================
        [TestCase]
        public void AddInterchangeables_ShouldRecognizeByEurocodeAndAddThem()
        {
            string testFile = $@"{projectDirectory}\ProductExcelSheets.app.tests\testGlasses_{this.GetCurrentMethod()}.xlsx";
            if (File.Exists(testFile))
            {
                File.Delete(testFile);
            }

            File.Copy(filePath, testFile);

            ExcelManager excelManager = new ExcelManager(mockedFileLogger.Object, "test.info", testFile);

            string sourceSheet = "interchangeables";
            string targetSheet = "SaintGobain";
            excelManager.AddInterchangeables(sourceSheet, targetSheet);

            var fileinfo = new FileInfo(testFile);
            if (fileinfo.Exists)
            {
                using (ExcelPackage excelPackage = new ExcelPackage(fileinfo))
                {
                    ExcelWorkbook excelWorkBook = excelPackage.Workbook;
                    ExcelWorksheet excelWorksheet = excelWorkBook.Worksheets.Where(s => s.Name == targetSheet).FirstOrDefault();

                    var euroCodeTargetColumn = excelWorksheet.Cells["1:1"].First(c => c.Value.ToString() == "Eurocode").Start.Column;
                    var sameAs1TargetColumn = excelWorksheet.Cells["1:1"].First(c => c.Value.ToString() == "SameAs1").Start.Column;
                    var sameAs2TargetColumn = excelWorksheet.Cells["1:1"].First(c => c.Value.ToString() == "SameAs2").Start.Column;
                    var sameAs3TargetColumn = excelWorksheet.Cells["1:1"].First(c => c.Value.ToString() == "SameAs3").Start.Column;

                    int rangeMaxRows = excelWorksheet.Dimension.End.Row;
                    for (int i = 2; i <= rangeMaxRows; i++)
                    {
                        string cellStrEurocode = excelWorksheet.Cells[i, euroCodeTargetColumn].Value.ToString();
                        if (cellStrEurocode == "8534AGNGN")
                        {
                            string cellSameAs1 = excelWorksheet.Cells[i, sameAs1TargetColumn].Value.ToString();
                            Assert.AreEqual(cellSameAs1, "testEurocodeAGNGN");
                            string cellSameAs2 = excelWorksheet.Cells[i, sameAs2TargetColumn].Value.ToString();
                            Assert.AreEqual(cellSameAs2, "testEurocodeAGNGN2");
                            Assert.IsTrue(excelManager.IsCellEmpty(excelWorksheet.Cells[i, sameAs3TargetColumn].Value));
                        }
                        else if (cellStrEurocode == "8834AGN")
                        {
                            string cellSameAs1 = excelWorksheet.Cells[i, sameAs1TargetColumn].Value.ToString();
                            Assert.AreEqual(cellSameAs1, "4633AGN");
                            string cellSameAs2 = excelWorksheet.Cells[i, sameAs2TargetColumn].Value.ToString();
                            Assert.AreEqual(cellSameAs2, "7253AGN");
                            Assert.IsTrue(excelManager.IsCellEmpty(excelWorksheet.Cells[i, sameAs3TargetColumn].Value));
                        }
                        else
                        {
                            Assert.IsTrue(excelManager.IsCellEmpty(excelWorksheet.Cells[i, sameAs1TargetColumn].Value));
                            Assert.IsTrue(excelManager.IsCellEmpty(excelWorksheet.Cells[i, sameAs2TargetColumn].Value));
                            Assert.IsTrue(excelManager.IsCellEmpty(excelWorksheet.Cells[i, sameAs3TargetColumn].Value));
                        }
                    }
                }
            }

            this.RemoveFile(testFile);
        }

        // ============================================================================
        [TestCase]
        public void AdaptProductsPlace_ShouldRecognizeByEurocodeAndAddThem()
        {
            string testFile = $@"{projectDirectory}\ProductExcelSheets.app.tests\testGlasses_{this.GetCurrentMethod()}.xlsx";
            if (File.Exists(testFile))
            {
                File.Delete(testFile);
            }

            File.Copy(filePath, testFile);

            ExcelManager excelManager = new ExcelManager(mockedFileLogger.Object, "test.info", testFile);

            string quantitiesSheet = "quantities";
            string interchangeablesSheet = "interchangeables";
            string targetSheet = "SaintGobain";
            excelManager.AdaptProductsPlace(quantitiesSheet, targetSheet, interchangeablesSheet);

            var fileinfo = new FileInfo(testFile);
            if (fileinfo.Exists)
            {
                using (ExcelPackage excelPackage = new ExcelPackage(fileinfo))
                {
                    ExcelWorkbook excelWorkBook = excelPackage.Workbook;
                    ExcelWorksheet excelWorksheet = excelWorkBook.Worksheets.Where(s => s.Name == targetSheet).FirstOrDefault();

                    var euroCodeTargetColumn = excelWorksheet.Cells["1:1"].First(c => c.Value.ToString() == "Eurocode").Start.Column;
                    var liulinTargetColumn = excelWorksheet.Cells["1:1"].First(c => c.Value.ToString() == "Liulin").Start.Column;
                    var slatinaTargetColumn = excelWorksheet.Cells["1:1"].First(c => c.Value.ToString() == "Slatina").Start.Column;
                    var rouseTargetColumn = excelWorksheet.Cells["1:1"].First(c => c.Value.ToString() == "Rouse").Start.Column;

                    int rangeMaxRows = excelWorksheet.Dimension.End.Row;
                    for (int i = 2; i <= rangeMaxRows; i++)
                    {
                        string cellStrEurocode = excelWorksheet.Cells[i, euroCodeTargetColumn].Value.ToString();
                        if (cellStrEurocode == "2436LGNS4RD")
                        {
                            string cellLiulin = excelWorksheet.Cells[i, liulinTargetColumn].Value.ToString();
                            Assert.AreEqual(cellLiulin, "Стойка 1");
                            string cellSlatina = excelWorksheet.Cells[i, slatinaTargetColumn].Value.ToString();
                            Assert.AreEqual(cellSlatina, "Стойка 2");
                            string cellRuse = excelWorksheet.Cells[i, rouseTargetColumn].Value.ToString();
                            Assert.AreEqual(cellRuse, "Стойка 3");
                        }
                        else if (cellStrEurocode == "3030ASMM")
                        {
                            string cellLiulin = excelWorksheet.Cells[i, liulinTargetColumn].Value.ToString();
                            Assert.AreEqual(cellLiulin, "16");
                            string cellSlatina = excelWorksheet.Cells[i, slatinaTargetColumn].Value.ToString();
                            Assert.AreEqual(cellSlatina, "18");
                            string cellRuse = excelWorksheet.Cells[i, rouseTargetColumn].Value.ToString();
                            Assert.AreEqual(cellRuse, "35");
                        }
                        else if (cellStrEurocode == "8534AGNGN" || cellStrEurocode == "testEurocodeAGNGN")
                        {
                            string cellLiulin = excelWorksheet.Cells[i, liulinTargetColumn].Value.ToString();
                            Assert.AreEqual(cellLiulin, "3");
                            string cellSlatina = excelWorksheet.Cells[i, slatinaTargetColumn].Value.ToString();
                            Assert.AreEqual(cellSlatina, "3");
                            string cellRuse = excelWorksheet.Cells[i, rouseTargetColumn].Value.ToString();
                            Assert.AreEqual(cellRuse, "3");
                        }
                        else if (cellStrEurocode == "4633AGN" || cellStrEurocode == "8834AGN")
                        {
                            string cellLiulin = excelWorksheet.Cells[i, liulinTargetColumn].Value.ToString();
                            Assert.AreEqual(cellLiulin, "55");
                            string cellSlatina = excelWorksheet.Cells[i, slatinaTargetColumn].Value.ToString();
                            Assert.AreEqual(cellSlatina, "55");
                            string cellRuse = excelWorksheet.Cells[i, rouseTargetColumn].Value.ToString();
                            Assert.AreEqual(cellRuse, "55");
                        }
                        else
                        {
                            Assert.IsTrue(excelManager.IsCellEmpty(excelWorksheet.Cells[i, liulinTargetColumn].Value));
                            Assert.IsTrue(excelManager.IsCellEmpty(excelWorksheet.Cells[i, slatinaTargetColumn].Value));
                            Assert.IsTrue(excelManager.IsCellEmpty(excelWorksheet.Cells[i, rouseTargetColumn].Value));
                        }
                    }
                }
            }

            this.RemoveFile(testFile);
        }
    }
}
