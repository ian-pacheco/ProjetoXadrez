using System;
using tabuleiro;

namespace xadrez {
    class PartidaXadrez { 
        public Tabuleiro Tab { get; private set; }
        private int Turno;
        private Cor JogadorAtual;
        public bool Terminada { get; private set; }

        public PartidaXadrez() {
            Tab = new Tabuleiro(8, 8);
            Turno = 1;
            JogadorAtual = Cor.Branco;
            ColocarPecas();
            Terminada = false;
        }

        public void ExecutaMovimento(Posicao origem, Posicao destino) {
            Peca p = Tab.RetirarPeca(origem);
            p.Movimento();
            Peca PecaCapturada = Tab.RetirarPeca(destino);
            Tab.ColocarPeca(p, destino);

        }

        private void ColocarPecas() {
            Tab.ColocarPeca(new Torre(Tab, Cor.Preto), new PosicaoXadrez('a', 8).ToPosicao());
            Tab.ColocarPeca(new Torre(Tab, Cor.Preto), new Posicao(0, 7));
            Tab.ColocarPeca(new Rei(Tab, Cor.Preto), new Posicao(0, 3));
            Tab.ColocarPeca(new Torre(Tab, Cor.Branco), new Posicao(7, 0));
            Tab.ColocarPeca(new Torre(Tab, Cor.Branco), new Posicao(7, 7));
            Tab.ColocarPeca(new Rei(Tab, Cor.Branco), new Posicao(7, 4));
        }



    }
}
