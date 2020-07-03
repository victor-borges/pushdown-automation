using PushdownAutomation.Resources;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using System.Text;

namespace PushdownAutomation
{
    public class Program
    {
        private static async Task Main(string[] args)
        {
            Console.InputEncoding = Encoding.UTF8;
            Console.OutputEncoding = Encoding.UTF8;

            var filePaths = new List<string>();

            if (args.Length == 0)
            {
                Console.WriteLine(Messages.UsageHelp);
                return;
            }

            foreach (var arg in args)
            {
                if (!File.Exists(arg))
                {
                    Console.WriteLine(string.Format(CultureInfo.CurrentCulture, Messages.FileNotFound, arg));
                    continue;
                }

                filePaths.Add(arg);
            }

            var automataSettings = new List<AutomataSettings>();
            
            foreach (var filePath in filePaths)
            {
                using var fileStream = File.OpenRead(filePath);
                automataSettings.Add(await JsonSerializer.DeserializeAsync<AutomataSettings>(fileStream).ConfigureAwait(false));
            }

            Console.WriteLine(string.Format(CultureInfo.CurrentCulture, Messages.ParsedAutomatas, automataSettings.Count));

            var untestableAutomatasCount =
                (from setting in automataSettings
                 where setting.PushdownAutomata == null ||
                       setting.PushdownAutomata.States == null ||
                       setting.PushdownAutomata.InitialState == null ||
                       setting.Inputs == null
                 select setting).Count();

            if (untestableAutomatasCount != 0)
                Console.WriteLine(string.Format(CultureInfo.CurrentCulture, Messages.UntestableAutomatas, untestableAutomatasCount));

            var testableAutomataSettings =
                from setting in automataSettings
                where setting.PushdownAutomata != null &&
                      setting.PushdownAutomata.States != null &&
                      setting.PushdownAutomata.InitialState != null &&
                      setting.Inputs != null
                select setting;

            foreach (var setting in testableAutomataSettings)
            {
                Console.WriteLine();
                Console.WriteLine($"Name: {setting.PushdownAutomata?.Name}");
                Console.WriteLine($"Description: {setting.PushdownAutomata?.Description}");
                Console.WriteLine();

                if (setting.Inputs == null)
                    continue;

                foreach (var input in setting.Inputs)
                {
                    var automata = new DeterministicPushdownAutomata(
                        setting.PushdownAutomata?.InputAlphabet,
                        setting.PushdownAutomata?.StackAlphabet,
                        setting.PushdownAutomata?.States ?? new HashSet<int>(),
                        setting.PushdownAutomata?.InitialState ?? default,
                        setting.PushdownAutomata?.TransitionRules,
                        setting.PushdownAutomata?.InitialStackSymbol);

                    var encodedInput = EncodeString(input);

                    if (encodedInput?.Length > 25)
                    {
                        Console.Write($"{string.Join("", encodedInput.Take(25).ToArray()) + "...",-30}");
                    }
                    else
                    {
                        Console.Write($"{encodedInput,-30}");
                    }

                    if (automata.Matches(input))
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine(Messages.StringAccepted);
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine(Messages.StringRejected);
                    }

                    Console.ResetColor();
                }
            }
        }

        private static string EncodeString(string? original)
        {
            string encodedInput;

            if (original == null)
            {
                encodedInput = "<null>";
            }
            else if (original.Length == 0)
            {
                encodedInput = "<empty>";
            }
            else
            {
                encodedInput = string.Join("", original.Select(c => char.IsControl(c) ? $"\\{(int)c:000}" : c.ToString()).ToArray());
            }

            return encodedInput;
        }
    }
}
