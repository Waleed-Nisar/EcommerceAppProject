namespace ECS.Application.DTOs.Common;

/// <summary>
/// Paginated response for large datasets
/// </summary>
public class PaginatedResponse<T>
{
    public List<T> Data { get; set; } = new();
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public int TotalPages { get; set; }
    public int TotalRecords { get; set; }
    public bool HasPreviousPage => PageNumber > 1;
    public bool HasNextPage => PageNumber < TotalPages;

    public static PaginatedResponse<T> Create(List<T> data, int pageNumber, int pageSize, int totalRecords)
    {
        return new PaginatedResponse<T>
        {
            Data = data,
            PageNumber = pageNumber,
            PageSize = pageSize,
            TotalRecords = totalRecords,
            TotalPages = (int)Math.Ceiling(totalRecords / (double)pageSize)
        };
    }
}