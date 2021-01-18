using System;
using System.Threading;

namespace Recetas.Cap04
{
    public class FibonacciMultithread
    {
        private long numero;
        private long fibonacci;
        private ManualResetEvent calculoCompleto;

        public long Numero
        {
            get
            {
                return numero;
            }
        }

        public long Fibonacci
        {
            get
            {
                return numero;
            }
        }

        public FibonacciMultithread(int numero, ManualResetEvent calculoCompleto)
        {
            this.numero = numero;
            this.calculoCompleto = calculoCompleto;
        }

        public void ControlSegundoPlano(Object info)
        {
            int indiceThread = (int)info;

            Console.WriteLine("La ejecución del hilo no. {0} ha empezado.", indiceThread);

            fibonacci = CalcularFibonacci(numero);

            Console.WriteLine("La ejecución del hilo no. {0} ha terminado.", indiceThread);

            calculoCompleto.Set();
        }
        public long CalcularFibonacci(long num)
        {
            if (num <= 1)
            {
                return num;
            }
            else
            {
                return CalcularFibonacci(num - 1) + CalcularFibonacci(num - 2);
            }
        }
    }

    public sealed class UsoFibonacciMultithread
    {
        public static void Main()
        {
            const int seriesCalcular = 10;

            ManualResetEvent[] calculoCompletos = new ManualResetEvent[seriesCalcular];

            FibonacciMultithread[] fibonaccis = new FibonacciMultithread[seriesCalcular];

            Random aleatorio = new Random();

            Console.WriteLine("Se está iniciando la inicialización de {0} tareas...", seriesCalcular);
            for (int i = 0; i < seriesCalcular; ++i)
            {
                calculoCompletos[i] = new ManualResetEvent(false);
                FibonacciMultithread fm = new FibonacciMultithread(aleatorio.Next(20, 40), calculoCompletos[i]);

                fibonaccis[i] = fm;

                ThreadPool.QueueUserWorkItem(fm.ControlSegundoPlano, i);
            }

            WaitHandle.WaitAll(calculoCompletos);
            Console.WriteLine("Todos los calculos de la serie fibonacci se han completado.");

            for (int i = 0; i < fibonaccis.Length; ++i)
            {
                Console.WriteLine("Fibonacci({0}) = {1}", fibonaccis[i].Numero.ToString(), fibonaccis[i].Fibonacci.ToString());
            }

            Console.WriteLine();
        }
    }
}