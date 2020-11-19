using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Text.RegularExpressions;
using System.Data.SqlClient;
using TradingBell.WebCat.CatalogDB;
using TradingBell.WebCat.Helpers;
using TradingBell.WebCat.TemplateRender;
using TradingBell.WebCat.CommonServices;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Web.Services;
using System.Web.Configuration;
using System.Data.OleDb;
using System.IO;
using TradingBell.WebCat.EasyAsk;

public partial class UI_ProductFinder : System.Web.UI.UserControl
{

    EasyAsk_WES EasyAsk = new EasyAsk_WES();
    Security objSecurity = new Security();
    TBWTemplateRenderProductFinder objTBWTemplateRenderProductFinder = new TBWTemplateRenderProductFinder();
    static string strimgPath = HttpContext.Current.Server.MapPath("ProdImages");

    string Cable1 = "";
    string Cable2 = "";
    public string cblconnector_type = "";
    string leftBrand = "";
    string rightModel = "";

    string leftVBrand = "";
    string rightVModel = "";

    string type = "";
    string value = "";
    string tab = "";
    string ea_path = "";
    string Ea = "AllProducts////WESAUSTRALASIA";
    string tempEa = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        


        if (Request.QueryString["tab"] != null)
        {

            tab = Request.QueryString["tab"].ToString();
        }
        if (tab == "" || tab == "1")
        {
            if (Request.QueryString["l"] != null)
            {
                Cable1 = Request.QueryString["l"].ToString();
               
            }
            if (Request.QueryString["r"] != null)
            {
                Cable2 = Request.QueryString["r"].ToString();
            }
            if (Request.QueryString["ct"] != null)
            {
                cblconnector_type = Request.QueryString["ct"].ToString();
            }
            else
            {
                cblconnector_type = "Cables";
            }
        }
        else if (tab == "2")
        {
            if (Request.QueryString["l"] != null)
            {
                leftBrand  = Request.QueryString["l"].ToString();
             
            }
            if (Request.QueryString["r"] != null)
            {
                rightModel = Request.QueryString["r"].ToString();
            }
        }
        else if (tab == "3")
        {
            if (Request.QueryString["l"] != null)
            {
                leftVBrand = Request.QueryString["l"].ToString();

            }
            if (Request.QueryString["r"] != null)
            {
                rightVModel = Request.QueryString["r"].ToString();
            }
        }
        
        if (Request.QueryString["type"] != null)
        {
            type = Request.QueryString["type"].ToString();
        }
        if (Request.QueryString["value"] != null)
        {
            value  = Request.QueryString["value"].ToString();
        }
        if (Request.QueryString["ea"] != null)
        {
            ea_path = HttpUtility.UrlDecode(objSecurity.StringDeCrypt(Request.QueryString["ea"].ToString()));
            Session["EA"]=HttpUtility.UrlDecode(objSecurity.StringDeCrypt(Request.QueryString["ea"].ToString()));
        }
       // if (IsPostBack == false)
        //{
            if (tab == "1" && Cable1 != "" & Cable2!="")
            {

                if (type.ToLower() == "cablelr")
                    HttpContext.Current.Session["TOPAttributes"] = null;
                 //Ea = Ea + "////AttribSelect=CableL = '" + Cable1 + "'";
                EasyAsk.GetAttributeProducts("ProductList", "", type, value,  Cable1, "24", "0", "Next");
            }
            else if (tab == "2" && leftBrand != "" & rightModel != "")
            {

                if (type.ToLower() == "model")
                    HttpContext.Current.Session["TOPAttributes"] = null;
                //Ea = Ea + "////AttribSelect=CableL = '" + Cable1 + "'";
                EasyAsk.GetAttributeProducts("ProductList", "", type, value, leftBrand, "24", "0", "Next");
            }
            else if (tab == "3" && leftVBrand != "" & rightVModel != "")
            {
                if (type.ToLower() == "model")
                    HttpContext.Current.Session["TOPAttributes"] = null;

                //Ea = Ea + "////AttribSelect=CableL = '" + Cable1 + "'";
                EasyAsk.GetAttributeProducts("ProductList", "", type, value, leftVBrand, "24", "0", "Next");
            }
        //}
        
    }
    public string GetCableLeftRight()
    {      
        string strrtn = "";
     
        if (Cable1 == "")
        {

            FindCableL("");
            if (Session["CableL"] != null )
            {
                strrtn = objTBWTemplateRenderProductFinder.ST_CableLImage_Load((DataTable)Session["CableL"],"",cblconnector_type );
            }
        }
        else if (Cable1 != "")
        {
            
            FindCableLR("", Cable1);
            if (Session["CableLR"] != null && Session["Cable_images"] != null)
            {
                tempEa = Ea + "////AttribSelect=CableL = '" + Cable1 + "'";
                strrtn = objTBWTemplateRenderProductFinder.ST_CableRImage_Load((DataTable)Session["CableLR"], Cable1, tempEa, "", cblconnector_type);
            }
        }
        return strrtn;
    }
    public string GetCable1Value()
    {
        if (Request.QueryString["l"] != null)
        {
            return Request.QueryString["l"].ToString();
        }
        else
            return "";
    }
    public string GetCableLeft()
    {
        string strrtn = "";
        if (Cable1!="")
        {

            FindCableL("");
            if (Session["CableL"]!=null && Session["Cable_images"]!=null)
            {
               strrtn= objTBWTemplateRenderProductFinder.ST_CableL_Load((DataTable) Session["CableL"] ,Cable1, GetCableFilterImagePath( (DataTable) Session["Cable_images"],Cable1));
            }
        }
        else
        {
            FindCableL("");
            if (Session["CableL"]!=null && Session["Cable_images"]!=null)
            {
               strrtn= objTBWTemplateRenderProductFinder.ST_CableL_Load((DataTable) Session["CableL"] ,"", "");
            }
        }
        return strrtn;
    }
    public string GetCableRight()
    {
        string strrtn = "";
           tempEa = Ea + "////AttribSelect=CableL = '" + Cable1 + "'";
        if (Cable1 != "" & Cable2 != "")
        {

            FindCableLR("",Cable1);
            if (Session["CableLR"] != null && Session["Cable_images"] != null)
            {
                strrtn = objTBWTemplateRenderProductFinder.ST_CableR_Load((DataTable)Session["CableLR"], Cable2, GetCableFilterImagePath((DataTable)Session["Cable_images"], Cable2), Cable1, tempEa);
            }
        }
        else if (Cable1 != "" & Cable2 == "")
        {
            FindCableLR("", Cable1);
            if (Session["CableLR"] != null)
            {
               
                strrtn = objTBWTemplateRenderProductFinder.ST_CableR_Load((DataTable)Session["CableLR"], "", "", Cable1,tempEa);
            }
        }
        else 
        {
            strrtn = objTBWTemplateRenderProductFinder.ST_CableR_Load(null, "", "","","");           
        }
        return strrtn;
    }
    public  void FindCableL(string strfindvalue)
    {

        DataSet rtnds = new DataSet();

        DataTable tblCable = new DataTable();
        DataTable tblCableL = new DataTable();
        


        string rtnstr = "";
        
        string tempst = "";
        TradingBell.WebCat.EasyAsk.EasyAsk_WES EasyAsk = new TradingBell.WebCat.EasyAsk.EasyAsk_WES();


        DataColumn dc = new DataColumn("CableL");
        dc.DefaultValue = "";
        tblCableL.Columns.Add(dc);
         dc = new DataColumn("Image_Path");
        dc.DefaultValue = "";
        tblCableL.Columns.Add(dc);
        dc = new DataColumn("connector_type");
        dc.DefaultValue = "";
        tblCableL.Columns.Add(dc); 



        DataSet dsCat = new DataSet();

        dsCat = EasyAsk.GetCategoryAndBrand("Filter");

        string strSQL = "Exec STP_TBWC_PICK_CABLE_FILTER";
        HelperDB objHelperDB = new HelperDB();
        DataSet dsCables = objHelperDB.GetDataSetDB(strSQL);

        if (dsCables != null && dsCables.Tables.Count > 0 && dsCables.Tables[0] != null && dsCables.Tables[0].Rows.Count > 0)
        {
            tblCable = dsCables.Tables[0].Copy();
        }

        if (dsCat == null || dsCat.Tables.Count == 0 || dsCat.Tables["CableL"] == null || dsCat.Tables["CableL"].Rows.Count == 0)
            rtnds.GetXml();
        else
        {
            rtnds.Tables.Add(dsCat.Tables["CableL"].Copy());
        }
        DataRow dr = null;
        if (rtnds != null && rtnds.Tables[0] != null && rtnds.Tables[0].Rows.Count > 0)
        {
            
            for (int i = 0; i <= rtnds.Tables[0].Rows.Count - 1; i++)
            {

                //rtnds.Tables[0].Rows[i]["Image_Path"] = GetCableFilterImagePath(tblCable, rtnds.Tables[0].Rows[i]["CableL"].ToString());
                if (strfindvalue != "")
                {
                    if (rtnds.Tables[0].Rows[i]["CableL"].ToString().ToLower().Contains(strfindvalue.ToLower()))
                    {
                        dr = tblCableL.NewRow(); 
                        dr["CableL"]=rtnds.Tables[0].Rows[i]["CableL"].ToString();
                        dr["Image_Path"]= GetCableFilterImagePath(tblCable, rtnds.Tables[0].Rows[i]["CableL"].ToString());
                        dr["Connector_Type"] = GetCableFilterConnector_type(tblCable, rtnds.Tables[0].Rows[i]["CableL"].ToString()); 
                         tblCableL.Rows.Add(dr);   
                        //DrpCable1.Items.Add(rtnds.Tables[0].Rows[i]["CableL"].ToString());
                        //rtnstr = rtnstr + rtnds.Tables[0].Rows[i]["CableL"].ToString() + "&&&&&" + GetCableFilterImagePath(tblCable, rtnds.Tables[0].Rows[i]["CableL"].ToString()) + "#####";
                    }
                }
                else
                {
                    dr = tblCableL.NewRow(); 
                    dr["CableL"] = rtnds.Tables[0].Rows[i]["CableL"].ToString();
                    dr["Image_Path"] = GetCableFilterImagePath(tblCable, rtnds.Tables[0].Rows[i]["CableL"].ToString()); ;
                    dr["Connector_Type"] = GetCableFilterConnector_type(tblCable, rtnds.Tables[0].Rows[i]["CableL"].ToString()); 
                    tblCableL.Rows.Add(dr);   

                    //DrpCable1.Items.Add(rtnds.Tables[0].Rows[i]["CableL"].ToString());
                    //rtnstr = rtnstr + rtnds.Tables[0].Rows[i]["CableL"].ToString() + "&&&&&" + GetCableFilterImagePath(tblCable, rtnds.Tables[0].Rows[i]["CableL"].ToString()) + "#####";
                }

            }
        }
        Session["CableL"] = tblCableL;
        Session["Cable_images"] = tblCable;
      
    
    }
    public void FindCableLR(string strfindvalue, string strCable1value)
    {

        DataSet rtnds = new DataSet();
        DataTable tblCable = new DataTable();
        DataTable tblCableLR = new DataTable();
        string rtnstr = "";
        string tempst = "";


        DataColumn dc = new DataColumn("CableLR");
        dc.DefaultValue = "";
        tblCableLR.Columns.Add(dc);
        dc = new DataColumn("Image_Path");
        dc.DefaultValue = "";
        tblCableLR.Columns.Add(dc);
        dc = new DataColumn("connector_type");
        dc.DefaultValue = "";
        tblCableLR.Columns.Add(dc); 

        TradingBell.WebCat.EasyAsk.EasyAsk_WES EasyAsk = new TradingBell.WebCat.EasyAsk.EasyAsk_WES();

        Session["CableLR"] = tblCableLR;
        if (strCable1value == "")
            return;

        DataSet dsCat = new DataSet();

        dsCat = EasyAsk.GetCategoryAndBrand("Filter");

        string strSQL = "Exec STP_TBWC_PICK_CABLE_FILTER";
        HelperDB objHelperDB = new HelperDB();
        DataSet dsCables = objHelperDB.GetDataSetDB(strSQL);

        if (dsCables != null && dsCables.Tables.Count > 0 && dsCables.Tables[0] != null && dsCables.Tables[0].Rows.Count > 0)
        {
            tblCable = dsCables.Tables[0].Copy();
        }

        if (dsCat == null || dsCat.Tables.Count == 0 || dsCat.Tables["CableLR"] == null || dsCat.Tables["CableLR"].Rows.Count == 0)
            rtnds.GetXml();
        else
        {
            rtnds.Tables.Add(dsCat.Tables["CableLR"].Copy());
        }
        rtnstr = "All Cable" + "&&&&&" + GetCableFilterImagePath(tblCable, "All Cable") + "#####";
        DataRow dr = null;
        if (rtnds != null && rtnds.Tables[0] != null && rtnds.Tables[0].Rows.Count > 0)
        {
           

            for (int i = 0; i <= rtnds.Tables[0].Rows.Count - 1; i++)
            {
                tempst = rtnds.Tables[0].Rows[i]["CableLR"].ToString();
                if (tempst.Contains(":"))
                {
                    string[] str = tempst.Split(':');
                    if (str.Length > 0)
                    {
                        if (str[0].ToString().ToLower() == strCable1value.ToLower())
                        {
                            if (strfindvalue != "")
                            {
                                if (str[1].ToLower().Contains(strfindvalue.ToLower()))
                                {
                                    dr = tblCableLR.NewRow(); 
                                    dr["CableLR"] = str[1].ToString();
                                    dr["Image_Path"] = GetCableFilterImagePath(tblCable, str[1].ToString());
                                    dr["Connector_Type"] = GetCableFilterConnector_type(tblCable, str[1].ToString()); 
                                    tblCableLR.Rows.Add(dr); 

                                    //rtnstr = rtnstr + str[1].ToString() + "&&&&&" + GetCableFilterImagePath(tblCable, str[1].ToString()) + "#####";
                                }
                            }
                            else
                            {
                                dr = tblCableLR.NewRow(); 
                                dr["CableLR"] = str[1].ToString();
                                dr["Image_Path"] = GetCableFilterImagePath(tblCable, str[1].ToString());
                                dr["Connector_Type"] = GetCableFilterConnector_type(tblCable, str[1].ToString()); 
                                tblCableLR.Rows.Add(dr); 
                                //rtnstr = rtnstr + str[1].ToString() + "&&&&&" + GetCableFilterImagePath(tblCable, str[1].ToString()) + "#####";
                            }


                        }
                    }

                }

            }
        }
        //if (rtnstr != "")
        //    rtnstr = rtnstr.Substring(0, rtnstr.Length - 5);

        //return rtnstr;
        Session["CableLR"] = tblCableLR;
        Session["Cable_images"] = tblCable;
    }
    public static String GetCableFilterImagePath(DataTable dt, string cableName)
    {
        FileInfo Fil;
        try
        {
            if (dt != null && dt.Rows.Count > 0)
            {
                DataRow[] dr = dt.Select("Cable_Name='" + cableName + "'");
                if (dr.Length > 0)
                {
                    Fil = new FileInfo(strimgPath + dr[0]["Cable_image"].ToString());

                    if (Fil.Exists)
                        return "/prodimages" + dr[0]["Cable_image"].ToString();
                    else
                        return "/images/noimage.gif";

                }

            }
            else
            {
                return "/images/noimage.gif";
            }
        }
        catch (Exception ex)
        {
        }
        return "/images/noimage.gif";
    }
    public static String GetCableFilterConnector_type(DataTable dt, string cableName)
    {
        
        try
        {
            if (dt != null && dt.Rows.Count > 0)
            {
                DataRow[] dr = dt.Select("Cable_Name='" + cableName + "'");
                if (dr.Length > 0)
                {
                    if (dr[0]["connector_type"].ToString().Trim()!="")
                        return dr[0]["connector_type"].ToString();
                    else
                        return "Cables";
                }
                else
                {
                    return "Cables";
                }

            }
            else
            {
                return "Cables";
            }
        }
        catch (Exception ex)
        {
        }
        return "Cables";
    }



    public  void FindMyBrand(string strfindvalue)
    {

        DataSet rtnds = new DataSet();
        DataTable tblbrand = new DataTable();
        DataTable tblmodel = new DataTable();
        DataTable tblFindBrand = new DataTable();
        string rtnstr = "";
        string tmpstr = "";
        string tmpstrimage = "";
        string tmpstrCat_id = "";
        TradingBell.WebCat.EasyAsk.EasyAsk_WES EasyAsk = new TradingBell.WebCat.EasyAsk.EasyAsk_WES();
        HelperDB objHelperDB = new HelperDB();

        DataColumn dc = new DataColumn("Brand");
        dc.DefaultValue = "";
        tblFindBrand.Columns.Add(dc);
        dc = new DataColumn("Image_Path");
        dc.DefaultValue = "";
        tblFindBrand.Columns.Add(dc);
        dc = new DataColumn("Category_id");
        dc.DefaultValue = "";
        tblFindBrand.Columns.Add(dc); 


        DataSet dsCat = new DataSet();

        dsCat = EasyAsk.GetCategoryAndBrand("Filter");


        DataSet dsbrands = (DataSet)objHelperDB.GetGenericPageDataDB("Brand", "GET_BRAND_MODEL", HelperDB.ReturnType.RTDataSet);
        DataSet dsmodels = (DataSet)objHelperDB.GetGenericPageDataDB("Model", "GET_BRAND_MODEL", HelperDB.ReturnType.RTDataSet);




        DataRow dr = null;

        if (dsbrands != null && dsbrands.Tables.Count > 0 && dsbrands.Tables[0] != null && dsbrands.Tables[0].Rows.Count > 0)
        {
            tblbrand = dsbrands.Tables[0].Copy();
        }
        if (dsmodels != null && dsmodels.Tables.Count > 0 && dsmodels.Tables[0] != null && dsmodels.Tables[0].Rows.Count > 0)
        {
            tblmodel = dsmodels.Tables[0].Copy();
        }

        if (dsCat == null || dsCat.Tables.Count == 0 || dsCat.Tables["Brand"] == null || dsCat.Tables["Brand"].Rows.Count == 0)
            rtnds.GetXml();
        else
        {
            rtnds.Tables.Add(dsCat.Tables["Brand"].Copy());
        }
        
        if (rtnds != null && rtnds.Tables[0] != null && rtnds.Tables[0].Rows.Count > 0)
        {

            for (int i = 0; i <= rtnds.Tables[0].Rows.Count - 1; i++)
            {

                tmpstr = rtnds.Tables[0].Rows[i]["Brand"].ToString();
                tmpstrimage = GetBrandFilterImagePath(tblbrand, tmpstr);
                tmpstrCat_id = GetBrandFilterCategory_id(tblbrand, tmpstr);
                dr = tblFindBrand.NewRow();
                dr["Brand"] = tmpstr;
                dr["Image_Path"] = tmpstrimage;
                dr["Category_id"] = tmpstrCat_id;
                tblFindBrand.Rows.Add(dr); 

                

            }
        }


        Session["ProdFinder_Brand"] = tblFindBrand;
        Session["ProdFinder_Brand_Images"] = tblbrand; 
        
    }

    public void FindMyModel(string strfindvalue, string strBrandvalue)
    {

        DataSet rtnds = new DataSet();
        DataTable tblmodel = new DataTable();
        DataTable tblFindModel = new DataTable();
        string rtnstr = "";
        string tempst = "";


        DataColumn dc = new DataColumn("Model");
        dc.DefaultValue = "";
        tblFindModel.Columns.Add(dc);
        dc = new DataColumn("Image_Path");
        dc.DefaultValue = "";
        tblFindModel.Columns.Add(dc);

        dc = new DataColumn("Category_id");
        dc.DefaultValue = "";
        tblFindModel.Columns.Add(dc); 


        TradingBell.WebCat.EasyAsk.EasyAsk_WES EasyAsk = new TradingBell.WebCat.EasyAsk.EasyAsk_WES();
        Session["ProdFinder_Model"] = tblFindModel;
        HelperDB objHelperDB = new HelperDB();
        if (strBrandvalue == "")
            return ;



        DataSet dsCat = new DataSet();

        dsCat = EasyAsk.GetCategoryAndBrand("Filter");

        DataSet dsmodels = (DataSet)objHelperDB.GetGenericPageDataDB("Model", "GET_BRAND_MODEL", HelperDB.ReturnType.RTDataSet);

        if (dsmodels != null && dsmodels.Tables.Count > 0 && dsmodels.Tables[0] != null && dsmodels.Tables[0].Rows.Count > 0)
        {
            tblmodel = dsmodels.Tables[0].Copy();
        }

        if (dsCat == null || dsCat.Tables.Count == 0 || dsCat.Tables["Models"] == null || dsCat.Tables["Models"].Rows.Count == 0)
            rtnds.GetXml();
        else
        {
            rtnds.Tables.Add(dsCat.Tables["Models"].Copy());
        }
        
        DataRow dr = null;
        if (rtnds != null && rtnds.Tables[0] != null && rtnds.Tables[0].Rows.Count > 0)
        {
            for (int i = 0; i <= rtnds.Tables[0].Rows.Count - 1; i++)
            {
                tempst = rtnds.Tables[0].Rows[i]["Models"].ToString();
                if (tempst.Contains(":"))
                {
                    string[] str = tempst.Split(':');
                    if (str.Length > 0)
                    {
                        if (str[0].ToString().ToLower() == strBrandvalue.ToLower())
                        {
                           
                                dr = tblFindModel.NewRow();
                                dr["Model"] = str[1].ToString();
                                dr["Image_Path"] = GetBrandFilterImagePath(tblmodel, str[1].ToString());
                                dr["Category_id"] = GetBrandFilterCategory_id(tblmodel, str[1].ToString());
                                tblFindModel.Rows.Add(dr); 

                          

                        }
                    }

                }

            }
        }
        Session["ProdFinder_Model"] = tblFindModel;
        Session["ProdFinder_Model_images"] = tblmodel;
    }

    public String GetBrandFilterImagePath(DataTable dt, string bmName)
    {
        FileInfo Fil;
        try
        {
            if (dt != null && dt.Rows.Count > 0)
            {
                DataRow[] dr = dt.Select("BMName='" + bmName + "'");
                if (dr.Length > 0)
                {
                    Fil = new FileInfo(strimgPath + dr[0]["BM_Image"].ToString());

                    if (Fil.Exists)
                        return "/prodimages" + dr[0]["BM_Image"].ToString();
                    else
                        return "/images/noimage.gif";

                }

            }
            else
            {
                return "/images/noimage.gif";
            }
        }
        catch (Exception ex)
        {
        }
        return "/images/noimage.gif";
    }

    public String GetBrandFilterCategory_id(DataTable dt, string bmName)
    {
     
        try
        {
            if (dt != null && dt.Rows.Count > 0)
            {
                DataRow[] dr = dt.Select("BMName='" + bmName + "'");
                if (dr.Length > 0)
                {                    
                        return dr[0]["Category_ID"].ToString();
                }

            }
            else
            {
                return "";
            }
        }
        catch (Exception ex)
        {
        }
        return "";
    }
    public string GetLeftBrand()
    {
        string strrtn = "";
        if (leftBrand != "")
        {

            FindMyBrand("");
            if (Session["ProdFinder_Brand"] != null && Session["ProdFinder_Brand_Images"] != null)
            {
                strrtn = objTBWTemplateRenderProductFinder.ST_BrandL_Load((DataTable)Session["ProdFinder_Brand"], leftBrand, GetCableFilterImagePath((DataTable)Session["ProdFinder_Brand_Images"], leftBrand));
            }
        }
        else
        {
            FindMyBrand("");
            if (Session["ProdFinder_Brand"] != null && Session["ProdFinder_Brand_Images"] != null)
            {
                strrtn = objTBWTemplateRenderProductFinder.ST_BrandL_Load((DataTable)Session["ProdFinder_Brand"], "", "");
            }
        }
        return strrtn;
    }
    public string GetRightModel()
    {
        string strrtn = "";
        tempEa = Ea + "////AttribSelect=Brand = '" + leftBrand + "'";
        if (leftBrand != "" & rightModel  != "")
        {

            FindMyModel("", leftBrand);
            if (Session["ProdFinder_Model"] != null && Session["ProdFinder_Model_images"] != null)
            {
                strrtn = objTBWTemplateRenderProductFinder.ST_ModelR_Load((DataTable)Session["ProdFinder_Model"], rightModel, GetCableFilterImagePath((DataTable)Session["ProdFinder_Model_images"], rightModel), leftBrand, tempEa);
            }
        }
        else if (leftBrand != "" & rightModel == "")
        {
            FindMyModel("", leftBrand);
            if (Session["ProdFinder_Model"] != null)
            {

                strrtn = objTBWTemplateRenderProductFinder.ST_ModelR_Load((DataTable)Session["ProdFinder_Model"], "", "", leftBrand, tempEa);
            }
        }
        else
        {
            strrtn = objTBWTemplateRenderProductFinder.ST_ModelR_Load(null, "", "", "", "");
        }
        return strrtn;
    }

    public string GetBrandModelImagelist()
    {
        string strrtn = "";

        if (leftBrand  == "")
        {

            FindMyBrand("");
            if (Session["ProdFinder_Brand"] != null)
            {
                strrtn = objTBWTemplateRenderProductFinder.ST_BrandLImage_Load((DataTable)Session["ProdFinder_Brand"], "");
            }
        }
        else if (leftBrand != "")
        {

            FindMyModel("", leftBrand);
            if (Session["ProdFinder_Model"] != null && Session["ProdFinder_Model_images"] != null)
            {
                tempEa = Ea + "////AttribSelect=Brand = '" + leftBrand + "'";
                strrtn = objTBWTemplateRenderProductFinder.ST_ModelRImage_Load((DataTable)Session["ProdFinder_Model"], leftBrand, tempEa, "");
            }
        }
        return strrtn;
    }


    public string GetLeftVechicleBrand()
    {
        string strrtn = "";
        if (leftVBrand != "")
        {

            FindMyBrand("");
            if (Session["ProdFinder_Brand"] != null && Session["ProdFinder_Brand_Images"] != null)
            {
                strrtn = objTBWTemplateRenderProductFinder.ST_VechicleBrandL_Load((DataTable)Session["ProdFinder_Brand"], leftVBrand, GetCableFilterImagePath((DataTable)Session["ProdFinder_Brand_Images"], leftVBrand));
            }
        }
        else
        {
            FindMyBrand("");
            if (Session["ProdFinder_Brand"] != null && Session["ProdFinder_Brand_Images"] != null)
            {
                strrtn = objTBWTemplateRenderProductFinder.ST_VechicleBrandL_Load((DataTable)Session["ProdFinder_Brand"], "", "");
            }
        }
        return strrtn;
    }
    public string GetRightVechicleModel()
    {
        string strrtn = "";
        tempEa = Ea + "////AttribSelect=Brand = '" + leftVBrand + "'";
        if (leftVBrand != "" & rightVModel != "")
        {

            FindMyModel("", leftVBrand);
            if (Session["ProdFinder_Model"] != null && Session["ProdFinder_Model_images"] != null)
            {
                strrtn = objTBWTemplateRenderProductFinder.ST_VechicleModelR_Load((DataTable)Session["ProdFinder_Model"], rightVModel, GetCableFilterImagePath((DataTable)Session["ProdFinder_Model_images"], rightVModel), leftVBrand, tempEa);
            }
        }
        else if (leftVBrand != "" & rightVModel == "")
        {
            FindMyModel("", leftVBrand);
            if (Session["ProdFinder_Model"] != null)
            {

                strrtn = objTBWTemplateRenderProductFinder.ST_VechicleModelR_Load((DataTable)Session["ProdFinder_Model"], "", "", leftVBrand, tempEa);
            }
        }
        else
        {
            strrtn = objTBWTemplateRenderProductFinder.ST_ModelR_Load(null, "", "", "", "");
        }
        return strrtn;
    }

    public string GetVechicleBrandModelImagelist()
    {
        string strrtn = "";

        if (leftVBrand == "")
        {

            FindMyBrand("");
            if (Session["ProdFinder_Brand"] != null)
            {
                strrtn = objTBWTemplateRenderProductFinder.ST_VechicleBrandLImage_Load((DataTable)Session["ProdFinder_Brand"], "");
            }
        }
        else if (leftVBrand != "")
        {

            FindMyModel("", leftVBrand);
            if (Session["ProdFinder_Model"] != null && Session["ProdFinder_Model_images"] != null)
            {
                tempEa = Ea + "////AttribSelect=Brand = '" + leftVBrand + "'";
                strrtn = objTBWTemplateRenderProductFinder.ST_VechicleModelRImage_Load((DataTable)Session["ProdFinder_Model"], leftVBrand, tempEa, "");
            }
        }
        return strrtn;
    }
}

