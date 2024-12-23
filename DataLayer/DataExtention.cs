//using System.Data;
//using Microsoft.AspNetCore.Mvc;
//using System.Data.Objects;
//using System.Linq;
//using System.Data.Objects.DataClasses;
//using System.ComponentModel;
//using System.Runtime.Serialization;
//using System.IO;
//using System.Reflection;
//using System.Xml.Serialization;
//using System.Xml;
//using System;
//using System.Collections.Generic;
//using System.Linq.Expressions;
//using System.Collections.ObjectModel;
//using System.Xml.Linq;
//using DataLayer.Enums;
//using DataLayer.Contract;
//using DataLayer.EF;
//using Microsoft.EntityFrameworkCore;

//namespace DataLayer
//{

//    public static class DataExtension
//    {

//        public static void RejectAll(this OnlineShopping context)
//        {
//            context.RejectAll();
//        }
//        //public static void RejectChange(this IQueryable objSet)
//        //{
//        //    foreach (var item in objSet)
//        //    {
//        //        EntityObject obj = item as EntityObject;
//        //        if (obj.EntityState == EntityState.Deleted || obj.EntityState == EntityState.Added || obj.EntityState == EntityState.Modified)
//        //            (item as EntityObject).RejectChange();
//        //    }

//        //}


//        public static EntityObject CloneEntity(this EntityObject obj)
//        {
//            DataContractSerializer dcSer = new DataContractSerializer(obj.GetType());
//            MemoryStream memoryStream = new MemoryStream();

//            dcSer.WriteObject(memoryStream, obj);
//            memoryStream.Position = 0;

//            EntityObject newObject = (EntityObject)dcSer.ReadObject(memoryStream);
//            dcSer = null;
//            memoryStream = null;
//            return newObject;
//        }
//        public static string GetEntryValueInString(this ObjectStateEntry entry, bool isOrginal)
//        {
//            if (entry.Entity is EntityObject)
//            {
//                object target = ((EntityObject)entry.Entity).CloneEntity();
//                foreach (string propName in entry.GetModifiedProperties())
//                {
//                    object setterValue = null;
//                    if (isOrginal)
//                    {
//                        //Get orginal value 
//                        setterValue = entry.OriginalValues[propName];
//                    }
//                    else
//                    {
//                        //Get orginal value 
//                        setterValue = entry.CurrentValues[propName];
//                    }
//                    //Find property to update 
//                    PropertyInfo propInfo = target.GetType().GetProperty(propName);
//                    //update property with orgibal value 
//                    if (setterValue == DBNull.Value)
//                    {//
//                        setterValue = null;
//                    }
//                    propInfo.SetValue(target, setterValue, null);
//                }//end foreach

//                XmlSerializer formatter = new XmlSerializer(target.GetType());
//                XDocument document = new XDocument();

//                using (XmlWriter xmlWriter = document.CreateWriter())
//                {
//                    formatter.Serialize(xmlWriter, target);
//                }
//                return document.Root.ToString();
//                //return Serializer.Serialize(target);
//            }
//            return null;
//        }
//        public static int GetMaxValue<T>(this OnlineShopping contex, string keyFieldName)
//        {

//            if (null != contex)
//            {
//                try
//                {
//                    // First we define the parameter that we are going to use the clause.
//                    var param = Expression.Parameter(typeof(T), typeof(T).Name);
//                    // Now we’ll make our lambda function that returns the "ID" property .
//                    var maxExpression = Expression.Lambda<Func<T, int>>(Expression.Property(param, keyFieldName), param);
//                    // Now I can get desire entityset from context using reflection.
//                    System.Data.Objects.ObjectQuery<T> query = contex.GetType().GetProperties().Where(p => p.PropertyType.BaseType.Equals(typeof(System.Data.Objects.ObjectQuery)) && p.PropertyType.GetGenericArguments().First().Equals(typeof(T))).First().GetValue(contex, null) as System.Data.Objects.ObjectQuery<T>;

//                    if (null != query)
//                    {
//                        int? i = query.Max<T, int>(maxExpression);
//                        if (i.HasValue)
//                            return i.Value;
//                    }
//                }

//                catch (Exception) { }

//            }

//            return 0;

//        }
//        /// <summary>
//        /// Select Entities with Condition
//        /// </summary>
//        /// <param name="selectExpression">Selection Condition</param>
//        /// <returns>Collection of Entites</returns>
//        public static ObservableCollection<T> GetEntity<T>(this OnlineShopping contex, Expression<Func<T, bool>> selectExpression)
//            where T : EntityObject, new()
//        {
//            // Now I find out the object query
//            System.Data.Objects.ObjectQuery<T> query = contex.GetType().GetProperties().Where(p => p.PropertyType.BaseType.Equals(typeof(System.Data.Objects.ObjectQuery))
//                                                                                            && p.PropertyType.GetGenericArguments().DefaultIfEmpty(null).First()
//                                                                                            .Equals(typeof(T))).DefaultIfEmpty(null).First()
//                                                                                            .GetValue(contex, null) as System.Data.Objects.ObjectQuery<T>;

//            if (null != query)
//            {//return query result  
//                return null;// new ObservableCollection<T>(query.Where(selectExpression));
//            }//end if 
//            return null;
//        }

//        public static void MarkAtModified(this OnlineShopping context, EntityObject entity, IEnumerable<string> modifiedProperties)
//        {
//            // Define an ObjectStateEntry and EntityKey for the current object.
//            EntityKey key;
//            object originalItem;
//            // Get the detached object's entity key.
//            if (entity.EntityKey == null)
//            {
//                // Get the entity key of the updated object.
//                key = context.CreateEntityKey(entity.GetType().Name, entity);
//            }
//            else
//            {
//                key = entity.EntityKey;
//            }
//            try
//            {
//                // Get the original item based on the entity key from the context
//                // or from the database.
//                if (context.TryGetObjectByKey(key, out originalItem))
//                {//accept the changed property
//                    if (originalItem is EntityObject)
//                    {
//                        ObjectStateEntry entry = context.ObjectStateManager.GetObjectStateEntry(key);

//                        if (null != modifiedProperties)
//                        {
//                            foreach (var pro in modifiedProperties)
//                            {
//                                entry.SetModifiedProperty(pro);
//                            }
//                        }

//                        if (entry.State == EntityState.Unchanged)
//                        {
//                            entry.SetModified();
//                        }//end if 
//                    }//end if 
//                }
//            }
//            catch (System.Data.MissingPrimaryKeyException ex)
//            {
//                throw ex;
//            }
//            catch (System.Data.MappingException ex)
//            {
//                throw ex;
//            }
//            catch (System.Data.DataException ex)
//            {
//                throw ex;
//            }
//        }
//        public static void MarkAtEntityState(this OnlineShopping context, EntityObject entity, EntityState state)
//        {
//            // Define an ObjectStateEntry and EntityKey for the current object.

//            ObjectStateEntry entry = context.ObjectStateManager.GetObjectStateEntry(entity);
//            if (entry != null)
//            {
//                entry.ChangeState(state);
//            }
//        }

//        public static EntityObject EditEntity(this OnlineShopping context, ref EntityObject entity)
//        {
//            // Define an ObjectStateEntry and EntityKey for the current object.
//            EntityKey key;
//            object originalItem;
//            // Get the detached object's entity key.
//            if (entity.EntityKey == null)
//            {
//                // Get the entity key of the updated object.

//                key = context.CreateEntityKey(context.GetEntitSetName(entity).Name, entity);

//            }
//            else
//            {
//                key = entity.EntityKey;
//            }
//            try
//            {
//                // Get the original item based on the entity key from the context
//                // or from the database.
//                if (context.TryGetObjectByKey(key, out originalItem))
//                {//accept the changed property

//                    // Call the ApplyPropertyChanges method to apply changes
//                    // from the updated item to the original version.
//                    return context.ApplyCurrentValues(key.EntitySetName, entity);

//                }
//                else
//                {//add the new entity
//                    context.AddObject(key.EntitySetName, entity);
//                    return entity;
//                    //context.MarkAtEntityState(entity,EntityState.Modified);
//                }//end else
//            }
//            catch (System.Data.MissingPrimaryKeyException ex)
//            {
//                throw ex;
//            }
//            catch (System.Data.MappingException ex)
//            {
//                throw ex;
//            }
//            catch (System.Data.DataException ex)
//            {
//                throw ex;
//            }
//        }

//        public static EntitySetBase GetEntitSetName(this OnlineShopping context, EntityObject entity)
//        {

//            Type entityType = entity.GetType();
//            while (entityType.BaseType != typeof(EntityObject) && entityType.BaseType != typeof(EntityObject))
//                entityType = entityType.BaseType;
//            EntityContainer container = context.MetadataWorkspace.GetEntityContainer(context.DefaultContainerName, DataSpace.CSpace);

//            EntitySetBase entitySet = container.BaseEntitySets.Where(item => item.ElementType.Name.Equals(entityType.Name)).FirstOrDefault();

//            return entitySet; ;
//        }


//        public static string GetSestionUserIdGeustAndUsers(this UserContract userContract)
//        {
//            string sestionUserId = "";
//            if (userContract.Id == ConstSetting.UserIdGuest)
//            { sestionUserId = userContract.TempPassword; }
//            else { sestionUserId = userContract.Id.ToString(); }
//            return sestionUserId;
//        }
//    }
//}
