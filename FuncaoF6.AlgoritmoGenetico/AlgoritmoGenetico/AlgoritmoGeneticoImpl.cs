using FuncaoF6.AlgoritmoGenetico.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FuncaoF6.AlgoritmoGenetico.AlgoritmoGenetico
{
    public class AlgoritmoGeneticoImpl
    {
        public Random r = new Random();
        public int POPULACAO_TAMANHO = 100;
        public int CROMOSSOMO_TAMANHO = 44;
        public int NUMERO_GERACOES = 100;
        public double TAXA_MUTACAO = 0.005;
        public double TAXA_REPRODUCAO = 0.65;

        public List<Solucao> Populacao = new List<Solucao>();
        public List<Solucao> Descendentes = new List<Solucao>();


        public void IniciaPopulacao()
        {
            while (Populacao.Count < POPULACAO_TAMANHO)
            {
                int[] cromossomo = new int[CROMOSSOMO_TAMANHO];
                for (int i = 0; i < CROMOSSOMO_TAMANHO; i++)
                    cromossomo[i] = r.Next(2);
                Populacao.Add(new Solucao(cromossomo));
            }
        }

        public void RealizaCruzamento()
        {
            int numeroDeCruzamentos = (int)((POPULACAO_TAMANHO / 2) * TAXA_REPRODUCAO) * 2;
            double SomatorioFitness = Populacao.Sum(x => x.Fitness);

            for (int i = 0; i < numeroDeCruzamentos; i += 2)
            {
                int P1 = SelecionarRoleta(SomatorioFitness);
                int P2 = SelecionarRoleta(SomatorioFitness);

                Solucao F1 = null;
                Solucao F2 = null;

                Cruzar(Populacao[P1], Populacao[P2], ref F1, ref F2);
                Descendentes.Add(F1);
                Descendentes.Add(F2);
            }
        }

        public void RealizaMutacao()
        {
            for (int i = 0; i < Descendentes.Count; i++)
                for (int j = 0; j < CROMOSSOMO_TAMANHO; j++)
                {
                    double random = r.NextDouble();
                    if (random <= TAXA_MUTACAO)
                        Descendentes[i].Cromossomo[j] = Descendentes[i].Cromossomo[j] == 0 ? 1 : 0;
                }
        }

        public void ProximaGeracao()
        {
            if (Descendentes.Count > 0)
            {
                Descendentes.ToList().ForEach(x => Populacao.Add(x));

                Solucao melhorSolucao = Populacao[0];

                Populacao.ForEach(x =>
                {
                    if (melhorSolucao.Fitness < x.Fitness)
                        melhorSolucao = x;
                });

                Descendentes.Clear();
                Populacao.Clear();

                Populacao.Add(melhorSolucao);
            }
            IniciaPopulacao();
        }

        private void Cruzar(Solucao P1, Solucao P2, ref Solucao F1, ref Solucao F2)
        {
            int tamanhoVariavelCromossomo = CROMOSSOMO_TAMANHO / 2; // 22
            int indexX = r.Next(tamanhoVariavelCromossomo); // 0 <= indexX < 21
            int indexY = r.Next(tamanhoVariavelCromossomo); // 22 <= IndexY < 43
            indexY += tamanhoVariavelCromossomo;

            int[] cromossomo_F1 = new int[CROMOSSOMO_TAMANHO];
            int[] cromossomo_F2 = new int[CROMOSSOMO_TAMANHO];


            for (int i1 = 0, i2 = tamanhoVariavelCromossomo; i1 < tamanhoVariavelCromossomo; i1++, i2++)
            {
                cromossomo_F1[i1] = (i1 < indexX) ? P1.Cromossomo[i1] : P2.Cromossomo[i1];
                cromossomo_F2[i1] = (i1 < indexX) ? P2.Cromossomo[i1] : P1.Cromossomo[i1];

                cromossomo_F1[i2] = (i2 < indexY) ? P1.Cromossomo[i2] : P2.Cromossomo[i2];
                cromossomo_F2[i2] = (i2 < indexY) ? P2.Cromossomo[i2] : P1.Cromossomo[i2];
            }

            F1 = new Solucao(cromossomo_F1);
            F2 = new Solucao(cromossomo_F2);
        }

        private int SelecionarRoleta(double somatorioFitness)
        {
            int index = 0;
            double random = r.NextDouble() * somatorioFitness;
            double acumulado = 0.0;

            for (int i = 0; i < Populacao.Count; i++)
            {
                acumulado += Populacao[i].Fitness;
                if (random < acumulado)
                {
                    index = i;
                    break;
                }
            }
            return index;
        }
    }
}
