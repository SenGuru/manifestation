/**
Copyright (c) 2016 Foundation.IO (https://github.com/foundationio). All rights reserved.

This work is licensed under the terms of the BSD license.
For a copy, see <https://opensource.org/licenses/BSD-3-Clause>.
**/
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using CsvHelper;
using CsvHelper.Configuration.Attributes;
using DocumentFormat.OpenXml.EMMA;
using Framework.Infrastructure.Utils;
using Manifest.Common.Models.DB;

namespace CodeGenerator
{
    public enum ApplicableToUITypes
    {
        All,
        Pr,
        Or,
        PrAndOr,
        ATypical
    }

    public enum TableTypes
    {
        None,
        Record,
        List,
        MultiSelect,
        SelectList
    }

    public enum ControlTypes
    {
        None,
        Label,
        AutoFill,
        CheckBox,
        DatePicker,
        HyperLink,
        Link,
        MultiSelect,
        RadioButton,
        SingleSelect,
        TextBox,
        TimePicker,
        FilePicker
    }

    public enum ElementTypes
    {
        None,
        RunningNumber,
        EnumWithMetaData,
        MetaData,
        MetaDataWithChild,
        LookupList,
        String,
        Enum,
        Int,
        AddLookupList,
        Boolean,
        File,
        Date,
        Float,
        Label,
        SmallInt,
        Time,
        Long
    }

    public enum CSharpDataTypes
    {
        FKey,
        @char,
        @string,
        @long,
        @int,
        @bool,
        @float,
        DateTime,
        Time,
        File,
        Unknown
    }

    public enum TsDataTypes
    {
        fKey,
        @string,
        number,
        Date,
        boolean,
        File,
        unknown
    }


    public class Column
    {
        public string Tag { get; set; } = "";
        public bool IsPkForTable { get; set; } = false;
        public string OriginalMetaDataType { get; set; } = "";
        public string MetaDataType { get; set; } = "";
        public string TsMetaDataType { get { return MetaDataType.MakeFirstCharAsLower(); } }
        public bool PkForView { get; set; } = false;
        public string PkForViewTableName { get; set; } = "";
        public string ColumnName { get; set; }
        public string DbColumnName { get; set; }
        public string DbColumnNameWithDash { get { return DbColumnName.MakeFirstCharAsLower().SplitCamelCase("-").ToLower(); } }

        public string DbColumnNameWithoutId { get { return DbColumnName.Replace("Id", ""); } }
        public string DbColumnNameWithoutLastS { get { if (!DbColumnName.EndsWith('s')) return DbColumnName; return DbColumnName.Substring(0, DbColumnName.Length - 1); } }
        public string DbColumnNameWithoutLastSAndId { get { if (!DbColumnNameWithoutLastS.EndsWith("Id")) return DbColumnName; return DbColumnName.Substring(0, DbColumnName.Length - 2); } }
        public string DbColumnNameWithoutLastSWithDash { get { return DbColumnNameWithoutLastS.MakeFirstCharAsLower().SplitCamelCase("-").ToLower(); } }

        public string TsDbColumnNameWithoutLastS { get { if (!TsDbColumnName.EndsWith('s')) return TsDbColumnName; return TsDbColumnName.Substring(0, TsDbColumnName.Length - 1); } }
        public string DbColumnNameWithoutIds { get { return DbColumnName.Replace("Ids", ""); } }
        public string TsDbColumnNameWithoutId { get { return TsDbColumnName.Replace("Id", ""); } }
        public string TsDbColumnNameWithoutIds { get { return TsDbColumnName.Replace("Ids", ""); } }
        public string TsDbColumnName { get { return CredUIHelper.ToFirstLowerCase(DbColumnName); } }
        public string FKeyTypeName { get; set; }
        public string TsFKeyTypeName { get { return FKeyTypeName.MakeFirstCharAsLower(); } }

        public string FKeyTsType { get { return FKeyTypeName == null ? TsDataType.ToString() : FKeyTypeName; } }
        public string FKeyCsType { get { return FKeyTypeName == null ? DataType.ToString() : FKeyTypeName; } }
        public bool IsNull { get; set; }
        public int Size { get; set; }
        public CSharpDataTypes DataType { get; set; }
        public string MigrationDataType { get { return CredUIHelper.ToMigrationDataType(DataType); } }
        public TsDataTypes TsDataType { get { return CredUIHelper.ToTsDataTypes(DataType); } }
        public ControlTypes ControlType { get; set; }
        public Table LookupControlTable { get; set; }
        public string LookupControlTableName { get; set; }
        public ApplicableToUITypes ApplicableTo { get; set; }
        public ElementTypes ElementType { get; set; }
        public string Caption { get; set; }
        public Table ParentTable { get; set; }
        public bool IsArray { get; set; } = false;
        public bool IsIgnoredForWrite { get; set; } = false;
        public bool IsViewModelArray { get; set; } = false;
        public bool IsFkey { get; set; } = false;
        public List<(String FileColumn, CSharpDataTypes DataType, String TsFileColumn, TsDataTypes TsDataType)> FileRelatedColumnsForView
        {
            get
            {
                var result = new List<(String FileColumn, CSharpDataTypes DataType, String TsFileColumn, TsDataTypes TsDataType)>();
                if (ElementType == ElementTypes.File)
                {
                    var fileColumn = DbColumnName;
                    if (DbColumnName.EndsWith("Id"))
                        fileColumn = DbColumnName.Replace("Id", "");

                    result.Add((fileColumn + "FileName", CSharpDataTypes.@string, CredUIHelper.ToFirstLowerCase(fileColumn) + "FileName", TsDataTypes.@string));
                    result.Add((fileColumn + "FileSize", CSharpDataTypes.@long, CredUIHelper.ToFirstLowerCase(fileColumn) + "FileSize", TsDataTypes.number));
                }
                return result;
            }
        }

        public List<(String FileColumn, CSharpDataTypes DataType, String TsFileColumn, TsDataTypes TsDataType)> FileRelatedColumnsForCreateAndUpdate
        {
            get
            {
                var result = new List<(String FileColumn, CSharpDataTypes DataType, String TsFileColumn, TsDataTypes TsDataType)>();
                if (ElementType == ElementTypes.File)
                {
                    var fileColumn = DbColumnName;
                    if (DbColumnName.EndsWith("Id"))
                        fileColumn = DbColumnName.Replace("Id", "");

                    result.Add((fileColumn + "FileName", CSharpDataTypes.@string, CredUIHelper.ToFirstLowerCase(fileColumn) + "FileName", TsDataTypes.@string));
                    //result.Add((fileColumn + "FileData", CSharpDataTypes.File, CredUIHelper.ToFirstLowerCase(fileColumn) + "FileSize", TsDataTypes.File));
                }
                return result;
            }
        }

        public string FkeyName { get; set; }
        /*
        {
            get
            {
                if (DbColumnName == null)
                    return DbColumnName;
                var cName = DbColumnName;
                if (cName.EndsWith("Id"))
                    cName = cName.Replace("Id", "");
                return cName;
            }
        }
        */

        public string FkeyVariableType { get; set; }

        public string TsFkeyVariableType
        {
            get { return FkeyVariableType.MakeFirstCharAsLower(); }
        }
        /*
        {
            get
            {
                var r = GenerickeyVariableType();
                return $"List<{r}>";
            }
        }

        public string TsFkeyVariableType
        {
            get
            {
                var r = GenerickeyVariableType();
                return $"{r}[]";
            }
        }

        string GenerickeyVariableType()
        {
            if (DbColumnName == null)
                return DbColumnName;
            var cName = DbColumnName;
            if (cName.EndsWith("Id"))
                cName = cName.Replace("Id", "");
            if (cName.EndsWith("Ids"))
                cName = cName.Replace("Ids", "");
            return cName;
        }        

        public string TsFkeyName
        {
            get
            {
                if (FkeyName == null)
                    return FkeyName;
                return FkeyName.Substring(0, 1).ToLower() + FkeyName.Substring(1);
            }
        }
        */

        public override string ToString()
        {
            return $"Tag = {Tag} DbColumnName = {DbColumnName}, TypeName = {DataType} , IsNull = {IsNull}, Size = {Size} , ElementType = {ElementType}, ControlType = {ControlType}, FkeyVariableType = {FkeyVariableType}, FKeyTypeName = {FKeyTypeName}, FkeyName = {FkeyName},LookupControlTable = {LookupControlTable}, ApplicableTo = {ApplicableTo}, Caption = {Caption} , OriginalMetaDataType = {OriginalMetaDataType} , MetaDataType = {MetaDataType}";
        }
    }

    public class Table
    {
        public string TableName { get; set; }
        public string DbTableName { get; set; }
        public string TsDbTableName { get { return DbTableName.MakeFirstCharAsLower(); } }
        public string DbLowerTableNameWithDash { get { return DbTableName.MakeFirstCharAsLower().SplitCamelCase("-").ToLower(); } }
        public TableTypes TableType { get; set; }
        public bool IsList { get; set; } = false;
        public Foo OriginalCsvLine { get; set; }
        public bool ListName { get; set; } = false;
        public List<Column> Columns { get; set; } = new List<Column>();
        public override string ToString()
        {
            return $"TableName = {TableName}, TableType = {TableType} ";
        }
    }

    public class SubSection
    {
        public string SubSectionName { get; set; }

        public string SubSectionViewName { get { return SubSectionName.Replace(" ", "").Replace("-", "").Replace("/", ""); } }

        public string TsSubSectionViewName { get { return SubSectionViewName.MakeFirstCharAsLower(); } }

        public string SubSectionNameWithDash { get { return SubSectionName.MakeFirstCharAsLower().SplitCamelCase("-").ToLower(); } }

        public List<Table> SectionTables { get; set; } = new List<Table>();

        public List<Column> GetAllSectionTableColumns()
        {
            var result = new List<Column>();
            foreach (var c in SectionTables)
            {
                result.AddRange(c.Columns);
            }
            return result;
        }

        public List<Column> SectionColumnsX { get; set; } = new List<Column>();

        public void AddToColumns(Column col)
        {
            if (SectionColumnsX.Exists(x => x.DbColumnName == col.DbColumnName))
                return;
            SectionColumnsX.Add(col);
        }
    }

    public class Section
    {
        public string ViewClassName { get; set; }

        public string TsViewClassName { get { return ViewClassName.MakeFirstCharAsLower(); } }

        public string ViewLowerClassNameWithDash { get { return ViewClassName.MakeFirstCharAsLower().SplitCamelCase("-").ToLower(); } }
        public string SectionName { get; set; }
        public List<SubSection> SubSections { get; set; } = new List<SubSection>();
        public List<Column> AllColumns { get; set; } = new List<Column>();

        public List<Column> GetMergedColumns()
        {
            var result = new List<Column>();
            var addedTables = new List<Table>();
            foreach (var col in AllColumns)
            {
                var eTable = addedTables.FirstOrDefault(x => x.TableName == col.ParentTable.TableName);
                if (eTable != null)
                {
                    col.ParentTable = eTable;
                }
                else
                {
                    addedTables.Add(col.ParentTable);
                }
                result.Add(col);
            }
            return result;
        }

        public List<Column> GetMergedColumnsThatHasGrid()
        {
            var result = new List<Column>();
            foreach (var col in GetMergedColumns())
            {
                if (col.ElementType == ElementTypes.MetaData || col.ElementType == ElementTypes.MetaDataWithChild || col.ElementType == ElementTypes.EnumWithMetaData)
                    continue;

                if (col.IsArray)
                    result.Add(col);
            }
            return result;
        }

        public Column GetPK()
        {
            var pkCol = this.AllColumns.Where(x => x.PkForView).FirstOrDefault();
            return pkCol;
        }
    }

    public class Code
    {
        public List<Section> Sections { get; set; } = new List<Section>();
        public List<Table> Tables { get; set; } = new List<Table>();
    }

    public class Foo
    {

        [Name("SectionName")]
        public string SectionName { get; set; }

        [Name("SSubName")]
        public string SSubName { get; set; }

        [Name("SSEntityParentFKey")]
        public string SSEntityParentFKey { get; set; }

        [Name("SSubApplicableTo")]
        public string SSubApplicableTo { get; set; }

        [Name("SSubType")]
        public string SSubType { get; set; }

        [Name("SSEntity")]
        public string SSEntity { get; set; }

        [Name("SSElementName")]
        public string SSElementName { get; set; }

        [Name("Ignore")]
        public string Ignore { get; set; }

        [Name("SSElementIsRequired")]
        public string SSElementIsRequired { get; set; }

        [Name("SSElementType")]
        public string SSElementType { get; set; }

        [Name("FieldSize")]
        public string FieldSize { get; set; }

        [Name("SSElementControl")]
        public string SSElementControl { get; set; }

        [Name("old-metadataid")]
        public string OldMetadataid { get; set; }

        [Name("SSLookupEntity")]
        public string SSLookupEntity { get; set; }

        [Name("SSElementMetadataID")]
        public string SSElementMetadataID { get; set; }

        [Name("Comment")]
        public string Comment { get; set; }

        public override string ToString()
        {
            return $"SectionName = {SectionName} , SSubName = {SSubName} , SSubApplicableTo = {SSubApplicableTo} , SSubType = {SSubType} ,  SSEntity = {SSEntity} , SSElementName = {SSElementName} ";
        }
    }

    public enum CredOrOnboardEntityProfileSectionTypes
    {
        ProviderInformation = 1,
        ProfessionalIDs = 2,
        EducationAndProfessionTraining = 3,
        Specialities = 4,
        PracticeLocations = 5,
        HospitalAffilations = 6,
        Contacts = 7,
        ProfessionalLiabilityInsurance = 8,
        EmploymentInformation = 9,
        ProfessionalReferences = 10,
        Disclosure = 11,
        Attestion = 12,
        SiteVisit = 13,
        FacilityQuestionaire = 14,
        FacilityDisclosures = 15,
        TaxAffiliationWithinLocation = 16,
        InNetworkWithinLocation = 17,
        OutNetworkWithinLocation = 18,
        Accreditation = 19,
        Supportstaff = 20,
        CustomAttribute = 21,
        Contracting = 22,
    }

    public class CredUIHelper
    {
        public static List<(string prop, string repoName)> RepoMapper()
        {
            var result = new List<(string prop, string repoName)>
            {
                ("EntityProfileId", "companyRepository"),
                ("CompanyId", "companyRepository")
            };
            return result;
        }

        public static string ToTitleCase(string str)
        {
            string result = str;
            if (!string.IsNullOrEmpty(str))
            {
                var words = str.Split(' ');
                for (int index = 0; index < words.Length; index++)
                {
                    var s = words[index];
                    if (s.Length > 0)
                    {
                        words[index] = s[0].ToString().ToUpper() + s.Substring(1);
                    }
                }
                result = string.Join(" ", words);
            }
            return result;
        }

        public static string ToFirstLowerCase(string str)
        {
            string result = str;
            if (!string.IsNullOrEmpty(str) && str.Length > 0)
            {
                if (str.Length > 1)
                    result = result.Substring(0, 1).ToLower() + result.Substring(1);
                else
                    result = result.Substring(0, 1).ToLower();
            }

            if (result != null && result.StartsWith("iD", StringComparison.CurrentCulture))
            {
                result = "id" + result.Substring(2);
            }
            return result;
        }

        public static CSharpDataTypes ToCSharpDataTypes(string str, int line)
        {
            if (str == "")
                return CSharpDataTypes.FKey;
            if (str == "char")
                return CSharpDataTypes.@char;
            if (str == "string")
                return CSharpDataTypes.@string;
            if (str == "long")
                return CSharpDataTypes.@long;
            if (str == "int")
                return CSharpDataTypes.@int;
            if (str == "float")
                return CSharpDataTypes.@float;
            if (str == "datetime")
                return CSharpDataTypes.DateTime;
            if (str == "time")
                return CSharpDataTypes.Time;
            if (str == "boolean" || str == "bool")
                return CSharpDataTypes.@bool;

            throw new Exception(str + " is not a valid a Data type in line " + line.ToString());
        }

        public static string ToMigrationDataType(CSharpDataTypes dtype)
        {
            if (dtype == CSharpDataTypes.FKey)
                return "Int64";
            if (dtype == CSharpDataTypes.@char)
                return "String";
            if (dtype == CSharpDataTypes.@string)
                return "String";
            if (dtype == CSharpDataTypes.@long)
                return "Int64";
            if (dtype == CSharpDataTypes.@int)
                return "Int32";
            if (dtype == CSharpDataTypes.@float)
                return "Double";
            if (dtype == CSharpDataTypes.DateTime)
                return "DateTime";
            if (dtype == CSharpDataTypes.Time)
                return "Int64";
            if (dtype == CSharpDataTypes.@long)
                return "Int64";
            if (dtype == CSharpDataTypes.@bool)
                return "Boolean";
            return "Unknown";
        }

        public static TsDataTypes ToTsDataTypes(CSharpDataTypes dtype)
        {
            if (dtype == CSharpDataTypes.FKey)
                return TsDataTypes.fKey;
            if (dtype == CSharpDataTypes.@char)
                return TsDataTypes.@string;
            if (dtype == CSharpDataTypes.@string)
                return TsDataTypes.@string;
            if (dtype == CSharpDataTypes.@long)
                return TsDataTypes.number;
            if (dtype == CSharpDataTypes.@int)
                return TsDataTypes.number;
            if (dtype == CSharpDataTypes.@float)
                return TsDataTypes.number;
            if (dtype == CSharpDataTypes.DateTime)
                return TsDataTypes.Date;
            if (dtype == CSharpDataTypes.Time)
                return TsDataTypes.number;
            if (dtype == CSharpDataTypes.@bool)
                return TsDataTypes.boolean;
            return TsDataTypes.unknown;
        }


        public static string GetITypeFromString(string institutionType)
        {
            institutionType = ToTitleCase(institutionType);
            if (institutionType == null)
                return institutionType;

            return institutionType.Replace(" ", "").Replace("/", "Or").Replace("&", "And").Replace(",", "And").Replace("-", "").Replace(".", "").Replace("(", "").Replace(")", "").Replace("-", "");
        }

        public static ControlTypes ToControlType(Foo r, int line)
        {
            if (r.SSElementControl == null || r.SSElementControl.Trim() == "")
                return ControlTypes.None;

            string a = r.SSElementControl?.Trim().ToUpper();

            if (a == "LABEL")
                return ControlTypes.Label;

            if (a == "AUTO_FILL")
                return ControlTypes.AutoFill;

            if (a == "CHECK_BOX")
                return ControlTypes.CheckBox;

            if (a == "DATE_PICKER")
                return ControlTypes.DatePicker;

            if (a == "HYPER_LINK")
                return ControlTypes.HyperLink;

            if (a == "LINK")
                return ControlTypes.Link;

            if (a == "MULTI_SELECT")
                return ControlTypes.MultiSelect;

            if (a == "RADIO_BUTTON")
                return ControlTypes.RadioButton;

            if (a == "SINGLE_SELECT")
                return ControlTypes.SingleSelect;

            if (a == "TEXT_BOX")
                return ControlTypes.TextBox;

            if (a == "TIME_PICKER")
                return ControlTypes.TimePicker;

            if (a == "FILE_PICKER")
                return ControlTypes.TimePicker;


            throw new Exception($"Control Type  ({r.SSElementControl}) is not valid in Line - {line}");
        }

        public static ApplicableToUITypes ToApplicableTo(Foo r, int line)
        {
            if (r.SSubApplicableTo == null || r.SSubApplicableTo.Trim() == "")
                return ApplicableToUITypes.All;

            var a = r.SSubApplicableTo.Trim().ToUpper();
            if (a == "ALL" || a == "INDI/ORG/ATYP")
                return ApplicableToUITypes.All;

            if (a == "ORG")
                return ApplicableToUITypes.Or;

            if (a == "INDI")
                return ApplicableToUITypes.Pr;

            if (a == "INDI/ORG")
                return ApplicableToUITypes.PrAndOr;

            if (a == "ATYP")
                return ApplicableToUITypes.ATypical;

            throw new Exception($"ToApplicableTo  ({r.SSubApplicableTo}) is not valid in Line - {line}");
        }

        public static ElementTypes ToElementType(Foo r, int line)
        {
            if (r.SSElementType == null || r.SSElementType.Trim() == "")
                return ElementTypes.None;
            //throw new Exception("Line - " + line.ToString() + " does not have property Element Type Set with SSElementName = " + r.SSElementName);

            var a = r.SSElementType.Replace("+", "With").Replace("-", "").Replace(" ", "").Trim().ToLower();

            if (a == "runningnumber")
                return ElementTypes.RunningNumber;

            if (a == "enumwithmetadata")
                return ElementTypes.EnumWithMetaData;
            if (a == "metadata")
                return ElementTypes.MetaData;
            if (a == "metadatawithchild")
                return ElementTypes.MetaDataWithChild;
            if (a == "lookuplist")
                return ElementTypes.LookupList;
            if (a == "string")
                return ElementTypes.String;
            if (a == "enum")
                return ElementTypes.Enum;
            if (a == "int")
                return ElementTypes.Int;
            if (a == "addlookuplist")
                return ElementTypes.AddLookupList;
            if (a == "date" || a == "datetime")
                return ElementTypes.Date;
            if (a == "float" || a == "decimal" || a == "double")
                return ElementTypes.Float;
            if (a == "label")
                return ElementTypes.Label;
            if (a == "smallint")
                return ElementTypes.SmallInt;
            if (a == "time")
                return ElementTypes.Time;
            if (a == "boolean")
                return ElementTypes.Boolean;
            if (a == "file")
                return ElementTypes.File;
            if (a == "long")
                return ElementTypes.Long;

            throw new Exception($"ElementType  ({r.SSElementType} , a = {a}) is not valid in Line - {line}");
        }


        public static Column GetColumn2(Table tbl, Foo r, int line, bool isForTable = false)
        {
            var ssElementControl = "";
            var ssElementType = "";
            var ssElementIsRequired = "Y";
            var ssLookupEntity = "";

            var applicableTo = ApplicableToUITypes.All;
            var elementType = ElementTypes.Long;
            var controlType = ControlTypes.None;
            var fieldSize = "";
            var dbName = "";
            var ssName = "";
            bool isArray = false;
            bool isFKey = false;
            string fKeyTypeName = "";
            string metaDataType = "";

            //string fKeyVariable = "";

            if (!isForTable)
            {
                applicableTo = ToApplicableTo(r, line);
                elementType = ToElementType(r, line);
                controlType = ToControlType(r, line);

                ssElementControl = r.SSElementControl;
                ssElementType = r.SSElementType;
                ssElementIsRequired = r.SSElementIsRequired;
                ssLookupEntity = r.SSLookupEntity;
                fieldSize = r.FieldSize;

                ssName = r.SSElementName;

                if (r.Ignore != null && r.Ignore.Trim().ToLower() == "y")
                    return null;
            }
            else
            {
                ssName = tbl.DbTableName;
            }

            var name = ssName;
            if (name == null || name == "")
                throw new Exception($"Column is not valid in line - {line}");

            var idx = name.IndexOf(":");
            var caption = ssName;

            if (idx != -1)
            {
                name = name.Substring(0, idx);
                caption = ssName?.Substring(idx);
            }

            dbName = GetITypeFromString(name);

            if (!isForTable)
            {
                var eC = tbl.Columns.Where(x => x.ColumnName == name);
                if (eC == null)
                    return null;


                if (elementType == ElementTypes.MetaData || elementType == ElementTypes.MetaDataWithChild)
                {
                    dbName = dbName + "MetaDataId";
                    metaDataType = r.SSElementMetadataID;
                }
                else if (r.SSLookupEntity != null && r.SSLookupEntity.Trim() != "")
                {
                    dbName = dbName + "Id";
                }
                else if (dbName.ToLower().EndsWith("type") || elementType == ElementTypes.Enum || elementType == ElementTypes.EnumWithMetaData)
                {
                    dbName = dbName + "Id";
                }
            }
            else
            {
                dbName = dbName + "Id";
            }

            if (dbName.EndsWith("Id") && controlType == ControlTypes.MultiSelect)
            {
                dbName = dbName + "s";
                isArray = true;
                isFKey = true;
                fKeyTypeName = r.SSLookupEntity;
            }

            if (elementType == ElementTypes.AddLookupList || elementType == ElementTypes.LookupList)
            {
                //    dbName = dbName + "s";
                isArray = (controlType == ControlTypes.MultiSelect);
                isFKey = true;
                fKeyTypeName = r.SSLookupEntity;
            }

            if (dbName.ToLower() == "verificationproof")
            {
                dbName = "VerificationEntityProfileFileId";
                isFKey = true;
                fKeyTypeName = "EntityProfileFile";

                controlType = ControlTypes.FilePicker;
                elementType = ElementTypes.File;
            }

            var c = new Column
            {
                Tag = "1",
                ParentTable = tbl,
                DbColumnName = dbName,
                ColumnName = name,
                OriginalMetaDataType = metaDataType,
                MetaDataType = (metaDataType ?? "").Trim().Replace(" ", "").Replace("(", "").Replace(")", "").Replace("-", ""),
                Caption = caption ?? "",
                DataType = getCColumnType(controlType, elementType, line),
                //TsTypeName = getTsColumnType(ssElementControl, ssElementType, line),
                //CTypeName = getCColumnType(ssElementControl, ssElementType, line),
                IsNull = !(ssElementIsRequired == "Y" || ssElementIsRequired == "y" || ssElementIsRequired.ToLower() == "true"),
                Size = ToInt(fieldSize ?? ""),
                LookupControlTable = null,
                LookupControlTableName = ssLookupEntity,
                ApplicableTo = applicableTo,
                ElementType = elementType,
                ControlType = controlType,
                IsArray = isArray,
                IsFkey = isFKey,
                FKeyTypeName = fKeyTypeName != null ? GetITypeFromString(fKeyTypeName) : null,
            };
            return c;
        }


        public static CSharpDataTypes getCColumnType(ControlTypes controlType, ElementTypes elementType, int line)
        {
            if (elementType == ElementTypes.File || elementType == ElementTypes.AddLookupList || elementType == ElementTypes.Enum || elementType == ElementTypes.EnumWithMetaData || elementType == ElementTypes.MetaData || elementType == ElementTypes.MetaDataWithChild || elementType == ElementTypes.LookupList || elementType == ElementTypes.Enum)
                return CSharpDataTypes.@long;

            if (elementType == ElementTypes.String)
                return CSharpDataTypes.@string;

            if (elementType == ElementTypes.Int || elementType == ElementTypes.SmallInt)
                return CSharpDataTypes.@int;

            if (elementType == ElementTypes.Long)
                return CSharpDataTypes.@long;

            if (elementType == ElementTypes.Boolean)
                return CSharpDataTypes.@bool;

            if (elementType == ElementTypes.Date)
                return CSharpDataTypes.DateTime;

            if (elementType == ElementTypes.Float)
                return CSharpDataTypes.@float;

            if (elementType == ElementTypes.Time)
                return CSharpDataTypes.@int;

            if (elementType == ElementTypes.RunningNumber)
                return CSharpDataTypes.@string;

            if (elementType == ElementTypes.File)
                return CSharpDataTypes.File;

            if (elementType == ElementTypes.Label)
                return CSharpDataTypes.@string;

            if (elementType == ElementTypes.None)
                return CSharpDataTypes.Unknown;

            throw new Exception(elementType + " is not a valid a Data type in line " + line.ToString());
        }

        public static int ToInt(string numStr)
        {
            if (!int.TryParse(numStr, out int result))
                return -1;
            return result;
        }

        public static TableTypes GetTableType(string type, int lineNo)
        {
            if (type == null || type.Trim() == "")
                return TableTypes.None;

            type = type.Trim().ToLower();

            if (type == "record")
                return TableTypes.Record;

            if (type == "list")
                return TableTypes.List;

            if (type == "multiselect")
                return TableTypes.MultiSelect;

            if (type == "selectlist")
                return TableTypes.SelectList;

            throw new Exception($"Type {type} is not defined in line - {lineNo}");
        }

        public static Code GetModel(string csvFileName)
        {
            using (var reader = new StreamReader(File.OpenRead(csvFileName)))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                var records = csv.GetRecords<Foo>();
                var c = new Code();

                Section currentSection = null;
                SubSection currentSubSection = null;
                Table currentTable = null;
                var allTables = new List<Table>(); ;
                int line = 0;
                int lineToBreak = -1;
                string currentSSubType = "Record";
                foreach (var r in records)
                {
                    //Console.WriteLine(r.SectionName);
                    if (lineToBreak == line)
                        break;

                    line++;

                    if (r.Ignore != null && r.Ignore.ToLower().Trim() == "y")
                    {
                        continue;
                    }

                    if (r.SectionName?.Trim() == "" && r.SSubName?.Trim() == "" && r.SSubApplicableTo?.Trim() == "" && r.SSubType?.Trim() == "" && r.SSEntity?.Trim() == "" && r.SSElementName?.Trim() == "")
                    {
                        //throw new Exception($"Breaking in Line - {line} - {r.ToString()}");
                        break;
                    }

                    //Console.WriteLine(r.SSElementName);

                    if (r.SectionName == "Practice Location Address")
                    {
                        if (currentSection != null && currentSection.SectionName == "xyz")
                            break;
                    }

                    if (r.SectionName != "")
                    {
                        currentSection = new Section() { SectionName = r.SectionName, ViewClassName = GetITypeFromString(r.SectionName) };
                        c.Sections.Add(currentSection);
                    }

                    if (r.SSubName != "")
                    {
                        if (currentSection == null)
                            throw new Exception($"Current Section is not valid - Line - {line}");
                        currentSubSection = new SubSection() { SubSectionName = r.SSubName };
                        currentSection.SubSections.Add(currentSubSection);
                    }

                    if (!r.SSubType.IsTrimmedStringNullOrEmpty())
                    {
                        currentSSubType = r.SSubType;
                    }

                    if (r.SSEntity != "")
                    {
                        if (currentSubSection == null)
                            throw new Exception($"Current Sub Section is not valid - Line - {line}");

                        var tt = GetTableType(r.SSubType ?? "", line);

                        if (!allTables.Any(x => x.TableName == r.SSEntity))
                        {
                            var atbl = new Table() { DbTableName = GetITypeFromString(r.SSEntity), TableName = r.SSEntity, TableType = tt, OriginalCsvLine = r };
                            if (!atbl.Columns.Any(x => x.DbColumnName == GetITypeFromString(r.SSEntity) + "Id"))
                            {
                                var atblCol = new Column { Tag = "2", ParentTable = atbl, DbColumnName = GetITypeFromString(r.SSEntity) + "Id", IsPkForTable = true };
                                atbl.Columns.Add(atblCol);
                                currentSubSection.AddToColumns(atblCol);
                            }

                            if (!atbl.Columns.Any(x => x.DbColumnName == GetITypeFromString(r.SSEntityParentFKey) + "Id"))
                            {
                                var atblCol = new Column { Tag = "3", ParentTable = atbl, DbColumnName = GetITypeFromString(r.SSEntityParentFKey) + "Id", PkForView = true, PkForViewTableName = GetITypeFromString(r.SSEntityParentFKey) };
                                atbl.Columns.Add(atblCol);
                                currentSubSection.AddToColumns(atblCol);
                            }

                            Console.WriteLine("Adding Table - " + atbl.DbTableName);
                            allTables.Add(atbl);
                        }

                        currentTable = new Table() { DbTableName = GetITypeFromString(r.SSEntity), TableName = r.SSEntity, TableType = tt, IsList = (tt == TableTypes.List), OriginalCsvLine = r };

                        var parentFKey = GetITypeFromString(r.SSEntityParentFKey);
                        if (parentFKey != null && parentFKey.Trim() != "")
                        {
                            var cCol = new Column { Tag = "4", ParentTable = currentTable, DbColumnName = GetITypeFromString(r.SSEntityParentFKey) + "Id", PkForView = true, PkForViewTableName = GetITypeFromString(r.SSEntityParentFKey) };
                            currentTable.Columns.Add(cCol);
                            currentSubSection.AddToColumns(cCol);
                        }

                        currentSubSection.SectionTables.Add(currentTable);
                    }


                    if (r.SSElementName == null || r.SSElementName.Trim() == "")
                    {
                        throw new Exception($"SSElementName is not valid in Line - {line}");
                    }

                    if (currentTable == null)
                    {
                        throw new Exception($"Table name is not valid in Line - {line}");
                    }

                    var atItem = allTables.FirstOrDefault(x => x.TableName == currentTable.TableName);
                    if (atItem == null)
                    {
                        throw new Exception($"Global Table name ({currentTable.TableName}) is not valid in Line - {line}");
                    }

                    var aC = GetColumn2(atItem, r, line);
                    if (aC != null)
                    {
                        if (!atItem.Columns.Any(x => x.DbColumnName == aC.DbColumnName))
                        {
                            atItem.Columns.Add(aC);
                            currentSubSection.AddToColumns(aC);
                        }
                    }

                    var ctC = GetColumn2(currentTable, r, line);
                    if (ctC != null)
                    {
                        if (!currentTable.Columns.Any(x => x.DbColumnName == ctC.DbColumnName))
                        {
                            currentTable.Columns.Add(ctC);
                            currentSubSection.AddToColumns(ctC);
                        }
                    }

                    //If this is a multiple relationship key then we need create a temp table for this 
                    if (aC != null && aC.IsFkey && aC.IsArray)
                    {
                        var newTableName = GetITypeFromString(atItem.TableName) + GetITypeFromString(aC.DbColumnName).Replace("Ids", "");
                        var newTbl = new Table { DbTableName = newTableName, TableName = newTableName };
                        var col = new Column { Tag = "5", IsPkForTable = true, DbColumnName = newTableName + "Id" };
                        newTbl.Columns.Add(col);
                        //currentSubSection.AddToColumns(col);

                        col = new Column { Tag = "6", DbColumnName = GetITypeFromString(atItem.TableName) + "Id" };
                        newTbl.Columns.Add(col);
                        //currentSubSection.AddToColumns(col);

                        if (aC.ElementType == ElementTypes.MetaData || aC.ElementType == ElementTypes.MetaDataWithChild || aC.ElementType == ElementTypes.EnumWithMetaData)
                        {
                            col = new Column { Tag = "7", DbColumnName = "MetaDataId", IsFkey = true, FKeyTypeName = "MetaData" };
                            newTbl.Columns.Add(col);
                            //currentSubSection.AddToColumns(col);
                        }
                        else
                        {
                            col = new Column { Tag = "8", DbColumnName = GetITypeFromString((aC.FKeyTypeName ?? "") + "Id").Replace("Ids", "Id") };
                            newTbl.Columns.Add(col);
                            //currentSubSection.AddToColumns(col);
                        }

                        Console.WriteLine("Adding FKey Table - " + newTbl.DbTableName + " - " + aC.ToString());
                        allTables.Add(newTbl);
                    }
                }

                //All Table
                var seNum = 0;
                foreach (var s in c.Sections)
                {
                    seNum++;

                    //if (seNum == 1)
                    //    continue;

                    //if (seNum > 6)
                    //    break;

                    var columns = new List<Column>();

                    //Console.WriteLine(s.SectionName);
                    foreach (var sc in s.SubSections)
                    {
                        //Console.WriteLine("   " + sc.SubSectionName);
                        foreach (var t in sc.SectionTables)
                        {
                            if (t.IsList)
                            {
                                var col = new Column { Tag = "9", ParentTable = t, DbColumnName = GetITypeFromString(t.OriginalCsvLine.SSEntityParentFKey) + "Id", PkForView = true, PkForViewTableName = GetITypeFromString(t.OriginalCsvLine.SSEntityParentFKey) };
                                columns.Add(col);
                                sc.AddToColumns(col);

                                col = new Column { Tag = "10", ParentTable = t, DbColumnName = GetITypeFromString(t.OriginalCsvLine.SSEntity) + "s", IsArray = true, IsFkey = true, FKeyTypeName = GetITypeFromString(t.OriginalCsvLine.SSEntity) };
                                columns.Add(col);
                                //sc.AddToColumns(col);

                                //var cL = GetColumn2(t, null, 0, true);
                                //cL.IsFkey = true;
                                //columns.Add(cL);

                                continue;
                            }

                            if (!columns.Any(x => x.DbColumnName == t.DbTableName + "Id"))
                            {
                                //Console.WriteLine("Adding Table Column - " + t.DbTableName + "Id");
                                var col = GetColumn2(t, null, 0, true);
                                columns.Add(col);
                                //sc.AddToColumns(col);
                            }

                            //Console.WriteLine("   " + "   " + t.ToString());
                            foreach (var cl in t.Columns)
                            {
                                if (columns.Any(x => x.DbColumnName == cl.DbColumnName))
                                    continue;
                                //Console.WriteLine("   " + "   " + "   " + cl.DbColumnName + " ( " + cl.ElementType + " " + cl.LookupControlTableName + " )");

                                columns.Add(cl);
                                sc.AddToColumns(cl);
                            }
                        }
                    }

                    s.AllColumns.Clear();
                    foreach (var cx in columns)
                    {
                        if (s.AllColumns.Any(x => x.DbColumnName == cx.DbColumnName))
                            continue;

                        s.AllColumns.Add(cx);
                    }

                    //s.AllColumns = columns;

                    //Console.WriteLine(GenerateTsModels2(s, columns));
                }

                foreach (var a in allTables)
                {
                    CredUIHelper.AddMiscColumns(a);
                }

                c.Tables = allTables;
                return c;
            }
        }

        public static void AddMiscColumns(Table atbl)
        {

            var items = new List<(string, CSharpDataTypes)> { ("CreatedByUserId", CSharpDataTypes.@long), ("ModifiedByUserId", CSharpDataTypes.@long) };
            var items2 = new List<(string, CSharpDataTypes)> { ("CreatedDate", CSharpDataTypes.DateTime), ("ModifiedDate", CSharpDataTypes.DateTime) };

            var atblCol2 = new Column { Tag = "13", ParentTable = atbl, DbColumnName = "IsDeleted", DataType = CSharpDataTypes.@bool, IsIgnoredForWrite = true, IsFkey = false, IsNull = false };
            atbl.Columns.Add(atblCol2);

            foreach (var i in items)
            {
                if (!atbl.Columns.Any(x => x.DbColumnName == i.Item1))
                {
                    var atblCol = new Column { Tag = "11", ParentTable = atbl, DbColumnName = i.Item1, IsFkey = true, FKeyTypeName = GetITypeFromString("User"), PkForViewTableName = GetITypeFromString("User"), IsIgnoredForWrite = true, IsNull = true };
                    atbl.Columns.Add(atblCol);
                }
            }

            foreach (var i in items2)
            {
                if (!atbl.Columns.Any(x => x.DbColumnName == i.Item1))
                {
                    var atblCol = new Column { Tag = "12", ParentTable = atbl, DbColumnName = i.Item1, DataType = CSharpDataTypes.DateTime, IsIgnoredForWrite = true, IsNull = false };
                    atbl.Columns.Add(atblCol);
                }
            }
        }

        public static string GenerateTsModels2(Section s, List<Column> columns)
        {
            var sb = new StringBuilder();
            sb.AppendLine("export class " + s.ViewClassName + "Model {");
            foreach (var c in columns)
            {
                var arry = c.IsArray ? "[]" : "";

                if (c.PkForView)
                    sb.AppendLine($"    {c.TsDbColumnName} :  number{arry}; //({c.ParentTable.DbTableName})");
                else if (c.IsFkey)
                    sb.AppendLine($"    {c.TsDbColumnName} :  {c.FKeyTsType}{arry}; //({c.ParentTable.DbTableName})");
                else
                    sb.AppendLine($"    {c.TsDbColumnName} :{c.TsDataType}{arry}; //({c.ParentTable.DbTableName})");
            }
            sb.AppendLine("}");
            return sb.ToString();
        }

        public static string GenerateMigModels(Section s, List<Column> columns)
        {
            var sb = new StringBuilder();
            sb.AppendLine("export class " + s.ViewClassName + "Model {");
            foreach (var c in columns)
            {
                var arry = c.IsArray ? "[]" : "";

                if (c.PkForView)
                    sb.AppendLine($"    {c.TsDbColumnName} :  number{arry}; //({c.ParentTable.DbTableName})");
                else if (c.IsFkey)
                    sb.AppendLine($"    {c.TsDbColumnName} :  {c.FKeyTsType}{arry}; //({c.ParentTable.DbTableName})");
                else
                    sb.AppendLine($"    {c.TsDbColumnName} :{c.TsDataType}{arry}; //({c.ParentTable.DbTableName})");
            }
            sb.AppendLine("}");
            return sb.ToString();
        }
    }
}
