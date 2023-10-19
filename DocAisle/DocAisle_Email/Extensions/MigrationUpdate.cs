using DocAisle_Email.Db;
using Microsoft.EntityFrameworkCore;

namespace DocAisle_Email.Extensions
{
    public static class MigrationUpdate
    {
        
        
            public static IApplicationBuilder UseMigration(this IApplicationBuilder app)
            {

                using (var scope = app.ApplicationServices.CreateScope())
                {
                    var _db = scope.ServiceProvider.GetRequiredService<EmailDbContext>();
                    if (_db.Database.GetPendingMigrations().Count() > 0)
                    {
                        _db.Database.Migrate();
                    }
                }

                return app;
            }
        }
    }

