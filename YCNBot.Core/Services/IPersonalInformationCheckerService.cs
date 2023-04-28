namespace YCNBot.Core.Services
{
    public interface IPersonalInformationCheckerService
    {
        bool CheckIfStringHasNames(string text);
    }
}