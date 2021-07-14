using Microsoft.AspNetCore.Http;
using ServerProjectTracker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServerProjectTracker.AppLogic
{
    public static class Session
    {
        public static void setUser(this ISession session, Users user)
        {
            session.SetInt32("UserId", user.UserId);
            session.SetInt32("UserAccess", user.UserAccessLevel);
        }

        public static int? getUserId(this ISession session)
        {
            return session.GetInt32("UserId");
        }

        public static int? getUserAccess(this ISession session)
        {
            return session.GetInt32("UserAccess");
        }
    }
}
