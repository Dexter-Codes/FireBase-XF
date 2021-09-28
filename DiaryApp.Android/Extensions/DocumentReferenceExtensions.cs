using System;
using System.Collections.Generic;
using Firebase.Firestore;
using DiaryApp.Services;
using Java.Text;
using System.Globalization;

namespace DiaryApp.Droid.Extensions
{
    public static class DocumentReferenceExtensions
    {
        public static T Convert<T>(this DocumentSnapshot doc) where T : IIdentifiable
        {
            //try
            //{
            //    var jsonStr = Newtonsoft.Json.JsonConvert.SerializeObject(doc.Data.ToDictionary());
            //    var item = Newtonsoft.Json.JsonConvert.DeserializeObject<T>(jsonStr);
            //    item.Id = doc.Id;
            //    return item;
            //}
            //catch (Exception ex)
            //{
            //    System.Diagnostics.Debug.WriteLine("EXCEPTION THROWN");
            //}
            //return default;

            var dict = new Dictionary<string, object>();
            var docDict = doc.Data;
            foreach (var key in docDict.Keys)
            {
                var val = docDict[key];
                if (val is Java.Lang.String str)
                {
                    dict[key.ToString()] = str.ToString();
                }
                else if (val is Java.Lang.Double num)
                {
                    var numStr = num.ToString();
                    if (numStr.Contains("."))
                    {
                        dict[key.ToString()] = num.DoubleValue();
                    }
                    else
                    {
                        dict[key.ToString()] = num.IntValue();
                    }
                }
                else if (val is Java.Util.Date time)
                {
                    SimpleDateFormat rformat = new SimpleDateFormat("yyyy-MM-dd HH:mm:ss ");//Format text as what ParseExact method can do 
                    DateTime dt1 = DateTime.ParseExact(rformat.Format(time).ToString(), "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);//plz become Datetime!!! And don't give me a runtime error here
                    dict[key.ToString()] = dt1;


                 //   var formatter = new SimpleDateFormat("yyyy-MM-dd'T'HH:mm:ssZZZZZ");
                   // formatter.DateFormat = "yyyy-MM-dd'T'HH:mm:ssZZZZZ";
                //    dict[key.ToString()] = DateTime.Parse(formatter.ToString(time));
                }
            }
            dict["Id"] = doc.Id;
            var tJson = Newtonsoft.Json.JsonConvert.SerializeObject(dict);
            return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(tJson);


        }

        public static List<T> Convert<T>(this QuerySnapshot docs) where T : IIdentifiable
        {
            var list = new List<T>();
            foreach (var doc in docs.Documents)
            {
                list.Add(doc.Convert<T>());
            }
            return list;
        }
    }
}
