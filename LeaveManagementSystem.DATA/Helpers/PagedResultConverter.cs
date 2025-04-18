using AutoMapper;
using LeaveManagementSystem.DATA.Common;

namespace LeaveManagementSystem.DATA.Helpers
{
    public class PagedResultConverter<TSource, TDestination> :
      ITypeConverter<PagedResult<TSource>, PagedResult<TDestination>>
    {
        public PagedResult<TDestination> Convert(PagedResult<TSource> source, PagedResult<TDestination> destination, ResolutionContext context)
        {
            return new PagedResult<TDestination>
            {
                Items = context.Mapper.Map<IEnumerable<TDestination>>(source.Items),
                TotalItems = source.TotalItems,
                Page = source.Page,
                PageSize = source.PageSize
            };
        }
    }
}
