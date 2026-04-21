using Microsoft.Data.SqlClient;
using MusicGlue.Models;

namespace MusicGlue.ViewModels
{
    public class ReportingOrganisationRepository
    {
        private readonly string ConnectionString;
        private List<ReportingOrganisation> reportingOrganisations;

        public ReportingOrganisationRepository()
        {

            reportingOrganisations = new List<ReportingOrganisation>();

            ConnectionString = Configuration.ConnectionString;

            InitializeRepository();
        }

        private void InitializeRepository()
        {
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("SELECT Id, Name, Country FROM REPORTINGORGANISATION", con);
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        ReportingOrganisation reportingOrganisation = new ReportingOrganisation
                        {
                            Id = dr.GetInt32(0),
                            Name = (string)dr["Name"],
                            Country = (string)dr["Country"]
                        };
                        reportingOrganisations.Add(reportingOrganisation);
                    }
                }
            }

        }

        public List<ReportingOrganisation> GetAll()
        {
            return reportingOrganisations;
        }

        public ReportingOrganisation Get(int id)
        {
            return reportingOrganisations.Find(reportingOrg => reportingOrg.Id == id);
        }
    }
}
