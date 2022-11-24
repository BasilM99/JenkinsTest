using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ArabyAds.AdFalcon.AdServer.Integration.Services.Interfaces;
using ArabyAds.AdFalcon.Persistence.Repositories.Reports;

namespace ArabyAds.AdFalcon.Services.Services.Reports
{
    public class EmailReportingService : IEmailReportingService
    {
        private readonly IReportRepository _reportRepository;
        public EmailReportingService(IReportRepository reportRepository)
        {
            _reportRepository = reportRepository;
        }
        public List<AppEmailReportDto> GetAppEmailReport(EmailReportCriteriaDto criteriaDto)
        {
            return _reportRepository.GetAppEmailReport(criteriaDto);
        }
    }
}
