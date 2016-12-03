using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using HtmlAgilityPack;

namespace Videospieldatenbank.Database
{
    public static class IgdbParser
    {
        /// <summary>
        /// Erstellt ein Game-Objekt aus einem IGDB-Link.
        /// </summary>
        /// <param name="igdbUrl">Link zur IGDB-Seite des Spiels.</param>
        /// <returns><see cref="Game"/>-Objekt der Website.</returns>
        public static Game ParseGame(string igdbUrl)
        {
            Game game = new Game();

            HtmlDocument website = new HtmlDocument();
            using (WebClient client = new WebClient())
                website.LoadHtml(client.DownloadString(igdbUrl));

            game.IgdbUrl = igdbUrl;

            game.Name = website.DocumentNode.Descendants("h1")
                .First(n => n.GetAttributeValue("class", "").Equals("banner-title "))
                .InnerText
                .Replace("<!-- react-text: 14 -->", "")
                .Replace("<!-- /react-text -->", "");

            game.CoverUrl = website.DocumentNode.Descendants("img")
                .First(n => n.GetAttributeValue("class", "").Equals("img-responsive cover_big"))
                .GetAttributeValue("src", "");

            game.Developer = website.DocumentNode.Descendants("h3")
                .First(n => n.GetAttributeValue("class", "").Equals("banner-subsubheading"))
                .InnerText;

            game.Genres = website.DocumentNode.Descendants("p")
                .First(n => n.Descendants("span").First().InnerText.Equals("Genre: "))
                .Descendants("a").Select(descendant => descendant.InnerText).ToArray();
            
            game.Plattforms = website.DocumentNode.Descendants("p")
                .First(n => n.Descendants("span").First().InnerText.Equals("Platforms: "))
                .Descendants("a").Select(descendant => descendant.InnerText).ToArray();

            game.Rating = int.Parse(website.DocumentNode.Descendants("svg")
                .First(n => n.GetAttributeValue("class", "").Equals("gauge filter-great gauge-twin"))
                .Descendants("text").First().InnerText);
            return game;
        }
    }
}
