using System.Net;

namespace Videospieldatenbank.Database
{
    public class Game
    {
        public string CoverUrl;
        public string Developer;
        public string[] Genres;
        public string IgdbUrl;
        public string Name;
        public string[] Plattforms;
        public int Rating;
        public string Website;
        public string Wiki;

        public byte[] Cover
        {
            get
            {
                using (var webClient = new WebClient())
                {
                    return webClient.DownloadData(CoverUrl);
                }
            }
        }
    }
}