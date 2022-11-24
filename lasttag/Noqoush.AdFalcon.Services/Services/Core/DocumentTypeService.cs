using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Noqoush.AdFalcon.Services.Interfaces.Services.Core;
using Noqoush.AdFalcon.Domain.Repositories.Core;
using Noqoush.AdFalcon.Services.Interfaces.DTOs.Core;
using Noqoush.AdFalcon.Services.Mapping;
using System.Linq.Expressions;
using Noqoush.AdFalcon.Domain.Model.Core;

namespace Noqoush.AdFalcon.Services.Services.Core
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

        public int GetMIMEById(int Id)
        {
          


            var documentType = _documentTypeRepository.Query(p => p.ID == Id).SingleOrDefault();
           return documentType.MIMETypes.First().ID;

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
