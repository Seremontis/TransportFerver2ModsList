using System.Threading.Tasks;

namespace Importer
{
    public interface IBuilderData
    {
        Task CreateJson(string localPath = null, bool update = false);
    }
}