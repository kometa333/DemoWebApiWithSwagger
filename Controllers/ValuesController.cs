using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using DemoWebApiWithSwagger.Models;

namespace DemoWebApiWithSwagger.Controllers
{
    public class ValuesController : ApiController
    {

        //store items in memory
        private static List<CustomListItem> _listItems { get; set; } = new List<CustomListItem>();

        /// <summary>
        /// Lists all the data from memory 
        /// </summary>
        public IEnumerable<CustomListItem> Get()
        {
            return _listItems;
        }

        /// <summary>
        /// Gets item by Id. 
        /// </summary>
        /// /// <param name="id">Enter the id as a number</param>
        /// <returns>An collection of items</returns>
        public HttpResponseMessage Get(int id)
        {
            var item = _listItems.FirstOrDefault(x => x.Id == id);
            if (item != null)
            {
                return Request.CreateResponse(HttpStatusCode.OK, item);
            }
            return Request.CreateResponse(HttpStatusCode.NotFound);
        }

        /// <summary>
        /// Gets the number of sold articles by Date. 
        /// </summary>
        /// <param name="date">Enter the date in the following format dd-MM-yyyy.</param>
        /// <returns>A number</returns>
        /// <example>{06-10-2020}</example>

        [Route("GetNoSoldArticlesByDate/{date}")]
        public HttpResponseMessage GetNoSoldArticlesByDate(String date)
        {
            var item = _listItems.Where(x => x.Date.ToString("dd-MM-yyyy") == date).Count();
            
            if (item !=0)
            {
                return Request.CreateResponse(HttpStatusCode.OK, "Number of sold articles per day: " + item);
            }
            return Request.CreateResponse(HttpStatusCode.NotFound);
        }

        /// <summary>
        /// Gets the total revenue per date. 
        /// </summary>
        /// <param name="date">Enter the date in the following format dd-MM-yyyy.</param>
        /// <returns>A number</returns>
        /// <example>{06-10-2020}</example>

        [Route("GetTotalRevenueByDate/{date}")]
        public HttpResponseMessage GetTotalRevenueByDate(String date)
        {
            var item = _listItems.Where(x => x.Date.ToString("dd-MM-yyyy") == date).Sum(x=>x.SalesPrice);

            if (item != 0)
            {
                return Request.CreateResponse(HttpStatusCode.OK, "Total revenue per day: €" + item);
            }
            return Request.CreateResponse(HttpStatusCode.NotFound);
        }

        /// <summary>
        /// Gets total revenue grouped by articles. 
        /// </summary>
        /// <returns>A number</returns>
        
        [Route("GetRevenueByArticles/")]
        public HttpResponseMessage GetRevenueByArticles()
        {
            var itemList = _listItems.GroupBy(x => new { x.ArticleNumber }).Select(group => new { 
                ArticleNo = group.Key.ArticleNumber,
                TotalSalesPrice = group.Sum(c => c.SalesPrice)});

            if (itemList.Count() != 0)
            {
                StringBuilder responseBuilder = new StringBuilder();

                foreach (var item in itemList)
                {
                    responseBuilder.Append("[ArticleNo: " + item.ArticleNo + ", TotalSalesPrice: €" + item.TotalSalesPrice + "]  ");
                }
                return Request.CreateResponse(HttpStatusCode.OK, responseBuilder.ToString());
            }
            return Request.CreateResponse(HttpStatusCode.NotFound);
        }

        /// <summary>
        /// Makes a post to memory 
        /// </summary>
        /// <param name="model">Enter the price as double and ArticleNumber as a string up to 32 characters long</param>
        /// <example>"Id": 0, "ArticleNumber": "AN1", "SalesPrice": 10, "Date": "2020-10-07T12:15:10.179Z"}</example>
        /// 
        public HttpResponseMessage Post([FromBody] CustomListItem model)
        {
            if (ModelState.IsValid && model != null)
            {
                var maxId = 0;
                if (_listItems.Count > 0)
                {
                    maxId = _listItems.Max(x => x.Id);
                }
                model.Id = maxId + 1;
                _listItems.Add(model);

            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }

            return Request.CreateResponse(HttpStatusCode.Created, model);

        }

        
    }
}
