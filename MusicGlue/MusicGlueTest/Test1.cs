using MusicGlue;
using MusicGlue.Models;
using MusicGlue.Models.Formatters;
using MusicGlue.ViewModels;

namespace MusicGlueTest
{
    [TestClass]
    public sealed class Test1
    {
        // Arrange
        string date = DateTime.Now.ToString("yyMMdd"); 
        List<Consignment> consignments = new List<Consignment>();
        IFormatter formatter;
        ReportRepository reportRepository;

        [TestInitialize]
        public void init()
        {
            ProductDescription d1 = new ProductDescription()
            {
                Id = 1,
                Barcode = "5321900114742",
            };

            ProductDescription d2 = new ProductDescription()
            {
                Id = 1,
                Barcode = "5321900983034",
            };

            MusicProduct m1 = new MusicProduct()
            {
                Id = 1,
                Price = 5.99,
                Description = d1
            };


            MusicProduct m2 = new MusicProduct()
            {
                Id = 2,
                Price = 4.99,
                Description = d1
            };

            MusicProduct m3 = new MusicProduct()
            {
                Id = 3,
                Price = 12.99,
                Description = d2
            };

            Consignment c1 = new Consignment()
            {
                Id = 1,
                ZipCode = "WC",
                CustomerCountry = "England",
                ConsignmentStatus = ConsignmentStatus.Dispatched,
                ReportingStatus = ConsignmentReportingStatus.NotReported,
                MusicProducts = new List<MusicProduct>() { m1, m1, m1, m2, m2, m3, m3 }
            };

            Consignment c2 = new Consignment()
            {
                Id = 2,
                ZipCode = "WC",
                CustomerCountry = "England",
                ConsignmentStatus = ConsignmentStatus.Dispatched,
                ReportingStatus = ConsignmentReportingStatus.NotReported,
                MusicProducts = new List<MusicProduct>() { m3, m3, m3, m3, m3, m3, m3, m3, m3, m3, m3, m3, m3, m3, m3, m3, m3, m3, m3, m3, m2, m1, m1, m1 }
            };

            Consignment c3 = new Consignment()
            {
                Id = 3,
                ZipCode = "L",
                CustomerCountry = "England",
                ConsignmentStatus = ConsignmentStatus.Dispatched,
                ReportingStatus = ConsignmentReportingStatus.NotReported,
                MusicProducts = new List<MusicProduct>() { m1, m1, m3 }
            };

            Consignment c4 = new Consignment()
            {
                Id = 4,
                ZipCode = "WC",
                CustomerCountry = "England",
                ConsignmentStatus = ConsignmentStatus.Dispatched,
                ReportingStatus = ConsignmentReportingStatus.NotReported,
                MusicProducts = new List<MusicProduct>() { m3 }
            };

            consignments.Add(c1);
            consignments.Add(c2);
            consignments.Add(c3);
            consignments.Add(c4);
            formatter = new OCCFormatter();

        }

        [TestMethod]
        public void IsOCCFormatCorrect()
        {
            //Act
            string actual = formatter.Format(consignments);
            string expected = "0WC   " + date + Environment.NewLine +
                "15321900114742000006 00599" + Environment.NewLine +
                "15321900114742000003 00499" + Environment.NewLine +
                "15321900983034000023 01299" + Environment.NewLine +
                "9WC   00003" + Environment.NewLine +
                Environment.NewLine +
                "0L    " + date + Environment.NewLine +
                "15321900114742000002 00599" + Environment.NewLine +
                "15321900983034000001 01299" + Environment.NewLine +
                "9L    00002";

            Console.Write(Environment.NewLine);
            Console.WriteLine("Expected:");
            Console.WriteLine(expected);
            Console.Write(Environment.NewLine);
            Console.WriteLine("-------------------");
            Console.Write(Environment.NewLine);
            Console.WriteLine("Actual:");
            Console.WriteLine(actual);

            //Assert
            Assert.AreEqual(
                expected,
                actual
            );
        }

        [TestMethod]
        public void CheckForPreexistingFile()
        {
            //Act
            string consignmentToReport = formatter.Format(consignments);
            string fileName = "MusicGlue_new_platform" + DateTime.Now.ToString("yyMMdd") + ".txt";
            
            if(ReportHandler.CheckReportHasBeenSent(fileName))
                File.Delete(fileName);

            ReportHandler.SaveSendReport(consignmentToReport, fileName);

            //Assert
            Assert.IsTrue(ReportHandler.CheckReportHasBeenSent(fileName));
        }

        [TestMethod]
        public void CheckingIfRepositoryWorks()
        {
            //Act
            reportRepository = new ReportRepository();
            List<Report> reports = reportRepository.GetAll();

            //Assert
            Assert.AreEqual("MusicGlue_new_platform260414.txt", reports[0].FileName);
        }
    }
}
