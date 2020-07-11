using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using TF2ModsList.Models;

namespace TF2ModsList.Services.Interface
{
    public interface ITF2ListItemOperation:IDataOperation
    {
        ObservableCollection<Mod> ReturnModsItem();

        Tuple<string, string> ReturnAccesPage();
    }
}
