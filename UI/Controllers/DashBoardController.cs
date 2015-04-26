using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Newtonsoft.Json.Linq;
using UI.Controllers.DTOs;

namespace UI.Controllers
{
    public class DashBoardController : ApiController
    {
        // GET: api/DashBoard
        public DashBoardDTO Get()
        {          
            var httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri("http://127.0.0.1:2113/projection/TotalMoneyInTheBank/state");
            var responseMessage = httpClient.GetAsync("");

            var responseBody = responseMessage.Result.Content.ReadAsStringAsync();

            if (responseBody.Result != "Not Found")
            {

                JObject o = JObject.Parse(responseBody.Result);

                return new DashBoardDTO { TotalMoneyInBank = (double)o["TotalMoney"] };
            }
            else
            { 
            return new DashBoardDTO { TotalMoneyInBank = 0 };
            
            }

        }


        // GET: api/DashBoard/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/DashBoard
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/DashBoard/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/DashBoard/5
        public void Delete(int id)
        {
        }
    }
}
