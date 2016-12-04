using System.Net;
using Videospieldatenbank.Utils;

namespace Videospieldatenbank.Tests
{
    public class ImageUtilsTest : ITest
    {
        public bool Test()
        {
            return
                TestImageUtils(
                    "http://www.skringers.com/wp-content/uploads/Beautiful-Landscape-Wallpapers-Island-150x150.jpg") &&
                TestImageUtils(
                    "http://www.mikemayer.photography/wp-content/uploads/2016/08/Baden-W%C3%BCrttemberg-Kaiserstuhl-150x150.jpg") &&
                TestImageUtils(
                    "http://png.clipart.me/graphics/thumbs/795/vector-landscape-night-summer-forest-with-green-trees-and-the-sky-with-moon-and-stars_79537402.jpg") &&
                TestImageUtils("http://shutterstoppers.com/wp-content/uploads/2014/08/landscape-photo5-150x150.jpg");
        }

        private static bool TestImageUtils(string url)
        {
            var image = new WebClient().DownloadData(url);
            var imageString = ImageUtils.ByteArrayToString(image);
            var newImage = ImageUtils.StringToByteArray(imageString);
            var equal = true;
            for (var i = 0; i < image.Length; i++)
            {
                equal = image[i] == newImage[i];
                if (!equal) break;
            }
            return equal;
        }
    }
}