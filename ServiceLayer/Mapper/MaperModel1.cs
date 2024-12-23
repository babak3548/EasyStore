﻿


//------------------------------------------------------------------------------
// <auto-generated>
// by mehrang 
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Data.Objects;
using System.Data.Objects.DataClasses;
using System.Data.EntityClient;
using System.ComponentModel;
using System.Xml.Serialization;
using System.Runtime.Serialization;
//using System.ComponentModel.DataAnnotations;
//using System.Web.Mvc;
using DataLayer;
using System.Collections.Generic;
using ServiceLayer;
using System.Linq;
using DataLayer.Contract;
using Utility;
using DataLayer.Enums;
using DataLayer.EF;

namespace ServiceLayer.Maper
{
    public abstract class BaseMaper<TEntity>
        where TEntity : class
    {
        public abstract void EntityToEntity(TEntity srcEntity, TEntity desEntity);
    }
    
    /// <summary>
    /// No Metadata Documentation available.
    /// </summary>
    
    public  partial class AccessMaper : BaseMaper<Access>
    {
       public override void EntityToEntity(Access srcEntity, Access desEntity)
        {
        desEntity.Id =srcEntity.Id;
        desEntity.DisplayMode =srcEntity.DisplayMode;
        desEntity.FkRole =srcEntity.FkRole;
        desEntity.FkFiled =srcEntity.FkFiled;
        }
    
    
    
    }
    /// <summary>
    /// No Metadata Documentation available.
    /// </summary>
    
    public  partial class AccountingMaper : BaseMaper<Accounting>
    {
       public override void EntityToEntity(Accounting srcEntity, Accounting desEntity)
        {
        desEntity.Id =srcEntity.Id;
        desEntity.Name =srcEntity.Name;
        desEntity.Debtor =srcEntity.Debtor;
        desEntity.Creditor =srcEntity.Creditor;
        desEntity.FkUser =srcEntity.FkUser;
        desEntity.FkInvoice =srcEntity.FkInvoice;
        desEntity.Date =srcEntity.Date;
        }
    
    
    
    }
    /// <summary>
    /// No Metadata Documentation available.
    /// </summary>
    
    public  partial class Bridge_BusinessOwner_MarketerMaper : BaseMaper<BridgeBusinessOwnerMarketer>
    {
       public override void EntityToEntity(Bridge_BusinessOwner_Marketer srcEntity, Bridge_BusinessOwner_Marketer desEntity)
        {
        desEntity.Id =srcEntity.Id;
        desEntity.FkBusinessOwner =srcEntity.FkBusinessOwner;
        desEntity.FkMarketer =srcEntity.FkMarketer;
        desEntity.DiscriptionRequest =srcEntity.DiscriptionRequest;
        desEntity.AcceptRequest =srcEntity.AcceptRequest;
        desEntity.RequestFromBusinessOwner =srcEntity.RequestFromBusinessOwner;
        desEntity.RequestFromMarketer =srcEntity.RequestFromMarketer;
        desEntity.RejectRequest =srcEntity.RejectRequest;
        desEntity.date =srcEntity.date;
        }
    
    
    
    }
    /// <summary>
    /// No Metadata Documentation available.
    /// </summary>
    
    public  partial class Bridge_Invoice_ProductMaper : BaseMaper<Bridge_Invoice_Product>
    {
       public override void EntityToEntity(Bridge_Invoice_Product srcEntity, Bridge_Invoice_Product desEntity)
        {
        desEntity.Id =srcEntity.Id;
        desEntity.Count =srcEntity.Count;
        desEntity.FkProduct =srcEntity.FkProduct;
        desEntity.FkInvoice =srcEntity.FkInvoice;
        }
    
    
    
    }
    /// <summary>
    /// No Metadata Documentation available.
    /// </summary>
    
    public  partial class Bridge_Province_BusinessOwnerMaper : BaseMaper<BridgeProvinceBusinessOwner>
    {
       public override void EntityToEntity(BridgeProvinceBusinessOwner srcEntity, BridgeProvinceBusinessOwner desEntity)
        {
        desEntity.Id =srcEntity.Id;
        desEntity.FkProvince =srcEntity.FkProvince;
        desEntity.FkBusinessOwner =srcEntity.FkBusinessOwner;
        desEntity.AnyXkg =srcEntity.AnyXkg;
        desEntity.AnyXkgmony =srcEntity.AnyXkgmony;
        desEntity.FreeGratherThanMony =srcEntity.FreeGratherThanMony;
        }
    
    
    
    }
    /// <summary>
    /// No Metadata Documentation available.
    /// </summary>
    
    public  partial class BusinessOwnerMaper : BaseMaper<BusinessOwner>
    {
       public override void EntityToEntity(BusinessOwner srcEntity, BusinessOwner desEntity)
        {
        desEntity.Id =srcEntity.Id;
        desEntity.Name =srcEntity.Name;
        desEntity.TypeActivity =srcEntity.TypeActivity;
        desEntity.WordKey =srcEntity.WordKey;
        desEntity.Address =srcEntity.Address;
        desEntity.NationalCode =srcEntity.NationalCode;
        desEntity.FkUser =srcEntity.FkUser;
        desEntity.FkProvince =srcEntity.FkProvince;
        desEntity.FkMarketer =srcEntity.FkMarketer;
        desEntity.DocumentFile =srcEntity.DocumentFile;
        desEntity.Image =srcEntity.Image;
        desEntity.WebSiteAddress =srcEntity.WebSiteAddress;
        desEntity.Discription =srcEntity.Discription;
        desEntity.Active =srcEntity.Active;
        desEntity.Tel =srcEntity.Tel;
        desEntity.Yahoo =srcEntity.Yahoo;
        desEntity.Gmail =srcEntity.Gmail;
        desEntity.Skype =srcEntity.Skype;
        desEntity.PaymentPorsantAmount =srcEntity.PaymentPorsantAmount;
        desEntity.TypeSells =srcEntity.TypeSells;
        }
    
    
    
    }
    /// <summary>
    /// No Metadata Documentation available.
    /// </summary>
    
    public  partial class CategoryMaper : BaseMaper<Category>
    {
       public override void EntityToEntity(Category srcEntity, Category desEntity)
        {
        desEntity.Id =srcEntity.Id;
        desEntity.Name =srcEntity.Name;
        desEntity.Discription =srcEntity.Discription;
        desEntity.FkCategory =srcEntity.FkCategory;
        }
       /// <summary>  
       /// این متد یک کلاس انتیتی را به کلاس کانترکت معادل خود تبدیل می کند
       /// </summary>
       /// <param name=""></param>
       /// <returns></returns>
       public  CategoryContract EntityToContract(Category category)
       {
           var categoryContract = new CategoryContract()
           {

               Id = category.Id,


               Name = category.Name,


               Discription = category.Discription,

               FkCategory = category.FkCategory,
           };
        
           return categoryContract;
       }

       public IEnumerable<CategoryContract> EntityToContract(IEnumerable<Category> iEnumerableEntityContract)
       {
           foreach (var entity in iEnumerableEntityContract)
           {
               yield return this.EntityToContract(entity);
           }
       }
    
    
    }
    /// <summary>
    /// No Metadata Documentation available.
    /// </summary>
    
    public  partial class CategorySettingMaper : BaseMaper<CategorySetting>
    {
       public override void EntityToEntity(CategorySetting srcEntity, CategorySetting desEntity)
        {
        desEntity.Id =srcEntity.Id;
        desEntity.Name =srcEntity.Name;
        desEntity.GroupName =srcEntity.GroupName;
        desEntity.Discription =srcEntity.Discription;
        }
    
    
    
    }
    /// <summary>
    /// No Metadata Documentation available.
    /// </summary>
    
    public  partial class CommentMaper : BaseMaper<Comment>
    {
       public override void EntityToEntity(Comment srcEntity, Comment desEntity)
        {
        desEntity.Id =srcEntity.Id;
        desEntity.FkComment =srcEntity.FkComment;
        desEntity.KeyWord =srcEntity.KeyWord;
        desEntity.Comment1 =srcEntity.Comment1;
        desEntity.VotePositive =srcEntity.VotePositive;
        desEntity.ComputerIp =srcEntity.ComputerIp;
        desEntity.Active =srcEntity.Active;
        desEntity.FkProduct =srcEntity.FkProduct;
        desEntity.CommentType =srcEntity.CommentType;
        desEntity.VoteNegative =srcEntity.VoteNegative;
        }
    
    
    
    }
    /// <summary>
    /// No Metadata Documentation available.
    /// </summary>
    
    public  partial class ContentMaper : BaseMaper<Content>
    {
       public override void EntityToEntity(Content srcEntity, Content desEntity)
        {
        desEntity.Id =srcEntity.Id;
        desEntity.Name =srcEntity.Name;
        desEntity.ShowValue =srcEntity.ShowValue;
        desEntity.PartialType =srcEntity.PartialType;
        desEntity.Width =srcEntity.Width;
        desEntity.Position =srcEntity.Position;
        desEntity.ContentType =srcEntity.ContentType;
        }
    
    
    
    }
    /// <summary>
    /// No Metadata Documentation available.
    /// </summary>
    
    public  partial class DisputeResolutionMaper : BaseMaper<DisputeResolution>
    {
       public override void EntityToEntity(DisputeResolution srcEntity, DisputeResolution desEntity)
        {
        desEntity.Id =srcEntity.Id;
        desEntity.FkInvoice =srcEntity.FkInvoice;
        desEntity.FkUser =srcEntity.FkUser;
        desEntity.FkBusinessOwner =srcEntity.FkBusinessOwner;
        desEntity.FkMarketer =srcEntity.FkMarketer;
        desEntity.writer =srcEntity.writer;
        desEntity.Discription =srcEntity.Discription;
        desEntity.FileDocumentAdress =srcEntity.FileDocumentAdress;
        desEntity.Date =srcEntity.Date;
        desEntity.disputerwhoPerson =srcEntity.disputerwhoPerson;
        }
    
    
    
    }
    /// <summary>
    /// No Metadata Documentation available.
    /// </summary>
    
    public  partial class EntityMaper : BaseMaper<Entity>
    {
       public override void EntityToEntity(Entity srcEntity, Entity desEntity)
        {
        desEntity.Id =srcEntity.Id;
        desEntity.Name =srcEntity.Name;
        desEntity.PersianName =srcEntity.PersianName;
        }
    
    
    
    }
    /// <summary>
    /// No Metadata Documentation available.
    /// </summary>
    
    public  partial class FiledMaper : BaseMaper<Filed>
    {
       public override void EntityToEntity(Filed srcEntity, Filed desEntity)
        {
        desEntity.Id =srcEntity.Id;
        desEntity.Name =srcEntity.Name;
        desEntity.PartialType =srcEntity.PartialType;
        desEntity.OrderByValue =srcEntity.OrderByValue;
        desEntity.FkEntity =srcEntity.FkEntity;
        }
    
    
    
    }
    /// <summary>
    /// No Metadata Documentation available.
    /// </summary>
    
    public  partial class InvoiceMaper : BaseMaper<Invoice>
    {
       public override void EntityToEntity(Invoice srcEntity, Invoice desEntity)
        {
        desEntity.Id =srcEntity.Id;
        desEntity.Name =srcEntity.Name;
        desEntity.Date =srcEntity.Date;
        desEntity.State =srcEntity.State;
        desEntity.DeliveryAddress =srcEntity.DeliveryAddress;
        desEntity.MoneySum =srcEntity.MoneySum;
        desEntity.Discount =srcEntity.Discount;
        desEntity.Shipping =srcEntity.Shipping;
        desEntity.PaymentSum =srcEntity.PaymentSum;
        desEntity.PaymentToCountinue =srcEntity.PaymentToCountinue;
        desEntity.FkUser =srcEntity.FkUser;
        desEntity.FkBusinessOwner =srcEntity.FkBusinessOwner;
        desEntity.type =srcEntity.type;
        desEntity.DeliveryCode =srcEntity.DeliveryCode;
        desEntity.DeliveryCodedUnPerfect =srcEntity.DeliveryCodedUnPerfect;
        desEntity.NoteForBusinessOwner =srcEntity.NoteForBusinessOwner;
        desEntity.NoteForUser =srcEntity.NoteForUser;
        desEntity.FkMarketer =srcEntity.FkMarketer;
        desEntity.FkProvince =srcEntity.FkProvince;
        desEntity.VoteBusinessOwner =srcEntity.VoteBusinessOwner;
        desEntity.VoteMarketer =srcEntity.VoteMarketer;
        desEntity.Time =srcEntity.Time;
        desEntity.PaymentBankCode =srcEntity.PaymentBankCode;
        desEntity.TransctionRefrenceId =srcEntity.TransctionRefrenceId;
        desEntity.ShippingNumber =srcEntity.ShippingNumber;
        desEntity.ShippingCompany =srcEntity.ShippingCompany;
        desEntity.CommentForBusinessman =srcEntity.CommentForBusinessman;
        desEntity.CommentForMarketer =srcEntity.CommentForMarketer;
        desEntity.Vat =srcEntity.Vat;
        desEntity.writer =srcEntity.writer;
        }
    
    
    
    }
    /// <summary>
    /// No Metadata Documentation available.
    /// </summary>
    
    public  partial class LangugeMaper : BaseMaper<Languge>
    {
       public override void EntityToEntity(Languge srcEntity, Languge desEntity)
        {
        desEntity.Id =srcEntity.Id;
        desEntity.Name =srcEntity.Name;
        desEntity.Value =srcEntity.Value;
        desEntity.Title =srcEntity.Title;
        desEntity.FkFiled =srcEntity.FkFiled;
        }
    
    
    
    }
    /// <summary>
    /// No Metadata Documentation available.
    /// </summary>
    
    public  partial class MarketerMaper : BaseMaper<Marketer>
    {
       public override void EntityToEntity(Marketer srcEntity, Marketer desEntity)
        {
        desEntity.Id =srcEntity.Id;
        desEntity.Name =srcEntity.Name;
        desEntity.Specialty =srcEntity.Specialty;
        desEntity.WordKey =srcEntity.WordKey;
        desEntity.FkUser =srcEntity.FkUser;
        desEntity.FkMarketer =srcEntity.FkMarketer;
        desEntity.Image =srcEntity.Image;
        desEntity.WebSiteAddress =srcEntity.WebSiteAddress;
        desEntity.Tel =srcEntity.Tel;
        desEntity.Active =srcEntity.Active;
        desEntity.Yahoo =srcEntity.Yahoo;
        desEntity.Gmail =srcEntity.Gmail;
        desEntity.Skype =srcEntity.Skype;
        desEntity.PaymentPorsantAmount =srcEntity.PaymentPorsantAmount;
        }
    
    
    
    }
    /// <summary>
    /// No Metadata Documentation available.
    /// </summary>
    
    public  partial class MessageMaper : BaseMaper<Message>
    {
       public override void EntityToEntity(Message srcEntity, Message desEntity)
        {
        desEntity.Id =srcEntity.Id;
        desEntity.FkUseSender =srcEntity.FkUseSender;
        desEntity.FkUserReceiver =srcEntity.FkUserReceiver;
        desEntity.Date =srcEntity.Date;
        desEntity.Readed =srcEntity.Readed;
        desEntity.Text =srcEntity.Text;
        desEntity.type =srcEntity.type;
        }
    
    
    
    }
    /// <summary>
    /// No Metadata Documentation available.
    /// </summary>
    
    public  partial class ProductMaper : BaseMaper<Product>
    {
       public override void EntityToEntity(Product srcEntity, Product desEntity)
        {
        desEntity.Id =srcEntity.Id;
        desEntity.Name =srcEntity.Name;
        desEntity.Brand =srcEntity.Brand;
        desEntity.Image =srcEntity.Image;
        desEntity.Image1 =srcEntity.Image1;
        desEntity.Image2 =srcEntity.Image2;
        desEntity.Price =srcEntity.Price;
        desEntity.RegisterDate =srcEntity.RegisterDate;
        desEntity.Discription =srcEntity.Discription;
        desEntity.FkBusinessOwner =srcEntity.FkBusinessOwner;
        desEntity.MadeInCountry =srcEntity.MadeInCountry;
        desEntity.CountPrice =srcEntity.CountPrice;
        desEntity.MinCountForPrice =srcEntity.MinCountForPrice;
        desEntity.available =srcEntity.available;
        desEntity.PersentMarkater =srcEntity.PersentMarkater;
        desEntity.AcceptReturnDay =srcEntity.AcceptReturnDay;
        desEntity.ShippingDiscription =srcEntity.ShippingDiscription;
        desEntity.WordKey =srcEntity.WordKey;
        desEntity.SellOrBuy =srcEntity.SellOrBuy;
        desEntity.UnitBuy =srcEntity.UnitBuy;
        desEntity.wightUnitBuyWithKG =srcEntity.wightUnitBuyWithKG;
        desEntity.Active =srcEntity.Active;
        desEntity.FkCategory =srcEntity.FkCategory;
        desEntity.RankShow =srcEntity.RankShow;
        desEntity.FkContent =srcEntity.FkContent;
        }
    
    
    
    }
    /// <summary>
    /// No Metadata Documentation available.
    /// </summary>
    
    public  partial class ProvinceMaper : BaseMaper<Province>
    {
       public override void EntityToEntity(Province srcEntity, Province desEntity)
        {
        desEntity.Id =srcEntity.Id;
        desEntity.Name =srcEntity.Name;
        desEntity.Country =srcEntity.Country;
        }
    
    
    
    }
    /// <summary>
    /// No Metadata Documentation available.
    /// </summary>
    
    public  partial class RoleMaper : BaseMaper<Role>
    {
       public override void EntityToEntity(Role srcEntity, Role desEntity)
        {
        desEntity.Id =srcEntity.Id;
        desEntity.Name =srcEntity.Name;
        desEntity.Discription =srcEntity.Discription;
        desEntity.Type =srcEntity.Type;
        }
    
    
    
    }
    /// <summary>
    /// No Metadata Documentation available.
    /// </summary>
    
    public  partial class SettingMaper : BaseMaper<Setting>
    {
       public override void EntityToEntity(Setting srcEntity, Setting desEntity)
        {
        desEntity.Id =srcEntity.Id;
        desEntity.Name =srcEntity.Name;
        desEntity.Value =srcEntity.Value;
        desEntity.Value2 =srcEntity.Value2;
        desEntity.FkCategorySetting =srcEntity.FkCategorySetting;
        }
    
    
    
    }
    /// <summary>
    /// No Metadata Documentation available.
    /// </summary>
    
    public  partial class UserMaper : BaseMaper<User>
    {
       public override void EntityToEntity(User srcEntity, User desEntity)
        {
        desEntity.Id =srcEntity.Id;
        desEntity.Name =srcEntity.Name;
        desEntity.Email =srcEntity.Email;
        desEntity.Mobile =srcEntity.Mobile;
        desEntity.RegisterDate =srcEntity.RegisterDate;
        desEntity.FkRole =srcEntity.FkRole;
        desEntity.CurrentInvoice =srcEntity.CurrentInvoice;
        desEntity.Ative =srcEntity.Ative;
        desEntity.AtivationCode =srcEntity.AtivationCode;
        desEntity.Password =srcEntity.Password;
        desEntity.TempPassword =srcEntity.TempPassword;
        desEntity.IpComputerCreator =srcEntity.IpComputerCreator;
        desEntity.IpComputerLast =srcEntity.IpComputerLast;
        }
    
    
    
    }
}