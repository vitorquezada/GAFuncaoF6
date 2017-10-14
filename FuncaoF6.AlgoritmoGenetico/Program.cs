using FuncaoF6.AlgoritmoGenetico.AlgoritmoGenetico;
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
            string[,] resultados = new string[numeroDeExecucoes, 7];



            for (int i = 0; i < numeroDeExecucoes; i++)
            {
                Console.Clear();
                Console.Write($"Execução numero {i + 1}/{numeroDeExecucoes}");

                AlgoritmoGeneticoImpl ga = new AlgoritmoGeneticoImpl();

                ga.IniciaPopulacao();
                ga.Populacao = ga.Populacao.OrderByDescending(x => x.Fitness).ToList();

                int geracoesComResultadoIgual = 0;
                double melhorFitness = 0;

                for (int j = 0; j < ga.NUMERO_GERACOES; j++)
                {
                    melhorFitness = ga.Populacao.First().Fitness;
                    //Console.WriteLine("Geração: " + (j + 1));
                    //Console.WriteLine("Melhor: " + ga.Populacao.First().ToString());
                    //Console.WriteLine("Pior: " + ga.Populacao.Last().ToString());
                    ga.RealizaCruzamento();
                    ga.RealizaMutacao();
                    ga.SelecionaParaProximaGeracao();

                    // Tratamento para retirar convergencia local
                    if (melhorFitness == ga.Populacao.First().Fitness)
                        geracoesComResultadoIgual++;
                    else
                        geracoesComResultadoIgual = 0;

                    if (geracoesComResultadoIgual == 6)
                    {
                        ga.IniciaPopulacao();
                        geracoesComResultadoIgual = 0;
                    }

                }


                resultados[i, 0] = ga.POPULACAO_TAMANHO.ToString();
                resultados[i, 1] = ga.NUMERO_GERACOES.ToString();
                resultados[i, 2] = (100 * ga.TAXA_MUTACAO).ToString();
                resultados[i, 3] = (100 * ga.TAXA_REPRODUCAO).ToString();
                resultados[i, 4] = ga.Populacao.First().x.ToString("0.0000");
                resultados[i, 5] = ga.Populacao.First().y.ToString("0.0000");
                resultados[i, 6] = ga.Populacao.First().Fitness.ToString("0.0000");
            }


            //Dá saída em um arquivo
            var nomeArquivo = $"ResultadoGA_{DateTime.Now.ToString("yyyyMMdd-HHmmffff")}.txt";
            var caminhoArquivo = System.AppDomain.CurrentDomain.BaseDirectory.ToString() + nomeArquivo;

            if (!System.IO.File.Exists(caminhoArquivo))
                using (System.IO.File.Create(caminhoArquivo)) { }

            using (System.IO.StreamWriter file = new System.IO.StreamWriter(caminhoArquivo, true))
            {
                file.WriteLine("-------------------------- Resultado Execução --------------------------\n\n");

                file.WriteLine(String.Format("Fitness|Execução|Tamanho da População|Numero de Gerações|Taxa de Mutação|Taxa de Crossover|X      |Y"));
                for (int i = 0; i < numeroDeExecucoes; i++)
                    file.WriteLine(String.Format("{7}|{0}|{1}|{2}|{3}|{4}|{5}|{6}",
                        (i + 1).ToString().PadRight(8),
                        resultados[i, 0].PadRight(20),
                        resultados[i, 1].PadRight(18),
                        (resultados[i, 2] + "%").PadRight(15),
                        (resultados[i, 3] + "%").PadRight(17),
                        resultados[i, 4].PadLeft(7),
                        resultados[i, 5].PadLeft(7),
                        resultados[i, 6].PadLeft(7)));

                file.Write("\n\n------------------------------------------------------------------------");

                
            }

            System.Diagnostics.Process.Start(caminhoArquivo);
            Console.ReadKey();
            System.IO.File.Delete(caminhoArquivo);

        }

    }
}
