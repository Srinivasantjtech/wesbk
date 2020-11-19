<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="OnlineCatalogue_price.ascx.cs" Inherits="WES.UC.OnlineCatalogue_price" %>
<%@ Import Namespace="TradingBell.WebCat.CatalogDB" %>
<%@ Import Namespace="TradingBell.WebCat.Helpers" %>
<%@ Import Namespace="TradingBell.WebCat.CommonServices" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="System.IO" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<div class="cat_tabs clearfixx">
    <ul class="cat_tab-links">
 
  <% 
            string newsdisp = "block";
            string catdisp = "block";
 	        int Userid = 999;
            HelperDB objHelperDB = new HelperDB();
            UserServices objUserServices = new UserServices();
            CategoryServices objCategoryServices = new CategoryServices();
            Security objSecurity = new Security();
            HelperServices objHelperServices = new HelperServices();
            if (Session["USER_ID"] != null && Session["USER_ID"] != ""){
                Userid = objHelperServices.CI(Session["USER_ID"]);
            }else if (!string.IsNullOrEmpty(Request["UserId"])){
                Userid = objHelperServices.CI(objSecurity.StringDeCrypt(Request["UserId"].ToString()));
            }
           var tab= "";
             if (!string.IsNullOrEmpty(Request["tab"]))
            {
                tab = Request["tab"].ToString();
            }
            if (tab != "" && tab=="wes_news"){
                newsdisp ="block";
                catdisp = "none";
	        }
            else if(Userid == 999){
                newsdisp ="none";
                catdisp = "block";
            }


            DataRow[] dr = null;
            DataRow[] dr3 = null;
            int pricecode = 0;
            //int count = 0;
            string WesNewsCategoryId = System.Configuration.ConfigurationManager.AppSettings["WESNEWS_CATEGORY_ID"].ToString();
            string _Action = "CATALOGUE";
            string soh = "none";
            string soh1 = "none";
            string pdfdisp = "inline-block";
            
            
            %>
        <li style="display:<%=catdisp%>" class="active"><a class="wescat_tb" href="#wes_catalogue">Catalogue & Price Lists</a></li>
        <li style="display:<%=newsdisp%>"><a class="wesnews_tb" href="#wes_news">WES NEWS Flyers (Latest Products)</a></li>
           
    </ul>
    <div class="ttab-content clearfixxx">

        <% if(catdisp=="block"){
             %>
        <div id="wes_catalogue" class="tab active">

            <!--/*---Catalogue Start ----*/ -->
            <div class="container clearfixx">

                <% 
                           
                            
                            if (Session["USER_ID"] != null && Session["USER_ID"] != "")
                            {
                                Userid = objHelperServices.CI(Session["USER_ID"]);
                            }
                            else if (!string.IsNullOrEmpty(Request["UserId"]))
                            {
                                Userid = objHelperServices.CI(objSecurity.StringDeCrypt(Request["UserId"].ToString()));
                            }
                            //string query = "select SOH from WES_CUSTOMER a join TBWC_COMPANY_BUYERS b on b.COMPANY_ID=a.WES_CUSTOMER_ID where a.WEBSITE_ID=1 and a.SOH=1 and USER_ID=" + Userid;
                            //bool SOHVAlue = objHelperDB.ExecuteSQLQueryDB(query);
                            bool SOHVAlue = objUserServices.IsUserSOH(Userid.ToString());
                            if (SOHVAlue)
                            {
                                soh = "inline";
                                soh1 = "inline-block";
                            }
                            if (Userid == 999)
                            {
                                pdfdisp = "none";
                            }
                            pricecode = objHelperDB.GetPriceCode(Userid.ToString());
                            DataSet dsCatalog = new DataSet();
                            try
                            {
                                dsCatalog = objCategoryServices.GetCatalogPDFDownload_Price(2);
                                if (dsCatalog != null)
                                {
                                    if (_Action == "CATALOGUE")
                                    {
                                        //dr = dsCatalog.Tables[0].Select("category_id in ('WES-02B','WES-03C','WES-04D','WES-05E','WES-07G','WES-06F','WES-10J','WES-01A','WES-08H','WES-11K','WES-12L','WES0830') ");
                                        if (Userid == 999)
                                        {
                                            dr = dsCatalog.Tables[0].Select("category_id in ('WES-02B','WES-03C','WES-04D','WES-05E','WES-07G','WES-06F','WES-10J','WES-01A','WES-08H','WES-11K','WES-12L') ");
                                        }
                                        else
                                        {
                                            dr = dsCatalog.Tables[0].Select("category_id in ('WES-02B','WES-03C','WES-04D','WES-05E','WES-07G','WES-06F','WES-10J','WES-01A','WES-08H','WES-11K','WES-12L','WES0830') "); 
                                        }
                                        //dr = dsCatalog.Tables[0].Select("PARENT_CATEGORY<>'" + WesNewsCategoryId + "' And CUSTOM_NUM_FIELD1=1 And CUSTOM_NUM_FIELD2=0 ");

                                    }

                                    if (dr.Length > 0)
                                    {
                                        //                                         lstrecords = new TBWDataList[dr.Length + 1];
                                        dsCatalog = new DataSet();
                                        if (_Action == "NEWS")
                                        {
                                            DataTable dtSortedTableold = dr.CopyToDataTable().Copy();


                                            DataTable dtSortedTable = dtSortedTableold.AsEnumerable()
                                                .OrderByDescending(row => row.Field<DateTime>("PublishedDate"))
                                                .CopyToDataTable();
                                            dsCatalog.Tables.Add(dtSortedTable);
                                        }
                                        else
                                        {
                                            DataTable dtSortedTableold = dr.CopyToDataTable().Copy();

                                            dtSortedTableold.Columns.Add("DESC");

                                            DataTable dtSortedTable = dtSortedTableold.AsEnumerable()
                                                .OrderBy(row => row.Field<string>("IMAGE_NAME2"))
                                                .CopyToDataTable();

                                            DataRow[] dr1 = dtSortedTable.Select("IMAGE_NAME2 <>''");
                                            if (dr1.Length > 0)
                                            {
                                                dsCatalog.Tables.Add(dr1.CopyToDataTable().Copy());
                                            }

                                            DataRow[] dr2 = dtSortedTable.Select("IMAGE_NAME2 =''");
                                            if (dr2.Length > 0)
                                            {
                                                dsCatalog.Tables[0].Merge(dr2.CopyToDataTable().Copy());
                                            }
                                            if (dr1.Length == 0 && dr2.Length == 0)
                                            {
                                                dsCatalog.Tables.Add(dtSortedTable);
                                            }
                                        }
                                    }
                                }
                            }
                            catch (Exception e)
                            { 
                            }
                    
                    %>

                <div class="clearfixx">
                    <div class="form-col-4-8">
                        <img src="/images/catalogue_icons/big-img.jpg" class="img-responsive">
                    </div>
                    <div class="form-col-4-8 margin_lft15">
                        <div class="catalogue_fullinfo">
                            <h1 class="">WES 2020  Full Catalogue</h1>

                            <div class="form-col-2-8 viewfull_btn">
                               <% if(Userid ==999){ %>
                                    <a href="/mediapub/ebook/wescat2020np/" class="btn primary-btn-blue no_radius " target="_blank">
                                    <img src="/images/bookk.png"><span>View eBook</span></a>
                                <%}else { %>
                                     <a href="/media/ebook/wescat2020/" class="btn primary-btn-blue no_radius " target="_blank">
                                    <img src="/images/bookk.png"><span>View eBook</span></a>
                                <%} %>
                                <a href="/media/pdf/wescat2020/WES_CAT2020_EXGST.pdf" class="btn primary-btn-red no_radius " target="_blank" style="display:<%=pdfdisp%>">
                                    <img src="/images/pdff.png"><span>View PDF</span></a>
                                <a id="btnOpenUrl" class="btn primary-btn-yellow no_radius " style="display:<%=soh1%>">
                                    <img src="/images/popup_icon.png"><span>Open URL</span></a>
                                <a class="fullprice_list csv" href="#" style="display:none">Full Price List CSV</a>
                                <a class="fullprice_list" href="#" style="display:none">Full Price List  XLS</a>
                            </div>


                            <div class="form-col-5-8">
                                <p>
                                    Please select to view full catalogue on the left or by sections from below.
                                    <br>
                                    For best viewing experience we recommend viewing using select eBook
                                </p>
                            </div>


                        </div>
                    </div>
                </div>

                <div class="clearfix">
                    <div class="catalogue-panelwrap mgntb20 clearfixx">

                        <%
                            //HelperDB objHelperDB = new HelperDB();
                            //HelperServices objHelperServices = new HelperServices();
                            //CategoryServices objCategoryServices = new CategoryServices();
                            //DataRow[] dr = null;
                            //int pricecode = 0;
                            int count = 0;
                            //int Userid = 999;
                            //string WesNewsCategoryId = System.Configuration.ConfigurationManager.AppSettings["WESNEWS_CATEGORY_ID"].ToString();
                            //string _Action = "CATALOGUE";
                            string countstr = string.Empty;
                            //if (Session["USER_ID"] != null)
                            //{
                            //    Userid = objHelperServices.CI(Session["USER_ID"]);
                            //}
                            //else if (!string.IsNullOrEmpty(Request["UserId"]))
                            //{
                            //    Userid = objHelperServices.CI(Request["UserId"].ToString());
                            //}
                            //pricecode = objHelperDB.GetPriceCode(Userid.ToString());
                            //DataSet dsCatalog = new DataSet();
                            try
                            {
                                //dsCatalog = objCategoryServices.GetCatalogPDFDownload_Price(2);
                                if (dsCatalog != null)
                                {
                                    //if (_Action == "CATALOGUE")
                                    //{
                                    //    dr = dsCatalog.Tables[0].Select("category_id in ('WES-02B','WES-03C','WES-04D','WES-05E','WES-07G','WES-06F','WES0830','WES-10J','WES-01A','WES-08H','WES-11K') ");
                                    //    //dr = dsCatalog.Tables[0].Select("PARENT_CATEGORY<>'" + WesNewsCategoryId + "' And CUSTOM_NUM_FIELD1=1 And CUSTOM_NUM_FIELD2=0 ");

                                    //}

                                    if (dr.Length > 0)
                                    {
                                        //                                         lstrecords = new TBWDataList[dr.Length + 1];
                                        //dsCatalog = new DataSet();
                                        //if (_Action == "NEWS")
                                        //{
                                        //    DataTable dtSortedTableold = dr.CopyToDataTable().Copy();


                                        //    DataTable dtSortedTable = dtSortedTableold.AsEnumerable()
                                        //        .OrderByDescending(row => row.Field<DateTime>("PublishedDate"))
                                        //        .CopyToDataTable();
                                        //    dsCatalog.Tables.Add(dtSortedTable);
                                        //}
                                        //else
                                        //{
                                        //    DataTable dtSortedTableold = dr.CopyToDataTable().Copy();

                                        //    dtSortedTableold.Columns.Add("DESC");
                                     
                                        //    DataTable dtSortedTable = dtSortedTableold.AsEnumerable()
                                        //        .OrderBy(row => row.Field<string>("IMAGE_NAME2"))
                                        //        .CopyToDataTable();

                                        //    DataRow[] dr1 = dtSortedTable.Select("IMAGE_NAME2 <>''");
                                        //    if (dr1.Length > 0)
                                        //    {
                                        //        dsCatalog.Tables.Add(dr1.CopyToDataTable().Copy());
                                        //    }

                                        //    DataRow[] dr2 = dtSortedTable.Select("IMAGE_NAME2 =''");
                                        //    if (dr2.Length > 0)
                                        //    {
                                        //        dsCatalog.Tables[0].Merge(dr2.CopyToDataTable().Copy());
                                        //    }
                                        //    if (dr1.Length == 0 && dr2.Length == 0)
                                        //    {
                                        //        dsCatalog.Tables.Add(dtSortedTable);
                                        //    }
                                        //}
                                        foreach (DataRow rCat in dsCatalog.Tables[0].Rows)
                                        {
                                            count = count + 1;
                                            if (count < 10)
                                            {
                                                countstr = "0"+ count.ToString();
                                            }
                                            else
                                            {
                                                countstr = count.ToString();
                                            }
                        %>


                        <%
                                            if (rCat["CATEGORY_ID"].ToString() == "WES-02B")
                                            {
                                                rCat["DESC"] = "<li>TV / SAT Distribution. Multi-Room Solutions</li><li>AV / Distribution / Conversion</li>";
                                            }
                                            else if (rCat["CATEGORY_ID"].ToString() == "WES-03C")
                                            {
                                                rCat["DESC"] = "<li>Cables / Leads. Plug / Sockets / Adaptors</li><li>Prepared interconnect</li>";
                                            }
                                            else if (rCat["CATEGORY_ID"].ToString() == "WES-04D")
                                            {
                                                rCat["DESC"] = "<li>Headphones / Microphones</li><li>Box / Wall / Ceiling Speakers</li>";
                                            }
                                            else if (rCat["CATEGORY_ID"].ToString() == "WES-05E")
                                            {
                                                rCat["DESC"] = " <li>Data Cables / Patch Leads Racks / Data</li><li>Centre. PC Cards / Peripherals / Accessories</li>";
                                            }
                                            else if (rCat["CATEGORY_ID"].ToString() == "WES-07G")
                                            {
                                                rCat["DESC"] = "<li>CCTV / IP Cameras. DVR Systems</li><li>Alarm Installation / Accessories</li>";
                                            }
                                            else if (rCat["CATEGORY_ID"].ToString() == "WES-06F")
                                            {
                                                rCat["DESC"] = "<li>CB / uhf / 2-Way Accessories. Cellular Phone</li><li>Accessories. Antenna and Installation</li>";
                                            }
                                            else if (rCat["CATEGORY_ID"].ToString() == "WES-10J")
                                            {
                                                rCat["DESC"] = "<li>Hand Tools / Service Aids / Drill Bits. Cable</li><li>Running / Glues and Adhesives. Cleaning</li><li>Solvents / Multimeters, etc.</li>";
                                            }
                                            else if (rCat["CATEGORY_ID"].ToString() == "WES-01A")
                                            {
                                                rCat["DESC"] = "<li>AC / DC Power Supplies / DC chargers.</li><li>Batteries / LED Lighting and Fittings</li><li>AC Mains Outlets and Mounting Hardware</li>";
                                            }
                                            else if (rCat["CATEGORY_ID"].ToString() == "WES-08H")
                                            {
                                                rCat["DESC"] = " <li>Vehicle Audio Solutions. Power Distribution</li><li>Mounting / Installation</li>";
                                            }
                                            else if (rCat["CATEGORY_ID"].ToString() == "WES-11K")
                                            {
                                                rCat["DESC"] = "<li>IC's, Transistors, Switches. Active / Passive</li><li>Components. Knobs and LED's / Lamps</li>";
                                            }
                                            else if (rCat["CATEGORY_ID"].ToString() == "WES-12L")
                                            {
                                                rCat["DESC"] = "<li>Phone Stylus and Belts Audio, TV, DVD, VCR</li><li>and Microwave Repair Parts</li>";
                                            }
                                            else if (rCat["CATEGORY_ID"].ToString() == "WES0830")
                                            {
                                                rCat["DESC"] = "<li>Accessories for Mobile phones such as </li><li>batteries, leather cases, high gain out door </li><li>antennae, AC and Car Chargers.</li>";
                                            }
                                            
                                            
                                            string Ebook_pdf_FileRef = System.Configuration.ConfigurationManager.AppSettings["Ebook_pdf_FileRef"].ToString();
                                            string MyFile = Server.MapPath(string.Format("attachments{0}", rCat["IMAGE_FILE2"].ToString()));

                                            string checkoldfiles = string.Empty;
                                            string ebookpath = string.Empty;
                                            string pdfpath = string.Empty;
                                            string time = string.Empty;
                                            string URL_PATH = string.Empty;
                                            if (MyFile.ToLower().Contains(Ebook_pdf_FileRef) == false)
                                            {
                                                checkoldfiles = MyFile.Replace("\\media\\pdf\\", "\\media\\wes_secure_files\\pdf\\");
                                                MyFile = MyFile.Replace("\\media\\pdf\\", "\\media\\wes_secure_files\\pdf\\");

                                            }
                                            else
                                            {

                                                checkoldfiles = MyFile;
                                            }
                                            if (System.IO.File.Exists(MyFile) || rCat["IMAGE_NAME"].ToString() != "")
                                            {
                                                //_stg_records = new StringTemplateGroup("cataloguedownload", stemplatepath);
                                                //_stmpl_records = _stg_records.GetInstanceOf("cataloguedownload" + "\\" + "cell");




                                                //cntoddeven++;
                                                //if ((cntoddeven % 2) == 0)
                                                //{
                                                //    //objHelperServices.writelog("Insite row1 "); 
                                                //    _stg_container = new StringTemplateGroup("cataloguedownload", stemplatepath);
                                                //    _stmpl_container = _stg_container.GetInstanceOf("cataloguedownload" + "\\" + "row1");
                                                //}
                                                //else
                                                //{
                                                //    //objHelperServices.writelog("Insite row"); 
                                                //    _stg_container = new StringTemplateGroup("cataloguedownload", stemplatepath);
                                                //    _stmpl_container = _stg_container.GetInstanceOf("cataloguedownload" + "\\" + "row");
                                                //}

                                                //if (System.IO.File.Exists(MyFile))
                                                //{
                                                    //_stmpl_records.SetAttribute("TBT_PDF_DESCRIPTION", rCat["IMAGE_NAME2"].ToString());

                                                    string[] file = rCat["IMAGE_FILE2"].ToString().Split(new string[] { @"/" }, StringSplitOptions.None);

                                                    pdfpath = rCat["IMAGE_FILE2"].ToString().Replace("\\", "/").Replace("wes_secure_files/", "").Replace("wes_secure_files\\", "");
                                                    //_stmpl_records.SetAttribute("PDF_FILE_NAME", file[file.Length - 1].ToString());
                                                    //_stmpl_records.SetAttribute("PDF", rCat["IMAGE_FILE2"].ToString().Replace("\\", "/").Replace("wes_secure_files/", "").Replace("wes_secure_files\\", ""));

                                                    string newfile = rCat["IMAGE_FILE2"].ToString();
                                                    if (newfile.ToLower().Contains(Ebook_pdf_FileRef) == false)
                                                    {
                                                        newfile = newfile.Replace("/media/pdf/", "/media/wes_secure_files/pdf/").Replace("\\media\\pdf\\", "\\media\\wes_secure_files\\pdf\\");
                                                    }
                                                    

                                                    if (System.IO.File.Exists(MyFile))
                                                    {
                                                        FileInfo finfo = new FileInfo(Server.MapPath(string.Format("attachments{0}", newfile)));
                                                        long FileInBytes = finfo.Length;
                                                        long FileInKB = finfo.Length / 1024;
                                                        long FileInMB = FileInKB / 1024;
                                                        time = finfo.LastWriteTime.ToString("dd-MM-yyyy");
                                                    }
                                                    //_stmpl_records.SetAttribute("PDF_SIZE", FileInKB + " KB");
                                                    //    if (_Action == "NEWS")
                                                    //    {
                                                    //        if (Convert.ToDateTime(rCat["PublishedDate"].ToString()).Year == 1900)
                                                    //        {
                                                    //            _stmpl_records.SetAttribute("PDF_DATE", "");
                                                    //        }
                                                    //        else
                                                    //        {
                                                    //            _stmpl_records.SetAttribute("PDF_DATE", Convert.ToDateTime(rCat["PublishedDate"].ToString()).ToString("yyyy-MM-dd"));
                                                    //        }
                                                    //    }
                                                    //    else
                                                    //    {
                                                    //        _stmpl_records.SetAttribute("PDF_DATE", time);
                                                    //    }
                                                    //}
                                                    //else
                                                    //{
                                                    //    _stmpl_records.SetAttribute("TBT_PDF_DESCRIPTION", "");
                                                    //    _stmpl_records.SetAttribute("PDF_FILE_NAME", "");
                                                    //}

                                                    if (_Action == "NEWS" || _Action == "CATALOGUE")
                                                    {
                                                        if (rCat["IMAGE_NAME"].ToString() != "")
                                                        {
                                                            ebookpath = objHelperServices.viewebook(rCat["IMAGE_NAME"].ToString());
                                                            ebookpath = ebookpath.Replace("wes_secure_files/", "").Replace("wes_secure_files\\", "");
                                                            if (ebookpath.Contains("www."))
                                                            {

                                                                ebookpath = ebookpath.ToLower().Replace("attachments", "").Replace("\\", "").Replace("http://", "").Replace("https://", "");
                                                                ebookpath = "http://" + ebookpath;
                                                                ebookpath = ebookpath.Replace("wes_secure_files/", "").Replace("wes_secure_files\\", "");
                                                            }
                                                            if (Userid == 999)
                                                            {
                                                                ebookpath = ebookpath.Replace("media", "mediapub").Replace("wescat2020", "wescat2020np");
                                                            }
                                                            //_stmpl_records.SetAttribute("PDF_EBOOK", ebookpath.Replace("wes_secure_files/", "").Replace("wes_secure_files\\", ""));
                                                            //_stmpl_records.SetAttribute("EBOOK_DISPLAY", true);
                                                        }
                                                        else
                                                        {
                                                            //_stmpl_records.SetAttribute("PDF_EBOOK", "");
                                                            //_stmpl_records.SetAttribute("EBOOK_DISPLAY", false);
                                                        }
                                                        string Ea_Path = "";
                                                        if (rCat["CATEGORY_PATH"].ToString().Contains("////") == true)
                                                        {
                                                            if (rCat["CATEGORY_PATH"].ToString().EndsWith("////" + rCat["CATEGORY_NAME"].ToString()) == true)
                                                            {
                                                                string[] str = rCat["CATEGORY_PATH"].ToString().Split(new string[] { "////" }, StringSplitOptions.None);
                                                                Ea_Path = "AllProducts////WESAUSTRALASIA";
                                                                for (int i = 0; i < str.Length - 1; i++)
                                                                {
                                                                    if (Ea_Path.Contains(str[i].ToString()) == false)
                                                                    {
                                                                        Ea_Path = Ea_Path + "////" + str[i].ToString();
                                                                    }
                                                                }
                                                                //$TBT_URL_PATH$?&amp;id=0&amp;pcr=$TBT_PARENT_CATEGORY_ID$&amp;cid=$TBT_CATEGORY_ID$&amp;tsb=&amp;tsm=&amp;searchstr=&amp;type=Category&amp;value=$TBT_ATTRIBUTE_VALUE$&amp;bname=&amp;byp=2&amp;Path=$EA_PATH$
                                                                URL_PATH="product_list.aspx?&amp;id=0&amp;pcr="+HttpUtility.UrlEncode(rCat["PARENT_CATEGORY"].ToString())+"&amp;cid="+ rCat["CATEGORY_ID"].ToString()+ "&amp;tsb=&amp;tsm=&amp;searchstr=&amp;type=Category&amp;value=" +HttpUtility.UrlEncode(rCat["CATEGORY_NAME"].ToString()) +"&amp;bname=&amp;byp=2&amp;Path=" +HttpUtility.UrlEncode(objSecurity.StringEnCrypt(Ea_Path));
                                                                //_stmpl_records.SetAttribute("TBT_URL_PATH", "product_list.aspx");
                                                                //_stmpl_records.SetAttribute("TBT_ISCAT", false);
                                                            }
                                                            else
                                                            {

                                                                Ea_Path = "AllProducts////WESAUSTRALASIA////" + rCat["CATEGORY_PATH"].ToString();
                                                                URL_PATH="product_list.aspx?&amp;id=0&amp;pcr="+HttpUtility.UrlEncode(rCat["PARENT_CATEGORY"].ToString())+"&amp;cid="+ rCat["CATEGORY_ID"].ToString()+ "&amp;tsb=&amp;tsm=&amp;searchstr=&amp;type=Category&amp;value=" +HttpUtility.UrlEncode(rCat["CATEGORY_NAME"].ToString()) +"&amp;bname=&amp;byp=2&amp;Path=" +HttpUtility.UrlEncode(objSecurity.StringEnCrypt(Ea_Path));
                                                                
                                                                //_stmpl_records.SetAttribute("TBT_URL_PATH", "product_list.aspx");
                                                                //_stmpl_records.SetAttribute("TBT_ISCAT", false);
                                                            }

                                                        }
                                                        else
                                                        {
                                                            //_stmpl_records.SetAttribute("TBT_URL_PATH", "categorylist.aspx");
                                                            Ea_Path = "AllProducts////WESAUSTRALASIA";
                                                            URL_PATH = "categorylist.aspx?&ld=0&cid=" + rCat["CATEGORY_ID"].ToString() + "&byp=" + HttpUtility.UrlEncode(rCat["CUSTOM_NUM_FIELD3"].ToString()) + "&path=" + HttpUtility.UrlEncode(objSecurity.StringEnCrypt(Ea_Path));
                                                            //_stmpl_records.SetAttribute("TBT_ISCAT", true);
                                                        }
                                                        %>
                                                <div class="form-col-4-8 p0">

                            <div class="ctpanel_block clearfixx">
                                <div class="clogue-no">
                                    <a href="<%=ebookpath %>" target="_blank"><%=countstr %></a><!--<i class="glyphicon glyphicon-chevron-right"></i> -->
                                </div>
                                <div class="clogue-icon ctbg<%=count%>">
                                    <a href="<%=ebookpath %>" target="_blank">
                                        <img src="/images/catalogue_icons/img<%=count%>.jpg"></a>
                                </div>
                                <div class="clogue-cnt">
                                    <div class="clogue_cat">
                                        <%Response.Write("<h3><a href=\"" + ebookpath + "\" target=\"_blank\">" + rCat["CATEGORY_NAME"].ToString() + "</a></h3>");
                                          Response.Write(rCat["DESC"].ToString());
                                           %>
                                    </div>
                                    <div class="clogue_catbtn viewbook_btn">
                                        <a href="<%=ebookpath%>" class="btn primary-btn-blue no_radius margin_top" target="_blank">View eBook</a>
                                        <a href="<%=pdfpath %>" class="btn primary-btn-red no_radius margin_top" target="_blank" style="display:<%=pdfdisp%>">View PDF</a>
                                    </div>
                                    <span class="small" style="display:none;float: left;">Section Price List <a class="small csvxl" href="/excel_csv/PriceType_<%=pricecode%>/<%=rCat["CATEGORY_ID"]%>.csv" target="_blank" style="display:<%=soh%>">CSV</a> <a class="small csvxl" href="/excel_csv/PriceType_<%=pricecode%>/<%=rCat["CATEGORY_ID"]%>.xls" target="_blank" style="display:<%=soh%>">XLS</a></span>
                                </div>

                            </div>
                        </div>
                        <%
                                                        //_stmpl_records.SetAttribute("TBT_PARENT_CATEGORY_ID", HttpUtility.UrlEncode(rCat["PARENT_CATEGORY"].ToString()));

                                                        //_stmpl_records.SetAttribute("TBT_CATEGORY_ID", HttpUtility.UrlEncode(rCat["CATEGORY_ID"].ToString()));

                                                        //_stmpl_records.SetAttribute("TBT_ATTRIBUTE_VALUE", HttpUtility.UrlEncode(rCat["CATEGORY_NAME"].ToString()));

                                                        //_stmpl_records.SetAttribute("TBT_CUSTOM_NUM_FIELD3", HttpUtility.UrlEncode(rCat["CUSTOM_NUM_FIELD3"].ToString()));

                                                        //_stmpl_records.SetAttribute("EA_PATH", HttpUtility.UrlEncode(objSecurity.StringEnCrypt(Ea_Path)));


                                                    }
                                                    if (_Action == "CATALOGUE")
                                                    {
                                                        //_stmpl_records.SetAttribute("TBT_PDF_CATALOGUE", true);
                                                        //_stmpl_records.SetAttribute("TBT_PDF_NEWS", false);
                                                        //_stmpl_records.SetAttribute("TBT_PDF_FORMS", false);
                                                    }
                                                    //else if (_Action == "NEWS")
                                                    //{
                                                    //    _stmpl_records.SetAttribute("TBT_PDF_CATALOGUE", false);
                                                    //    _stmpl_records.SetAttribute("TBT_PDF_NEWS", true);

                                                    //    _stmpl_records.SetAttribute("TBT_PDF_FORMS", false);

                                                    //}

                                                    //else if (_Action == "FORMS")
                                                    //{
                                                    //    _stmpl_records.SetAttribute("TBT_PDF_CATALOGUE", false);
                                                    //    _stmpl_records.SetAttribute("TBT_PDF_NEWS", false);
                                                    //    _stmpl_records.SetAttribute("TBT_PDF_FORMS", true);
                                                    //}


                                                    //_stmpl_container.SetAttribute("TBWDataList", _stmpl_records.ToString());



                                                    //lstrecords[counter] = new TBWDataList(_stmpl_container.ToString());
                                                    //counter++;\
                                               // }
                                            }
                                        }
                                    }
                                }
                            }
                            catch (Exception e)
                            {

                            }
                            
                        %>

                    </div>

                </div>
                <!--end row -->

                <style>
                </style>
           <%--     <div class="clearfix">
                    <div class="other_catlogue_wrap">
                        <div class="othr_catlogur_title">
                            <h1>Other Catalogues</h1>
                        </div>
                        <div class="othrCat_table_wrap">
                            <table class="othrCat_table">
                                <tr>
                                    <th>Section</th>
                                    <th>PDF Modified</th>
                                    <th>Download PDF</th>
                                    <th>Online eBook</th>
                                    <th>Browse Online</th>
                                </tr>
                                <%
                                    foreach (DataRow rCat in dsCatalog.Tables[0].Rows)
                                        {

                                            string Ebook_pdf_FileRef = System.Configuration.ConfigurationManager.AppSettings["Ebook_pdf_FileRef"].ToString();
                                            string MyFile = Server.MapPath(string.Format("attachments{0}", rCat["IMAGE_FILE2"].ToString()));

                                            string checkoldfiles = string.Empty;
                                            string ebookpath = string.Empty;
                                            string pdfpath = string.Empty;
                                            string time = string.Empty;
                                            string URL_PATH=string.Empty;
                                            long FileInKB=0;
                                            long FileInMB = 0;
                                            if (MyFile.ToLower().Contains(Ebook_pdf_FileRef) == false)
                                            {
                                                checkoldfiles = MyFile.Replace("\\media\\pdf\\", "\\media\\wes_secure_files\\pdf\\");
                                                MyFile = MyFile.Replace("\\media\\pdf\\", "\\media\\wes_secure_files\\pdf\\");

                                            }
                                            else
                                            {

                                                checkoldfiles = MyFile;
                                            }


                                            string[] file = rCat["IMAGE_FILE2"].ToString().Split(new string[] { @"/" }, StringSplitOptions.None);

                                            pdfpath = rCat["IMAGE_FILE2"].ToString().Replace("\\", "/").Replace("wes_secure_files/", "").Replace("wes_secure_files\\", "");
                                            //_stmpl_records.SetAttribute("PDF_FILE_NAME", file[file.Length - 1].ToString());
                                            //_stmpl_records.SetAttribute("PDF", rCat["IMAGE_FILE2"].ToString().Replace("\\", "/").Replace("wes_secure_files/", "").Replace("wes_secure_files\\", ""));

                                            string newfile = rCat["IMAGE_FILE2"].ToString();
                                            if (newfile.ToLower().Contains(Ebook_pdf_FileRef) == false)
                                            {
                                                newfile = newfile.Replace("/media/pdf/", "/media/wes_secure_files/pdf/").Replace("\\media\\pdf\\", "\\media\\wes_secure_files\\pdf\\");
                                            }
                                           

                                            if (System.IO.File.Exists(MyFile))
                                            {
                                                FileInfo finfo = new FileInfo(Server.MapPath(string.Format("attachments{0}", newfile)));
                                                long FileInBytes = finfo.Length;
                                                FileInKB = finfo.Length / 1024;
                                                FileInMB = FileInKB / 1024;
                                                time = finfo.LastWriteTime.ToString("dd-MM-yyyy");
                                            }
                                        
                                            if (rCat["IMAGE_NAME"].ToString() != "")
                                            {
                                                
                                                ebookpath = objHelperServices.viewebook(rCat["IMAGE_NAME"].ToString());

                                                if (ebookpath.Contains("www."))
                                                {

                                                    ebookpath = ebookpath.ToLower().Replace("attachments", "").Replace("\\", "").Replace("http://", "").Replace("https://", "");
                                                    ebookpath = "http://" + ebookpath;
                                                    ebookpath = ebookpath.Replace("wes_secure_files/", "").Replace("wes_secure_files\\", "");
                                                }

                                                //_stmpl_records.SetAttribute("PDF_EBOOK", ebookpath.Replace("wes_secure_files/", "").Replace("wes_secure_files\\", ""));
                                                //_stmpl_records.SetAttribute("EBOOK_DISPLAY", true);
                                            }
                                            else
                                            {
                                                //_stmpl_records.SetAttribute("PDF_EBOOK", "");
                                                //_stmpl_records.SetAttribute("EBOOK_DISPLAY", false);                                     
                                            }
                                                                                                string Ea_Path = "";
                                                        if (rCat["CATEGORY_PATH"].ToString().Contains("////") == true)
                                                        {
                                                            if (rCat["CATEGORY_PATH"].ToString().EndsWith("////" + rCat["CATEGORY_NAME"].ToString()) == true)
                                                            {
                                                                string[] str = rCat["CATEGORY_PATH"].ToString().Split(new string[] { "////" }, StringSplitOptions.None);
                                                                Ea_Path = "AllProducts////WESAUSTRALASIA";
                                                                for (int i = 0; i < str.Length - 1; i++)
                                                                {
                                                                    if (Ea_Path.Contains(str[i].ToString()) == false)
                                                                    {
                                                                        Ea_Path = Ea_Path + "////" + str[i].ToString();
                                                                    }
                                                                }
                                                                //$TBT_URL_PATH$?&amp;id=0&amp;pcr=$TBT_PARENT_CATEGORY_ID$&amp;cid=$TBT_CATEGORY_ID$&amp;tsb=&amp;tsm=&amp;searchstr=&amp;type=Category&amp;value=$TBT_ATTRIBUTE_VALUE$&amp;bname=&amp;byp=2&amp;Path=$EA_PATH$
                                                                URL_PATH="product_list.aspx?&amp;id=0&amp;pcr="+HttpUtility.UrlEncode(rCat["PARENT_CATEGORY"].ToString())+"&amp;cid="+ rCat["CATEGORY_ID"].ToString()+ "&amp;tsb=&amp;tsm=&amp;searchstr=&amp;type=Category&amp;value=" +HttpUtility.UrlEncode(rCat["CATEGORY_NAME"].ToString()) +"&amp;bname=&amp;byp=2&amp;Path=" +HttpUtility.UrlEncode(objSecurity.StringEnCrypt(Ea_Path));
                                                                //_stmpl_records.SetAttribute("TBT_URL_PATH", "product_list.aspx");
                                                                //_stmpl_records.SetAttribute("TBT_ISCAT", false);
                                                            }
                                                            else
                                                            {

                                                                Ea_Path = "AllProducts////WESAUSTRALASIA////" + rCat["CATEGORY_PATH"].ToString();
                                                                URL_PATH="product_list.aspx?&amp;id=0&amp;pcr="+HttpUtility.UrlEncode(rCat["PARENT_CATEGORY"].ToString())+"&amp;cid="+ rCat["CATEGORY_ID"].ToString()+ "&amp;tsb=&amp;tsm=&amp;searchstr=&amp;type=Category&amp;value=" +HttpUtility.UrlEncode(rCat["CATEGORY_NAME"].ToString()) +"&amp;bname=&amp;byp=2&amp;Path=" +HttpUtility.UrlEncode(objSecurity.StringEnCrypt(Ea_Path));
                                                                
                                                                //_stmpl_records.SetAttribute("TBT_URL_PATH", "product_list.aspx");
                                                                //_stmpl_records.SetAttribute("TBT_ISCAT", false);
                                                            }

                                                        }
                                                        else
                                                        {
                                                            //_stmpl_records.SetAttribute("TBT_URL_PATH", "categorylist.aspx");
                                                            Ea_Path = "AllProducts////WESAUSTRALASIA";
                                                            URL_PATH = "categorylist.aspx?&ld=0&cid=" + rCat["CATEGORY_ID"].ToString() + "&byp=" + HttpUtility.UrlEncode(rCat["CUSTOM_NUM_FIELD3"].ToString()) + "&path=" + HttpUtility.UrlEncode(objSecurity.StringEnCrypt(Ea_Path));
                                                            //_stmpl_records.SetAttribute("TBT_ISCAT", true);
                                                        }
                                     %>
                                <tr class="tblack">
                                    <td><%=rCat["IMAGE_NAME2"].ToString()%></td>
                                    <td><%=time%></td>
                                    <td> <a href="<%=pdfpath%>" target="_blank"><%=rCat["CATEGORY_NAME"].ToString()%>.pdf (<%=FileInMB.ToString() %> MB)</a></td>
                                    <td> <a href="<%=ebookpath%>" target="_blank">View eBook</a></td>
                                    <td> <a href="<%=URL_PATH%>" target="_blank">View Online</a></td>
                                </tr>
                                <%} %>
                           
                            </table>
                        </div>
                    </div>
                </div>--%>

            </div>
            <!--/*  Catalogue End ------*/ -->

        </div>
       <%
       }
       if(newsdisp=="block"){
        %>
        <div id="wes_news" class="tab">

                  <div class="othrCat_table_wrap">
                            <table width="850" class="border_7">
                               <%-- <tr>
                                    <th>Section</th>
                                    <th>PDF Modified</th>
                                    <th>Download PDF</th>
                                    <th>Online eBook</th>
                                    <th>Browse Online</th>
                                </tr>--%>

                                <tr>
                                    <td align="left" class="tx_7" bgcolor="#0077cc" style="color: white; font-size: 10px;height: 35px;">Published Date
                                    </td>
                                    <td align="left" class="tx_7" bgcolor="#0077cc" style="color: white; font-size: 10px;">File Description
                                    </td>

                                    <td align="right" class="tx_7" bgcolor="#0077cc" style="color: white; font-size: 10px;">PDF Size
                                    </td>
                                    <td bgcolor="#0077cc" width="10px"></td>
                                    <td align="left" class="tx_7" bgcolor="#0077cc" style="color: white; font-size: 10px;">Download PDF
                                    </td>
                                    <td align="center" class="tx_7" bgcolor="#0077cc" style="color: white; font-size: 10px;">Online eBook
                                    </td>
                                    <td align="center" class="tx_7" bgcolor="#0077cc" style="color: white; font-size: 10px;">Browse Online
                                    </td>
                                </tr>


                                <%
                                    DataSet dsCatalog1 = new DataSet();
                                    dsCatalog1 = objCategoryServices.GetCatalogPDFDownload(2);

                                    dr3 = dsCatalog1.Tables[0].Select("PARENT_CATEGORY='" + WesNewsCategoryId + "' And CUSTOM_NUM_FIELD1=1 And CUSTOM_NUM_FIELD2=0 ");

                                    DataTable dtSortedTableold1 = dr3.CopyToDataTable().Copy();


                                    DataTable dtSortedTable1 = dtSortedTableold1.AsEnumerable()
                                        .OrderByDescending(row => row.Field<DateTime>("PublishedDate"))
                                        .CopyToDataTable();
                                    dsCatalog1.Tables.Add(dtSortedTable1);
                                    
                                    foreach (DataRow rCat in dsCatalog1.Tables[1].Rows)
                                        {

                                            string Ebook_pdf_FileRef = System.Configuration.ConfigurationManager.AppSettings["Ebook_pdf_FileRef"].ToString();
                                            string MyFile = Server.MapPath(string.Format("attachments{0}", rCat["IMAGE_FILE2"].ToString()));

                                            string checkoldfiles = string.Empty;
                                            string ebookpath = "block";
                                            string ebookdisplay = string.Empty;
                                            string pdfpath = string.Empty;
                                            string pdffilename = string.Empty;
                                            string time = string.Empty;
                                            string URL_PATH=string.Empty;
                                            long FileInKB=0;
                                            long FileInMB = 0;
                                            if (MyFile.ToLower().Contains(Ebook_pdf_FileRef) == false)
                                            {
                                                checkoldfiles = MyFile.Replace("\\media\\pdf\\", "\\media\\wes_secure_files\\pdf\\");
                                                MyFile = MyFile.Replace("\\media\\pdf\\", "\\media\\wes_secure_files\\pdf\\");

                                            }
                                            else
                                            {

                                                checkoldfiles = MyFile;
                                            }


                                            string[] file = rCat["IMAGE_FILE2"].ToString().Split(new string[] { @"/" }, StringSplitOptions.None);

                                            pdfpath = rCat["IMAGE_FILE2"].ToString().Replace("\\", "/").Replace("wes_secure_files/", "").Replace("wes_secure_files\\", "");
                                            pdffilename = file[file.Length - 1].ToString();
                                            //_stmpl_records.SetAttribute("PDF_FILE_NAME", file[file.Length - 1].ToString());
                                            //_stmpl_records.SetAttribute("PDF", rCat["IMAGE_FILE2"].ToString().Replace("\\", "/").Replace("wes_secure_files/", "").Replace("wes_secure_files\\", ""));

                                            string newfile = rCat["IMAGE_FILE2"].ToString();
                                            if (newfile.ToLower().Contains(Ebook_pdf_FileRef) == false)
                                            {
                                                newfile = newfile.Replace("/media/pdf/", "/media/wes_secure_files/pdf/").Replace("\\media\\pdf\\", "\\media\\wes_secure_files\\pdf\\");
                                            }
                                           

                                            if (System.IO.File.Exists(MyFile))
                                            {
                                                FileInfo finfo = new FileInfo(Server.MapPath(string.Format("attachments{0}", newfile)));
                                                long FileInBytes = finfo.Length;
                                                FileInKB = finfo.Length / 1024;
                                                FileInMB = FileInKB / 1024;
                                                //time = finfo.LastWriteTime.ToString("dd-MM-yyyy");
                                                if (Convert.ToDateTime(rCat["PublishedDate"].ToString()).Year == 1900)
                                                {
                                                    time = "";
                                                }
                                                else
                                                {
                                                    time = Convert.ToDateTime(rCat["PublishedDate"].ToString()).ToString("yyyy-MM-dd");
                                                }
                                            //}
                                        
                                            if (rCat["IMAGE_NAME"].ToString() != "")
                                            {
                                                
                                                ebookpath = objHelperServices.viewebook(rCat["IMAGE_NAME"].ToString());
                                                ebookpath = ebookpath.Replace("wes_secure_files/", "").Replace("wes_secure_files\\", "");
                                                if (ebookpath.Contains("www."))
                                                {

                                                    ebookpath = ebookpath.ToLower().Replace("attachments", "").Replace("\\", "").Replace("http://", "").Replace("https://", "");
                                                    ebookpath = "http://" + ebookpath;
                                                    ebookpath = ebookpath.Replace("wes_secure_files/", "").Replace("wes_secure_files\\", "");
                                                }
                                                ebookdisplay = "block";
                                                //_stmpl_records.SetAttribute("PDF_EBOOK", ebookpath.Replace("wes_secure_files/", "").Replace("wes_secure_files\\", ""));
                                                //_stmpl_records.SetAttribute("EBOOK_DISPLAY", true);
                                            }
                                            else
                                            {
                                                ebookpath = "";
                                                ebookdisplay = "none";
                                                //_stmpl_records.SetAttribute("PDF_EBOOK", "");
                                                //_stmpl_records.SetAttribute("EBOOK_DISPLAY", false);                                     
                                            }
                                                       string Ea_Path = "";
                                                        if (rCat["CATEGORY_PATH"].ToString().Contains("////") == true)
                                                        {
                                                            if (rCat["CATEGORY_PATH"].ToString().EndsWith("////" + rCat["CATEGORY_NAME"].ToString()) == true)
                                                            {
                                                                string[] str = rCat["CATEGORY_PATH"].ToString().Split(new string[] { "////" }, StringSplitOptions.None);
                                                                Ea_Path = "AllProducts////WESAUSTRALASIA";
                                                                for (int i = 0; i < str.Length - 1; i++)
                                                                {
                                                                    if (Ea_Path.Contains(str[i].ToString()) == false)
                                                                    {
                                                                        Ea_Path = Ea_Path + "////" + str[i].ToString();
                                                                    }
                                                                }
                                                                //$TBT_URL_PATH$?&amp;id=0&amp;pcr=$TBT_PARENT_CATEGORY_ID$&amp;cid=$TBT_CATEGORY_ID$&amp;tsb=&amp;tsm=&amp;searchstr=&amp;type=Category&amp;value=$TBT_ATTRIBUTE_VALUE$&amp;bname=&amp;byp=2&amp;Path=$EA_PATH$
                                                                URL_PATH="product_list.aspx?&amp;id=0&amp;pcr="+HttpUtility.UrlEncode(rCat["PARENT_CATEGORY"].ToString())+"&amp;cid="+ rCat["CATEGORY_ID"].ToString()+ "&amp;tsb=&amp;tsm=&amp;searchstr=&amp;type=Category&amp;value=" +HttpUtility.UrlEncode(rCat["CATEGORY_NAME"].ToString()) +"&amp;bname=&amp;byp=2&amp;Path=" +HttpUtility.UrlEncode(objSecurity.StringEnCrypt(Ea_Path));
                                                                //_stmpl_records.SetAttribute("TBT_URL_PATH", "product_list.aspx");
                                                                //_stmpl_records.SetAttribute("TBT_ISCAT", false);
                                                            }
                                                            else
                                                            {

                                                                Ea_Path = "AllProducts////WESAUSTRALASIA////" + rCat["CATEGORY_PATH"].ToString();
                                                                URL_PATH="product_list.aspx?&amp;id=0&amp;pcr="+HttpUtility.UrlEncode(rCat["PARENT_CATEGORY"].ToString())+"&amp;cid="+ rCat["CATEGORY_ID"].ToString()+ "&amp;tsb=&amp;tsm=&amp;searchstr=&amp;type=Category&amp;value=" +HttpUtility.UrlEncode(rCat["CATEGORY_NAME"].ToString()) +"&amp;bname=&amp;byp=2&amp;Path=" +HttpUtility.UrlEncode(objSecurity.StringEnCrypt(Ea_Path));
                                                                
                                                                //_stmpl_records.SetAttribute("TBT_URL_PATH", "product_list.aspx");
                                                                //_stmpl_records.SetAttribute("TBT_ISCAT", false);
                                                            }

                                                        }
                                                        else
                                                        {
                                                            //_stmpl_records.SetAttribute("TBT_URL_PATH", "categorylist.aspx");
                                                            Ea_Path = "AllProducts////WESAUSTRALASIA";
                                                            URL_PATH = "categorylist.aspx?&ld=0&cid=" + rCat["CATEGORY_ID"].ToString() + "&byp=" + HttpUtility.UrlEncode(rCat["CUSTOM_NUM_FIELD3"].ToString()) + "&path=" + HttpUtility.UrlEncode(objSecurity.StringEnCrypt(Ea_Path));
                                                            //_stmpl_records.SetAttribute("TBT_ISCAT", true);
                                                        }
                                     %>
                               <%-- <tr class="tblack">
                                    <td><%=rCat["IMAGE_NAME2"].ToString()%></td>
                                    <td><%=time%></td>
                                    <td> <a href="<%=pdfpath%>" target="_blank"><%=rCat["CATEGORY_NAME"].ToString()%>.pdf (<%=FileInMB.ToString()%> MB)</a></td>
                                    <td> <a href="<%=ebookpath%>" target="_blank">View eBook</a></td>
                                    <td> <a href="<%=URL_PATH%>" target="_blank">View Online</a></td>
                                </tr>--%>


                                   <tr>
                                    <td align="left"><%=time%></td>
                                    <td align="left"><%=rCat["IMAGE_NAME2"].ToString()%></td>
                                    <td align="right"><%=FileInKB.ToString()%> KB</td>
                                    <td></td>
                                    <td align="left">
                                        <a href="<%=pdfpath%>" target="_blank" style="color: Blue"><%=pdffilename.ToString()%></a>
                                    </td>
                                    <td align="center">
                                        <a href="<%=ebookpath%>" target="_blank" style="color: Blue;display :<%=ebookdisplay%>">View eBook</a>
                                    </td>
                                    <td align="center"> 
                                         <a href="<%=URL_PATH%>" target="_blank" style="color: Blue">Browse Online</a>
                                    </td>

                                </tr>
                           

                                <%}} %>

                             
                            </table>
                        </div>
                  


            <%--<p>
                Wes News and Latest Products
                <br>
                <br>
            </p>--%>
            <p></p>
        </div>
        <%} %>
    </div>
</div>

<asp:Button ID="btnTemp" runat="server" Style="display: none;" />

<cc1:ModalPopupExtender ID="mpeMessageBox" runat="server" DynamicServicePath="" Enabled="True"
    TargetControlID="btnTemp" PopupControlID="pnlMessageBox" BackgroundCssClass="modal"
    PopupDragHandleControlID="pnlMessageBox" CancelControlID="btnCancel" BehaviorID="mpeFirmMessageBox">
</cc1:ModalPopupExtender>

<asp:Panel ID="pnlMessageBox" runat="server" Style="display: none; text-align: center; padding-bottom: 20px; width: 1000px"
    class="MessageBoxPopUp">



    <%--    <asp:TextBox ID="txtUrl" runat="server" MaxLength="40" Width="242px" CssClass="input_dr" />
        <input type="button" id="Button1"  class="btnbuy2 button btngreen" onclick="closeModelPopup(this)"  value="Ok" />--%>

    <%--   <table border="0" cellpadding="0" cellspacing="0" width="100%">

            <tr class="MessageBoxHeader" style="height: 17px;">

                <td colspan="2">

                    <asp:Image ID="imgInfo" runat="server" ImageUrl="~/Images/info_msg.png" />
                </td>

                <td align="right" style="padding: 2px 2px 0px 0px;">

                </td>

            </tr>
            <tr>
                <td colspan="2" style="height: 5px;"></td>
            </tr>
            <tr>
                <td class="MessageBoxData" colspan="2" style="width: 100%; padding: 15px; text-align: center;">
                    <asp:Label ID="lblMessage" runat="server"></asp:Label>
                </td>
            </tr>
            <tr style="vertical-align: bottom; height: 20px; padding: 0px 5px 5px 0px;">
                <td align="right" style="width: 56px; text-align: center; margin: 0 auto; margin-top: -15px; display: block;">
                    <input type="button" id="Button2" class="btnbuy2 button btngreen" onclick="closeModelPopup(this)" value="Ok" />
                </td>
            </tr>
        </table>--%>

    <div class="modal-dialog custom">
        <div class="modal-content">
            <div class="close-selected" style="margin: 9px;">
                <%--<asp:ImageButton ID="ImageButton3" runat="server" OnClientClick="closeModelPopup();" style="margin:10px"/>--%>
                <img src="../images/close2.png" onclick="closeModelPopup();" />
            </div>
            <div class="modal-header">
                <h4 id="H4" class="text-center">COPY URL</h4>
            </div>
            <div class="modal-body">
                <asp:TextBox ID="txtUrl" runat="server" Style="height: 25px; width: 500px; float: left;"></asp:TextBox>
                <a id="idCopy" class="btn primary-btn-red no_radius" style="height: 25px; width: 100px; line-height: 25px;"><span style="font-size: 12px;">Copy Text</span></a>
            </div>
            <div class="modal-footer clear border_top_none">
                <%--<asp:Button ID="Button1" runat="server" Text="Close" CssClass="btn button-close" OnClientClick="closeModelPopup();" CausesValidation="false" />--%>
                <%--<a id="A1" class="btn primary-btn-red no_radius ">
                    <img src="/images/pdff.png"><span>Open URL</span></a>--%>
            </div>
        </div>
    </div>




</asp:Panel>



<script type="text/javascript">
    jQuery(document).ready(function () {
        var url = $(location).attr('href'),
            parts = url.split("/"),
            last_part = parts[parts.length - 1];

        if (last_part == "wesnews") {
            jQuery('#wes_news').show().siblings().hide();
            jQuery('.cat_tabs .cat_tab-links .wesnews_tb').parent('li').addClass('active').siblings().removeClass('active');
        }

        var paramlength = window.location.href.slice(window.location.href.indexOf('?') + 1).split('&').length;
        if (paramlength > 0) {
            var name = GetParameterValues('tab');
            if (name != undefined && name != "") {
                jQuery('#' + name).show().siblings().hide();
                if (name == "wes_news") {
                    jQuery('.cat_tabs .cat_tab-links .wesnews_tb').parent('li').addClass('active').siblings().removeClass('active');
                }
                else {
                    jQuery('.cat_tabs .cat_tab-links .wescat_tb').parent('li').addClass('active').siblings().removeClass('active');
                }
            }
        }

        function GetParameterValues(param) {
            var url = window.location.href.slice(window.location.href.indexOf('?') + 1).split('&');
            for (var i = 0; i < url.length; i++) {
                var urlparam = url[i].split('=');
                if (urlparam[0] == param) {
                    return urlparam[1];
                }
            }
        }


        jQuery('.cat_tabs .cat_tab-links a').click(function (e) {
            var currentAttrValue = jQuery(this).attr('href');
            // Show/Hide Tabs
            jQuery('.cat_tabs ' + currentAttrValue).show().siblings().hide();
            // Change/remove current tab to active
            jQuery(this).parent('li').addClass('active').siblings().removeClass('active');
            e.preventDefault();
        });

        $("#btnOpenUrl").click(function () {
            console.log("open url buttton clicked");
            $find('mpeFirmMessageBox').show();
            //    Showpopup();
        });
        $('#idCopy').click(function () {
            var clipboardText = "";

            clipboardText = $('#ctl00_maincontent_OnlineCatalogue_price1_txtUrl').val();

            copyToClipboard(clipboardText);
            alert("URL Copied to Clipboard");
            $find('mpeFirmMessageBox').hide();
        });

        function copyToClipboard(text) {

            var textArea = document.createElement("textarea");
            textArea.value = text;
            document.body.appendChild(textArea);

            textArea.select();

            try {
                var successful = document.execCommand('copy');
                var msg = successful ? 'successful' : 'unsuccessful';
                console.log('Copying text command was ' + msg);
            } catch (err) {
                console.log('Oops, unable to copy');
            }

            document.body.removeChild(textArea);
        }

    });


    function closeModelPopup() {
        $find('mpeFirmMessageBox').hide();
    }
</script>
