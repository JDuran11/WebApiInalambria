using WebApiInalambria.DTOs.NumToText;

namespace WebApiInalambria.Ports
{
    public interface INumToTextRepositoryPort
    {
        string GetMessage();
        NumToTextDTO NumberToWords(NumDTO number);
    }
}
