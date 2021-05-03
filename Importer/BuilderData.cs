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
        private static NLog.Logger _logger = NLog.LogManager.GetCurrentClassLogger();
        private IDataLoader dataLoader;
        private IWebsiteManipulation websiteManipulation;
        public JsonModsFile NewModItems = new JsonModsFile();
        public JsonModsFile OldModItems = new JsonModsFile();
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
            else
                localPath += $"\\ListMods_{DateTime.Now.ToString("dd-MM-yyyy")}.json";
            await PrepareDataBeforeSave(localPath, update);
            string json = JsonSerializer.Serialize(NewModItems);
            File.WriteAllText(localPath, json);
            _logger.Info("Save file complete");
        }

        private async Task PrepareDataBeforeSave(string localPath, bool update)
        {
            HtmlDocument page = dataLoader.GetHtml().Result;
            List<Website> uris = websiteManipulation.GetUrisCategory(page);
            AddSchemaCategoryToModsList(uris);
            List<Task> tasks = new List<Task>();
            foreach (Website uri in uris)        
                tasks.Add(PrepareFromFirstSubPage(uri));
            if (update)
                tasks.Add(GetDataWithFile(localPath));
            await Task.WhenAll(tasks);
            if (update)
                CompareFile();
        }

        private void CompareFile()
        {
            foreach (Mod item in NewModItems.Mods)
            {
                Mod itemOld = OldModItems.Mods.Where(x => x.Id == item.Id).FirstOrDefault();
                if (itemOld != null)
                {
                    if (GetEqualProperty(itemOld, item))
                        item.StateFile = EnumStateFile.Old;
                    else
                        item.StateFile = EnumStateFile.Update;
                }
            }
        }

        private void AddSchemaCategoryToModsList(List<Website> urisAndCategoryName)
        {
            foreach (Website item in urisAndCategoryName)
                NewModItems.CategoriesMap.Add(new SchemaCategory() { ChildElement = item.Category, ParentElement = item.ParentCategory });
        }

        private bool GetEqualProperty(Mod item1, Mod item2)
        {
            foreach (PropertyInfo propertyInfo in item1.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance))
                if (!propertyInfo.GetValue(item1).Equals(propertyInfo.GetValue(item2)))
                    return false;
            return true;
        }

        private async Task GetDataWithFile(string path)
        {
            OldModItems.Mods = await dataLoader.GetFile(path);
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
                NewModItems.Mods.AddRange(websiteManipulation.SearchItems(category, page));
        }

    }
}
