using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TradingBell.WebCat.CatalogDB;

namespace WES
{
    public partial class ProductItem : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                ConnectionDB objConnection = new ConnectionDB();
                string SELECTNOT = "SELECT * FROM TB_NOTIFICATION_DETAILS where website_id=1 order by created_date";

                SqlCommand SELECTcmd = new SqlCommand(SELECTNOT, objConnection.GetConnection());
                SqlDataAdapter daFILL = new SqlDataAdapter(SELECTcmd);
                DataSet DS = new DataSet();
                daFILL.Fill(DS);
                if (DS != null)
                {
                    if (DS.Tables[0].Rows.Count > 0)
                    {
                        Response.Redirect(DS.Tables[0].Rows[0][1].ToString());
                    }
                }

            }
            catch (Exception ex)
            {
                //objErrorhandler.ErrorMsg = ex;
                //objErrorhandler.CreateLog();
            }
        }
    }
}