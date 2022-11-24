using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Noqoush.AdFalcon.Services.Interfaces.DTOs.Core
{
    public class PlatfromTree
    {
        public int Id { set; get; }
        public List<ManuTree> Manu { set; get; }
        public bool IsAll { set; get; }

    }


    public class ManuTree
    {
        public int Id { set; get; }
        public List<int> Devices { set; get; }
        public bool IsAll { set; get; }
    }

}
