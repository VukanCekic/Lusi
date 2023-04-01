using System.Text;
using Lusio.Lexer;

public class Lusi
{
    static bool _hadError = false;

    public static void Main(string[] args)
    {
        
        switch (args.Length)
        {
            case > 1:
                Environment.Exit(64);
                break;
            case  1:
                // If you start  from the command line and give  a path to a file, reads the file and executes it.
                runFile(args[0]);
                break;
            default:
                // Fire up without arguments, prompt where you can enter and execute code one line at a time.
                runPrompt();
                break;
        }
    }

    private static void runFile(string filePath)
    {
        var bytes = File.ReadAllBytes(filePath);
        run(Encoding.Default.GetString(bytes));
        // it’s good engineering practice to separate the code that generates the errors from the code that reports them.
        if (_hadError) Environment.Exit(64);

    }

    private static void runPrompt()
    {
        Console.Clear();
        for (;;)
        {
            Console.Write("lusi>");
            string? line = Console.ReadLine();

            if (line == null)
            {
                break;
            }
            run(line);
            _hadError = false;
        }

    }
    private static async void run(string source) {
        Scanner scanner = new Scanner(source);
        List<Token> tokens = scanner.scanTokens();

        // For now, just print the tokens.
        foreach (Token token in tokens)
        {
            Console.WriteLine(token);
        }
        
    }
    
    public static void error(int line, string message) {
        report(line, "", message);
    }

    private static void report(int line, string where,
        string message) {
        Console.Error.WriteLine(
            "[line " + line + "] Error" + where + ": " + message);
        _hadError = true;
    }

    
}