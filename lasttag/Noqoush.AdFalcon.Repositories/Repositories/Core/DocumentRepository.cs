using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Noqoush.Framework.Persistence;
using Noqoush.AdFalcon.Domain.Model.Core;
using Noqoush.AdFalcon.Domain.Repositories.Core;
using System.Xml.Linq;
using System.Net;
using System.Xml.Schema;
using System.IO;
using System.Xml;
using System.Web.Hosting;

namespace Noqoush.AdFalcon.Persistence.Repositories.Core
{
    public class DocumentRepository : RepositoryBase<Document, int>, IDocumentRepository
    {
        public DocumentRepository(RepositoryImplBase<Document, int> repository)
            : base(repository)
        {

        }

    }
}
