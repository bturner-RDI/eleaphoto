using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;

namespace AzureApp.Utilities
{
    public interface IMapper
    {
        T2 Map<T, T2>(T source);

        IEnumerable<T2> Map<T, T2>(IEnumerable<T> source);
    }
    public class Mapper : IMapper
    {
        public T2 Map<T, T2>(T source)
        {
            AutoMapper.Mapper.CreateMap<T, T2>();
            return AutoMapper.Mapper.Map<T, T2>(source);
        }

        public IEnumerable<T2> Map<T, T2>(IEnumerable<T> source)
        {
            AutoMapper.Mapper.CreateMap<T, T2>();
            return AutoMapper.Mapper.Map<IEnumerable<T2>>(source);
        }
    }
}
