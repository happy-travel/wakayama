// <auto-generated />
using System;
using System.Reflection;
using HappyTravel.MultiLanguage;
using HappyTravel.Wakayama.Data.Models;
using Microsoft.EntityFrameworkCore.Metadata;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#pragma warning disable 219, 612, 618
#nullable enable

namespace HappyTravel.Wakayama.Data.CompiledModels
{
    internal partial class GlobalRegionEntityType
    {
        public static RuntimeEntityType Create(RuntimeModel model, RuntimeEntityType? baseEntityType = null)
        {
            var runtimeEntityType = model.AddEntityType(
                "HappyTravel.Wakayama.Data.Models.GlobalRegion",
                typeof(GlobalRegion),
                baseEntityType);

            var id = runtimeEntityType.AddProperty(
                "Id",
                typeof(int),
                propertyInfo: typeof(GlobalRegion).GetProperty("Id", BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly),
                fieldInfo: typeof(GlobalRegion).GetField("<Id>k__BackingField", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly),
                valueGenerated: ValueGenerated.OnAdd,
                afterSaveBehavior: PropertySaveBehavior.Throw);
            id.AddAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            var created = runtimeEntityType.AddProperty(
                "Created",
                typeof(DateTimeOffset),
                propertyInfo: typeof(GlobalRegion).GetProperty("Created", BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly),
                fieldInfo: typeof(GlobalRegion).GetField("<Created>k__BackingField", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly),
                valueGenerated: ValueGenerated.OnAdd);
            created.AddAnnotation("Relational:DefaultValueSql", "now() at time zone 'utc'");

            var modified = runtimeEntityType.AddProperty(
                "Modified",
                typeof(DateTimeOffset),
                propertyInfo: typeof(GlobalRegion).GetProperty("Modified", BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly),
                fieldInfo: typeof(GlobalRegion).GetField("<Modified>k__BackingField", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly),
                valueGenerated: ValueGenerated.OnAdd);
            modified.AddAnnotation("Relational:DefaultValueSql", "now() at time zone 'utc'");

            var name = runtimeEntityType.AddProperty(
                "Name",
                typeof(MultiLanguage<string>),
                propertyInfo: typeof(GlobalRegion).GetProperty("Name", BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly),
                fieldInfo: typeof(GlobalRegion).GetField("<Name>k__BackingField", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly));
            name.AddAnnotation("Relational:ColumnType", "jsonb");

            var key = runtimeEntityType.AddKey(
                new[] { id });
            runtimeEntityType.SetPrimaryKey(key);

            return runtimeEntityType;
        }

        public static void CreateAnnotations(RuntimeEntityType runtimeEntityType)
        {
            runtimeEntityType.AddAnnotation("Relational:FunctionName", null);
            runtimeEntityType.AddAnnotation("Relational:Schema", null);
            runtimeEntityType.AddAnnotation("Relational:SqlQuery", null);
            runtimeEntityType.AddAnnotation("Relational:TableName", "GlobalRegions");
            runtimeEntityType.AddAnnotation("Relational:ViewName", null);
            runtimeEntityType.AddAnnotation("Relational:ViewSchema", null);

            Customize(runtimeEntityType);
        }

        static partial void Customize(RuntimeEntityType runtimeEntityType);
    }
}
