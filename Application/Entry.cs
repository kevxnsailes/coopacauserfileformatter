
namespace MyNamespace
{
    public class Muki
    {
        public static void Muka()
        {
            string[] args = new string[5];

            if (File.Exists("Coopaca-Migration-File.txt"))
            {
                File.Delete("Coopaca-Migration-File.txt");
            }

            List<string> clientLines = new();
            var sep = '\t';

            if (args.Length == 2)
            {
                List<string> cuLines = new();

                using (StreamReader sr = new(args[0]))
                {
                    while (!sr.EndOfStream) cuLines.Add(sr.ReadLine());
                }

                List<string> ccLines = new();

                using (StreamReader sr = new(args[1]))
                {
                    while (!sr.EndOfStream) ccLines.Add(sr.ReadLine());
                }

                foreach (var line in cuLines)
                {
                    var lineSplitByTab = line.Split(sep);

                    if (double.TryParse(lineSplitByTab[0], out _))
                    {
                        var nameSplit = lineSplitByTab[1].Split(' ');
                        nameSplit = nameSplit.Where(x => !string.IsNullOrEmpty(x)).ToArray();

                        if (nameSplit.Length >= 2)
                        {
                            string appender = string.Empty;
                            int count = 0;

                            foreach (var item in nameSplit)
                            {
                                if (count > 0) appender = $"{appender} {item}";
                                count++;
                            }

                            foreach (var item in ccLines)
                            {
                                var ccLinesSplit = item.Split(sep);

                                if (ccLinesSplit[0].Trim() == lineSplitByTab[0].Trim())
                                {
                                    clientLines.Add($"CU|{lineSplitByTab[0].Trim()}|{nameSplit[0].Trim()}|{appender.Trim()}|{nameSplit[0].Trim()}.{lineSplitByTab[0].Trim()}@ezstatementliteview.com");
                                }
                            }
                        }
                        else Console.WriteLine($"Problem getting name: {line}");
                    }
                    else Console.WriteLine($"Problem getting customer number: {line}");
                }

                var distinctLines = clientLines.Distinct().ToList();

                foreach (var line in distinctLines)
                {
                    var lineSplit = line.Split('|');

                    List<string> ccFinalLines = new();

                    foreach (var item in ccLines)
                    {
                        var ccLinesSplit = item.Split(sep);

                        if (ccLinesSplit[0].Trim() == lineSplit[1].Trim())
                        {
                            ccFinalLines.Add($"CR|{lineSplit[1].Trim()}|{ccLinesSplit[1]}|001");
                        }
                    }

                    if (ccFinalLines.Any())
                    {
                        using StreamWriter sw = new("Coopaca-Migration-File.txt", true);

                        sw.WriteLine(line);

                        foreach (var item in ccFinalLines)
                        {
                            sw.WriteLine(item);
                        }
                    }
                    else Console.WriteLine("No valid lines were found to create file.");
                }
            }
            else Console.WriteLine("The two expected file paths were not found");
        }
    }
}