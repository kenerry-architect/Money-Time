using MoneyTime.Infrastructure.IntegrityKeeperValidator.Implementation;
using System;
using System.Collections.Generic;

namespace MoneyTime.Infrastructure.IntegrityKeeperValidator.Abstraction
{
    public interface IIntegrityKeeper
    {
        IntegrityKeeper ValidEmail(string email, string customKey = "");
        IntegrityKeeper ValidPassword(string password, string customKey = "");
        IntegrityKeeper NotNullOrEmpty(string value, string field = "", string customKey = "");
        IntegrityKeeper NotEqual(object value, object valueToCompare, string field = "", string customKey = "");
        IntegrityKeeper NotNull(object obj, string searchedField = "", string searchedFieldValue = "", string customKey = "");
        IntegrityKeeper GreaterThan(IComparable number, IComparable greaterThanNumber, string field = "", string customKey = "");
        IntegrityKeeper LessThan(IComparable number, IComparable lessThanNumber, string field = "", string customKey = "");
        IntegrityKeeper NotEqualByReflection(object obj, string propertyName, object value, string field = "", string customKey = "");
        IntegrityKeeper HasOneOrMore<T>(IList<T> list, string field = "", string customKey = "");
        IntegrityKeeper ValidDomain(string domain, string customKey = "");
    }
}
