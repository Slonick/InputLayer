using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;

namespace InputLayer.Converters.Base
{
    /// <summary>
    ///     MarkupValueConverter is a MarkupExtension which can be used for ValueConverter.
    /// </summary>
    /// <seealso cref="System.Windows.Markup.MarkupExtension"/>
    /// <seealso cref="System.Windows.Data.IValueConverter"/>
    [MarkupExtensionReturnType(typeof(IValueConverter))]
    public abstract class MarkupValueConverter : MarkupExtension, IValueConverter
    {
        /// <inheritdoc/>
        public abstract object Convert(object value, Type targetType, object parameter, CultureInfo culture);

        /// <inheritdoc/>
        public virtual object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => throw new NotImplementedException();

        /// <inheritdoc/>
        public override object ProvideValue(IServiceProvider serviceProvider) => this;
    }
}