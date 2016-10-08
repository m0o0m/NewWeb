using GL.Data.BLL;
using GL.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Web.Http;

namespace MWeb.Controllers
{
    [Authorize]
    public class CommonProblemsApiController : ApiController
    {


        public object Delete([FromBody]FAQ model)
        {
            int result = FAQBLL.Delete(model);
            if (result > 0)
            {
                return new { result = 0 };
            }
            return new { result = 1 };

        }


        public object Put([FromBody]FAQ model)
        {

            if (string.IsNullOrWhiteSpace(model.faqtitle) || string.IsNullOrWhiteSpace(model.faqcontent))
            {
                return new { result = Result.ValueCanNotBeNull };
            }

            if (!Regex.IsMatch(model.faqtitle, @"^[\u0391-\uFFE5a-zA-Z_]\w{3,64}"))
            {
                return new { result = Result.AccountOnlyConsistOfLettersAndNumbers };
            }
            if (!Regex.IsMatch(model.faqcontent, @"^[\u0391-\uFFE5a-zA-Z_]\w{3,300}"))
            {
                return new { result = Result.AccountOnlyConsistOfLettersAndNumbers };
            }




            model.faqtype = 1;
            model.operdate = DateTime.Now;



            int result = FAQBLL.Add(model);
            if (result > 0)
            {
                return new { result = 0 };
            }

            return new { result = 4 };



        }


        public object Options([FromBody]FAQ model)
        {

            if (string.IsNullOrWhiteSpace(model.faqtitle) || string.IsNullOrWhiteSpace(model.faqcontent))
            {
                return new { result = Result.ValueCanNotBeNull };
            }

            if (!Regex.IsMatch(model.faqtitle, @"^[\u0391-\uFFE5a-zA-Z_]\w{3,64}"))
            {
                return new { result = Result.AccountOnlyConsistOfLettersAndNumbers };
            }
            if (!Regex.IsMatch(model.faqcontent, @"^[\u0391-\uFFE5a-zA-Z_]\w{3,300}"))
            {
                return new { result = Result.AccountOnlyConsistOfLettersAndNumbers };
            }

            FAQ mi = FAQBLL.GetModelByID(model);
            mi.faqtitle = model.faqtitle;
            mi.faqcontent = model.faqcontent;


            int result = FAQBLL.Update(mi);
            if (result > 0)
            {
                return new { result = 0 };
            }

            return new { result = 4 };



        }


    }
}
