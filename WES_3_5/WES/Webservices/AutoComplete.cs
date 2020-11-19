using System;
using System.Web;
using System.Data;
using System.IO;
using System.Collections;
using System.Web.Services;
using System.Web.Services.Protocols;
using TradingBell.Common;
using TradingBell.WebServices;

//[assembly: System.Reflection.AssemblyVersion("5.0")]
namespace TradingBell.WebServices
{
    /// <summary>
    /// This is used to listout all words in the dropdown box
    /// </summary>
    /// <example>
    /// AutoComplete oAutCom =new AutoComplete();
    /// </example>
    [WebService(Namespace = "http://WebCat.com/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class AutoComplete : System.Web.Services.WebService
    {
        /// <summary>
        /// Default Constructor
        /// </summary>
        public AutoComplete()
        {
            //Uncomment the following line if using designed components 
            //InitializeComponent(); 
        }
        Helper oHelper = new Helper();
        ErrorHandler oErrHand = new ErrorHandler();

        private static string[] autoCompleteWordList = null;
        /// <summary>
        /// This is used to list out the frequently used list of words in the Text box        
        /// </summary>
        /// <param name="prefixText">string</param>
        /// <param name="count">int</param>
        /// <returns>String[]</returns>
        /// <example>
        /// <code>
        ///using System;
        ///using System.Web;
        ///using System.Data;
        ///using System.IO;
        ///using TradingBell.Common;
        ///using TradingBell.WebServices;
        /// protected void Page_Load(object sender, EventArgs e)
        /// {
        ///     AutoComplete AutCom =new AutoComplete();
        ///     string prefixText;
        ///     int count;
        ///     String AutComRetstr;
        ///     ...
        ///     AutComRetstr=AutCom.GetWordList(prefixText,count);
        /// }
        /// </code>
        /// </example>
        [WebMethod]
        public String[] GetWordList(string prefixText, int count)
        {
            try
            {
                DataSet ds = new DataSet();
                string sSQL;
                sSQL = " SELECT CATALOG_ITEM_NO FROM TB_PRODUCT";
                oHelper.SQLString = sSQL;
                ds = oHelper.GetDataSet();

                FileInfo oFileInfo = new FileInfo(Server.MapPath("~/App_Data/WordList.txt"));
                oFileInfo.Delete();
                StreamWriter oStramWriter = oFileInfo.CreateText();
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    oStramWriter.WriteLine(dr["CATALOG_ITEM_NO"].ToString());
                    oStramWriter.Write(oStramWriter.NewLine);
                }

                oStramWriter.Close();
            }
            catch (Exception ex)
            {
                oErrHand.ErrorMsg = ex;
                //oErrHand.CreateLog();
            }

            if (autoCompleteWordList == null)
            {
                string[] temp = File.ReadAllLines(Server.MapPath("~/App_Data/WordList.txt"));
                Array.Sort(temp, new CaseInsensitiveComparer());
                autoCompleteWordList = temp;
            }

            int index = Array.BinarySearch(autoCompleteWordList, prefixText,
              new CaseInsensitiveComparer());
            if (index < 0)
            {
                index = ~index;
            }

            int matchingCount;
            for (matchingCount = 0;
                 matchingCount < count && index + matchingCount <
                 autoCompleteWordList.Length;
                 matchingCount++)
            {
                if (!autoCompleteWordList[index +
                  matchingCount].StartsWith(prefixText,
                  StringComparison.CurrentCultureIgnoreCase))
                {
                    break;
                }
            }

            String[] returnValue = new string[matchingCount];
            if (matchingCount > 0)
            {
                Array.Copy(autoCompleteWordList, index, returnValue, 0,
                  matchingCount);
            }
            return returnValue;
        }



    }
}
