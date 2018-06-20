namespace tabuleiro {
    class Peca {
        public Posicao Posicao { get; set; }
        public Cor  Cor { get; protected set; }
        public int QntMovimento { get; protected set; }
        public Tabuleiro Tab { get; protected set; }

        public Peca(Posicao posicao, Tabuleiro tab, Cor cor) {
            this.Posicao = posicao;
            this.Tab = tab;
            this.Cor = cor;
            QntMovimento = 0;   
        }
    }
}
