// <auto-generated />
using System;
using System.Collections.Generic;
using System.Reflection;
using HappyTravel.Wakayama.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#pragma warning disable 219, 612, 618
#nullable enable

namespace HappyTravel.Wakayama.Data.CompiledModels
{
    internal partial class LocalityProvinceRelationEntityType
    {
        public static RuntimeEntityType Create(RuntimeModel model, RuntimeEntityType? baseEntityType = null)
        {
            var runtimeEntityType = model.AddEntityType(
                "HappyTravel.Wakayama.Data.Models.LocalityProvinceRelation",
                typeof(LocalityProvinceRelation),
                baseEntityType);

            var id = runtimeEntityType.AddProperty(
                "Id",
                typeof(int),
                propertyInfo: typeof(LocalityProvinceRelation).GetProperty("Id", BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly),
                fieldInfo: typeof(LocalityProvinceRelation).GetField("<Id>k__BackingField", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly),
                valueGenerated: ValueGenerated.OnAdd,
                afterSaveBehavior: PropertySaveBehavior.Throw);
            id.AddAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            var created = runtimeEntityType.AddProperty(
                "Created",
                typeof(DateTimeOffset),
                propertyInfo: typeof(LocalityProvinceRelation).GetProperty("Created", BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly),
                fieldInfo: typeof(LocalityProvinceRelation).GetField("<Created>k__BackingField", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly),
                valueGenerated: ValueGenerated.OnAdd);
            created.AddAnnotation("Relational:DefaultValueSql", "now() at time zone 'utc'");

            var localityId = runtimeEntityType.AddProperty(
                "LocalityId",
                typeof(int),
                propertyInfo: typeof(LocalityProvinceRelation).GetProperty("LocalityId", BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly),
                fieldInfo: typeof(LocalityProvinceRelation).GetField("<LocalityId>k__BackingField", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly));

            var modified = runtimeEntityType.AddProperty(
                "Modified",
                typeof(DateTimeOffset),
                propertyInfo: typeof(LocalityProvinceRelation).GetProperty("Modified", BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly),
                fieldInfo: typeof(LocalityProvinceRelation).GetField("<Modified>k__BackingField", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly),
                valueGenerated: ValueGenerated.OnAdd);
            modified.AddAnnotation("Relational:DefaultValueSql", "now() at time zone 'utc'");

            var provinceId = runtimeEntityType.AddProperty(
                "ProvinceId",
                typeof(int),
                propertyInfo: typeof(LocalityProvinceRelation).GetProperty("ProvinceId", BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly),
                fieldInfo: typeof(LocalityProvinceRelation).GetField("<ProvinceId>k__BackingField", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly));

            var relatedLanguageCodes = runtimeEntityType.AddProperty(
                "RelatedLanguageCodes",
                typeof(List<string>),
                propertyInfo: typeof(LocalityProvinceRelation).GetProperty("RelatedLanguageCodes", BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly),
                fieldInfo: typeof(LocalityProvinceRelation).GetField("<RelatedLanguageCodes>k__BackingField", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly));

            var key = runtimeEntityType.AddKey(
                new[] { id });
            runtimeEntityType.SetPrimaryKey(key);

            var index = runtimeEntityType.AddIndex(
                new[] { localityId });

            var index0 = runtimeEntityType.AddIndex(
                new[] { provinceId });

            return runtimeEntityType;
        }

        public static RuntimeForeignKey CreateForeignKey1(RuntimeEntityType declaringEntityType, RuntimeEntityType principalEntityType)
        {
            var runtimeForeignKey = declaringEntityType.AddForeignKey(new[] { declaringEntityType.FindProperty("LocalityId")! },
                principalEntityType.FindKey(new[] { principalEntityType.FindProperty("Id")! })!,
                principalEntityType,
                deleteBehavior: DeleteBehavior.Restrict,
                required: true);

            var locality = declaringEntityType.AddNavigation("Locality",
                runtimeForeignKey,
                onDependent: true,
                typeof(Locality),
                propertyInfo: typeof(LocalityProvinceRelation).GetProperty("Locality", BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly),
                fieldInfo: typeof(LocalityProvinceRelation).GetField("<Locality>k__BackingField", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly));

            var localityAndProvinceRelations = principalEntityType.AddNavigation("LocalityAndProvinceRelations",
                runtimeForeignKey,
                onDependent: false,
                typeof(List<LocalityProvinceRelation>),
                propertyInfo: typeof(Locality).GetProperty("LocalityAndProvinceRelations", BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly),
                fieldInfo: typeof(Locality).GetField("<LocalityAndProvinceRelations>k__BackingField", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly));

            return runtimeForeignKey;
        }

        public static RuntimeForeignKey CreateForeignKey2(RuntimeEntityType declaringEntityType, RuntimeEntityType principalEntityType)
        {
            var runtimeForeignKey = declaringEntityType.AddForeignKey(new[] { declaringEntityType.FindProperty("ProvinceId")! },
                principalEntityType.FindKey(new[] { principalEntityType.FindProperty("Id")! })!,
                principalEntityType,
                deleteBehavior: DeleteBehavior.Restrict,
                required: true);

            var province = declaringEntityType.AddNavigation("Province",
                runtimeForeignKey,
                onDependent: true,
                typeof(Province),
                propertyInfo: typeof(LocalityProvinceRelation).GetProperty("Province", BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly),
                fieldInfo: typeof(LocalityProvinceRelation).GetField("<Province>k__BackingField", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly));

            var localityAndProvinceRelations = principalEntityType.AddNavigation("LocalityAndProvinceRelations",
                runtimeForeignKey,
                onDependent: false,
                typeof(List<LocalityProvinceRelation>),
                propertyInfo: typeof(Province).GetProperty("LocalityAndProvinceRelations", BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly),
                fieldInfo: typeof(Province).GetField("<LocalityAndProvinceRelations>k__BackingField", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly));

            return runtimeForeignKey;
        }

        public static void CreateAnnotations(RuntimeEntityType runtimeEntityType)
        {
            runtimeEntityType.AddAnnotation("Relational:FunctionName", null);
            runtimeEntityType.AddAnnotation("Relational:Schema", null);
            runtimeEntityType.AddAnnotation("Relational:SqlQuery", null);
            runtimeEntityType.AddAnnotation("Relational:TableName", "LocalityProvinceRelation");
            runtimeEntityType.AddAnnotation("Relational:ViewName", null);
            runtimeEntityType.AddAnnotation("Relational:ViewSchema", null);

            Customize(runtimeEntityType);
        }

        static partial void Customize(RuntimeEntityType runtimeEntityType);
    }
}
