using System;
using System.Collections.Generic;
using tabuleiro;
using xadrez;

namespace XadrezConsole {
    class Tela {

        public static void ImprimirPartida(PartidaXadrez partida) {
            ImprimirTabuleiro(partida.Tab);
            Console.WriteLine();
            ImprimirPecaCapturadas(partida);
            Console.WriteLine();
            Console.WriteLine("Turno: " + partida.Turno);
            Console.WriteLine("Aguardando jogada: " + partida.JogadorAtual);
            if (partida.EXeque) {
                Console.Write("XEQUE!!");
            }
        }

        public static void ImprimirPecaCapturadas(PartidaXadrez partida) {
            Console.WriteLine("Peças capturadas: ");
            Console.Write("Brancas: ");
            ImprimirConjunto(partida.PecaCapturadas(Cor.Branco));
            Console.WriteLine();
            Console.Write("Pretas: ");
            ConsoleColor aux = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Yellow;
            ImprimirConjunto(partida.PecaCapturadas(Cor.Preto));
            Console.ForegroundColor = aux;
            Console.WriteLine();
        }

        public static void ImprimirConjunto(HashSet<Peca> conjunto) {
            Console.Write("[");
            foreach (Peca  x in conjunto) {
                Console.Write(x + " ");
            }
            Console.Write("]");
        }

        public static void ImprimirTabuleiro(Tabuleiro tab) {

            for (int i = 0; i < tab.Linhas; i++) {
                Console.Write(8 - i + " ");
                for (int j = 0; j < tab.Colunas; j++) {
                    ImprimirPeca(tab.Peca(i, j));
                }
                Console.WriteLine();
            }
            Console.WriteLine("  a b c d e f g h");
        }
        public static void ImprimirTabuleiro(Tabuleiro tab, bool[,] PossicoesPossiveis) {
            ConsoleColor FundoOriginal = Console.BackgroundColor;
            ConsoleColor fundoAlterado = ConsoleColor.DarkGray;

            for (int i = 0; i < tab.Linhas; i++) {
                Console.Write(8 - i + " ");
                for (int j = 0; j < tab.Colunas; j++) {
                    if (PossicoesPossiveis[i,j]) {
                        Console.BackgroundColor = fundoAlterado;
                    }
                    else {
                        Console.BackgroundColor = FundoOriginal;
                    }
                    ImprimirPeca(tab.Peca(i, j));
                    Console.BackgroundColor = FundoOriginal;
                }
                Console.WriteLine();
            }
            Console.BackgroundColor = FundoOriginal;
            Console.WriteLine("  a b c d e f g h");
        }

        public static PosicaoXadrez LerPosicaoXadez() {
            string s = Console.ReadLine();
            char coluna = s[0];
            int linha = int.Parse(s[1] + "");
            return new PosicaoXadrez(coluna, linha);

        }

        public static void ImprimirPeca(Peca peca) {

            if (peca == null) {
                Console.Write("- ");
            }
            else { 
                if (peca.Cor == Cor.Branco) { 
                    Console.Write(peca);
                }
                else {
                    ConsoleColor aux = Console.ForegroundColor;
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.Write(peca);
                    Console.ForegroundColor = aux;
                }
                Console.Write(" ");
            }
        }
    }
}

