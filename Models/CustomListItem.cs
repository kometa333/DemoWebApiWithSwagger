using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DemoWebApiWithSwagger.Models
{
    public class CustomListItem
    {
        public int Id { get; set; }

        [Required, StringLength(32, MinimumLength = 1)]
        public string ArticleNumber { get; set; }

        public double SalesPrice { get; set; }

        public DateTime Date { get; set; }
    }

}