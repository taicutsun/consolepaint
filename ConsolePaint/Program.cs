using ConsolePaint.Terminal;

var terminal = 
                    args.Length != 0 
                    ? new Terminal(args[0]) 
                    : new Terminal();

terminal.Run();


