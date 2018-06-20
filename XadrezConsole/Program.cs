using System;
using tabuleiro;
using xadrez;

namespace XadrezConsole {
    class Program {
        static void Main(string[] args) {

            try {
                Tabuleiro tab = new Tabuleiro(8, 8);

                tab.ColocarPeca(new Torre(tab, Cor.Preto), new Posicao(0, 0));
                tab.ColocarPeca(new Torre(tab, Cor.Preto), new Posicao(1, 7));
                tab.ColocarPeca(new Rei(tab, Cor.Preto), new Posicao(0, 5));

                Tela.ImprimirTabuleiro(tab);
            }

            catch (TabuleiroException e) {
                Console.WriteLine(e.Message);
            }
        }
    }
}
