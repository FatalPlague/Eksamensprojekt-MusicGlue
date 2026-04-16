using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Runtime.ConstrainedExecution;
using System.Text;

namespace MusicGlue.ViewModels
{
    public class ReportingOrganisationRepository
    {
        private readonly string ConnectionString;
        private List<ReportingOrganisation> reportingOrganisations;

        public ReportingOrganisationRepository()
        {
            IConfigurationRoot config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();

            cars = new List<Car>();

            ConnectionString = config.GetConnectionString("MyDBConnection");
        }

        // Relevante CRUD-metoder indsættes her

    }
}
