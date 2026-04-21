using Microsoft.Data.SqlClient;
using MusicGlue.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.CodeDom.Compiler;

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
                    int consignmentIdChecker = -1;

                    //temp consignment attributes
                    int tempId = 0;
                    string tempCustomerCountry = "";
                    string tempZipCode = "";
                    ConsignmentStatus tempConsignmentStatus = ConsignmentStatus.NotDispatched;
                    bool tempReportingStatus = false;
                    List<MusicProduct> tempMusicProducts = new List<MusicProduct>();

                    while (dr.Read())
                    {
                        //add musicProducts to consignment
                        if (consignmentIdChecker == dr.GetInt32(0))
                        {
                            MusicProduct musicproduct = new MusicProduct
                            {
                                ProductId = dr.GetInt32(0),
                                Price = (double)dr["MUSICPRODUCT.Price"],
                                ProductDescriptionId = (int)dr["MUSICPRODUCT.ProductDescriptionId"]
                            };
                            tempMusicProducts.Add(musicproduct);
                        }

                        //new consignment begins
                        if (consignmentIdChecker != dr.GetInt32(0))
                        {
                            // add consignment to consignments list
                            if (consignmentIdChecker != -1)
                            {
                                Consignment consignment = new Consignment
                                {
                                    Id = tempId,
                                    CustomerCountry = tempCustomerCountry,
                                    ZipCode = tempZipCode,
                                    ConsignmentStatus = tempConsignmentStatus,
                                    ReportingStatus = tempReportingStatus,
                                    MusicProducts = tempMusicProducts
                                };
                                consignments.Add(consignment);
                            }

                            //add new consignment attributes
                            tempId = dr.GetInt32(0);
                            tempCustomerCountry = (string)dr["CONSIGNMENT.CustomerCountry"];
                            tempZipCode = (string)dr["CONSIGNMENT.ZipCode"];
                            tempConsignmentStatus = (ConsignmentStatus)dr["CONSIGNMENT.Consignmentstatus"];
                            tempReportingStatus = (bool)dr["CONSIGNMENT.ReportingStatus"];
                            tempMusicProducts.Clear();

                            //update consingmentIdChecker
                            consignmentIdChecker = dr.GetInt32(0);
                        }
                    }


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
