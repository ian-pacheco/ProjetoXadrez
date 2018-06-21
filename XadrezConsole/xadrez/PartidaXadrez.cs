using System;
using tabuleiro;

namespace xadrez {
    class PartidaXadrez { 
        public Tabuleiro Tab { get; private set; }
        public int Turno { get; private set; }
        public Cor JogadorAtual { get; private set; }
        public bool Terminada { get; private set; }

        public PartidaXadrez() {
            Tab = new Tabuleiro(8, 8);
            Turno = 1;
            JogadorAtual = Cor.Branco;
            ColocarPecas();
            Terminada = false;
        }

        public void RealizaJogada(Posicao origem, Posicao destino) {
            ExecutaMovimento(origem, destino);
            Turno++;
            MudaJogador();
        }

        public void ValidarPosicaoOrigem(Posicao pos) {
            if(Tab.Peca(pos) == null) {
                throw new TabuleiroException("Não existe peça na posição escolhida!");
            }
            if (JogadorAtual != Tab.Peca(pos).Cor) {
                throw new TabuleiroException("A peça não é sua!!");
            }
            if (!Tab.Peca(pos).ExisteMovPossivel()) {
                throw new TabuleiroException("Não há movimentos possíveis para esta peça!!");
            }
        }

        public void ValidarPosDestino(Posicao origem, Posicao destino) {
            if(!Tab.Peca(origem).PermiteMover(destino)) {
                throw new TabuleiroException("Posição destino inválida!!");
            } 
        }

        private void MudaJogador() {
            if (JogadorAtual == Cor.Branco) {
                JogadorAtual = Cor.Preto;
            }
            else {
                JogadorAtual = Cor.Branco;
            }
        }

        public void ExecutaMovimento(Posicao origem, Posicao destino) {
            Peca p = Tab.RetirarPeca(origem);
            p.Movimento();
            Peca PecaCapturada = Tab.RetirarPeca(destino);
            Tab.ColocarPeca(p, destino);

        }

        private void ColocarPecas() {
            Tab.ColocarPeca(new Torre(Tab, Cor.Preto), new PosicaoXadrez('e', 8).ToPosicao());
            Tab.ColocarPeca(new Torre(Tab, Cor.Preto), new PosicaoXadrez('e', 7).ToPosicao());
            Tab.ColocarPeca(new Torre(Tab, Cor.Preto), new PosicaoXadrez('c', 7).ToPosicao());
            Tab.ColocarPeca(new Torre(Tab, Cor.Preto), new PosicaoXadrez('c', 8).ToPosicao());
            Tab.ColocarPeca(new Torre(Tab, Cor.Preto), new PosicaoXadrez('d', 7).ToPosicao());
            Tab.ColocarPeca(new Rei(Tab, Cor.Preto), new Posicao(0, 3));
            Tab.ColocarPeca(new Torre(Tab, Cor.Branco), new Posicao(7, 0));
            Tab.ColocarPeca(new Torre(Tab, Cor.Branco), new Posicao(7, 7));
            Tab.ColocarPeca(new Rei(Tab, Cor.Branco), new Posicao(7, 4));
        }



    }
}
