namespace tabuleiro {
    class Peca {
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

    }
}
