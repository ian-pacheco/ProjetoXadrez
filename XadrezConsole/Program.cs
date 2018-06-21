using System;
using tabuleiro;
using xadrez;

namespace XadrezConsole {
    class Program {
        static void Main(string[] args) {

            try {
                PartidaXadrez partida = new PartidaXadrez();

                while (!partida.Terminada) {

                    try {
                        Console.Clear();
                        Tela.ImprimirTabuleiro(partida.Tab);
                        Console.WriteLine();
                        Console.WriteLine("Turno: " + partida.Turno);
                        Console.WriteLine("Aguardando jogada: " + partida.JogadorAtual);

                        Console.WriteLine();
                        Console.Write("Origem: ");
                        Posicao origem = Tela.LerPosicaoXadez().ToPosicao();
                        partida.ValidarPosicaoOrigem(origem);

                        bool[,] PosicoesPossiveis = partida.Tab.Peca(origem).MovimentosPossiveis();


                        Console.Clear();
                        Tela.ImprimirTabuleiro(partida.Tab, PosicoesPossiveis);

                        Console.WriteLine();
                        Console.Write("Destino: ");
                        Posicao destino = Tela.LerPosicaoXadez().ToPosicao();
                        partida.ValidarPosDestino(origem, destino);

                        partida.RealizaJogada(origem, destino);
                    }
                    catch (TabuleiroException e) {
                        Console.WriteLine(e.Message);
                        Console.ReadLine();                        
                    }
                }

                Tela.ImprimirTabuleiro(partida.Tab);
            }

            catch (TabuleiroException e) {
                Console.WriteLine(e.Message);
            }

            

        }


    }
}
