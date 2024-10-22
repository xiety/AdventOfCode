var start_time = System.Diagnostics.Stopwatch.GetTimestamp();

var solver = new A2021.Problem23.Solver();
var result = solver.RunA(@"..\2021\A2021.Problem23\Data\sampleA.txt");

//var solver = new A2021.Problem22.Solver();
//var result = solver.RunB(@"..\2021\A2021.Problem22\Data\input.txt");

Console.WriteLine(result);

Console.WriteLine($"Time: {System.Diagnostics.Stopwatch.GetElapsedTime(start_time)}");
