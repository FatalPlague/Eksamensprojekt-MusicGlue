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
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("INSERT INTO Report (FileName, ReportingDate, TotalSales, Reportstatus, ReportingOrganisationId) " +
                     "VALUES(@FileName, @ReportingDate, @TotalSales, @ReportStatus, @ReportingOrganisationId)" + "SELECT @@IDENTITY", con))
                {
                    cmd.Parameters.Add("@FileName", SqlDbType.NVarChar).Value = reportToBeCreated.FileName;
                    cmd.Parameters.Add("@ReportingDate", SqlDbType.DateTime).Value = reportToBeCreated.ReportingDate;
                    cmd.Parameters.Add("@TotalSales", SqlDbType.Int).Value = reportToBeCreated.TotalSales;
                    cmd.Parameters.Add("@ReportStatus", SqlDbType.Int).Value = reportToBeCreated.ReportStatus;
                    cmd.Parameters.Add("@ReportingOrganisationId", SqlDbType.Int).Value = reportToBeCreated.ReportingOrganisationId;
                    reportToBeCreated.Id = Convert.ToInt32(cmd.ExecuteScalar());

                    reports.Add(reportToBeCreated);
                }
            }
        }

        private void InitializeRepository()
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("exec spSelectReportsJoinConsignmentIds", con);
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    Dictionary<int, Report> reportDictionary = new Dictionary<int, Report>();

                    while (dr.Read())
                    {
                        int reportId = (int)dr["ReportId"];
                        if (!reportDictionary.TryGetValue(reportId, out Report? report))
                        {
                            report = new Report
                            {
                                Id = reportId,
                                FileName = (string)dr["FileName"],
                                ReportingDate = (DateTime)dr["ReportingDate"],
                                TotalSales = (int)dr["TotalSales"],
                                ReportStatus = (ReportStatus)dr["ReportStatus"],
                                ReportingOrganisationId = (int)dr["ReportingOrganisationId"],
                                ConsignmentIds = new List<int>()
                            };

                            reportDictionary.Add(reportId, report);
                        }

                        report.ConsignmentIds.Add((int)dr["ConsignmentId"]);



                        //string fileName = (string)dr["FileName"];
                        //Report report = new Report
                        //{
                        //    Id = dr.GetInt32(0),
                        //    FileName = fileName,
                        //    ReportingDate = (DateTime)dr["ReportingDate"],
                        //    TotalSales = (int)dr["TotalSales"],
                        //    ReportStatus = (ReportStatus)dr["ReportStatus"],
                        //    ReportingOrganisationId = (int)dr["ReportingOrganisationId"]
                        //};
                        //reports.Add(report);
                    }
                    reports = reportDictionary.Values.ToList();
                }
            }

        }
        public void Update(Report reportToBeUpdated)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();

                string query = "UPDATE REPORT " +
                    "SET ReportStatus = @ReportStatus, " +
                    "FileName = @FileName " + 
                    "WHERE Id = @Id";

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.Add("@ReportStatus", SqlDbType.Int).Value = reportToBeUpdated.ReportStatus;
                    cmd.Parameters.Add("@FileName", SqlDbType.NVarChar).Value = reportToBeUpdated.FileName;
                    cmd.Parameters.Add("@Id", SqlDbType.Int).Value = reportToBeUpdated.Id;

                    cmd.ExecuteNonQuery();

                    Report report = reports.Find(report => report.Id == reportToBeUpdated.Id);
                    report.ReportStatus = reportToBeUpdated.ReportStatus;
                    report.FileName = reportToBeUpdated.FileName;
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
