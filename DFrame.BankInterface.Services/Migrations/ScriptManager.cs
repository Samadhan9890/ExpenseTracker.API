using System.Diagnostics;
using System;
using System.Configuration;
using System.IO;
using System.Linq;

namespace ExpenseTracker.Services.Migrations
{
    public static class ScriptManager
    {
        public static bool RunningUnderIIS() => Process.GetCurrentProcess().ProcessName == "w3wp" || Process.GetCurrentProcess().ProcessName == "iisexpress";

        public static FileInfo[] GetAllScripts(string migrationName, DirectionMigrationEnum sensmigration)
        {
            DirectoryInfo directory;
            string scriptDirectory = Path.Combine("Migrations", "SQLScripts", migrationName, sensmigration.ToString());

            //string initialPath = (System.Configuration.ConfigurationManager.AppSetting["ScriptMigrationPathWorker"] ?? "");

            if(RunningUnderIIS())
            {
                string binDir = string.Empty;// System.Web.Hosting.HostingEnvironment.MapPath("~/bin");
                directory= new DirectoryInfo(Path.Combine(binDir,scriptDirectory));
            }
            else
            {
                directory = new DirectoryInfo(scriptDirectory);
            }

            Console.WriteLine($"Directory Path : {directory.FullName}");

            FileInfo[] files = directory.GetFiles("*.sql", SearchOption.AllDirectories);
            Console.WriteLine($"{files.Length} scripts to apply");

            if(sensmigration == DirectionMigrationEnum.Up)
            {
                return files.OrderBy(f => f.Name).ToArray();
            }

            return files.OrderByDescending(s=>s.Name).ToArray();
        }

        public static void ExecuteScripts(string migration, DirectionMigrationEnum sensmigration, Action<string, bool, object> executeSqlFuction)
        {
            Console.WriteLine($"Applying the scripts for the migration{migration} : {sensmigration}");
            FileInfo[] scripts = GetAllScripts(migration, sensmigration);

            foreach(FileInfo file in scripts)
            {
                Console.WriteLine($"Load script : {file.Name}");
                executeSqlFuction(file.FullName, false, null);
            }

            Console.WriteLine($"End of scripts");
        }
    }
}
