namespace ProductExcelSheets.app
{
    using GGG_OnlineShop.Common.Services.Contracts;
    using System;

    public class Engine
    {
        private string filePathName;
        private string errorsfilePathToWrite;

        public Engine(ILogger consoleWriter, ILogger fileLogger,
                      IReader consoleReader, string filePathName,
                      IExcelManager ExcelManager, string errorsfilePathToWrite, string infofilePathToWrite)
        {
            this.filePathName = filePathName;
            this.errorsfilePathToWrite = errorsfilePathToWrite;
            this.FileLogger = fileLogger;
            this.ConsoleWriter = consoleWriter;
            this.ConsoleReader = consoleReader;
            this.ExcelManager = ExcelManager;
        }

        public ILogger ConsoleWriter { get; set; }

        public ILogger FileLogger { get; set; }

        public IReader ConsoleReader{ get; set; }

        public IExcelManager ExcelManager { get; set; }

        public void Startup()
        {
            ConsoleWriter.LogInfo($"used file: {this.filePathName}");
            Help();

            ConsoleWriter.LogInfo("Enter command number:");
            int command = -1;
            while (command != 0)
            {
                if (int.TryParse(ConsoleReader.Read(), out command))
                {
                    switch (command)
                    {
                        case 0: return;
                        case 1: AdaptDescription(); break;
                        case 2: ReplaceDescription(); break;
                        case 3: AddQuantities(); break;
                        case 4: AddInterchangeables(); break;
                        case 5: AddOESCode(); break;
                        case 6: AddAndAdaptPlaces(); break;
                        default: Help(); break;
                    }
                    ConsoleWriter.LogInfo("Enter command number:");
                }
            }
        }

        public void ReplaceDescription()
        {
            string baseSheetName = "";
            string[] targetSheets = new string[] { };
            try
            {
                ConsoleWriter.LogInfo("Enter baseSheet name. Should be 1 word, no spaces");
                baseSheetName = ConsoleReader.Read();

                ConsoleWriter.LogInfo("Enter sheets you want to replace description column with baseSheet's one; names separated by spaces. No empty spaces in sheet names!");
                targetSheets = ConsoleReader.Read().Split(' ');
            }
            catch (Exception)
            {
                ConsoleWriter.LogError("Input not entered correctly");
            }

            foreach (var targetSheetName in targetSheets)
            {
                try
                {
                    ExcelManager.ReplaceGivenColumnWithSourceOne(baseSheetName, targetSheetName, "Description");
                }
                catch (Exception e)
                {
                    FileLogger.LogError($"-------------------------- DESCRIPTION REPLACEMENT: {targetSheetName} --------------------------", errorsfilePathToWrite);
                    FileLogger.LogError(e.Message, errorsfilePathToWrite);
                }
            }
        }

        public void AdaptDescription()
        {
            string[] sheetsToAdaptDescriptionColumn = new string[] { };
            try
            {
                ConsoleWriter.LogInfo("Enter sheet names you want to adapt the descripton column, separated by spaces. No empty spaces in sheet names!");
                sheetsToAdaptDescriptionColumn = ConsoleReader.Read().Split(' ');
            }
            catch (Exception)
            {
                ConsoleWriter.LogError("sheet names not entered correctly");
            }

            foreach (var sheetName in sheetsToAdaptDescriptionColumn)
            {
                try
                {
                    ExcelManager.AdaptDescription(sheetName);
                }
                catch (Exception e)
                {
                    FileLogger.LogError($"-------------------------- DESCRIPTION ADAPTATION: {sheetName} --------------------------", errorsfilePathToWrite);
                    FileLogger.LogError(e.Message, errorsfilePathToWrite);
                }
            }
        }

        public void AddQuantities()
        {
            ConsoleWriter.LogInfo("Enter quantitiesSheet name. Should be 1 word, no spaces");
            string quantitiesSheetName = "";
            string[] sheetsToAddQuantities = new string[] { };
            try
            {
                quantitiesSheetName = ConsoleReader.Read();

                ConsoleWriter.LogInfo("Enter sheet names for which you want to add quantities.");
                sheetsToAddQuantities = ConsoleReader.Read().Split(' ');
            }
            catch (Exception)
            {
                ConsoleWriter.LogError("input not entered correctly");
            }

            foreach (var sheetName in sheetsToAddQuantities)
            {
                try
                {
                    ExcelManager.AddQuantities(quantitiesSheetName, sheetName);
                }
                catch (Exception e)
                {
                    FileLogger.LogError($"-------------------------- Adding Quantities: {sheetName} --------------------------", errorsfilePathToWrite);
                    FileLogger.LogError(e.Message, errorsfilePathToWrite);
                }
            }
        }

        public void AddInterchangeables()
        {
            ConsoleWriter.LogInfo("Enter interchangeable parts sheet name. Should be 1 word, no spaces");
            string interchangeablesSheetName = "";
            string[] sheetsToAddQuantities = new string[] { };
            try
            {
                interchangeablesSheetName = ConsoleReader.Read();

                ConsoleWriter.LogInfo("Enter sheet names for which you want to add interchangeable parts.");
                sheetsToAddQuantities = ConsoleReader.Read().Split(' ');
            }
            catch (Exception)
            {
                ConsoleWriter.LogError("input not entered correctly");
            }

            foreach (var sheetName in sheetsToAddQuantities)
            {
                try
                {
                    ExcelManager.AddInterchangeables(interchangeablesSheetName, sheetName);
                }
                catch (Exception e)
                {
                    FileLogger.LogError($"-------------------------- Adding interchangeables: {sheetName} --------------------------", errorsfilePathToWrite);
                    FileLogger.LogError(e.Message, errorsfilePathToWrite);
                }
            }
        }

        public void AddOESCode()
        {
            ConsoleWriter.LogInfo("Enter baseSheet(source) name. Should be 1 word, no spaces");
            string baseSheetName = "";
            string[] targetSheets = new string[] { };
            try
            {
                baseSheetName = ConsoleReader.Read();

                ConsoleWriter.LogInfo("Enter sheets you want to add OESCode column with baseSheet's one; names separated by spaces. No empty spaces in sheet names!");
                targetSheets = ConsoleReader.Read().Split(' ');
            }
            catch (Exception)
            {
                ConsoleWriter.LogError("input not entered correctly");
            }

            foreach (var targetSheetName in targetSheets)
            {
                try
                {
                    ExcelManager.ReplaceGivenColumnWithSourceOne(baseSheetName, targetSheetName, "OESCode");
                }
                catch (Exception e)
                {
                    FileLogger.LogError($"-------------------------- OESCode add: {targetSheetName} --------------------------", errorsfilePathToWrite);
                    FileLogger.LogError(e.Message, errorsfilePathToWrite);
                }
            }
        }

        public void AddAndAdaptPlaces()
        {
            ConsoleWriter.LogInfo("Enter quantitiesSheet name. Should be 1 word, no spaces");
            string quantitiesSheetName = "";
            string interchangeablesSheet = "";
            string[] sheetsToAddQuantities = new string[] { };
            try
            {
                quantitiesSheetName = ConsoleReader.Read();

                ConsoleWriter.LogInfo("Enter interchangeables Sheet name. Should be 1 word, no spaces");
                interchangeablesSheet = ConsoleReader.Read();

                ConsoleWriter.LogInfo("Enter sheet names for which you want to adapt product places.");
                sheetsToAddQuantities = ConsoleReader.Read().Split(' ');
            }
            catch (Exception)
            {
                ConsoleWriter.LogError("input not entered correctly");
            }

            foreach (var sheetName in sheetsToAddQuantities)
            {
                try
                {
                    ExcelManager.AdaptProductsPlace(quantitiesSheetName, sheetName, interchangeablesSheet);
                }
                catch (Exception e)
                {
                    FileLogger.LogError($"-------------------------- Adding Product Places: {sheetName} --------------------------", errorsfilePathToWrite);
                    FileLogger.LogError(e.Message, errorsfilePathToWrite);
                }
            }
        }

        public void Help()
        {
            string info = @"
0.Exit\n
1.Adapt Description
2.Replace Description with baseSheet info
3.Add quantities
4.Add interchangeable parts
5.Add OESCode
6.Adapt products place
";
            ConsoleWriter.LogInfo(info);
        }
    }
}
