using Microsoft.Data.SqlClient;
using MusicGlue.Models;
using System;
using System.Collections.Generic;
using System.Text;

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

                string query = "";
            }
        }
    }
}
