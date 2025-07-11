
using System;

namespace KENGAYTIR
{
    class Program
    {
        // 1) Natijalar tarixi
        private static readonly List<string> History = new();

        public static void Main(string[] args)
        {
            System.Console.WriteLine("Kengaytirilgan Kalkulyator (exit || history)");

            string? input;

            // 2) Cmd-Loop

            do
            {
                System.Console.Write("> Nimani hisoblamoqchi bo'lsangiz hisoblang: ");
                input = Console.ReadLine()?.Trim();

                if (string.IsNullOrWhiteSpace(input))
                    continue;

                // 3) Buyruqlarni tekshirish
                if (input.Equals("exit", StringComparison.OrdinalIgnoreCase))
                    break;

                if (input.Equals("history", StringComparison.OrdinalIgnoreCase))
                {
                    ShowHistory();
                    continue;
                }

                // 4) Hisoblash

                if (TryCalculate(input, out decimal result, out string? error))
                {
                    Console.WriteLine(result);
                    History.Add($"{input} = {result}");
                }
                else
                {
                    Console.WriteLine("Error");
                }
            } while (true);

            Console.WriteLine("Chiqildi. Xayr!!");
        }

            // 5) Oddiy yoki ifoda rejimiga qarab hisoblash

            private static bool TryCalculate(string expression, out decimal result, out string? error)
        {

            result = 0;
            error = null;

            var parts = expression.Split(' ', StringSplitOptions.RemoveEmptyEntries);

            // --------------- Oddiy rejim (son amal son)-----

            if (parts.Length == 3 &&
               decimal.TryParse(parts[0], out decimal left) &&
               decimal.TryParse(parts[2], out decimal right))
            {
                var op = parts[1];

                try
                {
                    result = op switch
                    {
                        "+" => Add(left, right),
                        "-" => Subtract(left, right),
                        "*" => Multiply(left, right),
                        "/" => Divide(left, right, out error),
                        _ => throw new InvalidOperationException("Noma'lum amal")
                    };

                    return error == null; //0 - ga bo'lish xatosi bo'lsa false bo'ladi
                }
                catch (Exception ex)
                {
                    error = ex.Message;
                    return false;
                }
            }

            //--------------- Ifoda rejimi (ketma-ket, chapdan o'ngga)------------

            return EvaluateLeftToRight(parts, out result, out error);

        }

        // 6) Chapdan o'ngga oddiy ifoda (3 + 4 * 2 = (3 + 4) = 7,  7 * 2 = 14)

        private static bool EvaluateLeftToRight(string[] tokens, out decimal result, out string? error)
        {
            result = 0;
            error = null;

            if (tokens.Length < 3 || tokens.Length % 2 == 0)
            {
                error = "Format: son amal son [mal son]....";
                return false;
            }


            // Keyingi "amal son" juftliklari

            for (int i = 1; i < tokens.Length; i += 2)
            {
                string op = tokens[i];

                if (!decimal.TryParse(tokens[i + 1], out decimal right))
                {
                    error = $"SOsn emas: {tokens[i + 1]}";
                    return false;
                }

                switch (op)
                {
                    case "+":
                        result = Add(result, right);
                        break;
                   case "-":
                        result = Subtract(result, right);
                        break;
                    case "*":
                        result = Multiply(result, right);
                        break;
                    case "/":
                        result = Divide(result, right, out error);
                        if (error != null) return false;
                        break;
                    default:
                        error = $"Noma'lum amal: {op}";
                        return false;

                }
            }
            return true;
        }
        // 7) Amallarni alohida metodlar

        private static decimal Add(decimal a, decimal b) => a + b;
        private static decimal Subtract(decimal a, decimal b) => a - b;
        private static decimal Multiply(decimal a, decimal b) => a * b;
        private static decimal Divide(decimal a, decimal b, out string? error)
        {
            error = null;
            if (b == 0)
            {
                error = "0 ga bo'lish mumkin emas";
                return 0;
            }

            return a / b;
        }

        // 8) Tarixni chiqarish

        private static void ShowHistory()
        {
            if (History.Count == 0)
            {
                System.Console.WriteLine("Tarix bo'sh");
                return;
            }

            System.Console.WriteLine("--- Tarix----");
            foreach (var entry in History)
            {
                System.Console.WriteLine(entry);
                System.Console.WriteLine("----------------------");
            }

        }
    }
}
