UPDATE      TOP (1000) Shapping.Category
SET                Active = 1
FROM            Shapping.Category LEFT OUTER JOIN
                         Shapping.Product ON Shapping.Category.Id = Shapping.Product.FK_Category
WHERE        (Shapping.Product.Id IS not NULL)

-------------------------------
SELECT TOP (1000) [Id]
      ,[Name]
      ,[Date]
      ,[State]
      ,[DeliveryAddress]
      ,[MoneySum]
      ,[Discount]
      ,[Shipping]
      ,[PaymentSum]
      ,[PaymentToCountinue]
      ,[FK_User]
      ,[FK_BusinessOwner]
      ,[type]
      ,[DeliveryCode]
      ,[DeliveryCodedUnPerfect]
      ,[NoteForBusinessOwner]
      ,[NoteForUser]
      ,[FK_Marketer]
      ,[FK_Province]
      ,[VoteBusinessOwner]
      ,[VoteMarketer]
      ,[Time]
      ,[PaymentBankCode]
      ,[TransctionRefrenceId]
      ,[ShippingNumber]
      ,[ShippingCompany]
      ,[CommentForBusinessman]
      ,[CommentForMarketer]
      ,[Vat]
      ,[writer]
      ,[TypeShipping]
      ,[TypeSell]
  FROM [OnlineShopping].[Accounting].[Invoice]

  update [OnlineShopping].[Accounting].[Invoice]
  set Vat = 0 , [ShippingCompany]= 0

    update [OnlineShopping].[Accounting].[Invoice]
  set [PaymentToCountinue]= 0
  where [PaymentToCountinue] is null

      update [OnlineShopping].[Accounting].[Invoice]
  set PaymentBankCode= 0
  where PaymentBankCode is null

        update [OnlineShopping].[Accounting].[Invoice]
  set FK_Province= 8
  where FK_Province is null

 update [OnlineShopping].[Accounting].[Invoice]
  set FK_Marketer=16
  where FK_Marketer is null  -- باید بتونه نال بگیره
   
  
 update [OnlineShopping].[Accounting].[Invoice]
  set Discount=0
  where Discount is null 

  -------------------------------2020-12-05
  
  update [EasyStore].[Miscellaneous].[Province]
  set [Fk_Province] = null

    delete [EasyStore].[Miscellaneous].[Province]
where Id in (1,33)
--------------

    BEGIN TRANSACTION
  update [EasyStore].[Shapping].[Product] 
  set [Image2]= ','+[Image2] +','+[Image3] +','+[Image4]
  where Id <>4605
   commit TRANSACTION 


   ------------ به روز رسانی گروه کالای خاص
     update [OnlineShopping].[Shapping].[Product]
 -- [Price] = cast ([Price]  +([Price] *  12 / 100 ) /10000 as int) * 10000 ,
set   [BeforDiscountPrice] = cast ([Price]  +([Price] *  8 / 100 ) /10000 as int) * 10000 
   where FK_Category = 5 and [Name] =  N'%یلدا%'

   update [EasyStore].[Shapping].[Product]
 set [Price] = [Price]  + convert (int , ([Price] *  1 / 100 /10000)) *10000 , [BeforDiscountPrice] = [Price]  + convert (int , ([Price] *  1 / 100 /10000)) *10000
 where FK_Category in(697, 699 ) --and [Name] =  N'%یلدا%'  ok
 ---------------------------

   --------------------------------
SELECT        TOP (1000) PaymentLogs.Description, PaymentLogs.Status, PaymentLogs.CreateDate, PaymentLogs.UpdateDate, PaymentLogs.Amount, Accounting.Invoice.FK_User, Accounting.Invoice.PaymentToCountinue, 
                         Accounting.Invoice.ShippingCost, Accounting.Invoice.Status AS Expr1, Accounting.Invoice.UpdateDate AS Expr2, Accounting.Invoice.HistoryStateAndDescription, Accounting.Bridge_Invoice_Product.Price, 
                         Accounting.Bridge_Invoice_Product.Colore, Accounting.Bridge_Invoice_Product.Image, Accounting.Bridge_Invoice_Product.Count, Accounting.Bridge_Invoice_Product.BeforDiscountPrice, 
                         Accounting.Bridge_Invoice_Product.BuyPrice
FROM            Accounting.Bridge_Invoice_Product INNER JOIN
                         Accounting.Invoice ON Accounting.Bridge_Invoice_Product.FK_Invoice = Accounting.Invoice.Id LEFT OUTER JOIN
                         PaymentLogs ON Accounting.Invoice.Id = PaymentLogs.InvoiceId
------------------------------------
  update [EasyStore].[Shapping].[Product]
 set [Price] =  [Price] + 150000
 ---------------------
 
  update [EasyStore].[Shapping].[Product] 
  set AddDate = cast([RegisterDate] as datetime2(7)) ,UpdateDate =cast([RegisterDate] as datetime2(7))

  update [EasyStore].[Shapping].[Product]
set[BeforDiscountPrice]=[Price] , [Price] =  convert (decimal ,([Price]/1.3 / 10000) )  * 10000 
  where   [UpdateDate]<=  '2022-01-22'And MinCountForPrice is null

      BEGIN TRANSACTION
update [EasyStore].[Shapping].[Product]
set MinCountForPrice =  convert (int ,5000000/ ([Price]/4 ) )
  where   [UpdateDate]<=  '2022-01-22'And MinCountForPrice is null

   commit TRANSACTION

     update [EasyStore].[Shapping].[Product]
  set [MinCountForPrice]= convert(int,[MinCountForPrice]/5)*5 
    update [EasyStore].[Shapping].[Product]
  set [MinCountForPrice]=5
  where [MinCountForPrice] =0

  ----------------- به روز رسانی قیمت
begin tran
update [EasyStore].[Shapping].[Product]
--set [Price] =   convert (decimal ,([Price]*1.3 / 10000) )  * 10000
set [BeforDiscountPrice] =   convert (decimal ,([BeforDiscountPrice]*1.3 / 10000) )  * 10000
  where FK_Category = 701
commit tran
----------------------------------به روز رسانی تلفن از توضیحات محصولات
SELECT TOP (1000) [Id]
 ,[Discription]
   ,  replace(Discription, '77656227', '66955385') as cc
  FROM [EasyStore].[Shapping].[Product]
  where Discription like N'%66955385%'

  begin tran
  update [EasyStore].[Shapping].[Product]
   set Discription =  replace(Discription, '77656227', '66955385')
   where Discription like N'%77656227%'
   commit tran
--------------------------------