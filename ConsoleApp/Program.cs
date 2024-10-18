var start_time = System.Diagnostics.Stopwatch.GetTimestamp();

var solver = new A2023.Problem22.Solver();
var result = solver.RunA(@"..\2023\A2023.Problem22\Data\input.txt");

Console.WriteLine(result);

Console.WriteLine($"Time: {System.Diagnostics.Stopwatch.GetElapsedTime(start_time)}");

//25.2s -> 6.2
