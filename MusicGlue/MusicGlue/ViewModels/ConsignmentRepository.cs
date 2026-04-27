using Microsoft.Data.SqlClient;
using MusicGlue.Models;
using System.Data;

namespace MusicGlue.ViewModels
{
    public class ConsignmentRepository
    {
        private readonly string connectionString;
        private List<Consignment> consignments;

        public ConsignmentRepository()
        {
            connectionString = Configuration.ConnectionString;

            consignments = new List<Consignment>();
            InitializeRepository();
        }

        private void InitializeRepository()
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();

                string query = "EXEC spSelectConsignmentsJoinMusicProductsAndProudctDescription";

                SqlCommand cmd = new SqlCommand(query, con);
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    Dictionary<int, Consignment> consignmentDictionary = new Dictionary<int, Consignment>();

                    while (dr.Read())
                    {
                        int consignmentId = (int)dr["ConsignmentId"];
                        if (!consignmentDictionary.TryGetValue(consignmentId, out Consignment? consignment))
                        {
                            consignment = new Consignment
                            {
                                Id = consignmentId,
                                CustomerCountry = (string)dr["CustomerCountry"],
                                ZipCode = (string)dr["ZipCode"],
                                ConsignmentStatus = (ConsignmentStatus)dr["ConsignmentStatus"],
                                ReportingStatus = (bool)dr["ReportingStatus"],
                                MusicProducts = new List<MusicProduct>()
                            };

                            consignmentDictionary.Add(consignmentId, consignment);
                        }

                        ProductDescription productDescription = new ProductDescription
                        {
                            Id = (int)dr["ProductDescriptionId"],
                            APN = (string)dr["APN"],
                            CatalogNumber = (string)dr["CatalogNumber"],
                            SKU = (string)dr["SKU"],
                            Barcode = (string)dr["Barcode"]
                        };

                        MusicProduct musicProduct = new MusicProduct
                        {
                            Id = (int)dr["MusicProductId"],
                            Price = (double)dr["Price"],
                            Description = productDescription
                        };

                        consignment.MusicProducts.Add(musicProduct);
                    }

                    consignments = consignmentDictionary.Values.ToList();
                }
            }
        }

        public void Update (Consignment consignmentToBeUpdated)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();

                string query = "UPDATE CONSIGNMENT " +
                    "SET CONSIGNMENT.ReportingStatus = @CONSIGNMENT.ReportingStatus " +
                    "WHERE Id = @Id";

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.Add("@CONSIGNMENT.ReportingStatus", SqlDbType.Bit).Value = consignmentToBeUpdated.ReportingStatus;
                    cmd.Parameters.Add("@Id", SqlDbType.Int).Value = consignmentToBeUpdated.Id;

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public List<Consignment> GetByCustomerCountry(string country)
        {
            return consignments.FindAll(o => o.CustomerCountry == country);
        }
    }
}
