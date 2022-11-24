using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

//namespace ArabyAds.AdFalcon.Base
//
    public interface IAuditable<T>
    {
        T ID { get; }
    }
    public interface IEntity<T> : IAuditable<T>, IEntityDescriber
    {
        bool IsDeleted { get; set; }
    }

    public interface IComplexEntity
    {


    }

    public interface IEntityDescriber
    {
        string GetDescription();

    }

    public abstract class CriteriaWebBase<TEntity>
    {
        public abstract Expression<Func<TEntity, bool>> GetExpression();
        public abstract Func<TEntity, bool> GetWhere();

    }

    public class PropertyDescriptorValueAttribute : Attribute
    {
        public string PropertyName;

        public PropertyDescriptorValueAttribute(string PropertyNameVar)
        {
            PropertyName = PropertyNameVar;

        }
    }
//}
