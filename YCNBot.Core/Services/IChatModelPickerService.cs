namespace YCNBot.Core.Services
{
    public interface IChatModelPickerService
    {
        IChatCompletionService? GetModel(string model);
    }
}