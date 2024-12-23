using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using DataLayer;
using DataLayer.Enums;
using Utility;

using DataLayer.EF;
using Microsoft.EntityFrameworkCore;
using System.Xml.Linq;
using DataLayer.EFLog;

namespace ServiceLayer
{



    //to do merge with orginal BaseService
    public class BaseServiceLog<TEntity> :  IRepository<TEntity> where TEntity : class
    {
        public AppSetting AppSetting { get; set; }
        public string message { get; set; }
        string _errorMessage;
        TEntity _entity;
        protected EasyStoreLog _EasyStoreLog; 
        public BaseServiceLog(EasyStoreLog EasyStoreLog)
            : base()
        {
            _EasyStoreLog = EasyStoreLog;
            AppSetting = new AppSetting();
        }

        private void ResetContextState() => _EasyStoreLog.ChangeTracker.Entries()
    .Where(e => e.Entity != null).ToList()
    .ForEach(e => e.State = EntityState.Detached);






        ///برای ولیدیت کردن صاحب انتیتی مورد استفاده قرار می گیرد
        #region ValidationOwnerEntity
        /// <summary>
        ///  تمام انتیتی ها که بخواهند از آن استفاده کننند باید آن را پیاده سازی کنند
        /// این متد کخه کاربر جاری مالک این انتتی با ای دی زیر هست یا نه
        /// </summary>
        /// <param name="Id"></param>
        public virtual bool CheckOwnerEntity(int Id, int FK_User, out TEntity entity)
        {
            throw new NotImplementedException();
        }

      /// <summary>
      ///  تمام انتیتی ها که بخواهند از آن استفاده کننند باید آن را پیاده سازی کنند
      /// این متد کخه کاربر جاری مالک این انتتی با ای دی زیر هست یا نه
      /// </summary>
      /// <param name="Id"></param>
      public virtual bool CheckOwnerEntity(int Id, int FK_User)
      {
          throw new NotImplementedException();
      }

      /// <summary>
      /// تمام انتیتی ها که بخواهند از آن استفاده کننند باید آن را پیاده سازی کنند
      /// این متد کخه کاربر جاری مالک این انتتی هست یا نه
      /// </summary>
      /// <param name="Id"></param>
      public virtual bool CheckOwnerEntity(TEntity entity, int FK_User)
      {
        throw new NotImplementedException();
      }

/// <summary>
/// این متد ذخیره یا ریجت تغییرات را انجام می دهد
///این متد تا حد ممکن در کلاس های کنترولر استفاده گردد 
/// </summary>
/// <param name="accept"></param>
      public void SaveAllChengeOrAllReject(bool accept)
      {
          if (accept)
          {
                    _EasyStoreLog.SaveChanges();
          }
          else
          {
                ResetContextState();
          }
      }

        public object GetByFilterAndContians(Dictionary<string, object> filterDic)
        {
           // _OnlineShopping.Query<TEntity>()
            throw new NotImplementedException();
        }
        #endregion ValidationOwnerEntity

        //ولیدشن قوانین تجاری
        #region BusinessRawValidation
        public virtual bool BusinessRawValidation( TEntity entity)
      {
          throw new NotImplementedException();
      }



        public IQueryable<TEntity> GetAll()
        {
            return _EasyStoreLog.Set<TEntity>();
        }
        public IPagedList<TEntity> GetAll(Pagination pagination)
        {
            var result = _EasyStoreLog.Set<TEntity>();
            return PagedList<TEntity>.Create(result, pagination);
        }

        public IEnumerable<TEntity> Find(Func<TEntity, bool> predicate)
        {
            return GetAll().Where(predicate);
        }

        public TEntity FirstOrDefault(Func<TEntity, bool> predicate)
        {
            return GetAll().FirstOrDefault(predicate);
        }


// to do Remove
        public TEntity First(Func<TEntity, bool> predicate)
        {
            return GetAll().FirstOrDefault(predicate);
        }

        public void Add(TEntity entity)
        {
            _EasyStoreLog.Add<TEntity>(entity);
        }

        public void Update(TEntity entity, Func<TEntity, bool> predicate)
        {
            _EasyStoreLog.Update(predicate);
        }
        public void Update(TEntity entity)
        {
            _EasyStoreLog.Update(entity);
        }

        public void Delete(TEntity entity)
        {
            _EasyStoreLog.Remove<TEntity>(entity);

        }

  

        public bool SaveChanges()
        {
           return (_EasyStoreLog.SaveChanges() > 0) ;
        }





        public IEnumerable<TEntity> GetByProp(string PropName, int Value)
        {
            throw new NotImplementedException();
        }



        public void Dispose()
        {
            _EasyStoreLog.Dispose();
        }

        #endregion BusinessRawValidation
    }
}
