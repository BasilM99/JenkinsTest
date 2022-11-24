using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Noqoush.AdFalcon.Services.Interfaces.DTOs.Account
{
    public class TrialResultDto
    {
        public IEnumerable<TrialDto> Items { get; set; }
        public long TotalCount { get; set; }

    }
}
