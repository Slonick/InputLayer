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

        public LocalizationBinding(PropertyPath path, string stringFormat = null, object key = null)
        {
            this.Path = path;
            this.StringFormat = stringFormat;
            this.Key = key;
        }

        [ConstructorArgument("key")]
        public object Key { get; set; }

        [ConstructorArgument("path")]
        public PropertyPath Path { get; set; }

        [ConstructorArgument("stringFormat")]
        public string StringFormat { get; set; }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            var binding = new Binding
            {
                Path = this.Path,
                Converter = new LocalizationConverter(),
                ConverterParameter = this.StringFormat,
                UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged,
                Mode = BindingMode.OneWay
            };

            if (this.Key != null)
            {
                binding.Source = this.Key;
            }

            return binding.ProvideValue(serviceProvider);
        }
    }
}