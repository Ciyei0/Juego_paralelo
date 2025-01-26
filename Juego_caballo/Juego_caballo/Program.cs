using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


class Program
{
    static async Task Main(string[] args)
    {
        Console.WriteLine("**** RIMG Sports ****");
        Console.WriteLine();
        Console.WriteLine("Carrera de Caballo (pulse una tecla para comenzar)");
        Console.ReadKey();


        Console.WriteLine("Elija el numero de tu caballo:");
        string jugada = Console.ReadLine();

        int NumCaballos = 10;
        int distanciaCarrera = 100;
        int ganadores = 3;

        Random random = new Random();
        var cts = new CancellationTokenSource();


        List<(int num_caballo, int time)> ganador = new List<(int num_caballo, int time)>();

        List<Task> c_tareas = new List<Task>();


        for (int i = 0; i < NumCaballos; i++)
        {
            int num_caballo = i;


            var ctarea = Task.Run(async () =>
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

                    Console.WriteLine($"Caballo {num_caballo} ha avanzado {distancia}m ");


                }

                lock (ganador)
                {
                    if (ganador.Count < ganadores)
                    {
                        ganador.Add((num_caballo, totaltime));
                        Console.WriteLine($"Caballo {num_caballo} ha terminado la carrera en {totaltime}");

                        if (ganador.Count == ganadores)
                        {
                            cts.Cancel();
                        }
                    }
                }
            }, cts.Token);



            c_tareas.Add(ctarea);
        }
        try
        {
            await Task.WhenAll(c_tareas);
        }

        catch
        {
            Console.WriteLine();
            Console.WriteLine("******* Carrera Terminada *******");

        }

    
        Console.WriteLine();
        Console.WriteLine("******* Ganadores *******");


        //Seleccion de puesto realizado por iA
        ganador = ganador.OrderBy(x => x.time).ToList();
        for (int i = 0; i < ganador.Count; i++)
        {
            Console.WriteLine($"Puesto {i + 1} : Caballo {ganador[i].num_caballo} en {ganador[i].time}ms");
            Console.WriteLine();

            //Aprender OrderBy y OrderByDescending para ordenar la lista de ganadores

        }

        //Seleccioanr el caballo ganador

        int jugadaInt = Convert.ToInt32(jugada);
        if (jugadaInt == ganador[0].num_caballo)
        {
            Console.WriteLine($"Felicidades, el caballo: {jugada} ha ganado ");
        }
        else
        {
            Console.WriteLine($"Lo siento el caballo {jugada} a perdido");
        }


    }

}



