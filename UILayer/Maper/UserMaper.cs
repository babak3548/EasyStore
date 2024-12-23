using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DataLayer;
using DataLayer.Contract;
using DataLayer.EF;

namespace UILayer.Maper
{
    public partial class UserMaper : BaseMaper<User, UserContract>
    {
        public  User ContractToEntity(UserContract userContract)
        {
            var user = new User()
            {

                Id = userContract.Id,


       


                Mobile = userContract.Mobile,


        


                CityName = userContract.CityName,


                Address = userContract.Address,


                RegisterDate = userContract.RegisterDate,
            
             
                FkProvince = userContract.FkProvince,



                FkRole = userContract.FK_Role,

                IpComputer = userContract.IpComputer,


              

            };
            PartialMethodEntityToContract(ref  user, ref  userContract);
            return user;
        }
    }
    
}