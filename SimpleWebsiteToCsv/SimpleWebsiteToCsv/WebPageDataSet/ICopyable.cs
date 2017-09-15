namespace SimpleWebsiteToCsv.WebPageDataSet
{
    public interface ICopyable<T> where T : new()
    {
        void CopyFrom(T objectFrom);
    }
}
