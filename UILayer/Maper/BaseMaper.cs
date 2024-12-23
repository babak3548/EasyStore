using System;

using System.ComponentModel;
using System.Xml.Serialization;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;

using DataLayer;
using System.Collections.Generic;
using ServiceLayer;
using System.Linq;
using DataLayer.Contract;
using Microsoft.AspNetCore.Http;

namespace UILayer.Maper
{
    public interface IBaseMaper<T, K>
        where T : class
        where K : IContract
    {
        IEnumerable<K> EntityToContract(IEnumerable<T> iEnumerableAccessContract);
        K EntityToContract(T Entity);//دستی
        K EntityToContract(T Entity, K Contract);
        T FormCollectionToEntity(FormCollection formCollection);
        T FormCollectionToEntity(FormCollection formCollection, T Entity);
    }

    public abstract class BaseMaper<T, K> : IBaseMaper<T, K>
        where T : class
        where K : IContract
    {
        public IEnumerable<K> EntityToContract(IEnumerable<T> iEnumerableAccessContract)
        {
            foreach (var entity in iEnumerableAccessContract)
            {
                yield return this.EntityToContract(entity);
            }
        }

        public abstract void EntityToEntity(T srcEntity,T desEntity);

        public abstract K EntityToContract(T Entity);

        public abstract K EntityToContract(T Entity, K Contract);

        public abstract T FormCollectionToEntity(FormCollection formCollection);

        public abstract T FormCollectionToEntity(FormCollection formCollection, T Entity);

    }
}