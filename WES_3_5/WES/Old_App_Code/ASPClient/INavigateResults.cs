namespace TradingBell.Common
{
    using System;
    using System.Collections.Generic;

    public interface INavigateResults
    {
        IList<string> getArrangeByChoices();
        bool getAtTopNode();
        IList<string> getAttributeNames();
        IList<string> getAttributeNames(int filter, int displayMode);
        int getAttributeRetrievalMethod();
        IBreadCrumbTrail getBreadCrumbTrail();
        IList<ICarveOut> getCarveOuts();
        string getCatPath();
        string getCellData(int row, int col);
        int getColumnIndex(string colName);
        string getCommentary();
        IList<string> getCommonAttributeNames(bool onlySelected);
        IList<string> getCorrectedWords();
        string getCorrection(string word);
        int getCurrentPage();
        IList<IDataDescription> getDataDescriptions();
        IList<INavigateAttribute> getDetailedAttributeValues(string attributeName);
        IList<INavigateAttribute> getDetailedAttributeValues(string attributeName, int displayMode);
        IList<INavigateCategory> getDetailedCategories();
        IList<INavigateCategory> getDetailedCategories(int nDisplayMode);
        IList<INavigateAttribute> getDetailedCommonAttributeValues(string attributeName);
        IList<INavigateAttribute> getDetailedCommonAttributeValues(string attributeName, int displayMode);
        IFeaturedProducts getFeaturedProducts();
        int getFirstItem();
        IGroupedResultSet getGroupedResult();
        int getInitialDispLimitForAttrNames();
        int getInitialDispLimitForAttrValues(string attrName);
        bool getIsCommand();
        bool getIsDrillDown();
        bool getItemsFoundByModifyingQuery();
        bool getItemsFoundWIthSecondarySearch();
        int getLastItem();
        INavigateHierarchy getNavigateHierarchy();
        int getPageCount();
        int getProductRetrievalMethod();
        bool getProductsFromGlobalSearch();
        string getQuestion();
        string getRedirect();
        IList<string> getRelaxedTerms();
        int getResultsPerPage();
        int getReturnCode();
        IResultRow getRow(int pageRow);
        string getSortOrder();
        string getSpellCorrections();
        string getSuggestedCategoryTitle();
        int getTotalItems();
        bool isGroupedResult();
        bool isInitialDispLimitedForAttrNames();
        bool isInitialDispLimitedForAttrValues(string attrName);
        bool isPresentationError();
        bool isRedirect();
    }
}

