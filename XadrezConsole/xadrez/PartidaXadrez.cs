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
        public bool EXeque { get; private set; }

        public PartidaXadrez() {
            Tab = new Tabuleiro(8, 8);
            Turno = 1;
            JogadorAtual = Cor.Branco;
            Minha = new HashSet<Peca>();
            Capturada = new HashSet<Peca>();
            ColocarPecas();
            Terminada = false;
            EXeque = false;
        }

        public Peca ExecutaMovimento(Posicao origem, Posicao destino) {
            Peca p = Tab.RetirarPeca(origem);
            p.Movimento();
            Peca PecaCapturada = Tab.RetirarPeca(destino);
            Tab.ColocarPeca(p, destino);
            if (PecaCapturada != null) {
                Capturada.Add(PecaCapturada);
            }
            return PecaCapturada;
        }

        public void DesfazMovimento(Posicao origem, Posicao destino, Peca pecaCapturada) {
            Peca p = Tab.RetirarPeca(destino);
            p.DesMovimento();
            if (pecaCapturada != null) {
                Tab.ColocarPeca(pecaCapturada, destino);
                Capturada.Remove(pecaCapturada);
            }
            Tab.ColocarPeca(p, origem);
        }

        public void RealizaJogada(Posicao origem, Posicao destino) {
            Peca PecaCapturada = ExecutaMovimento(origem, destino);
            if (Xeque(JogadorAtual)) {
                DesfazMovimento(origem, destino, PecaCapturada);
                throw new TabuleiroException("Você não pode se colocar em Xeque!!");
            }
            if (Xeque(Adversaria(JogadorAtual))) {
                EXeque = true;
            }
            else {
                EXeque = false;
            }
            if (XequeMate(Adversaria(JogadorAtual))) {
                Terminada = true;
            }
            else {
                Turno++;
                MudaJogador();
            }
        }

        public void ValidarPosicaoOrigem(Posicao pos) {
            if (Tab.Peca(pos) == null) {
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
            if (!Tab.Peca(origem).PermiteMover(destino)) {
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

        private Cor Adversaria(Cor cor) {
            if (cor == Cor.Branco) {
                return Cor.Preto;
            }
            else {
                return Cor.Branco;
            }
        }

        private Peca Rei(Cor cor) {
            foreach (Peca x in PecasEmJogo(cor)) {
                if (x is Rei) {
                    return x;
                }
            }
            return null;
        }

        public bool Xeque(Cor cor) {
            Peca R = Rei(cor);
            if (R == null) {
                throw new TabuleiroException("Não tem rei da cor " + cor + " no tabuleiro!!");
            }

            foreach (Peca x in PecasEmJogo(Adversaria(cor))) {
                bool[,] mat = x.MovimentosPossiveis();
                if (mat[R.Posicao.Linha, R.Posicao.Coluna]) {
                    return true;
                }
            }
            return false;
        }

        public bool XequeMate(Cor cor) {
            if (!Xeque(cor)) {
                return false;
            }
            foreach (Peca x in PecasEmJogo(cor)) {
                bool[,] mat = x.MovimentosPossiveis();
                for (int i = 0; i<Tab.Linhas; i++) {
                    for (int j = 0; j <Tab.Colunas; j++) {
                        if (mat[i,j]) {
                            Posicao origem = x.Posicao;
                            Posicao destino = new Posicao(i, j);
                            Peca pecaCapturada = ExecutaMovimento(origem, destino);
                            bool testeXeque = Xeque(cor);
                            DesfazMovimento(origem, destino, pecaCapturada);
                            if(!testeXeque) {
                                return false;
                            }
                        }
                    }
                }
            }
            return true;

        }

        public void ColocarNovaPeca(char coluna, int linha, Peca peca) {
            Tab.ColocarPeca(peca, new PosicaoXadrez(coluna, linha).ToPosicao());
            Minha.Add(peca);
        }

        private void ColocarPecas() {
            // ColocarNovaPeca('e', 8, new Torre(Tab, Cor.Preto));
            //ColocarNovaPeca('e', 7, new Torre(Tab, Cor.Preto));
            //ColocarNovaPeca('c', 7, new Torre(Tab, Cor.Preto));
            //ColocarNovaPeca('c', 8, new Torre(Tab, Cor.Preto));
            ColocarNovaPeca('b', 8, new Torre(Tab, Cor.Preto));
            ColocarNovaPeca('a', 8, new Rei(Tab, Cor.Preto));
            ColocarNovaPeca('d', 1, new Torre(Tab, Cor.Branco));
            ColocarNovaPeca('h', 7, new Torre(Tab, Cor.Branco));
            //ColocarNovaPeca('d', 1, new Torre(Tab, Cor.Branco));
            //ColocarNovaPeca('d', 2, new Torre(Tab, Cor.Branco));
            //ColocarNovaPeca('e', 2, new Torre(Tab, Cor.Branco));
            ColocarNovaPeca('e', 1, new Rei(Tab, Cor.Branco));
        }
    }
}
