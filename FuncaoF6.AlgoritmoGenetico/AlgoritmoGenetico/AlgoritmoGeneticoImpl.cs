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
            if(Populacao.Count > 0)
            {
                Populacao.RemoveRange(1, Populacao.Count-1);
            }

            for (int i = Populacao.Count; i < POPULACAO_TAMANHO; i++)
            {
                int[] cromossomo = new int[CROMOSSOMO_TAMANHO];
                for (int j = 0; j < CROMOSSOMO_TAMANHO; j++)
                    cromossomo[j] = r.Next(2);
                Populacao.Add(new Solucao(cromossomo));
            }
        }

        public void RealizaCruzamento()
        {
            int numeroDeCruzamentos = (int)((POPULACAO_TAMANHO / 2) * TAXA_REPRODUCAO) * 2;
            double SomatorioFitness = 0;


            foreach (Solucao x in Populacao)
                SomatorioFitness += x.Fitness;

            for (int i = 0; i < numeroDeCruzamentos; i += 2)
            {
                int P1 = SelecionarRoleta(SomatorioFitness);
                int P2 = SelecionarRoleta(SomatorioFitness);

                Cruzar(Populacao[P1], Populacao[P2], out Solucao F1, out Solucao F2);
                Descendentes.Add(F1);
                Descendentes.Add(F2);
            }
        }

        public void RealizaMutacao()
        {
            foreach (Solucao item in Descendentes)
                for (int i = 0; i < CROMOSSOMO_TAMANHO; i++)
                {
                    double random = r.NextDouble();
                    if (random <= TAXA_MUTACAO)
                        item.Cromossomo[i] = item.Cromossomo[i] == 0 ? 1 : 0;
                }
        }

        public void SelecionaParaProximaGeracao()
        {
            foreach (Solucao item in Descendentes)
                Populacao.Add(item);

            Populacao = Populacao.OrderByDescending(x => x.Fitness).ToList();

            for (int i = Populacao.Count - 1; i >= POPULACAO_TAMANHO; i--)
                Populacao.RemoveAt(i);

            Descendentes.Clear();
        }

        private void Cruzar(Solucao P1, Solucao P2, out Solucao F1, out Solucao F2)
        {
            int tamanhoVariavelCromossomo = CROMOSSOMO_TAMANHO / 2; // 22
            int indexX = r.Next(tamanhoVariavelCromossomo); // 0 <= indexX < 21
            int indexY = tamanhoVariavelCromossomo + r.Next(tamanhoVariavelCromossomo); // 22 <= IndexY < 43
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
            int index = Populacao.Count - 1;
            double random = r.NextDouble() * somatorioFitness;
            double acumulado = 0;

            for (int i = 0; i < Populacao.Count; i++)
            {
                acumulado += Populacao[i].Fitness;
                if (random <= acumulado)
                {
                    index = i;
                    break;
                }
            }
            return index;
        }

    }
}
