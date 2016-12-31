using System.Collections.Generic;
using System.Linq;
using System.Net;
using HtmlAgilityPack;

namespace Videospieldatenbank.Database
{
    /// <summary>
    ///     Beinhaltet Methoden zum Abfragen von Spielinformationen von IGDB.
    /// </summary>
    public static class IgdbParser
    {
        /// <summary>
        ///     Erstellt ein Game-Objekt aus einem IGDB-Link.
        /// </summary>
        /// <param name="igdbUrl">Link zur IGDB-Seite des Spiels.</param>
        /// <returns><see cref="Game" />-Objekt der Website.</returns>
        public static Game ParseGame(string igdbUrl)
        {
            Game game = new Game();

            HtmlDocument website = new HtmlDocument();
            using (WebClient client = new WebClient())
            {
                website.LoadHtml(client.DownloadString(igdbUrl));
            }

            game.IgdbUrl = igdbUrl;

            try
            {
                game.Name = website.DocumentNode.Descendants("h1")
                    .First(n => n.GetAttributeValue("class", "").Equals("banner-title "))
                    .InnerText
                    .Replace("<!-- react-text: 14 -->", "")
                    .Replace("<!-- /react-text -->", "");
            }
            catch
            {
                game.Name = "N/A";
            }

            try
            {
                game.CoverUrl = "http:" + website.DocumentNode.Descendants("img")
                                    .First(n => n.GetAttributeValue("class", "").Equals("img-responsive cover_big"))
                                    .GetAttributeValue("src", "");
            }
            catch
            {
                game.CoverUrl = "N/A";
            }

            try
            {
                game.Developer = website.DocumentNode.Descendants("h3")
                    .First(n => n.GetAttributeValue("class", "").Equals("banner-subsubheading"))
                    .InnerText;
            }
            catch
            {
                game.Developer = "N/A";
            }

            try
            {
                game.Genres = website.DocumentNode.Descendants("p")
                    .First(n => n.Descendants("span").First().InnerText.Equals("Genre: "))
                    .Descendants("a").Select(descendant => descendant.InnerText).ToArray();
            }
            catch
            {
                game.Genres = new string[0];
            }

            try
            {
                game.Plattforms = website.DocumentNode.Descendants("p")
                    .First(n => n.Descendants("span").First().InnerText.Equals("Platforms: "))
                    .Descendants("a").Select(descendant => descendant.InnerText).ToArray();
            }
            catch
            {
                game.Plattforms = new string[0];
            }

            try
            {
                game.Rating = int.Parse(website.DocumentNode.Descendants("svg")
                    .First(n => n.GetAttributeValue("class", "").Contains("gauge-twin"))
                    .Descendants("text").First().InnerText);
            }
            catch
            {
                game.Rating = 0;
            }

            return game;
        }

        /// <summary>
        ///     Erstellt ein Array mit den aktuellen Top 100 Spielen auf IGDB.
        /// </summary>
        /// <returns>Array mit den top 100 Spielen auf IGDB.</returns>
        public static Game[] GetTop100()
        {
            Game[] games = new Game[100];
            HtmlDocument website = new HtmlDocument();
            using (WebClient client = new WebClient())
            {
                website.LoadHtml(client.DownloadString("https://www.igdb.com/top-100/games"));
            }

            IEnumerable<HtmlNode> rows = website.DocumentNode.Descendants("tbody").First()
                .Descendants("tr");

            int i = 0;
            foreach (HtmlNode row in rows)
            {
                string url = row.Descendants("a")
                    .First().GetAttributeValue("href", "");
                games[i] = ParseGame("https://www.igdb.com" + url);
                i++;
            }
            return games;
        }
    }
}