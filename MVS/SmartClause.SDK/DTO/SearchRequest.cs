using System;
using System.Collections.Generic;

namespace SmartClause.SDK.DTO
{
    public class SearchSheetElement
    {
        public string Name { get; set; }
        public int Comparator { get; set; }
        public string Value { get; set; }
        public string Between { get; set; }
    }
    public class DriveSearchRequest
    {
        public string Search { get; set; }
        public bool None { get; set; }
        public bool Negotiation { get; set; }
        public bool Signature { get; set; }
        public bool Signed { get; set; }
        public bool Validated { get; set; }
        public int SignedComparator { get; set; }
        public DateTime? SignedDate { get; set; }
        public DateTime? SignedBetween { get; set; }
        public int CreationComparator { get; set; }
        public DateTime? CreationDate { get; set; }
        public DateTime? CreationBetween { get; set; }
        public string Country { get; set; }
        public string Language { get; set; }
        public string Project { get; set; }
        public string Owner { get; set; }
        public string[] Templates { get; set; }
        public string[] Contributors { get; set; }
        public List<SearchSheetElement> SheetElements { get; set; }
        public string SortField { get; set; }
        public bool IsDescending { get; set; }
        public int PageSize { get; set; }
        public int PageNumber { get; set; }
    }

    public class SearchRequest
    {
        public string Query { get; set; }
        public bool InTitle { get; set; }
        public bool InContent { get; set; }
        public bool InMetadata { get; set; }
        public List<SearchCriterionRequest> Criteria { get; set; }

        public int? NumberItems { get; set; }
        public int? SkipItems { get; set; }
    }

    public class AdvancedSearchRequest
    {
        public List<SearchRequest> Requests { get; set; }
    }
}