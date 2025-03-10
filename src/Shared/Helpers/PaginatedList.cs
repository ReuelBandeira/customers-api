namespace Api.Shared.Helpers;

public class PaginatedList<T> : List<T>
{
    public int currentPage { get; private set; }
    public int totalPages { get; private set; }
    public int pageSize { get; private set; }
    public int totalCount { get; private set; }

    private PaginatedList(IEnumerable<T> items, int count, int pageNumber, int pageSize)
    {
        totalCount = count;
        currentPage = pageNumber;
        this.pageSize = pageSize;
        totalPages = (int)Math.Ceiling(count / (double)pageSize);

        AddRange(items);
    }

    public static async Task<PaginatedList<T>> CreateAsync(IQueryable<T> source, int pageNumber, int pageSize)
    {
        var count = await source.CountAsync();
        var items = await source
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return new PaginatedList<T>(items, count, pageNumber, pageSize);
    }

    public void AddPaginationHeaders(HttpResponse response)
    {
        response.Headers.Add("X-Total-Count", totalCount.ToString());
        response.Headers.Add("X-Total-Pages", totalPages.ToString());
        response.Headers.Add("X-Current-Page", currentPage.ToString());
        response.Headers.Add("X-Page-Size", pageSize.ToString());
    }
}