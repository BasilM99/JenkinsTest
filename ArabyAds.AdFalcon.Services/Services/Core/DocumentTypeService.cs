using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ArabyAds.AdFalcon.Services.Interfaces.Services.Core;
using ArabyAds.AdFalcon.Domain.Repositories.Core;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Core;
using ArabyAds.AdFalcon.Services.Mapping;
using System.Linq.Expressions;
using ArabyAds.AdFalcon.Domain.Model.Core;
using ArabyAds.Framework;

namespace ArabyAds.AdFalcon.Services.Services.Core
{
    public class DocumentTypeService : IDocumentTypeService
    {
        private readonly IDocumentTypeRepository _documentTypeRepository = null;
        public DocumentTypeService(IDocumentTypeRepository documentTypeRepository)
        {
            _documentTypeRepository = documentTypeRepository;
        }

        public IEnumerable<DocumentTypeDto> GetAll()
        {
            var list = _documentTypeRepository.GetAll();
            return list.Select(operatorDto => MapperHelper.Map<DocumentTypeDto>(operatorDto)).ToList();
        }

        public ValueMessageWrapper<int> GetMIMEById(ValueMessageWrapper<int> Id)
        {
          


            var documentType = _documentTypeRepository.Query(p => p.ID == Id.Value).SingleOrDefault();
           return ValueMessageWrapper.Create(documentType.MIMETypes.First().ID);

        }
        public DocumentTypeDto GetByCode(string code)
        {
            if (string.IsNullOrEmpty(code))
                throw new ArgumentNullException();


            var documentType = _documentTypeRepository.Query(p => p.Code == code).SingleOrDefault();
            return MapperHelper.Map<DocumentTypeDto>(documentType);
        }
    }
}
