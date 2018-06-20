using System;
using tabuleiro;
using xadrez;

namespace XadrezConsole {
    class Program {
        static void Main(string[] args) {

                Tabuleiro tab = new Tabuleiro(8, 8);

                tab.ColocarPeca(new Torre(tab, Cor.Preto), new Posicao(0, 0));
                tab.ColocarPeca(new Torre(tab, Cor.Preto), new Posicao(0, 7));
                tab.ColocarPeca(new Rei(tab, Cor.Preto), new Posicao(0, 3));
                tab.ColocarPeca(new Torre(tab, Cor.Branco), new Posicao(7, 0));
                tab.ColocarPeca(new Torre(tab, Cor.Branco), new Posicao(7, 7));
                tab.ColocarPeca(new Rei(tab, Cor.Branco), new Posicao(7, 4));

            Tela.ImprimirTabuleiro(tab);

            Console.WriteLine("  a b c d e f g h");

        }


    }
}
