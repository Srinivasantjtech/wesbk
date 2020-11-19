using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Caching;
using TradingBell.WebCat.EasyAsk;
using TradingBell.WebCat.TemplateRender;
using System.Data;
namespace WES.App_Code.CL
{
       [System.ComponentModel.DataObject]
    public class StaticCache
    {
           public static void LoadStaticCache()
           {


               TBWTemplateEngine tbwtEngine = new TBWTemplateEngine("BOTTOM", HttpContext.Current.Server.MapPath("~\\Templates"), "");
               string html = tbwtEngine.ST_Bottom_Load();
               HttpRuntime.Cache.Insert("Cache_bottom", html, null, Cache.NoAbsoluteExpiration, Cache.NoSlidingExpiration,
                CacheItemPriority.NotRemovable, null);
               tbwtEngine = new TBWTemplateEngine("TopCache", HttpContext.Current.Server.MapPath("~\\Templates"), "");
               string html_top = tbwtEngine.ST_Top_Load_cache();
               HttpRuntime.Cache.Insert("Cache_Top", html_top, null, Cache.NoAbsoluteExpiration, Cache.NoSlidingExpiration,
                CacheItemPriority.NotRemovable, null);
               EasyAsk_WES EasyAsk = new EasyAsk_WES();
               HttpContext.Current.Application["key_MainCategory"] = EasyAsk.GetCategoryAndBrand_Applicationstart("MainCategory");
           
           }
    }
}