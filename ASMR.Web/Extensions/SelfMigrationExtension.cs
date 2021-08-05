//
// ASMR: Coffee Beans Management Solution
// © 2021 Pandora Karya Digital. All right reserved.
//
// Written by Danang Galuh Tegar Prasetyo [connect@danang.id]
// at 5/8/2021 7:38 AM
//
// SelfMigrationExtension.cs
//
using ASMR.Web.Data;
using Microsoft.AspNetCore.Builder;

namespace ASMR.Web.Extensions
{
    public static class SelfMigrationExtension
    {
        public static IApplicationBuilder UseSelfMigration(this IApplicationBuilder app)
        {
            SelfMigrator.Migrate(app);

            return app;
        } 
    }
}
