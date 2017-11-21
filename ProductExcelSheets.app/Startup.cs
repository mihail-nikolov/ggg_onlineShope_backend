namespace ProductExcelSheets.app
{
    using GGG_OnlineShop.Common.Services;
    using System;
    using System.IO;

    class Startup
    {
        static void Main()
        {
            string projectDirectory = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName;
            string errorsfilePathToWrite = $@"{projectDirectory}\ExcelManagerErrors_{DateTime.Now.ToString("ddMMyy_HHmm")}.txt";
            string infofilePathToWrite = $@"{projectDirectory}\ExcelManagerInfo_{DateTime.Now.ToString("ddMMyy_HHmm")}.txt";

            ConsoleWriter consoleWriter = new ConsoleWriter();
            FileLogger fileLogger = new FileLogger();
            ConsoleReader consoleReader = new ConsoleReader();
            string filePathName = @"D:\my documents\professional\GGG_OnlineShop\ggg\glasses.xlsx"; // TODO should not be hardcoded
            ExcelManager excelManager = new ExcelManager(fileLogger, infofilePathToWrite, filePathName);

            Engine engine = new Engine(consoleWriter, fileLogger, consoleReader, filePathName, excelManager, errorsfilePathToWrite, infofilePathToWrite);
        }
    }
}
