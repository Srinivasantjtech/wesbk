using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TradingBell.WebCat.EasyAsk;
using System.Data;  
namespace WES
{
    public partial class EAhomeindex : System.Web.UI.Page
    {
        EasyAsk_WES EasyAsk = new EasyAsk_WES();
        protected void Page_Load(object sender, EventArgs e)
        {

            EasyAsk.Create_NewProducts_Json();
          
            DataSet  main=EasyAsk.GetCategoryAndBrand("", true);

            if (main != null && main.Tables.Count > 0 && main.Tables[0].Rows.Count > 0)
            {
                //foreach (DataRow dr in main.Tables[0].Rows)
                for( int i=0;i<= main.Tables[0].Rows.Count-1;i++)  
                {
                    EasyAsk.GetMainMenuClickDetailJson(main.Tables[0].Rows[i]["CATEGORY_ID"].ToString(), "", false);
                }
            }
        }
    }
}