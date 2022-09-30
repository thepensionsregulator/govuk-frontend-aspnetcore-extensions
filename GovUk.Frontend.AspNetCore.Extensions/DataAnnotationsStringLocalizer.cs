using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GovUk.Frontend.AspNetCore.Extensions
{
    public class DataAnnotationStringLocalizer : IStringLocalizer
    {
        private readonly IStringLocalizer primaryLocalizer;
        private readonly IStringLocalizer fallbackLocalizer;

        public DataAnnotationStringLocalizer(IStringLocalizer primaryLocalizer, IStringLocalizer fallbackLocalizer)
        {
            this.primaryLocalizer = primaryLocalizer ?? throw new ArgumentNullException(nameof(primaryLocalizer));
            this.fallbackLocalizer = fallbackLocalizer ?? throw new ArgumentNullException(nameof(fallbackLocalizer));
        }

        public LocalizedString this[string name]
        {
            get
            {
                LocalizedString localizedString = primaryLocalizer[name];
                if (localizedString.ResourceNotFound)
                {
                    localizedString = fallbackLocalizer[name];
                }

                return localizedString;
            }
        }

        public LocalizedString this[string name, params object[] arguments]
        {
            get
            {
                LocalizedString localizedString = primaryLocalizer[name, arguments];
                if (localizedString.ResourceNotFound)
                {
                    localizedString = fallbackLocalizer[name, arguments];
                }

                return localizedString;
            }
        }

        public IEnumerable<LocalizedString> GetAllStrings(bool includeParentCultures)
            => primaryLocalizer.GetAllStrings(includeParentCultures).Concat(fallbackLocalizer.GetAllStrings(includeParentCultures));


        //public IStringLocalizer WithCulture(CultureInfo culture)
        //    => new DataAnnotationStringLocalizer(primaryLocalizer.CurrentCulture(culture), fallbackLocalizer.WithCulture(culture));
    }
}
