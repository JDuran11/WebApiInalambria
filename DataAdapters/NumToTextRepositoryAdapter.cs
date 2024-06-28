using WebApiInalambria.Ports;
using WebApiInalambria.DTOs.NumToText;

namespace WebApiInalambria.DataAdapters
{
    public class NumToTextRepositoryAdapter : INumToTextRepositoryPort
    {
        public string GetMessage()
        {
            return "hello form data adapter";
        }

        public NumToTextDTO NumberToWords(NumDTO number)
        {
            if (number.number < 0 || number.number > 999999999999)
                throw new ArgumentOutOfRangeException(nameof(number), "El número debe estar entre 0 y 999.999.999.999.");

            if (number.number == 0)
                return new NumToTextDTO
                {
                    number = number.number,
                    text = "cero"
                };

            //En este almaceno las unidades y de paso los numeros de 10 a 19 ya que son casos especiales
            string[] unidades = { "", "un", "dos", "tres", "cuatro", "cinco", "seis", "siete", "ocho", "nueve", "diez", "once", "doce", "trece", "catorce", "quince", "dieciséis", "diecisiete", "dieciocho", "diecinueve" };
            string[] decenas = { "", "", "veinte", "treinta", "cuarenta", "cincuenta", "sesenta", "setenta", "ochenta", "noventa" };
            string[] centenas = { "", "ciento", "doscientos", "trescientos", "cuatrocientos", "quinientos", "seiscientos", "setecientos", "ochocientos", "novecientos" };

            string SpanishWordsForThreeDigitNumber(int n, bool isLastGroup)
            {
                if (n == 100) return "cien";

                string result = "";

                if (n >= 100)
                {
                    result += centenas[n / 100] + " ";
                    n %= 100;
                }
                if (n >= 20)
                {
                    result += decenas[n / 10];
                    if (n % 10 != 0) result += " y " + (isLastGroup && n % 10 == 1 ? "uno" : unidades[n % 10]);
                }
                else if (n > 0)
                {
                    result += isLastGroup && n == 1 ? "uno" : unidades[n];
                }

                return result.Trim();
            }

            string result = "";
            int billions = (int)(number.number / 1000000000);
            int millions = (int)((number.number / 1000000) % 1000);
            int thousands = (int)((number.number / 1000) % 1000);
            int remainder = (int)(number.number % 1000);

            result += (billions > 0 ? SpanishWordsForThreeDigitNumber(billions, false) + (billions == 1 ? " billón " : " billones ") : "")
                    + (millions > 0 ? SpanishWordsForThreeDigitNumber(millions, false) + (millions == 1 ? " millón " : " millones ") : "")
                    + (thousands > 0 ? SpanishWordsForThreeDigitNumber(thousands, false) + " mil " : "")
                    + (remainder > 0 ? SpanishWordsForThreeDigitNumber(remainder, true) : "");

            return new NumToTextDTO
            {
                number = number.number,
                text = result.Trim()
            };

        }


    }
}
