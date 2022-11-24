using System;
using System.Collections.Generic;
using System.Linq;
using ArabyAds.AdFalcon.Domain.Model.Core;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Core;
using ArabyAds.AdFalcon.Services.Interfaces.Services.Campaign;
using ArabyAds.AdFalcon.Services.Mapping;
using ArabyAds.AdFalcon.Domain.Repositories.Core;
using System.Text;
using System.Configuration;
using ArabyAds.Framework.Utilities;
using ArabyAds.Framework;

namespace ArabyAds.AdFalcon.Services.Services.Core
{
    public class DocumentService : IDocumentService
    {
        private readonly IDocumentRepository _documentRepository = null;
        private readonly IDocumentTypeRepository _documentTypeRepository = null;
        private static Id64Generator _id64Generator = new Id64Generator(int.Parse(JsonConfigurationManager.AppSettings["HostId"]));
        public DocumentService(IDocumentRepository documentRepository, IDocumentTypeRepository documentTypeRepository)
        {
            _documentRepository = documentRepository;
            _documentTypeRepository = documentTypeRepository;
        }
        public IEnumerable<DocumentDto> QueryByCratiria(DocumentCriteria criteria)
        {
            var list = _documentRepository.Query(criteria.GetExpression());
            return list.Select(document => MapperHelper.Map<DocumentDto>(document)).ToList();
        }
        public ValueMessageWrapper<bool> Delete(IEnumerable<int> documentIds)
        {
            foreach (var item in documentIds.Select(documentId => _documentRepository.Get(documentId)))
            {
                item.Delete();
                _documentRepository.Save(item);
            }
            return  ValueMessageWrapper.Create(true);
        }
        public ValueMessageWrapper<int> Save(DocumentDto document)
        {
            //var item = _documentRepository.Get(document.ID);
            //if (item != null)
            //{
            //    item.Name = document.Name;
            //    item.Content = document.Content;
            //    item.Extension = document.Extension;
            //    item.Size = document.Size;
            //    item.Extension = document.Extension;
            //    item.Extension = document.Extension;
            //    item.Extension = document.Extension;

            //}
            //else
            //{
            document.IsWebHDFS = true;
            var item = MapperHelper.Map<Document>(document);
            item.UploadedDate = Framework.Utilities.Environment.GetServerTime();
            item.DocumentType = _documentTypeRepository.Get(document.DocumentTypeId);
         
            if (document.IsWebHDFS)
            {
                item.Name = item.StructureTheName(item.GetNameWithNoExtension()+ item.Extension);
            }
            item.WriteContent(item.Content);
            // }
          
            _documentRepository.Save(item);
            return ValueMessageWrapper.Create(item.ID);
        }
        public ValueMessageWrapper<int> SaveFromInputPath(DocumentDto document)
        {
           
            var item = MapperHelper.Map<Document>(document);
            item.UploadedDate = Framework.Utilities.Environment.GetServerTime();
            item.DocumentType = _documentTypeRepository.Get(document.DocumentTypeId);
            item.IsWebHDFS = true;
            var subFolder = Framework.Utilities.Environment.GetServerTime().ToString("yyyyMMdd");

           string name=  item.GetNameWithNoExtension() + item.Extension;
            item.Name = item.StructureTheName(name);
            item.WriteContent( System.IO.File.ReadAllBytes(document.InputPath));
            _documentRepository.Save(item);
            return  ValueMessageWrapper.Create(item.ID);
        }
        public DocumentDto Get(ValueMessageWrapper<int> documentId)
        {
            var item = _documentRepository.Get(documentId.Value);
   
            var dto= item != null ? MapperHelper.Map<DocumentDto>(item) : null;
          if(dto!=null)
            dto.Content = item.ReadContent();

            return dto;
        }

    }
}
