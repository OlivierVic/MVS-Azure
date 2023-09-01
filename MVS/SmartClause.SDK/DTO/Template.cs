using System;
using System.Collections.Generic;

namespace Smartclause.SDK.DTO
{
    public class Template
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int? SignatureType { get; set; } = 1;
        public System.DateTime ModifiedTime { get; set; }
        public Nullable<System.DateTime> ArchivedTime { get; set; }
        public bool IsDraft { get; set; }
        public string Author { get; set; }
        public string Field { get; set; }
        public string Language { get; set; }
        public string Nation { get; set; }
        public string AuthorId { get; set; }
    }

    public class TemplateStats
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public int Downloaded { get; set; }
        public int Generated { get; set; }
        public int Filled { get; set; }
        public int Validated { get; set; }
        public int AverageSentEmails { get; set; }
        public int Signed { get; set; }
        public Dictionary<string, int> DirectionGenerations { get; set; }
        public TimeSpan AverageTimeForValidation { get; set; }
        public TimeSpan AverageTimeForNegociation { get; set; }
        public TimeSpan AverageTimeForSignature { get; set; }
        public int AverageCommentsGenerated { get; set; }
        public int AverageRevisionsGenerated { get; set; }
    }

    public class TemplateAvailableFields
    {
        public class Field
        {
            public string Label { get; set; }
            public string Write { get; set; }
            public string Type { get; set; }
        }

        public List<Field> AvailableFields { get; set; } = new();
    }

    public class TemplateAvailableClauses
    {
        public class DocumentField
        {
            public string id { get; set; }
            public string content { get; set; }
            public string name { get; set; }
            public string displayName { get; set; }
            public string type { get; set; }
            public string selections { get; set; }
            public string units { get; set; }
            public string computeFunction { get; set; }
        }
        public class ClauseVariante
        {
            public string text { get; set; }
            public string name { get; set; }
            public string displayName { get; set; }
            public List<DocumentField> fields { get; set; }
            public string groupName { get; set; }
            public string uid { get; set; }
        }

        public class Clause
        {
            public string Name { get; set; }
            public string DisplayName { get; set; }
            public string OriginalText { get; set; }
            public string GroupName { get; set; }
            public List<ClauseVariante> Variantes { get; set; }
            public int CurrentVarianteIndex { get; set; }
            public int Idx { get; set; }

            public string Type { get; set; }
        }

        public List<TemplateAvailableFields.Field> AvailableFields { get; set; } = new();
        public List<Clause> AvailableClauses { get; set; } = new();        
    }
}