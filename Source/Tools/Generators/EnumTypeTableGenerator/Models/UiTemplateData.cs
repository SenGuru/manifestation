using System;
using CodeGenerator;

namespace EnumTypeTableGenerator.Models
{
    public class CredUiTemplateData
    {
        public Code Code { get; set; }

        public Section CSection { get; set; }

        public Table CTable { get; set; }
    }

    public class UiTemplateData
    {
        public bool SearchHasIdOrIds { get; set; }

        public string SplitetCapitalizeEntityName { get; set; }

        public string CapitalizeEntityName { get; set; }

        public string SmallEntityName { get; set; }

        public string SmallDashEntityName { get; set; }

        public Type EntityType { get; set; }

        public Type SearchCriteriaType { get; set; }

        public string CapitalizeSearchName { get; set; }

        public string SmallSearchName { get; set; }
    }
}