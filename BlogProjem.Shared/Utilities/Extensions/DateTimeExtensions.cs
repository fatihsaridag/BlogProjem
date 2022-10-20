using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogProjem.Shared.Utilities.Extensions
{
     public  static class DateTimeExtensions           //Extend işlemleriyle ilgili methodlarımız her zaman static olmalı.
    {
        public static string FullDateAndTimeStringWithUnderscore(this DateTime dateTime)      //Buradaki işlem tüm zaman ve tarih bilgisini bizlere alt treler ile veriyor olacak. String olacak ve bu değeri geri alacağız.
        {
            return $"{dateTime.Millisecond}_{dateTime.Second}_{dateTime.Minute}_{dateTime.Hour}_{dateTime.Day}_{dateTime.Month}_{dateTime.Year}";
            /*FatihSaridag_587_5_38_12_3_10_2022.png*/


        }
    }
}
