using System;
using System.Collections.Generic;
using System.Text;

namespace Utility
{
    public class Pagination
    {
        public static Pagination DefaultInstance => Create(50);

        public static Pagination Create(int pageIndex)
        {
            return new Pagination(50, pageIndex);
        }

        public static Pagination Create(int pageIndex, int pageSize)
        {
            return new Pagination(pageSize, pageIndex);
        }

        private Pagination(int pageSize, int pageIndex)
        {
            PageSize = pageSize;
            PageIndex = pageIndex;
        }

        public int PageSize { get; set; }
        public int PageIndex { get; set; }
    }
}
