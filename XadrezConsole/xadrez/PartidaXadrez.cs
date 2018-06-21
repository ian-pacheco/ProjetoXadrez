using System.Collections.Generic;
using tabuleiro;

namespace xadrez {
    class PartidaXadrez { 
        public Tabuleiro Tab { get; private set; }
        public int Turno { get; private set; }
        public Cor JogadorAtual { get; private set; }
        public bool Terminada { get; private set; }
        private HashSet<Peca> Minha;
        private HashSet<Peca> Capturada;


        public PartidaXadrez() {
            Tab = new Tabuleiro(8, 8);
            Turno = 1;
            JogadorAtual = Cor.Branco;
            Minha = new HashSet<Peca>();
            Capturada = new HashSet<Peca>();
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

        public HashSet<Peca> PecaCapturadas(Cor cor) {
            HashSet<Peca> aux = new HashSet<Peca>();
            foreach (Peca x in Capturada) {
                if (x.Cor == cor) {
                    aux.Add(x);
                }
            }
            return aux;
        }

        public HashSet<Peca> PecasEmJogo(Cor cor) {
            HashSet<Peca> aux = new HashSet<Peca>();
            foreach (Peca x in Minha) {
                if (x.Cor == cor) {
                    aux.Add(x);
                }
            }
            aux.ExceptWith(PecaCapturadas(cor));
            return aux;
        }


        public void ExecutaMovimento(Posicao origem, Posicao destino) {
            Peca p = Tab.RetirarPeca(origem);
            p.Movimento();
            Peca PecaCapturada = Tab.RetirarPeca(destino);
            Tab.ColocarPeca(p, destino);
            if (PecaCapturada != null) {
                Capturada.Add(PecaCapturada);
            }

        }

        public void ColocarNovaPeca(char coluna, int linha, Peca peca) {
            Tab.ColocarPeca(peca, new PosicaoXadrez(coluna, linha).ToPosicao());
            Minha.Add(peca);

        }

        private void ColocarPecas() {
            ColocarNovaPeca('e', 8, new Torre(Tab, Cor.Preto));
            ColocarNovaPeca('e', 7, new Torre(Tab, Cor.Preto));
            ColocarNovaPeca('c', 7, new Torre(Tab, Cor.Preto));
            ColocarNovaPeca('c', 8, new Torre(Tab, Cor.Preto));
            ColocarNovaPeca('d', 7, new Torre(Tab, Cor.Preto));
            ColocarNovaPeca('d', 8, new Rei(Tab, Cor.Preto));
            ColocarNovaPeca('f', 1, new Torre(Tab, Cor.Branco));
            ColocarNovaPeca('f', 2, new Torre(Tab, Cor.Branco));
            ColocarNovaPeca('d', 1, new Torre(Tab, Cor.Branco));
            ColocarNovaPeca('d', 2, new Torre(Tab, Cor.Branco));
            ColocarNovaPeca('e', 2, new Torre(Tab, Cor.Branco));
            ColocarNovaPeca('e', 1, new Rei(Tab, Cor.Branco));
        }



    }
}
