using System.Collections.Generic;
using FluentMigrator.SqlServer;
using Framework.Infrastructure.Interfaces.DbAccess;

namespace Manifest.Repository.Migration.Application
{
    public class BaseMigration : FluentMigrator.Migration, IDbFrameworkMigration
    {
        public override void Down()
        {
        }

        public void CreateSchema(string schema)
        {
            if (schema == null)
                schema = "";
            schema = schema.ToLower();

            if (schema == "dbo")
            {
                IfDatabase("PostgreSQL").Create.Schema(schema);
                IfDatabase("Postgres92").Create.Schema(schema);
                IfDatabase("MySql").Create.Schema(schema);
                IfDatabase("MariaDB").Create.Schema(schema);
            }
            else if (schema == "public")
            {
                IfDatabase("SqlServer").Create.Schema(schema);
                IfDatabase("MySql").Create.Schema(schema);
                IfDatabase("MariaDB").Create.Schema(schema);
            }
            else
            {
                Create.Schema(schema);
            }
        }

        public void UpdateSequenceStartValue(string schema, string tableName, string pkColumnName, long startSequence)
        {
            var pSchema = (schema == "" || schema == null) ? "public" : schema;
            var psql = $"SELECT setval(pg_get_serial_sequence(\'{pSchema}.\"{tableName}\"\', \'{pkColumnName}\') ,{startSequence}, true);";
            IfDatabase("PostgreSQL").Execute.Sql(psql);
            IfDatabase("Postgres92").Execute.Sql(psql);
        }

        public void IdentityInsertData(string tableName, object param)
        {
            IfDatabase("PostgreSQL").Insert.IntoTable(tableName).Row(param);
            IfDatabase("Postgres92").Insert.IntoTable(tableName).Row(param);
            IfDatabase("MySql").Insert.IntoTable(tableName).Row(param);
            IfDatabase("MariaDB").Insert.IntoTable(tableName).Row(param);
            IfDatabase("Sqlite").Insert.IntoTable(tableName).Row(param);
            IfDatabase("SqlServer").Insert.IntoTable(tableName).WithIdentityInsert().Row(param);
        }

        public void IdentityInsertData(string tableName, object[] paramList)
        {
            foreach (var param in paramList)
                IdentityInsertData(tableName, param);
        }

        public void IdentityInsertData(string tableName, List<object> paramList)
        {
            IdentityInsertData(tableName, paramList.ToArray());
        }

        public void InsertData(string tableName, object param)
        {
            this.Insert.IntoTable(tableName).Row(param);
        }

        public void InsertData(string tableName, object[] paramList)
        {
            foreach (var param in paramList)
                this.Insert.IntoTable(tableName).Row(param);
        }

        public void InsertData(string tableName, List<object> paramList)
        {
            InsertData(tableName, paramList.ToArray());
        }

        public override void Up()
        {
        }
    }
}
