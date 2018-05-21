namespace ProductExcelSheets.app.tests
{
    using app;
    using GGG_OnlineShop.Common.Services.Contracts;
    using Mocked;
    using Moq;
    using NUnit.Framework;
    using System;
    using System.Collections.Generic;

    [TestFixture]
    class EngineTests
    {
        Mock<ILogger> mockedConsoleWriter;
        Mock<ILogger> mockedFileLogger;
        Mock<IExcelManager> mockedExcelManager;

        [SetUp]
        public void Init()
        {
            mockedConsoleWriter = new Mock<ILogger>();
            mockedFileLogger = new Mock<ILogger>();
            mockedExcelManager = new Mock<IExcelManager>();
        }

        [TestCase]
        public void Startup_ShouldCallHelpInfoAndWorksOK_WhenNotRecognizedCommand()
        {
            var arguments = new Queue<string>(new string[] { "10", "0" });
            var mockedReader = new MockedReader(arguments);
            Engine engine = new Engine(mockedConsoleWriter.Object, mockedFileLogger.Object,
                          mockedReader, "test", mockedExcelManager.Object, "test.err", "test.info");
            engine.Startup();
            string info = @"
0.Exit\n
1.Adapt Description
2.Replace Description with baseSheet info
3.Add quantities
4.Add interchangeable parts
5.Add OESCode
6.Adapt products place
7.ReplaceGivenColumnWithSourceOneByGivenColumnMatch
";
            mockedConsoleWriter.Verify(x => x.LogInfo(info, ""), Times.Exactly(2));
        }

        // ============================================================================
        [TestCase]
        public void Startup_ShouldCallReplaceDescriptionAndWorksOK_WhenCalled_With2()
        {
            var arguments = new Queue<string>(new string[] { "2", "sourceSheetName", "targetSheet1 targetSheet2", "0" });
            var mockedReader = new MockedReader(arguments);
            Engine engine = new Engine(mockedConsoleWriter.Object, mockedFileLogger.Object,
                          mockedReader, "test", mockedExcelManager.Object, "test.err", "test.info");
            engine.Startup();

            mockedExcelManager.Verify(x => x.ReplaceGivenColumnWithSourceOne("sourceSheetName", "targetSheet1", "Description"),
                                    Times.Once);

            mockedExcelManager.Verify(x => x.ReplaceGivenColumnWithSourceOne("sourceSheetName", "targetSheet2", "Description"),
                                   Times.Once);
        }

        [TestCase]
        public void Startup_ShouldCallReplaceDescriptionAndCatchException_WhenCalledWith2AndNOK()
        {
            var arguments = new Queue<string>(new string[] { "2", "sourceSheetName", "targetSheet1", "0" });
            var mockedReader = new MockedReader(arguments);

            mockedExcelManager.Setup(x => x.ReplaceGivenColumnWithSourceOne(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
              .Throws(new ArgumentException("There is not such a sheet"));

            Engine engine = new Engine(mockedConsoleWriter.Object, mockedFileLogger.Object,
                          mockedReader, "test", mockedExcelManager.Object, "test.err", "test.info");
            engine.Startup();

            mockedFileLogger.Verify(x => x.LogError("-------------------------- DESCRIPTION REPLACEMENT: targetSheet1 --------------------------",
                                        "test.err"), Times.Once);

            mockedFileLogger.Verify(x => x.LogError("There is not such a sheet", "test.err"), Times.Once);
        }

        // ============================================================================
        [TestCase]
        public void Startup_ShouldCallAdaptDescriptionAndWorksOK_WhenCalled_With1()
        {
            var arguments = new Queue<string>(new string[] { "1", "sourceSheetName", "0" });
            var mockedReader = new MockedReader(arguments);
            Engine engine = new Engine(mockedConsoleWriter.Object, mockedFileLogger.Object,
                          mockedReader, "test", mockedExcelManager.Object, "test.err", "test.info");
            engine.Startup();

            mockedExcelManager.Verify(x => x.AdaptDescription("sourceSheetName"),
                                    Times.Once);

            mockedExcelManager.Verify(x => x.AdaptDescription("sourceSheetName"),
                                   Times.Once);
        }

        [TestCase]
        public void Startup_ShouldCallAdaptDescriptionAndCatchException_WhenCalledAndNOK()
        {
            var arguments = new Queue<string>(new string[] { "1", "sourceSheetName", "0" });
            var mockedReader = new MockedReader(arguments);

            mockedExcelManager.Setup(x => x.AdaptDescription(It.IsAny<string>()))
              .Throws(new ArgumentException("There is not such a sheet"));

            Engine engine = new Engine(mockedConsoleWriter.Object, mockedFileLogger.Object,
                          mockedReader, "test", mockedExcelManager.Object, "test.err", "test.info");
            engine.Startup();

            mockedFileLogger.Verify(x => x.LogError("-------------------------- DESCRIPTION ADAPTATION: sourceSheetName --------------------------",
                                        "test.err"), Times.Once);

            mockedFileLogger.Verify(x => x.LogError("There is not such a sheet", "test.err"), Times.Once);
        }

        // ============================================================================
        [TestCase]
        public void Startup_ShouldCallAddQuantitiesAndWorksOK_WhenCalled_With3()
        {
            var arguments = new Queue<string>(new string[] { "3", "sourceSheetName", "targetSheet1 targetSheet2", "0" });
            var mockedReader = new MockedReader(arguments);
            Engine engine = new Engine(mockedConsoleWriter.Object, mockedFileLogger.Object,
                          mockedReader, "test", mockedExcelManager.Object, "test.err", "test.info");
            engine.Startup();

            mockedExcelManager.Verify(x => x.AddQuantities("sourceSheetName", "targetSheet1"),
                                    Times.Once);

            mockedExcelManager.Verify(x => x.AddQuantities("sourceSheetName", "targetSheet2"),
                                   Times.Once);
        }

        [TestCase]
        public void Startup_ShouldCallAddQuantitiesAndCatchException_WhenCalledWith3AndNOK()
        {
            var arguments = new Queue<string>(new string[] { "3", "sourceSheetName", "targetSheet1", "0" });
            var mockedReader = new MockedReader(arguments);

            mockedExcelManager.Setup(x => x.AddQuantities(It.IsAny<string>(), It.IsAny<string>()))
              .Throws(new ArgumentException("There is not such a sheet"));

            Engine engine = new Engine(mockedConsoleWriter.Object, mockedFileLogger.Object,
                          mockedReader, "test", mockedExcelManager.Object, "test.err", "test.info");
            engine.Startup();

            mockedFileLogger.Verify(x => x.LogError("-------------------------- Adding Quantities: targetSheet1 --------------------------",
                                        "test.err"), Times.Once);

            mockedFileLogger.Verify(x => x.LogError("There is not such a sheet", "test.err"), Times.Once);
        }

        // ============================================================================
        [TestCase]
        public void Startup_ShouldCallAddInterchangeablesAndWorksOK_WhenCalled_With4()
        {
            var arguments = new Queue<string>(new string[] { "4", "sourceSheetName", "targetSheet1 targetSheet2", "0" });
            var mockedReader = new MockedReader(arguments);
            Engine engine = new Engine(mockedConsoleWriter.Object, mockedFileLogger.Object,
                          mockedReader, "test", mockedExcelManager.Object, "test.err", "test.info");
            engine.Startup();

            mockedExcelManager.Verify(x => x.AddInterchangeables("sourceSheetName", "targetSheet1"), Times.Once);
            mockedExcelManager.Verify(x => x.AddInterchangeables("sourceSheetName", "targetSheet2"), Times.Once);
        }

        [TestCase]
        public void Startup_ShouldCallAddInterchangeablesAndCatchException_WhenCalledWith4AndNOK()
        {
            var arguments = new Queue<string>(new string[] { "4", "sourceSheetName", "targetSheet1", "0" });
            var mockedReader = new MockedReader(arguments);

            mockedExcelManager.Setup(x => x.AddInterchangeables(It.IsAny<string>(), It.IsAny<string>()))
              .Throws(new ArgumentException("There is not such a sheet"));

            Engine engine = new Engine(mockedConsoleWriter.Object, mockedFileLogger.Object,
                          mockedReader, "test", mockedExcelManager.Object, "test.err", "test.info");
            engine.Startup();

            mockedFileLogger.Verify(x => x.LogError("-------------------------- Adding interchangeables: targetSheet1 --------------------------",
                                        "test.err"), Times.Once);

            mockedFileLogger.Verify(x => x.LogError("There is not such a sheet", "test.err"), Times.Once);
        }

        // ============================================================================
        [TestCase]
        public void Startup_ShouldCallAddOESCodeAndWorksOK_WhenCalled_With5()
        {
            var arguments = new Queue<string>(new string[] { "5", "sourceSheetName", "targetSheet1 targetSheet2", "0" });
            var mockedReader = new MockedReader(arguments);
            Engine engine = new Engine(mockedConsoleWriter.Object, mockedFileLogger.Object,
                          mockedReader, "test", mockedExcelManager.Object, "test.err", "test.info");
            engine.Startup();

            mockedExcelManager.Verify(x => x.ReplaceGivenColumnWithSourceOne("sourceSheetName", "targetSheet1", "OESCode"),
                                    Times.Once);

            mockedExcelManager.Verify(x => x.ReplaceGivenColumnWithSourceOne("sourceSheetName", "targetSheet2", "OESCode"),
                                   Times.Once);
        }

        [TestCase]
        public void Startup_ShouldCallAddOESCodeAndCatchException_WhenCalledWith5AndNOK()
        {
            var arguments = new Queue<string>(new string[] { "5", "sourceSheetName", "targetSheet1", "0" });
            var mockedReader = new MockedReader(arguments);

            mockedExcelManager.Setup(x => x.ReplaceGivenColumnWithSourceOne(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
              .Throws(new ArgumentException("There is not such a sheet"));

            Engine engine = new Engine(mockedConsoleWriter.Object, mockedFileLogger.Object,
                          mockedReader, "test", mockedExcelManager.Object, "test.err", "test.info");
            engine.Startup();

            mockedFileLogger.Verify(x => x.LogError("-------------------------- OESCode add: targetSheet1 --------------------------",
                                        "test.err"), Times.Once);

            mockedFileLogger.Verify(x => x.LogError("There is not such a sheet", "test.err"), Times.Once);
        }


        // ============================================================================
        [TestCase]
        public void Startup_ShouldCallAddAndAdaptPlacesAndWorksOK_WhenCalled_With6()
        {
            var arguments = new Queue<string>(new string[] { "6", "quantities", "interchangeables", "targetSheet1 targetSheet2", "0" });
            var mockedReader = new MockedReader(arguments);
            Engine engine = new Engine(mockedConsoleWriter.Object, mockedFileLogger.Object,
                          mockedReader, "test", mockedExcelManager.Object, "test.err", "test.info");
            engine.Startup();

            mockedExcelManager.Verify(x => x.AdaptProductsPlace("quantities", "targetSheet1", "interchangeables"),
                                    Times.Once);

            mockedExcelManager.Verify(x => x.AdaptProductsPlace("quantities", "targetSheet2", "interchangeables"),
                                   Times.Once);
        }

        [TestCase]
        public void Startup_ShouldCallAddAndAdaptPlacesAndCatchException_WhenCalledWith6AndNOK()
        {
            var arguments = new Queue<string>(new string[] { "6", "quantities", "interchangeables", "targetSheet1", "0" });
            var mockedReader = new MockedReader(arguments);

            mockedExcelManager.Setup(x => x.AdaptProductsPlace(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
              .Throws(new ArgumentException("There is not such a sheet"));

            Engine engine = new Engine(mockedConsoleWriter.Object, mockedFileLogger.Object,
                          mockedReader, "test", mockedExcelManager.Object, "test.err", "test.info");
            engine.Startup();

            mockedFileLogger.Verify(x => x.LogError("-------------------------- Adding Product Places: targetSheet1 --------------------------","test.err"));
            mockedFileLogger.Verify(x => x.LogError("There is not such a sheet", "test.err"), Times.Once);
        }
    }
}
