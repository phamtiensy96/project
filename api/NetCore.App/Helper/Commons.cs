//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Web;
//using System.Web.Routing;

//namespace MachineVueJSLTE.Helper
//{
//  public static class Commons
//  {
//    public static string GetCurrentWebsiteRoot()
//    {
//      return HttpContext.Current.Request.Url.AbsolutePath;
//    }

//    public static string ActiveMenu(string action)
//    {
//      string path = GetCurrentWebsiteRoot().ToLower();
//      string queryString = HttpContext.Current.Request.Url.Query;

//      if ((path == "/"&& action == "home"))
//      {
//        return "active";
//      }
//      else if ( path == "/"+action)
//      {
//        return "active";
//      }
//      else if (queryString.Contains(action))
//      {
//        return "active";
//      }
//      else
//        return string.Empty;
//    }
//  }
//}
