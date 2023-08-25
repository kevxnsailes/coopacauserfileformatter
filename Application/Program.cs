using Application;

if (File.Exists("Coopaca-Migration-File.txt"))
{
    File.Delete("Coopaca-Migration-File.txt");
}

string path = @"D:\_repositories\_evertec.projects\esta\app.ezview\merchants\4.2. Finastra - Coopaca - 636\End User Management Report2022.csv";

List<string> clientLines = new();

List<string> fullLines = new();
List<string> cuLines = new();
List<string> ccLines = new();

var sep = ',';
var pipe = '|';

///---------------------------------------

string path1 = @"D:\_repositories\_evertec.projects\esta\app.ezview\merchants\4.2. Finastra - Coopaca - 636\!user_creation\3rd Full User Creation\Coopaca-Migration-File.txt";

using (StreamReader sr = new(path1))
{
    while (!sr.EndOfStream) fullLines.Add(sr.ReadLine());
}

foreach (var line in fullLines)
{
    var fullLineSplitByTab = line.Split(pipe);

    var oldEmail = fullLineSplitByTab[4].Split('.');

    var newEmail = $"{oldEmail[1]}{oldEmail[2]}";

    clientLines.Add($"{fullLineSplitByTab[0]}|{fullLineSplitByTab[1]}|{fullLineSplitByTab[2]}|{fullLineSplitByTab[3]}|{newEmail}");
}

using (StreamWriter sw1 = new("Coopaca-Migration-File.txt", true))
{
    foreach (var item in clientLines)
    {
        sw1.WriteLine(item);
    }
}

///---------------------------------------

if (args.Length == 0)
{
    //using (StreamReader sr = new(args[0]))
    using (StreamReader sr = new(path))
    {
        while (!sr.EndOfStream) fullLines.Add(sr.ReadLine());
    }

    foreach (var line in fullLines)
    {
        var fullLineSplitByTab = line.Split(sep);

        //Is AcctNumber|UserId Empty
        if (string.IsNullOrEmpty(fullLineSplitByTab[0]) is true)
        {
            Console.WriteLine($"ERR1 - {line}");
            continue;
        }

        //Is First Name Empty
        if (string.IsNullOrEmpty(fullLineSplitByTab[3]) is true)
        {
            Console.WriteLine($"ERR2 - {line}");
            continue;
        }

        //Is Last Name Empty
        if (string.IsNullOrEmpty(fullLineSplitByTab[4]) is true)
        {
            Console.WriteLine($"ERR3 - {line}");
            continue;
        }

        var finalSplit = fullLineSplitByTab[0].Split(pipe);

        if (finalSplit.Length != 2)
        {
            Console.WriteLine($"ERR4 - {line}");
            continue;
        }

        //Is AcctNumber Empty
        if (string.IsNullOrEmpty(finalSplit[0]) is true)
        {
            Console.WriteLine($"ERR5 - {line}");
            continue;
        }

        //Is User Id Empty
        if (string.IsNullOrEmpty(finalSplit[1]) is true)
        {
            Console.WriteLine($"ERR6 - {line}");
            continue;
        }

        ApplicationModel applicationModel = new()
        {
            AccountNumber = finalSplit[0].Trim(),
            ExternalId = finalSplit[1].Trim(),
            FirstName = fullLineSplitByTab[3].Trim(),
            LastName = fullLineSplitByTab[4].Trim(),
            EmailUsername = $"{fullLineSplitByTab[3].Split(' ')[0].Trim()}.{DateTime.Now:HHmmssfffffff}"
        };

        if (double.TryParse(applicationModel.AccountNumber, out _))
        {
            List<string> f = clientLines.Where(x => x.Contains($"CU|{applicationModel.ExternalId}|")).ToList();

            if (f.Any())
            {
                Console.WriteLine("\nINF1:");
                foreach (var item in f) Console.WriteLine(item);
                Console.WriteLine($"{line} \n");
            }
            else clientLines.Add($"{applicationModel.CUOperation}|{applicationModel.ExternalId}|{applicationModel.FirstName}" +
                $"|{applicationModel.LastName}|{applicationModel.EmailUsername}@ezstatementliteview.com");

            clientLines.Add($"{applicationModel.CROperation}|{applicationModel.ExternalId}|{applicationModel.AccountNumber}" +
                $"|{applicationModel.ProductId}");
        }
        else Console.WriteLine($"ERR7: {line}");
    }

    var distinctLines = clientLines.Distinct().OrderBy(x => x).ToList();

    //foreach (var item in distinctLines)
    //{
    //    clientLines.Add(item);

    //    var cuSplit = item.Split(pipe);

    //    List<string> f = ccLines.Where(x => x.Contains($"CR|{cuSplit[1]}|")).ToList();

    //    foreach (var itemCc in f)
    //    {
    //        clientLines.Add(itemCc);
    //    }
    //}

    if (distinctLines.Any())
    {
        using StreamWriter sw = new("Coopaca-Migration-File.txt", true);

        foreach (var item in distinctLines)
        {
            sw.WriteLine(item);
        }
    }
    else Console.WriteLine("No valid lines were found to create file.");

}
else Console.WriteLine("No file found in the request");