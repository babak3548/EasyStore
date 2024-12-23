using DataLayer;
using DataLayer.Enums;
using Utility;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

using DataLayer.EF;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;

using DataLayer.Models;

using DataLayer.Models;
using System.Linq.Expressions;
using DataLayer.Contract;
using System.Threading.Tasks;

namespace ServiceLayer
{
    public partial class ProductService : BaseService<Product>
    {

        //public string ConnectionString
        //{
        //    get
        //    {
        //        return ConfigurationExtensions.GetConnectionString(this.Configuration, "ConnectionStringShoppingCenter");
        //    }
        //}


        public ProductService(OnlineShopping onlineShopping) : base(onlineShopping)
        {

        }
        ///برای ولیدیت کردن صاحب انتیتی مورد استفاده قرار می گیردو متد های کلاس پدر را هر کلاس باید اور راید می کند
        #region ValidationOwnerEntity
        public override bool CheckOwnerEntity(int Id, int FkUser, out Product product)
        {
            product = FirstOrDefault(p => p.Id == Id);
            if (product != null && product.FkBusinessOwnerNavigation.FkUser == FkUser)
                return true;
            else
                return false;
        }

        public override bool CheckOwnerEntity(Product entity, int FkUser)
        {
            if (entity != null && entity.FkBusinessOwnerNavigation.FkUser == FkUser)
                return true;
            else
                return false;
        }
        #endregion ValidationOwnerEntity

        #region BusinessRawValidation
        public override bool BusinessRawValidation(Product entity)
        {
            if (entity.PersentMarkater < ConstSetting.MinParentMarketing || entity.PersentMarkater > ConstSetting.MaxParentMarketing)
                return false;
            else
                return true;

        }


        #endregion BusinessRawValidation

        public string sitemapAddress
        {
            get
            {
                return ConfigurationExtensions.GetConnectionString(AppSetting.Configuration, "sitemapAddress");
            }
        }
        public void AddRecordSiteMap(Product product)
        {
            // var s=  Path.GetFullPath(sitemapAddress);
            //  var x = Server.MapPath();
            // var s = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);//Path.GetDirectoryName(Application.ExecutablePath);// Server.MapPath("~");
            //+ Paths.LogPath + Paths.ErrorLogFileName
            if (File.Exists(sitemapAddress))
            {
                string str = File.ReadAllText(sitemapAddress);
                object urlset = XmlSerializerUtility.DeSerialize<urlset>(str);
                object urls = XmlSerializerUtility.DeSerialize<url>(str);
            }
        }

        public IEnumerable<url> CreateSiteMapList(string lastModifiedProd)
        {
            var resultUrl = Find(p => p.Active != false).OrderBy(p => p.Id).Select(p => new url
            {
                changefreq = "monthly",
                lastmod = (p.RegisterDate.StringDateToInteger() >= lastModifiedProd.StringDateToInteger() ? DateTime.Now.Date.ToString("yyyy-MM-dd") : "2014-05-22"),
                priority = "0.7",
                loc = AppSetting.DomainName + "/Product/" + p.NameForUrll
            });

            return resultUrl;
        }


        public IPagedList<Product> GetAll(Pagination pagination , Product product, ProductActivationStatus? productActivationStatus)
        {

            var products = GetAll();

            if (product?.Id > 0) products = products.Where(p => p.Id == product.Id);
            if (product?.FkCategory > 2 ) products = products.Where(p => p.FkCategory == product.FkCategory);
            if (!string.IsNullOrWhiteSpace(product?.Name)) products = products.Where(p => p.Name.Contains(product.Name));

            if (product?.BeforDiscountPrice >0) products = products.Where(p => p.BeforDiscountPrice > product.BeforDiscountPrice);

            if (productActivationStatus == ProductActivationStatus.ActiveForSell) products = products.Where(p => p.Active == true);
            if (productActivationStatus == ProductActivationStatus.UnActiveAnyReason) products = products.Where(p => p.Active == false);

            if (product.AddDate != null && product.AddDate > new DateTime(1,1,1)) products = products.Where(p => p.AddDate >= product.AddDate);
            var query = products.OrderByDescending(o => o.Id).Include(a => a.BrigeProductCategories);

            

            return PagedList<Product>.Create(query, pagination);
        }
        public Product GetById(int id)
        {
            var result = _OnlineShopping.Product.FirstOrDefault(a => a.Id == id);

            return result;
        }
        public IQueryable<Product> SrchNamBrandKeywdDiscription(SearchModel searchModel, out int count)
        {
            
            IQueryable<Product> query = _OnlineShopping.Product.Include(p => p.PromotionProducts).Where(p => p.Active != false);
            // .OrderByDescending(o => o.FkCategory == fK_Category)

            if (searchModel.Price_min > 0) query = query.Where(p => p.Price > searchModel.Price_min);
            if (searchModel.Price_max > 0) query = query.Where(p => p.Price < searchModel.Price_max);

            if (searchModel.PromotionType != PromotionTypes.NoSetPromotionType)
                query = query.Where(p => p.PromotionProducts.Any(pr => pr.PromotionType == searchModel.PromotionType));

            if (!string.IsNullOrWhiteSpace(searchModel.query))
                query = query.Where(p => p.Brand.Contains(searchModel.query) || p.Name.Contains(searchModel.query)
            || p.WordKey.Contains(searchModel.query) || p.Discription.Contains(searchModel.query));

            if (!string.IsNullOrWhiteSpace( searchModel.category) &&  searchModel.category != DefualtValue.AllCategory)
            {
                int fk_category = _OnlineShopping.Category.FirstOrDefault(c => c.Discription == searchModel.category).Id;
                //var idStr = searchModel.category.ToString();
                query = query.Where(p => p.FkCategory == fk_category ||
                p.FkCategoryNavigation.FkCategory == fk_category );
            }

            count = query.Count();
            if (searchModel.Sorting == Sorting.cheaper) query = query.OrderBy(o => o.Price);
            else if (searchModel.Sorting == Sorting.expensive) query = query.OrderByDescending(o => o.Price);
            else if (searchModel.Sorting == Sorting.RankSelling) query = query.OrderByDescending(o => o.RankSelling); // to do job for filling this field
            else if (searchModel.Sorting == Sorting.newer) query = query.OrderByDescending(o => o.RegisterDate);
            else query = query.OrderByDescending(o => o.RankShow);



            return query.Skip(searchModel.PageSize * (searchModel.PageNo - 1)).Take(searchModel.PageSize);
            // throw new NotImplementedException();
        }

        public void Copy(Product destintion, Product source)
        {
            destintion.Id = source.Id;
           // destintion.NameForUrll = source.NameForUrll; تغییر نمی کند
            destintion.Name = source.Name;
            destintion.Brand = source.Brand;
            destintion.Image = source.Image;
            destintion.Image1 = source.Image1;
            destintion.Image2 = source.Image2;
            destintion.Image3 = source.Image3;
            destintion.Image4 = source.Image4;
            destintion.BeforDiscountPrice = source.BeforDiscountPrice;
            destintion.BuyPrice = source.BuyPrice;
            destintion.Price = source.Price;
            destintion.RegisterDate = source.RegisterDate;
            destintion.AddDate = source.AddDate;
            destintion.UpdateDate = source.UpdateDate;
            destintion.Discription = source.Discription;
            destintion.FkBusinessOwner = source.FkBusinessOwner;
            destintion.MadeInCountry = source.MadeInCountry;
            destintion.CountPrice = source.CountPrice;
            destintion.MinCountForPrice = source.MinCountForPrice;
            destintion.Available = source.Available;
            destintion.AvailableColors = source.AvailableColors;
            destintion.PersentMarkater = source.PersentMarkater;
            destintion.AcceptReturnDay = source.AcceptReturnDay;
            destintion.ShippingDiscription = source.ShippingDiscription;//to do rename
            destintion.MaxShippingDay = source.MaxShippingDay;
            destintion.MinShippingDay = source.MinShippingDay;
            destintion.WordKey = source.WordKey;
            destintion.Dimansion = source.Dimansion;
            destintion.UnitBuy = source.UnitBuy;
            destintion.WightUnitBuyWithKg = source.WightUnitBuyWithKg;
            destintion.Active = source.Active;
            destintion.FkCategory = source.FkCategory;
            destintion.RankShow = source.RankShow;
            destintion.RankSelling = source.RankSelling;
            destintion.FkContent = source.FkContent;
            destintion.CalculatedStar = source.CalculatedStar;
            destintion.UserStar = source.UserStar;
            destintion.MetaDescription = source.MetaDescription;
            destintion.VideoUrl = source.VideoUrl;
        }

        public List<Product> GetRelaetedProduct(int? fkCategory)
        {
            return GetProductACategory(fkCategory, p => p.RankShow).Take(15).ToList();
        }

        private List<Product> GetProductACategory(int? fkCategory, Expression<Func<Product, int?>> keySelector)
        {
            IQueryable<Product> queryOginal = _OnlineShopping.Product.Where(p => p.Active != false);
            IQueryable<Product> query = queryOginal;
            if (fkCategory != null)
                query = queryOginal.OrderByDescending(keySelector).Where(p => p.FkCategory == fkCategory);

            var list = query.ToList();
            if (list.Count >= 8)
                return list;


            var category = _OnlineShopping.Category.FirstOrDefault(category => category.Id == fkCategory);
            var listIds = list.Select(s => s.Id);
            string parenCategoryId = category != null ? category.FkCategory.ToString() : "2";
            query = queryOginal.Where(p => p.FkCategoryNavigation != null && p.FkCategoryNavigation.IdsParent.Contains(parenCategoryId) && !listIds.Contains(p.Id));
            int modeh = 8 - list.Count;
            var res2 = query.OrderByDescending(keySelector).Take(modeh).ToList();

            list.AddRange(res2);

            if (list.Count >= 7) return list;

            // query = queryOginal.OrderByDescending(keySelector).Take(6);

            return queryOginal.OrderByDescending(keySelector).Take(6).ToList();
        }

        public List<Product> GetBestSellerProduct(int? fkCategory)
        {
            return GetProductACategory(fkCategory, p => p.RankSelling).Take(15).ToList();
        }

        public async Task<List<PromotionProductModel>> PromotionProductByCategory(int fkCategory)
        {
            IQueryable<PromotionProduct> queryOginal = _OnlineShopping.PromotionProduct.Where(p => p.Product.Active != false && p.Product.Available > 0 && p.FkCategory == fkCategory);

            return await GetPromotionProductModels(queryOginal);
        }

        private async Task<List<PromotionProductModel>> GetPromotionProductModels(IQueryable<PromotionProduct> queryOginal)
        {
            queryOginal = queryOginal.OrderByDescending(p => p.Order);

            return await queryOginal.Select(p => new PromotionProductModel
            {
                PromotionId = p.Id,
                ExpireDateTime = p.ExpireDateTime,
                FkCategory = p.FkCategory,
                CategoryName = p.Category.Name,
                FkProduct = p.FkProduct,
                Order = p.Order,
                Price = p.Product.Price,
                ProductImage = p.Product.Image,
                ProductImage1 = p.Product.Image1,
                ProductName = p.Product.Name,
                PromotionType = p.PromotionType,

                Available = p.Product.Available,
                AvailableColors = p.Product.AvailableColors,
                 BeforDiscountPrice = p.Product.BeforDiscountPrice,
                 Discription = p.Product.Discription,
                 AbstractDiscription = p.Product.ShippingDiscription,
                ProductImage2 = p.Product.Image2,
                 ProductImage3 = p.Product.Image3,
                 ProductImage4=p.Product.Image4,
                 ProductBrand=p.Product.Brand,
                
                 ProductNameForUrl = p.Product.NameForUrll ,
            }
                ).ToListAsync();
        }

        public List<PromotionProductModel> GetPromotionProductModels(List<Product> list)
        {
            list = list.OrderByDescending(p => p.RankShow).ToList();

            return  list.Select(p => new PromotionProductModel
            {
                PromotionId = p.Id,
                ExpireDateTime = DateTime.Now,
                FkCategory = p.FkCategory,
                CategoryName = p.FkCategoryNavigation.Name,
                FkProduct = p.Id,
                Order = p.RankShow != null ? p.RankShow : 0,
                Price = p.Price,
                ProductImage = p.Image,
                ProductImage1 = p.Image1,
                ProductName = p.Name,
                ProductBrand=p.Brand,
                PromotionType = PromotionTypes.NoSetPromotionType,
                ProductNameForUrl = p.NameForUrll,
                Available = p.Available,
                AvailableColors = p.AvailableColors,
                BeforDiscountPrice = p.BeforDiscountPrice,
                Discription = p.Discription,
                AbstractDiscription = p.ShippingDiscription,
                ProductImage2 = p.Image2,
                ProductImage3 = p.Image3,
                ProductImage4 = p.Image4,
            }
                ).ToList();
        }
        public async Task<List<PromotionProductModel>> GetPromotionProductModels(IQueryable<Product> queryOginal)
        {
            queryOginal = queryOginal.OrderByDescending(p => p.RankShow);

            return await queryOginal.Select(p => new PromotionProductModel
            {
                PromotionId = p.Id,
                ExpireDateTime = DateTime.Now,
                FkCategory = p.FkCategory,
                CategoryName = p.FkCategoryNavigation.Name,
                FkProduct = p.Id,
                Order = p.RankShow != null ? p.RankShow : 0 ,
                Price = p.Price,
                ProductImage = p.Image,
                ProductImage1 = p.Image1,
                ProductName = p.Name,
                ProductBrand = p.Brand,
                PromotionType = PromotionTypes.NoSetPromotionType,
                ProductNameForUrl = p.NameForUrll,
                Available = p.Available,
                AvailableColors = p.AvailableColors,
                BeforDiscountPrice = p.BeforDiscountPrice,
                Discription = p.Discription,
                AbstractDiscription = p.ShippingDiscription,
                ProductImage2 = p.Image2,
                ProductImage3 = p.Image3,
                ProductImage4 = p.Image4,
            }
                ).ToListAsync();
        }

        public Product GetProduct(int id)
        {
            var product = _OnlineShopping.Product.Include(p => p.Comment).Include(p => p.FkCategoryNavigation).Include(p => p.BrigeProductCategories).FirstOrDefault(p => p.Id == id);
            return product;
        }
        public Product GetProduct(string nameForUrll)
        {
            var product = _OnlineShopping.Product.Include(p => p.Comment).Include(p => p.FkCategoryNavigation).Include(p => p.BrigeProductCategories).FirstOrDefault(p => p.NameForUrll == nameForUrll);
            return product;
        }
        

        public async  Task<List<PromotionProductModel>> PromotionProductByType(PromotionTypes promotionType)
        {
            IQueryable<PromotionProduct> queryOginal = _OnlineShopping.PromotionProduct.Where(p => p.Product.Active != false && p.Product.Available > 0 && p.PromotionType == promotionType);

            return await GetPromotionProductModels(queryOginal);
        }

        public async Task<List<PromotionProductModel>> PromotionProductByType(PromotionTypes promotionType, int fkCategory)
        {
            IQueryable<PromotionProduct> queryOginal = _OnlineShopping.PromotionProduct.Where(p => p.Product.Active != false && p.Product.Available > 0 && p.PromotionType == promotionType && p.FkCategory == fkCategory);
            return await GetPromotionProductModels(queryOginal);
        }

        public IQueryable<TopHomeCategoryModel> PromotionCategoryByType(PromotionCategory topHomeCategory)
        {
            // var query = _OnlineShopping.Category.Where(p => p.Active == true);

            var query = from cat in _OnlineShopping.Category
                        where cat.Product.Count() > 2 && cat.Active == true                    //join pro in _OnlineShopping.Product
                        //on cat.Id equals pro.FkCategory into g
                        select new TopHomeCategoryModel { CategoryId = cat.Id, ProductCount = cat.Product.Count(), CategoryName = cat.Name };

            //var results = from c in query
            //               group c. by p.FkCategory into g
            //             //  where g.Key.== true
            //               select new TopHomeCategoryModel  { CategoryId = g.Key, ProductCount = g.Count() , CategoryName=g.FirstOrDefault().Name };

            return query.Take(10);
        }

        public async Task IntegrateSatarUserAndComment()
        {
           var products=await GetAll().ToListAsync();
            foreach (var product in products)
            {
                int countVotePos = product.Comment.Count(c => c.IsPositiveComment == true && c.VotePositive > 0 && c.Active == true);
                float AveScorePos = countVotePos == 0 ? 0 : product.Comment.Where(c => c.IsPositiveComment == true && c.VotePositive > 0).Sum(c => c.VotePositive.Value) / countVotePos;

                int countVoteNeg = product.Comment.Count(c => c.IsPositiveComment == false && c.VotePositive > 0 && c.Active == true);
                float AveScoreNeg = countVoteNeg == 0 ? 0 : product.Comment.Where(c => c.IsPositiveComment == false && c.VotePositive > 0).Sum(c => c.VotePositive.Value) / countVoteNeg;

                float totalVote = ((AveScorePos * countVotePos) + ((5 - AveScoreNeg) * countVoteNeg)) / (countVotePos + countVoteNeg);
                product.CalculatedStar = totalVote >= 0 ? product.UserStar : ((product.UserStar * 3 + totalVote) / 4);
            }

            SaveAllChengeOrAllReject(true);
        }




    }
}
