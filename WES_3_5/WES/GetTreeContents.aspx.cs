using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Xml;
using System.Configuration;
using System.Data.OleDb;
using System.Data.SqlClient;
using TradingBell.WebCat.CatalogDB;
using TradingBell.WebCat.Helpers;
using TradingBell.WebCat.TemplateRender;
using TradingBell.WebCat.CommonServices;
namespace ajaxtree
{
	/// <summary>
	/// Summary description for GetTreeContents.
	/// </summary>
    /// 
	public partial class GetTreeContents : System.Web.UI.Page
	{
        CategoryServices objCategoryServices = new CategoryServices();
        HelperServices objHelperServices = new HelperServices();
        string catalogID="";
	 	protected void Page_Load(object sender, System.EventArgs e)
		{			
			//id passed thro' the url query string
			string key = Request.QueryString["q"];
            catalogID = objHelperServices.GetOptionValues("DEFAULT CATALOG").ToString();
			if (key != null)
			{				
               
                LoadSubCategoriesFunction(key);
                if (Session["subcattreeID"] != null && Session["subcattreeID"].ToString().Length > 0)
                {
                    string Category_id = Session["subcattreeID"].ToString();
                    //Response.Write("<script type=\"text\\javascript\">alert('hello');</script>");
                    //Response.Write("<script>if (Toggle(" + Category_id + "))showContentsEx('divTree{" + Category_id + "}','{" + Category_id + "}','');</script>");
                }
                
                
			}
           
		}
        public void LoadSubCategoriesFunction(string CatID)
        {
            DataSet SubCategories = new DataSet();
            string sSQL = " SELECT * FROM TB_CATEGORY TC,TB_CATALOG_SECTIONS TCS";
            sSQL = sSQL + " WHERE TC.PARENT_CATEGORY =N'" + CatID + "'";
            sSQL = sSQL + " AND TC.CATEGORY_ID = TCS.CATEGORY_ID AND TCS.CATALOG_ID =" + catalogID;
            sSQL = sSQL + " ORDER BY SORT_ORDER";
            oHelper.SQLString = sSQL;
            SubCategories = oHelper.GetDataSet(); 
            if (SubCategories != null)
            {
                if(SubCategories.Tables[0].Rows.Count>0)
                Response.Write("<ul class='wTreeStyle'>"); 	
                for (int i = 0; i < SubCategories.Tables[0].Rows.Count; i++)
                {
                    string CateID = SubCategories.Tables[0].Rows[i]["CATEGORY_ID"].ToString();
                    string CatName = SubCategories.Tables[0].Rows[i]["CATEGORY_NAME"].ToString();
                    int catcount = Convert.ToInt32(oCat.GetSubCategoriesCount(CateID,Convert.ToInt32(catalogID)));
                    //if (catcount > 0)
                    //{
                    //    Response.Write(string.Format("<li id='{0}'><span valign='bottom' title='{1}' onclick=\"javascript:if (Toggle(this))showContentsEx('divTree{0}','{0}','');\"><IMG  align='bottom' SRC='images/rightarrow.gif'> <span class='treeStyleNode' >{1}</span></span><span id ='divTree{0}'></span></li>",
                    //            CateID, CatName));
                    //}
                    //else
                    //{
                    //    Response.Write(string.Format("<li id='{0}'><span valign='bottom' title='{1}' onclick=\"javascript:if (Toggle(this))showContentsEx('divTree{0}','{0}','');\"><span class='treeStyleNode' >{1}</span></span><span id ='divTree{0}'></span></li>",
                    //           CateID, CatName));
                    //}
                    if (catcount > 0)
                    {
                        if (Session["subcattreeID"] != null && Session["subcattreeID"].ToString().Length > 0 && Session["subcattreeID"].ToString() == CateID)
                        {
                            Response.Write(string.Format("<li id='{0}'><span valign='bottom' title='{1}' ><IMG  align='bottom' SRC='images/leftarrow.gif' onclick=\"javascript:if (Toggle(this))showContentsEx('divTree{0}','{0}','');\"/> <span class='treeStyleNode'><a href=\"bybrand.aspx?&ld=0&&cid=" + CateID + "\">{1}</a></span></span><span id ='divTree{0}'></span></li>",
                                                                CateID, CatName));
                            DataSet dSubcategories = new DataSet();
                            sSQL = " SELECT * FROM TB_CATEGORY TC,TB_CATALOG_SECTIONS TCS";
                            sSQL = sSQL + " WHERE TC.PARENT_CATEGORY =N'" + CateID + "'";
                            sSQL = sSQL + " AND TC.CATEGORY_ID = TCS.CATEGORY_ID AND TCS.CATALOG_ID =" + catalogID;
                            sSQL = sSQL + " ORDER BY SORT_ORDER";
                            oHelper.SQLString = sSQL;
                            dSubcategories = oHelper.GetDataSet();
                            if (dSubcategories != null)
                            {
                                if (dSubcategories.Tables[0].Rows.Count > 0)
                                    Response.Write("<ul class='wTreeStyle'>");
                                for (int j = 0; j < dSubcategories.Tables[0].Rows.Count; j++)
                                {
                                    CateID = dSubcategories.Tables[0].Rows[j]["CATEGORY_ID"].ToString();
                                    CatName = dSubcategories.Tables[0].Rows[j]["CATEGORY_NAME"].ToString();
                                    catcount = Convert.ToInt32(oCat.GetSubCategoriesCount(CateID, Convert.ToInt32(catalogID)));
                                    if (catcount > 0)
                                    {
                                        Response.Write(string.Format("<li id='{0}'><span valign='bottom' title='{1}' ><IMG  align='bottom' SRC='images/leftarrow.gif' onclick=\"javascript:if (Toggle(this))showContentsEx('divTree{0}','{0}','');\"/> <span class='treeStyleNode'><a href=\"bybrand.aspx?&ld=0&&cid=" + CateID + "\">{1}</a></span></span><span id ='divTree{0}'></span></li>",
                                                                                 CateID, CatName));

                                    }
                                    else
                                    {
                                        Response.Write(string.Format("<li style=\"margin-left:-10px; padding-left:45px;\" id='{0}'><span valign='bottom' title='{1}'><span class='treeStyleNode' ><a onclick=\" javascript:GetTreecategoryid('" + CateID + "');\"  href=\"bybrand.aspx?&ld=0&&cid=" + CateID + "\" >{1}</a></span></span><span id ='divTree{0}'></span></li>",
                              CateID, CatName));
                                    }
                                }
                                if (dSubcategories.Tables[0].Rows.Count > 0)
                                    Response.Write("</ul>");
                                //Session["subcattreeID"] = null;
                            }

                        }
                        else
                        {
                            Response.Write(string.Format("<li id='{0}'><span valign='bottom' title='{1}' ><IMG id='{0}' align='bottom' SRC='images/rightarrow.gif' onclick=\"javascript:if (Toggle(this))showContentsEx('divTree{0}','{0}','');\"/> <span class='treeStyleNode'><a href=\"bybrand.aspx?&ld=0&&cid=" + CateID + "\">{1}</a></span></span><span id ='divTree{0}'></span></li>",
                                    CateID, CatName));
                        }

                    }
                    else
                    {
                        Response.Write(string.Format("<li style=\"margin-left:-10px; padding-left:45px;\" id='{0}'><span valign='bottom' title='{1}'><span class='treeStyleNode' ><a onclick=\" javascript:GetTreecategoryid('" + CateID  + "');\"  href=\"bybrand.aspx?&ld=0&&cid=" + CateID + "\" >{1}</a></span></span><span id ='divTree{0}'></span></li>",
                               CateID, CatName));
                    }
                }
                if (SubCategories.Tables[0].Rows.Count > 0)
                Response.Write("</ul>"); 
              
            }
        }

		#region Web Form Designer generated code
		override protected void OnInit(EventArgs e)
		{
			//
			// CODEGEN: This call is required by the ASP.NET Web Form Designer.
			//
			InitializeComponent();
			base.OnInit(e);
		}
		
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{    
		}
		#endregion
        public DataSet GetDataSet(string SQLString)
        {
            string sql = @"Data Source=TBRND1\RND1;Initial Catalog=TB_WESNEW_SUIT;User ID=tbadmin;Password=data2go";
            SqlConnection oCon = new SqlConnection(sql);
            DataSet oDs = new DataSet();
            try
            {
                oCon.Open();
                SqlDataAdapter oDA = new SqlDataAdapter(SQLString, oCon);
                oDA.Fill(oDs);
                oCon.Close(); 
            }
            catch (Exception ex)
            {
              
            }
            return oDs;
        }
		 
	}
}
