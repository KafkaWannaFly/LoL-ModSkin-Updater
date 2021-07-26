using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;

String url = "http://modskinpro.com/p/tai-phan-mem-mod-skin-lol-pro-2020-chn";

Console.WriteLine("LoL ModSkin Updater. Press Enter key to continue...");
Console.ReadLine();

string downloadLink = GetDownloadLink(await GetAsync(url));
Console.WriteLine($"Get: {downloadLink}");

Console.WriteLine($"Start downloading...");
string downloadFile = await DownloadFile(downloadLink);

Console.WriteLine($"Result at: {downloadFile}");
Console.ReadLine();

async Task<string> DownloadFile(string resourcePath)
{
    var fileName = Path.Combine(Directory.GetCurrentDirectory(), GetFileNameFromURL(resourcePath));

    using (var webClient = new WebClient())
    {
        webClient.DownloadProgressChanged += (sender, e) =>
        {
            ClearCurrentConsoleLine();
            Console.Write($"Progress: {e.ProgressPercentage}/100%");
        };

        await webClient.DownloadFileTaskAsync(resourcePath, fileName);
    }

    return fileName;
}

void ClearCurrentConsoleLine()
{
    int currentLineCursor = Console.CursorTop;
    Console.SetCursorPosition(0, Console.CursorTop);
    Console.Write(new string(' ', Console.WindowWidth));
    Console.SetCursorPosition(0, currentLineCursor);
}

string GetFileNameFromURL(string url)
{
    Uri uri;
    if (!Uri.TryCreate(url, UriKind.Absolute, out uri))
        uri = new Uri(url, UriKind.Absolute);

    return Path.GetFileName(uri.LocalPath);
}

string GetDownloadLink(string html)
{
    var magicWord = "link3 = ";
    var begin = html.IndexOf(magicWord) + magicWord.Length;
    var end = html.IndexOf(";", begin);

    var downloadLink = html.Substring(begin, end - begin).Replace("\"", "");

    return downloadLink;
}

async Task<string> GetAsync(string uri)
{
    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
    request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;

    using (HttpWebResponse response = (HttpWebResponse)await request.GetResponseAsync())
    using (Stream stream = response.GetResponseStream())
    using (StreamReader reader = new StreamReader(stream))
    {
        return await reader.ReadToEndAsync();
    }
}