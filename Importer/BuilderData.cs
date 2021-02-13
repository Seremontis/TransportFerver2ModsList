using HtmlAgilityPack;
using Importer.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

namespace Importer
{
    public class BuilderData
    {
        private IWebsiteLoader websiteLoader;
        private IWebsiteManipulation websiteManipulation;
        private string path;
        public List<ModItem> modItems=new List<ModItem>();
        private object locker = new object();

        public BuilderData(IWebsiteLoader websiteLoader, IWebsiteManipulation websiteManipulation)
        {
            this.websiteLoader = websiteLoader;
            this.websiteManipulation = websiteManipulation;
        }
        public async Task CreateJson(string localPath = null)
        {
            path = string.IsNullOrEmpty(localPath) ? Directory.GetCurrentDirectory() : localPath;
            HtmlDocument page = websiteLoader.GetHtml().Result;
            List<Website> uris = websiteManipulation.GetUrisCategory(page);
            List<Task> tasks = new List<Task>();
            foreach (Website uri in uris)
                tasks.Add(PrepareFromFirstSubPage(uri));
            try
            {
                await Task.WhenAll(tasks);
                var json = JsonSerializer.Serialize(modItems);
                //File.WriteAllText(localPath!=null?localPath:Directory.GetCurrentDirectory()+"\\"+DateTime.Now.ToString("dd-MM-yyyy"), json);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void UpdateJson(string localPath)
        {

        }

        private async Task PrepareFromFirstSubPage(Website uri)
        {

            HtmlDocument subpage = await websiteLoader.GetHtml(uri.UriPage);
            List<Task> subTasks = new List<Task>();
            foreach (Uri item in LoadUriNumberPages(uri.Category, subpage).Result)
                subTasks.Add(LoadItems(uri.Category, uri: item));
            await Task.WhenAll(subTasks);
        }

        private async Task<List<Uri>> LoadUriNumberPages(string category,HtmlDocument page)
        {
            List<Uri> uriSub = new List<Uri>();
            uriSub.AddRange(websiteManipulation.GetPageList(page));
            await LoadItems(category,html: page);
            return uriSub;
        }
        private async Task LoadItems(string category, Uri uri = null, HtmlDocument html = null)
        {
            HtmlDocument page = html != null ? html : await websiteLoader.GetHtml(uri);
            lock (locker)
                modItems.AddRange(websiteManipulation.SearchItems(category, page));
        }

    }
}
