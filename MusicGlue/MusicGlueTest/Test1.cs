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
        Consignment c1;
        MusicProduct m1;
        MusicProduct m2;
        ProductDescription d1;
        ProductDescription d2;
        IFormatter f;

        [TestInitialize]
        public void init()
        {
            // Act
            d1 = new ProductDescription()
            {
                Id = 1,
                Barcode = "521900114742",
            };

            d2 = new ProductDescription()
            {
                Id = 1,
                Barcode = "856200115725",
            };

            m1 = new MusicProduct()
            {
                Id = 1,
                Price = 1.56,
                Description = d1
            };

            m2 = new MusicProduct()
            {
                Id = 2,
                Price = 13.60,
                Description = d2
            };

            c1 = new Consignment()
            {
                Id = 1,
                ZipCode = "WC",
                CustomerCountry = "England",
                ConsignmentStatus = ConsignmentStatus.Dispatched,
                ReportingStatus = false,
                MusicProducts = new List<MusicProduct>() {m1, m1, m2, m2, m2, m2 ,m2 ,m2 ,m2 ,m2 ,m2 ,m2 }
            };

            f = new OCCFormatter();
            consignments.Add(c1);
            


        }

        [TestMethod]
        public void OCCFormatterTest()
        {
            //Assert
            Assert.AreEqual(
                "0WC  260423\n" +
                "1521900114742000002 00156\n" +
                "1856200115725000010 01360\n" +
                "9WC  00002"
                ,
                f.Format(consignments)
                );
        }
    }
}
