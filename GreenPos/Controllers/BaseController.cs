using GreenPOS.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;

namespace GreenPOS.Controllers
{
    public class BaseController : Controller
    {
        public long UserId
        {
            get
            {
                var userId = HttpContext.Session.GetString(Constants.UserId.ToString());
                if (!string.IsNullOrEmpty(userId))
                    return Convert.ToInt64(userId);

                return 0;
            }
        }
    }
}
