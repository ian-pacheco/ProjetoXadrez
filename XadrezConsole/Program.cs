using System;
using tabuleiro;
using xadrez;

namespace XadrezConsole {
    class Program {
        static void Main(string[] args) {

            try {
                PartidaXadrez partida = new PartidaXadrez();

                while (!partida.Terminada) {

                    Console.Clear();
                    Tela.ImprimirTabuleiro(partida.Tab);

                    Console.WriteLine();
                    Console.Write("Origem: ");
                    Posicao origem = Tela.LerPosicaoXadez().ToPosicao();

                    bool[,] PosicoesPossiveis = partida.Tab.Peca(origem).MovimentosPossiveis();


                    Console.Clear();
                    Tela.ImprimirTabuleiro(partida.Tab, PosicoesPossiveis);

                    Console.Write("Destino: ");
                    Posicao destino = Tela.LerPosicaoXadez().ToPosicao();
                    partida.ExecutaMovimento(origem, destino);


                }

                Tela.ImprimirTabuleiro(partida.Tab);
            }

            catch (TabuleiroException e) {
                Console.WriteLine(e.Message);
            }

            

        }


    }
}
