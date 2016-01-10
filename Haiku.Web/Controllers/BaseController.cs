using Haiku.DTO;
using Haiku.Services;
using Haiku.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Haiku.Web.Controllers
{
    public abstract class BaseController : Controller
    {
        public string LoggedUserNickname
        {
            get
            {
                return ((SessionDto)Session[SessionsService.SessionTokenLabelConst]).Nickname;
            }
        }
    }
}