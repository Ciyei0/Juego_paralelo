using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

class Program
{
    static async Task Main(string[] args)
    {
        Console.WriteLine("**** RIMG Sports ****");
        Console.WriteLine();
        Console.WriteLine("Carrera de Caballos (pulse una tecla para comenzar)");
        Console.ReadKey();

        Console.WriteLine("Elija un caballo del (0 a 9):");
        string jugada = Console.ReadLine();
        int jugadaInt;

       
        while (!int.TryParse(jugada, out jugadaInt) || jugadaInt < 0 || jugadaInt >= 10)
        {
            Console.WriteLine("Tine que ser un numero del 0 al 9:");
            jugada = Console.ReadLine();
        }

        int numCaballos = 10;
        int distanciaCarrera = 100;
        int ganadores = 3;

        Random random = new Random();
        var cts = new CancellationTokenSource();

       
        List<(int num_caballo, int time)> ganador = new List<(int, int)>();
        List<Task> c_tareas = new List<Task>();

        for (int i = 0; i < numCaballos; i++)
        {
            int num_caballo = i;

      
            var ctarea = Task.Factory.StartNew(async () =>
            {
                int distancia = 0;
                int totaltime = 0;

                while (distancia < distanciaCarrera)
                {
                    int step = random.Next(10, 20); 
                    int stepTime = random.Next(200, 500);
                    await Task.Delay(stepTime, cts.Token);
                    distancia += step;
                    totaltime += stepTime;

                    Console.WriteLine($"Caballo {num_caballo} ha avanzado {distancia}m");
                }

                lock (ganador)
                {
                    if (ganador.Count < ganadores)
                    {
                        ganador.Add((num_caballo, totaltime));
                        Console.WriteLine($"Caballo {num_caballo} ha terminado la carrera {totaltime}");

                        if (ganador.Count == ganadores)
                        {
                            cts.Cancel();
                        }
                    }
                }
                return num_caballo; 

            }, cts.Token, TaskCreationOptions.AttachedToParent, TaskScheduler.Default).Unwrap();

          
            ctarea.ContinueWith(t =>
            {
                Console.WriteLine($"Caballo {t.Result} ha terminado");
            }, TaskContinuationOptions.OnlyOnRanToCompletion);

           
            ctarea.ContinueWith(t =>
            {
                Console.WriteLine($"Caballo {num_caballo} ha perdido");
            }, TaskContinuationOptions.OnlyOnCanceled);

            c_tareas.Add(ctarea);
        }

        
        Task.Factory.ContinueWhenAny(c_tareas.ToArray(), completedTask =>
        {
            Console.WriteLine();
        });


        var firstCompleted = await Task.WhenAny(c_tareas);
        Console.WriteLine("CAballo ");

        // Esperar a que todas las tareas terminen
        try
        {
            await Task.WhenAll(c_tareas);
        }
        catch (OperationCanceledException)
        {
            Console.WriteLine();
            Console.WriteLine("******* Carrera Terminada *******");
        }

        Console.WriteLine();
        Console.WriteLine("******* Ganadores *******");

        // Practicar esta parte de seleccionar los primero que llegaron
        ganador = ganador.OrderBy(x => x.time).ToList();
        for (int i = 0; i < ganador.Count; i++)
        {
            Console.WriteLine($"Puesto {i + 1}: Caballo {ganador[i].num_caballo} en {ganador[i].time}");
        }

        Console.WriteLine();

        
        if (jugadaInt == ganador[0].num_caballo)
        {
            Console.WriteLine($"Felicidades, El caballo {jugadaInt} ha ganado la carrera");
        }
        else
        {
            Console.WriteLine($"Lo siento, el caballo {jugadaInt} no ha ganado");
        }
    }
}
