using tabuleiro;

namespace xadrez {
    class Rei : Peca {

        private PartidaXadrez Partida;

        public Rei(Tabuleiro tab, Cor cor, PartidaXadrez partida) : base(tab, cor) {
            Partida = partida;
        }
        public override string ToString() {
            return "R";
        }

        private bool PodeMover(Posicao pos) {
            Peca p = Tab.Peca(pos);
            return p == null || p.Cor != Cor;
        }

        private bool TesteTorreRoque(Posicao pos) {
            Peca p = Tab.Peca(pos);
            return p != null && p is Torre && p.Cor == Cor && p.QntMovimento == 0;
        }

        public override bool[,] MovimentosPossiveis() {
            bool[,] mat = new bool[Tab.Linhas, Tab.Colunas];

            Posicao pos = new Posicao(0, 0);

            //n
            pos.DefinirValores(Posicao.Linha - 1, Posicao.Coluna);
            if (Tab.PosicaoValida(pos) && PodeMover(pos)) {
                mat[pos.Linha, pos.Coluna] = true;
            }

            //ne
            pos.DefinirValores(Posicao.Linha - 1, Posicao.Coluna + 1);
            if (Tab.PosicaoValida(pos) && PodeMover(pos)) {
                mat[pos.Linha, pos.Coluna] = true;
            }

            //e
            pos.DefinirValores(Posicao.Linha, Posicao.Coluna + 1);
            if (Tab.PosicaoValida(pos) && PodeMover(pos)) {
                mat[pos.Linha, pos.Coluna] = true;
            }

            //se
            pos.DefinirValores(Posicao.Linha + 1, Posicao.Coluna + 1);
            if (Tab.PosicaoValida(pos) && PodeMover(pos)) {
                mat[pos.Linha, pos.Coluna] = true;
            }

            //s
            pos.DefinirValores(Posicao.Linha +1 , Posicao.Coluna);
            if (Tab.PosicaoValida(pos) && PodeMover(pos)) {
                mat[pos.Linha, pos.Coluna] = true;
            }

            //sw
            pos.DefinirValores(Posicao.Linha + 1, Posicao.Coluna -1);
            if (Tab.PosicaoValida(pos) && PodeMover(pos)) {
                mat[pos.Linha, pos.Coluna] = true;
            }

            //w
            pos.DefinirValores(Posicao.Linha, Posicao.Coluna - 1);
            if (Tab.PosicaoValida(pos) && PodeMover(pos)) {
                mat[pos.Linha, pos.Coluna] = true;
            }

            //nw
            pos.DefinirValores(Posicao.Linha - 1, Posicao.Coluna - 1);
            if (Tab.PosicaoValida(pos) && PodeMover(pos)) {
                mat[pos.Linha, pos.Coluna] = true;
            }

            //#Especial Roque
            if(QntMovimento == 0 && !Partida.EXeque) {
                //#Roque Pequeno
                Posicao PosT1 = new Posicao(Posicao.Linha, Posicao.Coluna + 3);
                if (TesteTorreRoque(PosT1)) {
                    Posicao p1 = new Posicao(Posicao.Linha, Posicao.Coluna + 1);
                    Posicao p2 = new Posicao(Posicao.Linha, Posicao.Coluna + 2);
                    if (Tab.Peca(p1) == null && Tab.Peca(p2) == null) {
                        mat[Posicao.Linha, Posicao.Coluna + 2] = true;
                    }
                }
                //#Roque Grande
                Posicao PosT2 = new Posicao(Posicao.Linha, Posicao.Coluna - 4);
                if (TesteTorreRoque(PosT2)) {
                    Posicao p1 = new Posicao(Posicao.Linha, Posicao.Coluna - 1);
                    Posicao p2 = new Posicao(Posicao.Linha, Posicao.Coluna - 2);
                    Posicao p3 = new Posicao(Posicao.Linha, Posicao.Coluna - 3);
                    if (Tab.Peca(p1) == null && Tab.Peca(p2) == null && Tab.Peca(p3) == null) {
                        mat[Posicao.Linha, Posicao.Coluna - 2] = true;
                    }
                }
            }
            return mat;
        }
    }
}
