namespace YCNBot.Core.Services
{
    public interface ICaseLawDetectionService
    {
        bool CheckContainsCaseLaw(string text);
    }
}