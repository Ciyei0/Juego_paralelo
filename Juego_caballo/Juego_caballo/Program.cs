using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


class Program
{
    static async Task Main(string[] args)
    {
        Console.WriteLine("**** RIMG Sports ****");
        Console.WriteLine("Carrera de Caballo (pulse una tecla para comenzar)");
        Console.ReadKey();


        int NumCaballos = 10;
        int distanciaCarrera = 100;
        int ganadores = 3;

        Random random = new Random();
        var cts = new CancellationTokenSource();


        List<Task<(int numcaballo, int time)>>  caballotask = new List<Task<(int, int)>>();


        for (int i = 0; i < NumCaballos; i++)
        {
            int numcaballo = i;


            var tarea = Task.Run(async () =>
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

                    Console.WriteLine($"Caballo {NumCaballos} ha avanzado {distancia}m ");

                }
            });
        }

    }

}



