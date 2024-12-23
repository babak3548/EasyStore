using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ServiceLayer;
using UILayer.Maper;
using DataLayer.Miscellaneous;
using DataLayer.Contract;
using DataLayer;
using Utility;

using DataLayer.Enums;
using UILayer.Miscellaneous;

using System.Runtime.Serialization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Net;
using System.IO;
using DataLayer.EF;

namespace UILayer.Controllers
{
    //   public class BaseController<T,K,M> : Controller where T : TEntity
    public class BaseController<TEntity, TRepository, TMaper, TContract> : Base0Controller
        where TEntity : class
        where TRepository : BaseService<TEntity>
        where TMaper : BaseMaper<TEntity, TContract>
        where TContract : IContract
    {
        protected TRepository _service;
        protected TMaper _maper;
        protected TEntity _entity;
        //protected const int PageSize = 20;
        protected const int BeginRow = 1;
        protected string _entityName = string.Empty;
        protected string _errorMessage;
        //private  ContentService _contentService = new ContentService();
        //private  ContentMaper _contentMaper = new ContentMaper();
        private Dictionary<string, Object> FilterParam = new Dictionary<string, Object>();

        protected ModelRow _modelRow = new ModelRow();
        protected ModelGrid _modeGrid = new ModelGrid();
        protected List<ModelRow> _ListModelRow = new List<ModelRow>();
        private string propIdName = "Id";

        protected ModelGrid PartialViewValuesWithGrid = new ModelGrid();

        // public OnlineShopping objectContext;

        public BaseController(string EntityName, OnlineShopping _onlineShopping) : base(EntityName, _onlineShopping)
        {
            _entityName = EntityName;
        }


    }
}


