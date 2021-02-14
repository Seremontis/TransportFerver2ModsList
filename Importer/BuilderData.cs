using HtmlAgilityPack;
using Importer.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using System.Threading.Tasks;

namespace Importer
{
    public class BuilderData
    {
        private IDataLoader dataLoader;
        private IWebsiteManipulation websiteManipulation;
        private string path;
        public List<ModItem> NewModItems = new List<ModItem>();
        public List<ModItem> OldModItems = new List<ModItem>();
        private object locker = new object();

        public BuilderData(IDataLoader websiteLoader, IWebsiteManipulation websiteManipulation)
        {
            this.dataLoader = websiteLoader;
            this.websiteManipulation = websiteManipulation;
        }
        public async Task CreateJson(string localPath = null, bool update = false)
        {
            if (string.IsNullOrEmpty(localPath))
                localPath = Directory.GetCurrentDirectory() + $"\\ListMods_{DateTime.Now.ToString("dd-MM-yyyy")}.json";
            HtmlDocument page = dataLoader.GetHtml().Result;
            List<Website> uris = websiteManipulation.GetUrisCategory(page);
            List<Task> tasks = new List<Task>();
            foreach (Website uri in uris)
                tasks.Add(PrepareFromFirstSubPage(uri));
            if (update)
                tasks.Add(GetDataWithFile(localPath));
            try
            {
                await Task.WhenAll(tasks);
                if (update)
                    CompareFile();
                string json = JsonSerializer.Serialize(NewModItems);
                File.WriteAllText(localPath, json);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void CompareFile()
        {
            foreach (ModItem item in NewModItems)
            {
                ModItem itemOld = OldModItems.Where(x => x.Id == item.Id).FirstOrDefault();
                if (itemOld != null)
                {
                    if (GetEqualProperty(itemOld, item))
                        item.StateFile = EnumStateFile.Old;
                    else
                        item.StateFile = EnumStateFile.Update;
                }         
            }
        }

        private bool GetEqualProperty(ModItem item1, ModItem item2)
        {
            foreach (var propertyInfo in item1.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance))
                if (!propertyInfo.GetValue(item1).Equals(propertyInfo.GetValue(item2)))
                    return false;
            return true;
        }

        private async Task GetDataWithFile(string path)
        {
            OldModItems = await dataLoader.GetFile(path);
        }

        private async Task PrepareFromFirstSubPage(Website uri)
        {

            HtmlDocument subpage = await dataLoader.GetHtml(uri.UriPage);
            List<Task> subTasks = new List<Task>();
            foreach (Uri item in LoadUriNumberPages(uri.Category, subpage).Result)
                subTasks.Add(LoadItems(uri.Category, uri: item));
            await Task.WhenAll(subTasks);
        }

        private async Task<List<Uri>> LoadUriNumberPages(string category, HtmlDocument page)
        {
            List<Uri> uriSub = new List<Uri>();
            uriSub.AddRange(websiteManipulation.GetPageList(page));
            await LoadItems(category, html: page);
            return uriSub;
        }
        private async Task LoadItems(string category, Uri uri = null, HtmlDocument html = null)
        {
            HtmlDocument page = html != null ? html : await dataLoader.GetHtml(uri);
            lock (locker)
                NewModItems.AddRange(websiteManipulation.SearchItems(category, page));
        }

    }
}
