using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Newtonsoft.Json;
using TradingBell.WebCat.CatalogDB;
using TradingBell.WebCat.CommonServices;
using TradingBell.WebCat.Helpers;

namespace WES
{
    public partial class SendNotificationMsg : System.Web.UI.Page
    {

        ErrorHandler objErrorhandler = new ErrorHandler();
        protected void Page_Load(object sender, EventArgs e)
        {

            try
            {
                ConnectionDB objConnection = new ConnectionDB();

                try
                {
                    string SELECTNOT = "SELECT * FROM TB_NOTIFICATION_DETAILS order by created_date desc ";

                    SqlCommand SELECTcmd = new SqlCommand(SELECTNOT, objConnection.GetConnection());
                    SqlDataAdapter daFILL = new SqlDataAdapter(SELECTcmd);
                    DataSet DS = new DataSet();
                    daFILL.Fill(DS);
                    if (DS != null)
                    {
                        if (DS.Tables[0].Rows.Count > 0)
                        {
                            Label1.Text = DS.Tables[0].Rows[0][0].ToString();
                            Label2.Text = DS.Tables[0].Rows[0][1].ToString();
                            HiddenField3.Value = DS.Tables[0].Rows[0][0].ToString();
                        }
                    }
                }
                catch (Exception ex)
                {
                    objErrorhandler.ErrorMsg = ex;
                    objErrorhandler.CreateLog();
                }




                DataSet ds = new DataSet();
                HelperServices objHelperService = new HelperServices();
                string querystr = "select distinct * from TB_NOTIFICATION_SUB_DETAILS where [MessageStatus]=0 and website_id=1";

                SqlCommand pscmd = new SqlCommand(querystr, objConnection.GetConnection());
                pscmd.CommandType = CommandType.Text;
                SqlDataAdapter da = new SqlDataAdapter(pscmd);
                da.Fill(ds);

                if (ds != null)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        //for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        //{
                        HiddenField1.Value = ds.Tables[0].Rows[0][0].ToString();
                        HiddenField2.Value = ds.Tables[0].Rows[0][1].ToString();
                        Page.ClientScript.RegisterStartupScript(this.GetType(), "Javascript", "javascript: SendNotification(); ", true);

                        try
                        {
                            string updatestr = "update  TB_NOTIFICATION_SUB_DETAILS  set MessageStatus=1 where [ENDPOINT]='" + HiddenField2.Value + "'";

                            SqlCommand updatecmd = new SqlCommand(updatestr, objConnection.GetConnection());
                            updatecmd.ExecuteNonQuery();
                        }
                        catch (Exception ex)
                        {
                            objErrorhandler.ErrorMsg = ex;
                            objErrorhandler.CreateLog();
                        }
                        //}

                    }


                }
                //using (StreamReader subds = File.OpenText(strxml))
                //{
                //    using (JsonReader reader = new JsonTextReader(subds))
                //    {
                //        JsonSerializer serializer = new JsonSerializer();


                //        // read the json from a stream
                //        // json size doesn't matter because only a small piece is read at a time from the HTTP request
                //       DataSet SubCategory = (DataSet)serializer.Deserialize(subds, typeof(DataSet));
                //        reader.Close();
                //    }
                //}


            }
            catch (Exception ex)
            {


            }
        }

    }
}