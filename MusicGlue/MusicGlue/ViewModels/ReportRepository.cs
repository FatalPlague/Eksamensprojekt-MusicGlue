using Microsoft.Data.SqlClient;
using MusicGlue.Models;
using MusicGlue.Models.Formatters;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace MusicGlue.ViewModels
{
    public class ReportRepository
    {
        private readonly string connectionString;
        private List<Report> reports;

        public ReportRepository()
        {
            reports = new List<Report>();
            connectionString = Configuration.ConnectionString;
            InitializeRepository();
        }

        public void Create(Report reportToBeCreated)
        {
            reports.Add(reportToBeCreated);
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("INSERT INTO Report (Id, FileName, ReportingDate, TotalSales, Reportstatus, ReportingOrganisationId) " +
                     "VALUES(@Id, @FileName, @ReportingDate, @TotalSales, @ReportStatus, @ReportingOrganisationId)" + "SELECT @@IDENTITY", con))
                {
                    cmd.Parameters.Add("@Id", SqlDbType.Int).Value = reportToBeCreated.Id;
                    cmd.Parameters.Add("@FileName", SqlDbType.NVarChar).Value = reportToBeCreated.FileName;
                    cmd.Parameters.Add("@ReportingDate", SqlDbType.DateTime).Value = reportToBeCreated.ReportingDate;
                    cmd.Parameters.Add("@TotalSales", SqlDbType.Int).Value = reportToBeCreated.TotalSales;
                    cmd.Parameters.Add("@ReportStatus", SqlDbType.Int).Value = reportToBeCreated.ReportStatus;
                    cmd.Parameters.Add("@ReportingOrganisationId", SqlDbType.Int).Value = reportToBeCreated.ReportingOrganisationId;
                }
            }
        }

        private void InitializeRepository()
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("SELECT Id, FileName, ReportingDate, TotalSales, ReportStatus, ReportingOrganisationId FROM REPORT", con);
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        string fileName = (string)dr["FileName"];
                        Report report = new Report
                        {
                            Id = dr.GetInt32(0),
                            FileName = fileName,
                            ReportingDate = (DateTime)dr["ReportingDate"],
                            TotalSales = (int)dr["TotalSales"],
                            ReportStatus = (ReportStatus)dr["ReportStatus"],
                            ReportingOrganisationId = (int)dr["ReportingOrganisationId"]
                        };
                        reports.Add(report);
                    }
                }
            }

        }
        public void Update(Report reportToBeUpdated)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();

                string query = "UPDATE REPORT " +
                    "SET ReportStatus = @ReportStatus " +
                    "WHERE Id = @Id";

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.Add("@ReportStatus", SqlDbType.Int).Value = reportToBeUpdated.ReportStatus;
                    cmd.Parameters.Add("@Id", SqlDbType.Int).Value = reportToBeUpdated.Id;

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public List<Report> GetAll()
        {
            return reports;
        }

        public Report GetById(int id)
        {
            return reports.Find(r => r.Id == id);
        }
    }
}
