using System;
using System.Windows.Markup;

namespace WpfColorView
{
    public class EnumBindingSourceExtension : MarkupExtension
    {
        private Type _enumType;

        public EnumBindingSourceExtension()
        { }

        public EnumBindingSourceExtension(Type enumType)
        {
            EnumType = enumType;
        }

        public Type EnumType
        {
            get => _enumType;
            set
            {
                if (value != _enumType)
                {
                    if (value is not null)
                    {
                        Type enumType = Nullable.GetUnderlyingType(value) ?? value;

                        if (!enumType.IsEnum)
                        {
                            throw new ArgumentException("Type must be Enum.");
                        }
                    }

                    _enumType = value;
                }
            }
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            if (_enumType is null)
            {
                throw new InvalidOperationException("EnumType is null, it must be specified.");
            }

            Type actualEnumType = Nullable.GetUnderlyingType(_enumType) ?? _enumType;
            Array enumValues = Enum.GetValues(actualEnumType);

            if (actualEnumType == _enumType)
            {
                return enumValues;
            }

            Array tempArray = Array.CreateInstance(actualEnumType, enumValues.Length + 1);
            enumValues.CopyTo(tempArray, 1);
            return tempArray;
        }
    }
}