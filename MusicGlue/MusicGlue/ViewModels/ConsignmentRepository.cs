using Microsoft.Data.SqlClient;
using MusicGlue.Models;
using System;
using System.Collections.Generic;
using System.Text;
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

                string query = "SELECT CONSIGNMENT.Id, CONSIGNMENT.CustomerCountry, CONSIGNMENT.ZipCode, CONSIGNMENT.Consignmentstatus, CONSIGNMENT.ReportingStatus, MUSICPRODUCT.Id, MUSICPRODUCT.Price, MUSICPRODUCT.ProductDescriptionId " +
                    "FROM CONSIGNMENT, MUSICPRODUCT_CONSIGNMENT, MUSICPRODUCT " +
                    "WHERE Consignment.Id = MUSICPRODUCT_CONSIGNMENT.ConsignmentId AND MUSICPRODUCT.Id = MUSICPRODUCT_CONSIGNMENT.ProductId";

                SqlCommand cmd = new SqlCommand(query, con);
                using (SqlDataReader dr = cmd.ExecuteReader())
                {

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
