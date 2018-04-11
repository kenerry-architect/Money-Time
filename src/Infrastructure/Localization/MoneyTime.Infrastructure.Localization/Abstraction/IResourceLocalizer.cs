using Microsoft.Extensions.Localization;
using System.Collections.Generic;

namespace MoneyTime.Infrastructure.Localization.Abstraction
{
    public interface IResourceLocalizer
    {
        LocalizedString this[string key] { get; }
        LocalizedString this[string name, params object[] arguments] { get; }
        IEnumerable<LocalizedString> GetAllStrings();
    }
}
