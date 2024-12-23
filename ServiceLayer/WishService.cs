using DataLayer.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using DataLayer;
using DataLayer.Enums;
using Utility;
using System.Reflection;



using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;

using DataLayer.Models;

using DataLayer.Models;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace ServiceLayer
{
    public class WishService : BaseService<Wish>
    {
        public WishService(OnlineShopping OnlineShopping)
            : base(OnlineShopping)
        {
        }

        public  IQueryable<Wish> GetWishUser(int userId)
        {
          return  _OnlineShopping.Wishes.Where(w => w.IsDeleted == false && w.FkUser == userId).Include(p => p.Product);
        }
        public void DeActiveWishUser(int userId, int wishId)
        {
            var remItem= FirstOrDefault(w => w.IsDeleted == false && w.FkUser == userId && w.Id==wishId) ;
            remItem.IsDeleted = true;
            SaveAllChengeOrAllReject(true);
        }
        public void AddToWishUser(int userId, int ProductId)
        {
            var newItem = new Wish() { FkProduct = ProductId, FkUser = userId, IsDeleted = false, RegisterDate = DateTime.Now };
            Add(newItem);
            SaveAllChengeOrAllReject(true);
        }

        public int GetCountWishes(int id)
        {
          return  GetAll().Where(w => w.FkUser == id && w.IsDeleted==false).CountAsync().Result;
        }
    }

}
