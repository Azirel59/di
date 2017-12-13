using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace TagsCloudConsoleClient.Options
{
    public abstract class OptionPicker<T>
    {
        protected OptionInfo<T>[] Options;

        public int OptionCount => Options.Length;

        protected OptionPicker(IEnumerable<T> options)
        {
            Options = options.Select(opt => new
                {
                    Description = opt.GetType().GetCustomAttribute<DescriptionAttribute>()?.Description,
                    Implementation = opt
                })
                .OrderBy(opt => opt.Description)
                .Select(opt => new OptionInfo<T>()
                {
                    Description = opt.Description,
                    Implementation = opt.Implementation
                }).ToArray();
        }

        public string GetOptionsText()
        {
            return string.Join("\n", Options.Select((opt, index) => $"{index}. {opt.Description}"));
        }

        public OptionInfo<T> GetOption(long index)
        {
            return Options[index];
        }

        public long GetOptionIndexByType(Type type)
        {
            for (var i = 0; i < OptionCount; i++)
            {
                if (GetOption(i).Implementation.GetType() == type)
                {
                    return i;
                }
            }
            throw new IndexOutOfRangeException();
        }
    }
}
