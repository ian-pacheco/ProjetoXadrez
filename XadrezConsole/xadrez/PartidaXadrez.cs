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
        public Peca VulneravelEnPassant { get; private set; }


        public PartidaXadrez() {
            Tab = new Tabuleiro(8, 8);
            Turno = 1;
            JogadorAtual = Cor.Branco;
            Minha = new HashSet<Peca>();
            Capturada = new HashSet<Peca>();
            ColocarPecas();
            Terminada = false;
            EXeque = false;
            VulneravelEnPassant = null;
        }

        public Peca ExecutaMovimento(Posicao origem, Posicao destino) {
            Peca p = Tab.RetirarPeca(origem);
            p.Movimento();
            Peca PecaCapturada = Tab.RetirarPeca(destino);
            Tab.ColocarPeca(p, destino);
            if (PecaCapturada != null) {
                Capturada.Add(PecaCapturada);
            }

            //#Roque Pequeno
            if (p is Rei && destino.Coluna == origem.Coluna + 2) {
                Posicao origemT = new Posicao(origem.Linha, origem.Coluna + 3);
                Posicao destinoT = new Posicao(origem.Linha, origem.Coluna + 1);
                Peca T = Tab.RetirarPeca(origemT);
                T.Movimento();
                Tab.ColocarPeca(T, destinoT);
            }

            //#Roque Grande
            if (p is Rei && destino.Coluna == origem.Coluna - 2) {
                Posicao origemT = new Posicao(origem.Linha, origem.Coluna - 4);
                Posicao destinoT = new Posicao(origem.Linha, origem.Coluna - 1);
                Peca T = Tab.RetirarPeca(origemT);
                T.Movimento();
                Tab.ColocarPeca(T, destinoT);
            }

            //#EnPassant
            if (p is Peao) {
                if (origem.Coluna != destino.Coluna && PecaCapturada == null) {
                    Posicao posP;
                    if(p.Cor == Cor.Branco) {
                        posP = new Posicao(destino.Linha + 1, destino.Coluna);
                    }
                    else {
                        posP = new Posicao(destino.Linha - 1, destino.Coluna);
                    }
                    PecaCapturada = Tab.RetirarPeca(posP);
                    Capturada.Add(PecaCapturada);
                }
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

            //#Roque Pequeno
            if (p is Rei && destino.Coluna == origem.Coluna + 2) {
                Posicao origemT = new Posicao(origem.Linha, origem.Coluna + 3);
                Posicao destinoT = new Posicao(origem.Linha, origem.Coluna + 1);
                Peca T = Tab.RetirarPeca(destinoT);
                T.DesMovimento();
                Tab.ColocarPeca(T, origemT);
            }
            //#Roque Grande
            if (p is Rei && destino.Coluna == origem.Coluna - 2) {
                Posicao origemT = new Posicao(origem.Linha, origem.Coluna - 4);
                Posicao destinoT = new Posicao(origem.Linha, origem.Coluna - 1);
                Peca T = Tab.RetirarPeca(destinoT);
                T.DesMovimento();
                Tab.ColocarPeca(T, origemT);
            }

            //#EnPassant
            if (p is Peao) {
                if (origem.Coluna != destino.Coluna && pecaCapturada == VulneravelEnPassant) {
                    Peca peao = Tab.RetirarPeca(destino);
                    Posicao posP;
                    if(p.Cor == Cor.Branco) {
                        posP = new Posicao(3, destino.Coluna);
                    }
                    else {
                        posP = new Posicao(4, destino.Coluna);
                    }
                    Tab.ColocarPeca(peao, posP);
                }
            }
        }

        public void RealizaJogada(Posicao origem, Posicao destino) {
            Peca PecaCapturada = ExecutaMovimento(origem, destino);
            if (Xeque(JogadorAtual)) {
                DesfazMovimento(origem, destino, PecaCapturada);
                throw new TabuleiroException("Você não pode se colocar em Xeque!!");
            }

            Peca p = Tab.Peca(destino);

            //Promocao
            if(p is Peao) {
                if((p.Cor == Cor.Branco && destino.Linha == 0) || (p.Cor == Cor.Preto && destino.Linha == 7)){
                    p = Tab.RetirarPeca(destino);
                    Minha.Remove(p);
                    Peca dama = new Dama(Tab, p.Cor);
                    Tab.ColocarPeca(dama, destino);
                    Minha.Add(dama);
                }
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

            //#EnPassant
            if (p is Peao && (destino.Linha == origem.Linha - 2 || destino.Linha == origem.Linha + 2)) {
                VulneravelEnPassant = p;
            }
            else {
                VulneravelEnPassant = null;
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
            if (!Tab.Peca(origem).MovimentoPossivel(destino)) {
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
                for (int i = 0; i < Tab.Linhas; i++) {
                    for (int j = 0; j < Tab.Colunas; j++) {
                        if (mat[i, j]) {
                            Posicao origem = x.Posicao;
                            Posicao destino = new Posicao(i, j);
                            Peca pecaCapturada = ExecutaMovimento(origem, destino);
                            bool testeXeque = Xeque(cor);
                            DesfazMovimento(origem, destino, pecaCapturada);
                            if (!testeXeque) {
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
            ColocarNovaPeca('a', 8, new Torre(Tab, Cor.Preto));
            ColocarNovaPeca('b', 8, new Cavalo(Tab, Cor.Preto));
            ColocarNovaPeca('c', 8, new Bispo(Tab, Cor.Preto));
            ColocarNovaPeca('d', 8, new Dama(Tab, Cor.Preto));
            ColocarNovaPeca('e', 8, new Rei(Tab, Cor.Preto, this));
            ColocarNovaPeca('f', 8, new Bispo(Tab, Cor.Preto));
            ColocarNovaPeca('g', 8, new Cavalo(Tab, Cor.Preto));
            ColocarNovaPeca('h', 8, new Torre(Tab, Cor.Preto));
            ColocarNovaPeca('a', 7, new Peao(Tab, Cor.Preto, this));
            ColocarNovaPeca('b', 7, new Peao(Tab, Cor.Preto, this));
            ColocarNovaPeca('c', 7, new Peao(Tab, Cor.Preto, this));
            ColocarNovaPeca('d', 7, new Peao(Tab, Cor.Preto, this));
            ColocarNovaPeca('e', 7, new Peao(Tab, Cor.Preto, this));
            ColocarNovaPeca('f', 7, new Peao(Tab, Cor.Preto, this));
            ColocarNovaPeca('g', 7, new Peao(Tab, Cor.Preto, this));
            ColocarNovaPeca('h', 7, new Peao(Tab, Cor.Preto, this));

            ColocarNovaPeca('a', 1, new Torre(Tab, Cor.Branco));
            ColocarNovaPeca('b', 1, new Cavalo(Tab, Cor.Branco));
            ColocarNovaPeca('c', 1, new Bispo(Tab, Cor.Branco));
            ColocarNovaPeca('d', 1, new Dama(Tab, Cor.Branco));
            ColocarNovaPeca('e', 1, new Rei(Tab, Cor.Branco, this));
            ColocarNovaPeca('f', 1, new Bispo(Tab, Cor.Branco));
            ColocarNovaPeca('g', 1, new Cavalo(Tab, Cor.Branco));
            ColocarNovaPeca('h', 1, new Torre(Tab, Cor.Branco));
            ColocarNovaPeca('a', 2, new Peao(Tab, Cor.Branco, this));
            ColocarNovaPeca('b', 2, new Peao(Tab, Cor.Branco, this));
            ColocarNovaPeca('c', 2, new Peao(Tab, Cor.Branco, this));
            ColocarNovaPeca('d', 2, new Peao(Tab, Cor.Branco, this));
            ColocarNovaPeca('e', 2, new Peao(Tab, Cor.Branco, this));
            ColocarNovaPeca('f', 2, new Peao(Tab, Cor.Branco, this));
            ColocarNovaPeca('g', 2, new Peao(Tab, Cor.Branco, this));
            ColocarNovaPeca('h', 2, new Peao(Tab, Cor.Branco, this));
        }
    }
}
