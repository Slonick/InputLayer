using System;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;
using InputLayer.Converters;

namespace InputLayer.MarkupExtensions
{
    public class LocalizationBinding : MarkupExtension
    {
        public LocalizationBinding() { }

        public LocalizationBinding(PropertyPath path, string stringFormat = null)
        {
            this.Path = path;
            this.StringFormat = stringFormat;
        }

        [ConstructorArgument("path")]
        public PropertyPath Path { get; set; }

        [ConstructorArgument("stringFormat")]
        public string StringFormat { get; set; }

        public override object ProvideValue(IServiceProvider serviceProvider)
            => new Binding(this.Path.Path)
            {
                Path = this.Path,
                Converter = new LocalizationConverter(),
                ConverterParameter = this.StringFormat,
                UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged,
                Mode = BindingMode.OneTime
            };
    }
}