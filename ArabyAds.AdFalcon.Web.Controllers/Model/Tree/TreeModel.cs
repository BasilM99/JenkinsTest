using System.Collections.Generic;
using System.Linq;
using Noqoush.AdFalcon.Services.Interfaces.DTOs.Core;
using Noqoush.AdFalcon.Services.Interfaces.DTOs.Reports.QB;

namespace Noqoush.AdFalcon.Web.Controllers.Model.Tree
{
    public class TreeModel
    {
        public TreeAttribute attributes;
        public IList<TreeModel> children;
        public string data;

        private static TreeModel GetTreeNode(TreeDto treeDto, bool isSelected = false)
        {
            var children = GetTreeSubNodes(treeDto.Childs, isSelected);
            if (treeDto.Key == null)
                treeDto.Key = string.Empty;
            var returnvalue = new TreeModel
            {
                data = treeDto.Name.ToString(),
                attributes = new TreeAttribute { id = treeDto.Id.ToString(), selected = isSelected, Key = treeDto.Key, state = treeDto.state, style = treeDto.style },
                children = children,

            };


            return returnvalue;
        }
              
        private static IList<TreeModel> GetTreeSubNodes(IEnumerable<TreeDto> treeDtos, bool isSelected = false)
        {
            var returnList = new List<TreeModel>();
            if (treeDtos == null)
                return returnList;
            returnList.AddRange(treeDtos.Select(treeDto => GetTreeNode(treeDto, isSelected)));
            return returnList;
        }

        public static IList<TreeModel> GetTreeNodes(IEnumerable<TreeDto> treeDtos, bool isSelected = false)
        {
            var returnList = new List<TreeModel>();
            if (treeDtos == null)
                return returnList;
            returnList.AddRange(treeDtos.Select(treeDto => GetTreeNode(treeDto, isSelected)));
            foreach (var treeModel in returnList)
            {
                treeModel.attributes.isRoot = true;
            }
            return returnList;
        }


        private static TreeModel GetTreeQBNode(TreeQBDto treeDto, bool isSelected = false)
        {
            var children = GetTreeQBSubNodes(treeDto.Childs, isSelected);
            if (treeDto.Key == null)
                treeDto.Key = string.Empty;
            var returnvalue = new TreeModel
            {
                data = treeDto.data,
                attributes = new TreeAttribute { id = treeDto.Id.ToString(), selected = isSelected, Key = treeDto.Key, state = treeDto.state, style = treeDto.style },
                children = children,

            };


            return returnvalue;
        }
        private static IList<TreeModel> GetTreeQBSubNodes(IEnumerable<TreeQBDto> treeDtos, bool isSelected = false)
        {
            var returnList = new List<TreeModel>();
            if (treeDtos == null)
                return returnList;
            returnList.AddRange(treeDtos.Select(treeDto => GetTreeQBNode(treeDto, isSelected)));
            return returnList;
        }
        public static IList<TreeModel> GetTreeQBNodes(IEnumerable<TreeQBDto> treeDtos, bool isSelected = false)
        {
            var returnList = new List<TreeModel>();
            if (treeDtos == null)
                return returnList;
            returnList.AddRange(treeDtos.Select(treeDto => GetTreeQBNode(treeDto, isSelected)));
            foreach (var treeModel in returnList)
            {
                treeModel.attributes.isRoot = true;
            }
            return returnList;
        }
    }
}