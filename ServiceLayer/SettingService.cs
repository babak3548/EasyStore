using DataLayer.EF;
using DataLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ServiceLayer
{
  public  class SettingService :  BaseService<Setting>
    {
        public SettingService(OnlineShopping onlineShopping) : base(onlineShopping)
        {

        }

        static List<SettingLigthModel> settingLigths;

        public string GetSettingValueByKey(string key)
        {
            if (settingLigths == null || settingLigths.Count > 0 )
            {
                // دریافت فقط 1000 تایی اول به خاطر ایجاد محدویت روی رم سرور است
                settingLigths = GetAll().Select(s=> new SettingLigthModel {Name=s.Name, Value=s.Value }).Take(1000).ToList();
            }

            SettingLigthModel settingLigthModel= settingLigths.FirstOrDefault(o => o.Name == key);
            return settingLigthModel.Value;
        }

        public void ClearCache()
        {
            settingLigths = null;

          //  throw new NotImplementedException();
        }
    }


}
