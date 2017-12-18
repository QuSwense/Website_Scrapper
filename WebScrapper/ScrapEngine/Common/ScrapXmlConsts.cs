namespace ScrapEngine.Common
{
    public static class ScrapXmlConsts
    {
        #region Node Names

        public const string WebDataNodeName = "WebData";
        public const string ScrapHtmlTableNodeName = "ScrapHtmlTable";
        public const string ScrapCsvNodeName = "ScrapCsv";
        public const string ColumnNodeName = "Column";
        public const string ManipulateNodeName = "Manipulate";
        public const string SplitNodeName = "Split";
        public const string RegexNodeName = "Regex";
        public const string TrimNodeName = "Trim";
        public const string ValidateNodeName = "Validate";
        public const string ReplaceNodeName = "Replace";
        public const string RegexReplaceNodeName = "RegexReplace";
        public const string PurgeNodeName = "Purge";
        public const string DbchangeNodeName = "Dbchange";
        public const string DbchangeSelectNodeName = "Select";
        public const string HtmlDecodeNodeName = "HtmlDecode";
        public const string DbRowNodeName = "DbRow";

        #endregion Node Names

        #region Attributes

        public const string IdAttributeName = "id";
        public const string NameAttributeName = "name";
        public const string UrlAttributeName = "url";
        public const string XPathAttributeName = "xpath";
        public const string IsUniqueAttributeName = "isunique";
        public const string DataAttributeName = "data";
        public const string IndexAttributeName = "index";
        public const string PatternAttributeName = "pattern";
        public const string TableAttributeName = "table";
        public const string ColAttributeName = "col";
        public const string CardinalAttributeName = "cardinal";
        public const string InAttributeName = "in";
        public const string OutAttributeName = "out";
        public const string DelimiterAttributeName = "delimiter";
        public const string SkipFirstAttributeName = "skipfirst";
        public const string DoUpdateOnlyAttributeName = "doupdateonly";
        public const string IsEmptyOrNullAttributeName = "isemptyornull";
        public const string InnerJoinCriteriaAttributeName = "innerjoincriteria";
        public const string QueryAttributeName = "query";
        public const string IsExistsAttributeName = "isexists";
        public const string ValueAsInnerHtmlAttributeName = "valueasinnerhtml";
        public const string ReplaceAttributeName = "replace";
        public const string SplitAsStringAttributeName = "splitasstring";
        public const string IsWhitespaceAttributeName = "iswhitespace";
        public const string SkipIfValueAttributeName = "skipifvalue";
        public const string LevelAttributeName = "level";

        #endregion Attributes

        #region Values

        public static string AllValue { get { return "*"; } }
        public static string LastIndexValue { get { return "last"; } }

        #endregion Values
    }
}
