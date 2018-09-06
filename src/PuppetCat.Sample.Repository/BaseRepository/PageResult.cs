using System;
using System.Collections.Generic;
using System.Text;

namespace PuppetCat.Sample.Repository
{
    /// <summary>
    /// Paging return data
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class PageResult<T>
    {
        public int ItemCount { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public int PageCount
        {
            get
            {
                try
                {
                    int m = ItemCount % PageSize;
                    if (m == 0)
                        return ItemCount / PageSize;
                    else
                        return ItemCount / PageSize + 1;
                }
                catch
                {
                    return 0;
                }
            }
        }

        public List<T> Data { get; set; }
    }
}
