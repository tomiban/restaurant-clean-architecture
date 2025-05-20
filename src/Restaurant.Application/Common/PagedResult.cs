namespace Restaurant.Application.Common;

public class PagedResult<T>
{
    public PagedResult(IEnumerable<T> items, int totalCount, int? pageSize, int? pageNumber)
    {
        Items = items;
        TotalItemsCount = totalCount;
        TotalPages =
            (int)Math.Ceiling(totalCount / (double)pageSize); // calculo la cantidad de pag redondeando para arriba
        ItemsFrom = (int)(pageSize * (pageNumber - 1) + 1); //el primer elemento de la pagina
        ItemsTo = Math.Min((int)(ItemsFrom + pageSize - 1),
            TotalItemsCount); // ultimo elemento de la pagina. el mat min es en el caso de que en la ultima pagina haya menos elementos que el size
    }

    public IEnumerable<T> Items { get; set; }
    public int TotalPages { get; set; }
    public int TotalItemsCount { get; set; }
    public int ItemsFrom { get; set; }
    public int ItemsTo { get; set; }
}