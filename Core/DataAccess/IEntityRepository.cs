using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Core.DataAccess

{    //generic constraint getiriyoruz buraya. Entities/Concrete in altındaki nesneler veri tipi seçilebilsin diye.
    //class: Referans tip olabilir demek.
    // IEntity: IEntity olabilir veya IEntity yi implemente eden bir class olabilir.
    // new(): new lenebilir olmalı.

    public interface IEntityRepository <T> where T:class,IEntity,new()
    {   // Expression GetAll metodundalinq ile özelleştirebilmek için gerekli. burada filtre=null demek filtre vermeyebilirsin demek.
        List<T> GetAll (Expression<Func<T,bool>> filter=null);
        T Get(Expression<Func<T, bool>> filter);
        void Add(T entity);
        void Update(T entity);
        void Delete(T entity);
        
    }
}
