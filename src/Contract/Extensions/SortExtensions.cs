using System.Linq.Expressions;

namespace Contract.Extensions
{
    public static class SortExtensions
    {
        public static IQueryable<T> Sort<T>(
        this IQueryable<T> source,
        string? sortColumn,
        bool? isDescending = true)
        {
            // 1. Nếu sortColumn rỗng thì không thực hiện sort
            if (string.IsNullOrWhiteSpace(sortColumn))
                return source;

            // 3. Tạo một Expression kiểu T đặt tên giả định là 'x'
            // => Tạo một tham số "x" đại diện cho mỗi phần tử của source (kiểu T)
            var parameter = Expression.Parameter(typeof(T), "x");

            // 4. Tạo một Expression thực hiện truy cập thuộc tính có tên sortColumn
            // => Tạo biểu thức truy cập: x => x.sortColumn
            // (trong đó sortColumn là tên thuộc tính được truyền vào)
            Expression property = Expression.PropertyOrField(parameter, sortColumn);

            // 5. Chuyển đổi kiểu trả về của property về object
            // => Vì OrderBy(OrderByDescending) sử dụng lambda kiểu Func<T, object>,
            // ta cần ép kiểu giá trị trả về sang object
            Expression conversion = Expression.Convert(property, typeof(object));

            // 6. Tạo một lambda expression từ biểu thức trên
            // => Lambda được tạo ra có dạng: x => (object)x.sortColumn
            var lambda = Expression.Lambda<Func<T, object>>(conversion, parameter);


            // 7. Thực hiện sắp xếp dựa vào biến descending:
            //    Nếu descending = true: dùng OrderByDescending(lambda)
            //    Ngược lại, dùng OrderBy(lambda)
            return isDescending == true ? source.OrderByDescending(lambda) : source.OrderBy(lambda);
        }
    }
}
