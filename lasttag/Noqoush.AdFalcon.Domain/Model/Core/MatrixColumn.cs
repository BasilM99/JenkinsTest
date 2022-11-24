using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Noqoush.AdFalcon.Domain.Model.Core
{
    public class metriceColumn
    {
        public virtual bool Hide { get; set; }
        public virtual int Id { get; set; }
        public virtual int Order { get; set; }
        public virtual bool Advertiser { get; set; }
        public virtual bool Publisher { get; set; }
        public virtual bool DSP { get; set; }
        public virtual bool IsSelected { get; set; }
        public virtual string HeaderResourceKey { get; set; }
        public virtual string HeaderResourceSet { get; set; }
        public virtual string GroupKey { get; set; }
        public virtual string DataBaseFieldName { get; set; }
        public virtual string AppFieldName { get; set; }
        public virtual string Format { get; set; }

    }
}
