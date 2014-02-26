using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Data.Entity;
using System.Data.Entity.Migrations;

namespace NewsWcfService
{
    // ПРИМЕЧАНИЕ. Команду "Переименовать" в меню "Рефакторинг" можно использовать для одновременного изменения имени класса "NewsService" в коде, SVC-файле и файле конфигурации.
    public class NewsService : INewsHandler
    {
        NewsCollection NewsFromXML = NewsSerializer.Deserialize();

        public int NewsCount()
        {
            return NewsFromXML.New.Length;
        }

        public List<New> GetNewsFromTable()
        {
            using (var db = new NewsContext())
            {
                //Database.SetInitializer<NewsContext>(null);
                var query = from b in db.News
                            select b;
                List<New> ReceivedNews = new List<New> { };
                foreach (var item in query)
                {
                    ReceivedNews.Add(item);
                }
                return ReceivedNews;
            }
        }

        public New GetNewByNumber(int number)
        {

            if (NewsFromXML.New.Length > number)
            {
                return NewsFromXML.New[number];
            }

            return null;
        }

        public void SaveNewByNumber(int number)
        {
            using (var db = new NewsContext())
            {
                New newToSave = null;
                if (NewsFromXML.New.Length > number)
                {
                    newToSave = NewsFromXML.New[number];
                    db.News.Add(newToSave);
                    db.SaveChanges();
                }
            }
        }
    }

    public class NewsContext : DbContext
    {
        public static string ConnectionStringName = "context";

        public NewsContext()
            : base(ConnectionStringName)
        {
        }
        
        public DbSet<New> News { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<NewsContext, Configuration>());
            base.OnModelCreating(modelBuilder);
        }


    }

    internal sealed class Configuration : DbMigrationsConfiguration<NewsContext>
    {
       public Configuration()
       {
          AutomaticMigrationsEnabled = true;
          AutomaticMigrationDataLossAllowed = true;
       }
    }
}
