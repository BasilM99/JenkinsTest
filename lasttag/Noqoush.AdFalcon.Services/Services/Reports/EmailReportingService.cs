using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Noqoush.AdFalcon.AdServer.Integration.Services.Interfaces;
using Noqoush.AdFalcon.Persistence.Repositories.Reports;

namespace Noqoush.AdFalcon.Services.Services.Reports
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
