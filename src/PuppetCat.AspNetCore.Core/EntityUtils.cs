using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Linq;

namespace PuppetCat.AspNetCore.Core
{
    public class EntityUtils
    {
        /// <summary>
        /// Copy value to another List<Model>, their fields name must be same
        /// </summary>
        /// <typeparam name="From"></typeparam>
        /// <typeparam name="To"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public static List<To> CopyToList<From, To>(List<From> source)
            where From : class, new()
            where To : class, new()
        {
            List<To> t2List = new List<To>();
            PropertyInfo[] pi = typeof(To).GetProperties();
            PropertyInfo[] pi1 = typeof(From).GetProperties();

            foreach (From t1Model in source)
            {
                To model = CopyToModel<From, To>(t1Model);
                //for (int i = 0; i < pi.Length; i++)
                //{
                //    PropertyInfo pi1Proerty
                //    pi[i].SetValue(model, pi1.GetValue(t1Model, null), null);
                //}
                t2List.Add(model);
            }
            return t2List;
        }

        /// <summary>
        /// Copy value to another Model, their fields name must be same
        /// </summary>
        /// <typeparam name="From"></typeparam>
        /// <typeparam name="To"></typeparam>
        /// <param name="source"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public static To CopyToModel<From, To>(From source, To model)
            where From : class, new()
            where To : class, new()
        {
            if (null != source)
            {
                PropertyInfo[] pi = typeof(To).GetProperties();
                PropertyInfo[] pi1 = typeof(From).GetProperties();
                for (int i = 0; i < pi.Length; i++)
                {
                    string propertyName = pi[i].Name;

                    PropertyInfo pi1Property = pi1.FirstOrDefault(a => a.Name.ToLower() == propertyName.ToLower());
                    if (null != pi1Property)
                    {
                        object value = pi1Property.GetValue(source, null);
                        if (null != value)
                        {
                            pi[i].SetValue(model, value, null);
                        }
                    }
                }
            }
            return model;
        }

        /// <summary>
        /// Copy value to another Model, their fields name must be same
        /// </summary>
        /// <typeparam name="From"></typeparam>
        /// <typeparam name="To"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public static To CopyToModel<From, To>(From source)
            where From : class, new()
            where To : class, new()
        {
            To model = new To();
            CopyToModel<From, To>(source, model);
            return model;
        }
    }
}
