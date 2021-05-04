using Importer;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;

namespace TFDesktopApp
{
    public partial class MainWindow : Window
    {
        private readonly Regex exFile = new Regex(@"ListMods_(?<day>\d{2})-(?<month>\d{2})-(?<year>\d{4})\.json", RegexOptions.Compiled);
        private JsonModsFile data;
        public MainWindow()
        {
            InitializeComponent();
            string[] test = Directory.GetFiles(@"C:\ModsList\Mods");
            foreach (string item in test)
            {
                if (exFile.IsMatch(item))
                {
                    data = JsonConvert.DeserializeObject<JsonModsFile>(File.ReadAllText(item));
                    PrepareFilterSection();
                    break;
                }
            }
        }

        private void PrepareFilterSection()
        {
            foreach (string item in data.CategoriesMap?.Select(x => x.ParentElement)?.Distinct())
                AddElements(item, data.CategoriesMap.Where(x => x.ParentElement == item).Select(x => x.ChildElement));          
        }

        private void AddElements(string mainCategory, IEnumerable<string> subCategory)
        {
            AddCheckBox(mainCategory);
            foreach (var item in subCategory)
                AddCheckBox(item, false);
        }

        private void AddCheckBox(string text,bool parent = true)
        {
            CheckBox checkBox = new CheckBox();
            checkBox.Style= (parent?Application.Current.Resources["categoryCheckBox"] : Application.Current.Resources["subcategoryCheckBox"]) as Style;
            checkBox.Content = text;
            CategoryAndSubCategories.Children.Add(checkBox);
        }

        private void AddModElement()
        {
            DefineGrid();
            foreach (var item in data.Mods.Take(5))
            {
                ModElement modElement = new ModElement();
                modElement.Element = item;
                ElementsMod.Children.Add(modElement);
            }
        }

        private void DefineGrid()
        {
            int columns = (int)(ElementsMod.ActualHeight / 400);
            for (int i = 0; i < columns-1; i++)
                ElementsMod.ColumnDefinitions.Add(new ColumnDefinition());
        }
    }
}
