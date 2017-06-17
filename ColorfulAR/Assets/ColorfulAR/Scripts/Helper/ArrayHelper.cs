using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;//.Tasks;


namespace GJM.Helper
{
    class ArrayHelper
    {

        /// <summary> T:给我什么类型的数组.Tkey:你要比较什么
        /// </summary>
        /// <param name="array"></param>
        /// <param name="handler"></param>
        /// <returns></returns>
        public static void OrderBy<T, Tkey>(T[] array, Func<T, Tkey> handler) where Tkey : IComparable, IComparable<Tkey>
        {
            
            for (int r = 0; r < array.Length; r++)
            {
                int midIndex = 0;
                for (int i = 1; i < array.Length - r; i++)
                {
                    if (handler(array[midIndex]).CompareTo(handler(array[i])) == -1)
                    {
                        midIndex = i;
                    }
                }
                T midNumber=array[midIndex];
                array[midIndex] = array[array.Length - 1 - r];
                array[array.Length - 1 - r] = midNumber;
            }
            //return array;
        }
        public static void OrderByDescending<T, Tkey>(T[] array, Func<T, Tkey> handler) where Tkey : IComparable, IComparable<Tkey>
        {
            for (int r = 0; r < array.Length; r++)
            {
                int midIndex = 0;
                for (int i = 1; i < array.Length - r; i++)
                {
                    if (handler(array[midIndex]).CompareTo(handler(array[i])) == 1)
                    {
                        midIndex = i;
                    }
                }
                T midNumber = array[midIndex];
                array[midIndex] = array[array.Length - 1 - r];
                array[array.Length - 1 - r] = midNumber;
            }
        }
        public static T OrderByMax<T, Tkey>(T[] array, Func<T, Tkey> handler) where Tkey : IComparable, IComparable<Tkey>
        {
          int midIndex = 0;
          for (int i = 1; i < array.Length ; i++)
          {
              if (handler(array[midIndex]).CompareTo(handler(array[i])) == -1)
              {
                  midIndex = i;
              }
          }
          return array[midIndex];
        }
        public static T OrderByMin<T, Tkey>(T[] array, Func<T, Tkey> handler) where Tkey : IComparable, IComparable<Tkey>
        {
            int midIndex = 0;
            for (int i = 1; i < array.Length; i++)
            {
                if (handler(array[midIndex]).CompareTo(handler(array[i])) == 1)
                {
                    midIndex = i;
                }
            }
            return array[midIndex];
        }
        public static T OrderByFind<T>(T[] array, Func<T, bool> handlerTarget) 
        {
            for (int i = 1; i < array.Length; i++)
            {
                if (handlerTarget(array[i]))
                {
                    return array[i];
                }
            }
            return default(T);
        }
        public static T[] OrderByFindAll<T>(T[] array, Func<T,bool> handlerTarget) 
        {
            List<T> list = new List<T>();
            for (int i = 1; i < array.Length; i++)
            {
                if (handlerTarget(array[i]))
                {
                    list.Add(array[i]);
                }
            }
            return list.ToArray();
        }
        /// <summary> 从原来对象的集合中提取每个对象的某个属性值做成一个新的数组
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="Tkey"></typeparam>
        /// <param name="array"></param>
        /// <param name="handler"></param>
        /// <returns></returns>
        public static Tkey[] Select<T, Tkey>(T[] array, Func<T, Tkey> handler)
        {
            List<Tkey> list = new List<Tkey>();
            for (int i = 0; i < array.Length; i++)
            {
                list.Add(handler(array[i]));
            }
            return list.ToArray();
        }
    }
}
