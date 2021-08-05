namespace SapCo2.Samples.Core.Abstracts
{
    public interface IPrintable<in T> where T: class, new()
    {
        void Print(T model);
    }
}
