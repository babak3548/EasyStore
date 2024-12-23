using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataLayer;
using DataLayer.Enums;
using Utility;

using System.Web;

using DataLayer.Contract;
using ServiceLayer.Maper;
using DataLayer.EF;
using Microsoft.EntityFrameworkCore;

namespace ServiceLayer
{
    public partial class CategoryService : BaseService<Category>
    {


        Category _Category = new Category();

        public CategoryService(OnlineShopping OnlineShopping)
            : base(OnlineShopping)
        {
        }

        private static List<CategoryContract> _categoryList;
        public static List<CategoryContract> categoryList
        {
            get
            {
                if (_categoryList != null) return _categoryList;
                else return null;
            }
        }

        public void FillCategory()
        {
            //  if (OnlineShopping == null) OnlineShopping = new OnlineShopping();
            CategoryService _categoryService = new CategoryService(_OnlineShopping);
            CategoryMaper _categoryMaper = new CategoryMaper();
            ProductService productService = new ProductService(_OnlineShopping);
            var cats = _categoryService.GetAll();
            var prds = productService.GetAll();

            var _categoryContracts = _categoryMaper.EntityToContract(cats.ToList()).ToList();
            var resCatsHaveProduct = from cat in cats
                                     join prd in prds
                                      on cat.Id equals prd.FkCategory
                                     where prd.Active != false
                                     select cat;

            foreach (var CatContract in _categoryContracts)
            {
                if (resCatsHaveProduct.Any(c => c.IdsParent.Contains("," + CatContract.Id.ToString() + ",")))
                { CatContract.HaveProduct = true; }
                else
                { CatContract.HaveProduct = false; }
            }
            _categoryList = _categoryContracts.OrderByDescending(c => c.Image_ForOrderByUse).ToList();
        }



        public CategoryContract ParentCategoryById(int? Id)
        {
            if (Id == 0 | Id == null) return categoryList.FirstOrDefault(c => c.Id == ConstSetting.FK_CategoryBaseParent);

            CategoryContract currentCategoryContract = categoryList.FirstOrDefault(c => c.Id == Id);
            if (currentCategoryContract.FK_Category == ConstSetting.FK_CategoryBaseParent) return currentCategoryContract;
            else return ParentCategoryById(currentCategoryContract.FK_Category);
        }
        public void GetParentsCategoryIdsInString(int? Id, ref string categoryIdsParents)
        {

            if (Id == 0 | Id == null) categoryIdsParents += ConstSetting.FK_CategoryBaseParent.ToString() + ",";

            Category currentCategoryContract = FirstOrDefault(c => c.Id == Id);
            if (currentCategoryContract.Id == ConstSetting.FK_CategoryBaseParent) categoryIdsParents += currentCategoryContract.Id + ",";
            else
            {
                categoryIdsParents += currentCategoryContract.Id + ",";
                GetParentsCategoryIdsInString(currentCategoryContract.FkCategory, ref categoryIdsParents);
            }
        }

        public CategoryContract GetFirestCategory(int? Id)
        {
            if (Id == 0 | Id == null) return categoryList.FirstOrDefault(c => c.Id == ConstSetting.FK_CategoryBaseParent);

            CategoryContract currentCategoryContract = categoryList.FirstOrDefault(c => c.Id == Id);
            CategoryContract FirestCategoryForId = categoryList.FirstOrDefault(c => c.FK_Category == Id);
            if (FirestCategoryForId == null) return currentCategoryContract;
            else return GetFirestCategory(FirestCategoryForId.Id);
        }
        public int GetFirestCategoryId(int? Id)
        {
            if (Id == 0) return 0;
            else
                return GetFirestCategory(Id).Id;
        }

        public List<SelectListItem> CategorySelectListItem(int CurrentCatId)
        {
            List<SelectListItem> listSelectListItem1 = new List<SelectListItem>();
            foreach (var parentCategory in categoryList.Where(c => c.FK_Category == ConstSetting.FK_CategoryBaseParent))
            {
                foreach (var subCategory in categoryList.Where(c => c.FK_Category == parentCategory.Id))
                {
                    if (categoryList.FirstOrDefault(c => c.FK_Category == subCategory.Id) != null)//اگر زیر گروه داشت
                    {
                        foreach (var childCategory in categoryList.Where(c => c.FK_Category == subCategory.Id))
                        {
                            listSelectListItem1.Add(new SelectListItem
                            {
                                Text = parentCategory.Name + ">>" + subCategory.Name + ">>" + childCategory.Name,
                                Value = childCategory.Id.ToString(),
                                Selected = (childCategory.Id == CurrentCatId)
                            });
                        }
                    }
                    else//اگر زیر گروه نداشت
                    {
                        listSelectListItem1.Add(new SelectListItem
                        {
                            Text = parentCategory.Name + ">>" + subCategory.Name,
                            Value = subCategory.Id.ToString(),
                            Selected = (subCategory.Id == CurrentCatId)
                        });
                    }
                }
            }
            return listSelectListItem1;
        }

        public string GetCategoryNameById(int Id)
        {
            if (Id == 0) return "همه گروه ها";
            return categoryList.FirstOrDefault(c => c.Id == Id).Name;
        }
        public string GetCategoryNameByEn(string enName)
        {
            if (string.IsNullOrWhiteSpace( enName)) return "همه گروه ها";
            return categoryList.FirstOrDefault(c => c.Discription == enName).Name;
        }


        public IEnumerable<url> CreateSiteMapList(string lastModifiedProd)
        {
            var resultUrl = categoryList.Where(c => c.HaveProduct == true).Select(c => new url
            {
                changefreq = "monthly",//weeklyاگر سایت شلوغ شد این فیل باید به هفته گی یا روزانه تبدیل گردد 
                lastmod = "",
                priority = "0.6",
                loc = AppSetting.DomainName + "/?FkCategory=" + c.Id.ToString()
                      + "&CategoryName=" + c.Name.Replace(" ", "-")
            });

            return resultUrl;
        }
        
        public IPagedList<Category> GetAll(Pagination pagination)
        {
           var result = _OnlineShopping.Category.Include(a => a.FkCategoryNavigation).Where(a => true);

            return PagedList<Category>.Create(result, pagination);
        }
        public Category GetCatrgory(int id)
        {
            var result = _OnlineShopping.Category.Include(a => a.FkCategoryNavigation).FirstOrDefault(a=> a.Id == id);

            return result;
        }
        public void DeleteCategory(int id)
        {
            var category = _OnlineShopping.Category.First(a => a.Id == id);

            var  hasProduct = _OnlineShopping.Product.Any(a => a.FkCategory == category.Id);
            if (hasProduct)
            {
                throw new BizException("گروه دارای کالا می باشد و امکان حذف آن وجود ندارد.");
            }

            var haschild = _OnlineShopping.Category.Any(a => a.FkCategory == category.Id);
            if (haschild)
            {
                throw new BizException("گروه به عنوان سر گروه گروههای دیگر می باشد.");
            }

            _OnlineShopping.Category.Remove(category);
            _OnlineShopping.SaveChanges();
        }
        public void Save(Category category)
        {
            if(category.Id <= 0)
            {
                category.TitlePage = category.TitlePage.Replace(" ", "-");
                _OnlineShopping.Category.Add(category);
                _OnlineShopping.SaveChanges();
            }
            else
            {
            var oldCat=    _OnlineShopping.Category.FirstOrDefault(c=>c.Id == category.Id);
                oldCat.Name = category.Name;
                //oldCat.Image = category.Image;
                oldCat.IdsParent = category.IdsParent;
                oldCat.TitlePage = category.TitlePage;
                oldCat.FkCategory = category.FkCategory;
                oldCat.Active = category.Active;
               // oldCat.PromotionType = category.PromotionType;
                // _OnlineShopping.Category.Update(category);
                _OnlineShopping.SaveChanges();
            }
        }

    }
}
