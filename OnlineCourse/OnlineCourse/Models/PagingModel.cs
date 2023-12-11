using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnlineCourse.Models
{
    public class PagingModel
    {
        public int currentPage { get; set; }
        public int countPages { get; set; }

        public Func<int?, string> generateUrl { get; set; }
    }
}