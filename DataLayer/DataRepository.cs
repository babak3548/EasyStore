//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Data.Objects.DataClasses;
//using System.Data;
//using System.Data.Common;
//using System.Data.Objects;
//using System.Web.Configuration;
//using DataLayer;
//using System.Linq.Expressions;
//using System.Reflection;
//using System.Data.SqlClient;
//using System.Runtime.Serialization;

//namespace DataLayer
//{
//    //[Serializable()]
//    //[DataContractAttribute(IsReference = true)]
//    //public class EntityObject : EntityObject
//    //{

//    //    public virtual global::System.Int32 Id { get; set; }
//    //    //در صورت استفاد باید،  از این کلاس  انتیتی های پروژه ارث بری کنند و ویژگی این کلاس در تمام انتیتی ها اور راید شود 
//    //}

//    public class StaticObjectContext
//    {
//        private static ObjectContext _ObjectContext;

//        public static ObjectContext ObjectContext
//        {
//            get
//            {
//                if (_ObjectContext == null)
//                {
//                    _ObjectContext = new ShoppingCentersEntities(AppSetting.ConnectionString);
//                    return _ObjectContext;
//                }
//                else
//                {
//                    return _ObjectContext;
//                }


//            }
//        }
//    }

//    public interface IRepository<T> : IDisposable where T : EntityObject
//    {

//        IQueryable<T> Fetch();

//        IEnumerable<T> GetAll();

//        IEnumerable<T> Find(Func<T, bool> predicate);
//        T FirstOrDefault(Func<T, bool> predicate);

//        T Single(Func<T, bool> predicate);

//        T First(Func<T, bool> predicate);

//        void Add(T entity);

//        void Update(T entity, Func<T, bool> predicate);

//        bool Delete(T entity);

//        void Attach(T entity);

//        bool SaveChanges(out string message);

//        void SaveChanges(SaveOptions options);

//        IEnumerable<T> GetByFilterAndContians(Dictionary<string, object> FilterParam);

//        IEnumerable<T> GetByFilterOrEqual(Dictionary<string, object> FilterParam);

//        IEnumerable<T> GetByProp(string PropName, int Value);

//        void DeleteByPropName(string PropName, int Value);

//    }
//    /// <summary>

//    /// A generic repository for working with data in the database

//    /// </summary>

//    /// <typeparam name="T"></typeparam>

//    public class DataRepository<T> : IRepository<T> where T : EntityObject
//    {
//        ExpressionCreator<T> expressionCreator = new ExpressionCreator<T>();
//        string errorMessage;
//        /// <summary>

//        /// The context object for the database

//        /// </summary>
//        protected ObjectContext _context;



//        /// <summary>

//        /// The IObjectSet that represents the current entity.

//        /// </summary>
//        private IObjectSet<T> _objectSet;


//        /// <summary>

//        /// Initializes a new instance of the DataRepository class

//        /// </summary>
//        //public DataRepository()
//        //    : this(AppSetting.ConnectionString)
//        //{
//        //}

//        /// <summary>

//        /// Initializes a new instance of the DataRepository class
//        ///شروع ایجاد یک آبجکت از انتی تی فریم ورک 
//        /// </summary>
//        public DataRepository(string connectionString)
//            : this(new ShoppingCentersEntities(connectionString))
//        {
//        }


//        /// <summary>

//        /// Initializes a new instance of the DataRepository class

//        /// </summary>

//        /// <param name="context">The Entity Framework ObjectContext</param>
//        public DataRepository(ObjectContext context)
//        {
//            _context = context;
//            _objectSet = _context.CreateObjectSet<T>();

//        }

//        #region FullTextSearch Function

//        //public IEnumerable<Product> SrchAjaxKeyWordFuncProducts(string srchValue, int? p_FkCategory)
//        //{

//        //    return ((ShoppingCentersEntities)_context).SrchAjaxKeyWord(srchValue, p_FkCategory);
//        //}

//        public List<Product> SrchNamBrandKeywdDiscription(string srchValue, int? rowPerPage, int? pageNo, int? p_FkCategory, out  int count)
//        {
         
//            ObjectParameter pramcount = new ObjectParameter(@"count", typeof(int?));
//            var result= ((ShoppingCentersEntities)_context).SrchNamBrandKeywdDiscription(srchValue, rowPerPage, pageNo, p_FkCategory, pramcount).ToList();
//            count = (int)pramcount.Value;
//            return result;
//        }

//        public List<BusinessOwner> SrchBusOwnNamTypeActivite(string srchValue, int? rowPerPage, int? pageNo, out int count)
//        {
//            ObjectParameter countParam = new ObjectParameter(@"count", typeof(int));
//            var result = ((ShoppingCentersEntities)_context).SrchBusOwnNamTypeActivite(srchValue, rowPerPage, pageNo, countParam).ToList();
//            count = (int)countParam.Value;
//            return result;
//        }

//        public List<Marketer> SrchMarketerNamTypeActivite(string srchValue, int? rowPerPage, int? pageNo, out int count)
//        {
//            ObjectParameter countParam = new ObjectParameter(@"count", typeof(int));

//            var result = ((ShoppingCentersEntities)_context).SrchMarketerNamTypeActivite(srchValue, rowPerPage, pageNo, countParam).ToList();
//            count = (int)countParam.Value;
//            return result;
//        }
       
//        /*
//                public int SrchNamBrandKeywdDiscriptionCount(string srchValue, int? catId)
//                {
//                    return ((ShoppingCentersEntities)_context).SrchNamBrandKeywdDiscriptionCount(srchValue,catId).FirstOrDefault().Value;
//                }

      

//                public int SrchBusOwnNamTypeActivateCount(string srchValue)
//                {

//                    return ((ShoppingCentersEntities)_context).SrchBusOwnNamTypeActivateCount(srchValue).FirstOrDefault().Value;
//                }



//                public int SrchMarketerNamTypeActiviteCount(string srchValue)
//                {

//                    return ((ShoppingCentersEntities)_context).SrchMarketerNamTypeActiviteCount(srchValue).FirstOrDefault().Value;
//                }
//                */
//        #endregion FullTextSearch Function


//        /// <summary>

//        /// Gets all records as an IQueryable

//        /// </summary>

//        /// <returns></returns>
//        public IQueryable<T> Fetch()
//        {

//            return _objectSet;
//        }



//        /// <summary>

//        /// Gets all records as an IEnumberable

//        /// </summary>

//        /// <returns>An IEnumberable object containing the results of the query</returns>
//        public IEnumerable<T> GetAll()
//        {
//            //ehtmalan .CreateObjectSet<T>() bayad hazf shavad
//            return _context.CreateObjectSet<T>().AsQueryable<T>();
//        }

//        /// <summary>
//        ///این فیلتیر برای اند وشامل بودن  
//        /// </summary>
//        /// <param name="FilterParam"></param>
//        /// <returns></returns>
//        public IEnumerable<T> GetByFilterAndContians(Dictionary<string, object> FilterParam)
//        {
//            //ehtmalan .CreateObjectSet<T>() bayad hazf shavad
//            if (FilterParam == null)
//                return GetAll();
//            return _context.CreateObjectSet<T>().AsQueryable<T>().Where(expressionCreator.AndContains(FilterParam).Compile());
//        }

//        /// <summary>
//        ///این فیلتیر برای اور مساوی  بودن  
//        /// </summary>
//        /// <param name="FilterParam">لیستی از نام پروپرتی و مقدار فیلتیر برای این ویژگیی می باشد</param>
//        /// <returns></returns>
//        public IEnumerable<T> GetByFilterOrEqual(Dictionary<string, object> FilterParam)
//        {
//            //ehtmalan .CreateObjectSet<T>() bayad hazf shavad
//            if (FilterParam == null)
//                return GetAll();
//            return _context.CreateObjectSet<T>().AsQueryable<T>().Where(expressionCreator.OrEqual(FilterParam).Compile());
//        }

//        /// <summary>
//        /// این فیلتیر برای مساوی  بودن پروپرتیهای از نوع اینت 32
//        /// </summary>
//        /// <param name="PropName">نام پروپرتی</param>
//        /// <param name="Value">مقدار پروپرتی</param>
//        /// <returns></returns>
//        public IEnumerable<T> GetByProp(string PropName, int Value)
//        {
//            // DataLayer.Marketer x = new Marketer();

//            return _context.CreateObjectSet<T>().AsQueryable<T>().Where(expressionCreator.EqualInt32(PropName, Value).Compile());
//        }


//        public void DeleteByPropName(string PropName, int Value)
//        {

//            IEnumerable<T> records = GetByProp(PropName, Value);



//            foreach (T record in records)
//            {

//                _objectSet.DeleteObject(record);

//            }
//            //  SaveChanges( out errorMessage);
//        }

//        public void RoleBackContext()
//        {
//            // delete added objects that did not get saved
//            foreach (var entry in _context.ObjectStateManager.GetObjectStateEntries(EntityState.Added))
//            {
//                if (entry.Entity != null)
//                    _context.DeleteObject(entry.Entity);
//            }
//            // Refetch modified objects from database
//            foreach (var entry in _context.ObjectStateManager.GetObjectStateEntries(EntityState.Modified))
//            {
//                if (entry.Entity != null)
//                    _context.Refresh(System.Data.Objects.RefreshMode.StoreWins, entry.Entity);
//            }
//            // Recover modified objects that got deleted
//            foreach (var entry in _context.ObjectStateManager.GetObjectStateEntries(EntityState.Deleted))
//            {
//                if (entry.Entity != null)
//                    _context.Refresh(System.Data.Objects.RefreshMode.StoreWins, entry.Entity);
//            }

//            _context.AcceptAllChanges();
//        }

//        /// <summary>

//        /// Finds a record with the specified criteria

//        /// </summary>

//        /// <param name="predicate">Criteria to match on</param>

//        /// <returns>A collection containing the results of the query</returns>
//        public IEnumerable<T> Find(Func<T, bool> predicate)
//        {

//            return _objectSet.Where<T>(predicate);
//        }



//        /// <summary>

//        /// Gets a single record by the specified criteria (usually the unique identifier)

//        /// </summary>

//        /// <param name="predicate">Criteria to match on</param>

//        /// <returns>A single record that matches the specified criteria</returns>
//        public T Single(Func<T, bool> predicate)
//        {

//            return _objectSet.Single<T>(predicate);

//        }



//        /// <summary>

//        /// The first record matching the specified criteria

//        /// </summary>

//        /// <param name="predicate"></param>

//        /// <returns></returns>
//        public T First(Func<T, bool> predicate)
//        {
//            return _objectSet.First<T>(predicate);
//        }

//        /// <summary>
//        /// The first record matching the specified criteria

//        /// </summary>

//        /// <param name="predicate">Criteria to match on</param>

//        /// <returns>A single record containing the first record matching the specified criteria</returns>
//        public T FirstOrDefault(Func<T, bool> predicate)
//        {

//            return _objectSet.FirstOrDefault<T>(predicate);

//        }



//        /// <summary>

//        /// Deletes the specified entitiy

//        /// </summary>

//        /// <param name="entity">Entity to delete</param>

//        /// <exception cref="ArgumentNullException"> if <paramref name="entity"/> is null</exception>
//        public bool Delete(T entity)
//        {

//            if (entity == null)
//            {

//                throw new ArgumentNullException("entity");

//            }



//            _objectSet.DeleteObject(entity);
//            return true;

//        }



//        /// <summary>

//        /// Deletes records matching the specified criteria

//        /// </summary>

//        /// <param name="predicate">Criteria to match on</param>
//        public void Delete(Func<T, bool> predicate)
//        {

//            IEnumerable<T> records = from x in _objectSet.Where<T>(predicate) select x;



//            foreach (T record in records)
//            {

//                _objectSet.DeleteObject(record);

//            }
//            //  SaveChanges( out errorMessage);
//        }



//        /// <summary>

//        /// Adds the specified entity

//        /// </summary>

//        /// <param name="entity">Entity to add</param>

//        /// <exception cref="ArgumentNullException"> if <paramref name="entity"/> is null</exception>
//        public void Add(T entity)
//        {

//            if (entity == null)
//            {

//                throw new ArgumentNullException("entity");

//            }

//            _objectSet.AddObject(entity);
//            // SaveChanges(out errorMessage);
//        }
//        /// <summary>
//        /// این متد یک انتیتی را اضافه یا به روز رسانی می کند
//        /// </summary>
//        /// <param name="entity"></param>
//        /// <param name="predicate"> یک دلیگیت بر اساس آیدی انتیتی پاس داده شده است</param>
//        public void Update(T entity, Func<T, bool> predicate)
//        {
//            if (entity == null)
//            {
//                throw new ArgumentException("entity");
//            }
//            var _entity = _objectSet.FirstOrDefault<T>(predicate);
//            if (_entity != null)
//            {
//                _objectSet.DeleteObject(_entity);
//                _context.SaveChanges();

//            }

//            _objectSet.AddObject(entity);


//        }

//        /// <summary>

//        /// Attaches the specified entity

//        /// </summary>

//        /// <param name="entity">Entity to attach</param>
//        public void Attach(T entity)
//        {

//            _objectSet.Attach(entity);

//        }

//        /// Saves all context changes
//        public void SaveChanges()
//        {
//            _context.SaveChanges();


//        }

//        /// <summary>

//        /// Saves all context changes
//        public bool SaveChanges(out  string errorMessage)
//        {
//            _context.SaveChanges();
//            errorMessage = string.Empty;
//            return true;
//        }

//        /// </summary>
//        // public bool   SaveChanges(out  string  errorMessage )
//        //{
//        //    try
//        //    {
//        //        _context.SaveChanges();
//        //    }
//        //    catch (Exception ex)
//        //    {
//        //        if (((System.Data.SqlClient.SqlException)(ex.InnerException)) != null)
//        //        {
//        //            switch (((System.Data.SqlClient.SqlException)(ex.InnerException)).Number)
//        //        {
//        //            case 2601:
//        //                {
//        //                    errorMessage = "به علت تکراری بودن داده ها رکورد مورد نظر ذخیره نمی شود";
//        //                    return false;
//        //                }
//        //            case 547:
//        //                {
//        //                    errorMessage = "به علت ارتباط این رکورد با سایر رکوردهای جداول دیگر ، امکان حذف آن وجود ندارد  ";
//        //                    return false;
//        //                }
//        //            default:
//        //                {
//        //                    errorMessage = "خطای سمت پایگاه داده رخ داده است";
//        //                    return false;
//        //                }
//        //        }
//        //        }
//        //        errorMessage = "عملیات با شکست مواجه شد";
//        //        return false;
//        //    }
//        //    errorMessage = string.Empty;
//        //    return true;

//        //}



//        /// <summary>

//        /// Saves all context changes with the specified SaveOptions

//        /// </summary>

//        /// <param name="options">Options for saving the context</param>
//        public void SaveChanges(SaveOptions options)
//        {

//            _context.SaveChanges(options);

//        }

//        /// <summary>

//        /// Releases all resources used by the WarrantManagement.DataExtract.Dal.ReportDataBase

//        /// </summary>
//        public void Dispose()
//        {

//            Dispose(true);

//            GC.SuppressFinalize(this);

//        }

//        /// <summary>

//        /// Releases all resources used by the WarrantManagement.DataExtract.Dal.ReportDataBase

//        /// </summary>

//        /// <param name="disposing">A boolean value indicating whether or not to dispose managed resources</param>
//        protected virtual void Dispose(bool disposing)
//        {

//            if (disposing)
//            {

//                if (_context != null)
//                {

//                    _context.Dispose();

//                    _context = null;

//                }

//            }

//        }


//        public object Attach()
//        {
//            throw new NotImplementedException();
//        }

//        public void RejectAProperty(T entity, string propertyName)
//        {
//            entity.RejectChange(propertyName, _context);
//        }

//        public void RejectAEntity(T entity)
//        {
//            entity.RejectChange(_context);
//        }

//        public void RejectAll()
//        {
//            _context.RejectAll();
//        }

//        //IEnumerable<T> IRepository<T>.GetAll()
//        //{
//        //    throw new NotImplementedException();
//        //}



//    }


//}
