namespace FirstLab
{
    using NewsClient;

    internal class Program
    {
        static void Main(string[] args)
        {
            MonthNewsPublications.Month();
        }
    }


}


namespace NewsProvider
{
    using System.Collections;

    public struct News
    {
        public string Title;        
        public string Author;     
        public string Body;
        public bool IsTheNewsFresh;

        public News(string title, string author, string body, bool isTheNewsFresh)
        {
            Title = title;
            Author = author;
            Body = body;
            IsTheNewsFresh = isTheNewsFresh;
        }
    }

    public delegate void ProcessNewsCallback(News news);

    public class NewsDB
    {
        ArrayList list = new();

        public void AddNews(News buildNews)
        {
            list.Add(buildNews);
        }

        public void ProcessFreshNews(ProcessNewsCallback processNews)
        {
            foreach (News b in list)
            {
                if (b.IsTheNewsFresh)
                    processNews(b);
            }
        }
    }

    public static class BuildNews
    {
        public static News FreshNews(string title, string author, string body)
        {
            return new News(title, author, body, true);
        }

        public static News OldNews(string title, string author, string body)
        {
            return new News(title, author, body, false);
        }
    }
}

namespace NewsClient
{
    using NewsProvider;
    using System.Collections;

    class NewsTotaller
    {
        ArrayList authors = new();

        int countNews = 0;
        int authorCount = 0;

        internal void AddNewsToTotal(News News)
        {
            countNews += 1;

            if (!authors.Contains(News.Author))
            {
                authorCount += 1;
                authors.Add(News.Author);
            }
        }

        internal int GetCountNews()
        {
            return countNews;
        }

        internal int GetAuthorsCount()
        {
            return authorCount;
        }
    }

    class MonthNewsPublications
    {
        static void PrintTitle(News b)
        {
            Console.WriteLine($"   {b.Title}");
        }

        public static void Month()
        {
            NewsDB NewsDB = new();

            AddNews(NewsDB);

            Console.WriteLine("Fresh News Titles:");

            NewsDB.ProcessFreshNews(PrintTitle);

            NewsTotaller totaller = new();

            NewsDB.ProcessFreshNews(totaller.AddNewsToTotal);

            Console.WriteLine("In that month we published " + totaller.GetCountNews() + 
                " news from " + totaller.GetAuthorsCount() + " authors");
        }

        static void AddNews(NewsDB NewsDB)
        {
            NewsDB.AddNews(BuildNews.FreshNews(
                "The C Programming Language", 
                "Ray Duncan", 
                "C is a Great Programming Language"));
            NewsDB.AddNews(BuildNews.OldNews(
                "The Unicode Standard 2.0", 
                "The Unicode Consortium", 
                "Unicode is a great standart"));
            NewsDB.AddNews(BuildNews.FreshNews(
                "The MS-DOS Encyclopedia", 
                "Ray Duncan", 
                "It is a lot MS-Dos systems in the world"));
            NewsDB.AddNews(BuildNews.FreshNews(
                "Debug all that you can see",
                "Scott Adams", 
                "But don't debug your cat, is's already broken animal"));
        }
    }
}

