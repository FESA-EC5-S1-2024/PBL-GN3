using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace PBLprojectMVC.Controllers
{
    public static class HelperController
    {
        public static bool LoginSessionVerification(ISession session)
        {
            if(session.GetString("UserLogged") == null)
                return false;
            else
                return true;
        }

        public static bool AdminSessitionVerification(ISession session)
        {
            if(session.GetString("IsAdmin") == null)
                return false;
            else
                return true;
        }

        public static int? ActualUserID(ISession session)
        {
            return session.GetInt32("ID");
        }

    }
}

