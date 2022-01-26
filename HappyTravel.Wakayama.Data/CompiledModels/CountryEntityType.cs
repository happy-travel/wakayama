﻿// <auto-generated />
using System;
using System.Collections.Generic;
using System.Reflection;
using HappyTravel.MultiLanguage;
using HappyTravel.Wakayama.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#pragma warning disable 219, 612, 618
#nullable enable

namespace HappyTravel.Wakayama.Data.CompiledModels
{
    internal partial class CountryEntityType
    {
        public static RuntimeEntityType Create(RuntimeModel model, RuntimeEntityType? baseEntityType = null)
        {
            var runtimeEntityType = model.AddEntityType(
                "HappyTravel.Wakayama.Data.Models.Country",
                typeof(Country),
                baseEntityType);

            var id = runtimeEntityType.AddProperty(
                "Id",
                typeof(int),
                propertyInfo: typeof(Country).GetProperty("Id", BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly),
                fieldInfo: typeof(Country).GetField("<Id>k__BackingField", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly),
                valueGenerated: ValueGenerated.OnAdd,
                afterSaveBehavior: PropertySaveBehavior.Throw);
            id.AddAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            var countryCode = runtimeEntityType.AddProperty(
                "CountryCode",
                typeof(string),
                propertyInfo: typeof(Country).GetProperty("CountryCode", BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly),
                fieldInfo: typeof(Country).GetField("<CountryCode>k__BackingField", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly));

            var created = runtimeEntityType.AddProperty(
                "Created",
                typeof(DateTimeOffset),
                propertyInfo: typeof(Country).GetProperty("Created", BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly),
                fieldInfo: typeof(Country).GetField("<Created>k__BackingField", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly),
                valueGenerated: ValueGenerated.OnAdd);
            created.AddAnnotation("Relational:DefaultValueSql", "now() at time zone 'utc'");

            var globalRegionId = runtimeEntityType.AddProperty(
                "GlobalRegionId",
                typeof(int),
                propertyInfo: typeof(Country).GetProperty("GlobalRegionId", BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly),
                fieldInfo: typeof(Country).GetField("<GlobalRegionId>k__BackingField", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly));

            var modified = runtimeEntityType.AddProperty(
                "Modified",
                typeof(DateTimeOffset),
                propertyInfo: typeof(Country).GetProperty("Modified", BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly),
                fieldInfo: typeof(Country).GetField("<Modified>k__BackingField", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly),
                valueGenerated: ValueGenerated.OnAdd);
            modified.AddAnnotation("Relational:DefaultValueSql", "now() at time zone 'utc'");

            var name = runtimeEntityType.AddProperty(
                "Name",
                typeof(MultiLanguage<string>),
                propertyInfo: typeof(Country).GetProperty("Name", BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly),
                fieldInfo: typeof(Country).GetField("<Name>k__BackingField", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly));
            name.AddAnnotation("Relational:ColumnType", "jsonb");

            var key = runtimeEntityType.AddKey(
                new[] { id });
            runtimeEntityType.SetPrimaryKey(key);

            var index = runtimeEntityType.AddIndex(
                new[] { countryCode });

            var index0 = runtimeEntityType.AddIndex(
                new[] { globalRegionId });

            return runtimeEntityType;
        }

        public static RuntimeForeignKey CreateForeignKey1(RuntimeEntityType declaringEntityType, RuntimeEntityType principalEntityType)
        {
            var runtimeForeignKey = declaringEntityType.AddForeignKey(new[] { declaringEntityType.FindProperty("GlobalRegionId")! },
                principalEntityType.FindKey(new[] { principalEntityType.FindProperty("Id")! })!,
                principalEntityType,
                deleteBehavior: DeleteBehavior.Restrict,
                required: true);

            var globalRegion = declaringEntityType.AddNavigation("GlobalRegion",
                runtimeForeignKey,
                onDependent: true,
                typeof(GlobalRegion),
                propertyInfo: typeof(Country).GetProperty("GlobalRegion", BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly),
                fieldInfo: typeof(Country).GetField("<GlobalRegion>k__BackingField", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly));

            var countries = principalEntityType.AddNavigation("Countries",
                runtimeForeignKey,
                onDependent: false,
                typeof(List<Country>),
                propertyInfo: typeof(GlobalRegion).GetProperty("Countries", BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly),
                fieldInfo: typeof(GlobalRegion).GetField("<Countries>k__BackingField", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly));

            return runtimeForeignKey;
        }

        public static void CreateAnnotations(RuntimeEntityType runtimeEntityType)
        {
            runtimeEntityType.AddAnnotation("Relational:FunctionName", null);
            runtimeEntityType.AddAnnotation("Relational:Schema", null);
            runtimeEntityType.AddAnnotation("Relational:SqlQuery", null);
            runtimeEntityType.AddAnnotation("Relational:TableName", "Countries");
            runtimeEntityType.AddAnnotation("Relational:ViewName", null);
            runtimeEntityType.AddAnnotation("Relational:ViewSchema", null);

            Customize(runtimeEntityType);
        }

        static partial void Customize(RuntimeEntityType runtimeEntityType);
    }
}