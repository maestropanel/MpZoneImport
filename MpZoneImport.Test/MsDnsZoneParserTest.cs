using MpZoneImport;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.IO;

namespace MpZoneImport.Test
{
    
    
    /// <summary>
    ///This is a test class for MsDnsZoneParserTest and is intended
    ///to contain all MsDnsZoneParserTest Unit Tests
    ///</summary>
    [TestClass()]
    public class MsDnsZoneParserTest
    {


        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        // 
        //You can use the following additional attributes as you write your tests:
        //
        //Use ClassInitialize to run code before running the first test in the class
        //[ClassInitialize()]
        //public static void MyClassInitialize(TestContext testContext)
        //{
        //}
        //
        //Use ClassCleanup to run code after all tests in a class have run
        //[ClassCleanup()]
        //public static void MyClassCleanup()
        //{
        //}
        //
        //Use TestInitialize to run code before running each test
        //[TestInitialize()]
        //public void MyTestInitialize()
        //{
        //}
        //
        //Use TestCleanup to run code after each test has run
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        //
        #endregion


        /// <summary>
        ///A test for Start
        ///</summary>
        [TestMethod()]
        public void StartTest()
        {
            string zoneDirectory = @"C:\Users\oguzhan.O\Desktop\dns";
            MsDnsZoneParser target = new MsDnsZoneParser(zoneDirectory);            
            List<MsDnsZone> actual = target.Start();

            Assert.AreEqual(true, (actual.Count > 0));                        
        }

        [TestMethod()]
        public void SOAMatchTest()
        {
            string SOA_PATTERN = @"@\s+IN\s{2}SOA\s([a-z0-9\-\.]*).\s{2}([a-z0-9\-\.]*).\s\(\s*(\d+).+\s*(\d+).+\s*(\d+).+\s*(\d+).+\s*(\d+).+";
            string ZONE_FILE = @"C:\Users\oguzhan.O\Desktop\dns\akademni.net.dns";
            string zoneFileText = File.ReadAllText(ZONE_FILE);

            var match = Regex.Match(zoneFileText, SOA_PATTERN, RegexOptions.Multiline | RegexOptions.IgnoreCase);                        

            Assert.AreEqual(true, match.Success);

        }
    }
}
