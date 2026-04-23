using MusicGlue;
using MusicGlue.Models;
using MusicGlue.Models.Formatters;

namespace MusicGlueTest
{
    [TestClass]
    public sealed class Test1
    {
        // Arrange
        List<Consignment> consignments;
        IFormatter formatter;

        [TestInitialize]
        public void init()
        {
            // Act
            ProductDescription d1 = new ProductDescription()
            {
                Id = 1,
                Barcode = "521900114742",
            };

            ProductDescription d2 = new ProductDescription()
            {
                Id = 1,
                Barcode = "856200115725",
            };

            MusicProduct m1 = new MusicProduct()
            {
                Id = 1,
                Price = 1.56,
                Description = d1
            };

            MusicProduct m2 = new MusicProduct()
            {
                Id = 2,
                Price = 13.60,
                Description = d2
            };

            Consignment c1 = new Consignment()
            {
                Id = 1,
                ZipCode = "WC",
                CustomerCountry = "England",
                ConsignmentStatus = ConsignmentStatus.Dispatched,
                ReportingStatus = false,
                MusicProducts = new List<MusicProduct>() {m1, m1, m2, m2, m2, m2 ,m2 ,m2 ,m2 ,m2 ,m2 ,m2 }
            };

            consignments.Add(c1);
            formatter = new OCCFormatter();
        }

        [TestMethod]
        public void OCCFormatterTest()
        {
            //Assert
            Assert.AreEqual(
                "0WC  260423" + Environment.NewLine +
                "1521900114742000002 00156" + Environment.NewLine +
                "1856200115725000010 01360" + Environment.NewLine +
                "9WC  00002",

                formatter.Format(consignments)
            );
        }
    }
}
