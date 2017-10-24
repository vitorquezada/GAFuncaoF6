using FuncaoF6.AlgoritmoGenetico.AlgoritmoGenetico;
using FuncaoF6.AlgoritmoGenetico.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace FuncaoF6.AlgoritmoGenetico
{
    class Program
    {
        public static void Main(string[] args)
        {
            int numeroDeExecucoes = 50;

            for (int i = 0; i < numeroDeExecucoes; i++)
            {
                AlgoritmoGeneticoImpl ga = new AlgoritmoGeneticoImpl();

                for (int j = 0; j < ga.NUMERO_GERACOES; j++)
                {
                    ga.ProximaGeracao();
                    ga.RealizaCruzamento();
                    ga.RealizaMutacao();
                }

                /*
                 * Trecho de código para imprimir solução da execução
                 * 
                 * **/
                Solucao melhorSolucao = ga.Populacao[0];

                ga.Populacao.ForEach(x =>
                {
                    if (melhorSolucao.Fitness < x.Fitness)
                        melhorSolucao = x;
                });
                Console.WriteLine(String.Format("Execução {0}: {1}", i + 1, melhorSolucao.ToString()));
            }

            Console.ReadKey();
        }

    }
}
