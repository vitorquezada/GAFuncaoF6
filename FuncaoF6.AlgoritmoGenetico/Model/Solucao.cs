using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FuncaoF6.AlgoritmoGenetico.Model
{
    public class Solucao : IComparable<Solucao>
    {
        public int tamanho_cromossomo;
        public int[] Cromossomo;
        public int ValorMaximo = 100;
        public int ValorMinimo = -100;
        public double x;
        public double y;
        public double Fitness;

        public Solucao(int[] cromossomo)
        {
            Cromossomo = cromossomo;
            tamanho_cromossomo = cromossomo.Length;
            DecodificaCromossomo();
            Fitness = CalcularFitness();
        }

        public double CalcularFitness()
        {
            var fitness = 0.5 - ((Math.Pow(Math.Sin(Math.Sqrt(Math.Pow(x, 2) + Math.Pow(y, 2))), 2) - 0.5) / Math.Pow(1.0 + (0.001 * (Math.Pow(x, 2) + Math.Pow(y, 2))), 2));
            return fitness;
        }

        private void DecodificaCromossomo()
        {
            int numeroDeBits = (tamanho_cromossomo / 2);
            var range = ValorMaximo - ValorMinimo;

            for (int i = numeroDeBits - 1; i >= 0; i--)
            {
                x += Cromossomo[i] * Math.Pow(2, i);
                y += Cromossomo[i + numeroDeBits] * Math.Pow(2, i);
            }

            x = (x * (range / Math.Pow(2, numeroDeBits))) + ValorMinimo;
            y = (y * (range / Math.Pow(2, numeroDeBits))) + ValorMinimo;
        }

        public int Compare(Solucao x, Solucao y)
        {
            var FitnessX = x.Fitness;
            var FitnessY = y.Fitness;
            if (FitnessX == FitnessY)
                return 0;
            return (FitnessX < FitnessY) ? -1 : 1;
        }

        public override string ToString()
        {
            String str = String.Format("X: {0}\tY: {1}\tFitness: {2}", x, y, Fitness);
            return str;
        }

        public int CompareTo(Solucao other)
        {
            if (this.Fitness == other.Fitness)
                return 0;
            return (this.Fitness < other.Fitness) ? -1 : 1;
        }
    }
}
