namespace Application.Interfaces.ICommon
{
    public interface IExcellService
    {
        MemoryStream ExportList<T>(IReadOnlyList<T> list, string sheetName);
    }
}
