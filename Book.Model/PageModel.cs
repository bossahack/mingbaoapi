﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Book.Model
{
    public class Page<T>
    {
        public int Total { get; set;}

        public List<T> Items { get; set; }
    }

    public class PageSearch
    {
        public int PageIndex { get; set; } 
        public int PageSize { get; set; }
    }
}
