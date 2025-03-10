namespace Api.Shared.Helpers;

public class PaginationParams
{
    private const int MAX_PAGE_SIZE = 50;

    private int _pageSize = 10;

    public int pageNumber { get; set; } = 1;

    public int pageSize
    {
        get => _pageSize;
        set => _pageSize = (value > MAX_PAGE_SIZE) ? MAX_PAGE_SIZE : value;
    }
}