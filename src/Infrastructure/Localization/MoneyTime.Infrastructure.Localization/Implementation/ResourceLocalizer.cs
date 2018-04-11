using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Localization;
using MoneyTime.Infrastructure.Localization.Abstraction;
using MoneyTime.Infrastructure.Localization.Model;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace MoneyTime.Infrastructure.Localization.Implementation
{
    public class ResourceLocalizer : IResourceLocalizer
    {
        private readonly IHostingEnvironment _environment;
        private IList<JsonLocalization> _localizationValuesList = new List<JsonLocalization>();

        public ResourceLocalizer(IHostingEnvironment environment)
        {
            _environment = environment;
            SetLocalizationValueList();
        }

        private string JsonFile =>
            File.ReadAllText(Path.Combine(_environment.WebRootPath, "App_Data",
                $@"resources.{CultureInfo.CurrentCulture}.json"));

        public LocalizedString this[string key]
        {
            get
            {
                var translation = GetValueByKey(key);
                return new LocalizedString(key, translation ?? key, translation == null);
            }
        }

        public LocalizedString this[string key, params object[] arguments]
        {
            get
            {
                var translation = GetValueByKey(key);
                var value = string.Format(translation ?? key, arguments);
                return new LocalizedString(key, value, translation == null);
            }
        }

        public IEnumerable<LocalizedString> GetAllStrings()
        {
            return _localizationValuesList
                .Select(localization => new LocalizedString(localization.Key, localization.Value))
                .AsEnumerable();
        }

        private string GetValueByKey(string key)
        {
            return _localizationValuesList.FirstOrDefault(localization => localization.Key == key)?.Value;
        }

        private void SetCurrentCulture(CultureInfo withCulture)
        {
            CultureInfo.CurrentCulture = withCulture ?? CultureInfo.CurrentCulture;
        }

        private void SetLocalizationValueList()
        {
            _localizationValuesList = JsonConvert.DeserializeObject<List<JsonLocalization>>(JsonFile);
        }
    }
}
