using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataLayer;
using DataLayer.EF;
using DataLayer.Enums;
using Microsoft.EntityFrameworkCore;

namespace ServiceLayer
{
    public partial class ContentService : BaseService<Content>
    {
        public ContentService(OnlineShopping OnlineShopping)
            : base(OnlineShopping)
        {
        }

        public  IQueryable<Content> GetContentByType(ContentTypes contentType)
        {
            var list = GetAll().Where(c => c.ContentType == contentType).OrderByDescending(c=>c.Position);

            return list;


        }

 
    }
}
