using MediatR;
using MoneyTime.Domain.Core.Notification;
using MoneyTime.Infrastructure.IntegrityKeeperValidator.Abstraction;
using MoneyTime.Infrastructure.Localization.Abstraction;
using MoneyTime.Infrastructure.Localization.Constant;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;

namespace MoneyTime.Infrastructure.IntegrityKeeperValidator.Implementation
{
    public class IntegrityKeeper : IIntegrityKeeper
    {
        private const string ValidDomainExpression =
            @"^(([a-zA-Z]{1})|([a-zA-Z]{1}[a-zA-Z]{1})|([a-zA-Z]{1}[0-9]{1})|([0-9]{1}[a-zA-Z]{1})|([a-zA-Z0-9][a-zA-Z0-9-_]{1,61}[a-zA-Z0-9]))\.([a-zA-Z]{2,6}|[a-zA-Z0-9-]{2,30}\.[a-zA-Z]{2,3})$";

        private const string ValidEmailExpression =
            @"^((([a-z]|\d|[!#\$%&'\*\+\-\/=\?\^_`{\|}~]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])+(\.([a-z]|\d|[!#\$%&'\*\+\-\/=\?\^_`{\|}~]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])+)*)|((\x22)((((\x20|\x09)*(\x0d\x0a))?(\x20|\x09)+)?(([\x01-\x08\x0b\x0c\x0e-\x1f\x7f]|\x21|[\x23-\x5b]|[\x5d-\x7e]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(\\([\x01-\x09\x0b\x0c\x0d-\x7f]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]))))*(((\x20|\x09)*(\x0d\x0a))?(\x20|\x09)+)?(\x22)))@((([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-||_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.)+(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])+|(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])+([a-z]+|\d|-|\.{0,1}|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])?([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))$";

        private readonly IResourceLocalizer _localizer;
        private readonly DomainNotificationHandler _notification;

        public IntegrityKeeper(INotificationHandler<DomainNotification> notification, IResourceLocalizer localizer)
        {
            _notification = (DomainNotificationHandler)notification;
            _localizer = localizer;
        }

        public bool IsValid()
        {
            return _notification.HasNotifications();
        }

        public IntegrityKeeper NotNull(object obj, string searchedField = "", string searchedFieldValue = "",
            string customKey = "")
        {
            if (string.IsNullOrEmpty(customKey))
            {
                if (obj is null)
                    NewErrorNotification(LocalizationKey.NotNullOrEmpty, searchedField, searchedFieldValue);
            }
            else if (!string.IsNullOrEmpty(customKey))
            {
                if (obj is null)
                    NewErrorNotification(customKey, searchedField, searchedFieldValue);
            }

            return this;
        }

        public IntegrityKeeper NotNullOrEmpty(string value, string field = "", string customKey = "")
        {
            if (string.IsNullOrEmpty(customKey))
            {
                if (string.IsNullOrEmpty(value))
                    NewErrorNotification(LocalizationKey.NotNullOrEmpty, field);
            }
            else if (!string.IsNullOrEmpty(customKey))
            {
                if (string.IsNullOrEmpty(value))
                    NewErrorNotification(customKey, field);
            }

            return this;
        }

        public IntegrityKeeper NotEqual(object value, object valueToCompare, string field = "", string customKey = "")
        {
            if (string.IsNullOrEmpty(customKey))
            {
                if (!EqualityComparer<object>.Default.Equals(value, valueToCompare))
                    NewErrorNotification(LocalizationKey.NotEqual, field);
            }
            else if (!string.IsNullOrEmpty(customKey))
            {
                if (!EqualityComparer<object>.Default.Equals(value, valueToCompare))
                    NewErrorNotification(customKey, field);
            }

            return this;
        }

        public IntegrityKeeper NotEqualByReflection(object obj, string propertyName, object value, string field = "",
            string customKey = "")
        {
            var valueToCompare = obj?.GetType().GetProperty(propertyName).GetValue(obj, null);

            if (string.IsNullOrEmpty(customKey))
            {
                if (!EqualityComparer<object>.Default.Equals(value, valueToCompare))
                    NewErrorNotification(LocalizationKey.NotEqual, field);
            }
            else if (!string.IsNullOrEmpty(customKey))
            {
                if (!EqualityComparer<object>.Default.Equals(value, valueToCompare))
                    NewErrorNotification(customKey, field);
            }

            return this;
        }

        public IntegrityKeeper GreaterThan(IComparable number, IComparable greaterThanNumber, string field = "",
            string customKey = "")
        {
            if (string.IsNullOrEmpty(customKey))
            {
                if (number.CompareTo(greaterThanNumber) <= 0)
                    NewErrorNotification(LocalizationKey.GreaterThan, field);
            }
            else if (!string.IsNullOrEmpty(customKey))
            {
                if (number.CompareTo(greaterThanNumber) <= 0)
                    NewErrorNotification(customKey, field);
            }

            return this;
        }

        public IntegrityKeeper LessThan(IComparable number, IComparable lessThanNumber, string field = "",
            string customKey = "")
        {
            if (string.IsNullOrEmpty(customKey))
            {
                if (number.CompareTo(lessThanNumber) >= 0)
                    NewErrorNotification(LocalizationKey.LessThan, field);
            }
            else if (!string.IsNullOrEmpty(customKey))
            {
                if (number.CompareTo(lessThanNumber) >= 0)
                    NewErrorNotification(customKey, field);
            }

            return this;
        }

        public IntegrityKeeper ValidEmail(string email, string customKey = "")
        {
            var regex = new Regex(ValidEmailExpression, RegexOptions.IgnoreCase);

            if (string.IsNullOrEmpty(customKey))
            {
                if (!regex.IsMatch(email))
                    NewErrorNotification(LocalizationKey.InvalidEmail, nameof(email), email);
            }
            else if (!string.IsNullOrEmpty(customKey))
            {
                if (!regex.IsMatch(email))
                    NewErrorNotification(customKey, nameof(email), email);
            }

            return this;
        }

        public IntegrityKeeper ValidPassword(string password, string customKey = "")
        {
            if (string.IsNullOrEmpty(customKey))
            {
                if (string.IsNullOrEmpty(password) || password.Length < 8)
                    NewErrorNotification(LocalizationKey.InvalidPassword, nameof(password), password);
            }
            else if (!string.IsNullOrEmpty(customKey))
            {
                if (string.IsNullOrEmpty(password) || password.Length < 8)
                    NewErrorNotification(customKey, nameof(password), password);
            }

            return this;
        }

        public IntegrityKeeper HasOneOrMore<T>(IList<T> list, string field = "", string customKey = "")
        {
            if (string.IsNullOrEmpty(customKey))
            {
                if (list == null || list.Count == 0)
                    NewErrorNotification(LocalizationKey.HasOneOrMore, field);
            }
            else if (!string.IsNullOrEmpty(customKey))
            {
                if (list == null || list.Count == 0)
                    NewErrorNotification(customKey, field);
            }

            return this;
        }

        public IntegrityKeeper ValidDomain(string domain, string customKey = "")
        {
            var regex = new Regex(ValidDomainExpression, RegexOptions.IgnoreCase);

            if (string.IsNullOrEmpty(customKey))
            {
                if (!regex.IsMatch(domain))
                    NewErrorNotification(LocalizationKey.InvalidDomain, nameof(domain), domain);
            }
            else if (!string.IsNullOrEmpty(customKey))
            {
                if (!regex.IsMatch(domain))
                    NewErrorNotification(customKey, nameof(domain), domain);
            }

            return this;
        }

        private void NewErrorNotification(string key, string field = "", params string[] parameters)
        {
            var completeMessage = _localizer[key]?.Value;
            if (completeMessage == null)
                return;

            completeMessage = parameters.Length > 0
                ? parameters.Aggregate(completeMessage, (current, item) => current.Replace("'{0}'", item))
                : completeMessage.Replace("'{0}'", string.Empty);

            _notification.Handle(new DomainNotification(field, completeMessage), default(CancellationToken));
        }
    }
}
