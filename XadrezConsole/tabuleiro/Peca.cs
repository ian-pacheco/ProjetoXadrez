namespace tabuleiro {
    abstract class Peca {
        public Posicao Posicao { get; set; }
        public Cor  Cor { get; protected set; }
        public int QntMovimento { get; protected set; }
        public Tabuleiro Tab { get; protected set; }

        public Peca(Tabuleiro tab, Cor cor) {
            Posicao = null;
            this.Tab = tab;
            this.Cor = cor;
            QntMovimento = 0;
        }

        public void Movimento() {
            QntMovimento++;
        }
        public void DesMovimento() {
            QntMovimento--;
        }

        public bool ExisteMovPossivel() {
            bool[,] mat = MovimentosPossiveis();
            for (int i =0; i< Tab.Linhas; i++) {
                for (int j = 0; j < Tab.Colunas; j++) {
                    if (mat[i,j] ) {
                        return true;
                    }
                }
            }
            return false;
        }

        public bool PermiteMover(Posicao pos) {
            return MovimentosPossiveis()[pos.Linha, pos.Coluna];
        }

        public abstract bool[,] MovimentosPossiveis();
    }
}
