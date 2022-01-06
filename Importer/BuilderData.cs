using HtmlAgilityPack;
using Importer.Interface;
using Importer.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Importer
{
    public class BuilderData
    {
        private static NLog.Logger _logger = NLog.LogManager.GetCurrentClassLogger();

        #region field and variable
        protected IDataLoader _dataLoader;
        protected IWebsiteManipulation _websiteManipulation;
        public JsonModsFile NewModItems = new JsonModsFile();
        public JsonModsFile OldModItems = new JsonModsFile();
        private object locker = new object();
        private EnumWebsite _website;
        #endregion

        public BuilderData(IDataLoader dataLoader,IWebsiteManipulation websiteManipulation,EnumWebsite website)
        {
            this._dataLoader = dataLoader;
            this._websiteManipulation = websiteManipulation;
            this._website = website;
        }

        public async Task CreateJson(string localPath = null, bool update = false)
        {
            if (string.IsNullOrEmpty(localPath))
                localPath = Directory.GetCurrentDirectory() + $"\\{_website}_{DateTime.Now.ToString("dd-MM-yyyy")}.json";
            else
                localPath += $"\\{_website}_{DateTime.Now.ToString("dd-MM-yyyy")}.json";
            await PrepareDataBeforeSave(localPath, update);
            string json = JsonSerializer.Serialize(NewModItems);
            _logger.Info("{0} > mods count: {1}", _website,NewModItems.Mods.Count);
            File.WriteAllText(localPath, json);
            _logger.Info("{0} > Save file complete",_website);
        }

        #region private method

        
        private async Task PrepareDataBeforeSave(string localPath, bool update)
        {
            HtmlDocument page = _dataLoader.GetHtmlAsync().Result;
            IEnumerable<Website> uris = _websiteManipulation.GetUrisCategory(page);
            NewModItems.CategoriesMap = AddSchemaCategoryToModsList(uris).ToList();
            List<Task> tasks = new List<Task>();
            foreach (Website uri in uris)
                tasks.Add(PrepareFromFirstSubPage(uri));
            if (update)
                tasks.Add(new Task(async () => { OldModItems.Mods = await GetDataWithFile(localPath); }));
            await Task.WhenAll(tasks);
            if (update)
                CompareFile(ref NewModItems, OldModItems);
        }
        private IEnumerable<SchemaCategory> AddSchemaCategoryToModsList(IEnumerable<Website> urisAndCategoryName)
        {
            foreach (Website item in urisAndCategoryName)
                yield return new SchemaCategory() { ChildElement = item.Category, ParentElement = item.ParentCategory };
        }
        private async Task PrepareFromFirstSubPage(Website uri)
        {
            HtmlDocument subpage = await _dataLoader.GetHtmlAsync(uri.UriPage);
            List<Task> subTasks = new List<Task>();
            _logger.Info("{0} > Searching mods for {1}...",_website ,uri.Category);
            foreach (Uri item in LoadUriNumberPages(uri.Category, subpage,uri.UriPage).Result)
                subTasks.Add(LoadItems(uri.Category, uri: item));
            await Task.WhenAll(subTasks);
            _logger.Info("{0} > Ending search mods for {1}", _website,uri.Category);
        }

        #endregion

        #region private method

        private async Task<List<Uri>> LoadUriNumberPages(string category, HtmlDocument page, Uri uri=null)
        {
            List<Uri> uriSub = new List<Uri>();
            uriSub.AddRange(_websiteManipulation.GetPageList(page, uri));
            await LoadItems(category, html: page);
            if(uri!=null && _website==EnumWebsite.Steam)
                uriSub.RemoveAt(0);
            return uriSub;
        }

        private async Task LoadItems(string category, Uri uri = null, HtmlDocument html = null)
        {
            HtmlDocument page = html != null ? html : await _dataLoader.GetHtmlAsync(uri);
            lock (locker)
                NewModItems.Mods.AddRange(_websiteManipulation.SearchItems(category, page));
        }

        private bool GetEqualProperty(Mod item1, Mod item2)
        {
            foreach (PropertyInfo propertyInfo in item1.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance))
                if (!propertyInfo.GetValue(item1).Equals(propertyInfo.GetValue(item2)))
                    return false;
            return true;
        }        
        private async Task<List<Mod>> GetDataWithFile(string path)
        {
            return await _dataLoader.GetFileAsync(path);
        }
        private void CompareFile(ref JsonModsFile newMods, JsonModsFile oldMods)
        {
            foreach (Mod item in newMods.Mods)
            {
                Mod itemOld = oldMods.Mods.Where(x => x.Id == item.Id).FirstOrDefault();
                if (itemOld != null)
                {
                    if (GetEqualProperty(itemOld, item))
                        item.StateFile = EnumStateFile.Old;
                    else
                        item.StateFile = EnumStateFile.Update;
                }
            }
        }
        #endregion 

    }
}
