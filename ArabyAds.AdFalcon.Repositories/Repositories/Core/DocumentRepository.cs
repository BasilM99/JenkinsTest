using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ArabyAds.Framework.Persistence;
using ArabyAds.AdFalcon.Domain.Model.Core;
using ArabyAds.AdFalcon.Domain.Repositories.Core;

namespace ArabyAds.AdFalcon.Persistence.Repositories.Core
{
    public class DocumentRepository : RepositoryBase<Document, int>, IDocumentRepository
    {
        public DocumentRepository(RepositoryImplBase<Document, int> repository)
            : base(repository)
        {

        }

    }
}
