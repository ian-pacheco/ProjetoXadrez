﻿namespace Tauleiro {
    class Posicao {
        public int Linha { get; set; }
        public int Coluna { get; set; }

        public Posicao(int linha, int coluna) {
            this.Coluna = coluna;
            this.Linha = linha;
        }

        public override string ToString() {
            return Linha + ", " + Coluna;
        }

    }
}
