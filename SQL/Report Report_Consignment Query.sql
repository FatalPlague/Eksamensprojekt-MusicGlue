SELECT 
	REPORT.Id AS ReportId, 
	REPORT.FileName, 
	REPORT.ReportingDate, 
	REPORT.TotalSales, 
	REPORT.ReportStatus, 
	REPORT.ReportingOrganisationId, 
	REPORT_CONSIGNMENT.ConsignmentId
FROM 
	REPORT, 
	REPORT_CONSIGNMENT
WHERE 
	REPORT.Id = REPORT_CONSIGNMENT.ReportId